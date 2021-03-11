using ERCHTMS.Entity.SafetyWorkSupervise;
using ERCHTMS.IService.SafetyWorkSupervise;
using ERCHTMS.Service.SafetyWorkSupervise;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.JPush;

namespace ERCHTMS.Busines.SafetyWorkSupervise
{
    /// <summary>
    /// �� ������ȫ�ص㹤�����췴����Ϣ
    /// </summary>
    public class SuperviseconfirmationBLL
    {
        private SuperviseconfirmationIService service = new SuperviseconfirmationService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SuperviseconfirmationEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SuperviseconfirmationEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, SuperviseconfirmationEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);

                SafetyworksuperviseBLL announRes = new SafetyworksuperviseBLL();
                var sl = announRes.GetEntity(entity.SuperviseId);
                UserBLL userbll = new UserBLL();
                //�ж��Ƕ�����ɻ����˻�
                if (entity.SuperviseResult == "1")//�˻�
                {
                    UserEntity userEntity = userbll.GetEntity(sl.DutyPersonId);//��ȡ�������û���Ϣ
                    JPushApi.PushMessage(userEntity.Account, sl.DutyPerson, "GZDB003", "���а�ȫ����", sl.Id);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
