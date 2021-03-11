using ERCHTMS.Entity.SaftyCheck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.IService.SaftyCheck
{
    public interface SaftyCheckContentIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<SaftyCheckContentEntity> GetList(string queryJson);
         /// <summary>
        /// 根据检查项目Id获取检查结果信息
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        SaftyCheckContentEntity Get(string itemId);
        SaftyCheckContentEntity GetEntity(string id);
        void Update(string keyValue, SaftyCheckContentEntity entity);
        #endregion

        #region 提交数据
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveForm(string keyValue, SaftyCheckContentEntity entity);
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue);
        int SaveNotice(string keyValue, CheckNoticeEntity sn);
        #endregion
    }
}
