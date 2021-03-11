using System.Collections.Generic;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Busines.CarManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using BSFramework.Util.Extension;
using ERCHTMS.Code;
using ERCHTMS.Entity.OccupationalHealthManage;
using ERCHTMS.Entity.SystemManage;
using System.Data;
using System;
using ERCHTMS.Busines.MatterManage;

namespace ERCHTMS.Web.Areas.CarManage.Controllers
{
    /// <summary>
    /// 描 述：车辆进出记录表
    /// </summary>
    public class CarinlogController : Controller
    {
        private CarinlogBLL carinlogbll = new CarinlogBLL();

        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult BusinessForm()
        {
            return View();
        }
        /// <summary>
        /// 统计页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Statistics()
        {
            return View();
        }

        /// <summary>
        /// 车辆轨迹
        /// </summary>
        /// <returns></returns>
        public ActionResult CarTrajectory()
        {
            return View();
        }
        /// <summary>
        /// 车辆进出厂记录
        /// </summary>
        /// <returns></returns>
        public ActionResult InandOutRecord()
        {
            return View();
        }
        /// <summary>
        /// 三维车辆定位
        /// </summary>
        /// <returns></returns>
        public ActionResult CarLocation()
        {
            string sql = string.Format("select t.itemname,t.itemvalue,t.itemcode from base_dataitem d join BASE_DATAITEMDETAIL t on d.itemid=t.itemid  where d.itemcode='KmConfigure' order by t.sortcode asc");
            DataTable dt = new OperticketmanagerBLL().GetDataTable(sql);
            if (dt.Rows.Count > 1)
            {//可门配置信息
                ViewBag.SDmanager = dt.Rows[3][1].ToString();//三维图数据包路径
                dt.Dispose();
            }
            return View();
        }

        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            string orgcode = OperatorProvider.Provider.Current().OrganizeCode;
            var watch = CommonHelper.TimerStart();
            string cid = "";
            var queryParam = queryJson.ToJObject();

            //根据车辆筛选
            if (!queryParam["CID"].IsEmpty())
            {
                cid = queryParam["CID"].ToString();

            }
            pagination.p_kid = "ID";
            pagination.p_fields = "createuserid,createdate,cid,carno,status,address,NVL(pnum,0) as pnum";
            pagination.p_tablename = @"BIS_CARINLOG log
            left join (select lid,count(lid) pnum from bis_carride where cid='" + cid + "' and STATUS=1  group by lid ) ride on ride.lid=log.id";
            pagination.conditionJson = " 1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();


            var data = carinlogbll.GetPageList(pagination, queryJson);
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
        /// 根据关联ID获取道闸数据
        /// </summary>
        /// <param name="Cid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetList(string Cid)
        {
            var data = carinlogbll.GetList(Cid);
            return Content(data.ToJson());
        }


        [HttpGet]
        public ActionResult GetStatus()
        {
            List<DataItemDetailEntity> ddlist = new List<DataItemDetailEntity>();
            DataItemDetailEntity d1 = new DataItemDetailEntity();
            d1.ItemName = "入厂";
            d1.ItemValue = "0";
            DataItemDetailEntity d2 = new DataItemDetailEntity();
            d2.ItemName = "出厂";
            d2.ItemValue = "1";
            ddlist.Add(d1);
            ddlist.Add(d2);
            return Content(ddlist.ToJson());
        }



        /// <summary>
        /// 统计车辆进出饼状图
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <returns></returns>
        [HttpGet]
        public string GetLogChart(string year = "")
        {
            return carinlogbll.GetLogChart(year);
        }

