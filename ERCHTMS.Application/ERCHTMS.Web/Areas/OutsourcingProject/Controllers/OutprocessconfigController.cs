using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using System;
using System.Linq;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// �� ��������������ñ�
    /// </summary>
    public class OutprocessconfigController : MvcControllerBase
    {
        private OutprocessconfigBLL outprocessconfigbll = new OutprocessconfigBLL();

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
        public ActionResult Create()
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
        public ActionResult GetListJson()
        {
            var data = outprocessconfigbll.GetList();
            return ToJsonResult(data);
        }
        /// <summary>
        /// �Ƿ����������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult IsConfigExist()
        {
            try
            {
                Operator currUser = OperatorProvider.Provider.Current();
                var data = outprocessconfigbll.GetList().Where(x => x.DeptId == currUser.OrganizeId).ToList();
                return ToJsonResult(data.Count);
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }


        }
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "t.id";
                pagination.p_fields = @"b.fullname as deptname ";
                pagination.p_tablename = @"WF_SCHEMECONTENT t inner join base_department b on t.WFSCHEMEINFOID=b.departmentid";
                pagination.sidx = "t.createdate";//�����ֶ�
                pagination.sord = "desc";//����ʽ
                Operator currUser = OperatorProvider.Provider.Current();
                if (currUser.IsSystem)
                {
                    pagination.conditionJson = "  1=1 ";
                }
                //else if (currUser.RoleName.Contains("ʡ��"))
                //{
                //    pagination.conditionJson = string.Format(@" t.deptcode  in (select encode
                //        from base_department d
                //        where d.deptcode like '{0}%' and d.nature = '����' and d.description is null)", currUser.NewDeptCode);
                //}
                //else 
                //{
                //    pagination.conditionJson = string.Format(" t.deptid ='{0}' ", currUser.OrganizeId);
                //}

                var data = outprocessconfigbll.GetPageListJson(pagination, queryJson);


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
            var data = outprocessconfigbll.GetEntity(keyValue);
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
            outprocessconfigbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, OutprocessconfigEntity entity, string mode)
        {
            if (string.IsNullOrWhiteSpace(entity.FrontModuleCode) && string.IsNullOrWhiteSpace(entity.FrontModuleName))
            {
                entity.FrontModuleName = " ";
                entity.FrontModuleCode = " ";
            }
            if (mode == "Create")
            {
                var count = outprocessconfigbll.IsExistByModuleCode(entity.DeptId, entity.ModuleCode);
                if (count > 0)
                {
                    return Error("��ģ���Ѿ�����,�����ظ����á�");
                }
                else
                {
                    outprocessconfigbll.SaveForm(keyValue, entity);
                    return Success("���óɹ���");
                }
            }
            else
            {
                outprocessconfigbll.SaveForm(keyValue, entity);
                return Success("���óɹ���");
            }


        }
        #endregion
    }
}
