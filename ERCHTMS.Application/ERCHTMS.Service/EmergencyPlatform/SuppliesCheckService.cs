using ERCHTMS.Entity.EmergencyPlatform;
using ERCHTMS.IService.EmergencyPlatform;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Util;
using ERCHTMS.Code;
using BSFramework.Data;
using BSFramework.Util.Extension;
using System;
using ERCHTMS.Service.BaseManage;

namespace ERCHTMS.Service.EmergencyPlatform
{
    /// <summary>
    /// 描 述：应急物资检查
    /// </summary>
    public class SuppliesCheckService : RepositoryFactory<SuppliesCheckEntity>, SuppliesCheckIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SuppliesCheckEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SuppliesCheckEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string role = user.RoleName;
            var queryParam = queryJson.ToJObject();
            #region 查表
            pagination.p_kid = "a.id";
            pagination.p_fields = string.IsNullOrWhiteSpace(pagination.p_fields) ? @"a.createuserid,a.createuserdeptcode,a.createuserorgcode,to_char(a.CHECKDATE,'yyyy-MM-dd') as CHECKDATE,a.checkusername,a.checkuserdept,('检查项：' ||  a.checknum || '项 不合格项：' || a.badnum || '项') as checkdetail,a.createusername,to_char(a.createdate,'yyyy-MM-dd') as createdate,a.checknum,a.badnum" : pagination.p_fields;
            pagination.p_tablename = @"mae_suppliescheck a";
            if (pagination.sidx == null)
            {
                pagination.sidx = "a.createdate";
            }
            if (pagination.sord == null)
            {
                pagination.sord = "desc";
            }
            #endregion

            //检查开始时间
            if (!queryParam["checkstartdate"].IsEmpty())
            {
                string from = queryParam["checkstartdate"].ToString().Trim();
                pagination.conditionJson += string.Format(" and a.checkdate>=to_date('{0}','yyyy-mm-dd')", from);
            }
            //检查结束时间
            if (!queryParam["checkenddate"].IsEmpty())
            {
                string to = Convert.ToDateTime(queryParam["checkenddate"].ToString().Trim()).AddDays(1).ToString("yyyy-MM-dd");
                pagination.conditionJson += string.Format(" and a.checkdate<to_date('{0}','yyyy-mm-dd')", to);
            }
            //检查人
            if (!queryParam["checkperson"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and a.checkusername like '%{0}%' ", queryParam["checkperson"].ToString());
            }
            //检查单位
            if (!queryParam["code"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and instr(',' || a.checkuserdeptcode || ',', ',{0},') > 0", queryParam["code"].ToString());
            }

            //关联物资管理
            if (!queryParam["suppliesid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and a.id in(select recid from mae_suppliescheckdetail where suppliesid ='{0}')", queryParam["suppliesid"].ToString());
            }

            DataTable data = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            
            return data;
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            var res = DbFactory.Base().BeginTrans();
            try
            {
                res.Delete<SuppliesCheckEntity>(keyValue);
                res.Delete<SuppliesCheckDetailEntity>(t => t.RecId == keyValue);
                res.Commit();
            }
            catch (Exception ex)
            {
                res.Rollback();
                throw ex;
            }
            
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, SuppliesCheckEntity entity)
        {
            var res = DbFactory.Base().BeginTrans();
            SuppliesCheckDetailService detailservice = new SuppliesCheckDetailService();
            try
            {
                if (string.IsNullOrEmpty(entity.Id))
                {
                    entity.Id = string.IsNullOrWhiteSpace(keyValue) ? Guid.NewGuid().ToString() : keyValue; //给entity实体主键赋值
                }
                foreach (var item in entity.DetailData)
                {
                    item.RecId = entity.Id;
                    if (!string.IsNullOrWhiteSpace(item.Id)) //根据DetailData主键获取对象，判断为null执行insert  否则执行update
                    {
                        var detail = detailservice.GetEntity(item.Id);
                        if (detail == null)
                        {
                            item.Create();
                            res.Insert(item);
                        }
                        else
                        {
                            item.Modify(item.Id);
                            res.Update(item);
                        }
                    }
                    else
                    {
                        item.Create();
                        res.Insert(item);
                    }
                }
                UserService userservice = new UserService();
                DataTable dt = userservice.GetUserTable(entity.CheckUserId.Split(','));
                entity.CheckUserDept = string.Join(",", dt.AsEnumerable().Select(d => d.Field<string>("DEPTNAME")).Distinct().ToArray());
                entity.CheckUserDeptCode = string.Join(",", dt.AsEnumerable().Select(d => d.Field<string>("DEPTCODE")).Distinct().ToArray());
                entity.CheckUserDeptId = string.Join(",", dt.AsEnumerable().Select(d => d.Field<string>("DEPARTMENTID")).Distinct().ToArray());
                entity.CheckNum = entity.DetailData.Count;
                entity.BadNum = entity.DetailData.Where(t => t.CheckResult == 1).Count();
                var data = GetEntity(entity.Id); //根据entity主键获取对象，判断为null执行insert  否则执行update
                if (data == null)
                {
                    entity.Create();
                    res.Insert(entity);
                }
                else
                {
                    entity.DetailData = null;
                    entity.Modify(entity.Id);
                    res.Update(entity);
                }
                res.Commit();
            }
            catch (Exception ex)
            {
                res.Rollback();
                throw ex;
            }
            

        }
        #endregion
    }
}
