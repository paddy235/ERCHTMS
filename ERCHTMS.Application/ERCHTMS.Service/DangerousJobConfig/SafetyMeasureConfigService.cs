using ERCHTMS.Entity.DangerousJobConfig;
using ERCHTMS.IService.DangerousJobConfig;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Util;
using BSFramework.Data;

namespace ERCHTMS.Service.DangerousJobConfig
{
    /// <summary>
    /// �� ����Σ����ҵ��ȫ��ʩ����
    /// </summary>
    public class SafetyMeasureConfigService : RepositoryFactory<SafetyMeasureConfigEntity>, SafetyMeasureConfigIService
    {
        private SafetyMeasureDetailService safetymeasuredetailservice = new SafetyMeasureDetailService();
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetPageListJson(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            DataTable dt = new DataTable();
            if (!string.IsNullOrWhiteSpace(queryJson))
            {
                var queryParam = queryJson.ToJObject();
                if (!string.IsNullOrWhiteSpace("WorkType"))
                {
                    pagination.conditionJson += string.Format(" and worktype ='{0}'", queryParam["WorkType"].ToString());
                }
            }
            dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            return dt;
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SafetyMeasureConfigEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SafetyMeasureConfigEntity GetEntity(string keyValue)
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
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <param name="list">��������</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, SafetyMeasureConfigEntity entity, List<SafetyMeasureDetailEntity> list)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                var res = GetEntity(keyValue);
                if (res == null)
                {
                    entity.Id = keyValue;
                    entity.Create();
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
            safetymeasuredetailservice.RemoveFormByRecId(entity.Id);
            foreach (var item in list)
            {
                item.RecId = entity.Id;
                safetymeasuredetailservice.SaveForm("", item);
            }
        }
        #endregion
    }
}
