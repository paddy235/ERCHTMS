using ERCHTMS.Entity.HazardsourceManage;
using ERCHTMS.IService.HazardsourceManage;
using ERCHTMS.Service.HazardsourceManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.HazardsourceManage
{
    /// <summary>
    /// �� ����Σ��Դ�嵥
    /// </summary>
    public class Hisrelationhd_qdBLL
    {
        private IHisrelationhd_qdService service = new Hisrelationhd_qdService();

        #region ��ȡ����
        public IEnumerable<Hisrelationhd_qdEntity> GetListForRecord(string queryJson)
        {
            return service.GetListForRecord(queryJson);
        }
        public DataTable GetReportForDistrictName(string queryJson)
        {
            return service.GetReportForDistrictName(queryJson);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<Hisrelationhd_qdEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public Hisrelationhd_qdEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        public string StaQueryList(string queryJson) {
            return service.StaQueryList(queryJson);
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
        public void SaveForm(string keyValue, Hisrelationhd_qdEntity entity)
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
        #endregion
    }
}
