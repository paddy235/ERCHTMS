using ERCHTMS.Entity.SystemManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;

namespace ERCHTMS.IService.SystemManage
{
    /// <summary>
    /// 描 述：分项指标项目表
    /// </summary>
    public interface IClassificationIndexService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<ClassificationIndexEntity> GetList(string classificationId);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="organizeId"></param>
        /// <returns></returns>
        IEnumerable<ClassificationIndexEntity> GetListByOrganizeId(string organizeId);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        ClassificationIndexEntity GetEntity(string keyValue);

        ClassificationIndexEntity GetEntity(string organizeId, string classificationCode, string indexCode);
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
        void SaveForm(string keyValue, ClassificationIndexEntity entity);
        #endregion
    }
}