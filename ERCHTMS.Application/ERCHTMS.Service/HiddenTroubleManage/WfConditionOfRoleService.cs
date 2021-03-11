using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.IService.HiddenTroubleManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using System.Data;
using ERCHTMS.Code;
using BSFramework.Util;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：流程配置条件角色附加表
    /// </summary>
    public class WfConditionOfRoleService : RepositoryFactory<WfConditionOfRoleEntity>, WfConditionOfRoleIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<WfConditionOfRoleEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList().Where(p => p.INSTANCEID == queryJson).OrderBy(p => p.SERIALNUMBERS).ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public WfConditionOfRoleEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion



        #region  流程参数配置信息
        /// <summary>
        /// 流程配置实例信息
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetInstanceConditionInfoList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            if (pagination.p_fields.IsEmpty())
            {
                pagination.p_fields = @"conditioncode,conditiontype,instanceid,remarks,describes,createdate,conditionfunc,serialnumbers";
            }
            pagination.p_kid = "id";
            pagination.conditionJson = " 1=1";
            var queryParam = queryJson.ToJObject();
            //当前用户
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.p_tablename = @" bis_wfconditionofrole ";
            //实例id
            if (!queryParam["instanceid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and  instanceid ='{0}' ", queryParam["instanceid"].ToString());
            }
            var dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);

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
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, WfConditionOfRoleEntity entity)
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
