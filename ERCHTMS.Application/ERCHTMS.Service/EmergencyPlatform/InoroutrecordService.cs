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
namespace ERCHTMS.Service.EmergencyPlatform
{
    /// <summary>
    /// 描 述：出入库记录
    /// </summary>
    public class InoroutrecordService : RepositoryFactory<InoroutrecordEntity>, IInoroutrecordService
    {
        #region 获取数据

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="condition">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<InoroutrecordEntity> GetListForCon(Expression<Func<InoroutrecordEntity, bool>> condition)
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
                        //case "PostName":            //账户
                        //    pagination.conditionJson += string.Format(" and PostName  like '%{0}%'", keyord);
                        //    break;
                        //case "RealName":          //姓名
                        //    pagination.conditionJson += string.Format(" and REALNAME  like '%{0}%'", keyord);
                        //    break;
                        //case "Mobile":          //手机
                        //    pagination.conditionJson += string.Format(" and MOBILE like '%{0}%'", keyord);
                        //    break;
                        default:
                            break;
                    }
                }
                //公司主键
                //if (!queryParam["organizeId"].IsEmpty())
                //{
                //    string organizeId = queryParam["organizeId"].ToString();
                //    pagination.conditionJson += string.Format(" and ORGANIZEID = '{0}'", organizeId);
                //}
                ////部门主键
                //if (!queryParam["departmentId"].IsEmpty())
                //{
                //    string departmentId = queryParam["departmentId"].ToString();
                //    pagination.conditionJson += string.Format(" and DEPARTMENTID = '{0}'", departmentId);
                //}

                //if (!queryParam["code"].IsEmpty() && !queryParam["isOrg"].IsEmpty())
                //{
                //    Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                //    string deptCode = queryParam["code"].ToString();
                //    if (queryParam["isOrg"].ToString() == "Organize")
                //    {
                //        pagination.conditionJson += string.Format(" and organizecode  like '{0}%'", deptCode);
                //        if (user.IsSystem || user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户"))
                //        {
                //            pagination.conditionJson += string.Format(" and (departmentcode like '{0}%'", deptCode);
                //        }
                //        else
                //        {
                //            pagination.conditionJson += string.Format(" and (departmentcode like '{0}%'", user.DeptCode);
                //        }
                //        pagination.conditionJson += string.Format(" or departmentid in(select departmentid from BASE_DEPARTMENT t where senddeptid=(select departmentid from base_department d where d.encode='{0}')))", user.DeptCode);
                //    }

                //    else if (queryParam["isOrg"].ToString() == "Department" || user.IsSystem)
                //    {
                //        pagination.conditionJson += string.Format(" and departmentcode like '{0}%'", deptCode);
                //    }
                //    else
                //    {
                //        pagination.conditionJson += string.Format(" and (departmentcode like '{0}%'", deptCode);
                //        pagination.conditionJson += string.Format(" and departmentid in(select departmentid from BASE_DEPARTMENT t where t.senddeptid=(select departmentid from base_department d where d.encode='{0}')))", user.DeptCode);
                //    }
                //}
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<InoroutrecordEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public InoroutrecordEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, InoroutrecordEntity entity)
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
