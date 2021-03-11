using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using ERCHTMS.Entity.LllegalStandard;
using ERCHTMS.Busines.LllegalStandard;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Code;
using ERCHTMS.Busines.SystemManage;
using System.Web;
using BSFramework.Util.Offices;
using System.Data;
using System.Collections;
using ERCHTMS.Entity.SystemManage.ViewModel;
using BSFramework.Util.Extension;
using ERCHTMS.Busines.AuthorizeManage;

namespace ERCHTMS.Web.Areas.LllegalStandard.Controllers
{
    /// <summary>
    /// �� ����Υ�±�׼��
    /// </summary>
    public class LllegalStandardController : MvcControllerBase
    {
        private LllegalstandardBLL lllegalstandardbll = new LllegalstandardBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();

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
        [HttpGet]
        public ActionResult Select()
        {
            return View();
        }
        #endregion

        #region ҳ�������ʼ��
        /// <summary>
        /// ��ʼ������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetInitDataJson()
        {
            //Υ������ Υ�µȼ� ��ҵ����
            string itemCode = "'LllegalType','LllegalLevel','LllegalBusType'";
            //����
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode(itemCode);
            //����ֵ
            var josnData = new
            {
                LllegalType = itemlist.Where(p => p.EnCode == "LllegalType"), //Υ������
                LllegalLevel = itemlist.Where(p => p.EnCode == "LllegalLevel"),  //Υ�¼���
                LIIegalBusType = itemlist.Where(p => p.EnCode == "LllegalBusType") //��ҵ����
            };

            return Content(josnData.ToJson());
        }
        #endregion

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var queryParam = queryJson.ToJObject();
            Operator opertator = new OperatorProvider().Current();
            if (pagination.p_fields.IsEmpty())
            {
                pagination.p_fields = @"createdate,createuserid,createuserdeptcode,createuserorgcode,modifydate,modifyuserid,des,leglevel,legLevalName,legtype,legTypeName,bustype,busTypeName,descore,demoney,firstdescore,firstdemoney,seconddescore,seconddemoney,remark";
            }
            pagination.p_kid = "id";
            pagination.p_tablename = @"v_lllegalstdinfo";
            pagination.conditionJson = " 1=1";
            string authWhere = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "CREATEUSERDEPTCODE", "CREATEUSERORGCODE");
            if (!string.IsNullOrEmpty(authWhere))
            {//����Ȩ��,��ϵͳ����Ա��ӵ����ݡ�
                pagination.conditionJson += " and (" + authWhere + " or CREATEUSERORGCODE='00')";
            }
            else
            {
                pagination.conditionJson += " and CREATEUSERORGCODE='" + opertator.OrganizeCode + "'";
            }

