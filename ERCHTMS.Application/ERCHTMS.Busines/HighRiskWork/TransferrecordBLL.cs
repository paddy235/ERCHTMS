using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using ERCHTMS.Service.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace ERCHTMS.Busines.HighRiskWork
{
    /// <summary>
    /// �� ����ת����¼��
    /// </summary>
    public class TransferrecordBLL
    {
        private TransferrecordIService service = new TransferrecordService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="condition">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<TransferrecordEntity> GetList(Expression<Func<TransferrecordEntity, bool>> condition)
        {
            return service.GetList(condition);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public TransferrecordEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, TransferrecordEntity entity)
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
        /// �����(�Ƚ����ҵ��ID�����̽ڵ�ID����������ʧЧ�������ڲ���һ������ת��������)
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void SaveRealForm(string keyValue, TransferrecordEntity entity)
        {
            try
            {
                TransferrecordEntity realentity = service.GetList(t => t.RecId == entity.RecId && t.FlowId == entity.FlowId && t.Disable == 0).FirstOrDefault();
                if (realentity != null)
                {
                    realentity.InTransferUserAccount += (entity.InTransferUserAccount + ",");
                    realentity.InTransferUserId += (entity.InTransferUserId + ",");
                    realentity.InTransferUserName += (entity.InTransferUserName + ",");
                    //��ʵ��ת���������д��ڵ�ǰ������ʱ��ת���������޳���ǰ����ת�����˺�
                    realentity.InTransferUserAccount = realentity.InTransferUserAccount.Contains(entity.OutTransferUserAccount) ? realentity.InTransferUserAccount.Replace(entity.OutTransferUserAccount + ",", "") : realentity.InTransferUserAccount;
                    realentity.InTransferUserId = realentity.InTransferUserId.Contains(entity.OutTransferUserId) ? realentity.InTransferUserId.Replace(entity.OutTransferUserId + ",", "") : realentity.InTransferUserId;
                    realentity.InTransferUserName = realentity.InTransferUserName.Contains(entity.OutTransferUserName) ? realentity.InTransferUserName.Replace(entity.OutTransferUserName + ",", "") : realentity.InTransferUserName;
                    realentity.OutTransferUserAccount += (entity.OutTransferUserAccount + ",");
                    realentity.OutTransferUserId += (entity.OutTransferUserId + ",");
                    realentity.OutTransferUserName += (entity.OutTransferUserName + ",");

                    //����ǰת�������˴�����ʵ��ת��������ʱ(����ǰ��ǰת����������ǰ����ת������),��ʵ��ת���������е�����˺��޳�
                    string[] InTransferUserAccountList = entity.InTransferUserAccount.Split(',');
                    foreach (var item in InTransferUserAccountList)
                    {
                        if (!string.IsNullOrWhiteSpace(item) && realentity.OutTransferUserAccount.Contains(item + ","))
                        {
                            realentity.OutTransferUserAccount = realentity.OutTransferUserAccount.Replace(item + ",", "");
                        }
                    }

                    string[] InTransferUserIdList = entity.InTransferUserId.Split(',');
                    foreach (var item in InTransferUserIdList)
                    {
                        if (!string.IsNullOrWhiteSpace(item) && realentity.OutTransferUserId.Contains(item + ","))
                        {
                            realentity.OutTransferUserId = realentity.OutTransferUserId.Replace(item + ",", "");
                        }
                    }

                    string[] InTransferUserNameList = entity.InTransferUserName.Split(',');
                    foreach (var item in InTransferUserNameList)
                    {
                        if (!string.IsNullOrWhiteSpace(item) && realentity.OutTransferUserName.Contains(item + ","))
                        {
                            realentity.OutTransferUserName = realentity.OutTransferUserName.Replace(item + ",", "");
                        }
                    }

                    //��ת�������˴�����ת��������ʱ����ת���������е�����˺��޳�
                    string[] OutTransferUserAccountList = realentity.OutTransferUserAccount.Split(',');
                    foreach (var item in OutTransferUserAccountList)
                    {
                        if (!string.IsNullOrWhiteSpace(item) && realentity.InTransferUserAccount.Contains(item + ","))
                        {
                            realentity.InTransferUserAccount = realentity.InTransferUserAccount.Replace(item + ",", "");
                        }
                    }

                    string[] OutTransferUserIdList = realentity.OutTransferUserId.Split(',');
                    foreach (var item in OutTransferUserIdList)
                    {
                        if (!string.IsNullOrWhiteSpace(item) && realentity.InTransferUserId.Contains(item + ","))
                        {
                            realentity.InTransferUserId = realentity.InTransferUserId.Replace(item + ",", "");
                        }
                    }

                    string[] OutTransferUserNameList = realentity.OutTransferUserName.Split(',');
                    foreach (var item in OutTransferUserNameList)
                    {
                        if (!string.IsNullOrWhiteSpace(item) && realentity.InTransferUserName.Contains(item + ","))
                        {
                            realentity.InTransferUserName = realentity.InTransferUserName.Replace(item + ",", "");
                        }
                    }
                    
                    service.SaveForm(realentity.Id, realentity);
                    entity.Disable = 1;
                    service.SaveForm(keyValue, entity);
                }
                else
                {
                    entity.Disable = 1;
                    entity.OutTransferUserAccount += ",";
                    entity.OutTransferUserId += ",";
                    entity.OutTransferUserName += ",";
                    entity.InTransferUserAccount += ",";
                    entity.InTransferUserId += ",";
                    entity.InTransferUserName += ",";
                    service.SaveForm(keyValue, entity);
                    entity.Disable = 0;
                    entity.Id = "";
                    service.SaveForm(keyValue, entity);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        #endregion
    }
}
