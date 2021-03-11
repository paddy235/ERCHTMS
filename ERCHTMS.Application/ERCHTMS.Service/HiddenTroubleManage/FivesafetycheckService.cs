using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.IService.HiddenTroubleManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Util;

using BSFramework.Data;
using ERCHTMS.Code;
using BSFramework.Util.Extension;
using ERCHTMS.Service.BaseManage;
using System.Data.Common;
using ERCHTMS.Entity.BaseManage;
using System;

namespace ERCHTMS.Service.HiddenTroubleManage
{
    /// <summary>
    /// �� �����嶨��ȫ���
    /// </summary>
    public class FivesafetycheckService : RepositoryFactory<FivesafetycheckEntity>, FivesafetycheckIService
    {
        #region ��ȡ����
        #region �򵥵�ʶ�����벿����������ŵĿ����� ���� 2020/12/21 (�����Ƽ�����������ߣ�����ô�Ƶ�����ȷ��)
        /// <summary>
        ///  �������ƺͲ��ż���,���������ƵĴ���
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetDeptByName(string name)
        {
            DataTable dt = this.BaseRepository().FindTable("select fullname from base_department where fullname is not null"); //Ϊ�˼������ļ�������ʹ�����в�����������
            string result = String.Empty;
            Dictionary<string, int> fc = new Dictionary<string, int>();

            string allword = string.Empty;

            foreach (DataRow line in dt.Rows)
            {
                if (fc.ContainsKey(line["fullname"].ToString()))
                {
                    int nums = 0;
                    if (fc.TryGetValue(line["fullname"].ToString(), out nums))
                    {
                        fc[line["fullname"].ToString()] = nums + 1;
                    }
                }
                else
                {
                    fc.Add(line["fullname"].ToString(), 1);
                }

                foreach (char v in line["fullname"].ToString())
                {
                    if (allword.IndexOf(v) == -1)
                    {
                        allword += v;
                    }
                }
            }

            List<string> dic = new List<string>();
            dic = WordEdit(name, allword, dic);

            foreach (var str in dic)
            {
                if (fc.ContainsKey(str))
                {
                    result = str;
                    break;
                }
            }

            // ���Ȳ�Ҫ����10������ֹ�ٶȹ���
            if (result == "" && name.Length <= 10)
            {
                // ���ں��ֱ���̫���ˣ��ڶ��μ�����룬ֻ��ǰ10�����ݽ��ж���  ���� ������Ҫ�Ż�
                // 2020/12/22 ���������Ĳ��ң��ٶ�����
                for (int jk = 0; jk < (dic.Count > 10 ? 10 : dic.Count); jk++)
                {
                    //List<string> dictwofinish = new List<string>();
                    Dictionary<string, int> dictwofinish = new Dictionary<string, int>();
                    dictwofinish = WordEdit_v2(dic[jk], allword, dictwofinish);


                    foreach (var str in dictwofinish.Keys)
                    {
                        if (fc.ContainsKey(str))
                        {
                            result = str;
                            break;
                        }
                    }

                    if (result != "")
                    {
                        break;
                    }

                }

            }

            return result;

        }

        /// <summary>
        /// ����ͬ����ľ�������ݲ�ѯ����  ���� 
        /// </summary>
        /// <param name="word"></param>
        /// <param name="alpha"></param>
        /// <param name="resultV"></param>
        /// <returns></returns>
        public List<string> WordEdit(string word, string alpha, List<string> resultV)
        {
            List<string> result = resultV;

            int length = word.Length;

            for (int i = 0; i < word.Length; i++)
            {
                string res = word.Remove(i, 1);
                if (result.IndexOf(res) == -1)
                {
                    result.Add(res);
                }

            }
            for (int i = 0; i < word.Length - 1; i++)
            {

                string res = word.Substring(0, i) + word[i + 1] + word[i] + word.Substring(i + 2);
                if (result.IndexOf(res) == -1)
                {
                    result.Add(res);
                }

            }
            for (int i = 0; i < word.Length; i++)
            {
                foreach (var a in alpha)
                {
                    string res = word.Substring(0, i) + a + word.Substring(i + 1);
                    if (result.IndexOf(res) == -1)
                    {
                        result.Add(res);
                    }
                }


            }
            for (int i = 0; i <= word.Length; i++)
            {
                foreach (var a in alpha)
                {
                    string res = word.Substring(0, i) + a + word.Substring(i);
                    if (result.IndexOf(res) == -1)
                    {
                        result.Add(res);
                    }
                }

            }


            return result;
        }

