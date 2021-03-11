using ERCHTMS.Entity.BaseManage;
using ERCHTMS.IService.AuthorizeManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.Service.AuthorizeManage;
using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Data;
using ERCHTMS.Code;
using System;

namespace ERCHTMS.Service.BaseManage
{
    /// <summary>
    /// 描 述：岗位管理
    /// </summary>
    public class PostService : RepositoryFactory<RoleEntity>, IPostService
    {
        private IAuthorizeService<RoleEntity> iauthorizeservice = new AuthorizeService<RoleEntity>();

        #region 获取数据
        /// <summary>
        /// 验证岗位名称是否重复
        /// </summary>
        /// <param name="postName">部门名称</param>
        /// <returns></returns>
        public bool ExistPostJugement(string postName)
        {
            IEnumerable<RoleEntity> count = this.BaseRepository().FindList(string.Format("select ROLEID  from base_role t where t.fullname='{0}'", postName));
            if (count.Count() > 0)
                return false;
            else return true;
        }
        /// <summary>
        /// 岗位列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RoleEntity> GetList()
        {
            var expression = LinqExtensions.True<RoleEntity>();
            expression = expression.And(t => t.Category == 2).And(t => t.EnabledMark == 1).And(t => t.DeleteMark == 0);
            return this.BaseRepository().IQueryable(expression).OrderByDescending(t => t.CreateDate).ToList();
        }
        /// <summary>
        /// 岗位列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<RoleEntity> GetPageList(Pagination pagination, string queryJson)
        {
            var expression = LinqExtensions.True<RoleEntity>();
            var queryParam = queryJson.ToJObject();
            //查询条件
            if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            {
                string condition = queryParam["condition"].ToString();
                string keyword = queryParam["keyword"].ToString();
                switch (condition)
                {
                    case "EnCode":            //岗位编号
                        expression = expression.And(t => t.EnCode.Contains(keyword));
                        break;
                    case "FullName":          //岗位名称
                        expression = expression.And(t => t.FullName.Contains(keyword));
                        break;
                    default:
                        break;
                }
            }
            expression = expression.And(t => t.Category == 2);
            return this.BaseRepository().FindList(expression, pagination);
        }
        
