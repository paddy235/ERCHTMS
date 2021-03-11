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
    /// �� ������ȫ��������
    /// </summary>
    public class SafetyAssessmentService : RepositoryFactory<SafetyAssessmentEntity>, SafetyAssessmentIService
    {

        private IManyPowerCheckService powerCheck = new ManyPowerCheckService();
        #region ��ȡ����
        /// <summary>
        /// ������ȫ���˻���
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
        /// ��ȡ�ڲ�����
        /// </summary>
        /// <returns></returns>
        public DataTable GetInDeptData()
        {
            DataTable dt = null;
            Operator user = OperatorProvider.Provider.Current();

            try
            {
                string sql = "SELECT DEPARTMENTID, FULLNAME, ENCODE FROM BASE_DEPARTMENT WHERE NATURE = '����' AND ORGANIZEID =  '" + user.OrganizeId + "' AND DESCRIPTION IS NULL";
                dt = this.BaseRepository().FindTable(sql);
            }
            catch (Exception)
            {


            }
            return dt;
        }

        /// <summary>
        /// �����ڲ���λ����
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
                               " WHERE(NATURE = '����' " +
                                "  OR NATURE = 'רҵ' OR NATURE = '����') " +
                                "  AND ORGANIZEID = '" + user.OrganizeId + "')," +
                           "  T2 AS " +
                            "  (SELECT DEPARTMENTID, FULLNAME, ENCODE FROM BASE_DEPARTMENT WHERE NATURE = '����' AND ORGANIZEID =  '" + user.OrganizeId + "' AND DESCRIPTION IS NULL), " +
                           " T3 AS " +
                           "  (SELECT USERID��ACCOUNT��REALNAME��DEPARTMENTCODE " +
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
        /// �����ⲿ���ſ���
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
                               //" -- ����а������еĲ���(�������¼�����)" +
                                " (select DEPARTMENTID, FULLNAME, ENCODE" +
                                  "  from base_department" +
                                  "  where (NATURE = '�ְ���'" +
                                  "    or NATURE = '�а���')" +
                                  "   and ORGANIZEID = '"+user.OrganizeId+"')," +
                              "  t2 as" +
                               //" --����а������ϲ�Ĳ���" +
                               "  (select a.DEPARTMENTID, a.FULLNAME, a.ENCODE" +
                                   " from base_department a" +
                                   " where a.parentid in " +
                                         "(select DEPARTMENTID" +
                                           " from base_department t" +
                                           " where t.description like '%������̳а���%'" +
                                            " and ORGANIZEID = '" + user.OrganizeId + "'))," +
                               " t3 as" +
                               //" --��ѯ���в���(�����ݹ��¼�����)�µ���Ա" +
                               " (select USERID��ACCOUNT��realname��DEPARTMENTCODE" +
                                  "  from base_user a" +
                                  "  where a.DEPARTMENTID in (select DEPARTMENTID from t1))," +
                               " t4 as" +
                                //"--��ѯ��������" +
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
                                      //" --Ϊ��ʱ�������Ա" +
                                      " nvl(t2.FULLNAME, ts.FULLNAME) as bmname," +
                                      " nvl(t2.ENCODE, ts.ENCODE) as bmnamecode" +
                                 "  from t4" +
                                //"--�������Ų�ѯ���ϼ�����" +
                                 "  left join t2" +
                                  "  on substr(t4.EVALUATEDEPT, 0, length(t2.ENCODE)) = t2.ENCODE" +
                               //" -- ������Ա�������ϼ�����" +
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
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
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
                    //��ʼʱ��
                    if (!queryParam["sTime"].IsEmpty())
                    {
                        pagination.conditionJson += string.Format(@" and examinetime >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", queryParam["sTime"].ToString());
                    }
                    //����ʱ��
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
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SafetyAssessmentEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
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
        ///  ��ȡ��ţ�ÿ�³�ʼ������λ��ˮ��
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
        /// ��ȡ��ǰ��ɫ������������
        /// </summary>
        /// <returns></returns>
        public string  GetApplyNum()
        {
            Operator user = OperatorProvider.Provider.Current();
            string num = DbFactory.Base().FindObject("select count(1) from epg_SafetyAssessment where (flowdept = '" + user.DeptId + "') and instr('" + user.RoleName + "',FLOWROLENAME) >0 and ISSAVED = 1 and ISOVER = 0").ToString();
            return num;
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, SafetyAssessmentEntity entity)
        {
            entity.ID = keyValue;
            //��ʼ����
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

        #region  ��ǰ��¼���Ƿ���Ȩ����˲���ȡ��һ�����Ȩ��ʵ��
        /// <summary>
        /// ��ǰ��¼���Ƿ���Ȩ����˲���ȡ��һ�����Ȩ��ʵ��
        /// </summary>
        /// <param name="currUser">��ǰ��¼��</param>
        /// <param name="state">�Ƿ���Ȩ����� 1������� 0 ���������</param>
        /// <param name="moduleName">ģ������</param>
        /// <param name="createdeptid">�����˲���ID</param>
        /// <returns>null-��ǰΪ���һ�����,ManyPowerCheckEntity����һ�����Ȩ��ʵ��</returns>
        public ManyPowerCheckEntity CheckAuditPower(Operator currUser, out string state, string moduleName, string createdeptid,string startnum)
        {
            ManyPowerCheckEntity nextCheck = null;//��һ�����
            List<ManyPowerCheckEntity> powerList = powerCheck.GetListBySerialNum(currUser.OrganizeCode, moduleName);

            if (powerList.Count > 0)
            {
                //�Ȳ��ִ�в��ű���
                for (int i = 0; i < powerList.Count; i++)
                {
                    if (powerList[i].CHECKDEPTCODE == "-3" || powerList[i].CHECKDEPTID == "-3")
                    {
                        var createdeptentity = new DepartmentService().GetEntity(createdeptid);
                        //  ����רҵ�Ͱ����ʱ��һֱ������һֱ���鵽����
                        while (createdeptentity.Nature == "רҵ" || createdeptentity.Nature == "����")
                        {
                            createdeptentity = new DepartmentService().GetEntity(createdeptentity.ParentId);
                        }
                        // ����ǲ��źͳ�����ֱ��ѡ�е�ǰ����
                        if (createdeptentity.Nature == "����" || createdeptentity.Nature == "����")
                        {
                            createdeptentity = new DepartmentService().GetEntity(createdeptentity.DepartmentId);
                        }

                        powerList[i].CHECKDEPTCODE = createdeptentity.EnCode;
                        powerList[i].CHECKDEPTID = createdeptentity.DepartmentId;

                        
                        
                    }
                }
                List<ManyPowerCheckEntity> checkPower = new List<ManyPowerCheckEntity>();

                //��¼���Ƿ������Ȩ��--�����Ȩ��ֱ�����ͨ��
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
                    ManyPowerCheckEntity check = checkPower.Last();//��ǰ

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
                //    //��ǰ�������µĶ�Ӧ����
                //    var serialList = powerList.Where(p => p.SERIALNUM == nextCheck.SERIALNUM);
                //    //���ϼ�¼����1�����ʾ���ڲ�����ˣ���飩�����
                //    if (serialList.Count() > 1)
                //    {
                //        string flowdept = string.Empty;  // ��ȡֵ��ʽ a1,a2
                //        string flowdeptname = string.Empty; // ��ȡֵ��ʽ b1,b2
                //        string flowrole = string.Empty;   // ��ȡֵ��ʽ c1|c2|  (c1���ݹ��ɣ� cc1,cc2,cc3)
                //        string flowrolename = string.Empty; // ��ȡֵ��ʽ d1|d2| (d1���ݹ��ɣ� dd1,dd2,dd3)

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
        /// ��ѯ�������ͼ
        /// </summary>
        /// <param name="keyValue">����</param>
        /// <param name="urltype">��ѯ���ͣ�0����ȫ����</param>
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
                    nodes.id = dr["id"].ToString(); //����
                    nodes.img = "";
                    nodes.name = dr["flowname"].ToString();
                    endflowname = dr["flowname"].ToString();
                    nodes.type = "stepnode";
                    //λ��
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
                        nodedesignatedata.status = "ͬ��";
                        if (nlist.Count == 0)
                        {
                            nodedesignatedata.prevnode = "��";
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
                        nodedesignatedata.createdate = "��";
                        //DataTable dtuser = userservice.GetUserTable(entity.PowerCutPersonId.Split(','));
                        //string[] usernames = dtuser.AsEnumerable().Select(d => d.Field<string>("realname")).ToArray();
                        //string[] deptnames = dtuser.AsEnumerable().Select(d => d.Field<string>("deptname")).ToArray().GroupBy(t => t).Select(p => p.Key).ToArray();
                        //nodedesignatedata.createuser = usernames.Length > 0 ? string.Join(",", usernames) : "��";
                        //nodedesignatedata.creatdept = deptnames.Length > 0 ? string.Join(",", deptnames) : "��";
                        nodedesignatedata.createdate = "��";
                        nodedesignatedata.creatdept = dr["fullname"].ToString();
                        nodedesignatedata.createuser = this.BaseRepository().FindObject(" select wm_concat(realname)  from base_user where departmentid = '" + dr["flowdeptid"].ToString() + "' and rolename like '%������%' ").ToString() ;
                        nodedesignatedata.status = dr["flowname"].ToString(); ;
                        if (nlist.Count == 0)
                        {
                            nodedesignatedata.prevnode = "��";
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



                #region ����node����

                //���̽����ڵ�
                nodes nodes_end = new nodes();
                nodes_end.alt = true;
                nodes_end.isclick = false;
                nodes_end.css = "";
                nodes_end.id = Guid.NewGuid().ToString();
                nodes_end.img = "";
                nodes_end.name = "���̽���";
                nodes_end.type = "endround";
                nodes_end.width = 150;
                nodes_end.height = 60;
                //ȡ���һ���̵�λ�ã������λ
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

                    //ȡ���̽���ʱ�Ľڵ���Ϣ
                    nodedesignatedata.createdate = endcreatedate;
                    nodedesignatedata.creatdept = endcreatdept;
                    nodedesignatedata.createuser = endcreateuser;
                    nodedesignatedata.status = "ͬ��";
                    nodedesignatedata.prevnode = endflowname;

                    nodelist.Add(nodedesignatedata);
                    sinfo.NodeDesignateData = nodelist;
                    nodes_end.setInfo = sinfo;
                }

                #endregion

                #region ����line����

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
