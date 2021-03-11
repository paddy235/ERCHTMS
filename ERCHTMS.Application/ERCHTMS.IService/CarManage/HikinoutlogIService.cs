using ERCHTMS.Entity.CarManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.IService.CarManage
{
    /// <summary>
    /// �� �����豸��¼��Ա������־
    /// </summary>
    public interface HikinoutlogIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<HikinoutlogEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡ����ȫ������
        /// </summary>
        /// <returns></returns>
        DataTable GetNums();
        /// <summary>
        /// ��ȡ�������Ա��������
        /// </summary>
        /// <returns></returns>
        Dictionary<string, int> GetTodayCarPeopleCount();
        /// <summary>
        /// ��ȡ���µĳ�����Ա�������ݣ�ȡǰ����
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> GetCarPeopleTopData();

        /// <summary>
        /// ��ȡ�������һ��ˢ����¼
        /// </summary>
        /// <returns></returns>
        HikinoutlogEntity GetLastInoutLog();

        /// <summary>
        /// һ�Ŵ�������ͳ��
        /// </summary>
        /// <returns></returns>
        DataTable GetCarStatistic();


        /// <summary>
        /// �û��б�
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
        HikinoutlogEntity GetEntity(string keyValue);

        /// <summary>
        /// �����û�id��ѯ���û�����δ������¼
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        HikinoutlogEntity GetInUser(string UserId);

        /// <summary>
        /// �����豸ID
        /// </summary>
        /// <param name="HikId"></param>
        /// <returns></returns>
        HikinoutlogEntity DeviceGetLog(string HikId);
        /// <summary>
        /// ��ȡ����һ����������
        /// </summary>
        /// <returns></returns>
        List<AreaModel> GetAccPersonNum();

        /// <summary>
        /// ��ȡ���ڵ�Code��ȡ���������ӽڵ�
        /// </summary>
        /// <returns></returns>
        List<DistrictEntity> GetAreaSon(string code);

        /// <summary>
        /// ��ȡ�����ŵ���Աͳ������
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetAllDepartment(string queryJson);

        /// <summary>
        /// ���ݲ������Ƽ�����Ա����
        /// </summary>
        /// <param name="deptName"></param>
        /// <returns></returns>
        DataTable GetTableByDeptname(Pagination pagination, string deptName,string personName);


        /// <summary>
        /// ��ȡ��Ա���ñ�����
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="ModuleType"></param>
        /// <returns></returns>
        DataTable GetPersonSet(Pagination pagination, string ModuleType);

        /// <summary>
        /// ��ȡ������������
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable Get_BIS_CARVIOLATION(Pagination pagination, string queryJson);
        #endregion

        #region �ύ����

        /// <summary>
        /// ��Աͨ���ύ
        /// </summary>
        /// <param name="insert"></param>
        /// <param name="update"></param>
        void UserAisleSave(HikinoutlogEntity insert, HikinoutlogEntity update);
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
        void SaveForm(string keyValue, HikinoutlogEntity entity);
        List<int[]> GetPersonData();
        int[] GetAreaData();
        System.Collections.IList GetTopFiveById(string hikId);
        HikinoutlogEntity GetFirsetData();

        /// <summary>
        /// �����Ž����豸��Ż�ȡ��ر��
        /// </summary>
        /// <param name="DoorIndexCode">�Ž����豸���</param>
        /// <returns></returns>
        string GetCameraIndexCodeByDoorIndexCode(string DoorIndexCode);

        /// <summary>
        /// ���ڸ澯
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        DataTable GetAttendanceWarningPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// ����ȱ��ͳ��
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        DataTable GetAbsenteeismPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// ��ȡ����ȱ��ͳ����Ա���ñ�����
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="ModuleType"></param>
        /// <returns></returns>
        DataTable GetAbsenteeismPersonSet(Pagination pagination, string ModuleType);


        /// <summary>
        /// ��������������Ա���ɲ�ѯ�Ž�
        /// </summary>
        void SaveAbsenteeismPersonSet(string json, string ModuleType);


        /// <summary>
        /// ������Ա����IDɾ������
        /// </summary>
        void DeleteAbsenteeismPersonSet(string keyValue);

        /// <summary>
        /// ��ȡ���ڸ澯��Ա���ñ�����
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="ModuleType"></param>
        /// <returns></returns>
        DataTable GetAttendanceWarningPersonSet(Pagination pagination, string ModuleType);


        /// <summary>
        /// ��������������Ա���ɲ�ѯ�Ž�
        /// </summary>
        void SaveAttendanceWarningPersonSet(string json, string ModuleType);


        /// <summary>
        /// ������Ա����IDɾ������
        /// </summary>
        void DeleteAttendanceWarningPersonSet(string keyValue);

        /// <summary>
        /// �����û�ID�޸��볡״̬
        /// </summary>
        /// <param name="keyValue"></param>
        void UpdateByID(string keyValue);

        /// <summary>
        /// ��ȡ������Ա�Ž�����ͳ��
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetTableUserRole(Pagination pagination, string queryJson);

        /// <summary>
        /// ��ȡ��Ա�Ž�������ϸ
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetTableByUserID(Pagination pagination, string queryJson);

        /// <summary>
        /// ��������������Ա���ɲ�ѯ�Ž�
        /// </summary>
        /// <param name="json"></param>
        /// <param name="ModuleType"></param>
        void SavePersonSet(string json, string ModuleType);

        /// <summary>
        /// ������Ա����IDɾ������
        /// </summary>
        /// <param name="keyValue"></param>
        void DeletePersonSet(string keyValue);
        #endregion
    }
}
