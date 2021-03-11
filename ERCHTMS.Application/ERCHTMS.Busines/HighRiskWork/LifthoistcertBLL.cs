using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using ERCHTMS.Service.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.IService.OutsourcingProject;
using ERCHTMS.Service.OutsourcingProject;

namespace ERCHTMS.Busines.HighRiskWork
{
    /// <summary>
    /// 描 述：起吊证
    /// </summary>
    public class LifthoistcertBLL
    {
        private LifthoistcertIService service = new LifthoistcertService();
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
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public LifthoistcertEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
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
        public void SaveForm(string keyValue, LifthoistcertEntity entity)
        {
            try
            {
                var dbentity = this.GetEntity(keyValue);
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
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 凭吊证审核
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="auditEntity">审核实体</param>
        public void ApplyCheck(string keyValue, LifthoistcertEntity entity = null, LifthoistauditrecordEntity auditEntity = null)
        {
            Operator currUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            LifthoistcertEntity certEntity = this.GetEntity(keyValue);
            if (certEntity == null)
            {
                throw new ArgumentException("无法找到当前业务信息，请确认业务ID是否有误！");
            }
            ManyPowerCheckEntity mpcEntity = null;
            //如果外部传入的实体不为null，则说明是负责人操作
            if (entity != null)
            {
                certEntity.CHARGEPERSONNAME = entity.CHARGEPERSONNAME;
                certEntity.CHARGEPERSONID = entity.CHARGEPERSONID;
                certEntity.CHARGEPERSONSIGN = entity.CHARGEPERSONSIGN;
                certEntity.HOISTAREAPERSONNAMES = entity.HOISTAREAPERSONNAMES;
                certEntity.HOISTAREAPERSONIDS = entity.HOISTAREAPERSONIDS;
                certEntity.HOISTAREAPERSONSIGNS = entity.HOISTAREAPERSONSIGNS;
                certEntity.safetys = entity.safetys;
            }
            if (auditEntity != null)
            {
                //把当前业务流程节点赋值到审核记录中
                auditEntity.FLOWID = certEntity.FLOWID;
            }
            string moduleName = "(起重吊装准吊证)审核";
            mpcEntity = peopleReviwservice.CheckAuditForNextByOutsourcing(currUser, moduleName, certEntity.CONSTRUCTIONUNITID, certEntity.FLOWID, false, true);
            if (auditEntity != null && auditEntity.AUDITSTATE == 0)
            {
                certEntity.AUDITSTATE = 0;
                certEntity.FLOWID = string.Empty;
                certEntity.FLOWNAME = currUser.UserName + "审核/批不同意";
                certEntity.FLOWDEPTID = currUser.DeptId;
                certEntity.FLOWDEPTNAME = currUser.DeptName;
                certEntity.FLOWROLEID = currUser.RoleId;
                certEntity.FLOWROLENAME = currUser.RoleName;
            }
            else
            {
                if (mpcEntity != null)
                {
                    certEntity.AUDITSTATE = 1;
                    certEntity.FLOWID = mpcEntity.ID;
                    certEntity.FLOWNAME = mpcEntity.FLOWNAME;
                    certEntity.FLOWDEPTID = mpcEntity.CHECKDEPTID;
                    certEntity.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                    certEntity.FLOWROLEID = mpcEntity.CHECKROLEID;
                    certEntity.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                }
                else
                {
                    certEntity.AUDITSTATE = 2;
                    certEntity.FLOWNAME = "已完结";
                }
            }
            //处理实体，更新到数据库
            service.ApplyCheck(certEntity, auditEntity);
        }
        #endregion
    }
}
