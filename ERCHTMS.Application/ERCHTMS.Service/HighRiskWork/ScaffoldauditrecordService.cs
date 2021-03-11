using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System;
using System.Data.Common;
using BSFramework.Data;

namespace ERCHTMS.Service.HighRiskWork
{
    /// <summary>
    /// �� ����1.���ּ���˼�¼��
    /// </summary>
    public class ScaffoldauditrecordService : RepositoryFactory<ScaffoldauditrecordEntity>, ScaffoldauditrecordIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="scaffoldid">��ѯ����</param>
        /// <returns>�����б�</returns>
        public List<ScaffoldauditrecordEntity> GetList(string scaffoldid)
        {
            Expression<Func<ScaffoldauditrecordEntity, bool>> condition = e => e.ScaffoldId == scaffoldid;
            return this.BaseRepository().IQueryable(condition).OrderByDescending(t => t.AuditDate).ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public ScaffoldauditrecordEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }


        public List<ScaffoldauditrecordEntity> GetApplyAuditList(string keyValue, int AuditType)
        {
            string sql = string.Format(@"  select t.auditdate,t.auditdeptid,
                            t.auditdeptcode,t.auditdeptname,t.audituserid,
                            t.auditusername,t.auditremark,t.auditstate,t.auditsignimg
                            from bis_scaffoldauditrecord t where t.scaffoldid='{0}' and t.AuditType={1}", keyValue, AuditType);
            return this.BaseRepository().FindList(sql).OrderBy(t => t.AuditDate).ToList();
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="scaffoldid">���ּ���ϢID</param>
        /// <param name="departname">������</param>
        /// <param name="rolename">��ɫ��</param>
        /// <returns></returns>
        public ScaffoldauditrecordEntity GetEntity(string scaffoldid, string departname, string rolename)
        {
            string sql = @"
                select a.* from bis_scaffoldauditrecord a,base_user b
                where a.auditdeptname = @departname
                and scaffoldid = @scaffoldid";
            var parameter = new List<DbParameter>();
            parameter.Add(DbParameters.CreateDbParameter("@departname", departname));
            parameter.Add(DbParameters.CreateDbParameter("@scaffoldid", scaffoldid));
            if (!string.IsNullOrEmpty(rolename))
            {
                sql += "  and a.audituserid = b.userid and instr(b.roleid,(select roleid from base_role where fullname = @rolename and category = 1))>0 ";
                parameter.Add(DbParameters.CreateDbParameter("@rolename", rolename));
            }

            List<ScaffoldauditrecordEntity> list = this.BaseRepository().FindList(sql, parameter.ToArray()).OrderBy(x => x.AuditDate).ToList();
            if (list != null && list.Count >= 1)
            {
                return list[0];
            }
            return null;
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="scaffoldid">���ּ���ϢID</param>
        /// <param name="departcode">����</param>
        /// <returns></returns>
        public IEnumerable<ScaffoldauditrecordEntity> GetEntitys(string scaffoldid, string departcode)
        {
            string sql = @"
                select a.* from bis_scaffoldauditrecord a  where a.auditdeptcode = @departcode  and scaffoldid = @scaffoldid";
            var parameter = new List<DbParameter>();
            parameter.Add(DbParameters.CreateDbParameter("@deppartcode", departcode));
            parameter.Add(DbParameters.CreateDbParameter("@scaffoldid", scaffoldid));

            return this.BaseRepository().FindList(sql, parameter.ToArray()).OrderBy(x => x.AuditDate);
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
        public void SaveForm(string keyValue, ScaffoldauditrecordEntity entity)
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
