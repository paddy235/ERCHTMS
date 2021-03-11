using ERCHTMS.Entity.StandardSystem;
using ERCHTMS.IService.StandardSystem;
using ERCHTMS.Service.StandardSystem;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Busines.StandardSystem
{
    /// <summary>
    /// �� ������׼����
    /// </summary>
    public class StcategoryBLL
    {
        private StcategoryIService service = new StcategoryService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<StcategoryEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public StcategoryEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// �жϴ˽ڵ����Ƿ����ӽڵ�
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public bool IsHasChild(string parentId)
        {
            return service.IsHasChild(parentId);
        }
        /// <summary>
        /// �Ϲ�������-ȡ����
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StcategoryEntity> GetCategoryList()
        {
            return service.GetCategoryList();
        }
        public IEnumerable<StcategoryEntity> GetRankList(string Category)
        {
            return service.GetRankList(Category);
        }

        /// <summary>
        /// ��ȡʵ��(�������ƻ�parentid)
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public StcategoryEntity GetQueryEntity(string queryJson)
        {
            return service.GetQueryEntity(queryJson);
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
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, StcategoryEntity entity)
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
