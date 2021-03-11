using ERCHTMS.Entity.SystemManage;
using ERCHTMS.IService.SystemManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System;

namespace ERCHTMS.Service.SystemManage
{
    /// <summary>
    /// 描 述：消息推送记录表表
    /// </summary>
    public class MessagePushRecordService : RepositoryFactory<MessagePushRecordEntity>, MessagePushRecordIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<MessagePushRecordEntity> GetList(string queryJson)
        {


            string sql = @"select * from BIS_MESSAGEPUSHRECORD a  where 1=1 ";
            if (!string.IsNullOrEmpty(queryJson)) 
            {
                var queryParam = queryJson.ToJObject();

                //关联id
                if (!queryParam["relvanceid"].IsEmpty())
                {
                    sql += string.Format(" and  relvanceid='{0}' ",queryParam["relvanceid"].ToString());
                }
                //标记
                if (!queryParam["mark"].IsEmpty())
                {
                    sql += string.Format(" and  mark='{0}' ", queryParam["mark"].ToString());
                }
                //源日期
                if (!queryParam["sourcedate"].IsEmpty())
                {
                    DateTime sourcedate =  Convert.ToDateTime(queryParam["sourcedate"].ToString());
                    sql += string.Format(" and  sourcedate = to_date('{0}','yyyy-mm-dd hh24:mi:ss') ", sourcedate);
                }
                //推送代码
                if (!queryParam["pushcode"].IsEmpty())
                {
                    string pushcode = queryParam["pushcode"].ToString();
                    sql += string.Format(" and  pushcode = '{0}' ", pushcode);
                }
                //推送人
                if (!queryParam["pushaccount"].IsEmpty())
                {
                    string pushaccount = queryParam["pushaccount"].ToString();
                    sql += string.Format(" and  pushaccount = '{0}' ", pushaccount); 
                }
            }
            return this.BaseRepository().FindList(sql);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public MessagePushRecordEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, MessagePushRecordEntity entity)
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