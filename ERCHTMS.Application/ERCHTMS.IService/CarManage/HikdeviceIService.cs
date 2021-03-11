using ERCHTMS.Entity.CarManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.SystemManage;

namespace ERCHTMS.IService.CarManage
{
    /// <summary>
    /// �� �����Ž��豸����
    /// </summary>
    public interface HikdeviceIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<HikdeviceEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        HikdeviceEntity GetEntity(string keyValue);

        /// <summary>
        /// �û��б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        HikdeviceEntity GetDeviceEntity(string HikID);

        /// <summary>
        /// ��ȡ��ǰ�糧���е��Ž��豸����
        /// ���ý��ڱ��������  ϵͳ����->�����Ž��豸����
        /// </summary>
        /// <returns></returns>
         IEnumerable<DataItemEntity> GetDeviceArea();
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
        void SaveForm(string keyValue, HikdeviceEntity entity);
        List<HikdeviceEntity> GetDeviceByArea(string areaName);
        #endregion
    }
}
