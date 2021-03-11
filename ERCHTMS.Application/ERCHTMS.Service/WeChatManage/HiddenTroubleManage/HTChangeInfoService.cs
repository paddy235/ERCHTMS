using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.IService.HiddenTroubleManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;

namespace ERCHTMS.Service.HiddenTroubleManage
{
    /// <summary>
    /// �� ��������������Ϣ��
    /// </summary>
    public class HTChangeInfoService : RepositoryFactory<HTChangeInfoEntity>, HTChangeInfoIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<HTChangeInfoEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }

        /// <summary>
        /// ��ȡ��ʷ������������Ϣ
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HTChangeInfoEntity> GetHistoryList(string hidCode)
        {
            var list = this.BaseRepository().IQueryable().Where(p => p.HIDCODE == hidCode).OrderByDescending(p => p.AUTOID).ToList();
            if (list.Count() > 0)
            {
                list.RemoveAt(0);  //�Ƴ���һ��
            }
            return list;
        }



        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public HTChangeInfoEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }


        public HTChangeInfoEntity GetEntityByCode(string keyValue)
        {
            return this.BaseRepository().IQueryable().Where(p => p.HIDCODE == keyValue).OrderByDescending(p => p.AUTOID).ToList().FirstOrDefault();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="hidCode"></param>
        /// <returns></returns>
        public HTChangeInfoEntity GetEntityByHidCode(string hidCode)  
        {
            string sql = string.Format(@"select * from bis_htchangeinfo where hidcode ='{0}' order by autoid desc",hidCode);
            return  this.BaseRepository().FindList(sql).FirstOrDefault();
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

        
        public void RemoveFormByCode(string hidcode)
        {
            string sql = string.Format(@" delete bis_htchangeinfo where hidcode ='{0}' ", hidcode);
            this.BaseRepository().ExecuteBySql(sql);
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, HTChangeInfoEntity entity)
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
