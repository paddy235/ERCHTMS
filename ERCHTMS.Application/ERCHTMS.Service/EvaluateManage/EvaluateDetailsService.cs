using ERCHTMS.Entity.EvaluateManage;
using ERCHTMS.IService.EvaluateManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using ERCHTMS.Code;
using BSFramework.Data;
using BSFramework.Util;
using System;
using BSFramework.Util.Extension;
using System.Data;
using System.Linq.Expressions;
using ERCHTMS.Entity.StandardSystem;
using System.Text;

namespace ERCHTMS.Service.EvaluateManage
{
    /// <summary>
    /// 描 述：合规性评价明细
    /// </summary>
    public class EvaluateDetailsService : RepositoryFactory<EvaluateDetailsEntity>, EvaluateDetailsIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;
            if (queryJson != null && queryJson != "")
            {
                var queryParam = queryJson.ToJObject();
                //关联的评价计划
                if (!queryParam["MainId"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and MainId='{0}'", queryParam["MainId"].ToString());
                }
                //关联的评价计划
                if (!queryParam["EvaluatePlanId"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and EvaluatePlanId='{0}'", queryParam["EvaluatePlanId"].ToString());
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<EvaluateDetailsEntity> GetList(string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;
            if (queryJson != null && queryJson != "")
            {
               
                var queryParam = queryJson.ToJObject();
                var expression = LinqExtensions.True<EvaluateDetailsEntity>();
                if (!queryParam["IsConform"].IsEmpty())
                {
                    int IsConform = Convert.ToInt32(queryParam["IsConform"]);
                    expression = expression.And(t => t.IsConform == IsConform);
                }
                if (!queryParam["MainId"].IsEmpty())
                {
                    string MainId = queryParam["MainId"].ToString();
                    expression = expression.And(t => t.MainId == MainId);
                }
                if (!queryParam["EvaluatePlanId"].IsEmpty())
                {
                    string EvaluatePlanId = queryParam["EvaluatePlanId"].ToString();
                    expression = expression.And(t => t.EvaluatePlanId == EvaluatePlanId);
                }

                 return this.BaseRepository().IQueryable(expression).OrderBy(x=>x.CreateDate).ToList();

            }
            else
            {
                return this.BaseRepository().IQueryable().OrderBy(x => x.CreateDate).ToList();
            }
        }

        public DataTable GetStCategory()
        {
            //获取需要评价的部门
            DataTable dt = BaseRepository().FindTable("select * from HRS_STCATEGORY t where parentid=(select id from HRS_STCATEGORY where parentid='0' and name like'%法律法规%' and rownum<2)");
            return dt;
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public EvaluateDetailsEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, EvaluateDetailsEntity entity)
        {
            entity.Id = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                EvaluateDetailsEntity ee = this.BaseRepository().FindEntity(keyValue);
                if (ee == null)
                {
                    entity.Create();
                    this.BaseRepository().Insert(entity);
                }
                else
                {
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }
            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }
        #endregion

        /// <summary>
        /// 根据文件名称获取分类名称（取大分类）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string getStCategory(string str)
        {
            try
            {
                Operator curUser = OperatorProvider.Provider.Current();
                DataTable dt = BaseRepository().FindTable(@"select c.name from hrs_stcategory a 
left join hrs_standardsystem b on b.categorycode =a.id 
left join hrs_stcategory c on c.id =a.parentid 
where b.filename='" + str + "'");
                return dt.Rows[0][0].ToString();
            }
            catch
            {
                return "";
            }
        }
    }
}
