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
    /// �� ������������
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
        #region ��ͼ����
        /// <summary>
        /// �б�ҳ��
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
                ipA = dataItemDetailBLL.GetItemValue("PoundA:IP");//���ŵذ���IP
                CacheFactory.Cache().WriteCache<string>(ipA, "PoundA:IP");

            }
            if (string.IsNullOrEmpty(ipB))
            {
                ipB = dataItemDetailBLL.GetItemValue("PoundB:IP");//һ�ŵذ���IP
                CacheFactory.Cache().WriteCache<string>(ipB, "PoundB:IP");
            }
            ViewBag.PoundAIP = ipA;
            ViewBag.PoundBIP = ipB;
            return View();
        }


        /// <summary>
        /// �б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult addForm()
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
        /// �ذ�Ա��Ȩ�б�
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UserList()
        {
            return View();
        }
        /// <summary>
        /// �ذ�Ա��Ȩ����
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UserForm()
        {
            return View();
        }

        /// <summary>
        /// ������¼
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Record()
        {
            return View();
        }

        /// <summary>
        /// ������¼����
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RecordForm()
        {
            return View();
        }


        /// <summary>
        /// ��¼ͳ��
        /// </summary>
        /// <returns></returns>
        public ActionResult CalculateCount()
        {
            return View();
        }

        /// <summary>
        /// ��ӡ��ͼ
        /// </summary>
        /// <returns></returns>
        public ActionResult Stamp()
        {
            return View();
        }

        /// <summary>
        /// �ӱ��¼��ӡ
        /// </summary>
        /// <returns></returns>
        public ActionResult NewStamp()
        {
            return View();
        }

        /// <summary>
        /// ���˼�¼����
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowForm()
        {
            return View();
        }

        /// <summary>
        /// ����ͳ����ϸ�鿴
        /// </summary>
        /// <returns></returns>
        public ActionResult CoountDetailForm()
        {
            return View();
        }

        /// <summary>
        /// �﹩����Ʊ��Ϣ
        /// </summary>
        /// <returns></returns>
        public ActionResult Orders()
        {
            return View();
        }

        /// <summary>
        /// δ�������Ͽ�Ʊ��
        /// </summary>
        /// <returns></returns>
        public ActionResult TicketSelect()
        {
            return View();
        }

        #endregion

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = calculatebll.GetList(queryJson);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
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
        /// ��ȡ��Ӧ�ӱ��¼�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
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
        /// ������¼�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
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
        /// ��ȡ����ͳ���б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetCountPageList(Pagination pagination, string queryJson)
        {

            string strwhere = "";
            string stime = string.Empty; string etime = string.Empty;
            var queryParam = queryJson.ToJObject();
            if (!queryParam["keyword"].IsEmpty())
            {//���ƺ�
                string PlateNumber = queryParam["keyword"].ToString().Trim();
                strwhere += string.Format(" and PlateNumber like '%{0}%'", PlateNumber);
            }
            if (!queryParam["Takegoodsname"].IsEmpty() && queryParam["Takegoodsname"].ToString().Trim() != "ȫ��")
            {//�����
                string Transporttype = queryParam["Takegoodsname"].ToString().Trim();
                strwhere += string.Format(" and Takegoodsname='{0}'", Transporttype);
            }
            if (!queryParam["Transporttype"].IsEmpty() && queryParam["Transporttype"].ToString().Trim() != "ȫ��")
            {//��������
                string Transporttype = queryParam["Transporttype"].ToString().Trim();
                strwhere += string.Format(" and Transporttype like '%{0}%'", Transporttype);
            }
            if (!queryParam["Goodsname"].IsEmpty() && queryParam["Goodsname"].ToString().Trim() != "ȫ��")
            {//����Ʒ����
                string Goodsname = queryParam["Goodsname"].ToString().Trim();
                strwhere += string.Format(" and Goodsname like '%{0}%'", Goodsname);
            }
            if (!queryParam["Stime"].IsEmpty() && !queryParam["Etime"].IsEmpty())
            {//��ӡʱ����
                stime = queryParam["Stime"].ToString().Trim();
                strwhere += string.Format(" and roughtime >=  to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') ", Convert.ToDateTime(stime));
                //��ӡʱ��ֹ
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
        /// ����ͳ����ϸ�鿴
        /// </summary>
        /// <param name="queryJson">���ݹ���ɸѡ����</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCointDetailList(string queryJson)
        {
            // Pagination pagination = new Pagination();
            string strwhere = " 1=1 and t.isdelete='1' ";
            string stime = string.Empty; string etime = string.Empty;
            var queryParam = queryJson.ToJObject();
            if (!queryParam["Takegoodsname"].IsEmpty() )
            {//�����
                string Transporttype = queryParam["Takegoodsname"].ToString().Trim();
                strwhere += string.Format(" and t.Takegoodsname='{0}'", Transporttype);
            }
            if (!queryParam["Transporttype"].IsEmpty())
            {//��������
                string Transporttype = queryParam["Transporttype"].ToString().Trim();
                strwhere += string.Format(" and t.Transporttype='{0}'", Transporttype);
            }
            if (!queryParam["Goodsname"].IsEmpty() )
            {//����Ʒ����
                string Goodsname = queryParam["Goodsname"].ToString().Trim();
                strwhere += string.Format(" and t.Goodsname='{0}'", Goodsname);
            }
            if (!queryParam["RoughDate"].IsEmpty() )
            {//��ӡʱ����
                string RoughDate = queryParam["RoughDate"].ToString().Trim();
                strwhere += string.Format(" and to_char(t.roughtime,'yyyy-MM-dd')='{0}' ", RoughDate);
                           }
            else
            {//Ĭ�ϱ���                 
                strwhere += string.Format(" and to_char(t.roughtime,'yyyy-MM-dd')='{0}' ", DateTime.Now.ToString("yyyy-MM-dd"));
            }

            string sql = string.Format(@"select to_char(d.Getdata,'MM-dd') as rctime,to_char(d.Getdata,'hh24:mi') as rctime1��
                                          to_char(t.taretime,' hh24:mi') as BalanceTime,
                                          to_char(t.roughtime,'MM-dd') as mztime,to_char(t.roughtime,' hh24:mi') as mztime1,
                                          to_char(d.outdate,' hh24:mi') as outdate,
                                          d.Takegoodsname,d.platenumber,d.Transporttype,d.Numbers as Numbers1 ,t.numbers,t.tare,t.tareusername,
                                          t.rough,t.roughusername,t.netwneight,d.Opername,d.LetMan,d.status,d.PassRemark,
                                          d.Getdata,d.BalanceTime as balancetime1,d.outdate as outdate1
                                          from wl_operticketmanager d
                                          join wl_calculate t on d.id = t.baseid where {0}	
	                                        union 
                                          select to_char(f.createdate,'MM-dd') as rctime,null as rctime1��
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
        /// ��ȡ�ذ��ҿ�Ʊ��Ϣ
        /// </summary>                       
        /// <param name="pagination">��ҳɸѡ����</param>
        /// <param name="queryJson">���ݹ���ɸѡ����</param>
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

        #region ���ݵ���
        /// <summary>
        /// ��������ͳ���б�
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "�������ϳ�������")]
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

            //���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "���ϳ��ؼ���";
            excelconfig.TitleFont = "΢���ź�";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "���ϳ��ؼ���.xls";
            excelconfig.IsAllSizeColumn = true;
            //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity = listColumnEntity;
            ColumnEntity columnentity = new ColumnEntity();
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "numbers", ExcelColumn = "���/ת�˵���", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "takegoodsname", ExcelColumn = "�˻���λ", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "platenumber", ExcelColumn = "���ƺ�", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "goodsname", ExcelColumn = "����", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "rough", ExcelColumn = "ë��", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "tare", ExcelColumn = "Ƥ��", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "netwneight", ExcelColumn = "����", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "roughtime", ExcelColumn = "ë��ʱ��", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "roughusername", ExcelColumn = "ë��˾��Ա", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "taretime", ExcelColumn = "Ƥ��ʱ��", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "tareusername", ExcelColumn = "Ƥ��˾��Ա", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "stamptime", ExcelColumn = "���ص���ӡʱ��", Alignment = "center" });
            //���õ�������
            // ExcelHelper.ExcelDownload(data, excelconfig);

            ExcelHelper.ExportByAspose(data, "���ϳ��ؼ���", excelconfig.ColumnEntity);

            return Success("�����ɹ���");
        }

        /// <summary>
        /// ����ʱ��֮�����ӣ�
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
        /// ��ȡ�ذ�Ա�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetPageUserList(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "USERID";
            pagination.p_fields = "d.createdate,t.starttime,t.endtime,t.status,d.account,d.realname,d.gender,d.mobile,d.ispresence";
            pagination.p_tablename = " V_USERINFO d left join(select id,createdate,endtime,userids,starttime,status  from (select id,status,starttime,createdate,endtime,userid as userids,row_number() over(PARTITION BY userid ORDER BY createdate desc) as row_flg from wl_userempowerRecord t) temp where row_flg=1) t on t.userids= d.userid ";
            pagination.conditionJson = "d.Account!='System' and d.ispresence='��'";
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
        /// ��ȡ�ذ�Ա��ɫ
        /// </summary>
        /// <returns></returns>
        public string Getrolename()
        {
            string res = string.Format("{0}", "�ذ�Ա��ɫ");
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
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = calculatebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// �Ƿ�ֻ��һ�γ�����Ϣ 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetOnlyFormJson(string keyValue)
        {
            var data = calculatebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }


        /// <summary>
        /// ��ȡ�ӱ��¼ʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetDetailedFormJson(string keyValue)
        {
            var data = calculatebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ����һ����¼ʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetNewFormJson(string keyValue)
        {
            var data = calculatebll.GetNewEntity("");
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ��Ȩ��¼ʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetUserFormJson(string keyValue)
        {
            UserEmpowerRecordEntity entity = calculatebll.GetUserRecord(keyValue);
            if (entity != null)
            {//������Ȩ��¼����Ϣ
                return ToJsonResult(entity);
            }
            else
            {//�����û�����Ϣ
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
        /// ��ǰ�û��Ƿ�����Ȩʱ�䷶Χ��
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
        /// ��ȡ�����������ṹ
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
        /// ��ȡ��ʱ�����ṹ
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
        /// ��ȡ�ѿ�Ʊ���ƺ�
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetTicketEntity(string plateNo)
        {
            var entity = calculatebll.GetEntranceTicket(plateNo);
            return entity.ToJson();
        }

        /// <summary>
        ///��ȡ�ƽܳ�������
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
            calculatebll.RemoveForm(keyValue);
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
                //���¿�Ʊ��Ϣ
                if (entity.Rough.HasValue)
                {
                    #region �ж�ë�������뿪Ʊ���������Ƿ�һ��
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

            #region ̧�ܳ��ذ�
            try
            {
                ipA = CacheFactory.Cache().GetCache<string>("PoundA:IP");
                ipB = CacheFactory.Cache().GetCache<string>("PoundB:IP");
                if (string.IsNullOrEmpty(ipA))
                {
                    ipA = dataItemDetailBLL.GetItemValue("PoundA:IP");//���ŵذ���IP
                    CacheFactory.Cache().WriteCache<string>(ipA, "PoundA:IP");
                }
                if (string.IsNullOrEmpty(ipB))
                {
                    ipB = dataItemDetailBLL.GetItemValue("PoundB:IP");//һ�ŵذ���IP
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
                this.SaveDailyRecord(entity, "��������");
            else this.SaveDailyRecord(entity, "�޸�����");
            return Success("�����ɹ���", entity.ID);
        }


        /// <summary>
        /// �ذ��ҳ��ص� ����
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveWeightBridge(string keyValue, CalculateDetailedEntity entity)
        {
            #region �����ذ��ҳ��ص�
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
            //    this.SaveDailyRecord(entity, "��������");
            return Success("�����ɹ���", entity.ID);
        }


        /// <summary>
        /// ��������ӽ���ͣ������Ȩ��
        /// </summary>
        /// <param name="CarNo"></param>
        public void AddCarpermission(string CarNo)
        {
            string parkName = "���ŵذ�";
            string key = string.Empty;// "21049470";
            string sign = string.Empty;// "4gZkNoh3W92X6C66Rb6X";
            var pitem = dataItemDetailBLL.GetItemValue("Hikappkey");//������������Կ
            var baseurl = dataItemDetailBLL.GetItemValue("HikBaseUrl");//������������ַ
            if (!string.IsNullOrEmpty(pitem))
            {
                key = pitem.Split('|')[0];
                sign = pitem.Split('|')[1];
            }

            #region ��鳵���ں���ƽ̨�Ƿ����
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
                #region ����Ȩ�ޱ༭
                string[] parkNames = parkName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                //������Ҫ������ͣ����
                List<string> pakindex = new List<string>();
                foreach (string pname in parkNames)
                {
                    pakindex.Add(pl.data.FirstOrDefault(x => x.parkName.Contains(pname))?.parkIndexCode);
                }
                if (existsVehicle.code == "0" && existsVehicle.data.total == 0)//���������ھ���������
                {
                    var addModel = new
                    {
                        plateNo = CarNo,
                        plateType = 0,
                        plateColor = 1,
                        carType = 2,
                        carColor = 0,
                        mark = "���ϳ����ذ��ң�",
                        parkIndexCode = string.Join(",", pakindex),
                        startTime = DateTime.Now.ToString("yyyy-MM-dd"),
                        endTime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")
                    };
                    SocketHelper.LoadCameraList(new List<dynamic>() { addModel }, baseurl, "/artemis/api/v1/vehicle/addVehicle", key, sign);
                }
                else if (existsVehicle.code == "0" && existsVehicle.data.total > 0)//�������ھ��޸ĳ���
                {
                    var updateModel = new
                    {
                        plateNo = CarNo,
                        oldPlateNo = CarNo,
                        plateType = 0,
                        plateColor = 1,
                        carType = 2,
                        carColor = 0,
                        mark = "���ϳ����ذ��ң�",
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
        /// �Ƴ�����ʶ����Զ�̧�˷���Ȩ��
        /// </summary>
        /// <param name="carNo">���ƺ�</param>
        /// <param name="key">����</param>
        [HttpGet]
        public ActionResult RemoveCarpermission(string carNo)
        {

            try
            {
                try
                {
                    #region ɾ����������Ȩ��  
                    string key = CacheFactory.Cache().GetCache<string>("Hik:key");// "21049470";
                    string sign = CacheFactory.Cache().GetCache<string>("Hik:sign");// "4gZkNoh3W92X6C66Rb6X";
                    string baseUrl = CacheFactory.Cache().GetCache<string>("Hik:baseUrl");
                    if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(key))
                    {
                        var pitem = dataItemDetailBLL.GetItemValue("Hikappkey");//������������Կ                    
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
                        baseUrl = dataItemDetailBLL.GetItemValue("HikBaseUrl");//������������ַ
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
                return Success("�����ɹ���");
            }
            catch (Exception)
            {
                return Success("����ʧ�ܣ�");
            }
        }
        /// <summary>
        ///���س���75T(̧�ܷ���)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string OverLoad()
        {
            #region ̧�ܳ��ذ�
            try
            {
                ipA = CacheFactory.Cache().GetCache<string>("PoundA:IP");
                ipB = CacheFactory.Cache().GetCache<string>("PoundB:IP");
                if (string.IsNullOrEmpty(ipA))
                {
                    ipA = dataItemDetailBLL.GetItemValue("PoundA:IP");//���ŵذ���IP
                    CacheFactory.Cache().WriteCache<string>(ipA, "PoundA:IP");
                }
                if (string.IsNullOrEmpty(ipB))
                {
                    ipB = dataItemDetailBLL.GetItemValue("PoundB:IP");//һ�ŵذ���IP
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
        /// ̧�ܷ���
        /// </summary>
        private void RealaseVehicle(string openIndex)
        {

            //һ�ţ����ŵذ���Ӧ�ĵ�բ���
            Dictionary<string, string> parkRoadWay = new Dictionary<string, string>();
            //һ��  ��������δ��װ��

            //����
            parkRoadWay.Add("556e0aa5f4744bdfadbbd25b13546289", "689ba6f0b9ac4e1c807483808720589b");
            parkRoadWay.Add("689ba6f0b9ac4e1c807483808720589b", "556e0aa5f4744bdfadbbd25b13546289");
            string roadwaycode = parkRoadWay[openIndex];
            string key = string.Empty;// "21049470";
            string sign = string.Empty;// "4gZkNoh3W92X6C66Rb6X";
            var pitem = dataItemDetailBLL.GetItemValue("Hikappkey");//������������Կ
            var baseurl = dataItemDetailBLL.GetItemValue("HikBaseUrl");//������������ַ
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
        /// �޸�ʱ�����˳��ƺŸ��������ݵĳ���״̬
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
        /// �����û���Ȩ��Ϣ
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveUserForm(string keyValue, UserEmpowerRecordEntity entity)
        {
            calculatebll.SaveUserForm(keyValue, entity);
            return Success("�����ɹ���");
        }


        /// <summary>
        /// ɾ����¼״̬1����0��ɾ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
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
                Opertickebll.GetDataTable(sql);//ͬ���޸��Ӽ�¼״̬
                SaveDailyRecord(data, "ɾ������");
            }
            return Success("�����ɹ���");
        }

        /// <summary>
        /// ��ӡʱ�޸Ĵ�ӡʱ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
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
                SaveDailyRecord(data, "��ӡ���ص�");
            }
            return "1";
        }

        /// <summary>
        /// ��ӹ�����־
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
        /// <param name="Takegoodsname">���˵�λ</param>
        /// <param name="Goodsname">����Ʒ</param>
        /// <param name="Rough">ë��</param>
        /// <param name="Tare">Ƥ��</param>
        /// <param name="Remark">��ע</param>
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
            return Success("�����ɹ���");
        }

        /// <summary>
        /// ��ӹ�����־
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
