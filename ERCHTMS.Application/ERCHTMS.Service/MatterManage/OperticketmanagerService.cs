using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.WebControl;
using ERCHTMS.Code;
using ERCHTMS.Entity.MatterManage;
using ERCHTMS.IService.MatterManage;

namespace ERCHTMS.Service.MatterManage
{
    /// <summary>
    /// �� ������Ʊ�����볧��Ʊ
    /// </summary>
    public class OperticketmanagerService : RepositoryFactory<OperticketmanagerEntity>, OperticketmanagerIService
    {
        private object NumberLock = new object();
        #region ��ȡ����

        /// <summary>
        /// ���ɿ�Ʊ����
        /// </summary>
        /// <param name="product">����Ʒ����</param>
        /// <param name="takeGoodsName">�����</param>
        /// <param name="transportType">��������(�����ת��)</param>
        /// <returns></returns>
        public string GetTicketNumber(string product, string takeGoodsName, string transportType)
        {
            string sql = string.Format("SELECT COUNT(NUMBERS) FROM WL_OPERTICKETMANAGER WHERE  TO_CHAR(CREATEDATE,'yyyy-MM-dd')='{0}'", DateTime.Now.ToString("yyyy-MM-dd"));
            StringBuilder sbFilter = new StringBuilder();
            string preFix = "";
            string middleFix = DateTime.Now.ToString("yyMMdd");
            switch (product)
            {
                case "��ú��":
                    sbFilter.Append(" AND PRODUCTTYPE='��ú��'");
                    if (takeGoodsName == "������")
                    {
                        preFix = "H";
                        sbFilter.Append(" AND TAKEGOODSNAME='������'");
                    }
                    else if (takeGoodsName == "�����")
                    {
                        preFix = "HE";
                        sbFilter.Append(" AND TAKEGOODSNAME='�����'");
                    }
                    break;
                case "ʯ��":
                    sbFilter.Append(" AND PRODUCTTYPE='ʯ��'");
                    if (transportType == "���")
                    {
                        preFix = "S";
                        sbFilter.Append(" AND TRANSPORTTYPE='���'");
                    }
                    break;
                case "ʯ��ת��":
                    preFix = "SG";
                    sbFilter.Append(" AND TRANSPORTTYPE='ת��'");
                    break;
                case "¯����":
                    preFix = "Z";
                    sbFilter.Append(" AND PRODUCTTYPE='¯����'");
                    break;
            }
            Repository<object> repository = new Repository<object>(DbFactory.Base());
           
            string numbers = string.Format("{0}{1}", preFix, middleFix);
            lock (NumberLock)
            {
                sql = sql + sbFilter.ToString();
                DataTable count = repository.FindTable(sql);
                numbers = numbers + (Convert.ToInt32(count.Rows[0][0]) + 1).ToString().PadLeft(3, '0');
            }
            return numbers;
        }

        /// <summary>
        /// ��ȡDataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string sql)
        {
            return this.BaseRepository().FindTable(sql);
        }


        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<OperticketmanagerEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public OperticketmanagerEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// ��ȡ�鿴���̹���ʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public OperticketmanagerEntity GetProcessEntity(string keyValue)
        {
            OperticketmanagerEntity entity = this.BaseRepository().FindEntity(keyValue);
            try
            {
                if (entity != null)
                {
                    entity.RCdbTime = this.GetTimeDouble(entity.Getdata, entity.BalanceTime).ToString();
                    entity.DbOutTime = this.GetTimeDouble(entity.BalanceTime, entity.OutDate).ToString();
                    if (entity.StayTime != null && entity.StayTime > 59) { entity.StayTimeStatus = "�쳣"; }
                    else entity.StayTimeStatus = "����";
                    entity.NetweightStatus = "����";
                }
            }
            catch (Exception)
            {
                entity = this.BaseRepository().FindEntity(keyValue);
            }
            return entity;
        }

