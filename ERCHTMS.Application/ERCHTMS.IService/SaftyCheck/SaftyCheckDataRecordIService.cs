using ERCHTMS.Entity.SaftyCheck;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Code;

namespace ERCHTMS.IService.SaftyCheck
{
    /// <summary>
    /// �� ������ȫ����¼
    /// </summary>
    public interface SaftyCheckDataRecordIService
    {
        #region ��ȡ����
         /// <summary>
        /// �������Ǽ���ѡ�����¼���й���
        /// </summary>
        /// <param name="recId">��ȫ����¼Id</param>
        /// <param name="user"></param>
        /// <returns></returns>
        string GetRecordFromHT(string recId, Operator user);
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        IEnumerable<SaftyCheckDataRecordEntity> GetPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// ��ȡ�����豸��������¼�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        DataTable GetPageListJsonByTz(Pagination pagination);

        /// <summary>
        ///��ȡͳ�Ʊ������
        /// </summary>
        /// <param name="deptCode">���ű���</param>
        /// <param name="year">���</param>
        /// <param name="belongdistrict">����</param>
        /// <param name="ctype">�������</param>
        /// <returns></returns>
        string GetSaftyList(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "");
        string GetGrpSaftyList(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "");
        List<SaftyCheckCountEntity> GetSaftyList(string deptCode, string year, string ctype);
        /// <summary>
        ///��ȡͳ�Ʊ������(�Ա�)
        /// </summary>
        /// <param name="deptCode">���ű���</param>
        /// <param name="year">���</param>
        /// <param name="belongdistrict">����</param>
        /// <param name="ctype">�������</param>
        /// <returns></returns>
        string GetSaftyListDB(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "");
        string GetGrpSaftyListDB(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "");
        /// <summary>
        /// ����ͼͳ������
        /// </summary>
        /// <param name="deptCode">���ű���</param>
        /// <param name="year">���</param>
        /// <param name="belongdistrict">����</param>
        /// <param name="ctype">�������</param>
        /// <returns></returns>
        /// <summary>
        /// ��ȡ�Ա�����
        /// </summary>
        /// <param name="deptCode">���ű���</param>
        /// <param name="year">���</param>
        /// <param name="belongdistrict">����</param>
        /// <param name="ctype">�������</param>
        /// <returns></returns>
        string GetAreaSaftyState(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "");
        string GetGrpAreaSaftyState(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "");
        string getRatherCheckCount(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "");
        string getGrpRatherCheckCount(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "");
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="deptCode">���ű���</param>
        /// <param name="year">���</param>
        /// <param name="belongdistrict">����</param>
        /// <param name="ctype">�������</param>
        /// <returns></returns>

        string getMonthCheckCount(string deptCode, string year = "", string belongdistrict = "", string ctype = "");
        string getGrpMonthCheckCount(string deptCode, string year = "", string belongdistrict = "", string ctype = "");
        /// <summary>
        /// ר��������б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        IEnumerable<SaftyCheckDataRecordEntity> GetPageListForType(Pagination pagination, string queryJson);

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<SaftyCheckDataRecordEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        SaftyCheckDataRecordEntity GetEntity(string keyValue);
        /// <summary>
        /// ����
        /// </summary>
        DataTable ExportData(Pagination pagination, string queryJson);

        /// <summary>
        /// ���ݲ��ź����ͻ�ȡ���ŵļ�������
        /// </summary>
        /// <param name="DeptCode">����code����</param>
        /// <returns></returns>
        DataTable AddDeptCheckTable(string DeptCode, string Type);

        /// <summary>
        /// ���ݲ���CODE��ȡ������Ա����
        /// </summary>
        /// <param name="Encode">����Code</param>
        /// <returns>���ض���Json</returns>
        DataTable GetPeopleByEncode(string Encode);
        #endregion

        #region �ύ����
        /// <summary>
        /// ���ĵǼ���
        /// </summary>
        void RegisterPer(string userAccount,string id);
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
        int SaveForm(string keyValue, SaftyCheckDataRecordEntity entity, ref string recid);
        /// <summary>
        /// �޸��Ѽ����Ա
        /// </summary>
        void UpdateCheckMan(string userAccount);
        #endregion

        #region ��ȡ����(�ֻ���)
        IEnumerable<SaftyCheckDataRecordEntity> GetSaftyDataList(string safeCheckTypeId, string searchString, Operator user,string deptCode="");
        IEnumerable<SaftyCheckDataRecordEntity> GetSaftyDataList(string safeCheckTypeId, string searchString, Operator user, string deptCode ,int page,int size,out int total);
        SaftyCheckDataRecordEntity getSaftyCheckDataRecordEntity(string safeCheckIdItem);
        DataTable getCheckRecordDetail(string safeCheckIdItem, string riskPointId);
        int addDailySafeCheck(SaftyCheckDataRecordEntity se, Operator user);
        DataTable selectCheckPerson(Operator user);
        List<SaftyCheckDataRecordEntity> GetSaftDataIndexList(ERCHTMS.Code.Operator user);
        DataTable GetSaftyCheckDataList(string safeCheckTypeId, long status, Operator user, string deptCode, long page, long size, out int total, string startTime, string endTime);
        
        #endregion

        #region ��ҳԤ��
        DataTable GetSafeCheckWarning(Operator user, string mode = "0");
        decimal GetSafeCheckWarningM(Operator user,string date,int mode=0);

        string GetSafeCheckWarningS();
        decimal GetSafeCheckSumCount(Operator user);
           /// <summary>
        /// ��ȡ��ҳ��ȫ��鿼�˽������
        /// </summary>
        /// <param name="user">��ǰ��¼��user</param>
        /// <param name="time">ͳ��ʱ��</param>
        /// <returns></returns>
        object GetSafeCheckWarningByTime(ERCHTMS.Code.Operator user, string time,int mode=0);
        #endregion

        #region ͳ��
          /// <summary>
        /// ͳ�ƶ��������糧�·����������ͳ��
        /// </summary>
        /// <param name="user"></param>
        /// <param name="startDate">��ʼʱ��</param>
        /// <param name="endDate">����ʱ��</param>
        /// <returns></returns>
        string getCheckTaskCount(Operator user, string startDate = "", string endDate = "");
        DataTable getCheckTaskData(Operator user, string startDate = "", string endDate = "");
           /// <summary>
        /// ��ȫ�������
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetCheckTaskList(Pagination pagination, string queryJson);

        #endregion

        /// <summary>
        /// ������ȫ����¼�ļ����Ա
        /// </summary>
        void UpdateCheckUsers();
    }
}
