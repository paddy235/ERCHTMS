using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.EquipmentManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity;
using ERCHTMS.Entity.SystemManage.ViewModel;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.EquipmentManage;
using BSFramework.Util.Offices;
using System.Web;
using ERCHTMS.Cache;
using ERCHTMS.Entity.SystemManage;

namespace ERCHTMS.Web.Areas.EquipmentManage.Controllers
{
    /// <summary>
    /// 描 述：危险化学品库存
    /// </summary>
    public class DangerChemicalsController : MvcControllerBase
    {
        private UserBLL userbll = new UserBLL(); //用户操作对象
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private DangerChemicalsBLL DangerChemicalsBll = new DangerChemicalsBLL();
        private readonly DepartmentBLL departBLL = new DepartmentBLL();
        private readonly UserBLL userBLL = new UserBLL();
        private readonly OrganizeBLL orgBLL = new OrganizeBLL();
        private DepartmentBLL departmentBLL = new DepartmentBLL();

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
        /// 获取列表(库存管理)
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {

            pagination.p_kid = "id";
            pagination.p_fields = @"createdate,createuserid,createuserdeptcode,createuserorgcode,modifydate,modifyuserid,MAXNUM,
Name,Alias,Cas,Specification,Inventory,Unit,RiskType,ProductionDate,DepositDate,Site,Deadline,DutyUser,DutyUserId,DutyDept,DutyDeptCode,IsScene,
SpecificationUnit,Amount,AmountUnit";
            pagination.p_tablename = @"XLD_DANGEROUSCHEMICAL";
            pagination.conditionJson = string.Format(" (IsDelete<>1 or IsDelete is null) ");
            //pagination.sidx = "createdate";//排序字段
            //pagination.sord = "desc";//排序方式  

            var watch = CommonHelper.TimerStart();            
            var data = DangerChemicalsBll.GetList(pagination, queryJson);
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
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = DangerChemicalsBll.GetEntity(keyValue);
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
            DangerChemicalsBll.RemoveForm(keyValue);//删除申请
            //new PlanDetailsBLL().RemoveFormByApplyId(keyValue);//删除详情
            //new PlanCheckBLL().RemoveForm(keyValue);//删除审核           

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
        public ActionResult SaveForm(string keyValue, DangerChemicalsEntity entity)
        {
            try
            {
                string sTime = DateTime.Now.AddDays(-180).ToString("yyyy-MM-dd");
                string eTime = DateTime.Now.ToString("yyyy-MM-dd");
                string sql = string.Format(@"select sum(to_number(d.practicalnum)) numcount from 
XLD_DANGEROUSCHEMICALRECEIVE d where d.createdate between to_date('{0}', 'yyyy-MM-dd') and  to_date('{1}', 'yyyy-MM-dd')", sTime, eTime);
                var data = departmentBLL.GetDataTable(sql);
                if (!string.IsNullOrEmpty(keyValue))
                {
                    var old = DangerChemicalsBll.GetEntity(keyValue);
                    if (old != null)
                    {
                        if (Convert.ToDecimal(entity.Inventory) > Convert.ToDecimal(data.Rows[0][0]))
                        {
                            var officeuser = new UserBLL().GetEntity(entity.DutyUserId);
                            MessageEntity msg = new MessageEntity()
                            {
                                Id = Guid.NewGuid().ToString(),
                                UserId = officeuser.Account,
                                UserName = officeuser.RealName,
                                SendTime = DateTime.Now,
                                SendUser = "system",
                                SendUserName = "系统管理员",
                                Title = "危化品超量提醒",
                                Content = string.Format("您好，根据本单位《危险化学品管理办法》，危化品的存量一般不应大于半年用量，请提醒使用部门（单位）编制危化品采购计划时，充分考虑生产需要和不超量贮存情况。"),
                                Category = "其它"
                            };
                            new MessageBLL().SaveForm("", msg);
                        }
                    }
                }
                
            }
            catch { }
            DangerChemicalsBll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        /// <summary>
        /// 提交表单
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, DangerChemicalsEntity entity)
        {
            return Success("操作成功。");
        }
        #endregion

        #region 数据导出
        /// <summary>
        /// 导出
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "导出危化品")]
        public ActionResult Export(string queryJson)
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "rownum idx";
            pagination.p_fields = @"Name,(Concat(Specification,SpecificationUnit)) as Specification,
(Concat(amount,amountUnit)) as amount,
(Concat(inventory,Unit)) as inventory,maxnum,
 risktype,deadline,site,dutydept,dutyuser";
            pagination.p_tablename = "XLD_DANGEROUSCHEMICAL t";
            pagination.conditionJson = string.Format(" (IsDelete<>1 or IsDelete is null) ");
            //pagination.sidx = "createdate";//排序字段
            //pagination.sord = "desc";//排序方式  
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                pagination.conditionJson += string.Format(" and CREATEUSERORGCODE ='{0}'", user.OrganizeCode);
            }

