using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.IService.RiskDatabase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSFramework.Util;
using BSFramework.Util.Extension;
namespace ERCHTMS.Service.RiskDatabase
{
    public class HtStandardItemService : RepositoryFactory<HtStandardItemEntity>, HtStandardItemIService
    {
        #region 获取数据
        /// <summary>
        /// 根据关联词查找排查标准明细项
        /// </summary>
        /// <param name="relWord">关联词</param>
        /// <param name="orgCode">用户所在机构编码</param>
        /// <returns></returns>
        public DataTable GetList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();
            var relWord = queryParam["equipmentname"].ToString();
            var orgCode = queryParam["orgcode"].ToString();
            pagination.conditionJson += string.Format(" and stid in(select id from BIS_HTSTANDARD where name ='{0}' and (createuserorgcode='00' or createuserorgcode='{1}'))", relWord, orgCode);

            DataTable dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);

            return dt;
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();
            if (!queryParam["Content"].IsEmpty())
            {
                string content = queryParam["Content"].ToString();
                pagination.conditionJson += string.Format(" and content like '%{0}%'", content);
            }  
            DataTable dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            return dt;
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<HtStandardItemEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }

        public List<HtStandardItemEntity> GetItemList(string recId)
        {
            return this.BaseRepository().IQueryable(p=>p.StId == recId).OrderBy(p=>p.CreateDate).ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public HtStandardItemEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, HtStandardItemEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }
        #endregion
    }
}
