using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Util;
using BSFramework.Data;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.OutsourcingProject
{
    /// <summary>
    /// �� �������������Ա֤����
    /// </summary>
    public class CertificateinspectorsService : RepositoryFactory<CertificateinspectorsEntity>, CertificateinspectorsIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<CertificateinspectorsEntity> GetList(string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            //��ѯ����
            if (!queryParam["UserId"].IsEmpty())
            {
                return this.BaseRepository().IQueryable().ToList().Where(x => x.USERID == queryParam["UserId"].ToString());
            }
            return null;
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public CertificateinspectorsEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, CertificateinspectorsEntity entity)
        {
            int count = 0;
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.ID = keyValue;
                CertificateinspectorsEntity cert = BaseRepository().FindEntity(keyValue);
                if (cert==null)
                {
                    entity.Create();  
                    count = this.BaseRepository().Insert(entity);
                }
                else
                {
                    entity.Modify(keyValue);
                    count = this.BaseRepository().Update(entity);
                }
              
            }
            else
            {
                entity.Create();
                count = this.BaseRepository().Insert(entity);
            }
            if (count > 0)
            {
                count = int.Parse(BaseRepository().FindObject(string.Format("select count(1) from EPG_CERTIFICATEINSPECTORS where userid='{0}' and (CREDENTIALSNAME='{1}' or certtype='{1}') ", entity.USERID, "������ҵ����֤")).ToString());
                if (count > 0)
                {
                    BaseRepository().ExecuteBySql(string.Format("update EPG_APTITUDEINVESTIGATEPEOPLE set isspecial='��' where id='{0}'", entity.USERID));
                }
                count = int.Parse(BaseRepository().FindObject(string.Format("select count(1) from EPG_CERTIFICATEINSPECTORS where userid='{0}' and (CREDENTIALSNAME='{1}' or certtype='{1}') ", entity.USERID, "�����豸��ҵ��Ա֤")).ToString());
                if (count > 0)
                {
                    BaseRepository().ExecuteBySql(string.Format("update EPG_APTITUDEINVESTIGATEPEOPLE set isspecialequ='��' where id='{0}'", entity.USERID));
                }
            }
        }
        #endregion
    }
}
