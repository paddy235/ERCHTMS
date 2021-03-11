using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.IService.RiskDatabase;
using ERCHTMS.Service.RiskDatabase;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using ERCHTMS.Code;
using System.Data;

namespace ERCHTMS.Busines.RiskDatabase
{
    /// <summary>
    /// 描 述：风险点信息
    /// </summary>
    public class DangerSourceBLL
    {
        private DangerSourceIService service = new DangerSourceService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<DangerSourceEntity> GetList()
        {
            return service.GetList();
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="parentId">区域Id</param>
        /// <param name="keyword">关键字查询</param>
        /// <returns></returns>
        public IEnumerable<DangerSourceEntity> GetList(string parentId, string keyword = "")
        {
            return service.GetList(parentId, keyword);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public DangerSourceEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

          /// <summary>
        /// 根据名称查询记录是否存在
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public string GetIdByName(string name)
        {
            return service.GetIdByName(name);
        }
        /// <summary>
        /// 获取下拉选项html字符串
        /// <param name="parentId">父节点Id</param>
        /// </summary>
        public string GetOptionsStringForArea(string parentId, string orgCode = "")
        {
            return service.GetOptionsStringForArea(parentId, orgCode);
        }
        /// <summary>
        /// 获取内置部门信息
        /// </summary>
        public string GetOptionsStringForInitDept()
        {
            return service.GetOptionsStringForInitDept();
        }
        /// <summary>
        /// 获取内置岗位信息
        /// </summary>
        public string GetOptionsStringForInitPost(string deptName)
        {
            return service.GetOptionsStringForInitPost(deptName);
        }

            /// <summary>
        /// 根据区域编码获取名称全路径,格式如1>1.1>1.1.1
        /// </summary>
        /// <param name="code">区域编码</param>
        /// <returns></returns>
        public string GetPathName(string code, string orgId)
        {
            return service.GetPathName(code,orgId);
        }
         /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询条件</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
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
        public void SaveForm(string keyValue, DangerSourceEntity entity)
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
        /// 修改内置风险点和区域间的关系
        /// </summary>
        /// <param name="districtId">区域ID</param>
        /// <param name="areaId">内置区域ID,多个用英文逗号分割</param>
        /// <param name="areaCode">区域Code</param>
        /// <param name="areaName">区域名称</param>
        /// <param name="deptCode">管控部门Code</param>
        ///  <param name="user">当前操作用户</param>
        /// <returns></returns>
        public int Update(string districtId, string areaId, string areaCode, string areaName, string deptCode, Operator user)
        {
            return service.Update(districtId, areaId, areaCode, areaName, deptCode, user);
        }
         /// <summary>
        /// 保存部门与内置部门的风险配置清单信息
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="deptName">部门名称</param>
        /// <param name="newDeptName">关联的内置部门名称，多个用英文逗号分隔</param>
        /// <param name="postName">关联的岗位名称，多个用英文逗号分隔</param>
        /// <param name="newPostName">岗位名称</param>
        /// <param name="postName">岗位Id</param>
        /// <param name="postName">当前用户对象</param>
        /// <returns></returns>
        public int SaveConfig(string deptCode, string deptName, string newDeptName, string postName, string newPostName, string postId, Operator user)
        {
            return service.SaveConfig(deptCode, deptName, newDeptName, postName, newPostName, postId, user);
        }
        #endregion
    }
}
