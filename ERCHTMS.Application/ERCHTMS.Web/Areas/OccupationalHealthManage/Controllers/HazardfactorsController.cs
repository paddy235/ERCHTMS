using ERCHTMS.Entity.OccupationalHealthManage;
using ERCHTMS.Busines.OccupationalHealthManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Data;
using ERCHTMS.Code;
using System.Web;
using System;
using BSFramework.Util.Offices;
using ERCHTMS.Busines.SystemManage;
using System.Collections.Generic;
using ERCHTMS.Entity.SystemManage;
using System.Text.RegularExpressions;
using System.Linq;
using System.Linq.Expressions;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;
using Newtonsoft.Json;

namespace ERCHTMS.Web.Areas.OccupationalHealthManage.Controllers
{
    /// <summary>
    /// 描 述：危险因素清单
    /// </summary>
    public class HazardfactorsController : MvcControllerBase
    {
        private HazardfactorsBLL hazardfactorsbll = new HazardfactorsBLL();

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
        /// 导入页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Import()
        {
            return View();
        }

        /// <summary>
        /// 选择树页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Select()
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
            pagination.p_kid = "HID";
            pagination.p_fields = "AREAID,AREAVALUE,RISKID,RISKVALUE,CONTACTNUMBER";//注：此处要替换成需要查询的列
            pagination.p_tablename = "BIS_HAZARDFACTORS";
            pagination.conditionJson = "1=1";
            pagination.sidx = "CREATEDATE";

            ERCHTMS.Code.Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                string where = new ERCHTMS.Busines.AuthorizeManage.AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
                pagination.conditionJson += " and " + where;
            }

            var data = hazardfactorsbll.GetPageListByProc(pagination, queryJson);
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
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = hazardfactorsbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetList()
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

