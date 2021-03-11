using BSFramework.Util.WebControl;
using ERCHTMS.Entity.HseManage.ViewModel;
using ERCHTMS.Entity.HseToolMange;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.IService.HseToolMange
{
    public  interface HseObserveNormIService
    {

        /// <summary>
        /// 获取台账分页数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        List<HseObserveNormEntity> GetList();
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        HseObserveNormEntity GetEntity(string keyValue);
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
        void SaveForm(string keyValue, HseObserveNormEntity entity);

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveFormList(List<HseObserveNormEntity> entity);
    }
}
