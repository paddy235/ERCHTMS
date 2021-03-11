using ERCHTMS.Entity.OccupationalHealthManage;
using ERCHTMS.IService.OccupationalHealthManage;
using ERCHTMS.Service.OccupationalHealthManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;

namespace ERCHTMS.Busines.OccupationalHealthManage
{
    /// <summary>
    /// �� ����ְҵ��Σ�����ؼ��
    /// </summary>
    public class HazarddetectionBLL
    {
        private HazarddetectionIService service = new HazarddetectionService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<HazarddetectionEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public HazarddetectionEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
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
        /// ��ȡȫ���б�����
        /// </summary>
        /// <param name="riskid">Σ������</param>
        /// <param name="areaid">����</param>
        /// <param name="starttime">ʱ�䷶Χ</param>
        /// <param name="endtime">ʱ�䷶Χ</param>
        /// <param name="isexcessive">�Ƿ񳬱�</param>
        /// <param name="detectionuserid">�����id</param>
        /// <returns></returns>
        public DataTable GetDataTable(string queryJson, string where)
        {

            return service.GetDataTable(queryJson, where);
        }
        /// <summary>
        /// ��ȡ����ָ�꼰��׼
        /// </summary>
        /// <param name="RiskId">ְҵ��id</param>
        /// <returns></returns>
        public string GetStandard(string RiskId, string where)
        {
            return service.GetStandard(RiskId, where);
        }

        /// <summary>
        /// ��ȡΣ�����ؼ��ͳ������
        /// </summary>
        /// <param name="year">��һ������</param>
        /// <param name="risk">ְҵ������</param>
        /// <param name="type">true��ѯȫ�� false��ѯ��������</param>
        /// <returns></returns>
        public DataTable GetStatisticsHazardTable(int year, string risk, bool type, string where)
        {
            return service.GetStatisticsHazardTable(year, risk, type, where);
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ����id��������ɾ��
        /// </summary>
        /// <param name="Ids"></param>
        public void Remove(string Ids)
        {
            try
            {
                service.Remove(Ids);
            }
            catch (Exception)
            {

                throw;
            }
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
        public void SaveForm(string keyValue, HazarddetectionEntity entity)
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

        #region �ֻ���
        /// <summary>
        /// ����Σ�����ؼ������
        /// </summary>
        /// <param name="assess">ʵ��</param>
        /// <param name="user">��ǰ�û�</param>
        /// <returns></returns>
        public int SaveHazard(HazarddetectionEntity hazard, ERCHTMS.Code.Operator user)
        {
            try
            {
                return service.SaveHazard(hazard, user);
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// ��ȡȫ���б�����
        /// </summary>
        /// <param name="riskid">Σ������</param>
        /// <param name="areaid">����</param>
        /// <param name="starttime">ʱ�䷶Χ</param>
        /// <param name="endtime">ʱ�䷶Χ</param>
        /// <param name="isexcessive">�Ƿ񳬱�</param>
        /// <param name="detectionuserid">�����id</param>
        /// <returns></returns>
        public DataTable GetDataTable(string riskid, string areaid, string starttime, string endtime, string isexcessive, string detectionuserid, string where)
        {
            try
            {
                return service.GetDataTable(riskid, areaid, starttime, endtime, isexcessive, detectionuserid, where);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}
