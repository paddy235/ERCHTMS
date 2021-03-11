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
    /// 描 述：隐患基本信息表
    /// </summary>
    public class HTBaseInfoController : MvcControllerBase
    {
        private HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL(); //隐患基本信息
        private HTChangeInfoBLL htchangeinfobll = new HTChangeInfoBLL(); //隐患整改信息
        private HTApprovalBLL htapprovalbll = new HTApprovalBLL(); //隐患评估信息
        private HTAcceptInfoBLL htacceptinfobll = new HTAcceptInfoBLL(); //隐患验收信息
        private HtReCheckBLL htrecheckbll = new HtReCheckBLL(); //隐患复查验证
        private HTEstimateBLL htestimatebll = new HTEstimateBLL(); //整改效果评估信息
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL(); //隐患流程
        private UserBLL userbll = new UserBLL(); //用户操作对象
        private SaftyCheckDataRecordBLL saftycheckdatarecordbll = new SaftyCheckDataRecordBLL();

        private DistrictBLL districtbll = new DistrictBLL(); //区域管理
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private OrganizeCache organizeCache = new OrganizeCache();
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private FileInfoBLL fileinfobll = new FileInfoBLL();

        private WfControlBLL wfcontrolbll = new WfControlBLL();//自动化流程服务
        private ModuleListColumnAuthBLL modulelistcolumnauthbll = new ModuleListColumnAuthBLL();

        private LllegalRegisterBLL lllegalregisterbll = new LllegalRegisterBLL(); // 违章基本信息
        private LllegalPunishBLL lllegalpunishbll = new LllegalPunishBLL(); // 考核信息对象
        private LllegalAwardDetailBLL lllegalawarddetailbll = new LllegalAwardDetailBLL(); //违章奖励信息
        private LllegalReformBLL lllegalreformbll = new LllegalReformBLL(); //整改信息对象
        private LllegalAcceptBLL lllegalacceptbll = new LllegalAcceptBLL(); //验收信息对象

        private QuestionInfoBLL questioninfobll = new QuestionInfoBLL();
        private QuestionReformBLL questionreformbll = new QuestionReformBLL();
        private QuestionVerifyBLL questionverifybll = new QuestionVerifyBLL();

        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            Operator curUser = OperatorProvider.Provider.Current();

            string actionName = string.Empty;

            string GDXJ_HYC_ORGCODE = dataitemdetailbll.GetItemValue("GDXJ_HYC_ORGCODE");
            //国电新疆红雁池专用
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
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HYCForm()
        {
            return View();
        }

        /// <summary>
        /// 表单页面
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

        //隐患分类分级
        [HttpGet]
        public ActionResult WarningDetail()
        {
            return View();
        }

        /// <summary>
        /// 获取曝光列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExposureIndex()
        {
            return View();
        }

        /// <summary>
        /// 导入页面
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
        /// 期限设置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExpirationForm()
        {
            return View();
        }


        /// <summary>
        /// 流程说明
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FlowForm()
        {
            return View();
        }

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {

            var watch = CommonHelper.TimerStart();
            Operator opertator = new OperatorProvider().Current();
            string userId = opertator.UserId;
            queryJson = queryJson.Insert(1, "\"userId\":\"" + userId + "\","); //添加当前用户
            opertator.isPlanLevel = "0";
            if (opertator.RoleName.Contains("公司级") || opertator.RoleName.Contains("厂级")) { opertator.isPlanLevel = "1"; }
            queryJson = queryJson.Insert(1, "\"isPlanLevel\":\"" + opertator.isPlanLevel + "\","); //添加当前是否公司及厂级
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

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
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

        #region 获取实体
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            //隐患基本信息
            var baseInfo = htbaseinfobll.GetEntity(keyValue);

            var changeInfo = htchangeinfobll.GetEntityByHidCode(baseInfo.HIDCODE);

            var acceptInfo = htacceptinfobll.GetEntityByHidCode(baseInfo.HIDCODE);

            var recheckInfo = htrecheckbll.GetEntityByHidCode(baseInfo.HIDCODE);

            var estimateInfo = htestimatebll.GetEntityByHidCode(baseInfo.HIDCODE);

            var userInfo = OperatorProvider.Provider.Current();  //获取当前用户

            int isReformBack = 0;

            var historyacceptList = htacceptinfobll.GetHistoryList(baseInfo.HIDCODE).ToList();
            if (historyacceptList.Count() > 0)
            {
                isReformBack = 1;
            }
            //厂级
            if (userInfo.RoleName.Contains("公司级") || userInfo.RoleName.Contains("厂级"))
            {
                userInfo.DeptName = userInfo.OrganizeName;
                userInfo.DeptCode = userInfo.OrganizeCode;
            }

            var data = new { baseInfo = baseInfo, changeInfo = changeInfo, acceptInfo = acceptInfo, userInfo = userInfo, estimateInfo = estimateInfo, isreformback = isReformBack, recheck = recheckInfo };

            return ToJsonResult(data);
        }
        #endregion

        #region 违章列表
        /// <summary>
        /// 违章列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>   
        //[HandlerMonitor(3, "分页查询用户信息!")]
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

        #region 获取项目数据  承包商下各项目
        /// <summary>
        /// 获取项目数据  承包商下各项目
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetProjectDataJson()
        {

            var organizedata = organizeCache.GetList();
            var departmentdata = departmentBLL.GetList().Where(p => p.Nature == "承包商" || p.Nature == "项目");
            var treeList = new List<TreeEntity>();
            foreach (OrganizeEntity item in organizedata)
            {
                #region 机构
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
                #region 部门
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
                if (item.Nature == "承包商")
                {
                    tree.AttributeValue = "Contractor";
                }
                if (item.Nature == "项目")
                {
                    tree.AttributeValue = "Project";
                }
                treeList.Add(tree);
                #endregion
            }
            return Content(treeList.TreeToJson());
        }
        #endregion

        #region 是否启用极简模式
        /// <summary>
        /// 是否启用极简模式
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

        #region 页面组件初始化
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetInitDataJson(string keyValue="")
        {
            Operator curUser = OperatorProvider.Provider.Current();
            //隐患编码
            string HidCode = DateTime.Now.ToString("yyyyMMddHHmmssfff").ToString();
            //安全检查类型 隐患类型 隐患级别  培训模板  整改责任单位关联人员
            string itemCode = "'SaftyCheckType','HidType','HidRank','TrainTemplateName','HidMajorClassify','GIHiddenClassify','GIHiddenType','ChangeDeptRelevancePerson','AppSettings','HidBmEnableOrganize','IsDisableMajorClassify','IsEnableMajorClassify','AcceptPersonControl','ControlPicMustUpload'";
            //集合
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode(itemCode);

            var deptData = departmentBLL.GetList();

            List<DepartmentEntity> dlist = new List<DepartmentEntity>();

            if (curUser.RoleName.Contains("省级用户"))
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
                //当前用户的所属机构
                DepartmentEntity dept = userbll.GetUserOrgInfo(curUser.UserId);
                dlist.Add(dept);
            }

            //公司级用户
            if (userbll.HaveRoleListByKey(curUser.UserId, dataitemdetailbll.GetItemValue("HidOrganize")).Rows.Count > 0)
            {
                curUser.DeptCode = curUser.OrganizeCode;
                curUser.DeptName = curUser.OrganizeName;
            }

            //专业分类/隐患分类
            IEnumerable<DataItemModel> MajorClassify = new List<DataItemModel>();
            //隐患类别
            IEnumerable<DataItemModel> HidType = new List<DataItemModel>();

            if (curUser.Industry != "电力" && !string.IsNullOrEmpty(curUser.Industry))
            {
                MajorClassify = itemlist.Where(p => p.EnCode == "GIHiddenClassify");
                HidType = itemlist.Where(p => p.EnCode == "GIHiddenType");
            }
            else
            {
                MajorClassify = itemlist.Where(p => p.EnCode == "HidMajorClassify");
                HidType = itemlist.Where(p => p.EnCode == "HidType");
            }

            string KFDC_ORGCODE = dataitemdetailbll.GetItemValue("KFDC_ORGCODE");  //开封电厂

            string RelevancePersonRole = string.Empty;
            if (itemlist.Where(p => p.EnCode == "ChangeDeptRelevancePerson").Count() > 0)
            {
                var curRelevanceRole = itemlist.Where(p => p.EnCode == "ChangeDeptRelevancePerson").Where(p => p.ItemName == curUser.OrganizeCode);
                if (curRelevanceRole.Count() > 0)
                {
                    RelevancePersonRole = curRelevanceRole.FirstOrDefault().ItemValue;
                }
            }

            //相关图片必传控制配置
            string ControlPicMustUpload = string.Empty;
            var cpmu = itemlist.Where(p => p.EnCode == "ControlPicMustUpload").Where(p => p.ItemName == curUser.OrganizeId);
            if (cpmu.Count() > 0)
            {
                ControlPicMustUpload = cpmu.FirstOrDefault().ItemValue;
            }

            var IsDeliver = htworkflowbll.GetCurUserWfAuth("一般隐患", "隐患整改", "隐患整改", "厂级隐患排查", "转交", string.Empty, string.Empty, string.Empty, keyValue) == "1" ? "1" : "";
            var IsAcceptDeliver = htworkflowbll.GetCurUserWfAuth("一般隐患", "隐患验收", "隐患验收", "厂级隐患排查", "转交", string.Empty, string.Empty, string.Empty, keyValue) == "1" ? "1" : "";
            //返回值
            var josnData = new
            {
                CreateUser = curUser.UserName,
                User = curUser,  //用户对象
                HidCode = HidCode,
                HidPhoto = Guid.NewGuid().ToString(), //隐患图片
                HidChangePhoto = Guid.NewGuid().ToString(), //整改图片
                AcceptPhoto = Guid.NewGuid().ToString(), //验收图片 
                Attachment = Guid.NewGuid().ToString(),
                EstimatePhoto = Guid.NewGuid().ToString(), //评估图片  
                CheckType = itemlist.Where(p => p.EnCode == "SaftyCheckType"),

                HidType = HidType,
                HidRank = itemlist.Where(p => p.EnCode == "HidRank"),
                TrainTemplateName = itemlist.Where(p => p.EnCode == "TrainTemplateName"),
                MajorClassify = MajorClassify,  //隐患专业分类 
                DeptData = dlist,
                KFDC_ORGCODE = KFDC_ORGCODE,
                RelevancePersonRole = RelevancePersonRole,
                IsEnable_RemoteEqu = dataitemdetailbll.GetItemValue("IsGdxy"),
                IsDeliver = IsDeliver,
                IsAcceptDeliver = IsAcceptDeliver,
                IsEnable_HidBm = itemlist.Where(p => p.EnCode == "HidBmEnableOrganize").Where(p => p.ItemValue == curUser.OrganizeCode).Count(), //是否启用所属部门
                IsDisable_MajorClassify = itemlist.Where(p => p.EnCode == "IsDisableMajorClassify").Where(p => p.ItemValue == curUser.OrganizeCode).Count(), //是否禁用专业分类 
                IsEnableMajorClassify = itemlist.Where(p => p.EnCode == "IsEnableMajorClassify").Where(p => p.ItemValue == curUser.OrganizeCode).Count(), //是否启用专业分类 
                IsEnableAccept = itemlist.Where(p => p.EnCode == "AcceptPersonControl").Where(p => p.ItemValue == curUser.OrganizeCode).Count(), //是否启用验收人控制 
                ControlPicMustUpload = ControlPicMustUpload
            };

            return Content(josnData.ToJson());
        }
        #endregion

        #region 获取默认项列表
        /// <summary>
        /// 获取默认项列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDefaultOptionsList()
        {
            Operator curUser = OperatorProvider.Provider.Current();
            DefaultDataSettingBLL defaultdatasettingbll = new DefaultDataSettingBLL();
            List<object> data = new List<object>(); //返回的结果集合
            List<DefaultDataSettingEntity> list = defaultdatasettingbll.GetList(curUser.UserId).ToList(); //排序
            return Content(list.ToJson());
        }
        #endregion

        #region 获取隐患类型树结构
        /// <summary>
        /// 获取隐患类型树结构
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetHiddenTypeDataJson(string checktypeid = "")
        {
            //集合
            var data = dataitemdetailbll.GetDataItemListByItemCode("'HidType'");
            //安全检查类型
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

        #region 获取通用行业版本的隐患类型
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

        #region 获取隐患级别标准
        /// <summary>
        /// 获取隐患级别标准
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

        #region 获取编码管理中的项目
        /// <summary>
        /// 获取编码管理中的项目
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

        #region 初始化查询条件
        /// <summary>
        /// 初始化查询条件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetQueryConditionJson()
        {
            Operator curUser = OperatorProvider.Provider.Current();
            //隐患级别，整改状态，流程状态，检查类型，隐患类型 ,数据范围
            string itemCode = "'HidRank','ChangeStatus','WorkStream','SaftyCheckType','HidType','DataScope','GIHiddenType','HidStandingType','IsEnableMinimalistMode'";
            //集合
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode(itemCode);
            var deptData = departmentBLL.GetList();
            List<DepartmentEntity> dlist = new List<DepartmentEntity>();
            if (curUser.RoleName.Contains("省级用户"))
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
                //当前用户的所属机构
                DepartmentEntity dept = userbll.GetUserOrgInfo(curUser.UserId);
                dlist.Add(dept);
            }
            //隐患类别
            IEnumerable<DataItemModel> HidType = new List<DataItemModel>();
            if (curUser.Industry != "电力" && !string.IsNullOrEmpty(curUser.Industry))
            {
                HidType = itemlist.Where(p => p.EnCode == "GIHiddenType");
            }
            else
            {
                HidType = itemlist.Where(p => p.EnCode == "HidType");
            }
            //返回值
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

        #region 获取当前用户是否为安全管理员身份
        /// <summary>
        /// 获取当前用户是否为安全管理员身份
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult QuerySafetyRole()
        {
            //安全管理员角色编码
            string roleCode = dataitemdetailbll.GetItemValue("HidApprovalSetting");

            Operator curUser = OperatorProvider.Provider.Current();

            int uModel = 0;

            string HidApproval = dataitemdetailbll.GetItemValue("HidApproval");

            string[] pstr = HidApproval.Split('#');  //分隔机构组

            IList<UserEntity> ulist = new List<UserEntity>();

            foreach (string strArgs in pstr)
            {
                string[] str = strArgs.Split('|');
                //当前机构相同，且为本部门安全管理员验证
                if (str[0].ToString() == curUser.OrganizeId && str[1].ToString() == "0")
                {
                    ulist = userbll.GetUserListByRole(curUser.DeptCode, roleCode, curUser.OrganizeId).ToList();

                    break;
                }
                if (str[0].ToString() == curUser.OrganizeId && str[1].ToString() == "1")
                {
                    //获取指定部门的所有人员
                    ulist = new UserBLL().GetUserListByDeptCode(str[2].ToString(), null, false, curUser.OrganizeId).ToList();

                    break;
                }
            }

            if (ulist.Count() > 0)
            {
                //返回的记录数,大于0，标识当前用户拥有安全管理员身份，反之则无
                uModel = ulist.Where(p => p.UserId == curUser.UserId).Count();
            }
            return Content(uModel.ToString());
        }
        #endregion

        #region 获取当前用户的流程权限
        /// <summary>
        /// 获取当前用户的流程权限
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
            //获取下一流程的操作人
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

        #region 通过当前操作是否具有必填操作(国电新疆红雁池版本)
        /// <summary>
        /// 通过当前操作是否具有必填操作(国电新疆红雁池版本)
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
            //获取下一流程的操作人
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

        #region 提交数据

        #region 删除数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(6, "删除隐患基本信息")]
        public ActionResult RemoveForm(string keyValue, string hidcode)
        {

            HTBaseInfoEntity entity = htbaseinfobll.GetEntity(keyValue);
            Operator user = OperatorProvider.Provider.Current();

            //删除隐患信息
            htbaseinfobll.RemoveForm(keyValue);

            //删除评估信息
            htapprovalbll.RemoveFormByCode(hidcode);

            //删除整改信息
            htchangeinfobll.RemoveFormByCode(hidcode);

            //删除验收信息
            htacceptinfobll.RemoveFormByCode(hidcode);

            //删除整改效果评估信息
            htestimatebll.RemoveFormByCode(hidcode);

            LogEntity logEntity = new LogEntity();
            logEntity.Browser = this.Request.Browser.Browser;
            logEntity.CategoryId = 6;
            logEntity.OperateTypeId = "6";
            logEntity.OperateType = "删除";
            logEntity.OperateAccount = user.UserName;
            logEntity.OperateUserId = OperatorProvider.Provider.Current().UserId;
            logEntity.ExecuteResult = 1;
            logEntity.Module = SystemInfo.CurrentModuleName;
            logEntity.ModuleId = SystemInfo.CurrentModuleId;
            logEntity.ExecuteResultJson = "操作信息:删除隐患编号为" + entity.HIDCODE + ",隐患描述为" + entity.HIDDESCRIBE + "的隐患信息, 请求引用: 无, 其他信息:无";
            LogBLL.WriteLog(logEntity);

            return Success("删除成功!");
        }
        #endregion

        #region  保存表单（新增、修改）
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, HTBaseInfoEntity entity, HTChangeInfoEntity cEntity, HTAcceptInfoEntity aEntity)
        {
            bool issucess = CommonSaveForm(keyValue, entity, cEntity, aEntity);
            if (issucess)
            {
                return Success("操作成功!");
            }
            else
            {
                return Error("隐患编码重复,请重新新增!");
            }
        }
        #endregion

        #region 公用方法，保存数据(厂级)
        /// <summary>
        /// 公用方法，保存数据
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public bool CommonSaveForm(string keyValue, HTBaseInfoEntity entity, HTChangeInfoEntity cEntity, HTAcceptInfoEntity aEntity)
        {
            bool issucessful = true;
            Operator curuser = OperatorProvider.Provider.Current();
            //提交通过
            string userId = curuser.UserId;
            //隐患编码
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
                /********隐患信息**********/
                entity.ISBREAKRULE = "0"; //非违章
                //设备
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
                //安全检查
                string safetycheckid = string.Empty;
                string ctype = string.Empty;
                if (null != Request.Form["SAFETYCHECKID"])
                {
                    safetycheckid = Request.Form["SAFETYCHECKID"].ToString();
                }
                if (null != Request.Form["CTYPE"])
                {
                    ctype = Request.Form["CTYPE"].ToString();//安全检查类型(从安全检查传递过来的)
                }
                if (!string.IsNullOrEmpty(entity.SAFETYCHECKOBJECTID) && string.IsNullOrEmpty(ctype) && string.IsNullOrEmpty(safetycheckid))
                {
                    entity.RELEVANCEID = new SaftyCheckDataRecordBLL().GetRecordFromHT(entity.SAFETYCHECKOBJECTID, curuser);
                }
                //保存隐患基本信息
                htbaseinfobll.SaveForm(keyValue, entity);

                //当前是隐患，非违章，则创建流程实例

                //创建流程实例
                if (string.IsNullOrEmpty(keyValue))
                {
                    string workFlow = "01";//隐患处理
                    bool isSucess = htworkflowbll.CreateWorkFlowObj(workFlow, entity.ID, userId);
                    if (isSucess)
                    {
                        htworkflowbll.UpdateWorkStreamByObjectId(entity.ID);  //更新业务流程状态
                    }
                }

                /********整改信息************/
                string CHANGEID = Request.Form["CHANGEID"].ToString();
                cEntity.HIDCODE = entity.HIDCODE;
                //新增状态下添加
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

                /********验收信息************/
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

        #region 公用方法，保存数据(省级)
        /// <summary>
        /// 公用方法，保存数据
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public bool CommonSaveForm(string keyValue, HTBaseInfoEntity entity, HtReCheckEntity kEntity, HTChangeInfoEntity cEntity, HTAcceptInfoEntity aEntity)
        {
            //提交通过
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
                /********隐患信息**********/
                entity.ISBREAKRULE = "0"; //非违章
                //设备
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
                //保存隐患基本信息
                htbaseinfobll.SaveForm(keyValue, entity);

                //当前是隐患，非违章，则创建流程实例

                //创建流程实例
                if (string.IsNullOrEmpty(keyValue))
                {
                    string workFlow = "01";//隐患处理
                    bool isSucess = htworkflowbll.CreateWorkFlowObj(workFlow, entity.ID, userId);
                    if (isSucess)
                    {
                        htworkflowbll.UpdateWorkStreamByObjectId(entity.ID);  //更新业务流程状态
                    }
                }
                /********整改信息************/
                string CHANGEID = Request.Form["CHANGEID"].ToString();
                cEntity.HIDCODE = entity.HIDCODE;
                //新增状态下添加
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


                /********验收信息************/
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

        #region 提交流程（同时新增、修改隐患信息）(电厂级)
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, string isSubmit, HTBaseInfoEntity entity, HTChangeInfoEntity cEntity, HTAcceptInfoEntity aEntity)
        {
            cEntity.BACKREASON = "";  //回退原因赋值为空
            CommonSaveForm(keyValue, entity, cEntity, aEntity);

            //创建完流程实例后
            if (string.IsNullOrEmpty(keyValue))
            {
                keyValue = entity.ID;
            }

            //此处需要判断当前人是否为安全管理员
            string wfFlag = string.Empty;
            string participant = string.Empty;
            Operator curUser = OperatorProvider.Provider.Current();

            WfControlObj wfentity = new WfControlObj();
            wfentity.businessid = keyValue; //
            wfentity.argument1 = entity.MAJORCLASSIFY; //专业分类
            wfentity.argument2 = curUser.DeptId; //当前部门
            wfentity.argument3 = entity.HIDTYPE; //隐患类别
            wfentity.argument4 = entity.HIDBMID; //所属部门
            wfentity.startflow = "隐患登记";
            //是否上报
            if (isSubmit == "1")
            {
                wfentity.submittype = "上报";
            }
            else
            {
                wfentity.submittype = "提交";
                //不指定整改责任人
                if (cEntity.ISAPPOINT == "0")
                {
                    wfentity.submittype = "制定提交";
                }
            }

            #region 国电新疆版本
            if (entity.ADDTYPE == "3")
            {    //非本部门提交
                if (entity.ISSELFCHANGE == "0")
                {
                    wfentity.submittype = "制定提交";
                }
            }
            #endregion
            wfentity.rankid = entity.HIDRANK;
            wfentity.user = curUser;
            wfentity.mark = "厂级隐患排查";
            wfentity.organizeid = entity.HIDDEPART; //对应电厂id
            //获取下一流程的操作人
            WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
            //处理成功
            if (result.code == WfCode.Sucess)
            {
                participant = result.actionperson;
                wfFlag = result.wfflag;
                if (!string.IsNullOrEmpty(participant))
                {
                    int count = htworkflowbll.SubmitWorkFlow(wfentity, result, keyValue, participant, wfFlag, curUser.UserId);

                    if (count > 0)
                    {
                        htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //更新业务流程状态
                    }
                }
                else
                {
                    return Error("请联系系统管理员，添加本单位及相关单位评估人员!");
                }
                return Success(result.message);
            }
            else
            {
                return Error(result.message);
            }
        }
        #endregion

        #region  保存表单（新增、修改）(省级)
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveCForm(string keyValue, HTBaseInfoEntity entity, HtReCheckEntity kEntity, HTChangeInfoEntity cEntity, HTAcceptInfoEntity aEntity)
        {
            bool issucess = CommonSaveForm(keyValue, entity, kEntity, cEntity, aEntity);
            if (issucess)
            {
                return Success("操作成功!");
            }
            else
            {
                return Error("隐患编码重复,请重新新增!");
            }
        }
        #endregion

        #region 省级公司提交流程（同时新增、修改隐患信息 省级）
        /// <summary>
        /// 保存表单（新增、修改）
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

            cEntity.BACKREASON = "";  //回退原因赋值为空
            CommonSaveForm(keyValue, entity, kEntity, cEntity, aEntity);

            //创建完流程实例后
            if (string.IsNullOrEmpty(keyValue))
            {
                keyValue = entity.ID;
            }

            //此处需要判断当前人是否为安全管理员
            string wfFlag = string.Empty;

            Operator curUser = OperatorProvider.Provider.Current();

            //配置的隐患完善人员
            WfControlObj wfentity = new WfControlObj();
            wfentity.businessid = keyValue; //
            wfentity.startflow = "隐患登记";
            wfentity.submittype = "提交";
            wfentity.rankid = entity.HIDRANK; //级别
            wfentity.user = curUser;
            wfentity.mark = "省级隐患排查";
            wfentity.organizeid = entity.HIDDEPART; //对应电厂id
            //获取下一流程的操作人
            WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
            //处理成功
            if (result.code == WfCode.Sucess)
            {
                participant = result.actionperson;
                wfFlag = result.wfflag;
                if (!string.IsNullOrEmpty(participant))
                {
                    int count = htworkflowbll.SubmitWorkFlow(wfentity, result, keyValue, participant, wfFlag, curUser.UserId);

                    if (count > 0)
                    {
                        htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //更新业务流程状态
                    }
                }
                else
                {
                    return Error("请联系系统管理员，添加所属单位的隐患完善人员!");
                }
                return Success(result.message);
            }
            else
            {
                return Error(result.message);
            }
        }
        #endregion

        #region 提交隐患完善流程（同时修改隐患信息）
        /// <summary>
        /// 省级公司提交隐患完善流程（同时修改隐患信息）
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
                    return Error("隐患编码重复,请重新新增!");
                }
            }
            cEntity.BACKREASON = "";  //回退原因赋值为空
            CommonSaveForm(keyValue, entity, cEntity, aEntity);

            //创建完流程实例后
            if (string.IsNullOrEmpty(keyValue))
            {
                keyValue = entity.ID;
            }

            //此处需要判断当前人是否为安全管理员
            string wfFlag = string.Empty;

            Operator curUser = OperatorProvider.Provider.Current();

            WfControlObj wfentity = new WfControlObj();
            wfentity.businessid = keyValue; //
            wfentity.startflow = "隐患完善";
            wfentity.submittype = "提交";
            wfentity.rankid = entity.HIDRANK;
            wfentity.user = curUser;
            wfentity.mark = "省级隐患排查";
            wfentity.organizeid = entity.HIDDEPART; //对应电厂id
            //获取下一流程的操作人
            WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
            //处理成功
            if (result.code == WfCode.Sucess)
            {
                participant = result.actionperson;
                wfFlag = result.wfflag;
                if (!string.IsNullOrEmpty(participant))
                {
                    int count = htworkflowbll.SubmitWorkFlow(wfentity, result, keyValue, participant, wfFlag, curUser.UserId);

                    if (count > 0)
                    {
                        htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //更新业务流程状态
                    }
                }
                else
                {
                    return Error("请联系系统管理员，添加当前流程下的评估人!");
                }
                return Success(result.message);
            }
            else
            {
                return Error(result.message);
            }
        }
        #endregion

        #region 提交制定整改计划流程（同时修改隐患信息）
        /// <summary>
        /// 提交制定整改计划流程（同时修改隐患信息）
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
                    return Error("隐患编码重复,请重新新增!");
                }
            }
            cEntity.BACKREASON = "";  //回退原因赋值为空
            entity.ISFORMULATE = "1"; //标记已经进行了制定整改计划内容
            CommonSaveForm(keyValue, entity, cEntity, aEntity);

            //创建完流程实例后
            if (string.IsNullOrEmpty(keyValue))
            {
                keyValue = entity.ID;
            }


            //bool isspecial = false;
            #region 检查当前是否为生技部(具有直接提交到隐患整改的权限，也可设置其他角色部门),

            Operator curUser = OperatorProvider.Provider.Current();
            //WfControlObj wfValentity = new WfControlObj();
            //wfValentity.businessid = ""; //
            //wfValentity.startflow = "制定整改计划";
            //wfValentity.endflow = "隐患整改";
            //wfValentity.submittype = "提交";
            //wfValentity.rankid = entity.HIDRANK;
            //wfValentity.user = curUser;
            //wfValentity.mark = "厂级隐患排查";
            //wfValentity.isvaliauth = true;

            //string resultVal = string.Empty;
            ////获取下一流程的操作人
            //WfControlResult valresult = wfcontrolbll.GetWfControl(wfValentity);
            //isspecial = valresult.ishave; //验证结果
            #endregion


            //此处需要判断当前人是否为安全管理员
            string wfFlag = string.Empty;

            WfControlObj wfentity = new WfControlObj();
            wfentity.businessid = keyValue; //
            wfentity.startflow = "制定整改计划";
            //具有生技部的权限，且整改部门就是生技部，则直接提交到整改
            //if (isspecial && cEntity.CHANGEDUTYDEPARTID == curUser.DeptId)
            //{
            wfentity.submittype = "提交";
            //}
            //else
            //{
            //    wfentity.submittype = "制定提交";
            //}
            wfentity.rankid = entity.HIDRANK;
            wfentity.user = curUser;
            wfentity.mark = "厂级隐患排查";
            wfentity.organizeid = entity.HIDDEPART; //对应电厂id
            //获取下一流程的操作人
            WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
            //处理成功
            if (result.code == WfCode.Sucess)
            {
                participant = result.actionperson;
                wfFlag = result.wfflag;
                if (!string.IsNullOrEmpty(participant))
                {
                    int count = htworkflowbll.SubmitWorkFlow(wfentity, result, keyValue, participant, wfFlag, curUser.UserId);

                    if (count > 0)
                    {
                        htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //更新业务流程状态
                    }
                }
                else
                {
                    return Error("请联系系统管理员，添加当前流程下的参与者!");
                }
                return Success(result.message);
            }
            else
            {
                return Error(result.message);
            }
        }
        #endregion

        #region 转发制定整改计划/隐患整改、隐患验收流程
        /// <summary>
        /// 转发制定整改计划/隐患整改、隐患验收流程
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
            /********整改信息************/
            if (!Request.Form["CHANGEID"].IsEmpty())
            {  
                string CHANGEID = Request.Form["CHANGEID"].ToString();
                //更新整改信息
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
            /********验收信息************/
            if (!Request.Form["ACCEPTID"].IsEmpty())
            {
                string ACCEPTID = Request.Form["ACCEPTID"].ToString();
                //更新验收信息
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
            //此处需要判断当前人是否为安全管理员
            string wfFlag = string.Empty;
            WfControlObj wfentity = new WfControlObj();
            wfentity.businessid = keyValue; //业务id
            wfentity.startflow = entity.WORKSTREAM;
            wfentity.endflow = entity.WORKSTREAM;
            wfentity.submittype = "转交";
            wfentity.rankid = entity.HIDRANK;
            wfentity.user = curUser;
            wfentity.mark = "厂级隐患排查";
            wfentity.organizeid = entity.HIDDEPART; //对应电厂id
            //获取下一流程的操作人
            WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
            //处理成功
            if (result.code == WfCode.Sucess)
            {
                participant = result.actionperson;
                wfFlag = result.wfflag;
                if (!string.IsNullOrEmpty(participant))
                {
                    //不更改状态
                    if (!result.ischangestatus)
                    {
                        htworkflowbll.SubmitWorkFlowNoChangeStatus(wfentity, result, keyValue, participant, curUser.UserId);
                        return Success(result.message);
                    }
                }
                else
                {
                    return Error("请联系系统管理员，添加当前流程下的参与者!");
                }
                return Success(result.message);
            }
            else
            {
                return Error(result.message);
            }
        }
        #endregion

        #region 一次性提交多个关联检查人的登记隐患信息(电厂级)
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult CheckHiddenForm(string saftycheckdatarecordid)
        {
            Operator curUser = OperatorProvider.Provider.Current();

            var dtHid = htbaseinfobll.GetList(saftycheckdatarecordid, curUser.UserId, "", "隐患登记");

            string keyValue = string.Empty;

            string changeperson = string.Empty;

            string hiddepart = string.Empty;

            string rankid = string.Empty;

            foreach (DataRow row in dtHid.Rows)
            {
                keyValue = row["id"].ToString();

                hiddepart = row["hiddepart"].ToString();

                rankid = row["hidrank"].ToString(); //隐患级别

                string upsubmit = row["upsubmit"].ToString(); //隐患级别

                string isselfchange = row["isselfchange"].ToString(); //是否本部门整改

                string isappoint = row["isappoint"].ToString(); //是否制定

                string addtype = row["addtype"].ToString(); //用于判断是否省级公司提交的隐患

                string hidbmid = row["hidbmid"].ToString(); //所属部门

                string majorclassify = row["majorclassify"].ToString(); //专业分类

                string hidtype = row["hidtype"].ToString(); //隐患类别

                string createuserid = row["createuserid"].ToString(); //创建人

                //此处需要判断当前人是否为安全管理员
                string wfFlag = string.Empty;
                //参与人员
                string participant = string.Empty;
                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = keyValue; //
                wfentity.argument1 = majorclassify; //专业分类
                wfentity.argument2 = curUser.DeptId; //当前部门
                wfentity.argument3 = hidtype; //隐患类别
                wfentity.argument4 = hidbmid; //所属部门
                wfentity.startflow = "隐患登记";
                //是否上报
                if (upsubmit == "1")
                {
                    wfentity.submittype = "上报";
                }
                else
                {
                    wfentity.submittype = "提交";

                    //不指定整改责任人
                    if (!string.IsNullOrEmpty(isappoint) && isappoint == "0")
                    {
                        wfentity.submittype = "制定提交";
                    }
                }

                wfentity.rankid = rankid;
                wfentity.spuser = userbll.GetUserInfoEntity(createuserid);
                wfentity.organizeid = hiddepart; //对应电厂id
                //省级登记的
                if (addtype == "2")
                {
                    wfentity.mark = "省级隐患排查";
                }
                else
                {
                    wfentity.mark = "厂级隐患排查";
                }

                #region 国电新疆版本
                if (addtype == "3")
                {    //非本部门提交
                    if (isselfchange == "0")
                    {
                        wfentity.submittype = "制定提交";
                    }
                }
                #endregion
                //获取下一流程的操作人
                WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
                //处理成功
                if (result.code == WfCode.Sucess)
                {
                    participant = result.actionperson;
                    wfFlag = result.wfflag;
                    if (!string.IsNullOrEmpty(participant))
                    {
                        int count = htworkflowbll.SubmitWorkFlow(wfentity, result, keyValue, participant, wfFlag, createuserid);

                        if (count > 0)
                        {
                            htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //更新业务流程状态
                        }
                    }
                }
            }
            return Success("操作成功!");
        }
        #endregion

        #region 立即整改、提交隐患及整改信息(电厂级)
        /// <summary>
        /// 一次性提交隐患及整改信息
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
            //此处需要判断当前人是否为安全管理员
            string wfFlag = string.Empty;

            Operator curUser = OperatorProvider.Provider.Current();

            //参与人员
            string participant = string.Empty;

            ////保存隐患信息
            entity.ISBREAKRULE = "0";//未违章
            cEntity.CHANGERESULT = CHANGERESULT;
            iEntity.ACCEPTSTATUS = ACCEPTSTATUS;
            CommonSaveForm(keyValue, entity, cEntity, iEntity);

            ////保存评估信息
            aEntity.APPROVALRESULT = APPROVALRESULT;
            htapprovalbll.SaveForm(ApprovalID, aEntity);

            wfFlag = "2";//整改结束

            participant = curUser.Account;

            int count = htworkflowbll.SubmitWorkFlow(entity.ID, participant, wfFlag, curUser.UserId);

            if (count > 0)
            {
                htworkflowbll.UpdateWorkStreamByObjectId(entity.ID);  //更新业务流程状态
            }

            return Success("操作成功!");
        }
        #endregion

        #region 导入隐患信息
        /// <summary>
        /// 导入隐患信息
        /// </summary>
        /// <param name="checkid">安全检查id</param>
        /// <param name="repeatdata">重复数据</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportHiddenInfo(string checkid, string repeatdata, string mode)
        {

            if (OperatorProvider.Provider.Current().IsSystem)
            {
                return "超级管理员无此操作权限";
            }
            var curUser = OperatorProvider.Provider.Current();
            string orgId = curUser.OrganizeId;//所属公司

            string message = "请选择格式正确的文件再导入!";
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
                        return "请按正确的方式导入文件,一次上传最多支持两份文件(即一份excel数据文件,一份图片压缩文件).";
                    }
                    HttpPostedFileBase file = HttpContext.Request.Files[0];
                    string hiddenDirectory = string.Empty;
                    Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();

                    #region 两份文件时
                    if (HttpContext.Request.Files.Count == 2)
                    {
                        HttpPostedFileBase file2 = HttpContext.Request.Files[1];
                        if (string.IsNullOrEmpty(file.FileName) || string.IsNullOrEmpty(file2.FileName))
                        {
                            return message;
                        }
                        Boolean isZip1 = file.FileName.ToLower().Substring(file.FileName.ToLower().IndexOf('.')).Contains("zip");//第一个文件是否为Zip格式
                        Boolean isZip2 = file2.FileName.ToLower().Substring(file2.FileName.ToLower().IndexOf('.')).Contains("zip");//第二个文件是否为Zip格式
                        if ((isZip1 || isZip2) == false || (isZip1 && isZip2) == true)
                        {
                            return message;
                        }
                        string fileName1 = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                        file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName1));
                        string fileName2 = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file2.FileName);
                        file2.SaveAs(Server.MapPath("~/Resource/temp/" + fileName2));
                        hiddenDirectory = Server.MapPath("~/Resource/temp/") + DateTime.Now.ToString("yyyyMMddhhmmssfff") + "\\"; //隐患/违章图片存放地址                                                                                                    //当前文件夹存在
                        if (!Directory.Exists(hiddenDirectory))
                        {
                            System.IO.Directory.CreateDirectory(hiddenDirectory); //创建目录
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
                    #region 一份文件时
                    else  //一份文件时
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
                    #region 第二张表部分(违章考核、奖励)
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
                    //记录错误信息
                    List<string> resultlist = new List<string>();
                    List<UserEntity> ulist = userbll.GetList().OrderBy(p => p.SortCode).ToList();
                    List<DepartmentEntity> dlist = departmentBLL.GetList().OrderBy(p => p.SortCode).ToList();
                    int total = 0;
                    int checkproject = 0;
                    SaftyCheckDataDetailBLL sdbll = new SaftyCheckDataDetailBLL();
                    SaftyCheckContentBLL scbll = new SaftyCheckContentBLL();
                    SaftyCheckDataRecordEntity safetyEntity = null;
                    //检查记录
                    if (!string.IsNullOrEmpty(checkid))
                    {
                        safetyEntity = saftycheckdatarecordbll.GetEntity(checkid);

                        checkproject = sdbll.GetCheckItemCount(checkid);
                    }
                    //检查类型集合
                    var checktypelist = dataitemdetailbll.GetDataItemListByItemCode("'SaftyCheckType'");
                    //隐患部分
                    #region 隐患部分
                    if (sheets.Name.Contains("隐患") || dt.Columns.Contains("隐患名称"))
                    {
                        #region 对象装载
                        List<ImportHidden> list = new List<ImportHidden>();
                        //先获取到职务列表;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string resultmessage = "第" + (i + 1).ToString() + "行"; //显示错误

                            bool isadddobj = true;
                            //隐患编码
                            string hidcode = dt.Columns.Contains("隐患编码") ? dt.Rows[i]["隐患编码"].ToString().Trim() : string.Empty;
                            //检查对象
                            string checkobj = dt.Columns.Contains("检查对象") ? dt.Rows[i]["检查对象"].ToString().Trim() : string.Empty;
                            //检查内容
                            string checkcontent = dt.Columns.Contains("检查内容") ? dt.Rows[i]["检查内容"].ToString().Trim() : string.Empty;
                            //隐患名称 
                            string hiddenname = dt.Columns.Contains("隐患名称") ? dt.Rows[i]["隐患名称"].ToString().Trim() : string.Empty;
                            //隐患级别
                            string rankname = dt.Columns.Contains("隐患级别") ? dt.Rows[i]["隐患级别"].ToString().Trim() : string.Empty;
                            //隐患区域
                            string areaname = dt.Columns.Contains("隐患区域") ? dt.Rows[i]["隐患区域"].ToString().Trim() : string.Empty;
                            //专业分类
                            string majorclassify = dt.Columns.Contains("专业分类") ? dt.Rows[i]["专业分类"].ToString().Trim() : string.Empty;
                            //隐患类别
                            string hidtype = dt.Columns.Contains("隐患类别") ? dt.Rows[i]["隐患类别"].ToString().Trim() : string.Empty;
                            //设备名称
                            string devicename = dt.Columns.Contains("设备名称") ? dt.Rows[i]["设备名称"].ToString().Trim() : string.Empty;
                            //隐患描述
                            string hiddescribe = dt.Columns.Contains("事故隐患描述(简题)") ? dt.Rows[i]["事故隐患描述(简题)"].ToString().Trim() : string.Empty;
                            //华电江陵
                            if (mode == "6")
                            {
                                hiddescribe = dt.Columns.Contains("隐患描述") ? dt.Rows[i]["隐患描述"].ToString().Trim() : string.Empty;
                            }
                            //整改责任人
                            string changeperson = dt.Columns.Contains("整改责任人") ? dt.Rows[i]["整改责任人"].ToString().Trim() : string.Empty;
                            //整改责任人电话
                            string telephone = dt.Columns.Contains("整改责任人电话") ? dt.Rows[i]["整改责任人电话"].ToString().Trim() : string.Empty;
                            //整改责任单位
                            string changedept = dt.Columns.Contains("整改责任单位") ? dt.Rows[i]["整改责任单位"].ToString().Trim() : string.Empty;
                            //整改截止时间
                            string changedeadline = dt.Columns.Contains("整改截止时间") ? dt.Rows[i]["整改截止时间"].ToString().Trim() : string.Empty;
                            //整改措施
                            string changemeasure = dt.Columns.Contains("整改措施") ? dt.Rows[i]["整改措施"].ToString().Trim() : string.Empty;
                            //验收人
                            string acceptperson = dt.Columns.Contains("验收人") ? dt.Rows[i]["验收人"].ToString().Trim() : string.Empty;
                            //验收单位
                            string acceptdept = dt.Columns.Contains("验收单位") ? dt.Rows[i]["验收单位"].ToString().Trim() : string.Empty;
                            //验收日期
                            string acceptdate = dt.Columns.Contains("验收日期") ? dt.Rows[i]["验收日期"].ToString().Trim() : string.Empty;

                            string relevanceid = string.Empty;

                            string muchmark = string.Empty;
                            try
                            {
                                #region 对象集合
                                ImportHidden entity = new ImportHidden();
                                //编码
                                entity.hidcode = hidcode; //编码
                                entity.checkcontent = checkcontent; //检查内容
                                entity.checkobj = checkobj; //检查对象
                                //隐患名称
                                entity.hiddenname = hiddenname;
                                //隐患级别
                                if (!string.IsNullOrEmpty(rankname))
                                {
                                    var rankEntity = dataitemdetailbll.GetEntityByItemName(rankname);
                                    if (null != rankEntity)
                                    {
                                        entity.rankid = rankEntity.ItemDetailId; //隐患级别id
                                    }
                                }
                                //隐患区域
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
                                        entity.areacode = districtEntity.DistrictCode; //区域编码
                                        entity.areaname = districtEntity.DistrictName; //区域名称
                                    }
                                }

                                //专业分类
                                if (!string.IsNullOrEmpty(majorclassify))
                                {
                                    var majorlist = dataitemdetailbll.GetDataItemListByItemCode("'HidMajorClassify'").Where(p => p.ItemName == majorclassify);
                                    if (majorlist.Count() > 0)
                                    {
                                        entity.majorclassify = majorlist.FirstOrDefault().ItemDetailId; //专业分类id
                                    }
                                }
                                //隐患类别
                                if (!string.IsNullOrEmpty(hidtype))
                                {
                                    var hidtypeEntity = dataitemdetailbll.GetEntityByItemName(hidtype);
                                    if (null != hidtypeEntity)
                                    {
                                        entity.hidtype = hidtypeEntity.ItemDetailId; //隐患类别id
                                    }
                                }
                                entity.devicename = devicename; //设备名称
                                //隐患描述
                                if (!string.IsNullOrEmpty(hiddescribe))
                                {
                                    entity.hiddescribe = hiddescribe; //隐患描述
                                }

                                //华电可门需求
                                if (mode == "5")
                                {
                                    #region  整改责任人
                                    if (!string.IsNullOrEmpty(changeperson))
                                    {
                                        var userlist = ulist.Where(p => p.RealName == changeperson).ToList();
                                        if (!string.IsNullOrEmpty(telephone))
                                        {
                                            userlist = userlist.Where(p => p.Telephone == telephone || p.Mobile == telephone).ToList();
                                        }
                                        if (userlist.Count() > 1)
                                        {
                                            muchmark = "1";  //整改人存在重名
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

                                    #region 整改责任单位
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

                                    #region 验收人
                                    if (!string.IsNullOrEmpty(acceptperson))
                                    {
                                        var userlist = ulist.Where(p => p.RealName == acceptperson).ToList();
                                        if (userlist.Count() > 1)
                                        {
                                            muchmark = "2";  //验收人存在重名
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

                                    #region 验收单位
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
                                    //整改责任人 及 整改责任单位
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

                                    //验收人 及 验收单位
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

                                //整改截止时间
                                if (!string.IsNullOrEmpty(changedeadline))
                                {
                                    entity.changedeadline = Convert.ToDateTime(changedeadline); //整改截止时间
                                }
                                //整改措施
                                if (!string.IsNullOrEmpty(changemeasure))
                                {
                                    entity.changemeasure = changemeasure; //整改措施
                                }
                                //验收日期
                                if (!string.IsNullOrEmpty(acceptdate))
                                {
                                    entity.acceptdate = Convert.ToDateTime(acceptdate); //验收日期
                                }
                                #endregion

                                #region 必填验证
                                if (string.IsNullOrEmpty(entity.hidcode))
                                {
                                    resultmessage += "隐患编码为空、";
                                    isadddobj = false;
                                }
                                if (!string.IsNullOrEmpty(entity.hidcode))
                                {
                                    //  if (Regex.IsMatch(str, "^(?<year>\\d{2,4})(?<month>\\d{1,2})(?<day>\\d{1,2})(\\d{4})?$"))
                                    if (!Regex.IsMatch(entity.hidcode, "^(\\d{2,4})(\\d0[1-9]|1[0-2])((0[1-9])|(1[0-9])|(2[0-9])|30|31)(\\d{4})$"))
                                    {
                                        resultmessage += "隐患编码格式验证失败、";
                                        isadddobj = false;
                                    }
                                }
                                if (string.IsNullOrEmpty(entity.rankid))
                                {
                                    resultmessage += "隐患级别为空、";
                                    isadddobj = false;
                                }
                                if (string.IsNullOrEmpty(entity.areacode) && mode != "6")
                                {
                                    if (string.IsNullOrEmpty(areaname))
                                    {
                                        resultmessage += "隐患区域为空、";
                                    }
                                    else
                                    {
                                        resultmessage += "隐患区域与参考数据不匹配、";
                                    }
                                    isadddobj = false;
                                }
                                if (string.IsNullOrEmpty(entity.majorclassify))
                                {
                                    resultmessage += "专业分类为空、";
                                    isadddobj = false;
                                }
                                if (string.IsNullOrEmpty(entity.hidtype))
                                {
                                    resultmessage += "隐患类别为空、";
                                    isadddobj = false;
                                }

                                if (string.IsNullOrEmpty(hiddescribe))
                                {
                                    //华电江陵
                                    if (mode == "6")
                                    {
                                        resultmessage += "隐患描述为空、";
                                    }
                                    else { resultmessage += "事故隐患描述(简题)为空、"; }

                                    isadddobj = false;
                                }


                                //华电可门判断
                                if (mode == "5")
                                {
                                    if (string.IsNullOrEmpty(entity.changepersonid))
                                    {
                                        if (string.IsNullOrEmpty(changeperson))
                                        {
                                            resultmessage += "整改责任人为空、";
                                        }
                                        else
                                        {
                                            if (muchmark == "1")
                                            {
                                                resultmessage += "整改责任人出现重名、";
                                            }
                                            else
                                            {
                                                resultmessage += "整改责任人不存在于系统中、";
                                            }
                                        }
                                        isadddobj = false;
                                    }


                                    if (string.IsNullOrEmpty(entity.acceptpersonid))
                                    {
                                        if (string.IsNullOrEmpty(acceptperson))
                                        {
                                            resultmessage += "验收人为空、";
                                        }
                                        else
                                        {
                                            if (muchmark == "2")
                                            {
                                                resultmessage += "验收人出现重名、";
                                            }
                                            else
                                            {
                                                resultmessage += "验收人不存在于系统中、";
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
                                            resultmessage += "整改责任单位为空、";
                                        }
                                        else
                                        {
                                            if (string.IsNullOrEmpty(changeperson))
                                            {
                                                resultmessage += "整改责任人为空、";
                                            }
                                            else
                                            {
                                                resultmessage += "整改责任单位或整改责任人不存在于系统中、";
                                            }
                                        }
                                        isadddobj = false;
                                    }


                                    if (string.IsNullOrEmpty(entity.acceptpersonid) || string.IsNullOrEmpty(entity.acceptdeptcode))
                                    {
                                        if (string.IsNullOrEmpty(acceptdept))
                                        {
                                            resultmessage += "验收单位为空、";
                                        }
                                        else
                                        {
                                            if (string.IsNullOrEmpty(acceptperson))
                                            {
                                                resultmessage += "验收人为空、";
                                            }
                                            else
                                            {
                                                resultmessage += "验收单位或验收人不存在于系统中、";
                                            }
                                        }
                                        isadddobj = false;
                                    }
                                }

                                if (string.IsNullOrEmpty(changedeadline))
                                {
                                    resultmessage += "整改截止时间为空、";
                                    isadddobj = false;
                                }


                                //获取已存在的隐患数据
                                var htlist = htbaseinfobll.GetListByCode(entity.hidcode);

                                if (htlist.Count() > 0)
                                {
                                    //跳过操作
                                    if (repeatdata == "0")
                                    {
                                        resultmessage += "编号为" + entity.hidcode + "的隐患已存在，导入操作已忽略并跳过、";
                                        isadddobj = false;
                                    }
                                }

                                //符合条件的添加到集合当中
                                if (isadddobj)
                                {
                                    list.Add(entity);
                                }
                                else
                                {
                                    resultmessage = resultmessage.Substring(0, resultmessage.Length - 1) + ",无法正常导入";
                                    resultlist.Add(resultmessage);
                                }
                                #endregion
                            }
                            catch
                            {
                                resultmessage += "出现数据异常,无法正常导入";
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

                        #region 隐患数据集合
                        foreach (ImportHidden entity in list)
                        {
                            string keyValue = string.Empty;
                            bool isExecute = true;
                            HTBaseInfoEntity baseentity = new HTBaseInfoEntity();
                            //获取已存在的隐患数据
                            var htlist = htbaseinfobll.GetListByCode(entity.hidcode);

                            if (htlist.Count() > 0)
                            {
                                //覆盖操作
                                if (repeatdata == "1")
                                {
                                    baseentity = htlist.FirstOrDefault();
                                    keyValue = baseentity.ID;
                                }
                                else  //跳过
                                {
                                    isExecute = false;
                                }
                            }

                            entity.relevanceid = saftycheckdatarecordbll.GetCheckContentId(checkid, entity.checkobj, entity.checkcontent, curUser);
                            if (string.IsNullOrEmpty(entity.relevanceid))
                            {
                                isExecute = false;
                                falseMessage += "检查对象及检查内容未匹配或者当前检查项已被检查过或者该项属于其他人检查范围,无法正常导入</br>";
                            }
                            else
                            {
                                listIds.Add(entity.relevanceid.Split(',')[0]);
                            }

                            //是否执行
                            if (isExecute)
                            {
                                #region 隐患信息保存
                                //隐患基本信息

                                baseentity.ADDTYPE = "0"; //非立即整改隐患
                                baseentity.APPSIGN = "Import"; //导入标记
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
                                    baseentity.RELEVANCEID = checkcontentid; //检查内容id
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
                                #region 添加隐患图片
                                if (!string.IsNullOrEmpty(hiddenDirectory))
                                {
                                    //当前文件夹存在
                                    if (Directory.Exists(hiddenDirectory))
                                    {
                                        //读取图片
                                        DirectoryInfo directinfo = new DirectoryInfo(hiddenDirectory);
                                        List<FileInfo> fileinfoes = GetFiles(directinfo, new List<FileInfo>());
                                        #region 图片文件
                                        foreach (FileInfo finfo in fileinfoes)
                                        {
                                            string fextension = finfo.Extension;//文件扩展名
                                            string fname = finfo.Name; //文件名称
                                            //过滤图片格式
                                            #region 过滤图片格式
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
                                                    //创建文件夹
                                                    string path = Path.GetDirectoryName(fullFileName);
                                                    Directory.CreateDirectory(path);
                                                    FileInfoEntity fileInfoEntity = new FileInfoEntity();
                                                    if (!System.IO.File.Exists(fullFileName))
                                                    {
                                                        //保存文件
                                                        finfo.CopyTo(fullFileName);
                                                    }
                                                    finfo.Delete();//删除文件
                                                    //文件信息写入数据库
                                                    fileInfoEntity.Create();
                                                    fileInfoEntity.FileId = fileGuid;
                                                    fileInfoEntity.RecId = baseentity.HIDPHOTO; //关联ID
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
                                    string workFlow = "01";//隐患处理
                                    bool isSucess = htworkflowbll.CreateWorkFlowObj(workFlow, baseentity.ID, curUser.UserId);
                                    if (isSucess)
                                    {
                                        bool res = htworkflowbll.UpdateWorkStreamByObjectId(baseentity.ID);  //更新业务流程状态 
                                    }
                                }

                                //整改基本信息
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
                                //验收基本信息
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
                    #region 违章部分
                    else if (sheets.Name.Contains("违章") || dt.Columns.Contains("违章类型"))//违章信息导入
                    {

                        #region 对象装载
                        List<ImportLllegal> list = new List<ImportLllegal>();
                        List<ImportLllegal> khlist = new List<ImportLllegal>();
                        //先获取到职务列表;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string resultmessage = "第" + (i + 1).ToString() + "行"; //显示错误

                            bool isadddobj = true;

                            //违章编码
                            string lllegalnumber = dt.Columns.Contains("违章编码") ? dt.Rows[i]["违章编码"].ToString().Trim() : string.Empty;
                            //检查对象
                            string checkobj = dt.Columns.Contains("检查对象") ? dt.Rows[i]["检查对象"].ToString().Trim() : string.Empty;
                            //检查内容
                            string checkcontent = dt.Columns.Contains("检查内容") ? dt.Rows[i]["检查内容"].ToString().Trim() : string.Empty;
                            //违章类型 
                            string lllegaltype = dt.Columns.Contains("违章类型") ? dt.Rows[i]["违章类型"].ToString().Trim() : string.Empty;
                            //违章级别
                            string lllegallevel = dt.Columns.Contains("违章级别") ? dt.Rows[i]["违章级别"].ToString().Trim() : string.Empty;
                            //专业分类
                            string majorclassify = dt.Columns.Contains("专业分类") ? dt.Rows[i]["专业分类"].ToString().Trim() : string.Empty;
                            //违章人员
                            string lllegalperson = dt.Columns.Contains("违章人员") ? dt.Rows[i]["违章人员"].ToString().Trim() : string.Empty;
                            //违章单位
                            string lllegalteam = dt.Columns.Contains("违章单位") ? dt.Rows[i]["违章单位"].ToString().Trim() : string.Empty;
                            //违章时间
                            string lllegaltime = dt.Columns.Contains("违章时间") ? dt.Rows[i]["违章时间"].ToString().Trim() : string.Empty;
                            //违章地点
                            string lllegaladdress = dt.Columns.Contains("违章地点") ? dt.Rows[i]["违章地点"].ToString().Trim() : string.Empty;
                            //违章描述
                            string lllegaldescribe = dt.Columns.Contains("违章描述") ? dt.Rows[i]["违章描述"].ToString().Trim() : string.Empty;
                            //整改责任人
                            string reformpeople = dt.Columns.Contains("整改责任人") ? dt.Rows[i]["整改责任人"].ToString().Trim() : string.Empty;
                            //整改责任人电话
                            string telephone = dt.Columns.Contains("整改责任人电话") ? dt.Rows[i]["整改责任人电话"].ToString().Trim() : string.Empty;
                            //整改责任单位
                            string reformdeptname = dt.Columns.Contains("整改责任单位") ? dt.Rows[i]["整改责任单位"].ToString().Trim() : string.Empty;
                            //整改截止时间
                            string reformdeadline = dt.Columns.Contains("整改截止时间") ? dt.Rows[i]["整改截止时间"].ToString().Trim() : string.Empty;
                            //整改措施
                            string reformrequire = dt.Columns.Contains("整改措施") ? dt.Rows[i]["整改措施"].ToString().Trim() : string.Empty;
                            //验收人
                            string acceptpeople = dt.Columns.Contains("验收人") ? dt.Rows[i]["验收人"].ToString().Trim() : string.Empty;
                            //验收单位
                            string acceptdeptname = dt.Columns.Contains("验收单位") ? dt.Rows[i]["验收单位"].ToString().Trim() : string.Empty;
                            //验收日期
                            string accepttime = dt.Columns.Contains("验收日期") ? dt.Rows[i]["验收日期"].ToString().Trim() : string.Empty;

                            //违章责任单位
                            string lllegaldepart = dt.Columns.Contains("违章责任单位") ? dt.Rows[i]["违章责任单位"].ToString().Trim() : string.Empty;
                            //违章单位考核金额
                            string wzdwpunish = dt.Columns.Contains("违章单位考核金额") ? dt.Rows[i]["违章单位考核金额"].ToString().Trim() : string.Empty;
                            //责任单位考核金额
                            string zrdwpunish = dt.Columns.Contains("责任单位考核金额") ? dt.Rows[i]["责任单位考核金额"].ToString().Trim() : string.Empty;

                            string relevanceid = string.Empty;

                            // 违章类型 违章级别 流程状态
                            string itemCode = "'LllegalType','LllegalLevel','HidMajorClassify'";
                            //集合
                            var itemlist = dataitemdetailbll.GetDataItemListByItemCode(itemCode);
                            try
                            {
                                #region 对象集合
                                ImportLllegal entity = new ImportLllegal();
                                //违章编号
                                entity.lllegalnumber = lllegalnumber; //违章编号
                                entity.checkcontent = checkcontent; //检查内容
                                entity.checkobj = checkobj; //检查对象
                                //违章类型
                                if (!string.IsNullOrEmpty(lllegaltype))
                                {
                                    var typeList = itemlist.Where(p => p.EnCode == "LllegalType" && p.ItemName == lllegaltype);
                                    if (typeList.Count() > 0)
                                    {
                                        entity.lllegaltype = typeList.FirstOrDefault().ItemDetailId; //违章类型id
                                    }
                                }
                                //违章级别
                                if (!string.IsNullOrEmpty(lllegallevel))
                                {
                                    var levelList = itemlist.Where(p => p.EnCode == "LllegalLevel" && p.ItemName == lllegallevel);
                                    if (levelList.Count() > 0)
                                    {
                                        entity.lllegallevel = levelList.FirstOrDefault().ItemDetailId; //违章级别id
                                    }
                                }
                                //专业分类
                                if (!string.IsNullOrEmpty(majorclassify))
                                {
                                    var majorList = itemlist.Where(p => p.EnCode == "HidMajorClassify" && p.ItemName == majorclassify);
                                    if (majorList.Count() > 0)
                                    {
                                        entity.majorclassify = majorList.FirstOrDefault().ItemDetailId; //专业分类id
                                        entity.majorclassifyvalue = majorList.FirstOrDefault().ItemValue; //专业分类值
                                    }
                                }

                                //违章单位
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

                                    //违章人 
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
                                    else  //仅有部门
                                    {
                                        var dentity = deptlist.FirstOrDefault();
                                        if (null != dentity)
                                        {
                                            entity.lllegalteam = dentity.FullName;
                                            entity.lllegalteamcode = dentity.EnCode;
                                        }
                                    }
                                }

                                //整改责任单位
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

                                //违章责任单位
                                #region 违章责任单位
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
                                            //可门需求
                                            if (mode == "5")
                                            {
                                                //整改责任单位
                                                entity.reformdeptcode = dentity.EnCode;
                                                entity.reformdeptname = dentity.FullName;
                                                reformdeptname = entity.reformdeptname;

                                                //整改责任人为整改单位专业主管或安全员
                                                #region 专业主管
                                                if (!string.IsNullOrEmpty(entity.majorclassifyvalue))
                                                {
                                                    var zguserDt = htbaseinfobll.GetGeneralQueryBySql(string.Format(@"select * from v_userinfo where departmentcode = '{0}' and rolename like '%专工%' and  (','||specialtytype||',') like ',{1},' ", entity.reformdeptcode, entity.majorclassifyvalue));
                                                    if (zguserDt.Rows.Count > 0)
                                                    {
                                                        entity.reformpeopleid = zguserDt.Rows[0]["userid"].ToString();
                                                        entity.reformpeople = zguserDt.Rows[0]["realname"].ToString();
                                                        entity.reformtelephone = zguserDt.Rows[0]["mobile"].ToString();
                                                    }
                                                    else   //安全管理员
                                                    {
                                                        var aquserDt = htbaseinfobll.GetGeneralQueryBySql(string.Format(@"select * from v_userinfo where departmentcode = '{0}' and rolename like '%安全管理员%'  ", entity.reformdeptcode));
                                                        if (aquserDt.Rows.Count > 0)
                                                        {
                                                            entity.reformpeopleid = aquserDt.Rows[0]["userid"].ToString();
                                                            entity.reformpeople = aquserDt.Rows[0]["realname"].ToString();
                                                            entity.reformtelephone = aquserDt.Rows[0]["mobile"].ToString();
                                                        }
                                                    }
                                                }
                                                else   //安全管理员
                                                {
                                                    var aquserDt = htbaseinfobll.GetGeneralQueryBySql(string.Format(@"select userid,realname,mobile  from v_userinfo where departmentcode = '{0}' and rolename like '%安全管理员%'  ", entity.reformdeptcode));
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

                                //违章时间
                                if (!string.IsNullOrEmpty(lllegaltime))
                                {
                                    entity.lllegaltime = Convert.ToDateTime(lllegaltime); //违章时间
                                }
                                //可门
                                if (string.IsNullOrEmpty(lllegaltime) && mode == "5")
                                {
                                    lllegaltime = DateTime.Now.ToString("yyyy-MM-dd");
                                    entity.lllegaltime = Convert.ToDateTime(lllegaltime); //违章时间
                                }
                                //整改截止时间
                                if (!string.IsNullOrEmpty(reformdeadline))
                                {
                                    entity.reformdeadline = Convert.ToDateTime(reformdeadline); //整改截止时间
                                }
                                //违章描述
                                if (!string.IsNullOrEmpty(lllegaldescribe))
                                {
                                    entity.lllegaldescribe = lllegaldescribe; //违章描述
                                }
                                //整改措施
                                if (!string.IsNullOrEmpty(reformrequire))
                                {
                                    entity.reformrequire = reformrequire; //整改措施
                                }
                                //验收人 及 验收单位
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
                                //可门
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
                                        resultmessage += "验收人填写格式错误、";
                                        isadddobj = false;
                                    }
                                }
                                //违章单位考核金额
                                if (!string.IsNullOrEmpty(wzdwpunish))
                                {
                                    entity.wzdwpunish = wzdwpunish;
                                }
                                //责任单位考核金额
                                if (!string.IsNullOrEmpty(zrdwpunish))
                                {
                                    entity.zrdwpunish = zrdwpunish;
                                }
                                //验收日期
                                if (!string.IsNullOrEmpty(accepttime))
                                {
                                    entity.accepttime = Convert.ToDateTime(accepttime); //验收日期
                                }
                                #endregion

                                #region 必填验证
                                if (string.IsNullOrEmpty(entity.lllegalnumber))
                                {
                                    resultmessage += "违章编码为空、";
                                    isadddobj = false;
                                }
                                if (!string.IsNullOrEmpty(entity.lllegalnumber))
                                {
                                    //  if (Regex.IsMatch(str, "^(?<year>\\d{2,4})(?<month>\\d{1,2})(?<day>\\d{1,2})(\\d{4})?$"))
                                    if (!Regex.IsMatch(entity.lllegalnumber, "^(\\d{2,4})(\\d0[1-9]|1[0-2])((0[1-9])|(1[0-9])|(2[0-9])|30|31)(\\d{3})$"))
                                    {
                                        resultmessage += "违章编码格式验证失败、";
                                        isadddobj = false;
                                    }
                                }
                                if (string.IsNullOrEmpty(entity.lllegaltype))
                                {
                                    resultmessage += "违章类型为空、";
                                    isadddobj = false;
                                }
                                if (string.IsNullOrEmpty(entity.lllegallevel))
                                {
                                    resultmessage += "违章级别为空、";
                                    isadddobj = false;
                                }
                                if (string.IsNullOrEmpty(entity.majorclassify))
                                {
                                    resultmessage += "专业分类为空、";
                                    isadddobj = false;
                                }
                                if (string.IsNullOrEmpty(entity.lllegalteamcode))
                                {
                                    if (string.IsNullOrEmpty(lllegalteam))
                                    {
                                        resultmessage += "违章单位为空、";
                                    }
                                    else
                                    {
                                        resultmessage += "违章单位不存在于系统中、";
                                    }
                                    isadddobj = false;
                                }
                                if (string.IsNullOrEmpty(lllegaltime))
                                {
                                    resultmessage += "违章时间为空、";
                                    isadddobj = false;
                                }
                                if (string.IsNullOrEmpty(lllegaldescribe))
                                {
                                    resultmessage += "违章描述为空、";
                                    isadddobj = false;
                                }

                                if (mode == "5")
                                {
                                    if (string.IsNullOrEmpty(entity.lllegaldepartcode))
                                    {
                                        if (string.IsNullOrEmpty(lllegaldepart))
                                        {
                                            resultmessage += "违章责任单位为空、";
                                        }
                                        else
                                        {
                                            resultmessage += "违章责任单位不存在于系统中、";
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
                                            resultmessage += "违章责任单位对应的专业专工或安全管理员不存在、";
                                        }
                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(entity.lllegaldepartcode))
                                        {
                                            if (string.IsNullOrEmpty(reformdeptname))
                                            {
                                                resultmessage += "整改责任单位为空、";
                                            }
                                            else
                                            {
                                                if (string.IsNullOrEmpty(reformpeople))
                                                {
                                                    resultmessage += "整改责任人为空、";
                                                }
                                                else
                                                {
                                                    resultmessage += "整改责任单位或整改责任人不存在于系统中、";
                                                }
                                            }
                                        }
                                    }
                                    isadddobj = false;
                                }
                                if (string.IsNullOrEmpty(reformdeadline))
                                {
                                    resultmessage += "整改截止时间为空、";
                                    isadddobj = false;
                                }
                                if (string.IsNullOrEmpty(entity.acceptpeopleid) || string.IsNullOrEmpty(entity.acceptdeptcode))
                                {
                                    if (string.IsNullOrEmpty(acceptdeptname) && string.IsNullOrEmpty(mode))
                                    {
                                        resultmessage += "验收单位为空、";
                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(acceptpeople))
                                        {
                                            resultmessage += "验收人为空、";
                                        }
                                        else
                                        {
                                            resultmessage += "验收单位或验收人不存在于系统中、";
                                        }
                                    }
                                    isadddobj = false;
                                }

                                //获取已存在的隐患数据
                                var htlist = lllegalregisterbll.GetListByNumber(entity.lllegalnumber);

                                if (htlist.Count() > 0)
                                {
                                    //跳过操作
                                    if (repeatdata == "0")
                                    {
                                        resultmessage += "编号为" + entity.lllegalnumber + "的违章已存在，导入操作已忽略并跳过、";
                                        isadddobj = false;
                                    }
                                }

                                if (isadddobj)
                                {
                                    list.Add(entity);
                                }
                                else
                                {
                                    resultmessage = resultmessage.Substring(0, resultmessage.Length - 1) + ",无法正常导入";
                                    resultlist.Add(resultmessage);
                                }
                                #endregion
                            }
                            catch
                            {
                                resultmessage += "出现数据异常,无法正常导入";
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

                        #region 违章数据集合
                        foreach (ImportLllegal entity in list)
                        {
                            string keyValue = string.Empty;
                            int excuteVal = 0;
                            //违章基本信息
                            LllegalRegisterEntity baseentity = new LllegalRegisterEntity();

                            //获取已存在的隐患数据
                            var lllegallist = lllegalregisterbll.GetListByNumber(entity.lllegalnumber);

                            if (lllegallist.Count() > 0)
                            {
                                //覆盖操作
                                if (repeatdata == "1")
                                {
                                    var otherlllegal = lllegallist.Where(p => p.CREATEUSERID != curUser.UserId);
                                    //其他人创建的
                                    if (otherlllegal.Count() > 0)
                                    {
                                        falseMessage += "违章编码为'" + entity.lllegalnumber + "'的数据因已被" + otherlllegal.FirstOrDefault().CREATEUSERNAME + "创建而无法覆盖,不予操作</br>";
                                        excuteVal = -1;
                                    }
                                    else //自己创建
                                    {
                                        if (lllegallist.Where(p => p.RESEVERONE == checkid && p.APPSIGN == "Import").Count() > 0)
                                        {
                                            baseentity = lllegallist.Where(p => p.RESEVERONE == checkid && p.APPSIGN == "Import").FirstOrDefault();
                                            //先删除，后新增
                                            lllegalregisterbll.RemoveForm(baseentity.ID);
                                            baseentity = new LllegalRegisterEntity();
                                            excuteVal = 1;
                                        }
                                        else
                                        {
                                            falseMessage += "违章编码为'" + entity.lllegalnumber + "'的数据因已通过其他方式创建而无法覆盖,不予操作</br>";
                                            excuteVal = -1;
                                        }
                                    }
                                }
                                else  //跳过
                                {
                                    excuteVal = 0;
                                }
                            }
                            else
                            {
                                excuteVal = 1;
                            }

                            //日常检查无检查项目，直接关联检查记录   有检查项目及检查对象，导入无检查内容和检查对象，自动创建其他检查内容    有检查项目及检查对象，导入有检查内容未匹配成功，提示无法导入
                            //专项及其他检查，有检查项目，导入无检查内容和检查对象，自动创建其他检查内容    有检查项目及检查对象，导入有检查内容未匹配成功，提示无法导入

                            //存在检查项目
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
                                    falseMessage += "违章编码为'" + entity.lllegalnumber + "'的数据检查对象及检查内容未匹配或者当前检查项已被检查过或者该项属于其他人检查范围,无法正常导入</br>";
                                }
                                else
                                {
                                    listIds.Add(entity.resevertwo);
                                }
                            }

                            //是否执行
                            if (excuteVal > 0)
                            {
                                #region 违章信息保存
                                baseentity.ADDTYPE = "0"; //非立即整改违章
                                baseentity.APPSIGN = "Import"; //导入标记
                                baseentity.RESEVERONE = checkid; //检查记录id
                                baseentity.RESEVERID = entity.reseverid; //检查对象id
                                baseentity.RESEVERTWO = entity.resevertwo; //检查内容id
                                baseentity.BELONGDEPARTID = curUser.OrganizeId;
                                baseentity.BELONGDEPART = curUser.OrganizeName;
                                baseentity.CREATEDEPTID = curUser.DeptId;
                                baseentity.CREATEDEPTNAME = curUser.DeptName;
                                baseentity.LLLEGALNUMBER = entity.lllegalnumber; //违章编码 //lllegalregisterbll.GenerateHidCode("bis_lllegalregister", "lllegalnumber", int.Parse(lenNum));
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

                                #region 添加违章图片
                                if (!string.IsNullOrEmpty(hiddenDirectory))
                                {
                                    //当前文件夹存在
                                    if (Directory.Exists(hiddenDirectory))
                                    {
                                        //读取图片
                                        DirectoryInfo directinfo = new DirectoryInfo(hiddenDirectory);
                                        List<FileInfo> fileinfoes = GetFiles(directinfo, new List<FileInfo>());
                                        #region 图片文件
                                        foreach (FileInfo finfo in fileinfoes)
                                        {
                                            string fextension = finfo.Extension;//文件扩展名
                                            string fname = finfo.Name; //文件名称
                                            //过滤图片格式
                                            #region 过滤图片格式
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
                                                    //创建文件夹
                                                    string path = Path.GetDirectoryName(fullFileName);
                                                    Directory.CreateDirectory(path);
                                                    FileInfoEntity fileInfoEntity = new FileInfoEntity();
                                                    if (!System.IO.File.Exists(fullFileName))
                                                    {
                                                        //保存文件
                                                        finfo.CopyTo(fullFileName);
                                                    }
                                                    finfo.Delete();//删除文件
                                                    //文件信息写入数据库
                                                    fileInfoEntity.Create();
                                                    fileInfoEntity.FileId = fileGuid;
                                                    fileInfoEntity.RecId = baseentity.LLLEGALPIC; //关联ID
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

                                //用于考核使用
                                entity.lllegalid = baseentity.ID;
                                khlist.Add(entity);

                                if (string.IsNullOrEmpty(keyValue))
                                {
                                    string workFlow = "03";//违章处理
                                    bool isSucess = htworkflowbll.CreateWorkFlowObj(workFlow, baseentity.ID, curUser.UserId);
                                    if (isSucess)
                                    {
                                        bool res = lllegalregisterbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", baseentity.ID);  //更新业务流程状态
                                    }
                                }

                                #region 考核信息
                                //考核单位
                                //if (!string.IsNullOrEmpty(entity.wzdwpunish))
                                //{
                                //    if (Convert.ToDecimal(entity.wzdwpunish) > 0)
                                //    {
                                //        LllegalPunishEntity newpunishEntity = new LllegalPunishEntity();
                                //        newpunishEntity.LLLEGALID = baseentity.ID;
                                //        newpunishEntity.ASSESSOBJECT = "考核单位"; //考核对象
                                //        newpunishEntity.PERSONINCHARGEID = entity.lllegalteamcode;
                                //        newpunishEntity.PERSONINCHARGENAME = entity.lllegalteam;
                                //        newpunishEntity.PERFORMANCEPOINT = 0;
                                //        newpunishEntity.ECONOMICSPUNISH = Convert.ToDecimal(entity.wzdwpunish);
                                //        newpunishEntity.EDUCATION = 0;
                                //        newpunishEntity.LLLEGALPOINT = 0;
                                //        newpunishEntity.AWAITJOB = 0;
                                //        newpunishEntity.MARK = newpunishEntity.ASSESSOBJECT.Contains("考核") ? "0" : "1"; //标记0考核，1联责
                                //        lllegalpunishbll.SaveForm("", newpunishEntity);
                                //    }
                                //}

                                ////责任单位
                                //if (!string.IsNullOrEmpty(entity.zrdwpunish))
                                //{
                                //    if (Convert.ToDecimal(entity.zrdwpunish) > 0)
                                //    {
                                //        LllegalPunishEntity newpunishEntity = new LllegalPunishEntity();
                                //        newpunishEntity.LLLEGALID = baseentity.ID;
                                //        newpunishEntity.ASSESSOBJECT = "第一联责单位"; //考核对象
                                //        newpunishEntity.PERSONINCHARGEID = entity.lllegaldepartcode;
                                //        newpunishEntity.PERSONINCHARGENAME = entity.lllegaldepart;
                                //        newpunishEntity.PERFORMANCEPOINT = 0;
                                //        newpunishEntity.ECONOMICSPUNISH = Convert.ToDecimal(entity.zrdwpunish);
                                //        newpunishEntity.EDUCATION = 0;
                                //        newpunishEntity.LLLEGALPOINT = 0;
                                //        newpunishEntity.AWAITJOB = 0;
                                //        newpunishEntity.MARK = newpunishEntity.ASSESSOBJECT.Contains("考核") ? "0" : "1"; //标记0考核，1联责
                                //        lllegalpunishbll.SaveForm("", newpunishEntity);
                                //    }
                                //}
                                #endregion


                                //整改基本信息
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
                                //验收基本信息
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

                        #region 违章考核部分
                        if (wb.Worksheets.Count == 3 || seconddt.Columns.Contains("违章编码"))//违章考核信息导入
                        {
                            List<ImportLllegalExamin> examlist = new List<ImportLllegalExamin>();
                            int khIndex = 1;
                            foreach (DataRow khrow in seconddt.Rows)
                            {
                                ImportLllegalExamin examentity = new ImportLllegalExamin();
                                int errornum = 0;
                                string resultmessage = "违章考核导入表中第" + khIndex.ToString() + "行"; //显示错误
                                //违章编码
                                string lllegalnumber = seconddt.Columns.Contains("违章编码") ? khrow["违章编码"].ToString().Trim() : "";
                                if (string.IsNullOrEmpty(lllegalnumber))
                                {
                                    lllegalnumber = seconddt.Columns.Contains("Column1") ? khrow["Column1"].ToString().Trim() : "";
                                }
                                //考核对象
                                string assessobject = seconddt.Columns.Contains("考核对象") ? khrow["考核对象"].ToString().Trim() : "";
                                if (string.IsNullOrEmpty(assessobject))
                                {
                                    assessobject = seconddt.Columns.Contains("Column2") ? khrow["Column2"].ToString().Trim() : "";
                                }
                                //考核人员/单位
                                string personinchargename = seconddt.Columns.Contains("考核人员/单位") ? khrow["考核人员/单位"].ToString().Trim() : "";
                                if (string.IsNullOrEmpty(personinchargename))
                                {
                                    personinchargename = seconddt.Columns.Contains("Column3") ? khrow["Column3"].ToString().Trim() : "";
                                }
                                //经济处罚(元) 
                                string economicspunish = seconddt.Columns.Contains("经济处罚(元)") ? khrow["经济处罚(元)"].ToString().Trim() : "0";
                                //违章扣分
                                string lllegalpoint = seconddt.Columns.Contains("违章扣分(分)") ? khrow["违章扣分(分)"].ToString().Trim() : "0";
                                //教育培训(学时)
                                string education = seconddt.Columns.Contains("教育培训(学时)") ? khrow["教育培训(学时)"].ToString().Trim() : "0";
                                //待岗(月)
                                string awaitjob = seconddt.Columns.Contains("待岗(月)") ? khrow["待岗(月)"].ToString().Trim() : "0";
                                //EHS绩效考核(分)
                                string performancepoint = seconddt.Columns.Contains("EHS绩效考核(分)") ? khrow["EHS绩效考核(分)"].ToString().Trim() : "0";
                                //奖励人员
                                string awardusername = seconddt.Columns.Contains("奖励人员") ? khrow["奖励人员"].ToString().Trim() : "";
                                if (string.IsNullOrEmpty(awardusername))
                                {
                                    awardusername = seconddt.Columns.Contains("Column9") ? khrow["Column9"].ToString().Trim() : "";
                                }
                                //奖励积分(分)
                                string awardpoint = seconddt.Columns.Contains("奖励积分(分)") ? khrow["奖励积分(分)"].ToString().Trim() : "0";
                                //奖励金额(元)
                                string awardmoney = seconddt.Columns.Contains("奖励金额(元)") ? khrow["奖励金额(元)"].ToString().Trim() : "0";

                                examentity.lllegalnumber = lllegalnumber;
                                examentity.assessobject = assessobject;
                                #region 考核对象
                                if (!string.IsNullOrEmpty(personinchargename))
                                {
                                    if (!string.IsNullOrEmpty(examentity.assessobject))
                                    {
                                        #region 考核人员
                                        if (examentity.assessobject.Contains("人员"))
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
                                        #region 考核部门
                                        else if (examentity.assessobject.Contains("部门"))
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

                                #region 奖励对象
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
                                            resultmessage += "考核对象未选择、奖励人员未填写、";
                                            errornum += 1;
                                        }
                                        else
                                        {
                                            //考核对象不为空
                                            if (!string.IsNullOrEmpty(assessobject))
                                            {
                                                //考核单位
                                                if (assessobject.Contains("部门") && string.IsNullOrEmpty(examentity.personinchargeid))
                                                {
                                                    resultmessage += assessobject + "填写错误或不存在、";
                                                    errornum += 1;
                                                }
                                            }
                                            if (!string.IsNullOrEmpty(awardusername))
                                            {
                                                if (string.IsNullOrEmpty(examentity.awarduserid))
                                                {
                                                    resultmessage += "奖励人员填写错误或不存在、";
                                                    errornum += 1;
                                                }
                                            }
                                        }

                                        if ((examentity.assessobject.Contains("部门") && !string.IsNullOrEmpty(examentity.personinchargeid)) || 
                                            (examentity.assessobject.Contains("人员") && !string.IsNullOrEmpty(examentity.personinchargename)) || 
                                            (!string.IsNullOrEmpty(examentity.awarduserid) &&!string.IsNullOrEmpty(examentity.awarddeptid)) )
                                        {
                                            var wzObject = khlist.Where(p => p.lllegalnumber == examentity.lllegalnumber).FirstOrDefault();
                                            examentity.lllegalid = wzObject.lllegalid;
                                            examlist.Add(examentity);
                                        }
                                    }
                                    else
                                    {
                                        resultmessage += "违章编码未关联违章导入表中的违章、";
                                    }
                                }
                                else
                                {
                                    resultmessage += "违章编码未填写、";
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

                            //覆盖操作
                            if (repeatdata == "1")
                            {
                                List<string> khids = new List<string>();
                                List<string> jlids = new List<string>();
                                foreach (ImportLllegalExamin newexam in examlist)
                                {
                                    if ((newexam.assessobject.Contains("部门") && !string.IsNullOrEmpty(newexam.personinchargeid)) ||
                                   (newexam.assessobject.Contains("人员") && !string.IsNullOrEmpty(newexam.personinchargename)))
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
                                //删除考核
                                foreach (string strId in khids)
                                {
                                    //先删除关联考核集合
                                    lllegalpunishbll.DeleteLllegalPunishList(strId, "");
                                }
                                //删除奖励
                                foreach (string strId in jlids)
                                {
                                    //先删除关联奖励集合
                                    lllegalawarddetailbll.DeleteLllegalAwardList(strId);
                                }
                                int exmaIndex = 0;
                                int awardIndex = 0;
                                foreach (ImportLllegalExamin newexam in examlist)
                                {
                               
                                    if ((newexam.assessobject.Contains("部门") && !string.IsNullOrEmpty(newexam.personinchargeid)) ||
                                        (newexam.assessobject.Contains("人员") && !string.IsNullOrEmpty(newexam.personinchargename)))
                                    {
                                        exmaIndex++;
                                        LllegalPunishEntity newpunishEntity = new LllegalPunishEntity();
                                        newpunishEntity.LLLEGALID = newexam.lllegalid;
                                        newpunishEntity.ASSESSOBJECT = newexam.assessobject; //考核对象
                                        newpunishEntity.PERSONINCHARGEID = newexam.personinchargeid;
                                        newpunishEntity.PERSONINCHARGENAME = newexam.personinchargename;
                                        newpunishEntity.PERFORMANCEPOINT = !string.IsNullOrEmpty(newexam.performancepoint) ? Convert.ToDecimal(newexam.performancepoint) : 0;
                                        newpunishEntity.ECONOMICSPUNISH = !string.IsNullOrEmpty(newexam.economicspunish) ? Convert.ToDecimal(newexam.economicspunish) : 0;
                                        newpunishEntity.EDUCATION = !string.IsNullOrEmpty(newexam.education) ? Convert.ToDecimal(newexam.education) : 0;
                                        newpunishEntity.LLLEGALPOINT = !string.IsNullOrEmpty(newexam.lllegalpoint) ? Convert.ToDecimal(newexam.lllegalpoint) : 0;
                                        newpunishEntity.AWAITJOB = !string.IsNullOrEmpty(newexam.awaitjob) ? Convert.ToDecimal(newexam.awaitjob) : 0;
                                        newpunishEntity.MARK = newpunishEntity.ASSESSOBJECT.Contains("考核") ? "0" : "1"; //标记0考核，1联责
                                        lllegalpunishbll.SaveForm("", newpunishEntity);
                                       
                                    }

                                    if (!string.IsNullOrEmpty(newexam.awarduserid) && !string.IsNullOrEmpty(newexam.awarddeptid))
                                    {
                                        awardIndex++;
                                        LllegalAwardDetailEntity awardEntity = new LllegalAwardDetailEntity();
                                        awardEntity.LLLEGALID = newexam.lllegalid;
                                        awardEntity.USERID = newexam.awarduserid; //奖励对象
                                        awardEntity.USERNAME = newexam.awardusername;
                                        awardEntity.DEPTID = newexam.awarddeptid;
                                        awardEntity.DEPTNAME = newexam.awarddeptname;
                                        awardEntity.POINTS = !string.IsNullOrEmpty(newexam.awardpoint) ? int.Parse(newexam.awardpoint) : 0;
                                        awardEntity.MONEY = !string.IsNullOrEmpty(newexam.awardmoney) ? Convert.ToDecimal(newexam.awardmoney) : 0;
                                        lllegalawarddetailbll.SaveForm("", awardEntity);
                                    }
                                }
                                childmessage = ",其中违章考核表共有" + seconddt.Rows.Count.ToString() + "条记录,成功导入违章考核" + exmaIndex.ToString() + "条，违章奖励" + exmaIndex.ToString() + "条.";
                            }
                        }
                        #endregion
                    }
                    #endregion
                    #region 问题部分
                    else if (sheets.Name.Contains("问题") || dt.Columns.Contains("问题描述"))//问题信息导入
                    {
                        bool isHavaWorkFlow = htworkflowbll.IsHaveCurWorkFlow("厂级问题流程");

                        #region 对象装载
                        List<ImportQuestion> list = new List<ImportQuestion>();
                        //先获取到职务列表;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string resultmessage = "第" + (i + 1).ToString() + "行数据"; //显示错误

                            bool isadddobj = true;

                            //问题编码
                            string questionnumber = dt.Columns.Contains("问题编码") ? dt.Rows[i]["问题编码"].ToString().Trim() : string.Empty;
                            //问题描述
                            string questiondescribe = dt.Columns.Contains("问题描述") ? dt.Rows[i]["问题描述"].ToString().Trim() : string.Empty;
                            //问题地点
                            string questionaddress = dt.Columns.Contains("问题地点") ? dt.Rows[i]["问题地点"].ToString().Trim() : string.Empty;
                            //检查重点内容
                            string checkimpcontent = dt.Columns.Contains("检查重点内容") ? dt.Rows[i]["检查重点内容"].ToString().Trim() : string.Empty;
                            //检查对象
                            string checkobj = dt.Columns.Contains("检查对象") ? dt.Rows[i]["检查对象"].ToString().Trim() : string.Empty;
                            //检查内容
                            string checkcontent = dt.Columns.Contains("检查内容") ? dt.Rows[i]["检查内容"].ToString().Trim() : string.Empty;
                            //整改责任人
                            string reformpeoplename = dt.Columns.Contains("整改责任人") ? dt.Rows[i]["整改责任人"].ToString().Trim() : string.Empty;
                            //整改责任人电话
                            string telephone = dt.Columns.Contains("整改责任人电话") ? dt.Rows[i]["整改责任人电话"].ToString().Trim() : string.Empty;
                            //整改责任单位
                            string reformdeptname = dt.Columns.Contains("整改责任单位") ? dt.Rows[i]["整改责任单位"].ToString().Trim() : string.Empty;
                            //联责单位
                            string dutydeptname = dt.Columns.Contains("联责单位") ? dt.Rows[i]["联责单位"].ToString().Trim() : string.Empty;
                            //计划完成日期
                            string reformplandate = dt.Columns.Contains("计划完成日期") ? dt.Rows[i]["计划完成日期"].ToString().Trim() : string.Empty;
                            //整改措施
                            string reformmeasure = dt.Columns.Contains("整改措施") ? dt.Rows[i]["整改措施"].ToString().Trim() : string.Empty;
                            //验证人
                            string verifypeoplename = dt.Columns.Contains("验证人") ? dt.Rows[i]["验证人"].ToString().Trim() : string.Empty;
                            //验证单位
                            string verifydeptname = dt.Columns.Contains("验证单位") ? dt.Rows[i]["验证单位"].ToString().Trim() : string.Empty;
                            //验证日期
                            string verifydate = dt.Columns.Contains("验证日期") ? dt.Rows[i]["验证日期"].ToString().Trim() : string.Empty;

                            string relevanceid = string.Empty;
                            try
                            {
                                #region 对象集合
                                ImportQuestion entity = new ImportQuestion();
                                //序号
                                entity.serialnumber = i + 1; //序号
                                entity.questionnumber = questionnumber; //问题编码
                                entity.checkimpcontent = checkimpcontent; //检查重点内容
                                entity.checkobj = checkobj; //检查对象
                                entity.checkcontent = checkcontent; //检查内容
                                //整改责任单位
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
                                //联责单位
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

                                #region 整改责任人
                                bool reformWarn = true;
                                if (!string.IsNullOrEmpty(reformpeoplename))
                                {
                                    List<UserEntity> reformuserlist = ulist;
                                    //整改责任单位 //联责单位
                                    if (!string.IsNullOrEmpty(entity.reformdeptid) || !string.IsNullOrEmpty(entity.dutydeptid))
                                    {
                                        reformuserlist = reformuserlist.Where(p => p.DepartmentId == entity.reformdeptid || p.DepartmentId == entity.dutydeptid).ToList();
                                    }

                                    string[] reformpeoples = new string[] { };

                                    if (reformpeoplename.Split(',').Length > 0)
                                    {
                                        reformpeoples = reformpeoplename.Split(',');
                                    }
                                    else if (reformpeoplename.Split('，').Length > 0)
                                    {
                                        reformpeoples = reformpeoplename.Split('，');
                                    }
                                    else if (reformpeoplename.Split('、').Length > 0)
                                    {
                                        reformpeoples = reformpeoplename.Split('、');
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

                                //计划完成日期
                                if (!string.IsNullOrEmpty(reformplandate))
                                {
                                    entity.reformplandate = Convert.ToDateTime(reformplandate); //整改截止时间
                                }
                                //问题地点
                                if (!string.IsNullOrEmpty(questionaddress))
                                {
                                    entity.questionaddress = questionaddress; //问题地点
                                }
                                //问题描述
                                if (!string.IsNullOrEmpty(questiondescribe))
                                {
                                    entity.questiondescribe = questiondescribe; //问题描述
                                }
                                //整改措施
                                if (!string.IsNullOrEmpty(reformmeasure))
                                {
                                    entity.reformmeasure = reformmeasure; //整改措施
                                }
                                //西塞山自动跳过
                                if (mode != "8")
                                {
                                    #region 验证人
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
                                //验证日期
                                if (!string.IsNullOrEmpty(verifydate))
                                {
                                    entity.verifydate = Convert.ToDateTime(verifydate); //验证日期
                                }
                                #endregion

                                #region 必填验证
                                if (string.IsNullOrEmpty(questionnumber))
                                {
                                    resultmessage += "问题编码为空、";
                                    isadddobj = false;
                                }
                                if (!string.IsNullOrEmpty(questionnumber))
                                {
                                    if (questionnumber.Length == 13 || questionnumber.Length == 14)
                                    {
                                        //AQ2020第11期0001
                                        if (questionnumber.Substring(0, 2) == "AQ" && questionnumber.Substring(6, 1) == "第" && (questionnumber.Substring(8, 1) == "期" || questionnumber.Substring(9, 1) == "期"))
                                        {
                                            isadddobj = true;
                                        }
                                        else
                                        {
                                            resultmessage += "问题编码格式验证失败、";
                                            isadddobj = false;
                                        }
                                    }
                                    else
                                    {
                                        resultmessage += "问题编码格式验证失败、";
                                        isadddobj = false;
                                    }
                                    ////  if (Regex.IsMatch(str, "^(?<year>\\d{2,4})(?<month>\\d{1,2})(?<day>\\d{1,2})(\\d{4})?$"))
                                    //if (!Regex.IsMatch(questionnumber, "AQ^(\\d{2,4})(\\d0[1-9]|1[0-2])((0[1-9])|(1[0-9])|(2[0-9])|30|31)(\\d{4})$"))
                                    //{
                                    //    resultmessage += "隐患编码格式验证失败、";
                                    //    isadddobj = false;
                                    //}
                                }
                                if (string.IsNullOrEmpty(questiondescribe))
                                {
                                    resultmessage += "问题描述为空、";
                                    isadddobj = false;
                                }
                                if (string.IsNullOrEmpty(reformpeoplename))
                                {
                                    resultmessage += "整改责任人为空、";
                                    isadddobj = false;
                                }
                                else
                                {
                                    if (reformWarn)
                                    {
                                        resultmessage += "整改责任人中有人员填写错误或不存在于整改责任单位(联责单位)、";
                                        isadddobj = false;
                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(entity.reformpeople))
                                        {
                                            resultmessage += "整改责任人填写错误或不存在、";
                                            isadddobj = false;
                                        }
                                    }
                                }

                                if (string.IsNullOrEmpty(reformdeptname))
                                {
                                    resultmessage += "整改责任单位为空、";
                                    isadddobj = false;
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(entity.reformdeptid))
                                    {
                                        resultmessage += "整改责任单位填写错误或不存在、";
                                        isadddobj = false;
                                    }
                                }

                                if (string.IsNullOrEmpty(reformmeasure))
                                {
                                    resultmessage += "整改措施为空、";
                                    isadddobj = false;
                                }
                                if (string.IsNullOrEmpty(reformplandate))
                                {
                                    resultmessage += "计划完成日期为空、";
                                    isadddobj = false;
                                }

                                if (isadddobj)
                                {
                                    list.Add(entity);
                                }
                                else
                                {
                                    resultmessage = resultmessage.Substring(0, resultmessage.Length - 1) + ",无法正常导入";
                                    resultlist.Add(resultmessage);
                                }
                                #endregion
                            }
                            catch
                            {
                                resultmessage += "出现数据异常,无法正常导入";
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
                        #region 问题数据集合
                        foreach (ImportQuestion entity in list)
                        {
                            string keyValue = string.Empty;
                            int excuteVal = 0;
                            //问题基本信息
                            QuestionInfoEntity baseentity = new QuestionInfoEntity();

                            //获取已存在的问题数据
                            var questionlist = questioninfobll.GetListByNumber(entity.questionnumber);

                            if (questionlist.Count() > 0)
                            {
                                //覆盖操作
                                if (repeatdata == "1")
                                {
                                    var otherQuestion = questionlist.Where(p => p.CREATEUSERID != curUser.UserId);
                                    //其他人创建的
                                    if (otherQuestion.Count() > 0)
                                    {
                                        falseMessage += "问题编码为'" + entity.questionnumber + "'的数据因已被" + otherQuestion.FirstOrDefault().CREATEUSERNAME + "创建而无法覆盖,不予操作</br>";
                                        excuteVal = -1;
                                    }
                                    else //自己创建
                                    {
                                        if (questionlist.Where(p => p.CHECKID == checkid && p.APPSIGN == "Import").Count() > 0)
                                        {
                                            baseentity = questionlist.Where(p => p.CHECKID == checkid && p.APPSIGN == "Import").FirstOrDefault();
                                            //先删除，后新增
                                            questioninfobll.RemoveForm(baseentity.ID);
                                            baseentity = new QuestionInfoEntity();
                                            excuteVal = 1;
                                        }
                                        else
                                        {
                                            falseMessage += "问题编码为'" + entity.questionnumber + "'的数据因已通过其他方式创建而无法覆盖,不予操作</br>";
                                            excuteVal = -1;
                                        }
                                    }
                                }
                                else  //跳过
                                {
                                    excuteVal = 0;
                                }
                            }
                            else
                            {
                                excuteVal = 1;
                            }

                            //日常检查无检查项目，直接关联检查记录   有检查项目及检查对象，导入无检查内容和检查对象，自动创建其他检查内容    有检查项目及检查对象，导入有检查内容未匹配成功，提示无法导入
                            //专项及其他检查，有检查项目，导入无检查内容和检查对象，自动创建其他检查内容    有检查项目及检查对象，导入有检查内容未匹配成功，提示无法导入

                            //存在检查项目
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
                                    falseMessage += "问题编码为'" + entity.questionnumber + "'的数据检查对象及检查内容未匹配或者当前检查项已被检查过或者该项属于其他人检查范围,无法正常导入</br>";
                                }
                                else
                                {
                                    listIds.Add(entity.correlationid);
                                }
                            }
                          

                            //是否成功执行
                            if (excuteVal > 0)
                            {
                                baseentity.APPSIGN = "Import"; //导入标记
                                baseentity.CHECKID = checkid; //检查记录id
                                baseentity.QUESTIONNUMBER = entity.questionnumber;//问题编码
                                baseentity.CHECKCONTENT = entity.checkimpcontent; //检查重点内容
                                baseentity.RELEVANCEID = entity.relevanceid; //检查对象id
                                baseentity.CORRELATIONID = entity.correlationid; //检查内容id
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
                                baseentity.CHECKDATE = DateTime.Now; //检查时间
                                baseentity.QUESTIONADDRESS = entity.questionaddress; //问题地点
                                baseentity.QUESTIONDESCRIBE = entity.questiondescribe;//问题描述
                                baseentity.QUESTIONPIC = Guid.NewGuid().ToString();
                                #region 添加问题图片
                                if (!string.IsNullOrEmpty(hiddenDirectory))
                                {
                                    //当前文件夹存在
                                    if (Directory.Exists(hiddenDirectory))
                                    {
                                        //读取图片
                                        DirectoryInfo directinfo = new DirectoryInfo(hiddenDirectory);
                                        List<FileInfo> fileinfoes = GetFiles(directinfo, new List<FileInfo>());
                                        #region 图片文件
                                        foreach (FileInfo finfo in fileinfoes)
                                        {
                                            string fextension = finfo.Extension;//文件扩展名
                                            string fname = finfo.Name; //文件名称
                                            //过滤图片格式
                                            #region 过滤图片格式
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
                                                    //创建文件夹
                                                    string path = Path.GetDirectoryName(fullFileName);
                                                    Directory.CreateDirectory(path);
                                                    FileInfoEntity fileInfoEntity = new FileInfoEntity();
                                                    if (!System.IO.File.Exists(fullFileName))
                                                    {
                                                        //保存文件
                                                        finfo.CopyTo(fullFileName);
                                                    }
                                                    finfo.Delete();//删除文件
                                                    //文件信息写入数据库
                                                    fileInfoEntity.Create();
                                                    fileInfoEntity.FileId = fileGuid;
                                                    fileInfoEntity.RecId = baseentity.QUESTIONPIC; //关联ID
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
                                string workFlow = "09";//问题处理
                                bool isSucess = htworkflowbll.CreateWorkFlowObj(workFlow, baseentity.ID, curUser.UserId);
                                if (isSucess)
                                {
                                    htworkflowbll.UpdateFlowStateByObjectId("bis_questioninfo", "flowstate", baseentity.ID);  //更新业务流程状态
                                }
                                //整改基本信息
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

                                //当前没有配置流程，则新加验证信息
                                if (!isHavaWorkFlow)
                                {
                                    //验证基本信息
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
                                falseMessage += "问题编码为" + entity.questionnumber + "的数据因问题编码重复而自动跳过,不予操作</br>";
                            }
                        }
                        #endregion
                    }
                    #endregion
                    count = dt.Rows.Count;
                    message = "共有" + count.ToString() + "条记录,成功导入" + total.ToString() + "条,失败" + (count - total).ToString() + "条" + childmessage;
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
        /// 读取文件
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

        #region 获取隐患排查指标相关数据
        /// <summary>
        /// 获取隐患排查指标相关数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult GetHiddenInfoOfWarning(string time = "")
        {
            Operator curUser = OperatorProvider.Provider.Current();
            string startDate = string.Empty;
            string endDate = string.Empty;
            //不为空
            if (!string.IsNullOrEmpty(time))
            {
                startDate = time;
                endDate = Convert.ToDateTime(time).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                var josnData = htbaseinfobll.GetHiddenInfoOfEveryMonthWarning(curUser, startDate, endDate);
                return Content(josnData.ToJson());
            }
            else
            {
                startDate = DateTime.Now.Year.ToString() + "-01" + "-01";  //从每年的一月一号计算起
                endDate = DateTime.Now.ToString("yyyy-MM-dd");
                var josnData = htbaseinfobll.GetHiddenInfoOfWarning(curUser, startDate, endDate);
                return Content(josnData.ToJson());
            }
        }
        #endregion

        #region 导出隐患基本信息
        /// <summary>
        /// 导出隐患基本信息
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
            queryJson = queryJson.Insert(1, "\"userId\":\"" + curUser.UserId + "\","); //添加当前用户
            string userId = curUser.UserId;
            string p_fields = string.Empty;
            string p_fieldsName = string.Empty;
            try
            {
                //系统默认的列表设置
                var defaultdata = modulelistcolumnauthbll.GetEntity(currentModuleId, "", 0);
                if (null != defaultdata)
                {
                    p_fields = defaultdata.DEFAULTCOLUMNFIELDS;
                    p_fieldsName = defaultdata.DEFAULTCOLUMNNAME;
                }
                //当前用户的列表设置
                var data = modulelistcolumnauthbll.GetEntity(currentModuleId, curUser.UserId, 1);
                //为空，自动读取系统默认
                if (null != data)
                {
                    p_fields = data.DEFAULTCOLUMNFIELDS;
                    p_fieldsName = data.DEFAULTCOLUMNNAME;
                }
                //将隐患类型进行替换为换算之后的类型
                p_fields = "workstream," + p_fields + ",participantname";
                p_fieldsName = "流程状态," + p_fieldsName + ",流程处理人";

                pagination.p_fields = p_fields + ",hiddentypename";
                //取出数据源
                DataTable exportTable = htbaseinfobll.GetHiddenBaseInfoPageList(pagination, queryJson);
                exportTable.Columns.Remove("id");
                exportTable.Columns["r"].SetOrdinal(0);

                // 详细列表内容
                string fielname = fileName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                wb.Open(Server.MapPath("~/Resource/ExcelTemplate/tmp.xls"));
                Aspose.Cells.Worksheet sheet = wb.Worksheets[0] as Aspose.Cells.Worksheet;

                Aspose.Cells.Cell cell = sheet.Cells[0, 0];
                cell.PutValue("隐患排查基本信息"); //标题
                cell.Style.Pattern = BackgroundType.Solid;
                cell.Style.Font.Size = 14;
                cell.Style.Font.Color = Color.Black;

                //动态加列
                int colLength = 0;
                if (!string.IsNullOrEmpty(p_fieldsName))
                {
                    //再设置相关行列
                    string[] p_filedsNameArray = p_fieldsName.Split(',');
                    colLength = p_filedsNameArray.Length + 1;
                    //序号列
                    Aspose.Cells.Cell serialcell = sheet.Cells[1, 0];
                    serialcell.PutValue("序号"); //填报单位

                    for (int i = 0; i < p_filedsNameArray.Length; i++)
                    {
                        Aspose.Cells.Cell curcell = sheet.Cells[1, i + 1];
                        //行业版本
                        if (curUser.Industry != "电力" && !string.IsNullOrEmpty(curUser.Industry) && p_filedsNameArray[i] == "专业分类")
                        {
                            p_filedsNameArray[i] = "隐患分类";
                        }
                        curcell.PutValue(p_filedsNameArray[i].ToString()); //列头
                    }
                    //合并单元格
                    Aspose.Cells.Cells cells = sheet.Cells;
                    cells.Merge(0, 0, 1, p_filedsNameArray.Length);
                }

                //需求垃圾，User Go Die 。。。。。。。。 改GJB
                var typedata = dataitemdetailbll.GetDataItemListByItemCode("'HidType'").Where(p => p.ItemCode == "0").ToList();
                int rowIndex = 2;

                Aspose.Cells.Style style = wb.Styles[wb.Styles.Add()];
                style.ForegroundColor = System.Drawing.Color.FromArgb(153, 204, 0);
                style.Pattern = Aspose.Cells.BackgroundType.Solid;
                style.Font.IsBold = true;
                //字段数组
                string[] p_fieldsarrayObj = ("r," + p_fields).Split(',');
                //遍历行
                foreach (DataItemModel model in typedata)
                {
                    string hidtypename = model.ItemName;
                    Aspose.Cells.Cell typecell = sheet.Cells[rowIndex, 0];
                    typecell.Style.HorizontalAlignment = TextAlignmentType.Left; //设置文字偏左
                    typecell.PutValue(hidtypename); //隐患类别
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
                                    colrowcell.PutValue(row[i].ToString()); //填充内容值
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
            return Success("导出成功!");
        }
        #endregion

        #region 导出一览表
        /// <summary>
        /// 导出一览表
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
            if (curUser.RoleName.Contains("公司级") || curUser.RoleName.Contains("厂级")) { curUser.isPlanLevel = "1"; }
            queryJson = queryJson.Insert(1, "\"userId\":\"" + userId + "\","); //添加当前用户
            queryJson = queryJson.Insert(1, "\"isPlanLevel\":\"" + curUser.isPlanLevel + "\","); //添加当前是否公司及厂级
            try
            {
                DesktopBLL dbll = new DesktopBLL();
                if (dbll.IsGeneric())
                {
                    //如果不是电厂则不导出专业分类和厂级监控人员
                    pagination.p_fields =
                        @"hidcode,hiddescribe,hidrankname,changedeadine, 
                  (case when workstream ='隐患验收' or workstream ='整改效果评估' or workstream ='整改结束' then '是' else '否' end) isclose,'' curstatus";
                }
                else
                {
                    pagination.p_fields =
                        @"hidcode,hiddescribe,hidrankname,majorclassifyname,monitorpersonname,changedeadine, 
                  (case when workstream ='隐患验收' or workstream ='整改效果评估' or workstream ='整改结束' then '是' else '否' end) isclose,'' curstatus";
                }

                //取出数据源
                DataTable exportTable = htbaseinfobll.GetHiddenBaseInfoPageList(pagination, queryJson);
                exportTable.Columns.Remove("curapprovedate");
                exportTable.Columns.Remove("curacceptdate");
                exportTable.Columns.Remove("beforeapprovedate");
                exportTable.Columns.Remove("beforeacceptdate");
                exportTable.Columns.Remove("afterapprovedate");
                exportTable.Columns.Remove("afteracceptdate");
                exportTable.Columns.Remove("id");
                exportTable.Columns["r"].SetOrdinal(0);
                // 确定导出文件名
                string fileName = "事故隐患排查治理情况一览表";

                // 详细列表内容
                string fielname = fileName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                wb.Open(Server.MapPath("~/Resource/ExcelTemplate/事故隐患排查治理情况一览表.xlsx"));
                Aspose.Cells.Worksheet sheet = wb.Worksheets[0] as Aspose.Cells.Worksheet;
                Aspose.Cells.Cell cell = sheet.Cells[1, 1];

                cell.PutValue(curUser.DeptName); //填报单位
                Aspose.Cells.Cell endcell = sheet.Cells[1, 8];
                endcell.PutValue(DateTime.Now.ToString("yyyy-MM-dd")); //填报单位


                int result = dataitemdetailbll.GetDataItemListByItemCode("'IsEnableMinimalistMode'").Where(p => p.ItemValue == curUser.OrganizeId).Count();
                if (result > 0)
                {
                    Aspose.Cells.Cell describeCell = sheet.Cells[2, 2]; //
                    describeCell.PutValue("隐患内容"); //隐患内容
                }
                string JLIndex = dataitemdetailbll.GetItemValue("JLIndex"); //华电江陵版配置
                if (!string.IsNullOrEmpty(JLIndex))
                {
                    Aspose.Cells.Cell describeCell = sheet.Cells[2, 2]; //
                    describeCell.PutValue("隐患描述"); //隐患描述
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
            return Success("导出成功!");
        }

        #endregion

        #region 导出隐患治理台账表
        /// <summary>
        /// 导出隐患治理台账表
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
            if (curUser.RoleName.Contains("公司级") || curUser.RoleName.Contains("厂级")) { curUser.isPlanLevel = "1"; }
            queryJson = queryJson.Insert(1, "\"userId\":\"" + userId + "\","); //添加当前用户
            queryJson = queryJson.Insert(1, "\"isPlanLevel\":\"" + curUser.isPlanLevel + "\","); //添加当前是否公司及厂级
            DesktopBLL dbll = new DesktopBLL();
            try
            {
                string p_fields = string.Empty;
                pagination.p_fields = @" (hidpointname||'/'||hidplace) checkarea,checkdate,hiddescribe,hidrankname,checkmanname,changemeasure,planmanagecapital,changedeadine,changedutydepartname,changepersonname,changeresult,acceptpersonname,acceptdate,hidbasefilepath,reformfilepath";

                //取出数据源
                DataTable exportTable = htbaseinfobll.GetHiddenBaseInfoPageList(pagination, queryJson);
                exportTable.Columns.Remove("curapprovedate");
                exportTable.Columns.Remove("curacceptdate");
                exportTable.Columns.Remove("beforeapprovedate");
                exportTable.Columns.Remove("beforeacceptdate");
                exportTable.Columns.Remove("afterapprovedate");
                exportTable.Columns.Remove("afteracceptdate");
                exportTable.Columns.Remove("id");
                // 确定导出文件名
                string fileName = "隐患排查治理台账表";
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
                // 详细列表内容
                string fielname = fileName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                wb.Open(Server.MapPath("~/Resource/ExcelTemplate/隐患排查治理台账模板.xls"));
                Aspose.Cells.Worksheet sheet = wb.Worksheets[0] as Aspose.Cells.Worksheet;
                Aspose.Cells.Cell cell = sheet.Cells[1, 1];
                cell.PutValue(zgdeptname); //填报单位

                int colLength = 15;
                int rowIndex = 4;
                int serialNumber = 1;
                foreach (DataRow row in exportTable.Rows)
                {
                    Aspose.Cells.Cell fcell = sheet.Cells[rowIndex, 0];
                    fcell.PutValue(serialNumber.ToString()); //填充内容值

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
                            colrowcell.PutValue(row[i].ToString()); //填充内容值
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
            return Success("导出成功!");
        }
        #endregion

        #region 重大隐患信息报告单
        /// <summary>
        /// 重大隐患信息报告单
        /// </summary>
        /// <param name="queryJson"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public ActionResult ExportImportant(string keyValue)
        {
            //隐患基本信息
            var baseInfo = htbaseinfobll.GetEntity(keyValue);
            //隐患评估信息
            var approvalInfo = htapprovalbll.GetEntityByHidCode(baseInfo.HIDCODE);
            //隐患整改信息
            var changeInfo = htchangeinfobll.GetEntityByHidCode(baseInfo.HIDCODE);

            var rankitem = dataitemdetailbll.GetEntity(baseInfo.HIDRANK);

            string approvaldate = approvalInfo != null ? approvalInfo.APPROVALDATE.ToString() : "";

            var userInfo = OperatorProvider.Provider.Current();  //获取当前用户

            string fileName = "重大隐患信息报告单_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";

            string strDocPath = Server.MapPath("~/Resource/ExcelTemplate/重大隐患信息报告单.docx");

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

            return Success("导出成功!");
        }
        #endregion

        #region 事故隐患排查治理档案表
        /// <summary>
        /// 事故隐患排查治理档案表
        /// </summary>
        /// <param name="queryJson"></param>
        /// <param name="fileName"></param> 
        /// <returns></returns>
        public ActionResult ExportRecordInfo(string keyValue)
        {
            //隐患基本信息
            var baseInfo = htbaseinfobll.GetEntity(keyValue);
            //评估信息
            var approvalInfo = htapprovalbll.GetEntityByHidCode(baseInfo.HIDCODE);
            //隐患整改信息
            var changeInfo = htchangeinfobll.GetEntityByHidCode(baseInfo.HIDCODE);
            //隐患验收信息
            var acceptInfo = htacceptinfobll.GetEntityByHidCode(baseInfo.HIDCODE);
            //隐患整改效果评估
            var estimateInfo = htestimatebll.GetEntityByHidCode(baseInfo.HIDCODE);

            var hidtypekitem = dataitemdetailbll.GetEntity(baseInfo.HIDTYPE);

            var hidrankitem = dataitemdetailbll.GetEntity(baseInfo.HIDRANK);

            string changeresume = string.Empty;

            if (baseInfo.WORKSTREAM == "隐患验收" || baseInfo.WORKSTREAM == "整改效果评估" || baseInfo.WORKSTREAM == "整改结束")
            {
                changeresume = "已整改";
            }
            else
            {
                changeresume = "未整改";
            }

            //评估时间
            string approvaldate = approvalInfo != null ? approvalInfo.APPROVALDATE.ToString() : "";

            var userInfo = OperatorProvider.Provider.Current();  //获取当前用户

            string fileName = "事故隐患排查治理档案表_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";

            string strDocPath = Server.MapPath("~/Resource/ExcelTemplate/事故隐患排查治理档案表.docx");
            DesktopBLL dbll = new DesktopBLL();
            if (dbll.IsGeneric())
            {
                strDocPath = Server.MapPath("~/Resource/ExcelTemplate/通用事故隐患排查治理档案表.docx");
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
            row["hiddescribe"] = baseInfo.HIDDESCRIBE; //事故隐患描述(简题)
            row["hidtype"] = null != hidtypekitem ? hidtypekitem.ItemName : "";//隐患类别
            row["checkmanname"] = baseInfo.CHECKMANNAME; //隐患发现人(排查人)
            row["checkdate"] = null != baseInfo.CHECKDATE ? baseInfo.CHECKDATE.Value.ToString("yyyy-MM-dd") : "";//发现日期
            row["hidcode"] = baseInfo.HIDCODE;
            row["hidconsequence"] = baseInfo.HIDCONSEQUENCE;
            row["hidrank"] = null != hidrankitem ? hidrankitem.ItemName : "";//隐患级别
            row["hidapproval"] = null != approvalInfo ? approvalInfo.APPROVALPERSONNAME : "";//评估负责人
            row["changedutydepartname"] = null != changeInfo ? changeInfo.CHANGEDUTYDEPARTNAME : ""; //整改部门
            row["changepersonname"] = null != changeInfo ? changeInfo.CHANGEPERSONNAME : ""; //整改人
            row["startyear"] = !string.IsNullOrEmpty(approvaldate) ? Convert.ToDateTime(approvaldate).ToString("yyyy-MM-dd") : ""; //评估时间
            row["endyear"] = null != changeInfo ? (null != changeInfo.CHANGEDEADINE ? changeInfo.CHANGEDEADINE.Value.ToString("yyyy-MM-dd") : "") : "";//整改时间
            row["preventmeasure"] = baseInfo.PREVENTMEASURE;

            row["changeresume"] = changeresume; //治理完成情况
            row["realitymanagecapital"] = null != changeInfo ? (null != changeInfo.REALITYMANAGECAPITAL ? Math.Round(changeInfo.REALITYMANAGECAPITAL.Value / 10000, 4).ToString() : "0") : "0";
            row["changedepartname"] = null != changeInfo ? changeInfo.CHANGEDUTYDEPARTNAME : "";
            row["changepeoplename"] = null != changeInfo ? changeInfo.CHANGEPERSONNAME : "";
            row["changefinishdate"] = null != changeInfo ? (null != changeInfo.CHANGEFINISHDATE ? changeInfo.CHANGEFINISHDATE.Value.ToString("yyyy-MM-dd") : "") : "";
            row["acceptdepartname"] = null != acceptInfo ? acceptInfo.ACCEPTDEPARTNAME : "";
            row["acceptidea"] = null != acceptInfo ? acceptInfo.ACCEPTIDEA : "";
            row["describetitle"] = "事故隐患描述(简题)";
            int result = dataitemdetailbll.GetDataItemListByItemCode("'IsEnableMinimalistMode'").Where(p => p.ItemValue == userInfo.OrganizeId).Count();
            if (result > 0)
            {
                row["describetitle"] = "隐患内容";
            }

            if (null != acceptInfo)
            {
                if (acceptInfo.ACCEPTSTATUS == "1")
                {
                    row["acceptstatus"] = "验收通过";
                }
                else if (acceptInfo.ACCEPTSTATUS == "0")
                {
                    row["acceptstatus"] = "验收不通过";
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

            return Success("导出成功!");
        }
        #endregion

        #region 电力安全隐患排查治理情况月报表
        /// <summary>
        /// 电力安全隐患排查治理情况月报表
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportSituation(string mode = "")
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string deptcode = curUser.OrganizeCode;
            string curdate = DateTime.Now.ToString("yyyy");
            try
            {
                DataTable dt = htbaseinfobll.GetHiddenSituationOfMonth(deptcode, curdate, curUser); //当前年度下的隐患统计月报表

                decimal yjtotal = 0; //一级重大
                decimal yjzgtotal = 0; //已整改一级重大
                decimal yjzgl = 0; //一级整改率 
                decimal ejtotal = 0; //二级重大
                decimal ejzgtotal = 0; //已整改二级重大
                decimal ejzgl = 0; //二级整改率 
                decimal ybtotal = 0; //一般
                decimal ybzgtotal = 0; //已整改一般
                decimal ybzgl = 0; //一般整改率 
                decimal money = 0;  //资金总额
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
                yjzgl = yjtotal > 0 ? Math.Round(yjzgtotal / yjtotal * 100, 2) : 0; //一级隐患整改率
                ejzgl = ejtotal > 0 ? Math.Round(ejzgtotal / ejtotal * 100, 2) : 0; //二级隐患整改率
                ybzgl = ybtotal > 0 ? Math.Round(ybzgtotal / ybtotal * 100, 2) : 0; //一般隐患整改率

                DesktopBLL dbll = new DesktopBLL();
                // 确定导出文件名
                string fileName = "电力安全隐患排查治理情况月报表";
                string fileUrl = "~/Resource/ExcelTemplate/电力安全隐患排查治理情况月报表.xlsx";
                if (dbll.IsGeneric())
                {
                    fileName = "安全隐患排查治理情况月报表";
                    fileUrl = "~/Resource/ExcelTemplate/安全隐患排查治理情况月报表.xlsx";
                }


                //详细列表内容
                string fielname = fileName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                wb.Open(Server.MapPath(fileUrl));
                Aspose.Cells.Worksheet sheet = wb.Worksheets[0] as Aspose.Cells.Worksheet;
                Aspose.Cells.Cells cells = sheet.Cells;
                Aspose.Cells.Cell cell = sheet.Cells[1, 1];
                cell.PutValue(curUser.DeptName); //填报单位
                Aspose.Cells.Cell endcell = sheet.Cells[1, 13];
                endcell.PutValue(DateTime.Now.ToString("yyyy-MM-dd")); //填报单位

                //填充汇总对象
                sheet.Cells[7, 4].PutValue(yjtotal.ToString()); //一级总数
                sheet.Cells[7, 5].PutValue(yjzgtotal.ToString()); //一级已整改总数
                sheet.Cells[7, 6].PutValue(yjzgl.ToString()); //一级整改率
                sheet.Cells[7, 7].PutValue(ejtotal.ToString()); //二级总数
                sheet.Cells[7, 8].PutValue(ejzgtotal.ToString()); //二级已整改总数
                sheet.Cells[7, 9].PutValue(ejzgl.ToString()); //二级整改率
                sheet.Cells[7, 10].PutValue(ybtotal.ToString()); //一般总数
                sheet.Cells[7, 11].PutValue(ybzgtotal.ToString()); //一般已整改总数
                sheet.Cells[7, 12].PutValue(ybzgl.ToString()); //一般整改率
                sheet.Cells[7, 13].PutValue(money.ToString()); //一般整改率

                sheet.Cells.ImportDataTable(dt, false, 8, 0);
                int lastRow = 8 + dt.Rows.Count;
                //上一年度数据汇总
                curdate = DateTime.Now.AddYears(-1).ToString("yyyy");
                DataTable prevdt = htbaseinfobll.GetHiddenSituationOfMonth(deptcode, curdate, curUser); //当前年度下的隐患统计月报表 
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
                sheet.Cells[lastRow, 4].PutValue(yjwzg.ToString());//上一年度一级未整改
                sheet.Cells[lastRow, 7].PutValue(ejwzg.ToString());//上一年度二级未整改
                sheet.Cells[lastRow, 10].PutValue(ybwzg.ToString()); //上一年度一般未整改
                sheet.Cells[lastRow, 13].PutValue(lsmoney.ToString()); //上一年度资金总额

                cells.Merge(lastRow, 4, 1, 3);  //用于填写上一年度的一级未整改隐患总和
                cells.Merge(lastRow, 7, 1, 3);  //用于填写上一年度的二级未整改隐患总和
                cells.Merge(lastRow, 10, 1, 3); //用于填写上一年度的一般未整改隐患总和

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

            return Success("导出成功!");
        }
        #endregion

        #region 获取首页隐患top n的隐患统计数据

        /// <summary>
        /// 获取首页隐患top n的隐患统计数据
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

        #region 保存期限设置
        /// <summary>
        /// 保存期限设置
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

                return Success("操作成功!");
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region 获取到期设置数据
        /// <summary>
        /// 获取到期设置数据
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

        #region 获取整改计划数据
        /// <summary>
        /// 获取整改计划数据
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

        #region 保存整改计划
        /// <summary>
        /// 保存整改计划
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
                return Success("操作成功。");
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

