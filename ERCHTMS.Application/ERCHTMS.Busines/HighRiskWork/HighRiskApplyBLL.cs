using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using ERCHTMS.Service.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.HighRiskWork
{
    /// <summary>
    /// �� �����߷�����ҵ�������
    /// </summary>
    public class HighRiskApplyBLL
    {
        private HighRiskApplyIService service = new HighRiskApplyService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<HighRiskApplyEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<HighRiskApplyEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public HighRiskApplyEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageDataTable(Pagination pagination, string queryJson)
        {
            return service.GetPageDataTable(pagination, queryJson);
        }

        /// <summary>
        /// ��ȡ���������ҵ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetSelectDataTable(Pagination pagination, string queryJson)
        {
            return service.GetSelectDataTable(pagination, queryJson);
        }
        /// <summary>
        /// ��ȡ���(��)�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetVerifyPageTableJson(Pagination pagination, string queryJson)
        {
            return service.GetVerifyPageTableJson(pagination, queryJson);
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
        public int SaveForm(string keyValue, HighRiskApplyEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        #endregion

        #region ͳ��

        /// <summary>
        /// ��ȡ���������ҵ�б�(ͳ����ת)
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetStatisticsWorkTable(Pagination pagination, string queryJson)
        {
            return service.GetStatisticsWorkTable(pagination, queryJson);
        }

        /// <summary>
        /// ����ҵ����ͳ��
        /// </summary>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public string GetHighWorkCount(string starttime, string endtime, string deptid, string deptcode)
        {
            return service.GetHighWorkCount(starttime, endtime, deptid, deptcode);
        }


        /// <summary>
        ///��ҵ����ͳ��(ͳ�Ʊ��)
        /// </summary>
        /// <returns></returns>
        public string GetHighWorkList(string starttime, string endtime, string deptid, string deptcode)
        {
            return service.GetHighWorkList(starttime, endtime, deptid, deptcode);
        }

        /// <summary>
        /// �¶�����(ͳ��ͼ)
        /// </summary>
        /// <param name="year"></param>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public string GetHighWorkYearCount(string year, string deptid, string deptcode)
        {
            return service.GetHighWorkYearCount(year, deptid, deptcode);
        }

        /// <summary>
        /// �¶�����(���)
        /// </summary>
        /// <param name="year"></param>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public string GetHighWorkYearList(string year, string deptid, string deptcode)
        {
            return service.GetHighWorkYearList(year, deptid, deptcode);
        }
        #endregion

        #region �ƶ���
        /// <summary>
        /// ����ҵ����ͳ��
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataTable GetWorkTypeInfo(string starttime, string endtime, string deptid, string deptcode)
        {
            return service.GetWorkTypeInfo(starttime, endtime, deptid, deptcode);
        }

        /// <summary>
        /// �¶�����
        /// </summary>
        /// <param name="year"></param>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataTable GetWorkYearCount(string year, string deptid, string deptcode)
        {
            return service.GetWorkYearCount(year, deptid, deptcode);
        }
        #endregion
    }
}
