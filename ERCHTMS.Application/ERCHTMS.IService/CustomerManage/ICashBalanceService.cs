using ERCHTMS.Entity.CustomerManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;

namespace ERCHTMS.IService.CustomerManage
{
    /// <summary>
    /// �� �����ֽ����
    /// </summary>
    public interface ICashBalanceService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<CashBalanceEntity> GetList(string queryJson);
        #endregion

        #region �ύ����
        /// <summary>
        /// �����֧���
        /// </summary>
        /// <param name="db"></param>
        /// <param name="cashBalanceEntity"></param>
        void AddBalance(IRepository db, CashBalanceEntity cashBalanceEntity);
        #endregion
    }
}
