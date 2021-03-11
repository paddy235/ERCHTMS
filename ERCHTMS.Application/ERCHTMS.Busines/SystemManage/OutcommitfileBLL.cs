using ERCHTMS.Entity.SystemManage;
using ERCHTMS.IService.SystemManage;
using ERCHTMS.Service.SystemManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.SystemManage
{
    /// <summary>
    /// �� ��������糧�ύ����˵����
    /// </summary>
    public class OutcommitfileBLL
    {
        private OutcommitfileIService service = new OutcommitfileService();

        #region ��ȡ����


        public DataTable GetPageList(Pagination pagination, string queryJson) {
            return service.GetPageList(pagination, queryJson);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<OutcommitfileEntity> GetList()
        {
            return service.GetList();
        }
        /// <summary>
        /// ���ݻ���Code��ѯ�������Ƿ��Ѿ����
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public bool GetIsExist(string orgCode){
            return service.GetIsExist(orgCode);
        }

        /// <summary>
        /// ���ݻ���Code��ѯ��������ӵ���Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public OutcommitfileEntity GetEntityByOrgCode(string orgCode)
        {
            return service.GetEntityByOrgCode(orgCode);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public OutcommitfileEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, OutcommitfileEntity entity)
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
