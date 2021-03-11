using ERCHTMS.Entity.StandardSystem;
using ERCHTMS.IService.StandardSystem;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.Data;
using System;
using ERCHTMS.Entity.SafetyLawManage;
using Newtonsoft.Json.Linq;
using ERCHTMS.Code;


namespace ERCHTMS.Service.StandardSystem
{
    /// <summary>
    /// �� �������湤������swp
    /// </summary>
    public class WrittenWorkService : RepositoryFactory<WrittenWorkEntity>, WrittenWorkIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageDataTable(Pagination pagination, string queryJson, string authType)
        {
            DatabaseType dataType = DatabaseType.Oracle;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            #region ���
            pagination.p_kid = "Id as swpid";
            pagination.p_fields = "CreateDate,FileName,IssueDept,FileCode,PublishDate,CarryDate,FilesId,createuserid,createuserdeptcode,createuserorgcode,belongtypecode";
            pagination.p_tablename = " hrs_writtenwork";
            #endregion
            pagination.conditionJson = "1=1";
            if (!user.IsSystem)
            {
               
                if (!string.IsNullOrEmpty(authType))
                {
                    switch (authType)
                    {
                        case "1":
                            pagination.conditionJson += " and createuserid='" + user.UserId + "'";
                            break;
                        case "2":
                            pagination.conditionJson += " and BelongTypeCode='" + user.DeptCode + "'";
                            break;
                        case "3":
                            pagination.conditionJson += " and BelongTypeCode like'" + user.DeptCode + "%'";
                            break;
                        case "4":
                            pagination.conditionJson += " and BelongTypeCode like'" + user.OrganizeCode + "%'";
                            break;
                    }
                }
                else
                {
                    pagination.conditionJson += " and 0=1";
                }
            }
            #region  ɸѡ����
            var queryParam = queryJson.ToJObject();
            //��ѯ����
            if (!queryParam["keyword"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and FileName  like '%{0}%'", queryParam["keyword"].ToString());
            }
            //���ͽڵ�
            if (!queryParam["code"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and BelongTypeCode like '{0}%'", queryParam["code"].ToString());
            }
            //ѡ�е�����
            if (!queryParam["idsData"].IsEmpty())
            {
                var ids = queryParam["idsData"].ToString();
                string idsarr = "";
                if (ids.Contains(','))
                {
                    string[] array = ids.TrimEnd(',').Split(',');
                    foreach (var item in array)
                    {
                        idsarr += "'" + item + "',";
                    }
                    if (idsarr.Contains(","))
                        idsarr = idsarr.TrimEnd(',');
                }
                pagination.conditionJson += string.Format(" and id in({0})", idsarr);
            }
            #endregion
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }


        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<WrittenWorkEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return this.BaseRepository().FindList(pagination);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<WrittenWorkEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public WrittenWorkEntity GetEntity(string keyValue)
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
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            try
            {
                db.Delete<WrittenWorkEntity>(keyValue);
                db.Delete<StoreLawEntity>(t => t.LawId
.Equals(keyValue));
                db.Commit();
            }
            catch (Exception)
            {
                db.Rollback();
                throw;
            }
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, WrittenWorkEntity entity)
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

