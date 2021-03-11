using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using BSFramework.Util.Extension;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Linq;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.IService.AuthorizeManage;
using ERCHTMS.Service.AuthorizeManage;
using System;
using ERCHTMS.Code;
using ERCHTMS.Entity.EmergencyPlatform;
using ERCHTMS.IService.EmergencyPlatform;
using ERCHTMS.Entity.AccidentEvent;
using ERCHTMS.IService.AccidentEvent;
using System.Linq.Expressions;
using ERCHTMS.Service.CommonPermission;

namespace ERCHTMS.Service.AccidentEvent
{
    /// <summary>
    /// 描 述：事故事件快报
    /// </summary>
    public class BulletinService : RepositoryFactory<BulletinEntity>, IBulletinService
    {
        #region 获取数据

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="condition">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<BulletinEntity> GetListForCon(Expression<Func<BulletinEntity, bool>> condition)
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

            DatabaseType dataType = DbHelper.DbType;

            if (queryJson.Length > 0)
            {
                var queryParam = queryJson.ToJObject();
                //查询条件
                if (!queryParam["keyword"].IsEmpty())
                {

                    string keyord = queryParam["keyword"].ToString();
                    //keyord
                    pagination.conditionJson += string.Format(" and (SGNAME  like '%{0}%' or SGKBUSERNAME like '%{1}%' or AREANAME like '%{2}%') ", keyord, keyord, keyord);
                }

                if (!queryParam["sgtypeLevel"].IsEmpty())
                {
                    string sgtypeLevel = queryParam["sgtypeLevel"].ToString();
                    pagination.conditionJson += " and sgtype='" + sgtypeLevel + "'";
                }

                //事故原因
                if (!queryParam["sgyy"].IsEmpty())
                {
                    string keyord = queryParam["sgyy"].ToString();
                    if (keyord == "IsXW")
                        pagination.conditionJson += string.Format(" and BAQXWNAME is not null ");
                    else if (keyord == "IsZT")
                        pagination.conditionJson += string.Format(" and BAQZTNAME is not null ");
                    else
                        pagination.conditionJson += string.Format(" and (JJYYNAME  like '%{0}%' or BAQXWNAME like '%{1}%' or BAQZTNAME like '%{2}%') ", keyord, keyord, keyord);
                }
                if (!queryParam["sgtype"].IsEmpty())
                {
                    string sgtype = queryParam["sgtype"].ToString();
                    pagination.conditionJson += string.Format(" and sgtype = '{0}'", sgtype);
                } if (!queryParam["sgtypename"].IsEmpty())
                {
                    string sgtypename = queryParam["sgtypename"].ToString();
                    pagination.conditionJson += string.Format(" and sgtypename = '{0}'", sgtypename);
                }

                if (!queryParam["rsshsgtypename"].IsEmpty())
                {
                    string rsshsgtypename = queryParam["rsshsgtypename"].ToString();
                    pagination.conditionJson += string.Format(" and rsshsgtypename = '{0}'", rsshsgtypename);
                }
                if (!queryParam["sglevelname"].IsEmpty())
                {
                    string sglevelname = queryParam["sglevelname"].ToString();
                    pagination.conditionJson += string.Format(" and sglevelname = '{0}'", sglevelname);
                }
                if (!queryParam["rsshsgtype_deal"].IsEmpty())
                {
                    string rsshsgtype_deal = queryParam["rsshsgtype_deal"].ToString();
                    pagination.conditionJson += string.Format(" and rsshsgtype_deal = '{0}'", rsshsgtype_deal);
                }
                if (!queryParam["happentimestart"].IsEmpty())
                {
                    string happentimestart = queryParam["happentimestart"].ToString();
                    pagination.conditionJson += string.Format(" and happentime >= (select  to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') from dual)", happentimestart);
                }
                if (!queryParam["happentimeend"].IsEmpty())
                {
                    string happentimeend = queryParam["happentimeend"].ToString();
                    if (happentimeend.Length == 10)
                        happentimeend = Convert.ToDateTime(happentimeend).AddDays(1).ToString();
                    pagination.conditionJson += string.Format(" and happentime <= (select  to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') from dual)", happentimeend);
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
                //报表类型
                if (!queryParam["type"].IsEmpty())
                {
                    var type = queryParam["type"].ToString();
                    if (type == "swrs")
                        pagination.conditionJson += string.Format(" and sgtypename like '%人身伤亡事故%'");
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
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetGenericPageList(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;

            if (queryJson.Length > 0)
            {
                var queryParam = queryJson.ToJObject();
                //查询条件
                if (!queryParam["keyword"].IsEmpty())
                {

                    string keyord = queryParam["keyword"].ToString();
                    //keyord
                    pagination.conditionJson += string.Format(" and (SGNAME  like '%{0}%' or SGKBUSERNAME like '%{1}%' or AREANAME like '%{2}%') ", keyord, keyord, keyord);
                }

                if (!queryParam["sgtypeLevel"].IsEmpty())
                {
                    string sgtypeLevel = queryParam["sgtypeLevel"].ToString();
                    pagination.conditionJson += " and sgtype='" + sgtypeLevel + "'";
                }

                //事故原因
                if (!queryParam["sgyy"].IsEmpty())
                {
                    string keyord = queryParam["sgyy"].ToString();
                    if (keyord == "IsXW")
                        pagination.conditionJson += string.Format(" and BAQXWNAME is not null ");
                    else if (keyord == "IsZT")
                        pagination.conditionJson += string.Format(" and BAQZTNAME is not null ");
                    else
                        pagination.conditionJson += string.Format(" and (JJYYNAME  like '%{0}%' or BAQXWNAME like '%{1}%' or BAQZTNAME like '%{2}%') ", keyord, keyord, keyord);
                }
                if (!queryParam["sgtype"].IsEmpty())
                {
                    string sgtype = queryParam["sgtype"].ToString();
                    pagination.conditionJson += string.Format(" and RSSHSGTYPE = '{0}'", sgtype);
                } if (!queryParam["sgtypename"].IsEmpty())
                {
                    string sgtypename = queryParam["sgtypename"].ToString();
                    pagination.conditionJson += string.Format(" and sgtypename = '{0}'", sgtypename);
                }

                if (!queryParam["rsshsgtypename"].IsEmpty())
                {
                    string rsshsgtypename = queryParam["rsshsgtypename"].ToString();
                    pagination.conditionJson += string.Format(" and rsshsgtypename = '{0}'", rsshsgtypename);
                }
                if (!queryParam["sglevelname"].IsEmpty())
                {
                    string sglevelname = queryParam["sglevelname"].ToString();
                    pagination.conditionJson += string.Format(" and sglevelname = '{0}'", sglevelname);
                }
                if (!queryParam["rsshsgtype_deal"].IsEmpty())
                {
                    string rsshsgtype_deal = queryParam["rsshsgtype_deal"].ToString();
                    pagination.conditionJson += string.Format(" and rsshsgtype_deal = '{0}'", rsshsgtype_deal);
                }
                if (!queryParam["happentimestart"].IsEmpty())
                {
                    string happentimestart = queryParam["happentimestart"].ToString();
                    pagination.conditionJson += string.Format(" and happentime >= (select  to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') from dual)", happentimestart);
                }
                if (!queryParam["happentimeend"].IsEmpty())
                {
                    string happentimeend = queryParam["happentimeend"].ToString();
                    if (happentimeend.Length == 10)
                        happentimeend = Convert.ToDateTime(happentimeend).AddDays(1).ToString();
                    pagination.conditionJson += string.Format(" and happentime <= (select  to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') from dual)", happentimeend);
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
                //报表类型
                if (!queryParam["type"].IsEmpty())
                {
                    var type = queryParam["type"].ToString();
                    if (type == "swrs")
                        pagination.conditionJson += string.Format(" and sgtypename like '%人身伤亡事故%'");
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
        public IEnumerable<BulletinEntity> GetList(string queryJson)
        {
            return this.BaseRepository().FindList(" select * from v_aem_Bulletin_deal where 1=1 " + queryJson).ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public BulletinEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, BulletinEntity entity)
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
