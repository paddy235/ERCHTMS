using BSFramework.Util.WebControl;
using ERCHTMS.Entity.HseManage.ViewModel;
using ERCHTMS.Entity.HseToolMange;
using ERCHTMS.IService.HseToolMange;
using ERCHTMS.Service.HseObserveManage;
using ERCHTMS.Service.HseToolManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Busines.HseToolManage
{
    /// <summary>
    /// 安全观察内容标准
    /// </summary>
    public class HseObserveNormBLL
    {
        private HseObserveNormIService service = new HseObserveNormService();


        /// <summary>
        /// 获取台账分页数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public List<HseObserveNormEntity> GetList() {
            return service.GetList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public HseObserveNormEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="keyValue"></param>
        public void RemoveForm(string keyValue)
        {
            service.RemoveForm(keyValue);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void SaveForm(string keyValue, HseObserveNormEntity entity)
        {
            service.SaveForm(keyValue, entity);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="entity"></param>
        public void SaveFormList(List<HseObserveNormEntity> entity)
        {
            service.SaveFormList(entity);
        }
    }
}
