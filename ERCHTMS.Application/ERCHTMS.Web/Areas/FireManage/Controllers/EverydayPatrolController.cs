using ERCHTMS.Entity.FireManage;
using ERCHTMS.Busines.FireManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ERCHTMS.Busines.SystemManage;
using System.Linq;
using Newtonsoft.Json;
using System;
using ERCHTMS.Entity.SystemManage;
using System.Data;
using System.Web;
using BSFramework.Util.Extension;
using Aspose.Words;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Busines.PublicInfoManage;

namespace ERCHTMS.Web.Areas.FireManage.Controllers
{
    /// <summary>
    /// 描 述：日常巡查
    /// </summary>
    public class EverydayPatrolController : MvcControllerBase
    {
        private EverydayPatrolBLL everydaypatrolbll = new EverydayPatrolBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private EverydayPatrolDetailBLL everydaypatroldetailbll = new EverydayPatrolDetailBLL();
        private AffirmRecordBLL affirmrecordbll = new AffirmRecordBLL();

        #region 视图功能
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
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            //ViewBag.ItemDetailValue = "";
            ////获取日常巡查ID
            //DataItemDetailEntity entity = dataitemdetailbll.GetListByItemCodeEntity("EverydayPatrol");
            //if (entity != null)
            //    ViewBag.ItemDetailValue = entity.ItemValue;
            ViewBag.ItemDetailValue1 = "";//日检
            ViewBag.ItemDetailValue2 = "";//周检
            ViewBag.ItemDetailValue3 = "";//月检
            ViewBag.ItemDetailValue4 = "";//其他
            //获取日常巡查ID
            DataItemDetailEntity entity1 = dataitemdetailbll.GetListByItemCodeEntity("RJ");
            if (entity1 != null)
                ViewBag.ItemDetailValue1 = entity1.ItemValue;
            DataItemDetailEntity entity2 = dataitemdetailbll.GetListByItemCodeEntity("ZJ");
            if (entity2 != null)
                ViewBag.ItemDetailValue2 = entity2.ItemValue;
            DataItemDetailEntity entity3 = dataitemdetailbll.GetListByItemCodeEntity("YJ");
            if (entity3 != null)
                ViewBag.ItemDetailValue3 = entity3.ItemValue;
            DataItemDetailEntity entity4 = dataitemdetailbll.GetListByItemCodeEntity("QT");
            if (entity4 != null)
                ViewBag.ItemDetailValue4 = entity4.ItemValue;
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
            queryJson = queryJson ?? "";
            pagination.p_kid = "Id";
            pagination.p_fields = "PatrolType,PatrolTypeCode,AffirmState,AffirmUserId,District,PATROLDEPT,PATROLDATE,PATROLPERSON,PATROLPLACE,PROBLEMNUM,createuserid,createuserdeptcode,createuserorgcode,DutyUser,ByDept,Signature";
            pagination.p_tablename = "HRS_EVERYDAYPATROL";
            pagination.conditionJson = "1=1";
            var watch = CommonHelper.TimerStart();
            var data = everydaypatrolbll.GetPageList(pagination, queryJson);
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
            var data = everydaypatrolbll.GetList(queryJson);
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
            var data = everydaypatrolbll.GetEntity(keyValue);
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
            everydaypatrolbll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="postData">实体对象json</param>
        /// <param name="jsonData">明细json</param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, string postData, string jsonData)
        {
            //everydaypatrolbll.SaveForm(keyValue, entity);
            //return Success("操作成功。");
            try
            {
                List<EverydayPatrolDetailEntity> projects = JsonConvert.DeserializeObject<List<EverydayPatrolDetailEntity>>(jsonData);
                EverydayPatrolEntity model = JsonConvert.DeserializeObject<EverydayPatrolEntity>(postData);
                if (projects == null)
                {
                    return Error("保存出错，错误信息：参数为null");
                }
                var num = 0;
                if (projects.Count > 0)
                {
                    foreach (var item in projects)
                    {
                        if (item.Result == 1)
                        {
                            num = num + 1;
                        }
                        everydaypatroldetailbll.SaveForm(item.Id, item);//保存明细
                    }
                    model.ProblemNum = num;
                    everydaypatrolbll.SaveForm(keyValue, model);//保存主表
                }
            }
            catch (Exception ex)
            {
                return Error("保存出错，错误信息：" + ex.Message);
            }

            return Success("提交成功。");
        }
        #endregion

       /// <summary>
       /// 查询部门负责人
       /// </summary>
       /// <param name="departmentid"></param>
       /// <returns></returns>
        [HttpGet]
        public ActionResult GetMajorUserId(string departmentid)
        {
            string majorUserId = everydaypatrolbll.GetMajorUserId(departmentid);
            return Content(majorUserId);
        }

