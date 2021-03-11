using ERCHTMS.Entity.QuestionManage;
using ERCHTMS.IService.QuestionManage;
using ERCHTMS.Service.QuestionManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.QuestionManage
{
    /// <summary>
    /// 描 述：问题基本信息表
    /// </summary>
    public class QuestionInfoBLL
    {
        private QuestionInfoIService service = new QuestionInfoService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<QuestionInfoEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public QuestionInfoEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
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
        public void SaveForm(string keyValue, QuestionInfoEntity entity)
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
        #endregion

        #region 通过问题编号，来判断是否存在重复现象
        /// <summary>
        /// 通过问题编号，来判断是否存在重复现象
        /// </summary>
        /// <param name="QuestionNumber"></param>
        /// <returns></returns>
        public IList<QuestionInfoEntity> GetListByNumber(string QuestionNumber)
        {
            try
            {
                return service.GetListByNumber(QuestionNumber);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion



        #region 问题集合

        /// <summary>
        /// 问题集合
        /// </summary>
        /// <param name="relevanceId"></param>
        /// <returns></returns>
        public DataTable GeQuestiontByRelevanceId(string relevanceId, string flowstate = "问题登记")
        {
            try
            {
                return service.GeQuestiontByRelevanceId(relevanceId, flowstate);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region 问题检查集合

        /// <summary>
        /// 问题检查集合
        /// </summary>
        /// <param name="checkId"></param>
        /// <param name="checkman"></param>
        /// <returns></returns>
        public DataTable GeQuestiontOfCheckList(string checkId, string checkman, string flowstate)
        {
            try
            {
                return service.GeQuestiontOfCheckList(checkId, checkman, flowstate);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 问题实体所有元素对象
        /// <summary>
        /// 问题实体所有元素对象
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public DataTable GetQuestionModel(string keyValue)
        {
            try
            {
                return service.GetQuestionModel(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 获取台账数据
        /// <summary>
        /// 获取台账数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetQuestionBaseInfo(Pagination pagination, string queryJson)
        {
            try
            {
                return service.GetQuestionBaseInfo(pagination, queryJson);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion


        #region 获取新编码
        /// <summary>
        /// 获取新编码
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="maxfields"></param>
        /// <param name="seriallen">4</param>
        /// <returns></returns>
        public string GenerateCode(string tablename, string maxfields, int seriallen)
        {
            try
            {
                return service.GenerateCode(tablename, maxfields, seriallen);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}