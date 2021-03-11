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
            this.BaseRepository().ExecuteBySql(string.Format("Delete bis_district where districtCode like '{0}%'",entity.DistrictCode));
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

            OrganizeEntity oEntity = null;

            DistrictEntity dEntity = null;

            string deptcode = string.Empty;

            if (districtEntity.ParentID == "0")//ѡ���ǻ���,û��ѡ���ϼ�����
            {
                oEntity = new OrganizeService().BaseRepository().FindEntity(districtEntity.OrganizeId);  //��ȡ������(����)

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