            var data = hazardfactorsbll.GetList("", wheresql);
            return ToJsonResult(data);
        }


        /// <summary>
        /// 根据查询条件获取职业病数据
        /// </summary>
        /// <param name="Code">职业病字典Code</param>
        /// <param name="deptIds">页面带过来的职业病ids</param>
        /// <param name="keyword">关键字</param>
        /// <param name="checkMode">单选或多选，0:单选，1:多选</param>
        /// <returns>返回树形Json</returns>
        [HttpGet]
        public ActionResult GetOccpuationalTreeJson(string Code, string keyword, int checkMode = 0, int mode = 0, string deptIds = "0")
        {

            DataItemBLL di = new DataItemBLL();
            //先获取到字典项
            DataItemEntity DataItems = di.GetEntityByCode(Code);

            DataItemDetailBLL did = new DataItemDetailBLL();
            //根据字典项获取值
            List<DataItemDetailEntity> didList = did.GetList(DataItems.ItemId).OrderBy(t => t.ItemValue).ToList();



            //didList = didList.Where(it => it.ParentId == "0").ToList();//这里主要是由于开发时缓存 多做了一遍筛选

            var treeList = new List<TreeEntity>();

            //获取所有父节点集合
            List<DataItemDetailEntity> parentList = didList.Where(it => it.ItemValue.Length == 2).ToList();

            //获取所有子节点节点集合
            List<DataItemDetailEntity> SunList = didList.Where(it => it.ItemValue.Length > 2).ToList();

            if (!string.IsNullOrEmpty(keyword))
            {
                SunList = SunList.Where(t => t.ItemName.Contains(keyword)).ToList();
            }

            //先绑定父级树
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

            //再绑定子集树
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
                tree.parentId = item.ItemValue.Substring(0, 2);
                tree.Attribute = "Sort";
                tree.AttributeValue = "Sun";
                tree.AttributeA = "manager";
                tree.AttributeValueA = "" + "," + "" + "," + "2";
                treeList.Add(tree);
            }
            return Content(treeList.TreeToJson());
        }

        /// <summary>
        /// 输出危险因素下拉数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetRiskCmbList()
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

            //先获取所有数据
            DataTable hlist = hazardfactorsbll.GetList("", wheresql);
            List<ComboxEntity> Rlist = new List<ComboxEntity>();
            List<string> conStr = new List<string>();//用于判断重复的集合
            foreach (DataRow item in hlist.Rows)
            {
                string rv = item["RISKVALUE"].ToString();
                string rid = item["RISKID"].ToString();


                ComboxEntity risk = new ComboxEntity();
                risk.itemName = rv;
                risk.itemValue = rid;
                if (!conStr.Contains(rv))
                {
                    Rlist.Add(risk);//如果没有重复则加入
                    conStr.Add(rv);
                }
            }

            var data = Rlist;

            return ToJsonResult(data);
        }

        /// <summary>
        /// 是否超标下拉框数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetIsExcessiveCmbList()
        {
            List<ComboxEntity> Rlist = new List<ComboxEntity>();
            ComboxEntity yes = new ComboxEntity();
            yes.itemName = "是";
            yes.itemValue = "1";

            ComboxEntity no = new ComboxEntity();
            no.itemName = "否";
            no.itemValue = "0";

            Rlist.Add(yes);
            Rlist.Add(no);

            return ToJsonResult(Rlist);

        }

        /// <summary>
        /// 根据所选区域id输出危险因素下拉数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetArRiskCmbList(string areaid)
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

            //先获取所有数据
            DataTable hlist = hazardfactorsbll.GetList(areaid, wheresql);
            List<ComboxEntity> Rlist = new List<ComboxEntity>();
            List<string> conStr = new List<string>();//用于判断重复的集合
            foreach (DataRow item in hlist.Rows)
            {

                string rv = item["RISKVALUE"].ToString();
                string rid = item["RISKID"].ToString();


                ComboxEntity risk = new ComboxEntity();
                risk.itemName = rv;
                risk.itemValue = rid;
                if (!conStr.Contains(rv))
                {
                    Rlist.Add(risk);//如果没有重复则加入
                    conStr.Add(rv);
                }



            }
            var data = Rlist;

            return ToJsonResult(data);
        }

        /// <summary>
        /// 输出区域下拉数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAreaCmbList()
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

            //先获取所有数据
            DataTable hlist = hazardfactorsbll.GetList("", wheresql);
            List<ComboxEntity> Rlist = new List<ComboxEntity>();
            List<string> conStr = new List<string>();//用于判断重复的集合
            foreach (DataRow item in hlist.Rows)
            {

                string av = item["AreaValue"].ToString();
                string ai = item["AreaId"].ToString();
                ComboxEntity risk = new ComboxEntity();
                risk.itemName = av;
                risk.itemValue = ai;
                if (!conStr.Contains(av))
                {
                    Rlist.Add(risk);//如果没有重复则加入
                    conStr.Add(av);
                }

            }
            var data = Rlist;
            return ToJsonResult(data);
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
            hazardfactorsbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, HazardfactorsEntity entity, string UserName, string UserId)
        {
            try
            {
                string[] RiskValueList = entity.RiskValue.Split('|');
                string[] RiskIdList = entity.Riskid.Split('|');
                List<HazardfactorsEntity> list = new List<HazardfactorsEntity>();
                for (int i = 0; i < RiskValueList.Length; i++)
                {
                    HazardfactorsEntity temp = new HazardfactorsEntity();
                    temp = JsonConvert.DeserializeObject<HazardfactorsEntity>(JsonConvert.SerializeObject(entity));
                    temp.Riskid = RiskIdList[i];
                    temp.RiskValue = RiskValueList[i];
                    list.Add(temp);
                }
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                foreach (var item in list)
                {
                    if (hazardfactorsbll.ExistDeptJugement(item.AreaValue, user.OrganizeCode, item.RiskValue))
                    {
                        hazardfactorsbll.SaveForm(keyValue, item, UserName, UserId);
                    }
                }
                return Success("1");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
            

        }
        /// <summary>
        /// 导入清单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportDept()
        {

            int error = 0;
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
                DataTable dt = ExcelHelper.ExcelImport(Server.MapPath("~/Resource/temp/" + fileName));
                int order = 1;

                //先获取字典
                DataItemBLL di = new DataItemBLL();
                DistrictBLL districtbll = new DistrictBLL();
                IEnumerable<DataItemDetailEntity> ReskList = di.GetList("Risk");
                IEnumerable<DistrictEntity> AreaList = districtbll.GetOrgList(OperatorProvider.Provider.Current().OrganizeId);

                Expression<Func<UserEntity, bool>> condition;
                if (OperatorProvider.Provider.Current().IsSystem)
                {
                    condition = it => it.Account != "System" && it.IsPresence == "1";//如果是管理员
                }
                else
                {
                    string orgid = OperatorProvider.Provider.Current().OrganizeId;
                    condition = it => it.Account != "System" && it.IsPresence == "1" && it.OrganizeId == orgid;//不是管理员则查到所有本机构下的用户
                }

                //先获取到该用户下可以选择的用户
                List<UserEntity> userlist = new UserBLL().GetListForCon(condition).ToList();

                for (int i = 1; i < dt.Rows.Count; i++)
                {

                    //区域
                    string AreaName = dt.Rows[i][0].ToString();
                    string AreaValue = "";

                    //危险源
                    string RiskName = dt.Rows[i][1].ToString();//危险源名称
                    Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                    //验证同一公司范围内是否有重复数据
                    if (!hazardfactorsbll.ExistDeptJugement(AreaName, user.OrganizeCode, RiskName))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行数据存在重复,未能导入.";
                        error++;
                        continue;
                    }

                    //验证区域是否有匹配项
                    if (!GetAreaIsTrue(AreaList, AreaName.Trim(), out AreaValue))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行值区域值不在可选范围内,未能导入.";
                        error++;
                        continue;
                    }





                    string RiskValue = "";
                    //if (Risks.Length == 0)
                    //{
                    //    falseMessage += "</br>" + "第" + (i + 2) + "行值职业病危害因素名称值为空,未能导入.";
                    //    error++;
                    //    continue;
                    //}
                    if (!GetIsTrue(ReskList, RiskName, out RiskValue))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行值职业病危害因素名称值不在可选范围内,未能导入.";
                        error++;
                        continue;
                    }

                    ////危险源
                    //string Num = dt.Rows[i][2].ToString();//接触人数量

                    //string pattern = "^[0-9]+$";
                    //int Contactnumber = 0;


                    ////验证是否是数字
                    //if (Regex.IsMatch(Num, pattern))
                    //{
                    //    Contactnumber = Convert.ToInt32(Num);
                    //}
                    //else if (Num.Trim() == "")
                    //{

                    //}
                    //else
                    //{
                    //    falseMessage += "</br>" + "第" + (i + 2) + "行值接触人数只能填写数字或者不填,值类型错误,未能导入.";
                    //    error++;
                    //    continue;
                    //}

                    string users = dt.Rows[i][2].ToString();
                    if (users.Trim() == "")
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行值接触人员不能为空,未能导入.";
                        error++;
                        continue;
                    }

                    users = users.Replace('，', ',');//把所有大写，替换成小写,

                    string[] uses = users.Split(',');
                    string userids = "";
                    string errorname = "";
                    for (int j = 0; j < uses.Length; j++)
                    {
                        UserEntity ue = userlist.Where(it => it.RealName == uses[j]).FirstOrDefault();
                        if (ue == null)//如果用户不存在
                        {
                            if (errorname == "")
                            {
                                errorname = uses[j];
                            }
                            else
                            {
                                errorname += "," + uses[j];
                            }
                            continue;
                        }

                        if (j == 0)
                        {
                            userids = ue.UserId;
                        }
                        else
                        {
                            userids += "," + ue.UserId;
                        }
                    }

                    if (errorname.Trim() != "")
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行值接触人员:" + errorname + ",值不在可选范围内,未能导入.";
                        error++;
                        continue;
                    }

                    HazardfactorsEntity hf = new HazardfactorsEntity();
                    hf.AreaId = AreaValue;
                    hf.AreaValue = AreaName;
                    hf.Riskid = RiskValue;
                    hf.RiskValue = RiskName;
                    if (uses.Length > 0)
                    {
                        hf.ContactNumber = uses.Length;
                    }


                    try
                    {
                        hazardfactorsbll.SaveForm("", hf, users, userids);
                    }
                    catch
                    {
                        error++;
                    }

                }
                count = dt.Rows.Count - 1;
                message = "共有" + count + "条记录,成功导入" + (count - error) + "条，失败" + error + "条";
                message += "</br>" + falseMessage;
            }

            return message;
        }

        /// <summary>
        /// 导入字典危险源 //临时添加数据使用，后期如果要重新导入数据可以使用
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportDataItem()
        {

            int error = 0;
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
                DataTable dt = ExcelHelper.ExcelImport(Server.MapPath("~/Resource/temp/" + fileName));
                int order = 1;

                //先获取字典
                DataItemBLL di = new DataItemBLL();
                DataItemEntity Resk = di.GetEntityByCode("Risk");
                DataItemDetailBLL detabll = new DataItemDetailBLL();
                string[] names = { "粉尘", "化学因素", "物理因素", "放射性因素", "生物因素", "其他因素" };
                string[] namevalues = { "01", "02", "03", "04", "05", "06" };

                string value = "";
                int index = 1;
                string fname = file.FileName.Substring(0, file.FileName.IndexOf('.'));
                for (int i = 0; i < names.Length; i++)
                {
                    if (fname == names[i])
                    {
                        value = namevalues[i];
                    }
                }

                if (value != "")
                {
                    DataItemDetailEntity dide = new DataItemDetailEntity();
                    dide.ItemId = Resk.ItemId;
                    dide.ItemValue = value;
                    dide.ItemName = fname;
                    dide.SortCode = index;
                    dide.ParentId = "0";
                    try
                    {
                        detabll.SaveForm("", dide);
                    }
                    catch (Exception)
                    {

                        error++;
                    }

                    foreach (DataRow dr in dt.Rows)
                    {
                        DataItemDetailEntity did = new DataItemDetailEntity();
                        did.ItemId = Resk.ItemId;
                        if (index < 10)
                        {
                            did.ItemValue = value + "00" + index;
                        }
                        else if (index < 100)
                        {
                            did.ItemValue = value + "0" + index;
                        }
                        else
                        {
                            did.ItemValue = value + index;
                        }
                        did.ItemName = dr[0].ToString();
                        did.SortCode = index;
                        did.ParentId = "0";
                        index++;
                        try
                        {
                            detabll.SaveForm("", did);
                        }
                        catch (Exception)
                        {

                            error++;
                        }
                    }
                }



                count = dt.Rows.Count - 1;
                message = "共有" + count + "条记录,成功导入" + (count - error) + "条，失败" + error + "条";
                message += "</br>" + falseMessage;


            }
            return message;
        }

        #endregion
        /// <summary>
        /// 判断值是否包含在集合中
        /// </summary>
        /// <param name="ReskList"></param>
        /// <param name="value"></param>
        /// <param name="RiskValue"></param>
        /// <returns></returns>
        public bool GetIsTrue(IEnumerable<DataItemDetailEntity> ReskList, string value, out string RiskValue)
        {
            RiskValue = "";
            bool listFlag = false;
            foreach (DataItemDetailEntity item in ReskList)
            {
                if (value.Trim() == item.ItemName)
                {

                    RiskValue = item.ItemValue;
                    listFlag = true;
                }
            }
            return listFlag;
        }

        /// <summary>
        /// 判断值是否包含在集合中
        /// </summary>
        /// <param name="ReskList"></param>
        /// <param name="value"></param>
        /// <param name="RiskValue"></param>
        /// <returns></returns>
        public bool GetAreaIsTrue(IEnumerable<DistrictEntity> ReskList, string value, out string RiskValue)
        {
            RiskValue = "";
            bool listFlag = false;
            foreach (DistrictEntity item in ReskList)
            {
                if (value.Trim() == item.DistrictName)
                {
                    RiskValue = item.DistrictID;
                    listFlag = true;
                }
            }
            return listFlag;
        }

        ///// <summary>
        ///// 判断数组中是否有参数
        ///// </summary>
        ///// <param name="Risks"></param>
        ///// <param name="ReskList"></param>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //public bool GetCountTrue(string Risks, IEnumerable<DataItemDetailEntity> ReskList, out string Riskvalue)
        //{
        //    string value = "";
        //    Riskvalue = "";
        //    foreach (string r in Risks)
        //    {
        //        if (GetIsTrue(ReskList, r, out value))
        //        {
        //            if (Riskvalue == "")
        //            {
        //                Riskvalue = value;
        //            }
        //            else
        //            {
        //                Riskvalue += "," + value;
        //            }
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        /// <summary>
        /// 导出到Excel
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult Excel(string queryJson)
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
                wheresql += " and ha." + where;
            }

            DataTable dt = hazardfactorsbll.GetTable(queryJson, wheresql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i][0] = i + 1;
            }
            string FileUrl = @"\Resource\ExcelTemplate\职业病危害因素清单_导出模板.xlsx";
            AsposeExcelHelper.ExecuteResult(dt, FileUrl, "职业病危害因素清单", "职业病危害因素列表");

            return Success("导出成功。");
        }

        /// <summary>
        /// 数组去重复值
        /// </summary>
        /// <param name="Group"></param>
        /// <returns></returns>
        public string[] Deduplication(string[] Group)
        {
            List<string> str = new List<string>();
            foreach (string item in Group)
            {
                if (!str.Contains(item))
                {
                    str.Add(item);
                }
            }
            return str.ToArray();
        }
    }
}
