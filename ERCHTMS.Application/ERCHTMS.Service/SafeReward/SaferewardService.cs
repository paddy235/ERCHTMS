using System;
using ERCHTMS.Entity.SafeReward;
using ERCHTMS.IService.SafeReward;
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
using ERCHTMS.Service.BaseManage;
using ERCHTMS.Service.OutsourcingProject;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ERCHTMS.Service.SystemManage;

namespace ERCHTMS.Service.SafeReward
{
    /// <summary>
    /// �� ������ȫ����
    /// </summary>
    public class SaferewardService : RepositoryFactory<SaferewardEntity>, SaferewardIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SaferewardEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }

        /// <summary>
        /// ��ȡ������׼
        /// </summary>
        public object GetStandardJson()
        {
            string sql = @"select  itemname,itemvalue from base_dataitemdetail where itemid =(select itemid from base_dataitem  where itemname = '���͹����׼') order by sortcode";
            DataTable dt = this.BaseRepository().FindTable(sql);
            List<object> objects = new List<object>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    objects.Add(new
                    {
                        ItemName = row["ITEMNAME"].ToString(),
                        ItemValue = row["ITEMVALUE"].ToString()
                    });
                }

            }

            return objects;
        }

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SaferewardEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// ��ҳ��ѯ������Ϣ
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
                    pagination.conditionJson += " and ((instr(ApproverPeopleIds,'" + OperatorProvider.Provider.Current().UserId + "' )> 0 and APPLYSTATE <> '0'  and FLOWSTATE ='0') or (CreateUserId = '" + OperatorProvider.Provider.Current().UserId + "'and APPLYSTATE = '0'  and FLOWSTATE !='1' )) ";
                    //pagination.conditionJson += " and instr(ApproverPeopleIds,'" + OperatorProvider.Provider.Current().UserId + "' )> 0 and APPLYSTATE <> '0' and FLOWSTATE ='0'";

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
                //��ѯ����
                if (!queryParam["keyword"].IsEmpty())
                {
                    string keyord = queryParam["keyword"].ToString().Trim();
                    pagination.conditionJson += string.Format(" and RewardUserName  like '%{0}%' or  ApplyDeptName like '%{0}%'", keyord);
                }
            }

            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }




        /// <summary>
        ///��ȡͳ������
        /// </summary>
        /// <param name="year">���</param>
        /// <returns></returns>
        public string GetRewardStatisticsCount(string year = "")
        {
            string ownorgcode = OperatorProvider.Provider.Current().OrganizeCode;
            string whereSQL = " and a.createuserorgcode='" + ownorgcode + "'";
            //����
            if (!string.IsNullOrEmpty(year))
            {
                whereSQL += " and to_char(APPLYTIME,'yyyy')='" + year + "'";
            }
            whereSQL += " and FLOWSTATE ='1'";
            List<object> dic = new List<object>();
            string strsql =
                "select encode ,fullname  from base_department where parentid =(select departmentid from base_department where encode ='" + ownorgcode + "') order by sortcode ";
            // string strsql = "select APPLYDEPTID, APPLYDEPTNAME   from bis_safereward where createuserorgcode ='" + ownorgcode + "' and FLOWSTATE ='1'  group by APPLYDEPTID, APPLYDEPTNAME";
            DataTable dt = this.BaseRepository().FindTable(strsql);
            List<string> listmonths = new List<string>();
            for (int i = 1; i <= 12; i++)
            {
                listmonths.Add(i.ToString() + "��");
            }
            foreach (DataRow item in dt.Rows)
            {
                List<int> list = new List<int>();
                for (int i = 1; i <= 12; i++)
                {
                    string whereSQL2 = " and to_char(APPLYTIME,'mm')=" + i.ToString();
                    string forsql = string.Format(@" select nvl(sum(b.rewardnum),0) as cou from bis_safereward a left join bis_saferewarddetail b on a.id=b.rewardid where  b.belongdept in (select departmentid from base_department where encode like '{0}%') {1} {2}", item["encode"], whereSQL, whereSQL2);
                    //string forsql = string.Format(@"select nvl(sum(APPLYREWARDRMB),0) as cou from bis_safereward where APPLYDEPTID='{0}' {1} {2}", item["APPLYDEPTID"], whereSQL, whereSQL2);
                    int num = this.BaseRepository().FindObject(forsql).ToInt();
                    list.Add(num);
                }
                //dic.Add(new { name = item["APPLYDEPTNAME"], data = list });
                dic.Add(new { name = item["fullname"], data = list });
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { x = listmonths, y = dic });
        }


        /// <summary>
        ///��ȡͳ������
        /// </summary>
        /// <param name="year">���</param>
        /// <returns></returns>
        public string GetRewardStatisticsTime(string year = "")
        {
            string ownorgcode = OperatorProvider.Provider.Current().OrganizeCode;
            string whereSQL = " and a.createuserorgcode='" + ownorgcode + "'";
            //����
            if (!string.IsNullOrEmpty(year))
            {
                whereSQL += " and to_char(APPLYTIME,'yyyy')='" + year + "'";
            }
            whereSQL += " and FLOWSTATE ='1'";
            List<object> dic = new List<object>();
            string strsql =
                "select encode ,fullname  from base_department where parentid =(select departmentid from base_department where encode ='" + ownorgcode + "') order by sortcode ";
            // string strsql = "select APPLYDEPTID, APPLYDEPTNAME   from bis_safereward where createuserorgcode ='" + ownorgcode + "' and FLOWSTATE ='1'  group by APPLYDEPTID, APPLYDEPTNAME";
            DataTable dt = this.BaseRepository().FindTable(strsql);
            List<string> listmonths = new List<string>();
            for (int i = 1; i <= 12; i++)
            {
                listmonths.Add(i.ToString() + "��");
            }
            foreach (DataRow item in dt.Rows)
            {
                List<int> list = new List<int>();
                for (int i = 1; i <= 12; i++)
                {
                    string whereSQL2 = " and to_char(APPLYTIME,'mm')=" + i.ToString();
                    string forsql = string.Format(@" select nvl(count(b.id),0) as cou from bis_safereward a left join bis_saferewarddetail b on a.id=b.rewardid where  b.belongdept in (select departmentid from base_department where encode like '{0}%') {1} {2}", item["encode"], whereSQL, whereSQL2);
                    //string forsql = string.Format(@"select nvl(sum(APPLYREWARDRMB),0) as cou from bis_safereward where APPLYDEPTID='{0}' {1} {2}", item["APPLYDEPTID"], whereSQL, whereSQL2);
                    int num = this.BaseRepository().FindObject(forsql).ToInt();
                    list.Add(num);
                }
                //dic.Add(new { name = item["APPLYDEPTNAME"], data = list });
                dic.Add(new { name = item["fullname"], data = list });
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { x = listmonths, y = dic });
        }


        /// <summary>
        ///��ȡͳ�Ʊ������
        /// </summary>
        /// <param name="year">���</param>
        /// <returns></returns>
        public DataTable GetRewardStatisticsList(string year = "")
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
            string sql = string.Format(@"select m.fullname,
                               nvl(sum(p.january), 0) january,
                               nvl(sum(p.february), 0) february,
                               nvl(sum(p.march), 0) march,
                               nvl(sum(p.april), 0) april,
                               nvl(sum(p.may), 0) may,
                               nvl(sum(p.june), 0) june,
                               nvl(sum(p.july), 0) july,
                               nvl(sum(p.august), 0) august,
                               nvl(sum(p.september), 0) september,
                               nvl(sum(p.october), 0) october,
                               nvl(sum(p.november), 0) november,
                               nvl(sum(p.december), 0) december,
                               nvl(sum(nvl(p.january, 0) + nvl(p.february, 0) + nvl(p.march, 0) +
                                       nvl(p.april, 0) + nvl(p.may, 0) + nvl(p.june, 0) +
                                       nvl(p.july, 0) + nvl(p.august, 0) + nvl(p.september, 0) +
                                       nvl(p.october, 0) + nvl(p.november, 0) + nvl(p.december, 0)),
                                   0) total,
                               m.encode,
                               max(m.departmentid) departmentid,
                               max(m.parentid) parentid,
                               m.sortcode
                          from base_department m
                          left join (select belongdept,
                                            january,
                                            february,
                                            march,
                                            april,
                                            may,
                                            june,
                                            july,
                                            august,
                                            september,
                                            october,
                                            november,
                                            december
                                       from (select to_char(a.APPLYTIME, 'mm') as month,
                                                    b.belongdept,
                                                    sum(b.rewardnum) total
                                               from bis_safereward a
                                               left join bis_saferewarddetail b
                                                 on a.id = b.rewardid
                                                where 1=1 {0}
                                              group by b.belongdept, to_char(a.APPLYTIME, 'mm'))
                                     pivot(max(total)
                                        for month in('01' january,
                                                    '02' february,
                                                    '03' march,
                                                    '04' april,
                                                    '05' may,
                                                    '06' june,
                                                    '07' july,
                                                    '08' august,
                                                    '09' september,
                                                    '10' october,
                                                    '11' november,
                                                    '12' december))) p
                            on m.departmentid = p.belongdept
                         where m.encode like '%{1}%'
                         group by m.fullname, m.encode,m.sortcode
                         order by m.sortcode
                        ", whereSQL, ownorgcode);
            return this.BaseRepository().FindTable(sql);
        }


        /// <summary>
        ///��ȡ���ͳ��Excel����
        /// </summary>
        /// <param name="year">���</param>
        /// <returns></returns>
        public string GetRewardStatisticsExcel(string year = "")
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
            dt.Columns.Add("DeptName");
            for (int i = 1; i <= 12; i++)
            {
                dt.Columns.Add("num" + i, typeof(int));
            }
            dt.Columns.Add("Total");
            string strsql =
                "select encode ,fullname  from base_department where parentid =(select departmentid from base_department where encode ='" + ownorgcode + "') order by sortcode ";
            //string strsql = "select APPLYDEPTID, APPLYDEPTNAME   from bis_safereward where createuserorgcode ='" + ownorgcode + "' and FLOWSTATE ='1'  group by APPLYDEPTID, APPLYDEPTNAME";
            DataTable dtsqlde = this.BaseRepository().FindTable(strsql);
            for (int i = 0; i < dtsqlde.Rows.Count; i++)
            {
                double Total = 0;
                DataRow row = dt.NewRow();
                row["DeptName"] = dtsqlde.Rows[i]["fullname"].ToString();
                //row["DeptName"] = dtsqlde.Rows[i]["APPLYDEPTNAME"].ToString();
                for (int k = 1; k <= 12; k++)
                {
                    string whereSQL2 = " and to_char(APPLYTIME,'mm')=" + k.ToString();
                    string forsql = string.Format(@" select nvl(sum(b.rewardnum),0) as cou from bis_safereward a left join bis_saferewarddetail b on a.id=b.rewardid where  b.belongdept in (select departmentid from base_department where encode like '{0}%') {1} {2}", dtsqlde.Rows[i]["encode"], whereSQL, whereSQL2);
                    //string forsql = string.Format(@"select nvl(sum(APPLYREWARDRMB),0) as cou from bis_safereward where APPLYDEPTID='{0}' {1} {2}", dtsqlde.Rows[i]["APPLYDEPTID"], whereSQL, whereSQL2);
                    int num = this.BaseRepository().FindObject(forsql).ToInt();
                    row["num" + k] = num;
                    Total += num;
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
        ///��ȡ����ͳ��Excel����
        /// </summary>
        /// <param name="year">���</param>
        /// <returns></returns>
        public string GetRewardStatisticsTimeExcel(string year = "")
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
            dt.Columns.Add("DeptName");
            for (int i = 1; i <= 12; i++)
            {
                dt.Columns.Add("num" + i, typeof(int));
            }
            dt.Columns.Add("Total");
            string strsql =
                "select encode ,fullname  from base_department where parentid =(select departmentid from base_department where encode ='" + ownorgcode + "') order by sortcode ";
            //string strsql = "select APPLYDEPTID, APPLYDEPTNAME   from bis_safereward where createuserorgcode ='" + ownorgcode + "' and FLOWSTATE ='1'  group by APPLYDEPTID, APPLYDEPTNAME";
            DataTable dtsqlde = this.BaseRepository().FindTable(strsql);
            for (int i = 0; i < dtsqlde.Rows.Count; i++)
            {
                double Total = 0;
                DataRow row = dt.NewRow();
                row["DeptName"] = dtsqlde.Rows[i]["fullname"].ToString();
                //row["DeptName"] = dtsqlde.Rows[i]["APPLYDEPTNAME"].ToString();
                for (int k = 1; k <= 12; k++)
                {
                    string whereSQL2 = " and to_char(APPLYTIME,'mm')=" + k.ToString();
                    string forsql = string.Format(@" select nvl(count(1),0) as cou from bis_safereward a left join bis_saferewarddetail b on a.id=b.rewardid where  b.belongdept in (select departmentid from base_department where encode like '{0}%') {1} {2}", dtsqlde.Rows[i]["encode"], whereSQL, whereSQL2);
                    //string forsql = string.Format(@"select nvl(sum(APPLYREWARDRMB),0) as cou from bis_safereward where APPLYDEPTID='{0}' {1} {2}", dtsqlde.Rows[i]["APPLYDEPTID"], whereSQL, whereSQL2);
                    int num = this.BaseRepository().FindObject(forsql).ToInt();
                    row["num" + k] = num;
                    Total += num;
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
        ///��ȡ���������������
        /// </summary>
        /// <param name="year">���</param>
        /// <returns></returns>
        public DataTable GetRewardStatisticsTimeList(string year = "")
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
            string sql = string.Format(@"select m.fullname,
                               nvl(sum(p.january), 0) january,
                               nvl(sum(p.february), 0) february,
                               nvl(sum(p.march), 0) march,
                               nvl(sum(p.april), 0) april,
                               nvl(sum(p.may), 0) may,
                               nvl(sum(p.june), 0) june,
                               nvl(sum(p.july), 0) july,
                               nvl(sum(p.august), 0) august,
                               nvl(sum(p.september), 0) september,
                               nvl(sum(p.october), 0) october,
                               nvl(sum(p.november), 0) november,
                               nvl(sum(p.december), 0) december,
                               nvl(sum(nvl(p.january, 0) + nvl(p.february, 0) + nvl(p.march, 0) +
                                       nvl(p.april, 0) + nvl(p.may, 0) + nvl(p.june, 0) +
                                       nvl(p.july, 0) + nvl(p.august, 0) + nvl(p.september, 0) +
                                       nvl(p.october, 0) + nvl(p.november, 0) + nvl(p.december, 0)),
                                   0) total,
                               m.encode,
                               max(m.departmentid) departmentid,
                               max(m.parentid) parentid,
                               m.sortcode
                          from base_department m
                          left join (select belongdept,
                                            january,
                                            february,
                                            march,
                                            april,
                                            may,
                                            june,
                                            july,
                                            august,
                                            september,
                                            october,
                                            november,
                                            december
                                       from (select to_char(a.APPLYTIME, 'mm') as month,
                                                    b.belongdept,
                                                    count(1) total
                                               from bis_safereward a
                                               left join bis_saferewarddetail b
                                                 on a.id = b.rewardid
                                                where 1=1 {0}
                                              group by b.belongdept, to_char(a.APPLYTIME, 'mm'))
                                     pivot(max(total)
                                        for month in('01' january,
                                                    '02' february,
                                                    '03' march,
                                                    '04' april,
                                                    '05' may,
                                                    '06' june,
                                                    '07' july,
                                                    '08' august,
                                                    '09' september,
                                                    '10' october,
                                                    '11' november,
                                                    '12' december))) p
                            on m.departmentid = p.belongdept
                         where m.encode like '%{1}%'
                         group by m.fullname, m.encode,m.sortcode
                         order by m.sortcode asc
                        ", whereSQL, ownorgcode);
            return this.BaseRepository().FindTable(sql);
        }


        /// <summary>
        /// ��ȡ�����Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public DataTable GetAptitudeInfo(string keyValue)
        {
            string node_sql = string.Format(@"select a.itemvalue id,a.itemname flowname,b.approverpeopleids,b.AUDITDEPT auditdeptname,b.AUDITPEOPLE auditusername,b.AUDITTIME auditdate,b.AUDITRESULT auditstate,b.AUDITOPINION auditremark,b.auditsignimg
            from ( select itemvalue,itemname from base_dataitemdetail  where itemid =  (select itemid from base_dataitem where itemname = '��������')) a
            left join (  select t.* , c.approverpeopleids  from  epg_aptitudeinvestigateaudit t left join bis_safereward c on  t.APTITUDEID = c.id
            where   t.disable ='1' and  t.audittime in (select  max(audittime) audittime  from epg_aptitudeinvestigateaudit  where APTITUDEID = '{0}'    group by  APTITUDEID,remark )  )  b  on   a.itemvalue =b.REMARK order by id", keyValue);

            DataTable nodeDt = this.BaseRepository().FindTable(node_sql);
            return nodeDt;
        }

        /// <summary>
        /// ��ȡ��ȫ��������ͼ����
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public Flow GetFlow(string keyValue)
        {
            List<nodes> nlist = new List<nodes>();
            List<lines> llist = new List<lines>();
            //bool isendflow = false;
            SaferewardEntity safereward = GetEntity(keyValue);
            //if (safereward.ApplyState == "5")
            //{
            //    isendflow = true;
            //}

            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string node_sql = string.Format(@"select a.itemvalue id,a.itemname flowname,b.approverpeopleids,b.AUDITDEPT auditdeptname,b.AUDITPEOPLE auditusername,b.AUDITTIME auditdate,b.AUDITRESULT auditstate,b.AUDITOPINION auditremark
            from ( select itemvalue,itemname from base_dataitemdetail  where itemid =  (select itemid from base_dataitem where itemname = '��������')) a
            left join (  select t.* , c.approverpeopleids  from  epg_aptitudeinvestigateaudit t left join bis_safereward c on  t.APTITUDEID = c.id
            where   t.disable ='1' and  t.audittime in (select  max(audittime) audittime  from epg_aptitudeinvestigateaudit  where APTITUDEID = '{0}'    group by  APTITUDEID,remark )  )  b  on   a.itemvalue =b.REMARK order by id", keyValue);

            DataTable nodeDt = this.BaseRepository().FindTable(node_sql);
            SaferewardEntity entity = GetEntity(keyValue);
            UserInfoEntity createuser = new UserInfoService().GetUserInfoEntity(entity.CreateUserId);
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DataItemModel ehsDepart = new DataItemDetailService().GetDataItemListByItemCode("'EHSDepartment'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();
            string ehsDepartCode = "";
            if (ehsDepart != null)
                ehsDepartCode = ehsDepart.ItemValue;
            if (entity.RewardMoney <= 5000)
            {
                nodeDt.Rows.RemoveAt(5);
            }
            if (createuser.Nature == "רҵ" && !createuser.RoleName.Contains("������"))
            {
            }
            else if ((createuser.Nature == "רҵ" && createuser.RoleName.Contains("������")) || (createuser.Nature == "����" && createuser.RoleName.Contains("������")) || (createuser.Nature == "����" && !createuser.RoleName.Contains("������") && createuser.DepartmentCode != ehsDepartCode))
            {
                nodeDt.Rows.RemoveAt(1);
            }
            else if ((createuser.Nature == "����" && createuser.RoleName.Contains("������") && createuser.DepartmentCode != ehsDepartCode) || (createuser.Nature == "����" && !createuser.RoleName.Contains("������") && createuser.DepartmentCode == ehsDepartCode))
            {
                nodeDt.Rows.RemoveAt(2);
                nodeDt.Rows.RemoveAt(1);
            }
            else if ((createuser.Nature == "����" && createuser.RoleName.Contains("������") && createuser.DepartmentCode == ehsDepartCode) || (!GetLeaderId().Contains(entity.CreateUserId) && !GetCeoId().Contains(entity.CreateUserId) && createuser.RoleName.Contains("��˾���û�")))
            {
                nodeDt.Rows.RemoveAt(3);
                nodeDt.Rows.RemoveAt(2);
                nodeDt.Rows.RemoveAt(1);
            }
            else if (GetLeaderId().Contains(entity.CreateUserId) && !GetCeoId().Contains(entity.CreateUserId))
            {
                nodeDt.Rows.RemoveAt(4);
                nodeDt.Rows.RemoveAt(3);
                nodeDt.Rows.RemoveAt(2);
                nodeDt.Rows.RemoveAt(1);
            }
            else if (GetCeoId().Contains(entity.CreateUserId))
            {
                if (entity.RewardMoney > 5000)
                {
                    nodeDt.Rows.RemoveAt(5);
                }
                nodeDt.Rows.RemoveAt(4);
                nodeDt.Rows.RemoveAt(3);
                nodeDt.Rows.RemoveAt(2);
                nodeDt.Rows.RemoveAt(1);
            }
            else
            {
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

                ////���̽����ڵ�
                //nodes nodes_end = new nodes();
                //nodes_end.alt = true;
                //nodes_end.isclick = false;
                //nodes_end.css = "";
                //nodes_end.id = Guid.NewGuid().ToString();
                //nodes_end.img = "";
                //nodes_end.name = "���̽���";
                //nodes_end.type = "endround";
                //nodes_end.width = 150;
                //nodes_end.height = 60;
                ////ȡ���һ���̵�λ�ã������λ
                //nodes_end.left = nlist[nlist.Count - 1].left;
                //nodes_end.top = nlist[nlist.Count - 1].top + 100;
                //nlist.Add(nodes_end);

                ////���״̬Ϊ���ͨ����ͨ�������̽������б�ʶ 
                //if (isendflow)
                //{
                //    setInfo sinfo = new setInfo();
                //    sinfo.NodeName = nodes_end.name;
                //    sinfo.Taged = 1;
                //    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                //    NodeDesignateData nodedesignatedata = new NodeDesignateData();

                //    //ȡ���̽���ʱ�Ľڵ���Ϣ
                //    DataRow[] end_rows = nodeDt.Select("id = '" + safereward.ApplyState + "'");
                //    DataRow end_row = end_rows[0];
                //    DateTime auditdate;
                //    DateTime.TryParse(end_row["auditdate"].ToString(), out auditdate);
                //    nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                //    nodedesignatedata.creatdept = end_row["auditdeptname"].ToString();
                //    nodedesignatedata.createuser = end_row["checkrolename"].ToString();
                //    nodedesignatedata.status = end_row["auditstate"].ToString() == "1" ? "ͬ��" : "��ͬ��";
                //    nodedesignatedata.prevnode = end_row["flowname"].ToString();

                //    nodelist.Add(nodedesignatedata);
                //    sinfo.NodeDesignateData = nodelist;
                //    nodes_end.setInfo = sinfo;
                //}

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

                //lines lines_end = new lines();
                //lines_end.alt = true;
                //lines_end.id = Guid.NewGuid().ToString();
                //lines_end.from = nodeDt.Rows[nodeDt.Rows.Count - 1]["id"].ToString();
                //lines_end.to = nodes_end.id;
                //llist.Add(lines_end);
                #endregion

                flow.nodes = nlist;
                flow.lines = llist;
            }
            return flow;
        }



        /// <summary>
        /// ���ݲ���id(���Ų㼶)��ȡ�������Ϣ
        /// </summary>
        /// <returns></returns>
        public List<object> GetSpecialtyPrincipal(string applyDeptId)
        {
            List<object> list = new List<object>();
            DepartmentService department = new DepartmentService();
            DepartmentEntity dept = department.GetEntity(applyDeptId);
            if (dept != null)
            {
                var data = department.GetList().Where(t => t.DeptCode.StartsWith(dept.DeptCode));

                foreach (DepartmentEntity itemEntity in data)
                {

                    DataTable tb = new UserService().GetAllTableByArgs("", itemEntity.EnCode, itemEntity.OrganizeId, "", "");
                    foreach (DataRow itemRow in tb.Rows)
                    {
                        list.Add(new
                        {
                            SpecialtyPrincipalId = itemRow["userid"],
                            SpecialtyPrincipalName = itemRow["realname"]
                        });
                        
                    }
                    
                }
            }
            return list;
        }

        /// <summary>
        /// ��ȡ�ֹ��쵼��Ϣ
        /// </summary>
        /// <returns></returns>
        public List<object> GetLeaderList()
        {
            string strsql =
                "select UserId,RealName from base_user where organizeid = '" + OperatorProvider.Provider.Current().OrganizeId + "' and instr(rolename,'��˾���û�')>0 and  instr(rolename,'��ȫ����Ա')>0";
            DataTable table = this.BaseRepository().FindTable(strsql);
            List<object> list = new List<object>();
            foreach (DataRow dr in table.Rows)
            {
                list.Add(new
                {
                    LeaderShipId = dr["UserId"].ToString(),
                    LeaderShipName = dr["RealName"].ToString()
                });
            }
            return list;
        }

        /// <summary>
        /// ��ȡ���
        /// </summary>
        /// <returns></returns>
        public string GetRewardCode()
        {
            string sql = string.Format("select saferewardcode from bis_safereward where SafeRewardCode like '%{0}%' order by createdate desc", DateTime.Now.ToString("yyyyMMdd"));
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

            return "001";
        }

        //���ݱ���������id ��ȡ���ż�����id
        public string GetDeptPId(string deptId)
        {
            DepartmentService department = new DepartmentService();
            DepartmentEntity dept = department.GetEntity(deptId);
            while (dept != null && dept.Nature != "����")
            {
                if (dept.Nature=="����")
                    break;
                if (!string.IsNullOrEmpty(dept.ParentId) && dept.ParentId!="0")
                dept = department.GetEntity(dept.ParentId);
            }
            return dept.DepartmentId;
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public string GetRewardNum()
        {
            string sql = "select count(id) num   from bis_safereward where flowstate = 0 and applystate <>0 and  instr(ApproverPeopleIds,'" + OperatorProvider.Provider.Current().UserId + "'  )> 0";
            DataTable tb = this.BaseRepository().FindTable(sql);
            return tb.Rows.Count > 0 ? tb.Rows[0][0].ToString() : "0";
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
        public void SaveForm(string keyValue, SaferewardEntity entity)
        {
            entity.ID = keyValue;
            entity.LeaderShipId = "";
            entity.LeaderShipName = "";
            if (!string.IsNullOrEmpty(keyValue))
            {
                SaferewardEntity eEntity = GetEntity(keyValue);
                if (eEntity != null)
                {
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }
                else
                {
                    entity.Create();
                    entity.ApplyState = !string.IsNullOrEmpty(entity.ApplyState) ? entity.ApplyState : "0";
                    //var userRole = OperatorProvider.Provider.Current().RoleName;
                    //if (userRole.Contains("��ͨ�û�") || userRole.Contains("���鼶�û�") || userRole.Contains("רҵ���û�"))
                    //{
                    //    entity.ApplyState = "0";
                    //}
                    //if (userRole.Contains("���ż��û�") || (userRole.Contains("���鼶�û�") && userRole.Contains("������")) || userRole.Contains("ר��"))
                    //{
                    //    entity.ApplyState = "1";
                    //}

                    //if ((userRole.Contains("���ż��û�") && userRole.Contains("������")))
                    //{
                    //    entity.ApplyState = "2";
                    //}

                    entity.FlowState = "0";
                    entity.ApproverPeopleIds = "";
                    entity.ApplyUserDeptId = OperatorProvider.Provider.Current().DeptId;
                    entity.ApplyUserDeptName = OperatorProvider.Provider.Current().DeptName;
                    this.BaseRepository().Insert(entity);
                }
            }
        }


        /// <summary>
        /// �ύ����
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void CommitApply(string keyValue, AptitudeinvestigateauditEntity aentity, string leaderId)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                SaferewardEntity entity = GetEntity(keyValue);
                UserInfoEntity createuser = new UserInfoService().GetUserInfoEntity(entity.CreateUserId);
                Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
                DataItemModel ehsDepart = new DataItemDetailService().GetDataItemListByItemCode("'EHSDepartment'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();
                string ehsDepartCode = "";
                if (ehsDepart != null)
                    ehsDepartCode = ehsDepart.ItemValue;
                entity.Modify(keyValue);
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
                        aidEntity.AUDITSIGNIMG = aentity.AUDITSIGNIMG;//����ǩ��
                        aidEntity.Disable = "1"; //����ͼ�����¼�¼
                        if (entity.ApplyState != null) aidEntity.REMARK = (entity.ApplyState).ToString(); //��ע �����̵�˳���
                        new AptitudeinvestigateauditService().SaveForm(aidEntity.ID, aidEntity);

                        #endregion
                    }

                    switch (entity.ApplyState ?? "0")
                    {
                        case "0":
                            if ((createuser.Nature == "רҵ" || createuser.Nature=="����") && !createuser.RoleName.Contains("������"))
                            {
                                entity.ApplyState = "1";
                                entity.FlowState = "0";
                                entity.ApproverPeopleIds = GetMajorUserId(createuser.DepartmentId);
                            }
                            else if (((createuser.Nature == "רҵ" || createuser.Nature == "����") && createuser.RoleName.Contains("������"))  || (createuser.Nature == "����" && !createuser.RoleName.Contains("������") && createuser.DepartmentCode != ehsDepartCode))
                            {
                                entity.ApplyState = "2";
                                entity.FlowState = "0";
                                //����ǰ��鼶������һ������
                                entity.ApproverPeopleIds = GetRoleUserId(entity.CreateUserId) ?? "";
                            }
                            else if ((createuser.Nature == "����" && createuser.RoleName.Contains("������") && createuser.DepartmentCode != ehsDepartCode) || (createuser.Nature == "����" && !createuser.RoleName.Contains("������") && createuser.DepartmentCode == ehsDepartCode))
                            {
                                entity.ApplyState = "3";
                                entity.FlowState = "0";
                                entity.ApproverPeopleIds = GetEhsUserId() ?? "";
                            }
                            else if ((createuser.Nature == "����" && createuser.RoleName.Contains("������") && createuser.DepartmentCode == ehsDepartCode) || ( !GetLeaderId().Contains(entity.CreateUserId) && !GetCeoId().Contains(entity.CreateUserId) && createuser.RoleName.Contains("��˾���û�")))
                            {
                                entity.ApplyState = "4";
                                entity.FlowState = "0";
                                entity.ApproverPeopleIds = GetLeaderId() ?? "";
                            }
                            else if (GetLeaderId().Contains(entity.CreateUserId) && !GetCeoId().Contains(entity.CreateUserId))
                            {
                                if (entity.RewardMoney > 5000)
                                {
                                    entity.ApplyState = "5";
                                    entity.FlowState = "0";
                                    entity.ApproverPeopleIds = GetCeoId() ?? "";
                                }
                                else
                                {
                                    entity.ApplyState = "6";
                                    entity.FlowState = "0";
                                    entity.FlowState = "1";
                                }
                            }
                            else if (GetCeoId().Contains(entity.CreateUserId))
                            {
                                entity.ApplyState = "6";
                                entity.FlowState = "0";
                                entity.FlowState = "1";
                            }
                            else
                            {
                                entity.ApplyState = "1";
                                entity.FlowState = "0";
                                entity.ApproverPeopleIds = GetMajorUserId(createuser.DepartmentId);
                            }
                            break;
                        case "1":
                            if (aentity.AUDITRESULT == "0")
                            {
                                entity.ApplyState = "2";

                                //����ǰ��鼶������һ������
                                entity.ApproverPeopleIds = GetRoleUserId(entity.CreateUserId) ?? "";
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
                                entity.ApproverPeopleIds = GetEhsUserId() ?? "";
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
                        case "3":
                            if (aentity.AUDITRESULT == "0")
                            {
                                entity.ApplyState = "4";
                                //entity.ApproverPeopleIds = GetLeaderId(entity.ApplyDeptId) ?? "";
                                //entity.LeaderShipId = leaderId;
                                entity.ApproverPeopleIds = GetLeaderId() ?? "";
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
                        case "4":
                            if (aentity.AUDITRESULT == "0")
                            {
                                if (entity.RewardMoney > 5000)
                                {
                                    entity.ApplyState = "5";
                                    entity.ApproverPeopleIds = GetCeoId() ?? "";
                                }
                                else
                                {
                                    entity.ApplyState = "6";
                                    entity.FlowState = "1";
                                    entity.ApproverPeopleIds = "";
                                }
                                
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
                        case "5":
                            if (aentity.AUDITRESULT == "0")
                            {
                                entity.ApplyState = "6";
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

                    this.BaseRepository().Update(entity);
                }
            }
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



        //        /// <summary>
        //        /// ���ݽ�������id��ȡ���������зֹ��쵼id
        //        /// </summary>
        //        /// <returns></returns>
        //        private string GetLeaderId(string deptId)
        //        {

        //            string sql1 =
        //                @"select itemname, itemvalue from base_dataitemdetail where itemid =(select itemid from base_dataitem  where itemname = '���͹����ɫ') and itemname like '%���ܴ�'";

        //            string ownorgcode = OperatorProvider.Provider.Current().OrganizeCode;
        //            string sql2 = string.Format(@"select count(departmentid) as num   from base_department where   organizeid ='{0}'
        //            and   fullname in ('���粿','����֧�ֲ�','EHS��')  and  departmentid = @deptId", ownorgcode);
        //            DataTable dtValue = this.BaseRepository().FindTable(sql1);
        //            int num = 0;
        //            if (dtValue.Rows.Count > 0)
        //            {
        //                DataTable dt = this.BaseRepository().FindTable(sql2, new DbParameter[] { DbParameters.CreateDbParameter("@deptId", deptId) });
        //                if (dt.Rows.Count > 0)
        //                {
        //                    num = int.Parse(dt.Rows[0][0].ToString());
        //                }

        //                return num == 1 ? dtValue.Select("itemname = '�ֹ�1#���ܴ�'")[0]["itemvalue"].ToString() :
        //                    dtValue.Select("itemname = '�ֹ�2#���ܴ�'")[0]["itemvalue"].ToString();
        //            }       
        //            return "";
        //        }

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
        #endregion
    }
}
