using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using ERCHTMS.Service.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Code;

namespace ERCHTMS.Busines.OutsourcingProject
{
    /// <summary>
    /// �� ���������λ��������
    /// </summary>
    public class OutprojectblacklistBLL
    {
        private OutprojectblacklistIService service = new OutprojectblacklistService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<OutprojectblacklistEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public OutprojectblacklistEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        public DataTable GetPageBlackListJson(Pagination pagination, string queryJson)
        {
            return service.GetPageBlackListJson(pagination, queryJson);
        }
        /// <summary>
        /// ���󣨺ˣ�����λ������顢���󣨺ˣ�����Ա������顢���󣨺ˣ����������������󣨺ˣ��������豸���ա����󣨺ˣ�����ȫ/�綯���������ա����󣨺ˣ����볧��ɡ����󣨺ˣ�����������
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> ToAuditOutPeoject(Operator user)
        {
            return service.ToAuditOutPeoject(user);
        }
        public List<int> ToIndexData(Operator user)
        {
            return service.ToIndexData(user);
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
        public void SaveForm(string keyValue, OutprojectblacklistEntity entity)
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
