using ERCHTMS.Entity.KbsDeviceManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;

namespace ERCHTMS.IService.KbsDeviceManage
{
    /// <summary>
    /// �� ������λ���¼��
    /// </summary>
    public interface LocationIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<LocationEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        LocationEntity GetEntity(string keyValue);
        /// <summary>
        /// ����ʱ��λ�ȡ��λ��¼
        /// </summary>
        /// <param name="lableid"></param>
        /// <param name="st"></param>
        /// <param name="et"></param>
        /// <returns></returns>
        List<LocationEntity> GetLocation(string lableid, DateTime st, DateTime et);
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
        void SaveForm(string keyValue, LocationEntity entity);


        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="entityList"></param>
        bool Insert(List<LocationEntity> entityList);

        #endregion
    }
}
