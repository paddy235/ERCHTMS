using ERCHTMS.Entity.SaftyCheck;
using ERCHTMS.IService.SaftyCheck;
using ERCHTMS.Service.SaftyCheck;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using ERCHTMS.Code;
using System.Data;
using ERCHTMS.Busines.BaseManage;

namespace ERCHTMS.Busines.SaftyCheck
{
    /// <summary>
    /// 描 述：安全检查表
    /// </summary>
    public class SaftyCheckDataBLL
    {
        private SaftyCheckDataIService service = new SaftyCheckDataService();

        #region 获取数据
        /// <summary>
        /// 安全检查名称字典列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetCheckNamePageList(Pagination pagination, string queryJson)
        {
            return service.GetCheckNamePageList(pagination, queryJson);
        }
            /// <summary>
            /// 通过folderId 获取对应的文件
            /// </summary>
            /// <param name="folderId"></param>
            /// <returns></returns>
            public DataTable GetListByObject(string folderId)
        {
            return service.GetListByObject(folderId);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        public DataTable GetCheckStat(ERCHTMS.Code.Operator user, int category = 2)
        {
            return service.GetCheckStat(user, category);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SaftyCheckDataEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SaftyCheckDataEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 安全检查表列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<SaftyCheckDataEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }


        /// <summary>
        /// 首页待办事项
        /// </summary>
        /// <param name="user"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public int[] GetCheckCount(ERCHTMS.Code.Operator user, int mode)
        {
            return service.GetCheckCount(user, mode);
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
        /// 删除检查名称
        /// </summary>
        /// <param name="keyValue"></param>
        public void RemoveCheckName(string keyValue)
        {
            service.RemoveCheckName(keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public int SaveForm(string keyValue, SaftyCheckDataEntity entity)
        {
            try
            {
                return service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int SaveCheckName(Operator user, List<CheckNameSetEntity> list)
        {
            return service.SaveCheckName(user, list);
        }
        #endregion

        #region 手机端
            public IEnumerable<SaftyCheckDataEntity> selectCheckExcel(Operator user)
        {
            return service.selectCheckExcel(user);
        }

        public List<DistinctArray> getDistinctGroup(string recid)
        {
            return service.getDistinctGroup(recid);
        }

        public List<DistinctArray> getDistinctGroupDj(string recid, string checkdatatype, Operator user)
        {
            return service.getDistinctGroupDj(recid, checkdatatype, user);
        }

        public DataTable selectCheckContent(string risknameid, string userAccount, string type)
        {
            return service.selectCheckContent(risknameid, userAccount, type);
        }

        public DataTable getCheckPlanList(Operator user, string ctype)
        {
            return service.getCheckPlanList(user, ctype);
        }

        public object GetCheckStatistics(Operator user, string deptCode = "")
        {
            return service.GetCheckStatistics(user, deptCode);
        }
        public DataTable GetCheckObjects(string recId, int mode = 0)
        {
            return service.GetCheckObjects(recId, mode);
        }
        public DataTable GetCheckItems(string checkObjId, string recId, int mode = 0)
        {
            return service.GetCheckItems(checkObjId, recId, mode);
        }
        public List<object> GetCheckContents(string checkId, int mode = 0)
        {
            return service.GetCheckContents(checkId, mode);
        }
        /// <summary>
        /// 获取隐患和违章数量（顺序依次为隐患，违章）
        /// </summary>
        /// <param name="checkId">检查记录Id</param>
        /// <param name="mode">查询方式（0：获取关联检查记录的所有隐患和违章数量，1：获取检查项目登记的隐患和违章数量）</param>
        /// <returns></returns>
        public List<int> GetHtAndWzCount(string recId, int mode)
        {
            return service.GetHtAndWzCount(recId, mode);
        }
        /// <summary>
        /// 获取检查中登记的违章列表
        /// </summary>
        /// <param name="recId"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public DataTable GetWzList(string recId, int mode)
        {
            return service.GetWzList(recId, mode);
        }
        /// <summary>
        /// 获取检查中登记的隐患列表
        /// </summary>
        /// <param name="recId"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public DataTable GetHtList(string recId, int mode)
        {
            return service.GetHtList(recId, mode);
        }
        /// <summary>
        /// 获取检查内容详情
        /// </summary>
        /// <param name="itemid"></param>
        /// <returns></returns>
        public DataTable GetCheckContentInfo(string itemid)
        {
            return service.GetCheckContentInfo(itemid);
        }
        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetEquimentInfo(string id)
        {
            return service.GetEquimentInfo(id);
        }

        /// <summary>
        /// 执行周期性计划，根据规则自动创建检查计划
        /// </summary>
        /// <returns></returns>
        public string AutoCreateCheckPlan()
        {
            return service.AutoCreateCheckPlan();
        }
        /// <summary>
        /// 设置是否中止周期性计划任务
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int SetStatus(string id, int status)
        {
            return service.SetStatus(id, status);
        }
        #endregion
    }
}
