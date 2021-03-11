using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using ERCHTMS.Service.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using ERCHTMS.Code;

namespace ERCHTMS.Busines.OutsourcingProject
{
    /// <summary>
    /// �� ����������������Ϣ��
    /// </summary>
    public class AptitudeinvestigateinfoBLL
    {
        private AptitudeinvestigateinfoIService service = new AptitudeinvestigateinfoService();
        private QualificationIService quaService = new QualificationService();


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
        public IEnumerable<AptitudeinvestigateinfoEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ���������λId��ȡ���һ�����ͨ����������Ϣ
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public AptitudeinvestigateinfoEntity GetListByOutprojectId(string outprojectId)
        {
            return service.GetListByOutprojectId(outprojectId);
        }
        /// <summary>
        /// ��ȡ����֤���б�
        /// </summary>
        /// <returns></returns>
        public IEnumerable<QualificationEntity> GetZzzjList() {
            return quaService.GetList();
        }
        public AptitudeinvestigateinfoEntity GetListByOutengineerId(string outengineerId)
        {
            return service.GetListByOutengineerId(outengineerId);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public AptitudeinvestigateinfoEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }


        public AptitudeinvestigateinfoEntity GetEntityByOutEngineerId(string engineerid) {
            return service.GetEntityByOutEngineerId(engineerid);
        }
        /// <summary>
        /// ��ѯ�������ͼ
        /// </summary>
        /// <param name="keyValue">����</param>
        /// <param name="urlType">��ѯ���ͣ�1 ��λ���� 2 ��Ա���� 3 �����豸���� 4 �綯/��ȫ���������� 5�������� 6�볧��� 7��������</param>
        /// <returns></returns>
        public Flow GetAuditFlowData(string keyValue, string urltype) {
            return service.GetAuditFlowData(keyValue, urltype);
        }
        /// <summary>
        /// ��ѯ�������ͼ-�ֻ���ʹ��
        /// </summary>
        /// <param name="keyValue">����</param>
        /// <param name="urltype">��ѯ���ͣ�1 ��λ���� 2 ��Ա���� 3 �����豸���� 4 �綯/��ȫ���������� 5�������� 6�볧��� 7��������</param>
        /// <returns></returns>
        public List<CheckFlowData> GetAppFlowList(string keyValue, string urltype, Operator currUser)
        {
            return service.GetAppFlowList(keyValue, urltype, currUser);
        }

        public List<CheckFlowList> GetAppCheckFlowList(string keyValue, string urltype, Operator currUser)
        {
            return service.GetAppCheckFlowList(keyValue, urltype, currUser);
        }
        /// <summary>
        /// ��ȡ����֤���б�
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetZzzjPageJson(Pagination pagination, string queryJson) {
            return quaService.GetZzzjPageJson(pagination, queryJson);
        }
        /// <summary>
        /// ��ȡ����֤��ʵ��
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public QualificationEntity GetZzzjFormJson(string keyValue)
        {
            return quaService.GetZzzjFormJson(keyValue);
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
        /// ɾ������֤������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveZzzjForm(string keyValue)
        {
            try
            {
                quaService.RemoveZzzjForm(keyValue);
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
        public void SaveForm(string keyValue, AptitudeinvestigateinfoEntity entity)
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
        /// ��������֤�������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveZzzjForm(string keyValue, QualificationEntity entity) {
            try
            {
                quaService.SaveZzzjForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
