using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using ERCHTMS.Service.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Busines.HighRiskWork
{
    /// <summary>
    /// �� ������Σ����ҵ���/������
    /// </summary>
    public class HighRiskCheckBLL
    {
        private HighRiskCheckIService service = new HighRiskCheckService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<HighRiskCheckEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public HighRiskCheckEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }


        /// <summary>
        /// ��������id��ȡ�����Ϣ[������û���]
        /// </summary>
        /// <param name="approveid"></param>
        /// <returns></returns>
        public IEnumerable<HighRiskCheckEntity> GetCheckListInfo(string approveid) 
        {
            return service.GetCheckListInfo(approveid);
        }

        /// <summary>
        /// ��������id��ȡ������Ϣ[������û���]
        /// </summary>
        /// <param name="approveid"></param>
        /// <returns></returns>
        public HighRiskCheckEntity GetApproveInfo(string approveid)
        {
            return service.GetApproveInfo(approveid);
        }

        /// <summary>
        /// ��������id��ȡû��˵�����
        /// </summary>
        /// <param name="approveid"></param>
        /// <returns></returns>
        public int GetNoCheckNum(string approveid)
        {
            return service.GetNoCheckNum(approveid);
        }

          /// <summary>
        /// ��������id�͵�ǰ��¼�˻�ȡ���(��)��¼
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="chapterid"></param>
        /// <returns></returns>
        public HighRiskCheckEntity GetNeedCheck(string approveid)
        {
            return service.GetNeedCheck(approveid);
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
        /// ���������idɾ������
        /// </summary>
        public int Remove(string workid)
        {
            try
            {
                service.Remove(workid);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, HighRiskCheckEntity entity)
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
