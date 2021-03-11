using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System;
using System.Data.Common;
using BSFramework.Data;
using ERCHTMS.Entity.HighRiskWork.ViewModel;

namespace ERCHTMS.Service.HighRiskWork
{
    /// <summary>
    /// �� ����1.���ּ�������Ŀ
    /// </summary>
    public class ScaffoldprojectService : RepositoryFactory<ScaffoldprojectEntity>, ScaffoldprojectIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="scaffoldid">��ѯ����</param>
        /// <returns>�����б�</returns>
        public List<ScaffoldprojectEntity> GetList(string scaffoldid)
        {
            string sql = @"select * from bis_scaffoldproject where scaffoldid = @scaffoldid";
            return this.BaseRepository().FindList(sql, new DbParameter[]{
                    DbParameters.CreateDbParameter("@scaffoldid",scaffoldid)
            }).ToList();
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<ScaffoldprojectEntity> GetListByCondition(string queryJson)
        {
            return this.BaseRepository().FindList(string.Format("select * from bis_scaffoldproject where 1=1 " + queryJson)).ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public ScaffoldprojectEntity GetEntity(string keyValue)
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
        public void RemoveForm(Expression<Func<ScaffoldprojectEntity, bool>> condition)
        {
            this.BaseRepository().Delete(condition);
        }

        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, ScaffoldprojectEntity entity)
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
