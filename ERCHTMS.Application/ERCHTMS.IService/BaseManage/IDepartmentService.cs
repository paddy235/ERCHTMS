using BSFramework.Util.WebControl;
using ERCHTMS.Entity.BaseManage;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace ERCHTMS.IService.BaseManage
{
    /// <summary>
    /// 描 述：部门管理
    /// </summary>
    public interface IDepartmentService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// 获取上一个结点的部门
        /// </summary>
        /// <param name="parentid"></param>
        /// <param name="nature"></param>
        /// <returns></returns>
        DepartmentEntity GetParentDeptBySpecialArgs(string parentid, string nature);
        /// <summary>
        /// 部门列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<DepartmentEntity> GetList();
        /// <summary>
        /// 部门实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        DepartmentEntity GetEntity(string keyValue);

        DepartmentEntity GetEntityByCode(string keyValue);

        DataTable GetDataTableByParams(string sql, DbParameter[] parameters);
        /// <summary>
        /// 判断是否存在上级
        /// </summary>
        /// <param name="departID"></param>
        /// <returns></returns>
        bool IsExistSuperior(string departID);

        /// <summary>
        /// 递归获取部门
        /// </summary>
        /// <returns></returns>
        IEnumerable<DepartmentEntity> GetDepts(string parentid);
        /// <summary>
        /// 获取机构厂级部门
        /// <param name="orgid">当前机构</param>
        /// <param name="orgid">是否厂级 0-否 1-是</param>
        /// </summary>
        IEnumerable<DepartmentEntity> GetDepts(string orgid, int isorg);
        /// <summary>
        /// 非黑名单的承包商列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<DepartmentEntity> GetNotBlackList();
        /// <summary>
        /// 获取承包商信息
        /// </summary>
        /// <param name="orgCode"></param>
        /// <returns></returns>
        DataTable GetContractDepts(string orgCode);
        /// <summary>
        /// 根据部门名称和机构Id获取部门编码
        /// </summary>
        /// <param name="deptName">部门名称</param>
        /// <param name="orgId">机构Id</param>
        /// <returns></returns>
        string GetDeptCode(string deptName, string orgId);

        /// <summary>
        /// 根据岗位名称和机构Id获取岗位Id
        /// </summary>
        /// <param name="postName">部门名称</param>
        /// <param name="orgId">机构Id</param>
        /// <param name="deptCode">部门编码</param>
        /// <returns></returns>
        string GetPostId(string postName, string orgId, string deptCode);
        /// <summary>
        /// 根据区域名称和机构Id获取区域Id
        /// </summary>
        /// <param name="areaName">区域名称</param>
        /// <param name="orgId">机构Id</param>
        /// <returns></returns>
        string GetAreaId(string areaName, string orgId);

        /// <summary>
        /// 获取当前用户所属的省级
        /// </summary>
        /// <param name="user">当前用户</param>
        /// <returns></returns>
        DepartmentEntity GetUserCompany(ERCHTMS.Code.Operator user);
        /// <summary>
        /// 获取指定部门的所属机构信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        DepartmentEntity GetDeptOrgInfo(string deptId);
        /// <summary>
        /// 获取省级下属管辖的所有电厂
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        DataTable GetAllFactory(ERCHTMS.Code.Operator user);

        DataTable GetDataTable(string sql);
        /// <summary>
        /// 获取DataTable
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="connStr">连接串key</param>
        /// <param name="DbType">1:Oracle,2:SqlServer,3:MySql</param>
        /// <returns></returns>
        DataTable GetDataTable(string sql, string connStr,int DbType);
         /// <summary>
        /// 执行sql操作
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="connStr">连接串key</param>
        /// <param name="DbType">1:Oracle,2:SqlServer,3:MySql</param>
        /// <returns></returns>
        int ExecuteSql(string sql, string connStr, int DbType = 1);
        int ExecuteSql(string sql);
        #endregion

        #region 验证数据
        /// <summary>
        /// 部门编号不能重复
        /// </summary>
        /// <param name="enCode">编号</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        bool ExistEnCode(string enCode, string keyValue);
        /// <summary>
        /// 部门名称不能重复
        /// </summary>
        /// <param name="fullName">名称</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        bool ExistFullName(string fullName, string keyValue);
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="keyValue">主键</param>
        bool RemoveForm(string keyValue, List<UserEntity> ue);
        /// <summary>
        /// 保存部门表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="departmentEntity">机构实体</param>
        /// <returns></returns>
        bool SaveForm(string keyValue, DepartmentEntity departmentEntity);
        /// <summary>
        /// 批量导入部门的时候部门code
        /// </summary>
        /// <param name="parentName">上级部门名称</param>
        /// <param name="orgId">当前用户所属机构id</param>
        /// <returns></returns>
        List<string> GetImportDeptCode(string parentName, string orgId);

        /// <summary>
        /// 验证部门名称是否重复
        /// </summary>
        /// <param name="departmentName">部门名称</param>
        /// <returns></returns>
        bool ExistDeptJugement(string departmentName, string orgId = "");

        /// <summary>
        /// 初始化培训平台部门和用户信息并保存到双控平台
        /// </summary>
        /// <param name="deptId">部门Id</param>
        /// <param name="deptCode">部门编码</param>
        /// <param name="jsonDepts">来自培训平台的部门信息</param>
        /// <param name="jsonUsers">来自培训平台的用户信息</param>
        /// <returns></returns>
        string SyncDeptForTrain(string deptId, string deptCode, string jsonDepts, string jsonUsers, string deptKey = "");
        /// <summary>
        /// 同步部门信息到培训平台
        /// </summary>
        /// <param name="dept"></param>
        /// <returns></returns>
        string SyncDept(DepartmentEntity dept, string keyValue);
        #endregion

        List<DepartmentEntity> GetSubDepartments(string id, string category);

        DepartmentEntity GetCompany(string deptId);

    }
}
