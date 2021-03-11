using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.IService.HiddenTroubleManage;
using ERCHTMS.Service.HiddenTroubleManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.HiddenTroubleManage
{
    /// <summary>
    /// �� ��������ȫ���ƻ�
    /// </summary>
    public class MatrixsafecheckBLL
    {
        private MatrixsafecheckIService service = new MatrixsafecheckService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�����������
        /// </summary>
        /// <returns></returns>
        public string GetActionNum()
        {
            return service.GetActionNum();
        }

        /// <summary>
        /// ������ȡ����
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetCanlendarListJson(string queryJson)
        {
            return service.GetCanlendarListJson(queryJson);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public DataTable GetPageListJson(Pagination pagination, string queryJson)
        {
            return service.GetPageListJson(pagination, queryJson);
        }
        /// <summary>
        /// ����sql��ѯ����
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetInfoBySql(string sql)
        {
            return service.GetInfoBySql(sql);
        }
        public int ExecuteBySql(string sql)
        {
            return service.ExecuteBySql(sql);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<MatrixsafecheckEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public MatrixsafecheckEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        public MatrixsafecheckEntity SetFormJson(string keyValue, string recid)
        {
            return service.SetFormJson(keyValue, recid);
        }

        public DataTable GetContentPageJson(string queryJson)
        {
            return service.GetContentPageJson(queryJson);
        }

        public DataTable GetDeptPageJson(string queryJson)
        {
            return service.GetDeptPageJson(queryJson);
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
        public void SaveForm(string keyValue, MatrixsafecheckEntity entity)
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
