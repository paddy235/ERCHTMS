using ERCHTMS.Entity.EquipmentManage;
using ERCHTMS.Busines.EquipmentManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using System.Data;
using System.Collections.Generic;
using BSFramework.Data;
using BSFramework.Util.Extension;
using BSFramework.Util.Offices;
using System;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.BaseManage;
using System.Linq;
using System.Web;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ERCHTMS.Busines.SystemManage;

namespace ERCHTMS.Web.Areas.EquipmentManage.Controllers
{
    /// <summary>
    /// 描 述：特种设备基本信息表
    /// </summary>
    public class SpecialEquipmentController : MvcControllerBase
    {
        private SpecialEquipmentBLL specialequipmentbll = new SpecialEquipmentBLL();
        private OutsouringengineerBLL outsouringengineerbll = new OutsouringengineerBLL();
        private ERCHTMS.Busines.PublicInfoManage.FileInfoBLL fileInfoBLL = new ERCHTMS.Busines.PublicInfoManage.FileInfoBLL();
        private DepartmentBLL departmentBLL = new DepartmentBLL();
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
        [HttpGet]
        public ActionResult IndexList()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Import()
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
        /// 统计页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Stat()
        {
            return View();
        }
        /// <summary>
        /// 选择设备
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult Select()
        {
            return View();
        }

        /// <summary>
        /// 选择设备
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult RemoteSelect()
        {
            return View();
        }

        


        /// <summary>
        /// 省级列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult SJIndex()
        {
            return View();
        }

        /// <summary>
        /// 省级统计页面
        /// </summary>
        /// <returns></returns>
        public ActionResult SJStat()
        {
            return View();
        }


        [HttpGet]
        public ActionResult SJIndexList()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CaseList()
        {
            return View();
        }

        /// <summary>
        /// 省级离场清单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SJLeaveList()
        {
            return View();
        }

        /// <summary>
        /// 离场表单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Leave()
        {
            return View();
        }

        /// <summary>
        /// 离场清单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LeaveList()
        {
            return View();
        }
        /// <summary>
        /// 检验日期
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Checkout()
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
            pagination.p_fields = @"EquipmentName,EquipmentName as normalequipmentname,EquipmentNo,Specifications,CertificateNo,CheckDate,NextCheckDate,district,districtid,districtcode,case when t.state=1 then '未启用'
when t.state=2 then '在用' when t.state=3 then '停用' when t.state=4 then '报废' when t.state=5 then '离厂' end as state,certificateid,decode(checkfileid,null,''),acceptance,ControlDeptCode,CreateUserId,affiliation,CheckFileID,ExamineUnit";
            pagination.p_tablename = "BIS_SPECIALEQUIPMENT t";

            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                pagination.conditionJson = string.Format(" CREATEUSERORGCODE in(select  encode from BASE_DEPARTMENT start with encode='{0}' connect by  prior  departmentid = parentid)", user.OrganizeCode);
            }

            //if (user.IsSystem)
            //{
            //    pagination.conditionJson = "1=1";
            //}
            //else
            //{
            //    string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
            //    pagination.conditionJson = where;
            //}

