using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.AuthorizeManage
{
    public class UserAuthorizeEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get;set;}
        /// <summary>
        /// 用户所属部门Code
        /// </summary>
        public string DeptCode { get; set; }
        /// <summary>
        /// 用户所属机构Code
        /// </summary>
        public string OrgCode { get; set; }
    }
}
