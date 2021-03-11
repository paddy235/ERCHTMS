using System;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.IService.CarManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using ERCHTMS.Code;

namespace ERCHTMS.Service.CarManage
{
    /// <summary>
    /// �� ����Σ��Ʒ���������Ŀ��
    /// </summary>
    public class CarcheckitemdetailService : RepositoryFactory<CarcheckitemdetailEntity>, CarcheckitemdetailIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<CarcheckitemdetailEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable(it=>it.PID==queryJson).OrderBy(it=>it.Sort).ToList();
        }

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public CarcheckitemdetailEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion

        #region �ύ����

        /// <summary>
        /// �޸������б��ύ״̬
        /// </summary>
        /// <param name="detaillist"></param>
        public void Update(List<CarcheckitemdetailEntity> detaillist)
        {
            var res = DbFactory.Base().BeginTrans();
            try
            {
                Repository<HazardouscarEntity> inlogdb = new Repository<HazardouscarEntity>(DbFactory.Base());
                HazardouscarEntity haza= inlogdb.FindEntity(detaillist[0].PID);
                Operator user = OperatorProvider.Provider.Current();
                haza.HazardousProcess = 1;
                haza.ProcessingName = user.UserName;
                haza.ProcessingId = user.UserId;
                haza.ProcessingSign = user.SignImg;
                haza.Modify(detaillist[0].PID);
                for (int i = 0; i < detaillist.Count; i++)
                {
                    detaillist[i].Modify(detaillist[i].ID);
                }

                res.Update<HazardouscarEntity>(haza);
                res.Update<CarcheckitemdetailEntity>(detaillist);
                res.Commit();
            }
            catch (Exception e)
            {
                res.Rollback();
                throw;
            }
        }

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
        public void SaveForm(string keyValue, CarcheckitemdetailEntity entity)
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
