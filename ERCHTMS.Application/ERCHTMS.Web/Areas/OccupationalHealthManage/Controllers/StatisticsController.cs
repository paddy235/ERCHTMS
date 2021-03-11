using ERCHTMS.Busines.OccupationalHealthManage;
using ERCHTMS.Entity.OccupationalHealthManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.OccupationalHealthManage.Controllers
{
    public class StatisticsController : MvcControllerBase
    {
        private OccupationalstaffdetailBLL occupationalstaffdetailbll = new OccupationalstaffdetailBLL();
        #region 视图
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult HazardIndex()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 根据年份获取职业病种类统计数据
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSickType(string year)
        {

            string wheresql = "";
            ERCHTMS.Code.Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                wheresql = "";
            }
            else
            {
                string where = new ERCHTMS.Busines.AuthorizeManage.AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
                wheresql +=  where;
            }
            //获取基础统计数据
            DataTable dt = occupationalstaffdetailbll.NewGetStatisticsUserTable(year, wheresql);
            List<OccStatisticsEntity> stlist = new List<OccStatisticsEntity>();
            int Sum = 0;
            foreach (DataRow dr in dt.Rows)
            {
                //先获取DataTable中数据
                OccStatisticsEntity st = new OccStatisticsEntity();
                st.Sicktype = dr[0].ToString();
                st.SickUserNum = Convert.ToInt32(dr[1]);
                st.SickValue = dr[2].ToString();
                Sum += Convert.ToInt32(dr[1]); //获取总和
                stlist.Add(st);
            }
            //求每条数据的比例
            for (int i = 0; i < stlist.Count; i++)
            {
                stlist[i].Proportion = string.Format("{0:0.00%}", Convert.ToDouble(stlist[i].SickUserNum) / Convert.ToDouble(Sum));
            }
            OccStatisticsEntity occ = new OccStatisticsEntity();
            occ.Sicktype = "合计";
            occ.SickUserNum = Sum;
            occ.Proportion = "100%";
            occ.SickValue = "";
            stlist.Add(occ);

            return ToJsonResult(stlist);

        }


        /// <summary>
        /// 根据年份获取部门职业病统计数据
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDeptSick(string year)
        {
            string wheresql = "";
            ERCHTMS.Code.Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                wheresql = "";
            }
            else
            {
                string where = new ERCHTMS.Busines.AuthorizeManage.AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
                wheresql += where;
            }
            //获取基础数据
            DataTable dt = occupationalstaffdetailbll.GetStatisticsDeptTable(year,wheresql);
            List<OccStatisticsEntity> stlist = new List<OccStatisticsEntity>();
            int Sum = 0;
            foreach (DataRow dr in dt.Rows)
            {
                //先获取DataTable中数据
                OccStatisticsEntity st = new OccStatisticsEntity();
                st.Sicktype = dr[0].ToString();
                st.SickUserNum = Convert.ToInt32(dr[1]);
                st.SickValue = dr[2].ToString();
                Sum += Convert.ToInt32(dr[1]); //获取总和
                stlist.Add(st);
            }
            //求每条数据的比例
            for (int i = 0; i < stlist.Count; i++)
            {
                stlist[i].Proportion = string.Format("{0:0.00%}", Convert.ToDouble(stlist[i].SickUserNum) / Convert.ToDouble(Sum));
            }
            OccStatisticsEntity occ = new OccStatisticsEntity();
            occ.Sicktype = "合计";
            occ.SickUserNum = Sum;
            occ.Proportion = "100%";
            occ.SickValue = "";
            stlist.Add(occ);

            return ToJsonResult(stlist);

        }

        /// <summary>
        /// 根据年份获取职业病统计数据
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetYearSick(string year, string dept)
        {
            string wheresql = "";
            ERCHTMS.Code.Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                wheresql = "";
            }
            else
            {
                string where = new ERCHTMS.Busines.AuthorizeManage.AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
                wheresql += where;
            }

            int yearNum = Convert.ToInt32(year);
            int NowYear = DateTime.Now.Year;
            //获取已有年份数据
            DataTable dt = occupationalstaffdetailbll.GetStatisticsYearTable(yearNum, dept,wheresql);
            List<OccStatisticsEntity> stlist = new List<OccStatisticsEntity>();
            int Sum = 0;
            int dtRow = 0;
            if (yearNum != 0)
            {
                //根据近X年 进行数据补全
                for (int i = 1; i <= yearNum; i++)
                {
                    //验证是否有这一年数据
                    if (dtRow < dt.Rows.Count)
                    {
                        int y = Convert.ToInt32(dt.Rows[dtRow][0]);
                        if (y == NowYear - (yearNum - i))   //如果有这个年份直接添加
                        {
                            //先获取DataTable中数据
                            OccStatisticsEntity st = new OccStatisticsEntity();
                            st.Sicktype = dt.Rows[dtRow][0].ToString();
                            st.SickUserNum = Convert.ToInt32(dt.Rows[dtRow][1]);
                            stlist.Add(st);
                            dtRow++;
                        }
                        else
                        {
                            //先获取DataTable中数据
                            OccStatisticsEntity st = new OccStatisticsEntity();
                            st.Sicktype = (NowYear - (yearNum - i)).ToString();
                            st.SickUserNum = 0;
                            stlist.Add(st);
                        }
                    }
                    else
                    {
                        //先获取DataTable中数据
                        OccStatisticsEntity st = new OccStatisticsEntity();
                        st.Sicktype = (NowYear - i).ToString();
                        st.SickUserNum = 0;
                        stlist.Add(st);
                    }

                }
            }
            else 
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //先获取DataTable中数据
                    OccStatisticsEntity st = new OccStatisticsEntity();
                    st.Sicktype = dt.Rows[i][0].ToString();
                    st.SickUserNum = Convert.ToInt32(dt.Rows[i][1]);
                    stlist.Add(st);
                }
            }

            return ToJsonResult(stlist);

        }

        /// <summary>
        /// 根据年份获取职业病统计图表数据
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetYearImageSick(string year, string dept)
        {

            string wheresql = "";
            ERCHTMS.Code.Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                wheresql = "";
            }
            else
            {
                string where = new ERCHTMS.Busines.AuthorizeManage.AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
                wheresql += where;
            }

            List<string> yValues = new List<string>();
            List<object> dic = new List<object>();
            List<int> num = new List<int>();
            int yearNum = Convert.ToInt32(year);
            int NowYear = DateTime.Now.Year;
            //获取已有年份数据
            DataTable dt = occupationalstaffdetailbll.GetStatisticsYearTable(yearNum, dept,wheresql);
            List<OccStatisticsEntity> stlist = new List<OccStatisticsEntity>();
            int dtRow = 0;
            if (yearNum != 0)
            {
                //根据近X年 进行数据补全
                for (int i = 1; i <= yearNum; i++)
                {
                    //验证是否有这一年数据
                    if (dtRow < dt.Rows.Count)
                    {
                        int y = Convert.ToInt32(dt.Rows[dtRow][0]);
                        if (y == NowYear - (yearNum - i))   //如果有这个年份直接添加
                        {
                            //先获取DataTable中数据
                            yValues.Add(dt.Rows[dtRow][0].ToString());
                            num.Add(Convert.ToInt32(dt.Rows[dtRow][1]));
                            dtRow++;
                        }
                        else
                        {
                            yValues.Add((NowYear - (yearNum - i)).ToString());
                            num.Add(0);
                        }
                    }
                    else
                    {
                        yValues.Add((NowYear - (yearNum - i)).ToString());
                        num.Add(0);
                    }

                }
            }
            else 
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //先获取DataTable中数据
                    yValues.Add(dt.Rows[i][0].ToString());
                    num.Add(Convert.ToInt32(dt.Rows[i][1]));
                    dtRow++;
                }
            }
            dic.Add(new { name = "职业病数量", data = num });

            return Newtonsoft.Json.JsonConvert.SerializeObject(new { x = dic, y = yValues });
        }

        /// <summary>
        /// 获取职业病危害因素监测统计数据
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetHazardSick(string year, string risk)
        {

            string wheresql = "";
            ERCHTMS.Code.Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                wheresql = "";
            }
            else
            {
                string where = new ERCHTMS.Busines.AuthorizeManage.AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
                wheresql += " and " + where;
            }

            HazarddetectionBLL habll = new HazarddetectionBLL();
            int years = Convert.ToInt32(year);

            DataTable dt = habll.GetStatisticsHazardTable(years, risk, true, wheresql);//全部统计数据
            DataTable dtCb = habll.GetStatisticsHazardTable(years, risk, false, wheresql);//超标统计数据
            List<OccStatisticsEntity> stlist = new List<OccStatisticsEntity>();
            int[] Month = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };//先设定月份
            int Sum = 0;
            int CbSum = 0;
            int dtRow = 0;//第几行数据
            int dtCbRow = 0;

            for (int i = 0; i < Month.Length; i++)
            {
                //验证是否有这一年数据
                if (dtRow < dt.Rows.Count)
                {
                    int mon = Convert.ToInt32(dt.Rows[dtRow][0]);
                    if (mon == Month[i])   //如果有这个年份直接添加
                    {
                        //先获取DataTable中数据
                        OccStatisticsEntity st = new OccStatisticsEntity();
                        st.Sicktype = Convert.ToInt32(dt.Rows[dtRow][0]).ToString();
                        st.SickUserNum = Convert.ToInt32(dt.Rows[dtRow][1]);
                        if (dtCbRow < dtCb.Rows.Count)
                        {
                            int monCb = Convert.ToInt32(dtCb.Rows[dtCbRow][0]);
                            if (monCb == Month[i])
                            {
                                st.SickValue = dtCb.Rows[dtCbRow][1].ToString();
                                CbSum += Convert.ToInt32(dtCb.Rows[dtCbRow][1]);
                                dtCbRow++;
                            }
                            else
                            {
                                st.SickValue = "0";
                            }
                        }
                        else
                        {
                            st.SickValue = "0";
                        }
                        Sum += Convert.ToInt32(dt.Rows[dtRow][1]);
                        stlist.Add(st);
                        dtRow++;
                    }
                    else
                    {
                        OccStatisticsEntity st = new OccStatisticsEntity();
                        st.Sicktype = Month[i].ToString();
                        st.SickUserNum = 0;
                        st.SickValue = "0";
                        stlist.Add(st);
                    }
                }
                else
                {
                    OccStatisticsEntity st = new OccStatisticsEntity();
                    st.Sicktype = Month[i].ToString();
                    st.SickUserNum = 0;
                    st.SickValue = "0";
                    stlist.Add(st);
                }

            }

            OccStatisticsEntity Hj = new OccStatisticsEntity();
            Hj.Sicktype = "合计";
            Hj.SickUserNum = Sum;
            Hj.SickValue = CbSum.ToString();
            stlist.Add(Hj);
            //遍历修改其比例和合计
            for (int i = 0; i < stlist.Count; i++)
            {
                if (stlist[i].SickValue != "0")
                {
                    stlist[i].Proportion = string.Format("{0:0.00%}", Convert.ToDouble(stlist[i].SickValue) / Convert.ToDouble(stlist[i].SickUserNum));
                }
                else
                {
                    stlist[i].Proportion = "0.00%";
                }
            }



            return ToJsonResult(stlist);

        }


        /// <summary>
        /// 获取职业病危害因素监测统计数据(图标)
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetHazardSickImage(string year, string risk)
        {
            string wheresql = "";
            ERCHTMS.Code.Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                wheresql = "";
            }
            else
            {
                string where = new ERCHTMS.Busines.AuthorizeManage.AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
                wheresql += " and " + where;
            }

            List<string> yValues = new List<string>();
            List<object> dic = new List<object>();
            List<int> num = new List<int>();
            List<int> CBnum = new List<int>();
            List<double> Cblist = new List<double>();

            HazarddetectionBLL habll = new HazarddetectionBLL();
            int years = Convert.ToInt32(year);
            DataTable dt = habll.GetStatisticsHazardTable(years, risk, true, wheresql);//全部统计数据
            DataTable dtCb = habll.GetStatisticsHazardTable(years, risk, false, wheresql);//超标统计数据
            int[] Month = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };//先设定月份
            int dtRow = 0;//第几行数据
            int dtCbRow = 0;

            for (int i = 0; i < Month.Length; i++)
            {
                yValues.Add(Month[i].ToString());
                //验证是否有这一年数据
                if (dtRow < dt.Rows.Count)
                {
                    int mon = Convert.ToInt32(dt.Rows[dtRow][0]);
                    if (mon == Month[i])   //如果有这个年份直接添加
                    {
                        int Sum = Convert.ToInt32(dt.Rows[dtRow][1]);
                        num.Add(Sum);
                        if (dtCbRow < dtCb.Rows.Count)
                        {
                            int monCb = Convert.ToInt32(dtCb.Rows[dtCbRow][0]);
                            if (monCb == Month[i])
                            {
                                int Cb = Convert.ToInt32(dtCb.Rows[dtCbRow][1]);
                                CBnum.Add(Cb);
                                double c = Convert.ToDouble(string.Format("{0:0.00}", (Convert.ToDouble(Cb) / Sum) * 100));
                                Cblist.Add(c);
                                dtCbRow++;
                            }
                            else
                            {
                                CBnum.Add(0);
                                Cblist.Add(0);
                            }
                        }
                        else
                        {
                            CBnum.Add(0);
                            Cblist.Add(0);
                        }
                        dtRow++;
                    }
                    else
                    {
                        num.Add(0);
                        CBnum.Add(0);
                        Cblist.Add(0);
                    }
                }
                else
                {
                    num.Add(0);
                    CBnum.Add(0);
                    Cblist.Add(0);
                }

            }

            dic.Add(new { name = "超标数量", type = "column", yAxis = 1, data = CBnum });
            dic.Add(new { name = "监测数量", type = "column", yAxis = 1, data = num });
            dic.Add(new { name = "超标率%", type = "spline", yAxis = 0, data = Cblist });//, tooltip = "{valueSuffix: ' %'}"

            return Newtonsoft.Json.JsonConvert.SerializeObject(new { x = dic, y = yValues });

        }

        /// <summary>
        /// 返回年份下拉框数据
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetYearCmbList(string year, string dept)
        {
            List<ComboxEntity> cmbList = new List<ComboxEntity>();
            ComboxEntity cmb = new ComboxEntity();
            cmb.itemName = "近5年";
            cmb.itemValue = "5";

            ComboxEntity cmb2 = new ComboxEntity();
            cmb2.itemName = "近10年";
            cmb2.itemValue = "10";
           

            ComboxEntity cmb3 = new ComboxEntity();
            cmb3.itemName = "全部";
            cmb3.itemValue = "0";

            cmbList.Add(cmb);
            cmbList.Add(cmb2);
            cmbList.Add(cmb3);
            return ToJsonResult(cmbList);
        }

        #endregion
    }
}
