using ERCHTMS.Entity.NosaManage;
using ERCHTMS.IService.NosaManage;
using ERCHTMS.Service.NosaManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.IService;
using ERCHTMS.Service;
using ERCHTMS.Code;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Entity;
using ERCHTMS.Business;
using ERCHTMS.Entity.SafetyLawManage;

namespace ERCHTMS.Busines.NosaManage
{
    /// <summary>
    /// 描 述：培训文件
    /// </summary>
    public class NosatrafilesBLL
    {
        private NosatrafilesIService service = new NosatrafilesService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<NosatrafilesEntity> GetList(string queryJson)
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
        public NosatrafilesEntity GetEntity(string keyValue)
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
                service.RemoveForm(keyValue);
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
        public void SaveForm(string keyValue, NosatrafilesEntity entity)
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

    /// <summary>
    /// 描述：培训文件
    /// </summary>
    public class NosatrafilesBsBLL:BaseBLL<NosatrafilesEntity>
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public override DataTable GetList(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var queryParam = queryJson.ToJObject();
            if (pagination.p_fields.IsEmpty())
            {
                pagination.p_fields = @"createdate,createuserid,createuserdeptcode,createuserorgcode,modifydate,modifyuserid,filename,refid,refname,pubuserid,pubusername,pubdepartid,pubdepartname,pubdate,viewtimes";
            }
            pagination.p_kid = "id";
            pagination.p_tablename = @"hrs_nosatrafiles";
            pagination.conditionJson = string.Format(" createuserorgcode='{0}'", user.OrganizeCode);
            //文件名称
            if (!queryParam["filename"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and filename like '%{0}%'", queryParam["filename"].ToString());
            }
            //引用id
            if (!queryParam["refid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and refid = '{0}'", queryParam["refid"].ToString());
            }

            return base.GetList(pagination, queryJson);
        }
    }       
}

namespace ERCHTMS.Business
{
    /// <summary>
    /// 基础业务类
    /// </summary>
    public class BaseBLL<T> where T : BSEntity, new()
    {
        protected BaseIService<T> service = new BaseService<T>();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public virtual IEnumerable<T> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public virtual DataTable GetList(Pagination pagination, string queryJson)
        {
            return service.GetList(pagination, queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public virtual T GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public virtual void RemoveForm(string keyValue)
        {
            try
            {                
                service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public virtual void RemoveForm(T entity)
        {
            try
            {
                service.RemoveForm(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public virtual void RemoveForm(List<T> entities)
        {
            try
            {
                service.RemoveForm(entities);
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
        public virtual void SaveForm(string keyValue, T entity)
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