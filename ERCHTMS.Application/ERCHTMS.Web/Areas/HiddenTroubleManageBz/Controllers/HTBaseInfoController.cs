using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Cache;
using System.Web;
using System.IO;
using NPOI.HSSF.UserModel;
using ERCHTMS.Busines.SaftyCheck;
using System.Data;
using ERCHTMS.Entity.SystemManage;
using BSFramework.Util.Offices;


namespace ERCHTMS.Web.Areas.HiddenTroubleManageBz.Controllers
{
    /// <summary>
    /// �� ��������������Ϣ��
    /// </summary>
    public class HTBaseInfoController : MvcControllerBase
    {
        private HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL(); //����������Ϣ
        private HTChangeInfoBLL htchangeinfobll = new HTChangeInfoBLL(); //����������Ϣ
        private HTApprovalBLL htapprovalbll = new HTApprovalBLL(); //����������Ϣ
        private HTAcceptInfoBLL htacceptinfobll = new HTAcceptInfoBLL(); //����������Ϣ
        private HTEstimateBLL htestimatebll = new HTEstimateBLL(); //����Ч��������Ϣ
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL(); //��������
        private UserBLL userbll = new UserBLL(); //�û���������
        private SaftyCheckDataRecordBLL saftycheckdatarecordbll = new SaftyCheckDataRecordBLL();

        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private OrganizeCache organizeCache = new OrganizeCache();
        private DepartmentBLL departmentBLL = new DepartmentBLL();

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

        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NewForm()
        {
            return View();
        }


        /// <summary>
        /// ��ȡ�ع��б�
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExposureIndex()
        {
            return View();
        }

        /// <summary>
        /// ����ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ImportForm()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DetailList()
        {
            return View();
        }


        /// <summary>
        /// ��ȡ��ǰ�û��Ƿ�Ϊ��ȫ����Ա���
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult QuerySafetyRole()
        {
            //��ȫ����Ա��ɫ����
            string roleCode = dataitemdetailbll.GetItemValue("HidApprovalSetting");

            Operator curUser = OperatorProvider.Provider.Current();

            string uModel = string.Empty;

            string HidApproval = dataitemdetailbll.GetItemValue("HidApproval");

            string[] pstr = HidApproval.Split('#');  //�ָ�������

            IList<UserEntity> ulist = new List<UserEntity>();

            foreach (string strArgs in pstr)
            {
                string[] str = strArgs.Split('|');
                //��ǰ������ͬ����Ϊ�����Ű�ȫ����Ա��֤
                if (str[0].ToString() == curUser.OrganizeId && str[1].ToString() == "0")
                {
                    ulist = userbll.GetUserListByRole(curUser.DeptCode, roleCode, curUser.OrganizeId).ToList();

                    break;
                }
                if (str[0].ToString() == curUser.OrganizeId && str[1].ToString() == "1")
                {
                    //��ȡָ�����ŵ�������Ա
                    ulist = new UserBLL().GetUserListByDeptCode(str[2].ToString(), null, false, curUser.OrganizeId).ToList();

                    break;
                }
            }

            if (ulist.Count() > 0)
            {
                //���صļ�¼��,����0����ʶ��ǰ�û�ӵ�а�ȫ����Ա��ݣ���֮����
                uModel = ulist.Where(p => p.UserId == curUser.UserId).Count().ToString();
            }
            return Content(uModel);
        }


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
            Operator opertator = new OperatorProvider().Current();
            string userId = opertator.UserId;
            queryJson = queryJson.Insert(1, "\"userId\":\"" + userId + "\","); //��ӵ�ǰ�û�
            queryJson = queryJson.Insert(1, "\"isPlanLevel\":\"" + opertator.isPlanLevel + "\","); //��ӵ�ǰ�Ƿ�˾������
            var data = htbaseinfobll.GetHiddenBaseInfoPageList(pagination, queryJson);
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
            //����������Ϣ
            var baseInfo = htbaseinfobll.GetEntity(keyValue);

            var changeInfo = htchangeinfobll.GetEntityByHidCode(baseInfo.HIDCODE);

