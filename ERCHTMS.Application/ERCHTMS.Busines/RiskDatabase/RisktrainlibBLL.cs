using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.IService.RiskDatabase;
using ERCHTMS.Service.RiskDatabase;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.RiskDatabase
{
    /// <summary>
    /// 描 述：风险预知训练库
    /// </summary>
    public class RisktrainlibBLL
    {
        private RisktrainlibIService service = new RisktrainlibService();

        #region 获取数据


        public DataTable GetPageListJson(Pagination pagination, string queryJson) {
            return service.GetPageListJson(pagination,queryJson);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<RisktrainlibEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public RisktrainlibEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 获取作业安全分析库
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public DataTable GetRisktrainlibList(string p)
        {
            return service.GetRisktrainlibList(p);
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
        /// 删除来源风险库数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public bool DelRiskData()
        {
            try
            {
                return service.DelRiskData();
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
        public void SaveForm(string keyValue, RisktrainlibEntity entity, List<RisktrainlibdetailEntity> listMesures)
        {
            try
            {
                service.SaveForm(keyValue, entity, listMesures);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertRiskTrainLib(List<RisktrainlibEntity> RiskLib) {
            try
            {
                service.InsertRiskTrainLib(RiskLib);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void InsertImportData(List<RisktrainlibEntity> RiskLib, List<RisktrainlibdetailEntity> detailLib)
        {
            try
            {
                service.InsertImportData(RiskLib, detailLib);
            }
            catch (Exception)
            {
                throw;
            }
        }
        

        #endregion

    }
}
