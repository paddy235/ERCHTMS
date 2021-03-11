using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.WebControl;
using ERCHTMS.Code;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Entity.MatterManage;
using ERCHTMS.IService.CarManage;
using ERCHTMS.Service.SystemManage;
using Newtonsoft.Json;

namespace ERCHTMS.Service.CarManage
{
    /// <summary>
    /// �� �����ݷó�����
    /// </summary>
    public class VisitcarService : RepositoryFactory<VisitcarEntity>, VisitcarIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<VisitcarEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable(it => it.Phone == queryJson).OrderByDescending(it => it.CreateDate).ToList();
        }

        /// <summary>
        /// ��ȡ�Ÿڲ�ѯ�����ϼ��ݷ���Ϣ
        /// </summary>
        /// <returns></returns>
        public DataTable GetDoorList()
        {
            //            string sql = string.Format(@"select * from (
            //            select ID,Createdate,carno,CONCAT(CONCAT(CONCAT('�ݷ�-',visitdept),'-'),visituser) as purpose,dirver
            //            ,phone,note,accompanyingnumber anumber,driverlicenseurl,drivinglicenseurl,state,'0' type from bis_visitcar where state>0 and state<4
            //
            //            union 
            //
            //            select ID,Createdate,platenumber carno,CONCAT(CONCAT(CONCAT(CONCAT(dress,'-'),transporttype),'-'),PRODUCTTYPE) as purpose,
            //            DriverName dirver,DriverTel phone,Supplyname note,0 anumber,JsImgpath driverlicenseurl,XsImgpath drivinglicenseurl,examinestatus state,'1' type 
            //            from WL_OPERTICKETMANAGER where isdelete=1 and examinestatus<4 and examinestatus>0
            //
            //            union 
            //            
            //            select ID,Createdate,carno,CONCAT(CONCAT(CONCAT('Σ��Ʒ-',thecompany),'-'),hazardousname) as purpose,dirver
            //            ,phone,note,accompanyingnumber anumber,driverlicenseurl,drivinglicenseurl,state,'2' type from bis_hazardouscar where state>0 and state<4
            //            ) a1 order by Createdate desc");

            string sql = string.Format(@"  select * from (
             select ID,Createdate,carno,CONCAT('�ݷ�-',visitdept) as purpose,dirver,accompanyingperson as CyName
            ,phone,VisitUser as note,accompanyingnumber anumber,VisitUserPhone as driverlicenseurl,drivinglicenseurl,state,appstatue,3 type from BIS_USERCAR where state>0 and state<4
             union
            
            select ID,Createdate,carno,CONCAT(CONCAT(CONCAT('�ݷ�-',visitdept),'-'),visituser) as purpose,dirver,MODIFYUSERID as CyName
            ,phone,note,accompanyingnumber anumber,driverlicenseurl,drivinglicenseurl,state,appstatue, 0 type from bis_visitcar where state>0 and state<4

            union 

            select ID,Createdate,platenumber carno,CONCAT(CONCAT(CONCAT(CONCAT(dress,'-'),transporttype),'-'),PRODUCTTYPE) as purpose,DriverName dirver,MODIFYUSERID as CyName
            ,DriverTel phone,Supplyname note,0 anumber,JsImgpath driverlicenseurl,XsImgpath drivinglicenseurl,examinestatus state,1 appstatue,1 type 
            from WL_OPERTICKETMANAGER where isdelete=1 and examinestatus<4 and examinestatus>0

            union 
            
            select ID,Createdate,carno,CONCAT(CONCAT(CONCAT('Σ��Ʒ-',thecompany),'-'),hazardousname) as purpose,dirver,MODIFYUSERID as CyName
            ,phone,note,accompanyingnumber anumber,driverlicenseurl,drivinglicenseurl,state,1 appstatue,2 type from bis_hazardouscar where state>0 and state<4
            ) a1 order by Createdate desc");

            return BaseRepository().FindTable(sql);
        }

        /// <summary>
        /// ��ѯ�Ƿ����ظ����ƺŰݷó���/Σ��Ʒ����
        /// </summary>
        /// <param name="CarNo">���ƺ�</param>
        /// <param name="type">3λ�ݷ� 5ΪΣ��Ʒ</param>
        /// <returns></returns>
        public bool GetVisitCf(string CarNo, int type)
        {
            string dbname = "bis_visitcar";
            if (type == 3)
            {
                dbname = "bis_visitcar";
            }
            else if (type == 5)
            {
                dbname = "BIS_HAZARDOUSCAR";
            }

            string sql = string.Format("select count(id) as cou from {1} where carno ='{0}' and state<4", CarNo, dbname);

            int num1 = Convert.ToInt32(BaseRepository().FindObject(sql));

            if (num1 > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// ��õ���������������
        /// </summary>
        /// <returns></returns>
        public List<string> GetOutCarNum()
        {
            int num1 = 0;
            int num2 = 0;
            string sql = string.Format(@"select (select count(id)  as carnum from bis_hazardouscar where state>2 and state!=99 and TO_CHAR(CREATEDATE,'yyyy-MM-dd') >= '{0}' ) as SumNum,(select count(id) as carnum from bis_hazardouscar where state=3 and TO_CHAR(CREATEDATE,'yyyy-MM-dd') >= '{0}' ) as Num from bis_hazardouscar where rownum<2", DateTime.Now.ToString("yyyy-MM-dd"));
            string wlsql = string.Format(@"select (select count(id) from wl_operticketmanager where examinestatus>2 and examinestatus!=99 and TO_CHAR(Getdata,'yyyy-MM-dd') >= '{0}' ) as SumNum,(select count(id) as carnum from wl_operticketmanager where examinestatus=2 and TO_CHAR(Getdata,'yyyy-MM-dd') >= '{0}' ) as Num from wl_operticketmanager  where rownum<2", DateTime.Now.ToString("yyyy-MM-dd"));
            string whpsql = string.Format(@"select (select count(id)  as carnum from bis_visitcar where state>2 and state!=99 and TO_CHAR(CREATEDATE,'yyyy-MM-dd') >= '{0}' ) as SumNum,(select count(id) as carnum from bis_visitcar where state=3 and TO_CHAR(CREATEDATE,'yyyy-MM-dd') >= '{0}' ) as Num from bis_visitcar where rownum<2", DateTime.Now.ToString("yyyy-MM-dd"));
            DataTable dt = this.BaseRepository().FindTable(sql);
            DataTable dt1 = this.BaseRepository().FindTable(wlsql);
            DataTable dt2 = this.BaseRepository().FindTable(whpsql);
            if (dt.Rows.Count > 0)
            {
                num1 += Convert.ToInt32(dt.Rows[0][0].ToString());
                num2 += Convert.ToInt32(dt.Rows[0][1].ToString());
            }
            if (dt1.Rows.Count > 0)
            {
                num1 += Convert.ToInt32(dt1.Rows[0][0].ToString());
                num2 += Convert.ToInt32(dt1.Rows[0][1].ToString());
            }
            if (dt2.Rows.Count > 0)
            {
                num1 += Convert.ToInt32(dt2.Rows[0][0].ToString());
                num2 += Convert.ToInt32(dt2.Rows[0][1].ToString());
            }
            List<string> list = new List<string>();
            list.Add(num1.ToString());
            list.Add(num2.ToString());
            return list;
        }

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public VisitcarEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// ���ݳ��ƺŻ�ȡ������Ϣ
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public VisitcarEntity GetCar(string CarNo)
        {
            return this.BaseRepository().IQueryable(it => it.CarNo == CarNo).OrderByDescending(it => it.CreateDate).FirstOrDefault();
        }

        /// <summary>
        /// ���ݳ��ƺŻ�ȡ�˳��ƽ������°ݷ���Ϣ
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public VisitcarEntity NewGetCar(string CarNo)
        {
            string sql = string.Format("select * from bis_visitcar where carno='{0}' and state>1 order by createdate desc", CarNo);
            return this.BaseRepository().FindList(sql).FirstOrDefault();
        }




        /// <summary>
        /// ��ҳ��ѯ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            var curuser = OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();

            pagination.conditionJson += " and state > 0";

            //��������
            if (!queryParam["type"].IsEmpty())
            {
                string Type = queryParam["type"].ToString();

                pagination.conditionJson += string.Format(" and type = {0}", Type);

            }

            //��������
            if (!queryParam["sttime"].IsEmpty())
            {
                string sttime = queryParam["sttime"].ToString() + " 00:00:00";

                pagination.conditionJson += string.Format(" and TO_CHAR(CREATEDATE,'yyyy-MM-dd HH:mm:ss') >= '{0}'  ", sttime);

            }

            if (!queryParam["endtime"].IsEmpty())
            {
                string endtime = queryParam["endtime"].ToString() + " 23:59:59";
                pagination.conditionJson += string.Format(" and TO_CHAR(CREATEDATE,'yyyy-MM-dd HH:mm:ss') <= '{0}' ", endtime);
            }

            //״̬
            if (!queryParam["Status"].IsEmpty())
            {
                string Status = queryParam["Status"].ToString();

                if (Status == "1")
                {
                    pagination.conditionJson += string.Format(" and (state = 1 or state=2)");
                }
                else
                {

                    pagination.conditionJson += string.Format(" and state = {0}", Status);
                }
            }

            if (!queryParam["condition"].IsEmpty())
            {
                //���ƺ�
                if (queryParam["condition"].ToString() == "CarNo")
                {
                    string CarNo = queryParam["keyword"].ToString();

                    pagination.conditionJson += string.Format(" and CarNo like '%{0}%'", CarNo);

                }
                //��ʻ��
                if (queryParam["condition"].ToString() == "Dirver")
                {
                    string Dirver = queryParam["keyword"].ToString();
                    pagination.conditionJson += string.Format(" and Dirver like '%{0}%'", Dirver);
                }
                //��ʻ��
                if (queryParam["condition"].ToString() == "Phone")
                {
                    string Phone = queryParam["keyword"].ToString();
                    pagination.conditionJson += string.Format(" and Phone like '%{0}%'", Phone);
                }
            }




            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// ��ʼ���ݷ�\Σ��Ʒ\���ϳ���
        /// </summary>
        /// <returns></returns>
        public List<CarAlgorithmEntity> IniVHOCar()
        {
            //���ó�1Сʱ�ڵĴ򿨼�¼����Ϊ�ѽ���
            Repository<CarAlgorithmEntity> inlogdb = new Repository<CarAlgorithmEntity>(DbFactory.Base());
            string sql = string.Format(
                "select * from (select ID,carno,gpsid,gpsname,linename,'3' as type,'0' as State,'' as GoodsName from bis_visitcar where state='3'" +
                "union all" +
                " select ID,carno,gpsid,gpsname,HazardousName as linename,'5' as type,'0' as State,'' as GoodsName from bis_hazardouscar where state='3')");
            List<CarAlgorithmEntity> CarList = inlogdb.FindList(sql).ToList();
            Repository<OperticketmanagerEntity> wldb = new Repository<OperticketmanagerEntity>(DbFactory.Base());
            List<OperticketmanagerEntity> wllist = wldb.IQueryable(it => it.ExamineStatus == 3).ToList();
            if (wllist.Count > 0)
            {
                foreach (var op in wllist)
                {
                    CarAlgorithmEntity Car = new CarAlgorithmEntity();
                    Car.CarNo = op.Platenumber;
                    Car.GPSID = op.GpsId;
                    Car.GPSName = op.GpsName;
                    Car.ID = op.ID;
                    Car.State = 0;
                    Car.Type = 4;
                    string Dress = op.Dress;
                    Car.GoodsName = Dress;
                    int ISwharf = op.ISwharf;
                    string Transporttype = op.Transporttype;
                    if (Transporttype == "���")
                    {
                        Car.LineName = op.Dress + Transporttype;
                        if (ISwharf == 1)
                        {
                            Car.LineName += "(��ͷ)";
                        }
                    }
                    else
                    {
                        if (ISwharf == 1)
                        {
                            Car.LineName = "����ת��(��ͷ)";
                        }
                        else
                        {
                            Car.LineName = "ת��(������)";
                        }
                    }
                    CarList.Add(Car);
                }
            }

            return CarList;
        }

        #endregion

        #region �ύ����
        /// <summary>
        /// ����ID�ı���ѡ·��
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="LineName"></param>
        /// <param name="LineID"></param>
        public void ChangeLine(string keyValue, string LineName, string LineID)
        {
            string sql = string.Format("update bis_visitcar set linename='{0}',lineid='{1}' where id='{2}'", LineName, LineID, keyValue);
            BaseRepository().ExecuteBySql(sql);
        }

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
        public void SaveForm(string keyValue, VisitcarEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                Repository<CarUserFileImgEntity> caruserdb = new Repository<CarUserFileImgEntity>(DbFactory.Base());
                List<CarUserFileImgEntity> list = caruserdb.IQueryable(it => it.Baseid == keyValue).ToList();
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
                //if (entity.State == 3)
                //{//����ͨ�� (�·��볡Ȩ��)
                //    DataItemDetailService data = new DataItemDetailService();
                //    var pitem = data.GetItemValue("Hikappkey");
                //    var baseurl = data.GetItemValue("HikBaseUrl");
                //    string Key = string.Empty;
                //    string Signature = string.Empty;
                //    if (!string.IsNullOrEmpty(pitem))
                //    {
                //        Key = pitem.Split('|')[0];
                //        Signature = pitem.Split('|')[1];
                //    }
                //    UploadUserlimits(list, baseurl, Key, Signature);
                //}
                if (entity.State == 99)
                {//�ܾ��볡(ɾ������ƽ̨�����û�)
                    DeleteUserHiK(list);
                }
            }
            else
            {
                SaveUserFileImgForm(entity);
            }
        }

        /// <summary>
        /// ���������Ա������ͼƬ(�������ֻ�)
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <param name="userjson"></param>
        public void SaveFaceUserForm(string keyValue, VisitcarEntity entity, List<CarUserFileImgEntity> userjson)
        {
            //��ʼ����
            var res = DbFactory.Base().BeginTrans();
            try
            {
                List<CarUserFileImgEntity> list = new List<CarUserFileImgEntity>();
                for (int i = 0; i < userjson.Count; i++)
                {
                    CarUserFileImgEntity uentity = new CarUserFileImgEntity();
                    if (!string.IsNullOrEmpty(entity.Dirver))
                    {//������Ա
                        uentity.Create();
                        uentity.Username = userjson[i].Username;
                        uentity.Userimg = userjson[i].Userimg;
                        uentity.Imgdata = userjson[i].Imgdata;
                        uentity.Baseid = entity.ID;
                        uentity.ID = Guid.NewGuid().ToString();
                        uentity.CreateDate = DateTime.Now;
                        uentity.OrderNum = i;
                        entity.AccompanyingPerson = entity.AccompanyingPerson + userjson[i].Username + ",";
                        list.Add(uentity);
                    }
                }
                entity.AccompanyingNumber = userjson.Count;
                entity.AccompanyingPerson = entity.AccompanyingPerson.TrimEnd(',');
                res.Insert<CarUserFileImgEntity>(list);
                res.Insert(entity);
                res.Commit();
                UploadUserHiK(entity, list);//��Աͬ��������ƽ̨
            }
            catch (Exception)
            {
                res.Rollback();
            }
        }


        /// <summary>
        /// ����������Ա��Ϣ
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="roleEntity">��ɫʵ��</param>
        /// <returns></returns>
        public void SaveUserFileImgForm(VisitcarEntity entity)
        {
            //��ʼ����
            var res = DbFactory.Base().BeginTrans();
            try
            {
                List<CarUserFileImgEntity> list = new List<CarUserFileImgEntity>();
                for (int i = 0; i < entity.AccompanyingPerson.Split(',').Length; i++)
                {
                    CarUserFileImgEntity uentity = new CarUserFileImgEntity();
                    if (!string.IsNullOrEmpty(entity.AccompanyingPerson))
                    {//������Ա
                        uentity.Create();
                        uentity.Username = entity.AccompanyingPerson.Split(',')[i];
                        uentity.Baseid = entity.ID;
                        uentity.ID = Guid.NewGuid().ToString();
                        uentity.CreateDate = DateTime.Now;
                        uentity.OrderNum = i;
                        list.Add(uentity);
                    }
                }
                res.Insert<CarUserFileImgEntity>(list);
                res.Insert(entity);
                res.Commit();
                UploadUserHiK(entity, list);//��Աͬ��������ƽ̨
            }
            catch (Exception)
            {
                res.Rollback();
            }
        }


        #region ����ͬ����Ȩ���·�������ƽ̨

        /// <summary>
        /// ���ݷ���Ա��Ϣ�ϴ�������ƽ̨
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="list"></param>
        public void UploadUserHiK(VisitcarEntity entity, List<CarUserFileImgEntity> list)
        {
            DataItemDetailService data = new DataItemDetailService();
            var pitem = data.GetItemValue("Hikappkey");
            var baseurl = data.GetItemValue("HikBaseUrl");
            string Key = string.Empty;
            string Signature = string.Empty;
            //string no = new Random().Next(10, 888888).ToString();
            if (!string.IsNullOrEmpty(pitem))
            {
                Key = pitem.Split('|')[0];
                Signature = pitem.Split('|')[1];
            }
            string Url = "/artemis/api/resource/v1/person/single/add";
            foreach (var item in list)
            {
                string time = DateTime.Now.ToString("yyyyMMddHHmmss");
                var no = Str.PinYin(item.Username).ToUpper() + time;//����Ψһ
                //������Ϣ��base64����Ϊjpg��ʽ��
                List<FaceEntity> faces = new List<FaceEntity>();
                FaceEntity face = new FaceEntity();
                face.faceData = item.Imgdata;
                faces.Add(face);
                var model = new
                {
                    personId = item.ID,
                    personName = item.Username,
                    gender = "1",
                    orgIndexCode = "root000000",
                    birthday = "1990-01-01",
                    phoneNo = entity.Phone,
                    email = "person1@person.com",
                    certificateType = "990",
                    certificateNo = no,
                    faces
                };

                string msg = SocketHelper.LoadCameraList(model, baseurl, Url, Key, Signature);
                parkList1 pl = JsonConvert.DeserializeObject<parkList1>(msg);
                if (pl != null && pl.code == "0")
                {
                    SetLoadUserCarNo(item, baseurl, Key, Signature);
                }
            }
        }

        /// <summary>
        /// ������ӵ���Ա���俨��
        /// </summary>
        private void SetLoadUserCarNo(CarUserFileImgEntity Item, string baseUrl, string Key, string Signature)
        {
            var url = "/artemis/api/cis/v1/card/bindings";
            List<cardList1> cardList = new List<cardList1>();
            cardList1 entity = new cardList1();
            string time = DateTime.Now.ToString("yyyyMMddHHmmss");
            var no = time + Str.PinYin(Item.Username).ToUpper();//����Ψһ
            entity.cardNo = no.Trim();
            entity.personId = Item.ID;
            entity.cardType = 1;
            cardList.Add(entity);
            var model = new
            {
                startDate = DateTime.Now.ToString("yyyy-MM-dd"),
                endDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"),
                cardList
            };
            string msg = SocketHelper.LoadCameraList(model, baseUrl, url, Key, Signature);
            parkList2 pl = JsonConvert.DeserializeObject<parkList2>(msg);
        }

        /// <summary>
        /// ����Ȩ������
        /// </summary>
        private void UploadUserlimits(List<CarUserFileImgEntity> list, string baseUrl, string Key, string Signature)
        {
            if (list.Count > 0)
            {
                var url = "/artemis/api/acps/v1/auth_config/add";
                List<personDatas1> personDatas = new List<personDatas1>();
                personDatas1 entity = new personDatas1();
                List<string> codes = new List<string>();
                foreach (var item in list)
                {
                    codes.Add(item.ID);//��ԱId
                }
                entity.indexCodes = codes;
                entity.personDataType = "person";
                personDatas.Add(entity);

                string Qres = string.Empty;//��ʱ��ԱĬ��һ�Ÿ�
                string sql = string.Format("select t.itemname,t.itemvalue,t.itemcode from base_dataitem d join BASE_DATAITEMDETAIL t on d.itemid=t.itemid  where d.itemcode='equipment1' order by t.sortcode asc");
                DataTable dt = this.BaseRepository().FindTable(sql);
                if (dt.Rows.Count > 0)
                {
                    List<resourceInfos1> resourceInfos = new List<resourceInfos1>();//�豸��Ϣ����
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        List<int> nos = new List<int>();
                        if (dt.Rows[i][2].ToString() == "1")
                        {//�Ž�ͨ��
                            nos.Add(1);
                            nos.Add(2);
                        }
                        else
                        {//�Ž�
                            nos.Add(1);
                        }
                        resourceInfos1 entity1 = new resourceInfos1();
                        entity1.resourceIndexCode = dt.Rows[i][1].ToString();//�豸Ψһ���
                        entity1.resourceType = "acsDevice";
                        entity1.channelNos = nos;
                        resourceInfos.Add(entity1);
                    }
                    string stime = Convert.ToDateTime(list[0].CreateDate).ToString("yyyy-MM-ddTHH:mm:ss+08:00", DateTimeFormatInfo.InvariantInfo);//ISO8601ʱ���ʽ
                    string etime = Convert.ToDateTime(list[0].CreateDate).AddDays(1).ToString("yyyy-MM-ddTHH:mm:ss+08:00", DateTimeFormatInfo.InvariantInfo);
                    var model = new
                    {
                        personDatas,
                        resourceInfos,
                        startTime = stime,
                        endTime = etime
                    };
                    string msg = SocketHelper.LoadCameraList(model, baseUrl, url, Key, Signature);
                    downloadUserlimits(resourceInfos, baseUrl, Key, Signature);
                }
            }
        }

        /// <summary>
        /// ���ݳ���Ȩ�����ÿ������(IC���š�����)
        /// </summary>
        private void downloadUserlimits(List<resourceInfos1> resourceInfos, string baseUrl, string Key, string Signature)
        {
            var url = "/artemis/api/acps/v1/authDownload/configuration/shortcut";
            var model = new
            {
                taskType = 5,
                resourceInfos
            };
            string msg = SocketHelper.LoadCameraList(model, baseUrl, url, Key, Signature);
        }


        /// <summary>
        /// �볡��ܾ��볡ɾ������ƽ̨��Ա��Ϣ
        /// </summary>
        /// <param name="list">�볧��Ա</param>
        /// <param name="type">0�ܾ��볡 1��Ա�볧</param>
        public void DeleteUserHiK(List<CarUserFileImgEntity> list, int type = 0)
        {
            try
            {
                if (list.Count > 0)
                {
                    DataItemDetailService data = new DataItemDetailService();
                    var pitem = data.GetItemValue("Hikappkey");//������������Կ
                    var baseurl = data.GetItemValue("HikBaseUrl");//������������ַ
                    string Key = string.Empty;
                    string Signature = string.Empty;
                    if (!string.IsNullOrEmpty(pitem))
                    {
                        Key = pitem.Split('|')[0];
                        Signature = pitem.Split('|')[1];
                    }
                    string Url = "/artemis/api/resource/v1/person/batch/delete";//�ӿڵ�ַ
                    List<string> dellist = new List<string>();
                    foreach (var item in list)
                    {
                        dellist.Add(item.ID);
                    }
                    if (type == 1) { DeleteUserlimits(list, baseurl, Key, Signature); };
                    var model = new
                    {
                        personIds = dellist
                    };
                    SocketHelper.LoadCameraList(model, baseurl, Url, Key, Signature);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ɾ����Ӧ�豸�г���Ȩ������
        /// </summary>
        public void DeleteUserlimits(List<CarUserFileImgEntity> list, string baseUrl, string Key, string Signature)
        {
            string msg = string.Empty;
            if (list.Count > 0)
            {
                var url = "/artemis/api/acps/v1/auth_config/delete";
                List<personDatas1> personDatas = new List<personDatas1>();
                personDatas1 entity = new personDatas1();
                List<string> codes = new List<string>();
                foreach (var Item in list)
                {
                    codes.Add(Item.ID);//��ԱId
                }
                entity.indexCodes = codes;
                entity.personDataType = "person";
                personDatas.Add(entity);
                string sql = string.Format("select t.itemname,t.itemvalue,t.itemcode from base_dataitem d join BASE_DATAITEMDETAIL t on d.itemid=t.itemid  where d.itemcode='equipment1' order by t.sortcode asc");
                //����Ȩ��
                DataTable dt = this.BaseRepository().FindTable(sql);
                if (dt.Rows.Count > 0)
                {
                    List<resourceInfos1> resourceInfos = new List<resourceInfos1>();//�豸��Ϣ����
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        List<int> nos = new List<int>();
                        if (dt.Rows[i][2].ToString() == "1")
                        {//�Ž�ͨ��
                            nos.Add(1);
                            nos.Add(2);
                        }
                        else
                        {//�Ž�
                            nos.Add(1);
                        }
                        resourceInfos1 entity1 = new resourceInfos1();
                        entity1.resourceIndexCode = dt.Rows[i][1].ToString();//�豸Ψһ���
                        entity1.resourceType = "acsDevice";
                        entity1.channelNos = nos;
                        resourceInfos.Add(entity1);
                    }
                    var model = new
                    {
                        personDatas,
                        resourceInfos
                    };
                    msg = SocketHelper.LoadCameraList(model, baseUrl, url, Key, Signature);
                }
            }
        }

        #endregion


        /// <summary>
        /// �ı�GPS����Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <param name="pgpslist"></param>
        public void ChangeGps(string keyValue, VisitcarEntity entity, List<PersongpsEntity> pgpslist)
        {
            //��ʼ����
            var res = DbFactory.Base().BeginTrans();
            try
            {
                //���ó�1Сʱ�ڵĴ򿨼�¼����Ϊ�ѽ���
                Repository<VisitcarEntity> inlogdb = new Repository<VisitcarEntity>(DbFactory.Base());
                VisitcarEntity old = inlogdb.FindEntity(keyValue);
                bool isupdate = false;//�Ƿ��޸�
                if (old.GPSID != entity.GPSID)
                {
                    isupdate = true;
                }
                old.GPSID = entity.GPSID;
                old.GPSNAME = entity.GPSNAME;
                old.Modify(keyValue);
                if (old.State == 1)
                {
                    old.State = 2;
                    if (pgpslist != null)
                    {
                        for (int i = 0; i < pgpslist.Count; i++)
                        {
                            pgpslist[i].InTime = DateTime.Now;
                            pgpslist[i].State = 0;
                            pgpslist[i].Type = 0;
                            pgpslist[i].VID = old.ID;
                            pgpslist[i].UserName = pgpslist[i].UserName.Substring(0, pgpslist[i].UserName.IndexOf(':'));
                            pgpslist[i].Create();

                        }
                        res.Insert<PersongpsEntity>(pgpslist);
                    }

                    CargpsEntity cgps = new CargpsEntity();
                    cgps.AID = old.ID;
                    cgps.CarNo = old.CarNo;
                    cgps.GpsId = old.GPSID;
                    cgps.GpsName = old.GPSNAME;
                    cgps.Status = 0;
                    cgps.Type = 1;
                    cgps.Create();

                    res.Insert<CargpsEntity>(cgps);
                }
                else
                {
                    string sql = "";
                    if (pgpslist != null)
                    {
                        for (int i = 0; i < pgpslist.Count; i++)
                        {
                            sql += "update bis_persongps set gpsid='" + pgpslist[i].GPSID + "' , gpsname='" +
                                   pgpslist[i].GPSNAME + "' where id='" + pgpslist[i].ID + "'";
                            res.ExecuteBySql(sql);
                        }
                    }

                    if (isupdate)
                    {
                        Repository<CargpsEntity> Carinlogdb = new Repository<CargpsEntity>(DbFactory.Base());
                        CargpsEntity Car = Carinlogdb.IQueryable(it => it.AID == old.ID && it.Status == 0).FirstOrDefault();
                        Car.GpsId = old.GPSID;
                        Car.GpsName = old.GPSNAME;
                        Car.Modify(Car.ID);
                        res.Update<CargpsEntity>(Car);
                    }
                }
                res.Update<VisitcarEntity>(old);
                res.Commit();
            }
            catch (Exception ex)
            {
                res.Rollback();
                throw ex;
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="Note"></param>
        /// <param name="type"></param>
        public void CarOut(string keyValue, string Note, int type, List<PersongpsEntity> pergps)
        {
            //��ʼ����
            var res = DbFactory.Base().BeginTrans();
            try
            {
                List<CarUserFileImgEntity> dellist = new List<CarUserFileImgEntity>();//�볡
                Repository<CarUserFileImgEntity> caruserdb = new Repository<CarUserFileImgEntity>(DbFactory.Base());
                if (type == 0)
                {//�ݷó���
                    Repository<VisitcarEntity> inlogdb = new Repository<VisitcarEntity>(DbFactory.Base());
                    VisitcarEntity old = inlogdb.FindEntity(keyValue);
                    //old.OutTime = DateTime.Now;
                    //old.State = 4;
                    old.Issubmit = 1;
                    old.Note = Note;
                    old.Modify(old.ID);
                    Repository<PersongpsEntity> pgpsinlogdb = new Repository<PersongpsEntity>(DbFactory.Base());
                    List<PersongpsEntity> pgps = pgpsinlogdb.IQueryable(it => it.VID == keyValue && it.State == 0).ToList();
                    foreach (PersongpsEntity item in pergps)
                    {
                        if (item.UserName == "����" && item.State == 1)
                        {//����������������Ա����δ����
                            old.Issubmit = 2;
                            //List<PersongpsEntity> list = pgpsinlogdb.IQueryable(it => it.VID == keyValue && it.State == 0).ToList();
                            //if (list.Count == 0)
                            //{//�ӱ�������Ա�������޸��������״̬
                            //    old.OutTime = DateTime.Now;
                            //    old.State = 4;
                            //}
                        }
                        else
                        {//��Ա
                            PersongpsEntity entity = pgpsinlogdb.FindEntity(item.ID);
                            if (entity != null)
                            {
                                entity.Issubmit = 1;
                                entity.State = item.State;
                                entity.Modify(item.ID);
                                if (item.State == 1)
                                {
                                    entity.OutTime = DateTime.Now;
                                    var carentity = caruserdb.IQueryable(it => it.Baseid == keyValue && it.Username == entity.UserName).ToList();
                                    dellist.AddRange(carentity);
                                }
                                pgps.Add(entity);
                            }
                        }
                        if (pergps.Where(t => t.State == 1).ToList().Count == pergps.Count)
                        {//�ӱ�������Ա�������޸��������״̬
                            old.OutTime = DateTime.Now;
                            old.State = 4;
                        }

                    }
                    res.Update<VisitcarEntity>(old);
                    res.Update<PersongpsEntity>(pgps);
                }
                else if (type == 1)
                {
                    //���ó�1Сʱ�ڵĴ򿨼�¼����Ϊ�ѽ���
                    Repository<OperticketmanagerEntity> inlogdb = new Repository<OperticketmanagerEntity>(DbFactory.Base());
                    OperticketmanagerEntity old = inlogdb.FindEntity(keyValue);
                    old.ExamineStatus = 4;
                    old.OutDate = DateTime.Now;
                    old.PassRemark = Note;
                    old.Modify(old.ID);
                    res.Update<OperticketmanagerEntity>(old);
                }
                else
                {//Σ��Ʒ����
                    //���ó�1Сʱ�ڵĴ򿨼�¼����Ϊ�ѽ���
                    Repository<HazardouscarEntity> inlogdb = new Repository<HazardouscarEntity>(DbFactory.Base());
                    HazardouscarEntity old = inlogdb.FindEntity(keyValue);
                    //old.OutTime = DateTime.Now;
                    //old.State = 4;
                    old.Note = Note;
                    old.Issubmit = 1;
                    old.Modify(old.ID);
                    Repository<PersongpsEntity> pgpsinlogdb = new Repository<PersongpsEntity>(DbFactory.Base());
                    List<PersongpsEntity> pgps = new List<PersongpsEntity>();
                    foreach (PersongpsEntity item in pergps)
                    {
                        if (item.UserName == "����" && item.State == 1)
                        {//����������������Ա����δ����
                            old.Issubmit = 2;
                        }
                        else
                        {//��Ա
                            PersongpsEntity entity = pgpsinlogdb.FindEntity(item.ID);
                            if (entity != null)
                            {
                                entity.Issubmit = 1;
                                entity.State = item.State;
                                entity.Modify(item.ID);
                                if (item.State == 1)
                                {
                                    entity.OutTime = DateTime.Now;
                                    var carentity = caruserdb.IQueryable(it => it.Baseid == keyValue && it.Username == entity.UserName).ToList();
                                    dellist.AddRange(carentity);
                                }
                                pgps.Add(entity);
                            }
                        }
                        if (pergps.Where(t => t.State == 1).ToList().Count == pergps.Count)
                        {//�ӱ�������Ա�������޸��������״̬
                            old.OutTime = DateTime.Now;
                            old.State = 4;
                        }

                    }
                    res.Update<HazardouscarEntity>(old);
                    res.Update<PersongpsEntity>(pgps);
                }
                Repository<CargpsEntity> cgpsinlogdb = new Repository<CargpsEntity>(DbFactory.Base());
                List<CargpsEntity> cgps = cgpsinlogdb.IQueryable(it => it.AID == keyValue && it.Status == 0).ToList();
                if (cgps.Count > 0)
                {
                    for (int i = 0; i < cgps.Count; i++)
                    {
                        cgps[i].Status = 1;
                        cgps[i].EndTime = DateTime.Now;
                        cgps[i].Modify(cgps[i].ID);
                    }
                    res.Update<CargpsEntity>(cgps);
                }

                //����Υ�¼�¼����
                Repository<CarviolationEntity> violinlogdb = new Repository<CarviolationEntity>(DbFactory.Base());
                List<CarviolationEntity> violation = violinlogdb.IQueryable(it => it.CID == keyValue && it.IsProcess == 0).ToList();
                if (violation.Count > 0)
                {
                    for (int i = 0; i < violation.Count; i++)
                    {
                        violation[i].IsProcess = 1;
                        violation[i].ProcessMeasure = Note;
                        violation[i].Modify(violation[i].ID);
                    }
                    res.Update<CarviolationEntity>(violation);
                }

                res.Commit();
                DeleteUserHiK(dellist, 1);
            }
            catch (Exception ex)
            {
                res.Rollback();
                throw ex;
            }

        }


        /// <summary>
        /// �ı�GPS����Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <param name="pgpslist"></param>
        public void WlChangeGps(string keyValue, OperticketmanagerEntity entity)
        {
            //��ʼ����
            var res = DbFactory.Base().BeginTrans();
            try
            {
                //���ó�1Сʱ�ڵĴ򿨼�¼����Ϊ�ѽ���
                Repository<OperticketmanagerEntity> inlogdb = new Repository<OperticketmanagerEntity>(DbFactory.Base());
                OperticketmanagerEntity old = inlogdb.FindEntity(keyValue);
                bool isupdate = false;//�Ƿ��޸�
                if (old.GpsId != entity.GpsId)
                {
                    isupdate = true;
                }
                old.GpsId = entity.GpsId;
                old.GpsName = entity.GpsName;
                old.Modify(keyValue);
                if (old.ExamineStatus == 1)
                {
                    old.ExamineStatus = 2;


                    CargpsEntity cgps = new CargpsEntity();
                    cgps.AID = old.ID;
                    cgps.CarNo = old.Platenumber;
                    cgps.GpsId = old.GpsId;
                    cgps.GpsName = old.GpsName;
                    cgps.Status = 0;
                    cgps.Type = 1;
                    cgps.Create();
                    res.Insert<CargpsEntity>(cgps);
                }
                else
                {


                    if (isupdate)
                    {
                        Repository<CargpsEntity> Carinlogdb = new Repository<CargpsEntity>(DbFactory.Base());
                        CargpsEntity Car = Carinlogdb.IQueryable(it => it.AID == old.ID && it.Status == 0).FirstOrDefault();
                        Car.GpsId = old.GpsId;
                        Car.GpsName = old.GpsName;
                        Car.Modify(Car.ID);
                        res.Update<CargpsEntity>(Car);
                    }



                }
                res.Update<OperticketmanagerEntity>(old);
                res.Commit();
            }
            catch (Exception ex)
            {
                res.Rollback();
                throw ex;
            }
        }

        #endregion
    }
}
