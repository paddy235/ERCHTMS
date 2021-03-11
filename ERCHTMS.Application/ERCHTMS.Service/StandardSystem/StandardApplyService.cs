using ERCHTMS.Entity.StandardSystem;
using ERCHTMS.IService.StandardSystem;
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

namespace ERCHTMS.Service.StandardSystem
{
    /// <summary>
    /// 描 述：标准修编申请
    /// </summary>
    public class StandardApplyService : RepositoryFactory<StandardApplyEntity>, StandardApplyIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<StandardApplyEntity> GetList(string queryJson)
        {
            var sql = string.Format("select * from HRS_STANDARDAPPLY where 1=1 {0}", queryJson);
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
                pagination.p_fields = @"createdate,createuserid,createuserdeptcode,createuserorgcode,modifydate,modifyuserid,filename,editperson,editdeptid,editdeptname,editdate,remark,checkdeptid,checkdeptname,checkuserid,checkusername,flowstate,case when flowstate='申请人申请' then 1 when flowstate='结束' then 3 else 2 end num";
            }
            pagination.p_kid = "id";
            pagination.p_tablename = @"hrs_standardapply";
            pagination.conditionJson = string.Format(" createuserorgcode='{0}'",user.OrganizeCode);  
            //违章描述 
            if (!queryParam["filename"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and filename like '%{0}%'", queryParam["filename"].ToString());
            }
            //流程状态
            var flowstate = "";
            if (!queryParam["flowstate"].IsEmpty())
            {
                flowstate = queryParam["flowstate"].ToString();
                pagination.conditionJson += string.Format(@" and flowstate ='{0}'", flowstate);
            }
            //数据范围
            if (!queryParam["datascope"].IsEmpty())
            {
                var datascope = queryParam["datascope"].ToString();
                var ismeWhere = "";
                if (datascope=="1")
                {//我申请的数据
                    ismeWhere = string.Format(@" and createuserid ='{0}'", user.UserId);
                }
                else if(datascope == "2")
                {//我处理的数据      
                    ismeWhere = string.Format(@" and instr(checkuserid,'{0}')>0", user.UserId);
                }
                pagination.conditionJson += ismeWhere;
            }
            //首页待办事项提醒
            if (!queryParam["indextype"].IsEmpty())
            {
                var indextype = queryParam["indextype"].ToString();                
                if (indextype == "1")
                {//重新申请
                    pagination.conditionJson += string.Format(@" and createuserid ='{0}' and flowstate='申请人申请' and exists(select 1 from hrs_standardcheck where recid=hrs_standardapply.id)", user.UserId);
                }
                else if (indextype == "2")
                {//待审核（批）      
                    pagination.conditionJson += " and (";
                    pagination.conditionJson += string.Format(@" (instr(checkuserid,'{0}')>0 and flowstate in('1级审核','2级审核','审批'))", user.UserId);
                    pagination.conditionJson += string.Format(@" or (flowstate ='部门会签' and instr(checkuserid,'{0}')>0 and instr(checkusername,'{1}(已签)')<1)", user.UserId, user.UserName);
                    pagination.conditionJson += string.Format(@" or (flowstate ='分委会审核' and instr(checkuserid,'{0}')>0 and instr(checkusername,'{1}(已审)')<1)", user.UserId, user.UserName);
                    pagination.conditionJson += ")";
                }
                else if (indextype == "3")
                {//待分配      
                    pagination.conditionJson += string.Format(@" and instr(checkuserid,'{0}')>0 and flowstate in('审核分配会签','分配分委会')", user.UserId);
                }                
            }
            var dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);

            return dt;
        }
        /// <summary>
        /// 统计首页待办事项数量
        /// </summary>
        /// <param name="indextype">1：重新申请，2：待审核（批），3：待分配</param>
        /// <returns></returns>
        public int CountIndex(string indextype)
        {
            int r = 0;

            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string sql = string.Format("select count(1) from HRS_STANDARDAPPLY where createuserorgcode='{0}'", user.OrganizeCode);
            switch (indextype)
            {
                case "1":
                    sql += string.Format(@" and createuserid ='{0}' and flowstate='申请人申请' and exists(select 1 from hrs_standardcheck where recid=hrs_standardapply.id)", user.UserId); 
                    break;
                case "2":
                    sql += " and (";
                    sql += string.Format(@" (instr(checkuserid,'{0}')>0 and flowstate in('1级审核','2级审核','审批'))", user.UserId);
                    sql += string.Format(@" or (flowstate ='部门会签' and instr(checkuserid,'{0}')>0 and instr(checkusername,'{1}(已签)')<1)", user.UserId,user.UserName);
                    sql += string.Format(@" or (flowstate ='分委会审核' and instr(checkuserid,'{0}')>0 and instr(checkusername,'{1}(已审)')<1)",user.UserId, user.UserName);
                    sql += ")";
                    break;
                case "3":
                    sql += string.Format(@" and instr(checkuserid,'{0}')>0 and flowstate in('审核分配会签','分配分委会')", user.UserId);
                    break;
            }
            object obj = this.BaseRepository().FindObject(sql);
            int.TryParse(obj.ToString(), out r);

            return r;
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
        public StandardApplyEntity GetEntity(string keyValue)
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
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, StandardApplyEntity entity)
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
