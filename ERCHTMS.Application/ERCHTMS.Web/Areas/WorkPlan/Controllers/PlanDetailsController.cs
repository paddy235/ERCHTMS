using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.Offices;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Busines.WorkPlan;
using ERCHTMS.Code;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ERCHTMS.Entity.WorkPlan;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.WorkPlan.Controllers
{
    /// <summary>
    /// 描 述：工作计划详情
    /// </summary>
    public class PlanDetailsController : MvcControllerBase
    {
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL(); //流程业务对象
        private UserBLL userbll = new UserBLL(); //用户操作对象
        private DepartmentBLL deptbll = new DepartmentBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private PlanDetailsBLL planDetailsbll = new PlanDetailsBLL();
        
        #region 视图功能        
        /// <summary>
        /// 添加修改页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }
        /// <summary>
        /// 变更页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ChangeForm()
        {
            return View();
        }
        /// <summary>
        /// 变更历史
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult History()
        {
            return View();
        }
        /// <summary>
        /// 添加修改页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult BaseForm()
        {
            return View();
        }
        /// <summary>
        /// 统计页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Statistics()
        {
            return View();
        }
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.ehsDepartCode = "";
            //当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            DataItemModel ehsDepart = dataitemdetailbll.GetDataItemListByItemCode("'EHSDepartment'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();
            if (ehsDepart != null)
                ViewBag.ehsDepartCode = ehsDepart.ItemValue;

            return View();
        }
        /// <summary>
        /// 部门的个人工作计划内容
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DepartIndex()
        {
            return View();
        }
        #endregion

        #region 获取数据        
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            //当前用户
            Operator curUser = OperatorProvider.Provider.Current();            
            DataItemModel ehsDepart = dataitemdetailbll.GetDataItemListByItemCode("'EHSDepartment'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();
            var ehsDeptCode = "";
            if (ehsDepart != null)
                ehsDeptCode = ehsDepart.ItemValue;
            var data = planDetailsbll.GetEntity(keyValue);
            object baseData = null;
            if (data != null)
            {
                baseData = new PlanApplyBLL().GetEntity(data.ApplyId);
            }
            //返回值
            var josnData = new
            {
                data,
                baseData,
                ehsDeptCode
            };

            return Content(josnData.ToJson());
        }
        /// <summary>
        /// 获取初始化数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetInitDataJson()
        {
            //当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            DataItemModel ehsDepart = dataitemdetailbll.GetDataItemListByItemCode("'EHSDepartment'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();
            var ehsDeptCode = "";
            if (ehsDepart != null)
                ehsDeptCode = ehsDepart.ItemValue;          
            //返回值
            var josnData = new
            {       
                ehsDeptCode
            };

            return Content(josnData.ToJson());
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var data = planDetailsbll.GetList(pagination, queryJson);
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return Content(JsonData.ToJson());
        }
        #endregion

        #region 统计
        /// <summary>
        /// 统计计划数据
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult StatisticsNumber(string queryJson)
        {
            //当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            var deptId = curUser.DeptId;
            var querParam = queryJson.ToJObject();
            if (querParam["departid"].IsEmpty())
            {                
                DataItemModel ehsDepart = dataitemdetailbll.GetDataItemListByItemCode("'EHSDepartment'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();  
                if (ehsDepart != null && curUser.DeptCode==ehsDepart.ItemValue)
                    deptId = curUser.OrganizeId;//EHS部门查看全厂统计，其他部门看本部门及子部门数据                
            }
            else
            {
                deptId = querParam["departid"].ToString();
            }
            var dt = planDetailsbll.Statistics(deptId, querParam["starttime"].ToString(), querParam["endtime"].ToString(), querParam["applytype"].ToString());
            
            var jsonData = new
            {
                categories = dt == null ? new List<string>() : from a in dt.Select() select a.ItemArray[1],
                grp1 = dt == null ? new List<string>() : from a in dt.Select() select a.ItemArray[2],
                grp2 = dt == null ? new List<string>() : from a in dt.Select() select a.ItemArray[3],
                grp3 = dt == null ? new List<string>() : from a in dt.Select() select a.ItemArray[4],
                data = dt
            };

            return Content(jsonData.ToJson());
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
            planDetailsbll.RemoveForm(keyValue);//删除      

            return Success("删除成功。");
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="pEntity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, PlanDetailsEntity pEntity)
        {
            if (pEntity.PlanFinDate.HasValue)
            {
                pEntity.PlanFinDate = pEntity.PlanFinDate.Value.AddMonths(1).AddSeconds(-1);//取本月底。
            }            
            planDetailsbll.SaveForm(keyValue, pEntity);

            return Success("操作成功。");
        }
        /// <summary>
        /// 变更保存
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="pEntity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveChangeForm(string keyValue, PlanDetailsEntity pEntity)
        {
            //保存历史
            var newApplyId = Request["NewApplyId"];
            var oldEntity = planDetailsbll.GetEntity(keyValue);
            if (!string.IsNullOrEmpty(newApplyId) && oldEntity != null)
            {
                var hisEntity = planDetailsbll.GetList(string.Format(" and baseid='{0}' and not exists(select 1 from hrs_planapply a where a.id=hrs_plandetails.applyid)",pEntity.ID)).FirstOrDefault();
                if (hisEntity == null)
                {//首次变更，保存历史
                    oldEntity.ID = Guid.NewGuid().ToString();
                    oldEntity.ApplyId = newApplyId;                    
                    oldEntity.BaseId = pEntity.ID;
                    planDetailsbll.SaveForm("", oldEntity);
                }
                else
                {//纠正来因未提交丢失的历史数据
                    hisEntity.ApplyId = newApplyId;
                    planDetailsbll.SaveForm(hisEntity.ID, hisEntity);
                }
            }
            //更新记录
            if (pEntity.PlanFinDate.HasValue)
            {
                pEntity.PlanFinDate = pEntity.PlanFinDate.Value.AddMonths(1).AddSeconds(-1);//取本月底。
            }
            pEntity.IsChanged = 1;
            pEntity.IsMark = 1;
            planDetailsbll.SaveForm(keyValue, pEntity);

            return Success("操作成功。");
        }
        /// <summary>
        /// 按月保存
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="pEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveMonth(string keyValue, PlanDetailsEntity pEntity)
        {
            if (pEntity.PlanFinDate.HasValue)
            {
                pEntity.PlanFinDate = pEntity.PlanFinDate.Value.AddMonths(1).AddSeconds(-1);//取本月底。
            }
            planDetailsbll.SaveMonth(keyValue, pEntity);

            return Success("操作成功。");
        }
        #endregion

        #region 导出数据
        [HandlerMonitor(0, "数据导出")]
        public ActionResult Export(string queryJson,string sortname,string sortorder)
        {
            var pagination = new Pagination()
            {
                page = 1,
                rows = 10000,
                sidx = string.IsNullOrWhiteSpace(sortname) ? "planfindate" : sortname,
                sord = string.IsNullOrWhiteSpace(sortorder) ? "asc" : sortorder
            };
            var dt = planDetailsbll.GetList(pagination, queryJson);
            var now = DateTime.Now;
            foreach(DataRow dr in dt.Rows)
            {
                DateTime pdt = DateTime.Parse(dr["planfindate"].ToString());
                object obj = dr["realfindate"];
                if(obj!=null && obj != DBNull.Value)
                {
                    var rdt = DateTime.Parse(obj.ToString());
                    dr["state"] = (pdt < rdt) ? "超期完成" : "按时完成";
                }
                else
                {
                    dr["state"] = (pdt < now) ?  "未完成": "待完成";
                }
                dr["plandate"] = pdt.ToString("yyyy-MM");
            }
            string title = string.Format("工作计划清单");
            var queryParam = queryJson.ToJObject();
            if(!queryParam["starttime"].IsEmpty()&& !queryParam["endtime"].IsEmpty())
            {
                title = string.Format("{0}-{1}工作计划清单",DateTime.Parse(queryParam["starttime"].ToString()).ToString("yyyy年MM月"), DateTime.Parse(queryParam["endtime"].ToString()).ToString("yyyy年MM月"));
            }
            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = title;       
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 16;
            excelconfig.FileName = string.Format("工作计划清单_{0}.xls",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            excelconfig.IsAllSizeColumn = false;//此属性为True时，数据过大会导致Excel列宽过长报错
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            ColumnEntity columnentity = new ColumnEntity();
            listColumnEntity.Add(new ColumnEntity() { Column = "contents".ToLower(), ExcelColumn = "工作内容", Alignment = "left", Width = 15 });
            listColumnEntity.Add(new ColumnEntity() { Column = "dutydepartname".ToLower(), ExcelColumn = "责任部门", Width = 15 });
            listColumnEntity.Add(new ColumnEntity() { Column = "dutyusername".ToLower(), ExcelColumn = "责任人", Width = 15 });
            listColumnEntity.Add(new ColumnEntity() { Column = "plandate".ToLower(), ExcelColumn = "计划完成时间", Width = 15 });
            listColumnEntity.Add(new ColumnEntity() { Column = "realfindate".ToLower(), ExcelColumn = "实际完成时间", Width = 15 });
            listColumnEntity.Add(new ColumnEntity() { Column = "stdname".ToLower(), ExcelColumn = "管理标准",Width=60 });
            listColumnEntity.Add(new ColumnEntity() { Column = "state".ToLower(), ExcelColumn = "完成情况", Width = 15 });
            excelconfig.ColumnEntity = listColumnEntity;
          
          
            //调用导出方法
            ExcelHelper.ExcelDownload(dt, excelconfig);

            return Success("导出成功。");
        }
        #endregion        
    }
}