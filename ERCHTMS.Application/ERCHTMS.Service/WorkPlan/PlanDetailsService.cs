using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.WebControl;
using ERCHTMS.Code;
using ERCHTMS.Entity.WorkPlan;
using ERCHTMS.IService.WorkPlan;
using ERCHTMS.Service.BaseManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ERCHTMS.Service.WorkPlan
{
    /// <summary>
    /// 描 述：工作计划详情
    /// </summary>
    public class PlanDetailsService : RepositoryFactory<PlanDetailsEntity>, PlanDetailsIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<PlanDetailsEntity> GetList(string queryJson)
        {
            var sql = string.Format("select * from hrs_plandetails where 1=1 {0}", queryJson);
            return this.BaseRepository().FindList(sql);
        }
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var queryParam = queryJson.ToJObject();
            if (pagination.p_fields.IsEmpty())
            {
                pagination.p_fields = @"createdate,createuserid,createuserdeptcode,createuserorgcode,modifydate,modifyuserid,contents,dutydepartid,dutydepartname,dutyuserid,dutyusername,planfindate,'' as plandate,realfindate,stdname,(select flowstate from hrs_planapply where hrs_planapply.id=hrs_plandetails.applyid) as flowstate,'' as state,(select count(1) from hrs_plandetails s where s.baseid=hrs_plandetails.id) as changed,ischanged,baseid,applyid,ismark";
            }
            pagination.p_kid = "id";
            pagination.p_tablename = @"hrs_plandetails";
            pagination.conditionJson = string.Format(" createuserorgcode='{0}' ", user.OrganizeCode);
            //申请id
            if (!queryParam["applyid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and applyid = '{0}'", queryParam["applyid"].ToString());
            }
            //台帐列表
            if (!queryParam["plandetailsindex"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and exists(select 1 from hrs_planapply a where a.id=hrs_plandetails.applyid and a.createuserorgcode='{0}' and  a.flowstate='结束')", user.OrganizeCode, user.UserId, user.Account);
            }
            //引用编号
            if (!queryParam["baseid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and baseid = '{0}'", queryParam["baseid"].ToString());
            }
            //开始时间
            if (!queryParam["starttime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and planfindate >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(queryParam["starttime"].ToString()).ToString("yyyy-MM-dd"));
            }
            //结束时间
            if (!queryParam["endtime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and planfindate < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(queryParam["endtime"].ToString()).AddMonths(1).ToString("yyyy-MM-dd"));
            }
            //部门id
            if (!queryParam["deptid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and createuserdeptcode = '{0}'", queryParam["deptid"].ToString());
            }
            //部门编码
            if (!queryParam["deptcode"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and createuserdeptcode like '{0}%'", queryParam["deptcode"].ToString());
            }
            //申请类型
            if (!queryParam["applytype"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and applyid in (select id from hrs_planapply where applytype='{0}')", queryParam["applytype"].ToString());
            }
            //申请审批通过
            if (!queryParam["applychecked"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and applyid in (select id from hrs_planapply where flowstate='结束')");
            }
            //工作内容
            if (!queryParam["contents"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and contents  like '%{0}%'", queryParam["contents"].ToString());
            }
            //我的工作记录
            if (!queryParam["ismy"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and dutyuserid = '{0}'", user.UserId);
            }
            //原始数据
            if (!queryParam["isavailable"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and baseid is null");
            }
            //未删除数据
            if (!queryParam["iscancel"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and iscancel = 0");
            }
            //完成情况
            if (!queryParam["state"].IsEmpty())
            {
                var state = queryParam["state"].ToString();
                if (state=="未完成")
                {
                    pagination.conditionJson += string.Format(@" and realfindate is null and planfindate < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));
                }
                else if (state == "超期完成")
                {
                    pagination.conditionJson += " and realfindate is not null and realfindate>planfindate";
                }
                else if (state == "按时完成")
                {
                    pagination.conditionJson += " and realfindate is not null and realfindate<=planfindate";
                }
                else if (state == "待完成")
                {
                    pagination.conditionJson += string.Format(@" and realfindate is null and planfindate >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", DateTime.Now.ToString("yyyy-MM-dd"));
                }                
            }
            var dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);

            return dt;
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public PlanDetailsEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion

        #region 统计
        /// <summary>
        /// 统计工作计划
        /// </summary>
        /// <param name="deptId"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public DataTable Statistics(string deptId, string starttime, string endtime,string applytype="")
        {
            DataTable dt = null;

            if (!string.IsNullOrEmpty(deptId))
            {
                string stWhere = "";
                if (!string.IsNullOrEmpty(starttime))
                {
                    stWhere = string.Format(" and planfindate >= to_date('{0}','yyyy-mm-dd hh24:mi:ss') ", Convert.ToDateTime(starttime).ToString("yyyy-MM-dd"));
                }
                string edWhere = "";
                if (!string.IsNullOrEmpty(endtime))
                {
                    edWhere = string.Format(" and planfindate < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(endtime).AddMonths(1).ToString("yyyy-MM-dd"));
                }
                string atWhere = "";
                if (!string.IsNullOrWhiteSpace(applytype))
                {
                    atWhere = string.Format(" and exists(select 1 from hrs_planapply a where a.id=hrs_plandetails.applyid and a.applytype='{0}')", applytype);
                }
                string sql = string.Format(@"select encode,fullname,plannum,realnum,(case when plannum=0 then 0 else round(realnum/plannum,4)*100 end) pct from 
(select encode, fullname,
(select count(1) from hrs_plandetails where createuserdeptcode like t.encode || '%' {0} {1} {2} and baseid is null and exists(select 1 from hrs_planapply a where a.id = hrs_plandetails.applyid and a.flowstate = '结束')) as plannum,
(select count(1) from hrs_plandetails where createuserdeptcode like t.encode || '%'  {0} {1} {2} and baseid is null and realfindate is not null and realfindate < planfindate and  exists(select 1 from hrs_planapply a where a.id = hrs_plandetails.applyid and a.flowstate = '结束')) as realnum
from(
select encode, fullname from base_department where departmentid = '{3}'
union all
select encode, fullname from(select encode, fullname from base_department where parentid = '{3}' order by sortcode)
) t)", stWhere, edWhere, atWhere, deptId);

                dt = this.BaseRepository().FindTable(sql);
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
            this.BaseRepository().Delete(x => x.ID == keyValue || x.BaseId == keyValue);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue"></param>
        public void RemoveFormByApplyId(string keyValue)
        {
            var resp = this.BaseRepository();
            var list = this.GetList(string.Format(" and baseid in(select id from hrs_plandetails where applyid='{0}')", keyValue)).ToList();
            if (list != null && list.Count > 0)
                resp.Delete(list);
            resp.Delete(x => x.ApplyId == keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, PlanDetailsEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                var old = GetEntity(keyValue);
                if (old == null)
                {
                    entity.Create();
                    entity.ID = keyValue;
                    this.BaseRepository().Insert(entity);
                }
                else
                {
                    entity.Modify(keyValue);                    
                    this.BaseRepository().Update(entity);
                }                
            }
            else
            {
                entity.Create();               
                this.BaseRepository().Insert(entity);
            }
        }
        /// <summary>
        /// 更新变更状态
        /// </summary>
        /// <param name="applyId"></param>
        public void UpdateChangedData(string applyId)
        {
            var resp = this.BaseRepository();
            //更新相关工作计划有历史内容
            string sql = string.Format("update hrs_plandetails set ischanged=0,ismark=1 where applyId='{0}'", applyId);
            resp.ExecuteBySql(sql);
            sql = string.Format("update hrs_plandetails set ischanged=0,ismark=1 where baseid in(select id from hrs_plandetails where applyId='{0}')", applyId);
            resp.ExecuteBySql(sql);
            //删除相关工作计划及历史内容
            sql = string.Format("delete from hrs_plandetails where baseid in(select id from hrs_plandetails where iscancel=1 and applyId='{0}')", applyId);
            resp.ExecuteBySql(sql);
            sql = string.Format("delete from hrs_plandetails where iscancel=1 and applyId='{0}'", applyId);
            resp.ExecuteBySql(sql);
            //删除无用的工作计划内容
            sql = string.Format("delete from hrs_plandetails where not exists(select 1 from hrs_planapply a where a.id=hrs_plandetails.applyid)");
            resp.ExecuteBySql(sql);
        }
        #endregion
    }
}
