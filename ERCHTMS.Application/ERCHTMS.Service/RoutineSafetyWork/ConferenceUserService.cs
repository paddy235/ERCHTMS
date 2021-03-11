using ERCHTMS.Entity.RoutineSafetyWork;
using ERCHTMS.IService.RoutineSafetyWork;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.Dynamic;
using System;

namespace ERCHTMS.Service.RoutineSafetyWork
{
    /// <summary>
    /// 描 述：安全会议参会人员表
    /// </summary>
    public class ConferenceUserService : RepositoryFactory<ConferenceUserEntity>, ConferenceUserIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            if (!queryParam["state"].IsEmpty())
            {
                if (queryParam["state"].ToString() == "0")
                {
                    pagination.conditionJson += string.Format(" and r.ReviewState='1' ");
                }
                else {
                    pagination.conditionJson += string.Format(" and (r.ReviewState='2' or r.ReviewState='3')");
                }
                
            }
            //查询条件
            if (!queryParam["txtSearch"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and e.ConferenceName like '%{0}%'", queryParam["txtSearch"].ToString());
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<ConferenceUserEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ConferenceUserEntity GetEntity(string keyValue,string UserId)
        {
            string sql = string.Format(" select id from BIS_ConferenceUser t where t.ConferenceID='{0}' and UserID='{1}'", keyValue, UserId);
            DataTable dt = this.BaseRepository().FindTable(sql);
            if (dt != null && dt.Rows.Count > 0) {
                keyValue = dt.Rows[0][0].ToString();
            }
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 获取签到表数据
        /// </summary>
        /// <param name="keyValue">会议ID</param>
        /// <returns></returns>
        public List<object> GetSignTable(string keyValue) {
            List<Object> dataList = new List<Object>();
            //签到
            List<Object> dataQD = new List<Object>();
            //未签到
            List<Object> dataWQD = new List<Object>();
            //请假
            List<Object> dataQ = new List<Object>();
            
            DataTable dt = new DataTable();
            //获取已签到人员
            string sql = string.Format(@"select t.username,t.photourl,r.deptname from BIS_ConferenceUser t left join v_userinfo r  on t.userid=r.userid 
where t.issign='0' and t.conferenceid='{0}'",keyValue);
            dt = this.BaseRepository().FindTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    dynamic dy = new ExpandoObject();
                    dy.username = dr["username"].ToString();
                    dy.photourl = dr["photourl"].ToString();
                    dy.deptname = dr["deptname"].ToString();
                    dataQD.Add(dy);
                }
            }
            else {
                dataQD = new List<Object>();
            }
            dataList.Add(dataQD);
            //获取未签到人员
            sql = string.Format(@"select t.username,t.photourl,r.deptname from BIS_ConferenceUser t left join v_userinfo r  on t.userid=r.userid 
where t.issign='1' and t.conferenceid='{0}' and t.reviewstate!='2'", keyValue);
            dt = this.BaseRepository().FindTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    dynamic dy = new ExpandoObject();
                    dy.username = dr["username"].ToString();
                    dy.photourl = dr["photourl"].ToString();
                    dy.deptname = dr["deptname"].ToString();
                    dataWQD.Add(dy);
                }
            }
            else
            {
                dataWQD = new List<Object>();
            }
            dataList.Add(dataWQD);
            //获取请假人员
            sql = string.Format(@"select t.username,t.photourl,r.deptname from BIS_ConferenceUser t left join v_userinfo r  on t.userid=r.userid 
where  t.conferenceid='{0}' and t.reviewstate='2'", keyValue);
            dt = this.BaseRepository().FindTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    dynamic dy = new ExpandoObject();
                    dy.username = dr["username"].ToString();
                    dy.photourl = dr["photourl"].ToString();
                    dy.deptname = dr["deptname"].ToString();
                    dataQ.Add(dy);
                }
            }
            else
            {
                dataQ = new List<Object>();
            }
            dataList.Add(dataQ);
            return dataList;
        }

        
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, ConferenceUserEntity entity)
        {
            string sql = string.Format(" select id from BIS_ConferenceUser t where t.ConferenceID='{0}' and UserID='{1}'", keyValue, entity.UserID);
            keyValue = this.BaseRepository().FindObject(sql).ToString();
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }
        #endregion
    }
}
