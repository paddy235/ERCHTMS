using ERCHTMS.Busines.OccupationalHealthManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.OccupationalHealthManage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BSFramework.Util;
using System.Web;
using ERCHTMS.AppSerivce.Model;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.SystemManage;

namespace ERCHTMS.AppSerivce.Controllers
{
    public class OccupationalHealthController : BaseApiController
    {
        private HazardfactorsBLL hazardfactorsbll = new HazardfactorsBLL();
        private HazarddetectionBLL hazarddetectionbll = new HazarddetectionBLL();
        /// <summary>
        /// 输出危险因素下拉数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetRiskCmbList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;
                Operator user = OperatorProvider.Provider.Current();

                //先获取所有数据
                IEnumerable<HazardfactorsEntity> hlist = hazardfactorsbll.PhoneGetList("", user.OrganizeCode);
                List<ComboxEntity> Rlist = new List<ComboxEntity>();
                List<string> conStr = new List<string>();//用于判断重复的集合
                foreach (HazardfactorsEntity item in hlist)
                {
                    ComboxEntity risk = new ComboxEntity();
                    risk.itemName = item.RiskValue;
                    risk.itemValue = item.Riskid;
                    if (!conStr.Contains(item.RiskValue))
                    {
                        Rlist.Add(risk);//如果没有重复则加入
                        conStr.Add(item.RiskValue);
                    }
                }

