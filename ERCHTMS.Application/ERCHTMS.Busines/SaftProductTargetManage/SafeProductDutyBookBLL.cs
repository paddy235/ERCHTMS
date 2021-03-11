using ERCHTMS.Entity.SaftProductTargetManage;
using ERCHTMS.IService.SaftProductTargetManage;
using ERCHTMS.Service.SaftProductTargetManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Busines.SaftProductTargetManage
{
    /// <summary>
    /// �� ������ȫ����������
    /// </summary>
    public class SafeProductDutyBookBLL
    {
        private SafeProductDutyBookIService service = new SafeProductDutyBookService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SafeProductDutyBookEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SafeProductDutyBookEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// ��ȡ��ȫĿ���������б�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public IEnumerable<SafeProductDutyBookEntity> GetListByProductId(string productId)
        {
            return service.GetListByProductId(productId);
        }

        /// <summary>
        /// �б��ҳ
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public IEnumerable<SafeProductDutyBookEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// ���ݰ�ȫ����Ŀ��idɾ������
        /// </summary>
        /// <param name="planId">Ŀ��ID</param>
        public int Remove(string productId)
        {
            try
            {
                service.Remove(productId);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, SafeProductDutyBookEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
