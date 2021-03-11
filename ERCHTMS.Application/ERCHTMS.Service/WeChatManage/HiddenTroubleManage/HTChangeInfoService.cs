using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.IService.HiddenTroubleManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;

namespace ERCHTMS.Service.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：隐患整改信息表
    /// </summary>
    public class HTChangeInfoService : RepositoryFactory<HTChangeInfoEntity>, HTChangeInfoIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<HTChangeInfoEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }

        /// <summary>
        /// 获取历史的所有整改信息
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HTChangeInfoEntity> GetHistoryList(string hidCode)
        {
            var list = this.BaseRepository().IQueryable().Where(p => p.HIDCODE == hidCode).OrderByDescending(p => p.AUTOID).ToList();
            if (list.Count() > 0)
            {
                list.RemoveAt(0);  //移除第一个
            }
            return list;
        }



        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public HTChangeInfoEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }


        public HTChangeInfoEntity GetEntityByCode(string keyValue)
        {
            return this.BaseRepository().IQueryable().Where(p => p.HIDCODE == keyValue).OrderByDescending(p => p.AUTOID).ToList().FirstOrDefault();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="hidCode"></param>
        /// <returns></returns>
        public HTChangeInfoEntity GetEntityByHidCode(string hidCode)  
        {
            string sql = string.Format(@"select * from bis_htchangeinfo where hidcode ='{0}' order by autoid desc",hidCode);
            return  this.BaseRepository().FindList(sql).FirstOrDefault();
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

        
        public void RemoveFormByCode(string hidcode)
        {
            string sql = string.Format(@" delete bis_htchangeinfo where hidcode ='{0}' ", hidcode);
            this.BaseRepository().ExecuteBySql(sql);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, HTChangeInfoEntity entity)
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
