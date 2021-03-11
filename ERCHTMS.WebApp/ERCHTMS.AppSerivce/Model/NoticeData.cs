using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERCHTMS.AppSerivce.Model
{
    public class NoticeData
    {
        public string Id { get; set; }
 
        /// <summary>
        /// 标题
        /// </summary>
        /// <returns></returns>
        public string Title { get; set; }
        /// <summary>
        /// 发布人
        /// </summary>
        /// <returns></returns>
        public string Publisher { get; set; }
        /// <summary>
        /// 发布人ID
        /// </summary>
        /// <returns></returns>
        public string PublisherId { get; set; }
        /// <summary>
        /// 发布部门
        /// </summary>
        /// <returns></returns>
        public string PublisherDept { get; set; }
        /// <summary>
        /// 发布部门ID
        /// </summary>
        /// <returns></returns>
        public string PublisherDeptId { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        /// <returns></returns>
        public DateTime? ReleaseTime { get; set; }
        /// <summary>
        /// 是否重要
        /// </summary>
        /// <returns></returns>
        public string IsImportant { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        /// <returns></returns>
        public string Content { get; set; }
        /// <summary>
        /// 是否发送(0是，1否)
        /// </summary>
        /// <returns></returns>
        public string IsSend { get; set; }
        /// <summary>
        /// 已读人员
        /// </summary>
        /// <returns></returns>
        public string UserId { get; set; }
        /// <summary>
        /// 已读人员
        /// </summary>
        /// <returns></returns>
        public string UserName { get; set; }

        /// <summary>
        /// 发布范围
        /// </summary>
        /// <returns></returns>
        public string IssueRangeDept { get; set; }

        /// <summary>
        /// 发布范围
        /// </summary>
        /// <returns></returns>
        public string IssueRangeDeptCode { get; set; }

        /// <summary>
        /// 发布范围
        /// </summary>
        /// <returns></returns>
        public string IssueRangeDeptName { get; set; }
        /// <summary>
        /// 文件
        /// </summary>
        public List<Photo> file { get; set; }
       /// <summary>
       /// 未读人员
       /// </summary>
        public string NotReadUser { get; set; }
        /// <summary>
        /// 已读人员
        /// </summary>
        public string ReadUser { get; set; }
    }
}