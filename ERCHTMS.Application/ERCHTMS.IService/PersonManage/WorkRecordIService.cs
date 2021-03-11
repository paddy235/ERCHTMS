using System;
using ERCHTMS.Entity.PersonManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.IService.PersonManage
{
    /// <summary>
    /// �� ������Ա֤��
    /// </summary>
    public interface WorkRecordIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ��Ա�Ĺ�������
        /// </summary>
        /// <param name="userId">�û�Id</param>
        /// <returns>�����б�</returns>
        IEnumerable<WorkRecordEntity> GetList(string userId);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        WorkRecordEntity GetEntity(string keyValue);
 
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        void SaveForm(string keyValue, WorkRecordEntity entity);

        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        void NewSaveForm(string keyValue, WorkRecordEntity entity);
          /// <summary>
        /// ��Ա�볡ʱд������¼
        /// </summary>
        /// <param name="userId">�û�Id</param>
        /// <param name="deptId">����Id</param>
        /// <returns></returns>
        int WriteWorkRecord(UserInfoEntity user, ERCHTMS.Code.Operator currUser);

        /// <summary>
        /// ��Ա����������ʱд��¼
        /// </summary>
        /// <param name="userId">�û�Id</param>
        /// <param name="deptId">����Id</param>
        /// <returns></returns>
        int WriteChangeRecord(UserInfoEntity user, ERCHTMS.Code.Operator currUser);

        /// <summary>
        /// �޸��볡ԭ�����޸�����Ĺ�����¼����ʱ��
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        int EidtRecord(string userid, string time);

        #endregion

    }
}
