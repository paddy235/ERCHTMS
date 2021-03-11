using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Code;
using System.Web;
using System;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.BaseManage;
using Aspose.Words;
using System.IO;
using System.Drawing;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Busines.AuthorizeManage;

namespace ERCHTMS.Web.Areas.HiddenTroubleManage.Controllers
{
    /// <summary>
    /// 描 述：矩阵安全检查计划
    /// </summary>
    public class MatrixsafecheckController : MvcControllerBase
    {
        private MatrixsafecheckBLL matrixsafecheckbll = new MatrixsafecheckBLL();
        private MatrixcontentBLL matrixcontentbll = new MatrixcontentBLL();
        private MatrixdeptBLL matrixdeptbll = new MatrixdeptBLL();

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
        /// 设置页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SetForm()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeptForm()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ContentForm()
        {
            return View();
        }

        /// <summary>
        /// 代办页面 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ActionForm()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CheckForm()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Matiximport()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取权限
        /// </summary>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public string GetDataAuthority()
        {
            AuthorizeBLL authBLL = new AuthorizeBLL();
            string result = authBLL.GetDataAuthority(OperatorProvider.Provider.Current(), Request.Cookies["currentmoduleId"].Value, "");
            return result;
        }

        /// <summary>
        /// 获取日历数据
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetCanlendarListJson(string queryJson)
        {
            var data = matrixsafecheckbll.GetCanlendarListJson(queryJson);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var data = matrixsafecheckbll.GetPageListJson(pagination, queryJson);
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
        /// 获取已有的年份集合
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetYearList()
        {
            var data = matrixsafecheckbll.GetInfoBySql("select to_char(CHECKTIME,'yyyy') yealname from bis_matrixsafecheck group by to_char(CHECKTIME,'yyyy') order by to_char(CHECKTIME,'yyyy')  ");
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = matrixsafecheckbll.GetList(queryJson);
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
            var data = matrixsafecheckbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 更新矩阵关联id 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult SetFormJson(string keyValue,string recid)
        {
            var data = matrixsafecheckbll.SetFormJson(keyValue, recid);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取检查内容
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetContentPageJson(string queryJson)
        {
            var data = matrixsafecheckbll.GetContentPageJson(queryJson);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取检查部门
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDeptPageJson(string queryJson)
        {
            var data = matrixsafecheckbll.GetDeptPageJson(queryJson);
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
            matrixsafecheckbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, MatrixsafecheckEntity entity)
        {
            entity.ISOVER = 0;
            
            DataTable dt = new DataTable();
            if (keyValue == "" || keyValue == null)
            {
                dt = matrixsafecheckbll.GetInfoBySql("select id from bis_matrixsafecheck where checktime  = to_date('" + entity.CHECKTIME + "', 'yyyy-MM-dd HH24:mi:ss')  ");
            }
            else
            {
                dt = matrixsafecheckbll.GetInfoBySql("select id from bis_matrixsafecheck where checktime  =  to_date('" + entity.CHECKTIME + "', 'yyyy-MM-dd HH24:mi:ss')  and ID <> '" + keyValue + "' ");
            }
            if (dt.Rows.Count == 0)
            {
                matrixsafecheckbll.SaveForm(keyValue, entity);
                return Success("操作成功。");
            }
            else
            {
                return Error("已存在相同时间的数据。");

            }
            

            
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
        public ActionResult SubmitForm(string keyValue, MatrixsafecheckEntity entity)
        {
            entity.ISOVER = 1;
            
            DataTable dt = new DataTable();
            if (keyValue == "" || keyValue == null)
            {
                dt = matrixsafecheckbll.GetInfoBySql("select id from bis_matrixsafecheck where checktime  = to_date('" + entity.CHECKTIME + "', 'yyyy-MM-dd HH24:mi:ss')  ");
            }
            else
            {
                dt = matrixsafecheckbll.GetInfoBySql("select id from bis_matrixsafecheck where checktime  =  to_date('" + entity.CHECKTIME + "', 'yyyy-MM-dd HH24:mi:ss')  and ID <> '" + keyValue + "' ");
            }
            if (dt.Rows.Count == 0)
            {
                matrixsafecheckbll.SaveForm(keyValue, entity);
                return Success("操作成功。");
            }
            else
            {
                return Error("已存在相同时间的数据。");

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveSetForm(MatrixEntity entity)
        {
            var user = OperatorProvider.Provider.Current();
            string isrole = "1";
            if (user.RoleName.Contains("厂级"))
            {
                isrole = "0";
            }

            if (!string.IsNullOrEmpty(entity.arrcontent))
            {
                var arrcontentlist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MatrixcontentEntity>>(entity.arrcontent);
                foreach (MatrixcontentEntity arrcontentinfo in arrcontentlist)
                {
                    MatrixcontentEntity en = matrixcontentbll.GetEntity(arrcontentinfo.ID);
                    if (en != null) //执行update
                    {
                        en.CONTENT = arrcontentinfo.CONTENT;
                        en.CODE = arrcontentinfo.CODE;
                        en.ISROLE = isrole;
                        matrixcontentbll.SaveForm(en.ID, en);
                    }
                    else // 新增
                    {
                        arrcontentinfo.ISROLE = isrole;
                        matrixcontentbll.SaveForm("", arrcontentinfo);
                    }
                }
               
            }

            if (!string.IsNullOrEmpty(entity.arrdept))
            {
                var arrdeptlist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MatrixdeptEntity>>(entity.arrdept);
                foreach (MatrixdeptEntity arrcondeptinfo in arrdeptlist)
                {
                    MatrixdeptEntity en = matrixdeptbll.GetEntity(arrcondeptinfo.ID);
                    if (en != null) //执行update
                    {
                        en.DEPTNAME = arrcondeptinfo.DEPTNAME;
                        en.DEPT = arrcondeptinfo.DEPT;
                        en.DEPTCODE = arrcondeptinfo.DEPTCODE;
                        en.CODE = arrcondeptinfo.CODE;
                        en.ISROLE = isrole;
                        matrixdeptbll.SaveForm(en.ID, en);
                    }
                    else // 新增
                    {
                        arrcondeptinfo.ISROLE = isrole;
                        matrixdeptbll.SaveForm("", arrcondeptinfo);
                    }
                }
            }

            if (!string.IsNullOrEmpty(entity.delcontent))
            {
                var arr = entity.delcontent.Split(',');

                foreach (string ain in arr)
                {
                    matrixcontentbll.RemoveForm(ain);
                }
            }

            if (!string.IsNullOrEmpty(entity.deldept))
            {
                var arr = entity.deldept.Split(',');

                foreach (string ain in arr)
                {
                    matrixdeptbll.RemoveForm(ain);
                }
            }

            return Success("操作成功。");
        }


        /// <summary>
        /// 导入j矩阵检查
        /// </summary>
        /// <param name="keyvalue"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportQuestion()
        {
            var user = OperatorProvider.Provider.Current();
            if (OperatorProvider.Provider.Current().IsSystem)
            {
                return "超级管理员无此操作权限";
            }
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

                string where = "";
                if (user.RoleName.Contains("厂级"))
                {
                    where += "  a.ISROLE = '0' and a.CREATEUSERDEPTCODE ='" + user.DeptCode + "'";
                }
                else
                {
                    where += "  a.ISROLE = '1' and a.CREATEUSERDEPTCODE ='" + user.DeptCode + "'";
                }
                DataTable dtcontent = matrixsafecheckbll.GetInfoBySql("select ID, CODE,CONTENT from BIS_MATRIXCONTENT a where   " + where);
                if (dtcontent.Rows.Count == 0)
                {
                    return "请先设置检查内容";
                }
                DataTable dtdept = matrixsafecheckbll.GetInfoBySql("select ID, code,dept,deptname,deptcode from BIS_MATRIXDEPT a where   " + where);
                if (dtdept.Rows.Count == 0)
                {
                    return "请先设置检查部门";
                }
                DataTable dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn + 1, false);
                dt.Rows.RemoveAt(0);
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    //检查日期
                    string checktime = dt.Rows[i][0].ToString();
                    //检查内容
                    string checkcontent = dt.Rows[i][1].ToString();
                    //检查部门
                    string checkdept = dt.Rows[i][2].ToString();
                    //检查人员
                    string checkuser = dt.Rows[i][3].ToString();

                    //---****值存在空验证*****--

                    if (string.IsNullOrEmpty(checktime) && string.IsNullOrEmpty(checkcontent) && string.IsNullOrEmpty(checkdept) && string.IsNullOrEmpty(checkuser))
                    {
                        continue;
                    }
                    if (string.IsNullOrEmpty(checktime) || string.IsNullOrEmpty(checkcontent) || string.IsNullOrEmpty(checkdept))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行值存在空,未能导入.";
                        error++;
                        continue;
                    }

                    

                    MatrixsafecheckEntity entir = new MatrixsafecheckEntity();
                    entir.ISOVER = 1;


                    // 检查时间
                    try
                    {
                        if (!string.IsNullOrEmpty(checktime))
                        {
                            // 检查时间
                            DataTable dtcount = matrixsafecheckbll.GetInfoBySql("select id from bis_matrixsafecheck where checktime  = to_date('" + DateTime.Parse(checktime).ToString("yyyy-MM-dd") + "', 'yyyy-MM-dd HH24:mi:ss')  ");

                            if (dtcount.Rows.Count > 0)
                            {
                                falseMessage += "</br>" + "第" + (i + 2) + "行检查时间已存在,未能导入.";
                                error++;
                                continue;
                            }
                            else
                            {
                                entir.CHECKTIME = DateTime.Parse(DateTime.Parse(checktime).ToString("yyyy-MM-dd"));
                            }

                            
                        }

                    }
                    catch
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行检查时间有误,未能导入.";
                        error++;
                        continue;
                    }

                    // 检查内容
                    entir.CONTENT = checkcontent;
                    if (!string.IsNullOrEmpty(checkcontent))
                    {
                        var conarr = checkcontent.Split(',');
                        int contentresult = 0;
                        
                        foreach (string continfo in conarr)
                        {
                            foreach (DataRow condr in dtcontent.Rows)
                            {
                                if (continfo == condr["CONTENT"].ToString())
                                {
                                    if (entir.CONTENTID == "" || entir.CONTENTID == null)
                                    {
                                        
                                        entir.CONTENTID = condr["ID"].ToString();
                                        entir.CONTENTNUM = condr["CODE"].ToString();
                                    }
                                    else
                                    {
                                        entir.CONTENTID += "," + condr["ID"].ToString();
                                        entir.CONTENTNUM += "," + condr["CODE"].ToString();
                                    }
                                    contentresult++;
                                    break;
                                    
                                }
                            }
                        }
                        if (contentresult != conarr.Length)
                        {
                            falseMessage += "</br>" + "第" + (i + 2) + "行检查内容有误,未能导入.";
                            error++;
                            continue;
                        }
                    }

                    // 检查部门
                    entir.CHECKDEPTNAME = checkdept;
                    if (!string.IsNullOrEmpty(checkdept))
                    {
                        var deptarr = checkdept.Split(',');
                        int checkdeptresult = 0;
                        
                        foreach (string deptinfo in deptarr)
                        {
                            foreach (DataRow deptdr in dtdept.Rows)
                            {
                                if (deptinfo == deptdr["DEPTNAME"].ToString())
                                {
                                    if (entir.CHECKDEPTSEL == "" || entir.CHECKDEPTSEL == null)
                                    {
                                        
                                        entir.CHECKDEPTSEL = deptdr["ID"].ToString();
                                        entir.CHECKDEPT = deptdr["DEPT"].ToString();
                                        entir.CHECKDEPTCODE = deptdr["DEPTCODE"].ToString();
                                        entir.CHECKDEPTNUM = deptdr["CODE"].ToString();

                                    }
                                    else
                                    {
                                        entir.CHECKDEPTSEL += "," + deptdr["ID"].ToString();
                                        entir.CHECKDEPT += "," + deptdr["DEPT"].ToString();
                                        entir.CHECKDEPTCODE += "," + deptdr["DEPTCODE"].ToString();
                                        entir.CHECKDEPTNUM += "," + deptdr["CODE"].ToString();

                                    }
                                    checkdeptresult++;
                                    break;
                                    
                                }
                            }
                        }
                        if (checkdeptresult != deptarr.Length)
                        {
                            falseMessage += "</br>" + "第" + (i + 2) + "行检查部门有误,未能导入.";
                            error++;
                            continue;
                        }
                    }

                    // 检查人员
                    entir.CHECKUSERNAME = checkuser;
                    if (!string.IsNullOrEmpty(checkuser))
                    {
                        var userarr = checkuser.Split(',');
                        int checkuserresult = 0;
                        foreach (string userinfo in userarr)
                        {
                            DataTable userdt = matrixsafecheckbll.GetInfoBySql(" SELECT USERID,ACCOUNT,DEPARTMENTCODE,REALNAME FROM BASE_USER WHERE REALNAME = '"+ userinfo + "' ");

                            if (userdt.Rows.Count == 0)
                            {
                                break;
                            }
                            else
                            {
                                if (entir.CHECKUSER == "" || entir.CHECKUSER == null)
                                {

                                    entir.CHECKUSER = userdt.Rows[0]["USERID"].ToString();
                                    entir.CHECKUSERCODE = userdt.Rows[0]["ACCOUNT"].ToString();
                                    entir.CHECKUSERDEPT = userdt.Rows[0]["DEPARTMENTCODE"].ToString();

                                }
                                else
                                {
                                    entir.CHECKUSER += "," + userdt.Rows[0]["USERID"].ToString();
                                    entir.CHECKUSERCODE += "," + userdt.Rows[0]["ACCOUNT"].ToString();
                                    entir.CHECKUSERDEPT = userdt.Rows[0]["DEPARTMENTCODE"].ToString();

                                }
                                checkuserresult++;
                            }

                        }


                        if (checkuserresult != userarr.Length)
                        {
                            falseMessage += "</br>" + "第" + (i + 2) + "行检查人员有误,未能导入.";
                            error++;
                            continue;
                        }
                    }

                    try
                    {
                        entir.ID = Guid.NewGuid().ToString();
                        matrixsafecheckbll.SaveForm(entir.ID, entir);
                    }
                    catch
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行保存失败,未能导入.";
                        error++;
                        continue;
                    }

                }


                count = dt.Rows.Count - 1;
                message = "共有" + count + "条记录,成功导入" + (count - error) + "条，失败" + error + "条";
                message += "</br>" + falseMessage;
            }
            return message;
        }

        public ActionResult SavePic(string qystr)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string guid = Guid.NewGuid().ToString();
            try
            {
                //
                var fileurl = string.Empty;
                DataItemDetailBLL dd = new DataItemDetailBLL();
                string path = Server.MapPath("~") + "\\Resource\\" + "Temp";
                byte[] arr2 = Convert.FromBase64String(qystr);
                using (MemoryStream ms2 = new MemoryStream(arr2))
                {//将png格式图片存储为jpg格式文件
                    System.Drawing.Bitmap bmp2 = new System.Drawing.Bitmap(ms2);
                    //bmp2 = KiResizelmage(bmp2, 600, 450);
                    string fileName = Guid.NewGuid().ToString() + ".png";
                    bmp2.Save(path + "\\" + fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    fileurl = "\\Resource\\" + "Temp" + "\\" + fileName;
                }
                //img = Server.MapPath("~" + fileurl);
                matrixsafecheckbll.ExecuteBySql("insert into bis_matrixsafecheck_file(id, contentbase) values ('" + guid + "', '" + fileurl + "')");

            }
            catch (Exception ex)
            {

               
            }


            

            return ToJsonResult(guid);
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="keyvalue"></param>
        public void ExportAuditTotal(string qystr)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            HttpResponse resp = System.Web.HttpContext.Current.Response;
            //报告对象
            string fileName = Server.MapPath("~/Resource/ExcelTemplate/matrixcheck/矩阵式安全检查表.docx");
            Aspose.Words.Document doc = new Aspose.Words.Document(fileName);
            DocumentBuilder builder = new DocumentBuilder(doc);
            builder.MoveToBookmark("matrixhtml");

            DataTable dt = matrixsafecheckbll.GetInfoBySql("select contentbase from bis_matrixsafecheck_file where id = '" + qystr + "'");
            string basere = "";
            if (dt.Rows.Count > 0)
            {
                basere = dt.Rows[0]["CONTENTBASE"].ToString();


            }

            string html = "<img width='983' height='552' src='"+ Server.MapPath("~" + basere) + "' />";
            builder.InsertHtml(html);
            //Aspose.Words.DocumentBuilder db = new Aspose.Words.DocumentBuilder(doc);
            //db.MoveToMergeField("gzpic");
            
            //byte[] bytes = Convert.FromBase64String(cs);

            //db.InsertImage(@"D:\cs.png",700,500);

            doc.Save(resp, Server.UrlEncode("矩阵式安全检查表" + DateTime.Now.ToString("yyyyMMddHHmm") + ".doc"), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
        }

        #endregion
    }
}


