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
using System;
using ERCHTMS.Code;
using ERCHTMS.Entity.SystemManage;

namespace ERCHTMS.Service.EvaluateManage
{
    /// <summary>
    /// 描 述：合规性评价计划
    /// </summary>
    public class EvaluatePlanService : RepositoryFactory<EvaluatePlanEntity>, EvaluatePlanIService
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
                    pagination.conditionJson += string.Format(" and Year='{0}'", queryParam["Year"].ToString());
                }
                //工作标题
                if (!queryParam["WorkTitle"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and WorkTitle like'{0}%'", queryParam["WorkTitle"].ToString());
                }
            }
            var userid = OperatorProvider.Provider.Current().UserId;
            pagination.conditionJson += string.Format(" and (CreateUserId='{0}' or IsSubmit>=1)", userid);
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<EvaluatePlanEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public EvaluatePlanEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, EvaluatePlanEntity entity)
        {
            if (string.IsNullOrEmpty(keyValue))
            {
                entity.Id = System.Guid.NewGuid().ToString();
            }
            else
            {
                entity.Id = keyValue;
            }
            //插入评价数据
            if (entity.CheckState == 0 || entity.CheckState == null)
            {
                entity.DoneDeptNum = 0;
                if (entity.IsSubmit == 1)
                {
                    entity.CheckState = 0;//0数据已提交 1评价报告保存 2评价报告提交 3审核保存 4审核提交
                    entity.Dept = AddEvaluate(entity, 1);
                }
                else
                {
                    entity.Dept = AddEvaluate(entity, 0);
                }
                try
                {
                    string str = entity.Dept.TrimEnd(',');
                    string[] strArr = str.Split(',');
                    entity.DeptNum = strArr.Length;
                }
                catch { }
            }
            //审核结束 修改整改状态
            if (entity.CheckState == 4)
            {
                UpdateRectifyState(entity);
            }
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                entity.Year = DateTime.Now.Year;//年度
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
            
        }
        /// <summary>
        /// 添加合规性评价表
        /// </summary>
        /// <param name="entity">合规性评价计划</param>
        /// <param name="type">0 仅查询部门  1查询并插入子表</param>
        public string AddEvaluate(EvaluatePlanEntity entity,int type)
        {

            try
            {
                int deptnum = 0;
                string deptstr = "";
                Operator curUser = OperatorProvider.Provider.Current();
                //获取需要评价的部门
                DataTable dt = BaseRepository().FindTable("select DEPARTMENTID,DEPTCODE,FULLNAME,ENCODE from BASE_DEPARTMENT t where nature='部门' and description is null and organizeid='"+ curUser.OrganizeId + "'");
                
                //插入数据
                if (dt != null)
                {
                    foreach (DataRow item in dt.Rows) {
                        deptnum++;
                        deptstr += item["fullname"].ToString() + ",";
                        if (type == 1)
                        {
                            IRepository db = new RepositoryFactory().BaseRepository();
                            EvaluateEntity fe = new EvaluateEntity();
                            fe.WorkTitle = entity.WorkTitle;
                            fe.DutyDept = item["fullname"].ToString();
                            fe.DutyDeptCode = item["deptcode"].ToString();
                            fe.EvaluateState = 0; //0未评价 1评价中 2已评价
                            fe.EvaluatePlanId = entity.Id;
                            fe.RectifyState = 99;
                            fe.Create();
                            db.Insert<EvaluateEntity>(fe);
                        }
                    }
                }
                return deptstr;
            }
            catch (Exception ex) {
                return "";
            }
        }

        /// <summary>
        /// 审核结束后修改存在不符合项的部门整改状态
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateRectifyState(EvaluatePlanEntity entity)
        {
            try
            {
                string idstr = "";
                DataTable dt = BaseRepository().FindTable(@"select t1.id from HRS_EVALUATE t1
                            left join  HRS_EVALUATEDETAILS t2 on t2.mainid = t1.id
                            where t2.IsConform = 1 and t1.EvaluatePlanId = '" + entity.Id + "'");
                if (dt != null)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        idstr += "'" + item["id"].ToString() + "',";
                    }
                    string str = idstr.TrimEnd(',');
                    DataTable dt1 = BaseRepository().FindTable(@"update HRS_EVALUATE set rectifystate=1 where id in(" + str + ")");
                }
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// 一键提醒
        /// </summary>
        public DataTable GetRemindUser(string keyValue)
        {

            try
            {
                Operator curUser = OperatorProvider.Provider.Current();
                //获取需要评价的部门
                DataTable dt = BaseRepository().FindTable(@"select b.MANAGERID as userid,b.MANAGER as username
                                                            from HRS_EVALUATE a
                                                            left join BASE_DEPARTMENT b on a.dutydeptcode=b.deptcode
                                                            where a.evaluateplanid='"+ keyValue + "' and a.evaluatestate<2");
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
    }
}
