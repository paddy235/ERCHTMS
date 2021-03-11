using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using ERCHTMS.Entity.LllegalStandard;
using ERCHTMS.Busines.LllegalStandard;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Code;
using ERCHTMS.Busines.SystemManage;
using System.Web;
using BSFramework.Util.Offices;
using System.Data;
using System.Collections;
using ERCHTMS.Entity.SystemManage.ViewModel;
using BSFramework.Util.Extension;
using ERCHTMS.Busines.AuthorizeManage;

namespace ERCHTMS.Web.Areas.LllegalStandard.Controllers
{
    /// <summary>
    /// 描 述：违章标准表
    /// </summary>
    public class LllegalStandardController : MvcControllerBase
    {
        private LllegalstandardBLL lllegalstandardbll = new LllegalstandardBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();

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
        [HttpGet]
        public ActionResult Select()
        {
            return View();
        }
        #endregion

        #region 页面组件初始化
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetInitDataJson()
        {
            //违章类型 违章等级 作业类型
            string itemCode = "'LllegalType','LllegalLevel','LllegalBusType'";
            //集合
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode(itemCode);
            //返回值
            var josnData = new
            {
                LllegalType = itemlist.Where(p => p.EnCode == "LllegalType"), //违章类型
                LllegalLevel = itemlist.Where(p => p.EnCode == "LllegalLevel"),  //违章级别
                LIIegalBusType = itemlist.Where(p => p.EnCode == "LllegalBusType") //作业类型
            };

            return Content(josnData.ToJson());
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var queryParam = queryJson.ToJObject();
            Operator opertator = new OperatorProvider().Current();
            if (pagination.p_fields.IsEmpty())
            {
                pagination.p_fields = @"createdate,createuserid,createuserdeptcode,createuserorgcode,modifydate,modifyuserid,des,leglevel,legLevalName,legtype,legTypeName,bustype,busTypeName,descore,demoney,firstdescore,firstdemoney,seconddescore,seconddemoney,remark";
            }
            pagination.p_kid = "id";
            pagination.p_tablename = @"v_lllegalstdinfo";
            pagination.conditionJson = " 1=1";
            string authWhere = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "CREATEUSERDEPTCODE", "CREATEUSERORGCODE");
            if (!string.IsNullOrEmpty(authWhere))
            {//数据权限,含系统管理员添加的数据。
                pagination.conditionJson += " and (" + authWhere + " or CREATEUSERORGCODE='00')";
            }
            else
            {
                pagination.conditionJson += " and CREATEUSERORGCODE='" + opertator.OrganizeCode + "'";
            }

