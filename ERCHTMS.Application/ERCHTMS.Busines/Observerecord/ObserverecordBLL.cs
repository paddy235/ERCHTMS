using ERCHTMS.Entity.Observerecord;
using ERCHTMS.IService.Observerecord;
using ERCHTMS.Service.Observerecord;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.Observerecord
{
    /// <summary>
    /// �� �����۲��¼��
    /// </summary>
    public class ObserverecordBLL
    {
        private ObserverecordIService service = new ObserverecordService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }

        public DataTable GetTable(string sql) {
            return service.GetTable(sql);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<ObserverecordEntity> GetList()
        {
            return service.GetList();
        }
        /// <summary>
        /// ���ݹ۲��¼Id��ȡ�۲����
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public DataTable GetObsTypeData(string keyValue) {
            return service.GetObsTypeData(keyValue);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public ObserverecordEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// ��ȡ��ȫ��Ϊ�벻��ȫ��Ϊռ��ͳ��
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public string GetSafetyStat(string deptCode, string year = "", string quarter = "",string month="")
        {
            return service.GetSafetyStat(deptCode, year, quarter, month);
        }
        /// <summary>
        /// ��ȡ�۲�����Ա�ͼ
        /// </summary>
        /// <param name="deptCode">��λCode</param>
        /// <param name="year">��</param>
        /// <param name="quarter">����</param>
        /// <param name="month">�¶�</param>
        /// <param name="issafety">issafety 0 ����ȫ��Ϊ 1 ��ȫ��Ϊ</param>
        /// <returns></returns>
        public string GetUntiDbStat(string deptCode, string issafety, string year = "", string quarter = "", string month = "")
        {
            return service.GetUntiDbStat(deptCode,issafety, year, quarter, month);
        }
        /// <summary>
        /// ��ȡ����ȫ��������ͼ
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public string GetQsStat(string deptCode, string year = "")
        {
            return service.GetQsStat(deptCode, year);
        }
        /// <summary>
        /// ���ݹ۲�ƻ�Id������ֽ�Id��ѯ�Ƿ�����˹۲��¼
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="planfjid"></param>
        /// <returns></returns>
        public bool GetObsRecordByPlanIdAndFjId(string planid, string planfjid) {
            return service.GetObsRecordByPlanIdAndFjId(planid, planfjid);
        }

        public DataTable GetMenuTable(string sql) {
            return service.GetTable(sql);
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
        public void SaveForm(string keyValue, ObserverecordEntity entity, List<ObservecategoryEntity> listCategory, List<ObservesafetyEntity> safetyList)
        {
            try
            {
                service.SaveForm(keyValue, entity, listCategory, safetyList);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
