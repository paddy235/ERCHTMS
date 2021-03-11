using ERCHTMS.Entity.AssessmentManage;
using ERCHTMS.Busines.AssessmentManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;

namespace ERCHTMS.Web.Areas.AssessmentManage.Controllers
{
    /// <summary>
    /// 描 述：不适宜项筛选
    /// </summary>
    public class NoSuitableDetailController : MvcControllerBase
    {
        private NoSuitableDetailBLL nosuitabledetailbll = new NoSuitableDetailBLL();
        private KScoreDetailBLL kscoredetailbll = new KScoreDetailBLL();

        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }

        /// <summary>
        /// 不适宜项原因详情
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NoSuitResult()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = nosuitabledetailbll.GetList(queryJson);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
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
        /// 获取实体 
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

        #region 提交数据
        /// <summary>
        /// 删除数据
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
            return Success("删除成功。");
        }

        /// <summary>
        /// 保存表单（新增、修改）
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
                resultEntity.NSuitReason = entity.NSuitReason;//不适宜项原因
                nosuitabledetailbll.SaveForm(resultEntity.Id, resultEntity);
            }
            else
            {
                resultEntity = new NoSuitableDetailEntity();
                resultEntity.NSuitReason = entity.NSuitReason;//不适宜项原因
                resultEntity.AssessmentPlanID = planid;
                resultEntity.ChapterID = smallchaperid;
                nosuitabledetailbll.SaveForm("", resultEntity);
            }
            //如果是不适宜项.看扣分表中是否存在此要素的数据，如果存在，就删除
            KScoreDetailEntity kentity = kscoredetailbll.GetKScoreByPlanOrChapID(planid, smallchaperid);
            if (kentity != null)
                kscoredetailbll.RemoveForm(resultEntity.Id);
            return Success("操作成功。");
        }
        #endregion


        /// <summary>
        /// 根据条件获取数据
        /// </summary>
        [HttpGet]
        public ActionResult GetDetailInfo(string planid)
        {
            var data = nosuitabledetailbll.GetDetailInfo(planid);
            return ToJsonResult(data);
        }


        /// <summary>
        /// 大项的所有小项列表
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
        /// 不适宜项列表
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
