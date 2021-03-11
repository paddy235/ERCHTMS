using ERCHTMS.Entity.SystemManage;
using ERCHTMS.IService.SystemManage;
using ERCHTMS.Service.SystemManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Busines.SystemManage
{
    /// <summary>
    /// 描 述：分项指标项目表
    /// </summary>
    public class ClassificationIndexBLL
    {
        private IClassificationIndexService service = new ClassificationIndexService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<ClassificationIndexEntity> GetList(string classificationId)
        {
            return service.GetList(classificationId);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ClassificationIndexEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 获取单个对象
        /// </summary>
        /// <param name="organizeId"></param>
        /// <param name="classificationCode"></param>
        /// <param name="indexCode"></param>
        /// <returns></returns>
        public ClassificationIndexEntity GetEntity(string organizeId, string classificationCode, string indexCode)
        {
            return service.GetEntity(organizeId, classificationCode, indexCode);
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
        public void SaveForm(string keyValue, ClassificationIndexEntity entity)
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