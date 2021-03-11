using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using ERCHTMS.Service.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.IService.OutsourcingProject;
using ERCHTMS.Service.OutsourcingProject;

namespace ERCHTMS.Busines.HighRiskWork
{
    /// <summary>
    /// �� �������֤
    /// </summary>
    public class LifthoistcertBLL
    {
        private LifthoistcertIService service = new LifthoistcertService();
        private PeopleReviewIService peopleReviwservice = new PeopleReviewService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public DataTable GetList(Pagination page, LifthoistSearchModel search)
        {
            return service.GetList(page, search);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public LifthoistcertEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, LifthoistcertEntity entity)
        {
            try
            {
                var dbentity = this.GetEntity(keyValue);
                if (dbentity == null)
                {
                    //˵��������
                    entity.ID = keyValue;
                    service.SaveForm(string.Empty, entity);
                }
                else
                {
                    //˵�����޸�
                    service.SaveForm(keyValue, entity);
                }
                //���Ϊ1��˵����ֱ���ύ������Ҫ�����
                if (entity.AUDITSTATE == 1)
                {
                    this.ApplyCheck(keyValue);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ƾ��֤���
        /// </summary>
        /// <param name="keyValue">����</param>
        /// <param name="auditEntity">���ʵ��</param>
        public void ApplyCheck(string keyValue, LifthoistcertEntity entity = null, LifthoistauditrecordEntity auditEntity = null)
        {
            Operator currUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            LifthoistcertEntity certEntity = this.GetEntity(keyValue);
            if (certEntity == null)
            {
                throw new ArgumentException("�޷��ҵ���ǰҵ����Ϣ����ȷ��ҵ��ID�Ƿ�����");
            }
            ManyPowerCheckEntity mpcEntity = null;
            //����ⲿ�����ʵ�岻Ϊnull����˵���Ǹ����˲���
            if (entity != null)
            {
                certEntity.CHARGEPERSONNAME = entity.CHARGEPERSONNAME;
                certEntity.CHARGEPERSONID = entity.CHARGEPERSONID;
                certEntity.CHARGEPERSONSIGN = entity.CHARGEPERSONSIGN;
                certEntity.HOISTAREAPERSONNAMES = entity.HOISTAREAPERSONNAMES;
                certEntity.HOISTAREAPERSONIDS = entity.HOISTAREAPERSONIDS;
                certEntity.HOISTAREAPERSONSIGNS = entity.HOISTAREAPERSONSIGNS;
                certEntity.safetys = entity.safetys;
            }
            if (auditEntity != null)
            {
                //�ѵ�ǰҵ�����̽ڵ㸳ֵ����˼�¼��
                auditEntity.FLOWID = certEntity.FLOWID;
            }
            string moduleName = "(���ص�װ׼��֤)���";
            mpcEntity = peopleReviwservice.CheckAuditForNextByOutsourcing(currUser, moduleName, certEntity.CONSTRUCTIONUNITID, certEntity.FLOWID, false, true);
            if (auditEntity != null && auditEntity.AUDITSTATE == 0)
            {
                certEntity.AUDITSTATE = 0;
                certEntity.FLOWID = string.Empty;
                certEntity.FLOWNAME = currUser.UserName + "���/����ͬ��";
                certEntity.FLOWDEPTID = currUser.DeptId;
                certEntity.FLOWDEPTNAME = currUser.DeptName;
                certEntity.FLOWROLEID = currUser.RoleId;
                certEntity.FLOWROLENAME = currUser.RoleName;
            }
            else
            {
                if (mpcEntity != null)
                {
                    certEntity.AUDITSTATE = 1;
                    certEntity.FLOWID = mpcEntity.ID;
                    certEntity.FLOWNAME = mpcEntity.FLOWNAME;
                    certEntity.FLOWDEPTID = mpcEntity.CHECKDEPTID;
                    certEntity.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                    certEntity.FLOWROLEID = mpcEntity.CHECKROLEID;
                    certEntity.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                }
                else
                {
                    certEntity.AUDITSTATE = 2;
                    certEntity.FLOWNAME = "�����";
                }
            }
            //����ʵ�壬���µ����ݿ�
            service.ApplyCheck(certEntity, auditEntity);
        }
        #endregion
    }
}
