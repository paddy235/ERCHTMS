using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Util.Extension;
using ERCHTMS.Code;
using System;

namespace ERCHTMS.Service.HighRiskWork
{
    /// <summary>
    /// �� ������Σ����ҵ���/������
    /// </summary>
    public class HighRiskCheckService : RepositoryFactory<HighRiskCheckEntity>, HighRiskCheckIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<HighRiskCheckEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public HighRiskCheckEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// ��������id��ȡ�����Ϣ[������û���]
        /// </summary>
        /// <param name="approveid"></param>
        /// <returns></returns>
        public IEnumerable<HighRiskCheckEntity> GetCheckListInfo(string approveid)
        {
            var expression = LinqExtensions.True<HighRiskCheckEntity>();
            if (!string.IsNullOrEmpty(approveid))
            {
                expression = expression.And(t => t.ApproveId == approveid).And(t => t.ApproveState != "0").And(t => t.ApproveStep == "1");
            }
            return this.BaseRepository().IQueryable(expression).OrderBy(t => t.ModifyDate).ToList();
        }

        /// <summary>
        /// ��������id��ȡ������Ϣ[������û���]
        /// </summary>
        /// <param name="approveid"></param>
        /// <returns></returns>
        public HighRiskCheckEntity GetApproveInfo(string approveid)
        {
            var expression = LinqExtensions.True<HighRiskCheckEntity>();
            if (!string.IsNullOrEmpty(approveid))
            {
                expression = expression.And(t => t.ApproveId == approveid).And(t => t.ApproveState != "0").And(t => t.ApproveStep == "2");
            }
            return this.BaseRepository().IQueryable(expression).OrderBy(t => t.ModifyDate).ToList().FirstOrDefault();
        }

        /// <summary>
        /// ��������id��ȡû��˵�����
        /// </summary>
        /// <param name="approveid"></param>
        /// <returns></returns>
        public int GetNoCheckNum(string approveid)
        {
            int num = 0;
            var expression = LinqExtensions.True<HighRiskCheckEntity>();
            if (!string.IsNullOrEmpty(approveid))
            {
                expression = expression.And(t => t.ApproveId == approveid).And(t => t.ApproveState == "0").And(t => t.ApproveStep == "1");
                num = this.BaseRepository().IQueryable(expression).ToList().Count();
            }
            return num;
        }

        /// <summary>
        /// ��������id�͵�ǰ��¼�˻�ȡ���(��)��¼
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="chapterid"></param>
        /// <returns></returns>
        public HighRiskCheckEntity GetNeedCheck(string approveid)
        {
            var user = OperatorProvider.Provider.Current();
            var expression = LinqExtensions.True<HighRiskCheckEntity>();
            if (!string.IsNullOrEmpty(approveid))
            {
                expression = expression.And(t => t.ApproveId == approveid).And(t => t.ApprovePerson == user.UserId);
            }
            return this.BaseRepository().IQueryable(expression).ToList().FirstOrDefault();
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
        ///���������idɾ������
        /// </summary>
        public int Remove(string workid)
        {
            return this.BaseRepository().ExecuteBySql(string.Format("delete from bis_highriskcheck  where  ApproveId='{0}'", workid));
        }

        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, HighRiskCheckEntity entity)
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
