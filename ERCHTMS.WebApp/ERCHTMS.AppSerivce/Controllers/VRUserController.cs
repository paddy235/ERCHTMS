using BSFramework.Util.WebControl;
using ERCHTMS.Busines.RiskDatabase;
using ERCHTMS.Code;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BSFramework.Util;
using ERCHTMS.Busines.MatterManage;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;
using ERCHTMS.Entity.CarManage;

namespace ERCHTMS.AppSerivce.Controllers
{
    /// <summary>
    /// 可门首页三维接口集合
    /// </summary>
    public class VRUserController : BaseApiController
    {
        private OperticketmanagerBLL operticketmanagerbll = new OperticketmanagerBLL();

        /// <summary>
        /// 获取在厂拜访人员信息列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetUserInformationList(string  json)
        {
            try
            {
                //string res = //json.Value<string>("json");//[FromBody]JObject
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(json);
                string userId = dy.userid;
                string username = dy.data.username;//姓名
                string jobnumber = dy.data.jobnumber;//手机号

                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    curUser = GetOperator(dy.userid);
                    if (curUser == null)
                    {
                        return new { code = -1, count = 0, info = "当前用户不存在,请核对用户信息!" };
                    }
                }
                Pagination pagination = new Pagination();
                pagination.p_kid = "id";
                pagination.p_fields = @" d.username,d.vid,d.gpsid,d.gpsname,d.intime,d.outtime,d.state,'' as phone,'' as comname ";
                pagination.p_tablename = "bis_persongps  d";
                pagination.conditionJson = " state=0 ";
                //pagination.sidx = pagination.sidx + " " + pagination.sord + ",id";

                if (!string.IsNullOrEmpty(username))
                { //姓名
                    pagination.conditionJson += " and username like '%" + username.Trim() + "%'";
                }
                if (!string.IsNullOrEmpty(dy.data.starttime))
                {//
                    pagination.conditionJson += " and username like '" + jobnumber.Trim() + "%'";
                }

                //获取页数和条数
                int page = Convert.ToInt32(dy.data.pagenum), rows = Convert.ToInt32(dy.data.pagesize);
                pagination.page = page;//页数
                pagination.rows = rows;//行数
                pagination.sidx = "createdate";//排序字段
                pagination.sord = "desc";//排序方式
                DataTable data = operticketmanagerbll.GetPageList(pagination, null);
                if (data.Rows.Count > 0)
                {
                    foreach (DataRow Rows in data.Rows)
                    {
                        string pid = Rows[2].ToString();
                        string sql = string.Format("select d.dirver,d.phone,CONCAT(CONCAT(d.visitdept,'/'),d.carno) as purpose from bis_usercar d where d.id='{0}' ", pid);
                        string sql1 = string.Format("select d.dirver,d.phone,CONCAT(CONCAT(d.visitdept,'/'),d.carno) as purpose from bis_visitcar d where d.id='{0}' ", pid);
                        string sql2 = string.Format("select Phone,TheCompany,d.dirver from bis_hazardouscar d  where d.id='{0}' ", pid);
                        DataTable dt = operticketmanagerbll.GetDataTable(sql);
                        DataTable dt1 = operticketmanagerbll.GetDataTable(sql1);
                        DataTable dt2 = operticketmanagerbll.GetDataTable(sql2);
                        if (dt.Rows.Count > 0)
                        {//拜访（无车）
                            if (Rows[1].ToString() == dt.Rows[0][0].ToString())
                                Rows["phone"] = dt.Rows[0][1].ToString();
                            Rows["comname"] = dt.Rows[0][2].ToString();
                        }
                        else if (dt1.Rows.Count > 0)
                        {//拜访（有车）
                            if (Rows[1].ToString() == dt1.Rows[0][0].ToString())
                                Rows["phone"] = dt1.Rows[0][1].ToString();
                            Rows["comname"] = dt1.Rows[0][2].ToString();
                        }
                        else if (dt2.Rows.Count > 0)
                        {//拜访（危化品）
                            if (Rows[1].ToString() == dt2.Rows[0][2].ToString())
                                Rows["phone"] = dt2.Rows[0][0].ToString();
                            Rows["comname"] = dt2.Rows[0][1].ToString();
                        }
                        dt.Dispose(); dt1.Dispose(); dt2.Dispose();
                    }
                }
                return new { code = 0, info = "获取数据成功", count = pagination.records, data = data.ToJson() };
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }
        }


