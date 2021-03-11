using ERCHTMS.Entity.AssessmentManage;
using ERCHTMS.Busines.AssessmentManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;

namespace ERCHTMS.Web.Areas.AssessmentManage.Controllers
{
    /// <summary>
    /// �� ������������ɸѡ
    /// </summary>
    public class NoSuitableDetailController : MvcControllerBase
    {
        private NoSuitableDetailBLL nosuitabledetailbll = new NoSuitableDetailBLL();
        private KScoreDetailBLL kscoredetailbll = new KScoreDetailBLL();

        #region ��ͼ����
        /// <summary>
        /// �б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }

        /// <summary>
        /// ��������ԭ������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NoSuitResult()
        {
            return View();
        }
        #endregion

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = nosuitabledetailbll.GetList(queryJson);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.p_kid = "a.Id";
            pagination.p_fields = "a.PlanName,'' as progress,'' as Status,'' as score,'' as isUpdate,'' as able,b.count ";
            pagination.p_tablename = string.Format("bis_assessmentplan a left join (select assessmentplanid,count(assessmentplanid) as count from bis_nosuitabledetail where createuserid='{0}' group by assessmentplanid ) b  on a.id=b.assessmentplanid", user.UserId);
            pagination.conditionJson = "1=1";
            var data = nosuitabledetailbll.GetPageListJson(pagination, queryJson);
            var watch = CommonHelper.TimerStart();
            var jsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return ToJsonResult(jsonData);
        }

        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="smallchaperid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetFormJson(string planid, string smallchaperid)
        {
            NoSuitableDetailEntity resultEntity = nosuitabledetailbll.GetNoSuitByPlanOrChapID(planid, smallchaperid);
            return ToJsonResult(resultEntity);
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="smallchaperid"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string planid, string smallchaperid)
        {
            NoSuitableDetailEntity resultEntity = nosuitabledetailbll.GetNoSuitByPlanOrChapID(planid, smallchaperid);
            if (resultEntity != null)
            {
                nosuitabledetailbll.RemoveForm(resultEntity.Id);
            }
            return Success("ɾ���ɹ���");
        }

        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="smallchaperid"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string planid, string smallchaperid, NoSuitableDetailEntity entity)
        {
            NoSuitableDetailEntity resultEntity = nosuitabledetailbll.GetNoSuitByPlanOrChapID(planid, smallchaperid);
            if (resultEntity != null)
            {
                resultEntity.NSuitReason = entity.NSuitReason;//��������ԭ��
                nosuitabledetailbll.SaveForm(resultEntity.Id, resultEntity);
            }
            else
            {
                resultEntity = new NoSuitableDetailEntity();
                resultEntity.NSuitReason = entity.NSuitReason;//��������ԭ��
                resultEntity.AssessmentPlanID = planid;
                resultEntity.ChapterID = smallchaperid;
                nosuitabledetailbll.SaveForm("", resultEntity);
            }
            //����ǲ�������.���۷ֱ����Ƿ���ڴ�Ҫ�ص����ݣ�������ڣ���ɾ��
            KScoreDetailEntity kentity = kscoredetailbll.GetKScoreByPlanOrChapID(planid, smallchaperid);
            if (kentity != null)
                kscoredetailbll.RemoveForm(resultEntity.Id);
            return Success("�����ɹ���");
        }
        #endregion


        /// <summary>
        /// ����������ȡ����
        /// </summary>
        [HttpGet]
        public ActionResult GetDetailInfo(string planid)
        {
            var data = nosuitabledetailbll.GetDetailInfo(planid);
            return ToJsonResult(data);
        }


        /// <summary>
        /// ���������С���б�
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="chaperOrfrist"></param>
        /// <param name="planid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAllDetailPage(Pagination pagination, string chaperOrfrist, string planid)
        {
            pagination.p_kid = "a.Id as sid";
            pagination.p_fields = "b.id as nsuitid,MajorNumber,ChaptersName,Content,Score,nsuitreason,ChaptersParentID";
            pagination.p_tablename = string.Format(@" bis_assessmentchapters  a left join (select * from bis_nosuitabledetail where assessmentplanid='{0}') b on a.id=b.chapterid", planid);
            pagination.conditionJson = string.Format("ChaptersParentID='{0}' and Content is not null", chaperOrfrist);
            var data = nosuitabledetailbll.GetAllDetailPage(pagination);
            var watch = CommonHelper.TimerStart();
            var jsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return ToJsonResult(jsonData);
        }



        /// <summary>
        /// ���������б�
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="chaperOrfrist"></param>
        /// <param name="planid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetNoSuitDetailPage(Pagination pagination, string chaperOrfrist, string planid)
        {
            pagination.p_kid = "a.Id";
            pagination.p_fields = "MajorNumber,ChaptersName,Content,Score,NSuitReason,b.createusername";
            pagination.p_tablename = " bis_nosuitabledetail b  left join bis_assessmentchapters a  on a.id=b.chapterid";
            pagination.conditionJson = string.Format("chapterid in( select id from bis_assessmentchapters where  ChaptersParentID='{0}') and assessmentplanid='{1}'", chaperOrfrist, planid);
            var data = nosuitabledetailbll.GetAllDetailPage(pagination);
            var watch = CommonHelper.TimerStart();
            var jsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return ToJsonResult(jsonData);
        }

    }
}
