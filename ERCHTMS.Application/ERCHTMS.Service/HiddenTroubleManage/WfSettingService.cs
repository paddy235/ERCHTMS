using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.IService.HiddenTroubleManage;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using ERCHTMS.Code;
using System.Data.Common;

namespace ERCHTMS.Service.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：流程转向配置实例表
    /// </summary>
    public class WfSettingService : RepositoryFactory<WfSettingEntity>, WfSettingIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<WfSettingEntity> GetList(string queryJson)
        {
            if (!string.IsNullOrEmpty(queryJson))
            {
                return this.BaseRepository().IQueryable().Where(p => p.INSTANCEID == queryJson).ToList();
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
        public WfSettingEntity GetEntity(string keyValue)
        {
            try
            {
                return this.BaseRepository().FindEntity(keyValue);
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }



        #region  流程配置实例信息
        /// <summary>
        /// 流程配置实例信息
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetWfSettingInfoPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            if (pagination.p_fields.IsEmpty())
            {
                pagination.p_fields = @"organizeid,organizename,createdate, instancename,instanceid, settingname,startflow,endflow,submittype,isautohandle,
                                        isupdateflow,wfflag,isexcutesql,serialnumber,scriptcontent,isexcutecursql,scriptcurcontent,flowcode";
            }
            pagination.p_kid = "id";
            pagination.conditionJson = " 1=1";
            var queryParam = queryJson.ToJObject();

            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.p_tablename = @" (
                                            select f.organizeid,f.organizename, a.createdate, f.instancename , a.id,a.instanceid, a.settingname,a.startflow,a.endflow,a.submittype,a.isautohandle,
                                            a.isupdateflow,a.wfflag,a.isexcutesql,a.serialnumber,a.scriptcontent ,a.isexcutecursql,a.scriptcurcontent ,a.flowcode,f.instancetype from bis_wfsetting  a
                                            left join bis_wfinstance f on a.instanceid = f.id  
                                        ) ";
            //流程配置ID
            if (!queryParam["instanceid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and  instanceid = '{0}' ", queryParam["instanceid"].ToString());
            }
            //机构ID
            if (!queryParam["organizeid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and  organizeid = '{0}' ", queryParam["organizeid"].ToString());
            }
            //提交类型ID
            if (!queryParam["submittype"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and  submittype = '{0}' ", queryParam["submittype"].ToString());
            }
            //是否更新流程状态ID
            if (!queryParam["isupdateflow"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and  isupdateflow = '{0}' ", queryParam["isupdateflow"].ToString());
            }
            //起始流程
            if (!queryParam["startflow"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and  startflow = '{0}' ", queryParam["startflow"].ToString());
            }
            //目标流程
            if (!queryParam["endflow"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and  endflow = '{0}' ", queryParam["endflow"].ToString());
            }
            //流程代码
            if (!queryParam["flowcode"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and  flowcode like '%{0}%' ", queryParam["flowcode"].ToString());
            }
            //流程类型
            if (!queryParam["instancetype"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and  instancetype = '{0}'  ", queryParam["instancetype"].ToString());
            }
            else
            {
                pagination.conditionJson += @" and  instancetype !='基础流程' ";
            }
            var dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);

            return dt;
        }
        #endregion



        #region 根据对象获取所有相关的适配条件内容
        /// <summary>
        /// 根据对象获取所有相关的适配条件内容
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="instanceId"></param>
        /// <param name="settingtype"></param>
        /// <param name="settingId"></param>
        /// <returns></returns>
        public DataTable GetWfSettingForInstance(WfControlObj entity, string settingtype, string settingId)
        {
            DataTable dt = new DataTable();

            List<DbParameter> list = new List<DbParameter>();

            string strWhere = string.Empty;

            string sql = string.Empty;

            //流程转向类型
            if (!string.IsNullOrEmpty(settingtype))
            {
                sql = string.Format(@" select a.* from (select a.*,b.id  conditionid, b.organizeid,b.explains,b.depttype,b.roletype,b.choosetype,b.rolerule,b.rolecode,nvl(b.settingtype,'{0}') settingtype,b.isexecsql,b.sqlcontent from bis_wfsetting  a
                                        left join  bis_wfcondition  b on  a.id = b.settingid) a  where a.instanceid  = @instanceid ", settingtype);
            }
            else
            {
                sql = @"select a.* from (select a.*,b.id  conditionid, b.organizeid,b.explains,b.depttype,b.roletype,b.choosetype,b.rolerule,b.rolecode,b.settingtype,b.isexecsql,b.sqlcontent from bis_wfsetting  a
                                        left join  bis_wfcondition  b on  a.id = b.settingid) a  where a.instanceid  = @instanceid";
            }
            list.Add(DbParameters.CreateDbParameter("@instanceid ", entity.instanceid));
            //起始流程状态
            if (!string.IsNullOrEmpty(entity.startflow))
            {
                strWhere += @" and a.startflow = @startflow";
                list.Add(DbParameters.CreateDbParameter("@startflow ", entity.startflow));
            }
            //目标流程状态
            if (!string.IsNullOrEmpty(entity.endflow))
            {
                strWhere += @" and a.endflow = @endflow";
                list.Add(DbParameters.CreateDbParameter("@endflow ", entity.endflow));
            }
            //提交形式
            if (!string.IsNullOrEmpty(entity.submittype))
            {
                strWhere += @" and a.submittype = @submittype";
                list.Add(DbParameters.CreateDbParameter("@submittype ", entity.submittype));
            }
            //具体的配置项
            if (!string.IsNullOrEmpty(settingId))
            {
                strWhere += @" and a.id = @settingId";
                list.Add(DbParameters.CreateDbParameter("@settingId ", settingId));
            }
            //流程转向类型
            if (!string.IsNullOrEmpty(settingtype))
            {
                strWhere += @" and a.settingtype = @settingtype";
                list.Add(DbParameters.CreateDbParameter("@settingtype ", settingtype));
            }
            //具体的配置项
            if (!string.IsNullOrEmpty(entity.istoend))
            {
                strWhere += @" and a.isendpoint = @isendpoint";
                list.Add(DbParameters.CreateDbParameter("@isendpoint ", entity.istoend));
            }
            sql += strWhere;
            DbParameter[] param = list.ToArray();
            dt = this.BaseRepository().FindTable(sql, param);

            return dt;
        }
        #endregion

        /// <summary>
        /// 获取通用的查询内容
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetGeneralQuery(string sql, DbParameter[] param)
        {
            var dt = this.BaseRepository().FindTable(sql, param);
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
        public void SaveForm(string keyValue, WfSettingEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
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