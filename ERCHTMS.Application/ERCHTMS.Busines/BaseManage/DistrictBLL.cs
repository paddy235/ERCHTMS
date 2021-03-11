using ERCHTMS.Entity.BaseManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.Service.BaseManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using System.Linq.Expressions;
using ERCHTMS.Entity.RiskDatabase;

namespace ERCHTMS.Busines.BaseManage
{
    /// <summary>
    /// 描 述：区域设置
    /// </summary>
    public class DistrictBLL
    {
        private IDistrictService service = new DistrictService();

        #region 获取数据
        /// <summary>
        /// 获取部门管控区域名称集合
        /// </summary>
        /// <param name="deptId">部门Id</param>
        /// <returns>返回列表Json</returns>
        public DataTable GetDeptNames(string deptId)
        {
            return service.GetDeptNames(deptId);
        }
        /// <summary>
        /// 根据公司id查询其下所有区域
        /// </summary>
        /// <param name="orgID"></param>
        /// <returns></returns>
        public IEnumerable<DistrictEntity> GetOrgList(string orgID)
        {
            return service.GetOrgList(orgID);
        }

        public List<DistrictEntity> GetDistricts(string companyid, string districtId)
        {
            return service.GetDistricts(companyid, districtId);
        }

        /// <summary>
        /// 根据一定的条件获取区域信息
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<DistrictEntity> GetListByOrgIdAndParentId(string orgId, string parentId)
        {
            try
            {
                return service.GetListByOrgIdAndParentId(orgId, parentId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IEnumerable<DistrictEntity> GetListForCon(Expression<Func<DistrictEntity, bool>> condition)
        {
            return service.GetListForCon(condition);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<DistrictEntity> GetList(string orgID = "")
        {
            return service.GetList(orgID);
        }
        /// <summary>
        /// 获取名称和ID
        /// </summary>
        /// <param name="IDS">id集合</param>
        /// <returns>返回列表</returns>
        public DataTable GetNameAndID(string ids)
        {
            return service.GetNameAndID(ids);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public DistrictEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 区域列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<DistrictEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// 根据机构Id和区域名称获取区域
        /// </summary>
        /// <param name="orgId">机构Id</param>
        /// <param name="name">区域名称</param>
        /// <returns></returns>
        public DistrictEntity GetDistrict(string orgId, string name)
        {
            return service.GetDistrict(orgId, name);
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
        public void SaveForm(string keyValue, DistrictEntity entity)
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

        #endregion
    }
}
