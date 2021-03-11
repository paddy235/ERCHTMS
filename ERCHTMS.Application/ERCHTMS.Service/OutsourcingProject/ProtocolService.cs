using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using ERCHTMS.Service.BaseManage;

namespace ERCHTMS.Service.OutsourcingProject
{
    /// <summary>
    /// �� ����Э��
    /// </summary>
    public class ProtocolService : RepositoryFactory<ProtocolEntity>, ProtocolIService
    {
        private DepartmentService departmentservice = new DepartmentService();
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public DataTable GetList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
           
            DataTable dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            dt.Columns.Add("createuserorgid", typeof(string));
            foreach (DataRow item in dt.Rows)
            {
                if (!string.IsNullOrWhiteSpace(item["createuserorgcode"].ToString())) {
                    var dlist=new DepartmentService().GetList().Where(x => x.EnCode == item["createuserorgcode"].ToString()).ToList();
                    if (dlist.Count > 0) {
                        item["createuserorgid"] = dlist.FirstOrDefault().DepartmentId;
                    }
                }
            }
            return dt;
        }
        public IEnumerable<ProtocolEntity> GetList()
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        public DataTable GetProFilesList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
           
            if (!string.IsNullOrWhiteSpace(queryParam["ItemCode"].ToString()))
            {
                var dept = departmentservice.GetEntityByCode(queryParam["ItemCode"].ToString());
                if (dept.Nature == "�а���" || dept.Nature == "�ְ���")
                {
                    pagination.conditionJson += string.Format(" and (t.outprojectid ='{0}' or t.supervisorid='{0}')", dept.DepartmentId);
                }
                else
                {
                    while (dept.Nature == "����" || dept.Nature == "רҵ")
                    {
                        dept = departmentservice.GetEntity(dept.ParentId);
                    }
                    pagination.conditionJson += string.Format(" and t.engineerletdeptid in (select departmentid from base_department where encode like '{0}%')", dept.EnCode);
                }
                //pagination.conditionJson += string.Format(" and t.createuserdeptcode  = '{0}'", queryParam["ItemCode"].ToString());
            }
            DataTable dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            dt.Columns.Add("createuserorgid", typeof(string));
            foreach (DataRow item in dt.Rows)
            {
                if (!string.IsNullOrWhiteSpace(item["createuserorgcode"].ToString()))
                {
                    var dlist = new DepartmentService().GetList().Where(x => x.EnCode == item["createuserorgcode"].ToString()).ToList();
                    if (dlist.Count > 0)
                    {
                        item["createuserorgid"] = dlist.FirstOrDefault().DepartmentId;
                    }
                }
            }
            return dt;
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public DataTable GetEntity(string keyValue)
        {
            string sql = string.Format(@"select t.id,r.engineername,r.engineercode,r.engineertype,r.engineerarea,r.engineerlevel,r.engineerletdept,r.engineercontent,
t.firstparty,t.secondparty,t.firstpartyid,t.secondpartyid,t.signplace,t.signdate,t.projectid,r.engineerareaname
 from epg_protocol t left join epg_outsouringengineer r on t.projectid=r.id 
where t.id='{0}'", keyValue);
            DataTable data = this.BaseRepository().FindTable(sql);
            return data;
        }
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
        public void SaveForm(string keyValue, ProtocolEntity entity)
        {
            entity.ID = keyValue;
            //��ʼ����
            var res = DbFactory.Base().BeginTrans();
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    ProtocolEntity se = this.BaseRepository().FindEntity(keyValue);
                    if (se == null)
                    {
                        entity.Create();
                        res.Insert<ProtocolEntity>(entity);
                        //res.Commit();
                    }
                    else
                    {
                        entity.Modify(keyValue);
                        res.Update<ProtocolEntity>(entity);
                    }
                    #region ���¹�������״̬
                    //Repository<CompactEntity> repCompactEntity = new Repository<CompactEntity>(DbFactory.Base());
                    //List<CompactEntity> CompactList = repCompactEntity.FindList(string.Format("select * from EPG_Compact t where t.ProjectID='{0}'", entity.PROJECTID)).ToList();
                    //if (CompactList.Count > 0) {
                    //    CompactEntity Compact = CompactList.FirstOrDefault();
                    //    if (!string.IsNullOrEmpty(Compact.ID))
                    //    {
                          
                    //    }
                    //}
                    Repository<StartappprocessstatusEntity> proecss = new Repository<StartappprocessstatusEntity>(DbFactory.Base());
                    StartappprocessstatusEntity startProecss = proecss.FindList(string.Format("select * from epg_startappprocessstatus t where t.outengineerid='{0}'", entity.PROJECTID)).ToList().FirstOrDefault();
                    startProecss.PACTSTATUS = "1";
                    res.Update<StartappprocessstatusEntity>(startProecss);
                    res.Commit();
                    #endregion
                }
                else
                {
                    entity.Create();
                    res.Insert<ProtocolEntity>(entity);
                    res.Commit();
                }
            }
            catch (System.Exception)
            {
                res.Rollback();
                throw;
            }
        }
        #endregion
    }
}
