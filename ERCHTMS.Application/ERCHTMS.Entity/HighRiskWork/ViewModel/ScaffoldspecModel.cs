using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.HighRiskWork.ViewModel
{
    public class ScaffoldspecModel
    {
        #region 实体成员
        /// <summary>
        /// </summary>
        /// <returns></returns>
        public string Id { get; set; }
        /// <summary>
        /// 宽
        /// </summary>
        /// <returns></returns>
        public int? SWidth { get; set; }

        /// <summary>
        /// 高
        /// </summary>
        /// <returns></returns>
        public int? SHigh { get; set; }
        /// <summary>
        /// 架体形式
        /// </summary>
        /// <returns></returns>
        public string SFrameName { get; set; }

        /// <summary>
        /// 长
        /// </summary>
        /// <returns></returns>
        public int? SLength { get; set; }
        /// <summary>
        /// 脚手架信息ID
        /// </summary>
        /// <returns></returns>
        public string ScaffoldId { get; set; }

        /// <summary>
        /// 架体形式ID
        /// </summary>
        /// <returns></returns>
        public int? SFrameId { get; set; }

        #endregion

    }
}
