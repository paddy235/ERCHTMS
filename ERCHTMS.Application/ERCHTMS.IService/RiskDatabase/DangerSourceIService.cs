using ERCHTMS.Entity.RiskDatabase;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using ERCHTMS.Code;
using System.Data;

namespace ERCHTMS.IService.RiskDatabase
{
    /// <summary>
    /// 描 述：风险点信息
    /// </summary>
    public interface DangerSourceIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<DangerSourceEntity> GetList();
        IEnumerable<DangerSourceEntity> GetList(string parentId, string keyword);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        DangerSourceEntity GetEntity(string keyValue);
        /// <summary>
        /// 根据名称查询记录是否存在
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        string GetIdByName(string name);
         /// <summary>
        /// 获取下拉选项html字符串
        /// <param name="parentId">父节点Id</param>
        /// </summary>
        string GetOptionsStringForArea(string parentId, string orgCode = "");
        /// <summary>
        /// 获取内置部门信息
        /// </summary>
        string GetOptionsStringForInitDept();
        /// <summary>
        /// 获取内置岗位信息
        /// </summary>
        string GetOptionsStringForInitPost(string deptName);

         /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询条件</param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);

        
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveForm(string keyValue, DangerSourceEntity entity);

         /// <summary>
        /// 修改内置风险点和区域间的关系
        /// </summary>
        /// <param name="districtId">区域ID</param>
        /// <param name="areaId">内置区域ID,多个用英文逗号分割</param>
        /// <param name="areaCode">区域Code</param>
        /// <param name="areaName">区域名称</param>
        /// <param name="deptCode">管控部门Code</param>
        /// <param name="user">当前操作用户</param>
        /// <returns></returns>
        int Update(string districtId, string areaId, string areaCode, string areaName,string deptCode,Operator user);
        /// <summary>
        /// 保存部门与内置部门的风险配置清单信息
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="deptName">部门名称</param>
        /// <param name="newDeptName">关联的内置部门名称，多个用英文逗号分隔</param>
        /// <param name="postName">关联的岗位名称，多个用英文逗号分隔</param>
        /// <param name="newPostName">岗位名称</param>
        /// <param name="postName">岗位Id</param>
        /// <returns></returns>
        int SaveConfig(string deptCode, string deptName, string newDeptName, string postName, string newPostName, string postId, Operator user);
            /// <summary>
        /// 根据区域编码获取名称全路径,格式如1>1.1>1.1.1
        /// </summary>
        /// <param name="code">区域编码</param>
        /// <returns></returns>
        string GetPathName(string code, string orgId);
        #endregion
    }
}