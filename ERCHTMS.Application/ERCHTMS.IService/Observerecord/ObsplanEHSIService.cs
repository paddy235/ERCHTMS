using ERCHTMS.Entity.Observerecord;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Code;

namespace ERCHTMS.IService.Observerecord
{
    /// <summary>
    /// �� �����۲�ƻ�
    /// </summary>
    public interface ObsplanEHSIService
    {
        #region ��ȡ����
      
        ObsplanEHSEntity GetEntity(string keyValue);
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        void SaveForm(string keyValue, ObsplanEHSEntity entity);
        #endregion

     
    }
}
