using System;
using System.Linq;
using System.Data;
using System.Web.Mvc;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Code;
using ERCHTMS.Entity.QuestionManage;
using ERCHTMS.Busines.QuestionManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.HiddenTroubleManage;
using System.Web;
using Aspose.Cells;
using System.Drawing;
using Aspose.Words;
using ERCHTMS.Entity.BaseManage;
using System.Collections.Generic;
using ERCHTMS.Busines.SaftyCheck;
using ERCHTMS.Entity.SaftyCheck;
using ERCHTMS.Entity.HiddenTroubleManage.ViewModel;
using System.IO;
using ERCHTMS.Entity.PublicInfoManage;

namespace ERCHTMS.Web.Areas.QuestionManage.Controllers
{
    /// <summary>
    /// 描 述：问题基本信息表
    /// </summary>
    public class QuestionInfoController : MvcControllerBase
    {
        private QuestionInfoBLL questioninfobll = new QuestionInfoBLL();
        private QuestionReformBLL questionreformbll = new QuestionReformBLL();
        private QuestionVerifyBLL questionverifybll = new QuestionVerifyBLL();
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL(); //流程业务对象
        private UserBLL userbll = new UserBLL(); //用户操作对象
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private FileInfoBLL fileinfobll = new FileInfoBLL();
        private ModuleListColumnAuthBLL modulelistcolumnauthbll = new ModuleListColumnAuthBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private SaftyCheckDataRecordBLL saftycheckdatarecordbll = new SaftyCheckDataRecordBLL();
        private WfControlBLL wfcontrolbll = new WfControlBLL();//自动化流程服务

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
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SdIndex()
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
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DoneForm()
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }
        #endregion

        #region 获取数据

        #region 页面组件初始化
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetInitDataJson()
        {
            Operator CreateUser = OperatorProvider.Provider.Current();
            //检查类型
            string itemCode = "'SaftyCheckType','QuestionFlowState'";
            //集合
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode(itemCode);

            bool isHavaWorkFlow = htworkflowbll.IsHaveCurWorkFlow("厂级问题流程");
            //返回值
            var josnData = new
            {
                User = CreateUser,  //用户对象
                CheckType = itemlist.Where(p => p.EnCode == "SaftyCheckType"),  //检查类型  
                FlowState = itemlist.Where(p => p.EnCode == "QuestionFlowState"),  //问题流程状态
                IsHavaWorkFlow = isHavaWorkFlow
            };
            return Content(josnData.ToJson());
        }
        #endregion

        #region 初始化查询条件
        /// <summary>
        /// 初始化查询条件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetQueryConditionJson()
        {
            Operator CreateUser = OperatorProvider.Provider.Current();

            //流程状态、数据范围、台账类型
            string itemCode = "'QuestionFlowState','QuestionDataScope','HidStandingType'";
            //集合
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode(itemCode);

            bool isHavaWorkFlow = htworkflowbll.IsHaveCurWorkFlow("厂级问题流程");
            //返回值
            var josnData = new
            {
                FlowState = itemlist.Where(p => p.EnCode == "QuestionFlowState"),  //问题流程状态
                DataScope = itemlist.Where(p => p.EnCode == "QuestionDataScope"),//数据范围 
                HidStandingType = itemlist.Where(p => p.EnCode == "HidStandingType"),  //台账类型 
                IsHavaWorkFlow = isHavaWorkFlow
            };

            return Content(josnData.ToJson());
        }
        #endregion

        #region 获取问题列表数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {

            var watch = CommonHelper.TimerStart();
            Operator opertator = new OperatorProvider().Current();
            queryJson = queryJson.Insert(1, "\"userId\":\"" + opertator.UserId + "\","); //添加当前用户
            var data = questioninfobll.GetQuestionBaseInfo(pagination, queryJson);
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


        #region 获取问题列表数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetFindListJson(string queryJson)
        {
            Pagination pagination = new Pagination();
            pagination.rows = Int32.MaxValue;
            pagination.page = 1;
            pagination.sidx = "createdate";
            pagination.sord = "desc";
            var watch = CommonHelper.TimerStart();
            var data = questioninfobll.GetQuestionBaseInfo(pagination, queryJson);
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


        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var baseInfo = questioninfobll.GetEntity(keyValue);  //问题基本信息

            var reformInfo = questionreformbll.GetEntityByBid(baseInfo.ID); // 整改信息

            var verifyInfo = questionverifybll.GetEntityByBid(baseInfo.ID); //验证信息

            var userInfo = OperatorProvider.Provider.Current();  //获取当前用户

            var data = new { baseInfo = baseInfo, reformInfo = reformInfo, verifyInfo = verifyInfo, userInfo = userbll.GetUserInfoEntity(userInfo.UserId) };

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
            questioninfobll.RemoveForm(keyValue);
            return Success("删除成功。");
        }

        #endregion

