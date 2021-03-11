using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Data;
using ERCHTMS.Code;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.Service.BaseManage;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Service.SystemManage;
using ERCHTMS.Entity.EmergencyPlatform;
using System.Data.Common;
using System.Net;
using System.Web;

namespace ERCHTMS.Service.OutsourcingProject
{
    public class PeopleReviewService : RepositoryFactory<PeopleReviewEntity>, PeopleReviewIService
    {
        private IManyPowerCheckService powerCheck = new ManyPowerCheckService();

        private DepartmentService deptservice = new DepartmentService();
        private OutsouringengineerService outsouringengineerservice = new OutsouringengineerService();
        private DepartmentService departmentservice = new DepartmentService();
        private UserService userservice = new UserService();
        private UserInfoService userinfoservice = new UserInfoService();


        public IEnumerable<PeopleReviewEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }

        public PeopleReviewEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        public void RemoveForm(string keyValue)
        {
            var res = DbFactory.Base().BeginTrans();
            try
            {
                Repository<AptitudeinvestigatepeopleEntity> regAd = new Repository<AptitudeinvestigatepeopleEntity>(DbFactory.Base());
                List<AptitudeinvestigatepeopleEntity> list = regAd.FindList(string.Format("select * from epg_aptitudeinvestigatepeople t where t.peoplereviewid='{0}'", keyValue)).ToList();
                if (list.Count > 0)
                {
                    res.Delete<AptitudeinvestigatepeopleEntity>(list);
                }
                res.Delete<PeopleReviewEntity>(keyValue);
                res.Commit();
            }
            catch (Exception ex)
            {

                res.Rollback();
            }
            this.BaseRepository().Delete(keyValue);
        }

