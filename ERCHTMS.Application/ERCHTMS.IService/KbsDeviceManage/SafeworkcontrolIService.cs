using ERCHTMS.Entity.KbsDeviceManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Entity.RiskDatabase;

namespace ERCHTMS.IService.KbsDeviceManage
{
    /// <summary>
    /// �� ������ҵ�ֳ���ȫ�ܿ� 
    /// </summary>
    public interface SafeworkcontrolIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<SafeworkcontrolEntity> GetList(string queryJson);
        /// <summary>
        /// ����״̬��ȡ�ֳ���ҵ��Ϣ
        /// </summary>
        /// <param name="State">1��ʼ 2����</param>
        /// <returns></returns>
        IEnumerable<SafeworkcontrolEntity> GetStartList(int State);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        SafeworkcontrolEntity GetEntity(string keyValue);
        /// <summary>
        /// ��ȡԤ���б�����
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        List<WarningInfoEntity> GetWarningInfoList(int type);
        /// <summary>
        /// ��ȡ����Ԥ����Ϣ�б�
        /// </summary>
        /// <returns></returns>
        List<WarningInfoEntity> GetWarningAllList();
        /// <summary>
        /// ��ȡ��Ա��ȫ�ܿظ���ʱ������
        /// </summary>
        /// <returns></returns>
        List<KbsEntity> GetDayTimeIntervalUserNum();
        /// <summary>
        /// �����б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// ��ȡ�����е�ǰ��ʼ����ҵ
        /// </summary>
        /// <returns></returns>
        List<SafeworkcontrolEntity> GetNowWork();

        /// <summary>
        /// ��ȡ���ո߷�����ҵ
        /// </summary>
        /// <returns></returns>    
        List<SafeworkcontrolEntity> GetDangerWorkToday(string level);
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
        void SaveForm(string keyValue, SafeworkcontrolEntity entity);

        /// <summary>
        /// ������ҵ��Ա�Ƿ���������״̬
        /// </summary>
        /// <param name="workid"></param>
        /// <param name="userid"></param>
        /// <param name="state"></param>
        void SaveSafeworkUserStateIofo(string workid, string userid, int state);

        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        void AppSaveForm(string keyValue, SafeworkcontrolEntity entity);
        /// <summary>
        /// ��ȡԤ��ʵ��
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        WarningInfoEntity GetWarningInfoEntity(string keyValue);
        /// <summary>
        /// ����Ԥ�����������޸ģ�
        /// </summary>
        /// <param name="type">0���� 1�޸�</param>
        /// <param name="list"></param>
        void SaveWarningInfoForm(int type, IList<WarningInfoEntity> list);
        /// <summary>
        /// ����Ԥ�����������޸ģ�
        /// </summary>
        /// <param name="keyValue">����</param>
        /// <param name="entity"></param>
        void SaveWarningInfoForm(string keyValue, WarningInfoEntity entity);
        /// <summary>
        /// ɾ��Ԥ��
        /// </summary>
        /// <param name="keyValue">����</param>
        void DelWarningInForm(string keyValue);
        /// <summary>
        /// ����ɾ��Ԥ����Ϣ��ͨ�������¼Id��
        /// </summary>
        /// <param name="BaseId"></param>
        void DelBatchWarningInForm(string BaseId);

        #endregion

        List<WarningInfoEntity> GetBatchWarningInfoList(string workid);
        List<RiskAssessEntity> GetDistrictLevel();
    }
}
