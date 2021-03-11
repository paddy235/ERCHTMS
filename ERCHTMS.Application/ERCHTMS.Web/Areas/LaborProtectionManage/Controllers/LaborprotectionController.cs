using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using ERCHTMS.Entity.LaborProtectionManage;
using ERCHTMS.Busines.LaborProtectionManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Web.Areas.LaborProtectionManage.Controllers
{
    /// <summary>
    /// 描 述：劳动防护用品
    /// </summary>
    public class LaborprotectionController : MvcControllerBase
    {
        private LaborprotectionBLL laborprotectionbll = new LaborprotectionBLL();
        private LaborinfoBLL laborinfobll = new LaborinfoBLL();
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
        /// 物资表获取名称列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetNameList()
        {
            //获取到已选数据
            List<LaborprotectionEntity> laborlist = laborprotectionbll.GetLaborList();

            DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();
            var data = dataItemDetailBLL.GetDataItemListByItemCode("'LaborName'").ToList();
            for (int i = 0; i < data.Count; i++)
            {
                //如果当前没有这个数据 则加入到集合中
                if (laborlist.Where(it => it.Name == data[i].ItemName).Count() == 0)
                {
                    LaborprotectionEntity lb = new LaborprotectionEntity();
                    lb.ID = i.ToString();
                    lb.Name = data[i].ItemName;
                    laborlist.Add(lb);
                }
            }

            return ToJsonResult(laborlist);;
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
            pagination.p_kid = "ID";
            pagination.p_fields = "NAME,NO,UNIT,MODEL,TYPE,TIMENUM,TIMETYPE,NOTE,LABOROPERATIONUSERNAME,LABOROPERATIONTIME,createuserid,createuserdeptcode,createuserorgcode";//注：此处要替换成需要查询的列
            pagination.p_tablename = "BIS_LABORPROTECTION";
            pagination.conditionJson = "1=1";
            pagination.sidx = "CREATEDATE";

            ERCHTMS.Code.Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {

                if (laborinfobll.GetPer())
                {

                    pagination.conditionJson += " and CREATEUSERORGCODE='" + user.OrganizeCode + "'";
                }
                else
                {
                    string where = new ERCHTMS.Busines.AuthorizeManage.AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
                    pagination.conditionJson += " and " + where;
                }
            }

            var data = laborprotectionbll.GetPageListByProc(pagination, queryJson);
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
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = laborprotectionbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取新的编号
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetNo()
        {
            string no = laborprotectionbll.GetNo();
            return no;
        }

        #endregion

        #region 提交数据

        /// <summary>
        /// 导入用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public string ImportLabor()
        {

            var currUser = OperatorProvider.Provider.Current();
            string orgId = OperatorProvider.Provider.Current().OrganizeId;//所属公司

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
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                wb.Open(Server.MapPath("~/Resource/temp/" + fileName));
                Aspose.Cells.Cells cells = wb.Worksheets[0].Cells;
                if (cells.MaxDataRow == 0)
                {
                     message = "没有数据,请选择先填写模板在进行导入!";
                    return message;
                }

                DataTable dt = cells.ExportDataTable(0, 0, cells.MaxDataRow + 1, cells.MaxColumn + 1, true);
                int order = 1;
                IList<LaborprotectionEntity> LaborList = new List<LaborprotectionEntity>();

                //先获取到原始的一个编号
                string no = laborprotectionbll.GetNo();
                int ysno = Convert.ToInt32(no);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    order = i;

                    string Name = dt.Rows[i]["劳动防护用品名称"].ToString();
                    string Unit = dt.Rows[i]["劳动防护用品单位"].ToString();
                    string Model = dt.Rows[i]["型号"].ToString();
                    string Type = dt.Rows[i]["类型"].ToString();
                    string TimeNum = dt.Rows[i]["使用期限时间"].ToString();
                    string TimeType = dt.Rows[i]["使用期限单位"].ToString();
                    string Note = dt.Rows[i]["使用说明"].ToString().Trim();



                    //---****值存在空验证*****--
                    if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Unit))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行值存在空,未能导入.";
                        error++;
                        continue;
                    }


                    LaborprotectionEntity ue = new LaborprotectionEntity();
                    ue.Name = Name;
                    ue.Unit = Unit;
                    ue.Model = Model;
                    ue.Type = Type;
                    if (TimeNum != "")
                        ue.TimeNum = Convert.ToInt32(TimeNum);//工号
                    ue.TimeType = TimeType;
                    ue.Note = Note;
                    ue.No = ysno.ToString();
                    //下一条编号增加
                    ysno++;
                    ue.LaborOperationUserName = currUser.UserName;
                    ue.LaborOperationTime = DateTime.Now;

                    try
                    {
                        laborprotectionbll.SaveForm("", ue);
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
            laborprotectionbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, LaborprotectionEntity entity)
        {
            laborprotectionbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}
