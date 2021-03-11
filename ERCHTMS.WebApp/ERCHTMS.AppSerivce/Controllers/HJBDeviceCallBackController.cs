using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Http;
using ERCHTMS.Busines.PersonManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Entity.PersonManage;
using Newtonsoft.Json;

namespace ERCHTMS.AppSerivce.Controllers
{
    public class HJBDeviceCallBackController : BaseApiController
    {
        EarlyWarningBLL earlyWarningBll = new EarlyWarningBLL();

        /// <summary>
        /// 海康身份证识别仪回调接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object ReadIdentityCard()
        {
            string json = "";
            string fileName = "ReadIdentityCard" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            try
            {
                Stream s = System.Web.HttpContext.Current.Request.InputStream;
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                json = reader.ReadToEnd();
                SocketHelper.SetLog(fileName, "读取身份证信息", json);


                return new { Code = 0, Count = 1, Info = "接受数据成功", data = "" };
            }
            catch (Exception e)
            {
                if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/logs")))
                {
                    System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/logs"));
                }
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：读取身份证信息存储异常：" + e.Message + "Json:" + json + "\r\n");
                return new { Code = -1, Count = 0, Info = e.Message, data = "" };
            }
        }

        /// <summary>
        /// 海康智能摄像头识别回调接口 1417219
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object SmartCameraDiscern()
        {
            string json = "";
            string fileName = "SmartCameraDiscern" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            try
            {
                Stream s = System.Web.HttpContext.Current.Request.InputStream;
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                json = reader.ReadToEnd();
                SocketHelper.SetLog(fileName, "智能摄像头识别", json);
                HikSmartCamera data = JsonConvert.DeserializeObject<HikSmartCamera>(json);

                foreach (SafetyHelmeDetection obj in data.safetyHelmeDetection)
                {
                    string code = new Random().Next().ToString();
                    EarlyWarningEntity entity = new EarlyWarningEntity()
                    {
                        AreaCode = code,
                        AreaName = "汽机房区域" + code,
                        DepartCode = "",
                        DepartName = "电气一次班" + code,
                        DeviceIndex = obj.targetAttrs.deviceIndexCode,
                        DeviceName = string.Format("{0}#智能摄像头", code),
                        DutyPerson = "江伟" + code,
                        DutyPersonId = code,
                        PicUrl = obj.imageUrl,
                        WarningContent = obj.targetAttrs.cameraAddress + "江伟得小弟进入除灰间未佩戴安全头盔" + code,
                        WarningTime = data.dateTime.HasValue ? data.dateTime.Value : DateTime.Now
                    };

                    earlyWarningBll.SaveForm("", entity);
                }

                return new { Code = 0, Count = 1, Info = "接受数据成功", data = "" };
            }
            catch (Exception e)
            {
                if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/logs")))
                    System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/logs"));
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：智能摄像头识别存储异常：" + e.Message + "Json:" + json + "\r\n");
                return new { Code = -1, Count = 0, Info = e.Message, data = "" };
            }
        }
    }
}
