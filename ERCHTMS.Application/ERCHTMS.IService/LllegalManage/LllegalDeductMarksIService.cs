using ERCHTMS.Entity.LllegalManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.LllegalManage
{
    /// <summary>
    /// 描 述：违章档案扣分表
    /// </summary>
    public interface LllegalDeductMarksIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<LllegalDeductMarksEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        LllegalDeductMarksEntity GetEntity(string keyValue);

        /// <summary>
        /// 获取单个实例
        /// </summary>
        /// <param name="punishid"></param>
        /// <returns></returns>
        LllegalDeductMarksEntity GetLllegalRecordEntity(string punishid);

        List<LllegalDeductMarksEntity> GetLllegalRecorList(string punishdate, string userid, string describe, string deptid = "", string teamid = "");
        #endregion

        #region 提交数据
        /// <summary>
                /// 删除数据
                /// </summary>
                /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveForm(string keyValue, LllegalDeductMarksEntity entity);
        #endregion

        #region  违章基础信息查询
        /// <summary>
        /// 违章基础信息查询    
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        DataTable GetLllegalRecordInfo(Pagination pagination, string queryJson);
        #endregion

        #region  违章得分信息查询
        /// <summary>
        /// 违章得分信息查询    
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        DataTable GetLllegalPointInfo(Pagination pagination, string queryJson);
        #endregion

    }
}