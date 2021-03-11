using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using ERCHTMS.Service.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.HiddenTroubleManage;

namespace ERCHTMS.Busines.OutsourcingProject
{
    /// <summary>
    /// 描 述：外包工程信息表
    /// </summary>
    public class OutsouringengineerBLL
    {
        private OutsouringengineerIService service = new OutsouringengineerService();

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

        public DataTable GetIndexToList(Pagination pagination, string queryJson) {
            return service.GetIndexToList(pagination, queryJson);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<OutsouringengineerEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<OutsouringengineerEntity> GetList()
        {
            return service.GetList();
        }
        /// <summary>
        /// 根据当前登录人Id获取外包工程
        /// </summary>
        /// <param name="mode">001--单位资质 002--人员资质 003--合同 004--协议 005--安全技术交底 006--三措两案 
        /// 007--电动工器具验收 008--特种设备验收 009--入场许可 010--开工申请 011--保证金 012--安全评价</param>
        /// <param name="outProjectId"></param>
        /// <returns></returns>
        public DataTable GetEngineerDataByCurrdeptId(Operator currUser, string mode = "", string orgid = "")
        {
            return service.GetEngineerDataByCurrdeptId(currUser, mode, orgid);
        }

        public DataTable GetEngineerDataByCondition(Operator currUser, string mode = "", string orgid = "") 
        {
            return service.GetEngineerDataByCondition(currUser, mode, orgid);
        }
        /// <summary>
        /// 根据外包单位Id获取外包工程
        /// </summary>
        /// <param name="outProjectId"></param>
        /// <returns></returns>
        public DataTable GetEngineerDataByWBId(string deptId,string mode="")
        {
            return service.GetEngineerDataByWBId(deptId, mode);
        }
        /// <summary>
        /// 根据登录人id 查询已经在建的工程(已经通过开工申请的工程)
        /// </summary>
        /// <returns></returns>
        public DataTable GetOnTheStock(Operator currUser)
        {
            return service.GetOnTheStock(currUser);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public OutsouringengineerEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 根据当前登录人 获取已经停工的工程信息
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        public DataTable GetStopEngineerList()
        {
            return service.GetStopEngineerList();
        }
        #region 工程统计
        public string GetTypeCount(string deptid, string year = "")
        {
            return service.GetTypeCount(deptid, year);
        }
        public string GetTypeList(string deptid, string year = "")
        {
            return service.GetTypeList(deptid, year);
        }
        public string GetStateCount(string deptid, string year = "")
        {
            return service.GetStateCount(deptid, year);
        }
        public string GetStateList(string deptid, string year = "")
        {
            return service.GetStateList(deptid, year);
        }
        /// <summary>
        /// 获取工程的流程状态图
        /// </summary>
        /// <param name="keyValue">工程Id</param>
        /// <returns></returns>
        public Flow GetFlow(string keyValue) {
            return service.GetProjectFlow(keyValue);
        }

        public DataTable GetEngineerByCurrDept() {
            return service.GetEngineerByCurrDept();
        }
        #endregion

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
        public void SaveForm(string keyValue, OutsouringengineerEntity entity)
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
        public bool ProIsOver(string keyValue)
        {
            try
            {
                return service.ProIsOver(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
      
        #endregion
    }
}
