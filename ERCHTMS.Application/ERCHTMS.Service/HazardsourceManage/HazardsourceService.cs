using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using BSFramework.Util.Extension;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ERCHTMS.Service.CommonPermission;
using ERCHTMS.Entity.HazardsourceManage;
using ERCHTMS.IService.HazardsourceManage;
using ERCHTMS.Code;


namespace ERCHTMS.Service.HazardsourceManage
{
    /// <summary>
    /// �� ����Σ��Դ��ʶ����
    /// </summary>
    public class HazardsourceService : RepositoryFactory<HazardsourceEntity>, IHazardsourceService
    {
        #region ��ȡ����

        /// <summary>
        /// �û��б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;

            if (queryJson.Length > 0)
            {
                var queryParam = queryJson.ToJObject();
                #region ��ѯ����
                //��ѯ����
                if (!queryParam["DistrictName"].IsEmpty())
                {
                    string DistrictName = queryParam["DistrictName"].ToString();
                    pagination.conditionJson += " and DistrictName like '%" + DistrictName + "%'";
                }
                //����
                if (!queryParam["areaCode"].IsEmpty())
                {
                    string areaCode = queryParam["areaCode"].ToString();
                    pagination.conditionJson += string.Format(" and districtid in(select districtid from bis_district where districtcode like '{0}%')", areaCode);
                }
                //��ѯ����
                if (!queryParam["DangerSource"].IsEmpty())
                {
                    string DangerSource = queryParam["DangerSource"].ToString();
                    pagination.conditionJson += string.Format(" and DangerSource like '%{0}%'", DangerSource);
                }
                //��ѯ����
                if (!queryParam["IsDanger"].IsEmpty())
                {
                    string IsDanger = queryParam["IsDanger"].ToString();
                    pagination.conditionJson += string.Format(" and IsDanger = '{0}'", IsDanger);
                }
                //��ѯ����
                if (!queryParam["GradeVal"].IsEmpty())
                {
                    string GradeVal = queryParam["GradeVal"].ToString();
                    pagination.conditionJson += string.Format(" and GradeVal = '{0}'", GradeVal);
                }
                if (!queryParam["TimeYear"].IsEmpty())
                {
                    string TimeYear = queryParam["TimeYear"].ToString();
                    pagination.conditionJson += string.Format(" and to_char(CreateDate, 'yyyy')='{0}'", TimeYear);
                }
                if (!queryParam["State"].IsEmpty()) {
                    string State = queryParam["State"].ToString();
                    if (State == "1")
                    {
                        pagination.conditionJson += string.Format(" and t.jkyhzgids>0");
                    }
                    else if (State == "2")
                    {
                        pagination.conditionJson += string.Format(" and (t.jkskstatus='0' or t.jkskstatus is null)");
                    }
                    else if (State == "3")
                    {
                        pagination.conditionJson += string.Format(" and  (t.isdjjd = '0' or t.isdjjd is null) ");
                    }
                }
                if (!queryParam["fullName"].IsEmpty()) {
                    if (queryParam["fullName"].ToString() == "ȫ��") {

                    }
                    else
                    {
                        if (!queryParam["UnitCode"].IsEmpty())
                        {
                            pagination.conditionJson += string.Format(" and deptcode like '{0}%'", queryParam["UnitCode"].ToString());
                        }
                    }
                }
                #endregion


                #region Ȩ���ж�
                if (!queryParam["code"].IsEmpty() && !queryParam["isOrg"].IsEmpty())
                {
                    var isOrg = queryParam["isOrg"].ToString();
                    var deptCode = queryParam["code"].ToString();
                    Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

                    pagination.conditionJson += string.Format(" and DEPTCODE like '{0}%'", deptCode);

                }
                #endregion
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<HazardsourceEntity> GetList(string queryJson)
        {
            return this.BaseRepository().FindList(" select * from hsd_hazardsource where 1=1 " + queryJson).ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public HazardsourceEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion

        #region �ύ����

        /// <summary>
        /// ִ��sql
        /// </summary>
        /// <param name="sql">sql���</param>
        public int ExecuteBySql(string sql)
        {

            return this.BaseRepository().ExecuteBySql(sql);
        }

        public DataTable FindTableBySql(string sql)
        {
            return this.BaseRepository().FindTable(sql);
        }

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
        public void SaveForm(string keyValue, HazardsourceEntity entity)
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
