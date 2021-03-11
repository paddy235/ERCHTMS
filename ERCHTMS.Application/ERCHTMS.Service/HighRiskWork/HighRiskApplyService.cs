using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.Data;
using System;
using ERCHTMS.Code;

namespace ERCHTMS.Service.HighRiskWork
{
    /// <summary>
    /// �� �����߷�����ҵ�������
    /// </summary>
    public class HighRiskApplyService : RepositoryFactory<HighRiskApplyEntity>, HighRiskApplyIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<HighRiskApplyEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return this.BaseRepository().FindList(pagination);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<HighRiskApplyEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public HighRiskApplyEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// ��ȡ�����ҵ�����б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageDataTable(Pagination pagination, string queryJson)
        {
            var user = OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //��ѯ����
            if (!queryParam["status"].IsEmpty())//��ҵ���״̬
            {
                pagination.conditionJson += string.Format(" and ApplyState='{0}'", queryParam["status"].ToString());
            }
            if (!queryParam["worktype"].IsEmpty())//��ҵ����
            {
                pagination.conditionJson += string.Format(" and WorkType='{0}'", queryParam["worktype"].ToString());
            }
            //ʱ��ѡ��
            if (!queryParam["st"].IsEmpty())//��ҵ��ʼʱ��
            {
                string from = queryParam["st"].ToString().Trim();
                pagination.conditionJson += string.Format(" and WorkStartTime>=to_date('{0}','yyyy-mm-dd')", from);

            }
            if (!queryParam["et"].IsEmpty())//��ҵ����ʱ��
            {
                //string to = queryParam["et"].ToString().Trim();
                string to = Convert.ToDateTime(queryParam["et"].ToString().Trim()).AddDays(1).ToString("yyyy-MM-dd");
                pagination.conditionJson += string.Format(" and WorkEndTime<=to_date('{0}','yyyy-mm-dd')", to);
            }
            if (!queryParam["workdept"].IsEmpty() && !queryParam["workdeptid"].IsEmpty() && queryParam["workdept"].ToString() != user.OrganizeCode)//��ҵ��λ
            {
                pagination.conditionJson += string.Format(" and ApplyDeptCode in(select encode from base_department  where encode like '{0}%' or senddeptid='{1}')", queryParam["workdept"].ToString(), queryParam["workdeptid"].ToString());
            }
            if (queryParam["myself"].IsEmpty())//Ĭ��Ϊ��������(�κ�״̬)
            {
                pagination.conditionJson += string.Format(" and ApplyUserId='{0}'", user.UserId);
            }
            else
            {
                pagination.conditionJson += string.Format("  and a.id  not in(select id from bis_highriskapply where applystate='1' and  applyuserid!='{0}')", user.UserId);
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }



