using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System;
using ERCHTMS.Code;

namespace ERCHTMS.Service.OutsourcingProject
{
    /// <summary>
    /// 描 述：外包-安全验收
    /// </summary>
    public class SafetyCollectService : RepositoryFactory<SafetyCollectEntity>, SafetyCollectIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;
            if (queryJson != null && queryJson != "")
            {
                var queryParam = queryJson.ToJObject();
                //承包单位
                if (!queryParam["OutprojectName"].IsEmpty())
                {
                    string OutprojectName = queryParam["OutprojectName"].ToString();
                    pagination.conditionJson += string.Format(" and p.outsourcingname like '%{0}%'", OutprojectName);
                }
                //外包工程名称
                if (!queryParam["EngineerName"].IsEmpty())
                {
                    string EngineerName = queryParam["EngineerName"].ToString();
                    pagination.conditionJson += string.Format(" and o.engineername like '%{0}%'", EngineerName);
                }
                //开始时间
                if (!queryParam["StartTime"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(@" and t.CREATEDATE >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", queryParam["StartTime"].ToString());
                }
                //结束时间
                if (!queryParam["EndTime"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(@" and t.CREATEDATE < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(queryParam["EndTime"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
                }
                if (!queryParam["mode"].IsEmpty())
                {
                    string ids = GetCheckList(user);
                    if (ids.Length > 0)
                    {
                        ids = ids.TrimEnd(',');
                        pagination.conditionJson += string.Format(@" and t.id in({0})", ids);
                    }
                    else
                    {
                        pagination.conditionJson += string.Format(@" and t.id=''");
                    }
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        public string GetCheckList(ERCHTMS.Code.Operator user)
        {
            string ids = "";
            DataTable dt = this.BaseRepository().FindTable(string.Format(@"select * from EPG_SAFETYCOLLECT t where flowdept like'%{0}%'", user.DeptId));
            int count = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (!string.IsNullOrWhiteSpace(dr["flowrolename"].ToString()))
                {

                    var roleArr = user.RoleName.Split(','); //当前人员角色
                    var roleName = dr["flowrolename"].ToString(); //审核橘色
                    for (var i = 0; i < roleArr.Length; i++)
                    {
                        //满足审核部门同当前人部门id一致，切当前人角色存在与审核角色中
                        if (roleName.IndexOf(roleArr[i]) >= 0)
                        {
                            ids += "'" + dr["id"].ToString() + "',";
                            break;
                        }
                    }
                }
            }
            return ids;
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SafetyCollectEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SafetyCollectEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, SafetyCollectEntity entity)
        {
            entity.ID = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                SafetyCollectEntity fe = this.BaseRepository().FindEntity(keyValue);
                if (fe == null)
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
    }
}