using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSFramework.Util.WebControl;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using ERCHTMS.Service.OutsourcingProject;
using ERCHTMS.Entity.EmergencyPlatform;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Busines.BaseManage;

namespace ERCHTMS.Busines.OutsourcingProject
{
    public class PeopleReviewBLL
    {
        private PeopleReviewIService service = new PeopleReviewService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<PeopleReviewEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public PeopleReviewEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        public DataTable GetPagePeopleReviewListJson(Pagination pagination, string queryJson)
        {
            return service.GetPagePeopleReviewListJson(pagination, queryJson);
        }

        /// <summary>
        /// 当前登录人是否有权限审核并获取下一次审核权限实体
        /// </summary>
        /// <param name="currUser">当前登录人</param>
        /// <param name="state">是否有权限审核 1：能审核 0 ：不能审核</param>
        /// <param name="moduleName">模块名称</param>
        /// <param name="outengineerid">工程Id</param>
        /// <returns>null-当前为最后一次审核,ManyPowerCheckEntity：下一次审核权限实体</returns>
        public ManyPowerCheckEntity CheckAuditPower(Operator currUser, out string state, string moduleName, string outengineerid, Boolean isStep = true, string CurFlowId = "",string EngineerletDeptId = "")
        {
            return service.CheckAuditPower(currUser, out state, moduleName, outengineerid, isStep, CurFlowId, EngineerletDeptId);
        }
        #endregion

        #region 按顺序进行获取下一个流程节点
        /// <summary>
        /// 按顺序进行获取下一个流程节点
        /// </summary>
        /// <param name="currUser"></param>
        /// <param name="moduleName"></param>
        /// <param name="outengineerid"></param>
        /// <param name="flowtype"></param>
        /// <param name="curFlowId"></param>
        /// <param name="isBack"></param>
        /// <returns></returns>
        public ManyPowerCheckEntity CheckAuditForNextFlow(Operator currUser, string moduleName, string outengineerid, string curFlowId, bool isBack, bool isUseSetting)
        {
            return service.CheckAuditForNextFlow(currUser, moduleName, outengineerid, curFlowId, isBack, isUseSetting);
        }
        /// <summary>
        /// 演练记录评价流程
        /// </summary>
        /// <param name="currUser">当前用户</param>
        /// <param name="moduleName">模块名称</param>
        /// <param name="DrillRecordId">演练记录Id</param>
        /// <param name="curFlowId">当前节点</param>
        /// <param name="isUseSetting"></param>
        /// <returns></returns>
        public ManyPowerCheckEntity CheckEvaluateForNextFlow(Operator currUser, string moduleName, DrillplanrecordEntity entity)
        {
            return service.CheckEvaluateForNextFlow(currUser, moduleName, entity);
        }
        #endregion

        /// <summary>
        /// 通过作业单位查询配置审核的下一个审核单位和角色(单位内部)
        /// </summary>
        /// <param name="currUser"></param>
        /// <param name="moduleName"></param>
        /// <param name="workUnitId"></param>
        /// <param name="curFlowId"></param>
        /// <param name="isBack"></param>
        /// <returns></returns>
        public ManyPowerCheckEntity CheckAuditForNextByWorkUnit(Operator currUser, string moduleName, string workUnitId, string curFlowId, bool isBack)
        {
            return service.CheckAuditForNextByWorkUnit(currUser, moduleName, workUnitId, curFlowId, isBack);
        }

