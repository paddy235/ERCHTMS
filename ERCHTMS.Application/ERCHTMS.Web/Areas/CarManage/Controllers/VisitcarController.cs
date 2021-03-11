using System;
using System.Collections.Generic;
using System.Linq;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Busines.CarManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.MatterManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.MatterManage;
using ERCHTMS.Entity.OccupationalHealthManage;
using Newtonsoft.Json;
using System.Data;
using System.Dynamic;
namespace ERCHTMS.Web.Areas.CarManage.Controllers
{
    /// <summary>
    /// 描 述：拜访车辆表
    /// </summary>
    public class VisitcarController : MvcControllerBase
    {
        private VisitcarBLL visitcarbll = new VisitcarBLL();
        PersongpsBLL pbll = new PersongpsBLL();
        private CarinlogBLL logbll = new CarinlogBLL();
        CarUserBLL CarUserbll = new CarUserBLL();
        DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();
        OperticketmanagerBLL Opertickebll = new OperticketmanagerBLL();

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
        /// 发放定位终端页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult IssueGps()
        {
            return View();
        }
        /// <summary>
        /// 人员拜访发放定位终端页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RyIssueGps()
        {
            return View();
        }
        /// <summary>
        /// 发放定位终端页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult WlIssueGps()
        {
            return View();
        }

        /// <summary>
        /// 车辆准出页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult OutForm()
        {
            return View();
        }
        /// <summary>
        /// 人员准出页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult OutUserForm()
        {
            return View();
        }
        /// <summary>
        /// 人脸录入选择页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CheckUserFace()
        {
            return View();
        }


        /// <summary>
        /// 车辆人员记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CarRecord()
        {
            return View();
        }

        /// <summary>
        /// 查看人员记录详情
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ShowUserRecord()
        {
            return View();
        }

        /// <summary>
        /// 拜访车辆后台列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ListIndex()
        {
            return View();
        }

        /// <summary>
        /// 拜访车辆后台列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ShowForm()
        {
            return View();
        }

        /// <summary>
        /// 查看图片
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ImgShow()
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
            var data = visitcarbll.GetList(queryJson);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public string GetLogNum()
        {
            var data = logbll.GetLogNum();
            return data.ToString();
        }

        /// <summary>
        /// 获得当日外来车辆数量
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetOutCarNum()
        {
            var list = visitcarbll.GetOutCarNum();
            return Content(list.ToJson());
        }
        /// <summary>
        /// 获得当日外来人员数量
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetForeignUserNum()
        {
            string sql = string.Format("select (select count(1) from bis_persongps s where TO_CHAR(intime,'yyyy-MM-dd') >= '{0}' ) as SumNum,(select count(1) from bis_persongps s1 where s1.state='0' and TO_CHAR(intime,'yyyy-MM-dd') >= '{0}' ) as Num  from bis_persongps d where rownum<2", DateTime.Now.ToString("yyyy-MM-dd"));
            var data = Opertickebll.GetDataTable(sql);
            List<string> list = new List<string>();
            string num = "0";
            string num1 = "0";
            if (data.Rows.Count > 0)
            {
                num = data.Rows[0][0].ToString();
                num1 = data.Rows[0][1].ToString();
            }
            list.Add(num);
            list.Add(num1);
            return Content(list.ToJson());
        }

