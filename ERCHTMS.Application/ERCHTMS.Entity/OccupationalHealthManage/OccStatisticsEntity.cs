using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.OccupationalHealthManage
{
    /// <summary>
    /// 职业病统计类
    /// </summary>
    public class OccStatisticsEntity
    {
        /// <summary>
        /// 职业病种类
        /// </summary>
        public string Sicktype { get; set; }

        /// <summary>
        /// 患病人员数量
        /// </summary>
        public int SickUserNum { get; set; }

        /// <summary>
        /// 比例
        /// </summary>
        public string Proportion { get; set; }

        /// <summary>
        /// 职业病id
        /// </summary>
        public string SickValue { get; set; }
    }
}