            var watch = CommonHelper.TimerStart();
            var data = specialequipmentbll.GetPageList(pagination, queryJson);
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
        /// 获取省级列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJsonForSJ(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "ID";
            pagination.p_fields = @"EquipmentName,EquipmentNo,Specifications,CertificateNo,NextCheckDate,t.district,t.districtid,districtcode,case when t.state=1 then '未启用'
when t.state=2 then '在用' when t.state=3 then '停用' when t.state=4 then '报废' when t.state=5 then '离厂' end as state,certificateid,checkfileid,acceptance,ControlDeptCode,t.CreateUserId,affiliation,b.organizeid createuserorgid";
            pagination.p_tablename = "BIS_SPECIALEQUIPMENT t left join base_department b on t.createuserorgcode=b.encode";
            pagination.sidx = "t.createdate";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                IEnumerable<DepartmentEntity> orgcodelist = new List<DepartmentEntity>();
                orgcodelist = departmentBLL.GetList().Where(t => t.DeptCode.Contains(user.NewDeptCode) && t.Nature == "厂级");
                pagination.conditionJson += "  (";
                foreach (DepartmentEntity item in orgcodelist)
                {
                    pagination.conditionJson += " createuserorgcode ='" + item.EnCode + "' or ";
                }
                pagination.conditionJson += " createuserorgcode = '" + user.OrganizeCode + "') ";
            }

            //if (user.IsSystem)
            //{
            //    pagination.conditionJson = "1=1";
            //}
            //else
            //{
            //    string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
            //    pagination.conditionJson = where;
            //}

            var watch = CommonHelper.TimerStart();
            var data = specialequipmentbll.GetPageList(pagination, queryJson);
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
            var data = specialequipmentbll.GetList(queryJson);
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
            var data = specialequipmentbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取设备编号
        /// </summary>
        /// <param name="EquipmentNo">设备类别</param>
        /// <param name="orgcode">机构编码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetEquipmentNo(string EquipmentNo, string orgcode)
        {
            return ToJsonResult(specialequipmentbll.GetEquipmentNo(EquipmentNo, orgcode));
        }
        /// <summary>
        /// 获取附件集合
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult GetFiles(string keyValue)
        {
            var data = fileInfoBLL.GetFiles(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取设备类别统计图和列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetEquipmentTypeStat(string queryJson)
        {
            DataTable dt = specialequipmentbll.GetEquipmentTypeStat(queryJson, null);
            return ToJsonResult(dt);
        }


        /// <summary>
        /// 获取设备运行故障统计图和列表
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult GetOperationFailureStat(string queryJson)
        {
            object obj = specialequipmentbll.GetOperationFailureStat(queryJson, null);
            return ToJsonResult(obj);
        }
        /// <summary>
        /// 获取设备隐患统计图和列表
        /// </summary>
        /// <param name="queryJson"></param>
        /// <param name="se"></param>
        /// <returns></returns>
        public ActionResult GetEquipmentHidStat(string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string sqlwhere = string.Empty;
            var queryParam = queryJson.ToJObject();
            if (!queryParam["StartTime"].IsEmpty())
            {
                string startTime = queryParam["StartTime"].ToString();
                string endTime = queryParam["EndTime"].ToString();
                if (queryParam["EndTime"].IsEmpty())
                {
                    endTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
                sqlwhere = string.Format(" and t2.checkdate between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", startTime, endTime);
            }
            string sql = string.Format(@"  select s.itemname,s.itemvalue,(select count(1) from v_basehiddeninfo t1 left join BIS_specialequipment t2 on t1.deviceid=t2.id where t2.id is not null and t2.createuserorgcode='{1}' and t2.affiliation='1'and t2.equipmenttype=s.itemvalue {0}) as OwnEquipment ,
 (select count(1) from v_basehiddeninfo t1 left join BIS_specialequipment t2 on t1.deviceid=t2.id where t2.id is not null and t2.createuserorgcode='{1}' and t2.affiliation='2' and t2.equipmenttype=s.itemvalue {0}) as ExternalEquipment,s.sortcode
 from (select a.itemname,a.itemvalue,a.sortcode from BASE_DATAITEMDETAIL a
 left join base_dataitem b on a.itemid = b.itemid  where b.itemcode ='EQUIPMENTTYPE' ) s 
 group by s.itemname,s.itemvalue,s.sortcode order by s.sortcode ", sqlwhere, user.OrganizeCode);
            DataTable dt = specialequipmentbll.SelectData(sql);
            return ToJsonResult(dt);
        }

        public string SaveEquipmentStat(string TableHtml)
        {
            string PID = Guid.NewGuid().ToString();
            try
            {
                if (System.IO.File.Exists(Server.MapPath("~/Resource/Temp/") + PID + ".txt"))
                {
                    System.IO.File.Delete(Server.MapPath("~/Resource/Temp/") + PID + ".txt");
                }
                System.IO.File.AppendAllText(Server.MapPath("~/Resource/Temp/") + PID + ".txt", TableHtml, System.Text.Encoding.UTF8);
            }
            catch (Exception)
            {

                return "0";
            }

            return PID;
        }
        public void ExportEquipmentStat(string PID, string filename)
        {
            System.IO.StreamReader sr = new System.IO.StreamReader(Server.MapPath("~/Resource/Temp/" + PID + ".txt"), System.Text.Encoding.UTF8);
            string res = sr.ReadToEnd();
            sr.Close();
            if (System.IO.File.Exists(Server.MapPath("~/Resource/Temp/") + PID + ".txt"))
            {
                System.IO.File.Delete(Server.MapPath("~/Resource/Temp/") + PID + ".txt");
            }

            System.Web.HttpContext.Current.Response.Charset = "utf-8";
            System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(Server.UrlDecode(filename), System.Text.Encoding.UTF8) + ".xls");
            System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            System.Web.HttpContext.Current.Response.ContentType = "application/ms-excel";
            System.Web.HttpContext.Current.Response.Write(Server.UrlDecode(res.ToString()));
            System.Web.HttpContext.Current.Response.End();
        }

        /// <summary> 
        /// 通过设备id获取特种设备列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSpecialEquipmentTable(string ids)
        {
            DataTable dt = specialequipmentbll.GetSpecialEquipmentTable(ids.Split(','));
            return Content(dt.ToJson());
        }
        #endregion


        #region 省级统计数据

        /// <summary>
        /// 获取省级设备类别列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetEquipmentTypeStatForSJ(string queryJson)
        {
            DataTable dt = specialequipmentbll.GetEquipmentTypeStatGridForSJ(queryJson);
            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                total = 1,
                page = 1,
                records = dt.Rows.Count,
                rows = dt
            });
        }

        /// <summary>
        /// 获取省级设备类别图形
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public string GetEquipmentTypeStatDataForSJ(string queryJson)
        {
            return specialequipmentbll.GetEquipmentTypeStatDataForSJ(queryJson);
        }


        /// <summary>
        /// 导出省级设备类别
        /// </summary>
        [HandlerMonitor(0, "导出数据")]
        public ActionResult ExportEquipmentTypeStatDataForSJ(string queryJson)
        {
            try
            {
                DataTable dt = specialequipmentbll.GetEquipmentTypeStatGridForSJ(queryJson);
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "不同类型特种设备数量统计";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "不同类型特种设备数量统计.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //需跟数据源列顺序保持一致
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "name", ExcelColumn = "电厂名称", Width = 20 });
                if (dt.Rows.Count > 0)
                {
                    for (int i = 2; i < dt.Columns.Count; i++)
                    {
                        excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = dt.Columns[i].ColumnName, ExcelColumn = dt.Rows[0][i].ToString().Split(',')[1].ToString(), Width = 20 });
                    }
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 2; j < dt.Columns.Count; j++)
                    {
                        dt.Rows[i][j] = dt.Rows[i][j].ToString().Split(',')[0].ToString();
                    }
                }
                //调用导出方法
                ExcelHelper.ExcelDownload(dt, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("导出成功。");
        }

        /// <summary>
        /// 获取省级隐患数量表格
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetEquipmentHidGridForSJ(string queryJson)
        {
            DataTable dt = specialequipmentbll.GetEquipmentHidGridForSJ(queryJson);
            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                total = 1,
                page = 1,
                records = dt.Rows.Count,
                rows = dt
            });

        }

        /// <summary>
        /// 获取省级隐患数量图形
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetEquipmentHidDataForSJ(string queryJson)
        {
            return specialequipmentbll.GetEquipmentHidDataForSJ(queryJson);
        }

        /// <summary>
        /// 导出省级隐患数量
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HandlerMonitor(0, "导出数据")]
        public ActionResult ExportEquipmentHidDataForSJ(string queryJson)
        {
            try
            {
                DataTable dt = specialequipmentbll.GetEquipmentHidGridForSJ(queryJson);
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "隐患数量统计";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "隐患数量统计.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //需跟数据源列顺序保持一致
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "typename", ExcelColumn = "电厂名称", Width = 30 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "num", ExcelColumn = "数量", Width = 30 });
                //调用导出方法
                ExcelHelper.ExcelDownload(dt, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("导出成功。");
        }

        /// <summary>
        /// 获取省级检查次数表格
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetEquipmentCheckGridForSJ(string queryJson)
        {
            DataTable dt = specialequipmentbll.GetEquipmentCheckGridForSJ(queryJson);
            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                total = 1,
                page = 1,
                records = dt.Rows.Count,
                rows = dt
            });
        }

        /// <summary>
        /// 获取省级检查次数图形
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetEquipmentCheckDataForSJ(string queryJson)
        {
            return specialequipmentbll.GetEquipmentCheckDataForSJ(queryJson);
        }

        /// <summary>
        /// 导出省级检查次数
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HandlerMonitor(0, "导出数据")]
        public ActionResult ExportEquipmentCheckDataForSJ(string queryJson)
        {
            try
            {
                DataTable dt = specialequipmentbll.GetEquipmentCheckGridForSJ(queryJson);
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "检查次数统计";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "检查次数统计.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //需跟数据源列顺序保持一致
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "typename", ExcelColumn = "电厂名称", Width = 30 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "num", ExcelColumn = "数量", Width = 30 });
                //调用导出方法
                ExcelHelper.ExcelDownload(dt, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("导出成功。");
        }

        /// <summary>
        /// 获取省级运行故障图形
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetEquipmentFailureDataForSJ(string queryJson)
        {
            return specialequipmentbll.GetEquipmentFailureDataForSJ(queryJson);
        }

        /// <summary>
        /// 获取省级运行故障列表
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetEquipmentFailureGridForSJ(string queryJson)
        {
            DataTable dt = specialequipmentbll.GetEquipmentFailureGridForSJ(queryJson);
            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                total = 1,
                page = 1,
                records = dt.Rows.Count,
                rows = dt
            });
        }

        /// <summary>
        /// 导出省级运行故障
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HandlerMonitor(0, "导出数据")]
        public ActionResult ExportEquipmentFailureDataForSJ(string queryJson)
        {
            try
            {
                DataTable dt = specialequipmentbll.GetEquipmentFailureGridForSJ(queryJson);
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "运行故障统计";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "运行故障统计.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //需跟数据源列顺序保持一致
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "typename", ExcelColumn = "电厂名称", Width = 30 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "num", ExcelColumn = "数量", Width = 30 });
                //调用导出方法
                ExcelHelper.ExcelDownload(dt, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("导出成功。");
        }

        /// <summary>
        /// 获取省级检查次数记录
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetSafetyCheckRecordForSJ(string queryJson)
        {
            DataTable dt = specialequipmentbll.GetSafetyCheckRecordForSJ(queryJson);
            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                total = 1,
                page = 1,
                records = dt.Rows.Count,
                rows = dt
            });
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
            specialequipmentbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, SpecialEquipmentEntity entity)
        {
            specialequipmentbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }

        /// <summary>
        /// 特种设备离场
        /// </summary>
        /// <param name="leaveTime">离场时间</param>
        /// <param name="equipmentId">设备Id,多个用逗号分隔</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "特种设备离场")]
        public ActionResult Leave(string leaveTime, [System.Web.Http.FromBody]string specialequipmentId, [System.Web.Http.FromBody]string DepartureReason)
        {
            try
            {
                if (specialequipmentbll.SetLeave(specialequipmentId, leaveTime, DepartureReason) > 0)
                {
                    return Success("操作成功。");
                }
                else
                {
                    return Error("操作不成功。");
                }

            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        /// <summary>
        /// 特种设备修改检验日期
        /// </summary>
        /// <param name="CheckDate">离场时间</param>
        /// <param name="equipmentId">设备Id,多个用逗号分隔</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "特种设备批量修改检验日期")]
        public ActionResult Checkout(string CheckDate, [System.Web.Http.FromBody]string specialequipmentId)
        {
            try
            {
                if (specialequipmentbll.SetCheck(specialequipmentId, CheckDate) > 0)
                {
                    return Success("操作成功。");
                }
                else
                {
                    return Error("操作不成功。");
                }

            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        #endregion

        #region 数据导出
        /// <summary>
        /// 导出用户列表
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "导出特种设备数据")]
        public ActionResult Export(string queryJson)
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "rownum idx";
            pagination.p_fields = @"EmployDept,equipmentregisterno,specifications,factoryno,employsite,location,
case when t.state=1 then '未启用'
when t.state=2 then '在用' when t.state=3 then '停用' when t.state=4 then '报废' when t.state=5 then '离厂' end as state ,
equipmentno,b.itemname equipmenttype,c.itemname equipmentkind,d.itemname equipmentbreed,
t.ReportNumber,
checkdate,
nextcheckdate,
t.ExamineVerdict,
examineappeardate,
reportexaminedate,
acceptstate";
            pagination.p_tablename = @"BIS_SPECIALEQUIPMENT t 
                                      left join base_dataitemdetail b
    on t.equipmenttype = b.itemvalue
   and b.itemid =
       (select itemid from base_dataitem where itemcode = 'EQUIPMENTTYPE')
  left join base_dataitemdetail c
    on t.equipmentkind = c.itemvalue
   and c.itemid =
       (select itemid from base_dataitem where itemcode = 'EquipmentKind')
  left join base_dataitemdetail d
    on t.equipmentbreed = d.itemvalue
   and d.itemid =
       (select itemid from base_dataitem where itemcode = 'EquipmentBreed')";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
                pagination.conditionJson = where;
            }
            pagination.sidx = "t.CreateDate";
            pagination.sord = "desc";
            var watch = CommonHelper.TimerStart();
            var data = specialequipmentbll.GetPageList(pagination, queryJson);

            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "特种设备管理";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "特种设备管理.xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity = listColumnEntity;
            ColumnEntity columnentity = new ColumnEntity();
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "equipmentname", ExcelColumn = "设备名称", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "idx", ExcelColumn = "序号", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "employdept", ExcelColumn = "使用单位", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "equipmentregisterno", ExcelColumn = "设备注册代码", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "specifications", ExcelColumn = "设备型号", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "factoryno", ExcelColumn = "出厂编号", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "employsite", ExcelColumn = "使用地点", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "location", ExcelColumn = "设备所在地", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "state", ExcelColumn = "使用状态", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "equipmentno", ExcelColumn = "设备内部编号", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "equipmenttype", ExcelColumn = "设备种类", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "equipmentkind", ExcelColumn = "设备类别", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "equipmentbreed", ExcelColumn = "设备品种", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "reportnumber", ExcelColumn = "检验报告编号", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkdate", ExcelColumn = "检验日期", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "nextcheckdate", ExcelColumn = "下次检验日期", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "examineverdict", ExcelColumn = "检验结论", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "examineappeardate", ExcelColumn = "监察单上报日期", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "reportexaminedate", ExcelColumn = "报检日期", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "acceptstate", ExcelColumn = "受理状态", Alignment = "center" });
            //调用导出方法
            ExcelHelper.ExcelDownload(data, excelconfig);

            return Success("导出成功。");
        }


        /// <summary>
        /// 导出用户列表
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "导出特种设备数据")]
        public ActionResult ExportForSJ(string queryJson)
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "ID";
            pagination.p_fields = @"EquipmentName,EquipmentNo,Specifications,CertificateNo,NextCheckDate,district,case when t.state=1 then '未启用'
