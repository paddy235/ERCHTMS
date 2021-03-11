using ERCHTMS.Entity.SafeReward;
using ERCHTMS.IService.SafeReward;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Service.BaseManage;

namespace ERCHTMS.Service.SafeReward
{
    /// <summary>
    /// �� ������ȫ������ϸ
    /// </summary>
    public class SaferewarddetailService : RepositoryFactory<SaferewarddetailEntity>, SaferewarddetailIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SaferewarddetailEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SaferewarddetailEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, SaferewarddetailEntity entity)
        {
            if (entity.RewardType == "��Ա")
            {
                UserEntity user = new UserService().GetEntity(entity.RewardNameId);
                entity.BelongDept = user.DepartmentId;
            }
            else if (entity.RewardType == "����")
            {
                entity.BelongDept = entity.RewardNameId;
            }
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
        /// ��ȡ������ϸ�б�
        /// </summary>
        /// <param name="rewardId">��ȫ����id</param>
        /// <returns></returns>
        public IEnumerable<SaferewarddetailEntity> GetListByRewardId(string rewardId)
        {
            return this.BaseRepository().IQueryable().ToList().Where(t => t.RewardId == rewardId);
        }

        /// <summary>
        /// ���ݰ�ȫ����IDɾ������
        /// </summary>
        /// <param name="rewardId">��ȫ����ID</param>
        public int Remove(string rewardId)
        {
            return this.BaseRepository().ExecuteBySql(string.Format("delete from BIS_SAFEREWARDDETAIL where rewardId='{0}'", rewardId));
        }
        #endregion
    }
}
