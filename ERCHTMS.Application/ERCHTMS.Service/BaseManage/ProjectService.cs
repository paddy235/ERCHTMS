using ERCHTMS.Entity.BaseManage;
using ERCHTMS.IService.BaseManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.Data;
using System.Linq.Expressions;
using System;

namespace ERCHTMS.Service.BaseManage
{
    /// <summary>
    /// �� �������������Ŀ��Ϣ
    /// </summary>
    public class ProjectService : RepositoryFactory<ProjectEntity>, IProjectService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<ProjectEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public ProjectEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// �б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public IEnumerable<ProjectEntity> GetPageList(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();

            //ѡ�������
            if (!queryParam["type"].IsEmpty())
            {
                string type = queryParam["type"].ToString();
                pagination.conditionJson += string.Format(" and t.ProjectStatus in ('{0}') and t.ProjectStatus is not null", type);
            }
            //�����ؼ���
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyword = queryParam["keyword"].ToString();
                pagination.conditionJson += string.Format(" and t.ProjectName like '%{0}%'", keyword);
            }
            IEnumerable<ProjectEntity> list = this.BaseRepository().FindListByProcPager(pagination, dataType);

            return list;

        }

        /// <summary>
        /// �б��ҳ
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetPageDataTable(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();

            //ѡ�������
            if (!queryParam["type"].IsEmpty())
            {
                string type = queryParam["type"].ToString();
                pagination.conditionJson += string.Format(" and t.ProjectStatus in ('{0}') and t.ProjectStatus is not null", type);
            }
            //�����ؼ���
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyword = queryParam["keyword"].ToString();
                pagination.conditionJson += string.Format(" and t.ProjectName like '%{0}%'", keyword);
            }
            DataTable dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            return dt;

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
        public void SaveForm(string keyValue, ProjectEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                if (entity.ProjectStartDate == null)
                {
                    entity.ProjectStartDate = Convert.ToDateTime("1900-01-01");
                }
                if (entity.ProjectEndDate == null)
                {
                    entity.ProjectEndDate = Convert.ToDateTime("1900-01-01");
                }
                this.BaseRepository().Update(entity);
            }
            else
            {
                entity.Create();
                entity.OrganizeCode = entity.ProjectDeptCode.Substring(0, 3);
                this.BaseRepository().Insert(entity);
            }
        }
        #endregion
    }
}
