using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Data;
using BSFramework.Util;
using BSFramework.Data;
using BSFramework.Util.Extension;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Code;
using ERCHTMS.Service.BaseManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Entity;
using ERCHTMS.Service.HighRiskWork;
using ERCHTMS.Entity.PowerPlantInside;
using ERCHTMS.Service.PowerPlantInside;
using ERCHTMS.Service.PersonManage;
using ERCHTMS.Entity.PersonManage;
using ERCHTMS.Service.RoutineSafetyWork;
using ERCHTMS.Entity.RoutineSafetyWork;
using System.Data.Common;
using ERCHTMS.Entity.DangerousJob;

using ERCHTMS.Entity.HighRiskWork.ViewModel;
using ERCHTMS.IService.DangerousJob;




namespace ERCHTMS.Service.OutsourcingProject
{
    /// <summary>
    /// 描 述：安全考核主表
    /// </summary>
    public class SafetyAssessmentService : RepositoryFactory<SafetyAssessmentEntity>, SafetyAssessmentIService
    {

        private IManyPowerCheckService powerCheck = new ManyPowerCheckService();
        #region 获取数据
        /// <summary>
        /// 导出安全考核汇总
        /// </summary>
        /// <param name="time"></param>
        /// <param name="deptid"></param>
        /// <returns></returns>
        public DataTable ExportDataTotal(string time, string deptid)
        {
            DataTable dt = null;

            Operator user = OperatorProvider.Provider.Current();


            try
            {
                string sql = "select " +
                    "   b.CREATEDATE,a.EVALUATEDEPTNAME,a.EVALUATEDEPT,a.EVALUATETYPE, " +
                    "   nvl((case when a.EVALUATEDEPT = '-1' then a.EVALUATEDEPTNAME when a.EVALUATETYPE = 1 or a.EVALUATETYPE = 3 then a.EVALUATEDEPTNAME when a.EVALUATETYPE = 2 or a.EVALUATETYPE = 4 then null  end),d.FULLNAME) as deptresultName," +
                    "   d.FULLNAME,b.EXAMINEREASON,b.EXAMINEBASIS,b.EXAMINETYPENAME,a.SCORETYPE,a.SCORE,a.EVALUATESCORE,a.EVALUATECONTENT    " +
                    "   from " +
                    "   epg_safetyassessmentperson a inner join epg_safetyassessment b on a.safetyassessmentid = b.id " +
                    "   left join base_user c on c.USERID = a.EVALUATEDEPT left join base_department d on d.departmentid = c.DEPARTMENTID" +
                    "   where b.ISOVER='1'  " +
                    "   and b.EXAMINEDEPTID in (select dept.DEPARTMENTID from base_department dept where dept.ENCODE like '"+ deptid + "%' ) and b.CREATEDATE between to_date('" + time + "-01 00:00:00','yyyy-mm-dd hh24:mi:ss') and add_months(to_date('" + time + "-01 00:00:00','yyyy-mm-dd hh24:mi:ss'),1)   and a.CREATEUSERDEPTCODE like '" + user.OrganizeCode + "%'";
                dt = this.BaseRepository().FindTable(sql);
            }
            catch (Exception)
            {

                
            }
            return dt;
        }

        /// <summary>
        /// 获取内部部门
        /// </summary>
        /// <returns></returns>
        public DataTable GetInDeptData()
        {
            DataTable dt = null;
            Operator user = OperatorProvider.Provider.Current();

            try
            {
                string sql = "SELECT DEPARTMENTID, FULLNAME, ENCODE FROM BASE_DEPARTMENT WHERE NATURE = '部门' AND ORGANIZEID =  '" + user.OrganizeId + "' AND DESCRIPTION IS NULL";
                dt = this.BaseRepository().FindTable(sql);
            }
            catch (Exception)
            {


            }
            return dt;
        }