        /// <summary>
        /// ����ʱ��֮�����ӣ�
        /// </summary>
        /// <param name="stime"></param>
        /// <param name="etime"></param>
        /// <returns></returns>
        public double GetTimeDouble(DateTime? stime, DateTime? etime)
        {
            double tnumber = 0;
            try
            {
                if (stime != null && etime != null)
                {
                    System.TimeSpan t1 = DateTime.Parse(etime.ToString()) - DateTime.Parse(stime.ToString());
                    tnumber = Math.Truncate(t1.TotalMinutes);
                }
            }
            catch (Exception)
            {
                tnumber = 0;
            }
            return tnumber;
        }

        /// <summary>
        /// �б��ҳ
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();

            if (!queryParam["keyword"].IsEmpty())
            {//���ƺ�
                string PlateNumber = queryParam["keyword"].ToString().Trim();
                pagination.conditionJson += string.Format(" and PlateNumber like '%{0}%'", PlateNumber);
            }

            if (!queryParam["Takegoodsname"].IsEmpty() && queryParam["Takegoodsname"].ToString().Trim() != "ȫ��")
            {//�����
                string Transporttype = queryParam["Takegoodsname"].ToString().Trim();
                pagination.conditionJson += string.Format(" and Takegoodsname='{0}'", Transporttype);
            }
            if (!queryParam["Transporttype"].IsEmpty() && queryParam["Transporttype"].ToString().Trim() != "ȫ��")
            {//��������
                string Transporttype = queryParam["Transporttype"].ToString().Trim();
                pagination.conditionJson += string.Format(" and Transporttype like '%{0}%'", Transporttype);
            }
            if (!queryParam["Producttype"].IsEmpty() && queryParam["Producttype"].ToString().Trim() != "ȫ��")
            {//��Ʒ����
                string Producttype = queryParam["Producttype"].ToString().Trim();
                pagination.conditionJson += string.Format(" and Producttype like '%{0}%'", Producttype);
            }
            if (!queryParam["Stime"].IsEmpty())
            {//����ʱ����
                string stime = queryParam["Stime"].ToString().Trim();
                pagination.conditionJson += string.Format(" and GetData >  to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') ", Convert.ToDateTime(stime));
            }
            if (!queryParam["Etime"].IsEmpty())
            {//����ʱ����
                string etime = queryParam["Etime"].ToString().Trim();
                DateTime dst = Convert.ToDateTime(etime).AddMinutes(1);
                pagination.conditionJson += string.Format(" and GetData < to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') ", dst);
            }
            //����ʱ����
            if (!queryParam["OutStartTime"].IsEmpty())
            {
                string outStartTime = queryParam["OutStartTime"].ToString().Trim();
                pagination.conditionJson += string.Format(" and OutDate >=  to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') ", outStartTime);
            }
            //����ʱ����
            if (!queryParam["OutEndtime"].IsEmpty())
            {
                string outEndTime = queryParam["OutEndtime"].ToString().Trim();
                pagination.conditionJson += string.Format(" and OutDate <= to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') ", outEndTime);
            }

            if (!queryParam["QueryDress"].IsEmpty() && queryParam["QueryDress"].ToString().Trim() != "ȫ��")
            {//װ�ҵ�
                string QueryDress = queryParam["QueryDress"].ToString().Trim();
                pagination.conditionJson += string.Format(" and Dress like '%{0}%'", QueryDress);
            }

            if (queryParam["QueryDress"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and Dress  is null");
            }

            DataTable dt = this.BaseRepository().FindTableByProcPager(pagination, DbHelper.DbType);



            return dt;
        }


