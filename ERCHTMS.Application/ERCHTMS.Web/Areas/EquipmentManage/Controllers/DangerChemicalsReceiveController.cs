using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.EquipmentManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity;
using ERCHTMS.Entity.SystemManage.ViewModel;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.EquipmentManage;
using ERCHTMS.Entity.OutsourcingProject;
using Newtonsoft.Json;
using BSFramework.Util.Offices;
using Aspose.Cells;
using System.Drawing;
using System.Web;

namespace ERCHTMS.Web.Areas.EquipmentManage.Controllers
{
    /// <summary>
    /// 描 述：EHS计划申请表
    /// </summary>
    public class DangerChemicalsReceiveController : MvcControllerBase
    {
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL(); //流程业务对象
        private UserBLL userbll = new UserBLL(); //用户操作对象
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private DangerChemicalsReceiveBLL DangerChemicalsReceiveBll = new DangerChemicalsReceiveBLL();

        private OutsouringengineerBLL outsouringengineerbll = new OutsouringengineerBLL();
        private AptitudeinvestigateauditBLL aptitudeinvestigateauditbll = new AptitudeinvestigateauditBLL();

        private DangerChemicalsBLL DangerChemicalsBll = new DangerChemicalsBLL();
        private DailyexamineBLL dailyexaminebll = new DailyexamineBLL();
        private AptitudeinvestigateinfoBLL aptitudeinvestigateinfobll = new AptitudeinvestigateinfoBLL();


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
        /// 获取列表(领用)
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.p_fields = @"t.createdate,t.createuserid,t.createuserdeptcode,t.createuserorgcode,t.modifydate,t.modifyuserid,
t.MainId,t.Purpose,t.ReceiveNum,t.ReceiveUnit,t.ReceiveUserId,t.ReceiveUser,t.SignImg,t.GrantState,t.FLOWNAME,t.FLOWROLE,t.FLOWROLENAME,t.FLOWDEPT,t.FLOWDEPTNAME,t.ISSAVED,t.ISOVER,t.FlowId,
t.Name,t.Specification,t.SpecificationUnit,t.Inventory,t.Unit,t.RiskType,t.Amount,t.AmountUnit,t.Site,t1.GrantPersonId,t1.GrantPerson,t.GrantUser";
            pagination.p_kid = "t.id";
            pagination.p_tablename = @"XLD_DANGEROUSCHEMICALRECEIVE t left join XLD_DANGEROUSCHEMICAL t1 on t.mainid=t1.id";
            //pagination.sidx = "createdate";//排序字段
            //pagination.sord = "desc";//排序方式  
            if (curUser.RoleName.Contains("省级用户") || curUser.RoleName.Contains("集团用户"))
            {
                pagination.conditionJson = string.Format(" t.GrantState=3 ");
            }
            else
            {
                pagination.conditionJson = string.Format(" 1=1 ");
            }


