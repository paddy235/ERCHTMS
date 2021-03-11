using System.Collections.Generic;
using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.WebControl;
using ERCHTMS.Entity.PersonManage;
using ERCHTMS.IService.PersonManage;

namespace ERCHTMS.Service.PersonManage
{
    /// <summary>
    /// 人员行为安全管控预警数据操作
    /// </summary>
    public class EarlyWarningService : RepositoryFactory<EarlyWarningEntity>, IEarlyWarningService
    {
        /// <summary>
        /// 分页查询人员行为安全管控预警数据
        /// </summary>
        /// <param name="queryJson"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public IEnumerable<EarlyWarningEntity> GetPageList(string queryJson, Pagination pagination)
        {

            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();

            pagination.p_kid = "id";
            pagination.p_fields = " PICURL,WARNINGCONTENT,AREANAME,DUTYPERSON,DEPARTNAME,WARNINGTIME,DEVICENAME ";
            pagination.p_tablename = " BIS_HIKCAMERAWARNING ";

            pagination.conditionJson = " 1=1";
            //开始日期
            if (!queryParam["StartDate"].IsEmpty())
            {
                string startDate = queryParam["StartDate"].ToString();
                pagination.conditionJson += string.Format(" and to_char(warningtime,'yyyy-MM-dd')>='{0}'", startDate);
            }
            //结束日期
            if (!queryParam["EndDate"].IsEmpty())
            {
                string endDate = queryParam["EndDate"].ToString();
                pagination.conditionJson += string.Format(" and to_char(warningtime,'yyyy-MM-dd')<='{0}'", endDate);
            }
             //关键字
            if (!queryParam["KeyWord"].IsEmpty())
            {
                string keyWord = queryParam["KeyWord"].ToString();
                pagination.conditionJson += string.Format(" and (warningcontent like '%{0}%' or areaname like '%{0}%' or dutyperson like '%{0}%' )", keyWord);
            }

            return this.BaseRepository().FindListByProcPager(pagination, dataType);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public EarlyWarningEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }


        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public bool SaveForm(string keyValue, EarlyWarningEntity entity)
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
            return true;
        }
    }
}
