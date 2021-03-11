using ERCHTMS.Entity.OccupationalHealthManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System;

namespace ERCHTMS.IService.OccupationalHealthManage
{
    /// <summary>
    /// 描 述：职业病人详情表
    /// </summary>
    public interface OccupationalstaffdetailIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<OccupationalstaffdetailEntity> GetList(string queryJson);
        /// <summary>
        /// 获取职业病统计表
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        DataTable GetStatisticsUserTable(string year, string where);
        /// <summary>
        /// 获取新的职业病统计表
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        DataTable NewGetStatisticsUserTable(string year, string where);
        /// <summary>
        /// 获取部门职业病统计表
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        DataTable GetStatisticsDeptTable(string year, string where);

        /// <summary>
        /// 获取年度职业病统计表
        /// </summary>
        /// <param name="yearType">近几年数据</param>
        /// <param name="Dept">部门EnCode</param>
        /// <returns></returns>
        DataTable GetStatisticsYearTable(int yearType, string Dept, string where);
        /// <summary>
        /// 获取用户id下的所有体检记录
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        DataTable GetUserTable(string userid);

        /// <summary>
        /// 获取职业病人员清单人员数量
        /// </summary>
        /// <param name="queryJson">查询条件</param>
        /// <returns></returns>
        int GetStaffListSum(string queryJson, string where);
        /// <summary>
        /// 获取职业病人员清单(全部的)
        /// </summary>
        /// <param name="queryJson">查询条件</param>
        /// <returns></returns>
        DataTable GetStaffList(string queryJson, string where);
        /// <summary>
        /// 获取职业病人员清单
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetStaffListPage(Pagination pagination, string queryJson);
        /// <summary>
        /// 根据条件查询所有数据
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetTable(string queryJson,string where);
        /// <summary>
        /// 根据父id和是否生病查询表中信息
        /// </summary>
        /// <param name="Pid">父id</param>
        /// <param name="SickType">是否生病</param>
        /// <returns></returns>
        IEnumerable<OccupationalstaffdetailEntity> GetList(string Pid, int Issick);

        /// <summary>
        /// 存储过程分页
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        DataTable GetPageListByProc(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        OccupationalstaffdetailEntity GetEntity(string keyValue);

        /// <summary>
        /// 获取用户id下的所有体检记录 新
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        DataTable NewGetUserTable(string userid);

        /// <summary>
        /// 获取用户的接触危害因素
        /// </summary>
        /// <returns></returns>
        DataTable GetUserHazardfactor(string useraccount);
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
        void SaveForm(string keyValue, OccupationalstaffdetailEntity entity);

        /// <summary>
        /// 根据关联id批量修改体检时间
        /// </summary>
        /// <param name="time">体检时间</param>
        /// <param name="parenid">关联id</param>
        /// <returns></returns>
        int UpdateTime(DateTime time, string parenid);

        /// <summary>
        /// 根据条件删除健康用户 重新添加
        /// </summary>
        /// <param name="time">体检时间</param>
        /// <param name="parenid">关联id</param>
        /// <returns></returns>
        int Delete(string parenid, int SickType);
        #endregion
    }
}
