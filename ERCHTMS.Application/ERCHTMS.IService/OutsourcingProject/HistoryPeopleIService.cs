using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSFramework.Util.WebControl;
using ERCHTMS.Entity.OutsourcingProject;

namespace ERCHTMS.IService.OutsourcingProject
{
    public interface HistoryPeopleIService
    {
        HistoryPeople GetEntity(string keyValue);
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
        void SaveForm(string keyValue, HistoryPeople entity);

        /// <summary>
        /// 获取人员历史记录分页显示
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetHistoryPageList(Pagination pagination, string queryJson);
    }
}
