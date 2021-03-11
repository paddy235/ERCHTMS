using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSFramework.Data.Repository;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using System.Data;

namespace ERCHTMS.Service.OutsourcingProject
{
    public class HistoryAuditService : RepositoryFactory<HistoryAudit>, HistoryAuditIService
    {
        public HistoryAudit GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        public DataTable GetHisAuditRecList(string keyValue)
        {
            string sql = string.Format(@"select auditpeople,auditresult,audittime,auditopinion,auditdept from EPG_HISTORYAUDIT t where t.aptitudeid='{0}' order by audittime asc", keyValue);
            return new RepositoryFactory().BaseRepository().FindTable(sql);
        }
        public DataTable GetAuditRecList(string keyValue)
        {
            string sql = string.Format(@"select auditpeople,auditresult,audittime,auditopinion,auditdept,createtime,auditdeptid,aptitudeid,auditsignimg from epg_aptitudeinvestigateaudit t where t.aptitudeid='{0}' order by createtime asc", keyValue);
            return new RepositoryFactory().BaseRepository().FindTable(sql);
        }
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }

        public void SaveForm(string keyValue, HistoryAudit entity)
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
    }
}
