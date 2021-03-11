using ERCHTMS.Entity.PersonManage;
using ERCHTMS.IService.PersonManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
namespace ERCHTMS.Service.PersonManage
{
    /// <summary>
    /// 描 述：人员积分
    /// </summary>
    public class UserScoreService : RepositoryFactory<UserScoreEntity>, UserScoreIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<UserScoreEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return this.BaseRepository().FindList(pagination);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public  DataTable GetList(string userId)
        {
            return this.BaseRepository().FindTable(string.Format("select s.itemname,t.createdate as time,itemtype,t.score from  BIS_USERSCORE t left join base_user u on t.userid=u.userid left join bis_scoreset s on t.itemid=s.id where t.userid='{0}'",userId));
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public UserScoreEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 获取人员本年底积分和累计积分
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public string GetScoreInfo(string userId)
        {
            int score = 100;//select t.itemvalue from BASE_DATAITEMDETAIL t where t.itemdetailid=(select * from base_user u where u.userid='{0}')
            DataTable dt = BaseRepository().FindTable(string.Format("select t.itemvalue from BASE_DATAITEMDETAIL t where t.itemdetailid=(select organizeid from base_user u where u.userid='{0}')", userId));
            if(dt.Rows.Count>0)
            {
                score=dt.Rows[0][0].ToInt();
            }
            else
            {
                dt = BaseRepository().FindTable(string.Format("select t.itemvalue from BASE_DATAITEMDETAIL t where t.itemdetailid='{0}'", "csjf"));
                if (dt.Rows.Count > 0)
                {
                    score = dt.Rows[0][0].ToInt();
                }
            }
            string sql = string.Format("(select nvl(sum(score),0) from BIS_USERSCORE t where year='{1}' and userid='{0}') union all  (select nvl(sum(score),0) from BIS_USERSCORE t where userid='{0}')",userId,System.DateTime.Now.Year);
            dt = BaseRepository().FindTable(sql);
            int currScore=score + int.Parse(dt.Rows[0][0].ToString());
            score += int.Parse(dt.Rows[1][0].ToString());
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { currScore = currScore, sumScore = score });
        }
        /// <summary>
        /// 获取人员积分考核明细
        /// </summary>
        /// <param name="keyValue">记录Id</param>
        /// <returns></returns>
        public object GetInfo(string keyValue)
        {
            DataTable dt = this.BaseRepository().FindTable(string.Format("select t.score,t.userid,s.itemname,s.itemtype,u.realname,u.usertype,u.identifyid,t.itemid  from BIS_USERSCORE t left join bis_scoreset s on t.itemid=s.id left join base_user u on t.userid=u.userid where t.id='{0}'",keyValue));

            return new { UserId = dt.Rows[0]["userid"].ToString(), UserName = dt.Rows[0]["realname"].ToString(), Score = dt.Rows[0]["score"].ToString(), ItemId = dt.Rows[0]["itemid"].ToString(), ItemName = dt.Rows[0]["itemname"].ToString(), ItemType = dt.Rows[0]["itemtype"].ToString(), UserType = dt.Rows[0]["usertype"].ToString(), IdCard = dt.Rows[0]["identifyid"].ToString() };
        }
        /// <summary>
        /// 获取用户指定年份的积分
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="year">年度</param>
        /// <returns></returns>
        public decimal GetUserScore(string userId,string year)
        {
            object obj = BaseRepository().FindObject(string.Format("select sum(score) from bis_userscore where userid='{0}' and year='{1}'",userId,year));
            return obj == null || obj == System.DBNull.Value ? 0 : obj.ToDecimal();

        }
        /// <summary>
        /// 存储过程分页查询
        /// </summary>
        /// <param name="pagination">分页条件</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageJsonList(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;
            if (!string.IsNullOrEmpty(queryJson))
            {
                 var queryParam = queryJson.ToJObject();
                 if (!queryParam["userId"].IsEmpty())
                 {
                     string userId = queryParam["userId"].ToString().Trim();
                     pagination.conditionJson += string.Format(" and u.userId='{0}'", userId);
                 }
            //考核时间
            if (!queryParam["startDate"].IsEmpty() && !queryParam["endDate"].IsEmpty())
            {
                 string startDate = queryParam["startDate"].ToString().Trim();
                 string endDate = queryParam["endDate"].ToString().Trim();
                 pagination.conditionJson += string.Format(" and t.createdate between to_date('{0}','yyyy-mm-dd hh24:mi:ss') and to_date('{1} 23:59:59','yyyy-mm-dd hh24:mi:ss')", startDate,endDate);
            }
            //积分类型
            if (!queryParam["itemType"].IsEmpty())
            {
                string itemType = queryParam["itemType"].ToString().Trim();
                pagination.conditionJson += string.Format(" and itemType='{0}'", itemType);
            }
            //查询年份
            if (!queryParam["year"].IsEmpty())
            {
                string year = queryParam["year"].ToString().Trim();
                pagination.conditionJson += string.Format(" and year='{0}'", year);
            }
            //风险级别
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyword = queryParam["keyword"].ToString().Trim();
                pagination.conditionJson += string.Format(" and (realname like '%{0}%' or deptname like '%{0}%' or itemname  like '%{0}%')", keyword);
            }
            }
           
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
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
        public void SaveForm(string keyValue, UserScoreEntity entity)
        {
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
        /// <summary>
        /// 批量保存积分考核记录
        /// </summary>
        /// <param name="list"></param>
        public void Save(List<UserScoreEntity> list)
        {
            this.BaseRepository().Insert(list);
        }
        #endregion
    }
}
