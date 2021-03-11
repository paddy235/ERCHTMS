using ERCHTMS.Entity.CustomerManage;
using ERCHTMS.IService.CustomerManage;
using ERCHTMS.Service.CustomerManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Busines.CustomerManage
{
    /// <summary>
    /// �� �����ֽ����
    /// </summary>
    public class CashBalanceBLL
    {
        private ICashBalanceService service = new CashBalanceService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<CashBalanceEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        #endregion

        #region �ύ����
        #endregion
    }
}
