using ERCHTMS.Entity.PersonManage;
using ERCHTMS.IService.PersonManage;
using ERCHTMS.Service.PersonManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.PersonManage
{
    /// <summary>
    /// �� ������Ա����
    /// </summary>
    public class UserScoreBLL
    {
        private UserScoreIService service = new UserScoreService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<UserScoreEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public DataTable GetList(string userId)
        {
            return service.GetList(userId);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public UserScoreEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
         /// <summary>
        /// �洢���̷�ҳ��ѯ
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetPageJsonList(Pagination pagination, string queryJson)
        {
            return service.GetPageJsonList(pagination, queryJson);
        }
        /// <summary>
        /// ��ȡ��Ա���ֿ�����ϸ
        /// </summary>
        /// <param name="keyValue">��¼Id</param>
        /// <returns></returns>
        public object GetInfo(string keyValue)
        {
            return service.GetInfo(keyValue);
        }
         /// <summary>
        /// ��ȡ�û�ָ����ݵĻ���
        /// </summary>
        /// <param name="userId">�û�Id</param>
        /// <param name="year">���</param>
        /// <returns></returns>
        public decimal GetUserScore(string userId,string year)
        {
            return service.GetUserScore(userId, year);
        }
         /// <summary>
        /// ��ȡ��Ա����׻��ֺ��ۼƻ���
        /// </summary>
        /// <param name="userId">�û�Id</param>
        /// <returns></returns>
        public string GetScoreInfo(string userId)
        {
            return service.GetScoreInfo(userId);
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
        public void SaveForm(string keyValue, UserScoreEntity entity)
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
        /// ����������ֿ��˼�¼
        /// </summary>
        /// <param name="list"></param>
        public void Save(List<UserScoreEntity> list)
        {
            service.Save(list);
        }
        #endregion
    }
}
