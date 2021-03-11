using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.OccupationalHealthManage
{
    /// <summary>
    /// 绑定combox用
    /// </summary>
    public class ComboxEntity
    {
        /// <summary>
        /// 下拉的值
        /// </summary>
        public string itemValue { get; set; }
        /// <summary>
        /// 下拉显示的名称
        /// </summary>
        public string itemName { get; set; }
    }

    public class ComboxsEntity
    {
        /// <summary>
        /// 下拉的值
        /// </summary>
        public string itemValue { get; set; }
        /// <summary>
        /// 下拉显示的名称
        /// </summary>
        public string itemName { get; set; }

        /// <summary>
        /// key值
        /// </summary>
        public string Key { get; set; }
    }
}
