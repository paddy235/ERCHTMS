using ERCHTMS.Entity.SaftyCheck;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using ERCHTMS.Code;
using System.Data;

namespace ERCHTMS.IService.SaftyCheck
{
    /// <summary>
    /// 描 述：安全检查表
    /// </summary>
    public interface SaftyCheckDataIService
    {
        #region 获取数据
        /// <summary>
        /// 通过folderId 获取对应的文件
        /// </summary>
        /// <param name="folderId"></param>
        /// <returns></returns>
        DataTable GetListByObject(string folderId);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<SaftyCheckDataEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        SaftyCheckDataEntity GetEntity(string keyValue);
        /// <summary>
        /// 安全检查表列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        IEnumerable<SaftyCheckDataEntity> GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 安全检查名称字典列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        DataTable GetCheckNamePageList(Pagination pagination, string queryJson);

        /// <summary>
        /// 首页待办事项
        /// </summary>
        /// <param name="user"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        int[] GetCheckCount(ERCHTMS.Code.Operator user, int mode);
        #endregion

        #region 提交数据
        DataTable GetCheckStat(ERCHTMS.Code.Operator user, int category);
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue);
        void RemoveCheckName(string keyValue);
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        int SaveForm(string keyValue, SaftyCheckDataEntity entity);
        int SaveCheckName(Operator user, List<CheckNameSetEntity> list);
        #endregion

        #region 手机端
        IEnumerable<SaftyCheckDataEntity> selectCheckExcel(Operator user);
        List<DistinctArray> getDistinctGroup(string recid);
        List<DistinctArray> getDistinctGroupDj(string recid, string checkdatatype, Operator user);
        DataTable selectCheckContent(string risknameid, string userAccount, string type);

        DataTable getCheckPlanList(Operator user, string ctype);

        object GetCheckStatistics(ERCHTMS.Code.Operator user, string deptCode);

        DataTable GetCheckObjects(string recId, int mode = 0);

        DataTable GetCheckItems(string checkObjId, string recId, int mode = 0);

        List<object> GetCheckContents(string checkId, int mode = 0);
        /// <summary>
        /// 获取隐患和违章数量（顺序依次为隐患，违章）
        /// </summary>
        /// <param name="checkId">检查记录Id</param>
        /// <param name="mode">查询方式（0：获取关联检查记录的所有隐患和违章数量，1：获取检查项目登记的隐患和违章数量）</param>
        /// <returns></returns>
        List<int> GetHtAndWzCount(string recId, int mode);
        /// <summary>
        /// 获取检查中登记的隐患列表
        /// </summary>
        /// <param name="recId"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        DataTable GetHtList(string recId, int mode);
        /// <summary>
        /// 获取检查中登记的违章列表
        /// </summary>
        /// <param name="recId"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        DataTable GetWzList(string recId, int mode);
        /// <summary>
        /// 获取检查内容详情
        /// </summary>
        /// <param name="itemid"></param>
        /// <returns></returns>
        DataTable GetCheckContentInfo(string itemid);
        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DataTable GetEquimentInfo(string id);


        /// <summary>
        /// 执行周期性计划，根据规则自动创建检查计划
        /// </summary>
        /// <returns></returns>
        string AutoCreateCheckPlan();
        /// <summary>
        /// 设置是否中止周期性计划任务
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        int SetStatus(string id, int status);
        #endregion
    }
}
