using BSFramework.Util.WebControl;
using ERCHTMS.Entity.RiskDatabase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.IService.RiskDatabase
{
    public interface IRiskEvaluateService
    {
        DataTable GetPageList(Pagination pagination, string queryJson);
        IEnumerable<RiskEvaluate> GetList();
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
        void SaveForm(string keyValue, RiskEvaluate entity);
    }
}
