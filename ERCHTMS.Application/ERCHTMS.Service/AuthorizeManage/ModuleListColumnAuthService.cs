using ERCHTMS.Entity.AuthorizeManage;
using ERCHTMS.IService.AuthorizeManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.Data;
using ERCHTMS.Code;

namespace ERCHTMS.Service.AuthorizeManage
{
    /// <summary>
    /// 描 述：应用模块列表的列查看权限设置表
    /// </summary>
    public class ModuleListColumnAuthService : RepositoryFactory<ModuleListColumnAuthEntity>, ModuleListColumnAuthIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<ModuleListColumnAuthEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }


        /// <summary>
        /// 获取对应的列表内容(通过模块id)
        /// </summary>
        /// <param name="moduleid"></param>
        /// <param name="userid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ModuleListColumnAuthEntity GetEntity(string moduleid, string userid, int type)
        {
            ModuleListColumnAuthEntity entity = null;
            List<ModuleListColumnAuthEntity> list = this.BaseRepository().IQueryable().Where(p=>p.LISTTYPE==type).ToList();
            if (!string.IsNullOrEmpty(moduleid)) 
            {
                list = list.Where(p => p.MODULEID == moduleid).ToList();
                
            }
            if (!string.IsNullOrEmpty(userid)) 
            {
                list = list.Where(p => p.USERID == userid).ToList();
            }
            if (list.Count() > 0) { entity = list.FirstOrDefault(); }
            return entity;
        }


        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public DataTable GetListByType(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;

            if (pagination.p_fields.IsEmpty())
            {
                pagination.p_fields = @" moduleid,modulename,listcolumnname,listcolumnfields,defaultcolumnname,defaultcolumnfields,userid,username,listtype,remark";
            }

            pagination.p_kid = "id";

            pagination.conditionJson = " 1=1";

            var queryParam = queryJson.ToJObject();

            pagination.p_tablename = @" base_modulelistcolumnauth a";

            //当前用户
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            //创建用户所属部门
            pagination.conditionJson += string.Format(@" and  a.createuserdeptcode  = '{0}' ", user.DeptCode);

            //类型
            if (!queryParam["type"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and  a.listtype  = '{0}' ", queryParam["type"].ToString());
            }
            //模块id
            if (!queryParam["moduleid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and  a.moduleid  = '{0}' ", queryParam["moduleid"].ToString());
            }
            //模块名称
            if (!queryParam["modulename"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and  a.modulename  like '%{0}%' ", queryParam["modulename"].ToString());
            }
            //当前用户id
            if (!queryParam["curuserid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and  a.userid  = '{0}' ", queryParam["curuserid"].ToString());
            }

            var dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);

            return dt;
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ModuleListColumnAuthEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
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
        public void SaveForm(string keyValue, ModuleListColumnAuthEntity entity)
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