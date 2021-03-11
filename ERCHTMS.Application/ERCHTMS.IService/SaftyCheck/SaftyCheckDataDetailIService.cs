using ERCHTMS.Entity.SaftyCheck;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Code;

namespace ERCHTMS.IService.SaftyCheck
{
    /// <summary>
    /// 描 述：安全检查表详情
    /// </summary>
    public interface SaftyCheckDataDetailIService
    {
        #region 获取数据
        /// <summary>
        /// 更改登记状态
        /// </summary>
        void RegisterPer(string userAccount, string id);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<SaftyCheckDataDetailEntity> GetList(string queryJson);
        DataTable GetDetails(string ids);
          /// <summary>
        /// 获取检查记录检查内容的数量
        /// </summary>
        /// <param name="recId"></param>
        /// <returns></returns>
        int GetCount(string recId);
        int GetCheckItemCount(string recId);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        SaftyCheckDataDetailEntity GetEntity(string keyValue);
        /// <summary>
        /// 安全检查表详情列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        IEnumerable<SaftyCheckDataDetailEntity> GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 安全检查表列表(系统生成)
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        DataTable GetPageOfSysCreate(Pagination pagination, string queryJson);

          /// <summary>
        /// 安全检查表列表(系统生成)
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        DataTable GetListOfSysCreate(string queryJson);

        
        /// <summary>
        /// 获取 检查内容
        /// </summary>
        /// <param name="baseID">风险点ID</param>
        DataTable GetPageContent(string baseID);

         /// <summary>
        /// 获取人员需要检查的项目数量
        /// </summary>
        /// <param name="recId">检查计划Id</param>
        /// <param name="userAccount">用户账号</param>
        /// <returns></returns>
        int GetCheckCount(string recId, string userAccount);
        DataTable GetDataTableList(Pagination pagination, string queryJson);
        
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveForm(string keyValue, List<SaftyCheckDataDetailEntity> list);
        void SaveFormToContent(string keyValue, List<SaftyCheckDataDetailEntity> list);
        void SaveResultForm(List<SaftyCheckDataDetailEntity> list);
        int Remove(string recid);
        void Update(string keyValue, SaftyCheckDataDetailEntity entity);
         /// <summary>
        /// 根据检查记录删除检查项目
        /// </summary>
        /// <param name="recId"></param>
        /// <returns></returns>
      
                /// <summary>
        /// 保存检查项目信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="list">检查项目</param>
        /// <param name="deptCode">参与检查的部门（多个英文逗号分隔）</param>
        void Save(string keyValue, List<SaftyCheckDataDetailEntity> list, string deptCode="");
        void Save(string keyValue, List<SaftyCheckDataDetailEntity> list, SaftyCheckDataRecordEntity entity, Operator user, string deptCode = "");
        #endregion

        #region 获取数据(手机端)
        IEnumerable<SaftyCheckDataDetailEntity> GetSaftyDataDetail(string safeCheckIdItem);
        void insertIntoDetails(string checkExcelId, string recid);
        #endregion
    }
}
