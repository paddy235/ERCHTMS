using ERCHTMS.Entity.BaseManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.Service.BaseManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using BSFramework.Application.Entity;
using ERCHTMS.Code;
using ERCHTMS.IService.HighRiskWork;
using ERCHTMS.Service.HighRiskWork;

namespace ERCHTMS.Busines.HighRiskWork
{
    /// <summary>
    /// �� ����
    /// </summary>
    public class ProvinceHighWorkBLL
    {
        private ProvinceHighWorkIService service = new ProvinceHighWorkService();


        #region ͳ��
        /// <summary>
        /// ����ҵ����ͳ��
        /// </summary>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public string GetProvinceHighCount(string starttime, string endtime, string deptid, string deptcode)
        {
            return service.GetProvinceHighCount(starttime, endtime, deptid, deptcode);
        }

        /// <summary>
        ///��ҵ����ͳ��(ͳ�Ʊ��)
        /// </summary>
        /// <returns></returns>
        public string GetProvinceHighList(string starttime, string endtime, string deptid, string deptcode)
        {
            return service.GetProvinceHighList(starttime, endtime, deptid, deptcode);
        }

        /// <summary>
        /// ��λ�Ա�(ͳ��ͼ)
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public string GetProvinceHighDepartCount(string starttime, string endtime)
        {
            return service.GetProvinceHighDepartCount(starttime, endtime);
        }

        /// <summary>
        ///��λ�Ա�(ͳ�Ʊ��)
        /// </summary>
        /// <returns></returns>
        public string GetProvinceHighDepartList(string starttime, string endtime)
        {
            return service.GetProvinceHighDepartList(starttime, endtime);
        }
        #endregion

        /// <summary>
        /// ��ȡ�߷�����ҵ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageDataTable(Pagination pagination, string queryJson)
        {
            return service.GetPageDataTable(pagination, queryJson);
        }

        #region �ֻ��˸߷�����ҵͳ��
        public DataTable AppGetHighWork(string starttime, string endtime, string deptid, string deptcode)
        {
            return service.AppGetHighWork(starttime, endtime, deptid, deptcode);
        }
        #endregion
    }
}
