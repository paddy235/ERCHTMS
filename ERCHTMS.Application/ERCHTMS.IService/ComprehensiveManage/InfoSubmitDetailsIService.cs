using ERCHTMS.Entity.ComprehensiveManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.ComprehensiveManage
{
    /// <summary>
    /// 描 述：信息报送详情
    /// </summary>
    public interface InfoSubmitDetailsIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<InfoSubmitDetailsEntity> GetList(string queryJson);
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        InfoSubmitDetailsEntity GetEntity(string keyValue);
        /// <summary>
        /// 更新变更状态
        /// </summary>
        /// <param name="applyId"></param>
        void UpdateChangedData(string infoId);
        #endregion


        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue);
        void RemoveFormByInfoId(string keyValue);        
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveForm(string keyValue, InfoSubmitDetailsEntity entity);
        #endregion
    }
}
