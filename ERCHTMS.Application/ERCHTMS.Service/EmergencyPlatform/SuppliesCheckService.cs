using ERCHTMS.Entity.EmergencyPlatform;
using ERCHTMS.IService.EmergencyPlatform;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Util;
using ERCHTMS.Code;
using BSFramework.Data;
using BSFramework.Util.Extension;
using System;
using ERCHTMS.Service.BaseManage;

namespace ERCHTMS.Service.EmergencyPlatform
{
    /// <summary>
    /// �� ����Ӧ�����ʼ��
    /// </summary>
    public class SuppliesCheckService : RepositoryFactory<SuppliesCheckEntity>, SuppliesCheckIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SuppliesCheckEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SuppliesCheckEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string role = user.RoleName;
            var queryParam = queryJson.ToJObject();
            #region ���
            pagination.p_kid = "a.id";
            pagination.p_fields = string.IsNullOrWhiteSpace(pagination.p_fields) ? @"a.createuserid,a.createuserdeptcode,a.createuserorgcode,to_char(a.CHECKDATE,'yyyy-MM-dd') as CHECKDATE,a.checkusername,a.checkuserdept,('����' ||  a.checknum || '�� ���ϸ��' || a.badnum || '��') as checkdetail,a.createusername,to_char(a.createdate,'yyyy-MM-dd') as createdate,a.checknum,a.badnum" : pagination.p_fields;
            pagination.p_tablename = @"mae_suppliescheck a";
            if (pagination.sidx == null)
            {
                pagination.sidx = "a.createdate";
            }
            if (pagination.sord == null)
            {
                pagination.sord = "desc";
            }
            #endregion

            //��鿪ʼʱ��
            if (!queryParam["checkstartdate"].IsEmpty())
            {
                string from = queryParam["checkstartdate"].ToString().Trim();
                pagination.conditionJson += string.Format(" and a.checkdate>=to_date('{0}','yyyy-mm-dd')", from);
            }
            //������ʱ��
            if (!queryParam["checkenddate"].IsEmpty())
            {
                string to = Convert.ToDateTime(queryParam["checkenddate"].ToString().Trim()).AddDays(1).ToString("yyyy-MM-dd");
                pagination.conditionJson += string.Format(" and a.checkdate<to_date('{0}','yyyy-mm-dd')", to);
            }
            //�����
            if (!queryParam["checkperson"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and a.checkusername like '%{0}%' ", queryParam["checkperson"].ToString());
            }
            //��鵥λ
            if (!queryParam["code"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and instr(',' || a.checkuserdeptcode || ',', ',{0},') > 0", queryParam["code"].ToString());
            }

            //�������ʹ���
            if (!queryParam["suppliesid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and a.id in(select recid from mae_suppliescheckdetail where suppliesid ='{0}')", queryParam["suppliesid"].ToString());
            }

            DataTable data = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            
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
            var res = DbFactory.Base().BeginTrans();
            try
            {
                res.Delete<SuppliesCheckEntity>(keyValue);
                res.Delete<SuppliesCheckDetailEntity>(t => t.RecId == keyValue);
                res.Commit();
            }
            catch (Exception ex)
            {
                res.Rollback();
                throw ex;
            }
            
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, SuppliesCheckEntity entity)
        {
            var res = DbFactory.Base().BeginTrans();
            SuppliesCheckDetailService detailservice = new SuppliesCheckDetailService();
            try
            {
                if (string.IsNullOrEmpty(entity.Id))
                {
                    entity.Id = string.IsNullOrWhiteSpace(keyValue) ? Guid.NewGuid().ToString() : keyValue; //��entityʵ��������ֵ
                }
                foreach (var item in entity.DetailData)
                {
                    item.RecId = entity.Id;
                    if (!string.IsNullOrWhiteSpace(item.Id)) //����DetailData������ȡ�����ж�Ϊnullִ��insert  ����ִ��update
                    {
                        var detail = detailservice.GetEntity(item.Id);
                        if (detail == null)
                        {
                            item.Create();
                            res.Insert(item);
                        }
                        else
                        {
                            item.Modify(item.Id);
                            res.Update(item);
                        }
                    }
                    else
                    {
                        item.Create();
                        res.Insert(item);
                    }
                }
                UserService userservice = new UserService();
                DataTable dt = userservice.GetUserTable(entity.CheckUserId.Split(','));
                entity.CheckUserDept = string.Join(",", dt.AsEnumerable().Select(d => d.Field<string>("DEPTNAME")).Distinct().ToArray());
                entity.CheckUserDeptCode = string.Join(",", dt.AsEnumerable().Select(d => d.Field<string>("DEPTCODE")).Distinct().ToArray());
                entity.CheckUserDeptId = string.Join(",", dt.AsEnumerable().Select(d => d.Field<string>("DEPARTMENTID")).Distinct().ToArray());
                entity.CheckNum = entity.DetailData.Count;
                entity.BadNum = entity.DetailData.Where(t => t.CheckResult == 1).Count();
                var data = GetEntity(entity.Id); //����entity������ȡ�����ж�Ϊnullִ��insert  ����ִ��update
                if (data == null)
                {
                    entity.Create();
                    res.Insert(entity);
                }
                else
                {
                    entity.DetailData = null;
                    entity.Modify(entity.Id);
                    res.Update(entity);
                }
                res.Commit();
            }
            catch (Exception ex)
            {
                res.Rollback();
                throw ex;
            }
            

        }
        #endregion
    }
}
