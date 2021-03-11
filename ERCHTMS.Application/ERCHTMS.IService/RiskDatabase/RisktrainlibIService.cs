using ERCHTMS.Entity.RiskDatabase;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.RiskDatabase
{
    /// <summary>
    /// �� ��������Ԥ֪ѵ����
    /// </summary>
    public interface RisktrainlibIService
    {
        #region ��ȡ����
        DataTable GetPageListJson(Pagination pagination, string queryJson);
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<RisktrainlibEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        RisktrainlibEntity GetEntity(string keyValue);

        /// <summary>
        /// ��ȡ��ҵ��ȫ������
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        DataTable GetRisktrainlibList(string p);
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// ɾ����Դ���տ�����
        /// </summary>
        /// <param name="keyValue">����</param>
        bool DelRiskData();
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        void SaveForm(string keyValue, RisktrainlibEntity entity, List<RisktrainlibdetailEntity> listMesures);
        void InsertRiskTrainLib(List<RisktrainlibEntity> RiskLib);

        void InsertImportData(List<RisktrainlibEntity> RiskLib, List<RisktrainlibdetailEntity> detailLib);
        #endregion


    }
}
