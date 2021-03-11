using ERCHTMS.Entity.WorkPlan;
using ERCHTMS.IService.WorkPlan;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util.Extension;
using BSFramework.Util;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Service.BaseManage;
using ERCHTMS.Service.AuthorizeManage;
using ERCHTMS.Code;
using System;

namespace ERCHTMS.Service.WorkPlan
{
    /// <summary>
    /// 描 述：EHS计划申请表
    /// </summary>
    public class PlanApplyService : RepositoryFactory<PlanApplyEntity>, PlanApplyIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<PlanApplyEntity> GetList(string queryJson)
        {
            var sql = string.Format("select * from hrs_planapply where 1=1 {0}", queryJson);
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
                pagination.p_fields = @"createdate,createuserid,createuserdeptcode,createuserorgcode,modifydate,modifyuserid,userid,username,departid,departname,applytype,applydate,flowstate,checkuseraccount,case when flowstate='上报计划' then 1 when flowstate='结束' then 3 else 2 end num,(select count(1) from hrs_planapply a where a.baseid=hrs_planapply.id) as changed";
            }
            pagination.p_kid = "id";
            pagination.p_tablename = @"hrs_planapply";
            pagination.conditionJson = string.Format(" createuserorgcode='{0}' and (createuserid='{1}' or instr(checkuseraccount,'{2}')>0 or flowstate='结束')", user.OrganizeCode, user.UserId, user.Account);
            //开始时间
            if (!queryParam["starttime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and applydate >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", queryParam["starttime"].ToString());
            }
            //结束时间
            if (!queryParam["endtime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and applydate < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(queryParam["endtime"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
            }
            //引用编号 
            if (!queryParam["baseid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and baseid = '{0}'", queryParam["baseid"].ToString());
            }
            //部门id 
            if (!queryParam["departid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and departid = '{0}'", queryParam["departid"].ToString());
            }
            //部门code 
            if (!queryParam["departcode"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and exists(select 1 from base_department d where d.encode like '%{0}%' and d.departmentid =departid)", queryParam["departcode"].ToString());
            }
            //申请类型 
            if (!queryParam["applytype"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and applytype = '{0}'", queryParam["applytype"].ToString());
            }
            //有效数据
            if (!queryParam["isavailable"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and baseid is null");
            }
            //流程状态
            if (!queryParam["flowstate"].IsEmpty())
            {
                var flowstate = queryParam["flowstate"].ToString();
                if (flowstate == "1")
                {
                    pagination.conditionJson += string.Format(@" and flowstate ='{0}'", "上报计划");
                }
                else if (flowstate == "2")
                {
                    pagination.conditionJson += string.Format(@" and flowstate !='{0}' and flowstate !='{1}'", "上报计划", "结束");
                }
                else if(flowstate=="3")
                {
                    pagination.conditionJson += string.Format(@" and flowstate ='{0}'", "结束");
                }                
            }
            //数据范围
            if (!queryParam["datascope"].IsEmpty())
            {
                var datascope = queryParam["datascope"].ToString();
                var ismeWhere = "";
                if (datascope == "1")
                {//我申请的数据
                    ismeWhere = string.Format(@" and createuserid ='{0}'", user.UserId);
                }
                else if (datascope == "2")
                {//我处理的数据      
                    ismeWhere = string.Format(@" and instr(checkuseraccount,'{0}')>0", user.Account);
                }
                pagination.conditionJson += ismeWhere;
            }
            var dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);

            return dt;
        }        
        /// <summary>
        /// 获取工作流节点
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public DataTable GetWorkDetailList(string objectId)
        {
            DatabaseType dataType = DbHelper.DbType;
            string sql = string.Format("select * from sys_wftbactivity where processid='{0}' order by autoid asc", objectId);
            var dt = this.BaseRepository().FindTable(sql);

            return dt;
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public PlanApplyEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 获取待审核部门工作计划
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int GetPlanApplyBMNum(ERCHTMS.Code.Operator user) {
            int count = 0;
            try
            {
                count = BaseRepository().FindObject(string.Format(@"select count(0)
from hrs_planapply
where createuserorgcode='{0}' 
and (instr(checkuseraccount,'{1}')>0 or flowstate='结束') and applytype = '部门工作计划' and baseid is null
and checkuseraccount like '%{1}%'", user.OrganizeCode, user.Account)).ToInt();
            }
            catch {
                return 0;
            }
            return count;
        }
        /// <summary>
        /// 获取待审核个人工作计划
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int GetPlanApplyGRNum(ERCHTMS.Code.Operator user)
        {
            int count = 0;
            try
            {
                count = BaseRepository().FindObject(string.Format(@"select count(0)
from hrs_planapply
where createuserorgcode='{0}' 
and (instr(checkuseraccount,'{1}')>0 or flowstate='结束') and applytype = '个人工作计划' and baseid is null
and checkuseraccount like '%{1}%'", user.OrganizeCode,user.Account)).ToInt();
            }
            catch
            {
                return 0;
            }
            return count;
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
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, PlanApplyEntity entity)
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
        #endregion
    }
}
