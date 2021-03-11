using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.Data;
using System;
using ERCHTMS.Code;

namespace ERCHTMS.Service.HighRiskWork
{
    /// <summary>
    /// �� ������վ�ල��Ա(�߷�����ҵ)
    /// </summary>
    public class SidePersonService : RepositoryFactory<SidePersonEntity>, SidePersonIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<SidePersonEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return this.BaseRepository().FindList(pagination);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SidePersonEntity> GetList(string queryJson)
        {
            //return this.BaseRepository().IQueryable().ToList();
            return this.BaseRepository().FindList(" select * from BIS_SIDEPERSON where 1=1 " + queryJson).ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SidePersonEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageDataTable(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //��ѯ����
            if (!queryParam["keyword"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and SideUserName like '%{0}%'", queryParam["keyword"].ToString());
            }
            //��ѯ������
            if (!queryParam["deptcode"].IsEmpty() && !queryParam["deptid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and sideuserdeptid in(select departmentid from base_department  where encode like '{0}%' union select b.departmentid from epg_outsouringengineer  a left join base_department b on a.outprojectid=b.departmentid  where  a.engineerletdeptid='{1}')", queryParam["deptcode"].ToString(), queryParam["deptid"].ToString());
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
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
        public void SaveForm(string keyValue, SidePersonEntity entity)
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


        #region ��֤����
        /// <summary>
        /// ��վ�ල��Ա�����ظ�(��ǰ����)
        /// </summary>
        /// <returns></returns>
        public bool ExistSideUser(string userid)
        {
            string ownorgcode = OperatorProvider.Provider.Current().OrganizeCode;
            var expression = LinqExtensions.True<SidePersonEntity>();
            expression = expression.And(t => t.SideUserId == userid).And(t => t.CreateUserOrgCode == ownorgcode);
            return this.BaseRepository().IQueryable(expression).Count() == 0 ? true : false;
        }
        #endregion
    }
}
