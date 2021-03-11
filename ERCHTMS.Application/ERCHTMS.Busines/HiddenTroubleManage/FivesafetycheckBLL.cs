using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.IService.HiddenTroubleManage;
using ERCHTMS.Service.HiddenTroubleManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Busines.HiddenTroubleManage
{
    /// <summary>
    /// �� �����嶨��ȫ���
    /// </summary>
    public class FivesafetycheckBLL
    {
        private FivesafetycheckIService service = new FivesafetycheckService();

        #region ��ȡ����
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetDeptByName(string name)
        {
            return service.GetDeptByName(name);
        }

        /// <summary>
        /// ���ݼ�����ͱ�Ų�ѯ��ҳ
        /// </summary>
        /// <param name="itemcode"></param>
        /// <returns></returns>
        public DataTable DeskTotalByCheckType(string itemcode)
        {
            return service.DeskTotalByCheckType(itemcode);
        }

        /// <summary>
        /// ���ذ�ȫ���˲�ͬ���ʹ�����������������
        /// </summary>
        /// <param name="fivetype">�������</param>
        /// <param name="istopcheck"> 0:�ϼ���˾��� 1����˾��ȫ���</param>
        /// <param name="type"> 0:������̣�1������  2������</param>
        /// <returns></returns>
        public string GetApplyNum(string fivetype, string istopcheck, string type)
        {
            return service.GetApplyNum(fivetype, istopcheck, type);
        }

        /// <summary>
        /// ����sql��ѯ����
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetInfoBySql(string sql)
        {
            return service.GetInfoBySql(sql);
        }
        public DataTable ExportAuditTotal(string keyvalue)
        {
            return service.ExportAuditTotal(keyvalue);
        }

        /// <summary>
        /// ��ѯ�������ͼ
        /// </summary>
        /// <param name="keyValue">����</param>
        /// <param name="urltype">��ѯ���ͣ�0����ȫ����</param>
        /// <returns></returns>
        public Flow GetAuditFlowData(string keyValue, string urltype)
        {
            return service.GetAuditFlowData(keyValue, urltype);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<FivesafetycheckEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }

        public IEnumerable<UserEntity> GetStepDept(ManyPowerCheckEntity powerinfo, string id)
        {
            return service.GetStepDept(powerinfo, id);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public DataTable GetPageListJson(Pagination pagination, string queryJson)
        {
            return service.GetPageListJson(pagination, queryJson);
        }

        /// <summary>
        /// ��ȡ��������б��ҳ
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetAuditListJson(Pagination pagination, string queryJson)
        {
            return service.GetAuditListJson(pagination, queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public FivesafetycheckEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, FivesafetycheckEntity entity)
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
