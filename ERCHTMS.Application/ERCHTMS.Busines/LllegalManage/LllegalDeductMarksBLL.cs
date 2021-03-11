using ERCHTMS.Entity.LllegalManage;
using ERCHTMS.IService.LllegalManage;
using ERCHTMS.Service.LllegalManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.LllegalManage
{
    /// <summary>
    /// 描 述：违章档案扣分表
    /// </summary>
    public class LllegalDeductMarksBLL
    {
        private LllegalDeductMarksIService service = new LllegalDeductMarksService();

        #region 获取数据
        /// <summary>
                /// 获取列表
                /// </summary>
                /// <param name="queryJson">查询参数</param>
                /// <returns>返回列表</returns>
        public IEnumerable<LllegalDeductMarksEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public LllegalDeductMarksEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 获取单个实例
        /// </summary>
        /// <param name="punishid"></param>
        /// <returns></returns>
        public LllegalDeductMarksEntity GetLllegalRecordEntity(string punishid)
        {
            return service.GetLllegalRecordEntity(punishid);
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
        public void SaveForm(string keyValue, LllegalDeductMarksEntity entity)
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

        #region  违章基础信息查询
        /// <summary>
        /// 违章基础信息查询    
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetLllegalRecordInfo(Pagination pagination, string queryJson)
        {
            try
            {
                return service.GetLllegalRecordInfo(pagination, queryJson);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region  违章得分信息查询
        /// <summary>
        /// 违章得分信息查询    
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetLllegalPointInfo(Pagination pagination, string queryJson)
        {
            try
            {
                return service.GetLllegalPointInfo(pagination, queryJson);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        public List<LllegalDeductMarksEntity> GetLllegalRecorList(string punishdate, string userid, string describe, string deptid = "", string teamid = "")
        {
            try
            {
                return service.GetLllegalRecorList(punishdate, userid, describe, deptid, teamid);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}