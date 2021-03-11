using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.IService.HiddenTroubleManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;

namespace ERCHTMS.Service.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：隐患验收信息表
    /// </summary>
    public class HTAcceptInfoService : RepositoryFactory<HTAcceptInfoEntity>, HTAcceptInfoIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<HTAcceptInfoEntity> GetList(string queryJson)
        {
            var list = this.BaseRepository().IQueryable().ToList();
            if (!string.IsNullOrEmpty(queryJson))
            {
                list = list.Where(p => p.HIDCODE == queryJson).OrderByDescending(p => p.AUTOID).ToList();
            }
            return list;
        }

        /// <summary>
        /// 获取历史的所有验收信息
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HTAcceptInfoEntity> GetHistoryList(string hidCode)
        {
            var list = this.BaseRepository().IQueryable().Where(p => p.HIDCODE == hidCode).OrderByDescending(p => p.AUTOID).ToList();
            list = list.Where(p => p.ACCEPTSTATUS == "1" ||  p.ACCEPTSTATUS =="0").ToList();
            return list;
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public HTAcceptInfoEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="hidCode"></param>
        /// <returns></returns>
        public HTAcceptInfoEntity GetEntityByHidCode(string hidCode)
        {
            string sql = string.Format(@"select * from bis_htacceptinfo where hidcode ='{0}' order by autoid desc", hidCode);
            return this.BaseRepository().FindList(sql).FirstOrDefault();
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
        public void SaveForm(string keyValue, HTAcceptInfoEntity entity)
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


        public void RemoveFormByCode(string hidcode)
        {
            string sql = string.Format(@" delete bis_htacceptinfo where hidcode ='{0}' ", hidcode);
            this.BaseRepository().ExecuteBySql(sql);
        }
    }
}
