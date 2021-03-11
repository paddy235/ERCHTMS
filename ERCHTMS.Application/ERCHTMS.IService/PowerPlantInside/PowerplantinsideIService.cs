using ERCHTMS.Entity.PowerPlantInside;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.IService.PowerPlantInside
{
    /// <summary>
    /// �� ������λ�ڲ��챨
    /// </summary>
    public interface PowerplantinsideIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<PowerplantinsideEntity> GetList(string queryJson);

        System.Data.DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        PowerplantinsideEntity GetEntity(string keyValue);
        
        /// <summary>
        /// ͳ���б�
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        string GetStatisticsList(int year,string mode);
        
        /// <summary>
        /// �¶ȱ仯ͳ��ͼ
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        string GetStatisticsHighchart(string year, string mode);

        #region  ��ǰ��¼���Ƿ���Ȩ����˲���ȡ��һ�����Ȩ��ʵ��
        /// <summary>
        /// ��ǰ��¼���Ƿ���Ȩ����˲���ȡ��һ�����Ȩ��ʵ��
        /// </summary>
        /// <param name="currUser">��ǰ��¼��</param>
        /// <param name="state">�Ƿ���Ȩ����� 1������� 0 ���������</param>
        /// <param name="moduleName">ģ������</param>
        /// <param name="createdeptid">�����˲���ID</param>
        /// <returns>null-��ǰΪ���һ�����,ManyPowerCheckEntity����һ�����Ȩ��ʵ��</returns>
        ManyPowerCheckEntity CheckAuditPower(Operator currUser, out string state, string moduleName, string createdeptid);
        #endregion

        /// <summary>
        /// ������¹��¼�����
        /// </summary>
        /// <returns></returns>
        string GetAccidentEventNum();
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
        void SaveForm(string keyValue, PowerplantinsideEntity entity);
        #endregion
    }
}
