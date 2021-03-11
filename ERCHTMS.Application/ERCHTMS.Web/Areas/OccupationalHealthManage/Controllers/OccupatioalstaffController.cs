using ERCHTMS.Entity.OccupationalHealthManage;
using ERCHTMS.Busines.OccupationalHealthManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System;
using ERCHTMS.Busines.PublicInfoManage;
using System.Data;
using ERCHTMS.Code;
using System.Web;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.SystemManage.ViewModel;

namespace ERCHTMS.Web.Areas.OccupationalHealthManage.Controllers
{
    /// <summary>
    /// 描 述：职业病人员表
    /// </summary>
    public class OccupatioalstaffController : MvcControllerBase
    {
        private OccupatioalstaffBLL occupatioalstaffbll = new OccupatioalstaffBLL();

        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {

            ViewBag.IsQx = "0";
            //当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            DataItemModel Depart = dataitemdetailbll.GetDataItemListByItemCode("'HealthDeptQx'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();
            if (Depart != null)
            {
                if (Depart.ItemValue.Contains(curUser.DeptId))
                {
                    ViewBag.IsQx = "1";
                }
            }
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

        /// <summary>
        /// 文件列表
        /// </summary>
        /// <returns></returns>
        public ActionResult FileList()
        {
            return View();
        }
        [HttpGet]
        public ActionResult OpenWord()
        {
            //PageOffice.PageOfficeCtrl pc = new PageOffice.PageOfficeCtrl();
            //pc.AddCustomToolButton("保存", "Save()", 1);
            //pc.SaveFilePage = "/Word/SaveDoc";
            //pc.ServerPage = Request.ApplicationPath + "/pageoffice/server.aspx";

            //pc.WebOpen(@"E:\项目文档\开发框架简介_V2.0_20180516.docx", PageOffice.OpenModeType.docReadOnly, "s");

            //ViewBag.EditorHtml = pc.GetHtmlCode("PageOfficeCtrl1");
            //return Redirect("../../Utility/DownloadFile?filePath=" + Server.UrlEncode("E:\\资料\\上传文件.txt"));
            return View();
        }
        
        public void GetWord(string fileUrl)
        {
            fileUrl = HttpUtility.UrlDecode(fileUrl);
            // 打开文档
            //string filePath = Server.MapPath(Request.ApplicationPath + "/Resource/EmergencyPlan/" + fileUrl);
            //用于去掉前面的~号
            //fileUrl = fileUrl.Substring(1);
            string filePath = Server.MapPath(fileUrl);
            //string filePath = Request.ApplicationPath + "/Resource/EmergencyPlan/" + fileUrl;
            string[] files = filePath.Split('.');
            if (files[files.Length - 1] == "pdf")
            {
                PageOffice.PDFCtrl PdfCtrl1 = new PageOffice.PDFCtrl();
                PdfCtrl1.ServerPage = Request.ApplicationPath + "/pageoffice/server.aspx";
                PdfCtrl1.AddCustomToolButton("打印", "Print()", 6);
                //PdfCtrl1.AddCustomToolButton("-", "", 0);
                //PdfCtrl1.AddCustomToolButton("显示/隐藏书签", "SwitchBKMK()", 0);
                //PdfCtrl1.AddCustomToolButton("实际大小", "SetPageReal()", 16);
                //PdfCtrl1.AddCustomToolButton("适合页面", "SetPageFit()", 17);
                //PdfCtrl1.AddCustomToolButton("适合宽度", "SetPageWidth()", 18);
                //PdfCtrl1.AddCustomToolButton("-", "", 0);
                //PdfCtrl1.AddCustomToolButton("首页", "FirstPage()", 0);
                //PdfCtrl1.AddCustomToolButton("上一页", "PreviousPage()", 9);
                //PdfCtrl1.AddCustomToolButton("下一页", "NextPage()", 10);
                //PdfCtrl1.AddCustomToolButton("尾页", "LastPage()", 11);
                //PdfCtrl1.AddCustomToolButton("-", "", 0);
                PdfCtrl1.WebOpen(filePath);
                Response.Write(PdfCtrl1.GetHtmlCode("PdfCtrl1"));
                Response.End();
            }
            else if (files[files.Length - 1] == "xls" || files[files.Length - 1] == "xlsx" || files[files.Length - 1] == "doc" || files[files.Length - 1] == "docx" || files[files.Length - 1] == "ppt" || files[files.Length - 1] == "pptx")
            {
                PageOffice.PageOfficeCtrl PageOfficeCtrl1 = new PageOffice.PageOfficeCtrl();
                PageOfficeCtrl1.ServerPage = Request.ApplicationPath + "/pageoffice/server.aspx";
                // 设置保存文件页面
                //PageOfficeCtrl1.SaveFilePage = Server.MapPath(Request.ApplicationPath + "/DayCheckManage/EmergencyPlan/SaveFile?filrUrl=EmergencyPlan&keyValue=" + keyValue);
                //PageOfficeCtrl1.SaveFilePage = Request.ApplicationPath + "/DayCheckManage/EmergencyPlan/SaveFile?filrUrl=EmergencyPlan&keyValue=" + keyValue;
                //添加自定义按钮
                //PageOfficeCtrl1.Caption = "文档编辑";

                PageOfficeCtrl1.Titlebar = false; //隐藏标题栏
                PageOfficeCtrl1.Menubar = false; //隐藏菜单栏
                PageOfficeCtrl1.CustomToolbar = false; //隐藏自定义工具栏
                PageOfficeCtrl1.OfficeToolbars = false; //隐藏Office工具栏
                PageOfficeCtrl1.Theme = PageOffice.ThemeType.CustomStyle;
                if (files[files.Length - 1] == "doc" || files[files.Length - 1] == "docx")//判断是否是word
                {
                    PageOfficeCtrl1.WebOpen(filePath, PageOffice.OpenModeType.docReadOnly, "fwz");
                }
                else if (files[files.Length - 1] == "xls" || files[files.Length - 1] == "xlsx")
                {
                    PageOfficeCtrl1.WebOpen(filePath, PageOffice.OpenModeType.xlsReadOnly, "fwz");
                }
                else if (files[files.Length - 1] == "ppt" || files[files.Length - 1] == "pptx")
                {
                    PageOfficeCtrl1.WebOpen(filePath, PageOffice.OpenModeType.pptReadOnly, "fwz");//没有的类型用word打开
                }
                else
                {
                    PageOfficeCtrl1.WebOpen(filePath, PageOffice.OpenModeType.docReadOnly, "fwz");//没有的类型用word打开
                }
                Response.Write(PageOfficeCtrl1.GetHtmlCode("PageOfficeCtrl1"));
                Response.End();
            }
            
        }

        #region 获取数据
        public ActionResult GetFiles(string keyValue)
        {
            FileInfoBLL file = new FileInfoBLL();
            var dt = file.GetFiles(keyValue);//先获取上传的附件
            return ToJsonResult(dt);
        }

        //判断文件是否存在
        [HttpPost]
        public bool IsUrl(string fileUrl)
        {
            fileUrl = fileUrl.Substring(1);
            string filePath = Server.MapPath(Request.ApplicationPath + fileUrl);
            //判断是否有文件
            bool flag = System.IO.File.Exists(filePath);
            return flag;
        }

        /// <summary>
        /// 导出到Excel
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult Excel(string queryJson)
        {
            string whereSql = "";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                whereSql = "";
            }
            else
            {
                string where = new ERCHTMS.Busines.AuthorizeManage.AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
                whereSql += " and " + where;
            }

            DataTable dt = occupatioalstaffbll.GetTable(queryJson, whereSql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i][0] = (i + 1).ToString();
                dt.Rows[i]["INSPECTIONTIME"] = Convert.ToDateTime(dt.Rows[i]["INSPECTIONTIME"]).ToString("yyyy-MM-dd");
            }
            string FileUrl = @"\Resource\ExcelTemplate\职业健康体检列表_导出模板.xlsx";



