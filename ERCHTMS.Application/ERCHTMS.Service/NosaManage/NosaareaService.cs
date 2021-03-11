using ERCHTMS.Entity.NosaManage;
using ERCHTMS.IService.NosaManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Code;

namespace ERCHTMS.Service.NosaManage
{
    /// <summary>
    /// 描 述：Nosa区域表
    /// </summary>
    public class NosaareaService : RepositoryFactory<NosaareaEntity>, NosaareaIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<NosaareaEntity> GetList(string queryJson)
        {
            var sql = string.Format("select * from HRS_NOSAAREA where 1=1 and state!=1 {0}", queryJson);
            return this.BaseRepository().FindList(sql);
        }
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var queryParam = queryJson.ToJObject();
            if (pagination.p_fields.IsEmpty())
            {
                pagination.p_fields = @"createdate,createuserid,createuserdeptcode,createuserorgcode,modifydate,modifyuserid,no,name,dutyuserid,dutyusername,arearange,dutydepartid,dutydepartname";
            }
            pagination.p_kid = "id";
            pagination.p_tablename = @"hrs_nosaarea";
            pagination.conditionJson = string.Format(" createuserorgcode='{0}' and state!=1", user.OrganizeCode);
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

            if (!queryParam["DutyName"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and dutyusername like '%{0}%'", queryParam["DutyName"].ToString());
            }
            if (!queryParam["DutyDept"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and dutydepartname like '%{0}%'", queryParam["DutyDept"].ToString());
            }
            //负责人id
            if (!queryParam["dutyuserid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and dutyuserid = '{0}'", queryParam["dutyuserid"].ToString());
            }
            var dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);

            return dt;
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public NosaareaEntity GetEntity(string keyValue)
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
            var resp = this.BaseRepository();
            var entity = resp.FindEntity(keyValue);
            if (entity != null)
            {
                entity.State = 1;
                resp.Update(entity);
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, NosaareaEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                var old = GetEntity(keyValue);
                if (old == null)
                {
                    entity.Create();
                    entity.ID = keyValue;
                    this.BaseRepository().Insert(entity);
                }
                else
                {
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
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
