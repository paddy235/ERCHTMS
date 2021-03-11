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
using System.Linq.Expressions;
using ERCHTMS.Service.CommonPermission;

namespace ERCHTMS.Service.EmergencyPlatform
{
    /// <summary>
    /// 描 述：应急演练
    /// </summary>
    public class DrillplanService : RepositoryFactory<DrillplanEntity>, IDrillplanService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="condition">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<DrillplanEntity> GetListForCon(Expression<Func<DrillplanEntity, bool>> condition)
        {
            return this.BaseRepository().IQueryable(condition).ToList();
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
                if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
                {
                    string condition = queryParam["condition"].ToString();
                    string keyord = queryParam["keyword"].ToString();
                    switch (condition)
                    {
                        case "DrillTypeName":            //账户
                            pagination.conditionJson += string.Format(" and DrillTypeName  like '%{0}%'", keyord);
                            break;
                        case "Name":          //姓名
                            pagination.conditionJson += string.Format(" and Name  like '%{0}%'", keyord);
                            break;
                        case "DrillModeName":          //手机
                            pagination.conditionJson += string.Format(" and DrillModeName like '%{0}%'", keyord);
                            break;
                        default:
                            break;
                    }
                }
                if (!queryParam["DrillType"].IsEmpty())
                {
                    string DrillType = queryParam["DrillType"].ToString();
                    pagination.conditionJson += string.Format(" and DrillType = '{0}'", DrillType);
                }
                if (!queryParam["DrillMode"].IsEmpty())
                {
                    string DrillMode = queryParam["DrillMode"].ToString();
                    pagination.conditionJson += string.Format(" and DrillMode = '{0}'", DrillMode);
                }
                if (!queryParam["happentimestart"].IsEmpty())
                {
                    string happentimestart = queryParam["happentimestart"].ToString();
                    pagination.conditionJson += string.Format(" and plantime >= (select  to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') from dual)", happentimestart);
                }
                if (!queryParam["happentimeend"].IsEmpty())
                {
                    string happentimeend = queryParam["happentimeend"].ToString();
                    pagination.conditionJson += string.Format(" and plantime <= (select  to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') from dual)", happentimeend);
                }
                if (!queryParam["Name"].IsEmpty())
                {
                    string Name = queryParam["Name"].ToString();
                    pagination.conditionJson += string.Format(" and Name  like '%{0}%'", Name);
                }

                #region 权限判断
                if (!queryParam["code"].IsEmpty() && !queryParam["isOrg"].IsEmpty())
                {
                    if (!queryParam["code"].IsEmpty() && !queryParam["isOrg"].IsEmpty())
                    {
                        var isOrg = queryParam["isOrg"].ToString();
                        var deptCode = queryParam["code"].ToString();
                        if (isOrg == "Organize")
                        {
                            pagination.conditionJson += string.Format(" and CREATEUSERORGCODE  like '{0}%'", deptCode);
                        }

                        else
                        {
                            pagination.conditionJson += string.Format(" and orgdeptcode like '{0}%'", deptCode);
                        }

                        //pagination = PermissionByCurrent.GetPermissionByCurrent2(pagination, queryParam["code"].ToString(), queryParam["isOrg"].ToString());
                    }
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
        public IEnumerable<DrillplanEntity> GetList(string queryJson)
        {
            return this.BaseRepository().FindList(" select * from V_mae_Drillplan where 1=1 " + queryJson).ToList();

        }

        public IEnumerable<DrillplanEntity> GetList(int year, string deptId, int monthStart, int monthEnd)
        {
            return this.BaseRepository().IQueryable().Where(e => e.CREATEDATE.Value.Year == year && e.DEPARTID == deptId && e.CREATEDATE.Value.Month >= monthStart && e.CREATEDATE.Value.Month <= monthEnd).ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public DrillplanEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, DrillplanEntity entity)
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
