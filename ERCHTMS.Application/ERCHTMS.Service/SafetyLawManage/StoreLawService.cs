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
using ERCHTMS.Code;
using ERCHTMS.Service.BaseManage;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Service.SafetyLawManage
{
    /// <summary>
    /// �� �����ղط��ɷ���
    /// </summary>
    public class StoreLawService : RepositoryFactory<StoreLawEntity>, StoreLawIService
    {
        DepartmentService DepartmentService = new DepartmentService();
        #region ��ȡ����
        /// <summary>
        /// ��ȡ��ȫ�������ɷ����ҵ��ղ��б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageDataTable(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //��ѯ����
            if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            {
                string condition = queryParam["condition"].ToString();
                string keyord = queryParam["keyword"].ToString();
                switch (condition)
                {
                    case "filename"://�ļ�����
                        pagination.conditionJson += string.Format(" and b.FileName  like '%{0}%'", keyord);
                        break;
                    case "issuedept"://�䷢����
                        pagination.conditionJson += string.Format(" and b.IssueDept  like '%{0}%'", keyord);
                        break;
                    default:
                        break;
                }
            }
            //���ͽڵ�
            if (!queryParam["code"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and b.LawTypeCode like '%{0}%'", queryParam["code"].ToString());
            }
            #region ���湤������SWP
            //��ѯ����
            if (!queryParam["filename"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and FileName  like '%{0}%'", queryParam["filename"].ToString());
            }
            //������λ
            if (!queryParam["belongcode"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and BelongTypeCode like '{0}%'", queryParam["belongcode"].ToString());
            }
            #endregion
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
                pagination.conditionJson += string.Format(" and b.id in({0})", idsarr);
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }


        /// <summary>
        /// ��ȡ��ȫ�����ƶ��ҵ��ղ��б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageJsonInstitution(Pagination pagination, string queryJson)
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
                    pagination.conditionJson += string.Format(" and t.createuserorgcode ='{0}'", queryParam["orgcode"].ToString());
                }
            }
            else
            {
                //0����,1�ϼ�
                if (!queryParam["state"].IsEmpty())
                {
                    if (queryParam["state"].ToString() == "0")
                    {
                        pagination.conditionJson += string.Format(" and t.createuserorgcode ='{0}'", user.OrganizeCode);
                    }
                    else
                    {
                        var provdata = DepartmentService.GetList().Where(t => user.NewDeptCode.StartsWith(t.DeptCode) && t.Nature == "ʡ��" && string.IsNullOrWhiteSpace(t.Description));
                        if (provdata.Count() > 0)
                        {
                            DepartmentEntity provEntity = provdata.FirstOrDefault();
                            pagination.conditionJson += string.Format(" and t.createuserorgcode ='{0}'", provEntity.EnCode);
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
                pagination.conditionJson += string.Format(" and storeid in({0})", idsarr);
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);

        }


        /// <summary>
        /// ��ȡ��ȫ��������ҵ��ղ��б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageJsonStandards(Pagination pagination, string queryJson)
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
                    pagination.conditionJson += string.Format(" and t.createuserorgcode ='{0}'", queryParam["orgcode"].ToString());
                }
            }
            else
            {
                //0����,1�ϼ�
                if (!queryParam["state"].IsEmpty())
                {
                    if (queryParam["state"].ToString() == "0")
                    {
                        pagination.conditionJson += string.Format(" and t.createuserorgcode ='{0}'", user.OrganizeCode);
                    }
                    else
                    {
                        var provdata = DepartmentService.GetList().Where(t => user.NewDeptCode.StartsWith(t.DeptCode) && t.Nature == "ʡ��" && string.IsNullOrWhiteSpace(t.Description));
                        if (provdata.Count() > 0)
                        {
                            DepartmentEntity provEntity = provdata.FirstOrDefault();
                            pagination.conditionJson += string.Format(" and t.createuserorgcode ='{0}'", provEntity.EnCode);
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
                pagination.conditionJson += string.Format(" and storeid in({0})", idsarr);
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);

        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<StoreLawEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public StoreLawEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// ���ݷ���idȷ���Ƿ����ղ�
        /// </summary>
        /// <returns></returns>
        public int GetStoreBylawId(string lawid)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            int num = 0;
            StoreLawEntity entity = this.BaseRepository().IQueryable().ToList().Where(t => t.LawId == lawid && t.UserId==user.UserId).FirstOrDefault();
            if (entity != null)
                num = 1;
            return num;
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
        public void SaveForm(string keyValue, StoreLawEntity entity)
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
