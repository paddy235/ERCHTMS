using ERCHTMS.Entity.HazardsourceManage;
using ERCHTMS.Busines.HazardsourceManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.HiddenTroubleManage;
using System.Linq;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using BSFramework.Util.Offices;
using System.Collections.Generic;

namespace ERCHTMS.Web.Areas.HazardsourceManage.Controllers
{
    /// <summary>
    /// �� �������
    /// </summary>
    public class JkjcController : MvcControllerBase
    {
        private JkjcBLL jkjcbll = new JkjcBLL();
        private HazardsourceBLL hazardsourcebll = new HazardsourceBLL();
        #region ��ͼ����
        public ActionResult yhtz()
        {
            return View();
        }


        /// <summary>
        /// �Ǽ�����
        /// </summary>
        /// <returns></returns>
        public ActionResult djyh()
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
            var data = jkjcbll.GetList(queryJson);
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
            var data = jkjcbll.GetEntity(keyValue);
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
            jkjcbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, JkjcEntity entity)
        {           
            //��ѯ��������
            HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL(); //����������Ϣ
            var list = htbaseinfobll.GetList(" and relevanceType='DangerSource' and RelevanceId='" + entity.ID + "'").ToList();
            entity.JkyhzgIds = list.Count.ToString();
            jkjcbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion


        #region ���ݵ���
        /// <summary>
        /// �����û��б�
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "��������")]
        public ActionResult Export(string condition, string queryJson)
        {
            Pagination pagination = new Pagination();
            queryJson = queryJson ?? "";
            pagination.p_kid = "ID";
            pagination.p_fields = "districtname, DANGERSOURCE, jkarear,jktimestart,jktimeend,JkyhzgIds,case WHEN  jkskstatus>0 then '���ܿ�' WHEN  jkskstatus=0 then '���ܿ�' else 'δ���' end as jkskstatusname";
            pagination.p_tablename = "V_HSD_DANGERQD_JK t";
            pagination.conditionJson = "1=1";
            pagination.page = 1;
            pagination.rows = 100000000;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                if (user.RoleName.Contains("ʡ��"))
                {
                    pagination.conditionJson += " and (CreateUserId='" + user.UserId + "' or DeptCode in(select  encode from BASE_DEPARTMENT start with encode='" + user.NewDeptCode + "' connect by  prior  departmentid = parentid))";
                }
                else
                {
                    pagination.conditionJson += " and (CreateUserId='" + user.UserId + "' or DeptCode in(select  encode from BASE_DEPARTMENT start with encode='" + user.DeptCode + "' connect by  prior  departmentid = parentid))";
                }
                //pagination.conditionJson += " and (CreateUserId='" + user.UserId + "' or DeptCode like '" + user.DeptCode + "%')";
            }



            var watch = CommonHelper.TimerStart();
            var data = hazardsourcebll.GetPageList(pagination, queryJson);

            //���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "�ش�Σ��Դ��ؼ��";
            excelconfig.TitleFont = "΢���ź�";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "�ش�Σ��Դ��ؼ��.xls";
            excelconfig.IsAllSizeColumn = true;
            //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();

            listColumnEntity.Add(new ColumnEntity() { Column = "districtname", ExcelColumn = "��������" });
            listColumnEntity.Add(new ColumnEntity() { Column = "DANGERSOURCE".ToLower(), ExcelColumn = "Σ��Դ����/����" });
            listColumnEntity.Add(new ColumnEntity() { Column = "jkarear".ToLower(), ExcelColumn = "��صص�" });
            listColumnEntity.Add(new ColumnEntity() { Column = "jktimestart".ToLower(), ExcelColumn = "���ʱ����" });
            listColumnEntity.Add(new ColumnEntity() { Column = "jktimeend".ToLower(), ExcelColumn = "���ʱ��ֹ" });
            listColumnEntity.Add(new ColumnEntity() { Column = "JkyhzgIds".ToLower(), ExcelColumn = "����" });
            listColumnEntity.Add(new ColumnEntity() { Column = "jkskstatusname".ToLower(), ExcelColumn = "���״̬" });
            excelconfig.ColumnEntity = listColumnEntity;

            //���õ�������
            ExcelHelper.ExcelDownload(data, excelconfig);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
