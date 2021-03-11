using ERCHTMS.Entity.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.OutsourcingProject
{
    /// <summary>
    /// �� �������������˱�
    /// </summary>
    public interface AptitudeinvestigateauditIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<AptitudeinvestigateauditEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        AptitudeinvestigateauditEntity GetEntity(string keyValue);
        AptitudeinvestigateauditEntity GetAuditEntity(string FKId);



        List<AptitudeinvestigateauditEntity> GetAuditList(string keyValue);
        DataTable GetPageList(Pagination pagination, string queryJson);
          /// <summary>
        /// ��ȡҵ����ص���˼�¼
        /// </summary>
        /// <param name="recId">ҵ���¼Id</param>
        /// <returns></returns>
        DataTable GetAuditRecList(string recId);
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        void RemoveForm(string keyValue);

        /// <summary>
        /// ����ҵ������ɾ������
        /// </summary>
        /// <param name="aptitudeId">ҵ������</param>
        void DeleteFormByAptitudeId(string aptitudeId);
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        void SaveForm(string keyValue, AptitudeinvestigateauditEntity entity);
        /// <summary>
        /// ��������������޸ģ�
        /// ����������ͨ��ͬ�� ���� ��λ ��Ա��Ϣ
        /// �޸������λ�볡״̬,�޸����̱��������״̬
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        void SaveSynchrodata(string keyValue, AptitudeinvestigateauditEntity entity);
        /// <summary>
        /// ��������������޸ģ�
        /// ��ȫ��֤�����
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        void SaveSafetyEamestMoney(string keyValue, AptitudeinvestigateauditEntity entity);


          /// <summary>
        /// ��������������޸ģ�
        /// �����������:���¹���״̬
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        void AuditReturnForWork(string keyValue, AptitudeinvestigateauditEntity entity);

        /// <summary>
        /// ��������������޸ģ�
        /// �����������:���¹���״̬
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        void AuditStartApply(string keyValue, AptitudeinvestigateauditEntity entity);


        List<string> AuditPeopleReview(string keyValue, AptitudeinvestigateauditEntity entity);
        #endregion
    }
}
