using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Entity.LllegalManage;
using ERCHTMS.IService.LllegalManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Data;
using ERCHTMS.Entity.BaseManage;
using BSFramework.Data;
using ERCHTMS.Service.BaseManage;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Service.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.Entity.LllegalManage.ViewModel;
using ERCHTMS.IService.SystemManage;
using System.Web;

namespace ERCHTMS.Service.LllegalManage
{
    /// <summary>
    /// 描 述：违章基本信息
    /// </summary>
    public class LllegalRegisterService : RepositoryFactory<LllegalRegisterEntity>, LllegalRegisterIService
    {

        private IDepartmentService Idepartmentservice = new DepartmentService();
        private IDataItemDetailService idataitemdetailservice = new DataItemDetailService();
        #region 获取数据

        #region 获取列表
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<LllegalRegisterEntity> GetList(string queryJson)
        {
            //return this.BaseRepository().IQueryable().ToList();
            return this.BaseRepository().FindList(" select * from bis_lllegalregister where 1=1 " + queryJson).ToList();
        }
        #endregion

        /// <summary>
        /// 获取通用查询分页
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public DataTable GetGeneralQuery(Pagination pagination)
        {
            DatabaseType dataType = DbHelper.DbType;
            var dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            return dt;
        }

        #region 获取实体
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public LllegalRegisterEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion



