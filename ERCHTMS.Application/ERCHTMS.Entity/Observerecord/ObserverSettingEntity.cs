using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ERCHTMS.Entity.Observerecord
{
    /// <summary>
    /// 描 述：参与度指标设置
    /// </summary>
    [Table("HSE_OBSERVERSETTING")]
    public class ObserverSettingEntity : BaseEntity
    {
        [Column("SETTINGID")]
        public string SettingId { get; set; }
        [Column("SETTINGNAME")]
        public string SettingName { get; set; }
        [Column("CYCLE")]
        public string Cycle { get; set; }
        [Column("TIMES")]
        public int Times { get; set; }
        [NotMapped]
        public string DeptName { get; set; }
        [NotMapped]
        public string DeptId { get; set; }
        [NotMapped]
        public List<ObserverSettingItemEntity> Items { get; set; }
    }

    /// <summary>
    /// 描 述：参与度指标设置项
    /// </summary>
    [Table("HSE_OBSERVERSETTINGITEM")]
    public class ObserverSettingItemEntity : BaseEntity
    {
        [Column("ITEMID")]
        public string ItemId { get; set; }
        [Column("SETTINGID")]
        public string SettignId { get; set; }
        [Column("DEPTID")]
        public string DeptId { get; set; }
    }
}