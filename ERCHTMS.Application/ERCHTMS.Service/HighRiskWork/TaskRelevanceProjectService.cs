using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Util.Extension;
using System.Data;
using BSFramework.Data;

namespace ERCHTMS.Service.HighRiskWork
{
    /// <summary>
    /// �� �����Ѽ��ļ����Ŀ
    /// </summary>
    public class TaskRelevanceProjectService : RepositoryFactory<TaskRelevanceProjectEntity>, TaskRelevanceProjectIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<TaskRelevanceProjectEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public TaskRelevanceProjectEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// ���ݼල�����ȡ�Ѽ����Ŀ
        /// </summary>
        /// <param name="approveid"></param>
        /// <returns></returns>
        public IEnumerable<TaskRelevanceProjectEntity> GetEndCheckInfo(string superviseid)
        {
            var expression = LinqExtensions.True<TaskRelevanceProjectEntity>();
            if (!string.IsNullOrEmpty(superviseid))
            {
                expression = expression.And(t => t.SuperviseId == superviseid);
            }
            return this.BaseRepository().IQueryable(expression).ToList();
        }

        /// <summary>
        /// ���ݼ����Ŀid�ͼල����id��ȡ��Ϣ
        /// </summary>
        /// <returns></returns>
        public TaskRelevanceProjectEntity GetCheckResultInfo(string checkprojectid, string superviseid)
        {
            var expression = LinqExtensions.True<TaskRelevanceProjectEntity>();
            expression = expression.And(t => t.SuperviseId == superviseid).And(t => t.CheckProjectId == checkprojectid);
            return this.BaseRepository().IQueryable(expression).ToList().FirstOrDefault();
        }


        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageDataTable(Pagination pagination)
        {
            DatabaseType dataType = DbHelper.DbType;
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// ���ݼලid��ȡ������Υ����Ϣ
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetTaskHiddenInfo(string superviseid)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("hiddescribe");
            dt.Columns.Add("changemeasure");
            dt.Columns.Add("lllegaldescribe");
            dt.Columns.Add("reformmeasure");
            string strsql = string.Format("select hiddescribe,changemeasure from v_hiddenbasedata where relevanceId='{0}'", superviseid);
            DataTable dtstr = this.BaseRepository().FindTable(strsql);
            string breaksql = string.Format("select lllegaldescribe,reformmeasure from v_lllegalallbaseinfo where reseverone='{0}'", superviseid);
            DataTable dtbreak = this.BaseRepository().FindTable(breaksql);
            var hiddescribe = "";
            var changemeasure = "";
            var lllegaldescribe = "";
            var reformmeasure = "";
            int count = 0;
            for (int i = 0; i < dtstr.Rows.Count; i++)
            {
                hiddescribe = hiddescribe + ((i + 1) + ":" + dtstr.Rows[i]["hiddescribe"]);
                changemeasure = changemeasure + ((i + 1) + ":" + dtstr.Rows[i]["changemeasure"]);

                count++;
            }
            for (int k = 0; k < dtbreak.Rows.Count; k++)
            {
                lllegaldescribe = lllegaldescribe + ((count + k + 1) + ":" + dtbreak.Rows[k]["lllegaldescribe"]);
                reformmeasure = reformmeasure + ((count + k + 1) + ":" + dtbreak.Rows[k]["reformmeasure"]);
            }
            DataRow row = dt.NewRow();
            row["hiddescribe"] = hiddescribe;
            row["changemeasure"] = changemeasure;
            row["lllegaldescribe"] = lllegaldescribe;
            row["reformmeasure"] = reformmeasure;
            dt.Rows.Add(row);
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
        public void SaveForm(string keyValue, TaskRelevanceProjectEntity entity)
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
