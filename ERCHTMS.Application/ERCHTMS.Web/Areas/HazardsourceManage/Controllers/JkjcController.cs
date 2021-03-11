using ERCHTMS.Entity.HazardsourceManage;
using ERCHTMS.Busines.HazardsourceManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.HiddenTroubleManage;
using System.Linq;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using BSFramework.Util.Offices;
using System.Collections.Generic;

namespace ERCHTMS.Web.Areas.HazardsourceManage.Controllers
{
    /// <summary>
    /// 描 述：监控
    /// </summary>
    public class JkjcController : MvcControllerBase
    {
        private JkjcBLL jkjcbll = new JkjcBLL();
        private HazardsourceBLL hazardsourcebll = new HazardsourceBLL();
        #region 视图功能
        public ActionResult yhtz()
        {
            return View();
        }


        /// <summary>
        /// 登记隐患
        /// </summary>
        /// <returns></returns>
        public ActionResult djyh()
        {
            return View();
        }

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
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = jkjcbll.GetList(queryJson);
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
            var data = jkjcbll.GetEntity(keyValue);
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
            jkjcbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, JkjcEntity entity)
        {           
            //查询隐患数量
            HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL(); //隐患基本信息
            var list = htbaseinfobll.GetList(" and relevanceType='DangerSource' and RelevanceId='" + entity.ID + "'").ToList();
            entity.JkyhzgIds = list.Count.ToString();
            jkjcbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion


        #region 数据导出
        /// <summary>
        /// 导出用户列表
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "导出数据")]
        public ActionResult Export(string condition, string queryJson)
        {
            Pagination pagination = new Pagination();
            queryJson = queryJson ?? "";
            pagination.p_kid = "ID";
            pagination.p_fields = "districtname, DANGERSOURCE, jkarear,jktimestart,jktimeend,JkyhzgIds,case WHEN  jkskstatus>0 then '已受控' WHEN  jkskstatus=0 then '不受控' else '未监控' end as jkskstatusname";
            pagination.p_tablename = "V_HSD_DANGERQD_JK t";
            pagination.conditionJson = "1=1";
            pagination.page = 1;
            pagination.rows = 100000000;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                if (user.RoleName.Contains("省级"))
                {
                    pagination.conditionJson += " and (CreateUserId='" + user.UserId + "' or DeptCode in(select  encode from BASE_DEPARTMENT start with encode='" + user.NewDeptCode + "' connect by  prior  departmentid = parentid))";
                }
                else
                {
                    pagination.conditionJson += " and (CreateUserId='" + user.UserId + "' or DeptCode in(select  encode from BASE_DEPARTMENT start with encode='" + user.DeptCode + "' connect by  prior  departmentid = parentid))";
                }
                //pagination.conditionJson += " and (CreateUserId='" + user.UserId + "' or DeptCode like '" + user.DeptCode + "%')";
            }



            var watch = CommonHelper.TimerStart();
            var data = hazardsourcebll.GetPageList(pagination, queryJson);

            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "重大危险源监控检测";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "重大危险源监控检测.xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();

            listColumnEntity.Add(new ColumnEntity() { Column = "districtname", ExcelColumn = "所属区域" });
            listColumnEntity.Add(new ColumnEntity() { Column = "DANGERSOURCE".ToLower(), ExcelColumn = "危险源名称/场所" });
            listColumnEntity.Add(new ColumnEntity() { Column = "jkarear".ToLower(), ExcelColumn = "监控地点" });
            listColumnEntity.Add(new ColumnEntity() { Column = "jktimestart".ToLower(), ExcelColumn = "监控时间起" });
            listColumnEntity.Add(new ColumnEntity() { Column = "jktimeend".ToLower(), ExcelColumn = "监控时间止" });
            listColumnEntity.Add(new ColumnEntity() { Column = "JkyhzgIds".ToLower(), ExcelColumn = "隐患" });
            listColumnEntity.Add(new ColumnEntity() { Column = "jkskstatusname".ToLower(), ExcelColumn = "监控状态" });
            excelconfig.ColumnEntity = listColumnEntity;

            //调用导出方法
            ExcelHelper.ExcelDownload(data, excelconfig);
            return Success("导出成功。");
        }
        #endregion
    }
}
