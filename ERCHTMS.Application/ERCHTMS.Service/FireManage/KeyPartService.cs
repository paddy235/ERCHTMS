using ERCHTMS.Entity.FireManage;
using ERCHTMS.IService.FireManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using System;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.FireManage
{
    /// <summary>
    /// �� �����ص����λ
    /// </summary>
    public class KeyPartService : RepositoryFactory<KeyPartEntity>, KeyPartIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            if (queryJson != null && queryJson != "")
            {
                var queryParam = queryJson.ToJObject();
                //����
                if (!queryParam["PartNo"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and PartNo='{0}'", queryParam["PartNo"].ToString());
                }
                //����
                if (!queryParam["DutydeptCode"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and DutydeptCode like'{0}%'", queryParam["DutydeptCode"].ToString());
                }
                //ʹ��״̬
                if (!queryParam["EmployState"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and EmployState='{0}'", queryParam["EmployState"].ToString());
                }
                //���𼶱�
                if (!queryParam["Rank"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and Rank='{0}'", queryParam["Rank"].ToString());
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<KeyPartEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public KeyPartEntity GetEntity(string keyValue)
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
        /// <returns></returns>
        public void SaveForm(string keyValue, KeyPartEntity entity)
        {
            entity.Id = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                KeyPartEntity ke = this.BaseRepository().FindEntity(keyValue);
                if (ke == null)
                {
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
        }
        #endregion
    }
}
