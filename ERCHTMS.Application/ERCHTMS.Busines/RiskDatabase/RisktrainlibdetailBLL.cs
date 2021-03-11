using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.IService.RiskDatabase;
using ERCHTMS.Service.RiskDatabase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Busines.RiskDatabase
{
    /// <summary>
    /// 描 述：风险预知训练库管控措施
    /// </summary>
    public class RisktrainlibdetailBLL
    {
        private RisktrainlibdetailIService service = new RisktrainlibdetailService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<RisktrainlibdetailEntity> GetList(string workId)
        {
            return service.GetList(workId);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public RisktrainlibdetailEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }


        public DataTable GetTrainLibDetail(string workId)
        {
            return service.GetTrainLibDetail(workId);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, RisktrainlibdetailEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void InsertRiskTrainDetailLib(List<RisktrainlibdetailEntity> detailLib) {
            try
            {
                service.InsertRiskTrainDetailLib(detailLib);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
