using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System;
using ERCHTMS.Code;
using ERCHTMS.Busines.BaseManage;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// �� ����ͣ���������
    /// </summary>
    public class StopreturnworkController : MvcControllerBase
    {
        private StopreturnworkBLL stopreturnworkbll = new StopreturnworkBLL();
        private ReturntoworkBLL returnbll = new ReturntoworkBLL();
        private StartapplyforBLL startapplyforbll = new StartapplyforBLL();
        private DepartmentBLL departmentbll = new DepartmentBLL();

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
            var data = stopreturnworkbll.GetList(queryJson);
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
            var data = stopreturnworkbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡ����ʱ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetApplyRetrunTime(string outProjectId, string outEngId) {
            var applyReturnTime = "";
            var data = returnbll.GetApplyRetrunTime(outProjectId, outEngId);

            if (data == null) {
                var startData = startapplyforbll.GetApplyReturnTime(outProjectId, outEngId);
                if (startData != null)
                    applyReturnTime = startData.APPLYRETURNTIME.Value.ToString("yyyy-MM-dd");
            }else
                applyReturnTime = data.APPLYRETURNTIME.Value.ToString("yyyy-MM-dd");
            var resultData = new
            {
                APPLYRETURNTIME = applyReturnTime
            };
            return ToJsonResult(resultData);
        }
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "t.id";
                pagination.p_fields = @"t.createuserid,t.iscommit,
                                           t.outprojectid,
                                           t.outengineerid,
                                           t.stoptime,
                                           t.transmittime,
                                           t.transmitpeople,
                                           t.transmitpeopleid,
                                           t.acceptpeople,
                                           t.acceptpeopleid,
                                           b.fullname outprojectname,
                                           b.senddeptname,b.senddeptid,
                                           e.engineerletdept,
                                           e.engineername";
                pagination.p_tablename = @" epg_stopreturnwork t
                                              left join epg_outsouringengineer e on e.id=t.outengineerid
                                              left join base_department b on b.departmentid=t.outprojectid";
                pagination.sidx = "t.createdate";//�����ֶ�
                pagination.sord = "desc";//����ʽ
                Operator currUser = OperatorProvider.Provider.Current();
                if (currUser.IsSystem)
                {
                    pagination.conditionJson = " 1=1  ";
                }
                else if (currUser.RoleName.Contains("���������û�") || currUser.RoleName.Contains("��˾���û�"))
                {
                    pagination.conditionJson =string.Format("  (t.iscommit='1'or t.createuserid='{0}') ",currUser.UserId);
                }
                else if (currUser.RoleName.Contains("�а��̼��û�"))
                {
                    pagination.conditionJson = string.Format("  (t.OUTPROJECTID ='{0}' or e.supervisorid='{0}' or t.createuserid='{1}' )", currUser.DeptId, currUser.UserId);
                }
                else
                {
                    var deptentity = departmentbll.GetEntity(currUser.DeptId);
                    while (deptentity.Nature == "����" || deptentity.Nature == "רҵ")
                    {
                        deptentity = departmentbll.GetEntity(deptentity.ParentId);
                    }
                    pagination.conditionJson = string.Format(" (e.engineerletdeptid in (select departmentid from base_department where encode like '{0}%') and  t.iscommit='1' or t.createuserid='{1}') ", deptentity.EnCode, currUser.UserId);

                    //pagination.conditionJson = string.Format("  (e.engineerletdeptid ='{0}' and t.iscommit='1' or t.createuserid='{1}') ", currUser.DeptId, currUser.UserId);
                }

                var data = stopreturnworkbll.GetPageList(pagination, queryJson);
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

                throw new Exception(ex.Message);
            }

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
            stopreturnworkbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, StopreturnworkEntity entity)
        {
            stopreturnworkbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
