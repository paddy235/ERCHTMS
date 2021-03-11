using ERCHTMS.Entity.StandardSystem;
using ERCHTMS.IService.StandardSystem;
using ERCHTMS.Service.StandardSystem;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.StandardSystem
{
    /// <summary>
    /// 描 述：违章标准表
    /// </summary>
    public class StandardCheckBLL
    {
        private StandardCheckIService service = new StandardCheckService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<StandardCheckEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 根据申请编辑获取最近的审核记录
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public StandardCheckEntity GetLastEntityByRecId(string keyValue,string checkType)
        {
            return service.GetLastEntityByRecId(keyValue,checkType);
        }
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetList(Pagination pagination, string queryJson)
        {
            return service.GetList(pagination, queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public StandardCheckEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 会签是否完成
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="checkUserId"></param>
        /// <returns></returns>
        public bool FinishSign(string keyValue,string checkUserId)
        {
            return service.FinishSign(keyValue,checkUserId);
        }
        /// <summary>
        /// 分委会审核是否完成
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="checkUserId"></param>
        /// <returns></returns>
        public bool FinishCommittee(string keyValue, string checkUserId)
        {
            return service.FinishCommittee(keyValue, checkUserId);
        }
        /// <summary>
        /// 是否全部完成审核
        /// </summary>
        /// <returns></returns>
        public bool FinishComplete(string checkUserId,string checkUserName, string checkType)
        {
            return service.FinishComplete(checkUserId, checkUserName,checkType);
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
        public void SaveForm(string keyValue, StandardCheckEntity entity)
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
    }
}
