using ERCHTMS.Entity.BaseManage;
using ERCHTMS.IService.BaseManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System;
using System.Data;
using System.Linq.Expressions;
using System.Data.Common;
using ERCHTMS.Entity.RiskDatabase;

namespace ERCHTMS.Service.BaseManage
{
    /// <summary>
    /// �� ������������
    /// </summary>
    public class DistrictService : RepositoryFactory<DistrictEntity>, IDistrictService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="orgID">��ǰ�û�������֯����ID</param>
        /// <returns>�����б�</returns>
        public IEnumerable<DistrictEntity> GetList(string orgID = "")
        {
            if (orgID == "")
                return this.BaseRepository().IQueryable().Where(t => t.DistrictCode.Length > 0).ToList();
            else return this.BaseRepository().IQueryable().ToList().Where(a => a.ParentID == orgID && a.DistrictCode.Length > 0);
        }

        public IEnumerable<DistrictEntity> GetListForCon(Expression<Func<DistrictEntity, bool>> condition)
        {

            return this.BaseRepository().IQueryable(condition).ToList();
        }

        public List<DistrictEntity> GetDistricts(string companyid, string districtId)
        {
            var query = from q in this.BaseRepository().IQueryable()
                        where q.OrganizeId == companyid// && q.ParentID == districtId
                        select q;

            return query.OrderBy(x => x.DistrictCode).ToList();
        }

