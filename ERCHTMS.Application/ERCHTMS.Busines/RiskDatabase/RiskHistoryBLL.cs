using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.IService.RiskDatabase;
using ERCHTMS.Service.RiskDatabase;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Linq;
using BSFramework.Util.Extension;
using System.Data;
using ERCHTMS.Busines.BaseManage;
using System.Text;
using ERCHTMS.Entity.BaseManage;
namespace ERCHTMS.Busines.RiskDatabase
{
    /// <summary>
    /// 描 述：企业风险辨识库
    /// </summary>
    public class RiskHistotyBLL
    {
        private RiskHistotyIService service = new RiskHistoryService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="workId">作业步骤ID</param>
        /// <param name="areaCode">区域编码</param>
        /// <param name="areaId">区域ID</param>
        /// <param name="grade">风险等级</param>
        /// <param name="accType">事故类型</param>
        /// <param name="deptCode">部门编码</param>
        /// <param name="keyWord">查询关键字</param>
        /// <returns>返回列表</returns>
        public IEnumerable<RiskHistoryEntity> GetList(string areaCode, string areaId, string grade, string accType, string deptCode, string keyWord)
        {
            return service.GetList(areaCode, areaId, grade, accType, deptCode, keyWord);
        }
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public RiskHistoryEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        #endregion

        #region 提交数据
        
        /// <summary>
        /// 根据计划Id删除风险历史记录
        /// </summary>
        /// <param name="planId">计划ID</param>
        /// <returns></returns>
        public int Remove(string planId)
        {
            return service.Remove(planId);
        }
        #endregion
    }
}
