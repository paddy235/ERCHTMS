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
    public class SpecialEquipmentBLL
    {
        private SpecialEquipmentIService service = new SpecialEquipmentService();

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
        public IEnumerable<SpecialEquipmentEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SpecialEquipmentEntity GetEntity(string keyValue)
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
        /// ��ȡ�豸���ͳ��ͼ���б�
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetEquipmentTypeStat(string queryJson, IEnumerable<SpecialEquipmentEntity> se)
        {
            return service.GetEquipmentTypeStat(queryJson, se);
        }

        /// <summary>
        /// ��ȡ�豸���ͳ��ͼ���б�
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public object GetOperationFailureStat(string queryJson, IEnumerable<SpecialEquipmentEntity> se)
        {

            return service.GetOperationFailureStat(queryJson, se);
        }
        /// <summary>
        /// ����Id��ȡ�����豸����ͨ�豸��Ϣ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetEquimentList(string id)
        {
            return service.GetEquimentList(id);
        }

        /// <summary> 
        /// ͨ���豸id��ȡ�����豸�б�
        /// </summary>
        /// <returns></returns>
        public DataTable GetSpecialEquipmentTable(string[] ids)
        {
            return service.GetSpecialEquipmentTable(ids);
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
        public void SaveForm(string keyValue, SpecialEquipmentEntity entity)
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
        /// �����豸�볡
        /// </summary>
        /// <param name="equipmentId">�û�Id</param>
        /// <param name="leaveTime">�볡ʱ��</param>
        /// <returns></returns>
        public int SetLeave(string specialequipmentId, string leaveTime, string DepartureReason)
        {
            return service.SetLeave(specialequipmentId, leaveTime, DepartureReason);
        }
        /// <summary>
        /// �����޸ļ�������
        /// </summary>
        /// <param name="equipmentId">�û�Id</param>
        /// <param name="CheckDate">��������</param>
        /// <returns></returns>
        public int SetCheck(string specialequipmentId, string CheckDate)
        {
            return service.SetCheck(specialequipmentId, CheckDate);
        }
        #endregion

        #region ��ȡʡ��ͳ������
        /// <summary>
        /// ��ȡʡ���豸���ͳ��
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetEquipmentTypeStatGridForSJ(string queryJson)
        {
            return service.GetEquipmentTypeStatGridForSJ(queryJson);
        }

        /// <summary>
        /// ��ȡʡ���豸���ͼ��
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public string GetEquipmentTypeStatDataForSJ(string queryJson)
        {
            return service.GetEquipmentTypeStatDataForSJ(queryJson);
        }


        /// <summary>
        /// ��ȡʡ����������ͼ��
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public string GetEquipmentHidDataForSJ(string queryJson)
        {
            return service.GetEquipmentHidDataForSJ(queryJson);
        }

        /// <summary>
        /// ��ȡʡ�������������
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetEquipmentHidGridForSJ(string queryJson)
        {
            return service.GetEquipmentHidGridForSJ(queryJson);
        }

        /// <summary>
        /// ��ȡʡ��������ͼ��
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public string GetEquipmentCheckDataForSJ(string queryJson)
        {
            return service.GetEquipmentCheckDataForSJ(queryJson);
        }

        /// <summary>
        /// ��ȡʡ����ȫ����б�
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetEquipmentCheckGridForSJ(string queryJson)
        {
            return service.GetEquipmentCheckGridForSJ(queryJson);
        }

        /// <summary>
        /// ��ȡʡ�����й���ͼ��
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public string GetEquipmentFailureDataForSJ(string queryJson)
        {
            return service.GetEquipmentFailureDataForSJ(queryJson);
        }

        /// <summary>
        /// ��ȡʡ�����й����б�
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetEquipmentFailureGridForSJ(string queryJson)
        {
            return service.GetEquipmentFailureGridForSJ(queryJson);
        }

        public DataTable GetSafetyCheckRecordForSJ(string queryJson)
        {
            return service.GetSafetyCheckRecordForSJ(queryJson);
        }
        #endregion

        #region app�ӿ�
        public DataTable SelectData(string sql)
        {
            return service.SelectData(sql);
        }
        public int UpdateData(string sql)
        {
            return service.UpdateData(sql);
        }
        #endregion
    }
}
