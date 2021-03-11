using ERCHTMS.Entity.RoutineSafetyWork;
using ERCHTMS.IService.RoutineSafetyWork;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.RoutineSafetyWork
{
    /// <summary>
    /// �� ������ȫ����
    /// </summary>
    public class ConferenceService : RepositoryFactory<ConferenceEntity>, ConferenceIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //���
            if (!queryParam["year"].IsEmpty())
            {
                if (queryParam["year"].ToString() != "ȫ��")
                    pagination.conditionJson += string.Format(" and to_char(ConferenceTime,'yyyy')='{0}'", queryParam["year"].ToString());

            }
            //��ѯ����
            if (!queryParam["txtSearch"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and ConferenceName like '%{0}%'", queryParam["txtSearch"].ToString());
            }
            //if (!queryParam["code"].IsEmpty() && !queryParam["isOrg"].IsEmpty())
            //{
            //    string deptCode = queryParam["code"].ToString();
            //    if (queryParam["isOrg"].ToString() == "Organize")
            //    {
            //        pagination.conditionJson += string.Format(" and CREATEUSERORGCODE  like '{0}%'", deptCode);
            //    }

            //    else
            //    {
            //        pagination.conditionJson += string.Format(" and CREATEUSERDEPTCODE like '{0}%'", deptCode);
            //    }
            //}
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<ConferenceEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public ConferenceEntity GetEntity(string keyValue)
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
            Repository<ConferenceUserEntity> rep = new Repository<ConferenceUserEntity>(DbFactory.Base());
            this.BaseRepository().Delete(keyValue);
            //ɾ���ӱ��¼
            rep.ExecuteBySql(string.Format("delete BIS_ConferenceUser where ConferenceID='{0}' ", keyValue));
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, ConferenceEntity entity)
        {
            bool b = true;
            entity.Id = keyValue;
            Repository<ConferenceUserEntity> rep = new Repository<ConferenceUserEntity>(DbFactory.Base());

            if (!string.IsNullOrEmpty(keyValue))
            {
                ConferenceEntity se = this.BaseRepository().FindEntity(keyValue);
                if (se != null)
                {
                    b = false;
                }
            }
            if (b)
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
            else
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
                rep.ExecuteBySql(string.Format("delete BIS_ConferenceUser where ConferenceID='{0}' ", keyValue));
            }
            //�����ӱ��¼
            var arrId = entity.UserId.Split(',');
            var arrName = entity.UserName.Split(',');
            List<ConferenceUserEntity> list = new List<ConferenceUserEntity>();
            for (int i = 0; i < arrId.Length; i++)
            {
                ConferenceUserEntity newEntity = new ConferenceUserEntity();
                newEntity.Create();
                newEntity.UserID = arrId[i];
                newEntity.UserName = arrName[i];
                newEntity.ConferenceID = entity.Id;
                newEntity.Issign = "1";
                newEntity.ReviewState = "0";
                list.Add(newEntity);
                
            }
            rep.Insert(list);
        }
        #endregion
    }
}