        /// <summary>
        /// 通过作业单位查询配置审核的下一个审核单位和角色(外包单位)
        /// </summary>
        /// <param name="currUser"></param>
        /// <param name="moduleName"></param>
        /// <param name="workUnitId"></param>
        /// <param name="curFlowId"></param>
        /// <param name="isBack"></param>
        /// <param name="isUseSetting"></param>
        /// <returns></returns>
        public ManyPowerCheckEntity CheckAuditForNextByOutsourcing(Operator currUser, string moduleName, string workUnitId, string curFlowId, bool isBack, bool isUseSetting, string projectid="")
        {
            return service.CheckAuditForNextByOutsourcing(currUser, moduleName, workUnitId, curFlowId, isBack, isUseSetting, projectid);
        }
        /// <summary>
        /// 起重吊装作业专用(审核专业)
        /// </summary>
        /// <param name="currUser">当前人</param>
        /// <param name="moduleName">模块名称</param>
        /// <param name="workUnitId">审核专业ID</param>
        /// <param name="curFlowId">当前节点ID</param>
        /// <param name="isBack">是否回退</param>
        /// <returns></returns>
        public ManyPowerCheckEntity CheckAuditForNextByLiftHoist(Operator currUser, string moduleName, string workUnitId, string curFlowId, bool isBack)
        {
            return service.CheckAuditForNextByLiftHoist(currUser, moduleName, workUnitId, curFlowId, isBack);
        }
        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, PeopleReviewEntity entity)
        {
            try
            {
                List<string> userids = service.SaveForm(keyValue, entity);
                //对接培训平台
                if (userids.Count > 0)
                {
                    try
                    {
                        string way = new DataItemDetailBLL().GetItemValue("WhatWay");
                        Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                        DepartmentEntity org = new DepartmentBLL().GetEntity(user.OrganizeId);
                        foreach (var item in userids)
                        {
                            var userInfo = new UserBLL().GetUserInfoEntity(item);
                            var userEntity = new UserBLL().GetEntity(item);
                            if (org.IsTrain == 1)
                            {
                                //对接.net培训平台
                                if (way == "0")
                                {

                                }
                                //对接java培训平台
                                if (way == "1")
                                {
                                    DepartmentEntity dept = new DepartmentBLL().GetEntity(userInfo.DepartmentId);
                                    if (dept != null)
                                    {
                                        string deptId = dept.DepartmentId;
                                        string enCode = dept.EnCode;
                                        if (!string.IsNullOrWhiteSpace(dept.DeptKey))
                                        {
                                            string[] arr = dept.DeptKey.Split('|');
                                            deptId = arr[0];
                                            if (arr.Length > 1)
                                            {
                                                enCode = arr[1];
                                            }
                                        }
                                        Task.Run(() =>
                                        {
                                            object obj = new
                                            {
                                                action = "add",
                                                time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                                userId = userInfo.UserId,
                                                userName = userInfo.RealName,
                                                account = userInfo.Account,
                                                deptId = deptId,
                                                deptCode = enCode,
                                                password = "Abc123456", //为null时不要修改密码!
                                                sex = userInfo.Gender,
                                                idCard = userInfo.IdentifyID,
                                                email = userInfo.Email,
                                                mobile = userInfo.Mobile,
                                                birth = userInfo.Birthday == null ? "" : userInfo.Birthday.Value.ToString("yyyy-MM-dd"),//生日
                                                postName = userInfo.DutyName,//岗位
                                                age = userInfo.Age,//年龄
                                                native = userInfo.Native, //籍贯
                                                nation = userInfo.Nation, //民族
                                                encode = userInfo.EnCode,//工号
                                                companyId = org.InnerPhone,
                                                role = userInfo.IsTrainAdmin == null ? 0 : userInfo.IsTrainAdmin, //角色（0:学员，1:培训管理员）
                                                postId = userEntity.DutyId,
                                                jobTitle = userEntity.JobTitle,
                                                techLevel = userEntity.TechnicalGrade,
                                                workType = userEntity.Craft,
                                                trainRoles = userEntity.TrainRoleId
                                            };
                                            List<object> list = new List<object>();
                                            list.Add(obj);
                                            Busines.JPush.JPushApi.PushMessage(list, 1);
                                        });
                                    }

                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        
                    }
                    
                }
                
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public void RemovePeople(string keyValue) {
        //    try
        //    {
        //        service.RemovePeople(keyValue);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        #endregion
    }
}
