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
    /// �� ����ְҵ����Ա��
    /// </summary>
    public class OccupatioalstaffService : RepositoryFactory<OccupatioalstaffEntity>, OccupatioalstaffIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<OccupatioalstaffEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public OccupatioalstaffEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
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

                switch (condition)
                {
                    case "name":          //����
                        pagination.conditionJson += string.Format(" and MECHANISMNAME  like '%{0}%'", keyord.Trim());
                        break;
                    default:
                        break;
                }

            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }


        


        /// <summary>
        /// ����������ѯ��������
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetTable(string queryJson,string where)
        {
            string Sql = "SELECT OCCID,MECHANISMNAME,TO_CHAR(INSPECTIONTIME,'yyyy-mm-dd hh24:mi:ss')as INSPECTIONTIME,INSPECTIONNUM,PATIENTNUM,ISANNEX,UNUSUALNUM FROM V_OCCUPATIOALSTAFF WHERE 1=1";
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //��ѯ����
            if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            {
                string condition = queryParam["condition"].ToString();
                string keyord = queryParam["keyword"].ToString();

                switch (condition)
                {
                    case "name":          //����
                        Sql += string.Format(" and MECHANISMNAME  like '%{0}%'", keyord.Trim());
                        break;
                    default:
                        break;
                }

            }

            Sql += where;

            Sql += " ORDER BY INSPECTIONTIME DESC";
            return this.BaseRepository().FindTable(Sql);
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
        public void SaveForm(string keyValue, OccupatioalstaffEntity entity)
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

        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="IsNew">�Ƿ�����</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(bool IsNew, OccupatioalstaffEntity entity)
        {
            if (!IsNew)
            {
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