        /// <summary>
        /// ����dictionary�������������ٶ�
        /// </summary>
        /// <param name="word"></param>
        /// <param name="alpha"></param>
        /// <param name="resultV"></param>
        /// <returns></returns>
        public Dictionary<string, int> WordEdit_v2(string word, string alpha, Dictionary<string, int> resultV)
        {
            Dictionary<string, int> result = resultV;

            int length = word.Length;

            // ɾ��
            for (int i = 0; i < word.Length; i++)
            {
                string res = word.Remove(i, 1);
                if (!result.ContainsKey(res))
                {
                    result.Add(res, 0);
                }

            }
            // ����
            for (int i = 0; i < word.Length - 1; i++)
            {

                string res = word.Substring(0, i) + word[i + 1] + word[i] + word.Substring(i + 2);
                if (!result.ContainsKey(res))
                {
                    result.Add(res, 0);
                }

            }
            // �滻
            for (int i = 0; i < word.Length; i++)
            {
                foreach (var a in alpha)
                {
                    string res = word.Substring(0, i) + a + word.Substring(i + 1);
                    if (!result.ContainsKey(res))
                    {
                        result.Add(res, 0);
                    }
                }

            }
            //����
            for (int i = 0; i <= word.Length; i++)
            {
                foreach (var a in alpha)
                {
                    string res = word.Substring(0, i) + a + word.Substring(i);
                    if (!result.ContainsKey(res))
                    {
                        result.Add(res, 0);
                    }
                }

            }


            return result;
        }



        #endregion

