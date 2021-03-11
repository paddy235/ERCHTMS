using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using BSFramework.Data;
using BSFramework.Util;
namespace ERCHTMS.Service.OutsourcingProject
{
    /// <summary>
    /// 描 述：安全信用评价分数表
    /// </summary>
    public class SafetyCreditScoreService : RepositoryFactory<SafetyCreditScoreEntity>, SafetyCreditScoreIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SafetyCreditScoreEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public DataTable GetList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SafetyCreditScoreEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, SafetyCreditScoreEntity entity)
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


        /// <summary>
        /// 提交分数算总分
        /// </summary>
        /// <param name="keyValue"></param>
        public void SaveScoreTotal(string keyValue)
        {
            int number = DbFactory.Base().ExecuteBySql(string.Format(" begin update Epg_Safetycreditevaluate set ACTUALSCORE =(select nvl(sum(score),100) from (select case when scoretype =0 then  100 +sum(Score) when scoretype =1 then -sum(Score)  end Score from EPG_SAFETYCREDITSCORE where SAFETYCREDITEVALUATEID = '{0}' group by scoretype)) where ID = '{0}';  update EPG_SAFETYCREDITSCORE set ISVALID = 1 where SAFETYCREDITEVALUATEID = '{0}';  end;", keyValue));
        }
        #endregion
    }
}