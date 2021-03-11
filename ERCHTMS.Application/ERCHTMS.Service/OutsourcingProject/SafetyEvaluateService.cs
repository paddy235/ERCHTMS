using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;

namespace ERCHTMS.Service.OutsourcingProject
{
    /// <summary>
    /// �� ������ȫ����
    /// </summary>
    public class SafetyEvaluateService : RepositoryFactory<SafetyEvaluateEntity>, SafetyEvaluateIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public DataTable GetList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public DataTable GetEntity(string keyValue)
        {
            string sql = string.Format(@"select t.id,r.ID engid,r.ENGINEERNAME,
r.ENGINEERCODE,r.ENGINEERTYPE,r.ENGINEERAREA,r.ENGINEERLEVEL,r.ENGINEERLETDEPT,r.ENGINEERCONTENT,r.ENGINEERAREANAME as EngAreaName,
t.SiteManagementScore,t.QualityScore,t.ProjectProgressScore,t.FieldServiceScore,t.Evaluator,t.EvaluatorId,t.EvaluationTime,t.EvaluationScore
 from EPG_SafetyEvaluate t left join EPG_OutSouringEngineer r on t.projectid=r.id  where t.id='{0}'", keyValue);
            DataTable data = this.BaseRepository().FindTable(sql);
            return data;
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
        public void SaveForm(string keyValue, SafetyEvaluateEntity entity)
        {
            var res = DbFactory.Base().BeginTrans();
            try
            {
                entity.ID = keyValue;
                if (!string.IsNullOrEmpty(keyValue))
                {
                    SafetyEvaluateEntity se = this.BaseRepository().FindEntity(keyValue);
                    if (se == null)
                    {
                        entity.Create();
                        res.Insert<SafetyEvaluateEntity>(entity);
                    }
                    else
                    {
                        entity.Modify(keyValue);
                        res.Update<SafetyEvaluateEntity>(entity);
                    }
                    //Repository<OutsouringengineerEntity> ourEngineer = new Repository<OutsouringengineerEntity>(DbFactory.Base());
                    //OutsouringengineerEntity engineerEntity = ourEngineer.FindEntity(entity.PROJECTID);
                    //if (entity.ISSEND == "1")//�ύʱ���¹���״̬Ϊ'���깤'
                    //{
                    //    engineerEntity.ENGINEERSTATE = "003";
                    //    res.Update<OutsouringengineerEntity>(engineerEntity);
                    //}
                }
                else
                {
                    entity.Create();
                    res.Insert<SafetyEvaluateEntity>(entity);
                }
                
            }
            catch (System.Exception)
            {

                res.Rollback();
            }
            res.Commit();
        }
        #endregion


        public IEnumerable<SafetyEvaluateEntity> GetList()
        {
            return this.BaseRepository().IQueryable().ToList();
        }
    }
}
