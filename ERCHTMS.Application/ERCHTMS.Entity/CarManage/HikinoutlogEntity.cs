using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.CarManage
{
    /// <summary>
    /// 描 述：设备记录人员进出日志
    /// </summary>
    [Table("BIS_HIKINOUTLOG")]
    public class HikinoutlogEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 0为入 1为出
        /// </summary>
        /// <returns></returns>
        [Column("INOUT")]
        public int? InOut { get; set; }
        /// <summary>
        /// 创建人所属部门
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 人员姓名
        /// </summary>
        /// <returns></returns>
        [Column("USERNAME")]
        public string UserName { get; set; }
        /// <summary>
        /// 操作用户
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 相关联的抓拍图片
        /// </summary>
        /// <returns></returns>
        [Column("SCREENSHOT")]
        public string ScreenShot { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 0为双控人员 1为双控外包人员 2为场外临时人员
        /// </summary>
        /// <returns></returns>
        [Column("USERTYPE")]
        public int? UserType { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 部门ID
        /// </summary>
        /// <returns></returns>
        [Column("DEPTID")]
        public string DeptId { get; set; }
        /// <summary>
        /// 关联入场ID
        /// </summary>
        /// <returns></returns>
        [Column("INID")]
        public string InId { get; set; }
        /// <summary>
        /// 出厂时间 (未出厂为空)
        /// </summary>
        /// <returns></returns>
        [Column("OUTTIME")]
        public DateTime? OutTime { get; set; }
        /// <summary>
        /// 门禁点名称
        /// </summary>
        /// <returns></returns>
        [Column("DEVICENAME")]
        public string DeviceName { get; set; }
        /// <summary>
        /// 创建人所属机构
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 门禁设备类型 0为人脸识别设备 1为车辆道闸设备 2位门禁刷卡设备
        /// </summary>
        /// <returns></returns>
        [Column("DEVICETYPE")]
        public int? DeviceType { get; set; }
        /// <summary>
        /// 是否出厂 0为未出厂 1为出厂
        /// </summary>
        /// <returns></returns>
        [Column("ISOUT")]
        public int? IsOut { get; set; }
        /// <summary>
        /// 门禁点区域名称
        /// </summary>
        /// <returns></returns>
        [Column("AREANAME")]
        public string AreaName { get; set; }
        /// <summary>
        /// 门禁事件类型 1为人脸通过事件 2为车辆放行事件 3为门禁刷卡事件 4为门禁指纹通过事件
        /// </summary>
        /// <returns></returns>
        [Column("EVENTTYPE")]
        public int? EventType { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        /// <returns></returns>
        [Column("DEPTNAME")]
        public string DeptName { get; set; }
        /// <summary>
        /// 用户ID如果不是双控中用户则为空
        /// </summary>
        /// <returns></returns>
        [Column("USERID")]
        public string UserId { get; set; }
        /// <summary>
        /// 门禁点在海康平台ID
        /// </summary>
        /// <returns></returns>
        [Column("DEVICEHIKID")]
        public string DeviceHikID { get; set; }
        
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