using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using ERCHTMS.Service.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Busines.BaseManage;
using System.Threading.Tasks;

namespace ERCHTMS.Busines.OutsourcingProject
{
    /// <summary>
    /// 描 述：资质审查审核表
    /// </summary>
    public class AptitudeinvestigateauditBLL
    {
        private AptitudeinvestigateauditIService service = new AptitudeinvestigateauditService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<AptitudeinvestigateauditEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public AptitudeinvestigateauditEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        public AptitudeinvestigateauditEntity GetAuditEntity(string FKId)
        {
            return service.GetAuditEntity(FKId);
        }

        public List<AptitudeinvestigateauditEntity> GetAuditList(string keyValue) 
        {
            return service.GetAuditList(keyValue);
        }

        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
          /// <summary>
        /// 获取业务相关两的审核记录
        /// </summary>
        /// <param name="recId">业务记录Id</param>
        /// <returns></returns>
        public DataTable GetAuditRecList(string recId)
        {
            return service.GetAuditRecList(recId);
        }
        #endregion


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
        public void SaveForm(string keyValue, AptitudeinvestigateauditEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// 资质审查审核通过同步 工程 单位 人员信息
        /// 修改外包单位入场状态,修改流程表资质审核状态
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>

        public void SaveSynchrodata(string keyValue, AptitudeinvestigateauditEntity entity)
        {
            try
            {
                service.SaveSynchrodata(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// 保证金审核
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveSafetyEamestMoney(string keyValue, AptitudeinvestigateauditEntity entity)
        {
            try
            {
                service.SaveSafetyEamestMoney(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 保存表单（新增、修改）
        /// 复工申请审核:更新工程状态
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void AuditReturnForWork(string keyValue, AptitudeinvestigateauditEntity entity)
        {
            try
            {
                service.AuditReturnForWork(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
         /// <summary>
        /// 保存表单（新增、修改）
        /// 开工申请审核:更新工程状态
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void AuditStartApply(string keyValue, AptitudeinvestigateauditEntity entity)
        {
            try
            {
                service.AuditStartApply(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void AuditPeopleReview(string keyValue, AptitudeinvestigateauditEntity entity)
        {
            try
            {
                List<string> userids = service.AuditPeopleReview(keyValue, entity);
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

        
        #endregion
    }
}
