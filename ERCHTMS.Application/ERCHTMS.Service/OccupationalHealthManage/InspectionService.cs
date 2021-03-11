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
using System;

namespace ERCHTMS.Service.OccupationalHealthManage
{
    /// <summary>
    /// �� ����ְҵ�������
    /// </summary>
    public class InspectionService : RepositoryFactory<InspectionEntity>, InspectionIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<InspectionEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }

        /// <summary>
        /// �洢���̷�ҳ
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageListByProc(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //��ѯ����
            if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            {
                string condition = queryParam["condition"].ToString();
                string keyord = queryParam["keyword"].ToString();
                DateTime st = condition.ToDate();
                DateTime ed = keyord.ToDate();
                //�Զ��ж����ڴ�С
                if (ed > st)
                {
                    //����ʱ�䷶Χ��ѯ
                    pagination.conditionJson += string.Format(" AND INSPECTIONTIME  >= TO_DATE('{0}','yyyy-mm-dd') AND INSPECTIONTIME<=TO_DATE('{1}','yyyy-mm-dd')", condition.Trim(), keyord.Trim());
                }
                else
                {
                    //����ʱ�䷶Χ��ѯ
                    pagination.conditionJson += string.Format(" AND INSPECTIONTIME  >= TO_DATE('{0}','yyyy-mm-dd') AND INSPECTIONTIME<=TO_DATE('{1}','yyyy-mm-dd')", condition.Trim(), keyord.Trim());
                }


            }
            else if (!queryParam["condition"].IsEmpty())//ֻ�п�ʼʱ��
            {
                string condition = queryParam["condition"].ToString();
                //����ʱ�䷶Χ��ѯ
                pagination.conditionJson += string.Format(" AND INSPECTIONTIME  >= TO_DATE('{0}','yyyy-mm-dd') ", condition.Trim());
            }
            else if (!queryParam["keyword"].IsEmpty())//ֻ�п�ʼʱ��
            {
                string keyord = queryParam["keyword"].ToString();
                //����ʱ�䷶Χ��ѯ
                pagination.conditionJson += string.Format(" AND  INSPECTIONTIME<=TO_DATE('{0}','yyyy-mm-dd') ", keyord.Trim());
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public InspectionEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, InspectionEntity entity)
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
