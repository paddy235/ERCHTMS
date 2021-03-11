using ERCHTMS.Entity.FireManage;
using ERCHTMS.Busines.FireManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ERCHTMS.Busines.SystemManage;
using System.Linq;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using ERCHTMS.Busines.BaseManage;
using System;
using System.Web;
using System.Data;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Cache;
using System.Text.RegularExpressions;

namespace ERCHTMS.Web.Areas.FireManage.Controllers
{
    /// <summary>
    /// 描 述：消防队伍
    /// </summary>
    public class FireTroopsController : MvcControllerBase
    {
        private FireTroopsBLL FireTroopsbll = new FireTroopsBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private readonly UserBLL userBLL = new UserBLL();
        private readonly OrganizeBLL orgBLL = new OrganizeBLL();
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private UserBLL userbll = new UserBLL();
        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.ehsDepartCode = "";
            //当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            DataItemModel ehsDepart = dataitemdetailbll.GetDataItemListByItemCode("'EHSDepartment'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();
            if (ehsDepart != null)
                ViewBag.ehsDepartCode = ehsDepart.ItemValue;
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
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            queryJson = queryJson ?? "";
            pagination.p_kid = "t.Id";
            pagination.p_fields = "t.UserName,t.UserId,t.Dept,t.DeptCode,t.Sex,t.IdentityCard,t.Quarters,t.QuartersId,t.Phone,t.Degrees,t.DegreesId,t.PlaceDomicile,t.Certificates,t.SortCode,t.createusername,t.createdate,t.createuserorgcode,t.createuserdeptcode,t.createuserid,d.nature";
            pagination.p_tablename = "HRS_FireTroops t left join Base_Department d on t.deptcode=d.encode";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                pagination.conditionJson += string.Format(" and t.CREATEUSERORGCODE ='{0}'", user.OrganizeCode);
            }
            var watch = CommonHelper.TimerStart();
            var data = FireTroopsbll.GetPageList(pagination, queryJson);
            foreach (DataRow dr in data.Rows)
            {
                if (dr["nature"].ToString() == "专业" || dr["nature"].ToString() == "班组")
                {
                    //DataTable dt = departmentBLL.GetDataTable(string.Format("select fullname from BASE_DEPARTMENT where encode=(select encode from BASE_DEPARTMENT t where instr('{0}',encode)=1 and nature='{1}' and organizeid='{2}') or encode='{0}' order by deptcode", dr["deptcode"], "部门", dr["createuserorgcode"]));
                    DataTable dt = departmentBLL.GetDataTable(string.Format(@"select D.FULLNAME,d.nature from BASE_DEPARTMENT O 
LEFT JOIN BASE_DEPARTMENT D ON o.parentid = D.DEPARTMENTID where o.encode='{0}'", dr["deptcode"]));
                    if (dt.Rows.Count > 0)
                    {
                        string name = "";
                        foreach (DataRow dr1 in dt.Rows)
                        {
                            name += dr1["fullname"].ToString() + "/";
                        }
                        name += dr["dept"].ToString();
                        dr["dept"] = name.TrimEnd('/');
                    }
                }
            }
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
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = FireTroopsbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = FireTroopsbll.GetEntity(keyValue);
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
            FireTroopsbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, FireTroopsEntity entity)
        {
            FireTroopsbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        /// <summary>
        /// 获取最大排序号
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetMaxSortCode(string queryJson)
        {
            try
            {
                var sortcode = 1;
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var data = new DepartmentBLL().GetDataTable("select max(sortcode) sortcode from HRS_FireTroops where  CREATEUSERORGCODE ='" + user.OrganizeCode + "'");
                if (data.Rows.Count > 0)
                {
                    if (data.Rows[0][0].ToString() != "")
                    {
                        sortcode = Convert.ToInt32(data.Rows[0][0].ToString()) + 1;
                    }
                }
                return ToJsonResult(sortcode);
            }
            catch {
                return ToJsonResult(1);
            }
        }
        #endregion

        #region 数据导出
        /// <summary>
        /// 导出
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "导出专职消防队伍")]
        public ActionResult Export(string queryJson)
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "rownum idx";
            pagination.p_fields = @"username,quarters,
sex,
identitycard,
 degreesid,certificates,placedomicile,phone,dept";
            pagination.p_tablename = "HRS_FireTroops t";
            pagination.conditionJson = string.Format(" 1=1 ");
            pagination.sidx = "SortCode";//排序字段
            pagination.sord = "asc";//排序方式  
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                pagination.conditionJson += string.Format(" and CREATEUSERORGCODE ='{0}'", user.OrganizeCode);
            }

            var watch = CommonHelper.TimerStart();
            var data = FireTroopsbll.GetPageList(pagination, queryJson);

            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "专职消防队伍";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 16;
            excelconfig.FileName = "专职消防队伍.xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity = listColumnEntity;
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "idx", ExcelColumn = "序号", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "username", ExcelColumn = "姓名", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "quarters", ExcelColumn = "职务", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "sex", ExcelColumn = "性别", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "identitycard", ExcelColumn = "身份证号", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "degreesid", ExcelColumn = "学历", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "certificates", ExcelColumn = "持证情况", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "placedomicile", ExcelColumn = "户籍所在地", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "phone", ExcelColumn = "联系方式", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "dept", ExcelColumn = "所属部门", Alignment = "center" });

            //调用导出方法
            ExcelHelper.ExcelDownload(data, excelconfig);

            return Success("导出成功。");
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        //[AjaxOnly]
        //[HandlerAuthorize(PermissionMode.Ignore)]
        // [ValidateAntiForgeryToken]
        public string ExcelImport()
        {
            if (OperatorProvider.Provider.Current().IsSystem)
            {
                return "超级管理员无此操作权限";
            }
            string orgId = OperatorProvider.Provider.Current().OrganizeId;//所属公司 
            var currUser = OperatorProvider.Provider.Current();
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
                int order = 2;
                string orgid = OperatorProvider.Provider.Current().OrganizeId;
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    FireTroopsEntity item = new FireTroopsEntity();
                    order = i + 1;
                    #region 序号
                    string sortcode = dt.Rows[i][0].ToString();
                    int tempSortCode;
                    if (!string.IsNullOrEmpty(sortcode))
                    {
                        if (Int32.TryParse(sortcode, out tempSortCode))
                        {
                            item.SortCode = tempSortCode;
                        }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,序号必须为整数！</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"第{0}行导入失败,序号不能为空！</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region 责任部门
                    string deptlist = dt.Rows[i][1].ToString();
                    if (string.IsNullOrEmpty(deptlist))
                    {
                        falseMessage += string.Format(@"第{0}行导入失败,单位（部门）不能为空！</br>", order);
                        error++;
                        continue;
                    }
                    var p1 = string.Empty; var p2 = string.Empty;
                    var array = deptlist.Split('/');
                    var deptFlag = false;
                    for (int j = 0; j < array.Length; j++)
                    {
                        if (j == 0)
                        {
                            if (currUser.RoleName.Contains("省级") || currUser.RoleName.Contains("集团"))
                            {
                                var entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.FullName == array[j].ToString()).FirstOrDefault();
                                if (entity1 == null)
                                {
                                    //falseMessage += "</br>" + "第" + (i + 3) + "行部门不存在,未能导入.";
                                    //error++;
                                    //deptFlag = true;
                                    //break;
                                    item.Dept = deptlist;
                                    break;
                                }
                                else
                                {
                                    item.Dept = entity1.FullName;
                                    item.DeptCode = entity1.EnCode;
                                    p1 = entity1.DepartmentId;
                                }
                            }
                            else
                            {
                                var entity = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "厂级" && x.FullName == array[j].ToString()).FirstOrDefault();
                                if (entity == null)
                                {
                                    entity = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && (x.Nature == "部门" || x.Nature == "承包商" || x.Nature == "分包商") && x.FullName == array[j].ToString()).FirstOrDefault();
                                    if (entity == null)
                                    {
                                        entity = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "承包商" && x.FullName == array[j].ToString()).FirstOrDefault();
                                        if (entity == null)
                                        {
                                            //falseMessage += "</br>" + "第" + (i + 3) + "行部门不存在,未能导入.";
                                            //error++;
                                            //deptFlag = true;
                                            //break;
                                            item.Dept = deptlist;
                                            break;
                                        }
                                        else
                                        {
                                            item.Dept = entity.FullName;
                                            item.DeptCode = entity.EnCode;
                                            p1 = entity.DepartmentId;
                                        }
                                    }
                                    else
                                    {
                                        item.Dept = entity.FullName;
                                        item.DeptCode = entity.EnCode;
                                        p1 = entity.DepartmentId;
                                    }
                                }
                                else
                                {
                                    p1 = entity.DepartmentId;
                                }
                            }
                        }
                        else if (j == 1)
                        {
                            var entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && (x.Nature == "专业" || x.Nature == "承包商" || x.Nature == "分包商") && x.FullName == array[j].ToString() && x.ParentId == p1).FirstOrDefault();
                            if (entity1 == null)
                            {
                                entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "班组" && x.FullName == array[j].ToString() && x.ParentId == p1).FirstOrDefault();
                                if (entity1 == null)
                                {
                                    //falseMessage += "</br>" + "第" + (i + 3) + "行专业/班组不存在,未能导入.";
                                    //error++;
                                    //deptFlag = true;
                                    //break;
                                    item.Dept = deptlist;
                                    break;
                                }
                                else
                                {
                                    item.Dept = entity1.FullName;
                                    item.DeptCode = entity1.EnCode;
                                    p2 = entity1.DepartmentId;
                                }
                            }
                            else
                            {
                                item.Dept = entity1.FullName;
                                item.DeptCode = entity1.EnCode;
                                p2 = entity1.DepartmentId;
                            }

                        }
                        else
                        {
                            var entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && (x.Nature == "班组" || x.Nature == "承包商" || x.Nature == "分包商") && x.FullName == array[j].ToString() && x.ParentId == p2).FirstOrDefault();
                            if (entity1 == null)
                            {
                                //falseMessage += "</br>" + "第" + (i + 3) + "行班组不存在,未能导入.";
                                //error++;
                                //deptFlag = true;
                                //break;
                                item.Dept = deptlist;
                                break;
                            }
                            else
                            {
                                item.Dept = entity1.FullName;
                                item.DeptCode = entity1.EnCode;
                            }
                        }
                    }
                    if (deptFlag) continue;
                    #endregion

                    #region 名称
                    string username = dt.Rows[i][2].ToString().Trim();
                    if (string.IsNullOrEmpty(username))
                    {
                        falseMessage += string.Format(@"第{0}行导入失败,姓名不能为空！</br>", order);
                        error++;
                        continue;
                    }
                    if (username != "")
                    {
                        if (item.DeptCode != "" && item.DeptCode != null)
                        {
                            UserInfoEntity userEntity = userbll.GetUserInfoByName(item.Dept, username);
                            if (userEntity == null)
                            {
                                falseMessage += string.Format(@"第{0}行,姓名在【{1}】不存在！</br>", order, item.Dept);
                                error++;
                                continue;
                            }
                            else
                            {
                                item.UserId = userEntity.UserId;
                                item.UserName = userEntity.RealName;
                            }
                        }
                        else {
                            item.UserName = username;
                        }
                    }
                    #endregion
                    

                    #region 职务
                    string quarters = dt.Rows[i][3].ToString();
                    if (!string.IsNullOrEmpty(quarters))
                    {
                        var data = new DataItemCache().ToItemValue("Quarters", quarters);
                        if (data != null && !string.IsNullOrEmpty(data))
                        {
                            item.Quarters = quarters;
                        }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,职务不存在！</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"第{0}行导入失败,职务不能为空！</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region 性别
                    item.Sex = dt.Rows[i][4].ToString().Trim();
                    #endregion

                    #region 身份证号
                    //身份证号
                    string identity = dt.Rows[i][5].ToString().Trim();
                    if (!string.IsNullOrEmpty(identity)) {
                        if (!Regex.IsMatch(identity, @"^(^d{15}$|^\d{18}$|^\d{17}(\d|X|x))$", RegexOptions.IgnoreCase))
                        {
                            falseMessage += string.Format(@"第{0}行身份证号格式有误！</br>", order);
                            error++;
                            continue;
                        }
                        else {
                            item.IdentityCard = identity;
                        }
                    }
                    #endregion

                    #region 学历
                    string degrees = dt.Rows[i][6].ToString().Trim();
                    if (!string.IsNullOrEmpty(degrees))
                    {
                        var data = new DataItemCache().ToItemValue("Degrees", degrees);
                        if (data != null && !string.IsNullOrEmpty(data))
                        {
                            item.DegreesId = degrees;
                        }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,学历不存在！</br>", order);
                            error++;
                            continue;
                        }
                    }
                    #endregion

                    #region 持证情况
                    string certificates = dt.Rows[i][7].ToString().Trim();
                    if (!string.IsNullOrEmpty(certificates))
                    {
                        if (certificates.Length > 100)
                        {
                            falseMessage += string.Format(@"第{0}行持证情况字符过长！</br>", order);
                            error++;
                            continue;
                        }
                        else
                        {
                            item.Certificates = certificates;
                        }
                    }
                    #endregion

                    #region 户籍所在地
                    string placedomicile = dt.Rows[i][8].ToString().Trim();
                    if (!string.IsNullOrEmpty(placedomicile))
                    {
                        item.PlaceDomicile = placedomicile;
                    }
                    #endregion

                    #region 手机号
                    string phone = dt.Rows[i][9].ToString().Trim();
                    if (!string.IsNullOrEmpty(phone))
                    {
                        if (!Regex.IsMatch(phone, @"^(\+\d{2,3}\-)?\d{11}$", RegexOptions.IgnoreCase))
                        {
                            falseMessage += string.Format(@"第{0}行手机号格式不正确！</br>", order);
                            error++;
                            continue;
                        }
                        else
                        {
                            item.Certificates = certificates;
                        }
                    }
                    #endregion


                    try
                    {
                        FireTroopsbll.SaveForm("", item);
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
        /// 验证身份证是否合法
        /// </summary>
        /// <param name="IdCard"></param>
        /// <param name="error"></param>
        /// <param name="sbirthday"></param>
        /// <returns></returns>
        public bool CheckIdCard(string IdCard, out string error, out string sbirthday)
        {
            //var aCity ={ 11: "北京", 12: "天津", 13: "河北", 14: "山西", 15: "内蒙古", 21: "辽宁", 22: "吉林", 23: "黑龙江", 31: "上海", 32: "江苏", 33: "浙江", 34: "安徽", 35: "福建", 36: "江西", 37: "山东", 41: "河南", 42: "湖北", 43: "湖南", 44: "广东", 45: "广西", 46: "海南", 50: "重庆", 51: "四川", 52: "贵州", 53: "云南", 54: "西藏", 61: "陕西", 62: "甘肃", 63: "青海", 64: "宁夏", 65: "新疆", 71: "台湾", 81: "香港", 82: "澳门", 91: "国外" };
            List<int> aCity = new List<int>() { 11, 12, 13, 14, 15, 21, 22, 23, 31, 32, 33, 34, 35, 36, 37, 41, 42, 43, 44, 45, 46, 50, 51, 52, 53, 54, 61, 62, 63, 64, 65, 71, 81, 82, 91 };
            error = "";
            sbirthday = "";
            var iSum = 0.0;
            if (!Regex.IsMatch(IdCard, @"^(^d{15}$|^\d{18}$|^\d{17}(\d|X|x))$", RegexOptions.IgnoreCase))
            {
                error = "你输入的身份证长度或格式错误";
                return false;
            }
            //IdCard = IdCard.replace(/x$/i, "a");
            if (!aCity.Contains(Convert.ToInt32(IdCard.Substring(0, 2))))
            {
                error = "你的身份证地区非法";
                return false;
            }
            var sBirthday = IdCard.Substring(6, 4) + "-" + IdCard.Substring(10, 2) + "-" + IdCard.Substring(12, 2);
            try
            {

                if (!string.IsNullOrEmpty(sBirthday))
                {
                    DateTime r = new DateTime();
                    if (DateTime.TryParse(sBirthday, out r))
                    {
                        sbirthday = r.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        error = "身份证上的出生日期非法";
                        return false;
                    }

                }
            }
            catch
            {
                error = "身份证号有误";
                return false;
            }
            //for (var i = 17; i >= 0; i--) {
            //    iSum += (Math.Pow(2, i) % 11) * Convert.ToInt32(IdCard.ToArray()[17 - i].ToInt().ToString(), 11);
            //}
            //if (iSum % 11 != 1) {
            //    error = "你输入的身份证号非法";
            //    return false;
            //}

            return true;
        }
        #endregion
    }
}
