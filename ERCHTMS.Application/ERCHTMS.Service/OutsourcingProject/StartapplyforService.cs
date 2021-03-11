using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System;
using System.Text;
using ERCHTMS.Code;
using ERCHTMS.Service.HighRiskWork;
using ERCHTMS.Service.BaseManage;

namespace ERCHTMS.Service.OutsourcingProject
{
    /// <summary>
    /// 描 述：开工申请表
    /// </summary>
    public class StartapplyforService : RepositoryFactory<StartapplyforEntity>, StartapplyforIService
    {
        private OutsouringengineerService outsouringengineerservice = new OutsouringengineerService();
        private DepartmentService departmentservice = new DepartmentService();
        private ManyPowerCheckService powerCheck = new ManyPowerCheckService();
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<StartapplyforEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public StartapplyforEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        public DataTable GetApplyInfo(string keyValue)
        {
            string sql = string.Format(@"select s.id,s.iscommit,s.OUTENGINEERID projectid,e.ENGINEERLETDEPT deptname,s.applyno,s.applycause,s.applytype,s.outprojectid unitid,s.htnum,s.safetyman,s.dutyman,
                                           b.fullname unitname,ENGINEERCODE projectcode,e.engineerareaname areaname,f.itemname projecttype,g.itemname projectlevel,ENGINEERCONTENT projectcontent,
                                           e.engineername projectname,s.applyreturntime startdate,s.applyendtime enddate,isover,nodename,checkresult,checkusers,e.ENGINEERLETDEPTid deptid,s.applytime,s.applypeople,nodeid from  
                                          epg_startapplyfor s
 left join epg_outsouringengineer e on e.id = s.outengineerid left join base_department b on b.departmentid = s.outprojectid 
                                    left join base_dataitemdetail f on e.engineertype=f.itemvalue 
                                    left join base_dataitemdetail g on e.engineerlevel=g.itemvalue
                                    where f.itemid=(select itemid from base_dataitem where itemcode='ProjectType')
                                    and g.itemid=(select itemid from base_dataitem where itemcode='ProjectLevel') and s.id='{0}'", keyValue);
            return BaseRepository().FindTable(sql);
        }
        /// <summary>
        /// 获取工程合同编号
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public object GetContractSno(string projectId)
        {
            return BaseRepository().FindObject(string.Format("select t.compactno from epg_compact t where projectid='{0}'",projectId));
        }
        /// <summary>
        /// 判断当前用户是否有审核权限
        /// </summary>
        /// <param name="nodeId">节点Id</param>
        /// <param name="user">当前用户</param>
        /// <param name="projectId">工程Id</param>
        /// <returns></returns>
        public bool HasCheckPower(string nodeId,ERCHTMS.Code.Operator user,string projectId)
        {
            bool flag = false;
            string sendDeptId = BaseRepository().FindObject(string.Format("select ENGINEERLETDEPTID from epg_outsouringengineer where id='{0}'",projectId)).ToString();
            DataTable dt = BaseRepository().FindTable(string.Format("select t.checkdeptid,t.checkroleid  from BIS_MANYPOWERCHECK t where t.id='{0}'",nodeId));
            string deptId=dt.Rows[0][0].ToString();
            string roleId =dt.Rows[0][1].ToString();
            string[] arrRole = user.RoleId.Split(',');
            if (deptId=="-1")
            {
                if (sendDeptId == user.DeptId)
                {
                   foreach (string rId in arrRole)
                   {
                     if (roleId.Contains(rId))
                     {
                        flag= true;
                        break;
                     }
                   }
                }
                
            }
            else
            {
                if (deptId==user.DeptId)
                {
                    foreach (string rId in arrRole)
                    {
                        if (roleId.Contains(rId))
                        {
                            flag= true;
                            break;
                        }
                    }
                }
            }
            return flag;
        }
        /// <summary>
        /// 获取开工条件各项目完成情况
        /// </summary>
        /// <param name="projectId">工程Id</param>
        /// <returns></returns>
        public DataTable GetStartWorkStatus(string projectId)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("status");
            dt.Columns.Add("checkuser");
            int status = 0;
            string username = "";
            DataRow dr = dt.NewRow();

            status = 0;
            username = "";
            //资质审查
            DataTable dtUser = BaseRepository().FindTable(string.Format("select a.auditpeople from epg_aptitudeinvestigateaudit a where a.aptitudeid=(select id from (select id from  epg_aptitudeinvestigateinfo t where t.isauditover='1' and OUTENGINEERID='{0}' order by createdate desc) where rownum=1) order by createtime desc", projectId));
            if (dtUser.Rows.Count>0)
            {
                status = 1;
                username = dtUser.Rows[0][0].ToString();
            }
            dr[0] = status; dr[1] = username;
            dt.Rows.Add(dr);
            //三措两案
            status = 0;
            username = "";
            dtUser = BaseRepository().FindTable(string.Format("select a.auditpeople from epg_aptitudeinvestigateaudit a where a.aptitudeid in (select id from (select id from  EPG_SCHEMEMEASURE t where  projectid=(select OUTENGINEERID from  epg_startappprocessstatus where THREETWOSTATUS='1' and OUTENGINEERID='{0}') order by createdate desc) where rownum=1) order by createtime desc", projectId));
            if (dtUser.Rows.Count > 0)
            {
                status = 1;
                username = dtUser.Rows[0][0].ToString();
            }
            dr = dt.NewRow();
            dr[0] = status; dr[1] = username;
            dt.Rows.Add(dr);
            //安全技术交底
            status = 0;
            username = "";
            dtUser = BaseRepository().FindTable(string.Format("select DISCLOSUREPERSON from (select DISCLOSUREPERSON from EPG_TECHDISCLOSURE where projectid='{0}' order by createdate desc) where rownum=1", projectId));
            if (dtUser.Rows.Count>0)
            {
                status = 1;
                username = dtUser.Rows[0][0].ToString();
            }
            dr = dt.NewRow();
            dr[0] = status; dr[1] = username;
            dt.Rows.Add(dr);
            //工器具
            status = 0;
            username = "";
            dtUser = BaseRepository().FindTable(string.Format("select a.AUDITPEOPLE from EPG_TOOLSAUDIT a where a.TOOLSID in (select TOOLSID from (select TOOLSID from  EPG_TOOLS t where  OUTENGINEERID=(select OUTENGINEERID from  epg_startappprocessstatus where EQUIPMENTTOOLSTATUS='1' and OUTENGINEERID='{0}') order by createdate desc) where rownum=1) order by audittime desc", projectId));
            if (dtUser.Rows.Count > 0)
            {
                status = 1;
                username = dtUser.Rows[0][0].ToString();
            }
            dr = dt.NewRow();
            dr[0] = status; dr[1] = username;
            dt.Rows.Add(dr);

            return dt;
        }
        /// <summary>
        /// 获取工程施工现场负责人和安全员信息
        /// </summary>
        /// <param name="projectId">工程Id</param>
        /// <returns></returns>
        public List<string> GetSafetyUserInfo(string projectId)
        {
            List<string> list = new List<string>();
            StringBuilder sb = new StringBuilder();
            DataTable dt = BaseRepository().FindTable(string.Format("select realname,DUTYNAME from base_user u where u.projectid='{0}'  and (u.DUTYNAME like'%现场负责人%' or u.DUTYNAME like '%安全员%' or u.DUTYNAME like'%专/兼职安全员%')", projectId));
            DataRow []rows=dt.Select("dutyname like'%现场负责人%'");
            foreach(DataRow dr in rows)
            {
                sb.AppendFormat("{0},",dr[0].ToString());  
            }
            list.Add(sb.ToString().TrimEnd(','));
            sb.Clear();
            rows = dt.Select("dutyname like '%安全员%' or dutyname like'%专/兼职安全员%'");
            foreach (DataRow dr in rows)
            {
                sb.AppendFormat("{0},", dr[0].ToString());
            }
            list.Add(sb.ToString().TrimEnd(','));
            return list;
        }

