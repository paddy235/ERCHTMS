using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using System;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.BaseManage;
using System.Linq;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// �� ���������λ������Ϣ��
    /// </summary>
    public class OutsourcingprojectController : MvcControllerBase
    {
        private OutsourcingprojectBLL outsourcingprojectbll = new OutsourcingprojectBLL();
        private AptitudeinvestigateinfoBLL aptitudeinvestigateinfobll = new AptitudeinvestigateinfoBLL();
        private FileInfoBLL fileBll = new FileInfoBLL();

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

        [HttpGet]
        public ActionResult StatisticDefault()
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
            var data = outsourcingprojectbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ҳ�б�
        /// servicesstarttime ����ʵ�ʿ���ʱ����Сֵ
        /// servicesendtime   ����ʵ���깤ʱ�����ֵ
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
         [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "t.id";
                pagination.p_fields = @"t.outsourcingname, 
                                            s.servicesstarttime, 
                                            s.servicesendtime, 
                                            s.engineerletdeptid,
                                            decode(t.outorin,'0','�볡','1','�볡','') outorin,
                                            t.legalrep,
                                            t.legalrepphone,
                                            t.outprojectid,b.managerid,
                                            b.senddeptid,b.createdate";
                pagination.p_tablename = @" base_department b
                                      inner join epg_outsourcingproject t on b.departmentid = t.outprojectid
                                      left join (select min(e.planenddate) servicesstarttime,
                                                    max(e.actualenddate) servicesendtime,e.outprojectid outprojectid,max(e.engineerletdeptid) engineerletdeptid
                                      from  epg_outsouringengineer e 
                                      group by e.outprojectid)s on   t.outprojectid=s.outprojectid ";
                pagination.sidx = "b.createdate";//�����ֶ�
                pagination.sord = "desc";//����ʽ
                Operator currUser = OperatorProvider.Provider.Current();
                if (currUser.IsSystem)
                {
                    pagination.conditionJson = "  1=1 ";
                }
                else if (currUser.RoleName.Contains("ʡ��"))
                {
                    pagination.conditionJson = string.Format(@" b.encode  in (select encode
                        from base_department d
                        where d.deptcode like '{0}%' and d.description is null)", currUser.NewDeptCode);
                }
                else if (currUser.RoleName.Contains("���������û�") || currUser.RoleName.Contains("��˾���û�"))
                {
                    pagination.conditionJson = string.Format(" b.encode like '{0}%' ", currUser.OrganizeCode);
                }
                else if (currUser.RoleName.Contains("�а��̼��û�"))
                {
                    pagination.conditionJson = string.Format("  b.departmentid ='{0}'", currUser.DeptId);
                }
                else
                {
                    pagination.conditionJson = string.Format("  b.departmentid in(select distinct(t.outprojectid) from EPG_OUTSOURINGENGINEER t where t.engineerletdeptid='{0}')", currUser.DeptId);
                }
                pagination.conditionJson += string.Format(" and t.blackliststate='0' and b.parentid in (select departmentid from base_department t where t.description ='������̳а���' and Organizeid='{0}') ", currUser.OrganizeId);
                var data = outsourcingprojectbll.GetPageList(pagination, queryJson);


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
        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = outsourcingprojectbll.GetEntity(keyValue);
            var zzData = aptitudeinvestigateinfobll.GetListByOutprojectId(data.OUTPROJECTID);

            var resultData = new
            {
                data = data,
                zzData = zzData
            };
            return ToJsonResult(resultData);
        }

        [HttpGet]
        public ActionResult GetEntityByDeptId(string DeptId)
        {
            var data = outsourcingprojectbll.GetOutProjectInfo(DeptId);
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
            outsourcingprojectbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, OutsourcingprojectEntity entity)
        {
            outsourcingprojectbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        /// <summary>
        /// �볡/�볡
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult Leave(string keyValue,string state)
        {
            OutsourcingprojectEntity pro= outsourcingprojectbll.GetEntity(keyValue);
            //��λ�볡ʱ,�õ�λ�µ���Աȫ���볡
            if (state == "1")
            {
                pro.OUTORIN = state;
                pro.LEAVETIME = DateTime.Now;
               var userList= new UserBLL().GetList().Where(x => x.DepartmentId == pro.OUTPROJECTID).ToList();
                if (userList.Count > 0) {
                    for (int i = 0; i < userList.Count; i++)
                    {
                        userList[i].DepartureTime = DateTime.Now;
                        userList[i].IsPresence = "0";
                        new UserBLL().SaveForm(userList[i].UserId,userList[i]);
                    }
                }
            }
            else {
                pro.OUTORIN = state;
                pro.LEAVETIME = null;
            }
            //pro.OUTORIN = state;
            //pro.LEAVETIME = DateTime.Now;
            outsourcingprojectbll.SaveForm(keyValue, pro);
            return Success("�����ɹ���");
        }
        #endregion


        [HttpGet]
        public string StaQueryList(string queryJson)
        {

            return outsourcingprojectbll.StaQueryList(queryJson);
        }
        [HttpGet]

        public ActionResult GetAllFactory() {
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            return ToJsonResult(new DepartmentBLL().GetAllFactory(user));
        }
    }
}
