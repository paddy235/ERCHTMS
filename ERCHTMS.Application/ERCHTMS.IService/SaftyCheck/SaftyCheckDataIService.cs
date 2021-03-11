using ERCHTMS.Entity.SaftyCheck;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using ERCHTMS.Code;
using System.Data;

namespace ERCHTMS.IService.SaftyCheck
{
    /// <summary>
    /// �� ������ȫ����
    /// </summary>
    public interface SaftyCheckDataIService
    {
        #region ��ȡ����
        /// <summary>
        /// ͨ��folderId ��ȡ��Ӧ���ļ�
        /// </summary>
        /// <param name="folderId"></param>
        /// <returns></returns>
        DataTable GetListByObject(string folderId);
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<SaftyCheckDataEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        SaftyCheckDataEntity GetEntity(string keyValue);
        /// <summary>
        /// ��ȫ�����б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        IEnumerable<SaftyCheckDataEntity> GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// ��ȫ��������ֵ��б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        DataTable GetCheckNamePageList(Pagination pagination, string queryJson);

        /// <summary>
        /// ��ҳ��������
        /// </summary>
        /// <param name="user"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        int[] GetCheckCount(ERCHTMS.Code.Operator user, int mode);
        #endregion

        #region �ύ����
        DataTable GetCheckStat(ERCHTMS.Code.Operator user, int category);
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        void RemoveForm(string keyValue);
        void RemoveCheckName(string keyValue);
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        int SaveForm(string keyValue, SaftyCheckDataEntity entity);
        int SaveCheckName(Operator user, List<CheckNameSetEntity> list);
        #endregion

        #region �ֻ���
        IEnumerable<SaftyCheckDataEntity> selectCheckExcel(Operator user);
        List<DistinctArray> getDistinctGroup(string recid);
        List<DistinctArray> getDistinctGroupDj(string recid, string checkdatatype, Operator user);
        DataTable selectCheckContent(string risknameid, string userAccount, string type);

        DataTable getCheckPlanList(Operator user, string ctype);

        object GetCheckStatistics(ERCHTMS.Code.Operator user, string deptCode);

        DataTable GetCheckObjects(string recId, int mode = 0);

        DataTable GetCheckItems(string checkObjId, string recId, int mode = 0);

        List<object> GetCheckContents(string checkId, int mode = 0);
        /// <summary>
        /// ��ȡ������Υ��������˳������Ϊ������Υ�£�
        /// </summary>
        /// <param name="checkId">����¼Id</param>
        /// <param name="mode">��ѯ��ʽ��0����ȡ��������¼������������Υ��������1����ȡ�����Ŀ�Ǽǵ�������Υ��������</param>
        /// <returns></returns>
        List<int> GetHtAndWzCount(string recId, int mode);
        /// <summary>
        /// ��ȡ����еǼǵ������б�
        /// </summary>
        /// <param name="recId"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        DataTable GetHtList(string recId, int mode);
        /// <summary>
        /// ��ȡ����еǼǵ�Υ���б�
        /// </summary>
        /// <param name="recId"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        DataTable GetWzList(string recId, int mode);
        /// <summary>
        /// ��ȡ�����������
        /// </summary>
        /// <param name="itemid"></param>
        /// <returns></returns>
        DataTable GetCheckContentInfo(string itemid);
        /// <summary>
        /// ��ȡ�豸��Ϣ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DataTable GetEquimentInfo(string id);


        /// <summary>
        /// ִ�������Լƻ������ݹ����Զ��������ƻ�
        /// </summary>
        /// <returns></returns>
        string AutoCreateCheckPlan();
        /// <summary>
        /// �����Ƿ���ֹ�����Լƻ�����
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        int SetStatus(string id, int status);
        #endregion
    }
}