        /// <summary>
        /// ��ȡ���(��)�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetVerifyPageTableJson(Pagination pagination, string queryJson)
        {
            var user = OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //��ѯ����
            if (!queryParam["status"].IsEmpty())//��ҵ���״̬
            {
                pagination.conditionJson += string.Format(" and ApplyState='{0}'", queryParam["status"].ToString());
            }
            if (!queryParam["worktype"].IsEmpty())//��ҵ����
            {
                pagination.conditionJson += string.Format(" and WorkType='{0}'", queryParam["worktype"].ToString());
            }
            //ʱ��ѡ��
            if (!queryParam["st"].IsEmpty())//��ҵ��ʼʱ��
            {
                string from = queryParam["st"].ToString().Trim();
                pagination.conditionJson += string.Format(" and WorkStartTime>=to_date('{0}','yyyy-mm-dd')", from);

            }
            if (!queryParam["et"].IsEmpty())//��ҵ����ʱ��
            {
                string to = Convert.ToDateTime(queryParam["et"].ToString().Trim()).AddDays(1).ToString("yyyy-MM-dd");
                //string to = queryParam["et"].ToString().Trim();
                pagination.conditionJson += string.Format(" and WorkEndTime<=to_date('{0}','yyyy-mm-dd')", to);
            }
            if (!queryParam["workdept"].IsEmpty() && !queryParam["workdeptid"].IsEmpty() && queryParam["workdept"].ToString() != user.OrganizeCode)//��ҵ��λ
            {
                pagination.conditionJson += string.Format(" and ApplyDeptCode in(select encode from base_department  where encode like '{0}%' or senddeptid='{1}')", queryParam["workdept"].ToString(), queryParam["workdeptid"].ToString());
            }
            if (queryParam["myverify"].IsEmpty())//Ĭ��Ϊ������Ҫ���/���ļ�¼
            {
                pagination.conditionJson += " and d.approvestate='0' and ((ApplyState ='2' and d.ApproveStep='1') or (ApplyState ='4'  and d.ApproveStep='2'))";
            }
            else
            {
                //1.ȫ��(���������/�����ʹ����/����) 2.�����(��)�������ܵ�����״̬��Ρ�
                string myverify = queryParam["myverify"].ToString().Trim();
                if (myverify == "2")
                {
                    pagination.conditionJson += " and d.approvestate!='0'";
                }
                if (myverify == "1")//��Ҫ�������,������δ���,����Ϊ��˲�ͨ���ļ�¼����ʾ,��û��������һ��,Ҳ����ʾ��¼
                {
                    pagination.conditionJson += string.Format("  and approveid  not in( select approveid from bis_highriskapply a left join bis_highriskcheck  b on  a.id=b.approveid  where  b.approveperson='{0}' and  b.approvestate='0' and ((a.applystate='3') or (a.applystate='2' and  d.ApproveStep='2')))", user.UserId);
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }



        /// <summary>
        /// ��ȡ���������ҵ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetSelectDataTable(Pagination pagination, string queryJson)
        {
            var user = OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //��ѯ����
            if (!queryParam["checktype"].IsEmpty())//��ҵ����
            {
                pagination.conditionJson += string.Format(" and WorkType='{0}'", queryParam["checktype"].ToString());
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            try
            {
                db.Delete<HighRiskApplyEntity>(keyValue);
                db.Delete<HighRiskCheckEntity>(t => t.ApproveId.Equals(keyValue));
                db.Commit();
            }
            catch (Exception)
            {
                db.Rollback();
                throw;
            }
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, HighRiskApplyEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
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
        #endregion


        #region ͳ��

        /// <summary>
        /// ��ȡ���������ҵ�б�(ͳ����ת)
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetStatisticsWorkTable(Pagination pagination, string queryJson)
        {
            var user = OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //Ȩ��
            if (user.RoleName.Contains("��˾") || user.RoleName.Contains("����"))
            {
                pagination.conditionJson += string.Format(" and ApplyDeptCode like '%{0}%' ", user.OrganizeCode);
            }
            else
            {
                pagination.conditionJson += string.Format(" and  ApplyDeptCode in(select encode from base_department  where encode like '{0}%' or senddeptid='{1}')", user.DeptCode, user.DeptId);
            }
            //��ѯ����
            if (!queryParam["worktype"].IsEmpty())//��ҵ����
            {
                pagination.conditionJson += string.Format(" and WorkType='{0}'", queryParam["worktype"].ToString());
            }
            if (!queryParam["st"].IsEmpty())//��ҵ��ʼʱ��
            {
                string from = queryParam["st"].ToString().Trim();
                pagination.conditionJson += string.Format(" and WorkStartTime>=to_date('{0}','yyyy-mm-dd')", from);

            }
            //��ѯ���ڽ��е���ҵ
            if (!queryParam["mode"].IsEmpty())
            {
                if (queryParam["mode"].ToString()=="0")
                {
                    pagination.conditionJson += string.Format(" and sysdate between workstarttime and workendtime");
                }
                //else
                //{
                //    pagination.conditionJson += string.Format(" and sysdate between workstarttime and workendtime");
                //}
            }
            if (!queryParam["et"].IsEmpty())//��ҵ����ʱ��
            {
                string to = Convert.ToDateTime(queryParam["et"].ToString().Trim()).AddDays(1).ToString("yyyy-MM-dd");
                pagination.conditionJson += string.Format(" and WorkEndTime<=to_date('{0}','yyyy-mm-dd')", to);
            }
            if (!queryParam["workdept"].IsEmpty() && !queryParam["workdeptid"].IsEmpty() && queryParam["workdept"].ToString() != user.OrganizeCode)//��ҵ��λ
            {
                pagination.conditionJson += string.Format(" and ApplyDeptCode in(select encode from base_department  where encode like '{0}%' or senddeptid='{1}')", queryParam["workdept"].ToString(), queryParam["workdeptid"].ToString());
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// ����ҵ����ͳ��
        /// </summary>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public string GetHighWorkCount(string starttime, string endtime, string deptid, string deptcode)
        {
            List<object[]> list = new List<object[]>();
            Dictionary<string, int> dic = new Dictionary<string, int>();
            string sql = string.Format("select worktype,count(1) from bis_highriskapply where applystate='6'");
            var user = OperatorProvider.Provider.Current();
            //Ȩ��
            if (user.RoleName.Contains("��˾") || user.RoleName.Contains("����"))
            {
                sql += string.Format(" and ApplyDeptCode like '%{0}%' ", user.OrganizeCode);
            }
            else
            {
                sql += string.Format(" and  ApplyDeptCode in(select encode from base_department  where encode like '{0}%' or senddeptid='{1}')", user.DeptCode, user.DeptId);
            }
            if (!string.IsNullOrEmpty(deptcode) && !string.IsNullOrEmpty(deptid) && deptcode != user.OrganizeCode)
            {
                sql += string.Format(" and ApplyDeptCode in(select encode from base_department  where encode like '{0}%' or senddeptid='{1}')", deptcode, deptid);
            }
            if (!string.IsNullOrEmpty(starttime))
            {
                sql += string.Format(" and WorkStartTime>=to_date('{0}','yyyy-mm-dd')", starttime);
            }
            if (!string.IsNullOrEmpty(endtime))
            {
                endtime = Convert.ToDateTime(endtime).AddDays(1).ToString("yyyy-MM-dd");
                sql += string.Format(" and WorkEndTime<=to_date('{0}','yyyy-mm-dd')", endtime);
            }
            sql += " group by worktype";
            DataTable dt = this.BaseRepository().FindTable(sql);

            int count = dt.Select("worktype='1'").Length == 0 ? 0 : int.Parse(dt.Select("worktype='1'")[0][1].ToString());
            object[] arr = { "�ߴ���ҵ", count }; list.Add(arr);
            count = dt.Select("worktype='2'").Length == 0 ? 0 : int.Parse(dt.Select("worktype='2'")[0][1].ToString());
            arr = new object[] { "������ҵ", count }; list.Add(arr);
            count = dt.Select("worktype='3'").Length == 0 ? 0 : int.Parse(dt.Select("worktype='3'")[0][1].ToString());
            arr = new object[] { "���޿ռ���ҵ", count }; list.Add(arr);
            count = dt.Select("worktype='4'").Length == 0 ? 0 : int.Parse(dt.Select("worktype='4'")[0][1].ToString());
            arr = new object[] { "���ص�װ��ҵ", count }; list.Add(arr);
            count = dt.Select("worktype='5'").Length == 0 ? 0 : int.Parse(dt.Select("worktype='5'")[0][1].ToString());
            arr = new object[] { "��ʱ�õ���ҵ", count }; list.Add(arr);
            count = dt.Select("worktype='6'").Length == 0 ? 0 : int.Parse(dt.Select("worktype='6'")[0][1].ToString());
            arr = new object[] { "�ٽ���������ҵ", count }; list.Add(arr);
            count = dt.Select("worktype='7'").Length == 0 ? 0 : int.Parse(dt.Select("worktype='7'")[0][1].ToString());
            arr = new object[] { "������ҵ", count }; list.Add(arr);
            count = dt.Select("worktype='8'").Length == 0 ? 0 : int.Parse(dt.Select("worktype='8'")[0][1].ToString());
            arr = new object[] { "����", count }; list.Add(arr);
            dt.Dispose();

            return Newtonsoft.Json.JsonConvert.SerializeObject(list);
        }


        /// <summary>
        ///��ҵ����ͳ��(ͳ�Ʊ��)
        /// </summary>
        /// <returns></returns>
        public string GetHighWorkList(string starttime, string endtime, string deptid, string deptcode)
        {
            DataTable dtresult = GetWorkTypeInfo(starttime, endtime, deptid, deptcode);
            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                total = 1,
                page = 1,
                records = dtresult.Rows.Count,
                rows = dtresult
            });
        }


        public DataTable GetWorkTypeInfo(string starttime, string endtime, string deptid, string deptcode)
        {
            DataTable dtresult = new DataTable();
            dtresult.Columns.Add("worktype");
            dtresult.Columns.Add("worktypeval");
            dtresult.Columns.Add("typenum", typeof(int));
            dtresult.Columns.Add("percent", typeof(string));
            string sql = string.Format("select worktype,count(1) as cou from bis_highriskapply where applystate='6'");
            var user = OperatorProvider.Provider.Current();
            //Ȩ��
            if (user.RoleName.Contains("��˾") || user.RoleName.Contains("����"))
            {
                sql += string.Format(" and ApplyDeptCode like '%{0}%' ", user.OrganizeCode);
            }
            else
            {
                sql += string.Format(" and  ApplyDeptCode in(select encode from base_department  where encode like '{0}%' or senddeptid='{1}')", user.DeptCode, user.DeptId);
            }
            if (!string.IsNullOrEmpty(deptcode) && !string.IsNullOrEmpty(deptid) && deptcode != user.OrganizeCode)
            {
                sql += string.Format(" and ApplyDeptCode in(select encode from base_department  where encode like '{0}%' or senddeptid='{1}')", deptcode, deptid);
            }
            if (!string.IsNullOrEmpty(starttime))
            {
                sql += string.Format(" and WorkStartTime>=to_date('{0}','yyyy-mm-dd')", starttime);
            }
            if (!string.IsNullOrEmpty(endtime))
            {
                endtime = Convert.ToDateTime(endtime).AddDays(1).ToString("yyyy-MM-dd");
                sql += string.Format(" and WorkEndTime<=to_date('{0}','yyyy-mm-dd')", endtime);
            }
            sql += " group by worktype";
            DataTable dt = this.BaseRepository().FindTable(sql);
            int allnum = dt.Rows.Count == 0 ? 0 : Convert.ToInt32(dt.Compute("sum(cou)", "true"));
            string strsql = "select itemvalue,itemname from base_dataitemdetail b where  itemid =(select itemid from base_dataitem where itemcode='WorkType') order by itemvalue";
            DataTable dtstr = this.BaseRepository().FindTable(strsql);
            foreach (DataRow item in dtstr.Rows)
            {
                DataRow row = dtresult.NewRow();
                int count = dt.Select("worktype='" + item["itemvalue"].ToString() + "'").Length == 0 ? 0 : int.Parse(dt.Select("worktype='" + item["itemvalue"].ToString() + "'")[0][1].ToString());
                decimal percent = allnum == 0 || count == 0 ? 0 : decimal.Parse(count.ToString()) / decimal.Parse(allnum.ToString());
                percent = percent * 100;
                dtresult.Rows.Add(item["itemname"].ToString(),item["itemvalue"].ToString(), count, Math.Round(percent, 2));

            }
            return dtresult;
        }

        /// <summary>
        /// �¶�����(ͳ��ͼ)
        /// </summary>
        /// <param name="year"></param>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public string GetHighWorkYearCount(string year, string deptid, string deptcode)
        {
            string whereSQL = "";
            var user = OperatorProvider.Provider.Current();
            //Ȩ��
            if (user.RoleName.Contains("��˾") || user.RoleName.Contains("����"))
            {
                whereSQL = string.Format(" and ApplyDeptCode like '%{0}%' ", user.OrganizeCode);
            }
            else
            {
                whereSQL = string.Format(" and ApplyDeptCode in(select encode from base_department  where encode like '{0}%' or senddeptid='{1}')", user.DeptCode, user.DeptId);
            }

            //����
            if (!string.IsNullOrEmpty(year))
            {
                whereSQL += " and to_char(WorkStartTime,'yyyy')='" + year + "'";
            }
            //��ҵ��λ
            if (!string.IsNullOrEmpty(deptcode) && !string.IsNullOrEmpty(deptid) && deptcode != user.OrganizeCode)
            {
                whereSQL += string.Format(" and ApplyDeptCode in(select encode from base_department  where encode like '{0}%' or senddeptid='{1}')", deptcode, deptid);
            }
            List<string> listmonths = new List<string>();
            for (int i = 1; i <= 12; i++)
            {
                listmonths.Add(i.ToString() + "��");
            }
            List<int> list = new List<int>();
            for (int i = 1; i <= 12; i++)
            {
                string whereSQL2 = " and to_char(WorkStartTime,'mm')=" + i.ToString();
                string forsql = string.Format(@"select count(1) as cou from bis_highriskapply where applystate='6' {0} {1}", whereSQL, whereSQL2);
                int num = this.BaseRepository().FindObject(forsql).ToInt();
                list.Add(num);
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(new { y = list, x = listmonths });

        }

        /// <summary>
        /// �¶�����(���)
        /// </summary>
        /// <param name="year"></param>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public string GetHighWorkYearList(string year, string deptid, string deptcode)
        {
            string whereSQL = "";
            var user = OperatorProvider.Provider.Current();
            //Ȩ��
            if (user.RoleName.Contains("��˾") || user.RoleName.Contains("����"))
            {
                whereSQL = string.Format(" and ApplyDeptCode like '%{0}%' ", user.OrganizeCode);
            }
            else
            {
                whereSQL = string.Format(" and ApplyDeptCode in(select encode from base_department  where encode like '{0}%' or senddeptid='{1}')", user.DeptCode, user.DeptId);
            }

            //����
            if (!string.IsNullOrEmpty(year))
            {
                whereSQL += " and to_char(WorkStartTime,'yyyy')='" + year + "'";
            }
            //��ҵ��λ
            if (!string.IsNullOrEmpty(deptcode) && !string.IsNullOrEmpty(deptid) && deptcode != user.OrganizeCode)
            {
                whereSQL += string.Format(" and ApplyDeptCode in(select encode from base_department  where encode like '{0}%' or senddeptid='{1}')", deptcode, deptid);
            }
            DataTable dtresult = new DataTable();
            dtresult.Columns.Add("name");
            for (int i = 1; i <= 12; i++)
            {
                dtresult.Columns.Add("num" + i, typeof(int));
            }
            DataRow row = dtresult.NewRow();
            row["name"] = "�߷�����ҵ����";
            for (int i = 1; i <= 12; i++)
            {
                string whereSQL2 = " and to_char(WorkStartTime,'mm')=" + i.ToString();
                string forsql = string.Format(@"select count(1) as cou from bis_highriskapply where applystate='6' {0} {1}", whereSQL, whereSQL2);
                int num = this.BaseRepository().FindObject(forsql).ToInt();
                row["num" + i] = num;
            }
            dtresult.Rows.Add(row);
            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                total = 1,
                page = 1,
                records = dtresult.Rows.Count,
                rows = dtresult
            });
        }
        #endregion


        #region �ƶ���
        /// <summary>
        /// �¶�����
        /// </summary>
        /// <param name="year"></param>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataTable GetWorkYearCount(string year, string deptid, string deptcode)
        {
            string whereSQL = "";
            var user = OperatorProvider.Provider.Current();
            //Ȩ��
            if (user.RoleName.Contains("��˾") || user.RoleName.Contains("����"))
            {
                whereSQL = string.Format(" and ApplyDeptCode like '%{0}%' ", user.OrganizeCode);
            }
            else
            {
                whereSQL = string.Format(" and ApplyDeptCode in(select encode from base_department  where encode like '{0}%' or senddeptid='{1}')", user.DeptCode, user.DeptId);
            }

            //����
            if (!string.IsNullOrEmpty(year))
            {
                whereSQL += " and to_char(WorkStartTime,'yyyy')='" + year + "'";
            }
            //��ҵ��λ
            if (!string.IsNullOrEmpty(deptcode) && !string.IsNullOrEmpty(deptid) && deptcode != user.OrganizeCode)
            {
                whereSQL += string.Format(" and ApplyDeptCode in(select encode from base_department  where encode like '{0}%' or senddeptid='{1}')", deptcode, deptid);
            }
            DataTable dtresult = new DataTable();
            dtresult.Columns.Add("name");
            dtresult.Columns.Add("num", typeof(int));

            for (int i = 1; i <= 12; i++)
            {
                DataRow row = dtresult.NewRow();
                row["name"] = i.ToString() + "��";
                string whereSQL2 = " and to_char(WorkStartTime,'mm')=" + i.ToString();
                string forsql = string.Format(@"select count(1) as cou from bis_highriskapply where applystate='6' {0} {1}", whereSQL, whereSQL2);
                int num = this.BaseRepository().FindObject(forsql).ToInt();
                row["num"] = num;
                dtresult.Rows.Add(row);
            }
            return dtresult;
        }
        #endregion
    }
}
