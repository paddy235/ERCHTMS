﻿using ERCHTMS.Code;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ERCHTMS.Entity;

namespace ERCHTMS.Entity.AuthorizeManage
{
    /// <summary>
    /// 描 述：系统表单实例
    /// </summary>
    [Table("BASE_MODULEFORMINSTANCE")]
    public class ModuleFormInstanceEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 表单主键
        /// </summary>
        [Column("FORMINSTANCEID")]
        public string FormInstanceId { set; get; }
        /// <summary>
        /// 功能主键
        /// </summary>
        [Column("FORMID")]
        public string FormId { set; get; }
        /// <summary>
        /// 编码
        /// </summary>
        [Column("FORMINSTANCEJSON")]
        public string FormInstanceJson { set; get; }
        /// <summary>
        /// 名称
        /// </summary>
        [Column("OBJECTID")]
        public string ObjectId { set; get; }
        /// <summary>
        /// 排序码
        /// </summary>
        [Column("SORTCODE")]
        public int? SortCode { set; get; }
        /// <summary>
        /// 备注
        /// </summary>
        [Column("DESCRIPTION")]
        public string Description { set; get; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.FormInstanceId = Guid.NewGuid().ToString();
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.FormInstanceId = keyValue;
        }
        #endregion
    }
}


