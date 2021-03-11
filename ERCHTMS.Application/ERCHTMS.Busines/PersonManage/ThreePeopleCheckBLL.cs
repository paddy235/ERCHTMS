using ERCHTMS.Entity.PersonManage;
using ERCHTMS.IService.PersonManage;
using ERCHTMS.Service.PersonManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Code;

namespace ERCHTMS.Busines.PersonManage
{
    /// <summary>
    /// �� ��������������ҵ���
    /// </summary>
    public class ThreePeopleCheckBLL
    {
        private ThreePeopleCheckIService service = new ThreePeopleCheckService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<ThreePeopleCheckEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
         /// <summary>
        /// ��ȡ��Ա��Ϣ
        /// </summary>
        /// <param name="applyId">�����Id</param>
        /// <returns></returns>
        public IEnumerable<ThreePeopleInfoEntity> GetUserList(string applyId)
        {
            return service.GetUserList(applyId);
        }
        public DataTable GetItemList(string id)
        {
            return service.GetItemList(id);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public ThreePeopleCheckEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, ThreePeopleCheckEntity entity)
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
        public bool SaveForm(string keyValue, ThreePeopleCheckEntity entity, List<ThreePeopleInfoEntity> list, ERCHTMS.Entity.OutsourcingProject.AptitudeinvestigateauditEntity auditInfo = null)
        {
            return service.SaveForm(keyValue, entity, list, auditInfo);
        }
        #endregion
        /// <summary>
        /// ��ǰ��¼���Ƿ���Ȩ����˲���ȡ��һ�����Ȩ��ʵ��
        /// </summary>
        /// <param name="currUser">��ǰ��¼��</param>
        /// <param name="state">�Ƿ���Ȩ����� 1������� 0 ���������</param>
        /// <param name="moduleName">ģ������</param>
        /// <param name="createdeptid">�����˲���ID</param>
        /// <returns>null-��ǰΪ���һ�����,ManyPowerCheckEntity����һ�����Ȩ��ʵ��</returns>
        public ManyPowerCheckEntity CheckAuditPower(Operator currUser, out string state, string moduleName, string createdeptid,string applyId="")
        {
            return service.CheckAuditPower(currUser, out state, moduleName, createdeptid, applyId);
        }
    }
}
