using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.OutsourcingProject
{
    /// <summary>
    /// �� ������ȫ�������ݱ�
    /// </summary>
    public class SafestandarditemService : RepositoryFactory<SafestandarditemEntity>, SafestandarditemIService
    {
        #region ��ȡ����

        /// <summary>
        /// �б��ҳ
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //�ϼ��ڵ�ID
            if (!queryParam["parentId"].IsEmpty())
            {
                string parentId = queryParam["parentId"].ToString();
                pagination.conditionJson += string.Format(" and parentId = '{0}'", parentId);
            }

            //��׼����
            if (!queryParam["enCode"].IsEmpty())
            {
                string enCode = queryParam["enCode"].ToString();
                pagination.conditionJson += string.Format(" and STCODE like '{0}%'", enCode);
            }
            //��ѯ�ؼ���
            if (!queryParam["keyWord"].IsEmpty())
            {
                string keyWord = queryParam["keyWord"].ToString();
                pagination.conditionJson += string.Format(" and (content like '%{0}%' or require like '%{0}%')", keyWord.Trim());
            }
            //��������԰�ȫ����е�ѡ��
            if (!queryParam["stIds"].IsEmpty())
            {
                string ids = queryParam["stIds"].ToString().Replace("[", "").Replace("]", "").Replace("\r\n", "").Replace("\"", "'");
                pagination.conditionJson += string.Format(" and stid in({0})", System.Text.RegularExpressions.Regex.Replace(ids, @"\s", ""));
                return this.BaseRepository().FindTable(string.Concat("select ", pagination.p_kid, ",", pagination.p_fields, " from ", pagination.p_tablename, " where ", pagination.conditionJson, "order by ", pagination.sidx, " ", pagination.sord));
            }
            else
            {
                return this.BaseRepository().FindTableByProcPager(pagination, dataType);
            }

        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SafestandarditemEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SafestandarditemEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, SafestandarditemEntity entity)
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
