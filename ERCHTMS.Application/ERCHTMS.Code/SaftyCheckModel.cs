using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Code
{
    public class SaftyCheckModel
    {
        /// <summary>
        /// 所属区域
        /// </summary>
        /// <returns></returns>
        public string BelongDistrict { get; set; }
        /// <summary>
        /// 所属区域主键
        /// </summary>
        /// <returns></returns>
        public string BelongDistrictID { get; set; }
        /// <summary>
        /// 所属部门
        /// </summary>
        /// <returns></returns>
        public string BelongDept { get; set; }
        /// <summary>
        /// 所属部门主键
        /// </summary>
        /// <returns></returns>
        public string BelongDeptID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 修改日期
        /// </summary>		
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 修改用户主键
        /// </summary>		
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 修改用户
        /// </summary>		
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string CreateUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 检查内容
        /// </summary>
        /// <returns></returns>
        public string CheckContent { get; set; }
        /// <summary>
        /// 风险点名称
        /// </summary>
        /// <returns></returns>
        public string RiskName { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        public string ID { get; set; }
        /// <summary>
        /// 关联主表主键
        /// </summary>
        /// <returns></returns>
        public string RecID { get; set; }
        /// <summary>
        /// 所属隐患数量
        /// </summary>
        /// <returns></returns>
        public int Count { get; set; }

        /// <summary>
        /// 所属隐患数量
        /// </summary>
        /// <returns></returns>
        public string CountID { get; set; }

        /// <summary>
        /// 检查状态
        /// </summary>
        /// <returns></returns>
        public int CheckState { get; set; }
        /// <summary>
        /// 检查人
        /// </summary>
        /// <returns></returns>
        public string CheckMan { get; set; }
        /// <summary>
        /// 检查人主键
        /// </summary>
        /// <returns></returns>
        public string CheckManID { get; set; }
        /// <summary>
        /// 所属区域主键
        /// </summary>
        /// <returns></returns>
        public string BelongDistrictCode { get; set; }
        /// <summary>
        /// 检查表ID
        /// </summary>
        /// <returns></returns>
        public string CheckDataId { get; set; }
        /// <summary>
        /// 检查对象
        /// </summary>
        /// <returns></returns>
        public string CheckObject { get; set; }
        /// <summary>
        /// 检查对象ID
        /// </summary>
        /// <returns></returns>
        public string CheckObjectId { get; set; }
        /// <summary>
        /// 检查对象类型 0为设备 1为危险源
        /// </summary>
        /// <returns></returns>
        public string CheckObjectType { get; set; }
    }
}
