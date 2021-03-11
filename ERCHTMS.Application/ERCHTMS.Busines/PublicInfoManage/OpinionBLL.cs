using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.IService.PublicInfoManage;
using ERCHTMS.Service.PublicInfoManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System;
using System.Collections.Generic;

namespace ERCHTMS.Busines.PublicInfoManage
{
    /// <summary>
    /// 描 述：意见反馈
    /// </summary>
    public class OpinionBLL
    {
        private IOpinionService service = new OpinionService();

        #region 获取数据
        /// <summary>
        /// 意见反馈列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<OpinionEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// 意见反馈实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public OpinionEntity GetEntity(string keyValue)
        {
            OpinionEntity newsEntity = service.GetEntity(keyValue);
            //newsEntity.NewsContent = WebHelper.HtmlDecode(newsEntity.NewsContent);
            return newsEntity;
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除意见反馈
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
        /// 保存意见反馈表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="newsEntity">意见反馈实体</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, OpinionEntity opinionEntity)
        {
            try
            {
                service.SaveForm(keyValue, opinionEntity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
