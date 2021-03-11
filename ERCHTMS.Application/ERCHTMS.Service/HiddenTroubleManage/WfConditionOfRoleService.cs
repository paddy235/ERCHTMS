using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.IService.HiddenTroubleManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using System.Data;
using ERCHTMS.Code;
using BSFramework.Util;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.HiddenTroubleManage
{
    /// <summary>
    /// �� ������������������ɫ���ӱ�
    /// </summary>
    public class WfConditionOfRoleService : RepositoryFactory<WfConditionOfRoleEntity>, WfConditionOfRoleIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<WfConditionOfRoleEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList().Where(p => p.INSTANCEID == queryJson).OrderBy(p => p.SERIALNUMBERS).ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public WfConditionOfRoleEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion



        #region  ���̲���������Ϣ
        /// <summary>
        /// ��������ʵ����Ϣ
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetInstanceConditionInfoList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            if (pagination.p_fields.IsEmpty())
            {
                pagination.p_fields = @"conditioncode,conditiontype,instanceid,remarks,describes,createdate,conditionfunc,serialnumbers";
            }
            pagination.p_kid = "id";
            pagination.conditionJson = " 1=1";
            var queryParam = queryJson.ToJObject();
            //��ǰ�û�
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.p_tablename = @" bis_wfconditionofrole ";
            //ʵ��id
            if (!queryParam["instanceid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and  instanceid ='{0}' ", queryParam["instanceid"].ToString());
            }
            var dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);

            return dt;
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
        public void SaveForm(string keyValue, WfConditionOfRoleEntity entity)
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
        #endregion
    }
}
