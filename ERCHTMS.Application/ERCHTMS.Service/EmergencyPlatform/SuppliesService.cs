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
using ERCHTMS.Service.BaseManage;

namespace ERCHTMS.Service.EmergencyPlatform
{
    /// <summary>
    /// �� ����Ӧ������
    /// </summary>
    public class SuppliesService : RepositoryFactory<SuppliesEntity>, ISuppliesService
    {
        #region ��ȡ����

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="condition">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SuppliesEntity> GetListForCon(Expression<Func<SuppliesEntity, bool>> condition)
        {

            return this.BaseRepository().IQueryable(condition).ToList();
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SuppliesEntity> GetList(string queryJson)
        {
            return this.BaseRepository().FindList(" select * from v_mae_Supplies where 1=1 " + queryJson).ToList();
        }

        public IEnumerable<SuppliesEntity> GetMutipleDataJson(string Ids)
        {
            if (!string.IsNullOrWhiteSpace(Ids))
            {
                string str = string.Join("','", Ids.Split(','));
                return this.BaseRepository().FindList(" select * from v_mae_Supplies where id in('" + str + "')").ToList();
            }
            else
            {
                return new List<SuppliesEntity>();
            }
        }

        /// <summary>
        /// ���������˻�ȡ���������
        /// </summary>
        /// <param name="DutyPerson"></param>
        /// <returns></returns>
        public IEnumerable<SuppliesEntity> GetDutySuppliesDataJson(string DutyPerson)
        {
            if (!string.IsNullOrWhiteSpace(DutyPerson))
            {
                return this.BaseRepository().FindList(" select * from v_mae_Supplies where iscontaions(USERID,'" + DutyPerson + "')>0").ToList();
            }
            else
            {
                return new List<SuppliesEntity>();
            }
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public string GetMaxCode()
        {
            var entity = this.BaseRepository().FindList(" select max(SuppliesCode) as SuppliesCode from mae_Supplies").FirstOrDefault();
            if (entity == null || entity.SUPPLIESCODE == null)
                return DateTime.Now.ToString("yyyy") + "0001";
            return (int.Parse(entity.SUPPLIESCODE) + 1).ToString();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SuppliesEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
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
                if (!queryParam["condition"].IsEmpty())
                {
                    string condition = queryParam["condition"].ToString();
                    string SuppliesCode = queryParam["SuppliesCode"].ToString();
                    string SuppliesName = queryParam["SuppliesName"].ToString();
                    string StoragePlace = queryParam["StoragePlace"].ToString();
                    string UserName = queryParam["UserName"].ToString();
                    //switch (condition)
                    //{
                    //    case "SuppliesCode":            //�˻�
                    //        pagination.conditionJson += string.Format(" and SuppliesCode  like '%{0}%'", SuppliesCode);
                    //        break;
                    //    //case "SuppliesTypeName":          //����
                    //    //    pagination.conditionJson += string.Format(" and SuppliesTypeName  like '%{0}%'", keyord);
                    //    //    break;
                    //    case "SuppliesName":          //�ֻ�
                    //        pagination.conditionJson += string.Format(" and SuppliesName like '%{0}%'", SuppliesName);
                    //        break;
                    //    case "StoragePlace":          //�ֻ�
                    //        pagination.conditionJson += string.Format(" and StoragePlace like '%{0}%'", StoragePlace);
                    //        break;
                    //    case "UserName":          //�ֻ�
                    //        pagination.conditionJson += string.Format(" and UserName like '%{0}%'", UserName);
                    //        break;
                    //    default:
                    //        break;
                    //}
                }
                if (!queryParam["SuppliesCode"].IsEmpty())
                {
                    var SuppliesCode = queryParam["SuppliesCode"].ToString();
                    pagination.conditionJson += string.Format(" and SuppliesCode  like '%{0}%'", SuppliesCode);
                }

                if (!queryParam["SuppliesName"].IsEmpty())
                {
                    var SuppliesName = queryParam["SuppliesName"].ToString();
                    pagination.conditionJson += string.Format(" and SuppliesName like '%{0}%'", SuppliesName);
                }
                if (!queryParam["StoragePlace"].IsEmpty())
                {
                    var StoragePlace = queryParam["StoragePlace"].ToString();
                    pagination.conditionJson += string.Format(" and StoragePlace like '%{0}%'", StoragePlace);
                }
                if (!queryParam["UserName"].IsEmpty())
                {
                    var UserName = queryParam["UserName"].ToString();
                    pagination.conditionJson += string.Format(" and UserName  = '{0}'", UserName);
                }
                if (!queryParam["SuppliesType"].IsEmpty())
                {
                    var SuppliesType = queryParam["SuppliesType"].ToString();
                    pagination.conditionJson += string.Format(" and SuppliesType  = '{0}'", SuppliesType);
                }
                if (!queryParam["areaCode"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and WORKAREACODE like '{0}%'", queryParam["areaCode"].ToString());
                }

                #region Ȩ���ж�
                if (!queryParam["code"].IsEmpty() && !queryParam["isOrg"].IsEmpty())
                {
                    //pagination = PermissionByCurrent.GetPermissionByCurrent2(pagination, queryParam["code"].ToString(), queryParam["isOrg"].ToString());
                    string deptCode = queryParam["code"].ToString();
                    string isOrg = queryParam["isOrg"].ToString();
                    DepartmentService deptservice = new DepartmentService();
                    var dept = deptservice.GetEntityByCode(deptCode);
                    pagination.conditionJson += string.Format(" and  instr(',' || DEPARTID || ',','{0}')>0", dept.DepartmentId);
                }
                #endregion
            }

            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// �жϸ�������û���ڼ��
        /// </summary>
        /// <param name="keyvalue"></param>
        /// <returns></returns>
        public DataTable CheckRemove(string keyvalue)
        {
            return this.BaseRepository().FindTable("select a.* from mae_suppliesaccept a left join mae_suppliesaccept_detail b on a.id=b.recid where (a.status=1 or a.status=0) and b.suppliesid='" + keyvalue + "'");
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
        public void SaveForm(string keyValue, SuppliesEntity entity)
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
        public void SaveForm(List<SuppliesEntity> slist) {
            this.BaseRepository().Insert(slist);
        }
        #endregion
    }
}
