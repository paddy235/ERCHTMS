using ERCHTMS.Entity.WorkPlan;
using ERCHTMS.IService.WorkPlan;
using ERCHTMS.Service.WorkPlan;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.WorkPlan
{
    /// <summary>
    /// �� ���������ƻ�����
    /// </summary>
    public class PlanDetailsBLL
    {
        private PlanDetailsIService service = new PlanDetailsService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<PlanDetailsEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡ��ҳ�б�
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetList(Pagination pagination, string queryJson)
        {
            return service.GetList(pagination, queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public PlanDetailsEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// ���±��״̬
        /// </summary>
        /// <param name="applyId"></param>
        public void UpdateChangedData(string applyId)
        {
            service.UpdateChangedData(applyId);
        }
        #endregion

        #region ͳ��
        /// <summary>
        /// ͳ�ƹ����ƻ�
        /// </summary>
        /// <param name="deptId"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public DataTable Statistics(string deptId, string starttime, string endtime, string applytype = "")
        {
            return service.Statistics(deptId, starttime, endtime,applytype);
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
        public void RemoveFormByApplyId(string keyValue)
        {
            try
            {
                service.RemoveFormByApplyId(keyValue);
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
        public void SaveForm(string keyValue, PlanDetailsEntity entity)
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
        /// ���±���
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void SaveMonth(string keyValue, PlanDetailsEntity entity)
        {
            try
            {
                for (int i = 0; i < 12; i++)
                {
                    var pdt = DateTime.Parse(string.Format("{0}-{1}-01", entity.PlanFinDate.Value.Year, (i + 1))).AddMonths(1).AddSeconds(-1);
                    entity.ID = Guid.NewGuid().ToString();
                    entity.PlanFinDate = pdt;
                    service.SaveForm("", entity);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
