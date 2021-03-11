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
    /// �� ������������
    /// </summary>
    public class FireTroopsController : MvcControllerBase
    {
        private FireTroopsBLL FireTroopsbll = new FireTroopsBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private readonly UserBLL userBLL = new UserBLL();
        private readonly OrganizeBLL orgBLL = new OrganizeBLL();
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private UserBLL userbll = new UserBLL();
        #region ��ͼ����
        /// <summary>
        /// �б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.ehsDepartCode = "";
            //��ǰ�û�
            Operator curUser = OperatorProvider.Provider.Current();
            DataItemModel ehsDepart = dataitemdetailbll.GetDataItemListByItemCode("'EHSDepartment'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();
            if (ehsDepart != null)
                ViewBag.ehsDepartCode = ehsDepart.ItemValue;
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
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
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
                if (dr["nature"].ToString() == "רҵ" || dr["nature"].ToString() == "����")
                {
                    //DataTable dt = departmentBLL.GetDataTable(string.Format("select fullname from BASE_DEPARTMENT where encode=(select encode from BASE_DEPARTMENT t where instr('{0}',encode)=1 and nature='{1}' and organizeid='{2}') or encode='{0}' order by deptcode", dr["deptcode"], "����", dr["createuserorgcode"]));
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
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = FireTroopsbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = FireTroopsbll.GetEntity(keyValue);
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
            FireTroopsbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, FireTroopsEntity entity)
        {
            FireTroopsbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
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

        #region ���ݵ���
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "����רְ��������")]
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
            pagination.sidx = "SortCode";//�����ֶ�
            pagination.sord = "asc";//����ʽ  
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                pagination.conditionJson += string.Format(" and CREATEUSERORGCODE ='{0}'", user.OrganizeCode);
            }

            var watch = CommonHelper.TimerStart();
            var data = FireTroopsbll.GetPageList(pagination, queryJson);

            //���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "רְ��������";
            excelconfig.TitleFont = "΢���ź�";
            excelconfig.TitlePoint = 16;
            excelconfig.FileName = "רְ��������.xls";
            excelconfig.IsAllSizeColumn = true;
            //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity = listColumnEntity;
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "idx", ExcelColumn = "���", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "username", ExcelColumn = "����", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "quarters", ExcelColumn = "ְ��", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "sex", ExcelColumn = "�Ա�", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "identitycard", ExcelColumn = "���֤��", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "degreesid", ExcelColumn = "ѧ��", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "certificates", ExcelColumn = "��֤���", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "placedomicile", ExcelColumn = "�������ڵ�", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "phone", ExcelColumn = "��ϵ��ʽ", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "dept", ExcelColumn = "��������", Alignment = "center" });

            //���õ�������
            ExcelHelper.ExcelDownload(data, excelconfig);

            return Success("�����ɹ���");
        }

        /// <summary>
        /// ����
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
                int order = 2;
                string orgid = OperatorProvider.Provider.Current().OrganizeId;
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    FireTroopsEntity item = new FireTroopsEntity();
                    order = i + 1;
                    #region ���
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
                            falseMessage += string.Format(@"��{0}�е���ʧ��,��ű���Ϊ������</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"��{0}�е���ʧ��,��Ų���Ϊ�գ�</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region ���β���
                    string deptlist = dt.Rows[i][1].ToString();
                    if (string.IsNullOrEmpty(deptlist))
                    {
                        falseMessage += string.Format(@"��{0}�е���ʧ��,��λ�����ţ�����Ϊ�գ�</br>", order);
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
                            if (currUser.RoleName.Contains("ʡ��") || currUser.RoleName.Contains("����"))
                            {
                                var entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.FullName == array[j].ToString()).FirstOrDefault();
                                if (entity1 == null)
                                {
                                    //falseMessage += "</br>" + "��" + (i + 3) + "�в��Ų�����,δ�ܵ���.";
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
                                var entity = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "����" && x.FullName == array[j].ToString()).FirstOrDefault();
                                if (entity == null)
                                {
                                    entity = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && (x.Nature == "����" || x.Nature == "�а���" || x.Nature == "�ְ���") && x.FullName == array[j].ToString()).FirstOrDefault();
                                    if (entity == null)
                                    {
                                        entity = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "�а���" && x.FullName == array[j].ToString()).FirstOrDefault();
                                        if (entity == null)
                                        {
                                            //falseMessage += "</br>" + "��" + (i + 3) + "�в��Ų�����,δ�ܵ���.";
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
                            var entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && (x.Nature == "רҵ" || x.Nature == "�а���" || x.Nature == "�ְ���") && x.FullName == array[j].ToString() && x.ParentId == p1).FirstOrDefault();
                            if (entity1 == null)
                            {
                                entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "����" && x.FullName == array[j].ToString() && x.ParentId == p1).FirstOrDefault();
                                if (entity1 == null)
                                {
                                    //falseMessage += "</br>" + "��" + (i + 3) + "��רҵ/���鲻����,δ�ܵ���.";
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
                            var entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && (x.Nature == "����" || x.Nature == "�а���" || x.Nature == "�ְ���") && x.FullName == array[j].ToString() && x.ParentId == p2).FirstOrDefault();
                            if (entity1 == null)
                            {
                                //falseMessage += "</br>" + "��" + (i + 3) + "�а��鲻����,δ�ܵ���.";
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

                    #region ����
                    string username = dt.Rows[i][2].ToString().Trim();
                    if (string.IsNullOrEmpty(username))
                    {
                        falseMessage += string.Format(@"��{0}�е���ʧ��,��������Ϊ�գ�</br>", order);
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
                                falseMessage += string.Format(@"��{0}��,�����ڡ�{1}�������ڣ�</br>", order, item.Dept);
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
                    

                    #region ְ��
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
                            falseMessage += string.Format(@"��{0}�е���ʧ��,ְ�񲻴��ڣ�</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"��{0}�е���ʧ��,ְ����Ϊ�գ�</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region �Ա�
                    item.Sex = dt.Rows[i][4].ToString().Trim();
                    #endregion

                    #region ���֤��
                    //���֤��
                    string identity = dt.Rows[i][5].ToString().Trim();
                    if (!string.IsNullOrEmpty(identity)) {
                        if (!Regex.IsMatch(identity, @"^(^d{15}$|^\d{18}$|^\d{17}(\d|X|x))$", RegexOptions.IgnoreCase))
                        {
                            falseMessage += string.Format(@"��{0}�����֤�Ÿ�ʽ����</br>", order);
                            error++;
                            continue;
                        }
                        else {
                            item.IdentityCard = identity;
                        }
                    }
                    #endregion

                    #region ѧ��
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
                            falseMessage += string.Format(@"��{0}�е���ʧ��,ѧ�������ڣ�</br>", order);
                            error++;
                            continue;
                        }
                    }
                    #endregion

                    #region ��֤���
                    string certificates = dt.Rows[i][7].ToString().Trim();
                    if (!string.IsNullOrEmpty(certificates))
                    {
                        if (certificates.Length > 100)
                        {
                            falseMessage += string.Format(@"��{0}�г�֤����ַ�������</br>", order);
                            error++;
                            continue;
                        }
                        else
                        {
                            item.Certificates = certificates;
                        }
                    }
                    #endregion

                    #region �������ڵ�
                    string placedomicile = dt.Rows[i][8].ToString().Trim();
                    if (!string.IsNullOrEmpty(placedomicile))
                    {
                        item.PlaceDomicile = placedomicile;
                    }
                    #endregion

                    #region �ֻ���
                    string phone = dt.Rows[i][9].ToString().Trim();
                    if (!string.IsNullOrEmpty(phone))
                    {
                        if (!Regex.IsMatch(phone, @"^(\+\d{2,3}\-)?\d{11}$", RegexOptions.IgnoreCase))
                        {
                            falseMessage += string.Format(@"��{0}���ֻ��Ÿ�ʽ����ȷ��</br>", order);
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
                message = "����" + count + "����¼,�ɹ�����" + (count - error) + "����ʧ��" + error + "��";
                message += "</br>" + falseMessage;
            }

            return message;
        }
        /// <summary>
        /// ��֤���֤�Ƿ�Ϸ�
        /// </summary>
        /// <param name="IdCard"></param>
        /// <param name="error"></param>
        /// <param name="sbirthday"></param>
        /// <returns></returns>
        public bool CheckIdCard(string IdCard, out string error, out string sbirthday)
        {
            //var aCity ={ 11: "����", 12: "���", 13: "�ӱ�", 14: "ɽ��", 15: "���ɹ�", 21: "����", 22: "����", 23: "������", 31: "�Ϻ�", 32: "����", 33: "�㽭", 34: "����", 35: "����", 36: "����", 37: "ɽ��", 41: "����", 42: "����", 43: "����", 44: "�㶫", 45: "����", 46: "����", 50: "����", 51: "�Ĵ�", 52: "����", 53: "����", 54: "����", 61: "����", 62: "����", 63: "�ຣ", 64: "����", 65: "�½�", 71: "̨��", 81: "���", 82: "����", 91: "����" };
            List<int> aCity = new List<int>() { 11, 12, 13, 14, 15, 21, 22, 23, 31, 32, 33, 34, 35, 36, 37, 41, 42, 43, 44, 45, 46, 50, 51, 52, 53, 54, 61, 62, 63, 64, 65, 71, 81, 82, 91 };
            error = "";
            sbirthday = "";
            var iSum = 0.0;
            if (!Regex.IsMatch(IdCard, @"^(^d{15}$|^\d{18}$|^\d{17}(\d|X|x))$", RegexOptions.IgnoreCase))
            {
                error = "����������֤���Ȼ��ʽ����";
                return false;
            }
            //IdCard = IdCard.replace(/x$/i, "a");
            if (!aCity.Contains(Convert.ToInt32(IdCard.Substring(0, 2))))
            {
                error = "������֤�����Ƿ�";
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
                        error = "���֤�ϵĳ������ڷǷ�";
                        return false;
                    }

                }
            }
            catch
            {
                error = "���֤������";
                return false;
            }
            //for (var i = 17; i >= 0; i--) {
            //    iSum += (Math.Pow(2, i) % 11) * Convert.ToInt32(IdCard.ToArray()[17 - i].ToInt().ToString(), 11);
            //}
            //if (iSum % 11 != 1) {
            //    error = "����������֤�ŷǷ�";
            //    return false;
            //}

            return true;
        }
        #endregion
    }
}
