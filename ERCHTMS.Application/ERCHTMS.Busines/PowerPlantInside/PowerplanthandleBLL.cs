using ERCHTMS.Entity.PowerPlantInside;
using ERCHTMS.IService.PowerPlantInside;
using ERCHTMS.Service.PowerPlantInside;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.HighRiskWork.ViewModel;

namespace ERCHTMS.Busines.PowerPlantInside
{
    /// <summary>
    /// �� �����¹��¼�����
    /// </summary>
    public class PowerplanthandleBLL
    {
        private PowerplanthandleIService service = new PowerplanthandleService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<PowerplanthandleEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public PowerplanthandleEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }

        /// <summary>
        /// ��ǰ��¼���Ƿ���Ȩ����˲���ȡ��һ�����Ȩ��ʵ��
        /// </summary>
        /// <param name="currUser">��ǰ��¼��</param>
        /// <param name="state">�Ƿ���Ȩ����� 1������� 0 ���������</param>
        /// <param name="moduleName">ģ������</param>
        /// <param name="createdeptid">�����˲���ID</param>
        /// <returns>null-��ǰΪ���һ�����,ManyPowerCheckEntity����һ�����Ȩ��ʵ��</returns>
        public ManyPowerCheckEntity CheckAuditPower(Operator currUser, out string state, string moduleName, string createdeptid)
        {
            return service.CheckAuditPower(currUser, out state, moduleName, createdeptid);
        }

        /// <summary>
        /// �¹��¼��������
        /// </summary>
        /// <returns></returns>
        public List<string> ToAuditPowerHandle()
        {
            return service.ToAuditPowerHandle();
        }

        public DataTable GetAuditInfo(string keyValue, string modulename)
        {
            return service.GetAuditInfo(keyValue, modulename);
        }

        /// <summary>
        /// ��ȡ����ͼ������Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="modulename"></param>
        /// <returns></returns>
        public DataTable GetReformInfo(string keyValue)
        {
            return service.GetReformInfo(keyValue);
        }

        /// <summary>
        /// ��ȡ����ͼ������Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="modulename"></param>
        /// <returns></returns>
        public DataTable GetCheckInfo(string keyValue, string modulename)
        {
            return service.GetCheckInfo(keyValue, modulename);
        }

        public DataTable GetTableBySql(string sql)
        {
            return service.GetTableBySql(sql);
        }

        public string GetUserName(string flowdeptid, string flowrolename, string type = "", string specialtytype = "")
        {
            return service.GetUserName(flowdeptid, flowrolename, type, specialtytype);
        }

        public List<CheckFlowData> GetAppFlowList(string keyValue)
        {
            return service.GetAppFlowList(keyValue);
        }

        public List<CheckFlowData> GetAppFullFlowList(string keyValue)
        {
            return service.GetAppFullFlowList(keyValue);
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
        public void SaveForm(string keyValue, PowerplanthandleEntity entity)
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
        /// �����¹��¼���¼״̬
        /// </summary>
        /// <param name="keyValue"></param>
        public void UpdateApplyStatus(string keyValue)
        {
            try
            {
                service.UpdateApplyStatus(keyValue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}
