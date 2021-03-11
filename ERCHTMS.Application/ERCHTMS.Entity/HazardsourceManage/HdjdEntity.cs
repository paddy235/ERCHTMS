using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HazardsourceManage
{
    /// <summary>
    /// 描 述：登记建档
    /// </summary>
    [Table("HSD_HDJD")]
    public class HdjdEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("XGFJ")]
        public string Xgfj { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("AQJC")]
        public string Aqjc { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("AQPGBG")]
        public string Aqpgbg { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("YLPGFILE")]
        public string YlpgFile { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("YLPGFILENAME")]
        public string YlpgFileName { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("YLJHFILE")]
        public string YljhFile { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("YLJHFILENAME")]
        public string YljhFileName { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("YJYAPSFILE")]
        public string YjyapsFile { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("YJYAPSFILENAME")]
        public string YjyapsFileName { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("YJYAFILE")]
        public string YjyaFile { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("YJYAFILENAME")]
        public string YjyaFileName { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CZGCFILE")]
        public string GzgcFile { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CZGCFILENAME")]
        public string GzgcFileName { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("GLZDFILE")]
        public string GlzdFile { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("GLZDFILENAME")]
        public string GlzdFileName { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("HXPSMSIDS")]
        public string HxpsmsIds { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("HXPSMS")]
        public string Hxpsms { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 关联的重大危险源id
        /// </summary>
        /// <returns></returns>
        [Column("HDID")]
        public string HdId { get; set; }
        /// <summary>
        /// 基本特征表
        /// </summary>
        /// <returns></returns>
        [Column("JBTZB")]
        public string Jbtzb { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 创建人orgcode
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 是否备案 0否,1是
        /// </summary>
        /// <returns></returns>
        [Column("ISBA")]
        public int? Isba { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 二维码
        /// </summary>
        /// <returns></returns>
        [Column("QRCODE")]
        public string Qrcode { get; set; }
        /// <summary>
        /// 核销情况 0否,1是
        /// </summary>
        /// <returns></returns>
        [Column("ISHX")]
        public int? Ishx { get; set; }
        /// <summary>
        /// 创建人部门code
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 是否登记建档 0否,1是
        /// </summary>
        /// <returns></returns>
        [Column("ISDJJD")]
        public int? IsDjjd { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
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
            this.ID = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}