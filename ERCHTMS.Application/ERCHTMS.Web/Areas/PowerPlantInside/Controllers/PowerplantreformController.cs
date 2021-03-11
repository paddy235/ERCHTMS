using ERCHTMS.Entity.PowerPlantInside;
using ERCHTMS.Busines.PowerPlantInside;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.BaseManage;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ERCHTMS.Web.Areas.PowerPlantInside.Controllers
{
    /// <summary>
    /// �� �����¹��¼���������
    /// </summary>
    public class PowerplantreformController : MvcControllerBase
    {
        private PowerplantreformBLL powerplantreformbll = new PowerplantreformBLL();
        private PowerplanthandledetailBLL powerplanthandledetailbll = new PowerplanthandledetailBLL();
        private PowerplanthandleBLL powerplanthandlebll = new PowerplanthandleBLL();
        private PowerplantcheckBLL powerplantcheckbll = new PowerplantcheckBLL();

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

        /// <summary>
        /// ǩ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SignForm()
        {
            return View();
        }

        public ActionResult AppHandleForm()
        {
            return View();
        }
        #endregion

        #region ��ȡ����

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                pagination.p_kid = "d.ID";
                pagination.p_fields = @"d.CreateUserId,d.CreateDate,d.CreateUserName,d.ModifyUserId,d.ModifyDate,d.ModifyUserName,d.CreateUserDeptCode,d.CreateUserOrgCode,a.id as powerplanthandleid,d.applystate,a.accidenteventname,b.itemname as accidenteventtype,c.itemname as accidenteventproperty,a.HAPPENTIME,d.RECTIFICATIONTIME,e.id as powerplantreformid,d.rectificationdutypersonid,e.outtransferuseraccount,e.intransferuseraccount";
                pagination.p_tablename = @"bis_powerplanthandledetail d left join bis_powerplantreform e on d.id=e.powerplanthandledetailid and e.disable=0 left join BIS_POWERPLANTHANDLE a on d.powerplanthandleid =a.id 
                left join ( select * from base_dataitemdetail  where itemid = ( select itemid from base_dataitem where  parentid = 
                (select itemid from base_dataitem where itemname = '��λ�ڲ��챨' ) and  itemcode = 'AccidentEventType') ) b on a.accidenteventtype = b.itemvalue
                  left join ( select * from base_dataitemdetail  where itemid = ( select itemid from base_dataitem where  parentid = 
                (select itemid from base_dataitem where itemname = '��λ�ڲ��챨' ) and  itemcode = 'AccidentEventProperty') ) c on a.accidenteventproperty = c.itemvalue
                left join (select recid,flowid,outtransferuseraccount,intransferuseraccount,outtransferusername,intransferusername,row_number()  over(partition by recid,flowid order by createdate desc) as num from BIS_TRANSFERRECORD where disable=0) e on d.id=e.recid and e.num=1";
                pagination.conditionJson = "1=1";
                if (!user.IsSystem)
                {
                    //���ݵ�ǰ�û���ģ���Ȩ�޻�ȡ��¼
                    string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "createuserdeptcode", "createuserorgcode");
                    if (!string.IsNullOrEmpty(where))
                    {
                        pagination.conditionJson += " and " + where;
                    }
                }


                var watch = CommonHelper.TimerStart();
                var data = powerplantreformbll.GetPageList(pagination, queryJson);
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
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = powerplantreformbll.GetList(queryJson);
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
            var data = powerplantreformbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        ///// <summary>
        ///// ��ȡʵ�� 
        ///// </summary>
        ///// <param name="keyValue">������Ϣ����ֵ</param>
        ///// <returns>���ض���Json</returns>
        //[HttpGet]
        //public ActionResult GetFormJsonByHandleDetailId(string keyValue)
        //{
        //    try
        //    {
        //        var reformlist = powerplantreformbll.GetList("").Where(t => t.PowerPlantHandleDetailId == keyValue && t.Disable == 0).ToList();
        //        if (reformlist.Count > 0)
        //        {
        //            var data = powerplantreformbll.GetEntity(reformlist[0].Id);
        //            return ToJsonResult(data);
        //        }
        //        else
        //        {
        //            return Error("ϵͳ��������ϵϵͳ����Ա����");
        //        }
                
        //    }
        //    catch (Exception ex)
        //    {
        //        return Error(ex.ToString());
        //    }
            
        //}


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
                pagination.p_kid = "id";
                pagination.p_fields = @"powerplanthandleid,powerplanthandledetailid,rectificationperson,rectificationpersonsignimg,rectificationsituation,rectificationendtime";
                pagination.p_tablename = @"bis_powerplantreform";
                pagination.conditionJson = string.Format("POWERPLANTHANDLEDETAILID='{0}' and disable=1", PowerInsideHandleDetailId);



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
        [HandlerMonitor(6, "�¹��¼���������ɾ��")]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            powerplantreformbll.RemoveForm(keyValue);
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
        [HandlerMonitor(5, "�¹��¼��������ı���")]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, PowerplantreformEntity entity)
        {
            try
            {
                PowerplanthandledetailEntity powerplanthandledetailentity = powerplanthandledetailbll.GetEntity(entity.PowerPlantHandleDetailId);
                if (powerplanthandledetailentity != null)
                {
                    Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
                    powerplanthandledetailentity.RealReformDept = curUser.DeptName;
                    powerplanthandledetailentity.RealReformDeptId = curUser.DeptId;
                    powerplanthandledetailentity.RealReformDeptCode = curUser.DeptCode;
                    string state = string.Empty;

                    string moduleName = "�¹��¼������¼-����";

                    /// <param name="currUser">��ǰ��¼��</param>
                    /// <param name="state">�Ƿ���Ȩ����� 1������� 0 ���������</param>
                    /// <param name="moduleName">ģ������</param>
                    /// <param name="outengineerid">����Id</param>
                    ManyPowerCheckEntity mpcEntity = powerplanthandlebll.CheckAuditPower(curUser, out state, moduleName, curUser.DeptId);

                    string flowid = string.Empty;
                    List<ManyPowerCheckEntity> powerList = new ManyPowerCheckBLL().GetListBySerialNum(curUser.OrganizeCode, moduleName);
                    List<ManyPowerCheckEntity> checkPower = new List<ManyPowerCheckEntity>();
                    foreach (var item in powerList)
                    {
                        if (item.CHECKDEPTID == "-3" || item.CHECKDEPTID == "-1")
                        {
                            item.CHECKDEPTID = curUser.DeptId;
                            item.CHECKDEPTCODE = curUser.DeptCode;
                            item.CHECKDEPTNAME = curUser.DeptName;
                        }
                    }
                    for (int i = 0; i < powerList.Count; i++)
                    {
                        if (powerList[i].CHECKDEPTID == curUser.DeptId)
                        {
                            var rolelist = curUser.RoleName.Split(',');
                            for (int j = 0; j < rolelist.Length; j++)
                            {
                                if (powerList[i].CHECKROLENAME.Contains(rolelist[j]))
                                {
                                    checkPower.Add(powerList[i]);
                                    break;
                                }
                            }
                        }
                    }
                    if (checkPower.Count > 0)
                    {
                        ManyPowerCheckEntity check = checkPower.Last();//��ǰ

                        for (int i = 0; i < powerList.Count; i++)
                        {
                            if (check.ID == powerList[i].ID)
                            {
                                flowid = powerList[i].ID;
                            }
                        }
                    }
                    if (null != mpcEntity)
                    {
                        powerplanthandledetailentity.FlowDept = mpcEntity.CHECKDEPTID;
                        powerplanthandledetailentity.FlowDeptName = mpcEntity.CHECKDEPTNAME;
                        powerplanthandledetailentity.FlowRole = mpcEntity.CHECKROLEID;
                        powerplanthandledetailentity.FlowRoleName = mpcEntity.CHECKROLENAME;
                        powerplanthandledetailentity.ApplyState = 4; //����δ��ɣ�1��ʾ���
                        powerplanthandledetailentity.FlowId = mpcEntity.ID;
                        powerplanthandledetailentity.FlowName = mpcEntity.CHECKDEPTNAME + "������";
                    }
                    else  //Ϊ�����ʾ�Ѿ��������
                    {
                        powerplanthandledetailentity.FlowDept = "";
                        powerplanthandledetailentity.FlowDeptName = "";
                        powerplanthandledetailentity.FlowRole = "";
                        powerplanthandledetailentity.FlowRoleName = "";
                        powerplanthandledetailentity.ApplyState = 5; //����δ��ɣ�1��ʾ���
                        powerplanthandledetailentity.FlowName = "";
                        powerplanthandledetailentity.FlowId = "";
                    }
                    entity.RectificationPerson = curUser.UserName;
                    entity.RectificationPersonId = curUser.UserId;
                    entity.Disable = 0;
                    entity.RectificationPersonSignImg = string.IsNullOrWhiteSpace(entity.RectificationPersonSignImg) ? "" : entity.RectificationPersonSignImg.ToString().Replace("../..", "");
                    powerplantreformbll.SaveForm(keyValue, entity);
                    powerplanthandledetailbll.SaveForm(powerplanthandledetailentity.Id, powerplanthandledetailentity);
                    powerplanthandlebll.UpdateApplyStatus(entity.PowerPlantHandleId);

                    //���������Ϣ
                    if (state == "1")
                    {
                        //������Ϣ
                        PowerplantcheckEntity checkEntity = new PowerplantcheckEntity();
                        checkEntity.AuditResult = 0; //ͨ��
                        checkEntity.AuditTime = DateTime.Now;
                        checkEntity.AuditPeople = curUser.UserName;
                        checkEntity.AuditPeopleId = curUser.UserId;
                        checkEntity.PowerPlantHandleId = entity.PowerPlantHandleId;
                        checkEntity.PowerPlantHandleDetailId = entity.PowerPlantHandleDetailId;
                        checkEntity.PowerPlantReformId = keyValue;
                        checkEntity.AuditOpinion = ""; //������
                        checkEntity.AuditSignImg = string.IsNullOrWhiteSpace(entity.RectificationPersonSignImg) ? "" : entity.RectificationPersonSignImg.ToString().Replace("../..", "");
                        checkEntity.FlowId = flowid;
                        if (null != mpcEntity)
                        {
                            checkEntity.Remark = (mpcEntity.AUTOID.Value - 1).ToString(); //��ע �����̵�˳���
                        }
                        else
                        {
                            checkEntity.Remark = "7";
                        }
                        checkEntity.AuditDeptId = curUser.DeptId;
                        checkEntity.AuditDept = curUser.DeptName;
                        checkEntity.Disable = 0;
                        powerplantcheckbll.SaveForm(checkEntity.Id, checkEntity);
                    }
                    powerplantreformbll.SaveForm(keyValue, entity);
                    return Success("�����ɹ���");
                }
                else
                {
                    return Error("ϵͳ����,����ϵϵͳ����Ա");
                }
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
        }
        #endregion
    }
}
