using ERCHTMS.Entity.LllegalManage;
using ERCHTMS.IService.LllegalManage;
using ERCHTMS.Service.LllegalManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Service.HiddenTroubleManage;
using ERCHTMS.IService.HiddenTroubleManage;
using System.Linq;
using BSFramework.Util;

namespace ERCHTMS.Busines.LllegalManage
{
    /// <summary>
    /// 描 述：违章基本信息
    /// </summary>
    public class LllegalStatisticsBLL
    {
        private LllegalStatisticsIService service = new LllegalStatisticsService();

        #region 统计图



        public List<object> GetLllegalLevelTotal(string queryJson)
        {
            var list = new List<dynamic>();
            var colors = new Dictionary<string, string>()
            {
                {"一般违章","#558ED5" },
                {"较严重违章","#FFC000" },
                {"严重违章","#FF0000" }
            };
            var dt = service.GetLllegalLevelTotal(queryJson);
            for (var i = 0; i < dt.Columns.Count; i++)
            {
                var colName = dt.Columns[i].ColumnName;
                if (i == 0)
                    list.Add(new { name = colName, color = GetColor(colors, colName), y = int.Parse(dt.Rows[0][i].ToString()), sliced = true, selected = true });
                else
                    list.Add(new { name = colName, color = GetColor(colors, colName), y = int.Parse(dt.Rows[0][i].ToString()) });
            }

            if (list.All(x => x.y == 0))
                list.Clear();

            return list;
        }
        public List<object> GetLllegalLevelTotalGrp(string queryJson)
        {
            var list = new List<dynamic>();
            var colors = new Dictionary<string, string>()
            {
                {"一般违章","#558ED5" },
                {"较严重违章","#FFC000" },
                {"严重违章","#FF0000" }
            };
            var dt = service.GetLllegalLevelTotalGrp(queryJson);
            for (var i = 0; i < dt.Columns.Count; i++)
            {
                var colName = dt.Columns[i].ColumnName;
                if (i == 0)
                    list.Add(new { name = colName, color = GetColor(colors, colName), y = int.Parse(dt.Rows[0][i].ToString()), sliced = true, selected = true });
                else
                    list.Add(new { name = colName, color = GetColor(colors, colName), y = int.Parse(dt.Rows[0][i].ToString()) });
            }

            if (list.All(x => x.y == 0))
                list.Clear();

            return list;
        }

