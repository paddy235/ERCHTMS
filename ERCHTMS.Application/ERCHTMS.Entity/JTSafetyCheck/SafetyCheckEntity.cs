using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Code;

namespace ERCHTMS.Entity.JTSafetyCheck
{
    /// <summary>
    /// 描 述：安全事例
    /// </summary>
    [Table("jt_safetycheck")]
    public class SafetyCheckEntity : BaseEntity
    {
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 创建用户所属部门编码
        /// </summary>
        /// <returns></returns>
        [Column("DEPTCODE")]
        public string DeptCode { get; set; }
        /// <summary>
        /// 检查名称
        /// </summary>
        /// <returns></returns>
        [Column("CHECKTITLE")]
        public string CheckTitle { get; set; }

        /// <summary>
        /// 检查类型
        /// </summary>
        /// <returns></returns>
        [Column("CHECKTYPE")]
        public string CheckType { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        /// <returns></returns>
        [Column("STARTDATE")]
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        /// <returns></returns>
        [Column("ENDDATE")]
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 检查级别
        /// </summary>
        /// <returns></returns>
        [Column("CHECKLEVEL")]
        public string CheckLevel { get; set; }
        /// <summary>
        ///检查部门(多个用英文逗号分隔)
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDEPT")]
        public string CheckDept { get; set; }
        /// <summary>
        ///检查部门编码(多个用英文逗号分隔)
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDEPTCODE")]
        public string CheckDeptCode{ get; set; }
        /// <summary>
        ///检查部门编码(多个用英文逗号分隔)
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDEPTID")]
        public string CheckDeptId { get; set; }
        /// <summary>
        /// 创建用户id
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        /// <returns></returns>
        [Column("CHARGEMAN")]
        public string ChargeMan { get; set; }
        /// <summary>
        /// 负责人ID
        /// </summary>
        /// <returns></returns>
        [Column("CHARGEMANID")]
        public string ChargeManId { get; set; }

        /// <summary>
        ///检查成员(多个用英文逗号分隔)
        /// </summary>
        /// <returns></returns>
        [Column("CHECKUSER")]
        public string CheckUser { get; set; }
        /// <summary>
        /// 检查成员账号(多个用英文逗号分隔)
        /// </summary>
        /// <returns></returns>
        [Column("CHECKUSERID")]
        public string CheckUserId { get; set; }
        /// <summary>
        ///检查区域
        /// </summary>
        /// <returns></returns>
        [Column("CHECKAREA")]
        public string CheckArea { get; set; }
        /// <summary>
        /// 记录创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATETIME")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }

        [NotMapped]
        public string aaa { get; set; }

        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;
            this.CreateTime = DateTime.Now;
            var user=OperatorProvider.Provider.Current();
            if (user!=null)
            {
                this.CreateUserId = user.UserId;
                DeptCode = user.DeptCode;
            }
           
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.Id = keyValue;
        }
    }
}
