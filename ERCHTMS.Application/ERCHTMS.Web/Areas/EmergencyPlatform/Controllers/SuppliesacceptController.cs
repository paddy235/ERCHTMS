using ERCHTMS.Entity.EmergencyPlatform;
using ERCHTMS.Busines.EmergencyPlatform;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Data;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Code;
using System.Linq;
using ERCHTMS.Entity.BaseManage;
using Aspose.Words;
using System.Web;
using ERCHTMS.Busines.OutsourcingProject;
using System;

namespace ERCHTMS.Web.Areas.EmergencyPlatform.Controllers
{
    /// <summary>
    /// �� ����Ӧ��������������
    /// </summary>
    public class SuppliesacceptController : MvcControllerBase
    {
        private SuppliesacceptBLL suppliesacceptbll = new SuppliesacceptBLL();
        

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
        /// ����ͼ
        /// </summary>
        /// <returns></returns>
        public ActionResult Flow()
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
        public ActionResult GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.conditionJson = " 1=1 ";

                var data = suppliesacceptbll.GetPageList(pagination, queryJson);
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
            catch (System.Exception ex)
            {
                return Error(ex.ToString());
            }
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = suppliesacceptbll.GetList(queryJson);
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
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var data = suppliesacceptbll.GetEntity(keyValue);
                if (data.Status == 1)
                {
                    ManyPowerCheckBLL bll = new ManyPowerCheckBLL();
                    ManyPowerCheckEntity power= bll.GetListByModuleNo(user.OrganizeCode, "YJWZLYSP").OrderByDescending(t => t.SERIALNUM).FirstOrDefault();
                    ManyPowerCheckEntity flow = bll.GetEntity(data.FlowId);
                    if (power != null && power.SERIALNUM==flow.SERIALNUM)
                    {
                        data.IsLastAudit = true;
                    }
                    else
                    {
                        data.IsLastAudit = false;
                    }
                }
                else
                {
                    data.IsLastAudit = false;
                }
                return ToJsonResult(data);
            }
            catch (System.Exception ex)
            {
                return Error(ex.ToString());
            }
            
        }

        /// <summary>
        /// ��ȡ����ͼ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetFlow(string keyValue)
        {
            try
            {
                var data = suppliesacceptbll.GetFlow(keyValue, "Ӧ��������������");
                return ToJsonResult(data);
            }
            catch (System.Exception ex)
            {
                return Error(ex.ToString());
            }
        }
        
        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="keyValue"></param>
        public void ExportData(string keyValue)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            HttpResponse resp = System.Web.HttpContext.Current.Response;
            //�������
            string fileName = Server.MapPath("~/Resource/ExcelTemplate/Ӧ���������õ�ģ��.docx");
            Aspose.Words.Document doc = new Aspose.Words.Document(fileName);
            DocumentBuilder builder = new DocumentBuilder(doc);
            DataTable dt = new DataTable();
            dt.Columns.Add("applydept"); //���벿��
            dt.Columns.Add("applyperson"); //������ 
            dt.Columns.Add("applytime"); //����ʱ�� 
            dt.Columns.Add("reason"); //����ԭ��
            dt.Columns.Add("idea1");  //���Ÿ��������
            dt.Columns.Add("person1"); //���Ÿ�����
            dt.Columns.Add("time1"); //���Ÿ���������ʱ��
            dt.Columns.Add("idea2"); //���������������
            dt.Columns.Add("person2"); //������������
            dt.Columns.Add("time2"); //����������������ʱ��
            dt.Columns.Add("idea3");//���ϲ����������
            dt.Columns.Add("person3");//���ϲ�������
            dt.Columns.Add("time3"); //���ϲ�����������ʱ��
            dt.Columns.Add("idea4"); //�ֹ��쵼���        
            dt.Columns.Add("person4"); //�ֹ��쵼ǩ��
            dt.Columns.Add("time4"); //�ֹ��쵼ʱ��
            dt.Columns.Add("idea5"); //Ӧ�����ʳ������������
            dt.Columns.Add("person5"); //Ӧ�����ʳ�����
            dt.Columns.Add("time5"); //Ӧ�����ʳ���������ʱ��
            DataRow row = dt.NewRow();

            SuppliesacceptEntity entity = suppliesacceptbll.GetEntity(keyValue);
            row["applydept"] = entity.ApplyDept;
            row["applyperson"] = entity.ApplyPerson;
            row["applytime"] = entity.ApplyDate.Value.ToString("yyyy��MM��dd��HHʱmm��");
            row["reason"] = entity.AcceptReason;


            //������Ϣ
            DataTable dt1 = new DataTable("A");
            dt1.Columns.Add("num");
            dt1.Columns.Add("suppliesname");
            dt1.Columns.Add("models");
            dt1.Columns.Add("acceptnum");

            SuppliesAcceptDetailBLL suppliesacceptdetailbll = new SuppliesAcceptDetailBLL();

            var list = suppliesacceptdetailbll.GetList("").Where(t => t.RecId == keyValue).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                DataRow row1 = dt1.NewRow();
                row1["num"] = i + 1;
                row1["suppliesname"] = list[i].SuppliesName;
                row1["models"] = list[i].Models;
                row1["acceptnum"] = list[i].AcceptNum;
                dt1.Rows.Add(row1);
            }
            doc.MailMerge.ExecuteWithRegions(dt1);


            //��˼�¼
            AptitudeinvestigateauditBLL aptitudeinvestigateauditbll = new AptitudeinvestigateauditBLL();
            ManyPowerCheckBLL manypowercheckbll = new ManyPowerCheckBLL();
            var data = aptitudeinvestigateauditbll.GetAuditRecList(keyValue);
            for (int i = 0; i < data.Rows.Count; i++)
            {
                try
                {
                    var power = manypowercheckbll.GetEntity(data.Rows[i]["flowid"].ToString());
                    row["idea" + power.SERIALNUM] = data.Rows[i]["auditopinion"].ToString();
                    if (string.IsNullOrWhiteSpace(data.Rows[i]["auditsignimg"].ToString()))
                    {
                        row["person" + power.SERIALNUM] = Server.MapPath("~/content/Images/no_1.png");
                    }
                    else
                    {
                        var filepath = Server.MapPath("~/") + data.Rows[i]["auditsignimg"].ToString().Replace("../../", "").ToString();
                        if (System.IO.File.Exists(filepath))
                        {
                            row["person" + power.SERIALNUM] = filepath;
                        }
                        else
                        {
                            row["person" + power.SERIALNUM] = Server.MapPath("~/content/Images/no_1.png");
                        }
                    }
                    row["time" + power.SERIALNUM] = Convert.ToDateTime(data.Rows[i]["audittime"].ToString()).ToString("yyyy��MM��dd��");
                }
                catch (System.Exception ex)
                {
                }
            }
            dt.Rows.Add(row);
            doc.MailMerge.Execute(dt);
            doc.MailMerge.DeleteFields();
            doc.Save(resp, Server.UrlEncode("Ӧ���������õ�" + DateTime.Now.ToString("yyyyMMdd") + ".docx"), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Docx));
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
            suppliesacceptbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, SuppliesacceptEntity entity)
        {
            try
            {
                string message = "";
                message= suppliesacceptbll.SaveForm(keyValue, entity);
                if (!string.IsNullOrWhiteSpace(message))
                {
                    return Error(message);
                }
                else
                {
                    return Success("�����ɹ���");
                }
                
            }
            catch (System.Exception ex)
            {
                return Error(ex.ToString());
            }
        }

        /// <summary>
        /// ��˱�
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="aentity"></param>
        /// <param name="DetailData"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AuditForm(string keyValue, AptitudeinvestigateauditEntity aentity,string DetailData)
        {
            try
            {
                string message = "";
                message = suppliesacceptbll.AuditForm(keyValue, aentity, DetailData);
                if (!string.IsNullOrWhiteSpace(message))
                {
                    return Error(message);
                }
                else
                {
                    return Success("��˳ɹ���");
                }
            }
            catch (System.Exception ex)
            {
                return Error(ex.ToString());
            }
        }
        #endregion
    }
}
