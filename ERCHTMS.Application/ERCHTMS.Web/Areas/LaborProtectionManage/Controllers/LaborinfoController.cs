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
    /// �� �����Ͷ�������Ʒ��
    /// </summary>
    public class LaborinfoController : MvcControllerBase
    {
        private LaborinfoBLL laborinfobll = new LaborinfoBLL();

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
        /// ��ȡ�Ƿ����ѡ����
        /// </summary>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public string GetIsDept()
        {
            var issystem = OperatorProvider.Provider.Current().IsSystem;
            if (!issystem)
            {//�������ϵͳ����Ա
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

                if (OperatorProvider.Provider.Current().RoleName.Contains("��˾���û�"))
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
        /// ��ȡ�Ƿ�ӵ��Ȩ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetPer()
        {
            return laborinfobll.GetPer().ToString();
        }

        /// <summary>
        /// ��ȡ 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
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
                    if (type == "�·�")
                    {
                        laboruser.Size = "L";
                    }
                    else if (type == "Ь��")
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
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
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
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = laborinfobll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #endregion

        #region ��������
        /// <summary>
        /// ������Excel
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

            string FileUrl = @"\Resource\ExcelTemplate\�Ͷ�������Ʒ����_����.xls";
            AsposeExcelHelper.ExecuteResult(dt, FileUrl, "�Ͷ�������Ʒ�����嵥", "�Ͷ�������Ʒ�����б�");

            return Success("�����ɹ���");
        }
        #endregion

        #region �ύ����

        /// <summary>
        /// �����û�
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
            //��ȡ����ѡ����
            List<LaborprotectionEntity> laborlist = laborprotectionbll.GetLaborList();
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
                if (cells.MaxDataRow == 0)
                {
                    message = "û������,��ѡ������дģ���ڽ��е���!";
                    return message;
                }
                DataTable dt = cells.ExportDataTable(0, 0, cells.MaxDataRow + 1, cells.MaxColumn + 1, true);
                int order = 1;

                IList<LaborprotectionEntity> LaborList = new List<LaborprotectionEntity>();

                IEnumerable<DepartmentEntity> deptlist = new DepartmentBLL().GetList();
                OrganizeBLL orgbll = new OrganizeBLL();
                //�Ȼ�ȡ��ԭʼ��һ�����
                string no = laborprotectionbll.GetNo();
                int ysno = Convert.ToInt32(no);

                DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();
                var dataitem = dataItemDetailBLL.GetDataItemListByItemCode("'LaborName'").ToList();
                List<LaborprotectionEntity> insertpro = new List<LaborprotectionEntity>();
                List<LaborinfoEntity> insertinfo = new List<LaborinfoEntity>();
                //�Ȼ�ȡ��Ա
                List<UserEntity> userlist =
                    new UserBLL().GetListForCon(it => it.IsPresence == "1" && it.Account != "System").ToList();
                List<LaborequipmentinfoEntity> eqlist = new List<LaborequipmentinfoEntity>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    order = i;
                    string Name = dt.Rows[i]["����"].ToString();
                    string Model = dt.Rows[i]["�ͺ�"].ToString();
                    string Type = dt.Rows[i]["����"].ToString();
                    string DeptName = dt.Rows[i]["ʹ�ò���"].ToString();
                    string OrgName = dt.Rows[i]["ʹ�õ�λ"].ToString();
                    string PostName = dt.Rows[i]["ʹ�ø�λ"].ToString().Trim();
                    string Unit = dt.Rows[i]["�Ͷ�������Ʒ��λ"].ToString().Trim();
                    string Time = dt.Rows[i]["ʹ������"].ToString().Trim();
                    string TimeType = dt.Rows[i]["ʹ�����޵�λ"].ToString().Trim();
                    string deptId = "", deptCode = "", PostId = "";
                    //---****ֵ���ڿ���֤*****--
                    if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Unit) || string.IsNullOrEmpty(DeptName) || string.IsNullOrEmpty(OrgName) || string.IsNullOrEmpty(PostName))
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "��ֵ���ڿ�,δ�ܵ���.";
                        error++;
                        continue;
                    }

                    //��֤�����ǲ��Ǻ��Լ�һ������
                    DepartmentEntity org = deptlist.Where(it => it.FullName == OrgName).FirstOrDefault();
                    if (org == null)
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "��ʹ�õ�λ���Ʋ�����,δ�ܵ���.";
                        error++;
                        continue;
                    }
                    //�������Ļ���id�ͱ��˵Ļ���id��һ��
                    if (org.DepartmentId != currUser.OrganizeId)
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "��ʹ�õ�λ���ǵ����ߵĵ�λ,δ�ܵ���.";
                        error++;
                        continue;
                    }

                    //��֤������Ƿ����
                    var deptFlag = false;
                    var entity1 = deptlist.Where(x => x.OrganizeId == currUser.OrganizeId && x.FullName == DeptName).FirstOrDefault();
                    if (entity1 == null)
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "��ʹ�ò��Ų�����,δ�ܵ���.";
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
                    //        if (currUser.RoleName.Contains("ʡ��"))
                    //        {
                    //            var entity1 = deptlist.Where(x => x.OrganizeId == currUser.OrganizeId && x.FullName == array[j].ToString()).FirstOrDefault();
                    //            if (entity1 == null)
                    //            {
                    //                falseMessage += "</br>" + "��" + (i + 2) + "�в��Ų�����,δ�ܵ���.";
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
                    //            var entity = deptlist.Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "����" && x.FullName == array[j].ToString()).FirstOrDefault();
                    //            if (entity == null)
                    //            {
                    //                entity = deptlist.Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "����" && x.FullName == array[j].ToString()).FirstOrDefault();
                    //                if (entity == null)
                    //                {
                    //                    entity = deptlist.Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "�а���" && x.FullName == array[j].ToString()).FirstOrDefault();
                    //                    if (entity == null)
                    //                    {
                    //                        falseMessage += "</br>" + "��" + (i + 2) + "�в��Ų�����,δ�ܵ���.";
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
                    //        var entity1 = deptlist.Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "רҵ" && x.FullName == array[j].ToString()).FirstOrDefault();
                    //        if (entity1 == null)
                    //        {
                    //            entity1 = deptlist.Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "����" && x.FullName == array[j].ToString()).FirstOrDefault();
                    //            if (entity1 == null)
                    //            {
                    //                falseMessage += "</br>" + "��" + (i + 2) + "��רҵ/���鲻����,δ�ܵ���.";
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
                    //        var entity1 = deptlist.Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "����" && x.FullName == array[j].ToString()).FirstOrDefault();
                    //        if (entity1 == null)
                    //        {
                    //            falseMessage += "</br>" + "��" + (i + 2) + "�а��鲻����,δ�ܵ���.";
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

                    //���������λ�Ƿ������乫˾���߲���
                    if (string.IsNullOrEmpty(deptId) || deptId == "undefined")
                    {
                        //������˾
                        RoleEntity data = postCache.GetList(orgId, "true").OrderBy(x => x.SortCode).Where(a => a.FullName == PostName).FirstOrDefault();
                        if (data == null)
                        {
                            falseMessage += "</br>" + "��" + (i + 2) + "�и�λ�����ڸù�˾,δ�ܵ���.";
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        //��������
                        //������˾
                        RoleEntity data = postCache.GetList(orgId, deptId).OrderBy(x => x.SortCode).Where(a => a.FullName == PostName).FirstOrDefault();
                        if (data == null)
                        {
                            falseMessage += "</br>" + "��" + (i + 2) + "�и�λ�����ڸò���,δ�ܵ���.";
                            error++;
                            continue;
                        }
                    }
                    //--**��֤��λ�Ƿ����**--


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
                        falseMessage += "</br>" + "��" + (i + 2) + "�и�λ����,δ�ܵ���.";
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
                    //����Ѵ�����Ʒ����
                    LaborprotectionEntity lp = laborlist.Where(it => it.Name == Name).FirstOrDefault();
                    if (lp != null)
                    {
                        linfo.No = lp.No;
                        linfo.LId = lp.ID;
                        //���������ֵ ��ʹ�ÿ����ֵ
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
                        //��Ӹ�λ������Ա
                        LaborequipmentinfoEntity eq = new LaborequipmentinfoEntity();
                        eq.UserName = ulist[j].RealName;
                        eq.AssId = linfo.ID;
                        eq.LaborType = 0;
                        eq.ShouldNum = 1;
                        num++;
                        eq.UserId = ulist[j].UserId;
                        if (linfo.Type == "�·�")
                        {
                            eq.Size = "L";
                        }
                        else if (linfo.Type == "Ь��")
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
                message = "����" + count + "����¼,�ɹ�����" + (count - error) + "����ʧ��" + error + "��";
                message += "</br>" + falseMessage;
            }

            return message;
        }

        public bool isInt(string instring)
        {
            return Regex.IsMatch(instring, @"[1-9]\d*$");
        }

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
            laborinfobll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, LaborinfoEntity entity, string json, string ID)
        {
            json = HttpUtility.UrlDecode(json);
            laborinfobll.SaveForm(keyValue, entity, json, ID);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
