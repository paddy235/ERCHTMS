using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.IService.RiskDatabase;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Util.Extension;
using System.Data;
using System.Data.Common;
using BSFramework.Data;
using BSFramework.Util;
using System.Text;
using System;

namespace ERCHTMS.Service.RiskDatabase
{
    /// <summary>
    /// 描 述：安全风险库历史记录
    /// </summary>
    public class RiskHistoryService : RepositoryFactory<RiskHistoryEntity>, RiskHistotyIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="workId">作业步骤ID</param>
        /// <param name="areaCode">区域编码</param>
        /// <param name="areaId">区域ID</param>
        /// <param name="grade">风险等级</param>
        ///  <param name="accType">事故类型</param>
        ///  <param name="deptCode">部门编码</param>
        ///  <param name="keyWord">查询关键字</param>
        /// <returns>返回列表</returns>
        public IEnumerable<RiskHistoryEntity> GetList(string areaCode, string areaId, string grade, string accType, string deptCode, string keyWord)
        {
            var expression = LinqExtensions.True<RiskHistoryEntity>();
            if (!string.IsNullOrEmpty(areaCode))
            {
                expression = expression.And(t => t.AreaCode == areaCode);
            }
            if (!string.IsNullOrEmpty(areaId))
            {
                expression = expression.And(t => t.AreaId == areaId);
            }
            if (!string.IsNullOrEmpty(grade))
            {
                expression = expression.And(t => t.Grade == grade);
            }
            if (!string.IsNullOrEmpty(accType))
            {
                expression = expression.And(t => accType.Contains(t.AccidentName));
            }
            if (!string.IsNullOrEmpty(deptCode))
            {
                expression = expression.And(t => t.DeptCode.Contains(deptCode));
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                expression = expression.And(t => t.Description.Contains(keyWord) || t.DangerSource.Contains(keyWord));
            }
            return this.BaseRepository().IQueryable().Where(expression);
        }
        /// <summary>
        /// 列表分页
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();
            //计划ID
            if (!queryParam["planId"].IsEmpty())
            {
                string planId = queryParam["planId"].ToString();
                pagination.conditionJson += string.Format(" and planId ='{0}'", planId);
            }
            //岗位
            if (!queryParam["postId"].IsEmpty())
            {
                string postId = queryParam["postId"].ToString();
                pagination.conditionJson += string.Format(" and postId ='{0}'", postId);
            }
            //状态
            if (!queryParam["status"].IsEmpty())
            {
                string status = queryParam["status"].ToString();
                pagination.conditionJson += string.Format(" and status ={0}", status);
            }
            //区域Code
            string areaCode = "";
            if (!queryParam["areaCode"].IsEmpty())
            {
                areaCode = queryParam["areaCode"].ToString();
                pagination.conditionJson += string.Format(" and areaCode like '{0}%'", areaCode);
            }
            //区域ID
            if (!queryParam["areaId"].IsEmpty())
            {
                string areaId = queryParam["areaId"].ToString();
                pagination.conditionJson += string.Format(" and areaId = '{0}'", areaId);
            }
            //风险等级
            if (!queryParam["grade"].IsEmpty())
            {
                string grade = queryParam["grade"].ToString();
                pagination.conditionJson += string.Format(" and grade = '{0}'", grade);
            }
            //风险类别
            if (!queryParam["riskType"].IsEmpty())
            {
                string riskType = queryParam["riskType"].ToString();
                pagination.conditionJson += string.Format(" and risktype ='{0}'", riskType);
            }
            //事故类型
            if (!queryParam["accType"].IsEmpty())
            {
                string accType = queryParam["accType"].ToString();
                pagination.conditionJson += string.Format(" and AccidentName like '%{0}%'", accType);
            }
            //部门Code
            if (!queryParam["deptCode"].IsEmpty())
            {
                string deptCode = queryParam["deptCode"].ToString();
                pagination.conditionJson += string.Format(" and deptCode like '{0}%'", deptCode);
            }
            //本人辨识的
            if (!queryParam["selfSpot"].IsEmpty())
            {
                string selfSpot = queryParam["selfSpot"].ToString();
                pagination.conditionJson += string.Format(" and planid in(select planid from BIS_RISKPPLANDATA where userid='{0}' and DATATYPE=0)", selfSpot);
            }
            //本人评估的
            if (!queryParam["selfAssess"].IsEmpty())
            {
                string selfAssess = queryParam["selfAssess"].ToString();
                pagination.conditionJson += string.Format(" and planid in(select planid from BIS_RISKPPLANDATA where userid='{0}' and DATATYPE=1)", selfAssess);
            }
            //查询关键字
            if (!queryParam["keyWord"].IsEmpty())
            {
                string keyWord = queryParam["keyWord"].ToString();
                pagination.conditionJson += string.Format(" and (Description like '%{0}%' or DangerSource like '%{0}%' or result like '%{0}%') ", keyWord.Trim());
            }
            if (!queryParam["keyValue"].IsEmpty())
            {
                string keyValue = queryParam["keyValue"].ToString();
                pagination.conditionJson += string.Format(" and ( deptname like '%{0}%' or postname like '%{0}%') ", keyValue.Trim());
            }
            if (!queryParam["AreaIds"].IsEmpty())
            {
                string AreaIds = queryParam["AreaIds"].ToString();
                if (AreaIds.Length > 0)
                {
                    pagination.conditionJson += string.Format(" and districtid in('{0}')", AreaIds.Replace(",", "','"));
                }
            }
            if (!queryParam["initAreaId"].IsEmpty())
            {
                string initAreaId = GetAreaIdsByCode(areaCode);
                if (initAreaId.Length>0)
                {
                     pagination.conditionJson += string.Format(" or areaId in('{0}')", initAreaId.Replace(",", "','"));
                }
               
            }
            //部门Code
            if (!queryParam["historyid"].IsEmpty())
            {
                string historyid = queryParam["historyid"].ToString();
                pagination.conditionJson += string.Format(" and historyid = '{0}'", historyid);
            }
            if (!queryParam["Name"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and Name like '%{0}%'", queryParam["Name"].ToString());
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 根据区域Code获取下属所有区域包含的内置区域ID
        /// </summary>
        /// <param name="areaCode">区域code</param>
        /// <returns></returns>
        public string GetAreaIdsByCode(string areaCode)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dt = this.BaseRepository().FindTable(string.Format("select linktocompanyid from BIS_DISTRICT  where linktocompanyid is not null and districtcode like '{0}%'", areaCode));
            foreach (DataRow dr in dt.Rows)
            {
                if (!sb.ToString().Contains(dr[0].ToString()))
                {
                    sb.Append(dr[0].ToString());
                }

            }
            dt.Dispose();
            return sb.ToString();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public RiskHistoryEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        
        #endregion

        #region 提交数据
        /// <summary>
        /// 根据计划Id删除风险历史记录
        /// </summary>
        /// <param name="planId">计划ID</param>
        /// <returns></returns>
        public int Remove(string planId)
        {
            return this.BaseRepository().ExecuteBySql(string.Format("delete from BIS_RISKHISTORY where planid=@planId"), new DbParameter[] { DbParameters.CreateDbParameter("@planId", planId) });
        }
        
        #endregion
    }
}
