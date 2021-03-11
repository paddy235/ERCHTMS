using ERCHTMS.Code;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.HighRiskWork
{
    /// <summary>
    /// 描 述：消防水执行情况
    /// </summary>
    [Table("BIS_FIREWATER")]
    public class FireWaterCondition:BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 执行部门
        /// </summary>
        /// <returns></returns>
        [Column("CONDITIONDEPTCODE")]
        public string ConditionDeptCode { get; set; }
        /// <summary>
        /// 执行部门
        /// </summary>
        /// <returns></returns>
        [Column("CONDITIONDEPT")]
        public string ConditionDept { get; set; }
        /// <summary>
        /// 执行部门
        /// </summary>
        /// <returns></returns>
        [Column("CONDITIONDEPTID")]
        public string ConditionDeptId { get; set; }
        /// <summary>
        /// 执行人
        /// </summary>
        /// <returns></returns>
        [Column("CONDITIONPERSON")]
        public string ConditionPerson { get; set; }
        /// <summary>
        /// 执行人
        /// </summary>
        /// <returns></returns>
        [Column("CONDITIONPERSONID")]
        public string ConditionPersonId { get; set; }
        /// <summary>
        /// 执行时间
        /// </summary>
        /// <returns></returns>
        [Column("CONDITIONTIME")]
        public DateTime? ConditionTime { get; set; }
        /// <summary>
        /// 执行情况
        /// </summary>
        /// <returns></returns>
        [Column("CONDITIONCONTENT")]
        public string ConditionContent { get; set; }
        /// <summary>
        /// 消防水Id
        /// </summary>
        /// <returns></returns>
        [Column("FIREWATERID")]
        public string FireWaterId { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 创建用户id
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 修改用户id
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 创建用户所属部门编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 创建用户所属机构编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }

        /// <summary>
        /// 作业台账状态 0:作业开始 1:作业结束
        /// </summary>
        [Column("LEDGERTYPE")]
        public string LedgerType { get; set; }

        /// <summary>
        /// 现场图片
        /// </summary>
        [NotMapped]
        public string ScenePicPath { get; set; }

        /// <summary>
        /// 现场图片集合
        /// </summary>
        [NotMapped]
        public IList<Photo> piclist { get; set; }

        [NotMapped]
        public int num { get; set; }

        /// <summary>
        /// 附件数量
        /// </summary>
        [NotMapped]
        public string filenum { get; set; }

        /// <summary>
        /// 现场图片集合
        /// </summary>
        [NotMapped]
        public IList<Photo> filelist { get; set; }

        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;
            this.CreateDate = DateTime.Now;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            this.CreateUserName = OperatorProvider.Provider.Current().UserName;
            this.CreateUserDeptCode = OperatorProvider.Provider.Current().DeptCode;
            this.CreateUserOrgCode = OperatorProvider.Provider.Current().OrganizeCode;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.Id = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}