        /// <summary>
        /// 获取拜访车辆在厂违章记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetCarViolationList(string json)
        {
            try
            {
               // string res = json.Value<string>("json");//[FromBody]JObject
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(json);
                string userId = dy.userid;
                string username = dy.data.username;//姓名
                string jobnumber = dy.data.jobnumber;//工号

                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    curUser = GetOperator(dy.userid);
                    if (curUser == null)
                    {
                        return new { code = -1, count = 0, info = "当前用户不存在,请核对用户信息!" };
                    }
                }

                Pagination pagination = new Pagination();
                string orgcode = OperatorProvider.Provider.Current().OrganizeCode;
                pagination.p_kid = "a1.ID";
                pagination.p_fields = "a1.Createdate, carno, comname,ee.DIRVER,ee.PHONE, '拜访' as objective,ee.PROCESSMEASURE,gpsid";
                pagination.p_tablename = @"(
            select null  Takegoodsname,ID,Createdate,carno,'拜访' as purpose,dirver,dirver as comname
            ,phone,note,ACCOMPANYINGPERSON anumber,driverlicenseurl,drivinglicenseurl,state,'0' type,NVL(vnum,0) vnum,gpsid from bis_visitcar  vi
            left join (select count(cid) vnum ,cid from BIS_CARVIOLATION  group by cid  )  cv on vi.id=cv.cid
            where state=3 

            union 

            select Takegoodsname,ID,Createdate,platenumber carno,'物料' as purpose,Takegoodsname as comname,
            DriverName dirver,DriverTel phone,PassRemark note,JsImgpath anumber,JsImgpath driverlicenseurl,XsImgpath drivinglicenseurl,examinestatus state,'1' type,NVL(vnum,0) vnum,gpsid 
            from WL_OPERTICKETMANAGER vi
            left join (select count(cid) vnum ,cid from BIS_CARVIOLATION  group by cid  )  cv on vi.id=cv.cid
             where isdelete=1 and examinestatus=3 
            
            union 

            select null Takegoodsname,ID,Createdate,carno,'危化品' as purpose,dirver,TheCompany as comname
            ,phone,note,ACCOMPANYINGPERSON anumber,driverlicenseurl,drivinglicenseurl,state,'2' type,NVL(vnum,0) vnum,gpsid from bis_hazardouscar  hazardous
            left join (select count(cid) vnum ,cid from BIS_CARVIOLATION  group by cid  )  cv on hazardous.id=cv.cid
            where state=3 
            ) a1  join BIS_CARVIOLATION ee on a1.id = ee.cid

            ";

                int page = Convert.ToInt32(dy.data.pagenum), rows = Convert.ToInt32(dy.data.pagesize);
                pagination.conditionJson = " 1=1";
                pagination.sord = "desc";
                pagination.records = 0;
                pagination.page = page;//页数
                pagination.rows = rows;//行数
                pagination.sidx = "ee.Createdate";//排序字段


