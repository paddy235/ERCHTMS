using ERCHTMS.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.PersonManage
{
    /// <summary>
    /// 临时人员
    /// </summary>
    [Table("BIS_TEMPORARYUSER")]
    public class TemporaryUserEntity:BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("USERID")]
        public string USERID { get; set; }

        /// <summary>
        /// 操作用户
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
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
        /// 创建人所属机构
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 创建人所属部门
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }

        /// <summary>
        /// 组名称
        /// </summary>
        /// <returns></returns>
        [Column("GROUPSNAME")]
        public string GroupsName { get; set; }

        /// <summary>
        /// 组Id
        /// </summary>
        /// <returns></returns>
        [Column("GROUPSID")]
        public string Groupsid { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        /// <returns></returns>
        [Column("USERNAME")]
        public string UserName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        /// <returns></returns>
        [Column("GENDER")]
        public string Gender { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        /// <returns></returns>
        [Column("IDENTIFYID")]
        public string Identifyid { get; set; }


        /// <summary>
        /// 单位
        /// </summary>
        /// <returns></returns>
        [Column("COMNAME")]
        public string ComName { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        /// <returns></returns>
        [Column("TEL")]
        public string Tel { get; set; }
        /// <summary>
        /// 考勤开始时间
        /// </summary>
        /// <returns></returns>
        [Column("STARTTIME")]
        public DateTime? startTime { get; set; }
        /// <summary>
        /// 考勤结束时间
        /// </summary>
        /// <returns></returns>
        [Column("ENDTIME")]
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 通行门岗
        /// </summary>
        /// <returns></returns>
        [Column("PASSPOST")]
        public string PassPost { get; set; }
        /// <summary>
        /// 通行门岗设备唯一Id
        /// </summary>
        [Column("PASSPOSTID")]
        public string PassPostId { get; set; }
        /// <summary>
        /// 是否禁入
        /// </summary>
        /// <returns></returns>
        [Column("ISDEBAR")]
        public int ISDebar { get; set; }
        /// <summary>
        /// 人脸图片
        /// </summary>
        /// <returns></returns>
        [Column("USERIMG")]
        public string UserImg { get; set; }

        /// <summary>
        /// 人脸base64
        /// </summary>
        /// <returns></returns>
        [Column("IMGDATA")]
        public string ImgData { get; set; }

        /// <summary>
        /// 禁入原因
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }

        /// <summary>
        /// 是否临时人员（1临时0电厂）
        /// </summary>
        /// <returns></returns>
        [Column("ISTEMPORARY")]
        public int Istemporary { get; set; }

        /// <summary>
        /// 岗位 
        /// </summary>
        /// <returns></returns>
        [Column("POSTNAME")]
        public string Postname { get; set; }

        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.USERID = string.IsNullOrEmpty(USERID) ? Guid.NewGuid().ToString() : USERID;
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
            this.USERID = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
        }
        #endregion
    }
}