            var watch = CommonHelper.TimerStart();            
            var data = DangerChemicalsReceiveBll.GetList(pagination, queryJson);
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
            var data = DangerChemicalsReceiveBll.GetEntity(keyValue);
            Operator curUser = OperatorProvider.Provider.Current();
            var dataCheck = aptitudeinvestigateinfobll.GetAppCheckFlowList(keyValue, "1", curUser);
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
            DangerChemicalsReceiveBll.RemoveForm(keyValue);//删除申请
            htworkflowbll.DeleteWorkFlowObj(keyValue);//删除流程

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
        public ActionResult SaveForm(string keyValue, DangerChemicalsReceiveEntity entity)
        {
            entity.ISSAVED = "0"; //标记申请中
            DangerChemicalsReceiveBll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        /// <summary>
        /// 发放确认
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult GrantForm(string keyValue, DangerChemicalsReceiveEntity entity)
        {
            try
            {
                //更新主表库存
                var clEntity = DangerChemicalsBll.GetEntity(entity.MainId);
                if (entity.GrantUnit == clEntity.Unit)
                {
                    if (Convert.ToDecimal(entity.GrantNum) <= Convert.ToDecimal(clEntity.Inventory))
                    {
                        clEntity.Inventory = (Convert.ToDecimal(clEntity.Inventory) - Convert.ToDecimal(entity.GrantNum)).ToString();
                        clEntity.Amount= (Convert.ToDecimal(clEntity.Inventory) / Convert.ToDecimal(clEntity.Specification)).ToString("#0.00");
                        DangerChemicalsBll.SaveForm(entity.MainId, clEntity);
                        entity.PracticalNum = Convert.ToDecimal(entity.GrantNum).ToString(); //实际发放库存量
                    }
                    else
                    {
                        return Error("操作失败，该危化品实际库存已变化，库存为：" + clEntity.Inventory + " " + clEntity.Unit + "！");
                    }
                }
                if (entity.GrantUnit == clEntity.AmountUnit)
                {
                    if (Convert.ToDecimal(entity.GrantNum) <= Convert.ToDecimal(clEntity.Amount))
                    {
                        clEntity.Inventory = (Convert.ToDecimal(clEntity.Inventory) - (Convert.ToDecimal(entity.GrantNum) * Convert.ToDecimal(clEntity.Specification))).ToString();
                        clEntity.Amount = (Convert.ToDecimal(clEntity.Inventory) / Convert.ToDecimal(clEntity.Specification)).ToString("#0.00");
                        DangerChemicalsBll.SaveForm(entity.MainId, clEntity);
                        entity.PracticalNum = (Convert.ToDecimal(entity.GrantNum) * Convert.ToDecimal(clEntity.Specification)).ToString(); //实际发放库存量
                    }
                    else
                    {
                        return Error("操作失败，该危化品实际库存已变化，库存为：" + clEntity.Inventory + " " + clEntity.Unit + "！");
                    }
                }
            }
            catch { }
            entity.GrantState = 3; //标记发放完成
            DangerChemicalsReceiveBll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #region 登记的内容提交到审核或者结束
        /// <summary>
        /// 登记的内容提交到审核或者结束（提交到下一个流程）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, DangerChemicalsReceiveEntity entity)
        {
            var clEntity = DangerChemicalsBll.GetEntity(entity.MainId);
            if (clEntity.IsScene == "现场存放")
            {
                try
                {
                    if (entity.ReceiveUnit == clEntity.Unit)
                    {
                        if (Convert.ToDecimal(entity.ReceiveNum) <= Convert.ToDecimal(clEntity.Inventory))
                        {
                            clEntity.Inventory = (Convert.ToDecimal(clEntity.Inventory) - Convert.ToDecimal(entity.ReceiveNum)).ToString();
                            clEntity.Amount = (Convert.ToDecimal(clEntity.Inventory) / Convert.ToDecimal(clEntity.Specification)).ToString("#0.00");
                            DangerChemicalsBll.SaveForm(entity.MainId, clEntity);
                            entity.PracticalNum = Convert.ToDecimal(entity.ReceiveNum).ToString(); //实际发放库存量
                        }
                        else
                        {
                            return Error("操作失败，该危化品实际库存已变化，库存为：" + clEntity.Inventory + " " + clEntity.Unit + "！");
                        }
                    }
                    if (entity.ReceiveUnit == clEntity.AmountUnit)
                    {
                        if (Convert.ToDecimal(entity.ReceiveNum) <= Convert.ToDecimal(clEntity.Amount))
                        {
                            clEntity.Inventory = (Convert.ToDecimal(clEntity.Inventory) - (Convert.ToDecimal(entity.ReceiveNum) * Convert.ToDecimal(clEntity.Specification))).ToString();
                            clEntity.Amount = (Convert.ToDecimal(clEntity.Inventory) / Convert.ToDecimal(clEntity.Specification)).ToString("#0.00");
                            DangerChemicalsBll.SaveForm(entity.MainId, clEntity);
                            entity.PracticalNum = (Convert.ToDecimal(entity.ReceiveNum) * Convert.ToDecimal(clEntity.Specification)).ToString(); //实际发放库存量
                        }
                        else
                        {
                            return Error("操作失败，该危化品实际库存已变化，库存为：" + clEntity.Inventory + " " + clEntity.Unit + "！");
                        }
                    }
                }
                catch { }

                entity.FLOWDEPT = "";
                entity.FLOWDEPTNAME = "";
                entity.FLOWROLE = "";
                entity.FLOWROLENAME = "";
                entity.ISSAVED = "1"; //标记已经从登记到审核阶段
                entity.ISOVER = "1"; //流程未完成，1表示完成
                entity.GrantState = 3; //2表示发放中
                entity.FLOWNAME = "";
                entity.FlowId = "";
                entity.GrantDate = DateTime.Now;

                DangerChemicalsReceiveBll.SaveForm(keyValue, entity);
            }
            else
            {
                Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

                string state = string.Empty;

                string flowid = string.Empty;

                string moduleName = "危化品领用";

                // <param name="state">是否有权限审核 1：能审核 0 ：不能审核</param>
                ManyPowerCheckEntity mpcEntity = dailyexaminebll.CheckAuditPower(curUser, out state, moduleName, curUser.DeptId);

                //新增时会根据角色自动审核
                List<ManyPowerCheckEntity> powerList = new ManyPowerCheckBLL().GetListBySerialNum(curUser.OrganizeCode, "危化品领用");
                List<ManyPowerCheckEntity> checkPower = new List<ManyPowerCheckEntity>();
                //先查出执行部门编码
                for (int i = 0; i < powerList.Count; i++)
                {
                    if (powerList[i].CHECKDEPTCODE == "-1" || powerList[i].CHECKDEPTID == "-1")
                    {
                        var createdeptentity = new DepartmentBLL().GetEntityByCode(curUser.DeptCode);
                        while (createdeptentity.Nature == "专业" || createdeptentity.Nature == "班组")
                        {
                            createdeptentity = new DepartmentBLL().GetEntity(createdeptentity.ParentId);
                        }
                        powerList[i].CHECKDEPTCODE = createdeptentity.DeptCode;
                        powerList[i].CHECKDEPTID = createdeptentity.DepartmentId;
                    }
                    //创建部门
                    if (powerList[i].CHECKDEPTCODE == "-3" || powerList[i].CHECKDEPTID == "-3")
                    {
                        var createdeptentity = new DepartmentBLL().GetEntityByCode(curUser.DeptCode);
                        while (createdeptentity.Nature == "专业" || createdeptentity.Nature == "班组")
                        {
                            createdeptentity = new DepartmentBLL().GetEntity(createdeptentity.ParentId);
                        }
                        powerList[i].CHECKDEPTCODE = createdeptentity.DeptCode;
                        powerList[i].CHECKDEPTID = createdeptentity.DepartmentId;
                    }
                }
                //登录人是否有审核权限--有审核权限直接审核通过
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
                else
                {
                    if (powerList.Count > 0)
                    {
                        mpcEntity = powerList.First();
                    }
                    
                }
                if (null != mpcEntity)
                {
                    //保存三措两案记录
                    entity.FLOWDEPT = mpcEntity.CHECKDEPTID;
                    entity.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                    entity.FLOWROLE = mpcEntity.CHECKROLEID;
                    entity.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                    entity.ISSAVED = "1"; //标记已经从登记到审核阶段
                    entity.ISOVER = "0"; //流程未完成，1表示完成
                    entity.FLOWNAME = mpcEntity.CHECKDEPTNAME + "审核中";
                    entity.FlowId = mpcEntity.ID;
                }
                else  //为空则表示已经完成流程
                {
                    entity.FLOWDEPT = "";
                    entity.FLOWDEPTNAME = "";
                    entity.FLOWROLE = "";
                    entity.FLOWROLENAME = "";
                    entity.ISSAVED = "1"; //标记已经从登记到审核阶段
                    entity.ISOVER = "1"; //流程未完成，1表示完成
                    entity.GrantState = 2; //2表示发放中
                    entity.FLOWNAME = "";
                    entity.FlowId = flowid;
                }

                DangerChemicalsReceiveBll.SaveForm(keyValue, entity);


                //添加审核记录
                if (state == "1")
                {
                    //审核信息表
                    AptitudeinvestigateauditEntity aidEntity = new AptitudeinvestigateauditEntity();
                    aidEntity.AUDITRESULT = "0"; //通过
                    aidEntity.AUDITTIME = DateTime.Now;
                    aidEntity.AUDITPEOPLE = curUser.UserName;
                    aidEntity.AUDITPEOPLEID = curUser.UserId;
                    aidEntity.APTITUDEID = entity.ID;  //关联的业务ID 
                    aidEntity.AUDITOPINION = ""; //审核意见
                    aidEntity.AUDITSIGNIMG = curUser.SignImg;
                    if (null != mpcEntity)
                    {
                        aidEntity.REMARK = (powerList[0].AUTOID.Value - 1).ToString(); //备注 存流程的顺序号

                        //aidEntity.FlowId = mpcEntity.ID;
                    }
                    else
                    {
                        aidEntity.REMARK = "7";
                    }
                    aidEntity.FlowId = flowid;
                    aidEntity.AUDITDEPTID = curUser.DeptId;
                    aidEntity.AUDITDEPT = curUser.DeptName;

                    aptitudeinvestigateauditbll.SaveForm(aidEntity.ID, aidEntity);
                }
            }

            return Success("操作成功!");
        }
        #endregion

        #region 提交到审核或者结束
        /// <summary>
        /// 登记的内容提交到审核或者结束（提交到下一个流程）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult ApporveForm(string keyValue, DangerChemicalsReceiveEntity entity, AptitudeinvestigateauditEntity aentity)
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            string state = string.Empty;

            string moduleName = "危化品领用";


            /// <param name="currUser">当前登录人</param>
            /// <param name="state">是否有权限审核 1：能审核 0 ：不能审核</param>
            /// <param name="moduleName">模块名称</param>
            /// <param name="outengineerid">工程Id</param>
            //ManyPowerCheckEntity mpcEntity = peoplereviewbll.CheckAuditPower(curUser, out state, moduleName, outengineerid);
            ManyPowerCheckEntity mpcEntity = dailyexaminebll.CheckAuditPower(curUser, out state, moduleName, curUser.DeptId);

            #region //审核信息表
            AptitudeinvestigateauditEntity aidEntity = new AptitudeinvestigateauditEntity();
            aidEntity.AUDITRESULT = aentity.AUDITRESULT; //通过
            aidEntity.AUDITTIME = Convert.ToDateTime(aentity.AUDITTIME.Value.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss")); //审核时间
            aidEntity.AUDITPEOPLE = aentity.AUDITPEOPLE;  //审核人员姓名
            aidEntity.AUDITPEOPLEID = aentity.AUDITPEOPLEID;//审核人员id
            aidEntity.APTITUDEID = keyValue;  //关联的业务ID 
            aidEntity.AUDITDEPTID = aentity.AUDITDEPTID;//审核部门id
            aidEntity.AUDITDEPT = aentity.AUDITDEPT; //审核部门
            aidEntity.AUDITOPINION = aentity.AUDITOPINION; //审核意见
            aidEntity.FlowId = aentity.FlowId;
            aidEntity.AUDITSIGNIMG = string.IsNullOrWhiteSpace(aentity.AUDITSIGNIMG) ? "" : aentity.AUDITSIGNIMG.ToString().Replace("../..", "");
            if (null != mpcEntity)
            {
                aidEntity.REMARK = (mpcEntity.AUTOID.Value - 1).ToString(); //备注 存流程的顺序号
            }
            else
            {
                aidEntity.REMARK = "7";
            }
            aptitudeinvestigateauditbll.SaveForm(aidEntity.ID, aidEntity);
            #endregion

            #region  //保存危化品领用记录
            var smEntity = DangerChemicalsReceiveBll.GetEntity(keyValue);
            //审核通过
            if (aentity.AUDITRESULT == "0")
            {

                //0表示流程未完成，1表示流程结束
                if (null != mpcEntity)
                {
                    smEntity.FLOWDEPT = mpcEntity.CHECKDEPTID;
                    smEntity.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                    smEntity.FLOWROLE = mpcEntity.CHECKROLEID;
                    smEntity.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                    smEntity.ISSAVED = "1";
                    smEntity.ISOVER = "0";
                    smEntity.FlowId = mpcEntity.ID;//赋值流程Id
                    smEntity.FLOWNAME = mpcEntity.CHECKDEPTNAME + "审核中";
                }
                else
                {
                    smEntity.FLOWDEPT = "";
                    smEntity.FLOWDEPTNAME = "";
                    smEntity.FLOWROLE = "";
                    smEntity.FLOWROLENAME = "";
                    smEntity.ISSAVED = "1";
                    smEntity.ISOVER = "1";
                    smEntity.GrantState = 2; //流程未完成，2表示发放中
                    smEntity.FLOWNAME = "";
                }
            }
            else //审核不通过 
            {
                smEntity.FLOWDEPT = "";
                smEntity.FLOWDEPTNAME = "";
                smEntity.FLOWROLE = "";
                smEntity.FLOWROLENAME = "";
                smEntity.ISSAVED = "0"; //处于登记阶段
                smEntity.ISOVER = "0"; //是否完成状态赋值为未完成
                smEntity.FLOWNAME = "";
                smEntity.FlowId = "";//回退后流程Id清空
                //var applyUser = new UserBLL().GetEntity(smEntity.CREATEUSERID);
                //if (applyUser != null)
                //{
                //    JPushApi.PushMessage(applyUser.Account, smEntity.CREATEUSERNAME, "WB002", entity.ID);
                //}

            }
            //更新危化品领用基本状态信息
            DangerChemicalsReceiveBll.SaveForm(keyValue, smEntity);
            #endregion

            #region    //审核不通过
            if (aentity.AUDITRESULT == "1")
            {
                //获取当前业务对象的所有审核记录
                var shlist = aptitudeinvestigateauditbll.GetAuditList(keyValue);
                //批量更新审核记录关联ID
                foreach (AptitudeinvestigateauditEntity mode in shlist)
                {
                    //mode.APTITUDEID = hsentity.ID; //对应新的ID
                    mode.REMARK = "99";
                    aptitudeinvestigateauditbll.SaveForm(mode.ID, mode);
                }
            }
            #endregion

            return Success("操作成功!");
        }
        #endregion

        #endregion

        #region 数据导出
        /// <summary>
        /// 导出用户列表
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "导出领用记录")]
        public ActionResult Export(string queryJson)
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "rownum idx";
            pagination.p_fields = @"t.Name,(Concat(t.Specification,t.SpecificationUnit)) as Specification,
t.risktype,t.site,
(Concat(t.receivenum,t.receiveUnit)) as receivenum,t.receiveuser,t.grantuser,t.purpose";
            pagination.p_tablename = "XLD_DANGEROUSCHEMICALRECEIVE t  left join XLD_DANGEROUSCHEMICAL t1 on t.mainid=t1.id";
            
