using ERCHTMS.Entity.RoutineSafetyWork;
using ERCHTMS.IService.RoutineSafetyWork;
using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util.Extension;
using BSFramework.Util;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Data;
using BSFramework.Util.WebControl;
using ERCHTMS.Code;

namespace ERCHTMS.Service.RoutineSafetyWork
{
    /// <summary>
    /// 描 述：用户组管理
    /// </summary>
    public class UserGroupManageService : RepositoryFactory<UserGroupManageEntity>, UserGroupManageIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageList(Pagination pagination)
        {
            DatabaseType dataType = DbHelper.DbType;
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<UserGroupManageEntity> GetList()
        {
            return this.BaseRepository().IQueryable().OrderByDescending(t => t.CreateDate).ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public UserGroupManageEntity GetEntity(string keyValue)
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

            var queryParam = queryJson.ToJObject();
            //查询条件
            if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            {
                string condition = queryParam["condition"].ToString();
                string keyord = queryParam["keyword"].ToString();
                switch (condition)
                {
                    case "Account":            //账户
                        pagination.conditionJson += string.Format(" and ACCOUNT  like '%{0}%'", keyord);
                        break;
                    case "RealName":          //姓名
                        pagination.conditionJson += string.Format(" and REALNAME  like '%{0}%'", keyord);
                        break;
                    case "Mobile":          //手机
                        pagination.conditionJson += string.Format(" and MOBILE like '%{0}%'", keyord);
                        break;
                    default:
                        break;
                }
            }
            if (!queryParam["state"].IsEmpty() && queryParam["state"].ToString() == "1")
            {
                //用户组id
                string userListId = queryParam["code"].ToString();
                pagination.conditionJson += string.Format(" and userid in(select e.userid from BIS_UserListManage e where e.moduleid='{0}')", userListId);
            }
            else {
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
                    pagination.conditionJson += string.Format(" and DEPARTMENTID = '{0}'", departmentId);
                }
               
                if (!queryParam["code"].IsEmpty() && !queryParam["isOrg"].IsEmpty())
                {
                    Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                    string deptCode = queryParam["code"].ToString();
                    if (queryParam["isOrg"].ToString() == "Organize")
                    {
                        pagination.conditionJson += string.Format(" and organizecode  like '{0}%'", deptCode);
                        pagination.conditionJson += string.Format(" and (departmentcode like '{0}%'", deptCode);
                        //if (user.IsSystem || user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户"))
                        //{
                        //    pagination.conditionJson += string.Format(" and (departmentcode like '{0}%'", deptCode);
                        //}
                        //else
                        //{
                        //    pagination.conditionJson += string.Format(" and (departmentcode like '{0}%'", user.DeptCode);
                        //}
                        pagination.conditionJson += string.Format(" or departmentid in(select departmentid from BASE_DEPARTMENT t where senddeptid=(select departmentid from base_department d where d.encode='{0}')))", user.DeptCode);
                    }

                    else if (queryParam["isOrg"].ToString() == "Department" || user.IsSystem)
                    {
                        pagination.conditionJson += string.Format(" and departmentcode like '{0}%'", deptCode);
                    }
                    else
                    {
                        pagination.conditionJson += string.Format(" and (departmentcode like '{0}%'", deptCode);
                        pagination.conditionJson += string.Format(" and departmentid in(select departmentid from BASE_DEPARTMENT t where t.senddeptid=(select departmentid from base_department d where d.encode='{0}')))", user.DeptCode);
                    }
                }
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
            Repository<UserListManageEntity> rep = new Repository<UserListManageEntity>(DbFactory.Base());
            this.BaseRepository().Delete(keyValue);
            rep.ExecuteBySql(string.Format("delete bis_userlistmanage where moduleid='{0}' ", keyValue));
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, UserGroupManageEntity entity)
        {
            Repository<UserListManageEntity> rep = new Repository<UserListManageEntity>(DbFactory.Base());
            
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
                rep.ExecuteBySql(string.Format("delete bis_userlistmanage where moduleid='{0}' ", keyValue));
                var arrId = entity.UserId.Split(',');
                List<UserListManageEntity> list = new List<UserListManageEntity>();

                foreach (string userId in arrId)
                {
                    UserListManageEntity newEntity = new UserListManageEntity();
                    newEntity.Create();
                    newEntity.UserId = userId;
                    newEntity.ModuleId = entity.Id;
                    newEntity.ModuleType = "2";
                    list.Add(newEntity);
                    //rep.Insert(newEntity);
                }
                rep.Insert(list);
            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
                var arrId = entity.UserId.Split(',');
                List<UserListManageEntity> list = new List<UserListManageEntity>();
                
                foreach (string userId in arrId)
                {
                    UserListManageEntity newEntity = new UserListManageEntity();
                    newEntity.Create();
                    newEntity.UserId = userId;
                    newEntity.ModuleId = entity.Id;
                    newEntity.ModuleType = "2";
                    list.Add(newEntity);
                    //rep.Insert(newEntity);
                }
                rep.Insert(list);
            }
        }
        #endregion
    }
}