        public List<string> SaveForm(string keyValue, PeopleReviewEntity entity)
        {
            var res = DbFactory.Base().BeginTrans();
            entity.ID = keyValue;
            Repository<OutsouringengineerEntity> repEng = new Repository<OutsouringengineerEntity>(DbFactory.Base());
            OutsouringengineerEntity outeng = repEng.FindEntity(entity.OUTENGINEERID);
            
            string flowid = string.Empty;
            try
            {
                List<AptitudeinvestigatepeopleEntity> peoplelist = new List<AptitudeinvestigatepeopleEntity>();
                List<UserEntity> list = new List<UserEntity>();
                List<string> userids = new List<string>();
                if (entity.ISSAVEORCOMMIT == "1")
                {
                    //当提交时候  将人员资质人员信息 SUBMITSTATE设置为1
                    res.ExecuteBySql(string.Format("update epg_aptitudeinvestigatepeople set SUBMITSTATE=1 where PEOPLEREVIEWID='{0}'", keyValue));
                    Operator currUser = OperatorProvider.Provider.Current();
                    string state = string.Empty;
                    ManyPowerCheckEntity nextCheck = CheckAuditPower(currUser, out state, "人员资质审查", entity.OUTENGINEERID);
                    if (!string.IsNullOrEmpty(state) && state == "1")
                    {
                        AptitudeinvestigateauditEntity auditEntity = new AptitudeinvestigateauditEntity();
                        auditEntity.ID = Guid.NewGuid().ToString();
                        auditEntity.APTITUDEID = entity.ID;
                        auditEntity.AUDITDEPT = currUser.DeptName;
                        auditEntity.AUDITDEPTID = currUser.DeptId;
                        auditEntity.AUDITRESULT = "0";
                        auditEntity.AUDITPEOPLEID = currUser.UserId;
                        auditEntity.AUDITPEOPLE = currUser.UserName;
                        auditEntity.AUDITTIME = DateTime.Now;
                        List<ManyPowerCheckEntity> powerList = new ManyPowerCheckService().GetListBySerialNum(currUser.OrganizeCode, "人员资质审查");
                        List<ManyPowerCheckEntity> checkPower = new List<ManyPowerCheckEntity>();
                        //先查出执行部门编码
                        for (int i = 0; i < powerList.Count; i++)
                        {
                            //执行部门
                            if (powerList[i].CHECKDEPTCODE == "-1" || powerList[i].CHECKDEPTID == "-1")
                            {
                                powerList[i].CHECKDEPTCODE = new DepartmentService().GetEntity(outeng.ENGINEERLETDEPTID).EnCode;
                                powerList[i].CHECKDEPTID = new DepartmentService().GetEntity(outeng.ENGINEERLETDEPTID).DepartmentId;
                            }
                            //外包单位
                            if (powerList[i].CHECKDEPTCODE == "-2" || powerList[i].CHECKDEPTID == "-2")
                            {
                                powerList[i].CHECKDEPTCODE = new DepartmentService().GetEntity(outeng.OUTPROJECTID).EnCode;
                                powerList[i].CHECKDEPTID = new DepartmentService().GetEntity(outeng.OUTPROJECTID).DepartmentId;
                            }
                            if (powerList[i].CHECKDEPTCODE == "-6" || powerList[i].CHECKDEPTID == "-6")
                            {
                                if (!string.IsNullOrEmpty(outeng.SupervisorId))
                                {
                                    powerList[i].CHECKDEPTCODE = new DepartmentService().GetEntity(outeng.SupervisorId).EnCode;
                                    powerList[i].CHECKDEPTID = new DepartmentService().GetEntity(outeng.SupervisorId).DepartmentId;
                                }
                                else
                                {
                                    powerList[i].CHECKDEPTCODE = "";
                                    powerList[i].CHECKDEPTID = "";
                                }
                            }
                            
                        }
                        //登录人是否有审核权限--有审核权限直接审核通过
                        for (int i = 0; i < powerList.Count; i++)
                        {

                            if (powerList[i].ApplyType == "0")
                            {
                                if (powerList[i].CHECKDEPTID == currUser.DeptId)
                                {
                                    var rolelist = currUser.RoleName.Split(',');
                                    for (int j = 0; j < rolelist.Length; j++)
                                    {
                                        if (powerList[i].CHECKROLENAME.Contains(rolelist[j]))
                                        {
                                            checkPower.Add(powerList[i]);
                                            break;
                                        }
                                    }
                                }
                            }
                            else if (powerList[i].ApplyType == "1")
                            {
                                var parameter = new List<DbParameter>();
                                //取脚本，获取账户的范围信息
                                if (powerList[i].ScriptCurcontent.Contains("@outengineerid"))
                                {
                                    parameter.Add(DbParameters.CreateDbParameter("@outengineerid", !string.IsNullOrEmpty(entity.OUTENGINEERID) ? entity.OUTENGINEERID : ""));
                                }
                                DbParameter[] arrayparam = parameter.ToArray();
                                var userIds = DbFactory.Base().FindList<UserEntity>(powerList[i].ScriptCurcontent, arrayparam).Cast<UserEntity>().Aggregate("", (current, user) => current + (user.UserId + ",")).Trim(',');
                                if (userIds.Contains(currUser.UserId))
                                {
                                    checkPower.Add(powerList[i]);
                                    //break;
                                }
                            }
                        }
                        if (checkPower.Count > 0)
                        {
                            ManyPowerCheckEntity check = checkPower.Last();//当前

                            for (int i = 0; i < powerList.Count; i++)
                            {
                                if (check.ID == powerList[i].ID)
                                {
                                    flowid = powerList[i].ID;
                                }
                            }
                        }
                        auditEntity.FlowId = flowid;
                        res.Insert<AptitudeinvestigateauditEntity>(auditEntity);
                    }
                    if (nextCheck == null)
                    {
                        entity.ISAUDITOVER = "1";
                        entity.FlowId = flowid;
                        //更新工程流程状态
                        Repository<StartappprocessstatusEntity> proecss = new Repository<StartappprocessstatusEntity>(DbFactory.Base());
                        StartappprocessstatusEntity startProecss = proecss.FindList(string.Format("select * from epg_startappprocessstatus t where t.outengineerid='{0}'", entity.OUTENGINEERID)).ToList().FirstOrDefault();
                        startProecss.PEOPLESTATUS = "1";
                        res.Update<StartappprocessstatusEntity>(startProecss);
                        //人员资质审查审核结束同步人员信息
                        Repository<UserEntity> repuser = new Repository<UserEntity>(DbFactory.Base());
                        Repository<AptitudeinvestigatepeopleEntity> reppeople = new Repository<AptitudeinvestigatepeopleEntity>(DbFactory.Base());
                        peoplelist = reppeople.FindList(string.Format("select * from epg_aptitudeinvestigatepeople t where t.PEOPLEREVIEWID='{0}'",keyValue)).ToList();
                        //List<UserEntity> list = new List<UserEntity>();
                        foreach (var item in peoplelist)
                        {
                            var expression = LinqExtensions.True<UserEntity>();
                            expression = expression.And(t => t.IdentifyID == item.IDENTIFYID && t.Account == item.ACCOUNTS);
                            var user = res.FindList<UserEntity>(expression).FirstOrDefault();
                            if (user == null)
                            {
                                #region 新增同步人员和证件
                                string pwd = "Abc123456";
                                string key = Md5Helper.MD5(CommonHelper.CreateNo(), 16).ToLower();
                                pwd = Md5Helper.MD5(DESEncrypt.Encrypt(Md5Helper.MD5(pwd, 32).ToLower(), key).ToLower(), 32).ToLower();

                                string sql = "";
                                string isldap = GetItemValue("IsOpenPassword");
                                if (isldap == "true")
                                {
                                    pwd = "Abcd1234";
                                    pwd = Md5Helper.MD5(DESEncrypt.Encrypt(Md5Helper.MD5(pwd, 32).ToLower(), key).ToLower(), 32).ToLower();
                                }
                                //同步人员信息
                                sql = string.Format(@"insert into base_user(
                                                        userid,encode,realname,headicon,birthday,mobile,telephone,email,oicq,wechat,
                                                        msn,organizeid,dutyid,dutyname,postid,postname,gender,isspecialequ,isspecial,
                                                        projectid,nation,native,usertype,isepiboly,degreesid,degrees,identifyid,
                                                        organizecode,createuserdeptcode,createuserorgcode,createdate,createuserid,
                                                        departmentid,departmentcode,ISPRESENCE,account,enabledmark,password,secretkey,                   craft,deletemark,entertime,isfourperson,fourpersontype,healthstatus,craftage,ACCOUNTTYPE,ISAPPLICATIONLDAP,QUICKQUERY,MANAGER,DISTRICT,STREET,ADDRESS,AGE,SpecialtyType)
                                               select p.id,p.encode,p.realname,p.headicon,p.birthday,p.mobile,p.telephone,p.email,
                                                        p.oicq,p.wechat,p.msn,p.organizeid,p.dutyid,p.dutyname,p.postid,p.postname,
                                                        p.gender,p.isspecialequ,p.isspecial,p.OUTENGINEERID,p.nation,p.native,p.usertype,
                                                        p.isepiboly,p.degreesid,p.degrees,p.identifyid,p.organizecode,p.createuserdeptcode,
                                                        p.createuserorgcode,p.createdate,p.createuserid,p.outprojectid,p.outprojectcode,'1',
                                                        accounts as account,1,'{1}','{2}',p.workoftype,0,to_date('{3}','yyyy-MM-dd'),
                                                        isfourperson,fourpersontype,stateofhealth,workyear,ACCOUNTTYPE,ISAPPLICATIONLDAP,QUICKQUERY,MANAGER,DISTRICT,STREET,ADDRESS,AGE,SpecialtyType
                                                from EPG_APTITUDEINVESTIGATEPEOPLE p 
                                                    where p.id='{0}' ", item.ID, pwd, key, DateTime.Now.ToString("yyyy-MM-dd"));
                                res.ExecuteBySql(sql);

                                //通过岗位ID获取角色信息
                                sql = string.Format("select t.roleids,t.rolenames from BASE_ROLE t where t.organizeid='{0}' and t.roleid='{1}'", item.ORGANIZEID, item.DUTYID);
                                DataTable dtRoles = BaseRepository().FindTable(sql);

                                if (dtRoles != null && dtRoles.Rows.Count > 0)
                                {
                                    //根据岗位ID更新人员岗位名称
                                    string sqlRole = string.Format(@" update base_user b set b.rolename= '{0}',b.roleid='{2}' where b.dutyid = '{1}'", dtRoles.Rows[0]["rolenames"].ToString(), item.DUTYID, dtRoles.Rows[0]["roleids"].ToString());
                                    res.ExecuteBySql(sqlRole);
                                    string[] arr = dtRoles.Rows[0]["roleids"].ToString().Split(',');
                                    foreach (string roleId in arr)
                                    {
                                        //插入角色人员关系表
                                        sql = string.Format(string.Format(@" insert into BASE_USERRELATION(USERRELATIONID,userid,Objectid,category,sortcode,isdefault) select '{1}' || rownum,p.id,'{2}','2',0 ,1
                                from EPG_APTITUDEINVESTIGATEPEOPLE p where p.id='{0}'", item.ID, Guid.NewGuid().ToString(), roleId));
                                        res.ExecuteBySql(sql);
                                    }
                                }

                                sql = string.Format(string.Format(@"insert into bis_certificate(id,userid,years,certname,certnum,senddate,sendorgan,filepath,certtype,WorkType,WorkItem,ItemNum,ApplyDate,StartDate,Grade,Industry,UserType,Craft,ZGName,EndDate)
                                                                    select c.id,c.userid,c.validttime,c.credentialsname,
                                                                    c.credentialscode,c.credentialstime,c.credentialsorg,
                                                                    c.credentialsfile,certtype,c.WorkType,c.WorkItem,c.ItemNum,c.ApplyDate,c.StartDate,c.Grade,c.Industry,c.UserType,c.Craft,c.ZGName,c.EndDate 
                                                                    from EPG_CERTIFICATEINSPECTORS c where c.userid in (select Id from EPG_APTITUDEINVESTIGATEPEOPLE p 
                                                    where p.id='{0}')", item.ID));
                                if (res.ExecuteBySql(sql) > 0)
                                {
                                    //同步证件照片
                                    sql = string.Format(@"insert into BASE_FILEINFO(fileid,t.folderid,filename,filepath,filesize,t.fileextensions,t.filetype,recid) select fileid || rownum,t.folderid,filename,filepath,filesize,t.fileextensions,t.filetype,recid from BASE_FILEINFO t
where recid in(select id from EPG_CERTIFICATEINSPECTORS where userid in (select Id from EPG_APTITUDEINVESTIGATEPEOPLE p 
                                                    where p.id='{0}'))", item.ID);
                                    res.ExecuteBySql(sql);
                                }
                                userids.Add(item.ID);
                                //list.Add(userservice.GetEntity(item.ID));
                                #endregion
                            }
                            else
                            {
                                #region 修改同步人员和证件
                                string sql = "";
                                //同步人员信息
                                sql = string.Format(@"update base_user set (
                                                        encode,realname,headicon,birthday,mobile,telephone,email,oicq,wechat,
                                                        msn,organizeid,dutyid,dutyname,postid,postname,gender,isspecialequ,isspecial,
                                                        projectid,nation,native,usertype,isepiboly,degreesid,degrees,identifyid,
                                                        organizecode,createuserdeptcode,createuserorgcode,createdate,createuserid,
                                                        departmentid,departmentcode,ISPRESENCE,account,enabledmark,                                                   craft,deletemark,entertime,isfourperson,fourpersontype,healthstatus,craftage,ACCOUNTTYPE,ISAPPLICATIONLDAP,QUICKQUERY,MANAGER,DISTRICT,STREET,ADDRESS,AGE,SpecialtyType)=
                                               (select p.encode,p.realname,p.headicon,p.birthday,p.mobile,p.telephone,p.email,
                                                        p.oicq,p.wechat,p.msn,p.organizeid,p.dutyid,p.dutyname,p.postid,p.postname,
                                                        p.gender,p.isspecialequ,p.isspecial,p.OUTENGINEERID,p.nation,p.native,p.usertype,
                                                        p.isepiboly,p.degreesid,p.degrees,p.identifyid,p.organizecode,p.createuserdeptcode,
                                                        p.createuserorgcode,p.createdate,p.createuserid,p.outprojectid,p.outprojectcode,'1',
                                                        accounts as account,1,p.workoftype,0,to_date('{1}','yyyy-MM-dd'),
                                                        isfourperson,fourpersontype,stateofhealth,workyear,ACCOUNTTYPE,ISAPPLICATIONLDAP,QUICKQUERY,MANAGER,DISTRICT,STREET,ADDRESS,AGE,SpecialtyType
                                                from EPG_APTITUDEINVESTIGATEPEOPLE p 
                                                    where p.id='{0}') where userid='{2}' ", item.ID, DateTime.Now.ToString("yyyy-MM-dd"), user.UserId);
                                res.ExecuteBySql(sql);
                                //查询岗位信息
                                sql = string.Format("select dutyid,organizeid from EPG_APTITUDEINVESTIGATEPEOPLE p where p.id='{0}'", item.ID);
                                DataTable dt = BaseRepository().FindTable(sql);
                                foreach (DataRow dr in dt.Rows)
                                {
                                    //通过岗位ID获取角色信息
                                    sql = string.Format("select t.roleids,t.rolenames from BASE_ROLE t where t.organizeid='{0}' and t.roleid='{1}'", dr[1].ToString(), dr[0].ToString());
                                    DataTable dtRoles = BaseRepository().FindTable(sql);

                                    if (dtRoles != null && dtRoles.Rows.Count > 0)
                                    {
                                        //根据岗位ID更新人员岗位名称
                                        string sqlRole = string.Format(@" update base_user b set b.rolename= '{0}',b.roleid='{2}' where b.dutyid = '{1}'", dtRoles.Rows[0]["rolenames"].ToString(), dr["dutyid"].ToString(), dtRoles.Rows[0]["roleids"].ToString());
                                        res.ExecuteBySql(sqlRole);
                                        string sqlDelRelation = string.Format(@"delete BASE_USERRELATION where userid ='{0}'", user.UserId);
                                        res.ExecuteBySql(sqlDelRelation);
                                        string[] arr = dtRoles.Rows[0]["roleids"].ToString().Split(',');
                                        foreach (string roleId in arr)
                                        {
                                            //插入角色人员关系表
                                            sql = string.Format(string.Format(@" insert into BASE_USERRELATION(USERRELATIONID,userid,Objectid,category,sortcode,isdefault) select '{1}' || rownum,'{3}','{2}','2',0 ,1
                                from EPG_APTITUDEINVESTIGATEPEOPLE p where p.id='{0}'", item.ID, Guid.NewGuid().ToString(), roleId, user.UserId));
                                            res.ExecuteBySql(sql);
                                        }
                                    }
                                }
                                //同步人员证件信息
                                sql = string.Format(string.Format(@"insert into bis_certificate(id,userid,years,certname,certnum,senddate,sendorgan,filepath,enddate,certtype)
                                                                    select c.id,'{1}',c.validttime,c.credentialsname,
                                                                    c.credentialscode,c.credentialstime,c.credentialsorg,
                                                                    c.credentialsfile,ADD_months(credentialstime,validttime*12),certtype 
                                                                    from EPG_CERTIFICATEINSPECTORS c where c.userid ='{0}'", item.ID, user.UserId));
                                if (res.ExecuteBySql(sql) > 0)
                                {
                                    //同步证件照片
                                    sql = string.Format(@"insert into BASE_FILEINFO(fileid,t.folderid,filename,filepath,filesize,t.fileextensions,t.filetype,recid) select fileid || rownum,t.folderid,filename,filepath,filesize,t.fileextensions,t.filetype,'{1}' from BASE_FILEINFO t
where recid in(select id from EPG_CERTIFICATEINSPECTORS where userid ='{0}')", item.ID, user.UserId);
                                    res.ExecuteBySql(sql);
                                }
                                userids.Add(user.UserId);
                                //list.Add(userservice.GetEntity(user.UserId));
                                #endregion
                            }

                        }

                       
                    }
                    else
                    {
                        entity.NEXTAUDITDEPTID = nextCheck.CHECKDEPTID;
                        entity.NEXTAUDITDEPTCODE = nextCheck.CHECKDEPTCODE;
                        entity.NEXTAUDITROLE = nextCheck.CHECKROLENAME;
                        entity.FlowId = nextCheck.ID;
                        entity.ISAUDITOVER = "0";
                    }
                }

                if (!string.IsNullOrEmpty(keyValue))
                {
                    PeopleReviewEntity e = this.BaseRepository().FindEntity(keyValue);
                    if (e != null)
                    {
                        if (string.IsNullOrEmpty(entity.CREATEUSERID))
                        {
                            entity.Create();
                        }
                        entity.Modify(keyValue);
                        res.Update<PeopleReviewEntity>(entity);
                    }
                    else
                    {
                        entity.Create();
                        res.Insert<PeopleReviewEntity>(entity);
                    }
                }
                else
                {
                    entity.Create();
                    res.Insert<PeopleReviewEntity>(entity);
                }
                res.Commit();
                if (entity.ISAUDITOVER == "1")
                {
                    //同步人员到班组
                    if (!string.IsNullOrWhiteSpace(new DataItemDetailService().GetItemValue("bzAppUrl")))
                    {
                        foreach (var item in userids)
                        {
                            list.Add(userservice.GetEntity(item));
                        }
                        SaveUser(list);
                    }
                    //同步外包人员到科技MIS系统(国电荥阳)
                    var isGdxy = new ERCHTMS.Service.SystemManage.DataItemDetailService().GetItemValue("IsGdxy");
                    if (!string.IsNullOrWhiteSpace(isGdxy))
                    {
                        new DepartmentService().SyncUsers(peoplelist);
                    }
                    return userids;
                }
            }
            catch (Exception ex)
            {
                res.Rollback();
                throw ex;
            }
            return new List<string>();
        }

        public void SaveUser(List<UserEntity> user)
        {
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            wc.Headers.Add("Content-Type", "application/json");
            DataItemDetailService dd = new DataItemDetailService();
            string imgUrl = dd.GetItemValue("imgUrl");
            string bzAppUrl = dd.GetItemValue("bzAppUrl");
            wc.Credentials = CredentialCache.DefaultCredentials;
            string logPath = new DataItemDetailService().GetItemValue("imgPath") + "\\logs\\syncbz\\";
            //发送请求到web api并获取返回值，默认为post方式
            try
            {
                List<UserEntity> list = new List<UserEntity>();
                foreach (var item in user)
                {
                    //用户信息
                    item.Gender = item.Gender == "男" ? "1" : "0";
                    if (item.EnterTime == null)
                    {
                        item.EnterTime = DateTime.Now;
                    }
                    if (!string.IsNullOrEmpty(item.SignImg))
                    {
                        item.SignImg = imgUrl + item.SignImg;
                        //if (System.IO.File.Exists(srcPath))
                        //{ //读图片转为Base64String
                        //    var base64 = Convert.ToBase64String(System.IO.File.ReadAllBytes(srcPath));
                        //    user.SignImg = base64;
                        //}
                    }
                    if (!string.IsNullOrEmpty(item.HeadIcon))
                    {
                        item.HeadIcon = imgUrl + item.HeadIcon;
                    }
                    if (!string.IsNullOrWhiteSpace(item.Password))
                    {
                        if (item.Password.Contains("***"))
                        {
                            item.Password = null;
                        }

                    }
                    
                    list.Add(item);
                }
                wc.UploadStringCompleted += wc_UploadStringCompleted;
                wc.UploadStringAsync(new Uri(bzAppUrl + "PostEmployees"), "post", list.ToJson());
            }
            catch (Exception ex)
            {
                if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/logs/syncbz")))
                {
                    System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/logs/syncbz"));
                }
                //将同步结果写入日志文件
                string fileName = "user_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                System.IO.File.AppendAllText(logPath + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：同步数据失败，同步信息" + Newtonsoft.Json.JsonConvert.SerializeObject(user) + ",异常信息：" + ex.Message + "\r\n");
            }
        }
        void wc_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            var error = e.Error;
            //将同步结果写入日志文件
            string fileName = "user_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/logs/syncbz")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/logs/syncbz"));
            }
            try
            {

                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/syncbz/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：同步用户结果>" + e.Result + "\r\n");
            }
            catch (Exception ex)
            {
                string msg = error == null ? ex.Message : error.Message;
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/syncbz/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：同步用户结果>" + msg + "\r\n");
            }

        }
        public string GetItemValue(string itemName)
        {
            string sql = string.Format("select t.itemvalue from base_dataitemdetail t where itemname='{0}'", itemName);
            object obj = new RepositoryFactory().BaseRepository().FindObject(sql);
            return obj == null || obj == System.DBNull.Value ? "" : obj.ToString();
        }
        /// <summary>
        /// 获取上一个部门
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        public DepartmentEntity GetDepartEntityBySpecial(string deptId)
        {
            DepartmentEntity pentity = new DepartmentEntity();

            try
            {
                var deptEntity = new DepartmentService().GetEntity(deptId);

                string professinalName = new DataItemDetailService().GetItemValue("ProfessinalQuality"); //部门性质
                if (null != deptEntity)
                {
                    //如果当前部门是专业,则取再上一个部门
                    if (deptEntity.Nature == professinalName)
                    {
                        pentity = GetDepartEntityBySpecial(deptEntity.ParentId);
                    }
                    else
                    {
                        pentity = deptEntity;
                    }
                }
            }
            catch (System.Exception)
            {
                throw;
            }
            return pentity;
        }
        #region 高风险作业
        /// <summary>
        /// 通过作业单位查询配置审核的下一个审核单位和角色(单位内部流程)
        /// </summary>
        /// <param name="currUser">当前人</param>
        /// <param name="moduleName">模块名称</param>
        /// <param name="workUnitId">作业单位ID</param>
        /// <param name="curFlowId">当前节点ID</param>
        /// <param name="isMulti">是否多个节点人员参与审核(审查/审批)</param>
        /// <returns></returns>
        public ManyPowerCheckEntity CheckAuditForNextByWorkUnit(Operator currUser, string moduleName, string workUnitId, string curFlowId, bool isBack)
        {
            ManyPowerCheckEntity nextCheck = null;//下一步审核
            List<ManyPowerCheckEntity> powerList = powerCheck.GetListBySerialNum(currUser.OrganizeCode, moduleName).ToList();
            if (powerList.Count > 0)
            {
                var workunit = new DepartmentService().GetEntity(workUnitId);
                if (workunit != null)
                {
                    //作业单位为班组时,审核的执行单位未班组或者上级部门,作业单位不为班组审核的执行单位为作业单位
                    if (workunit.Nature == "班组")
                    {

                        var checkCode = string.Empty;
                        var checkDept = string.Empty;
                        //查询出作业班组的上级部门
                        var deptParent = GetDepartEntityBySpecial(workunit.ParentId);
                        if (deptParent != null)
                        {
                            //先查出执行部门编码
                            for (int i = 0; i < powerList.Count; i++)
                            {
                                //执行部门重新赋值
                                if (powerList[i].CHECKDEPTCODE == "-1" || powerList[i].CHECKDEPTID == "-1")
                                {
                                    powerList[i].CHECKDEPTCODE = deptParent.EnCode + "," + workunit.EnCode;
                                    powerList[i].CHECKDEPTID = deptParent.DepartmentId + "," + workunit.DepartmentId;
                                }
                            }
                        }
                    }
                    else
                    {
                        //先查出执行部门编码
                        for (int i = 0; i < powerList.Count; i++)
                        {
                            //执行部门重新赋值
                            if (powerList[i].CHECKDEPTCODE == "-1" || powerList[i].CHECKDEPTID == "-1")
                            {
                                powerList[i].CHECKDEPTCODE = workunit.EnCode;
                                powerList[i].CHECKDEPTID = workunit.DepartmentId;
                            }
                        }
                    }
                    for (int i = 0; i < powerList.Count; i++)
                    {
                        //值班部门重新赋值
                        if (powerList[i].CHECKDEPTCODE == "-5" || powerList[i].CHECKDEPTID == "-5")
                        {
                            powerList[i].CHECKDEPTCODE = workunit.EnCode;
                            powerList[i].CHECKDEPTID = workunit.DepartmentId;
                        }
                    }
                    //不退回
                    if (!isBack)
                    {
                        //(当前流程节点不为控)寻找下一个流程节点
                        if (!string.IsNullOrEmpty(curFlowId))
                        {
                            int curIndex = powerList.FindIndex(p => p.ID == curFlowId); //当前的索引

                            int nextIndex = curIndex + 1; //下一个索引记录

                            if (nextIndex < powerList.Count())
                            {
                                nextCheck = powerList.ElementAt(nextIndex);
                            }
                        }
                        else  //当前流程节点为空，取索引为0的对象 ,则记为初始登记提交流程
                        {
                            nextCheck = powerList.ElementAt(0);  //取当前集合下的第一个节点
                        }

                        if (null != nextCheck)
                        {
                            //当前审核序号下的对应集合
                            var serialList = powerList.Where(p => p.SERIALNUM == nextCheck.SERIALNUM);
                            //集合记录大于1，则表示存在并行审核（审查）的情况
                            if (serialList.Count() > 1)
                            {
                                string flowdept = string.Empty;  // 存取值形式 a1,a2
                                string flowdeptname = string.Empty; // 存取值形式 b1,b2
                                string flowrole = string.Empty;   // 存取值形式 c1|c2|  (c1数据构成： cc1,cc2,cc3)
                                string flowrolename = string.Empty; // 存取值形式 d1|d2| (d1数据构成： dd1,dd2,dd3)

                                ManyPowerCheckEntity slastEntity = new ManyPowerCheckEntity();
                                slastEntity = serialList.LastOrDefault();
                                foreach (ManyPowerCheckEntity model in serialList)
                                {
                                    flowdept += model.CHECKDEPTID + ",";
                                    flowdeptname += model.CHECKDEPTNAME + ",";
                                    flowrole += model.CHECKROLEID + ",";
                                    flowrolename += model.CHECKROLENAME + ",";
                                }
                                if (!flowdept.IsEmpty())
                                {
                                    slastEntity.CHECKDEPTID = flowdept.Substring(0, flowdept.Length - 1);
                                }
                                if (!flowdeptname.IsEmpty())
                                {
                                    slastEntity.CHECKDEPTNAME = flowdeptname.Substring(0, flowdeptname.Length - 1);
                                }
                                if (!flowdept.IsEmpty())
                                {
                                    slastEntity.CHECKROLEID = flowrole.Substring(0, flowrole.Length - 1);
                                }
                                if (!flowdept.IsEmpty())
                                {
                                    slastEntity.CHECKROLENAME = flowrolename.Substring(0, flowrolename.Length - 1);
                                }
                                nextCheck = slastEntity;
                            }
                        }
                    }
                    //退回则取默认为空。
                    return nextCheck;
                }
                else
                {
                    //无作业单位返回空
                    return nextCheck;
                }
            }
            else
            {
                //审核配置返回空
                return nextCheck;
            }
        }


        /// <summary>
        /// 按顺序进行获取下一个流程节点(外包单位流程)
        /// </summary>
        /// <param name="currUser"></param>
        /// <param name="moduleName"></param>
        /// <param name="workUnitId"></param>
        /// <param name="curFlowId"></param>
        /// <param name="isBack"></param>
        /// <param name="isUseSetting"></param>
        /// <returns></returns>
        public ManyPowerCheckEntity CheckAuditForNextByOutsourcing(Operator currUser, string moduleName, string workUnitId, string curFlowId, bool isBack, bool isUseSetting, string projectid="")
        {
            ManyPowerCheckEntity nextCheck = null;//下一步审核
            List<ManyPowerCheckEntity> powerList = powerCheck.GetListBySerialNum(currUser.OrganizeCode, moduleName).ToList();

            if (powerList.Count > 0)
            {
                //先查出执行部门编码
                for (int i = 0; i < powerList.Count; i++)
                {
                    //执行部门重新赋值
                    if (powerList[i].CHECKDEPTCODE == "-1" || powerList[i].CHECKDEPTID == "-1")
                    {
                        Repository<OutsouringengineerEntity> ourEngineer = new Repository<OutsouringengineerEntity>(DbFactory.Base());
                        powerList[i].CHECKDEPTCODE = deptservice.GetEntity(ourEngineer.FindEntity(projectid).ENGINEERLETDEPTID).EnCode;
                        powerList[i].CHECKDEPTID = deptservice.GetEntity(ourEngineer.FindEntity(projectid).ENGINEERLETDEPTID).DepartmentId;
                    }
                    //外包单位、审核专业
                    if (powerList[i].CHECKDEPTCODE == "-2" || powerList[i].CHECKDEPTID == "-2")
                    {
                        var cbsentity = deptservice.GetList().Where(t => t.Description == "外包工程承包商" && t.OrganizeId == currUser.OrganizeId).FirstOrDefault();
                        var wbentity = deptservice.GetEntity(workUnitId);
                        while (wbentity.ParentId!=cbsentity.DepartmentId)
                        {
                            wbentity = deptservice.GetEntity(wbentity.ParentId);
                        }
                        powerList[i].CHECKDEPTCODE = wbentity.EnCode;
                        powerList[i].CHECKDEPTID = wbentity.DepartmentId;
                    }
                }
                //不退回
                if (!isBack)
                {
                    //(当前流程节点不为控)寻找下一个流程节点
                    if (!string.IsNullOrEmpty(curFlowId))
                    {
                        int curIndex = powerList.FindIndex(p => p.ID == curFlowId); //当前的索引

                        int nextIndex = curIndex + 1; //下一个索引记录

                        if (nextIndex < powerList.Count())
                        {
                            nextCheck = powerList.ElementAt(nextIndex);
                        }
                    }
                    else  //当前流程节点为空，取索引为0的对象 ,则记为初始登记提交流程
                    {
                        nextCheck = powerList.ElementAt(0);  //取当前集合下的第一个节点
                    }

                    if (null != nextCheck)
                    {
                        //当前审核序号下的对应集合
                        var serialList = powerList.Where(p => p.SERIALNUM == nextCheck.SERIALNUM);
                        //集合记录大于1，则表示存在并行审核（审查）的情况
                        if (serialList.Count() > 1)
                        {
                            //已配置审查
                            if (isUseSetting)
                            {
                                string flowdept = string.Empty;  // 存取值形式 a1,a2
                                string flowdeptname = string.Empty; // 存取值形式 b1,b2
                                string flowrole = string.Empty;   // 存取值形式 c1|c2|  (c1数据构成： cc1,cc2,cc3)
                                string flowrolename = string.Empty; // 存取值形式 d1|d2| (d1数据构成： dd1,dd2,dd3)

                                ManyPowerCheckEntity slastEntity = new ManyPowerCheckEntity();
                                slastEntity = serialList.LastOrDefault();
                                foreach (ManyPowerCheckEntity model in serialList)
                                {
                                    flowdept += model.CHECKDEPTID + ",";
                                    flowdeptname += model.CHECKDEPTNAME + ",";
                                    flowrole += model.CHECKROLEID + "|";
                                    flowrolename += model.CHECKROLENAME + "|";
                                }
                                if (!flowdept.IsEmpty())
                                {
                                    slastEntity.CHECKDEPTID = flowdept.Substring(0, flowdept.Length - 1);
                                }
                                if (!flowdeptname.IsEmpty())
                                {
                                    slastEntity.CHECKDEPTNAME = flowdeptname.Substring(0, flowdeptname.Length - 1);
                                }
                                if (!flowdept.IsEmpty())
                                {
                                    slastEntity.CHECKROLEID = flowrole.Substring(0, flowrole.Length - 1);
                                }
                                if (!flowdept.IsEmpty())
                                {
                                    slastEntity.CHECKROLENAME = flowrolename.Substring(0, flowrolename.Length - 1);
                                }
                                nextCheck = slastEntity;
                            }
                            else
                            {
                                int shFirstIndex = serialList.Count();
                                nextCheck = powerList.ElementAt(shFirstIndex);
                            }

                        }
                    }
                }
                //退回则取默认为空。
                return nextCheck;
            }
            else
            {
                return nextCheck;
            }
        }


        /// <summary>
        /// 起重吊装作业专用(审核专业)
        /// </summary>
        /// <param name="currUser">当前人</param>
        /// <param name="moduleName">模块名称</param>
        /// <param name="workUnitId">审核专业ID</param>
        /// <param name="curFlowId">当前节点ID</param>
        /// <param name="isBack">是否回退</param>
        /// <returns></returns>
        public ManyPowerCheckEntity CheckAuditForNextByLiftHoist(Operator currUser, string moduleName, string workUnitId, string curFlowId, bool isBack)
        {
            ManyPowerCheckEntity nextCheck = null;//下一步审核
            List<ManyPowerCheckEntity> powerList = powerCheck.GetListBySerialNum(currUser.OrganizeCode, moduleName).ToList();
            if (powerList.Count > 0)
            {
                //如果为审核专业，则为当前审核专业下的人审核
                var workunit = new DepartmentService().GetEntity(workUnitId);
                for (int i = 0; i < powerList.Count; i++)
                {
                    //执行部门重新赋值
                    if (powerList[i].CHECKDEPTCODE == "-1" || powerList[i].CHECKDEPTID == "-1")
                    {
                        if (workunit == null)
                        {
                            Repository<OutsouringengineerEntity> ourEngineer = new Repository<OutsouringengineerEntity>(DbFactory.Base());
                            powerList[i].CHECKDEPTCODE = deptservice.GetEntity(ourEngineer.FindEntity(workUnitId).ENGINEERLETDEPTID).EnCode;
                            powerList[i].CHECKDEPTID = deptservice.GetEntity(ourEngineer.FindEntity(workUnitId).ENGINEERLETDEPTID).DepartmentId;
                        }
                        else
                        {
                            if (workunit.Nature == "班组" || workunit.Nature == "专业")
                            {
                                var checkCode = string.Empty;
                                var checkDept = string.Empty;
                                //查询出作业班组的上级部门
                                var deptParent = GetDepartEntityBySpecial(workunit.ParentId);
                                if (deptParent != null)
                                {
                                    powerList[i].CHECKDEPTCODE = deptParent.EnCode + "," + workunit.EnCode;
                                    powerList[i].CHECKDEPTID = deptParent.DepartmentId + "," + workunit.DepartmentId;
                                }
                            }
                            else
                            {
                                powerList[i].CHECKDEPTCODE = workunit.EnCode;
                                powerList[i].CHECKDEPTID = workunit.DepartmentId;
                            }
                        }
                    }
                }
                //不退回
                if (!isBack)
                {
                    //(当前流程节点不为控)寻找下一个流程节点
                    if (!string.IsNullOrEmpty(curFlowId))
                    {
                        int curIndex = powerList.FindIndex(p => p.ID == curFlowId); //当前的索引

                        int nextIndex = curIndex + 1; //下一个索引记录

                        if (nextIndex < powerList.Count())
                        {
                            nextCheck = powerList.ElementAt(nextIndex);
                        }
                    }
                    else  //当前流程节点为空，取索引为0的对象 ,则记为初始登记提交流程
                    {
                        nextCheck = powerList.ElementAt(0);  //取当前集合下的第一个节点
                    }

                    if (null != nextCheck)
                    {
                        //当前审核序号下的对应集合
                        var serialList = powerList.Where(p => p.SERIALNUM == nextCheck.SERIALNUM);
                        //集合记录大于1，则表示存在并行审核（审查）的情况
                        if (serialList.Count() > 1)
                        {
                            string flowdept = string.Empty;  // 存取值形式 a1,a2
                            string flowdeptname = string.Empty; // 存取值形式 b1,b2
                            string flowrole = string.Empty;   // 存取值形式 c1|c2|  (c1数据构成： cc1,cc2,cc3)
                            string flowrolename = string.Empty; // 存取值形式 d1|d2| (d1数据构成： dd1,dd2,dd3)

                            ManyPowerCheckEntity slastEntity = new ManyPowerCheckEntity();
                            slastEntity = serialList.LastOrDefault();
                            foreach (ManyPowerCheckEntity model in serialList)
                            {
                                flowdept += model.CHECKDEPTID + ",";
                                flowdeptname += model.CHECKDEPTNAME + ",";
                                flowrole += model.CHECKROLEID + "|";
                                flowrolename += model.CHECKROLENAME + "|";
                            }
                            if (!flowdept.IsEmpty())
                            {
                                slastEntity.CHECKDEPTID = flowdept.Substring(0, flowdept.Length - 1);
                            }
                            if (!flowdeptname.IsEmpty())
                            {
                                slastEntity.CHECKDEPTNAME = flowdeptname.Substring(0, flowdeptname.Length - 1);
                            }
                            if (!flowdept.IsEmpty())
                            {
                                slastEntity.CHECKROLEID = flowrole.Substring(0, flowrole.Length - 1);
                            }
                            if (!flowdept.IsEmpty())
                            {
                                slastEntity.CHECKROLENAME = flowrolename.Substring(0, flowrolename.Length - 1);
                            }
                            nextCheck = slastEntity;
                        }
                    }
                }
                //退回则取默认为空。
                return nextCheck;
            }
            else
            {
                //审核配置返回空
                return nextCheck;
            }
        }
        #endregion


        #region 按顺序进行获取下一个流程节点
        /// <summary>
        /// 按顺序进行获取下一个流程节点
        /// </summary>
        /// <param name="currUser"></param>
        /// <param name="moduleName"></param>
        /// <param name="outengineerid"></param>
        /// <param name="flowtype"></param>
        /// <param name="curFlowId"></param>
        /// <param name="isBack"></param>
        /// <param name="isMulti">是否多个节点人员参与审核(审查/审批)</param>
        /// <returns></returns>
        public ManyPowerCheckEntity CheckAuditForNextFlow(Operator currUser, string moduleName, string outengineerid, string curFlowId, bool isBack, bool isUseSetting)
        {
            ManyPowerCheckEntity nextCheck = null;//下一步审核
            List<ManyPowerCheckEntity> powerList = powerCheck.GetListBySerialNum(currUser.OrganizeCode, moduleName).ToList();

            if (powerList.Count > 0)
            {
                //先查出执行部门编码
                for (int i = 0; i < powerList.Count; i++)
                {
                    //执行部门重新赋值
                    if (powerList[i].CHECKDEPTCODE == "-1" || powerList[i].CHECKDEPTID == "-1")
                    {
                        Repository<OutsouringengineerEntity> ourEngineer = new Repository<OutsouringengineerEntity>(DbFactory.Base());
                        powerList[i].CHECKDEPTCODE = new DepartmentService().GetEntity(ourEngineer.FindEntity(outengineerid).ENGINEERLETDEPTID).EnCode;
                        powerList[i].CHECKDEPTID = new DepartmentService().GetEntity(ourEngineer.FindEntity(outengineerid).ENGINEERLETDEPTID).DepartmentId;
                    }
                    //外包单位重新赋值
                    if (powerList[i].CHECKDEPTCODE == "-2" || powerList[i].CHECKDEPTID == "-2")
                    {
                        Repository<OutsouringengineerEntity> ourEngineer = new Repository<OutsouringengineerEntity>(DbFactory.Base());
                        powerList[i].CHECKDEPTCODE = new DepartmentService().GetEntity(ourEngineer.FindEntity(outengineerid).OUTPROJECTID).EnCode;
                        powerList[i].CHECKDEPTID = new DepartmentService().GetEntity(ourEngineer.FindEntity(outengineerid).OUTPROJECTID).DepartmentId;
                    }
                }
                //不退回
                if (!isBack)
                {
                    //(当前流程节点不为控)寻找下一个流程节点
                    if (!string.IsNullOrEmpty(curFlowId))
                    {
                        int curIndex = powerList.FindIndex(p => p.ID == curFlowId); //当前的索引

                        int nextIndex = curIndex + 1; //下一个索引记录

                        if (nextIndex < powerList.Count())
                        {
                            nextCheck = powerList.ElementAt(nextIndex);
                        }
                    }
                    else  //当前流程节点为空，取索引为0的对象 ,则记为初始登记提交流程
                    {
                        nextCheck = powerList.ElementAt(0);  //取当前集合下的第一个节点
                    }

                    if (null != nextCheck)
                    {
                        //当前审核序号下的对应集合
                        var serialList = powerList.Where(p => p.SERIALNUM == nextCheck.SERIALNUM);
                        //集合记录大于1，则表示存在并行审核（审查）的情况
                        if (serialList.Count() > 1)
                        {
                            //已配置审查
                            if (isUseSetting)
                            {
                                string flowdept = string.Empty;  // 存取值形式 a1,a2
                                string flowdeptname = string.Empty; // 存取值形式 b1,b2
                                string flowrole = string.Empty;   // 存取值形式 c1|c2|  (c1数据构成： cc1,cc2,cc3)
                                string flowrolename = string.Empty; // 存取值形式 d1|d2| (d1数据构成： dd1,dd2,dd3)

                                ManyPowerCheckEntity slastEntity = new ManyPowerCheckEntity();
                                slastEntity = serialList.LastOrDefault();
                                foreach (ManyPowerCheckEntity model in serialList)
                                {
                                    flowdept += model.CHECKDEPTID + ",";
                                    flowdeptname += model.CHECKDEPTNAME + ",";
                                    flowrole += model.CHECKROLEID + "|";
                                    flowrolename += model.CHECKROLENAME + "|";
                                }
                                if (!flowdept.IsEmpty())
                                {
                                    slastEntity.CHECKDEPTID = flowdept.Substring(0, flowdept.Length - 1);
                                }
                                if (!flowdeptname.IsEmpty())
                                {
                                    slastEntity.CHECKDEPTNAME = flowdeptname.Substring(0, flowdeptname.Length - 1);
                                }
                                if (!flowdept.IsEmpty())
                                {
                                    slastEntity.CHECKROLEID = flowrole.Substring(0, flowrole.Length - 1);
                                }
                                if (!flowdept.IsEmpty())
                                {
                                    slastEntity.CHECKROLENAME = flowrolename.Substring(0, flowrolename.Length - 1);
                                }
                                nextCheck = slastEntity;
                            }
                            else
                            {
                                int shFirstIndex = serialList.Count();
                                nextCheck = powerList.ElementAt(shFirstIndex);
                            }

                        }
                    }
                }
                //退回则取默认为空。
                return nextCheck;
            }
            else
            {
                return nextCheck;
            }
        }
        #endregion
        /// <summary>
        /// 演练记录评价流程
        /// </summary>
        /// <param name="currUser">当前用户</param>
        /// <param name="moduleName">模块名称</param>
        /// <param name="DrillRecordId">演练记录Id</param>
        /// <param name="curFlowId">当前节点</param>
        /// <param name="isUseSetting"></param>
        /// <returns></returns>
        public ManyPowerCheckEntity CheckEvaluateForNextFlow(Operator currUser, string moduleName, DrillplanrecordEntity entity)
        {
            ManyPowerCheckEntity nextCheck = null;//下一步审核
            List<ManyPowerCheckEntity> powerList = powerCheck.GetListBySerialNum(currUser.OrganizeCode, moduleName).ToList();

            if (powerList.Count > 0)
            {
                //先查出执行部门编码
                for (int i = 0; i < powerList.Count; i++)
                {
                   
                    //执行部门重新赋值
                    if (powerList[i].CHECKDEPTCODE == "-1" || powerList[i].CHECKDEPTID == "-1")
                    {
                        powerList[i].CHECKDEPTCODE = new DepartmentService().GetParentDeptBySpecialArgs(new DepartmentService().GetEntityByCode(entity.CREATEUSERDEPTCODE).ParentId, "部门").EnCode;
                        powerList[i].CHECKDEPTID = new DepartmentService().GetParentDeptBySpecialArgs(new DepartmentService().GetEntityByCode(entity.CREATEUSERDEPTCODE).ParentId, "部门").DepartmentId;
                    }
                    //创建部门
                    if (powerList[i].CHECKDEPTCODE == "-3" || powerList[i].CHECKDEPTID == "-3")
                    {
                        powerList[i].CHECKDEPTCODE = new DepartmentService().GetEntityByCode(entity.CREATEUSERDEPTCODE).EnCode;
                        powerList[i].CHECKDEPTID = new DepartmentService().GetEntityByCode(entity.CREATEUSERDEPTCODE).DepartmentId;
                    }
                }
            
                    //(当前流程节点不为控)寻找下一个流程节点
                if (!string.IsNullOrEmpty(entity.NodeId))
                    {
                        int curIndex = powerList.FindIndex(p => p.ID == entity.NodeId); //当前的索引

                        int nextIndex = curIndex + 1; //下一个索引记录

                        if (nextIndex < powerList.Count())
                        {
                            nextCheck = powerList.ElementAt(nextIndex);
                        }
                    }
                    else  //当前流程节点为空，取索引为0的对象 ,则记为初始登记提交流程
                    {
                        nextCheck = powerList.ElementAt(0);  //取当前集合下的第一个节点
                    }
                //退回则取默认为空。
                return nextCheck;
            }
            else
            {
                return nextCheck;
            }
        }
        #region  当前登录人是否有权限审核并获取下一次审核权限实体
        /// <summary>
        /// 当前登录人是否有权限审核并获取下一次审核权限实体
        /// </summary>
        /// <param name="currUser">当前登录人</param>
        /// <param name="state">是否有权限审核 1：能审核 0 ：不能审核</param>
        /// <param name="moduleName">模块名称</param>
        /// <param name="outengineerid">工程Id</param>
        /// <param name="isStep">是否跳级审核</param>
        /// <param name="CurFlowId">当前流程节点ID</param>
        /// <returns>null-当前为最后一次审核,ManyPowerCheckEntity：下一次审核权限实体</returns>
        public ManyPowerCheckEntity CheckAuditPower(Operator currUser, out string state, string moduleName, string outengineerid,Boolean isStep=true,string CurFlowId="", string EngineerletDeptId = "")
        {
            try
            {
                ManyPowerCheckEntity nextCheck = null;//下一步审核
                List<ManyPowerCheckEntity> powerList = powerCheck.GetListBySerialNum(currUser.OrganizeCode, moduleName);

                if (powerList.Count > 0)
                {
                    //先查出执行部门编码
                    for (int i = 0; i < powerList.Count; i++)
                    {
                        if (powerList[i].ApplyType != "1") // 如果取脚本数据，就不执行角色判断 2020/12/03
                        {
                            if (powerList[i].CHECKDEPTCODE == "-1" || powerList[i].CHECKDEPTID == "-1")
                            {
                                Repository<OutsouringengineerEntity> ourEngineer = new Repository<OutsouringengineerEntity>(DbFactory.Base());
                                string executedept = ourEngineer.FindEntity(outengineerid).ENGINEERLETDEPTID;
                                //powerList[i].CHECKDEPTCODE = new DepartmentService().GetEntity(ourEngineer.FindEntity(outengineerid).ENGINEERLETDEPTID).EnCode;
                                //powerList[i].CHECKDEPTID = new DepartmentService().GetEntity(ourEngineer.FindEntity(outengineerid).ENGINEERLETDEPTID).DepartmentId;
                                switch (powerList[i].ChooseDeptRange) //判断部门范围
                                {
                                    case "0":
                                        powerList[i].CHECKDEPTID = executedept;
                                        break;
                                    case "1":
                                        var dept = deptservice.GetEntity(executedept);
                                        while (dept.Nature != "部门")
                                        {
                                            dept = deptservice.GetEntity(dept.ParentId);
                                        }
                                        powerList[i].CHECKDEPTID = dept.DepartmentId;
                                        break;
                                    case "2":
                                        var dept1 = deptservice.GetEntity(executedept);
                                        while (dept1.Nature != "部门")
                                        {
                                            dept1 = deptservice.GetEntity(dept1.ParentId);
                                        }
                                        powerList[i].CHECKDEPTID = (dept1.DepartmentId + "," + executedept).Trim(',');
                                        break;
                                    default:
                                        powerList[i].CHECKDEPTID = executedept;
                                        break;
                                }

                            }
                            if (powerList[i].CHECKDEPTCODE == "-2" || powerList[i].CHECKDEPTID == "-2")
                            {
                                Repository<OutsouringengineerEntity> ourEngineer = new Repository<OutsouringengineerEntity>(DbFactory.Base());
                                powerList[i].CHECKDEPTCODE = new DepartmentService().GetEntity(ourEngineer.FindEntity(outengineerid).OUTPROJECTID).EnCode;
                                powerList[i].CHECKDEPTID = new DepartmentService().GetEntity(ourEngineer.FindEntity(outengineerid).OUTPROJECTID).DepartmentId;
                            }
                            if (powerList[i].CHECKDEPTCODE == "-6" || powerList[i].CHECKDEPTID == "-6")
                            {
                                Repository<OutsouringengineerEntity> ourEngineer = new Repository<OutsouringengineerEntity>(DbFactory.Base());
                                var Engineer = ourEngineer.FindEntity(outengineerid);
                                if (string.IsNullOrEmpty(Engineer.SupervisorId))
                                {
                                    powerList[i].CHECKDEPTCODE = "";
                                    powerList[i].CHECKDEPTID = "";
                                }
                                else
                                {
                                    powerList[i].CHECKDEPTCODE = new DepartmentService().GetEntity(ourEngineer.FindEntity(outengineerid).SupervisorId).EnCode;
                                    powerList[i].CHECKDEPTID = new DepartmentService().GetEntity(ourEngineer.FindEntity(outengineerid).SupervisorId).DepartmentId;
                                }

                            }
                        }
                        
                    }
                    List<ManyPowerCheckEntity> checkPower = new List<ManyPowerCheckEntity>();

                    if (isStep)
                    {
                        //登录人是否有审核权限--有审核权限直接审核通过
                        for (int i = 0; i < powerList.Count; i++)
                        {
                            if (powerList[i].ApplyType == "0")
                            {
                                if (powerList[i].CHECKDEPTID == currUser.DeptId)
                                {
                                    var rolelist = currUser.RoleName.Split(',');
                                    for (int j = 0; j < rolelist.Length; j++)
                                    {
                                        if (powerList[i].CHECKROLENAME.Contains(rolelist[j]))
                                        {
                                            checkPower.Add(powerList[i]);
                                            break;
                                        }
                                    }
                                }
                            }
                            else if (powerList[i].ApplyType == "1")
                            {
                                var parameter = new List<DbParameter>();
                                //取脚本，获取账户的范围信息
                                if (powerList[i].ScriptCurcontent.Contains("@outengineerid"))
                                {
                                    parameter.Add(DbParameters.CreateDbParameter("@outengineerid", !string.IsNullOrEmpty(outengineerid) ? outengineerid : ""));
                                }
                                if (powerList[i].ScriptCurcontent.Contains("@engineerletdeptid"))
                                {
                                    parameter.Add(DbParameters.CreateDbParameter("@engineerletdeptid", !string.IsNullOrEmpty(EngineerletDeptId) ? EngineerletDeptId : ""));
                                }
                                DbParameter[] arrayparam = parameter.ToArray();
                                var userIds = DbFactory.Base().FindList<UserEntity>(powerList[i].ScriptCurcontent, arrayparam).Cast<UserEntity>().Aggregate("", (current, user) => current + (user.UserId + ",")).Trim(',');
                                if (userIds.Contains(currUser.UserId))
                                {
                                    checkPower.Add(powerList[i]);
                                    //break;
                                }
                            }
                        }
                    }
                    else
                    {
                        //(当前流程节点不为控)寻找下一个流程节点
                        if (!string.IsNullOrEmpty(CurFlowId))
                        {
                            int curIndex = powerList.FindIndex(p => p.ID == CurFlowId); //当前的索引

                            int nextIndex = curIndex + 1; //下一个索引记录

                            if (nextIndex < powerList.Count())
                            {
                                nextCheck = powerList.ElementAt(nextIndex);
                            }
                            else
                            {
                                state = "0";
                                return null;
                            }
                        }
                        else  //当前流程节点为空，取索引为0的对象 ,则记为初始登记提交流程
                        {
                            nextCheck = powerList.ElementAt(0);  //取当前集合下的第一个节点
                        }
                    }
                    if (checkPower.Count > 0)
                    {
                        state = "1";
                    }
                    else
                    {
                        state = "0";
                    }

                    while (nextCheck == null || (string.IsNullOrEmpty(nextCheck.CHECKDEPTID) && nextCheck.ApplyType == "0"))
                    {
                        if (isStep)
                        {
                            if (checkPower.Count > 0)
                            {
                                ManyPowerCheckEntity check = checkPower.Last();//当前

                                for (int i = 0; i < powerList.Count; i++)
                                {
                                    if (check.ID == powerList[i].ID)
                                    {
                                        if ((i + 1) >= powerList.Count)
                                        {
                                            return null;
                                        }
                                        else
                                        {
                                            nextCheck = powerList[i + 1];
                                        }
                                    }
                                }
                            }
                            else
                            {
                                nextCheck = powerList.First();
                            }
                            checkPower.Add(nextCheck);
                        }
                        else
                        {
                            for (int i = 0; i < powerList.Count; i++)
                            {
                                if (nextCheck.ID == powerList[i].ID)
                                {
                                    if ((i + 1) >= powerList.Count)
                                    {
                                        return null;
                                    }
                                    else
                                    {
                                        nextCheck = powerList[i + 1];
                                        break;
                                    }
                                }
                            }
                        }
                        
                    }
                   
                }
                else
                {
                    state = "0";
                }

                if (nextCheck != null)
                {
                    //当前审核序号下的对应集合
                    var serialList = powerList.Where(p => p.SERIALNUM == nextCheck.SERIALNUM);
                    //集合记录大于1，则表示存在并行审核（审查）的情况
                    if (serialList.Count() > 1)
                    {
                        ManyPowerCheckEntity slastEntity = new ManyPowerCheckEntity();
                        slastEntity = serialList.LastOrDefault();
                        nextCheck = slastEntity;
                    }
                }
                return nextCheck;
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }
        #endregion
        public DataTable GetPagePeopleReviewListJson(Pagination pagination, string queryJson)
        {
            Operator currUser = OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //查询条件
            if (!queryParam["orgCode"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.createuserorgcode='{0}' ", queryParam["orgCode"].ToString());
            }
            if (!queryParam["proname"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and e.engineername like'%{0}%' ", queryParam["proname"].ToString());
            }
            if (!queryParam["outengineerid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.outengineerid ='{0}' ", queryParam["outengineerid"].ToString());
            }
            if (!queryParam["indexState"].IsEmpty())//首页代办
            {

                string strCondition = "";
                strCondition = string.Format(" and t.createuserorgcode='{0}' and t.isauditover=0 and t.issaveorcommit='1'", currUser.OrganizeCode);
                DataTable data = BaseRepository().FindTable("select " + pagination.p_kid + "," + pagination.p_fields + " from " + pagination.p_tablename + " where " + pagination.conditionJson + strCondition);
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    var engineerEntity = outsouringengineerservice.GetEntity(data.Rows[i]["outengineerid"].ToString());
                    var excutdept = departmentservice.GetEntity(engineerEntity.ENGINEERLETDEPTID).DepartmentId;
                    var outengineerdept = departmentservice.GetEntity(engineerEntity.OUTPROJECTID).DepartmentId;
                    var supervisordept = string.IsNullOrEmpty(engineerEntity.SupervisorId) ? "" : departmentservice.GetEntity(engineerEntity.SupervisorId).DepartmentId;
                    //获取下一步审核人
                    string str = powerCheck.GetApproveUserId(data.Rows[i]["flowid"].ToString(), data.Rows[i]["id"].ToString(), "", "", excutdept, outengineerdept, "", "", "", supervisordept, data.Rows[i]["outengineerid"].ToString());
                    data.Rows[i]["approveuserids"] = str;
                }

                string[] applyids = data.Select(" approveuserids like '%" + currUser.UserId + "%'").AsEnumerable().Select(d => d.Field<string>("id")).ToArray();

                pagination.conditionJson += string.Format(" and t.id in ('{0}') {1}", string.Join("','", applyids), strCondition);
            }
            if (!queryParam["projectid"].IsEmpty())//工程管理流程图跳转
            {
                pagination.conditionJson += string.Format(" and e.id ='{0}'", queryParam["projectid"].ToString());
            }
            DataTable dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            dt.Columns.Add("auditpeople", typeof(string));
            dt.Columns.Add("auditresult", typeof(string));
            dt.Columns.Add("flowdeptname", typeof(string));
            dt.Columns.Add("flowname", typeof(string));
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    if (item["isauditover"].ToString() == "1")
                    {

                        List<AptitudeinvestigateauditEntity> list = new AptitudeinvestigateauditService().GetList("").Where(x => x.APTITUDEID == item["id"].ToString()).ToList();
                        if (list.Count > 0)
                        {
                            AptitudeinvestigateauditEntity lastAudit = list.OrderBy(x => x.AUDITTIME).Last();
                            if (lastAudit != null)
                            {
                                item["auditpeople"] = lastAudit.AUDITPEOPLE;
                                item["auditresult"] = lastAudit.AUDITRESULT == "0" ? "合格" : "不合格";
                            }
                        }
                        item["flowdeptname"] = "";
                        item["flowname"] = "";
                        //AptitudeinvestigateauditEntity lastAudit = new AptitudeinvestigateauditService().GetList("").Where(x => x.APTITUDEID == item["id"].ToString()).ToList().Last();
                    }
                    else
                    {
                        var dept = new DepartmentService().GetEntity(item["nextauditdeptid"].ToString());
                        item["flowdeptname"] = dept == null ? "" : dept.FullName;
                        item["flowname"] = dept == null ? "" : dept.FullName + "审核中";
                    }
                }
            } 
            return dt;
        }
    }
}
