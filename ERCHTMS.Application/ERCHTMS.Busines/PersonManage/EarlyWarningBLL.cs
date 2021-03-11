using System.Collections.Generic;
using BSFramework.Util.WebControl;
using ERCHTMS.Entity.PersonManage;
using ERCHTMS.IService.PersonManage;
using ERCHTMS.Service.PersonManage;

namespace ERCHTMS.Busines.PersonManage
{
    public class EarlyWarningBLL
    {
        IEarlyWarningService service = null;
        public EarlyWarningBLL()
        {
            service = new EarlyWarningService();
        }

        /// <summary>
        /// 分页查询人员行为安全管控预警数据
        /// </summary>
        /// <param name="queryJson"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public IEnumerable<EarlyWarningEntity> GetPageList(string queryJson, Pagination pagination)
        {
            IEnumerable<EarlyWarningEntity> result = new List<EarlyWarningEntity>();
            if (service != null)
                result = service.GetPageList(queryJson, pagination);  
            return result;
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public EarlyWarningEntity GetEntity(string keyValue)
        {
            if (service != null)
                return service.GetEntity(keyValue);
            else
                return null;
        }

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public bool SaveForm(string keyValue, EarlyWarningEntity entity)
        {
            if (service != null)
                return service.SaveForm(keyValue, entity);
            else
                return false;
        }
    }
}
