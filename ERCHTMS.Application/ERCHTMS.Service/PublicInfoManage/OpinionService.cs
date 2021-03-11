using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.IService.PublicInfoManage;
using BSFramework.Data.Repository;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.WebControl;
using System.Collections.Generic;

namespace ERCHTMS.Service.PublicInfoManage
{
    /// <summary>
    /// 描 述：意见反馈
    /// </summary>
    public class OpinionService : RepositoryFactory<OpinionEntity>, IOpinionService
    {
        #region 获取数据
        /// <summary>
        /// 意见反馈列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<OpinionEntity> GetPageList(Pagination pagination, string queryJson)
        {
            var expression = LinqExtensions.True<OpinionEntity>();
            var queryParam = queryJson.ToJObject();
            if (!queryParam["OpinionContent"].IsEmpty())
            {
                string OpinionContent = queryParam["OpinionContent"].ToString();
                expression = expression.And(t => t.OpinionContent.Contains(OpinionContent));
            }
            //if (!queryParam["OrgCode"].IsEmpty())
            //{
            //    string OrgCode = queryParam["OrgCode"].ToString();
            //    expression = expression.And(t => t.CreateUserOrgCode == OrgCode);
            //}
            return this.BaseRepository().FindList(expression, pagination);
        }
        /// <summary>
        /// 意见反馈实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public OpinionEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除意见反馈
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存意见反馈表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="opinionEntity">意见反馈实体</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, OpinionEntity opinionEntity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                opinionEntity.Modify(keyValue);
                this.BaseRepository().Update(opinionEntity); 
            }
            else
            {
                opinionEntity.Create();
                this.BaseRepository().Insert(opinionEntity);
            }
        }
        #endregion
    }
}
