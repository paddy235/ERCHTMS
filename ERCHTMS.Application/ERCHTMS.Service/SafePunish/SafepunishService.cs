using System;
using System.Collections;
using ERCHTMS.Entity.SafePunish;
using ERCHTMS.IService.SafePunish;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Entity.SafeReward;
using ERCHTMS.Service.BaseManage;
using ERCHTMS.Service.OutsourcingProject;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ERCHTMS.Service.SystemManage;

namespace ERCHTMS.Service.SafePunish
{
    /// <summary>
    /// �� ������ȫ�ͷ�
    /// </summary>
    public class SafepunishService : RepositoryFactory<SafepunishEntity>, SafepunishIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SafepunishEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }

        /// <summary>
        /// ��ҳ��ѯ�ͷ���Ϣ
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;

            if (!string.IsNullOrEmpty(queryJson) && queryJson != "\"\"")
            {
                var queryParam = queryJson.ToJObject();

                //�ҵĹ�����¼
                if (!queryParam["pager"].IsEmpty() && queryParam["pager"].ToString() == "True")
                {
                    pagination.conditionJson += " and ((instr(ApproverPeopleIds,'" + OperatorProvider.Provider.Current().UserId + "' )> 0  and APPLYSTATE <> '0'  and FLOWSTATE ='0') or (CreateUserId = '" + OperatorProvider.Provider.Current().UserId + "'and APPLYSTATE = '0' and FLOWSTATE !='1' )) ";
                    //pagination.conditionJson += " and instr(ApproverPeopleIds,'" + OperatorProvider.Provider.Current().UserId + "' )> 0 and APPLYSTATE <> '0'  and FLOWSTATE ='0' ";
                }
                //ʱ�䷶Χ
                if (!queryParam["sTime"].IsEmpty() || !queryParam["eTime"].IsEmpty())
                {
                    string startTime = queryParam["sTime"].ToString() + " 00:00:00";
                    string endTime = queryParam["eTime"].ToString() + " 23:59:59";
                    if (queryParam["sTime"].IsEmpty())
                    {
                        startTime = "1899-01-01" + " 00:00:00";
                    }
                    if (queryParam["eTime"].IsEmpty())
                    {
                        endTime = DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59";
                    }
                    pagination.conditionJson += string.Format(" and applytime between to_date('{0}','yyyy-MM-dd HH24:mi:ss') and  to_date('{1}','yyyy-MM-dd HH24:mi:ss')", startTime, endTime);
                }
                //����״̬
                if (!queryParam["flowstate"].IsEmpty())
                {
                    string flowstate = queryParam["flowstate"].ToString().Trim();
                    pagination.conditionJson += string.Format(" and flowstate   = '{0}'", flowstate);
                }
                //��������
                if (!queryParam["amercetype"].IsEmpty())
                {
                    string amercetype = queryParam["amercetype"].ToString().Trim();
                    pagination.conditionJson += string.Format(" and amercetype   = '{0}'", amercetype);
                }
                ////��ѯ����
                //if (!queryParam["keyword"].IsEmpty())
                //{
                //    string keyord = queryParam["keyword"].ToString().Trim();
                //    pagination.conditionJson += string.Format(" and SafePunishCode  like '%{0}%' or  ApplyUserName like '%{0}%'", keyord);
                //}


            }

            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }                                
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SafepunishEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }



        /// <summary>
        ///��ȡͳ������
        /// </summary>
        /// <param name="year">���</param>
        /// <returns></returns>
        public string GetPunishStatisticsCount(string year, string statMode)
        {
            string ownorgcode = OperatorProvider.Provider.Current().OrganizeCode;
            string whereSQL = " and instr(a.createuserorgcode,'" + ownorgcode + "' )> 0 ";

            //����
            if (!string.IsNullOrEmpty(year))
            {
                whereSQL += " and to_char(APPLYTIME,'yyyy')='" + year + "'";
            }
            whereSQL += " and FLOWSTATE ='1'";
            List<object> dic = new List<object>();
            //string strsql = @"select itemvalue,itemname from base_dataitemdetail where itemid = (select itemid from base_dataitem where itemcode = 'AEM_WSSJTYPE') order by SORTCODE ";
            //DataTable dt = this.BaseRepository().FindTable(strsql);
            DataTable dtsqlde = new DataTable();
            dtsqlde.Columns.Add("value");
            dtsqlde.Columns.Add("text");
            for (int i = 1; i < 5; i++)
            {
                DataRow dtrow = dtsqlde.NewRow();
                dtrow["value"] = i.ToString();
                switch (i)
                {
                    case 1:
                        dtrow["text"] = "�¹��¼�";
                        break;
                    case 2:
                        dtrow["text"] = "����";
                        break;
                    case 3:
                        dtrow["text"] = "�����Ų�����";
                        break;
                    case 4:
                        dtrow["text"] = "�ճ�����";
                        break;
                    default:
                        break;
                }
                dtsqlde.Rows.Add(dtrow);
            }
            List<string> listmonths = new List<string>();
            for (int i = 1; i <= 12; i++)
            {
                listmonths.Add(i.ToString() + "��");
            }
            foreach (DataRow item in dtsqlde.Rows)
            {
                List<int> list = new List<int>();
                for (int i = 1; i <= 12; i++)
                {
                    string whereSQL2 = " and to_char(APPLYTIME,'mm')=" + i.ToString();
                    string forsql = "";
                    if (statMode == "0")
                    {
                         forsql = string.Format(@"select nvl(count(1),0) as cou from bis_safepunish a left join bis_safepunishdetail b on a.id=b.punishid  where amercetype='{0}' {1} {2}", item["value"], whereSQL, whereSQL2);
                    }
                    else if (statMode == "1")
                    {
                        forsql = string.Format(@"select nvl(sum(b.punishnum),0) as cou from bis_safepunish a left join bis_safepunishdetail b on a.id=b.punishid where amercetype='{0}' {1} {2}", item["value"], whereSQL, whereSQL2);
                    }
                    if (forsql != "")
                    {
                        int num = this.BaseRepository().FindObject(forsql).ToInt();
                        list.Add(num);
                    }   
                  
                }
                dic.Add(new { name = item["text"], data = list });
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { x = listmonths, y = dic });
        }


        /// <summary>
        ///��ȡͳ�Ʊ������
        /// </summary>
        /// <param name="year">���</param>
        /// <returns></returns>
        public string GetPunishStatisticsList(string year, string statMode)
        {
            string ownorgcode = OperatorProvider.Provider.Current().OrganizeCode;
            string whereSQL = " and a.createuserorgcode='" + ownorgcode + "'";

            //����
            if (!string.IsNullOrEmpty(year))
            {
                whereSQL += " and to_char(APPLYTIME,'yyyy')='" + year + "'";
            }
            whereSQL += " and FLOWSTATE ='1'";
            DataTable dt = new DataTable();
            dt.Columns.Add("TypeName");
            for (int i = 1; i <= 12; i++)
            {
                dt.Columns.Add("num" + i, typeof(int));
            }
            dt.Columns.Add("Total");
            //string strsql = @"select itemvalue,itemname from base_dataitemdetail where itemid = (select itemid from base_dataitem where itemcode = 'AEM_WSSJTYPE') order by SORTCODE ";
            //DataTable dtsqlde = this.BaseRepository().FindTable(strsql);
            DataTable dtsqlde = new DataTable();
            dtsqlde.Columns.Add("value");
            dtsqlde.Columns.Add("text");
            for (int i = 1; i < 5; i++)
            {
                DataRow dtrow = dtsqlde.NewRow();
                dtrow["value"] = i.ToString();
                switch (i)
                {
                    case 1:
                        dtrow["text"] = "�¹��¼�";
                        break;
                    case 2:
                        dtrow["text"] = "����";
                        break;
                    case 3:
                        dtrow["text"] = "�����Ų�����";
                        break;
                    case 4:
                        dtrow["text"] = "�ճ�����";
                        break;
                    default:
                        break;
                }
                dtsqlde.Rows.Add(dtrow);
            }
            
            for (int i = 0; i < dtsqlde.Rows.Count; i++)
            {
                int Total = 0;
                DataRow row = dt.NewRow();
                row["TypeName"] = dtsqlde.Rows[i]["text"].ToString();
                for (int k = 1; k <= 12; k++)
                {
                    string whereSQL2 = " and to_char(APPLYTIME,'mm')=" + k.ToString();
                    string forsql = "";
                    if (statMode == "0")
                    {
                        forsql = string.Format(@"select nvl(count(1),0) as cou from bis_safepunish a left join bis_safepunishdetail b on a.id=b.punishid  where amercetype='{0}' {1} {2}", dtsqlde.Rows[i]["value"].ToString(), whereSQL, whereSQL2);
                    }
                    else if (statMode == "1")
                    {
                        forsql = string.Format(@"select nvl(sum(b.punishnum),0) as cou from bis_safepunish a left join bis_safepunishdetail b on a.id=b.punishid where amercetype='{0}' {1} {2}", dtsqlde.Rows[i]["value"].ToString(), whereSQL, whereSQL2);
                    }

                    if (forsql != "")
                    {
                        int num = this.BaseRepository().FindObject(forsql).ToInt();
                        row["num" + k] = num;
                        Total += num;
                    }              
                }
                row["Total"] = Total.ToString();
                dt.Rows.Add(row);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                total = 1,
                page = 1,
                records = dt.Rows.Count,
                rows = dt
            });
        }

        /// <summary>
        /// ��ȡ�����Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public DataTable GetAptitudeInfo(string keyValue)
        {
            SafepunishEntity safereward = GetEntity(keyValue);

            string flowname = "�ͷ�����";
            if (safereward.AmerceType == "1" || safereward.AmerceType == "2")
            {
                flowname = "�¹��¼���������";
            }
            else if (safereward.AmerceType == "3" || safereward.AmerceType == "4")
            {
                flowname = "�ͷ�����";
            }
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string node_sql = string.Format(@"select a.itemvalue id,a.itemname flowname, b.approverpeopleids,b.AUDITDEPT auditdeptname,b.AUDITPEOPLE auditusername,b.AUDITTIME auditdate,b.AUDITRESULT auditstate,b.AUDITOPINION auditremark,b.auditsignimg
            from ( select itemvalue,itemname from base_dataitemdetail  where itemid =  (select itemid from base_dataitem where itemname = '{1}')) a
            left join (  select t.* , c.approverpeopleids  from  epg_aptitudeinvestigateaudit t left join bis_safepunish c on  t.APTITUDEID = c.id
            where   t.disable ='1' and  t.audittime in (select  max(audittime) audittime  from epg_aptitudeinvestigateaudit  where APTITUDEID = '{0}'    group by  APTITUDEID,remark )  )   b  on   a.itemvalue =b.REMARK order by id", keyValue, flowname);

            DataTable nodeDt = this.BaseRepository().FindTable(node_sql);
            return nodeDt;
        }

        /// <summary>
        /// ��ȡ��ȫ�ͷ�����ͼ����
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public Flow GetFlow(string keyValue)
        {
            List<nodes> nlist = new List<nodes>();
            List<lines> llist = new List<lines>();
            SafepunishEntity safereward = GetEntity(keyValue);

            string flowname = "�ͷ�����";
            if (safereward.AmerceType=="1"|| safereward.AmerceType=="2")
            {
                flowname = "�¹��¼���������";
            }
            else if (safereward.AmerceType == "3" || safereward.AmerceType == "4")
            {
                flowname = "�ͷ�����";
            }
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string node_sql = string.Format(@"select a.itemvalue id,a.itemname flowname, b.approverpeopleids,b.AUDITDEPT auditdeptname,b.AUDITPEOPLE auditusername,b.AUDITTIME auditdate,b.AUDITRESULT auditstate,b.AUDITOPINION auditremark
            from ( select itemvalue,itemname from base_dataitemdetail  where itemid =  (select itemid from base_dataitem where itemname = '{1}')) a
            left join (  select t.* , c.approverpeopleids  from  epg_aptitudeinvestigateaudit t left join bis_safepunish c on  t.APTITUDEID = c.id
            where   t.disable ='1' and  t.audittime in (select  max(audittime) audittime  from epg_aptitudeinvestigateaudit  where APTITUDEID = '{0}'    group by  APTITUDEID,remark )  )   b  on   a.itemvalue =b.REMARK order by id", keyValue, flowname);

            DataTable nodeDt = this.BaseRepository().FindTable(node_sql);

            UserInfoEntity createuser = new UserInfoService().GetUserInfoEntity(safereward.CreateUserId);
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DataItemModel ehsDepart = new DataItemDetailService().GetDataItemListByItemCode("'EHSDepartment'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();
            string ehsDepartCode = "";
            if (ehsDepart != null)
                ehsDepartCode = ehsDepart.ItemValue;


            if (safereward.AmerceType == "1" || safereward.AmerceType == "2") //�¹��¼�����������
            {
                if (createuser.RoleName.Contains("���������û�") && createuser.RoleName.Contains("������"))
                {
                    nodeDt.Rows.RemoveAt(1);
                }
                else if (GetLeaderId().Contains(safereward.CreateUserId))
                {
                    nodeDt.Rows.RemoveAt(2);
                    nodeDt.Rows.RemoveAt(1);
                }
            }
            if (safereward.AmerceType == "3" || safereward.AmerceType == "4")  //�ճ����˸������Ų�����
            {
                if (((createuser.Nature == "רҵ" || createuser.Nature == "����") && createuser.RoleName.Contains("������")) || (createuser.Nature == "����" && !createuser.RoleName.Contains("������")))
                {
                    nodeDt.Rows.RemoveAt(1);
                }
                else if (createuser.Nature == "����" && createuser.RoleName.Contains("������"))
                {
                    nodeDt.Rows.RemoveAt(2);
                    nodeDt.Rows.RemoveAt(1);
                }
            }

            Flow flow = new Flow();
            flow.title = "";
            flow.initNum = 22;
            flow.activeID = safereward.ApplyState;
            if (nodeDt != null && nodeDt.Rows.Count > 0)
            {
                #region ����node����
                int Taged = 0;
                for (int i = 0; i < nodeDt.Rows.Count; i++)
                {
                    DataRow dr = nodeDt.Rows[i];
                    nodes nodes = new nodes();
                    nodes.alt = true;
                    nodes.isclick = false;
                    nodes.css = "";
                    nodes.id = dr["id"].ToString(); //����
                    nodes.img = "";
                    nodes.name = dr["flowname"].ToString();
                    nodes.type = "stepnode";
                    nodes.width = 150;
                    nodes.height = 60;
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
                    //��˼�¼
                    if (dr["auditdeptname"] != null && !string.IsNullOrEmpty(dr["auditdeptname"].ToString()))
                    {
                        Taged = 1;
                        List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                        DateTime auditdate;
                        DateTime.TryParse(dr["auditdate"].ToString(), out auditdate);
                        nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                        nodedesignatedata.creatdept = dr["auditdeptname"].ToString();
                        nodedesignatedata.createuser = dr["auditusername"].ToString();
                        nodedesignatedata.status = dr["auditstate"].ToString() == "0" ? "�Ѵ���" : "δ����";
                        if (i == 0)
                        {
                            nodedesignatedata.prevnode = "��";
                        }
                        else
                        {
                            nodedesignatedata.prevnode = nodeDt.Rows[i - 1]["flowname"].ToString();
                        }

                        nodelist.Add(nodedesignatedata);
                        sinfo.Taged = Taged;
                        sinfo.NodeDesignateData = nodelist;
                        nodes.setInfo = sinfo;

                        if (dr["auditstate"].ToString() == "1")
                        {
                            Taged = 0;
                        }
                    }
                    else if (Taged == 1)
                    {
                        if (i == (nodeDt.Rows.Count - 1))
                        {
                            Taged = 1;
                            //List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                            //NodeDesignateData nodedesignatedata = new NodeDesignateData();
                            //nodelist.Add(nodedesignatedata);
                            sinfo.Taged = Taged;
                            //sinfo.NodeDesignateData = nodelist;
                            nodes.setInfo = sinfo;
                        }
                        else
                        {
                            Taged = 0;
                            List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                            NodeDesignateData nodedesignatedata = new NodeDesignateData();
                            nodedesignatedata.createdate = "����";
                            var userid = nodeDt.Rows[i - 1]["approverpeopleids"].ToString();
                            string[] ids = userid.Split(',');
                            string DeptName = "", RealName = "";
                            if (ids.Length > 0)
                            {
                                for (int j = 0; j < ids.Length; j++)
                                {
                                    if (!string.IsNullOrEmpty(ids[j]))
                                    {
                                        UserInfoEntity uInfor = new UserInfoService().GetUserInfoEntity(ids[j]);
                                        DeptName += uInfor.DeptName + ",";
                                        RealName += uInfor.RealName + ",";
                                    }
                                }
                                nodedesignatedata.creatdept = DeptName.Length > 0 ? DeptName.Substring(0, DeptName.Length - 1) : "";
                                nodedesignatedata.createuser = RealName.Length > 0 ? RealName.Substring(0, RealName.Length - 1) : "";
                            }
                            else
                            {
                                nodedesignatedata.creatdept = "��";
                                nodedesignatedata.createuser = "��";
                            }
                            nodedesignatedata.status = "���ڴ���...";
                            if (i == 0)
                            {
                                nodedesignatedata.prevnode = "��";
                            }
                            else
                            {
                                nodedesignatedata.prevnode = nodeDt.Rows[i - 1]["flowname"].ToString();
                            }

                            nodelist.Add(nodedesignatedata);
                            sinfo.Taged = Taged;
                            sinfo.NodeDesignateData = nodelist;
                            nodes.setInfo = sinfo;
                        }
                        
                    }
                    else if (safereward.ApplyState == "0" && i == 0)
                    {
                        List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                        nodedesignatedata.createdate = "����";
                        UserInfoEntity uInfor = new UserInfoService().GetUserInfoEntity(safereward.ApproverPeopleIds);
                        if (uInfor != null)
                        {
                            nodedesignatedata.creatdept = uInfor.DeptName;
                            nodedesignatedata.createuser = uInfor.RealName;
                        }
                        else
                        {
                            nodedesignatedata.creatdept = safereward.ApplyUserDeptName;
                            nodedesignatedata.createuser = safereward.ApplyUserName;
                            
                        }
                       
                        nodedesignatedata.status = "���ڴ���...";
                        if (i == 0)
                        {
                            nodedesignatedata.prevnode = "��";
                        }
                        else
                        {
                            nodedesignatedata.prevnode = nodeDt.Rows[i - 1]["flowname"].ToString();
                        }

                        nodelist.Add(nodedesignatedata);
                        sinfo.Taged = Taged;
                        sinfo.NodeDesignateData = nodelist;
                        nodes.setInfo = sinfo;
                    }
                    nlist.Add(nodes);
                }

               

                #endregion

                #region ����line����

                for (int i = 0; i < nodeDt.Rows.Count; i++)
                {
                    lines lines = new lines();
                    lines.alt = true;
                    lines.id = Guid.NewGuid().ToString();
                    lines.from = nodeDt.Rows[i]["id"].ToString();
                    if (i < nodeDt.Rows.Count - 1)
                    {
                        lines.to = nodeDt.Rows[i + 1]["id"].ToString();
                    }
                    lines.name = "";
                    lines.type = "sl";
                    llist.Add(lines);
                }
             
                #endregion

                flow.nodes = nlist;
                flow.lines = llist;
            }
            return flow;
        }


        public string GetPunishCode()
        {
            string sql = string.Format("select safepunishcode from bis_safepunish where safepunishcode like '%{0}%' order by createdate desc", DateTime.Now.ToString("yyyyMMdd"));
            DataTable tb = this.BaseRepository().FindTable(sql);
            if (tb.Rows.Count > 0)
            {
                int number = tb.Rows.Count + 1;
                if (number < 10)
                {
                    return "00" + number.ToString();
                }
                else if (number < 100)
                {
                    return "0" + number.ToString();
                }
                else
                {
                    return number.ToString();
                }
            }
            else
            {
                return "001";
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public string GetPunishNum()
        {
            string sql = "select count(id) num   from bis_safePunish where flowstate = 0 and applystate <>0 and  instr(ApproverPeopleIds,'" + OperatorProvider.Provider.Current().UserId + "'  )> 0";
            DataTable tb = this.BaseRepository().FindTable(sql);
            return tb.Rows.Count > 0 ? tb.Rows[0][0].ToString() : "0";
        }


        /// <summary>
        /// ��ҳ����
        /// </summary>
        /// <returns></returns>
        public int CountIndex(ERCHTMS.Code.Operator currUser)
        {
            int num = 0;
            string sqlwhere = "";

            sqlwhere += string.Format(" and ((flowdept ='{0}'", currUser.DeptId);

            string[] arr = currUser.RoleName.Split(',');
            if (arr.Length > 0)
            {
                sqlwhere += " and (";
                foreach (var item in arr)
                {
                    sqlwhere += string.Format(" flowrolename  like '%{0}%' or", item);
                }
                sqlwhere = sqlwhere.Substring(0, sqlwhere.Length - 2);
                sqlwhere += " )";
            }
            sqlwhere += string.Format(") and isover='0' and issaved='1')");
            string sql = string.Format("select count(1) from epg_dailyexamine where  createuserorgcode='{0}' {1}", currUser.OrganizeCode, sqlwhere);
            object obj = this.BaseRepository().FindObject(sql);
            int.TryParse(obj.ToString(), out num);

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
            SafekpidataService safekpidata = new SafekpidataService();
            SafekpidataEntity list = safekpidata.GetList("").Where(p => p.SafePunishId == keyValue).FirstOrDefault();
            if (list != null)
            {
                safekpidata.RemoveForm(list.ID);
            }
            this.BaseRepository().Delete(keyValue);
        }

        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <param name="kpiEntity">������Ϣ</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, SafepunishEntity entity, SafekpidataEntity kpiEntity)
        {
            entity.ID = keyValue;  
     
            //��Ҫ�������Ҫ����
            string deptName = "";
            if (kpiEntity != null)
            {
                 deptName = GetDeptNames(kpiEntity.KpiUserId, kpiEntity.OnDutyUserId, kpiEntity.PrincipalId);
            }

            if (entity.AmerceType == "2")
            {
                entity.PunishType = "";
            }
            else
            {
                entity.ApplyDeptCode = "";
                entity.ApplyDeptId = "";
                entity.ApplyDeptName = "";
                entity.PunishUserId = "";
                entity.PunishUserName = "";
                entity.AmerceAmount = "";
                if (entity.PunishType != "2")
                {
                    kpiEntity.PrincipalName = "";
                    kpiEntity.PrincipalId= "";
                    kpiEntity.KpiScore10 = "";                   
                }

                if (entity.PunishType == "2" || entity.PunishType == "4")
                {
                    kpiEntity.KpiUserName = "";
                    kpiEntity.KpiUserId= "";
                    kpiEntity.WssjScore = "";
                }
            }
            SafepunishEntity eEntity = GetEntity(keyValue);
            if (eEntity!=null)
            {
                entity.Modify(keyValue);
                if (entity.AmerceType == "1")
                {
                    entity.PunishObjectNames = !string.IsNullOrEmpty(entity.PunishDeptName) ? deptName + ',' + entity.PunishDeptName : deptName;
                }
                else if (entity.AmerceType == "2")
                {
                    entity.PunishObjectNames = entity.ApplyDeptName;
                }
                this.BaseRepository().Update(entity);
            }
            else
            {
                entity.Create();
                if (kpiEntity != null)
                kpiEntity.SafePunishId = entity.ID;
                entity.ApplyState =!string.IsNullOrEmpty(entity.ApplyState)?entity.ApplyState: "0";
                //var userRole = OperatorProvider.Provider.Current().RoleName;
                //if (userRole.Contains("��ͨ�û�") || userRole.Contains("���鼶�û�") || userRole.Contains("ר��"))
                //{
                //    entity.ApplyState = "0";
                //}
                //if (userRole.Contains("���ż��û�"))
                //{
                //    entity.ApplyState = "1";
                //}          

                entity.FlowState = "0";
                entity.ApproverPeopleIds = "";
                if (entity.AmerceType == "1")
                {
                    entity.PunishObjectNames = !string.IsNullOrEmpty(entity.PunishDeptName) ? deptName + ',' + entity.PunishDeptName : deptName;
                }
                else if (entity.AmerceType == "2")
                {
                    entity.PunishObjectNames = entity.ApplyDeptName;
                }
                entity.ApplyUserDeptId = OperatorProvider.Provider.Current().DeptId;
                entity.ApplyUserDeptName = OperatorProvider.Provider.Current().DeptName;

                this.BaseRepository().Insert(entity);
            }

            if (entity.AmerceType == "1")
            {
                new SafekpidataService().SaveForm(keyValue, kpiEntity);
            }
        }


        /// <summary>
        /// �ύ����
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void CommitApply(string keyValue, AptitudeinvestigateauditEntity aentity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                SafepunishEntity entity = GetEntity(keyValue);
                UserInfoEntity createuser = new UserInfoService().GetUserInfoEntity(entity.CreateUserId);
                Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
                entity.Modify(keyValue);
                DataItemModel ehsDepart = new DataItemDetailService().GetDataItemListByItemCode("'EHSDepartment'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();
                string ehsDepartCode = "";
                if (ehsDepart != null)
                    ehsDepartCode = ehsDepart.ItemValue;
                if (entity.FlowState != "1")
                {               
                    if (!string.IsNullOrEmpty(entity.ApplyState))
                    {
                        #region //�����Ϣ��
                        AptitudeinvestigateauditEntity aidEntity = new AptitudeinvestigateauditEntity();
                        aidEntity.AUDITRESULT = aentity.AUDITRESULT; //ͨ��
                        if (aentity.AUDITTIME != null)
                            aidEntity.AUDITTIME =
                                Convert.ToDateTime(aentity.AUDITTIME.Value.ToString("yyyy-MM-dd") + " " +
                                                   DateTime.Now.ToString("HH:mm:ss")); //���ʱ��
                        aidEntity.AUDITPEOPLE = aentity.AUDITPEOPLE;  //�����Ա����
                        aidEntity.AUDITPEOPLEID = aentity.AUDITPEOPLEID;//�����Աid
                        aidEntity.APTITUDEID = keyValue;  //������ҵ��ID 
                        aidEntity.AUDITDEPTID = aentity.AUDITDEPTID;//��˲���id
                        aidEntity.AUDITDEPT = aentity.AUDITDEPT; //��˲���
                        aidEntity.AUDITOPINION = aentity.AUDITOPINION; //������
                        aidEntity.AUDITSIGNIMG = aentity.AUDITSIGNIMG; //����ǩ��
                        aidEntity.Disable = "1"; //����ͼ�����¼�¼
                        if (entity.ApplyState != null) aidEntity.REMARK = (entity.ApplyState).ToString(); //��ע �����̵�˳���
                        new AptitudeinvestigateauditService().SaveForm(aidEntity.ID, aidEntity);
                        #endregion
                    }

                    if (entity.AmerceType == "1" || entity.AmerceType == "2") //�¹��¼�����������
                    {
                        switch (entity.ApplyState ?? "0")
                        {
                            case "0":
                                if (createuser.RoleName.Contains("���������û�") && !createuser.RoleName.Contains("������"))
                                {
                                    entity.ApplyState = "1";
                                    entity.FlowState = "0";
                                    entity.ApproverPeopleIds = GetEhsUserId() ?? "";
                                }
                                else if (createuser.RoleName.Contains("���������û�") && createuser.RoleName.Contains("������"))
                                {
                                    entity.ApplyState = "2";
                                    entity.FlowState = "0";
                                    entity.ApproverPeopleIds = GetLeaderId() ?? "";
                                }
                                else if (GetLeaderId().Contains(entity.CreateUserId))
                                {
                                    entity.ApplyState = "3";
                                    entity.FlowState = "1";
                                    entity.ApproverPeopleIds = "";
                                }

                                break;
                            case "1":
                                if (aentity.AUDITRESULT == "0")
                                {
                                    entity.ApplyState = "2";
                                    entity.ApproverPeopleIds = GetLeaderId() ?? "";
                                    entity.DeptManagerId = entity.ApproverPeopleIds;
                                }
                                else
                                {
                                    entity.ApplyState = "0";
                                    entity.FlowState = "2";
                                    entity.ApproverPeopleIds = "";
                                    string strsql = string.Format("update EPG_APTITUDEINVESTIGATEAUDIT set Disable = 0 where APTITUDEID ='{0}'", keyValue);
                                    this.BaseRepository().ExecuteBySql(strsql);
                                }

                                break;
                            case "2":
                                if (aentity.AUDITRESULT == "0")
                                {
                                    entity.ApplyState = "3";
                                    entity.FlowState = "1";
                                }
                                else
                                {
                                    entity.ApplyState = "0";
                                    entity.FlowState = "2";
                                    entity.ApproverPeopleIds = "";
                                    string strsql = string.Format("update EPG_APTITUDEINVESTIGATEAUDIT set Disable = 0 where APTITUDEID ='{0}'", keyValue);
                                    this.BaseRepository().ExecuteBySql(strsql);
                                }
                                break;

                        }
                    }
                    if (entity.AmerceType == "3" || entity.AmerceType == "4")  //�ճ����˸������Ų�����
                    {
                        switch (entity.ApplyState ?? "0")
                        {
                            case "0":
                                if ((createuser.Nature == "רҵ" || createuser.Nature == "����") && !createuser.RoleName.Contains("������"))
                                {
                                    entity.ApplyState = "1";
                                    entity.FlowState = "0";
                                    entity.ApproverPeopleIds = GetMajorUserId(createuser.DepartmentId);
                                }
                                else if (((createuser.Nature == "רҵ" || createuser.Nature == "����") && createuser.RoleName.Contains("������")) || (createuser.Nature == "����" && !createuser.RoleName.Contains("������")))
                                {
                                    entity.ApplyState = "2";
                                    entity.FlowState = "0";
                                    entity.ApproverPeopleIds = GetRoleUserId(entity.CreateUserId) ?? "";
                                }
                                else if (createuser.Nature == "����" && createuser.RoleName.Contains("������"))
                                {
                                    entity.ApplyState = "3";
                                    entity.FlowState = "1";
                                    entity.ApproverPeopleIds = "";
                                }
                                break;
                            case "1":
                                if (aentity.AUDITRESULT == "0")
                                {
                                    entity.ApplyState = "2";
                                    entity.ApproverPeopleIds = GetRoleUserId(entity.CreateUserId) ?? "";
                                    entity.DeptManagerId = entity.ApproverPeopleIds;
                                }
                                else
                                {
                                    entity.ApplyState = "0";
                                    entity.FlowState = "2";
                                    entity.ApproverPeopleIds = "";
                                    string strsql = string.Format("update EPG_APTITUDEINVESTIGATEAUDIT set Disable = 0 where APTITUDEID ='{0}'", keyValue);
                                    this.BaseRepository().ExecuteBySql(strsql);
                                }

                                break;
                            case "2":
                                if (aentity.AUDITRESULT == "0")
                                {
                                    entity.ApplyState = "3";
                                    entity.FlowState = "1";
                                }
                                else
                                {
                                    entity.ApplyState = "0";
                                    entity.FlowState = "2";
                                    entity.ApproverPeopleIds = "";
                                    string strsql = string.Format("update EPG_APTITUDEINVESTIGATEAUDIT set Disable = 0 where APTITUDEID ='{0}'", keyValue);
                                    this.BaseRepository().ExecuteBySql(strsql);
                                }
                                break;

                        }
                    }
                    

                    this.BaseRepository().Update(entity);
                }
            }
        }
        #endregion

        /// <summary>
        /// ��ȡרҵ����
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
//        private string GetRoleUserId(string userid)
//        {
//            string sql = @"select userid from base_user where instr(rolename,'������' )> 0  and departmentid =
//            (select departmentid from base_user where  userid = @userid) ";
//            DataTable dt = this.BaseRepository().FindTable(sql, new DbParameter[] { DbParameters.CreateDbParameter("@userid", userid) });
//            string approverPeopleIds = "";
//            foreach (DataRow dr in dt.Rows)
//            {
//                approverPeopleIds += dr["userid"] + ",";
//            }
//            return !string.IsNullOrEmpty(approverPeopleIds) ? approverPeopleIds.Substring(0, approverPeopleIds.Length - 1) : "";
//        }

        /// <summary>
        /// ��ȡ��Ҫ���β������Ҫ���β���name
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        private string GetDeptNames(string userid1,string userid2,string userid3)
        {
            UserInfoService userservice = new UserInfoService();
            UserInfoEntity user = new UserInfoEntity();
            ArrayList ids = new ArrayList() ;
            string deptNames = "";
            if (!string.IsNullOrEmpty(userid1) && userid1!="0")
                ids.Add(userid1);
            if (!string.IsNullOrEmpty(userid2) && userid2 != "0")
                ids.Add(userid2);
            if (!string.IsNullOrEmpty(userid3) && userid3 != "0")
                ids.Add(userid3);
            if (ids.Count > 0)
            {
                foreach (string id in ids)
                {
                    user = userservice.GetUserInfoEntity(id);
                    if (user != null)
                        deptNames += user.DeptName+"��";
                }
            }


            return !string.IsNullOrEmpty(deptNames) ? deptNames.Substring(0, deptNames.Length - 1) : "";
        }


        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        private string GetDepteUserId(string userid)
        {
            string sql = @"select userid from base_user where instr(rolename,'������' )> 0  and departmentid =
            (select parentid from base_department where departmentid = (select departmentid from base_user where  userid =@userid)) ";
            DataTable dt = this.BaseRepository().FindTable(sql, new DbParameter[] { DbParameters.CreateDbParameter("@userid", userid) });
            string approverPeopleIds = "";
            foreach (DataRow dr in dt.Rows)
            {
                approverPeopleIds += dr["userid"] + ",";
            }
            return !string.IsNullOrEmpty(approverPeopleIds) ? approverPeopleIds.Substring(0, approverPeopleIds.Length - 1) : "";
        }

        /// <summary>
        /// ��ȡרҵ������ID
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        private string GetMajorUserId(string departmentid)
        {
            string sql = @"select userid from base_user where instr(rolename,'������' )> 0 and departmentid =@departmentid";
            DataTable dt = this.BaseRepository().FindTable(sql, new DbParameter[] { DbParameters.CreateDbParameter("@departmentid", departmentid) });
            string approverPeopleIds = "";
            foreach (DataRow dr in dt.Rows)
            {
                approverPeopleIds += dr["userid"] + ",";
            }
            return !string.IsNullOrEmpty(approverPeopleIds) ? approverPeopleIds.Substring(0, approverPeopleIds.Length - 1) : "";
        }

        /// <summary>
        /// �����û���ɫ��ȡ�����쵼id
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        private string GetRoleUserId(string userid)
        {
            string sql = @"select userid from base_user where instr(rolename,'������' )> 0 and departmentid = 
            (select case  when b.isbz > 0  then parentid else departmentid end as parentid from base_department a
            right join (select departmentcode,instr(rolename,'���鼶�û�' ) + instr(rolename,'רҵ���û�' ) isbz from base_user where  userid = @userid) b
            on a.encode  = b.departmentcode)";
            DataTable dt = this.BaseRepository().FindTable(sql, new DbParameter[] { DbParameters.CreateDbParameter("@userid", userid) });
            string approverPeopleIds = "";
            foreach (DataRow dr in dt.Rows)
            {
                approverPeopleIds += dr["userid"] + ",";
            }
            return !string.IsNullOrEmpty(approverPeopleIds) ? approverPeopleIds.Substring(0, approverPeopleIds.Length - 1) : "";
        }

        /// <summary>
        /// ��ȡ�������ø���EHS����id��ѯEHS���Ÿ�����id
        /// </summary>
        /// <returns></returns>
        private string GetEhsUserId()
        {
            string sql = @"select userid from base_user
                                         where instr(rolename,'������' )> 0 and departmentcode = 
                                        (select itemvalue from base_dataitemdetail where itemid =(select itemid from base_dataitem  where itemname = 'EHS����') and itemname = '" + OperatorProvider.Provider.Current().OrganizeId + "')";
            DataTable dt = this.BaseRepository().FindTable(sql);
            string approverPeopleIds = "";
            foreach (DataRow dr in dt.Rows)
            {
                approverPeopleIds += dr["userid"] + ",";
            }
            return !string.IsNullOrEmpty(approverPeopleIds) ? approverPeopleIds.Substring(0, approverPeopleIds.Length - 1) : "";
        }

        /// <summary>
        /// ��ȡ�ֹ��쵼��Ϣ
        /// </summary>
        /// <returns></returns>
        public string GetLeaderId()
        {
            string strsql =
                "select UserId,RealName from base_user where organizeid = '" + OperatorProvider.Provider.Current().OrganizeId + "' and instr(rolename,'��˾���û�')>0 and  instr(rolename,'��ȫ����Ա')>0";
            DataTable dt = this.BaseRepository().FindTable(strsql);
            string approverPeopleIds = "";
            foreach (DataRow dr in dt.Rows)
            {
                approverPeopleIds += dr["userid"] + ",";
            }
            return !string.IsNullOrEmpty(approverPeopleIds) ? approverPeopleIds.Substring(0, approverPeopleIds.Length - 1) : "";
        }


        /// <summary>
        /// ��ȡCeoId
        /// </summary>
        /// <returns></returns>
        private string GetCeoId()
        {
            string organizeid = OperatorProvider.Provider.Current().OrganizeId;
            string sql = @"   select * from base_user where organizeid = '" + organizeid + "'and  instr(rolename,'��˾�쵼' )> 0  and dutyname = '�ܾ���'";
            DataTable dt = this.BaseRepository().FindTable(sql);
            string approverPeopleIds = "";
            foreach (DataRow dr in dt.Rows)
            {
                approverPeopleIds += dr["userid"] + ",";
            }
            return !string.IsNullOrEmpty(approverPeopleIds) ? approverPeopleIds.Substring(0, approverPeopleIds.Length - 1) : "";
        }
    }
}
