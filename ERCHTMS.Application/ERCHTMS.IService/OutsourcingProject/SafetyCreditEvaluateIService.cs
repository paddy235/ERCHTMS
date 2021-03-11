using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Entity.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;


namespace ERCHTMS.IService.OutsourcingProject
{
    public interface SafetyCreditEvaluateIService
    {
        #region 获取数据

        /// <summary>
        /// 获取项目基本信息
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        DataTable GetEngineerDataById(string orgid);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<SafetyCreditEvaluateEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        SafetyCreditEvaluateEntity GetEntity(string keyValue);

        DataTable GetList(Pagination pagination, string queryJson);
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        string RemoveForm(string keyValue);
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveForm(string keyValue, SafetyCreditEvaluateEntity entity);

        /// <summary>
        /// 结束评价数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void FinishForm(string keyValue);
        #endregion
    }
}
