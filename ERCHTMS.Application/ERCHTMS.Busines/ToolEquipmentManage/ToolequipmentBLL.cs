using ERCHTMS.Entity.ToolEquipmentManage;
using ERCHTMS.IService.ToolEquipmentManage;
using ERCHTMS.Service.ToolEquipmentManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.ToolEquipmentManage
{
    /// <summary>
    /// �� ���������߻�����Ϣ��
    /// </summary>
    public class ToolequipmentBLL
    {
        private ToolequipmentIService service = new ToolequipmentService();

        #region ��ȡ����

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<ToolequipmentEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public ToolequipmentEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

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
        /// ��ȡ�豸���
        /// </summary>
        /// <param name="EquipmentNo">�豸���</param>
        /// <returns></returns>
        public string GetEquipmentNo(string EquipmentNo, string orgcode)
        {
            return service.GetEquipmentNo(EquipmentNo, orgcode);
        }

        /// <summary>
        /// ��ȡ�豸���ͳ��ͼ���б�
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public string GetEquipmentTypeStat(string queryJson)
        {
            return service.GetEquipmentTypeStat(queryJson);
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
        public void SaveForm(string keyValue, ToolequipmentEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion


        public DataTable GetToolRecordList(string keyValue)
        {
            return service.GetToolRecordList(keyValue);
        }

        public void SaveToolrecord(string keyValue, ToolrecordEntity entity)
        {
            service.SaveToolrecord(keyValue, entity);
        }

        public DataTable GetToolStatisticsList(string queryJson)
        {
            return service.GetToolStatisticsList(queryJson);
        }

        public object GetToolName(string tooltype)
        {
            try
            {
                return service.GetToolName(tooltype);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
