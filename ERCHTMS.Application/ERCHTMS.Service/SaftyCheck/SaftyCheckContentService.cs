using BSFramework.Data.Repository;
using BSFramework.Util.Extension;
using ERCHTMS.Entity.SaftyCheck;
using ERCHTMS.IService.SaftyCheck;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Service.SaftyCheck
{
    /// <summary>
    /// 描 述：安全检查内容
    /// </summary>
    public class SaftyCheckContentService : RepositoryFactory<SaftyCheckContentEntity>, SaftyCheckContentIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SaftyCheckContentEntity> GetList(string queryJson)
        {
            IEnumerable<SaftyCheckContentEntity> content = this.BaseRepository().IQueryable().ToList();
            return content;
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, SaftyCheckContentEntity entity)
        {
            entity.Create();
            this.BaseRepository().Insert(entity);
        }
        public void Update(string keyValue, SaftyCheckContentEntity entity)
        {
            entity.Modify(keyValue);
            this.BaseRepository().Update(entity);
        }
        /// <summary>
        /// 根据检查项目Id获取检查结果信息
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public SaftyCheckContentEntity Get(string itemId)
        {
            DataTable dt = BaseRepository().FindTable(string.Format("select id from BIS_SAFTYCONTENT where DetailId='{0}'",itemId));
            if(dt.Rows.Count>0)
            {
                return BaseRepository().FindEntity(dt.Rows[0][0].ToString());
            }
            else
            {
                return null;
            }
        }
        public SaftyCheckContentEntity GetEntity(string id)
        {
            return BaseRepository().FindEntity(id);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                this.BaseRepository().ExecuteBySql("delete BIS_SAFTYCONTENT where recid='" + keyValue + "'");
            }
        }
        public int SaveNotice(string keyValue,CheckNoticeEntity sn)
        {
            
            var db=DbFactory.Base().BeginTrans();
            try
            {
                db.ExecuteBySql(string.Format("delete from BIS_CHECKNOTICE where checkid='{0}'", sn.CheckId));
                sn.Id = keyValue;
                sn.Create();
                db.Insert<CheckNoticeEntity>(sn);
                db.Commit();
                return 1;
            }
            catch
            {
                db.Rollback();
                return 0;
            }
            
        }
        #endregion
    }
}
