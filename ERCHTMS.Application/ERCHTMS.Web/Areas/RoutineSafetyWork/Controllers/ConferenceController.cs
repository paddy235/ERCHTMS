using ERCHTMS.Entity.RoutineSafetyWork;
using ERCHTMS.Busines.RoutineSafetyWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using System.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System;
using System.Web;
using ERCHTMS.Busines.EquipmentManage;
using System.IO;

namespace ERCHTMS.Web.Areas.RoutineSafetyWork.Controllers
{
    /// <summary>
    /// 描 述：安全会议
    /// </summary>
    public class ConferenceController : MvcControllerBase
    {
        private ConferenceBLL conferencebll = new ConferenceBLL();
        private ConferenceUserBLL conferenceuserbll = new ConferenceUserBLL();
        private SpecialEquipmentBLL specialequipmentbll = new SpecialEquipmentBLL();

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
            pagination.p_kid = "ID";
            pagination.p_fields = "createuserid,ConferenceName,Compere,COMPEREID,CompereDept,ConferenceTime,Locale,ConferencePerson,IsSend,UserId";
            pagination.p_tablename = "BIS_Conference t";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string role = user.RoleName;
            var queryParam = queryJson.ToJObject();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                string sqlWhere = "";
                string deptCode = "";
                if (!queryParam["code"].IsEmpty() && !queryParam["isOrg"].IsEmpty())
                {
                    deptCode= queryParam["code"].ToString();
                }

                if (role.Contains("公司级用户") || role.Contains("厂级部门用户"))
                {
                    if (!string.IsNullOrEmpty(deptCode)&&deptCode!=user.DeptCode)
                    {
                        pagination.conditionJson = string.Format(" ((CREATEUSERORGCODE = '{0}' and CREATEUSERDEPTCODE like '{1}%' and IsSend='0'))", user.OrganizeCode, deptCode, user.UserId);
                    }
                    else {
                        pagination.conditionJson = string.Format(" ((CREATEUSERORGCODE = '{0}' and IsSend='0') or  createuserid='{1}')", user.OrganizeCode,user.UserId);
                    }
                    
                }
                else {
                    if (!queryParam["isOrg"].IsEmpty()&&queryParam["isOrg"].ToString() == "Organize")
                    {
                        pagination.conditionJson = string.Format("((instr(userid,'{0}')>0 and IsSend='0') or createuserid='{0}' or (CREATEUSERDEPTCODE like '{1}%' and IsSend='0')) ", user.UserId, user.DeptCode);
                    }
                    else {
                        if (!string.IsNullOrEmpty(deptCode))
                        {
                            if (deptCode == user.DeptCode)
                            {
                                pagination.conditionJson = string.Format("((instr(userid,'{0}')>0 and IsSend='0') or createuserid='{0}' or (CREATEUSERDEPTCODE like '{1}%' and IsSend='0')) ", user.UserId, deptCode);
                            }
                            else
                            {
                                pagination.conditionJson = string.Format("((instr(userid,'{0}')>0 and IsSend='0') or createuserid='{0}') and CREATEUSERDEPTCODE like '{1}%'", user.UserId, deptCode);
                            }
                        }
                        else
                        {
                            pagination.conditionJson = string.Format("((instr(userid,'{0}')>0 and IsSend='0') or createuserid='{0}' or (CREATEUSERDEPTCODE like '{1}%' and IsSend='0')) ", user.UserId, user.DeptCode);
                        }
                    }
                }
            }

