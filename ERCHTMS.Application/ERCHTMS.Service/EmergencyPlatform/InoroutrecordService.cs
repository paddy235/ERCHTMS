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
namespace ERCHTMS.Service.EmergencyPlatform
{
    /// <summary>
    /// �� ����������¼
    /// </summary>
    public class InoroutrecordService : RepositoryFactory<InoroutrecordEntity>, IInoroutrecordService
    {
        #region ��ȡ����

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="condition">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<InoroutrecordEntity> GetListForCon(Expression<Func<InoroutrecordEntity, bool>> condition)
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
                        //case "PostName":            //�˻�
                        //    pagination.conditionJson += string.Format(" and PostName  like '%{0}%'", keyord);
                        //    break;
                        //case "RealName":          //����
                        //    pagination.conditionJson += string.Format(" and REALNAME  like '%{0}%'", keyord);
                        //    break;
                        //case "Mobile":          //�ֻ�
                        //    pagination.conditionJson += string.Format(" and MOBILE like '%{0}%'", keyord);
                        //    break;
                        default:
                            break;
                    }
                }
                //��˾����
                //if (!queryParam["organizeId"].IsEmpty())
                //{
                //    string organizeId = queryParam["organizeId"].ToString();
                //    pagination.conditionJson += string.Format(" and ORGANIZEID = '{0}'", organizeId);
                //}
                ////��������
                //if (!queryParam["departmentId"].IsEmpty())
                //{
                //    string departmentId = queryParam["departmentId"].ToString();
                //    pagination.conditionJson += string.Format(" and DEPARTMENTID = '{0}'", departmentId);
                //}

                //if (!queryParam["code"].IsEmpty() && !queryParam["isOrg"].IsEmpty())
                //{
                //    Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                //    string deptCode = queryParam["code"].ToString();
                //    if (queryParam["isOrg"].ToString() == "Organize")
                //    {
                //        pagination.conditionJson += string.Format(" and organizecode  like '{0}%'", deptCode);
                //        if (user.IsSystem || user.RoleName.Contains("��˾���û�") || user.RoleName.Contains("���������û�"))
                //        {
                //            pagination.conditionJson += string.Format(" and (departmentcode like '{0}%'", deptCode);
                //        }
                //        else
                //        {
                //            pagination.conditionJson += string.Format(" and (departmentcode like '{0}%'", user.DeptCode);
                //        }
                //        pagination.conditionJson += string.Format(" or departmentid in(select departmentid from BASE_DEPARTMENT t where senddeptid=(select departmentid from base_department d where d.encode='{0}')))", user.DeptCode);
                //    }

                //    else if (queryParam["isOrg"].ToString() == "Department" || user.IsSystem)
                //    {
                //        pagination.conditionJson += string.Format(" and departmentcode like '{0}%'", deptCode);
                //    }
                //    else
                //    {
                //        pagination.conditionJson += string.Format(" and (departmentcode like '{0}%'", deptCode);
                //        pagination.conditionJson += string.Format(" and departmentid in(select departmentid from BASE_DEPARTMENT t where t.senddeptid=(select departmentid from base_department d where d.encode='{0}')))", user.DeptCode);
                //    }
                //}
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<InoroutrecordEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public InoroutrecordEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, InoroutrecordEntity entity)
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
