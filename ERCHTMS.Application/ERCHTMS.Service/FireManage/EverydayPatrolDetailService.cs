using ERCHTMS.Entity.FireManage;
using ERCHTMS.IService.FireManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System;

namespace ERCHTMS.Service.FireManage
{
    /// <summary>
    /// �� �����ճ�Ѳ�飨���ݣ�
    /// </summary>
    public class EverydayPatrolDetailService : RepositoryFactory<EverydayPatrolDetailEntity>, EverydayPatrolDetailIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<EverydayPatrolDetailTjEntity> GetList(string queryJson)
        {
            //DatabaseType dataType = DbHelper.DbType;
            //if (queryJson != null && queryJson != "")
            //{
            //    var queryParam = queryJson.ToJObject();
            //    if (!queryParam["PatrolId"].IsEmpty())
            //    {
            //        string PatrolId = queryParam["PatrolId"].ToString();
            //        //return this.BaseRepository().IQueryable().Where(t => t.PatrolId == PatrolId).ToList();
            //        return this.BaseRepository().IQueryable(t => t.PatrolId == PatrolId).OrderBy(t => t.OrderNumber).ToList();
            //    }
            //    else
            //    {
            //        return this.BaseRepository().IQueryable().OrderBy(t => t.OrderNumber).ToList();
            //    }
            //}
            //else
            //{
            //    return this.BaseRepository().IQueryable().ToList();
            //}
            
            queryJson = queryJson ?? "";
           
            string sql= string.Format(@"select Id,CreateUserId,CreateUserDeptCode,CreateUserOrgCode,CreateDate,CreateUserName,PatrolId,ResultTrue,ResultFalse,Result,PatrolContent,Problem,Dispose,OrderNumber,
              (select count(1) from bis_htbaseinfo o where o.RelevanceId=e.id) as yhCount,
              (select count(1) from BIS_LLLEGALREGISTER l where l.reseverone=e.id) as wzCount  
              from HRS_EVERYDAYPATROLDETAIL e");
            sql += "  where 1=1";
            if (queryJson != null && queryJson != "")
            {
                var queryParam = queryJson.ToJObject();
                
                //��������ID
                if (!queryParam["PatrolId"].IsEmpty())
                {
                    sql += string.Format(" and PatrolId='{0}'", queryParam["PatrolId"].ToString());
                }
            }
            IRepository db = new RepositoryFactory().BaseRepository();
            IEnumerable<EverydayPatrolDetailTjEntity> fe = db.FindList<EverydayPatrolDetailTjEntity>(sql+ " order by OrderNumber asc").ToList();

            return fe;
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public EverydayPatrolDetailEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, EverydayPatrolDetailEntity entity)
        {
            entity.Id = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                EverydayPatrolDetailEntity ee = this.BaseRepository().FindEntity(keyValue);
                if (ee == null)
                {
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
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }
        #endregion
    }
}
