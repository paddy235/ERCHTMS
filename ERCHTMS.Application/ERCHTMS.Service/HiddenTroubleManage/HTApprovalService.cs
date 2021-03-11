using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.IService.HiddenTroubleManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace ERCHTMS.Service.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：隐患评估信息表
    /// </summary>
    public class HTApprovalService : RepositoryFactory<HTApprovalEntity>, HTApprovalIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<HTApprovalEntity> GetList(string queryJson)
        {
            var  list = this.BaseRepository().IQueryable().ToList();
            if (!string.IsNullOrEmpty(queryJson))
            {
                list = list.Where(p => p.HIDCODE == queryJson).OrderByDescending(p => p.AUTOID).ToList();
            }
            return list;
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public HTApprovalEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }


        /// <summary>
        /// 评估信息
        /// </summary>
        /// <param name="hidCode"></param>
        /// <returns></returns>
        public HTApprovalEntity GetEntityByHidCode(string hidCode)
        {
            string sql = string.Format(@"select * from  bis_htapproval where hidcode ='{0}' order by autoid desc", hidCode);
            return this.BaseRepository().FindList(sql).FirstOrDefault();
        }
        
        /// <summary>
        /// 获取历史的所有评估信息
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HTApprovalEntity> GetHistoryList(string hidCode)
        {
            var list = this.BaseRepository().IQueryable().Where(p => p.HIDCODE == hidCode).OrderByDescending(p => p.AUTOID).ToList();
            return list;
        }


        /// <summary>
        /// 评估信息
        /// </summary>
        /// <param name="hidCode"></param>
        /// <returns></returns>
        public DataTable  GetDataTableByHidCode(string hidCode) 
        {
            string sql = string.Format(@"select b.* from  bis_htapproval  a 
                                        left join base_user b on a.approvalperson = b.userid
                                        where a.hidcode ='{0}' order by a.autoid desc", hidCode);
            return this.BaseRepository().FindTable(sql);
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
            string sql = string.Format(@" delete bis_htapproval where hidcode ='{0}' ", hidcode);
            this.BaseRepository().ExecuteBySql(sql);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, HTApprovalEntity entity)
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
