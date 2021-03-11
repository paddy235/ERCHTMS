using ERCHTMS.Entity.SafetyLawManage;
using ERCHTMS.IService.SafetyLawManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.Data;
using System;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Service.BaseManage;
using ERCHTMS.Code;

namespace ERCHTMS.Service.SafetyLawManage
{
    /// <summary>
    /// 描 述：事故案例库
    /// </summary>
    public class AccidentCaseLawService : RepositoryFactory<AccidentCaseLawEntity>, AccidentCaseLawIService
    {
        DepartmentService DepartmentService = new DepartmentService();
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<AccidentCaseLawEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return this.BaseRepository().FindList(pagination);
        }


        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageDataTable(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var queryParam = queryJson.ToJObject();
            //查询条件
            if (!queryParam["keyword"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and FileName  like '%{0}%'", queryParam["keyword"].ToString());
            }

            //本电厂  本省公司
            if (!queryParam["type"].IsEmpty())
            {
                if (queryParam["type"].ToString() == "0")
                {
                    pagination.conditionJson += string.Format(" and createuserorgcode  = '{0}'", user.OrganizeCode);
                }
                else if (queryParam["type"].ToString() == "1")
                {
                    IEnumerable<DepartmentEntity> orgcodelist = new List<DepartmentEntity>();
                    orgcodelist = DepartmentService.GetList().Where(t => user.NewDeptCode.Contains(t.DeptCode) && t.Nature == "省级");
                    if (orgcodelist.Count() > 0)
                    {
                        pagination.conditionJson += " and (";
                        foreach (DepartmentEntity item in orgcodelist)
                        {
                            pagination.conditionJson += " createuserorgcode ='" + item.EnCode + "' or";
                        }

                        pagination.conditionJson =
                            pagination.conditionJson.Substring(0, pagination.conditionJson.Length - 2);
                        pagination.conditionJson += ")";
                    }
                }
            }

            //事故范围
            if (!queryParam["range"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and AccRange='{0}'", queryParam["range"].ToString());
            }
            //时间选择
            if (!queryParam["st"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and  AccTime>=to_date('{0}','yyyy-mm-dd hh24:mi')", queryParam["st"].ToString().Trim());

            }
            if (!queryParam["et"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and AccTime<=to_date('{0}','yyyy-mm-dd  hh24:mi')", queryParam["et"].ToString().Trim());
            }
            //选中的数据
            if (!queryParam["idsData"].IsEmpty())
            {
                var ids = queryParam["idsData"].ToString();
                string idsarr = "";
                if (ids.Contains(','))
                {
                    string[] array = ids.TrimEnd(',').Split(',');
                    foreach (var item in array)
                    {
                        idsarr += "'" + item + "',";
                    }
                    if (idsarr.Contains(","))
                        idsarr = idsarr.TrimEnd(',');
                }
                pagination.conditionJson += string.Format(" and id in({0})", idsarr);
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<AccidentCaseLawEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public AccidentCaseLawEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, AccidentCaseLawEntity entity)
        {
            bool b = false;
            if (!string.IsNullOrWhiteSpace(keyValue))
            {
                var sl = BaseRepository().FindEntity(keyValue);
                if (sl != null)
                {
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }
                else
                {
                    b = true;
                }
            }
            else
            {
                b = true;
            }
            if (b)
            {
                entity.Id = keyValue;
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }
        #endregion
    }
}
