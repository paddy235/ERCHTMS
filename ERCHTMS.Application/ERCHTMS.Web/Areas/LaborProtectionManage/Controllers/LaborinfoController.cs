using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using ERCHTMS.Entity.LaborProtectionManage;
using ERCHTMS.Busines.LaborProtectionManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using BSFramework.Util.Extension;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.PersonManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Cache;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.SystemManage.ViewModel;

namespace ERCHTMS.Web.Areas.LaborProtectionManage.Controllers
{
    /// <summary>
    /// 描 述：劳动防护用品表
    /// </summary>
    public class LaborinfoController : MvcControllerBase
    {
        private LaborinfoBLL laborinfobll = new LaborinfoBLL();

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
        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }

        #endregion

        #region 获取数据

        /// <summary>
        /// 获取是否可以选择部门
        /// </summary>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public string GetIsDept()
        {
            var issystem = OperatorProvider.Provider.Current().IsSystem;
            if (!issystem)
            {//如果不是系统管理员
                var deptid = OperatorProvider.Provider.Current().DeptId;
                var deptname = OperatorProvider.Provider.Current().DeptCode;
                DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();
                var data = dataItemDetailBLL.GetDataItemListByItemCode("'SelectDept'").ToList();
                foreach (var item in data)
                {
                    string value = item.ItemValue;
                    string[] values = value.Split('|');
                    for (int i = 0; i < values.Length; i++)
                    {
                        if (values[i] == deptname)
                        {
                            return deptid + ",true";
                        }
                    }
                }

                if (OperatorProvider.Provider.Current().RoleName.Contains("公司级用户"))
                {
                    return deptid + ",true";
                }

                return deptid + ",flase";


            }
            else
            {
                return ",true";
            }
        }

        /// <summary>
        /// 获取是否拥有权限
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetPer()
        {
            return laborinfobll.GetPer().ToString();
        }

        /// <summary>
        /// 获取 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetLaborUser(string deptId, string postId, string type)
        {
            UserBLL userbll = new UserBLL();
            List<LaborequipmentinfoEntity> LaborUlist = new List<LaborequipmentinfoEntity>();
            if (postId != "")
            {
                List<UserEntity> userlist = userbll.GetListForCon(it =>
                    it.DepartmentId == deptId && it.DutyId == postId && it.IsPresence == "1").ToList();
                foreach (var user in userlist)
                {
                    LaborequipmentinfoEntity laboruser = new LaborequipmentinfoEntity();
                    laboruser.UserName = user.RealName;
                    laboruser.LaborType = 0;
                    laboruser.ShouldNum = 1;
                    laboruser.UserId = user.UserId;
                    if (type == "衣服")
                    {
                        laboruser.Size = "L";
                    }
                    else if (type == "鞋子")
                    {
                        laboruser.Size = "40";
                    }
                    else
                    {
                        laboruser.Size = "";
                    }

                    laboruser.Create();
                    LaborUlist.Add(laboruser);
                }
            }
            return ToJsonResult(LaborUlist);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            string orgcode = OperatorProvider.Provider.Current().OrganizeCode;
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "ID";
            pagination.p_fields = "NO,info.createuserid,info.createuserdeptcode,info.createuserorgcode,info.NAME,TYPE,ORGNAME,DEPTNAME,POSTNAME,SHOULDNUM,UNIT,TIMENUM,TIMETYPE,RECENTTIME,NEXTTIME,ISSUENUM,'' InStock,yj.value";
            pagination.p_tablename = "BIS_LABORINFO info left join (select name,value from bis_laboreamyj where createuserorgcode='" + orgcode + "') yj on info.name=yj.name";
            pagination.conditionJson = " 1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                if (laborinfobll.GetPer())
                {

                    pagination.conditionJson += " and CREATEUSERORGCODE='" + user.OrganizeCode + "'";
                }
                else
                {
                    string where = new ERCHTMS.Busines.AuthorizeManage.AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
                    pagination.conditionJson += " and " + where;
                }



            }

            var data = laborinfobll.GetPageList(pagination, queryJson);

            DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();

            var datadetail = dataItemDetailBLL.GetDataItemListByItemCode("'LaborName'");