        /// <summary>
        /// 获取拜访车辆列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListPageJson(string paginations, string queryJson)
        {
            Pagination pagination = JsonConvert.DeserializeObject<Pagination>(paginations);

            string orgcode = OperatorProvider.Provider.Current().OrganizeCode;
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "ID";
            pagination.p_fields = "Createdate,OUTTIME,carno,purpose,dirver,phone,note,anumber,driverlicenseurl,drivinglicenseurl,state,type,vnum,intime";
            pagination.p_tablename = @"(
            select * from (
            select ID,Createdate,OUTTIME,carno,CONCAT(CONCAT(CONCAT('拜访-',visitdept),'-'),visituser) as purpose,dirver
            ,phone,note,ACCOMPANYINGPERSON anumber,driverlicenseurl,drivinglicenseurl,state,'0' type,NVL(vnum,0) vnum,intime from bis_visitcar  vi
            left join (select count(cid) vnum ,cid from BIS_CARVIOLATION  group by cid  )  cv on vi.id=cv.cid
            where state>2 

            union 

            select ID,Createdate,OutDate OUTTIME,platenumber carno,CONCAT(CONCAT(CONCAT(CONCAT(dress,'-'),transporttype),'-'),PRODUCTTYPE) as purpose,
            DriverName dirver,DriverTel phone,PassRemark note,JsImgpath anumber,JsImgpath driverlicenseurl,XsImgpath drivinglicenseurl,examinestatus state,'1' type,NVL(vnum,0) vnum,Getdata as intime
            from WL_OPERTICKETMANAGER vi
            left join (select count(cid) vnum ,cid from BIS_CARVIOLATION  group by cid  )  cv on vi.id=cv.cid
             where isdelete=1 and examinestatus>2 
            
            union 

            select ID,Createdate,OUTTIME,carno,CONCAT(CONCAT(CONCAT('危化品-',thecompany),'-'),hazardousname) as purpose,dirver
            ,phone,note,ACCOMPANYINGPERSON anumber,driverlicenseurl,drivinglicenseurl,state,'2' type,NVL(vnum,0) vnum,intime from bis_hazardouscar  hazardous
            left join (select count(cid) vnum ,cid from BIS_CARVIOLATION  group by cid  )  cv on hazardous.id=cv.cid
            where state>2 ) ts order by ts.Createdate desc

            ) a1 
            ";
            pagination.conditionJson = " 1=1";
            pagination.sord = "Createdate desc";
            pagination.records = 0;
            pagination.conditionJson = "1=1";

