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
using Aspose.Words;
using ERCHTMS.Busines.AuthorizeManage;
using Aspose.Cells;
using System.Drawing;
using ERCHTMS.Busines.Desktop;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ERCHTMS.Entity.HiddenTroubleManage.ViewModel;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Entity.SaftyCheck;
using ERCHTMS.Entity.LllegalManage;
using ERCHTMS.Busines.LllegalManage;
using ERCHTMS.Busines.QuestionManage;
using ERCHTMS.Entity.QuestionManage;
using System.Text.RegularExpressions;
using BSFramework.Util.Extension;

namespace ERCHTMS.Web.Areas.HiddenTroubleManage.Controllers
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
        private HtReCheckBLL htrecheckbll = new HtReCheckBLL(); //����������֤
        private HTEstimateBLL htestimatebll = new HTEstimateBLL(); //����Ч��������Ϣ
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL(); //��������
        private UserBLL userbll = new UserBLL(); //�û���������
        private SaftyCheckDataRecordBLL saftycheckdatarecordbll = new SaftyCheckDataRecordBLL();

        private DistrictBLL districtbll = new DistrictBLL(); //�������
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private OrganizeCache organizeCache = new OrganizeCache();
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private FileInfoBLL fileinfobll = new FileInfoBLL();

        private WfControlBLL wfcontrolbll = new WfControlBLL();//�Զ������̷���
        private ModuleListColumnAuthBLL modulelistcolumnauthbll = new ModuleListColumnAuthBLL();

        private LllegalRegisterBLL lllegalregisterbll = new LllegalRegisterBLL(); // Υ�»�����Ϣ
        private LllegalPunishBLL lllegalpunishbll = new LllegalPunishBLL(); // ������Ϣ����
        private LllegalAwardDetailBLL lllegalawarddetailbll = new LllegalAwardDetailBLL(); //Υ�½�����Ϣ
        private LllegalReformBLL lllegalreformbll = new LllegalReformBLL(); //������Ϣ����
        private LllegalAcceptBLL lllegalacceptbll = new LllegalAcceptBLL(); //������Ϣ����

        private QuestionInfoBLL questioninfobll = new QuestionInfoBLL();
        private QuestionReformBLL questionreformbll = new QuestionReformBLL();
        private QuestionVerifyBLL questionverifybll = new QuestionVerifyBLL();

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
            Operator curUser = OperatorProvider.Provider.Current();

            string actionName = string.Empty;

            string GDXJ_HYC_ORGCODE = dataitemdetailbll.GetItemValue("GDXJ_HYC_ORGCODE");
            //�����½������ר��
            if (curUser.OrganizeCode == GDXJ_HYC_ORGCODE)
            {
                string[] allKeys = Request.QueryString.AllKeys;
                if (allKeys.Count() > 0)
                {
                    actionName = "HYCForm?";
                    int num = 0;
                    foreach (string str in allKeys)
                    {
                        string strValue = Request.QueryString[str];
                        if (num == 0)
                        {
                            actionName += str + "=" + strValue;
                        }
                        else
                        {
                            actionName += "&" + str + "=" + strValue;
                        }
                        num++;
                    }
                }
                else
                {
                    actionName = "HYCForm";
                }
                return Redirect(actionName);
            }
            else
            {
                return View();
            }
        }
        #endregion


        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HYCForm()
        {
            return View();
        }

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
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CForm()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DoneForm()
        {
            return View();
        }

        //��������ּ�
        [HttpGet]
        public ActionResult WarningDetail()
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
        [HttpGet]
        public ActionResult AppIndex()
        {
            return View();
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExpirationForm()
        {
            return View();
        }


        /// <summary>
        /// ����˵��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FlowForm()
        {
            return View();
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
            opertator.isPlanLevel = "0";
            if (opertator.RoleName.Contains("��˾��") || opertator.RoleName.Contains("����")) { opertator.isPlanLevel = "1"; }
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
        #endregion

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJsonByRelevanceId(Pagination pagination, string queryJson)
        {

            var watch = CommonHelper.TimerStart();
            Operator opertator = new OperatorProvider().Current();
            var data = htbaseinfobll.GetHiddenByRelevanceId(pagination, queryJson);
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

        #region ��ȡʵ��
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

            var recheckInfo = htrecheckbll.GetEntityByHidCode(baseInfo.HIDCODE);

            var estimateInfo = htestimatebll.GetEntityByHidCode(baseInfo.HIDCODE);

            var userInfo = OperatorProvider.Provider.Current();  //��ȡ��ǰ�û�

            int isReformBack = 0;

            var historyacceptList = htacceptinfobll.GetHistoryList(baseInfo.HIDCODE).ToList();
            if (historyacceptList.Count() > 0)
            {
                isReformBack = 1;
            }
            //����
            if (userInfo.RoleName.Contains("��˾��") || userInfo.RoleName.Contains("����"))
            {
                userInfo.DeptName = userInfo.OrganizeName;
                userInfo.DeptCode = userInfo.OrganizeCode;
            }

            var data = new { baseInfo = baseInfo, changeInfo = changeInfo, acceptInfo = acceptInfo, userInfo = userInfo, estimateInfo = estimateInfo, isreformback = isReformBack, recheck = recheckInfo };

            return ToJsonResult(data);
        }
        #endregion

        #region Υ���б�
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

        #region �Ƿ����ü���ģʽ
        /// <summary>
        /// �Ƿ����ü���ģʽ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public int IsEnableMinimalistMode()
        {
            Operator curUser = OperatorProvider.Provider.Current();

            var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'IsEnableMinimalistMode'");

            int result = itemlist.Where(p => p.ItemValue == curUser.OrganizeId).Count();

            return result;
        }
        #endregion

        #region ҳ�������ʼ��
        /// <summary>
        /// ��ʼ������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetInitDataJson(string keyValue="")
        {
            Operator curUser = OperatorProvider.Provider.Current();
            //��������
            string HidCode = DateTime.Now.ToString("yyyyMMddHHmmssfff").ToString();
            //��ȫ������� �������� ��������  ��ѵģ��  �������ε�λ������Ա
            string itemCode = "'SaftyCheckType','HidType','HidRank','TrainTemplateName','HidMajorClassify','GIHiddenClassify','GIHiddenType','ChangeDeptRelevancePerson','AppSettings','HidBmEnableOrganize','IsDisableMajorClassify','IsEnableMajorClassify','AcceptPersonControl','ControlPicMustUpload'";
            //����
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode(itemCode);

            var deptData = departmentBLL.GetList();

            List<DepartmentEntity> dlist = new List<DepartmentEntity>();

            if (curUser.RoleName.Contains("ʡ���û�"))
            {
                var dtDept = departmentBLL.GetAllFactory(curUser);
                foreach (DataRow row in dtDept.Rows)
                {
                    DepartmentEntity entity = new DepartmentEntity();
                    entity.DepartmentId = row["departmentid"].ToString();
                    entity.EnCode = row["encode"].ToString();
                    entity.FullName = row["fullname"].ToString();
                    entity.DeptCode = row["deptcode"].ToString();
                    entity.Manager = row["manager"].ToString();
                    dlist.Add(entity);
                }
            }
            else
            {
                //��ǰ�û�����������
                DepartmentEntity dept = userbll.GetUserOrgInfo(curUser.UserId);
                dlist.Add(dept);
            }

            //��˾���û�
            if (userbll.HaveRoleListByKey(curUser.UserId, dataitemdetailbll.GetItemValue("HidOrganize")).Rows.Count > 0)
            {
                curUser.DeptCode = curUser.OrganizeCode;
                curUser.DeptName = curUser.OrganizeName;
            }

            //רҵ����/��������
            IEnumerable<DataItemModel> MajorClassify = new List<DataItemModel>();
            //�������
            IEnumerable<DataItemModel> HidType = new List<DataItemModel>();

            if (curUser.Industry != "����" && !string.IsNullOrEmpty(curUser.Industry))
            {
                MajorClassify = itemlist.Where(p => p.EnCode == "GIHiddenClassify");
                HidType = itemlist.Where(p => p.EnCode == "GIHiddenType");
            }
            else
            {
                MajorClassify = itemlist.Where(p => p.EnCode == "HidMajorClassify");
                HidType = itemlist.Where(p => p.EnCode == "HidType");
            }

            string KFDC_ORGCODE = dataitemdetailbll.GetItemValue("KFDC_ORGCODE");  //����糧

            string RelevancePersonRole = string.Empty;
            if (itemlist.Where(p => p.EnCode == "ChangeDeptRelevancePerson").Count() > 0)
            {
                var curRelevanceRole = itemlist.Where(p => p.EnCode == "ChangeDeptRelevancePerson").Where(p => p.ItemName == curUser.OrganizeCode);
                if (curRelevanceRole.Count() > 0)
                {
                    RelevancePersonRole = curRelevanceRole.FirstOrDefault().ItemValue;
                }
            }

            //���ͼƬ�ش���������
            string ControlPicMustUpload = string.Empty;
            var cpmu = itemlist.Where(p => p.EnCode == "ControlPicMustUpload").Where(p => p.ItemName == curUser.OrganizeId);
            if (cpmu.Count() > 0)
            {
                ControlPicMustUpload = cpmu.FirstOrDefault().ItemValue;
            }

            var IsDeliver = htworkflowbll.GetCurUserWfAuth("һ������", "��������", "��������", "���������Ų�", "ת��", string.Empty, string.Empty, string.Empty, keyValue) == "1" ? "1" : "";
            var IsAcceptDeliver = htworkflowbll.GetCurUserWfAuth("һ������", "��������", "��������", "���������Ų�", "ת��", string.Empty, string.Empty, string.Empty, keyValue) == "1" ? "1" : "";
            //����ֵ
            var josnData = new
            {
                CreateUser = curUser.UserName,
                User = curUser,  //�û�����
                HidCode = HidCode,
                HidPhoto = Guid.NewGuid().ToString(), //����ͼƬ
                HidChangePhoto = Guid.NewGuid().ToString(), //����ͼƬ
                AcceptPhoto = Guid.NewGuid().ToString(), //����ͼƬ 
                Attachment = Guid.NewGuid().ToString(),
                EstimatePhoto = Guid.NewGuid().ToString(), //����ͼƬ  
                CheckType = itemlist.Where(p => p.EnCode == "SaftyCheckType"),

                HidType = HidType,
                HidRank = itemlist.Where(p => p.EnCode == "HidRank"),
                TrainTemplateName = itemlist.Where(p => p.EnCode == "TrainTemplateName"),
                MajorClassify = MajorClassify,  //����רҵ���� 
                DeptData = dlist,
                KFDC_ORGCODE = KFDC_ORGCODE,
                RelevancePersonRole = RelevancePersonRole,
                IsEnable_RemoteEqu = dataitemdetailbll.GetItemValue("IsGdxy"),
                IsDeliver = IsDeliver,
                IsAcceptDeliver = IsAcceptDeliver,
                IsEnable_HidBm = itemlist.Where(p => p.EnCode == "HidBmEnableOrganize").Where(p => p.ItemValue == curUser.OrganizeCode).Count(), //�Ƿ�������������
                IsDisable_MajorClassify = itemlist.Where(p => p.EnCode == "IsDisableMajorClassify").Where(p => p.ItemValue == curUser.OrganizeCode).Count(), //�Ƿ����רҵ���� 
                IsEnableMajorClassify = itemlist.Where(p => p.EnCode == "IsEnableMajorClassify").Where(p => p.ItemValue == curUser.OrganizeCode).Count(), //�Ƿ�����רҵ���� 
                IsEnableAccept = itemlist.Where(p => p.EnCode == "AcceptPersonControl").Where(p => p.ItemValue == curUser.OrganizeCode).Count(), //�Ƿ����������˿��� 
                ControlPicMustUpload = ControlPicMustUpload
            };

            return Content(josnData.ToJson());
        }
        #endregion

        #region ��ȡĬ�����б�
        /// <summary>
        /// ��ȡĬ�����б�
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDefaultOptionsList()
        {
            Operator curUser = OperatorProvider.Provider.Current();
            DefaultDataSettingBLL defaultdatasettingbll = new DefaultDataSettingBLL();
            List<object> data = new List<object>(); //���صĽ������
            List<DefaultDataSettingEntity> list = defaultdatasettingbll.GetList(curUser.UserId).ToList(); //����
            return Content(list.ToJson());
        }
        #endregion

        #region ��ȡ�����������ṹ
        /// <summary>
        /// ��ȡ�����������ṹ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetHiddenTypeDataJson(string checktypeid = "")
        {
            //����
            var data = dataitemdetailbll.GetDataItemListByItemCode("'HidType'");
            //��ȫ�������
            if (!string.IsNullOrEmpty(checktypeid))
            {
                var checktypeitem = dataitemdetailbll.GetEntity(checktypeid);
                if (null != checktypeitem)
                {
                    data = data.Where(p => p.Description.Trim().Contains(checktypeitem.ItemValue.Trim())).ToList();
                }
                else
                {
                    data = new List<DataItemModel>();
                }
            }

            var treeList = new List<TreeEntity>();
            foreach (DataItemModel item in data)
            {
                TreeEntity tree = new TreeEntity();
                bool hasChildren = data.Where(p => p.ItemCode == item.ItemDetailId).ToList().Count() == 0 ? false : true;
                tree.id = item.ItemDetailId;
                tree.text = item.ItemName;
                tree.value = item.ItemDetailId;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChildren;
                tree.parentId = item.ItemCode;
                treeList.Add(tree);
            }
            if (treeList.Count > 0)
            {
                treeList[0].isexpand = true;
            }
            return Content(treeList.TreeToJson());

        }
        #endregion

        #region ��ȡͨ����ҵ�汾����������
        [HttpGet]
        public ActionResult GetGIHiddenType(string majorclassifyId, string itemcode = "GIHiddenType")
        {
            DataItemDetailEntity detailitem = dataitemdetailbll.GetEntity(majorclassifyId);

            string detailcode = string.Empty;
            if (null != detailitem)
            {
                detailcode = detailitem.ItemCode;
            }
            var data = dataitemdetailbll.GetDataItemByDetailCode(itemcode, detailcode);

            return Content(data.ToJson());
        }
        #endregion

        #region ��ȡ���������׼
        /// <summary>
        /// ��ȡ���������׼
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetHiddenRankStandard()
        {
            Operator curUser = OperatorProvider.Provider.Current();
            DataItemModel itemmode = new DataItemModel();
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'HiddenRankStandardLib'");
            if (itemlist.Count() > 0)
            {
                var curItem = itemlist.Where(p => p.ItemValue == curUser.OrganizeId);
                if (curItem.Count() > 0)
                {
                    itemmode = curItem.FirstOrDefault();
                }
                else
                {
                    itemmode = itemlist.Where(p => p.ItemValue == "HidRankBaseStandard").FirstOrDefault();
                }
            }
            return Content(itemmode.ToJson());
        }
        #endregion

        #region ��ȡ��������е���Ŀ
        /// <summary>
        /// ��ȡ��������е���Ŀ
        /// </summary>
        /// <param name="encode"></param>
        /// <param name="itemObj"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetQueryJsonByEnCode(string encode, string itemObj)
        {
            string itemCode = "'" + itemObj + "'";
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode(itemCode).Where(p => p.ItemValue == encode).ToList().FirstOrDefault();
            return Content(itemlist.ToJson());
        }
        #endregion

        #region ��ʼ����ѯ����
        /// <summary>
        /// ��ʼ����ѯ����
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetQueryConditionJson()
        {
            Operator curUser = OperatorProvider.Provider.Current();
            //������������״̬������״̬��������ͣ��������� ,���ݷ�Χ
            string itemCode = "'HidRank','ChangeStatus','WorkStream','SaftyCheckType','HidType','DataScope','GIHiddenType','HidStandingType','IsEnableMinimalistMode'";
            //����
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode(itemCode);
            var deptData = departmentBLL.GetList();
            List<DepartmentEntity> dlist = new List<DepartmentEntity>();
            if (curUser.RoleName.Contains("ʡ���û�"))
            {
                var dtDept = departmentBLL.GetAllFactory(curUser);
                foreach (DataRow row in dtDept.Rows)
                {
                    DepartmentEntity entity = new DepartmentEntity();
                    entity.DepartmentId = row["departmentid"].ToString();
                    entity.EnCode = row["encode"].ToString();
                    entity.FullName = row["fullname"].ToString();
                    entity.DeptCode = row["deptcode"].ToString();
                    dlist.Add(entity);
                }
            }
            else
            {
                //��ǰ�û�����������
                DepartmentEntity dept = userbll.GetUserOrgInfo(curUser.UserId);
                dlist.Add(dept);
            }
            //�������
            IEnumerable<DataItemModel> HidType = new List<DataItemModel>();
            if (curUser.Industry != "����" && !string.IsNullOrEmpty(curUser.Industry))
            {
                HidType = itemlist.Where(p => p.EnCode == "GIHiddenType");
            }
            else
            {
                HidType = itemlist.Where(p => p.EnCode == "HidType");
            }
            //����ֵ
            var josnData = new
            {
                HidRank = itemlist.Where(p => p.EnCode == "HidRank"),
                ChangeStatus = itemlist.Where(p => p.EnCode == "ChangeStatus"),
                WorkStream = itemlist.Where(p => p.EnCode == "WorkStream"),
                SaftyCheckType = itemlist.Where(p => p.EnCode == "SaftyCheckType"),
                HidType = HidType,
                DataScope = itemlist.Where(p => p.EnCode == "DataScope"),
                DeptData = dlist,
                HidStandingType = itemlist.Where(p => p.EnCode == "HidStandingType"),
                IsEnableMinimalistMode = itemlist.Where(p => p.ItemValue == curUser.OrganizeId).Count()
            };
            return Content(josnData.ToJson());
        }
        #endregion

        #region ��ȡ��ǰ�û��Ƿ�Ϊ��ȫ����Ա���
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

            int uModel = 0;

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
                uModel = ulist.Where(p => p.UserId == curUser.UserId).Count();
            }
            return Content(uModel.ToString());
        }
        #endregion

        #region ��ȡ��ǰ�û�������Ȩ��
        /// <summary>
        /// ��ȡ��ǰ�û�������Ȩ��
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCurUserWfAuth(string rankname, string workflow, string endflow, string mark, string submittype, string arg1 = "", string arg2 = "", string arg3 = "", string arg4 = "", string businessid = "")
        {
            Operator curUser = OperatorProvider.Provider.Current();
            WfControlObj wfentity = new WfControlObj();
            wfentity.businessid = businessid; //
            wfentity.startflow = workflow;
            wfentity.endflow = endflow;
            wfentity.submittype = submittype;
            wfentity.rankname = rankname;
            wfentity.user = curUser;
            wfentity.mark = mark;
            wfentity.isvaliauth = true;
            wfentity.argument1 = arg1;
            wfentity.argument2 = arg2;
            wfentity.argument3 = arg3;
            wfentity.argument4 = arg4;
            //��ȡ��һ���̵Ĳ�����
            WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
            if (result.ishave)
            {
                return Content("1");
            }
            else
            {
                return Content("0");
            }
        }
        #endregion

        #region ͨ����ǰ�����Ƿ���б������(�����½�����ذ汾)
        /// <summary>
        /// ͨ����ǰ�����Ƿ���б������(�����½�����ذ汾)
        /// </summary>
        /// <param name="rankname"></param>
        /// <param name="submittype"></param>
        /// <param name="workflow"></param>
        /// <param name="endflow"></param>
        /// <param name="mark"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCurUserWfAuthOfGDXJ(string rankname, string submittype, string workflow, string endflow, string mark)
        {
            Operator curUser = OperatorProvider.Provider.Current();
            WfControlObj wfentity = new WfControlObj();
            wfentity.businessid = ""; //
            wfentity.startflow = workflow;
            wfentity.endflow = endflow;
            wfentity.submittype = submittype;
            wfentity.rankname = rankname;
            wfentity.user = curUser;
            wfentity.mark = mark;
            wfentity.isvaliauth = true;

            string resultVal = string.Empty;
            //��ȡ��һ���̵Ĳ�����
            WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
            if (result.ishave)
            {
                resultVal = "1";
            }
            else
            {
                resultVal = "0";
            }

            return Content(resultVal);
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
            bool issucess = CommonSaveForm(keyValue, entity, cEntity, aEntity);
            if (issucess)
            {
                return Success("�����ɹ�!");
            }
            else
            {
                return Error("���������ظ�,����������!");
            }
        }
        #endregion

        #region ���÷�������������(����)
        /// <summary>
        /// ���÷�������������
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public bool CommonSaveForm(string keyValue, HTBaseInfoEntity entity, HTChangeInfoEntity cEntity, HTAcceptInfoEntity aEntity)
        {
            bool issucessful = true;
            Operator curuser = OperatorProvider.Provider.Current();
            //�ύͨ��
            string userId = curuser.UserId;
            //��������
            var curHtBaseInfor = htbaseinfobll.GetListByCode(entity.HIDCODE).FirstOrDefault();
            if (null != curHtBaseInfor)
            {
                if (curHtBaseInfor.ID != keyValue && string.IsNullOrEmpty(keyValue))
                {
                    entity.HIDCODE = DateTime.Now.ToString("yyyyMMddHHmmssfff").ToString();
                }
            }

            if (issucessful)
            {
                /********������Ϣ**********/
                entity.ISBREAKRULE = "0"; //��Υ��
                //�豸
                if (string.IsNullOrEmpty(entity.DEVICEID))
                {
                    entity.DEVICEID = string.Empty;
                }
                if (string.IsNullOrEmpty(entity.DEVICENAME))
                {
                    entity.DEVICENAME = string.Empty;
                }
                if (string.IsNullOrEmpty(entity.DEVICECODE))
                {
                    entity.DEVICECODE = string.Empty;
                }
                //��ȫ���
                string safetycheckid = string.Empty;
                string ctype = string.Empty;
                if (null != Request.Form["SAFETYCHECKID"])
                {
                    safetycheckid = Request.Form["SAFETYCHECKID"].ToString();
                }
                if (null != Request.Form["CTYPE"])
                {
                    ctype = Request.Form["CTYPE"].ToString();//��ȫ�������(�Ӱ�ȫ��鴫�ݹ�����)
                }
                if (!string.IsNullOrEmpty(entity.SAFETYCHECKOBJECTID) && string.IsNullOrEmpty(ctype) && string.IsNullOrEmpty(safetycheckid))
                {
                    entity.RELEVANCEID = new SaftyCheckDataRecordBLL().GetRecordFromHT(entity.SAFETYCHECKOBJECTID, curuser);
                }
                //��������������Ϣ
                htbaseinfobll.SaveForm(keyValue, entity);

                //��ǰ����������Υ�£��򴴽�����ʵ��

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
            return issucessful;
        }
        #endregion

        #region ���÷�������������(ʡ��)
        /// <summary>
        /// ���÷�������������
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public bool CommonSaveForm(string keyValue, HTBaseInfoEntity entity, HtReCheckEntity kEntity, HTChangeInfoEntity cEntity, HTAcceptInfoEntity aEntity)
        {
            //�ύͨ��
            string userId = OperatorProvider.Provider.Current().UserId;

            bool issucessful = true;

            var curHtBaseInfor = htbaseinfobll.GetListByCode(entity.HIDCODE).FirstOrDefault();
            if (null != curHtBaseInfor)
            {
                if (curHtBaseInfor.ID != keyValue && string.IsNullOrEmpty(keyValue))
                {
                    issucessful = false;
                }
            }

            if (issucessful == true)
            {
                /********������Ϣ**********/
                entity.ISBREAKRULE = "0"; //��Υ��
                //�豸
                if (string.IsNullOrEmpty(entity.DEVICEID))
                {
                    entity.DEVICEID = string.Empty;
                }
                if (string.IsNullOrEmpty(entity.DEVICENAME))
                {
                    entity.DEVICENAME = string.Empty;
                }
                if (string.IsNullOrEmpty(entity.DEVICECODE))
                {
                    entity.DEVICECODE = string.Empty;
                }
                //��������������Ϣ
                htbaseinfobll.SaveForm(keyValue, entity);

                //��ǰ����������Υ�£��򴴽�����ʵ��

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

            return issucessful;

        }
        #endregion

        #region �ύ���̣�ͬʱ�������޸�������Ϣ��(�糧��)
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, string isSubmit, HTBaseInfoEntity entity, HTChangeInfoEntity cEntity, HTAcceptInfoEntity aEntity)
        {
            cEntity.BACKREASON = "";  //����ԭ��ֵΪ��
            CommonSaveForm(keyValue, entity, cEntity, aEntity);

            //����������ʵ����
            if (string.IsNullOrEmpty(keyValue))
            {
                keyValue = entity.ID;
            }

            //�˴���Ҫ�жϵ�ǰ���Ƿ�Ϊ��ȫ����Ա
            string wfFlag = string.Empty;
            string participant = string.Empty;
            Operator curUser = OperatorProvider.Provider.Current();

            WfControlObj wfentity = new WfControlObj();
            wfentity.businessid = keyValue; //
            wfentity.argument1 = entity.MAJORCLASSIFY; //רҵ����
            wfentity.argument2 = curUser.DeptId; //��ǰ����
            wfentity.argument3 = entity.HIDTYPE; //�������
            wfentity.argument4 = entity.HIDBMID; //��������
            wfentity.startflow = "�����Ǽ�";
            //�Ƿ��ϱ�
            if (isSubmit == "1")
            {
                wfentity.submittype = "�ϱ�";
            }
            else
            {
                wfentity.submittype = "�ύ";
                //��ָ������������
                if (cEntity.ISAPPOINT == "0")
                {
                    wfentity.submittype = "�ƶ��ύ";
                }
            }

            #region �����½��汾
            if (entity.ADDTYPE == "3")
            {    //�Ǳ������ύ
                if (entity.ISSELFCHANGE == "0")
                {
                    wfentity.submittype = "�ƶ��ύ";
                }
            }
            #endregion
            wfentity.rankid = entity.HIDRANK;
            wfentity.user = curUser;
            wfentity.mark = "���������Ų�";
            wfentity.organizeid = entity.HIDDEPART; //��Ӧ�糧id
            //��ȡ��һ���̵Ĳ�����
            WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
            //����ɹ�
            if (result.code == WfCode.Sucess)
            {
                participant = result.actionperson;
                wfFlag = result.wfflag;
                if (!string.IsNullOrEmpty(participant))
                {
                    int count = htworkflowbll.SubmitWorkFlow(wfentity, result, keyValue, participant, wfFlag, curUser.UserId);

                    if (count > 0)
                    {
                        htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //����ҵ������״̬
                    }
                }
                else
                {
                    return Error("����ϵϵͳ����Ա����ӱ���λ����ص�λ������Ա!");
                }
                return Success(result.message);
            }
            else
            {
                return Error(result.message);
            }
        }
        #endregion

        #region  ��������������޸ģ�(ʡ��)
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveCForm(string keyValue, HTBaseInfoEntity entity, HtReCheckEntity kEntity, HTChangeInfoEntity cEntity, HTAcceptInfoEntity aEntity)
        {
            bool issucess = CommonSaveForm(keyValue, entity, kEntity, cEntity, aEntity);
            if (issucess)
            {
                return Success("�����ɹ�!");
            }
            else
            {
                return Error("���������ظ�,����������!");
            }
        }
        #endregion

        #region ʡ����˾�ύ���̣�ͬʱ�������޸�������Ϣ ʡ����
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitCForm(string keyValue, HTBaseInfoEntity entity, HtReCheckEntity kEntity, HTChangeInfoEntity cEntity, HTAcceptInfoEntity aEntity)
        {

            string participant = string.Empty;

            cEntity.BACKREASON = "";  //����ԭ��ֵΪ��
            CommonSaveForm(keyValue, entity, kEntity, cEntity, aEntity);

            //����������ʵ����
            if (string.IsNullOrEmpty(keyValue))
            {
                keyValue = entity.ID;
            }

            //�˴���Ҫ�жϵ�ǰ���Ƿ�Ϊ��ȫ����Ա
            string wfFlag = string.Empty;

            Operator curUser = OperatorProvider.Provider.Current();

            //���õ�����������Ա
            WfControlObj wfentity = new WfControlObj();
            wfentity.businessid = keyValue; //
            wfentity.startflow = "�����Ǽ�";
            wfentity.submittype = "�ύ";
            wfentity.rankid = entity.HIDRANK; //����
            wfentity.user = curUser;
            wfentity.mark = "ʡ�������Ų�";
            wfentity.organizeid = entity.HIDDEPART; //��Ӧ�糧id
            //��ȡ��һ���̵Ĳ�����
            WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
            //����ɹ�
            if (result.code == WfCode.Sucess)
            {
                participant = result.actionperson;
                wfFlag = result.wfflag;
                if (!string.IsNullOrEmpty(participant))
                {
                    int count = htworkflowbll.SubmitWorkFlow(wfentity, result, keyValue, participant, wfFlag, curUser.UserId);

                    if (count > 0)
                    {
                        htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //����ҵ������״̬
                    }
                }
                else
                {
                    return Error("����ϵϵͳ����Ա�����������λ������������Ա!");
                }
                return Success(result.message);
            }
            else
            {
                return Error(result.message);
            }
        }
        #endregion

        #region �ύ�����������̣�ͬʱ�޸�������Ϣ��
        /// <summary>
        /// ʡ����˾�ύ�����������̣�ͬʱ�޸�������Ϣ��
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitPerfectionForm(string keyValue, HTBaseInfoEntity entity, HTChangeInfoEntity cEntity, HTAcceptInfoEntity aEntity)
        {
            string participant = string.Empty;

            var curHtBaseInfor = htbaseinfobll.GetListByCode(entity.HIDCODE).FirstOrDefault();

            if (null != curHtBaseInfor)
            {
                if (curHtBaseInfor.ID != keyValue && string.IsNullOrEmpty(keyValue))
                {
                    return Error("���������ظ�,����������!");
                }
            }
            cEntity.BACKREASON = "";  //����ԭ��ֵΪ��
            CommonSaveForm(keyValue, entity, cEntity, aEntity);

            //����������ʵ����
            if (string.IsNullOrEmpty(keyValue))
            {
                keyValue = entity.ID;
            }

            //�˴���Ҫ�жϵ�ǰ���Ƿ�Ϊ��ȫ����Ա
            string wfFlag = string.Empty;

            Operator curUser = OperatorProvider.Provider.Current();

            WfControlObj wfentity = new WfControlObj();
            wfentity.businessid = keyValue; //
            wfentity.startflow = "��������";
            wfentity.submittype = "�ύ";
            wfentity.rankid = entity.HIDRANK;
            wfentity.user = curUser;
            wfentity.mark = "ʡ�������Ų�";
            wfentity.organizeid = entity.HIDDEPART; //��Ӧ�糧id
            //��ȡ��һ���̵Ĳ�����
            WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
            //����ɹ�
            if (result.code == WfCode.Sucess)
            {
                participant = result.actionperson;
                wfFlag = result.wfflag;
                if (!string.IsNullOrEmpty(participant))
                {
                    int count = htworkflowbll.SubmitWorkFlow(wfentity, result, keyValue, participant, wfFlag, curUser.UserId);

                    if (count > 0)
                    {
                        htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //����ҵ������״̬
                    }
                }
                else
                {
                    return Error("����ϵϵͳ����Ա����ӵ�ǰ�����µ�������!");
                }
                return Success(result.message);
            }
            else
            {
                return Error(result.message);
            }
        }
        #endregion

        #region �ύ�ƶ����ļƻ����̣�ͬʱ�޸�������Ϣ��
        /// <summary>
        /// �ύ�ƶ����ļƻ����̣�ͬʱ�޸�������Ϣ��
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitChangePlanForm(string keyValue, HTBaseInfoEntity entity, HTChangeInfoEntity cEntity, HTAcceptInfoEntity aEntity)
        {
            string participant = string.Empty;

            var curHtBaseInfor = htbaseinfobll.GetListByCode(entity.HIDCODE).FirstOrDefault();

            if (null != curHtBaseInfor)
            {
                if (curHtBaseInfor.ID != keyValue && string.IsNullOrEmpty(keyValue))
                {
                    return Error("���������ظ�,����������!");
                }
            }
            cEntity.BACKREASON = "";  //����ԭ��ֵΪ��
            entity.ISFORMULATE = "1"; //����Ѿ��������ƶ����ļƻ�����
            CommonSaveForm(keyValue, entity, cEntity, aEntity);

            //����������ʵ����
            if (string.IsNullOrEmpty(keyValue))
            {
                keyValue = entity.ID;
            }


            //bool isspecial = false;
            #region ��鵱ǰ�Ƿ�Ϊ������(����ֱ���ύ���������ĵ�Ȩ�ޣ�Ҳ������������ɫ����),

            Operator curUser = OperatorProvider.Provider.Current();
            //WfControlObj wfValentity = new WfControlObj();
            //wfValentity.businessid = ""; //
            //wfValentity.startflow = "�ƶ����ļƻ�";
            //wfValentity.endflow = "��������";
            //wfValentity.submittype = "�ύ";
            //wfValentity.rankid = entity.HIDRANK;
            //wfValentity.user = curUser;
            //wfValentity.mark = "���������Ų�";
            //wfValentity.isvaliauth = true;

            //string resultVal = string.Empty;
            ////��ȡ��һ���̵Ĳ�����
            //WfControlResult valresult = wfcontrolbll.GetWfControl(wfValentity);
            //isspecial = valresult.ishave; //��֤���
            #endregion


            //�˴���Ҫ�жϵ�ǰ���Ƿ�Ϊ��ȫ����Ա
            string wfFlag = string.Empty;

            WfControlObj wfentity = new WfControlObj();
            wfentity.businessid = keyValue; //
            wfentity.startflow = "�ƶ����ļƻ�";
            //������������Ȩ�ޣ������Ĳ��ž�������������ֱ���ύ������
            //if (isspecial && cEntity.CHANGEDUTYDEPARTID == curUser.DeptId)
            //{
            wfentity.submittype = "�ύ";
            //}
            //else
            //{
            //    wfentity.submittype = "�ƶ��ύ";
            //}
            wfentity.rankid = entity.HIDRANK;
            wfentity.user = curUser;
            wfentity.mark = "���������Ų�";
            wfentity.organizeid = entity.HIDDEPART; //��Ӧ�糧id
            //��ȡ��һ���̵Ĳ�����
            WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
            //����ɹ�
            if (result.code == WfCode.Sucess)
            {
                participant = result.actionperson;
                wfFlag = result.wfflag;
                if (!string.IsNullOrEmpty(participant))
                {
                    int count = htworkflowbll.SubmitWorkFlow(wfentity, result, keyValue, participant, wfFlag, curUser.UserId);

                    if (count > 0)
                    {
                        htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //����ҵ������״̬
                    }
                }
                else
                {
                    return Error("����ϵϵͳ����Ա����ӵ�ǰ�����µĲ�����!");
                }
                return Success(result.message);
            }
            else
            {
                return Error(result.message);
            }
        }
        #endregion

        #region ת���ƶ����ļƻ�/�������ġ�������������
        /// <summary>
        /// ת���ƶ����ļƻ�/�������ġ�������������
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult DeliverPlanForm(string keyValue, HTChangeInfoEntity centity, HTAcceptInfoEntity aentity)
        {
            string participant = string.Empty;

            HTBaseInfoEntity entity = htbaseinfobll.GetEntity(keyValue);
            /********������Ϣ************/
            if (!Request.Form["CHANGEID"].IsEmpty())
            {  
                string CHANGEID = Request.Form["CHANGEID"].ToString();
                //����������Ϣ
                if (!string.IsNullOrEmpty(CHANGEID))
                {
                    var changeEntity = htchangeinfobll.GetEntity(CHANGEID);
                    changeEntity.CHARGEDEPTID = centity.CHARGEDEPTID;
                    changeEntity.CHARGEDEPTNAME = centity.CHARGEDEPTNAME;
                    changeEntity.CHARGEPERSON = centity.CHARGEPERSON;
                    changeEntity.CHARGEPERSONNAME = centity.CHARGEPERSONNAME;
                    changeEntity.CHANGEPERSONNAME = centity.CHANGEPERSONNAME;
                    changeEntity.CHANGEPERSON = centity.CHANGEPERSON;
                    changeEntity.CHANGEDUTYDEPARTNAME = centity.CHANGEDUTYDEPARTNAME;
                    changeEntity.CHANGEDUTYDEPARTID = centity.CHANGEDUTYDEPARTID;
                    changeEntity.CHANGEDUTYDEPARTCODE = centity.CHANGEDUTYDEPARTCODE;
                    changeEntity.CHANGEDUTYTEL = centity.CHANGEDUTYTEL;
                    htchangeinfobll.SaveForm(CHANGEID, changeEntity);
                }
            }
            /********������Ϣ************/
            if (!Request.Form["ACCEPTID"].IsEmpty())
            {
                string ACCEPTID = Request.Form["ACCEPTID"].ToString();
                //����������Ϣ
                if (!string.IsNullOrEmpty(ACCEPTID))
                {
                    var acceptEntity = htacceptinfobll.GetEntity(ACCEPTID);
                    acceptEntity.ACCEPTDEPARTCODE = aentity.ACCEPTDEPARTCODE;
                    acceptEntity.ACCEPTDEPARTNAME = aentity.ACCEPTDEPARTNAME;
                    acceptEntity.ACCEPTPERSON = aentity.ACCEPTPERSON;
                    acceptEntity.ACCEPTPERSONNAME = aentity.ACCEPTPERSONNAME;
                    htacceptinfobll.SaveForm(ACCEPTID, acceptEntity);
                }
            }

            Operator curUser = OperatorProvider.Provider.Current();
            //�˴���Ҫ�жϵ�ǰ���Ƿ�Ϊ��ȫ����Ա
            string wfFlag = string.Empty;
            WfControlObj wfentity = new WfControlObj();
            wfentity.businessid = keyValue; //ҵ��id
            wfentity.startflow = entity.WORKSTREAM;
            wfentity.endflow = entity.WORKSTREAM;
            wfentity.submittype = "ת��";
            wfentity.rankid = entity.HIDRANK;
            wfentity.user = curUser;
            wfentity.mark = "���������Ų�";
            wfentity.organizeid = entity.HIDDEPART; //��Ӧ�糧id
            //��ȡ��һ���̵Ĳ�����
            WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
            //����ɹ�
            if (result.code == WfCode.Sucess)
            {
                participant = result.actionperson;
                wfFlag = result.wfflag;
                if (!string.IsNullOrEmpty(participant))
                {
                    //������״̬
                    if (!result.ischangestatus)
                    {
                        htworkflowbll.SubmitWorkFlowNoChangeStatus(wfentity, result, keyValue, participant, curUser.UserId);
                        return Success(result.message);
                    }
                }
                else
                {
                    return Error("����ϵϵͳ����Ա����ӵ�ǰ�����µĲ�����!");
                }
                return Success(result.message);
            }
            else
            {
                return Error(result.message);
            }
        }
        #endregion

        #region һ�����ύ�����������˵ĵǼ�������Ϣ(�糧��)
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult CheckHiddenForm(string saftycheckdatarecordid)
        {
            Operator curUser = OperatorProvider.Provider.Current();

            var dtHid = htbaseinfobll.GetList(saftycheckdatarecordid, curUser.UserId, "", "�����Ǽ�");

            string keyValue = string.Empty;

            string changeperson = string.Empty;

            string hiddepart = string.Empty;

            string rankid = string.Empty;

            foreach (DataRow row in dtHid.Rows)
            {
                keyValue = row["id"].ToString();

                hiddepart = row["hiddepart"].ToString();

                rankid = row["hidrank"].ToString(); //��������

                string upsubmit = row["upsubmit"].ToString(); //��������

                string isselfchange = row["isselfchange"].ToString(); //�Ƿ񱾲�������

                string isappoint = row["isappoint"].ToString(); //�Ƿ��ƶ�

                string addtype = row["addtype"].ToString(); //�����ж��Ƿ�ʡ����˾�ύ������

                string hidbmid = row["hidbmid"].ToString(); //��������

                string majorclassify = row["majorclassify"].ToString(); //רҵ����

                string hidtype = row["hidtype"].ToString(); //�������

                string createuserid = row["createuserid"].ToString(); //������

                //�˴���Ҫ�жϵ�ǰ���Ƿ�Ϊ��ȫ����Ա
                string wfFlag = string.Empty;
                //������Ա
                string participant = string.Empty;
                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = keyValue; //
                wfentity.argument1 = majorclassify; //רҵ����
                wfentity.argument2 = curUser.DeptId; //��ǰ����
                wfentity.argument3 = hidtype; //�������
                wfentity.argument4 = hidbmid; //��������
                wfentity.startflow = "�����Ǽ�";
                //�Ƿ��ϱ�
                if (upsubmit == "1")
                {
                    wfentity.submittype = "�ϱ�";
                }
                else
                {
                    wfentity.submittype = "�ύ";

                    //��ָ������������
                    if (!string.IsNullOrEmpty(isappoint) && isappoint == "0")
                    {
                        wfentity.submittype = "�ƶ��ύ";
                    }
                }

                wfentity.rankid = rankid;
                wfentity.spuser = userbll.GetUserInfoEntity(createuserid);
                wfentity.organizeid = hiddepart; //��Ӧ�糧id
                //ʡ���Ǽǵ�
                if (addtype == "2")
                {
                    wfentity.mark = "ʡ�������Ų�";
                }
                else
                {
                    wfentity.mark = "���������Ų�";
                }

                #region �����½��汾
                if (addtype == "3")
                {    //�Ǳ������ύ
                    if (isselfchange == "0")
                    {
                        wfentity.submittype = "�ƶ��ύ";
                    }
                }
                #endregion
                //��ȡ��һ���̵Ĳ�����
                WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
                //����ɹ�
                if (result.code == WfCode.Sucess)
                {
                    participant = result.actionperson;
                    wfFlag = result.wfflag;
                    if (!string.IsNullOrEmpty(participant))
                    {
                        int count = htworkflowbll.SubmitWorkFlow(wfentity, result, keyValue, participant, wfFlag, createuserid);

                        if (count > 0)
                        {
                            htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //����ҵ������״̬
                        }
                    }
                }
            }
            return Success("�����ɹ�!");
        }
        #endregion

        #region �������ġ��ύ������������Ϣ(�糧��)
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
        /// <param name="checkid">��ȫ���id</param>
        /// <param name="repeatdata">�ظ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportHiddenInfo(string checkid, string repeatdata, string mode)
        {

            if (OperatorProvider.Provider.Current().IsSystem)
            {
                return "��������Ա�޴˲���Ȩ��";
            }
            var curUser = OperatorProvider.Provider.Current();
            string orgId = curUser.OrganizeId;//������˾

            string message = "��ѡ���ʽ��ȷ���ļ��ٵ���!";
            string falseMessage = "";
            string childmessage = "";
            int count = HttpContext.Request.Files.Count;
            try
            {
                List<string> listIds = new List<string>();
                if (count > 0)
                {
                    if (HttpContext.Request.Files.Count > 2)
                    {
                        return "�밴��ȷ�ķ�ʽ�����ļ�,һ���ϴ����֧�������ļ�(��һ��excel�����ļ�,һ��ͼƬѹ���ļ�).";
                    }
                    HttpPostedFileBase file = HttpContext.Request.Files[0];
                    string hiddenDirectory = string.Empty;
                    Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();

                    #region �����ļ�ʱ
                    if (HttpContext.Request.Files.Count == 2)
                    {
                        HttpPostedFileBase file2 = HttpContext.Request.Files[1];
                        if (string.IsNullOrEmpty(file.FileName) || string.IsNullOrEmpty(file2.FileName))
                        {
                            return message;
                        }
                        Boolean isZip1 = file.FileName.ToLower().Substring(file.FileName.ToLower().IndexOf('.')).Contains("zip");//��һ���ļ��Ƿ�ΪZip��ʽ
                        Boolean isZip2 = file2.FileName.ToLower().Substring(file2.FileName.ToLower().IndexOf('.')).Contains("zip");//�ڶ����ļ��Ƿ�ΪZip��ʽ
                        if ((isZip1 || isZip2) == false || (isZip1 && isZip2) == true)
                        {
                            return message;
                        }
                        string fileName1 = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                        file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName1));
                        string fileName2 = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file2.FileName);
                        file2.SaveAs(Server.MapPath("~/Resource/temp/" + fileName2));
                        hiddenDirectory = Server.MapPath("~/Resource/temp/") + DateTime.Now.ToString("yyyyMMddhhmmssfff") + "\\"; //����/Υ��ͼƬ��ŵ�ַ                                                                                                    //��ǰ�ļ��д���
                        if (!Directory.Exists(hiddenDirectory))
                        {
                            System.IO.Directory.CreateDirectory(hiddenDirectory); //����Ŀ¼
                        }
                        if (isZip1)
                        {
                            fileinfobll.UnZip(Server.MapPath("~/Resource/temp/" + fileName1), hiddenDirectory, "", true);
                            wb.Open(Server.MapPath("~/Resource/temp/" + fileName2));
                            if (string.IsNullOrEmpty(file2.FileName))
                            {
                                return message;
                            }
                            if (!(file2.FileName.ToLower().Substring(file2.FileName.ToLower().IndexOf('.')).Contains("xls") || file2.FileName.ToLower().Substring(file2.FileName.ToLower().IndexOf('.')).Contains("xlsx")))
                            {
                                return message;
                            }
                        }
                        else
                        {
                            fileinfobll.UnZip(Server.MapPath("~/Resource/temp/" + fileName2), hiddenDirectory, "", true);
                            wb.Open(Server.MapPath("~/Resource/temp/" + fileName1));
                            if (string.IsNullOrEmpty(file.FileName))
                            {
                                return message;
                            }
                            if (!(file.FileName.ToLower().Substring(file.FileName.ToLower().IndexOf('.')).Contains("xls") || file.FileName.ToLower().Substring(file.FileName.ToLower().IndexOf('.')).Contains("xlsx")))
                            {
                                return message;
                            }
                        }
                    }
                    #endregion
                    #region һ���ļ�ʱ
                    else  //һ���ļ�ʱ
                    {
                        if (string.IsNullOrEmpty(file.FileName))
                        {
                            return message;
                        }
                        if (!(file.FileName.ToLower().Substring(file.FileName.ToLower().IndexOf('.')).Contains("xls") || file.FileName.ToLower().Substring(file.FileName.ToLower().IndexOf('.')).Contains("xlsx")))
                        {
                            return message;
                        }
                        string fileName1 = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                        file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName1));
                        wb.Open(Server.MapPath("~/Resource/temp/" + fileName1));
                    }
                    #endregion
                    Worksheet sheets = wb.Worksheets[0];
                    Aspose.Cells.Cells cells = sheets.Cells;
                    DataTable dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn + 1, true);
                    #region �ڶ��ű���(Υ�¿��ˡ�����)
                    Worksheet secondsheets;
                    Aspose.Cells.Cells secondcells;
                    DataTable seconddt = new DataTable();
                    if (wb.Worksheets.Count == 3)
                    {
                        secondsheets = wb.Worksheets[1];
                        secondcells = secondsheets.Cells;
                        seconddt = secondcells.ExportDataTable(2, 0, secondcells.MaxDataRow, secondcells.MaxColumn + 1, true);
                    } 
                    #endregion
                    //��¼������Ϣ
                    List<string> resultlist = new List<string>();
                    List<UserEntity> ulist = userbll.GetList().OrderBy(p => p.SortCode).ToList();
                    List<DepartmentEntity> dlist = departmentBLL.GetList().OrderBy(p => p.SortCode).ToList();
                    int total = 0;
                    int checkproject = 0;
                    SaftyCheckDataDetailBLL sdbll = new SaftyCheckDataDetailBLL();
                    SaftyCheckContentBLL scbll = new SaftyCheckContentBLL();
                    SaftyCheckDataRecordEntity safetyEntity = null;
                    //����¼
                    if (!string.IsNullOrEmpty(checkid))
                    {
                        safetyEntity = saftycheckdatarecordbll.GetEntity(checkid);

                        checkproject = sdbll.GetCheckItemCount(checkid);
                    }
                    //������ͼ���
                    var checktypelist = dataitemdetailbll.GetDataItemListByItemCode("'SaftyCheckType'");
                    //��������
                    #region ��������
                    if (sheets.Name.Contains("����") || dt.Columns.Contains("��������"))
                    {
                        #region ����װ��
                        List<ImportHidden> list = new List<ImportHidden>();
                        //�Ȼ�ȡ��ְ���б�;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string resultmessage = "��" + (i + 1).ToString() + "��"; //��ʾ����

                            bool isadddobj = true;
                            //��������
                            string hidcode = dt.Columns.Contains("��������") ? dt.Rows[i]["��������"].ToString().Trim() : string.Empty;
                            //������
                            string checkobj = dt.Columns.Contains("������") ? dt.Rows[i]["������"].ToString().Trim() : string.Empty;
                            //�������
                            string checkcontent = dt.Columns.Contains("�������") ? dt.Rows[i]["�������"].ToString().Trim() : string.Empty;
                            //�������� 
                            string hiddenname = dt.Columns.Contains("��������") ? dt.Rows[i]["��������"].ToString().Trim() : string.Empty;
                            //��������
                            string rankname = dt.Columns.Contains("��������") ? dt.Rows[i]["��������"].ToString().Trim() : string.Empty;
                            //��������
                            string areaname = dt.Columns.Contains("��������") ? dt.Rows[i]["��������"].ToString().Trim() : string.Empty;
                            //רҵ����
                            string majorclassify = dt.Columns.Contains("רҵ����") ? dt.Rows[i]["רҵ����"].ToString().Trim() : string.Empty;
                            //�������
                            string hidtype = dt.Columns.Contains("�������") ? dt.Rows[i]["�������"].ToString().Trim() : string.Empty;
                            //�豸����
                            string devicename = dt.Columns.Contains("�豸����") ? dt.Rows[i]["�豸����"].ToString().Trim() : string.Empty;
                            //��������
                            string hiddescribe = dt.Columns.Contains("�¹���������(����)") ? dt.Rows[i]["�¹���������(����)"].ToString().Trim() : string.Empty;
                            //���罭��
                            if (mode == "6")
                            {
                                hiddescribe = dt.Columns.Contains("��������") ? dt.Rows[i]["��������"].ToString().Trim() : string.Empty;
                            }
                            //����������
                            string changeperson = dt.Columns.Contains("����������") ? dt.Rows[i]["����������"].ToString().Trim() : string.Empty;
                            //���������˵绰
                            string telephone = dt.Columns.Contains("���������˵绰") ? dt.Rows[i]["���������˵绰"].ToString().Trim() : string.Empty;
                            //�������ε�λ
                            string changedept = dt.Columns.Contains("�������ε�λ") ? dt.Rows[i]["�������ε�λ"].ToString().Trim() : string.Empty;
                            //���Ľ�ֹʱ��
                            string changedeadline = dt.Columns.Contains("���Ľ�ֹʱ��") ? dt.Rows[i]["���Ľ�ֹʱ��"].ToString().Trim() : string.Empty;
                            //���Ĵ�ʩ
                            string changemeasure = dt.Columns.Contains("���Ĵ�ʩ") ? dt.Rows[i]["���Ĵ�ʩ"].ToString().Trim() : string.Empty;
                            //������
                            string acceptperson = dt.Columns.Contains("������") ? dt.Rows[i]["������"].ToString().Trim() : string.Empty;
                            //���յ�λ
                            string acceptdept = dt.Columns.Contains("���յ�λ") ? dt.Rows[i]["���յ�λ"].ToString().Trim() : string.Empty;
                            //��������
                            string acceptdate = dt.Columns.Contains("��������") ? dt.Rows[i]["��������"].ToString().Trim() : string.Empty;

                            string relevanceid = string.Empty;

                            string muchmark = string.Empty;
                            try
                            {
                                #region ���󼯺�
                                ImportHidden entity = new ImportHidden();
                                //����
                                entity.hidcode = hidcode; //����
                                entity.checkcontent = checkcontent; //�������
                                entity.checkobj = checkobj; //������
                                //��������
                                entity.hiddenname = hiddenname;
                                //��������
                                if (!string.IsNullOrEmpty(rankname))
                                {
                                    var rankEntity = dataitemdetailbll.GetEntityByItemName(rankname);
                                    if (null != rankEntity)
                                    {
                                        entity.rankid = rankEntity.ItemDetailId; //��������id
                                    }
                                }
                                //��������
                                if (!string.IsNullOrEmpty(areaname))
                                {
                                    string[] strarray = areaname.Split(new string[] { "=>" }, StringSplitOptions.None);
                                    string areaspname = strarray[0];
                                    var districtList = districtbll.GetOrgList(curUser.OrganizeId);
                                    if (!string.IsNullOrEmpty(areaspname))
                                    {
                                        districtList = districtList.Where(p => p.DistrictName == areaspname).ToList();
                                    }
                                    if (strarray.Length > 1)
                                    {
                                        string areaspcode = strarray[1];
                                        if (!string.IsNullOrEmpty(areaspcode))
                                        {
                                            districtList = districtList.Where(p => p.DistrictCode == areaspcode).ToList();
                                        }
                                    }
                                    if (districtList.Count() > 0)
                                    {
                                        var districtEntity = districtList.FirstOrDefault();
                                        entity.areacode = districtEntity.DistrictCode; //�������
                                        entity.areaname = districtEntity.DistrictName; //��������
                                    }
                                }

                                //רҵ����
                                if (!string.IsNullOrEmpty(majorclassify))
                                {
                                    var majorlist = dataitemdetailbll.GetDataItemListByItemCode("'HidMajorClassify'").Where(p => p.ItemName == majorclassify);
                                    if (majorlist.Count() > 0)
                                    {
                                        entity.majorclassify = majorlist.FirstOrDefault().ItemDetailId; //רҵ����id
                                    }
                                }
                                //�������
                                if (!string.IsNullOrEmpty(hidtype))
                                {
                                    var hidtypeEntity = dataitemdetailbll.GetEntityByItemName(hidtype);
                                    if (null != hidtypeEntity)
                                    {
                                        entity.hidtype = hidtypeEntity.ItemDetailId; //�������id
                                    }
                                }
                                entity.devicename = devicename; //�豸����
                                //��������
                                if (!string.IsNullOrEmpty(hiddescribe))
                                {
                                    entity.hiddescribe = hiddescribe; //��������
                                }

                                //�����������
                                if (mode == "5")
                                {
                                    #region  ����������
                                    if (!string.IsNullOrEmpty(changeperson))
                                    {
                                        var userlist = ulist.Where(p => p.RealName == changeperson).ToList();
                                        if (!string.IsNullOrEmpty(telephone))
                                        {
                                            userlist = userlist.Where(p => p.Telephone == telephone || p.Mobile == telephone).ToList();
                                        }
                                        if (userlist.Count() > 1)
                                        {
                                            muchmark = "1";  //�����˴�������
                                        }
                                        else
                                        {
                                            foreach (UserEntity uentity in userlist)
                                            {
                                                entity.changepersonid = uentity.UserId;
                                                entity.changepersonname = uentity.RealName;
                                                entity.changetelephone = uentity.Telephone;
                                            }
                                        }
                                    }
                                    #endregion

                                    #region �������ε�λ
                                    if (!string.IsNullOrEmpty(changedept))
                                    {
                                        var deptlist = dlist.Where(e => e.FullName == changedept).ToList();
                                        UserEntity uentity = new UserEntity();
                                        if (!string.IsNullOrEmpty(entity.changepersonid))
                                        {
                                            uentity = userbll.GetEntity(entity.changepersonid);
                                            foreach (DepartmentEntity dentity in deptlist)
                                            {
                                                if (uentity.DepartmentId == dentity.DepartmentId)
                                                {
                                                    entity.changedeptcode = dentity.EnCode;
                                                    entity.changedeptname = dentity.FullName;
                                                    entity.changedeptid = dentity.DepartmentId;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            DepartmentEntity dentity = deptlist.FirstOrDefault();
                                            entity.changedeptcode = dentity.EnCode;
                                            entity.changedeptname = dentity.FullName;
                                            entity.changedeptid = dentity.DepartmentId;
                                        }
                                    }
                                    #endregion

                                    #region ������
                                    if (!string.IsNullOrEmpty(acceptperson))
                                    {
                                        var userlist = ulist.Where(p => p.RealName == acceptperson).ToList();
                                        if (userlist.Count() > 1)
                                        {
                                            muchmark = "2";  //�����˴�������
                                        }
                                        else
                                        {
                                            foreach (UserEntity uentity in userlist)
                                            {
                                                entity.acceptpersonid = uentity.UserId;
                                                entity.acceptpersonname = uentity.RealName;
                                            }
                                        }
                                    }
                                    #endregion

                                    #region ���յ�λ
                                    if (!string.IsNullOrEmpty(acceptdept))
                                    {
                                        var deptlist = dlist.Where(e => e.FullName == acceptdept).ToList();
                                        UserEntity uentity = new UserEntity();
                                        if (!string.IsNullOrEmpty(entity.acceptpersonid))
                                        {
                                            uentity = userbll.GetEntity(entity.acceptpersonid);
                                            foreach (DepartmentEntity dentity in deptlist)
                                            {
                                                if (uentity.DepartmentId == dentity.DepartmentId)
                                                {
                                                    entity.acceptdeptcode = dentity.EnCode;
                                                    entity.acceptdeptname = dentity.FullName;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            DepartmentEntity dentity = deptlist.FirstOrDefault();
                                            entity.acceptdeptcode = dentity.EnCode;
                                            entity.acceptdeptname = dentity.FullName;
                                        }
                                    }
                                    #endregion
                                }
                                else
                                {
                                    //���������� �� �������ε�λ
                                    if (!string.IsNullOrEmpty(changeperson) && !string.IsNullOrEmpty(changedept))
                                    {
                                        var userlist = ulist.Where(p => p.RealName == changeperson).ToList();
                                        var deptlist = dlist.Where(e => e.FullName == changedept).ToList();
                                        if (!string.IsNullOrEmpty(telephone))
                                        {
                                            userlist = userlist.Where(p => p.Telephone == telephone || p.Mobile == telephone).ToList();
                                        }
                                        foreach (DepartmentEntity dentity in deptlist)
                                        {
                                            foreach (UserEntity uentity in userlist)
                                            {
                                                if (uentity.DepartmentId == dentity.DepartmentId)
                                                {
                                                    entity.changepersonid = uentity.UserId;
                                                    entity.changepersonname = uentity.RealName;
                                                    entity.changedeptcode = dentity.EnCode;
                                                    entity.changedeptname = dentity.FullName;
                                                    entity.changedeptid = dentity.DepartmentId;
                                                    entity.changetelephone = uentity.Telephone;
                                                }
                                            }
                                        }
                                    }

                                    //������ �� ���յ�λ
                                    if (!string.IsNullOrEmpty(acceptperson) && !string.IsNullOrEmpty(acceptdept))
                                    {
                                        var userlist = ulist.Where(p => p.RealName == acceptperson).ToList();
                                        var deptlist = dlist.Where(e => e.FullName == acceptdept).ToList();
                                        foreach (DepartmentEntity dentity in deptlist)
                                        {
                                            foreach (UserEntity uentity in userlist)
                                            {
                                                if (uentity.DepartmentId == dentity.DepartmentId)
                                                {
                                                    entity.acceptpersonid = uentity.UserId;
                                                    entity.acceptpersonname = uentity.RealName;
                                                    entity.acceptdeptcode = dentity.EnCode;
                                                    entity.acceptdeptname = dentity.FullName;
                                                }
                                            }
                                        }
                                    }
                                }

                                //���Ľ�ֹʱ��
                                if (!string.IsNullOrEmpty(changedeadline))
                                {
                                    entity.changedeadline = Convert.ToDateTime(changedeadline); //���Ľ�ֹʱ��
                                }
                                //���Ĵ�ʩ
                                if (!string.IsNullOrEmpty(changemeasure))
                                {
                                    entity.changemeasure = changemeasure; //���Ĵ�ʩ
                                }
                                //��������
                                if (!string.IsNullOrEmpty(acceptdate))
                                {
                                    entity.acceptdate = Convert.ToDateTime(acceptdate); //��������
                                }
                                #endregion

                                #region ������֤
                                if (string.IsNullOrEmpty(entity.hidcode))
                                {
                                    resultmessage += "��������Ϊ�ա�";
                                    isadddobj = false;
                                }
                                if (!string.IsNullOrEmpty(entity.hidcode))
                                {
                                    //  if (Regex.IsMatch(str, "^(?<year>\\d{2,4})(?<month>\\d{1,2})(?<day>\\d{1,2})(\\d{4})?$"))
                                    if (!Regex.IsMatch(entity.hidcode, "^(\\d{2,4})(\\d0[1-9]|1[0-2])((0[1-9])|(1[0-9])|(2[0-9])|30|31)(\\d{4})$"))
                                    {
                                        resultmessage += "���������ʽ��֤ʧ�ܡ�";
                                        isadddobj = false;
                                    }
                                }
                                if (string.IsNullOrEmpty(entity.rankid))
                                {
                                    resultmessage += "��������Ϊ�ա�";
                                    isadddobj = false;
                                }
                                if (string.IsNullOrEmpty(entity.areacode) && mode != "6")
                                {
                                    if (string.IsNullOrEmpty(areaname))
                                    {
                                        resultmessage += "��������Ϊ�ա�";
                                    }
                                    else
                                    {
                                        resultmessage += "����������ο����ݲ�ƥ�䡢";
                                    }
                                    isadddobj = false;
                                }
                                if (string.IsNullOrEmpty(entity.majorclassify))
                                {
                                    resultmessage += "רҵ����Ϊ�ա�";
                                    isadddobj = false;
                                }
                                if (string.IsNullOrEmpty(entity.hidtype))
                                {
                                    resultmessage += "�������Ϊ�ա�";
                                    isadddobj = false;
                                }

                                if (string.IsNullOrEmpty(hiddescribe))
                                {
                                    //���罭��
                                    if (mode == "6")
                                    {
                                        resultmessage += "��������Ϊ�ա�";
                                    }
                                    else { resultmessage += "�¹���������(����)Ϊ�ա�"; }

                                    isadddobj = false;
                                }


                                //��������ж�
                                if (mode == "5")
                                {
                                    if (string.IsNullOrEmpty(entity.changepersonid))
                                    {
                                        if (string.IsNullOrEmpty(changeperson))
                                        {
                                            resultmessage += "����������Ϊ�ա�";
                                        }
                                        else
                                        {
                                            if (muchmark == "1")
                                            {
                                                resultmessage += "���������˳���������";
                                            }
                                            else
                                            {
                                                resultmessage += "���������˲�������ϵͳ�С�";
                                            }
                                        }
                                        isadddobj = false;
                                    }


                                    if (string.IsNullOrEmpty(entity.acceptpersonid))
                                    {
                                        if (string.IsNullOrEmpty(acceptperson))
                                        {
                                            resultmessage += "������Ϊ�ա�";
                                        }
                                        else
                                        {
                                            if (muchmark == "2")
                                            {
                                                resultmessage += "�����˳���������";
                                            }
                                            else
                                            {
                                                resultmessage += "�����˲�������ϵͳ�С�";
                                            }
                                        }
                                        isadddobj = false;
                                    }
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(entity.changepersonid) || string.IsNullOrEmpty(entity.changedeptcode))
                                    {
                                        if (string.IsNullOrEmpty(changedept))
                                        {
                                            resultmessage += "�������ε�λΪ�ա�";
                                        }
                                        else
                                        {
                                            if (string.IsNullOrEmpty(changeperson))
                                            {
                                                resultmessage += "����������Ϊ�ա�";
                                            }
                                            else
                                            {
                                                resultmessage += "�������ε�λ�����������˲�������ϵͳ�С�";
                                            }
                                        }
                                        isadddobj = false;
                                    }


                                    if (string.IsNullOrEmpty(entity.acceptpersonid) || string.IsNullOrEmpty(entity.acceptdeptcode))
                                    {
                                        if (string.IsNullOrEmpty(acceptdept))
                                        {
                                            resultmessage += "���յ�λΪ�ա�";
                                        }
                                        else
                                        {
                                            if (string.IsNullOrEmpty(acceptperson))
                                            {
                                                resultmessage += "������Ϊ�ա�";
                                            }
                                            else
                                            {
                                                resultmessage += "���յ�λ�������˲�������ϵͳ�С�";
                                            }
                                        }
                                        isadddobj = false;
                                    }
                                }

                                if (string.IsNullOrEmpty(changedeadline))
                                {
                                    resultmessage += "���Ľ�ֹʱ��Ϊ�ա�";
                                    isadddobj = false;
                                }


                                //��ȡ�Ѵ��ڵ���������
                                var htlist = htbaseinfobll.GetListByCode(entity.hidcode);

                                if (htlist.Count() > 0)
                                {
                                    //��������
                                    if (repeatdata == "0")
                                    {
                                        resultmessage += "���Ϊ" + entity.hidcode + "�������Ѵ��ڣ���������Ѻ��Բ�������";
                                        isadddobj = false;
                                    }
                                }

                                //������������ӵ����ϵ���
                                if (isadddobj)
                                {
                                    list.Add(entity);
                                }
                                else
                                {
                                    resultmessage = resultmessage.Substring(0, resultmessage.Length - 1) + ",�޷���������";
                                    resultlist.Add(resultmessage);
                                }
                                #endregion
                            }
                            catch
                            {
                                resultmessage += "���������쳣,�޷���������";
                                resultlist.Add(resultmessage);
                            }
                        }
                        if (resultlist.Count > 0)
                        {
                            foreach (string str in resultlist)
                            {
                                falseMessage += str + "</br>";
                            }
                        }
                        #endregion

                        #region �������ݼ���
                        foreach (ImportHidden entity in list)
                        {
                            string keyValue = string.Empty;
                            bool isExecute = true;
                            HTBaseInfoEntity baseentity = new HTBaseInfoEntity();
                            //��ȡ�Ѵ��ڵ���������
                            var htlist = htbaseinfobll.GetListByCode(entity.hidcode);

                            if (htlist.Count() > 0)
                            {
                                //���ǲ���
                                if (repeatdata == "1")
                                {
                                    baseentity = htlist.FirstOrDefault();
                                    keyValue = baseentity.ID;
                                }
                                else  //����
                                {
                                    isExecute = false;
                                }
                            }

                            entity.relevanceid = saftycheckdatarecordbll.GetCheckContentId(checkid, entity.checkobj, entity.checkcontent, curUser);
                            if (string.IsNullOrEmpty(entity.relevanceid))
                            {
                                isExecute = false;
                                falseMessage += "�����󼰼������δƥ����ߵ�ǰ������ѱ��������߸������������˼�鷶Χ,�޷���������</br>";
                            }
                            else
                            {
                                listIds.Add(entity.relevanceid.Split(',')[0]);
                            }

                            //�Ƿ�ִ��
                            if (isExecute)
                            {
                                #region ������Ϣ����
                                //����������Ϣ

                                baseentity.ADDTYPE = "0"; //��������������
                                baseentity.APPSIGN = "Import"; //������
                                baseentity.SAFETYCHECKOBJECTID = checkid;
                                if (null != safetyEntity)
                                {
                                    baseentity.SAFETYCHECKNAME = safetyEntity.CheckDataRecordName;
                                    baseentity.CHECKTYPE = checktypelist.Where(p => p.ItemValue == safetyEntity.CheckDataType.Value.ToString()).FirstOrDefault().ItemDetailId;
                                    baseentity.CHECKMANNAME = curUser.UserName;
                                    baseentity.CHECKMAN = curUser.UserId;
                                    baseentity.CHECKDEPARTID = curUser.DeptId;
                                    baseentity.CHECKDEPARTNAME = curUser.DeptName;
                                }
                                if (!string.IsNullOrEmpty(entity.relevanceid))
                                {
                                    string checkcontentid = entity.relevanceid.Split(',')[0];
                                    baseentity.RELEVANCEID = checkcontentid; //�������id
                                }
                                baseentity.HIDDEPART = curUser.OrganizeId;
                                baseentity.HIDDEPARTNAME = curUser.OrganizeName;
                                baseentity.HIDCODE = entity.hidcode;
                                baseentity.HIDNAME = entity.hiddenname;
                                baseentity.HIDRANK = entity.rankid;
                                baseentity.HIDPOINT = entity.areacode;
                                baseentity.HIDPOINTNAME = entity.areaname;
                                baseentity.MAJORCLASSIFY = entity.majorclassify;
                                baseentity.HIDTYPE = entity.hidtype;
                                baseentity.HIDDESCRIBE = entity.hiddescribe;
                                baseentity.HIDPHOTO = Guid.NewGuid().ToString();
                                baseentity.CHECKDATE = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                                baseentity.DEVICENAME = entity.devicename;
                                #region �������ͼƬ
                                if (!string.IsNullOrEmpty(hiddenDirectory))
                                {
                                    //��ǰ�ļ��д���
                                    if (Directory.Exists(hiddenDirectory))
                                    {
                                        //��ȡͼƬ
                                        DirectoryInfo directinfo = new DirectoryInfo(hiddenDirectory);
                                        List<FileInfo> fileinfoes = GetFiles(directinfo, new List<FileInfo>());
                                        #region ͼƬ�ļ�
                                        foreach (FileInfo finfo in fileinfoes)
                                        {
                                            string fextension = finfo.Extension;//�ļ���չ��
                                            string fname = finfo.Name; //�ļ�����
                                            //����ͼƬ��ʽ
                                            #region ����ͼƬ��ʽ
                                            if (fextension.ToLower().Contains("jpg") || fextension.ToLower().Contains("png") || fextension.ToLower().Contains("bmp")
                                             || fextension.ToLower().Contains("psd") || fextension.ToLower().Contains("gif") || fextension.ToLower().Contains("jpeg"))
                                            {
                                                string fhidcode = fname.Split('-')[0].ToString();
                                                if (entity.hidcode.ToString() == fhidcode)
                                                {
                                                    string fileGuid = Guid.NewGuid().ToString();
                                                    long filesize = finfo.Length;
                                                    string uploadDate = DateTime.Now.ToString("yyyyMMdd");
                                                    string virtualPath = string.Format("~/Resource/ht/images/{0}/{1}{2}", uploadDate, fileGuid, fextension);
                                                    string fullFileName = Server.MapPath(virtualPath);
                                                    //�����ļ���
                                                    string path = Path.GetDirectoryName(fullFileName);
                                                    Directory.CreateDirectory(path);
                                                    FileInfoEntity fileInfoEntity = new FileInfoEntity();
                                                    if (!System.IO.File.Exists(fullFileName))
                                                    {
                                                        //�����ļ�
                                                        finfo.CopyTo(fullFileName);
                                                    }
                                                    finfo.Delete();//ɾ���ļ�
                                                    //�ļ���Ϣд�����ݿ�
                                                    fileInfoEntity.Create();
                                                    fileInfoEntity.FileId = fileGuid;
                                                    fileInfoEntity.RecId = baseentity.HIDPHOTO; //����ID
                                                    fileInfoEntity.FolderId = "ht/images";
                                                    fileInfoEntity.FileName = file.FileName;
                                                    fileInfoEntity.FilePath = virtualPath;
                                                    fileInfoEntity.FileSize = filesize.ToString();
                                                    fileInfoEntity.FileExtensions = fextension;
                                                    fileInfoEntity.FileType = fextension.Replace(".", "");
                                                    fileinfobll.SaveForm("", fileInfoEntity);

                                                }
                                            }
                                            #endregion
                                        }
                                        #endregion
                                    }
                                }

                                #endregion

                                htbaseinfobll.SaveForm(baseentity.ID, baseentity);

                                if (string.IsNullOrEmpty(keyValue))
                                {
                                    string workFlow = "01";//��������
                                    bool isSucess = htworkflowbll.CreateWorkFlowObj(workFlow, baseentity.ID, curUser.UserId);
                                    if (isSucess)
                                    {
                                        bool res = htworkflowbll.UpdateWorkStreamByObjectId(baseentity.ID);  //����ҵ������״̬ 
                                    }
                                }

                                //���Ļ�����Ϣ
                                HTChangeInfoEntity centity = new HTChangeInfoEntity();
                                string changeid = string.Empty;
                                if (!string.IsNullOrEmpty(keyValue))
                                {
                                    centity = htchangeinfobll.GetEntityByCode(baseentity.HIDCODE);
                                    changeid = centity.ID;
                                }
                                centity.APPSIGN = "Import";
                                centity.HIDCODE = baseentity.HIDCODE;
                                centity.CHANGEPERSON = entity.changepersonid;
                                centity.CHANGEPERSONNAME = entity.changepersonname;
                                centity.CHANGEDUTYTEL = entity.changetelephone;
                                centity.CHANGEDUTYDEPARTCODE = entity.changedeptcode;
                                centity.CHANGEDUTYDEPARTNAME = entity.changedeptname;
                                centity.CHANGEDUTYDEPARTID = entity.changedeptid;
                                centity.CHANGEMEASURE = entity.changemeasure;
                                centity.CHANGEDEADINE = entity.changedeadline;
                                centity.HIDCHANGEPHOTO = Guid.NewGuid().ToString();
                                centity.ATTACHMENT = Guid.NewGuid().ToString();
                                centity.PLANMANAGECAPITAL = 0;
                                htchangeinfobll.SaveForm(changeid, centity);
                                //���ջ�����Ϣ
                                HTAcceptInfoEntity aentity = new HTAcceptInfoEntity();
                                string acceptid = string.Empty;
                                if (!string.IsNullOrEmpty(keyValue))
                                {
                                    aentity = htacceptinfobll.GetEntityByHidCode(baseentity.HIDCODE);
                                    acceptid = aentity.ID;
                                }
                                aentity.APPSIGN = "Import";
                                aentity.HIDCODE = baseentity.HIDCODE;
                                aentity.ACCEPTDEPARTCODE = entity.acceptdeptcode;
                                aentity.ACCEPTDEPARTNAME = entity.acceptdeptname;
                                aentity.ACCEPTPERSON = entity.acceptpersonid;
                                aentity.ACCEPTPERSONNAME = entity.acceptpersonname;
                                aentity.ACCEPTPHOTO = Guid.NewGuid().ToString();
                                aentity.ACCEPTDATE = entity.acceptdate;
                                htacceptinfobll.SaveForm(acceptid, aentity);
                                #endregion

                                total += 1;
                            }
                        }
                        #endregion
                    }
                    #endregion
                    #region Υ�²���
                    else if (sheets.Name.Contains("Υ��") || dt.Columns.Contains("Υ������"))//Υ����Ϣ����
                    {

                        #region ����װ��
                        List<ImportLllegal> list = new List<ImportLllegal>();
                        List<ImportLllegal> khlist = new List<ImportLllegal>();
                        //�Ȼ�ȡ��ְ���б�;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string resultmessage = "��" + (i + 1).ToString() + "��"; //��ʾ����

                            bool isadddobj = true;

                            //Υ�±���
                            string lllegalnumber = dt.Columns.Contains("Υ�±���") ? dt.Rows[i]["Υ�±���"].ToString().Trim() : string.Empty;
                            //������
                            string checkobj = dt.Columns.Contains("������") ? dt.Rows[i]["������"].ToString().Trim() : string.Empty;
                            //�������
                            string checkcontent = dt.Columns.Contains("�������") ? dt.Rows[i]["�������"].ToString().Trim() : string.Empty;
                            //Υ������ 
                            string lllegaltype = dt.Columns.Contains("Υ������") ? dt.Rows[i]["Υ������"].ToString().Trim() : string.Empty;
                            //Υ�¼���
                            string lllegallevel = dt.Columns.Contains("Υ�¼���") ? dt.Rows[i]["Υ�¼���"].ToString().Trim() : string.Empty;
                            //רҵ����
                            string majorclassify = dt.Columns.Contains("רҵ����") ? dt.Rows[i]["רҵ����"].ToString().Trim() : string.Empty;
                            //Υ����Ա
                            string lllegalperson = dt.Columns.Contains("Υ����Ա") ? dt.Rows[i]["Υ����Ա"].ToString().Trim() : string.Empty;
                            //Υ�µ�λ
                            string lllegalteam = dt.Columns.Contains("Υ�µ�λ") ? dt.Rows[i]["Υ�µ�λ"].ToString().Trim() : string.Empty;
                            //Υ��ʱ��
                            string lllegaltime = dt.Columns.Contains("Υ��ʱ��") ? dt.Rows[i]["Υ��ʱ��"].ToString().Trim() : string.Empty;
                            //Υ�µص�
                            string lllegaladdress = dt.Columns.Contains("Υ�µص�") ? dt.Rows[i]["Υ�µص�"].ToString().Trim() : string.Empty;
                            //Υ������
                            string lllegaldescribe = dt.Columns.Contains("Υ������") ? dt.Rows[i]["Υ������"].ToString().Trim() : string.Empty;
                            //����������
                            string reformpeople = dt.Columns.Contains("����������") ? dt.Rows[i]["����������"].ToString().Trim() : string.Empty;
                            //���������˵绰
                            string telephone = dt.Columns.Contains("���������˵绰") ? dt.Rows[i]["���������˵绰"].ToString().Trim() : string.Empty;
                            //�������ε�λ
                            string reformdeptname = dt.Columns.Contains("�������ε�λ") ? dt.Rows[i]["�������ε�λ"].ToString().Trim() : string.Empty;
                            //���Ľ�ֹʱ��
                            string reformdeadline = dt.Columns.Contains("���Ľ�ֹʱ��") ? dt.Rows[i]["���Ľ�ֹʱ��"].ToString().Trim() : string.Empty;
                            //���Ĵ�ʩ
                            string reformrequire = dt.Columns.Contains("���Ĵ�ʩ") ? dt.Rows[i]["���Ĵ�ʩ"].ToString().Trim() : string.Empty;
                            //������
                            string acceptpeople = dt.Columns.Contains("������") ? dt.Rows[i]["������"].ToString().Trim() : string.Empty;
                            //���յ�λ
                            string acceptdeptname = dt.Columns.Contains("���յ�λ") ? dt.Rows[i]["���յ�λ"].ToString().Trim() : string.Empty;
                            //��������
                            string accepttime = dt.Columns.Contains("��������") ? dt.Rows[i]["��������"].ToString().Trim() : string.Empty;

                            //Υ�����ε�λ
                            string lllegaldepart = dt.Columns.Contains("Υ�����ε�λ") ? dt.Rows[i]["Υ�����ε�λ"].ToString().Trim() : string.Empty;
                            //Υ�µ�λ���˽��
                            string wzdwpunish = dt.Columns.Contains("Υ�µ�λ���˽��") ? dt.Rows[i]["Υ�µ�λ���˽��"].ToString().Trim() : string.Empty;
                            //���ε�λ���˽��
                            string zrdwpunish = dt.Columns.Contains("���ε�λ���˽��") ? dt.Rows[i]["���ε�λ���˽��"].ToString().Trim() : string.Empty;

                            string relevanceid = string.Empty;

                            // Υ������ Υ�¼��� ����״̬
                            string itemCode = "'LllegalType','LllegalLevel','HidMajorClassify'";
                            //����
                            var itemlist = dataitemdetailbll.GetDataItemListByItemCode(itemCode);
                            try
                            {
                                #region ���󼯺�
                                ImportLllegal entity = new ImportLllegal();
                                //Υ�±��
                                entity.lllegalnumber = lllegalnumber; //Υ�±��
                                entity.checkcontent = checkcontent; //�������
                                entity.checkobj = checkobj; //������
                                //Υ������
                                if (!string.IsNullOrEmpty(lllegaltype))
                                {
                                    var typeList = itemlist.Where(p => p.EnCode == "LllegalType" && p.ItemName == lllegaltype);
                                    if (typeList.Count() > 0)
                                    {
                                        entity.lllegaltype = typeList.FirstOrDefault().ItemDetailId; //Υ������id
                                    }
                                }
                                //Υ�¼���
                                if (!string.IsNullOrEmpty(lllegallevel))
                                {
                                    var levelList = itemlist.Where(p => p.EnCode == "LllegalLevel" && p.ItemName == lllegallevel);
                                    if (levelList.Count() > 0)
                                    {
                                        entity.lllegallevel = levelList.FirstOrDefault().ItemDetailId; //Υ�¼���id
                                    }
                                }
                                //רҵ����
                                if (!string.IsNullOrEmpty(majorclassify))
                                {
                                    var majorList = itemlist.Where(p => p.EnCode == "HidMajorClassify" && p.ItemName == majorclassify);
                                    if (majorList.Count() > 0)
                                    {
                                        entity.majorclassify = majorList.FirstOrDefault().ItemDetailId; //רҵ����id
                                        entity.majorclassifyvalue = majorList.FirstOrDefault().ItemValue; //רҵ����ֵ
                                    }
                                }

                                //Υ�µ�λ
                                if (!string.IsNullOrEmpty(lllegalteam))
                                {
                                    var deptlist =new List<DepartmentEntity>() ;
                                    var userlist = new List<UserEntity>();

                                    if (lllegalteam.Contains("/"))
                                    {
                                         string[] depts = lllegalteam.Split('/');
                                         deptlist = dlist.Where(p => p.FullName == depts[0].ToString()).ToList();
                                        if (deptlist.Count() > 0)
                                        {
                                            deptlist = dlist.Where(p => p.FullName == depts[1].ToString() && p.ParentId == deptlist.FirstOrDefault().DepartmentId).ToList();
                                        }
                                    }
                                    else
                                    {
                                        deptlist = dlist.Where(e => e.FullName == lllegalteam).ToList();
                                    }

                                    //Υ���� 
                                    if (!string.IsNullOrEmpty(lllegalperson))
                                    {
                                        if (lllegalperson.Contains("/"))
                                        {
                                            string[] persons = lllegalperson.Split('/');
                                            var tempuserlist = ulist;
                                            if (!string.IsNullOrEmpty(persons[0].ToString()))
                                            {
                                                tempuserlist = tempuserlist.Where(p => p.RealName == persons[0].ToString().Trim()).ToList();
                                            }
                                            if (!string.IsNullOrEmpty(persons[1].ToString()))
                                            {
                                                tempuserlist = tempuserlist.Where(p => p.Account == persons[1].ToString().Trim() || p.Mobile == persons[1].ToString().Trim() || p.Telephone == persons[1].ToString().Trim()).ToList();
                                            }
                                            userlist = tempuserlist;
                                        }
                                        else
                                        {
                                            userlist = ulist.Where(p => p.RealName == lllegalperson.Trim() || p.Account == lllegalperson.Trim() || p.Mobile == lllegalperson.Trim() || p.Telephone == lllegalperson.Trim()).ToList();
                                        }

                                        if (userlist.Count() > 0)
                                        {
                                            foreach (UserEntity uentity in userlist)
                                            {
                                                foreach (DepartmentEntity dentity in deptlist)
                                                {
                                                    if (uentity.DepartmentId == dentity.DepartmentId)
                                                    {
                                                        entity.lllegalpersonid = uentity.UserId;
                                                        entity.lllegalperson = uentity.RealName;
                                                        entity.lllegalteam = dentity.FullName;
                                                        entity.lllegalteamcode = dentity.EnCode;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            entity.lllegalperson = lllegalperson;
                                            var dentity = deptlist.FirstOrDefault();
                                            if (null != dentity)
                                            {
                                                entity.lllegalteam = dentity.FullName;
                                                entity.lllegalteamcode = dentity.EnCode;
                                            }
                                        }
                                    }
                                    else  //���в���
                                    {
                                        var dentity = deptlist.FirstOrDefault();
                                        if (null != dentity)
                                        {
                                            entity.lllegalteam = dentity.FullName;
                                            entity.lllegalteamcode = dentity.EnCode;
                                        }
                                    }
                                }

                                //�������ε�λ
                                if (!string.IsNullOrEmpty(reformpeople) && !string.IsNullOrEmpty(reformdeptname))
                                {
                                    var userlist = new List<UserEntity>();
                                    var deptlist = new List<DepartmentEntity>();
                                    if (reformdeptname.Contains("/"))
                                    {
                                        string[] depts = reformdeptname.Split('/');
                                        deptlist = dlist.Where(p => p.FullName == depts[0].ToString()).ToList();
                                        if (deptlist.Count() > 0)
                                        {
                                            deptlist = dlist.Where(p => p.FullName == depts[1].ToString() && p.ParentId == deptlist.FirstOrDefault().DepartmentId).ToList();
                                        }
                                    }
                                    else
                                    {
                                        deptlist = dlist.Where(e => e.FullName == reformdeptname).ToList();
                                    }

                                    if (reformpeople.Contains("/"))
                                    {
                                        string[] persons = reformpeople.Split('/');
                                        var tempuserlist = ulist;
                                        if (!string.IsNullOrEmpty(persons[0].ToString()))
                                        {
                                            tempuserlist = tempuserlist.Where(p => p.RealName == persons[0].ToString().Trim()).ToList();
                                        }
                                        if (!string.IsNullOrEmpty(persons[1].ToString()))
                                        {
                                            tempuserlist = tempuserlist.Where(p => p.Account == persons[1].ToString().Trim() || p.Mobile == persons[1].ToString().Trim() || p.Telephone == persons[1].ToString().Trim()).ToList();
                                        }
                                        userlist = tempuserlist;
                                    }
                                    else
                                    {
                                        userlist = ulist.Where(p => p.RealName == reformpeople.Trim() || p.Account == reformpeople.Trim() || p.Mobile == reformpeople.Trim() || p.Telephone == reformpeople.Trim()).ToList();
                                    }
                                    //if (!string.IsNullOrEmpty(telephone))
                                    //{
                                    //    userlist = userlist.Where(p => p.Telephone == telephone || p.Mobile == telephone).ToList();
                                    //}
                                    foreach (UserEntity uentity in userlist)
                                    {
                                        foreach (DepartmentEntity dentity in deptlist)
                                        {
                                            if (uentity.DepartmentId == dentity.DepartmentId)
                                            {
                                                entity.reformpeopleid = uentity.UserId;
                                                entity.reformpeople = uentity.RealName;
                                                entity.reformdeptcode = dentity.EnCode;
                                                entity.reformdeptname = dentity.FullName;
                                                entity.reformtelephone = uentity.Telephone;
                                            }
                                        }
                                    }
                                }

                                //Υ�����ε�λ
                                #region Υ�����ε�λ
                                if (!string.IsNullOrEmpty(lllegaldepart))
                                {
                                    var deptlist = new List<DepartmentEntity>();
                                    if (lllegaldepart.Contains("/"))
                                    {
                                        string[] depts = lllegaldepart.Split('/');
                                        deptlist = dlist.Where(p => p.FullName == depts[0].ToString()).ToList();
                                        if (deptlist.Count() > 0)
                                        {
                                            deptlist = dlist.Where(p => p.FullName == depts[1].ToString() && p.ParentId == deptlist.FirstOrDefault().DepartmentId).ToList();
                                        }
                                    }
                                    else
                                    {
                                        deptlist = dlist.Where(e => e.FullName == lllegaldepart).ToList();
                                    }
                                    if (deptlist.Count() > 0)
                                    {
                                        var dentity = deptlist.FirstOrDefault();
                                        if (null != dentity)
                                        {
                                            entity.lllegaldepart = dentity.FullName;
                                            entity.lllegaldepartcode = dentity.EnCode;
                                            //��������
                                            if (mode == "5")
                                            {
                                                //�������ε�λ
                                                entity.reformdeptcode = dentity.EnCode;
                                                entity.reformdeptname = dentity.FullName;
                                                reformdeptname = entity.reformdeptname;

                                                //����������Ϊ���ĵ�λרҵ���ܻ�ȫԱ
                                                #region רҵ����
                                                if (!string.IsNullOrEmpty(entity.majorclassifyvalue))
                                                {
                                                    var zguserDt = htbaseinfobll.GetGeneralQueryBySql(string.Format(@"select * from v_userinfo where departmentcode = '{0}' and rolename like '%ר��%' and  (','||specialtytype||',') like ',{1},' ", entity.reformdeptcode, entity.majorclassifyvalue));
                                                    if (zguserDt.Rows.Count > 0)
                                                    {
                                                        entity.reformpeopleid = zguserDt.Rows[0]["userid"].ToString();
                                                        entity.reformpeople = zguserDt.Rows[0]["realname"].ToString();
                                                        entity.reformtelephone = zguserDt.Rows[0]["mobile"].ToString();
                                                    }
                                                    else   //��ȫ����Ա
                                                    {
                                                        var aquserDt = htbaseinfobll.GetGeneralQueryBySql(string.Format(@"select * from v_userinfo where departmentcode = '{0}' and rolename like '%��ȫ����Ա%'  ", entity.reformdeptcode));
                                                        if (aquserDt.Rows.Count > 0)
                                                        {
                                                            entity.reformpeopleid = aquserDt.Rows[0]["userid"].ToString();
                                                            entity.reformpeople = aquserDt.Rows[0]["realname"].ToString();
                                                            entity.reformtelephone = aquserDt.Rows[0]["mobile"].ToString();
                                                        }
                                                    }
                                                }
                                                else   //��ȫ����Ա
                                                {
                                                    var aquserDt = htbaseinfobll.GetGeneralQueryBySql(string.Format(@"select userid,realname,mobile  from v_userinfo where departmentcode = '{0}' and rolename like '%��ȫ����Ա%'  ", entity.reformdeptcode));
                                                    if (aquserDt.Rows.Count > 0)
                                                    {
                                                        entity.reformpeopleid = aquserDt.Rows[0]["userid"].ToString();
                                                        entity.reformpeople = aquserDt.Rows[0]["realname"].ToString();
                                                        entity.reformtelephone = aquserDt.Rows[0]["mobile"].ToString();
                                                    }
                                                }
                                                reformpeople = entity.reformpeople;
                                                #endregion
                                            }
                                        }
                                    }
                                }
                                #endregion

                                //Υ��ʱ��
                                if (!string.IsNullOrEmpty(lllegaltime))
                                {
                                    entity.lllegaltime = Convert.ToDateTime(lllegaltime); //Υ��ʱ��
                                }
                                //����
                                if (string.IsNullOrEmpty(lllegaltime) && mode == "5")
                                {
                                    lllegaltime = DateTime.Now.ToString("yyyy-MM-dd");
                                    entity.lllegaltime = Convert.ToDateTime(lllegaltime); //Υ��ʱ��
                                }
                                //���Ľ�ֹʱ��
                                if (!string.IsNullOrEmpty(reformdeadline))
                                {
                                    entity.reformdeadline = Convert.ToDateTime(reformdeadline); //���Ľ�ֹʱ��
                                }
                                //Υ������
                                if (!string.IsNullOrEmpty(lllegaldescribe))
                                {
                                    entity.lllegaldescribe = lllegaldescribe; //Υ������
                                }
                                //���Ĵ�ʩ
                                if (!string.IsNullOrEmpty(reformrequire))
                                {
                                    entity.reformrequire = reformrequire; //���Ĵ�ʩ
                                }
                                //������ �� ���յ�λ
                                if (!string.IsNullOrEmpty(acceptpeople) && !string.IsNullOrEmpty(acceptdeptname))
                                {
                                    var userlist = new List<UserEntity>();
                                    var deptlist = new List<DepartmentEntity>();
                                    if (acceptdeptname.Contains("/"))
                                    {
                                        string[] depts = acceptdeptname.Split('/');
                                        deptlist = dlist.Where(p => p.FullName == depts[0].ToString()).ToList();
                                        if (deptlist.Count() > 0)
                                        {
                                            deptlist = dlist.Where(p => p.FullName == depts[1].ToString() && p.ParentId == deptlist.FirstOrDefault().DepartmentId).ToList();
                                        }
                                    }
                                    else
                                    {
                                        deptlist = dlist.Where(e => e.FullName == acceptdeptname).ToList();
                                    }

                                    if (acceptpeople.Contains("/"))
                                    {
                                        string[] persons = acceptpeople.Split('/');
                                        var tempuserlist = ulist;
                                        if (!string.IsNullOrEmpty(persons[0].ToString()))
                                        {
                                            tempuserlist = tempuserlist.Where(p => p.RealName == persons[0].ToString().Trim()).ToList();
                                        }
                                        if (!string.IsNullOrEmpty(persons[1].ToString()))
                                        {
                                            tempuserlist = tempuserlist.Where(p => p.Account == persons[1].ToString().Trim() || p.Mobile == persons[1].ToString().Trim() || p.Telephone == persons[1].ToString().Trim()).ToList();
                                        }
                                        userlist = tempuserlist;
                                    }
                                    else
                                    {
                                        userlist = ulist.Where(p => p.RealName == acceptpeople.Trim() || p.Account == acceptpeople.Trim() || p.Mobile == acceptpeople.Trim() || p.Telephone == acceptpeople.Trim()).ToList();
                                    }

                                    foreach (UserEntity uentity in userlist)
                                    {
                                        foreach (DepartmentEntity dentity in deptlist)
                                        {
                                            if (uentity.DepartmentId == dentity.DepartmentId)
                                            {
                                                entity.acceptpeopleid = uentity.UserId;
                                                entity.acceptpeople = uentity.RealName;
                                                entity.acceptdeptcode = dentity.EnCode;
                                                entity.acceptdeptname = dentity.FullName;
                                            }
                                        }
                                    }
                                }
                                //����
                                if (!string.IsNullOrEmpty(acceptpeople) && mode == "5")
                                {
                                    string[] userstr = acceptpeople.Split('/');
                                    if (userstr.Length == 2)
                                    {
                                        if (!string.IsNullOrEmpty(userstr[0]) && !string.IsNullOrEmpty(userstr[1]))
                                        {
                                            var userlist = ulist.Where(p => p.RealName == userstr[0] && p.Account == userstr[1]).ToList();

                                            if (userlist.Count() > 0)
                                            {
                                                var uentity = userlist.FirstOrDefault();
                                                entity.acceptpeopleid = uentity.UserId;
                                                entity.acceptpeople = uentity.RealName;
                                                var acceptdept = departmentBLL.GetEntity(uentity.DepartmentId);
                                                entity.acceptdeptcode = acceptdept.EnCode;
                                                entity.acceptdeptname = acceptdept.FullName;
                                                acceptdeptname = entity.acceptdeptname;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        resultmessage += "��������д��ʽ����";
                                        isadddobj = false;
                                    }
                                }
                                //Υ�µ�λ���˽��
                                if (!string.IsNullOrEmpty(wzdwpunish))
                                {
                                    entity.wzdwpunish = wzdwpunish;
                                }
                                //���ε�λ���˽��
                                if (!string.IsNullOrEmpty(zrdwpunish))
                                {
                                    entity.zrdwpunish = zrdwpunish;
                                }
                                //��������
                                if (!string.IsNullOrEmpty(accepttime))
                                {
                                    entity.accepttime = Convert.ToDateTime(accepttime); //��������
                                }
                                #endregion

                                #region ������֤
                                if (string.IsNullOrEmpty(entity.lllegalnumber))
                                {
                                    resultmessage += "Υ�±���Ϊ�ա�";
                                    isadddobj = false;
                                }
                                if (!string.IsNullOrEmpty(entity.lllegalnumber))
                                {
                                    //  if (Regex.IsMatch(str, "^(?<year>\\d{2,4})(?<month>\\d{1,2})(?<day>\\d{1,2})(\\d{4})?$"))
                                    if (!Regex.IsMatch(entity.lllegalnumber, "^(\\d{2,4})(\\d0[1-9]|1[0-2])((0[1-9])|(1[0-9])|(2[0-9])|30|31)(\\d{3})$"))
                                    {
                                        resultmessage += "Υ�±����ʽ��֤ʧ�ܡ�";
                                        isadddobj = false;
                                    }
                                }
                                if (string.IsNullOrEmpty(entity.lllegaltype))
                                {
                                    resultmessage += "Υ������Ϊ�ա�";
                                    isadddobj = false;
                                }
                                if (string.IsNullOrEmpty(entity.lllegallevel))
                                {
                                    resultmessage += "Υ�¼���Ϊ�ա�";
                                    isadddobj = false;
                                }
                                if (string.IsNullOrEmpty(entity.majorclassify))
                                {
                                    resultmessage += "רҵ����Ϊ�ա�";
                                    isadddobj = false;
                                }
                                if (string.IsNullOrEmpty(entity.lllegalteamcode))
                                {
                                    if (string.IsNullOrEmpty(lllegalteam))
                                    {
                                        resultmessage += "Υ�µ�λΪ�ա�";
                                    }
                                    else
                                    {
                                        resultmessage += "Υ�µ�λ��������ϵͳ�С�";
                                    }
                                    isadddobj = false;
                                }
                                if (string.IsNullOrEmpty(lllegaltime))
                                {
                                    resultmessage += "Υ��ʱ��Ϊ�ա�";
                                    isadddobj = false;
                                }
                                if (string.IsNullOrEmpty(lllegaldescribe))
                                {
                                    resultmessage += "Υ������Ϊ�ա�";
                                    isadddobj = false;
                                }

                                if (mode == "5")
                                {
                                    if (string.IsNullOrEmpty(entity.lllegaldepartcode))
                                    {
                                        if (string.IsNullOrEmpty(lllegaldepart))
                                        {
                                            resultmessage += "Υ�����ε�λΪ�ա�";
                                        }
                                        else
                                        {
                                            resultmessage += "Υ�����ε�λ��������ϵͳ�С�";
                                        }
                                        isadddobj = false;
                                    }
                                }
                                if (string.IsNullOrEmpty(entity.reformpeopleid) || string.IsNullOrEmpty(entity.reformdeptcode))
                                {
                                    if (!string.IsNullOrEmpty(mode))
                                    {
                                        if (string.IsNullOrEmpty(entity.reformpeopleid))
                                        {
                                            resultmessage += "Υ�����ε�λ��Ӧ��רҵר����ȫ����Ա�����ڡ�";
                                        }
                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(entity.lllegaldepartcode))
                                        {
                                            if (string.IsNullOrEmpty(reformdeptname))
                                            {
                                                resultmessage += "�������ε�λΪ�ա�";
                                            }
                                            else
                                            {
                                                if (string.IsNullOrEmpty(reformpeople))
                                                {
                                                    resultmessage += "����������Ϊ�ա�";
                                                }
                                                else
                                                {
                                                    resultmessage += "�������ε�λ�����������˲�������ϵͳ�С�";
                                                }
                                            }
                                        }
                                    }
                                    isadddobj = false;
                                }
                                if (string.IsNullOrEmpty(reformdeadline))
                                {
                                    resultmessage += "���Ľ�ֹʱ��Ϊ�ա�";
                                    isadddobj = false;
                                }
                                if (string.IsNullOrEmpty(entity.acceptpeopleid) || string.IsNullOrEmpty(entity.acceptdeptcode))
                                {
                                    if (string.IsNullOrEmpty(acceptdeptname) && string.IsNullOrEmpty(mode))
                                    {
                                        resultmessage += "���յ�λΪ�ա�";
                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(acceptpeople))
                                        {
                                            resultmessage += "������Ϊ�ա�";
                                        }
                                        else
                                        {
                                            resultmessage += "���յ�λ�������˲�������ϵͳ�С�";
                                        }
                                    }
                                    isadddobj = false;
                                }

                                //��ȡ�Ѵ��ڵ���������
                                var htlist = lllegalregisterbll.GetListByNumber(entity.lllegalnumber);

                                if (htlist.Count() > 0)
                                {
                                    //��������
                                    if (repeatdata == "0")
                                    {
                                        resultmessage += "���Ϊ" + entity.lllegalnumber + "��Υ���Ѵ��ڣ���������Ѻ��Բ�������";
                                        isadddobj = false;
                                    }
                                }

                                if (isadddobj)
                                {
                                    list.Add(entity);
                                }
                                else
                                {
                                    resultmessage = resultmessage.Substring(0, resultmessage.Length - 1) + ",�޷���������";
                                    resultlist.Add(resultmessage);
                                }
                                #endregion
                            }
                            catch
                            {
                                resultmessage += "���������쳣,�޷���������";
                                resultlist.Add(resultmessage);
                            }
                        }
                        if (resultlist.Count > 0)
                        {
                            foreach (string str in resultlist)
                            {
                                falseMessage += str + "</br>";
                            }
                        }
                        #endregion

                        #region Υ�����ݼ���
                        foreach (ImportLllegal entity in list)
                        {
                            string keyValue = string.Empty;
                            int excuteVal = 0;
                            //Υ�»�����Ϣ
                            LllegalRegisterEntity baseentity = new LllegalRegisterEntity();

                            //��ȡ�Ѵ��ڵ���������
                            var lllegallist = lllegalregisterbll.GetListByNumber(entity.lllegalnumber);

                            if (lllegallist.Count() > 0)
                            {
                                //���ǲ���
                                if (repeatdata == "1")
                                {
                                    var otherlllegal = lllegallist.Where(p => p.CREATEUSERID != curUser.UserId);
                                    //�����˴�����
                                    if (otherlllegal.Count() > 0)
                                    {
                                        falseMessage += "Υ�±���Ϊ'" + entity.lllegalnumber + "'���������ѱ�" + otherlllegal.FirstOrDefault().CREATEUSERNAME + "�������޷�����,�������</br>";
                                        excuteVal = -1;
                                    }
                                    else //�Լ�����
                                    {
                                        if (lllegallist.Where(p => p.RESEVERONE == checkid && p.APPSIGN == "Import").Count() > 0)
                                        {
                                            baseentity = lllegallist.Where(p => p.RESEVERONE == checkid && p.APPSIGN == "Import").FirstOrDefault();
                                            //��ɾ����������
                                            lllegalregisterbll.RemoveForm(baseentity.ID);
                                            baseentity = new LllegalRegisterEntity();
                                            excuteVal = 1;
                                        }
                                        else
                                        {
                                            falseMessage += "Υ�±���Ϊ'" + entity.lllegalnumber + "'����������ͨ��������ʽ�������޷�����,�������</br>";
                                            excuteVal = -1;
                                        }
                                    }
                                }
                                else  //����
                                {
                                    excuteVal = 0;
                                }
                            }
                            else
                            {
                                excuteVal = 1;
                            }

                            //�ճ�����޼����Ŀ��ֱ�ӹ�������¼   �м����Ŀ�������󣬵����޼�����ݺͼ������Զ����������������    �м����Ŀ�������󣬵����м������δƥ��ɹ�����ʾ�޷�����
                            //ר�������飬�м����Ŀ�������޼�����ݺͼ������Զ����������������    �м����Ŀ�������󣬵����м������δƥ��ɹ�����ʾ�޷�����

                            //���ڼ����Ŀ
                            if (checkproject > 0)
                            {
                                string checkconnectid = saftycheckdatarecordbll.GetCheckContentId(checkid, entity.checkobj, entity.checkcontent, curUser);
                                if (!string.IsNullOrEmpty(checkconnectid))
                                {
                                    if (checkconnectid.Contains(","))
                                    {
                                       entity.resevertwo = checkconnectid.Split(',')[0];
                                       entity.reseverid = checkconnectid.Split(',')[1];
                                    }
                                }
                                if (string.IsNullOrEmpty(entity.resevertwo))
                                {
                                    excuteVal = -2;
                                    falseMessage += "Υ�±���Ϊ'" + entity.lllegalnumber + "'�����ݼ����󼰼������δƥ����ߵ�ǰ������ѱ��������߸������������˼�鷶Χ,�޷���������</br>";
                                }
                                else
                                {
                                    listIds.Add(entity.resevertwo);
                                }
                            }

                            //�Ƿ�ִ��
                            if (excuteVal > 0)
                            {
                                #region Υ����Ϣ����
                                baseentity.ADDTYPE = "0"; //����������Υ��
                                baseentity.APPSIGN = "Import"; //������
                                baseentity.RESEVERONE = checkid; //����¼id
                                baseentity.RESEVERID = entity.reseverid; //������id
                                baseentity.RESEVERTWO = entity.resevertwo; //�������id
                                baseentity.BELONGDEPARTID = curUser.OrganizeId;
                                baseentity.BELONGDEPART = curUser.OrganizeName;
                                baseentity.CREATEDEPTID = curUser.DeptId;
                                baseentity.CREATEDEPTNAME = curUser.DeptName;
                                baseentity.LLLEGALNUMBER = entity.lllegalnumber; //Υ�±��� //lllegalregisterbll.GenerateHidCode("bis_lllegalregister", "lllegalnumber", int.Parse(lenNum));
                                baseentity.REFORMREQUIRE = entity.reformrequire;
                                baseentity.LLLEGALTYPE = entity.lllegaltype;
                                baseentity.LLLEGALLEVEL = entity.lllegallevel;
                                baseentity.MAJORCLASSIFY = entity.majorclassify;
                                baseentity.LLLEGALPERSON = entity.lllegalperson;
                                baseentity.LLLEGALPERSONID = entity.lllegalpersonid;
                                baseentity.LLLEGALTEAM = entity.lllegalteam;
                                baseentity.LLLEGALTEAMCODE = entity.lllegalteamcode;
                                baseentity.LLLEGALDEPART = entity.lllegaldepart;
                                baseentity.LLLEGALDEPARTCODE = entity.lllegaldepartcode;
                                baseentity.LLLEGALTIME = entity.lllegaltime;
                                baseentity.LLLEGALADDRESS = entity.lllegaladdress;
                                baseentity.LLLEGALDESCRIBE = entity.lllegaldescribe;
                                baseentity.LLLEGALPIC = Guid.NewGuid().ToString();

                                #region ���Υ��ͼƬ
                                if (!string.IsNullOrEmpty(hiddenDirectory))
                                {
                                    //��ǰ�ļ��д���
                                    if (Directory.Exists(hiddenDirectory))
                                    {
                                        //��ȡͼƬ
                                        DirectoryInfo directinfo = new DirectoryInfo(hiddenDirectory);
                                        List<FileInfo> fileinfoes = GetFiles(directinfo, new List<FileInfo>());
                                        #region ͼƬ�ļ�
                                        foreach (FileInfo finfo in fileinfoes)
                                        {
                                            string fextension = finfo.Extension;//�ļ���չ��
                                            string fname = finfo.Name; //�ļ�����
                                            //����ͼƬ��ʽ
                                            #region ����ͼƬ��ʽ
                                            if (fextension.ToLower().Contains("jpg") || fextension.ToLower().Contains("png") || fextension.ToLower().Contains("bmp")
                                             || fextension.ToLower().Contains("psd") || fextension.ToLower().Contains("gif") || fextension.ToLower().Contains("jpeg"))
                                            {
                                                string flllegalnumber = fname.Split('-')[0].ToString();
                                                if (entity.lllegalnumber.ToString() == flllegalnumber)
                                                {
                                                    string fileGuid = Guid.NewGuid().ToString();
                                                    long filesize = finfo.Length;
                                                    string uploadDate = DateTime.Now.ToString("yyyyMMdd");
                                                    string virtualPath = string.Format("~/Resource/ht/images/{0}/{1}{2}", uploadDate, fileGuid, fextension);
                                                    string fullFileName = Server.MapPath(virtualPath);
                                                    //�����ļ���
                                                    string path = Path.GetDirectoryName(fullFileName);
                                                    Directory.CreateDirectory(path);
                                                    FileInfoEntity fileInfoEntity = new FileInfoEntity();
                                                    if (!System.IO.File.Exists(fullFileName))
                                                    {
                                                        //�����ļ�
                                                        finfo.CopyTo(fullFileName);
                                                    }
                                                    finfo.Delete();//ɾ���ļ�
                                                    //�ļ���Ϣд�����ݿ�
                                                    fileInfoEntity.Create();
                                                    fileInfoEntity.FileId = fileGuid;
                                                    fileInfoEntity.RecId = baseentity.LLLEGALPIC; //����ID
                                                    fileInfoEntity.FolderId = "ht/images";
                                                    fileInfoEntity.FileName = file.FileName;
                                                    fileInfoEntity.FilePath = virtualPath;
                                                    fileInfoEntity.FileSize = filesize.ToString();
                                                    fileInfoEntity.FileExtensions = fextension;
                                                    fileInfoEntity.FileType = fextension.Replace(".", "");
                                                    fileinfobll.SaveForm("", fileInfoEntity);

                                                }
                                            }
                                            #endregion
                                        }
                                        #endregion
                                    }
                                }

                                #endregion

                                lllegalregisterbll.SaveForm(keyValue, baseentity);

                                //���ڿ���ʹ��
                                entity.lllegalid = baseentity.ID;
                                khlist.Add(entity);

                                if (string.IsNullOrEmpty(keyValue))
                                {
                                    string workFlow = "03";//Υ�´���
                                    bool isSucess = htworkflowbll.CreateWorkFlowObj(workFlow, baseentity.ID, curUser.UserId);
                                    if (isSucess)
                                    {
                                        bool res = lllegalregisterbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", baseentity.ID);  //����ҵ������״̬
                                    }
                                }

                                #region ������Ϣ
                                //���˵�λ
                                //if (!string.IsNullOrEmpty(entity.wzdwpunish))
                                //{
                                //    if (Convert.ToDecimal(entity.wzdwpunish) > 0)
                                //    {
                                //        LllegalPunishEntity newpunishEntity = new LllegalPunishEntity();
                                //        newpunishEntity.LLLEGALID = baseentity.ID;
                                //        newpunishEntity.ASSESSOBJECT = "���˵�λ"; //���˶���
                                //        newpunishEntity.PERSONINCHARGEID = entity.lllegalteamcode;
                                //        newpunishEntity.PERSONINCHARGENAME = entity.lllegalteam;
                                //        newpunishEntity.PERFORMANCEPOINT = 0;
                                //        newpunishEntity.ECONOMICSPUNISH = Convert.ToDecimal(entity.wzdwpunish);
                                //        newpunishEntity.EDUCATION = 0;
                                //        newpunishEntity.LLLEGALPOINT = 0;
                                //        newpunishEntity.AWAITJOB = 0;
                                //        newpunishEntity.MARK = newpunishEntity.ASSESSOBJECT.Contains("����") ? "0" : "1"; //���0���ˣ�1����
                                //        lllegalpunishbll.SaveForm("", newpunishEntity);
                                //    }
                                //}

                                ////���ε�λ
                                //if (!string.IsNullOrEmpty(entity.zrdwpunish))
                                //{
                                //    if (Convert.ToDecimal(entity.zrdwpunish) > 0)
                                //    {
                                //        LllegalPunishEntity newpunishEntity = new LllegalPunishEntity();
                                //        newpunishEntity.LLLEGALID = baseentity.ID;
                                //        newpunishEntity.ASSESSOBJECT = "��һ����λ"; //���˶���
                                //        newpunishEntity.PERSONINCHARGEID = entity.lllegaldepartcode;
                                //        newpunishEntity.PERSONINCHARGENAME = entity.lllegaldepart;
                                //        newpunishEntity.PERFORMANCEPOINT = 0;
                                //        newpunishEntity.ECONOMICSPUNISH = Convert.ToDecimal(entity.zrdwpunish);
                                //        newpunishEntity.EDUCATION = 0;
                                //        newpunishEntity.LLLEGALPOINT = 0;
                                //        newpunishEntity.AWAITJOB = 0;
                                //        newpunishEntity.MARK = newpunishEntity.ASSESSOBJECT.Contains("����") ? "0" : "1"; //���0���ˣ�1����
                                //        lllegalpunishbll.SaveForm("", newpunishEntity);
                                //    }
                                //}
                                #endregion


                                //���Ļ�����Ϣ
                                LllegalReformEntity centity = new LllegalReformEntity();
                                string changeid = string.Empty;
                                if (!string.IsNullOrEmpty(keyValue))
                                {
                                    centity = lllegalreformbll.GetEntityByBid(keyValue);
                                    changeid = centity.ID;
                                }
                                centity.APPSIGN = "Import";
                                centity.LLLEGALID = baseentity.ID;
                                centity.REFORMPEOPLE = entity.reformpeople;
                                centity.REFORMPEOPLEID = entity.reformpeopleid;
                                centity.REFORMTEL = entity.reformtelephone;
                                centity.REFORMDEPTCODE = entity.reformdeptcode;
                                centity.REFORMDEPTNAME = entity.reformdeptname;
                                centity.REFORMDEADLINE = entity.reformdeadline;
                                centity.REFORMPIC = Guid.NewGuid().ToString();

                                lllegalreformbll.SaveForm(changeid, centity);
                                //���ջ�����Ϣ
                                LllegalAcceptEntity aentity = new LllegalAcceptEntity();
                                string acceptid = string.Empty;
                                if (!string.IsNullOrEmpty(keyValue))
                                {
                                    aentity = lllegalacceptbll.GetEntityByBid(keyValue);
                                    acceptid = centity.ID;
                                }
                                aentity.APPSIGN = "Import";
                                aentity.LLLEGALID = baseentity.ID;
                                aentity.ACCEPTPEOPLE = entity.acceptpeople;
                                aentity.ACCEPTPEOPLEID = entity.acceptpeopleid;
                                aentity.ACCEPTDEPTCODE = entity.acceptdeptcode;
                                aentity.ACCEPTDEPTNAME = entity.acceptdeptname;
                                aentity.ACCEPTTIME = entity.accepttime;
                                aentity.ACCEPTPIC = Guid.NewGuid().ToString();
                                lllegalacceptbll.SaveForm(acceptid, aentity);
                                #endregion

                                total += 1;
                            }
                        }
                        #endregion

                        resultlist = new List<string>();

                        #region Υ�¿��˲���
                        if (wb.Worksheets.Count == 3 || seconddt.Columns.Contains("Υ�±���"))//Υ�¿�����Ϣ����
                        {
                            List<ImportLllegalExamin> examlist = new List<ImportLllegalExamin>();
                            int khIndex = 1;
                            foreach (DataRow khrow in seconddt.Rows)
                            {
                                ImportLllegalExamin examentity = new ImportLllegalExamin();
                                int errornum = 0;
                                string resultmessage = "Υ�¿��˵�����е�" + khIndex.ToString() + "��"; //��ʾ����
                                //Υ�±���
                                string lllegalnumber = seconddt.Columns.Contains("Υ�±���") ? khrow["Υ�±���"].ToString().Trim() : "";
                                if (string.IsNullOrEmpty(lllegalnumber))
                                {
                                    lllegalnumber = seconddt.Columns.Contains("Column1") ? khrow["Column1"].ToString().Trim() : "";
                                }
                                //���˶���
                                string assessobject = seconddt.Columns.Contains("���˶���") ? khrow["���˶���"].ToString().Trim() : "";
                                if (string.IsNullOrEmpty(assessobject))
                                {
                                    assessobject = seconddt.Columns.Contains("Column2") ? khrow["Column2"].ToString().Trim() : "";
                                }
                                //������Ա/��λ
                                string personinchargename = seconddt.Columns.Contains("������Ա/��λ") ? khrow["������Ա/��λ"].ToString().Trim() : "";
                                if (string.IsNullOrEmpty(personinchargename))
                                {
                                    personinchargename = seconddt.Columns.Contains("Column3") ? khrow["Column3"].ToString().Trim() : "";
                                }
                                //���ô���(Ԫ) 
                                string economicspunish = seconddt.Columns.Contains("���ô���(Ԫ)") ? khrow["���ô���(Ԫ)"].ToString().Trim() : "0";
                                //Υ�¿۷�
                                string lllegalpoint = seconddt.Columns.Contains("Υ�¿۷�(��)") ? khrow["Υ�¿۷�(��)"].ToString().Trim() : "0";
                                //������ѵ(ѧʱ)
                                string education = seconddt.Columns.Contains("������ѵ(ѧʱ)") ? khrow["������ѵ(ѧʱ)"].ToString().Trim() : "0";
                                //����(��)
                                string awaitjob = seconddt.Columns.Contains("����(��)") ? khrow["����(��)"].ToString().Trim() : "0";
                                //EHS��Ч����(��)
                                string performancepoint = seconddt.Columns.Contains("EHS��Ч����(��)") ? khrow["EHS��Ч����(��)"].ToString().Trim() : "0";
                                //������Ա
                                string awardusername = seconddt.Columns.Contains("������Ա") ? khrow["������Ա"].ToString().Trim() : "";
                                if (string.IsNullOrEmpty(awardusername))
                                {
                                    awardusername = seconddt.Columns.Contains("Column9") ? khrow["Column9"].ToString().Trim() : "";
                                }
                                //��������(��)
                                string awardpoint = seconddt.Columns.Contains("��������(��)") ? khrow["��������(��)"].ToString().Trim() : "0";
                                //�������(Ԫ)
                                string awardmoney = seconddt.Columns.Contains("�������(Ԫ)") ? khrow["�������(Ԫ)"].ToString().Trim() : "0";

                                examentity.lllegalnumber = lllegalnumber;
                                examentity.assessobject = assessobject;
                                #region ���˶���
                                if (!string.IsNullOrEmpty(personinchargename))
                                {
                                    if (!string.IsNullOrEmpty(examentity.assessobject))
                                    {
                                        #region ������Ա
                                        if (examentity.assessobject.Contains("��Ա"))
                                        {
                                            examentity.personinchargename = personinchargename;
                                            string[] userstr = personinchargename.Split('/');
                                            if (userstr.Length == 2)
                                            {
                                                if (!string.IsNullOrEmpty(userstr[0]) && !string.IsNullOrEmpty(userstr[1]))
                                                {
                                                    var userlist = ulist.Where(p => p.RealName == userstr[0].Trim() && (p.Account == userstr[1].Trim() || p.Telephone == userstr[1].Trim() || p.Mobile == userstr[1].Trim())).ToList();

                                                    if (userlist.Count() > 0)
                                                    {
                                                        var uentity = userlist.FirstOrDefault();
                                                        examentity.personinchargeid = uentity.UserId;
                                                        examentity.personinchargename = uentity.RealName;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                var userlist = ulist.Where(p => p.Account == userstr[0].Trim() || p.Telephone == userstr[0].Trim() || p.Mobile == userstr[0].Trim()).ToList();
                                                if (userlist.Count() > 0)
                                                {
                                                    var uentity = userlist.FirstOrDefault();
                                                    examentity.personinchargeid = uentity.UserId;
                                                    examentity.personinchargename = uentity.RealName;
                                                }
                                            }
                                        }
                                        #endregion
                                        #region ���˲���
                                        else if (examentity.assessobject.Contains("����"))
                                        {
                                            if (personinchargename.Contains("/"))
                                            {
                                                string[] depts = personinchargename.Split('/');
                                                var deptlist = dlist.Where(p => p.FullName == depts[0].ToString()).ToList();
                                                if (deptlist.Count() > 0)
                                                {
                                                    var childdeptlist = dlist.Where(p => p.FullName == depts[1].ToString() && p.ParentId == deptlist.FirstOrDefault().DepartmentId).ToList();
                                                    if (childdeptlist.Count() > 0)
                                                    {
                                                        var reformentity = childdeptlist.FirstOrDefault();
                                                        examentity.personinchargeid = reformentity.EnCode;
                                                        examentity.personinchargename = reformentity.FullName;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                var deptlist = dlist.Where(e => e.FullName == personinchargename).ToList();
                                                if (deptlist.Count() > 0)
                                                {
                                                    var reformentity = deptlist.FirstOrDefault();
                                                    examentity.personinchargeid = reformentity.EnCode;
                                                    examentity.personinchargename = reformentity.FullName;
                                                }
                                            }
                                        }
                                        #endregion

                                        if (!string.IsNullOrEmpty(economicspunish))
                                        {
                                            examentity.economicspunish = economicspunish;
                                        }
                                        if (!string.IsNullOrEmpty(lllegalpoint))
                                        {
                                            examentity.lllegalpoint = lllegalpoint;
                                        }
                                        if (!string.IsNullOrEmpty(education))
                                        {
                                            examentity.education = education;
                                        }
                                        if (!string.IsNullOrEmpty(awaitjob))
                                        {
                                            examentity.awaitjob = awaitjob;
                                        }
                                        if (!string.IsNullOrEmpty(performancepoint))
                                        {
                                            examentity.performancepoint = performancepoint;
                                        }
                                    }
                                }
                                #endregion

                                #region ��������
                                if (!string.IsNullOrEmpty(awardusername))
                                {
                                    string[] userstr = awardusername.Split('/');
                                    if (userstr.Length == 2)
                                    {
                                        if (!string.IsNullOrEmpty(userstr[0]) && !string.IsNullOrEmpty(userstr[1]))
                                        {
                                            var userlist = ulist.Where(p => p.RealName == userstr[0].Trim() && (p.Account == userstr[1].Trim() || p.Telephone == userstr[1].Trim() || p.Mobile == userstr[1].Trim())).ToList();

                                            if (userlist.Count() > 0)
                                            {
                                                var uentity = userlist.FirstOrDefault();
                                                examentity.awarduserid = uentity.UserId;
                                                examentity.awardusername = uentity.RealName;
                                                examentity.awarddeptid = uentity.DepartmentId;
                                                examentity.awarddeptname = dlist.Where(p => p.DepartmentId == uentity.DepartmentId).FirstOrDefault().FullName;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var userlist = ulist.Where(p => p.Account == userstr[0] || p.Telephone == userstr[0] || p.Mobile == userstr[0]).ToList();
                                        if (userlist.Count() > 0)
                                        {
                                            var uentity = userlist.FirstOrDefault();
                                            examentity.awarduserid = uentity.UserId;
                                            examentity.awardusername = uentity.RealName;
                                            examentity.awarddeptid = uentity.DepartmentId;
                                            examentity.awarddeptname = dlist.Where(p => p.DepartmentId == uentity.DepartmentId).FirstOrDefault().FullName;
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(examentity.awarduserid))
                                    {
                                        if (!string.IsNullOrEmpty(awardpoint))
                                        {
                                            examentity.awardpoint = awardpoint;
                                        }
                                        if (!string.IsNullOrEmpty(awardmoney))
                                        {
                                            examentity.awardmoney = awardmoney;
                                        }
                                    }
                                }
                                #endregion

                                if (!string.IsNullOrEmpty(examentity.lllegalnumber))
                                {
                                    if (khlist.Where(p => p.lllegalnumber == examentity.lllegalnumber).Count() == 1)
                                    {
                                        if (string.IsNullOrEmpty(assessobject) && string.IsNullOrEmpty(awardusername))
                                        {
                                            resultmessage += "���˶���δѡ�񡢽�����Աδ��д��";
                                            errornum += 1;
                                        }
                                        else
                                        {
                                            //���˶���Ϊ��
                                            if (!string.IsNullOrEmpty(assessobject))
                                            {
                                                //���˵�λ
                                                if (assessobject.Contains("����") && string.IsNullOrEmpty(examentity.personinchargeid))
                                                {
                                                    resultmessage += assessobject + "��д����򲻴��ڡ�";
                                                    errornum += 1;
                                                }
                                            }
                                            if (!string.IsNullOrEmpty(awardusername))
                                            {
                                                if (string.IsNullOrEmpty(examentity.awarduserid))
                                                {
                                                    resultmessage += "������Ա��д����򲻴��ڡ�";
                                                    errornum += 1;
                                                }
                                            }
                                        }

                                        if ((examentity.assessobject.Contains("����") && !string.IsNullOrEmpty(examentity.personinchargeid)) || 
                                            (examentity.assessobject.Contains("��Ա") && !string.IsNullOrEmpty(examentity.personinchargename)) || 
                                            (!string.IsNullOrEmpty(examentity.awarduserid) &&!string.IsNullOrEmpty(examentity.awarddeptid)) )
                                        {
                                            var wzObject = khlist.Where(p => p.lllegalnumber == examentity.lllegalnumber).FirstOrDefault();
                                            examentity.lllegalid = wzObject.lllegalid;
                                            examlist.Add(examentity);
                                        }
                                    }
                                    else
                                    {
                                        resultmessage += "Υ�±���δ����Υ�µ�����е�Υ�¡�";
                                    }
                                }
                                else
                                {
                                    resultmessage += "Υ�±���δ��д��";
                                }
                                if (errornum > 0)
                                {
                                    resultlist.Add(resultmessage);
                                }
                                khIndex++;
                            }

                            if (resultlist.Count > 0)
                            {
                                foreach (string str in resultlist)
                                {
                                    falseMessage += str + "</br>";
                                }
                            }

                            //���ǲ���
                            if (repeatdata == "1")
                            {
                                List<string> khids = new List<string>();
                                List<string> jlids = new List<string>();
                                foreach (ImportLllegalExamin newexam in examlist)
                                {
                                    if ((newexam.assessobject.Contains("����") && !string.IsNullOrEmpty(newexam.personinchargeid)) ||
                                   (newexam.assessobject.Contains("��Ա") && !string.IsNullOrEmpty(newexam.personinchargename)))
                                    {
                                        if (!khids.Contains(newexam.lllegalid))
                                        {
                                            khids.Add(newexam.lllegalid);
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(newexam.awarduserid) && !string.IsNullOrEmpty(newexam.awarddeptid))
                                    {
                                        if (!jlids.Contains(newexam.lllegalid))
                                        {
                                            jlids.Add(newexam.lllegalid);
                                        }
                                    }
                               }
                                //ɾ������
                                foreach (string strId in khids)
                                {
                                    //��ɾ���������˼���
                                    lllegalpunishbll.DeleteLllegalPunishList(strId, "");
                                }
                                //ɾ������
                                foreach (string strId in jlids)
                                {
                                    //��ɾ��������������
                                    lllegalawarddetailbll.DeleteLllegalAwardList(strId);
                                }
                                int exmaIndex = 0;
                                int awardIndex = 0;
                                foreach (ImportLllegalExamin newexam in examlist)
                                {
                               
                                    if ((newexam.assessobject.Contains("����") && !string.IsNullOrEmpty(newexam.personinchargeid)) ||
                                        (newexam.assessobject.Contains("��Ա") && !string.IsNullOrEmpty(newexam.personinchargename)))
                                    {
                                        exmaIndex++;
                                        LllegalPunishEntity newpunishEntity = new LllegalPunishEntity();
                                        newpunishEntity.LLLEGALID = newexam.lllegalid;
                                        newpunishEntity.ASSESSOBJECT = newexam.assessobject; //���˶���
                                        newpunishEntity.PERSONINCHARGEID = newexam.personinchargeid;
                                        newpunishEntity.PERSONINCHARGENAME = newexam.personinchargename;
                                        newpunishEntity.PERFORMANCEPOINT = !string.IsNullOrEmpty(newexam.performancepoint) ? Convert.ToDecimal(newexam.performancepoint) : 0;
                                        newpunishEntity.ECONOMICSPUNISH = !string.IsNullOrEmpty(newexam.economicspunish) ? Convert.ToDecimal(newexam.economicspunish) : 0;
                                        newpunishEntity.EDUCATION = !string.IsNullOrEmpty(newexam.education) ? Convert.ToDecimal(newexam.education) : 0;
                                        newpunishEntity.LLLEGALPOINT = !string.IsNullOrEmpty(newexam.lllegalpoint) ? Convert.ToDecimal(newexam.lllegalpoint) : 0;
                                        newpunishEntity.AWAITJOB = !string.IsNullOrEmpty(newexam.awaitjob) ? Convert.ToDecimal(newexam.awaitjob) : 0;
                                        newpunishEntity.MARK = newpunishEntity.ASSESSOBJECT.Contains("����") ? "0" : "1"; //���0���ˣ�1����
                                        lllegalpunishbll.SaveForm("", newpunishEntity);
                                       
                                    }

                                    if (!string.IsNullOrEmpty(newexam.awarduserid) && !string.IsNullOrEmpty(newexam.awarddeptid))
                                    {
                                        awardIndex++;
                                        LllegalAwardDetailEntity awardEntity = new LllegalAwardDetailEntity();
                                        awardEntity.LLLEGALID = newexam.lllegalid;
                                        awardEntity.USERID = newexam.awarduserid; //��������
                                        awardEntity.USERNAME = newexam.awardusername;
                                        awardEntity.DEPTID = newexam.awarddeptid;
                                        awardEntity.DEPTNAME = newexam.awarddeptname;
                                        awardEntity.POINTS = !string.IsNullOrEmpty(newexam.awardpoint) ? int.Parse(newexam.awardpoint) : 0;
                                        awardEntity.MONEY = !string.IsNullOrEmpty(newexam.awardmoney) ? Convert.ToDecimal(newexam.awardmoney) : 0;
                                        lllegalawarddetailbll.SaveForm("", awardEntity);
                                    }
                                }
                                childmessage = ",����Υ�¿��˱���" + seconddt.Rows.Count.ToString() + "����¼,�ɹ�����Υ�¿���" + exmaIndex.ToString() + "����Υ�½���" + exmaIndex.ToString() + "��.";
                            }
                        }
                        #endregion
                    }
                    #endregion
                    #region ���ⲿ��
                    else if (sheets.Name.Contains("����") || dt.Columns.Contains("��������"))//������Ϣ����
                    {
                        bool isHavaWorkFlow = htworkflowbll.IsHaveCurWorkFlow("������������");

                        #region ����װ��
                        List<ImportQuestion> list = new List<ImportQuestion>();
                        //�Ȼ�ȡ��ְ���б�;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string resultmessage = "��" + (i + 1).ToString() + "������"; //��ʾ����

                            bool isadddobj = true;

                            //�������
                            string questionnumber = dt.Columns.Contains("�������") ? dt.Rows[i]["�������"].ToString().Trim() : string.Empty;
                            //��������
                            string questiondescribe = dt.Columns.Contains("��������") ? dt.Rows[i]["��������"].ToString().Trim() : string.Empty;
                            //����ص�
                            string questionaddress = dt.Columns.Contains("����ص�") ? dt.Rows[i]["����ص�"].ToString().Trim() : string.Empty;
                            //����ص�����
                            string checkimpcontent = dt.Columns.Contains("����ص�����") ? dt.Rows[i]["����ص�����"].ToString().Trim() : string.Empty;
                            //������
                            string checkobj = dt.Columns.Contains("������") ? dt.Rows[i]["������"].ToString().Trim() : string.Empty;
                            //�������
                            string checkcontent = dt.Columns.Contains("�������") ? dt.Rows[i]["�������"].ToString().Trim() : string.Empty;
                            //����������
                            string reformpeoplename = dt.Columns.Contains("����������") ? dt.Rows[i]["����������"].ToString().Trim() : string.Empty;
                            //���������˵绰
                            string telephone = dt.Columns.Contains("���������˵绰") ? dt.Rows[i]["���������˵绰"].ToString().Trim() : string.Empty;
                            //�������ε�λ
                            string reformdeptname = dt.Columns.Contains("�������ε�λ") ? dt.Rows[i]["�������ε�λ"].ToString().Trim() : string.Empty;
                            //����λ
                            string dutydeptname = dt.Columns.Contains("����λ") ? dt.Rows[i]["����λ"].ToString().Trim() : string.Empty;
                            //�ƻ��������
                            string reformplandate = dt.Columns.Contains("�ƻ��������") ? dt.Rows[i]["�ƻ��������"].ToString().Trim() : string.Empty;
                            //���Ĵ�ʩ
                            string reformmeasure = dt.Columns.Contains("���Ĵ�ʩ") ? dt.Rows[i]["���Ĵ�ʩ"].ToString().Trim() : string.Empty;
                            //��֤��
                            string verifypeoplename = dt.Columns.Contains("��֤��") ? dt.Rows[i]["��֤��"].ToString().Trim() : string.Empty;
                            //��֤��λ
                            string verifydeptname = dt.Columns.Contains("��֤��λ") ? dt.Rows[i]["��֤��λ"].ToString().Trim() : string.Empty;
                            //��֤����
                            string verifydate = dt.Columns.Contains("��֤����") ? dt.Rows[i]["��֤����"].ToString().Trim() : string.Empty;

                            string relevanceid = string.Empty;
                            try
                            {
                                #region ���󼯺�
                                ImportQuestion entity = new ImportQuestion();
                                //���
                                entity.serialnumber = i + 1; //���
                                entity.questionnumber = questionnumber; //�������
                                entity.checkimpcontent = checkimpcontent; //����ص�����
                                entity.checkobj = checkobj; //������
                                entity.checkcontent = checkcontent; //�������
                                //�������ε�λ
                                if (!string.IsNullOrEmpty(reformdeptname))
                                {
                                    if (reformdeptname.Contains("/"))
                                    {
                                        string[] depts = reformdeptname.Split('/');
                                        var deptlist = dlist.Where(p => p.FullName == depts[0].ToString()).ToList();
                                        if (deptlist.Count() > 0)
                                        {
                                            var childdeptlist = dlist.Where(p => p.FullName == depts[1].ToString() && p.ParentId == deptlist.FirstOrDefault().DepartmentId).ToList();
                                            if (childdeptlist.Count() > 0)
                                            {
                                                var reformentity = childdeptlist.FirstOrDefault();
                                                entity.reformdeptid = reformentity.DepartmentId;
                                                entity.reformdeptcode = reformentity.EnCode;
                                                entity.reformdeptname = reformentity.FullName;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var deptlist = dlist.Where(e => e.FullName == reformdeptname).ToList();
                                        if (deptlist.Count() > 0)
                                        {
                                            var reformentity = deptlist.FirstOrDefault();
                                            entity.reformdeptid = reformentity.DepartmentId;
                                            entity.reformdeptcode = reformentity.EnCode;
                                            entity.reformdeptname = reformentity.FullName;
                                        }
                                    }
                                }
                                //����λ
                                if (!string.IsNullOrEmpty(dutydeptname))
                                {
                                    if (dutydeptname.Contains("/"))
                                    {
                                        string[] depts = dutydeptname.Split('/');
                                        var deptlist = dlist.Where(p => p.FullName == depts[0].ToString()).ToList();
                                        if (deptlist.Count() > 0)
                                        {
                                            var childdeptlist = dlist.Where(p => p.FullName == depts[1].ToString() && p.ParentId == deptlist.FirstOrDefault().DepartmentId).ToList();
                                            if (childdeptlist.Count() > 0)
                                            {
                                                var dutydeptentity = childdeptlist.FirstOrDefault();
                                                entity.dutydeptid = dutydeptentity.DepartmentId;
                                                entity.dutydeptcode = dutydeptentity.EnCode;
                                                entity.dutydeptname = dutydeptentity.FullName;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var deptlist = dlist.Where(e => e.FullName == dutydeptname).ToList();
                                        if (deptlist.Count() > 0)
                                        {
                                            var dutydeptentity = deptlist.FirstOrDefault();
                                            entity.dutydeptid = dutydeptentity.DepartmentId;
                                            entity.dutydeptcode = dutydeptentity.EnCode;
                                            entity.dutydeptname = dutydeptentity.FullName;
                                        }
                                    }
                                }

                                #region ����������
                                bool reformWarn = true;
                                if (!string.IsNullOrEmpty(reformpeoplename))
                                {
                                    List<UserEntity> reformuserlist = ulist;
                                    //�������ε�λ //����λ
                                    if (!string.IsNullOrEmpty(entity.reformdeptid) || !string.IsNullOrEmpty(entity.dutydeptid))
                                    {
                                        reformuserlist = reformuserlist.Where(p => p.DepartmentId == entity.reformdeptid || p.DepartmentId == entity.dutydeptid).ToList();
                                    }

                                    string[] reformpeoples = new string[] { };

                                    if (reformpeoplename.Split(',').Length > 0)
                                    {
                                        reformpeoples = reformpeoplename.Split(',');
                                    }
                                    else if (reformpeoplename.Split('��').Length > 0)
                                    {
                                        reformpeoples = reformpeoplename.Split('��');
                                    }
                                    else if (reformpeoplename.Split('��').Length > 0)
                                    {
                                        reformpeoples = reformpeoplename.Split('��');
                                    }

                                    int reformlen = reformpeoples.Length;
                                    int searchIndex = 0;
                                    foreach (string userstr in reformpeoples)
                                    {
                                        List<UserEntity> glreformusers = new List<UserEntity>();

                                        glreformusers = reformuserlist;

                                        if (!string.IsNullOrEmpty(userstr))
                                        {
                                            if (userstr.Contains("/"))
                                            {
                                                string[] persons = userstr.Split('/');
                                                if (!string.IsNullOrEmpty(persons[0].ToString()))
                                                {
                                                    glreformusers = glreformusers.Where(p => p.RealName == persons[0].ToString().Trim()).ToList();
                                                }
                                                if (!string.IsNullOrEmpty(persons[1].ToString()))
                                                {
                                                    glreformusers = glreformusers.Where(p => p.Account == persons[1].ToString().Trim() || p.Mobile == persons[1].ToString().Trim() || p.Telephone == persons[1].ToString().Trim()).ToList();
                                                }
                                            }
                                            else
                                            {
                                                glreformusers = glreformusers.Where(p => p.RealName == userstr.Trim() || p.Account == userstr.Trim() || p.Mobile == userstr.Trim() || p.Telephone == userstr.Trim()).ToList();
                                            }
                                            if (glreformusers.Count() > 0)
                                            {
                                                var reformUserEntity = glreformusers.FirstOrDefault();
                                                entity.reformpeople += reformUserEntity.Account + ",";
                                                entity.reformpeoplename += reformUserEntity.RealName + ",";
                                                if (!string.IsNullOrEmpty(reformUserEntity.Mobile))
                                                {
                                                    if (!string.IsNullOrEmpty(reformUserEntity.Telephone))
                                                    {
                                                        entity.reformtelephone += !string.IsNullOrEmpty(reformUserEntity.Mobile) ? reformUserEntity.Mobile + "," : reformUserEntity.Telephone + ",";
                                                    }
                                                    else
                                                    {
                                                        entity.reformtelephone += !string.IsNullOrEmpty(reformUserEntity.Mobile) ? reformUserEntity.Mobile + "," : "";
                                                    }
                                                }
                                                searchIndex++;
                                            }
                                        }
                                    }

                                    if (searchIndex == reformlen)
                                    {
                                        reformWarn = false;

                                        if (!string.IsNullOrEmpty(entity.reformpeople))
                                        {
                                            entity.reformpeople = entity.reformpeople.Substring(0, entity.reformpeople.Length - 1);
                                        }
                                        if (!string.IsNullOrEmpty(entity.reformpeoplename))
                                        {
                                            entity.reformpeoplename = entity.reformpeoplename.Substring(0, entity.reformpeoplename.Length - 1);
                                        }
                                        if (!string.IsNullOrEmpty(entity.reformtelephone))
                                        {
                                            entity.reformtelephone = entity.reformtelephone.Substring(0, entity.reformtelephone.Length - 1);
                                        }
                                    }

                                }
                                #endregion

                                //�ƻ��������
                                if (!string.IsNullOrEmpty(reformplandate))
                                {
                                    entity.reformplandate = Convert.ToDateTime(reformplandate); //���Ľ�ֹʱ��
                                }
                                //����ص�
                                if (!string.IsNullOrEmpty(questionaddress))
                                {
                                    entity.questionaddress = questionaddress; //����ص�
                                }
                                //��������
                                if (!string.IsNullOrEmpty(questiondescribe))
                                {
                                    entity.questiondescribe = questiondescribe; //��������
                                }
                                //���Ĵ�ʩ
                                if (!string.IsNullOrEmpty(reformmeasure))
                                {
                                    entity.reformmeasure = reformmeasure; //���Ĵ�ʩ
                                }
                                //����ɽ�Զ�����
                                if (mode != "8")
                                {
                                    #region ��֤��
                                    if (!string.IsNullOrEmpty(verifypeoplename))
                                    {
                                        List<UserEntity> verifyuserlist = new List<UserEntity>();
                                        if (verifypeoplename.Contains("/"))
                                        {
                                            string[] persons = verifypeoplename.Split('/');
                                            if (!string.IsNullOrEmpty(persons[0].ToString()))
                                            {
                                                verifyuserlist = ulist.Where(p => p.RealName == persons[0].ToString().Trim()).ToList();
                                            }
                                            if (!string.IsNullOrEmpty(persons[1].ToString()))
                                            {
                                                verifyuserlist = ulist.Where(p => p.Account == persons[1].ToString().Trim() || p.Mobile == persons[1].ToString().Trim() || p.Telephone == persons[1].ToString().Trim()).ToList();
                                            }
                                        }
                                        else
                                        {
                                            verifyuserlist = ulist.Where(p => p.RealName == verifypeoplename.Trim() || p.Account == verifypeoplename.Trim() || p.Mobile == verifypeoplename.Trim() || p.Telephone == verifypeoplename.Trim()).ToList();
                                        }
                                        if (verifyuserlist.Count() > 0)
                                        {
                                            var verifyUserEntity = verifyuserlist.FirstOrDefault();
                                            entity.verifypeople = verifyUserEntity.Account;
                                            entity.verifypeoplename = verifyUserEntity.RealName;
                                            var verifyDeptEntity = dlist.Where(p => p.DepartmentId == verifyUserEntity.DepartmentId).FirstOrDefault();
                                            if (null != verifyDeptEntity)
                                            {
                                                entity.verifydeptid = verifyDeptEntity.DepartmentId;
                                                entity.verifydeptcode = verifyDeptEntity.EnCode;
                                                entity.verifydeptname = verifyDeptEntity.FullName;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var currentUser = ulist.Where(p => p.UserId == curUser.UserId).FirstOrDefault();
                                        entity.verifypeople = curUser.Account;
                                        entity.verifypeoplename = curUser.UserName;
                                        entity.verifydeptid = curUser.DeptId;
                                        entity.verifydeptcode = curUser.DeptCode;
                                        entity.verifydeptname = curUser.DeptName;
                                    }
                                    #endregion
                                }
                                //��֤����
                                if (!string.IsNullOrEmpty(verifydate))
                                {
                                    entity.verifydate = Convert.ToDateTime(verifydate); //��֤����
                                }
                                #endregion

                                #region ������֤
                                if (string.IsNullOrEmpty(questionnumber))
                                {
                                    resultmessage += "�������Ϊ�ա�";
                                    isadddobj = false;
                                }
                                if (!string.IsNullOrEmpty(questionnumber))
                                {
                                    if (questionnumber.Length == 13 || questionnumber.Length == 14)
                                    {
                                        //AQ2020��11��0001
                                        if (questionnumber.Substring(0, 2) == "AQ" && questionnumber.Substring(6, 1) == "��" && (questionnumber.Substring(8, 1) == "��" || questionnumber.Substring(9, 1) == "��"))
                                        {
                                            isadddobj = true;
                                        }
                                        else
                                        {
                                            resultmessage += "��������ʽ��֤ʧ�ܡ�";
                                            isadddobj = false;
                                        }
                                    }
                                    else
                                    {
                                        resultmessage += "��������ʽ��֤ʧ�ܡ�";
                                        isadddobj = false;
                                    }
                                    ////  if (Regex.IsMatch(str, "^(?<year>\\d{2,4})(?<month>\\d{1,2})(?<day>\\d{1,2})(\\d{4})?$"))
                                    //if (!Regex.IsMatch(questionnumber, "AQ^(\\d{2,4})(\\d0[1-9]|1[0-2])((0[1-9])|(1[0-9])|(2[0-9])|30|31)(\\d{4})$"))
                                    //{
                                    //    resultmessage += "���������ʽ��֤ʧ�ܡ�";
                                    //    isadddobj = false;
                                    //}
                                }
                                if (string.IsNullOrEmpty(questiondescribe))
                                {
                                    resultmessage += "��������Ϊ�ա�";
                                    isadddobj = false;
                                }
                                if (string.IsNullOrEmpty(reformpeoplename))
                                {
                                    resultmessage += "����������Ϊ�ա�";
                                    isadddobj = false;
                                }
                                else
                                {
                                    if (reformWarn)
                                    {
                                        resultmessage += "����������������Ա��д����򲻴������������ε�λ(����λ)��";
                                        isadddobj = false;
                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(entity.reformpeople))
                                        {
                                            resultmessage += "������������д����򲻴��ڡ�";
                                            isadddobj = false;
                                        }
                                    }
                                }

                                if (string.IsNullOrEmpty(reformdeptname))
                                {
                                    resultmessage += "�������ε�λΪ�ա�";
                                    isadddobj = false;
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(entity.reformdeptid))
                                    {
                                        resultmessage += "�������ε�λ��д����򲻴��ڡ�";
                                        isadddobj = false;
                                    }
                                }

                                if (string.IsNullOrEmpty(reformmeasure))
                                {
                                    resultmessage += "���Ĵ�ʩΪ�ա�";
                                    isadddobj = false;
                                }
                                if (string.IsNullOrEmpty(reformplandate))
                                {
                                    resultmessage += "�ƻ��������Ϊ�ա�";
                                    isadddobj = false;
                                }

                                if (isadddobj)
                                {
                                    list.Add(entity);
                                }
                                else
                                {
                                    resultmessage = resultmessage.Substring(0, resultmessage.Length - 1) + ",�޷���������";
                                    resultlist.Add(resultmessage);
                                }
                                #endregion
                            }
                            catch
                            {
                                resultmessage += "���������쳣,�޷���������";
                                resultlist.Add(resultmessage);
                            }
                        }
                        if (resultlist.Count > 0)
                        {
                            foreach (string str in resultlist)
                            {
                                falseMessage += str + "</br>";
                            }
                        }
                        #endregion
                        #region �������ݼ���
                        foreach (ImportQuestion entity in list)
                        {
                            string keyValue = string.Empty;
                            int excuteVal = 0;
                            //���������Ϣ
                            QuestionInfoEntity baseentity = new QuestionInfoEntity();

                            //��ȡ�Ѵ��ڵ���������
                            var questionlist = questioninfobll.GetListByNumber(entity.questionnumber);

                            if (questionlist.Count() > 0)
                            {
                                //���ǲ���
                                if (repeatdata == "1")
                                {
                                    var otherQuestion = questionlist.Where(p => p.CREATEUSERID != curUser.UserId);
                                    //�����˴�����
                                    if (otherQuestion.Count() > 0)
                                    {
                                        falseMessage += "�������Ϊ'" + entity.questionnumber + "'���������ѱ�" + otherQuestion.FirstOrDefault().CREATEUSERNAME + "�������޷�����,�������</br>";
                                        excuteVal = -1;
                                    }
                                    else //�Լ�����
                                    {
                                        if (questionlist.Where(p => p.CHECKID == checkid && p.APPSIGN == "Import").Count() > 0)
                                        {
                                            baseentity = questionlist.Where(p => p.CHECKID == checkid && p.APPSIGN == "Import").FirstOrDefault();
                                            //��ɾ����������
                                            questioninfobll.RemoveForm(baseentity.ID);
                                            baseentity = new QuestionInfoEntity();
                                            excuteVal = 1;
                                        }
                                        else
                                        {
                                            falseMessage += "�������Ϊ'" + entity.questionnumber + "'����������ͨ��������ʽ�������޷�����,�������</br>";
                                            excuteVal = -1;
                                        }
                                    }
                                }
                                else  //����
                                {
                                    excuteVal = 0;
                                }
                            }
                            else
                            {
                                excuteVal = 1;
                            }

                            //�ճ�����޼����Ŀ��ֱ�ӹ�������¼   �м����Ŀ�������󣬵����޼�����ݺͼ������Զ����������������    �м����Ŀ�������󣬵����м������δƥ��ɹ�����ʾ�޷�����
                            //ר�������飬�м����Ŀ�������޼�����ݺͼ������Զ����������������    �м����Ŀ�������󣬵����м������δƥ��ɹ�����ʾ�޷�����

                            //���ڼ����Ŀ
                            if (checkproject > 0)
                            {
                                string checkconnectid = saftycheckdatarecordbll.GetCheckContentId(checkid, entity.checkobj, entity.checkcontent, curUser);
                                if (!string.IsNullOrEmpty(checkconnectid))
                                {
                                    if (checkconnectid.Contains(","))
                                    {
                                        entity.correlationid = checkconnectid.Split(',')[0].ToString();
                                        entity.relevanceid = checkconnectid.Split(',')[1].ToString();
                                    }
                                }
                                if (string.IsNullOrEmpty(entity.correlationid))
                                {
                                    excuteVal = -2;
                                    falseMessage += "�������Ϊ'" + entity.questionnumber + "'�����ݼ����󼰼������δƥ����ߵ�ǰ������ѱ��������߸������������˼�鷶Χ,�޷���������</br>";
                                }
                                else
                                {
                                    listIds.Add(entity.correlationid);
                                }
                            }
                          

                            //�Ƿ�ɹ�ִ��
                            if (excuteVal > 0)
                            {
                                baseentity.APPSIGN = "Import"; //������
                                baseentity.CHECKID = checkid; //����¼id
                                baseentity.QUESTIONNUMBER = entity.questionnumber;//�������
                                baseentity.CHECKCONTENT = entity.checkimpcontent; //����ص�����
                                baseentity.RELEVANCEID = entity.relevanceid; //������id
                                baseentity.CORRELATIONID = entity.correlationid; //�������id
                                baseentity.BELONGDEPTID = curUser.OrganizeId;
                                baseentity.BELONGDEPTNAME = curUser.OrganizeName;
                                if (null != safetyEntity)
                                {
                                    baseentity.CHECKNAME = safetyEntity.CheckDataRecordName;
                                    baseentity.CHECKTYPE = safetyEntity.CheckDataType.Value.ToString();
                                    baseentity.CHECKPERSONNAME = curUser.UserName;
                                    baseentity.CHECKPERSONID = curUser.UserId;
                                    baseentity.CHECKDEPTID = curUser.DeptId;
                                    baseentity.CHECKDEPTNAME = curUser.DeptName;
                                }
                                baseentity.CHECKDATE = DateTime.Now; //���ʱ��
                                baseentity.QUESTIONADDRESS = entity.questionaddress; //����ص�
                                baseentity.QUESTIONDESCRIBE = entity.questiondescribe;//��������
                                baseentity.QUESTIONPIC = Guid.NewGuid().ToString();
                                #region �������ͼƬ
                                if (!string.IsNullOrEmpty(hiddenDirectory))
                                {
                                    //��ǰ�ļ��д���
                                    if (Directory.Exists(hiddenDirectory))
                                    {
                                        //��ȡͼƬ
                                        DirectoryInfo directinfo = new DirectoryInfo(hiddenDirectory);
                                        List<FileInfo> fileinfoes = GetFiles(directinfo, new List<FileInfo>());
                                        #region ͼƬ�ļ�
                                        foreach (FileInfo finfo in fileinfoes)
                                        {
                                            string fextension = finfo.Extension;//�ļ���չ��
                                            string fname = finfo.Name; //�ļ�����
                                            //����ͼƬ��ʽ
                                            #region ����ͼƬ��ʽ
                                            if (fextension.ToLower().Contains("jpg") || fextension.ToLower().Contains("png") || fextension.ToLower().Contains("bmp")
                                             || fextension.ToLower().Contains("psd") || fextension.ToLower().Contains("gif") || fextension.ToLower().Contains("jpeg"))
                                            {
                                                string fserialnumber = fname.Split('-')[0].ToString();
                                                if (entity.questionnumber.ToString() == fserialnumber)
                                                {
                                                    string fileGuid = Guid.NewGuid().ToString();
                                                    long filesize = finfo.Length;
                                                    string uploadDate = DateTime.Now.ToString("yyyyMMdd");
                                                    string virtualPath = string.Format("~/Resource/ht/images/{0}/{1}{2}", uploadDate, fileGuid, fextension);
                                                    string fullFileName = Server.MapPath(virtualPath);
                                                    //�����ļ���
                                                    string path = Path.GetDirectoryName(fullFileName);
                                                    Directory.CreateDirectory(path);
                                                    FileInfoEntity fileInfoEntity = new FileInfoEntity();
                                                    if (!System.IO.File.Exists(fullFileName))
                                                    {
                                                        //�����ļ�
                                                        finfo.CopyTo(fullFileName);
                                                    }
                                                    finfo.Delete();//ɾ���ļ�
                                                    //�ļ���Ϣд�����ݿ�
                                                    fileInfoEntity.Create();
                                                    fileInfoEntity.FileId = fileGuid;
                                                    fileInfoEntity.RecId = baseentity.QUESTIONPIC; //����ID
                                                    fileInfoEntity.FolderId = "ht/images";
                                                    fileInfoEntity.FileName = file.FileName;
                                                    fileInfoEntity.FilePath = virtualPath;
                                                    fileInfoEntity.FileSize = filesize.ToString();
                                                    fileInfoEntity.FileExtensions = fextension;
                                                    fileInfoEntity.FileType = fextension.Replace(".", "");
                                                    fileinfobll.SaveForm("", fileInfoEntity);
                                                }
                                            }
                                            #endregion
                                        }
                                        #endregion
                                    }
                                }
                                #endregion
                                questioninfobll.SaveForm("", baseentity);
                                string workFlow = "09";//���⴦��
                                bool isSucess = htworkflowbll.CreateWorkFlowObj(workFlow, baseentity.ID, curUser.UserId);
                                if (isSucess)
                                {
                                    htworkflowbll.UpdateFlowStateByObjectId("bis_questioninfo", "flowstate", baseentity.ID);  //����ҵ������״̬
                                }
                                //���Ļ�����Ϣ
                                QuestionReformEntity centity = new QuestionReformEntity();
                                centity.APPSIGN = "Import";
                                centity.QUESTIONID = baseentity.ID;
                                centity.REFORMMEASURE = entity.reformmeasure;
                                centity.REFORMPEOPLENAME = entity.reformpeoplename;
                                centity.REFORMPEOPLE = entity.reformpeople;
                                centity.REFORMTEL = entity.reformtelephone;
                                centity.REFORMDEPTID = entity.reformdeptid;
                                centity.REFORMDEPTCODE = entity.reformdeptcode;
                                centity.REFORMDEPTNAME = entity.reformdeptname;
                                centity.REFORMPLANDATE = entity.reformplandate;
                                centity.REFORMPIC = Guid.NewGuid().ToString();
                                centity.DUTYDEPTID = entity.dutydeptid;
                                centity.DUTYDEPTNAME = entity.dutydeptname;
                                centity.DUTYDEPTCODE = entity.dutydeptcode;
                                questionreformbll.SaveForm("", centity);

                                //��ǰû���������̣����¼���֤��Ϣ
                                if (!isHavaWorkFlow)
                                {
                                    //��֤������Ϣ
                                    QuestionVerifyEntity aentity = new QuestionVerifyEntity();
                                    aentity.APPSIGN = "Import";
                                    aentity.QUESTIONID = baseentity.ID;
                                    aentity.VERIFYDEPTCODE = entity.verifydeptcode;
                                    aentity.VERIFYDEPTNAME = entity.verifydeptname;
                                    aentity.VERIFYPEOPLE = entity.verifypeople;
                                    aentity.VERIFYPEOPLENAME = entity.verifypeoplename;
                                    aentity.VERIFYDEPTID = entity.verifydeptid;
                                    aentity.VERIFYDATE = entity.verifydate;
                                    questionverifybll.SaveForm("", aentity);
                                }
                                total += 1;
                            }
                            else if (excuteVal == 0)
                            {
                                falseMessage += "�������Ϊ" + entity.questionnumber + "����������������ظ����Զ�����,�������</br>";
                            }
                        }
                        #endregion
                    }
                    #endregion
                    count = dt.Rows.Count;
                    message = "����" + count.ToString() + "����¼,�ɹ�����" + total.ToString() + "��,ʧ��" + (count - total).ToString() + "��" + childmessage;
                    message += "</br>" + falseMessage;
                    message += string.Format("<script type=\"text/javascript\">top.Details.window.reloadGrid(\"{0}\")</script>", string.Join(",", listIds));
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return message;
        }
        #endregion

        #region MyRegion
        /// <summary>
        /// ��ȡ�ļ�
        /// </summary>
        /// <param name="direct"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public List<FileInfo> GetFiles(DirectoryInfo direct, List<FileInfo> files)
        {

            FileInfo[] fileinfoes = direct.GetFiles();
            DirectoryInfo[] directlist = direct.GetDirectories();
            foreach (FileInfo finfo in fileinfoes)
            {
                files.Add(finfo);
            }
            foreach (DirectoryInfo cdirect in directlist)
            {
                GetFiles(cdirect, files);
            }
            return files;
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

        #region ��������������Ϣ
        /// <summary>
        /// ��������������Ϣ
        /// </summary>
        /// <param name="queryJson"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public ActionResult ExportExcel(string queryJson, string fileName, string currentModuleId, string mode = "")
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            queryJson = queryJson.Insert(1, "\"userId\":\"" + curUser.UserId + "\","); //��ӵ�ǰ�û�
            string userId = curUser.UserId;
            string p_fields = string.Empty;
            string p_fieldsName = string.Empty;
            try
            {
                //ϵͳĬ�ϵ��б�����
                var defaultdata = modulelistcolumnauthbll.GetEntity(currentModuleId, "", 0);
                if (null != defaultdata)
                {
                    p_fields = defaultdata.DEFAULTCOLUMNFIELDS;
                    p_fieldsName = defaultdata.DEFAULTCOLUMNNAME;
                }
                //��ǰ�û����б�����
                var data = modulelistcolumnauthbll.GetEntity(currentModuleId, curUser.UserId, 1);
                //Ϊ�գ��Զ���ȡϵͳĬ��
                if (null != data)
                {
                    p_fields = data.DEFAULTCOLUMNFIELDS;
                    p_fieldsName = data.DEFAULTCOLUMNNAME;
                }
                //���������ͽ����滻Ϊ����֮�������
                p_fields = "workstream," + p_fields + ",participantname";
                p_fieldsName = "����״̬," + p_fieldsName + ",���̴�����";

                pagination.p_fields = p_fields + ",hiddentypename";
                //ȡ������Դ
                DataTable exportTable = htbaseinfobll.GetHiddenBaseInfoPageList(pagination, queryJson);
                exportTable.Columns.Remove("id");
                exportTable.Columns["r"].SetOrdinal(0);

                // ��ϸ�б�����
                string fielname = fileName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                wb.Open(Server.MapPath("~/Resource/ExcelTemplate/tmp.xls"));
                Aspose.Cells.Worksheet sheet = wb.Worksheets[0] as Aspose.Cells.Worksheet;

                Aspose.Cells.Cell cell = sheet.Cells[0, 0];
                cell.PutValue("�����Ų������Ϣ"); //����
                cell.Style.Pattern = BackgroundType.Solid;
                cell.Style.Font.Size = 14;
                cell.Style.Font.Color = Color.Black;

                //��̬����
                int colLength = 0;
                if (!string.IsNullOrEmpty(p_fieldsName))
                {
                    //�������������
                    string[] p_filedsNameArray = p_fieldsName.Split(',');
                    colLength = p_filedsNameArray.Length + 1;
                    //�����
                    Aspose.Cells.Cell serialcell = sheet.Cells[1, 0];
                    serialcell.PutValue("���"); //���λ

                    for (int i = 0; i < p_filedsNameArray.Length; i++)
                    {
                        Aspose.Cells.Cell curcell = sheet.Cells[1, i + 1];
                        //��ҵ�汾
                        if (curUser.Industry != "����" && !string.IsNullOrEmpty(curUser.Industry) && p_filedsNameArray[i] == "רҵ����")
                        {
                            p_filedsNameArray[i] = "��������";
                        }
                        curcell.PutValue(p_filedsNameArray[i].ToString()); //��ͷ
                    }
                    //�ϲ���Ԫ��
                    Aspose.Cells.Cells cells = sheet.Cells;
                    cells.Merge(0, 0, 1, p_filedsNameArray.Length);
                }

                //����������User Go Die ���������������� ��GJB
                var typedata = dataitemdetailbll.GetDataItemListByItemCode("'HidType'").Where(p => p.ItemCode == "0").ToList();
                int rowIndex = 2;

                Aspose.Cells.Style style = wb.Styles[wb.Styles.Add()];
                style.ForegroundColor = System.Drawing.Color.FromArgb(153, 204, 0);
                style.Pattern = Aspose.Cells.BackgroundType.Solid;
                style.Font.IsBold = true;
                //�ֶ�����
                string[] p_fieldsarrayObj = ("r," + p_fields).Split(',');
                //������
                foreach (DataItemModel model in typedata)
                {
                    string hidtypename = model.ItemName;
                    Aspose.Cells.Cell typecell = sheet.Cells[rowIndex, 0];
                    typecell.Style.HorizontalAlignment = TextAlignmentType.Left; //��������ƫ��
                    typecell.PutValue(hidtypename); //�������
                    Aspose.Cells.Cells mcells = sheet.Cells;
                    mcells.Merge(rowIndex, 0, 1, colLength);
                    sheet.Cells[rowIndex, 0].Style = style;

                    DataRow[] hidRows = exportTable.Select(string.Format(" hiddentypename='{0}'", hidtypename));
                    int hidtypeRowIndex = 1;
                    foreach (DataRow row in hidRows)
                    {
                        rowIndex += 1;
                        row["r"] = hidtypeRowIndex;
                        hidtypeRowIndex++;
                        for (int i = 0; i < colLength; i++)
                        {
                            if (p_fieldsarrayObj[i] == "hidbasefilepath")
                            {
                                string lllegalfilepath = row[i].ToString();
                                if (!string.IsNullOrEmpty(lllegalfilepath))
                                {
                                    string imageUrl = System.Web.HttpContext.Current.Server.MapPath(lllegalfilepath);
                                    if (System.IO.File.Exists(imageUrl))
                                    {
                                        sheet.Pictures.Add(rowIndex, i, rowIndex + 1, i + 1, imageUrl);
                                    }
                                }
                            }
                            else if (p_fieldsarrayObj[i] == "reformfilepath")
                            {
                                string reformfilepath = row[i].ToString();
                                if (!string.IsNullOrEmpty(reformfilepath))
                                {
                                    string imageUrl = System.Web.HttpContext.Current.Server.MapPath(reformfilepath);
                                    if (System.IO.File.Exists(imageUrl))
                                    {
                                        sheet.Pictures.Add(rowIndex, i, rowIndex + 1, i + 1, imageUrl);
                                    }
                                }
                            }
                            else
                            {
                                if (p_fieldsarrayObj[i] != "hiddentypename")
                                {
                                    Aspose.Cells.Cell colrowcell = sheet.Cells[rowIndex, i];
                                    colrowcell.PutValue(row[i].ToString()); //�������ֵ
                                }

                            }
                        }
                    }
                    rowIndex += 1;
                }

                if (!string.IsNullOrEmpty(mode))
                {
                    string tempSavePath = Server.MapPath("~/Resource/Temp/") + fielname;
                    wb.Save(tempSavePath);
                    string url = "../../Utility/DownloadFile?filePath=~/Resource/Temp/" + fielname + "&speed=10240000&newFileName=" + fielname;
                    return Redirect(url);
                }
                else
                {
                    HttpResponse resp = System.Web.HttpContext.Current.Response;
                    resp.Clear();
                    resp.Buffer = true;
                    wb.Save(Server.UrlEncode(fielname), Aspose.Cells.FileFormatType.Excel2003, Aspose.Cells.SaveType.OpenInBrowser, resp);
                }
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
            return Success("�����ɹ�!");
        }
        #endregion

        #region ����һ����
        /// <summary>
        /// ����һ����
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportList(string queryJson, string mode = "")
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;

            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string userId = curUser.UserId;
            curUser.isPlanLevel = "0";
            if (curUser.RoleName.Contains("��˾��") || curUser.RoleName.Contains("����")) { curUser.isPlanLevel = "1"; }
            queryJson = queryJson.Insert(1, "\"userId\":\"" + userId + "\","); //��ӵ�ǰ�û�
            queryJson = queryJson.Insert(1, "\"isPlanLevel\":\"" + curUser.isPlanLevel + "\","); //��ӵ�ǰ�Ƿ�˾������
            try
            {
                DesktopBLL dbll = new DesktopBLL();
                if (dbll.IsGeneric())
                {
                    //������ǵ糧�򲻵���רҵ����ͳ��������Ա
                    pagination.p_fields =
                        @"hidcode,hiddescribe,hidrankname,changedeadine, 
                  (case when workstream ='��������' or workstream ='����Ч������' or workstream ='���Ľ���' then '��' else '��' end) isclose,'' curstatus";
                }
                else
                {
                    pagination.p_fields =
                        @"hidcode,hiddescribe,hidrankname,majorclassifyname,monitorpersonname,changedeadine, 
                  (case when workstream ='��������' or workstream ='����Ч������' or workstream ='���Ľ���' then '��' else '��' end) isclose,'' curstatus";
                }

                //ȡ������Դ
                DataTable exportTable = htbaseinfobll.GetHiddenBaseInfoPageList(pagination, queryJson);
                exportTable.Columns.Remove("curapprovedate");
                exportTable.Columns.Remove("curacceptdate");
                exportTable.Columns.Remove("beforeapprovedate");
                exportTable.Columns.Remove("beforeacceptdate");
                exportTable.Columns.Remove("afterapprovedate");
                exportTable.Columns.Remove("afteracceptdate");
                exportTable.Columns.Remove("id");
                exportTable.Columns["r"].SetOrdinal(0);
                // ȷ�������ļ���
                string fileName = "�¹������Ų��������һ����";

                // ��ϸ�б�����
                string fielname = fileName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                wb.Open(Server.MapPath("~/Resource/ExcelTemplate/�¹������Ų��������һ����.xlsx"));
                Aspose.Cells.Worksheet sheet = wb.Worksheets[0] as Aspose.Cells.Worksheet;
                Aspose.Cells.Cell cell = sheet.Cells[1, 1];

                cell.PutValue(curUser.DeptName); //���λ
                Aspose.Cells.Cell endcell = sheet.Cells[1, 8];
                endcell.PutValue(DateTime.Now.ToString("yyyy-MM-dd")); //���λ


                int result = dataitemdetailbll.GetDataItemListByItemCode("'IsEnableMinimalistMode'").Where(p => p.ItemValue == curUser.OrganizeId).Count();
                if (result > 0)
                {
                    Aspose.Cells.Cell describeCell = sheet.Cells[2, 2]; //
                    describeCell.PutValue("��������"); //��������
                }
                string JLIndex = dataitemdetailbll.GetItemValue("JLIndex"); //���罭�������
                if (!string.IsNullOrEmpty(JLIndex))
                {
                    Aspose.Cells.Cell describeCell = sheet.Cells[2, 2]; //
                    describeCell.PutValue("��������"); //��������
                }
                sheet.Cells.ImportDataTable(exportTable, false, 3, 0);


                if (!string.IsNullOrEmpty(mode))
                {
                    string tempSavePath = Server.MapPath("~/Resource/Temp/") + fielname;
                    wb.Save(tempSavePath);
                    string url = "../../Utility/DownloadFile?filePath=~/Resource/Temp/" + fielname + "&speed=10240000&newFileName=" + fielname;
                    return Redirect(url);
                }
                else
                {
                    HttpResponse resp = System.Web.HttpContext.Current.Response;
                    resp.Clear();
                    resp.Buffer = true;
                    wb.Save(Server.UrlEncode(fielname), Aspose.Cells.FileFormatType.Excel2003, Aspose.Cells.SaveType.OpenInBrowser, resp);
                }
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
            return Success("�����ɹ�!");
        }

        #endregion

        #region ������������̨�˱�
        /// <summary>
        /// ������������̨�˱�
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult ExportGovern(string queryJson, string mode = "")
        {

            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;

            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string userId = curUser.UserId;
            curUser.isPlanLevel = "0";
            if (curUser.RoleName.Contains("��˾��") || curUser.RoleName.Contains("����")) { curUser.isPlanLevel = "1"; }
            queryJson = queryJson.Insert(1, "\"userId\":\"" + userId + "\","); //��ӵ�ǰ�û�
            queryJson = queryJson.Insert(1, "\"isPlanLevel\":\"" + curUser.isPlanLevel + "\","); //��ӵ�ǰ�Ƿ�˾������
            DesktopBLL dbll = new DesktopBLL();
            try
            {
                string p_fields = string.Empty;
                pagination.p_fields = @" (hidpointname||'/'||hidplace) checkarea,checkdate,hiddescribe,hidrankname,checkmanname,changemeasure,planmanagecapital,changedeadine,changedutydepartname,changepersonname,changeresult,acceptpersonname,acceptdate,hidbasefilepath,reformfilepath";

                //ȡ������Դ
                DataTable exportTable = htbaseinfobll.GetHiddenBaseInfoPageList(pagination, queryJson);
                exportTable.Columns.Remove("curapprovedate");
                exportTable.Columns.Remove("curacceptdate");
                exportTable.Columns.Remove("beforeapprovedate");
                exportTable.Columns.Remove("beforeacceptdate");
                exportTable.Columns.Remove("afterapprovedate");
                exportTable.Columns.Remove("afteracceptdate");
                exportTable.Columns.Remove("id");
                // ȷ�������ļ���
                string fileName = "�����Ų�����̨�˱�";
                string zgdeptname = string.Empty;
                foreach (DataRow row in exportTable.Rows)
                {
                    string rowdept = row["changedutydepartname"].ToString();
                    if (!string.IsNullOrEmpty(rowdept) && !zgdeptname.Contains(rowdept))
                    {
                        zgdeptname += rowdept.ToString() + ",";
                    }
                }
                if (!string.IsNullOrEmpty(zgdeptname))
                {
                    zgdeptname = zgdeptname.Substring(0, zgdeptname.Length - 1);
                }
                if (zgdeptname.Split(',').Length > 1)
                {
                    zgdeptname = string.Empty;
                }
                Response.Clear();
                // ��ϸ�б�����
                string fielname = fileName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                wb.Open(Server.MapPath("~/Resource/ExcelTemplate/�����Ų�����̨��ģ��.xls"));
                Aspose.Cells.Worksheet sheet = wb.Worksheets[0] as Aspose.Cells.Worksheet;
                Aspose.Cells.Cell cell = sheet.Cells[1, 1];
                cell.PutValue(zgdeptname); //���λ

                int colLength = 15;
                int rowIndex = 4;
                int serialNumber = 1;
                foreach (DataRow row in exportTable.Rows)
                {
                    Aspose.Cells.Cell fcell = sheet.Cells[rowIndex, 0];
                    fcell.PutValue(serialNumber.ToString()); //�������ֵ

                    for (int i = 0; i < colLength; i++)
                    {
                        if (i == 13)
                        {
                            string lllegalfilepath = row[i].ToString();
                            if (!string.IsNullOrEmpty(lllegalfilepath))
                            {
                                string imageUrl = System.Web.HttpContext.Current.Server.MapPath(lllegalfilepath);
                                if (System.IO.File.Exists(imageUrl))
                                {
                                    sheet.Pictures.Add(rowIndex, i + 1, rowIndex + 1, i + 2, imageUrl);
                                }
                            }
                        }
                        else if (i == 14)
                        {
                            string reformfilepath = row[i].ToString();
                            if (!string.IsNullOrEmpty(reformfilepath))
                            {
                                string imageUrl = System.Web.HttpContext.Current.Server.MapPath(reformfilepath);
                                if (System.IO.File.Exists(imageUrl))
                                {
                                    sheet.Pictures.Add(rowIndex, i + 1, rowIndex + 1, i + 2, imageUrl);
                                }
                            }
                        }
                        else
                        {
                            Aspose.Cells.Cell colrowcell = sheet.Cells[rowIndex, i + 1];
                            colrowcell.PutValue(row[i].ToString()); //�������ֵ
                        }
                    }

                    serialNumber++;
                    rowIndex++;
                }

                if (!string.IsNullOrEmpty(mode))
                {
                    string tempSavePath = Server.MapPath("~/Resource/Temp/") + fielname;
                    wb.Save(tempSavePath);
                    string url = "../../Utility/DownloadFile?filePath=~/Resource/Temp/" + fielname + "&speed=10240000&newFileName=" + fielname;
                    return Redirect(url);
                }
                else
                {
                    HttpResponse resp = System.Web.HttpContext.Current.Response;
                    resp.Clear();
                    resp.Buffer = true;
                    wb.Save(Server.UrlEncode(fielname), Aspose.Cells.FileFormatType.Excel2003, Aspose.Cells.SaveType.OpenInBrowser, resp);
                }
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
            return Success("�����ɹ�!");
        }
        #endregion

        #region �ش�������Ϣ���浥
        /// <summary>
        /// �ش�������Ϣ���浥
        /// </summary>
        /// <param name="queryJson"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public ActionResult ExportImportant(string keyValue)
        {
            //����������Ϣ
            var baseInfo = htbaseinfobll.GetEntity(keyValue);
            //����������Ϣ
            var approvalInfo = htapprovalbll.GetEntityByHidCode(baseInfo.HIDCODE);
            //����������Ϣ
            var changeInfo = htchangeinfobll.GetEntityByHidCode(baseInfo.HIDCODE);

            var rankitem = dataitemdetailbll.GetEntity(baseInfo.HIDRANK);

            string approvaldate = approvalInfo != null ? approvalInfo.APPROVALDATE.ToString() : "";

            var userInfo = OperatorProvider.Provider.Current();  //��ȡ��ǰ�û�

            string fileName = "�ش�������Ϣ���浥_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";

            string strDocPath = Server.MapPath("~/Resource/ExcelTemplate/�ش�������Ϣ���浥.docx");

            Aspose.Words.Document doc = new Aspose.Words.Document(strDocPath);

            DataTable dt = new DataTable();
            dt.Columns.Add("createdeptname");
            dt.Columns.Add("createdate");
            dt.Columns.Add("hidname");
            dt.Columns.Add("hidrank");
            dt.Columns.Add("hiddeptname");
            dt.Columns.Add("approvaldate");
            dt.Columns.Add("hidleader");
            dt.Columns.Add("hidleadertel");
            dt.Columns.Add("hidprincipal");
            dt.Columns.Add("hidprincipaltel");
            dt.Columns.Add("hidstatus");
            dt.Columns.Add("hidreason");
            dt.Columns.Add("hiddangerlevel");
            dt.Columns.Add("preventmeasure");
            dt.Columns.Add("changemeasure");
            dt.Columns.Add("hidchageplan");
            dt.Columns.Add("exigenceresume");
            HttpResponse resp = System.Web.HttpContext.Current.Response;

            DataRow row = dt.NewRow();
            row["createdeptname"] = userInfo.DeptName;
            row["createdate"] = DateTime.Now.ToString("yyyy-MM-dd");
            row["hidname"] = baseInfo.HIDNAME;
            row["hidrank"] = null != rankitem ? rankitem.ItemName : "";
            row["hiddeptname"] = userInfo.OrganizeName;
            row["approvaldate"] = !string.IsNullOrEmpty(approvaldate) ? Convert.ToDateTime(approvaldate).ToString("yyyy-MM-dd") : "";
            row["hidleader"] = "";
            row["hidleadertel"] = "";
            row["hidprincipal"] = changeInfo.CHANGEPERSONNAME;
            row["hidprincipaltel"] = changeInfo.CHANGEDUTYTEL;
            row["hidstatus"] = baseInfo.HIDSTATUS;
            row["hidreason"] = baseInfo.HIDREASON;
            row["hiddangerlevel"] = baseInfo.HIDDANGERLEVEL;
            row["preventmeasure"] = baseInfo.PREVENTMEASURE;
            row["changemeasure"] = changeInfo.CHANGEMEASURE;
            row["hidchageplan"] = baseInfo.HIDCHAGEPLAN;
            row["exigenceresume"] = baseInfo.EXIGENCERESUME;
            dt.Rows.Add(row);

            doc.MailMerge.Execute(dt);
            doc.MailMerge.DeleteFields();
            doc.Save(resp, Server.UrlEncode(fileName), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));

            return Success("�����ɹ�!");
        }
        #endregion

        #region �¹������Ų���������
        /// <summary>
        /// �¹������Ų���������
        /// </summary>
        /// <param name="queryJson"></param>
        /// <param name="fileName"></param> 
        /// <returns></returns>
        public ActionResult ExportRecordInfo(string keyValue)
        {
            //����������Ϣ
            var baseInfo = htbaseinfobll.GetEntity(keyValue);
            //������Ϣ
            var approvalInfo = htapprovalbll.GetEntityByHidCode(baseInfo.HIDCODE);
            //����������Ϣ
            var changeInfo = htchangeinfobll.GetEntityByHidCode(baseInfo.HIDCODE);
            //����������Ϣ
            var acceptInfo = htacceptinfobll.GetEntityByHidCode(baseInfo.HIDCODE);
            //��������Ч������
            var estimateInfo = htestimatebll.GetEntityByHidCode(baseInfo.HIDCODE);

            var hidtypekitem = dataitemdetailbll.GetEntity(baseInfo.HIDTYPE);

            var hidrankitem = dataitemdetailbll.GetEntity(baseInfo.HIDRANK);

            string changeresume = string.Empty;

            if (baseInfo.WORKSTREAM == "��������" || baseInfo.WORKSTREAM == "����Ч������" || baseInfo.WORKSTREAM == "���Ľ���")
            {
                changeresume = "������";
            }
            else
            {
                changeresume = "δ����";
            }

            //����ʱ��
            string approvaldate = approvalInfo != null ? approvalInfo.APPROVALDATE.ToString() : "";

            var userInfo = OperatorProvider.Provider.Current();  //��ȡ��ǰ�û�

            string fileName = "�¹������Ų���������_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";

            string strDocPath = Server.MapPath("~/Resource/ExcelTemplate/�¹������Ų���������.docx");
            DesktopBLL dbll = new DesktopBLL();
            if (dbll.IsGeneric())
            {
                strDocPath = Server.MapPath("~/Resource/ExcelTemplate/ͨ���¹������Ų���������.docx");
            }

            Aspose.Words.Document doc = new Aspose.Words.Document(strDocPath);

            DataTable dt = new DataTable();
            dt.Columns.Add("year");
            dt.Columns.Add("deptname");
            dt.Columns.Add("hiddescribe");
            dt.Columns.Add("hidtype");
            dt.Columns.Add("checkmanname");
            dt.Columns.Add("checkdate");
            dt.Columns.Add("hidcode");
            dt.Columns.Add("hidconsequence");
            dt.Columns.Add("hidrank");
            dt.Columns.Add("hidapproval");
            dt.Columns.Add("changedutydepartname");
            dt.Columns.Add("changepersonname");
            dt.Columns.Add("startyear");
            dt.Columns.Add("endyear");
            dt.Columns.Add("preventmeasure");
            dt.Columns.Add("changeresume");
            dt.Columns.Add("realitymanagecapital");
            dt.Columns.Add("changedepartname");
            dt.Columns.Add("changepeoplename");
            dt.Columns.Add("changefinishdate");
            dt.Columns.Add("acceptdepartname");
            dt.Columns.Add("acceptidea");
            dt.Columns.Add("acceptstatus");
            dt.Columns.Add("acceptpersonname");
            dt.Columns.Add("acceptdate");
            dt.Columns.Add("describetitle");

            HttpResponse resp = System.Web.HttpContext.Current.Response;

            DocumentBuilder builder = new DocumentBuilder(doc);
            IEnumerable<FileInfoEntity> hidphotofile = fileinfobll.GetImageListByObject(baseInfo.HIDPHOTO);
            foreach (FileInfoEntity fentity in hidphotofile)
            {
                string url = AppDomain.CurrentDomain.BaseDirectory + fentity.FilePath.Substring(1).Replace("~/", "");
                if (System.IO.File.Exists(url))
                {
                    builder.MoveToMergeField("hidphoto"); //builder.CellFormat.Width
                    builder.InsertImage(url, Aspose.Words.Drawing.RelativeHorizontalPosition.Margin, 0, Aspose.Words.Drawing.RelativeVerticalPosition.Margin, 0, 200, 150, Aspose.Words.Drawing.WrapType.Inline);
                }
            }
            IEnumerable<FileInfoEntity> changephotofile = fileinfobll.GetImageListByObject(changeInfo.HIDCHANGEPHOTO);
            foreach (FileInfoEntity fentity in changephotofile)
            {
                string url = AppDomain.CurrentDomain.BaseDirectory + fentity.FilePath.Substring(1).Replace("~/", "");
                if (System.IO.File.Exists(url))
                {
                    builder.MoveToMergeField("changephoto");
                    builder.InsertImage(url, Aspose.Words.Drawing.RelativeHorizontalPosition.Margin, 0, Aspose.Words.Drawing.RelativeVerticalPosition.Margin, 0, 200, 150, Aspose.Words.Drawing.WrapType.Inline);
                }
            }

            DataRow row = dt.NewRow();
            row["year"] = DateTime.Now.ToString("yyyy");
            row["deptname"] = userInfo.DeptName;
            row["hiddescribe"] = baseInfo.HIDDESCRIBE; //�¹���������(����)
            row["hidtype"] = null != hidtypekitem ? hidtypekitem.ItemName : "";//�������
            row["checkmanname"] = baseInfo.CHECKMANNAME; //����������(�Ų���)
            row["checkdate"] = null != baseInfo.CHECKDATE ? baseInfo.CHECKDATE.Value.ToString("yyyy-MM-dd") : "";//��������
            row["hidcode"] = baseInfo.HIDCODE;
            row["hidconsequence"] = baseInfo.HIDCONSEQUENCE;
            row["hidrank"] = null != hidrankitem ? hidrankitem.ItemName : "";//��������
            row["hidapproval"] = null != approvalInfo ? approvalInfo.APPROVALPERSONNAME : "";//����������
            row["changedutydepartname"] = null != changeInfo ? changeInfo.CHANGEDUTYDEPARTNAME : ""; //���Ĳ���
            row["changepersonname"] = null != changeInfo ? changeInfo.CHANGEPERSONNAME : ""; //������
            row["startyear"] = !string.IsNullOrEmpty(approvaldate) ? Convert.ToDateTime(approvaldate).ToString("yyyy-MM-dd") : ""; //����ʱ��
            row["endyear"] = null != changeInfo ? (null != changeInfo.CHANGEDEADINE ? changeInfo.CHANGEDEADINE.Value.ToString("yyyy-MM-dd") : "") : "";//����ʱ��
            row["preventmeasure"] = baseInfo.PREVENTMEASURE;

            row["changeresume"] = changeresume; //����������
            row["realitymanagecapital"] = null != changeInfo ? (null != changeInfo.REALITYMANAGECAPITAL ? Math.Round(changeInfo.REALITYMANAGECAPITAL.Value / 10000, 4).ToString() : "0") : "0";
            row["changedepartname"] = null != changeInfo ? changeInfo.CHANGEDUTYDEPARTNAME : "";
            row["changepeoplename"] = null != changeInfo ? changeInfo.CHANGEPERSONNAME : "";
            row["changefinishdate"] = null != changeInfo ? (null != changeInfo.CHANGEFINISHDATE ? changeInfo.CHANGEFINISHDATE.Value.ToString("yyyy-MM-dd") : "") : "";
            row["acceptdepartname"] = null != acceptInfo ? acceptInfo.ACCEPTDEPARTNAME : "";
            row["acceptidea"] = null != acceptInfo ? acceptInfo.ACCEPTIDEA : "";
            row["describetitle"] = "�¹���������(����)";
            int result = dataitemdetailbll.GetDataItemListByItemCode("'IsEnableMinimalistMode'").Where(p => p.ItemValue == userInfo.OrganizeId).Count();
            if (result > 0)
            {
                row["describetitle"] = "��������";
            }

            if (null != acceptInfo)
            {
                if (acceptInfo.ACCEPTSTATUS == "1")
                {
                    row["acceptstatus"] = "����ͨ��";
                }
                else if (acceptInfo.ACCEPTSTATUS == "0")
                {
                    row["acceptstatus"] = "���ղ�ͨ��";
                }
                else
                {
                    row["acceptstatus"] = "";
                }
            }
            else
            {
                row["acceptstatus"] = "";
            }

            row["acceptpersonname"] = null != acceptInfo ? acceptInfo.ACCEPTPERSONNAME : "";
            row["acceptdate"] = null != acceptInfo ? (null != acceptInfo.ACCEPTDATE ? acceptInfo.ACCEPTDATE.Value.ToString("yyyy-MM-dd") : "") : "";
            dt.Rows.Add(row);

            doc.MailMerge.Execute(dt);
            doc.MailMerge.DeleteFields();
            doc.Save(resp, Server.UrlEncode(fileName), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));

            return Success("�����ɹ�!");
        }
        #endregion

        #region ������ȫ�����Ų���������±���
        /// <summary>
        /// ������ȫ�����Ų���������±���
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportSituation(string mode = "")
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string deptcode = curUser.OrganizeCode;
            string curdate = DateTime.Now.ToString("yyyy");
            try
            {
                DataTable dt = htbaseinfobll.GetHiddenSituationOfMonth(deptcode, curdate, curUser); //��ǰ����µ�����ͳ���±���

                decimal yjtotal = 0; //һ���ش�
                decimal yjzgtotal = 0; //������һ���ش�
                decimal yjzgl = 0; //һ�������� 
                decimal ejtotal = 0; //�����ش�
                decimal ejzgtotal = 0; //�����Ķ����ش�
                decimal ejzgl = 0; //���������� 
                decimal ybtotal = 0; //һ��
                decimal ybzgtotal = 0; //������һ��
                decimal ybzgl = 0; //һ�������� 
                decimal money = 0;  //�ʽ��ܶ�
                foreach (DataRow row in dt.Rows)
                {
                    yjtotal += Convert.ToDecimal(row["yjhid"].ToString());
                    yjzgtotal += Convert.ToDecimal(row["zgyjhid"].ToString());
                    ejtotal += Convert.ToDecimal(row["ejhid"].ToString());
                    ejzgtotal += Convert.ToDecimal(row["zgejhid"].ToString());
                    ybtotal += Convert.ToDecimal(row["ybhid"].ToString());
                    ybzgtotal += Convert.ToDecimal(row["zgybhid"].ToString());
                    money += Convert.ToDecimal(row["money"].ToString());
                }
                yjzgl = yjtotal > 0 ? Math.Round(yjzgtotal / yjtotal * 100, 2) : 0; //һ������������
                ejzgl = ejtotal > 0 ? Math.Round(ejzgtotal / ejtotal * 100, 2) : 0; //��������������
                ybzgl = ybtotal > 0 ? Math.Round(ybzgtotal / ybtotal * 100, 2) : 0; //һ������������

                DesktopBLL dbll = new DesktopBLL();
                // ȷ�������ļ���
                string fileName = "������ȫ�����Ų���������±���";
                string fileUrl = "~/Resource/ExcelTemplate/������ȫ�����Ų���������±���.xlsx";
                if (dbll.IsGeneric())
                {
                    fileName = "��ȫ�����Ų���������±���";
                    fileUrl = "~/Resource/ExcelTemplate/��ȫ�����Ų���������±���.xlsx";
                }


                //��ϸ�б�����
                string fielname = fileName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                wb.Open(Server.MapPath(fileUrl));
                Aspose.Cells.Worksheet sheet = wb.Worksheets[0] as Aspose.Cells.Worksheet;
                Aspose.Cells.Cells cells = sheet.Cells;
                Aspose.Cells.Cell cell = sheet.Cells[1, 1];
                cell.PutValue(curUser.DeptName); //���λ
                Aspose.Cells.Cell endcell = sheet.Cells[1, 13];
                endcell.PutValue(DateTime.Now.ToString("yyyy-MM-dd")); //���λ

                //�����ܶ���
                sheet.Cells[7, 4].PutValue(yjtotal.ToString()); //һ������
                sheet.Cells[7, 5].PutValue(yjzgtotal.ToString()); //һ������������
                sheet.Cells[7, 6].PutValue(yjzgl.ToString()); //һ��������
                sheet.Cells[7, 7].PutValue(ejtotal.ToString()); //��������
                sheet.Cells[7, 8].PutValue(ejzgtotal.ToString()); //��������������
                sheet.Cells[7, 9].PutValue(ejzgl.ToString()); //����������
                sheet.Cells[7, 10].PutValue(ybtotal.ToString()); //һ������
                sheet.Cells[7, 11].PutValue(ybzgtotal.ToString()); //һ������������
                sheet.Cells[7, 12].PutValue(ybzgl.ToString()); //һ��������
                sheet.Cells[7, 13].PutValue(money.ToString()); //һ��������

                sheet.Cells.ImportDataTable(dt, false, 8, 0);
                int lastRow = 8 + dt.Rows.Count;
                //��һ������ݻ���
                curdate = DateTime.Now.AddYears(-1).ToString("yyyy");
                DataTable prevdt = htbaseinfobll.GetHiddenSituationOfMonth(deptcode, curdate, curUser); //��ǰ����µ�����ͳ���±��� 
                decimal yjwzg = 0;
                decimal ejwzg = 0;
                decimal ybwzg = 0;
                decimal lsmoney = 0;
                foreach (DataRow row in prevdt.Rows)
                {
                    yjwzg += Convert.ToDecimal(row["yjhid"].ToString()) - Convert.ToDecimal(row["zgyjhid"].ToString());
                    ejwzg += Convert.ToDecimal(row["ejhid"].ToString()) - Convert.ToDecimal(row["zgejhid"].ToString());
                    ybwzg += Convert.ToDecimal(row["ybhid"].ToString()) - Convert.ToDecimal(row["zgybhid"].ToString());
                    lsmoney += Convert.ToDecimal(row["money"].ToString());
                }
                sheet.Cells[lastRow, 4].PutValue(yjwzg.ToString());//��һ���һ��δ����
                sheet.Cells[lastRow, 7].PutValue(ejwzg.ToString());//��һ��ȶ���δ����
                sheet.Cells[lastRow, 10].PutValue(ybwzg.ToString()); //��һ���һ��δ����
                sheet.Cells[lastRow, 13].PutValue(lsmoney.ToString()); //��һ����ʽ��ܶ�

                cells.Merge(lastRow, 4, 1, 3);  //������д��һ��ȵ�һ��δ���������ܺ�
                cells.Merge(lastRow, 7, 1, 3);  //������д��һ��ȵĶ���δ���������ܺ�
                cells.Merge(lastRow, 10, 1, 3); //������д��һ��ȵ�һ��δ���������ܺ�

                if (!string.IsNullOrEmpty(mode))
                {
                    string tempSavePath = Server.MapPath("~/Resource/Temp/") + fielname;
                    wb.Save(tempSavePath);
                    string url = "../../Utility/DownloadFile?filePath=~/Resource/Temp/" + fielname + "&speed=10240000&newFileName=" + fielname;
                    return Redirect(url);
                }
                else
                {
                    HttpResponse resp = System.Web.HttpContext.Current.Response;
                    resp.Clear();
                    resp.Buffer = true;
                    wb.Save(Server.UrlEncode(fielname), Aspose.Cells.FileFormatType.Excel2003, Aspose.Cells.SaveType.OpenInBrowser, resp);
                }
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

            return Success("�����ɹ�!");
        }
        #endregion

        #region ��ȡ��ҳ����top n������ͳ������

        /// <summary>
        /// ��ȡ��ҳ����top n������ͳ������
        /// </summary>
        /// <param name="qtype"></param>
        /// <param name="topnum"></param>
        /// <returns></returns>
        public ActionResult GetHomePageHiddenByHidType(int qtype, int topnum)
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var dt = htbaseinfobll.GetHomePageHiddenByHidType(curUser, DateTime.Now.Year, topnum, qtype);
            List<TroubleStatists> result = new List<TroubleStatists>();
            foreach (DataRow row in dt.Rows)
            {
                TroubleStatists entity = new TroubleStatists();
                entity.numbers = int.Parse(row["numbers"].ToString());
                entity.encode = row["encode"].ToString();
                entity.fullname = row["fullname"].ToString();
                List<TroubleStatistsDetail> list = new List<TroubleStatistsDetail>();
                var detailDt = htbaseinfobll.GetHomePageHiddenByDepart(curUser.OrganizeId, entity.encode, DateTime.Now.Year.ToString(), qtype);
                foreach (DataRow drow in detailDt.Rows)
                {
                    TroubleStatistsDetail dentity = new TroubleStatistsDetail();
                    dentity.numbers = int.Parse(drow["numbers"].ToString());
                    dentity.encode = drow["encode"].ToString();
                    dentity.fullname = drow["fullname"].ToString();
                    list.Add(dentity);
                }
                entity.detail = list;
                result.Add(entity);
            }
            return Content(result.ToJson());
        }
        #endregion

        #region ������������
        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveExpirationForm(string keyValue, ExpirationTimeSettingEntity entity)
        {
            try
            {
                Operator curUser = OperatorProvider.Provider.Current();
                var list = htbaseinfobll.GetExpList(curUser.OrganizeId, entity.MODULENAME);
                if (list.Count() > 0)
                {
                    keyValue = list.FirstOrDefault().ID;
                }
                entity.ORGANIZEID = curUser.OrganizeId;
                entity.ORGANIZENAME = curUser.OrganizeName;
                htbaseinfobll.SaveExpirationTimeEntity(keyValue, entity);

                return Success("�����ɹ�!");
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region ��ȡ������������
        /// <summary>
        /// ��ȡ������������
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="modulename"></param>
        /// <returns></returns>
        public ActionResult GetExpirationList(string modulename)
        {
            try
            {
                Operator curUser = OperatorProvider.Provider.Current();
                var data = htbaseinfobll.GetExpList(curUser.OrganizeId, modulename).FirstOrDefault();
                return ToJsonResult(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region ��ȡ���ļƻ�����
        /// <summary>
        /// ��ȡ���ļƻ�����
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="modulename"></param>
        /// <returns></returns>
        public ActionResult GetChangePlanEntity(string keyValue)
        {
            try
            {
                var data = htbaseinfobll.GetChangePlanEntity(keyValue);
                return ToJsonResult(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region �������ļƻ�
        /// <summary>
        /// �������ļƻ�
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveChangePlan(string keyValue, ChangePlanDetailEntity entity)
        {
            try
            {
                htbaseinfobll.SaveChangePlan(keyValue, entity);
                return Success("�����ɹ���");
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion



        #endregion
    }

}

