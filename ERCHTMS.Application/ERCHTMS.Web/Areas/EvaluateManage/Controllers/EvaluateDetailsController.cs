using ERCHTMS.Entity.EvaluateManage;
using ERCHTMS.Busines.EvaluateManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using System.Web;
using System;
using System.Data;
using BSFramework.Util.Offices;
using ERCHTMS.Busines.StandardSystem;
using ERCHTMS.Entity.StandardSystem;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Busines.PublicInfoManage;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using ERCHTMS.Busines.BaseManage;
using System.Linq;

namespace ERCHTMS.Web.Areas.EvaluateManage.Controllers
{
    /// <summary>
    /// 描 述：合规性评价明细
    /// </summary>
    public class EvaluateDetailsController : MvcControllerBase
    {
        private EvaluateDetailsBLL evaluatedetailsbll = new EvaluateDetailsBLL();
        private StcategoryBLL StcategoryBLL = new StcategoryBLL();
        private readonly UserBLL userBLL = new UserBLL();
        private readonly OrganizeBLL orgBLL = new OrganizeBLL();
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
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var data = evaluatedetailsbll.GetPageList(pagination, queryJson);
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
            var data = evaluatedetailsbll.GetList(queryJson);
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
            var data = evaluatedetailsbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取大类
        /// </summary>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetCategoryListJson()
        {
            var data = StcategoryBLL.GetCategoryList();
            return Content(data.ToJson());
        }
        /// <summary>
        /// 获取小类
        /// </summary>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetRankListJson(string Category)
        {
            var data = StcategoryBLL.GetRankList(Category);
            return Content(data.ToJson());
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
            evaluatedetailsbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, EvaluateDetailsEntity entity)
        {
            evaluatedetailsbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion

        #region 导入数据
        /// <summary>
        /// 导入合规性评价
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string ImportEvaluateDetails(string keyValue,string EvaluatePlanId)
        {
            string orgId = OperatorProvider.Provider.Current().OrganizeId;//所属公司          
            int error = 0;
            string message = "请选择格式正确的文件再导入!";
            string falseMessage = "";
            int count = HttpContext.Request.Files.Count;
            string orgid = OperatorProvider.Provider.Current().OrganizeId;
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
                if (dt.Rows.Count < 1)
                {
                    falseMessage += string.Format(@"导入失败,请填写数据！</br>", order);
                }
                else
                {
                    for (int i = 2; i < dt.Rows.Count; i++)
                    {
                        EvaluateDetailsEntity item = new EvaluateDetailsEntity();
                        order = i + 1;
                        item.MainId = keyValue;
                        item.EvaluatePlanId = EvaluatePlanId;
                        #region 大类
                        string categoryname = dt.Rows[i][1].ToString();
                        if (!string.IsNullOrEmpty(categoryname))
                        {
                            item.CategoryName = categoryname;
                            JObject queryJson = new JObject();
                            queryJson.Add(new JProperty("name", categoryname));
                            var data = StcategoryBLL.GetQueryEntity(queryJson.ToString());
                            if (data != null)
                                item.Category = data.ID;
                            else
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,大类在系统中不存在！</br>", order);
                                error++;
                                continue;
                            }
                        }
                        //else
                        //{
                        //    falseMessage += string.Format(@"第{0}行导入失败,类别不能为空！</br>", order);
                        //    error++;
                        //    continue;
                        //}
                        #endregion

                        #region 小类
                        string rankname = dt.Rows[i][2].ToString();
                        if (!string.IsNullOrEmpty(rankname))
                        {
                            item.RankName = rankname;
                            JObject queryJson = new JObject();
                            queryJson.Add(new JProperty("name", rankname));
                            queryJson.Add(new JProperty("parentid", item.Category));
                            var data = StcategoryBLL.GetQueryEntity(queryJson.ToString());
                            if (data != null)
                                item.Rank = data.ID;
                            else
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,小类在系统中不存在或者不属于大类！</br>", order);
                                error++;
                                continue;
                            }
                        }
                        //else
                        //{
                        //    falseMessage += string.Format(@"第{0}行导入失败,类别不能为空！</br>", order);
                        //    error++;
                        //    continue;
                        //}
                        #endregion

                        #region 文件名称
                        string filename = dt.Rows[i][3].ToString();
                        if (!string.IsNullOrEmpty(filename))
                        {
                            item.FileName = filename;
                        }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,文件编号及名称不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region 颁布部门
                        string dutydept = dt.Rows[i][4].ToString();
                        if (!string.IsNullOrEmpty(dutydept))
                        {
                            item.DutyDept = dutydept;
                        }
                        #endregion

                        #region 实施日期
                        string putdate = dt.Rows[i][5].ToString();
                        if (!string.IsNullOrEmpty(putdate))
                        {
                            try
                            {
                                item.PutDate = DateTime.Parse(DateTime.Parse(putdate).ToString("yyyy-MM-dd"));
                            }
                            catch
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,实施日期不对！(正确示例：2019-01-01)</br>", order);
                                error++;
                                continue;
                            }
                        }
                        #endregion

                        #region 纳入企业标准的名称
                        string normname = dt.Rows[i][6].ToString();
                        if (!string.IsNullOrEmpty(normname))
                        {
                            item.NormName = normname;
                        }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,纳入企业标准的名称不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region 适用条款
                        string clause = dt.Rows[i][7].ToString();
                        if (!string.IsNullOrEmpty(clause))
                        {
                            item.Clause = clause;
                        }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,适用条款不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region 适用范围
                        string applyscope = dt.Rows[i][8].ToString();
                        if (!string.IsNullOrEmpty(applyscope))
                        {
                            item.ApplyScope = applyscope;
                        }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,适用范围不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region 符合性
                        string isconform = dt.Rows[i][9].ToString();
                        if (!string.IsNullOrEmpty(isconform))
                        {
                            if (isconform == "符合") item.IsConform = 0;
                            else if (isconform == "不符合") item.IsConform = 1;
                            else
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,符合性不存在！</br>", order);
                                error++;
                                continue;
                            }
                        }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,符合性不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region 现状符合性
                        string describe = dt.Rows[i][10].ToString();
                        if (!string.IsNullOrEmpty(describe))
                        {
                            item.Describe = describe;
                        }
                        #endregion

                        

