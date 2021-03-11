using System.Collections.Generic;
using System.Linq;
using ERCHTMS.Entity.EnvironmentalManage;
using ERCHTMS.Busines.EnvironmentalManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using BSFramework.Util.Offices;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.SystemManage.ViewModel;

namespace ERCHTMS.Web.Areas.EnvironmentalManage.Controllers
{
    /// <summary>
    /// 描 述：噪音检测
    /// </summary>
    public class NoisecheckController : MvcControllerBase
    {
        private NoisecheckBLL noisecheckbll = new NoisecheckBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();

        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.ehsDepartCode = "";
            //当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            DataItemModel ehsDepart = dataitemdetailbll.GetDataItemListByItemCode("'EHSDepartment'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();
            if (ehsDepart != null)
                ViewBag.ehsDepartCode = ehsDepart.ItemValue;
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
            pagination.p_fields = @"a.CreateUserId,a.CreateDate,a.CreateUserName,a.ModifyUserId,a.ModifyDate,a.ModifyUserName,a.CreateUserDeptCode,a.CreateUserOrgCode,checkuserid,checkusername,to_char(checkdate,'yyyy-MM-dd') checkdate
            ,zj1,zj2,zj3,zj4,zj5,zj6,yj1,yj2,yj3,yj4,yj5,yj6";
            pagination.p_tablename = @"BIS_NoiseCheck a ";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                //根据当前用户对模块的权限获取记录
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "createuserdeptcode", "createuserorgcode");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }
            }


            var watch = CommonHelper.TimerStart();
            var data = noisecheckbll.GetPageList(pagination, queryJson);
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
            var data = noisecheckbll.GetList(queryJson);
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
            var data = noisecheckbll.GetEntity(keyValue);
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
            noisecheckbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, NoisecheckEntity entity)
        {
            noisecheckbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion

        #region 导出excel列表

        /// <summary>
        /// 噪音检测报告
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "噪音检测报告")]
        public ActionResult exportExcelData(string condition, string queryJson)
        {
            Pagination pagination = new Pagination();
            queryJson = queryJson ?? "";
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "ID";
            pagination.p_fields = @"checkusername,zj1,yj1,zj2,yj2,zj3,yj3,zj4,yj4,zj5,yj5,zj6,yj6,to_char(checkdate,'yyyy-MM-dd') checkdate";
            pagination.p_tablename = @"BIS_NoiseCheck a ";
            pagination.sord = "CreateDate";
            #region 权限校验
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "CREATEUSERDEPTCODE", "CREATEUSERORGCODE");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }

            }
            #endregion
            var data = noisecheckbll.GetPageList(pagination, queryJson);

            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "噪音检测报告";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "噪音检测报告.xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();
            listColumnEntity.Add(new ColumnEntity() { Column = "checkusername", ExcelColumn = "检测人员" });
            listColumnEntity.Add(new ColumnEntity() { Column = "zj1", Width = 20, ExcelColumn = "1#厂界噪声点位昼间" });
            listColumnEntity.Add(new ColumnEntity() { Column = "yj1", Width = 20, ExcelColumn = "1#厂界噪声点位夜间" });
            listColumnEntity.Add(new ColumnEntity() { Column = "zj2", Width = 20, ExcelColumn = "2#厂界噪声点位昼间" });
            listColumnEntity.Add(new ColumnEntity() { Column = "yj2", Width = 20, ExcelColumn = "2#厂界噪声点位夜间" });
            listColumnEntity.Add(new ColumnEntity() { Column = "zj3", Width = 20, ExcelColumn = "3#厂界噪声点位昼间" });
            listColumnEntity.Add(new ColumnEntity() { Column = "yj3", Width = 20, ExcelColumn = "3#厂界噪声点位夜间" });
            listColumnEntity.Add(new ColumnEntity() { Column = "zj4", Width = 20, ExcelColumn = "4#厂界噪声点位昼间" });
            listColumnEntity.Add(new ColumnEntity() { Column = "yj4", Width = 20, ExcelColumn = "4#厂界噪声点位夜间" });
            listColumnEntity.Add(new ColumnEntity() { Column = "zj5", Width = 20, ExcelColumn = "5#厂界噪声点位昼间" });
            listColumnEntity.Add(new ColumnEntity() { Column = "yj5", Width = 20, ExcelColumn = "5#厂界噪声点位夜间" });
            listColumnEntity.Add(new ColumnEntity() { Column = "zj6", Width = 20, ExcelColumn = "6#厂界噪声点位昼间" });
            listColumnEntity.Add(new ColumnEntity() { Column = "yj6", Width = 20, ExcelColumn = "6#厂界噪声点位夜间" });
            listColumnEntity.Add(new ColumnEntity() { Column = "checkdate", ExcelColumn = "检测日期" });
            excelconfig.ColumnEntity = listColumnEntity;

            //调用导出方法
            ExcelHelper.ExcelDownload(data, excelconfig);
            return Success("导出成功。");
        }

        #endregion
    }
}