            AsposeExcelHelper.ExecuteResult(dt, FileUrl, "职业健康体检列表", "职业健康体检列表");

            return Success("导出成功。");
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
            pagination.p_kid = "OCCID";
            pagination.p_fields = "MECHANISMNAME,INSPECTIONTIME,INSPECTIONNUM,PATIENTNUM,ISANNEX,FILENUM,unusualnum";//注：此处要替换成需要查询的列
            pagination.p_tablename = "V_OCCUPATIOALSTAFF";
            pagination.conditionJson = "1=1";
            pagination.sidx = "INSPECTIONTIME";

            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                string where = new ERCHTMS.Busines.AuthorizeManage.AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
                pagination.conditionJson += " and " + where;
            }

            var data = occupatioalstaffbll.GetPageListByProc(pagination, queryJson);
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
            var data = occupatioalstaffbll.GetEntity(keyValue);
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
            OccupationalstaffdetailBLL occupationalstaffdetailbll = new OccupationalstaffdetailBLL();
            occupationalstaffdetailbll.Delete(keyValue, 0);//先删除其下详情数据
            occupationalstaffdetailbll.Delete(keyValue, 1);
            occupatioalstaffbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, string strentity, string Users, string UserIds, bool isNew)
        {
            OccupatioalstaffEntity userEntity = strentity.ToObject<OccupatioalstaffEntity>();

            #region 处理体检员工表
            //先查询出患病的用户
            OccupationalstaffdetailBLL occupationalstaffdetailbll = new OccupationalstaffdetailBLL();
            IEnumerable<OccupationalstaffdetailEntity> odlist = occupationalstaffdetailbll.GetList(keyValue, 1);//查询出所有填写的患病记录
            IEnumerable<OccupationalstaffdetailEntity> yclist = occupationalstaffdetailbll.GetList(keyValue, 2);//查询出所有填写的异常记录
            occupationalstaffdetailbll.UpdateTime((DateTime)(userEntity.InspectionTime), keyValue);//先将所有的病人体检时间纠正过来
            if (!isNew) //修改模式
            {
                occupationalstaffdetailbll.Delete(keyValue, 0);//先把健康人员记录删除再重新添加
            }
            //获取到所有体检用户
            string[] UserGroup = UserIds.Split(',');
            string[] UsersGroup = Users.Split(',');
            if (odlist != null)
            {
                for (int i = 0; i < UserGroup.Length; i++)
                {
                    if (!IsTrue(odlist, yclist, UserGroup[i]))
                    {
                        //判断为健康员工
                        OccupationalstaffdetailEntity occ = new OccupationalstaffdetailEntity();
                        occ.UserId = UserGroup[i];
                        occ.UserName = UsersGroup[i];
                        occ.UserNamePinYin = Str.PinYin(UsersGroup[i]);
                        occ.Note = "";
                        occ.Issick = 0;
                        occ.InspectionTime = userEntity.InspectionTime;
                        occ.OccId = keyValue;
                        occ.SickType = "无";
                        occupationalstaffdetailbll.SaveForm("", occ);
                    }
                }
            }
            #endregion
            FileInfoBLL file = new FileInfoBLL();
            DataTable dt = file.GetFiles(keyValue);//先获取上传的附件
            if (dt != null && dt.Rows.Count > 0)
            {
                userEntity.IsAnnex = 1;//有附件
            }
            else
            {
                userEntity.IsAnnex = 0;//没有附件
            }
            userEntity.OccId = keyValue;
            userEntity.InspectionNum = UserGroup.Length;
            userEntity.PatientNum = odlist.Count();
            userEntity.UnusualNum = yclist.Count();

            occupatioalstaffbll.SaveForm(isNew, userEntity);
            return Success("操作成功。");
        }
        #endregion

        /// <summary>
        /// 判断生病员工中是否有这个员工
        /// </summary>
        /// <param name="odlist"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool IsTrue(IEnumerable<OccupationalstaffdetailEntity> odlist, IEnumerable<OccupationalstaffdetailEntity> yclist, string value)
        {
            foreach (OccupationalstaffdetailEntity item in odlist)
            {
                if (item.UserId == value) //判断是否是生病员工
                {
                    return true;
                }
            }
            foreach (OccupationalstaffdetailEntity item in yclist)
            {
                if (item.UserId == value) //判断是否是异常员工
                {
                    return true;
                }
            }
            return false;
        }
    }
}
