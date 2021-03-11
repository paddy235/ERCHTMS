using ERCHTMS.Entity.StandardSystem;
using ERCHTMS.IService.StandardSystem;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.StandardSystem
{
    /// <summary>
    /// �� ������ӦԪ�ر�
    /// </summary>
    public class ElementService : RepositoryFactory<ElementEntity>, ElementIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<ElementEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public ElementEntity GetEntity(string keyValue)
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
        /// �жϽڵ��������ӽڵ�����
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public bool IsHasChild(string parentId)
        {
            return this.BaseRepository().FindObject(string.Format("select count(1) from hrs_element where parentid='{0}'", parentId)).ToInt() > 0 ? true : false;
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, ElementEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                var node = this.BaseRepository().FindEntity(entity.PARENTID);
                string enCode = node == null ? "" : node.ENCODE;
                int count = BaseRepository().FindObject(string.Format("select count(1) from hrs_element where parentid='{0}'", entity.PARENTID)).ToInt();
                count++;
                if (count.ToString().Length < 2)
                {
                    enCode += "00" + count;
                }
                else if (count.ToString().Length >= 2 && count.ToString().Length < 3)
                {
                    enCode += "0" + count;
                }
                else
                {
                    enCode += count.ToString();
                }
                entity.ENCODE = enCode;
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }
        #endregion
    }
}
