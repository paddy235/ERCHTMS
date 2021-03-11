using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.KbsDeviceManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BSFramework.Util.Extension;
using BSFramework.Util;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.MatterManage;
using ERCHTMS.Busines.SystemManage;
using System.Data;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Entity.KbsDeviceManage;

namespace ERCHTMS.Web.Areas.KbsDeviceManage.Controllers
{
    /// <summary>
    /// 描述：人员行为安全管控 
    /// </summary>
    public class SafeUserControlController : MvcControllerBase
    {
        private SafeworkcontrolBLL safeworkcontrolbll = new SafeworkcontrolBLL();
        private OperticketmanagerBLL Opertickebll = new OperticketmanagerBLL();

        #region 视图功能
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 人员行为安全管控首页
        /// </summary>
        /// <returns></returns>
        public ActionResult UserHome() {
            return View();
        }

        #endregion

        #region 获取数据
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "a.ID";
            pagination.p_fields = "f.fullname as deptname,nature,t.organizeid,t.departmentcode,a.LiableName,a.warningcontent,a.warningtime,a.baseid  ";
            pagination.p_tablename = @" bis_earlywarning a left join base_user t on a.liableid=t.userid join base_department f on t.departmentid=f.departmentid  ";
            pagination.conditionJson = " 1=1 ";

            var queryParam = queryJson.ToJObject();
            if (!queryParam["type"].IsEmpty())
            {//预警类型 0现场作业 1人员
                string Type = queryParam["type"].ToString();
                pagination.conditionJson += string.Format(" and a.type ={0} ", Type);
            }
            var data = safeworkcontrolbll.GetWaringPageList(pagination, queryJson);
            foreach (DataRow dr in data.Rows)
            {
                if (dr["nature"].ToString() == "专业" || dr["nature"].ToString() == "班组")
                {
                    DataTable dt = new DepartmentBLL().GetDataTable(string.Format("select fullname from base_department where encode=(select encode from base_department t where instr('{0}',encode)=1 and nature='{1}' and organizeid='{2}') or encode='{0}' order by deptcode", dr["DEPARTMENTCODE"], "部门", dr["organizeid"]));
                    if (dt.Rows.Count > 0)
                    {
                        string name = "";
                        foreach (DataRow dr1 in dt.Rows)
                        {
                            name += dr1["fullname"].ToString() + "/";
                        }
                        dr["deptname"] = name.TrimEnd('/');
                    }
                }
            }
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
        /// 获取外包人员
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetOutsourceUserList()
        {
            string sql = string.Empty;
            DataTable data = new DataTable();
            DataItemDetailBLL pdata = new DataItemDetailBLL();
            var list = pdata.GetDataItemListByItemCode("KbshomeCount");
            if (list != null)
            {
                var item = list.Where(a => a.EnabledMark == 1).FirstOrDefault();
                if (item.ItemValue == "EquipmentManage")
                { //关联设备进出记录
                    sql = string.Format(@"select d.deptname,count(1) as num from bis_hikinoutlog d  join base_department t on d.deptid=t.departmentid and t.nature='承包商' and d.isout=0 group  by d.deptid,d.deptname");
                }
                else if (item.ItemValue == "LableManage")
                {//关联标签
                    sql = string.Format(@"select d.deptname,count(1) as num from bis_lableonlinelog d join  base_department t on d.deptid=t.departmentid and t.nature='承包商' and d.isout=0 group  by d.deptid,d.deptname");
                }
            }
            if (!string.IsNullOrEmpty(sql))
            {
                data = Opertickebll.GetDataTable(sql);
            }
            return Content(data.ToJson());
        }

        /// <summary>
        /// 人员实时预警
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetUserRealtimeWarning(int type)
        {
            var list = safeworkcontrolbll.GetWarningInfoList(type).Where(a => a.State == 0).OrderByDescending(it => it.CREATEDATE).ToList();
            return Content(list.ToJson());
        }

        /// <summary>
        /// 人员类别在线统计图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetUserOnlineGroupJson()
        {
            string sql = string.Format("select t.labletypename,t.labletypeid,count(1)as num from bis_lableonlinelog d join bis_lablemanage t on d.lableid=t.lableid where d.isout=0 and t.type=0 and t.state='在线' group by t.labletypename,t.labletypeid ");
            var dt = Opertickebll.GetDataTable(sql);
            DataItemBLL di = new DataItemBLL();
            //先获取到字典项
            DataItemEntity DataItems = di.GetEntityByCode("LabelType");
            DataItemDetailBLL did = new DataItemDetailBLL();
            //根据字典项获取值
            List<DataItemDetailEntity> didList = did.GetList(DataItems.ItemId).Where(a => a.ItemName != "厂外车辆" && a.ItemName != "厂内车辆").ToList();
            List<KbsEntity> klist = new List<KbsEntity>();
            int Znum = 0;
            foreach (var item in didList)
            {
                KbsEntity kbs = new KbsEntity();
                kbs.Name = item.ItemName;
                int num = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["labletypeid"].ToString() == item.ItemValue)
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
                klist[j].Num2 = Znum;
                klist[j].Proportion = Proportion.ToString("0.00") + "%";
            }
            return Content(klist.ToJson());
        }

        /// <summary>
        /// 统计图形显示信息
        /// </summary>
        /// <returns></returns>
        public string GetLableChart()
        {
            List<object[]> list = new List<object[]>();
            string sql = string.Format("select t.labletypename,t.labletypeid,count(1)as num from bis_lableonlinelog d join bis_lablemanage t on d.lableid=t.lableid where d.isout=0 and t.type=0 and t.state='在线' group by t.labletypename,t.labletypeid ");
            DataTable dt = Opertickebll.GetDataTable(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                object[] arr = { dt.Rows[i][0].ToString(), Convert.ToInt32(dt.Rows[i][2]) };
                list.Add(arr);
            }
            dt.Dispose();
            return Newtonsoft.Json.JsonConvert.SerializeObject(list);
        }

        /// <summary>
        /// 当天各个时段人数统计
        /// </summary>
        /// <returns></returns>
        public string GetDayTimeIntervalUserNum()
        {
            List<KbsEntity> inlist = safeworkcontrolbll.GetDayTimeIntervalUserNum();
            List<int> list = new List<int>();
            int[] intime = { 0, 2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22 };
            for (int i = 0; i < intime.Length; i++)
            {
                int innum = intime[i];
                int num = 0;
                if (i == 0)
                {
                    num = inlist.Where(a => a.Num2 == 23 || a.Num2 == 0).Sum(b => b.Num);
                }
                else
                {
                    num = inlist.Where(a => a.Num2 >= (innum - 1) && a.Num2 <= innum).Sum(b => b.Num);
                }
                list.Add(num);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(list);
        }

        #endregion

    }
}