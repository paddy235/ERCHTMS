using ERCHTMS.Entity.CarManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using ERCHTMS.Entity.SystemManage.ViewModel;

namespace ERCHTMS.IService.CarManage
{
    /// <summary>
    /// �� ��������·��������
    /// </summary>
    public interface RouteconfigIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<RouteconfigEntity> GetList(string queryJson);

        /// <summary>
        /// ��ȡ���ڵ������
        /// </summary>
        /// <returns></returns>
        List<RouteconfigEntity> GetTree(int type);

        /// <summary>
        /// ��ȡ����·��
        /// </summary>
        /// <returns></returns>
        List<Route> GetRoute();

        /// <summary>
        /// ��ȡ�ݷ�·�߸��ڵ�
        /// </summary>
        /// <returns></returns>
        string GetVisitParentid();

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        RouteconfigEntity GetEntity(string keyValue);

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        IEnumerable<DataItemModel> GetWlList();


        /// <summary>
        /// ��ȡ·����������
        /// </summary>
        /// <returns></returns>
        List<RouteconfigEntity> RouteDropdown();
        #endregion

        #region �ύ����
        /// <summary>
        /// ѡ��·��
        /// </summary>
        /// <param name="ID"></param>
        void SelectLine(string ID);
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
        void SaveForm(string keyValue, RouteconfigEntity entity);

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="rlist"></param>
        void SaveList(List<RouteconfigEntity> rlist);

        #endregion
    }
}
