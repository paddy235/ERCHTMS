using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using BSFramework.Util.Extension;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Linq;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.IService.AuthorizeManage;
using ERCHTMS.Service.AuthorizeManage;
using System;
using ERCHTMS.Code;
using ERCHTMS.Entity.EmergencyPlatform;
using ERCHTMS.IService.EmergencyPlatform;
using ERCHTMS.Service.CommonPermission;

namespace ERCHTMS.Service.EmergencyPlatform
{
    /// <summary>
    /// 描 述：应急任务
    /// </summary>
    public class TeamService : RepositoryFactory<TeamEntity>, ITeamService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<TeamEntity> GetList(string queryJson)
        {
            return this.BaseRepository().FindList(" select * from v_mae_team where 1=1 " + queryJson).ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public TeamEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;

            if (queryJson.Length > 0)
            {
                var queryParam = queryJson.ToJObject();
                //查询条件
                //姓名
                if (!queryParam["RealName"].IsEmpty())
                {
                    string RealName = queryParam["RealName"].ToString();
                    pagination.conditionJson += string.Format(" and UserFullName like '%{0}%'", RealName);
                }
                if (!queryParam["PostId"].IsEmpty())
                {
                    string PostId = queryParam["PostId"].ToString();
                    pagination.conditionJson += string.Format(" and PostId = '{0}'", PostId);
                }

                //公司主键
                if (!queryParam["organizeId"].IsEmpty())
                {
                    string organizeId = queryParam["organizeId"].ToString();
                    pagination.conditionJson += string.Format(" and ORGANIZEID = '{0}'", organizeId);
                }
                //部门主键
                if (!queryParam["departmentId"].IsEmpty())
                {
                    string departmentId = queryParam["departmentId"].ToString();
                    pagination.conditionJson += string.Format(" and departmentid = '{0}'", departmentId);
                }
                if (!queryParam["OrgCode"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and OrgName = '{0}'", queryParam["OrgCode"].ToString());
                }
                #region 权限判断
                if (!queryParam["code"].IsEmpty() && !queryParam["isOrg"].IsEmpty())
                {
                    pagination = PermissionByCurrent.GetPermissionByCurrent2(pagination, queryParam["code"].ToString(), queryParam["isOrg"].ToString());
                }
                #endregion
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
        public void SaveForm(string keyValue, TeamEntity entity)
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
