using ERCHTMS.Entity.SafetyWorkSupervise;
using ERCHTMS.IService.SafetyWorkSupervise;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;

namespace ERCHTMS.Service.SafetyWorkSupervise
{
    /// <summary>
    /// �� ������ȫ�ص㹤�����췴����Ϣ
    /// </summary>
    public class SuperviseconfirmationService : RepositoryFactory<SuperviseconfirmationEntity>, SuperviseconfirmationIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SuperviseconfirmationEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SuperviseconfirmationEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, SuperviseconfirmationEntity entity)
        {
            bool b = false;
            if (!string.IsNullOrWhiteSpace(keyValue))
            {
                var sl = BaseRepository().FindEntity(keyValue);
                if (sl != null)
                {
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }
                else
                {
                    b = true;
                }
            }
            else
            {
                b = true;
            }
            if (b)
            {
                bool flag = false;
                entity.Id = keyValue;
                entity.Create();
                if (entity.SuperviseResult == "0")
                {
                    flag = false;
                }
                else if (entity.SuperviseResult == "1")
                {
                    flag = true;
                    entity.Flag = "1";
                }
                this.BaseRepository().Insert(entity);
                Repository<SafetyworksuperviseEntity> announRes = new Repository<SafetyworksuperviseEntity>(DbFactory.Base());
                var sl = announRes.FindEntity(entity.SuperviseId);
                if (sl != null)
                {   //������������״̬
                    if (flag)
                    {
                        sl.FlowState = "1";
                    }
                    else{
                        sl.FlowState = "3";
                    }
                    announRes.Update(sl);
                }
                Repository<SafetyworkfeedbackEntity> feedback = new Repository<SafetyworkfeedbackEntity>(DbFactory.Base());
                var s2 = feedback.FindEntity(entity.FeedbackId);
                if (s2 != null)
                {   //���·���������״̬(��ǰ����ʷ����)
                    if (flag)
                    {
                        s2.Flag = "1";
                    }
                    feedback.Update(s2);
                }
            }
        }
        #endregion
    }
}
