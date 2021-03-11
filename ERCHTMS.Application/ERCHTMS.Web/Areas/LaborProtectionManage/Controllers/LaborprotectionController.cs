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
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Web.Areas.LaborProtectionManage.Controllers
{
    /// <summary>
    /// �� �����Ͷ�������Ʒ
    /// </summary>
    public class LaborprotectionController : MvcControllerBase
    {
        private LaborprotectionBLL laborprotectionbll = new LaborprotectionBLL();
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
        /// ���ʱ��ȡ�����б�
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetNameList()
        {
            //��ȡ����ѡ����
            List<LaborprotectionEntity> laborlist = laborprotectionbll.GetLaborList();

            DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();
            var data = dataItemDetailBLL.GetDataItemListByItemCode("'LaborName'").ToList();
            for (int i = 0; i < data.Count; i++)
            {
                //�����ǰû��������� ����뵽������
                if (laborlist.Where(it => it.Name == data[i].ItemName).Count() == 0)
                {
                    LaborprotectionEntity lb = new LaborprotectionEntity();
                    lb.ID = i.ToString();
                    lb.Name = data[i].ItemName;
                    laborlist.Add(lb);
                }
            }

            return ToJsonResult(laborlist);;
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
            pagination.p_kid = "ID";
            pagination.p_fields = "NAME,NO,UNIT,MODEL,TYPE,TIMENUM,TIMETYPE,NOTE,LABOROPERATIONUSERNAME,LABOROPERATIONTIME,createuserid,createuserdeptcode,createuserorgcode";//ע���˴�Ҫ�滻����Ҫ��ѯ����
            pagination.p_tablename = "BIS_LABORPROTECTION";
            pagination.conditionJson = "1=1";
            pagination.sidx = "CREATEDATE";

            ERCHTMS.Code.Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
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

            var data = laborprotectionbll.GetPageListByProc(pagination, queryJson);
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
            var data = laborprotectionbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ�µı��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetNo()
        {
            string no = laborprotectionbll.GetNo();
            return no;
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

                //�Ȼ�ȡ��ԭʼ��һ�����
                string no = laborprotectionbll.GetNo();
                int ysno = Convert.ToInt32(no);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    order = i;

                    string Name = dt.Rows[i]["�Ͷ�������Ʒ����"].ToString();
                    string Unit = dt.Rows[i]["�Ͷ�������Ʒ��λ"].ToString();
                    string Model = dt.Rows[i]["�ͺ�"].ToString();
                    string Type = dt.Rows[i]["����"].ToString();
                    string TimeNum = dt.Rows[i]["ʹ������ʱ��"].ToString();
                    string TimeType = dt.Rows[i]["ʹ�����޵�λ"].ToString();
                    string Note = dt.Rows[i]["ʹ��˵��"].ToString().Trim();



                    //---****ֵ���ڿ���֤*****--
                    if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Unit))
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "��ֵ���ڿ�,δ�ܵ���.";
                        error++;
                        continue;
                    }


                    LaborprotectionEntity ue = new LaborprotectionEntity();
                    ue.Name = Name;
                    ue.Unit = Unit;
                    ue.Model = Model;
                    ue.Type = Type;
                    if (TimeNum != "")
                        ue.TimeNum = Convert.ToInt32(TimeNum);//����
                    ue.TimeType = TimeType;
                    ue.Note = Note;
                    ue.No = ysno.ToString();
                    //��һ���������
                    ysno++;
                    ue.LaborOperationUserName = currUser.UserName;
                    ue.LaborOperationTime = DateTime.Now;

                    try
                    {
                        laborprotectionbll.SaveForm("", ue);
                    }
                    catch
                    {
                        error++;
                    }
                }

                count = dt.Rows.Count;
                message = "����" + count + "����¼,�ɹ�����" + (count - error) + "����ʧ��" + error + "��";
                message += "</br>" + falseMessage;
            }

            return message;
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
            laborprotectionbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, LaborprotectionEntity entity)
        {
            laborprotectionbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