        /// <summary>
        /// 导出内部单位考核
        /// </summary>
        /// <param name="time"></param>
        /// <param name="deptid"></param>
        /// <returns></returns>
        public DataTable ExportDataInDept(string time,string deptid)
        {
            DataTable dt = null;
            Operator user = OperatorProvider.Provider.Current();
            try
            {
                string sql = "WITH T1 AS"+
                             " (SELECT DEPARTMENTID, FULLNAME, ENCODE, ORGANIZEID  " +
                               " FROM BASE_DEPARTMENT " +
                               " WHERE(NATURE = '部门' " +
                                "  OR NATURE = '专业' OR NATURE = '班组') " +
                                "  AND ORGANIZEID = '" + user.OrganizeId + "')," +
                           "  T2 AS " +
                            "  (SELECT DEPARTMENTID, FULLNAME, ENCODE FROM BASE_DEPARTMENT WHERE NATURE = '部门' AND ORGANIZEID =  '" + user.OrganizeId + "' AND DESCRIPTION IS NULL), " +
                           " T3 AS " +
                           "  (SELECT USERID，ACCOUNT，REALNAME，DEPARTMENTCODE " +
                           "     FROM BASE_USER A " +
                           "    WHERE A.DEPARTMENTID IN (SELECT DEPARTMENTID FROM T1)), " +
                          "  T4 AS " +
                          "   (SELECT " +
                          "     B.ID, " +
                         "     B.CREATEDATE, " +
                           "          A.EVALUATEDEPTNAME, " +
                            "         A.EVALUATEDEPT, " +
                            "         A.EVALUATETYPE, " +
                            "         A.CREATEUSERDEPTCODE, " +
                            "         B.EXAMINEREASON, " +
                            "         B.EXAMINEBASIS, " +
                            "         B.EXAMINETYPENAME, " +
                            "         A.SCORETYPE," +
                            "         A.SCORE, " +
                            "         A.EVALUATESCORE, " +
                            "         A.EVALUATECONTENT, " +
                            "         USR.DEPARTMENTCODE, " +
                            "         B.EXAMINEBASISID " +
                            "    FROM EPG_SAFETYASSESSMENTPERSON A " +
                            "   INNER JOIN EPG_SAFETYASSESSMENT B " +
                            "      ON A.SAFETYASSESSMENTID = B.ID " +
                            "    LEFT JOIN BASE_USER USR " +
                            "      ON USR.USERID = A.EVALUATEDEPT " +
                            "   WHERE B.ISOVER = '1'  and b.EXAMINEDEPTID in (select dept.DEPARTMENTID from base_department dept where dept.ENCODE like '" + deptid + "%' ) " +
                            "     AND (A.EVALUATEDEPT IN (SELECT ENCODE FROM T1) OR " +
                            "         A.EVALUATEDEPT IN(SELECT USERID FROM T3)) " +
                            "     AND B.CREATEDATE BETWEEN " +
                            "         to_date('" + time + "-01 00:00:00','yyyy-mm-dd hh24:mi:ss') AND " +
                            "          add_months(to_date('" + time + "-01 00:00:00','yyyy-mm-dd hh24:mi:ss'),1)" +
                            "     AND A.CREATEUSERDEPTCODE LIKE '" + user.OrganizeCode + "%') " +
                            "     SELECT* FROM( " +
                            "    SELECT T4.*, " +
                            "          NVL(T2.FULLNAME, TS.FULLNAME) AS BMNAME," +
                            "          NVL(T2.ENCODE, TS.ENCODE) AS BMNAMECODE,NVL(T2.DEPARTMENTID, TS.DEPARTMENTID) AS BMNAMEID " +
                            "      FROM T4 " +
                            "      LEFT JOIN T2 " +
                            "        ON SUBSTR(T4.EVALUATEDEPT, 0, LENGTH(T2.ENCODE)) = T2.ENCODE " +
                            "      LEFT JOIN T2 TS " +
                            "        ON SUBSTR(T4.DEPARTMENTCODE, 0, LENGTH(TS.ENCODE)) = TS.ENCODE ) ORDER BY CREATEDATE, ID";
                dt = this.BaseRepository().FindTable(sql);
            }
            catch (Exception)
            {


            }
            return dt;
        }