        public DataTable GetList(Pagination pagination, string queryJson)
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
                    case "EnCode":
                        pagination.conditionJson += string.Format(" and t.EnCode  like '%{0}%'", keyord);
                        break;
                    case "FullName":
                        pagination.conditionJson += string.Format(" and t.FullName  like '%{0}%'", keyord);
                        break;
                    default:
                        break;
                }
            }
            if (!queryParam["deptid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and (t.deptid='{0}' or IsPublic=1)", queryParam["deptid"].ToString());

                //var deptEntity = new DepartmentService().GetEntity(queryParam["deptid"].ToString());
                //if (deptEntity != null) {
                //    pagination.conditionJson += string.Format(" and t.deptid  in (select t.departmentid from base_department t where t.encode like '{0}%')", deptEntity.EnCode);
                //}
            }
            if (!queryParam["deptcode"].IsEmpty())
            {
                var deptentity = new DepartmentService().GetEntityByCode(queryParam["deptcode"].ToString());
                if (deptentity!=null)
                {
                    if (deptentity.Nature == "厂级" || deptentity.Nature == "省级" || deptentity.Nature == "集团")
                    {
                        pagination.conditionJson += string.Format(" and t.organizeid ='{0}'", deptentity.OrganizeId);
                    }
                    else
                    {
                        pagination.conditionJson += string.Format(" and t.deptid  in (select t.departmentid from base_department t where t.encode like '{0}%') and t.organizeid ='{1}'", queryParam["deptcode"].ToString(), deptentity.OrganizeId);
                    }
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 岗位列表(ALL)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RoleEntity> GetAllList()
        {
            var strSql = new StringBuilder();
            strSql.Append(@"SELECT  r.RoleId ,
				                    o.FullName AS OrganizeId ,
				                    r.Category ,
				                    r.EnCode ,
				                    r.FullName ,
				                    r.SortCode ,
				                    r.EnabledMark ,
				                    r.Description ,
				                    r.CreateDate
                    FROM    Base_Role r
				                    LEFT JOIN Base_Organize o ON o.OrganizeId = r.OrganizeId
                    WHERE   o.FullName is not null and r.Category = 2 and r.EnabledMark =1
                    ORDER BY o.FullName, r.SortCode");
            return this.BaseRepository().FindList(strSql.ToString());
        }
        /// <summary>
        /// 岗位实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public RoleEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 手机端获取岗位
        /// </summary>
        /// <returns></returns>
        public DataTable GetPostForAPP()
        {
            Operator user = OperatorProvider.Provider.Current();
            var strSql = new StringBuilder();
            strSql.Append(@"select a.roleid as postid,a.fullname as postname,b.departmentid as deptid,b.fullname as deptname 
                            from base_role a left join base_department b on a.deptid =b.departmentid where a.category ='2' and a.organizeid ='" + user.OrganizeId + "'");
            strSql.Append(" order by b.fullname,a.fullname");

            return this.BaseRepository().FindTable(strSql.ToString());
        }
        #endregion

        #region 验证数据
        /// <summary>
        /// 岗位编号不能重复
        /// </summary>
        /// <param name="enCode">编号</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool ExistEnCode(string enCode, string keyValue)
        {
            var expression = LinqExtensions.True<RoleEntity>();
            expression = expression.And(t => t.EnCode == enCode).And(t => t.Category == 2);
            if (!string.IsNullOrEmpty(keyValue))
            {
                expression = expression.And(t => t.RoleId != keyValue);
            }
            return this.BaseRepository().IQueryable(expression).Count() == 0 ? true : false;
        }
        /// <summary>
        /// 岗位名称不能重复
        /// </summary>
        /// <param name="fullName">名称</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool ExistFullName(string fullName, string keyValue)
        {
            var expression = LinqExtensions.True<RoleEntity>();
            expression = expression.And(t => t.FullName == fullName).And(t => t.Category == 2);
            if (!string.IsNullOrEmpty(keyValue))
            {
                expression = expression.And(t => t.RoleId != keyValue);
            }
            return this.BaseRepository().IQueryable(expression).Count() == 0 ? true : false;
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除岗位
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存岗位表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="postEntity">岗位实体</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, RoleEntity postEntity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                postEntity.Modify(keyValue);
                this.BaseRepository().Update(postEntity);
            }
            else
            {
                string[] arrId =new string[]{};
                string[] arrName = new string[] { };
                if (!string.IsNullOrWhiteSpace(postEntity.DeptId))
                {
                    arrId = postEntity.DeptId.Split(',');
                    arrName = postEntity.DeptName.Split(',');
                }
              
                if (arrId.Length <2)
                {
                    postEntity.Create();
                    postEntity.Category = 2;
                    //postEntity.EnCode = GetPostEnCode(postEntity);
                    this.BaseRepository().Insert(postEntity);
                }
                else
                {
                    int j = 0;
                    List<RoleEntity> list = new List<RoleEntity>();
                    foreach (string deptId in arrId)
                    {
                        list.Add(new RoleEntity { 
                          CreateDate=DateTime.Now,
                          RoleId=System.Guid.NewGuid().ToString(),
                          FullName = postEntity.FullName,
                          DeptId=deptId,
                          EnCode=postEntity.EnCode+"0"+j,
                          DeptName = arrName[j],
                          Category=2,
                          Nature = postEntity.Nature,
                          RoleIds = postEntity.RoleIds,
                          RoleNames = postEntity.RoleNames,
                          DeleteMark=0,
                          EnabledMark=1,
                          OrganizeId=postEntity.OrganizeId
                        });
                        j++;
                    }
                    this.BaseRepository().Insert(list);
                }
               
            }
        }
        /// <summary>
        /// 获取编号
        /// </summary>
        /// <param name="districtEntity"></param>
        /// <returns></returns>
        public string GetPostEnCode(RoleEntity districtEntity)
        {
            return "";
        }
        #endregion
    }
}