            var acceptInfo = htacceptinfobll.GetEntityByHidCode(baseInfo.HIDCODE);

            var estimateInfo = htestimatebll.GetEntityByHidCode(baseInfo.HIDCODE);

            var userInfo = OperatorProvider.Provider.Current();  //��ȡ��ǰ�û�

            var data = new { baseInfo = baseInfo, changeInfo = changeInfo, acceptInfo = acceptInfo, userInfo = userInfo, estimateInfo = estimateInfo };

            return ToJsonResult(data);
        }

        /// <summary>
        /// Υ���б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>   
        //[HandlerMonitor(3, "��ҳ��ѯ�û���Ϣ!")]
        [HttpGet]
        public ActionResult GetRulerPageListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var data = htbaseinfobll.GetRulerPageList(pagination, queryJson);
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
        #endregion


        #region ��ȡ��Ŀ����  �а����¸���Ŀ
        /// <summary>
        /// ��ȡ��Ŀ����  �а����¸���Ŀ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetProjectDataJson()
        {

            var organizedata = organizeCache.GetList();
            var departmentdata = departmentBLL.GetList().Where(p => p.Nature == "�а���" || p.Nature == "��Ŀ");
            var treeList = new List<TreeEntity>();
            foreach (OrganizeEntity item in organizedata)
            {
                #region ����
                TreeEntity tree = new TreeEntity();
                bool hasChildren = organizedata.Count(t => t.ParentId == item.OrganizeId) == 0 ? false : true;
                if (hasChildren == false)
                {
                    hasChildren = departmentdata.Count(t => t.OrganizeId == item.OrganizeId) == 0 ? false : true;
                }
                tree.id = item.OrganizeId;
                tree.text = item.FullName;
                tree.value = item.OrganizeId;
                tree.parentId = item.ParentId;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChildren;
                tree.Attribute = "Sort";
                tree.AttributeValue = "Organize";
                treeList.Add(tree);
                #endregion
            }
            foreach (DepartmentEntity item in departmentdata)
            {
                #region ����
                TreeEntity tree = new TreeEntity();
                bool hasChildren = departmentdata.Count(t => t.ParentId == item.DepartmentId) == 0 ? false : true;
                tree.id = item.DepartmentId;
                tree.text = item.FullName;
                tree.value = item.DepartmentId;
                if (item.ParentId == "0")
                {
                    tree.parentId = item.OrganizeId;
                }
                else
                {
                    tree.parentId = item.ParentId;
                }
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChildren;
                tree.Attribute = "Sort";
                if (item.Nature == "�а���")
                {
                    tree.AttributeValue = "Contractor";
                }
                if (item.Nature == "��Ŀ")
                {
                    tree.AttributeValue = "Project";
                }
                treeList.Add(tree);
                #endregion
            }
            return Content(treeList.TreeToJson());
        }
        #endregion


        [HttpGet]
        public ActionResult IsOrginazeLeader()
        {
            Operator curUser = OperatorProvider.Provider.Current();

            //��˾���û�
            if (userbll.HaveRoleListByKey(curUser.UserId, dataitemdetailbll.GetItemValue("HidOrganize")).Rows.Count > 0)
            {
                curUser.DeptCode = curUser.OrganizeCode;
                curUser.DeptName = curUser.OrganizeName;
            }
            return Content(curUser.ToJson());
        }

