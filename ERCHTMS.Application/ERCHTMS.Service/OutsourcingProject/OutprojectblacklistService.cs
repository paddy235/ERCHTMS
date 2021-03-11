using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Data;
using System;
using ERCHTMS.Code;
using ERCHTMS.Service.BaseManage;

namespace ERCHTMS.Service.OutsourcingProject
{
    /// <summary>
    /// 描 述：外包单位黑名单表
    /// </summary>
    public class OutprojectblacklistService : RepositoryFactory<OutprojectblacklistEntity>, OutprojectblacklistIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<OutprojectblacklistEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public OutprojectblacklistEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 查询黑名单单位列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetPageBlackListJson(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //外包单位名称
            if (!queryParam["projectname"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and  t.outsourcingname like'%{0}%' ", queryParam["projectname"].ToString());
            }
            //黑名单
            if (!queryParam["BlackState"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and  t.blackliststate='{0}' ", queryParam["BlackState"].ToString());
            }
            //移出
            if (!queryParam["outInState"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and bk.state='{0}'", queryParam["outInState"].ToString());
            }
            if (!queryParam["orgCode"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.createuserorgcode like'%{0}%'", queryParam["orgCode"].ToString());
            }
            DataTable dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            return dt;
        }


        #region 外包流程管理
        /// <summary>
        /// 待审（核）批单位资质审查、待审（核）批人员资质审查、待审（核）批三措两案、待审（核）批特种设备验收、待审（核）批安全/电动工器具验收、待审（核）批入厂许可、待审（核）批开工申请
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> ToAuditOutPeoject(Operator currUser)
        {
            List<int> toAuditNum = new List<int>();
            string role = currUser.RoleName;
            string deptId = string.Empty;
            string deptName = string.Empty;

            //公司级用户取机构对象
            if (role.Contains("公司级用户"))
            {
                deptId = currUser.OrganizeId;  //机构ID
                deptName = currUser.OrganizeName;//机构名称
            }
            else
            {
                deptId = currUser.DeptId; //部门ID
                deptName = currUser.DeptName; //部门ID
            }
            string sql = string.Empty;
            string[] arrRole = role.Split(',');

            string strWhere = string.Empty;
            #region 单位资质审查待审核(查)
            sql = string.Empty;
            sql = string.Format("select '' as approveuserids,outengineerid,flowid,id from epg_aptitudeinvestigateinfo s ");

            strWhere = string.Empty;
            strWhere += string.Format(" where  s.isauditover=0 and s.createuserorgcode='{0}' and s.issaveorcommit='1' ",  currUser.OrganizeCode);

            var dt = this.BaseRepository().FindTable(sql + strWhere);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var engineerEntity =new OutsouringengineerService().GetEntity(dt.Rows[i]["outengineerid"].ToString());
                var excutdept = new DepartmentService().GetEntity(engineerEntity.ENGINEERLETDEPTID).DepartmentId;
                var outengineerdept = new DepartmentService().GetEntity(engineerEntity.OUTPROJECTID).DepartmentId;
                var supervisordept = string.IsNullOrEmpty(engineerEntity.SupervisorId) ? "" : new DepartmentService().GetEntity(engineerEntity.SupervisorId).DepartmentId;
                //获取下一步审核人
                string str = new ManyPowerCheckService().GetApproveUserId(dt.Rows[i]["flowid"].ToString(), dt.Rows[i]["outengineerid"].ToString(), "", "", excutdept, outengineerdept, "", "", "", supervisordept, dt.Rows[i]["outengineerid"].ToString());
                dt.Rows[i]["approveuserids"] = str;
            }

            string[] applyids = dt.Select(" approveuserids like '%" + currUser.UserId + "%'").AsEnumerable().Select(d => d.Field<string>("id")).ToArray();
            if (applyids.Length > 0)
            {
                toAuditNum.Add(applyids.Length);
            }
            else
            {
                toAuditNum.Add(0);
            }
            #endregion
            #region 人员审查待审核
            sql = string.Empty;
            sql = string.Format("select '' as approveuserids,outengineerid,flowid,id from epg_peoplereview s ");

            strWhere = string.Empty;
            strWhere += string.Format(" where  s.createuserorgcode='{0}' and isauditover=0 and s.issaveorcommit='1' ", currUser.OrganizeCode);
            dt = this.BaseRepository().FindTable(sql + strWhere);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var engineerEntity = new OutsouringengineerService().GetEntity(dt.Rows[i]["outengineerid"].ToString());
                var excutdept = new DepartmentService().GetEntity(engineerEntity.ENGINEERLETDEPTID).DepartmentId;
                var outengineerdept = new DepartmentService().GetEntity(engineerEntity.OUTPROJECTID).DepartmentId;
                var supervisordept = string.IsNullOrEmpty(engineerEntity.SupervisorId) ? "" : new DepartmentService().GetEntity(engineerEntity.SupervisorId).DepartmentId;
                //获取下一步审核人
                string str = new ManyPowerCheckService().GetApproveUserId(dt.Rows[i]["flowid"].ToString(), dt.Rows[i]["outengineerid"].ToString(), "", "", excutdept, outengineerdept, "", "", "", supervisordept, dt.Rows[i]["outengineerid"].ToString());
                dt.Rows[i]["approveuserids"] = str;
            }

            applyids = dt.Select(" approveuserids like '%" + currUser.UserId + "%'").AsEnumerable().Select(d => d.Field<string>("id")).ToArray();
            if (applyids.Length > 0)
            {
                toAuditNum.Add(applyids.Length);
            }
            else
            {
                toAuditNum.Add(0);
            }
            #endregion
            #region 三措两案待审核
            sql = string.Empty;
            sql = string.Format("select '' as approveuserids,projectid,flowid,id,ENGINEERLETDEPTID from epg_schememeasure s ");

            strWhere = string.Empty;
            strWhere += string.Format(" where s.createuserorgcode='{0}' and s.isover='0' and s.issaved='1' ", currUser.OrganizeCode);
            dt = this.BaseRepository().FindTable(sql + strWhere);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var engineerEntity = new OutsouringengineerService().GetEntity(dt.Rows[i]["projectid"].ToString());
                if (engineerEntity != null)
                {
                    var excutdept = new DepartmentService().GetEntity(engineerEntity.ENGINEERLETDEPTID).DepartmentId;
                    var outengineerdept = new DepartmentService().GetEntity(engineerEntity.OUTPROJECTID).DepartmentId;
                    var supervisordept = string.IsNullOrEmpty(engineerEntity.SupervisorId) ? "" : new DepartmentService().GetEntity(engineerEntity.SupervisorId).DepartmentId;
                    //获取下一步审核人
                    string str = new ManyPowerCheckService().GetApproveUserId(dt.Rows[i]["flowid"].ToString(), dt.Rows[i]["id"].ToString(), "", "", excutdept, outengineerdept, "", "", "", supervisordept, dt.Rows[i]["projectid"].ToString());
                    dt.Rows[i]["approveuserids"] = str;
                }
                else
                {
                    string str = new ManyPowerCheckService().GetApproveUserId(dt.Rows[i]["flowid"].ToString(), dt.Rows[i]["id"].ToString(), "", "", dt.Rows[i]["engineerletdeptid"].ToString(), "", "", "", "", "", dt.Rows[i]["projectid"].ToString());
                    dt.Rows[i]["approveuserids"] = str;
                }


                //dt.Rows[i]["approveuserids"] = str;
            }

            applyids = dt.Select(" approveuserids like '%" + currUser.UserId + "%'").AsEnumerable().Select(d => d.Field<string>("id")).ToArray();
            if (applyids.Length > 0)
            {
                toAuditNum.Add(applyids.Length);
            }
            else
            {
                toAuditNum.Add(0);
            }
            #endregion
            #region 特种设备
            sql = string.Empty;
            sql = string.Format("select '' as approveuserids,outengineerid,flowid,toolsid from epg_tools s ");

            strWhere = string.Empty;
            strWhere += string.Format(" where  equiptype='2'  and s.createuserorgcode='{0}' and s.isover='0' and s.issaved='1'   ", currUser.OrganizeCode);
            dt = this.BaseRepository().FindTable(sql + strWhere);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var engineerEntity = new OutsouringengineerService().GetEntity(dt.Rows[i]["outengineerid"].ToString());
                var excutdept = new DepartmentService().GetEntity(engineerEntity.ENGINEERLETDEPTID).DepartmentId;
                var outengineerdept = new DepartmentService().GetEntity(engineerEntity.OUTPROJECTID).DepartmentId;
                var supervisordept = string.IsNullOrEmpty(engineerEntity.SupervisorId) ? "" : new DepartmentService().GetEntity(engineerEntity.SupervisorId).DepartmentId;
                //获取下一步审核人
                string str = new ManyPowerCheckService().GetApproveUserId(dt.Rows[i]["flowid"].ToString(), dt.Rows[i]["toolsid"].ToString(), "", "", excutdept, outengineerdept, "", "", "", supervisordept, dt.Rows[i]["outengineerid"].ToString());
                dt.Rows[i]["approveuserids"] = str;
            }

            applyids = dt.Select(" approveuserids like '%" + currUser.UserId + "%'").AsEnumerable().Select(d => d.Field<string>("toolsid")).ToArray();
            if (applyids.Length > 0)
            {
                toAuditNum.Add(applyids.Length);
            }
            else
            {
                toAuditNum.Add(0);
            }
            #endregion
            #region 普通设备
            sql = string.Empty;
            sql = string.Format("select '' as approveuserids,outengineerid,flowid,toolsid from epg_tools s ");

            strWhere = string.Empty;
            strWhere += string.Format(" where  equiptype='1'  and s.createuserorgcode='{0}' and s.isover='0' and s.issaved='1'   ", currUser.OrganizeCode);
            dt = this.BaseRepository().FindTable(sql + strWhere);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var engineerEntity = new OutsouringengineerService().GetEntity(dt.Rows[i]["outengineerid"].ToString());
                var excutdept = new DepartmentService().GetEntity(engineerEntity.ENGINEERLETDEPTID).DepartmentId;
                var outengineerdept = new DepartmentService().GetEntity(engineerEntity.OUTPROJECTID).DepartmentId;
                var supervisordept = string.IsNullOrEmpty(engineerEntity.SupervisorId) ? "" : new DepartmentService().GetEntity(engineerEntity.SupervisorId).DepartmentId;
                //获取下一步审核人
                string str = new ManyPowerCheckService().GetApproveUserId(dt.Rows[i]["flowid"].ToString(), dt.Rows[i]["toolsid"].ToString(), "", "", excutdept, outengineerdept, "", "", "", supervisordept, dt.Rows[i]["outengineerid"].ToString());
                dt.Rows[i]["approveuserids"] = str;
            }

