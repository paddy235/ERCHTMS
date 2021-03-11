using BSFramework.Util.WebControl;
using ERCHTMS.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Service.CommonPermission
{
    /// <summary>
    /// 
    /// </summary>
    public class PermissionByCurrent
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="deptCode"></param>
        /// <param name="isOrg"></param>
        /// <returns></returns>
        public static Pagination GetPermissionByCurrent(Pagination pagination, string deptCode, string isOrg)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            if (isOrg == "Organize")
            {
                pagination.conditionJson += string.Format(" and CREATEUSERORGCODE  like '{0}%'", deptCode);
                if (user.IsSystem || user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户"))
                {
                    pagination.conditionJson += string.Format(" and (CREATEUSERDEPTCODE like '{0}%'", deptCode);
                }
                else
                {
                    pagination.conditionJson += string.Format(" and (CREATEUSERDEPTCODE like '{0}%'", user.DeptCode);
                }
                pagination.conditionJson += string.Format(" or CREATEUSERDEPTID in(select departmentid from BASE_DEPARTMENT t where senddeptid=(select departmentid from base_department d where d.encode='{0}')))", user.DeptCode);
            }

            else if (isOrg == "Department" || user.IsSystem)
            {
                pagination.conditionJson += string.Format(" and CREATEUSERDEPTCODE like '{0}%'", deptCode);
            }
            else
            {
                pagination.conditionJson += string.Format(" and (CREATEUSERDEPTCODE like '{0}%'", deptCode);
                pagination.conditionJson += string.Format(" and CREATEUSERDEPTID in(select departmentid from BASE_DEPARTMENT t where t.senddeptid=(select departmentid from base_department d where d.encode='{0}')))", user.DeptCode);
            }
            return pagination;
        }

        /// <summary>
        /// 首次登陆时判断
        /// </summary>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public static Pagination GetPermissionByCurrent2(Pagination pagination, string deptCode, string isOrg)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (isOrg == "Organize")
            {
                pagination.conditionJson += string.Format(" and CREATEUSERORGCODE  like '{0}%'", deptCode);
            }

            else
            {
                pagination.conditionJson += string.Format(" and CREATEUSERDEPTCODE like '{0}%'", deptCode);
            }

            return pagination;
        }
    }
}
