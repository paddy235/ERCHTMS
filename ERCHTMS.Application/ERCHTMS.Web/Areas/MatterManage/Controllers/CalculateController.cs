using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Web.Mvc;
using BSFramework.Cache.Factory;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.Offices;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.MatterManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Entity.MatterManage;
using Newtonsoft.Json;

namespace ERCHTMS.Web.Areas.MatterManage.Controllers
{
    /// <summary>
    /// 描 述：计量管理
    /// </summary>
    public class CalculateController : MvcControllerBase
    {
        private CalculateBLL calculatebll = new CalculateBLL();
        private OperticketmanagerBLL Opertickebll = new OperticketmanagerBLL();
        private UserBLL userBLL = new UserBLL();
        private DataItemBLL dataItemBLL = new DataItemBLL();
        private DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();
        private string ipA = string.Empty;
        private string ipB = string.Empty;
        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.IPAddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            ipA = CacheFactory.Cache().GetCache<string>("PoundA:IP");
            ipB = CacheFactory.Cache().GetCache<string>("PoundB:IP");
            if (string.IsNullOrEmpty(ipA))
            {
                ipA = dataItemDetailBLL.GetItemValue("PoundA:IP");//二号地磅室IP
                CacheFactory.Cache().WriteCache<string>(ipA, "PoundA:IP");

            }
            if (string.IsNullOrEmpty(ipB))
            {
                ipB = dataItemDetailBLL.GetItemValue("PoundB:IP");//一号地磅室IP
                CacheFactory.Cache().WriteCache<string>(ipB, "PoundB:IP");
            }
            ViewBag.PoundAIP = ipA;
            ViewBag.PoundBIP = ipB;
            return View();
        }


        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult addForm()
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
        /// 地磅员授权列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UserList()
        {
            return View();
        }
        /// <summary>
        /// 地磅员授权详情
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UserForm()
        {
            return View();
        }

        /// <summary>
        /// 计量记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Record()
        {
            return View();
        }

        /// <summary>
        /// 计量记录详情
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RecordForm()
        {
            return View();
        }


        /// <summary>
        /// 记录统计
        /// </summary>
        /// <returns></returns>
        public ActionResult CalculateCount()
        {
            return View();
        }

        /// <summary>
        /// 打印视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Stamp()
        {
            return View();
        }

        /// <summary>
        /// 子表记录打印
        /// </summary>
        /// <returns></returns>
        public ActionResult NewStamp()
        {
            return View();
        }

        /// <summary>
        /// 托运记录详情
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowForm()
        {
            return View();
        }

        /// <summary>
        /// 物料统计明细查看
        /// </summary>
        /// <returns></returns>
        public ActionResult CoountDetailForm()
        {
            return View();
        }

        /// <summary>
        /// 物供部开票信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Orders()
        {
            return View();
        }

        /// <summary>
        /// 未出厂物料开票单
        /// </summary>
        /// <returns></returns>
        public ActionResult TicketSelect()
        {
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
        public ActionResult GetListJson(string queryJson)
        {
            var data = calculatebll.GetList(queryJson);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetNewPageList(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "Id";
            pagination.p_fields = "ca.id,createdate,Numbers,goodsName,rough,tare,netwneight,TakeGoodsName,TransportType,BaseId, PlateNumber,roughTime,Remark,roughUserName,tareUserName,stampTime,taretime,isout";
            pagination.p_tablename = "WL_CALCULATE ca ";
            pagination.conditionJson = " Isdelete='1' ";
            var watch = CommonHelper.TimerStart();
            var data = calculatebll.GetNewPageList(pagination, queryJson);
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };

            return Content(JsonData.ToJson());
        }

        /// <summary>
        /// 获取对应子表记录列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetDetailedPageList(string BaseId)
        {
            Pagination pagination = new Pagination();
            pagination.p_kid = "Id";
            pagination.p_fields = "id,createdate,Numbers,goodsName,rough,tare,netwneight,TakeGoodsName,TransportType,PlateNumber,roughTime,Remark,roughUserName,tareUserName,stampTime,taretime,isout";
            pagination.p_tablename = "WL_CALCULATE";
            pagination.conditionJson = "1=1 and Isdelete='1' and BaseId='" + BaseId + "' ";
            var watch = CommonHelper.TimerStart();
            //var data = calculatebll.GetNewPageList(pagination,null);
            string sql = string.Format("select  {0} from {1} where {2}", pagination.p_fields, pagination.p_tablename, pagination.conditionJson += "  order by createdate desc");
            var data = Opertickebll.GetDataTable(sql);
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return Content(JsonData.ToJson());
        }


        /// <summary>
        /// 计量记录列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetRecordPageList(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "Id";
            pagination.p_fields = "createdate,Numbers,goodsName,rough,tare,netwneight,TakeGoodsName,TransportType,PlateNumber,roughTime,Remark,roughUserName,tareUserName,stampTime,taretime,isdelete,deletecontent";
            pagination.p_tablename = "WL_CALCULATE";
            pagination.conditionJson = "1=1 ";
            var watch = CommonHelper.TimerStart();
            var data = calculatebll.GetPageList(pagination, queryJson);
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return Content(JsonData.ToJson());
        }


