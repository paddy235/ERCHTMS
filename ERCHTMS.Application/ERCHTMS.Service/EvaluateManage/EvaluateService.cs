using ERCHTMS.Entity.EvaluateManage;
using ERCHTMS.IService.EvaluateManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Code;
using System;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Service.EvaluateManage
{
    /// <summary>
    /// 描 述：合规性评价
    /// </summary>
    public class EvaluateService : RepositoryFactory<EvaluateEntity>, EvaluateIService
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
                //年度
                if (!queryParam["Year"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and to_char(CreateDate,'yyyy')='{0}'", queryParam["Year"].ToString());
                }
                //工作标题
                if (!queryParam["WorkTitle"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and WorkTitle like'{0}%'", queryParam["WorkTitle"].ToString());
                }
                //整改（查询待整改、整改中、已整改数据）
                if (!queryParam["RectifyState"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and RectifyState<>{0}", queryParam["RectifyState"].ToString());
                }
                //部门
                if (!queryParam["DutyDeptCode"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and DutyDeptCode like '{0}%'", queryParam["DutyDeptCode"].ToString());
                }
                //关联的评价计划
                if (!queryParam["EvaluatePlanId"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and EvaluatePlanId='{0}' and EvaluateState>=2", queryParam["EvaluatePlanId"].ToString());
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<EvaluateEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public EvaluateEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, EvaluateEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
                if (entity.EvaluateState == 2)//1 保存 2 提交
                {
                    UpdateDoneDeptNum(entity);
                }
            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }
        /// <summary>
        /// 自动获取
        /// </summary>
        /// <param name="keyValue">主表ID</param>
        /// <param name="fileSpotList">实体对象</param>
        /// <param name="EvaluatePlanId">评价计划ID</param>
        /// <returns></returns>
        public void SaveForm3(string keyValue, IEnumerable<FileSpotEntity> fileSpotList, string EvaluatePlanId)
        {
            var user = OperatorProvider.Provider.Current();
            String insertSql = "";
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    if (fileSpotList.Count() > 0)
                    {
                        foreach (var item in fileSpotList)
                        {
                            insertSql += string.Format(@"insert into 
                            HRS_EVALUATEDETAILS(ID,
                                                AUTOID,
                                                CreateUserId,
                                                CreateUserDeptCode,
                                                CreateUserOrgCode,
                                                CreateDate,
                                                CreateUserName,
                                                ModifyDate,
                                                ModifyUserId,
                                                ModifyUserName,
                                                Category,
                                                Rank,
                                                DutyDept,
                                                DutyDeptCode,
                                                Clause,
                                                Describe,
                                                Opinion,
                                                FinishTime,
                                                Remake,
                                                MainId,
                                                FileName,
                                                Year,
                                                IsConform,
                                                Measure,
                                                PracticalFinishTime,
                                                EvaluatePlanId,
                                                PutDate,
                                                EvaluatePerson,
                                                EvaluatePersonId) 
values('{0}','','{1}','{2}','{3}','{4}','{5}','','','','','','{6}','{7}','','','','','','{8}','{9}',{10},0,'','','{11}','','{12}',
(select USERID from base_user where REALNAME='{12}' and rownum<2));",
    Guid.NewGuid().ToString(), user.UserId, user.DeptCode, user.OrganizeCode, DateTime.Now, user.UserName, user.DeptName, user.DeptCode, keyValue, item.FileName, DateTime.Now.Year, EvaluatePlanId,item.UserName);
                        }
                        BaseRepository().FindTable(insertSql);
                    }
                }
            }
            catch { }
        }
        /// <summary>
        /// 提交更新计划完成情况
        /// </summary>
        /// <param name="entity">合规性评价计划</param>
        public void UpdateDoneDeptNum(EvaluateEntity entity)
        {

            try
            {
                IRepository db = new RepositoryFactory().BaseRepository();
                //获取需要评价的部门
                EvaluatePlanEntity ee = db.FindEntity<EvaluatePlanEntity>(entity.EvaluatePlanId);

                //插入数据
                if (ee != null)
                {
                    ee.DoneDeptNum = ee.DoneDeptNum + 1;
                    db.Update<EvaluatePlanEntity>(ee);
                }
            }
            catch (Exception ex) { }
        }
        #endregion
    }
}
