using ERCHTMS.Entity.NosaManage;
using ERCHTMS.IService.NosaManage;
using ERCHTMS.Service.NosaManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;


namespace ERCHTMS.Busines.NosaManage
{
    /// <summary>
    /// 描 述：工作成果
    /// </summary>
    public class NosaworkresultBLL
    {
        private NosaworkresultIService service = new NosaworkresultService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<NosaworkresultEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取分页列表
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
        public NosaworkresultEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }       
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            try
            {                
                var entity = service.GetEntity(keyValue);
                if (entity != null && !string.IsNullOrWhiteSpace(entity.TemplatePath))
                {
                    string filename = System.Web.Hosting.HostingEnvironment.MapPath(entity.TemplatePath);
                    if (System.IO.File.Exists(filename))
                    {
                        System.IO.File.Delete(filename);
                    }
                }
                service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void RemoveByWorkId(string keyValue)
        {
            try
            {
                var list = service.GetList(string.Format(" and workid='{0}'", keyValue));
                foreach (var entity in list)
                {
                    if (!string.IsNullOrWhiteSpace(entity.TemplatePath))
                    {
                        string filename = System.Web.Hosting.HostingEnvironment.MapPath(entity.TemplatePath);
                        if (System.IO.File.Exists(filename))
                        {
                            System.IO.File.Delete(filename);
                        }
                        service.RemoveForm(entity.ID);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, NosaworkresultEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
