using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System;
using ERCHTMS.Code;
using ERCHTMS.Entity.ComprehensiveManage;
using ERCHTMS.IService.ComprehensiveManage;

namespace ERCHTMS.Service.ComprehensiveManage
{
    /// <summary>
    /// 荣誉分享
    /// </summary>
    public class HonoursService : RepositoryFactory<HonoursEntity>, HonoursIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //时间范围
            if (!queryParam["sTime"].IsEmpty() || !queryParam["eTime"].IsEmpty())
            {
                string startTime = queryParam["sTime"].ToString();
                string endTime = queryParam["eTime"].ToString();
                if (queryParam["sTime"].IsEmpty())
                {
                    startTime = "1899-01-01";
                }
                if (queryParam["eTime"].IsEmpty())
                {
                    endTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
                pagination.conditionJson += string.Format(" and releasetime between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", startTime, endTime);
            }
            //查询条件
            if (!queryParam["txtSearch"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and title like '%{0}%'", queryParam["txtSearch"].ToString());
            }
            //部门
            if (!queryParam["DeptCode"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and CreateUserDeptCode like '{0}%'", queryParam["DeptCode"].ToString());
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<HonoursEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public HonoursEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, HonoursEntity entity)
        {
            entity.Id = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                HonoursEntity se = this.BaseRepository().FindEntity(keyValue);
                if (se == null)
                {
                    entity.Create();
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


        /// <summary>
        /// 获取荣誉展示
        /// </summary>
        /// <returns></returns>
        public DataTable GetTrends(string num)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string sql = string.Format(@" select * from (select id,e.title,to_char(releasetime,'yyyy-mm-dd') time,f.filepath from  hrs_Honours e  left join 
                    ( select recid,filepath,row_number() over(partition by recid order by createdate asc) as num from  base_fileinfo) f on e.id=f.recid and f.num = 1 where issend=0 and e.createuserorgcode='{0}' order by  e.createdate desc) where rownum<'{1}'", user.OrganizeCode, num);
            return BaseRepository().FindTable(sql);
        }
    }
}