        public DataTable GetLllegalZgv(string deptcode)
        {
            var dt = service.GetLllegalZgv(deptcode);
            return dt;
        }
        public DataTable GetLllegalLevelList(string queryJson)
        {
            var dt = service.GetLllegalLevelList(queryJson);

            /*if (dt.Rows.Count > 0)
            {
                int num1 = 0, num2 = 0, num3 = 0;
                foreach (DataRow r in dt.Rows)
                {
                    num1 += int.Parse(r[3].ToString());
                    num2 += int.Parse(r[4].ToString());
                    num3 += int.Parse(r[5].ToString());
                }
                var totalRow = dt.NewRow();
                totalRow["fullname"] = "合计";
                totalRow[dt.Columns[3].ColumnName] = num1;
                totalRow[dt.Columns[4].ColumnName] = num2;
                totalRow[dt.Columns[5].ColumnName] = num3;
                dt.Rows.Add(totalRow);
            }*/

            return dt;
        }
        public DataTable GetLllegalLevelListGrp(string queryJson)
        {
            var dt = service.GetLllegalLevelListGrp(queryJson);

            /*if (dt.Rows.Count > 0)
            {
                int num1 = 0, num2 = 0, num3 = 0;
                foreach (DataRow r in dt.Rows)
                {
                    num1 += int.Parse(r[3].ToString());
                    num2 += int.Parse(r[4].ToString());
                    num3 += int.Parse(r[5].ToString());
                }
                var totalRow = dt.NewRow();
                totalRow["fullname"] = "合计";
                totalRow[dt.Columns[3].ColumnName] = num1;
                totalRow[dt.Columns[4].ColumnName] = num2;
                totalRow[dt.Columns[5].ColumnName] = num3;
                dt.Rows.Add(totalRow);
            }*/

            return dt;
        }
        public List<object> GetLllegalTypeTotal(string queryJson)
        {
            var list = new List<dynamic>();
            var colors = new Dictionary<string, string>()
            {
                {"作业类","#C0504D" },
                {"管理类","#98B65A" },
                {"指挥类","#8064A2" },
                {"装置类","#4F81BD" },
                {"文明卫生类","#F6EC08" }
            };
            var dt = service.GetLllegalTypeTotal(queryJson);
            for (var i = 0; i < dt.Columns.Count; i++)
            {
                var colName = dt.Columns[i].ColumnName;
                if (i == 0)
                    list.Add(new { name = colName, color = GetColor(colors, colName), y = int.Parse(dt.Rows[0][i].ToString()), sliced = true, selected = true });
                else
                    list.Add(new { name = colName, color = GetColor(colors, colName), y = int.Parse(dt.Rows[0][i].ToString()) });
            }

            if (list.All(x => x.y == 0))
                list.Clear();

            return list;
        }
        public List<object> GetLllegalTypeTotalGrp(string queryJson)
        {
            var list = new List<dynamic>();
            var colors = new Dictionary<string, string>()
            {
                {"作业类","#C0504D" },
                {"管理类","#98B65A" },
                {"指挥类","#8064A2" },
                {"装置类","#4F81BD" },
                {"文明卫生类","#F6EC08" }
            };
            var dt = service.GetLllegalTypeTotalGrp(queryJson);
            for (var i = 0; i < dt.Columns.Count; i++)
            {
                var colName = dt.Columns[i].ColumnName;
                if (i == 0)
                    list.Add(new { name = colName, color = GetColor(colors, colName), y = int.Parse(dt.Rows[0][i].ToString()), sliced = true, selected = true });
                else
                    list.Add(new { name = colName, color = GetColor(colors, colName), y = int.Parse(dt.Rows[0][i].ToString()) });
            }

            if (list.All(x => x.y == 0))
                list.Clear();

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
        public DataTable GetLllegalTypeList(string queryJson)
        {
            return service.GetLllegalTypeList(queryJson);
        }
        public DataTable GetLllegalTypeListGrp(string queryJson)
        {
            return service.GetLllegalTypeListGrp(queryJson);
        }
        #endregion

        #region 趋势统计图
        public DataTable GetLllegalTrendData(string queryJson)
        {
            var dt = service.GetLllegalTrendData(queryJson);

            return dt;
        }
        public DataTable GetLllegalTrendDataGrp(string queryJson)
        {
            var dt = service.GetLllegalTrendDataGrp(queryJson);

            return dt;
        }

        public DataTable GetJFStatis(string queryJson) {
            var dt = service.GetJFStatis(queryJson);

            return dt;
        }
        #endregion

        #region 对比统计图
        public DataTable GetLllegalCompareData(string queryJson)
        {
            var dt = service.GetLllegalCompareData(queryJson);

            return dt;
        }
        public DataTable GetLllegalCompareDataGrp(string queryJson)
        {
            var dt = service.GetLllegalCompareDataGrp(queryJson);

            return dt;
        }
        #endregion


        #region 移动端违章统计
        /// <summary>
        /// 移动端违章统计
        /// </summary>
        /// <param name="code"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public DataTable GetAppLllegalStatistics(string code,string year, int mode)
        {
            try
            {
                return service.GetAppLllegalStatistics(code,year, mode);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        
        #region  获取前几个曝光的数据(取当前年的)
        /// <summary>
        /// 获取前几个曝光的数据(取当前年的)
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public DataTable QueryExposureLllegal(string num) 
        {
            try
            {
                return service.QueryExposureLllegal(num); 
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}