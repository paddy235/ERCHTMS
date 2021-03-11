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
    /// �� ������������
    /// </summary>
    public class TaskShareBLL
    {
        private TaskShareIService service = new TaskShareService();
        private IDepartmentService departmentservice = new DepartmentService();
        private IDataItemDetailService dataitemdetailservice = new DataItemDetailService();
        private IUserService userservice = new UserService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<TaskShareEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public TaskShareEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// ��ȡ���������б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public DataTable GetDataTable(Pagination page, string queryJson, string authType)
        {
            return service.GetDataTable(page, queryJson, authType);
        }

        /// <summary>
        /// ��վ�ලͳ��
        /// </summary>
        /// <param name="sentity"></param>
        /// <returns></returns>
        public DataTable QueryStatisticsByAction(StatisticsEntity sentity)
        {
            return service.QueryStatisticsByAction(sentity);
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
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
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
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
        /// ����
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
                        //if (pushdata.SendCode == "ZY015")//�����
                        //{
                        //    //��ȫ���ܲ���
                        //    Operator currUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
                        //    IEnumerable<DepartmentEntity> departs = departmentservice.GetDepts(currUser.OrganizeId, 1);
                        //    string deptids = string.Empty;
                        //    //���õĽ�ɫ
                        //    string rolenames = dataitemdetailservice.GetItemValue(currUser.OrganizeId, "urgerole");
                        //    IList<UserEntity> users2 = null;
                        //    if (departs != null && departs.Count() > 0 && !string.IsNullOrEmpty(rolenames))
                        //    {
                        //        rolenames = "'" + rolenames.Replace(",", "','") + "'";
                        //        deptids = string.Join(",", departs.Select(x => x.DepartmentId).ToArray());
                        //        deptids = "'" + deptids.Replace(",", "','") + "'";
                        //        //��ȫ���ܲ��ţ���ɫ���û�
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
                        if (pushdata.SendCode == "ZY016")//��ִ�е�
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
                        else if (pushdata.SendCode == "ZY017")//�������
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
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
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
