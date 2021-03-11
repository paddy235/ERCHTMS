using ERCHTMS.Entity.PersonManage;
using ERCHTMS.Busines.PersonManage;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Linq;
using ERCHTMS.Busines.AuthorizeManage;
using System;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Code;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using ERCHTMS.Busines.SystemManage;
namespace ERCHTMS.Web.Areas.PersonManage.Controllers
{
    /// <summary>
    /// 描 述：人员证书
    /// </summary>
    public class BlacklistController : MvcControllerBase
    {
        private BlacklistBLL certificatebll = new BlacklistBLL();

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
        /// <param name="userId">用户Id</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string userId)
        {
            var data = certificatebll.GetList(userId).ToList();
            return ToJsonResult(data);
            //var JsonData = new
            //{
            //    rows = data,
            //    total = data.Count,
            //    page = 1,
            //    records = 1
            //};
            //return Content(JsonData.ToJson());
        }
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "id";
            pagination.p_fields = "REALNAME,GENDER,identifyid,DEPTNAME,reason,jointime,a.userid";
            pagination.p_tablename = "bis_blacklist a left join v_userinfo u on a.userid=u.userid";
            pagination.conditionJson = "isblack=1";

            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string where = "";
            if (user.RoleName.Contains("省级"))
            {
                var queryParam = queryJson.ToJObject();
                if (!queryParam["departmentCode"].IsEmpty())
                {
                    var dept = new DepartmentBLL().GetEntityByCode(queryParam["departmentCode"].ToString());
                    if (dept != null)
                    {
                        if (dept.Nature == "省级")
                        {
                            pagination.conditionJson += string.Format(" and departmentCode in(select encode from BASE_DEPARTMENT where deptcode like '{0}%')", dept.DeptCode);
                        }
                        else
                        {
                            pagination.conditionJson += string.Format(" and departmentCode  like '{0}%'", dept.EnCode);
                        }
                    }
                }
            }
            else
            {
                where = "departmentcode like '" + user.DeptCode + "%'";
                if (!user.IsSystem)
                {
                    string con = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "departmentcode", "organizecode");
                    if (!string.IsNullOrEmpty(con))
                    {
                        where = con;
                        pagination.conditionJson += " and " + where;
                    }
                    else
                    {
                        where = "1=1";
                    }
                }

                // where = new AuthorizeBLL().GetModuleDataAuthority(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "DEPARTMENTCODE", "OrganizeCode");
                //pagination.conditionJson = string.IsNullOrEmpty(where) ? pagination.conditionJson : pagination.conditionJson + " and " + where;
            }
            var watch = CommonHelper.TimerStart();
            var data = new UserBLL().GetPageList(pagination, queryJson);
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
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = certificatebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取黑名单条件用户
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpPost]
        public ActionResult GetBlacklistUsers()
        {
            Operator user = OperatorProvider.Provider.Current();
            var data = certificatebll.GetBlacklistUsers(user);
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
            certificatebll.RemoveForm(keyValue);
            SaveForbidden(keyValue, "", 1);
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
        public ActionResult SaveForm(string keyValue, BlacklistEntity entity)
        {
            certificatebll.SaveForm(keyValue, entity);
            SaveForbidden(entity.UserId, entity.Reason, 0);
            return Success("操作成功。");
        }

        /// <summary>
        /// 同步修改华电可门电厂海康平台出入权限（黑名单）
        /// </summary>
        /// <param name="entity"></param>
        public void SaveForbidden(string UserId, string Reason, int type)
        {
            //说明：加入禁入名单相当于【双控离厂、加入黑名单】
            DataItemDetailBLL itemBll = new DataItemDetailBLL();
            string KMIndex = itemBll.GetItemValue("KMIndexUrl");
            if (!string.IsNullOrEmpty(KMIndex))
            {//只允许可门电厂人员执行该操作
                List<TemporaryUserEntity> tempuserList = new TemporaryGroupsBLL().GetUserList();//所有临时人员
                List<TemporaryUserEntity> list = new List<TemporaryUserEntity>();
                if (type == 0)
                {//加入
                    foreach (var uid in UserId.Split(','))
                    {
                        var uentity = tempuserList.Where(t => t.USERID == uid).FirstOrDefault();
                        if (uentity != null)
                        {
                            uentity.EndTime = DateTime.Now;
                            uentity.Remark = Reason;
                            list.Add(uentity);
                        }
                    }
                    new TemporaryGroupsBLL().SaveForbidden(list);
                }
                else
                {//移除
                    new TemporaryGroupsBLL().RemoveForbidden(UserId);
                }
            }
        }

        /// <summary>
        /// 加入黑名单
        /// </summary>
        /// <param name="leaveTime">离场时间</param>
        /// <param name="userId">用户Id,多个用逗号分隔</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "人员离场")]
        public ActionResult Leave(string leaveTime, [System.Web.Http.FromBody]string userId)
        {
            try
            {
                UserBLL userBLL = new UserBLL();
                userBLL.SetBlack(userId, 1);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        #endregion


        #region 数据导出
        /// <summary>
        /// 事故事件快报
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "人员黑名单清单")]
        public ActionResult Export(string queryJson)
        {
            Pagination pagination = new Pagination();
            queryJson = queryJson ?? "";
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "id";
            pagination.p_fields = "REALNAME,GENDER,identifyid,DEPTNAME,jointime,reason";
            pagination.p_tablename = "bis_blacklist a left join v_userinfo u on a.userid=u.userid";
            pagination.conditionJson = "isblack=1";
            string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "DEPARTMENTCODE", "OrganizeCode");
            pagination.conditionJson = string.IsNullOrEmpty(where) ? pagination.conditionJson : pagination.conditionJson + " and " + where;
            var watch = CommonHelper.TimerStart();
            var data = new UserBLL().GetPageList(pagination, queryJson);

            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "人员黑名单";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "人员黑名单.xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();

            listColumnEntity.Add(new ColumnEntity() { Column = "REALNAME".ToLower(), ExcelColumn = "姓名", Alignment = "Center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "GENDER".ToLower(), ExcelColumn = "性别", Alignment = "Center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "identifyid".ToLower(), ExcelColumn = "身份证号", Alignment = "Center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "DEPTNAME".ToLower(), ExcelColumn = "单位/部门", Alignment = "Center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "jointime".ToLower(), ExcelColumn = "加入黑名单时间", Alignment = "Center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "reason".ToLower(), ExcelColumn = "加入黑名单原因", Alignment = "Center" });
            excelconfig.ColumnEntity = listColumnEntity;

            //调用导出方法
            ExcelHelper.ExcelDownload(data, excelconfig);
            return Success("导出成功。");
        }
        #endregion
    }
}
