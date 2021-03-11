using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.Home
{
    /// <summary>
    /// 华升大屏实时工作Model
    /// </summary>
    public class RealTimeWorkModel
    {
        /// <summary>
        /// 数据Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 排序时间
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// 单位名称
        /// </summary>
        public string DeptName { get; set; }
        /// <summary>
        /// 单位Code
        /// </summary>
        public string DetpCode { get; set; }
        /// <summary>
        /// 单位Id
        /// </summary>
        public string DeptId { get; set; }
        /// <summary>
        /// 工作描述
        /// </summary>
        public string WorkDescribe { get; set; }
        /// <summary>
        /// 所属模块名称
        /// </summary>
        public string ModuleType { get; set; }
        /// <summary>
        /// 备用字段
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 处理状态 0：未处理 1：已处理
        /// </summary>
        public int Status { get; set; }
    }
}
