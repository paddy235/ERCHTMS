using ERCHTMS.Entity.SaftyCheck;
using ERCHTMS.IService.SaftyCheck;
using ERCHTMS.Service.SaftyCheck;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Code;
using ERCHTMS.Busines.BaseManage;

namespace ERCHTMS.Busines.SaftyCheck
{
    /// <summary>
    /// 描 述：安全检查记录
    /// </summary>
    public class SaftyCheckDataRecordBLL
    {
        private SaftyCheckDataRecordIService service = new SaftyCheckDataRecordService();

        #region 获取数据

         /// <summary>
        /// 在隐患登记中选择检查记录进行关联
        /// </summary>
        /// <param name="recId">安全检查记录Id</param>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GetRecordFromHT(string recId,Operator user)
        {
            return service.GetRecordFromHT(recId,user);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<SaftyCheckDataRecordEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// 获取特种设备关联检查记录列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageListJsonByTz(Pagination pagination)
        {
            return service.GetPageListJsonByTz(pagination);
        }
        /// <summary>
        /// 安全检查任务
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetCheckTaskList(Pagination pagination, string queryJson)
        {
            return service.GetCheckTaskList(pagination, queryJson);
        }
        /// <summary>
        ///获取统计表格数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">年份</param>
        /// <param name="belongdistrict">区域</param>
        /// <param name="ctype">检查类型</param>
        /// <returns></returns>
        public string GetSaftyList(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "")
        {
            return service.GetSaftyList(deptCode, year, belongdistrictcode, ctype);
        }
        public string GetGrpSaftyList(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "")
        {
            return service.GetGrpSaftyList(deptCode, year, belongdistrictcode, ctype);
        }
        public List<SaftyCheckCountEntity> GetSaftyList(string deptCode, string year, string ctype)
        {
            return service.GetSaftyList(deptCode, year, ctype);
        }
        /// <summary>
        ///获取统计表格数据(对比)
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">年份</param>
        /// <param name="belongdistrict">区域</param>
        /// <param name="ctype">检查类型</param>
        /// <returns></returns>
        public string GetSaftyListDB(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "")
        {
            return service.GetSaftyListDB(deptCode, year, belongdistrictcode, ctype);
        }
        public string GetGrpSaftyListDB(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "")
        {
            return service.GetGrpSaftyListDB(deptCode, year, belongdistrictcode, ctype);
        }
        /// <summary>
        ///获取对比数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">年份</param>
        /// <param name="belongdistrict">区域</param>
        /// <param name="ctype">检查类型</param>
        /// <returns></returns>
        public string GetAreaSaftyState(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "")
        {
            return service.GetAreaSaftyState(deptCode, year, belongdistrictcode, ctype);
        }
        public string GetGrpAreaSaftyState(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "")
        {
            return service.GetGrpAreaSaftyState(deptCode, year, belongdistrictcode, ctype);
        }
        /// <summary>
        /// 条形图统计数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">年份</param>
        /// <param name="belongdistrictcode">区域</param>
        /// <param name="ctype">检查类型</param>
        /// <returns></returns>
        public string getRatherCheckCount(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "")
        {
            return service.getRatherCheckCount(deptCode, year, belongdistrictcode, ctype);
        }
        public string getGrpRatherCheckCount(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "")
        {
            return service.getGrpRatherCheckCount(deptCode, year, belongdistrictcode, ctype);
        }
        /// <summary>
        /// 曲线图统计数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">年份</param>
        /// <param name="belongdistrict">区域</param>
        /// <param name="ctype">检查类型</param>
        /// <returns></returns>
        public string getMonthCheckCount(string deptCode, string year = "", string belongdistrict = "", string ctype = "")
        {
            return service.getMonthCheckCount(deptCode, year, belongdistrict, ctype);
        }
        public string getGrpMonthCheckCount(string deptCode, string year = "", string belongdistrict = "", string ctype = "")
        {
            return service.getGrpMonthCheckCount(deptCode, year, belongdistrict, ctype);
        }
        /// <summary>
        /// 专项和其他列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<SaftyCheckDataRecordEntity> GetPageListForType(Pagination pagination, string queryJson)
        {
            return service.GetPageListForType(pagination, queryJson);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SaftyCheckDataRecordEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SaftyCheckDataRecordEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 导出
        /// </summary>
        public DataTable ExportData(Pagination pagination, string queryJson)
        {
            return service.ExportData(pagination, queryJson);
        }

        /// <summary>
        /// 根据部门和类型获取部门的检查表内容
        /// </summary>
        /// <param name="DeptCode">部门code集合</param>
        /// <returns></returns>
        public DataTable AddDeptCheckTable(string DeptCode, string Type) {
            return service.AddDeptCheckTable(DeptCode, Type);
        }

        /// <summary>
        /// 根据部门CODE获取部门人员集合
        /// </summary>
        /// <param name="Encode">部门Code</param>
        /// <returns>返回对象Json</returns>
        public DataTable GetPeopleByEncode(string Encode)
        {
            return service.GetPeopleByEncode(Encode);
        }

        /// <summary>
        /// 根据检查对象和检查内容名称获取检查内容ID和检查对象ID(中间用逗号分隔，依次为检查内容ID、检查对象ID)，当无匹配时返回空字符串
        /// </summary>
        ///  <param name="chkId">检查记录Id</param>
        /// <param name="checkObject">检查对象名称</param>
        /// <param name="checkContent">检查内容名称</param>
        /// <param name="user">当前用户</param>
        /// <returns></returns>
        public string GetCheckContentId(string chkId, string checkObject, string checkContent, Operator user)
        {
            string id = "";
             DepartmentBLL deptBll=new DepartmentBLL();
            if (string.IsNullOrWhiteSpace(checkObject) && string.IsNullOrWhiteSpace(checkContent))
            {
                id = GetRecordFromHT(chkId, user);
            }
            else
            {

                string sql = string.Format("select id from BIS_SAFTYCHECKDATADETAILED where recid='{2}' and checkobject='{0}' and checkcontent='{1}'", checkObject.Trim(), checkContent.Trim(), chkId);
             
                SaftyCheckDataRecordEntity sd = GetEntity(chkId);
                if (sd!=null)
                {
                   if(sd.CheckDataType!=1)
                   {
                       sql+=string.Format(" and (',' || checkmanid || ',') like '%,{0},%'",user.Account);
                   }
                }
                 DataTable dt =deptBll.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    id = dt.Rows[0][0].ToString();
                    DataTable dtItem = deptBll.GetDataTable(string.Format("select id from BIS_SAFTYCONTENT where checkobject='{0}' and detailid='{1}' and recid='{2}'", checkObject.Trim(), id, chkId));
                    if (dtItem.Rows.Count>0)
                    {
                        id = "";
                    }
                    
                }
            }
            if (!string.IsNullOrWhiteSpace(id))
            {
                DataTable dt = deptBll.GetDataTable(string.Format("select checkobjectid from BIS_SAFTYCHECKDATADETAILED where  id='{0}' and recid='{1}'",  id, chkId));
                if(dt.Rows.Count>0)
                {
                    id += ","+dt.Rows[0][0].ToString();
                }
            }
            return id;
        }

        #endregion

        #region 提交数据
        /// <summary>
        /// 更改登记人
        /// </summary>
        public void RegisterPer(string userAccount,string id)
        {
            try
            {
                service.RegisterPer(userAccount,id);
            }
            catch (Exception)
            {
                throw;
            }
        }
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
        public int SaveForm(string keyValue, SaftyCheckDataRecordEntity entity,ref string recid)
        {
            try
            {
                return service.SaveForm(keyValue, entity, ref recid);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 修改已检察人员
        /// </summary>
        public void UpdateCheckMan(string userAccount)
        {
            try
            {
                service.UpdateCheckMan(userAccount);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 获取数据手机端
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="safeCheckTypeId">检查类型</param>
        /// <param name="searchString">检查记录名称</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<SaftyCheckDataRecordEntity> GetSaftyDataList(string safeCheckTypeId, string searchString, Operator user,string deptCode="")
        {
            return service.GetSaftyDataList(safeCheckTypeId, searchString,user);
        }
        public IEnumerable<SaftyCheckDataRecordEntity> GetSaftyDataList(string safeCheckTypeId, string searchString, Operator user, string deptCode, int page, int size, out int total)
        {
            
            return service.GetSaftyDataList(safeCheckTypeId, searchString, user, deptCode, page, size,out total);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="safeCheckIdItem">检查记录</param>
        /// <returns>返回分页列表</returns>
        public SaftyCheckDataRecordEntity getSaftyCheckDataRecordEntity(string safeCheckIdItem)
        {
            return service.getSaftyCheckDataRecordEntity(safeCheckIdItem);
        }
        /// <summary>
        /// 获取隐患实体
        /// </summary>
        /// <param name="safeCheckIdItem">检查记录</param>
        /// <param name="safeCheckIdItem">检查记录</param>
        /// <returns>隐患实体</returns>
        public DataTable getCheckRecordDetail(string safeCheckIdItem, string riskPointId)
        {
            return service.getCheckRecordDetail(safeCheckIdItem, riskPointId);
        }
        /// <summary>
        /// 新增日常检查计划
        /// </summary>
        public int addDailySafeCheck(SaftyCheckDataRecordEntity se, Operator user)
        {
            return service.addDailySafeCheck(se,user);
        }

        /// <summary>
        /// 选择检查人员
        /// </summary>
        public DataTable selectCheckPerson(Operator user)
        {
            return service.selectCheckPerson(user);
        }

        public List<SaftyCheckDataRecordEntity> GetSaftDataIndexList(ERCHTMS.Code.Operator user)
        {
            return service.GetSaftDataIndexList(user);
        }
        public DataTable GetSaftyCheckDataList(string safeCheckTypeId, long status, Operator user, string deptCode, long page, long size, out int total, string startTime, string endTime)
        {
            return service.GetSaftyCheckDataList(safeCheckTypeId, status, user, deptCode, page, size, out total, startTime, endTime);
        }
        #endregion

        #region 首页预警
        public DataTable GetSafeCheckWarning(Operator user, string mode = "0")
        {
            return service.GetSafeCheckWarning(user, mode);
        }
        public decimal GetSafeCheckWarningM(Operator user, string date,int mode=0)
        {
            return service.GetSafeCheckWarningM(user,date,mode);
        }
        public string GetSafeCheckWarningS()
        {
            return service.GetSafeCheckWarningS();
        }

        public decimal GetSafeCheckSumCount(Operator user) 
        {
            return service.GetSafeCheckSumCount(user);
        }
           /// <summary>
        /// 获取首页安全检查考核结果数据
        /// </summary>
        /// <param name="user">当前登录人user</param>
        /// <param name="time">统计时间</param>
        /// <returns></returns>
        public object GetSafeCheckWarningByTime(ERCHTMS.Code.Operator user, string time,int mode=0)
        {
            return service.GetSafeCheckWarningByTime(user, time,mode);
        }
        #endregion

        #region 统计
          /// <summary>
        /// 统计对下属各电厂下发的任务次数统计
        /// </summary>
        /// <param name="user"></param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns></returns>
        public string getCheckTaskCount(Operator user,string startDate = "", string endDate="")
        {
            return service.getCheckTaskCount(user, startDate, endDate);
        }
        public DataTable getCheckTaskData(Operator user, string startDate = "", string endDate = "")
        {
            return service.getCheckTaskData(user, startDate, endDate);
        }
        #endregion
          /// <summary>
        /// 修正安全检查记录的检查人员
        /// </summary>
        public void UpdateCheckUsers()
        {
            service.UpdateCheckUsers();
        }
    }
}