            //Υ������
            if (!queryParam["lllegaltype"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and  legtype='{0}' ", queryParam["lllegaltype"].ToString());
            }
            //Υ�¼���
            if (!queryParam["lllegallevel"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and leglevel ='{0}'", queryParam["lllegallevel"].ToString());
            }
            //Υ������ 
            if (!queryParam["lllegaldescribe"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and des like '%{0}%'", queryParam["lllegaldescribe"].ToString());
            }
            var data = lllegalstandardbll.GetLllegalStdInfo(pagination, queryJson);
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
            var data = lllegalstandardbll.GetEntity(keyValue);
            //Υ������ Υ�µȼ� ��ҵ����
            string itemCode = "'LllegalType','LllegalLevel','LllegalBusType'";
            //����
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode(itemCode);
            //����ֵ
            var josnData = new
            {
                data = data,
                LllegalType = itemlist.Where(p => p.EnCode == "LllegalType"), //Υ������
                LllegalLevel = itemlist.Where(p => p.EnCode == "LllegalLevel"),  //Υ�¼���
                LIIegalBusType = itemlist.Where(p => p.EnCode == "LllegalBusType") //��ҵ����
            };

            return Content(josnData.ToJson());
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
            lllegalstandardbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, LllegalstandardEntity entity)
        {
            lllegalstandardbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion

        #region ����������׼
        /// <summary>
        /// ����������׼
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportLegStd()
        {
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
                //Υ������ Υ�µȼ� ��ҵ����
                string itemCode = "'LllegalType','LllegalLevel','LllegalBusType'";
                //����
                var itemlist = dataitemdetailbll.GetDataItemListByItemCode(itemCode).ToList();
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                string filePath = Server.MapPath("~/Resource/temp/" + fileName);
                file.SaveAs(filePath);
                DataTable dt = ExcelHelper.ExcelImport(filePath);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    object[] vals = dt.Rows[i].ItemArray;
                    if (IsEndRow(vals) == true)
                        break;
                    var msg = "";
                    if (Validate(i, vals, itemlist, out msg) == true)
                    {
                        var entity = GenEntity(vals, itemlist);
                        lllegalstandardbll.SaveForm("", entity);
                        sussceed++;
                    }
                    else
                    {
                        falseMessage += "��" + (i + 1) + "��" + msg + "</br>";
                        error++;
                    }
                }
                count = sussceed + error;
                message = "����" + count + "����¼,�ɹ�����" + sussceed + "����ʧ��" + error + "��";
                message += "</br>" + falseMessage;
                //ɾ����ʱ�ļ�
                System.IO.File.Delete(filePath);
            }
            return message;
        }
        private bool IsEndRow(object[] vals)
        {
            bool r = false;

            r = Array.TrueForAll(vals, x => (x == null || x == DBNull.Value || x.ToString() == ""));

            return r;
        }
        private bool Validate(int index, object[] vals, List<DataItemModel> item, out string msg)
        {
            var r = true;
            var i = index + 1;
            msg = "";
            if (vals.Length < 10)
            {
                msg += "����ʽ����ȷ";
                r = false;
            }
            var obj = vals[0];
            if (obj == null || obj == DBNull.Value || obj.ToString() == "")
            {
                msg += "��Υ����������Ϊ��";
                r = false;
            }
            obj = vals[1];
            if (obj == null || obj == DBNull.Value || obj.ToString() == "")
            {
                msg += "��Υ�����Ͳ���Ϊ��";
                r = false;
            }
            else
            {
                int ncount = item.Count(p => p.EnCode == "LllegalType" && p.ItemName == obj.ToString());
                if (ncount <= 0)
                {
                    msg += "��Υ�����Ͳ���ȷ";
                    r = false;
                }
            }
            obj = vals[2];
            if (obj == null || obj == DBNull.Value || obj.ToString() == "")
            {
                msg += "��Υ�¼�����Ϊ��";
                r = false;
            }
            else
            {
                int ncount = item.Count(p => p.EnCode == "LllegalLevel" && p.ItemName == obj.ToString());
                if (ncount <= 0)
                {
                    msg += "��Υ�¼�����ȷ";
                    r = false;
                }
            }
            obj = vals[3];
            if (obj == null || obj == DBNull.Value || obj.ToString() == "")
            {
                msg += "����ҵ���Ͳ���Ϊ��";
                r = false;
            }
            else
            {
                int ncount = item.Count(p => p.EnCode == "LllegalBusType" && p.ItemName == obj.ToString());
                if (ncount <= 0)
                {
                    msg += "����ҵ���Ͳ���ȷ";
                    r = false;
                }
            }
            obj = vals[4];
            if (obj != null && obj != DBNull.Value)
            {
                double num = 0;
                if (!double.TryParse(obj.ToString(), out num) || num < 0)
                {
                    msg += "��Υ�������˿۷�ֻ���Ǵ��ڵ����������";
                    r = false;
                }
            }
            obj = vals[5];
            if (obj != null && obj != DBNull.Value)
            {
                double num = 0;
                if (!double.TryParse(obj.ToString(), out num) || num < 0)
                {
                    msg += "��Υ�������˿���ֻ���Ǵ��ڵ����������";
                    r = false;
                }
            }
            obj = vals[6];
            if (obj != null && obj != DBNull.Value)
            {
                double num = 0;
                if (!double.TryParse(obj.ToString(), out num) || num < 0)
                {
                    msg += "����һ�����˿۷�ֻ���Ǵ��ڵ����������";
                    r = false;
                }
            }
            obj = vals[7];
            if (obj != null && obj != DBNull.Value)
            {
                double num = 0;
                if (!double.TryParse(obj.ToString(), out num) || num < 0)
                {
                    msg += "����һ�����˿���ֻ���Ǵ��ڵ����������";
                    r = false;
                }
            }
            obj = vals[8];
            if (obj != null && obj != DBNull.Value)
            {
                double num = 0;
                if (!double.TryParse(obj.ToString(), out num) || num < 0)
                {
                    msg += "���ڶ������˿۷�ֻ���Ǵ��ڵ����������";
                    r = false;
                }
            }
            obj = vals[9];
            if (obj != null && obj != DBNull.Value)
            {
                double num = 0;
                if (!double.TryParse(obj.ToString(), out num) || num < 0)
                {
                    msg += "���ڶ������˿���ֻ���Ǵ��ڵ����������";
                    r = false;
                }
            }
            if (!string.IsNullOrWhiteSpace(msg))
            {
                msg = string.Format("��{0}��{1}��</br>", i, msg);
                r = false;
            }

            return r;
        }
        private LllegalstandardEntity GenEntity(object[] vals, List<DataItemModel> item)
        {
            LllegalstandardEntity entity = new LllegalstandardEntity();
            entity.DES = vals[0].ToString();
            entity.REMARK = "��������";
            var obj = vals[1];
            if (obj != null && obj != DBNull.Value)
            {
                var n = item.FirstOrDefault(p => p.EnCode == "LllegalType" && p.ItemName == obj.ToString());
                if (n != null)
                {
                    entity.LEGTYPE = n.ItemDetailId;
                }
            }
            obj = vals[2];
            if (obj != null && obj != DBNull.Value)
            {
                var n = item.FirstOrDefault(p => p.EnCode == "LllegalLevel" && p.ItemName == obj.ToString());
                if (n != null)
                {
                    entity.LEGLEVEL = n.ItemDetailId;
                }
            }
            obj = vals[3];
            if (obj != null && obj != DBNull.Value)
            {
                var n = item.FirstOrDefault(p => p.EnCode == "LllegalBusType" && p.ItemName == obj.ToString());
                if (n != null)
                {
                    entity.BUSTYPE = n.ItemDetailId;
                }
            }
            obj = vals[4];
            if (obj != null && obj != DBNull.Value)
            {
                decimal num = 0;
                if (decimal.TryParse(obj.ToString(), out num))
                {
                    entity.DESCORE = num;
                }
            }
            obj = vals[5];
            if (obj != null && obj != DBNull.Value)
            {
                double num = 0;
                if (double.TryParse(obj.ToString(), out num))
                {
                    entity.DEMONEY = num;
                }
            }
            obj = vals[6];
            if (obj != null && obj != DBNull.Value)
            {
                decimal num = 0;
                if (decimal.TryParse(obj.ToString(), out num))
                {
                    entity.FIRSTDESCORE = num;
                }
            }
            obj = vals[7];
            if (obj != null && obj != DBNull.Value)
            {
                double num = 0;
                if (double.TryParse(obj.ToString(), out num))
                {
                    entity.FIRSTDEMONEY = num;
                }
            }
            obj = vals[8];
            if (obj != null && obj != DBNull.Value)
            {
                decimal num = 0;
                if (decimal.TryParse(obj.ToString(), out num))
                {
                    entity.SECONDDESCORE = num;
                }
            }
            obj = vals[9];
            if (obj != null && obj != DBNull.Value)
            {
                double num = 0;
                if (double.TryParse(obj.ToString(), out num))
                {
                    entity.SECONDDEMONEY = num;
                }
            }

            return entity;
        }
        #endregion
    }
}
