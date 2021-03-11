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
using ERCHTMS.Service.StandardSystem;
using Newtonsoft.Json;
using ERCHTMS.Service.SystemManage;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ERCHTMS.Service.BaseManage;

namespace ERCHTMS.Service.CarManage
{
    /// <summary>
    /// �� ��������������Ϣ��
    /// </summary>
    public class CarinfoService : RepositoryFactory<CarinfoEntity>, CarinfoIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<CarinfoEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }

        /// <summary>
        /// ��ȡ¼�복����
        /// </summary>
        /// <returns></returns>
        public List<CarinfoEntity> GetGspCar()
        {
            return BaseRepository().IQueryable(it =>  it.GpsName != null && it.GpsId != null && it.Type < 3).ToList();
        }

        /// <summary>
        /// ���ƺ��Ƿ����ظ�
        /// </summary>
        /// <param name="CarNo"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool GetCarNoIsRepeat(string CarNo, string id)
        {

            string sql = string.Format("select count(id) from bis_carinfo where carno='{0}' ", CarNo);
            if (!id.IsEmpty())
            {
                sql += string.Format(" and id!='{0}'", id);
            }

            int count = Convert.ToInt32(BaseRepository().FindObject(sql));
            return count > 0;
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public CarinfoEntity GetUserCar(string userid)
        {
            return this.BaseRepository().IQueryable(it => it.CreateUserId == userid && it.Type == 1).FirstOrDefault();
        }
        /// <summary>
        /// ���ݳ��ƺŻ�ȡ������Ϣ
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public CarinfoEntity GetBusCar(string CarNo)
        {
            return this.BaseRepository().IQueryable(it => it.CarNo == CarNo && it.Type == 0).FirstOrDefault();
        }

        /// <summary>
        /// ���ݳ��ƺŻ�ȡ������Ϣ
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public CarinfoEntity GetCar(string CarNo)
        {
            return this.BaseRepository().IQueryable(it => it.CarNo == CarNo).FirstOrDefault();
        }

        /// <summary>
        /// �û��б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            var curuser = OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();

            //��������
            if (!queryParam["Type"].IsEmpty())
            {
                string Type = queryParam["Type"].ToString();

                pagination.conditionJson += string.Format(" and info.Type = {0}", Type);

            }

            if (!queryParam["WzStatus"].IsEmpty())
            {
                int WzStatus = Convert.ToInt32(queryParam["WzStatus"]);
                if (WzStatus == 0)
                {
                    pagination.conditionJson += string.Format(" and vi.num >0");
                }
                else
                {
                    pagination.conditionJson += string.Format(" and vi.num is null");
                }
            }
            //ͨ���Ÿ�
            if (!queryParam["CurrentName"].IsEmpty())
            {
                string name = queryParam["CurrentName"].ToString();
                pagination.conditionJson += string.Format(" and info.currentgname like '%{0}%'", name);
            }


            if (!queryParam["condition"].IsEmpty())
            {
                //���ƺ�
                if (queryParam["condition"].ToString() == "CarNo" && !queryParam["keyword"].IsEmpty())
                {
                    string CarNo = queryParam["keyword"].ToString();

                    pagination.conditionJson += string.Format(" and info.CarNo like '%{0}%'", CarNo);

                }
                //��ʻ��
                if (queryParam["condition"].ToString() == "Dirver" && !queryParam["keyword"].IsEmpty())
                {
                    string Dirver = queryParam["keyword"].ToString();
                    pagination.conditionJson += string.Format(" and info.Dirver like '%{0}%'", Dirver);
                }
                //�绰����
                if (queryParam["condition"].ToString() == "Phone" && !queryParam["keyword"].IsEmpty())
                {

                    string Phone = queryParam["keyword"].ToString();
                    pagination.conditionJson += string.Format(" and info.Phone  like '{0}%'", Phone);
                }

                //��λ
                if (queryParam["condition"].ToString() == "Remark" && !queryParam["keyword"].IsEmpty())
                {
                    string Phone = queryParam["keyword"].ToString();
                    pagination.conditionJson += string.Format(" and info.Remark  like '{0}%'", Phone);
                }

            }

            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }



        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public CarinfoEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue, string IP, int Port)
        {
            try
            {
                var data = this.BaseRepository().FindEntity(keyValue);
                if (data != null)
                {//ͬ��ɾ������ƽ̨����
                    OperHaiKangRecord(data, 2, "");
                }
            this.BaseRepository().Delete(keyValue);
            //������ģ�鷢�ͳ���ɾ��ָ��
            CarAlgorithmEntity Car = new CarAlgorithmEntity();
            Car.ID = keyValue;
            Car.State = 1;
            SocketHelper.SendMsg(Car.ToJson(), IP, Port);
        }
            catch (Exception)
            {
                throw;
            }
        }
     

        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, CarinfoEntity entity, string pitem, string url,string IP,int Port)
        {
            //��ʼ����
            var res = DbFactory.Base().BeginTrans();
            try
            {
                //string key = string.Empty;// "21049470";
                //string sign = string.Empty;// "4gZkNoh3W92X6C66Rb6X";
                //if (!string.IsNullOrEmpty(pitem))
                //{
                //    key = pitem.Split('|')[0];
                //    sign = pitem.Split('|')[1];
                //}

                if (!string.IsNullOrEmpty(keyValue))
                {

                    //���ó�1Сʱ�ڵĴ򿨼�¼����Ϊ�ѽ���
                    Repository<CarinfoEntity> inlogdb = new Repository<CarinfoEntity>(DbFactory.Base());
                    CarinfoEntity old = inlogdb.FindEntity(keyValue);
                    //�ж��Ƿ��޸�GPS��Ϣ������޸�����Ӧ���޸�GPS��Ϣ����
                    if (entity.GpsId != null && old.GpsId != entity.GpsId && entity.GpsId.Trim() != "")
                    {
                        string sql = string.Format(@"update bis_cargps set gpsid='{0}',gpsname='{1}' where aid='{2}'", entity.GpsId, entity.GpsName, entity.ID);
                        res.ExecuteBySql(sql);
                    }
                    else if (entity.GpsId == null && old.GpsId != entity.GpsId)
                    {
                        string sql = string.Format(@"update bis_cargps set gpsid='{0}',gpsname='{1}' where aid='{2}'", "", "", entity.ID);
                        res.ExecuteBySql(sql);
                    }

                    entity.Modify(keyValue);
                    if (string.IsNullOrEmpty(entity.Dirver))
                    {
                        entity.Dirver = "";
                    }
                    if (string.IsNullOrEmpty(entity.Remark))
                    {
                        entity.Remark = "";
                    }
                    if (string.IsNullOrEmpty(entity.Phone))
                    {
                        entity.Phone = "";
                    }

                    res.Update<CarinfoEntity>(entity);

                    #region ���ݳ������ƺ���ͬ���޸ĺ����Ǳߵĳ�������
                    var data = this.BaseRepository().FindEntity(keyValue);
                    if (data != null)
                    {
                        OperHaiKangRecord(entity, 1, data.CarNo);
                    }
                    #endregion
                    res.Commit();
                    if (!string.IsNullOrEmpty(entity.GpsId) && !string.IsNullOrEmpty(entity.GpsName))
                    {
                        CarAlgorithmEntity Car = new CarAlgorithmEntity();
                        Car.CarNo = entity.CarNo;
                        Car.GPSID = entity.GpsId;
                        Car.GPSName = entity.GpsName;
                        Car.ID = entity.ID;
                        Car.Type = Convert.ToInt32(entity.Type);
                        Car.State = 0;
                        Car.LineName = "";
                        Car.GoodsName = "";
                        SocketHelper.SendMsg(Car.ToJson(), IP, Port);
                    }
                }
                else
                {
                    entity.Create();
                    #region ����Ӻ�������
                    //var model = new
                    //{
                    //    clientId = 1,
                    //    plateNo = entity.CarNo

                    //};
                    //List<object> modellist = new List<object>();
                    //modellist.Add(model);
                    //string addMsg = SocketHelper.LoadCameraList(model, url, "/artemis/api/resource/v1/vehicle/batch/add", key, sign);
                    //AddCarMsg addcar = JsonConvert.DeserializeObject<AddCarMsg>(addMsg);
                    //if (addcar != null && addcar.code == "0" && addcar.data.successes.Count > 0)
                    //{
                    //    entity.ID = addcar.data.successes[0].vehicleId;
                    //}
                    OperHaiKangRecord(entity, 0, "");
                    #endregion
                    res.Insert<CarinfoEntity>(entity);
                    CargpsEntity gps = new CargpsEntity();
                    gps.AID = entity.ID;
                    gps.CarNo = entity.CarNo;
                    gps.GpsId = entity.GpsId;
                    gps.GpsName = entity.GpsName;
                    gps.Status = 0;
                    gps.Type = 0;
                    gps.Create();
                    res.Insert<CargpsEntity>(gps);
                    res.Commit();

                    if (!string.IsNullOrEmpty(entity.GpsId) && !string.IsNullOrEmpty(entity.GpsName))
                    {
                        CarAlgorithmEntity Car = new CarAlgorithmEntity();
                        Car.CarNo = entity.CarNo;
                        Car.GPSID = entity.GpsId;
                        Car.GPSName = entity.GpsName;
                        Car.ID = entity.ID;
                        Car.Type = Convert.ToInt32(entity.Type);
                        Car.State = 0;
                        Car.LineName = "";
                        Car.GoodsName = "";
                        SocketHelper.SendMsg(Car.ToJson(), IP, Port);
                    }
                }
            }
            catch (Exception ex)
            {
                res.Rollback();
                throw ex;
            }
        }

        /// <summary>
        /// /�����
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void SaveForm(string keyValue, CarinfoEntity entity)
        {
            //��ʼ����
            var res = DbFactory.Base().BeginTrans();
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    entity.Modify(keyValue);
                    res.Update<CarinfoEntity>(entity);
                    res.Commit();
                }
                else
                {
                    entity.Create();
                    res.Insert<CarinfoEntity>(entity);
                    res.Commit();
                }
            }
            catch (Exception)
            {
                res.Rollback();
            }
        }

        /// <summary>
        /// ˽�ҳ������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void CartoExamine(string keyValue, CarinfoEntity entity)
        {
            //��ʼ����
            var res = DbFactory.Base().BeginTrans();
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    entity.Modify(keyValue);
                    res.Update<CarinfoEntity>(entity);
                    res.Commit();
                    if (entity.State == "1")
                    {
                        OperHaiKangRecord(entity, 0, "");
                    }
                }
            }
            catch (Exception)
            {
                res.Rollback();
            }
        }

        /// <summary>
        /// �ֻ�app�޸ĺ���������Ϣ
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="NewCar"></param>
        public void UpdateHiaKangCar(CarinfoEntity entity, string OldCar)
        {
            try
            {
                OperHaiKangRecord(entity, 2, OldCar);
            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion

        #region ����ƽ̨

      /// <summary>
        /// ����ƽ̨����ͬ����Ϣ
      /// </summary>
      /// <param name="entity">ʵ��</param>
      /// <param name="type">0���� 1�޸� 2ɾ��</param>
      /// <param name="NewCar">�ɳ���</param>
        public void OperHaiKangRecord(CarinfoEntity entity, int type, string OldCar)
        {
            try
            {
                #region ��ȡ������ַ����Կ
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
                #endregion
                #region ��ӳ���
                if (type == 0)
                {//���
                    string addurl = "/artemis/api/v1/vehicle/addVehicle";
                    List<object> mllist = new List<object>();
                    var user = new UserService().GetEntity(entity.DirverId);
                    if (user != null)
                    {
                        var model = new
                        {//�г���
                            personId = entity.DirverId,
                            plateNo = entity.CarNo,
                            parkIndexCode = GetTrafficPost(entity.Currentgid, entity.Type),//ͣ����Ψһ��ʶ
                            plateType = 0,
                            plateColor = 0,
                            carType = 0,
                            carColor = 0,
                            startTime = DateTime.Parse(entity.Starttime.ToString()).ToString("yyyy-MM-dd"),
                            endTime = DateTime.Parse(entity.Endtime.ToString()).ToString("yyyy-MM-dd"),
                            vehicleGroup = GetGroupInfo(entity.Type)//Ⱥ��Ψһ��ʶ
                        };
                        mllist.Add(model);
                    }
                    else
                    {
                        var model = new
                        {//�޳���
                            //personId = entity.DirverId,
                            plateNo = entity.CarNo,
                            parkIndexCode = GetTrafficPost(entity.Currentgid, entity.Type),//ͣ����Ψһ��ʶ
                            plateType = 0,
                            plateColor = 0,
                            carType = 0,
                            carColor = 0,
                            startTime = DateTime.Parse(entity.Starttime.ToString()).ToString("yyyy-MM-dd"),
                            endTime = DateTime.Parse(entity.Endtime.ToString()).ToString("yyyy-MM-dd"),
                            vehicleGroup = GetGroupInfo(entity.Type)//Ⱥ��Ψһ��ʶ
                        };
                        mllist.Add(model);
                    }
                    SocketHelper.LoadCameraList(mllist, baseurl, addurl, Key, Signature);
                }
                #endregion
                #region �޸ĳ���
                else if (type == 1)
                {//�޸�
                    string updateurl = "/artemis/api/v1/vehicle/updateVehicle";
                    List<object> mllist = new List<object>();
                    var user = new UserService().GetEntity(entity.DirverId);
                    if (user != null)
                    {
                        var model = new
                        {//�г���
                            plateNo = entity.CarNo,//�³���
                            oldPlateNo = OldCar,//�ɳ���
                            ownerId = entity.DirverId,
                            parkIndexCode = GetTrafficPost(entity.Currentgid, entity.Type),//ͣ����Ψһ��ʶ
                            plateType = 0,
                            plateColor = 0,
                            carType = 0,
                            carColor = 0,
                            startTime = DateTime.Parse(entity.Starttime.ToString()).ToString("yyyy-MM-dd"),
                            endTime = DateTime.Parse(entity.Endtime.ToString()).ToString("yyyy-MM-dd"),
                            isUpdateFunction = 1,
                            vehicleGroup = GetGroupInfo(entity.Type)//Ⱥ��Ψһ��ʶ
                        };
                        mllist.Add(model);
                    }
                    else
                    {
                        var model = new
                        {//�޳���
                            plateNo = entity.CarNo,//�³���
                            oldPlateNo = OldCar,//�ɳ���
                            parkIndexCode = GetTrafficPost(entity.Currentgid, entity.Type),//ͣ����Ψһ��ʶ
                            plateType = 0,
                            plateColor = 0,
                            carType = 0,
                            carColor = 0,
                            startTime = DateTime.Parse(entity.Starttime.ToString()).ToString("yyyy-MM-dd"),
                            endTime = DateTime.Parse(entity.Endtime.ToString()).ToString("yyyy-MM-dd"),
                            isUpdateFunction = 1,
                            vehicleGroup = GetGroupInfo(entity.Type)//Ⱥ��Ψһ��ʶ
                        };
                        mllist.Add(model);
                    }
                    SocketHelper.LoadCameraList(mllist, baseurl, updateurl, Key, Signature);
                }
                #endregion
                #region ɾ������
                else if (type == 2)
                {//ɾ��
                    string selurl = "/artemis/api/v1/vehicle/fetchVehicle";
                    var model = new
                    {
                        plateNo = entity.CarNo,
                        pageNo = 1,
                        pageSize = 10
                    };
                    //��ѯ������¼Ψһ��ʶ
                    string msg = SocketHelper.LoadCameraList(model, baseurl, selurl, Key, Signature);
                    CarSelectEntity pl = JsonConvert.DeserializeObject<CarSelectEntity>(msg);
                    if (pl != null && pl.code == "0")
                    {
                        string carid = pl.data.rows[0].vehicleId;
                        //ɾ������
                        string delurl = "/artemis/api/resource/v1/vehicle/batch/delete";
                        List<string> list = new List<string>();
                        list.Add(carid);
                        var delmodel = new
                        {
                            vehicleIds = list
                        };
                        SocketHelper.LoadCameraList(delmodel, baseurl, delurl, Key, Signature);
                    }
                }
                #endregion
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ��ȡͨ�и�Ψһ��ʾ�������⣩
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetTrafficPost(string currentid, int type)
        {
            string res = string.Empty;
            try
            {
                switch (type)
                {
                    case 1://˽�ҳ�
                        res = currentid;
                        break;
                    case 6://��ʱͨ�г���
                        res = currentid;
                        break;
                    default:
                        DataItemDetailService itembll = new DataItemDetailService();
                        IEnumerable<DataItemModel> list = itembll.GetDataItemListByItemCode("KmCarType");
                        var entity = list.Where(a => a.ItemValue == type.ToString()).FirstOrDefault();
                        if (entity != null && entity.Description != null)
                        {//�������������
                            res = entity.Description.Replace("<p>", "").Replace("</p>", "").Trim(); ;
                        }
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return res;
        }

        /// <summary>
        /// ��ȡ����Ⱥ����Ϣ
        /// </summary>
        /// <returns></returns>
        public string GetGroupInfo(int type)
        {
            string res = string.Empty;
            try
            {
                DataItemDetailService itembll = new DataItemDetailService();
                IEnumerable<DataItemModel> list = itembll.GetDataItemListByItemCode("KmCarType");
                var entity = list.Where(a => a.ItemValue == type.ToString()).FirstOrDefault();
                if (entity != null)
                {//�������������
                    res = entity.ItemCode;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return res;
        }


        #endregion

    }
}
