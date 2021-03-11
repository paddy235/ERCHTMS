using System;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.IService.HiddenTroubleManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;

namespace ERCHTMS.Service.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：整改延期对象
    /// </summary>
    public class HTExtensionService : RepositoryFactory<HTExtensionEntity>, HTExtensionIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<HTExtensionEntity> GetList(string hidCode) 
        {
            return this.BaseRepository().IQueryable().Where(p => p.HIDCODE == hidCode).OrderByDescending(p => p.CREATEDATE).ToList(); 
        }

        #region 获取最近的一组申请详情
        /// <summary>
        /// 获取最近的一组申请详情
        /// </summary>
        /// <param name="hidCode"></param>
        /// <returns></returns>
        public IList<HTExtensionEntity> GetListByCondition(string hidCode)
        {
            string sql = string.Format(@"select * from bis_htextension where id in ( select id from (select id,  row_number() over(partition by handleid order by createdate asc ) rn from bis_htextension  where hidcode ='{0}') where rn=1) order by createdate desc", hidCode);
          
            var list = this.BaseRepository().FindList(sql).ToList();

            return list;
        }
        #endregion

        /// <summary>
        /// 获取最新的一个整改申请对象
        /// </summary>
        /// <param name="hidCode"></param>
        /// <returns></returns>
        public HTExtensionEntity GetFirstEntity(string hidCode)  
        {
            return this.BaseRepository().IQueryable().Where(p => p.HIDCODE == hidCode && p.HANDLETYPE =="0" ).OrderByDescending(p => p.HANDLEID).ToList().FirstOrDefault();
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public HTExtensionEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, HTExtensionEntity entity)
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