using ERCHTMS.Entity.EngineeringManage;
using ERCHTMS.IService.EngineeringManage;
using ERCHTMS.Service.EngineeringManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.EngineeringManage
{
    /// <summary>
    /// 描 述：危大工程管理
    /// </summary>
    public class PerilEngineeringBLL
    {
        private PerilEngineeringIService service = new PerilEngineeringService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<PerilEngineeringEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public PerilEngineeringEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
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
        public void SaveForm(string keyValue, PerilEngineeringEntity entity)
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

        #region 统计
        /// <summary>
        ///获取统计数据
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        public string GetEngineeringCount(string year = "")
        {
            return service.GetEngineeringCount(year);
        }

        /// <summary>
        ///获取统计表格数据
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        public string GetEngineeringList(string year = "")
        {
            return service.GetEngineeringList(year);
        }


        /// <summary>
        ///获取施工方案,技术交底统计数据
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        public string GetEngineeringFile(string year = "")
        {
            return service.GetEngineeringFile(year);
        }

        /// <summary>
        ///获取施工方案,技术交底统计数据（表格）
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        public string GetEngineeringFileGrid(string year = "")
        {
            return service.GetEngineeringFileGrid(year);
        }

        /// <summary>
        ///危大工程完成情况统计
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        public string GetEngineeringCase(string year = "")
        {
            return service.GetEngineeringCase(year);
        }

        /// <summary>
        ///危大工程完成情况统计(表格)
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        public string GetEngineeringCaseGrid(string year = "")
        {
            return service.GetEngineeringCaseGrid(year);
        }

        /// <summary>
        ///单位内部、各外包单位对比
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        public string GetEngineeringContrast(string year = "", string month = "")
        {
            return service.GetEngineeringContrast(year, month);
        }

        /// <summary>
        ///单位内部、各外包单位对比（表格）
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        public string GetEngineeringContrastGrid(string year = "", string month = "")
        {
            return service.GetEngineeringContrastGrid(year, month);
        }
        #endregion

        #region 省级统计
        /// <summary>
        ///各电厂单位对比
        /// </summary>
        /// <param name="year">统计年份</param>
        /// <returns></returns>
        public string GetEngineeringContrastForSJ(string year = "")
        {
            return service.GetEngineeringContrastForSJ(year);
        }

        /// <summary>
        /// 各电厂单位对比表格
        /// </summary>
        /// <param name="year">统计年份</param>
        /// <returns></returns>
        public DataTable GetEngineeringContrastGridForSJ(string year = "")
        {
            return service.GetEngineeringContrastGridForSJ(year);
        }

        /// <summary>
        /// 工程类别统计表格
        /// </summary>
        /// <param name="year">统计年份</param>
        /// <returns></returns>
        public DataTable GetEngineeringCategoryGridForSJ(string year = "")
        {
            return service.GetEngineeringCategoryGridForSJ(year);
        }

        /// <summary>
        /// 工程类别图形
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public string GetEngineeringCategoryForSJ(string year = "")
        {
            return service.GetEngineeringCategoryForSJ(year);
        }

        /// <summary>
        /// 月度趋势图形
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public string GetEngineeringMonthForSJ(string year = "")
        {
            return service.GetEngineeringMonthForSJ(year);
        }

        /// <summary>
        /// 月度趋势表格
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public DataTable GetEngineeringMonthGridForSJ(string year = "")
        {
            return service.GetEngineeringMonthGridForSJ(year);
        }

        /// <summary>
        /// 获取工程类别
        /// </summary>
        /// <returns></returns>
        public DataTable GetEngineeringType()
        {
            return service.GetEngineeringType();
        }
        #endregion

        #region 列表页面的统计
        public string GetPeril(string code = "", string st = "", string et = "", string keyword = "")
        {
            return service.GetPeril(code, st, et, keyword);
        }

        public string GetPerilForSJIndex(string queryJson)
        {
            return service.GetPerilForSJIndex(queryJson);
        }
        #endregion

        #region app接口
        public DataTable GetPerilEngineeringList(string sqlwhere)
        {
            return service.GetPerilEngineeringList(sqlwhere);
        }
        #endregion
    }
}