        #region ҳ�������ʼ��
        /// <summary>
        /// ��ʼ������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetInitDataJson()
        {
            Operator CreateUser = OperatorProvider.Provider.Current();
            //��������
            string HidCode = DateTime.Now.ToString("yyyyMMddHHmmssfff").ToString();
            //��ȫ������� �������� ��������  ��ѵģ��
            string itemCode = "'SaftyCheckType','HidType','HidRank','TrainTemplateName'";
            //����
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode(itemCode);

            //��˾���û�
            if (userbll.HaveRoleListByKey(CreateUser.UserId, dataitemdetailbll.GetItemValue("HidOrganize")).Rows.Count > 0)
            {
                CreateUser.DeptCode = CreateUser.OrganizeCode;
                CreateUser.DeptName = CreateUser.OrganizeName;
            }

            //����ֵ
            var josnData = new
            {
                CreateUser = CreateUser.UserName,
                User = CreateUser,  //�û�����
                HidCode = HidCode,
                HidPhoto = Guid.NewGuid().ToString(), //����ͼƬ
                HidChangePhoto = Guid.NewGuid().ToString(), //����ͼƬ
                AcceptPhoto = Guid.NewGuid().ToString(), //����ͼƬ 
                Attachment = Guid.NewGuid().ToString(),
                EstimatePhoto = Guid.NewGuid().ToString(), //����ͼƬ  
                CheckType = itemlist.Where(p => p.EnCode == "SaftyCheckType"),
                HidType = itemlist.Where(p => p.EnCode == "HidType"),
                HidRank = itemlist.Where(p => p.EnCode == "HidRank"),
                TrainTemplateName = itemlist.Where(p => p.EnCode == "TrainTemplateName")
            };

            return Content(josnData.ToJson());
        }
        #endregion

        [HttpGet]
        public ActionResult GetQueryJsonByEnCode(string encode, string itemObj)
        {
            string itemCode = "'" + itemObj + "'";
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode(itemCode).Where(p => p.ItemValue == encode).ToList().FirstOrDefault();
            return Content(itemlist.ToJson());
        }


        #region ��ʼ����ѯ����
        /// <summary>
        /// ��ʼ����ѯ����
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetQueryConditionJson()
        {
            //������������״̬������״̬��������ͣ��������� ,���ݷ�Χ
            string itemCode = "'HidRank','ChangeStatus','WorkStream','SaftyCheckType','HidType','DataScope'";
            //����
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode(itemCode);
            //����ֵ
            var josnData = new
            {
                HidRank = itemlist.Where(p => p.EnCode == "HidRank"),
                ChangeStatus = itemlist.Where(p => p.EnCode == "ChangeStatus"),
                WorkStream = itemlist.Where(p => p.EnCode == "WorkStream"),
                SaftyCheckType = itemlist.Where(p => p.EnCode == "SaftyCheckType"),
                HidType = itemlist.Where(p => p.EnCode == "HidType"),
                DataScope = itemlist.Where(p => p.EnCode == "DataScope")
            };

            return Content(josnData.ToJson());
        }
        #endregion

        #region �ύ����

