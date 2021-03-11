using ERCHTMS.Entity.CarManage;
using ERCHTMS.IService.CarManage;
using ERCHTMS.Service.CarManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Entity.MatterManage;

namespace ERCHTMS.Busines.CarManage
{
    /// <summary>
    /// �� �����ݷó�����
    /// </summary>
    public class VisitcarBLL
    {
        private VisitcarIService service = new VisitcarService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<VisitcarEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public VisitcarEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// ���ݳ��ƺŻ�ȡ������Ϣ
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public VisitcarEntity GetCar(string CarNo)
        {
            return service.GetCar(CarNo);
        }

        /// <summary>
        /// ���ݳ��ƺŻ�ȡ�˳��ƽ������°ݷ���Ϣ
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public VisitcarEntity NewGetCar(string CarNo)
        {
            return service.NewGetCar(CarNo);
        }

        /// <summary>
        /// ��ȡ�Ÿڲ�ѯ�����ϼ��ݷ���Ϣ
        /// </summary>
        /// <returns></returns>
        public DataTable GetDoorList()
        {
            return service.GetDoorList();
        }

        /// <summary>
        /// ��ҳ��ѯ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }

        /// <summary>
        /// ��õ���������������
        /// </summary>
        /// <returns></returns>
        public List<string> GetOutCarNum()
        {
            return service.GetOutCarNum();
        }

        /// <summary>
        /// ��ѯ�Ƿ����ظ����ƺŰݷó���/Σ��Ʒ����
        /// </summary>
        /// <param name="CarNo">���ƺ�</param>
        /// <param name="type">3λ�ݷ� 5ΪΣ��Ʒ</param>
        /// <returns></returns>
        public bool GetVisitCf(string CarNo, int type)
        {
            return service.GetVisitCf(CarNo, type);
        }

        /// <summary>
        /// ��ʼ���ݷ�\Σ��Ʒ\���ϳ���
        /// </summary>
        /// <returns></returns>
        public List<CarAlgorithmEntity> IniVHOCar()
        {
            return service.IniVHOCar();
        }

        #endregion

        #region �ύ����

        /// <summary>
        /// ����ID�ı���ѡ·��
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="LineName"></param>
        /// <param name="LineID"></param>
        public void ChangeLine(string keyValue, string LineName, string LineID)
        {
            service.ChangeLine(keyValue, LineName, LineID);
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
        public void SaveForm(string keyValue, VisitcarEntity entity)
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
        /// �����������Ա������ͼƬ
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveFaceUserForm(string keyValue, VisitcarEntity entity, List<CarUserFileImgEntity> userjson)
        {
            try
            {
                service.SaveFaceUserForm(keyValue, entity, userjson);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// �ı�GPS����Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <param name="pgpslist"></param>
        public void ChangeGps(string keyValue, VisitcarEntity entity, List<PersongpsEntity> pgpslist)
        {
            service.ChangeGps(keyValue, entity, pgpslist);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="Note"></param>
        /// <param name="type"></param>
        public void CarOut(string keyValue, string Note, int type, List<PersongpsEntity> pergps)
        {
            service.CarOut(keyValue, Note, type, pergps);
        }

        /// <summary>
        /// �ı�GPS����Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <param name="pgpslist"></param>
        public void WlChangeGps(string keyValue, OperticketmanagerEntity entity)
        {
            service.WlChangeGps(keyValue, entity);
        }

        #endregion
    }
}
