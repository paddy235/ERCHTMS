using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERCHTMS.AppSerivce.Model
{
    public class WorkTicketEntity
    {
        /// <summary>
        /// 工作票编号
        /// </summary>
        public string WO_SD_NO_P { get; set; }
        /// <summary>
        /// 工作内容
        /// </summary>
        public string WO_SD_DE { get; set; }
        /// <summary>
        /// 票类型
        /// </summary>
        public string WO_SD_TY { get; set; }
        /// <summary>
        /// 票类型描述
        /// </summary>
        public string WO_SD_TY_Desc { get; set; }
        /// <summary>
        /// 图片列表
        /// </summary>
        public List<string> PEimgList { get; set; }
        /// <summary>
        /// 工作地点
        /// </summary>
        public string WO_SD_LO_Desc { get; set; }
        /// <summary>
        /// 工作票状态
        /// </summary>
        public int? WO_SD_ST_NO { get; set; }
        /// <summary>
        /// 实体ID
        /// </summary>
        public string ENTITY_ID { get; set; }
        /// <summary>
        /// NOSA区域
        /// </summary>
        public string WO_SD_AR { get; set; }
        /// <summary>
        /// 状态描述
        /// </summary>
        public string WO_SD_ST_NO_Desc { get; set; }
        /// <summary>
        /// 隔离证编号
        /// </summary>
        public string WO_SD_IC { get; set; }
        /// <summary>
        /// 高风险类型
        /// </summary>
        public string WO_SD_RK_TY { get; set; }
        /// <summary>
        /// 高风险类型描述
        /// </summary>
        public string WO_SD_RK_TY_Desc { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        public string COMPANY_ID { get; set; }
        /// <summary>
        /// 签发人
        /// </summary>
        public string WO_SD_AP_ID { get; set; }
        /// <summary>
        /// 签发人姓名
        /// </summary>
        public string WO_SD_AP_ID_Name { get; set; }
        /// <summary>
        /// 签发时间
        /// </summary>
        public string WO_SD_AP_DT { get; set; }
        /// <summary>
        /// 许可人
        /// </summary>
        public string WO_SD_CS_ID { get; set; }
        /// <summary>
        /// 许可人姓名
        /// </summary>
        public string WO_SD_CS_ID_Name { get; set; }
        /// <summary>
        /// 许可时间
        /// </summary>
        public string WO_SD_CS_DT { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        public string WO_SD_IS_ID { get; set; }
        /// <summary>
        /// 负责人姓名
        /// </summary>
        public string WO_SD_IS_ID_Name { get; set; }
        /// <summary>
        /// 区域负责人
        /// </summary>
        public string WO_SD_AR_ID { get; set; }
        /// <summary>
        /// 区域负责人姓名
        /// </summary>
        public string WO_SD_AR_ID_Name { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string WO_SD_TEL { get; set; }
        /// <summary>
        /// 设备编码
        /// </summary>
        public string WO_SD_EQ { get; set; }
        /// <summary>
        /// 设备描述
        /// </summary>
        public string WO_SD_EQ_Desc { get; set; }
        /// <summary>
        /// 计划开始时间
        /// </summary>
        public string WO_SD_VF_DT { get; set; }
        /// <summary>
        /// 计划结束时间
        /// </summary>
        public string WO_SD_VT_DT { get; set; }
        /// <summary>
        /// 检修专业
        /// </summary>
        public string SA_WO_ZY { get; set; }
        /// <summary>
        /// 检修专业描述
        /// </summary>
        public string SA_WO_ZY_Desc { get; set; }
        /// <summary>
        /// 安健环区域
        /// </summary>
        public string WO_SD_AR_LO { get; set; }
        /// <summary>
        /// 安健环区域描述
        /// </summary>
        public string WO_SD_AR_LO_Desc { get; set; }
        /// <summary>
        /// 安健环区域编码
        /// </summary>
        public string EQ_LO_USERDEF3 { get; set; }
        /// <summary>
        /// 运行专业
        /// </summary>
        public string WO_SD_YX_ZY { get; set; }
        /// <summary>
        /// 动火执行人
        /// </summary>
        public string WO_SD_EX_NM { get; set; }
        /// <summary>
        ///     动火执行人姓名
        /// </summary>
        public string WO_SD_EX_NM_Name { get; set; }
        /// <summary>
        /// 班组
        /// </summary>
        public string JOBTEAM { get; set; }
        /// <summary>
        /// 班组成员
        /// </summary>
        public string JOBMEMBER { get; set; }
        /// <summary>
        /// 工作结束时间
        /// </summary>
        public string WO_SD_CR_DT { get; set; }

        /// <summary>
        /// 风险评估
        /// </summary>
        public string LK_NO_P { get; set; }
        /// <summary>
        /// 风险等级
        /// </summary>
        public string RA_RA_FX_DJ { get; set; }

    }
}