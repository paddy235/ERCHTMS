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
    /// 描 述：应急预案
    /// </summary>
    public class ReserverplanService : RepositoryFactory<ReserverplanEntity>, IReserverplanService
    {
        #region 获取数据

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            DatabaseType dataType = DbHelper.DbType;

            if (user.RoleName.Contains("承包商级用户"))
            {
                pagination.conditionJson += string.Format(" and departid_bz  in (select departmentid from base_department where encode like '{0}%')", user.DeptCode);
            }

            if (queryJson.Length > 0)
            {
                var queryParam = queryJson.ToJObject();
                //查询条件
                if (!queryParam["PlanType"].IsEmpty())
                {
                    string PlanType = queryParam["PlanType"].ToString();
                    pagination.conditionJson += string.Format(" and PlanType = '{0}'", PlanType);
                }
                if (!queryParam["Name"].IsEmpty())
                {
                    string Name = queryParam["Name"].ToString();
                    pagination.conditionJson += string.Format(" and Name  like '%{0}%'", Name);
                }
                #region 权限判断
                if (!queryParam["code"].IsEmpty() && !queryParam["isOrg"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and departid_bz  in (select departmentid from base_department where encode like '{0}%')", queryParam["code"].ToString());
                }
                #endregion
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }


        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<ReserverplanEntity> GetList(string queryJson)
        {
            return this.BaseRepository().FindList(" select * from mae_Reserverplan where 1=1 " + queryJson).ToList();
        }


        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ReserverplanEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, ReserverplanEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                ReserverplanEntity se = this.BaseRepository().FindEntity(keyValue);
                if (se == null)
                {
                    entity.ID = keyValue;
                    entity.Create();
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