            var data = visitcarbll.GetPageList(pagination, queryJson);
            //data.DefaultView.Sort = "createdate desc";
            //var dt = data.DefaultView.ToTable();
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
        /// 获取拜访人员记录列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListPageJsonNew(string paginations, string queryJson)
        {
            Pagination pagination = JsonConvert.DeserializeObject<Pagination>(paginations);
            string orgcode = OperatorProvider.Provider.Current().OrganizeCode;
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "d.ID";
            pagination.p_fields = @" d.Createdate,d.dirver,d.phone,CONCAT(CONCAT(CONCAT('拜访-',d.visitdept),'-'),d.visituser) as purpose,d.note,d.intime,d.outtime";
            pagination.p_tablename = @" BIS_USERCAR d  ";
            pagination.conditionJson = " 1=1";
            pagination.sord = " d.Createdate desc";
            pagination.records = 0;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            //if (user.IsSystem)
            //{
            pagination.conditionJson = "1=1";
            var data = CarUserbll.GetPageList(pagination, queryJson);
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
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult BackGetListPageJson(Pagination pagination, string queryJson)
        {
            string orgcode = OperatorProvider.Provider.Current().OrganizeCode;
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "ID";
            pagination.p_fields = "createuserid,createdate,modifyuserid,modifydate,createuserdeptcode,createuserorgcode,outtime,state,carno,dirver,phone,visitdept,NVL(vnum,0) vnum";
            pagination.p_tablename = @" ( select id,createuserid,createdate,modifyuserid,modifydate,createuserdeptcode,createuserorgcode,outtime,state,carno,dirver,phone,visitdept,vnum    from bis_visitcar  vi
            left join (select count(cid) vnum ,cid from BIS_CARVIOLATION  group by cid  )  cv on vi.id=cv.cid) v1 ";
            pagination.conditionJson = " 1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            //if (user.IsSystem)
            //{
            pagination.conditionJson = "1=1";
            //}
            //else
            //{




            //    string where = new ERCHTMS.Busines.AuthorizeManage.AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
            //    pagination.conditionJson += " and " + where;




            //}

            var data = visitcarbll.GetPageList(pagination, queryJson);

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
        /// 审批状态
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetStatus()
        {
            List<ComboxEntity> Rlist = new List<ComboxEntity>();
            ComboxEntity y1 = new ComboxEntity();
            y1.itemName = "待审核";
            y1.itemValue = "1";

            //ComboxEntity y2 = new ComboxEntity();
            //y2.itemName = "已绑定GPS";
            //y2.itemValue = "2";

            ComboxEntity y3 = new ComboxEntity();
            y3.itemName = "已入厂";
            y3.itemValue = "3";

            ComboxEntity y4 = new ComboxEntity();
            y4.itemName = "已离厂";
            y4.itemValue = "4";

            ComboxEntity y5 = new ComboxEntity();
            y5.itemName = "拒绝入厂";
            y5.itemValue = "99";

            Rlist.Add(y1);
            //Rlist.Add(y2);
            Rlist.Add(y3);
            Rlist.Add(y4);
            Rlist.Add(y5);


            return ToJsonResult(Rlist);

        }

        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = visitcarbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 根据类型获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetEntity(string keyValue, int type)
        {
            if (type == 0)
            {
                var data = visitcarbll.GetEntity(keyValue);
                return ToJsonResult(data);
            }
            else if (type == 1)
            {
                OperticketmanagerBLL operbll = new OperticketmanagerBLL();
                var data = operbll.GetEntity(keyValue);
                return ToJsonResult(data);
            }
            else if (type == 3)
            {//拜访人员
                CarUserBLL CarUserbll = new CarUserBLL();
                var data = CarUserbll.GetEntity(keyValue);
                return ToJsonResult(data);
            }
            else
            {
                HazardouscarBLL haza = new HazardouscarBLL();
                var data = haza.GetEntity(keyValue);
                return ToJsonResult(data);
            }
        }

        /// <summary>
        /// 获取门岗数据 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetDoorList()
        {
            var data = visitcarbll.GetDoorList();
            return ToJsonResult(data);
        }

        /// <summary>
        /// 查询此GPS设备是否被占用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetGps(string id, string gpsid)
        {

            if (id.Trim() == "")
            {
                id = "1";
            }

            return pbll.GetGps(id, gpsid).ToString();
        }

        DepartmentBLL departmentBLL = new DepartmentBLL();

        public ActionResult GetDepartment(string OrgId)
        {
            OrganizeBLL orgBLL = new OrganizeBLL();

            string organizeId = OrgId;
            string parentId = "0";
            IList<SelectDeptData> list = new List<SelectDeptData>();
            //获取当前机构下的所有部门
            OrganizeEntity org = orgBLL.GetEntity(organizeId);
            SelectDeptData dept = new SelectDeptData();
            dept.deptid = org.OrganizeId;
            dept.code = org.EnCode;
            dept.oranizeid = org.OrganizeId;
            dept.parentcode = "";
            dept.parentid = parentId;
            dept.name = org.FullName;
            list.Add(dept);
            List<DepartmentEntity> data = new List<DepartmentEntity>();

            data = departmentBLL.GetList().Where(t => t.OrganizeId == organizeId).OrderBy(t => t.SortCode).ToList();

            foreach (DepartmentEntity entity in data)
            {
                SelectDeptData depts = new SelectDeptData();
                depts.deptid = entity.DepartmentId;
                depts.code = entity.EnCode;
                depts.oranizeid = entity.OrganizeId;
                depts.parentid = entity.ParentId;
                var parentDept = data.Where(p => p.DepartmentId == depts.parentid).FirstOrDefault();
                if (null != parentDept)
                {
                    depts.parentcode = parentDept.EnCode;
                }
                else
                {
                    depts.parentcode = "";
                }
                depts.name = entity.FullName;
                list.Add(depts);
            }

            return Content(list.ToJson());
        }

        private UserBLL userbll = new UserBLL();
        /// <summary>
        /// 获取对应部门下的用户信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetUserInfos(string deptcode)
        {
            List<UserInfoEntity> ulist = userbll.GetUserInfoByDeptCode(deptcode).ToList();

            return Content(ulist.ToJson());
        }

        /// <summary>
        /// 通过主表Id获取人员与Gps关联表
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetPersongpslist(string keyValue)
        {
            var data = CarUserbll.GetPersongpslist(keyValue);
            return Content(data.ToJson());
        }

        /// <summary>
        /// 获取拜访人员记录
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetUserRecodelist(string keyValue, string name)
        {
            string sql = string.Format("select w.intime,w.outtime,w.state,(select count(1) from bis_persongps w where w.vid='{0}') as sum,(select count(1) from bis_persongps w where w.vid='{0}' and w.state='1') as outnum from bis_persongps w where w.vid='{0}'  and w.username='{1}' ", keyValue, name);
            var data = Opertickebll.GetDataTable(sql);
            return Content(data.ToJson());
        }

        /// <summary>
        /// /判断手机端拜访记录是否审批通过
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ApprovalIsAdopt(string keyValue, int type)
        {
            int State = 0;
            if (type == 3)
            {//人员
                var uentity = new CarUserBLL().GetEntity(keyValue);
                if (uentity != null) State = uentity.AppStatue;
            }
            else if (type == 0)
            {//车辆
                var ventity = new VisitcarBLL().GetEntity(keyValue);
                if (ventity != null) State = ventity.AppStatue;
            }
            return Content(State.ToString());
        }

        #endregion

        #region 提交数据
        /// <summary>
        /// 根据ID修改
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="LineName"></param>
        /// <param name="LineID"></param>
        /// <returns></returns>
        public ActionResult ChangeLine(string keyValue, string LineName, string LineID)
        {
            visitcarbll.ChangeLine(keyValue, LineName, LineID);
            return Success("提交成功。");
        }

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
            visitcarbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, VisitcarEntity entity, List<PersongpsEntity> pergps)
        {
            visitcarbll.ChangeGps(keyValue, entity, pergps);
            return Success("操作成功。");
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
        public ActionResult SaveUserForm(string keyValue, CarUserEntity entity, List<PersongpsEntity> pergps)
        {
            CarUserbll.ChangeGps(keyValue, entity, pergps);
            return Success("操作成功。");
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
        public ActionResult WlSaveForm(string keyValue, OperticketmanagerEntity entity)
        {

            visitcarbll.WlChangeGps(keyValue, entity);
            return Success("操作成功。");
        }

        /// <summary>
        /// 车辆出厂
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult CarOut(string keyValue, string Note, int type, List<PersongpsEntity> pergps)
        {
            //根据危化品类型添加二级节点
            var data = dataItemDetailBLL.GetDataItemListByItemCode("'SocketUrl'");
            string IP = "";
            int Port = 0;
            foreach (var item in data)
            {
                if (item.ItemName == "IP")
                {
                    IP = item.ItemValue;
                }
                else if (item.ItemName == "Port")
                {
                    Port = Convert.ToInt32(item.ItemValue);
                }
            }
            visitcarbll.CarOut(keyValue, Note, type, pergps);
            CarAlgorithmEntity Car = new CarAlgorithmEntity();
            Car.ID = keyValue;
            Car.State = 1;
            SocketHelper.SendMsg(Car.ToJson(), IP, Port);

            DataItemDetailBLL pdata = new DataItemDetailBLL();
            string key = string.Empty;// "21049470";
            string sign = string.Empty;// "4gZkNoh3W92X6C66Rb6X";
            var pitem = pdata.GetItemValue("Hikappkey");//海康服务器密钥
            var url = pdata.GetItemValue("HikBaseUrl");//海康服务器地址
            if (!string.IsNullOrEmpty(pitem))
            {
                key = pitem.Split('|')[0];
                sign = pitem.Split('|')[1];
            }

            if (type == 0)
            {
                VisitcarEntity car = visitcarbll.GetEntity(keyValue);
                HikOut(url, key, sign, car.CarNo);
            }
            else if (type == 1)
            {
                OperticketmanagerBLL obll = new OperticketmanagerBLL();
                OperticketmanagerEntity op = obll.GetEntity(keyValue);
                HikOut(url, key, sign, op.Platenumber);
            }
            else if (type == 2)
            {
                HazardouscarBLL hbll = new HazardouscarBLL();
                HazardouscarEntity ha = hbll.GetEntity(keyValue);
                HikOut(url, key, sign, ha.CarNo);
            }
            return Success("操作成功。");
        }

        /// <summary>
        /// 拜访人员出厂
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult CarUserOut(string keyValue, string Note, int type, List<PersongpsEntity> pergps)
        {
            try
            {
                if (pergps == null)
                    pergps = new List<PersongpsEntity>();
                CarUserbll.CarOut(keyValue, Note, type, pergps);
                return Success("操作成功。");
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// 改变状态（准许入场下发门禁出入权限）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangeState(string keyvalue, int type, int state)
        {

            //根据危化品类型添加二级节点
            #region 获取编码管理配置信息
            var data = dataItemDetailBLL.GetDataItemListByItemCode("'SocketUrl'");
            string IP = "";
            int Port = 0;
            foreach (var item in data)
            {
                if (item.ItemName == "IP")
                {
                    IP = item.ItemValue;
                }
                else if (item.ItemName == "Port")
                {
                    Port = Convert.ToInt32(item.ItemValue);
                }
            }

            DataItemDetailBLL pdata = new DataItemDetailBLL();
            string key = string.Empty;// "21049470";
            string sign = string.Empty;// "4gZkNoh3W92X6C66Rb6X";
            var pitem = pdata.GetItemValue("Hikappkey");//海康服务器密钥
            var baseurl = pdata.GetItemValue("HikBaseUrl");//海康服务器地址
            if (!string.IsNullOrEmpty(pitem))
            {
                key = pitem.Split('|')[0];
                sign = pitem.Split('|')[1];
            }
            #endregion
            state++;
            if (type == 0)
            {//拜访车辆
                VisitcarEntity car = visitcarbll.GetEntity(keyvalue);
                car.State = state;
                car.InTime = DateTime.Now;
                visitcarbll.SaveForm(keyvalue, car);
                CarAlgorithmEntity Car = new CarAlgorithmEntity();
                Car.CarNo = car.CarNo;
                Car.GPSID = car.GPSID;
                Car.GPSName = car.GPSNAME;
                Car.ID = car.ID;
                Car.Type = 3;
                Car.State = 0;
                Car.LineName = car.LineName;
                Car.GoodsName = "";
                SocketHelper.SendMsg(Car.ToJson(), IP, Port);
                //车辆放行 (疫情期间此功能暂时屏蔽)
                //AddCarpermission(baseurl, key, sign, car.CarNo, car.Phone, car.Dirver);
                var baselist = CarUserbll.addUserJurisdiction(keyvalue, state, baseurl, key, sign);
                return Content(baselist.ToJson());
            }
            else if (type == 3)
            {//拜访人员
                CarUserEntity car = CarUserbll.GetEntity(keyvalue);
                car.State = type;
                car.InTime = DateTime.Now;
                CarUserbll.SaveForm(keyvalue, car, null);
                //人员添加出入权限
                var baselist = CarUserbll.addUserJurisdiction(keyvalue, state, baseurl, key, sign);
                return Content(baselist.ToJson());
            }
            else if (type == 1)
            {//物料车辆
                string parkNames = "1号岗,二号地磅";
                OperticketmanagerBLL obll = new OperticketmanagerBLL();
                OperticketmanagerEntity op = obll.GetEntity(keyvalue);
                op.ExamineStatus = state;
                if (state == 3)
                {
                    op.Getdata = DateTime.Now;
                }
                obll.SaveForm(keyvalue, op);
                CarAlgorithmEntity Car = new CarAlgorithmEntity();
                Car.CarNo = op.Platenumber;
                Car.GPSID = op.GpsId;
                Car.GPSName = op.GpsName;
                Car.ID = op.ID;
                Car.State = 0;
                Car.Type = 4;
                string Dress = op.Dress;
                Car.GoodsName = Dress;
                int ISwharf = op.ISwharf;
                string Transporttype = op.Transporttype;
                if (Transporttype == "提货")
                {
                    Car.LineName = op.Dress + Transporttype;
                    if (ISwharf == 1)
                    {
                        Car.LineName += "(码头)";
                        parkNames += ",码头岗";
                    }
                }
                else
                {
                    if (ISwharf == 1)
                    {
                        Car.LineName = "物料转运(码头)";
                        parkNames += ",码头岗";
                    }
                    else
                    {
                        Car.LineName = "转运(纯称重)";
                    }
                }
                SocketHelper.SendMsg(Car.ToJson(), IP, Port);
                //车辆放行
                AddCarpermission(baseurl, key, sign, op.Platenumber, op.DriverName, op.DriverTel, parkNames);
                return Success("操作成功。");
            }
            else
            {//危化品车辆
                HazardouscarBLL hbll = new HazardouscarBLL();
                HazardouscarEntity ha = hbll.GetEntity(keyvalue);
                ha.State = state;
                ha.InTime = DateTime.Now;
                hbll.Update(keyvalue, ha);
                CarAlgorithmEntity Car = new CarAlgorithmEntity();
                Car.CarNo = ha.CarNo;
                Car.GPSID = ha.GPSID;
                Car.GPSName = ha.GPSNAME;
                Car.ID = ha.ID;
                Car.Type = 5;
                Car.State = 0;
                Car.LineName = ha.HazardousName;
                Car.GoodsName = ha.HazardousName;
                SocketHelper.SendMsg(Car.ToJson(), IP, Port);
                //车辆放行
                CarIn(baseurl, key, sign, ha.CarNo, ha.Phone, ha.Dirver);
                //人员添加出入权限
                var baselist = CarUserbll.addUserJurisdiction(keyvalue, state, baseurl, key, sign);
                return Content(baselist.ToJson());
            }
        }

        /// <summary>
        /// 拒绝入场
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult Refuse(string keyvalue, int type)
        {
            if (type == 0)
            {//拜访车辆（有车）
                VisitcarEntity car = visitcarbll.GetEntity(keyvalue);
                car.State = 99;
                visitcarbll.SaveForm(keyvalue, car);
            }
            else if (type == 3)
            {//拜访人员
                CarUserEntity car = CarUserbll.GetEntity(keyvalue);
                car.State = 99;
                CarUserbll.SaveForm(keyvalue, car, null);
            }
            else if (type == 1)
            {//物料车
                OperticketmanagerBLL obll = new OperticketmanagerBLL();
                OperticketmanagerEntity op = obll.GetEntity(keyvalue);
                op.ExamineStatus = 99;
                obll.SaveForm(keyvalue, op);
            }
            else
            {//2危化品车
                HazardouscarBLL hbll = new HazardouscarBLL();
                HazardouscarEntity ha = hbll.GetEntity(keyvalue);
                ha.State = 99;
                hbll.Update(keyvalue, ha);
            }
            return Success("操作成功。");
        }



        #endregion

        #region 海康平台数据提交

        /// <summary>
        /// 给车辆添加进入停车场的权限
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="key"></param>
        /// <param name="sign"></param>
        /// <param name="CarNo"></param>
        /// <param name="Phone"></param>
        /// <param name="UserName"></param>
        /// <param name="parkName"></param>
        public void AddCarpermission(string Url, string key, string sign, string CarNo, string Phone, string UserName, string parkName = "1号岗")
        {
            #region 检查车辆在海康平台是否存在
            var selectmodel = new
            {
                pageNo = 1,
                pageSize = 100,
                plateNo = CarNo
            };
            var existsVehicleStr = SocketHelper.LoadCameraList(selectmodel, Url, "/artemis/api/resource/v1/vehicle/advance/vehicleList", key, sign);
            dynamic existsVehicle = JsonConvert.DeserializeObject<ExpandoObject>(existsVehicleStr);
            #endregion
            var parkmodel = new
            {
                parkIndexCodes = ""
            };

            string parkMsg = SocketHelper.LoadCameraList(parkmodel, Url, "/artemis/api/resource/v1/park/parkList", key, sign);
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
                        mark = "物料车",
                        parkIndexCode = string.Join(",", pakindex),
                        startTime = DateTime.Now.ToString("yyyy-MM-dd"),
                        endTime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")
                    };
                    SocketHelper.LoadCameraList(new List<dynamic>() { addModel }, Url, "/artemis/api/v1/vehicle/addVehicle", key, sign);
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
                        mark = "物料车",
                        parkIndexCode = string.Join(",", pakindex),
                        startTime = DateTime.Now.ToString("yyyy-MM-dd"),
                        endTime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"),
                        isUpdateFunction = 1
                    };
                    string updateMsg = SocketHelper.LoadCameraList(new List<dynamic>() { updateModel }, Url, "/artemis/api/v1/vehicle/updateVehicle", key, sign);
                }
                #endregion
            }
        }


