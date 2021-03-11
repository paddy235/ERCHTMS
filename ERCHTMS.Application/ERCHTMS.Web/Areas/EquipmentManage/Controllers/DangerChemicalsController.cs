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
    /// �� ����Σ�ջ�ѧƷ���
    /// </summary>
    public class DangerChemicalsController : MvcControllerBase
    {
        private UserBLL userbll = new UserBLL(); //�û���������
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private DangerChemicalsBLL DangerChemicalsBll = new DangerChemicalsBLL();
        private readonly DepartmentBLL departBLL = new DepartmentBLL();
        private readonly UserBLL userBLL = new UserBLL();
        private readonly OrganizeBLL orgBLL = new OrganizeBLL();
        private DepartmentBLL departmentBLL = new DepartmentBLL();

        #region ��ͼ����
        /// <summary>
        /// �б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }
        /// <summary>
        /// ����ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }
        #endregion

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�(������)
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {

            pagination.p_kid = "id";
            pagination.p_fields = @"createdate,createuserid,createuserdeptcode,createuserorgcode,modifydate,modifyuserid,MAXNUM,
Name,Alias,Cas,Specification,Inventory,Unit,RiskType,ProductionDate,DepositDate,Site,Deadline,DutyUser,DutyUserId,DutyDept,DutyDeptCode,IsScene,
SpecificationUnit,Amount,AmountUnit";
            pagination.p_tablename = @"XLD_DANGEROUSCHEMICAL";
            pagination.conditionJson = string.Format(" (IsDelete<>1 or IsDelete is null) ");
            //pagination.sidx = "createdate";//�����ֶ�
            //pagination.sord = "desc";//����ʽ  

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
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = DangerChemicalsBll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            DangerChemicalsBll.RemoveForm(keyValue);//ɾ������
            //new PlanDetailsBLL().RemoveFormByApplyId(keyValue);//ɾ������
            //new PlanCheckBLL().RemoveForm(keyValue);//ɾ�����           

            return Success("ɾ���ɹ���");
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
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
                                SendUserName = "ϵͳ����Ա",
                                Title = "Σ��Ʒ��������",
                                Content = string.Format("���ã����ݱ���λ��Σ�ջ�ѧƷ����취����Σ��Ʒ�Ĵ���һ�㲻Ӧ���ڰ���������������ʹ�ò��ţ���λ������Σ��Ʒ�ɹ��ƻ�ʱ����ֿ���������Ҫ�Ͳ��������������"),
                                Category = "����"
                            };
                            new MessageBLL().SaveForm("", msg);
                        }
                    }
                }
                
            }
            catch { }
            DangerChemicalsBll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        /// <summary>
        /// �ύ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, DangerChemicalsEntity entity)
        {
            return Success("�����ɹ���");
        }
        #endregion

        #region ���ݵ���
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "����Σ��Ʒ")]
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
            //pagination.sidx = "createdate";//�����ֶ�
            //pagination.sord = "desc";//����ʽ  
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                pagination.conditionJson += string.Format(" and CREATEUSERORGCODE ='{0}'", user.OrganizeCode);
            }

            var watch = CommonHelper.TimerStart();
            var data = DangerChemicalsBll.GetList(pagination, queryJson);

            //���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "Σ��Ʒ���";
            excelconfig.TitleFont = "΢���ź�";
            excelconfig.TitlePoint = 16;
            excelconfig.FileName = "Σ��Ʒ���.xls";
            excelconfig.IsAllSizeColumn = true;
            //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity = listColumnEntity;
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "idx", ExcelColumn = "���", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "name", ExcelColumn = "����", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "specification", ExcelColumn = "���", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "amount", ExcelColumn = "����", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "inventory", ExcelColumn = "�������", Alignment = "center" });

            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "maxnum", ExcelColumn = "���洢��", Alignment = "center" });

            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "risktype", ExcelColumn = "Σ��Ʒ����", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "deadline", ExcelColumn = "�������", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "site", ExcelColumn = "��ŵص�", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "dutydept", ExcelColumn = "���β���", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "dutyuser", ExcelColumn = "������", Alignment = "center" });

            //���õ�������
            ExcelHelper.ExcelDownload(data, excelconfig);

            return Success("�����ɹ���");
        }
        #endregion

        #region ���ݵ���
        /// <summary>
        /// �����ص����λ
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
                return "��������Ա�޴˲���Ȩ��";
            }
            string orgId = OperatorProvider.Provider.Current().OrganizeId;//������˾ 
            var currUser = OperatorProvider.Provider.Current();
            int error = 0;
            string message = "��ѡ���ʽ��ȷ���ļ��ٵ���!";
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
                    #region Σ��Ʒ����
                    string name = dt.Rows[i][0].ToString();
                    if (!string.IsNullOrEmpty(name))
                    {
                        item.Name = name;
                    }
                    else
                    {
                        falseMessage += string.Format(@"��{0}�е���ʧ��,Σ��Ʒ���Ʋ���Ϊ�գ�</br>", order);
                        error++;
                        continue;
                    }
                    #endregion
                    //����
                    string alias = dt.Rows[i][1].ToString();
                    if (!string.IsNullOrEmpty(alias))
                    {
                        item.Alias = alias;
                    }
                    //CAS��
                    string cas = dt.Rows[i][2].ToString();
                    if (!string.IsNullOrEmpty(cas))
                    {
                        item.Cas = cas;
                    }
                    #region Σ��Ʒ����
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
                            falseMessage += string.Format(@"��{0}�е���ʧ��,Σ��Ʒ���Ͳ����ڣ�</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"��{0}�е���ʧ��,Σ��Ʒ���Ͳ���Ϊ�գ�</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region ���
                    string specification = dt.Rows[i][4].ToString();
                    decimal tempSpecification;
                    if (!string.IsNullOrEmpty(specification))
                    {
                        if (decimal.TryParse(specification, out tempSpecification))
                            item.Specification = tempSpecification.ToString();
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,������Ϊ����(������λС��)��</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"��{0}�е���ʧ��,�����Ϊ�գ�</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region ���λ
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
                                item.AmountUnit = specificationunit.Substring(index + 1);//������λ
                                item.Unit = specificationunit.Substring(0, index);//��浥λ
                            }
                        }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,���λ�����ڣ�</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"��{0}�е���ʧ��,���λ����Ϊ�գ�</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region ����
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
                            falseMessage += string.Format(@"��{0}�е���ʧ��,��������Ϊ����(������λС��)��</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"��{0}�е���ʧ��,��������Ϊ�գ�</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region ��������
                    string productionDate = dt.Rows[i][7].ToString();
                    DateTime tempProductionDate;
                    if (!string.IsNullOrEmpty(productionDate))
                        if (DateTime.TryParse(productionDate, out tempProductionDate))
                            item.ProductionDate = tempProductionDate;
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,�������ڲ��ԣ�</br>", order);
                            error++;
                            continue;
                        }
                    else {
                        falseMessage += string.Format(@"��{0}�е���ʧ��,�������ڲ���Ϊ�գ�</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region �������
                    string depositDate = dt.Rows[i][8].ToString();
                    DateTime tempDepositDate;
                    if (!string.IsNullOrEmpty(depositDate))
                        if (DateTime.TryParse(depositDate, out tempDepositDate))
                            item.DepositDate = tempDepositDate;
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,������ڲ��ԣ�</br>", order);
                            error++;
                            continue;
                        }
                    else
                    {
                        falseMessage += string.Format(@"��{0}�е���ʧ��,������ڲ���Ϊ�գ�</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region ��ŵص�����
                    string isscene = dt.Rows[i][9].ToString();
                    if (!string.IsNullOrEmpty(isscene))
                    {
                        if (isscene == "�ֳ����" || isscene == "�ֿ���")
                        {

                            item.IsScene = isscene;
                        }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,��ŵص����Ͳ����ڣ�</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"��{0}�е���ʧ��,��ŵص����Ͳ���Ϊ�գ�</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region ��ŵص�
                    string site = dt.Rows[i][10].ToString();
                    if (!string.IsNullOrEmpty(site))
                    {
                         item.Site = site;     
                    }
                    else
                    {
                        falseMessage += string.Format(@"��{0}�е���ʧ��,��ŵص㲻��Ϊ�գ�</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region �������
                    string deadline = dt.Rows[i][11].ToString();
                    int tempDeadline;
                    if (!string.IsNullOrEmpty(deadline))
                    {
                        if (int.TryParse(deadline, out tempDeadline))
                            item.Deadline = tempDeadline;
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,������ޱ���Ϊ���֣�</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"��{0}�е���ʧ��,������޲���Ϊ�գ�</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region ������
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
                            falseMessage += string.Format(@"��{0}�е���ʧ��,�����˲����ڣ�</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"��{0}�е���ʧ��,�����˲���Ϊ�գ�</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region ������
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
                            falseMessage += string.Format(@"��{0}�е���ʧ��,�����˲����ڣ�</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"��{0}�е���ʧ��,�����˲���Ϊ�գ�</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region ���β���
                    string deptlist = dt.Rows[i][14].ToString();
                    var p1 = string.Empty; var p2 = string.Empty;
                    var array = deptlist.Split('/');
                    var deptFlag = false;
                    for (int j = 0; j < array.Length; j++)
                    {
                        if (j == 0)
                        {
                            if (currUser.RoleName.Contains("ʡ��") || currUser.RoleName.Contains("����"))
                            {
                                var entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.FullName == array[j].ToString()).FirstOrDefault();
                                if (entity1 == null)
                                {
                                    falseMessage += "</br>" + "��" + (i + 3) + "�в��Ų�����,δ�ܵ���.";
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
                                var entity = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "����" && x.FullName == array[j].ToString()).FirstOrDefault();
                                if (entity == null)
                                {
                                    entity = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && (x.Nature == "����" || x.Nature == "�а���" || x.Nature == "�ְ���") && x.FullName == array[j].ToString()).FirstOrDefault();
                                    if (entity == null)
                                    {
                                        entity = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "�а���" && x.FullName == array[j].ToString()).FirstOrDefault();
                                        if (entity == null)
                                        {
                                            falseMessage += "</br>" + "��" + (i + 3) + "�в��Ų�����,δ�ܵ���.";
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
                            var entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && (x.Nature == "רҵ" || x.Nature == "�а���" || x.Nature == "�ְ���") && x.FullName == array[j].ToString() && x.ParentId == p1).FirstOrDefault();
                            if (entity1 == null)
                            {
                                entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "����" && x.FullName == array[j].ToString() && x.ParentId == p1).FirstOrDefault();
                                if (entity1 == null)
                                {
                                    falseMessage += "</br>" + "��" + (i + 3) + "��רҵ/���鲻����,δ�ܵ���.";
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
                            var entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && (x.Nature == "����" || x.Nature == "�а���" || x.Nature == "�ְ���") && x.FullName == array[j].ToString() && x.ParentId == p2).FirstOrDefault();
                            if (entity1 == null)
                            {
                                falseMessage += "</br>" + "��" + (i + 3) + "�а��鲻����,δ�ܵ���.";
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
                message = "����" + count + "����¼,�ɹ�����" + (count - error) + "����ʧ��" + error + "��";
                message += "</br>" + falseMessage;
            }

            return message;
        }
        #endregion
    }
}
