using BSFramework.Data.Repository;
using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Service.HighRiskWork
{
    public class HighRiskApplyMBXXService : RepositoryFactory<HighRiskApplyMBXXEntity>, HighRiskApplyMBXXIService
    {
        #region [获取数据]
        /// <summary>
        /// 获取盲板信息
        /// </summary>
        /// <param name="highRiskId">高风险作业id</param>
        /// <returns></returns>
        public List<HighRiskApplyMBXXEntity> GetList(string highRiskId)
        {
            return this.BaseRepository().FindList(string.Format("select * from BIS_HIGHRISKCOMMONAPPLY_MBXX where HIGHRISKCOMMONAPPLYID='{0}'", highRiskId)).ToList();
        }

        #endregion


        #region [提交数据]

        public void SafeForm(List<HighRiskApplyMBXXEntity> list)
        {
            this.BaseRepository().Insert(list);
        }
        #endregion
    }
}
