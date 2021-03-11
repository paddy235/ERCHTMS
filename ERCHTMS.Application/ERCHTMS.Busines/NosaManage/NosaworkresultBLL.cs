using ERCHTMS.Entity.NosaManage;
using ERCHTMS.IService.NosaManage;
using ERCHTMS.Service.NosaManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;


namespace ERCHTMS.Busines.NosaManage
{
    /// <summary>
    /// �� ���������ɹ�
    /// </summary>
    public class NosaworkresultBLL
    {
        private NosaworkresultIService service = new NosaworkresultService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<NosaworkresultEntity> GetList(string queryJson)
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
        public NosaworkresultEntity GetEntity(string keyValue)
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
                var entity = service.GetEntity(keyValue);
                if (entity != null && !string.IsNullOrWhiteSpace(entity.TemplatePath))
                {
                    string filename = System.Web.Hosting.HostingEnvironment.MapPath(entity.TemplatePath);
                    if (System.IO.File.Exists(filename))
                    {
                        System.IO.File.Delete(filename);
                    }
                }
                service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void RemoveByWorkId(string keyValue)
        {
            try
            {
                var list = service.GetList(string.Format(" and workid='{0}'", keyValue));
                foreach (var entity in list)
                {
                    if (!string.IsNullOrWhiteSpace(entity.TemplatePath))
                    {
                        string filename = System.Web.Hosting.HostingEnvironment.MapPath(entity.TemplatePath);
                        if (System.IO.File.Exists(filename))
                        {
                            System.IO.File.Delete(filename);
                        }
                        service.RemoveForm(entity.ID);
                    }
                }
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
        public void SaveForm(string keyValue, NosaworkresultEntity entity)
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
