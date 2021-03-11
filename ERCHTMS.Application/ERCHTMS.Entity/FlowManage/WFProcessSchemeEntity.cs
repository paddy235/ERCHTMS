﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.FlowManage
{
    /// <summary>
    /// 日 期：2016.03.18 09:58
    /// 描 述：工作流实例模板表
    /// </summary>
    [Table("WF_PROCESSSCHEME")]
    public class WFProcessSchemeEntity : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 主键
        /// </summary>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 模板内容
        /// </summary>
        [Column("SCHEMECONTENT")]
        public string SchemeContent { get; set; }
        /// <summary>
        /// 工作流模板信息Id
        /// </summary>
        [Column("WFSCHEMEINFOID")]
        public string WFSchemeInfoId { get; set; }
        /// <summary>
        /// 模板版本
        /// </summary>
        [Column("SCHEMEVERSION")]
        public string SchemeVersion { get; set; }
        /// <summary>
        /// 类型(0.正常,1.草稿)
        /// </summary>
        [Column("PROCESSTYPE")]
        public int? ProcessType { get; set; }
        #endregion
        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.Id = keyValue;
        }
        #endregion
    }
}
