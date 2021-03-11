using System;
using System.Data;
using System.Linq;
using System.Web;
using ERCHTMS.Entity.NosaManage;
using ERCHTMS.Busines.NosaManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using BSFramework.Util.Offices;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;

namespace ERCHTMS.Web.Areas.NosaManage.Controllers
{
    /// <summary>
    /// �� ����Nosa�����
    /// </summary>
    public class NosaareaController : MvcControllerBase
    {
        private NosaareaBLL nosaareabll = new NosaareaBLL();

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
        /// �Ƿ������ͬ��ŵ�����
        /// </summary>
        /// <param name="keyValue">id</param>
        /// <param name="No">���</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult ExistEleNo(string keyValue, string No)
        {
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string id = "";
            if (keyValue != "")
            {
                id = string.Format("and id<>'{0}'", keyValue);
            }

            var oldList = nosaareabll.GetList(String.Format(" and createuserorgcode='{0}' and no='{1}' {2}", user.OrganizeCode, No, id)).ToList();
            var r = oldList.Count > 0;

            return Success("�Ѵ��ڸ�����", r);
        }

        /// <summary>
        /// ��ȡ��ǰ��Ա�ֻ���
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetPhone(string userid)
        {
            UserBLL userbll = new UserBLL();
            string mobile = userbll.GetEntity(userid).Mobile;
            if (mobile != null)
            {
                return mobile;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// ��ȡ�Ƿ����ѡ����
        /// </summary>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public string GetIsUpdate()
        {
            var issystem = OperatorProvider.Provider.Current().IsSystem;
            if (!issystem)
            {//�������ϵͳ����Ա
                DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();
                string postname = OperatorProvider.Provider.Current().PostName;
                var data = dataItemDetailBLL.GetDataItemListByItemCode("'NOSA'").ToList();
                foreach (var item in data)
                {
                    string value = item.ItemValue;
                    if (postname.Contains(value))
                    {
                        return "true";
                    }

                }

                //if (OperatorProvider.Provider.Current().RoleName.Contains("��˾���û�"))
                //{
                //    return "true";
                //}

                return "flase";


            }
            else
            {
                return "true";
            }
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var data = nosaareabll.GetList(pagination, queryJson);
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
            var data = nosaareabll.GetEntity(keyValue);
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
            nosaareabll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, NosaareaEntity entity)
        {
            nosaareabll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }

        /// <summary>
        /// ����������׼
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportEle()
        {
            UserBLL userbll=new UserBLL();
            NosaeleBLL nosaelebll = new NosaeleBLL();
            int error = 0;
            int sussceed = 0;
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
                string filePath = Server.MapPath("~/Resource/temp/" + fileName);
                file.SaveAs(filePath);

                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                wb.Open(Server.MapPath("~/Resource/temp/" + fileName));
                Aspose.Cells.Cells cells = wb.Worksheets[0].Cells;
                if ((cells.MaxDataRow-3) == 0)
                {
                    message = "û������,��ѡ������дģ���ڽ��е���!";
                    return message;
                }
                DataTable dt = cells.ExportDataTable(3, 0, (cells.MaxDataRow -2), cells.MaxColumn + 1, true);

                //DataTable dt = ExcelHelper.ExcelImport(filePath);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    object[] vals = dt.Rows[i].ItemArray;
                    if (IsEndRow(vals) == true)
                        break;
                    var msg = "";
                    if (Validate(i, vals, userbll, nosaelebll, out msg) == true)
                    {
                        var entity = GenEntity(vals, userbll, nosaelebll);
                        nosaareabll.SaveForm(entity.ID, entity);
                        sussceed++;
                    }
                    else
                    {
                        falseMessage += "��" + (i + 1) + "��" + msg + "<br/>";
                        error++;
                    }
                }
                count = dt.Rows.Count;
                message = "����" + count + "����¼,�ɹ�����" + sussceed + "����ʧ��" + error + "��";
                message += "<br/>" + falseMessage;
                //ɾ����ʱ�ļ�
                System.IO.File.Delete(filePath);
            }
            return message;
        }
        private NosaareaEntity GenEntity(object[] vals, UserBLL userbll, NosaeleBLL nosaelebll)
        {
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            NosaareaEntity entity = new NosaareaEntity() { ID = Guid.NewGuid().ToString() };
            var oldList = nosaareabll.GetList(String.Format(" and createuserorgcode='{0}' and no='{1}'", user.OrganizeCode, vals[0].ToString().Trim())).ToList();
            if (oldList.Count > 0)
                entity = oldList[0];

            entity.NO = vals[0].ToString().Trim();
            entity.Name = vals[1].ToString().Trim();
            entity.AreaRange = vals[2].ToString().Trim();
            entity.DutyDepartName = vals[3].ToString().Trim();
            entity.DutyUserName = vals[4].ToString().Trim();
            var uEntity = userbll.GetUserInfoByName(entity.DutyDepartName, entity.DutyUserName);
            entity.DutyUserId = uEntity.UserId;
            entity.DutyDepartId = uEntity.DepartmentId;

            entity.Mobile = vals[5].ToString();

            return entity;
        }
        private bool IsEndRow(object[] vals)
        {
            bool r = false;

            r = Array.TrueForAll(vals, x => (x == null || x == DBNull.Value || x.ToString() == ""));

            return r;
        }
        private bool IsNull(object obj)
        {
            return obj == null || obj == DBNull.Value || obj.ToString() == "";
        }
        private bool Validate(int index, object[] vals, UserBLL userbll, NosaeleBLL nosaelebll, out string msg)
        {
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            var r = true;
            var i = index + 1;
            msg = "";
            if (vals.Length < 6)
            {
                msg += ",��ʽ����ȷ";
                r = false;
            }
            var obj = vals[0];
            if (IsNull(obj))
            {
                msg += ",�����Ų���Ϊ��";
                r = false;
            }
            else
            {
                var oldList = nosaelebll.GetList(String.Format(" and createuserorgcode='{0}' and no='{1}'", user.OrganizeCode, obj.ToString().Trim())).ToList();
                if (oldList.Count > 0)
                    msg += "���������Ѵ���";
            }
            obj = vals[1];
            if (IsNull(obj))
            {
                msg += ",�������Ʋ���Ϊ��";
                r = false;
            }
            //obj = vals[2];
            //if (IsNull(obj))
            //{
            //    msg += "������Χ����Ϊ��";
            //    r = false;
            //}
            obj = vals[3];
            if (IsNull(obj))
            {
                msg += ",�������β��Ų���Ϊ��";
                r = false;
            }

            obj = vals[4];
            if (IsNull(obj))
            {
                msg += ",���������Ϊ��";
                r = false;
            }
            else if (!IsNull(vals[3]))
            {
                var entity = userbll.GetUserInfoByName(vals[3].ToString().Trim(), obj.ToString().Trim());
                if (entity == null)
                {
                    msg += ",���β��Ų���ȷ�����β����в�������Ӧ����������û�";
                    r = false;
                }
            }

            obj = vals[5];//��ϵ�绰
            //if (!IsNull(obj))
            //{
            //    var list = nosaelebll.GetList(String.Format(" and createuserorgcode='{0}' and no='{1}'", user.OrganizeCode, obj.ToString().Trim()));
            //    if (list.Count() == 0)
            //    {
            //        msg += "���ϼ������Ų�����";
            //        r = false;
            //    }
            //}

            if (!string.IsNullOrWhiteSpace(msg))
            {
                msg += "��";
                r = false;
            }

            return r;
        }
        #endregion
    }
}
