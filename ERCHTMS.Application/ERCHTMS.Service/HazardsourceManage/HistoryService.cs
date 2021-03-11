using ERCHTMS.Entity.HazardsourceManage;
using ERCHTMS.IService.HazardsourceManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using ERCHTMS.Service.CommonPermission;
using BSFramework.Data;
using System.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System;

namespace ERCHTMS.Service.HazardsourceManage
{
    /// <summary>
    /// �� ������ʷ��¼
    /// </summary>
    public class HistoryService : RepositoryFactory<HistoryEntity>, IHistoryService
    {
        #region ��ȡ����


        /// <summary>
        /// �û��б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;

            if (queryJson.Length > 0)
            {
                var queryParam = queryJson.ToJObject();
                #region ��ѯ����
                //��ѯ����
                if (!queryParam["TimeEnd"].IsEmpty())
                {
                    string TimeEnd = queryParam["TimeEnd"].ToString();
                    TimeEnd = DateTime.Parse(TimeEnd).AddDays(1).ToString();
                    pagination.conditionJson += string.Format(" and CreateDate <= (select  to_date('{0}', 'yyyy-MM-dd') from dual)", TimeEnd);

                }
                //��ѯ����
                if (!queryParam["TimeStart"].IsEmpty())
                {
                    string TimeStart = queryParam["TimeStart"].ToString();
                    pagination.conditionJson += string.Format(" and CreateDate >= (select  to_date('{0}', 'yyyy-MM-dd') from dual)", TimeStart);
                }
                //��ѯ����
                if (!queryParam["DangerSourceName"].IsEmpty())
                {
                    string DangerSourceName = queryParam["DangerSourceName"].ToString();
                    pagination.conditionJson += string.Format(" and DangerSourceName like '%{0}%'", DangerSourceName);
                }

                #endregion


                #region Ȩ���ж�
                if (!queryParam["code"].IsEmpty() && !queryParam["isOrg"].IsEmpty())
                {
                    pagination = PermissionByCurrent.GetPermissionByCurrent2(pagination, queryParam["code"].ToString(), queryParam["isOrg"].ToString());
                }
                #endregion
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }


        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<HistoryEntity> GetList(string queryJson)
        {
            return this.BaseRepository().FindList(" select * from hsd_history where 1=1 " + queryJson).ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public HistoryEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
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
        public void SaveForm(string keyValue, HistoryEntity entity)
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

        public void Save(HistoryEntity entity)
        {
            this.BaseRepository().Insert(entity);
        }
        #endregion
    }
}
