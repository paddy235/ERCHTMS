using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace ERCHTMS.Web.Areas.HiddenTroubleManage.Controllers
{
    /// <summary>
    /// 描 述：流程配置实例表
    /// </summary>
    public class WfInstanceController : MvcControllerBase
    {
        private WfInstanceBLL wfinstancebll = new WfInstanceBLL();
        private WfSettingBLL wfsettingbll = new WfSettingBLL();
        private WfConditionBLL wfconditionbll = new WfConditionBLL();
        private WfConditionAddtionBLL wfconditionaddtionbll = new WfConditionAddtionBLL();
        private WfConditionOfRoleBLL wfconditionofrolebll = new WfConditionOfRoleBLL();

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
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CopyForm()
        {
            return View();
        }
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult BaseIndex()
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult BaseForm()
        {
            return View();
        }

        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ConditionForm()
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
            var data = wfinstancebll.GetList(queryJson);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取条件配置列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetConditionListJson(string queryJson)
        {
            var data = wfconditionofrolebll.GetList(queryJson);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetProcessJson()
        {
            var data = wfinstancebll.GetProcessData();
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetActivityJson(string instanceid)
        {
            var data = wfinstancebll.GetActivityData(instanceid);

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
            var data = wfinstancebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }


        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetConditionFormJson(string keyValue)
        {
            var data = wfconditionofrolebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }


        #region 流程配置实例列表
        /// <summary>
        /// 流程配置实例列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>   
        [HttpGet]
        public ActionResult GetWfInstanceInfoPageList(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var data = wfinstancebll.GetWfInstanceInfoPageList(pagination, queryJson);
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
        #endregion

        #region 流程参数信息列表
        /// <summary>
        /// 流程参数信息列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>   
        [HttpGet]
        public ActionResult GetInstanceConditionInfoList(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var data = wfconditionofrolebll.GetInstanceConditionInfoList(pagination, queryJson);
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
            var setting = wfsettingbll.GetList(keyValue);

            foreach (WfSettingEntity sentity in setting)
            {
                string settingId = sentity.ID;
                var condition = wfconditionbll.GetList(settingId);
                foreach (WfConditionEntity centity in condition)
                {
                    string conditionid = centity.ID;
                    var addtion = wfconditionaddtionbll.GetList(conditionid);
                    foreach (WfConditionAddtionEntity aentity in addtion)
                    {
                        string addtionid = aentity.ID;
                        wfconditionaddtionbll.RemoveForm(addtionid);
                    }
                    wfconditionbll.RemoveForm(conditionid);
                }
                wfsettingbll.RemoveForm(settingId);
            }
            wfinstancebll.RemoveForm(keyValue);

            return Success("删除成功。");
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveConditionForm(string keyValue)
        {
            wfconditionofrolebll.RemoveForm(keyValue);

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
        public ActionResult SaveForm(string keyValue, WfInstanceEntity entity)
        {
            wfinstancebll.SaveForm(keyValue, entity);

            return Success("操作成功。");
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
        public ActionResult SaveConditionForm(string keyValue, WfConditionOfRoleEntity entity)
        {
            wfconditionofrolebll.SaveForm(keyValue, entity);

            return Success("操作成功。");
        }


        /// <summary>
        /// 复制流程所有对象（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveCopyForm(string keyValue)
        {
            var wfinstance = wfinstancebll.GetEntity(keyValue);
            //创建新的流程实例
            WfInstanceEntity entity = new WfInstanceEntity();
            entity = wfinstance;
            entity.ID = string.Empty;
            entity.INSTANCENAME = wfinstance.INSTANCENAME + "_复制对象";
            wfinstancebll.SaveForm("", entity);

            string newId = entity.ID; //新的InstanceId
            if (!string.IsNullOrEmpty(newId))
            {
                List<WfSettingEntity> slist = wfsettingbll.GetList(keyValue).ToList();

                foreach (WfSettingEntity sentiy in slist)
                {
                    string sourceSettingId = sentiy.ID;
                    WfSettingEntity setting = new WfSettingEntity();
                    setting = sentiy;
                    setting.ID = string.Empty;
                    setting.INSTANCEID = newId;
                    wfsettingbll.SaveForm("", setting);
                    //新配置项
                    if (!string.IsNullOrEmpty(setting.ID))
                    {
                        #region 新条件设置
                        List<WfConditionEntity> clist = wfconditionbll.GetList(sourceSettingId).ToList();
                        foreach (WfConditionEntity condition in clist)
                        {
                            string sourceConditionId = condition.ID;
                            WfConditionEntity centity = new WfConditionEntity();
                            centity = condition;
                            centity.ID = string.Empty;
                            centity.SETTINGID = setting.ID;
                            wfconditionbll.SaveForm("", centity);
                            //新具体条件内容明细
                            if (!string.IsNullOrEmpty(centity.ID))
                            {
                                List<WfConditionAddtionEntity> calist = wfconditionaddtionbll.GetList(sourceConditionId).ToList();
                                foreach (WfConditionAddtionEntity caentity in calist)
                                {
                                    WfConditionAddtionEntity addentity = new WfConditionAddtionEntity();
                                    addentity = caentity;
                                    addentity.ID = string.Empty;
                                    addentity.WFCONDITIONID = centity.ID;
                                    wfconditionaddtionbll.SaveForm("", addentity);
                                }
                            }
                        }
                        #endregion
                    }
                }

                //条件
                List<WfConditionOfRoleEntity> rolelist = wfconditionofrolebll.GetList(keyValue).ToList();
                foreach (WfConditionOfRoleEntity rentity in rolelist)
                {
                    WfConditionOfRoleEntity newrentity = new WfConditionOfRoleEntity();
                    newrentity = rentity;
                    newrentity.ID = string.Empty;
                    newrentity.INSTANCEID = newId;
                    wfconditionofrolebll.SaveForm("", newrentity);

                }
            }
            return Success("操作成功。");
        }
        #endregion

        #region 创建最新的流程
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveNewInstanceForm(string queryJson)
        {
            wfinstancebll.SaveNewInstance(queryJson);
            return Success("操作成功。");
        }
        #endregion


        #region 创建最新的流程
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult BatchUpdateInstance(string typename="基础流程") 
        {
            wfinstancebll.BatchUpdateInstance(typename); 
            return Success("操作成功。");
        }
        #endregion
    }
}