using ERCHTMS.Entity.BaseManage;
using ERCHTMS.IService.BaseManage;
using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util.Extension;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Data;

namespace ERCHTMS.Service.BaseManage
{
    /// <summary>
    /// 描 述：部门管理
    /// </summary>
    public class DepartmentService : RepositoryFactory<DepartmentEntity>, IDepartmentService
    {
        #region 获取数据
        /// <summary>
        /// 验证部门名称是否重复
        /// </summary>
        /// <param name="departmentName">部门名称</param>
        /// <returns></returns>
        public bool ExistDeptJugement(string departmentName)
        {
            IEnumerable<DepartmentEntity> count = this.BaseRepository().FindList(string.Format("select departmentid  from BASE_DEPARTMENT t where t.fullname='{0}'", departmentName));
            if (count.Count() > 0)
                return false;
            else return true;
        }

        /// <summary>
        /// 部门列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DepartmentEntity> GetList()
        {
            return this.BaseRepository().IQueryable().OrderByDescending(t => t.CreateDate).ToList();
        }
        /// <summary>
        /// 部门实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public DepartmentEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }


        public DepartmentEntity GetEntityByCode(string keyValue)
        {
            return this.BaseRepository().IQueryable().Where(p => p.EnCode == keyValue).FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<DepartmentEntity> GetDepts(string parentid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT * FROM base_department u CONNECT BY PRIOR u.departmentid=u.parentid  START WITH u.departmentid='{0}'", parentid);
            return new RepositoryFactory().BaseRepository().FindList<DepartmentEntity>(strSql.ToString());
        }
        #endregion

        #region 验证数据
        /// <summary>
        /// 部门编号不能重复
        /// </summary>
        /// <param name="enCode">编号</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool ExistEnCode(string enCode, string keyValue)
        {
            var expression = LinqExtensions.True<DepartmentEntity>();
            expression = expression.And(t => t.EnCode == enCode);
            if (!string.IsNullOrEmpty(keyValue))
            {
                expression = expression.And(t => t.DepartmentId != keyValue);
            }
            return this.BaseRepository().IQueryable(expression).Count() == 0 ? true : false;
        }
        /// <summary>
        /// 部门名称不能重复
        /// </summary>
        /// <param name="fullName">名称</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool ExistFullName(string fullName, string keyValue)
        {
            var expression = LinqExtensions.True<DepartmentEntity>();
            expression = expression.And(t => t.FullName == fullName);
            if (!string.IsNullOrEmpty(keyValue))
            {
                expression = expression.And(t => t.DepartmentId != keyValue);
            }
            return this.BaseRepository().IQueryable(expression).Count() == 0 ? true : false;
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="ue">用户实体</param>
        public void RemoveForm(string keyValue, List<UserEntity> ue)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            try
            {
                db.Delete<DepartmentEntity>(keyValue);
                db.Delete(ue);
                db.Commit();
            }
            catch (Exception)
            {
                db.Rollback();
                throw;
            }
        }
        /// <summary>
        /// 保存部门表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="departmentEntity">机构实体</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, DepartmentEntity departmentEntity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                DepartmentEntity curEntity = this.BaseRepository().FindEntity(keyValue);  //获取父对象
                if (curEntity.OrganizeId != departmentEntity.OrganizeId || string.IsNullOrEmpty(curEntity.EnCode))
                {
                    departmentEntity.EnCode = GetDepartmentCode(departmentEntity);
                }
                departmentEntity.Modify(keyValue);
                this.BaseRepository().Update(departmentEntity);
                if (curEntity.Nature == "承包商")
                {
                    var expression = LinqExtensions.True<DepartmentEntity>();
                    expression = expression.And(t => t.ParentId == departmentEntity.DepartmentId);
                    var allc = this.BaseRepository().IQueryable(expression).ToList();
                    foreach (var item in allc)
                    {
                        //将此承包商下级的分包商的发包单位改为承包商的发包单位
                        item.SendDeptID = departmentEntity.SendDeptID;
                        item.SendDeptName = departmentEntity.SendDeptName;
                        this.BaseRepository().Update(item);
                    }
                }
            }
            else
            {
                departmentEntity.EnCode = GetDepartmentCode(departmentEntity);
                departmentEntity.Create();
                if (departmentEntity.Nature == "分包商")
                {
                    //将此分包商的发包单位改为此上级承包商的发包单位
                    DepartmentEntity curEntity = this.BaseRepository().FindEntity(departmentEntity.ParentId);
                    departmentEntity.SendDeptID = curEntity.SendDeptID;
                    departmentEntity.SendDeptName = curEntity.SendDeptName;
                }
                this.BaseRepository().Insert(departmentEntity);
            }
        }
        #endregion