        /// <summary>
        /// 将外来车辆加入到海康预约接口使其可以进场
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="key"></param>
        /// <param name="sign"></param>
        /// <param name="CarNo"></param>
        /// <param name="Phone"></param>
        /// <param name="UserName"></param>
        /// <param name="parkName">停车场名称</param>
        public void CarIn(string Url, string key, string sign, string CarNo, string Phone, string UserName, string parkName = "1号岗")
        {
            var ckmodel = new
            {
                parkIndexCodes = ""
            };
            string parkMsg = SocketHelper.LoadCameraList(ckmodel, Url, "/artemis/api/resource/v1/park/parkList", key, sign);
            parkList pl = JsonConvert.DeserializeObject<parkList>(parkMsg);
            if (pl != null && pl.data != null && pl.data.Count > 0)
            {
                #region 车辆预约进入
                string[] parkNames = parkName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string pname in parkNames)
                {
                    //允许进出次数
                    int inOutTimes = 1;
                    if (pname == "1号岗")
                        inOutTimes = 0;
                    var model = new
                    {
                        parkSyscode = pl.data.FirstOrDefault(x => x.parkName.Contains(pname))?.parkIndexCode,
                        plateNo = CarNo,
                        phoneNo = Phone,
                        owner = UserName,
                        allowTimes = inOutTimes,
                        isCharge = "0",
                        resvWay = "6",
                        startTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "+08:00",
                        endTime = DateTime.Now.AddHours(1).ToString("yyyy-MM-ddTHH:mm:ss") + "+08:00"
                    };
                    SocketHelper.LoadCameraList(model, Url, "/artemis/api/pms/v2/parkingSpace/reservations/addition", key, sign);
                }
                #endregion
            }
        }

