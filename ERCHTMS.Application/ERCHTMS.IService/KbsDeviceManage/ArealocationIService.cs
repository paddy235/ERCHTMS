using ERCHTMS.Entity.KbsDeviceManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using ERCHTMS.Entity.CarManage;

namespace ERCHTMS.IService.KbsDeviceManage
{
    /// <summary>
    /// �� ��������λ��
    /// </summary>
    public interface ArealocationIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<ArealocationEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡ�����������������
        /// </summary>
        /// <returns></returns>
        List<KbsAreaLocation> GetTable();

        /// <summary>
        /// ��ȡ�����������������
        /// </summary>
        /// <returns></returns>
        List<KbsAreaColor> GetRiskTable();

        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <returns></returns>
        List<AreaHiddenCount> GetHiddenCount();

        /// <summary>
        /// ��ȡ�����������������(һ������)
        /// </summary>
        /// <returns></returns>
        List<KbsAreaLocation> GetOneLevelTable();
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        ArealocationEntity GetEntity(string keyValue);
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
        void SaveForm(string keyValue, ArealocationEntity entity);
        #endregion
    }
}