        #region ɾ������
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(6, "ɾ������������Ϣ")]
        public ActionResult RemoveForm(string keyValue, string hidcode)
        {

            HTBaseInfoEntity entity = htbaseinfobll.GetEntity(keyValue);
            Operator user = OperatorProvider.Provider.Current();

            //ɾ��������Ϣ
            htbaseinfobll.RemoveForm(keyValue);

            //ɾ��������Ϣ
            htapprovalbll.RemoveFormByCode(hidcode);

            //ɾ��������Ϣ
            htchangeinfobll.RemoveFormByCode(hidcode);

            //ɾ��������Ϣ
            htacceptinfobll.RemoveFormByCode(hidcode);

            //ɾ������Ч��������Ϣ
            htestimatebll.RemoveFormByCode(hidcode);

            LogEntity logEntity = new LogEntity();
            logEntity.Browser = this.Request.Browser.Browser;
            logEntity.CategoryId = 6;
            logEntity.OperateTypeId = "6";
            logEntity.OperateType = "ɾ��";
            logEntity.OperateAccount = user.UserName;
            logEntity.OperateUserId = OperatorProvider.Provider.Current().UserId;
            logEntity.ExecuteResult = 1;
            logEntity.Module = SystemInfo.CurrentModuleName;
            logEntity.ModuleId = SystemInfo.CurrentModuleId;
            logEntity.ExecuteResultJson = "������Ϣ:ɾ���������Ϊ" + entity.HIDCODE + ",��������Ϊ" + entity.HIDDESCRIBE + "��������Ϣ, ��������: ��, ������Ϣ:��";
            LogBLL.WriteLog(logEntity);

            return Success("ɾ���ɹ�!");
        }
        #endregion

        #region  ��������������޸ģ�
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, HTBaseInfoEntity entity, HTChangeInfoEntity cEntity, HTAcceptInfoEntity aEntity)
        {
            CommonSaveForm(keyValue, entity, cEntity, aEntity);
            return Success("�����ɹ�!");
        }
        #endregion

        #region ���÷�������������
        /// <summary>
        /// ���÷�������������
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void CommonSaveForm(string keyValue, HTBaseInfoEntity entity, HTChangeInfoEntity cEntity, HTAcceptInfoEntity aEntity)
        {
            //�ύͨ��
            string userId = OperatorProvider.Provider.Current().UserId;
            /********������Ϣ**********/
            if (string.IsNullOrEmpty(keyValue))
            {
                entity.HIDDEPARTCODE = OperatorProvider.Provider.Current().DeptCode;
                entity.HIDDEPARTNAME = OperatorProvider.Provider.Current().DeptName;
            }
            //��������������Ϣ
            htbaseinfobll.SaveForm(keyValue, entity);

            //��ǰ����������Υ�£��򴴽�����ʵ��
            if (entity.ISBREAKRULE == "0")
            {
                //��������ʵ��
                if (string.IsNullOrEmpty(keyValue))
                {
                    string workFlow = "01";//��������
                    bool isSucess = htworkflowbll.CreateWorkFlowObj(workFlow, entity.ID, userId);
                    if (isSucess)
                    {
                        htworkflowbll.UpdateWorkStreamByObjectId(entity.ID);  //����ҵ������״̬
                    }
                }

                /********������Ϣ************/
                string CHANGEID = Request.Form["CHANGEID"].ToString();
                cEntity.HIDCODE = entity.HIDCODE;
                cEntity.BACKREASON = null;
                //����״̬�����
                if (!string.IsNullOrEmpty(CHANGEID))
                {
                    var tempEntity = htchangeinfobll.GetEntity(CHANGEID);
                    cEntity.AUTOID = tempEntity.AUTOID;
                    cEntity.APPLICATIONSTATUS = tempEntity.APPLICATIONSTATUS;
                    cEntity.POSTPONEDAYS = tempEntity.POSTPONEDAYS;
                    cEntity.POSTPONEDEPT = tempEntity.POSTPONEDEPT;
                    cEntity.POSTPONEDEPTNAME = tempEntity.POSTPONEDEPTNAME;
                }
    
                htchangeinfobll.SaveForm(CHANGEID, cEntity);


                /********������Ϣ************/
                string ACCEPTID = Request.Form["ACCEPTID"].ToString();
                aEntity.HIDCODE = entity.HIDCODE;
                if (!string.IsNullOrEmpty(ACCEPTID))
                {
                    var tempEntity = htacceptinfobll.GetEntity(ACCEPTID);
                    aEntity.AUTOID = tempEntity.AUTOID;
                }
                htacceptinfobll.SaveForm(ACCEPTID, aEntity);
            }
        }
        #endregion

        #region �ύ���̣�ͬʱ�������޸�������Ϣ��
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, HTBaseInfoEntity entity, HTChangeInfoEntity cEntity, HTAcceptInfoEntity aEntity)
        {

            var curHtBaseInfor = htbaseinfobll.GetListByCode(entity.HIDCODE).FirstOrDefault();

            if (null != curHtBaseInfor)
            {
                if (curHtBaseInfor.ID != keyValue && string.IsNullOrEmpty(keyValue))
                {
                    return Error("���������ظ�,����������!");
                }
            }

            CommonSaveForm(keyValue, entity, cEntity, aEntity);

            //����������ʵ����
            if (string.IsNullOrEmpty(keyValue))
            {
                keyValue = entity.ID;
            }

            //�˴���Ҫ�жϵ�ǰ���Ƿ�Ϊ��ȫ����Ա
            string wfFlag = string.Empty;

            Operator curUser = OperatorProvider.Provider.Current();

            IList<UserEntity> ulist = new List<UserEntity>();

            string HidApproval = dataitemdetailbll.GetItemValue("HidApproval");

            string[] pstr = HidApproval.Split('#');  //�ָ�������

            foreach (string strArgs in pstr)
            {
                string[] str = strArgs.Split('|');
                //��ǰ������ͬ����Ϊ�����Ű�ȫ����Ա��֤
                if (str[0].ToString() == curUser.OrganizeId && str[1].ToString() == "0")
                {

                    //��˾���û�
                    if (userbll.HaveRoleListByKey(curUser.UserId, dataitemdetailbll.GetItemValue("HidOrganize")).Rows.Count > 0)
                    {
                        string curdeptCode = "'" + curUser.OrganizeCode + "'";
                        ulist = userbll.GetUserListByDeptCode(curdeptCode, dataitemdetailbll.GetItemValue("HidApprovalSetting"), false, curUser.OrganizeId);
                    }
                    else
                    {
                        //�˲�����Ҫ�ж��Ƿ�Ϊ��˾����
                        ulist = userbll.GetUserListByRole(curUser.DeptCode, dataitemdetailbll.GetItemValue("HidApprovalSetting"), curUser.OrganizeId).ToList();
                    }
                    break;
                }
                if (str[0].ToString() == curUser.OrganizeId && str[1].ToString() == "1")
                {
                    //��ȡָ�����ŵ�������Ա
                    ulist = userbll.GetUserListByDeptCode(str[2].ToString(), null, false, curUser.OrganizeId).ToList();

                    break;
                }
            }

            //���صļ�¼��,����0����ʶ��ǰ�û�ӵ�а�ȫ����Ա��ݻ�ָ��������Ա��ݣ���֮����
            int uModel = ulist.Where(p => p.UserId == curUser.UserId).Count();

            //������Ա
            string participant = string.Empty;
            //ָ������
            bool isPointDept = false;

            //��ȫ����Ա�£�ֱ�ӽ�������
            if (uModel > 0)
            {
                wfFlag = "3";//ֱ������

                string keyId = Request.Form["CHANGEPERSON"].ToString();

                if (!string.IsNullOrEmpty(keyId) && keyId != "&nbsp;")
                {
                    participant = userbll.GetEntity(keyId).Account;  //������
                }
                else { return Error("��ȷ���Ƿ�ѡ��������!"); }

            }
            else
            {
                wfFlag = "1";//��������

                IEnumerable<UserEntity> list = new List<UserEntity>();

                foreach (string strArgs in pstr)
                {
                    string[] str = strArgs.Split('|');

                    if (str[0].ToString() == curUser.OrganizeId && str[1].ToString() == "0")
                    {
                        //ȡ��ǰ�û���Ӧ���ŵİ�ȫ����Ա
                        //��˾���û�
                        if (userbll.HaveRoleListByKey(curUser.UserId, dataitemdetailbll.GetItemValue("HidOrganize")).Rows.Count > 0)
                        {
                            string curdeptCode = "'" + curUser.OrganizeCode + "'";
                            list = userbll.GetUserListByDeptCode(curdeptCode, dataitemdetailbll.GetItemValue("HidApprovalSetting"), false, curUser.OrganizeId);
                        }
                        else
                        {
                            //�˲�����Ҫ�ж��Ƿ�Ϊ��˾����
                            list = userbll.GetUserListByRole(curUser.DeptCode, dataitemdetailbll.GetItemValue("HidApprovalSetting"), curUser.OrganizeId).ToList();
                        }
                        break;
                    }
                    //ָ������
                    if (str[0].ToString() == curUser.OrganizeId && str[1].ToString() == "1")
                    {
                        //��ȡָ�����ŵ�������Ա
                        list = userbll.GetUserListByDeptCode(str[2].ToString(), null, false, curUser.OrganizeId);

                        string curDeptCode = "'" + curUser.DeptCode + "'";
                        //��ǰ�ύ������ָ������֮��
                        if (str[2].ToString().Contains(curDeptCode))
                        {
                            isPointDept = true;
                        }
                        break;
                    }
                }

                foreach (UserEntity u in list)
                {
                    participant += u.Account + ",";
                }

                if (!string.IsNullOrEmpty(participant))
                {
                    participant = participant.Substring(0, participant.Length - 1);
                }

                //�����ǰ�ύ����ָ���Ĳ�����Ա,��ֱ���ύ������
                if (isPointDept)
                {
                    wfFlag = "3";//ֱ������

                    string keyId = Request.Form["CHANGEPERSON"].ToString();

                    participant = userbll.GetEntity(keyId).Account;  //������
                }
            }

            if (!string.IsNullOrEmpty(participant))
            {
                int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, curUser.UserId);

                if (count > 0)
                {
                    htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //����ҵ������״̬
                }
            }
            else
            {
                return Error("����ϵϵͳ����Ա����ӱ���λ����ص�λ������Ա!");
            }
            return Success("�����ɹ�!");
        }
        #endregion


        #region һ�����ύ�����������˵ĵǼ�������Ϣ
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult CheckHiddenForm(string saftycheckdatarecordid)
        {
            Operator curUser = OperatorProvider.Provider.Current();

            var dtHid = htbaseinfobll.GetList(saftycheckdatarecordid, curUser.UserId, "", "�����Ǽ�");

            string keyValue = string.Empty;

            string changeperson = string.Empty;

            foreach (DataRow row in dtHid.Rows)
            {
                keyValue = row["id"].ToString();

                changeperson = row["changeperson"].ToString();

                //�˴���Ҫ�жϵ�ǰ���Ƿ�Ϊ��ȫ����Ա
                string wfFlag = string.Empty;


                IList<UserEntity> ulist = new List<UserEntity>();

                string HidApproval = dataitemdetailbll.GetItemValue("HidApproval");

                string[] pstr = HidApproval.Split('#');  //�ָ�������

                foreach (string strArgs in pstr)
                {
                    string[] str = strArgs.Split('|');
                    //��ǰ������ͬ����Ϊ�����Ű�ȫ����Ա��֤
                    if (str[0].ToString() == curUser.OrganizeId && str[1].ToString() == "0")
                    {
                        ulist = userbll.GetUserListByRole(curUser.DeptCode, dataitemdetailbll.GetItemValue("HidApprovalSetting"), curUser.OrganizeId).ToList();

                        break;
                    }
                    if (str[0].ToString() == curUser.OrganizeId && str[1].ToString() == "1")
                    {
                        //��ȡָ�����ŵ�������Ա
                        ulist = userbll.GetUserListByDeptCode(str[2].ToString(), null, false, curUser.OrganizeId).ToList();

                        break;
                    }
                }

                //���صļ�¼��,����0����ʶ��ǰ�û�ӵ�а�ȫ����Ա��ݻ�ָ��������Ա��ݣ���֮����
                int uModel = ulist.Where(p => p.UserId == curUser.UserId).Count();

                //������Ա
                string participant = string.Empty;
                //ָ������
                bool isPointDept = false;

                //��ȫ����Ա�£�ֱ�ӽ�������
                if (uModel > 0)
                {
                    wfFlag = "3";//ֱ������

                    string keyId = changeperson; //  Request.Form["CHANGEPERSON"].ToString();

                    if (!string.IsNullOrEmpty(keyId) && keyId != "&nbsp;")
                    {
                        participant = userbll.GetEntity(keyId).Account;  //������
                    }
                    else { return Error("��ȷ���Ƿ�ѡ��������!"); }

                }
                else
                {
                    wfFlag = "1";//��������

                    IEnumerable<UserEntity> list = new List<UserEntity>();

                    foreach (string strArgs in pstr)
                    {
                        string[] str = strArgs.Split('|');

                        if (str[0].ToString() == curUser.OrganizeId && str[1].ToString() == "0")
                        {
                            //ȡ��ǰ�û���Ӧ���ŵİ�ȫ����Ա
                            list = userbll.GetUserListByRole(curUser.DeptCode, dataitemdetailbll.GetItemValue("HidApprovalSetting"), curUser.OrganizeId);
                            break;
                        }
                        //ָ������
                        if (str[0].ToString() == curUser.OrganizeId && str[1].ToString() == "1")
                        {
                            //��ȡָ�����ŵ�������Ա
                            list = userbll.GetUserListByDeptCode(str[2].ToString(), null, false, curUser.OrganizeId);

                            string curDeptCode = "'" + curUser.DeptCode + "'";
                            //��ǰ�ύ������ָ������֮��
                            if (str[2].ToString().Contains(curDeptCode))
                            {
                                isPointDept = true;
                            }
                            break;
                        }
                    }

                    foreach (UserEntity u in list)
                    {
                        participant += u.Account + ",";
                    }

                    if (!string.IsNullOrEmpty(participant))
                    {
                        participant = participant.Substring(0, participant.Length - 1);
                    }

                    //�����ǰ�ύ����ָ���Ĳ�����Ա,��ֱ���ύ������
                    if (isPointDept)
                    {
                        wfFlag = "3";//ֱ������

                        string keyId = changeperson;

                        participant = userbll.GetEntity(keyId).Account;  //������
                    }
                }

                if (!string.IsNullOrEmpty(participant))
                {
                    int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, curUser.UserId);

                    if (count > 0)
                    {
                        htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //����ҵ������״̬
                    }
                }
                else
                {
                    return Error("����ϵϵͳ����Ա����ӱ���λ����ص�λ������Ա!");
                }
            }
            //����
            //saftycheckdatarecordbll.UpdateCheckMan(curUser.Account);

            return Success("�����ɹ�!");
        }
        #endregion

        #region
        /// <summary>
        /// һ�����ύ������������Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult OneSubmitForm(string keyValue, HTBaseInfoEntity entity, HTApprovalEntity aEntity, HTChangeInfoEntity cEntity, HTAcceptInfoEntity iEntity)
        {
            string APPROVALRESULT = Request.Form["APPROVALRESULT"].ToString();
            string CHANGERESULT = Request.Form["CHANGERESULT"].ToString();
            string ACCEPTSTATUS = Request.Form["ACCEPTSTATUS"].ToString();
            string ApprovalID = Request.Form["APPROVALID"].ToString();
            string ChangeID = Request.Form["CHANGEID"].ToString();
            string AcceptID = Request.Form["ACCEPTID"].ToString();
            //�˴���Ҫ�жϵ�ǰ���Ƿ�Ϊ��ȫ����Ա
            string wfFlag = string.Empty;

            Operator curUser = OperatorProvider.Provider.Current();

            //������Ա
            string participant = string.Empty;

            ////����������Ϣ
            entity.ISBREAKRULE = "0";//δΥ��
            cEntity.CHANGERESULT = CHANGERESULT;
            iEntity.ACCEPTSTATUS = ACCEPTSTATUS;
            CommonSaveForm(keyValue, entity, cEntity, iEntity);

            ////����������Ϣ
            aEntity.APPROVALRESULT = APPROVALRESULT;
            htapprovalbll.SaveForm(ApprovalID, aEntity);

            wfFlag = "2";//���Ľ���

            participant = curUser.Account;

            int count = htworkflowbll.SubmitWorkFlow(entity.ID, participant, wfFlag, curUser.UserId);

            if (count > 0)
            {
                htworkflowbll.UpdateWorkStreamByObjectId(entity.ID);  //����ҵ������״̬
            }

            return Success("�����ɹ�!");
        }
        #endregion


        #region ����������Ϣ
        /// <summary>
        /// ����������Ϣ
        /// </summary>
        /// <param name="Filedata"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadifyHidden(string actionType, HttpPostedFileBase file)
        {
            HSSFWorkbook hssfworkbook;
            if (!string.IsNullOrEmpty(actionType))
            {
                string path = Server.MapPath("~/Resource/HiddenFile/") + file.FileName;
                file.SaveAs(path); //�����ļ�����ǰ������ 
                using (FileStream files = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    hssfworkbook = new HSSFWorkbook(files);
                }
                if (actionType == "No")
                {

                }
                else
                {

                }
            }
            return Content("�����ɹ�!");
        }
        #endregion


        #region ��ȡ�����Ų�ָ���������
        /// <summary>
        /// ��ȡ�����Ų�ָ���������
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult GetHiddenInfoOfWarning(string time = "")
        {
            Operator curUser = OperatorProvider.Provider.Current();
            string startDate = string.Empty;
            string endDate = string.Empty;
            //��Ϊ��
            if (!string.IsNullOrEmpty(time))
            {
                startDate = time;
                endDate = Convert.ToDateTime(time).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                var josnData = htbaseinfobll.GetHiddenInfoOfEveryMonthWarning(curUser, startDate, endDate);
                return Content(josnData.ToJson());
            }
            else 
            {
                startDate = DateTime.Now.Year.ToString() + "-01" + "-01";  //��ÿ���һ��һ�ż�����
                endDate = DateTime.Now.ToString("yyyy-MM-dd");
                var josnData = htbaseinfobll.GetHiddenInfoOfWarning(curUser, startDate, endDate);
                return Content(josnData.ToJson());
            }
        }
        #endregion

        //#region ��ȡ�����Ų�ָ���������
        ///// <summary>
        ///// ��ȡ�����Ų�ָ���������
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //[AjaxOnly]
        //public ActionResult GetHiddenInfoOfEveryMonthWarning(string time)
        //{
        //    Operator curUser = OperatorProvider.Provider.Current();
        //    string endDate = Convert.ToDateTime(time).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
        //    var josnData = htbaseinfobll.GetHiddenInfoOfEveryMonthWarning(curUser, time, endDate);
        //    return Content(josnData.ToJson());
        //}
        //#endregion

        #region ��������������Ϣ
        /// <summary>
        /// ��������������Ϣ
        /// </summary>
        /// <param name="queryJson"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public ActionResult ExportExcel(string queryJson, string fileName)
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;

            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string userId = curUser.UserId;
            queryJson = queryJson.Insert(1, "\"userId\":\"" + userId + "\","); //��ӵ�ǰ�û�
            queryJson = queryJson.Insert(1, "\"isPlanLevel\":\"" + curUser.isPlanLevel + "\","); //��ӵ�ǰ�Ƿ�˾������
            pagination.p_fields = @"workstream,hidcode,hidtypename,hidrankname,hidpointname,checktypename,checkdepartname,checkmanname,
                                    changedutydepartname,changepersonname,changedeadine,hiddescribe,changemeasure";
            //ȡ������Դ
            DataTable exportTable = htbaseinfobll.GetHiddenBaseInfoPageList(pagination, queryJson);
            ////���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "�����Ų������Ϣ";
            excelconfig.FileName = fileName + ".xls";
            ////ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity = listColumnEntity;
            ColumnEntity columnentity = new ColumnEntity();

            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "workstream", ExcelColumn = "����״̬", Width = 20 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "hidcode", ExcelColumn = "��������", Width = 20 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "hidtypename", ExcelColumn = "�������", Width = 20 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "hidrankname", ExcelColumn = "��������", Width = 20 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "hidpointname", ExcelColumn = "��������", Width = 20 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checktypename", ExcelColumn = "�������", Width = 20 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkdepartname", ExcelColumn = "�Ų鵥λ", Width = 25 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkmanname", ExcelColumn = "�Ų���", Width = 20 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "changedutydepartname", ExcelColumn = "���ĵ�λ", Width = 25 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "changepersonname", ExcelColumn = "������", Width = 20 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "changedeadine", ExcelColumn = "���Ľ�ֹʱ��", Width = 20 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "hiddescribe", ExcelColumn = "��������", Width = 30 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "changemeasure", ExcelColumn = "���Ĵ�ʩ", Width = 30 });
            ////���õ�������
            ExcelHelper.ExcelDownload(exportTable, excelconfig);

            return Success("�����ɹ�!");
        }
        #endregion

        #endregion
    }
}
