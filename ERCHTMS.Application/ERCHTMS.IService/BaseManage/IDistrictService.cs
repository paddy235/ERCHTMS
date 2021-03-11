using ERCHTMS.Entity.BaseManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System;
using ERCHTMS.Entity.RiskDatabase;

namespace ERCHTMS.IService.BaseManage
{
    /// <summary>
    /// 描 述：区域设置
    /// </summary>
    public interface IDistrictService
    {
        #region 获取数据

        /// <summary>
        /// 获取部门管控区域名称集合
        /// </summary>
        /// <param name="deptId">部门Id</param>
        /// <returns>返回列表Json</returns>
        DataTable GetDeptNames(string deptId);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<DistrictEntity> GetList(string orgID = "");

        IEnumerable<DistrictEntity> GetListForCon(Expression<Func<DistrictEntity, bool>> condition);
        IEnumerable<DistrictEntity> GetOrgList(string orgID);


        /// <summary>
        /// 根据一定的条件获取区域信息
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        List<DistrictEntity> GetListByOrgIdAndParentId(string orgId, string parentId);
        List<DistrictEntity> GetDistricts(string companyid, string districtId);

        /// <summary>
        /// 获取名称和ID
        /// </summary>
        /// <param name="IDS">id集合</param>
        /// <returns>返回列表</returns>
        DataTable GetNameAndID(string ids);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        DistrictEntity GetEntity(string keyValue);

        /// <summary>
        /// 区域列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        IEnumerable<DistrictEntity> GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 根据机构Id和区域名称获取区域
        /// </summary>
        /// <param name="orgId">机构Id</param>
        /// <param name="name">区域名称</param>
        /// <returns></returns>

        DistrictEntity GetDistrict(string orgId, string name);
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
        void SaveForm(string keyValue, DistrictEntity entity);
        #endregion
    }
}
