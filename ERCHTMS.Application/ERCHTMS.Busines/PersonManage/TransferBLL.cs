using ERCHTMS.Entity.PersonManage;
using ERCHTMS.IService.PersonManage;
using ERCHTMS.Service.PersonManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.PersonManage
{
    /// <summary>
    /// �� ����ת����Ϣ��
    /// </summary>
    public class TransferBLL
    {
        private TransferIService service = new TransferService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<TransferEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public TransferEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public TransferEntity GetUsertraEntity(string keyValue)
        {
            return service.GetUsertraEntity(keyValue);
        }

        /// <summary>
        /// ��ȡ��ǰ�û�����ת�ڴ�������
        /// </summary>
        /// <returns></returns>
        public int GetTransferNum()
        {
            return service.GetTransferNum();
        }

        /// <summary>
        /// ��ȡ�����б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public DataTable GetPageListByProc(Pagination pagination, string queryJson)
        {
            return service.GetTransferList(pagination, queryJson);
        }

        /// <summary>
        /// ���ݵ�ǰ����id��ȡ�㼶��ʾ����
        /// </summary>
        /// <param name="deptid"></param>
        /// <returns></returns>
        public string GetDeptName(string deptid)
        {
            return service.GetDeptName(deptid);
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
        public void SaveForm(string keyValue, TransferEntity entity)
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
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void AppSaveForm(string keyValue, TransferEntity entity, string Userid)
        {
            try
            {
                service.AppSaveForm(keyValue, entity, Userid);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ת��ȷ�ϲ���
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void Update(string keyValue, TransferEntity entity)
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
