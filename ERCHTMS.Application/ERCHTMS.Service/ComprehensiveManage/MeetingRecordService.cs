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
    /// �� ����֪ͨ����
    /// </summary>
    public class MeetingRecordService : RepositoryFactory<MeetingRecordEntity>, MeetingRecordIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<MeetingRecordEntity> GetList(string queryJson)
        {

            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            if (queryJson != null && queryJson != "")
            {
                var queryParam = queryJson.ToJObject();
                //ʱ�䷶Χ
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
                //����
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
                //���˷���
                if (!queryParam["UserId"].IsEmpty())
                {
                    //���˴������˽��գ��ѷ��ͣ�
                    pagination.conditionJson += string.Format(" and (CreateUserId = '{0}' or (IsSend=1 and IssuerUserIdList like '%{0}%'))", queryParam["UserId"].ToString());
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// ��ȡʵ�� ������ǰ��¼�û�д���Ѷ���Ա��Ϣ�У�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
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

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
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
