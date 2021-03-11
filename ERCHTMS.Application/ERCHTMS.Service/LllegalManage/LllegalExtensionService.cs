using ERCHTMS.Entity.LllegalManage;
using ERCHTMS.IService.LllegalManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;

namespace ERCHTMS.Service.LllegalManage
{
    /// <summary>
    /// 描 述：违章整改延期信息表
    /// </summary>
    public class LllegalExtensionService : RepositoryFactory<LllegalExtensionEntity>, LllegalExtensionIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<LllegalExtensionEntity> GetList(string queryJson)
        {
            List<LllegalExtensionEntity> list =  this.BaseRepository().IQueryable().ToList();
            if (!string.IsNullOrEmpty(queryJson)) 
            {
                list = list.Where(p => p.LLLEGALID == queryJson).OrderBy(p=>p.CREATEDATE).ToList();
            }
            return list;
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public LllegalExtensionEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion

        #region 获取最近的一组申请详情
        /// <summary>
        /// 获取最近的一组申请详情
        /// </summary>
        /// <param name="hidCode"></param>
        /// <returns></returns>
        public IList<LllegalExtensionEntity> GetListByCondition(string lllegalId)
        {
            string sql = string.Format(@"select * from bis_lllegalextension where id in ( select id from (select id,  row_number() over(partition by handleid order by autoid desc ) rn from bis_lllegalextension  where lllegalid ='{0}') where rn=1) order by createdate desc", lllegalId);

            var list = this.BaseRepository().FindList(sql).ToList();

            return list;
        }
        #endregion

        /// <summary>
        /// 获取最新的一个整改申请对象
        /// </summary>
        /// <param name="hidCode"></param>
        /// <returns></returns>
        public LllegalExtensionEntity GetFirstEntity(string lllegalId)
        {
            return this.BaseRepository().IQueryable().Where(p => p.LLLEGALID == lllegalId && p.HANDLETYPE == "0").OrderByDescending(p => p.HANDLEID).ToList().FirstOrDefault();
        }

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
        public void SaveForm(string keyValue, LllegalExtensionEntity entity)
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