using ERCHTMS.Entity.SafetyWorkSupervise;
using ERCHTMS.IService.SafetyWorkSupervise;
using ERCHTMS.Service.SafetyWorkSupervise;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.JPush;

namespace ERCHTMS.Busines.SafetyWorkSupervise
{
    /// <summary>
    /// �� ������ȫ�ص㹤�����췴����Ϣ
    /// </summary>
    public class SafetyworkfeedbackBLL
    {
        private SafetyworkfeedbackIService service = new SafetyworkfeedbackService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SafetyworkfeedbackEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SafetyworkfeedbackEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, SafetyworkfeedbackEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
                SafetyworksuperviseBLL announRes = new SafetyworksuperviseBLL();
                var sl = announRes.GetEntity(entity.SuperviseId);
                UserBLL userbll = new UserBLL();
                UserEntity userEntity = userbll.GetEntity(sl.SupervisePersonId);//��ȡ�������û���Ϣ
                JPushApi.PushMessage(userEntity.Account, sl.SupervisePerson, "GZDB002", "���а�ȫ����", sl.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