                return new { Code = 0, Count = Rlist.Count, Info = "获取数据成功", data = Rlist };
            }
            catch (Exception ex)
            {

                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 根据所选区域id输出危险因素下拉数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetArRiskCmbList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string areaid = dy.data.areaid;
                string userId = dy.userid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;
                Operator user = OperatorProvider.Provider.Current();

                //先获取所有数据
                IEnumerable<HazardfactorsEntity> hlist = hazardfactorsbll.PhoneGetList(areaid, user.OrganizeCode);
                List<ComboxEntity> Rlist = new List<ComboxEntity>();
                List<string> conStr = new List<string>();//用于判断重复的集合
                foreach (HazardfactorsEntity item in hlist)
                {

                    ComboxEntity risk = new ComboxEntity();
                    risk.itemName = item.RiskValue;
                    risk.itemValue = item.Riskid;
                    if (!conStr.Contains(item.RiskValue))
                    {
                        Rlist.Add(risk);//如果没有重复则加入
                        conStr.Add(item.RiskValue);
                    }

                }
                return new { Code = 0, Count = Rlist.Count, Info = "获取数据成功", data = Rlist };
            }
            catch (Exception ex)
            {

                return new { Code = -1, Count = 0, Info = ex.Message };
            }

        }

        /// <summary>
        /// 获取最近一次测量指标及标准
        /// </summary>
        /// <param name="RiskId">职业病危害因素ID</param>
        /// <returns>返回对象Json</returns>
        [HttpPost]
        public object GetStandard([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string RiskId = dy.data.riskid;
                string userId = dy.userid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;
                Operator user = OperatorProvider.Provider.Current();
                string where = string.Format(" and CREATEUSERORGCODE = '{0}'", user.OrganizeCode);
                string Sta = hazarddetectionbll.GetStandard(RiskId, where);
                List<StandardData> slist = new List<StandardData>();
                object cmb;
                if (Sta.Trim().Length > 0)
                {
                    string[] stand = Sta.Trim().Split(';');
                    for (int i = 0; i < stand.Length; i++)
                    {
                        string[] svalue = stand[i].Split(',');
                        StandardData sdata = new StandardData();
                        sdata.name = svalue[0];
                        sdata.value = svalue[1];
                        sdata.maxValue = svalue[2];
                        slist.Add(sdata);
                    }
                    cmb = new { standardName = "true", standardValue = slist };
                }
                else
                {
                    cmb = new { standardName = "false", standardValue = slist };
                }
                //ComboxEntity cmb = new ComboxEntity();
                //if (Sta != null && Sta != "")
                //{
                //    cmb.itemName = "true";
                //    cmb.itemValue = Sta;
                //}
                //else
                //{
                //    cmb.itemName = "false";
                //    cmb.itemValue = "";
                //}
                return new { Code = 0, Count = 1, Info = "获取数据成功", data = cmb };
            }
            catch (Exception ex)
            {

                return new { Code = -1, Count = 0, Info = ex.Message };
            }

        }

        /// <summary>
        /// 输出查看标准html路径
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetHtml([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string name = dy.data.name;
                DataItemBLL di = new DataItemBLL();
                //先获取到字典项
                DataItemEntity DataItems = di.GetEntityByCode("Standard");

                DataItemDetailBLL did = new DataItemDetailBLL();
                //根据字典项获取值
                IEnumerable<DataItemDetailEntity> didList = did.GetList(DataItems.ItemId);

                List<DataItemDetailEntity> dlist = didList.Where(it => it.ItemName == name).ToList();
                if (dlist.Count > 0)
                {
                    string path = did.GetItemValue("imgUrl") + "/Content/SecurityDynamics/Phone.html?url=" + dlist[0].Description + "&top=" + did.GetItemValue("imgUrl");
                    return new { Code = 0, Count = 1, Info = "获取数据成功", url = path };
                }
                else
                {
                    return new { Code = 0, Count = 1, Info = "获取数据成功", url = "" };
                }

            }
            catch (Exception ex)
            {

                return new { Code = -1, Count = 0, Info = ex.Message };
            }


        }

        /// <summary>
        /// 输出区域下拉数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetAreaCmbList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;
                Operator user = OperatorProvider.Provider.Current();

                //先获取所有数据
                IEnumerable<HazardfactorsEntity> hlist = hazardfactorsbll.PhoneGetList("", user.OrganizeCode);
                List<ComboxEntity> Rlist = new List<ComboxEntity>();
                List<string> conStr = new List<string>();//用于判断重复的集合
                foreach (HazardfactorsEntity item in hlist)
                {
                    ComboxEntity risk = new ComboxEntity();
                    risk.itemName = item.AreaValue;
                    risk.itemValue = item.AreaId;
                    if (!conStr.Contains(item.AreaValue))
                    {
                        Rlist.Add(risk);//如果没有重复则加入
                        conStr.Add(item.AreaValue);
                    }
                    //ComboxEntity risk = new ComboxEntity();
                    //risk.itemName = item.AreaValue;
                    //risk.itemValue = item.AreaId;
                    //Rlist.Add(risk);//如果没有重复则加入

                }

                return new { Code = 0, Count = Rlist.Count, Info = "获取数据成功", data = Rlist };
            }
            catch (Exception ex)
            {

                return new { Code = -1, Count = 0, Info = ex.Message };
            }

        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpPost]
        public object GetListJson([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;
                Operator user = OperatorProvider.Provider.Current();
                string riskid = dy.data.riskid;
                string areaid = dy.data.areaid;
                string starttime = dy.data.starttime;
                string endtime = dy.data.endtime;
                string isexcessive = dy.data.isexcessive;
                string detectionuserid = dy.data.detectionuserid;
                string where = string.Format(" and CREATEUSERORGCODE='{0}'", user.OrganizeCode);
                //获取全部数据
                var data = hazarddetectionbll.GetDataTable(riskid, areaid, starttime, endtime, isexcessive, detectionuserid, where);
                //data.Rows.Count
                return new { Code = 0, Count = data.Rows.Count, Info = "获取数据成功", data = data };
            }
            catch (Exception ex)
            {

                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpPost]
        public object GetFormJson([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string id = dy.data.hid;
                HazarddetectionEntity data = hazarddetectionbll.GetEntity(id);
                if (data != null)
                {
                    List<StandardData> slist = new List<StandardData>();
                    string[] stand = data.Standard.Split(';');
                    for (int i = 0; i < stand.Length; i++)
                    {
                        string[] svalue = stand[i].Split(',');
                        StandardData sdata = new StandardData();
                        sdata.name = svalue[0];
                        sdata.value = svalue[1];
                        sdata.maxValue = svalue[2];
                        slist.Add(sdata);
                    }
                    var entity = new
                    {
                        Hid = data.HId,
                        AreaId = data.AreaId,
                        AreaValue = data.AreaValue,
                        RiskId = data.RiskId,
                        RiskValue = data.RiskValue,
                        Location = data.Location,
                        StartTime = data.StartTime,
                        EndTime = data.EndTime,
                        Standard = slist,
                        DetectionUserId = data.DetectionUserId,
                        DetectionUserName = data.DetectionUserName,
                        IsExcessive = data.IsExcessive
                    };
                    return new { Code = 0, Count = 1, Info = "获取数据成功", data = entity };
                }
                else
                {
                    return new { Code = 0, Count = 0, Info = "请检查hid是否正确", data = "" };
                }


            }
            catch (Exception ex)
            {

                return new { Code = -1, Count = 0, Info = ex.Message };
            }

        }

        /// <summary>
        /// 新增数据 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpPost]
        public object Insert([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string stan = JsonConvert.SerializeObject(dy.data.standard);
                if (stan == "")
                {
                    return new { Code = -1, Count = 0, Info = "测量指标及测量值请勿传空值" };
                }
                List<StandardData> datalist = JsonConvert.DeserializeObject<List<StandardData>>(stan);
                string standard = "";
                for (int i = 0; i < datalist.Count; i++)
                {
                    if (i == 0)
                    {
                        standard = datalist[i].name + "," + datalist[i].value + "," + datalist[i].maxValue;
                    }
                    else
                    {
                        standard += ";" + datalist[i].name + "," + datalist[i].value + "," + datalist[i].maxValue;
                    }
                }
                HazarddetectionEntity ha = new HazarddetectionEntity();
                ha.AreaId = dy.data.areaid;
                ha.AreaValue = dy.data.areavalue;
                ha.RiskId = dy.data.riskid;
                ha.RiskValue = dy.data.riskvalue;
                ha.Location = dy.data.location;
                ha.StartTime = Convert.ToDateTime(dy.data.starttime);
                ha.EndTime = Convert.ToDateTime(dy.data.endtime);
                ha.Standard = standard;
                ha.DetectionUserId = dy.data.detectionuserid;
                ha.DetectionUserName = dy.data.detectionusername;
                ha.IsExcessive = Convert.ToInt32(dy.data.isexcessive);
                string userId = dy.userid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;
                Operator user = OperatorProvider.Provider.Current();
                hazarddetectionbll.SaveHazard(ha, user);
                return new { Code = 0, Count = 0, Info = "新增数据成功" };
            }
            catch (Exception ex)
            {

                return new { Code = -1, Count = 0, Info = ex.Message };
            }

        }


    }
}
