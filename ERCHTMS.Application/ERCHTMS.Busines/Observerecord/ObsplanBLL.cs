using ERCHTMS.Entity.Observerecord;
using ERCHTMS.IService.Observerecord;
using ERCHTMS.Service.Observerecord;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Code;
using ERCHTMS.IService.Observerecord;

namespace ERCHTMS.Busines.Observerecord
{
    /// <summary>
    /// �� �����۲�ƻ�
    /// </summary>
    public class ObsplanBLL
    {
        private ObsplanIService service = new ObsplanService();
        private ObsplanEHSIService serviceEHS = new ObsEHSplanService();
        private ObsplanFBIService serviceFB = new ObsFBplanService();
        private ObsplanTZIService serviceTZ = new ObsTZplanService();
        private ObsFeedBackIService obsfeedback = new ObsFeedBackService();
        private ObsFeedBackEHSIService obsfeedbackehs = new ObsFeedBackEHSService();
        private ObsFeedBackFBIService obsfeedbackfb = new ObsFeedBackFBService();

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
        /// ��ȡ����б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
     
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<ObsplanEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public ObsplanEHSEntity GetEHSEntity(string keyValue)
        {
            return serviceEHS.GetEntity(keyValue);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public ObsplanFBEntity GetFBEntity(string keyValue)
        {
            return serviceFB.GetEntity(keyValue);
        }

        public ObsplanTZEntity GetTZEntity(string keyValue)
        {
            return serviceTZ.GetEntity(keyValue);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public ObsplanEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// ���ݹ۲�ƻ�Id������ֽ�Id��ȡ��Ӧ��Ϣ
        /// </summary>
        /// <param name="PlanId">�ƻ�id </param>
        /// <param name="PlanFjId">����ֽ�Id</param>
        /// <returns></returns>
        public DataTable GetPlanById(string PlanId, string PlanFjId, string month)
        {
            return service.GetPlanById(PlanId, PlanFjId, month);
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
        public void SaveForm(string keyValue, ObsplanEntity entity)
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
        public void SaveEHSForm(string keyValue, ObsplanEHSEntity entity)
        {
            try
            {
                serviceEHS.SaveForm(keyValue, entity);
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
        public void SaveFBForm(string keyValue, ObsplanFBEntity entity)
        {
            try
            {
                serviceFB.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CommitEhsData(Operator currUser)
        {
            return service.CommitEhsData(currUser);
        }
        #endregion


        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveFeedBackForm(string keyValue, ObsFeedBackEntity entity)
        {
            try
            {
                obsfeedback.SaveForm(keyValue, entity);
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
        public void SaveFeedBackEHSForm(string keyValue, ObsFeedBackEHSEntity entity)
        {
            try
            {
                obsfeedbackehs.SaveForm(keyValue, entity);
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
        public void SaveFeedBackFBForm(string keyValue, ObsFeedBackFBEntity entity)
        {
            try
            {
                obsfeedbackfb.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ��ѯ����ȸù۲�ƻ���Щ�·ݽ����˹۲��¼
        /// </summary>
        /// <param name="planId">�ƻ�Id</param>
        /// <param name="planfjid">�ƻ�����ֽ�Id</param>
        /// <param name="year">���</param>
        /// <returns></returns>
        public DataTable GetObsRecordIsExist(string planId, string planfjid, string year) {
            try
            {
                return service.GetObsRecordIsExist(planId, planfjid,year);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// ����ģ������-���ż�
        /// </summary>
        /// <param name="obsplan"></param>
        /// <param name="obsplanwork"></param>
        public void InsertImportData(List<ObsplanEntity> obsplan, List<ObsplanworkEntity> obsplanwork) {
            try
            {
                service.InsertImportData(obsplan, obsplanwork);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// ����ģ������-Ehs��
        /// </summary>
        /// <param name="obsplan"></param>
        /// <param name="obsplanwork"></param>
        public void InsertImportData(List<ObsplanEHSEntity> obsplan, List<ObsplanworkEntity> obsplanwork)
        {
            try
            {
                service.InsertImportData(obsplan, obsplanwork);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// ������ȼƻ�
        /// </summary>
        /// <param name="currUser">��ǰ�û�</param>
        /// <param name="oldYear">���Ƶ����</param>
        /// <param name="newYear">���Ƶ������</param>
        /// <returns></returns>
        public bool CopyHistoryData(Operator currUser, string oldYear, string newYear) {

            return service.CopyHistoryData(currUser, oldYear, newYear);
        }
        /// <summary>
        /// ֻ�޸ļƻ��·�ֱ��ͬ����EHS�뷢��������
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void SynchData(string planid, string planfjid) {

            service.SynchData(planid, planfjid);
        }
    }
}
