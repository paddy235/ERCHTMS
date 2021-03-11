using ERCHTMS.Code;
using BSFramework.Util;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ERCHTMS.Busines.RiskDatabase;
using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;
using System.Collections.Generic;
using System.Data;
using System.Web;
using BSFramework.Util.Offices;
using System;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.SystemManage;
using BSFramework.Util.Attributes;
using ERCHTMS.Entity.RiskDataBaseConfig;
using ERCHTMS.Busines.RiskDataBaseConfig;
using ERCHTMS.Busines.AuthorizeManage;
using System.Linq.Expressions;

namespace ERCHTMS.Web.Areas.RiskDatabase.Controllers
{
    /// <summary>
    /// 描 述：危险点管理
    /// </summary>
    public class DangerSourceController : MvcControllerBase
    {
        private DangerSourceBLL dangerBLL = new DangerSourceBLL();
        private DistrictBLL districtBLL = new DistrictBLL();
        private RiskwayconfigBLL riskwayconfigbll = new RiskwayconfigBLL();
        private RiskwayconfigdetailBLL riskwayconfigdetailbll = new RiskwayconfigdetailBLL();
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private PostBLL postbll = new PostBLL();

        #region 视图功能
        /// <summary>
        /// 危险点管理
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 风险清单汇总
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult SumList()
        {
            return View();
        }
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult SumItem()
        {
            return View();
        }
        /// <summary>
        /// 危险点表单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult Form()
        {
            return View();
        }
        /// <summary>
        /// 风险清单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult List()
        {
            return View();
        }
        /// <summary>
        /// 详细信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult Detail()
        {
            return View();
        }
        /// <summary>
        /// 导入风险库
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult Import()
        {
            return View();
        }
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult ImportList()
        {
            return View();
        }
        /// <summary>
        /// 选择风险库
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult Select()
        {
            return View();
        }
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult SelectNew()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 区域列表 
        /// </summary>
        /// <param name="value">当前主键</param>
        /// <returns>返回树形Json</returns>
        [HttpPost]
        public ActionResult GetTreeJson(string value)
        {
            var filterdata = districtBLL.GetList().ToList();
            StringBuilder sb = new StringBuilder();
            List<object> list = new List<object>();
            foreach (DistrictEntity item in filterdata)
            {
                List<object> listDangers = new List<object>();
                List<DangerSourceEntity> ds = dangerBLL.GetList(item.DistrictID, "").ToList();
                foreach (DangerSourceEntity danger in ds)
                {
                    List<DangerSourceEntity> children = dangerBLL.GetList(danger.Id, "").ToList();

                    listDangers.Add(new { Id = danger.Id, Name = danger.Name, ParentId = item.ParentID, DeptCode = danger.DeptCode, DeptName = danger.DeptName, Nodes = children });
                }
                list.Add(new
                {
                    Id = item.DistrictID,
                    Name = item.DistrictName,
                    ParentId = item.ParentID,
                    DeptCode = item.ChargeDeptCode,
                    DeptName = item.ChargeDept,
                    Nodes = listDangers
                });

            }
            return Content(list.ToJson());
        }
        /// <summary>
        ///  获取区域列表
        /// </summary>
        /// <param name="parentId">上级节点ID</param>
        /// <returns>返回树形Json</returns>
        [HttpGet]
        public ActionResult GetAreaTreeJson(string parentId = "0")
        {
            var data = dangerBLL.GetList(parentId);
            var treeList = new List<TreeEntity>();
            foreach (DangerSourceEntity item in data)
            {
                TreeEntity tree = new TreeEntity();
                bool hasChildren = data.Count(t => t.ParentId == item.Id) == 0 ? false : true;
                tree.id = item.Id;
                tree.text = item.Name.Replace("\\", "╲").Replace("\\n", "").Replace("\\r", "");
                tree.value = item.Id;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChildren;
                tree.parentId = item.ParentId;
                treeList.Add(tree);
            }
            return Content(treeList.TreeToJson(parentId));
        }
        [HttpPost]
        public string GetPostTreeJson(string deptName)
        {
            return dangerBLL.GetOptionsStringForInitPost(deptName);
        }
        [HttpPost]
        public string GetInitAreaTreeJson(string parentId, string orgCode)
        {
            return dangerBLL.GetOptionsStringForArea(parentId, orgCode);
        }
        [HttpGet]
        public ActionResult GetTreeJson1(string organizeId, string id = "0", string ids = "")
        {
            List<DistrictEntity> list = districtBLL.GetList().Where(t => t.ParentID != "-1").ToList();
            List<DistrictEntity> data = new List<DistrictEntity>();
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                data = list.Where(a => a.ParentID == id && a.OrganizeId == user.OrganizeId).ToList();
            }

            if (!string.IsNullOrEmpty(ids))
            {
                data = list.Where(t => ids.Contains(t.DistrictID)).ToList();
                //if(id=="0")
                //{
                List<DistrictEntity> data1 = list.Where(t => ids.Contains(t.DistrictID)).ToList();
                foreach (DistrictEntity entity in data1)
                {
                    string codes = "";
                    for (int j = 6; j < entity.DistrictCode.Length; j += 3)
                    {
                        codes += entity.DistrictCode.Substring(0, j) + ",";
                    }
                    List<DistrictEntity> data2 = list.Where(t => codes.Contains(t.DistrictCode)).ToList();
                    foreach (DistrictEntity item in data2)
                    {
                        if (data.Where(t => t.DistrictCode == item.DistrictCode).Count() == 0)
                        {
                            DistrictEntity de = list.Where(t => t.DistrictCode == item.DistrictCode).FirstOrDefault();
                            if (!data.Contains(de))
                            {
                                data.Add(de);
                            }
                        }
                    }

                }
                //}
                //else
                //{
                //    data = list.Where(a =>a.ParentID == id && ids.Contains(a.DistrictID)).ToList();
                //}
            }

            data = data.OrderBy(a => a.DistrictCode).ThenBy(a => a.SortCode).ToList();
            var treeList = new List<TreeEntity>();
            foreach (DistrictEntity item in data)
            {
                TreeEntity tree = new TreeEntity();
                bool hasChildren = list.Count(t => t.ParentID == item.DistrictID) == 0 ? false : true;
                tree.id = item.DistrictID;
                tree.text = item.DistrictName;
                tree.value = item.DistrictCode;
                tree.isexpand = false;
                tree.complete = !hasChildren;
                tree.hasChildren = hasChildren;
                tree.Attribute = "code";
                tree.AttributeValue = item.DistrictCode;
                tree.AttributeA = "initAreaIds";
                tree.AttributeValueA = item.LinkToCompanyID;
                tree.parentId = item.ParentID;
                treeList.Add(tree);
            }
            return Content(treeList.TreeToJson(id));
        }
        /// <summary>
        /// 风险点或作业内容列表
        /// </summary>
        /// <param name="value">当前主键</param>
        /// <param name="keyword">关键字查询</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string value, string keyword)
        {
            string parentId = value == null ? "0" : value;
            var data = dangerBLL.GetList(parentId, keyword).ToList();
            return Content(data.ToJson());
        }
        [HttpGet]
        public ActionResult GetPageSumListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "id";
            pagination.p_fields = "risktype,riskname,districtname,areacode,gradeval,'' fl,'' unit,'' man,'' num";
            pagination.p_tablename = "v_risklist";
            pagination.conditionJson = "status=1 and deletemark=0 and enabledmark=0";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.conditionJson = "deptcode like '"+user.OrganizeCode+"%'";
            try
            {
                RiskAssessBLL riskassessbll = new RiskAssessBLL();
                var data = riskassessbll.GetPageList(pagination, queryJson);
                DepartmentBLL deptBll=new DepartmentBLL();
                foreach (DataRow dr in data.Rows)
                {
                    string sql = string.Format("select levelname,deptname,dutyperson from BIS_RISKASSESS where districtname='{0}' and risktype='{1}'", dr["districtname"].ToString(), dr["risktype"].ToString());
                    if (dr["risktype"].ToString()=="作业")
                    {
                        sql += string.Format(" and worktask='{0}'", dr["riskname"].ToString());
                    }
                    if (dr["risktype"].ToString() == "设备")
                    {
                        sql += string.Format(" and equipmentname='{0}'", dr["riskname"].ToString());
                    }
                    if (dr["risktype"].ToString() == "区域")
                    {
                        sql += string.Format(" and HjSystem='{0}'", dr["riskname"].ToString());
                    }
                    if (dr["risktype"].ToString() == "管理")
                    {
                        sql += string.Format(" and dangersource='{0}'", dr["riskname"].ToString());
                    }
                    if (dr["risktype"].ToString() == "岗位")
                    {
                        sql += string.Format(" and JobName='{0}'", dr["riskname"].ToString());
                    }
                    if (dr["risktype"].ToString() == "工器具及危化品")
                    {
                        sql += string.Format(" and ToolOrDanger='{0}'", dr["riskname"].ToString());
                    }
                    DataTable dtItems = deptBll.GetDataTable(sql);
                    List<string> list = new List<string>();
                    foreach(DataRow dr1 in dtItems.Rows)
                    {
                       ;
                        foreach (string str in dr1["levelname"].ToString().Split(','))
                        {
                            if (!list.Contains(str))
                            {
                                list.Add(str);
                            }
                        }
                    }
                    dr["fl"]=string.Join(",",list.ToArray());
                    list = new List<string>();
                    foreach (DataRow dr1 in dtItems.Rows)
                    {
                        ;
                        foreach (string str in dr1["deptname"].ToString().Split(','))
                        {
                            if (!list.Contains(str))
                            {
                                list.Add(str);
                            }
                        }
                    }
                    dr["unit"] = string.Join(",", list.ToArray());
                    list = new List<string>();
                    foreach (DataRow dr1 in dtItems.Rows)
                    {
                        ;
                        foreach (string str in dr1["dutyperson"].ToString().Split(','))
                        {
                            if (!list.Contains(str))
                            {
                                list.Add(str);
                            }
                        }
                    }
                    dr["man"] = string.Join(",", list.ToArray());
                    dr["num"] = dtItems.Rows.Count;
                }
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
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson, string mode = "")
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "id";
            pagination.p_fields = "areaid,worktask,dangersource,riskdesc,description,itemr,grade,accidentname,result,deptname,createdate,measure";
            pagination.p_tablename = "BIS_RISKDATABASE";
            pagination.conditionJson = "deletemark=0 and risktype='作业'";

