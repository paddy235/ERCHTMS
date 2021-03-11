using ERCHTMS.Entity.SafetyMeshManage;
using ERCHTMS.IService.SafetyMeshManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.Data;
using System.Linq.Expressions;
using System;
using ERCHTMS.Code;

namespace ERCHTMS.Service.SafetyMeshManage
{
    /// <summary>
    /// 描 述：安全网络
    /// </summary>
    public class SafetyMeshService : RepositoryFactory<SafetyMeshEntity>, SafetyMeshIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SafetyMeshEntity> GetList()
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取DataTable
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetTable(string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string sql = "";
            string orgCode = "";
            string whereStr = " 1= 1";
            if (!string.IsNullOrEmpty(queryJson))
            {
                var queryParam = queryJson.ToJObject();
                int mode = queryParam["mode"].ToInt();
                if (!queryParam["orgCode"].IsEmpty())
                {
                    orgCode = queryParam["orgCode"].ToString();
                }
                //查询条件
                if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
                {
                    string condition = queryParam["condition"].ToString();
                    string keyord = queryParam["keyword"].ToString().Trim();
                    switch (condition)
                    {
                        case "meshname":          //名称
                            whereStr += string.Format(" and meshname like'%{0}%' or meshname2 like'%{0}%' or meshname3 like'%{0}%' or meshname4 like'%{0}%'", keyord);
                            break;
                        case "dutyuser":          //责任人
                            whereStr += string.Format(" and dutyuser like'%{0}%' or dutyuser2 like'%{0}%' or dutyuser3 like'%{0}%' or dutyuser4 like'%{0}%'", keyord);
                            break;
                        default:
                            break;
                    }
                }
                sql = string.Format(@"select * from (
select d.id tid,
       d.createdate,
       d.createuserdeptcode,
       d.meshname,
       d.superiorid,
       d.superiorname,
       d.MeshRank,
       d.SortCode,
       d.dutyuser,
       d.dutytel,
       c.id           id2,
       c.createdate createdate2,
       c.createuserdeptcode createuserdeptcode2,
       c.meshname meshname2,
       c.superiorid   superiorid2,
       c.superiorname superiorname2,
       c.MeshRank     MeshRank2,
       c.SortCode     SortCode2,
       c.dutyuser     dutyuser2,
       c.dutytel dutytel2,
       b.id           id3,
       b.createdate createdate3,
       b.createuserdeptcode createuserdeptcode3,
       b.meshname meshname3,
       b.superiorid   superiorid3,
       b.superiorname superiorname3,
       b.MeshRank     MeshRank3,
       b.SortCode     SortCode3,
       b.dutyuser     dutyuser3,
       b.dutytel dutytel3,
       a.id           id4,
       a.createdate createdate4,
       a.createuserdeptcode createuserdeptcode4,
       a.meshname meshname4,
       a.superiorid   superiorid4,
       a.superiorname superiorname4,
       a.MeshRank     MeshRank4,
       a.SortCode     SortCode4,
       a.dutyuser     dutyuser4,
       a.dutytel dutytel4
  from HD_SAFETYMESH a
  left join HD_SAFETYMESH b
    on a.superiorid = b.id
  left join HD_SAFETYMESH c
    on b.superiorid = c.id
  left join HD_SAFETYMESH d
    on c.superiorid = d.id
 where a.meshrank = '4' and a.createuserorgcode='{0}'
union all
select c.id,
       c.createdate,
       c.createuserdeptcode,
       c.meshname,
       c.superiorid,
       c.superiorname,
       c.MeshRank,
       c.SortCode,
       c.dutyuser,
       c.dutytel,
       b.id id2,
       b.createdate createdate2,
       b.createuserdeptcode createuserdeptcode2,
       b.meshname meshname2,
       b.superiorid superiorid2,
       b.superiorname superiorname2,
       b.MeshRank MeshRank2,
       b.SortCode SortCode2,
       b.dutyuser dutyuser2,
       b.dutytel dutytel2,
       a.id id3,
       a.createdate createdate3,
       a.createuserdeptcode createuserdeptcode3,
       a.meshname meshname3,
       a.superiorid superiorid3,
       a.superiorname superiorname3,
       a.MeshRank MeshRank3,
       a.SortCode SortCode3,
       a.dutyuser dutyuser3,
       a.dutytel dutytel3,
       N'' id4,
       null createdate4,
       N'' createuserdeptcode4,
       N'' meshname4,
       N'' superiorid4,
       N'' superiorname4,
       N'' MeshRank4,
       null SortCode4,
       N'' dutyuser4,
       N'' dutytel4
  from HD_SAFETYMESH a
  left join HD_SAFETYMESH b on a.superiorid=b.id
  left join HD_SAFETYMESH c on b.superiorid=c.id
  where a.meshrank='3'  and a.createuserorgcode='{0}'
  and not exists(
   select 1 from HD_SAFETYMESH a1
  left join HD_SAFETYMESH b1 on a1.superiorid=b1.id
  where a1.meshrank='4' and b1.id=a.id)
  union all
select b.id,
       b.createdate,
       b.createuserdeptcode,
       b.meshname,
       b.superiorid,
       b.superiorname,
       b.MeshRank,
       b.SortCode,
       b.dutyuser,
       b.dutytel,
       a.id id2,
       a.createdate createdate2,
       a.createuserdeptcode createuserdeptcode2,
       a.meshname meshname2,
       a.superiorid superiorid2,
       a.superiorname superiorname2,
       a.MeshRank MeshRank2,
       a.SortCode SortCode2,
       a.dutyuser dutyuser2,
       a.dutytel dutytel2,
       N'' id3,
       null createdate3,
       N'' createuserdeptcode3,
       N'' meshname3,
       N'' superiorid3,
       N'' superiorname3,
       N'' MeshRank3,
       null SortCode3,
       N'' dutyuser3,
       N'' dutytel3,
       N'' id4,
       null createdate4,
       N'' createuserdeptcode4,
       N'' meshname4,
       N'' superiorid4,
       N'' superiorname4,
       N'' MeshRank4,
       null SortCode4,
       N'' dutyuser4,
       N'' dutytel4
  from HD_SAFETYMESH a
  left join HD_SAFETYMESH b on a.superiorid=b.id
  where a.meshrank='2'  and a.createuserorgcode='{0}'
  and not exists(
   select 1 from HD_SAFETYMESH a1
  left join HD_SAFETYMESH b1 on a1.superiorid=b1.id
  left join HD_SAFETYMESH c1 on b1.superiorid=c1.id
  where (a1.meshrank='4' and c1.id=a.id) or (a1.meshrank='3' and b1.id=a.id))
  union all
select a.id,
       a.createdate,
       a.createuserdeptcode,
       a.meshname,
       a.superiorid,
       a.superiorname,
       a.MeshRank,
       a.SortCode,
       a.dutyuser,
       a.dutytel,
       N'' id2,
       null createdate2,
       N'' createuserdeptcode2,
       N'' meshname2,
       N'' superiorid2,
       N'' superiorname2,
       N'' MeshRank2,
       null SortCode2,
       N'' dutyuser2,
       N'' dutytel2,
       N'' id3,
       null createdate3,
       N'' createuserdeptcode3,
       N'' meshname3,
       N'' superiorid3,
       N'' superiorname3,
       N'' MeshRank3,
       null SortCode3,
       N'' dutyuser3,
       N'' dutytel3,
       N'' id4,
       null createdate4,
       N'' createuserdeptcode4,
       N'' meshname4,
       N'' superiorid4,
       N'' superiorname4,
       N'' MeshRank4,
       null SortCode4,
       N'' dutyuser4,
       N'' dutytel4
  from HD_SAFETYMESH a
  left join HD_SAFETYMESH b on a.superiorid=b.id
  where a.meshrank='1'  and a.createuserorgcode='{0}'
  and not exists(
   select 1 from HD_SAFETYMESH a1
  left join HD_SAFETYMESH b1 on a1.superiorid=b1.id
  where a1.meshrank='2' and b1.id=a.id)
  )s where {1} order by SortCode asc,createdate desc,superiorid desc,SortCode2 asc,
 createdate2 desc,superiorid2 desc,SortCode3 asc,createdate3 desc,superiorid3 desc,SortCode4 asc,createdate4 desc,superiorid4 desc", orgCode, whereStr);
            }
            return this.BaseRepository().FindTable(sql);
        }
        public DataTable GetTableList(string queryJson)
        {
            string sql = "";
            string orgCode = "";
            string whereStr = " 1= 1";
            if (!string.IsNullOrEmpty(queryJson))
            {
                var queryParam = queryJson.ToJObject();
                int mode = queryParam["mode"].ToInt();
                if (!queryParam["orgCode"].IsEmpty())
                {
                    orgCode = queryParam["orgCode"].ToString();
                }
                //查询条件
                if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
                {
                    string condition = queryParam["condition"].ToString();
                    string keyord = queryParam["keyword"].ToString().Trim();
                    switch (condition)
                    {
                        case "meshname":          //名称
                            whereStr += string.Format(" and meshname like'%{0}%' or meshname2 like'%{0}%' or meshname3 like'%{0}%' or meshname4 like'%{0}%'", keyord);
                            break;
                        case "dutyuser":          //责任人
                            whereStr += string.Format(" and dutyuser like'%{0}%' or dutyuser2 like'%{0}%' or dutyuser3 like'%{0}%' or dutyuser4 like'%{0}%'", keyord);
                            break;
                        default:
                            break;
                    }
                }
                sql = string.Format(@"select a.id tid,
       a.meshname,
       a.superiorid,
       a.superiorname,
       case when MeshRank = '1' then '一级网格' when MeshRank = '2' then '二级网格' when MeshRank = '3' then '三级网格' when MeshRank = '微级网格' then '其他' else '' end as MeshRank,
       a.SortCode,
       a.dutyuser,
       a.dutytel,
       a.district,
       a.workjob
  from HD_SAFETYMESH a where {0} order by createdate desc", whereStr);
            }
            return this.BaseRepository().FindTable(sql);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SafetyMeshEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        public IEnumerable<SafetyMeshEntity> GetListForCon(Expression<Func<SafetyMeshEntity, bool>> condition)
        {

            return this.BaseRepository().IQueryable(condition).ToList();
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            //this.BaseRepository().Delete(keyValue);
            //this.BaseRepository().Delete(it => it.SuperiorId == keyValue);
            string[] arr = keyValue.Split(',');
            foreach (string id in arr)
            {

                this.BaseRepository().ExecuteBySql(string.Format("delete from HD_SAFETYMESH where id in (select t.id from HD_SAFETYMESH t start with id='{0}' connect by prior t.id=t.superiorid)", id));
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, SafetyMeshEntity entity)
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
        #endregion
    }
}
