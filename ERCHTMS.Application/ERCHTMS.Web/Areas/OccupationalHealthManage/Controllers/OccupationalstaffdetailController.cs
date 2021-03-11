using ERCHTMS.Entity.OccupationalHealthManage;
using ERCHTMS.Busines.OccupationalHealthManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.SystemManage;
using System.Collections.Generic;
using ERCHTMS.Busines.BaseManage;
using System.Data;
using System;
using ERCHTMS.Code;
using System.Linq;
using ERCHTMS.Entity.BaseManage;
using BSFramework.Util.Extension;

namespace ERCHTMS.Web.Areas.OccupationalHealthManage.Controllers
{
    /// <summary>
    /// 描 述：职业病人详情表
    /// </summary>
    public class OccupationalstaffdetailController : MvcControllerBase
    {
        private OccupationalstaffdetailBLL occupationalstaffdetailbll = new OccupationalstaffdetailBLL();

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
        /// 选择职业病
        /// </summary>
        /// <returns></returns>
        public ActionResult Select()
        {
            return View();
        }
        /// <summary>
        /// 查看体检人员
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
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
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "OCCDETAILID";
            pagination.p_fields = "USERNAME,USERNAMEPINYIN,INSPECTIONTIME,ISSICK,SICKTYPE,SICKTYPENAME,UNUSUALNOTE";//注：此处要替换成需要查询的列
            pagination.p_tablename = "BIS_OCCUPATIONALSTAFFDETAIL";
            pagination.conditionJson = "1=1";
            pagination.sidx = "INSPECTIONTIME";

            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                string where = new ERCHTMS.Busines.AuthorizeManage.AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
                pagination.conditionJson += " and " + where;
            }

            var data = occupationalstaffdetailbll.GetPageListByProc(pagination, queryJson);
            var jsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return ToJsonResult(jsonData);

