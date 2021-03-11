using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.IService.KbsDeviceManage;
using ERCHTMS.Service.KbsDeviceManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using ERCHTMS.Entity.CarManage;

namespace ERCHTMS.Busines.KbsDeviceManage
{
    /// <summary>
    /// �� ��������λ��
    /// </summary>
    public class ArealocationBLL
    {
        private ArealocationIService service = new ArealocationService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<ArealocationEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡ�����������������
        /// </summary>
        /// <returns></returns>
        public List<KbsAreaLocation> GetTable()
        {
            return service.GetTable();
        }

        /// <summary>
        /// ��ȡ��������(������)
        /// </summary>
        /// <returns></returns>
        public List<KbsAreaColor> GetRiskTable()
        {
            return service.GetRiskTable();
        }

        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <returns></returns>
        public List<AreaHiddenCount> GetHiddenCount()
        {
            return service.GetHiddenCount();
        }

        /// <summary>
        /// ��ȡ�����������������(һ������)
        /// </summary>
        /// <returns></returns>
        public List<KbsAreaLocation> GetOneLevelTable()
        {
            return service.GetOneLevelTable();
        }

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public ArealocationEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
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
        public void SaveForm(string keyValue, ArealocationEntity entity)
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
