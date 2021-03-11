using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using ERCHTMS.Service.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.IService.OutsourcingProject;
using ERCHTMS.Code;
using ERCHTMS.Service.OutsourcingProject;
using ERCHTMS.Entity.HiddenTroubleManage;

namespace ERCHTMS.Busines.HighRiskWork
{
    /// <summary>
    /// �� �������ص�װ��ҵ
    /// </summary>
    public class LifthoistjobBLL
    {
        private LifthoistjobIService service = new LifthoistjobService();
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
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public DataTable getTempEquipentList(Pagination page, LifthoistSearchModel search)
        {
            return service.getTempEquipentList(page, search);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public LifthoistjobEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// �õ�����ͼ
        /// </summary>
        /// <param name="keyValue">ҵ���ID</param>
        /// <param name="modulename">�����ģ����</param>
        /// <returns></returns>
        public Flow GetFlow(string keyValue, string modulename)
        {
            return service.GetFlow(keyValue, modulename);
        }

        public string GetLifthoistjobNum()
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            Pagination pagination = new Pagination();
            pagination.conditionJson = "1=1 and a.auditstate=1";
            pagination.page = 1;
            pagination.rows = 1000000000;
            LifthoistSearchModel serch = new LifthoistSearchModel();
            serch.viewrange = "selfaudit";
            DataTable dt = service.GetList(pagination, serch);
            return dt.Rows.Count.ToString();
        }

        public List<CheckFlowData> GetAppFlowList(string keyValue, string modulename)
        {
            return service.GetAppFlowList(keyValue, modulename);
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
        public void SaveForm(string keyValue, LifthoistjobEntity entity)
        {
            try
            {
                var dbentity = service.GetEntity(keyValue);
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
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// ���ص�װ��ҵ���
        /// </summary>
        /// <param name="keyValue">����</param>
        /// <param name="auditEntity">���ʵ��</param>
        public void ApplyCheck(string keyValue, LifthoistauditrecordEntity auditEntity = null)
        {
            try
            {
                Operator currUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var jobEntity = service.GetEntity(keyValue);
                ManyPowerCheckEntity mpcEntity = null;
                if (jobEntity == null)
                {
                    throw new ArgumentException("�޷��ҵ���ǰҵ����Ϣ����ȷ��ҵ��ID�Ƿ�����");
                }
                if (auditEntity != null)
                {
                    //�ѵ�ǰҵ�����̽ڵ㸳ֵ����˼�¼��
                    auditEntity.FLOWID = jobEntity.FLOWID;
                }
                //Ĭ��30T��������
                string moduleName = "(���ص�װ��ҵ30T����)���";
                if (jobEntity.QUALITYTYPE != "0")
                {
                    //30T��������
                    moduleName = "(���ص�װ��ҵ30T����)���";
                }
                if (jobEntity.WORKDEPTTYPE == "0")
                {
                    mpcEntity = peopleReviwservice.CheckAuditForNextByLiftHoist(currUser, moduleName, jobEntity.CONSTRUCTIONUNITID, jobEntity.FLOWID, false);
                }
                else
                {
                    mpcEntity = peopleReviwservice.CheckAuditForNextByLiftHoist(currUser, moduleName, jobEntity.ENGINEERINGID, jobEntity.FLOWID, false);
                }
               
                if (auditEntity != null && auditEntity.AUDITSTATE == 0)
                {
                    jobEntity.AUDITSTATE = 0;
                    jobEntity.FLOWID = string.Empty;
                    jobEntity.FLOWNAME = string.Empty;
                    jobEntity.FLOWDEPTID = string.Empty;
                    jobEntity.FLOWDEPTNAME = string.Empty;
                    jobEntity.FLOWROLEID = string.Empty;
                    jobEntity.FLOWROLENAME = string.Empty ;
                    jobEntity.FLOWREMARK = string.Empty;
                }
                else
                {
                    if (mpcEntity != null)
                    {
                        jobEntity.AUDITSTATE = 1;
                        jobEntity.FLOWID = mpcEntity.ID;
                        jobEntity.FLOWNAME = mpcEntity.FLOWNAME;
                        jobEntity.FLOWDEPTID = mpcEntity.CHECKDEPTID;
                        jobEntity.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                        jobEntity.FLOWROLEID = mpcEntity.CHECKROLEID;
                        jobEntity.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                        jobEntity.FLOWREMARK = mpcEntity.REMARK;
                    }
                    else
                    {
                        jobEntity.AUDITSTATE = 2;
                        jobEntity.FLOWNAME = "�����";
                        jobEntity.FLOWDEPTID = "";
                        jobEntity.FLOWDEPTNAME = "";
                        jobEntity.FLOWID = "";
                        jobEntity.FLOWREMARK = "";
                        jobEntity.FLOWROLEID = "";
                        jobEntity.FLOWROLENAME = "";
                    }
                }
                //����ʵ�壬���µ����ݿ�
                service.ApplyCheck(jobEntity, auditEntity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            

        }
        #endregion
    }
}
