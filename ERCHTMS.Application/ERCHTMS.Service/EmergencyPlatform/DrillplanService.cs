using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using BSFramework.Util.Extension;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Linq;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.IService.AuthorizeManage;
using ERCHTMS.Service.AuthorizeManage;
using System;
using ERCHTMS.Code;
using ERCHTMS.Entity.EmergencyPlatform;
using ERCHTMS.IService.EmergencyPlatform;
using System.Linq.Expressions;
using ERCHTMS.Service.CommonPermission;

namespace ERCHTMS.Service.EmergencyPlatform
{
    /// <summary>
    /// �� ����Ӧ������
    /// </summary>
    public class DrillplanService : RepositoryFactory<DrillplanEntity>, IDrillplanService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="condition">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<DrillplanEntity> GetListForCon(Expression<Func<DrillplanEntity, bool>> condition)
        {
            return this.BaseRepository().IQueryable(condition).ToList();
        }
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
                //��ѯ����
                if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
                {
                    string condition = queryParam["condition"].ToString();
                    string keyord = queryParam["keyword"].ToString();
                    switch (condition)
                    {
                        case "DrillTypeName":            //�˻�
                            pagination.conditionJson += string.Format(" and DrillTypeName  like '%{0}%'", keyord);
                            break;
                        case "Name":          //����
                            pagination.conditionJson += string.Format(" and Name  like '%{0}%'", keyord);
                            break;
                        case "DrillModeName":          //�ֻ�
                            pagination.conditionJson += string.Format(" and DrillModeName like '%{0}%'", keyord);
                            break;
                        default:
                            break;
                    }
                }
                if (!queryParam["DrillType"].IsEmpty())
                {
                    string DrillType = queryParam["DrillType"].ToString();
                    pagination.conditionJson += string.Format(" and DrillType = '{0}'", DrillType);
                }
                if (!queryParam["DrillMode"].IsEmpty())
                {
                    string DrillMode = queryParam["DrillMode"].ToString();
                    pagination.conditionJson += string.Format(" and DrillMode = '{0}'", DrillMode);
                }
                if (!queryParam["happentimestart"].IsEmpty())
                {
                    string happentimestart = queryParam["happentimestart"].ToString();
                    pagination.conditionJson += string.Format(" and plantime >= (select  to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') from dual)", happentimestart);
                }
                if (!queryParam["happentimeend"].IsEmpty())
                {
                    string happentimeend = queryParam["happentimeend"].ToString();
                    pagination.conditionJson += string.Format(" and plantime <= (select  to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') from dual)", happentimeend);
                }
                if (!queryParam["Name"].IsEmpty())
                {
                    string Name = queryParam["Name"].ToString();
                    pagination.conditionJson += string.Format(" and Name  like '%{0}%'", Name);
                }

                #region Ȩ���ж�
                if (!queryParam["code"].IsEmpty() && !queryParam["isOrg"].IsEmpty())
                {
                    if (!queryParam["code"].IsEmpty() && !queryParam["isOrg"].IsEmpty())
                    {
                        var isOrg = queryParam["isOrg"].ToString();
                        var deptCode = queryParam["code"].ToString();
                        if (isOrg == "Organize")
                        {
                            pagination.conditionJson += string.Format(" and CREATEUSERORGCODE  like '{0}%'", deptCode);
                        }

                        else
                        {
                            pagination.conditionJson += string.Format(" and orgdeptcode like '{0}%'", deptCode);
                        }

                        //pagination = PermissionByCurrent.GetPermissionByCurrent2(pagination, queryParam["code"].ToString(), queryParam["isOrg"].ToString());
                    }
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
        public IEnumerable<DrillplanEntity> GetList(string queryJson)
        {
            return this.BaseRepository().FindList(" select * from V_mae_Drillplan where 1=1 " + queryJson).ToList();

        }

        public IEnumerable<DrillplanEntity> GetList(int year, string deptId, int monthStart, int monthEnd)
        {
            return this.BaseRepository().IQueryable().Where(e => e.CREATEDATE.Value.Year == year && e.DEPARTID == deptId && e.CREATEDATE.Value.Month >= monthStart && e.CREATEDATE.Value.Month <= monthEnd).ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public DrillplanEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, DrillplanEntity entity)
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
