using ERCHTMS.Entity.CarManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.CarManage
{
    /// <summary>
    /// �� ����Σ�����س�����
    /// </summary>
    public interface HazardouscarIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<HazardouscarEntity> GetList(string queryJson);

        /// <summary>
        /// ��ҳ��ѯ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        HazardouscarEntity GetEntity(string keyValue);

        /// <summary>
        /// ��ȡ��Σ�������Ƿ������˼���
        /// </summary>
        /// <param name="HazardousId"></param>
        /// <returns></returns>
        bool GetHazardous(string HazardousId);

        /// <summary>
        /// ���ݳ��ƺŻ�ȡ������Ϣ
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        HazardouscarEntity GetCar(string CarNo);

        /// <summary>
        /// ��ȡ����Σ��Ʒ��������
        /// </summary>
        /// <returns></returns>
        List<HazardouscarEntity> GetHazardousList(string day);
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        void SaveForm(string keyValue, HazardouscarEntity entity);
        void SaveFaceUserForm(string keyValue, HazardouscarEntity entity, List<CarUserFileImgEntity> userjson);

        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        void Update(string keyValue, HazardouscarEntity entity);

        /// <summary>
        /// �ı�GPS����Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <param name="pgpslist"></param>
        void ChangeGps(string keyValue, HazardouscarEntity entity, List<PersongpsEntity> pgpslist);

        /// <summary>
        /// �ı�Σ��Ʒ��������״̬λ�������
        /// </summary>
        /// <param name="id"></param>
        void ChangeProcess(string id);

        #endregion
    }
}
