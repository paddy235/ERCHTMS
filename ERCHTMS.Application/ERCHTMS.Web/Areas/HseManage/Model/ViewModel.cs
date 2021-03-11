using ERCHTMS.Entity.BaseManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ERCHTMS.Web.Areas.HseManage.Model
{
    /// <summary>
    /// X轴数据
    /// </summary>
    public class Xseries
    {
        public string name { get; set; }
        public decimal y { get; set; }
        public string drilldown { get; set; }
    }
    /// <summary>
    /// 下钻数据
    /// </summary>
    public class Series
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<Xseries> data { get; set; }
    }
    public class ViewModel
    {

        public ViewModel()
        {
            Xseriess = new List<Xseries>();
            Seriess = new List<Series>();
        }
        /// <summary>
        /// X轴数据
        /// </summary>
        public List<Xseries> Xseriess { get; set; }
        /// <summary>
        /// 下钻数据
        /// </summary>
        public List<Series> Seriess{ get; set; }

        /// <summary>
        /// 组装下钻型条形图所需的数据
        /// </summary>
        /// <param name="deptList">部门表</param>
        /// <param name="dt">各部门参与率统计数据</param>
        /// <param name="userList">用户列表</param>
        /// <param name="OrgId">组织Id</param>
        /// <param name="IsSystem">是否是系统管理员，系统管理员只生成一级数据,非系统管理员生成部门加班组级数据</param>
        /// <returns></returns>
        public void InitData(List<DepartmentEntity> deptList, DataTable dt, string OrgId,IEnumerable<UserEntity> userList,bool IsSystem = false)
        {
                var depts = deptList.Where(p => p.ParentId == OrgId).OrderBy(p => p.EnCode).ToList();
                if (depts !=null && depts.Count>0)
                {
                    depts.ForEach(dept => {
                        Xseries xseries = new Xseries();
                        var drItem = dt.Select(" DEPTCODE LIKE '" + dept.EnCode + "%'");
                        xseries.name = dept.FullName;
                        xseries.y = GetNum(drItem, userList, dept.EnCode);
                        if (!IsSystem)
                        {
                            //系统管理员只找一级数据
                            //非系统管理员一级为部门级，再查一个班组级


                            var childDept = deptList.Where(p => p.ParentId == dept.DepartmentId).OrderBy(p => p.EnCode).ToList();
                            if (childDept != null && childDept.Count > 0)
                            {
                                xseries.drilldown = dept.DepartmentId;
                                Series series = new Series();
                                series.name = dept.FullName;
                                series.id = dept.DepartmentId;
                                series.data = new List<Xseries>();
                                childDept.ForEach(child =>
                                {
                                    var drchild = dt.Select(" DEPTCODE LIKE '" + child.EnCode + "%'");
                                    Xseries childInfo = new Xseries()
                                    {
                                        name = child.FullName,
                                        y = GetNum(drchild, userList, child.DeptCode),
                                        drilldown = child.DepartmentId,
                                    };
                                    series.data.Add(childInfo);
                                });
                                this.Seriess.Add(series);
                            }
                        }
                        this.Xseriess.Add(xseries);
                    });
                }
        }
        private decimal GetNum(DataRow[] drItem, IEnumerable<UserEntity> userList, string deptCode)
        {
            decimal userCount = userList.Count(x => x.DepartmentCode.StartsWith(deptCode));    //总人数
            if (userCount < 1) return 0;
            decimal allCount = 0;
            foreach (DataRow dr in drItem)
            {
                allCount += dr["COUNT"] == null ? 0 : Convert.ToDecimal(dr["COUNT"]);    //已提交总数
                                                                                         //参与度=已提交/应提交*100%
            }
            return Math.Round(allCount / userCount * 100, 2);//百分比
        }
    }

    /// <summary>
    /// 树形表格数据结构
    /// </summary>
    public struct TreeGird
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        public string DeptId { get; set; }
        public string DeptCode { get; set; }
        /// <summary>
        /// 已提交人数
        /// </summary>
        public string SubmitCount { get; set; }
        /// <summary>
        /// 应提交人数
        /// </summary>
        public string AllUserCount { get; set; }
        /// <summary>
        /// 未提交人数
        /// </summary>
        public string NotSubmitCount { get; set; }
        /// <summary>
        /// 参与率
        /// </summary>
        public string CYD { get; set; }

        public string Nature { get; set; }
    }
}