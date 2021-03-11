using ERCHTMS.Entity.HighRiskWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.IService.HighRiskWork
{
   public interface HighRiskApplyMBXXIService
    {
        #region [获取数据]
        /// <summary>
        /// 获取盲板信息
        /// </summary>
        /// <param name="highRiskId">高风险作业id</param>
        /// <returns></returns>
        List<HighRiskApplyMBXXEntity> GetList(string highRiskId);
        #endregion

        #region [提交数据]
        void SafeForm(List<HighRiskApplyMBXXEntity> list);
        #endregion

    }
}