            //违章类型
            if (!queryParam["lllegaltype"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and  legtype='{0}' ", queryParam["lllegaltype"].ToString());
            }
            //违章级别
            if (!queryParam["lllegallevel"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and leglevel ='{0}'", queryParam["lllegallevel"].ToString());
            }
            //违章描述 
            if (!queryParam["lllegaldescribe"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and des like '%{0}%'", queryParam["lllegaldescribe"].ToString());
            }
            var data = lllegalstandardbll.GetLllegalStdInfo(pagination, queryJson);
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
            var data = lllegalstandardbll.GetEntity(keyValue);
            //违章类型 违章等级 作业类型
            string itemCode = "'LllegalType','LllegalLevel','LllegalBusType'";
            //集合
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode(itemCode);
            //返回值
            var josnData = new
            {
                data = data,
                LllegalType = itemlist.Where(p => p.EnCode == "LllegalType"), //违章类型
                LllegalLevel = itemlist.Where(p => p.EnCode == "LllegalLevel"),  //违章级别
                LIIegalBusType = itemlist.Where(p => p.EnCode == "LllegalBusType") //作业类型
            };

            return Content(josnData.ToJson());
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
            lllegalstandardbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, LllegalstandardEntity entity)
        {
            lllegalstandardbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion

        #region 导入自评标准
        /// <summary>
        /// 导入自评标准
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportLegStd()
        {
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
                //违章类型 违章等级 作业类型
                string itemCode = "'LllegalType','LllegalLevel','LllegalBusType'";
                //集合
                var itemlist = dataitemdetailbll.GetDataItemListByItemCode(itemCode).ToList();
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                string filePath = Server.MapPath("~/Resource/temp/" + fileName);
                file.SaveAs(filePath);
                DataTable dt = ExcelHelper.ExcelImport(filePath);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    object[] vals = dt.Rows[i].ItemArray;
                    if (IsEndRow(vals) == true)
                        break;
                    var msg = "";
                    if (Validate(i, vals, itemlist, out msg) == true)
                    {
                        var entity = GenEntity(vals, itemlist);
                        lllegalstandardbll.SaveForm("", entity);
                        sussceed++;
                    }
                    else
                    {
                        falseMessage += "第" + (i + 1) + "行" + msg + "</br>";
                        error++;
                    }
                }
                count = sussceed + error;
                message = "共有" + count + "条记录,成功导入" + sussceed + "条，失败" + error + "条";
                message += "</br>" + falseMessage;
                //删除临时文件
                System.IO.File.Delete(filePath);
            }
            return message;
        }
        private bool IsEndRow(object[] vals)
        {
            bool r = false;

            r = Array.TrueForAll(vals, x => (x == null || x == DBNull.Value || x.ToString() == ""));

            return r;
        }
        private bool Validate(int index, object[] vals, List<DataItemModel> item, out string msg)
        {
            var r = true;
            var i = index + 1;
            msg = "";
            if (vals.Length < 10)
            {
                msg += "，格式不正确";
                r = false;
            }
            var obj = vals[0];
            if (obj == null || obj == DBNull.Value || obj.ToString() == "")
            {
                msg += "，违章描述不能为空";
                r = false;
            }
            obj = vals[1];
            if (obj == null || obj == DBNull.Value || obj.ToString() == "")
            {
                msg += "，违章类型不能为空";
                r = false;
            }
            else
            {
                int ncount = item.Count(p => p.EnCode == "LllegalType" && p.ItemName == obj.ToString());
                if (ncount <= 0)
                {
                    msg += "，违章类型不正确";
                    r = false;
                }
            }
            obj = vals[2];
            if (obj == null || obj == DBNull.Value || obj.ToString() == "")
            {
                msg += "，违章级别不能为空";
                r = false;
            }
            else
            {
                int ncount = item.Count(p => p.EnCode == "LllegalLevel" && p.ItemName == obj.ToString());
                if (ncount <= 0)
                {
                    msg += "，违章级别不正确";
                    r = false;
                }
            }
            obj = vals[3];
            if (obj == null || obj == DBNull.Value || obj.ToString() == "")
            {
                msg += "，作业类型不能为空";
                r = false;
            }
            else
            {
                int ncount = item.Count(p => p.EnCode == "LllegalBusType" && p.ItemName == obj.ToString());
                if (ncount <= 0)
                {
                    msg += "，作业类型不正确";
                    r = false;
                }
            }
            obj = vals[4];
            if (obj != null && obj != DBNull.Value)
            {
                double num = 0;
                if (!double.TryParse(obj.ToString(), out num) || num < 0)
                {
                    msg += "，违章责任人扣分只能是大于等于零的数字";
                    r = false;
                }
            }
            obj = vals[5];
            if (obj != null && obj != DBNull.Value)
            {
                double num = 0;
                if (!double.TryParse(obj.ToString(), out num) || num < 0)
                {
                    msg += "，违章责任人考核只能是大于等于零的数字";
                    r = false;
                }
            }
            obj = vals[6];
            if (obj != null && obj != DBNull.Value)
            {
                double num = 0;
                if (!double.TryParse(obj.ToString(), out num) || num < 0)
                {
                    msg += "，第一联责人扣分只能是大于等于零的数字";
                    r = false;
                }
            }
            obj = vals[7];
            if (obj != null && obj != DBNull.Value)
            {
                double num = 0;
                if (!double.TryParse(obj.ToString(), out num) || num < 0)
                {
                    msg += "，第一联责人考核只能是大于等于零的数字";
                    r = false;
                }
            }
            obj = vals[8];
            if (obj != null && obj != DBNull.Value)
            {
                double num = 0;
                if (!double.TryParse(obj.ToString(), out num) || num < 0)
                {
                    msg += "，第二联责人扣分只能是大于等于零的数字";
                    r = false;
                }
            }
            obj = vals[9];
            if (obj != null && obj != DBNull.Value)
            {
                double num = 0;
                if (!double.TryParse(obj.ToString(), out num) || num < 0)
                {
                    msg += "，第二联责人考核只能是大于等于零的数字";
                    r = false;
                }
            }
            if (!string.IsNullOrWhiteSpace(msg))
            {
                msg = string.Format("第{0}行{1}。</br>", i, msg);
                r = false;
            }

            return r;
        }
        private LllegalstandardEntity GenEntity(object[] vals, List<DataItemModel> item)
        {
            LllegalstandardEntity entity = new LllegalstandardEntity();
            entity.DES = vals[0].ToString();
            entity.REMARK = "导入数据";
            var obj = vals[1];
            if (obj != null && obj != DBNull.Value)
            {
                var n = item.FirstOrDefault(p => p.EnCode == "LllegalType" && p.ItemName == obj.ToString());
                if (n != null)
                {
                    entity.LEGTYPE = n.ItemDetailId;
                }
            }
            obj = vals[2];
            if (obj != null && obj != DBNull.Value)
            {
                var n = item.FirstOrDefault(p => p.EnCode == "LllegalLevel" && p.ItemName == obj.ToString());
                if (n != null)
                {
                    entity.LEGLEVEL = n.ItemDetailId;
                }
            }
            obj = vals[3];
            if (obj != null && obj != DBNull.Value)
            {
                var n = item.FirstOrDefault(p => p.EnCode == "LllegalBusType" && p.ItemName == obj.ToString());
                if (n != null)
                {
                    entity.BUSTYPE = n.ItemDetailId;
                }
            }
            obj = vals[4];
            if (obj != null && obj != DBNull.Value)
            {
                decimal num = 0;
                if (decimal.TryParse(obj.ToString(), out num))
                {
                    entity.DESCORE = num;
                }
            }
            obj = vals[5];
            if (obj != null && obj != DBNull.Value)
            {
                double num = 0;
                if (double.TryParse(obj.ToString(), out num))
                {
                    entity.DEMONEY = num;
                }
            }
            obj = vals[6];
            if (obj != null && obj != DBNull.Value)
            {
                decimal num = 0;
                if (decimal.TryParse(obj.ToString(), out num))
                {
                    entity.FIRSTDESCORE = num;
                }
            }
            obj = vals[7];
            if (obj != null && obj != DBNull.Value)
            {
                double num = 0;
                if (double.TryParse(obj.ToString(), out num))
                {
                    entity.FIRSTDEMONEY = num;
                }
            }
            obj = vals[8];
            if (obj != null && obj != DBNull.Value)
            {
                decimal num = 0;
                if (decimal.TryParse(obj.ToString(), out num))
                {
                    entity.SECONDDESCORE = num;
                }
            }
            obj = vals[9];
            if (obj != null && obj != DBNull.Value)
            {
                double num = 0;
                if (double.TryParse(obj.ToString(), out num))
                {
                    entity.SECONDDEMONEY = num;
                }
            }

            return entity;
        }
        #endregion
    }
}
