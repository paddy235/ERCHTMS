using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BSFramework.Util;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.BaseManage;
using BSFramework.Application.Entity;
using System.Data;
using ERCHTMS.Code;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Busines.LllegalManage;

namespace ERCHTMS.Web.Areas.LllegalManage.Controllers
{
    /// <summary>
    /// 违章统计
    /// </summary>
    public class LllegalStatisticsController : MvcControllerBase
    {

        private LllegalStatisticsBLL legbll = new LllegalStatisticsBLL();
        private UserBLL userbll = new UserBLL(); //用户操作对象
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult TrendIndex()
        {
            return View();
        }


        [HttpGet]
        public ActionResult CompareIndex()
        {
            return View();
        }
       
        #region 判断人员是否公司
        /// <summary>
        /// 判断人员是否公司
        /// </summary>
        /// <returns></returns>
        public ActionResult QueryCurUser()
        {
            string result = "";

            var curUser = new OperatorProvider().Current(); //当前用户

            string hidPlantLevel = dataitemdetailbll.GetItemValue("HidPlantLevel");

            string hidOrganize = dataitemdetailbll.GetItemValue("HidOrganize");

            string CompanyRole = hidPlantLevel + "," + hidOrganize;


            var userList = userbll.GetUserListByDeptCode(curUser.DeptCode, CompanyRole, false, curUser.OrganizeId).Where(p => p.UserId == curUser.UserId).ToList();
            //当前用户是厂级
            if (userList.Count() > 0)
            {
                result = "1";
            }
            else
            {
                result = "0";
            }

            return Content(result);
        }
        #endregion

        #region 获取统计信息
        /// <summary>
        /// 获取违章数量统计
        /// </summary>
        /// <returns></returns>
        public ActionResult QueryLllegalNumberPie(string queryJson)
        {           
            var jsonData = new
            {
                legLevalTotal = legbll.GetLllegalLevelTotal(queryJson),
                legLevalList = legbll.GetLllegalLevelList(queryJson),
                legTypeTotal = legbll.GetLllegalTypeTotal(queryJson),
                legTypeList = legbll.GetLllegalTypeList(queryJson)
            };
            return Content(jsonData.ToJson());
        }
        /// <summary>
        /// 获取违章趋势统计
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult QueryLllegalNumberLine(string queryJson)
        {
            var dt = legbll.GetLllegalTrendData(queryJson);
            var jsonData = new
            {
                lineTotal = GetLllegalTrendTotal(dt,queryJson),
                lineList = dt
            };
            return Content(jsonData.ToJson());
        }
        /// <summary>
        /// 获取违章对比统计
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult QueryLllegalNumberColumn(string queryJson)
        {
            var dt = legbll.GetLllegalCompareData(queryJson);
            List<string> arr = new List<string>();
            foreach(DataRow dr in dt.Rows)
            {
                arr.Add(dr["fullname"].ToString());
            }
            var list = GetLllegalCompareTotal(dt, queryJson);
            var jsonData = new
            {
                columns= arr,
                columnTotal = list,
                columnList = dt
            };
            return Content(jsonData.ToJson());
        }
        #endregion

        #region 协助方法
        private List<object> GetLllegalTrendTotal(DataTable dt,string queryJson)
        {
            var list = new List<dynamic>();

            if (dt != null && dt.Rows.Count > 0)
            {
                var colors = new Dictionary<string, string>()
                {
                    {"一般违章","#558ED5" },
                    {"较严重违章","#FFC000" },
                    {"严重违章","#FF0000" },
                    {"合计","#000000" }
                };
                var select = dt.Select();
                var queryParam = queryJson.ToJObject();
                var groups = queryParam["levelGroups"].ToString();
                var grpList = groups.Split(new char[] { ',' });
                int[] total = new int[dt.Rows.Count];
                for (var i = 0; i < grpList.Length; i++)
                {
                    var grpName = grpList[i];
                    List<int> data = new List<int>();
                    for(int j=0;j< dt.Rows.Count;j++)
                    {
                        DataRow dr = dt.Rows[j];
                        int num = int.Parse(dr[grpName].ToString());
                        total[j] += num;
                        data.Add(num);
                    }
                    list.Add(new { name = grpName,color= GetColor(colors,grpName), data = data });
                }
                var totalName = "合计";
                list.Add(new { name = totalName, color = GetColor(colors,totalName), data = total });
            }

            return list;
        }
        private List<dynamic> GetLllegalCompareTotal(DataTable dt, string queryJson)
        {
            var list = new List<dynamic>();

            if (dt != null && dt.Rows.Count > 0)
            {
                var colors = new Dictionary<string, string>()
                {
                    {"一般违章","#558ED5" },
                    {"较严重违章","#FFC000" },
                    {"严重违章","#FF0000" }
                };
                var select = dt.Select();
                var queryParam = queryJson.ToJObject();
                var groups = queryParam["levelGroups"].ToString();
                var grpList = groups.Split(new char[] { ',' });                
                for (var i = 0; i < grpList.Length; i++)
                {
                    var grpName = grpList[i];
                    List<int> data = new List<int>();
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        DataRow dr = dt.Rows[j];
                        int num = int.Parse(dr[grpName].ToString());                       
                        data.Add(num);
                    }
                    list.Add(new { name = grpName, color = GetColor(colors,grpName), data = data });
                }               
            }

            return list;
        }
        private string GetColor(Dictionary<string, string> dic, string key)
        {
            Random rnd = new Random((int)DateTime.Now.Ticks);
            string r = string.Format("RGB({0}, {1}, {2})", rnd.Next(255), rnd.Next(255), rnd.Next(255));//默认随机颜色

            if (dic.ContainsKey(key))
                r = dic[key];

            return r;
        }
        #endregion
    }
}
