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
    /// 描 述：五定安全检查
    /// </summary>
    public class FivesafetycheckService : RepositoryFactory<FivesafetycheckEntity>, FivesafetycheckIService
    {
        #region 获取数据
        #region 简单的识别输入部门名称最大部门的可能性 刘畅 2020/12/21 (后续推荐将数据量提高，提高用词频提高正确率)
        /// <summary>
        ///  输入名称和部门集合,返回最相似的词语
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetDeptByName(string name)
        {
            DataTable dt = this.BaseRepository().FindTable("select fullname from base_department where fullname is not null"); //为了减少中文计算量，使用已有部门做成字组
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

            // 长度不要超过10个，防止速度过慢
            if (result == "" && name.Length <= 10)
            {
                // 由于汉字遍历太多了，第二次计算距离，只算前10条数据进行二次  刘畅 后续需要优化
                // 2020/12/22 换成索引的查找，速度提升
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
        /// 将不同情况的距离的数据查询出来  刘畅 
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
        /// 换成dictionary，走索引提升速度
        /// </summary>
        /// <param name="word"></param>
        /// <param name="alpha"></param>
        /// <param name="resultV"></param>
        /// <returns></returns>
        public Dictionary<string, int> WordEdit_v2(string word, string alpha, Dictionary<string, int> resultV)
        {
            Dictionary<string, int> result = resultV;

            int length = word.Length;

            // 删除
            for (int i = 0; i < word.Length; i++)
            {
                string res = word.Remove(i, 1);
                if (!result.ContainsKey(res))
                {
                    result.Add(res, 0);
                }

            }
            // 调换
            for (int i = 0; i < word.Length - 1; i++)
            {

                string res = word.Substring(0, i) + word[i + 1] + word[i] + word.Substring(i + 2);
                if (!result.ContainsKey(res))
                {
                    result.Add(res, 0);
                }

            }
            // 替换
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
            //增加
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
        /// 根据检查类型编号查询首页
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
        /// 返回安全考核不同类型待审批的数量的数量
        /// </summary>
        /// <param name="fivetype">检查类型</param>
        /// <param name="istopcheck"> 0:上级公司检查 1：公司安全检查</param>
        /// <param name="type"> 0:审核流程，1：整改  2：验收</param>
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
        /// 根据sql查出返回集合
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetInfoBySql(string sql)
        {
            return this.BaseRepository().FindTable(sql);
        }
        /// <summary>
        /// 导出查询数据
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
        /// 查询审核流程图 
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="urltype">查询类型：0：安全考核</param>
        /// <returns></returns>
        public Flow GetAuditFlowData(string keyValue, string urltype)
        {
            // 整体分两步流程前面审批流程，后面整改流程
            List<nodes> nlist = new List<nodes>();
            List<lines> llist = new List<lines>();
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            FivesafetycheckEntity fivecheckinfo = GetEntity(keyValue);
            string moduleno = string.Empty;
            string table = string.Empty;
            moduleno = "WDAQJC";
            string moduleName = "五定安全检查";
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DataTable dt = new DataTable();
            table = string.Format(@"left join epg_aptitudeinvestigateaudit b on t.id = b.flowid and b.aptitudeid = '{0}' ", keyValue);
            string flowSql = string.Format(@"select d.FLOWDEPT,flowdept flowdeptid, g.FULLNAME, d.FLOWDEPTNAME,d.FLOWROLENAME,d.issaved,d.isover, t.flowname,t.id,t.serialnum,t.checkrolename,t.checkroleid,t.checkdeptid,t.checkdeptcode,
                                            b.auditresult,b.auditdept,b.auditpeople,b.audittime,t.checkdeptname,t.applytype,t.scriptcurcontent,t.choosedeptrange
                                             from  bis_manypowercheck t {2}   left join bis_fivesafetycheck d on d.flowid =  t.id and d.id = '{3}'
                                             left join base_department g on d.FLOWDEPT = g.DEPARTMENTID
                                             where t.createuserorgcode='{1}' and b.REMARK <>  '-1' and t.moduleno='{0}' order by t.serialnum asc", moduleno, user.OrganizeCode, table, keyValue);

           

            dt = this.BaseRepository().FindTable(flowSql);
            // 获取步骤基本信息
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
                #region 获取流程信息步骤


                for (int i = 0; i < powerList.Count; i++)
                {
                    ManyPowerCheckEntity checinfo = powerList[i];
                    nodes nodes = new nodes();
                    nodes.alt = true;
                    nodes.isclick = false;
                    nodes.css = "";
                    nodes.id = checinfo.ID; //主键
                    nodes.img = "";
                    nodes.name = checinfo.FLOWNAME;
                    endflowname = checinfo.FLOWNAME;
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

                    //  如果检查信息还在审核中，根据主表的流程判断当前流程在哪里
                    if (fivecheckinfo.ISOVER == 0)
                    {
                        ManyPowerCheckEntity nowstep = new ManyPowerCheckService().GetEntity(fivecheckinfo.FLOWID); // 获取数据当前的流程的信息

                        if (nowstep.SERIALNUM > checinfo.SERIALNUM)// 当前检查的顺序号大于遍历的数据，说明遍历当前流程已走完
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
                            //end = 1;

                        }
                        else //反之遍历当前流程未走完,找脚本人员信息
                        {
                            string creatdeptnode = "";
                            string createusernode = "";
                            if (checinfo.SERIALNUM == 1) //判断是否会签流程
                            {
                                DataTable resultdata;
                                var parameter = new List<DbParameter>();
                                //取脚本，获取账户的范围信息
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
                                                       and a.rolename like '%负责人%'";
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
                            else //走正常流程
                            {
                                // 后续使用通用方法获取到审核人
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
                            nodedesignatedata.createdate = "无";
                            nodedesignatedata.creatdept = creatdeptnode;
                            nodedesignatedata.createuser = createusernode;
                            nodedesignatedata.status = checinfo.FLOWNAME;
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
                            if (nowstep.SERIALNUM >= checinfo.SERIALNUM) //只有当前的流程大于或等于遍历的流程，才展示详情
                            {
                                nodes.setInfo = sinfo;
                            }

                        }
                    }
                    else if (fivecheckinfo.ISOVER > 0) //审核流程走完
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
                        //end = 1;
                    }

                    nlist.Add(nodes);
                }

                #endregion


                #region 整改节点
                nodes nodeszg = new nodes();
                nodeszg.alt = true;
                nodeszg.isclick = false;
                nodeszg.css = "";
                nodeszg.id = "zg001"; //主键
                nodeszg.img = "";
                nodeszg.name = "检查问题整改";
                nodeszg.type = "stepnode";
                //位置
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
                    nodedesignatedatazg.prevnode = "无";
                }
                else
                {
                    nodedesignatedatazg.prevnode = nlist[nlist.Count - 1].name;
                }
                nodelistzg.Add(nodedesignatedatazg);
                sinfozg.NodeDesignateData = nodelistzg;


                if (fivecheckinfo.ISOVER == 1) // 整改中
                {
                    sinfozg.Taged = 0;
                    nodedesignatedatazg.createdate = "";
                    nodedesignatedatazg.status = "无";
                }
                else if (fivecheckinfo.ISOVER > 1) // 整改完成
                {
                    sinfozg.Taged = 1;
                    nodedesignatedatazg.status = "同意";
                    nodedesignatedatazg.createdate = this.BaseRepository().FindObject("select ACTUALDATE from (SELECT ACTUALDATE from bis_fivesafetycheckaudit where checkid = '" + keyValue + "' order by ACTUALDATE desc) where rownum = 1").ToString(); ;
                }
                if (fivecheckinfo.ISOVER > 0)
                {
                    nodeszg.setInfo = sinfozg;
                }

                nlist.Add(nodeszg);
                #endregion

                #region 验收节点
                nodes nodesys = new nodes();
                nodesys.alt = true;
                nodesys.isclick = false;
                nodesys.css = "";
                nodesys.id = "ys001"; //主键
                nodesys.img = "";
                nodesys.name = "检查问题验收";
                nodesys.type = "stepnode";
                //位置
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
                    nodedesignatedatays.prevnode = "无";
                }
                else
                {
                    nodedesignatedatays.prevnode = nlist[nlist.Count - 1].name;
                }
                nodelistys.Add(nodedesignatedatays);
                sinfoys.NodeDesignateData = nodelistys;


                if (fivecheckinfo.ISOVER == 2) // 验收中
                {
                    sinfoys.Taged = 0;
                    nodedesignatedatays.createdate = "";
                    nodedesignatedatays.status = "无";
                }
                else if (fivecheckinfo.ISOVER > 2) // 验收完成
                {
                    sinfoys.Taged = 1;
                    nodedesignatedatays.status = "同意";
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

                if (fivecheckinfo.ISOVER == 3)
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
            else // 未配置流程图展示后半部分
            {
                #region 整改节点
                nodes nodeszg = new nodes();
                nodeszg.alt = true;
                nodeszg.isclick = false;
                nodeszg.css = "";
                nodeszg.id = "zg001"; //主键
                nodeszg.img = "";
                nodeszg.name = "检查问题整改";
                nodeszg.type = "stepnode";
                //位置
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
                    nodedesignatedatazg.prevnode = "无";
                }
                else
                {
                    nodedesignatedatazg.prevnode = nlist[nlist.Count - 1].name;
                }
                nodelistzg.Add(nodedesignatedatazg);
                sinfozg.NodeDesignateData = nodelistzg;


                if (fivecheckinfo.ISOVER == 1) // 整改中
                {
                    sinfozg.Taged = 0;
                    nodedesignatedatazg.createdate = "";
                    nodedesignatedatazg.status = "无";
                }
                else if (fivecheckinfo.ISOVER > 1) // 整改完成
                {
                    sinfozg.Taged = 1;
                    nodedesignatedatazg.status = "同意";
                    nodedesignatedatazg.createdate = this.BaseRepository().FindObject("select ACTUALDATE from (SELECT ACTUALDATE from bis_fivesafetycheckaudit where checkid = '" + keyValue + "' order by ACTUALDATE desc) where rownum = 1").ToString(); ;
                }
                if (fivecheckinfo.ISOVER > 0)
                {
                    nodeszg.setInfo = sinfozg;
                }

                nlist.Add(nodeszg);
                #endregion

                #region 验收节点
                nodes nodesys = new nodes();
                nodesys.alt = true;
                nodesys.isclick = false;
                nodesys.css = "";
                nodesys.id = "ys001"; //主键
                nodesys.img = "";
                nodesys.name = "检查问题验收";
                nodesys.type = "stepnode";
                //位置
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
                    nodedesignatedatays.prevnode = "无";
                }
                else
                {
                    nodedesignatedatays.prevnode = nlist[nlist.Count - 1].name;
                }
                nodelistys.Add(nodedesignatedatays);
                sinfoys.NodeDesignateData = nodelistys;


                if (fivecheckinfo.ISOVER == 2) // 验收中
                {
                    sinfoys.Taged = 0;
                    nodedesignatedatays.createdate = "";
                    nodedesignatedatays.status = "无";
                }
                else if (fivecheckinfo.ISOVER > 2) // 验收完成
                {
                    sinfoys.Taged = 1;
                    nodedesignatedatays.status = "同意";
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

                if (fivecheckinfo.ISOVER == 3)
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

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<FivesafetycheckEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }

        /// <summary>
        /// 获取会签未审批部门
        /// </summary>
        /// <param name="powerinfo"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<UserEntity> GetStepDept(ManyPowerCheckEntity powerinfo,string id)
        {
            IEnumerable<UserEntity> result;
            var parameter = new List<DbParameter>();
            //取脚本，获取账户的范围信息
            parameter.Add(DbParameters.CreateDbParameter("@id", id));
            DbParameter[] arrayparam = parameter.ToArray();
            result = DbFactory.Base().FindList<UserEntity>(powerinfo.ScriptCurcontent, arrayparam);

            return result;
        }

        /// <summary>
        /// 获取列表分页
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
                // 代办
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
                // 关键字
                if (!queryParam["keyword"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and t.CHECKNAME like '%{0}%' ", queryParam["keyword"].ToString());
                }
                // 组织机构
                if (!queryParam["examinetodeptid"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and instr(CHECKEDDEPARTID,'{0}') >0 ", queryParam["examinetodeptid"].ToString());
                }
            }

            DataTable data = this.BaseRepository().FindTableByProcPager(pagination, dataType);

            for (int i = 0; i < data.Rows.Count; i++)
            {
                //获取下一步审核人
                string str = new ManyPowerCheckService().GetApproveUserId(data.Rows[i]["flowid"].ToString(), data.Rows[i]["id"].ToString(), "", "", "", "", "", "", "", "", "");
                data.Rows[i]["approveuserids"] = str;

            }



            return data;
        }



        /// <summary>
        /// 获取整改情况列表分页
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
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public FivesafetycheckEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, FivesafetycheckEntity entity)
        {
            entity.ID = keyValue;
            //开始事务
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
