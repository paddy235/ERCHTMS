using BSFramework.Util;
using BSFramework.Util.Offices;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.PersonManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.PersonManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace ERCHTMS.Web.Areas.BaseManage.Controllers
{
    /// <summary>
    /// 描 述：积分设置
    /// </summary>
    public class ScoreSetController : MvcControllerBase
    {
        private ScoreSetBLL scoresetbll = new ScoreSetBLL();

        #region 视图功能
        [HttpGet]
        public ActionResult Import()
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
            var data = scoresetbll.GetList(queryJson);
            return ToJsonResult(data);
        }

        [HttpGet]
        public ActionResult GetItemListJson(string queryJson)
        {
            string sidx = Request.Params["sidx"]; string sord = Request.Params["sord"];
            var data = scoresetbll.GetList(queryJson).Select(t => new { t.Id, t.ItemName, t.ItemType, t.Score }).ToList();
            if (!string.IsNullOrEmpty(sidx))
            {
                if (sidx == "Score")
                {
                    if (sord == "asc")
                    {
                        data = data.OrderBy(t => t.Score).ToList();
                    }
                    else
                    {
                        data = data.OrderByDescending(t => t.Score).ToList();
                    }
                }
                if (sidx == "ItemName")
                {
                    if (sord == "asc")
                    {
                        data = data.OrderBy(t => t.ItemName).ToList();
                    }
                    else
                    {
                        data = data.OrderByDescending(t => t.ItemName).ToList();
                    }
                }
                if (sidx == "ItemType")
                {
                    if (sord == "asc")
                    {
                        data = data.OrderBy(t => t.ItemType).ToList();
                    }
                    else
                    {
                        data = data.OrderByDescending(t => t.ItemType).ToList();
                    }
                }

            }
            else
            {
                data = data.OrderByDescending(t => t.ItemName).ToList();
            }
            return ToJsonResult(data);
        }
        /// <summary>
        /// 列表分页
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>   
        //[HandlerMonitor(3, "分页查询用户信息!")]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "id";
            pagination.p_fields = "ItemName,ItemType,Score,IsAuto,CreateDate,createuserorgcode";
            pagination.p_tablename = "BIS_SCORESET";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "deptcode='00'";
            }
            else
            {
                pagination.conditionJson = string.Format(" (deptcode='00' or deptcode='{0}')", user.OrganizeCode);
            }

            var data = scoresetbll.GetPageJsonList(pagination, queryJson);
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
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetForm(string keyValue)
        {
            var entity = scoresetbll.GetEntity(keyValue);
            return ToJsonResult(entity);
        }

        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            DataItemDetailBLL itemBll = new DataItemDetailBLL();
            ERCHTMS.Entity.SystemManage.DataItemDetailEntity entity = itemBll.GetEntity(keyValue);
            return ToJsonResult(entity);
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
            scoresetbll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        ///  <param name="score">初始积分值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, string score)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DataItemDetailBLL itemBll = new DataItemDetailBLL();
            ERCHTMS.Entity.SystemManage.DataItemDetailEntity entity = itemBll.GetEntity(keyValue);
            if (entity != null)
            {
                entity.ItemCode = user.OrganizeCode;
                entity.ItemName = user.OrganizeName;
                entity.ItemId = "1234567890";
                entity.ItemValue = score;
                itemBll.SaveForm(keyValue, entity);
            }
            else
            {
                entity = new Entity.SystemManage.DataItemDetailEntity();
                entity.ItemDetailId = user.OrganizeId;
                entity.ParentId = "0";
                entity.ItemName = user.OrganizeName;
                entity.ItemCode = user.OrganizeCode;
                entity.ItemId = "1234567890";
                entity.ItemValue = score;
                itemBll.SaveForm(keyValue, entity);
            }

            return Success("操作成功。");
        }

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        ///  <param name="score">初始积分值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult Save(string keyValue, ScoreSetEntity entity)
        {
            scoresetbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }

        #endregion

        #region 数据导出
        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HandlerMonitor(0, "导出数据")]
        public ActionResult Export(string queryJson)
        {
            Pagination pagination = new Pagination();
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "id";
            pagination.p_fields = "ItemName,ItemType,Score,case WHEN  isauto=0 then '否' else '是' end  as isautostr";
            pagination.p_tablename = "(select * from  BIS_SCORESET order by CreateDate desc) t";
            pagination.conditionJson = "1=1";
            pagination.page = 1;
            pagination.rows = 100000000;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "deptcode='00'";
            }
            else
            {
                pagination.conditionJson = string.Format(" (deptcode='00' or deptcode='{0}')", user.OrganizeCode);
            }

            var data = scoresetbll.GetPageJsonList(pagination, queryJson);
            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "积分标准设置";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "积分标准设置.xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();

            listColumnEntity.Add(new ColumnEntity() { Column = "ItemName".ToLower(), ExcelColumn = "积分项目", Alignment = "Center", Width = 20 });
            listColumnEntity.Add(new ColumnEntity() { Column = "ItemType".ToLower(), ExcelColumn = "积分类型", Alignment = "Center", Width = 20 });
            listColumnEntity.Add(new ColumnEntity() { Column = "score".ToLower(), ExcelColumn = "积分标准值（分/次）", Alignment = "Center", Width = 20 });
            listColumnEntity.Add(new ColumnEntity() { Column = "isautostr".ToLower(), ExcelColumn = "是否允许系统自动积分", Alignment = "Center", Width = 20 });
            excelconfig.ColumnEntity = listColumnEntity;

            //调用导出方法
            ExcelHelper.ExcelDownload(data, excelconfig);
            return Success("导出成功。");
        }
        #endregion

        #region 导入数据

        /// <summary>
        /// 导入数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportData()
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            int error = 0;
            string message = "请选择格式正确的文件再导入!";
            string falseMessage = "";
            int count = HttpContext.Request.Files.Count;
            if (count > 0)
            {
                HttpPostedFileBase file = HttpContext.Request.Files[0];
                if (string.IsNullOrEmpty(file.FileName))
                {
                    return message;
                }
                if (!(file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xls") || file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xlsx")))
                {
                    return message;
                }
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName));
                DataTable dt = ExcelHelper.ExcelImport(Server.MapPath("~/Resource/temp/" + fileName));
                int order = 1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    order = i;
                    //积分项目
                    string itemname = dt.Rows[i][0].ToString();

                    //积分类型
                    string itemtype = dt.Rows[i][1].ToString();
                    //积分标准值（分/次）
                    string score = dt.Rows[i][2].ToString();
                    try
                    {
                        var item = new ScoreSetEntity { ItemName = itemname, ItemType = itemtype, Score = int.Parse(score), DeptCode = user.DeptCode, IsAuto = 0 };

                        scoresetbll.SaveForm("", item);

                    }
                    catch
                    {
                        error++;
                    }

                }
                count = dt.Rows.Count;
                message = "共有" + count + "条记录,成功导入" + (count - error) + "条，失败" + error + "条";
                message += "</br>" + falseMessage;
            }

            return message;
        }
        #endregion

    }
}
