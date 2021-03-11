using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using System.Data;
using System;

namespace ERCHTMS.Service.HighRiskWork
{
    /// <summary>
    /// �� �������ص�װ��ҵ������Ա��
    /// </summary>
    public class LifthoistpersonService : RepositoryFactory<LifthoistpersonEntity>, LifthoistpersonIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<LifthoistpersonEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }


        /// <summary>
        /// ��ȡ���ص�װ�����Ա��Ϣ
        /// </summary>
        /// <param name="workid"></param>
        /// <returns></returns>
        public IEnumerable<LifthoistpersonEntity> GetRelateList(string workid)
        {
            return this.BaseRepository().IQueryable(t => t.RecId == workid).ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public LifthoistpersonEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                if (queryJson.Length > 0)
                {
                    var queryParam = queryJson.ToJObject();
                }
                return this.BaseRepository().FindTableByProcPager(pagination, DbHelper.DbType);
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }


        /// <summary>
        /// ֤���Ų����ظ�
        /// </summary>
        /// <param name="CertificateNum">֤����</param>
        /// <param name="keyValue">����</param>
        /// <returns></returns>
        public bool ExistCertificateNum(string CertificateNum, string keyValue)
        {
            
            if (string.IsNullOrEmpty(CertificateNum))
            {
                return true;
            }
            string sql = string.Format("select a.id from BIS_LIFTHOISTJOB a left join BIS_LIFTHOISTPERSON b on a.id=b.recid where b.certificatenum='{0}'", CertificateNum);
            if (!string.IsNullOrEmpty(keyValue))
            {
                sql += string.Format(" and b.id !='{0}'", keyValue);
            }
            return this.BaseRepository().FindTable(sql).Rows.Count == 0 ? true : false;
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
        /// ɾ�����ص�װ������Ա��Ϣ
        /// </summary>
        /// <param name="WorkId"></param>
        public void RemoveFormByWorkId(string WorkId)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            try
            {
                db.Delete<LifthoistpersonEntity>(t => t.RecId.Equals(WorkId));
                db.Commit();
            }
            catch (Exception ex)
            {
                db.Rollback();
                throw;
            }
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, LifthoistpersonEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                LifthoistpersonEntity se = this.BaseRepository().FindEntity(keyValue);
                if (se == null)
                {
                    entity.Id = keyValue;
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
