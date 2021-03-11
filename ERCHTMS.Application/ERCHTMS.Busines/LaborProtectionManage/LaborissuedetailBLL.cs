using ERCHTMS.Entity.LaborProtectionManage;
using ERCHTMS.IService.LaborProtectionManage;
using ERCHTMS.Service.LaborProtectionManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Busines.LaborProtectionManage
{
    /// <summary>
    /// �� �����Ͷ��������ű�����
    /// </summary>
    public class LaborissuedetailBLL
    {
        private LaborissuedetailIService service = new LaborissuedetailService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<LaborissuedetailEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public LaborissuedetailEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// ������Ʒ��id��ȡ���һ�η��ż�¼
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public LaborissuedetailEntity GetOrderLabor(string keyValue)
        {
            return service.GetOrderLabor(keyValue);
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
        public void SaveListForm( string json)
        {
            try
            {
                service.SaveListForm( json);
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
        public void SaveForm(string keyValue, LaborissuedetailEntity entity, string json, string InfoId)
        {
            try
            {
                service.SaveForm(keyValue, entity,json,InfoId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
