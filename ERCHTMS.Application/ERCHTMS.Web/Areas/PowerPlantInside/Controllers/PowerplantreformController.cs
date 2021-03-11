using ERCHTMS.Entity.PowerPlantInside;
using ERCHTMS.Busines.PowerPlantInside;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.BaseManage;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ERCHTMS.Web.Areas.PowerPlantInside.Controllers
{
    /// <summary>
    /// 描 述：事故事件处理整改
    /// </summary>
    public class PowerplantreformController : MvcControllerBase
    {
        private PowerplantreformBLL powerplantreformbll = new PowerplantreformBLL();
        private PowerplanthandledetailBLL powerplanthandledetailbll = new PowerplanthandledetailBLL();
        private PowerplanthandleBLL powerplanthandlebll = new PowerplanthandleBLL();
        private PowerplantcheckBLL powerplantcheckbll = new PowerplantcheckBLL();

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

        /// <summary>
        /// 签收页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SignForm()
        {
            return View();
        }

        public ActionResult AppHandleForm()
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
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                pagination.p_kid = "d.ID";
                pagination.p_fields = @"d.CreateUserId,d.CreateDate,d.CreateUserName,d.ModifyUserId,d.ModifyDate,d.ModifyUserName,d.CreateUserDeptCode,d.CreateUserOrgCode,a.id as powerplanthandleid,d.applystate,a.accidenteventname,b.itemname as accidenteventtype,c.itemname as accidenteventproperty,a.HAPPENTIME,d.RECTIFICATIONTIME,e.id as powerplantreformid,d.rectificationdutypersonid,e.outtransferuseraccount,e.intransferuseraccount";
                pagination.p_tablename = @"bis_powerplanthandledetail d left join bis_powerplantreform e on d.id=e.powerplanthandledetailid and e.disable=0 left join BIS_POWERPLANTHANDLE a on d.powerplanthandleid =a.id 
                left join ( select * from base_dataitemdetail  where itemid = ( select itemid from base_dataitem where  parentid = 
                (select itemid from base_dataitem where itemname = '单位内部快报' ) and  itemcode = 'AccidentEventType') ) b on a.accidenteventtype = b.itemvalue
                  left join ( select * from base_dataitemdetail  where itemid = ( select itemid from base_dataitem where  parentid = 
                (select itemid from base_dataitem where itemname = '单位内部快报' ) and  itemcode = 'AccidentEventProperty') ) c on a.accidenteventproperty = c.itemvalue
                left join (select recid,flowid,outtransferuseraccount,intransferuseraccount,outtransferusername,intransferusername,row_number()  over(partition by recid,flowid order by createdate desc) as num from BIS_TRANSFERRECORD where disable=0) e on d.id=e.recid and e.num=1";
                pagination.conditionJson = "1=1";
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
                var data = powerplantreformbll.GetPageList(pagination, queryJson);
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
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = powerplantreformbll.GetList(queryJson);
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
            var data = powerplantreformbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        ///// <summary>
        ///// 获取实体 
        ///// </summary>
        ///// <param name="keyValue">处理信息主键值</param>
        ///// <returns>返回对象Json</returns>
        //[HttpGet]
        //public ActionResult GetFormJsonByHandleDetailId(string keyValue)
        //{
        //    try
        //    {
        //        var reformlist = powerplantreformbll.GetList("").Where(t => t.PowerPlantHandleDetailId == keyValue && t.Disable == 0).ToList();
        //        if (reformlist.Count > 0)
        //        {
        //            var data = powerplantreformbll.GetEntity(reformlist[0].Id);
        //            return ToJsonResult(data);
        //        }
        //        else
        //        {
        //            return Error("系统错误，请联系系统管理员处理！");
        //        }
                
        //    }
        //    catch (Exception ex)
        //    {
        //        return Error(ex.ToString());
        //    }
            
        //}


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
                pagination.p_kid = "id";
                pagination.p_fields = @"powerplanthandleid,powerplanthandledetailid,rectificationperson,rectificationpersonsignimg,rectificationsituation,rectificationendtime";
                pagination.p_tablename = @"bis_powerplantreform";
                pagination.conditionJson = string.Format("POWERPLANTHANDLEDETAILID='{0}' and disable=1", PowerInsideHandleDetailId);



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
        [HandlerMonitor(6, "事故事件处理整改删除")]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            powerplantreformbll.RemoveForm(keyValue);
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
        [HandlerMonitor(5, "事故事件处理整改保存")]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, PowerplantreformEntity entity)
        {
            try
            {
                PowerplanthandledetailEntity powerplanthandledetailentity = powerplanthandledetailbll.GetEntity(entity.PowerPlantHandleDetailId);
                if (powerplanthandledetailentity != null)
                {
                    Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
                    powerplanthandledetailentity.RealReformDept = curUser.DeptName;
                    powerplanthandledetailentity.RealReformDeptId = curUser.DeptId;
                    powerplanthandledetailentity.RealReformDeptCode = curUser.DeptCode;
                    string state = string.Empty;

                    string moduleName = "事故事件处理记录-验收";

                    /// <param name="currUser">当前登录人</param>
                    /// <param name="state">是否有权限审核 1：能审核 0 ：不能审核</param>
                    /// <param name="moduleName">模块名称</param>
                    /// <param name="outengineerid">工程Id</param>
                    ManyPowerCheckEntity mpcEntity = powerplanthandlebll.CheckAuditPower(curUser, out state, moduleName, curUser.DeptId);

                    string flowid = string.Empty;
                    List<ManyPowerCheckEntity> powerList = new ManyPowerCheckBLL().GetListBySerialNum(curUser.OrganizeCode, moduleName);
                    List<ManyPowerCheckEntity> checkPower = new List<ManyPowerCheckEntity>();
                    foreach (var item in powerList)
                    {
                        if (item.CHECKDEPTID == "-3" || item.CHECKDEPTID == "-1")
                        {
                            item.CHECKDEPTID = curUser.DeptId;
                            item.CHECKDEPTCODE = curUser.DeptCode;
                            item.CHECKDEPTNAME = curUser.DeptName;
                        }
                    }
                    for (int i = 0; i < powerList.Count; i++)
                    {
                        if (powerList[i].CHECKDEPTID == curUser.DeptId)
                        {
                            var rolelist = curUser.RoleName.Split(',');
                            for (int j = 0; j < rolelist.Length; j++)
                            {
                                if (powerList[i].CHECKROLENAME.Contains(rolelist[j]))
                                {
                                    checkPower.Add(powerList[i]);
                                    break;
                                }
                            }
                        }
                    }
                    if (checkPower.Count > 0)
                    {
                        ManyPowerCheckEntity check = checkPower.Last();//当前

                        for (int i = 0; i < powerList.Count; i++)
                        {
                            if (check.ID == powerList[i].ID)
                            {
                                flowid = powerList[i].ID;
                            }
                        }
                    }
                    if (null != mpcEntity)
                    {
                        powerplanthandledetailentity.FlowDept = mpcEntity.CHECKDEPTID;
                        powerplanthandledetailentity.FlowDeptName = mpcEntity.CHECKDEPTNAME;
                        powerplanthandledetailentity.FlowRole = mpcEntity.CHECKROLEID;
                        powerplanthandledetailentity.FlowRoleName = mpcEntity.CHECKROLENAME;
                        powerplanthandledetailentity.ApplyState = 4; //流程未完成，1表示完成
                        powerplanthandledetailentity.FlowId = mpcEntity.ID;
                        powerplanthandledetailentity.FlowName = mpcEntity.CHECKDEPTNAME + "验收中";
                    }
                    else  //为空则表示已经完成流程
                    {
                        powerplanthandledetailentity.FlowDept = "";
                        powerplanthandledetailentity.FlowDeptName = "";
                        powerplanthandledetailentity.FlowRole = "";
                        powerplanthandledetailentity.FlowRoleName = "";
                        powerplanthandledetailentity.ApplyState = 5; //流程未完成，1表示完成
                        powerplanthandledetailentity.FlowName = "";
                        powerplanthandledetailentity.FlowId = "";
                    }
                    entity.RectificationPerson = curUser.UserName;
                    entity.RectificationPersonId = curUser.UserId;
                    entity.Disable = 0;
                    entity.RectificationPersonSignImg = string.IsNullOrWhiteSpace(entity.RectificationPersonSignImg) ? "" : entity.RectificationPersonSignImg.ToString().Replace("../..", "");
                    powerplantreformbll.SaveForm(keyValue, entity);
                    powerplanthandledetailbll.SaveForm(powerplanthandledetailentity.Id, powerplanthandledetailentity);
                    powerplanthandlebll.UpdateApplyStatus(entity.PowerPlantHandleId);

                    //添加验收信息
                    if (state == "1")
                    {
                        //验收信息
                        PowerplantcheckEntity checkEntity = new PowerplantcheckEntity();
                        checkEntity.AuditResult = 0; //通过
                        checkEntity.AuditTime = DateTime.Now;
                        checkEntity.AuditPeople = curUser.UserName;
                        checkEntity.AuditPeopleId = curUser.UserId;
                        checkEntity.PowerPlantHandleId = entity.PowerPlantHandleId;
                        checkEntity.PowerPlantHandleDetailId = entity.PowerPlantHandleDetailId;
                        checkEntity.PowerPlantReformId = keyValue;
                        checkEntity.AuditOpinion = ""; //审核意见
                        checkEntity.AuditSignImg = string.IsNullOrWhiteSpace(entity.RectificationPersonSignImg) ? "" : entity.RectificationPersonSignImg.ToString().Replace("../..", "");
                        checkEntity.FlowId = flowid;
                        if (null != mpcEntity)
                        {
                            checkEntity.Remark = (mpcEntity.AUTOID.Value - 1).ToString(); //备注 存流程的顺序号
                        }
                        else
                        {
                            checkEntity.Remark = "7";
                        }
                        checkEntity.AuditDeptId = curUser.DeptId;
                        checkEntity.AuditDept = curUser.DeptName;
                        checkEntity.Disable = 0;
                        powerplantcheckbll.SaveForm(checkEntity.Id, checkEntity);
                    }
                    powerplantreformbll.SaveForm(keyValue, entity);
                    return Success("操作成功。");
                }
                else
                {
                    return Error("系统错误,请联系系统管理员");
                }
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
        }
        #endregion
    }
}