            //pagination.sidx = "createdate";//排序字段
            //pagination.sord = "desc";//排序方式  
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                pagination.conditionJson = string.Format(" CREATEUSERORGCODE ='{0}'", user.OrganizeCode);
            }

            var watch = CommonHelper.TimerStart();
            var data = DangerChemicalsReceiveBll.GetList(pagination, queryJson);

            HttpResponse resp = System.Web.HttpContext.Current.Response;

            // 详细列表内容
            string fielname = "危化品领用记录" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
            wb.Open(Server.MapPath("~/Resource/ExcelTemplate/tmp.xls"));
            Aspose.Cells.Worksheet sheet = wb.Worksheets[0] as Aspose.Cells.Worksheet;

            Aspose.Cells.Cell cell = sheet.Cells[0, 0];
            cell.PutValue("危化品领用记录"); //标题
            cell.Style.Pattern = BackgroundType.Solid;
            cell.Style.Font.Size = 16;
            cell.Style.Font.Color = Color.Black;

            //Aspose.Cells.Style style = wb.Styles[wb.Styles.Add()];
            //style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;//文字居中
            //style.IsTextWrapped = true;//自动换行
            //style.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
            //style.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
            //style.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
            //style.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;

            Aspose.Cells.Cells cells = sheet.Cells;
            cells.Merge(0, 0, 1, 9);

