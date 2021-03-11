using ERCHTMS.Entity.ComprehensiveManage;
using ERCHTMS.IService.ComprehensiveManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using ERCHTMS.Code;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.Data;
using System;

namespace ERCHTMS.Service.ComprehensiveManage
{
    /// <summary>
    /// 描 述：通知公告
    /// </summary>
    public class MeetingRecordService : RepositoryFactory<MeetingRecordEntity>, MeetingRecordIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<MeetingRecordEntity> GetList(string queryJson)
        {

            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            if (queryJson != null && queryJson != "")
            {
                var queryParam = queryJson.ToJObject();
                //时间范围
                if (!queryParam["StartTime"].IsEmpty() || !queryParam["EndTime"].IsEmpty())
                {
                    string startTime = queryParam["StartTime"].ToString();
                    string endTime = queryParam["EndTime"].ToString();
                    if (queryParam["StartTime"].IsEmpty())
                    {
                        startTime = "1899-01-01";
                    }
                    if (queryParam["EndTime"].IsEmpty())
                    {
                        endTime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                    }
                    endTime = (Convert.ToDateTime(endTime).AddDays(1)).ToString("yyyy-MM-dd");
                    pagination.conditionJson += string.Format(" and IssueTime between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", startTime, endTime);
                }
                //标题
                if (!queryParam["Name"].IsEmpty())
                {
                    string title = queryParam["Name"].ToString();
                    pagination.conditionJson += string.Format(" and Name like '%{0}%'", title);
                }
                if (!queryParam["pager"].IsEmpty())
                {
                    if (queryParam["pager"].ToString() == "true"|| queryParam["pager"].ToString() == "True")
                        pagination.conditionJson += string.Format(" and SettlePerson = '{0}'", queryParam["UserName"].ToString());
                }
                //本人发布
                if (!queryParam["UserId"].IsEmpty())
                {
                    //本人创建或本人接收（已发送）
                    pagination.conditionJson += string.Format(" and (CreateUserId = '{0}' or (IsSend=1 and IssuerUserIdList like '%{0}%'))", queryParam["UserId"].ToString());
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取实体 （将当前登录用户写入已读人员信息中）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public MeetingRecordEntity GetEntity(string keyValue)
        {
            MeetingRecordEntity entity = this.BaseRepository().FindEntity(keyValue);
            if (entity != null)
            {
                string userIdStr = entity.ReadUserIdList;
                bool isCz = false;
                if (userIdStr != null)
                {
                    if (userIdStr.Length > 0)
                    {
                        userIdStr += ",";
                        if (userIdStr.Contains(OperatorProvider.Provider.Current().UserId))
                        {
                            isCz = true;
                        }
                    }
                }
                if (!isCz)
                {
                    entity.ReadUserIdList = userIdStr + OperatorProvider.Provider.Current().UserId;
                    this.BaseRepository().Update(entity);
                }
                //string userNameStr = entity.ReadUserIdList;
                //if (userNameStr.Length > 0)
                //{
                //    userNameStr += ",";
                //}
                //entity.ReadUserNameList = userNameStr + OperatorProvider.Provider.Current().UserName;
                
            }
            return this.BaseRepository().FindEntity(keyValue); ;
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
        public void SaveForm(string keyValue, MeetingRecordEntity entity)
        {
            entity.Id = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                MeetingRecordEntity ae = this.BaseRepository().FindEntity(keyValue);
                if (ae == null)
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
    }
}
