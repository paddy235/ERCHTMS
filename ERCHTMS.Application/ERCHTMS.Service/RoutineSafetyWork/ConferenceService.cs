using ERCHTMS.Entity.RoutineSafetyWork;
using ERCHTMS.IService.RoutineSafetyWork;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.RoutineSafetyWork
{
    /// <summary>
    /// 描 述：安全会议
    /// </summary>
    public class ConferenceService : RepositoryFactory<ConferenceEntity>, ConferenceIService
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
            //年度
            if (!queryParam["year"].IsEmpty())
            {
                if (queryParam["year"].ToString() != "全部")
                    pagination.conditionJson += string.Format(" and to_char(ConferenceTime,'yyyy')='{0}'", queryParam["year"].ToString());

            }
            //查询条件
            if (!queryParam["txtSearch"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and ConferenceName like '%{0}%'", queryParam["txtSearch"].ToString());
            }
            //if (!queryParam["code"].IsEmpty() && !queryParam["isOrg"].IsEmpty())
            //{
            //    string deptCode = queryParam["code"].ToString();
            //    if (queryParam["isOrg"].ToString() == "Organize")
            //    {
            //        pagination.conditionJson += string.Format(" and CREATEUSERORGCODE  like '{0}%'", deptCode);
            //    }

            //    else
            //    {
            //        pagination.conditionJson += string.Format(" and CREATEUSERDEPTCODE like '{0}%'", deptCode);
            //    }
            //}
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<ConferenceEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ConferenceEntity GetEntity(string keyValue)
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
            Repository<ConferenceUserEntity> rep = new Repository<ConferenceUserEntity>(DbFactory.Base());
            this.BaseRepository().Delete(keyValue);
            //删除子表记录
            rep.ExecuteBySql(string.Format("delete BIS_ConferenceUser where ConferenceID='{0}' ", keyValue));
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, ConferenceEntity entity)
        {
            bool b = true;
            entity.Id = keyValue;
            Repository<ConferenceUserEntity> rep = new Repository<ConferenceUserEntity>(DbFactory.Base());

            if (!string.IsNullOrEmpty(keyValue))
            {
                ConferenceEntity se = this.BaseRepository().FindEntity(keyValue);
                if (se != null)
                {
                    b = false;
                }
            }
            if (b)
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
            else
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
                rep.ExecuteBySql(string.Format("delete BIS_ConferenceUser where ConferenceID='{0}' ", keyValue));
            }
            //增加子表记录
            var arrId = entity.UserId.Split(',');
            var arrName = entity.UserName.Split(',');
            List<ConferenceUserEntity> list = new List<ConferenceUserEntity>();
            for (int i = 0; i < arrId.Length; i++)
            {
                ConferenceUserEntity newEntity = new ConferenceUserEntity();
                newEntity.Create();
                newEntity.UserID = arrId[i];
                newEntity.UserName = arrName[i];
                newEntity.ConferenceID = entity.Id;
                newEntity.Issign = "1";
                newEntity.ReviewState = "0";
                list.Add(newEntity);
                
            }
            rep.Insert(list);
        }
        #endregion
    }
}
