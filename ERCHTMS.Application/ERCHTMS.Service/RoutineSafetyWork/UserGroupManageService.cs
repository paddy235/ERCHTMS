using ERCHTMS.Entity.RoutineSafetyWork;
using ERCHTMS.IService.RoutineSafetyWork;
using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util.Extension;
using BSFramework.Util;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Data;
using BSFramework.Util.WebControl;
using ERCHTMS.Code;

namespace ERCHTMS.Service.RoutineSafetyWork
{
    /// <summary>
    /// �� �����û������
    /// </summary>
    public class UserGroupManageService : RepositoryFactory<UserGroupManageEntity>, UserGroupManageIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageList(Pagination pagination)
        {
            DatabaseType dataType = DbHelper.DbType;
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<UserGroupManageEntity> GetList()
        {
            return this.BaseRepository().IQueryable().OrderByDescending(t => t.CreateDate).ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public UserGroupManageEntity GetEntity(string keyValue)
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

            var queryParam = queryJson.ToJObject();
            //��ѯ����
            if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            {
                string condition = queryParam["condition"].ToString();
                string keyord = queryParam["keyword"].ToString();
                switch (condition)
                {
                    case "Account":            //�˻�
                        pagination.conditionJson += string.Format(" and ACCOUNT  like '%{0}%'", keyord);
                        break;
                    case "RealName":          //����
                        pagination.conditionJson += string.Format(" and REALNAME  like '%{0}%'", keyord);
                        break;
                    case "Mobile":          //�ֻ�
                        pagination.conditionJson += string.Format(" and MOBILE like '%{0}%'", keyord);
                        break;
                    default:
                        break;
                }
            }
            if (!queryParam["state"].IsEmpty() && queryParam["state"].ToString() == "1")
            {
                //�û���id
                string userListId = queryParam["code"].ToString();
                pagination.conditionJson += string.Format(" and userid in(select e.userid from BIS_UserListManage e where e.moduleid='{0}')", userListId);
            }
            else {
                //��˾����
                if (!queryParam["organizeId"].IsEmpty())
                {
                    string organizeId = queryParam["organizeId"].ToString();
                    pagination.conditionJson += string.Format(" and ORGANIZEID = '{0}'", organizeId);
                }
                //��������
                if (!queryParam["departmentId"].IsEmpty())
                {
                    string departmentId = queryParam["departmentId"].ToString();
                    pagination.conditionJson += string.Format(" and DEPARTMENTID = '{0}'", departmentId);
                }
               
                if (!queryParam["code"].IsEmpty() && !queryParam["isOrg"].IsEmpty())
                {
                    Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                    string deptCode = queryParam["code"].ToString();
                    if (queryParam["isOrg"].ToString() == "Organize")
                    {
                        pagination.conditionJson += string.Format(" and organizecode  like '{0}%'", deptCode);
                        pagination.conditionJson += string.Format(" and (departmentcode like '{0}%'", deptCode);
                        //if (user.IsSystem || user.RoleName.Contains("��˾���û�") || user.RoleName.Contains("���������û�"))
                        //{
                        //    pagination.conditionJson += string.Format(" and (departmentcode like '{0}%'", deptCode);
                        //}
                        //else
                        //{
                        //    pagination.conditionJson += string.Format(" and (departmentcode like '{0}%'", user.DeptCode);
                        //}
                        pagination.conditionJson += string.Format(" or departmentid in(select departmentid from BASE_DEPARTMENT t where senddeptid=(select departmentid from base_department d where d.encode='{0}')))", user.DeptCode);
                    }

                    else if (queryParam["isOrg"].ToString() == "Department" || user.IsSystem)
                    {
                        pagination.conditionJson += string.Format(" and departmentcode like '{0}%'", deptCode);
                    }
                    else
                    {
                        pagination.conditionJson += string.Format(" and (departmentcode like '{0}%'", deptCode);
                        pagination.conditionJson += string.Format(" and departmentid in(select departmentid from BASE_DEPARTMENT t where t.senddeptid=(select departmentid from base_department d where d.encode='{0}')))", user.DeptCode);
                    }
                }
            }
            
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue)
        {
            Repository<UserListManageEntity> rep = new Repository<UserListManageEntity>(DbFactory.Base());
            this.BaseRepository().Delete(keyValue);
            rep.ExecuteBySql(string.Format("delete bis_userlistmanage where moduleid='{0}' ", keyValue));
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, UserGroupManageEntity entity)
        {
            Repository<UserListManageEntity> rep = new Repository<UserListManageEntity>(DbFactory.Base());
            
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
                rep.ExecuteBySql(string.Format("delete bis_userlistmanage where moduleid='{0}' ", keyValue));
                var arrId = entity.UserId.Split(',');
                List<UserListManageEntity> list = new List<UserListManageEntity>();

                foreach (string userId in arrId)
                {
                    UserListManageEntity newEntity = new UserListManageEntity();
                    newEntity.Create();
                    newEntity.UserId = userId;
                    newEntity.ModuleId = entity.Id;
                    newEntity.ModuleType = "2";
                    list.Add(newEntity);
                    //rep.Insert(newEntity);
                }
                rep.Insert(list);
            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
                var arrId = entity.UserId.Split(',');
                List<UserListManageEntity> list = new List<UserListManageEntity>();
                
                foreach (string userId in arrId)
                {
                    UserListManageEntity newEntity = new UserListManageEntity();
                    newEntity.Create();
                    newEntity.UserId = userId;
                    newEntity.ModuleId = entity.Id;
                    newEntity.ModuleType = "2";
                    list.Add(newEntity);
                    //rep.Insert(newEntity);
                }
                rep.Insert(list);
            }
        }
        #endregion
    }
}
