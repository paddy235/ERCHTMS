using ERCHTMS.Entity.PersonManage;
using ERCHTMS.IService.PersonManage;
using ERCHTMS.Service.PersonManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Busines.PersonManage
{
    /// <summary>
    /// �� ������Ա֤��
    /// </summary>
    public class WorkRecordBLL
    {
        private WorkRecordIService service = new WorkRecordService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="userId">�û�Id</param>
        /// <returns>�����б�</returns>
        public IEnumerable<WorkRecordEntity> GetList(string userId)
        {
            return service.GetList(userId);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public WorkRecordEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, WorkRecordEntity entity)
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
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void NewSaveForm(string keyValue, WorkRecordEntity entity)
        {
            try
            {
                service.NewSaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ��Ա�볡ʱд������¼
        /// </summary>
        /// <param name="userId">�û�Id</param>
        /// <param name="deptId">����Id</param>
        /// <returns></returns>
        public int WriteWorkRecord(UserInfoEntity user, ERCHTMS.Code.Operator currUser)
        {
            return service.WriteWorkRecord(user, currUser);
        }

        /// <summary>
        /// ��Ա����������ʱд��¼
        /// </summary>
        /// <param name="userId">�û�Id</param>
        /// <param name="deptId">����Id</param>
        /// <returns></returns>
        public int WriteChangeRecord(UserInfoEntity user, ERCHTMS.Code.Operator currUser)
        {
            return service.WriteChangeRecord(user, currUser);
        }

        /// <summary>
        /// �޸��볡ԭ�����޸�����Ĺ�����¼����ʱ��
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public int EditRecord(string userid, string time)
        {
            return service.EidtRecord(userid, time);
        }

        #endregion

      
    }
}
