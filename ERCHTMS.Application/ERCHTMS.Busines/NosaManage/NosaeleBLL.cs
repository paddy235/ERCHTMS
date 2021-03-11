using ERCHTMS.Entity.NosaManage;
using ERCHTMS.IService.NosaManage;
using ERCHTMS.Service.NosaManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Code;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Business;

namespace ERCHTMS.Busines.NosaManage
{
    /// <summary>
    /// 描 述：元素表
    /// </summary>
    public class NosaeleBLL
    {
        private NosaeleIService service = new NosaeleService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<NosaeleEntity> GetList(string queryJson)
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
            return service.GetList(pagination,queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public NosaeleEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, NosaeleEntity entity)
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
    /// 描 述：元素表
    /// </summary>
    public class NosaeleBsBLL:BaseBLL<NosaeleEntity>
    {
        public override DataTable GetList(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var queryParam = queryJson.ToJObject();
            if (pagination.p_fields.IsEmpty())
            {
                pagination.p_fields = @"createdate,createuserid,createuserdeptcode,createuserorgcode,modifydate,modifyuserid,no,name,dutyuserid,dutyusername,dutydepartid,dutydepartname";
            }
            pagination.p_kid = "id";
            pagination.p_tablename = @"hrs_nosaele";
            pagination.conditionJson = string.Format(" createuserorgcode='{0}'", user.OrganizeCode);
            //编号
            if (!queryParam["no"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and no like '%{0}%'", queryParam["no"].ToString());
            }
            //名称
            if (!queryParam["name"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and name like '%{0}%'", queryParam["name"].ToString());
            }
            //负责人id
            if (!queryParam["dutyuserid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and dutyuserid = '{0}'", queryParam["dutyuserid"].ToString());
            }

            return base.GetList(pagination, queryJson);
        }
    }
}
