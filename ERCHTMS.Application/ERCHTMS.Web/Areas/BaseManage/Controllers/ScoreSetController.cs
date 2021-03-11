using BSFramework.Util;
using BSFramework.Util.Offices;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.PersonManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.PersonManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace ERCHTMS.Web.Areas.BaseManage.Controllers
{
    /// <summary>
    /// �� ������������
    /// </summary>
    public class ScoreSetController : MvcControllerBase
    {
        private ScoreSetBLL scoresetbll = new ScoreSetBLL();

        #region ��ͼ����
        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }
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
        #endregion

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = scoresetbll.GetList(queryJson);
            return ToJsonResult(data);
        }

        [HttpGet]
        public ActionResult GetItemListJson(string queryJson)
        {
            string sidx = Request.Params["sidx"]; string sord = Request.Params["sord"];
            var data = scoresetbll.GetList(queryJson).Select(t => new { t.Id, t.ItemName, t.ItemType, t.Score }).ToList();
            if (!string.IsNullOrEmpty(sidx))
            {
                if (sidx == "Score")
                {
                    if (sord == "asc")
                    {
                        data = data.OrderBy(t => t.Score).ToList();
                    }
                    else
                    {
                        data = data.OrderByDescending(t => t.Score).ToList();
                    }
                }
                if (sidx == "ItemName")
                {
                    if (sord == "asc")
                    {
                        data = data.OrderBy(t => t.ItemName).ToList();
                    }
                    else
                    {
                        data = data.OrderByDescending(t => t.ItemName).ToList();
                    }
                }
                if (sidx == "ItemType")
                {
                    if (sord == "asc")
                    {
                        data = data.OrderBy(t => t.ItemType).ToList();
                    }
                    else
                    {
                        data = data.OrderByDescending(t => t.ItemType).ToList();
                    }
                }

            }
            else
            {
                data = data.OrderByDescending(t => t.ItemName).ToList();
            }
            return ToJsonResult(data);
        }
        /// <summary>
        /// �б��ҳ
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>   
        //[HandlerMonitor(3, "��ҳ��ѯ�û���Ϣ!")]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "id";
            pagination.p_fields = "ItemName,ItemType,Score,IsAuto,CreateDate,createuserorgcode";
            pagination.p_tablename = "BIS_SCORESET";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "deptcode='00'";
            }
            else
            {
                pagination.conditionJson = string.Format(" (deptcode='00' or deptcode='{0}')", user.OrganizeCode);
            }

            var data = scoresetbll.GetPageJsonList(pagination, queryJson);
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
        public ActionResult GetForm(string keyValue)
        {
            var entity = scoresetbll.GetEntity(keyValue);
            return ToJsonResult(entity);
        }

        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            DataItemDetailBLL itemBll = new DataItemDetailBLL();
            ERCHTMS.Entity.SystemManage.DataItemDetailEntity entity = itemBll.GetEntity(keyValue);
            return ToJsonResult(entity);
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
            scoresetbll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        ///  <param name="score">��ʼ����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, string score)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DataItemDetailBLL itemBll = new DataItemDetailBLL();
            ERCHTMS.Entity.SystemManage.DataItemDetailEntity entity = itemBll.GetEntity(keyValue);
            if (entity != null)
            {
                entity.ItemCode = user.OrganizeCode;
                entity.ItemName = user.OrganizeName;
                entity.ItemId = "1234567890";
                entity.ItemValue = score;
                itemBll.SaveForm(keyValue, entity);
            }
            else
            {
                entity = new Entity.SystemManage.DataItemDetailEntity();
                entity.ItemDetailId = user.OrganizeId;
                entity.ParentId = "0";
                entity.ItemName = user.OrganizeName;
                entity.ItemCode = user.OrganizeCode;
                entity.ItemId = "1234567890";
                entity.ItemValue = score;
                itemBll.SaveForm(keyValue, entity);
            }

            return Success("�����ɹ���");
        }

        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        ///  <param name="score">��ʼ����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult Save(string keyValue, ScoreSetEntity entity)
        {
            scoresetbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }

        #endregion

        #region ���ݵ���
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HandlerMonitor(0, "��������")]
        public ActionResult Export(string queryJson)
        {
            Pagination pagination = new Pagination();
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "id";
            pagination.p_fields = "ItemName,ItemType,Score,case WHEN  isauto=0 then '��' else '��' end  as isautostr";
            pagination.p_tablename = "(select * from  BIS_SCORESET order by CreateDate desc) t";
            pagination.conditionJson = "1=1";
            pagination.page = 1;
            pagination.rows = 100000000;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "deptcode='00'";
            }
            else
            {
                pagination.conditionJson = string.Format(" (deptcode='00' or deptcode='{0}')", user.OrganizeCode);
            }

            var data = scoresetbll.GetPageJsonList(pagination, queryJson);
            //���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "���ֱ�׼����";
            excelconfig.TitleFont = "΢���ź�";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "���ֱ�׼����.xls";
            excelconfig.IsAllSizeColumn = true;
            //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();

            listColumnEntity.Add(new ColumnEntity() { Column = "ItemName".ToLower(), ExcelColumn = "������Ŀ", Alignment = "Center", Width = 20 });
            listColumnEntity.Add(new ColumnEntity() { Column = "ItemType".ToLower(), ExcelColumn = "��������", Alignment = "Center", Width = 20 });
            listColumnEntity.Add(new ColumnEntity() { Column = "score".ToLower(), ExcelColumn = "���ֱ�׼ֵ����/�Σ�", Alignment = "Center", Width = 20 });
            listColumnEntity.Add(new ColumnEntity() { Column = "isautostr".ToLower(), ExcelColumn = "�Ƿ�����ϵͳ�Զ�����", Alignment = "Center", Width = 20 });
            excelconfig.ColumnEntity = listColumnEntity;

            //���õ�������
            ExcelHelper.ExcelDownload(data, excelconfig);
            return Success("�����ɹ���");
        }
        #endregion

        #region ��������

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportData()
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
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
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    order = i;
                    //������Ŀ
                    string itemname = dt.Rows[i][0].ToString();

                    //��������
                    string itemtype = dt.Rows[i][1].ToString();
                    //���ֱ�׼ֵ����/�Σ�
                    string score = dt.Rows[i][2].ToString();
                    try
                    {
                        var item = new ScoreSetEntity { ItemName = itemname, ItemType = itemtype, Score = int.Parse(score), DeptCode = user.DeptCode, IsAuto = 0 };

                        scoresetbll.SaveForm("", item);

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
        #endregion

    }
}