            sheet.Cells[1, 0].PutValue("序号");
            sheet.Cells[1, 1].PutValue("名称");
            sheet.Cells[1, 2].PutValue("规格");
            sheet.Cells[1, 3].PutValue("危险品类型");
            sheet.Cells[1, 4].PutValue("存放地点");
            sheet.Cells[1, 5].PutValue("领用数量");
            sheet.Cells[1, 6].PutValue("领用人");
            sheet.Cells[1, 7].PutValue("发放人");
            sheet.Cells[1, 8].PutValue("用途及使用说明");
            sheet.Cells.SetColumnWidthPixel(8, 250);

            int rowIndex = 2;
            foreach (DataRow row in data.Rows)
            {
                Aspose.Cells.Cell idxcell = sheet.Cells[rowIndex, 0];
                idxcell.PutValue(row["idx"]); 
                Aspose.Cells.Cell namexcell = sheet.Cells[rowIndex, 1];
                namexcell.PutValue(row["name"]); 
                Aspose.Cells.Cell specificationcell = sheet.Cells[rowIndex, 2];
                specificationcell.PutValue(row["specification"]); 
                Aspose.Cells.Cell risktypexcell = sheet.Cells[rowIndex, 3];
                risktypexcell.PutValue(row["risktype"]); 
                Aspose.Cells.Cell sitexcell = sheet.Cells[rowIndex, 4];
                sitexcell.PutValue(row["site"]); 
                Aspose.Cells.Cell receivenumxcell = sheet.Cells[rowIndex, 5];
                receivenumxcell.PutValue(row["receivenum"]); 
                Aspose.Cells.Cell receiveuserxcell = sheet.Cells[rowIndex, 6];
                receiveuserxcell.PutValue(row["receiveuser"]); 
                Aspose.Cells.Cell grantuserxcell = sheet.Cells[rowIndex, 7];
                grantuserxcell.PutValue(row["grantuser"]); 
                Aspose.Cells.Cell purposecell = sheet.Cells[rowIndex,8];
                purposecell.PutValue(row["purpose"]);

                rowIndex++;
            }
            //设置导出格式
            //ExcelConfig excelconfig = new ExcelConfig();
            //excelconfig.Title = "危化品领用记录";
            //excelconfig.TitleFont = "微软雅黑";
            //excelconfig.TitlePoint = 16;
            //excelconfig.FileName = "危化品领用记录.xls";
            //excelconfig.IsAllSizeColumn = true;
            ////每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            //List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            //excelconfig.ColumnEntity = listColumnEntity;
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "idx", ExcelColumn = "序号", Alignment = "center" });
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "name", ExcelColumn = "名称", Alignment = "center" });
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "specification", ExcelColumn = "规格", Alignment = "center" });
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "risktype", ExcelColumn = "危险品类型", Alignment = "center" });
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "site", ExcelColumn = "存放地点", Alignment = "center" });
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "receivenum", ExcelColumn = "领用数量", Alignment = "center" });
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "receiveuser", ExcelColumn = "领用人", Alignment = "center" });
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "grantuser", ExcelColumn = "发放人", Alignment = "center" });
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "purpose", ExcelColumn = "用途及使用说明", Alignment = "center" });
            ////调用导出方法
            //ExcelHelper.ExcelDownload(data, excelconfig);
            wb.Save(Server.UrlEncode(fielname), Aspose.Cells.FileFormatType.Excel2003, Aspose.Cells.SaveType.OpenInBrowser, resp);

            return Success("导出成功。");
        }
        #endregion
    }
}
