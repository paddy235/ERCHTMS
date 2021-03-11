using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using ERCHTMS.Service.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Busines.HighRiskWork
{
    /// <summary>
    /// �� ������վ�ල��ҵ��Ϣ
    /// </summary>
    public class SuperviseWorkInfoBLL
    {
        private SuperviseWorkInfoIService service = new SuperviseWorkInfoService();

        #region ��ȡ����
        /// <summary>
        /// ���ݷ�������id��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SuperviseWorkInfoEntity> GetList(string strwhere)
        {
            return service.GetList(strwhere);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SuperviseWorkInfoEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

         /// <summary>
        /// ���ݷ�������id�Ͱ���id��ȡ��ҵ��Ϣ
        /// </summary>
        /// <param name="taskshareid"></param>
        /// <returns></returns>
        public IEnumerable<SuperviseWorkInfoEntity> GetList(string taskshareid, string teamid)
        {
            return service.GetList(taskshareid,teamid);
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
        public void SaveForm(string keyValue, SuperviseWorkInfoEntity entity)
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

                /// <summary>
        ///���ݷ���idɾ����ҵ��Ϣ
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveWorkByTaskShareId(string keyValue)
        {
            try
            {
                service.RemoveWorkByTaskShareId(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