        #region 获取违章积分数据
        /// <summary>
        /// 获取违章积分数据
        /// </summary>
        /// <param name="basePoint"></param>
        /// <param name="year"></param>
        /// <param name="userids"></param>
        /// <returns></returns>
        public DataTable GetLllegalPointData(string basePoint, string year, string userids, string condition)
        {
            string fileName = "推送违章对接培训平台接口" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            try
            {

                string strWhere = "1=1 ";
                string strWhere0 = "1=1 ";
                string uid = string.Empty;
                string sql = @"select * from ( select min(deptname) as deptname,realname,count(1) as wznum,sum(lllegalpoint) 
                           as lllegalpoint,  (case when (to_number({0})- sum(lllegalpoint)) >=0 then  (to_number({0})- sum(lllegalpoint)) else 0 end) score ,account,userid from v_lllegalassesforperson where {1} group by RealName ,account,userid) a  where {2}";

                if (!string.IsNullOrEmpty(year))
                {
                    strWhere += string.Format(" and  to_char(lllegaltime, 'yyyy')='{0}'", year);
                }
                if (!string.IsNullOrEmpty(userids))
                {
                    string[] userstr = userids.Split(',');
                    foreach (string str in userstr)
                    {
                        uid += "'" + str + "',";
                    }
                }
                if (!string.IsNullOrEmpty(uid))
                {
                    uid = uid.Substring(0, uid.Length - 1);

                    strWhere += string.Format(" and  userid  in ({0})", uid);
                }
                if (!string.IsNullOrEmpty(condition))
                {
                    strWhere0 += string.Format(" and  {0}", condition);
                }
                sql = string.Format(sql, basePoint, strWhere, strWhere0);

                return this.BaseRepository().FindTable(sql);
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), "推送违章对接培训平台接口3:错误信息:" + ex.ToJson() + "\r\n");
                throw ex;
            }
        }
        #endregion

        #region 违章检查集合
        ///// <summary>
        ///// 违章检查集合
        ///// </summary>
        ///// <param name="checkId"></param>
        ///// <param name="checkman"></param>
        ///// <returns></returns>
        public DataTable GetListByCheckId(string checkId, string checkman, string flowstate)
        {
            string sql = @"select a.id,b.reformpeopleid,b.reformpeople,b.isappoint,a.isupsafety,a.createuserid ,a.flowstate from v_lllegalallbaseinfo a 
                                                left join v_lllegalreforminfo b on a.id = b.lllegalid
                                                where 1=1";
            if (!string.IsNullOrEmpty(checkId))
            {
                sql += string.Format(" and (a.reseverone = '{0}' or a.resevertwo ='{0}')", checkId);
            }
            if (!string.IsNullOrEmpty(checkman))
            {
                sql += string.Format(" and a.createuserid = '{0}'", checkman);
            }
            if (!string.IsNullOrEmpty(flowstate))
            {
                sql += string.Format(" and a.flowstate = '{0}'", flowstate);
            }
            return this.BaseRepository().FindTable(sql);
        }
        #endregion

        #region 通过违章编号，来判断是否存在重复现象
        /// <summary>
        /// 通过违章编号，来判断是否存在重复现象
        /// </summary>
        /// <param name="LllegalNumber"></param>
        /// <returns></returns>
        public IList<LllegalRegisterEntity> GetListByNumber(string LllegalNumber)
        {
            return this.BaseRepository().IQueryable().ToList().Where(p => p.LLLEGALNUMBER == LllegalNumber).ToList();
        }
        #endregion

        #region 通过当前用户获取对应隐患的隐患描述(取前十个)
        /// <summary>
        /// 通过当前用户获取对应隐患的隐患描述
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable GetLllegalDescribeList(string userId, string lllegaldescribe)
        {

            string sql = string.Empty;

            string strwhere = " 1=1 ";

            if (!string.IsNullOrEmpty(lllegaldescribe))
            {
                strwhere += string.Format(@" and lllegaldescribe like '%{0}%'", lllegaldescribe);
            }

            sql = string.Format(@"select a.*  from (select createuserid, createdate,lllegalnumber,lllegaltype, lllegaltypename ,lllegaltime,lllegallevel,
                lllegallevelname, lllegalperson,lllegalpersonid,lllegalteam,lllegalteamcode,lllegaldepart,lllegaldepartcode,
                lllegaldescribe,lllegaladdress ,lllegalpic,reformrequire,flowstate,createusername ,addtype,
                reformpeople,reformpeopleid,reformtel,reformdeptcode,reformdeptname,reformdeadline,reformfinishdate,reformstatus,reformmeasure,
                acceptpeopleid,acceptpeople,acceptdeptname,acceptdeptcode,acceptresult,acceptmind,accepttime,reseverid,resevertype ,participant  ,row_number() over( order by createdate desc) as rn  from v_lllegalallbaseinfo
                                                                             where {0}  and createuserid ='{1}' and  lllegaldescribe is not null   order by createdate desc
                                                                             ) a  where rn <=10  order by createdate desc ", strwhere, userId);

            return this.BaseRepository().FindTable(sql);
        }
        #endregion

        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            string sql = string.Empty;
            /**********删除所有违章相关的信息************/
            //删除违章评估信息
            sql = string.Format(@" delete bis_lllegalapprove where lllegalid ='{0}' ", keyValue);
            this.BaseRepository().ExecuteBySql(sql);
            //删除违章整改信息
            sql = string.Format(@" delete bis_lllegalreform where lllegalid ='{0}' ", keyValue);
            this.BaseRepository().ExecuteBySql(sql);
            //删除违章验收信息    
            sql = string.Format(@" delete bis_lllegalaccept where lllegalid ='{0}' ", keyValue);
            this.BaseRepository().ExecuteBySql(sql);
            //删除违章考核信息 
            sql = string.Format(@" delete bis_lllegalpunish where lllegalid ='{0}' ", keyValue);
            this.BaseRepository().ExecuteBySql(sql);
            //删除违章完善信息    
            sql = string.Format(@" delete bis_lllegalconfirm where lllegalid ='{0}' ", keyValue);
            this.BaseRepository().ExecuteBySql(sql);
            //删除违章基本信息
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, LllegalRegisterEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }

        #region 删除违章相关所有内容
        /// <summary>
        /// 删除违章相关所有内容
        /// </summary>
        /// <param name="keyValue"></param>
        public void RemoveFormByBid(string keyValue)
        {
            string sql = string.Format(@" delete bis_lllegalregister where id ='{0}' ", keyValue); //违章基本信息表
            this.BaseRepository().ExecuteBySql(sql);
            sql = string.Format(@" delete bis_lllegalapprove where lllegalid ='{0}' ", keyValue); //违章核准
            this.BaseRepository().ExecuteBySql(sql);
            sql = string.Format(@" delete bis_lllegalpunish where lllegalid ='{0}' ", keyValue);  //核准过程中惩罚表
            this.BaseRepository().ExecuteBySql(sql);
            sql = string.Format(@" delete bis_lllegalreform where lllegalid ='{0}' ", keyValue); // 违章整改
            this.BaseRepository().ExecuteBySql(sql);
            sql = string.Format(@" delete bis_lllegalaccept where lllegalid ='{0}' ", keyValue); //违章验收
            this.BaseRepository().ExecuteBySql(sql);
        }
        #endregion

        #endregion

        public string GetCheckIds(string id)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dt = BaseRepository().FindTable(string.Format("select id from BIS_SAFTYCHECKDATARECORD where rid='{0}'", id));
            foreach (DataRow dr in dt.Rows)
            {
                sb.AppendFormat("{0},", dr[0].ToString());
                sb.AppendFormat("{0},", GetCheckIds(dr[0].ToString()));
            }
            return sb.ToString().Trim(',').Replace(",,", ",");
        }
        #region  违章基础信息查询
        /// <summary>
        /// 违章基础信息查询    
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetLllegalBaseInfo(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;

            if (pagination.p_fields.IsEmpty())
            {
                pagination.p_fields = @"belongdepart,createuserdeptcode,createuserid,createusername,createdeptname,createdate,createtime,lllegalnumber,lllegaltype, lllegaltypename ,lllegaltime,lllegaldate,lllegallevel,
                lllegallevelname, lllegalperson,lllegalpersonid,lllegalteam,lllegalteamcode,lllegaldepart,lllegaldepartcode,lllegaldescribe,lllegaladdress,lllegalpic,reformpic,reformrequire,
                flowstate,addtype,isexposure,reformpeople,reformpeopleid,reformtel,reformdeptcode,reformdeptname,reformdeadline,reformdeadlinetime,reformfinishdate,reformfinishtime,
                reformstatus,reformmeasure,applicationstatus,postponedays,postponedept,postponedeptname,postponeperson,postponepersonname,acceptpeopleid,acceptpeople,acceptdeptname,acceptdeptcode,
                acceptresult,acceptmind,accepttime,reseverid,resevertype ,participant,isupsafety,reseverone,resevertwo,reseverthree,reseverfour,reseverfive,engineerid,engineername,lllegalfilepath,
                reformfilepath,participantname,curapprovedate,curacceptdate,beforeapprovedate,beforeacceptdate,afterapprovedate,afteracceptdate,verifydeptid,verifydeptname";
            }
            pagination.p_kid = "id";

            pagination.conditionJson = " 1=1";

            var queryParam = queryJson.ToJObject();

            //当前用户
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            pagination.p_tablename = @"( 
                                          select a.* ,b.filepath as lllegalfilepath ,c.filepath as reformfilepath,d.deptcode, (case when a.flowstate ='流程结束' then 1 else 0 end)  ordernumber   from v_lllegalallbaseinfo a
                                          left join v_imageview b on a.lllegalpic = b.recid  
                                          left join v_imageview c on a.reformpic = c.recid 
                                          left join base_department d on a.createdeptid = d.departmentid
                                      ) a";

            //台账标记
            if (!queryParam["standingmark"].IsEmpty())
            {
                pagination.conditionJson += @" and flowstate  != '违章登记' and flowstate  != '违章举报'";
            }
            //从统计过来的标记
            if (!queryParam["qtype"].IsEmpty())
            {
                pagination.conditionJson += @" and flowstate in (select itemname from v_yesqrwzstatus)";
                if (queryParam["qtype"].ToString() == "wb")
                {
                    pagination.conditionJson += string.Format(" and to_char(lllegaltime,'yyyy')='{0}' and lllegalpersonid in (select userid from base_user where isepiboly='1' and organizecode='{1}')", DateTime.Now.Year, user.OrganizeCode);
                }
                else if (queryParam["qtype"].ToString() == "wbzb")
                {
                    pagination.conditionJson += string.Format(" and lllegalpersonid in (select userid from base_user where isepiboly='1' and organizecode='{1}')", DateTime.Now.Year, user.OrganizeCode);
                }
            }
            string queryDeptOrgCode = "createuserorgcode";  //1为按照创建单位  0 为按照整改单位 
            string queryDeptCode = "deptcode";
            string choosetag = string.Empty;
            string action = string.Empty;
            int querybtntype = 1;

            if (!queryParam["querybtntype"].IsEmpty())
            {
                querybtntype = int.Parse(queryParam["querybtntype"].ToString());

                //按创建单位来
                if (querybtntype > 0)
                {
                    queryDeptOrgCode = "createuserorgcode";
                    queryDeptCode = "deptcode";

                    choosetag = queryParam["choosetag"].ToString();
                }
                else //按整改单位来
                {
                    queryDeptOrgCode = "createuserorgcode";
                    queryDeptCode = "reformdeptcode";
                }
            }

            if (queryParam["isOrg"].IsEmpty())
            {
                if (user.RoleName.Contains("集团") || user.RoleName.Contains("省级用户"))
                {
                    //省公司查看电厂数据
                    pagination.conditionJson += string.Format(@" and {0}  in (select encode from base_department start with encode='{1}' connect by  prior departmentid=parentid)", queryDeptOrgCode, user.OrganizeCode);
                }
                else
                {
                    //电厂可查看省公司登记的违章。
                    pagination.conditionJson += string.Format(@" and ({0}  in (select encode from base_department where (nature = '集团' or nature = '省级' or nature = '厂级') start with encode='{1}' connect by  prior departmentid=parentid) or belongdepartid='{2}')", queryDeptOrgCode, user.OrganizeCode, user.OrganizeId);
                }
            }

            //查询条件
            if (!queryParam["action"].IsEmpty())
            {
                action = queryParam["action"].ToString();

                switch (action)
                {
                    //违章登记
                    case "Register":
                        pagination.conditionJson += string.Format(@" and  createuserid ='{0}' ", user.UserId);
                        break;
                    //违章完善
                    case "Perfect":
                        pagination.conditionJson += @" and flowstate  = '违章完善'";
                        break;
                    //违章核准
                    case "Approve":
                        pagination.conditionJson += @" and flowstate in ('违章核准','违章审核')";
                        break;
                    case "PlanReform":
                        pagination.conditionJson += @" and flowstate  = '制定整改计划'";
                        break;
                    //违章整改
                    case "Reform":
                        pagination.conditionJson += @" and flowstate  = '违章整改'";
                        break;
                    //违章验收
                    case "Accept":
                        pagination.conditionJson += @" and flowstate  = '违章验收'";
                        break;
                    //验收确认
                    case "Confirm":
                        pagination.conditionJson += @" and flowstate  = '验收确认'";
                        break;
                    //流程结束
                    case "BaseEnd":
                        pagination.conditionJson += @" and flowstate  = '流程结束'";
                        break;
                    //未闭环隐患
                    case "NotClose":
                        pagination.conditionJson += @" and flowstate in (select itemname from v_yesqrwzstatus where itemname !='流程结束')";
                        break;
                    //当前年所有违章(厂级领导)
                    case "AllLllegal":
                        pagination.conditionJson += string.Format(@" and reformdeptcode  like  '{0}%'  and to_char(createdate,'yyyy')='{1}'  and flowstate in (select itemname from v_yesqrwzstatus) ", user.OrganizeCode, DateTime.Now.Year);
                        break;
                    //违章数量
                    case "LllegalNum":
                        string recode = string.Empty;
                        if (user.RoleName.Contains("厂级") || user.RoleName.Contains("公司级"))
                        {
                            recode = user.OrganizeCode;
                        }
                        else
                        {
                            recode = user.DeptCode;
                        }
                        pagination.conditionJson += string.Format(@" and reformdeptcode  like  '{0}%'  and flowstate in (select itemname from v_yesqrwzstatus) ", recode);
                        break;
                    //整改延期阶段的数据
                    case "Postpone":
                        pagination.conditionJson += @" and applicationstatus  = '1'";
                        break;
                    //违章整改确认
                    case "ReformAffirm":
                        pagination.conditionJson += string.Format(@" and flowstate  = '违章整改' and  participant||',' not like  '%{0}%'", user.Account + ',');
                        break;
                    //违章积分低于8分的人员
                    case "UnderEight":
                        string wzorgid = string.Empty;
                        string wzperson = string.Empty;
                        string wzids = string.Empty;
                        if (!queryParam["wzorgid"].IsEmpty())
                        {
                            wzorgid = queryParam["wzorgid"].ToString();
                        }
                        //违章考核人
                        if (!queryParam["wzperson"].IsEmpty())
                        {
                            wzperson = queryParam["wzperson"].ToString();
                        }
                        if (!string.IsNullOrEmpty(wzorgid) && !string.IsNullOrEmpty(wzperson))
                        {
                            //获取个人分数
                            decimal personScore = 12; //默认初始
                            var dataitem = idataitemdetailservice.GetEntityByItemName("LllegalPointInitValue"); // 个人默认违章积分
                            if (null != dataitem)
                            {
                                if (!string.IsNullOrEmpty(dataitem.ItemValue))
                                {
                                    personScore = Convert.ToDecimal(dataitem.ItemValue);//获取个人默认违章积分 
                                }
                            }
                            string wzdyeSql = string.Format(@"select ids from (
                                                                  select ({0} - sum(a.lllegalpoint))  personscore,a.userid,wm_concat(to_char(a.id)) ids  from v_lllegalassesforperson  a
                                                                  left join (
                                                                    select count(1) pnum ,recoveruserid,createdate  from v_lllegalpointrecoverdetail group by recoveruserid,createdate 
                                                                  ) b on a.userid = b.recoveruserid  
                                                                where  a.organizeid='{1}'  and  a.userid ='{2}'  and  to_char(a.lllegaltime,'yyyy') = '{3}' and (nvl(b.pnum,0)=0  or  (nvl(b.pnum,0)>0  and  a.createdate > b.createdate)) group by a.realname,a.userid
                                                                ) a  where personscore<=8 ", personScore, wzorgid, wzperson, DateTime.Now.Year.ToString());

                            var wzdyDt = this.BaseRepository().FindTable(wzdyeSql);
                            if (wzdyDt.Rows.Count == 1)
                            {
                                wzids = wzdyDt.Rows[0]["ids"].ToString();
                            }
                        }
                        if (!string.IsNullOrEmpty(wzids))
                        {
                            wzids = "'" + wzids.Replace(",", "','") + "'";
                            pagination.conditionJson += string.Format(@" and id in ({0})", wzids);
                        }
                        break;
                }
            }
            //违章状态
            #region 违章状态
            if (!queryParam["reformstatus"].IsEmpty())
            {
                switch (queryParam["reformstatus"].ToString())
                {
                    case "未整改":
                        pagination.conditionJson += @" and flowstate = '违章整改' ";
                        break;
                    case "逾期未核准":
                        pagination.conditionJson += string.Format(@" and flowstate = '违章核准'  and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') > afterapprovedate", DateTime.Now);
                        break;
                    case "即将到期未核准":
                        pagination.conditionJson += string.Format(@" and flowstate = '违章核准'  and   to_date('{0}','yyyy-mm-dd hh24:mi:ss') >= beforeapprovedate  and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') <= afterapprovedate ", DateTime.Now);
                        break;
                    case "逾期未审核":
                        pagination.conditionJson += string.Format(@" and flowstate = '违章审核'  and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') > afterapprovedate", DateTime.Now);
                        break;
                    case "即将到期未审核":
                        pagination.conditionJson += string.Format(@" and flowstate = '违章审核'  and   to_date('{0}','yyyy-mm-dd hh24:mi:ss') >= beforeapprovedate  and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') <= afterapprovedate ", DateTime.Now);
                        break;
                    case "逾期未整改":
                        pagination.conditionJson += string.Format(@" and flowstate = '违章整改'  and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') > reformdeadline + 1", DateTime.Now);
                        break;
                    case "即将到期未整改":
                        pagination.conditionJson += string.Format(@" and flowstate = '违章整改' and (reformdeadline - 3 <= to_date('{0}','yyyy-mm-dd hh24:mi:ss')   and to_date('{0}','yyyy-mm-dd hh24:mi:ss')  <= reformdeadline + 1)", DateTime.Now);
                        break;
                    case "逾期未验收":
                        pagination.conditionJson += string.Format(@" and flowstate = '违章验收'  and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') > afteracceptdate", DateTime.Now);
                        break;
                    case "即将到期未验收":
                        pagination.conditionJson += string.Format(@" and flowstate = '违章验收'  and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') >= beforeacceptdate   and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') <= afteracceptdate ", DateTime.Now);
                        break;
                    case "本人登记":
                        pagination.conditionJson += string.Format(@" and  createuserid ='{0}'", user.UserId);
                        break;
                    case "本人举报":
                        pagination.conditionJson += string.Format(@" and  createuserid ='{0}'", user.UserId);
                        break;
                    case "已整改":
                        pagination.conditionJson += @" and  flowstate in (select itemname from v_yeszgwzstatus)";
                        break;
                    case "未闭环":
                        pagination.conditionJson += @" and  flowstate in (select itemname from v_yesqrwzstatus where itemname !='流程结束')";
                        break;
                    case "已闭环":
                        pagination.conditionJson += @" and  flowstate  = '流程结束'";
                        break;
                }
            }
            #endregion
            //数据范围
            #region 数据范围
            if (!queryParam["datascope"].IsEmpty())
            {
                string departmentCode = string.Empty;
                switch (queryParam["datascope"].ToString())
                {
                    case "本人登记":
                        pagination.conditionJson += string.Format(@" and  createuserid ='{0}'", user.UserId);
                        break;
                    case "本人举报":
                        pagination.conditionJson += string.Format(@" and  createuserid ='{0}'", user.UserId);
                        break;
                    case "本人完善":
                        pagination.conditionJson += string.Format(@" and  participant||','  like  '%{0}%' and flowstate = '违章完善'", user.Account + ',');
                        break;
                    case "本部门完善":
                        string deptCode = user.DeptCode;
                        pagination.conditionJson += string.Format(@" and  id in  (select distinct lllegalid from v_lllegalapprovaldata where departmentcode ='{0}' and name ='违章完善')", deptCode);
                        break;
                    case "本人核准":
                        pagination.conditionJson += string.Format(@" and  participant||','  like  '%{0}%' and flowstate = '违章核准'", user.Account + ',');
                        break;
                    case "本部门核准":
                        departmentCode = user.DeptCode;
                        pagination.conditionJson += string.Format(@" and  id in  (select distinct lllegalid from v_lllegalapprovaldata where departmentcode ='{0}' and name ='违章核准')", departmentCode);
                        break;
                    case "本人审核":
                        pagination.conditionJson += string.Format(@" and  participant||','  like  '%{0}%' and flowstate = '违章审核'", user.Account + ',');
                        break;
                    case "本部门审核":
                        departmentCode = user.DeptCode;
                        pagination.conditionJson += string.Format(@" and  id in  (select distinct lllegalid from v_lllegalapprovaldata where departmentcode ='{0}' and name ='违章审核')", departmentCode);
                        break;
                    case "本人整改":
                        pagination.conditionJson += string.Format(@" and reformpeopleid like '%{0}%' and flowstate = '违章整改'", user.UserId);
                        break;
                    case "本人制定整改计划":
                        pagination.conditionJson += string.Format(@" and  participant||','  like  '%{0}%' and flowstate = '制定整改计划'", user.Account + ',');
                        break;
                    case "本部门整改":
                        pagination.conditionJson += string.Format(@" and reformdeptcode  =  '{0}' and flowstate = '违章整改'", user.DeptCode);
                        break;
                    case "本人验收":
                        pagination.conditionJson += string.Format(@" and acceptpeopleid  like  '%{0}%' and flowstate = '违章验收'", user.UserId);
                        break;
                    case "本部门验收":
                        pagination.conditionJson += string.Format(@" and acceptdeptcode  =  '{0}' and flowstate = '违章验收' ", user.DeptCode);
                        break;
                    case "本单位验收":
                        pagination.conditionJson += string.Format(@" and acceptdeptcode  =  '{0}' and flowstate = '违章验收' ", user.DeptCode);
                        break;
                    case "本人确认":
                        pagination.conditionJson += string.Format(@" and  participant||','  like  '%{0}%' and flowstate = '验收确认'", user.Account + ',');
                        break;
                    case "本部门确认":
                        string deptCfmCode = user.DeptCode;
                        pagination.conditionJson += string.Format(@" and  id in  (select distinct lllegalid from v_lllegalapprovaldata where departmentcode ='{0}' and name ='验收确认')", deptCfmCode);
                        break;
                    case "本人审(核)批": //整改延期申请审批
                        pagination.conditionJson += string.Format(@" and  (applicationstatus ='1' and postponeperson  like  '%,{0},%')", user.Account);
                        break;
                    case "本部门审(核)批"://部门整改延期申请审批
                        pagination.conditionJson += string.Format(@"  and  (applicationstatus ='1' and postponedept  like  '%,{0},%')", user.DeptCode);
                        break;
                }
            }
            #endregion

            //整改部门编码
            if (!queryParam["qdeptcode"].IsEmpty())
            {
                //来自于--未闭环统计
                if (action == "NotClose")
                {
                    string tcode = queryParam["qdeptcode"].ToString();  //old部门编码

                    var newdeptcode = Idepartmentservice.GetEntityByCode(tcode); //获取对应的部门信息

                    if (newdeptcode.Nature == "厂级")
                    {
                        pagination.conditionJson += string.Format(@" and reformdeptcode = '{0}'", newdeptcode.EnCode);
                    }
                    else
                    {
                        pagination.conditionJson += string.Format(@" and reformdeptcode like '{0}%'", newdeptcode.EnCode);
                    }
                }
                else
                {
                    pagination.conditionJson += string.Format(@" and reformdeptcode like '{0}%'", queryParam["qdeptcode"].ToString());
                }
            }
            //创建部门编码
            if (!queryParam["createdeptcode"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and createuserdeptcode like '{0}%'", queryParam["createdeptcode"].ToString());
            }
            //创建人
            if (!queryParam["createuserid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and createuserid = '{0}'", queryParam["createuserid"].ToString());
            }
            //台账类型
            if (!queryParam["hidstandingtype"].IsEmpty())
            {
                string standingtype = queryParam["hidstandingtype"].ToString();

                pagination.conditionJson += @" and flowstate != '违章核准' and flowstate != '违章审核'";

                if (standingtype.Contains("公司级"))
                {
                    pagination.conditionJson += @" and  (rolename  like  '%公司级%' or   rolename  like  '%厂级部门用户%') ";
                }
                else
                {
                    pagination.conditionJson += string.Format(@" and rolename  like  '%{0}%'  and  rolename not like '%厂级%' ", standingtype);
                }
            }
            //外包工程id
            if (!queryParam["engineerid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and engineerid = '{0}'", queryParam["engineerid"].ToString());
            }
            //流程状态
            if (!queryParam["flowstate"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and flowstate = '{0}'", queryParam["flowstate"].ToString());
            }
            //违章类型
            if (!queryParam["lllegaltype"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and  lllegaltype='{0}' ", queryParam["lllegaltype"].ToString());
            }
            //违章级别
            if (!queryParam["lllegallevel"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and lllegallevel ='{0}'", queryParam["lllegallevel"].ToString());
            }
            //违章级别
            if (!queryParam["lllegallevelname"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and lllegallevelname ='{0}'", queryParam["lllegallevelname"].ToString());
            }
            //违章描述 
            if (!queryParam["lllegaldescribe"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and lllegaldescribe like '%{0}%'", queryParam["lllegaldescribe"].ToString());
            }
            //所属单位 
            if (!queryParam["lllegaldeptid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and belongdepartid = '{0}'", queryParam["lllegaldeptid"].ToString());
            }
            //违章时间开始时间
            if (!queryParam["starttime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and lllegaltime >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", queryParam["starttime"].ToString());
            }
            //违章时间结束时间
            if (!queryParam["endtime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and lllegaltime < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(queryParam["endtime"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
            }
            //创建时间开始时间
            if (!queryParam["stdate"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and createdate >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", queryParam["stdate"].ToString());
            }
            //创建时间结束时间
            if (!queryParam["etdate"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and createdate < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(queryParam["etdate"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
            }
            //违章部门
            if (!queryParam["deptcode"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and  lllegalteamcode in (select encode from base_department start with encode='{0}' connect by  prior departmentid = parentid)", queryParam["deptcode"].ToString());
            }
            //违章考核人
            if (!queryParam["wzperson"].IsEmpty())
            {
                if (!queryParam["qtype"].IsEmpty() && !queryParam["year"].IsEmpty())
                {
                    if (queryParam["qtype"].ToString() == "ryjf")
                    {
                        pagination.conditionJson += string.Format(@" and  id  in (select  b.id from bis_lllegalpunish a 
                                                            left join bis_lllegalregister b on a.lllegalid = b.id 
                                                            left join (select count(1) pnum ,recoveruserid userid,createdate  from v_lllegalpointrecoverdetail group by recoveruserid,createdate)  c on a.personinchargeid = c.userid 
                                                            where  a.personinchargeid is not null and  b.flowstate in (select itemname from v_yesqrwzstatus) and (nvl(c.pnum,0)=0  or (nvl(c.pnum,0)>0  and  b.createdate > c.createdate ))  
                                                            and  to_char(b.lllegaltime, 'yyyy')='{0}' and a.personinchargeid ='{1}')", queryParam["year"].ToString(), queryParam["wzperson"].ToString());
                    }
                }
                else
                {
                    pagination.conditionJson += string.Format(@" and  id  in (select id from  v_lllegalassesforperson  where userid ='{0}')", queryParam["wzperson"].ToString());
                }
               
            }
            //违章奖励人
            if (!queryParam["awardperson"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and  id  in (select lllegalid from  bis_lllegalawarddetail  where userid ='{0}')", queryParam["awardperson"].ToString());
            }
            //违章年度
            if (!queryParam["year"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and to_char(lllegaltime,'yyyy') = '{0}'", queryParam["year"].ToString());
            }
            //违章奖励状态 
            if (!queryParam["status"].IsEmpty())
            {
                if (queryParam["status"].ToString() == "已确认")
                {
                    pagination.conditionJson += string.Format(@" and id in (select distinct lllegalid from bis_lllegalreward where status='已确认')");
                }
                else
                {
                    pagination.conditionJson += string.Format(@" and id not in (select distinct lllegalid from bis_lllegalreward where status='已确认')");
                }
            }
            //违章考核人(MLGB西塞山特有)
            if (!queryParam["wzkhuserid"].IsEmpty())
            {
                string strWhere = string.Empty;
                //公司级  厂级
                if (user.RoleName.Contains("公司级") || user.RoleName.Contains("厂级"))
                {
                    strWhere += @" and  lllegalid  in (select distinct objectid from v_xsslllegalpointsdata where  rolename  like '%公司级%' or  rolename like '%厂级%')";
                }
                //厂级
                if (user.RoleName.Contains("部门级") && !user.RoleName.Contains("厂级"))
                {
                    strWhere += string.Format(@" and  lllegalid  in (select distinct objectid from v_xsslllegalpointsdata where  rolename  like '%部门级%' and   rolename  not like '%厂级%'  and  encode ='{0}')", user.DeptCode);
                }
                //班组级
                if (user.RoleName.Contains("班组级"))
                {
                    strWhere += string.Format(@" and  lllegalid in (select distinct objectid from v_xsslllegalpointsdata where  rolename  like '%班组级%' and  encode ='{0}')", user.DeptCode);
                }

                pagination.conditionJson += string.Format(@" and id in  (select distinct lllegalid from bis_lllegalpunish where personinchargeid = '{0}'  {1}) ", queryParam["wzkhuserid"].ToString(), strWhere);
            }

            //创建年度
            if (!queryParam["qyear"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and to_char(createdate,'yyyy') = '{0}'", queryParam["qyear"].ToString());
            }
            //创建年月
            if (!queryParam["qyearmonth"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and to_char(createdate,'yyyy-MM') = '{0}'", queryParam["qyearmonth"].ToString());
            }
            //违章月份
            if (!queryParam["month"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and extract(month from lllegaltime) = '{0}'", queryParam["month"].ToString());
            }
            //违章曝光
            if (!queryParam["isexposure"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and isexposure = '{0}'", queryParam["isexposure"].ToString());
            }
            //关联id 1
            if (!queryParam["reseverid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and reseverid = '{0}'", queryParam["reseverid"].ToString());
            }
            //关联id 1
            if (!queryParam["resevertype"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and resevertype = '{0}'", queryParam["resevertype"].ToString());
            }
            //关联id 1
            if (!queryParam["reseverone"].IsEmpty())
            {
                string ckId = queryParam["reseverone"].ToString();
                string ids = GetCheckIds(ckId);
                if (!queryParam["pfrom"].IsEmpty())
                {
                    string pfrom = queryParam["pfrom"].ToString();
                    if (pfrom == "0")
                    {
                        pagination.conditionJson += string.Format(@" and (reseverone in('{1}') or reseverone='{0}')", ckId, ids.Replace(",", "','"));
                    }
                    if (pfrom == "1")
                    {
                        string dutydept = BaseRepository().FindObject(string.Format("select dutydept from bis_saftycheckdatarecord where id='{0}'", ckId)).ToString();
                        pagination.conditionJson += string.Format(@" and (reseverone in('{1}') or reseverone='{0}')", ckId, ids.Replace(",", "','"));
                    }
                }
                else
                {
                    pagination.conditionJson += string.Format(@" and reseverone = '{0}'", queryParam["reseverone"].ToString());
                }
            }
            //关联id 2 
            if (!queryParam["resevertwo"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and resevertwo = '{0}'", queryParam["resevertwo"].ToString());
            }
            //关联id 3 
            if (!queryParam["reseverthree"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and reseverthree = '{0}'", queryParam["reseverthree"].ToString());
            }
            if (!queryParam["isOrg"].IsEmpty())
            {

                string isOrg = queryParam["isOrg"].ToString();
                string code = string.Empty;
                if (!queryParam["code"].IsEmpty())
                {
                    string oldcode = queryParam["code"].ToString();
                    var dpt = new DepartmentService().GetEntityByCode(oldcode);
                    //按创建单位 
                    if (querybtntype == 1)
                    {
                        code = dpt.DeptCode;
                    }
                    else
                    {
                        code = oldcode;
                    }

                    var nature = dpt.Nature;

                    //本单位
                    if (choosetag == "0")
                    {
                        //非省级用户,只能看自己电厂的
                        if (user.RoleName.Contains("省级用户"))
                        {
                            pagination.conditionJson += string.Format(@" and {0} = '{1}'  and  belongdepartid = '{2}'", queryDeptCode, code, user.OrganizeId);
                        }
                        else
                        {
                            pagination.conditionJson += string.Format(@" and {0} = '{1}' ", queryDeptCode, code);
                        }
                    }
                    else
                    {
                        //非省级用户,只能看自己电厂的
                        if (user.RoleName.Contains("省级用户"))
                        {
                            pagination.conditionJson += string.Format(@" and {0} like '{1}%'", queryDeptCode, code);
                        }
                        else
                        {
                            pagination.conditionJson += string.Format(@" and {0} like '{1}%' and  belongdepartid = '{2}'", queryDeptCode, code, user.OrganizeId);
                        }
                    }
                }
            }
            if (!queryParam["regcode"].IsEmpty())
            {
                //省公司或电厂登记
                var regcode = queryParam["regcode"].ToString();
                if (regcode == "本单位登记")
                {
                    pagination.conditionJson += " and isgrpaccept is not null";
                }
                else if (regcode == "电厂登记")
                {
                    pagination.conditionJson += " and isgrpaccept is null";
                }
            }

            //西塞山台账
            if (!queryParam["specialmark"].IsEmpty())
            {
                string specialmark = queryParam["specialmark"].ToString();

                if (specialmark == "xss")
                {

                    string idsql = string.Format(@"  or  id  in  (select distinct  objectid from v_xsslllegalstandingbook where  encode ='{0}' )", user.DeptCode);

                    //厂级、公司层级
                    if (user.RoleName.Contains("厂级") || user.RoleName.Contains("公司级"))
                    {
                        pagination.conditionJson += string.Format(@" and  (rolename  like  '%公司级%' or   rolename  like  '%厂级部门用户%'  {0}) ", idsql);
                    }
                    //部门层级
                    else if (user.RoleName.Contains("部门级") && !user.RoleName.Contains("厂级"))
                    {
                        pagination.conditionJson += string.Format(@" and  (rolename  like  '%公司级%' or   rolename  like  '%厂级部门用户%'  or  createuserdeptcode  ='{0}' {1})  ", user.DeptCode, idsql);
                    }
                    //班组层级
                    else if (user.RoleName.Contains("班组级"))
                    {
                        pagination.conditionJson += string.Format(@" and  (rolename  like  '%公司级%' or   rolename  like  '%厂级部门用户%'  or  createuserdeptcode in 
                        (select encode from base_department   where nature = '部门' start with encode='{0}' connect by  prior parentid  = departmentid)  or  createuserdeptcode  ='{0}' {1})  ", user.DeptCode, idsql);
                    }
                    //承包商层级
                    else if (user.RoleName.Contains("承包商"))
                    {
                        //当前用户外包工程不为空时
                        if (!string.IsNullOrEmpty(user.ProjectID))
                        {
                            pagination.conditionJson += string.Format(@" and  (rolename  like  '%公司级%' or   rolename  like  '%厂级部门用户%'  or  createuserdeptcode in 
                             (select a.encode from base_department a, epg_outsouringengineer  b   where a.nature = '部门'  and a.departmentid = b.engineerusedeptid and b.id ='{0}')  or  createuserdeptcode  ='{1}'  {2})", user.ProjectID, user.DeptCode, idsql);
                        }
                        else
                        {
                            pagination.conditionJson += string.Format(@" and  (rolename  like  '%公司级%' or   rolename  like  '%厂级部门用户%'  or  createuserdeptcode in 
                             (select a.encode from base_department a, epg_outsouringengineer  b   where a.nature = '部门'  and a.departmentid = b.engineerusedeptid and b.outprojectid ='{0}'  and b.createuserorgcode='{1}')  or  createuserdeptcode  ='{2}'  {3})", user.ProjectID, user.OrganizeCode, user.DeptCode, idsql);
                        }
                    }
                }
            }

            var dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);

            return dt;
        }
        #endregion

        #region 违章实体所有元素对象
        /// <summary>
        /// 违章实体所有元素对象
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public DataTable GetLllegalModel(string keyValue)
        {
            DataTable dt = new DataTable();
            string sql = string.Format(@"select * from  v_lllegalallbaseinfo where id ='{0}' ", keyValue);
            dt = this.BaseRepository().FindTable(sql);
            return dt;
        }
        #endregion



        /// <summary>
        /// 获取个人(反)违章档案
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable GetLllegalForPersonRecord(string userId)
        {
            try
            {
                //string sql = string.Format(@"with wzinfo as (
                //                                  select a.lllegaldescribe,a.createdate ,a.lllegaltime ,a.id ,b.points,b.money,0 EconomicsPunish , 0 LllegalPoint, b.userid from bis_lllegalregister a inner join bis_lllegalawarddetail b on a.id = b.lllegalid  
                //                                  where a.flowstate in (select itemname from v_yesqrwzstatus) and b.userid is not null 
                //                                  union
                //                                  select a.lllegaldescribe,a.createdate ,a.lllegaltime ,a.id ,0 points,0 money,b.EconomicsPunish , b.LllegalPoint, b.personinchargeid userid from bis_lllegalregister a inner join bis_lllegalpunish b on a.id = b.lllegalid  
                //                                  where a.flowstate in (select itemname from v_yesqrwzstatus)  and b.personinchargeid is not null 
                //                                )
                //                                select  sum(points) points ,sum(money) money,sum(EconomicsPunish) EconomicsPunish,sum(LllegalPoint) LllegalPoint,lllegaldescribe,userid,to_char(lllegaltime,'yyyy-MM-dd') lllegaltime from wzinfo where 1=1 and userid='{0}' group by lllegaldescribe,lllegaltime,userid order by lllegaltime desc ", userId);
                string sql = string.Format(@"with basewz as (
                                              select a.id,a.addtype,a.flowstate,a.lllegalnumber,a.createdate,to_char(a.lllegaltime,'yyyy-MM-dd') lllegaltime,a.lllegaltype,c.itemname lllegaltypename ,a.lllegallevel,d.itemname lllegallevelname,
                                              a.lllegaladdress,a.lllegaldescribe,a.lllegalteam,b.reformmeasure,a.reformrequire,a.lllegalpersonid,a.createuserid,nvl(e.EconomicsPunish,0) EconomicsPunish,nvl(e.LllegalPoint,0) LllegalPoint,nvl(f.points,0) points,nvl(f.money,0) money  from bis_lllegalregister a
                                              left join v_lllegalreforminfo b on a.id =b.lllegalid 
                                              left join base_dataitemdetail  c on a.lllegaltype = c.itemdetailid
                                              left join base_dataitemdetail  d on a.lllegallevel =d.itemdetailid 
                                              left join (select sum(EconomicsPunish) EconomicsPunish,sum(LllegalPoint) LllegalPoint, lllegalid from bis_lllegalpunish  where personinchargeid='{0}' group by lllegalid ) e on a.id =e.lllegalid
                                              left join (select sum(points) points,sum(money) money,lllegalid from bis_lllegalawarddetail where userid='{0}'  group by lllegalid ) f on a.id =f.lllegalid 
                                              where a.flowstate in (select itemname from v_yesqrwzstatus) 
                                            )
                                        select (case when lllegalpersonid ='{0}' then '本人违章' when  createuserid='{0}' then '本人登记' else '' end) actiontype,
                                        a.id,a.addtype,a.flowstate,a.lllegalnumber,a.createdate,a.lllegaltime,a.lllegaltype,a.lllegaltypename,a.lllegallevel,a.lllegallevelname,a.lllegaladdress,a.lllegaldescribe,a.lllegalteam,
                                        a.reformmeasure,a.reformrequire,a.lllegalpersonid,a.createuserid,a.EconomicsPunish,a.LllegalPoint,a.points,a.money from basewz a  where lllegalpersonid='{0}'
                                         or createuserid ='{0}' order by lllegaltime desc", userId);
                return this.BaseRepository().FindTable(sql);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region 获取违章曝光
        /// <summary>
        /// 获取违章曝光
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public DataTable QueryExposureLllegal(string num)
        {
            string sql = string.Empty;
            DataItemDetailService dataitemdetailservice = new DataItemDetailService();
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            sql = string.Format(@" select a.* from (
                                        select distinct a.id ,a.id lllegalid,a.lllegalnumber ,a.reformrequire,a.lllegaldescribe,a.createdate,a.flowstate,a.addtype,a.reformdeptcode, f.filepath  filepic, (case when f.filepath is not null then ('{2}'||substr(f.filepath,2)) else '' end)  filepath ,a.createuserorgcode from v_lllegalbaseinfo a
                                        left join v_imageview f on a.lllegalpic = f.recid  where a.isexposure ='1' and  a.reformdeptcode like '{1}%' 
                                  ) a where rownum <= {0} order by createdate  ", int.Parse(num), user.OrganizeCode, dataitemdetailservice.GetItemValue("imgUrl"));

            var dt = this.BaseRepository().FindTable(sql);

            return dt;
        }
        #endregion

        #region 获取违章档案(班组端)
        /// <summary>
        /// 获取违章档案(班组端)
        /// </summary>
        /// <param name="year"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public LllegalRecord GetLllegalRecord(string userid, string year)
        {
            LllegalRecord entity = new LllegalRecord();
            List<LllegalModel> data = new List<LllegalModel>();
            try
            {
                var lllegalPoint = idataitemdetailservice.GetDataItemListByItemCode("'LllegalTrainPointSetting'");
                string basePoint = lllegalPoint.Where(p => p.ItemName == "LllegalPointInitValue").FirstOrDefault().ItemValue; //基础分数值
                string sql = string.Empty;
                decimal totalscore = Convert.ToDecimal(basePoint); //总分
                decimal lastscore = 0;
                //违章总数
                sql = string.Format(@"select a.id,to_char(a.lllegaltime,'yyyy-MM-dd') lllegaltime,a.lllegaladdress,a.lllegaldescribe, to_char(sum(a.lllegalpoint))  lllegalpoint,to_char(sum(a.economicspunish)) economicspunish  from( 
                                    select a.id,a.lllegaltime,a.lllegaladdress,a.lllegaldescribe, sum(nvl(b.lllegalpoint,0)) lllegalpoint,sum(nvl(b.economicspunish,0)) economicspunish  from v_lllegalbaseinfo a , bis_lllegalpunish b where a.id = b.lllegalid 
                                    and  b.personinchargeid='{0}'  and b.assessobject ='考核人员' and to_char(a.createdate,'yyyy')='{1}' group by a.id,a.lllegaltime,a.lllegaladdress,a.lllegaldescribe
                                    union
                                    select a.id,a.lllegaltime,a.lllegaladdress,a.lllegaldescribe ,0 lllegalpoint ,0 economicspunish from v_lllegalbaseinfo a  where lllegalpersonid ='{0}' and to_char(a.createdate,'yyyy')='{1}'
                                 ) a  group by a.id,a.lllegaltime,a.lllegaladdress,a.lllegaldescribe order by lllegaltime desc ", userid, year);

                DataTable dt = this.BaseRepository().FindTable(sql);
                int lllegalcount = 0; //个人违章次数
                decimal deductmarks = 0; //个人违章扣分
                decimal penalty = 0; //个人违章罚款总数
                foreach (DataRow row in dt.Rows)
                {
                    LllegalModel brmodel = new LllegalModel();
                    brmodel.lllegalid = row["id"].ToString();
                    brmodel.lllegaltime = row["lllegaltime"].ToString();
                    brmodel.lllegaladdress = row["lllegaladdress"].ToString();
                    brmodel.lllegalpoint = row["lllegalpoint"].ToString();
                    brmodel.economicspunish = row["economicspunish"].ToString();
                    brmodel.lllegalduty = "本人违章";
                    lllegalcount += 1;
                    deductmarks += Convert.ToDecimal(row["lllegalpoint"].ToString());
                    penalty += Convert.ToDecimal(row["economicspunish"].ToString());
                    data.Add(brmodel);
                }
                entity.lllegalcount = lllegalcount.ToString();  //个人违章次数
                entity.deductmarks = deductmarks.ToString(); //个人违章扣分
                lastscore = totalscore - deductmarks; //最后得分
                entity.penalty = penalty.ToString(); //个人违章罚款总数

                //违章总数
                sql = string.Format(@" select a.id,to_char(a.lllegaltime,'yyyy-MM-dd') lllegaltime,a.lllegaladdress,a.lllegaldescribe, sum(nvl(b.lllegalpoint,0)) lllegalpoint,sum(nvl(b.economicspunish,0)) economicspunish  from v_lllegalbaseinfo a , bis_lllegalpunish b where a.id = b.lllegalid 
                                    and  b.personinchargeid='{0}'  and b.assessobject ='联责人员' and to_char(a.createdate,'yyyy')='{1}' group by a.id,a.lllegaltime,a.lllegaladdress,a.lllegaldescribe order by lllegaltime desc", userid, year);

                DataTable lddt = this.BaseRepository().FindTable(sql);
                int relatedcount = 0; //联责违章次数
                decimal relatedpoint = 0; //联责违章扣分
                decimal relatedpenalty = 0; //联责违章罚款总数
                foreach (DataRow row in lddt.Rows)
                {
                    LllegalModel ldmodel = new LllegalModel();
                    ldmodel.lllegalid = row["id"].ToString();
                    ldmodel.lllegaltime = row["lllegaltime"].ToString();
                    ldmodel.lllegaladdress = row["lllegaladdress"].ToString();
                    ldmodel.lllegalpoint = row["lllegalpoint"].ToString();
                    ldmodel.economicspunish = row["economicspunish"].ToString();
                    ldmodel.lllegalduty = "连带违章";
                    relatedcount += 1;
                    relatedpoint += Convert.ToDecimal(row["lllegalpoint"].ToString());
                    relatedpenalty += Convert.ToDecimal(row["economicspunish"].ToString());
                    data.Add(ldmodel);
                }
                entity.data = data;  //连带对象
                entity.relatedcount = relatedcount.ToString();  //联责违章次数
                entity.relatedpoint = relatedpoint.ToString(); //联责违章扣分
                lastscore = lastscore - relatedpoint; //最后得分
                entity.relatedpenalty = relatedpenalty.ToString(); //联责违章罚款总数

                entity.year = year.ToString(); //年度
                entity.residuePoint = lastscore.ToString(); //剩余积分
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return entity;
        }
        #endregion

        #region 获取新编码
        /// <summary>
        /// 获取新编码
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="maxfields"></param>
        /// <param name="seriallen"></param>
        /// <returns></returns>
        public string GenerateHidCode(string tablename, string maxfields, int seriallen)
        {
            string code = "";

            string newCode = "";

            try
            {

                string lastValue = DateTime.Now.ToString("yyyyMMdd");

                string sql = string.Format("select max(cast(substr({0},length({0})-({2}-1),{2}) as number)) from {1} t where {0} like '{3}%'", maxfields, tablename, seriallen, lastValue);

                object obj = this.BaseRepository().FindObject(sql);

                string num = obj == null || obj == DBNull.Value ? "1" : (int.Parse(obj.ToString()) + 1).ToString();

                string str = "";

                //最大值小于流水号长度
                if (num.Length < 3)
                {
                    for (int j = 0; j < 3 - num.Length; j++)
                    {
                        str += "0";
                    }
                }
                code = str + num;

                newCode = lastValue + code;
            }
            catch (Exception)
            {
                throw;
            }
            return newCode;
        }
        #endregion


        #region 通过安全检查id获取对应的违章统计数据
        /// <summary>
        /// 通过安全检查id获取对应的违章统计数据
        /// </summary>
        /// <param name="checkids"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public DataTable GetLllegalBySafetyCheckIds(List<string> checkids, int mode)
        {
            DataTable dt = new DataTable();
            string sql = string.Empty;
            string csql = string.Empty;
            string ids = string.Empty;

            foreach (string checkid in checkids)
            {
                ids += "'" + checkid + "',";
            }
            if (!string.IsNullOrEmpty(ids))
            {
                ids = ids.Substring(0, ids.Length - 1);
            }
            switch (mode)
            {
                case 0:
                    #region 违章考核总表
                    sql = string.Format(@"select rownum, a.* from (select to_char(a.createdate,'yyyy-MM-dd') createdate ,a.lllegaldescribe,a.lllegaltypename, a.lllegalteam,a.lllegaldepart,a.lllegalteamcode,a.lllegaldepartcode,'' expteam,'' expdepart,
                                '' expteamcode ,'' expdepartcode ,0 wwpunish,0 bmpunish,0 total, b.nature wzdwnature,c.nature wzzrdwnature,d.fullname wwparent,e.fullname bmparent,'' glbmname, a.id from  v_lllegalbaseinfo a
                                left join base_department b on a.lllegalteamcode = b.encode 
                                left join base_department c on a.lllegaldepartcode = c.encode 
                                left join base_department d on b.parentid = d.departmentid
                                left join base_department e on c.parentid = e.departmentid
                                where a.reseverone in ({0}) or a.reseverid in ({0}) order by a.createdate)  a", ids);

                    dt = this.BaseRepository().FindTable(sql);

                    foreach (DataRow row in dt.Rows)
                    {
                        string wzdwnature = row["wzdwnature"].ToString();
                        string wzzrdwnature = row["wzzrdwnature"].ToString();
                        if (wzdwnature == "承包商" && wzzrdwnature != "承包商")
                        {
                            row["expteam"] = row["lllegalteam"].ToString();
                            row["expdepart"] = row["lllegaldepart"].ToString();
                            row["expteamcode"] = row["lllegalteamcode"].ToString();
                            row["expdepartcode"] = row["lllegaldepartcode"].ToString();
                        }
                        else if (wzdwnature == "承包商" && wzzrdwnature == "承包商")
                        {
                            row["expteam"] = row["lllegalteam"].ToString() + "," + row["lllegaldepart"].ToString();
                            row["expteamcode"] = row["lllegalteamcode"].ToString() + "," + row["lllegaldepartcode"].ToString();

                            if (row["lllegalteamcode"].ToString() == row["lllegaldepartcode"].ToString())
                            {
                                row["expteam"] = row["lllegalteam"].ToString();
                                row["expteamcode"] = row["lllegalteamcode"].ToString();
                            }
                        }
                        else if (wzdwnature != "承包商" && wzzrdwnature == "承包商")
                        {
                            row["expteam"] = row["lllegaldepart"].ToString();
                            row["expdepart"] = row["lllegalteam"].ToString();
                            row["expteamcode"] = row["lllegaldepartcode"].ToString();
                            row["expdepartcode"] = row["lllegalteamcode"].ToString();
                        }
                        else if (wzdwnature != "承包商" && string.IsNullOrEmpty(wzzrdwnature))
                        {
                            row["expdepart"] = row["lllegalteam"].ToString();
                            row["expdepartcode"] = row["lllegalteamcode"].ToString();
                        }
                        else if (wzdwnature != "承包商" && !string.IsNullOrEmpty(wzzrdwnature) && wzzrdwnature != "承包商")
                        {
                            row["expdepart"] = row["lllegalteam"].ToString() + "," + row["lllegaldepart"].ToString();
                            row["expdepartcode"] = row["lllegalteamcode"].ToString() + "," + row["lllegaldepartcode"].ToString();

                            if (row["lllegalteamcode"].ToString() == row["lllegaldepartcode"].ToString())
                            {
                                row["expdepart"] = row["lllegalteam"].ToString();
                                row["expdepartcode"] = row["lllegalteamcode"].ToString();
                            }
                        }
                        string expteamcode = row["expteamcode"].ToString();
                        string expdepartcode = row["expdepartcode"].ToString();

                        //外委考核
                        if (!string.IsNullOrEmpty(expteamcode))
                        {
                            //外委单位对应的管理部门
                            var glsql = string.Format(@" select n.engineerletdept from epg_outsouringengineer n
                                 where n.outprojectid in (select departmentid from base_department t where t.encode in ({0}))", expteamcode);

                            var glbmdt = this.BaseRepository().FindTable(glsql);

                            if (glbmdt.Rows.Count > 0)
                            {
                                string glbmname = string.Empty;
                                foreach (DataRow glbmRow in glbmdt.Rows)
                                {
                                    glbmname += glbmRow["engineerletdept"].ToString() + ",";
                                }
                                if (!string.IsNullOrEmpty(glbmname))
                                {
                                    row["glbmname"] = glbmname.Substring(0, glbmname.Length - 1);
                                }
                            }

                            //外委考核金额
                            expteamcode = "," + expteamcode + ",";
                            csql = string.Format(@" select  nvl(sum(nvl(economicspunish,0)),0） punish from bis_lllegalpunish where lllegalid ='{0}' and  instr('{1}',','||personinchargeid||',')>0 and  assessobject like '%单位%'  ", row["id"].ToString(), expteamcode);
                            var wwdt = this.BaseRepository().FindTable(csql);
                            row["wwpunish"] = wwdt.Rows[0]["punish"];


                        }
                        //部门考核
                        if (!string.IsNullOrEmpty(expdepartcode))
                        {
                            expdepartcode = "," + expdepartcode + ",";
                            csql = string.Format(@" select  nvl(sum(nvl(economicspunish,0)),0） punish from bis_lllegalpunish where lllegalid ='{0}' and  instr('{1}',','||personinchargeid||',')>0 and  assessobject like '%单位%'  ", row["id"].ToString(), expdepartcode);
                            var bmdt = this.BaseRepository().FindTable(csql);
                            row["bmpunish"] = bmdt.Rows[0]["punish"];
                        }
                        //汇总
                        row["total"] = Convert.ToDecimal(row["wwpunish"].ToString()) + Convert.ToDecimal(row["bmpunish"].ToString());
                    }
                    #endregion
                    break;
                case 1:

                    break;
            }
            return dt;
        }
        #endregion
    }


}