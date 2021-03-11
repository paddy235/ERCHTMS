using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.IService.HiddenTroubleManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace ERCHTMS.Service.HiddenTroubleManage
{
    /// <summary>
    /// �� ��������������Ϣ��
    /// </summary>
    public class HTApprovalService : RepositoryFactory<HTApprovalEntity>, HTApprovalIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<HTApprovalEntity> GetList(string queryJson)
        {
            var  list = this.BaseRepository().IQueryable().ToList();
            if (!string.IsNullOrEmpty(queryJson))
            {
                list = list.Where(p => p.HIDCODE == queryJson).OrderByDescending(p => p.AUTOID).ToList();
            }
            return list;
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public HTApprovalEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }


        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="hidCode"></param>
        /// <returns></returns>
        public HTApprovalEntity GetEntityByHidCode(string hidCode)
        {
            string sql = string.Format(@"select * from  bis_htapproval where hidcode ='{0}' order by autoid desc", hidCode);
            return this.BaseRepository().FindList(sql).FirstOrDefault();
        }
        
        /// <summary>
        /// ��ȡ��ʷ������������Ϣ
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HTApprovalEntity> GetHistoryList(string hidCode)
        {
            var list = this.BaseRepository().IQueryable().Where(p => p.HIDCODE == hidCode).OrderByDescending(p => p.AUTOID).ToList();
            return list;
        }


        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="hidCode"></param>
        /// <returns></returns>
        public DataTable  GetDataTableByHidCode(string hidCode) 
        {
            string sql = string.Format(@"select b.* from  bis_htapproval  a 
                                        left join base_user b on a.approvalperson = b.userid
                                        where a.hidcode ='{0}' order by a.autoid desc", hidCode);
            return this.BaseRepository().FindTable(sql);
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
            string sql = string.Format(@" delete bis_htapproval where hidcode ='{0}' ", hidcode);
            this.BaseRepository().ExecuteBySql(sql);
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, HTApprovalEntity entity)
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
