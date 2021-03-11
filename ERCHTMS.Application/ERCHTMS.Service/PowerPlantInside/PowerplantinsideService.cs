using System;
using ERCHTMS.Entity.PowerPlantInside;
using ERCHTMS.IService.PowerPlantInside;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Code;
using ERCHTMS.Service.CommonPermission;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.Service.BaseManage;

namespace ERCHTMS.Service.PowerPlantInside
{
    /// <summary>
    /// �� ������λ�ڲ��챨
    /// </summary>
    public class PowerplantinsideService : RepositoryFactory<PowerplantinsideEntity>, PowerplantinsideIService
    {
        private IManyPowerCheckService powerCheck = new ManyPowerCheckService();
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<PowerplantinsideEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }


        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;

            if (queryJson.Length > 0)
            {
                var queryParam = queryJson.ToJObject();
                //��ѯ����
                if (!queryParam["sgtype"].IsEmpty())
                {
                    string sgtype = queryParam["sgtype"].ToString();
                    pagination.conditionJson += string.Format(" and accidenteventtype = '{0}'", sgtype);
                }
                if (!queryParam["keyword"].IsEmpty())
                {
                    string sgtypename = queryParam["keyword"].ToString();
                    pagination.conditionJson += string.Format(" and accidenteventname like '%{0}%'", sgtypename);
                }
                if (!queryParam["happentimestart"].IsEmpty())
                {
                    string happentimestart = queryParam["happentimestart"].ToString();
                    pagination.conditionJson += string.Format(" and happentime >= (select  to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') from dual)", happentimestart);
                }
                if (!queryParam["happentimeend"].IsEmpty())
                {
                    string happentimeend = queryParam["happentimeend"].ToString();
                    if (happentimeend.Length == 10)
                        happentimeend = Convert.ToDateTime(happentimeend).AddDays(1).ToString();
                    pagination.conditionJson += string.Format(" and happentime <= (select  to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') from dual)", happentimeend);
                }

                //��������
                if (!queryParam["mode"].IsEmpty())
                {
                    string mode = queryParam["mode"].ToString();
                    if (mode == "dbsx")
                    {
                        Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                        var roleArr = user.RoleName.Split(','); //��ǰ��Ա��ɫ
                        string strRole = "";
                        foreach (var rolename in roleArr)
                        {
                            strRole += string.Format(" flowrolename like '%{0}%' or ", rolename);
                        }

                        if (strRole.Length > 2)
                        {
                            strRole = strRole.Substring(0, strRole.Length - 3);
                        }

                        string strsql = string.Format(" and createuserorgcode = '{0}' and ISSAVED=1  and isover <> 1  and flowdept = '{1}' and ({2})", user.OrganizeCode, user.DeptId, strRole);

                        pagination.conditionJson += strsql;
                    }
                    else if (mode == "isover")
                    {
                        pagination.conditionJson += " and issaved=1 and isover=1";
                    }

                }

                #region Ȩ���ж�
                if (!queryParam["code"].IsEmpty() && !queryParam["isOrg"].IsEmpty())
                {
                    pagination = PermissionByCurrent.GetPermissionByCurrent2(pagination, queryParam["code"].ToString(), queryParam["isOrg"].ToString());
                }
                #endregion
            }

            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        #region  ��ǰ��¼���Ƿ���Ȩ����˲���ȡ��һ�����Ȩ��ʵ��
        /// <summary>
        /// ��ǰ��¼���Ƿ���Ȩ����˲���ȡ��һ�����Ȩ��ʵ��
        /// </summary>
        /// <param name="currUser">��ǰ��¼��</param>
        /// <param name="state">�Ƿ���Ȩ����� 1������� 0 ���������</param>
        /// <param name="moduleName">ģ������</param>
        /// <param name="createdeptid">�����˲���ID</param>
        /// <returns>null-��ǰΪ���һ�����,ManyPowerCheckEntity����һ�����Ȩ��ʵ��</returns>
        public ManyPowerCheckEntity CheckAuditPower(Operator currUser, out string state, string moduleName, string createdeptid)
        {
            ManyPowerCheckEntity nextCheck = null;//��һ�����
            List<ManyPowerCheckEntity> powerList = powerCheck.GetListBySerialNum(currUser.OrganizeCode, moduleName);

            if (powerList.Count > 0)
            {
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
                if (checkPower.Count > 0)
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

                if (null != nextCheck)
                {
                    //��ǰ�������µĶ�Ӧ����
                    var serialList = powerList.Where(p => p.SERIALNUM == nextCheck.SERIALNUM);
                    //���ϼ�¼����1�����ʾ���ڲ�����ˣ���飩�����
                    if (serialList.Count() > 1)
                    {
                        string flowdept = string.Empty;  // ��ȡֵ��ʽ a1,a2
                        string flowdeptname = string.Empty; // ��ȡֵ��ʽ b1,b2
                        string flowrole = string.Empty;   // ��ȡֵ��ʽ c1|c2|  (c1���ݹ��ɣ� cc1,cc2,cc3)
                        string flowrolename = string.Empty; // ��ȡֵ��ʽ d1|d2| (d1���ݹ��ɣ� dd1,dd2,dd3)

                        ManyPowerCheckEntity slastEntity = new ManyPowerCheckEntity();
                        slastEntity = serialList.LastOrDefault();
                        foreach (ManyPowerCheckEntity model in serialList)
                        {
                            flowdept += model.CHECKDEPTID + ",";
                            flowdeptname += model.CHECKDEPTNAME + ",";
                            flowrole += model.CHECKROLEID + "|";
                            flowrolename += model.CHECKROLENAME + "|";
                        }
                        if (!flowdept.IsEmpty())
                        {
                            slastEntity.CHECKDEPTID = flowdept.Substring(0, flowdept.Length - 1);
                        }
                        if (!flowdeptname.IsEmpty())
                        {
                            slastEntity.CHECKDEPTNAME = flowdeptname.Substring(0, flowdeptname.Length - 1);
                        }
                        if (!flowdept.IsEmpty())
                        {
                            slastEntity.CHECKROLEID = flowrole.Substring(0, flowrole.Length - 1);
                        }
                        if (!flowdept.IsEmpty())
                        {
                            slastEntity.CHECKROLENAME = flowrolename.Substring(0, flowrolename.Length - 1);
                        }
                        nextCheck = slastEntity;
                    }
                }
                return nextCheck;
            }
            else
            {
                state = "0";
                return nextCheck;
            }

        }
        #endregion

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public PowerplantinsideEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        #region ͳ��ͼ
        /// <summary>
        /// �¹��¼�ͳ��ͼ
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public string GetStatisticsHighchart(string year, string mode)
        {
            string statMode = "";
            List<object> dic = new List<object>();
            List<string> listtypes = new List<string>();
            List<Object> numInts = new List<Object>();
            string ownorgcode = OperatorProvider.Provider.Current().OrganizeCode;
            string whereSQL = "  createuserorgcode='" + ownorgcode + "'";
            if (mode == "0")
            {
                statMode = "AccidentEventType";
                whereSQL += string.Format(" and  AccidentEventType = ");
            }
            else if (mode == "1")
            {
                statMode = "AccidentEventProperty";
                whereSQL += string.Format(" and  AccidentEventProperty = ");
            }
            else if (mode == "4")
            {
                statMode = "SpecialtyType";
                whereSQL += string.Format(" and  Specialty = ");
            }
            else if (mode == "5")
            {
                statMode = "BelongSystem";
                whereSQL += string.Format(" and  BelongSystem = ");
            }
            switch (mode)
            {
                case "2":
                    //����
                    if (!string.IsNullOrEmpty(year))
                    {
                        whereSQL += " and to_char(HAPPENTIME,'yyyy')='" + year + "'";
                    }
                    string yssql = @"select itemvalue,itemname from base_dataitemdetail where itemid = (select itemid from base_dataitem where itemcode = 'AccidentEventCause') order by SORTCODE ";
                    DataTable ysdt = this.BaseRepository().FindTable(yssql);
                    for (int i = 1; i <= 12; i++)
                    {
                        listtypes.Add(i.ToString() + "��");
                    }
                    foreach (DataRow item in ysdt.Rows)
                    {
                        List<int> list = new List<int>();
                        for (int i = 1; i <= 12; i++)
                        {
                            string whereSQL2 = " and to_char(HAPPENTIME,'mm')=" + i.ToString();
                            string forsql = string.Format(@"select nvl(count(id),0) as cou from BIS_POWERPLANTINSIDE where instr(AccidentEventCause,'{0}')>0 and {1} {2}", item["itemvalue"], whereSQL, whereSQL2);
                            int num = this.BaseRepository().FindObject(forsql).ToInt();
                            list.Add(num);                           
                        }
                        dic.Add(new { name = item[1], data = list });
                    }
                    break;
                case "3":
                    string orgid = OperatorProvider.Provider.Current().OrganizeId;
                    //����
                    if (!string.IsNullOrEmpty(year))
                    {
                        whereSQL += " and to_char(HAPPENTIME,'yyyy')='" + year + "'";
                    }
                    string bmsql = @"select encode,fullname from BASE_DEPARTMENT where organizeid = '" + orgid + "'  and nature = '����' order by encode  desc";
                    DataTable deptdt = this.BaseRepository().FindTable(bmsql);
                    List<int> listint = new List<int>();
                    foreach (DataRow item in deptdt.Rows)
                    {
                        if (item["fullname"].ToString() != "��ί��λ")
                        {
                            listtypes.Add(item["fullname"].ToString());
                        }
                        string forsql = string.Format(@"select nvl(count(id),0) as cou from BIS_POWERPLANTINSIDE where   instr(belongdeptcode,'{0}')>0  and {1}",
                            item["encode"], whereSQL);
                        int num = this.BaseRepository().FindObject(forsql).ToInt();
                        listint.Add(num);                    
                    }
                    listtypes.Add("��ط�");
                    dic.Add(new { name = "������", data = listint });
                    break;
                case "6":
                    //����
                    if (!string.IsNullOrEmpty(year))
                    {
                        whereSQL += " and to_char(HAPPENTIME,'yyyy')='" + year + "'";
                    }
                    for (int i = 1; i < 13; i++)
                    {
                        listtypes.Add(i.ToString() + "��");
                        string whereSQL2 = " and to_char(HAPPENTIME,'mm')=" + i.ToString();
                        string forsql = string.Format(@" select nvl(count(id),0) as cs from BIS_POWERPLANTINSIDE where  {0} {1}", whereSQL, whereSQL2);
                        int num = this.BaseRepository().FindObject(forsql).ToInt();
                        numInts.Add(num);
                    }
                    dic.Add(new { name = "�¶�", data = numInts });
                    break;
                case "7":
                    for (int i = int.Parse(year) + 1; i <= Convert.ToInt32(DateTime.Now.ToString("yyyy")); i++)
                    {
                        listtypes.Add(i.ToString() + "��");
                        string whereSQL2 = " and to_char(HAPPENTIME,'yyyy')=" + i.ToString();
                        string forsql = string.Format(@" select nvl(count(id),0) as cs from BIS_POWERPLANTINSIDE where  {0} {1}", whereSQL, whereSQL2);
                        int num = this.BaseRepository().FindObject(forsql).ToInt();
                        numInts.Add(num);
                    }
                    dic.Add(new { name = "���", data = numInts });
                    break;
                default:
                    string selectYear = "";
                    //����
                    if (!string.IsNullOrEmpty(year.ToString()))
                    {
                        selectYear = " and to_char(HAPPENTIME,'yyyy')='" + year + "'";
                    }
                    string strsql = @"select itemvalue,itemname from base_dataitemdetail where itemid = (select itemid from base_dataitem where itemcode = '" + statMode + "') order by SORTCODE ";
                    DataTable dt = this.BaseRepository().FindTable(strsql);
                    foreach (DataRow item in dt.Rows)
                    {
                        listtypes.Add(item["itemname"].ToString());
                        string forsql = string.Format(@" select nvl(count(id),0) as cs from BIS_POWERPLANTINSIDE  where  {0} '{1}' {2} ", whereSQL, item["itemvalue"].ToString(), selectYear);
                        int num = this.BaseRepository().FindObject(forsql).ToInt();
                        numInts.Add(num);
                    }
                    dic.Add(new { name = "����", data = numInts });
                    break;
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(new { x = listtypes, y = dic });
        }
        #endregion


        #region ͳ���б�
        /// <summary>
        ///��ȡ�¶�ͳ�Ʊ������
        /// </summary>
        /// <param name="year">���</param>
        /// <returns></returns>
        public string GetStatisticsList(int year, string mode)
        {
            string statMode = "";
            double Total = 0;
            string ownorgcode = OperatorProvider.Provider.Current().OrganizeCode;
            string whereSQL = "  createuserorgcode='" + ownorgcode + "'";
            if (mode == "0")
            {
                statMode = "AccidentEventType";
                whereSQL += string.Format(" and  AccidentEventType = ");
            }
            else if (mode == "1")
            {
                statMode = "AccidentEventProperty";
                whereSQL += string.Format(" and  AccidentEventProperty = ");
            }
            else if (mode == "4")
            {
                statMode = "SpecialtyType";
                whereSQL += string.Format(" and  Specialty = ");
            }
            else if (mode == "5")
            {
                statMode = "BelongSystem";
                whereSQL += string.Format(" and  BelongSystem = ");
            }
            DataTable dt = new DataTable();
            DataRow row = dt.NewRow();
            switch (mode)
            {
                case "2":
                    //����
                    if (!string.IsNullOrEmpty(year.ToString()))
                    {
                        whereSQL += " and to_char(HAPPENTIME,'yyyy')='" + year + "'";
                    }
                    dt.Columns.Add("TypeName");
                    for (int i = 1; i <= 12; i++)
                    {
                        dt.Columns.Add("num" + i, typeof(int));
                    }
                    dt.Columns.Add("Total");
                    string zysql = @"select itemvalue,itemname from base_dataitemdetail where itemid = (select itemid from base_dataitem where itemcode = 'AccidentEventCause') order by SORTCODE ";
                    DataTable dtzy = this.BaseRepository().FindTable(zysql);
                    for (int i = 0; i < dtzy.Rows.Count; i++)
                    {
                        int count = 0;
                        DataRow dataRow = dt.NewRow();
                        dataRow["TypeName"] = dtzy.Rows[i]["itemname"].ToString();
                        for (int k = 1; k <= 12; k++)
                        {
                            string whereSQL2 = " and to_char(HAPPENTIME,'mm')=" + k.ToString();
                            string forsql =
                                string.Format(
                                    @"select nvl(count(id),0) as cou from BIS_POWERPLANTINSIDE where  instr(AccidentEventCause,'{0}')>0 and {1} {2}",
                                    dtzy.Rows[i]["itemvalue"], whereSQL, whereSQL2);
                            int num = this.BaseRepository().FindObject(forsql).ToInt();
                            dataRow["num" + k] = num;
                            count += num;                           
                        }
                        dataRow["Total"] = count.ToString();
                        dt.Rows.Add(dataRow);
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        total = 1,
                        page = 1,
                        records = dt.Rows.Count,
                        rows = dt
                    });
                    break;
                case "3":
                    string orgid = OperatorProvider.Provider.Current().OrganizeId;
                    //����
                    if (!string.IsNullOrEmpty(year.ToString()))
                    {
                        whereSQL += " and to_char(HAPPENTIME,'yyyy')='" + year + "'";
                    }
                    dt.Columns.Add("dept");          
                    string bmsql = @"select encode,fullname from BASE_DEPARTMENT where organizeid = '" + orgid + "'  and nature = '����' order by encode  desc";
                    DataTable dtbm = this.BaseRepository().FindTable(bmsql);
                    DataRow drow = dt.NewRow();
                    drow["dept"] = "����ͳ��";                
                    string totalsql = string.Format("select nvl(count(id),0) as total from BIS_POWERPLANTINSIDE where {0} ", whereSQL);
                    decimal total = this.BaseRepository().FindObject(totalsql).ToInt();
                    row["dept"] = "��ռ����";
                    for (int i = 0; i < dtbm.Rows.Count; i++)
                    {
                        dt.Columns.Add("num" + (i + 1), typeof(string));
  
          
                            string forsql =
                                string.Format(
                                    @"select nvl(count(id),0) as cou from BIS_POWERPLANTINSIDE where   instr(belongdeptcode,'{0}')>0  and {1}",
                                    dtbm.Rows[i]["encode"], whereSQL);
                            int num = this.BaseRepository().FindObject(forsql).ToInt();
                        drow["num" + (i + 1)] = num.ToString();
                        if (num != 0 && Math.Abs(total) > 0)
                        {
                            row["num" + (i + 1)] = System.Math.Round(Convert.ToDecimal(num) / total*100,2).ToString()+"%";
                        }
                        else
                        {
                            row["num" + (i + 1)] = "0%";
                        }
                        
                    }
                    dt.Columns.Add("Total");
                    drow["Total"] = total.ToString(CultureInfo.InvariantCulture);
                    row["Total"] = "100%";
                    dt.Rows.Add(drow);
                    break;
                case "6":
                    //����
                    if (!string.IsNullOrEmpty(year.ToString()))
                    {
                        whereSQL += " and to_char(HAPPENTIME,'yyyy')='" + year + "'";
                    }
                    dt.Columns.Add("cs");
                    for (int i = 1; i <= 12; i++)
                    {
                        dt.Columns.Add("num" + i, typeof(int));
                    }

                    dt.Columns.Add("Total");
                    row["cs"] = "����";
                    for (int k = 1; k <= 12; k++)
                    {
                        string whereSQL2 = " and to_char(HAPPENTIME,'mm')=" + k.ToString();
                        string forsql =
                            string.Format(@" select nvl(count(id),0) as cs from BIS_POWERPLANTINSIDE where  {0} {1}",
                                whereSQL, whereSQL2);
                        int num = this.BaseRepository().FindObject(forsql).ToInt();
                        row["num" + k] = num;
                        Total += num;
                    }

                    row["Total"] = Total.ToString();
                    break;
                case "7":
                    var newyear = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
                    if (year <= 0 || year > newyear || year < newyear - 10)
                    {
                        year = newyear - 5;
                    }
                    dt.Columns.Add("cs");
                    for (int i = year + 1; i <= newyear; i++)
                    {
                        dt.Columns.Add("num" + i, typeof(int));
                    }
                    row["cs"] = "����";
                    for (int k = year + 1; k <= newyear; k++)
                    {
                        string whereSQL2 = " and to_char(HAPPENTIME,'yyyy')='" + k + "'";
                        string forsql = string.Format(@" select nvl(count(id),0) as cs from BIS_POWERPLANTINSIDE where  {0} {1}", whereSQL, whereSQL2);
                        int num = this.BaseRepository().FindObject(forsql).ToInt();
                        row["num" + k] = num;
                    }
                    break;
                default:
                    string selectYear = "";
                    //����
                    if (!string.IsNullOrEmpty(year.ToString()))
                    {
                        selectYear = " and to_char(HAPPENTIME,'yyyy')='" + year + "'";
                    }
                    string strsql =
                               @"select itemvalue,itemname from base_dataitemdetail where itemid = (select itemid from base_dataitem where itemcode = '" + statMode + "') order by SORTCODE ";
                    DataTable dtsqlde = this.BaseRepository().FindTable(strsql);
                    dt.Columns.Add("type");
                    for (int i = 1; i <= dtsqlde.Rows.Count; i++)
                    {
                        dt.Columns.Add("num" + i, typeof(int));
                    }

                    dt.Columns.Add("Total");
                    row["type"] = "����";
                    for (int k = 1; k <= dtsqlde.Rows.Count; k++)
                    {
                        string forsql = string.Format(@" select nvl(count(id),0) as cs from BIS_POWERPLANTINSIDE where  {0} '{1}' {2}", whereSQL, dtsqlde.Rows[k - 1][0].ToString(), selectYear);
                        int num = this.BaseRepository().FindObject(forsql).ToInt();
                        row["num" + k] = num;
                        Total += num;
                    }
                    row["Total"] = Total.ToString();
                    break;
            }

            dt.Rows.Add(row);
            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                total = 1,
                page = 1,
                records = dt.Rows.Count,
                rows = dt
            });
        }

        #endregion

        /// <summary>
        /// ������¹��¼�����
        /// </summary>
        /// <returns></returns>
        public string GetAccidentEventNum()
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var roleArr = user.RoleName.Split(','); //��ǰ��Ա��ɫ
            string strRole = "";
            foreach (var rolename in roleArr)
            {
                strRole += string.Format(" flowrolename like '%{0}%' or ", rolename);
            }

            if (strRole.Length > 2)
            {
                strRole = strRole.Substring(0,strRole.Length - 3);
            }

            string strsql = string.Format(" createuserorgcode = '{0}' and ISSAVED=1  and isover <> 1  and flowdept = '{1}' and ({2})", user.OrganizeCode,user.DeptId, strRole); 
            string sql =
                string.Format(@"select count(id) num from BIS_POWERPLANTINSIDE a
            left join ( select * from base_dataitemdetail  where itemid = ( select itemid from base_dataitem where  parentid = 
            (select itemid from base_dataitem where itemname = '��λ�ڲ��챨' ) and  itemcode = 'AccidentEventType') ) b on a.accidenteventtype = b.itemvalue
              left join ( select * from base_dataitemdetail  where itemid = ( select itemid from base_dataitem where  parentid = 
            (select itemid from base_dataitem where itemname = '��λ�ڲ��챨' ) and  itemcode = 'AccidentEventProperty') ) c on a.accidenteventproperty = c.itemvalue
            left join ( select * from base_dataitemdetail  where itemid = ( select itemid from base_dataitem where   itemcode = 'SpecialtyType') ) e on a.Specialty = e.itemvalue
            where {0}", strsql);
            var num = this.BaseRepository().FindObject(sql).ToString();
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
        public void SaveForm(string keyValue, PowerplantinsideEntity entity)
        {
            entity.Id = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                PowerplantinsideEntity eEntity = GetEntity(keyValue);
                if (eEntity != null)
                {
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }
                else
                {
                    entity.Create();
                    this.BaseRepository().Insert(entity);
                }
            }
        }
        #endregion
    }
}
