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
    /// �� ���������λ��������
    /// </summary>
    public class OutprojectblacklistService : RepositoryFactory<OutprojectblacklistEntity>, OutprojectblacklistIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<OutprojectblacklistEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public OutprojectblacklistEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// ��ѯ��������λ�б�
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetPageBlackListJson(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //�����λ����
            if (!queryParam["projectname"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and  t.outsourcingname like'%{0}%' ", queryParam["projectname"].ToString());
            }
            //������
            if (!queryParam["BlackState"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and  t.blackliststate='{0}' ", queryParam["BlackState"].ToString());
            }
            //�Ƴ�
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


        #region ������̹���
        /// <summary>
        /// ���󣨺ˣ�����λ������顢���󣨺ˣ�����Ա������顢���󣨺ˣ����������������󣨺ˣ��������豸���ա����󣨺ˣ�����ȫ/�綯���������ա����󣨺ˣ����볧��ɡ����󣨺ˣ�����������
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> ToAuditOutPeoject(Operator currUser)
        {
            List<int> toAuditNum = new List<int>();
            string role = currUser.RoleName;
            string deptId = string.Empty;
            string deptName = string.Empty;

            //��˾���û�ȡ��������
            if (role.Contains("��˾���û�"))
            {
                deptId = currUser.OrganizeId;  //����ID
                deptName = currUser.OrganizeName;//��������
            }
            else
            {
                deptId = currUser.DeptId; //����ID
                deptName = currUser.DeptName; //����ID
            }
            string sql = string.Empty;
            string[] arrRole = role.Split(',');

            string strWhere = string.Empty;
            #region ��λ�����������(��)
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
                //��ȡ��һ�������
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
            #region ��Ա�������
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
                //��ȡ��һ�������
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
            #region �������������
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
                    //��ȡ��һ�������
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
            #region �����豸
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
                //��ȡ��һ�������
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
            #region ��ͨ�豸
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
                //��ȡ��һ�������
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
            #region �볡���
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
                //��ȡ��һ�������
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
            //    //�������
            //    strWhere += string.Format(@" select distinct a.id  from epg_intromission a 
            //                                    left join  bis_manypowercheck b on a.flowid = b.id
            //                                    left join bis_manypowercheck c on  b.serialnum = c.serialnum and b.moduleno = c.moduleno
            //                                    left join epg_outsouringengineer d on a.outengineerid = d.id 
            //                                    where  a.investigatestate ='1'  and  ((c.checkdeptid ='-1' and d.engineerletdeptid='{0}'  and c.checkrolename like '%{1}%')  
            //                                    or (c.checkdeptid =  '{0}'  and c.checkrolename like '%{1}%') or ( c.checkdeptid ='-2' and d.outprojectid='{0}'  and c.checkrolename like '%{1}%'))
            //                                    union 
            //                                    ", deptId, str);

            //    //�������
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
            #region ������������(��)
           
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
                //��ȡ��һ�������
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
            #region ����˰�ȫ��������
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
                //��ȡ��һ�������
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
        /// ��֤�𡢺�ͬ��Э�顢��ȫ�������ס���(��)���ᡢ��ȫ����
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
            else if (currUser.RoleName.Contains("ʡ��"))
            {
                queryRole = string.Format(@" and e.createuserorgcode  in (select encode
                        from base_department d
                        where d.deptcode like '{0}%' and d.nature = '����' and d.description is null)", currUser.NewDeptCode);
            }
            else if (currUser.RoleName.Contains("���������û�") || currUser.RoleName.Contains("��˾���û�"))
            {
                queryRole = string.Format(" and e.createuserorgcode like'%{0}%' ", currUser.OrganizeCode);
            }
            else if (currUser.RoleName.Contains("�а��̼��û�"))
            {
                queryRole = string.Format(" and e.outprojectid ='{0}'", currUser.DeptId);
            }
            else
            {
                queryRole = string.Format(" and e.engineerletdeptid ='{0}'", currUser.DeptId);
            }
            sql = string.Format(sql, queryRole);
            #region ��֤��
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
            #region ��ͬ
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
            #region Э��
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
            #region ��ȫ��������
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
            #region ��(��)����
            sql = string.Empty;
            strWhere = string.Empty;
            queryRole = string.Empty;
            sql = @"select count(id) num from bis_workmeeting where iscommit='1' {0}";
            if (currUser.IsSystem || role.Contains("���������û�") || role.Contains("��˾���û�") || role.Contains("��˾����Ա"))
            {
                queryRole = string.Format(" and createuserorgcode = '{0}' ", currUser.OrganizeCode);
            }
            else
            {
                //�������Ų鿴�����а��̵Ŀ��չ��ᡢ�а��̲鿴�������Ŵ����Ŀ��չ��ᡣ
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
            #region ��ȫ����
            sql = string.Empty;
            strWhere = string.Empty;
            queryRole = string.Empty;
            sql = @"select count(t.id) num from epg_safetyevaluate t left join epg_outsouringengineer r 
                                on t.projectid=r.id left join base_department e on r.outprojectid=e.departmentid where {0}";

            if (role.Contains("��˾���û�") || role.Contains("���������û�"))
            {
                queryRole = string.Format(" (t.createuserorgcode  = '{0}' and t.issend='1' or t.createuserid ='{1}')", currUser.OrganizeCode, currUser.UserId);
            }
            else if (role.Contains("�а��̼��û�"))
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

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
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
