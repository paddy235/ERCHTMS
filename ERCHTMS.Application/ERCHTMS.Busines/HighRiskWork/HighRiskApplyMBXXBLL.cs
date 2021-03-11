using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using ERCHTMS.Service.HighRiskWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Busines.HighRiskWork
{
    public class HighRiskApplyMBXXBLL
    {
        private HighRiskApplyMBXXIService service = new HighRiskApplyMBXXService();

        #region [获取数据]
        /// <summary>
        /// 获取盲板信息
        /// </summary>
        /// <param name="highRiskId">高风险作业id</param>
        /// <returns></returns>
        public List<HighRiskApplyMBXXEntity> GetList(string highRiskId)
        {
            return service.GetList(highRiskId);
        }
        #endregion

        #region [提交数据]
        public void SafeForm(List<HighRiskApplyMBXXEntity> list)
        {
            service.SafeForm(list);
        }
        #endregion
    }
}
