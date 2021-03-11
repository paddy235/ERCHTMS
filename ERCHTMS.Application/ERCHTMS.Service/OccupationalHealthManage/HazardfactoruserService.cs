using System;
using ERCHTMS.Entity.OccupationalHealthManage;
using ERCHTMS.IService.OccupationalHealthManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Code;

namespace ERCHTMS.Service.OccupationalHealthManage
{
    /// <summary>
    /// 描 述：危害因素人员表
    /// </summary>
    public class HazardfactoruserService : RepositoryFactory<HazardfactoruserEntity>, HazardfactoruserIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<HazardfactoruserEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable(it => it.Hid == queryJson).ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public HazardfactoruserEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 查询职业病接触用户表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            var curuser = OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();


            if (!queryParam["Name"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and realname='{0}'", queryParam["Name"].ToString().Trim());
            }
            if (!queryParam["DeptCode"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and DeptCode like  '{0}%'", queryParam["DeptCode"].ToString().Trim());
            }
            //姓名
            if (!queryParam["PostId"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and  PostId = '{0}'", queryParam["PostId"].ToString().Trim());
            }
            //身份证号
            if (!queryParam["us"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and  us like '%{0}%'", queryParam["us"].ToString().Trim());
            }

            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// 查询职业病接触用户表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(string sqlwhere, string queryJson)
        {
            var curuser = OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;

            string sql = string.Format(
                @" select u.USERID,REALNAME,MOBILE,OrganizeName,DEPTNAME,DUTYNAME,POSTNAME,usertype,GENDER,OrganizeCode,identifyid,realname as username,DEPARTURETIME,u.deptcode,nature,u.DEPARTMENTCODE,organizeid,u.IsTransfer,u.DepartureReason,us from v_userinfo u left join (select userid,username,LISTAGG(riskvalue,',') WITHIN GROUP (ORDER BY riskvalue) AS us from (  select userid,username,riskvalue from BIS_HAZARDFACTORUSER hauser left join BIS_HAZARDFACTORs ha on ha.hid=hauser.hid where 1=1 group by userid,username,riskvalue) f  group by userid,username) t on u.account=t.userid where ");

            var queryParam = queryJson.ToJObject();


            if (!queryParam["Name"].IsEmpty())
            {
                sqlwhere += string.Format(" and realname='{0}'", queryParam["Name"].ToString().Trim());
            }
            if (!queryParam["DeptCode"].IsEmpty())
            {
                sqlwhere += string.Format(" and DeptCode like  '{0}%'", queryParam["DeptCode"].ToString().Trim());
            }
            //姓名
            if (!queryParam["PostId"].IsEmpty())
            {
                sqlwhere += string.Format(" and  PostId = '{0}'", queryParam["PostId"].ToString().Trim());
            }
            //身份证号
            if (!queryParam["us"].IsEmpty())
            {
                sqlwhere += string.Format(" and  us like '%{0}%'", queryParam["us"].ToString().Trim());
            }

            return this.BaseRepository().FindTable(sql + sqlwhere);
        }

        /// <summary>
        /// 获取用户的接触危害因素
        /// </summary>
        /// <returns></returns>
        public string GetUserHazardfactor(string useraccount)
        {
            string sql = string.Format(
                @"select LISTAGG(riskvalue,',') WITHIN GROUP (ORDER BY riskvalue) AS us from (              
            select userid,username,riskvalue from BIS_HAZARDFACTORUSER hauser
            left join BIS_HAZARDFACTORs ha on ha.hid=hauser.hid
             where 1=1
             group by userid,username,riskvalue) f where userid='{0}'  group by userid,username", useraccount);
            object ret = BaseRepository().FindObject(sql);
            if (ret == null)
            {
                return "";
            }
            else
            {
                return ret.ToString();
            }
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
        public void SaveForm(string keyValue, HazardfactoruserEntity entity)
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
