using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.BaseManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Cache;
using System.Collections.Generic;
using ERCHTMS.Code;
using System.Linq;
using ERCHTMS.Busines.RiskDatabase;
using BSFramework.Util.Extension;
using System.Data;
using System.Text;
using System;
using System.Web;
using BSFramework.Util.Offices;

namespace ERCHTMS.Web.Areas.BaseManage.Controllers
{
    /// <summary>
    /// 描 述：区域设置
    /// </summary>
    public class DistrictController : MvcControllerBase
    {
        private DistrictBLL bis_districtbll = new DistrictBLL();
        private UserBLL userBLL = new UserBLL();
        private DepartmentBLL deptBLL = new DepartmentBLL();
        private OrganizeCache organizeCache = new OrganizeCache();
        private DangerSourceBLL dsBLL = new DangerSourceBLL();
        int lft = 1, rgt = 0;
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
        /// 生成二维码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult BuilderImage()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Select()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }
        [HttpGet]
        public ActionResult   MulSelect()
        {
            return View();
        }
        /// <summary>
        /// 标注位置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Mark()
        {
            string path = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("imgUrl");
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            //获取电厂区域图
            DataTable dtFiles = new ERCHTMS.Busines.PublicInfoManage.FileInfoBLL().GetFiles(user.OrganizeId);
            if (dtFiles.Rows.Count > 0)
            {
                string imgUrl = dtFiles.Rows[0]["filepath"].ToString();
                imgUrl = imgUrl.Replace("~", "");
                imgUrl = path + imgUrl;
                ViewBag.MapImage = imgUrl;
            }
            else
            {
                ViewBag.MapImage = "";
            }
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
        public ActionResult GetListJson()
        {
            var data = bis_districtbll.GetList();
            return ToJsonResult(data);
        }


        [HttpGet]
        public ActionResult GetListJsonByCode(string keyValue)
        {
            var data = bis_districtbll.GetList().Where(p => p.DistrictCode == keyValue).FirstOrDefault();
            return ToJsonResult(data);
        }



        [HttpGet]
        public ActionResult GetListJsonByHidCode(string keyValue) 
        {
            var data = bis_districtbll.GetList().Where(p => p.DistrictCode == keyValue).FirstOrDefault();
            var userEntity = userBLL.GetList().Where(p => p.Account == data.DisreictChargePersonID).FirstOrDefault();
            data.DisreictChargePersonID = userEntity.UserId;
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetDept(string userId)
        {
            UserEntity ue = userBLL.GetEntity(userId);
            DepartmentEntity de = deptBLL.GetEntity(ue.DepartmentId);
            if (de == null)//选择机构人员
            {
                OrganizeEntity org = organizeCache.GetEntity(ue.OrganizeId);
                de = new DepartmentEntity();
                de.DepartmentId = org.OrganizeId;
                de.EnCode = org.EnCode;
                de.FullName = org.FullName;
                de.Manager = org.Manager;
                de.ManagerId = org.ManagerId;
            }
            return Content(de.ToJson());
        }
        /// <summary>
        /// 获取部门管控区域名称集合
        /// </summary>
        /// <param name="deptId">部门Id</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetDeptNames(string deptId)
        {
            DataTable de = bis_districtbll.GetDeptNames(deptId);
            
            return Content(de.ToJson());
        }
        /// <summary>
        /// 获取单个区域
        /// </summary>
        /// <param name="id">区域ID</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetDistric(string id)
        {
            var data = bis_districtbll.GetEntity(id);
            return Content(data.ToJson());
        }
        /// <summary>
        /// 获取名称和ID
        /// </summary>
        /// <param name="ids">id集合</param>
        /// <returns>返回列表</returns>
        [HttpGet]
        public ActionResult GetNameAndID(string ids)
        {
            var data = bis_districtbll.GetNameAndID(ids);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 区域列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>   
        [HttpGet]
        public ActionResult GetPageListJson(string condition, string keyword)
        {
            
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
         
            List<DistrictEntity> districtdata = new List<DistrictEntity>();
            if (user.IsSystem)
            {
                districtdata = bis_districtbll.GetList().OrderBy(a => a.SortCode).ToList();
            }
            else
            {
                districtdata = bis_districtbll.GetList().Where(x=>x.OrganizeId==user.OrganizeId).OrderBy(a => a.SortCode).ToList();
            }
            if (!string.IsNullOrEmpty(condition) && !string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.Trim();
                #region 多条件查询
                switch (condition)
                {
                    case "DistrictName":    //名称
                        districtdata = districtdata.TreeWhere(a => a.DistrictName.Contains(keyword), "DistrictID", "ParentID");
                        break;
                    case "DistrictCode":      //编号
                        districtdata = districtdata.TreeWhere(a => a.DistrictCode.Contains(keyword), "DistrictID", "ParentID");
                        break;
                    default:
                        break;
                }
                #endregion
            }
            List<TreeGridEntity> treeList = new List<TreeGridEntity>();
            foreach (DistrictEntity item in districtdata)
            {
                item.LinkMail = item.DistrictName;
                TreeGridEntity tree = new TreeGridEntity();
                bool hasChildren = districtdata.Count(t => t.ParentID == item.DistrictID) == 0 ? false : true;
                tree.id = item.DistrictID;
                tree.parentId = item.ParentID;
                tree.expanded = false;
                tree.hasChildren = hasChildren;     
                string itemJson = item.ToJson();
                tree.entityJson = itemJson;
                treeList.Add(tree);
            }
            return Content(treeList.TreeJson("0"));

        }
        
        [HttpGet]
        public ActionResult GetCurListJson(string areaId="",string mode="",string planId="",string dataType="",string objId = "",string areaName="")
        {
            try
            {
                var curUser = new OperatorProvider().Current();
                List<DistrictEntity> list = new List<DistrictEntity>();
                if (!string.IsNullOrEmpty(objId))
                {
                    list = bis_districtbll.GetOrgList(objId).ToList();
                }
                else
                {
                    list = bis_districtbll.GetList().ToList();
                }
                if (!string.IsNullOrWhiteSpace(areaName))
                {
                    list = list.Where(t => t.DistrictName.Contains(areaName.Trim())).ToList();
                }
                List<DistrictEntity> allList = bis_districtbll.GetOrgList(curUser.OrganizeId).ToList();
                List<DistrictEntity> districtdata = list;
                if (!curUser.IsSystem && string.IsNullOrEmpty(objId))
                {
                    districtdata = districtdata.Where(p => p.OrganizeId == curUser.OrganizeId).ToList();
                }

                if (string.IsNullOrEmpty(areaId))
                {
                    if (mode == "1")
                    {
                        RiskPlanBLL plan = new RiskPlanBLL();
                        areaId = plan.GetPlanAreaIds(0, planId);
                        districtdata = districtdata.Where(t => !areaId.Contains(t.DistrictID) && t.DistrictID != "0").ToList();

                        List<DistrictEntity> list1 = districtdata.Where(t => t.DistrictCode.Length > 6).ToList();
                        foreach (DistrictEntity entity in list1)
                        {
                            if (entity.DistrictCode.Length >= 6)
                            {
                                string code = entity.DistrictCode.Substring(0, 6);
                                var d = districtdata.Where(t => t.DistrictCode == code);
                                if (districtdata.Where(t => t.DistrictCode == code).Count() == 0)
                                {
                                    DistrictEntity de = list.Where(t => t.DistrictCode == code).FirstOrDefault();
                                    if (!districtdata.Contains(de))
                                    {
                                        districtdata.Add(de);
                                    }
                                }
                            }
                        }
                    }
                    //else
                    //{
                    //    districtdata = districtdata.Where(t => areaId.Contains(t.DistrictID) && t.DistrictID != "0").ToList();
                    //}
                    List<TreeGridEntity> treeList = new List<TreeGridEntity>();

                    string parentId = "0";
                    if (!string.IsNullOrWhiteSpace(areaName) && districtdata.Count > 0)
                    {

                        districtdata = GetParentId(districtdata, allList);
                        // parentId = districtdata[0].ParentID;
                    }
                    districtdata = districtdata.OrderBy(a => a.DistrictCode).ThenBy(a => a.SortCode).ToList();
                    foreach (DistrictEntity item in districtdata)
                    {
                        TreeGridEntity tree = new TreeGridEntity();
                        int count = districtdata.Count(t => t.DistrictCode.StartsWith(item.DistrictCode));
                        int count1 = districtdata.Count(t => t.DistrictCode.StartsWith(item.DistrictCode) && t.DistrictCode != item.DistrictCode);
                        bool hasChildren = count1 == 0 ? false : true;
                        tree.id = item.DistrictID;
                        tree.parentId = item.ParentID;
                        tree.expanded = false;
                        tree.hasChildren = hasChildren;
                        tree.childCount = count;
                        tree.code = item.DistrictCode;
                        string itemJson = item.ToJson();
                        tree.entityJson = itemJson;
                        treeList.Add(tree);
                    }
                    return Content(treeList.TreeJson(parentId));
                }
                else
                {

                    if (mode == "1")
                    {
                        RiskPlanBLL plan = new RiskPlanBLL();
                        string ids = plan.GetPlanAreaIds(0, planId);
                        if (!string.IsNullOrEmpty(ids) && dataType.Equals("0"))
                        {
                            areaId = ids;
                            districtdata = districtdata.Where(t => !areaId.Contains(t.DistrictID) && t.DistrictID != "0").ToList();
                        }
                        else
                        {
                            if (dataType == "0")
                            {
                                districtdata = districtdata.Where(t => t.DistrictID != "0").ToList();
                            }
                            else
                            {
                                districtdata = districtdata.Where(t => areaId.Contains(t.DistrictID) && t.DistrictID != "0").ToList();
                            }

                        }
                        List<DistrictEntity> list1 = districtdata.Where(t => t.DistrictCode.Length > 6).ToList();
                        foreach (DistrictEntity entity in list1)
                        {
                            if (entity.DistrictCode.Length >= 6)
                            {
                                string code = entity.DistrictCode.Substring(0, 6);
                                if (districtdata.Where(t => t.DistrictCode == code).Count() == 0)
                                {
                                    DistrictEntity de = list.Where(t => t.DistrictCode == code).FirstOrDefault();
                                    if (!districtdata.Contains(de))
                                    {
                                        districtdata.Add(de);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        districtdata = districtdata.Where(t => areaId.Contains(t.DistrictID) && t.DistrictID != "0").ToList();
                    }
                    districtdata = districtdata.OrderBy(a => a.DistrictCode).ThenBy(a => a.SortCode).ToList();

                    var JsonData = new
                    {
                        rows = districtdata,
                        total = 1,
                        page = 1,
                        records = districtdata.Count
                    };
                    return Content(JsonData.ToJson());

                }
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
            
            

        }
        //找到集合中最上级父节点的id
        public List<DistrictEntity> GetParentId(IEnumerable<DistrictEntity> data, IEnumerable<DistrictEntity> alldata)
        {
            string id = "";
            List<DistrictEntity> newdata = new List<DistrictEntity>();
            if (data.Count() > 0)
            {
                newdata = data.ToList();

                for (int i = 0; i < newdata.Count; i++)
                {
                    id = newdata[i].ParentID;
                    //如果自己表里面没有父级 而查询前的表里面有则加入到表中
                    if (newdata.Where(it => it.DistrictID == id).Count() == 0 && alldata.Where(it => it.DistrictID == id).Count() > 0)
                    {

                        newdata.Add(alldata.Where(it => it.DistrictID == id).FirstOrDefault());
                    }

                }


            }

            return newdata;

        }
        /// <summary>
        /// 供辨识评估计划时调用
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="mode"></param>
        /// <param name="planId"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAreaListJson(string areaId = "", string mode = "", string planId = "", string dataType = "",string areaName="")
        {
            DangerSourceBLL dsBll = new DangerSourceBLL();
            var curUser = new OperatorProvider().Current();
            List<DistrictEntity> list = bis_districtbll.GetList().Where(p => p.OrganizeId==curUser.OrganizeId).ToList();
            List<DistrictEntity> districtdata =list.Where(p => p.DistrictCode.Contains(curUser.OrganizeCode) && p.DistrictID != "0").ToList();
          
            RiskPlanBLL plan = new RiskPlanBLL();
            string ids = plan.GetPlanAreaIds(0, planId);
            if (dataType.Equals("0") && string.IsNullOrEmpty(ids))
            {
                 areaId = "-1";
            }
            if (dataType.Equals("0"))
            {
                districtdata = districtdata.Where(t => !ids.Contains(t.DistrictID)).ToList();
            }
            else
            {
                  districtdata = districtdata.Where(t => areaId.Contains(t.DistrictID)).ToList();
            }
            List<DistrictEntity> list1 = new List<DistrictEntity>();
            foreach (DistrictEntity item in districtdata)
            {
                bool hasChildren = list.Count(t => t.ParentID == item.DistrictID) == 0 ? false : true;
                    if (!hasChildren)
                    {
                        item.Description = item.DistrictName;
                        item.DistrictName = dsBll.GetPathName(item.DistrictCode,curUser.OrganizeId);
                        list1.Add(item);
                    }
                    else
                    {
                        item.Description = item.DistrictName;
                        list1.Add(item);
                    }
             }
            if (!string.IsNullOrWhiteSpace(areaName))
            {
                list1 = list1.Where(t => t.DistrictName.Contains(areaName.Trim())).ToList();
            }
             var JsonData = new
                {
                    rows = list1.OrderBy(t=>t.DistrictCode).ThenBy(t=>t.SortCode).ToList(),
                    total = 1,
                    page = 1,
                    records = districtdata.Count
                };
                return Content(JsonData.ToJson());
        }
        /// <summary>
        /// 区域树列表
        /// </summary>
        /// <returns>区域树列表</returns>  
        public ActionResult GetTreeJson(string orgID = "0", string areaIds = "", string keyword = "",string checkAreaids="")
        {
            List<DistrictEntity> list = bis_districtbll.GetList().ToList() ;
            List<DistrictEntity> districtdata = new List<DistrictEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                list = list.Where(t => t.DistrictName.Contains(keyword.Trim())).ToList();
            }
            districtdata = list.OrderBy(a => a.SortCode).ToList();
            if (orgID != "0")
            {
                districtdata = districtdata.Where(a => a.OrganizeId == orgID).ToList();
            }
            if (!string.IsNullOrEmpty(areaIds))
            {
                districtdata = districtdata.Where(t => areaIds.Contains(t.DistrictID)).ToList();
            }
            List<DistrictEntity> list1 = new List<DistrictEntity>();
            foreach (DistrictEntity entity in districtdata)
            {
                string code = string.Empty;
                if (entity.DistrictCode.Length > 5)
                {
                    code = entity.DistrictCode.Substring(0, 6);
                }
                else 
                {
                    code = entity.DistrictCode.Substring(0, 3);
                }
                if (districtdata.Where(t => t.DistrictCode == code).Count() == 0)
                {
                    DistrictEntity de = list.Where(t => t.DistrictCode == code).FirstOrDefault();
                    if (de!=null)
                    {
                      if (!list1.Contains(de))
                      {
                        list1.Add(de);
                      }
                    }
                }
            }
            districtdata=districtdata.Concat(list1).ToList();
            districtdata = districtdata.OrderBy(t => t.DistrictCode).ThenBy(t => t.SortCode).ToList();
            List<TreeEntity> treeList = new List<TreeEntity>();
            foreach (DistrictEntity item in districtdata)
            {
                int chkState = 0;
                //设置部门默认选中状态
                if (!string.IsNullOrEmpty(checkAreaids))
                {
                    string[] arrids = checkAreaids.Split(',');
                    if (arrids.Contains(item.DistrictID))
                    {
                        chkState = 1;
                    }
                }
                TreeEntity tree = new TreeEntity();
                bool hasChildren = districtdata.Count(t => t.ParentID == item.DistrictID) == 0 ? false : true;
                tree.id = item.DistrictID;
                tree.text = item.DistrictName.Replace("\\", "╲");
                tree.value = item.DistrictCode;
                tree.Attribute ="Code";
                tree.AttributeValue = item.DistrictCode;
                tree.AttributeA = "Dept";
                tree.AttributeValueA = item.ChargeDept + ","+item.ChargeDeptCode+","+item.ChargeDeptID;
                tree.parentId =string.IsNullOrEmpty(keyword)? item.ParentID:"0";
                tree.isexpand = false;
                tree.complete = true;
                tree.hasChildren =string.IsNullOrEmpty(keyword)? hasChildren:false;
                tree.showcheck = true;
               
                tree.checkstate = chkState;
                treeList.Add(tree);
            }
            return Content(treeList.TreeToJson("0"));
        }
        [HttpGet]
        public ActionResult GetAreasJson(string orgID, string areaIds,string planId)
        {
            List<DistrictEntity> list = bis_districtbll.GetList().ToList();
            List<DistrictEntity> districtdata = new List<DistrictEntity>();
            districtdata = list.OrderBy(a => a.SortCode).ToList();
            if (orgID != "0")
            {
                districtdata = districtdata.Where(a => a.OrganizeId == orgID).ToList();
            }
            if (!string.IsNullOrEmpty(planId))
            {
                 RiskPlanBLL riskplanbll = new RiskPlanBLL();
                 string data = riskplanbll.GetCurrUserAreaId(planId, OperatorProvider.Provider.Current().Account);
                 districtdata = districtdata.Where(t => data.Contains(t.DistrictID)).ToList();
            }
            districtdata = districtdata.OrderBy(t => t.DistrictCode).ThenBy(t => t.SortCode).ToList();
            List<TreeEntity> treeList = new List<TreeEntity>();
            foreach (DistrictEntity item in districtdata)
            {
                TreeEntity tree = new TreeEntity();
                bool hasChildren = districtdata.Count(t => t.ParentID == item.DistrictID) == 0 ? false : true;
                tree.id = item.DistrictID;
                tree.text = item.DistrictName.Replace("\\", "╲");
                tree.value = item.DistrictCode;
                tree.Attribute = "Code";
                tree.AttributeValue = item.DistrictCode;
                tree.AttributeA = "Dept";
                tree.AttributeValueA = item.ChargeDept + "," + item.ChargeDeptCode + "," + item.ChargeDeptID;
                tree.parentId = "0";
                tree.isexpand = false;
                tree.complete = true;
                tree.hasChildren = hasChildren;
                tree.showcheck = true;
                treeList.Add(tree);
            }
            return Content(treeList.TreeToJson("0"));

        }
        /// <summary>
        /// 获取数据字典列表（绑定控件,返回机构）
        /// </summary>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetDataItemListJson()
        {
            OrganizeEntity parent = organizeCache.GetList().Where(a => a.OrganizeId == "0").FirstOrDefault();
            var data = organizeCache.GetList().Where(a => a.ParentId == parent.OrganizeId);
            return Content(data.ToJson());
        }
        /// <summary>
        /// 获取区域绑定控件(返回区域)
        /// </summary>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetDistricListJson(string orgID = "")
        {
            var districtdata = bis_districtbll.GetList(orgID);
            return Content(districtdata.ToJson());
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = bis_districtbll.GetEntity(keyValue);
            return Content(data.ToJson());
        }

        #region 获取区域树菜单
        /// <summary>
        /// 获取机构部门组织树菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDistrictTreeJson()
        {
            //string deptID= OperatorProvider.Provider.Current().DepartmentId;//获取部门ID
            //获取电厂(组织)
            var organizedata = organizeCache.GetList();
            var districtdata = bis_districtbll.GetList();
            List<TreeEntity> treeList = new List<TreeEntity>();
            string pid = "0";
            foreach (OrganizeEntity item in organizedata)
            {
                #region 机构
                if (item.ParentId == "0")
                {
                    pid = item.OrganizeId;
                    continue;
                }
                TreeEntity tree = new TreeEntity();
                bool hasChildren = organizedata.Count(t => t.ParentId == item.OrganizeId) == 0 ? false : true;
                if (hasChildren == false)
                {
                    hasChildren = districtdata.Count(t => t.ParentID == item.OrganizeId) == 0 ? false : true;
                }
                tree.id = item.OrganizeId;
                tree.text = item.FullName;
                tree.value = item.OrganizeId;
                tree.parentId = item.ParentId;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChildren;
                tree.AttributeA = "EnCode";
                tree.AttributeValueA = item.OrganizeId;
                treeList.Add(tree);
                #endregion
            }
            foreach (DistrictEntity item in districtdata)
            {
                #region 区域
                TreeEntity tree = new TreeEntity();
                bool hasChildren = districtdata.Count(t => t.ParentID == item.DistrictID) == 0 ? false : true;

                tree.id = item.DistrictID;
                tree.text = item.DistrictName;
                tree.value = item.DistrictID;
                tree.parentId = item.ParentID;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChildren;
                tree.AttributeA = "EnCode";
                tree.AttributeValueA = item.DistrictID;
                treeList.Add(tree);
                #endregion
            }

            return Content(treeList.TreeToJson(pid));
        }
        #endregion
        #endregion

        /// <summary>
        /// 根据管控部门获取区域
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDistrictByProject(string projectId)
        {
            var data = bis_districtbll.GetList().Where(p => p.ChargeDeptID == projectId);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 根据区域IDd获取区域
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDistrictByID(string ID)
        {
            var data = bis_districtbll.GetList().Where(p => p.DistrictID == ID).FirstOrDefault();
            return ToJsonResult(data);
        }
        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(6, "删除区域信息")]
        public ActionResult RemoveForm(string keyValue)
        {
            bis_districtbll.RemoveForm(keyValue);
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
        [HandlerMonitor(5, "新增或者修改区域信息")]
        public ActionResult SaveForm(string keyValue, DistrictEntity entity)
        {
            try
            {
                if (entity.LinkToCompany == null)
                {
                    entity.LinkToCompany = "";
                }
                if (entity.LinkToCompanyID == null)
                {
                    entity.LinkToCompanyID = "";
                }
                entity.DistrictName = entity.DistrictName.Replace("\\", "╲");
                bis_districtbll.SaveForm(keyValue, entity);
                if (!string.IsNullOrEmpty(entity.LinkToCompanyID))
                {
                    dsBLL.Update(entity.DistrictID, entity.LinkToCompanyID.TrimEnd(','), entity.DistrictCode, entity.DistrictName, entity.ChargeDeptCode, OperatorProvider.Provider.Current());
                }
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
           
        }

        /// <summary>
        /// 根据区域坐标位置绘制图形并获取区域相关统计信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAreaMapListJson(string id="")
        {
            try
            {
                var deptBll = new DepartmentBLL();
                var curUser = new OperatorProvider().Current();
                var expression = LinqExtensions.True<DistrictEntity>();
                expression = expression.And(t => t.OrganizeId == curUser.OrganizeId);
                var districtdata = bis_districtbll.GetListForCon(expression).Where(t => !string.IsNullOrWhiteSpace(t.LatLng)).ToList();
                if (!string.IsNullOrWhiteSpace(id))
                {
                    districtdata = districtdata.Where(t => !t.DistrictID.Equals(id)).ToList();
                }
                DataTable dt = new DataTable();
                dt.Columns.Add("code");
                dt.Columns.Add("status");
                dt.Columns.Add("htnum");
                dt.Columns.Add("fxnum");
                dt.Columns.Add("areacode");
                dt.Columns.Add("wxnum");
                dt.Columns.Add("content");
                List<object> list = new List<object>();
                StringBuilder sb = new StringBuilder();
                foreach (DistrictEntity area in districtdata)
                {
                    int val = 0;
                    string htNum = "";
                    string fxNum = "";
                    string areaCode = "";

                    areaCode = area.DistrictCode;
                    string sql = "";
                    DataTable obj = deptBll.GetDataTable(string.Format("select min(gradeval) from BIS_RISKASSESS t where status=1 and deletemark=0 and t.areacode like '{0}%' {1}", areaCode, sql));
                    if (obj.Rows.Count > 0)
                    {
                        val = obj.Rows[0][0].ToInt();
                    }
                    else
                    {
                        val = 0;
                    }
                    //隐患数量
                    DataTable dtHt = deptBll.GetDataTable(string.Format("select rankname,count(1) num from v_basehiddeninfo t where t.workstream!='整改结束' and t.hidpoint like '{0}%' group by rankname", areaCode));
                    if (dtHt.Rows.Count > 0)
                    {
                        var rows = dtHt.Select("rankname='一般隐患'");
                        if (rows.Length > 0)
                        {
                            htNum = rows[0][1].ToString();
                        }
                        else
                        {
                            htNum = "0";
                        }
                        rows = dtHt.Select("rankname='重大隐患'");
                        if (rows.Length > 0)
                        {
                            htNum += "," + rows[0][1].ToString();
                        }
                        else
                        {
                            htNum += ",0";
                        }
                    }
                    sb.Clear();
                    //风险数量
                    DataTable dtRisk = deptBll.GetDataTable(string.Format(@"select nvl(num,0) from (select 1 gradeval from dual union all select 2 gradeval from dual union all select 3 gradeval from dual union all select 4 gradeval from dual) a
left join (select gradeval,count(1) num from BIS_RISKASSESS t where status=1 and deletemark=0 and t.areacode like '{0}%' {1} group by grade,gradeval) b
on a.gradeval=b.gradeval order by a.gradeval asc", areaCode, sql));
                    foreach (DataRow dr in dtRisk.Rows)
                    {
                        sb.AppendFormat("{0},", dr[0].ToString());
                    }
                    //重大危险源数量
                    int count = deptBll.GetDataTable(string.Format("select count(1) from HSD_HAZARDSOURCE t where IsDanger=1 and gradeval>0 and deptcode like '{0}%' and t.districtid in(select districtid from bis_district d where d.districtcode like '{1}%')", curUser.OrganizeCode, areaCode)).Rows[0][0].ToInt();
                    fxNum = sb.ToString().TrimEnd(',');
                    list.Add(new
                    {
                        Code = area.DistrictCode,
                        LatLng = area.LatLng,
                        DistrictName = area.DistrictName,
                        Status = val,
                        htnum = htNum,
                        fxnum = fxNum,
                        wxnum = count,
                    });
                }
                return Content(list.ToJson());
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        /// <summary>
        /// 清除区域坐标
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ClearMark(string keyValue)
        {
            try
            {
                string sql = "update BIS_DISTRICT set latlng=''";
                if (!string.IsNullOrWhiteSpace(keyValue))
                {
                    sql += string.Format(" where districtid='{0}'", keyValue);
                }
                new DepartmentBLL().ExecuteSql(sql);
                return Success("操作成功");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        /// <summary>
        /// 导入区域
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportData(string OrganizeId,string orgName)
        {
            try
            {
                int error = 0;
                string orgId = OperatorProvider.Provider.Current().OrganizeId;
                if (!string.IsNullOrEmpty(OrganizeId))
                {
                    orgId = OrganizeId;
                }
                string message = "请选择格式正确的文件再导入!";
                string falseMessage = "";
                int count = HttpContext.Request.Files.Count;
                if (count > 0)
                {
                    HttpPostedFileBase file = HttpContext.Request.Files[0];
                    if (string.IsNullOrEmpty(file.FileName))
                    {
                        return message;
                    }
                    if (!(file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xls") || file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xlsx")))
                    {
                        return message;
                    }
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName));
                    Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                    wb.Open(Server.MapPath("~/Resource/temp/" + fileName));
                    Aspose.Cells.Cells cells = wb.Worksheets[0].Cells;
                    DataTable dt = cells.ExportDataTable(0, 0, cells.MaxDataRow+1, cells.MaxColumn + 1, true);
                    //DataTable dt = ExcelHelper.ExcelImport(Server.MapPath("~/Resource/temp/" + fileName));
                    int order = 1;
                    var deptBll = new DepartmentBLL();
                    for (int i = 1; i < dt.Rows.Count; i++)
                    {
                        order = i;
                        //区域名称
                        string areaName = dt.Rows[i][0].ToString().Trim();
                        if (string.IsNullOrWhiteSpace(areaName))
                        {
                            falseMessage += "</br>" + "第" + (i + 2) + "行区域名称为空,未能导入.";
                            error++;
                            continue;
                        }
                        //上级区域名称
                        string parentName = dt.Rows[i][1].ToString().Trim();
                        //管控部门名称
                        string deptName = dt.Rows[i][2].ToString().Trim();
                        if (string.IsNullOrWhiteSpace(deptName))
                        {
                            falseMessage += "</br>" + "第" + (i + 2) + "行管控部门名称为空,未能导入.";
                            error++;
                            continue;
                        }
                        //区域负责人
                        string dutyUser = dt.Rows[i][3].ToString().Trim();
                        if (string.IsNullOrWhiteSpace(dutyUser))
                        {
                            falseMessage += "</br>" + "第" + (i + 2) + "行区域负责人名称为空,未能导入.";
                            error++;
                            continue;
                        }
                        DistrictEntity area = new DistrictEntity();
                        area.DistrictName = areaName;
                        area.ChargeDept = deptName;
                        area.DisreictChargePerson = dutyUser;
                        area.OrganizeId = OrganizeId;
                        area.BelongCompany = orgName;
                        //验证上级区域名称并获取相关信息
                        if (!string.IsNullOrWhiteSpace(parentName))
                        {
                            DataTable dtArea = deptBll.GetDataTable(string.Format("select DistrictId from BIS_DISTRICT where DistrictName='{0}' and organizeid='{1}'", parentName, OrganizeId));
                            if (dtArea.Rows.Count > 0)
                            {
                                area.ParentID = dtArea.Rows[0][0].ToString();
                                dtArea = deptBll.GetDataTable(string.Format("select count(1) from BIS_DISTRICT where DistrictName='{0}' and organizeid='{1}' and parentid='{2}'", areaName, OrganizeId, area.ParentID));
                                if (dtArea.Rows[0][0].ToString() != "0")
                                {
                                    falseMessage += "</br>" + "第" + (i + 2) + "行区域信息已经存在,未能导入.已存在区域信息："+areaName;
                                    error++;
                                    continue;
                                }
                            }
                            else
                            {
                                falseMessage += "</br>" + "第" + (i + 2) + "行上级区域名称与系统区域不匹配,未能导入.错误的区域信息：" + parentName;
                                error++;
                                continue;
                            }
                        }
                        else
                        {
                            area.ParentID = "0";
                            DataTable dtArea = deptBll.GetDataTable(string.Format("select count(1) from BIS_DISTRICT where DistrictName='{0}' and organizeid='{1}' and parentid='0'", areaName, OrganizeId));
                            if (dtArea.Rows[0][0].ToString() != "0")
                            {
                                falseMessage += "</br>" + "第" + (i + 2) + "行区域信息已经存在,未能导入.错误的区域信息："+areaName;
                                error++;
                                continue;
                            }

                        }
                        //验证管控部门并获取相关信息
                        if (!string.IsNullOrWhiteSpace(deptName))
                        {
                            DataTable dtDept = deptBll.GetDataTable(string.Format("select encode,departmentid,MANAGER,MANAGERID,OuterPhone from BASE_DEPARTMENT where fullname='{0}' and organizeid='{1}'", deptName, OrganizeId));
                            if (dtDept.Rows.Count > 0)
                            {
                                area.ChargeDeptCode = dtDept.Rows[0][0].ToString();
                                area.ChargeDeptID = dtDept.Rows[0][1].ToString();
                                area.DeptChargePerson = dtDept.Rows[0][2].ToString();
                                area.DeptChargePersonID = dtDept.Rows[0][3].ToString();
                                area.LinkTel = dtDept.Rows[0][4].ToString();
                            }
                            else
                            {
                                falseMessage += "</br>" + "第" + (i + 2) + "行管控部门名称与系统部门名称不匹配,错误的部门信息："+deptName;
                                error++;
                                continue;
                            }
                        }
                        //验证区域负责人并获取相关信息
                        if (!string.IsNullOrWhiteSpace(dutyUser))
                        {
                            DataTable dtUsers = deptBll.GetDataTable(string.Format("select account from BASE_user where organizeid='{1}' and realname in('{0}') ", dutyUser.Replace(",", "','"), OrganizeId));
                            StringBuilder sb = new StringBuilder();
                            foreach (DataRow dr in dtUsers.Rows)
                            {
                                sb.AppendFormat("{0},", dr[0].ToString());
                            }
                            string users = sb.ToString().TrimEnd(',');
                            area.DisreictChargePersonID = sb.ToString().TrimEnd(',');
                            if (dutyUser.Split(',').Length != users.Split(',').Length)
                            {
                                falseMessage += "</br>" + "第" + (i + 2) + "行区域负责人信息与系统中人员不匹配，错误信息：" + dutyUser;
                                error++;
                            }

                        }
                        try
                        {
                            bis_districtbll.SaveForm("", area);
                        }
                        catch (Exception ex)
                        {
                            falseMessage += "</br>" + "第" + (i + 2) + "行数据插入出现异常，错误信息：" + Newtonsoft.Json.JsonConvert.SerializeObject(ex);
                            error++;
                        }

                    }
                    count = dt.Rows.Count - 1;
                    message = "共有" + count + "条记录,成功导入" + (count - error) + "条，失败" + error + "条";
                    message += "</br>" + falseMessage;
                }

                return message;
            }
            catch(Exception ex)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(ex);
            }
           
        }
        #endregion

        #region 验证数据
        /// <summary>
        /// 验证所选择的是不是人员
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult IsPerson(string uid)
        {
            bool exsit = true;
            var person = userBLL.GetList().Where(a => a.UserId == uid);
            if (person.Count() == 0)
            {
                exsit = false;
            }
            return Content(exsit.ToJson());
        }
        #endregion
    }
}
