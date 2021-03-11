using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.Busines.HighRiskWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;

namespace ERCHTMS.Web.Areas.HighRiskWork.Controllers
{
    /// <summary>
    /// 描 述：起重吊装作业操作人员表
    /// </summary>
    public class LifthoistpersonController : MvcControllerBase
    {
        private LifthoistpersonBLL lifthoistpersonbll = new LifthoistpersonBLL();

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
            var data = lifthoistpersonbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = lifthoistpersonbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }


        /// <summary>
        /// 证件号不能重复
        /// </summary>
        /// <param name="CertificateNum">证件号</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExistCertificateNum(string CertificateNum, string keyValue)
        {
            try
            {
                bool IsOk = lifthoistpersonbll.ExistCertificateNum(CertificateNum, keyValue);
                return Content(IsOk.ToString());
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            
        }

        /// <summary>
        /// 获取起重吊装人员信息列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="workId">起重吊装作业主键ID</param>
        /// <returns></returns>
        public ActionResult GetPersonListJson(Pagination pagination, string workId)
        {
            try
            {
                Operator user = OperatorProvider.Provider.Current();
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "a.ID";
                pagination.p_fields = "a.recid,a.PERSONTYPE,a.personname,a.personid,a.CERTIFICATENUM,filelist.filenum,a.belongdeptname";
                pagination.p_tablename = @" BIS_LIFTHOISTPERSON a left join (select id,count(fileid) as filenum from BIS_LIFTHOISTPERSON person left join base_fileinfo fileinfo on person.id=fileinfo.recid group by person.id) filelist on a.id=filelist.id ";
                pagination.conditionJson = string.Format("a.recid='{0}'", workId);
                pagination.sidx = "a.createdate";
                pagination.sord = "desc";
                var data = lifthoistpersonbll.GetPageList(pagination, "");
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
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RemoveForm(string keyValue)
        {
            lifthoistpersonbll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveForm(string keyValue, LifthoistpersonEntity entity)
        {
            lifthoistpersonbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}
