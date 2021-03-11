using ERCHTMS.Entity.SaftyCheck;
using ERCHTMS.IService.SaftyCheck;
using ERCHTMS.Service.SaftyCheck;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Code;

namespace ERCHTMS.Busines.SaftyCheck
{
    /// <summary>
    /// 描 述：安全检查表详情
    /// </summary>
    public class SaftyCheckDataDetailBLL
    {
        private SaftyCheckDataDetailIService service = new SaftyCheckDataDetailService();

        #region 获取数据
        /// <summary>
        /// 更改登记状态
        /// </summary>
        public void RegisterPer(string userAccount, string id)
        {
            try
            {
                service.RegisterPer(userAccount, id);
            }
            catch (Exception)
            {
                throw;
            }
        }
          /// <summary>
        /// 获取检查记录检查内容的数量
        /// </summary>
        /// <param name="recId"></param>
        /// <returns></returns>
        public int GetCount(string recId)
        {
            return service.GetCount(recId);
        }
        public int GetCheckItemCount(string recId)
        {
            return service.GetCheckItemCount(recId);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SaftyCheckDataDetailEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        public DataTable GetDetails(string ids)
        {
            return service.GetDetails(ids);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SaftyCheckDataDetailEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 安全检查表详情列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<SaftyCheckDataDetailEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// 安全检查表列表(系统生成)
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageOfSysCreate(Pagination pagination, string queryJson)
        {
            return service.GetPageOfSysCreate(pagination, queryJson);
        }
        /// <summary>
        /// 安全检查表列表(系统生成)
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetListOfSysCreate(string queryJson)
        {
            return service.GetListOfSysCreate(queryJson);
        }
        /// <summary>
        /// 获取 检查内容
        /// </summary>
        /// <param name="baseID">风险点ID</param>
        public DataTable GetPageContent(string baseID)
        {
            return service.GetPageContent(baseID);
        }

         /// <summary>
        /// 获取人员需要检查的项目数量
        /// </summary>
        /// <param name="recId">检查计划Id</param>
        /// <param name="userAccount">用户账号</param>
        /// <returns></returns>
        public int GetCheckCount(string recId,string userAccount)
        {
            return service.GetCheckCount(recId,userAccount);
        }
        public DataTable GetDataTableList(Pagination pagination, string queryJson)
        {
            return service.GetDataTableList(pagination, queryJson);
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
        /// 删除数据
        /// </summary>
        /// <param name="recid">安全检查表ID</param>
        public int Remove(string recid)
        {
            try
            {
                return service.Remove(recid);
            }
            catch (Exception)
            {
                return -1;
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, List<SaftyCheckDataDetailEntity> list)
        {
            try
            {
                service.SaveForm(keyValue, list);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 专项检查制定计划保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="list">实体对象</param>
        /// <returns></returns>
        public void SaveFormToContent(string keyValue, List<SaftyCheckDataDetailEntity> list)
        {
            try
            {
                service.SaveForm(keyValue, list);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void SaveResultForm(List<SaftyCheckDataDetailEntity> list)
        {
            try
            {
                service.SaveResultForm(list);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Update(string keyValue,SaftyCheckDataDetailEntity entity)
        {
            service.Update(keyValue, entity);
        }

                /// <summary>
        /// 保存检查项目信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="list">检查项目</param>
        /// <param name="deptCode">参与检查的部门（多个英文逗号分隔）</param>
        public void Save(string keyValue, List<SaftyCheckDataDetailEntity> list,string deptCode="")
        {
            service.Save(keyValue, list, deptCode);
        }
        public void Save(string keyValue, List<SaftyCheckDataDetailEntity> list, SaftyCheckDataRecordEntity entity, Operator user, string deptCode = "")
        {
            service.Save(keyValue, list,entity,user, deptCode);
        }
        #endregion

        #region 获取数据(手机端)
        public IEnumerable<SaftyCheckDataDetailEntity> GetSaftyDataDetail(string safeCheckIdItem)
        {
            return service.GetSaftyDataDetail(safeCheckIdItem);
        }
        public void insertIntoDetails(string checkExcelId, string recid)
        {
            try
            {
                service.insertIntoDetails(checkExcelId,recid);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
