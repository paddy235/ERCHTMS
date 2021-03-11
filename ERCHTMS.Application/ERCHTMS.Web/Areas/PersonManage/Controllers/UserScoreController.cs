using ERCHTMS.Entity.PersonManage;
using ERCHTMS.Busines.PersonManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.BaseManage;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using ERCHTMS.Busines.SystemManage;
using System;

namespace ERCHTMS.Web.Areas.PersonManage.Controllers
{
    /// <summary>
    /// 描 述：人员积分
    /// </summary>
    public class UserScoreController : MvcControllerBase
    {
        private UserScoreBLL userscorebll = new UserScoreBLL();

        #region 视图功能
        /// <summary>
        /// 积分列表
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
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit()
        {
            return View();
        }
        /// <summary>
        /// 积分排名
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Rank()
        {
            return View();
        }
        /// <summary>
        /// 人员积分明细
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Details()
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
            pagination.p_kid = "t.id as itemid";
            pagination.p_fields = "u.realname,u.Gender,u.identifyid,u.DEPTNAME,s.itemname,t.score,s.itemtype,u.OrganizeCode,u.DEPARTMENTCODE,u.realname as username,u.userid,t.createdate,t.createusername,case when s.isauto=0 then '手动积分' else '自动积分' end isauto";
            pagination.p_tablename = "BIS_USERSCORE t left join v_userinfo u on t.userid=u.userid left join bis_scoreset s on t.itemid=s.id";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "departmentcode", "organizecode");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }

            }
            var data = userscorebll.GetPageJsonList(pagination, queryJson);
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
        [HttpGet]
        public ActionResult GetRankListJson(Pagination pagination, string queryJson,string year)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "u.USERID";
            pagination.p_fields = "u.account,senddeptid,REALNAME,MOBILE,OrganizeName,DEPTNAME,DUTYNAME,POSTNAME,usertype,GENDER,u.OrganizeCode,u.CreateDate,isblack,identifyid,nvl(score,0) score,BIRTHDAY";
            pagination.p_tablename = "v_userinfo u left join (select a.userid,nvl(sum(score),0) as score from base_user a left join bis_userscore b on a.userid=b.userid where year='" + year + "' group by a.userid) t on u.userid=t.userid";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "departmentcode", "organizecode");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }

            }
          

            DataItemDetailBLL itemBll = new DataItemDetailBLL();
            ERCHTMS.Entity.SystemManage.DataItemDetailEntity entity = itemBll.GetEntity(user.OrganizeId);
            var data = userscorebll.GetPageJsonList(pagination, queryJson);
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch),
                userdata = new { score = entity == null ? "100" : entity.ItemValue }

            };
            return Content(JsonData.ToJson());
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = userscorebll.GetList(queryJson);
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
            var data = userscorebll.GetInfo(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取人员积分 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpPost]
        public string GetScoreInfo(string userId)
        {
            var data = userscorebll.GetScoreInfo(userId);
            return data;
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
            userscorebll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="userIds">用户Id(多个用逗号分隔)</param>
        /// <param name="itemIds">考核项目Id(多个用逗号分隔)</param>
        /// <param name="scores">考核分值(多个用逗号分隔)</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string userIds,string itemIds,string scores)
        {
            try
            {
                 string[] arr = userIds.Trim(',').Split(',');
                 string[] ids = itemIds.Trim(',').Split(',');
                 string[] arrScore = scores.Trim(',').Split(',');
                 Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                 string currUserId=user.UserId;
                 string deptCode=user.DeptCode;
                 string orgCode=user.OrganizeCode;
                 List<UserScoreEntity> list = new List<UserScoreEntity>();
                 foreach(string userid in arr)
                {
                  int j = 0;
                  foreach(string itemId in ids)
                  {
                    UserScoreEntity us = new UserScoreEntity
                    {
                        Id = System.Guid.NewGuid().ToString(),
                        UserId = userid,
                        ItemId = itemId,
                        CreateUserName=user.UserName,
                        Score = decimal.Parse(arrScore[j]),
                        Year=System.DateTime.Now.Year.ToString(),
                        CreateDate = System.DateTime.Now,
                        CreateUserId=currUserId,
                        CreateUserDeptCode=deptCode,
                        CreateUserOrgCode = orgCode
                    };
                    list.Add(us);
                   j++;
                }
               
            }
            userscorebll.Save(list);
            return Success("操作成功。");
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
           
        }

        /// <summary>
        /// 导出人员安全积分排名
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "导出人员安全积分排名")]
        public ActionResult ExportScoreRank(string condition, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DataItemDetailBLL itemBll = new DataItemDetailBLL();
            var item = itemBll.GetEntity(user.OrganizeId);
            string val = item==null ? "100" : item.ItemValue;
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "u.userid";
            pagination.p_fields = "REALNAME,GENDER,identifyid,DEPTNAME,(nvl(score,0)+" + val + ") score";
            pagination.p_tablename = "v_userinfo u left join (select a.userid,nvl(sum(score),0) as score from base_user a left join bis_userscore b on a.userid=b.userid where year='" + DateTime.Now.Year + "' group by a.userid) t on u.userid=t.userid";
            pagination.conditionJson = "1=1 ";
            pagination.sidx = "score";
            pagination.sord = "desc";
          
            string title = "人员安全积分排名";
            
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "departmentcode", "organizecode");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }

            }
            var data = userscorebll.GetPageJsonList(pagination, queryJson);

            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = title;
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = title + ".xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity = listColumnEntity;
            ColumnEntity columnentity = new ColumnEntity();
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "realname", ExcelColumn = "姓名", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "gender", ExcelColumn = "性别", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "identifyid", ExcelColumn = "身份证号", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "deptname", ExcelColumn = "单位/部门", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "score", ExcelColumn = "积分", Alignment = "center" });
            //调用导出方法
            ExcelHelper.ExcelDownload(data, excelconfig);
            return Success("导出成功。");
        }
        #endregion
    }
}