        /// <summary>
        /// ����һ����������ȡ������Ϣ
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<DistrictEntity> GetListByOrgIdAndParentId(string orgId, string parentId)
        {
            List<DbParameter> list = new List<DbParameter>();
            string sql = string.Format(@"select districtid,parentid,districtcode,districtname,sortcode,belongdept,description,createdate,createuserid ,createusername,modifydate,
                                         modifyuserid,modifyusername,deptchargeperson,disreictchargeperson,linktel,belongcompany,linkman,linkemail,chargedept,createuserdeptcode,
                                         createuserorgcode,deptchargepersonid,disreictchargepersonid,chargedeptid,linktocompany,linktocompanyid,chargedeptcode,organizeid from bis_district  where 1=1");
            string strWhere = string.Empty;
            try
            {
                //��ʼ����״̬
                if (!string.IsNullOrEmpty(orgId))
                {
                    strWhere += @" and organizeid = @organizeid";
                    list.Add(DbParameters.CreateDbParameter("@organizeid ", orgId));
                }
                //Ŀ������״̬
                if (!string.IsNullOrEmpty(parentId))
                {
                    strWhere += @" and parentid = @parentid";
                    list.Add(DbParameters.CreateDbParameter("@parentid ", parentId));
                }
                sql += strWhere;
                sql += " order by districtcode";
                DbParameter[] param = list.ToArray();
                var result = this.BaseRepository().FindList(sql, param);

                return result.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        /// <summary>
        /// ���ݹ�˾id��ѯ������������
        /// </summary>
        /// <param name="orgID"></param>
        /// <returns></returns>
        public IEnumerable<DistrictEntity> GetOrgList(string orgID)
        {
            return this.BaseRepository().IQueryable().ToList().Where(a => a.OrganizeId == orgID && a.DistrictCode.Length > 0);
        }
        /// <summary>
        /// ��ȡ���ƺ�ID
        /// </summary>
        /// <param name="ids">id����</param>
        /// <returns>�����б�</returns>
        public DataTable GetNameAndID(string ids)
        {
            string whereSQL = "'";
            string[] arr = ids.Split(',');

            if (arr.Length > 0)
            {
                foreach (string item in arr)
                {
                    whereSQL += item + "','";
                }
                whereSQL = whereSQL + "'";
            }
            else whereSQL = whereSQL + "'";
            DataTable dt = this.BaseRepository().FindTable("select DISTRICTNAME,PARENTID,DISTRICTID,CHARGEDEPT,CHARGEDEPTCODE from bis_district where DISTRICTID in(" + whereSQL + ")");
            return dt;
        }
        /// <summary>
        /// ��ȡ���Źܿ��������Ƽ���
        /// </summary>
        /// <param name="deptId">����Id</param>
        /// <returns>�����б�Json</returns>
        public DataTable GetDeptNames(string deptId)
        {
            DataTable dt = this.BaseRepository().FindTable("select DISTRICTNAME from bis_district where CHARGEDEPTID='" + deptId + "'");
            return dt;
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public DistrictEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// ���ݻ���Id���������ƻ�ȡ����
        /// </summary>
        /// <param name="orgId">����Id</param>
        /// <param name="name">��������</param>
        /// <returns></returns>
        public DistrictEntity GetDistrict(string orgId, string name)
        {
            var expression = LinqExtensions.True<DistrictEntity>();
            expression = expression.And(t => t.OrganizeId == orgId);
            expression = expression.And(t => t.DistrictName == name);
            return this.BaseRepository().FindEntity(expression);
        }
        /// <summary>
        /// �����б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public IEnumerable<DistrictEntity> GetPageList(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();

            //��ѯ����
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyord = queryParam["keyword"].ToString();
                pagination.conditionJson += string.Format(" and t.DISTRICTNAME  like '%{0}%'", keyord);
            }
            if (!queryParam["code"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and (t.DISTRICTID='{0}'or t.PARENTID='{0}')", queryParam["code"].ToString());
            }

            IEnumerable<DistrictEntity> list = this.BaseRepository().FindListByProcPager(pagination, dataType);

            return list;

        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue)
        {
            var entity = GetEntity(keyValue);
            //this.BaseRepository().Delete(keyValue);
            this.BaseRepository().ExecuteBySql("Delete BIS_DISTRICT where DISTRICTCODE like '" + entity.DistrictCode + "%'");
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, DistrictEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                entity.Create();
                entity.DistrictCode = GetDepartmentCode(entity);
                this.BaseRepository().Insert(entity);
            }
        }
        /// <summary>
        /// ���ݵ�ǰ������ȡ��Ӧ�Ļ�������  �������� 2-6-8-10  λ
        /// </summary>
        /// <param name="districtEntity"></param>
        /// <returns></returns>
        public string GetDepartmentCode(DistrictEntity districtEntity)
        {
            string maxCode = string.Empty;

            DepartmentEntity oEntity = null;

            DistrictEntity dEntity = null;

            string deptcode = string.Empty;

            if (districtEntity.ParentID == "0")//ѡ���ǻ���,û��ѡ���ϼ�����
            {
                oEntity = new DepartmentService().BaseRepository().FindEntity(districtEntity.OrganizeId);  //��ȡ������(����)

                deptcode = oEntity.EnCode;
            }
            else //ѡ����ǲ���
            {
                dEntity = this.BaseRepository().FindEntity(districtEntity.ParentID);//��ȡ���Ÿ�����

                deptcode = dEntity.DistrictCode;
            }
            var maxObj = this.BaseRepository().FindList(string.Format("select max(districtcode) as districtcode  from bis_district t where  parentid='{0}' and organizeid='{1}'", districtEntity.ParentID, districtEntity.OrganizeId)).FirstOrDefault();
            if (!string.IsNullOrEmpty(maxObj.DistrictCode))
            {
                maxCode = new OrganizeService().QueryOrganizeCodeByCondition(maxObj.DistrictCode);
            }
            else
            {
                DistrictEntity parentEntity = this.BaseRepository().FindEntity(districtEntity.ParentID);  //��ȡ������
                if (parentEntity != null && districtEntity.ParentID != "0")
                    maxCode = parentEntity.DistrictCode + "001";  //�̶�ֵ,�ǿɱ�
                else
                    maxCode = deptcode + "001";
            }

            ////ȷ���Ƿ�����ϼ�����,�ǲ��Ÿ��ڵ�
            //if (districtEntity.ParentID != "0")
            //{
            //    //���ڣ���ȡ����������һ��
            //    if (maxObj.Count() > 0)
            //    {
            //        maxCode = maxObj.FirstOrDefault().DistrictCode;  //��ȡ����Code 
            //        if (!string.IsNullOrEmpty(maxCode))
            //        {
            //            maxCode = new OrganizeService().QueryOrganizeCodeByCondition(maxCode);
            //        }
            //    }
            //    else
            //    {
            //        DistrictEntity parentEntity = this.BaseRepository().FindEntity(districtEntity.ParentID);  //��ȡ������

            //        maxCode = parentEntity.DistrictCode + "001";  //�̶�ֵ,�ǿɱ�
            //    }
            //}
            //else  //���Ÿ��ڵ�Ĳ���
            //{
            //    //do somethings 
            //    if (maxObj.Count() > 0)
            //    {
            //        maxCode = maxObj.FirstOrDefault().DistrictCode;  //��ȡ����Code 

            //        if (!string.IsNullOrEmpty(maxCode))
            //        {
            //            maxCode = new OrganizeService().QueryOrganizeCodeByCondition(maxCode);
            //        }
            //        else
            //        {
            //            maxCode = deptcode + "001";
            //        }
            //    }
            //    else
            //    {
            //        maxCode = deptcode + "001";  //�̶�ֵ,�ǿɱ�,�ӻ������룬����±���
            //    }
            //}
            //���жϵ�ǰ�����Ƿ���������ݿ����
            return maxCode;
        }

        #endregion
    }
}
