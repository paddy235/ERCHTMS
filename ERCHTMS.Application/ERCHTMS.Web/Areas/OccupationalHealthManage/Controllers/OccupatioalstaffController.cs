using ERCHTMS.Entity.OccupationalHealthManage;
using ERCHTMS.Busines.OccupationalHealthManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System;
using ERCHTMS.Busines.PublicInfoManage;
using System.Data;
using ERCHTMS.Code;
using System.Web;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.SystemManage.ViewModel;

namespace ERCHTMS.Web.Areas.OccupationalHealthManage.Controllers
{
    /// <summary>
    /// �� ����ְҵ����Ա��
    /// </summary>
    public class OccupatioalstaffController : MvcControllerBase
    {
        private OccupatioalstaffBLL occupatioalstaffbll = new OccupatioalstaffBLL();

        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        #region ��ͼ����
        /// <summary>
        /// �б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {

            ViewBag.IsQx = "0";
            //��ǰ�û�
            Operator curUser = OperatorProvider.Provider.Current();
            DataItemModel Depart = dataitemdetailbll.GetDataItemListByItemCode("'HealthDeptQx'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();
            if (Depart != null)
            {
                if (Depart.ItemValue.Contains(curUser.DeptId))
                {
                    ViewBag.IsQx = "1";
                }
            }
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
        #endregion

        /// <summary>
        /// �ļ��б�
        /// </summary>
        /// <returns></returns>
        public ActionResult FileList()
        {
            return View();
        }
        [HttpGet]
        public ActionResult OpenWord()
        {
            //PageOffice.PageOfficeCtrl pc = new PageOffice.PageOfficeCtrl();
            //pc.AddCustomToolButton("����", "Save()", 1);
            //pc.SaveFilePage = "/Word/SaveDoc";
            //pc.ServerPage = Request.ApplicationPath + "/pageoffice/server.aspx";

            //pc.WebOpen(@"E:\��Ŀ�ĵ�\������ܼ��_V2.0_20180516.docx", PageOffice.OpenModeType.docReadOnly, "s");

            //ViewBag.EditorHtml = pc.GetHtmlCode("PageOfficeCtrl1");
            //return Redirect("../../Utility/DownloadFile?filePath=" + Server.UrlEncode("E:\\����\\�ϴ��ļ�.txt"));
            return View();
        }
        
        public void GetWord(string fileUrl)
        {
            fileUrl = HttpUtility.UrlDecode(fileUrl);
            // ���ĵ�
            //string filePath = Server.MapPath(Request.ApplicationPath + "/Resource/EmergencyPlan/" + fileUrl);
            //����ȥ��ǰ���~��
            //fileUrl = fileUrl.Substring(1);
            string filePath = Server.MapPath(fileUrl);
            //string filePath = Request.ApplicationPath + "/Resource/EmergencyPlan/" + fileUrl;
            string[] files = filePath.Split('.');
            if (files[files.Length - 1] == "pdf")
            {
                PageOffice.PDFCtrl PdfCtrl1 = new PageOffice.PDFCtrl();
                PdfCtrl1.ServerPage = Request.ApplicationPath + "/pageoffice/server.aspx";
                PdfCtrl1.AddCustomToolButton("��ӡ", "Print()", 6);
                //PdfCtrl1.AddCustomToolButton("-", "", 0);
                //PdfCtrl1.AddCustomToolButton("��ʾ/������ǩ", "SwitchBKMK()", 0);
                //PdfCtrl1.AddCustomToolButton("ʵ�ʴ�С", "SetPageReal()", 16);
                //PdfCtrl1.AddCustomToolButton("�ʺ�ҳ��", "SetPageFit()", 17);
                //PdfCtrl1.AddCustomToolButton("�ʺϿ��", "SetPageWidth()", 18);
                //PdfCtrl1.AddCustomToolButton("-", "", 0);
                //PdfCtrl1.AddCustomToolButton("��ҳ", "FirstPage()", 0);
                //PdfCtrl1.AddCustomToolButton("��һҳ", "PreviousPage()", 9);
                //PdfCtrl1.AddCustomToolButton("��һҳ", "NextPage()", 10);
                //PdfCtrl1.AddCustomToolButton("βҳ", "LastPage()", 11);
                //PdfCtrl1.AddCustomToolButton("-", "", 0);
                PdfCtrl1.WebOpen(filePath);
                Response.Write(PdfCtrl1.GetHtmlCode("PdfCtrl1"));
                Response.End();
            }
            else if (files[files.Length - 1] == "xls" || files[files.Length - 1] == "xlsx" || files[files.Length - 1] == "doc" || files[files.Length - 1] == "docx" || files[files.Length - 1] == "ppt" || files[files.Length - 1] == "pptx")
            {
                PageOffice.PageOfficeCtrl PageOfficeCtrl1 = new PageOffice.PageOfficeCtrl();
                PageOfficeCtrl1.ServerPage = Request.ApplicationPath + "/pageoffice/server.aspx";
                // ���ñ����ļ�ҳ��
                //PageOfficeCtrl1.SaveFilePage = Server.MapPath(Request.ApplicationPath + "/DayCheckManage/EmergencyPlan/SaveFile?filrUrl=EmergencyPlan&keyValue=" + keyValue);
                //PageOfficeCtrl1.SaveFilePage = Request.ApplicationPath + "/DayCheckManage/EmergencyPlan/SaveFile?filrUrl=EmergencyPlan&keyValue=" + keyValue;
                //����Զ��尴ť
                //PageOfficeCtrl1.Caption = "�ĵ��༭";

                PageOfficeCtrl1.Titlebar = false; //���ر�����
                PageOfficeCtrl1.Menubar = false; //���ز˵���
                PageOfficeCtrl1.CustomToolbar = false; //�����Զ��幤����
                PageOfficeCtrl1.OfficeToolbars = false; //����Office������
                PageOfficeCtrl1.Theme = PageOffice.ThemeType.CustomStyle;
                if (files[files.Length - 1] == "doc" || files[files.Length - 1] == "docx")//�ж��Ƿ���word
                {
                    PageOfficeCtrl1.WebOpen(filePath, PageOffice.OpenModeType.docReadOnly, "fwz");
                }
                else if (files[files.Length - 1] == "xls" || files[files.Length - 1] == "xlsx")
                {
                    PageOfficeCtrl1.WebOpen(filePath, PageOffice.OpenModeType.xlsReadOnly, "fwz");
                }
                else if (files[files.Length - 1] == "ppt" || files[files.Length - 1] == "pptx")
                {
                    PageOfficeCtrl1.WebOpen(filePath, PageOffice.OpenModeType.pptReadOnly, "fwz");//û�е�������word��
                }
                else
                {
                    PageOfficeCtrl1.WebOpen(filePath, PageOffice.OpenModeType.docReadOnly, "fwz");//û�е�������word��
                }
                Response.Write(PageOfficeCtrl1.GetHtmlCode("PageOfficeCtrl1"));
                Response.End();
            }
            
        }

        #region ��ȡ����
        public ActionResult GetFiles(string keyValue)
        {
            FileInfoBLL file = new FileInfoBLL();
            var dt = file.GetFiles(keyValue);//�Ȼ�ȡ�ϴ��ĸ���
            return ToJsonResult(dt);
        }

        //�ж��ļ��Ƿ����
        [HttpPost]
        public bool IsUrl(string fileUrl)
        {
            fileUrl = fileUrl.Substring(1);
            string filePath = Server.MapPath(Request.ApplicationPath + fileUrl);
            //�ж��Ƿ����ļ�
            bool flag = System.IO.File.Exists(filePath);
            return flag;
        }

        /// <summary>
        /// ������Excel
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult Excel(string queryJson)
        {
            string whereSql = "";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                whereSql = "";
            }
            else
            {
                string where = new ERCHTMS.Busines.AuthorizeManage.AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
                whereSql += " and " + where;
            }

            DataTable dt = occupatioalstaffbll.GetTable(queryJson, whereSql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i][0] = (i + 1).ToString();
                dt.Rows[i]["INSPECTIONTIME"] = Convert.ToDateTime(dt.Rows[i]["INSPECTIONTIME"]).ToString("yyyy-MM-dd");
            }
            string FileUrl = @"\Resource\ExcelTemplate\ְҵ��������б�_����ģ��.xlsx";



