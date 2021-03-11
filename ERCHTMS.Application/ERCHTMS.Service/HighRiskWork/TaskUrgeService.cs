using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.Data;
using System;
using ERCHTMS.Code;
using System.Data.Common;
using ERCHTMS.Entity.BaseManage;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ERCHTMS.Service.HighRiskWork
{
    /// <summary>
    /// �� ������վ�������
    /// </summary>
    public class TaskUrgeService : RepositoryFactory<TaskUrgeEntity>, TaskUrgeIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<TaskUrgeEntity> GetList(string queryJson)
        {
            return this.BaseRepository().FindList(string.Format("select * from bis_taskurge where 1=1 " + queryJson)).ToList();
        }


        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DatabaseType.Oracle;
            #region ����Ȩ��
            #endregion
            #region ���
            pagination.p_kid = "a.id";
            pagination.p_fields = "idea,files,urgetime,urgeuserid,urgeusername,deptname,deptcode,deptid,staffid,createdate,dataissubmit";
            pagination.p_tablename = " bis_taskurge a";
            pagination.conditionJson = "1=1";
            #endregion
            #region  ɸѡ����
            var queryParam = JObject.Parse(queryJson);
            if (!queryParam["staffid"].IsEmpty())//����id
            {
                pagination.conditionJson += string.Format(" and staffid='{0}'", queryParam["staffid"].ToString());
            }
            #endregion
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public TaskUrgeEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, TaskUrgeEntity entity)
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