                        #region 整改意见
                        string opinion = dt.Rows[i][11].ToString();
                        if (!string.IsNullOrEmpty(opinion))
                        {
                            if (item.IsConform == 1)
                            {
                                item.Opinion = opinion;
                            }
                            else
                            {
                                item.Opinion = null;
                            }
                        }
                        else
                        {
                            if (item.IsConform == 1)
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,整改意见不能为空！</br>", order);
                                error++;
                                continue;
                            }
                        }
                        #endregion

                        #region 整改计划完成时间
                        string FinishTime = dt.Rows[i][12].ToString();
                        DateTime tempFinishTime;
                        if (!string.IsNullOrEmpty(FinishTime))
                        {
                            if (item.IsConform == 1)
                            {
                                try
                                {
                                    item.FinishTime = DateTime.Parse(DateTime.Parse(FinishTime).ToString("yyyy-MM-dd"));
                                }
                                catch
                                {
                                    falseMessage += string.Format(@"第{0}行导入失败,整改截止时间不对！(正确示例：2019-01-01)</br>", order);
                                    error++;
                                    continue;
                                }
                                //if (DateTime.TryParse(FinishTime, out tempFinishTime))
                                //    item.FinishTime = tempFinishTime;
                                //else
                                //{
                                //    falseMessage += string.Format(@"第{0}行导入失败,整改计划完成时间不对！</br>", order);
                                //    error++;
                                //    continue;
                                //}
                            }
                            else
                            {
                                item.FinishTime = null;
                            }
                        }
                        else
                        {
                            if (item.IsConform == 1)
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,整改计划完成时间不能为空！</br>", order);
                                error++;
                                continue;
                            }
                        }
                        #endregion

                        //评价人
                        //string evaluateperson = dt.Rows[i][13].ToString();
                        //if (!string.IsNullOrEmpty(evaluateperson))
                        //{
                        //    item.EvaluatePerson = evaluateperson;
                        //}
                        #region 评价人
                        string evaluateperson = dt.Rows[i][13].ToString();
                        if (!string.IsNullOrEmpty(evaluateperson))
                        {
                            var userEntity = userBLL.GetListForCon(x => x.RealName == evaluateperson && x.OrganizeId == orgid).FirstOrDefault();
                            if (userEntity != null)
                            {
                                item.EvaluatePersonId = userEntity.UserId;
                                item.EvaluatePerson = evaluateperson;
                            }
                            else
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,评价人不存在！</br>", order);
                                error++;
                                continue;
                            }
                        }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,评价人不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion
                        try
                        {
                            evaluatedetailsbll.SaveForm("", item);
                        }
                        catch
                        {
                            error++;
                        }

                    }
                }
                count = dt.Rows.Count;
                message = "共有" + (count-2) + "条记录,成功导入" + ((count - 2) - error) + "条，失败" + error + "条";
                message += "</br>" + falseMessage;
            }

            return message;
        }
        #endregion
        /// <summary>
        /// 复制附件
        /// </summary>
        /// <param name="postData"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostFile(string postData, string keyValue) {
            FileInfoEntity fileInfoEntity = new FileInfoEntity();
            FileInfoBLL fileInfoBLL = new FileInfoBLL();

            fileInfoBLL.DeleteFileByRecId(keyValue);//删除原有附件

            var dt = JsonConvert.DeserializeObject<List<FileInfoEntity>>(postData);
            List<FileInfoEntity> projects = dt;
            string dir = string.Format("~/Resource/{0}/{1}", "ht/images", DateTime.Now.ToString("yyyyMMdd"));
            if (Directory.Exists(Server.MapPath(dir)) == false)//如果不存在就创建文件夹
            {
                Directory.CreateDirectory(Server.MapPath(dir));
            }
            foreach (FileInfoEntity item in dt) {
                var filepath = Server.MapPath(item.FilePath);
                if (System.IO.File.Exists(filepath))
                {
                    string sufx = System.IO.Path.GetExtension(filepath);
                    string newFileName = Guid.NewGuid().ToString() + sufx;
                    string newFilePath = dir + "/" + newFileName;
                    System.IO.File.Copy(filepath, Server.MapPath(newFilePath));
                    item.FilePath = newFilePath;
                }
                item.RecId = keyValue;
                item.FileId = Guid.NewGuid().ToString();
                fileInfoBLL.SaveForm("", item);
            }
            fileInfoBLL.SaveForm("", fileInfoEntity);
            return Success("操作成功。");
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <returns></returns>
        //[HttpPost]
        //[AjaxOnly]
        //[HandlerAuthorize(PermissionMode.Ignore)]
        //[HandlerMonitor(0, "导出消防设施excel")]
        public ActionResult ExportExcel(string condition, string queryJson)
        {
            Pagination pagination = new Pagination
            {
                page = 1,
                rows = 100000000,
                p_kid = "Id",
                p_fields = @"CategoryName,RankName,FileName,DutyDept,
to_char(PutDate,'yyyy-MM-dd') as PutDate,NormName,Clause,ApplyScope,
case when IsConform = '0' then '符合' when IsConform = '1' then '不符合' else '' end as IsConform,
Describe,Opinion,to_char(FinishTime,'yyyy-MM-dd hh24:mi') as FinishTime,EvaluatePerson",
                p_tablename = "HRS_EVALUATEDETAILS",
                conditionJson = "1=1",
                sidx = "IsConform desc,CreateDate",
                sord = "desc"
            };
            DataTable data = evaluatedetailsbll.GetPageList(pagination, queryJson);


            //设置导出格式
            //ExcelConfig excelconfig = new ExcelConfig
            //{
            //    Title = "合规性评价",
            //    TitleFont = "宋体",
            //    TitleHeight = 30,
            //    TitlePoint = 25,
            //    FileName = "合规性评价" + ".xls",
            //    IsAllSizeColumn = true,
            //    //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            //    ColumnEntity = new List<ColumnEntity>(){
            //    new ColumnEntity() {Column = "categoryname", ExcelColumn = "大类", Alignment = "center"},
            //    new ColumnEntity() {Column = "rankname", ExcelColumn = "小类", Alignment = "center"},
            //    new ColumnEntity() {Column = "filename", ExcelColumn = "文件编号及名称", Alignment = "center"},
            //    new ColumnEntity() {Column = "dutydept", ExcelColumn = "颁布部门", Alignment = "center"},
            //    new ColumnEntity() {Column = "putdate", ExcelColumn = "实施日期", Alignment = "center"},
            //    new ColumnEntity() {Column = "normname", ExcelColumn = "纳入企业标准的名称", Alignment = "center"},
            //    new ColumnEntity() {Column = "clause", ExcelColumn = "适用条款", Alignment = "center"},
            //    new ColumnEntity() {Column = "applyscope", ExcelColumn = "适用范围", Alignment = "center"},
            //    new ColumnEntity() {Column = "isconform", ExcelColumn = "符合性",  Alignment = "center"},
            //    new ColumnEntity() {Column = "describe", ExcelColumn = "现状符合性描述", Alignment = "center"},
            //    new ColumnEntity() {Column = "opinion", ExcelColumn = "整改意见", Alignment = "center"},
            //    new ColumnEntity() {Column = "finishtime", ExcelColumn = "整改截止时间", Alignment = "center"},
            //    new ColumnEntity() {Column = "evaluateperson", ExcelColumn = "评价人", Alignment = "center"}
            //    }
            //};

            ////调用导出方法
            ////ExcelHelper.ExportByAspose(data, excelconfig.FileName, excelconfig.ColumnEntity);
            //ExcelHelper.ExcelDownload(data, excelconfig);
            //return Success("导出成功。");

            for (int i = 0; i < data.Rows.Count; i++)
            {
                data.Rows[i][0] = i + 1;
                //data.Rows[i]["TimeType"] = data.Rows[i]["TimeNum"].ToString() + data.Rows[i]["TimeType"].ToString();
                
            }

            string FileUrl = @"\Resource\ExcelTemplate\合规性评价导出.xls";
            AsposeExcelHelper.ExecuteResult(data, FileUrl, "合规性评价", "合规性评价");

            return Success("导出成功。");
        }
    }
}
