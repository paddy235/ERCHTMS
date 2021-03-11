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
using System.Data.Common;
using ERCHTMS.Entity.BaseManage;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Entity.LllegalManage;

namespace ERCHTMS.Service.HighRiskWork
{
    /// <summary>
    /// �� �������������Ա
    /// </summary>
    public class StaffInfoService : RepositoryFactory<StaffInfoEntity>, StaffInfoIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<StaffInfoEntity> GetList(string queryJson)
        {
            var queryParam = JObject.Parse(queryJson);
            var parameter = new List<DbParameter>();
            string sql = "select * from bis_staffinfo where  taskshareid= @taskshareid";
            parameter.Add(DbParameters.CreateDbParameter("@taskshareid", queryParam["taskshareid"].ToString()));
            if (!queryParam["teamid"].IsEmpty())//����id
            {
                sql += " and pteamid= @teamid";
                parameter.Add(DbParameters.CreateDbParameter("@pteamid", queryParam["teamid"].ToString()));
            }
            if (!queryParam["tasklevel"].IsEmpty())
            {
                sql += " and tasklevel=@tasklevel";
                parameter.Add(DbParameters.CreateDbParameter("@tasklevel", queryParam["tasklevel"].ToString()));
            }
            if (!queryParam["staffid"].IsEmpty())
            {
                sql += " and staffid=@staffid";
                parameter.Add(DbParameters.CreateDbParameter("@staffid", queryParam["staffid"].ToString()));
            }
            sql += " order by createdate desc";
            return this.BaseRepository().FindList(sql, parameter.ToArray()).ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public StaffInfoEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// ��ȡ�ල�����б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public DataTable GetDataTable(Pagination page, string queryJson)
        {
            DatabaseType dataTye = DatabaseType.Oracle;
            #region ���
            page.p_kid = "a.id";
            page.p_fields = @"a.taskshareid,a.dataissubmit,a.supervisestate,a.pteamname,a.pteamid,a.pteamcode,a.taskusername,a.taskuserid,a.sumtimestr,a.createdate,a.pstarttime,a.pendtime,b.id as workid,b.workname,b.workinfotype,b.workinfotypeid,b.workdeptname,b.workdeptcode,b.workdeptid,b.workstarttime,b.workendtime,b.workareaname,b.workplace,b.handtype";
            page.p_tablename = @" bis_staffinfo a left join bis_superviseworkinfo b on a.workinfoid=b.id";
            page.conditionJson = "1=1";
            var queryParam = queryJson.ToJObject();
            //�������id
            if (!queryParam["taskshareid"].IsEmpty())
            {
                page.conditionJson += string.Format(" and a.taskshareid='{0}'", queryParam["taskshareid"].ToString());
            }
            //�ල��λid
            if (!queryParam["teamid"].IsEmpty())
            {
                page.conditionJson += string.Format(" and pteamid='{0}'", queryParam["teamid"].ToString());
            }
            //�ල����
            if (!queryParam["tasklevel"].IsEmpty())
            {
                page.conditionJson += string.Format(" and tasklevel='{0}'", queryParam["tasklevel"].ToString());
            }
            //�Ƿ��ύ
            if (!queryParam["dataissubmit"].IsEmpty())
            {
                page.conditionJson += string.Format(" and dataissubmit='{0}'", queryParam["dataissubmit"].ToString());
            }
            //��ѯ�¼�
            if (!queryParam["staffid"].IsEmpty())
            {
                page.conditionJson += string.Format(" and staffid='{0}'", queryParam["staffid"].ToString());
            }
            //�ල״̬
            if (!queryParam["supervisestate"].IsEmpty())
            {
                page.conditionJson += string.Format(" and supervisestate='{0}'", queryParam["supervisestate"].ToString());
            }
            //��ҵ��ʼʱ��
            if (!queryParam["workstarttime"].IsEmpty())
            {
                string from = queryParam["workstarttime"].ToString().Trim();
                page.conditionJson += string.Format(" and b.workstarttime>=to_date('{0}','yyyy-mm-dd')", from);
            }
            //��ҵ����ʱ��
            if (!queryParam["workendtime"].IsEmpty())
            {
                string to = Convert.ToDateTime(queryParam["workendtime"].ToString().Trim()).AddDays(1).ToString("yyyy-MM-dd");
                page.conditionJson += string.Format(" and b.workendtime<=to_date('{0}','yyyy-mm-dd')", to);
            }
            //��ҵ��λ
            if (!queryParam["workdeptid"].IsEmpty())
            {
                page.conditionJson += string.Format(" and b.workdeptid='{0}'", queryParam["workdeptid"].ToString());
            }
            //��վ�ලԱ
            if (!queryParam["taskuserid"].IsEmpty())
            {
                page.conditionJson += string.Format(" and  ','||taskuserid||',' like '%,{0},%'", queryParam["taskuserid"].ToString());
            }
            //��վ��ʼʱ��
            if (!queryParam["pstarttime"].IsEmpty())
            {
                string from = queryParam["pstarttime"].ToString().Trim();
                page.conditionJson += string.Format(" and a.pstarttime>=to_date('{0}','yyyy-mm-dd')", from);
            }
            //��վ����ʱ��
            if (!queryParam["pendtime"].IsEmpty())
            {
                string to = Convert.ToDateTime(queryParam["pendtime"].ToString().Trim()).AddDays(1).ToString("yyyy-MM-dd");
                page.conditionJson += string.Format(" and a.pendtime<=to_date('{0}','yyyy-mm-dd')", to);
            }
            #endregion
            var data = this.BaseRepository().FindTableByProcPager(page, dataTye);
            return data;
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            try
            {
                db.Delete<StaffInfoEntity>(keyValue);
                db.Delete<StaffInfoEntity>(t => t.StaffId.Equals(keyValue));
                db.Delete<HTBaseInfoEntity>(t => t.RELEVANCEID.Equals(keyValue));
                db.Delete<LllegalRegisterEntity>(t => t.RESEVERONE.Equals(keyValue));
                db.Commit();
            }
            catch (Exception)
            {
                db.Rollback();
                throw;
            }
            this.BaseRepository().Delete(keyValue);
        }

        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public string SaveForm(string keyValue, StaffInfoEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                var sf = BaseRepository().FindEntity(keyValue);
                if (sf == null)
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
            return entity.Id;
        }
        #endregion
    }
}