            for (int i = 0; i < data.Rows.Count; i++)
            {
                DataItemModel dm = datadetail.Where(it => it.ItemName == data.Rows[i]["NAME"].ToString()).FirstOrDefault();
                if (dm != null)
                {
                    data.Rows[i]["InStock"] = dm.ItemValue.ToString();
                }
            }
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch),

            };
            return Content(JsonData.ToJson());
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = laborinfobll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #endregion

        #region 导出数据
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
                wheresql = "1=1";
            }
            else
            {
                string where = new ERCHTMS.Busines.AuthorizeManage.AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
                wheresql += " and " + where;

            }

            DataTable dt = laborinfobll.GetTable(queryJson, wheresql);
            DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();

            var datadetail = dataItemDetailBLL.GetDataItemListByItemCode("'LaborName'");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i][0] = i + 1;
                dt.Rows[i]["TimeType"] = dt.Rows[i]["TimeNum"].ToString() + dt.Rows[i]["TimeType"].ToString();
                DataItemModel dm = datadetail.Where(it => it.ItemName == dt.Rows[i]["NAME"].ToString()).FirstOrDefault();
                if (dm != null)
                {
                    dt.Rows[i]["InStock"] = dm.ItemValue.ToString();
                }
                else
                {
                    dt.Rows[i]["InStock"] = "";
                }
            }

            string FileUrl = @"\Resource\ExcelTemplate\劳动防护用品管理_导出.xls";
            AsposeExcelHelper.ExecuteResult(dt, FileUrl, "劳动防护用品管理清单", "劳动防护用品管理列表");

            return Success("导出成功。");
        }
        #endregion

        #region 提交数据

        /// <summary>
        /// 导入用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public string ImportLabor()
        {
            LaborprotectionBLL laborprotectionbll = new LaborprotectionBLL();
            PostCache postCache = new PostCache();
            PostBLL postBLL = new PostBLL();
            DepartmentBLL departmentBLL = new DepartmentBLL();
            //获取到已选数据
            List<LaborprotectionEntity> laborlist = laborprotectionbll.GetLaborList();
            var currUser = OperatorProvider.Provider.Current();
            string orgId = OperatorProvider.Provider.Current().OrganizeId;//所属公司
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
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                wb.Open(Server.MapPath("~/Resource/temp/" + fileName));
                Aspose.Cells.Cells cells = wb.Worksheets[0].Cells;
                if (cells.MaxDataRow == 0)
                {
                    message = "没有数据,请选择先填写模板在进行导入!";
                    return message;
                }
                DataTable dt = cells.ExportDataTable(0, 0, cells.MaxDataRow + 1, cells.MaxColumn + 1, true);
                int order = 1;

                IList<LaborprotectionEntity> LaborList = new List<LaborprotectionEntity>();

                IEnumerable<DepartmentEntity> deptlist = new DepartmentBLL().GetList();
                OrganizeBLL orgbll = new OrganizeBLL();
                //先获取到原始的一个编号
                string no = laborprotectionbll.GetNo();
                int ysno = Convert.ToInt32(no);

                DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();
                var dataitem = dataItemDetailBLL.GetDataItemListByItemCode("'LaborName'").ToList();
                List<LaborprotectionEntity> insertpro = new List<LaborprotectionEntity>();
                List<LaborinfoEntity> insertinfo = new List<LaborinfoEntity>();
                //先获取人员
                List<UserEntity> userlist =
                    new UserBLL().GetListForCon(it => it.IsPresence == "1" && it.Account != "System").ToList();
                List<LaborequipmentinfoEntity> eqlist = new List<LaborequipmentinfoEntity>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    order = i;
                    string Name = dt.Rows[i]["名称"].ToString();
                    string Model = dt.Rows[i]["型号"].ToString();
                    string Type = dt.Rows[i]["类型"].ToString();
                    string DeptName = dt.Rows[i]["使用部门"].ToString();
                    string OrgName = dt.Rows[i]["使用单位"].ToString();
                    string PostName = dt.Rows[i]["使用岗位"].ToString().Trim();
                    string Unit = dt.Rows[i]["劳动防护用品单位"].ToString().Trim();
                    string Time = dt.Rows[i]["使用期限"].ToString().Trim();
                    string TimeType = dt.Rows[i]["使用期限单位"].ToString().Trim();
                    string deptId = "", deptCode = "", PostId = "";
                    //---****值存在空验证*****--
                    if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Unit) || string.IsNullOrEmpty(DeptName) || string.IsNullOrEmpty(OrgName) || string.IsNullOrEmpty(PostName))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行值存在空,未能导入.";
                        error++;
                        continue;
                    }

                    //验证机构是不是和自己一个机构
                    DepartmentEntity org = deptlist.Where(it => it.FullName == OrgName).FirstOrDefault();
                    if (org == null)
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行使用单位名称不存在,未能导入.";
                        error++;
                        continue;
                    }
                    //如果导入的机构id和本人的机构id不一致
                    if (org.DepartmentId != currUser.OrganizeId)
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行使用单位不是导入者的单位,未能导入.";
                        error++;
                        continue;
                    }

                    //验证所填部门是否存在
                    var deptFlag = false;
                    var entity1 = deptlist.Where(x => x.OrganizeId == currUser.OrganizeId && x.FullName == DeptName).FirstOrDefault();
                    if (entity1 == null)
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行使用部门不存在,未能导入.";
                        error++;
                        deptFlag = true;
                        break;
                    }
                    else
                    {
                        deptId = entity1.DepartmentId;
                        deptCode = entity1.EnCode;
                    }

                    //var deptFlag = false;
                    //var array = DeptName.Split('/');
                    //for (int j = 0; j < array.Length; j++)
                    //{
                    //    if (j == 0)
                    //    {
                    //        if (currUser.RoleName.Contains("省级"))
                    //        {
                    //            var entity1 = deptlist.Where(x => x.OrganizeId == currUser.OrganizeId && x.FullName == array[j].ToString()).FirstOrDefault();
                    //            if (entity1 == null)
                    //            {
                    //                falseMessage += "</br>" + "第" + (i + 2) + "行部门不存在,未能导入.";
                    //                error++;
                    //                deptFlag = true;
                    //                break;
                    //            }
                    //            else
                    //            {
                    //                deptId = entity1.DepartmentId;
                    //                deptCode = entity1.EnCode;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            var entity = deptlist.Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "厂级" && x.FullName == array[j].ToString()).FirstOrDefault();
                    //            if (entity == null)
                    //            {
                    //                entity = deptlist.Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "部门" && x.FullName == array[j].ToString()).FirstOrDefault();
                    //                if (entity == null)
                    //                {
                    //                    entity = deptlist.Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "承包商" && x.FullName == array[j].ToString()).FirstOrDefault();
                    //                    if (entity == null)
                    //                    {
                    //                        falseMessage += "</br>" + "第" + (i + 2) + "行部门不存在,未能导入.";
                    //                        error++;
                    //                        deptFlag = true;
                    //                        break;
                    //                    }
                    //                    else
                    //                    {
                    //                        deptId = entity.DepartmentId;
                    //                        deptCode = entity.EnCode;
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    deptId = entity.DepartmentId;
                    //                    deptCode = entity.EnCode;
                    //                }
                    //            }
                    //            else
                    //            {
                    //                deptId = entity.DepartmentId;
                    //                deptCode = entity.EnCode;
                    //            }
                    //        }
                    //    }
                    //    else if (j == 1)
                    //    {
                    //        var entity1 = deptlist.Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "专业" && x.FullName == array[j].ToString()).FirstOrDefault();
                    //        if (entity1 == null)
                    //        {
                    //            entity1 = deptlist.Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "班组" && x.FullName == array[j].ToString()).FirstOrDefault();
                    //            if (entity1 == null)
                    //            {
                    //                falseMessage += "</br>" + "第" + (i + 2) + "行专业/班组不存在,未能导入.";
                    //                error++;
                    //                deptFlag = true;
                    //                break;
                    //            }
                    //            else
                    //            {
                    //                deptId = entity1.DepartmentId;
                    //                deptCode = entity1.EnCode;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            deptId = entity1.DepartmentId;
                    //            deptCode = entity1.EnCode;
                    //        }

                    //    }
                    //    else
                    //    {
                    //        var entity1 = deptlist.Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "班组" && x.FullName == array[j].ToString()).FirstOrDefault();
                    //        if (entity1 == null)
                    //        {
                    //            falseMessage += "</br>" + "第" + (i + 2) + "行班组不存在,未能导入.";
                    //            error++;
                    //            deptFlag = true;
                    //            break;
                    //        }
                    //        else
                    //        {
                    //            deptId = entity1.DepartmentId;
                    //            deptCode = entity1.EnCode;
                    //        }
                    //    }
                    //}
                    if (deptFlag) continue;

                    //检验所填岗位是否属于其公司或者部门
                    if (string.IsNullOrEmpty(deptId) || deptId == "undefined")
                    {
                        //所属公司
                        RoleEntity data = postCache.GetList(orgId, "true").OrderBy(x => x.SortCode).Where(a => a.FullName == PostName).FirstOrDefault();
                        if (data == null)
                        {
                            falseMessage += "</br>" + "第" + (i + 2) + "行岗位不属于该公司,未能导入.";
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        //所属部门
                        //所属公司
                        RoleEntity data = postCache.GetList(orgId, deptId).OrderBy(x => x.SortCode).Where(a => a.FullName == PostName).FirstOrDefault();
                        if (data == null)
                        {
                            falseMessage += "</br>" + "第" + (i + 2) + "行岗位不属于该部门,未能导入.";
                            error++;
                            continue;
                        }
                    }
                    //--**验证岗位是否存在**--


                    RoleEntity re = postBLL.GetList().Where(a => a.FullName == PostName && a.OrganizeId == orgId).FirstOrDefault();
                    if (!(string.IsNullOrEmpty(deptId) || deptId == "undefined"))
                    {
                        re = postBLL.GetList().Where(a => a.FullName == PostName && a.OrganizeId == orgId && a.DeptId == deptId).FirstOrDefault();
                        if (re == null)
                        {
                            re = postBLL.GetList().Where(a =>
                                a.FullName == PostName && a.OrganizeId == orgId &&
                                a.Nature == departmentBLL.GetEntity(deptId).Nature).FirstOrDefault();
                        }
                    }
                    if (re == null)
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行岗位有误,未能导入.";
                        error++;
                        continue;
                    }
                    else
                    {
                        PostId = re.RoleId;
                    }

                    LaborinfoEntity linfo = new LaborinfoEntity();
                    linfo.PostId = PostId;
                    linfo.DeptCode = deptCode;
                    linfo.DeptId = deptId;
                    linfo.DeptName = DeptName;
                    linfo.LaboroPerationTime = DateTime.Now;
                    linfo.LaboroPerationUserName = currUser.UserName;
                    linfo.Model = Model;
                    linfo.Name = Name;
                    linfo.OrgCode = currUser.OrganizeCode;
                    linfo.OrgId = currUser.OrganizeId;
                    linfo.OrgName = currUser.OrganizeName;
                    linfo.Type = Type;
                    if (Time == "" || !isInt(Time))
                    {
                        linfo.TimeNum = null;
                    }
                    else
                    {
                        linfo.TimeNum = Convert.ToInt32(Time);
                        linfo.TimeType = TimeType;
                    }

                    linfo.PostName = PostName;
                    linfo.Unit = Unit;
                    linfo.Create();
                    //如果已存在物品库中
                    LaborprotectionEntity lp = laborlist.Where(it => it.Name == Name).FirstOrDefault();
                    if (lp != null)
                    {
                        linfo.No = lp.No;
                        linfo.LId = lp.ID;
                        //如果库里有值 则使用库里的值
                        linfo.Type = linfo.Type;
                        linfo.TimeNum = lp.TimeNum;
                        linfo.TimeType = lp.TimeType;
                    }
                    else
                    {
                        LaborprotectionEntity newlp = new LaborprotectionEntity();
                        newlp.Create();
                        newlp.Name = Name;
                        newlp.No = ysno.ToString();
                        newlp.LaborOperationTime = DateTime.Now;
                        newlp.LaborOperationUserName = currUser.UserName;
                        newlp.Model = Model;
                        newlp.Type = Type;
                        newlp.Unit = Unit;
                        newlp.TimeNum = linfo.TimeNum;
                        newlp.TimeType = TimeType;
                        linfo.No = ysno.ToString();
                        linfo.LId = newlp.ID;
                        ysno++;
                        insertpro.Add(newlp);
                    }

                    int num = 0;
                    List<UserEntity> ulist = userlist.Where(it => it.DepartmentId == deptId && it.DutyId == PostId).ToList();
                    for (int j = 0; j < ulist.Count; j++)
                    {
                        //添加岗位关联人员
                        LaborequipmentinfoEntity eq = new LaborequipmentinfoEntity();
                        eq.UserName = ulist[j].RealName;
                        eq.AssId = linfo.ID;
                        eq.LaborType = 0;
                        eq.ShouldNum = 1;
                        num++;
                        eq.UserId = ulist[j].UserId;
                        if (linfo.Type == "衣服")
                        {
                            eq.Size = "L";
                        }
                        else if (linfo.Type == "鞋子")
                        {
                            eq.Size = "40";
                        }
                        else
                        {
                            eq.Size = "";
                        }
                        eq.Create();
                        eqlist.Add(eq);
                    }

                    linfo.ShouldNum = num;
                    insertinfo.Add(linfo);
                }



                laborinfobll.ImportSaveForm(insertinfo, insertpro, eqlist);

                count = dt.Rows.Count;
                message = "共有" + count + "条记录,成功导入" + (count - error) + "条，失败" + error + "条";
                message += "</br>" + falseMessage;
            }

            return message;
        }

        public bool isInt(string instring)
        {
            return Regex.IsMatch(instring, @"[1-9]\d*$");
        }

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
            laborinfobll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, LaborinfoEntity entity, string json, string ID)
        {
            json = HttpUtility.UrlDecode(json);
            laborinfobll.SaveForm(keyValue, entity, json, ID);
            return Success("操作成功。");
        }
        #endregion
    }
}
