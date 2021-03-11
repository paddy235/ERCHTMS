using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using ERCHTMS.Service.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.IService.OutsourcingProject;
using ERCHTMS.Code;
using ERCHTMS.Service.OutsourcingProject;
using ERCHTMS.Entity.HiddenTroubleManage;

namespace ERCHTMS.Busines.HighRiskWork
{
    /// <summary>
    /// 描 述：起重吊装作业
    /// </summary>
    public class LifthoistjobBLL
    {
        private LifthoistjobIService service = new LifthoistjobService();
        private PeopleReviewIService peopleReviwservice = new PeopleReviewService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public DataTable GetList(Pagination page, LifthoistSearchModel search)
        {
            return service.GetList(page, search);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public DataTable getTempEquipentList(Pagination page, LifthoistSearchModel search)
        {
            return service.getTempEquipentList(page, search);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public LifthoistjobEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 得到流程图
        /// </summary>
        /// <param name="keyValue">业务表ID</param>
        /// <param name="modulename">逐级审核模块名</param>
        /// <returns></returns>
        public Flow GetFlow(string keyValue, string modulename)
        {
            return service.GetFlow(keyValue, modulename);
        }

        public string GetLifthoistjobNum()
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            Pagination pagination = new Pagination();
            pagination.conditionJson = "1=1 and a.auditstate=1";
            pagination.page = 1;
            pagination.rows = 1000000000;
            LifthoistSearchModel serch = new LifthoistSearchModel();
            serch.viewrange = "selfaudit";
            DataTable dt = service.GetList(pagination, serch);
            return dt.Rows.Count.ToString();
        }

        public List<CheckFlowData> GetAppFlowList(string keyValue, string modulename)
        {
            return service.GetAppFlowList(keyValue, modulename);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, LifthoistjobEntity entity)
        {
            try
            {
                var dbentity = service.GetEntity(keyValue);
                if (dbentity == null)
                {
                    //说明是新增
                    entity.ID = keyValue;
                    service.SaveForm(string.Empty, entity);
                }
                else
                {
                    //说明是修改
                    service.SaveForm(keyValue, entity);
                }
                //如果为1，说明是直接提交，则需要走审核
                if (entity.AUDITSTATE == 1)
                {
                    this.ApplyCheck(keyValue);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 起重吊装作业审核
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="auditEntity">审核实体</param>
        public void ApplyCheck(string keyValue, LifthoistauditrecordEntity auditEntity = null)
        {
            try
            {
                Operator currUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var jobEntity = service.GetEntity(keyValue);
                ManyPowerCheckEntity mpcEntity = null;
                if (jobEntity == null)
                {
                    throw new ArgumentException("无法找到当前业务信息，请确认业务ID是否有误！");
                }
                if (auditEntity != null)
                {
                    //把当前业务流程节点赋值到审核记录中
                    auditEntity.FLOWID = jobEntity.FLOWID;
                }
                //默认30T以下流程
                string moduleName = "(起重吊装作业30T以下)审核";
                if (jobEntity.QUALITYTYPE != "0")
                {
                    //30T以上流程
                    moduleName = "(起重吊装作业30T以上)审核";
                }
                if (jobEntity.WORKDEPTTYPE == "0")
                {
                    mpcEntity = peopleReviwservice.CheckAuditForNextByLiftHoist(currUser, moduleName, jobEntity.CONSTRUCTIONUNITID, jobEntity.FLOWID, false);
                }
                else
                {
                    mpcEntity = peopleReviwservice.CheckAuditForNextByLiftHoist(currUser, moduleName, jobEntity.ENGINEERINGID, jobEntity.FLOWID, false);
                }
               
                if (auditEntity != null && auditEntity.AUDITSTATE == 0)
                {
                    jobEntity.AUDITSTATE = 0;
                    jobEntity.FLOWID = string.Empty;
                    jobEntity.FLOWNAME = string.Empty;
                    jobEntity.FLOWDEPTID = string.Empty;
                    jobEntity.FLOWDEPTNAME = string.Empty;
                    jobEntity.FLOWROLEID = string.Empty;
                    jobEntity.FLOWROLENAME = string.Empty ;
                    jobEntity.FLOWREMARK = string.Empty;
                }
                else
                {
                    if (mpcEntity != null)
                    {
                        jobEntity.AUDITSTATE = 1;
                        jobEntity.FLOWID = mpcEntity.ID;
                        jobEntity.FLOWNAME = mpcEntity.FLOWNAME;
                        jobEntity.FLOWDEPTID = mpcEntity.CHECKDEPTID;
                        jobEntity.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                        jobEntity.FLOWROLEID = mpcEntity.CHECKROLEID;
                        jobEntity.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                        jobEntity.FLOWREMARK = mpcEntity.REMARK;
                    }
                    else
                    {
                        jobEntity.AUDITSTATE = 2;
                        jobEntity.FLOWNAME = "已完结";
                        jobEntity.FLOWDEPTID = "";
                        jobEntity.FLOWDEPTNAME = "";
                        jobEntity.FLOWID = "";
                        jobEntity.FLOWREMARK = "";
                        jobEntity.FLOWROLEID = "";
                        jobEntity.FLOWROLENAME = "";
                    }
                }
                //处理实体，更新到数据库
                service.ApplyCheck(jobEntity, auditEntity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            

        }
        #endregion
    }
}
