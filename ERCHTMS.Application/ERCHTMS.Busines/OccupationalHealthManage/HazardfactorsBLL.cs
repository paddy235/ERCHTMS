using ERCHTMS.Entity.OccupationalHealthManage;
using ERCHTMS.IService.OccupationalHealthManage;
using ERCHTMS.Service.OccupationalHealthManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Code;

namespace ERCHTMS.Busines.OccupationalHealthManage
{
    /// <summary>
    /// �� ����Σ�������嵥
    /// </summary>
    public class HazardfactorsBLL
    {
        private HazardfactorsIService service = new HazardfactorsService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<HazardfactorsEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="AreaId">����id</param>
        /// <param name="where">������ѯ����</param>
        /// <returns></returns>
        public DataTable GetList(string AreaId, string where)
        {
            return service.GetList(AreaId, where);
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
        /// ��֤���������Ƿ��ظ�
        /// </summary>
        /// <param name="AreaValue">��������</param>
        /// <returns></returns>
        public bool ExistDeptJugement(string AreaValue, string orgCode, string RiskName)
        {
            return service.ExistDeptJugement(AreaValue, orgCode, RiskName);
        }

        /// <summary>
        /// ��֤����id��Σ��Դ�Ƿ��ظ�//���ֲ�ͬ��˾�û�
        /// </summary>
        /// <param name="Areaid">��������</param>
        /// <returns></returns>
        public bool ExistAreaidJugement(string Areaid, string RiskName, string Hid)
        {
            if (Hid == null || Hid == "")
            {
                Hid = "1";
            }
            return service.ExistAreaidJugement(Areaid, OperatorProvider.Provider.Current().OrganizeCode, RiskName, Hid);
        }

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public HazardfactorsEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, HazardfactorsEntity entity, string UserName, string UserId)
        {
            try
            {
                service.SaveForm(keyValue, entity,UserName,UserId);
            }
            catch (Exception)
            {
                throw;
            }
        }



        /// <summary>
        /// �ӷ����嵥��������
        /// </summary>
        /// <param name="Areaid">����id</param>
        /// <param name="AreaValue">��������</param>
        /// <param name="RiskValue">Σ��Դ����</param>
        public void Add(string Areaid, string AreaValue, string RiskValue)
        {
            try
            {
                if (service.ExistAreaidJugement(Areaid, OperatorProvider.Provider.Current().OrganizeCode, RiskValue)) //û���ظ����������
                {

                    string RiskId = service.IsRisk("Risk", RiskValue);
                    if (RiskId != "")
                    {
                        HazardfactorsEntity entity = new HazardfactorsEntity();
                        entity.AreaId = Areaid;
                        entity.AreaValue = AreaValue;
                        entity.Riskid = RiskId;
                        entity.RiskValue = RiskValue;
                        //service.SaveForm("", entity);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion
        #region �ֻ���
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<HazardfactorsEntity> PhoneGetList(string queryJson, string orgid)
        {
            return service.PhoneGetList(queryJson, orgid);
        }
        #endregion
    }
}
