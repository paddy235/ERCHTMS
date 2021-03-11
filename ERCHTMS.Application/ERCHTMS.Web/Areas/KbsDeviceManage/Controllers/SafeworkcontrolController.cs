using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.Busines.KbsDeviceManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.OccupationalHealthManage;
using System.Collections.Generic;
using ERCHTMS.Busines.MatterManage;
using System;
using System.Data;
using System.Linq;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Web.Areas.KbsDeviceManage.Models;
using Newtonsoft.Json;
using ERCHTMS.Code;

namespace ERCHTMS.Web.Areas.KbsDeviceManage.Controllers
{
    /// <summary>
    /// �� ������ҵ�ֳ���ȫ�ܿ� 
    /// </summary>
    public class SafeworkcontrolController : MvcControllerBase
    {
        private SafeworkcontrolBLL safeworkcontrolbll = new SafeworkcontrolBLL();
        private OperticketmanagerBLL Opertickebll = new OperticketmanagerBLL();
        private DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();

        #region ��ͼ����
        /// <summary>
        /// �б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }
        /// <summary>
        /// �ֳ�������ҳ
        /// </summary>
        /// <returns></returns>
        public ActionResult WorkHome()
        {
            return View();
        }

        /// <summary>
        /// Ԥ����Ϣ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult WarningInfo()
        {
            return View();
        }
        /// <summary>
        /// Ԥ����Ϣ����
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult WarningInfoForm()
        {
            return View();
        }
        /// <summary>
        /// Ԥ����Ϣ����
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ShowArea()
        {
            return View();
        }
        /// <summary>
        /// �鿴ץ�ļ�¼
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowRecordImg()
        {
            return View();
        }


        #endregion

        #region ��ȡ����


