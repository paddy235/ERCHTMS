using BSFramework.Util.WebControl;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.HighRiskWork;
using ERCHTMS.Busines.JPush;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.DangerousJob;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.IService.DangerousJob;
using ERCHTMS.Service.DangerousJob;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ERCHTMS.Busines.DangerousJob
{
    /// <summary>
    /// 描 述：作业安全证申请
    /// </summary>
    public class JobApprovalFormBLL
    {
        private JobApprovalFormIService service = new JobApprovalFormService();

        private FireWaterBLL firewaterbll = new FireWaterBLL();
        private DangerousJobFlowDetailIService DangerousJobFlowDetailIService = new DangerousJobFlowDetailService();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<JobApprovalFormEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        public DataTable GetAppPageList(Pagination pagination, string queryJson)
        {
            return service.GetAppPageList(pagination, queryJson);
        }

        public DataTable GetPageView(Pagination pagination, string queryJson)
        {
            return service.GetPageView(pagination, queryJson);
        }
        public DataTable GetCardPageList(Pagination pagination, string queryJson)
        {
            return service.GetCardPageList(pagination, queryJson);
        }

        public string IsLedgerSetting(string keyvalue)
        {
            return service.IsLedgerSetting(keyvalue);
        }
        /// <summary>
        /// 获取流程
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public DataTable GetCheckInfo(string keyValue)
        {
            return service.GetCheckInfo(keyValue);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public JobApprovalFormEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public DataTable ConfigurationByWorkList(string keyValue, string moduleno)
        {
            return service.ConfigurationByWorkList(keyValue, moduleno);
        }
        /// <summary>
        /// 获取作业安全证列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<JobApprovalFormEntity> GetJobSafetyCardApplyPageList(Pagination pagination, string queryJson)
        {
            return service.GetJobSafetyCardApplyPageList(pagination, queryJson);
        }
        /// <summary>
        /// 获取作业安全证列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetJobSafetyCardApplyList(Pagination pagination, string queryJson)
        {
            return service.GetJobSafetyCardApplyList(pagination, queryJson);
        }

        /// <summary>
        /// 获取编码名称
        /// </summary>
        /// <param name="type"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public string getDataItemName(string type, string encode)
        {
            string strName = "";
            var entity = new DataItemDetailBLL().GetDataItemListByItemCode("'" + encode + "'").Where(a => a.ItemValue == type).FirstOrDefault();
            if (entity != null)
                strName = entity.ItemName;
            return strName;
        }
        /// <summary>
        /// 根据ID获取作业安全证
        /// </summary>
        /// <param name="userids"></param>
        /// <returns></returns>
        public DataTable GetSafetyCardTable(string[] userids)
        {
            return service.GetSafetyCardTable(userids);
        }
        /// <summary>
        /// 获取手机流程图
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public List<CheckFlowData> GetAppFlowList(string keyValue)
        {
            return service.GetAppFlowList(keyValue);
        }

        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
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
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, JobApprovalFormEntity entity)
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
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, JobApprovalFormEntity entity, List<ManyPowerCheckEntity> data, string arr)
        {
            try
            {
                service.SaveForm(keyValue, entity, data, arr);
                if (entity.IsSubmit == 1)
                {
                    UserBLL userbll = new UserBLL();
                    DangerousJobFlowDetailEntity flow = DangerousJobFlowDetailIService.GetList().Where(t => t.BusinessId == entity.Id && t.Status == 0).FirstOrDefault();
                    string userids = DangerousJobFlowDetailIService.GetCurrentStepUser(entity.Id, flow.Id);
                    DataTable dt = userbll.GetUserTable(userids.Split(','));
                    entity = service.GetEntity(entity.Id);
                    entity.JobLevelName = dataitemdetailbll.GetItemName("DangerousJobCheck", entity.JobLevel);

                    JPushApi.PushMessage(string.Join(",", dt.AsEnumerable().Select(t => t.Field<string>("account")).ToArray()), string.Join(",", dt.AsEnumerable().Select(t => t.Field<string>("realname")).ToArray()), "WXZY001", entity.JobLevelName + "审批单(" + entity.JobTypeName + ")申请待您审批，请您及时处理。", entity.CreateUserName + "于" + entity.CreateDate.Value.ToString("yyyy年MM月dd日") + "申请的" + entity.JobLevelName + "审批单(" + entity.JobTypeName + ")需要您进行审批，请您及时处理。", entity.Id);
                }
                //if (entity.FlowState == "1")
                //{
                //    UserBLL userbll = new UserBLL();
                //    UserEntity userEntity = userbll.GetEntity(entity.DutyPersonId);//获取责任人用户信息
                //    JPushApi.PushMessage(userEntity.Account, entity.DutyPerson, "GZDB001", "例行安全工作", entity.Id);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 开始作业
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="ledgerType"></param>
        /// <param name="type"></param>
        /// <param name="worktime"></param>
        /// <param name="issendmessage"></param>
        /// <param name="conditioncontent"></param>
        /// <param name="conditionid"></param>
        /// <param name="iscomplete"></param>
        public void LedgerOp(string keyValue, string ledgerType, string type, string worktime, string issendmessage, string conditioncontent, string conditionid = "", string iscomplete = "")
        {
            string title = string.Empty;
            string message = string.Empty;
            var time = Convert.ToDateTime(worktime);

            Operator curUser = OperatorProvider.Provider.Current();
            JobApprovalFormEntity entity = service.GetEntity(keyValue);

            string sql = "";
            if (type == "HeightWorking" || type == "Lifting" || type == "Digging" || type == "OpenCircuit" || type == "WhenHot" || type == "BlindPlateWall" || type == "LimitedSpace" || type == "EquOverhaulClean")
            {
                if (ledgerType == "0")
                {
                    sql = string.Format(@"update bis_jobsafetycardapply set realityjobstarttime=to_date('{1}','yyyy-mm-dd hh24:mi:ss')," +
                        "jobstate=10,RealityJobEndTime='' where id='{0}'", keyValue, time.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else if (ledgerType == "1")
                {
                    int jobstate = 9;
                    if (iscomplete == "0")
                    {
                        jobstate = 9;//状态为作业暂停
                    }
                    else
                    {
                        UserBLL userbll = new UserBLL();
                        JobSafetyCardApplyBLL JobSafetyCardApplyBLL = new JobSafetyCardApplyBLL();
                        JobSafetyCardApplyEntity data = JobSafetyCardApplyBLL.GetEntity(keyValue);
                        if (type == "OpenCircuit")
                        {
                            jobstate = 6; //断路作业 结束作业后下一步流程为验收   状态值改为验收中：6
                            DataTable dt = userbll.GetUserTable(data.CheckPersonId.Split(','));
                            JPushApi.PushMessage(string.Join(",", dt.AsEnumerable().Select(t => t.Field<string>("account")).ToArray()), data.CheckPerson, "ZYAQZ004", data.JobTypeName + "安全证待您进行作业后验收，请您及时处理。", data.CreateUserName + "于" + data.CreateDate.Value.ToString("yyyy年MM月dd日") + "申请的" + data.JobTypeName + "安全证需要您进行作业后验收，请您及时处理。", data.Id);
                        }
                        else if (type == "EquOverhaulClean")
                        {
                            jobstate = 7; //设备检修清理作业 结束作业后下一步流程为送电  状态值改为送电中：7
                            DataTable dt = userbll.GetUserTable(data.PowerGivePersonId.Split(','));
                            JPushApi.PushMessage(string.Join(",", dt.AsEnumerable().Select(t => t.Field<string>("account")).ToArray()), data.PowerGivePerson, "ZYAQZ010", data.JobTypeName + "已结束，待您进行送电，请您及时处理。", data.CreateUserName + "于" + data.CreateDate.Value.ToString("yyyy年MM月dd日") + "申请的" + data.JobTypeName + "已结束，需要您进行送电操作，请您及时处理。", data.Id);
                        }
                        else
                        {
                            jobstate = 11; //其他作业  作业结束即为整个流程结束  状态值为11
                        }
                    }
                    sql = string.Format("update bis_jobsafetycardapply set jobstate={2},realityjobendtime=to_date('{1}','yyyy-mm-dd hh24:mi:ss') where id='{0}'",
                        keyValue, time.ToString("yyyy-MM-dd HH:mm:ss"), jobstate);
                }
            }
            else
            {
                if (ledgerType == "0")
                {
                    entity.RealityJobStartTime = time;

                    sql = string.Format(@"update BIS_JobApprovalForm set RealityJobStartTime=to_date('{1}','yyyy-mm-dd hh24:mi:ss')," +
                        "WorkOperate='0',RealityJobEndTime='' where id='{0}'", keyValue, time.ToString("yyyy-MM-dd HH:mm:ss"));
                    entity.WorkOperate = "0";
                    entity.RealityJobEndTime = null;
                }
                if (ledgerType == "1")
                {
                    entity.RealityJobEndTime = time;
                    if (iscomplete == "0")
                    {
                        entity.WorkOperate = "1";
                    }
                    sql = string.Format("update BIS_JobApprovalForm set WorkOperate='{2}',RealityJobEndTime=to_date('{1}','yyyy-mm-dd hh24:mi:ss') where id='{0}'",
                        keyValue, time.ToString("yyyy-MM-dd HH:mm:ss"), entity.WorkOperate);
                }
                //更新作业安全证状态
                UpdateGerOp(keyValue, ledgerType, type, worktime, issendmessage, conditioncontent, conditionid, iscomplete);
            }
            service.UpdateData(sql);
            #region 添加执行情况信息
            FireWaterCondition Conditionentity = new FireWaterCondition();
            Conditionentity.Id = !string.IsNullOrEmpty(conditionid) ? conditionid : "";
            Conditionentity.LedgerType = ledgerType;
            Conditionentity.ConditionTime = time;
            Conditionentity.ConditionContent = conditioncontent;
            Conditionentity.ConditionDept = curUser.DeptName;
            Conditionentity.ConditionDeptCode = curUser.DeptCode;
            Conditionentity.ConditionDeptId = curUser.DeptId;
            Conditionentity.ConditionPerson = curUser.UserName;
            Conditionentity.ConditionPersonId = curUser.UserId;
            Conditionentity.FireWaterId = keyValue;
            firewaterbll.SubmitCondition(conditionid, Conditionentity);

            #endregion


        }

        public void UpdateGerOp(string keyValue, string ledgerType, string type, string worktime, string issendmessage, string conditioncontent, string conditionid = "", string iscomplete = "")
        {

            string title = string.Empty;
            string message = string.Empty;
            var time = Convert.ToDateTime(worktime);
            Operator curUser = OperatorProvider.Provider.Current();
            var entity = service.GetEntity(keyValue);
            var pId = "";
            if (entity != null)
            {
                if (!string.IsNullOrEmpty(entity.JobSafetyCardId))
                {
                    var list = entity.JobSafetyCardId.TrimEnd(',').Split(',');
                    foreach (var item in list)
                    {
                        pId = Guid.NewGuid().ToString();

                        JobSafetyCardApplyService appluservise = new JobSafetyCardApplyService();
                        var aEntity = appluservise.GetEntity(item);
                        string sql = "";
                        if (aEntity.JobType == "HeightWorking" || aEntity.JobType == "Lifting" || aEntity.JobType == "Digging" || aEntity.JobType == "OpenCircuit" || aEntity.JobType == "WhenHot" || aEntity.JobType == "BlindPlateWall" || aEntity.JobType == "LimitedSpace" || aEntity.JobType == "EquOverhaulClean")
                        {
                            //作业证已结束作业后，不记录执行信息（ //送电中/验收中/已结束 危险作业不记录执行信息）
                            if (aEntity.JobState != 11 && aEntity.JobState != 6 && aEntity.JobState != 7)
                            {
                                //开始作业
                                if (ledgerType == "0")
                                {   //关联的作业安全证已经有开始作业的，危险作业审批单开始作业的数据不覆盖作业安全证的开始作业数据
                                    if (aEntity.JobState != 10)
                                    {
                                        sql = string.Format(@"update bis_jobsafetycardapply set realityjobstarttime=to_date('{1}','yyyy-mm-dd hh24:mi:ss')," +
                                            "jobstate=10,RealityJobEndTime='' where id='{0}'", aEntity.Id, time.ToString("yyyy-MM-dd HH:mm:ss"));
                                        service.UpdateData(sql);
                                        #region 添加执行情况信息
                                        FireWaterCondition Conditionentity = new FireWaterCondition();
                                        Conditionentity.Id = pId;
                                        Conditionentity.LedgerType = ledgerType;
                                        Conditionentity.ConditionTime = time;
                                        Conditionentity.ConditionContent = conditioncontent;
                                        Conditionentity.ConditionDept = curUser.DeptName;
                                        Conditionentity.ConditionDeptCode = curUser.DeptCode;
                                        Conditionentity.ConditionDeptId = curUser.DeptId;
                                        Conditionentity.ConditionPerson = curUser.UserName;
                                        Conditionentity.ConditionPersonId = curUser.UserId;
                                        Conditionentity.FireWaterId = aEntity.Id;
                                        firewaterbll.SubmitCondition(pId, Conditionentity);
                                        List<FileInfoEntity> filelist = new FileInfoBLL().GetFileList(conditionid); //现场图片
                                        if (filelist.Count > 0)
                                        {
                                            foreach (var items in filelist)
                                            {
                                                FileInfoEntity fileInfoEntity = new FileInfoEntity();
                                                string fileguid = Guid.NewGuid().ToString();
                                                fileInfoEntity.Create();
                                                fileInfoEntity.RecId = pId; //关联ID
                                                fileInfoEntity.FileName = items.FileName;
                                                fileInfoEntity.FilePath = items.FilePath;
                                                fileInfoEntity.FileSize = items.FileSize;
                                                fileInfoEntity.FileExtensions = items.FileExtensions;
                                                fileInfoEntity.FileType = items.FileType;
                                                fileInfoEntity.FolderId = items.FolderId;
                                                //TransportRemoteToServer(Server.MapPath("~/Resource//PeopleAudit/" + DateTime.Now.ToString("yyyyMMdd") + "/"), decompressionDirectory + item, fileguid + fileinfo.Extension);
                                                new FileInfoBLL().SaveForm("", fileInfoEntity);
                                            }
                                        }
                                        List<FileInfoEntity> filelist2 = new FileInfoBLL().GetFileList(conditionid + "_02");
                                        if (filelist2.Count > 0)
                                        {
                                            foreach (var items in filelist2)
                                            {
                                                FileInfoEntity fileInfoEntity = new FileInfoEntity();
                                                string fileguid = Guid.NewGuid().ToString();
                                                fileInfoEntity.Create();
                                                fileInfoEntity.RecId = pId + "_02"; //关联ID
                                                fileInfoEntity.FileName = items.FileName;
                                                fileInfoEntity.FilePath = items.FilePath;
                                                fileInfoEntity.FileSize = items.FileSize;
                                                fileInfoEntity.FileExtensions = items.FileExtensions;
                                                fileInfoEntity.FileType = items.FileType;
                                                fileInfoEntity.FolderId = items.FolderId;
                                                //TransportRemoteToServer(Server.MapPath("~/Resource//PeopleAudit/" + DateTime.Now.ToString("yyyyMMdd") + "/"), decompressionDirectory + item, fileguid + fileinfo.Extension);
                                                new FileInfoBLL().SaveForm("", fileInfoEntity);
                                            }
                                        }
                                    }
                                    #endregion
                                }

                                else if (ledgerType == "1")
                                {
                                    int jobstate = 9;
                                    //作业中 -作业暂停
                                    if (iscomplete == "0")
                                    {

                                        if (aEntity.JobState != 9)
                                        {
                                            jobstate = 9;//状态为作业暂停
                                            sql = string.Format("update bis_jobsafetycardapply set jobstate={2},realityjobendtime=to_date('{1}','yyyy-mm-dd hh24:mi:ss') where id='{0}'",
                                             aEntity.Id, time.ToString("yyyy-MM-dd HH:mm:ss"), jobstate);
                                            service.UpdateData(sql);
                                            #region 添加执行情况信息
                                            FireWaterCondition Conditionentity = new FireWaterCondition();
                                            Conditionentity.Id = pId;
                                            Conditionentity.LedgerType = ledgerType;
                                            Conditionentity.ConditionTime = time;
                                            Conditionentity.ConditionContent = conditioncontent;
                                            Conditionentity.ConditionDept = curUser.DeptName;
                                            Conditionentity.ConditionDeptCode = curUser.DeptCode;
                                            Conditionentity.ConditionDeptId = curUser.DeptId;
                                            Conditionentity.ConditionPerson = curUser.UserName;
                                            Conditionentity.ConditionPersonId = curUser.UserId;
                                            Conditionentity.FireWaterId = aEntity.Id;
                                            firewaterbll.SubmitCondition(pId, Conditionentity);
                                            List<FileInfoEntity> filelist = new FileInfoBLL().GetFileList(conditionid); //现场图片
                                            if (filelist.Count > 0)
                                            {
                                                foreach (var items in filelist)
                                                {
                                                    FileInfoEntity fileInfoEntity = new FileInfoEntity();
                                                    string fileguid = Guid.NewGuid().ToString();
                                                    fileInfoEntity.Create();
                                                    fileInfoEntity.RecId = pId; //关联ID
                                                    fileInfoEntity.FileName = items.FileName;
                                                    fileInfoEntity.FilePath = items.FilePath;
                                                    fileInfoEntity.FileSize = items.FileSize;
                                                    fileInfoEntity.FileExtensions = items.FileExtensions;
                                                    fileInfoEntity.FileType = items.FileType;
                                                    fileInfoEntity.FolderId = items.FolderId;
                                                    //TransportRemoteToServer(Server.MapPath("~/Resource//PeopleAudit/" + DateTime.Now.ToString("yyyyMMdd") + "/"), decompressionDirectory + item, fileguid + fileinfo.Extension);
                                                    new FileInfoBLL().SaveForm("", fileInfoEntity);
                                                }
                                            }
                                            List<FileInfoEntity> filelist2 = new FileInfoBLL().GetFileList(conditionid + "_02");
                                            if (filelist2.Count > 0)
                                            {
                                                foreach (var items in filelist2)
                                                {
                                                    FileInfoEntity fileInfoEntity = new FileInfoEntity();
                                                    string fileguid = Guid.NewGuid().ToString();
                                                    fileInfoEntity.Create();
                                                    fileInfoEntity.RecId = pId + "_02"; //关联ID
                                                    fileInfoEntity.FileName = items.FileName;
                                                    fileInfoEntity.FilePath = items.FilePath;
                                                    fileInfoEntity.FileSize = items.FileSize;
                                                    fileInfoEntity.FileExtensions = items.FileExtensions;
                                                    fileInfoEntity.FileType = items.FileType;
                                                    fileInfoEntity.FolderId = items.FolderId;
                                                    //TransportRemoteToServer(Server.MapPath("~/Resource//PeopleAudit/" + DateTime.Now.ToString("yyyyMMdd") + "/"), decompressionDirectory + item, fileguid + fileinfo.Extension);
                                                    new FileInfoBLL().SaveForm("", fileInfoEntity);
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                    //作业中 -完成作业
                                    else if (iscomplete == "1")
                                    {
                                        //作业证作业暂停
                                        if (aEntity.JobState != 9)
                                        {
                                            if (aEntity.JobType == "OpenCircuit")
                                            {
                                                jobstate = 6; //断路作业 结束作业后下一步流程为验收   状态值改为验收中：6
                                            }
                                            else if (aEntity.JobType == "EquOverhaulClean")
                                            {
                                                jobstate = 7; //设备检修清理作业 结束作业后下一步流程为送电  状态值改为送电中：7
                                            }
                                            else
                                            {
                                                jobstate = 11; //其他作业  作业结束即为整个流程结束  状态值为11
                                            }
                                            sql = string.Format("update bis_jobsafetycardapply set jobstate={2},realityjobendtime=to_date('{1}','yyyy-mm-dd hh24:mi:ss') where id='{0}'",
                                             aEntity.Id, time.ToString("yyyy-MM-dd HH:mm:ss"), jobstate);
                                            service.UpdateData(sql);
                                            #region 添加执行情况信息
                                            FireWaterCondition Conditionentity = new FireWaterCondition();
                                            Conditionentity.Id = pId;
                                            Conditionentity.LedgerType = ledgerType;
                                            Conditionentity.ConditionTime = time;
                                            Conditionentity.ConditionContent = conditioncontent;
                                            Conditionentity.ConditionDept = curUser.DeptName;
                                            Conditionentity.ConditionDeptCode = curUser.DeptCode;
                                            Conditionentity.ConditionDeptId = curUser.DeptId;
                                            Conditionentity.ConditionPerson = curUser.UserName;
                                            Conditionentity.ConditionPersonId = curUser.UserId;
                                            Conditionentity.FireWaterId = aEntity.Id;
                                            firewaterbll.SubmitCondition(pId, Conditionentity);
                                            #endregion
                                            List<FileInfoEntity> filelist = new FileInfoBLL().GetFileList(conditionid); //现场图片
                                            if (filelist.Count > 0)
                                            {
                                                foreach (var items in filelist)
                                                {
                                                    FileInfoEntity fileInfoEntity = new FileInfoEntity();
                                                    string fileguid = Guid.NewGuid().ToString();
                                                    fileInfoEntity.Create();
                                                    fileInfoEntity.RecId = pId; //关联ID
                                                    fileInfoEntity.FileName = items.FileName;
                                                    fileInfoEntity.FilePath = items.FilePath;
                                                    fileInfoEntity.FileSize = items.FileSize;
                                                    fileInfoEntity.FileExtensions = items.FileExtensions;
                                                    fileInfoEntity.FileType = items.FileType;
                                                    fileInfoEntity.FolderId = items.FolderId;
                                                    //TransportRemoteToServer(Server.MapPath("~/Resource//PeopleAudit/" + DateTime.Now.ToString("yyyyMMdd") + "/"), decompressionDirectory + item, fileguid + fileinfo.Extension);
                                                    new FileInfoBLL().SaveForm("", fileInfoEntity);
                                                }
                                            }
                                            List<FileInfoEntity> filelist2 = new FileInfoBLL().GetFileList(conditionid + "_02");
                                            if (filelist2.Count > 0)
                                            {
                                                foreach (var items in filelist2)
                                                {
                                                    FileInfoEntity fileInfoEntity = new FileInfoEntity();
                                                    string fileguid = Guid.NewGuid().ToString();
                                                    fileInfoEntity.Create();
                                                    fileInfoEntity.RecId = pId + "_02"; //关联ID
                                                    fileInfoEntity.FileName = items.FileName;
                                                    fileInfoEntity.FilePath = items.FilePath;
                                                    fileInfoEntity.FileSize = items.FileSize;
                                                    fileInfoEntity.FileExtensions = items.FileExtensions;
                                                    fileInfoEntity.FileType = items.FileType;
                                                    fileInfoEntity.FolderId = items.FolderId;
                                                    //TransportRemoteToServer(Server.MapPath("~/Resource//PeopleAudit/" + DateTime.Now.ToString("yyyyMMdd") + "/"), decompressionDirectory + item, fileguid + fileinfo.Extension);
                                                    new FileInfoBLL().SaveForm("", fileInfoEntity);
                                                }
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion
        /// <summary>
        /// 获取流程图
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public Flow GetFlow(string KeyValue)
        {
            return service.GetFlow(KeyValue);
        }
        public void ExchangeForm(string keyValue, string TransferUserName, string TransferUserAccount, string TransferUserId)
        {
            try
            {
                var entity = service.GetEntity(keyValue);
                service.ExchangeForm(keyValue, TransferUserName, TransferUserAccount, TransferUserId);

                UserBLL userbll = new UserBLL();
                DangerousJobFlowDetailEntity flow = DangerousJobFlowDetailIService.GetList().Where(t => t.BusinessId == entity.Id && t.Status == 0).FirstOrDefault();
                string userids = DangerousJobFlowDetailIService.GetCurrentStepUser(entity.Id, flow.Id);
                DataTable dt = userbll.GetUserTable(userids.Split(','));

                entity.JobLevelName = dataitemdetailbll.GetItemName("DangerousJobCheck", entity.JobLevel);

                JPushApi.PushMessage(string.Join(",", dt.AsEnumerable().Select(t => t.Field<string>("account")).ToArray()),
                    string.Join(",", dt.AsEnumerable().Select(t => t.Field<string>("realname")).ToArray()), "WXZY004", "【" + entity.JobLevelName + "】审批单（" + entity.JobTypeName + "）的审批已转交给您，请您及时处理。", "【" + entity.JobLevelName + "】审批单（" + entity.JobTypeName + "）的审批已由【" + entity.CreateUserName + "】转交给您，请您及时处理。", entity.Id);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string DangerousJobLevelCount(string starttime, string endtime, string deptid, string deptcode)
        {
            return service.DangerousJobLevelCount(starttime, endtime, deptid, deptcode);
        }

        public string DangerousJobLevelList(string starttime, string endtime, string deptid, string deptcode)
        {
            return service.DangerousJobLevelList(starttime, endtime, deptid, deptcode);
        }
    }
}
