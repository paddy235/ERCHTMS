using ERCHTMS.Entity.PersonManage;
using ERCHTMS.IService.PersonManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System;
using BSFramework.Data;
using System.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
namespace ERCHTMS.Service.PersonManage
{
    /// <summary>
    /// 描 述：人员证书
    /// </summary>
    public class BlacklistService : RepositoryFactory<BlacklistEntity>, BlacklistIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>返回列表</returns>
        public IEnumerable<BlacklistEntity> GetList(string userId)
        {
            return this.BaseRepository().IQueryable(t=>t.UserId==userId).ToList();
        }
        /// <summary>
        /// 获取满足黑名单条件的人员
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable GetBlacklistUsers(ERCHTMS.Code.Operator user)
        {
            DataTable dtTemp = new DataTable();
            dtTemp.Columns.Add("userid");
            dtTemp.Columns.Add("realname");
            dtTemp.Columns.Add("deptname");
            dtTemp.Columns.Add("itemname");
            dtTemp.Columns.Add("remark");
            DataTable dtItems = BaseRepository().FindTable(string.Format("select itemvalue,itemcode,remark from BIS_BLACKSET where status=1 and deptcode='{0}'", user.OrganizeCode));
            foreach (DataRow dr in dtItems.Rows)
            {
                DataTable dt =null;
                string sql = "";
                //年龄判断
                if (dr[1].ToString() == "01")
                {
                    string[] arr = dr[0].ToString().Split('|');
                    sql = string.Format("select userid,realname,u.DEPTNAME,'" + dr[2].ToString() + @"' itemname,('出生日期为：' || to_char(birthday,'yyyy-MM-dd')) remark from v_userinfo u where isblack=0 and  gender='{0}' and birthday is not null and u.DEPARTMENTCODE like '{3}%'
and (round(sysdate-to_date(to_char(u.birthday,'yyyy-MM-dd'),'yyyy-MM-dd'))/365<{1} or round(sysdate-to_date(to_char(u.birthday,'yyyy-MM-dd'),'yyyy-MM-dd'))/365>{2})", "男", arr[0], arr[1], user.DeptCode);
                    dt = BaseRepository().FindTable(sql);
                    dtTemp.Merge(dt);

                    sql = string.Format("select userid,realname,u.DEPTNAME,'" + dr[2].ToString() + @"' itemname,('出生日期为：' || to_char(birthday,'yyyy-mm-dd')) remark  from v_userinfo u where isblack=0 and gender='{0}' and birthday is not null and u.DEPARTMENTCODE like '{3}%'
and (round(sysdate-to_date(to_char(u.birthday,'yyyy-mm-dd'),'yyyy-MM-dd'))/365<to_number({1}) or round(sysdate-to_date(to_char(u.birthday,'yyyy-mm-dd'),'yyyy-MM-dd'))/365>to_number({2})) ", "女", arr[2], arr[3], user.DeptCode);
                    dt = BaseRepository().FindTable(sql);
                    dtTemp.Merge(dt);
                }
                 //一般违章
                if (dr[1].ToString() == "03")
                {
                    sql = string.Format("select t.lllegalpersonid userid,t.lllegalperson realname,'" + dr[2].ToString() + @"' itemname,LLLEGALTEAM deptname,('一般违章次数:' || count(1)) remark from V_LLLEGALBASEINFO t where t.flowstate='流程结束' and LLLEGALLEVEL='fc53ff18-b212-4763-9760-baf476eea5f3' and LLLEGALTEAMcode like '{1}%' and lllegalpersonid in(select userid from v_userinfo where isblack=0 and organizecode='{2}' ) group by 
lllegalpersonid,lllegalperson,LLLEGALTEAM having count(1)>{0}",  dr[0].ToString(), user.DeptCode,user.OrganizeCode);
                    dt = BaseRepository().FindTable(sql);
                    dtTemp.Merge(dt);
                }
                //严重违章
                if (dr[1].ToString() == "04")
                {
                    sql = string.Format(@"select t.lllegalpersonid userid,t.lllegalperson realname,'" + dr[2].ToString() + @"' itemname,LLLEGALTEAM deptname,('严重违章:' || count(1)) remark from V_LLLEGALBASEINFO t where t.flowstate='流程结束' and LLLEGALLEVEL='5aae9e88-c06d-4383-afec-6165d5c1a312' and LLLEGALTEAMcode like '{1}%' and lllegalpersonid in(select userid from v_userinfo where isblack=0 and organizecode='{2}' ) group by 
lllegalpersonid,lllegalperson,LLLEGALTEAM having count(1)>{0}", dr[0].ToString(), user.DeptCode,user.OrganizeCode);
                    dt = BaseRepository().FindTable(sql);
                    dtTemp.Merge(dt);
                }
            }
            return dtTemp;
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
            //状态
            if (!queryParam["status"].IsEmpty())
            {
                string status = queryParam["status"].ToString();
                pagination.conditionJson += string.Format(" and status ={0}", status);
            }
            //区域Code
            if (!queryParam["areaCode"].IsEmpty())
            {
                string areaCode = queryParam["areaCode"].ToString();
                pagination.conditionJson += string.Format(" and areaCode like '{0}%'", areaCode);
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public BlacklistEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            BlacklistEntity entity = this.BaseRepository().FindEntity(keyValue);
            entity.DeleteMark = entity.EnableMark = 1;
            if(this.BaseRepository().Update(entity)>0)
            {
                this.BaseRepository().ExecuteBySql(string.Format("update base_user set isblack=0 where userid='{0}'", entity.UserId));
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, BlacklistEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                string[] arr = entity.UserId.Split(',');
                List<BlacklistEntity> list = new List<BlacklistEntity>();
                foreach(string userId in arr)
                {
                    BlacklistEntity newEntity = new BlacklistEntity();
                    newEntity.Create();
                    newEntity.UserId = userId;
                    newEntity.Reason = entity.Reason;
                    newEntity.JoinTime = DateTime.Now;
                    list.Add(newEntity);
                }
                if(this.BaseRepository().Insert(list)>0)
                {
                    this.BaseRepository().ExecuteBySql(string.Format("update base_user set isblack=1 where userid in('{0}')", entity.UserId.Replace(",","','")));
                }
            }
        }
        #endregion
    }
}
