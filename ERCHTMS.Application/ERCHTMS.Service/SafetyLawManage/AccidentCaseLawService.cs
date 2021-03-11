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
using ERCHTMS.Service.BaseManage;
using ERCHTMS.Code;

namespace ERCHTMS.Service.SafetyLawManage
{
    /// <summary>
    /// �� �����¹ʰ�����
    /// </summary>
    public class AccidentCaseLawService : RepositoryFactory<AccidentCaseLawEntity>, AccidentCaseLawIService
    {
        DepartmentService DepartmentService = new DepartmentService();
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<AccidentCaseLawEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return this.BaseRepository().FindList(pagination);
        }


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
            if (!queryParam["keyword"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and FileName  like '%{0}%'", queryParam["keyword"].ToString());
            }

            //���糧  ��ʡ��˾
            if (!queryParam["type"].IsEmpty())
            {
                if (queryParam["type"].ToString() == "0")
                {
                    pagination.conditionJson += string.Format(" and createuserorgcode  = '{0}'", user.OrganizeCode);
                }
                else if (queryParam["type"].ToString() == "1")
                {
                    IEnumerable<DepartmentEntity> orgcodelist = new List<DepartmentEntity>();
                    orgcodelist = DepartmentService.GetList().Where(t => user.NewDeptCode.Contains(t.DeptCode) && t.Nature == "ʡ��");
                    if (orgcodelist.Count() > 0)
                    {
                        pagination.conditionJson += " and (";
                        foreach (DepartmentEntity item in orgcodelist)
                        {
                            pagination.conditionJson += " createuserorgcode ='" + item.EnCode + "' or";
                        }

                        pagination.conditionJson =
                            pagination.conditionJson.Substring(0, pagination.conditionJson.Length - 2);
                        pagination.conditionJson += ")";
                    }
                }
            }

            //�¹ʷ�Χ
            if (!queryParam["range"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and AccRange='{0}'", queryParam["range"].ToString());
            }
            //ʱ��ѡ��
            if (!queryParam["st"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and  AccTime>=to_date('{0}','yyyy-mm-dd hh24:mi')", queryParam["st"].ToString().Trim());

            }
            if (!queryParam["et"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and AccTime<=to_date('{0}','yyyy-mm-dd  hh24:mi')", queryParam["et"].ToString().Trim());
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
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<AccidentCaseLawEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public AccidentCaseLawEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, AccidentCaseLawEntity entity)
        {
            bool b = false;
            if (!string.IsNullOrWhiteSpace(keyValue))
            {
                var sl = BaseRepository().FindEntity(keyValue);
                if (sl != null)
                {
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }
                else
                {
                    b = true;
                }
            }
            else
            {
                b = true;
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
