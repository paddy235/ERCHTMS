using ERCHTMS.Entity.PersonManage;
using ERCHTMS.Busines.PersonManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.BaseManage;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using ERCHTMS.Busines.SystemManage;
using System;

namespace ERCHTMS.Web.Areas.PersonManage.Controllers
{
    /// <summary>
    /// �� ������Ա����
    /// </summary>
    public class UserScoreController : MvcControllerBase
    {
        private UserScoreBLL userscorebll = new UserScoreBLL();

        #region ��ͼ����
        /// <summary>
        /// �����б�
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
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit()
        {
            return View();
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Rank()
        {
            return View();
        }
        /// <summary>
        /// ��Ա������ϸ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Details()
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
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "t.id as itemid";
            pagination.p_fields = "u.realname,u.Gender,u.identifyid,u.DEPTNAME,s.itemname,t.score,s.itemtype,u.OrganizeCode,u.DEPARTMENTCODE,u.realname as username,u.userid,t.createdate,t.createusername,case when s.isauto=0 then '�ֶ�����' else '�Զ�����' end isauto";
            pagination.p_tablename = "BIS_USERSCORE t left join v_userinfo u on t.userid=u.userid left join bis_scoreset s on t.itemid=s.id";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "departmentcode", "organizecode");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }

            }
            var data = userscorebll.GetPageJsonList(pagination, queryJson);
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
        public ActionResult GetRankListJson(Pagination pagination, string queryJson,string year)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "u.USERID";
            pagination.p_fields = "u.account,senddeptid,REALNAME,MOBILE,OrganizeName,DEPTNAME,DUTYNAME,POSTNAME,usertype,GENDER,u.OrganizeCode,u.CreateDate,isblack,identifyid,nvl(score,0) score,BIRTHDAY";
            pagination.p_tablename = "v_userinfo u left join (select a.userid,nvl(sum(score),0) as score from base_user a left join bis_userscore b on a.userid=b.userid where year='" + year + "' group by a.userid) t on u.userid=t.userid";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "departmentcode", "organizecode");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }

            }
          

            DataItemDetailBLL itemBll = new DataItemDetailBLL();
            ERCHTMS.Entity.SystemManage.DataItemDetailEntity entity = itemBll.GetEntity(user.OrganizeId);
            var data = userscorebll.GetPageJsonList(pagination, queryJson);
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch),
                userdata = new { score = entity == null ? "100" : entity.ItemValue }

            };
            return Content(JsonData.ToJson());
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = userscorebll.GetList(queryJson);
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
            var data = userscorebll.GetInfo(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡ��Ա���� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpPost]
        public string GetScoreInfo(string userId)
        {
            var data = userscorebll.GetScoreInfo(userId);
            return data;
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
            userscorebll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="userIds">�û�Id(����ö��ŷָ�)</param>
        /// <param name="itemIds">������ĿId(����ö��ŷָ�)</param>
        /// <param name="scores">���˷�ֵ(����ö��ŷָ�)</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string userIds,string itemIds,string scores)
        {
            try
            {
                 string[] arr = userIds.Trim(',').Split(',');
                 string[] ids = itemIds.Trim(',').Split(',');
                 string[] arrScore = scores.Trim(',').Split(',');
                 Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                 string currUserId=user.UserId;
                 string deptCode=user.DeptCode;
                 string orgCode=user.OrganizeCode;
                 List<UserScoreEntity> list = new List<UserScoreEntity>();
                 foreach(string userid in arr)
                {
                  int j = 0;
                  foreach(string itemId in ids)
                  {
                    UserScoreEntity us = new UserScoreEntity
                    {
                        Id = System.Guid.NewGuid().ToString(),
                        UserId = userid,
                        ItemId = itemId,
                        CreateUserName=user.UserName,
                        Score = decimal.Parse(arrScore[j]),
                        Year=System.DateTime.Now.Year.ToString(),
                        CreateDate = System.DateTime.Now,
                        CreateUserId=currUserId,
                        CreateUserDeptCode=deptCode,
                        CreateUserOrgCode = orgCode
                    };
                    list.Add(us);
                   j++;
                }
               
            }
            userscorebll.Save(list);
            return Success("�����ɹ���");
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
           
        }

        /// <summary>
        /// ������Ա��ȫ��������
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "������Ա��ȫ��������")]
        public ActionResult ExportScoreRank(string condition, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DataItemDetailBLL itemBll = new DataItemDetailBLL();
            var item = itemBll.GetEntity(user.OrganizeId);
            string val = item==null ? "100" : item.ItemValue;
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "u.userid";
            pagination.p_fields = "REALNAME,GENDER,identifyid,DEPTNAME,(nvl(score,0)+" + val + ") score";
            pagination.p_tablename = "v_userinfo u left join (select a.userid,nvl(sum(score),0) as score from base_user a left join bis_userscore b on a.userid=b.userid where year='" + DateTime.Now.Year + "' group by a.userid) t on u.userid=t.userid";
            pagination.conditionJson = "1=1 ";
            pagination.sidx = "score";
            pagination.sord = "desc";
          
            string title = "��Ա��ȫ��������";
            
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "departmentcode", "organizecode");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }

            }
            var data = userscorebll.GetPageJsonList(pagination, queryJson);

            //���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = title;
            excelconfig.TitleFont = "΢���ź�";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = title + ".xls";
            excelconfig.IsAllSizeColumn = true;
            //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity = listColumnEntity;
            ColumnEntity columnentity = new ColumnEntity();
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "realname", ExcelColumn = "����", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "gender", ExcelColumn = "�Ա�", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "identifyid", ExcelColumn = "���֤��", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "deptname", ExcelColumn = "��λ/����", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "score", ExcelColumn = "����", Alignment = "center" });
            //���õ�������
            ExcelHelper.ExcelDownload(data, excelconfig);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
