using ERCHTMS.Entity.QuestionManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.QuestionManage
{
    /// <summary>
    /// 描 述：问题基本信息表
    /// </summary>
    public interface QuestionInfoIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<QuestionInfoEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        QuestionInfoEntity GetEntity(string keyValue);

        
        #region 问题检查集合

        /// <summary>
        /// 问题检查集合
        /// </summary>
        /// <param name="checkId"></param>
        /// <param name="checkman"></param>
        /// <returns></returns>
         DataTable GeQuestiontOfCheckList(string checkId, string checkman, string flowstate);
        #endregion
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
        void SaveForm(string keyValue, QuestionInfoEntity entity);
        #endregion




        #region 问题集合

        /// <summary>
        /// 问题集合
        /// </summary>
        /// <param name="relevanceId"></param>
        /// <returns></returns>
        DataTable GeQuestiontByRelevanceId(string relevanceId, string flowstate = "问题登记");
        #endregion


        /// <summary>
        /// 获取问题详情
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        DataTable GetQuestionModel(string keyValue);
        
        #region 通过问题编号，来判断是否存在重复现象
        /// <summary>
        /// 通过问题编号，来判断是否存在重复现象
        /// </summary>
        /// <param name="QuestionNumber"></param>
        /// <returns></returns>
        IList<QuestionInfoEntity> GetListByNumber(string QuestionNumber);
        #endregion

        /// <summary>
        /// 获取台账数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetQuestionBaseInfo(Pagination pagination, string queryJson);

        string GenerateCode(string tablename, string maxfields, int seriallen);
    }
}