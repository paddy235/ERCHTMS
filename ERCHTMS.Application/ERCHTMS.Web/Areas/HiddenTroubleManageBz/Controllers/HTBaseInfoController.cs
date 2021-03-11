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
    /// 描 述：隐患基本信息表
    /// </summary>
    public class HTBaseInfoController : MvcControllerBase
    {
        private HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL(); //隐患基本信息
        private HTChangeInfoBLL htchangeinfobll = new HTChangeInfoBLL(); //隐患整改信息
        private HTApprovalBLL htapprovalbll = new HTApprovalBLL(); //隐患评估信息
        private HTAcceptInfoBLL htacceptinfobll = new HTAcceptInfoBLL(); //隐患验收信息
        private HTEstimateBLL htestimatebll = new HTEstimateBLL(); //整改效果评估信息
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL(); //隐患流程
        private UserBLL userbll = new UserBLL(); //用户操作对象
        private SaftyCheckDataRecordBLL saftycheckdatarecordbll = new SaftyCheckDataRecordBLL();

        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private OrganizeCache organizeCache = new OrganizeCache();
        private DepartmentBLL departmentBLL = new DepartmentBLL();

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
            return View();
        }
        #endregion

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

            string uModel = string.Empty;

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
                uModel = ulist.Where(p => p.UserId == curUser.UserId).Count().ToString();
            }
            return Content(uModel);
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

            var estimateInfo = htestimatebll.GetEntityByHidCode(baseInfo.HIDCODE);

            var userInfo = OperatorProvider.Provider.Current();  //获取当前用户

            var data = new { baseInfo = baseInfo, changeInfo = changeInfo, acceptInfo = acceptInfo, userInfo = userInfo, estimateInfo = estimateInfo };

            return ToJsonResult(data);
        }

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


        [HttpGet]
        public ActionResult IsOrginazeLeader()
        {
            Operator curUser = OperatorProvider.Provider.Current();

            //公司级用户
            if (userbll.HaveRoleListByKey(curUser.UserId, dataitemdetailbll.GetItemValue("HidOrganize")).Rows.Count > 0)
            {
                curUser.DeptCode = curUser.OrganizeCode;
                curUser.DeptName = curUser.OrganizeName;
            }
            return Content(curUser.ToJson());
        }

        #region 页面组件初始化
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetInitDataJson()
        {
            Operator CreateUser = OperatorProvider.Provider.Current();
            //隐患编码
            string HidCode = DateTime.Now.ToString("yyyyMMddHHmmssfff").ToString();
            //安全检查类型 隐患类型 隐患级别  培训模板
            string itemCode = "'SaftyCheckType','HidType','HidRank','TrainTemplateName'";
            //集合
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode(itemCode);

            //公司级用户
            if (userbll.HaveRoleListByKey(CreateUser.UserId, dataitemdetailbll.GetItemValue("HidOrganize")).Rows.Count > 0)
            {
                CreateUser.DeptCode = CreateUser.OrganizeCode;
                CreateUser.DeptName = CreateUser.OrganizeName;
            }

            //返回值
            var josnData = new
            {
                CreateUser = CreateUser.UserName,
                User = CreateUser,  //用户对象
                HidCode = HidCode,
                HidPhoto = Guid.NewGuid().ToString(), //隐患图片
                HidChangePhoto = Guid.NewGuid().ToString(), //整改图片
                AcceptPhoto = Guid.NewGuid().ToString(), //验收图片 
                Attachment = Guid.NewGuid().ToString(),
                EstimatePhoto = Guid.NewGuid().ToString(), //评估图片  
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


        #region 初始化查询条件
        /// <summary>
        /// 初始化查询条件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetQueryConditionJson()
        {
            //隐患级别，整改状态，流程状态，检查类型，隐患类型 ,数据范围
            string itemCode = "'HidRank','ChangeStatus','WorkStream','SaftyCheckType','HidType','DataScope'";
            //集合
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode(itemCode);
            //返回值
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
            CommonSaveForm(keyValue, entity, cEntity, aEntity);
            return Success("操作成功!");
        }
        #endregion

        #region 公用方法，保存数据
        /// <summary>
        /// 公用方法，保存数据
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void CommonSaveForm(string keyValue, HTBaseInfoEntity entity, HTChangeInfoEntity cEntity, HTAcceptInfoEntity aEntity)
        {
            //提交通过
            string userId = OperatorProvider.Provider.Current().UserId;
            /********隐患信息**********/
            if (string.IsNullOrEmpty(keyValue))
            {
                entity.HIDDEPARTCODE = OperatorProvider.Provider.Current().DeptCode;
                entity.HIDDEPARTNAME = OperatorProvider.Provider.Current().DeptName;
            }
            //保存隐患基本信息
            htbaseinfobll.SaveForm(keyValue, entity);

            //当前是隐患，非违章，则创建流程实例
            if (entity.ISBREAKRULE == "0")
            {
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
                cEntity.BACKREASON = null;
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
        }
        #endregion

        #region 提交流程（同时新增、修改隐患信息）
        /// <summary>
        /// 保存表单（新增、修改）
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
                    return Error("隐患编码重复,请重新新增!");
                }
            }

            CommonSaveForm(keyValue, entity, cEntity, aEntity);

            //创建完流程实例后
            if (string.IsNullOrEmpty(keyValue))
            {
                keyValue = entity.ID;
            }

            //此处需要判断当前人是否为安全管理员
            string wfFlag = string.Empty;

            Operator curUser = OperatorProvider.Provider.Current();

            IList<UserEntity> ulist = new List<UserEntity>();

            string HidApproval = dataitemdetailbll.GetItemValue("HidApproval");

            string[] pstr = HidApproval.Split('#');  //分隔机构组

            foreach (string strArgs in pstr)
            {
                string[] str = strArgs.Split('|');
                //当前机构相同，且为本部门安全管理员验证
                if (str[0].ToString() == curUser.OrganizeId && str[1].ToString() == "0")
                {

                    //公司级用户
                    if (userbll.HaveRoleListByKey(curUser.UserId, dataitemdetailbll.GetItemValue("HidOrganize")).Rows.Count > 0)
                    {
                        string curdeptCode = "'" + curUser.OrganizeCode + "'";
                        ulist = userbll.GetUserListByDeptCode(curdeptCode, dataitemdetailbll.GetItemValue("HidApprovalSetting"), false, curUser.OrganizeId);
                    }
                    else
                    {
                        //此步骤需要判断是否为公司机构
                        ulist = userbll.GetUserListByRole(curUser.DeptCode, dataitemdetailbll.GetItemValue("HidApprovalSetting"), curUser.OrganizeId).ToList();
                    }
                    break;
                }
                if (str[0].ToString() == curUser.OrganizeId && str[1].ToString() == "1")
                {
                    //获取指定部门的所有人员
                    ulist = userbll.GetUserListByDeptCode(str[2].ToString(), null, false, curUser.OrganizeId).ToList();

                    break;
                }
            }

            //返回的记录数,大于0，标识当前用户拥有安全管理员身份或指定部门人员身份，反之则无
            int uModel = ulist.Where(p => p.UserId == curUser.UserId).Count();

            //参与人员
            string participant = string.Empty;
            //指定部门
            bool isPointDept = false;

            //安全管理员下，直接进行整改
            if (uModel > 0)
            {
                wfFlag = "3";//直接整改

                string keyId = Request.Form["CHANGEPERSON"].ToString();

                if (!string.IsNullOrEmpty(keyId) && keyId != "&nbsp;")
                {
                    participant = userbll.GetEntity(keyId).Account;  //整改人
                }
                else { return Error("请确认是否选择整改人!"); }

            }
            else
            {
                wfFlag = "1";//隐患评估

                IEnumerable<UserEntity> list = new List<UserEntity>();

                foreach (string strArgs in pstr)
                {
                    string[] str = strArgs.Split('|');

                    if (str[0].ToString() == curUser.OrganizeId && str[1].ToString() == "0")
                    {
                        //取当前用户对应部门的安全管理员
                        //公司级用户
                        if (userbll.HaveRoleListByKey(curUser.UserId, dataitemdetailbll.GetItemValue("HidOrganize")).Rows.Count > 0)
                        {
                            string curdeptCode = "'" + curUser.OrganizeCode + "'";
                            list = userbll.GetUserListByDeptCode(curdeptCode, dataitemdetailbll.GetItemValue("HidApprovalSetting"), false, curUser.OrganizeId);
                        }
                        else
                        {
                            //此步骤需要判断是否为公司机构
                            list = userbll.GetUserListByRole(curUser.DeptCode, dataitemdetailbll.GetItemValue("HidApprovalSetting"), curUser.OrganizeId).ToList();
                        }
                        break;
                    }
                    //指定部门
                    if (str[0].ToString() == curUser.OrganizeId && str[1].ToString() == "1")
                    {
                        //获取指定部门的所有人员
                        list = userbll.GetUserListByDeptCode(str[2].ToString(), null, false, curUser.OrganizeId);

                        string curDeptCode = "'" + curUser.DeptCode + "'";
                        //当前提交的人在指定部门之中
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

                //如果当前提交人是指定的部门人员,则直接提交到整改
                if (isPointDept)
                {
                    wfFlag = "3";//直接整改

                    string keyId = Request.Form["CHANGEPERSON"].ToString();

                    participant = userbll.GetEntity(keyId).Account;  //整改人
                }
            }

            if (!string.IsNullOrEmpty(participant))
            {
                int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, curUser.UserId);

                if (count > 0)
                {
                    htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //更新业务流程状态
                }
            }
            else
            {
                return Error("请联系系统管理员，添加本单位及相关单位评估人员!");
            }
            return Success("操作成功!");
        }
        #endregion


        #region 一次性提交多个关联检查人的登记隐患信息
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult CheckHiddenForm(string saftycheckdatarecordid)
        {
            Operator curUser = OperatorProvider.Provider.Current();

            var dtHid = htbaseinfobll.GetList(saftycheckdatarecordid, curUser.UserId, "", "隐患登记");

            string keyValue = string.Empty;

            string changeperson = string.Empty;

            foreach (DataRow row in dtHid.Rows)
            {
                keyValue = row["id"].ToString();

                changeperson = row["changeperson"].ToString();

                //此处需要判断当前人是否为安全管理员
                string wfFlag = string.Empty;


                IList<UserEntity> ulist = new List<UserEntity>();

                string HidApproval = dataitemdetailbll.GetItemValue("HidApproval");

                string[] pstr = HidApproval.Split('#');  //分隔机构组

                foreach (string strArgs in pstr)
                {
                    string[] str = strArgs.Split('|');
                    //当前机构相同，且为本部门安全管理员验证
                    if (str[0].ToString() == curUser.OrganizeId && str[1].ToString() == "0")
                    {
                        ulist = userbll.GetUserListByRole(curUser.DeptCode, dataitemdetailbll.GetItemValue("HidApprovalSetting"), curUser.OrganizeId).ToList();

                        break;
                    }
                    if (str[0].ToString() == curUser.OrganizeId && str[1].ToString() == "1")
                    {
                        //获取指定部门的所有人员
                        ulist = userbll.GetUserListByDeptCode(str[2].ToString(), null, false, curUser.OrganizeId).ToList();

                        break;
                    }
                }

                //返回的记录数,大于0，标识当前用户拥有安全管理员身份或指定部门人员身份，反之则无
                int uModel = ulist.Where(p => p.UserId == curUser.UserId).Count();

                //参与人员
                string participant = string.Empty;
                //指定部门
                bool isPointDept = false;

                //安全管理员下，直接进行整改
                if (uModel > 0)
                {
                    wfFlag = "3";//直接整改

                    string keyId = changeperson; //  Request.Form["CHANGEPERSON"].ToString();

                    if (!string.IsNullOrEmpty(keyId) && keyId != "&nbsp;")
                    {
                        participant = userbll.GetEntity(keyId).Account;  //整改人
                    }
                    else { return Error("请确认是否选择整改人!"); }

                }
                else
                {
                    wfFlag = "1";//隐患评估

                    IEnumerable<UserEntity> list = new List<UserEntity>();

                    foreach (string strArgs in pstr)
                    {
                        string[] str = strArgs.Split('|');

                        if (str[0].ToString() == curUser.OrganizeId && str[1].ToString() == "0")
                        {
                            //取当前用户对应部门的安全管理员
                            list = userbll.GetUserListByRole(curUser.DeptCode, dataitemdetailbll.GetItemValue("HidApprovalSetting"), curUser.OrganizeId);
                            break;
                        }
                        //指定部门
                        if (str[0].ToString() == curUser.OrganizeId && str[1].ToString() == "1")
                        {
                            //获取指定部门的所有人员
                            list = userbll.GetUserListByDeptCode(str[2].ToString(), null, false, curUser.OrganizeId);

                            string curDeptCode = "'" + curUser.DeptCode + "'";
                            //当前提交的人在指定部门之中
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

                    //如果当前提交人是指定的部门人员,则直接提交到整改
                    if (isPointDept)
                    {
                        wfFlag = "3";//直接整改

                        string keyId = changeperson;

                        participant = userbll.GetEntity(keyId).Account;  //整改人
                    }
                }

                if (!string.IsNullOrEmpty(participant))
                {
                    int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, curUser.UserId);

                    if (count > 0)
                    {
                        htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //更新业务流程状态
                    }
                }
                else
                {
                    return Error("请联系系统管理员，添加本单位及相关单位评估人员!");
                }
            }
            //更新
            //saftycheckdatarecordbll.UpdateCheckMan(curUser.Account);

            return Success("操作成功!");
        }
        #endregion

        #region
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
        /// <param name="Filedata"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadifyHidden(string actionType, HttpPostedFileBase file)
        {
            HSSFWorkbook hssfworkbook;
            if (!string.IsNullOrEmpty(actionType))
            {
                string path = Server.MapPath("~/Resource/HiddenFile/") + file.FileName;
                file.SaveAs(path); //保存文件到当前服务器 
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
            return Content("操作成功!");
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

        //#region 获取隐患排查指标相关数据
        ///// <summary>
        ///// 获取隐患排查指标相关数据
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

        #region 导出隐患基本信息
        /// <summary>
        /// 导出隐患基本信息
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
            queryJson = queryJson.Insert(1, "\"userId\":\"" + userId + "\","); //添加当前用户
            queryJson = queryJson.Insert(1, "\"isPlanLevel\":\"" + curUser.isPlanLevel + "\","); //添加当前是否公司及厂级
            pagination.p_fields = @"workstream,hidcode,hidtypename,hidrankname,hidpointname,checktypename,checkdepartname,checkmanname,
                                    changedutydepartname,changepersonname,changedeadine,hiddescribe,changemeasure";
            //取出数据源
            DataTable exportTable = htbaseinfobll.GetHiddenBaseInfoPageList(pagination, queryJson);
            ////设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "隐患排查基本信息";
            excelconfig.FileName = fileName + ".xls";
            ////每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity = listColumnEntity;
            ColumnEntity columnentity = new ColumnEntity();

            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "workstream", ExcelColumn = "流程状态", Width = 20 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "hidcode", ExcelColumn = "隐患编码", Width = 20 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "hidtypename", ExcelColumn = "隐患类别", Width = 20 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "hidrankname", ExcelColumn = "隐患级别", Width = 20 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "hidpointname", ExcelColumn = "隐患区域", Width = 20 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checktypename", ExcelColumn = "检查类型", Width = 20 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkdepartname", ExcelColumn = "排查单位", Width = 25 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkmanname", ExcelColumn = "排查人", Width = 20 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "changedutydepartname", ExcelColumn = "整改单位", Width = 25 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "changepersonname", ExcelColumn = "整改人", Width = 20 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "changedeadine", ExcelColumn = "整改截止时间", Width = 20 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "hiddescribe", ExcelColumn = "隐患描述", Width = 30 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "changemeasure", ExcelColumn = "整改措施", Width = 30 });
            ////调用导出方法
            ExcelHelper.ExcelDownload(exportTable, excelconfig);

            return Success("导出成功!");
        }
        #endregion

        #endregion
    }
}
