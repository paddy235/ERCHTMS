using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Data;
using BSFramework.Util;
using BSFramework.Data;
using BSFramework.Util.Extension;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Code;
using ERCHTMS.Service.BaseManage;
using ERCHTMS.IService.BaseManage;
using System.Text;

namespace ERCHTMS.Service.OutsourcingProject
{
    /// <summary>
    /// �� ������ȫ���� ������Ϣ��
    /// </summary>
    public class SafetyassessmentpersonService : RepositoryFactory<SafetyassessmentpersonEntity>, SafetyassessmentpersonIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SafetyassessmentpersonEntity> GetList(string queryJson)
        {
            //return this.BaseRepository().IQueryable().ToList();

            var strSql = new StringBuilder();
            strSql.AppendFormat(@"select id, createuserid, createuserdeptcode, createuserorgcode, createdate, createusername, modifydate, modifyuserid, 
                        modifyusername, scoretype, score, evaluatetype, evaluatetypename, evaluatedept, 
                        evaluatedeptname, evaluatescore, evaluatecontent, evaluateother, safetyassessmentid from epg_safetyassessmentperson where {0}", queryJson);
            return this.BaseRepository().FindList(strSql.ToString());
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SafetyassessmentpersonEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

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
        public void SaveForm(string keyValue, SafetyassessmentpersonEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                string[] deptIds = entity.EVALUATEDEPT.Split(',');
                string[] deptNames = entity.EVALUATEDEPTNAME.Split(',');
                entity.EVALUATEDEPT = deptIds[0];
                entity.EVALUATEDEPTNAME = deptNames[0];
                this.BaseRepository().Update(entity);
            }
            else
            {
                // ��ӵ�ʱ����Ҫ�ָ�����

                List<SafetyassessmentpersonEntity> list = new List<SafetyassessmentpersonEntity>();
                string[] deptIds = entity.EVALUATEDEPT.Split(',');
                string[] deptNames = entity.EVALUATEDEPTNAME.Split(',');
                for (int i = 0; i < deptIds.Length; i++)
                {
                    entity.Create();
                    entity.ID = Guid.NewGuid().ToString();
                    entity.EVALUATEDEPT = deptIds[i];
                    entity.EVALUATEDEPTNAME = deptNames[i];
                    this.BaseRepository().Insert(entity);
                    list.Add(entity);
                }



            }
        }

        /// <summary>
        ///  ���ݰ�������IDɾ��������Ϣ��
        /// </summary>
        /// <param name="keyValue"></param>
        public void DelByKeyId(string keyValue)
        {
            int number = DbFactory.Base().ExecuteBySql(string.Format("delete from  Epg_Safetyassessmentperson   WHERE  SAFETYASSESSMENTID ='{0}'", keyValue));
        }
        #endregion
    }
}
