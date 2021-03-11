using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.WebControl;
using ERCHTMS.Code;
using ERCHTMS.Entity.ComprehensiveManage;
using ERCHTMS.IService.ComprehensiveManage;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.Service.ComprehensiveManage
{
    /// <summary>
    /// �� ������Ϣ��������
    /// </summary>
    public class InfoSubmitDetailsService : RepositoryFactory<InfoSubmitDetailsEntity>, InfoSubmitDetailsIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<InfoSubmitDetailsEntity> GetList(string queryJson)
        {
            var sql = string.Format("select * from hrs_infosubmitdetails where 1=1 {0}", queryJson);
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
                pagination.p_fields = @"createdate,createuserid,createuserdeptcode,createusername,createuserorgcode,modifydate,modifyuserid,contents,departname,submitdate,infoid";
            }
            pagination.p_kid = "id";
            pagination.p_tablename = @"hrs_infosubmitdetails";
            pagination.conditionJson = string.Format(" createuserorgcode='{0}' ", user.OrganizeCode);            
            //��Ϣ���
            if (!queryParam["infoid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and infoid = '{0}'", queryParam["infoid"].ToString());
            }
            //�Ƿ��ύ
            if (!queryParam["issubmit"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and issubmit = '{0}'", queryParam["issubmit"].ToString());
            }
            var dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);

            return dt;
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public InfoSubmitDetailsEntity GetEntity(string keyValue)
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
            this.BaseRepository().Delete(x => x.ID == keyValue);
        }
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue"></param>
        public void RemoveFormByInfoId(string keyValue)
        {
            this.BaseRepository().Delete(x => x.InfoId == keyValue);
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, InfoSubmitDetailsEntity entity)
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
        /// <summary>
        /// ���±��״̬
        /// </summary>
        /// <param name="applyId"></param>
        public void UpdateChangedData(string applyId)
        {
        }
        #endregion
    }
}
