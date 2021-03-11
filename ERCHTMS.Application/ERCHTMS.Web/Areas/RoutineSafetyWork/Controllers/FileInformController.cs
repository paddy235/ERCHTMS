using ERCHTMS.Entity.RoutineSafetyWork;
using ERCHTMS.Busines.RoutineSafetyWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.OutsourcingProject;
using System.Collections.Generic;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.OutsourcingProject;
using System.Linq;
using System;
using BSFramework.Util.Extension;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using System.Data;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.SystemManage;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ERCHTMS.Web.Areas.RoutineSafetyWork.Controllers
{
    /// <summary>
    /// 描 述：文件通报管理
    /// </summary>
    public class FileInformController : MvcControllerBase
    {
        private FileInformBLL FileInformbll = new FileInformBLL();
        private DailyexamineBLL dailyexaminebll = new DailyexamineBLL();
        private ManyPowerCheckBLL manyPowerCheckbll = new ManyPowerCheckBLL();
        private AptitudeinvestigateauditBLL aptitudeinvestigateauditbll = new AptitudeinvestigateauditBLL();
        private FileInfoBLL fileInfoBLL = new FileInfoBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            //是否需要审核
            ViewBag.IsCheck = 0;
            //查询是否配置流程
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string moduleName = "安全动态";
            List<ManyPowerCheckEntity> powerList = manyPowerCheckbll.GetListBySerialNum(user.OrganizeCode, moduleName);
            if (powerList.Count > 0)
                ViewBag.IsCheck = powerList.Count;
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            //是否需要审核
            ViewBag.IsCheck = 0;
            //查询是否配置流程
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string moduleName = "安全动态";
            List<ManyPowerCheckEntity> powerList = manyPowerCheckbll.GetListBySerialNum(user.OrganizeCode, moduleName);
            if (powerList.Count > 0)
                ViewBag.IsCheck = powerList.Count;
            return View();
        }
        /// <summary>
        /// 审核详情页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ApproveForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
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
            pagination.p_kid = "ID";
            pagination.p_fields = "createuserid,Title,Publisher,IsSend,createusername,createdate,ReleaseTime";
            pagination.p_tablename = "BIS_FileInform t";
            //Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.conditionJson = string.Format(" CREATEUSERORGCODE ='{0}'", user.OrganizeCode);
            
            var watch = CommonHelper.TimerStart();
            var data = FileInformbll.GetPageList(pagination, queryJson);
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
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = FileInformbll.GetList(queryJson);
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
            var data = FileInformbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取大屏展示数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetScrnEntity()
        {
            try
            {
                Operator curUser = OperatorProvider.Provider.Current();
                var list = FileInformbll.GetList("").Where(t => t.CreateUserOrgCode == curUser.OrganizeCode && t.IsSend == "0").OrderBy(t => t.CreateDate).Take(2).ToList();
                List<object> data = new List<object>();
                foreach (var item in list)
                {
                    IList<Photo> pList = new List<Photo>(); //附件
                    DataTable file = fileInfoBLL.GetFiles(item.Id);
                    foreach (DataRow dr in file.Rows)
                    {
                        Photo p = new Photo();
                        p.fileid = dr["fileid"].ToString();
                        p.filename = dr["filename"].ToString();
                        p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + dr["filepath"].ToString().Substring(1);
                        pList.Add(p);
                    }
                    data.Add(new { id = item.Id, title = item.Title, file = pList });
                }
                return Newtonsoft.Json.JsonConvert.SerializeObject(data);
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
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
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            FileInformbll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, FileInformEntity entity)
        {
            FileInformbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}
