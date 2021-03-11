using ERCHTMS.Entity.PowerPlantInside;
using ERCHTMS.Busines.PowerPlantInside;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using System;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.BaseManage;
using System.Collections.Generic;
using System.Linq;

namespace ERCHTMS.Web.Areas.PowerPlantInside.Controllers
{
    /// <summary>
    /// �� �����¹��¼�����
    /// </summary>
    public class PowerplantcheckController : MvcControllerBase
    {
        private PowerplantcheckBLL powerplantcheckbll = new PowerplantcheckBLL();
        private PowerplanthandledetailBLL powerplanthandledetailbll = new PowerplanthandledetailBLL();
        private PowerplanthandleBLL powerplanthandlebll = new PowerplanthandleBLL();
        private PowerplantreformBLL powerplantreformbll = new PowerplantreformBLL();

        #region ��ͼ����
        /// <summary>
        /// �б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }

        /// <summary>
        /// ��ʷ�б�
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HistoryIndex()
        {
            return View();
        }
        #endregion

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = powerplantcheckbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = powerplantcheckbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }


        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="PowerInsideHandleDetailId">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string PowerInsideHandleDetailId)
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                pagination.p_kid = "a.id";
                pagination.p_fields = @"a.auditresult,a.audittime,a.auditopinion,a.auditpeople,a.auditsignimg,b.filenum";
                pagination.p_tablename = @"bis_powerplantcheck a left join (select c.id,count(d.fileid) as filenum from bis_powerplantcheck c left join base_fileinfo d on d.recid=c.id group by c.id) b on a.id=b.id";
                pagination.conditionJson = string.Format("a.powerplanthandledetailid='{0}' and a.disable=0", PowerInsideHandleDetailId);

                var watch = CommonHelper.TimerStart();
                var data = powerplantreformbll.GetPageList(pagination, "");
                var jsonData = new
                {
                    rows = data,
                    total = pagination.total,
                    page = pagination.page,
                    records = pagination.records,
                    costtime = CommonHelper.TimerEnd(watch)
                };
                return ToJsonResult(jsonData);
            }
            catch (System.Exception ex)
            {
                return Error(ex.ToString());
            }

        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="PowerInsideHandleReformId">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetPageListJsonByReformId(Pagination pagination, string PowerInsideHandleReformId)
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                pagination.p_kid = "a.id";
                pagination.p_fields = @"a.auditresult,a.auditopinion,a.auditpeople,a.auditsignimg,b.filenum";
                pagination.p_tablename = @"bis_powerplantcheck a left join (select c.id,count(d.fileid) as filenum from bis_powerplantcheck c left join base_fileinfo d on d.recid=c.id group by c.id) b on a.id=b.id";
                pagination.conditionJson = string.Format("a.POWERPLANTREFORMID='{0}'", PowerInsideHandleReformId);

                var watch = CommonHelper.TimerStart();
                var data = powerplantreformbll.GetPageList(pagination, "");
                var jsonData = new
                {
                    rows = data,
                    total = pagination.total,
                    page = pagination.page,
                    records = pagination.records,
                    costtime = CommonHelper.TimerEnd(watch)
                };
                return ToJsonResult(jsonData);
            }
            catch (System.Exception ex)
            {
                return Error(ex.ToString());
            }

        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="PowerInsideHandleDetailId">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetHistoryPageListJson(Pagination pagination, string PowerInsideHandleDetailId)
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                pagination.p_kid = "a.id";
                pagination.p_fields = @"a.auditresult,a.audittime,a.auditopinion,a.auditpeople,a.auditsignimg,b.filenum";
                pagination.p_tablename = @"bis_powerplantcheck a left join (select c.id,count(d.fileid) as filenum from bis_powerplantcheck c left join base_fileinfo d on d.recid=c.id group by c.id) b on a.id=b.id";
                pagination.conditionJson = string.Format("a.POWERPLANTHANDLEDETAILID='{0}' and a.disable=1 ", PowerInsideHandleDetailId, user.UserId);
                
                var watch = CommonHelper.TimerStart();
                var data = powerplantreformbll.GetPageList(pagination, "");
                var jsonData = new
                {
                    rows = data,
                    total = pagination.total,
                    page = pagination.page,
                    records = pagination.records,
                    costtime = CommonHelper.TimerEnd(watch)
                };
                return ToJsonResult(jsonData);
            }
            catch (System.Exception ex)
            {
                return Error(ex.ToString());
            }

        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(6, "�¹��¼�����ɾ��")]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            powerplantcheckbll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(5, "�¹��¼����ձ���")]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, PowerplantcheckEntity entity)
        {
            powerplantcheckbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }


        /// <summary>
        /// ����
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <param name="aentity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(0, "�¹��¼��������")]
        [AjaxOnly]
        public ActionResult ApporveForm(string keyValue, PowerplantcheckEntity aentity)
        {
            try
            {
                Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

                string state = string.Empty;

                string moduleName = "�¹��¼������¼-����";

                PowerplanthandledetailEntity entity = powerplanthandledetailbll.GetEntity(aentity.PowerPlantHandleDetailId);
                /// <param name="currUser">��ǰ��¼��</param>
                /// <param name="state">�Ƿ���Ȩ����� 1������� 0 ���������</param>
                /// <param name="moduleName">ģ������</param>
                /// <param name="createdeptid">�����˲���ID</param>
                ManyPowerCheckEntity mpcEntity = powerplanthandlebll.CheckAuditPower(curUser, out state, moduleName, entity.RealReformDeptId);


                #region //�����Ϣ��
                PowerplantcheckEntity aidEntity = new PowerplantcheckEntity();
                aidEntity.AuditResult = aentity.AuditResult; //ͨ��
                aidEntity.AuditTime = Convert.ToDateTime(aentity.AuditTime.Value.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss")); //���ʱ��
                aidEntity.AuditPeople = aentity.AuditPeople;  //�����Ա����
                aidEntity.AuditPeopleId = aentity.AuditPeopleId;//�����Աid
                aidEntity.AuditDeptId = aentity.AuditDeptId;//��˲���id
                aidEntity.AuditDept = aentity.AuditDept; //��˲���
                aidEntity.AuditOpinion = aentity.AuditOpinion; //������
                aidEntity.FlowId = entity.FlowId;
                aidEntity.AuditSignImg = string.IsNullOrWhiteSpace(aentity.AuditSignImg) ? "" : aentity.AuditSignImg.ToString().Replace("../..", "");
                aidEntity.PowerPlantHandleDetailId = aentity.PowerPlantHandleDetailId;
                aidEntity.PowerPlantHandleId = aentity.PowerPlantHandleId;
                aidEntity.PowerPlantReformId = aentity.PowerPlantReformId;
                aidEntity.AuditDeptId = curUser.DeptId;
                aidEntity.AuditDept = curUser.DeptName;
                aidEntity.Disable = 0;
                if (null != mpcEntity)
                {
                    aidEntity.Remark = (mpcEntity.AUTOID.Value - 1).ToString(); //��ע �����̵�˳���
                }
                else
                {
                    aidEntity.Remark = "7";
                }
                powerplantcheckbll.SaveForm(keyValue, aidEntity);
                #endregion

                #region  //�����¹��¼������¼
                //���ͨ��
                if (aentity.AuditResult == 0)
                {
                    //0��ʾ����δ��ɣ�1��ʾ���̽���
                    if (null != mpcEntity)
                    {
                        entity.FlowDept = mpcEntity.CHECKDEPTID;
                        entity.FlowDeptName = mpcEntity.CHECKDEPTNAME;
                        entity.FlowRole = mpcEntity.CHECKROLEID;
                        entity.FlowRoleName = mpcEntity.CHECKROLENAME;
                        entity.ApplyState = 4;
                        entity.FlowId = mpcEntity.ID;
                        entity.FlowName = mpcEntity.CHECKDEPTNAME + "������";
                    }
                    else
                    {
                        entity.FlowDept = "";
                        entity.FlowDeptName = "";
                        entity.FlowRole = "";
                        entity.FlowRoleName = "";
                        entity.ApplyState = 5;
                        entity.FlowName = "";
                        entity.FlowId = "";
                    }
                }
                else //���ղ�ͨ�� 
                {
                    entity.FlowDept = "";
                    entity.FlowDeptName = "";
                    entity.FlowRole = "";
                    entity.FlowRoleName = "";
                    entity.ApplyState = 3; //�˻ص�����״̬
                    entity.FlowName = "";
                    entity.FlowId = "";
                    entity.RealReformDept = "";
                    entity.RealReformDeptCode = "";
                    entity.RealReformDeptId = "";

                }
                //�����¹��¼�������Ϣ
                powerplanthandledetailbll.SaveForm(entity.Id, entity);
                powerplanthandlebll.UpdateApplyStatus(entity.PowerPlantHandleId);
                #endregion

                #region    //��˲�ͨ��
                if (aentity.AuditResult == 1)
                {
                    var reformlist = powerplantreformbll.GetList("").Where(t => t.PowerPlantHandleDetailId == aentity.PowerPlantHandleDetailId && t.Disable == 0).ToList(); //������Ϣ
                    var checklist = powerplantcheckbll.GetList("").Where(t => t.PowerPlantHandleDetailId == aentity.PowerPlantHandleDetailId && t.Disable == 0).ToList(); //������Ϣ
                    foreach (var item in reformlist)
                    {
                        item.Disable = 1; //��������Ϣ����ʧЧ
                        powerplantreformbll.SaveForm(item.Id, item);
                    }
                    foreach (var item in checklist)
                    {
                        item.Disable = 1; //��������Ϣ����ʧЧ
                        powerplantcheckbll.SaveForm(item.Id, item);

                    }
                }
                #endregion

                return Success("�����ɹ�!");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
            
        }
        #endregion
    }
}
