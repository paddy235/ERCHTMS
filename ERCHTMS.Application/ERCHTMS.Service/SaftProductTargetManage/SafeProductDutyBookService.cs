using ERCHTMS.Entity.SaftProductTargetManage;
using ERCHTMS.IService.SaftProductTargetManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.Data;

namespace ERCHTMS.Service.SaftProductTargetManage
{
    /// <summary>
    /// �� ������ȫ����������
    /// </summary>
    public class SafeProductDutyBookService : RepositoryFactory<SafeProductDutyBookEntity>, SafeProductDutyBookIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SafeProductDutyBookEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SafeProductDutyBookEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// ��ȡ��ȫĿ���������б�
        /// </summary>
        /// <param name="ProductId">��ȫĿ��id</param>
        /// <returns></returns>
        public IEnumerable<SafeProductDutyBookEntity> GetListByProductId(string productId)
        {
            return this.BaseRepository().IQueryable().ToList().Where(t => t.ProductId == productId);
        }

        /// <summary>
        /// �б��ҳ
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public IEnumerable<SafeProductDutyBookEntity> GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();
            if (!queryParam["productId"].IsEmpty())
            {
                string productid = queryParam["productId"].ToString();
                pagination.conditionJson += string.Format(" and ProductId='{0}'", productid);
            }
            else
            {
                pagination.conditionJson += string.Format(" and 1=2");
            }
            IEnumerable<SafeProductDutyBookEntity> list = this.BaseRepository().FindListByProcPager(pagination, dataType);
            return list;

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
        /// ���ݰ�ȫ����Ŀ��IDɾ������
        /// </summary>
        /// <param name="planId">��ȫ����Ŀ��id</param>
        public int Remove(string productId)
        {
            return this.BaseRepository().ExecuteBySql(string.Format("delete from bis_safeproductdutybook where productid='{0}'", productId));
        }

        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, SafeProductDutyBookEntity entity)
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
