using ERCHTMS.Entity.EngineeringManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.EngineeringManage
{
    /// <summary>
    /// �� ����Σ�󹤳̹���
    /// </summary>
    public interface PerilEngineeringIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<PerilEngineeringEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        PerilEngineeringEntity GetEntity(string keyValue);
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
        void SaveForm(string keyValue, PerilEngineeringEntity entity);
        #endregion

        #region ͳ��
        /// <summary>
        ///��ȡͳ������
        /// </summary>
        /// <param name="year">���</param>
        /// <returns></returns>
        string GetEngineeringCount(string year = "");

        /// <summary>
        ///��ȡͳ�Ʊ������
        /// </summary>
        /// <param name="year">���</param>
        /// <returns></returns>
        string GetEngineeringList(string year = "");

        /// <summary>
        ///��ȡ����������ͳ������
        /// </summary>
        /// <param name="year">���</param>
        /// <returns></returns>
        string GetEngineeringFile(string year = "");

        /// <summary>
        ///��ȡ����������ͳ������(���)
        /// </summary>
        /// <param name="year">���</param>
        /// <returns></returns>
        string GetEngineeringFileGrid(string year = "");

        /// <summary>
        ///Σ�󹤳�������ͳ��
        /// </summary>
        /// <param name="year">���</param>
        /// <returns></returns>
        string GetEngineeringCase(string year = "");

        /// <summary>
        ///Σ�󹤳�������ͳ�ƣ����
        /// </summary>
        /// <param name="year">���</param>
        /// <returns></returns>
        string GetEngineeringCaseGrid(string year = "");

        /// <summary>
        ///��λ�ڲ�������ί��λ�Ա�
        /// </summary>
        /// <param name="year">���</param>
        /// <returns></returns>
        string GetEngineeringContrast(string year = "", string month = "");

        /// <summary>
        ///��λ�ڲ�������ί��λ�Աȣ����
        /// </summary>
        /// <param name="year">���</param>
        /// <returns></returns>
        string GetEngineeringContrastGrid(string year = "", string month = "");
        #endregion

        #region ʡ��ͳ��
        /// <summary>
        ///���糧��λ�Ա�
        /// </summary>
        /// <param name="year">ͳ�����</param>
        /// <returns></returns>
        string GetEngineeringContrastForSJ(string year = "");

        /// <summary>
        /// ���糧��λ�Աȱ��
        /// </summary>
        /// <param name="year">ͳ�����</param>
        /// <returns></returns>
        DataTable GetEngineeringContrastGridForSJ(string year = "");

        /// <summary>
        /// �������ͳ�Ʊ��
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        DataTable GetEngineeringCategoryGridForSJ(string year = "");

        /// <summary>
        /// �������ͼ��
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        string GetEngineeringCategoryForSJ(string year = "");

        /// <summary>
        /// �¶�����ͼ��
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        string GetEngineeringMonthForSJ(string year = "");

        /// <summary>
        /// �¶����Ʊ��
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        DataTable GetEngineeringMonthGridForSJ(string year = "");

        /// <summary>
        /// ��ȡ�������
        /// </summary>
        /// <returns></returns>
        DataTable GetEngineeringType();
        #endregion

        string GetPerilForSJIndex(string queryJson);

        string GetPeril(string code = "", string st = "", string et = "", string keyword = "");


        #region app�ӿ�
        DataTable GetPerilEngineeringList(string sqlwhere);
        #endregion
    }
}
