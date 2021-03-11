using ERCHTMS.Entity.EquipmentManage;
using ERCHTMS.Busines.EquipmentManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Web.Areas.EquipmentManage.Controllers
{
    /// <summary>
    /// 描 述：特种设备检验记录
    /// </summary>
    public class EquipmentExamineController : MvcControllerBase
    {
        private EquipmentExamineBLL EquipmentExaminebll = new EquipmentExamineBLL();

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
            var queryParam = queryJson.ToJObject();
            //设备ID
            string equipmentid = queryParam["equipmentid"].ToString();
            pagination.p_kid = "ID";
            pagination.p_fields = "CreateUserId,CreateUserDeptCode,CreateUserOrgCode,CreateDate,ExamineUnit,ExamineType,ExamineDate,ExaminePeriod,ExamineVerdict,ReportNumber";
            pagination.p_tablename = "HRS_EQUIPMENTEXAMINE t";
            pagination.conditionJson = string.Format(@" equipmentid='{0}'", equipmentid);
            var watch = CommonHelper.TimerStart();
            var data = EquipmentExaminebll.GetPageList(pagination);
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
            var data = EquipmentExaminebll.GetList(queryJson);
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
            var data = EquipmentExaminebll.GetEntity(keyValue);
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
            EquipmentExaminebll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, EquipmentExamineEntity entity)
        {
            EquipmentExaminebll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        /// <summary>
        /// 保存表单（批量）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveAllForm(string keyValue,string EquipmentId, EquipmentExamineEntity entity)
        {
            int i = 0;
            string[] eid = EquipmentId.Split(',');
            foreach (var id in keyValue.Split(','))
            {
                entity.EquipmentId = eid[i];
                EquipmentExaminebll.SaveForm(id, entity);
                i++;
            }
            //EquipmentExaminebll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        /// <summary>
        /// 获取指定数量的GUID
        /// </summary>
        /// <param name="num">数量</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetGuidAll(int num)
        {
            List<string> guids = new List<string>();
            for (int i = 0; i < num; i++) {
                string guid= Guid.NewGuid().ToString();
                if (guids.Contains(guid))
                {
                    num++;
                }
                else
                {
                    guids.Add(guid);
                }
            }
            return ToJsonResult(guids);
        }
        #endregion
    }
}
