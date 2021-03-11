using System;
using System.Data;
using System.Linq;
using System.Web;
using ERCHTMS.Entity.NosaManage;
using ERCHTMS.Busines.NosaManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using BSFramework.Util.Offices;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;

namespace ERCHTMS.Web.Areas.NosaManage.Controllers
{
    /// <summary>
    /// 描 述：Nosa区域表
    /// </summary>
    public class NosaareaController : MvcControllerBase
    {
        private NosaareaBLL nosaareabll = new NosaareaBLL();

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
        /// 导入页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }
        #endregion

        #region 获取数据

        /// <summary>
        /// 是否存在相同编号的区域
        /// </summary>
        /// <param name="keyValue">id</param>
        /// <param name="No">编号</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult ExistEleNo(string keyValue, string No)
        {
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string id = "";
            if (keyValue != "")
            {
                id = string.Format("and id<>'{0}'", keyValue);
            }

            var oldList = nosaareabll.GetList(String.Format(" and createuserorgcode='{0}' and no='{1}' {2}", user.OrganizeCode, No, id)).ToList();
            var r = oldList.Count > 0;

            return Success("已存在该区域", r);
        }

        /// <summary>
        /// 获取当前人员手机号
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetPhone(string userid)
        {
            UserBLL userbll = new UserBLL();
            string mobile = userbll.GetEntity(userid).Mobile;
            if (mobile != null)
            {
                return mobile;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 获取是否可以选择部门
        /// </summary>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public string GetIsUpdate()
        {
            var issystem = OperatorProvider.Provider.Current().IsSystem;
            if (!issystem)
            {//如果不是系统管理员
                DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();
                string postname = OperatorProvider.Provider.Current().PostName;
                var data = dataItemDetailBLL.GetDataItemListByItemCode("'NOSA'").ToList();
                foreach (var item in data)
                {
                    string value = item.ItemValue;
                    if (postname.Contains(value))
                    {
                        return "true";
                    }

                }

                //if (OperatorProvider.Provider.Current().RoleName.Contains("公司级用户"))
                //{
                //    return "true";
                //}

                return "flase";


            }
            else
            {
                return "true";
            }
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var data = nosaareabll.GetList(pagination, queryJson);
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
        public ActionResult GetFormJson(string keyValue)
        {
            var data = nosaareabll.GetEntity(keyValue);
            return ToJsonResult(data);
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
            nosaareabll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, NosaareaEntity entity)
        {
            nosaareabll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }

        /// <summary>
        /// 导入自评标准
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportEle()
        {
            UserBLL userbll=new UserBLL();
            NosaeleBLL nosaelebll = new NosaeleBLL();
            int error = 0;
            int sussceed = 0;
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
                string filePath = Server.MapPath("~/Resource/temp/" + fileName);
                file.SaveAs(filePath);

                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                wb.Open(Server.MapPath("~/Resource/temp/" + fileName));
                Aspose.Cells.Cells cells = wb.Worksheets[0].Cells;
                if ((cells.MaxDataRow-3) == 0)
                {
                    message = "没有数据,请选择先填写模板在进行导入!";
                    return message;
                }
                DataTable dt = cells.ExportDataTable(3, 0, (cells.MaxDataRow -2), cells.MaxColumn + 1, true);

                //DataTable dt = ExcelHelper.ExcelImport(filePath);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    object[] vals = dt.Rows[i].ItemArray;
                    if (IsEndRow(vals) == true)
                        break;
                    var msg = "";
                    if (Validate(i, vals, userbll, nosaelebll, out msg) == true)
                    {
                        var entity = GenEntity(vals, userbll, nosaelebll);
                        nosaareabll.SaveForm(entity.ID, entity);
                        sussceed++;
                    }
                    else
                    {
                        falseMessage += "第" + (i + 1) + "行" + msg + "<br/>";
                        error++;
                    }
                }
                count = dt.Rows.Count;
                message = "共有" + count + "条记录,成功导入" + sussceed + "条，失败" + error + "条";
                message += "<br/>" + falseMessage;
                //删除临时文件
                System.IO.File.Delete(filePath);
            }
            return message;
        }
        private NosaareaEntity GenEntity(object[] vals, UserBLL userbll, NosaeleBLL nosaelebll)
        {
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            NosaareaEntity entity = new NosaareaEntity() { ID = Guid.NewGuid().ToString() };
            var oldList = nosaareabll.GetList(String.Format(" and createuserorgcode='{0}' and no='{1}'", user.OrganizeCode, vals[0].ToString().Trim())).ToList();
            if (oldList.Count > 0)
                entity = oldList[0];

            entity.NO = vals[0].ToString().Trim();
            entity.Name = vals[1].ToString().Trim();
            entity.AreaRange = vals[2].ToString().Trim();
            entity.DutyDepartName = vals[3].ToString().Trim();
            entity.DutyUserName = vals[4].ToString().Trim();
            var uEntity = userbll.GetUserInfoByName(entity.DutyDepartName, entity.DutyUserName);
            entity.DutyUserId = uEntity.UserId;
            entity.DutyDepartId = uEntity.DepartmentId;

            entity.Mobile = vals[5].ToString();

            return entity;
        }
        private bool IsEndRow(object[] vals)
        {
            bool r = false;

            r = Array.TrueForAll(vals, x => (x == null || x == DBNull.Value || x.ToString() == ""));

            return r;
        }
        private bool IsNull(object obj)
        {
            return obj == null || obj == DBNull.Value || obj.ToString() == "";
        }
        private bool Validate(int index, object[] vals, UserBLL userbll, NosaeleBLL nosaelebll, out string msg)
        {
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            var r = true;
            var i = index + 1;
            msg = "";
            if (vals.Length < 6)
            {
                msg += ",格式不正确";
                r = false;
            }
            var obj = vals[0];
            if (IsNull(obj))
            {
                msg += ",区域编号不能为空";
                r = false;
            }
            else
            {
                var oldList = nosaelebll.GetList(String.Format(" and createuserorgcode='{0}' and no='{1}'", user.OrganizeCode, obj.ToString().Trim())).ToList();
                if (oldList.Count > 0)
                    msg += "，区域编号已存在";
            }
            obj = vals[1];
            if (IsNull(obj))
            {
                msg += ",区域名称不能为空";
                r = false;
            }
            //obj = vals[2];
            //if (IsNull(obj))
            //{
            //    msg += "，区域范围不能为空";
            //    r = false;
            //}
            obj = vals[3];
            if (IsNull(obj))
            {
                msg += ",区域责任部门不能为空";
                r = false;
            }

            obj = vals[4];
            if (IsNull(obj))
            {
                msg += ",区域代表不能为空";
                r = false;
            }
            else if (!IsNull(vals[3]))
            {
                var entity = userbll.GetUserInfoByName(vals[3].ToString().Trim(), obj.ToString().Trim());
                if (entity == null)
                {
                    msg += ",责任部门不正确或责任部门中不存在相应的区域代表用户";
                    r = false;
                }
            }

            obj = vals[5];//联系电话
            //if (!IsNull(obj))
            //{
            //    var list = nosaelebll.GetList(String.Format(" and createuserorgcode='{0}' and no='{1}'", user.OrganizeCode, obj.ToString().Trim()));
            //    if (list.Count() == 0)
            //    {
            //        msg += "，上级区域编号不存在";
            //        r = false;
            //    }
            //}

            if (!string.IsNullOrWhiteSpace(msg))
            {
                msg += "。";
                r = false;
            }

            return r;
        }
        #endregion
    }
}
