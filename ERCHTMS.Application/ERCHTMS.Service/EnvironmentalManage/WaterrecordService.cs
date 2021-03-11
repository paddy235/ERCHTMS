using System;
using ERCHTMS.Entity.EnvironmentalManage;
using ERCHTMS.IService.EnvironmentalManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BSFramework.Util;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.EnvironmentalManage
{
    /// <summary>
    /// 描 述：水质分析记录
    /// </summary>
    public class WaterrecordService : RepositoryFactory<WaterrecordEntity>, WaterrecordIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<WaterrecordEntity> GetList(string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            StringBuilder strSql = new StringBuilder();
            //查询条件
            if (!queryParam["sampletype"].IsEmpty())
            {
                string id = queryParam["sampletype"].ToString();
                strSql.Append(String.Format("select projectcode,kpitarget from bis_waterrecord where sampletype ='{0}'", id));
                return this.BaseRepository().FindList(strSql.ToString());
            }   

            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public WaterrecordEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, WaterrecordEntity entity)
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
