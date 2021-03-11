using ERCHTMS.Entity.StandardSystem;
using ERCHTMS.IService.StandardSystem;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Data;
using System.Data;
using ERCHTMS.Code;

namespace ERCHTMS.Service.StandardSystem
{
    /// <summary>
    /// 描 述：标准体系
    /// </summary>
    public class StandardsystemService : RepositoryFactory<StandardsystemEntity>, StandardsystemIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                if (queryJson.Length > 0)
                {
                    var queryParam = queryJson.ToJObject();
                    if (!queryParam["standardtype"].IsEmpty())
                    {
                        pagination.conditionJson += " and a.standardtype='" + queryParam["standardtype"].ToString() + "'";
                    }
                    if (!queryParam["enCode"].IsEmpty())
                    {
                        pagination.conditionJson += " and b.id in (select id from hrs_stcategory where encode like '" + queryParam["enCode"].ToString() + "%')";
                    }
                    if (!queryParam["filename"].IsEmpty())
                    {
                        pagination.conditionJson += " and filename like '%" + queryParam["filename"].ToString() + "%'";
                    }
                    if (!queryParam["station"].IsEmpty())
                    {
                        string[] PostidList = queryParam["station"].ToString().Replace("，", ",").Split(',');
                        string forsql = " and (";
                        foreach (var item in PostidList)
                        {
                            forsql += "stationname like '%" + item.ToString() + "%' or";
                        }
                        if (forsql.Length > 6)
                        {
                            forsql = forsql.Substring(0, forsql.Length - 2);
                        }
                        forsql += ")";
                        pagination.conditionJson += forsql;
                    }
                    if (!queryParam["keyword"].IsEmpty())
                    {
                        pagination.conditionJson += " and (stationname like '%" + queryParam["keyword"].ToString() + "%' or filename like '%" + queryParam["keyword"].ToString() + "%')";
                    }
                    //以下合规性评价专用条件
                    if (!queryParam["standardtypestr"].IsEmpty())
                    {
                        pagination.page = 1;
                        pagination.rows = 1000000000;
                        pagination.conditionJson += " and a.standardtype in("+ queryParam["standardtypestr"].ToString() + ")";
                    }
                    if (!queryParam["timeliness"].IsEmpty())
                    {
                        pagination.conditionJson += " and a.timeliness ='" + queryParam["timeliness"].ToString() + "'";
                    }
                    if (!queryParam["carrydate"].IsEmpty())
                    {
                        pagination.conditionJson += " and to_char(carrydate,'yyyy')=" + queryParam["carrydate"].ToString();
                    }
                }
                return this.BaseRepository().FindTableByProcPager(pagination, DbHelper.DbType);
            }
            catch (System.Exception ex)
            {

                throw;
            }
            
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<StandardsystemEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public StandardsystemEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }



        //public DataTable GetStandardCount()
        //{
        //    Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
        //    string forsql = "";
        //    if (!user.PostName.IsEmpty())
        //    {
        //        string[] PostidList = user.PostName.Replace("，", ",").Split(',');
        //        forsql = " and (";
        //        foreach (var item in PostidList)
        //        {
        //            forsql += "stationname like '%" + item.ToString() + "%' or";
        //        }
        //        if (forsql.Length > 6)
        //        {
        //            forsql = forsql.Substring(0, forsql.Length - 2);
        //        }
        //        forsql += ")";
        //    }
        //    string sql = @"select  (select count(1)  from hrs_standardsystem where createuserorgcode = '" + user.OrganizeCode + @"' and standardtype=1 " + forsql + @" and add_months( createdate,1) > sysdate) as type1,
        //        (select count(1)  from hrs_standardsystem where createuserorgcode = '" + user.OrganizeCode + @"' and standardtype = 2 " + forsql + @" and add_months( createdate,1) > sysdate) as type2,
        //        (select count(1)  from hrs_standardsystem where createuserorgcode = '" + user.OrganizeCode + @"' and standardtype = 3 " + forsql + @" and add_months( createdate,1) > sysdate) as type3,
        //        (select count(1)  from hrs_standardsystem where createuserorgcode = '" + user.OrganizeCode + @"' and standardtype = 4 " + forsql + @" and add_months( createdate,1) > sysdate) as type4,
        //        (select count(1)  from hrs_standardsystem where createuserorgcode = '" + user.OrganizeCode + @"' and standardtype = 5 " + forsql + @" and add_months( createdate,1) > sysdate) as type5,
        //        (select count(1)  from hrs_standardsystem where createuserorgcode = '" + user.OrganizeCode + @"' and standardtype = 6 " + forsql + @" and add_months( createdate,1) > sysdate) as type6 from dual";
        //    return this.BaseRepository().FindTable(sql);
        //}

        /// <summary>
        /// 工作主页获取标准数量
        /// </summary>
        /// <returns></returns>
        public DataTable GetStandardCount()
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string forsql = "";
            DataTable dt = new DataTable();
            dt.Columns.Add("num");
            dt.Columns.Add("standardtype");
            dt.Columns.Add("standardtypename");
            if (!user.PostId.IsEmpty())
            {
                string[] PostidList = user.PostId.Replace("，", ",").Split(',');
                forsql = " and (";
                foreach (var item in PostidList)
                {
                    forsql += "stationid like '%" + item.ToString() + "%' or";
                }
                if (forsql.Length > 6)
                {
                    forsql = forsql.Substring(0, forsql.Length - 2);
                }
                forsql += ")";
            }
            string[] strlist = { "技术标准体系", "管理标准体系", "岗位标准体系", "上级标准化文件", "指导标准", "法律法规", "标准体系策划与构建", "标准体系评价与改进", "标准化培训" };
            for (int i = 1; i < 10; i++)
            {
                if (i!=7 && i!=8)
                {
                    DataRow dtrow = dt.NewRow();
                    string sql = @" select count(1) as num from hrs_standardsystem a left join hrs_standardreadrecord b on a.id = b.recid and 
                            b.createuserid = '" + user.UserId + @"' where a.createuserorgcode = '" + user.OrganizeCode + @"' and standardtype ='" + i + "' " + forsql + @" and b.recid is null";
                    DataTable dttemp = this.BaseRepository().FindTable(sql);
                    if (dttemp.Rows.Count>0)
                    {
                        dtrow["num"] = dttemp.Rows[0]["num"].ToString();
                        dtrow["standardtype"] = i;
                        dtrow["standardtypename"] = strlist[i-1].ToString();
                        dt.Rows.Add(dtrow);
                    }
                }
            }

            return dt;
            
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
        /// 删除标准种类同步删除相应种类下已经有的数据
        /// </summary>
        /// <param name="ids"></param>
        public void RemoveCategoryForms(string ids)
        {
            this.BaseRepository().ExecuteBySql("delete hrs_standardsystem where categorycode in (" + ids + ")");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, StandardsystemEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                StandardsystemEntity se = this.BaseRepository().FindEntity(keyValue);
                if (se == null)
                {
                    entity.ID = keyValue;
                    entity.Create();
                    this.BaseRepository().Insert(entity);


                }
                else
                {
                    entity.Modify(keyValue);
                    if (entity.CONSULTNUM == 0)
                    {
                        entity.CONSULTNUM = se.CONSULTNUM;
                    }
                    entity.CREATEUSERDEPTNAME = null;
                    entity.CATEGORYNAME = null;
                    this.BaseRepository().Update(entity);
                }
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
