using System;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Http;
using BSFramework.Cache.Factory;
using BSFramework.Util;
using ERCHTMS.Busines.CarManage;
using ERCHTMS.Busines.MatterManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Entity.MatterManage;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ERCHTMS.AppSerivce.Controllers
{
    /// <summary>
    /// 计量管理-称重计量
    /// </summary>
    public class CalculateManageController : BaseApiController
    {
        private CalculateBLL calculatebll = new CalculateBLL();
        private OperticketmanagerBLL Opertickebll = new OperticketmanagerBLL();
        private CarinlogBLL carbll = new CarinlogBLL();
        DataItemDetailBLL pdata = new DataItemDetailBLL();
        string msg = "操作成功";
        #region 获取数据

        /// <summary>
        /// 获取新地磅室称重信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getCalculate([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            try
            {
                if (dy != null)
                {
                    if (dy.Weight != null)
                    {
                        CacheFactory.Cache().WriteCache(dy.Weight, "PoundA:Weight");
                        var data = new { DataType = 1, Data =Convert.ToInt32(dy.Weight) };
                        string url = CacheFactory.Cache().GetCache<string>("SignalRUrl");
                        if (url.IsNullOrWhiteSpace())
                        {                            
                            url = pdata.GetItemValue("SignalRUrl");
                            url = url.Replace("signalr", "").Replace("\"", "");
                            CacheFactory.Cache().WriteCache<string>(url, "SignalRUrl");
                        }
                        HubConnection hubConnection = null;
                        IHubProxy ChatsHub = null;
                        try
                        {
                            hubConnection = new HubConnection(url);
                            ChatsHub = hubConnection.CreateHubProxy("ChatsHub");
                            hubConnection.Start().ContinueWith(task =>
                            {
                                if (!task.IsFaulted)
                                //连接成功调用服务端方法
                                {
                                    ChatsHub.Invoke("sendMsgKm", "192.168.9.234", data.ToJson());
                                    //结束连接
                                    //hubConnection.Stop();
                                }
                            });
                        }
                        catch (Exception e)
                        {
                            string fileName = "PoundAError" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/logs")))
                            {
                                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/logs"));
                            }
                            System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：地磅称重数据读取异常：" + e.Message + "Json:" + json + "\r\n");
                            return new { Code = -1, Count = 0, Info = e.Message, data = "" };
                        }
                    }

                    //获取用户基本信息
                    //OperatorProvider.AppUserId = "System";  //设置当前用户   
                    //Operator user = OperatorProvider.Provider.Current();
                    //if (user == null)
                    //{//默认超级管理员登录
                    //    curUser = GetOperator(OperatorProvider.AppUserId);
                    //}
                    //CarinlogEntity entity = GetCarNewNumber();//获取正在入场车牌号码
                    //if (entity != null)
                    //{
                    //    if (entity.Type == 4)
                    //    {//物料车
                    //        RckpAddRecord(dy, entity.CarNo);
                    //    }
                    //    else if (entity.Type == 5)
                    //    {//危化品车
                    //        WhpAddRecord(dy, entity.CarNo);
                    //    }
                    //    else if (entity.Type == 99)
                    //    {//外来车辆
                    //        wlclAddRecord(dy, entity.CarNo);
                    //    }
                    //}
                }
                return new { Code = 0, Count = -1, Info = msg, data = new { } };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

    }
}
