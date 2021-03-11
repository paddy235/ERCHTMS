using ERCHTMS.Entity.BaseManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.BaseManage
{
    /// <summary>
    /// 描 述：用户基本信息
    /// </summary>
    public interface IUserInfoService
    {
        #region 获取数据
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        DataTable GetTable();

        /// <summary>
        /// 获取用户集合
        /// </summary>
        /// <returns></returns>
        IList<UserInfoEntity> GetAllUserInfoList();
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        IEnumerable<UserInfoEntity> GetPageList(Pagination pagination, string queryJson);

                /// <summary>
        /// 获取特殊用户集合
        /// </summary>
        /// <param name="accounts"></param>
        /// <returns></returns>
        IList<UserInfoEntity> GetCurLevelAndHigherLevelUserByArgs(string accounts, string rolename);

        IList<UserInfoEntity> GetUserListByAnyCondition(string orgid, string deptmentid, string roleid, string majorclassify = "");
        /// <summary>
        /// 用户列表（ALL）
        /// </summary>
        /// <returns></returns>
        DataTable GetAllTable();
        /// <summary>
        /// 用户基本信息
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        UserInfoEntity GetUserInfoEntity(string keyValue);
        /// <summary>
        /// 导出用户列表
        /// </summary>
        /// <returns></returns>
        DataTable GetExportList();
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        UserInfoEntity CheckLogin(string username);


        DataTable HaveRoleListByKey(string keyValue, string rolename);
        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        IList<UserInfoEntity> GetUserInfoByDeptCode(string deptCode);
        UserInfoEntity GetUserInfoByAccount(string account);
        /// <summary>
        /// 根据部门、用户名称获取用户信息
        /// </summary>
        /// <param name="deptName">部门名称</param>
        /// <param name="userName">用户姓名</param>
        /// <returns></returns>
        UserInfoEntity GetUserInfoByName(string deptName, string userName);
        /// <summary>
        /// 流程配置条件提供程序
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="deptmentid"></param>
        /// <param name="roleid"></param>
        /// <param name="isContains"></param>
        /// <returns></returns>
        IList<UserInfoEntity> GetWFUserListByDeptRoleOrg(string orgid, string deptmentid, string natrue, string roleid, string useraccounts, string rolename = "", string specialtytype ="");
         /// <summary>
        /// 根据部门编码或角色编码获取用户信息
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        IList<UserInfoEntity> GetUserListByCodeAndRole(string deptCode, string roleCode);
           /// <summary>
        /// 根据人员持证率得分（特种作业人员和安全管理人员持证率）
        /// </summary>
        /// <param name="user"></param>
        /// <param name="time">月份，如2017-10-01</param>
        /// <returns></returns>
        decimal GetIndexScoreByTime(ERCHTMS.Code.Operator user, string time = "");

        /// <summary>
         /// 同步部门信息到培训平台
         /// </summary>
         /// <param name="user">用户基本信息</param>
         /// <param name="pwd">密码</param>
         /// <returns></returns>
        string SyncUser(UserEntity user, string pwd, ERCHTMS.Code.Operator curUser=null);

          /// 获取人员来自培训平台培训档案
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        string GetTrainRecord(string userId, string userAccount, string deptId, string idCard = "");
          /// <summary>
         /// 获取人员来自培训平台的考试记录
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        string GetExamRecord(string userId, string userAccount, string deptId, string idCard = "");

        IEnumerable<UserInfoEntity> GetUserListBySql(string strSql);
        #endregion




    }
}
