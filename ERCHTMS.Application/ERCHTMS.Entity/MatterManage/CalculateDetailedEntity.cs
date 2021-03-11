using ERCHTMS.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.MatterManage
{
    /// <summary>
    /// 计量管理详情表
    /// </summary>
    [Table("WL_CALCULATEDETAILED")]
    public class CalculateDetailedEntity : BaseEntity
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
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        #endregion

        #region 业务成员
        /// <summary>
        /// 称重单号
        /// </summary>
        /// <returns></returns>
        [Column("NUMBERS")]
        public string Numbers { get; set; }


        /// <summary>
        ///副产品类型（货名）
        /// </summary>
        /// <returns></returns>
        [Column("GOODSNAME")]
        public string Goodsname { get; set; }



        /// <summary>
        /// 提货方(运货单位)
        /// </summary>
        /// <returns></returns>
        [Column("TAKEGOODSNAME")]
        public string Takegoodsname { get; set; }


        /// <summary>
        /// 车牌号码
        /// </summary>
        /// <returns></returns>
        [Column("PLATENUMBER")]
        public string Platenumber { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }


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
        /// 数据类型（4 物料车（入场开票）、5危化品、99外来车辆）
        /// </summary>
        [Column("DATATYPE")]
        public string DataType { get; set; }


        /// <summary>
        /// 是否卸船
        /// </summary>
        [Column("SHIPUNLOADING")]
        public int ShipUnloading { get; set; }

        /// <summary>
        /// 地磅放行时间
        /// </summary>
        [Column("OUTTIME")]
        public DateTime? OutTime { get; set; }

        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID = Guid.NewGuid().ToString();
            this.CreateDate = DateTime.Now;
            if (string.IsNullOrEmpty(this.CreateUserId))
                this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            if (string.IsNullOrEmpty(this.CreateUserName))
                this.CreateUserName = OperatorProvider.Provider.Current().UserName;
            if (string.IsNullOrEmpty(this.CreateUserDeptCode))
                this.CreateUserDeptCode = OperatorProvider.Provider.Current().DeptCode;
            if (string.IsNullOrEmpty(this.CreateUserOrgCode))
                this.CreateUserOrgCode = OperatorProvider.Provider.Current().OrganizeCode;
            if (string.IsNullOrEmpty(this.Createuserdeptid))
                this.Createuserdeptid = OperatorProvider.Provider.Current().DeptId;
            this.IsDelete = 1;
            this.DataType = "0";
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