        #region  根据当前机构获取对应的机构代码  机构代码 2-6-8-10  位
        /// <summary>
        /// 根据当前机构获取对应的机构代码  机构代码 2-6-8-10  位
        /// </summary>
        /// <param name="departmentEntity"></param>
        /// <returns></returns>
        public string GetDepartmentCode(DepartmentEntity departmentEntity)
        {
            string maxCode = string.Empty;

            OrganizeEntity oEntity = null;

            DepartmentEntity dEntity = null;

            string deptcode = string.Empty;

            if (departmentEntity.ParentId == "0")//选择是机构
            {
                oEntity = new OrganizeService().BaseRepository().FindEntity(departmentEntity.OrganizeId);  //获取父对象(机构)

                deptcode = oEntity.EnCode;
            }
            else //选择的是部门
            {
                dEntity = this.BaseRepository().FindEntity(departmentEntity.ParentId);//获取部门父对象

                deptcode = dEntity.EnCode;
            }

            //查询是否存在平级部门
            IEnumerable<DepartmentEntity> maxObj = this.BaseRepository().FindList(string.Format("select t.*  from BASE_DEPARTMENT t where t.encode  like '{0}%' and t.encode !='{0}' and t.parentid='" + departmentEntity.ParentId + "' and  encode is not null order by encode desc", deptcode));

            //确定是否存在上级部门,非部门根节点
            if (departmentEntity.ParentId != "0")
            {
                //存在，则取编码最大的那一个
                if (maxObj.Count() > 0)
                {
                    maxCode = maxObj.FirstOrDefault().EnCode;  //获取最大的Code 
                    if (!string.IsNullOrEmpty(maxCode))
                    {
                        maxCode = new OrganizeService().QueryOrganizeCodeByCondition(maxCode);
                    }
                }
                else
                {
                    DepartmentEntity parentEntity = this.BaseRepository().FindEntity(departmentEntity.ParentId);  //获取父对象

                    maxCode = parentEntity.EnCode + "001";  //固定值,非可变
                }
            }
            else  //部门根节点的操作
            {
                //do somethings 
                if (maxObj.Count() > 0)
                {
                    maxCode = maxObj.FirstOrDefault().EnCode;  //获取最大的Code 

                    if (!string.IsNullOrEmpty(maxCode))
                    {
                        maxCode = new OrganizeService().QueryOrganizeCodeByCondition(maxCode);
                    }
                    else
                    {
                        maxCode = deptcode + "001";
                    }
                }
                else
                {
                    maxCode = deptcode + "001";  //固定值,非可变,加机构编码，组合新编码
                }
            }
            //先判断当前编码是否存在于数据库表中
            return maxCode;
        }

        /// <summary>
        /// 批量导入部门的时候部门code
        /// </summary>
        /// <param name="parentName">上级部门名称</param>
        /// <param name="orgId">当前用户所属机构id</param>
        /// <returns></returns>
        public List<string> GetImportDeptCode(string parentName,string orgId)
        {
            List<string> list = new List<string>();
            DataTable orgData = this.BaseRepository().FindTable("select *from base_organize where fullname='" + parentName + "'");
            string deptCode = string.Empty;
            string maxCode = string.Empty;
            if (orgData.Rows.Count > 0)
            {
                //相当于现在机构导入
                list.Add("0");//parentid
            }
            else
            {
                DataTable deptData = this.BaseRepository().FindTable("select *from base_department where fullname='" + parentName + "'");
                if (deptData.Rows.Count > 0)
                {
                    //相当于选择部门导入
                    list.Add(deptData.Rows[0]["departmentid"].ToString());//parentid
                }
                else {
                    //既不是选择的机构也不是选择的部门，那么导入失败
                    list.Add("-1");
                }
                
            }
            return list;
        }

        #endregion

        #region 根据当前部门判断是否存在上级部门
        /// <summary>
        /// 根据当前部门判断是否存在上级部门
        /// </summary>
        /// <param name="departID"></param>
        /// <returns></returns>
        public bool IsExistSuperior(string departID)
        {
            bool isSuccess = false;

            string sql = string.Format(@"select * from base_department where departmentid  = 
                          (select parentid from base_department where departmentid ='{0}')", departID);

            var list = this.BaseRepository().FindList(sql);

            if (list.Count() > 0)
            {
                isSuccess = true;
            }

            return isSuccess;
        }
        #endregion

    }
}
