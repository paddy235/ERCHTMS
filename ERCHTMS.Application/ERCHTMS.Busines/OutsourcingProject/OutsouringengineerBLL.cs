using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using ERCHTMS.Service.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.HiddenTroubleManage;

namespace ERCHTMS.Busines.OutsourcingProject
{
    /// <summary>
    /// �� �������������Ϣ��
    /// </summary>
    public class OutsouringengineerBLL
    {
        private OutsouringengineerIService service = new OutsouringengineerService();

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

        public DataTable GetIndexToList(Pagination pagination, string queryJson) {
            return service.GetIndexToList(pagination, queryJson);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<OutsouringengineerEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<OutsouringengineerEntity> GetList()
        {
            return service.GetList();
        }
        /// <summary>
        /// ���ݵ�ǰ��¼��Id��ȡ�������
        /// </summary>
        /// <param name="mode">001--��λ���� 002--��Ա���� 003--��ͬ 004--Э�� 005--��ȫ�������� 006--�������� 
        /// 007--�綯���������� 008--�����豸���� 009--�볡��� 010--�������� 011--��֤�� 012--��ȫ����</param>
        /// <param name="outProjectId"></param>
        /// <returns></returns>
        public DataTable GetEngineerDataByCurrdeptId(Operator currUser, string mode = "", string orgid = "")
        {
            return service.GetEngineerDataByCurrdeptId(currUser, mode, orgid);
        }

        public DataTable GetEngineerDataByCondition(Operator currUser, string mode = "", string orgid = "") 
        {
            return service.GetEngineerDataByCondition(currUser, mode, orgid);
        }
        /// <summary>
        /// ���������λId��ȡ�������
        /// </summary>
        /// <param name="outProjectId"></param>
        /// <returns></returns>
        public DataTable GetEngineerDataByWBId(string deptId,string mode="")
        {
            return service.GetEngineerDataByWBId(deptId, mode);
        }
        /// <summary>
        /// ���ݵ�¼��id ��ѯ�Ѿ��ڽ��Ĺ���(�Ѿ�ͨ����������Ĺ���)
        /// </summary>
        /// <returns></returns>
        public DataTable GetOnTheStock(Operator currUser)
        {
            return service.GetOnTheStock(currUser);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public OutsouringengineerEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// ���ݵ�ǰ��¼�� ��ȡ�Ѿ�ͣ���Ĺ�����Ϣ
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        public DataTable GetStopEngineerList()
        {
            return service.GetStopEngineerList();
        }
        #region ����ͳ��
        public string GetTypeCount(string deptid, string year = "")
        {
            return service.GetTypeCount(deptid, year);
        }
        public string GetTypeList(string deptid, string year = "")
        {
            return service.GetTypeList(deptid, year);
        }
        public string GetStateCount(string deptid, string year = "")
        {
            return service.GetStateCount(deptid, year);
        }
        public string GetStateList(string deptid, string year = "")
        {
            return service.GetStateList(deptid, year);
        }
        /// <summary>
        /// ��ȡ���̵�����״̬ͼ
        /// </summary>
        /// <param name="keyValue">����Id</param>
        /// <returns></returns>
        public Flow GetFlow(string keyValue) {
            return service.GetProjectFlow(keyValue);
        }

        public DataTable GetEngineerByCurrDept() {
            return service.GetEngineerByCurrDept();
        }
        #endregion

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
        public void SaveForm(string keyValue, OutsouringengineerEntity entity)
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
        public bool ProIsOver(string keyValue)
        {
            try
            {
                return service.ProIsOver(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
      
        #endregion
    }
}
