using ERCHTMS.Entity.CarManage;
using ERCHTMS.IService.CarManage;
using ERCHTMS.Service.CarManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using ERCHTMS.Entity.SystemManage.ViewModel;

namespace ERCHTMS.Busines.CarManage
{
    /// <summary>
    /// �� ��������·��������
    /// </summary>
    public class RouteconfigBLL
    {
        private RouteconfigIService service = new RouteconfigService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<RouteconfigEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }

        /// <summary>
        /// ��ȡ���ڵ������
        /// </summary>
        /// <returns></returns>
        public List<RouteconfigEntity> GetTree(int type)
        {
            return service.GetTree(type);
        }

        /// <summary>
        /// ��ȡ����·��
        /// </summary>
        /// <returns></returns>
        public List<Route> GetRoute()
        {
            return service.GetRoute();
        }

        /// <summary>
        /// ��ȡ�ݷó������ڵ�ID
        /// </summary>
        /// <returns></returns>
        public string GetVisitParentid()
        {
            return service.GetVisitParentid();
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns></returns> 
        public IEnumerable<DataItemModel> GetWlList()
        {
            return service.GetWlList();
        }

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public RouteconfigEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }


        /// <summary>
        /// ��ȡ·����������
        /// </summary>
        /// <returns></returns>
        public List<RouteconfigEntity> RouteDropdown()
        {
            return service.RouteDropdown();
        }
        #endregion

        #region �ύ����

        public void SelectLine(string ID)
        {
            service.SelectLine(ID);
        }

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
        public void SaveForm(string keyValue, RouteconfigEntity entity)
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
        /// �����������
        /// </summary>
        /// <param name="rlist"></param>
        public void SaveList(List<RouteconfigEntity> rlist)
        {
            try
            {
                service.SaveList(rlist);
            }
            catch (Exception e)
            {
                throw ;
            }
        }

        #endregion
    }
}
