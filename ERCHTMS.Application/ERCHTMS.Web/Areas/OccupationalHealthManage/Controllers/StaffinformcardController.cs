using ERCHTMS.Entity.OccupationalHealthManage;
using ERCHTMS.Busines.OccupationalHealthManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Entity.PublicInfoManage;
using System.Data;
using ERCHTMS.Code;

namespace ERCHTMS.Web.Areas.OccupationalHealthManage.Controllers
{
    /// <summary>
    /// �� ����ְҵ��Σ����֪��
    /// </summary>
    public class StaffinformcardController : MvcControllerBase
    {
        private StaffinformcardBLL staffinformcardbll = new StaffinformcardBLL();

        #region ��ͼ����
        /// <summary>
        /// �б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.IsSystem = ERCHTMS.Code.OperatorProvider.Provider.Current().IsSystem;
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
        /// ��֪����
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CardLibraryForm()
        {
            return View();
        }
        #endregion

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson, string type)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "ID";
            pagination.p_fields = "INFORMCARDNAME,INFORMCARDVALUE,INFORMACARDPOSITION,SETTINGTIME,FileId,FileName,filepath";//ע���˴�Ҫ�滻����Ҫ��ѯ����
            pagination.p_tablename = "V_STAFFINFORMCARD";
            pagination.conditionJson = "CARDTYPE=" + type;
            pagination.sidx = "SETTINGTIME";

            if (type == "0")//��֪���ⲻ�ж�Ȩ��
            {
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
            }

            var data = staffinformcardbll.GetPageListByProc(pagination, queryJson);
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
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = staffinformcardbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #endregion

        #region �ύ����

        /// <summary>
        /// ���Ƹ�֪��������λ
        /// </summary>
        /// <param name="keyValue">����</param>
        /// <param name="Fileid">�ļ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult CopyForm(string keyValue, string Fileid)
        {
            StaffinformcardEntity data = staffinformcardbll.GetEntity(keyValue);//�ȸ���id��ȡʵ��
            data.CardType = 0;
            data.Id = System.Guid.NewGuid().ToString();
            staffinformcardbll.SaveForm("", data);//��������
            FileInfoBLL file = new FileInfoBLL();
            FileInfoEntity fi = file.GetEntity(Fileid);//��ȡ�ļ���Ϣ
            string oldfilename = fi.FilePath.Substring(fi.FilePath.LastIndexOf('/') + 1);
            string url = fi.FilePath.Substring(1, fi.FilePath.LastIndexOf('/') + 1);//��Ҫ~
            string[] filenames = oldfilename.Split('.');
            if (filenames.Length > 1)
            {
                string hz = filenames[filenames.Length - 1];//��׺��
                string newfilename = System.Guid.NewGuid().ToString() + "." + hz;
                string newUrl = url + newfilename;
                string oldPath = fi.FilePath.Substring(1);
                CopyFile(oldPath, newUrl);//���Ƶ��µ�ַ
                fi.FileId = "";
                fi.RecId = data.Id;
                fi.FilePath = "~" + newUrl;//�����¼ʱ����~
                file.SaveForm("", fi);//�����µ��ļ���¼
                return Success("true");
            }
            return Success("false");

        }

        /// <summary>
        /// �����ļ�
        /// </summary>
        /// <param name="dir1">Ҫ���Ƶ��ļ���·���Ѿ�ȫ��(������׺)</param>
        /// <param name="dir2">Ŀ��λ��,��ָ���µ��ļ���</param>
        public void CopyFile(string dir1, string dir2)
        {
            dir1 = dir1.Replace("/", "\\");
            dir2 = dir2.Replace("/", "\\");
            if (System.IO.File.Exists(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir1))
            {
                System.IO.File.Copy(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir1, System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir2, true);
            }
        }

        /// <summary>
        /// ������Excel
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult Excel(string queryJson)
        {
            string whereSql = "";
            ERCHTMS.Code.Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                whereSql = "";
            }
            else
            {
                string where = new ERCHTMS.Busines.AuthorizeManage.AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
                whereSql += " and " + where;
            }
            whereSql += " and CARDTYPE=0";
            DataTable dt = staffinformcardbll.GetTable(queryJson, whereSql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i][0] = (i + 1).ToString();
            }
            string FileUrl = @"\Resource\ExcelTemplate\ְҵ��Σ����֪��_����ģ��.xlsx";



            AsposeExcelHelper.ExecuteResult(dt, FileUrl, "ְҵ��Σ����֪��", "ְҵ��Σ����֪��");

            return Success("�����ɹ���");
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
            staffinformcardbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, StaffinformcardEntity entity)
        {
            staffinformcardbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
