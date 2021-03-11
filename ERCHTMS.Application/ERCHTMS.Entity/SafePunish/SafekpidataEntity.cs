using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.SafePunish
{
    /// <summary>
    /// �� ������ȫ�ͷ�
    /// </summary>
    [Table("BIS_SAFEKPIDATA")]
    public class SafekpidataEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// �����û�id
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// �����û�
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// �޸��û�id
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// �޸�ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// �޸��û�
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// �����û��������ű���
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// �����û�������������
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// ��ȫ�ͷ�id
        /// </summary>
        /// <returns></returns>
        [Column("SAFEPUNISHID")]
        public string SafePunishId { get; set; }
        /// <summary>
        /// ��Ҫ������id
        /// </summary>
        /// <returns></returns>
        [Column("KPIUSERID")]
        public string KpiUserId { get; set; }
        /// <summary>
        /// ��Ҫ������name
        /// </summary>
        /// <returns></returns>
        [Column("KPIUSERNAME")]
        public string KpiUserName { get; set; }
        /// <summary>
        /// ��Ҫ�����˿۷�
        /// </summary>
        /// <returns></returns>
        [Column("WSSJSCORE")]
        public string WssjScore { get; set; }
        /// <summary>
        /// ��Ҫ������id
        /// </summary>
        /// <returns></returns>
        [Column("MINORUSERID")]
        public string MinorUserId { get; set; }
        /// <summary>
        /// ��Ҫ������name
        /// </summary>
        /// <returns></returns>
        [Column("MINORUSERNAME")]
        public string MinorUserName { get; set; }
        /// <summary>
        /// ��Ҫ�����˿۷�
        /// </summary>
        /// <returns></returns>
        [Column("KPISCORE1")]
        public string KpiScore1 { get; set; }
        /// <summary>
        /// ר��id
        /// </summary>
        /// <returns></returns>
        [Column("SPECIALTYUSERID")]
        public string SpecialtyUserId { get; set; }
        /// <summary>
        /// ר��name
        /// </summary>
        /// <returns></returns>
        [Column("SPECIALTYUSERNAME")]
        public string SpecialtyUserName { get; set; }
        /// <summary>
        /// ר���۷�
        /// </summary>
        /// <returns></returns>
        [Column("KPISCORE2")]
        public string KpiScore2 { get; set; }
        /// <summary>
        /// ����id
        /// </summary>
        /// <returns></returns>
        [Column("SUPERVISORID")]
        public string SupervisorId { get; set; }
        /// <summary>
        /// ����name
        /// </summary>
        /// <returns></returns>
        [Column("SUPERVISORNAME")]
        public string SupervisorName { get; set; }
        /// <summary>
        /// ���ܿ۷�
        /// </summary>
        /// <returns></returns>
        [Column("KPISCORE3")]
        public string KpiScore3 { get; set; }
        /// <summary>
        /// ��Ҫ���Ÿ�ְid
        /// </summary>
        /// <returns></returns>
        [Column("SECONDARYID2")]
        public string SecondaryId2 { get; set; }
        /// <summary>
        /// ��Ҫ���Ÿ�ְname
        /// </summary>
        /// <returns></returns>
        [Column("SECONDARYNAME2")]
        public string SecondaryName2 { get; set; }
        /// <summary>
        /// ��Ҫ���Ÿ�ְ�۷�
        /// </summary>
        /// <returns></returns>
        [Column("KPISCORE4")]
        public string KpiScore4 { get; set; }
        /// <summary>
        /// ��Ҫ������ְid
        /// </summary>
        /// <returns></returns>
        [Column("SECONDARYID1")]
        public string SecondaryId1 { get; set; }
        /// <summary>
        /// ��Ҫ������ְname
        /// </summary>
        /// <returns></returns>
        [Column("SECONDARYNAME1")]
        public string SecondaryName1 { get; set; }
        /// <summary>
        /// ��Ҫ������ְ�۷�
        /// </summary>
        /// <returns></returns>
        [Column("KPISCORE5")]
        public string KpiScore5 { get; set; }
        /// <summary>
        /// ר����ֵ�ฺ����id
        /// </summary>
        /// <returns></returns>
        [Column("ONDUTYUSERID")]
        public string OnDutyUserId { get; set; }
        /// <summary>
        /// ר����ֵ�ฺ����name
        /// </summary>
        /// <returns></returns>
        [Column("ONDUTYUSERNAME")]
        public string OnDutyUserName { get; set; }
        /// <summary>
        /// ר����ֵ�ฺ���˿۷�
        /// </summary>
        /// <returns></returns>
        [Column("KPISCORE6")]
        public string KpiScore6 { get; set; }
        /// <summary>
        /// ���ι���ʦid
        /// </summary>
        /// <returns></returns>
        [Column("DIRECOTORID")]
        public string DirecotorId { get; set; }
        /// <summary>
        /// ���ι���ʦname
        /// </summary>
        /// <returns></returns>
        [Column("DIRECOTORNAME")]
        public string DirecotorName { get; set; }
        /// <summary>
        /// ���ι���ʦ�۷�
        /// </summary>
        /// <returns></returns>
        [Column("KPISCORE7")]
        public string KpiScore7 { get; set; }
        /// <summary>
        /// ��Ҫ���Ÿ�ְid
        /// </summary>
        /// <returns></returns>
        [Column("SECONDARYID3")]
        public string SecondaryId3 { get; set; }
        /// <summary>
        /// ��Ҫ���Ÿ�ְname
        /// </summary>
        /// <returns></returns>
        [Column("SECONDARYNAME3")]
        public string SecondaryName3 { get; set; }
        /// <summary>
        /// ��Ҫ���Ÿ�ְ�۷�
        /// </summary>
        /// <returns></returns>
        [Column("KPISCORE8")]
        public string KpiScore8 { get; set; }
        /// <summary>
        /// ��Ҫ������ְid
        /// </summary>
        /// <returns></returns>
        [Column("SECONDARYID4")]
        public string SecondaryId4 { get; set; }
        /// <summary>
        /// ��Ҫ������ְname
        /// </summary>
        /// <returns></returns>
        [Column("SECONDARYNAME4")]
        public string SecondaryName4 { get; set; }
        /// <summary>
        /// ��Ҫ������ְ�۷�
        /// </summary>
        /// <returns></returns>
        [Column("KPISCORE9")]
        public string KpiScore9 { get; set; }

        /// <summary>
        /// һ���ϰ���Ҫ������Name
        /// </summary>
        [Column("PRINCIPALNAME")]
        public string PrincipalName { get; set; }
        /// <summary>
        /// һ���ϰ���Ҫ������id
        /// </summary>
        [Column("PRINCIPALID")]
        public string PrincipalId { get; set; }

        /// <summary>
        /// һ���ϰ���Ҫ������Name
        /// </summary>
        [Column("OTHERPRINCIPALNAME")]
        public string OtherPrincipalName { get; set; }

        /// <summary>
        /// һ���ϰ���Ҫ������id
        /// </summary>
        [Column("OTHERPRINCIPALID")]
        public string OtherPrincipalId { get; set; }

        /// <summary>
        /// ���β����쵼Name
        /// </summary>
        [Column("DEPTLEADNAME")]
        public string DeptLeadName { get; set; }

        /// <summary>
        /// ���β����쵼Id
        /// </summary>
        [Column("DEPTLEADID")]
        public string DeptLeadId { get; set; }




        /// <summary>
        /// ���β��ŷֹܸ�ְName
        /// </summary>
        [Column("FGDEPTLEADNAME")]
        public string FgDeptLeadName { get; set; }



        /// <summary>
        /// ���β��ŷֹܸ�ְId
        /// </summary>
        [Column("FGDEPTLEADID")]
        public string FgDeptLeadId { get; set; }



        /// <summary>
        /// ��Ŀ��˾�ֹܸ�ְName
        /// </summary>
        [Column("FGLEADFZNAME")]
        public string FgLeadFzName { get; set; }


        /// <summary>
        /// ��Ŀ��˾�ֹܸ�ְId
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

        #region ��չ����
        /// <summary>
        /// ��������
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
        /// �༭����
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