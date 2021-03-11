using System;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.IService.CarManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Code;
using ERCHTMS.Service.SystemManage;
using Newtonsoft.Json;
using System.Globalization;

namespace ERCHTMS.Service.CarManage
{
    /// <summary>
    /// �� ����Σ�����س�����
    /// </summary>
    public class HazardouscarService : RepositoryFactory<HazardouscarEntity>, HazardouscarIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<HazardouscarEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable(it => it.Phone == queryJson).ToList();
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
            if (!queryParam["StartTime"].IsEmpty())
            {
                string sttime = queryParam["StartTime"].ToString() + " 00:00:00";

                pagination.conditionJson += string.Format(" and TO_CHAR(CREATEDATE,'yyyy-MM-dd HH:mm:ss') >= '{0}'  ", sttime);

            }

            if (!queryParam["EndTime"].IsEmpty())
            {
                string endtime = queryParam["EndTime"].ToString() + " 23:59:59";
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

            //״̬
            if (!queryParam["Hazardous"].IsEmpty())
            {
                string Hazardous = queryParam["Hazardous"].ToString();

                pagination.conditionJson += string.Format(" and Hazardousid = '{0}'", Hazardous);

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





            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public HazardouscarEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// ��ȡ��Σ�������Ƿ������˼���
        /// </summary>
        /// <param name="HazardousId"></param>
        /// <returns></returns>
        public bool GetHazardous(string HazardousId)
        {
            Repository<CarcheckitemhazardousEntity> inlogdb = new Repository<CarcheckitemhazardousEntity>(DbFactory.Base());
            CarcheckitemhazardousEntity haza = inlogdb.FindEntity(it => it.HazardousId == HazardousId);
            if (haza != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// ���ݳ��ƺŻ�ȡ������Ϣ
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public HazardouscarEntity GetCar(string CarNo)
        {
            return this.BaseRepository().IQueryable(it => it.CarNo == CarNo).OrderByDescending(it => it.CreateDate).FirstOrDefault();
        }


        /// <summary>
        /// ��ȡ����Σ��Ʒ����
        /// </summary>
        /// <returns></returns>
        public List<HazardouscarEntity> GetHazardousList(string day)
        {
            string sql =
                string.Format(
                    @"select * from BIS_HAZARDOUSCAR where TO_CHAR(CREATEDATE,'yyyy-MM-dd')='{0}'",
                    day);
            return BaseRepository().FindList(sql).ToList();
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
        public void SaveForm(string keyValue, HazardouscarEntity entity)
        {
            //��ʼ����
            var res = DbFactory.Base().BeginTrans();
            try
            {
                string HazardousId = entity.HazardousId;
                Repository<CarcheckitemhazardousEntity> inlogdb = new Repository<CarcheckitemhazardousEntity>(DbFactory.Base());
                CarcheckitemhazardousEntity haza = inlogdb.FindEntity(it => it.HazardousId == HazardousId);

                Repository<CarcheckitemmodelEntity> modeldb = new Repository<CarcheckitemmodelEntity>(DbFactory.Base());
                IEnumerable<CarcheckitemmodelEntity> modelList = modeldb.IQueryable(it => it.CID == haza.CID).OrderBy(it => it.Sort);

                List<CarcheckitemdetailEntity> DetailList = new List<CarcheckitemdetailEntity>();
                foreach (CarcheckitemmodelEntity item in modelList)
                {
                    CarcheckitemdetailEntity detail = new CarcheckitemdetailEntity();
                    detail.CheckStatus = 0;
                    detail.CheckItem = item.CheckItem;
                    detail.Sort = item.Sort;
                    detail.CreateUserId = "System";
                    detail.CreateDate = DateTime.Now;
                    detail.CreateUserDeptCode = "00";
                    detail.CreateUserOrgCode = "00";
                    detail.ID = Guid.NewGuid().ToString();
                    detail.PID = entity.ID;
                    DetailList.Add(detail);
                }
                res.Insert<CarcheckitemdetailEntity>(DetailList);
                //res.Insert<HazardouscarEntity>(entity);
                res.Commit();
            }
            catch (Exception e)
            {
                res.Rollback();
            }
        }


        /// <summary>
        /// ���������Ա������ͼƬ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <param name="userjson"></param>
        public void SaveFaceUserForm(string keyValue, HazardouscarEntity entity, List<CarUserFileImgEntity> userjson)
        {
            //��ʼ����
            var res = DbFactory.Base().BeginTrans();
            try
            {
                string HazardousId = entity.HazardousId;
                Repository<CarcheckitemhazardousEntity> inlogdb = new Repository<CarcheckitemhazardousEntity>(DbFactory.Base());
                CarcheckitemhazardousEntity haza = inlogdb.FindEntity(it => it.HazardousId == HazardousId);

                Repository<CarcheckitemmodelEntity> modeldb = new Repository<CarcheckitemmodelEntity>(DbFactory.Base());
                IEnumerable<CarcheckitemmodelEntity> modelList = modeldb.IQueryable(it => it.CID == haza.CID).OrderBy(it => it.Sort);

                List<CarcheckitemdetailEntity> DetailList = new List<CarcheckitemdetailEntity>();
                foreach (CarcheckitemmodelEntity item in modelList)
                {
                    CarcheckitemdetailEntity detail = new CarcheckitemdetailEntity();
                    detail.CheckStatus = 0;
                    detail.CheckItem = item.CheckItem;
                    detail.Sort = item.Sort;
                    detail.CreateUserId = "System";
                    detail.CreateDate = DateTime.Now;
                    detail.CreateUserDeptCode = "00";
                    detail.CreateUserOrgCode = "00";
                    detail.ID = Guid.NewGuid().ToString();
                    detail.PID = entity.ID;
                    DetailList.Add(detail);
                }
                res.Insert<CarcheckitemdetailEntity>(DetailList);
                //res.Insert<HazardouscarEntity>(entity);
                res.Commit();
                SaveUserFileImgForm(entity, userjson);
            }
            catch (Exception)
            {
                res.Rollback();
            }
        }

        /// <summary>
        /// ����������Ա��Ϣ
        /// </summary>
        /// <param name="entity">����¼</param>
        /// <param name="userjson">������Ա����</param>
        public void SaveUserFileImgForm(HazardouscarEntity entity, List<CarUserFileImgEntity> userjson)
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
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void Update(string keyValue, HazardouscarEntity entity)
        {
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    Repository<CarUserFileImgEntity> caruserdb = new Repository<CarUserFileImgEntity>(DbFactory.Base());
                    List<CarUserFileImgEntity> list = caruserdb.IQueryable(it => it.Baseid == keyValue).ToList();
                    entity.Modify(keyValue);
                    BaseRepository().Update(entity);
                    //if (entity.State == 3)
                    //{//����ͨ�� (�·��볧Ȩ��)
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
                    {//�ܾ��볡
                        DeleteUserHiK(list);
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// �ı�GPS����Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <param name="pgpslist"></param>
        public void ChangeGps(string keyValue, HazardouscarEntity entity, List<PersongpsEntity> pgpslist)
        {
            //��ʼ����
            var res = DbFactory.Base().BeginTrans();
            try
            {
                //���ó�1Сʱ�ڵĴ򿨼�¼����Ϊ�ѽ���
                Repository<HazardouscarEntity> inlogdb = new Repository<HazardouscarEntity>(DbFactory.Base());
                HazardouscarEntity old = inlogdb.FindEntity(keyValue);
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
                res.Update<HazardouscarEntity>(old);
                res.Commit();
            }
            catch (Exception ex)
            {
                res.Rollback();
                throw ex;
            }
        }

        /// <summary>
        /// �ı�Σ��Ʒ��������״̬λ�������
        /// </summary>
        /// <param name="id"></param>
        public void ChangeProcess(string id)
        {
            HazardouscarEntity hazard = BaseRepository().FindEntity(id);
            Operator user = OperatorProvider.Provider.Current();
            hazard.HandoverName = user.UserName;
            hazard.HandoverId = user.UserId;
            hazard.HandoverSign = user.SignImg;
            hazard.HazardousProcess = 2;
            hazard.Modify(id);
            BaseRepository().Update(hazard);
        }

        #region ����ͬ����Ȩ���·�������ƽ̨

        /// <summary>
        /// ���ݷ���Ա��Ϣ�ϴ�������ƽ̨
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="list"></param>
        public void UploadUserHiK(HazardouscarEntity entity, List<CarUserFileImgEntity> list)
        {
            DataItemDetailService data = new DataItemDetailService();
            var pitem = data.GetItemValue("Hikappkey");
            var baseurl = data.GetItemValue("HikBaseUrl");
            string Key = string.Empty;
            string Signature = string.Empty;
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

        #endregion
    }
}
