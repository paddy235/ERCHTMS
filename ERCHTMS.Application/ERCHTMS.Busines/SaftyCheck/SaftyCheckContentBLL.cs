using ERCHTMS.Entity.SaftyCheck;
using ERCHTMS.IService.SaftyCheck;
using ERCHTMS.Service.SaftyCheck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Busines.SaftyCheck
{
   public class SaftyCheckContentBLL
    {
        private SaftyCheckContentIService service = new SaftyCheckContentService();
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SaftyCheckContentEntity> GetList(string queryJson)
        {   
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 根据检查项目Id获取检查结果信息
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public SaftyCheckContentEntity Get(string itemId)
        {
            return service.Get(itemId);
        }
        public SaftyCheckContentEntity GetEntity(string id)
        {
            return service.GetEntity(id);
        }
        public void Update(string keyValue, SaftyCheckContentEntity entity)
        {
            service.Update(keyValue, entity);
        }
        public int SaveNotice(string keyValue,CheckNoticeEntity sn)
        {
            return service.SaveNotice(keyValue, sn);
        }
        #endregion

        #region
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, SaftyCheckContentEntity entity)
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
        #endregion
    }
}
