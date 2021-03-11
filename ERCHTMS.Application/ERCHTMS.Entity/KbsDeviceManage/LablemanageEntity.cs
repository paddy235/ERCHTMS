using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.KbsDeviceManage
{
    /// <summary>
    /// 描 述：标签管理
    /// </summary>
    [Table("BIS_LABLEMANAGE")]
    public class LablemanageEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键ID
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 标签ID
        /// </summary>
        /// <returns></returns>
        [Column("LABLEID")]
        public string LableId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        /// <returns></returns>
        [Column("NAME")]
        public string Name { get; set; }
        /// <summary>
        /// 类型0人员 1车辆
        /// </summary>
        /// <returns></returns>
        [Column("TYPE")]
        public int? Type { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        /// <returns></returns>
        [Column("DEPTNAME")]
        public string DeptName { get; set; }
        /// <summary>
        /// 部门ID
        /// </summary>
        /// <returns></returns>
        [Column("DEPTID")]
        public string DeptId { get; set; }
        /// <summary>
        /// 部门CODE
        /// </summary>
        /// <returns></returns>
        [Column("DEPTCODE")]
        public string DeptCode { get; set; }
        /// <summary>
        /// 标签类型
        /// </summary>
        /// <returns></returns>
        [Column("LABLETYPENAME")]
        public string LableTypeName { get; set; }
        /// <summary>
        /// 标签类型ID
        /// </summary>
        /// <returns></returns>
        [Column("LABLETYPEID")]
        public string LableTypeId { get; set; }
        /// <summary>
        /// 身份证号/驾驶员
        /// </summary>
        /// <returns></returns>
        [Column("IDCARDORDRIVER")]
        public string IdCardOrDriver { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        /// <returns></returns>
        [Column("PHONE")]
        public string Phone { get; set; }
        /// <summary>
        /// 绑定用户ID
        /// </summary>
        /// <returns></returns>
        [Column("USERID")]
        public string UserId { get; set; }
        /// <summary>
        /// 绑定时间
        /// </summary>
        /// <returns></returns>
        [Column("BINDTIME")]
        public DateTime? BindTime { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        /// <returns></returns>
        [Column("OPERUSERID")]
        public string OperUserId { get; set; }
        /// <summary>
        /// 创建人ID
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
        /// 操作用户
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 创建人所属部门
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 创建人所属机构
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }

        /// <summary>
        /// 是否绑定 0为未绑定 1为绑定
        /// </summary>
        /// <returns></returns>
        [Column("ISBIND")]
        public int IsBind { get; set; }

        /// <summary>
        /// 状态 在线/离线
        /// </summary>
        [Column("STATE")]
        public string State { get; set; }
        /// <summary>
        /// 电量
        /// </summary>
        [Column("POWER")]
        public string Power { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID) ? Guid.NewGuid().ToString() : ID;
            this.CreateDate = DateTime.Now;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
                       this.CreateUserDeptCode = OperatorProvider.Provider.Current().DeptCode;
           this.CreateUserOrgCode = OperatorProvider.Provider.Current().OrganizeCode;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.ID = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
                    }
        #endregion
    }
}