        /// <summary>
        /// ���ݼ�����ͱ�Ų�ѯ��ҳ
        /// </summary>
        /// <param name="itemcode"></param>
        /// <returns></returns> 
        public DataTable DeskTotalByCheckType(string itemcode)
        {
            string sql = string.Format(@"      with t1 as
                                                (select t.ItemName, t.ItemValue
                                                   from BASE_DATAITEMDETAIL t
                                                  where t.enabledmark = 1
                                                    and t.deletemark = 0
                                                    and t.itemid in (select itemid
                                                                      from base_dataitem a
                                                                     where a.itemcode = '{0}')
                                                  order by sortcode),
                                               t2 as
                                                (select CHECKTYPEID, ACCEPTREUSLT,CHECKPASS, count(1) num
                                                   from bis_fivesafetycheckaudit a
                                                  inner join bis_fivesafetycheck b
                                                     on a.checkid = b.id where to_char(a.createdate,'yyyy') = to_char(sysdate,'yyyy')   and b.isover in ('1','2','3')
                                                  group by b.CHECKTYPEID, ACCEPTREUSLT,CHECKPASS),
                                               t3 as
                                                (select t1.ItemName,
                                                        nvl((select sum(num) from t2 where t2.CHECKTYPEID = t1.ItemValue),
                                                            0) total,
                                                        nvl((select sum(num)
                                                              from t2
                                                             where t2.CHECKTYPEID = t1.ItemValue
                                                               and (t2.ACCEPTREUSLT = '0' or t2.CHECKPASS='0')),
                                                            0) ysnum
                                                   from t1)
                                               select ItemName typename,
                                                      total,
                                                      ysnum,
                                                     decode(total, 0, 0, round((ysnum / total) * 100,2)) zgl
                                                 from t3 ", itemcode);


            return this.BaseRepository().FindTable(sql);
        }


        /// <summary>
        /// ���ذ�ȫ���˲�ͬ���ʹ�����������������
        /// </summary>
        /// <param name="fivetype">�������</param>
        /// <param name="istopcheck"> 0:�ϼ���˾��� 1����˾��ȫ���</param>
        /// <param name="type"> 0:������̣�1������  2������</param>
        /// <returns></returns>
        public string GetApplyNum(string fivetype,string istopcheck,string type)
        {
            int num = 0;
            Operator user = OperatorProvider.Provider.Current();

            if (type == "0")
            {
                string strCondition = "";
                strCondition = string.Format("  t.createuserorgcode='{0}' and t.isover='0' and t.issaved='1'", user.OrganizeCode);
                strCondition += string.Format(" and t.CHECKTYPEID ='{0}' ", fivetype);
                DataTable dataresult = BaseRepository().FindTable("select flowid,id from bis_fivesafetycheck t   where " + strCondition);
                for (int i = 0; i < dataresult.Rows.Count; i++)
                {
                    string str = new ManyPowerCheckService().GetApproveUserId(dataresult.Rows[i]["flowid"].ToString(), dataresult.Rows[i]["id"].ToString(), "", "", "", "", "", "", "", "", "");
                    if (str.Contains(user.UserId))
                    {
                        num++;
                    }
                }

            }
            else if (type == "1")
            {
                string sql = "select count(1) from bis_fivesafetycheckaudit t where t.CHECKPASS = '1' and t.checkid in (select id from bis_fivesafetycheck where ISOVER in ('1','2')) ";
                sql += string.Format(" and (t.actionresult <> '0' or t.actionresult is null)  and dutyuserid = '{0}'  ", user.UserId);
                num = BaseRepository().FindObject(sql).ToInt() ;

            }
            else if (type == "2")
            {
                string sql = "select count(1) from bis_fivesafetycheckaudit t where t.CHECKPASS = '1' and t.checkid in (select id from bis_fivesafetycheck where ISOVER in ('1','2')) ";
                sql += string.Format(" and t.actionresult = '0' and (t.ACCEPTREUSLT <> '0' or t.ACCEPTREUSLT is null)  and acceptuserid = '{0}'  ", user.UserId);
                num = BaseRepository().FindObject(sql).ToInt();

            }

            return num.ToString() ;
        }


        /// <summary>
        /// ����sql������ؼ���
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetInfoBySql(string sql)
        {
            return this.BaseRepository().FindTable(sql);
        }
        /// <summary>
        /// ������ѯ����
        /// </summary>
        /// <param name="keyvalue"></param>
        /// <returns></returns>
        public DataTable ExportAuditTotal(string keyvalue)
        {
            string sql = string.Format(@"select t.id,  t.createuserid,
                                   t.createuserdeptcode,
                                  t.createuserorgcode,
                                   t.createdate,
                                   t.createusername,
                                   t.modifydate,
                                   t.modifyuserid,
                                   t.modifyusername,
                                   t.findquestion,
                                   t.actioncontent,
                                   t.dutyusername,
                                   t.dutyuserid,
                                   t.dutydeptcode,
                                   t.dutydept,
                                   t.dutydeptid,
                                    to_char(t.finishdate,'yyyy-mm-dd') finishdate,
                                   t.acceptuser,
                                   t.acceptuserid,
                                   t.actionresult,
                                   to_char(t.actualdate,'yyyy-mm-dd') actualdate,
                                   t.beizhu,
                                   t.acceptreuslt,
                                   t.acceptcontent,
                                   t.checkid from  bis_fivesafetycheckaudit t where t.CHECKID ='{0}' order by  createdate,id desc", keyvalue);


            return this.BaseRepository().FindTable(sql);
        }


        /// <summary>
        /// ��ѯ�������ͼ 
        /// </summary>
        /// <param name="keyValue">����</param>
        /// <param name="urltype">��ѯ���ͣ�0����ȫ����</param>
        /// <returns></returns>
        public Flow GetAuditFlowData(string keyValue, string urltype)
        {
            // �������������ǰ���������̣�������������
            List<nodes> nlist = new List<nodes>();
            List<lines> llist = new List<lines>();
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            FivesafetycheckEntity fivecheckinfo = GetEntity(keyValue);
            string moduleno = string.Empty;
            string table = string.Empty;
            moduleno = "WDAQJC";
            string moduleName = "�嶨��ȫ���";
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DataTable dt = new DataTable();
            table = string.Format(@"left join epg_aptitudeinvestigateaudit b on t.id = b.flowid and b.aptitudeid = '{0}' ", keyValue);
            string flowSql = string.Format(@"select d.FLOWDEPT,flowdept flowdeptid, g.FULLNAME, d.FLOWDEPTNAME,d.FLOWROLENAME,d.issaved,d.isover, t.flowname,t.id,t.serialnum,t.checkrolename,t.checkroleid,t.checkdeptid,t.checkdeptcode,
                                            b.auditresult,b.auditdept,b.auditpeople,b.audittime,t.checkdeptname,t.applytype,t.scriptcurcontent,t.choosedeptrange
                                             from  bis_manypowercheck t {2}   left join bis_fivesafetycheck d on d.flowid =  t.id and d.id = '{3}'
                                             left join base_department g on d.FLOWDEPT = g.DEPARTMENTID
                                             where t.createuserorgcode='{1}' and b.REMARK <>  '-1' and t.moduleno='{0}' order by t.serialnum asc", moduleno, user.OrganizeCode, table, keyValue);

           

            dt = this.BaseRepository().FindTable(flowSql);
            // ��ȡ���������Ϣ
            List<ManyPowerCheckEntity> powerList = new ManyPowerCheckService().GetListBySerialNum(curUser.OrganizeCode, moduleName);
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
            if (powerList.Count > 0)
            {
                #region ��ȡ������Ϣ����


                for (int i = 0; i < powerList.Count; i++)
                {
                    ManyPowerCheckEntity checinfo = powerList[i];
                    nodes nodes = new nodes();
                    nodes.alt = true;
                    nodes.isclick = false;
                    nodes.css = "";
                    nodes.id = checinfo.ID; //����
                    nodes.img = "";
                    nodes.name = checinfo.FLOWNAME;
                    endflowname = checinfo.FLOWNAME;
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

                    //  ��������Ϣ��������У���������������жϵ�ǰ����������
                    if (fivecheckinfo.ISOVER == 0)
                    {
                        ManyPowerCheckEntity nowstep = new ManyPowerCheckService().GetEntity(fivecheckinfo.FLOWID); // ��ȡ���ݵ�ǰ�����̵���Ϣ

                        if (nowstep.SERIALNUM > checinfo.SERIALNUM)// ��ǰ����˳��Ŵ��ڱ��������ݣ�˵��������ǰ����������
                        {
                            sinfo.Taged = 1;

                            string createdatenode = "";
                            string creatdeptnode = "";
                            string createusernode = "";

                            int startnum = 0;
                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                if (dt.Rows[j]["id"].ToString() == checinfo.ID)
                                {
                                    if (startnum == 0)
                                    {
                                        creatdeptnode = dt.Rows[j]["auditdept"].ToString();
                                        createusernode = dt.Rows[j]["auditpeople"].ToString();
                                    }
                                    else
                                    {
                                        if (creatdeptnode.IndexOf(dt.Rows[j]["auditdept"].ToString()) == -1)
                                        {
                                            creatdeptnode += "," + dt.Rows[j]["auditdept"].ToString();
                                        }
                                        
                                        createusernode += "," + dt.Rows[j]["auditpeople"].ToString();
                                    }
                                    startnum++;
                                    createdatenode = Convert.ToDateTime(dt.Rows[j]["audittime"].ToString()).ToString("yyyy-MM-dd HH:mm");
                                }


                            }

                            List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                            NodeDesignateData nodedesignatedata = new NodeDesignateData();
                            nodedesignatedata.createdate = createdatenode;
                            //endcreatedate = createdatenode;
                            nodedesignatedata.creatdept = creatdeptnode;
                            endcreatdept = creatdeptnode;
                            nodedesignatedata.createuser = createusernode;
                            //endcreateuser = dr["auditpeople"].ToString();
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
                            //end = 1;

                        }
                        else //��֮������ǰ����δ����,�ҽű���Ա��Ϣ
                        {
                            string creatdeptnode = "";
                            string createusernode = "";
                            if (checinfo.SERIALNUM == 1) //�ж��Ƿ��ǩ����
                            {
                                DataTable resultdata;
                                var parameter = new List<DbParameter>();
                                //ȡ�ű�����ȡ�˻��ķ�Χ��Ϣ
                                parameter.Add(DbParameters.CreateDbParameter("@id", keyValue));
                                DbParameter[] arrayparam = parameter.ToArray();
                                string hqsql = @"with t1 as
                                                     (select CHECKEDDEPARTIDALL
                                                        from bis_fivesafetycheck
                                                       where id = @id),
                                                    t2 as
                                                     (select REGEXP_SUBSTR(t1.CHECKEDDEPARTIDALL, '[^,]+', 1, LEVEL) as deptid
                                                        from t1
                                                      connect by rownum <=
                                                                 LENGTH(t1.CHECKEDDEPARTIDALL) -
                                                                 LENGTH(regexp_replace(t1.CHECKEDDEPARTIDALL, ',', '')) + 1)
                                                   select a.USERID, a.REALNAME, b.DEPARTMENTID,b.FULLNAME
                                                      from base_user a left join base_department  b on a.departmentid = b.DEPARTMENTID
                                                     where a.DEPARTMENTID in (select deptid from t2 where deptid is not null)
                                                       and a.rolename like '%������%'";
                                resultdata = DbFactory.Base().FindTable(hqsql, arrayparam);
                                for (int j = 0; j < resultdata.Rows.Count; j++)
                                {
                                    if (j == 0)
                                    {
                                        creatdeptnode = resultdata.Rows[j]["fullname"].ToString();
                                        createusernode = resultdata.Rows[j]["realname"].ToString();
                                    }
                                    else
                                    {
                                        if (creatdeptnode.IndexOf(resultdata.Rows[j]["fullname"].ToString()) == -1)
                                        {
                                            creatdeptnode += "," + resultdata.Rows[j]["fullname"].ToString();
                                        }
                                        
                                        createusernode += "," + resultdata.Rows[j]["realname"].ToString();
                                    }
                                }
                            }
                            else //����������
                            {
                                // ����ʹ��ͨ�÷�����ȡ�������
                                string str = new ManyPowerCheckService().GetApproveUserId(checinfo.ID, keyValue, "", "", "", "", "", "", "", "", "");
                                if (str != "" && str != null)
                                {
                                    string[] applids = str.Split(',');
                                    for (int j = 0; j < applids.Length; j++)
                                    {
                                        if (applids[j] != "" && applids[j] != null)
                                        {
                                            var useapply = new UserService().GetEntity(applids[j]);
                                            var deptapply = new DepartmentService().GetEntity(useapply.DepartmentId);
                                            if (j == 0)
                                            {

                                                createusernode = useapply.RealName;
                                                creatdeptnode = deptapply.FullName;
                                            }
                                            else
                                            {
                                                
                                                createusernode += "," + useapply.RealName;
                                                if (creatdeptnode.IndexOf(deptapply.FullName) == -1)
                                                {
                                                    creatdeptnode += "," + deptapply.FullName;
                                                }
                                                
                                            }
                                        }

                                    }
                                }

                            }

                            sinfo.Taged = 0;
                            List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                            NodeDesignateData nodedesignatedata = new NodeDesignateData();
                            nodedesignatedata.createdate = "��";
                            nodedesignatedata.creatdept = creatdeptnode;
                            nodedesignatedata.createuser = createusernode;
                            nodedesignatedata.status = checinfo.FLOWNAME;
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
                            if (nowstep.SERIALNUM >= checinfo.SERIALNUM) //ֻ�е�ǰ�����̴��ڻ���ڱ��������̣���չʾ����
                            {
                                nodes.setInfo = sinfo;
                            }

                        }
                    }
                    else if (fivecheckinfo.ISOVER > 0) //�����������
                    {
                        sinfo.Taged = 1;

                        string createdatenode = "";
                        string creatdeptnode = "";
                        string createusernode = "";

                        int startnum = 0;
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            if (dt.Rows[j]["id"].ToString() == checinfo.ID)
                            {
                                if (startnum == 0)
                                {
                                    creatdeptnode = dt.Rows[j]["auditdept"].ToString();
                                    createusernode = dt.Rows[j]["auditpeople"].ToString();
                                }
                                else
                                {
                                    if (creatdeptnode.IndexOf(dt.Rows[j]["auditdept"].ToString()) == -1)
                                    {
                                        creatdeptnode += "," + dt.Rows[j]["auditdept"].ToString();
                                    }
                                    
                                    createusernode += "," + dt.Rows[j]["auditpeople"].ToString();
                                }
                                startnum++;
                                createdatenode = Convert.ToDateTime(dt.Rows[j]["audittime"].ToString()).ToString("yyyy-MM-dd HH:mm");
                            }


                        }

                        List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                        nodedesignatedata.createdate = createdatenode;
                        //endcreatedate = createdatenode;
                        nodedesignatedata.creatdept = creatdeptnode;
                        //endcreatdept = creatdeptnode;
                        nodedesignatedata.createuser = createusernode;
                        //endcreateuser = dr["auditpeople"].ToString();
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
                        //end = 1;
                    }

                    nlist.Add(nodes);
                }

                #endregion


                #region ���Ľڵ�
                nodes nodeszg = new nodes();
                nodeszg.alt = true;
                nodeszg.isclick = false;
                nodeszg.css = "";
                nodeszg.id = "zg001"; //����
                nodeszg.img = "";
                nodeszg.name = "�����������";
                nodeszg.type = "stepnode";
                //λ��
                int mzg = powerList.Count % 4;
                int nzg = powerList.Count / 4;
                if (mzg == 0)
                {
                    nodeszg.left = 120;
                }
                else
                {
                    nodeszg.left = 120 + ((150 + 60) * mzg);
                }
                if (nzg == 0)
                {
                    nodeszg.top = 54;
                }
                else
                {
                    nodeszg.top = (nzg * 100) + 54;
                }
                setInfo sinfozg = new setInfo();
                sinfozg.NodeName = nodeszg.name;
                List<NodeDesignateData> nodelistzg = new List<NodeDesignateData>();
                NodeDesignateData nodedesignatedatazg = new NodeDesignateData();

                nodedesignatedatazg.creatdept = this.BaseRepository().FindObject("select wm_concat(to_char(fullname)) from (select c.fullname from bis_fivesafetycheckaudit a inner join base_user b on a.DUTYUSERID = b.userid inner join base_department c on b.DEPARTMENTID = c.DEPARTMENTID where a.checkid = '" + keyValue + "' and a.CHECKPASS = '1' group by c.fullname)  ").ToString();
                //nodedesignatedatazg.createuser = this.BaseRepository().FindObject("SELECT wm_concat(to_char(DUTYUSERNAME)) from  bis_fivesafetycheckaudit where checkid = '" + keyValue + "' ").ToString();
                nodedesignatedatazg.createuser = this.BaseRepository().FindObject("SELECT wm_concat(to_char(DUTYUSERNAME)) from (SELECT DUTYUSERNAME from  bis_fivesafetycheckaudit where checkid ='" + keyValue + "' and CHECKPASS = '1' group by DUTYUSERNAME) ").ToString();


                if (nlist.Count == 0)
                {
                    nodedesignatedatazg.prevnode = "��";
                }
                else
                {
                    nodedesignatedatazg.prevnode = nlist[nlist.Count - 1].name;
                }
                nodelistzg.Add(nodedesignatedatazg);
                sinfozg.NodeDesignateData = nodelistzg;


                if (fivecheckinfo.ISOVER == 1) // ������
                {
                    sinfozg.Taged = 0;
                    nodedesignatedatazg.createdate = "";
                    nodedesignatedatazg.status = "��";
                }
                else if (fivecheckinfo.ISOVER > 1) // �������
                {
                    sinfozg.Taged = 1;
                    nodedesignatedatazg.status = "ͬ��";
                    nodedesignatedatazg.createdate = this.BaseRepository().FindObject("select ACTUALDATE from (SELECT ACTUALDATE from bis_fivesafetycheckaudit where checkid = '" + keyValue + "' order by ACTUALDATE desc) where rownum = 1").ToString(); ;
                }
                if (fivecheckinfo.ISOVER > 0)
                {
                    nodeszg.setInfo = sinfozg;
                }

                nlist.Add(nodeszg);
                #endregion

                #region ���սڵ�
                nodes nodesys = new nodes();
                nodesys.alt = true;
                nodesys.isclick = false;
                nodesys.css = "";
                nodesys.id = "ys001"; //����
                nodesys.img = "";
                nodesys.name = "�����������";
                nodesys.type = "stepnode";
                //λ��
                int mys = (powerList.Count + 1) % 4;
                int nys = (powerList.Count + 1) / 4;
                if (mys == 0)
                {
                    nodesys.left = 120;
                }
                else
                {
                    nodesys.left = 120 + ((150 + 60) * mys);
                }
                if (nys == 0)
                {
                    nodesys.top = 54;
                }
                else
                {
                    nodesys.top = (nys * 100) + 54;
                }
                setInfo sinfoys = new setInfo();
                sinfoys.NodeName = nodesys.name;
                List<NodeDesignateData> nodelistys = new List<NodeDesignateData>();
                NodeDesignateData nodedesignatedatays = new NodeDesignateData();

                nodedesignatedatays.creatdept = this.BaseRepository().FindObject("select wm_concat(to_char(fullname)) from (select c.fullname from bis_fivesafetycheckaudit a inner join base_user b on a.ACCEPTUSERID = b.userid inner join base_department c on b.DEPARTMENTID = c.DEPARTMENTID where a.checkid = '" + keyValue + "' and a.CHECKPASS = '1' group by c.fullname)  ").ToString();
                //nodedesignatedatays.createuser = this.BaseRepository().FindObject("SELECT wm_concat(to_char(ACCEPTUSER)) from  bis_fivesafetycheckaudit where checkid = '" + keyValue + "' ").ToString();
                nodedesignatedatays.createuser = this.BaseRepository().FindObject(" select wm_concat(to_char(ACCEPTUSER)) from ( SELECT ACCEPTUSER from  bis_fivesafetycheckaudit where checkid = '" + keyValue + "' and CHECKPASS = '1' group by ACCEPTUSER) ").ToString();

                if (nlist.Count == 0)
                {
                    nodedesignatedatays.prevnode = "��";
                }
                else
                {
                    nodedesignatedatays.prevnode = nlist[nlist.Count - 1].name;
                }
                nodelistys.Add(nodedesignatedatays);
                sinfoys.NodeDesignateData = nodelistys;


                if (fivecheckinfo.ISOVER == 2) // ������
                {
                    sinfoys.Taged = 0;
                    nodedesignatedatays.createdate = "";
                    nodedesignatedatays.status = "��";
                }
                else if (fivecheckinfo.ISOVER > 2) // �������
                {
                    sinfoys.Taged = 1;
                    nodedesignatedatays.status = "ͬ��";
                    nodedesignatedatays.createdate = this.BaseRepository().FindObject("select MODIFYDATE from (SELECT MODIFYDATE from bis_fivesafetycheckaudit where checkid = '" + keyValue + "' order by MODIFYDATE desc) where rownum = 1").ToString(); ;
                }

                if (fivecheckinfo.ISOVER > 1)
                {
                    nodesys.setInfo = sinfoys;
                }
                endcreatedate = nodedesignatedatays.createdate;
                endcreatdept = nodedesignatedatays.creatdept;
                endcreateuser = nodedesignatedatays.createuser;
                endflowname = nodedesignatedatays.prevnode;
                nlist.Add(nodesys);
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

                if (fivecheckinfo.ISOVER == 3)
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
            else // δ��������ͼչʾ��벿��
            {
                #region ���Ľڵ�
                nodes nodeszg = new nodes();
                nodeszg.alt = true;
                nodeszg.isclick = false;
                nodeszg.css = "";
                nodeszg.id = "zg001"; //����
                nodeszg.img = "";
                nodeszg.name = "�����������";
                nodeszg.type = "stepnode";
                //λ��
                int mzg = powerList.Count % 4;
                int nzg = powerList.Count / 4;
                if (mzg == 0)
                {
                    nodeszg.left = 120;
                }
                else
                {
                    nodeszg.left = 120 + ((150 + 60) * mzg);
                }
                if (nzg == 0)
                {
                    nodeszg.top = 54;
                }
                else
                {
                    nodeszg.top = (nzg * 100) + 54;
                }
                setInfo sinfozg = new setInfo();
                sinfozg.NodeName = nodeszg.name;
                List<NodeDesignateData> nodelistzg = new List<NodeDesignateData>();
                NodeDesignateData nodedesignatedatazg = new NodeDesignateData();

                nodedesignatedatazg.creatdept = this.BaseRepository().FindObject("select wm_concat(to_char(fullname)) from (select c.fullname from bis_fivesafetycheckaudit a inner join base_user b on a.DUTYUSERID = b.userid inner join base_department c on b.DEPARTMENTID = c.DEPARTMENTID where a.checkid = '" + keyValue + "' and a.CHECKPASS = '1' group by c.fullname)  ").ToString();
                //nodedesignatedatazg.createuser = this.BaseRepository().FindObject("SELECT wm_concat(to_char(DUTYUSERNAME)) from  bis_fivesafetycheckaudit where checkid = '" + keyValue + "' ").ToString();
                nodedesignatedatazg.createuser = this.BaseRepository().FindObject("SELECT wm_concat(to_char(DUTYUSERNAME)) from (SELECT DUTYUSERNAME from  bis_fivesafetycheckaudit where checkid ='" + keyValue + "' and CHECKPASS = '1' group by DUTYUSERNAME) ").ToString();

                if (nlist.Count == 0)
                {
                    nodedesignatedatazg.prevnode = "��";
                }
                else
                {
                    nodedesignatedatazg.prevnode = nlist[nlist.Count - 1].name;
                }
                nodelistzg.Add(nodedesignatedatazg);
                sinfozg.NodeDesignateData = nodelistzg;


                if (fivecheckinfo.ISOVER == 1) // ������
                {
                    sinfozg.Taged = 0;
                    nodedesignatedatazg.createdate = "";
                    nodedesignatedatazg.status = "��";
                }
                else if (fivecheckinfo.ISOVER > 1) // �������
                {
                    sinfozg.Taged = 1;
                    nodedesignatedatazg.status = "ͬ��";
                    nodedesignatedatazg.createdate = this.BaseRepository().FindObject("select ACTUALDATE from (SELECT ACTUALDATE from bis_fivesafetycheckaudit where checkid = '" + keyValue + "' order by ACTUALDATE desc) where rownum = 1").ToString(); ;
                }
                if (fivecheckinfo.ISOVER > 0)
                {
                    nodeszg.setInfo = sinfozg;
                }

                nlist.Add(nodeszg);
                #endregion

                #region ���սڵ�
                nodes nodesys = new nodes();
                nodesys.alt = true;
                nodesys.isclick = false;
                nodesys.css = "";
                nodesys.id = "ys001"; //����
                nodesys.img = "";
                nodesys.name = "�����������";
                nodesys.type = "stepnode";
                //λ��
                int mys = (powerList.Count + 1) % 4;
                int nys = (powerList.Count + 1) / 4;
                if (mys == 0)
                {
                    nodesys.left = 120;
                }
                else
                {
                    nodesys.left = 120 + ((150 + 60) * mys);
                }
                if (nys == 0)
                {
                    nodesys.top = 54;
                }
                else
                {
                    nodesys.top = (nys * 100) + 54;
                }
                setInfo sinfoys = new setInfo();
                sinfoys.NodeName = nodesys.name;
                List<NodeDesignateData> nodelistys = new List<NodeDesignateData>();
                NodeDesignateData nodedesignatedatays = new NodeDesignateData();

                nodedesignatedatays.creatdept = this.BaseRepository().FindObject("select wm_concat(to_char(fullname)) from (select c.fullname from bis_fivesafetycheckaudit a inner join base_user b on a.ACCEPTUSERID = b.userid inner join base_department c on b.DEPARTMENTID = c.DEPARTMENTID where a.checkid = '" + keyValue + "' and a.CHECKPASS = '1' group by c.fullname)  ").ToString();
                //nodedesignatedatays.createuser = this.BaseRepository().FindObject("SELECT wm_concat(to_char(ACCEPTUSER)) from  bis_fivesafetycheckaudit where checkid = '" + keyValue + "' ").ToString();
                nodedesignatedatays.createuser = this.BaseRepository().FindObject(" select wm_concat(to_char(ACCEPTUSER)) from ( SELECT ACCEPTUSER from  bis_fivesafetycheckaudit where checkid = '" + keyValue + "' and CHECKPASS = '1' group by ACCEPTUSER) ").ToString();

                if (nlist.Count == 0)
                {
                    nodedesignatedatays.prevnode = "��";
                }
                else
                {
                    nodedesignatedatays.prevnode = nlist[nlist.Count - 1].name;
                }
                nodelistys.Add(nodedesignatedatays);
                sinfoys.NodeDesignateData = nodelistys;


                if (fivecheckinfo.ISOVER == 2) // ������
                {
                    sinfoys.Taged = 0;
                    nodedesignatedatays.createdate = "";
                    nodedesignatedatays.status = "��";
                }
                else if (fivecheckinfo.ISOVER > 2) // �������
                {
                    sinfoys.Taged = 1;
                    nodedesignatedatays.status = "ͬ��";
                    nodedesignatedatays.createdate = this.BaseRepository().FindObject("select MODIFYDATE from (SELECT MODIFYDATE from bis_fivesafetycheckaudit where checkid = '" + keyValue + "' order by MODIFYDATE desc) where rownum = 1").ToString(); ;
                }

                if (fivecheckinfo.ISOVER > 1)
                {
                    nodesys.setInfo = sinfoys;
                }
                endcreatedate = nodedesignatedatays.createdate;
                endcreatdept = nodedesignatedatays.creatdept;
                endcreateuser = nodedesignatedatays.createuser;
                endflowname = nodedesignatedatays.prevnode;
                nlist.Add(nodesys);
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

                if (fivecheckinfo.ISOVER == 3)
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

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<FivesafetycheckEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }

        /// <summary>
        /// ��ȡ��ǩδ��������
        /// </summary>
        /// <param name="powerinfo"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<UserEntity> GetStepDept(ManyPowerCheckEntity powerinfo,string id)
        {
            IEnumerable<UserEntity> result;
            var parameter = new List<DbParameter>();
            //ȡ�ű�����ȡ�˻��ķ�Χ��Ϣ
            parameter.Add(DbParameters.CreateDbParameter("@id", id));
            DbParameter[] arrayparam = parameter.ToArray();
            result = DbFactory.Base().FindList<UserEntity>(powerinfo.ScriptCurcontent, arrayparam);

            return result;
        }

        /// <summary>
        /// ��ȡ�б��ҳ
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetPageListJson(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.p_kid = "t.id";
            pagination.p_fields = @" t.CHECKNAME,t.CHECKTYPE,to_char(t.CHECKBEGINDATE,'yyyy-mm-dd') CHECKBEGINDATE,t.FLOWID,'' as approveuserids,
                                       to_char(t.CHECKENDDATE,'yyyy-mm-dd') CHECKENDDATE ,t.flowdept,t.flowrolename,t.createuserdeptcode,t.createuserorgcode,t.createuserid,t.issaved,t.isover,
                                       (select count(1) from  BIS_FIVESAFETYCHECKAUDIT a where a.CHECKID = t.id) checkbnum,
                                        (select count(1) from  BIS_FIVESAFETYCHECKAUDIT a where a.CHECKID = t.id and (a.ACCEPTREUSLT = '0' or checkpass = '0') ) actionnum,
                                        (select count(1) from  BIS_FIVESAFETYCHECKAUDIT a where a.CHECKID = t.id and (a.ACCEPTREUSLT <> '0' or a.ACCEPTREUSLT is null) and checkpass = '1' and sysdate > a.FINISHDATE+1) cqactionnum";
            pagination.p_tablename = @" bis_fivesafetycheck t  ";

            pagination.conditionJson = " 1=1 ";

            if (!string.IsNullOrEmpty(queryJson))
            {
                var queryParam = queryJson.ToJObject();
                if (!queryParam["fivetype"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and t.CHECKTYPEID ='{0}' ", queryParam["fivetype"].ToString());
                }
                // ����
                if (!queryParam["qtype"].IsEmpty())
                {
                    string strCondition = "";
                    strCondition = string.Format(" and t.createuserorgcode='{0}' and t.isover='0' and t.issaved='1'", user.OrganizeCode);
                    DataTable dataresult = BaseRepository().FindTable("select " + pagination.p_kid + "," + pagination.p_fields + " from " + pagination.p_tablename + " where " + pagination.conditionJson + strCondition);
                    for (int i = 0; i < dataresult.Rows.Count; i++)
                    {
                        string str = new ManyPowerCheckService().GetApproveUserId(dataresult.Rows[i]["flowid"].ToString(), dataresult.Rows[i]["id"].ToString(), "", "", "", "", "", "", "", "", "");
                        dataresult.Rows[i]["approveuserids"] = str;

                    }

                    string[] applyids = dataresult.Select(" approveuserids like '%" + user.UserId + "%'").AsEnumerable().Select(d => d.Field<string>("id")).ToArray();

                    pagination.conditionJson += string.Format(" and t.id in ('{0}') {1}", string.Join("','", applyids), strCondition);
                }
                // �ؼ���
                if (!queryParam["keyword"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and t.CHECKNAME like '%{0}%' ", queryParam["keyword"].ToString());
                }
                // ��֯����
                if (!queryParam["examinetodeptid"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and instr(CHECKEDDEPARTID,'{0}') >0 ", queryParam["examinetodeptid"].ToString());
                }
            }

            DataTable data = this.BaseRepository().FindTableByProcPager(pagination, dataType);

            for (int i = 0; i < data.Rows.Count; i++)
            {
                //��ȡ��һ�������
                string str = new ManyPowerCheckService().GetApproveUserId(data.Rows[i]["flowid"].ToString(), data.Rows[i]["id"].ToString(), "", "", "", "", "", "", "", "", "");
                data.Rows[i]["approveuserids"] = str;

            }



            return data;
        }



        /// <summary>
        /// ��ȡ��������б��ҳ
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetAuditListJson(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.p_kid = "t.id";
            pagination.p_fields = @"t.createuserid,
                                   t.createuserdeptcode,
                                  t.createuserorgcode,
                                   t.createdate,
                                   t.createusername,
                                   t.modifydate,
                                   t.modifyuserid,
                                   t.modifyusername,
                                   t.findquestion,
                                   t.actioncontent,
                                   t.dutyusername,
                                   t.dutyuserid,
                                   t.dutydeptcode,
                                   t.dutydept,
                                   t.dutydeptid,
                                    to_char(t.finishdate,'yyyy-mm-dd') finishdate,
                                   t.acceptuser,
                                   t.acceptuserid,
                                   t.actionresult,
                                   to_char(t.actualdate,'yyyy-mm-dd') actualdate,
                                   t.beizhu,
                                   t.acceptreuslt,
                                   t.acceptcontent,
                                   t.checkid,t.checkpass";
            pagination.p_tablename = @" bis_fivesafetycheckaudit t  ";

            pagination.conditionJson = " 1=1 ";

            if (!string.IsNullOrEmpty(queryJson))
            {
                var queryParam = queryJson.ToJObject();
                if (!queryParam["checkid"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and t.CHECKID ='{0}' ", queryParam["checkid"].ToString());
                }

                if (!queryParam["status"].IsEmpty())
                {
                    if (queryParam["status"].ToString() == "0") // 
                    {
                        
                    }
                    else if (queryParam["status"].ToString() == "1")
                    {
                        pagination.conditionJson += " and  (t.ACCEPTREUSLT = '0' or checkpass = '0')  ";
                    }
                    else if (queryParam["status"].ToString() == "2")
                    {

                        pagination.conditionJson += " and  (t.ACCEPTREUSLT <> '0' or t.ACCEPTREUSLT is null) and checkpass = '1' and sysdate > t.FINISHDATE+1  ";
                    }
                }
            }



            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public FivesafetycheckEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
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
        public void SaveForm(string keyValue, FivesafetycheckEntity entity)
        {
            entity.ID = keyValue;
            //��ʼ����
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    FivesafetycheckEntity se = this.BaseRepository().FindEntity(keyValue);
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
    }
}
