using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.HseManage.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
   public  class HseKeyValue
    {
        public HseKeyValue()
        {

        }
        public HseKeyValue(string key=null , decimal value =0)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }
        public decimal Value { get; set; }

        /// <summary>
        /// 填写预警指标卡的数量
        /// </summary>
        public decimal  Num1 { get; set; }
        /// <summary>
        /// 人均填写预警指标卡的数量
        /// </summary>
        public decimal Num2 { get; set; }
        /// <summary>
        /// 安全观察项总数
        /// </summary>
        public decimal Num3 { get; set; }
        /// <summary>
        /// 有风险的观察项
        /// </summary>
        public decimal Num4 { get; set; }
        /// <summary>
        /// 安全比 
        /// </summary>
        public decimal Num5 { get; set; }

        public string DeptId { get; set; }

        public string Str1 { get; set; }

        /// <summary>
        ///  生成实体                    
        /// </summary>
        /// <param name="dataType">
        /// 1：一到十二个月的Key
        /// 2：只生成1到当前年的上一个月(如果当前月为一月，则生成空List实体)
        /// </param>
        /// <returns></returns>
        public List<HseKeyValue> InitData(int dataType)
        {
            List<HseKeyValue> data = new List<HseKeyValue>();
            switch (dataType)
            {
                case 1:
                    data.Add(new HseKeyValue("一月份", 0));
                    data.Add(new HseKeyValue("二月份", 0));
                    data.Add(new HseKeyValue("三月份", 0));
                    data.Add(new HseKeyValue("四月份", 0));
                    data.Add(new HseKeyValue("五月份", 0));
                    data.Add(new HseKeyValue("六月份", 0));
                    data.Add(new HseKeyValue("七月份", 0));
                    data.Add(new HseKeyValue("八月份", 0));
                    data.Add(new HseKeyValue("九月份", 0));
                    data.Add(new HseKeyValue("十月份", 0));
                    data.Add(new HseKeyValue("十一月份", 0));
                    data.Add(new HseKeyValue("十二月份", 0));
                    break;
                case 2:
                    int thisMonth =1 ;
                    int nowMnth = DateTime.Now.Month;
                    do {
                        data.Add(new HseKeyValue(Enum.GetName(typeof(MothName), thisMonth)));
                        thisMonth++;
                    }
                    while (thisMonth < nowMnth);
                    break;
            }
            return data;
        }

    }
    /// <summary>
    /// 各部门参与度用
    /// </summary>
    public  class HseKeyValueParameter
    {
        /// <summary>
        /// 顶级部门ID
        /// </summary>
        public string RootId { get; set; }
        /// <summary>
        /// 顶级部门的名称
        /// </summary>
        public string RootName { get; set; }
        /// <summary>
        /// 要搜索的部门Id
        /// </summary>
        public List<string> DeptIds { get; set; }
    }
    /// <summary>
    /// 月份枚举
    /// </summary>
    public enum MothName
    {
        一月份=1,
        二月份 = 2,
        三月份 = 3,
        四月份 = 4,
        五月份 = 5,
        六月份 = 6,
        七月份 = 7,
        八月份 = 8,
        九月份 = 9,
        十月份 = 10,
        十一月份 = 11,
        十二月份 = 12
    }

    /// <summary>
    /// 部门月份数据
    /// </summary>
    public class DeptMonthData
    {
        public DeptMonthData()
        {
            this.MonthData = new List<HseKeyValue>();
         
        }
        /// <summary>
        /// 根据年份，生成月份数据
        /// 当前年：生成的月份到上月的数据 MonthData ,
        /// 小于当前年：生成1到12个月的月份数据 MonthData是十二条数据
        /// </summary>
        /// <param name="Year"></param>
        public DeptMonthData(string Year,bool HasMonth=false)
        {
            this.MonthData = new List<HseKeyValue>();
            int thisMonth = 1;
            int nowMonth = 13;
            if (string.IsNullOrEmpty(Year) || Year == DateTime.Now.Year.ToString())
            {
                nowMonth = HasMonth ? DateTime.Now.Month + 1 : DateTime.Now.Month;
            }
            do
            {
                this.MonthData.Add(new HseKeyValue(Enum.GetName(typeof(MothName), thisMonth)));
                thisMonth++;
            }
            while (thisMonth < nowMonth);
        }
        public string DeptName { get; set; }

        public List<HseKeyValue> MonthData  { get; set; }

        public string Ttile { get; set; }
        public List<int> DataNum { get; set; }


    }

    public class HseEvaluateKv
    {
        public HseEvaluateKv()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="v1">已完成</param>
        /// <param name="v2">帽贴</param>
        /// <param name="v3">未完成</param>
        public HseEvaluateKv(string key,int v1=0,int v2=0 ,int v3=0, int v4 = 0, int v5 = 0)
        {
            this.Key = key;
            this.V1 = v1;
            this.V2 = v2;
            this.V3 = v3;
            this.V4 = v4;
            this.V5 = v5;
        }
        public string Key { get; set; }
        /// <summary>
        /// 完成
        /// </summary>
        public int V1 { get; set; }
        /// <summary>
        /// 帽贴
        /// </summary>
        public int V2 { get; set; }
        /// <summary>
        /// 未完成
        /// </summary>
        public int V3 { get; set; }
        public int V4 { get; set; }
        public int V5 { get; set; }
        /// <summary>
        /// 应参加人数
        /// </summary>
        public List<HseEvaluateKv> InitData()
        {
            List<HseEvaluateKv> hseEvaluateKvs = new List<HseEvaluateKv>();
            hseEvaluateKvs.Add(new HseEvaluateKv()
            {
                Key = "DGPX"
            });
            hseEvaluateKvs.Add(new HseEvaluateKv()
            {
                Key = "QZDZPX"
            });
            hseEvaluateKvs.Add(new HseEvaluateKv()
            {
                Key = "CNJDCPX"
            });
            hseEvaluateKvs.Add(new HseEvaluateKv()
            {
                Key = "GLSPX"
            });
            hseEvaluateKvs.Add(new HseEvaluateKv()
            {
                Key = "YLRQ"
            });
            hseEvaluateKvs.Add(new HseEvaluateKv()
            {
                Key = "GLZYPX"
            });
            hseEvaluateKvs.Add(new HseEvaluateKv()
            {
                Key = "DHZYPX"
            });
            hseEvaluateKvs.Add(new HseEvaluateKv()
            {
                Key = "JSJZYPX"
            });
            hseEvaluateKvs.Add(new HseEvaluateKv()
            {
                Key = "GKZYPX"
            });
            hseEvaluateKvs.Add(new HseEvaluateKv()
            {
                Key = "JJPX"
            });
            hseEvaluateKvs.Add(new HseEvaluateKv()
            {
                Key = "NONEPX"
            });
            return hseEvaluateKvs;
        }
    }
}
