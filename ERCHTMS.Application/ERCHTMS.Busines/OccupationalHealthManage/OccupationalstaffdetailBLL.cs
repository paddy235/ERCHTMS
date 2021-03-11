using ERCHTMS.Entity.OccupationalHealthManage;
using ERCHTMS.IService.OccupationalHealthManage;
using ERCHTMS.Service.OccupationalHealthManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using System.Linq;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;

namespace ERCHTMS.Busines.OccupationalHealthManage
{
    /// <summary>
    /// �� ����ְҵ���������
    /// </summary>
    public class OccupationalstaffdetailBLL
    {
        private OccupationalstaffdetailIService service = new OccupationalstaffdetailService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<OccupationalstaffdetailEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡְҵ����Ա�嵥��Ա����
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public int GetStaffListSum(string queryJson, string where)
        {
            return service.GetStaffListSum(queryJson, where);
        }
        /// <summary>
        /// ��ȡ�µ�ְҵ��ͳ�Ʊ�
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable NewGetStatisticsUserTable(string year, string where)
        {
            return service.NewGetStatisticsUserTable(year, where);
        }
        /// <summary>
        /// ��ȡְҵ����Ա�嵥(ȫ����)
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetStaffList(string queryJson, string where)
        {
            return service.GetStaffList(queryJson, where);
        }
        /// <summary>
        /// ��ȡְҵ��ͳ�Ʊ�
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public DataTable GetStatisticsUserTable(string year, string where)
        {
            return service.GetStatisticsUserTable(year, where);
        }
        /// <summary>
        /// ��ȡ����ְҵ��ͳ�Ʊ�
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public DataTable GetStatisticsDeptTable(string year, string where)
        {
            return service.GetStatisticsDeptTable(year, where);
        }
        /// <summary>
        /// ��ȡ���ְҵ��ͳ�Ʊ�
        /// </summary>
        /// <param name="yearType">����������</param>
        /// <param name="Dept">����EnCode</param>
        /// <returns></returns>
        public DataTable GetStatisticsYearTable(int yearType, string Dept, string where)
        {
            return service.GetStatisticsYearTable(yearType, Dept, where);
        }
        /// <summary>
        /// ��ȡ�û�id�µ���������¼
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetUserTable(string userid)
        {
            return service.GetUserTable(userid);
        }

        /// <summary>
        /// ��ȡְҵ����Ա�嵥
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetStaffListPage(Pagination pagination, string queryJson)
        {
            return service.GetStaffListPage(pagination, queryJson);
        }
        /// <summary>
        /// ����������ѯ��������
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetTable(string queryJson, string where)
        {
            return service.GetTable(queryJson, where);
        }

        /// <summary>
        /// ���ݸ�id���Ƿ�������ѯ������Ϣ
        /// </summary>
        /// <param name="Pid">��id</param>
        /// <param name="SickType">�Ƿ�����</param>
        /// <returns></returns>
        public IEnumerable<OccupationalstaffdetailEntity> GetList(string Pid, int Issick)
        {
            return service.GetList(Pid, Issick);
        }
        /// <summary>
        /// �洢���̷�ҳ
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageListByProc(Pagination pagination, string queryJson)
        {
            return service.GetPageListByProc(pagination, queryJson);
        }

        /// <summary>
        /// ��ȡ�û�id�µ���������¼ ��
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable NewGetUserTable(string userid)
        {
            return service.NewGetUserTable(userid);
        }

        /// <summary>
        /// ��ȡ�û��ĽӴ�Σ������
        /// </summary>
        /// <returns></returns>
        public DataTable GetUserHazardfactor(string useraccount)
        {
            return service.GetUserHazardfactor(useraccount);
        }

        /// <summary>
        /// �ж��Ƿ���Ȩ�� ���쵼/Ehs��������Դ��
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool IsPer()
        {
            bool IsPermission = false;
            if (OperatorProvider.Provider.Current().IsSystem)
            {
                return true;
            }

            //��ȡ��ǰ�����û�
            var Appuser = OperatorProvider.Provider.Current();   //��ȡ�û�������Ϣ
            //EHS����������Դ���������ֵ��� ͨ���ֵ����
            var Perdeptname = Appuser.DeptCode;
            DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();
            var data = dataItemDetailBLL.GetDataItemListByItemCode("'SelectDept'").ToList();
            if (data != null)
            {
                foreach (var Peritem in data)
                {
                    string value = Peritem.ItemValue;
                    string[] values = value.Split('|');
                    for (int i = 0; i < values.Length; i++)
                    {
                        if (values[i] == Perdeptname) //������ű����Ӧ������Ȩ�޵���
                        {
                            return true;
                        }
                    }
                }
            }

            //����ǳ��쵼Ҳ��Ȩ��
            if (Appuser.RoleName.Contains("���������û�") ||  Appuser.RoleName.Contains("��˾�쵼"))
            {
                IsPermission = true;
            }

            return IsPermission;
        }

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public OccupationalstaffdetailEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        #endregion

        #region �ύ����
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
        public void SaveForm(string keyValue, OccupationalstaffdetailEntity entity)
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
        /// ��������ɾ�������û� �������
        /// </summary>
        /// <param name="time">���ʱ��</param>
        /// <param name="parenid">����id</param>
        /// <returns></returns>
        public int Delete(string parenid, int SickType)
        {
            return service.Delete(parenid, SickType);
        }

        /// <summary>
        /// ���ݹ���id�����޸����ʱ��
        /// </summary>
        /// <param name="time">���ʱ��</param>
        /// <param name="parenid">����id</param>
        /// <returns></returns>
        public int UpdateTime(DateTime time, string parenid)
        {
            return service.UpdateTime(time, parenid);
        }
        #endregion
    }
}
