using ERCHTMS.Entity.PowerPlantInside;
using ERCHTMS.Busines.PowerPlantInside;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using System;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.BaseManage;
using System.Collections.Generic;
using System.Linq;

namespace ERCHTMS.Web.Areas.PowerPlantInside.Controllers
{
    /// <summary>
    /// 描 述：事故事件验收
    /// </summary>
    public class PowerplantcheckController : MvcControllerBase
    {
        private PowerplantcheckBLL powerplantcheckbll = new PowerplantcheckBLL();
        private PowerplanthandledetailBLL powerplanthandledetailbll = new PowerplanthandledetailBLL();
        private PowerplanthandleBLL powerplanthandlebll = new PowerplanthandleBLL();
        private PowerplantreformBLL powerplantreformbll = new PowerplantreformBLL();

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
        /// 历史列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HistoryIndex()
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
            var data = powerplantcheckbll.GetList(queryJson);
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
            var data = powerplantcheckbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }


        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="PowerInsideHandleDetailId">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string PowerInsideHandleDetailId)
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                pagination.p_kid = "a.id";
                pagination.p_fields = @"a.auditresult,a.audittime,a.auditopinion,a.auditpeople,a.auditsignimg,b.filenum";
                pagination.p_tablename = @"bis_powerplantcheck a left join (select c.id,count(d.fileid) as filenum from bis_powerplantcheck c left join base_fileinfo d on d.recid=c.id group by c.id) b on a.id=b.id";
                pagination.conditionJson = string.Format("a.powerplanthandledetailid='{0}' and a.disable=0", PowerInsideHandleDetailId);

                var watch = CommonHelper.TimerStart();
                var data = powerplantreformbll.GetPageList(pagination, "");
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
            catch (System.Exception ex)
            {
                return Error(ex.ToString());
            }

        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="PowerInsideHandleReformId">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJsonByReformId(Pagination pagination, string PowerInsideHandleReformId)
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                pagination.p_kid = "a.id";
                pagination.p_fields = @"a.auditresult,a.auditopinion,a.auditpeople,a.auditsignimg,b.filenum";
                pagination.p_tablename = @"bis_powerplantcheck a left join (select c.id,count(d.fileid) as filenum from bis_powerplantcheck c left join base_fileinfo d on d.recid=c.id group by c.id) b on a.id=b.id";
                pagination.conditionJson = string.Format("a.POWERPLANTREFORMID='{0}'", PowerInsideHandleReformId);

                var watch = CommonHelper.TimerStart();
                var data = powerplantreformbll.GetPageList(pagination, "");
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
            catch (System.Exception ex)
            {
                return Error(ex.ToString());
            }

        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="PowerInsideHandleDetailId">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetHistoryPageListJson(Pagination pagination, string PowerInsideHandleDetailId)
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                pagination.p_kid = "a.id";
                pagination.p_fields = @"a.auditresult,a.audittime,a.auditopinion,a.auditpeople,a.auditsignimg,b.filenum";
                pagination.p_tablename = @"bis_powerplantcheck a left join (select c.id,count(d.fileid) as filenum from bis_powerplantcheck c left join base_fileinfo d on d.recid=c.id group by c.id) b on a.id=b.id";
                pagination.conditionJson = string.Format("a.POWERPLANTHANDLEDETAILID='{0}' and a.disable=1 ", PowerInsideHandleDetailId, user.UserId);
                
                var watch = CommonHelper.TimerStart();
                var data = powerplantreformbll.GetPageList(pagination, "");
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
            catch (System.Exception ex)
            {
                return Error(ex.ToString());
            }

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
        [HandlerMonitor(6, "事故事件验收删除")]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            powerplantcheckbll.RemoveForm(keyValue);
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
        [HandlerMonitor(5, "事故事件验收保存")]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, PowerplantcheckEntity entity)
        {
            powerplantcheckbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }


        /// <summary>
        /// 验收
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <param name="aentity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(0, "事故事件验收审核")]
        [AjaxOnly]
        public ActionResult ApporveForm(string keyValue, PowerplantcheckEntity aentity)
        {
            try
            {
                Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

                string state = string.Empty;

                string moduleName = "事故事件处理记录-验收";

                PowerplanthandledetailEntity entity = powerplanthandledetailbll.GetEntity(aentity.PowerPlantHandleDetailId);
                /// <param name="currUser">当前登录人</param>
                /// <param name="state">是否有权限审核 1：能审核 0 ：不能审核</param>
                /// <param name="moduleName">模块名称</param>
                /// <param name="createdeptid">创建人部门ID</param>
                ManyPowerCheckEntity mpcEntity = powerplanthandlebll.CheckAuditPower(curUser, out state, moduleName, entity.RealReformDeptId);


                #region //审核信息表
                PowerplantcheckEntity aidEntity = new PowerplantcheckEntity();
                aidEntity.AuditResult = aentity.AuditResult; //通过
                aidEntity.AuditTime = Convert.ToDateTime(aentity.AuditTime.Value.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss")); //审核时间
                aidEntity.AuditPeople = aentity.AuditPeople;  //审核人员姓名
                aidEntity.AuditPeopleId = aentity.AuditPeopleId;//审核人员id
                aidEntity.AuditDeptId = aentity.AuditDeptId;//审核部门id
                aidEntity.AuditDept = aentity.AuditDept; //审核部门
                aidEntity.AuditOpinion = aentity.AuditOpinion; //审核意见
                aidEntity.FlowId = entity.FlowId;
                aidEntity.AuditSignImg = string.IsNullOrWhiteSpace(aentity.AuditSignImg) ? "" : aentity.AuditSignImg.ToString().Replace("../..", "");
                aidEntity.PowerPlantHandleDetailId = aentity.PowerPlantHandleDetailId;
                aidEntity.PowerPlantHandleId = aentity.PowerPlantHandleId;
                aidEntity.PowerPlantReformId = aentity.PowerPlantReformId;
                aidEntity.AuditDeptId = curUser.DeptId;
                aidEntity.AuditDept = curUser.DeptName;
                aidEntity.Disable = 0;
                if (null != mpcEntity)
                {
                    aidEntity.Remark = (mpcEntity.AUTOID.Value - 1).ToString(); //备注 存流程的顺序号
                }
                else
                {
                    aidEntity.Remark = "7";
                }
                powerplantcheckbll.SaveForm(keyValue, aidEntity);
                #endregion

                #region  //保存事故事件处理记录
                //审核通过
                if (aentity.AuditResult == 0)
                {
                    //0表示流程未完成，1表示流程结束
                    if (null != mpcEntity)
                    {
                        entity.FlowDept = mpcEntity.CHECKDEPTID;
                        entity.FlowDeptName = mpcEntity.CHECKDEPTNAME;
                        entity.FlowRole = mpcEntity.CHECKROLEID;
                        entity.FlowRoleName = mpcEntity.CHECKROLENAME;
                        entity.ApplyState = 4;
                        entity.FlowId = mpcEntity.ID;
                        entity.FlowName = mpcEntity.CHECKDEPTNAME + "验收中";
                    }
                    else
                    {
                        entity.FlowDept = "";
                        entity.FlowDeptName = "";
                        entity.FlowRole = "";
                        entity.FlowRoleName = "";
                        entity.ApplyState = 5;
                        entity.FlowName = "";
                        entity.FlowId = "";
                    }
                }
                else //验收不通过 
                {
                    entity.FlowDept = "";
                    entity.FlowDeptName = "";
                    entity.FlowRole = "";
                    entity.FlowRoleName = "";
                    entity.ApplyState = 3; //退回到整改状态
                    entity.FlowName = "";
                    entity.FlowId = "";
                    entity.RealReformDept = "";
                    entity.RealReformDeptCode = "";
                    entity.RealReformDeptId = "";

                }
                //更新事故事件处理信息
                powerplanthandledetailbll.SaveForm(entity.Id, entity);
                powerplanthandlebll.UpdateApplyStatus(entity.PowerPlantHandleId);
                #endregion

                #region    //审核不通过
                if (aentity.AuditResult == 1)
                {
                    var reformlist = powerplantreformbll.GetList("").Where(t => t.PowerPlantHandleDetailId == aentity.PowerPlantHandleDetailId && t.Disable == 0).ToList(); //整改信息
                    var checklist = powerplantcheckbll.GetList("").Where(t => t.PowerPlantHandleDetailId == aentity.PowerPlantHandleDetailId && t.Disable == 0).ToList(); //验收信息
                    foreach (var item in reformlist)
                    {
                        item.Disable = 1; //将整改信息设置失效
                        powerplantreformbll.SaveForm(item.Id, item);
                    }
                    foreach (var item in checklist)
                    {
                        item.Disable = 1; //将验收信息设置失效
                        powerplantcheckbll.SaveForm(item.Id, item);

                    }
                }
                #endregion

                return Success("操作成功!");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
            
        }
        #endregion
    }
}
