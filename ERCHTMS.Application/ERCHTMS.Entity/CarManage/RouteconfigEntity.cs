using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.CarManage
{
    /// <summary>
    /// 描 述：车辆路线配置树
    /// </summary>
    [Table("BIS_ROUTECONFIG")]
    public class RouteconfigEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
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
        /// 项名称
        /// </summary>
        /// <returns></returns>
        [Column("ITEMNAME")]
        public string ItemName { get; set; }
        /// <summary>
        /// 项关联ID
        /// </summary>
        /// <returns></returns>
        [Column("GID")]
        public string GID { get; set; }
        /// <summary>
        /// 当前级数(固定三级)
        /// </summary>
        /// <returns></returns>
        [Column("LEVEL")]
        public int? Level { get; set; }
        /// <summary>
        /// 父ID
        /// </summary>
        /// <returns></returns>
        [Column("PARENTID")]
        public string ParentId { get; set; }
        /// <summary>
        /// 是否启用路线 （0为否 1为是）
        /// </summary>
        /// <returns></returns>
        [Column("ISENABLE")]
        public int IsEnable { get; set; }
        /// <summary>
        /// 是否到码头 （0为否 1为是）
        /// </summary>
        /// <returns></returns>
        [Column("ISPIER")]
        public int? IsPier { get; set; }
        /// <summary>
        /// 排序码
        /// </summary>
        /// <returns></returns>
        [Column("SORT")]
        public int? Sort { get; set; }
        /// <summary>
        /// 路线的坐标点
        /// </summary>
        /// <returns></returns>
        [Column("POINTLIST")]
        public string PointList { get; set; }
        /// <summary>
        /// 路线类别(0其他路线 1拜访路线)
        /// </summary>
        /// <returns></returns>
        [Column("LINETYPE")]
        public int LineType { get; set; }
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