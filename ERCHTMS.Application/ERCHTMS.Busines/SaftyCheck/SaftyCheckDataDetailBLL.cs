using ERCHTMS.Entity.SaftyCheck;
using ERCHTMS.IService.SaftyCheck;
using ERCHTMS.Service.SaftyCheck;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Code;

namespace ERCHTMS.Busines.SaftyCheck
{
    /// <summary>
    /// �� ������ȫ��������
    /// </summary>
    public class SaftyCheckDataDetailBLL
    {
        private SaftyCheckDataDetailIService service = new SaftyCheckDataDetailService();

        #region ��ȡ����
        /// <summary>
        /// ���ĵǼ�״̬
        /// </summary>
        public void RegisterPer(string userAccount, string id)
        {
            try
            {
                service.RegisterPer(userAccount, id);
            }
            catch (Exception)
            {
                throw;
            }
        }
          /// <summary>
        /// ��ȡ����¼������ݵ�����
        /// </summary>
        /// <param name="recId"></param>
        /// <returns></returns>
        public int GetCount(string recId)
        {
            return service.GetCount(recId);
        }
        public int GetCheckItemCount(string recId)
        {
            return service.GetCheckItemCount(recId);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SaftyCheckDataDetailEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        public DataTable GetDetails(string ids)
        {
            return service.GetDetails(ids);
        }

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SaftyCheckDataDetailEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// ��ȫ���������б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public IEnumerable<SaftyCheckDataDetailEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// ��ȫ�����б�(ϵͳ����)
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetPageOfSysCreate(Pagination pagination, string queryJson)
        {
            return service.GetPageOfSysCreate(pagination, queryJson);
        }
        /// <summary>
        /// ��ȫ�����б�(ϵͳ����)
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetListOfSysCreate(string queryJson)
        {
            return service.GetListOfSysCreate(queryJson);
        }
        /// <summary>
        /// ��ȡ �������
        /// </summary>
        /// <param name="baseID">���յ�ID</param>
        public DataTable GetPageContent(string baseID)
        {
            return service.GetPageContent(baseID);
        }

         /// <summary>
        /// ��ȡ��Ա��Ҫ������Ŀ����
        /// </summary>
        /// <param name="recId">���ƻ�Id</param>
        /// <param name="userAccount">�û��˺�</param>
        /// <returns></returns>
        public int GetCheckCount(string recId,string userAccount)
        {
            return service.GetCheckCount(recId,userAccount);
        }
        public DataTable GetDataTableList(Pagination pagination, string queryJson)
        {
            return service.GetDataTableList(pagination, queryJson);
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
        /// ɾ������
        /// </summary>
        /// <param name="recid">��ȫ����ID</param>
        public int Remove(string recid)
        {
            try
            {
                return service.Remove(recid);
            }
            catch (Exception)
            {
                return -1;
            }
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, List<SaftyCheckDataDetailEntity> list)
        {
            try
            {
                service.SaveForm(keyValue, list);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// ר�����ƶ��ƻ���������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="list">ʵ�����</param>
        /// <returns></returns>
        public void SaveFormToContent(string keyValue, List<SaftyCheckDataDetailEntity> list)
        {
            try
            {
                service.SaveForm(keyValue, list);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void SaveResultForm(List<SaftyCheckDataDetailEntity> list)
        {
            try
            {
                service.SaveResultForm(list);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Update(string keyValue,SaftyCheckDataDetailEntity entity)
        {
            service.Update(keyValue, entity);
        }

                /// <summary>
        /// ��������Ŀ��Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="list">�����Ŀ</param>
        /// <param name="deptCode">������Ĳ��ţ����Ӣ�Ķ��ŷָ���</param>
        public void Save(string keyValue, List<SaftyCheckDataDetailEntity> list,string deptCode="")
        {
            service.Save(keyValue, list, deptCode);
        }
        public void Save(string keyValue, List<SaftyCheckDataDetailEntity> list, SaftyCheckDataRecordEntity entity, Operator user, string deptCode = "")
        {
            service.Save(keyValue, list,entity,user, deptCode);
        }
        #endregion

        #region ��ȡ����(�ֻ���)
        public IEnumerable<SaftyCheckDataDetailEntity> GetSaftyDataDetail(string safeCheckIdItem)
        {
            return service.GetSaftyDataDetail(safeCheckIdItem);
        }
        public void insertIntoDetails(string checkExcelId, string recid)
        {
            try
            {
                service.insertIntoDetails(checkExcelId,recid);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