                DataTable data = operticketmanagerbll.GetPageList(pagination, null);
                return new { code = 0, info = "获取数据成功", count = pagination.records, data = data.ToJson() };
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }
        }

        /// <summary>
        /// 通过GpsId获取在厂人员信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetUserEntity(string json)
        {
            DataTable data = new DataTable();
            try
            {
                //string res = json.Value<string>("json");//[FromBody]JObject
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(json);
                string userId = dy.userid;
                string GpsId = dy.gpsid;//gps设备编号

                string sql = string.Format(@" select *
  from (select d.dirver, d.phone, d.comname, '拜访' as objective, t.gpsid
          from bis_persongps t
          join bis_visitcar d on t.vid = d.id
         where t.state = '0'
        union
        select d.dirver,
               d.phone,
               TheCompany as comname,
               '拜访' as objective,
               t.gpsid
          from bis_persongps t
          join bis_hazardouscar d on t.vid = d.id
         where t.state = '0'
        union
        select d.dirver,
               d.phone,
               carno as comname,
               '拜访' as objective,
               t.gpsid
          from bis_persongps t
          join bis_usercar d on t.vid = d.id
         where t.state = '0') tb

 where tb.gpsid = '{0}'
 ", GpsId);
                DataTable dt = operticketmanagerbll.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    data = dt;
                }
                return new { code = 0, info = "获取数据成功", count = data.Rows.Count, data = data.ToJson() };
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }
        }

        /// <summary>
        /// 通过GpsId获取在厂车辆信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetCarEntity(string json)
        {
            try
            {
                DataTable data = new DataTable();
                //string res = json.Value<string>("json");//[FromBody]JObject
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(json);
                string userId = dy.userid;
                string GpsId = dy.gpsid;//gps设备编号
                string type = dy.type;
                string sql = string.Empty;
                switch (type)
                {
                    case"0":
                    case"1":
                    case"2":
                        sql = string.Format("select d.dirver,d.phone,d.deptname,'' as VisitUser, d.carno  from bis_carinfo d where d.gpsid='{0}'", GpsId);
                        break;
                   case "3":
                        sql = string.Format("select d.dirver,d.phone,comname,VisitUser,d.carno  from bis_visitcar d where d.gpsid='{0}' and d.state='3' ", GpsId);
                        break;
                    case "4":
                        sql = string.Format("select DriverName as dirver ,DriverTel as phone,Takegoodsname as comname , Supplyname as VisitUser,platenumber as carno from wl_operticketmanager d where d.examinestatus='3' and d.gpsid='{0}' ", GpsId);
                        break;
                    case "5":
                        sql = string.Format("select d.dirver,d.phone,TheCompany as comname,'' as VisitUser,d.carno  from bis_hazardouscar d where  d.state='3' and d.gpsid='{0}' ", GpsId);
                        break;
                    default:
                        break;
                }
                if (!string.IsNullOrEmpty(sql))
                {
                    DataTable dt = operticketmanagerbll.GetDataTable(sql);
                    if (dt.Rows.Count > 0) { data = dt; }
                }
                return new { code = 0, info = "获取数据成功", count = data.Rows.Count, data = data.ToJson() };
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }
        }

        /// <summary>
        /// 设备间门禁人员
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetEquipmentRoomUserList(string json)
        {
            try
            {
                // string res = json.Value<string>("json");//[FromBody]JObject
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(json);
                string userId = dy.userid;
                string username = dy.data.username;//姓名
                string jobnumber = dy.data.jobnumber;//手机号

                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    curUser = GetOperator(dy.userid);
                    if (curUser == null)
                    {
                        return new { code = -1, count = 0, info = "当前用户不存在,请核对用户信息!" };
                    }
                }
                Pagination pagination = new Pagination();
                pagination.p_kid = "id";
                pagination.p_fields = @" d.createdate,d.devicename,d.username,t.mobile, d.deptname, '进门' as state,t.userid ";
                pagination.p_tablename = "BIS_HIKINOUTLOG d join base_user t on d.userid=t.userid ";
                pagination.conditionJson = " d.inout='0' and d.isout='0' and d.devicetype='2' ";

                //获取页数和条数
                int page = Convert.ToInt32(dy.data.pagenum), rows = Convert.ToInt32(dy.data.pagesize);
                pagination.page = page;//页数
                pagination.rows = rows;//行数
                pagination.sidx = "d.createdate";//排序字段
                pagination.sord = "desc";//排序方式
                DataTable data = operticketmanagerbll.GetPageList(pagination, null);

                return new { code = 0, info = "获取数据成功", count = pagination.records, data = data.ToJson() };
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }
        }


    }
}
