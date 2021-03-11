using ERCHTMS.Entity.FireManage;
using ERCHTMS.IService.FireManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System;
using System.Data.Common;

namespace ERCHTMS.Service.FireManage
{
    /// <summary>
    /// �� �����ճ�Ѳ��
    /// </summary>
    public class EverydayPatrolService : RepositoryFactory<EverydayPatrolEntity>, EverydayPatrolIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            if (queryJson != null && queryJson != "")
            {
                var queryParam = queryJson.ToJObject();
                //ʱ�䷶Χ
                if (!queryParam["sTime"].IsEmpty() || !queryParam["eTime"].IsEmpty())
                {
                    string startTime = queryParam["sTime"].ToString();
                    string endTime = queryParam["eTime"].ToString();
                    if (queryParam["sTime"].IsEmpty())
                    {
                        startTime = "1899-01-01";
                    }
                    if (queryParam["eTime"].IsEmpty())
                    {
                        endTime = "2099-01-01";
                        //endTime = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    endTime = (Convert.ToDateTime(endTime).AddDays(1)).ToString("yyyy-MM-dd");
                    pagination.conditionJson += string.Format(" and PatrolDate between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", startTime, endTime);
                }
                //��ѯ����
                if (!queryParam["PatrolPersonId"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and PatrolPersonId='{0}'", queryParam["PatrolPersonId"].ToString());
                }
                //����
                if (!queryParam["PatrolDeptCode"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and PatrolDeptCode like '{0}%'", queryParam["PatrolDeptCode"].ToString());
                }
                //Ѳ������
                if (!queryParam["PatrolTypeCode"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and PatrolTypeCode='{0}'", queryParam["PatrolTypeCode"].ToString());
                }
                //����״̬
                if (!queryParam["AffirmState"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and AffirmState='{0}'", queryParam["AffirmState"].ToString());
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<EverydayPatrolEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public EverydayPatrolEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// ��ȡ���Ÿ������˻�
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string GetMajorUserId(string departmentid)
        {
            string sql = @"select ACCOUNT from base_user where instr(rolename,'������' )> 0 and departmentid =@departmentid";
            DataTable dt = this.BaseRepository().FindTable(sql, new DbParameter[] { DbParameters.CreateDbParameter("@departmentid", departmentid) });
            string approverPeopleIds = "";
            foreach (DataRow dr in dt.Rows)
            {
                approverPeopleIds += dr["account"] + ",";
            }
            return !string.IsNullOrEmpty(approverPeopleIds) ? approverPeopleIds.Substring(0, approverPeopleIds.Length - 1) : "";
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
        public void SaveForm(string keyValue, EverydayPatrolEntity entity)
        {
            entity.Id = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                EverydayPatrolEntity ee = this.BaseRepository().FindEntity(keyValue);
                if (ee == null)
                {
                    if (entity.PatrolTypeCode == "RJ" || entity.PatrolTypeCode == "ZJ")
                    {
                        entity.AffirmUserId = entity.DutyUserId;
                    }
                    else
                    {
                        entity.AffirmUserId = entity.ByUserId;
                    }
                    //entity.AffirmState = 0;
                    entity.Create();
                    this.BaseRepository().Insert(entity);
                }
                else
                {
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }
            }
            else
            {
                if (entity.PatrolTypeCode == "RJ" || entity.PatrolTypeCode == "ZJ")
                {
                    entity.AffirmUserId = entity.DutyUserId;
                }
                else
                {
                    entity.AffirmUserId = entity.ByUserId;
                }
                //entity.AffirmState = 0;
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }
        #endregion
    }
}
