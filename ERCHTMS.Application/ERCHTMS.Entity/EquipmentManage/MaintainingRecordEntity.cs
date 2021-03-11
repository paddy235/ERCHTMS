using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.EquipmentManage
{
    /// <summary>
    /// 描 述：维护保养记录表
    /// </summary>
    [Table("BIS_MAINTAININGRECORD")]
    public class MaintainingRecordEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }

        /// <summary>
        /// 记录名称
        /// </summary>
        /// <returns></returns>
        [Column("RECORDNAME")]
        public string RecordName { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        /// <returns></returns>
        [Column("EQUIPMENTNAME")]
        public string EquipmentName { get; set; }
        /// <summary>
        /// 设备ID
        /// </summary>
        /// <returns></returns>
        [Column("EQUIPMENTID")]
        public string EquipmentId { get; set; }
        /// <summary>
        /// 设备编号
        /// </summary>
        /// <returns></returns>
        [Column("EQUIPMENTNO")]
        public string EquipmentNo{ get; set; }
        /// <summary>
        /// 所在区域
        /// </summary>
        /// <returns></returns>
        [Column("DISTRICT")]
        public string District { get; set; }
        /// <summary>
        /// 所在区域ID
        /// </summary>
        /// <returns></returns>
        [Column("DISTRICTID")]
        public string DistrictID { get; set; }
        /// <summary>
        /// 保养人员
        /// </summary>
        /// <returns></returns>
        [Column("MAINTAININGUSER")]
        public string MaintainingUser { get; set; }
        /// <summary>
        /// 保养单位
        /// </summary>
        /// <returns></returns>
        [Column("MAINTAININGDEPT")]
        public string MaintainingDept { get; set; }
        /// <summary>
        /// 保养时间
        /// </summary>
        /// <returns></returns>
        [Column("MAINTAININGDATE")]
        public DateTime? MaintainingDate { get; set; }
        /// <summary>
        /// 保养内容
        /// </summary>
        /// <returns></returns>
        [Column("MAINTAININGCONTENT")]
        public string MaintainingContent { get; set; }
        /// <summary>
        /// 保养结果
        /// </summary>
        /// <returns></returns>
        [Column("MAINTAININGRESULT")]
        public string MaintainingResult { get; set; }
        /// <summary>
        /// 结果验证
        /// </summary>
        /// <returns></returns>
        [Column("RESULTPROVING")]
        public string ResultProving { get; set; }
        /// <summary>
        /// 登记人员
        /// </summary>
        /// <returns></returns>
        [Column("REGISTERUSER")]
        public string RegisterUser { get; set; }
        /// <summary>
        /// 登记人员ID
        /// </summary>
        /// <returns></returns>
        [Column("REGISTERUSERID")]
        public string RegisterUserId { get; set; }
        /// <summary>
        /// 登记时间
        /// </summary>
        /// <returns></returns>
        [Column("REGISTERDATE")]
        public DateTime? RegisterDate { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id)?Guid.NewGuid().ToString():Id;
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