            var watch = CommonHelper.TimerStart();
            var data = conferencebll.GetPageList(pagination, queryJson);
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
            var data = conferencebll.GetList(queryJson);
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
            var data = conferencebll.GetEntity(keyValue);
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
            conferencebll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, ConferenceEntity entity)
        {
            conferencebll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion

        #region 数据导出
        /// <summary>
        /// 导出用户列表
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "导出安全会议数据")]
        public ActionResult Export(string queryJson)
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "ID";
            pagination.p_fields = "ConferenceName,Locale,conferencetime,comperedept,compere,conferenceperson";
            pagination.p_tablename = "BIS_Conference t";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
                pagination.conditionJson = where + string.Format(" and ((instr(userid,'{0}')>0 and IsSend='0') or createuserid='{0}') ", user.UserId);
            }

            var watch = CommonHelper.TimerStart();
            var data = conferencebll.GetPageList(pagination, queryJson);

            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "安全会议";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "安全会议.xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity = listColumnEntity;
            ColumnEntity columnentity = new ColumnEntity();
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "conferencename", ExcelColumn = "会议地点", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "locale", ExcelColumn = "地点", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "conferencetime", ExcelColumn = "会议时间", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "comperedept", ExcelColumn = "召开部门", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "compere", ExcelColumn = "主持人", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "conferenceperson", ExcelColumn = "会议应到人数", Alignment = "center" });
            //调用导出方法
            ExcelHelper.ExcelDownload(data, excelconfig);

            return Success("导出成功。");
        }

        /// <summary>
        /// 导出用户列表
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "导出安全会议签到表数据")]
        public ActionResult ExportQD(string keyValue)
        {
            //获取会议基本信息
            ConferenceEntity cr = conferencebll.GetEntity(keyValue);
            ////获取会议签到人员信息
            //List<Object> data = conferenceuserbll.GetSignTable(keyValue);
            ////签到
            //List<Object> dataQD = data[0] as List<Object>;
            ////未签到
            //List<Object> dataWQD = data[1] as List<Object>;
            ////请假
            //List<Object> dataQ = data[2] as List<Object>;

            string path = Request.PhysicalApplicationPath;
            int index = 0;
            string fileName = HttpUtility.UrlDecode("安全会议签到") + DateTime.Now.ToString("yyyyMMddHHmm") + ".xls";  //定义导出文件名，一定记得编码
            try
            {
                Aspose.Cells.Workbook wk = new Aspose.Cells.Workbook();
                wk.Open(Server.MapPath("~/Resource/ExcelTemplate/安全会议签到表模板.xlsx")); //加载模板
                Aspose.Cells.Worksheet sheet = wk.Worksheets[0];
                Aspose.Cells.Style style = wk.Styles[wk.Styles.Add()];
                style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;//文字居中
                style.IsTextWrapped = true;//自动换行
                style.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                style.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                style.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                style.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                string sql = string.Format(" select t.USERNAME,o.DEPTNAME,t.PHOTOURL,t.issign,t.REVIEWSTATE from BIS_ConferenceUser t left join v_userinfo o on t.userid=o.userid where t.ConferenceID='{0}'", keyValue);
                DataTable dt = specialequipmentbll.SelectData(sql);
                //参会人员集合
                var listC = dt;
                //签到人员集合
                var SdUser = listC.Select(" issign='0'");
                //未签到人员集合
                var YdUser = listC.Select(" issign<>'0' and REVIEWSTATE<>'2'");
                //请假人数
                var QUser = listC.Select(" REVIEWSTATE='2'");
                sheet.Cells[1, 2].PutValue(cr.ConferenceName);//会议名称
                sheet.Cells[1, 4].PutValue(cr.Locale);//会议地点
                sheet.Cells[2, 2].PutValue(cr.CompereDept);//召开部门   
                sheet.Cells[2, 4].PutValue(cr.Compere);//主持人
                sheet.Cells[3, 2].PutValue(cr.Content);//主要议题
                if (SdUser.Length == 0)
                {
                    sheet.Cells[5, 1].PutValue("无");
                }
                else
                {
                    sheet.Cells[5 + SdUser.Length, 0].PutValue("已签到人数");
                    sheet.Cells.Merge(5, 0, SdUser.Length, 1);
                    foreach (DataRow item in SdUser)
                    {
                        index++;
                        sheet.Cells[4 + index, 1].PutValue(index);
                        sheet.Cells[4 + index, 2].PutValue(item[0].ToString());//参会人员
                        sheet.Cells[4 + index, 4].PutValue(item[1].ToString());//所属部门
                        if (!string.IsNullOrEmpty(item[2].ToString())) {
                            string[] u = item[2].ToString().Split('/');
                            string url = path + "/" + u[1] + "/" + u[2] + "/" + u[3] + "/" + u[4] + "/" + u[5];//签名
                            FileStream fs = new FileStream(url, FileMode.Open);
                            int imgIndex = sheet.Pictures.Add(4 + index, 3, fs, 120, 30);
                            fs.Close();
                            sheet.Cells.SetColumnWidth(5, 20);
                            sheet.Cells.SetRowHeight(4 + index, 44);
                            sheet.Pictures[imgIndex].Width = 139;
                            sheet.Pictures[imgIndex].Height = 50;
                            sheet.Pictures[imgIndex].Top = 3;
                            sheet.Pictures[imgIndex].Left = 6;
                        }
                    }

                }
                int num = SdUser.Length == 0 ? 1 : SdUser.Length;
                int s = SdUser.Length == 0 ? 1 : SdUser.Length;
                if (index > 0)
                {
                    s = 0;
                }

                if (YdUser.Length == 0)
                {
                    sheet.Cells[5 + num, 0].PutValue("未签到人数");
                    sheet.Cells[5 + num, 1].PutValue("无");
                }
                else
                {
                    sheet.Cells[5 + num, 0].PutValue("未签到人数");
                    sheet.Cells.Merge(5 + num, 0, YdUser.Length, 1);
                    foreach (DataRow item in YdUser)
                    {
                        index++;
                        sheet.Cells[4 + s + index, 1].PutValue(index);
                        sheet.Cells[4 + s + index, 2].PutValue(item[0].ToString());
                        sheet.Cells[4 + s + index, 4].PutValue(item[1].ToString());
                        sheet.Cells[4 + s + index, 3].PutValue("");

                    }

                }
                s = YdUser.Length == 0 ? 1 : YdUser.Length;
                int qj = s + SdUser.Length == 0 ? 1 : s + SdUser.Length;
                qj = SdUser.Length == 0 ? qj + 1 : qj;
                int qjNum = 0;
                if (QUser.Length > 0)
                {
                    sheet.Cells[5 + qj, 0].PutValue("请假人数");
                    sheet.Cells.Merge(5 + qj, 0, QUser.Length, 1);
                    foreach (DataRow dr in QUser)
                    {
                        sheet.Cells[5 + qj + qjNum, 1].PutValue(index++ + 1);
                        sheet.Cells[5 + qj + qjNum, 2].PutValue(dr[0].ToString());
                        sheet.Cells[5 + qj + qjNum, 4].PutValue(dr[1].ToString());
                        sheet.Cells[5 + qj + qjNum, 3].PutValue("");
                        qjNum++;
                    }
                }
                else
                {
                    qjNum += 1;
                    sheet.Cells[5 + qj, 0].PutValue("请假人数");
                    sheet.Cells[5 + qj, 1].PutValue("无");
                }
                Aspose.Cells.Range r = sheet.Cells.CreateRange(5, 0, qj + qjNum, 5);
                r.Style = style;
                HttpResponse resp = System.Web.HttpContext.Current.Response;
                wk.Save(HttpUtility.UrlEncode(fileName), Aspose.Cells.FileFormatType.Excel2003, Aspose.Cells.SaveType.OpenInExcel, resp);

                return ToJsonResult("0");
            }
            catch (Exception ex)
            {
                string env = Config.GetValue("Environment");
                string msg = env == "dev" ? ex.Message : "对不起,系统出错了";
                return ToJsonResult("" + msg + "");
            }
            
        }
        #endregion
    }
}
