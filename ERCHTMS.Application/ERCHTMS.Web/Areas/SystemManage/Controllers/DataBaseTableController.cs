using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.SystemManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Web.Mvc;
using System.IO;
using System;
using System.Data;
using System.Linq;
using System.Drawing;
using Aspose.Words;
namespace ERCHTMS.Web.Areas.SystemManage.Controllers
{
    /// <summary>
    /// 描 述：据库表管理
    /// </summary>
    public class DataBaseTableController : MvcControllerBase
    {
        private DataBaseTableBLL dataBaseTableBLL = new DataBaseTableBLL();
        private DataBaseLinkBLL databaseLinkBLL = new DataBaseLinkBLL();

        #region 视图功能
        /// <summary>
        /// 数据库管理
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        [HandlerMonitor(3, "访问数据库表列表")]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 数据表表单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult Form()
        {
            return View();
        }
        /// <summary>
        /// 打开表数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult TableData()
        {
            return View();
        }
        /// <summary>
        /// 数据表字段表单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FieldForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 数据库表列表
        /// </summary>
        /// <param name="dataBaseLinkId">库连接Id</param>
        /// <param name="keyword">关键字查询</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetTableListJson(string dataBaseLinkId, string keyword)
        {
            var data = dataBaseTableBLL.GetTableList(dataBaseLinkId, keyword);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 数据库表字段列表 
        /// </summary>
        /// <param name="dataBaseLinkId">库连接Id</param>
        /// <param name="tableName">表明</param>
        /// <param name="nameId">字段Id</param>
        /// <returns>返回树形Json</returns>
        [HttpGet]
        public ActionResult GetTableFiledTreeJson(string dataBaseLinkId, string tableName, string nameId)
        {
            List<string> nameArray = new List<string>();
            if (!string.IsNullOrEmpty(nameId))
            {
                nameArray = new List<string>(nameId.Split(','));
            }

            var data = dataBaseTableBLL.GetTableFiledList(dataBaseLinkId, tableName);
            var treeList = new List<TreeEntity>();
            TreeEntity tree = new TreeEntity();
            tree.id = tableName;
            tree.text = tableName;
            tree.value = tableName;
            tree.parentId = "0";
            tree.img = "fa fa-list-alt";
            tree.isexpand = true;
            tree.complete = true;
            tree.hasChildren = true;
            treeList.Add(tree);
            foreach (DataBaseTableFieldEntity item in data)
            {
                tree = new TreeEntity();
                tree.id = item.column_name;
                tree.text = item.remark + "（" + item.column_name + "）";
                tree.value = item.remark;
                tree.parentId = tableName;
                tree.img = "fa fa-wrench";
                tree.isexpand = true;
                tree.complete = true;
                tree.showcheck = true;
                tree.checkstate = nameArray.Contains(item.column_name) == true ? 1 : 0;
                tree.hasChildren = false;
                treeList.Add(tree);
            }
            return Content(treeList.TreeToJson());
        }
        /// <summary>
        /// 数据库表字段列表
        /// </summary>
        /// <param name="dataBaseLinkId">库连接Id</param>
        /// <param name="tableName">表明</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetTableFiledListJson(string dataBaseLinkId, string tableName)
        {
            var data = dataBaseTableBLL.GetTableFiledList(dataBaseLinkId, tableName);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 数据库表数据列表
        /// </summary>
        /// <param name="dataBaseLinkId">库连接Id</param>
        /// <param name="tableName">表明</param>
        /// <param name="switchWhere">条件</param>
        /// <param name="logic">逻辑</param>
        /// <param name="keyword">关键字</param>
        /// <param name="pagination">分页参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetTableDataListJson(string dataBaseLinkId, string tableName, string switchWhere, string logic, string keyword, Pagination pagination)
        {
            var watch = CommonHelper.TimerStart();
            var data = dataBaseTableBLL.GetTableDataList(dataBaseLinkId, tableName, switchWhere, logic, keyword, pagination);
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return ToJsonResult(JsonData);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 保存数据库表表单（新增、修改）
        /// </summary>
        /// <param name="dataBaseLinkId">库连接Id</param>
        /// <param name="tableName">表名称</param>
        /// <param name="tableDescription">表说明</param>
        /// <param name="fieldListJson">字段列表Json</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "保存数据库表表单(新增、修改)")]
        public ActionResult SaveForm(string dataBaseLinkId, string tableName, string tableDescription, string fieldListJson)
        {
            dataBaseTableBLL.SaveForm(dataBaseLinkId, tableName, tableDescription, fieldListJson);
            return Success("操作成功。");
        }
        /// <summary>
        /// 删除表
        /// </summary>
        /// <param name="dataBaseLinkId">库连接Id</param>
        /// <param name="tableName">表名称</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(6, "删除表")]
        public ActionResult RemoveForm(string dataBaseLinkId, string tableName)
        {
            dataBaseTableBLL.RemoveForm(dataBaseLinkId, tableName);
            return Success("操作成功。");
        }
        /// <summary>
        /// 保存表字段信息（新增、修改）
        /// </summary>
        /// <param name="dataBaseLinkId">库连接Id</param>
        /// <param name="tableName">表名称</param>
        /// <param name="tableDescription">表说明</param>
        /// <param name="fieldListJson">字段列表Json</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "保存表字段信息(新增、修改)")]
        public ActionResult SaveField(string dataBaseLinkId, string tableName, string tableDescription, string fieldJson)
        {
            dataBaseTableBLL.SaveForm(dataBaseLinkId, tableName, tableDescription, fieldJson);
            return Success("操作成功。");
        }
        /// <summary>
        ///导出数据库设计报告文档
        /// </summary>
        public ActionResult CreateDBDoc(string dataBaseLinkId)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                //获取所有用户表信息
                DataTable dtTableLIst = dataBaseTableBLL.GetTableList(dataBaseLinkId, "");
                dtTableLIst.TableName = "T";

                //加载导出模板
                Aspose.Words.Document doc = new Aspose.Words.Document(Server.MapPath("~/Resource/Temp/db.doc"));
                Aspose.Words.DocumentBuilder db = new Aspose.Words.DocumentBuilder(doc);

                //填充2.1表汇总信息
                doc.MailMerge.ExecuteWithRegions(dtTableLIst);
                //系统中文名称
                string zhName = Config.GetValue("SystemName");
                //软件英文名称
                string enName = Config.GetValue("SoftName");
                //程序版本
                string version = Config.GetValue("Version");
                string appName = zhName + " " + version + "(" + enName + ")";
                //数据库连接信息
                DataBaseLinkEntity dbLink = databaseLinkBLL.GetEntity(dataBaseLinkId);
                //获取数据库类型及版本
                string dbName = dbLink.DbType + dbLink.Version;
                //填充基本信息
                doc.MailMerge.Execute(new string[] { "appName", "dbName" }, new string[] { appName, dbName });
                //表详细设计起始编号
                string num = "2.2.";
                //开始绘制表格信息
                db.MoveToDocumentEnd();
                int j = 1;
                foreach (DataRow dr in dtTableLIst.Rows)
                {
                    //表显示名称
                    string title = dr["name"].ToString();
                    if (dr["tdescription"] != DBNull.Value && dr["tdescription"] != null)
                    {
                        title = num + j + " " + dr["tdescription"].ToString() + "(" + title + ")";
                    }
                    else
                    {
                        title = num + j + " " + dr["name"].ToString();
                    }
                    //定义书签名称
                    string bookName = "mark" + j;
                    //开始创建书签并填充信息
                    db.StartBookmark(bookName);
                    db.ParagraphFormat.StyleIdentifier = StyleIdentifier.Heading3;
                    db.ParagraphFormat.Alignment = Aspose.Words.ParagraphAlignment.Left;
                    db.Bold = true;
                    db.Write(title);
                    db.EndBookmark(bookName);

                    //表名
                    string tableName = dr["name"].ToString();
                    //获取表结构
                    var tableInfo = dataBaseTableBLL.GetTableFiledList(dataBaseLinkId, tableName).ToList();
                    db.MoveToDocumentEnd();
                    //开始绘制表结构表格
                    db.StartTable();
                    //表头设置
                    db.ParagraphFormat.StyleIdentifier = StyleIdentifier.Normal;
                    db.ParagraphFormat.Alignment = Aspose.Words.ParagraphAlignment.Center;
                    db.RowFormat.Alignment = Aspose.Words.Tables.RowAlignment.Center;
                    db.Bold = true;
                    db.CellFormat.Borders.Color = Color.Black;
                    db.CellFormat.Borders.LineStyle = Aspose.Words.LineStyle.Single;
                    db.InsertCell();
                    db.Write("列名");
                    db.InsertCell();
                    db.Write("数据类型");
                    db.InsertCell();
                    db.Write("可为空");
                    db.InsertCell();
                    db.Write("字段描述");
                    db.InsertCell();
                    db.Write("备注");
                    db.EndRow();

                    //开始绘制列信息
                    foreach (DataBaseTableFieldEntity field in tableInfo)
                    {
                        db.Bold = false;
                        db.ParagraphFormat.Alignment = Aspose.Words.ParagraphAlignment.Center;
                        db.RowFormat.Alignment = Aspose.Words.Tables.RowAlignment.Center;
                        db.CellFormat.Borders.Color = Color.Black;
                        db.CellFormat.Borders.LineStyle = Aspose.Words.LineStyle.Single;
                        db.InsertCell();
                        db.Write(field.column_name);//列名称
                        db.InsertCell();
                        db.Write(field.datatype);//列类型
                        db.InsertCell();
                        //是否非空
                        db.Write(field.isnullable=="1"?"否":"是");
                        //列描述
                        db.InsertCell();
                        if (string.IsNullOrEmpty(field.remark))
                        {
                            db.Write("");
                        }
                        else
                        {
                            db.ParagraphFormat.Alignment = Aspose.Words.ParagraphAlignment.Left;
                            db.Write(field.remark);
                        }
                        //备注
                        db.InsertCell();
                        db.Write("");
                        db.EndRow();
                    }
                    db.EndTable();
                    db.ParagraphFormat.ClearFormatting();
                    db.InsertParagraph(); db.InsertParagraph();
                    j++;
                }
                //生成文档目录导航
                db.ParagraphFormat.ClearFormatting();
                //移动到目录书签
                db.MoveToBookmark("dir");
                //db.InsertTableOfContents("\\o \"1-3\" \\h \\z \\u");

                Aspose.Words.Layout.LayoutCollector lc = new Aspose.Words.Layout.LayoutCollector(doc);
                j = 1;
                foreach (DataRow dr in dtTableLIst.Rows)
                {
                    string title = num + j + " " + dr["name"].ToString();
                    if (dr["tdescription"] != DBNull.Value && dr["tdescription"] != null)
                    {
                        title = num + j + " " + dr["tdescription"].ToString();
                    }
                    string bookName = "mark" + j;
                    db.ParagraphFormat.Alignment = Aspose.Words.ParagraphAlignment.Right;
                    //获取表书签所在的页码
                    string page = "";
                    if (doc.Range.Bookmarks[bookName] != null)
                    {
                        int pageIndex = lc.GetStartPageIndex(doc.Range.Bookmarks[bookName].BookmarkStart);
                        page = pageIndex.ToString();
                    }
                    //db.Bold = false;
                    //生成目录链接导航
                    //db.InsertHyperlink(title, bookName, true);
                    j++;
                }
                //设置文件名
                string fileName = zhName + "数据库设计报告_" + DateTime.Now.ToString("yyyyMMdd") + ".doc";
                doc.Save(Server.MapPath("~/Resource/Temp/" + fileName));

                return Success("操作成功", fileName + "$" + CommonHelper.TimerEnd(watch));
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        #endregion


    }
}
