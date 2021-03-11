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
using ERCHTMS.Service.BaseManage;
using ERCHTMS.Code;

namespace ERCHTMS.Service.OutsourcingProject
{
    /// <summary>
    /// 描 述：复工申请表
    /// </summary>
    public class ReturntoworkService : RepositoryFactory<ReturntoworkEntity>, ReturntoworkIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<ReturntoworkEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ReturntoworkEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 获取复工时间
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        public ReturntoworkEntity GetApplyRetrunTime(string outProjectId, string outEngId)
        {
            string sql = string.Format(@"select * from epg_returntowork t where t.outengineerid='{0}' and t.outprojectid='{1}'", outEngId, outProjectId);
            return this.BaseRepository().FindList(sql).OrderByDescending(x => x.CREATEDATE).ToList().FirstOrDefault();
        }
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            if (!queryParam["proName"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and  b.fullname like'%{0}%' ", queryParam["proName"].ToString());
            }
            if (!queryParam["engineerName"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and  e.engineername like'%{0}%' ", queryParam["engineerName"].ToString());
            }
            if (!queryParam["SendDept"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and e.engineerletdeptid='{0}' ", queryParam["SendDept"].ToString());
            }
            if (!queryParam["orgCode"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.createuserorgcode='{0}' ", queryParam["orgCode"].ToString());
            }
            //if (!queryParam["StartTime"].IsEmpty())
            //{
            //    pagination.conditionJson += string.Format(" and t.applyreturntime>='{0}' ", queryParam["StartTime"].ToString());
            //}
            //if (!queryParam["EndTime"].IsEmpty())
            //{
            //    pagination.conditionJson += string.Format(" and t.applyreturntime<='{0}' ", queryParam["EndTime"].ToString());
            //}
            if (!queryParam["StartTime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.applyreturntime>=to_date('{0}','yyyy-MM-dd hh24:mi:ss') ", queryParam["StartTime"].ToString());
            }
            if (!queryParam["EndTime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.applyreturntime<=to_date('{0}','yyyy-MM-dd hh24:mi:ss') ", Convert.ToDateTime(queryParam["EndTime"]).AddDays(1).ToString("yyyy-MM-dd"));
            }
            DataTable dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            return dt;
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
        public void SaveForm(string keyValue, ReturntoworkEntity entity)
        {
            Operator currUser = OperatorProvider.Provider.Current();
            //开始事务
            var res = DbFactory.Base().BeginTrans();
            try
            {
                entity.ID = keyValue;
                ReturntoworkEntity re = this.BaseRepository().FindEntity(keyValue);
                if (re == null)
                {
                    entity.Create();
                    string sql = string.Format("select * from epg_returntowork t where t.createuserorgcode ='{0}'", entity.CREATEUSERORGCODE);
                    int Code = this.BaseRepository().FindList(sql).ToList().Count;
                    switch (Code.ToString().Length)
                    {
                        case 1:
                            entity.APPLYNO = "FG000" + (Code + 1);
                            break;
                        case 2:
                            entity.APPLYNO = "FG00" + (Code + 1);
                            break;
                        case 3:
                            entity.APPLYNO = "FG0" + (Code + 1);
                            break;
                        default:
                            entity.APPLYNO = "FG" + (Code + 1);
                            break;
                    }
                    res.Insert<ReturntoworkEntity>(entity);
                }
                else
                {
                    entity.Modify(keyValue);
                    res.Update<ReturntoworkEntity>(entity);
                }
                if (entity.ISCOMMIT == "1")
                {
                    var sendDeptid = new DepartmentService().GetList().Where(x => x.DepartmentId == entity.OUTPROJECTID).ToList().FirstOrDefault().SendDeptID;
                    if (currUser.RoleName.Contains("公司级用户") || currUser.RoleName.Contains("厂级部门用户")
                           || (sendDeptid == currUser.DeptId && currUser.RoleName.Contains("负责人"))
                           || (sendDeptid == currUser.DeptId && currUser.RoleName.Contains("安全管理人员")))
                    {
                        AptitudeinvestigateauditEntity e = new AptitudeinvestigateauditEntity();
                        e.AUDITDEPT = currUser.DeptId;
                        e.AUDITDEPT = currUser.DeptName;
                        e.AUDITPEOPLE = currUser.UserName;
                        e.AUDITPEOPLEID = currUser.UserId;
                        e.AUDITRESULT = "0";
                        e.APTITUDEID = entity.ID;
                        e.ID = Guid.NewGuid().ToString();
                        res.Insert<AptitudeinvestigateauditEntity>(e);
                        Repository<OutsouringengineerEntity> ourEngineer = new Repository<OutsouringengineerEntity>(DbFactory.Base());
                        OutsouringengineerEntity engineerEntity = ourEngineer.FindEntity(entity.OUTENGINEERID);
                        engineerEntity.STOPRETURNSTATE = "0";
                        res.Update<OutsouringengineerEntity>(engineerEntity);
                    }
                }
                res.Commit();
            }
            catch (System.Exception)
            {

                res.Rollback();
            }

        }
        #endregion
    }
}
