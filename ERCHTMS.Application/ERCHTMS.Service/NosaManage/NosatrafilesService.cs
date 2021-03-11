using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.WebControl;
using ERCHTMS.Code;
using ERCHTMS.Entity;
using ERCHTMS.Entity.NosaManage;
using ERCHTMS.IService;
using ERCHTMS.IService.NosaManage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace ERCHTMS.Service.NosaManage
{
    /// <summary>
    /// �� ������ѵ�ļ�
    /// </summary>
    public class NosatrafilesService : RepositoryFactory<NosatrafilesEntity>, NosatrafilesIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<NosatrafilesEntity> GetList(string queryJson)
        {
            var sql = string.Format("select * from hrs_nosatrafiles where 1=1 {0}", queryJson);
            return this.BaseRepository().FindList(sql);
        }
        /// <summary>
        /// ��ȡ��ҳ�б�
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var queryParam = queryJson.ToJObject();
            if (pagination.p_fields.IsEmpty())
            {
                pagination.p_fields = @"createdate,createuserid,createuserdeptcode,createuserorgcode,modifydate,modifyuserid,filename,refid,refname,pubuserid,pubusername,pubdepartid,pubdepartname,pubdate,viewtimes";
            }
            pagination.p_kid = "id";
            pagination.p_tablename = @"hrs_nosatrafiles";
            pagination.conditionJson = string.Format(" createuserorgcode='{0}'", user.OrganizeCode);
            //�ļ�����
            if (!queryParam["filename"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and filename like '%{0}%'", queryParam["filename"].ToString());
            } 
            //����id
            if (!queryParam["refid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and refid = '{0}'", queryParam["refid"].ToString());
            }

            var dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);

            return dt;
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public NosatrafilesEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, NosatrafilesEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                var old = GetEntity(keyValue);
                if (old == null)
                {
                    entity.Create();
                    entity.ID = keyValue;
                    this.BaseRepository().Insert(entity);
                }
                else
                {
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }
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

namespace ERCHTMS.Service
{
    /// <summary>
    /// �� ����ͨ�÷���
    /// </summary>
    public class BaseService<T> : RepositoryFactory<T>, BaseIService<T> where T : BSEntity, new()
    {
        #region ��������
        /// <summary>
        /// ��ȡ���͵ĵ�������
        /// </summary>
        public static TA GetSingleAttribute<TA>(Type eType) where TA : Attribute
        {
            TA r = null;

            object[] attrs = eType.GetCustomAttributes(false);
            foreach (object a in attrs)
            {
                r = a as TA;
                if (r != null)
                    break;
            }

            return r;
        }
        /// <summary>
        /// ��ȡ���͵����Լ���
        /// </summary>
        public static List<TA> GetAttributes<TA>(Type eType) where TA : Attribute
        {
            List<TA> list = new List<TA>();

            object[] attrs = eType.GetCustomAttributes(false);
            foreach (object a in attrs)
            {
                TA t = a as TA;
                if (t != null)
                {
                    list.Add(t);
                }
            }

            return list;
        }
        #endregion

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<T> GetList(string queryJson)
        {
            var eType = typeof(T);
            var tAttr = GetSingleAttribute<TableAttribute>(eType);
            if (tAttr == null)
                throw new Exception("ʵ��δ���TableAttribute����ָ������");

            var sql = string.Format("select * from {0} where 1=1 {1}", tAttr.Name.ToLower(), queryJson);
            return this.BaseRepository().FindList(sql);
        }
        /// <summary>
        /// ��ȡ��ҳ�б�
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;      
            var dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);

            return dt;
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public T GetEntity(string keyValue)
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
        public void RemoveForm(T entity)
        {
            this.BaseRepository().Delete(entity);
        }
        public void RemoveForm(List<T> entities)
        {
            this.BaseRepository().Delete(entities);
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, T entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                var old = GetEntity(keyValue);
                if (old == null)
                {
                    entity.Create();
                    entity.ID = keyValue;
                    this.BaseRepository().Insert(entity);
                }
                else
                {
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }
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