using ERCHTMS.Entity.AssessmentManage;
using ERCHTMS.IService.AssessmentManage;
using ERCHTMS.Service.AssessmentManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.AssessmentManage
{
    /// <summary>
    /// �� ���������ܽ�
    /// </summary>
    public class AssessmentSumBLL
    {
        private AssessmentSumIService service = new AssessmentSumService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<AssessmentSumEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public AssessmentSumEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }


        /// <summary>
        /// ���ݼƻ�id�ʹ���ڵ�id��ȡ
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="chapterid"></param>
        /// <returns></returns>
        public AssessmentSumEntity GetSumByPlanOrChapID(string planid, string chapterid)
        {
            return service.GetSumByPlanOrChapID(planid, chapterid);
        }

        /// <summary>
        /// ���ݼƻ�id�ʹ���ڵ�id��ȡ
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="chapterid"></param>
        /// <returns></returns>
        public DataTable GetSummarizeInfo(string planid, string chapterid)
        {
            return service.GetSummarizeInfo(planid, chapterid);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetSumUpPageJson(Pagination pagination, string queryJson)
        {
            return service.GetSumUpPageJson(pagination, queryJson);
        }

        /// <summary>
        /// ��ȡ�������������
        /// </summary>
        /// <param name="planid"></param>
        /// <returns></returns>
        public DataTable GetSumDataInfo(string planid)
        {
            return service.GetSumDataInfo(planid);
        }

        /// <summary>
        /// ���ݼƻ�idͳ������
        /// </summary>
        /// <param name="planid"></param>
        /// <returns></returns>
        public string GetSumDataCount(string planid)
        {
            return service.GetSumDataCount(planid);
        }

        public string GetEveryBigPerson(string planid, string type)
        {
            return service.GetEveryBigPerson(planid, type);
        }

        /// <summary>
        /// �üƻ�ÿ����۷ֺͣ���������,����͵�ʮ��֮�⣩
        /// </summary>
        /// <returns></returns>
        public int GetEverySumScore(string planid)
        {
            return service.GetEverySumScore(planid);
        }

        /// <summary>
        /// �üƻ�ÿ����۷�ֵ�Ϳ۷�ԭ�򣨳�������,����͵�ʮ��֮�⣩
        /// </summary>
        /// <returns></returns>
        public string GetEveryResonAndScore(string planid)
        {
            return service.GetEveryResonAndScore(planid);
        }
         /// <summary>
        /// �üƻ�������͵�ʮ��۷ֺ�
        /// </summary>
        /// <returns></returns>
        public int GetEverySumScore2(string planid)
        {
            return service.GetEverySumScore2(planid);
        }

          /// <summary>
        /// �üƻ�������͵�ʮ��۷�ֵ�Ϳ۷�ԭ��
        /// </summary>
        /// <returns></returns>
        public string GetEveryResonAndScore2(string planid)
        {
            return service.GetEveryResonAndScore2(planid);
        }

          /// <summary>
        /// �üƻ�����Ԫ�ؿ۷ֺ�
        /// </summary>
        /// <returns></returns>
        public int GetEverySumScore3(string planid, string strMarjor)
        {
            return service.GetEverySumScore3(planid,strMarjor);
        }

          /// <summary>
        /// �üƻ�����Ԫ�ؿ۷�ֵ�Ϳ۷�ԭ��
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="strMarjor"></param>
        /// <returns></returns>
        public string GetEveryResonAndScore3(string planid, string strMarjor)
        {
            return service.GetEveryResonAndScore3(planid,strMarjor);
        }

         /// <summary>
        /// ����һ��ͳ��
        /// </summary>
        /// <param name="planid"></param>
        /// <returns></returns>
        public DataTable GetAffixOne(string planid)
        {
            return service.GetAffixOne(planid);
        }

         /// <summary>
        /// ÿ����ı�׼�÷֣���������֣��۷֣����յ÷�
        /// </summary>
        /// <returns></returns>
        public DataTable GetEveryBigNoSuitScore(string planid)
        {
            return service.GetEveryBigNoSuitScore(planid);
        }

          /// <summary>
        /// ��ȡ���ڵ���½ڵ����б�׼��
        /// </summary>
        /// <param name="code"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public int GetBigChapterScore()
        {
            return service.GetBigChapterScore();
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ���������ƻ�idɾ������
        /// </summary>
        /// <param name="planId">�ƻ�id</param>
        public int Remove(string planId)
        {
            try
            {
                service.Remove(planId);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, AssessmentSumEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