        #region  保存表单（新增、修改）
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, QuestionInfoEntity entity, QuestionReformEntity rEntity, QuestionVerifyEntity aEntity)
        {
            CommonSaveForm(keyValue, entity, rEntity, aEntity);
            return Success("操作成功!");
        }
        #endregion

        #region 公用方法，保存数据
        /// <summary>
        /// 公用方法，保存数据
        /// </summary>
        /// <param name="keyValue">主键ID</param>
        /// <param name="entity">问题基本信息</param>
        /// <param name="rEntity">整改信息</param>
        /// <param name="aEntity">验收信息</param>
        public void CommonSaveForm(string keyValue, QuestionInfoEntity entity, QuestionReformEntity rEntity, QuestionVerifyEntity aEntity)
        {
            try
            {
                //提交通过
                string userId = OperatorProvider.Provider.Current().UserId;
                var userInfo = OperatorProvider.Provider.Current();  //获取当前用户
                //保存问题基本信息
                //通过违章编码判断是否存在重复
                if (string.IsNullOrEmpty(keyValue))
                {

                    entity.QUESTIONNUMBER = questioninfobll.GenerateCode("bis_questioninfo", "questionnumber", 4);

                    entity.BELONGDEPTID = userInfo.OrganizeId;

                    entity.BELONGDEPTNAME = userInfo.OrganizeName;
                }
                questioninfobll.SaveForm(keyValue, entity);

                //创建流程实例
                if (string.IsNullOrEmpty(keyValue))
                {
                    bool isSucess = htworkflowbll.CreateWorkFlowObj("09", entity.ID, userId);
                    if (isSucess)
                    {
                        htworkflowbll.UpdateFlowStateByObjectId("bis_questioninfo", "flowstate", entity.ID);  //更新业务流程状态
                    }
                }

                /********整改信息************/
                string REFORMID = Request.Form["REFORMID"].ToString();
                rEntity.QUESTIONID = entity.ID;
                //新增状态下添加
                if (!string.IsNullOrEmpty(REFORMID))
                {
                    var tempEntity = questionreformbll.GetEntity(REFORMID);
                    rEntity.AUTOID = tempEntity.AUTOID;
                    rEntity.REFORMSTATUS = string.Empty;
                }
                questionreformbll.SaveForm(REFORMID, rEntity);


                bool isHavaWorkFlow = htworkflowbll.IsHaveCurWorkFlow("厂级问题流程");
                //当前没有配置流程，则新加验证信息
                if (!isHavaWorkFlow)
                {
                    /********验证信息************/
                    string VERIFYID = Request.Form["VERIFYID"].ToString();
                    aEntity.QUESTIONID = entity.ID;
                    if (!string.IsNullOrEmpty(VERIFYID))
                    {
                        var tempEntity = questionverifybll.GetEntity(VERIFYID);
                        aEntity.AUTOID = tempEntity.AUTOID;
                    }
                    questionverifybll.SaveForm(VERIFYID, aEntity);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 提交流程（同时新增、修改问题信息）
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, QuestionInfoEntity entity, QuestionReformEntity rEntity, QuestionVerifyEntity aEntity)
        {
            //判断重复编号过程
            if (!string.IsNullOrEmpty(entity.QUESTIONNUMBER))
            {
                var curHtBaseInfor = questioninfobll.GetListByNumber(entity.QUESTIONNUMBER).FirstOrDefault();

                if (null != curHtBaseInfor)
                {
                    if (curHtBaseInfor.ID != keyValue && string.IsNullOrEmpty(keyValue))
                    {
                        return Error("问题编号重复,请重新新增!");
                    }
                }
            }
            try
            {
                //创建流程，保存对应信息
                CommonSaveForm(keyValue, entity, rEntity, aEntity);
                //创建完流程实例后
                if (string.IsNullOrEmpty(keyValue))
                {
                    keyValue = entity.ID;
                }
                //此处需要判断当前人是否为安全管理员
                string wfFlag = string.Empty;
                //当前用户
                Operator curUser = OperatorProvider.Provider.Current();

                //参与人员
                string participant = string.Empty;

                wfFlag = "1"; //到整改

                participant = rEntity.REFORMPEOPLE;  //整改人

                if (!string.IsNullOrEmpty(participant))
                {
                    int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, curUser.UserId);

                    if (count > 0)
                    {
                        htworkflowbll.UpdateFlowStateByObjectId("bis_questioninfo", "flowstate", keyValue);  //更新业务流程状态
                    }
                    return Success("操作成功!");
                }
                else
                {
                    return Success("请选择整改责任人!");
                }
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion

        #region 一次性提交多个关联检查人的登记问题信息
        [HttpPost]
        [AjaxOnly]
        public ActionResult CheckQuestionForm(string checkid)
        {
            Operator curUser = OperatorProvider.Provider.Current();

            var dtHid = questioninfobll.GeQuestiontOfCheckList(checkid, curUser.UserId, "问题登记");

            string keyValue = string.Empty;

            string reformpeopleid = string.Empty; //整改人

            foreach (DataRow row in dtHid.Rows)
            {
                keyValue = row["id"].ToString();

                reformpeopleid = row["reformpeople"].ToString();

                string createuserid = row["createuserid"].ToString();

                //此处需要判断当前人是否为安全管理员
                string wfFlag = string.Empty;
                //参与人员
                string participant = string.Empty;

                wfFlag = "1"; //到整改

                participant = reformpeopleid;  //整改人

                if (!string.IsNullOrEmpty(participant))
                {
                    int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, createuserid);

                    if (count > 0)
                    {
                        htworkflowbll.UpdateFlowStateByObjectId("bis_questioninfo", "flowstate", keyValue);  //更新业务流程状态
                    }
                }
            }
            return Success("操作成功!");
        }
        #endregion

        #region 导出问题基本信息
        /// <summary>
        /// 导出问题基本信息
        /// </summary>
        /// <param name="queryJson"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public ActionResult ExportExcel(string queryJson, string fileName, string currentModuleId)
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            queryJson = queryJson.Insert(1, "\"userId\":\"" + curUser.UserId + "\","); //添加当前用户
            string userId = curUser.UserId;
            string p_fields = string.Empty;
            string p_fieldsName = string.Empty;
            //系统默认的列表设置
            var defaultdata = modulelistcolumnauthbll.GetEntity(currentModuleId, "", 0);
            if (null != defaultdata)
            {
                p_fields = defaultdata.DEFAULTCOLUMNFIELDS;
                p_fieldsName = defaultdata.DEFAULTCOLUMNNAME;
            }
            //当前用户的列表设置
            var data = modulelistcolumnauthbll.GetEntity(currentModuleId, curUser.UserId, 1);
            //为空，自动读取系统默认
            if (null != data)
            {
                p_fields = data.DEFAULTCOLUMNFIELDS;
                p_fieldsName = data.DEFAULTCOLUMNNAME;
            }
            p_fields = "flowstate," + p_fields + ",participantname";
            p_fieldsName = "流程状态," + p_fieldsName + ",流程处理人";
            pagination.p_fields = p_fields;
            //取出数据源
            DataTable exportTable = questioninfobll.GetQuestionBaseInfo(pagination, queryJson);
            exportTable.Columns.Remove("id");
            exportTable.Columns["r"].SetOrdinal(0);

            HttpResponse resp = System.Web.HttpContext.Current.Response;

            // 详细列表内容
            string fielname = fileName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
            wb.Open(Server.MapPath("~/Resource/ExcelTemplate/tmp.xls"));
            Aspose.Cells.Worksheet sheet = wb.Worksheets[0] as Aspose.Cells.Worksheet;

            Aspose.Cells.Cell cell = sheet.Cells[0, 0];
            cell.PutValue("问题基本信息"); //标题
            cell.Style.Pattern = BackgroundType.Solid;
            cell.Style.Font.Size = 14;
            cell.Style.Font.Color = Color.Black;

            DataTable dt = new DataTable();
            dt.Columns.Add("r");
            //再设置相关行列
            if (!string.IsNullOrEmpty(p_fields))
            {
                string[] p_fieldsArray = p_fields.Split(',');
                for (int i = 0; i < p_fieldsArray.Length; i++)
                {
                    dt.Columns.Add(p_fieldsArray[i].ToString(), typeof(string));//列头
                }

                foreach (DataRow row in exportTable.Rows)
                {
                    DataRow newrow = dt.NewRow();
                    newrow["r"] = row["r"].ToString();
                    for (int i = 0; i < p_fieldsArray.Length; i++)
                    {
                        string curColName = p_fieldsArray[i].ToString();

                        if (curColName != "questionfilepath" && curColName != "reformfilepath")
                        {
                            newrow[curColName] = row[curColName].ToString();
                        }
                        else
                        {
                            newrow[curColName] = "";
                        }
                    }
                    dt.Rows.Add(newrow);
                }
            }

            //再设置相关行列
            if (!string.IsNullOrEmpty(p_fieldsName))
            {
                //动态加列
                string[] p_filedsNameArray = p_fieldsName.Split(',');
                //序号列
                Aspose.Cells.Cell serialcell = sheet.Cells[1, 0];
                serialcell.PutValue("序号"); //填报单位

                for (int i = 0; i < p_filedsNameArray.Length; i++)
                {
                    Aspose.Cells.Cell curcell = sheet.Cells[1, i + 1];
                    curcell.PutValue(p_filedsNameArray[i].ToString()); //列头
                }
                //合并单元格
                Aspose.Cells.Cells cells = sheet.Cells;
                cells.Merge(0, 0, 1, p_filedsNameArray.Length + 1);
            }

            //先导入数据
            sheet.Cells.ImportDataTable(dt, false, 2, 0);

            //插入图片操作
            if (!string.IsNullOrEmpty(p_fieldsName))
            {
                int picnum = 0;
                foreach (DataRow row in exportTable.Rows)
                {
                    //违章图片
                    if (exportTable.Columns.Contains("questionfilepath"))
                    {
                        int colIndex = exportTable.Columns.IndexOf("questionfilepath");
                        string questionfilepath = row["questionfilepath"].ToString();
                        if (!string.IsNullOrEmpty(questionfilepath))
                        {
                            string imageUrl = System.Web.HttpContext.Current.Server.MapPath(questionfilepath);
                            if (System.IO.File.Exists(imageUrl))
                            {
                                sheet.Pictures.Add(2 + picnum, colIndex, 3 + picnum, colIndex + 1, imageUrl);
                            }
                        }
                        row["questionfilepath"] = "";
                    }
                    //整改图片
                    if (exportTable.Columns.Contains("reformfilepath"))
                    {
                        int colIndex = exportTable.Columns.IndexOf("reformfilepath");
                        string reformfilepath = row["reformfilepath"].ToString();
                        if (!string.IsNullOrEmpty(reformfilepath))
                        {
                            string imageUrl = System.Web.HttpContext.Current.Server.MapPath(reformfilepath);
                            if (System.IO.File.Exists(imageUrl))
                            {
                                sheet.Pictures.Add(2 + picnum, colIndex, 3 + picnum, colIndex + 1, imageUrl);
                            }
                        }
                        row["reformfilepath"] = "";
                    }
                    picnum++;
                }
            }
            wb.Save(Server.UrlEncode(fielname), Aspose.Cells.FileFormatType.Excel2003, Aspose.Cells.SaveType.OpenInBrowser, resp);

            return Success("导出成功!");
        }
        #endregion

        #region 导出问题详情基本信息
        /// <summary>
        /// 导出问题基本信息
        /// </summary>
        /// <param name="queryJson"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public ActionResult ExportDetailExcel(string keyValue)
        {
            try
            {
                var dataDt = questioninfobll.GetQuestionModel(keyValue);

                var verifyList = questionverifybll.GetHistoryList(keyValue); //验证信息

                var userInfo = OperatorProvider.Provider.Current();  //获取当前用户

                string fileName = "问题整改验证表_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";

                string strDocPath = Server.MapPath("~/Resource/ExcelTemplate/问题整改验证表.doc");

                Aspose.Words.Document doc = new Aspose.Words.Document(strDocPath);
                DocumentBuilder builder = new DocumentBuilder(doc);
                NodeCollection allTables = doc.GetChildNodes(NodeType.Table, true); //获取word中所有表格table
                Aspose.Words.Tables.Table table1 = allTables[1] as Aspose.Words.Tables.Table;//获取第一个table

                int verifycount = verifyList.Count();
                var rows0 = table1.Rows[7];
                var rows1 = table1.Rows[8];
                var rows2 = table1.Rows[9];
                int qcount = 3;
                for (int i = 0; i < verifycount; i++)
                {
                    if (i != 0)
                    {
                        var rowsQ0 = rows0.Clone(true);
                        table1.Rows.Insert(7 + qcount, rowsQ0);
                        var rowsQ1 = rows1.Clone(true);
                        table1.Rows.Insert(8 + qcount, rowsQ1);
                        var rowsQ2 = rows2.Clone(true);
                        table1.Rows.Insert(9 + qcount, rowsQ2);
                    }
                }
                int num = 0;
                foreach (QuestionVerifyEntity entity in verifyList)
                {
                    builder.MoveToCell(1, 7 + num, 0, 0);//移动到第7+num行的第1个格子;
                    if (!string.IsNullOrEmpty(entity.VERIFYDEPTNAME))
                    {
                        builder.Write(entity.VERIFYDEPTNAME + "意见:");
                    }
                    builder.MoveToCell(1, 8 + num, 0, 0);//移动到第8+num行的第1个格子;
                    if (!string.IsNullOrEmpty(entity.VERIFYOPINION))
                    {
                        builder.Write(entity.VERIFYOPINION);
                    }
                    builder.MoveToCell(1, 9 + num, 1, 0);//移动到第9+num行的第2个格子;
                    if (!string.IsNullOrEmpty(entity.VERIFYPEOPLENAME))
                    {
                        builder.Write(entity.VERIFYPEOPLENAME); //验证人
                    }
                    builder.MoveToCell(1, 9 + num, 3, 0);//移动到第9+num行的第4个格子;
                    if (null != entity.VERIFYDATE)
                    {
                        builder.Write(entity.VERIFYDATE.Value.ToString("yyyy-MM-dd")); //验证日期
                    }
                    num += 3;
                }


                DataTable dt = new DataTable();
                dt.Columns.Add("questionnumber");
                dt.Columns.Add("checkname");
                dt.Columns.Add("checkcontent");
                dt.Columns.Add("reformdeptname");
                dt.Columns.Add("checkdate");
                dt.Columns.Add("questiondescribe");
                dt.Columns.Add("reformmeasure");
                dt.Columns.Add("reformdescribe");
                dt.Columns.Add("reformreason");
                dt.Columns.Add("reformpeoplename");
                dt.Columns.Add("reformfinishdate");
                HttpResponse resp = System.Web.HttpContext.Current.Response;
                DataRow row = dt.NewRow();
                row["questionnumber"] = dataDt.Rows[0]["questionnumber"].ToString();
                row["checkname"] = dataDt.Rows[0]["checkname"].ToString();
                row["checkcontent"] = dataDt.Rows[0]["checkcontent"].ToString();
                string reformdeptname = string.Empty;
                if (!string.IsNullOrEmpty(dataDt.Rows[0]["reformdeptname"].ToString()))
                {
                    reformdeptname += dataDt.Rows[0]["reformdeptname"].ToString();
                }
                if (!string.IsNullOrEmpty(dataDt.Rows[0]["dutydeptname"].ToString()))
                {
                    reformdeptname += "," + dataDt.Rows[0]["dutydeptname"].ToString();
                }
                row["reformdeptname"] = reformdeptname;
                row["checkdate"] = !string.IsNullOrEmpty(dataDt.Rows[0]["checkdate"].ToString()) ? Convert.ToDateTime(dataDt.Rows[0]["checkdate"].ToString()).ToString("yyyy-MM-dd") : "";
                row["questiondescribe"] = dataDt.Rows[0]["questiondescribe"].ToString();
                row["reformmeasure"] = dataDt.Rows[0]["reformmeasure"].ToString();
                row["reformdescribe"] = dataDt.Rows[0]["reformdescribe"].ToString();
                row["reformreason"] = dataDt.Rows[0]["reformreason"].ToString();
                row["reformpeoplename"] = dataDt.Rows[0]["reformpeoplename"].ToString();
                row["reformfinishdate"] = !string.IsNullOrEmpty(dataDt.Rows[0]["reformfinishdate"].ToString()) ? Convert.ToDateTime(dataDt.Rows[0]["reformfinishdate"].ToString()).ToString("yyyy-MM-dd") : "";
                dt.Rows.Add(row);
                doc.MailMerge.Execute(dt);
                doc.MailMerge.DeleteFields();

                doc.Save(resp, Server.UrlEncode(fileName), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));

                return Success("导出成功!");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion

        #region 导入隐患信息
        /// <summary>
        /// 导入隐患信息
        /// </summary>
        /// <param name="checkid">安全检查id</param>
        /// <param name="repeatdata">重复数据</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportInfo(string repeatdata, string mode)
        {
            if (OperatorProvider.Provider.Current().IsSystem)
            {
                return "超级管理员无此操作权限";
            }
            var curUser = OperatorProvider.Provider.Current();
            string orgId = curUser.OrganizeId;//所属公司
            string message = "请选择格式正确的文件再导入!";
            string falseMessage = "";
            int count = HttpContext.Request.Files.Count;
            try
            {
                List<string> listIds = new List<string>();
                if (count > 0)
                {
                    if (HttpContext.Request.Files.Count > 2)
                    {
                        return "请按正确的方式导入文件,一次上传最多支持两份文件(即一份excel数据文件,一份图片压缩文件).";
                    }
                    HttpPostedFileBase file = HttpContext.Request.Files[0];
                    string hiddenDirectory = string.Empty;
                    Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();

                    #region 两份文件时
                    if (HttpContext.Request.Files.Count == 2)
                    {
                        HttpPostedFileBase file2 = HttpContext.Request.Files[1];
                        if (string.IsNullOrEmpty(file.FileName) || string.IsNullOrEmpty(file2.FileName))
                        {
                            return message;
                        }
                        Boolean isZip1 = file.FileName.ToLower().Substring(file.FileName.ToLower().IndexOf('.')).Contains("zip");//第一个文件是否为Zip格式
                        Boolean isZip2 = file2.FileName.ToLower().Substring(file2.FileName.ToLower().IndexOf('.')).Contains("zip");//第二个文件是否为Zip格式
                        if ((isZip1 || isZip2) == false || (isZip1 && isZip2) == true)
                        {
                            return message;
                        }
                        string fileName1 = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                        file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName1));
                        string fileName2 = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file2.FileName);
                        file2.SaveAs(Server.MapPath("~/Resource/temp/" + fileName2));
                        hiddenDirectory = Server.MapPath("~/Resource/temp/") + DateTime.Now.ToString("yyyyMMddhhmmssfff") + "\\"; //隐患/违章图片存放地址
                        if (!Directory.Exists(hiddenDirectory))
                        {
                            System.IO.Directory.CreateDirectory(hiddenDirectory); //创建目录
                        }
                        if (isZip1)
                        {
                            fileinfobll.UnZip(Server.MapPath("~/Resource/temp/" + fileName1), hiddenDirectory, "", true);
                            wb.Open(Server.MapPath("~/Resource/temp/" + fileName2));
                            if (string.IsNullOrEmpty(file2.FileName))
                            {
                                return message;
                            }
                            if (!(file2.FileName.ToLower().Substring(file2.FileName.ToLower().IndexOf('.')).Contains("xls") || file2.FileName.ToLower().Substring(file2.FileName.ToLower().IndexOf('.')).Contains("xlsx")))
                            {
                                return message;
                            }
                        }
                        else
                        {
                            fileinfobll.UnZip(Server.MapPath("~/Resource/temp/" + fileName2), hiddenDirectory, "", true);
                            wb.Open(Server.MapPath("~/Resource/temp/" + fileName1));
                            if (string.IsNullOrEmpty(file.FileName))
                            {
                                return message;
                            }
                            if (!(file.FileName.ToLower().Substring(file.FileName.ToLower().IndexOf('.')).Contains("xls") || file.FileName.ToLower().Substring(file.FileName.ToLower().IndexOf('.')).Contains("xlsx")))
                            {
                                return message;
                            }
                        }
                    }
                    #endregion
                    #region 一份文件时
                    else  //一份文件时
                    {
                        if (string.IsNullOrEmpty(file.FileName))
                        {
                            return message;
                        }
                        if (!(file.FileName.ToLower().Substring(file.FileName.ToLower().IndexOf('.')).Contains("xls") || file.FileName.ToLower().Substring(file.FileName.ToLower().IndexOf('.')).Contains("xlsx")))
                        {
                            return message;
                        }
                        string fileName1 = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                        file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName1));
                        wb.Open(Server.MapPath("~/Resource/temp/" + fileName1));
                    }
                    #endregion
                    Worksheet sheets = wb.Worksheets[0];
                    Aspose.Cells.Cells cells = sheets.Cells;
                    DataTable dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn + 1, true);
                    //记录错误信息
                    List<string> resultlist = new List<string>();
                    List<UserEntity> ulist = userbll.GetList().OrderBy(p => p.SortCode).ToList();
                    List<DepartmentEntity> dlist = departmentBLL.GetList().OrderBy(p => p.SortCode).ToList();
                    int total = 0;
                    int checkproject = 0;
                    SaftyCheckDataDetailBLL sdbll = new SaftyCheckDataDetailBLL();
                    SaftyCheckContentBLL scbll = new SaftyCheckContentBLL();
                    var checkrecordlist = saftycheckdatarecordbll.GetList("");
                    //检查类型集合
                    var checktypelist = dataitemdetailbll.GetDataItemListByItemCode("'SaftyCheckType'");

                    bool isHavaWorkFlow = htworkflowbll.IsHaveCurWorkFlow("厂级问题流程");

                    #region 问题部分
                    if (sheets.Name.Contains("问题") || dt.Columns.Contains("问题描述"))//问题信息导入
                    {
                        string checkid = string.Empty;


                        #region 对象装载
                        List<ImportQuestion> list = new List<ImportQuestion>();
                        //先获取到职务列表;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string resultmessage = "第" + (i + 1).ToString() + "行数据"; //显示错误

                            bool isadddobj = true;

                            //问题编码
                            string questionnumber = dt.Columns.Contains("问题编码") ? dt.Rows[i]["问题编码"].ToString().Trim() : string.Empty;
                            //问题描述
                            string questiondescribe = dt.Columns.Contains("问题描述") ? dt.Rows[i]["问题描述"].ToString().Trim() : string.Empty;
                            //问题地点
                            string questionaddress = dt.Columns.Contains("问题地点") ? dt.Rows[i]["问题地点"].ToString().Trim() : string.Empty;
                            //检查人员
                            string checkperson = dt.Columns.Contains("检查人员") ? dt.Rows[i]["检查人员"].ToString().Trim() : string.Empty;
                            //检查单位
                            string checkdept = dt.Columns.Contains("检查单位") ? dt.Rows[i]["检查单位"].ToString().Trim() : string.Empty;
                            //检查类型
                            string checktype = dt.Columns.Contains("检查类型") ? dt.Rows[i]["检查类型"].ToString().Trim() : string.Empty;
                            //检查日期
                            string checkdate = dt.Columns.Contains("检查日期") ? dt.Rows[i]["检查日期"].ToString().Trim() : string.Empty;
                            //检查名称
                            string checkname = dt.Columns.Contains("检查名称") ? dt.Rows[i]["检查名称"].ToString().Trim() : string.Empty;
                            //检查重点内容
                            string checkimpcontent = dt.Columns.Contains("检查重点内容") ? dt.Rows[i]["检查重点内容"].ToString().Trim() : string.Empty;
                            //整改责任单位
                            string reformdeptname = dt.Columns.Contains("整改责任单位") ? dt.Rows[i]["整改责任单位"].ToString().Trim() : string.Empty;
                            //整改责任人
                            string reformpeoplename = dt.Columns.Contains("整改责任人") ? dt.Rows[i]["整改责任人"].ToString().Trim() : string.Empty;
                            //整改责任人电话
                            string telephone = dt.Columns.Contains("整改责任人电话") ? dt.Rows[i]["整改责任人电话"].ToString().Trim() : string.Empty;
                            //联责单位
                            string dutydeptname = dt.Columns.Contains("联责单位") ? dt.Rows[i]["联责单位"].ToString().Trim() : string.Empty;
                            //计划完成日期
                            string reformplandate = dt.Columns.Contains("计划完成日期") ? dt.Rows[i]["计划完成日期"].ToString().Trim() : string.Empty;
                            //整改措施
                            string reformmeasure = dt.Columns.Contains("整改措施") ? dt.Rows[i]["整改措施"].ToString().Trim() : string.Empty;
                            //验证人
                            string verifypeoplename = dt.Columns.Contains("验证人") ? dt.Rows[i]["验证人"].ToString().Trim() : string.Empty;
                            //验证单位
                            string verifydeptname = dt.Columns.Contains("验证单位") ? dt.Rows[i]["验证单位"].ToString().Trim() : string.Empty;
                            //验证日期
                            string verifydate = dt.Columns.Contains("验证日期") ? dt.Rows[i]["验证日期"].ToString().Trim() : string.Empty;

                            string relevanceid = string.Empty;
                            try
                            {
                                #region 对象集合
                                ImportQuestion entity = new ImportQuestion();
                                //序号
                                entity.serialnumber = i + 1; //序号
                                //问题编码
                                if (!string.IsNullOrEmpty(questionnumber))
                                {
                                    entity.questionnumber = questionnumber;
                                }
                                if (!string.IsNullOrEmpty(questionnumber))
                                {
                                    if (questionnumber.Length == 13 || questionnumber.Length == 14)
                                    {
                                        //AQ2020第11期0001
                                        if (questionnumber.Substring(0, 2) == "AQ" && questionnumber.Substring(6, 1) == "第" && (questionnumber.Substring(8, 1) == "期" || questionnumber.Substring(9, 1) == "期"))
                                        {
                                            isadddobj = true;
                                        }
                                        else
                                        {
                                            resultmessage += "问题编码格式验证失败、";
                                            isadddobj = false;
                                        }
                                    }
                                    else
                                    {
                                        resultmessage += "问题编码格式验证失败、";
                                        isadddobj = false;
                                    }
                                }
                                //问题描述
                                if (!string.IsNullOrEmpty(questiondescribe))
                                {
                                    entity.questiondescribe = questiondescribe;
                                }
                                //问题地点
                                if (!string.IsNullOrEmpty(questionaddress))
                                {
                                    entity.questionaddress = questionaddress; //问题地点
                                }

                                #region 检查人员
                                if (!string.IsNullOrEmpty(checkperson))
                                {
                                    List<UserEntity> ckuserlist = new List<UserEntity>();
                                    if (checkperson.Contains("/"))
                                    {
                                        string[] persons = checkperson.Split('/');
                                        if (!string.IsNullOrEmpty(persons[0].ToString()))
                                        {
                                            ckuserlist = ulist.Where(p => p.RealName == persons[0].ToString().Trim()).ToList();
                                        }
                                        if (!string.IsNullOrEmpty(persons[1].ToString()))
                                        {
                                            ckuserlist = ulist.Where(p => p.Account == persons[1].ToString().Trim() || p.Mobile == persons[1].ToString().Trim() || p.Telephone == persons[1].ToString().Trim()).ToList();
                                        }
                                    }
                                    else
                                    {
                                        ckuserlist = ulist.Where(p => p.RealName == checkperson.Trim() || p.Account == checkperson.Trim() || p.Mobile == checkperson.Trim() || p.Telephone == checkperson.Trim()).ToList();
                                    }
                                    if (ckuserlist.Count() > 0)
                                    {
                                        var checkUserEntity = ckuserlist.FirstOrDefault();
                                        entity.checkpersonid = checkUserEntity.UserId;
                                        entity.checkpersonname = checkUserEntity.RealName;
                                        var checkDeptEntity = dlist.Where(p => p.DepartmentId == checkUserEntity.DepartmentId).FirstOrDefault();
                                        if (null != checkDeptEntity)
                                        {
                                            entity.checkdeptid = checkDeptEntity.DepartmentId;
                                            entity.checkdeptname = checkDeptEntity.FullName;
                                        }
                                    }
                                }
                                else
                                {
                                    entity.checkpersonid = curUser.UserId;
                                    entity.checkpersonname = curUser.UserName;
                                    entity.checkdeptid = curUser.DeptId;
                                    entity.checkdeptname = curUser.DeptName;
                                }
                                #endregion

                                //检查日期
                                if (!string.IsNullOrEmpty(checkdate))
                                {
                                    entity.checkdate = checkdate;
                                }
                                else
                                {
                                    entity.checkdate = DateTime.Now.ToString("yyyy-MM-dd");
                                }

                                //检查类型
                                if (!string.IsNullOrEmpty(checktype))
                                {
                                    var checktypeEntity = checktypelist.Where(p => p.ItemName == checktype.ToString()).FirstOrDefault();
                                    if (null != checktypeEntity)
                                    {
                                        entity.checktype = checktypeEntity.ItemValue;
                                        entity.checkname = checktype;
                                    }
                                }

                                //检查名称
                                if (!string.IsNullOrEmpty(checkname))
                                {
                                    entity.checkname = checkname;
                                    var checkEntity = checkrecordlist.Where(p => p.CheckDataRecordName == checkname.Trim()).FirstOrDefault();
                                    if (null != checkEntity)
                                    {
                                        checkid = checkEntity.ID; //检查记录id 
                                        entity.checkid = checkid; 
                                    }
                                }

                                //检查重点内容
                                if (!string.IsNullOrEmpty(checkimpcontent))
                                {
                                    entity.checkimpcontent = checkimpcontent;
                                }
                                //整改责任单位
                                if (!string.IsNullOrEmpty(reformdeptname))
                                {
                                    if (reformdeptname.Contains("/"))
                                    {
                                        string[] depts = reformdeptname.Split('/');
                                        var deptlist = dlist.Where(p => p.FullName == depts[0].ToString()).ToList();
                                        if (deptlist.Count() > 0)
                                        {
                                            var childdeptlist = dlist.Where(p => p.FullName == depts[1].ToString() && p.ParentId == deptlist.FirstOrDefault().DepartmentId).ToList();
                                            if (childdeptlist.Count() > 0)
                                            {
                                                var reformentity = childdeptlist.FirstOrDefault();
                                                entity.reformdeptid = reformentity.DepartmentId;
                                                entity.reformdeptcode = reformentity.EnCode;
                                                entity.reformdeptname = reformentity.FullName;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var deptlist = dlist.Where(e => e.FullName == reformdeptname).ToList();
                                        if (deptlist.Count() > 0)
                                        {
                                            var reformentity = deptlist.FirstOrDefault();
                                            entity.reformdeptid = reformentity.DepartmentId;
                                            entity.reformdeptcode = reformentity.EnCode;
                                            entity.reformdeptname = reformentity.FullName;
                                        }
                                    }
                                }
                                //联责单位
                                if (!string.IsNullOrEmpty(dutydeptname))
                                {
                                    if (dutydeptname.Contains("/"))
                                    {
                                        string[] depts = dutydeptname.Split('/');
                                        var deptlist = dlist.Where(p => p.FullName == depts[0].ToString()).ToList();
                                        if (deptlist.Count() > 0)
                                        {
                                            var childdeptlist = dlist.Where(p => p.FullName == depts[1].ToString() && p.ParentId == deptlist.FirstOrDefault().DepartmentId).ToList();
                                            if (childdeptlist.Count() > 0)
                                            {
                                                var dutydeptentity = childdeptlist.FirstOrDefault();
                                                entity.dutydeptid = dutydeptentity.DepartmentId;
                                                entity.dutydeptcode = dutydeptentity.EnCode;
                                                entity.dutydeptname = dutydeptentity.FullName;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var deptlist = dlist.Where(e => e.FullName == dutydeptname).ToList();
                                        if (deptlist.Count() > 0)
                                        {
                                            var dutydeptentity = deptlist.FirstOrDefault();
                                            entity.dutydeptid = dutydeptentity.DepartmentId;
                                            entity.dutydeptcode = dutydeptentity.EnCode;
                                            entity.dutydeptname = dutydeptentity.FullName;
                                        }
                                    }
                                }

                                #region 整改责任人
                                bool reformWarn = true;
                                if (!string.IsNullOrEmpty(reformpeoplename))
                                {
                                    List<UserEntity> reformuserlist = ulist;
                                    //整改责任单位 //联责单位
                                    if (!string.IsNullOrEmpty(entity.reformdeptid) || !string.IsNullOrEmpty(entity.dutydeptid))
                                    {
                                        reformuserlist = reformuserlist.Where(p => p.DepartmentId == entity.reformdeptid || p.DepartmentId == entity.dutydeptid).ToList();
                                    }

                                    string[] reformpeoples = new string[] { };

                                    if (reformpeoplename.Split(',').Length>0)
                                    {
                                        reformpeoples = reformpeoplename.Split(',');
                                    }
                                    else if (reformpeoplename.Split('，').Length > 0)
                                    {
                                        reformpeoples = reformpeoplename.Split('，');
                                    }
                                    else if (reformpeoplename.Split('、').Length > 0)
                                    {
                                        reformpeoples = reformpeoplename.Split('、');
                                    }

                                    int reformlen = reformpeoples.Length;
                                    int searchIndex = 0;
                                    foreach (string userstr in reformpeoples)
                                    {
                                        List<UserEntity> glreformusers = new List<UserEntity>();

                                        glreformusers = reformuserlist;

                                        if (!string.IsNullOrEmpty(userstr))
                                        {
                                            if (userstr.Contains("/"))
                                            {
                                                string[] persons = userstr.Split('/');
                                                if (!string.IsNullOrEmpty(persons[0].ToString()))
                                                {
                                                    glreformusers = glreformusers.Where(p => p.RealName == persons[0].ToString().Trim()).ToList();
                                                }
                                                if (!string.IsNullOrEmpty(persons[1].ToString()))
                                                {
                                                    glreformusers = glreformusers.Where(p => p.Account == persons[1].ToString().Trim() || p.Mobile == persons[1].ToString().Trim() || p.Telephone == persons[1].ToString().Trim()).ToList();
                                                }
                                            }
                                            else
                                            {
                                                glreformusers = glreformusers.Where(p => p.RealName == userstr.Trim() || p.Account == userstr.Trim() || p.Mobile == userstr.Trim() || p.Telephone == userstr.Trim()).ToList();
                                            }
                                            if (glreformusers.Count() > 0)
                                            {
                                                var reformUserEntity = glreformusers.FirstOrDefault();
                                                entity.reformpeople += reformUserEntity.Account + ",";
                                                entity.reformpeoplename += reformUserEntity.RealName + ",";
                                                if (!string.IsNullOrEmpty(reformUserEntity.Mobile))
                                                {
                                                    if (!string.IsNullOrEmpty(reformUserEntity.Telephone))
                                                    {
                                                        entity.reformtelephone += !string.IsNullOrEmpty(reformUserEntity.Mobile) ? reformUserEntity.Mobile + "," : reformUserEntity.Telephone + ",";
                                                    }
                                                    else
                                                    {
                                                        entity.reformtelephone += !string.IsNullOrEmpty(reformUserEntity.Mobile) ? reformUserEntity.Mobile + "," : "";
                                                    }
                                                }
                                                searchIndex++;
                                            }
                                        }
                                    }

                                    if (searchIndex == reformlen)
                                    {
                                        reformWarn = false;

                                        if (!string.IsNullOrEmpty(entity.reformpeople) && entity.reformpeople.Length >0)
                                        {
                                            entity.reformpeople = entity.reformpeople.Substring(0, entity.reformpeople.Length - 1);
                                        }
                                        if (!string.IsNullOrEmpty(entity.reformpeoplename) && entity.reformpeoplename.Length > 0)
                                        {
                                            entity.reformpeoplename = entity.reformpeoplename.Substring(0, entity.reformpeoplename.Length - 1);
                                        }
                                        if (!string.IsNullOrEmpty(entity.reformtelephone) && entity.reformtelephone.Length > 0)
                                        {
                                            entity.reformtelephone = entity.reformtelephone.Substring(0, entity.reformtelephone.Length - 1);
                                        }
                                    }
                                    
                                }
                                #endregion

                                //计划完成日期
                                if (!string.IsNullOrEmpty(reformplandate))
                                {
                                    entity.reformplandate = Convert.ToDateTime(reformplandate); //整改截止时间
                                }

                                //整改措施
                                if (!string.IsNullOrEmpty(reformmeasure))
                                {
                                    entity.reformmeasure = reformmeasure; //整改措施
                                }

                                //西塞山自动跳过
                                if (mode != "8")
                                {
                                    #region 验证人
                                    if (!string.IsNullOrEmpty(verifypeoplename))
                                    {
                                        List<UserEntity> verifyuserlist = new List<UserEntity>();
                                        if (verifypeoplename.Contains("/"))
                                        {
                                            string[] persons = verifypeoplename.Split('/');
                                            if (!string.IsNullOrEmpty(persons[0].ToString()))
                                            {
                                                verifyuserlist = ulist.Where(p => p.RealName == persons[0].ToString().Trim()).ToList();
                                            }
                                            if (!string.IsNullOrEmpty(persons[1].ToString()))
                                            {
                                                verifyuserlist = ulist.Where(p => p.Account == persons[1].ToString().Trim() || p.Mobile == persons[1].ToString().Trim() || p.Telephone == persons[1].ToString().Trim()).ToList();
                                            }
                                        }
                                        else
                                        {
                                            verifyuserlist = ulist.Where(p => p.RealName == verifypeoplename.Trim() || p.Account == verifypeoplename.Trim() || p.Mobile == verifypeoplename.Trim() || p.Telephone == verifypeoplename.Trim()).ToList();
                                        }
                                        if (verifyuserlist.Count() > 0)
                                        {
                                            var verifyUserEntity = verifyuserlist.FirstOrDefault();
                                            entity.verifypeople = verifyUserEntity.Account;
                                            entity.verifypeoplename = verifyUserEntity.RealName;
                                            var verifyDeptEntity = dlist.Where(p => p.DepartmentId == verifyUserEntity.DepartmentId).FirstOrDefault();
                                            if (null != verifyDeptEntity)
                                            {
                                                entity.verifydeptid = verifyDeptEntity.DepartmentId;
                                                entity.verifydeptcode = verifyDeptEntity.EnCode;
                                                entity.verifydeptname = verifyDeptEntity.FullName;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var currentUser = ulist.Where(p => p.UserId == curUser.UserId).FirstOrDefault();
                                        entity.verifypeople = curUser.Account;
                                        entity.verifypeoplename = curUser.UserName;
                                        entity.verifydeptid = curUser.DeptId;
                                        entity.verifydeptcode = curUser.DeptCode;
                                        entity.verifydeptname = curUser.DeptName;
                                    }
                                    #endregion
                                }
                                //验证日期
                                if (!string.IsNullOrEmpty(verifydate))
                                {
                                    entity.verifydate = Convert.ToDateTime(verifydate); //验证日期
                                }
                                #endregion

                                #region 必填验证
                                if (string.IsNullOrEmpty(questionnumber))
                                {
                                    resultmessage += "问题编码为空、";
                                    isadddobj = false;
                                }
                                if (string.IsNullOrEmpty(questiondescribe))
                                {
                                    resultmessage += "问题描述为空、";
                                    isadddobj = false;
                                }
                                if (string.IsNullOrEmpty(checktype))
                                {
                                    resultmessage += "检查类型为空、";
                                    isadddobj = false;
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(entity.checktype))
                                    {
                                        resultmessage += "检查类型不存在、";
                                        isadddobj = false;
                                    }
                                }
                                if (string.IsNullOrEmpty(reformpeoplename))
                                {
                                    resultmessage += "整改责任人为空、";
                                    isadddobj = false;
                                }
                                else
                                {
                                    if (reformWarn)
                                    {
                                        resultmessage += "整改责任人中有人员填写错误或不存在于整改责任单位(联责单位)、";
                                        isadddobj = false;
                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(entity.reformpeople))
                                        {
                                            resultmessage += "整改责任人填写错误或不存在、";
                                            isadddobj = false;
                                        }
                                    }
                                }
                               
                                if (string.IsNullOrEmpty(reformdeptname))
                                {
                                    resultmessage += "整改责任单位为空、";
                                    isadddobj = false;
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(entity.reformdeptid))
                                    {
                                        resultmessage += "整改责任单位填写错误或不存在、";
                                        isadddobj = false;
                                    }
                                }

                                if (string.IsNullOrEmpty(reformmeasure))
                                {
                                    resultmessage += "整改措施为空、";
                                    isadddobj = false;
                                }
                                if (string.IsNullOrEmpty(reformplandate))
                                {
                                    resultmessage += "计划完成日期为空、";
                                    isadddobj = false;
                                }

                                if (isadddobj)
                                {
                                    list.Add(entity);
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(resultmessage))
                                    {
                                        resultmessage = resultmessage.Substring(0, resultmessage.Length - 1) + ",无法正常导入";
                                        resultlist.Add(resultmessage);
                                    }
                                }
                                #endregion
                            }
                            catch
                            {
                                resultmessage += "出现数据异常,无法正常导入";
                                resultlist.Add(resultmessage);
                            }
                        }
                        if (resultlist.Count > 0)
                        {
                            foreach (string str in resultlist)
                            {
                                falseMessage += str + "</br>";
                            }
                        }
                        #endregion
                        #region 问题数据集合

                        foreach (ImportQuestion entity in list)
                        {
                            string keyValue = string.Empty;
                            int excuteVal = 0;
                            //问题基本信息
                            QuestionInfoEntity baseentity = new QuestionInfoEntity();

                            //获取已存在的问题数据
                            var questionlist = questioninfobll.GetListByNumber(entity.questionnumber);

                            if (questionlist.Count() > 0)
                            {
                                //覆盖操作
                                if (repeatdata == "1")
                                {
                                    var otherQuestion = questionlist.Where(p => p.CREATEUSERID != curUser.UserId);
                                    //其他人创建的
                                    if (otherQuestion.Count() > 0)
                                    {
                                        falseMessage += "问题编码为'" + entity.questionnumber + "'的数据因已被" + otherQuestion.FirstOrDefault().CREATEUSERNAME + "创建而无法覆盖,不予操作</br>";
                                        excuteVal = -1;
                                    }
                                    else //自己创建
                                    {
                                        baseentity = questionlist.FirstOrDefault();
                                        //关联安全检查的自动跳过
                                        if (!string.IsNullOrEmpty(baseentity.CHECKID))
                                        {
                                            excuteVal = -2;
                                        }
                                        else
                                        { //先删除，后新增
                                            questioninfobll.RemoveForm(baseentity.ID);
                                            baseentity = new QuestionInfoEntity();
                                            excuteVal = 1;
                                        }
                                    }
                                }
                                else  //跳过
                                {
                                    excuteVal = 0;
                                }
                            }
                            else
                            {
                                excuteVal = 1;
                            }

                            if (!string.IsNullOrEmpty(entity.checkid))
                            {
                                checkproject = sdbll.GetCheckItemCount(entity.checkid);

                                if (checkproject>0)
                                {
                                    string checkconnectid = saftycheckdatarecordbll.GetCheckContentId(entity.checkid, entity.checkobj, entity.checkcontent, curUser);
                                    if (!string.IsNullOrEmpty(checkconnectid))
                                    {
                                        if (checkconnectid.Contains(","))
                                        {
                                            entity.correlationid = checkconnectid.Split(',')[0].ToString();
                                            entity.relevanceid = checkconnectid.Split(',')[1].ToString();
                                        }
                                    }
                                    if (string.IsNullOrEmpty(entity.correlationid))
                                    {
                                        excuteVal = -3;
                                        falseMessage += "问题编码为'" + entity.questionnumber + "'的数据检查对象及检查内容未匹配或者当前检查项已被检查过或者该项属于其他人检查范围,无法正常导入</br>";
                                    }
                                    else
                                    {
                                        listIds.Add(entity.correlationid);
                                    }
                                }
                            }

                            if (excuteVal > 0)
                            {
                                baseentity.APPSIGN = "Import"; //导入标记
                                baseentity.QUESTIONNUMBER = entity.questionnumber;//问题编码
                                baseentity.CHECKCONTENT = entity.checkimpcontent; //检查重点内容
                                baseentity.RELEVANCEID = entity.relevanceid; //检查对象id
                                baseentity.CORRELATIONID = entity.correlationid; //检查内容id
                                baseentity.BELONGDEPTID = curUser.OrganizeId;
                                baseentity.BELONGDEPTNAME = curUser.OrganizeName;

                                baseentity.CHECKID = entity.checkid; //检查记录id
                                baseentity.CHECKNAME = entity.checkname;
                                baseentity.CHECKTYPE = entity.checktype;
                                baseentity.CHECKPERSONNAME = entity.checkpersonname;
                                baseentity.CHECKPERSONID = entity.checkpersonid;
                                baseentity.CHECKDEPTID = entity.checkdeptid;
                                baseentity.CHECKDEPTNAME = entity.dutydeptname;
                                baseentity.CHECKDATE = DateTime.Now; //检查时间
                                #region 注释
                                //if (null != safetyEntity)
                                //{
                                //    baseentity.CHECKNAME = safetyEntity.CheckDataRecordName;
                                //    baseentity.CHECKTYPE = checktypelist.Where(p => p.ItemValue == safetyEntity.CheckDataType.Value.ToString()).FirstOrDefault().ItemDetailId;
                                //    baseentity.CHECKPERSONNAME = curUser.UserName;
                                //    baseentity.CHECKPERSONID = curUser.UserId;
                                //    baseentity.CHECKDEPTID = curUser.DeptId;
                                //    baseentity.CHECKDEPTNAME = curUser.DeptName;
                                //} 
                                #endregion
                                baseentity.QUESTIONADDRESS = entity.questionaddress; //问题地点
                                baseentity.QUESTIONDESCRIBE = entity.questiondescribe;//问题描述
                                baseentity.QUESTIONPIC = Guid.NewGuid().ToString();
                                #region 添加问题图片
                                if (!string.IsNullOrEmpty(hiddenDirectory))
                                {
                                    //当前文件夹存在
                                    if (Directory.Exists(hiddenDirectory))
                                    {
                                        //读取图片
                                        DirectoryInfo directinfo = new DirectoryInfo(hiddenDirectory);
                                        List<FileInfo> fileinfoes = GetFiles(directinfo, new List<FileInfo>());
                                        #region 图片文件
                                        foreach (FileInfo finfo in fileinfoes)
                                        {
                                            string fextension = finfo.Extension;//文件扩展名
                                            string fname = finfo.Name; //文件名称
                                            //过滤图片格式
                                            #region 过滤图片格式
                                            if (fextension.ToLower().Contains("jpg") || fextension.ToLower().Contains("png") || fextension.ToLower().Contains("bmp")
                                             || fextension.ToLower().Contains("psd") || fextension.ToLower().Contains("gif") || fextension.ToLower().Contains("jpeg"))
                                            {
                                                string fserialnumber = fname.Split('-')[0].ToString();
                                                if (entity.questionnumber.ToString() == fserialnumber)
                                                {
                                                    string fileGuid = Guid.NewGuid().ToString();
                                                    long filesize = finfo.Length;
                                                    string uploadDate = DateTime.Now.ToString("yyyyMMdd");
                                                    string virtualPath = string.Format("~/Resource/ht/images/{0}/{1}{2}", uploadDate, fileGuid, fextension);
                                                    string fullFileName = Server.MapPath(virtualPath);
                                                    //创建文件夹
                                                    string path = Path.GetDirectoryName(fullFileName);
                                                    Directory.CreateDirectory(path);
                                                    FileInfoEntity fileInfoEntity = new FileInfoEntity();
                                                    if (!System.IO.File.Exists(fullFileName))
                                                    {
                                                        //保存文件
                                                        finfo.CopyTo(fullFileName);
                                                    }
                                                    finfo.Delete();//删除文件
                                                    //文件信息写入数据库
                                                    fileInfoEntity.Create();
                                                    fileInfoEntity.FileId = fileGuid;
                                                    fileInfoEntity.RecId = baseentity.QUESTIONPIC; //关联ID
                                                    fileInfoEntity.FolderId = "ht/images";
                                                    fileInfoEntity.FileName = file.FileName;
                                                    fileInfoEntity.FilePath = virtualPath;
                                                    fileInfoEntity.FileSize = filesize.ToString();
                                                    fileInfoEntity.FileExtensions = fextension;
                                                    fileInfoEntity.FileType = fextension.Replace(".", "");
                                                    fileinfobll.SaveForm("", fileInfoEntity);
                                                }
                                            }
                                            #endregion
                                        }
                                        #endregion
                                    }
                                }
                                #endregion
                                questioninfobll.SaveForm("", baseentity);
                                string workFlow = "09";//问题处理
                                bool isSucess = htworkflowbll.CreateWorkFlowObj(workFlow, baseentity.ID, curUser.UserId);
                                if (isSucess)
                                {
                                    htworkflowbll.UpdateFlowStateByObjectId("bis_questioninfo", "flowstate", baseentity.ID);  //更新业务流程状态
                                }
                                //整改基本信息
                                QuestionReformEntity centity = new QuestionReformEntity();
                                centity.APPSIGN = "Import";
                                centity.QUESTIONID = baseentity.ID;
                                centity.REFORMMEASURE = entity.reformmeasure;
                                centity.REFORMPEOPLENAME = entity.reformpeoplename;
                                centity.REFORMPEOPLE = entity.reformpeople;
                                centity.REFORMTEL = entity.reformtelephone;
                                centity.REFORMDEPTID = entity.reformdeptid;
                                centity.REFORMDEPTCODE = entity.reformdeptcode;
                                centity.REFORMDEPTNAME = entity.reformdeptname;
                                centity.REFORMPLANDATE = entity.reformplandate;
                                centity.REFORMPIC = Guid.NewGuid().ToString();
                                centity.DUTYDEPTID = entity.dutydeptid;
                                centity.DUTYDEPTNAME = entity.dutydeptname;
                                centity.DUTYDEPTCODE = entity.dutydeptcode;
                                questionreformbll.SaveForm("", centity);

                                //当前没有配置流程，则新加验证信息
                                if (!isHavaWorkFlow)
                                {
                                    //验证基本信息
                                    QuestionVerifyEntity aentity = new QuestionVerifyEntity();
                                    aentity.APPSIGN = "Import";
                                    aentity.QUESTIONID = baseentity.ID;
                                    aentity.VERIFYDEPTCODE = entity.verifydeptcode;
                                    aentity.VERIFYDEPTNAME = entity.verifydeptname;
                                    aentity.VERIFYPEOPLE = entity.verifypeople;
                                    aentity.VERIFYPEOPLENAME = entity.verifypeoplename;
                                    aentity.VERIFYDEPTID = entity.verifydeptid;
                                    aentity.VERIFYDATE = entity.verifydate;
                                    questionverifybll.SaveForm("", aentity);
                                }

                                //此处需要判断当前人是否为安全管理员
                                string wfFlag = "1"; //到整改

                                if (!string.IsNullOrEmpty(centity.REFORMPEOPLE))
                                {
                                    int resultValue = htworkflowbll.SubmitWorkFlow(baseentity.ID, centity.REFORMPEOPLE, wfFlag, curUser.UserId);

                                    if (resultValue > 0)
                                    {
                                        htworkflowbll.UpdateFlowStateByObjectId("bis_questioninfo", "flowstate", baseentity.ID);  //更新业务流程状态
                                    }
                                }

                                total += 1;
                            }
                            else if(excuteVal == 0)
                            {
                                falseMessage += "问题编码为"+ entity.questionnumber + "的数据因问题编码重复而自动跳过,不予操作</br>";
                            }
                            else if (excuteVal == -2)
                            {
                                falseMessage += "问题编码为" + entity.questionnumber + "的数据因已关联安全检查而自动跳过,不予操作</br>";
                            }
                        }
                        #endregion
                    }
                    #endregion
                    count = dt.Rows.Count;
                    message = "共有" + count.ToString() + "条记录,成功导入" + total.ToString() + "条,失败" + (count - total).ToString() + "条";
                    message += "</br>" + falseMessage;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return message;
        }
        #endregion

        #region MyRegion
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="direct"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public List<FileInfo> GetFiles(DirectoryInfo direct, List<FileInfo> files)
        {

            FileInfo[] fileinfoes = direct.GetFiles();
            DirectoryInfo[] directlist = direct.GetDirectories();
            foreach (FileInfo finfo in fileinfoes)
            {
                files.Add(finfo);
            }
            foreach (DirectoryInfo cdirect in directlist)
            {
                GetFiles(cdirect, files);
            }
            return files;
        }
        #endregion
    }
}