        public List<string> GetSafetyUserInfo(string projectId,string roletype,string deptid)
        {
            List<string> list = new List<string>();
            StringBuilder sb = new StringBuilder();
            DataTable dt = BaseRepository().FindTable(string.Format("select realname,DUTYNAME from base_user u where u.projectid='{0}' and u.DUTYNAME in('现场负责人','安全员','专/兼职安全员')  and u.departmentid='{1}' ", projectId, deptid));
            if (roletype == "1")
            {
                DataRow[] rows = dt.Select("dutyname='现场负责人'");
                foreach (DataRow dr in rows)
                {
                    sb.AppendFormat("{0},", dr[0].ToString());
                }
                if (!string.IsNullOrEmpty(sb.ToString()))
                    list.Add(sb.ToString().TrimEnd(','));
            }
            else {
                DataRow[] rows = dt.Select("dutyname='安全员' or dutyname='专/兼职安全员'");
                foreach (DataRow dr in rows)
                {
                    sb.AppendFormat("{0},", dr[0].ToString());
                }
                if (!string.IsNullOrEmpty(sb.ToString()))
                    list.Add(sb.ToString().TrimEnd(','));
            }
            return list;
        }
           /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson) {
           
            DatabaseType dataType = DbHelper.DbType;
            Operator currUser = OperatorProvider.Provider.Current();
            var queryParam = queryJson.ToJObject();
            if (!queryParam["name"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and (e.engineername like'%{0}%' or b.fullname like'%{1}%') ", queryParam["name"].ToString(), queryParam["name"].ToString());
            }
            if (!queryParam["orgCode"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and s.createuserorgcode='{0}' ", queryParam["orgCode"].ToString());
            }
            if (!queryParam["StartTime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and s.applyreturntime>= to_date('{0}','yyyy-MM-dd hh24:mi:ss') ", queryParam["StartTime"].ToString());
            }
            if (!queryParam["EndTime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and s.applyreturntime<= to_date('{0}','yyyy-MM-dd hh24:mi:ss') ", Convert.ToDateTime(queryParam["EndTime"]).AddDays(1).ToString("yyyy-MM-dd"));
            }
            //外包工程Id
            if (!queryParam["ProjectId"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and s.OUTENGINEERID='{0}'", queryParam["ProjectId"].ToString());
            }
            //外包单位Id
            if (!queryParam["UnitId"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and s.OUTPROJECTID='{0}'", queryParam["UnitId"].ToString());
            }
            //发包部门Id
            if (!queryParam["DeptId"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and e.engineerletdeptid='{0}'", queryParam["DeptId"].ToString());
            }
            if (!queryParam["indexState"].IsEmpty())//首页代办
            {
                string strCondition = "";
                strCondition = string.Format(" and s.createuserorgcode='{0}' and s.iscommit ='1' and s.isover='0'", currUser.OrganizeCode);
                DataTable data = BaseRepository().FindTable("select " + pagination.p_kid + "," + pagination.p_fields + " from " + pagination.p_tablename + " where " + pagination.conditionJson + strCondition);
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    var engineerEntity = outsouringengineerservice.GetEntity(data.Rows[i]["outengineerid"].ToString());
                    var excutdept = departmentservice.GetEntity(engineerEntity.ENGINEERLETDEPTID).DepartmentId;
                    var outengineerdept = departmentservice.GetEntity(engineerEntity.OUTPROJECTID).DepartmentId;
                    var supervisordept = string.IsNullOrEmpty(engineerEntity.SupervisorId) ? "" : departmentservice.GetEntity(engineerEntity.SupervisorId).DepartmentId;
                    //获取下一步审核人
                    string str = powerCheck.GetApproveUserId(data.Rows[i]["nodeid"].ToString(), data.Rows[i]["id"].ToString(), "", "", excutdept, outengineerdept, "", "", "", supervisordept, data.Rows[i]["outengineerid"].ToString());
                    data.Rows[i]["approveuserids"] = str;
                }

                string[] applyids = data.Select(" approveuserids like '%" + currUser.UserId + "%'").AsEnumerable().Select(d => d.Field<string>("id")).ToArray();

                pagination.conditionJson += string.Format(" and s.id in ('{0}') {1}", string.Join("','", applyids), strCondition);
            }
            if (!queryParam["projectid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and e.id ='{0}'", queryParam["projectid"].ToString());
            }
            DataTable dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            return dt;
        }
        /// <summary>
        /// 获取开工申请对象
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        public StartapplyforEntity GetApplyReturnTime(string outProjectId, string outEngId)
        {
            string sql = string.Format(@"select * from epg_startapplyfor t where t.outengineerid='{0}' and t.outprojectid='{1}'", outEngId, outProjectId);
            return this.BaseRepository().FindList(sql).OrderByDescending(x => x.CREATEDATE).ToList().FirstOrDefault();
        }

        public DataTable GetStartForItem(string keyValue) {
            string sql = string.Format(@"select t.id itemid,t.investigatecontent,
                                                        t.investigateresult,t.investigatepeople,t.investigatepeopleid,
                                                        t.modifydate,d.sortid,t.signpic signpic
                                                        from epg_investigatedtrecord t 
                                                        left join epg_investigatecontent d on t.investigatecontentid=d.id 
                                                        where t.investigaterecordid=(select id from epg_investigaterecord where intofactoryid='{0}') 
                                                        order by to_number(d.sortid) asc ", keyValue);
            return this.BaseRepository().FindTable(sql);
        }
        
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public bool SaveForm(string keyValue, StartapplyforEntity entity)
        {
            try{
                entity.ID = keyValue;
                if (!string.IsNullOrEmpty(keyValue))
                {
                    var sl = this.BaseRepository().FindEntity(keyValue);
                    if (sl == null)
                    {
                        entity.Create();
                        string sql = string.Format("select max(APPLYNO) from epg_startapplyfor t where createuserorgcode ='{0}'", ERCHTMS.Code.OperatorProvider.Provider.Current().OrganizeCode);
                        if (string.IsNullOrWhiteSpace(entity.APPLYNO))
                        {
                            DataTable Code = this.BaseRepository().FindTable(sql);
                            if (Code.Rows.Count == 0 || string.IsNullOrEmpty(Code.Rows[0][0].ToString()))
                            {
                                entity.APPLYNO = "KG0001";
                            }
                            else
                            {
                                int num = Convert.ToInt32(Code.Rows[0][0].ToString().Substring(2));
                                num++;
                                entity.APPLYNO = "KG" + num.ToString().PadLeft(4, '0');
                            }
                        }
                        this.BaseRepository().Insert(entity);
                    }
                    else
                    {
                   
                        entity.Modify(keyValue);
                        this.BaseRepository().Update(entity);
                    }

                }
                else
                {
                    entity.Create();
                    string sql = string.Format("select max(APPLYNO) from epg_startapplyfor t where createuserorgcode ='{0}'", ERCHTMS.Code.OperatorProvider.Provider.Current().OrganizeCode);
                    if (string.IsNullOrWhiteSpace(entity.APPLYNO))
                    {
                        DataTable Code = this.BaseRepository().FindTable(sql);
                        if (Code.Rows.Count == 0 || string.IsNullOrEmpty(Code.Rows[0][0].ToString()))
                        {
                            entity.APPLYNO = "KG0001";
                        }
                        else
                        {
                            int num = Convert.ToInt32(Code.Rows[0][0].ToString().Substring(2));
                            num++;
                            entity.APPLYNO = "KG" + num.ToString().PadLeft(4, '0');
                        }
                    }
                    this.BaseRepository().Insert(entity);
                }
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
            
        }
        #endregion
    }
}
