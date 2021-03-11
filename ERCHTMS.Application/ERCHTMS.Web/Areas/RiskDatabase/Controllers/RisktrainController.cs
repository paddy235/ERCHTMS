using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.Busines.RiskDatabase;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Collections.Generic;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using BSFramework.Util.Offices;
using System.Data;

namespace ERCHTMS.Web.Areas.RiskDatabase.Controllers
{
    /// <summary>
    /// �� ��������Ԥ֪ѵ��
    /// </summary>
    public class RisktrainController : MvcControllerBase
    {
        private RisktrainBLL risktrainbll = new RisktrainBLL();
        private RiskEvaluateBLL riskevaluatebll = new RiskEvaluateBLL();
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
        public ActionResult Show()
        {
            return View();
        }
        /// <summary>
        /// ����ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddForm()
        {
            return View();
        }
        /// <summary>
        /// ����ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EvaluateForm()
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
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var watch = CommonHelper.TimerStart();
            pagination.p_tablename = "BIS_RISKTRAIN";
            pagination.p_kid = "id";
            pagination.p_fields = @"taskname,worktype,postname,taskcontent,Status,userids,createuserid,workusers,workfzr,workunit,to_char(workstarttime, 'yyyy-MM-dd HH:mm') workstarttime,
       to_char(workendtime, 'yyyy-MM-dd HH:mm')  workendtime,iscommit,workfzrid ";
            pagination.conditionJson = "(((userids like '%," + user.Account + ",%' and iscommit=1) or (createuserid='" + user.UserId + "') or (workfzrid='" + user.Account + "' and iscommit=1))";
            if (!user.IsSystem)
            {
                //���ݵ�ǰ�û���ģ���Ȩ�޻�ȡ��¼
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "createuserdeptcode", "createuserorgcode");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " or (" + where + " and iscommit=1)";
                }
                
            }
            pagination.conditionJson += ")";
            var data = risktrainbll.GetPageListJson(pagination, queryJson);
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
            var data = risktrainbll.GetList(queryJson);
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
            var data = risktrainbll.GetEntity(keyValue);
            
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡ���մ�ʩ
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetMeasures(string workId)
        {
            TrainmeasuresBLL tm = new TrainmeasuresBLL();
            var data = tm.GetListByWorkId(workId);
            return ToJsonResult(data);
        }
        [HttpGet]
        public ActionResult GetPageMeasures(Pagination pagination, string queryJson)
        {
            TrainmeasuresBLL tm = new TrainmeasuresBLL();
            var watch = CommonHelper.TimerStart();
            pagination.p_tablename = "bis_trainmeasures";
            pagination.p_kid = "id";
            pagination.p_fields = @"riskcontent,measure,workid,status,createdate,lspeople ";
            pagination.conditionJson = "1=1";
            pagination.rows = 1000000;
            pagination.page = 1;
            var data = tm.GetPageListByWorkId(pagination,queryJson);
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

        [HttpGet]
        public ActionResult GetEvaluatePageList(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var watch = CommonHelper.TimerStart();
            pagination.p_tablename = "BIS_EVALUATE";
            pagination.p_kid = "id";
            pagination.p_fields = @"createuserid,createdate,createusername,workid,evaluatescore,evaluatecontent ";
            pagination.conditionJson = "1=1";
            var data = riskevaluatebll.GetPageList(pagination, queryJson);
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
            risktrainbll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }
        /// <summary>
        /// ɾ����������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveEvaluate(string keyValue)
        {
            riskevaluatebll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }

        
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <param name="listMesures">��ع������񼰴�ʩ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, RisktrainEntity entity,string measuresJson)
        {
            List<TrainmeasuresEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TrainmeasuresEntity>>(measuresJson);
            risktrainbll.SaveForm(keyValue, entity, list);
            return Success("�����ɹ���");
        }
        ///// <summary>
        ///// ��������������޸ģ�
        ///// </summary>
        ///// <param name="keyValue">����ֵ</param>
        ///// <param name="entity">ʵ�����</param>
        ///// <param name="listMesures">��ع������񼰴�ʩ</param>
        ///// <returns></returns>
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[AjaxOnly]
        //public ActionResult CommitForm(string keyValue, RisktrainEntity entity, string measuresJson)
        //{
        //    List<TrainmeasuresEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TrainmeasuresEntity>>(measuresJson);
        //    risktrainbll.SaveForm(keyValue, entity, list);
        //    return Success("�����ɹ���");
        //}

         [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult CommitEvaluate(string keyValue, RiskEvaluate entity)
        {
            riskevaluatebll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
         public ActionResult ExportExcel(string queryJson, string fileName)
         {
             Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
             var watch = CommonHelper.TimerStart();
             Pagination pagination = new Pagination();
             pagination.page = 1;
             pagination.rows = 1000000000;
             pagination.p_tablename = "bis_risktrain";
             pagination.p_kid = "id";
             pagination.p_fields = @"taskname,workunit,workfzr,workusers,to_char(workstarttime, 'yyyy-MM-dd HH:mm') workstarttime,to_char(workendtime, 'yyyy-MM-dd HH:mm')  workendtime,taskcontent,decode(status,1,'�����',0,'δ���','','δ���')status ";
             pagination.conditionJson = "((userids like '%," + user.Account + ",%' or createuserid='" + user.UserId + "' or workfzrid='" + user.Account + "')";
             if (!user.IsSystem)
             {
                 //���ݵ�ǰ�û���ģ���Ȩ�޻�ȡ��¼
                 string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "createuserdeptcode", "createuserorgcode");
                 if (!string.IsNullOrEmpty(where))
                 {
                     pagination.conditionJson += " or " + where;
                 }

             }
             pagination.conditionJson += ")";
             DataTable data = risktrainbll.GetPageListJson(pagination, queryJson);
             //���õ�����ʽ
             ExcelConfig excelconfig = new ExcelConfig();
             excelconfig.Title = "����Ԥ֪ѵ��";
             excelconfig.FileName = fileName + ".xls";
             //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
             List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
             excelconfig.ColumnEntity = listColumnEntity;
             ColumnEntity columnentity = new ColumnEntity();
             excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "taskname", ExcelColumn = "��������", Width = 50 });
             excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "workunit", ExcelColumn = "��ҵ��λ", Width = 20 });
             excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "workfzr", ExcelColumn = "��ҵ������", Width = 20 });
             excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "workusers", ExcelColumn = "��ҵ��Ա", Width = 50 });
             excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "workstarttime", ExcelColumn = "��ҵ��ʼʱ��", Width = 20 });
             excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "workendtime", ExcelColumn = "��ҵ����ʱ��", Width = 20 });
             excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "taskcontent", ExcelColumn = "��ҵ��������", Width = 50 });
             excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "status", ExcelColumn = "״̬", Width = 10 });
             //���õ�������
             ExcelHelper.ExcelDownload(data, excelconfig);

             return Success("�����ɹ���");
         }
        #endregion
    }
}