        /// <summary>
        /// 获取当前车辆轨迹
        /// </summary>
        /// <param name="STime"></param>
        /// <param name="ETime"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetTrajectory(string Cid, int type, string STime, string ETime)
        {
            string Trajectory = @"[119.768602928571, 26.3668279285714],
    [119.768527857143, 26.3668808571429],
    [119.768452785714, 26.3669337857143],
    [119.768377714286, 26.3669867142857],
    [119.768302642857, 26.3670396428571],
    [119.768227571429, 26.3670925714286],
    [119.7681525, 26.3671455],
    [119.768077428571, 26.3671984285714],
    [119.768002357143, 26.3672513571429],
    [119.767927285714, 26.3673042857143],
    [119.767852214286, 26.3673572142857],
    [119.767777142857, 26.3674101428571],
    [119.767702071429, 26.3674630714286],
    [119.767627, 26.367516],
    [119.767684829268, 26.367588902439],
    [119.767742658537, 26.367661804878],
    [119.767800487805, 26.3677347073171],
    [119.767858317073, 26.3678076097561],
    [119.767916146341, 26.3678805121951],
    [119.76797397561, 26.3679534146341],
    [119.768031804878, 26.3680263170732],
    [119.768089634146, 26.3680992195122],
    [119.768147463415, 26.3681721219512],
    [119.768205292683, 26.3682450243902],
    [119.768263121951, 26.3683179268293],
    [119.76832095122, 26.3683908292683],
    [119.768378780488, 26.3684637317073],
    [119.768436609756, 26.3685366341463],
    [119.768494439024, 26.3686095365854],
    [119.768552268293, 26.3686824390244],
    [119.768610097561, 26.3687553414634],
    [119.768667926829, 26.3688282439024],
    [119.768725756098, 26.3689011463415],
    [119.768783585366, 26.3689740487805],
    [119.768841414634, 26.3690469512195],
    [119.768899243902, 26.3691198536585],
    [119.768957073171, 26.3691927560976],
    [119.769014902439, 26.3692656585366],
    [119.769072731707, 26.3693385609756],
    [119.769130560976, 26.3694114634146],
    [119.769188390244, 26.3694843658537],
    [119.769246219512, 26.3695572682927],
    [119.76930404878, 26.3696301707317],
    [119.769361878049, 26.3697030731707],
    [119.769419707317, 26.3697759756098],
    [119.769477536585, 26.3698488780488],
    [119.769535365854, 26.3699217804878],
    [119.769593195122, 26.3699946829268],
    [119.76965102439, 26.3700675853659],
    [119.769708853659, 26.3701404878049],
    [119.769766682927, 26.3702133902439],
    [119.769824512195, 26.3702862926829],
    [119.769882341463, 26.370359195122],
    [119.769940170732, 26.370432097561],
    [119.769998, 26.370505],
    [119.769958666667, 26.3705563333333],
    [119.769919333333, 26.3706076666667],
    [119.76988, 26.370659],
    [119.769933142857, 26.3707308571429],
    [119.769986285714, 26.3708027142857],
    [119.770039428571, 26.3708745714286],
    [119.770092571429, 26.3709464285714],
    [119.770145714286, 26.3710182857143],
    [119.770198857143, 26.3710901428571],
    [119.770252, 26.371162],
    [119.770305142857, 26.3712338571429],
    [119.770358285714, 26.3713057142857],
    [119.770411428571, 26.3713775714286],
    [119.770464571429, 26.3714494285714],
    [119.770517714286, 26.3715212857143],
    [119.770570857143, 26.3715931428571],
    [119.770624, 26.371665],
    [119.770677142857, 26.3717368571429],
    [119.770730285714, 26.3718087142857],
    [119.770783428571, 26.3718805714286],
    [119.770836571429, 26.3719524285714],
    [119.770889714286, 26.3720242857143],
    [119.770942857143, 26.3720961428571],
    [119.770996, 26.372168],
    [119.7709635, 26.372221],
    [119.770931, 26.372274],
    [119.770850782609, 26.3723266521739],
    [119.770770565217, 26.3723793043478],
    [119.770690347826, 26.3724319565217],
    [119.770610130435, 26.3724846086957],
    [119.770529913043, 26.3725372608696],
    [119.770449695652, 26.3725899130435],
    [119.770369478261, 26.3726425652174],
    [119.77028926087, 26.3726952173913],
    [119.770209043478, 26.3727478695652],
    [119.770128826087, 26.3728005217391],
    [119.770048608696, 26.372853173913],
    [119.769968391304, 26.372905826087],
    [119.769888173913, 26.3729584782609],
    [119.769807956522, 26.3730111304348],
    [119.76972773913, 26.3730637826087],
    [119.769647521739, 26.3731164347826],
    [119.769567304348, 26.3731690869565],
    [119.769487086957, 26.3732217391304],
    [119.769406869565, 26.3732743913043],
    [119.769326652174, 26.3733270434783],
    [119.769246434783, 26.3733796956522],
    [119.769166217391, 26.3734323478261],
    [119.769086, 26.373485],
    [119.769026285714, 26.3734191428571],
    [119.768966571429, 26.3733532857143],
    [119.768906857143, 26.3732874285714],
    [119.768847142857, 26.3732215714286],
    [119.768787428571, 26.3731557142857],
    [119.768727714286, 26.3730898571429],
    [119.768668, 26.373024],
    [119.7685864, 26.3730739],
    [119.7685048, 26.3731238],
    [119.7684232, 26.3731737],
    [119.7683416, 26.3732236],
    [119.76826, 26.3732735],
    [119.7681784, 26.3733234],
    [119.7680968, 26.3733733],
    [119.7680152, 26.3734232],
    [119.7679336, 26.3734731],
    [119.767852, 26.373523],
    [119.767760222222, 26.3735081111111],
    [119.767668444444, 26.3734932222222],
    [119.767576666667, 26.3734783333333],
    [119.767484888889, 26.3734634444444],
    [119.767393111111, 26.3734485555556],
    [119.767301333333, 26.3734336666667],
    [119.767209555556, 26.3734187777778],
    [119.767117777778, 26.3734038888889],
    [119.767026, 26.373389],
    [119.766989, 26.3733142222222],
    [119.766952, 26.3732394444444],
    [119.766915, 26.3731646666667],
    [119.766878, 26.3730898888889],
    [119.766841, 26.3730151111111],
    [119.766804, 26.3729403333333],
    [119.766767, 26.3728655555556],
    [119.76673, 26.3727907777778],
    [119.766693, 26.372716],
    [119.7666233, 26.3727698],
    [119.7665536, 26.3728236],
    [119.7664839, 26.3728774],
    [119.7664142, 26.3729312],
    [119.7663445, 26.372985],
    [119.7662748, 26.3730388],
    [119.7662051, 26.3730926],
    [119.7661354, 26.3731464],
    [119.7660657, 26.3732002],
    [119.765996, 26.373254],
    [119.765913, 26.37325875],
    [119.76583, 26.3732635],
    [119.765747, 26.37326825],
    [119.765664, 26.373273],
    [119.7655996, 26.37320612],
    [119.7655352, 26.37313924],
    [119.7654708, 26.37307236],
    [119.7654064, 26.37300548],
    [119.765342, 26.3729386],
    [119.7652776, 26.37287172],
    [119.7652132, 26.37280484],
    [119.7651488, 26.37273796],
    [119.7650844, 26.37267108],
    [119.76502, 26.3726042],
    [119.7649556, 26.37253732],
    [119.7648912, 26.37247044],
    [119.7648268, 26.37240356],
    [119.7647624, 26.37233668],
    [119.764698, 26.3722698],
    [119.7646336, 26.37220292],
    [119.7645692, 26.37213604],
    [119.7645048, 26.37206916],
    [119.7644404, 26.37200228],
    [119.764376, 26.3719354],
    [119.7643116, 26.37186852],
    [119.7642472, 26.37180164],
    [119.7641828, 26.37173476],
    [119.7641184, 26.37166788],
    [119.764054, 26.371601],
    [119.7639915, 26.3715401666667],
    [119.763929, 26.3714793333333],
    [119.7638665, 26.3714185],
    [119.763804, 26.3713576666667],
    [119.7637415, 26.3712968333333],
    [119.763679, 26.371236],
    [119.7636575, 26.37116375],
    [119.763636, 26.3710915],
    [119.7636145, 26.37101925],
    [119.763593, 26.370947],
    [119.763621846154, 26.3708671538462],
    [119.763650692308, 26.3707873076923],
    [119.763679538462, 26.3707074615385],
    [119.763708384615, 26.3706276153846],
    [119.763737230769, 26.3705477692308],
    [119.763766076923, 26.3704679230769],
    [119.763794923077, 26.3703880769231],
    [119.763823769231, 26.3703082307692],
    [119.763852615385, 26.3702283846154],
    [119.763881461538, 26.3701485384615],
    [119.763910307692, 26.3700686923077],
    [119.763939153846, 26.3699888461538],
    [119.763968, 26.369909],
    [119.764048555556, 26.3698556031746],
    [119.764129111111, 26.3698022063492],
    [119.764209666667, 26.3697488095238],
    [119.764290222222, 26.3696954126984],
    [119.764370777778, 26.369642015873],
    [119.764451333333, 26.3695886190476],
    [119.764531888889, 26.3695352222222],
    [119.764612444444, 26.3694818253968],
    [119.764693, 26.3694284285714],
    [119.764773555556, 26.369375031746],
    [119.764854111111, 26.3693216349206],
    [119.764934666667, 26.3692682380952],
    [119.765015222222, 26.3692148412698],
    [119.765095777778, 26.3691614444444],
    [119.765176333333, 26.369108047619],
    [119.765256888889, 26.3690546507937],
    [119.765337444444, 26.3690012539683],
    [119.765418, 26.3689478571429],
    [119.765498555556, 26.3688944603175],
    [119.765579111111, 26.3688410634921],
    [119.765659666667, 26.3687876666667],
    [119.765740222222, 26.3687342698413],
    [119.765820777778, 26.3686808730159],
    [119.765901333333, 26.3686274761905],
    [119.765981888889, 26.3685740793651],
    [119.766062444444, 26.3685206825397],
    [119.766143, 26.3684672857143],
    [119.766223555556, 26.3684138888889],
    [119.766304111111, 26.3683604920635],
    [119.766384666667, 26.3683070952381],
    [119.766465222222, 26.3682536984127],
    [119.766545777778, 26.3682003015873],
    [119.766626333333, 26.3681469047619],
    [119.766706888889, 26.3680935079365],
    [119.766787444444, 26.3680401111111],
    [119.766868, 26.3679867142857],
    [119.766948555556, 26.3679333174603],
    [119.767029111111, 26.3678799206349],
    [119.767109666667, 26.3678265238095],
    [119.767190222222, 26.3677731269841],
    [119.767270777778, 26.3677197301587],
    [119.767351333333, 26.3676663333333],
    [119.767431888889, 26.3676129365079],
    [119.767512444444, 26.3675595396825],
    [119.767593, 26.3675061428571],
    [119.767673555556, 26.3674527460317],
    [119.767754111111, 26.3673993492063],
    [119.767834666667, 26.367345952381],
    [119.767915222222, 26.3672925555556],
    [119.767995777778, 26.3672391587302],
    [119.768076333333, 26.3671857619048],
    [119.768156888889, 26.3671323650794],
    [119.768237444444, 26.367078968254],
    [119.768318, 26.3670255714286],
    [119.768398555556, 26.3669721746032],
    [119.768479111111, 26.3669187777778],
    [119.768559666667, 26.3668653809524],
    [119.768640222222, 26.366811984127],
    [119.768720777778, 26.3667585873016],
    [119.768801333333, 26.3667051904762],
    [119.768881888889, 26.3666517936508],
    [119.768962444444, 26.3665983968254],
    [119.769043, 26.366545]";
            return Trajectory;
        }

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetTableChar(string year = "")
        {
            var JsonData = carinlogbll.GetTableChar(year);
            return Content(JsonData.ToJson());
        }

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetLogDetail(string CarType, string Status, string year = "")
        {
            var JsonData = carinlogbll.GetLogDetail(year, CarType, Status);
            return Content(JsonData.ToJson());
        }


