using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.IService.RiskDatabase;
using ERCHTMS.Service.RiskDatabase;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.RiskDatabase
{
    /// <summary>
    /// �� ��������Ԥ֪ѵ����
    /// </summary>
    public class RisktrainlibBLL
    {
        private RisktrainlibIService service = new RisktrainlibService();

        #region ��ȡ����


        public DataTable GetPageListJson(Pagination pagination, string queryJson) {
            return service.GetPageListJson(pagination,queryJson);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<RisktrainlibEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public RisktrainlibEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// ��ȡ��ҵ��ȫ������
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public DataTable GetRisktrainlibList(string p)
        {
            return service.GetRisktrainlibList(p);
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
        /// ɾ����Դ���տ�����
        /// </summary>
        /// <param name="keyValue">����</param>
        public bool DelRiskData()
        {
            try
            {
                return service.DelRiskData();
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
        public void SaveForm(string keyValue, RisktrainlibEntity entity, List<RisktrainlibdetailEntity> listMesures)
        {
            try
            {
                service.SaveForm(keyValue, entity, listMesures);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertRiskTrainLib(List<RisktrainlibEntity> RiskLib) {
            try
            {
                service.InsertRiskTrainLib(RiskLib);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void InsertImportData(List<RisktrainlibEntity> RiskLib, List<RisktrainlibdetailEntity> detailLib)
        {
            try
            {
                service.InsertImportData(RiskLib, detailLib);
            }
            catch (Exception)
            {
                throw;
            }
        }
        

        #endregion

    }
}
