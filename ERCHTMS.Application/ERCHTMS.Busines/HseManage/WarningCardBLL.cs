using ERCHTMS.Entity.HseManage;
using ERCHTMS.Entity.HseManage.ViewModel;
using ERCHTMS.IService.HseManage;
using ERCHTMS.Service.HseManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Busines.HseManage
{
    public class WarningCardBLL
    {
        private IWarningCardService service;

        public WarningCardBLL()
        {
            this.service = new WarningCardService();
        }
        public List<WarningCardEntity> GetData(string key, int pagesize, int pageindex, out int total)
        {
            return this.service.GetData(key, pagesize, pageindex, out total);
        }

        public void Save(WarningCardEntity model)
        {
            this.service.Save(model);
        }

        public void Remove(string id)
        {
            this.service.Remove(id);
        }

        public WarningCardEntity GetDetail(string id)
        {
            return this.service.GetDetail(id);
        }

        public List<WarningCardEntity> GetMine(string userId, int pageSize, int pageIndex, out int total)
        {
            return this.service.GetMine(userId, pageSize, pageIndex, out total);
        }

        public List<WarningCardEntity> GetList(string[] deptId, string key, DateTime? from, DateTime? to, int pageSize, int pageIndex, out int total)
        {
            return this.service.GetList(deptId, key, from, to, pageSize, pageIndex, out total);
        }
        /// <summary>
        /// 获取安全比趋势图数据
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="deptId">部门ID</param>
        /// <returns></returns>
        public List<HseKeyValue> GetAQBData(string year, string deptId)
        {
            return service.GetAQBData(year, deptId);
        }
        /// <summary>
        ///  获取预警指标卡统计数据
        /// </summary>
        /// <param name="deptIds">部门</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns></returns>
        public List<HseKeyValue> GetWarningCardCount(List<string> deptIds, string start, string end)
        {
            return service.GetWarningCardCount(deptIds, start, end);
        }
        /// <summary>
        /// 获取参与度趋势统计图
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="deptId">部门ID</param>
        /// <returns></returns>
        public List<HseKeyValue> GetCYDData(string year, string deptId)
        {
            return service.GetCYDData(year, deptId);
        }
    }
}
