using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ERCHTMS.AppSerivce.Model;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Cache;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ERCHTMS.Busines.EmergencyPlatform;
using ERCHTMS.Entity.EmergencyPlatform;
using System.Linq.Expressions;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ERCHTMS.Busines.AuthorizeManage;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.SystemManage;

namespace ERCHTMS.AppSerivce.Controllers
{
    public class SuppliesController : BaseApiController
    {

        private UserBLL userbll = new UserBLL();
        private SuppliesBLL suppliesbll = new SuppliesBLL();
        private InoroutrecordBLL Inoroutrecordbll = new InoroutrecordBLL();
        private DataItemCache dataItemCache = new DataItemCache();

        #region 获取应急物资清单
        /// <summary>
        /// 获取公司机构
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetSuppliesList([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            string userid = dy.userid;
            string keyword = dy.keyword ?? "";
            //获取用户基本信息
            OperatorProvider.AppUserId = userid;  //设置当前用户

            Operator curUser = OperatorProvider.Provider.Current();
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            string roleNames = curUser.RoleName;
            //分页获取数据
            Pagination pagination = new Pagination();
            pagination.page = int.Parse(dy.page ?? "1");
            pagination.rows = int.Parse(dy.rows ?? "1");
            pagination.p_kid = "ID";
            pagination.p_fields = "SuppliesCode,SuppliesTypeName,SuppliesName,SuppliesType,Num,SuppliesUntil,SuppliesUntilName";
            pagination.p_tablename = "V_mae_supplies t";
            pagination.sidx = "CreateDate";
            pagination.sord = "desc";
            pagination.conditionJson = "CREATEUSERORGCODE='" + curUser.OrganizeCode + "'";
            if (keyword.Length > 0)
            {
                pagination.conditionJson += string.Format("and (SUPPLIESTYPENAME like '%{0}%' or SUPPLIESNAME like '%{1}%' or DEPARTNAME like '%{2}%')", keyword, keyword, keyword);
            }
            var data = suppliesbll.GetPageList(pagination, "");
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
            };
            return new { code = 0, info = "获取数据成功", data = JsonData };

        }


        #endregion

