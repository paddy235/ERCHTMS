using ERCHTMS.Entity.SafePunish;
using ERCHTMS.IService.SafePunish;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Service.BaseManage;

namespace ERCHTMS.Service.SafePunish
{
    /// <summary>
    /// �� ������ȫ������ϸ
    /// </summary>
    public class SafepunishdetailService : RepositoryFactory<SafepunishdetailEntity>, SafepunishdetailIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SafepunishdetailEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SafepunishdetailEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, SafepunishdetailEntity entity)
        {

            if (entity.PunishType == "��Ա")
            {
                UserEntity user = new UserService().GetEntity(entity.PunishNameId);
                if (user !=null)
                {
                    entity.BelongDept = user.DepartmentId;
                }
                
            }
            else if (entity.PunishType == "����")
            {
                entity.BelongDept = entity.PunishNameId;
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
        /// <param name="punishId">��ȫ����id</param>
        /// <param name="type">����</param>
        /// <returns></returns>
        public IEnumerable<SafepunishdetailEntity> GetListByPunishId(string punishId,string type)
        {
            return this.BaseRepository().IQueryable().ToList().Where(t => t.PunishId == punishId && t.Type == type);
        }

        /// <summary>
        /// ���ݰ�ȫ����IDɾ������
        /// </summary>
        /// <param name="punishId">��ȫ����ID</param>
        /// <param name="type">����</param>
        public int Remove(string punishId,string type)
        {
            return this.BaseRepository().ExecuteBySql(string.Format("delete from BIS_SAFEPUNISHDETAIL where punishId='{0}' and type='{1}'", punishId, type));
        }
        #endregion
    }
}
