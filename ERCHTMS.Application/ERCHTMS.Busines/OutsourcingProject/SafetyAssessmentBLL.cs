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
    /// �� ������ȫ��������
    /// </summary>
    public class SafetyAssessmentBLL
    {
        private SafetyAssessmentIService service = new SafetyAssessmentService();

        #region ��ȡ����
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tiem"></param>
        /// <returns></returns>
        public DataTable ExportDataTotal(string time, string deptid)
        {
            return service.ExportDataTotal(time, deptid);
        }

        /// <summary>
        /// ��ȡ�ڲ����ŵ�λ
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public DataTable GetInDeptData()
        {
            return service.GetInDeptData();
        }

        /// <summary>
        /// �ڲ����ŵ���
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public DataTable ExportDataInDept(string time, string deptid)
        {
            return service.ExportDataInDept(time, deptid);
        }

        /// <summary>
        /// �ⲿ����
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public DataTable ExportDataOutDept(string time, string deptid)
        {
            return service.ExportDataOutDept(time, deptid);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SafetyAssessmentEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SafetyAssessmentEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        public int GetFormJsontotal(string keyValue)
        {
            return service.GetFormJsontotal(keyValue);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }

        /// <summary>
        /// ��ȡ��ţ�ÿ�³�ʼ������λ��ˮ��
        /// </summary>
        /// <returns></returns>
        public string GetMaxCode()
        {
            return service.GetMaxCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetApplyNum()
        {
            return service.GetApplyNum();
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
        public void SaveForm(string keyValue, SafetyAssessmentEntity entity)
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

        #region  ��ǰ��¼���Ƿ���Ȩ����˲���ȡ��һ�����Ȩ��ʵ��
        /// <summary>
        /// ��ǰ��¼���Ƿ���Ȩ����˲���ȡ��һ�����Ȩ��ʵ��
        /// </summary>
        /// <param name="currUser">��ǰ��¼��</param>
        /// <param name="state">�Ƿ���Ȩ����� 1������� 0 ���������</param>
        /// <param name="moduleName">ģ������</param>
        /// <param name="createdeptid">�����˲���ID</param>
        /// <returns>null-��ǰΪ���һ�����,ManyPowerCheckEntity����һ�����Ȩ��ʵ��</returns>
        public ManyPowerCheckEntity CheckAuditPower(Operator currUser, out string state, string moduleName, string createdeptid,string startnum)
        {
            return service.CheckAuditPower(currUser, out state, moduleName, createdeptid, startnum);
        }

        /// <summary>
        /// ��ѯ�������ͼ
        /// </summary>
        /// <param name="keyValue">����</param>
        /// <param name="urltype">��ѯ���ͣ�0����ȫ����</param>
        /// <returns></returns>
        public Flow GetAuditFlowData(string keyValue, string urltype)
        {
            return service.GetAuditFlowData(keyValue, urltype);
        }
        #endregion
    }
}
