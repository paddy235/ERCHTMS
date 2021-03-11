using ERCHTMS.Entity.SafetyLawManage;
using ERCHTMS.IService.SafetyLawManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.Data;
using System;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Code;
using ERCHTMS.Service.BaseManage;

namespace ERCHTMS.Service.SafetyLawManage
{
    /// <summary>
    /// �� ������ȫ�������
    /// </summary>
    public class SafeStandardsService : RepositoryFactory<SafeStandardsEntity>, SafeStandardsIService
    {
        DepartmentService DepartmentService = new DepartmentService();
        #region ��ȡ����

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageDataTable(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var queryParam = queryJson.ToJObject();
            //��ѯ����
            if (!queryParam["filename"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and FileName like '%{0}%'", queryParam["filename"].ToString());
            }

            if (user.RoleName.Contains("ʡ���û�"))
            {
                if (!queryParam["orgcode"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and createuserorgcode ='{0}'", queryParam["orgcode"].ToString());
                }
            }
            else
            {
                //0����,1�ϼ�
                if (!queryParam["state"].IsEmpty())
                {
                    if (queryParam["state"].ToString() == "0")
                    {
                        pagination.conditionJson += string.Format(" and createuserorgcode ='{0}'", user.OrganizeCode);
                    }
                    else
                    {
                        var provdata = DepartmentService.GetList().Where(t => user.NewDeptCode.StartsWith(t.DeptCode) && t.Nature == "ʡ��" && string.IsNullOrWhiteSpace(t.Description));
                        if (provdata.Count() > 0)
                        {
                            DepartmentEntity provEntity = provdata.FirstOrDefault();
                            pagination.conditionJson += string.Format(" and createuserorgcode ='{0}'", provEntity.EnCode);
                        }
                    }
                }
            }
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
                    endTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
                pagination.conditionJson += string.Format(" and ReleaseDate between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", startTime, endTime);
            }
            //ѡ�������
            if (!queryParam["treeCode"].IsEmpty())
            {
                if (!queryParam["flag"].IsEmpty())
                {
                    if (queryParam["flag"].ToString() == "0")
                    {
                        pagination.conditionJson += string.Format(" and  LawTypeCode='{0}'", queryParam["treeCode"].ToString());
                    }
                    else
                    {
                        pagination.conditionJson += string.Format(@" and LawTypeCode like '{0}%'", queryParam["treeCode"].ToString());
                    }
                }
                else
                {
                    pagination.conditionJson += string.Format(" and  LawTypeCode='{0}'", queryParam["treeCode"].ToString());
                }
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
            if (!queryParam["code"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and ISSUEDEPT in (select departmentid from base_department where encode like '{0}%')", queryParam["code"].ToString());
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<SafeStandardsEntity> GetPageList(Pagination pagination, string queryJson)
        {
            if (!string.IsNullOrWhiteSpace(queryJson))
            {
                string sql = "select * from BIS_SAFESTANDARDS";
                if (queryJson.Trim().StartsWith("and"))
                {
                    sql += " where 1=1 " + queryJson;
                }
                else
                {
                    sql += " where " + queryJson;
                }
                return this.BaseRepository().FindList(sql).ToList();
            }
            else
            {
                return this.BaseRepository().IQueryable().ToList();
            }
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SafeStandardsEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SafeStandardsEntity GetEntity(string keyValue)
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
                db.Delete<SafeStandardsEntity>(keyValue);
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
        public void SaveForm(string keyValue, SafeStandardsEntity entity)
        {
            bool b = true;
            if (!string.IsNullOrWhiteSpace(keyValue))
            {
                var sl = BaseRepository().FindEntity(keyValue);
                if (sl != null)
                {
                    b = false;
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }
            }
            if (b)
            {
                entity.Id = keyValue;
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }
        #endregion
    }
}
