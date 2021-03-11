using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.SystemManage
{
    [Table("BASE_DEPTMENUAUTH")]
   public  class DeptMenuAuthEntity : BaseEntity
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// DeptId
        /// </summary>
        /// <returns></returns>
        [Column("DEPTID")]
        public string DeptId { get; set; }
        /// <summary>
        /// DeptCode
        /// </summary>
        /// <returns></returns>
        [Column("DEPTCODE")]
        public string DeptCode { get; set; }
        /// <summary>
        /// DeptName
        /// </summary>
        /// <returns></returns>
        [Column("DEPTNAME")]
        public string DeptName { get; set; }
        /// <summary>
        /// ModuleId
        /// </summary>
        /// <returns></returns>
        [Column("MODULEID")]
        public string ModuleId { get; set; }

        public DeptMenuAuthEntity Clone(string deptId,string deptCode,string deptName)
        {
            DeptMenuAuthEntity deptMenu = new DeptMenuAuthEntity()
            {
              Id = Guid.NewGuid().ToString(),
              DeptCode =            deptCode,
              DeptId = deptId,
              DeptName = deptName,
              ModuleId = this.ModuleId,
            };
            return deptMenu;
        }
    }
}