        /// <summary>
        /// 获取计量统计列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetCountPageList(Pagination pagination, string queryJson)
        {

            string strwhere = "";
            string stime = string.Empty; string etime = string.Empty;
            var queryParam = queryJson.ToJObject();
            if (!queryParam["keyword"].IsEmpty())
            {//车牌号
                string PlateNumber = queryParam["keyword"].ToString().Trim();
                strwhere += string.Format(" and PlateNumber like '%{0}%'", PlateNumber);
            }
            if (!queryParam["Takegoodsname"].IsEmpty() && queryParam["Takegoodsname"].ToString().Trim() != "全部")
            {//提货方
                string Transporttype = queryParam["Takegoodsname"].ToString().Trim();
                strwhere += string.Format(" and Takegoodsname='{0}'", Transporttype);
            }
            if (!queryParam["Transporttype"].IsEmpty() && queryParam["Transporttype"].ToString().Trim() != "全部")
            {//运输类型
                string Transporttype = queryParam["Transporttype"].ToString().Trim();
                strwhere += string.Format(" and Transporttype like '%{0}%'", Transporttype);
            }
            if (!queryParam["Goodsname"].IsEmpty() && queryParam["Goodsname"].ToString().Trim() != "全部")
            {//副产品类型
                string Goodsname = queryParam["Goodsname"].ToString().Trim();
                strwhere += string.Format(" and Goodsname like '%{0}%'", Goodsname);
            }
            if (!queryParam["Stime"].IsEmpty() && !queryParam["Etime"].IsEmpty())
            {//打印时间起
                stime = queryParam["Stime"].ToString().Trim();
                strwhere += string.Format(" and roughtime >=  to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') ", Convert.ToDateTime(stime));
                //打印时间止
                etime = queryParam["Etime"].ToString().Trim();
                DateTime dst = Convert.ToDateTime(etime).AddDays(1);
                strwhere += string.Format(" and roughtime <= to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') ", dst);
            }

            pagination.p_kid = "takegoodsname";
            pagination.p_fields = " transporttype,goodsname,netwneight,vehicleCount,roughtime ";
            pagination.p_tablename = @" ( select takegoodsname,transporttype,goodsname,sum(netwneight) as netwneight, count(1) as vehicleCount,to_char(d.roughtime,'yyyy-MM-dd') as roughtime from wl_calculate d
            where 1 = 1 and Isdelete = '1'" + strwhere + " group by to_char(d.roughtime,'yyyy-MM-dd'),d.takegoodsname, d.transporttype, d.goodsname ) ";
            pagination.conditionJson = "1=1 ";
            var watch = CommonHelper.TimerStart();
            var data = calculatebll.GetCountPageList(pagination, queryJson);
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return Content(JsonData.ToJson());
        }

        /// <summary>
        /// 物料统计明细查看
        /// </summary>
        /// <param name="queryJson">数据过滤筛选参数</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCointDetailList(string queryJson)
        {
            // Pagination pagination = new Pagination();
            string strwhere = " 1=1 and t.isdelete='1' ";
            string stime = string.Empty; string etime = string.Empty;
            var queryParam = queryJson.ToJObject();
            if (!queryParam["Takegoodsname"].IsEmpty() )
            {//提货方
                string Transporttype = queryParam["Takegoodsname"].ToString().Trim();
                strwhere += string.Format(" and t.Takegoodsname='{0}'", Transporttype);
            }
            if (!queryParam["Transporttype"].IsEmpty())
            {//运输类型
                string Transporttype = queryParam["Transporttype"].ToString().Trim();
                strwhere += string.Format(" and t.Transporttype='{0}'", Transporttype);
            }
            if (!queryParam["Goodsname"].IsEmpty() )
            {//副产品类型
                string Goodsname = queryParam["Goodsname"].ToString().Trim();
                strwhere += string.Format(" and t.Goodsname='{0}'", Goodsname);
            }
            if (!queryParam["RoughDate"].IsEmpty() )
            {//打印时间起
                string RoughDate = queryParam["RoughDate"].ToString().Trim();
                strwhere += string.Format(" and to_char(t.roughtime,'yyyy-MM-dd')='{0}' ", RoughDate);
                           }
            else
            {//默认本月                 
                strwhere += string.Format(" and to_char(t.roughtime,'yyyy-MM-dd')='{0}' ", DateTime.Now.ToString("yyyy-MM-dd"));
            }

            string sql = string.Format(@"select to_char(d.Getdata,'MM-dd') as rctime,to_char(d.Getdata,'hh24:mi') as rctime1，
                                          to_char(t.taretime,' hh24:mi') as BalanceTime,
                                          to_char(t.roughtime,'MM-dd') as mztime,to_char(t.roughtime,' hh24:mi') as mztime1,
                                          to_char(d.outdate,' hh24:mi') as outdate,
                                          d.Takegoodsname,d.platenumber,d.Transporttype,d.Numbers as Numbers1 ,t.numbers,t.tare,t.tareusername,
                                          t.rough,t.roughusername,t.netwneight,d.Opername,d.LetMan,d.status,d.PassRemark,
                                          d.Getdata,d.BalanceTime as balancetime1,d.outdate as outdate1
                                          from wl_operticketmanager d
                                          join wl_calculate t on d.id = t.baseid where {0}	
	                                        union 
                                          select to_char(f.createdate,'MM-dd') as rctime,null as rctime1，
                                          to_char(t.taretime,' hh24:mi') as BalanceTime,
                                          to_char(t.roughtime,'MM-dd') as mztime,to_char(t.roughtime,' hh24:mi') as mztime1,
                                          null as outdate,
                                          f.Takegoodsname,f.platenumber, null as Transporttype,f.Numbers as Numbers1 ,f.numbers,t.tare,t.tareusername,
                                          t.rough,t.roughusername,t.netwneight,null Opername, null LetMan,null status,f.remark as PassRemark,
                                          null as Getdata,null balancetime1,null as outdate1
                                          from wl_calculatedetailed f
	                                      join  wl_calculate t on f.id = t.baseid where {0}", strwhere);

            DataTable data = Opertickebll.GetDataTable(sql);
            data.Columns.Add("num1", typeof(double));
            data.Columns.Add("num2", typeof(double));
            data.Columns.Add("num3", typeof(double));
            foreach (DataRow Rows in data.Rows)
            {
                Rows["num1"] = GetTimeDouble(Rows["balancetime1"].ToString(), Rows["Getdata"].ToString());
                Rows["num2"] = GetTimeDouble(Rows["outdate1"].ToString(), Rows["balancetime1"].ToString());
                Rows["num3"] = GetTimeDouble(Rows["outdate1"].ToString(), Rows["Getdata"].ToString());
            }

            return Content(data.ToJson());
        }


        /// <summary>
        /// 获取地磅室开票信息
        /// </summary>                       
        /// <param name="pagination">分页筛选参数</param>
        /// <param name="queryJson">数据过滤筛选参数</param>
        /// <returns></returns>
        public ActionResult GetPoundOrderList(Pagination pagination, string queryJson)
        {

            var watch = CommonHelper.TimerStart();
            var data = calculatebll.GetPoundOrderList(pagination, queryJson);
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return Content(JsonData.ToJson());
        }

        #region 数据导出
        /// <summary>
        /// 导出物料统计列表
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "导出物料称重数据")]
        public ActionResult ExportCountList(string queryJson)
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.sidx = "roughTime";
            pagination.sord = "desc";
            pagination.rows = 100000000;
            pagination.p_kid = "Id";
            pagination.p_fields = "Numbers,TakeGoodsName,PlateNumber,goodsName,rough,tare,netwneight,to_char(roughTime,'yyyy-MM-dd HH24:mi') roughTime,roughUserName,to_char(taretime,'yyyy-MM-dd HH24:mi') taretime,tareUserName,to_char(stampTime,'yyyy-MM-dd HH24:mi') stampTime";
            pagination.p_tablename = "WL_CALCULATE";
            pagination.conditionJson = "1=1 ";
            var data = calculatebll.GetPageList(pagination, queryJson);

            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "物料称重计量";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "物料称重计量.xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity = listColumnEntity;
            ColumnEntity columnentity = new ColumnEntity();
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "numbers", ExcelColumn = "提货/转运单号", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "takegoodsname", ExcelColumn = "运货单位", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "platenumber", ExcelColumn = "车牌号", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "goodsname", ExcelColumn = "货名", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "rough", ExcelColumn = "毛重", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "tare", ExcelColumn = "皮重", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "netwneight", ExcelColumn = "净重", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "roughtime", ExcelColumn = "毛重时间", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "roughusername", ExcelColumn = "毛重司磅员", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "taretime", ExcelColumn = "皮重时间", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "tareusername", ExcelColumn = "皮重司磅员", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "stamptime", ExcelColumn = "称重单打印时间", Alignment = "center" });
            //调用导出方法
            // ExcelHelper.ExcelDownload(data, excelconfig);

            ExcelHelper.ExportByAspose(data, "物料称重计量", excelconfig.ColumnEntity);

            return Success("导出成功。");
        }

        /// <summary>
        /// 两个时间之间差（分钟）
        /// </summary>
        /// <param name="stime"></param>
        /// <param name="etime"></param>
        /// <returns></returns>
        public double GetTimeDouble(string stime, string etime)
        {
            double tnumber = 0;
            try
            {
                if (stime != null && etime != null)
                {
                    System.TimeSpan t1 = DateTime.Parse(stime.ToString()) - DateTime.Parse(etime.ToString());
                    tnumber = Math.Truncate(t1.TotalMinutes);
                }
            }
            catch (Exception)
            {
                tnumber = 0;
            }
            return tnumber;
        }

        #endregion


        /// <summary>
        /// 获取地磅员列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetPageUserList(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "USERID";
            pagination.p_fields = "d.createdate,t.starttime,t.endtime,t.status,d.account,d.realname,d.gender,d.mobile,d.ispresence";
            pagination.p_tablename = " V_USERINFO d left join(select id,createdate,endtime,userids,starttime,status  from (select id,status,starttime,createdate,endtime,userid as userids,row_number() over(PARTITION BY userid ORDER BY createdate desc) as row_flg from wl_userempowerRecord t) temp where row_flg=1) t on t.userids= d.userid ";
            pagination.conditionJson = "d.Account!='System' and d.ispresence='是'";
            var watch = CommonHelper.TimerStart();
            var data = calculatebll.GetPageUserList(pagination, queryJson, Getrolename());
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return Content(JsonData.ToJson());
        }

        /// <summary>
        /// 获取地磅员角色
        /// </summary>
        /// <returns></returns>
        public string Getrolename()
        {
            string res = string.Format("{0}", "地磅员角色");
            //var entity = dataItemBLL.GetEntityByCode("BalanceManage");
            //IEnumerable<DataItemDetailEntity> list = dataItemDetailBLL.GetList(entity.ItemId);
            //foreach (var item in list)
            //{
            //    if (string.IsNullOrEmpty(item.ItemValue)) continue;
            //    res += string.Format("'{0}',", item.ItemValue);
            //}
            //if (!string.IsNullOrEmpty(res)) res = res.TrimEnd(',');
            return res;
        }


        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = calculatebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 是否只有一次称重信息 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetOnlyFormJson(string keyValue)
        {
            var data = calculatebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }


        /// <summary>
        /// 获取子表记录实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetDetailedFormJson(string keyValue)
        {
            var data = calculatebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取最新一条记录实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetNewFormJson(string keyValue)
        {
            var data = calculatebll.GetNewEntity("");
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取授权记录实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetUserFormJson(string keyValue)
        {
            UserEmpowerRecordEntity entity = calculatebll.GetUserRecord(keyValue);
            if (entity != null)
            {//返回授权记录表信息
                return ToJsonResult(entity);
            }
            else
            {//返回用户表信息
                var data = userBLL.GetEntity(keyValue);
                entity = new UserEmpowerRecordEntity();
                if (data != null)
                {
                    entity.UserId = data.UserId;
                    entity.RealName = data.RealName;
                    entity.Account = data.Account;
                    entity.CreateUserName = OperatorProvider.Provider.Current().UserName;
                }
                return ToJsonResult(entity);
            }
        }

        /// <summary>
        /// 当前用户是否在授权时间范围内
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CurrentUserIsEmpowerTime()
        {
            int Nuber = 0;
            Operator user = OperatorProvider.Provider.Current();
            UserEmpowerRecordEntity entity = calculatebll.GetUserRecord(user.UserId);
            if (entity != null)
            {
                DateTime time = Convert.ToDateTime(entity.EndTime);
                if (entity.Status == "1" && time > DateTime.Now)
                {
                    Nuber = 1;
                }
            }
            else if (user.IsSystem)
            {
                Nuber = 1;
            }
            return ToJsonResult(Nuber);
        }



        /// <summary>
        /// 获取车辆类型树结构
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public ActionResult GetCarTypeTreeJson(string json)
        {
            var treeList = new List<TreeEntity>();
            var item = dataItemDetailBLL.GetDataItemListByItemCode("CarType");
            foreach (var Rows in item)
            {
                TreeEntity tree = new TreeEntity();
                tree.id = Guid.NewGuid().ToString();
                tree.text = Rows.ItemName;
                tree.value = Rows.ItemValue;
                tree.isexpand = false;
                tree.complete = true;
                tree.checkstate = 0;
                tree.showcheck = false;
                tree.hasChildren = false;
                tree.parentId = "0";
                tree.Attribute = "Code";
                treeList.Add(tree);
            }

            return Content(treeList.TreeToJson("0"));
        }

        /// <summary>
        /// 获取临时组树结构
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public ActionResult GetGroupTreeJson(string json)
        {
            var treeList = new List<TreeEntity>();
            //var item = dataItemDetailBLL.GetDataItemListByItemCode("CarType");
            string sql = string.Format("select d.id,d.groupsname from BIS_TEMPORARYGROUPS d order by createdate desc ");
            var dt = Opertickebll.GetDataTable(sql);
            foreach (DataRow Rows in dt.Rows)
            {
                TreeEntity tree = new TreeEntity();
                tree.id = Guid.NewGuid().ToString();
                tree.text = Rows[1].ToString();
                tree.value = Rows[0].ToString();
                tree.isexpand = false;
                tree.complete = true;
                tree.checkstate = 0;
                tree.showcheck = false;
                tree.hasChildren = false;
                tree.parentId = "0";
                tree.Attribute = "Code";
                treeList.Add(tree);
            }
            if (treeList.Count > 0)
            {
                return Content(treeList.TreeToJson("0"));
            }
            else
            {
                return Content("0");
            }
        }



        /// <summary>
        /// 获取已开票车牌号
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetTicketEntity(string plateNo)
        {
            var entity = calculatebll.GetEntranceTicket(plateNo);
            return entity.ToJson();
        }

        /// <summary>
        ///获取科杰称重数据
        /// </summary>
        /// <param name="carNo"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetBridgData()
        {
            string weight = CacheFactory.Cache().GetCache<string>("PoundA:Weight");
            CacheFactory.Cache().RemoveCache("PoundA:Weight");
            return weight;
        }


        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            calculatebll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, CalculateEntity entity)
        {
            string clientIp = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            if (entity.Tare.HasValue && !entity.Taretime.HasValue)
            {
                entity.Taretime = DateTime.Now;
                entity.Tareusername = entity.Roughusername;
                if (!entity.Roughtime.HasValue)
                    entity.Roughusername = null;
            }
            if (entity.Rough.HasValue && !entity.Roughtime.HasValue)
            {
                entity.Roughtime = DateTime.Now;
                entity.Stamptime = DateTime.Now;
            }

            var data = Opertickebll.GetEntity(entity.BaseId);
            if (data != null)
            {
                if (string.IsNullOrWhiteSpace(entity.Transporttype))
                    entity.Transporttype = data.Transporttype;
                if (data.ShipLoading == 1 && !data.Getdata.HasValue)
                {
                    data.Getdata = entity.Taretime;
                    data.ExamineStatus = 3;
                }
                //更新开票信息
                if (entity.Rough.HasValue)
                {
                    #region 判断毛重日期与开票单号日期是否一致
                    //char[] numberChar = entity.Numbers.ToCharArray();
                    //Array.Reverse(numberChar);
                    //string orgianlDate = new string(numberChar).Substring(2, 6);
                    //char[] orginalChar = orgianlDate.ToCharArray();
                    //Array.Reverse(orginalChar);
                    //string orginalDate = new string(orginalChar);
                    //if (orginalDate != entity.Roughtime.Value.ToString("yyMMdd"))
                    //{
                    //    entity.Numbers = Opertickebll.GetTicketNumber(entity.Goodsname, entity.Takegoodsname, entity.Transporttype);
                    //    data.Numbers = entity.Numbers;
                    //}
                    #endregion
                    data.Weight = entity.Rough - entity.Tare;
                    data.OutCu = "1";
                    if (data.ShipLoading == 1)
                    {
                        data.OutDate = entity.Roughtime;
                        data.ExamineStatus = 4;
                    }
                }
                Opertickebll.SaveForm(data.ID, data);
            }

            #region 抬杠出地磅
            try
            {
                ipA = CacheFactory.Cache().GetCache<string>("PoundA:IP");
                ipB = CacheFactory.Cache().GetCache<string>("PoundB:IP");
                if (string.IsNullOrEmpty(ipA))
                {
                    ipA = dataItemDetailBLL.GetItemValue("PoundA:IP");//二号地磅室IP
                    CacheFactory.Cache().WriteCache<string>(ipA, "PoundA:IP");
                }
                if (string.IsNullOrEmpty(ipB))
                {
                    ipB = dataItemDetailBLL.GetItemValue("PoundB:IP");//一号地磅室IP
                    CacheFactory.Cache().WriteCache<string>(ipB, "PoundB:IP");
                }
                string openIndex = string.Empty;
                if (clientIp == ipB)
                    openIndex = CacheFactory.Cache().GetCache<string>("PoundB:Roadway");
                else if (clientIp == ipA)
                    openIndex = CacheFactory.Cache().GetCache<string>("PoundA:Roadway");
                RealaseVehicle(openIndex);
            }
            catch (Exception)
            {

            }
            #endregion

            calculatebll.SaveForm(keyValue, entity);
            if (entity.DataType == "0" && entity.IsOut == 1)
                RemoveCarpermission(entity.Platenumber);

            if (string.IsNullOrEmpty(keyValue))
                this.SaveDailyRecord(entity, "新增数据");
            else this.SaveDailyRecord(entity, "修改数据");
            return Success("操作成功。", entity.ID);
        }


        /// <summary>
        /// 地磅室称重单 保存
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveWeightBridge(string keyValue, CalculateDetailedEntity entity)
        {
            #region 新增地磅室称重单
            try
            {
                calculatebll.SaveWeightBridgeDetail(keyValue, entity);
                AddCarpermission(entity.Platenumber);
                //new CalculateDetailBLL().SaveForm(keyValue, entity);
            }
            catch (Exception)
            {

            }
            #endregion
            //if (string.IsNullOrEmpty(keyValue))
            //    this.SaveDailyRecord(entity, "新增数据");
            return Success("操作成功。", entity.ID);
        }


        /// <summary>
        /// 给车辆添加进入停车场的权限
        /// </summary>
        /// <param name="CarNo"></param>
        public void AddCarpermission(string CarNo)
        {
            string parkName = "二号地磅";
            string key = string.Empty;// "21049470";
            string sign = string.Empty;// "4gZkNoh3W92X6C66Rb6X";
            var pitem = dataItemDetailBLL.GetItemValue("Hikappkey");//海康服务器密钥
            var baseurl = dataItemDetailBLL.GetItemValue("HikBaseUrl");//海康服务器地址
            if (!string.IsNullOrEmpty(pitem))
            {
                key = pitem.Split('|')[0];
                sign = pitem.Split('|')[1];
            }

            #region 检查车辆在海康平台是否存在
            var selectmodel = new
            {
                pageNo = 1,
                pageSize = 100,
                plateNo = CarNo
            };
            var existsVehicleStr = SocketHelper.LoadCameraList(selectmodel, baseurl, "/artemis/api/resource/v1/vehicle/advance/vehicleList", key, sign);
            dynamic existsVehicle = JsonConvert.DeserializeObject<ExpandoObject>(existsVehicleStr);
            #endregion
            var parkmodel = new
            {
                parkIndexCodes = ""
            };

            string parkMsg = SocketHelper.LoadCameraList(parkmodel, baseurl, "/artemis/api/resource/v1/park/parkList", key, sign);
            parkList pl = JsonConvert.DeserializeObject<parkList>(parkMsg);
            if (pl != null && pl.data != null && pl.data.Count > 0)
            {
                #region 车辆权限编辑
                string[] parkNames = parkName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                //车辆需要经过的停车场
                List<string> pakindex = new List<string>();
                foreach (string pname in parkNames)
                {
                    pakindex.Add(pl.data.FirstOrDefault(x => x.parkName.Contains(pname))?.parkIndexCode);
                }
                if (existsVehicle.code == "0" && existsVehicle.data.total == 0)//车辆不存在就新增车辆
                {
                    var addModel = new
                    {
                        plateNo = CarNo,
                        plateType = 0,
                        plateColor = 1,
                        carType = 2,
                        carColor = 0,
                        mark = "物料车（地磅室）",
                        parkIndexCode = string.Join(",", pakindex),
                        startTime = DateTime.Now.ToString("yyyy-MM-dd"),
                        endTime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")
                    };
                    SocketHelper.LoadCameraList(new List<dynamic>() { addModel }, baseurl, "/artemis/api/v1/vehicle/addVehicle", key, sign);
                }
                else if (existsVehicle.code == "0" && existsVehicle.data.total > 0)//车辆存在就修改车辆
                {
                    var updateModel = new
                    {
                        plateNo = CarNo,
                        oldPlateNo = CarNo,
                        plateType = 0,
                        plateColor = 1,
                        carType = 2,
                        carColor = 0,
                        mark = "物料车（地磅室）",
                        parkIndexCode = string.Join(",", pakindex),
                        startTime = DateTime.Now.ToString("yyyy-MM-dd"),
                        endTime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"),
                        isUpdateFunction = 1
                    };
                    string updateMsg = SocketHelper.LoadCameraList(new List<dynamic>() { updateModel }, baseurl, "/artemis/api/v1/vehicle/updateVehicle", key, sign);
                }
                #endregion
            }
        }


        /// <summary>
        /// 移除车辆识别后自动抬杆放行权限
        /// </summary>
        /// <param name="carNo">车牌号</param>
        /// <param name="key">主键</param>
        [HttpGet]
        public ActionResult RemoveCarpermission(string carNo)
        {

            try
            {
                try
                {
                    #region 删除车辆进出权限  
                    string key = CacheFactory.Cache().GetCache<string>("Hik:key");// "21049470";
                    string sign = CacheFactory.Cache().GetCache<string>("Hik:sign");// "4gZkNoh3W92X6C66Rb6X";
                    string baseUrl = CacheFactory.Cache().GetCache<string>("Hik:baseUrl");
                    if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(key))
                    {
                        var pitem = dataItemDetailBLL.GetItemValue("Hikappkey");//海康服务器密钥                    
                        if (!string.IsNullOrEmpty(pitem))
                        {
                            key = pitem.Split('|')[0];
                            sign = pitem.Split('|')[1];
                            CacheFactory.Cache().WriteCache<string>(key, "Hik:key");
                            CacheFactory.Cache().WriteCache<string>(sign, "Hik:sign");
                        }
                    }
                    if (string.IsNullOrEmpty(baseUrl))
                    {
                        baseUrl = dataItemDetailBLL.GetItemValue("HikBaseUrl");//海康服务器地址
                        CacheFactory.Cache().WriteCache<string>(baseUrl, "Hik:baseUrl");
                    }

                    if (!string.IsNullOrEmpty(carNo))
                    {
                        var selectmodel = new
                        {
                            pageNo = 1,
                            pageSize = 1,
                            plateNo = carNo
                        };
                        var existsVehicleStr = SocketHelper.LoadCameraList(selectmodel, baseUrl, "/artemis/api/resource/v1/vehicle/advance/vehicleList", key, sign);
                        dynamic existsVehicle = JsonConvert.DeserializeObject<dynamic>(existsVehicleStr);
                        List<dynamic> vechileList = new List<dynamic>();

                        if (existsVehicle.code == "0" && existsVehicle.data.total > 0)
                        {
                            foreach (dynamic obj in existsVehicle.data.list)
                            {
                                vechileList.Add(obj.vehicleId);
                                break;
                            }
                            var delModel = new
                            {
                                vehicleIds = vechileList
                            };
                            SocketHelper.LoadCameraList(delModel, baseUrl, "/artemis/api/resource/v1/vehicle/batch/delete", key, sign);
                        }
                    }
                }
                catch (Exception) { }
                calculatebll.UpdateCalculateDetailTime(carNo);
                #endregion
                return Success("操作成功！");
            }
            catch (Exception)
            {
                return Success("操作失败！");
            }
        }
        /// <summary>
        ///净重超载75T(抬杠放行)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string OverLoad()
        {
            #region 抬杠出地磅
            try
            {
                ipA = CacheFactory.Cache().GetCache<string>("PoundA:IP");
                ipB = CacheFactory.Cache().GetCache<string>("PoundB:IP");
                if (string.IsNullOrEmpty(ipA))
                {
                    ipA = dataItemDetailBLL.GetItemValue("PoundA:IP");//二号地磅室IP
                    CacheFactory.Cache().WriteCache<string>(ipA, "PoundA:IP");
                }
                if (string.IsNullOrEmpty(ipB))
                {
                    ipB = dataItemDetailBLL.GetItemValue("PoundB:IP");//一号地磅室IP
                    CacheFactory.Cache().WriteCache<string>(ipB, "PoundB:IP");
                }
                string clientIp = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                string openIndex = string.Empty;
                if (clientIp == ipB)
                    openIndex = CacheFactory.Cache().GetCache<string>("PoundB:Roadway");
                else if (clientIp == ipA)
                    openIndex = CacheFactory.Cache().GetCache<string>("PoundA:Roadway");
                RealaseVehicle(openIndex);
                return "1";
            }
            catch (Exception)
            {
                return "1";
            }
            #endregion
        }

        /// <summary>
        /// 抬杠放行
        /// </summary>
        private void RealaseVehicle(string openIndex)
        {

            //一号，二号地磅对应的道闸编号
            Dictionary<string, string> parkRoadWay = new Dictionary<string, string>();
            //一号  （工程暂未安装）

            //二号
            parkRoadWay.Add("556e0aa5f4744bdfadbbd25b13546289", "689ba6f0b9ac4e1c807483808720589b");
            parkRoadWay.Add("689ba6f0b9ac4e1c807483808720589b", "556e0aa5f4744bdfadbbd25b13546289");
            string roadwaycode = parkRoadWay[openIndex];
            string key = string.Empty;// "21049470";
            string sign = string.Empty;// "4gZkNoh3W92X6C66Rb6X";
            var pitem = dataItemDetailBLL.GetItemValue("Hikappkey");//海康服务器密钥
            var baseurl = dataItemDetailBLL.GetItemValue("HikBaseUrl");//海康服务器地址
            if (!string.IsNullOrEmpty(pitem))
            {
                key = pitem.Split('|')[0];
                sign = pitem.Split('|')[1];
            }
            var ckmodel = new
            {
                roadwaySyscode = roadwaycode,
                command = 1
            };
            SocketHelper.LoadCameraList(ckmodel, baseurl, "/artemis/api/pms/v1/deviceControl", key, sign);
        }


        /// <summary>
        /// 修改时更换了车牌号更改老数据的称重状态
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateOutCuStatus(CalculateEntity entity)
        {
            var data = Opertickebll.GetEntity(entity.BaseId);
            if (data != null)
            {
                if (!data.Platenumber.Contains(entity.Platenumber))
                {
                    data.OutCu = "";
                    data.Weight = 0;
                    Opertickebll.SaveForm(data.ID, data);
                }
            }
        }


        /// <summary>
        /// 保存用户授权信息
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveUserForm(string keyValue, UserEmpowerRecordEntity entity)
        {
            calculatebll.SaveUserForm(keyValue, entity);
            return Success("操作成功。");
        }


        /// <summary>
        /// 删除记录状态1可用0已删除
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpateStatus(string keyValue, CalculateEntity entity)
        {
            var data = calculatebll.GetEntity(keyValue);
            if (data != null)
            {
                data.IsDelete = 0;
                data.DeleteContent = entity.DeleteContent;
                calculatebll.SaveForm(keyValue, data);
                string sql = string.Format("update WL_CALCULATEDetailed d set d.isdelete='0',d.deletecontent='{1}' where d.id='{0}'", keyValue, entity.DeleteContent);
                if (data.DataType == "4")
                    sql = string.Format("update wl_operticketmanager d set d.isdelete='0',d.deletecontent='{1}' where d.id='{0}'", keyValue, entity.DeleteContent);
                Opertickebll.GetDataTable(sql);//同步修改子记录状态
                SaveDailyRecord(data, "删除数据");
            }
            return Success("操作成功。");
        }

        /// <summary>
        /// 打印时修改打印时间
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        public string UpateStampTime(string keyValue)
        {
            var data = calculatebll.GetEntity(keyValue);
            if (data != null)
            {
                if (data.Stamptime == null)
                    data.Stamptime = DateTime.Now;
                calculatebll.SaveForm(keyValue, data);
                SaveDailyRecord(data, "打印称重单");
            }
            return "1";
        }

        /// <summary>
        /// 添加工作日志
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        public void SaveDailyRecord(CalculateEntity data, string type)
        {
            DailyrRecordEntity entity = new DailyrRecordEntity();
            entity.Content = data.Platenumber + "," + data.Rough + "," + data.Tare + "," + data.Netwneight;
            entity.WorkType = 3;
            entity.Theme = type;
            Opertickebll.InsetDailyRecord(entity);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="Takegoodsname">货运单位</param>
        /// <param name="Goodsname">副产品</param>
        /// <param name="Rough">毛重</param>
        /// <param name="Tare">皮重</param>
        /// <param name="Remark">备注</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveDetailedFrom(string keyValue, string Takegoodsname, string Goodsname, string Rough, string RoughTime, string Tare, string TareTime, string Remark)
        {
            var data = calculatebll.GetEntity(keyValue);
            if (data != null)
            {

                bool weightIsChange = false;
                double routh = double.Parse(Rough);
                double rare = double.Parse(Tare);
                if (rare != data.Tare || routh != data.Rough)
                {
                    data.Rough = routh;
                    data.Tare = rare;
                    weightIsChange = true;
                }

                data.Roughtime = DateTime.Parse(RoughTime);
                data.Taretime = DateTime.Parse(TareTime);
                data.Takegoodsname = Takegoodsname;
                data.Goodsname = Goodsname;
                data.Remark = Remark;
                calculatebll.SaveForm(keyValue, data);
                if (weightIsChange)
                {
                    var ticket = Opertickebll.GetEntity(data.BaseId);
                    if (ticket != null)
                    {
                        ticket.Weight = data.Rough - data.Tare;
                        Opertickebll.SaveForm(data.BaseId, ticket);
                    }
                }
            }
            return Success("操作成功。");
        }

        /// <summary>
        /// 添加工作日志
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        public void SaveDailyRecord(CalculateDetailedEntity data, string type)
        {
            DailyrRecordEntity entity = new DailyrRecordEntity();
            entity.Content = data.Platenumber + "," + data.Goodsname + "," + data.Takegoodsname + "," + data.Remark;
            entity.WorkType = 3;
            entity.Theme = type;
            Opertickebll.InsetDailyRecord(entity);
        }

        #endregion
    }
}
