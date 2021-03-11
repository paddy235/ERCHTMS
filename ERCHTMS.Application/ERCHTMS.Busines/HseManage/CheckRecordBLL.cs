using ERCHTMS.Entity.HseManage;
using ERCHTMS.IService.HseManage;
using ERCHTMS.Service.HseManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Busines.HseManage
{
    public class CheckRecordBLL
    {
        private ICheckRecordService service;

        public CheckRecordBLL()
        {
            this.service = new CheckRecordService();
        }
        public List<CheckRecordEntity> GetData(string userid, int pagesize, int pageindex, out int total)
        {
            return this.service.GetData(userid, pagesize, pageindex, out total);
        }

        public void Save(CheckRecordEntity model)
        {
            this.service.Save(model);
        }

        public void Remove(string id)
        {
            this.service.Remove(id);
        }

        public CheckRecordEntity GetDetail(string id)
        {
            return this.service.GetDetail(id);
        }

        public List<CheckRecordEntity> GetMine(string userId, int pageSize, int pageIndex, out int total)
        {
            return this.service.GetMine(userId, pageSize, pageIndex, out total);
        }

        public List<CheckRecordEntity> GetList(string[] deptId, string key, DateTime? from, DateTime? to, int pageSize, int pageIndex, out int total)
        {
            return this.service.GetList(deptId, key, from, to, pageSize, pageIndex, out total);
        }
        public List<CheckRecordEntity> GetList(string[] deptid, string checkuser, string key, DateTime? from, DateTime? to, int pageSize, int pageIndex, out int total)
        {
            return this.service.GetList(deptid, checkuser, key, from, to, pageSize, pageIndex, out total);
        }
    }
}
