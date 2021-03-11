using ERCHTMS.Entity.SaftyCheck;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Code;

namespace ERCHTMS.IService.SaftyCheck
{
    /// <summary>
    /// 描 述：安全检查记录
    /// </summary>
    public interface SaftyCheckDataRecordIService
    {
        #region 获取数据
         /// <summary>
        /// 在隐患登记中选择检查记录进行关联
        /// </summary>
        /// <param name="recId">安全检查记录Id</param>
        /// <param name="user"></param>
        /// <returns></returns>
        string GetRecordFromHT(string recId, Operator user);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        IEnumerable<SaftyCheckDataRecordEntity> GetPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// 获取特种设备关联检查记录列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        DataTable GetPageListJsonByTz(Pagination pagination);

        /// <summary>
        ///获取统计表格数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">年份</param>
        /// <param name="belongdistrict">区域</param>
        /// <param name="ctype">检查类型</param>
        /// <returns></returns>
        string GetSaftyList(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "");
        string GetGrpSaftyList(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "");
        List<SaftyCheckCountEntity> GetSaftyList(string deptCode, string year, string ctype);
        /// <summary>
        ///获取统计表格数据(对比)
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">年份</param>
        /// <param name="belongdistrict">区域</param>
        /// <param name="ctype">检查类型</param>
        /// <returns></returns>
        string GetSaftyListDB(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "");
        string GetGrpSaftyListDB(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "");
        /// <summary>
        /// 条形图统计数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">年份</param>
        /// <param name="belongdistrict">区域</param>
        /// <param name="ctype">检查类型</param>
        /// <returns></returns>
        /// <summary>
        /// 获取对比数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">年份</param>
        /// <param name="belongdistrict">区域</param>
        /// <param name="ctype">检查类型</param>
        /// <returns></returns>
        string GetAreaSaftyState(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "");
        string GetGrpAreaSaftyState(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "");
        string getRatherCheckCount(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "");
        string getGrpRatherCheckCount(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "");
        /// <summary>
        /// 趋势数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">年份</param>
        /// <param name="belongdistrict">区域</param>
        /// <param name="ctype">检查类型</param>
        /// <returns></returns>

        string getMonthCheckCount(string deptCode, string year = "", string belongdistrict = "", string ctype = "");
        string getGrpMonthCheckCount(string deptCode, string year = "", string belongdistrict = "", string ctype = "");
        /// <summary>
        /// 专项和其他列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        IEnumerable<SaftyCheckDataRecordEntity> GetPageListForType(Pagination pagination, string queryJson);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<SaftyCheckDataRecordEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        SaftyCheckDataRecordEntity GetEntity(string keyValue);
        /// <summary>
        /// 导出
        /// </summary>
        DataTable ExportData(Pagination pagination, string queryJson);

        /// <summary>
        /// 根据部门和类型获取部门的检查表内容
        /// </summary>
        /// <param name="DeptCode">部门code集合</param>
        /// <returns></returns>
        DataTable AddDeptCheckTable(string DeptCode, string Type);

        /// <summary>
        /// 根据部门CODE获取部门人员集合
        /// </summary>
        /// <param name="Encode">部门Code</param>
        /// <returns>返回对象Json</returns>
        DataTable GetPeopleByEncode(string Encode);
        #endregion

        #region 提交数据
        /// <summary>
        /// 更改登记人
        /// </summary>
        void RegisterPer(string userAccount,string id);
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
        int SaveForm(string keyValue, SaftyCheckDataRecordEntity entity, ref string recid);
        /// <summary>
        /// 修改已检察人员
        /// </summary>
        void UpdateCheckMan(string userAccount);
        #endregion

        #region 获取数据(手机端)
        IEnumerable<SaftyCheckDataRecordEntity> GetSaftyDataList(string safeCheckTypeId, string searchString, Operator user,string deptCode="");
        IEnumerable<SaftyCheckDataRecordEntity> GetSaftyDataList(string safeCheckTypeId, string searchString, Operator user, string deptCode ,int page,int size,out int total);
        SaftyCheckDataRecordEntity getSaftyCheckDataRecordEntity(string safeCheckIdItem);
        DataTable getCheckRecordDetail(string safeCheckIdItem, string riskPointId);
        int addDailySafeCheck(SaftyCheckDataRecordEntity se, Operator user);
        DataTable selectCheckPerson(Operator user);
        List<SaftyCheckDataRecordEntity> GetSaftDataIndexList(ERCHTMS.Code.Operator user);
        DataTable GetSaftyCheckDataList(string safeCheckTypeId, long status, Operator user, string deptCode, long page, long size, out int total, string startTime, string endTime);
        
        #endregion

        #region 首页预警
        DataTable GetSafeCheckWarning(Operator user, string mode = "0");
        decimal GetSafeCheckWarningM(Operator user,string date,int mode=0);

        string GetSafeCheckWarningS();
        decimal GetSafeCheckSumCount(Operator user);
           /// <summary>
        /// 获取首页安全检查考核结果数据
        /// </summary>
        /// <param name="user">当前登录人user</param>
        /// <param name="time">统计时间</param>
        /// <returns></returns>
        object GetSafeCheckWarningByTime(ERCHTMS.Code.Operator user, string time,int mode=0);
        #endregion

        #region 统计
          /// <summary>
        /// 统计对下属各电厂下发的任务次数统计
        /// </summary>
        /// <param name="user"></param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns></returns>
        string getCheckTaskCount(Operator user, string startDate = "", string endDate = "");
        DataTable getCheckTaskData(Operator user, string startDate = "", string endDate = "");
           /// <summary>
        /// 安全检查任务
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetCheckTaskList(Pagination pagination, string queryJson);

        #endregion

        /// <summary>
        /// 修正安全检查记录的检查人员
        /// </summary>
        void UpdateCheckUsers();
    }
}
