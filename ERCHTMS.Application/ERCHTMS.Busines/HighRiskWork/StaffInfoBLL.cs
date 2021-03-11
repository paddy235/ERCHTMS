using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using ERCHTMS.Service.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Busines.SystemManage;
using System.Net;

namespace ERCHTMS.Busines.HighRiskWork
{
    /// <summary>
    /// 描 述：任务分配人员
    /// </summary>
    public class StaffInfoBLL
    {
        private StaffInfoIService service = new StaffInfoService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<StaffInfoEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public StaffInfoEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 获取监督任务列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public DataTable GetDataTable(Pagination page, string queryJson)
        {
            return service.GetDataTable(page, queryJson);
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
        public string SaveForm(string keyValue, StaffInfoEntity model)
        {
            try
            {

                keyValue = service.SaveForm(keyValue, model);
                return keyValue;
            }
            catch (Exception ex)
            {
                return "0";
            }
        }


        #region  对接班组(定时执行)
        public void SendTaskInfo()
        {
            try
            {
                string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                var tasklist = new TeamsInfoBLL().GetAllList(string.Format(" and dataissubmit='1' and teamstarttime<=to_date('{1}','yyyy-mm-dd hh24:mi:ss') and  teamendtime>=to_date('{0}','yyyy-mm-dd hh24:mi:ss') and (isaccomplish!='1' or isaccomplish is null)", DateTime.Now.ToString("yyyy-MM-dd 00:00:00"), DateTime.Now.ToString("yyyy-MM-dd 23:59:59")));
                List<object> datas = new List<object>();
                foreach (TeamsInfoEntity item in tasklist)
                {
                    string[] arr = item.WorkInfoId.Split(',');
                    for (int i = 0; i < arr.Length; i++)
                    {
                        var workentiy = new SuperviseWorkInfoBLL().GetEntity(arr[i]);
                        StaffInfoEntity staff = new StaffInfoEntity();
                        staff.Id = Guid.NewGuid().ToString();
                        staff.PTeamName = item.TeamName;
                        staff.PTeamCode = item.TeamCode;
                        staff.PTeamId = item.TeamId;
                        staff.PStartTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
                        staff.PEndTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));
                        staff.SumTimeStr = 0;
                        staff.TaskLevel = "1";
                        staff.DataIsSubmit = "0";
                        staff.SuperviseState = "0";//所有子项监督完成为1(已监督)
                        staff.TaskShareId = item.TaskShareId;
                        staff.IsSynchronization = "1";//同步数据
                        staff.WorkInfoId = workentiy.Id;
                        staff.WorkInfoName = workentiy.WorkName;
                        staff.CreateUserId = item.CreateUserId;
                        staff.CreateUserName = item.CreateUserName;
                        staff.CreateUserDeptCode = item.CreateUserDeptCode;
                        staff.CreateUserOrgCode = item.CreateUserOrgCode;
                        string result = new StaffInfoBLL().SaveForm("", staff);
                        if (result != "0")
                        {
                            var tempdata = new
                            {
                                Job = workentiy.WorkName + "旁站监督任务",
                                StartTime = staff.PStartTime,
                                EndTime = staff.PEndTime,
                                JobProject1 = string.IsNullOrEmpty(workentiy.WorkContent) ? "" : workentiy.WorkContent,//作业内容
                                JobDept = string.IsNullOrEmpty(workentiy.WorkDeptName) ? "" : workentiy.WorkDeptName,//作业单位
                                JobCategory = string.IsNullOrEmpty(workentiy.WorkInfoType) ? "" : workentiy.WorkInfoType,//作业类型
                                JobProject2 = string.IsNullOrEmpty(workentiy.EngineeringName) ? "" : workentiy.EngineeringName,//工程名称
                                JobNo = string.IsNullOrEmpty(workentiy.WorkTicketNo) ? "" : workentiy.WorkTicketNo,//工作票号
                                JobAddr = string.IsNullOrEmpty(workentiy.WorkPlace) ? "" : workentiy.WorkPlace,//作业地点
                                RecId = staff.Id,//主键id
                                GroupId = item.TeamId //班组id
                            };
                            datas.Add(tempdata);
                        }
                        else
                        {
                            System.IO.File.AppendAllText(new DataItemDetailBLL().GetItemValue("imgPath") + "/logs/" + fileName, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：异常数据为:" + Newtonsoft.Json.JsonConvert.SerializeObject(staff) + "\r\n");
                        }

                    }
                }
                WebClient wc = new WebClient();
                wc.Credentials = CredentialCache.DefaultCredentials;
                //发送请求到web api并获取返回值，默认为post方式
                System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                nc.Add("json", Newtonsoft.Json.JsonConvert.SerializeObject(datas));
                // wc.UploadValuesCompleted += wc_UploadValuesCompleted1;
                System.IO.File.AppendAllText(new DataItemDetailBLL().GetItemValue("imgPath") + "/logs/" + fileName, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：同步成功,数据为:" + Newtonsoft.Json.JsonConvert.SerializeObject(datas) + "\r\n");

                wc.UploadValuesAsync(new Uri(new DataItemDetailBLL().GetItemValue("bzurl") + "PostMonitorJob"), nc);
            }
            catch (Exception ex)
            {
                //将同步结果写入日志文件
                string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                System.IO.File.AppendAllText(new DataItemDetailBLL().GetItemValue("imgPath") + "/logs/" + fileName, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：数据失败" + ",异常信息：" + ex.Message + "\r\n");
            }
        }
        #endregion
        #endregion
    }
}
