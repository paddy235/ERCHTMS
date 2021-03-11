using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using ERCHTMS.Service.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Busines.HighRiskWork
{
    /// <summary>
    /// �� �����ල��������Ŀ
    /// </summary>
    public class SideCheckProjectBLL
    {
        private SideCheckProjectIService service = new SideCheckProjectService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SideCheckProjectEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SideCheckProjectEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// ��������Ŀ��Ϣ
        /// </summary>
        /// <param name="approveid"></param>
        /// <returns></returns>
        public IEnumerable<SideCheckProjectEntity> GetBigCheckInfo()
        {
            return service.GetBigCheckInfo();
        }

        /// <summary>
        /// ���ݴ�������Ŀid��ȡС������Ŀ
        /// </summary>
        /// <param name="parentid"></param>
        /// <returns></returns>
        public IEnumerable<SideCheckProjectEntity> GetAllSmallCheckInfo(string parentid)
        {
            return service.GetAllSmallCheckInfo(parentid);
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
        public void SaveForm(string keyValue, SideCheckProjectEntity entity)
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