        ///// <summary>
        ///// 获取实体 
        ///// </summary>
        ///// <param name="keyValue">主键值</param>
        ///// <returns>返回对象Json</returns>
        //[HttpGet]
        //public ActionResult GetFormJson(string keyValue)
        //{
        //    var data = carinlogbll.GetEntity(keyValue);
        //    return ToJsonResult(data);
        //}


        /// <summary>
        /// 获取车辆进出列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetIntandOutListJson(Pagination pagination, string queryJson)
        {
            string orgcode = OperatorProvider.Provider.Current().OrganizeCode;
            var watch = CommonHelper.TimerStart();
            var queryParam = queryJson.ToJObject();

            pagination.p_kid = "ID";
            pagination.p_fields = "carno,type,drivername,phone,address,status,createdate,cid,createuserid";
            pagination.p_tablename = @"bis_carinlog log ";
            pagination.conditionJson = " 1=1";

            var data = carinlogbll.GetPageList(pagination, queryJson);
            Session["CarIntandOutRecord"] = data;
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

        ///// <summary>
        ///// 导出到Excel
        ///// </summary>
        ///// <param name="queryJson"></param>
        ///// <returns></returns>
        [HandlerMonitor(0, "导出数据")]
        public ActionResult Excel(string type)
        {
            try
            {
                string[] column = { "createdate" };
                string[] column1 = { "type", "status" };
                var dt = Session["CarIntandOutRecord"] as DataTable;
                dt.TableName = "Table";
                dt = AsposeExcelHelper.UpdateDataTable(dt, column, "datetime");
                DataTable newdt = AsposeExcelHelper.UpdateDataTable(dt, column1, "string");
                for (int i = 0; i < newdt.Rows.Count; i++)
                {
                    string res = newdt.Rows[i]["status"].ToString();
                    if (res == "0") res = "进";
                    else
                        res = "出";

                    newdt.Rows[i]["status"] = res;
                    int num = newdt.Rows[i]["type"].ToInt();
                    switch (num)
                    {
                        case 0:
                            newdt.Rows[i]["type"] = "电厂班车";
                            break;
                        case 1:
                            newdt.Rows[i]["type"] = "私家车";
                            break;
                        case 2:
                            newdt.Rows[i]["type"] = "商务公车";
                            break;
                        case 3:
                            dt.Rows[i]["type"] = "拜访车辆";
                            break;
                        case 4:
                            newdt.Rows[i]["type"] = "物料车辆";
                            break;
                        case 5:
                            newdt.Rows[i]["type"] = "危化品车辆";
                            break;
                        case 6:
                            newdt.Rows[i]["type"] = "临时通行车辆";
                            break;
                    }
                }
                //模板路径
                string FileUrl = @"\Resource\ExcelTemplate\车辆进出厂记录_导出模板.xlsx";
                AsposeExcelHelper.ExecuteResult(newdt, FileUrl, "车辆进出厂记录列表", "车辆进出厂记录列表");
            }
            catch (Exception)
            {
                throw;
            }
            return Content("导出成功。");

        }

        #endregion

      
    }
}
