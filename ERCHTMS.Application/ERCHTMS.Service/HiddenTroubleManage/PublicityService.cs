using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.IService.HiddenTroubleManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using ERCHTMS.Code;
using BSFramework.Util.Extension;
using System;

namespace ERCHTMS.Service.HiddenTroubleManage
{
    /// <summary>
    /// �� ����
    /// </summary>
    public class PublicityService : RepositoryFactory<PublicityEntity>, PublicityIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;
            if (queryJson != null && queryJson != "")
            {
                var queryParam = queryJson.ToJObject();

                //��ѯ���� ��ȫ�������
                if (!queryParam["ExamineType"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and p.ExamineType='{0}'", queryParam["ExamineType"].ToString());
                }
                //��ѯ���� ���
                if (!queryParam["Year"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and p.Year='{0}'", queryParam["Year"].ToString());
                }
                //��ѯ���� չʾ���  1 ����  2 ����
                if (!queryParam["ShowType"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and p.ShowType='{0}'", queryParam["ShowType"].ToString());
                }
                //��ѯ���� ��Ա���
                if (!queryParam["StaffType"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and p.StaffType='{0}'", queryParam["StaffType"].ToString());
                }
                
                //ʱ�䷶Χ
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
                        endTime = "2099-01-01";
                        //endTime = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    endTime = (Convert.ToDateTime(endTime).AddDays(1)).ToString("yyyy-MM-dd");
                    pagination.conditionJson += string.Format(" and CreateDate between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", startTime, endTime);
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<PublicityEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public PublicityEntity GetEntity(string keyValue)
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
        /// ִ��sql
        /// </summary>
        /// <param name="sql">sql���</param>
        public int ExecuteBySql(string sql)
        {

            return this.BaseRepository().ExecuteBySql(sql);
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, PublicityEntity entity)
        {
            entity.Id = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                PublicityEntity fe = this.BaseRepository().FindEntity(keyValue);
                if (fe == null)
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