            AsposeExcelHelper.ExecuteResult(dt, FileUrl, "ְҵ��������б�", "ְҵ��������б�");

            return Success("�����ɹ���");
        }


        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "OCCID";
            pagination.p_fields = "MECHANISMNAME,INSPECTIONTIME,INSPECTIONNUM,PATIENTNUM,ISANNEX,FILENUM,unusualnum";//ע���˴�Ҫ�滻����Ҫ��ѯ����
            pagination.p_tablename = "V_OCCUPATIOALSTAFF";
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

            var data = occupatioalstaffbll.GetPageListByProc(pagination, queryJson);
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
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = occupatioalstaffbll.GetEntity(keyValue);
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
            OccupationalstaffdetailBLL occupationalstaffdetailbll = new OccupationalstaffdetailBLL();
            occupationalstaffdetailbll.Delete(keyValue, 0);//��ɾ��������������
            occupationalstaffdetailbll.Delete(keyValue, 1);
            occupatioalstaffbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, string strentity, string Users, string UserIds, bool isNew)
        {
            OccupatioalstaffEntity userEntity = strentity.ToObject<OccupatioalstaffEntity>();

            #region �������Ա����
            //�Ȳ�ѯ���������û�
            OccupationalstaffdetailBLL occupationalstaffdetailbll = new OccupationalstaffdetailBLL();
            IEnumerable<OccupationalstaffdetailEntity> odlist = occupationalstaffdetailbll.GetList(keyValue, 1);//��ѯ��������д�Ļ�����¼
            IEnumerable<OccupationalstaffdetailEntity> yclist = occupationalstaffdetailbll.GetList(keyValue, 2);//��ѯ��������д���쳣��¼
            occupationalstaffdetailbll.UpdateTime((DateTime)(userEntity.InspectionTime), keyValue);//�Ƚ����еĲ������ʱ���������
            if (!isNew) //�޸�ģʽ
            {
                occupationalstaffdetailbll.Delete(keyValue, 0);//�Ȱѽ�����Ա��¼ɾ�����������
            }
            //��ȡ����������û�
            string[] UserGroup = UserIds.Split(',');
            string[] UsersGroup = Users.Split(',');
            if (odlist != null)
            {
                for (int i = 0; i < UserGroup.Length; i++)
                {
                    if (!IsTrue(odlist, yclist, UserGroup[i]))
                    {
                        //�ж�Ϊ����Ա��
                        OccupationalstaffdetailEntity occ = new OccupationalstaffdetailEntity();
                        occ.UserId = UserGroup[i];
                        occ.UserName = UsersGroup[i];
                        occ.UserNamePinYin = Str.PinYin(UsersGroup[i]);
                        occ.Note = "";
                        occ.Issick = 0;
                        occ.InspectionTime = userEntity.InspectionTime;
                        occ.OccId = keyValue;
                        occ.SickType = "��";
                        occupationalstaffdetailbll.SaveForm("", occ);
                    }
                }
            }
            #endregion
            FileInfoBLL file = new FileInfoBLL();
            DataTable dt = file.GetFiles(keyValue);//�Ȼ�ȡ�ϴ��ĸ���
            if (dt != null && dt.Rows.Count > 0)
            {
                userEntity.IsAnnex = 1;//�и���
            }
            else
            {
                userEntity.IsAnnex = 0;//û�и���
            }
            userEntity.OccId = keyValue;
            userEntity.InspectionNum = UserGroup.Length;
            userEntity.PatientNum = odlist.Count();
            userEntity.UnusualNum = yclist.Count();

            occupatioalstaffbll.SaveForm(isNew, userEntity);
            return Success("�����ɹ���");
        }
        #endregion

        /// <summary>
        /// �ж�����Ա�����Ƿ������Ա��
        /// </summary>
        /// <param name="odlist"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool IsTrue(IEnumerable<OccupationalstaffdetailEntity> odlist, IEnumerable<OccupationalstaffdetailEntity> yclist, string value)
        {
            foreach (OccupationalstaffdetailEntity item in odlist)
            {
                if (item.UserId == value) //�ж��Ƿ�������Ա��
                {
                    return true;
                }
            }
            foreach (OccupationalstaffdetailEntity item in yclist)
            {
                if (item.UserId == value) //�ж��Ƿ����쳣Ա��
                {
                    return true;
                }
            }
            return false;
        }
    }
}
