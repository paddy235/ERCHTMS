using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.MatterManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.SystemManage;

namespace ERCHTMS.Web.Areas.CarManage.Controllers
{
    public class MaterialCarController : MvcControllerBase
    {
        private OperticketmanagerBLL operticketmanagerbll = new OperticketmanagerBLL();
        //
        // GET: /CarManage/MaterialCar/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Form()
        {
            return View();
        }

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

            pagination.p_kid = "Id";
            pagination.p_fields = "createuserid,createdate,examinestatus,platenumber,GetData,outdate,drivername,drivertel,transporttype,producttype,dress,staytime,status,NVL(vnum,0) vnum";
            pagination.p_tablename = " ( select Id,createuserid,createdate,examinestatus,platenumber,GetData,outdate,drivername,drivertel,transporttype,producttype,dress,staytime,status, vnum from WL_OPERTICKETMANAGER  wl left join (select count(cid) vnum ,cid from BIS_CARVIOLATION  group by cid  )  cv on wl.id=cv.cid and wl.isdelete=1 ) v1";
            pagination.conditionJson = " 1=1 ";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            //if (user.IsSystem)
            //{
            pagination.conditionJson = "1=1";
            //}
            var data = operticketmanagerbll.BackGetPageList(pagination, queryJson);

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
        /// 获取物料类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetWlType()
        {
            List<DataItemDetailEntity> ddlist = new List<DataItemDetailEntity>();
            DataItemDetailEntity d1 = new DataItemDetailEntity();
            d1.ItemName = "提货";
            d1.ItemValue = "提货";
            DataItemDetailEntity d2 = new DataItemDetailEntity();
            d2.ItemName = "转运";
            d2.ItemValue = "转运";
            ddlist.Add(d1);
            ddlist.Add(d2);
            return Content(ddlist.ToJson());
        }

        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = operticketmanagerbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #endregion
	}
}