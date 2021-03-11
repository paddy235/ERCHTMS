using ERCHTMS.Entity.PersonManage;
using ERCHTMS.Busines.PersonManage;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Linq;
using ERCHTMS.Busines.AuthorizeManage;
using System;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Code;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using ERCHTMS.Busines.SystemManage;
namespace ERCHTMS.Web.Areas.PersonManage.Controllers
{
    /// <summary>
    /// �� ������Ա֤��
    /// </summary>
    public class BlacklistController : MvcControllerBase
    {
        private BlacklistBLL certificatebll = new BlacklistBLL();

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
        /// <param name="userId">�û�Id</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string userId)
        {
            var data = certificatebll.GetList(userId).ToList();
            return ToJsonResult(data);
            //var JsonData = new
            //{
            //    rows = data,
            //    total = data.Count,
            //    page = 1,
            //    records = 1
            //};
            //return Content(JsonData.ToJson());
        }
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "id";
            pagination.p_fields = "REALNAME,GENDER,identifyid,DEPTNAME,reason,jointime,a.userid";
            pagination.p_tablename = "bis_blacklist a left join v_userinfo u on a.userid=u.userid";
            pagination.conditionJson = "isblack=1";

            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string where = "";
            if (user.RoleName.Contains("ʡ��"))
            {
                var queryParam = queryJson.ToJObject();
                if (!queryParam["departmentCode"].IsEmpty())
                {
                    var dept = new DepartmentBLL().GetEntityByCode(queryParam["departmentCode"].ToString());
                    if (dept != null)
                    {
                        if (dept.Nature == "ʡ��")
                        {
                            pagination.conditionJson += string.Format(" and departmentCode in(select encode from BASE_DEPARTMENT where deptcode like '{0}%')", dept.DeptCode);
                        }
                        else
                        {
                            pagination.conditionJson += string.Format(" and departmentCode  like '{0}%'", dept.EnCode);
                        }
                    }
                }
            }
            else
            {
                where = "departmentcode like '" + user.DeptCode + "%'";
                if (!user.IsSystem)
                {
                    string con = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "departmentcode", "organizecode");
                    if (!string.IsNullOrEmpty(con))
                    {
                        where = con;
                        pagination.conditionJson += " and " + where;
                    }
                    else
                    {
                        where = "1=1";
                    }
                }

                // where = new AuthorizeBLL().GetModuleDataAuthority(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "DEPARTMENTCODE", "OrganizeCode");
                //pagination.conditionJson = string.IsNullOrEmpty(where) ? pagination.conditionJson : pagination.conditionJson + " and " + where;
            }
            var watch = CommonHelper.TimerStart();
            var data = new UserBLL().GetPageList(pagination, queryJson);
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
            var data = certificatebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡ�����������û�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpPost]
        public ActionResult GetBlacklistUsers()
        {
            Operator user = OperatorProvider.Provider.Current();
            var data = certificatebll.GetBlacklistUsers(user);
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
            certificatebll.RemoveForm(keyValue);
            SaveForbidden(keyValue, "", 1);
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
        public ActionResult SaveForm(string keyValue, BlacklistEntity entity)
        {
            certificatebll.SaveForm(keyValue, entity);
            SaveForbidden(entity.UserId, entity.Reason, 0);
            return Success("�����ɹ���");
        }

        /// <summary>
        /// ͬ���޸Ļ�����ŵ糧����ƽ̨����Ȩ�ޣ���������
        /// </summary>
        /// <param name="entity"></param>
        public void SaveForbidden(string UserId, string Reason, int type)
        {
            //˵����������������൱�ڡ�˫���볧�������������
            DataItemDetailBLL itemBll = new DataItemDetailBLL();
            string KMIndex = itemBll.GetItemValue("KMIndexUrl");
            if (!string.IsNullOrEmpty(KMIndex))
            {//ֻ������ŵ糧��Աִ�иò���
                List<TemporaryUserEntity> tempuserList = new TemporaryGroupsBLL().GetUserList();//������ʱ��Ա
                List<TemporaryUserEntity> list = new List<TemporaryUserEntity>();
                if (type == 0)
                {//����
                    foreach (var uid in UserId.Split(','))
                    {
                        var uentity = tempuserList.Where(t => t.USERID == uid).FirstOrDefault();
                        if (uentity != null)
                        {
                            uentity.EndTime = DateTime.Now;
                            uentity.Remark = Reason;
                            list.Add(uentity);
                        }
                    }
                    new TemporaryGroupsBLL().SaveForbidden(list);
                }
                else
                {//�Ƴ�
                    new TemporaryGroupsBLL().RemoveForbidden(UserId);
                }
            }
        }

        /// <summary>
        /// ���������
        /// </summary>
        /// <param name="leaveTime">�볡ʱ��</param>
        /// <param name="userId">�û�Id,����ö��ŷָ�</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "��Ա�볡")]
        public ActionResult Leave(string leaveTime, [System.Web.Http.FromBody]string userId)
        {
            try
            {
                UserBLL userBLL = new UserBLL();
                userBLL.SetBlack(userId, 1);
                return Success("�����ɹ���");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        #endregion


        #region ���ݵ���
        /// <summary>
        /// �¹��¼��챨
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "��Ա�������嵥")]
        public ActionResult Export(string queryJson)
        {
            Pagination pagination = new Pagination();
            queryJson = queryJson ?? "";
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "id";
            pagination.p_fields = "REALNAME,GENDER,identifyid,DEPTNAME,jointime,reason";
            pagination.p_tablename = "bis_blacklist a left join v_userinfo u on a.userid=u.userid";
            pagination.conditionJson = "isblack=1";
            string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "DEPARTMENTCODE", "OrganizeCode");
            pagination.conditionJson = string.IsNullOrEmpty(where) ? pagination.conditionJson : pagination.conditionJson + " and " + where;
            var watch = CommonHelper.TimerStart();
            var data = new UserBLL().GetPageList(pagination, queryJson);

            //���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "��Ա������";
            excelconfig.TitleFont = "΢���ź�";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "��Ա������.xls";
            excelconfig.IsAllSizeColumn = true;
            //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();

            listColumnEntity.Add(new ColumnEntity() { Column = "REALNAME".ToLower(), ExcelColumn = "����", Alignment = "Center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "GENDER".ToLower(), ExcelColumn = "�Ա�", Alignment = "Center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "identifyid".ToLower(), ExcelColumn = "���֤��", Alignment = "Center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "DEPTNAME".ToLower(), ExcelColumn = "��λ/����", Alignment = "Center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "jointime".ToLower(), ExcelColumn = "���������ʱ��", Alignment = "Center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "reason".ToLower(), ExcelColumn = "���������ԭ��", Alignment = "Center" });
            excelconfig.ColumnEntity = listColumnEntity;

            //���õ�������
            ExcelHelper.ExcelDownload(data, excelconfig);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
