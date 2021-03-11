using ERCHTMS.Entity.BaseManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System;
using BSFramework.Data.Repository;
using System.Data.Common;

namespace ERCHTMS.IService.BaseManage
{
    /// <summary>
    /// 描 述：用户管理
    /// </summary>
    public interface IUserService
    {
        #region 获取数据

        /// <summary>
        ///获取培训人员记录(来自工具箱同步过来的数据)
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        DataTable GetTrainUsersPageList(Pagination pagination, string queryJson);
        IEnumerable<UserEntity> GetListForCon(Expression<Func<UserEntity, bool>> condition);

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        DataTable GetTable(string deptId = "");

        int ExcuteUser(string sql, DbParameter[] dbparams);

        int ExcuteBySql(string sql);

        /// <summary> 
        /// 通过用户id获取用户列表
        /// </summary>
        /// <returns></returns>
        DataTable GetUserTable(string[] userids);
        UserInfoEntity CheckUserLogin(string username);
        DataTable GetMembers(string deptId);
        DepartmentEntity GetUserOrganizeInfo(UserInfoEntity userEntity);
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<UserEntity> GetList();
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 用户列表（ALL）
        /// </summary>
        /// <returns></returns>
        DataTable GetAllTable();
        /// <summary>
        /// 用户实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        UserEntity GetEntity(string keyValue);
        /// <summary>
        /// 获取用户拥有的角色名称，多个用英文逗号分隔
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        string GetRoleName(string userId);
        /// <summary>
        /// 获取用户拥有的角色ID，多个用英文逗号分隔
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        string GetRoleId(string userId);
        /// <summary>
        /// 根据当前用户角色获取用户所属单位信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        DepartmentEntity GetUserOrgInfo(string userId);

        /// <summary>
        /// 根据用户ID和类别获取用户拥有的资源Id，多个用英文逗号分隔
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="category">数据类别，1:部门名称,2:角色名称,3:岗位名称,4:职位名称,5:工作组</param>
        /// <returns></returns>
        string GetObjectId(string userId, int category);
        /// <summary>
        /// 根据用户ID和类别获取用户拥有的资源名称，多个用英文逗号分隔
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="category">数据类别，1:部门名称,2:角色名称,3:岗位名称,4:职位名称,5:工作组</param>
        /// <returns></returns>
        string GetObjectName(string userId, int category);
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        UserEntity CheckLogin(string username);
        /// <summary>
        /// 导出用户列表
        /// </summary>
        /// <returns></returns>
        DataTable GetExportList(string condition, string keyword, string code);

        /// <summary>
        /// 根据角色获取用户基本信息
        /// </summary>
        /// <param name="category"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        IList<UserEntity> GetUserListByRole(string deptmentid, string roleCode, string orgid);

      

        /// <summary>
        /// 根据当前用户获取上级部门的安全管理员用户
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        IList<UserEntity> GetParentUserByCurrent(string userID, string userRoleCode);

        IList<UserEntity> GetUserListByDeptCode(string deptCode, string roleCode, bool isSplit, string orgid);
         /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        IList<UserEntity> GetUserListByDeptId(string deptId, string roleId, bool isSplit, string orgid);


        IList<UserEntity> GetUserListByRoleName(string deptId, string roleName, bool isSplit, string orgid);

        DataTable GetAllTableByArgs(string argValue, string organizeid);

        DataTable GetAllTableByArgs(string username, string deptid, string organizeid, string sjorgid,string reqmark, string threeperson = "");
        /// <summary>
        /// 通过身份证号获取用户信息
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        UserEntity GetUserByIdCard(string idCard);

        /// <summary>
        /// 根据单位编码统计单位人员信息
        /// </summary>
        /// <param name="deptCode">单位编码</param>
        /// <returns></returns>
        List<string> GetStatByDeptCode(string deptCode, string deptType = "");