            var watch = CommonHelper.TimerStart();
            var data = DangerChemicalsBll.GetList(pagination, queryJson);

            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "危化品库存";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 16;
            excelconfig.FileName = "危化品库存.xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity = listColumnEntity;
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "idx", ExcelColumn = "序号", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "name", ExcelColumn = "名称", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "specification", ExcelColumn = "规格", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "amount", ExcelColumn = "数量", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "inventory", ExcelColumn = "库存数量", Alignment = "center" });

            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "maxnum", ExcelColumn = "最大存储量", Alignment = "center" });

            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "risktype", ExcelColumn = "危险品类型", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "deadline", ExcelColumn = "存放期限", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "site", ExcelColumn = "存放地点", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "dutydept", ExcelColumn = "责任部门", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "dutyuser", ExcelColumn = "责任人", Alignment = "center" });

            //调用导出方法
            ExcelHelper.ExcelDownload(data, excelconfig);

            return Success("导出成功。");
        }
        #endregion

        #region 数据导入
        /// <summary>
        /// 导入重点防火部位
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
                int order = 1;
                string orgid = OperatorProvider.Provider.Current().OrganizeId;
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    DangerChemicalsEntity item = new DangerChemicalsEntity();
                    order = i + 1;
                    #region 危化品名称
                    string name = dt.Rows[i][0].ToString();
                    if (!string.IsNullOrEmpty(name))
                    {
                        item.Name = name;
                    }
                    else
                    {
                        falseMessage += string.Format(@"第{0}行导入失败,危化品名称不能为空！</br>", order);
                        error++;
                        continue;
                    }
                    #endregion
                    //别名
                    string alias = dt.Rows[i][1].ToString();
                    if (!string.IsNullOrEmpty(alias))
                    {
                        item.Alias = alias;
                    }
                    //CAS号
                    string cas = dt.Rows[i][2].ToString();
                    if (!string.IsNullOrEmpty(cas))
                    {
                        item.Cas = cas;
                    }
                    #region 危化品类型
                    string risktype = dt.Rows[i][3].ToString();
                    if (!string.IsNullOrEmpty(risktype))
                    {
                        var data = new DataItemCache().ToItemValue("ChemicalsRiskType", risktype);
                        if (data != null && !string.IsNullOrEmpty(data))
                        {
                            item.RiskType = risktype;
                        }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,危化品类型不存在！</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"第{0}行导入失败,危化品类型不能为空！</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region 规格
                    string specification = dt.Rows[i][4].ToString();
                    decimal tempSpecification;
                    if (!string.IsNullOrEmpty(specification))
                    {
                        if (decimal.TryParse(specification, out tempSpecification))
                            item.Specification = tempSpecification.ToString();
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,规格必须为数字(保留两位小数)！</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"第{0}行导入失败,规格不能为空！</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region 规格单位
                    string specificationunit = dt.Rows[i][5].ToString();
                    if (!string.IsNullOrEmpty(specificationunit))
                    {
                        var data = new DataItemCache().ToItemValue("ChemicalsUnit", specificationunit);
                        if (data != null && !string.IsNullOrEmpty(data))
                        {
                            item.SpecificationUnit = specificationunit;
                            if (specificationunit.IndexOf("/") >= 0)
                            {
                                var index = specificationunit.LastIndexOf("/");
                                item.AmountUnit = specificationunit.Substring(index + 1);//数量单位
                                item.Unit = specificationunit.Substring(0, index);//库存单位
                            }
                        }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,规格单位不存在！</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"第{0}行导入失败,规格单位不能为空！</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region 数量
                    string amount = dt.Rows[i][6].ToString();
                    decimal tempAmount;
                    if (!string.IsNullOrEmpty(amount))
                    {
                        if (decimal.TryParse(amount, out tempAmount))
                        {
                            item.Amount = tempAmount.ToString();
                            item.Inventory = (tempSpecification * tempAmount).ToString("#0.00");
                        }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,数量必须为数字(保留两位小数)！</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"第{0}行导入失败,数量不能为空！</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region 生产日期
                    string productionDate = dt.Rows[i][7].ToString();
                    DateTime tempProductionDate;
                    if (!string.IsNullOrEmpty(productionDate))
                        if (DateTime.TryParse(productionDate, out tempProductionDate))
                            item.ProductionDate = tempProductionDate;
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,生产日期不对！</br>", order);
                            error++;
                            continue;
                        }
                    else {
                        falseMessage += string.Format(@"第{0}行导入失败,生产日期不能为空！</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region 存放日期
                    string depositDate = dt.Rows[i][8].ToString();
                    DateTime tempDepositDate;
                    if (!string.IsNullOrEmpty(depositDate))
                        if (DateTime.TryParse(depositDate, out tempDepositDate))
                            item.DepositDate = tempDepositDate;
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,入库日期不对！</br>", order);
                            error++;
                            continue;
                        }
                    else
                    {
                        falseMessage += string.Format(@"第{0}行导入失败,入库日期不能为空！</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region 存放地点类型
                    string isscene = dt.Rows[i][9].ToString();
                    if (!string.IsNullOrEmpty(isscene))
                    {
                        if (isscene == "现场存放" || isscene == "仓库存放")
                        {

                            item.IsScene = isscene;
                        }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,存放地点类型不存在！</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"第{0}行导入失败,存放地点类型不能为空！</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region 存放地点
                    string site = dt.Rows[i][10].ToString();
                    if (!string.IsNullOrEmpty(site))
                    {
                         item.Site = site;     
                    }
                    else
                    {
                        falseMessage += string.Format(@"第{0}行导入失败,存放地点不能为空！</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region 存放期限
                    string deadline = dt.Rows[i][11].ToString();
                    int tempDeadline;
                    if (!string.IsNullOrEmpty(deadline))
                    {
                        if (int.TryParse(deadline, out tempDeadline))
                            item.Deadline = tempDeadline;
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,存放期限必须为数字！</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"第{0}行导入失败,存放期限不能为空！</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region 发放人
                    string Person = dt.Rows[i][12].ToString();
                    if (!string.IsNullOrEmpty(Person))
                    {
                        var userEntity = userBLL.GetListForCon(x => x.RealName == Person).FirstOrDefault();
                        if (userEntity != null)
                        {
                            item.GrantPersonId = userEntity.UserId;
                            item.GrantPerson = Person;
                        }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,发放人不存在！</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"第{0}行导入失败,发放人不能为空！</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region 责任人
                    string dutyUser = dt.Rows[i][13].ToString();
                    if (!string.IsNullOrEmpty(dutyUser))
                    {
                        var userEntity = userBLL.GetListForCon(x => x.RealName == dutyUser).FirstOrDefault();
                        if (userEntity != null)
                        {
                            item.DutyUserId = userEntity.UserId;
                            item.DutyUser = dutyUser;
                        }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,责任人不存在！</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"第{0}行导入失败,责任人不能为空！</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region 责任部门
                    string deptlist = dt.Rows[i][14].ToString();
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
                                    falseMessage += "</br>" + "第" + (i + 3) + "行部门不存在,未能导入.";
                                    error++;
                                    deptFlag = true;
                                    break;
                                }
                                else
                                {
                                    item.DutyDept = entity1.FullName;
                                    item.DutyDeptCode = entity1.EnCode;
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
                                            falseMessage += "</br>" + "第" + (i + 3) + "行部门不存在,未能导入.";
                                            error++;
                                            deptFlag = true;
                                            break;
                                        }
                                        else
                                        {
                                            item.DutyDept = entity.FullName;
                                            item.DutyDeptCode = entity.EnCode;
                                            p1 = entity.DepartmentId;
                                        }
                                    }
                                    else
                                    {
                                        item.DutyDept = entity.FullName;
                                        item.DutyDeptCode = entity.EnCode;
                                        p1 = entity.DepartmentId;
                                    }
                                }
                                else
                                {
                                    item.DutyDept = entity.FullName;
                                    item.DutyDeptCode = entity.EnCode;
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
                                    falseMessage += "</br>" + "第" + (i + 3) + "行专业/班组不存在,未能导入.";
                                    error++;
                                    deptFlag = true;
                                    break;
                                }
                                else
                                {
                                    item.DutyDept = entity1.FullName;
                                    item.DutyDeptCode = entity1.EnCode;
                                    p2 = entity1.DepartmentId;
                                }
                            }
                            else
                            {
                                item.DutyDept = entity1.FullName;
                                item.DutyDeptCode = entity1.EnCode;
                                p2 = entity1.DepartmentId;
                            }

                        }
                        else
                        {
                            var entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && (x.Nature == "班组" || x.Nature == "承包商" || x.Nature == "分包商") && x.FullName == array[j].ToString() && x.ParentId == p2).FirstOrDefault();
                            if (entity1 == null)
                            {
                                falseMessage += "</br>" + "第" + (i + 3) + "行班组不存在,未能导入.";
                                error++;
                                deptFlag = true;
                                break;
                            }
                            else
                            {
                                item.DutyDept = entity1.FullName;
                                item.DutyDeptCode = entity1.EnCode;
                            }
                        }
                    }
                    if (deptFlag) continue;
                    #endregion

                    try
                    {
                        DangerChemicalsBll.SaveForm("", item);
                    }
                    catch
                    {
                        error++;
                    }

                }
                count = dt.Rows.Count-1;
                message = "共有" + count + "条记录,成功导入" + (count - error) + "条，失败" + error + "条";
                message += "</br>" + falseMessage;
            }

            return message;
        }
        #endregion
    }
}