            var data = dangerBLL.GetPageList(pagination, queryJson);
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
        /// 区域列表（主要是绑定下拉框）
        /// </summary>
        /// <param name="parentId">节点Id</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetAreaListJson(string parentId)
        {
            var data = dangerBLL.GetList(parentId == null ? "0" : parentId);
            return Content(data.ToJson());
        }
        /// <summary>
        /// 区域实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = dangerBLL.GetEntity(keyValue);
            return Content(data.ToJson());
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除区域
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerAuthorize(PermissionMode.Enforce)]
        [HandlerMonitor(6, "删除危险点信息")]
        public ActionResult RemoveForm(string keyValue, string planId)
        {
            if (string.IsNullOrEmpty(keyValue))
            {
                return Error("参数不能为空！");
            }
            DangerSourceEntity entity = dangerBLL.GetEntity(keyValue);
            if (entity == null)
            {
                return Error("该记录已经不存在！");
            }
            if (!string.IsNullOrEmpty(planId))
            {
                entity.OtherId = planId;
                entity.EnabledMark = 1;
                dangerBLL.SaveForm(keyValue, entity);
            }
            else
            {
                entity.DeleteMark = 1;
                dangerBLL.RemoveForm(keyValue);
            }

            return Success("删除成功。");
        }
        /// <summary>
        /// 保存区域表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="areaEntity">区域实体</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "保存危险点(新增、修改)")]
        public ActionResult SaveForm(string keyValue, DangerSourceEntity areaEntity)
        {
            dangerBLL.SaveForm(keyValue, areaEntity);
            return Success("操作成功。");
        }
        /// <summary>
        /// 保存部门与内置部门的风险配置清单信息
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="deptName">部门名称</param>
        /// <param name="newDeptName">关联的内置部门名称，多个用英文逗号分隔</param>
        /// <param name="postName">关联的岗位名称，多个用英文逗号分隔</param>
        /// <param name="newPostName">岗位名称</param>
        /// <param name="postId">岗位Id</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "保存部门与内置部门的风险配置清单信息")]
        public ActionResult SaveConfig(string deptCode, string deptName, string newDeptName, string postName, string newPostName, string postId, string deptId = "")
        {
            DepartmentBLL deptBll = new DepartmentBLL();
            DepartmentEntity dept = deptBll.GetEntity(deptId);
            if (dept != null)
            {
                dept.RelatedDeptName = newDeptName;
                deptBll.SaveForm(deptId, dept);
            }
            int result = dangerBLL.SaveConfig(deptCode, deptName, newDeptName, postName, newPostName, postId, OperatorProvider.Provider.Current());

            return result > 0 ? Success("操作成功。") : Error("配置关系失败");
        }
        /// <summary>
        /// 导入风险库
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(5, "导入风险库")]
        public string ImportRisk()
        {
            int error = 0;
            string message = "请选择格式正确的文件再导入!";
            StringBuilder sb = new StringBuilder("具体错误位置：");
            int count = HttpContext.Request.Files.Count;
            if (count > 0)
            {
                HttpPostedFileBase file = HttpContext.Request.Files[0];
                if (string.IsNullOrEmpty(file.FileName))
                {
                    return message;
                }
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName));
                DataTable dt = ExcelHelper.ExcelImport(Server.MapPath("~/Resource/temp/" + fileName));
                for (int j = 2; j < dt.Rows.Count; j++)
                {
                    //区域名称
                    string areaName = dt.Rows[j][2].ToString();
                    //风险点名称
                    //string dangerName = dt.Rows[j][3].ToString();
                    //操作项目名称
                    //string itemName = dt.Rows[j][4].ToString();
                    //获取作业步骤
                    //string workName = dt.Rows[j][5].ToString();
                    if (string.IsNullOrEmpty(areaName))
                    {
                        continue;
                    }
                    try
                    {
                        //获取区域信息并判断该区域是否存在，没有则新增该区域
                        string areaId = dangerBLL.GetIdByName(areaName);
                        if (string.IsNullOrEmpty(areaId))
                        {
                            //新增区域信息
                            DangerSourceEntity ds = new DangerSourceEntity();
                            areaId = ds.Id = Guid.NewGuid().ToString();
                            ds.Name = areaName;
                            ds.DataType = -1; //设置内置的区域数据类型为-1
                            ds.ParentId = "0";
                            dangerBLL.SaveForm("", ds);
                        }
                        ////获取风险点信息并判断该风险点是否存在，没有则新增该风险点信息
                        //string dangerId = dangerBLL.GetIdByName(dangerName);
                        //if (string.IsNullOrEmpty(dangerId))
                        //{
                        //    //新增风险点信息
                        //    DangerSourceEntity ds = new DangerSourceEntity();
                        //    dangerId = ds.Id = Guid.NewGuid().ToString();
                        //    ds.Name = dangerName;
                        //    ds.DataType = 0;
                        //    ds.ParentId = areaId;
                        //    ds.OtherId = areaId;
                        //    dangerBLL.SaveForm("", ds);
                        //}
                        //string itemId = dangerBLL.GetIdByName(itemName);
                        //if (string.IsNullOrEmpty(itemId))
                        //{
                        //    //新增操作项目
                        //    DangerSourceEntity ds = new DangerSourceEntity();
                        //    itemId = ds.Id = Guid.NewGuid().ToString();
                        //    ds.Name = itemName;
                        //    ds.DataType =1;
                        //    ds.ParentId = dangerId;
                        //    ds.OtherId = areaId;
                        //    dangerBLL.SaveForm("", ds);
                        //}

                        ////获取作业步骤信息并判断该作业步骤是否存在，没有则新增该作业步骤信息
                        //string workId = dangerBLL.GetIdByName(workName);
                        //if (string.IsNullOrEmpty(workId))
                        //{
                        //    //新增作业步骤信息
                        //    DangerSourceEntity ds = new DangerSourceEntity();
                        //    workId = ds.Id = Guid.NewGuid().ToString();
                        //    ds.Name = workName;
                        //    ds.DataType = 2;
                        //    ds.ParentId = itemId;
                        //    ds.OtherId = areaId;
                        //    dangerBLL.SaveForm("", ds);
                        //}
                        //岗位
                        string postName = dt.Rows[j][0].ToString();
                        //部门班组
                        string deptName = dt.Rows[j][1].ToString();
                        //危害分类
                        //string harmType = dt.Rows[j][6].ToString();
                        //危害描述
                        string dangerSource = dt.Rows[j][7].ToString();
                        //危害属性
                        string harmType = dt.Rows[j][8].ToString();
                        //风险类别
                        string riskType = dt.Rows[j][9].ToString();
                        //风险后果
                        string result = dt.Rows[j][10].ToString();
                        //风险控制措施
                        string measures = dt.Rows[j][11].ToString();
                        //评价方式
                        string way = dt.Rows[j][12].ToString();
                        //辨识评价可能性
                        string itemA = dt.Rows[j][13].ToString();
                        //辨识评价频繁程度
                        string itemB = dt.Rows[j][14].ToString();
                        //辨识评价后果
                        string itemC = dt.Rows[j][15].ToString();
                        //辨识评价风险值
                        string itemR = dt.Rows[j][16].ToString();
                        //辨识评价风险等级
                        string grade = dt.Rows[j][17].ToString();
                        //可能导致的事故类型
                        string accType = dt.Rows[j][18].ToString();


                        RiskEntity risk = new RiskEntity();
                        risk.Id = Guid.NewGuid().ToString();
                        risk.HarmType = harmType;
                        risk.RiskType = riskType;
                        risk.AccidentName = risk.AccidentType = accType;
                        risk.PostName = postName;
                        risk.Result = result;
                        risk.AreaId = areaId;
                        risk.AreaName = areaName;
                        risk.Way = way;
                        risk.DangerSource = dangerSource;
                        risk.ItemA = string.IsNullOrEmpty(itemA) ? 0 : decimal.Parse(itemA);
                        risk.ItemB = string.IsNullOrEmpty(itemB) ? 0 : decimal.Parse(itemB);
                        risk.ItemC = string.IsNullOrEmpty(itemC) ? 0 : decimal.Parse(itemC);
                        risk.ItemR = risk.ItemA * risk.ItemB * risk.ItemC;
                        int gradeVal = 4;
                        if (risk.ItemR >= 400)
                        {
                            grade = "重大风险";
                            gradeVal = 1;
                        }
                        else if (risk.ItemR >= 200 && risk.ItemR < 400)
                        {
                            grade = "较大风险";
                            gradeVal = 2;
                        }
                        else if (risk.ItemR >= 70 && risk.ItemR < 200)
                        {
                            grade = "一般风险";
                            gradeVal = 3;
                        }
                        else
                        {
                            grade = "低风险";
                            gradeVal = 4;
                        }
                        risk.GradeVal = gradeVal;
                        risk.Grade = grade;
                        risk.DeptName = deptName;
                        risk.Status = 0;
                        RiskBLL riskBLL = new RiskBLL();

                        if (riskBLL.SaveForm("", risk) > 0)
                        {
                            MeasuresBLL measureBLL = new MeasuresBLL();
                            MeasuresEntity measure = new MeasuresEntity();
                            measure.RiskId = risk.Id;
                            measure.Content = measures;
                            measure.AreaId = areaId;
                            measureBLL.Save("", measure);
                        }
                        else
                        {
                            sb.AppendFormat("行:{0}，", j + 2);
                            error++;
                        }
                    }
                    catch (Exception ex)
                    {
                        sb.AppendFormat("行:{0}({1})，", j + 2, ex.Message);
                        error++;
                    }

                }
                count = dt.Rows.Count - 2;
                message = "共有" + count + "条记录,成功导入" + (count - error) + "条，失败" + error + "条。";
                if (error > 0)
                {
                    message += sb.ToString();
                }
            }

            return message;
        }
        #region 风险清单导入
        /// <summary>
        /// 风险导入
        /// </summary>
        /// <returns></returns>
        [HandlerLogin(LoginMode.Ignore)]
        [HandlerAuthorize(PermissionMode.Ignore)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(5, "导入风险清单")]
       
        public string ImportData()
        {
            try
            {
                string message = string.Empty;
                var IsGdxy = new DataItemDetailBLL().GetDataItemListByItemCode("'VManager'").ToList();
                var gxhs = new DataItemDetailBLL().GetItemValue("广西华昇版本");
                if (IsGdxy.Count>0)
                {
                    return ImportGdxyRiskData();
                }
                else if (!string.IsNullOrWhiteSpace(gxhs))
                {
                    return ImportGxhsRiskData();
                }
                else
                {
                    return ImportCommonRiskData();
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }
        /// <summary>
        /// 国电荥阳版本导入
        /// </summary>
        /// <returns></returns>
        private string ImportGdxyRiskData() {
            string message = "请选择格式正确的文件再导入!";
            int error = 0;
            StringBuilder sb = new StringBuilder("具体错误位置：<br />");
            int count = HttpContext.Request.Files.Count;
            if (count > 0)
            {
                count = 0;
                HttpPostedFileBase file = HttpContext.Request.Files[0];
                if (string.IsNullOrEmpty(file.FileName))
                {
                    return message;
                }
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName));
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                wb.Open(Server.MapPath("~/Resource/temp/" + fileName), Aspose.Cells.FileFormatType.Excel2003);

                Aspose.Cells.Cells cells = wb.Worksheets[0].Cells;

                List<string> types = new List<string> { "工程技术", "管理", "个体防护", "培训教育", "应急处置" };

                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                #region 作业风险库
                DepartmentBLL deptBll = new DepartmentBLL();
                DataTable dt = cells.ExportDataTable(2, 0, cells.MaxDataRow, cells.MaxColumn - 1, false);
                MeasuresBLL measureBLL = new MeasuresBLL();

                DistrictEntity dis = null;
                string areaId = "";
                string areaCode = "";
                //导入作业类型风险清单时,导入的数据同步到预知训练库
                DataTable TrainDt = dt.Clone();
                for (int j = 1; j < dt.Rows.Count; j++)
                {
                    areaId = "";
                    areaCode = "";
                    //区域名称
                    string areaName = dt.Rows[j][2].ToString();
                    //岗位
                    string person = dt.Rows[j][24].ToString().Trim();
                    //岗位
                    string postName = dt.Rows[j][23].ToString().Trim();
                    //管控部门
                    string deptName = dt.Rows[j][22].ToString().Trim();
                    //工作任务
                    string workTask = dt.Rows[j][4].ToString().Trim();

                    if (string.IsNullOrEmpty(deptName))
                    {
                        sb.AppendFormat("管控部门不能为空！作业风险库-行:{0}<br />", j);
                        error++;
                        continue;
                    }
                    if (string.IsNullOrEmpty(workTask))
                    {
                        sb.AppendFormat("工作任务不能为空！作业风险库-行:{0}<br />", j);
                        error++;
                        continue;
                    }
                    if (string.IsNullOrEmpty(areaName))
                    {
                        sb.AppendFormat("所属区域不能为空！作业风险库-行:{0}<br />", j);
                        error++;
                        continue;
                    }
                    try
                    {
                        //获取区域信息并判断该区域是否存在，没有则新增该区域
                        if (!string.IsNullOrEmpty(areaName))
                        {
                            dis = districtBLL.GetDistrict(user.OrganizeId, areaName);
                            if (dis != null)
                            {
                                areaId = dis.DistrictID;
                                areaCode = dis.DistrictCode;
                            }
                            else
                            {
                                sb.AppendFormat("区域与电厂区域不匹配！作业风险库-行:{0}<br />", j);
                            }
                        }
                        //管控层级
                        string level = dt.Rows[j][21].ToString().Trim();

                        //工序
                        string process = dt.Rows[j][5].ToString().Trim();
                        //作业类型
                        string worktype = dt.Rows[j][3].ToString().Trim();
                        //项目
                        string project = dt.Rows[j][6].ToString().Trim();
                        //危害源
                        string dangerSource = dt.Rows[j][7].ToString().Trim();
                        //风险描述
                        string desc = dt.Rows[j][8].ToString().Trim();
                        //风险类别
                        string riskType = "作业";
                        //风险后果
                        string result = dt.Rows[j][9].ToString().Trim();
                        //工程技术措施
                        string gcjscs = dt.Rows[j][16].ToString().Trim();
                        //管理措施
                        string glcs = dt.Rows[j][17].ToString().Trim();
                        //个人防护措施
                        string grfhcs = dt.Rows[j][18].ToString().Trim();
                        //培训教育措施
                        string pxjycs = dt.Rows[j][19].ToString().Trim();
                        //应急处置措施
                        string yjczcs = dt.Rows[j][20].ToString().Trim();
                        List<string> values = new List<string> { gcjscs, glcs, grfhcs, pxjycs, yjczcs };
                        string measures = "";
                        foreach (string str in values)
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                measures += str + "\r\n";
                            }
                        }
                        //评价方式
                        string way = dt.Rows[j][10].ToString().Trim();
                        string itemA = string.Empty;
                        string itemB = string.Empty;
                        string itemC = string.Empty;
                        //辨识评价可能性
                        itemA = dt.Rows[j][11].ToString().Trim();
                        if (way == "LEC")
                        {
                            //辨识评价频繁程度
                            itemB = dt.Rows[j][13].ToString().Trim();
                            //辨识评价后果
                            itemC = dt.Rows[j][12].ToString().Trim();
                        }
                        else
                        {
                            //辨识评价频繁程度
                            itemB = dt.Rows[j][12].ToString().Trim();
                            //辨识评价后果
                            itemC = dt.Rows[j][13].ToString().Trim();
                        }

                        ////辨识评价可能性
                        //string itemA = dt.Rows[j][11].ToString().Trim();
                        ////辨识评价频繁程度
                        //string itemB = dt.Rows[j][12].ToString().Trim();
                        ////辨识评价后果
                        //string itemC = dt.Rows[j][13].ToString().Trim();
                        //辨识评价风险值
                        string itemR = dt.Rows[j][14].ToString().Trim();
                        //辨识评价风险等级
                        string grade = dt.Rows[j][15].ToString().Trim();

                        RiskAssessEntity risk = new RiskAssessEntity();
                        risk.DeptCode = deptBll.GetDeptCode(deptName, user.OrganizeId);
                        risk.PostId = deptBll.GetPostId(postName, user.OrganizeId, risk.DeptCode);
                        if (string.IsNullOrEmpty(risk.DeptCode))
                        {
                            sb.AppendFormat("部门与系统的部门信息不匹配！作业风险库-行:{0}<br />", j);
                            error++;
                            continue;
                        }
                        risk.DeptId = deptBll.GetEntityByCode(risk.DeptCode).DepartmentId;
                        if (!string.IsNullOrWhiteSpace(worktype))
                        {
                            var data = new DataItemDetailBLL().GetDataItemListByItemCode("'StatisticsType'");
                            var list = worktype.Split(',');
                            for (int k = 0; k < list.Length; k++)
                            {
                                var entity = data.Where(x => x.ItemName == list[k].Trim()).FirstOrDefault();
                                if (entity != null)
                                {
                                    if (string.IsNullOrWhiteSpace(risk.WorkType))
                                    {

                                        risk.WorkTypeCode += entity.ItemValue + ",";
                                        risk.WorkType += entity.ItemName + ",";

                                    }
                                    else
                                    {
                                        if (!risk.WorkType.Contains(entity.ItemName))
                                        {
                                            risk.WorkTypeCode += entity.ItemValue + ",";
                                            risk.WorkType += entity.ItemName + ",";
                                        }
                                    }
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(risk.WorkTypeCode))
                            {
                                risk.WorkTypeCode = risk.WorkTypeCode.Substring(0, risk.WorkTypeCode.Length - 1);
                                risk.WorkType = risk.WorkType.Substring(0, risk.WorkType.Length - 1);
                            }
                        }
                        risk.Id = Guid.NewGuid().ToString();
                        risk.Result = result;
                        risk.RiskType = riskType;
                        risk.RiskDesc = desc;
                        risk.PostName = postName;
                        risk.DistrictId = areaId;
                        risk.AreaName = risk.DistrictName = areaName;
                        risk.AreaCode = areaCode;
                        risk.WorkTask = workTask;
                        risk.Process = process;
                        risk.Way = way;
                        risk.LevelName = level;
                        risk.DeptName = deptName;
                        risk.DutyPerson = person;
                        risk.Project = project;
                        risk.Measure = measures;
                        risk.DangerSource = dangerSource;
                        risk.ItemA = string.IsNullOrEmpty(itemA) ? 0 : decimal.Parse(itemA);
                        risk.ItemB = string.IsNullOrEmpty(itemB) ? 0 : decimal.Parse(itemB);
                        risk.ItemC = string.IsNullOrEmpty(itemC) ? 0 : decimal.Parse(itemC);

                        SetRiskGrade(risk);
                        risk.Status = 1;
                     
                        risk.DeptName = deptName;
                        RiskAssessBLL riskBLL = new RiskAssessBLL();

                        if (riskBLL.SaveForm("", risk) > 0)
                        {
                            int k = 0;
                            foreach (string str in values)
                            {
                                if (!string.IsNullOrEmpty(str))
                                {
                                    MeasuresEntity measure = new MeasuresEntity();
                                    measure.TypeName = types[k].Replace("措施", "");
                                    measure.RiskId = risk.Id;
                                    measure.Content = str;
                                    measure.AreaId = areaId;
                                    measureBLL.Save("", measure);
                                }
                                k++;
                            }
                            DataRow row = TrainDt.NewRow();
                            row.ItemArray = dt.Rows[j].ItemArray;
                            //row = dt.Rows[j];
                            TrainDt.Rows.Add(row);
                        }
                        else
                        {
                            sb.AppendFormat("作业风险库-行:{0}<br />", j);
                            error++;
                        }
                    }
                    catch (Exception ex)
                    {
                        sb.AppendFormat("作业风险库-行:{0}({1})<br />", j, ex.Message);
                        error++;
                    }
                }
                Dictionary<string, DataRow[]> dic = new Dictionary<string, DataRow[]>();
                //开始同步数据 TrainDt为已经导入风险清单的数据
                for (int i = 0; i < TrainDt.Rows.Count; i++)
                {
                    if (dic.Keys.Contains(TrainDt.Rows[i][4].ToString() + TrainDt.Rows[i][2].ToString()))
                    {
                        continue;
                    }
                    else
                    {
                        dic.Add(TrainDt.Rows[i][4].ToString() + TrainDt.Rows[i][2].ToString(), TrainDt.Select(string.Format("Column5='{0}' and Column3='{1}'", TrainDt.Rows[i][4].ToString(), TrainDt.Rows[i][2].ToString())));
                    }
                    //DataRow[] rows = TrainDt.Select(string.Format("worktask='{0}'", TrainDt.Rows[i]["worktask"].ToString()));
                }

                List<RisktrainlibEntity> RiskLib = new List<RisktrainlibEntity>();
                List<RisktrainlibdetailEntity> detailLib = new List<RisktrainlibdetailEntity>();
                foreach (string key in dic.Keys)
                {
                    DataRow[] item = dic[key];
                    var entity = new RisktrainlibEntity();
                    entity.Create();
                    entity.WorkTask = item[0][4].ToString();
                    int level = 4;
                    var data = new DataItemDetailBLL().GetDataItemListByItemCode("'StatisticsType'");
                    foreach (DataRow r in item)
                    {
                        entity.RiskLevel = r[14].ToString();
                        switch (entity.RiskLevel)
                        {
                            case "重大风险":
                                entity.RiskLevelVal = "1";
                                break;
                            case "较大风险":
                                entity.RiskLevelVal = "2";
                                break;
                            case "一般风险":
                                entity.RiskLevelVal = "3";
                                break;
                            case "低风险":
                                entity.RiskLevelVal = "4";
                                break;
                            default:
                                break;
                        }
                        if (Convert.ToInt32(entity.RiskLevelVal) < level)
                        {
                            level = Convert.ToInt32(entity.RiskLevelVal);
                        }

                        if (!string.IsNullOrWhiteSpace(r[3].ToString()))
                        {
                            var list = r[3].ToString().Split(',');
                            for (int k = 0; k < list.Length; k++)
                            {
                                var e = data.Where(x => x.ItemName == list[k].Trim()).FirstOrDefault();
                                if (e != null)
                                {
                                    if (string.IsNullOrWhiteSpace(entity.WorkType))
                                    {
                                        entity.WorkTypeCode += e.ItemValue + ",";
                                        entity.WorkType += e.ItemName + ",";
                                    }
                                    else
                                    {
                                        if (!entity.WorkType.Contains(e.ItemName))
                                        {
                                            entity.WorkTypeCode += e.ItemValue + ",";
                                            entity.WorkType += e.ItemName + ",";
                                        }
                                    }

                                }
                            }
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(entity.WorkTypeCode))
                    {
                        entity.WorkTypeCode = entity.WorkTypeCode.Substring(0, entity.WorkTypeCode.Length - 1);
                        entity.WorkType = entity.WorkType.Substring(0, entity.WorkType.Length - 1);
                    }
                    entity.RiskLevelVal = level.ToString();
                    switch (entity.RiskLevelVal)
                    {
                        case "1":
                            entity.RiskLevel = "重大风险";
                            break;
                        case "2":
                            entity.RiskLevel = "较大风险";
                            break;
                        case "3":
                            entity.RiskLevel = "一般风险";
                            break;
                        case "4":
                            entity.RiskLevel = "低风险";
                            break;
                        default:
                            break;
                    }
                    if (string.IsNullOrWhiteSpace(item[0][2].ToString()))
                    {
                        //falseMessage += "</br>" + "第" + (i+1) + "行区域为空,未能导入.";
                        //error++;
                        //continue;
                    }
                    else
                    {
                        entity.WorkArea = item[0][2].ToString();
                        DistrictEntity disEntity = districtBLL.GetDistrict(user.OrganizeId, item[0][2].ToString());
                        if (disEntity == null)
                        {
                            //电厂没有该区域则不赋值
                            entity.WorkArea = "";
                        }
                        else
                        {
                            entity.WorkAreaId = disEntity.DistrictID;
                        }
                    }
                    entity.WorkPost = item[0][23].ToString();
                    entity.DataSources = "1";
                    RiskLib.Add(entity);
                    foreach (DataRow it in item)
                    {
                        var dentity = new RisktrainlibdetailEntity();
                        dentity.Process = it[5].ToString();
                        dentity.AtRisk = it[7].ToString() + it[8].ToString();
                        //工程技术措施
                        string gcjscs = it[16].ToString().Trim();
                        //管理措施
                        string glcs = it[17].ToString().Trim();
                        //个人防护措施
                        string grfhcs = it[18].ToString().Trim();
                        //培训教育措施
                        string pxjycs = it[19].ToString().Trim();
                        //应急处置措施
                        string yjczcs = it[20].ToString().Trim();
                        List<string> values = new List<string> { gcjscs, glcs, grfhcs, pxjycs, yjczcs };
                        string measures = "";
                        foreach (string str in values)
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                measures += str + "\r\n";
                            }
                        }
                        dentity.Controls = measures;
                        dentity.Create();
                        dentity.WorkId = entity.ID;
                        detailLib.Add(dentity);
                    }
                }
                new RisktrainlibBLL().InsertImportData(RiskLib, detailLib);
                if (dt.Rows.Count > 1)
                {
                    count += dt.Rows.Count - 1;
                }
                #endregion

                #region 设备风险库

                cells = wb.Worksheets[1].Cells;
                dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn, false);
                for (int j = 1; j < dt.Rows.Count; j++)
                {
                    areaId = "";
                    areaCode = "";
                    //区域名称
                    string areaName = dt.Rows[j][3].ToString();
                    //管控部门
                    string deptName = dt.Rows[j][22].ToString().Trim();
                    if (string.IsNullOrEmpty(deptName))
                    {
                        sb.AppendFormat("管控部门不能为空！设备风险库-行:{0}<br />", j);
                        error++;
                        continue;
                    }
                    try
                    {
                        //获取区域信息并判断该区域是否存在，没有则新增该区域
                        if (!string.IsNullOrEmpty(areaName))
                        {
                            dis = districtBLL.GetDistrict(user.OrganizeId, areaName);
                            if (dis != null)
                            {
                                areaId = dis.DistrictID;
                                areaCode = dis.DistrictCode;
                            }
                            else
                            {
                                sb.AppendFormat("区域与电厂区域不匹配！设备风险库-行:{0}<br />", j);
                                error++;
                                //continue;
                            }
                        }
                        //所属单元
                        string element = dt.Rows[j][2].ToString().Trim();
                        //设备名称
                        string machineName = dt.Rows[j][4].ToString().Trim();
                        //部件
                        string parts = dt.Rows[j][5].ToString().Trim();
                        //故障类别
                        string faultcategory = dt.Rows[j][6].ToString().Trim();
                        //危害源--故障类型
                        string dangerSource = dt.Rows[j][7].ToString().Trim();
                        //风险描述
                        string desc = dt.Rows[j][8].ToString().Trim();
                        //风险类别
                        string riskType = "设备";
                        //风险后果
                        string result = dt.Rows[j][9].ToString().Trim();
                        //评价方式
                        string way = dt.Rows[j][10].ToString().Trim();
                        string itemA = string.Empty;
                        string itemB = string.Empty;
                        string itemC = string.Empty;
                        //辨识评价可能性
                        itemA = dt.Rows[j][11].ToString().Trim();
                        if (way == "LEC")
                        {
                            //辨识评价频繁程度
                            itemB = dt.Rows[j][13].ToString().Trim();
                            //辨识评价后果
                            itemC = dt.Rows[j][12].ToString().Trim();
                        }
                        else
                        {
                            //辨识评价频繁程度
                            itemB = dt.Rows[j][12].ToString().Trim();
                            //辨识评价后果
                            itemC = dt.Rows[j][13].ToString().Trim();
                        }

                        ////辨识评价可能性
                        //string itemA = dt.Rows[j][11].ToString().Trim();
                        ////辨识评价频繁程度
                        //string itemB = dt.Rows[j][12].ToString().Trim();
                        ////辨识评价后果
                        //string itemC = dt.Rows[j][13].ToString().Trim();
                        //辨识评价风险值
                        string itemR = dt.Rows[j][14].ToString().Trim();
                        //辨识评价风险等级
                        string grade = dt.Rows[j][15].ToString().Trim();
                        //工程技术措施
                        string gcjscs = dt.Rows[j][16].ToString().Trim();
                        //管理措施
                        string glcs = dt.Rows[j][17].ToString().Trim();
                        //个人防护措施
                        string grfhcs = dt.Rows[j][18].ToString().Trim();
                        //培训教育措施
                        string pxjycs = dt.Rows[j][19].ToString().Trim();
                        //应急处置措施
                        string yjczcs = dt.Rows[j][20].ToString().Trim();
                        List<string> values = new List<string> { gcjscs, glcs, grfhcs, pxjycs, yjczcs };
                        string measures = "";
                        foreach (string str in values)
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                measures += str + "\r\n";
                            }
                        }
                        //管控层级
                        string level = dt.Rows[j][21].ToString().Trim();

                        //岗位
                        string postName = dt.Rows[j][23].ToString().Trim();

                        //管控责任人
                        string dutyPerson = dt.Rows[j][24].ToString().Trim();

                        RiskAssessEntity risk = new RiskAssessEntity();
                        risk.DeptCode = deptBll.GetDeptCode(deptName, user.OrganizeId);
                        risk.PostId = deptBll.GetPostId(postName, user.OrganizeId, risk.DeptCode);
                        if (string.IsNullOrEmpty(risk.DeptCode))
                        {
                            sb.AppendFormat("部门与系统的部门信息不匹配！设备风险库-行:{0}<br />", j);
                            error++;
                            continue;
                        }
                        risk.DeptId = deptBll.GetEntityByCode(risk.DeptCode).DepartmentId;
                        risk.Id = Guid.NewGuid().ToString();
                        risk.FaultCategory = faultcategory;
                        risk.Result = result;
                        risk.DutyPerson = dutyPerson;
                        risk.RiskType = riskType;
                        risk.RiskDesc = desc;
                        risk.PostName = postName;
                        risk.DeptName = deptName;
                        risk.LevelName = level;
                        risk.AreaCode = areaCode;
                        risk.DistrictId = areaId;
                        risk.AreaName = risk.DistrictName = areaName;
                        risk.EquipmentName = machineName;
                        risk.Parts = parts;
                        risk.Element = element;
                        risk.Way = way;
                        risk.Status = 1;
                        risk.Measure = measures;
                        risk.DangerSource = dangerSource;
                        risk.FaultType = dangerSource;
                        risk.ItemA = string.IsNullOrEmpty(itemA) ? 0 : decimal.Parse(itemA);
                        risk.ItemB = string.IsNullOrEmpty(itemB) ? 0 : decimal.Parse(itemB);
                        risk.ItemC = string.IsNullOrEmpty(itemC) ? 0 : decimal.Parse(itemC);

                        SetRiskGrade(risk);
                        risk.DeptName = deptName;
                        RiskAssessBLL riskBLL = new RiskAssessBLL();

                        if (riskBLL.SaveForm("", risk) > 0)
                        {
                            int k = 0;
                            foreach (string str in values)
                            {
                                if (!string.IsNullOrEmpty(str))
                                {

                                    MeasuresEntity measure = new MeasuresEntity();
                                    measure.TypeName = types[k].Replace("措施", "");
                                    measure.RiskId = risk.Id;
                                    measure.Content = str;
                                    measure.AreaId = areaId;
                                    measureBLL.Save("", measure);
                                }
                                k++;
                            }
                        }
                        else
                        {
                            sb.AppendFormat("设备风险库-行:{0}<br />", j);
                            error++;
                        }
                    }
                    catch (Exception ex)
                    {
                        sb.AppendFormat("设备风险库-行:{0}({1})<br />", j, ex.Message);
                        error++;
                    }

                }
                if (dt.Rows.Count > 1)
                {
                    count += dt.Rows.Count - 1;
                }
                #endregion

                #region 环境风险库

                cells = wb.Worksheets[2].Cells;
                dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn, false);
                for (int j = 1; j < dt.Rows.Count; j++)
                {
                    areaId = "";
                    areaCode = "";
                    //区域名称
                    string areaName = dt.Rows[j][2].ToString();
                    //管控部门
                    string deptName = dt.Rows[j][20].ToString().Trim();
                    if (string.IsNullOrEmpty(deptName))
                    {
                        sb.AppendFormat("管控部门不能为空！区域风险库-行:{0}<br />", j);
                        error++;
                        continue;
                    }
                    try
                    {
                        //获取区域信息并判断该区域是否存在，没有则新增该区域
                        if (!string.IsNullOrEmpty(areaName))
                        {
                            dis = districtBLL.GetDistrict(user.OrganizeId, areaName);
                            if (dis != null)
                            {
                                areaId = dis.DistrictID;
                                areaCode = dis.DistrictCode;
                            }
                            else
                            {
                                sb.AppendFormat("区域与电厂区域不匹配！区域风险库-行:{0}<br />", j);
                                error++;
                                //continue;
                            }
                        }
                        //系统-对应环境风险
                        string hjsystem = dt.Rows[j][3].ToString().Trim();
                        //设备-对应环境风险
                        string hjequpment = dt.Rows[j][4].ToString().Trim();
                        //危险源
                        string dangerSource = dt.Rows[j][5].ToString().Trim();
                        //风险描述
                        string desc = dt.Rows[j][6].ToString().Trim();
                        //风险类别
                        string riskType = "区域";
                        //风险后果
                        string result = dt.Rows[j][7].ToString().Trim();
                        //评价方式
                        string way = dt.Rows[j][8].ToString().Trim();
                        string itemA = string.Empty;
                        string itemB = string.Empty;
                        string itemC = string.Empty;
                        //辨识评价可能性
                        itemA = dt.Rows[j][9].ToString().Trim();
                        if (way == "LEC")
                        {
                            //辨识评价频繁程度
                            itemB = dt.Rows[j][11].ToString().Trim();
                            //辨识评价后果
                            itemC = dt.Rows[j][10].ToString().Trim();
                        }
                        else
                        {
                            //辨识评价频繁程度
                            itemB = dt.Rows[j][10].ToString().Trim();
                            //辨识评价后果
                            itemC = dt.Rows[j][11].ToString().Trim();
                        }

                        ////辨识评价可能性
                        //string itemA = dt.Rows[j][9].ToString().Trim();
                        ////辨识评价频繁程度
                        //string itemB = dt.Rows[j][10].ToString().Trim();
                        ////辨识评价后果
                        //string itemC = dt.Rows[j][11].ToString().Trim();
                        //辨识评价风险值
                        string itemR = dt.Rows[j][12].ToString().Trim();
                        //辨识评价风险等级
                        string grade = dt.Rows[j][13].ToString().Trim();
                        //工程技术措施
                        string gcjscs = dt.Rows[j][14].ToString().Trim();
                        //管理措施
                        string glcs = dt.Rows[j][15].ToString().Trim();
                        //个人防护措施
                        string grfhcs = dt.Rows[j][16].ToString().Trim();
                        //培训教育措施
                        string pxjycs = dt.Rows[j][17].ToString().Trim();
                        //应急处置措施
                        string yjczcs = dt.Rows[j][18].ToString().Trim();
                        List<string> values = new List<string> { gcjscs, glcs, grfhcs, pxjycs, yjczcs };
                        string measures = "";
                        foreach (string str in values)
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                measures += str + "\r\n";
                            }
                        }
                        //管控层级
                        string level = dt.Rows[j][19].ToString().Trim();

                        //岗位
                        string postName = dt.Rows[j][21].ToString().Trim();

                        //管控责任人
                        string dutyPerson = dt.Rows[j][22].ToString().Trim();

                        RiskAssessEntity risk = new RiskAssessEntity();
                        risk.DeptCode = deptBll.GetDeptCode(deptName, user.OrganizeId);
                        risk.PostId = deptBll.GetPostId(postName, user.OrganizeId, risk.DeptCode);
                        if (string.IsNullOrEmpty(risk.DeptCode))
                        {
                            sb.AppendFormat("部门与系统的部门信息不匹配！区域风险库-行:{0}<br />", j);
                            error++;
                            continue;
                        }
                        risk.DeptId = deptBll.GetEntityByCode(risk.DeptCode).DepartmentId;
                        risk.Id = Guid.NewGuid().ToString();
                        risk.Result = result;
                        risk.RiskType = riskType;
                        risk.DutyPerson = dutyPerson;
                        risk.RiskDesc = desc;
                        risk.PostName = postName;
                        risk.DeptName = deptName;
                        risk.AreaId = areaId;
                        risk.LevelName = level;
                        risk.DistrictId = areaId;
                        risk.AreaCode = areaCode;
                        risk.AreaName = risk.DistrictName = areaName;
                        risk.Way = way;
                        risk.Status = 1;
                        risk.HjSystem = hjsystem;
                        risk.HjEqupment = hjequpment;
                        risk.Measure = measures;
                        risk.DangerSource = dangerSource;
                        risk.ItemA = string.IsNullOrEmpty(itemA) ? 0 : decimal.Parse(itemA);
                        risk.ItemB = string.IsNullOrEmpty(itemB) ? 0 : decimal.Parse(itemB);
                        risk.ItemC = string.IsNullOrEmpty(itemC) ? 0 : decimal.Parse(itemC);

                        SetRiskGrade(risk);
                        risk.DeptName = deptName;
                        RiskAssessBLL riskBLL = new RiskAssessBLL();

                        if (riskBLL.SaveForm("", risk) > 0)
                        {
                            int k = 0;
                            foreach (string str in values)
                            {
                                if (!string.IsNullOrEmpty(str))
                                {
                                    MeasuresEntity measure = new MeasuresEntity();
                                    measure.TypeName = types[k].Replace("措施", "");
                                    measure.RiskId = risk.Id;
                                    measure.Content = str;
                                    measure.AreaId = areaId;
                                    measureBLL.Save("", measure);
                                }
                                k++;
                            }
                        }
                        else
                        {
                            sb.AppendFormat("区域风险库-行:{0}<br />", j);
                            error++;
                        }
                    }
                    catch (Exception ex)
                    {
                        sb.AppendFormat("区域风险库-行:{0}({1})<br />", j, ex.Message);
                        error++;
                    }

                }
                if (dt.Rows.Count > 1)
                {
                    count += dt.Rows.Count - 1;
                }
                #endregion

                #region 管理风险库


                cells = wb.Worksheets[3].Cells;
                dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn, false);
                for (int j = 1; j < dt.Rows.Count; j++)
                {
                    areaId = "";
                    areaCode = "";
                    //获取区域信息并判断该区域是否存在，没有则新增该区域
                    string aName = dt.Rows[j][2].ToString().Trim();
                    //管控部门
                    string deptName = dt.Rows[j][21].ToString().Trim();
                    if (string.IsNullOrEmpty(deptName))
                    {
                        sb.AppendFormat("管控部门不能为空！管理风险库-行:{0}<br />", j);
                        error++;
                        continue;
                    }
                    if (!string.IsNullOrEmpty(aName))
                    {
                        dis = districtBLL.GetDistrict(user.OrganizeId, aName);
                        if (dis != null)
                        {
                            areaId = dis.DistrictID;
                            areaCode = dis.DistrictCode;
                        }
                        else
                        {
                            sb.AppendFormat("区域与电厂区域不匹配！管理风险库-行:{0}<br />", j);
                            error++;
                            //continue;
                        }
                    }
                    //危险源类别
                    string majornametype = dt.Rows[j][3].ToString().Trim();
                    //危险源类别
                    string majorname = dt.Rows[j][4].ToString().Trim();
                    //危险源
                    string dangerSourcetype = dt.Rows[j][5].ToString().Trim();
                    //危险源
                    string dangerSource = dt.Rows[j][6].ToString().Trim();

                    //风险描述
                    string desc = dt.Rows[j][7].ToString().Trim();

                    //风险类别
                    string riskType = "管理";
                    //风险后果
                    string result = dt.Rows[j][8].ToString().Trim();
                    //评价方式
                    string way = dt.Rows[j][9].ToString().Trim();
                    string itemA = string.Empty;
                    string itemB = string.Empty;
                    string itemC = string.Empty;
                    //辨识评价可能性
                    itemA = dt.Rows[j][10].ToString().Trim();
                    if (way == "LEC")
                    {
                        //辨识评价频繁程度
                        itemB = dt.Rows[j][12].ToString().Trim();
                        //辨识评价后果
                        itemC = dt.Rows[j][11].ToString().Trim();
                    }
                    else
                    {
                        //辨识评价频繁程度
                        itemB = dt.Rows[j][12].ToString().Trim();
                        //辨识评价后果
                        itemC = dt.Rows[j][12].ToString().Trim();
                    }


                    ////辨识评价可能性
                    //string itemA = dt.Rows[j][10].ToString().Trim();
                    ////辨识评价频繁程度
                    //string itemB = dt.Rows[j][11].ToString().Trim();
                    ////辨识评价后果
                    //string itemC = dt.Rows[j][12].ToString().Trim();
                    //辨识评价风险值
                    string itemR = dt.Rows[j][13].ToString().Trim();
                    //辨识评价风险等级
                    string grade = dt.Rows[j][14].ToString().Trim();
                    //工程技术措施
                    string gcjscs = dt.Rows[j][15].ToString().Trim();
                    //管理措施
                    string glcs = dt.Rows[j][16].ToString().Trim();
                    //个人防护措施
                    string grfhcs = dt.Rows[j][17].ToString().Trim();
                    //培训教育措施
                    string pxjycs = dt.Rows[j][18].ToString().Trim();
                    //应急处置措施
                    string yjczcs = dt.Rows[j][19].ToString().Trim();
                    List<string> values = new List<string> { gcjscs, glcs, grfhcs, pxjycs, yjczcs };
                    string measures = "";
                    foreach (string str in values)
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            measures += str + "\r\n";
                        }
                    }
                    //管控层级
                    string level = dt.Rows[j][20].ToString().Trim();
                    //岗位
                    string postName = dt.Rows[j][22].ToString().Trim();
                    //管控责任人
                    string dutyPerson = dt.Rows[j][23].ToString().Trim();

                    RiskAssessEntity risk = new RiskAssessEntity();
                    risk.DeptCode = deptBll.GetDeptCode(deptName, user.OrganizeId);
                    risk.PostId = deptBll.GetPostId(postName, user.OrganizeId, risk.DeptCode);
                    if (string.IsNullOrEmpty(risk.DeptCode))
                    {
                        sb.AppendFormat("部门与系统的部门信息不匹配！管理风险库-行:{0}<br />", j);
                        error++;
                        continue;
                    }
                    risk.DeptId = deptBll.GetEntityByCode(risk.DeptCode).DepartmentId;
                    risk.Id = Guid.NewGuid().ToString();
                    risk.RiskType = riskType;
                    risk.RiskDesc = desc;
                    risk.DutyPerson = dutyPerson;
                    risk.DangerSourceType = dangerSourcetype;
                    risk.MajorNameType = majornametype;
                    risk.MajorName = majorname;
                    risk.PostName = postName;
                    risk.DeptName = deptName;
                    risk.Result = result;
                    risk.DistrictId = areaId;
                    risk.AreaCode = areaCode;
                    risk.DistrictName = aName;
                    risk.Way = way;
                    risk.Status = 1;
                    risk.LevelName = level;
                    risk.Measure = measures;
                    risk.DangerSource = dangerSource;
                    risk.ItemA = string.IsNullOrEmpty(itemA) ? 0 : decimal.Parse(itemA);
                    risk.ItemB = string.IsNullOrEmpty(itemB) ? 0 : decimal.Parse(itemB);
                    risk.ItemC = string.IsNullOrEmpty(itemC) ? 0 : decimal.Parse(itemC);

                    SetRiskGrade(risk);
                    risk.DeptName = deptName;
                    RiskAssessBLL riskBLL = new RiskAssessBLL();

                    if (riskBLL.SaveForm("", risk) > 0)
                    {
                        int k = 0;
                        foreach (string str in values)
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                MeasuresEntity measure = new MeasuresEntity();
                                measure.TypeName = types[k].Replace("措施", "");
                                measure.RiskId = risk.Id;
                                measure.Content = str;
                                measure.AreaId = areaId;
                                measureBLL.Save("", measure);
                            }
                            k++;
                        }
                    }
                    else
                    {
                        sb.AppendFormat("管理风险库-行:{0}<br />", j);
                        error++;
                    }

                }
                if (dt.Rows.Count > 1)
                {
                    count += dt.Rows.Count - 1;
                }
                #endregion

                #region 职业病危害库

                //cells = wb.Worksheets[4].Cells;
                //dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn, false);
                //for (int j = 1; j < dt.Rows.Count; j++)
                //{
                //    areaId = "";
                //    areaCode = "";
                //    //区域名称
                //    string areaName = dt.Rows[j][3].ToString();
                //    //管控部门
                //    string deptName = dt.Rows[j][19].ToString().Trim();
                //    if (string.IsNullOrEmpty(deptName))
                //    {
                //        sb.AppendFormat("管控部门不能为空！职业病危害库-行:{0}<br />", j);
                //        error++;
                //        continue;
                //    }
                //    try
                //    {
                //        //获取区域信息并判断该区域是否存在
                //        if (!string.IsNullOrEmpty(areaName))
                //        {
                //            dis = districtBLL.GetDistrict(user.OrganizeId, areaName);
                //            if (dis != null)
                //            {
                //                areaId = dis.DistrictID;
                //                areaCode = dis.DistrictCode;
                //            }
                //            else
                //            {
                //                sb.AppendFormat("区域与电厂区域不匹配！职业病危害库-行:{0}<br />", j);
                //                error++;
                //                // continue;
                //            }
                //        }
                //        //岗位
                //        string postName = dt.Rows[j][20].ToString().Trim();

                //        //管控层级
                //        string level = dt.Rows[j][18].ToString().Trim();
                //        //风险点
                //        string majorname = dt.Rows[j][1].ToString().Trim();
                //        //职业病危害因素
                //        string dangerSource = dt.Rows[j][4].ToString().Trim();
                //        //作业分级
                //        string workType = dt.Rows[j][5].ToString().Trim();
                //        //导致的职业病或健康损伤
                //        string illType = dt.Rows[j][6].ToString().Trim();
                //        //风险分类
                //        string riskType = "职业病危害";
                //        ////
                //        //string result = dt.Rows[j]["职业病危害因素"].ToString().Trim();
                //        //工程技术措施
                //        string gcjscs = dt.Rows[j][13].ToString().Trim();
                //        //管理措施
                //        string glcs = dt.Rows[j][14].ToString().Trim();
                //        //个人防护措施
                //        string grfhcs = dt.Rows[j][15].ToString().Trim();
                //        //培训教育措施
                //        string pxjycs = dt.Rows[j][16].ToString().Trim();
                //        //应急处置措施
                //        string yjczcs = dt.Rows[j][17].ToString().Trim();
                //        List<string> values = new List<string> { gcjscs, glcs, grfhcs, pxjycs, yjczcs };
                //        string measures = "";
                //        foreach (string str in values)
                //        {
                //            if (!string.IsNullOrEmpty(str))
                //            {
                //                measures += str + "\r\n";
                //            }
                //        }
                //        //评价方式
                //        string way = dt.Rows[j][7].ToString().Trim();
                //        //辨识评价可能性
                //        string itemA = dt.Rows[j][8].ToString().Trim();
                //        //辨识评价频繁程度
                //        string itemB = dt.Rows[j][9].ToString().Trim();
                //        //辨识评价后果
                //        string itemC = dt.Rows[j][10].ToString().Trim();
                //        //辨识评价风险值
                //        string itemR = dt.Rows[j][11].ToString().Trim();
                //        //辨识评价风险等级
                //        string grade = dt.Rows[j][12].ToString().Trim();

                //        RiskAssessEntity risk = new RiskAssessEntity();
                //        risk.DeptCode = deptBll.GetDeptCode(deptName, user.OrganizeId);
                //        risk.PostId = deptBll.GetPostId(postName, user.OrganizeId, risk.DeptCode);
                //        if (string.IsNullOrEmpty(risk.DeptCode))
                //        {
                //            sb.AppendFormat("部门与系统的部门信息不匹配！职业病危害库-行:{0}<br />", j);
                //            error++;
                //            continue;
                //        }

                //        risk.Id = Guid.NewGuid().ToString();
                //        //risk.Result = result;
                //        risk.MajorName = majorname;
                //        risk.Description = dangerSource;
                //        risk.HarmType = workType;
                //        risk.HarmProperty = illType;
                //        risk.RiskType = riskType;
                //        risk.PostName = postName;

                //        risk.DeptName = deptName;
                //        //risk.Result = result;
                //        risk.AreaId = areaId;
                //        risk.DistrictId = areaId;
                //        risk.AreaCode = areaCode;
                //        risk.AreaName = risk.DistrictName = areaName;
                //        risk.Way = way;
                //        risk.LevelName = level;
                //        risk.Status = 1;
                //        risk.Measure = measures;
                //        risk.ItemA = string.IsNullOrEmpty(itemA) ? 0 : decimal.Parse(itemA);
                //        risk.ItemB = string.IsNullOrEmpty(itemB) ? 0 : decimal.Parse(itemB);
                //        risk.ItemC = string.IsNullOrEmpty(itemC) ?1 : decimal.Parse(itemC);
                //        //risk.ItemR = risk.ItemA  * risk.ItemB * risk.ItemC;
                //        int gradeVal = 4;
                //        if (way == "TRA")
                //        {
                //            risk.ItemR = risk.ItemA * risk.ItemB ;

                //            if (risk.ItemR >= 20)
                //            {
                //                grade = "重大风险";
                //                gradeVal = 1;
                //            }
                //            else if (risk.ItemR >= 10 && risk.ItemR < 20)
                //            {
                //                grade = "较大风险";
                //                gradeVal = 2;
                //            }
                //            else if (risk.ItemR >= 5 && risk.ItemR < 10)
                //            {
                //                grade = "一般风险";
                //                gradeVal = 3;
                //            }
                //            else
                //            {
                //                grade = "低风险";
                //                gradeVal = 4;
                //            }
                //        }
                //        else
                //        {
                //            risk.ItemR = risk.ItemA * risk.ItemB * risk.ItemC;
                //            if (risk.ItemR >= 400)
                //            {
                //                grade = "重大风险";
                //                gradeVal = 1;
                //            }
                //            else if (risk.ItemR >= 200 && risk.ItemR < 400)
                //            {
                //                grade = "较大风险";
                //                gradeVal = 2;
                //            }
                //            else if (risk.ItemR >= 70 && risk.ItemR < 200)
                //            {
                //                grade = "一般风险";
                //                gradeVal = 3;
                //            }
                //            else
                //            {
                //                grade = "低风险";
                //                gradeVal = 4;
                //            }
                //        }
                //        risk.GradeVal = gradeVal;
                //        risk.Grade = grade;
                //        risk.DeptName = deptName;
                //        RiskAssessBLL riskBLL = new RiskAssessBLL();

                //        if (riskBLL.SaveForm("", risk) > 0)
                //        {
                //            int k = 0;
                //            foreach (string str in values)
                //            {
                //                if (!string.IsNullOrEmpty(str))
                //                {
                //                    MeasuresEntity measure = new MeasuresEntity();
                //                    measure.TypeName = types[k].Replace("措施", "");
                //                    measure.RiskId = risk.Id;
                //                    measure.Content = str;
                //                    measure.AreaId = areaId;
                //                    measureBLL.Save("", measure);
                //                }
                //                k++;
                //            }
                //        }
                //        else
                //        {
                //            sb.AppendFormat("职业病危害库-行:{0}<br />", j);
                //            error++;
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        sb.AppendFormat("职业病危害库-行:{0}({1})<br />", j, ex.Message);
                //        error++;
                //    }

                //}
                //if (dt.Rows.Count > 1)
                //{
                //    count += dt.Rows.Count - 1;
                //}
                #endregion
                #region 岗位风险

                cells = wb.Worksheets[4].Cells;
                dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn, false);
                for (int j = 1; j < dt.Rows.Count; j++)
                {
                    areaId = "";
                    areaCode = "";
                    //区域名称
                    string areaName = dt.Rows[j][2].ToString();
                    //管控部门
                    string deptName = dt.Rows[j][22].ToString().Trim();
                    if (string.IsNullOrEmpty(deptName))
                    {
                        sb.AppendFormat("管控部门不能为空！岗位风险库-行:{0}<br />", j);
                        error++;
                        continue;
                    }
                    try
                    {
                        //获取区域信息并判断该区域是否存在，没有则新增该区域
                        if (!string.IsNullOrEmpty(areaName))
                        {
                            dis = districtBLL.GetDistrict(user.OrganizeId, areaName);
                            if (dis != null)
                            {
                                areaId = dis.DistrictID;
                                areaCode = dis.DistrictCode;
                            }
                            else
                            {
                                sb.AppendFormat("区域与电厂区域不匹配！岗位风险库-行:{0}<br />", j);
                                error++;
                                // continue;
                            }
                        }
                        //部门
                        string dept = dt.Rows[j][3].ToString().Trim();
                        //岗位名称--对应岗位风险
                        string jobname = dt.Rows[j][4].ToString().Trim();
                        //人员
                        string person = dt.Rows[j][5].ToString().Trim();
                        //危险源类别
                        string dangersourcetype = dt.Rows[j][6].ToString().Trim();
                        //危险源
                        string dangerSource = dt.Rows[j][7].ToString().Trim();

                        //风险类别
                        string riskType = "岗位";
                        //风险描述
                        string desc = dt.Rows[j][8].ToString().Trim();
                        //风险后果
                        string result = dt.Rows[j][9].ToString().Trim();
                        //评价方式
                        string way = dt.Rows[j][10].ToString().Trim();
                        string itemA = string.Empty;
                        string itemB = string.Empty;
                        string itemC = string.Empty;
                        //辨识评价可能性
                        itemA = dt.Rows[j][11].ToString().Trim();
                        if (way == "LEC")
                        {
                            //辨识评价频繁程度
                            itemB = dt.Rows[j][13].ToString().Trim();
                            //辨识评价后果
                            itemC = dt.Rows[j][12].ToString().Trim();
                        }
                        else
                        {
                            //辨识评价频繁程度
                            itemB = dt.Rows[j][12].ToString().Trim();
                            //辨识评价后果
                            itemC = dt.Rows[j][13].ToString().Trim();
                        }

                        ////辨识评价可能性
                        //string itemA = dt.Rows[j][11].ToString().Trim();
                        ////辨识评价频繁程度
                        //string itemB = dt.Rows[j][12].ToString().Trim();
                        ////辨识评价后果
                        //string itemC = dt.Rows[j][13].ToString().Trim();
                        //辨识评价风险值
                        string itemR = dt.Rows[j][14].ToString().Trim();
                        //辨识评价风险等级
                        string grade = dt.Rows[j][15].ToString().Trim();

                        //工程技术措施
                        string gcjscs = dt.Rows[j][16].ToString().Trim();
                        //管理措施
                        string glcs = dt.Rows[j][17].ToString().Trim();
                        //个人防护措施
                        string grfhcs = dt.Rows[j][18].ToString().Trim();
                        //培训教育措施
                        string pxjycs = dt.Rows[j][19].ToString().Trim();
                        //应急处置措施
                        string yjczcs = dt.Rows[j][20].ToString().Trim();
                        List<string> values = new List<string> { gcjscs, glcs, grfhcs, pxjycs, yjczcs };
                        string measures = "";
                        foreach (string str in values)
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                measures += str + "\r\n";
                            }
                        }

                        //管控层级
                        string level = dt.Rows[j][21].ToString().Trim();
                        //岗位
                        string postName = dt.Rows[j][23].ToString().Trim();
                        //管控责任人
                        string dutyPerson = dt.Rows[j][24].ToString().Trim();

                        RiskAssessEntity risk = new RiskAssessEntity();
                        risk.DeptCode = deptBll.GetDeptCode(deptName, user.OrganizeId);
                        risk.PostId = deptBll.GetPostId(postName, user.OrganizeId, risk.DeptCode);
                        if (string.IsNullOrEmpty(risk.DeptCode))
                        {
                            sb.AppendFormat("管控部门与系统的部门信息不匹配！岗位风险库-行:{0}<br />", j);
                            error++;
                            continue;
                        }
                        risk.DeptId = deptBll.GetEntityByCode(risk.DeptCode).DepartmentId;
                        risk.Id = Guid.NewGuid().ToString();
                        risk.PostDeptCode = deptBll.GetDeptCode(dept, user.OrganizeId);
                        if (string.IsNullOrEmpty(risk.PostDeptCode))
                        {
                            sb.AppendFormat("部门与系统的部门信息不匹配！岗位风险库-行:{0}<br />", j);
                            error++;
                            continue;
                        }
                        risk.PostDept = dept;
                        risk.PostDeptId = deptBll.GetEntityByCode(risk.PostDeptCode).DepartmentId;
                        risk.PostPerson = person;
                        risk.DutyPerson = dutyPerson;
                        risk.RiskType = riskType;
                        risk.RiskDesc = desc;
                        risk.PostName = postName;
                        risk.DeptName = deptName;
                        risk.Result = result;
                        risk.AreaId = areaId;
                        risk.LevelName = level;
                        risk.DistrictId = areaId;
                        risk.AreaCode = areaCode;
                        risk.AreaName = risk.DistrictName = areaName;
                        risk.Way = way;
                        risk.Status = 1;
                        risk.JobName = jobname;
                        risk.DangerSourceType = dangersourcetype;
                        risk.Measure = measures;
                        risk.DangerSource = dangerSource;
                        risk.ItemA = string.IsNullOrEmpty(itemA) ? 0 : decimal.Parse(itemA);
                        risk.ItemB = string.IsNullOrEmpty(itemB) ? 0 : decimal.Parse(itemB);
                        risk.ItemC = string.IsNullOrEmpty(itemC) ? 0 : decimal.Parse(itemC);

                        SetRiskGrade(risk);
                        risk.DeptName = deptName;
                        RiskAssessBLL riskBLL = new RiskAssessBLL();

                        if (riskBLL.SaveForm("", risk) > 0)
                        {
                            int k = 0;
                            foreach (string str in values)
                            {
                                if (!string.IsNullOrEmpty(str))
                                {
                                    MeasuresEntity measure = new MeasuresEntity();
                                    measure.TypeName = types[k].Replace("措施", "");
                                    measure.RiskId = risk.Id;
                                    measure.Content = str;
                                    measure.AreaId = areaId;
                                    measureBLL.Save("", measure);
                                }
                                k++;
                            }
                        }
                        else
                        {
                            sb.AppendFormat("岗位风险-行:{0}<br />", j);
                            error++;
                        }
                    }
                    catch (Exception ex)
                    {
                        sb.AppendFormat("岗位风险-行:{0}({1})<br />", j, ex.Message);
                        error++;
                    }

                }
                if (dt.Rows.Count > 1)
                {
                    count += dt.Rows.Count - 1;
                }
                #endregion
                #region 工器具及危化品风险

                cells = wb.Worksheets[5].Cells;
                dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn, false);
                for (int j = 1; j < dt.Rows.Count; j++)
                {
                    areaId = "";
                    areaCode = "";
                    //区域名称
                    string areaName = dt.Rows[j][2].ToString();
                    //管控部门
                    string deptName = dt.Rows[j][24].ToString().Trim();
                    if (string.IsNullOrEmpty(deptName))
                    {
                        sb.AppendFormat("管控部门不能为空！工器具及危化品风险库-行:{0}<br />", j);
                        error++;
                        continue;
                    }
                    try
                    {
                        //获取区域信息并判断该区域是否存在，没有则新增该区域
                        if (!string.IsNullOrEmpty(areaName))
                        {
                            dis = districtBLL.GetDistrict(user.OrganizeId, areaName);
                            if (dis != null)
                            {
                                areaId = dis.DistrictID;
                                areaCode = dis.DistrictCode;
                            }
                            else
                            {
                                sb.AppendFormat("区域与电厂区域不匹配！工器具及危化品风险库-行:{0}<br />", j);
                                error++;
                                //continue;
                            }
                        }

                        //风险点类别
                        string matype = dt.Rows[j][3].ToString().Trim();
                        //工器具及危化品--对应工器具及危化品风险
                        string toolordanger = dt.Rows[j][4].ToString().Trim();
                        string packuntil = dt.Rows[j][5].ToString().Trim();
                        int packnum = 0;
                        try
                        {
                            packnum = Convert.ToInt32(dt.Rows[j][6].ToString().Trim());
                        }
                        catch (Exception)
                        {
                            packnum = 0;
                        }
                        string space = dt.Rows[j][7].ToString().Trim();
                        //危险源类别
                        string dangerSourcetype = dt.Rows[j][8].ToString().Trim();
                        //危险源
                        string dangerSource = dt.Rows[j][9].ToString().Trim();

                        //风险类别
                        string riskType = "工器具及危化品";
                        //风险描述
                        string desc = dt.Rows[j][10].ToString().Trim();
                        //风险后果
                        string result = dt.Rows[j][11].ToString().Trim();
                        //评价方式
                        string way = dt.Rows[j][12].ToString().Trim();

                        string itemA = string.Empty;
                        string itemB = string.Empty;
                        string itemC = string.Empty;
                        //辨识评价可能性
                        itemA = dt.Rows[j][13].ToString().Trim();
                        if (way == "LEC")
                        {
                            //辨识评价频繁程度
                            itemB = dt.Rows[j][15].ToString().Trim();
                            //辨识评价后果
                            itemC = dt.Rows[j][14].ToString().Trim();
                        }
                        else
                        {
                            //辨识评价频繁程度
                            itemB = dt.Rows[j][14].ToString().Trim();
                            //辨识评价后果
                            itemC = dt.Rows[j][15].ToString().Trim();
                        }

                        ////辨识评价可能性
                        //string itemA = dt.Rows[j][13].ToString().Trim();
                        ////辨识评价频繁程度
                        //string itemB = dt.Rows[j][14].ToString().Trim();
                        ////辨识评价后果
                        //string itemC = dt.Rows[j][15].ToString().Trim();
                        //辨识评价风险值
                        string itemR = dt.Rows[j][16].ToString().Trim();
                        //辨识评价风险等级
                        string grade = dt.Rows[j][17].ToString().Trim();
                        //工程技术措施
                        string gcjscs = dt.Rows[j][18].ToString().Trim();
                        //管理措施
                        string glcs = dt.Rows[j][19].ToString().Trim();
                        //个人防护措施
                        string grfhcs = dt.Rows[j][20].ToString().Trim();
                        //培训教育措施
                        string pxjycs = dt.Rows[j][21].ToString().Trim();
                        //应急处置措施
                        string yjczcs = dt.Rows[j][22].ToString().Trim();
                        List<string> values = new List<string> { gcjscs, glcs, grfhcs, pxjycs, yjczcs };
                        string measures = "";
                        foreach (string str in values)
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                measures += str + "\r\n";
                            }
                        }
                        //管控层级
                        string level = dt.Rows[j][23].ToString().Trim();
                        //岗位
                        string postName = dt.Rows[j][25].ToString().Trim();
                        //管控责任人
                        string dutyperson = dt.Rows[j][26].ToString().Trim();

                        RiskAssessEntity risk = new RiskAssessEntity();
                        risk.DeptCode = deptBll.GetDeptCode(deptName, user.OrganizeId);
                        risk.PostId = deptBll.GetPostId(postName, user.OrganizeId, risk.DeptCode);
                        if (string.IsNullOrEmpty(risk.DeptCode))
                        {
                            sb.AppendFormat("部门与系统的部门信息不匹配！工器具及危化品风险库-行:{0}<br />", j);
                            error++;
                            continue;
                        }
                        risk.DeptId = deptBll.GetEntityByCode(risk.DeptCode).DepartmentId;
                        risk.Id = Guid.NewGuid().ToString();
                        risk.Result = result;
                        risk.RiskType = riskType;
                        risk.RiskDesc = desc;
                        risk.PackNum = packnum;
                        risk.PackUntil = packuntil;
                        risk.StorageSpace = space;
                        risk.DutyPerson = dutyperson;
                        risk.PostName = postName;
                        risk.DeptName = deptName;
                        risk.AreaId = areaId;
                        risk.LevelName = level;
                        risk.DistrictId = areaId;
                        risk.AreaCode = areaCode;
                        risk.AreaName = risk.DistrictName = areaName;
                        risk.Way = way;
                        risk.Status = 1;
                        risk.ToolOrDanger = toolordanger;
                        risk.DangerSourceType = dangerSourcetype;
                        risk.MajorNameType = matype;
                        risk.Measure = measures;
                        risk.DangerSource = dangerSource;
                        risk.ItemA = string.IsNullOrEmpty(itemA) ? 0 : decimal.Parse(itemA);
                        risk.ItemB = string.IsNullOrEmpty(itemB) ? 0 : decimal.Parse(itemB);
                        risk.ItemC = string.IsNullOrEmpty(itemC) ? 0 : decimal.Parse(itemC);
                      
                        SetRiskGrade(risk);
                        risk.DeptName = deptName;
                        RiskAssessBLL riskBLL = new RiskAssessBLL();

                        if (riskBLL.SaveForm("", risk) > 0)
                        {
                            int k = 0;
                            foreach (string str in values)
                            {
                                if (!string.IsNullOrEmpty(str))
                                {
                                    MeasuresEntity measure = new MeasuresEntity();
                                    measure.TypeName = types[k].Replace("措施", "");
                                    measure.RiskId = risk.Id;
                                    measure.Content = str;
                                    measure.AreaId = areaId;
                                    measureBLL.Save("", measure);
                                }
                                k++;
                            }
                        }
                        else
                        {
                            sb.AppendFormat("工器具及危化品风险-行:{0}<br />", j);
                            error++;
                        }
                    }
                    catch (Exception ex)
                    {
                        sb.AppendFormat("工器具及危化品风险-行:{0}({1})<br />", j, ex.Message);
                        error++;
                    }

                }
                if (dt.Rows.Count > 1)
                {
                    count += dt.Rows.Count - 1;
                }
                #endregion

                message = "共有" + count + "条记录,成功导入" + (count - error) + "条，失败" + error + "条。";
                if (error > 0)
                {
                    message += sb.ToString();
                }
            }

            return message;
        }
        /// <summary>
        /// 通用版本导入
        /// </summary>
        /// <returns></returns>
        private string ImportCommonRiskData()
        {
            string message = "请选择格式正确的文件再导入!";
            try
            {
                int error = 0;
                StringBuilder sb = new StringBuilder("具体错误位置：<br />");
                int count = HttpContext.Request.Files.Count;
                if (count > 0)
                {
                    count = 0;
                    HttpPostedFileBase file = HttpContext.Request.Files[0];
                    if (string.IsNullOrEmpty(file.FileName))
                    {
                        return message;
                    }
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName));
                    Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                    wb.Open(Server.MapPath("~/Resource/temp/" + fileName), Aspose.Cells.FileFormatType.Excel2003);

                    Aspose.Cells.Cells cells = wb.Worksheets[0].Cells;

                    List<string> types = new List<string> { "工程技术", "管理", "个体防护", "培训教育", "应急处置" };

                    Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                    #region 作业风险库
                    DepartmentBLL deptBll = new DepartmentBLL();
                    DataTable dt = cells.ExportDataTable(2, 0, cells.MaxDataRow, cells.MaxColumn - 1, false);
                    MeasuresBLL measureBLL = new MeasuresBLL();

                    DistrictEntity dis = null;
                    string areaId = "";
                    string areaCode = "";
                    //导入作业类型风险清单时,导入的数据同步到预知训练库
                    DataTable TrainDt = dt.Clone();
                    for (int j = 1; j < dt.Rows.Count; j++)
                    {
                        areaId = "";
                        areaCode = "";
                        //区域名称
                        string areaName = dt.Rows[j][2].ToString();
                        //岗位
                        string postName = dt.Rows[j][22].ToString().Trim();
                        //管控部门
                        string deptName = dt.Rows[j][21].ToString().Trim();
                        if (string.IsNullOrEmpty(deptName))
                        {
                            sb.AppendFormat("管控部门不能为空！作业风险库-行:{0}<br />", j);
                            error++;
                            continue;
                        }
                        try
                        {
                            //工作任务
                            string workTask = dt.Rows[j][4].ToString().Trim();
                            if (string.IsNullOrEmpty(workTask))
                            {
                                sb.AppendFormat("工作任务不能为空！作业风险库-行:{0}<br />", j);
                                error++;
                                continue;
                            }
                            //获取区域信息并判断该区域是否存在，没有则新增该区域
                            if (!string.IsNullOrEmpty(areaName))
                            {
                                dis = districtBLL.GetDistrict(user.OrganizeId, areaName);
                                if (dis != null)
                                {
                                    areaId = dis.DistrictID;
                                    areaCode = dis.DistrictCode;
                                }
                                else
                                {
                                    sb.AppendFormat("区域与电厂区域不匹配！作业风险库-行:{0}<br />", j);
                                    error++;
                                }
                            }
                            else {
                                sb.AppendFormat("所属区域不能为空！作业风险库-行:{0}<br />", j);
                                error++;
                                continue;
                            } 
                            //管控层级
                            string level = dt.Rows[j][20].ToString().Trim();
                            //危害源
                            string dangerSource = dt.Rows[j][6].ToString().Trim();
                            //风险描述
                            string desc = dt.Rows[j][7].ToString().Trim();
                            //风险类别
                            string riskType = "作业";
                            //风险后果
                            string result = dt.Rows[j][8].ToString().Trim();
                            //工程技术措施
                            string gcjscs = dt.Rows[j][15].ToString().Trim();
                            //管理措施
                            string glcs = dt.Rows[j][16].ToString().Trim();
                            //个人防护措施
                            string grfhcs = dt.Rows[j][17].ToString().Trim();
                            //培训教育措施
                            string pxjycs = dt.Rows[j][18].ToString().Trim();
                            //应急处置措施
                            string yjczcs = dt.Rows[j][19].ToString().Trim();
                            List<string> values = new List<string> { gcjscs, glcs, grfhcs, pxjycs, yjczcs };
                            string measures = "";
                            foreach (string str in values)
                            {
                                if (!string.IsNullOrEmpty(str))
                                {
                                    measures += str + "\r\n";
                                }
                            }
                            //评价方式
                            string way = dt.Rows[j][9].ToString().Trim();
                            string itemA = string.Empty;
                            string itemB = string.Empty;
                            string itemC = string.Empty;
                            //辨识评价可能性
                            itemA = dt.Rows[j][10].ToString().Trim();
                            if (way == "LEC")
                            {
                                //辨识评价频繁程度
                                itemB = dt.Rows[j][12].ToString().Trim();
                                //辨识评价后果
                                itemC = dt.Rows[j][11].ToString().Trim();
                            }
                            else {
                                //辨识评价频繁程度
                                itemB = dt.Rows[j][11].ToString().Trim();
                                //辨识评价后果
                                itemC = dt.Rows[j][12].ToString().Trim();
                            }
                            //辨识评价风险值
                            string itemR = dt.Rows[j][13].ToString().Trim();
                            //辨识评价风险等级
                            string grade = dt.Rows[j][14].ToString().Trim();
                            //工序
                            string process = dt.Rows[j][5].ToString().Trim();
                            string worktype = dt.Rows[j][3].ToString().Trim();
                            RiskAssessEntity risk = new RiskAssessEntity();
                            risk.DeptCode = deptBll.GetDeptCode(deptName, user.OrganizeId);
                            risk.PostId = deptBll.GetPostId(postName, user.OrganizeId, risk.DeptCode);
                            if (string.IsNullOrEmpty(risk.DeptCode))
                            {
                                sb.AppendFormat("部门与系统的部门信息不匹配！作业风险库-行:{0}<br />", j);
                                error++;
                                continue;
                            }

                            if (!string.IsNullOrWhiteSpace(worktype))
                            {
                                var data = new DataItemDetailBLL().GetDataItemListByItemCode("'StatisticsType'");
                                var list = worktype.Split(',');
                                for (int k = 0; k < list.Length; k++)
                                {
                                    var entity = data.Where(x => x.ItemName == list[k].Trim()).FirstOrDefault();
                                    if (entity != null)
                                    {
                                        if (string.IsNullOrWhiteSpace(risk.WorkType))
                                        {

                                            risk.WorkTypeCode += entity.ItemValue + ",";
                                            risk.WorkType += entity.ItemName + ",";

                                        }
                                        else
                                        {
                                            if (!risk.WorkType.Contains(entity.ItemName))
                                            {
                                                risk.WorkTypeCode += entity.ItemValue + ",";
                                                risk.WorkType += entity.ItemName + ",";
                                            }
                                        }
                                    }
                                }
                                if (!string.IsNullOrWhiteSpace(risk.WorkTypeCode))
                                {
                                    risk.WorkTypeCode = risk.WorkTypeCode.Substring(0, risk.WorkTypeCode.Length - 1);
                                    risk.WorkType = risk.WorkType.Substring(0, risk.WorkType.Length - 1);
                                }
                            }
                            risk.Id = Guid.NewGuid().ToString();
                            risk.Result = result;
                            risk.RiskType = riskType;
                            risk.RiskDesc = desc;
                            risk.PostName = postName;
                            risk.DistrictId = areaId;
                            risk.AreaName = risk.DistrictName = areaName;
                            risk.AreaCode = areaCode;
                            risk.WorkTask = workTask;
                            risk.Process = process;
                            risk.Way = way;
                            risk.LevelName = level;
                            risk.DeptName = deptName;
                            risk.Measure = measures;
                            risk.DangerSource = dangerSource;
                            risk.ItemA = string.IsNullOrEmpty(itemA) ? 0 : decimal.Parse(itemA);
                            risk.ItemB = string.IsNullOrEmpty(itemB) ? 0 : decimal.Parse(itemB);
                            risk.ItemC = string.IsNullOrEmpty(itemC) ? 0 : decimal.Parse(itemC);
                            risk.Status = 1;
                            SetRiskGrade(risk);
                            risk.DeptName = deptName;
                            RiskAssessBLL riskBLL = new RiskAssessBLL();

                            if (riskBLL.SaveForm("", risk) > 0)
                            {
                                int k = 0;
                                foreach (string str in values)
                                {
                                    if (!string.IsNullOrEmpty(str))
                                    {
                                        MeasuresEntity measure = new MeasuresEntity();
                                        measure.TypeName = types[k].Replace("措施", "");
                                        measure.RiskId = risk.Id;
                                        measure.Content = str;
                                        measure.AreaId = areaId;
                                        measureBLL.Save("", measure);
                                    }
                                    k++;
                                }
                                DataRow row = TrainDt.NewRow();
                                row.ItemArray = dt.Rows[j].ItemArray;
                                TrainDt.Rows.Add(row);
                            }
                            else
                            {
                                sb.AppendFormat("作业风险库-行:{0}<br />", j);
                                error++;
                            }
                        }
                        catch (Exception ex)
                        {
                            sb.AppendFormat("作业风险库-行:{0}({1})<br />", j, ex.Message);
                            error++;
                        }
                    }
                    Dictionary<string, DataRow[]> dic = new Dictionary<string, DataRow[]>();
                    //开始同步数据 TrainDt为已经导入风险清单的数据
                    for (int i = 0; i < TrainDt.Rows.Count; i++)
                    {
                        if (dic.Keys.Contains(TrainDt.Rows[i][4].ToString() + TrainDt.Rows[i][2].ToString()))
                        {
                            continue;
                        }
                        else
                        {
                            dic.Add(TrainDt.Rows[i][4].ToString() + TrainDt.Rows[i][2].ToString(), TrainDt.Select(string.Format("Column5='{0}' and Column3='{1}'", TrainDt.Rows[i][4].ToString(), TrainDt.Rows[i][2].ToString())));
                        }
                    }

                    List<RisktrainlibEntity> RiskLib = new List<RisktrainlibEntity>();
                    List<RisktrainlibdetailEntity> detailLib = new List<RisktrainlibdetailEntity>();
                    foreach (string key in dic.Keys)
                    {
                        DataRow[] item = dic[key];
                        var entity = new RisktrainlibEntity();
                        entity.Create();
                        entity.WorkTask = item[0][4].ToString();
                        int level = 4;
                        var data = new DataItemDetailBLL().GetDataItemListByItemCode("'StatisticsType'");
                        foreach (DataRow r in item)
                        {
                            entity.RiskLevel = r[14].ToString();
                            switch (entity.RiskLevel)
                            {
                                case "重大风险":
                                    entity.RiskLevelVal = "1";
                                    break;
                                case "较大风险":
                                    entity.RiskLevelVal = "2";
                                    break;
                                case "一般风险":
                                    entity.RiskLevelVal = "3";
                                    break;
                                case "低风险":
                                    entity.RiskLevelVal = "4";
                                    break;
                                default:
                                    break;
                            }
                            if (Convert.ToInt32(entity.RiskLevelVal) < level)
                            {
                                level = Convert.ToInt32(entity.RiskLevelVal);
                            }

                            if (!string.IsNullOrWhiteSpace(r[3].ToString()))
                            {
                                var list = r[3].ToString().Split(',');
                                for (int k = 0; k < list.Length; k++)
                                {
                                    var e = data.Where(x => x.ItemName == list[k].Trim()).FirstOrDefault();
                                    if (e != null)
                                    {
                                        if (string.IsNullOrWhiteSpace(entity.WorkType))
                                        {
                                            entity.WorkTypeCode += e.ItemValue + ",";
                                            entity.WorkType += e.ItemName + ",";
                                        }
                                        else
                                        {
                                            if (!entity.WorkType.Contains(e.ItemName))
                                            {
                                                entity.WorkTypeCode += e.ItemValue + ",";
                                                entity.WorkType += e.ItemName + ",";
                                            }
                                        }

                                    }
                                }
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(entity.WorkTypeCode))
                        {
                            entity.WorkTypeCode = entity.WorkTypeCode.Substring(0, entity.WorkTypeCode.Length - 1);
                            entity.WorkType = entity.WorkType.Substring(0, entity.WorkType.Length - 1);
                        }
                        entity.RiskLevelVal = level.ToString();
                        switch (entity.RiskLevelVal)
                        {
                            case "1":
                                entity.RiskLevel = "重大风险";
                                break;
                            case "2":
                                entity.RiskLevel = "较大风险";
                                break;
                            case "3":
                                entity.RiskLevel = "一般风险";
                                break;
                            case "4":
                                entity.RiskLevel = "低风险";
                                break;
                            default:
                                break;
                        }
                        if (string.IsNullOrWhiteSpace(item[0][2].ToString()))
                        {
                            //falseMessage += "</br>" + "第" + (i+1) + "行区域为空,未能导入.";
                            //error++;
                            //continue;
                        }
                        else
                        {
                            entity.WorkArea = item[0][2].ToString();
                            DistrictEntity disEntity = districtBLL.GetDistrict(user.OrganizeId, item[0][2].ToString());
                            if (disEntity == null)
                            {
                                //电厂没有该区域则不赋值
                                entity.WorkArea = "";
                            }
                            else
                            {
                                entity.WorkAreaId = disEntity.DistrictID;
                            }
                        }
                        entity.WorkPost = item[0][22].ToString();
                        entity.DataSources = "1";
                        RiskLib.Add(entity);
                        foreach (DataRow it in item)
                        {
                            var dentity = new RisktrainlibdetailEntity();
                            dentity.Process = it[5].ToString();
                            dentity.AtRisk = it[6].ToString() + it[7].ToString();
                            //工程技术措施
                            string gcjscs = it[15].ToString().Trim();
                            //管理措施
                            string glcs = it[16].ToString().Trim();
                            //个人防护措施
                            string grfhcs = it[17].ToString().Trim();
                            //培训教育措施
                            string pxjycs = it[18].ToString().Trim();
                            //应急处置措施
                            string yjczcs = it[19].ToString().Trim();
                            List<string> values = new List<string> { gcjscs, glcs, grfhcs, pxjycs, yjczcs };
                            string measures = "";
                            foreach (string str in values)
                            {
                                if (!string.IsNullOrEmpty(str))
                                {
                                    measures += str + "\r\n";
                                }
                            }
                            dentity.Controls = measures;
                            dentity.Create();
                            dentity.WorkId = entity.ID;
                            detailLib.Add(dentity);
                        }
                    }
                    new RisktrainlibBLL().InsertImportData(RiskLib, detailLib);
                    if (dt.Rows.Count > 1)
                    {
                        count += dt.Rows.Count - 1;
                    }
                    #endregion

                    #region 设备风险库

                    cells = wb.Worksheets[1].Cells;
                    dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn, false);
                    for (int j = 1; j < dt.Rows.Count; j++)
                    {
                        areaId = "";
                        areaCode = "";
                        //区域名称
                        string areaName = dt.Rows[j][2].ToString();
                        //管控部门
                        string deptName = dt.Rows[j][20].ToString().Trim();
                        if (string.IsNullOrEmpty(deptName))
                        {
                            sb.AppendFormat("管控部门不能为空！设备风险库-行:{0}<br />", j);
                            error++;
                            continue;
                        }
                        try
                        {
                            //获取区域信息并判断该区域是否存在，没有则新增该区域
                            if (!string.IsNullOrEmpty(areaName))
                            {
                                dis = districtBLL.GetDistrict(user.OrganizeId, areaName);
                                if (dis != null)
                                {
                                    areaId = dis.DistrictID;
                                    areaCode = dis.DistrictCode;
                                }
                                else
                                {
                                    sb.AppendFormat("区域与电厂区域不匹配！设备风险库-行:{0}<br />", j);
                                    error++;
                                }
                            }
                            //岗位
                            string postName = dt.Rows[j][21].ToString().Trim();
                            //管控层级
                            string level = dt.Rows[j][19].ToString().Trim();
                            //危害源
                            string dangerSource = dt.Rows[j][5].ToString().Trim();
                            //风险类别
                            string riskType = "设备";
                            //风险后果
                            string result = dt.Rows[j][7].ToString().Trim();
                            //工程技术措施
                            string gcjscs = dt.Rows[j][14].ToString().Trim();
                            //管理措施
                            string glcs = dt.Rows[j][15].ToString().Trim();
                            //个人防护措施
                            string grfhcs = dt.Rows[j][16].ToString().Trim();
                            //培训教育措施
                            string pxjycs = dt.Rows[j][17].ToString().Trim();
                            //应急处置措施
                            string yjczcs = dt.Rows[j][18].ToString().Trim();
                            List<string> values = new List<string> { gcjscs, glcs, grfhcs, pxjycs, yjczcs };
                            string measures = "";
                            foreach (string str in values)
                            {
                                if (!string.IsNullOrEmpty(str))
                                {
                                    measures += str + "\r\n";
                                }
                            }
                            //评价方式
                            string way = dt.Rows[j][8].ToString().Trim();

                            string itemA = string.Empty;
                            string itemB = string.Empty;
                            string itemC = string.Empty;
                            //辨识评价可能性
                            itemA = dt.Rows[j][9].ToString().Trim();
                            if (way == "LEC")
                            {
                                //辨识评价频繁程度
                                itemB = dt.Rows[j][11].ToString().Trim();
                                //辨识评价后果
                                itemC = dt.Rows[j][10].ToString().Trim();
                            }
                            else
                            {
                                //辨识评价频繁程度
                                itemB = dt.Rows[j][10].ToString().Trim();
                                //辨识评价后果
                                itemC = dt.Rows[j][11].ToString().Trim();
                            }
                            ////辨识评价可能性
                            //string itemA = dt.Rows[j][9].ToString().Trim();
                            ////辨识评价频繁程度
                            //string itemB = dt.Rows[j][10].ToString().Trim();
                            ////辨识评价后果
                            //string itemC = dt.Rows[j][11].ToString().Trim();
                            //辨识评价风险值
                            string itemR = dt.Rows[j][12].ToString().Trim();
                            //辨识评价风险等级
                            string grade = dt.Rows[j][13].ToString().Trim();
                            //设备名称
                            string machineName = dt.Rows[j][3].ToString().Trim();
                            //部件
                            string parts = dt.Rows[j][4].ToString().Trim();

                            //风险描述
                            string desc = parts + dt.Rows[j][6].ToString().Trim();

                            RiskAssessEntity risk = new RiskAssessEntity();
                            risk.DeptCode = deptBll.GetDeptCode(deptName, user.OrganizeId);
                            risk.PostId = deptBll.GetPostId(postName, user.OrganizeId, risk.DeptCode);
                            if (string.IsNullOrEmpty(risk.DeptCode))
                            {
                                sb.AppendFormat("部门与系统的部门信息不匹配！设备风险库-行:{0}<br />", j);
                                error++;
                                continue;
                            }
                            risk.Id = Guid.NewGuid().ToString();
                            risk.RiskType = riskType;
                            risk.RiskDesc = desc;
                            risk.PostName = postName;
                            risk.DeptName = deptName;
                            risk.Result = result;
                            risk.LevelName = level;
                            risk.AreaCode = areaCode;
                            risk.DistrictId = areaId;
                            risk.AreaName = risk.DistrictName = areaName;
                            risk.EquipmentName = machineName;
                            risk.Parts = parts;
                            risk.Way = way;
                            risk.Status = 1;
                            risk.Measure = measures;
                            risk.DangerSource = dangerSource;
                            risk.FaultType = dangerSource;
                            risk.ItemA = string.IsNullOrEmpty(itemA) ? 0 : decimal.Parse(itemA);
                            risk.ItemB = string.IsNullOrEmpty(itemB) ? 0 : decimal.Parse(itemB);
                            risk.ItemC = string.IsNullOrEmpty(itemC) ? 0 : decimal.Parse(itemC);
                            SetRiskGrade(risk);
                            risk.DeptName = deptName;
                            RiskAssessBLL riskBLL = new RiskAssessBLL();

                            if (riskBLL.SaveForm("", risk) > 0)
                            {
                                int k = 0;
                                foreach (string str in values)
                                {
                                    if (!string.IsNullOrEmpty(str))
                                    {

                                        MeasuresEntity measure = new MeasuresEntity();
                                        measure.TypeName = types[k].Replace("措施", "");
                                        measure.RiskId = risk.Id;
                                        measure.Content = str;
                                        measure.AreaId = areaId;
                                        measureBLL.Save("", measure);
                                    }
                                    k++;
                                }
                            }
                            else
                            {
                                sb.AppendFormat("设备风险库-行:{0}<br />", j);
                                error++;
                            }
                        }
                        catch (Exception ex)
                        {
                            sb.AppendFormat("设备风险库-行:{0}({1})<br />", j, ex.Message);
                            error++;
                        }

                    }
                    if (dt.Rows.Count > 1)
                    {
                        count += dt.Rows.Count - 1;
                    }
                    #endregion

                    #region 环境风险库

                    cells = wb.Worksheets[2].Cells;
                    dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn, false);
                    for (int j = 1; j < dt.Rows.Count; j++)
                    {
                        areaId = "";
                        areaCode = "";
                        //区域名称
                        string areaName = dt.Rows[j][2].ToString();
                        //管控部门
                        string deptName = dt.Rows[j][20].ToString().Trim();
                        if (string.IsNullOrEmpty(deptName))
                        {
                            sb.AppendFormat("管控部门不能为空！区域风险库-行:{0}<br />", j);
                            error++;
                            continue;
                        }
                        try
                        {
                            //获取区域信息并判断该区域是否存在，没有则新增该区域
                            if (!string.IsNullOrEmpty(areaName))
                            {
                                dis = districtBLL.GetDistrict(user.OrganizeId, areaName);
                                if (dis != null)
                                {
                                    areaId = dis.DistrictID;
                                    areaCode = dis.DistrictCode;
                                }
                                else
                                {
                                    sb.AppendFormat("区域与电厂区域不匹配！区域风险库-行:{0}<br />", j);
                                    error++;
                                    //continue;
                                }
                            }
                            //岗位
                            string postName = dt.Rows[j][21].ToString().Trim();
                            //管控层级
                            string level = dt.Rows[j][19].ToString().Trim();
                            //危险源
                            string dangerSource = dt.Rows[j][5].ToString().Trim();

                            //风险类别
                            string riskType = "区域";
                            //风险后果
                            string result = dt.Rows[j][7].ToString().Trim();
                            //工程技术措施
                            string gcjscs = dt.Rows[j][14].ToString().Trim();
                            //管理措施
                            string glcs = dt.Rows[j][15].ToString().Trim();
                            //个人防护措施
                            string grfhcs = dt.Rows[j][16].ToString().Trim();
                            //培训教育措施
                            string pxjycs = dt.Rows[j][17].ToString().Trim();
                            //应急处置措施
                            string yjczcs = dt.Rows[j][18].ToString().Trim();
                            List<string> values = new List<string> { gcjscs, glcs, grfhcs, pxjycs, yjczcs };
                            string measures = "";
                            foreach (string str in values)
                            {
                                if (!string.IsNullOrEmpty(str))
                                {
                                    measures += str + "\r\n";
                                }
                            }
                            //评价方式
                            string way = dt.Rows[j][8].ToString().Trim();

                            string itemA = string.Empty;
                            string itemB = string.Empty;
                            string itemC = string.Empty;
                            //辨识评价可能性
                            itemA = dt.Rows[j][9].ToString().Trim();
                            if (way == "LEC")
                            {
                                //辨识评价频繁程度
                                itemB = dt.Rows[j][11].ToString().Trim();
                                //辨识评价后果
                                itemC = dt.Rows[j][10].ToString().Trim();
                            }
                            else
                            {
                                //辨识评价频繁程度
                                itemB = dt.Rows[j][10].ToString().Trim();
                                //辨识评价后果
                                itemC = dt.Rows[j][11].ToString().Trim();
                            }

                            ////辨识评价可能性
                            //string itemA = dt.Rows[j][9].ToString().Trim();
                            ////辨识评价频繁程度
                            //string itemB = dt.Rows[j][10].ToString().Trim();
                            ////辨识评价后果
                            //string itemC = dt.Rows[j][11].ToString().Trim();
                            //辨识评价风险值
                            string itemR = dt.Rows[j][12].ToString().Trim();
                            //辨识评价风险等级
                            string grade = dt.Rows[j][13].ToString().Trim();
                            //风险描述
                            string desc = dt.Rows[j][6].ToString().Trim();
                            //系统-对应环境风险
                            string hjsystem = dt.Rows[j][3].ToString().Trim();
                            //设备-对应环境风险
                            string hjequpment = dt.Rows[j][4].ToString().Trim();
                            RiskAssessEntity risk = new RiskAssessEntity();
                            risk.DeptCode = deptBll.GetDeptCode(deptName, user.OrganizeId);
                            risk.PostId = deptBll.GetPostId(postName, user.OrganizeId, risk.DeptCode);
                            if (string.IsNullOrEmpty(risk.DeptCode))
                            {
                                sb.AppendFormat("部门与系统的部门信息不匹配！区域风险库-行:{0}<br />", j);
                                error++;
                                continue;
                            }
                            risk.Id = Guid.NewGuid().ToString();
                            risk.Result = result;
                            risk.RiskType = riskType;
                            risk.RiskDesc = desc;
                            risk.PostName = postName;
                            risk.DeptName = deptName;
                            risk.AreaId = areaId;
                            risk.LevelName = level;
                            risk.DistrictId = areaId;
                            risk.AreaCode = areaCode;
                            risk.AreaName = risk.DistrictName = areaName;
                            risk.Way = way;
                            risk.Status = 1;
                            risk.HjSystem = hjsystem;
                            risk.HjEqupment = hjequpment;
                            risk.Measure = measures;
                            risk.DangerSource = dangerSource;
                            risk.ItemA = string.IsNullOrEmpty(itemA) ? 0 : decimal.Parse(itemA);
                            risk.ItemB = string.IsNullOrEmpty(itemB) ? 0 : decimal.Parse(itemB);
                            risk.ItemC = string.IsNullOrEmpty(itemC) ? 0 : decimal.Parse(itemC);
                            SetRiskGrade(risk);
                            risk.DeptName = deptName;
                            RiskAssessBLL riskBLL = new RiskAssessBLL();

                            if (riskBLL.SaveForm("", risk) > 0)
                            {
                                int k = 0;
                                foreach (string str in values)
                                {
                                    if (!string.IsNullOrEmpty(str))
                                    {
                                        MeasuresEntity measure = new MeasuresEntity();
                                        measure.TypeName = types[k].Replace("措施", "");
                                        measure.RiskId = risk.Id;
                                        measure.Content = str;
                                        measure.AreaId = areaId;
                                        measureBLL.Save("", measure);
                                    }
                                    k++;
                                }
                            }
                            else
                            {
                                sb.AppendFormat("区域风险库-行:{0}<br />", j);
                                error++;
                            }
                        }
                        catch (Exception ex)
                        {
                            sb.AppendFormat("区域风险库-行:{0}({1})<br />", j, ex.Message);
                            error++;
                        }

                    }
                    if (dt.Rows.Count > 1)
                    {
                        count += dt.Rows.Count - 1;
                    }
                    #endregion

                    #region 管理风险库


                    cells = wb.Worksheets[3].Cells;
                    dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn, false);
                    for (int j = 1; j < dt.Rows.Count; j++)
                    {
                        areaId = "";
                        areaCode = "";
                        //获取区域信息并判断该区域是否存在，没有则新增该区域
                        string aName = dt.Rows[j][2].ToString().Trim();
                        //管控部门
                        string deptName = dt.Rows[j][18].ToString().Trim();
                        if (string.IsNullOrEmpty(deptName))
                        {
                            sb.AppendFormat("管控部门不能为空！管理风险库-行:{0}<br />", j);
                            error++;
                            continue;
                        }
                        if (!string.IsNullOrEmpty(aName))
                        {
                            dis = districtBLL.GetDistrict(user.OrganizeId, aName);
                            if (dis != null)
                            {
                                areaId = dis.DistrictID;
                                areaCode = dis.DistrictCode;
                            }
                            else
                            {
                                sb.AppendFormat("区域与电厂区域不匹配！管理风险库-行:{0}<br />", j);
                                error++;
                                //continue;
                            }
                        }

                        //岗位
                        string postName = dt.Rows[j][19].ToString().Trim();

                        if (string.IsNullOrEmpty(deptName))
                        {
                            continue;
                        }
                        //管控层级
                        string level = dt.Rows[j][17].ToString().Trim();
                        ////风险后果分类
                        //string resultType = dt.Rows[j][3].ToString().Trim();
                        //危险源
                        string dangerSource = dt.Rows[j][3].ToString().Trim();

                        //风险类别
                        string riskType = "管理";
                        //风险后果
                        string result = dt.Rows[j][5].ToString().Trim();
                        //工程技术措施
                        string gcjscs = dt.Rows[j][12].ToString().Trim();
                        //管理措施
                        string glcs = dt.Rows[j][13].ToString().Trim();
                        //个人防护措施
                        string grfhcs = dt.Rows[j][14].ToString().Trim();
                        //培训教育措施
                        string pxjycs = dt.Rows[j][15].ToString().Trim();
                        //应急处置措施
                        string yjczcs = dt.Rows[j][16].ToString().Trim();
                        List<string> values = new List<string> { gcjscs, glcs, grfhcs, pxjycs, yjczcs };
                        string measures = "";
                        foreach (string str in values)
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                measures += str + "\r\n";
                            }
                        }
                        //评价方式
                        string way = dt.Rows[j][6].ToString().Trim();

                        string itemA = string.Empty;
                        string itemB = string.Empty;
                        string itemC = string.Empty;
                        //辨识评价可能性
                        itemA = dt.Rows[j][7].ToString().Trim();
                        if (way == "LEC")
                        {
                            //辨识评价频繁程度
                            itemB = dt.Rows[j][9].ToString().Trim();
                            //辨识评价后果
                            itemC = dt.Rows[j][8].ToString().Trim();
                        }
                        else
                        {
                            //辨识评价频繁程度
                            itemB = dt.Rows[j][8].ToString().Trim();
                            //辨识评价后果
                            itemC = dt.Rows[j][9].ToString().Trim();
                        }


                        ////辨识评价可能性
                        //string itemA = dt.Rows[j][7].ToString().Trim();
                        ////辨识评价频繁程度
                        //string itemB = dt.Rows[j][8].ToString().Trim();
                        ////辨识评价后果
                        //string itemC = dt.Rows[j][9].ToString().Trim();
                        //辨识评价风险值
                        string itemR = dt.Rows[j][10].ToString().Trim();
                        //辨识评价风险等级
                        string grade = dt.Rows[j][11].ToString().Trim();
                        //风险描述
                        string desc = dt.Rows[j][4].ToString().Trim();

                        RiskAssessEntity risk = new RiskAssessEntity();
                        risk.DeptCode = deptBll.GetDeptCode(deptName, user.OrganizeId);
                        risk.PostId = deptBll.GetPostId(postName, user.OrganizeId, risk.DeptCode);
                        if (string.IsNullOrEmpty(risk.DeptCode))
                        {
                            sb.AppendFormat("部门与系统的部门信息不匹配！管理风险库-行:{0}<br />", j);
                            error++;
                            continue;
                        }
                        risk.Id = Guid.NewGuid().ToString();
                        risk.RiskType = riskType;
                        risk.RiskDesc = desc;
                        risk.PostName = postName;
                        risk.DeptName = deptName;
                        risk.Result = result;
                        risk.DistrictId = areaId;
                        risk.AreaCode = areaCode;
                        risk.DistrictName = aName;
                        risk.Way = way;
                        risk.Status = 1;
                        risk.LevelName = level;
                        risk.Measure = measures;
                        risk.DangerSource = dangerSource;
                        risk.ItemA = string.IsNullOrEmpty(itemA) ? 0 : decimal.Parse(itemA);
                        risk.ItemB = string.IsNullOrEmpty(itemB) ? 0 : decimal.Parse(itemB);
                        risk.ItemC = string.IsNullOrEmpty(itemC) ? 0 : decimal.Parse(itemC);
                        SetRiskGrade(risk);
                        risk.DeptName = deptName;
                        RiskAssessBLL riskBLL = new RiskAssessBLL();

                        if (riskBLL.SaveForm("", risk) > 0)
                        {
                            int k = 0;
                            foreach (string str in values)
                            {
                                if (!string.IsNullOrEmpty(str))
                                {
                                    MeasuresEntity measure = new MeasuresEntity();
                                    measure.TypeName = types[k].Replace("措施", "");
                                    measure.RiskId = risk.Id;
                                    measure.Content = str;
                                    measure.AreaId = areaId;
                                    measureBLL.Save("", measure);
                                }
                                k++;
                            }
                        }
                        else
                        {
                            sb.AppendFormat("管理风险库-行:{0}<br />", j);
                            error++;
                        }

                    }
                    if (dt.Rows.Count > 1)
                    {
                        count += dt.Rows.Count - 1;
                    }
                    #endregion

                    #region 职业病危害库

                    cells = wb.Worksheets[4].Cells;
                    dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn, false);
                    for (int j = 1; j < dt.Rows.Count; j++)
                    {
                        areaId = "";
                        areaCode = "";
                        //区域名称
                        string areaName = dt.Rows[j][3].ToString();
                        //管控部门
                        string deptName = dt.Rows[j][19].ToString().Trim();
                        if (string.IsNullOrEmpty(deptName))
                        {
                            sb.AppendFormat("管控部门不能为空！职业病危害库-行:{0}<br />", j);
                            error++;
                            continue;
                        }
                        try
                        {
                            //获取区域信息并判断该区域是否存在
                            if (!string.IsNullOrEmpty(areaName))
                            {
                                dis = districtBLL.GetDistrict(user.OrganizeId, areaName);
                                if (dis != null)
                                {
                                    areaId = dis.DistrictID;
                                    areaCode = dis.DistrictCode;
                                }
                                else
                                {
                                    sb.AppendFormat("区域与电厂区域不匹配！职业病危害库-行:{0}<br />", j);
                                    error++;
                                }
                            }
                            //岗位
                            string postName = dt.Rows[j][20].ToString().Trim();

                            //管控层级
                            string level = dt.Rows[j][18].ToString().Trim();
                            //风险点
                            string majorname = dt.Rows[j][1].ToString().Trim();
                            //职业病危害因素
                            string dangerSource = dt.Rows[j][4].ToString().Trim();
                            //作业分级
                            string workType = dt.Rows[j][5].ToString().Trim();
                            //导致的职业病或健康损伤
                            string illType = dt.Rows[j][6].ToString().Trim();
                            //风险分类
                            string riskType = "职业病危害";
                            //工程技术措施
                            string gcjscs = dt.Rows[j][13].ToString().Trim();
                            //管理措施
                            string glcs = dt.Rows[j][14].ToString().Trim();
                            //个人防护措施
                            string grfhcs = dt.Rows[j][15].ToString().Trim();
                            //培训教育措施
                            string pxjycs = dt.Rows[j][16].ToString().Trim();
                            //应急处置措施
                            string yjczcs = dt.Rows[j][17].ToString().Trim();
                            List<string> values = new List<string> { gcjscs, glcs, grfhcs, pxjycs, yjczcs };
                            string measures = "";
                            foreach (string str in values)
                            {
                                if (!string.IsNullOrEmpty(str))
                                {
                                    measures += str + "\r\n";
                                }
                            }
                            //评价方式
                            string way = dt.Rows[j][7].ToString().Trim();

                            string itemA = string.Empty;
                            string itemB = string.Empty;
                            string itemC = string.Empty;
                            //辨识评价可能性
                            itemA = dt.Rows[j][8].ToString().Trim();
                            if (way == "LEC")
                            {
                                //辨识评价频繁程度
                                itemB = dt.Rows[j][10].ToString().Trim();
                                //辨识评价后果
                                itemC = dt.Rows[j][9].ToString().Trim();
                            }
                            else
                            {
                                //辨识评价频繁程度
                                itemB = dt.Rows[j][9].ToString().Trim();
                                //辨识评价后果
                                itemC = dt.Rows[j][10].ToString().Trim();
                            }
                            ////辨识评价可能性
                            //string itemA = dt.Rows[j][8].ToString().Trim();
                            ////辨识评价频繁程度
                            //string itemB = dt.Rows[j][9].ToString().Trim();
                            ////辨识评价后果
                            //string itemC = dt.Rows[j][10].ToString().Trim();
                            //辨识评价风险值
                            string itemR = dt.Rows[j][11].ToString().Trim();
                            //辨识评价风险等级
                            string grade = dt.Rows[j][12].ToString().Trim();

                            RiskAssessEntity risk = new RiskAssessEntity();
                            risk.DeptCode = deptBll.GetDeptCode(deptName, user.OrganizeId);
                            risk.PostId = deptBll.GetPostId(postName, user.OrganizeId, risk.DeptCode);
                            if (string.IsNullOrEmpty(risk.DeptCode))
                            {
                                sb.AppendFormat("部门与系统的部门信息不匹配！职业病危害库-行:{0}<br />", j);
                                error++;
                                continue;
                            }
                            risk.Id = Guid.NewGuid().ToString();
                            risk.MajorName = majorname;
                            risk.Description = dangerSource;
                            risk.HarmType = workType;
                            risk.HarmProperty = illType;
                            risk.RiskType = riskType;
                            risk.PostName = postName;
                            risk.DeptName = deptName;
                            risk.AreaId = areaId;
                            risk.DistrictId = areaId;
                            risk.AreaCode = areaCode;
                            risk.AreaName = risk.DistrictName = areaName;
                            risk.Way = way;
                            risk.LevelName = level;
                            risk.Status = 1;
                            risk.Measure = measures;
                            risk.ItemA = string.IsNullOrEmpty(itemA) ? 0 : decimal.Parse(itemA);
                            risk.ItemB = string.IsNullOrEmpty(itemB) ? 0 : decimal.Parse(itemB);
                            risk.ItemC = string.IsNullOrEmpty(itemC) ? 1 : decimal.Parse(itemC);
                            SetRiskGrade(risk);
                            risk.DeptName = deptName;
                            RiskAssessBLL riskBLL = new RiskAssessBLL();

                            if (riskBLL.SaveForm("", risk) > 0)
                            {
                                int k = 0;
                                foreach (string str in values)
                                {
                                    if (!string.IsNullOrEmpty(str))
                                    {
                                        MeasuresEntity measure = new MeasuresEntity();
                                        measure.TypeName = types[k].Replace("措施", "");
                                        measure.RiskId = risk.Id;
                                        measure.Content = str;
                                        measure.AreaId = areaId;
                                        measureBLL.Save("", measure);
                                    }
                                    k++;
                                }
                            }
                            else
                            {
                                sb.AppendFormat("职业病危害库-行:{0}<br />", j);
                                error++;
                            }
                        }
                        catch (Exception ex)
                        {
                            sb.AppendFormat("职业病危害库-行:{0}({1})<br />", j, ex.Message);
                            error++;
                        }

                    }
                    if (dt.Rows.Count > 1)
                    {
                        count += dt.Rows.Count - 1;
                    }
                    #endregion
                    #region 岗位风险

                    cells = wb.Worksheets[5].Cells;
                    dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn, false);
                    for (int j = 1; j < dt.Rows.Count; j++)
                    {
                        areaId = "";
                        areaCode = "";
                        //区域名称
                        string areaName = dt.Rows[j][2].ToString();
                        //管控部门
                        string deptName = dt.Rows[j][20].ToString().Trim();
                        if (string.IsNullOrEmpty(deptName))
                        {
                            sb.AppendFormat("管控部门不能为空！岗位风险库-行:{0}<br />", j);
                            error++;
                            continue;
                        }
                        try
                        {
                            //获取区域信息并判断该区域是否存在，没有则新增该区域
                            if (!string.IsNullOrEmpty(areaName))
                            {
                                dis = districtBLL.GetDistrict(user.OrganizeId, areaName);
                                if (dis != null)
                                {
                                    areaId = dis.DistrictID;
                                    areaCode = dis.DistrictCode;
                                }
                                else
                                {
                                    sb.AppendFormat("区域与电厂区域不匹配！岗位风险库-行:{0}<br />", j);
                                    error++;
                                    // continue;
                                }
                            }
                            //岗位
                            string postName = dt.Rows[j][21].ToString().Trim();
                            //管控层级
                            string level = dt.Rows[j][19].ToString().Trim();
                            //危险源
                            string dangerSource = dt.Rows[j][5].ToString().Trim();
                            //风险类别
                            string riskType = "岗位";
                            //风险后果
                            string result = dt.Rows[j][7].ToString().Trim();
                            //工程技术措施
                            string gcjscs = dt.Rows[j][14].ToString().Trim();
                            //管理措施
                            string glcs = dt.Rows[j][15].ToString().Trim();
                            //个人防护措施
                            string grfhcs = dt.Rows[j][16].ToString().Trim();
                            //培训教育措施
                            string pxjycs = dt.Rows[j][17].ToString().Trim();
                            //应急处置措施
                            string yjczcs = dt.Rows[j][18].ToString().Trim();
                            List<string> values = new List<string> { gcjscs, glcs, grfhcs, pxjycs, yjczcs };
                            string measures = "";
                            foreach (string str in values)
                            {
                                if (!string.IsNullOrEmpty(str))
                                {
                                    measures += str + "\r\n";
                                }
                            }
                            //评价方式
                            string way = dt.Rows[j][8].ToString().Trim();
                            string itemA = string.Empty;
                            string itemB = string.Empty;
                            string itemC = string.Empty;
                            //辨识评价可能性
                            itemA = dt.Rows[j][9].ToString().Trim();
                            if (way == "LEC")
                            {
                                //辨识评价频繁程度
                                itemB = dt.Rows[j][11].ToString().Trim();
                                //辨识评价后果
                                itemC = dt.Rows[j][10].ToString().Trim();
                            }
                            else
                            {
                                //辨识评价频繁程度
                                itemB = dt.Rows[j][10].ToString().Trim();
                                //辨识评价后果
                                itemC = dt.Rows[j][11].ToString().Trim();
                            }

                            ////辨识评价可能性
                            //string itemA = dt.Rows[j][9].ToString().Trim();
                            ////辨识评价频繁程度
                            //string itemB = dt.Rows[j][10].ToString().Trim();
                            ////辨识评价后果
                            //string itemC = dt.Rows[j][11].ToString().Trim();
                            //辨识评价风险值
                            string itemR = dt.Rows[j][12].ToString().Trim();
                            //辨识评价风险等级
                            string grade = dt.Rows[j][13].ToString().Trim();
                            //风险描述
                            string desc = dt.Rows[j][6].ToString().Trim();
                            //岗位名称--对应岗位风险
                            string jobname = dt.Rows[j][3].ToString().Trim();
                            //危险源类别
                            string dangersourcetype = dt.Rows[j][4].ToString().Trim();
                            RiskAssessEntity risk = new RiskAssessEntity();
                            risk.DeptCode = deptBll.GetDeptCode(deptName, user.OrganizeId);
                            risk.PostId = deptBll.GetPostId(postName, user.OrganizeId, risk.DeptCode);
                            if (string.IsNullOrEmpty(risk.DeptCode))
                            {
                                sb.AppendFormat("部门与系统的部门信息不匹配！岗位风险库-行:{0}<br />", j);
                                error++;
                                continue;
                            }
                            risk.Id = Guid.NewGuid().ToString();
                            risk.RiskType = riskType;
                            risk.RiskDesc = desc;
                            risk.PostName = postName;
                            risk.DeptName = deptName;
                            risk.Result = result;
                            risk.AreaId = areaId;
                            risk.LevelName = level;
                            risk.DistrictId = areaId;
                            risk.AreaCode = areaCode;
                            risk.AreaName = risk.DistrictName = areaName;
                            risk.Way = way;
                            risk.Status = 1;
                            risk.JobName = jobname;
                            risk.DangerSourceType = dangersourcetype;
                            risk.Measure = measures;
                            risk.DangerSource = dangerSource;
                            risk.ItemA = string.IsNullOrEmpty(itemA) ? 0 : decimal.Parse(itemA);
                            risk.ItemB = string.IsNullOrEmpty(itemB) ? 0 : decimal.Parse(itemB);
                            risk.ItemC = string.IsNullOrEmpty(itemC) ? 0 : decimal.Parse(itemC);
                            SetRiskGrade(risk);
                            risk.DeptName = deptName;
                            RiskAssessBLL riskBLL = new RiskAssessBLL();

                            if (riskBLL.SaveForm("", risk) > 0)
                            {
                                int k = 0;
                                foreach (string str in values)
                                {
                                    if (!string.IsNullOrEmpty(str))
                                    {
                                        MeasuresEntity measure = new MeasuresEntity();
                                        measure.TypeName = types[k].Replace("措施", "");
                                        measure.RiskId = risk.Id;
                                        measure.Content = str;
                                        measure.AreaId = areaId;
                                        measureBLL.Save("", measure);
                                    }
                                    k++;
                                }
                            }
                            else
                            {
                                sb.AppendFormat("岗位风险-行:{0}<br />", j);
                                error++;
                            }
                        }
                        catch (Exception ex)
                        {
                            sb.AppendFormat("岗位风险-行:{0}({1})<br />", j, ex.Message);
                            error++;
                        }

                    }
                    if (dt.Rows.Count > 1)
                    {
                        count += dt.Rows.Count - 1;
                    }
                    #endregion
                    #region 工器具及危化品风险

                    cells = wb.Worksheets[6].Cells;
                    dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn, false);
                    for (int j = 1; j < dt.Rows.Count; j++)
                    {
                        areaId = "";
                        areaCode = "";
                        //区域名称
                        string areaName = dt.Rows[j][2].ToString();
                        //管控部门
                        string deptName = dt.Rows[j][20].ToString().Trim();
                        if (string.IsNullOrEmpty(deptName))
                        {
                            sb.AppendFormat("管控部门不能为空！工器具及危化品风险库-行:{0}<br />", j);
                            error++;
                            continue;
                        }
                        try
                        {
                            //获取区域信息并判断该区域是否存在，没有则新增该区域
                            if (!string.IsNullOrEmpty(areaName))
                            {
                                dis = districtBLL.GetDistrict(user.OrganizeId, areaName);
                                if (dis != null)
                                {
                                    areaId = dis.DistrictID;
                                    areaCode = dis.DistrictCode;
                                }
                                else
                                {
                                    sb.AppendFormat("区域与电厂区域不匹配！工器具及危化品风险库-行:{0}<br />", j);
                                    error++;
                                    //continue;
                                }
                            }
                            //岗位
                            string postName = dt.Rows[j][21].ToString().Trim();

                            //管控层级
                            string level = dt.Rows[j][19].ToString().Trim();
                            //危险源
                            string dangerSource = dt.Rows[j][5].ToString().Trim();

                            //风险类别
                            string riskType = "工器具及危化品";
                            //风险后果
                            string result = dt.Rows[j][7].ToString().Trim();
                            //工程技术措施
                            string gcjscs = dt.Rows[j][14].ToString().Trim();
                            //管理措施
                            string glcs = dt.Rows[j][15].ToString().Trim();
                            //个人防护措施
                            string grfhcs = dt.Rows[j][16].ToString().Trim();
                            //培训教育措施
                            string pxjycs = dt.Rows[j][17].ToString().Trim();
                            //应急处置措施
                            string yjczcs = dt.Rows[j][18].ToString().Trim();
                            List<string> values = new List<string> { gcjscs, glcs, grfhcs, pxjycs, yjczcs };
                            string measures = "";
                            foreach (string str in values)
                            {
                                if (!string.IsNullOrEmpty(str))
                                {
                                    measures += str + "\r\n";
                                }
                            }
                            //评价方式
                            string way = dt.Rows[j][8].ToString().Trim();

                            string itemA = string.Empty;
                            string itemB = string.Empty;
                            string itemC = string.Empty;
                            //辨识评价可能性
                            itemA = dt.Rows[j][9].ToString().Trim();
                            if (way == "LEC")
                            {
                                //辨识评价频繁程度
                                itemB = dt.Rows[j][11].ToString().Trim();
                                //辨识评价后果
                                itemC = dt.Rows[j][10].ToString().Trim();
                            }
                            else
                            {
                                //辨识评价频繁程度
                                itemB = dt.Rows[j][10].ToString().Trim();
                                //辨识评价后果
                                itemC = dt.Rows[j][11].ToString().Trim();
                            }
                            ////辨识评价可能性
                            //string itemA = dt.Rows[j][9].ToString().Trim();
                            ////辨识评价频繁程度
                            //string itemB = dt.Rows[j][10].ToString().Trim();
                            ////辨识评价后果
                            //string itemC = dt.Rows[j][11].ToString().Trim();
                            //辨识评价风险值
                            string itemR = dt.Rows[j][12].ToString().Trim();
                            //辨识评价风险等级
                            string grade = dt.Rows[j][13].ToString().Trim();
                            //风险描述
                            string desc = dt.Rows[j][6].ToString().Trim();
                            //工器具及危化品--对应工器具及危化品风险
                            string toolordanger = dt.Rows[j][3].ToString().Trim();
                            //危险源类别
                            string dangersourcetype = dt.Rows[j][4].ToString().Trim();
                            RiskAssessEntity risk = new RiskAssessEntity();
                            risk.DeptCode = deptBll.GetDeptCode(deptName, user.OrganizeId);
                            risk.PostId = deptBll.GetPostId(postName, user.OrganizeId, risk.DeptCode);
                            if (string.IsNullOrEmpty(risk.DeptCode))
                            {
                                sb.AppendFormat("部门与系统的部门信息不匹配！工器具及危化品风险库-行:{0}<br />", j);
                                error++;
                                continue;
                            }
                            risk.Id = Guid.NewGuid().ToString();
                            risk.RiskType = riskType;
                            risk.RiskDesc = desc;
                            risk.PostName = postName;
                            risk.DeptName = deptName;
                            risk.Result = result;
                            risk.AreaId = areaId;
                            risk.LevelName = level;
                            risk.DistrictId = areaId;
                            risk.AreaCode = areaCode;
                            risk.AreaName = risk.DistrictName = areaName;
                            risk.Way = way;
                            risk.Status = 1;
                            risk.ToolOrDanger = toolordanger;
                            risk.DangerSourceType = dangersourcetype;
                            risk.Measure = measures;
                            risk.DangerSource = dangerSource;
                            risk.ItemA = string.IsNullOrEmpty(itemA) ? 0 : decimal.Parse(itemA);
                            risk.ItemB = string.IsNullOrEmpty(itemB) ? 0 : decimal.Parse(itemB);
                            risk.ItemC = string.IsNullOrEmpty(itemC) ? 0 : decimal.Parse(itemC);
                            SetRiskGrade(risk);
                            risk.DeptName = deptName;
                            RiskAssessBLL riskBLL = new RiskAssessBLL();

                            if (riskBLL.SaveForm("", risk) > 0)
                            {
                                int k = 0;
                                foreach (string str in values)
                                {
                                    if (!string.IsNullOrEmpty(str))
                                    {
                                        MeasuresEntity measure = new MeasuresEntity();
                                        measure.TypeName = types[k].Replace("措施", "");
                                        measure.RiskId = risk.Id;
                                        measure.Content = str;
                                        measure.AreaId = areaId;
                                        measureBLL.Save("", measure);
                                    }
                                    k++;
                                }
                            }
                            else
                            {
                                sb.AppendFormat("工器具及危化品风险-行:{0}<br />", j);
                                error++;
                            }
                        }
                        catch (Exception ex)
                        {
                            sb.AppendFormat("工器具及危化品风险-行:{0}({1})<br />", j, ex.Message);
                            error++;
                        }

                    }
                    if (dt.Rows.Count > 1)
                    {
                        count += dt.Rows.Count - 1;
                    }
                    #endregion

                    message = "共有" + count + "条记录,成功导入" + (count - error) + "条，失败" + error + "条。";
                    if (error > 0)
                    {
                        message += sb.ToString();
                    }
                }

                return message;
            }
            catch (Exception)
            {
                return message;
            }
        }
        /// <summary>
        /// 广西华昇版本导入
        /// </summary>
        /// <returns></returns>
        private string ImportGxhsRiskData()
        {
            DataItemDetailBLL detailbll = new DataItemDetailBLL();
            BaseListingBLL baselistingbll = new BaseListingBLL();
            string message = "请选择格式正确的文件再导入!";
            try
            {
                int error = 0;
                StringBuilder sb = new StringBuilder("具体错误位置：<br />");
                int count = HttpContext.Request.Files.Count;
                if (count > 0)
                {
                    count = 0;
                    HttpPostedFileBase file = HttpContext.Request.Files[0];
                    if (string.IsNullOrEmpty(file.FileName))
                    {
                        return message;
                    }
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName));
                    Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                    wb.Open(Server.MapPath("~/Resource/temp/" + fileName), Aspose.Cells.FileFormatType.Excel2003);

                    Aspose.Cells.Cells cells = wb.Worksheets[0].Cells;


                    Operator currUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
                    #region 作业活动类
                    DepartmentBLL deptBll = new DepartmentBLL();
                    DataTable dt = cells.ExportDataTable(2, 0, cells.MaxDataRow - 1, cells.MaxColumn + 1, false);
                    MeasuresBLL measureBLL = new MeasuresBLL();

                    DistrictEntity dis = null;
                    string areaId = "";
                    string areaCode = "";
                    //导入作业类型风险清单时,导入的数据同步到预知训练库
                    DataTable TrainDt = dt.Clone();
                    for (int j = 1; j < dt.Rows.Count; j++)
                    {
                        areaId = "";
                        areaCode = "";
                        //区域名称
                        string areaName = dt.Rows[j][3].ToString();
                        try
                        {
                            //获取区域信息并判断该区域是否存在，没有则新增该区域
                            if (!string.IsNullOrEmpty(areaName))
                            {
                                dis = districtBLL.GetDistrict(currUser.OrganizeId, areaName);
                                if (dis != null)
                                {
                                    areaId = dis.DistrictID;
                                    areaCode = dis.DistrictCode;
                                }
                                else
                                {
                                    sb.AppendFormat("区域与电厂区域不匹配！作业活动类-行:{0}<br />", j);
                                    error++;
                                    continue;
                                }
                            }
                            //作业活动
                            string name = dt.Rows[j][1].ToString().Trim();
                            //活动步骤
                            string workcontent = dt.Rows[j][2].ToString().Trim();
                            //危害名称
                            string harmname = dt.Rows[j][4].ToString().Trim();
                            //危害种类
                            string hazardtype = dt.Rows[j][5].ToString().Trim();
                            //危害及有关信息描述
                            string harmdescription = dt.Rows[j][6].ToString().Trim();
                            //风险描述
                            string riskdesc = dt.Rows[j][7].ToString().Trim();
                            //风险种类
                            string typesofrisk = dt.Rows[j][8].ToString().Trim();
                            //风险范畴
                            string riskcategory = dt.Rows[j][9].ToString().Trim();
                            //暴露于人员/设备信息
                            string exposedrisk = dt.Rows[j][10].ToString().Trim();
                            //现有的控制措施
                            string existingmeasures = dt.Rows[j][11].ToString().Trim();
                            //后果
                            string ItemB = dt.Rows[j][12].ToString().Trim();
                            //暴露
                            string ItemC = dt.Rows[j][13].ToString().Trim();
                            //可能性
                            string ItemA = dt.Rows[j][14].ToString().Trim();
                            //辨识评价风险值
                            string itemR = dt.Rows[j][15].ToString().Trim();
                            //辨识评价风险等级
                            string grade = dt.Rows[j][16].ToString().Trim();
                            //建议采取的控制措施
                            string advicemeasures = dt.Rows[j][17].ToString().Trim();
                            //控制措施的有效性
                            string effectiveness = dt.Rows[j][18].ToString().Trim();
                            //控制措施的成本因素
                            string costfactor = dt.Rows[j][19].ToString().Trim();
                            //控制措施判断后果
                            string measuresresult = dt.Rows[j][20].ToString().Trim();
                            //措施的采纳
                            string IsAdopt = dt.Rows[j][21].ToString().Trim();
                            //填报单位
                            string deptlist = dt.Rows[j][22].ToString().Trim();
                            //责任人
                            string dutyperson = dt.Rows[j][23].ToString().Trim();
                            //备注
                            string remark = dt.Rows[j][24].ToString().Trim();
                            //岗位（工种）
                            string Post = dt.Rows[j][25].ToString().Trim();
                            string PostId = "";
                            //常规/非常规
                            string IsConventional = dt.Rows[j][26].ToString().Trim();

                            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(workcontent) || string.IsNullOrWhiteSpace(typesofrisk) || string.IsNullOrWhiteSpace(riskcategory))
                            {
                                sb.AppendFormat("作业活动类-行:{0}有必填栏目没有填写<br />", j);
                                error++;
                                continue;
                            }

                            string controlDept = currUser.DeptName;//管控部门
                            string controlDeptId = currUser.DeptId;//管控部门
                            string controlDeptCode = currUser.DeptCode;//管控部门
                            var p1 = string.Empty;
                            var p2 = string.Empty;
                            bool isSkip = false;
                            //验证所填部门是否存在
                            if (!string.IsNullOrWhiteSpace(deptlist))
                            {
                                var array = deptlist.Split('/');
                                for (int i = 0; i < array.Length; i++)
                                {
                                    if (i == 0)
                                    {
                                        var entity = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "厂级" && x.FullName == array[i].ToString()).FirstOrDefault();
                                        if (entity == null)
                                        {
                                            entity = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "部门" && x.FullName == array[i].ToString()).FirstOrDefault();
                                            if (entity == null)
                                            {
                                                entity = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "承包商" && x.FullName == array[i].ToString()).FirstOrDefault();
                                                if (entity == null)
                                                {
                                                    sb.AppendFormat("</br>" + "第" + (j + 2) + "行部门信息不存在,未能导入.");
                                                    error++;
                                                    isSkip = true;
                                                    break;
                                                }
                                                else
                                                {
                                                    controlDept = entity.FullName;
                                                    controlDeptId = entity.DepartmentId;
                                                    controlDeptCode = entity.EnCode;
                                                    p1 = entity.DepartmentId;
                                                }
                                            }
                                            else
                                            {
                                                controlDept = entity.FullName;
                                                controlDeptId = entity.DepartmentId;
                                                controlDeptCode = entity.EnCode;
                                                p1 = entity.DepartmentId;
                                            }
                                        }
                                        else
                                        {
                                            controlDept = entity.FullName;
                                            controlDeptId = entity.DepartmentId;
                                            controlDeptCode = entity.EnCode;
                                            p1 = entity.DepartmentId;
                                        }
                                    }
                                    else if (i == 1)
                                    {
                                        var entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && (x.Nature == "专业" || x.Nature == "承包商" || x.Nature == "分包商") && x.FullName == array[i].ToString() && x.ParentId == p1).FirstOrDefault();
                                        if (entity1 == null)
                                        {
                                            entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "班组" && x.FullName == array[i].ToString() && x.ParentId == p1).FirstOrDefault();
                                            if (entity1 == null)
                                            {
                                                sb.AppendFormat("</br>" + "第" + (j + 2) + "行部门信息不存在,未能导入.");
                                                error++;
                                                isSkip = true;
                                                break;
                                            }
                                            else
                                            {
                                                controlDept = entity1.FullName;
                                                controlDeptId = entity1.DepartmentId;
                                                controlDeptCode = entity1.EnCode;
                                                p2 = entity1.DepartmentId;
                                            }
                                        }
                                        else
                                        {
                                            controlDept = entity1.FullName;
                                            controlDeptId = entity1.DepartmentId;
                                            controlDeptCode = entity1.EnCode;
                                            p2 = entity1.DepartmentId;
                                        }

                                    }
                                    else
                                    {
                                        var entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && (x.Nature == "班组" || x.Nature == "承包商" || x.Nature == "分包商") && x.FullName == array[i].ToString() && x.ParentId == p2).FirstOrDefault();
                                        if (entity1 == null)
                                        {
                                            sb.AppendFormat("</br>" + "第" + (j + 2) + "行部门信息不存在,未能导入.");
                                            error++;
                                            isSkip = true;
                                            break;
                                        }
                                        else
                                        {
                                            controlDept = entity1.FullName;
                                            controlDeptId = entity1.DepartmentId;
                                            controlDeptCode = entity1.EnCode;
                                        }
                                    }
                                }
                            }
                            if (isSkip)
                            {
                                continue;
                            }
                            //验证岗位是不是在部门里面
                            if (!string.IsNullOrWhiteSpace(Post))
                            {
                                var controldept = departmentBLL.GetEntity(controlDeptId);
                                RoleEntity re = new RoleEntity();
                                if (controldept.Nature == "厂级")
                                {
                                    re = postbll.GetList().Where(a => (a.FullName == Post && a.OrganizeId == currUser.OrganizeId)).FirstOrDefault();
                                }
                                else
                                {
                                    re = postbll.GetList().Where(a => (a.FullName == Post && a.OrganizeId == currUser.OrganizeId && a.DeptId == controlDeptId)).FirstOrDefault();
                                }
                                if (re == null)
                                {
                                    sb.AppendFormat("作业活动类-行:{0}岗位有误,未能导入.<br />", j);
                                    error++;
                                    continue;
                                }
                                else
                                {
                                    PostId = re.RoleId;
                                }
                            }
                            var baselisting = baselistingbll.GetList(t => t.Name == name && t.ActivityStep == workcontent && t.Type == 0 && t.PostId == PostId).FirstOrDefault();
                            if (baselisting == null)
                            {
                                BaseListingEntity baselistingEntity = new BaseListingEntity();
                                baselistingEntity.Name = name;
                                baselistingEntity.ActivityStep = workcontent;
                                baselistingEntity.IsConventional = IsConventional == "常规" ? 0 : 1;
                                baselistingEntity.Others = "";
                                baselistingEntity.CreateUserDeptCode = string.IsNullOrWhiteSpace(deptlist) ? currUser.DeptCode : controlDeptCode;
                                baselistingEntity.ControlsDept = controlDept;
                                baselistingEntity.ControlsDeptId = controlDeptId;
                                baselistingEntity.ControlsDeptCode = controlDeptCode;
                                baselistingEntity.Type = 0;
                                baselistingEntity.Post = Post;
                                baselistingEntity.PostId = PostId;
                                Expression<Func<BaseListingEntity, bool>> condition = t => t.Name == name && !(t.AreaName == null || t.AreaName.Trim() == "");
                                var defualt = baselistingbll.GetList(condition).ToList().FirstOrDefault();
                                baselistingEntity.AreaName = defualt == null ? "" : defualt.AreaName;
                                baselistingEntity.AreaId = defualt == null ? "" : defualt.AreaId;
                                baselistingEntity.AreaCode = defualt == null ? "" : defualt.AreaCode;
                                baselistingbll.SaveForm("", baselistingEntity);
                                baselisting = baselistingEntity;
                            }
                            RiskAssessEntity risk = new RiskAssessEntity();

                            risk.Id = Guid.NewGuid().ToString();
                            risk.Name = name;
                            risk.WorkContent = workcontent;
                            risk.HarmName = harmname;
                            string HazardTypeValue = "";
                            foreach (var item in hazardtype.Split(','))
                            {
                                HazardTypeValue += detailbll.GetItemValue(item, "HazardType") + ",";
                            }
                            risk.HazardType = HazardTypeValue.Length > 0 ? HazardTypeValue.Substring(0, HazardTypeValue.Length - 1) : "";
                            risk.HarmDescription = harmdescription;
                            risk.RiskDesc = riskdesc;
                            risk.TypesOfRisk = typesofrisk;
                            risk.RiskCategory = riskcategory;
                            risk.ExposedRisk = exposedrisk;
                            risk.ExistingMeasures = existingmeasures;
                            risk.ItemA = string.IsNullOrEmpty(ItemA) ? 0 : decimal.Parse(ItemA);
                            risk.ItemB = string.IsNullOrEmpty(ItemB) ? 0 : decimal.Parse(ItemB);
                            risk.ItemC = string.IsNullOrEmpty(ItemC) ? 0 : decimal.Parse(ItemC);
                            risk.Status = 1;
                            risk.Way = "PSE";
                            risk.RiskType = "作业活动";
                            SetRiskGrade(risk);
                            risk.AdviceMeasures = advicemeasures;
                            risk.Effectiveness = string.IsNullOrWhiteSpace(effectiveness) ? 0 : decimal.Parse(effectiveness);
                            risk.CostFactor = string.IsNullOrWhiteSpace(costfactor) ? 0 : decimal.Parse(costfactor);
                            risk.MeasuresResultVal = risk.Effectiveness * risk.CostFactor;
                            if (!string.IsNullOrWhiteSpace(effectiveness) && !string.IsNullOrWhiteSpace(costfactor))
                            {
                                if (risk.MeasuresResultVal >= 10)
                                {
                                    risk.MeasuresResult = "预期的控制措施的费用支出恰当；";
                                }
                                else
                                {
                                    risk.MeasuresResult = "预期的控制措施的费用支出不恰当；";
                                }
                            }
                            risk.DistrictId = risk.AreaId = areaId;
                            risk.AreaName = risk.DistrictName = areaName;
                            risk.AreaCode = areaCode;
                            risk.DeptCode = risk.CreateUserDeptCode = string.IsNullOrWhiteSpace(deptlist) ? currUser.DeptCode : controlDeptCode;
                            risk.DeptName = string.IsNullOrWhiteSpace(deptlist) ? currUser.DeptName : controlDept;
                            risk.ListingId = baselisting.Id;
                            risk.IsConventional = baselisting.IsConventional;
                            risk.DutyPerson = dutyperson;
                            risk.Remark = remark;
                            if (!string.IsNullOrWhiteSpace(IsAdopt))
                            {
                                risk.IsAdopt = IsAdopt == "采纳" ? 0 : 1;
                            }
                            RiskAssessBLL riskBLL = new RiskAssessBLL();

                            if (riskBLL.SaveForm("", risk) > 0)
                            {
                                int k = 0;
                                DataRow row = TrainDt.NewRow();
                                row.ItemArray = dt.Rows[j].ItemArray;
                                TrainDt.Rows.Add(row);
                            }
                            else
                            {
                                sb.AppendFormat("作业活动类-行:{0}<br />", j);
                                error++;
                            }
                        }
                        catch (Exception ex)
                        {
                            sb.AppendFormat("作业活动类-行:{0}({1})<br />", j, ex.Message);
                            error++;
                        }
                    }
                    Dictionary<string, DataRow[]> dic = new Dictionary<string, DataRow[]>();
                    //开始同步数据 TrainDt为已经导入风险清单的数据
                    for (int i = 0; i < TrainDt.Rows.Count; i++)
                    {
                        if (dic.Keys.Contains(TrainDt.Rows[i][4].ToString() + TrainDt.Rows[i][2].ToString()))
                        {
                            continue;
                        }
                        else
                        {
                            dic.Add(TrainDt.Rows[i][4].ToString() + TrainDt.Rows[i][2].ToString(), TrainDt.Select(string.Format("Column5='{0}' and Column3='{1}'", TrainDt.Rows[i][4].ToString(), TrainDt.Rows[i][2].ToString())));
                        }
                    }

                    List<RisktrainlibEntity> RiskLib = new List<RisktrainlibEntity>();
                    List<RisktrainlibdetailEntity> detailLib = new List<RisktrainlibdetailEntity>();
                    foreach (string key in dic.Keys)
                    {
                        DataRow[] item = dic[key];
                        var entity = new RisktrainlibEntity();
                        entity.Create();
                        entity.WorkTask = item[0][4].ToString();
                        int level = 4;
                        var data = new DataItemDetailBLL().GetDataItemListByItemCode("'StatisticsType'");
                        foreach (DataRow r in item)
                        {
                            entity.RiskLevel = r[14].ToString();
                            switch (entity.RiskLevel)
                            {
                                case "重大风险":
                                    entity.RiskLevelVal = "1";
                                    break;
                                case "较大风险":
                                    entity.RiskLevelVal = "2";
                                    break;
                                case "一般风险":
                                    entity.RiskLevelVal = "3";
                                    break;
                                case "低风险":
                                    entity.RiskLevelVal = "4";
                                    break;
                                default:
                                    break;
                            }
                            if (Convert.ToInt32(entity.RiskLevelVal) < level)
                            {
                                level = Convert.ToInt32(entity.RiskLevelVal);
                            }

                            if (!string.IsNullOrWhiteSpace(r[3].ToString()))
                            {
                                var list = r[3].ToString().Split(',');
                                for (int k = 0; k < list.Length; k++)
                                {
                                    var e = data.Where(x => x.ItemName == list[k].Trim()).FirstOrDefault();
                                    if (e != null)
                                    {
                                        if (string.IsNullOrWhiteSpace(entity.WorkType))
                                        {
                                            entity.WorkTypeCode += e.ItemValue + ",";
                                            entity.WorkType += e.ItemName + ",";
                                        }
                                        else
                                        {
                                            if (!entity.WorkType.Contains(e.ItemName))
                                            {
                                                entity.WorkTypeCode += e.ItemValue + ",";
                                                entity.WorkType += e.ItemName + ",";
                                            }
                                        }

                                    }
                                }
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(entity.WorkTypeCode))
                        {
                            entity.WorkTypeCode = entity.WorkTypeCode.Substring(0, entity.WorkTypeCode.Length - 1);
                            entity.WorkType = entity.WorkType.Substring(0, entity.WorkType.Length - 1);
                        }
                        entity.RiskLevelVal = level.ToString();
                        switch (entity.RiskLevelVal)
                        {
                            case "1":
                                entity.RiskLevel = "重大风险";
                                break;
                            case "2":
                                entity.RiskLevel = "较大风险";
                                break;
                            case "3":
                                entity.RiskLevel = "一般风险";
                                break;
                            case "4":
                                entity.RiskLevel = "低风险";
                                break;
                            default:
                                break;
                        }
                        if (string.IsNullOrWhiteSpace(item[0][2].ToString()))
                        {
                            //falseMessage += "</br>" + "第" + (i+1) + "行区域为空,未能导入.";
                            //error++;
                            //continue;
                        }
                        else
                        {
                            entity.WorkArea = item[0][2].ToString();
                            DistrictEntity disEntity = districtBLL.GetDistrict(currUser.OrganizeId, item[0][2].ToString());
                            if (disEntity == null)
                            {
                                //电厂没有该区域则不赋值
                                entity.WorkArea = "";
                            }
                            else
                            {
                                entity.WorkAreaId = disEntity.DistrictID;
                            }
                        }
                        entity.WorkPost = item[0][22].ToString();
                        entity.DataSources = "1";
                        RiskLib.Add(entity);
                        foreach (DataRow it in item)
                        {
                            var dentity = new RisktrainlibdetailEntity();
                            dentity.Process = it[5].ToString();
                            dentity.AtRisk = it[6].ToString() + it[7].ToString();
                            //工程技术措施
                            string gcjscs = it[15].ToString().Trim();
                            //管理措施
                            string glcs = it[16].ToString().Trim();
                            //个人防护措施
                            string grfhcs = it[17].ToString().Trim();
                            //培训教育措施
                            string pxjycs = it[18].ToString().Trim();
                            //应急处置措施
                            string yjczcs = it[19].ToString().Trim();
                            List<string> values = new List<string> { gcjscs, glcs, grfhcs, pxjycs, yjczcs };
                            string measures = "";
                            foreach (string str in values)
                            {
                                if (!string.IsNullOrEmpty(str))
                                {
                                    measures += str + "\r\n";
                                }
                            }
                            dentity.Controls = measures;
                            dentity.Create();
                            dentity.WorkId = entity.ID;
                            detailLib.Add(dentity);
                        }
                    }
                    new RisktrainlibBLL().InsertImportData(RiskLib, detailLib);
                    if (dt.Rows.Count > 1)
                    {
                        count += dt.Rows.Count - 1;
                    }
                    #endregion

                    #region 设备风险库

                    cells = wb.Worksheets[1].Cells;
                    dt = cells.ExportDataTable(2, 0, cells.MaxDataRow - 1, cells.MaxColumn + 1, false);
                    for (int j = 1; j < dt.Rows.Count; j++)
                    {
                        areaId = "";
                        areaCode = "";
                        //区域名称
                        string areaName = dt.Rows[j][2].ToString();
                        try
                        {
                            //获取区域信息并判断该区域是否存在，没有则新增该区域
                            if (!string.IsNullOrEmpty(areaName))
                            {
                                dis = districtBLL.GetDistrict(currUser.OrganizeId, areaName);
                                if (dis != null)
                                {
                                    areaId = dis.DistrictID;
                                    areaCode = dis.DistrictCode;
                                }
                                else
                                {
                                    sb.AppendFormat("区域与电厂区域不匹配！设备设施类-行:{0}<br />", j);
                                    error++;
                                    continue;
                                }
                            }
                            else
                            {
                                sb.AppendFormat("所属区域不能为空！设备设施类-行:{0}<br />", j);
                                error++;
                                continue;
                            }
                            //设备名称
                            string name = dt.Rows[j][1].ToString().Trim();
                            //是否特种设备
                            string isspecialequ = dt.Rows[j][3].ToString().Trim();
                            //检查项目名称
                            string checkprojectname = dt.Rows[j][4].ToString().Trim();
                            //检查标准
                            string checkstandard = dt.Rows[j][5].ToString().Trim();
                            //风险描述
                            string riskdesc = dt.Rows[j][6].ToString().Trim();
                            //风险种类
                            string typesofrisk = dt.Rows[j][7].ToString().Trim();
                            //风险范畴
                            string riskcategory = dt.Rows[j][8].ToString().Trim();
                            //不符合标准情况及后果
                            string consequences = dt.Rows[j][9].ToString().Trim();
                            //现有的控制措施
                            string existingmeasures = dt.Rows[j][10].ToString().Trim();
                            //后果
                            string ItemB = dt.Rows[j][11].ToString().Trim();
                            //暴露
                            string ItemC = dt.Rows[j][12].ToString().Trim();
                            //可能性
                            string ItemA = dt.Rows[j][13].ToString().Trim();
                            //辨识评价风险值
                            string itemR = dt.Rows[j][14].ToString().Trim();
                            //辨识评价风险等级
                            string grade = dt.Rows[j][15].ToString().Trim();
                            //建议采取的控制措施
                            string advicemeasures = dt.Rows[j][16].ToString().Trim();
                            //控制措施的有效性
                            string effectiveness = dt.Rows[j][17].ToString().Trim();
                            //控制措施的成本因素
                            string costfactor = dt.Rows[j][18].ToString().Trim();
                            //控制措施判断后果
                            string measuresresult = dt.Rows[j][19].ToString().Trim();
                            //措施的采纳
                            string IsAdopt = dt.Rows[j][20].ToString().Trim();
                            //填报单位
                            string deptlist = dt.Rows[j][21].ToString().Trim();
                            //责任人
                            string dutyperson = dt.Rows[j][22].ToString().Trim();
                            //备注
                            string remark = dt.Rows[j][23].ToString().Trim();


                            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(areaName) || string.IsNullOrWhiteSpace(typesofrisk) || string.IsNullOrWhiteSpace(riskcategory))
                            {
                                sb.AppendFormat("设备设施类-行:{0}有必填栏目没有填写<br />", j);
                                error++;
                                continue;
                            }

                            string controlDept = string.Empty;//管控部门
                            string controlDeptId = string.Empty;//管控部门
                            string controlDeptCode = string.Empty;//管控部门
                            var p1 = string.Empty;
                            var p2 = string.Empty;
                            bool isSkip = false;
                            //验证所填部门是否存在
                            if (!string.IsNullOrWhiteSpace(deptlist))
                            {
                                var array = deptlist.Split('/');
                                for (int i = 0; i < array.Length; i++)
                                {
                                    if (i == 0)
                                    {
                                        var entity = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "厂级" && x.FullName == array[i].ToString()).FirstOrDefault();
                                        if (entity == null)
                                        {
                                            entity = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "部门" && x.FullName == array[i].ToString()).FirstOrDefault();
                                            if (entity == null)
                                            {
                                                entity = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "承包商" && x.FullName == array[i].ToString()).FirstOrDefault();
                                                if (entity == null)
                                                {
                                                    sb.AppendFormat("</br>" + "第" + (j + 2) + "行部门信息不存在,未能导入.");
                                                    error++;
                                                    isSkip = true;
                                                    break;
                                                }
                                                else
                                                {
                                                    controlDept = entity.FullName;
                                                    controlDeptId = entity.DepartmentId;
                                                    controlDeptCode = entity.EnCode;
                                                    p1 = entity.DepartmentId;
                                                }
                                            }
                                            else
                                            {
                                                controlDept = entity.FullName;
                                                controlDeptId = entity.DepartmentId;
                                                controlDeptCode = entity.EnCode;
                                                p1 = entity.DepartmentId;
                                            }
                                        }
                                        else
                                        {
                                            controlDept = entity.FullName;
                                            controlDeptId = entity.DepartmentId;
                                            controlDeptCode = entity.EnCode;
                                            p1 = entity.DepartmentId;
                                        }
                                    }
                                    else if (i == 1)
                                    {
                                        var entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && (x.Nature == "专业" || x.Nature == "承包商" || x.Nature == "分包商") && x.FullName == array[i].ToString() && x.ParentId == p1).FirstOrDefault();
                                        if (entity1 == null)
                                        {
                                            entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "班组" && x.FullName == array[i].ToString() && x.ParentId == p1).FirstOrDefault();
                                            if (entity1 == null)
                                            {
                                                sb.AppendFormat("</br>" + "第" + (j + 2) + "行部门信息不存在,未能导入.");
                                                error++;
                                                isSkip = true;
                                                break;
                                            }
                                            else
                                            {
                                                controlDept = entity1.FullName;
                                                controlDeptId = entity1.DepartmentId;
                                                controlDeptCode = entity1.EnCode;
                                                p2 = entity1.DepartmentId;
                                            }
                                        }
                                        else
                                        {
                                            controlDept = entity1.FullName;
                                            controlDeptId = entity1.DepartmentId;
                                            controlDeptCode = entity1.EnCode;
                                            p2 = entity1.DepartmentId;
                                        }

                                    }
                                    else
                                    {
                                        var entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && (x.Nature == "班组" || x.Nature == "承包商" || x.Nature == "分包商") && x.FullName == array[i].ToString() && x.ParentId == p2).FirstOrDefault();
                                        if (entity1 == null)
                                        {
                                            sb.AppendFormat("</br>" + "第" + (j + 2) + "行部门信息不存在,未能导入.");
                                            error++;
                                            isSkip = true;
                                            break;
                                        }
                                        else
                                        {
                                            controlDept = entity1.FullName;
                                            controlDeptId = entity1.DepartmentId;
                                            controlDeptCode = entity1.EnCode;
                                        }
                                    }
                                }
                            }
                            if (isSkip)
                            {
                                continue;
                            }
                            var baselisting = baselistingbll.GetList(t => t.Name == name && t.AreaName == areaName).FirstOrDefault();
                            if (baselisting == null)
                            {
                                BaseListingEntity Listingentity = new BaseListingEntity();
                                Listingentity.Name = name;
                                Listingentity.AreaCode = areaCode;
                                Listingentity.AreaId = areaId;
                                Listingentity.AreaName = areaName;
                                Listingentity.IsSpecialEqu = isspecialequ == "是" ? 0 : 1;
                                Listingentity.Others = "";
                                Listingentity.CreateUserDeptCode = string.IsNullOrWhiteSpace(deptlist) ? currUser.DeptCode : controlDeptCode;
                                Listingentity.ControlsDept = controlDept;
                                Listingentity.ControlsDeptId = controlDeptId;
                                Listingentity.ControlsDeptCode = controlDeptCode;
                                Listingentity.Type = 1;
                                baselistingbll.SaveForm("", Listingentity);
                                baselisting = Listingentity;
                            }
                            RiskAssessEntity risk = new RiskAssessEntity();

                            risk.Id = Guid.NewGuid().ToString();
                            risk.Name = name;
                            risk.CheckProjectName = checkprojectname;
                            risk.CheckStandard = checkstandard;
                            risk.RiskDesc = riskdesc;
                            risk.TypesOfRisk = typesofrisk;
                            risk.RiskCategory = riskcategory;
                            risk.Consequences = consequences;
                            risk.ExistingMeasures = existingmeasures;
                            risk.ItemA = string.IsNullOrEmpty(ItemA) ? 0 : decimal.Parse(ItemA);
                            risk.ItemB = string.IsNullOrEmpty(ItemB) ? 0 : decimal.Parse(ItemB);
                            risk.ItemC = string.IsNullOrEmpty(ItemC) ? 0 : decimal.Parse(ItemC);
                            risk.Status = 1;
                            risk.Way = "PSE";
                            risk.RiskType = "设备设施";
                            SetRiskGrade(risk);
                            risk.AdviceMeasures = advicemeasures;
                            risk.Effectiveness = string.IsNullOrWhiteSpace(effectiveness) ? 0 : decimal.Parse(effectiveness);
                            risk.CostFactor = string.IsNullOrWhiteSpace(costfactor) ? 0 : decimal.Parse(costfactor);
                            risk.MeasuresResultVal = risk.Effectiveness * risk.CostFactor;
                            if (!string.IsNullOrWhiteSpace(effectiveness) && !string.IsNullOrWhiteSpace(costfactor))
                            {
                                if (risk.MeasuresResultVal >= 10)
                                {
                                    risk.MeasuresResult = "预期的控制措施的费用支出恰当；";
                                }
                                else
                                {
                                    risk.MeasuresResult = "预期的控制措施的费用支出不恰当；";
                                }
                            }
                            risk.DistrictId = risk.AreaId = areaId;
                            risk.AreaName = risk.DistrictName = areaName;
                            risk.AreaCode = areaCode;
                            risk.DeptCode = risk.CreateUserDeptCode = string.IsNullOrWhiteSpace(deptlist) ? currUser.DeptCode : controlDeptCode;
                            risk.DeptName = string.IsNullOrWhiteSpace(deptlist) ? currUser.DeptName : controlDept;
                            risk.ListingId = baselisting.Id;
                            risk.IsConventional = baselisting.IsConventional;
                            risk.IsSpecialEqu = baselisting.IsSpecialEqu;
                            if (!string.IsNullOrWhiteSpace(IsAdopt))
                            {
                                risk.IsAdopt = IsAdopt == "采纳" ? 0 : 1;
                            }
                            RiskAssessBLL riskBLL = new RiskAssessBLL();

                            if (riskBLL.SaveForm("", risk) > 0)
                            {
                                int k = 0;
                                DataRow row = TrainDt.NewRow();
                                row.ItemArray = dt.Rows[j].ItemArray;
                                TrainDt.Rows.Add(row);
                            }
                            else
                            {
                                sb.AppendFormat("设备设施类-行:{0}<br />", j);
                                error++;
                            }
                        }
                        catch (Exception ex)
                        {
                            sb.AppendFormat("设备设施类-行:{0}({1})<br />", j, ex.Message);
                            error++;
                        }
                    }
                    if (dt.Rows.Count > 1)
                    {
                        count += dt.Rows.Count - 1;
                    }
                    #endregion

                    message = "共有" + count + "条记录,成功导入" + (count - error) + "条，失败" + error + "条。";
                    if (error > 0)
                    {
                        message += sb.ToString();
                    }
                }

                return message;
            }
            catch (Exception)
            {
                return message;
            }
        }
        /// <summary>
        /// 获取风险取值配置内容
        /// </summary>
        /// <param name="RiskType">风险类型</param>
        /// <param name="WayType">计算方式（LEC，PSE，风险矩阵法）</param>
        /// <returns></returns>
        private RiskwayconfigEntity GetWayConfigEntity(string RiskType, string WayType)
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var data = riskwayconfigbll.GetList("").Where(x => x.RiskType == RiskType && x.WayType == WayType && x.IsCommit == "1").ToList();
                if (data.Count > 0)
                {
                    var returnData = data.Where(x => x.DeptCode == user.OrganizeCode).ToList();
                    if (returnData.Count > 0)
                    {
                        for (int i = 0; i < returnData.Count; i++)
                        {
                            returnData[i].DetaileList = riskwayconfigdetailbll.GetList("").Where(x => x.WayConfigId == returnData[i].ID).ToList();
                        }
                        return returnData.FirstOrDefault();
                    }
                    else
                    {
                        var rData = data.Where(x => x.DeptCode == "0").ToList();
                        for (int i = 0; i < rData.Count; i++)
                        {
                            rData[i].DetaileList = riskwayconfigdetailbll.GetList("").Where(x => x.WayConfigId == rData[i].ID).ToList();
                        }
                        return rData.FirstOrDefault();
                    }
                }
                else
                {
                    return data.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 计算风险等级
        /// </summary>
        /// <param name="RiskLevel">风险等级</param>
        /// <param name="MinValue">最小值</param>
        /// <param name="MinSymbol">最小值符号</param>
        /// <param name="MaxValue">最大值</param>
        /// <param name="MaxSymbol">最大值符号</param>
        /// <param name="compareFlag">比较结果</param>
        /// <param name="result">计算的值</param>
        /// <returns></returns>
        private bool CompareRiskGrade(string RiskLevel, decimal? MinValue, string MinSymbol, decimal? MaxValue, string MaxSymbol, bool compareFlag, decimal? result)
        {
            var flag1 = false;
            var flag2 = false;
            if (MinValue > 0)
            {
                switch (MinSymbol)
                {
                    case "0":
                        if (MinValue < result)
                        {
                            flag1 = true;
                        }
                        else
                        {
                            flag1 = false;
                        }
                        break;
                    case "1":
                        if (MinValue > result)
                        {
                            flag1 = true;
                        }
                        else
                        {
                            flag1 = false;
                        }
                        break;
                    case "2":
                        if (MinValue == result)
                        {
                            flag1 = true;
                        }
                        else
                        {
                            flag1 = false;
                        }
                        break;
                    case "3":
                        if (MinValue <= result)
                        {
                            flag1 = true;
                        }
                        else
                        {
                            flag1 = false;
                        }
                        break;
                    case "4":
                        if (MinValue >= result)
                        {
                            flag1 = true;
                        }
                        else
                        {
                            flag1 = false;
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                flag1 = true;
            }
            if (MaxValue > 0)
            {
                switch (MaxSymbol)
                {
                    case "0":
                        if (result < MaxValue)
                        {
                            flag2 = true;
                        }
                        else
                        {
                            flag2 = false;
                        }
                        break;
                    case "1":
                        if (result > MaxValue)
                        {
                            flag2 = true;
                        }
                        else
                        {
                            flag2 = false;
                        }
                        break;
                    case "2":
                        if (result == MaxValue)
                        {
                            flag2 = true;
                        }
                        else
                        {
                            flag2 = false;
                        }
                        break;
                    case "3":
                        if (result <= MaxValue)
                        {
                            flag2 = true;
                        }
                        else
                        {
                            flag2 = false;
                        }
                        break;
                    case "4":
                        if (result >= MaxValue)
                        {
                            flag2 = true;
                        }
                        else
                        {
                            flag2 = false;
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                flag2 = true;
            }
            if (flag2 && flag1)
            {
                compareFlag = true;
            }
            return compareFlag;
        }
        /// <summary>
        /// 风险等级赋值
        /// </summary>
        /// <param name="risk">风险实体</param>
        /// <param name="config">配置实体</param>
        private void SetRiskGrade(RiskAssessEntity risk)
        {
            DataItemDetailBLL detailbll = new DataItemDetailBLL();
            string gxhs = detailbll.GetItemValue("广西华昇版本");
            if (risk.Way == "风险矩阵法")
            {
                risk.ItemR = risk.ItemA * risk.ItemB;
            }
            else
            {
                risk.ItemR = risk.ItemA * risk.ItemB * risk.ItemC;
            }
            RiskwayconfigEntity config = GetWayConfigEntity(risk.RiskType + "风险", risk.Way);
            if (config != null)
            {
                if (config.DetaileList.Count > 0)
                {
                    for (int i = 0; i < config.DetaileList.Count; i++)
                    {
                        var compareFlag = false;
                        var detail = config.DetaileList[i];
                        switch (detail.RiskLevel)
                        {
                            case "重大风险":
                                compareFlag = CompareRiskGrade(detail.RiskLevel, detail.MinValue, detail.MinSymbol, detail.MaxValue, detail.MaxSymbol, compareFlag, risk.ItemR);
                                if (compareFlag)
                                {
                                    risk.Grade = "重大风险";
                                    risk.GradeVal = 1;
                                }
                                break;
                            case "较大风险":
                                compareFlag = CompareRiskGrade(detail.RiskLevel, detail.MinValue, detail.MinSymbol, detail.MaxValue, detail.MaxSymbol, compareFlag, risk.ItemR);
                                if (compareFlag)
                                {
                                    risk.Grade = "较大风险";
                                    risk.GradeVal = 2;
                                }
                                break;
                            case "一般风险":
                                compareFlag = CompareRiskGrade(detail.RiskLevel, detail.MinValue, detail.MinSymbol, detail.MaxValue, detail.MaxSymbol, compareFlag, risk.ItemR);
                                if (compareFlag)
                                {
                                    risk.Grade = "一般风险";
                                    risk.GradeVal = 3;
                                }
                                break;
                            case "低风险":
                                compareFlag = CompareRiskGrade(detail.RiskLevel, detail.MinValue, detail.MinSymbol, detail.MaxValue, detail.MaxSymbol, compareFlag, risk.ItemR);
                                if (compareFlag)
                                {
                                    risk.Grade = "低风险";
                                    risk.GradeVal = 4;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    risk.Grade = "低风险";
                    risk.GradeVal = 4;
                }
            }
            else
            {
                risk.Grade = "低风险";
                risk.GradeVal = 4;
            }
        }
        #endregion
       
        [HandlerLogin(LoginMode.Ignore)]
        [HandlerAuthorize(PermissionMode.Ignore)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(5, "导入风险库")]
        public string ImportDatabase()
        {
            int error = 0;
            string message = "请选择格式正确的文件再导入!";
            StringBuilder sb = new StringBuilder("具体错误位置：<br />");
            int count = HttpContext.Request.Files.Count;
            if (count > 0)
            {
                HttpPostedFileBase file = HttpContext.Request.Files[0];
                if (string.IsNullOrEmpty(file.FileName))
                {
                    return message;
                }
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName));
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                wb.Open(Server.MapPath("~/Resource/temp/" + fileName), Aspose.Cells.FileFormatType.Excel2003);
                MeasuresBLL measureBLL = new MeasuresBLL();
                Aspose.Cells.Cells cells = wb.Worksheets[0].Cells;
                List<string> types = new List<string> { "工程技术", "管理", "个体防护", "培训教育", "应急处置" };
                #region 作业风险库

                DataTable dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn, false);

                for (int j = 1; j < dt.Rows.Count; j++)
                {
                    //区域名称
                    string areaName = dt.Rows[j][2].ToString();
                    //岗位
                    string postName = dt.Rows[j][22].ToString().Trim();
                    //管控部门
                    string deptName = dt.Rows[j][21].ToString().Trim();
                    if (string.IsNullOrEmpty(areaName))
                    {
                        continue;
                    }
                    try
                    {
                        //获取区域信息并判断该区域是否存在，没有则新增该区域
                        string areaId = dangerBLL.GetIdByName(areaName);
                        if (string.IsNullOrEmpty(areaId))
                        {
                            //新增区域信息
                            DangerSourceEntity ds = new DangerSourceEntity();
                            areaId = ds.Id = Guid.NewGuid().ToString();
                            ds.Name = areaName;
                            ds.DataType = -1; //设置内置的区域数据类型为-1
                            ds.ParentId = "0";
                            dangerBLL.SaveForm("", ds);
                        }
                        //管控层级
                        string level = dt.Rows[j][20].ToString().Trim();
                        ////风险后果分类
                        //string resultType = dt.Rows[j][6].ToString().Trim();
                        //危害源
                        string dangerSource = dt.Rows[j][6].ToString().Trim();
                        //风险描述
                        string desc = dt.Rows[j][7].ToString().Trim();
                        //风险类别
                        string riskType = "作业";
                        //风险后果
                        string result = dt.Rows[j][8].ToString().Trim();
                        //工程技术措施
                        string gcjscs = dt.Rows[j][15].ToString().Trim();
                        //管理措施
                        string glcs = dt.Rows[j][16].ToString().Trim();
                        //个人防护措施
                        string grfhcs = dt.Rows[j][17].ToString().Trim();
                        //培训教育措施
                        string pxjycs = dt.Rows[j][18].ToString().Trim();
                        //应急处置措施
                        string yjczcs = dt.Rows[j][19].ToString().Trim();
                        List<string> values = new List<string> { gcjscs, glcs, grfhcs, pxjycs, yjczcs };
                        string measures = "";
                        foreach (string str in values)
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                measures += str + "\r\n";
                            }
                        }
                        //评价方式
                        string way = dt.Rows[j][9].ToString().Trim();
                        //辨识评价可能性
                        string itemA = dt.Rows[j][10].ToString().Trim();
                        //辨识评价频繁程度
                        string itemB = dt.Rows[j][11].ToString().Trim();
                        //辨识评价后果
                        string itemC = dt.Rows[j][12].ToString().Trim();
                        //辨识评价风险值
                        string itemR = dt.Rows[j][13].ToString().Trim();
                        //辨识评价风险等级
                        string grade = dt.Rows[j][14].ToString().Trim();

                        ////可能导致的事故类型
                        //string accType = dt.Rows[j][8].ToString().Trim();
                        ////隐患描述
                        //string htDesc = dt.Rows[j][20].ToString().Trim();
                        ////是否违章
                        //string isWZ = dt.Rows[j][21].ToString().Trim();
                        ////隐患等级
                        //string htGrade = dt.Rows[j][22].ToString().Trim();
                        ////隐患类别
                        //string htType = dt.Rows[j][23].ToString().Trim();
                        ////隐患整改措施
                        //string htMeasures = dt.Rows[j][24].ToString().Trim();
                        //工作任务
                        string workTask = dt.Rows[j][4].ToString().Trim();
                        //工序
                        string process = dt.Rows[j][5].ToString().Trim();

                        RiskEntity risk = new RiskEntity();
                        risk.Id = Guid.NewGuid().ToString();
                        risk.Result = result;
                        risk.RiskType = riskType;
                        //risk.AccidentName = risk.AccidentType = accType;
                        risk.RiskDesc = desc;
                        //risk.ResultType = resultType;
                        risk.PostName = postName;
                        risk.Result = result;
                        risk.AreaId = areaId;
                        risk.AreaName = areaName;
                        risk.WorkTask = workTask;
                        risk.Process = process;
                        risk.Way = way;
                        //risk.IsWZ = isWZ;
                        //risk.HtDesc = htDesc;
                        //risk.HtGrade = htGrade;
                        //risk.HtType = htType;
                        //risk.HTMeasures = htMeasures;
                        risk.LevelName = level;
                        risk.Measure = measures;
                        risk.DangerSource = dangerSource;
                        risk.ItemA = string.IsNullOrEmpty(itemA) ? 0 : decimal.Parse(itemA);
                        risk.ItemB = string.IsNullOrEmpty(itemB) ? 0 : decimal.Parse(itemB);
                        risk.ItemC = string.IsNullOrEmpty(itemC) ?1 : decimal.Parse(itemC);
                        //risk.ItemR = risk.ItemA * risk.ItemB * risk.ItemC;
                        int gradeVal = 4;
                        if (way == "TRA")
                        {
                            risk.ItemR = risk.ItemA * risk.ItemB;

                            if (risk.ItemR >= 20)
                            {
                                grade = "重大风险";
                                gradeVal = 1;
                            }
                            else if (risk.ItemR >= 10 && risk.ItemR < 20)
                            {
                                grade = "较大风险";
                                gradeVal = 2;
                            }
                            else if (risk.ItemR >= 5 && risk.ItemR < 10)
                            {
                                grade = "一般风险";
                                gradeVal = 3;
                            }
                            else
                            {
                                grade = "低风险";
                                gradeVal = 4;
                            }
                        }
                        else
                        {
                            risk.ItemR = risk.ItemA * risk.ItemB * risk.ItemC;
                            if (risk.ItemR >= 400)
                            {
                                grade = "重大风险";
                                gradeVal = 1;
                            }
                            else if (risk.ItemR >= 200 && risk.ItemR < 400)
                            {
                                grade = "较大风险";
                                gradeVal = 2;
                            }
                            else if (risk.ItemR >= 70 && risk.ItemR < 200)
                            {
                                grade = "一般风险";
                                gradeVal = 3;
                            }
                            else
                            {
                                grade = "低风险";
                                gradeVal = 4;
                            }
                        }
                        risk.GradeVal = gradeVal;
                        risk.Grade = grade;
                        risk.DeptName = deptName;
                        risk.Status = 0;
                        RiskBLL riskBLL = new RiskBLL();

                        if (riskBLL.SaveForm("", risk) > 0)
                        {
                            int k = 0;
                            foreach (string str in values)
                            {
                                if (!string.IsNullOrEmpty(str))
                                {
                                    MeasuresEntity measure = new MeasuresEntity();
                                    measure.TypeName = types[k].Replace("措施", "");
                                    measure.RiskId = risk.Id;
                                    measure.Content = str;
                                    measure.AreaId = areaId;
                                    measureBLL.Save("", measure);
                                }
                                k++;
                            }
                            //MeasuresEntity measure = new MeasuresEntity();
                            //measure.TypeName = types[k].Replace("措施", "");
                            //measure.RiskId = risk.Id;
                            //measure.Content = measures;
                            //measure.AreaId = areaId;
                            //measureBLL.Save("", measure);
                        }
                        else
                        {
                            sb.AppendFormat("作业风险库-行:{0}，", j + 1);
                            error++;
                        }
                    }
                    catch (Exception ex)
                    {
                        sb.AppendFormat("作业风险库-行:{0}({1})，", j + 1, ex.Message);
                        error++;
                    }

                }
                count = dt.Rows.Count - 1;
                #endregion

                #region 设备风险库
                cells = wb.Worksheets[1].Cells;
                dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn, false);
                for (int j = 1; j < dt.Rows.Count; j++)
                {
                    //区域名称
                    string areaName = dt.Rows[j][2].ToString();
                    //管控部门
                    string deptName = dt.Rows[j][20].ToString().Trim();
                    if (string.IsNullOrEmpty(areaName))
                    {
                        continue;
                    }
                    try
                    {
                        //获取区域信息并判断该区域是否存在，没有则新增该区域
                        string areaId = dangerBLL.GetIdByName(areaName);
                        if (string.IsNullOrEmpty(areaId))
                        {
                            //新增区域信息
                            DangerSourceEntity ds = new DangerSourceEntity();
                            areaId = ds.Id = Guid.NewGuid().ToString();
                            ds.Name = areaName;
                            ds.DataType = -1; //设置内置的区域数据类型为-1
                            ds.ParentId = "0";
                            dangerBLL.SaveForm("", ds);
                        }
                        //岗位
                        string postName = dt.Rows[j][21].ToString().Trim();

                        //管控层级
                        string level = dt.Rows[j][19].ToString().Trim();
                        ////风险后果分类
                        //string resultType = dt.Rows[j][6].ToString().Trim();
                        //危害源
                        string dangerSource = dt.Rows[j][5].ToString().Trim();
                        //风险类别
                        string riskType = "设备";
                        //风险后果
                        string result = dt.Rows[j][7].ToString().Trim();
                        //工程技术措施
                        string gcjscs = dt.Rows[j][14].ToString().Trim();
                        //管理措施
                        string glcs = dt.Rows[j][15].ToString().Trim();
                        //个人防护措施
                        string grfhcs = dt.Rows[j][16].ToString().Trim();
                        //培训教育措施
                        string pxjycs = dt.Rows[j][17].ToString().Trim();
                        //应急处置措施
                        string yjczcs = dt.Rows[j][18].ToString().Trim();
                        List<string> values = new List<string> { gcjscs, glcs, grfhcs, pxjycs, yjczcs };
                        string measures = "";
                        foreach (string str in values)
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                measures += str + "\r\n";
                            }
                        }
                        //评价方式
                        string way = dt.Rows[j][8].ToString().Trim();
                        //辨识评价可能性
                        string itemA = dt.Rows[j][9].ToString().Trim();
                        //辨识评价频繁程度
                        string itemB = dt.Rows[j][10].ToString().Trim();
                        //辨识评价后果
                        string itemC = dt.Rows[j][11].ToString().Trim();
                        //辨识评价风险值
                        string itemR = dt.Rows[j][12].ToString().Trim();
                        //辨识评价风险等级
                        string grade = dt.Rows[j][13].ToString().Trim();
                        //可能导致的事故类型
                        //string accType = dt.Rows[j][8].ToString().Trim();
                        //隐患描述
                        //string htDesc = dt.Rows[j][20].ToString().Trim();
                        ////是否违章
                        //string isWZ = dt.Rows[j][21].ToString().Trim();
                        ////隐患等级
                        //string htGrade = dt.Rows[j][22].ToString().Trim();
                        ////隐患类别
                        //string htType = dt.Rows[j][23].ToString().Trim();
                        ////隐患整改措施
                        //string htMeasures = dt.Rows[j][24].ToString().Trim();
                        //设备名称
                        string machineName = dt.Rows[j][3].ToString().Trim();
                        //部件
                        string parts = dt.Rows[j][4].ToString().Trim();

                        //风险描述
                        string desc = parts + dt.Rows[j][6].ToString().Trim();



                        ////管控措施分类
                        //string typename = dt.Rows[j][16].ToString().Trim();

                        ////隐患描述
                        //string htDesc = dt.Rows[j][20].ToString().Trim();
                        ////是否违章
                        //string isWZ = dt.Rows[j][21].ToString().Trim();
                        ////隐患等级
                        //string htGrade = dt.Rows[j][22].ToString().Trim();
                        ////隐患类别
                        //string htType = dt.Rows[j][23].ToString().Trim();
                        ////隐患整改措施
                        //string htMeasures = dt.Rows[j][24].ToString().Trim();


                        RiskEntity risk = new RiskEntity();
                        risk.Id = Guid.NewGuid().ToString();
                        risk.Result = result;
                        risk.RiskType = riskType;
                        //risk.AccidentName = risk.AccidentType = accType;
                        risk.RiskDesc = desc;
                        //risk.ResultType = resultType;
                        risk.PostName = postName;
                        risk.Result = result;
                        risk.AreaId = areaId;
                        risk.AreaName = areaName;
                        risk.EquipmentName = machineName;
                        risk.Parts = parts;
                        risk.Way = way;
                        //risk.IsWZ = isWZ;
                        risk.LevelName = level;
                        //risk.HtDesc = htDesc;
                        //risk.HtGrade = htGrade;
                        //risk.HtType = htType;
                        //risk.HTMeasures = htMeasures;
                        risk.Measure = measures;
                        risk.DangerSource = dangerSource;
                        risk.ItemA = string.IsNullOrEmpty(itemA) ? 0 : decimal.Parse(itemA);
                        risk.ItemB = string.IsNullOrEmpty(itemB) ? 0 : decimal.Parse(itemB);
                        risk.ItemC = string.IsNullOrEmpty(itemC) ? 1 : decimal.Parse(itemC);
                        //risk.ItemR = risk.ItemA * risk.ItemB * risk.ItemC;
                        int gradeVal = 4;
                        if (way == "TRA")
                        {
                            risk.ItemR = risk.ItemA * risk.ItemB;

                            if (risk.ItemR >= 20)
                            {
                                grade = "重大风险";
                                gradeVal = 1;
                            }
                            else if (risk.ItemR >= 10 && risk.ItemR < 20)
                            {
                                grade = "较大风险";
                                gradeVal = 2;
                            }
                            else if (risk.ItemR >= 5 && risk.ItemR < 10)
                            {
                                grade = "一般风险";
                                gradeVal = 3;
                            }
                            else
                            {
                                grade = "低风险";
                                gradeVal = 4;
                            }
                        }
                        else
                        {
                            risk.ItemR = risk.ItemA * risk.ItemB * risk.ItemC;
                            if (risk.ItemR >= 400)
                            {
                                grade = "重大风险";
                                gradeVal = 1;
                            }
                            else if (risk.ItemR >= 200 && risk.ItemR < 400)
                            {
                                grade = "较大风险";
                                gradeVal = 2;
                            }
                            else if (risk.ItemR >= 70 && risk.ItemR < 200)
                            {
                                grade = "一般风险";
                                gradeVal = 3;
                            }
                            else
                            {
                                grade = "低风险";
                                gradeVal = 4;
                            }
                        }
                        risk.GradeVal = gradeVal;
                        risk.Grade = grade;
                        risk.DeptName = deptName;
                        risk.Status = 0;
                        risk.FaultType = dangerSource;
                        RiskBLL riskBLL = new RiskBLL();

                        if (riskBLL.SaveForm("", risk) > 0)
                        {
                            int k = 0;
                            foreach (string str in values)
                            {
                                if (!string.IsNullOrEmpty(str))
                                {

                                    MeasuresEntity measure = new MeasuresEntity();
                                    measure.TypeName = types[k].Replace("措施", "");
                                    measure.RiskId = risk.Id;
                                    measure.Content = str;
                                    measure.AreaId = areaId;
                                    measureBLL.Save("", measure);
                                }
                                k++;
                            }
                        }
                        else
                        {
                            sb.AppendFormat("设备风险库-行:{0}，", j + 1);
                            error++;
                        }
                    }
                    catch (Exception ex)
                    {
                        sb.AppendFormat("设备风险库-行:{0}({1})，", j + 1, ex.Message);
                        error++;
                    }

                }
                count += dt.Rows.Count - 1;
                #endregion

                #region 环境风险库
                cells = wb.Worksheets[2].Cells;
                dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn, false);
                for (int j = 1; j < dt.Rows.Count; j++)
                {
                    //区域名称
                    string areaName = dt.Rows[j][2].ToString();
                    //管控部门
                    string deptName = dt.Rows[j][18].ToString().Trim();
                    if (string.IsNullOrEmpty(areaName))
                    {
                        continue;
                    }
                    try
                    {
                        //获取区域信息并判断该区域是否存在，没有则新增该区域
                        string areaId = dangerBLL.GetIdByName(areaName);
                        if (string.IsNullOrEmpty(areaId))
                        {
                            //新增区域信息
                            DangerSourceEntity ds = new DangerSourceEntity();
                            areaId = ds.Id = Guid.NewGuid().ToString();
                            ds.Name = areaName;
                            ds.DataType = -1; //设置内置的区域数据类型为-1
                            ds.ParentId = "0";
                            dangerBLL.SaveForm("", ds);
                        }
                        //岗位
                        string postName = dt.Rows[j][19].ToString().Trim();

                        //管控层级
                        string level = dt.Rows[j][17].ToString().Trim();
                        ////风险后果分类
                        //string resultType = dt.Rows[j][4].ToString().Trim();
                        //危险源
                        string dangerSource = dt.Rows[j][3].ToString().Trim();

                        //风险类别
                        string riskType = "环境";
                        //风险后果
                        string result = dt.Rows[j][5].ToString().Trim();
                        //工程技术措施
                        string gcjscs = dt.Rows[j][12].ToString().Trim();
                        //管理措施
                        string glcs = dt.Rows[j][13].ToString().Trim();
                        //个人防护措施
                        string grfhcs = dt.Rows[j][14].ToString().Trim();
                        //培训教育措施
                        string pxjycs = dt.Rows[j][15].ToString().Trim();
                        //应急处置措施
                        string yjczcs = dt.Rows[j][16].ToString().Trim();
                        List<string> values = new List<string> { gcjscs, glcs, grfhcs, pxjycs, yjczcs };
                        string measures = "";
                        foreach (string str in values)
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                measures += str + "\r\n";
                            }
                        }
                        //评价方式
                        string way = dt.Rows[j][6].ToString().Trim();
                        //辨识评价可能性
                        string itemA = dt.Rows[j][7].ToString().Trim();
                        //辨识评价频繁程度
                        string itemB = dt.Rows[j][8].ToString().Trim();
                        //辨识评价后果
                        string itemC = dt.Rows[j][9].ToString().Trim();
                        //辨识评价风险值
                        string itemR = dt.Rows[j][10].ToString().Trim();
                        //辨识评价风险等级
                        string grade = dt.Rows[j][11].ToString().Trim();
                        //风险描述
                        string desc = dt.Rows[j][4].ToString().Trim();


                        RiskEntity risk = new RiskEntity();
                        risk.Id = Guid.NewGuid().ToString();
                        risk.Result = result;
                        risk.RiskType = riskType;
                        //risk.AccidentName = risk.AccidentType = accType;
                        risk.RiskDesc = desc;
                        //risk.ResultType = resultType;
                        risk.PostName = postName;
                        risk.Result = result;
                        risk.LevelName = level;
                        risk.AreaId = areaId;
                        risk.AreaName = areaName;
                        risk.Way = way;
                        //risk.IsWZ = isWZ;
                        //risk.HtDesc = htDesc;
                        //risk.HtGrade = htGrade;
                        //risk.HtType = htType;
                        //risk.HTMeasures = htMeasures;
                        risk.Measure = measures;
                        risk.DangerSource = dangerSource;
                        risk.ItemA = string.IsNullOrEmpty(itemA) ? 0 : decimal.Parse(itemA);
                        risk.ItemB = string.IsNullOrEmpty(itemB) ? 0 : decimal.Parse(itemB);
                        risk.ItemC = string.IsNullOrEmpty(itemC) ? 1 : decimal.Parse(itemC);
                        //risk.ItemR = risk.ItemA * risk.ItemB * risk.ItemC;
                        int gradeVal = 4;
                        if (way == "TRA")
                        {
                            risk.ItemR = risk.ItemA * risk.ItemB;

                            if (risk.ItemR >= 20)
                            {
                                grade = "重大风险";
                                gradeVal = 1;
                            }
                            else if (risk.ItemR >= 10 && risk.ItemR < 20)
                            {
                                grade = "较大风险";
                                gradeVal = 2;
                            }
                            else if (risk.ItemR >= 5 && risk.ItemR < 10)
                            {
                                grade = "一般风险";
                                gradeVal = 3;
                            }
                            else
                            {
                                grade = "低风险";
                                gradeVal = 4;
                            }
                        }
                        else
                        {
                            risk.ItemR = risk.ItemA * risk.ItemB * risk.ItemC;
                            if (risk.ItemR >= 400)
                            {
                                grade = "重大风险";
                                gradeVal = 1;
                            }
                            else if (risk.ItemR >= 200 && risk.ItemR < 400)
                            {
                                grade = "较大风险";
                                gradeVal = 2;
                            }
                            else if (risk.ItemR >= 70 && risk.ItemR < 200)
                            {
                                grade = "一般风险";
                                gradeVal = 3;
                            }
                            else
                            {
                                grade = "低风险";
                                gradeVal = 4;
                            }
                        }
                        risk.GradeVal = gradeVal;
                        risk.Grade = grade;
                        risk.DeptName = deptName;
                        risk.Status = 0;
                        RiskBLL riskBLL = new RiskBLL();

                        if (riskBLL.SaveForm("", risk) > 0)
                        {
                            int k = 0;
                            foreach (string str in values)
                            {
                                if (!string.IsNullOrEmpty(str))
                                {
                                    MeasuresEntity measure = new MeasuresEntity();
                                    measure.TypeName = types[k].Replace("措施", "");
                                    measure.RiskId = risk.Id;
                                    measure.Content = str;
                                    measure.AreaId = areaId;
                                    measureBLL.Save("", measure);
                                }
                                k++;
                            }
                        }
                        else
                        {
                            sb.AppendFormat("环境风险库-行:{0}，", j + 1);
                            error++;
                        }
                    }
                    catch (Exception ex)
                    {
                        sb.AppendFormat("环境风险库-行:{0}({1})，", j + 1, ex.Message);
                        error++;
                    }

                }
                count += dt.Rows.Count - 1;
                #endregion

                #region 管理风险库

                ////获取区域信息并判断该区域是否存在，没有则新增该区域
                //string aName = "厂区";
                //string aId = dangerBLL.GetIdByName(aName);
                //if (string.IsNullOrEmpty(aId))
                //{
                //    //新增区域信息
                //    DangerSourceEntity ds = new DangerSourceEntity();
                //    aId = ds.Id = Guid.NewGuid().ToString();
                //    ds.Name = "厂区";
                //    ds.DataType = -1; //设置内置的区域数据类型为-1
                //    ds.ParentId = "0";
                //    dangerBLL.SaveForm("", ds);
                //}

                cells = wb.Worksheets[3].Cells;
                dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn, false);
                for (int j = 1; j < dt.Rows.Count; j++)
                {
                    //区域名称
                    string areaName = dt.Rows[j][2].ToString();
                    //管控部门
                    string deptName = dt.Rows[j][18].ToString().Trim();
                    if (string.IsNullOrEmpty(areaName))
                    {
                        continue;
                    }
                    try
                    {
                        //获取区域信息并判断该区域是否存在，没有则新增该区域
                        string areaId = dangerBLL.GetIdByName(areaName);
                        if (string.IsNullOrEmpty(areaId))
                        {
                            //新增区域信息
                            DangerSourceEntity ds = new DangerSourceEntity();
                            areaId = ds.Id = Guid.NewGuid().ToString();
                            ds.Name = areaName;
                            ds.DataType = -1; //设置内置的区域数据类型为-1
                            ds.ParentId = "0";
                            dangerBLL.SaveForm("", ds);
                        }
                        //岗位
                        string postName = dt.Rows[j][19].ToString().Trim();

                        //管控层级
                        string level = dt.Rows[j][17].ToString().Trim();
                        ////风险后果分类
                        //string resultType = dt.Rows[j][4].ToString().Trim();
                        //危险源
                        string dangerSource = dt.Rows[j][3].ToString().Trim();

                        //风险类别
                        string riskType = "管理";
                        //风险后果
                        string result = dt.Rows[j][5].ToString().Trim();
                        //工程技术措施
                        string gcjscs = dt.Rows[j][12].ToString().Trim();
                        //管理措施
                        string glcs = dt.Rows[j][13].ToString().Trim();
                        //个人防护措施
                        string grfhcs = dt.Rows[j][14].ToString().Trim();
                        //培训教育措施
                        string pxjycs = dt.Rows[j][15].ToString().Trim();
                        //应急处置措施
                        string yjczcs = dt.Rows[j][16].ToString().Trim();
                        List<string> values = new List<string> { gcjscs, glcs, grfhcs, pxjycs, yjczcs };
                        string measures = "";
                        foreach (string str in values)
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                measures += str + "\r\n";
                            }
                        }
                        //评价方式
                        string way = dt.Rows[j][6].ToString().Trim();
                        //辨识评价可能性
                        string itemA = dt.Rows[j][7].ToString().Trim();
                        //辨识评价频繁程度
                        string itemB = dt.Rows[j][8].ToString().Trim();
                        //辨识评价后果
                        string itemC = dt.Rows[j][9].ToString().Trim();
                        //辨识评价风险值
                        string itemR = dt.Rows[j][10].ToString().Trim();
                        //辨识评价风险等级
                        string grade = dt.Rows[j][11].ToString().Trim();
                        //风险描述
                        string desc = dt.Rows[j][4].ToString().Trim();


                        RiskEntity risk = new RiskEntity();
                        risk.Id = Guid.NewGuid().ToString();
                        risk.Result = result;
                        risk.RiskType = riskType;
                        //risk.AccidentName = risk.AccidentType = accType;
                        risk.RiskDesc = desc;
                        //risk.ResultType = resultType;
                        risk.PostName = postName;
                        risk.Result = result;
                        risk.AreaId = areaId;
                        risk.AreaName = areaName;
                        risk.Way = way;
                        //risk.IsWZ = isWZ;
                        //risk.HtDesc = htDesc;
                        //risk.HtGrade = htGrade;
                        //risk.HtType = htType;
                        risk.LevelName = level;
                        //risk.HTMeasures = htMeasures;
                        risk.Measure = measures;
                        risk.DangerSource = dangerSource;
                        risk.ItemA = string.IsNullOrEmpty(itemA) ? 0 : decimal.Parse(itemA);
                        risk.ItemB = string.IsNullOrEmpty(itemB) ? 0 : decimal.Parse(itemB);
                        risk.ItemC = string.IsNullOrEmpty(itemC) ? 1 : decimal.Parse(itemC);
                        //risk.ItemR = risk.ItemA * risk.ItemB * risk.ItemC;
                        int gradeVal = 4;

                        if (way == "TRA")
                        {
                            risk.ItemR = risk.ItemA * risk.ItemB;

                            if (risk.ItemR >= 20)
                            {
                                grade = "重大风险";
                                gradeVal = 1;
                            }
                            else if (risk.ItemR >= 10 && risk.ItemR < 20)
                            {
                                grade = "较大风险";
                                gradeVal = 2;
                            }
                            else if (risk.ItemR >= 5 && risk.ItemR < 10)
                            {
                                grade = "一般风险";
                                gradeVal = 3;
                            }
                            else
                            {
                                grade = "低风险";
                                gradeVal = 4;
                            }
                        }
                        else
                        {
                            risk.ItemR = risk.ItemA * risk.ItemB * risk.ItemC;
                            if (risk.ItemR >= 400)
                            {
                                grade = "重大风险";
                                gradeVal = 1;
                            }
                            else if (risk.ItemR >= 200 && risk.ItemR < 400)
                            {
                                grade = "较大风险";
                                gradeVal = 2;
                            }
                            else if (risk.ItemR >= 70 && risk.ItemR < 200)
                            {
                                grade = "一般风险";
                                gradeVal = 3;
                            }
                            else
                            {
                                grade = "低风险";
                                gradeVal = 4;
                            }
                        }
                        risk.GradeVal = gradeVal;
                        risk.Grade = grade;
                        risk.DeptName = deptName;
                        risk.Status = 0;
                        RiskBLL riskBLL = new RiskBLL();

                        if (riskBLL.SaveForm("", risk) > 0)
                        {
                            int k = 0;
                            foreach (string str in values)
                            {
                                if (!string.IsNullOrEmpty(str))
                                {
                                    MeasuresEntity measure = new MeasuresEntity();
                                    measure.TypeName = types[k].Replace("措施", "");
                                    measure.RiskId = risk.Id;
                                    measure.Content = str;
                                    measure.AreaId = areaId;
                                    measureBLL.Save("", measure);
                                }
                                k++;
                            }
                        }
                        else
                        {
                            sb.AppendFormat("管理风险库-行:{0}，", j + 1);
                            error++;
                        }
                    }
                    catch (Exception ex)
                    {
                        sb.AppendFormat("管理风险库-行:{0}({1})，", j + 1, ex.Message);
                        error++;
                    }
                }
                count += dt.Rows.Count - 1;
                #endregion

                #region 职业病危害库
                cells = wb.Worksheets[4].Cells;
                dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn, false);
                for (int j = 1; j < dt.Rows.Count; j++)
                {
                    //区域名称
                    string areaName = dt.Rows[j][3].ToString();
                    //管控部门
                    string deptName = dt.Rows[j][19].ToString().Trim();
                    if (string.IsNullOrEmpty(areaName) || string.IsNullOrEmpty(deptName))
                    {
                        sb.AppendFormat("区域和管控部门不能为空！职业病危害库-行:{0}<br />", j);
                        error++;
                        continue;
                    }
                    try
                    {
                        //获取区域信息并判断该区域是否存在，没有则新增该区域
                        string areaId = dangerBLL.GetIdByName(areaName);
                        if (string.IsNullOrEmpty(areaId))
                        {
                            //新增区域信息
                            DangerSourceEntity ds = new DangerSourceEntity();
                            areaId = ds.Id = Guid.NewGuid().ToString();
                            ds.Name = areaName;
                            ds.DataType = -1; //设置内置的区域数据类型为-1
                            ds.ParentId = "0";
                            dangerBLL.SaveForm("", ds);
                        }
                        //岗位
                        string postName = dt.Rows[j][20].ToString().Trim();

                        //管控层级
                        string level = dt.Rows[j][18].ToString().Trim();
                        //风险点
                        string majorname = dt.Rows[j][1].ToString().Trim();
                        //职业病危害因素
                        string dangerSource = dt.Rows[j][4].ToString().Trim();
                        //作业分级
                        string workType = dt.Rows[j][5].ToString().Trim();
                        //导致的职业病或健康损伤
                        string illType = dt.Rows[j][6].ToString().Trim();
                        //风险分类
                        string riskType = "职业病危害";
                        ////
                        //string result = dt.Rows[j]["职业病危害因素"].ToString().Trim();
                        //工程技术措施
                        string gcjscs = dt.Rows[j][13].ToString().Trim();
                        //管理措施
                        string glcs = dt.Rows[j][14].ToString().Trim();
                        //个人防护措施
                        string grfhcs = dt.Rows[j][15].ToString().Trim();
                        //培训教育措施
                        string pxjycs = dt.Rows[j][16].ToString().Trim();
                        //应急处置措施
                        string yjczcs = dt.Rows[j][17].ToString().Trim();
                        List<string> values = new List<string> { gcjscs, glcs, grfhcs, pxjycs, yjczcs };
                        string measures = "";
                        foreach (string str in values)
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                measures += str + "\r\n";
                            }
                        }
                        //评价方式
                        string way = dt.Rows[j][7].ToString().Trim();
                        //辨识评价可能性
                        string itemA = dt.Rows[j][8].ToString().Trim();
                        //辨识评价频繁程度
                        string itemB = dt.Rows[j][9].ToString().Trim();
                        //辨识评价后果
                        string itemC = dt.Rows[j][10].ToString().Trim();
                        //辨识评价风险值
                        string itemR = dt.Rows[j][11].ToString().Trim();
                        //辨识评价风险等级
                        string grade = dt.Rows[j][12].ToString().Trim();

                        RiskEntity risk = new RiskEntity();

                        //if (string.IsNullOrEmpty(risk.DeptCode))
                        //{
                        //    sb.AppendFormat("部门与系统的部门信息不匹配！职业病危害库-行:{0}<br />", j);
                        //    error++;
                        //    continue;
                        //}

                        risk.Id = Guid.NewGuid().ToString();
                        //risk.Result = result;
                        risk.MajorName = majorname;
                        risk.Description = dangerSource;
                        risk.HarmType = workType;
                        risk.HarmProperty = illType;
                        risk.RiskType = riskType;
                        risk.PostName = postName;

                        risk.DeptName = deptName;
                        //risk.Result = result;
                        risk.AreaId = areaId;
                        //risk.DistrictId = areaId;

                        risk.AreaName = areaName;
                        risk.Way = way;
                        risk.Status = 1;
                        risk.LevelName = level;
                        risk.Measure = measures;
                        risk.ItemA = string.IsNullOrEmpty(itemA) ? 0 : decimal.Parse(itemA);
                        risk.ItemB = string.IsNullOrEmpty(itemB) ? 0 : decimal.Parse(itemB);
                        risk.ItemC = string.IsNullOrEmpty(itemC) ?1 : decimal.Parse(itemC);
                        //risk.ItemR = risk.ItemA  * risk.ItemB * risk.ItemC;
                        int gradeVal = 4;
                        if (way == "TRA")
                        {
                            risk.ItemR = risk.ItemA * risk.ItemB;

                            if (risk.ItemR >= 20)
                            {
                                grade = "重大风险";
                                gradeVal = 1;
                            }
                            else if (risk.ItemR >= 10 && risk.ItemR < 20)
                            {
                                grade = "较大风险";
                                gradeVal = 2;
                            }
                            else if (risk.ItemR >= 5 && risk.ItemR < 10)
                            {
                                grade = "一般风险";
                                gradeVal = 3;
                            }
                            else
                            {
                                grade = "低风险";
                                gradeVal = 4;
                            }
                        }
                        else
                        {
                            risk.ItemR = risk.ItemA * risk.ItemB * risk.ItemC;
                            if (risk.ItemR >= 400)
                            {
                                grade = "重大风险";
                                gradeVal = 1;
                            }
                            else if (risk.ItemR >= 200 && risk.ItemR < 400)
                            {
                                grade = "较大风险";
                                gradeVal = 2;
                            }
                            else if (risk.ItemR >= 70 && risk.ItemR < 200)
                            {
                                grade = "一般风险";
                                gradeVal = 3;
                            }
                            else
                            {
                                grade = "低风险";
                                gradeVal = 4;
                            }
                        }
                        risk.GradeVal = gradeVal;
                        risk.Grade = grade;
                        risk.DeptName = deptName;
                        RiskBLL riskBLL = new RiskBLL();

                        if (riskBLL.SaveForm("", risk) > 0)
                        {
                            int k = 0;
                            foreach (string str in values)
                            {
                                if (!string.IsNullOrEmpty(str))
                                {
                                    MeasuresEntity measure = new MeasuresEntity();
                                    measure.TypeName = types[k].Replace("措施", "");
                                    measure.RiskId = risk.Id;
                                    measure.Content = str;
                                    measure.AreaId = areaId;
                                    measureBLL.Save("", measure);
                                }
                                k++;
                            }
                        }
                        else
                        {
                            sb.AppendFormat("职业病危害库-行:{0}<br />", j);
                            error++;
                        }
                    }
                    catch (Exception ex)
                    {
                        sb.AppendFormat("职业病危害库-行:{0}({1})<br />", j, ex.Message);
                        error++;
                    }

                }
                if (dt.Rows.Count > 1)
                {
                    count += dt.Rows.Count - 1;
                }
                #endregion

                message = "共有" + count + "条记录,成功导入" + (count - error) + "条，失败" + error + "条。";
                if (error > 0)
                {
                    message += sb.ToString();
                }
            }

            return message;
        }
       
        public ActionResult ExportSumList(string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = int.MaxValue;
            pagination.p_kid = "id";
            pagination.p_fields = "risktype,riskname,districtname,gradeval,'' grade,'' fl,'' unit,'' man,'' num";
            pagination.p_tablename = "v_risklist";
            pagination.conditionJson = "status=1 and deletemark=0 and enabledmark=0";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.conditionJson = "deptcode like '" + user.OrganizeCode + "%'";
            try
            {
                RiskAssessBLL riskassessbll = new RiskAssessBLL();
                var data = riskassessbll.GetPageList(pagination, queryJson);
                DepartmentBLL deptBll = new DepartmentBLL();
                DataTable dtItems = new DataTable();
                foreach (DataRow dr in data.Rows)
                {
                    if(dr["gradeval"].ToString()=="1")
                    {
                        dr["grade"] = "重大风险";
                    }
                    if (dr["gradeval"].ToString() == "2")
                    {
                        dr["grade"] = "较大风险";
                    }
                    if (dr["gradeval"].ToString() == "3")
                    {
                        dr["grade"] = "一般风险";
                    }
                    if (dr["gradeval"].ToString() == "4")
                    {
                        dr["grade"] = "低风险";
                    }
                    string sql = string.Format("select levelname,deptname,dutyperson from BIS_RISKASSESS where districtname='{0}' and risktype='{1}'", dr["districtname"].ToString(), dr["risktype"].ToString());
                    if (dr["risktype"].ToString() == "作业")
                    {
                        sql += string.Format(" and worktask='{0}'", dr["riskname"].ToString());
                    }
                    if (dr["risktype"].ToString() == "设备")
                    {
                        sql += string.Format(" and equipmentname='{0}'", dr["riskname"].ToString());
                    }
                    if (dr["risktype"].ToString() == "区域")
                    {
                        sql += string.Format(" and HjSystem='{0}'", dr["riskname"].ToString());
                    }
                    if (dr["risktype"].ToString() == "管理")
                    {
                        sql += string.Format(" and dangersource='{0}'", dr["riskname"].ToString());
                    }
                    if (dr["risktype"].ToString() == "岗位")
                    {
                        sql += string.Format(" and JobName='{0}'", dr["riskname"].ToString());
                    }
                    if (dr["risktype"].ToString() == "工器具及危化品")
                    {
                        sql += string.Format(" and ToolOrDanger='{0}'", dr["riskname"].ToString());
                    }
                    dtItems = deptBll.GetDataTable(sql);
                    List<string> list = new List<string>();
                    foreach (DataRow dr1 in dtItems.Rows)
                    {
                        ;
                        foreach (string str in dr1["levelname"].ToString().Split(','))
                        {
                            if (!list.Contains(str))
                            {
                                list.Add(str);
                            }
                        }
                    }
                    dr["fl"] = string.Join(",", list.ToArray());
                    list = new List<string>();
                    foreach (DataRow dr1 in dtItems.Rows)
                    {
                        ;
                        foreach (string str in dr1["deptname"].ToString().Split(','))
                        {
                            if (!list.Contains(str))
                            {
                                list.Add(str);
                            }
                        }
                    }
                    dr["unit"] = string.Join(",", list.ToArray());
                    list = new List<string>();
                    foreach (DataRow dr1 in dtItems.Rows)
                    {
                        ;
                        foreach (string str in dr1["dutyperson"].ToString().Split(','))
                        {
                            if (!list.Contains(str))
                            {
                                list.Add(str);
                            }
                        }
                    }
                    dr["man"] = string.Join(",", list.ToArray());
                    dr["num"] = dtItems.Rows.Count;
                }
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "风险清单汇总表";
                excelconfig.FileName = "风险清单汇总表.xls";
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "risktype", ExcelColumn = "风险类别", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "riskname", ExcelColumn = "风险点", Width = 30, Alignment="left" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "districtname", ExcelColumn = "所属区域", Width = 25 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "grade", ExcelColumn = "风险等级", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "fl", ExcelColumn = "管控层级", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "unit", ExcelColumn = "管控责任单位", Width = 25 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "man", ExcelColumn = "责任人", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "num", ExcelColumn = "辨识数量", Width = 10 });
                ExcelHelper.ExcelDownload(data, excelconfig);
                return Success("导出成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        [HttpPost]
        public string GetOptionsStringForArea(string deptCode)
        {
            return dangerBLL.GetOptionsStringForArea("0", deptCode);
        }
        #endregion
    }
}
