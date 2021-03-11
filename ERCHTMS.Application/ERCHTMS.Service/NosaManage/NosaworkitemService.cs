using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.WebControl;
using ERCHTMS.Code;
using ERCHTMS.Entity.NosaManage;
using ERCHTMS.IService.NosaManage;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.Service.NosaManage
{
    /// <summary>
    /// �� ���������ɹ�
    /// </summary>
    public class NosaworkitemService : RepositoryFactory<NosaworkitemEntity>, NosaworkitemIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<NosaworkitemEntity> GetList(string queryJson)
        {
            var sql = string.Format("select * from hrs_nosaworkitem where 1=1 {0}", queryJson);
            return this.BaseRepository().FindList(sql);
        }
        /// <summary>
        /// ��ȡ��ҳ�б�
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var queryParam = queryJson.ToJObject();
            if (pagination.p_fields.IsEmpty())
            {
                pagination.p_fields = @"createdate,createuserid,createuserdeptcode,createuserorgcode,modifydate,modifyuserid,workid,dutyuserid,dutyusername,dutydepartid,dutydepartname,uploaddate,issubmitted,state,checkuserid,checkusername,checkidea,checkdate,(case when state='�����' then 1 else 2 end) srt";
            }
            pagination.p_kid = "id";
            pagination.p_tablename = @"hrs_nosaworkitem";
            pagination.conditionJson = string.Format(" createuserorgcode='{0}'", user.OrganizeCode);            
            //workid
            if (!queryParam["workid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and workid = '{0}'", queryParam["workid"].ToString());
            }

            var dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);

            return dt;
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public NosaworkitemEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(x=>x.WorkId==keyValue);
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, NosaworkitemEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                var old = GetEntity(keyValue);
                if (old == null)
                {
                    entity.Create();
                    entity.ID = keyValue;
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
                this.BaseRepository().Insert(entity);
            }
        }
        #endregion
    }
}
