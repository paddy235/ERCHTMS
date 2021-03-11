using ERCHTMS.Entity.OccupationalHealthManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.OccupationalHealthManage
{
    /// <summary>
    /// 描 述：危险因素清单
    /// </summary>
    public interface HazardfactorsIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<HazardfactorsEntity> GetList(string queryJson);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="AreaId">区域id</param>
        /// <param name="where">其他查询条件</param>
        /// <returns></returns>
        DataTable GetList(string AreaId, string where);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        HazardfactorsEntity GetEntity(string keyValue);

        /// <summary>
        /// 验证区域名称是否重复
        /// </summary>
        /// <param name="AreaValue">区域名称</param>
        /// <returns></returns>
        bool ExistDeptJugement(string AreaValue, string orgCode, string RiskName);

        /// <summary>
        /// 验证区域id是否重复//区分不同公司用户
        /// </summary>
        /// <param name="Areaid">区域名称</param>
        /// <returns></returns>
        bool ExistAreaidJugement(string Areaid, string orgCode, string RiskName);

        /// <summary>
        /// 验证区域id和危险源是否重复//区分不同公司用户
        /// </summary>
        /// <param name="Areaid">区域名称</param>
        /// <returns></returns>
        bool ExistAreaidJugement(string Areaid, string orgCode, string RiskName, string Hid);

        /// <summary>
        /// 验证是否有该危险源，如果有返回Code 如果没有返回空字符串
        /// </summary>
        /// <param name="code">字典的Code</param>
        /// <param name="RiskName">危险源名称</param>
        /// <returns></returns>
        string IsRisk(string code, string RiskName);

        /// <summary>
        /// 根据条件查询所有数据
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetTable(string queryJson, string where);

        /// <summary>
        /// 存储过程分页
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        DataTable GetPageListByProc(Pagination pagination, string queryJson);
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
        void SaveForm(string keyValue, HazardfactorsEntity entity, string UserName, string UserId);
        #endregion
        #region 手机端
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<HazardfactorsEntity> PhoneGetList(string queryJson, string orgid);
        #endregion
    }
}
