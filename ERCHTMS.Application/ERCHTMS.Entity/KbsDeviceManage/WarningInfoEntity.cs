using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.KbsDeviceManage
{
    /// <summary>
    /// 描述：预警信息
    /// </summary>
    [Table("bis_EarlyWarning")]
    public class WarningInfoEntity : BaseEntity
    {
        /// <summary>
        /// 创建人所属机构
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CREATEUSERORGCODE { get; set; }
        /// <summary>
        /// 创建人所属部门
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CREATEUSERDEPTCODE { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }
        /// <summary>
        /// 操作用户
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string MODIFYUSERID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CREATEDATE { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CREATEUSERID { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }

        /// <summary>
        /// 作业名称
        /// </summary>
        /// <returns></returns>
        [Column("TASKNAME")]
        public string TaskName { get; set; }

        /// <summary>
        /// 预警内容
        /// </summary>
        /// <returns></returns>
        [Column("WARNINGCONTENT")]
        public string WarningContent { get; set; }
        /// <summary>
        /// 责任人
        /// </summary>
        /// <returns></returns>
        [Column("LIABLENAME")]
        public string LiableName { get; set; }
        /// <summary>
        /// 责任人Id
        /// </summary>
        /// <returns></returns>
        [Column("LIABLEID")]
        public string LiableId { get; set; }

        /// <summary>
        /// 预警时间
        /// </summary>
        /// <returns></returns>
        [Column("WARNINGTIME")]
        public DateTime? WarningTime { get; set; }

        /// <summary>
        /// 主表记录Id
        /// </summary>
        /// <returns></returns>
        [Column("BASEID")]
        public string BaseId { get; set; }

        /// <summary>
        /// 区域/部门code
        /// </summary>
        /// <returns></returns>
        [Column("DEPTCODE")]
        public string deptCode { get; set; }
        /// <summary>
        /// 区域/部门名称
        /// </summary>
        /// <returns></returns>
        [Column("DEPTNAME")]
        public string deptName { get; set; }

        /// <summary>
        ///备用
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// 0未读 1已读
        /// </summary>
        [Column("STATE")]
        public int State { get; set; }

        /// <summary>
        /// 预警类型 0现场作业 1人员 2 设备离线预警
        /// </summary>
        /// <returns></returns>
        [Column("TYPE")]
        public int type { get; set; }

        /// <summary>
        /// 通知类型   工作预警     { 0 负责人未到岗 1工作成员未到岗 2 误入隔离区 }
        ///            人员预警     { 0 静止不动     1 SOS求助       2 误入隔离区 }
        ///            设备离线预警 { 0 基站故障     1 摄像头故障    2 标签电量低 }
        /// </summary>
        [Column("NOTICETYPE")]
        public int NoticeType { get; set; }


    }
}
