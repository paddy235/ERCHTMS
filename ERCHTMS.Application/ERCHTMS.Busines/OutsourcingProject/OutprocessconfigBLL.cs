using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using ERCHTMS.Service.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.OutsourcingProject
{
    /// <summary>
    /// �� ��������������ñ�
    /// </summary>
    public class OutprocessconfigBLL
    {
        private OutprocessconfigIService service = new OutprocessconfigService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<OutprocessconfigEntity> GetList()
        {
            return service.GetList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public OutprocessconfigEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// ��ȡ��ҳ����
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetPageListJson(Pagination pagination, string queryJson) {
            return service.GetPageListJson(pagination, queryJson);
        }
        /// <summary>
        /// �жϸõ糧�Ƿ���ڸ�ģ�������
        /// </summary>
        /// <param name="deptid">�糧ID</param>
        /// <param name="moduleCode">ģ��Code</param>
        /// <returns>0:������ >0 ����</returns>
        public int IsExistByModuleCode(string deptid, string moduleCode)
        {
            return service.IsExistByModuleCode(deptid, moduleCode);
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
        public void SaveForm(string keyValue, OutprocessconfigEntity entity)
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

        public void DeleteLinkData(string recid)
        {
            try
            {
                service.DeleteLinkData(recid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
