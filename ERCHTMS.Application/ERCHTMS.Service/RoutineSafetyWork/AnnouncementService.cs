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
using ERCHTMS.Code;
using System;

namespace ERCHTMS.Service.RoutineSafetyWork
{
    /// <summary>
    /// 描 述：通知公告
    /// </summary>
    public class AnnouncementService : RepositoryFactory<AnnouncementEntity>, AnnouncementIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson, string authType)
        {
            DatabaseType dataType = DbHelper.DbType;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var queryParam = queryJson.ToJObject();

            if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级"))
            {
                pagination.conditionJson += " and  t.createuserorgcode='" + user.OrganizeCode + "'";
            }
            else
            {
                pagination.conditionJson += string.Format(" and ((t.createuserdeptcode in(select encode from base_department  where encode like '{0}%' union select b.encode from epg_outsouringengineer  a left join base_department b on a.outprojectid=b.departmentid  where  a.engineerletdeptid='{1}')) or (d.userid='{2}'))", user.DeptCode, user.DeptId,user.UserId);
            }
            if (authType == "app")
            {
                pagination.conditionJson += " and t.issend='0'";
            }
            else
            {
                pagination.conditionJson += string.Format("  and  t.id  not in(select id from bis_announcement where issend='1' and  createuserid!='{0}')", user.UserId);
            }

            #region 查表
            pagination.p_kid = "t.id";
            pagination.p_fields = @"t.content,t.createuserid,t.title,t.publisher,t.publisherid,t.publisherdept,t.issuerangedept,t.NoticType,
            t.issuerangedeptname,t.releasetime,t.isimportant,t.issend,nvl(p.undonenum,0) undonenum,nvl(m.donenum,0) donenum,d.status,d.username,d.userid";
            pagination.p_tablename =string.Format(@"bis_announcement t
left join (select count(id) undonenum,u.auuounid from  bis_announdetail u where u.status=0 group by u.auuounid) p on p.auuounid=t.id
left join (select count(id) donenum,u.auuounid from  bis_announdetail u where u.status=1 group by u.auuounid) m on m.auuounid=t.id
left join bis_announdetail d on d.auuounid=t.id and d.userid='{0}'", user.UserId);
            if (pagination.sidx == null) {
                pagination.sidx = "t.createdate";
            }
            if (pagination.sord == null) {
                pagination.sord = "desc";
            }
            #endregion
            //年度
            if (!queryParam["year"].IsEmpty())
            {
                if (queryParam["year"].ToString() != "全部")
                    pagination.conditionJson += string.Format(" and to_char(t.releasetime,'yyyy')='{0}'", queryParam["year"].ToString());
            }
            //查询条件
            if (!queryParam["title"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.title like '%{0}%'", queryParam["title"].ToString());
            }
            if (!queryParam["code"].IsEmpty() && !queryParam["isOrg"].IsEmpty())
            {
                string deptCode = queryParam["code"].ToString();
                if (queryParam["isOrg"].ToString() == "Organize")
                {
                    pagination.conditionJson += string.Format(" and t.createuserorgcode  like '{0}%'", deptCode);
                }

                else
                {
                    pagination.conditionJson += string.Format(" and t.createuserdeptcode like '{0}%'", deptCode);
                }
            }
            if (!queryParam["showrange"].IsEmpty())
            {
                var showRange = queryParam["showrange"].ToString();
                if (showRange == "1")//本人发布
                {
                    pagination.conditionJson += string.Format(" and t.createuserid='{0}'", user.UserId);
                }
                else if (showRange == "2")//本人接收
                {
                    pagination.conditionJson += string.Format(" and d.userid='{0}'", user.UserId);
                }
            }
            if (!queryParam["NoticType"].IsEmpty())
            {
                var notictype = queryParam["NoticType"].ToString();
                if (!notictype.IsNullOrWhiteSpace()) pagination.conditionJson += string.Format(" and t.NoticType='{0}'", notictype);
            }
            if (!queryParam["Status"].IsEmpty()) 
            {
                var status = queryParam["Status"].ToString();
                if (!status.IsNullOrWhiteSpace()) pagination.conditionJson += string.Format(" and d.Status='{0}'", status);
            }
            if (!queryParam["keyword"].IsEmpty())
            {
                var keyword = queryParam["keyword"].ToString();
                if (!keyword.IsNullOrWhiteSpace()) pagination.conditionJson += string.Format(" and t.TITLE like '%{0}%'", keyword);
            }
            DataTable data = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            return data;
        }
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            if (!string.IsNullOrWhiteSpace(queryJson))
            {
                var queryParam = queryJson.ToJObject();
                if (!queryParam["showrange"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and t.createuserid='{0}'", "");
                }
            }
            DataTable data = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            return data;
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<AnnouncementEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public AnnouncementEntity GetEntity(string keyValue)
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
            var res = DbFactory.Base().BeginTrans();
            try
            {
                string sql = string.Format("delete from bis_announdetail t where t.auuounid='{0}'", keyValue);
                res.ExecuteBySql(sql);
                res.Delete<AnnouncementEntity>(keyValue);
                res.Commit();
            }
            catch (Exception)
            {
                res.Rollback();
                throw;
            }
           
           
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, AnnouncementEntity entity)
        {
            var res = DbFactory.Base().BeginTrans();
            try
            {
                Repository<AnnouncementEntity> announRes = new Repository<AnnouncementEntity>(DbFactory.Base());
                var sl = announRes.FindEntity(keyValue);
                if (sl == null)
                {
                    entity.Id = keyValue;
                    entity.Create();
                    res.Insert<AnnouncementEntity>(entity);
                }
                else
                {
                    entity.Modify(keyValue);
                    res.Update<AnnouncementEntity>(entity);
                }
                string sql = string.Format("delete from bis_announdetail t where t.auuounid='{0}'", entity.Id);
                res.ExecuteBySql(sql);

                string insertSql = string.Format(@"insert into bis_announdetail(id,createuserid,createuserdeptcode,createuserorgcode,
                                                                                createdate,createusername,username,useraccount,
                                                                                userid,deptid,deptname,deptcode,status,auuounid) 
                                                                        select newguid(),'{0}','{1}','{2}',sysdate,'{5}',u.realname,u.account,
                                                                               u.userid,u.departmentid,d.fullname,u.departmentcode,0,'{3}' 
                                                                        from base_user u
                                                                            left join base_department d on d.departmentid=u.departmentid
                                                                        where u.departmentcode in('{4}') and u.userid !='{6}' and u.ispresence='1' and (u.isblack=0 or u.isblack is null)"
                                                                         , entity.CreateUserId, entity.CreateUserDeptCode, entity.CreateUserOrgCode, entity.Id, string.Join("','", entity.IssueRangeDeptCode.Split(',')), entity.CreateUserName, entity.PublisherId);
                res.ExecuteBySql(insertSql);
                res.Commit();
            }
            catch (Exception)
            {
                res.Rollback();
                throw;
            }

        }
        #endregion
    }
}
