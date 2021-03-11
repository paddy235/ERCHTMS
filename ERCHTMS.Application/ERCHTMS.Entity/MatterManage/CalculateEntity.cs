using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.MatterManage
{
    /// <summary>
    /// 描 述：计量管理 
    /// </summary>
    [Table("WL_CALCULATE")]
    public class CalculateEntity : BaseEntity
    {
        #region 基本字段
        /// <summary>
        /// 修改用户名称
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 修改用户ID
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
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 创建用户机构编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 创建用户部门编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 创建用户部门ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTID")]
        public string Createuserdeptid { get; set; }
        /// <summary>
        /// 创建用户名称
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 创建用户Id
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("AUTOID")]
        public int? AUTOID { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        #endregion

        #region 业务成员

        /// <summary>
        /// 单子打印时间
        /// </summary>
        /// <returns></returns>
        [Column("STAMPTIME")]
        public DateTime? Stamptime { get; set; }
        /// <summary>
        /// 皮重司磅员
        /// </summary>
        /// <returns></returns>
        [Column("TAREUSERNAME")]
        public string Tareusername { get; set; }
        /// <summary>
        /// 毛重司磅员
        /// </summary>
        /// <returns></returns>
        [Column("ROUGHUSERNAME")]
        public string Roughusername { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// 皮重时间
        /// </summary>
        /// <returns></returns>
        [Column("TARETIME")]
        public DateTime? Taretime { get; set; }
        /// <summary>
        /// 毛重时间
        /// </summary>
        /// <returns></returns>
        [Column("ROUGHTIME")]
        public DateTime? Roughtime { get; set; }
        /// <summary>
        /// 车牌号码
        /// </summary>
        /// <returns></returns>
        [Column("PLATENUMBER")]
        public string Platenumber { get; set; }
        /// <summary>
        /// 运输类型
        /// </summary>
        /// <returns></returns>
        [Column("TRANSPORTTYPE")]
        public string Transporttype { get; set; }
        /// <summary>
        /// 提货方编码
        /// </summary>
        /// <returns></returns>
        [Column("TAKEGOODSID")]
        public string Takegoodsid { get; set; }
        /// <summary>
        /// 提货方(运货单位)
        /// </summary>
        /// <returns></returns>
        [Column("TAKEGOODSNAME")]
        public string Takegoodsname { get; set; }
        /// <summary>
        /// 异常提醒
        /// </summary>
        /// <returns></returns>
        [Column("UNUSUALREMIND")]
        public string Unusualremind { get; set; }
        /// <summary>
        /// 净重
        /// </summary>
        /// <returns></returns>
        [Column("NETWNEIGHT")]
        public double? Netwneight { get; set; }
        /// <summary>
        /// 皮重
        /// </summary>
        /// <returns></returns>
        [Column("TARE")]
        public double? Tare { get; set; }
        /// <summary>
        /// 毛重
        /// </summary>
        /// <returns></returns>
        [Column("ROUGH")]
        public double? Rough { get; set; }

        /// <summary>
        /// 皮重总和
        /// </summary>
        /// <returns></returns>
        [Column("TARECOUNT")]
        public double? TareCount { get; set; }
        /// <summary>
        /// 毛重总和
        /// </summary>
        /// <returns></returns>
        [Column("ROUGHCOUNT")]
        public double? RoughCount { get; set; }

        /// <summary>
        ///副产品类型(货名)
        /// </summary>
        /// <returns></returns>
        [Column("GOODSNAME")]
        public string Goodsname { get; set; }
        /// <summary>
        /// 称重单号
        /// </summary>
        /// <returns></returns>
        [Column("NUMBERS")]
        public string Numbers { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        /// <returns></returns>
        [Column("WEIGHT")]
        public double? Weight { get; set; }
        /// <summary>
        /// 磅名称
        /// </summary>
        /// <returns></returns>
        [Column("POUNDNAME")]
        public string Poundname { get; set; }
        /// <summary>
        /// 磅编号
        /// </summary>
        /// <returns></returns>
        [Column("POUNDCODE")]
        public string Poundcode { get; set; }

        /// <summary>
        /// 插入方式1自动0手动
        /// </summary>
        [Column("INSERTTYPE")]
        public string InsertType { get; set; }

        /// <summary>
        /// 是否可用1可用0不可用
        /// </summary>
        [Column("ISDELETE")]
        public int IsDelete { get; set; }

        /// <summary>
        /// 删除原因
        /// </summary>
        [Column("DELETECONTENT")]
        public string DeleteContent { get; set; }

        /// <summary>
        /// 入场开票主键、危害品主键
        /// </summary>
        [Column("BASEID")]
        public string BaseId { get; set; }

        /// <summary>
        /// 是否已出厂1是
        /// </summary>
        [Column("ISOUT")]
        public int? IsOut { get; set; }

        /// <summary>
        /// 数据类型 （4物料车（入场开票）、5危化品、99外来车辆）
        /// </summary>
        [Column("DATATYPE")]
        public string DataType { get; set; }

        /// <summary>
        /// 是否已入库（1是0否）
        /// </summary>
        [Column("ISSTORAGE")]
        public int? IsStorage { get; set; }

        /// <summary>
        /// 旧地磅记录主键
        /// </summary>
        [Column("OLDID")]
        public string OldId { get; set; }

       
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID = Guid.NewGuid().ToString();
            this.CreateDate = DateTime.Now;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            this.CreateUserName = OperatorProvider.Provider.Current().UserName;
            this.CreateUserDeptCode = OperatorProvider.Provider.Current().DeptCode;
            this.CreateUserOrgCode = OperatorProvider.Provider.Current().OrganizeCode;
            this.IsOut = 0; this.IsDelete = 1;
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
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}