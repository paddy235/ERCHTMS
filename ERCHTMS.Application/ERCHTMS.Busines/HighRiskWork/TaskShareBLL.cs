using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using ERCHTMS.Service.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using ERCHTMS.Code;
using ERCHTMS.Service.BaseManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Service.SystemManage;
using ERCHTMS.IService.SystemManage;
using System.Linq;
using ERCHTMS.Busines.JPush;
using ERCHTMS.Entity.HiddenTroubleManage;

namespace ERCHTMS.Busines.HighRiskWork
{
    /// <summary>
    /// 描 述：任务分配表
    /// </summary>
    public class TaskShareBLL
    {
        private TaskShareIService service = new TaskShareService();
        private IDepartmentService departmentservice = new DepartmentService();
        private IDataItemDetailService dataitemdetailservice = new DataItemDetailService();
        private IUserService userservice = new UserService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<TaskShareEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public TaskShareEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 获取分配任务列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public DataTable GetDataTable(Pagination page, string queryJson, string authType)
        {
            return service.GetDataTable(page, queryJson, authType);
        }

        /// <summary>
        /// 旁站监督统计
        /// </summary>
        /// <param name="sentity"></param>
        /// <returns></returns>
        public DataTable QueryStatisticsByAction(StatisticsEntity sentity)
        {
            return service.QueryStatisticsByAction(sentity);
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
        public void SaveForm(string keyValue, TaskShareEntity entity)
        {
            try
            {
                List<PushMessageData> list = service.SaveForm(keyValue, entity);
                PushSideMessage(list);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 推送
        /// </summary>
        /// <param name="list"></param>
        public void PushSideMessage(List<PushMessageData> list)
        {
            if (list != null)
            {
                foreach (var pushdata in list)
                {
                    if (pushdata.Success == 1 && !string.IsNullOrEmpty(pushdata.SendCode))
                    {
                        //if (pushdata.SendCode == "ZY015")//待监管
                        //{
                        //    //安全主管部门
                        //    Operator currUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
                        //    IEnumerable<DepartmentEntity> departs = departmentservice.GetDepts(currUser.OrganizeId, 1);
                        //    string deptids = string.Empty;
                        //    //配置的角色
                        //    string rolenames = dataitemdetailservice.GetItemValue(currUser.OrganizeId, "urgerole");
                        //    IList<UserEntity> users2 = null;
                        //    if (departs != null && departs.Count() > 0 && !string.IsNullOrEmpty(rolenames))
                        //    {
                        //        rolenames = "'" + rolenames.Replace(",", "','") + "'";
                        //        deptids = string.Join(",", departs.Select(x => x.DepartmentId).ToArray());
                        //        deptids = "'" + deptids.Replace(",", "','") + "'";
                        //        //安全主管部门，角色的用户
                        //        users2 = userservice.GetUserListByRoleName(deptids, rolenames, true, string.Empty);
                        //    }
                        //    List<UserEntity> users = new List<UserEntity>();
                        //    if (users2 != null && users2.Count > 0)
                        //    {
                        //        users.AddRange(users2);
                        //        users = users.Union(users2).ToList();
                        //    }
                        //    if (users != null && users.Count > 0)
                        //    {
                        //        string names = string.Join(",", users.Select(x => x.RealName).ToArray());
                        //        string accounts = string.Join(",", users.Select(x => x.Account).ToArray());
                        //        JPushApi.PushMessage(accounts, names, pushdata.SendCode, pushdata.EntityId);
                        //    }
                        //}
                        //else
                        if (pushdata.SendCode == "ZY016")//待执行的
                        {
                            if (pushdata.UserId != null)
                            {
                                string[] userid = pushdata.UserId.Split(',');
                                string names = "";
                                string accounts = "";
                                foreach (var item in userid)
                                {
                                    UserEntity userEntity = userservice.GetEntity(pushdata.UserId);
                                    if (userEntity != null)
                                    {
                                        names += userEntity.RealName + ",";
                                        accounts += userEntity.Account + ",";
                                    }
                                }
                                if (!string.IsNullOrEmpty(names))
                                {
                                    JPushApi.PushMessage(accounts.TrimEnd(','), names.TrimEnd(','), pushdata.SendCode, pushdata.EntityId);
                                }
                            }
                        }
                        else if (pushdata.SendCode == "ZY017")//待分配的
                        {
                            string flowdeptids = "'" + pushdata.UserDept.Replace(",", "','") + "'";
                            string flowrolenames = "'" + pushdata.UserRole.Replace(",", "','") + "'";
                            IList<UserEntity> users = new UserService().GetUserListByRoleName(flowdeptids, flowrolenames, true, string.Empty);
                            if (users != null && users.Count > 0)
                            {
                                string names = string.Join(",", users.Select(x => x.RealName).ToArray());
                                string accounts = string.Join(",", users.Select(x => x.Account).ToArray());
                                JPushApi.PushMessage(accounts, names, pushdata.SendCode, pushdata.EntityId);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveOnlyShare(string keyValue, TaskShareEntity entity)
        {
            try
            {
                service.SaveOnlyShare(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
