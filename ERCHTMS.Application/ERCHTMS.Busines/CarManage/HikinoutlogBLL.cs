using ERCHTMS.Entity.CarManage;
using ERCHTMS.IService.CarManage;
using ERCHTMS.Service.CarManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Busines.CarManage
{
    /// <summary>
    /// �� �����豸��¼��Ա������־
    /// </summary>
    public class HikinoutlogBLL
    {
        private HikinoutlogIService service = new HikinoutlogService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<HikinoutlogEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }

        /// <summary>
        /// �û��б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }

        /// <summary>
        /// ��ȡ����ȫ������
        /// </summary>
        /// <returns></returns>
        public DataTable GetNums()
        {
            return service.GetNums();
        }

        /// <summary>
        /// ��ȡ�������һ��ˢ��
        /// </summary>
        /// <returns></returns>
        public HikinoutlogEntity GetLastInoutLog()
        {
            return service.GetLastInoutLog();
        }

        /// <summary>
        /// ԰������ͳ��
        /// </summary>
        /// <returns></returns>
        public DataTable GetCarStatistic()
        {
            return service.GetCarStatistic();
        }


        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public HikinoutlogEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// �����û�id��ѯ���û�����δ������¼
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public HikinoutlogEntity GetInUser(string UserId)
        {
            return service.GetInUser(UserId);
        }

        /// <summary>
        /// �����豸ID
        /// </summary>
        /// <param name="HikId"></param>
        /// <returns></returns>
        public HikinoutlogEntity DeviceGetLog(string HikId)
        {
            return service.DeviceGetLog(HikId);
        }

        /// <summary>
        /// ��ȡ����һ����������
        /// </summary>
        /// <returns></returns>
        public List<AreaModel> GetAccPersonNum()
        {
            return service.GetAccPersonNum();
        }

        /// <summary>
        /// ��ȡ���ڵ�Code��ȡ���������ӽڵ�
        /// </summary>
        /// <returns></returns>
        public List<DistrictEntity> GetAreaSon(string code)
        {
            return service.GetAreaSon(code);
        }

        /// <summary>
        /// ��ȡ�����ŵ���Աͳ������
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetAllDepartment(string queryJson)
        {
            return service.GetAllDepartment(queryJson);
        }

        /// <summary>
        /// ���ݲ������Ƽ�����Ա����
        /// </summary>
        /// <param name="deptName"></param>
        /// <returns></returns>
        public DataTable GetTableByDeptname(Pagination pagination, string deptName, string personName)
        {
            return service.GetTableByDeptname(pagination, deptName, personName);
        }


        /// <summary>
        /// ��ȡ��Ա���ñ�����
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="ModuleType"></param>
        /// <returns></returns>
        public DataTable GetPersonSet(Pagination pagination, string ModuleType)
        {
            return service.GetPersonSet(pagination, ModuleType);
        }

        /// <summary>
        /// ��ȡ������������
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable Get_BIS_CARVIOLATION(Pagination pagination, string queryJson)
        {
            return service.Get_BIS_CARVIOLATION(pagination, queryJson);
        }
        #endregion

        #region �ύ����

        /// <summary>
        /// ��Աͨ���ύ
        /// </summary>
        /// <param name="insert"></param>
        /// <param name="update"></param>
        public void UserAisleSave(HikinoutlogEntity insert, HikinoutlogEntity update)
        {
            service.UserAisleSave(insert, update);
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
        public void SaveForm(string keyValue, HikinoutlogEntity entity)
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

        public List<int[]> GetPersonData()
        {
            return service.GetPersonData();
        }

        public int[] GetAreaData()
        {
            return service.GetAreaData();
        }
        /// <summary>
        /// ��ȡ�豸���ص�һ������
        /// </summary>
        /// <returns></returns>
        public HikinoutlogEntity GetFirsetData()
        {
            return service.GetFirsetData();
        }

        /// <summary>
        /// ����hikid�豸��ID��ȡ��Ա������ǰ��������
        /// </summary>
        /// <param name="hikId">�豸��Id</param>
        /// <returns></returns>
        public System.Collections.IList GetTopFiveById(string hikId)
        {
            return service.GetTopFiveById(hikId);
        }

        /// <summary>
        /// ��ȡ�������Ա��������
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, int> GetTodayCarPeopleCount()
        {
            return service.GetTodayCarPeopleCount();
        }

        /// <summary>
        /// ��ȡ���µĳ�����Ա�������ݣ�ȡǰ����
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetCarPeopleTopData()
        {
            return service.GetCarPeopleTopData();
        }

        /// <summary>
        /// �����Ž����豸��Ż�ȡ��ر��
        /// </summary>
        /// <param name="DoorIndexCode">�Ž����豸���</param>
        /// <returns></returns>
        public string GetCameraIndexCodeByDoorIndexCode(string DoorIndexCode)
        {
            return service.GetCameraIndexCodeByDoorIndexCode(DoorIndexCode);
        }

        /// <summary>
        /// ���ڸ澯
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetAttendanceWarningPageList(Pagination pagination, string queryJson)
        {
            return service.GetAttendanceWarningPageList(pagination, queryJson);
        }

        /// <summary>
        /// ����ȱ��ͳ��
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetAbsenteeismPageList(Pagination pagination, string queryJson)
        {
            return service.GetAbsenteeismPageList(pagination, queryJson);
        }

        /// <summary>
        /// ����ȱ��ͳ��
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetAbsenteeismPersonSet(Pagination pagination, string queryJson)
        {
            return service.GetAbsenteeismPersonSet(pagination, queryJson);
        }

        /// <summary>
        /// ��������������Ա���ɲ�ѯ�Ž�
        /// </summary>
        /// <param name="json"></param>
        /// <param name="ModuleType"></param>
        public void SaveAbsenteeismPersonSet(string json, string ModuleType)
        {
            try
            {
                service.SaveAbsenteeismPersonSet(json, ModuleType);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ������Ա����IDɾ������
        /// </summary>
        /// <param name="keyValue"></param>
        public void DeleteAbsenteeismPersonSet(string keyValue)
        {
            try
            {
                service.DeleteAbsenteeismPersonSet(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ��ȡ���ڸ澯��Ա���ñ�����
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetAttendanceWarningPersonSet(Pagination pagination, string queryJson)
        {
            return service.GetAttendanceWarningPersonSet(pagination, queryJson);
        }

        /// <summary>
        /// ��������������Ա���ɲ�ѯ�Ž�
        /// </summary>
        /// <param name="json"></param>
        /// <param name="ModuleType"></param>
        public void SaveAttendanceWarningPersonSet(string json, string ModuleType)
        {
            try
            {
                service.SaveAttendanceWarningPersonSet(json, ModuleType);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ������Ա����IDɾ������
        /// </summary>
        /// <param name="keyValue"></param>
        public void DeleteAttendanceWarningPersonSet(string keyValue)
        {
            try
            {
                service.DeleteAttendanceWarningPersonSet(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// �����û�ID�޸��볡״̬
        /// </summary>
        /// <param name="keyValue"></param>
        public void UpdateByID(string keyValue)
        {
            service.UpdateByID(keyValue);
        }

        /// <summary>
        /// ��ȡ������Ա�Ž�����ͳ��
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetTableUserRole(Pagination pagination, string queryJson)
        {
            return service.GetTableUserRole(pagination, queryJson);
        }

        /// <summary>
        /// ��ȡ��Ա�Ž�������ϸ
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetTableByUserID(Pagination pagination, string queryJson)
        {
            return service.GetTableByUserID(pagination, queryJson);
        }

        /// <summary>
        /// ��������������Ա���ɲ�ѯ�Ž�
        /// </summary>
        /// <param name="json"></param>
        /// <param name="ModuleType"></param>
        public void SavePersonSet(string json, string ModuleType)
        {
            try
            {
                service.SavePersonSet(json, ModuleType);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ������Ա����IDɾ������
        /// </summary>
        /// <param name="keyValue"></param>
        public void DeletePersonSet(string keyValue)
        {
            try
            {
                service.DeletePersonSet(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