        /// <summary>
        /// 导出外部部门考核
        /// </summary>
        /// <param name="time"></param>
        /// <param name="deptid"></param>
        /// <returns></returns>
        public DataTable ExportDataOutDept(string time, string deptid)
        {
            DataTable dt = null;
            Operator user = OperatorProvider.Provider.Current();
            try
            {
                string sql = "with t1 as"+
                               //" -- 查出承包商所有的部门(包括归下级部门)" +
                                " (select DEPARTMENTID, FULLNAME, ENCODE" +
                                  "  from base_department" +
                                  "  where (NATURE = '分包商'" +
                                  "    or NATURE = '承包商')" +
                                  "   and ORGANIZEID = '"+user.OrganizeId+"')," +
                              "  t2 as" +
                               //" --查出承包商最上层的部门" +
                               "  (select a.DEPARTMENTID, a.FULLNAME, a.ENCODE" +
                                   " from base_department a" +
                                   " where a.parentid in " +
                                         "(select DEPARTMENTID" +
                                           " from base_department t" +
                                           " where t.description like '%外包工程承包商%'" +
                                            " and ORGANIZEID = '" + user.OrganizeId + "'))," +
                               " t3 as" +
                               //" --查询所有部门(包括递归下级部门)下的人员" +
                               " (select USERID，ACCOUNT，realname，DEPARTMENTCODE" +
                                  "  from base_user a" +
                                  "  where a.DEPARTMENTID in (select DEPARTMENTID from t1))," +
                               " t4 as" +
                                //"--查询所有数据" +
                                 "(select b.CREATEDATE, " +
                                      "   a.EVALUATEDEPTNAME, " +
                                       "  a.EVALUATEDEPT, " +
                                        " a.EVALUATETYPE, " +
                                         "a.CREATEUSERDEPTCODE, " +
                                         "b.EXAMINEREASON, " +
                                         "b.EXAMINEBASIS, " +
                                         "b.EXAMINETYPENAME, " +
                                         "a.SCORETYPE, " +
                                         "a.SCORE, " +
                                         "a.EVALUATESCORE, " +
                                         "a.EVALUATECONTENT, " +
                                         "usr.departmentcode,b.EXAMINEDEPt, a.EVALUATEOTHER " +
                                    " from epg_safetyassessmentperson a" +
                                  "  inner" +
                                    " join epg_safetyassessment b" +
                                   " on a.safetyassessmentid = b.id" +
                                    " left" +
                                   " join base_user usr" +
                                    " on usr.userid = a.EVALUATEDEPT" +
                                   " where b.ISOVER = '1' and b.EXAMINEDEPTID in (select dept.DEPARTMENTID from base_department dept where dept.ENCODE like '" + deptid + "%' ) " +
                                  "   and(a.EVALUATEDEPT in (select ENCODE from t1) or" +
                                      "   a.EVALUATEDEPT in (select USERID from t3))" +
                                     " and b.CREATEDATE between" +
                                         " to_date('" + time + "-01 00:00:00','yyyy-mm-dd hh24:mi:ss') and" +
                                         " add_months(to_date('" + time + "-01 00:00:00','yyyy-mm-dd hh24:mi:ss'),1) " +
                                     " and a.CREATEUSERDEPTCODE like '"+user.OrganizeCode+"%')" +
                                "   select * from (select t4.*," +
                                      //" --为空时候查找人员" +
                                      " nvl(t2.FULLNAME, ts.FULLNAME) as bmname," +
                                      " nvl(t2.ENCODE, ts.ENCODE) as bmnamecode" +
                                 "  from t4" +
                                //"--关联部门查询最上级部门" +
                                 "  left join t2" +
                                  "  on substr(t4.EVALUATEDEPT, 0, length(t2.ENCODE)) = t2.ENCODE" +
                               //" -- 关联人员部门最上级部门" +
                                 " left join t2 ts" +
                                   " on substr(t4.departmentcode, 0, length(ts.ENCODE)) = ts.ENCODE ) order by bmnamecode,createdate "; 
                dt = this.BaseRepository().FindTable(sql);
            }
            catch (Exception)
            {


            }
            return dt;
        }


        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                Operator currUser = OperatorProvider.Provider.Current();
                if (!string.IsNullOrEmpty(queryJson))
                {
                    var queryParam = queryJson.ToJObject();
                    if (!queryParam["qtype"].IsEmpty())
                    {
                        pagination.conditionJson += " and (flowdept = '" + currUser.DeptId + "') and instr('" + currUser.RoleName + "',FLOWROLENAME) >0 and ISSAVED = 1 and ISOVER = 0 ";

                        //string[] arr = currUser.RoleName.Split(',');
                        //if (arr.Length > 0)
                        //{
                        //    pagination.conditionJson += " and (";
                        //    foreach (var item in arr)
                        //    {
                        //        pagination.conditionJson += string.Format(" flowrolename  like '%{0}%' or", item);
                        //    }
                        //    pagination.conditionJson = pagination.conditionJson.Substring(0, pagination.conditionJson.Length - 2);
                        //    pagination.conditionJson += " )";
                        //}
                        //pagination.conditionJson += string.Format(") and isover='0' and issaved='1')");
                    }
                    if (!queryParam["examinetype"].IsEmpty())
                    {
                        pagination.conditionJson += " and examinetype='" + queryParam["examinetype"].ToString() + "'";
                    }
                    if (!queryParam["examinereason"].IsEmpty())
                    {
                        pagination.conditionJson += " and examinereason like '%" + queryParam["examinereason"].ToString() + "%'";
                    }
                    if (!queryParam["examinetodeptid"].IsEmpty())
                    {
                        //pagination.conditionJson += " and EXAMINEDEPTID ='" + queryParam["examinetodeptid"].ToString() + "'";
                        pagination.conditionJson += " and  (t.id in (select hg.SAFETYASSESSMENTID from epg_safetyassessmentperson hg " +
                            "                           inner join base_user us on us.USERID = hg.EVALUATEDEPT " +
                            "                           where us.DEPARTMENTCODE  ='" + queryParam["examinetodeptid"].ToString() + "' " +
                            "                               and hg.EVALUATETYPE in ('2', '4'))" +
                            "                          or t.id in (select hg.SAFETYASSESSMENTID from " +
                            "                       epg_safetyassessmentperson hg where hg.EVALUATEDEPT ='" + queryParam["examinetodeptid"].ToString() + "'and hg.EVALUATETYPE in ('1', '3')))";
                    }
                    if (!queryParam["flowtype"].IsEmpty())
                    {
                        if (queryParam["flowtype"].ToString() == "1")
                        {
                            pagination.conditionJson += " and ISSAVED = '0' ";
                        }
                        else if (queryParam["flowtype"].ToString() == "2")
                        {
                            pagination.conditionJson += " and ISSAVED = '1' and ISOVER ='0' ";
                        }
                        else if (queryParam["flowtype"].ToString() == "3")
                        {
                            pagination.conditionJson += " and ISSAVED = '1' and ISOVER ='1' ";
                        }
                    }
                    //开始时间
                    if (!queryParam["sTime"].IsEmpty())
                    {
                        pagination.conditionJson += string.Format(@" and examinetime >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", queryParam["sTime"].ToString());
                    }
                    //结束时间
                    if (!queryParam["eTime"].IsEmpty())
                    {
                        pagination.conditionJson += string.Format(@" and examinetime < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(queryParam["eTime"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
                    }
                    if (!queryParam["contractid"].IsEmpty())
                    {
                        pagination.conditionJson += " and contractid='" + queryParam["contractid"].ToString() + "'";
                    }
                }

                DataTable dt = this.BaseRepository().FindTableByProcPager(pagination, DbHelper.DbType);

               
                return dt;
            }
            catch (System.Exception ex)
            {

                throw;
            }

        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SafetyAssessmentEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SafetyAssessmentEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public int GetFormJsontotal(string keyValue)
        {
            return this.BaseRepository().FindObject("select count(1) from Epg_Historysafetyassessment where CONTRACTID = '"+ keyValue + "' ").ToInt();
        }