            //var data = occupationalstaffdetailbll.GetList(queryJson);
            //return ToJsonResult(data);
        }

        /// <summary>
        /// 根据用户id查询体检记录
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetUserList(string UserId)
        {
            var data = occupationalstaffdetailbll.GetUserTable(UserId);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 根据用户id查询体检记录
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NewGetUserList(string UserId)
        {
            var data = occupationalstaffdetailbll.NewGetUserTable(UserId);
            return ToJsonResult(data);
        }


        /// <summary>
        /// 获取是否是厂领导 或者是配置的权限
        /// </summary>
        /// <returns></returns>
        public string GetPer()
        {

            return occupationalstaffdetailbll.IsPer().ToString();
        }

        /// <summary>
        /// 获取用户的接触危害因素
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetUserHazardfactor(string UserId)
        {
            UserEntity user = new UserBLL().GetEntity(UserId);
            DataTable data = new DataTable();
            //没有权限则显示空数据
            if (occupationalstaffdetailbll.IsPer() || UserId == OperatorProvider.Provider.Current().UserId || (OperatorProvider.Provider.Current().RoleName.Contains("负责人") && OperatorProvider.Provider.Current().RoleName.Contains("部门级用户")))
            {
                data = occupationalstaffdetailbll.GetUserHazardfactor(user.Account);
            }

            return ToJsonResult(data);
        }

        public ActionResult GetList(string Pid, int type)
        {
            var data = occupationalstaffdetailbll.GetList(Pid, type);
            return Content(data.ToJson());
        }

        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = occupationalstaffdetailbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取字典值
        /// </summary>
        /// <param name="Code">数据字典值</param>
        /// <returns></returns>
        public ActionResult GetCmbJson(string Code)
        {
            DataItemBLL di = new DataItemBLL();
            //先获取到字典项
            DataItemEntity DataItems = di.GetEntityByCode(Code);

            DataItemDetailBLL did = new DataItemDetailBLL();
            //根据字典项获取值
            IEnumerable<DataItemDetailEntity> didList = did.GetList(DataItems.ItemId);

            return Content(didList.ToJson());
        }

        //[HttpGet]
        //public ActionResult GetTreeJson(string Code)
        //{
        //    DataItemBLL di = new DataItemBLL();
        //    //先获取到字典项
        //    DataItemEntity DataItems = di.GetEntityByCode(Code);

        //    DataItemDetailBLL did = new DataItemDetailBLL();
        //    //根据字典项获取值
        //    IEnumerable<DataItemDetailEntity> didList = did.GetList(DataItems.ItemId);


        //    var treeList = new List<TreeEntity>();
        //    foreach (DataItemDetailEntity item in didList)
        //    {
        //        TreeEntity tree = new TreeEntity();
        //        bool hasChildren = didList.Count(t => t.ParentId == item.DepartmentId) == 0 ? false : true;
        //        tree.id = item.DepartmentId;
        //        tree.text = item.FullName;
        //        tree.value = item.DepartmentId;
        //        tree.isexpand = true;
        //        tree.complete = true;
        //        tree.hasChildren = hasChildren;
        //        tree.parentId = item.ParentId;
        //        tree.Attribute = "Code";
        //        tree.AttributeValue = item.EnCode;
        //        tree.AttributeA = "IsOrg";
        //        tree.AttributeValueA = item.IsOrg.ToString();
        //        treeList.Add(tree);
        //    }
        //    return Content(treeList.TreeToJson(id));
        //}

        /// <summary>
        /// 根据查询条件获取职业病数据
        /// </summary>
        /// <param name="Code">职业病字典Code</param>
        /// <param name="deptIds">页面带过来的职业病ids</param>
        /// <param name="keyword">关键字</param>
        /// <param name="checkMode">单选或多选，0:单选，1:多选</param>
        /// <returns>返回树形Json</returns>
        [HttpGet]
        public ActionResult GetOccpuationalTreeJson(string Code, int checkMode = 0, int mode = 0, string deptIds = "0")
        {

            DataItemBLL di = new DataItemBLL();
            //先获取到字典项
            DataItemEntity DataItems = di.GetEntityByCode(Code);

            DataItemDetailBLL did = new DataItemDetailBLL();
            //根据字典项获取值
            List<DataItemDetailEntity> didList = did.GetList(DataItems.ItemId).OrderBy(t => t.ItemValue).ToList();


            string parentId = "0";
            OrganizeBLL orgBLL = new OrganizeBLL();
            var treeList = new List<TreeEntity>();

            //获取所有父节点集合
            List<DataItemDetailEntity> parentList = didList.Where(it => it.ItemValue.Length == 3).ToList();

            //获取所有子节点节点集合
            List<DataItemDetailEntity> SunList = didList.Where(it => it.ItemValue.Length > 3).ToList();

            foreach (DataItemDetailEntity oe in parentList)
            {
                treeList.Add(new TreeEntity
                {
                    id = oe.ItemValue,
                    text = oe.ItemName,
                    value = oe.ItemValue,
                    parentId = "0",
                    isexpand = true,
                    complete = true,
                    showcheck = false,
                    hasChildren = true,
                    Attribute = "Sort",
                    AttributeValue = "Parent",
                    AttributeA = "manager",
                    AttributeValueA = "" + "," + "" + ",1"
                });
            }

            foreach (DataItemDetailEntity item in SunList)
            {
                int chkState = 0;
                string[] arrids = deptIds.Split(',');
                if (arrids.Contains(item.ItemValue))
                {
                    chkState = 1;
                }
                TreeEntity tree = new TreeEntity();
                bool hasChildren = false;
                tree.id = item.ItemValue;
                tree.text = item.ItemName;
                tree.value = item.ItemValue;
                tree.isexpand = true;
                tree.complete = true;
                tree.checkstate = chkState;
                tree.showcheck = checkMode == 0 ? false : true;
                tree.hasChildren = hasChildren;
                tree.parentId = item.ItemValue.Substring(0, 3);
                tree.Attribute = "Sort";
                tree.AttributeValue = "Sun";
                tree.AttributeA = "manager";
                tree.AttributeValueA = "" + "," + "" + "," + "2";
                treeList.Add(tree);
            }
            return Content(treeList.TreeToJson());
        }

        /// <summary>
        /// 根据用id绑定下拉框
        /// </summary>
        /// <param name="UserIDs"></param>
        /// <returns></returns>
        public ActionResult GetUserJson(string UserIDs)
        {
            UserBLL ubll = new UserBLL();
            DataTable dt = ubll.GetUserTable(UserIDs.Split(','));
            //DataTable dt = new DataTable();
            return Content(dt.ToJson());
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            occupationalstaffdetailbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, OccupationalstaffdetailEntity entity)
        {
            //entity.Issick = 1;
            entity.InspectionTime = DateTime.Now;
            //将名字转化为拼音
            entity.UserNamePinYin = Str.PinYin(entity.UserName);
            occupationalstaffdetailbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion

        /// <summary>
        /// 导出到Excel
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult Excel(string queryJson)
        {
            string column = "IDNUM,OCCDETAILID,USERNAME,USERNAMEPINYIN,INSPECTIONTIME,ISSICK,SICKTYPE,ISENDEMIC,ISUNUSUAL,UNUSUALNOTE";
            string stringcolumn = "ISSICK,SICKTYPE";
            string[] columns = column.Split(',');
            string[] stringcolumns = stringcolumn.Split(',');

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

            DataTable dt = occupationalstaffdetailbll.GetTable(queryJson, wheresql);
            DataTable Newdt = AsposeExcelHelper.UpdateDataTable(dt, columns, stringcolumns);//把所有字段转换成string 方便修改

            //先获取职业病数据
            DataItemBLL di = new DataItemBLL();
            //先获取到字典项
            DataItemEntity DataItems = di.GetEntityByCode("SICKTYPE");
            DataItemDetailBLL did = new DataItemDetailBLL();
            //根据字典项获取值
            IEnumerable<DataItemDetailEntity> didList = did.GetList(DataItems.ItemId);

            //循环修改信息
            for (int i = 0; i < Newdt.Rows.Count; i++)
            {
                Newdt.Rows[i]["IDNUM"] = i + 1;
                Newdt.Rows[i]["INSPECTIONTIME"] = Convert.ToDateTime(Newdt.Rows[i]["INSPECTIONTIME"]).ToString("yyyy-MM-dd");
                if (Convert.ToInt32(Newdt.Rows[i]["ISSICK"]) == 1)
                {
                    Newdt.Rows[i]["ISSICK"] = "是";
                    foreach (DataItemDetailEntity item in didList)
                    {
                        if (item.ItemValue == Newdt.Rows[i]["SICKTYPE"].ToString())
                        {
                            Newdt.Rows[i]["SICKTYPE"] = item.ItemName;
                        }
                    }
                }
                else
                {
                    Newdt.Rows[i]["ISSICK"] = "否";
                    Newdt.Rows[i]["SICKTYPE"] = "";
                }
            }

            string FileUrl = "";

            var queryParam = queryJson.ToJObject();
            string keyord = queryParam["keyword"].ToString();
            if (keyord.ToInt() == 1) //查询职业病
            {
                FileUrl = @"\Resource\ExcelTemplate\职业健康职业病人员列表_导出模板.xlsx";
                AsposeExcelHelper.ExecuteResult(Newdt, FileUrl, "职业病人员列表", "职业病人员列表");
            }
            else if (keyord.ToInt() == 2)  //查询异常人员
            {
                FileUrl = @"\Resource\ExcelTemplate\职业健康异常人员列表_导出模板.xlsx";
                AsposeExcelHelper.ExecuteResult(Newdt, FileUrl, "异常人员列表", "异常人员列表");
            }
            else if (keyord.ToInt() == 3)  //查询体检人员
            {
                FileUrl = @"\Resource\ExcelTemplate\职业健康体检人员列表_导出模板.xlsx";
                AsposeExcelHelper.ExecuteResult(Newdt, FileUrl, "体检人员列表", "体检人员列表");
            }
            

           

            return Success("导出成功。");
        }
    }
}
