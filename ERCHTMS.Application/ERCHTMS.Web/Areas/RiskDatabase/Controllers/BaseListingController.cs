using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.Busines.RiskDatabase;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System;
using System.Linq;
using System.Data;
using ERCHTMS.Code;
using System.Web;
using ERCHTMS.Busines.BaseManage;
using System.Linq.Expressions;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Web.Areas.RiskDatabase.Controllers
{
    /// <summary>
    /// �� ������ҵ����豸��ʩ�嵥
    /// </summary>
    public class BaseListingController : MvcControllerBase
    {
        private BaseListingBLL baselistingbll = new BaseListingBLL();
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private PostBLL postBLL = new PostBLL();

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
        /// ѡ���嵥
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Select()
        {
            return View();
        }
        /// <summary>
        /// ����
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
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "id";
                pagination.p_fields = "name,name as equname,activitystep,isconventional,isspecialequ,areaname,areacode,areaid,others,case when b.num>0 then 0 else 1 end as status,b.num as evaluatenum,createuserdeptcode,c.fullname as createuserdeptname,a.createdate,post";
                pagination.p_tablename = "bis_baselisting a left join (select count(1) as num,listingid from bis_riskassess where status=1 and deletemark=0 and enabledmark=0 group by listingid) b on a.id=b.listingid left join base_department c on a.createuserdeptcode=c.encode";
                pagination.conditionJson = "1=1";
                var data = baselistingbll.GetPageListJson(pagination, queryJson);
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
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        
        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = baselistingbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        public ActionResult GetNameData(string query,int type)
        {
            try
            {
                Expression<Func<BaseListingEntity, bool>> condition = t => t.Name.Contains(query) && t.Type == type;
                var data = baselistingbll.GetList(condition).Select(t => t.Name).Distinct().ToArray();
                return ToJsonResult(data);
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
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
            try
            {
                baselistingbll.RemoveForm(keyValue);
                return Success("ɾ���ɹ���");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
            
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
        public ActionResult SaveForm(string keyValue, BaseListingEntity entity)
        {
            try
            {
                //---****�ж�ϵͳ�Ƿ��Ѿ����ڸ���ҵ�������������*****--
                Expression<Func<BaseListingEntity, bool>> condition = t => t.Name == entity.Name && t.ActivityStep == entity.ActivityStep && t.Type == 0 && t.PostId == entity.PostId;
                if (baselistingbll.GetList(condition).Count() > 0)
                {
                    return Error("���ڸ���ҵ�������衢��λ�����ݣ��޷��ظ���ӡ�");
                }
                baselistingbll.SaveForm(keyValue, entity);
                return Success("�����ɹ���");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
            
        }

        [HttpPost]
        public string ImportData()
        {
            try
            {
                if (OperatorProvider.Provider.Current().IsSystem)
                {
                    return "��������Ա�޴˲���Ȩ��";
                }
                var currUser = OperatorProvider.Provider.Current();
                string orgId = OperatorProvider.Provider.Current().OrganizeId;//������˾

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
                    Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                    wb.Open(Server.MapPath("~/Resource/temp/" + fileName));
                    Aspose.Cells.Cells cells = wb.Worksheets[0].Cells;
                    DataTable dt = new DataTable();
                    if (cells.MaxDataRow > 1)
                    {
                        dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn + 1, true);
                        #region ��ҵ���
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            //����������֤�����
                            string deptlist = dt.Rows[i]["���λ"].ToString().Trim();

                            string controlDept = currUser.DeptName;//�ܿز���
                            string controlDeptId = currUser.DeptId;//�ܿز���
                            string controlDeptCode = currUser.DeptCode;//�ܿز���

                            //��λ�����֣�
                            string Post = dt.Rows[i]["��λ�����֣�"].ToString().Trim();
                            string PostId = string.Empty;
                            //��ҵ�
                            string Name = dt.Rows[i]["��ҵ�"].ToString().Trim();
                            //�����
                            string ActivityStep = dt.Rows[i]["�����"].ToString().Trim();
                            //����/�ǳ���
                            string IsConventional = dt.Rows[i]["����/�ǳ���"].ToString().Trim();
                            //����
                            string Others = dt.Rows[i]["����"].ToString().Trim();



                            //---****ֵ���ڿ���֤*****--
                            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(ActivityStep) || string.IsNullOrEmpty(IsConventional) || string.IsNullOrWhiteSpace(Post))
                            {
                                falseMessage += "</br>" + "��" + (i + 2) + "��ֵ���ڿ�,δ�ܵ���.";
                                error++;
                                continue;
                            }
                            var p1 = string.Empty;
                            var p2 = string.Empty;
                            bool isSkip = false;
                            //��֤������Ƿ����
                            if (!string.IsNullOrWhiteSpace(deptlist))
                            {
                                var array = deptlist.Split('/');
                                for (int j = 0; j < array.Length; j++)
                                {
                                    if (j == 0)
                                    {
                                        var entity = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "����" && x.FullName == array[j].ToString()).FirstOrDefault();
                                        if (entity == null)
                                        {
                                            entity = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "����" && x.FullName == array[j].ToString()).FirstOrDefault();
                                            if (entity == null)
                                            {
                                                entity = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "�а���" && x.FullName == array[j].ToString()).FirstOrDefault();
                                                if (entity == null)
                                                {
                                                    falseMessage += "</br>" + "��" + (i + 2) + "�в�����Ϣ������,δ�ܵ���.";
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
                                    else if (j == 1)
                                    {
                                        var entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && (x.Nature == "רҵ" || x.Nature == "�а���" || x.Nature == "�ְ���") && x.FullName == array[j].ToString() && x.ParentId == p1).FirstOrDefault();
                                        if (entity1 == null)
                                        {
                                            entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "����" && x.FullName == array[j].ToString() && x.ParentId == p1).FirstOrDefault();
                                            if (entity1 == null)
                                            {
                                                falseMessage += "</br>" + "��" + (i + 2) + "�в�����Ϣ������,δ�ܵ���.";
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
                                        var entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && (x.Nature == "����" || x.Nature == "�а���" || x.Nature == "�ְ���") && x.FullName == array[j].ToString() && x.ParentId == p2).FirstOrDefault();
                                        if (entity1 == null)
                                        {
                                            falseMessage += "</br>" + "��" + (i + 2) + "�в�����Ϣ������,δ�ܵ���.";
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

                            //��֤��λ�ǲ����ڲ�������
                            var controldept = departmentBLL.GetEntity(controlDeptId);
                            RoleEntity re = new RoleEntity();
                            if (controldept.Nature == "����")
                            {
                                re = postBLL.GetList().Where(a => (a.FullName == Post && a.OrganizeId == orgId)).FirstOrDefault();
                            }
                            else
                            {
                                re = postBLL.GetList().Where(a => (a.FullName == Post && a.OrganizeId == orgId && a.DeptId == controlDeptId)).FirstOrDefault();
                            }
                            if (re == null)
                            {
                                falseMessage += "</br>" + "��" + (i + 2) + "�и�λ����,δ�ܵ���.";
                                error++;
                                continue;
                            }
                            else
                            {
                                PostId = re.RoleId;
                            }
                            //---****�ж�ϵͳ�Ƿ��Ѿ����ڸ���ҵ�������������*****--
                            Expression<Func<BaseListingEntity, bool>> condition = t => t.Name == Name && t.ActivityStep == ActivityStep && t.Type == 0 && t.PostId == PostId;
                            if (baselistingbll.GetList(condition).Count() > 0)
                            {
                                falseMessage += "</br>" + "��" + (i + 2) + "�������Ѿ�������ϵͳ�У��������.";
                                error++;
                                continue;
                            }
                            BaseListingEntity Listingentity = new BaseListingEntity();
                            Listingentity.Name = Name;
                            Listingentity.ActivityStep = ActivityStep;
                            Listingentity.IsConventional = IsConventional == "����" ? 0 : 1;
                            Listingentity.Others = Others;
                            Listingentity.CreateUserDeptCode = string.IsNullOrWhiteSpace(deptlist) ? currUser.DeptCode : controlDeptCode;
                            Listingentity.ControlsDept = controlDept;
                            Listingentity.ControlsDeptId = controlDeptId;
                            Listingentity.ControlsDeptCode = controlDeptCode;
                            Listingentity.Type = 0;
                            Listingentity.Post = Post;
                            Listingentity.PostId = PostId;
                            condition = t => t.Name == Name && !(t.AreaName == null || t.AreaName.Trim() == "");
                            var defualt = baselistingbll.GetList(condition).ToList().FirstOrDefault();
                            Listingentity.AreaName = defualt == null ? "" : defualt.AreaName;
                            Listingentity.AreaId = defualt == null ? "" : defualt.AreaId;
                            Listingentity.AreaCode = defualt == null ? "" : defualt.AreaCode;
                            Listingentity.CreateDate = DateTime.Now.AddSeconds(i);
                            baselistingbll.SaveForm("", Listingentity);
                        }
                        count = dt.Rows.Count;
                        message = "��ҵ��๲��" + count + "����¼,�ɹ�����" + (count - error) + "����ʧ��" + error + "��";
                        message += "</br>" + falseMessage + "</br>";
                        #endregion
                    }
                    else
                    {
                        message = "��ҵ���û�����ݡ�</br>";
                    }


                   
                    error = 0;
                    falseMessage = "";
                    cells = wb.Worksheets[1].Cells;
                    if (cells.MaxDataRow > 1)
                    {
                        #region �豸��ʩ��
                        dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn + 1, true);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            //����������֤�����
                            string deptlist = dt.Rows[i]["���λ"].ToString().Trim();

                            string controlDept = currUser.DeptName;//�ܿز���
                            string controlDeptId = currUser.DeptId;//�ܿز���
                            string controlDeptCode = currUser.DeptCode;//�ܿز���
                            
                            //�豸����
                            string Name = dt.Rows[i]["�豸����"].ToString().Trim();
                            //���ڵص�
                            string arealist = dt.Rows[i]["���ڵص�"].ToString().Trim();
                            //�Ƿ������豸
                            string IsSpecialEqu = dt.Rows[i]["�Ƿ������豸"].ToString().Trim();
                            //����
                            string Others = dt.Rows[i]["����"].ToString().Trim();
                            string AreaName = string.Empty; //���ڵص�����
                            string AreaId = string.Empty; //���ڵص�Id
                            string AreaCode = string.Empty; //���ڵص�Code



                            //---****ֵ���ڿ���֤*****--
                            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(arealist) || string.IsNullOrEmpty(IsSpecialEqu))
                            {
                                falseMessage += "</br>" + "��" + (i + 2) + "��ֵ���ڿ�,δ�ܵ���.";
                                error++;
                                continue;
                            }
                            var p1 = string.Empty;
                            var p2 = string.Empty;
                            bool isSkip = false;
                            //��֤������Ƿ����
                            if (!string.IsNullOrWhiteSpace(deptlist))
                            {
                                var array = deptlist.Split('/');
                                for (int j = 0; j < array.Length; j++)
                                {
                                    if (j == 0)
                                    {
                                        var entity = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "����" && x.FullName == array[j].ToString()).FirstOrDefault();
                                        if (entity == null)
                                        {
                                            entity = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "����" && x.FullName == array[j].ToString()).FirstOrDefault();
                                            if (entity == null)
                                            {
                                                entity = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "�а���" && x.FullName == array[j].ToString()).FirstOrDefault();
                                                if (entity == null)
                                                {
                                                    falseMessage += "</br>" + "��" + (i + 2) + "�в�����Ϣ������,δ�ܵ���.";
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
                                    else if (j == 1)
                                    {
                                        var entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && (x.Nature == "רҵ" || x.Nature == "�а���" || x.Nature == "�ְ���") && x.FullName == array[j].ToString() && x.ParentId == p1).FirstOrDefault();
                                        if (entity1 == null)
                                        {
                                            entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "����" && x.FullName == array[j].ToString() && x.ParentId == p1).FirstOrDefault();
                                            if (entity1 == null)
                                            {
                                                falseMessage += "</br>" + "��" + (i + 2) + "�в�����Ϣ������,δ�ܵ���.";
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
                                        var entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && (x.Nature == "����" || x.Nature == "�а���" || x.Nature == "�ְ���") && x.FullName == array[j].ToString() && x.ParentId == p2).FirstOrDefault();
                                        if (entity1 == null)
                                        {
                                            falseMessage += "</br>" + "��" + (i + 2) + "�в�����Ϣ������,δ�ܵ���.";
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
                            //��֤���ڵص㣨����
                            var disItem = new DistrictBLL().GetListForCon(x => x.DistrictName == arealist && x.OrganizeId == currUser.OrganizeId).FirstOrDefault();
                            if (disItem != null)
                            {
                                AreaId = disItem.DistrictID;
                                AreaCode = disItem.DistrictCode;
                                AreaName = disItem.DistrictName;
                            }
                            else
                            {
                                falseMessage += "</br>" + "��" + (i + 2) + "�����ڵص���Ϣ��ϵͳ���õ�����һ��,δ�ܵ���.";
                                error++;
                                continue;
                            }
                            //---****�ж�ϵͳ�Ƿ��Ѿ����ڸ��豸���ơ����ڵص������*****--
                            Expression<Func<BaseListingEntity, bool>> condition = t => t.Name == Name && t.AreaId == AreaId && t.Type == 1;
                            if (baselistingbll.GetList(condition).Count() > 0)
                            {
                                falseMessage += "</br>" + "��" + (i + 2) + "�������Ѿ�������ϵͳ�У��������.";
                                error++;
                                continue;
                            }
                            BaseListingEntity Listingentity = new BaseListingEntity();
                            Listingentity.Name = Name;
                            Listingentity.AreaCode = AreaCode;
                            Listingentity.AreaId = AreaId;
                            Listingentity.AreaName = AreaName;
                            Listingentity.IsSpecialEqu = IsSpecialEqu == "��" ? 0 : 1;
                            Listingentity.Others = Others;
                            Listingentity.CreateUserDeptCode = string.IsNullOrWhiteSpace(deptlist) ? currUser.DeptCode : controlDeptCode;
                            Listingentity.ControlsDept = controlDept;
                            Listingentity.ControlsDeptId = controlDeptId;
                            Listingentity.ControlsDeptCode = controlDeptCode;
                            Listingentity.Type = 1;
                            Listingentity.CreateDate = DateTime.Now.AddSeconds(i);
                            baselistingbll.SaveForm("", Listingentity);
                        }
                        count = dt.Rows.Count;
                        message += "�豸��ʩ�๲��" + count + "����¼,�ɹ�����" + (count - error) + "����ʧ��" + error + "��";
                        message += "</br>" + falseMessage;
                        #endregion
                    }
                    else
                    {
                        message += "�豸��ʩ��û�����ݡ�</br>";
                    }
                }
                return message;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            
        }
        /// <summary>
        /// �������չܿ��б�
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult ExportExcel(string queryJson)
        {
            try
            {
                HttpResponse resp = System.Web.HttpContext.Current.Response;
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                Pagination pagination = new Pagination();
                pagination.p_kid = "";
                pagination.p_fields = "'' num1,post,name,activitystep,case when isconventional=0 then '����' else  '�ǳ���' end as isconventional,others as others1,b.fullname as dept1,to_char(a.createdate,'yyyy-MM-dd') as date1,'' num2,name as equname,areaname,case when isspecialequ=0 then '��' else  '��' end as isspecialequ,others as others2��b.fullname as dept2,to_char(a.createdate,'yyyy-MM-dd') as date2,type";
                pagination.p_tablename = "bis_baselisting a left join base_department b on a.createuserdeptcode=b.encode";
                pagination.conditionJson = "1=1";
                pagination.page = 1;
                pagination.rows = 100000;
                if (user.RoleName.Contains("��˾��") || user.RoleName.Contains("����"))
                {
                    pagination.conditionJson += " and a.CREATEUSERORGCODE ='" + user.OrganizeCode + "'";
                }
                else
                {
                    pagination.conditionJson += " and a.createuserdeptcode ='" + user.DeptCode + "'";
                }
                var data = baselistingbll.GetPageListJson(pagination, queryJson);
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                wb.Open(Server.MapPath("~/Resource/ExcelTemplate/��ҵ����豸��ʩ�嵥����ģ��.xls"));
                if (data.Rows.Count > 0)
                {
                    DataTable dt1 = data.Select("type=0").Count() > 0 ? data.Select("type=0").CopyToDataTable() : new DataTable();
                    
                    if (dt1.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            dt1.Rows[i]["num1"] = i + 1;
                        }
                        wb.Worksheets[0].Cells.ImportDataTable(dt1, false, 1, 0, data.Rows.Count, 7);
                    }

                    DataTable dt2 = data.Select("type=1").Count() > 0 ? data.Select("type=1").CopyToDataTable() : new DataTable();

                    if (dt2.Rows.Count > 0)
                    {
                        dt2.Columns.Remove("post"); dt2.Columns.Remove("name"); dt2.Columns.Remove("activitystep"); dt2.Columns.Remove("isconventional"); dt2.Columns.Remove("others1");
                        dt2.Columns.Remove("dept1"); dt2.Columns.Remove("date1"); dt2.Columns.Remove("num1");
                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            dt2.Rows[i]["num2"] = i + 1;
                        }
                        wb.Worksheets[1].Cells.ImportDataTable(dt2, false, 1, 0, data.Rows.Count, 7);
                    }
                }
                wb.Save(Server.UrlEncode("��ҵ����豸��ʩ�嵥.xls"), Aspose.Cells.FileFormatType.Excel2003, Aspose.Cells.SaveType.OpenInBrowser, resp);
                return Success("�����ɹ���");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }

        }
        #endregion
    }
}