        /// <summary>
        ///  获取编号，每月初始化后三位流水号
        /// </summary>
        /// <returns></returns>
        public string GetMaxCode()
        {
            var entity = this.BaseRepository().FindList(" select max(ExamineCode) as ExamineCode from epg_SafetyAssessment where to_char(CREATEDATE,'yyyymmdd')='" + DateTime.Now.ToString("yyyyMMdd") + "'").FirstOrDefault();
            if (entity == null || entity.EXAMINECODE == null)
                return DateTime.Now.ToString("yyyyMMdd") + "001";
            return (Int64.Parse(entity.EXAMINECODE) + 1).ToString();
        }

        /// <summary>
        /// 获取当前角色待审批的数量
        /// </summary>
        /// <returns></returns>
        public string  GetApplyNum()
        {
            Operator user = OperatorProvider.Provider.Current();
            string num = DbFactory.Base().FindObject("select count(1) from epg_SafetyAssessment where (flowdept = '" + user.DeptId + "') and instr('" + user.RoleName + "',FLOWROLENAME) >0 and ISSAVED = 1 and ISOVER = 0").ToString();
            return num;
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
        public void SaveForm(string keyValue, SafetyAssessmentEntity entity)
        {
            entity.ID = keyValue;
            //开始事务
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    SafetyAssessmentEntity se = this.BaseRepository().FindEntity(keyValue);
                    if (se == null)
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
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region  当前登录人是否有权限审核并获取下一次审核权限实体
        /// <summary>
        /// 当前登录人是否有权限审核并获取下一次审核权限实体
        /// </summary>
        /// <param name="currUser">当前登录人</param>
        /// <param name="state">是否有权限审核 1：能审核 0 ：不能审核</param>
        /// <param name="moduleName">模块名称</param>
        /// <param name="createdeptid">创建人部门ID</param>
        /// <returns>null-当前为最后一次审核,ManyPowerCheckEntity：下一次审核权限实体</returns>
        public ManyPowerCheckEntity CheckAuditPower(Operator currUser, out string state, string moduleName, string createdeptid,string startnum)
        {
            ManyPowerCheckEntity nextCheck = null;//下一步审核
            List<ManyPowerCheckEntity> powerList = powerCheck.GetListBySerialNum(currUser.OrganizeCode, moduleName);

            if (powerList.Count > 0)
            {
                //先查出执行部门编码
                for (int i = 0; i < powerList.Count; i++)
                {
                    if (powerList[i].CHECKDEPTCODE == "-3" || powerList[i].CHECKDEPTID == "-3")
                    {
                        var createdeptentity = new DepartmentService().GetEntity(createdeptid);
                        //  当是专业和班组的时候，一直往上找一直到查到部门
                        while (createdeptentity.Nature == "专业" || createdeptentity.Nature == "班组")
                        {
                            createdeptentity = new DepartmentService().GetEntity(createdeptentity.ParentId);
                        }
                        // 如果是部门和厂级，直接选中当前部门
                        if (createdeptentity.Nature == "部门" || createdeptentity.Nature == "厂级")
                        {
                            createdeptentity = new DepartmentService().GetEntity(createdeptentity.DepartmentId);
                        }

                        powerList[i].CHECKDEPTCODE = createdeptentity.EnCode;
                        powerList[i].CHECKDEPTID = createdeptentity.DepartmentId;

                        
                        
                    }
                }
                List<ManyPowerCheckEntity> checkPower = new List<ManyPowerCheckEntity>();

                //登录人是否有审核权限--有审核权限直接审核通过
                for (int i = 0; i < powerList.Count; i++)
                {
                    if (powerList[i].CHECKDEPTID == currUser.DeptId)
                    {
                        var rolelist = currUser.RoleName.Split(',');
                        for (int j = 0; j < rolelist.Length; j++)
                        {
                            if (powerList[i].CHECKROLENAME.Contains(rolelist[j]))
                            {
                                checkPower.Add(powerList[i]);
                                break;
                            }
                        }
                    }
                }

                powerList.GroupBy(t => t.SERIALNUM).ToList().Count();

                if (checkPower.Count > 0 && startnum !="0")
                {
                    state = "1";
                    ManyPowerCheckEntity check = checkPower.Last();//当前

                    for (int i = 0; i < powerList.Count; i++)
                    {
                        if (check.ID == powerList[i].ID)
                        {
                            if ((i + 1) >= powerList.Count)
                            {
                                nextCheck = null;
                            }
                            else
                            {
                                nextCheck = powerList[i + 1];
                            }
                        }
                    }
                }
                else
                {
                    state = "0";
                    nextCheck = powerList.First();
                }

                //if (null != nextCheck)
                //{
                //    //当前审核序号下的对应集合
                //    var serialList = powerList.Where(p => p.SERIALNUM == nextCheck.SERIALNUM);
                //    //集合记录大于1，则表示存在并行审核（审查）的情况
                //    if (serialList.Count() > 1)
                //    {
                //        string flowdept = string.Empty;  // 存取值形式 a1,a2
                //        string flowdeptname = string.Empty; // 存取值形式 b1,b2
                //        string flowrole = string.Empty;   // 存取值形式 c1|c2|  (c1数据构成： cc1,cc2,cc3)
                //        string flowrolename = string.Empty; // 存取值形式 d1|d2| (d1数据构成： dd1,dd2,dd3)

                //        ManyPowerCheckEntity slastEntity = new ManyPowerCheckEntity();
                //        slastEntity = serialList.LastOrDefault();
                //        foreach (ManyPowerCheckEntity model in serialList)
                //        {
                //            flowdept += model.CHECKDEPTID + ",";
                //            flowdeptname += model.CHECKDEPTNAME + ",";
                //            flowrole += model.CHECKROLEID + "|";
                //            flowrolename += model.CHECKROLENAME + "|";
                //        }
                //        if (!flowdept.IsEmpty())
                //        {
                //            slastEntity.CHECKDEPTID = flowdept.Substring(0, flowdept.Length - 1);
                //        }
                //        if (!flowdeptname.IsEmpty())
                //        {
                //            slastEntity.CHECKDEPTNAME = flowdeptname.Substring(0, flowdeptname.Length - 1);
                //        }
                //        if (!flowdept.IsEmpty())
                //        {
                //            slastEntity.CHECKROLEID = flowrole.Substring(0, flowrole.Length - 1);
                //        }
                //        if (!flowdept.IsEmpty())
                //        {
                //            slastEntity.CHECKROLENAME = flowrolename.Substring(0, flowrolename.Length - 1);
                //        }
                //        nextCheck = slastEntity;
                //    }
                //}
                return nextCheck;
            }
            else
            {
                state = "0";
                return nextCheck;
            }

        }

        /// <summary>
        /// 查询审核流程图
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="urltype">查询类型：0：安全考核</param>
        /// <returns></returns>
        public Flow GetAuditFlowData(string keyValue, string urltype)
        {
            List<nodes> nlist = new List<nodes>();
            List<lines> llist = new List<lines>();
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string moduleno = string.Empty;
            string table = string.Empty;
            moduleno = "AQKHSH";
            DataTable dt = new DataTable();
            table = string.Format(@"left join epg_aptitudeinvestigateaudit b on t.id = b.flowid and b.aptitudeid = '{0}' ", keyValue);
            string flowSql = string.Format(@"select d.FLOWDEPT,flowdept flowdeptid, g.FULLNAME, d.FLOWDEPTNAME,d.FLOWROLENAME,d.issaved,d.isover, t.flowname,t.id,t.serialnum,t.checkrolename,t.checkroleid,t.checkdeptid,t.checkdeptcode,
                                            b.auditresult,b.auditdept,b.auditpeople,b.audittime,t.checkdeptname,t.applytype,t.scriptcurcontent,t.choosedeptrange
                                             from  bis_manypowercheck t {2}   left join epg_safetyassessment d on d.flowid =  t.id and d.id = '{3}'
                                             left join base_department g on d.FLOWDEPT = g.DEPARTMENTID
                                             where t.createuserorgcode='{1}' and t.moduleno='{0}' order by t.serialnum asc", moduleno, user.OrganizeCode, table, keyValue);

            dt = this.BaseRepository().FindTable(flowSql);
            //DataTable nodeDt = GetCheckInfo(KeyValue);
            //JobSafetyCardApplyEntity entity = GetEntity(KeyValue);
            Flow flow = new Flow();
            flow.title = "";
            flow.initNum = 22;
            int end = 0;
            string endcreatedate = "";
            string endcreatdept = "";
            string endcreateuser = "";
            string endflowname = "";
            if (dt.Rows.Count > 0)
            {
                #region 

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    nodes nodes = new nodes();
                    nodes.alt = true;
                    nodes.isclick = false;
                    nodes.css = "";
                    nodes.id = dr["id"].ToString(); //主键
                    nodes.img = "";
                    nodes.name = dr["flowname"].ToString();
                    endflowname = dr["flowname"].ToString();
                    nodes.type = "stepnode";
                    //位置
                    int m = i % 4;
                    int n = i / 4;
                    if (m == 0)
                    {
                        nodes.left = 120;
                    }
                    else
                    {
                        nodes.left = 120 + ((150 + 60) * m);
                    }
                    if (n == 0)
                    {
                        nodes.top = 54;
                    }
                    else
                    {
                        nodes.top = (n * 100) + 54;
                    }
                    setInfo sinfo = new setInfo();
                    sinfo.NodeName = nodes.name;


                    //var emptyele = dangerousjoboperateservice.GetList("").Where(t => t.RecId == KeyValue && t.OperateType == 2).OrderByDescending(t => t.CreateDate).FirstOrDefault();
                    if (dr["auditresult"].ToString() == "0" || dr["isover"].ToString() == "1") 
                    {
                        sinfo.Taged = 1;
                        List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                        nodedesignatedata.createdate = Convert.ToDateTime(dr["audittime"].ToString()).ToString("yyyy-MM-dd HH:mm");
                        endcreatedate = Convert.ToDateTime(dr["audittime"].ToString()).ToString("yyyy-MM-dd HH:mm");
                        nodedesignatedata.creatdept = dr["auditdept"].ToString();
                        endcreatdept = dr["auditdept"].ToString();
                        nodedesignatedata.createuser = dr["auditpeople"].ToString();
                        endcreateuser = dr["auditpeople"].ToString();
                        nodedesignatedata.status = "同意";
                        if (nlist.Count == 0)
                        {
                            nodedesignatedata.prevnode = "无";
                        }
                        else
                        {
                            nodedesignatedata.prevnode = nlist[nlist.Count - 1].name;
                        }
                        nodelist.Add(nodedesignatedata);
                        sinfo.NodeDesignateData = nodelist;
                        nodes.setInfo = sinfo;
                        end = 1;

                    }
                    else if (dr["issaved"].ToString() == "1")
                    {
                        sinfo.Taged = 0;
                        List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                        nodedesignatedata.createdate = "无";
                        //DataTable dtuser = userservice.GetUserTable(entity.PowerCutPersonId.Split(','));
                        //string[] usernames = dtuser.AsEnumerable().Select(d => d.Field<string>("realname")).ToArray();
                        //string[] deptnames = dtuser.AsEnumerable().Select(d => d.Field<string>("deptname")).ToArray().GroupBy(t => t).Select(p => p.Key).ToArray();
                        //nodedesignatedata.createuser = usernames.Length > 0 ? string.Join(",", usernames) : "无";
                        //nodedesignatedata.creatdept = deptnames.Length > 0 ? string.Join(",", deptnames) : "无";
                        nodedesignatedata.createdate = "无";
                        nodedesignatedata.creatdept = dr["fullname"].ToString();
                        nodedesignatedata.createuser = this.BaseRepository().FindObject(" select wm_concat(realname)  from base_user where departmentid = '" + dr["flowdeptid"].ToString() + "' and rolename like '%负责人%' ").ToString() ;
                        nodedesignatedata.status = dr["flowname"].ToString(); ;
                        if (nlist.Count == 0)
                        {
                            nodedesignatedata.prevnode = "无";
                        }
                        else
                        {
                            nodedesignatedata.prevnode = nlist[nlist.Count - 1].name;
                        }
                        nodelist.Add(nodedesignatedata);
                        sinfo.NodeDesignateData = nodelist;
                        nodes.setInfo = sinfo;
                    }
                    nlist.Add(nodes);
                }

                #endregion



                #region 创建node对象

                //流程结束节点
                nodes nodes_end = new nodes();
                nodes_end.alt = true;
                nodes_end.isclick = false;
                nodes_end.css = "";
                nodes_end.id = Guid.NewGuid().ToString();
                nodes_end.img = "";
                nodes_end.name = "流程结束";
                nodes_end.type = "endround";
                nodes_end.width = 150;
                nodes_end.height = 60;
                //取最后一流程的位置，相对排位
                nodes_end.left = nlist[nlist.Count - 1].left;
                nodes_end.top = nlist[nlist.Count - 1].top + 100;
                nlist.Add(nodes_end);

                if (end == 1)
                {
                    setInfo sinfo = new setInfo();
                    sinfo.NodeName = nodes_end.name;
                    sinfo.Taged = 1;
                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                    NodeDesignateData nodedesignatedata = new NodeDesignateData();

                    //取流程结束时的节点信息
                    nodedesignatedata.createdate = endcreatedate;
                    nodedesignatedata.creatdept = endcreatdept;
                    nodedesignatedata.createuser = endcreateuser;
                    nodedesignatedata.status = "同意";
                    nodedesignatedata.prevnode = endflowname;

                    nodelist.Add(nodedesignatedata);
                    sinfo.NodeDesignateData = nodelist;
                    nodes_end.setInfo = sinfo;
                }

                #endregion

                #region 创建line对象

                for (int i = 0; i < nlist.Count; i++)
                {
                    lines lines = new lines();
                    lines.alt = true;
                    lines.id = Guid.NewGuid().ToString();
                    lines.from = nlist[i].id;
                    if (i < nlist.Count - 1)
                    {
                        lines.to = nlist[i + 1].id;
                    }
                    lines.name = "";
                    lines.type = "sl";
                    llist.Add(lines);
                }

                lines lines_end = new lines();
                lines_end.alt = true;
                lines_end.id = Guid.NewGuid().ToString();
                lines_end.from = nlist[nlist.Count - 1].id;
                lines_end.to = nodes_end.id;
                llist.Add(lines_end);
                #endregion

                flow.nodes = nlist;
                flow.lines = llist;
            }
            return flow;

        }
        #endregion
    }
}
