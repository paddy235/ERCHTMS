using ERCHTMS.Entity.EngineeringManage;
using ERCHTMS.IService.EngineeringManage;
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
using ERCHTMS.Service.BaseManage;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Service.EngineeringManage
{
    /// <summary>
    /// �� ����Σ�󹤳̹���
    /// </summary>
    public class PerilEngineeringService : RepositoryFactory<PerilEngineeringEntity>, PerilEngineeringIService
    {
        private DepartmentService DepartmentService = new DepartmentService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyword = queryParam["keyword"].ToString().Trim();
                pagination.conditionJson += string.Format(" and EngineeringName like '%{0}%'", keyword);
            }
            //ʱ��ѡ��
            if (!queryParam["st"].IsEmpty())
            {
                string from = queryParam["st"].ToString().Trim();
                //pagination.conditionJson += string.Format(" and EStartTime>='{0} 00:00:00'", from);
                pagination.conditionJson += string.Format(" and  EStartTime>=to_date('{0}','yyyy-mm-dd')", from);

            }
            if (!queryParam["et"].IsEmpty())
            {
                string to = queryParam["et"].ToString().Trim();
                //pagination.conditionJson += string.Format(" and EFinishTime<='{0} 23:59:59'", to);
                pagination.conditionJson += string.Format(" and EFinishTime<=to_date('{0}','yyyy-mm-dd')", to);
            }
            if (!queryParam["type"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and engineeringtype='{0}'", queryParam["type"].ToString());
            }
            if (!queryParam["year"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and to_char(EStartTime,'yyyy')='{0}'", queryParam["year"].ToString());
            }
            if (!queryParam["month"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and to_char(EStartTime,'mm')={0}", queryParam["month"].ToString());
            }

            //ѡ�����
            if (ERCHTMS.Code.OperatorProvider.Provider.Current().RoleName.Contains("ʡ���û�"))
            {
                if (!queryParam["code"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and  belongdeptid  in (select departmentid from base_department where deptcode like '{0}%' union select ORGANIZEID from BASE_ORGANIZE where encode like '{0}%')", queryParam["code"].ToString());
                }
            }
            else
            {
                if (!queryParam["code"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and  belongdeptid  in (select departmentid from base_department where encode like '{0}%' union select ORGANIZEID from BASE_ORGANIZE where encode like '{0}%')", queryParam["code"].ToString());
                }
            }
            
            
            //��չ���
            if (!queryParam["casetype"].IsEmpty())
            {
                string casetype = queryParam["casetype"].ToString().Trim();
                pagination.conditionJson += string.Format(" and EvolveCase='{0}'", casetype);
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<PerilEngineeringEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public PerilEngineeringEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, PerilEngineeringEntity entity)
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
        ///��ȡͳ�����ݣ�Σ�󹤳�������
        /// </summary>
        /// <param name="year">���</param>
        /// <returns></returns>
        public string GetEngineeringCount(string year = "")
        {
            string ownorgcode = OperatorProvider.Provider.Current().OrganizeCode;
            string whereSQL = " and createuserorgcode='" + ownorgcode + "'";
            //����
            if (!string.IsNullOrEmpty(year))
            {
                whereSQL += " and to_char(EStartTime,'yyyy')='" + year + "'";
            }
            List<object> dic = new List<object>();
            string strsql = "select id,programmecategory from bis_engineeringsetting order by programmecategory";
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
                    string whereSQL2 = " and to_char(EStartTime,'mm')=" + i.ToString();
                    string forsql = string.Format(@"select count(1) as cou from bis_perilengineering where engineeringtype='{0}' {1} {2}", item[0], whereSQL, whereSQL2);
                    int num = this.BaseRepository().FindObject(forsql).ToInt();
                    list.Add(num);
                }
                dic.Add(new { name = item[1], data = list });
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { x = dic, y = listmonths });
        }


        /// <summary>
        ///��ȡͳ�Ʊ�����ݣ�Σ�󹤳�������
        /// </summary>
        /// <param name="year">���</param>
        /// <returns></returns>
        public string GetEngineeringList(string year = "")
        {
            string ownorgcode = OperatorProvider.Provider.Current().OrganizeCode;
            string whereSQL = " and createuserorgcode='" + ownorgcode + "'";
            //����
            if (!string.IsNullOrEmpty(year))
            {
                whereSQL += " and to_char(EStartTime,'yyyy')='" + year + "'";
            }
            DataTable dt = new DataTable();
            dt.Columns.Add("typeName");
            for (int i = 1; i <= 12; i++)
            {
                dt.Columns.Add("num" + i, typeof(int));
            }
            string strsql = "select id,programmecategory from bis_engineeringsetting order by programmecategory";
            DataTable dtsqlde = this.BaseRepository().FindTable(strsql);
            for (int i = 0; i < dtsqlde.Rows.Count; i++)
            {
                DataRow row = dt.NewRow();
                row["typeName"] = dtsqlde.Rows[i]["programmecategory"].ToString();
                for (int k = 1; k <= 12; k++)
                {
                    string whereSQL2 = " and to_char(EStartTime,'mm')=" + k.ToString();
                    string forsql = string.Format(@"select count(1) as cou from bis_perilengineering where engineeringtype='{0}' {1} {2}", dtsqlde.Rows[i]["id"], whereSQL, whereSQL2);
                    int num = this.BaseRepository().FindObject(forsql).ToInt();
                    row["num" + k] = num;
                }
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
        ///��ȡ����������ͳ������
        /// </summary>
        /// <param name="year">���</param>
        /// <returns></returns>
        public string GetEngineeringFile(string year = "")
        {
            string ownorgcode = OperatorProvider.Provider.Current().OrganizeCode;
            string whereSQL = " where createuserorgcode='" + ownorgcode + "'";
            //����
            if (!string.IsNullOrEmpty(year))
            {
                whereSQL += " and to_char(EStartTime,'yyyy')='" + year + "'";
            }
            DataTable dt = new DataTable();
            dt.Columns.Add("num1", typeof(int));
            dt.Columns.Add("num2", typeof(int));
            dt.Columns.Add("num3", typeof(int));
            List<object> data = new List<object>();
            List<string> xValues = new List<string>();
            List<int> data1 = new List<int>(); List<int> data2 = new List<int>(); List<int> data3 = new List<int>();
            for (int k = 1; k <= 12; k++)
            {
                string whereSQL2 = " and to_char(EStartTime,'mm')=" + k.ToString();
                string forsql = string.Format(@"select count(1) from bis_perilengineering {0}{1}", whereSQL + " and createuserorgcode='" + ownorgcode + "'", whereSQL2);
                int num = this.BaseRepository().FindObject(forsql).ToInt();
                string forsql2 = string.Format(@"select count(1) from base_fileinfo where  recid in(select ConstructFiles from bis_perilengineering {0}{1})", whereSQL + " and createuserorgcode='" + ownorgcode + "'", whereSQL2);
                int num1 = this.BaseRepository().FindObject(forsql2).ToInt() > 0 ? 1 : 0;
                string forsql3 = string.Format(@"select count(1) from base_fileinfo where  recid in(select TaskFiles from bis_perilengineering {0}{1})", whereSQL + " and createuserorgcode='" + ownorgcode + "'", whereSQL2);
                int num2 = this.BaseRepository().FindObject(forsql3).ToInt() > 0 ? 1 : 0;
                data1.Add(num);
                data2.Add(num1);
                data3.Add(num2);
                xValues.Add(k + "��");
            }
            data.Add(new { name = "��������", data = data1 }); data.Add(new { name = "��������", data = data2 }); data.Add(new { name = "��ȫ�������׷���", data = data3 });
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { xValues = xValues, data = data });
        }

        /// <summary>
        ///��ȡ����������ͳ������(���)
        /// </summary>
        /// <param name="year">���</param>
        /// <returns></returns>
        public string GetEngineeringFileGrid(string year = "")
        {
            string ownorgcode = OperatorProvider.Provider.Current().OrganizeCode;
            string whereSQL = " where createuserorgcode='" + ownorgcode + "'";
            //����
            if (!string.IsNullOrEmpty(year))
            {
                whereSQL += " and to_char(EStartTime,'yyyy')='" + year + "'";
            }
            DataTable dt = new DataTable();
            dt.Columns.Add("months");
            dt.Columns.Add("num1", typeof(int));
            dt.Columns.Add("num2", typeof(int));
            dt.Columns.Add("num3", typeof(int));
            for (int k = 1; k <= 12; k++)
            {
                DataRow row = dt.NewRow();
                string whereSQL2 = " and to_char(EStartTime,'mm')=" + k.ToString();
                string forsql = string.Format(@"select count(1) from bis_perilengineering {0}{1}", whereSQL + " and createuserorgcode='" + ownorgcode + "'", whereSQL2);
                int num = this.BaseRepository().FindObject(forsql).ToInt();
                string forsql2 = string.Format(@"select count(1) from base_fileinfo where  recid in(select ConstructFiles from bis_perilengineering {0}{1})", whereSQL + " and createuserorgcode='" + ownorgcode + "'", whereSQL2);
                int num1 = this.BaseRepository().FindObject(forsql2).ToInt() > 0 ? 1 : 0;
                string forsql3 = string.Format(@"select count(1) from base_fileinfo where  recid in(select TaskFiles from bis_perilengineering {0}{1})", whereSQL + " and createuserorgcode='" + ownorgcode + "'", whereSQL2);
                int num2 = this.BaseRepository().FindObject(forsql3).ToInt() > 0 ? 1 : 0;
                row["num1"] = num;
                row["num2"] = num1;
                row["num3"] = num2;
                row["months"] = (k + "��").ToString();
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
        ///Σ�󹤳�������ͳ��
        /// </summary>
        /// <param name="year">ͳ�����</param>
        /// <returns></returns>
        public string GetEngineeringCase(string year = "")
        {
            string ownorgcode = OperatorProvider.Provider.Current().OrganizeCode;
            string whereSQL = " where createuserorgcode='" + ownorgcode + "'";
            //����
            if (!string.IsNullOrEmpty(year))
            {
                whereSQL += " and to_char(EStartTime,'yyyy')='" + year + "'";
            }
            List<object> data = new List<object>();
            List<string> xValues = new List<string>();
            List<int> data1 = new List<int>(); List<int> data2 = new List<int>(); List<int> data3 = new List<int>(); List<int> data4 = new List<int>();
            for (int k = 1; k <= 12; k++)
            {
                string whereSQL2 = " and to_char(EStartTime,'mm')=" + k.ToString();
                string forsql = string.Format(@"select evolvecase,count(1) from bis_perilengineering {0} {1} group by evolvecase", whereSQL, whereSQL2);
                DataTable dtresult = this.BaseRepository().FindTable(forsql);
                if (dtresult != null)
                {
                    int count1 = dtresult.Select("evolvecase='����ʩ��'").Length == 0 ? 0 : int.Parse(dtresult.Select("evolvecase='����ʩ��'")[0][1].ToString());
                    int count2 = dtresult.Select("evolvecase='δʩ��'").Length == 0 ? 0 : int.Parse(dtresult.Select("evolvecase='δʩ��'")[0][1].ToString());
                    int count3 = dtresult.Select("evolvecase='���깤'").Length == 0 ? 0 : int.Parse(dtresult.Select("evolvecase='���깤'")[0][1].ToString());
                    int sum = count3 + count2 + count1;
                    data1.Add(count1);
                    data2.Add(count2);
                    data3.Add(count3);
                    data4.Add(sum);
                }
                xValues.Add(k + "��");
            }
            data.Add(new { name = "��������", data = data4 });
            data.Add(new { name = "����ʩ��", data = data1 });
            data.Add(new { name = "δʩ��", data = data2 });
            data.Add(new { name = "���깤", data = data3 });
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { xValues = xValues, data = data });
        }



        /// <summary>
        ///Σ�󹤳�������ͳ��(���)
        /// </summary>
        /// <param name="year">ͳ�����</param>
        /// <returns></returns>
        public string GetEngineeringCaseGrid(string year = "")
        {
            string ownorgcode = OperatorProvider.Provider.Current().OrganizeCode;
            string whereSQL = " where createuserorgcode='" + ownorgcode + "'";
            //����
            if (!string.IsNullOrEmpty(year))
            {
                whereSQL += " and to_char(EStartTime,'yyyy')='" + year + "'";
            }
            DataTable dt = new DataTable();
            dt.Columns.Add("months");
            dt.Columns.Add("num1", typeof(int));
            dt.Columns.Add("num2", typeof(int));
            dt.Columns.Add("num3", typeof(int));
            dt.Columns.Add("num4", typeof(int));
            for (int k = 1; k <= 12; k++)
            {
                DataRow row = dt.NewRow();
                string whereSQL2 = " and to_char(EStartTime,'mm')=" + k.ToString();
                string forsql = string.Format(@"select evolvecase,count(1) from bis_perilengineering {0} {1} group by evolvecase", whereSQL, whereSQL2);
                DataTable dtresult = this.BaseRepository().FindTable(forsql);
                if (dtresult != null)
                {
                    int count1 = dtresult.Select("evolvecase='����ʩ��'").Length == 0 ? 0 : int.Parse(dtresult.Select("evolvecase='����ʩ��'")[0][1].ToString());
                    int count2 = dtresult.Select("evolvecase='δʩ��'").Length == 0 ? 0 : int.Parse(dtresult.Select("evolvecase='δʩ��'")[0][1].ToString());
                    int count3 = dtresult.Select("evolvecase='���깤'").Length == 0 ? 0 : int.Parse(dtresult.Select("evolvecase='���깤'")[0][1].ToString());
                    int sum = count3 + count2 + count1;
                    row["num1"] = count1;
                    row["num2"] = count2;
                    row["num3"] = count3;
                    row["num4"] = sum;
                }
                row["months"] = (k + "��").ToString();
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
        ///��λ�ڲ�������ί��λ�Ա�
        /// </summary>
        /// <param name="year">ͳ�����</param>
        /// <param name="month">ͳ���·�</param>
        /// <returns></returns>
        public string GetEngineeringContrast(string year = "", string month = "")
        {
            string ownorgcode = OperatorProvider.Provider.Current().OrganizeCode;
            string whereSQL = "";
            if (!string.IsNullOrEmpty(year))
            {
                whereSQL = "  and to_char(EStartTime,'yyyy')='" + year + "'";
            }
            if (!string.IsNullOrEmpty(month))
            {
                whereSQL += "  and to_char(EStartTime,'mm')='" + month + "'";
            }
            List<object> data = new List<object>();
            List<string> xValues = new List<string>();
            xValues.Add("��������");
            xValues.Add("����ʩ��");
            xValues.Add("δʩ��");
            xValues.Add("���깤");
            xValues.Add("��������");
            xValues.Add("������������");
            List<int> data1 = new List<int>(); List<int> data2 = new List<int>();
            for (int k = 1; k <= 2; k++)
            {
                string whereSQL2 = " where unittype='" + k + "'";
                string forsql = string.Format(@"select evolvecase,count(1) from bis_perilengineering {1} {0}  group by evolvecase ", whereSQL + " and createuserorgcode='" + ownorgcode + "'", whereSQL2);
                DataTable dtresult = this.BaseRepository().FindTable(forsql);
                if (dtresult != null)
                {
                    int count1 = dtresult.Select("evolvecase='����ʩ��'").Length == 0 ? 0 : int.Parse(dtresult.Select("evolvecase='����ʩ��'")[0][1].ToString());
                    int count2 = dtresult.Select("evolvecase='δʩ��'").Length == 0 ? 0 : int.Parse(dtresult.Select("evolvecase='δʩ��'")[0][1].ToString());
                    int count3 = dtresult.Select("evolvecase='���깤'").Length == 0 ? 0 : int.Parse(dtresult.Select("evolvecase='���깤'")[0][1].ToString());
                    int sum = count3 + count2 + count1;
                    if (k == 1)
                    {
                        data1.Add(sum);
                        data1.Add(count1);
                        data1.Add(count2);
                        data1.Add(count3);

                    }
                    else
                    {
                        data2.Add(sum);
                        data2.Add(count1);
                        data2.Add(count2);
                        data2.Add(count3);
                    }
                }
                string forsql2 = string.Format(@"select count(1) from base_fileinfo where  recid in(select ConstructFiles from bis_perilengineering {1}{0})", whereSQL + " and createuserorgcode='" + ownorgcode + "'", whereSQL2);
                int num1 = this.BaseRepository().FindObject(forsql2).ToInt() > 0 ? 1 : 0;
                string forsql3 = string.Format(@"select count(1) from base_fileinfo where  recid in(select TaskFiles from bis_perilengineering {1}{0})", whereSQL + " and createuserorgcode='" + ownorgcode + "'", whereSQL2);
                int num2 = this.BaseRepository().FindObject(forsql3).ToInt() > 0 ? 1 : 0;
                if (k == 1)
                {
                    data1.Add(num1);
                    data1.Add(num2);
                }
                else
                {
                    data2.Add(num1);
                    data2.Add(num2);
                }
            }
            data.Add(new { name = "�ڲ�", data = data1 });
            data.Add(new { name = "�ⲿ", data = data2 });
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { xValues = xValues, data = data });
        }


        /// <summary>
        ///��λ�ڲ�������ί��λ�Ա�
        /// </summary>
        /// <param name="year">ͳ�����</param>
        /// <param name="month">ͳ���·�</param>
        /// <returns></returns>
        public string GetEngineeringContrastGrid(string year = "", string month = "")
        {
            string ownorgcode = OperatorProvider.Provider.Current().OrganizeCode;
            string whereSQL = "";
            if (!string.IsNullOrEmpty(year))
            {
                whereSQL = "  and to_char(EStartTime,'yyyy')='" + year + "'";
            }
            if (!string.IsNullOrEmpty(month))
            {
                whereSQL += "  and to_char(EStartTime,'mm')='" + month + "'";
            }
            DataTable dt = new DataTable();
            dt.Columns.Add("nametype");
            dt.Columns.Add("num1", typeof(int));
            dt.Columns.Add("num2", typeof(int));
            dt.Columns.Add("num3", typeof(int));
            dt.Columns.Add("num4", typeof(int));
            dt.Columns.Add("num5", typeof(int));
            dt.Columns.Add("num6", typeof(int));
            for (int k = 1; k <= 2; k++)
            {
                DataRow row = dt.NewRow();
                if (k == 1)
                {
                    row["nametype"] = "��λ�ڲ�";
                }
                if (k == 2)
                {
                    row["nametype"] = "�������λ";
                }
                string whereSQL2 = " where unittype='" + k + "'";
                string forsql = string.Format(@"select evolvecase,count(1) from bis_perilengineering {1} {0}  group by evolvecase", whereSQL + " and createuserorgcode='" + ownorgcode + "'", whereSQL2);
                DataTable dtresult = this.BaseRepository().FindTable(forsql);
                if (dtresult != null)
                {
                    int count1 = dtresult.Select("evolvecase='����ʩ��'").Length == 0 ? 0 : int.Parse(dtresult.Select("evolvecase='����ʩ��'")[0][1].ToString());
                    int count2 = dtresult.Select("evolvecase='δʩ��'").Length == 0 ? 0 : int.Parse(dtresult.Select("evolvecase='δʩ��'")[0][1].ToString());
                    int count3 = dtresult.Select("evolvecase='���깤'").Length == 0 ? 0 : int.Parse(dtresult.Select("evolvecase='���깤'")[0][1].ToString());
                    int sum = count3 + count2 + count1;
                    row["num1"] = sum;
                    row["num2"] = count1;
                    row["num3"] = count2;
                    row["num4"] = count3;
                }
                string forsql2 = string.Format(@"select count(1) from base_fileinfo where  recid in(select ConstructFiles from bis_perilengineering {1}{0})", whereSQL + " and createuserorgcode='" + ownorgcode + "'", whereSQL2);
                int num1 = this.BaseRepository().FindObject(forsql2).ToInt();
                string forsql3 = string.Format(@"select count(1) from base_fileinfo where  recid in(select TaskFiles from bis_perilengineering {1}{0})", whereSQL + " and createuserorgcode='" + ownorgcode + "'", whereSQL2);
                int num2 = this.BaseRepository().FindObject(forsql3).ToInt();
                row["num5"] = num1 > 0 ? 1 : 0;
                row["num6"] = num2 > 0 ? 1 : 0;
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
        #endregion


        #region ʡ��ͳ��
        /// <summary>
        ///���糧��λ�Ա�
        /// </summary>
        /// <param name="year">ͳ�����</param>
        /// <returns></returns>
        public string GetEngineeringContrastForSJ(string year = "")
        {
            string ownorgcode = OperatorProvider.Provider.Current().OrganizeCode;
            string whereSQL = string.Format(" where belongdeptid  in (select departmentid from base_department where deptcode like '{0}%' union select ORGANIZEID from BASE_ORGANIZE where encode like '{0}%')", ownorgcode);
            if (!string.IsNullOrEmpty(year))
            {
                whereSQL += "  and to_char(EStartTime,'yyyy')='" + year + "'";
            }
            List<object> data = new List<object>();
            List<int> data1 = new List<int>();
            List<string> xValues = new List<string>();
            IEnumerable<DepartmentEntity> orgcodelist = new List<DepartmentEntity>();
            orgcodelist = DepartmentService.GetList().Where(t => t.DeptCode.Contains(OperatorProvider.Provider.Current().NewDeptCode) && t.Nature == "����");
            foreach (DepartmentEntity item in orgcodelist)
            {
                xValues.Add(item.FullName);
            }
            
            string forsql = string.Format(@"select createuserorgcode,count(1) from bis_perilengineering {0}  group by createuserorgcode ", whereSQL);
            DataTable dtresult = this.BaseRepository().FindTable(forsql);
            int total = 0;
            if (dtresult != null)
            {
                int count1 = 0;
                foreach (DepartmentEntity item in orgcodelist)
                {
                    count1 = dtresult.Select("createuserorgcode='" + item.EnCode + "'").Length == 0 ? 0 : int.Parse(dtresult.Select("createuserorgcode='" + item.EnCode + "'")[0][1].ToString());
                    data1.Add(count1);
                    total += count1;
                }
                
            }
            data.Add(new { name = year, data = data1 });
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { xValues = xValues, data = data, total = total });
        }


        /// <summary>
        ///���糧��λ�Ա�ͳ�Ʊ��
        /// </summary>
        /// <param name="year">ͳ�����</param>
        /// <returns></returns>
        public DataTable GetEngineeringContrastGridForSJ(string year = "")
        {
            string ownorgcode = OperatorProvider.Provider.Current().OrganizeCode;
            string whereSQL = string.Format(" where belongdeptid  in (select departmentid from base_department where deptcode like '{0}%' union select ORGANIZEID from BASE_ORGANIZE where encode like '{0}%')", ownorgcode);
            if (!string.IsNullOrEmpty(year))
            {
                whereSQL += "  and to_char(EStartTime,'yyyy')='" + year + "'";
            }
            DataTable dt = new DataTable();
            dt.Columns.Add("orgcode");
            dt.Columns.Add("typename");
            dt.Columns.Add("num");
            dt.Columns.Add("rate");
            IEnumerable<DepartmentEntity> orgcodelist = new List<DepartmentEntity>();
            orgcodelist = DepartmentService.GetList().Where(t => t.DeptCode.Contains(OperatorProvider.Provider.Current().NewDeptCode) && t.Nature == "����");

            string forsql = string.Format(@"select createuserorgcode,count(1) from bis_perilengineering {0}  group by createuserorgcode ", whereSQL);
            DataTable dtresult = this.BaseRepository().FindTable(forsql);
            int total = 0;
            if (dtresult != null)
            {
                int count1 = 0;
                foreach (DepartmentEntity item in orgcodelist)
                {
                    count1 = dtresult.Select("createuserorgcode='" + item.EnCode + "'").Length == 0 ? 0 : int.Parse(dtresult.Select("createuserorgcode='" + item.EnCode + "'")[0][1].ToString());
                    total += count1;
                }
                
                foreach (DepartmentEntity item in orgcodelist)
                {
                    DataRow dtRow = dt.NewRow();
                    count1 = dtresult.Select("createuserorgcode='" + item.EnCode + "'").Length == 0 ? 0 : int.Parse(dtresult.Select("createuserorgcode='" + item.EnCode + "'")[0][1].ToString());
                    dtRow["orgcode"] = item.DeptCode;
                    dtRow["typename"] = item.FullName;
                    dtRow["num"] = count1;
                    dtRow["rate"] = total == 0 ? "0.00%" : (((decimal)count1) / total).ToString("0.00%");
                    dt.Rows.Add(dtRow);
                }

            }
            return dt;
        }

        /// <summary>
        /// �������ͼ��
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public string GetEngineeringCategoryForSJ(string year = "")
        {
            string ownorgcode = OperatorProvider.Provider.Current().OrganizeCode;
            string whereSQL = string.Format(" where belongdeptid  in (select departmentid from base_department where deptcode like '{0}%' union select ORGANIZEID from BASE_ORGANIZE where encode like '{0}%')", ownorgcode);
            if (!string.IsNullOrEmpty(year))
            {
                whereSQL += "  and to_char(EStartTime,'yyyy')='" + year + "'";
            }
            List<object> data = new List<object>();
            List<object> data1 = new List<object>();

            string forsql = string.Format(@"select programmecategory,count(1) from bis_perilengineering a left join bis_engineeringsetting b on a.EngineeringType = b.id {0}  group by programmecategory  ", whereSQL);
            DataTable dtresult = this.BaseRepository().FindTable(forsql);
            int total = 0;
            if (dtresult != null)
            {
                foreach (DataRow item in dtresult.Rows)
                {
                    data1.Add(new { name = item[0].ToString(), y = int.Parse(item[1].ToString()) });
                    total += int.Parse(item[1].ToString());
                }
            }
            data.Add(new { name = year, data = data1, colorByPoint = true });
            return Newtonsoft.Json.JsonConvert.SerializeObject(new {data = data, total = total });
        }

        /// <summary>
        /// �������ͳ�Ʊ��
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public DataTable GetEngineeringCategoryGridForSJ(string year = "")
        {
            string ownorgcode = OperatorProvider.Provider.Current().OrganizeCode;
            string whereSQL = string.Format(" where belongdeptid  in (select departmentid from base_department where deptcode like '{0}%' union select ORGANIZEID from BASE_ORGANIZE where encode like '{0}%')", ownorgcode);
            if (!string.IsNullOrEmpty(year))
            {
                whereSQL += "  and to_char(EStartTime,'yyyy')='" + year + "'";
            }
            DataTable dt = new DataTable();
            dt.Columns.Add("type");
            dt.Columns.Add("typename");
            dt.Columns.Add("num");
            dt.Columns.Add("rate");

            string forsql = string.Format(@"select b.id,programmecategory,count(1) from bis_perilengineering a left join bis_engineeringsetting b on a.EngineeringType = b.id {0}  group by b.id, programmecategory  ", whereSQL);
            DataTable dtresult = this.BaseRepository().FindTable(forsql);
            int total = 0;
            if (dtresult != null)
            {
                foreach (DataRow item in dtresult.Rows)
                {
                    total += int.Parse(item[2].ToString());
                }
                foreach (DataRow item in dtresult.Rows)
                {
                    DataRow dtRow = dt.NewRow();
                    dtRow["type"] = item[0];
                    dtRow["typename"] = item[1];
                    dtRow["num"] = item[2];
                    dtRow["rate"] = total == 0 ? "0.00%" : (((decimal)int.Parse(item[2].ToString())) / total).ToString("0.00%");
                    dt.Rows.Add(dtRow);
                }
            }
            return dt;
        }

        /// <summary>
        /// �¶�����ͼ��
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public string GetEngineeringMonthForSJ(string year = "")
        {
            string ownorgcode = OperatorProvider.Provider.Current().OrganizeCode;
            string whereSQL = string.Format(" where belongdeptid  in (select departmentid from base_department where deptcode like '{0}%' union select ORGANIZEID from BASE_ORGANIZE where encode like '{0}%')", ownorgcode);
            if (!string.IsNullOrEmpty(year))
            {
                whereSQL += "  and to_char(EStartTime,'yyyy')='" + year + "'";
            }
            List<object> data = new List<object>();
            List<int> data1 = new List<int>();
            List<string> xValues = new List<string>();
            for (int i = 1; i <= 12; i++)
            {
                string whereSQL1 = " and to_char(EStartTime,'mm')=" + i;
                string forsql = string.Format(@"select count(1) as cou from bis_perilengineering  {0} {1} ", whereSQL, whereSQL1);
                int result = int.Parse(this.BaseRepository().FindTable(forsql).Rows[0][0].ToString());
                data1.Add(result);
                xValues.Add(i + "��");
            }

            data.Add(new { name = year, data = data1 });
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { data = data, xValues = xValues });
        }


        /// <summary>
        /// �¶����Ʊ��
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public DataTable GetEngineeringMonthGridForSJ(string year = "")
        {
            string ownorgcode = OperatorProvider.Provider.Current().OrganizeCode;
            string whereSQL = string.Format(" and belongdeptid  in (select departmentid from base_department where deptcode like '{0}%' union select ORGANIZEID from BASE_ORGANIZE where encode like '{0}%')", ownorgcode);
            //����
            if (!string.IsNullOrEmpty(year))
            {
                whereSQL += " and to_char(EStartTime,'yyyy')='" + year + "'";
            }
            DataTable dt = new DataTable();
            dt.Columns.Add("type");
            dt.Columns.Add("typeName");
            for (int i = 1; i <= 12; i++)
            {
                dt.Columns.Add("num" + i, typeof(int));
            }
            string strsql = "select id,programmecategory from bis_engineeringsetting order by programmecategory";
            DataTable dtsqlde = this.BaseRepository().FindTable(strsql);
            for (int i = 0; i < dtsqlde.Rows.Count; i++)
            {
                DataRow row = dt.NewRow();
                row["type"] = dtsqlde.Rows[i]["id"].ToString();
                row["typeName"] = dtsqlde.Rows[i]["programmecategory"].ToString();
                for (int k = 1; k <= 12; k++)
                {
                    string whereSQL2 = " and to_char(EStartTime,'mm')=" + k.ToString();
                    string forsql = string.Format(@"select count(1) as cou from bis_perilengineering where engineeringtype='{0}' {1} {2}", dtsqlde.Rows[i]["id"], whereSQL, whereSQL2);
                    int num = this.BaseRepository().FindObject(forsql).ToInt();
                    row["num" + k] = num;
                }
                dt.Rows.Add(row);
            }
            return dt;
        }


        /// <summary>
        /// ��ʱֻ��ʡ��Σ�󹤳�ͳ��
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public string GetPerilForSJIndex(string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            string whereSQL = "";
            string ownorgcode = OperatorProvider.Provider.Current().OrganizeCode;
            DataTable dt = new DataTable();
            dt.Columns.Add("num1", typeof(int));
            dt.Columns.Add("num2", typeof(int));
            dt.Columns.Add("num3", typeof(int));
            dt.Columns.Add("num4", typeof(int));
            //ѡ�����
            if (ERCHTMS.Code.OperatorProvider.Provider.Current().RoleName.Contains("ʡ���û�"))
            {
                whereSQL += " where belongdeptid  in (select departmentid from base_department where deptcode like '" + OperatorProvider.Provider.Current().NewDeptCode + "%' union select ORGANIZEID from BASE_ORGANIZE where encode like '" + OperatorProvider.Provider.Current().NewDeptCode + "%')";
                if (!queryParam["code"].IsEmpty())
                {
                    whereSQL += string.Format(" and  belongdeptid  in (select departmentid from base_department where deptcode like '{0}%' union select ORGANIZEID from BASE_ORGANIZE where encode like '{0}%')", queryParam["code"].ToString());
                }
            }
            else
            {
                if (!queryParam["code"].IsEmpty())
                {
                    whereSQL += string.Format(" and  belongdeptid  in (select departmentid from base_department where encode like '{0}%' union select ORGANIZEID from BASE_ORGANIZE where encode like '{0}%')", queryParam["code"].ToString());
                }
            }
            if (!queryParam["st"].IsEmpty())
            {
                whereSQL += string.Format(" and  EStartTime>=to_date('{0}','yyyy-mm-dd')", queryParam["st"].ToString());
            }
            if (!queryParam["et"].IsEmpty())
            {
                whereSQL += string.Format(" and EFinishTime<=to_date('{0}','yyyy-mm-dd')", queryParam["et"].ToString());
            }
            if (!queryParam["keyword"].IsEmpty())
            {
                whereSQL += string.Format(" and EngineeringName like '%{0}%'", queryParam["keyword"].ToString());
            }
            if (!queryParam["type"].IsEmpty())
            {
                whereSQL += string.Format(" and engineeringtype='{0}'", queryParam["type"].ToString());
            }
            if (!queryParam["year"].IsEmpty())
            {
                whereSQL += string.Format(" and to_char(EStartTime,'yyyy')='{0}'", queryParam["year"].ToString());
            }
            if (!queryParam["month"].IsEmpty())
            {
                whereSQL += string.Format(" and to_char(EStartTime,'mm')={0}", queryParam["month"].ToString());
            }
            string forsql = string.Format(@"select evolvecase,count(1) from bis_perilengineering {0} group by evolvecase", whereSQL);
            DataTable dtresult = this.BaseRepository().FindTable(forsql);
            if (dtresult != null)
            {
                DataRow row = dt.NewRow();
                int count1 = dtresult.Select("evolvecase='����ʩ��'").Length == 0 ? 0 : int.Parse(dtresult.Select("evolvecase='����ʩ��'")[0][1].ToString());
                int count2 = dtresult.Select("evolvecase='δʩ��'").Length == 0 ? 0 : int.Parse(dtresult.Select("evolvecase='δʩ��'")[0][1].ToString());
                int count3 = dtresult.Select("evolvecase='���깤'").Length == 0 ? 0 : int.Parse(dtresult.Select("evolvecase='���깤'")[0][1].ToString());
                int sum = count3 + count2 + count1;
                row["num1"] = count1;
                row["num2"] = count2;
                row["num3"] = count3;
                row["num4"] = sum;
                dt.Rows.Add(row);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(dt);
        }

        /// <summary>
        /// ��ȡ�������
        /// </summary>
        /// <returns></returns>
        public DataTable GetEngineeringType()
        {
            DataTable dt = new DataTable();
            string strsql = "select id itemValue,programmecategory itemName from bis_engineeringsetting";
            dt = this.BaseRepository().FindTable(strsql);
            return dt;
        }
        #endregion

        public string GetPeril(string code = "", string st = "", string et = "", string keyword = "")
        {
            string whereSQL = "";
            string ownorgcode = OperatorProvider.Provider.Current().OrganizeCode;
            DataTable dt = new DataTable();
            dt.Columns.Add("num1", typeof(int));
            dt.Columns.Add("num2", typeof(int));
            dt.Columns.Add("num3", typeof(int));
            dt.Columns.Add("num4", typeof(int));
            code = string.IsNullOrEmpty(code) ? ownorgcode : code;
            if (ERCHTMS.Code.OperatorProvider.Provider.Current().RoleName.Contains("ʡ���û�"))
            {
                whereSQL = "where  belongdeptid  in (select departmentid from base_department where deptcode like '" + code + "%' union select ORGANIZEID from BASE_ORGANIZE where encode like '" + code + "%')";
            }
            else
            {
                whereSQL = "where  belongdeptid  in (select departmentid from base_department where encode like '" + code + "%' union select ORGANIZEID from BASE_ORGANIZE where encode like '" + code + "%')";
            }
            if (!string.IsNullOrEmpty(st))
            {
                whereSQL += string.Format(" and  EStartTime>=to_date('{0}','yyyy-mm-dd')", st);
            }
            if (!string.IsNullOrEmpty(et))
            {
                whereSQL += string.Format(" and EFinishTime<=to_date('{0}','yyyy-mm-dd')", et);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                whereSQL += string.Format(" and EngineeringName like '%{0}%'", keyword);
            }
            string forsql = string.Format(@"select evolvecase,count(1) from bis_perilengineering {0} group by evolvecase", whereSQL);
            DataTable dtresult = this.BaseRepository().FindTable(forsql);
            if (dtresult != null)
            {
                DataRow row = dt.NewRow();
                int count1 = dtresult.Select("evolvecase='����ʩ��'").Length == 0 ? 0 : int.Parse(dtresult.Select("evolvecase='����ʩ��'")[0][1].ToString());
                int count2 = dtresult.Select("evolvecase='δʩ��'").Length == 0 ? 0 : int.Parse(dtresult.Select("evolvecase='δʩ��'")[0][1].ToString());
                int count3 = dtresult.Select("evolvecase='���깤'").Length == 0 ? 0 : int.Parse(dtresult.Select("evolvecase='���깤'")[0][1].ToString());
                int sum = count3 + count2 + count1;
                row["num1"] = count1;
                row["num2"] = count2;
                row["num3"] = count3;
                row["num4"] = sum;
                dt.Rows.Add(row);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(dt);
        }

        #region app�ӿ�
        public DataTable GetPerilEngineeringList(string sql)
        {

            return this.BaseRepository().FindTable(sql);
        }
        #endregion
    }
}
