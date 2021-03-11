using ERCHTMS.Entity.EquipmentManage;
using ERCHTMS.IService.EquipmentManage;
using ERCHTMS.Service.EquipmentManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.EquipmentManage
{
    /// <summary>
    /// �� ���������豸������Ϣ��
    /// </summary>
    public class EquipmentBLL
    {
        private EquipmentIService service = new EquipmentService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<EquipmentEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public EquipmentEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// ��ȡ�豸���
        /// </summary>
        /// <param name="EquipmentNo">�豸���</param>
        /// <returns></returns>
        public string GetEquipmentNo(string EquipmentNo, string orgcode)
        {
            return service.GetEquipmentNo(EquipmentNo, orgcode);
        }

        /// <summary> 
        /// ͨ���豸id��ȡ�豸�б�
        /// </summary>
        /// <returns></returns>
        public DataTable GetEquipmentTable(string[] ids)
        {
            return service.GetEquipmentTable(ids);
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
        public void SaveForm(string keyValue, EquipmentEntity entity)
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

        /// <summary>
        /// ��ͨ�豸�볡
        /// </summary>
        /// <param name="equipmentId">�û�Id</param>
        /// <param name="leaveTime">�볡ʱ��</param>
        /// <returns></returns>
        public int SetLeave(string equipmentId, string leaveTime, string DepartureReason)
        {
            return service.SetLeave(equipmentId, leaveTime, DepartureReason);
        }
        #endregion
    }
}
