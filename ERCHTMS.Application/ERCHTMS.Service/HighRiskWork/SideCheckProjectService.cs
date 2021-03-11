using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.HighRiskWork
{
    /// <summary>
    /// �� �����ල��������Ŀ
    /// </summary>
    public class SideCheckProjectService : RepositoryFactory<SideCheckProjectEntity>, SideCheckProjectIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SideCheckProjectEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SideCheckProjectEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// ��������Ŀ��Ϣ
        /// </summary>
        /// <param name="approveid"></param>
        /// <returns></returns>
        public IEnumerable<SideCheckProjectEntity> GetBigCheckInfo()
        {
            var expression = LinqExtensions.True<SideCheckProjectEntity>();
            expression = expression.And(t => t.ParentId == "-1");
            return this.BaseRepository().IQueryable(expression).OrderBy(t => t.CheckNumber).ToList();
        }

        /// <summary>
        /// ���ݴ�������Ŀid��ȡС������Ŀ
        /// </summary>
        /// <param name="parentid"></param>
        /// <returns></returns>
        public IEnumerable<SideCheckProjectEntity> GetAllSmallCheckInfo(string parentid)
        {
            var expression = LinqExtensions.True<SideCheckProjectEntity>();
            expression = expression.And(t => t.ParentId != "-1");
            if (!string.IsNullOrEmpty(parentid))
            {
                expression = expression.And(t => t.ParentId == parentid);
            }
            return this.BaseRepository().IQueryable(expression).OrderBy(t => t.CheckNumber).ToList();
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
        public void SaveForm(string keyValue, SideCheckProjectEntity entity)
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
