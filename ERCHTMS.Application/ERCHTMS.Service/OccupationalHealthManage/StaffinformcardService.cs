using ERCHTMS.Entity.OccupationalHealthManage;
using ERCHTMS.IService.OccupationalHealthManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.OccupationalHealthManage
{
    /// <summary>
    /// 描 述：职业病危害告知卡
    /// </summary>
    public class StaffinformcardService : RepositoryFactory<StaffinformcardEntity>, StaffinformcardIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<StaffinformcardEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }

        /// <summary>
        /// 存储过程分页
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageListByProc(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //查询条件
            if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            {
                string condition = queryParam["condition"].ToString();
                string keyord = queryParam["keyword"].ToString();

                switch (condition)
                {
                    case "name":          //姓名
                        pagination.conditionJson += string.Format(" and INFORMCARDNAME  like '%{0}%'", keyord.Trim());
                        break;
                    default:
                        break;
                }

            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// 存储过程分页
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetTable(string queryJson,string where)
        {
            string sql = "select row_number() over(ORDER BY SETTINGTIME) as IDNUM,ID,INFORMCARDNAME,INFORMCARDVALUE,INFORMACARDPOSITION,to_char(SETTINGTIME,'yyyy-mm-dd') as SETTINGTIME,FileId,FileName,filepath from V_STAFFINFORMCARD where 1=1 ";
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //查询条件
            if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            {
                string condition = queryParam["condition"].ToString();
                string keyord = queryParam["keyword"].ToString();

                switch (condition)
                {
                    case "name":          //姓名
                        sql += string.Format(" and INFORMCARDNAME  like '%{0}%'", keyord.Trim());
                        break;
                    default:
                        break;
                }

            }
            sql += where;

            sql += " order by SETTINGTIME ";
            return this.BaseRepository().FindTable(sql);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public StaffinformcardEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, StaffinformcardEntity entity)
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