            applyids = dt.Select(" approveuserids like '%" + currUser.UserId + "%'").AsEnumerable().Select(d => d.Field<string>("toolsid")).ToArray();
            if (applyids.Length > 0)
            {
                toAuditNum.Add(applyids.Length);
            }
            else
            {
                toAuditNum.Add(0);
            }
            #endregion
            #region 入场许可
            sql = string.Empty;
            sql = string.Format("select '' as approveuserids,outengineerid,flowid,id from epg_intromission s ");

            strWhere = string.Empty;
            strWhere += string.Format(" where   s.createuserorgcode='{0}' and (s.investigatestate='1' or s.investigatestate='2')   ", currUser.OrganizeCode);
            dt = this.BaseRepository().FindTable(sql + strWhere);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var engineerEntity = new OutsouringengineerService().GetEntity(dt.Rows[i]["outengineerid"].ToString());
                var excutdept = new DepartmentService().GetEntity(engineerEntity.ENGINEERLETDEPTID).DepartmentId;
                var outengineerdept = new DepartmentService().GetEntity(engineerEntity.OUTPROJECTID).DepartmentId;
                var supervisordept = string.IsNullOrEmpty(engineerEntity.SupervisorId) ? "" : new DepartmentService().GetEntity(engineerEntity.SupervisorId).DepartmentId;
                //获取下一步审核人
                string str = new ManyPowerCheckService().GetApproveUserId(dt.Rows[i]["flowid"].ToString(), dt.Rows[i]["id"].ToString(), "", "", excutdept, outengineerdept, "", "", "", supervisordept, dt.Rows[i]["outengineerid"].ToString());
                dt.Rows[i]["approveuserids"] = str;
            }

            applyids = dt.Select(" approveuserids like '%" + currUser.UserId + "%'").AsEnumerable().Select(d => d.Field<string>("id")).ToArray();
            if (applyids.Length > 0)
            {
                toAuditNum.Add(applyids.Length);
            }
            else
            {
                toAuditNum.Add(0);
            }
            //sql = string.Empty;
            //sql = string.Format("select count(id) intromnum from epg_intromission s ");
            //strWhere = string.Empty;
            //foreach (string str in arrRole)
            //{
            //    //审查内容
            //    strWhere += string.Format(@" select distinct a.id  from epg_intromission a 
            //                                    left join  bis_manypowercheck b on a.flowid = b.id
            //                                    left join bis_manypowercheck c on  b.serialnum = c.serialnum and b.moduleno = c.moduleno
            //                                    left join epg_outsouringengineer d on a.outengineerid = d.id 
            //                                    where  a.investigatestate ='1'  and  ((c.checkdeptid ='-1' and d.engineerletdeptid='{0}'  and c.checkrolename like '%{1}%')  
            //                                    or (c.checkdeptid =  '{0}'  and c.checkrolename like '%{1}%') or ( c.checkdeptid ='-2' and d.outprojectid='{0}'  and c.checkrolename like '%{1}%'))
            //                                    union 
            //                                    ", deptId, str);

            //    //审核内容
            //    strWhere += string.Format(@"   select  distinct a.id from epg_intromission a  where a.flowdept='{0}' and a.flowrolename like '%{1}%' and  a.investigatestate ='2' 
            //                                    union", deptId, str);
            //}
            //if (!string.IsNullOrEmpty(strWhere))
            //{
            //    strWhere = strWhere.Substring(0, strWhere.Length - 5);
            //}
            //var conditionDt1 = new IntromissionService().GetDataTableBySql(strWhere);
            //string ids1 = string.Empty;
            //foreach (DataRow row in conditionDt1.Rows)
            //{
            //    ids1 += "'" + row["id"].ToString() + "',";
            //}
            //if (!string.IsNullOrEmpty(ids1))
            //{
            //    ids1 = ids1.Substring(0, ids1.Length - 1);

            //    sql += string.Format("where s.id in ({0})", ids1);
            //}
            //else
            //{
            //    sql += string.Format("where 1!=1 ");
            //}
            //sql += string.Format("  and s.createuserorgcode='{0}'", currUser.OrganizeCode);
            //strWhere = string.Empty;
            //strWhere += string.Format(" where  s.flowdept like'%{0}%' and s.createuserorgcode='{1}' and s.investigatestate !='3'  ", currUser.DeptId, currUser.OrganizeCode);
            //strWhere += " and (";
            //foreach (string str in arrRole)
            //{
            //    strWhere += string.Format(" s.flowrolename  like '%{0}%' or", str);
            //}
            //strWhere = strWhere.Substring(0, strWhere.Length - 2);
            //strWhere += " )";
            //sql = sql + strWhere;
            //dt = this.BaseRepository().FindTable(sql);
            //if (dt.Rows.Count > 0)
            //{
            //    toAuditNum.Add(Convert.ToInt32(dt.Rows[0]["intromnum"].ToString()));
            //}
            //else
            //{
            //    toAuditNum.Add(0);
            //}
            #endregion
            #region 开工申请待审核(查)
           
            sql = string.Empty;
            sql = string.Format("select '' as approveuserids,outengineerid,nodeid,id from epg_startapplyfor s ");

            strWhere = string.Empty;
            strWhere += string.Format(" where  s.createuserorgcode='{0}' and  s.iscommit ='1' and  s.isover =0    ", currUser.OrganizeCode);
            dt = this.BaseRepository().FindTable(sql + strWhere);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var engineerEntity = new OutsouringengineerService().GetEntity(dt.Rows[i]["outengineerid"].ToString());
                var excutdept = new DepartmentService().GetEntity(engineerEntity.ENGINEERLETDEPTID).DepartmentId;
                var outengineerdept = new DepartmentService().GetEntity(engineerEntity.OUTPROJECTID).DepartmentId;
                var supervisordept = string.IsNullOrEmpty(engineerEntity.SupervisorId) ? "" : new DepartmentService().GetEntity(engineerEntity.SupervisorId).DepartmentId;
                //获取下一步审核人
                string str = new ManyPowerCheckService().GetApproveUserId(dt.Rows[i]["nodeid"].ToString(), dt.Rows[i]["id"].ToString(), "", "", excutdept, outengineerdept, "", "", "", supervisordept, dt.Rows[i]["outengineerid"].ToString());
                dt.Rows[i]["approveuserids"] = str;
            }

            applyids = dt.Select(" approveuserids like '%" + currUser.UserId + "%'").AsEnumerable().Select(d => d.Field<string>("id")).ToArray();
            if (applyids.Length > 0)
            {
                toAuditNum.Add(applyids.Length);
            }
            else
            {
                toAuditNum.Add(0);
            }
            #endregion
            #region 待审核安全技术交底
            sql = string.Empty;
            sql = string.Format("select '' as approveuserids,projectid as outengineerid,flowid,id from epg_techdisclosure s ");

            strWhere = string.Empty;
            strWhere += string.Format(" where  s.status=1 and s.createuserorgcode='{0}' and s.issubmit=1 ", currUser.OrganizeCode);

            dt = this.BaseRepository().FindTable(sql + strWhere);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var engineerEntity = new OutsouringengineerService().GetEntity(dt.Rows[i]["outengineerid"].ToString());
                var excutdept = engineerEntity == null ? "" : new DepartmentService().GetEntity(engineerEntity.ENGINEERLETDEPTID).DepartmentId;
                var outengineerdept = engineerEntity == null ? "" : new DepartmentService().GetEntity(engineerEntity.OUTPROJECTID).DepartmentId;
                var supervisordept = engineerEntity == null ? "" : string.IsNullOrEmpty(engineerEntity.SupervisorId) ? "" : new DepartmentService().GetEntity(engineerEntity.SupervisorId).DepartmentId;
                //获取下一步审核人
                string str = new ManyPowerCheckService().GetApproveUserId(dt.Rows[i]["flowid"].ToString(), dt.Rows[i]["id"].ToString(), "", "", excutdept, outengineerdept, "", "", "", supervisordept, dt.Rows[i]["outengineerid"].ToString());
                dt.Rows[i]["approveuserids"] = str;
            }
            applyids = dt.Select(" approveuserids like '%" + currUser.UserId + "%'").AsEnumerable().Select(d => d.Field<string>("id")).ToArray();
            if (applyids.Length > 0)
            {
                toAuditNum.Add(applyids.Length);
            }
            else
            {
                toAuditNum.Add(0);
            }
            #endregion
            return toAuditNum;
        }
        /// <summary>
        /// 保证金、合同、协议、安全技术交底、开(收)工会、安全评价
        /// </summary>
        /// <param name="currUser"></param>
        /// <returns></returns>
        public List<int> ToIndexData(Operator currUser)
        {
            List<int> toIndexData = new List<int>();
            string role = currUser.RoleName;
            string deptId = string.Empty;
            string deptName = string.Empty;
            string sql = string.Empty;
            string strWhere = string.Empty;
            string queryRole = string.Empty;
            sql = @"select count(e.id) num
                              from epg_outsouringengineer e
                     where e.id in (select distinct (t.outengineerid)
                                              from EPG_APTITUDEINVESTIGATEINFO t
                                             where t.isauditover = 1) {0} ";
            if (currUser.IsSystem)
            {
                queryRole = "   and 1=1 ";
            }
            else if (currUser.RoleName.Contains("省级"))
            {
                queryRole = string.Format(@" and e.createuserorgcode  in (select encode
                        from base_department d
                        where d.deptcode like '{0}%' and d.nature = '厂级' and d.description is null)", currUser.NewDeptCode);
            }
            else if (currUser.RoleName.Contains("厂级部门用户") || currUser.RoleName.Contains("公司级用户"))
            {
                queryRole = string.Format(" and e.createuserorgcode like'%{0}%' ", currUser.OrganizeCode);
            }
            else if (currUser.RoleName.Contains("承包商级用户"))
            {
                queryRole = string.Format(" and e.outprojectid ='{0}'", currUser.DeptId);
            }
            else
            {
                queryRole = string.Format(" and e.engineerletdeptid ='{0}'", currUser.DeptId);
            }
            sql = string.Format(sql, queryRole);
            #region 保证金
            strWhere = string.Format(" and e.id not in (select distinct(m.projectid) from epg_safetyeamestmoney m)");
            var dt = this.BaseRepository().FindTable(sql + strWhere);
            if (dt.Rows.Count > 0)
            {
                toIndexData.Add(Convert.ToInt32(dt.Rows[0]["num"].ToString()));
            }
            else
            {
                toIndexData.Add(0);
            }
            #endregion
            #region 合同
            strWhere = string.Format(" and e.id not in (select distinct(m.projectid) from epg_compact m)");
            //sql = sql + strWhere;
            dt = this.BaseRepository().FindTable(sql + strWhere);
            if (dt.Rows.Count > 0)
            {
                toIndexData.Add(Convert.ToInt32(dt.Rows[0]["num"].ToString()));
            }
            else
            {
                toIndexData.Add(0);
            }
            #endregion
            #region 协议
            strWhere = string.Format(" and e.id not in (select distinct(m.projectid) from epg_protocol m)");
            //sql = sql + strWhere;
            dt = this.BaseRepository().FindTable(sql + strWhere);
            if (dt.Rows.Count > 0)
            {
                toIndexData.Add(Convert.ToInt32(dt.Rows[0]["num"].ToString()));
            }
            else
            {
                toIndexData.Add(0);
            }
            #endregion
            #region 安全技术交底
            strWhere = " and e.id not in (select distinct(m.projectid) from epg_techdisclosure m)";
            //sql = sql + strWhere;
            dt = this.BaseRepository().FindTable(sql + strWhere);
            if (dt.Rows.Count > 0)
            {
                toIndexData.Add(Convert.ToInt32(dt.Rows[0]["num"].ToString()));
            }
            else
            {
                toIndexData.Add(0);
            }
            #endregion
            #region 开(收)工会
            sql = string.Empty;
            strWhere = string.Empty;
            queryRole = string.Empty;
            sql = @"select count(id) num from bis_workmeeting where iscommit='1' {0}";
            if (currUser.IsSystem || role.Contains("厂级部门用户") || role.Contains("公司级用户") || role.Contains("公司管理员"))
            {
                queryRole = string.Format(" and createuserorgcode = '{0}' ", currUser.OrganizeCode);
            }
            else
            {
                //发包部门查看所属承包商的开收工会、承包商查看发包部门创建的开收工会。
                queryRole = string.Format(@" and (engineerid in (select e.ID from epg_outsouringengineer e 
where e.ENGINEERLETDEPTID ='{0}') or engineerid in (select e.ID from EPG_OutSouringEngineer e where e.outprojectid='{0}') )", currUser.DeptId);
            }
            sql = string.Format(sql, queryRole);
            strWhere = " and to_char(meetingdate,'yyyy-MM-dd')=to_char(sysdate,'yyyy-MM-dd')";
            sql = sql + strWhere;
            dt = this.BaseRepository().FindTable(sql);
            if (dt.Rows.Count > 0)
            {
                toIndexData.Add(Convert.ToInt32(dt.Rows[0]["num"].ToString()));
            }
            else
            {
                toIndexData.Add(0);
            }
            #endregion
            #region 安全评价
            sql = string.Empty;
            strWhere = string.Empty;
            queryRole = string.Empty;
            sql = @"select count(t.id) num from epg_safetyevaluate t left join epg_outsouringengineer r 
                                on t.projectid=r.id left join base_department e on r.outprojectid=e.departmentid where {0}";

            if (role.Contains("公司级用户") || role.Contains("厂级部门用户"))
            {
                queryRole = string.Format(" (t.createuserorgcode  = '{0}' and t.issend='1' or t.createuserid ='{1}')", currUser.OrganizeCode, currUser.UserId);
            }
            else if (role.Contains("承包商级用户"))
            {
                queryRole = string.Format(" (e.departmentid = '{0}' or t.createuserid ='{1}') ", currUser.DeptId, currUser.UserId);
            }
            else
            {
                queryRole = string.Format(" (r.engineerletdeptid = '{0}' and t.issend='1' or t.createuserid ='{1}') ", currUser.DeptId, currUser.UserId);
            }
            sql = string.Format(sql, queryRole);
            strWhere = " and 1=1";
            sql = sql + strWhere;
            dt = this.BaseRepository().FindTable(sql);
            if (dt.Rows.Count > 0)
            {
                toIndexData.Add(Convert.ToInt32(dt.Rows[0]["num"].ToString()));
            }
            else
            {
                toIndexData.Add(0);
            }
            dt.Dispose();
            #endregion
            return toIndexData;
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
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, OutprojectblacklistEntity entity)
        {
            var rep = DbFactory.Base().BeginTrans();
            try
            {
                Repository<OutsourcingprojectEntity> ourProject = new Repository<OutsourcingprojectEntity>(DbFactory.Base());
                OutsourcingprojectEntity projectEntity = ourProject.FindList(string.Format("select * from epg_outsourcingproject  t where t.OUTPROJECTID='{0}'", entity.OUTPROJECTID)).ToList().FirstOrDefault();
                if (entity.STATE == "0")
                {
                    entity.INBLACKLISTTIME = DateTime.Now;
                    projectEntity.BLACKLISTSTATE = "1";
                }
                else if (entity.STATE == "1")
                {
                    entity.OUTBLACKLISTTIME = DateTime.Now;
                    projectEntity.BLACKLISTSTATE = "0";
                }
                if (!string.IsNullOrEmpty(keyValue))
                {
                    entity.Modify(keyValue);
                    rep.Update<OutprojectblacklistEntity>(entity);
                }
                else
                {
                    entity.Create();
                    rep.Insert<OutprojectblacklistEntity>(entity);
                }
                rep.Update<OutsourcingprojectEntity>(projectEntity);
                rep.Commit();
            }
            catch (System.Exception)
            {
                rep.Rollback();
            }


        }
        #endregion
    }
}
