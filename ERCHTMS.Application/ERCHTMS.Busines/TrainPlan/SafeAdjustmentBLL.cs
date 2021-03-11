using BSFramework.Util.WebControl;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Entity.TrainPlan;
using ERCHTMS.IService.TrainPlan;
using ERCHTMS.Service.TrainPlan;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Busines.TrainPlan
{
    public class SafeAdjustmentBLL
    {
        private ISafeAdjustmentService service = new SafeAdjustmentService();
        private ISafeMeasureService safeMeasure = new SafeMeasureService();



        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public SafeAdjustmentEntity GetAdjustInfo(string keyValue, string mode, string adjustId)
        {
            if (!string.IsNullOrEmpty(mode) && mode.Equals("show"))
            {
                return service.GetAdjustInfo(adjustId);
            }
            else
            {
                return service.GetEntity(keyValue);
            }
        }


        public void SubmitForm(SafeAdjustmentEntity adjustmentEntity)
        {
            adjustmentEntity.ID = Guid.NewGuid().ToString();
            Operator curUser = OperatorProvider.Provider.Current();
            string flowid = string.Empty;
            string status = string.Empty;
            string curFlowId = string.Empty;
            SafeMeasureEntity entity = safeMeasure.GetEntity(adjustmentEntity.SafeMeasureId);
            ManyPowerCheckEntity mpcEntity = service.CheckAuditPower(curUser, out status, "安措计划调整审批", curUser.DeptId, out curFlowId);


            if (mpcEntity != null)
            {
                //如果当前登录人是审核人，直接审核通过
                if (status.Equals("1"))
                {
                    AptitudeinvestigateauditEntity aentity = new AptitudeinvestigateauditEntity();
                    aentity.Create();
                    aentity.AUDITPEOPLEID = curUser.UserId;
                    aentity.AUDITPEOPLE = curUser.UserName;
                    aentity.AUDITDEPTID = curUser.DeptId;
                    aentity.AUDITDEPT = curUser.DeptName;
                    aentity.APTITUDEID = entity.Id;
                    aentity.APPLYID = adjustmentEntity.ID;
                    aentity.FlowId = curFlowId;//mpcEntity.ID;
                    aentity.AUDITTIME = DateTime.Now;
                    aentity.AUDITRESULT = "0";
                    aentity.Disable = "0";
                    new AptitudeinvestigateauditBLL().SaveForm("", aentity);
                    if (mpcEntity.CHECKROLENAME.Equals("公司领导"))
                    {
                        entity.Stauts = mpcEntity.CHECKROLENAME ;
                        adjustmentEntity.AdjustStauts = mpcEntity.CHECKROLENAME ;
                    }
                    else
                    {
                        entity.Stauts = mpcEntity.CHECKDEPTNAME + mpcEntity.CHECKROLENAME;
                        adjustmentEntity.AdjustStauts = mpcEntity.CHECKDEPTNAME + mpcEntity.CHECKROLENAME;
                    }
                    entity.ProcessState = 2;
                    adjustmentEntity.ProcessState = 2;
                }
                else
                {
                    if (mpcEntity.CHECKROLENAME.Equals("公司领导"))
                    {
                        entity.Stauts = mpcEntity.CHECKROLENAME;
                        adjustmentEntity.AdjustStauts = mpcEntity.CHECKROLENAME;
                    }
                    else
                    {
                        entity.Stauts = curUser.DeptName + mpcEntity.CHECKROLENAME;
                        adjustmentEntity.AdjustStauts = curUser.DeptName + mpcEntity.CHECKROLENAME;
                    }
                    entity.ProcessState = 1;
                    adjustmentEntity.ProcessState = 1;
                }
                entity.FlowId = mpcEntity.ID;
                entity.FlowName = mpcEntity.FLOWNAME;
                entity.FlowRole = mpcEntity.CHECKROLEID;
                entity.FlowRoleName = mpcEntity.CHECKROLENAME;
                entity.FlowDept = mpcEntity.CHECKDEPTID;
                entity.FlowDeptName = mpcEntity.CHECKDEPTNAME;
                entity.IsCommit = "1";
                entity.IsOver = 0;

            }
            else
            {
                entity.FlowId = "";
                entity.FlowName = "";
                entity.FlowRole = "";
                entity.FlowRoleName = "";
                entity.FlowDept = "";
                entity.FlowDeptName = "";
                entity.IsCommit = "1";
                entity.IsOver = 1;
                entity.Stauts = "审批通过";
                entity.ProcessState = 3;
                adjustmentEntity.ProcessState = 3;
                //审批通过，更新调整费用和调整时间
                if (adjustmentEntity.IsAdjustFee == 1)
                {
                    entity.Cost =Convert.ToDouble(adjustmentEntity.AdjustFee);
                }
                if (adjustmentEntity.IsDelay == 1)
                {
                    entity.PlanFinishDate = entity.PlanFinishDate.Value.AddDays(Convert.ToDouble(adjustmentEntity.DelayDays));
                }
                adjustmentEntity.AdjustStauts = "审批通过";
            }
            service.InsertAdjustment(adjustmentEntity);
            safeMeasure.UpdateForm(entity.Id, entity);
        }

        public bool ApproveForm(string keyValue, string applyId, AptitudeinvestigateauditEntity aentity)
        {
            SafeMeasureEntity entity = safeMeasure.GetEntity(keyValue);
            Operator curUser = OperatorProvider.Provider.Current();
            string status = "";
            string curFlowId = string.Empty;
            aentity.AUDITPEOPLEID = curUser.UserId;
            aentity.AUDITDEPTID = curUser.DeptId;
            aentity.APTITUDEID = keyValue;
            aentity.APPLYID = applyId;
            aentity.FlowId = entity.FlowId;
            ManyPowerCheckEntity mpcEntity = service.CheckAuditPower(curUser, out status, "安措计划调整审批", curUser.DeptId, out curFlowId);
            aentity.Disable = aentity.AUDITRESULT == "1" ? "1" : "0";

            SafeAdjustmentEntity adjustmentEntity = service.GetEntity(entity.Id);
            if (mpcEntity != null)
            {
                if (status == "0")
                {
                    return false;
                }
                else
                {
                    aentity.Create();
                    new AptitudeinvestigateauditBLL().SaveForm("", aentity);
                    entity.FlowId = mpcEntity.ID;
                    entity.FlowRoleName = mpcEntity.CHECKROLENAME;
                    entity.FlowDept = mpcEntity.CHECKDEPTID;
                    entity.FlowDeptName = mpcEntity.CHECKDEPTNAME;
                    entity.IsCommit = "1";
                    if (aentity.AUDITRESULT == "1")
                    {
                        //调整审批不通过
                        entity.FlowName = "审批不通过";
                        entity.Stauts = "审批不通过";
                        adjustmentEntity.AdjustStauts = "审批不通过";
                        entity.IsOver = 1;
                    }
                    else
                    {
                        entity.FlowName = mpcEntity.FLOWNAME;
                        entity.IsOver = 0;
                        if (mpcEntity.CHECKROLENAME.Equals("公司领导"))
                        {
                            entity.Stauts = mpcEntity.CHECKROLENAME;
                            adjustmentEntity.AdjustStauts = mpcEntity.CHECKROLENAME;
                        }
                        else
                        {
                            entity.Stauts = mpcEntity.CHECKDEPTNAME + mpcEntity.CHECKROLENAME;
                            adjustmentEntity.AdjustStauts = mpcEntity.CHECKDEPTNAME + mpcEntity.CHECKROLENAME;
                        }
                    }
                    entity.ProcessState = 2;
                    adjustmentEntity.ProcessState = 2;
                    safeMeasure.UpdateForm(keyValue, entity);
                    service.UpdateAdjustment(adjustmentEntity);
                    return true;
                }
            }
            else
            {
                if (status == "0")
                {
                    return false;
                }
                else
                {
                    aentity.Create();
                    new AptitudeinvestigateauditBLL().SaveForm("", aentity);
                    entity.FlowId = "";
                    entity.FlowRoleName = "";
                    entity.FlowDept = "";
                    entity.FlowDeptName = "";
                    entity.IsOver = 1;
                    entity.IsCommit = "1";
                    entity.ProcessState = 3;
                    adjustmentEntity.ProcessState = 3;
                    entity.Stauts = aentity.AUDITRESULT == "1" ? "审批不通过" : "审批通过";
                    if (aentity.AUDITRESULT == "0")
                    {
                        //审批通过，更新调整费用和调整时间

                        if (adjustmentEntity != null)
                        {
                            if (adjustmentEntity.IsAdjustFee == 1)
                            {
                                entity.Cost =Convert.ToDouble( adjustmentEntity.AdjustFee);
                            }
                            if (adjustmentEntity.IsDelay == 1)
                            {
                                entity.PlanFinishDate = entity.PlanFinishDate.Value.AddDays(Convert.ToDouble(adjustmentEntity.DelayDays));
                            }
                        }
                    }
                    safeMeasure.UpdateForm(keyValue, entity);
                    adjustmentEntity.AdjustStauts = aentity.AUDITRESULT == "1" ? "审批不通过" : "审批通过";
                    service.UpdateAdjustment(adjustmentEntity);
                    return true;
                }
            }
        }


        /// <summary>
        /// 获取调整申请/审批记录
        /// </summary>
        /// <param name="measureId"></param>
        /// <returns></returns>
        public DataTable GetAdjustList(string measureId)
        {
            return service.GetAdjustList(measureId);
        }


    }
}