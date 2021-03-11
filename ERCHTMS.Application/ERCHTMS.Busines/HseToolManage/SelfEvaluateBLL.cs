using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.HseToolMange;
using ERCHTMS.IService.HseToolMange;
using ERCHTMS.Service.HseToolManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Busines.HseToolManage
{
    public class SelfEvaluateBLL
    {
        private ISelfEvaluateService service = new SelfEvaluateService();
     

        public void SaveForm(SelfEvaluateEntity entity) 
        {
            service.SaveForm(entity);
        }
        public IEnumerable<SelfEvaluateEntity> GetList(string userid, string deptCode, string keyword="",string year=null,string month=null) 
        {
            return service.GetList(userid, deptCode, keyword,year,month);
        }
        public void RemoveForm(string id) 
        {
            service.RemoveForm(id);
        }
        public SelfEvaluateEntity GetEntity(string id) 
        {
            return service.GetEntity(id);
        }

        public void SaveSummary(EvaluateGroupSummaryEntity entity) 
        {
            service.SaveSummary(entity);
        }
        public EvaluateGroupSummaryEntity GetSummary(string year, string month) 
        {
            return service.GetSummary(year, month);
        }
        /// <summary>
        /// 获取各部门的自我评价参与度
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <param name="organizeCode">组织编码</param>
        /// <returns></returns>
        public DataTable GetChartsData(string year, string month, string organizeCode)
        {
            return service.GetChartsData(year, month, organizeCode);
        }
        /// <summary>
        /// 查询已提交的用户的iD
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        public List<string> GetSubmitByDeptCode(string deptCode, string year,string month)
        {
            return service.GetSubmitByDeptCode(deptCode, year, month);
        }
        /// <summary>
        /// 根据主键ID查找小结
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public EvaluateGroupSummaryEntity GetSummaryById(string id)
        {
            return service.GetSummaryById(id);
        }
        /// <summary>
        /// 获取安全危害的种类与人次
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        public DataTable GetDangerCount(string deptCode, string year, string month)
        {
            return service.GetDangerCount(deptCode,year,month);
        }
        /// <summary>
        /// 获取PPE的种类与人次
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        public DataTable GetPPECount(string deptCode, string year, string month)
        {
            return service.GetPPECount(deptCode,year,month);
        }
        /// <summary>
        /// 	HSE培训与授权情况
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public DataTable GetHseCount(string deptCode, string year, string month)
        {
            return service.GetHseCount(deptCode, year, month);
        }

        public decimal GetCycle(string category)
        {
            return service.GetCycle(category);
        }

        public decimal GetTimes(string category)
        {
            return service.Times(category);
        }

        /// <summary>
        /// 安全参与统计
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public DataTable GetSafeCount(string deptCode, string year, string month)
        {
            return service.GetSafeCount(deptCode, year, month);
        }
        /// <summary>
        /// 工余安健环
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="type">1 交通，2用电，3防火，4体力操作，5其他</param>
        /// <returns></returns>
        public DataTable GetFiveData(string deptCode, string year, string month, int type)
        {
            return service.GetFiveData(deptCode, year, month, type);
        }

        public EvaluateGroupSummaryEntity GetSummary(string year, string month, string deptId)
        {
            return service.GetSummary(year,month,deptId);
        }

        public decimal GetPeopleCount(string departmentId)
        {
            return service.GetPeopleCount(departmentId);
        }
    }
}
