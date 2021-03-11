using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.OutsourcingProject
{
    /// <summary>
    /// 描 述：资质审查人员表
    /// </summary>
    [Table("EPG_APTITUDEINVESTIGATEPEOPLE")]
    public class AptitudeinvestigatepeopleEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 用户主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 用户编码
        /// </summary>
        /// <returns></returns>
        [Column("ENCODE")]
        public string ENCODE { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        /// <returns></returns>
        [Column("REALNAME")]
        public string REALNAME { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        /// <returns></returns>
        [Column("HEADICON")]
        public string HEADICON { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        /// <returns></returns>
        [Column("BIRTHDAY")]
        public DateTime? BIRTHDAY { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        /// <returns></returns>
        [Column("MOBILE")]
        public string MOBILE { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        /// <returns></returns>
        [Column("TELEPHONE")]
        public string TELEPHONE { get; set; }
        /// <summary>
        /// 电子邮件
        /// </summary>
        /// <returns></returns>
        [Column("EMAIL")]
        public string EMAIL { get; set; }
        /// <summary>
        /// QQ号
        /// </summary>
        /// <returns></returns>
        [Column("OICQ")]
        public string OICQ { get; set; }
        /// <summary>
        /// 微信号
        /// </summary>
        /// <returns></returns>
        [Column("WECHAT")]
        public string WECHAT { get; set; }
        /// <summary>
        /// MSN
        /// </summary>
        /// <returns></returns>
        [Column("MSN")]
        public string MSN { get; set; }
        /// <summary>
        /// 机构主键
        /// </summary>
        /// <returns></returns>
        [Column("ORGANIZEID")]
        public string ORGANIZEID { get; set; }
        /// <summary>
        /// 岗位主键
        /// </summary>
        /// <returns></returns>
        [Column("DUTYID")]
        public string DUTYID { get; set; }
        /// <summary>
        /// 岗位名称
        /// </summary>
        /// <returns></returns>
        [Column("DUTYNAME")]
        public string DUTYNAME { get; set; }
        /// <summary>
        /// 职位主键
        /// </summary>
        /// <returns></returns>
        [Column("POSTID")]
        public string POSTID { get; set; }
        /// <summary>
        /// 职位名称
        /// </summary>
        /// <returns></returns>
        [Column("POSTNAME")]
        public string POSTNAME { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        /// <returns></returns>
        [Column("GENDER")]
        public string GENDER { get; set; }
        /// <summary>
        /// 是否特种设备操作人员
        /// </summary>
        /// <returns></returns>
        [Column("ISSPECIALEQU")]
        public string ISSPECIALEQU { get; set; }
        /// <summary>
        /// 是否特种作业人员
        /// </summary>
        /// <returns></returns>
        [Column("ISSPECIAL")]
        public string ISSPECIAL { get; set; }
        /// <summary>
        /// 外包工程Id
        /// </summary>
        /// <returns></returns>
        [Column("OUTENGINEERID")]
        public string OUTENGINEERID { get; set; }
        /// <summary>
        /// 民族
        /// </summary>
        /// <returns></returns>
        [Column("NATION")]
        public string NATION { get; set; }
        /// <summary>
        /// 人员类型
        /// </summary>
        /// <returns></returns>
        [Column("USERTYPE")]
        public string USERTYPE { get; set; }
        /// <summary>
        /// 籍贯
        /// </summary>
        /// <returns></returns>
        [Column("NATIVE")]
        public string NATIVE { get; set; }
        /// <summary>
        /// 是否外包人员
        /// </summary>
        /// <returns></returns>
        [Column("ISEPIBOLY")]
        public string ISEPIBOLY { get; set; }
        /// <summary>
        /// 学历主键
        /// </summary>
        /// <returns></returns>
        [Column("DEGREESID")]
        public string DEGREESID { get; set; }
        /// <summary>
        /// 学历
        /// </summary>
        /// <returns></returns>
        [Column("DEGREES")]
        public string DEGREES { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        /// <returns></returns>
        [Column("IDENTIFYID")]
        public string IDENTIFYID { get; set; }
        /// <summary>
        /// 外包单位编码
        /// </summary>
        /// <returns></returns>
        [Column("OUTPROJECTCODE")]
        public string OUTPROJECTCODE { get; set; }
        /// <summary>
        /// 机构编码
        /// </summary>
        /// <returns></returns>
        [Column("ORGANIZECODE")]
        public string ORGANIZECODE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CREATEUSERDEPTCODE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CREATEUSERORGCODE { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CREATEDATE { get; set; }
        /// <summary>
        /// 创建用户主键
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CREATEUSERID { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CREATEUSERNAME { get; set; }
        /// <summary>
        /// 修改日期
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }
        /// <summary>
        /// 修改用户主键
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string MODIFYUSERID { get; set; }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string MODIFYUSERNAME { get; set; }
        /// <summary>
        /// 外包单位Id
        /// </summary>
        /// <returns></returns>
        [Column("OUTPROJECTID")]
        public string OUTPROJECTID { get; set; }
        /// <summary>
        /// 人员审查Id
        /// </summary>
        /// <returns></returns>
        [Column("PEOPLEREVIEWID")]
        public string PEOPLEREVIEWID { get; set; }
        [Column("WORKOFTYPE")]
        public string WORKOFTYPE { get; set; }
        [Column("WORKYEAR")]
        public string WORKYEAR { get; set; }
        [Column("STATEOFHEALTH")]
        public string STATEOFHEALTH { get; set; }
        [Column("DESCRIPTION")]
        public string DESCRIPTION { get; set; }
        /// <summary>
        /// 是否四种人
        /// </summary>
        [Column("ISFOURPERSON")]
        public string ISFOURPERSON { get; set; }
        /// <summary>
        /// 四种人类别
        /// </summary>
        [Column("FOURPERSONTYPE")]
        public string FOURPERSONTYPE { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        [Column("ACCOUNTS")]
        public string ACCOUNTS { get; set; }
        ///// <summary>
        ///// 是否已同步 1 同步 0 未同步
        ///// </summary>
        //public string ISSYNCH { get; set; }
       
        /// <summary>
        /// 禁忌症名称
        /// </summary>
        [Column("COMTRAINDICATIONNAME")]
        public string ComtraindicationName { get; set; }
        /// <summary>
        /// 是否有从事职业的禁忌症
        /// </summary>
        [Column("ISCOMTRAINDICATION")]
        public string IsComtraindication { get; set; }
        /// <summary>
        /// 体检时间
        /// </summary>
        [Column("PHYSICALTIME")]
        public DateTime? PhysicalTime { get; set; }
        /// <summary>
        /// 体检单位
        /// </summary>
        [Column("PHYSICALUNIT")]
        public string PhysicalUnit { get; set; }
        /// <summary>
        /// 账号来源（0:本地，1:LDAP）
        /// </summary>
        [Column("ACCOUNTTYPE")]
        public string AccountType { get; set; }
        /// <summary>
        /// 是否申请ldap账号（0:不是，1:是）
        /// </summary>
        [Column("ISAPPLICATIONLDAP")]
        public string IsApplicationLdap { get; set; }

        /// <summary>
        /// 是否超龄 （0:不是，1:是）
        /// </summary>
        [Column("ISOVERAGE")]
        public string IsOverAge { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        [Column("AGE")]
        public string Age { get; set; }

        /// <summary>
        ///省份
        /// </summary>		
        [Column("QUICKQUERY")]
        public string QuickQuery { get; set; }
        /// <summary>
        /// 省份编码
        /// </summary>		
        [Column("SIMPLESPELLING")]
        public string SimpleSpelling { get; set; }

        /// <summary>
        /// 市编码
        /// </summary>		
        [Column("MANAGERID")]
        public string ManagerId { get; set; }
        /// <summary>
        /// 市
        /// </summary>	
        [Column("MANAGER")]
        public string Manager { get; set; }

        /// <summary>
        /// 区
        /// </summary>		
        [Column("DISTRICT")]
        public string District { get; set; }
        /// <summary>
        /// 区编码
        /// </summary>		
        [Column("DISTRICTCODE")]
        public string DistrictCode { get; set; }
        /// <summary>
        /// 街道
        /// </summary>		
        [Column("STREET")]
        public string Street { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>		
        [Column("ADDRESS")]
        public string Address { get; set; }
        /// <summary>
        /// 用工方式
        /// </summary>
        [Column("WORKERTYPE")]
        public string WorkerType { get; set; }

        /// <summary>
        /// 专业分类
        /// </summary>
        [Column("SPECIALTYTYPE")]
        public string SpecialtyType { get; set; }

        /// <summary>
        /// 人员资质是否提交 0：未提交   1：已经提交    默认为0   
        /// </summary>
        [Column("SUBMITSTATE")]
        public int? SubmitState { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID) ? Guid.NewGuid().ToString() : ID;
            this.CREATEDATE = DateTime.Now;
            this.CREATEUSERID = OperatorProvider.Provider.Current().UserId;
            this.CREATEUSERNAME = OperatorProvider.Provider.Current().UserName;
            this.CREATEUSERDEPTCODE = OperatorProvider.Provider.Current().DeptCode;
            this.CREATEUSERORGCODE = OperatorProvider.Provider.Current().OrganizeCode;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.ID = keyValue;
            this.MODIFYDATE = DateTime.Now;
            this.MODIFYUSERID = OperatorProvider.Provider.Current().UserId;
            this.MODIFYUSERNAME = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
    public class PhyInfoEntity {
        /// <summary>
        /// 体检单位
        /// </summary>
        public string PhysicalUnit { get; set; }
        /// <summary>
        /// 体检时间
        /// </summary>
        public DateTime? PhysicalTime { get; set; }
        /// <summary>
        /// 是否有从事职业的禁忌症
        /// </summary>
        public string IsComtraindication { get; set; }
        /// <summary>
        /// 禁忌症名称
        /// </summary>
        public string ComtraindicationName { get; set; }
        /// <summary>
        /// 需要更新的人员id集合
        /// </summary>
        public string ids { get; set; }
    }
}