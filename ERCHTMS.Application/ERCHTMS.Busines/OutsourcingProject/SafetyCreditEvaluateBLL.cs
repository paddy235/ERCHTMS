using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using ERCHTMS.Service.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;


namespace ERCHTMS.Busines.OutsourcingProject
{
    public class SafetyCreditEvaluateBLL
    {
        private SafetyCreditEvaluateIService service = new SafetyCreditEvaluateService();

        #region 获取数据
        /// <summary>
        /// 获取项目基本信息
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        public DataTable GetEngineerDataById(string orgid)
        {
            return service.GetEngineerDataById(orgid);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SafetyCreditEvaluateEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetList(Pagination pagination, string queryJson)
        {
            return service.GetList(pagination, queryJson);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SafetyCreditEvaluateEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public string RemoveForm(string keyValue)
        {
            string num = "0";
            try
            {
                num = service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                
            }

            return num;
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, SafetyCreditEvaluateEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception ex)
            {
                
            }
        }

        /// <summary>
        /// 结束评价
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void FinishForm(string keyValue)
        {
            try
            {
                service.FinishForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
