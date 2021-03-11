using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.IService.HiddenTroubleManage;
using BSFramework.Data.Repository;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util.WebControl;
using ERCHTMS.Code;

namespace ERCHTMS.Service.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：流程配置条件表
    /// </summary>
    public class WfConditionService : RepositoryFactory<WfConditionEntity>, WfConditionIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<WfConditionEntity> GetList(string queryJson)
        {
            if (!string.IsNullOrEmpty(queryJson))
            {
                return this.BaseRepository().IQueryable().Where(p=>p.SETTINGID ==queryJson).ToList();
            }
            else 
            {
                return this.BaseRepository().IQueryable().ToList();
            }

        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public WfConditionEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion

        #region  流程配置实例信息
        /// <summary>
        /// 流程配置实例信息
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetWfConditionInfoPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            if (pagination.p_fields.IsEmpty())
            {
                pagination.p_fields = @"settingid,organizeid,explains,depttype,roletype,settingtype,choosetype,rolerule,organizename,createdate,isexecsql,sqlcontent";
            }
            pagination.p_kid = "id";
            pagination.conditionJson = " 1=1";
            var queryParam = queryJson.ToJObject();
            //当前用户
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.p_tablename = @" (
                                         select a.id, a.settingid, a.explains,a.organizeid,a.depttype,a.createdate,
                                         a.roletype,a.settingtype,a.choosetype,a.rolerule ,a.organizename,a.isexecsql,a.sqlcontent from bis_wfcondition a 
                                        ) ";
            //级别ID
            if (!queryParam["settingid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and  settingid = '{0}' ", queryParam["settingid"].ToString());
            }
            //流程标记
            if (!queryParam["mode"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and  settingtype = '{0}' ", queryParam["mode"].ToString()=="start"?"起始流程":"目标流程");
            }
            var dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);

            return dt;
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, WfConditionEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                var model = this.BaseRepository().FindEntity(keyValue);
                if (null != model)
                {
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }
                else 
                {
                    entity.Create();
                    entity.ID = keyValue;
                    this.BaseRepository().Insert(entity);
                }
            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }
        #endregion
    }
}