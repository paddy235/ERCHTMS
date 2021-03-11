using BSFramework.Util.WebControl;
using ERCHTMS.Entity.HseManage.ViewModel;
using ERCHTMS.Entity.HseToolMange;
using ERCHTMS.IService.HseToolMange;
using ERCHTMS.Service.HseObserveManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Busines.HseToolManage
{
  public  class HseObserveBLL
    {

        private HseObserveIService service = new HseObserveService();


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
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public HseObserveEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, HseObserveEntity entity)
        {
            service.SaveForm(keyValue, entity);
        }
        /// <summary>
        /// 获取参与率
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="deptEncode">部门编码</param>
        /// <returns></returns>
        public List<HseKeyValue> GetCYLData(string year, string deptEncode)
        {
            return service.GetCYLData(year, deptEncode);
        }
        /// <summary>
        /// 获取各部门各月份的参与率
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="organizeCode">机构编码</param>
        /// <returns></returns>
        public DataTable GetDeptCYL(string year, string organizeCode)
        {
            return service.GetDeptCYL(year, organizeCode);
        }
        /// <summary>
        /// 危险项统计
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <returns></returns>
        public List<HseKeyValue> GetWXXCount(string year, string month)
        {
            return service.GetWXXCount(year, month);
        }
        /// <summary>
        /// 获取危险项每月数据
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public DataTable GetWXX(string year)
        {
            return service.GetWXX(year);
        }
    }
}
