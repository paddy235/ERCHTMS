using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSFramework.Util.WebControl;
using ERCHTMS.Entity.PersonManage;

namespace ERCHTMS.IService.PersonManage
{
    public interface IEarlyWarningService
    {

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<EarlyWarningEntity> GetPageList(string queryJson, Pagination pagination);

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        EarlyWarningEntity GetEntity(string keyValue);
        #endregion

        #region 提交数据
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        bool SaveForm(string keyValue, EarlyWarningEntity entity);
        #endregion
    }
}
