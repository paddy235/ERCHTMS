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
    public class HTAcceptInfoService : RepositoryFactory<HTAcceptInfoEntity>, HTAcceptInfoIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<HTAcceptInfoEntity> GetList(string queryJson)
        {
            var list = this.BaseRepository().IQueryable().ToList();
            if (!string.IsNullOrEmpty(queryJson))
            {
                list = list.Where(p => p.HIDCODE == queryJson).OrderByDescending(p => p.AUTOID).ToList();
            }
            return list;
        }

        /// <summary>
        /// ��ȡ��ʷ������������Ϣ
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HTAcceptInfoEntity> GetHistoryList(string hidCode)
        {
            var list = this.BaseRepository().IQueryable().Where(p => p.HIDCODE == hidCode).OrderByDescending(p => p.AUTOID).ToList();
            list = list.Where(p => p.ACCEPTSTATUS == "1" ||  p.ACCEPTSTATUS =="0").ToList();
            return list;
        }

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public HTAcceptInfoEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="hidCode"></param>
        /// <returns></returns>
        public HTAcceptInfoEntity GetEntityByHidCode(string hidCode)
        {
            string sql = string.Format(@"select * from bis_htacceptinfo where hidcode ='{0}' order by autoid desc", hidCode);
            return this.BaseRepository().FindList(sql).FirstOrDefault();
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
        public void SaveForm(string keyValue, HTAcceptInfoEntity entity)
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


        public void RemoveFormByCode(string hidcode)
        {
            string sql = string.Format(@" delete bis_htacceptinfo where hidcode ='{0}' ", hidcode);
            this.BaseRepository().ExecuteBySql(sql);
        }
    }
}
