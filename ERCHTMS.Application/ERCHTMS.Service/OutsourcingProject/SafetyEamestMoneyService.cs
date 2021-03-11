using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using System;
using BSFramework.Util.Extension;
using ERCHTMS.Code;
using ERCHTMS.Service.BaseManage;

namespace ERCHTMS.Service.OutsourcingProject
{
    /// <summary>
    /// 描 述：安全保证金
    /// </summary>
    public class SafetyEamestMoneyService : RepositoryFactory<SafetyEamestMoneyEntity>, SafetyEamestMoneyIService
    {
        private AptitudeinvestigateauditIService Aptitudeinvestigateaudit = new AptitudeinvestigateauditService();
        private StartappprocessstatusIService startprocess = new StartappprocessstatusService();

        #region 获取数据
        public IEnumerable<SafetyEamestMoneyEntity> GetList() {
            return this.BaseRepository().IQueryable();
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public DataTable GetList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //时间范围
            if (!queryParam["sTime"].IsEmpty() || !queryParam["eTime"].IsEmpty())
            {
                string startTime = queryParam["sTime"].ToString();
                string endTime = queryParam["eTime"].ToString();
                if (queryParam["sTime"].IsEmpty())
                {
                    startTime = "1899-01-01";
                }
                if (queryParam["eTime"].IsEmpty())
                {
                    endTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
                pagination.conditionJson += string.Format(" and to_date(to_char(paymentdate,'yyyy-MM-dd'),'yyyy-MM-dd') between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", startTime, endTime);
            }
            //查询条件
            if (!queryParam["condition"].IsEmpty() && !queryParam["txtSearch"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and {0} like '%{1}%'", queryParam["condition"].ToString(), queryParam["txtSearch"].ToString());
            }
            if (!queryParam["projectid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and r.id ='{0}'", queryParam["projectid"].ToString());
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public DataTable GetEntity(string keyValue)
        {
            string sql = string.Format(@"select t.id,r.engineername,r.engineercode,r.engineertype,r.engineerarea,
                                                r.engineerlevel,r.engineerletdept,r.engineercontent,t.deptname,
                                                t.deptid,t.paymentperson,t.paymentpersonid,t.paymentdate,t.paymentmoney,
                                                d.auditresult,d.aptitudeid,d.audittime,d.auditopinion,d.auditpeople,
                                                d.auditdept,d.id as auditid,t.sendback,t.sendbackmoney
                                        from epg_safetyeamestmoney t left join epg_outsouringengineer r on t.projectid=r.id 
                                                left join epg_aptitudeinvestigateaudit d on d.aptitudeid=t.id
                                         where t.id='{0}'", keyValue);
            DataTable data = this.BaseRepository().FindTable(sql);
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
            this.BaseRepository().Delete(keyValue);
        }

        public void RemoveExamineForm(string keyValue) {
            var res = DbFactory.Base().BeginTrans();
            try
            {
             
                res.Delete<SafetyMoneyExamineEntity>(keyValue);
                res.Commit();
            }
            catch (Exception)
            {
                res.Rollback();
            }
       
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, string state, string auditid, SafetyEamestMoneyEntity entity, List<SafetyMoneyExamineEntity> list)
        {
            var res = DbFactory.Base().BeginTrans();
            try
            {
                Operator currUser = OperatorProvider.Provider.Current();
                entity.ID = keyValue;
                entity.ISSEND = state;
                if (!string.IsNullOrEmpty(keyValue))
                {
                    SafetyEamestMoneyEntity se = this.BaseRepository().FindEntity(keyValue);
                    if (se == null)
                    {
                        entity.Create();
                        res.Insert<SafetyEamestMoneyEntity>(entity);
                    }
                    else
                    {
                        entity.Modify(keyValue);
                        res.Update<SafetyEamestMoneyEntity>(entity);
                    }
                }
                else
                {
                    entity.Create();
                    res.Insert<SafetyEamestMoneyEntity>(entity);
                }
                //1发送审核,0保存
                if (state == "1")
                {
                    Repository<OutsouringengineerEntity> repEng = new Repository<OutsouringengineerEntity>(DbFactory.Base());
                    OutsouringengineerEntity outeng = repEng.FindEntity(entity.PROJECTID);
                    var sendDeptid = outeng.ENGINEERLETDEPTID;

                    if (currUser.RoleName.Contains("公司级用户") || currUser.RoleName.Contains("厂级部门用户")
                        || (sendDeptid == currUser.DeptId && currUser.RoleName.Contains("负责人"))
                        || (sendDeptid == currUser.DeptId && currUser.RoleName.Contains("安全管理员")) || (sendDeptid == currUser.DeptId && currUser.RoleName.Contains("专工")))
                    {
                        AptitudeinvestigateauditEntity e = new AptitudeinvestigateauditEntity();
                        if (!string.IsNullOrEmpty(auditid))
                        {
                            e = Aptitudeinvestigateaudit.GetEntity(auditid);
                            e.AUDITDEPT = currUser.DeptId;
                            e.AUDITDEPT = currUser.DeptName;
                            e.AUDITPEOPLE = currUser.UserName;
                            e.AUDITPEOPLEID = currUser.UserId;
                            e.AUDITRESULT = "0";

                        }
                        else
                        {
                            e.AUDITRESULT = "0";
                            e.AUDITDEPT = currUser.DeptId;
                            e.AUDITDEPT = currUser.DeptName;
                            e.AUDITPEOPLE = currUser.UserName;
                            e.AUDITPEOPLEID = currUser.UserId;
                            e.APTITUDEID = entity.ID;
                            //Aptitudeinvestigateaudit.SaveForm(auditid, e);
                        }
                        Aptitudeinvestigateaudit.SaveForm(auditid, e);
                        Repository<StartappprocessstatusEntity> proecss = new Repository<StartappprocessstatusEntity>(DbFactory.Base());
                        StartappprocessstatusEntity startProecss = proecss.FindList(string.Format("select * from epg_startappprocessstatus t where t.outengineerid='{0}'", entity.PROJECTID)).ToList().FirstOrDefault();
                        startProecss.SECURITYSTATUS = "1";

                        startprocess.SaveForm(startProecss.ID, startProecss);
                    }
                    else
                    {
                        AptitudeinvestigateauditEntity e = new AptitudeinvestigateauditEntity();
                        if (!string.IsNullOrEmpty(auditid))
                        {
                            e = Aptitudeinvestigateaudit.GetEntity(auditid);
                            e.AUDITDEPT = currUser.DeptId;
                            e.AUDITDEPT = currUser.DeptName;
                            e.AUDITPEOPLE = currUser.UserName;
                            e.AUDITPEOPLEID = currUser.UserId;
                            e.AUDITRESULT = "2";
                            Aptitudeinvestigateaudit.SaveForm(auditid, e);
                        }
                        else
                        {
                            e.AUDITRESULT = "2";
                            e.AUDITDEPT = currUser.DeptId;
                            e.AUDITDEPT = currUser.DeptName;
                            e.AUDITPEOPLE = currUser.UserName;
                            e.AUDITPEOPLEID = currUser.UserId;
                            e.APTITUDEID = entity.ID;
                            Aptitudeinvestigateaudit.SaveForm(auditid, e);
                        }
                    }
                }
                //string sql = string.Format("delete epg_safetymoneyexamine t where t.safetymoneyid='{0}'", keyValue);
                //res.ExecuteBySql(sql);
                if (list.Count > 0) {
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].Create();
                        list[i].ExamineDept = currUser.DeptName;
                        list[i].ExamineDeptId = currUser.DeptId;
                        list[i].ExamineToDept = entity.DEPTNAME;
                        list[i].ExamineToDeptId = entity.DEPTID;
                    }
                    res.Insert<SafetyMoneyExamineEntity>(list);
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