when t.state=2 then '在用' when t.state=3 then '停用' when t.state=4 then '报废' when t.state=5 then '离厂'  end as state ,certificateid,checkfileid";
            pagination.p_tablename = "BIS_SPECIALEQUIPMENT t";
            pagination.sidx = " t.createdate";
            pagination.sord = "desc";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                IEnumerable<DepartmentEntity> orgcodelist = new List<DepartmentEntity>();
                orgcodelist = departmentBLL.GetList().Where(t => t.DeptCode.Contains(user.NewDeptCode) && t.Nature == "厂级");
                pagination.conditionJson += "  (";
                foreach (DepartmentEntity item in orgcodelist)
                {
                    pagination.conditionJson += " createuserorgcode ='" + item.EnCode + "' or ";
                }
                pagination.conditionJson += " createuserorgcode = '" + user.OrganizeCode + "') ";
            }

            var watch = CommonHelper.TimerStart();
            var data = specialequipmentbll.GetPageList(pagination, queryJson);

            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "特种设备管理";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "特种设备管理.xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity = listColumnEntity;
            ColumnEntity columnentity = new ColumnEntity();
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "equipmentname", ExcelColumn = "设备名称", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "equipmentno", ExcelColumn = "设备编号", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "specifications", ExcelColumn = "规格型号", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "certificateno", ExcelColumn = "使用登记证编号", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "nextcheckdate", ExcelColumn = "下次检验日期", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "district", ExcelColumn = "所在区域", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "state", ExcelColumn = "状态", Alignment = "center" });
            //调用导出方法
            ExcelHelper.ExcelDownload(data, excelconfig);

            return Success("导出成功。");
        }

        /// <summary>
        /// 导出省级检查次数记录
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HandlerMonitor(0, "导出省级检查次数记录")]
        public ActionResult ExportSafetyCheckRecordForSJ(string queryJson)
        {

            var watch = CommonHelper.TimerStart();
            var data = specialequipmentbll.GetSafetyCheckRecordForSJ(queryJson);

            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "省级特种设备检查次数统计";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "省级特种设备检查次数记录.xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity = listColumnEntity;
            ColumnEntity columnentity = new ColumnEntity();
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkendtime", ExcelColumn = "检查时间", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkdatarecordname", ExcelColumn = "检查名称", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "CHECKMAN", ExcelColumn = "检查人员", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "Count", ExcelColumn = "发现问题和隐患", Alignment = "center" });
            //调用导出方法
            ExcelHelper.ExcelDownload(data, excelconfig);

            return Success("导出成功。");
        }

        #endregion

        #region 导入普通设备
        /// <summary>
        /// 导入普通设备
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportEquipment()
        {
            try
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
                    var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                    string filePath = Server.MapPath("~/Resource/temp/" + fileName);
                    file.SaveAs(filePath);
                    DataTable dt = ExcelHelper.ExcelImport(filePath);
                    var districtList = new DistrictBLL().GetList().Where(x => x.OrganizeId == user.OrganizeId).ToList();
                    var deptList = new DepartmentBLL().GetList().Where(x => x.OrganizeId == user.OrganizeId).ToList();
                    var users = new UserBLL().GetUserInfoByDeptCode(user.OrganizeCode);
                    for (int i = 1; i < dt.Rows.Count; i++)
                    {
                        object[] vals = dt.Rows[i].ItemArray;
                        if (IsEndRow(vals) == true)
                            break;

                        var msg = "";
                        string a = "";
                        string Affiliation = "";
                        string EquipmentType = "";

                        var en = this.GetTypeValue(vals[4].ToString(), "'EQUIPMENTTYPE'");
                        if (en != null) {
                            EquipmentType = en.ItemValue;
                        }
                        var obj = vals[13];//所属单位
                        if (obj.ToString().Length > 0)
                        {
                            var dept = deptList.Where(t => t.FullName == obj.ToString().Trim()).FirstOrDefault();
                            if (dept != null)
                            {
                                if (dept.Nature == "承包商" || dept.Nature == "分包商" || dept.Description == "外包工程承包商")
                                {
                                    Affiliation = "外包单位所有";
                                }
                                else
                                {
                                    Affiliation = "本单位自有";
                                }
                            }
                        }
                        else
                        {
                            Affiliation = "本单位自有";
                        }
                        switch (Affiliation)
                        {
                            case "本单位自有":
                                a = "T1-";
                                break;
                            case "外包单位所有":
                                a = "T2-";
                                break;
                            default:
                                a = "T1-";
                                break;
                        }
                        switch (EquipmentType)
                        {
                            case "1":
                                a += "GL";
                                break;
                            case "2":
                                a += "RQ";
                                break;
                            case "3":
                                a += "GD";
                                break;
                            case "4":
                                a += "QZ";
                                break;
                            case "5":
                                a += "CL";
                                break;
                            case "6":
                                a += "FJ";
                                break;
                            case "7":
                                a += "DT";
                                break;
                            case "8"://压力管道元件
                                a += "YJ";
                                break;
                            case "9"://客运索道
                                a += "SD";
                                break;
                            case "10"://大型游乐设施
                                a += "SS";
                                break;
                            default:
                                break;
                        }

                        if (Validate(i, vals, deptList, districtList, out msg, users) == true)
                        {
                            var eno = int.Parse(specialequipmentbll.GetEquipmentNo(a, user.OrganizeCode)) + 1;
                            var entity = GenEntity(vals, deptList, districtList, a + eno.ToString().PadLeft(4, '0'), users);
                            specialequipmentbll.SaveForm("", entity);
                            eno++;
                            sussceed++;
                        }
                        else
                        {
                            falseMessage += "第" + (i + 2) + "行" + msg + "</br>";
                            error++;
                        }
                    }
                    count = dt.Rows.Count;
                    message = "共有" + (count - 1) + "条记录,成功导入" + sussceed + "条，失败" + error + "条";
                    if (error > 0)
                    {
                        message += "，错误信息如下：</br>" + falseMessage;
                    }

                    //删除临时文件
                    System.IO.File.Delete(filePath);
                }
                return message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        private bool IsEndRow(object[] vals)
        {
            bool r = false;

            r = Array.TrueForAll(vals, x => (x == null || x == DBNull.Value || x.ToString() == ""));

            return r;
        }
        private bool Validate(int index, object[] vals, List<DepartmentEntity> deptList, List<DistrictEntity> districtList, out string msg, IList<UserInfoEntity> users)
        {
            var r = true;
            msg = "：";
            var obj = vals[1];
            if (obj == null || obj == DBNull.Value || obj.ToString().Trim() == "")
            {
                msg += "所属区域不能为空，";
                r = false;
            }
            else
            {
                int ncount = districtList.Count(x => x.DistrictName == obj.ToString().Trim());
                if (ncount <= 0)
                {
                    msg += "所属区域名称与系统中的区域名称不匹配，";
                    r = false;
                }
            }
            obj = vals[3];
            if (obj == null || obj == DBNull.Value || obj.ToString().Trim() == "")
            {
                msg += "设备名称不能为空，";
                r = false;
            }
            var zl = "";
            var lb = "";
            obj = vals[4];
            if (obj == null || obj == DBNull.Value || obj.ToString().Trim() == "")
            {
                msg += "设备种类不能为空，";
                r = false;
            }
            else
            {
                var en = GetTypeValue(obj.ToString(), "'EQUIPMENTTYPE'");
                if (en != null)
                {
                    zl = en.ItemValue;
                }
                else {
                    msg += "设备种类系统中不存在，";
                    r = false;
                }
            }
            obj = vals[5];
            if (obj == null || obj == DBNull.Value || obj.ToString().Trim() == "")
            {

            }
            else
            {
                var en = GetTypeValue(obj.ToString(), "'EquipmentKind'");
                if (en != null)
                {
                    if (zl != en.ItemCode)
                    {
                        msg += "该设备类别不在对应设备种类中，";
                        r = false;
                    }
                    else
                    {

                        lb = en.ItemValue;
                    }
                }
                else
                {
                    msg += "设备种类系统中不存在，";
                    r = false;
                }
            }
            obj = vals[6];
            if (obj == null || obj == DBNull.Value || obj.ToString().Trim() == "")
            {

            }
            else
            {
                var en = GetTypeValue(obj.ToString(), "'EquipmentBreed'");
                if (en != null)
                {
                    if (lb != en.ItemCode)
                    {
                        msg += "该设备品种不在对应设备类别中，";
                        r = false;
                    }
                    else
                    {

                        lb = en.ItemValue;
                    }
                }
                else
                {
                    msg += "设备品种系统中不存在，";
                    r = false;
                }
            }

            obj = vals[11];
            if (obj == null || obj == DBNull.Value || obj.ToString().Trim() == "")
            {
                msg += "最近检验日期不能为空，";
                r = false;
            }
            else
            {
                DateTime checkDate;
                if (!DateTime.TryParse(obj.ToString(), out checkDate))
                {
                    msg += "最近检验日期格式错误，";
                    r = false;
                }
            }

            obj = vals[12];
            if (obj == null || obj == DBNull.Value || obj.ToString().Trim() == "")
            {
                msg += "检验周期不能为空，";
                r = false;
            }
            else
            {
                int period;
                if (!int.TryParse(obj.ToString(), out period))
                {
                    msg += "检验周期格式错误，";
                    r = false;
                }
            }
            obj = vals[13];
            if (obj == null || obj == DBNull.Value || obj.ToString().Trim() == "")
            {
                //msg += "所属单位不能为空，";
                //r = false;
            }
            else
            {
                var dept = deptList.Where(t => t.FullName == obj.ToString().Trim()).FirstOrDefault();
                if (dept == null)
                {
                    msg += "所属单位名称与系统中的单位名称不匹配，";
                    r = false;
                }
                else
                {
                    obj = vals[18];
                    if (obj.ToString().Trim().Length > 0)
                    {
                        DataRow[] rows = outsouringengineerbll.GetEngineerDataByWBId(dept.DepartmentId).Select("ENGINEERNAME='" + obj.ToString().Trim() + "'");
                        if (rows.Length == 0)
                        {
                            msg += "外包工程名称与系统中的工程名称不匹配，";
                            r = false;
                        }
                    }
                }
            }
            obj = vals[14];//使用单位
            if (obj.ToString().Trim().Length > 0)
            {
                var dept = deptList.Where(t => t.FullName == obj.ToString().Trim()).FirstOrDefault();
                if (dept == null)
                {
                    msg += "使用单位名称与系统中的单位名称不匹配，";
                    r = false;
                }
            }
            obj = vals[15];
            if (obj == null || obj == DBNull.Value || obj.ToString().Trim() == "")
            {
                msg += "管控部门不能为空，";
                r = false;
            }
            else
            {
                int ncount = deptList.Count(x => x.FullName == obj.ToString().Trim());
                if (ncount <= 0)
                {
                    msg += "管控部门不正确，";
                    r = false;
                }
            }
            obj = vals[16];
            if (obj == null || obj == DBNull.Value || obj.ToString().Trim() == "")
            {
                msg += "安全管理人员不能为空，";
                r = false;
            }
            else
            {
                int count = users.Where(t => t.RealName == obj.ToString().Trim()).Count();
                if (count == 0)
                {
                    msg += "安全管理人员在系统中不存在，";
                    r = false;
                }
            }
            obj = vals[17];
            if (obj.ToString().Trim().Length > 0)
            {
                int count = users.Where(t => t.RealName == obj.ToString().Trim()).Count();
                if (count == 0)
                {
                    msg += "操作人员在系统中不存在，";
                    r = false;
                }
            }
            obj = vals[19];
            if (obj == null || obj == DBNull.Value || obj.ToString() == "")
            {
                msg += "使用状况不能为空，";
                r = false;
            }
            else
            {
                var v = obj.ToString();
                if (v != "未启用" && v != "在用" && v != "停用" && v != "报废" && v != "离厂")
                {
                    msg += "使用状况值不正确，";
                    r = false;
                }
            }
            obj = vals[20];
            if (obj == null || obj == DBNull.Value || obj.ToString().Trim() == "")
            {
                msg += "是否检查验收不能为空，";
                r = false;
            }
            else
            {
                var v = obj.ToString();
                if (v != "是" && v != "否" )
                {
                    msg += "是否检查验收值不正确，";
                    r = false;
                }
            }
            msg = msg.TrimEnd('，');

            return r;
        }
        public DataItemModel GetTypeValue(string text,string str)
        {
            //string val = "";
            DataItemModel di = new DataItemModel();
            var list = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetDataItemListByItemCode(str);
            foreach (ERCHTMS.Entity.SystemManage.ViewModel.DataItemModel dataitem in list)
            {
                if (dataitem.ItemName == text)
                {
                    //val = dataitem.ItemValue;
                    di = dataitem;
                    break;
                }
            }
            return di;
        }
        private SpecialEquipmentEntity GenEntity(object[] vals, List<DepartmentEntity> deptList, List<DistrictEntity> districtList, string eno, IList<UserInfoEntity> users)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            SpecialEquipmentEntity entity = new SpecialEquipmentEntity();
            var obj = vals[1];
            if (obj != null && obj != DBNull.Value)
            {
                var district = districtList.FirstOrDefault(x => x.DistrictName == obj.ToString().Trim());
                if (district != null)
                {
                    entity.District = district.DistrictName;
                    entity.DistrictId = district.DistrictID;
                    entity.DistrictCode = district.DistrictCode;
                }
            }
            entity.EmploySite = vals[2].ToString().Trim();
            entity.EquipmentName = vals[3].ToString().Trim();
            obj = vals[4];//设备种类
            if (obj.ToString().Length > 0)
            {
                entity.EquipmentType = GetTypeValue(obj.ToString(), "'EQUIPMENTTYPE'").ItemValue;
            }
            obj = vals[5];//设备类别
            if (obj.ToString().Length > 0)
            {
                entity.EquipmentKind = GetTypeValue(obj.ToString(), "'EquipmentKind'").ItemValue;
            }
            obj = vals[6];//设备品种
            if (obj.ToString().Length > 0)
            {
                entity.EquipmentBreed = GetTypeValue(obj.ToString(), "'EquipmentBreed'").ItemValue;
            }
            entity.EquipmentNo = vals[7].ToString().Trim();//设备内部编号
            entity.Specifications = vals[8].ToString().Trim();
            entity.EquipmentRegisterNo = vals[9].ToString().Trim();//设备注册代码
            entity.CertificateNo = vals[10].ToString().Trim();//使用登记证编号
            entity.CheckDate = DateTime.Parse(DateTime.Parse(vals[11].ToString()).ToString("yyyy-MM-dd"));//最近检验日期
            entity.CheckDateCycle = vals[12].ToString();//检验周期
            obj = vals[13];//所属单位
            if (obj.ToString().Length > 0)
            {
                var dept = deptList.Where(t => t.FullName == obj.ToString().Trim()).FirstOrDefault();
                if (dept != null)
                {
                    entity.EPIBOLYDEPTID = dept.DepartmentId;//所属单位
                    entity.EPIBOLYDEPT = dept.FullName;//所属单位

                    if (dept.Nature == "承包商" || dept.Nature == "分包商" || dept.Description == "外包工程承包商")
                    {
                        entity.Affiliation = "2";
                    }
                    else
                    {
                        entity.Affiliation = "1";
                    }

                    DataRow[] rows = outsouringengineerbll.GetEngineerDataByWBId(dept.DepartmentId).Select("ENGINEERNAME='" + vals[18].ToString().Trim() + "'");
                    if (rows.Length > 0)
                    {
                        entity.EPIBOLYPROJECTID = rows[0][1].ToString();//外包工程
                        entity.EPIBOLYPROJECT = vals[18].ToString().Trim();//外包工程
                    }
                }
            }
            else
            {
                entity.EPIBOLYDEPTID = user.OrganizeId;
                entity.EPIBOLYDEPT = user.OrganizeName;
                entity.Affiliation = "1";
            }
            obj = vals[14];//使用单位
            if (obj.ToString().Length > 0)
            {
                var dept = deptList.Where(t => t.FullName == obj.ToString().Trim()).FirstOrDefault();
                if (dept != null)
                {
                    entity.EmployDeptId = dept.DepartmentId;//使用单位ID
                    entity.EmployDept = dept.FullName;//使用单位
                }
            }
            obj = vals[15];//管控部门
            if (obj != null && obj != DBNull.Value)
            {
                var dept = deptList.FirstOrDefault(x => x.FullName == obj.ToString().Trim());
                if (dept != null)
                {
                    entity.ControlDept = dept.FullName;
                    entity.ControlDeptCode = dept.EnCode;
                    entity.ControlDeptID = dept.DepartmentId;
                }
            }
            obj = vals[16];//安全管理人员
            var listUsers = users.Where(t => t.RealName == obj.ToString().Trim());
            if (listUsers.Count() > 0)
            {
                UserInfoEntity ManagerUser = listUsers.FirstOrDefault();
                entity.SecurityManagerUser = ManagerUser.RealName;
                entity.SecurityManagerUserID = ManagerUser.UserId;
                entity.Telephone = ManagerUser.Telephone;
            }
            obj = vals[17];//操作人员
            if (obj != null || obj != DBNull.Value || obj.ToString().Trim().Length > 0)
            {
                var listUser = users.Where(t => t.RealName == obj.ToString().Trim());
                if (listUser.Count() > 0)
                {
                    entity.OperUser = listUser.FirstOrDefault().RealName;
                    entity.OperUserID = listUser.FirstOrDefault().UserId;
                }
            }
            entity.RelWord = "";
            entity.State = "1";
            var state = vals[19].ToString();
            if (state == "在用")
                entity.State = "2";
            else if (state == "停用")
                entity.State = "3";
            else if (state == "报废")
                entity.State = "4";
            else if (state == "离厂")
                entity.State = "5";

            entity.IsCheck = vals[20].ToString();//是否检查验收
           
            entity.NextCheckDate = entity.CheckDate.Value.AddDays(int.Parse(entity.CheckDateCycle));

            entity.EquipmentNo = eno;
            return entity;
        }
        #endregion

    }
}