        /// <summary>
        /// 同步外包工程人员到双控平台用户
        /// </summary>
        /// <param name="projectId">工程Id</param>
        /// <param name="deptId">外包单位Id</param>
        /// <returns></returns>
        bool SyncUsers(string projectId, string deptId);

        /// <summary>
        /// 分页查询用户
        /// </summary>
        /// <param name="pageindex">页数</param>
        /// <param name="pagesize">分页大小</param>
        /// <returns></returns>
        IEnumerable<UserEntity> GetUserList();
        /// <summary>
        /// 人员统计（集团）
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="newCode"></param>
        /// <param name="deptType"></param>
        /// <returns></returns>
        List<string> GetStatByDeptCodeForGroup(string deptCode, string newCode, string deptType = "0");
          /// <summary>
        /// 上传个人签名
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="signImg">图片名称</param>
        /// <returns></returns>
        int UploadSignImg(string userId, string signImg,int mode=0);
        /// <summary>
        /// 根据角色和部门获取用户账号和姓名
        /// </summary>
        /// <param name="orgid">厂级Id</param>
        /// <param name="deptid">部门Id</param>
        /// <param name="rolename">角色名称</param>
        /// <returns></returns>
        DataTable GetUserAccountByRoleAndDept(string orgid, string deptid, string rolename);
        #endregion

        #region 验证数据
        /// <summary>
        /// 账户不能重复
        /// </summary>
        /// <param name="account">账户值</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        bool ExistAccount(string account, string keyValue);
        /// <summary>
        /// 身份证不能重复
        /// </summary>
        /// <param name="IdentifyID">身份证号</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        bool ExistIdentifyID(string IdentifyID, string keyValue);
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// 保存用户表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="userEntity">用户实体</param>
        /// <returns></returns>
        string SaveForm(string keyValue, UserEntity userEntity,int mode=0);
        /// <summary>
        /// 修改用户登录密码
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="Password">新密码（MD5 小写）</param>
        bool RevisePassword(string keyValue, string Password);
        /// <summary>
        /// 修改用户状态
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="State">状态：1-启动；0-禁用</param>
        void UpdateState(string keyValue, int State);
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="userEntity">实体对象</param>
        void UpdateEntity(UserEntity userEntity);
        /// <summary>
        /// 设置用户黑名单状态
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="status">状态值（0:不是黑名单，1：是黑名单）</param>
        /// <returns></returns>
        int SetBlack(string userId, int status = 0);
        /// <summary>
        /// 人员离场
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="leaveTime">离场时间</param>
        /// <returns></returns>
        /// <summary>
        int SetLeave(string userId, string leaveTime, string DepartureReason);

        List<UserEntity> GetList(string[] users, int pageSize, int pageIndex, out int total);

        /// <summary>
        /// 人员转岗
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="newDeptId">新部门Id</param>
        /// <param name="newPostId">新的岗位Id</param>
        /// <param name="newPostName">新的岗位名称</param>
        /// <param name="newDutyId">新的职务Id</param>
        /// <param name="newDutyName">新的职务名称</param>
        /// <param name="time"></param>
        /// <returns></returns>
        int LeavePost(string userId, string newDeptId, string newPostId, string newPostName, string newDutyId, string newDutyName, string time);
        #endregion


        /// <summary>
        /// 保存用户表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="userEntity">用户实体</param>
        /// <returns></returns>
        string SaveOnlyForm(string keyValue, UserEntity userEntity);

        DataTable GetUserByDeptCodeAndRoleName(string userid, string deptcode, string rolename);
        bool ExistMoblie(string mobile, string keyValue);

        IEnumerable<UserEntity> FindList(string strSql, DbParameter[] dbParameter);
        UserEntity GetUserInfoByUserName(string username);

        /// <summary>
        /// 修改人员离场审批状态
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="isLeaving">是否离场审批中</param>
        /// <returns></returns>
        int UpdateUserLeaveState(string userid, string isLeaving);
    }
}
