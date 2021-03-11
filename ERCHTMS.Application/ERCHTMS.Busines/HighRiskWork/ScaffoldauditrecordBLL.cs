using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using ERCHTMS.Service.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Linq.Expressions;
using BSFramework.Util.Extension;

namespace ERCHTMS.Busines.HighRiskWork
{
    /// <summary>
    /// �� ����1.���ּ���˼�¼��
    /// </summary>
    public class ScaffoldauditrecordBLL
    {
        private ScaffoldauditrecordIService service = new ScaffoldauditrecordService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="scaffoldid">��ѯ����</param>
        /// <returns>�����б�</returns>
        public List<ScaffoldauditrecordEntity> GetList(string scaffoldid)
        {
            return service.GetList(scaffoldid);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public ScaffoldauditrecordEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        public List<ScaffoldauditrecordEntity> GetApplyAuditList(string keyValue, int AuditType)
        {
            return service.GetApplyAuditList(keyValue, AuditType);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="scaffoldid">���ּ���ϢID</param>
        /// <param name="departname">������</param>
        /// <param name="rolename">��ɫ��</param>
        /// <returns></returns>
        public ScaffoldauditrecordEntity GetEntity(string scaffoldid, string departname, string rolename)
        {
            return service.GetEntity(scaffoldid, departname, rolename);
        }

          /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="scaffoldid">���ּ���ϢID</param>
        /// <param name="deppartcode">����</param>
        /// <returns></returns>
        public IEnumerable<ScaffoldauditrecordEntity> GetEntitys(string scaffoldid, string departcode)
        {
            return service.GetEntitys(scaffoldid,departcode);
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
        public void SaveForm(string keyValue, ScaffoldauditrecordEntity entity)
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