        /// <summary>
        /// �б��ҳ
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable BackGetPageList(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();


            pagination.conditionJson += " and ExamineStatus > 0";
            if (!queryParam["keyword"].IsEmpty())
            {
                //���ƺ�
                if (queryParam["condition"].ToString() == "CarNo")
                {
                    string PlateNumber = queryParam["keyword"].ToString().Trim();
                    pagination.conditionJson += string.Format(" and PlateNumber like '%{0}%'", PlateNumber);

                }
                //��ʻ��
                if (queryParam["condition"].ToString() == "Dirver")
                {
                    string Dirver = queryParam["keyword"].ToString();
                    pagination.conditionJson += string.Format(" and drivername like '%{0}%'", Dirver);
                }
                //�绰����
                if (queryParam["condition"].ToString() == "Phone")
                {

                    string Phone = queryParam["keyword"].ToString();
                    pagination.conditionJson += string.Format(" and drivertel  like '{0}%'", Phone);
                }
            }
            //��������
            if (!queryParam["Transporttype"].IsEmpty() && queryParam["Transporttype"].ToString().Trim() != "ȫ��")
            {
                string Transporttype = queryParam["Transporttype"].ToString().Trim();
                pagination.conditionJson += string.Format(" and Transporttype like '%{0}%'", Transporttype);
            }
            //״̬
            if (!queryParam["Status"].IsEmpty())
            {
                string Status = queryParam["Status"].ToString().Trim();
                if (Status == "1")
                {
                    pagination.conditionJson += string.Format(" and (ExamineStatus = 1 or ExamineStatus=2)");
                }
                else
                {

                    pagination.conditionJson += string.Format(" and ExamineStatus = {0}", Status);
                }
            }

            //״̬
            if (!queryParam["Vnum"].IsEmpty())
            {
                string Vnum = queryParam["Vnum"].ToString();
                if (Vnum == "0")
                {
                    pagination.conditionJson += string.Format(" and (vnum = 0 or vnum is null)");
                }
                else
                {
                    pagination.conditionJson += string.Format(" and vnum > 0");
                }
            }


            DataTable dt = this.BaseRepository().FindTableByProcPager(pagination, DbHelper.DbType);
            return dt;
        }

        /// <summary>
        /// ��ȡ�����볡��Ʊ��¼��Ϣ
        /// </summary>
        /// <param name="VehicleNumber">���ƺ�</param>
        /// <returns></returns>
        public OperticketmanagerEntity GetNewEntity(string VehicleNumber)
        {
            OperticketmanagerEntity entity = new OperticketmanagerEntity();
            if (!string.IsNullOrEmpty(VehicleNumber))
            {
                entity = this.BaseRepository().IQueryable().ToList().Where(a => a.Platenumber == VehicleNumber && a.ExamineStatus != 4 && a.Isdelete == 1).OrderByDescending(a => a.CreateDate).FirstOrDefault();
            }
            else
            {
                entity = this.BaseRepository().IQueryable().ToList().Where(a => a.Isdelete == 1).OrderByDescending(a => a.CreateDate).FirstOrDefault();
            }
            return entity;
        }

        /// <summary>
        /// ���ݳ��ƺŻ�ȡ������Ϣ
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public OperticketmanagerEntity GetCar(string CarNo)
        {
            return this.BaseRepository().IQueryable(it => it.Platenumber == CarNo).OrderByDescending(it => it.CreateDate).FirstOrDefault();
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
        public void SaveForm(string keyValue, OperticketmanagerEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                entity.Create();
                entity.Numbers = GetTicketNumber(entity.Producttype, entity.Takegoodsname, entity.Transporttype);

                if (GetNewEntity(entity.Platenumber) == null)
                    entity.IsFirst = "��";
                else entity.IsFirst = "��";
                var lastTicket = GetCar(entity.Platenumber);
                if (lastTicket != null)
                {
                    entity.DriverName = lastTicket.DriverName;
                    entity.DriverTel = lastTicket.DriverTel;
                }
                this.BaseRepository().Insert(entity);
            }
        }

        /// <summary>
        /// д��־����
        /// </summary>
        public void InsetDailyRecord(DailyrRecordEntity entity)
        {
            try
            {
                if (entity != null)
                {
                    Operator user = OperatorProvider.Provider.Current();
                    if (user != null)
                        entity.Create();
                    Repository<DailyrRecordEntity> equ = new Repository<DailyrRecordEntity>(DbFactory.Base());
                    equ.Insert(entity);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
