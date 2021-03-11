using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util.Extension;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.TrainPlan;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.IService.TrainPlan;
using ERCHTMS.Service.BaseManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Service.TrainPlan
{
    public class SafeAdjustmentService : RepositoryFactory<SafeAdjustmentEntity>, ISafeAdjustmentService
    {

        private DepartmentService deptservice = new DepartmentService();

        /// <summary>
        /// 安措计划调整申请
        /// </summary>
        /// <param name="entity"></param>
        public bool InsertAdjustment(SafeAdjustmentEntity entity)
        {
            if (entity.ID.IsEmpty())
            {
                entity.ID = Guid.NewGuid().ToString();
            }
            entity.CreateDate = DateTime.Now;
           return this.BaseRepository().Insert(entity)>0?true:false;
        }

        public void UpdateAdjustment(SafeAdjustmentEntity entity)
        {
           this.BaseRepository().Update(entity);
        }

        public SafeAdjustmentEntity GetAdjustInfo(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public SafeAdjustmentEntity GetEntity(string keyValue)
        {
            string sql =string.Format(@"select * from (select ID,ApplyReason,ISDELAY,DELAYDAYS,ISADJUSTFEE,ADJUSTFEE,CREATEUSERID,CREATEUSERNAME,APPLYDEPTID,APPLYDEPTNAME,CREATEDATE,SAFEMEASUREID  
from BIS_SAFEMEASURE_ADJUSTMENT where SAFEMEASUREID='{0}' order by CREATEDATE desc) where rownum<2 ", keyValue);
            return this.BaseRepository().FindList(sql).FirstOrDefault();
        }

        /// <summary>
        /// 当前登录人是否有权限审核并获取下一次审核权限实体
        /// </summary>
        /// <param name="currUser">当前登录人</param>
        /// <param name="state">是否有权限审核 1：能审核 0 ：不能审核</param>
        /// <param name="moduleName">模块名称</param>
        /// <param name="createdeptid">创建人部门ID</param>
        /// <returns>null-当前为最后一次审核,ManyPowerCheckEntity：下一次审核权限实体</returns>
        public ManyPowerCheckEntity CheckAuditPower(Operator currUser, out string state, string moduleName, string createdeptid,out string curFlowId)
        {
            ManyPowerCheckEntity nextCheck = null;//下一步审核
            IManyPowerCheckService powerCheck = new ManyPowerCheckService();
            //获取流程节点配置
            List<ManyPowerCheckEntity> powerList = powerCheck.GetListBySerialNum(currUser.OrganizeCode, moduleName);
            if (powerList.Count > 0)
            {
                DepartmentEntity department= new DepartmentService().GetEntity(createdeptid);
                foreach (var item in powerList)
                {
                    if (item.CHECKDEPTCODE == "-3" || item.CHECKDEPTID == "-3")
                    {
                        string executedept = department.DepartmentId;
                        //powerList[i].CHECKDEPTCODE = new DepartmentService().GetEntity(ourEngineer.FindEntity(outengineerid).ENGINEERLETDEPTID).EnCode;
                        //powerList[i].CHECKDEPTID = new DepartmentService().GetEntity(ourEngineer.FindEntity(outengineerid).ENGINEERLETDEPTID).DepartmentId;
                        switch (item.ChooseDeptRange) //判断部门范围
                        {
                            case "0":
                                item.CHECKDEPTID = executedept;
                                item.CHECKDEPTNAME = department.FullName;
                                break;
                            case "1":
                                var dept = deptservice.GetEntity(executedept);
                                while (dept.Nature != "部门" && dept.Nature!="厂级")
                                {
                                    dept = deptservice.GetEntity(dept.ParentId);
                                }
                                item.CHECKDEPTID = dept.DepartmentId;
                                item.CHECKDEPTNAME = dept.FullName;
                                break;
                            case "2":
                                var dept1 = deptservice.GetEntity(executedept);
                                while (dept1.Nature != "部门" && dept1.Nature != "厂级")
                                {
                                    dept1 = deptservice.GetEntity(dept1.ParentId);
                                }
                                item.CHECKDEPTID = (dept1.DepartmentId + "," + executedept).Trim(',');
                                item.CHECKDEPTNAME = (dept1.FullName + "," + department.FullName).Trim();
                                break;
                            default:
                                item.CHECKDEPTID = executedept;
                                item.CHECKDEPTNAME = department.FullName;
                                break;
                        }

                        ////查询 执行部门
                        //item.CHECKDEPTCODE = new DepartmentService().GetEntity(createdeptid).EnCode;
                        //item.CHECKDEPTID = new DepartmentService().GetEntity(createdeptid).DepartmentId;
                    }
                }

                List<ManyPowerCheckEntity> checkPower = new List<ManyPowerCheckEntity>();

                //登录人是否有审核权限--有审核权限直接审核通过
                for (int i = 0; i < powerList.Count; i++)
                {
                    if (powerList[i].CHECKDEPTID == currUser.DeptId)
                    {
                        var rolelist = currUser.RoleName.Split(',');
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

                powerList.GroupBy(t => t.SERIALNUM).ToList().Count();
                if (checkPower.Count > 0)
                {
                    state = "1";
                    ManyPowerCheckEntity check = checkPower.Last();//当前
                    curFlowId = check.ID;
                    for (int i = 0; i < powerList.Count; i++)
                    {
                        if (check.ID == powerList[i].ID)
                        {
                            if ((i + 1) >= powerList.Count)
                            {
                                nextCheck = null;
                            }
                            else
                            {
                                nextCheck = powerList[i + 1];
                            }
                        }
                    }
                }
                else
                {
                    state = "0";
                    curFlowId = "";
                    nextCheck = powerList.First();
                }

                if (nextCheck != null)
                {
                    //当前审核序号下的对应集合
                    var serialList = powerList.Where(p => p.SERIALNUM == nextCheck.SERIALNUM);
                    //集合记录大于1，则表示存在并行审核（审查）的情况
                    if (serialList.Count() > 1)
                    {
                        ManyPowerCheckEntity slastEntity = new ManyPowerCheckEntity();
                        slastEntity = serialList.LastOrDefault();
                        nextCheck = slastEntity;
                    }
                }

                return nextCheck;
            }
            else
            {
                state = "0";
                curFlowId = "";
                return nextCheck;
            }

        }

        /// <summary>
        /// 获取调整申请/审批记录
        /// </summary>
        /// <param name="measureId"></param>
        /// <returns></returns>
        public DataTable GetAdjustList(string measureId)
        {
            string sql = "select a.ID,A.DELAYDAYS,a.AdjustStauts as Stauts,b.iscommit,a.CREATEUSERNAME as ApplyUserName,to_char(a.CREATEDATE,'yyyy-MM-dd') as ApplyDate,b.flowrolename,b.flowdept,a.PROCESSSTATE,'' as approveusernames from BIS_SAFEMEASURE_ADJUSTMENT a inner join BIS_SAFEMEASURE b on a.SAFEMEASUREID=b.id where b.id=:SafeMeasureId order by a.CREATEDATE desc";

           return this.BaseRepository().FindTable(sql, new DbParameter[] { DbParameters.CreateDbParameter(":SafeMeasureId", measureId) });
        }

        /// <summary>
        /// 删除调整信息
        /// </summary>
        /// <param name="measureId"></param>
        public void DeleteAdjustment(string measureId)
        {
            string sql = "delete from BIS_SAFEMEASURE_ADJUSTMENT where SAFEMEASUREID=:SAFEMEASUREID";
            DbParameter[] dbParameters = {
                 DbParameters.CreateDbParameter(":SAFEMEASUREID", measureId),
            };
            this.BaseRepository().ExecuteBySql(sql, dbParameters);
        }
    }
}
