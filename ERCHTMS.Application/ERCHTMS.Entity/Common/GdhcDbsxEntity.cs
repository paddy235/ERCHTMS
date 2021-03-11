using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.Common
{
    /// <summary>
    /// 国电汉川对接待办事项实体
    /// </summary>
    public class GdhcDbsxEntity
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public string syscode { get; set; }

        /// <summary>
        /// 流程节点Id
        /// </summary>
        public string flowid { get; set; }

        /// <summary>
        /// 申请名称
        /// </summary>
        public string requestname { get; set; }

        /// <summary>
        /// 待办名称(需要对接人员确认好，不同待办模块有不同的编码)  人员转岗：RYZG 
        /// </summary>
        public string workflowname { get; set; }

        /// <summary>
        /// 流程节点名称
        /// </summary>
        public string nodename { get; set; }

        /// <summary>
        /// 待办跳转地址
        /// </summary>
        public string pcurl { get; set; }

        /// <summary>
        /// appurl
        /// </summary>
        public string appurl { get; set; }

        /// <summary>
        /// 创建人集团编号
        /// </summary>
        public string creator { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string createdatetime { get; set; }

        /// <summary>
        /// 接收人集团编号
        /// </summary>
        public string receiver { get; set; }

        /// <summary>
        /// 接收时间
        /// </summary>
        public string receivedatetime { get; set; }

        /// <summary>
        /// 待办紧急程度 
        /// </summary>
        public string requestlevel { get; set; }

        /// <summary>
        /// 国电汉川待办平台服务器地址
        /// </summary>
        public string GdhcUrl { get; set; }

        /// <summary>
        /// 双控系统logs文件物理路径
        /// </summary>
        public string LogUrl { get; set; }
    }

}
