using ERCHTMS.Entity.AccidentEvent;
using ERCHTMS.IService.AccidentEvent;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using System.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Code;
using System.Linq.Expressions;
using System;
using ERCHTMS.Service.CommonPermission;
namespace ERCHTMS.Service.AccidentEvent
{
    /// <summary>
    /// 描 述：WSSJBG
    /// </summary>
    public class WSSJBGService : RepositoryFactory<WSSJBGEntity>, IWSSJBGService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="condition">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<WSSJBGEntity> GetListForCon(Expression<Func<WSSJBGEntity, bool>> condition)
        {

            return this.BaseRepository().IQueryable(condition).ToList();
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
             Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            DatabaseType dataType = DbHelper.DbType;

            if (queryJson.Length > 0)
            {
                var queryParam = queryJson.ToJObject();
                //查询条件
                if (!queryParam["keyword"].IsEmpty())
                {

                    string keyord = queryParam["keyword"].ToString();
                    //keyord
                    pagination.conditionJson += string.Format(" and (WSSJNAME  like '%{0}%' or WSSJBGUSERNAME like '%{1}%' or AREANAME like '{2}') ", keyord, keyord, keyord);
                }
                if (!queryParam["wssjtype"].IsEmpty())
                {
                    string sgtype = queryParam["wssjtype"].ToString();
                    pagination.conditionJson += string.Format(" and WSSJTYPE = '{0}'", sgtype);
                }
                if (!queryParam["wssjtypename"].IsEmpty())
                {
                    string wssjtypename = queryParam["wssjtypename"].ToString();
                    pagination.conditionJson += string.Format(" and WSSJTYPENAME = '{0}'", wssjtypename);
                }


                if (!queryParam["happentimestart"].IsEmpty())
                {
                    string happentimestart = queryParam["happentimestart"].ToString();
                    pagination.conditionJson += string.Format(" and happentime >=(select  to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') from dual)", happentimestart);
                }
                if (!queryParam["happentimeend"].IsEmpty())
                {
                    string happentimeend = queryParam["happentimeend"].ToString();
                    if (happentimeend.Length == 10)
                        happentimeend = Convert.ToDateTime(happentimeend).AddDays(1).ToString();
                    pagination.conditionJson += string.Format(" and happentime <=(select  to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') from dual)", happentimeend);
                }
                //处理时间
                if (!queryParam["happentimestart_deal"].IsEmpty())
                {
                    string happentimestart_deal = queryParam["happentimestart_deal"].ToString();
                    pagination.conditionJson += string.Format(" and happentime_deal >= (select  to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') from dual)", happentimestart_deal);
                }
                if (!queryParam["happentimeend_deal"].IsEmpty())
                {
                    string happentimeend_deal = queryParam["happentimeend_deal"].ToString();
                    if (happentimeend_deal.Length == 10)
                        happentimeend_deal = Convert.ToDateTime(happentimeend_deal).AddDays(1).ToString();
                    pagination.conditionJson += string.Format(" and happentime_deal <= (select  to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') from dual)", happentimeend_deal);
                }
                if (!queryParam["IsSubmit"].IsEmpty())
                {
                    var IsSubmit = int.Parse(queryParam["IsSubmit"].ToString());
                    pagination.conditionJson += string.Format(" and IsSubmit = '{0}'", IsSubmit);
                }

                if (!queryParam["IsSubmit_Deal"].IsEmpty())
                {
                    var IsSubmit_Deal = int.Parse(queryParam["IsSubmit_Deal"].ToString());
                    if (IsSubmit_Deal == 0)
                        pagination.conditionJson += string.Format(" and ( IsSubmit_Deal = '{0}' OR IsSubmit_Deal is null)", IsSubmit_Deal);
                    else
                        pagination.conditionJson += string.Format(" and IsSubmit_Deal = '{0}'", IsSubmit_Deal);
                }

                //公司主键
                if (!queryParam["organizeId"].IsEmpty())
                {
                    string organizeId = queryParam["organizeId"].ToString();
                    pagination.conditionJson += string.Format(" and ORGANIZEID = '{0}'", organizeId);
                }
                //部门主键
                if (!queryParam["departmentId"].IsEmpty())
                {
                    string departmentId = queryParam["departmentId"].ToString();
                    pagination.conditionJson += string.Format(" and departmentid = '{0}'", departmentId);
                }
                //厂级领导安全指标参数
                if (!queryParam["action"].IsEmpty()) 
                {
                    pagination.conditionJson += string.Format(" and  t.createuserorgcode='{0}' and  issubmit_deal =1  and to_char(t.createdate,'yyyy')='{1}'", user.OrganizeCode, DateTime.Now.Year);
                }

                #region 权限判断
                if (!queryParam["code"].IsEmpty() && !queryParam["isOrg"].IsEmpty())
                {
                    pagination = PermissionByCurrent.GetPermissionByCurrent2(pagination, queryParam["code"].ToString(), queryParam["isOrg"].ToString());
                }
                #endregion
            }

            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }


        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<WSSJBGEntity> GetList(string queryJson)
        {
            return this.BaseRepository().FindList(" select * from v_aem_WSSJBG_deal where 1=1 " + queryJson).ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public WSSJBGEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, WSSJBGEntity entity)
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
