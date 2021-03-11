using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data.Common;
using BSFramework.Data;
using System;
using System.Linq.Expressions;

namespace ERCHTMS.Service.HighRiskWork
{
    /// <summary>
    /// �� ����1.���ּܹ����ʽ��
    /// </summary>
    public class ScaffoldspecService : RepositoryFactory<ScaffoldspecEntity>, ScaffoldspecIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="scaffoldid">��ѯ����</param>
        /// <returns>�����б�</returns>
        public List<ScaffoldspecEntity> GetList(string scaffoldid)
        {
            return this.BaseRepository().FindList("select * from bis_scaffoldspec where ScaffoldId = @ScaffoldId", new DbParameter[]{
                    DbParameters.CreateDbParameter("@ScaffoldId",scaffoldid)
            }).ToList();
        }

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public ScaffoldspecEntity GetEntity(string keyValue)
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
        /// �����������ʽɾ��
        /// </summary>
        /// <param name="condition"></param>
        public void RemoveForm(Expression<Func<ScaffoldspecEntity, bool>> condition)
        {
            this.BaseRepository().Delete(condition);
        }

        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, ScaffoldspecEntity entity)
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
