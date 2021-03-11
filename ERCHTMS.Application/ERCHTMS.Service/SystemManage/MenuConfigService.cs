using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.WebControl;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.IService.SystemManage;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Service.SystemManage
{
   public class MenuConfigService : RepositoryFactory<MenuConfigEntity>, IMenuConfigService
    {

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="deptId">电厂Id</param>
        /// <param name="paltformType">平台类型</param>
        /// <param name="roleId">授权的角色ID</param>
        /// <returns>返回列表</returns>
        public IEnumerable<MenuConfigEntity> GetList(string deptId, int? paltformType = null, List<string> roleId = null)
        {
            var deptAuthQuery = new RepositoryFactory().BaseRepository().IQueryable<DeptMenuAuthEntity>();
            if (!string.IsNullOrWhiteSpace(deptId))
                deptAuthQuery = deptAuthQuery.Where(x => x.DeptId == deptId);
            var deptAuthIds = deptAuthQuery.Select(p => p.ModuleId).ToList();
            var menuConfigQuery = this.BaseRepository().IQueryable(p=>deptAuthIds.Contains(p.ModuleId));
            if (paltformType != null)
                menuConfigQuery = menuConfigQuery.Where(p => p.PaltformType == paltformType);
            if (roleId != null && roleId.Count > 0)
                menuConfigQuery = menuConfigQuery.Where(x => roleId.Any(p => x.AuthorizeId.Contains(p)));
            return menuConfigQuery.ToList();
        }

        public List<MenuConfigEntity> GetAllList()
        {
            return this.BaseRepository().IQueryable().OrderBy(p=>p.Sort).ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public MenuConfigEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, MenuConfigEntity entity)
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
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagination">分页信息</param>
        /// <param name="queryJson">查询参数{ keyword:关键字（电厂名称，菜单名称） }</param>
        /// <returns></returns>
        public object GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                pagination.p_kid = "";
                pagination.p_fields = @"t.MENUICON,
t.ASSOCIATIONNAME,
t.ASSOCIATIONID,
t.SORT,
t.PARENTID,
t.ID,
t.CREATEUSERID,
t.CREATEUSERNAME,
t.CREATEDATE,
t.MODIFYDATE,
t.MODIFYUSERID,
t.MODIFYUSERNAME,
t.DEPTCODE,
t.DEPTID,
t.DEPTNAME,
t.PALTFORMTYPE,
t.MODULEID,
t.MODULECODE,
t.MODULENAME,
t.ISVIEW,
t.BAK1 as Remark,
t.BAK2,
t.BAK3,
t.BAK4,
t.AUTHORIZEID,
t.AUTHORIZENAME,
t.PARENTNAME";
                pagination.p_tablename = "BASE_MENUCONFIG t";
                pagination.conditionJson += " 1=1 ";
                if (!string.IsNullOrWhiteSpace(queryJson))
                {
                    JObject jsondata = queryJson.ToJObject();

                    if (!jsondata["condition"].IsEmpty() && !jsondata["keyword"].IsEmpty())
                    {
                        string condition = jsondata["condition"].ToString();
                        string keyword = jsondata["keyword"].ToString();
                        switch (condition)
                        {
                            case "DeptName":              //对象编号
                                pagination.conditionJson += " AND DeptName LIKE '%" + keyword + "%'";
                                break;
                            case "ModuleName":            //对象名称
                                pagination.conditionJson += " AND ModuleName LIKE '%" + keyword + "%'";
                                break;
                            default:

                                break;
                        }
                    }
                    int paltformtype = 0;
                    if (!jsondata["paltformType"].IsEmpty() && int.TryParse(jsondata["paltformType"].ToString(), out paltformtype))
                    {
                        pagination.conditionJson += " AND PaltformType=" + paltformtype;
                    }

                    if (!jsondata["parentId"].IsEmpty())
                    {
                        string parentId = jsondata["parentId"].ToString();
                        pagination.conditionJson += " AND ParentId='" + parentId + "'";
                    }

                }
                return this.BaseRepository().FindListByProcPager(pagination, DatabaseType.Oracle);
            }
            catch (Exception)
            {
                            return  new List<MenuConfigEntity>();
            }
      
        }
        /// <summary>
        /// 分页、取数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetPageDataTable(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            if (!string.IsNullOrWhiteSpace(queryJson))
            {
                JObject jsondata = queryJson.ToJObject();
                if (!jsondata["DeptId"].IsEmpty())//作业类型
                {
                    pagination.conditionJson += string.Format(" and u.DeptId='{0}'", jsondata["DeptId"].ToString());
                }

                if (!jsondata["PaltformType"].IsEmpty())//作业类型
                {
                    pagination.conditionJson += string.Format(" and u.PaltformType='{0}'", jsondata["PaltformType"].ToString());
                }
            }
            return BaseRepository().FindTable("select MODULEID as id ,MODULENAME as name,MODULECODE as key from base_menuconfig where "+pagination.conditionJson, pagination);
        }

        public List<MenuConfigEntity> GetListByModuleIds(List<string> moduleIds)
        {
            var query = this.BaseRepository().IQueryable();
            query = query.Where(p => moduleIds.Contains( p.ModuleId));
            return query.OrderBy(x => x.Sort).ToList();
        }

        public List<MenuConfigEntity> GetListByModuleIds(List<string> moduleIds, int? platform,string keyword=null)
        {
            var query = this.BaseRepository().IQueryable();
            query = query.Where(p => moduleIds.Contains(p.ModuleId));
            if (platform!=null)
            {
                query = query.Where(p => p.PaltformType==platform);
            }
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(p => p.ModuleName.Contains(keyword));
            }
            return query.OrderBy(x => x.Sort).ToList();
        }


        #endregion
    }
}
