using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.SafePunish
{
    /// <summary>
    /// 描 述：安全惩罚
    /// </summary>
    [Table("BIS_SAFEKPIDATA")]
    public class SafekpidataEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
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
        /// 安全惩罚id
        /// </summary>
        /// <returns></returns>
        [Column("SAFEPUNISHID")]
        public string SafePunishId { get; set; }
        /// <summary>
        /// 主要责任人id
        /// </summary>
        /// <returns></returns>
        [Column("KPIUSERID")]
        public string KpiUserId { get; set; }
        /// <summary>
        /// 主要责任人name
        /// </summary>
        /// <returns></returns>
        [Column("KPIUSERNAME")]
        public string KpiUserName { get; set; }
        /// <summary>
        /// 主要责任人扣分
        /// </summary>
        /// <returns></returns>
        [Column("WSSJSCORE")]
        public string WssjScore { get; set; }
        /// <summary>
        /// 次要责任人id
        /// </summary>
        /// <returns></returns>
        [Column("MINORUSERID")]
        public string MinorUserId { get; set; }
        /// <summary>
        /// 次要责任人name
        /// </summary>
        /// <returns></returns>
        [Column("MINORUSERNAME")]
        public string MinorUserName { get; set; }
        /// <summary>
        /// 次要责任人扣分
        /// </summary>
        /// <returns></returns>
        [Column("KPISCORE1")]
        public string KpiScore1 { get; set; }
        /// <summary>
        /// 专工id
        /// </summary>
        /// <returns></returns>
        [Column("SPECIALTYUSERID")]
        public string SpecialtyUserId { get; set; }
        /// <summary>
        /// 专工name
        /// </summary>
        /// <returns></returns>
        [Column("SPECIALTYUSERNAME")]
        public string SpecialtyUserName { get; set; }
        /// <summary>
        /// 专工扣分
        /// </summary>
        /// <returns></returns>
        [Column("KPISCORE2")]
        public string KpiScore2 { get; set; }
        /// <summary>
        /// 主管id
        /// </summary>
        /// <returns></returns>
        [Column("SUPERVISORID")]
        public string SupervisorId { get; set; }
        /// <summary>
        /// 主管name
        /// </summary>
        /// <returns></returns>
        [Column("SUPERVISORNAME")]
        public string SupervisorName { get; set; }
        /// <summary>
        /// 主管扣分
        /// </summary>
        /// <returns></returns>
        [Column("KPISCORE3")]
        public string KpiScore3 { get; set; }
        /// <summary>
        /// 主要部门副职id
        /// </summary>
        /// <returns></returns>
        [Column("SECONDARYID2")]
        public string SecondaryId2 { get; set; }
        /// <summary>
        /// 主要部门副职name
        /// </summary>
        /// <returns></returns>
        [Column("SECONDARYNAME2")]
        public string SecondaryName2 { get; set; }
        /// <summary>
        /// 主要部门副职扣分
        /// </summary>
        /// <returns></returns>
        [Column("KPISCORE4")]
        public string KpiScore4 { get; set; }
        /// <summary>
        /// 主要部门正职id
        /// </summary>
        /// <returns></returns>
        [Column("SECONDARYID1")]
        public string SecondaryId1 { get; set; }
        /// <summary>
        /// 主要部门正职name
        /// </summary>
        /// <returns></returns>
        [Column("SECONDARYNAME1")]
        public string SecondaryName1 { get; set; }
        /// <summary>
        /// 主要部门正职扣分
        /// </summary>
        /// <returns></returns>
        [Column("KPISCORE5")]
        public string KpiScore5 { get; set; }
        /// <summary>
        /// 专工或值班负责人id
        /// </summary>
        /// <returns></returns>
        [Column("ONDUTYUSERID")]
        public string OnDutyUserId { get; set; }
        /// <summary>
        /// 专工或值班负责人name
        /// </summary>
        /// <returns></returns>
        [Column("ONDUTYUSERNAME")]
        public string OnDutyUserName { get; set; }
        /// <summary>
        /// 专工或值班负责人扣分
        /// </summary>
        /// <returns></returns>
        [Column("KPISCORE6")]
        public string KpiScore6 { get; set; }
        /// <summary>
        /// 主任工程师id
        /// </summary>
        /// <returns></returns>
        [Column("DIRECOTORID")]
        public string DirecotorId { get; set; }
        /// <summary>
        /// 主任工程师name
        /// </summary>
        /// <returns></returns>
        [Column("DIRECOTORNAME")]
        public string DirecotorName { get; set; }
        /// <summary>
        /// 主任工程师扣分
        /// </summary>
        /// <returns></returns>
        [Column("KPISCORE7")]
        public string KpiScore7 { get; set; }
        /// <summary>
        /// 次要部门副职id
        /// </summary>
        /// <returns></returns>
        [Column("SECONDARYID3")]
        public string SecondaryId3 { get; set; }
        /// <summary>
        /// 次要部门副职name
        /// </summary>
        /// <returns></returns>
        [Column("SECONDARYNAME3")]
        public string SecondaryName3 { get; set; }
        /// <summary>
        /// 次要部门副职扣分
        /// </summary>
        /// <returns></returns>
        [Column("KPISCORE8")]
        public string KpiScore8 { get; set; }
        /// <summary>
        /// 次要部门正职id
        /// </summary>
        /// <returns></returns>
        [Column("SECONDARYID4")]
        public string SecondaryId4 { get; set; }
        /// <summary>
        /// 次要部门正职name
        /// </summary>
        /// <returns></returns>
        [Column("SECONDARYNAME4")]
        public string SecondaryName4 { get; set; }
        /// <summary>
        /// 次要部门正职扣分
        /// </summary>
        /// <returns></returns>
        [Column("KPISCORE9")]
        public string KpiScore9 { get; set; }

        /// <summary>
        /// 一类障碍主要负责人Name
        /// </summary>
        [Column("PRINCIPALNAME")]
        public string PrincipalName { get; set; }
        /// <summary>
        /// 一类障碍主要负责人id
        /// </summary>
        [Column("PRINCIPALID")]
        public string PrincipalId { get; set; }

        /// <summary>
        /// 一类障碍次要负责人Name
        /// </summary>
        [Column("OTHERPRINCIPALNAME")]
        public string OtherPrincipalName { get; set; }

        /// <summary>
        /// 一类障碍次要负责人id
        /// </summary>
        [Column("OTHERPRINCIPALID")]
        public string OtherPrincipalId { get; set; }

        /// <summary>
        /// 责任部门领导Name
        /// </summary>
        [Column("DEPTLEADNAME")]
        public string DeptLeadName { get; set; }

        /// <summary>
        /// 责任部门领导Id
        /// </summary>
        [Column("DEPTLEADID")]
        public string DeptLeadId { get; set; }




        /// <summary>
        /// 责任部门分管副职Name
        /// </summary>
        [Column("FGDEPTLEADNAME")]
        public string FgDeptLeadName { get; set; }



        /// <summary>
        /// 责任部门分管副职Id
        /// </summary>
        [Column("FGDEPTLEADID")]
        public string FgDeptLeadId { get; set; }



        /// <summary>
        /// 项目公司分管副职Name
        /// </summary>
        [Column("FGLEADFZNAME")]
        public string FgLeadFzName { get; set; }


        /// <summary>
        /// 项目公司分管副职Id
        /// </summary>
        [Column("FGLEADFZID")]
        public string FgLeadFzId { get; set; }




        [Column("KPISCORE10")]
        public string KpiScore10 { get; set; }

        [Column("KPISCORE11")]
        public string KpiScore11 { get; set; }

        [Column("KPISCORE12")]
        public string KpiScore12 { get; set; }

        [Column("KPISCORE13")]
        public string KpiScore13 { get; set; }

        [Column("KPISCORE14")]
        public string KpiScore14 { get; set; }

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
            this.SafePunishId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}