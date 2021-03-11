using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.OccupationalHealthManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.SystemManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.OccupationalHealthManage.Controllers
{
    public class StaffListController : MvcControllerBase
    {

        private OccupationalstaffdetailBLL occupationalstaffdetailbll = new OccupationalstaffdetailBLL();
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /OccupationalHealthManage/StaffList/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /OccupationalHealthManage/StaffList/Create

        public ActionResult Create()
        {
            return View();
        }

        #region 获取数据
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>   
        //[HandlerMonitor(3, "分页查询用户信息!")]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "OCCDETAILID";
            pagination.p_fields = "USERID,USERNAME,GENDER,DEPTNAME,DUTYNAME,ISSICK,SICKTYPE,SICKTYPENAME";
            pagination.p_tablename = "V_STAFFDETAILLIST";
            pagination.conditionJson = "ISSICK=1";
            pagination.sidx = "INSPECTIONTIME desc ,Usernamepinyin ";
            pagination.sord = "asc";

            ERCHTMS.Code.Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = " ISSICK=1";
            }
            else
            {
                string where = new ERCHTMS.Busines.AuthorizeManage.AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
                pagination.conditionJson += " and " + where;
            }

            var data = occupationalstaffdetailbll.GetStaffListPage(pagination, queryJson);
            var jsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return ToJsonResult(jsonData);
        }

        /// <summary>
        /// 获取总数量
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns> 
        public int GetSum(string queryJson)
        {
            string wheresql = "";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                wheresql = "";
            }
            else
            {
                string where = new ERCHTMS.Busines.AuthorizeManage.AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
                wheresql += " and " + where;
            }

            int sum = occupationalstaffdetailbll.GetStaffListSum(queryJson, wheresql);
            return sum;
        }
        #endregion
        //
        // POST: /OccupationalHealthManage/StaffList/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /OccupationalHealthManage/StaffList/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /OccupationalHealthManage/StaffList/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /OccupationalHealthManage/StaffList/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /OccupationalHealthManage/StaffList/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// 导出到Excel
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult Excel(string queryJson)
        {
            //string column = "IDNUM,OCCDETAILID,USERNAME,USERNAMEPINYIN,INSPECTIONTIME,ISSICK,SICKTYPE";
            //string stringcolumn = "ISSICK,SICKTYPE";
            //string[] columns = column.Split(',');
            //string[] stringcolumns = stringcolumn.Split(',');

            string wheresql = "";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                wheresql = "";
            }
            else
            {
                string where = new ERCHTMS.Busines.AuthorizeManage.AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
                wheresql += " and " + where;
            }

            DataTable dt = occupationalstaffdetailbll.GetStaffList(queryJson, wheresql);
            //DataTable Newdt = AsposeExcelHelper.UpdateDataTable(dt, columns, stringcolumns);//把所有字段转换成string 方便修改

            ////先获取职业病数据
            //DataItemBLL di = new DataItemBLL();
            ////先获取到字典项
            //DataItemEntity DataItems = di.GetEntityByCode("SICKTYPE");
            //DataItemDetailBLL did = new DataItemDetailBLL();
            ////根据字典项获取值
            //IEnumerable<DataItemDetailEntity> didList = did.GetList(DataItems.ItemId);

            //循环修改信息
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i][0] = i + 1;
                //if (Convert.ToInt32(dt.Rows[i]["ISSICK"]) == 1)
                //{
                //    foreach (DataItemDetailEntity item in didList)
                //    {
                //        if (item.ItemValue == dt.Rows[i]["SICKTYPE"].ToString())
                //        {
                //            dt.Rows[i]["SICKTYPE"] = item.ItemName;
                //        }
                //    }
                //}
                //else
                //{
                //    dt.Rows[i]["SICKTYPE"] = "";
                //}



            }

            string FileUrl = @"\Resource\ExcelTemplate\职业病人员清单_导出模板.xlsx";

            AsposeExcelHelper.ExecuteResult(dt, FileUrl, "职业病人员清单", "职业病人员列表");

            return Success("导出成功。");
        }
    }
}
