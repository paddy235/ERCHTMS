using ERCHTMS.Entity.OccupationalHealthManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System;

namespace ERCHTMS.IService.OccupationalHealthManage
{
    /// <summary>
    /// �� ����ְҵ���������
    /// </summary>
    public interface OccupationalstaffdetailIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<OccupationalstaffdetailEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡְҵ��ͳ�Ʊ�
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        DataTable GetStatisticsUserTable(string year, string where);
        /// <summary>
        /// ��ȡ�µ�ְҵ��ͳ�Ʊ�
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        DataTable NewGetStatisticsUserTable(string year, string where);
        /// <summary>
        /// ��ȡ����ְҵ��ͳ�Ʊ�
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        DataTable GetStatisticsDeptTable(string year, string where);

        /// <summary>
        /// ��ȡ���ְҵ��ͳ�Ʊ�
        /// </summary>
        /// <param name="yearType">����������</param>
        /// <param name="Dept">����EnCode</param>
        /// <returns></returns>
        DataTable GetStatisticsYearTable(int yearType, string Dept, string where);
        /// <summary>
        /// ��ȡ�û�id�µ���������¼
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        DataTable GetUserTable(string userid);

        /// <summary>
        /// ��ȡְҵ����Ա�嵥��Ա����
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        int GetStaffListSum(string queryJson, string where);
        /// <summary>
        /// ��ȡְҵ����Ա�嵥(ȫ����)
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        DataTable GetStaffList(string queryJson, string where);
        /// <summary>
        /// ��ȡְҵ����Ա�嵥
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetStaffListPage(Pagination pagination, string queryJson);
        /// <summary>
        /// ����������ѯ��������
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetTable(string queryJson,string where);
        /// <summary>
        /// ���ݸ�id���Ƿ�������ѯ������Ϣ
        /// </summary>
        /// <param name="Pid">��id</param>
        /// <param name="SickType">�Ƿ�����</param>
        /// <returns></returns>
        IEnumerable<OccupationalstaffdetailEntity> GetList(string Pid, int Issick);

        /// <summary>
        /// �洢���̷�ҳ
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        DataTable GetPageListByProc(Pagination pagination, string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        OccupationalstaffdetailEntity GetEntity(string keyValue);

        /// <summary>
        /// ��ȡ�û�id�µ���������¼ ��
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        DataTable NewGetUserTable(string userid);

        /// <summary>
        /// ��ȡ�û��ĽӴ�Σ������
        /// </summary>
        /// <returns></returns>
        DataTable GetUserHazardfactor(string useraccount);
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
        void SaveForm(string keyValue, OccupationalstaffdetailEntity entity);

        /// <summary>
        /// ���ݹ���id�����޸����ʱ��
        /// </summary>
        /// <param name="time">���ʱ��</param>
        /// <param name="parenid">����id</param>
        /// <returns></returns>
        int UpdateTime(DateTime time, string parenid);

        /// <summary>
        /// ��������ɾ�������û� �������
        /// </summary>
        /// <param name="time">���ʱ��</param>
        /// <param name="parenid">����id</param>
        /// <returns></returns>
        int Delete(string parenid, int SickType);
        #endregion
    }
}