        /// <summary>
        /// ��ȡ������Ϣ
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetSafeworkItemJson(string itemcode)
        {
            List<ComboxsEntity> Rlist = new List<ComboxsEntity>();
            DataItemDetailBLL pdata = new DataItemDetailBLL();
            var list = pdata.GetDataItemListByItemCode(itemcode);
            if (list != null)
            {
                foreach (var item in list)
                {
                    ComboxsEntity y1 = new ComboxsEntity();
                    y1.itemName = item.ItemName;
                    y1.itemValue = item.ItemValue;
                    y1.Key = item.SimpleSpelling;
                    Rlist.Add(y1);
                }
            }
            return ToJsonResult(Rlist);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = safeworkcontrolbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡ��ҳ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();    
            pagination.p_kid = "ID";
            pagination.p_fields = "workNo,taskName,taskType,taskManageName,taskRegionName,deptName,ActualStartTime,ActualEndTime,planenstarttime,planendtime,dangerlevel ";
            pagination.p_tablename = @" bis_SafeWorkControl ";
            pagination.conditionJson = " 1=1";

            var data = safeworkcontrolbll.GetPageList(pagination, queryJson);
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch),

            };
            return Content(JsonData.ToJson());
        }
        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = safeworkcontrolbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡԤ��ʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetWarningInfoEntity(string keyValue)
        {
            var data = safeworkcontrolbll.GetWarningInfoEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ҳ��ȡ���������쳣����ͳ��
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetWorkWarningGroupJson(string type)
        {
            DateTime stime = LinqHelper.Getmondydate(DateTime.Now);
            DateTime etime = LinqHelper.GetSundayDate(DateTime.Now); 
            string res = string.Empty;
            if (type == "����") res = " and TO_CHAR(createdate,'yyyy-MM-dd')>='" + stime.ToString("yyyy-MM-dd") + "'  and TO_CHAR(createdate,'yyyy-MM-dd')<='" + etime.ToString("yyyy-MM-dd") + "'";
            else res = " and TO_CHAR(createdate,'yyyy-MM')='" + DateTime.Now.ToString("yyyy-MM") + "' ";

            string sql = string.Format("select d.deptname,count(1) as num from bis_earlywarning d where d.type=0 {0}  group by d.deptname,d.deptcode", res);
            var dt = Opertickebll.GetDataTable(sql);

            return Content(dt.ToJson());
        }

        /// <summary>
        /// ��ҳ��ȡ��ҵ������ͳ��
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetWorkMonthGroupJson(int type)
        {
            //�ֳ�����
            string sql = string.Format("select to_char(Actualstarttime,'yyyy-MM') as month, count(1) as num from bis_safeworkcontrol d where  to_char(Actualstarttime,'yyyy')={0} group  by  to_char(Actualstarttime,'yyyy-MM')", DateTime.Now.Year);
            if (type == 1)
            {//Ԥ����ͳ��
                sql = string.Format("select to_char(createdate,'yyyy-MM') as month, count(1) as num from bis_earlywarning d where type=0 and to_char(createdate,'yyyy')={0} group  by  to_char(createdate,'yyyy-MM')", DateTime.Now.Year);
            }
            DataTable dt = Opertickebll.GetDataTable(sql);
            List<int> list = new List<int>();
            for (int i = 1; i < 13; i++)
            {
                bool boor = true;
                foreach (DataRow Rows in dt.Rows)
                {
                    var num = Rows[0].ToString().Split('-')[1];
                    int number = int.Parse(num);
                    if (number == i)
                    {
                        boor = false;
                        list.Add(int.Parse(Rows[1].ToString()));
                    }
                }
                if (boor)
                {
                    list.Add(0);
                }
            }
            return Content(list.ToJson());
        }

        /// <summary>
        /// ��ҵʵʱ�ֲ�ͳ��ͼ
        /// </summary>
        /// <returns></returns>
        public ActionResult GetWorkRealTimeDistribution()
        {
            string sql = string.Format(@"select tasktype,count(1) as num from bis_safeworkcontrol d where (
(Actualstarttime<=to_date('{0}','yyyy-mm-dd hh24:mi:ss') and  
ActualEndTime >=to_date('{0}','yyyy-mm-dd hh24:mi:ss')
)or( Actualstarttime<=to_date('{0}','yyyy-mm-dd hh24:mi:ss') and  ActualEndTime is null)) and state=1  group by d.tasktype", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            DataTable dt = Opertickebll.GetDataTable(sql);
            DataItemDetailBLL pdata = new DataItemDetailBLL();
            var list = pdata.GetDataItemListByItemCode("SafeWorkType");
            List<KbsEntity> klist = new List<KbsEntity>();
            int Znum = 0;
            foreach (var item in list)
            {
                KbsEntity kbs = new KbsEntity();
                kbs.Name = item.ItemName;
                int num = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (item.ItemValue == dt.Rows[i][0].ToString())
                    {
                        num = Convert.ToInt32(dt.Rows[i]["num"]);
                        break;
                    }
                }
                kbs.Num = num;
                Znum += num;
                klist.Add(kbs);
            }
            for (int j = 0; j < klist.Count; j++)
            {
                double Proportion = 0;
                if (Znum != 0)
                {
                    Proportion = (double)klist[j].Num / Znum;
                    Proportion = Proportion * 100;
                }
                klist[j].Proportion = Proportion.ToString("0.00") + "%";
            }
            dt.Dispose();
            return Content(klist.ToJson());
        }

        /// <summary>
        /// ͳ��ͼ����ʾ��Ϣ
        /// </summary>
        /// <returns></returns>
        public string GetLableChart()
        {
            List<object[]> list = new List<object[]>();
            string sql = string.Format("select tasktype,count(1) as num from bis_safeworkcontrol d where Actualstarttime<=to_date('{0}','yyyy-mm-dd hh24:mi:ss') and ActualEndTime>=to_date('{0}','yyyy-mm-dd hh24:mi:ss') and state=1  group by d.tasktype", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            DataTable dt = Opertickebll.GetDataTable(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                object[] arr = { dt.Rows[i][0].ToString(), Convert.ToInt32(dt.Rows[i][1]) };
                list.Add(arr);
            }
            dt.Dispose();
            return Newtonsoft.Json.JsonConvert.SerializeObject(list);
        }

        /// <summary>
        /// ��ҵʵʱ�ֲ�ͳ�Ʊ�
        /// </summary>
        /// <returns></returns>
        public ActionResult GetWorkRealTimeTableJson()
        {
            DistrictBLL districtbll = new DistrictBLL();
            List<DistrictEntity> AreaList = districtbll.GetListByOrgIdAndParentId("", "0");
            List<SafeworkcontrolEntity> WorkList = safeworkcontrolbll.GetNowWork();
            List<KbssEntity> klist = new List<KbssEntity>();
            int Znum = 0;
            foreach (var item in AreaList)
            {
                KbssEntity kbs = new KbssEntity();
                kbs.Name = item.DistrictName;
                kbs.Num = WorkList.Where(a => a.Taskregioncode.Contains(item.DistrictCode)).Count();
                kbs.DistrictCode = item.DistrictCode;
                Znum += kbs.Num;
                kbs.OnNum = WorkList.Where(a => a.Taskregioncode.Contains(item.DistrictCode)).Count();
                klist.Add(kbs);
            }
            for (int j = 0; j < klist.Count; j++)
            {
                double Proportion = 0;
                if (Znum != 0)
                {
                    Proportion = (double)klist[j].Num / Znum;
                    Proportion = Proportion * 100;
                }
                klist[j].OnNum = Znum;
                klist[j].OnProportion = Proportion.ToString("0") + "%";
            }
            return Content(klist.ToJson());
        }

        /// <summary>
        /// ��ȡ��ҵץ���б���Ϣ
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetRecordImg(string keyValue)
        {
            List<WorkcameracaptureEntity> list = new WorkcameracaptureBLL().GetCaptureList(keyValue, "", "");
            return ToJsonResult(list);
        }


        #endregion

            #region �ύ����
            /// <summary>
            /// ɾ������
            /// </summary>
            /// <param name="keyValue">����ֵ</param>
            /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            safeworkcontrolbll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }

        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, SafeworkcontrolEntity entity)
        {
            try
            {
                if (entity.State == 0)
                {//����
                    var data = safeworkcontrolbll.GetEntity(keyValue);
                    if (data != null)
                    {
                        data.Planendtime = entity.Planendtime;
                        safeworkcontrolbll.AppSaveForm(keyValue, data);
                    }
                }
                 else if (entity.State == 1)
                {//�ύ��ҵ
                    entity.comerid = GetElectricFenceCameraList(entity);
                    entity.Takeeffecttime = entity.Actualstarttime;
                    entity.Create();
                    safeworkcontrolbll.AppSaveForm(keyValue, entity);
                    //���ֳ�������Ϣͬ������̨���������
                    RabbitMQHelper rh = RabbitMQHelper.CreateInstance();
                    SendData sd = new SendData();
                    sd.DataName = "AddSafeworkcontrolEntity";
                    sd.EntityString = JsonConvert.SerializeObject(entity);
                    rh.SendMessage(JsonConvert.SerializeObject(sd));
                   
                }

                else if (entity.State == 2)
                {//������ҵ
                    var data = safeworkcontrolbll.GetEntity(keyValue);
                    if (data != null)
                    {
                        data.ActualEndTime = entity.ActualEndTime;
                        data.State = entity.State;
                        data.Invalidtime = entity.ActualEndTime;
                        safeworkcontrolbll.SaveForm(data.ID, data);
                        //֪ͨ��άɾ����ӦԤ����Ϣ
                        RabbitMQHelper rh = RabbitMQHelper.CreateInstance();
                        var list = safeworkcontrolbll.GetBatchWarningInfoList(data.ID);
                        if (list.Count > 0)
                        {
                            foreach (var item in list)
                            {
                                if (item.type == 0 && item.NoticeType != 2)
                                {//��Աδ����
                                    SendTDWarning(3, item.WarningContent, item.LiableId, item.BaseId, rh);
                                }
                                else if (item.type == 0 && item.NoticeType == 2)
                                {//����ҵ��Ա��
                                    SendTDWarning(4, item.WarningContent, item.LiableId, item.Remark, rh);
                                }
                            }
                            //���ֳ�������Ϣͬ������̨���������
                            SendData sd = new SendData();
                            sd.DataName = "DelSafeworkcontrolEntity";
                            sd.EntityString = data.ID;
                            rh.SendMessage(JsonConvert.SerializeObject(sd));
                        }
                        else
                        {
                            //���ֳ�������Ϣͬ������̨���������
                            SendData sd = new SendData();
                            sd.DataName = "DelSafeworkcontrolEntity";
                            sd.EntityString = data.ID;
                            rh.SendMessage(JsonConvert.SerializeObject(sd));
                        }
                    }
                }
                return Success("�����ɹ���");
            }
            catch (Exception ex)
            {
                return Success("����ʧ�ܣ�");
            }
        }




        /// <summary>
        /// ��ȡ����Χ������������ͷ��Ϣ
        /// </summary>
        public string GetElectricFenceCameraList(SafeworkcontrolEntity data)
        {
            string ComerId = string.Empty;
            try
            {
                //��ά��ص�λ����ӿڵ�ַ
                DataItemDetailBLL pdata = new DataItemDetailBLL();
                var ThreeDApi = pdata.GetItemValue("kbsThreeMonitoringPoints");
                space sp = new space();
                spacedata sds = new spacedata();
                List<spacegeo> geolist = new List<spacegeo>();
                List<spacepnt> pntlist = new List<spacepnt>();
                var comList = new KbscameramanageBLL().GetPageList("").Where(a => a.State == "����" && a.CameraType == "ǹ��" && a.MonitoringArea != null).ToList();
                spacegeo geo = new spacegeo();
                geo.id = data.ID;
                if (data.Areacode != null)
                {//����Χ�������
                    PositionsEntity alist = JsonConvert.DeserializeObject<PositionsEntity>(data.Areacode);
                    List<double> dlist = new List<double>();
                    if (data.Areastate == 0)
                    {//����
                        geo.type = 1;
                        geo.distance = 0;
                        //��һ����
                        dlist.Add(alist.positions[0].x);
                        dlist.Add(alist.positions[0].z);
                        //�ڶ�����
                        dlist.Add(alist.positions[1].x);
                        dlist.Add(alist.positions[0].z);
                        //��������
                        dlist.Add(alist.positions[1].x);
                        dlist.Add(alist.positions[1].z);
                        //���ĸ���
                        dlist.Add(alist.positions[0].x);
                        dlist.Add(alist.positions[1].z);
                        //�ص�ԭ��
                        dlist.Add(alist.positions[0].x);
                        dlist.Add(alist.positions[0].z);
                        geo.coor = dlist;
                        geolist.Add(geo);
                    }
                    else if (data.Areastate == 1)
                    {//Բ��
                        geo.type = 0;
                        dlist.Add(alist.positions[0].x);
                        dlist.Add(alist.positions[0].z);
                        geo.distance = Convert.ToInt32(data.Radius);
                        geo.coor = dlist;
                        geolist.Add(geo);
                    }
                    else if (data.Areastate == 2)
                    {//�ֻ�����
                        foreach (var ac in alist.positions)
                        {
                            dlist.Add(ac.x);
                            dlist.Add(ac.z);
                        }
                        //�պϻص�ԭ��
                        dlist.Add(alist.positions[0].x);
                        dlist.Add(alist.positions[0].z);
                        geo.coor = dlist;
                        geo.type = 1;
                        geo.distance = 0;
                        geolist.Add(geo);
                    }

                    foreach (var com in comList)
                    {//����ͷ����㼯
                        PositionsEntity alist1 = JsonConvert.DeserializeObject<PositionsEntity>(com.MonitoringArea);
                        if (alist1.floorID == alist.floorID)
                        {
                            List<double> dlist1 = new List<double>();
                            foreach (var ac in alist1.positions)
                            {
                                dlist1.Add(ac.x);
                                dlist1.Add(ac.z);
                            }
                            //�պϻص�ԭ��
                            dlist1.Add(alist1.positions[0].x);
                            dlist1.Add(alist1.positions[0].z);
                            spacepnt pnt = new spacepnt()
                            {
                                id = com.CameraId,
                                x = 0,
                                y = 0,
                                coor = dlist1
                            };
                            pntlist.Add(pnt);
                        }
                    }
                }
                sds.geo = geolist;
                sds.pnt = pntlist;
                sp.type = "2";
                sp.data = sds;
                string rtn = HttpUtillibKbs.HttpThreeDPost(ThreeDApi, JsonConvert.SerializeObject(sp));
                List<RtnSpace> rss = JsonConvert.DeserializeObject<List<RtnSpace>>(rtn);
                foreach (var item in rss)
                {
                    if (string.IsNullOrEmpty(item.pntId)) continue;
                    ComerId += item.pntId + ',';
                }
                return ComerId;
            }
            catch (Exception er)
            {
                return ComerId;
            }
        }


        /// <summary>
        /// ���͸���ά����Ԥ��
        /// </summary>
        /// <param name="we"></param>
        public void SendTDWarning(int type, string warningContent, string liableId, string baseId, RabbitMQHelper rh)
        {
            try
            {
                WarningEntity we = new WarningEntity
                {
                    type = type,
                    WarningContent = warningContent,
                    LiableName = "",
                    LiableId = liableId,
                    BaseId = baseId,
                    deptCode = "",
                    deptName = "",
                    TaskName = "",
                    typeIds = ""
                };
                //���͸���ά��ʱ���Ϊ��ǩID
                SendData sd = new SendData();
                sd.DataName = "WarningEnd";
                sd.EntityString = JsonConvert.SerializeObject(we);
                rh.SendMessage(JsonConvert.SerializeObject(sd));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        public ViewResult Edit(string id)
        {

            var items = dataItemDetailBLL.GetListItems("��ȫ�ܿ���ҵ����");
            ViewData["Tasktype"] = items.Select(x => new SelectListItem { Value = x.ItemName, Text = x.ItemName });

            items = dataItemDetailBLL.GetListItems("��ҵ���յȼ�");
            ViewData["DangerLevel"] = items.Select(x => new SelectListItem { Value = x.ItemName, Text = x.ItemName });

            var model = new TaskModel
            {
                Actualstarttime = DateTime.Now
            };
            return View(model);
        }


        public ViewResult OpenMap()
        {
            return View();
        }



    }



    public static class LinqHelper
    {


        public static DateTime Getmondydate(this DateTime somedate)
        {
            int i = somedate.DayOfWeek - DayOfWeek.Monday;
            if (i == -1) i = 6;
            TimeSpan ts = new TimeSpan(i, 0, 0, 0);
            return somedate.Subtract(ts);
        }
        public static DateTime GetSundayDate(this DateTime someDate)
        {
            int i = someDate.DayOfWeek - DayOfWeek.Sunday;
            if (i != 0) i = 7 - i;
            TimeSpan ts = new TimeSpan(i, 0, 0, 0);
            return someDate.Add(ts);
        }


    }

}
