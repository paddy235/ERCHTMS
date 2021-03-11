using ERCHTMS.Entity.AssessmentManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.AssessmentManage
{
    /// <summary>
    /// �� ���������ܽ�
    /// </summary>
    public interface AssessmentSumIService
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
        IEnumerable<AssessmentSumEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        AssessmentSumEntity GetEntity(string keyValue);

        /// <summary>
        /// ���ݼƻ�id�ʹ���ڵ�id��ȡ
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="chapterid"></param>
        /// <returns></returns>
        AssessmentSumEntity GetSumByPlanOrChapID(string planid, string chapterid);

        /// <summary>
        /// ���ݼƻ�id�ʹ���ڵ�id��ȡ
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="chapterid"></param>
        /// <returns></returns>
        DataTable GetSummarizeInfo(string planid, string chapterid);

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        DataTable GetSumUpPageJson(Pagination pagination, string queryJson);


        /// <summary>
        /// ��ȡ�������������
        /// </summary>
        /// <param name="planid"></param>
        /// <returns></returns>
        DataTable GetSumDataInfo(string planid);

        /// <summary>
        /// ���ݼƻ�idͳ������
        /// </summary>
        /// <param name="planid"></param>
        /// <returns></returns>
        string GetSumDataCount(string planid);


        string GetEveryBigPerson(string planid, string type);


        /// <summary>
        /// �üƻ�ÿ����۷ֺͣ���������,����͵�ʮ��֮�⣩
        /// </summary>
        /// <returns></returns>
        int GetEverySumScore(string planid);

        /// <summary>
        /// �üƻ�ÿ����۷�ֵ�Ϳ۷�ԭ�򣨳�������,����͵�ʮ��֮�⣩
        /// </summary>
        /// <returns></returns>
        string GetEveryResonAndScore(string planid);

        /// <summary>
        /// �üƻ�������͵�ʮ��۷ֺ�
        /// </summary>
        /// <returns></returns>
        int GetEverySumScore2(string planid);

        /// <summary>
        /// �üƻ�������͵�ʮ��۷�ֵ�Ϳ۷�ԭ��
        /// </summary>
        /// <returns></returns>
        string GetEveryResonAndScore2(string planid);

        /// <summary>
        /// �üƻ�����Ԫ�ؿ۷ֺ�
        /// </summary>
        /// <returns></returns>
        int GetEverySumScore3(string planid, string strMarjor);

        /// <summary>
        /// �üƻ�����Ԫ�ؿ۷�ֵ�Ϳ۷�ԭ��
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="strMarjor"></param>
        /// <returns></returns>
        string GetEveryResonAndScore3(string planid, string strMarjor);


        /// <summary>
        /// ����һ��ͳ��
        /// </summary>
        /// <param name="planid"></param>
        /// <returns></returns>
        DataTable GetAffixOne(string planid);


        /// <summary>
        /// ÿ����ı�׼�÷֣���������֣��۷֣����յ÷�
        /// </summary>
        /// <returns></returns>
        DataTable GetEveryBigNoSuitScore(string planid);

        /// <summary>
        /// ��ȡ���ڵ���½ڵ����б�׼��
        /// </summary>
        /// <param name="code"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        int GetBigChapterScore();
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
        void SaveForm(string keyValue, AssessmentSumEntity entity);


        /// <summary>
        /// ���������ƻ�idɾ������
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        int Remove(string planId);
        #endregion
    }
}
