using ERCHTMS.Entity.SafetyMeshManage;
using ERCHTMS.Busines.SafetyMeshManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System;
using System.Linq;
using System.Data;
using ERCHTMS.Code;
using System.Collections.Generic;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Busines.BaseManage;
using BSFramework.Util.Extension;
using BSFramework.Util.Offices;
using System.Text;
using System.Web;
using Aspose.Cells;
using System.Drawing;

namespace ERCHTMS.Web.Areas.SafetyMeshManage.Controllers
{
    /// <summary>
    /// 描 述：安全生产领导机构
    /// </summary>
    public class SafetyMeshController : MvcControllerBase
    {
        private SafetyMeshBLL SafetyMeshbll = new SafetyMeshBLL();

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
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson()
        {
            var data = SafetyMeshbll.GetList();
            return ToJsonResult(data);
        }
        [HttpGet]
        public ActionResult GetTableJson(string queryJson)
        {
            var data = SafetyMeshbll.GetTable(queryJson);
            //DepartmentBLL deptBll = new DepartmentBLL();
            //foreach(DataRow dr in data.Rows)
            //{
            //    int count = deptBll.GetDataTable(string.Format("select count(1) from BIS_NETORGCHANGERECORD t where userid='{0}' and changeid in(select id from bis_netorgchange where isover=0)",dr["userid"].ToString())).Rows[0][0].ToInt();
            //    dr["state"] = count>0?"1":"0";
            //    count = deptBll.GetDataTable(string.Format("select count(1) from BIS_NETORGCHANGERECORD t where userid='{0}' and changeid in(select id from bis_netorgchange where isover=0)", dr["userid1"].ToString())).Rows[0][0].ToInt();
            //    dr["state1"] = count > 0 ? "1" : "0";
            //}
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
            var data = SafetyMeshbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="MeshRank">级别</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetWorkJob(string MeshRank)
        {
            var user = OperatorProvider.Provider.Current();//所属公司
            var disItem = SafetyMeshbll.GetListForCon(x => x.MeshRank == MeshRank && x.CreateUserOrgCode == user.OrganizeCode).OrderByDescending(x => x.CreateDate).FirstOrDefault();
            return ToJsonResult(disItem);
        }
        /// <summary>
        /// 名称不能重复
        /// </summary>
        /// <param name="MeshName">网格名称</param>
        /// <param name="SuperiorId">上级编号</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExistName(string MeshName, string SuperiorId)
        {
            var user = OperatorProvider.Provider.Current();//所属公司
            var list = SafetyMeshbll.GetListForCon(x => x.MeshName == MeshName && x.SuperiorId == SuperiorId && x.CreateUserOrgCode == user.OrganizeCode).ToList();
            bool IsOk = list.Count() == 0 ? true : false;
            return Content(IsOk.ToString());
        }
        #region 获取上级网格树结构
        /// <summary>
        /// 获取上级网格树结构
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSuperiorDataJson(string orgID = "0", string keyword = "")
        {
            var user = OperatorProvider.Provider.Current();//所属公司    

            var list = SafetyMeshbll.GetListForCon(x => x.CreateUserOrgCode == user.OrganizeCode).ToList();
            List<SafetyMeshEntity> meshdata = new List<SafetyMeshEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                list = list.Where(t => t.MeshName.Contains(keyword.Trim())).ToList();
            }
            meshdata = list.OrderBy(a => a.SortCode).ToList();
            if (orgID != "0")
            {
                meshdata = meshdata.Where(a => a.CreateUserOrgCode == orgID).ToList();
            }
            //List<SafetyMeshEntity> list1 = new List<SafetyMeshEntity>();
            //foreach (SafetyMeshEntity entity in meshdata)
            //{
            //    string code = string.Empty;
            //    if (entity.DistrictCode.Length > 5)
            //    {
            //        code = entity.DistrictCode.Substring(0, 6);
            //    }
            //    else
            //    {
            //        code = entity.DistrictCode.Substring(0, 3);
            //    }
            //    if (districtdata.Where(t => t.DistrictCode == code).Count() == 0)
            //    {
            //        DistrictEntity de = list.Where(t => t.DistrictCode == code).FirstOrDefault();
            //        if (de != null)
            //        {
            //            if (!list1.Contains(de))
            //            {
            //                list1.Add(de);
            //            }
            //        }
            //    }
            //}
            //meshdata = meshdata.Concat(list).ToList();
            meshdata = meshdata.OrderBy(t => t.SortCode).ToList();
            List<TreeEntity> treeList = new List<TreeEntity>();

            //集合
            //var data = SafetyMeshbll.GetList(null);


            //var treeList = new List<TreeEntity>();
            foreach (SafetyMeshEntity item in meshdata)
            {
                TreeEntity tree = new TreeEntity();
                bool hasChildren = meshdata.Where(p => p.SuperiorId == item.Id).ToList().Count() == 0 ? false : true;
                tree.id = item.Id;
                tree.text = item.MeshName;
                tree.value = item.Id;
                tree.complete = true;
                tree.hasChildren = string.IsNullOrEmpty(keyword) ? hasChildren : false;
                tree.parentId = string.IsNullOrEmpty(keyword) ? (item.SuperiorId == null ? "0" : item.SuperiorId) : "0";
                tree.Attribute = "Code";
                tree.AttributeValue = item.MeshRank;
                tree.isexpand = false;
                tree.showcheck = true;
                treeList.Add(tree);
            }
            if (treeList.Count > 0)
            {
                treeList[0].isexpand = true;
            }
            return Content(treeList.TreeToJson());

        }
        #endregion
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
            SafetyMeshbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, SafetyMeshEntity entity)
        {
            try
            {
                SafetyMeshbll.SaveForm(keyValue, entity);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        [HandlerMonitor(0, "导出安全网格")]
        public ActionResult Export(string queryJson)
        {
            try
            {
                var exportTable = SafetyMeshbll.GetTableList(queryJson);

                ////调用导出方法
                //ExcelHelper.ExcelDownload(data, excelconfig);
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                string fName = "安全网格_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                wb.Open(Server.MapPath("~/Resource/ExcelTemplate/tmp.xls"));
                var num = wb.Worksheets[0].Cells.Columns.Count;

                Aspose.Cells.Worksheet sheet = wb.Worksheets[0] as Aspose.Cells.Worksheet;
                Aspose.Cells.Cell cell = sheet.Cells[0, 0];
                cell.PutValue("安全网格"); //标题
                cell.Style.Pattern = BackgroundType.Solid;
                cell.Style.Font.Size = 16;
                cell.Style.Font.Color = Color.Black;
                List<string> colList = new List<string>() { "网格名称", "上级网格名称", "网络责任人", "联系电话", "关联区域", "网络级别", "工作职责" };
                List<string> colList1 = new List<string>() { "meshname", "superiorname", "dutyuser", "dutytel", "district", "meshrank", "workjob" };
                for (int i = 0; i < colList.Count; i++)
                {
                    //序号列
                    Aspose.Cells.Cell serialcell = sheet.Cells[1, 0];
                    serialcell.PutValue(" ");

                    for (int j = 0; j < colList.Count; j++)
                    {
                        Aspose.Cells.Cell curcell = sheet.Cells[1, j];
                        //sheet.Cells.SetColumnWidth(j, 40);
                        curcell.Style.Pattern = BackgroundType.Solid;
                        curcell.Style.Font.Size = 12;
                        curcell.Style.Font.Color = Color.Black;
                        curcell.PutValue(colList[j].ToString()); //列头
                    }
                    Aspose.Cells.Cells cells = sheet.Cells;
                    cells.Merge(0, 0, 1, colList.Count);
                }
                for (int i = 0; i < exportTable.Rows.Count; i++)
                {
                    //内容填充
                    for (int j = 0; j < colList1.Count; j++)
                    {
                        Aspose.Cells.Cell curcell = sheet.Cells[i + 2, j];
                        curcell.PutValue(exportTable.Rows[i][colList1[j]].ToString());
                    }

                }
                HttpResponse resp = System.Web.HttpContext.Current.Response;
                //wb.Save(Server.MapPath("~/Resource/Temp/" + fName));
                wb.Save(Server.UrlEncode(fName), Aspose.Cells.FileFormatType.Excel2003, Aspose.Cells.SaveType.OpenInBrowser, resp);

                return Success("导出成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        [HttpPost]
        public ActionResult ExpInfo(string json)
        {
            try
            {
                Operator curUser = OperatorProvider.Provider.Current();
                string title = "安全网格";
                //设置文件名
                string fileName = curUser.OrganizeName + title + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
                title = curUser.OrganizeName + "<br />" + title;
                json = HttpUtility.UrlDecode(json);
                //加载导出模板
                Aspose.Words.Document doc = new Aspose.Words.Document(Server.MapPath("~/Resource/DocumentFile/html.docx"));
                Aspose.Words.DocumentBuilder db = new Aspose.Words.DocumentBuilder(doc);
                //导入到书签
                db.MoveToBookmark("title");
                db.InsertHtml(title);
                db.MoveToBookmark("HTML");
                db.InsertHtml(json.Replace(@"\", "").Replace(@"&nbsp;", "").TrimStart('"').TrimEnd('"'));
                doc.Save(Server.MapPath("~/Resource/Temp/" + fileName));
                string path = Server.MapPath("~/Resource/Temp/" + fileName);
                return Success("操作成功", fileName);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        /// <summary>
        /// 导入安全网格
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(5, "导入安全网格")]
        public string ImportData()
        {
            var user = OperatorProvider.Provider.Current();//所属公司          
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
                DataTable dt = cells.ExportDataTable(0, 0, cells.MaxDataRow + 1, cells.MaxColumn + 1, true);

                string orgid = OperatorProvider.Provider.Current().OrganizeId;
                int order = 2;
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    SafetyMeshEntity item = new SafetyMeshEntity();
                    order++;
                    #region 网格名称
                    string MeshName = dt.Rows[i][0].ToString();
                    if (!string.IsNullOrEmpty(MeshName))
                    {
                        item.MeshName = MeshName;
                    }
                    else
                    {
                        falseMessage += string.Format(@"第{0}行导入失败,网格名称不能为空！</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region 关联区域
                    string District = dt.Rows[i][1].ToString();
                    bool flag = false;
                    if (!string.IsNullOrEmpty(District))
                    {
                        string DistrictIdStr = "";
                        string DistrictStr = "";
                        string[] districtArr = District.Split(',');

                        foreach (string district in districtArr)
                        {
                            var disItem = new DistrictBLL().GetListForCon(x => x.DistrictName == district.Trim() && x.OrganizeId == orgid).FirstOrDefault();
                            if (disItem != null)
                            {
                                DistrictIdStr += disItem.DistrictID + ",";
                                DistrictStr += district.Trim() + ",";
                            }
                            else
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,关联区域[" + district + "]不存在！</br>", order);
                                error++;
                                flag = true;
                                continue;
                            }
                        }
                        item.DistrictId = DistrictIdStr.TrimEnd(',');
                        item.District = DistrictStr.TrimEnd(',');
                    }
                    if (flag)
                    {
                        continue;
                    }
                    #endregion

                    #region 上级网格名称
                    string Superior = dt.Rows[i][2].ToString();
                    if (!string.IsNullOrEmpty(Superior))
                    {
                        var disItem = SafetyMeshbll.GetListForCon(x => x.MeshName == Superior.Trim() && x.CreateUserOrgCode == user.OrganizeCode).FirstOrDefault();
                        if (disItem != null)
                        {
                            item.SuperiorId = disItem.Id;
                            item.SuperiorName = disItem.MeshName;
                            if (disItem.MeshRank == "1")
                            {
                                item.MeshRank = "2";
                            }
                            if (disItem.MeshRank == "2")
                            {
                                item.MeshRank = "3";
                            }
                            if (disItem.MeshRank == "3")
                            {
                                item.MeshRank = "4";
                            }
                            if (disItem.MeshRank == "4")
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,上级网格名称[" + Superior + "]为微级网格！</br>", order);
                                error++;
                                continue;
                            }
                            var list = SafetyMeshbll.GetListForCon(x => x.MeshName == item.MeshName && x.SuperiorId == item.SuperiorId && x.CreateUserOrgCode == user.OrganizeCode).ToList();
                            bool IsOk = list.Count() == 0 ? true : false;
                            if (!IsOk)
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,网格名称[" + item.MeshName + "]在[" + item.SuperiorName + "]下级存在重名！</br>", order);
                                error++;
                                continue;
                            }
                        }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,上级网格名称[" + Superior + "]不存在！</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        item.MeshRank = "1";
                        var list = SafetyMeshbll.GetListForCon(x => x.MeshName == item.MeshName && x.MeshRank == "1" && x.CreateUserOrgCode == user.OrganizeCode).ToList();
                        bool IsOk = list.Count() == 0 ? true : false;
                        if (!IsOk)
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,网格名称[" + item.MeshName + "]存在重名！</br>", order);
                            error++;
                            continue;
                        }
                    }
                    #endregion

                    #region 责任人
                    string DutyUser = dt.Rows[i][3].ToString();
                    if (!string.IsNullOrEmpty(DutyUser))
                    {
                        string DutyUserIdStr = "";
                        string DutyUserStr = "";
                        string[] userArr = DutyUser.Split(',');
                        foreach (string useritem in userArr)
                        {
                            if (!string.IsNullOrEmpty(useritem.Trim()))
                            {
                                string[] Arr = useritem.Split('/');
                                if (Arr.Count() == 1)
                                {
                                    var userList = new UserBLL().GetListForCon(x => x.RealName == useritem.Trim() && x.OrganizeId == orgid).ToList();
                                    if (userList.Count == 1)
                                    {
                                        var userEntity = userList.FirstOrDefault();
                                        DutyUserIdStr += userEntity.UserId + ",";
                                        DutyUserStr += useritem.Trim() + ",";
                                    }
                                    else if (userList.Count > 1)
                                    {
                                        falseMessage += string.Format(@"第{0}行导入失败,责任人[" + useritem + "]重名，请按格式[姓名/账号]重新填写！</br>", order);
                                        error++;
                                        flag = true;
                                        break;
                                    }
                                    else
                                    {
                                        falseMessage += string.Format(@"第{0}行导入失败,责任人[" + useritem + "]不存在！</br>", order);
                                        error++;
                                        flag = true;
                                        break;
                                    }
                                }
                                else if (Arr.Count() == 2)
                                {
                                    string name = Arr[0].Trim();
                                    string account = Arr[1].Trim();
                                    var userEntity2 = new UserBLL().GetListForCon(x => x.RealName == name && x.Account == account && x.OrganizeId == orgid).FirstOrDefault();
                                    if (userEntity2 != null)
                                    {
                                        DutyUserIdStr += userEntity2.UserId + ",";
                                        DutyUserStr += userEntity2.RealName + ",";
                                    }
                                    else
                                    {
                                        falseMessage += string.Format(@"第{0}行导入失败,责任人[" + Arr[0].Trim() + "]帐号[" + Arr[1].Trim() + "]不存在！</br>", order);
                                        error++;
                                        flag = true;
                                        break;
                                    }
                                }
                                else
                                {
                                    falseMessage += string.Format(@"第{0}行导入失败,责任人[" + useritem + "]输入格式有误！</br>", order);
                                    error++;
                                    flag = true;
                                    break;
                                }
                            }
                        }
                        item.DutyUserId = DutyUserIdStr.TrimEnd(',');
                        item.DutyUser = DutyUserStr.TrimEnd(',');
                    }
                    else
                    {
                        falseMessage += string.Format(@"第{0}行导入失败,责任人不能为空！</br>", order);
                        error++;
                        flag = true;
                        break;
                    }
                    if (flag)
                    {
                        continue;
                    }
                    #endregion

                    #region 联系电话
                    string DutyTel = dt.Rows[i][4].ToString();
                    if (!string.IsNullOrEmpty(DutyTel))
                    {
                        item.DutyTel = DutyTel;
                    }
                    #endregion

                    #region 排序
                    string SortCode = dt.Rows[i][5].ToString();
                    int tempSortCode;
                    if (!string.IsNullOrEmpty(SortCode))
                    {
                        if (int.TryParse(SortCode, out tempSortCode))
                            item.SortCode = tempSortCode;
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,网格排序必须为数字！</br>", order);
                            error++;
                            continue;
                        }
                    }
                    #endregion

                    #region 工作职责
                    string WorkJob = dt.Rows[i][6].ToString();
                    if (!string.IsNullOrEmpty(WorkJob))
                    {
                        item.WorkJob = WorkJob;
                    }
                    #endregion
                    try
                    {
                        SafetyMeshbll.SaveForm("", item);
                    }
                    catch
                    {
                        error++;
                    }
                }
                count = dt.Rows.Count - 1;
                message = "共有" + count + "条记录,成功导入" + (count - error) + "条，失败" + error + "条";
                message += "</br>" + falseMessage;
            }
            return message;
        }
        #endregion
    }
}