        /// <summary>
        /// 复制附件
        /// </summary>
        /// <param name="postData"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostFile(string postData, string keyValue)
        {
            FileInfoEntity fileInfoEntity = new FileInfoEntity();
            FileInfoBLL fileInfoBLL = new FileInfoBLL();

            var dt = JsonConvert.DeserializeObject<List<FileInfoEntity>>(postData);
            List<FileInfoEntity> projects = dt;
            string dir = string.Format("~/Resource/{0}/{1}", "ht/images", DateTime.Now.ToString("yyyyMMdd"));
            foreach (FileInfoEntity item in dt)
            {
                var filepath = Server.MapPath(item.FilePath);
                if (System.IO.File.Exists(filepath))
                {
                    string newFileName = Guid.NewGuid().ToString() + item.FileExtensions;
                    string newFilePath = dir + "/" + newFileName;
                    System.IO.File.Copy(filepath, Server.MapPath(newFilePath));
                }
                item.RecId = keyValue;
                item.FileId = Guid.NewGuid().ToString();
                fileInfoBLL.SaveForm("", item);
            }
            //fileInfoBLL.SaveForm("", fileInfoEntity);
            return Success("操作成功。");
        }

        #region 数据导出
        /// <summary>
        /// 导出日常巡查
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "导出日常巡查清单")]
        public ActionResult Export(string queryJson)
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "ID";
            pagination.p_fields = "PatrolType,PatrolDept,PatrolPerson,District,to_char(PatrolDate,'yyyy-MM-dd hh24:mi') as PatrolDate,PROBLEMNUM,createuserid";
            pagination.p_tablename = "HRS_EVERYDAYPATROL";
            pagination.conditionJson = "1=1";
            var watch = CommonHelper.TimerStart();
            var data = everydaypatrolbll.GetPageList(pagination, queryJson);

            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "消防日常巡查";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "消防日常巡查.xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();
            listColumnEntity.Add(new ColumnEntity() { Column = "patroltype", ExcelColumn = "巡查类型", Alignment = "center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "patroldept", ExcelColumn = "巡查部门", Alignment = "center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "patrolperson", ExcelColumn = "巡查人", Alignment = "center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "district", ExcelColumn = "巡查区域", Alignment = "center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "patroldate", ExcelColumn = "巡查时间", Alignment = "center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "problemnum", ExcelColumn = "存在问题数量", Alignment = "center" });

            excelconfig.ColumnEntity = listColumnEntity;
            //调用导出方法
            //ExcelHelper.ExcelDownload(data, excelconfig);
            //调用导出方法
            ExcelHelper.ExportByAspose(data, excelconfig.FileName, listColumnEntity);
            return Success("导出成功。");
        }
        /// <summary>
        /// 导出日常巡查(详细信息)
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "导出日常巡查记录")]
        public ActionResult Export2(string queryJson)
        {
            var data = everydaypatroldetailbll.GetList(queryJson).ToList();
            var affirmData = affirmrecordbll.GetList(queryJson).ToList();//确认记录

            var userInfo = OperatorProvider.Provider.Current();  //获取当前用户
            string fileName = "消防巡查记录_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
            string strDocPath = Server.MapPath("~/Resource/ExcelTemplate/日常巡查记录模板.doc");
            
            DataSet ds = new DataSet();
            DataTable dtPro = new DataTable("project");
            dtPro.Columns.Add("PatrolDate");
            dtPro.Columns.Add("PatrolDept");
            dtPro.Columns.Add("PatrolPlace");
            dtPro.Columns.Add("PatrolPerson");
            dtPro.Columns.Add("Date1");
            dtPro.Columns.Add("Date2");
            dtPro.Columns.Add("Date3");
            dtPro.Columns.Add("PatrolType");
            dtPro.Columns.Add("ByDept");
            dtPro.Columns.Add("DutyUser");
            dtPro.Columns.Add("Signature1");
            dtPro.Columns.Add("Signature2");
            dtPro.Columns.Add("Signature3");

            DataTable dt = new DataTable("list");
            dt.Columns.Add("no");
            dt.Columns.Add("PatrolContent");
            dt.Columns.Add("Result");
            dt.Columns.Add("Problem");
            dt.Columns.Add("Dispose");

            HttpResponse resp = System.Web.HttpContext.Current.Response;

            DataRow row = dtPro.NewRow();

            string pic = Server.MapPath("~/content/Images/no_1.png");//默认图片
            string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
            if (queryJson != null && queryJson != "")
            {
                var queryParam = queryJson.ToJObject();
                if (!queryParam["PatrolDate"].IsEmpty())
                {
                    row["PatrolDate"] = Convert.ToDateTime(queryParam["PatrolDate"]).ToString("yyyy-MM-dd");
                    row["Date1"] = Convert.ToDateTime(queryParam["PatrolDate"]).ToString("yyyy-MM-dd");
                }
                if (!queryParam["PatrolPerson"].IsEmpty())
                {
                    row["PatrolPerson"] = queryParam["PatrolPerson"].ToString();
                }
                if (!queryParam["PatrolPlace"].IsEmpty())
                {
                    row["PatrolPlace"] = queryParam["PatrolPlace"].ToString();
                }
                if (!queryParam["PatrolDept"].IsEmpty())
                {
                    row["PatrolDept"] = queryParam["PatrolDept"].ToString();
                }
                if (!queryParam["PatrolType"].IsEmpty())
                {
                    if (queryParam["PatrolType"].ToString() == "月检") {
                        strDocPath = Server.MapPath("~/Resource/ExcelTemplate/消防巡查记录模板.doc");
                    }
                    row["PatrolType"] = queryParam["PatrolType"].ToString();
                }
                if (!queryParam["DutyUser"].IsEmpty())
                {
                    row["DutyUser"] = queryParam["DutyUser"].ToString();
                }
                if (!queryParam["ByDept"].IsEmpty())
                {
                    row["ByDept"] = queryParam["ByDept"].ToString();
                }
                if (!queryParam["Signature"].IsEmpty())
                {
                    var filepath = "";
                    if ((queryParam["Signature"].ToString()).IndexOf("http") > -1)
                    {
                        filepath = (Server.MapPath("~/") + queryParam["Signature"].ToString().Replace(webUrl, "").ToString()).Replace("/", @"\").ToString();
                    }
                    else
                    {
                        filepath = queryParam["Signature"] == null ? "" : (Server.MapPath("~/") + queryParam["Signature"].ToString().Replace("../../", "").ToString()).Replace("/", @"\").ToString();

                    }//string filepath = queryParam["Signature"].ToString();
                    if (System.IO.File.Exists(filepath))
                        row["Signature1"] = filepath;
                    else
                        row["Signature1"] = pic;
                }
            }
            if (affirmData != null && affirmData.Count > 0)
            {
                if (affirmData.Count == 1)
                {
                    var filepath = "";
                    if ((affirmData[0].Signature.ToString()).IndexOf("http") > -1)
                    {
                        filepath = (Server.MapPath("~/") + affirmData[0].Signature.ToString().Replace(webUrl, "").ToString()).Replace("/", @"\").ToString();
                    }
                    else
                    {
                        filepath = affirmData[0].Signature == null ? "" : (Server.MapPath("~/") + affirmData[0].Signature.ToString().Replace("../../", "").ToString()).Replace(@"\/", "\\").ToString();

                    }
                    if (System.IO.File.Exists(filepath))
                        row["Signature2"] = filepath;
                    else
                        row["Signature2"] = pic;
                    
                    row["Date2"] = Convert.ToDateTime(affirmData[0].AffirmDate).ToString("yyyy-MM-dd");
                }
                if (affirmData.Count == 2)
                {
                    var filepath = "";
                    if ((affirmData[0].Signature.ToString()).IndexOf("http") > -1)
                    {
                        filepath = (Server.MapPath("~/") + affirmData[0].Signature.ToString().Replace(webUrl, "").ToString()).Replace("/", @"\").ToString();
                    }
                    else
                    {
                        filepath = affirmData[0].Signature == null ? "" : (Server.MapPath("~/") + affirmData[0].Signature.ToString().Replace("../../", "").ToString()).Replace(@"\/", "\\").ToString();

                    }
                    if (System.IO.File.Exists(filepath))
                        row["Signature2"] = filepath;
                    else
                        row["Signature2"] = pic;

                    row["Date2"] = Convert.ToDateTime(affirmData[0].AffirmDate).ToString("yyyy-MM-dd");

                    var filepath1 = "";
                    if ((affirmData[1].Signature.ToString()).IndexOf("http") > -1)
                    {
                        filepath1 = (Server.MapPath("~/") + affirmData[1].Signature.ToString().Replace(webUrl, "").ToString()).Replace("/", @"\").ToString();
                    }
                    else
                    {
                        filepath1 = affirmData[1].Signature == null ? "" : (Server.MapPath("~/") + affirmData[1].Signature.ToString().Replace("../../", "").ToString()).Replace(@"\/", "\\").ToString();

                    }
                    if (System.IO.File.Exists(filepath1))
                        row["Signature3"] = filepath1;
                    else
                        row["Signature3"] = pic;
                    row["Date3"] = Convert.ToDateTime(affirmData[1].AffirmDate).ToString("yyyy-MM-dd");
                }
            }
            dtPro.Rows.Add(row);

            if (data.Count() > 0)
            {
                for (int i = 0; i < data.Count(); i++)
                {
                    DataRow dtrow = dt.NewRow();
                    dtrow["no"] = (i + 1);
                    dtrow["PatrolContent"] = data[i].PatrolContent;
                    if (data[i].Result == 0)
                    {
                        dtrow["Result"] = data[i].ResultTrue;
                    }
                    else if (data[i].Result == 1)
                    {
                        dtrow["Result"] = data[i].ResultFalse;
                    }
                    else {
                        dtrow["Result"] = "";
                    }
                    dtrow["Problem"] = data[i].Problem;
                    dtrow["Dispose"] = data[i].Dispose;
                    dt.Rows.Add(dtrow);
                }
            }
            ds.Tables.Add(dt);

            ds.Tables.Add(dtPro);

            Aspose.Words.Document doc = new Aspose.Words.Document(strDocPath);
            doc.MailMerge.Execute(dtPro);
            doc.MailMerge.ExecuteWithRegions(dt);
            doc.MailMerge.DeleteFields();
            doc.Save(resp, Server.UrlEncode(fileName), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
            return Success("导出成功!");
        }
        #endregion
    }
}