        #region 获取应急物资详情
        /// <summary>
        /// 获取公司机构
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetSuppliesDetial([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            string userid = dy.userid;
            string id = dy.SuppliesId ?? "";
            //获取用户基本信息
            OperatorProvider.AppUserId = userid;  //设置当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            //string roleNames = curUser.RoleName;

            var entity = suppliesbll.GetEntity(id);
            return new
            {
                code = 0,
                info = "获取数据成功",
                data = new
                {
                    SuppliesId = entity.ID,
                    SuppliesCode = entity.SUPPLIESCODE,
                    SuppliesType = entity.SUPPLIESTYPE,
                    SuppliesTypeName = entity.SUPPLIESTYPENAME,
                    SuppliesName = entity.SUPPLIESNAME,
                    Num = entity.NUM,
                    SuppliesUnitName = entity.SUPPLIESUNTILNAME,
                    SuppliesUnit = entity.SUPPLIESUNTIL,
                    StoragePlace = entity.STORAGEPLACE,
                    DepartmentId = entity.DEPARTID,
                    DepartmentName = entity.DEPARTNAME,
                    UserId = entity.USERID,
                    UserName = entity.USERNAME,
                    Mobile = entity.MOBILE,
                    CreateTime = entity.CREATEDATE,
                    MainFun = entity.MAINFUN,
                    Files = entity.ID
                }
            };
        }

        #endregion

        #region 获取应急物资出入库记录
        /// <summary>
        /// 获取公司机构
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetSuppliesInorOutRecord([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            string userid = dy.userid;
            string id = dy.SuppliesId ?? "";
            //获取用户基本信息
            OperatorProvider.AppUserId = userid;  //设置当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            Pagination pagination = new Pagination();
            pagination.page = int.Parse(dy.page ?? "1");
            pagination.rows = int.Parse(dy.rows ?? "1");
            pagination.p_kid = "ID";
            pagination.p_fields = " SuppliesId,InOrOutTime,UserId,UserName,Num,StatuName";
            pagination.p_tablename = "MAE_INOROUTRECORD t";
            pagination.conditionJson = "1=1";
            pagination.conditionJson += string.Format(" and SUPPLIESID='{0}'", id);
            pagination.sidx = "CreateDate";
            pagination.sord = "desc";
            var data = Inoroutrecordbll.GetPageList(pagination, "");
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
            };
            return new { code = 0, info = "获取数据成功", data = JsonData };
        }

        #endregion

        #region 新增应急物资
        /// <summary>
        /// 新增应急物资
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object AddSupplies([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userid = dy.userid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                var entity = new SuppliesEntity
                {
                    SUPPLIESCODE = suppliesbll.GetMaxCode(),
                    SUPPLIESTYPE = dy.SuppliesType,
                    SUPPLIESTYPENAME = dy.SuppliesTypeName,
                    SUPPLIESNAME = dy.SuppliesName,
                    NUM = int.Parse(dy.Num.ToString()),
                    SUPPLIESUNTILNAME = dy.SuppliesUnitName,
                    SUPPLIESUNTIL = dy.SuppliesUnit,
                    STORAGEPLACE = dy.StoragePlace,
                    DEPARTID = dy.DepartmentId,
                    DEPARTNAME = dy.DepartmentName,
                    USERID = dy.ChargeUserId,
                    USERNAME = dy.ChargeUserName,
                    MOBILE = dy.Mobile,
                    CREATEDATE = dy.CreateTime,
                    MAINFUN = dy.MainFun,
                    FILES = dy.Files,

                };
                suppliesbll.SaveForm("", entity);
                var entityInorOut = new InoroutrecordEntity
                {
                    USERID = entity.USERID,
                    USERNAME = entity.USERNAME,
                    DEPARTID = entity.DEPARTID,
                    DEPARTNAME = entity.DEPARTNAME,
                    INOROUTTIME = DateTime.Now,
                    SUPPLIESCODE = entity.SUPPLIESCODE,
                    SUPPLIESTYPE = entity.SUPPLIESTYPE,
                    SUPPLIESTYPENAME = entity.SUPPLIESTYPENAME,
                    SUPPLIESNAME = entity.SUPPLIESNAME,
                    SUPPLIESUNTIL = entity.SUPPLIESUNTIL,
                    SUPPLIESUNTILNAME = entity.SUPPLIESUNTILNAME,
                    NUM = entity.NUM,
                    STORAGEPLACE = entity.STORAGEPLACE,
                    MOBILE = entity.MOBILE,
                    SUPPLIESID = entity.ID,
                    STATUS = 0
                };
                var inoroutrecordbll = new InoroutrecordBLL();
                inoroutrecordbll.SaveForm("", entityInorOut);
            }
            catch (Exception)
            {

                return new { code = -0, count = 0, info = "保存失败" };
            }
            return new { code = 0, count = 0, info = "保存成功" };
        }
        #endregion

        #region 编辑应急物资
        /// <summary>
        /// 编辑应急物资
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object EditSupplies([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userid = dy.userid;
                string id = dy.id ?? "";
                //获取用户基本信息
                OperatorProvider.AppUserId = userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string roleNames = curUser.RoleName;
                var entity = new SuppliesEntity
                {
                    SUPPLIESCODE = dy.suppliescode,
                    SUPPLIESTYPE = dy.suppliestype,
                    SUPPLIESTYPENAME = dy.suppliestypename,
                    MOBILE = dy.mobile,
                    DEPARTID = dy.departid,
                    DEPARTNAME = dy.departname,
                    USERID = dy.userid,
                    USERNAME = dy.username,
                    MAINFUN = dy.mianfun,
                    STORAGEPLACE = dy.storageplace,
                    SUPPLIESUNTIL = dy.suppliesuntil,
                    NUM = dy.num

                };
                suppliesbll.SaveForm(id, entity);
            }
            catch (Exception)
            {

                return new { code = -0, count = 0, info = "保存失败" };
            }
            return new { code = 0, count = 0, info = "保存成功" };
        }
        #endregion

        #region 出入库登记
        /// <summary>
        /// 应急物资入库登记
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object AddSuppliesInOrOutRecode([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userid = dy.userid;
                string id = dy.SuppliesId ?? "";
                //获取用户基本信息
                OperatorProvider.AppUserId = userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                var entityInorOut = new InoroutrecordEntity
                {
                    SUPPLIESCODE = dy.SuppliesCode,
                    SUPPLIESTYPE = dy.SuppliesType,
                    SUPPLIESTYPENAME = dy.SuppliesTypeName,
                    SUPPLIESNAME = dy.SuppliesName,
                    NUM = int.Parse(dy.Num.ToString()),
                    SUPPLIESUNTILNAME = dy.SuppliesUnitName,
                    SUPPLIESUNTIL = dy.SuppliesUnit,
                    STORAGEPLACE = dy.StoragePlace,
                    DEPARTID = dy.DepartmentId,
                    DEPARTNAME = dy.DepartmentName,
                    USERID = dy.ChargeUserId,
                    USERNAME = dy.ChargeUserName,
                    MOBILE = dy.Mobile,
                    SUPPLIESID = dy.SuppliesId,
                    INOROUTTIME = dy.InOrOutTime,

                    STATUS = int.Parse(dy.Status),
                    STATUNAME = dy.StatusName,
                };
                if (dy.Status == "1")
                {
                    entityInorOut.OUTREASON = dy.OutReason;
                    entityInorOut.OUTREASONNAME = dy.OutReasonName;
                }
                var inoroutrecordbll = new InoroutrecordBLL();
                inoroutrecordbll.SaveForm("", entityInorOut);
                //修改对应的数据
                var entity = suppliesbll.GetEntity(id);
                if (dy.Status == "1")
                    entity.NUM = entity.NUM - int.Parse(dy.Num.ToString());
                if (dy.Status == "2")
                    entity.NUM = entity.NUM + int.Parse(dy.Num.ToString());
                suppliesbll.SaveForm(id, entity);
            }
            catch (Exception)
            {

                return new { code = -0, count = 0, info = "保存失败" };
            }
            return new { code = 0, count = 0, info = "保存成功" };
        }
        #endregion

        #region 获取字典值
        /// <summary>
        /// 应急物资入库登记
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetDataItemListJson([FromBody]JObject json)
        {
            var list = new List<DataItemModel>();
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userid = dy.userid;
                string EnCode = dy.enCode ?? "";
                //获取用户基本信息
                OperatorProvider.AppUserId = userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                list = dataItemCache.GetDataItemList(EnCode).ToList();
            }
            catch (Exception)
            {

                return new { code = -0, count = 0, info = "获取数据失败" };
            }

            return new { code = 0, info = "获取数据成功", data = list };
        }

        public class ReturnListDataItem
        {

            public string ItemName { get; set; }

            public string ItemValue { get; set; }
        }

        #endregion
    }
}
