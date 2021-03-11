using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data.Common;
using BSFramework.Data;
using System.Text;
using System;

namespace ERCHTMS.Service.HighRiskWork
{
    /// <summary>
    /// �� ������վ�ල��ҵ��Ϣ
    /// </summary>
    public class SuperviseWorkInfoService : RepositoryFactory<SuperviseWorkInfoEntity>, SuperviseWorkInfoIService
    {
        #region ��ȡ����
        /// <summary>
        /// ���ݷ�������id��ȡ��ҵ��Ϣ
        /// </summary>
        /// <param name="taskshareid"></param>
        /// <returns></returns>
        public IEnumerable<SuperviseWorkInfoEntity> GetList(string strwhere)
        {
            return this.BaseRepository().FindList(string.Format("select * from bis_superviseworkinfo where 1=1 " + strwhere)).ToList();
        }


        /// <summary>
        /// ���ݷ�������id�Ͱ���id��ȡ��ҵ��Ϣ
        /// </summary>
        /// <param name="taskshareid"></param>
        /// <returns></returns>
        public IEnumerable<SuperviseWorkInfoEntity> GetList(string taskshareid,string teamid)
        {
            string sql = @" select * from bis_superviseworkinfo where id in(select distinct(a.wrokid) from  bis_teamswork a where a.teamtaskid in(select id from bis_teamsinfo t where t.taskshareid=@taskshareid  and t.teamid=@teamid))";
            var parameter = new List<DbParameter>();
            parameter.Add(DbParameters.CreateDbParameter("@taskshareid", taskshareid));
            parameter.Add(DbParameters.CreateDbParameter("@teamid", teamid));
            return this.BaseRepository().FindList(sql, parameter.ToArray()).ToList();
        }

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SuperviseWorkInfoEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, SuperviseWorkInfoEntity entity)
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
        ///���ݷ���idɾ����ҵ��Ϣ
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveWorkByTaskShareId(string keyValue)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            try
            {
                db.Delete<SuperviseWorkInfoEntity>(t => t.TaskShareId.Equals(keyValue));
                db.Commit();
            }
            catch (Exception)
            {
                db.Rollback();
                throw;
            }
        }
        #endregion
    }
}