        /// <summary>
        /// 将外来车辆加入到海康预约接口使其可以进场
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="key"></param>
        /// <param name="sign"></param>
        /// <param name="CarNo"></param>
        public void HikOut(string Url, string key, string sign, string CarNo)
        {
            #region 出厂先查账单
            var PreBillmodel = new
            {
                parkSyscode = "",
                plateNo = CarNo
            };
            string PreBillMsg = SocketHelper.LoadCameraList(PreBillmodel, Url, "/artemis/api/pms/v1/pay/quickPreBill", key, sign);
            #endregion
            PreBill pb = JsonConvert.DeserializeObject<PreBill>(PreBillMsg);
            if (pb != null && pb.code == "0" && pb.data != null)
            {
                #region 允许放行则虚拟付款允许出厂
                var receipt = new
                {
                    billSyscode = pb.data.billSyscode
                };
                string receiptMsg = SocketHelper.LoadCameraList(receipt, Url, "/artemis/api/pms/v2/pay/receipt", key, sign);

                receiptdata re = JsonConvert.DeserializeObject<receiptdata>(receiptMsg);
                #endregion
            }

            #region 删除车辆进出权限
            if (!string.IsNullOrEmpty(CarNo))
            {
                var selectmodel = new
                {
                    pageNo = 1,
                    pageSize = 1000,
                    plateNo = CarNo
                };
                var existsVehicleStr = SocketHelper.LoadCameraList(selectmodel, Url, "/artemis/api/resource/v1/vehicle/advance/vehicleList", key, sign);
                dynamic existsVehicle = JsonConvert.DeserializeObject<dynamic>(existsVehicleStr);
                List<dynamic> vechileList = new List<dynamic>();

                if (existsVehicle.code == "0" && existsVehicle.data.total > 0)
                {
                    foreach (dynamic obj in existsVehicle.data.list)
                    {
                        vechileList.Add(obj.vehicleId);
                        break;
                    }
                }

                var delModel = new
                {
                    vehicleIds = vechileList
                };
                SocketHelper.LoadCameraList(delModel, Url, "/artemis/api/resource/v1/vehicle/batch/delete", key, sign);
            }
            #endregion

        }

        #endregion

    }
}
