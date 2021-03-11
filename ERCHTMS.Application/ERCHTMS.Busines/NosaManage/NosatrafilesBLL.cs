using ERCHTMS.Entity.NosaManage;
using ERCHTMS.IService.NosaManage;
using ERCHTMS.Service.NosaManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.IService;
using ERCHTMS.Service;
using ERCHTMS.Code;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Entity;
using ERCHTMS.Business;
using ERCHTMS.Entity.SafetyLawManage;

namespace ERCHTMS.Busines.NosaManage
{
    /// <summary>
    /// �� ������ѵ�ļ�
    /// </summary>
    public class NosatrafilesBLL
    {
        private NosatrafilesIService service = new NosatrafilesService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<NosatrafilesEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡ��ҳ�б�
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetList(Pagination pagination, string queryJson)
        {
            return service.GetList(pagination, queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public NosatrafilesEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, NosatrafilesEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }

    /// <summary>
    /// ��������ѵ�ļ�
    /// </summary>
    public class NosatrafilesBsBLL:BaseBLL<NosatrafilesEntity>
    {
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public override DataTable GetList(Pagination pagination, string queryJson)
        {
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

            return base.GetList(pagination, queryJson);
        }
    }       
}

namespace ERCHTMS.Business
{
    /// <summary>
    /// ����ҵ����
    /// </summary>
    public class BaseBLL<T> where T : BSEntity, new()
    {
        protected BaseIService<T> service = new BaseService<T>();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public virtual IEnumerable<T> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡ��ҳ�б�
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public virtual DataTable GetList(Pagination pagination, string queryJson)
        {
            return service.GetList(pagination, queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public virtual T GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public virtual void RemoveForm(string keyValue)
        {
            try
            {                
                service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public virtual void RemoveForm(T entity)
        {
            try
            {
                service.RemoveForm(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public virtual void RemoveForm(List<T> entities)
        {
            try
            {
                service.RemoveForm(entities);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public virtual void SaveForm(string keyValue, T entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}