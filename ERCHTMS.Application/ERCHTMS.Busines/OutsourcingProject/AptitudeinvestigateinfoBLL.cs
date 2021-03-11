using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using ERCHTMS.Service.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using ERCHTMS.Code;

namespace ERCHTMS.Busines.OutsourcingProject
{
    /// <summary>
    /// 描 述：资质审查基础信息表
    /// </summary>
    public class AptitudeinvestigateinfoBLL
    {
        private AptitudeinvestigateinfoIService service = new AptitudeinvestigateinfoService();
        private QualificationIService quaService = new QualificationService();


        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
       
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<AptitudeinvestigateinfoEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 根据外包单位Id获取最近一次审核通过的资质信息
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public AptitudeinvestigateinfoEntity GetListByOutprojectId(string outprojectId)
        {
            return service.GetListByOutprojectId(outprojectId);
        }
        /// <summary>
        /// 获取资质证件列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<QualificationEntity> GetZzzjList() {
            return quaService.GetList();
        }
        public AptitudeinvestigateinfoEntity GetListByOutengineerId(string outengineerId)
        {
            return service.GetListByOutengineerId(outengineerId);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public AptitudeinvestigateinfoEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }


        public AptitudeinvestigateinfoEntity GetEntityByOutEngineerId(string engineerid) {
            return service.GetEntityByOutEngineerId(engineerid);
        }
        /// <summary>
        /// 查询审核流程图
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="urlType">查询类型：1 单位资质 2 人员资质 3 特种设备验收 4 电动/安全工器具验收 5三措两案 6入厂许可 7开工申请</param>
        /// <returns></returns>
        public Flow GetAuditFlowData(string keyValue, string urltype) {
            return service.GetAuditFlowData(keyValue, urltype);
        }
        /// <summary>
        /// 查询审核流程图-手机端使用
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="urltype">查询类型：1 单位资质 2 人员资质 3 特种设备验收 4 电动/安全工器具验收 5三措两案 6入厂许可 7开工申请</param>
        /// <returns></returns>
        public List<CheckFlowData> GetAppFlowList(string keyValue, string urltype, Operator currUser)
        {
            return service.GetAppFlowList(keyValue, urltype, currUser);
        }

        public List<CheckFlowList> GetAppCheckFlowList(string keyValue, string urltype, Operator currUser)
        {
            return service.GetAppCheckFlowList(keyValue, urltype, currUser);
        }
        /// <summary>
        /// 获取资质证件列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetZzzjPageJson(Pagination pagination, string queryJson) {
            return quaService.GetZzzjPageJson(pagination, queryJson);
        }
        /// <summary>
        /// 获取资质证件实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public QualificationEntity GetZzzjFormJson(string keyValue)
        {
            return quaService.GetZzzjFormJson(keyValue);
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
        /// 删除资质证件数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveZzzjForm(string keyValue)
        {
            try
            {
                quaService.RemoveZzzjForm(keyValue);
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
        public void SaveForm(string keyValue, AptitudeinvestigateinfoEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 保存资质证件表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveZzzjForm(string keyValue, QualificationEntity entity) {
            try
            {
                quaService.SaveZzzjForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
