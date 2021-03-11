using BSFramework.Util.WebControl;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.KbsDeviceManage;
using ERCHTMS.Busines.MatterManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.Entity.OccupationalHealthManage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BSFramework.Util;
using ERCHTMS.AppSerivce.Models;
using ERCHTMS.Entity.SystemManage;

namespace ERCHTMS.AppSerivce.Controllers
{
    /// <summary>
    /// 描述：康巴什现场作业和预警
    /// </summary>
    public class KbsDeviceManageController : BaseApiController
    {
        private SafeworkcontrolBLL safeworkcontrolbll = new SafeworkcontrolBLL();
        private OperticketmanagerBLL Opertickebll = new OperticketmanagerBLL();
        private DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();

        #region 获取数据
        /// <summary>
        /// 获取所有没结束的现场作业信息（定时服务）
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetStartList([FromBody]JObject json)
        {
            try
            {
                var list = safeworkcontrolbll.GetList("").Where(a => a.State != 2).ToList();
                return new { Code = 0, Count = 1, Info = "获取数据成功", data = list };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 现场作业实时管控列表信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetWorkRealTimeListJson([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string Taskname = dy.Taskname;//
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();

                var list = safeworkcontrolbll.GetList("").Where(a => a.Taskname.Contains(Taskname)).ToList();
                return new { Code = 0, Count = list.Count, Info = "获取数据成功", data = list };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 获取现场作业列表信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetWorkListJson([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string state = dy.type;//0保存任务 1开始任务 2结束任务
                string taskType = dy.data.taskType;
                string Taskregioncode = dy.data.Taskregioncode;
                //string deptcode = dy.data.deptcode;
                string stime = dy.data.startTime;
                string etime = dy.data.endTime;

                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                //获取页数和条数
                int pageSize = int.Parse(dy.data.pagesize.ToString()); //每页的记录数
                if (pageSize == 0) pageSize = 20;
                int pagenum = int.Parse(dy.data.pagenum.ToString());  //当前页索引
                Pagination pagination = new Pagination();

                pagination.p_kid = "ID";
                pagination.p_fields = "d.tasktype,d.taskname,d.taskregionname,d.taskregioncode,d.actualstarttime,d.actualendtime,d.planendtime,d.taskmembername,d.state";
                pagination.p_tablename = @" bis_safeworkcontrol d ";
                pagination.conditionJson = " 1=1 ";
                pagination.records = 0;
                pagination.page = pagenum;//页数
                pagination.rows = pageSize;//行数
                pagination.sidx = "Createdate";//排序字段
                pagination.sord = "desc";

                if (!string.IsNullOrEmpty(state))
                {
                    switch (state)
                    {
                        case "0"://未提交
                            pagination.conditionJson += " and state=0 ";
                            break;
                        case "1"://已提交
                            pagination.conditionJson += " and (state=1 or state=2) ";
                            break;
                        default:
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(taskType))
                {//作业类型
                    pagination.conditionJson += string.Format(" and taskType = '{0}' ", taskType);
                }
                if (!string.IsNullOrEmpty(Taskregioncode))
                {//作业区域
                    pagination.conditionJson += string.Format(" and  instr(Taskregioncode,'{0}')=1", Taskregioncode);
                }
                //根据时间进行筛选
                if (!string.IsNullOrEmpty(stime) || !string.IsNullOrEmpty(etime))
                {
                    if (!string.IsNullOrEmpty(stime))
                    {
                        pagination.conditionJson +=
                            string.Format(
                                " and ActualStartTime >= TO_Date('{0}','yyyy-mm-dd hh24:mi:ss')", stime + " 00:00:00");
                    }
                    if (!string.IsNullOrEmpty(etime))
                    {
                        pagination.conditionJson +=
                            string.Format(
                                " and ActualStartTime <= TO_Date('{0}','yyyy-mm-dd hh24:mi:ss')", etime + " 23:59:59");
                    }
                }
                var dt = safeworkcontrolbll.GetPageList(pagination, null);
                return new { Code = 0, Count = dt.Rows.Count, Info = "获取数据成功", data = dt };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }


        /// <summary>
        /// 获取现场作业台账列表信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetWorkStandingBookListJson([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string state = dy.type;//0保存任务 1开始任务 2结束任务
                string taskType = dy.data.taskType;
                string Taskregioncode = dy.data.Taskregioncode;
                string deptcode = dy.data.deptcode;
                string stime = dy.data.startTime;
                string etime = dy.data.endTime;

                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                //获取页数和条数
                int pageSize = int.Parse(dy.data.pagesize.ToString()); //每页的记录数
                if (pageSize == 0) pageSize = 20;
                int pagenum = int.Parse(dy.data.pagenum.ToString());  //当前页索引
                Pagination pagination = new Pagination();
                pagination.p_kid = "ID";
                pagination.p_fields = "d.tasktype,d.taskname,d.taskregionname,d.taskregioncode,d.actualstarttime,d.actualendtime,d.planendtime,d.taskmembername,d.state";
                pagination.p_tablename = @" bis_safeworkcontrol d ";
                pagination.conditionJson = " 1=1 ";
                pagination.records = 0;
                pagination.page = pagenum;//页数
                pagination.rows = pageSize;//行数
                pagination.sidx = "Createdate";//排序字段
                pagination.sord = "desc";

                if (!string.IsNullOrEmpty(state))
                {
                    switch (state)
                    {
                        case "0"://未完成任务
                            pagination.conditionJson += " and (state=0 or state=1) ";
                            break;
                        case "1"://已完成任务
                            pagination.conditionJson += " and state=2 ";
                            break;
                        default:
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(taskType))
                {//作业类型
                    pagination.conditionJson += string.Format(" and taskType = '{0}' ", taskType);
                }
                if (!string.IsNullOrEmpty(Taskregioncode))
                {//作业区域
                    pagination.conditionJson += string.Format(" and  instr(Taskregioncode,'{0}')=1", Taskregioncode);
                }
                if (!string.IsNullOrEmpty(deptcode))
                {//部门
                    pagination.conditionJson += string.Format(" and  instr(deptcode,'{0}')=1", deptcode);
                }
                //根据时间进行筛选
                if (!string.IsNullOrEmpty(stime) || !string.IsNullOrEmpty(etime))
                {
                    if (!string.IsNullOrEmpty(stime))
                    {
                        pagination.conditionJson +=
                            string.Format(
                                " and ActualStartTime >= TO_Date('{0}','yyyy-mm-dd hh24:mi:ss')  ", stime + " 00:00:00");
                    }
                    if (!string.IsNullOrEmpty(etime))
                    {
                        pagination.conditionJson +=
                            string.Format(
                                " and ActualStartTime <= TO_Date('{0}','yyyy-mm-dd hh24:mi:ss')", etime + " 23:59:59");
                    }
                }
                var dt = safeworkcontrolbll.GetPageList(pagination, null);
                return new { Code = 0, Count = dt.Rows.Count, Info = "获取数据成功", data = dt };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 获取作业类型
        /// </summary>
        /// <param name="itemcode"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetSafeworkTypeItemJson([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();

                List<ComboxsEntity> Rlist = new List<ComboxsEntity>();
                DataItemDetailBLL pdata = new DataItemDetailBLL();
                var list = pdata.GetDataItemListByItemCode("SafeWorkType");
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        ComboxsEntity y1 = new ComboxsEntity();
                        y1.itemName = item.ItemName;
                        y1.itemValue = item.ItemValue;
                        y1.Key = item.SimpleSpelling;
                        Rlist.Add(y1);
                    }
                }
                return new { Code = 0, Count = 1, Info = "获取数据成功", data = Rlist };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 获取现场作业预警类型统计
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetWorkWarningListJson([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();

                string sql = string.Format("select  noticetype, count(1) as num  from bis_EarlyWarning d where d.type=0 and d.state=0  group by d.noticetype order by noticetype asc");
                DataTable dt = Opertickebll.GetDataTable(sql);
                int num1 = 0; int num2 = 0; int num3 = 0;
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        int type = Convert.ToInt32(dt.Rows[i][0].ToString());
                        int Count = Convert.ToInt32(dt.Rows[i][1].ToString());
                        switch (type)
                        {
                            case 0:
                                num1 = Count;
                                break;
                            case 1:
                                num2 = Count;
                                break;
                            case 2:
                                num3 = Count;
                                break;
                            default:
                                break;
                        }
                    }
                }
                var mode = new
                {
                    ManageNotonDuty = num1,
                    MemberNotonDuty = num2,
                    LsolationArea = num3
                };
                return new { Code = 0, Count = dt.Rows.Count, Info = "获取数据成功", data = mode };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 根据通知类型获取对应现场工作预警列表信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetGetWorkWarningStateListJson([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                int type = Convert.ToInt32(dy.type);
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                var list = safeworkcontrolbll.GetWarningInfoList(0).Where(a => a.State == 0 && a.NoticeType == type).ToList(); ;
                return new { Code = 0, Count = list.Count, Info = "获取数据成功", data = list };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }


        /// <summary>
        /// 获取人员管控预警类型统计
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetUserWarningListJson([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                string sql = string.Format("select  noticetype, count(1) as num  from bis_EarlyWarning d where d.type=1 and d.state=0  group by d.noticetype order by noticetype asc");
                DataTable dt = Opertickebll.GetDataTable(sql);
                int num1 = 0; int num2 = 0; int num3 = 0;
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        int type = Convert.ToInt32(dt.Rows[i][0].ToString());
                        int Count = Convert.ToInt32(dt.Rows[i][1].ToString());
                        switch (type)
                        {
                            case 0:
                                num1 = Count;
                                break;
                            case 1:
                                num2 = Count;
                                break;
                            case 2:
                                num3 = Count;
                                break;
                            default:
                                break;
                        }
                    }
                }
                var mode = new
                {
                    DontMove = num1,
                    SoSNum = num2,
                    LsolationArea = num3
                };
                return new { Code = 0, Count = dt.Rows.Count, Info = "获取数据成功", data = mode };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 根据通知类型获取对应人员管控预警列表信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetGetUserWarningStateListJson([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                int type = Convert.ToInt32(dy.type);
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                var list = safeworkcontrolbll.GetWarningInfoList(1).Where(a => a.State == 0 && a.NoticeType == type).ToList(); ;
                return new { Code = 0, Count = list.Count, Info = "获取数据成功", data = list };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 获取下拉选项，通用
        /// </summary>
        /// <param name="parameterModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ListModel<DataItemDetailEntity> GetElements(ParameterModel<string> parameterModel)
        {
            var list = dataItemDetailBLL.GetListItems(parameterModel.Data);
            return new ListModel<DataItemDetailEntity> { Success = true, data = list == null ? null : list.ToList() };
        }

        #endregion

        #region 提交数据

        /// <summary>
        /// 添加、修改现场作业信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object AddSceneWorkIofo([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                Safeworkcontro dy = JsonConvert.DeserializeObject<Safeworkcontro>(res);
                string userId = dy.userId;
                string pid = dy.data.ID;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                SafeworkcontrolEntity entity = new SafeworkcontrolEntity();
                var data = safeworkcontrolbll.GetEntity(pid);
                if (data != null)
                {
                    data.Workno = dy.data.Workno;
                    data.Taskname = dy.data.Taskname;
                    data.Tasktype = dy.data.Tasktype;
                    data.Taskregionname = dy.data.Taskregionname;
                    data.Taskregioncode = dy.data.Taskregioncode;
                    data.Taskregionid = dy.data.Taskregionid;
                    data.Taskcontent = dy.data.Taskcontent;
                    data.Deptname = dy.data.Deptname;
                    data.Deptcode = dy.data.Deptcode;
                    data.Deptid = dy.data.Deptid;
                    data.Taskmanagename = dy.data.Taskmanagename;
                    data.Taskmanageid = dy.data.Taskmanageid;
                    data.Taskmembername = dy.data.Taskmembername;
                    data.Taskmemberid = dy.data.Taskmemberid;
                    data.Guardianname = dy.data.Guardianname;
                    data.Guardianid = dy.data.Guardianid;
                    data.Actualstarttime = dy.data.Actualstarttime;
                    data.Quarantinename = dy.data.Quarantinename;
                    data.Quarantinecode = dy.data.Quarantinecode;
                    data.Quarantineid = dy.data.Quarantineid;
                    data.Takeeffecttime = dy.data.Takeeffecttime;
                    data.Areastate = dy.data.Areastate;
                    data.Areacode = dy.data.Areacode;
                    data.Radius = dy.data.Radius;
                    data.State = dy.data.State;
                    data.DangerLevel = dy.data.DangerLevel;

                    if (data.State == 2)
                    {
                        data.State = 2;
                        data.ActualEndTime = dy.data.ActualEndTime;
                        data.Invalidtime = dy.data.Invalidtime;
                        safeworkcontrolbll.SaveForm(data.ID, data);
                        //将现场工作信息同步到后台计算服务中
                        RabbitMQHelper rh = RabbitMQHelper.CreateInstance();
                        SendData sd = new SendData();
                        sd.DataName = "DelSafeworkcontrolEntity";
                        sd.EntityString = entity.ID;
                        rh.SendMessage(JsonConvert.SerializeObject(sd));
                    }
                    else
                    {
                        if (data.State == 1)
                            data.comerid = GetElectricFenceCameraList(data);
                        safeworkcontrolbll.SaveForm(data.ID, data);
                        //将现场工作信息同步到后台计算服务中
                        RabbitMQHelper rh = RabbitMQHelper.CreateInstance();
                        SendData sd = new SendData();
                        sd.DataName = "UppSafeworkcontrolEntity";
                        sd.EntityString = JsonConvert.SerializeObject(data);
                        if (data.State == 1)
                            rh.SendMessage(JsonConvert.SerializeObject(sd));
                    }
                }
                else
                {
                    if (dy.data.State == 1)
                        dy.data.comerid = GetElectricFenceCameraList(dy.data);
                    dy.data.Create();
                    safeworkcontrolbll.AppSaveForm("", dy.data);
                    //将现场工作信息同步到后台计算服务中
                    RabbitMQHelper rh = RabbitMQHelper.CreateInstance();
                    SendData sd = new SendData();
                    sd.DataName = "AddSafeworkcontrolEntity";
                    sd.EntityString = JsonConvert.SerializeObject(dy.data);
                    if (dy.data.State == 1)
                        rh.SendMessage(JsonConvert.SerializeObject(sd));
                }
                return new { Code = 0, Count = 1, Info = "操作成功！" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 结束现场工作任务
        /// </summary>
        /// <returns></returns>
        public object WorkEedInfo([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string pid = dy.Id;
                string ActualEndTime = dy.data.ActualEndTime;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();

                var entity = safeworkcontrolbll.GetEntity(pid);
                if (entity != null)
                {
                    entity.State = 2;
                    entity.ActualEndTime = Convert.ToDateTime(ActualEndTime);
                    entity.Invalidtime = entity.ActualEndTime;
                    safeworkcontrolbll.SaveForm(entity.ID, entity);
                    //将现场工作信息同步到后台计算服务中
                    RabbitMQHelper rh = RabbitMQHelper.CreateInstance();
                    SendData sd = new SendData();
                    sd.DataName = "DelSafeworkcontrolEntity";
                    sd.EntityString = entity.ID;
                    rh.SendMessage(JsonConvert.SerializeObject(sd));
                }
                return new { Code = 0, Count = 1, Info = "操作成功！" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 获取现场作业详情信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetSceneWorkInfo([FromBody] JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string Id = dy.Id;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();

                SafeworkcontrolEntity entity = safeworkcontrolbll.GetEntity(Id);
                return new { Code = 0, Count = 1, Info = "操作成功！", data = entity };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 添加预警信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object AddEarlyWarningRecord([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //string userId = dy.userid;
                int type = Convert.ToInt32(dy.type);//预警类型（0现场作业 1人员 2设备离线）
                string WarningContent = dy.WarningContent;//预警内容
                string LiableName = dy.LiableName;//责任人
                string LiableId = dy.LiableId;
                string BaseId = dy.BaseId;//现场作业记录Id或人员Id
                string deptCode = dy.deptCode;
                string deptName = dy.deptName;
                int NoticeType = Convert.ToInt32(dy.NoticeType);
                string TaskName = res.Contains("TaskName") ? dy.TaskName : ""; //现场作业添加预警时把“作业名称”传过来

                //获取用户基本信息
                OperatorProvider.AppUserId = "System";  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                WarningInfoEntity entity = new WarningInfoEntity();
                entity.type = type;
                entity.WarningContent = WarningContent;
                entity.WarningTime = DateTime.Now;
                entity.LiableName = LiableName;
                entity.LiableId = LiableId;
                entity.BaseId = BaseId;
                entity.CREATEDATE = DateTime.Now;
                entity.CREATEUSERID = "System";
                entity.ID = Guid.NewGuid().ToString();
                entity.deptCode = deptCode;
                entity.deptName = deptName;
                entity.TaskName = TaskName;
                entity.NoticeType = NoticeType;
                entity.Remark = deptCode;
                entity.State = 0;//设备预警时 0离线1在线
                safeworkcontrolbll.SaveWarningInfoForm("", entity);
                return new { Code = 0, Count = 1, Info = "操作成功！" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 修改预警信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object UpdateEarlyWarningRecord([FromBody]JObject json)
        {
            try
            {
                string str = "操作成功！";
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string BaseId = dy.BaseId;//现场作业记录Id或人员Id
                int type = Convert.ToInt32(dy.type);//预警类型（0现场作业 1人员 2设备离线）
                int NoticeType = Convert.ToInt32(dy.NoticeType);
                string LiableId = dy.LiableId;//责任人
                var entity = safeworkcontrolbll.GetWarningAllList().Where(a => a.type == type && a.State == 0 && a.NoticeType == NoticeType && a.BaseId == BaseId && a.LiableId == LiableId).OrderByDescending(a => a.CREATEDATE).FirstOrDefault();
                if (entity != null)
                {
                    entity.State = 1;//设备预警时 0离线1在线
                    entity.MODIFYDATE = DateTime.Now;
                    safeworkcontrolbll.SaveWarningInfoForm(entity.ID, entity);
                }
                else
                {
                    str = "没找到对应记录信息！";
                }
                return new { Code = 0, Count = 0, Info = str };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 查看预警列表信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetEarlyWarningRecord([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string state = dy.type;//0工作预警 1人员预警
                string deptcode = dy.data.deptcode;
                string stime = dy.data.startTime;
                string etime = dy.data.endTime;

                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                //获取页数和条数
                int pageSize = int.Parse(dy.data.pagesize.ToString()); //每页的记录数
                if (pageSize == 0) pageSize = 20;
                int pagenum = int.Parse(dy.data.pagenum.ToString());  //当前页索引
                Pagination pagination = new Pagination();
                pagination.p_kid = "ID";
                pagination.p_fields = "taskname,warningcontent,liablename,baseid,warningtime,type,deptname,deptcode,remark,liableid,state,noticetype";
                pagination.p_tablename = @" bis_EarlyWarning d ";
                pagination.conditionJson = " 1=1 ";
                pagination.records = 0;
                pagination.page = pagenum;//页数
                pagination.rows = pageSize;//行数
                pagination.sidx = "Createdate";//排序字段
                pagination.sord = "desc";
                if (!string.IsNullOrEmpty(state))
                {//预警类型 0工作预警 1人员预警
                    pagination.conditionJson += string.Format("  and type={0} ", state);
                }
                if (!string.IsNullOrEmpty(deptcode))
                {//部门
                    pagination.conditionJson += string.Format(" and  instr(deptcode,'{0}')=1", deptcode);
                }
                //根据时间进行筛选
                if (!string.IsNullOrEmpty(stime) || !string.IsNullOrEmpty(etime))
                {
                    string startTime = stime;
                    string endTime = etime;
                    if (!string.IsNullOrEmpty(startTime))
                    {
                        pagination.conditionJson += string.Format(
                            " and warningtime >= TO_Date('{0} 00:00:00','yyyy-mm-dd') ", startTime);
                    }

                    if (!string.IsNullOrEmpty(endTime))
                    {
                        pagination.conditionJson += string.Format(
                            " and warningtime <= TO_Date('{0} 23:59:59','yyyy-mm-dd') ", endTime);
                    }
                    //pagination.conditionJson +=
                    //    string.Format(
                    //        " and warningtime >= TO_Date('{0}','yyyy-mm-dd') and  warningtime <= TO_Date('{1}','yyyy-mm-dd') ",
                    //        startTime, endTime);
                }
                var dt = safeworkcontrolbll.GetUserPageList(pagination, null);
                return new { Code = 0, Count = dt.Rows.Count, Info = "获取数据成功", data = dt };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }


        /// <summary>
        /// 获取离线设备预警类型统计
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetEquipmentWarningListJson([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                string sql = string.Format("select  noticetype, count(1) as num  from bis_EarlyWarning d where d.type=2 and d.state=0  group by d.noticetype order by noticetype asc");
                DataTable dt = Opertickebll.GetDataTable(sql);
                int num1 = 0; int num2 = 0; int num3 = 0;
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        int type = Convert.ToInt32(dt.Rows[i][0].ToString());
                        int Count = Convert.ToInt32(dt.Rows[i][1].ToString());
                        switch (type)
                        {
                            case 0:
                                num1 = Count;
                                break;
                            case 1:
                                num2 = Count;
                                break;
                            case 2:
                                num3 = Count;
                                break;
                            default:
                                break;
                        }
                    }
                }
                var mode = new
                {
                    BaseStationNum = num1,
                    CameraNum = num2,
                    LableNum = num3
                };
                return new { Code = 0, Count = dt.Rows.Count, Info = "获取数据成功", data = mode };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 获取设备离线预警列表信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetEquipmentStateListJson([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                int type = Convert.ToInt32(dy.NoticeType);// 0基站离线 1摄像头离线 2标签电量低

                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                var list = safeworkcontrolbll.GetWarningInfoList(2).Where(a => a.State == 0 && a.NoticeType == type).ToList();
                return new { Code = 0, Count = list.Count, Info = "获取数据成功", data = list };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 查看设备离线详情信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetEquipmentInfo([FromBody]JObject json)
        {
            try
            {
                DataTable dt = new DataTable();
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                int type = Convert.ToInt32(dy.data.NoticeType);
                string pid = dy.data.BaseId;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                string sql = string.Empty;
                switch (type)
                {
                    case 0://基站管理
                        sql = string.Format(@"select d.id,
                        d.StationID,
                        StationName,
                        d.StationType,
                        d.areaname,
                        d.FloorCode,
                        d.StationCode,
                        d.StationIP,
                        d.State,
                        e.warningtime
                        from bis_basestation d
                        left join （select max(e.warningtime) as warningtime, t.id
                        from bis_basestation t
                        join bis_EarlyWarning e on t.id = e.baseid
                        group by t.id, e.baseid）e on d.id = e.id
                        where d.id = '{0}'
                        order by warningtime desc", pid);
                        var data = Opertickebll.GetDataTable(sql);
                        if (data.Rows.Count > 0) { dt = data; }
                        break;
                    case 1://摄像头管理
                        sql = string.Format(@"select d.id,
                        d.CameraId,
                        CameraName,
                        d.CameraType,
                        d.AreaName,
                        d.FloorNo,
                        d.CameraPoint,
                        d.CameraIP,
                        d.State,
                        e.warningtime
                        from bis_kbscameramanage d
                        left join （select max(e.warningtime) as warningtime, t.id
                        from bis_kbscameramanage t
                        join bis_EarlyWarning e on t.id = e.baseid
                        group by t.id, e.baseid）e on d.id = e.id
                        where d.id = '{0}'
                        order by warningtime desc", pid);
                        var data1 = Opertickebll.GetDataTable(sql);
                        if (data1.Rows.Count > 0) { dt = data1; }
                        break;
                    case 2://标签管理
                        sql = string.Format(@"
                        select d.id,
                        d.LableId,
                        LableTypeName,
                        d.Name,
                        d.IdCardOrDriver,
                        d.Phone,
                        d.DeptName,
                        d.Power,
                        d.State,
                        e.warningtime
                        from bis_lablemanage d
                        left join （select max(e.warningtime) as warningtime, t.id
                        from bis_lablemanage t
                        join bis_EarlyWarning e on t.id = e.baseid
                        group by t.id, e.baseid）e on d.id = e.id
                        where d.id = '{0}'
                        order by warningtime desc", pid);
                        var data2 = Opertickebll.GetDataTable(sql);
                        if (data2.Rows.Count > 0) { dt = data2; }
                        break;
                    default:
                        break;
                }

                return new { Code = 0, Count = dt.Rows.Count, Info = "获取数据成功", data = dt };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 设备基础预警列表信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetBasiceWarningListJson([FromBody]JObject json)
        {
            try
            {
                string where = "";
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string type = dy.data.type;//0基站离线 1摄像头离线 2标签电量低
                string faultState = dy.data.faultState;
                string deptcode = dy.data.deptcode;
                string stime = dy.data.startTime;
                string etime = dy.data.endTime;

                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                //获取页数和条数
                int pageSize = int.Parse(dy.data.pagesize.ToString()); //每页的记录数
                if (pageSize == 0) pageSize = 20;
                int pagenum = int.Parse(dy.data.pagenum.ToString());  //当前页索引
                Pagination pagination = new Pagination();
                pagination.p_kid = "ee.baseid";
                pagination.p_fields = "warningcontent,warningtime,num,noticetype,state";
                pagination.conditionJson = " 1=1 ";
                pagination.records = 0;
                pagination.page = pagenum;//页数
                pagination.rows = pageSize;//行数
                pagination.sidx = "ee.warningtime";//排序字段
                pagination.sord = "desc";

                if (!string.IsNullOrEmpty(type))
                {//设备类型 0基站离线 1摄像头离线 2标签电量低
                    where += string.Format("  and noticetype={0} ", type);
                }
                if (!string.IsNullOrEmpty(faultState))
                {//故障或正常
                    where += string.Format(" and state={0} ", faultState);
                }
                if (!string.IsNullOrEmpty(deptcode))
                {//区域筛选
                    where += string.Format(" and  instr(deptcode,'{0}')=1", deptcode);
                }
                //根据时间进行筛选
                if (!string.IsNullOrEmpty(stime) || !string.IsNullOrEmpty(etime))
                {
                    if (!string.IsNullOrEmpty(stime))
                    {
                        pagination.conditionJson +=
                            string.Format(
                                " and warningtime >=TO_Date('{0}','yyyy-mm-dd hh24:mi:ss')  ", stime + " 00:00:00");
                    }
                    if (!string.IsNullOrEmpty(etime))
                    {
                        pagination.conditionJson +=
                            string.Format(
                                " and warningtime <= TO_Date('{0}','yyyy-mm-dd hh24:mi:ss')  ", etime + " 23:59:59");
                    }
                }
                pagination.p_tablename = @"( select d.baseid,d.warningcontent,max(d.warningtime) as warningtime,count(1) as num,d.noticetype,'' state from bis_EarlyWarning d where d.type=2 " + where + "  group by d.baseid,d.warningcontent,d.noticetype) ee ";
                var dt = safeworkcontrolbll.GetPageList(pagination, null);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var noticetype = dt.Rows[i][4].ToString();
                        var pid = dt.Rows[i][0].ToString();
                        switch (noticetype)
                        {
                            case "0":
                                var data = new BaseStationBLL().GetEntity(pid);
                                if (data != null) dt.Rows[i][5] = data.State;
                                break;
                            case "1":
                                var data1 = new KbscameramanageBLL().GetEntity(pid);
                                if (data1 != null) dt.Rows[i][5] = data1.State;
                                break;
                            case "2":
                                var data2 = new LablemanageBLL().GetEntity(pid);
                                if (data2 != null) dt.Rows[i][5] = data2.State;
                                break;
                            default:
                                break;
                        }
                    }
                }
                return new { Code = 0, Count = dt.Rows.Count, Info = "获取数据成功", data = dt };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 设备基础预警详细信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetBasiceWarningInfoJson([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string noticetype = dy.data.noticetype;
                string pid = dy.data.BaseId;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                switch (noticetype)
                {
                    case "0":
                        var data = new BaseStationBLL().GetEntity(pid);
                        return new { Code = 0, Count = 1, Info = "获取数据成功", data = data };
                    case "1":
                        var data1 = new KbscameramanageBLL().GetEntity(pid);
                        return new { Code = 0, Count = 1, Info = "获取数据成功", data = data1 };
                    case "2":
                        var data2 = new LablemanageBLL().GetEntity(pid);
                        return new { Code = 0, Count = 1, Info = "获取数据成功", data = data2 };
                    default:
                        break;
                }
                return new { Code = 0, Count = 1, Info = "获取数据成功", data = "" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 设备基础预警故障记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetWarningRecordJson([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string pid = dy.data.BaseId;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                var data = safeworkcontrolbll.GetWarningAllList().Where(a => a.BaseId == pid).ToList();
                return new { Code = 0, Count = 1, Info = "获取数据成功", data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        

        /// <summary>
        /// 获取电子围栏内所有摄像头信息
        /// </summary>
        public string GetElectricFenceCameraList(SafeworkcontrolEntity data)
        {
            string ComerId = string.Empty;
            try
            {
                //三维监控点位服务接口地址
                DataItemDetailBLL pdata = new DataItemDetailBLL();
                var ThreeDApi = pdata.GetItemValue("kbsThreeMonitoringPoints");
                space sp = new space();
                spacedata sds = new spacedata();
                List<spacegeo> geolist = new List<spacegeo>();
                List<spacepnt> pntlist = new List<spacepnt>();
                var comList = new KbscameramanageBLL().GetPageList("").Where(a => a.State == "在线" && a.CameraType == "枪机" && !a.MonitoringArea.IsNullOrWhiteSpace()).ToList();
                foreach (var com in comList)
                {//摄像头坐标点集
                    PositionsEntity alist = JsonConvert.DeserializeObject<PositionsEntity>(com.MonitoringArea);
                    List<double> dlist = new List<double>();
                    foreach (var ac in alist.positions)
                    {
                        dlist.Add(ac.x);
                        dlist.Add(ac.z);
                    }
                    //闭合回到原点
                    dlist.Add(alist.positions[0].x);
                    dlist.Add(alist.positions[0].z);
                    spacepnt pnt = new spacepnt()
                    {
                        id = com.CameraId,
                        x = 0,
                        y = 0,
                        coor = dlist
                    };
                    pntlist.Add(pnt);
                }
                spacegeo geo = new spacegeo();
                geo.id = data.ID;
                if (data.Areacode != null)
                {//电子围栏坐标点
                    PositionsEntity alist = JsonConvert.DeserializeObject<PositionsEntity>(data.Areacode);
                    List<double> dlist = new List<double>();
                    if (data.Areastate == 0)
                    {//矩形
                        geo.type = 1;
                        geo.distance = 0;
                        //第一个点
                        dlist.Add(alist.positions[0].x);
                        dlist.Add(alist.positions[0].z);
                        //第二个点
                        dlist.Add(alist.positions[1].x);
                        dlist.Add(alist.positions[0].z);
                        //第三个点
                        dlist.Add(alist.positions[1].x);
                        dlist.Add(alist.positions[1].z);
                        //第四个点
                        dlist.Add(alist.positions[0].x);
                        dlist.Add(alist.positions[1].z);
                        //回到原点
                        dlist.Add(alist.positions[0].x);
                        dlist.Add(alist.positions[0].z);
                        geo.coor = dlist;
                        geolist.Add(geo);
                    }
                    else if (data.Areastate == 1)
                    {//圆形
                        geo.type = 0;
                        dlist.Add(alist.positions[0].x);
                        dlist.Add(alist.positions[0].z);
                        geo.distance = Convert.ToInt32(data.Radius);
                        geo.coor = dlist;
                        geolist.Add(geo);
                    }
                    else if (data.Areastate == 2)
                    {//手绘多边形
                        foreach (var ac in alist.positions)
                        {
                            dlist.Add(ac.x);
                            dlist.Add(ac.z);
                        }
                        //闭合回到原点
                        dlist.Add(alist.positions[0].x);
                        dlist.Add(alist.positions[0].z);
                        geo.coor = dlist;
                        geo.type = 1;
                        geo.distance = 0;
                        geolist.Add(geo);
                    }
                }
                sds.geo = geolist;
                sds.pnt = pntlist;
                sp.type = "2";
                sp.data = sds;
                string rtn = HttpUtillibKbs.HttpThreeDPost(ThreeDApi, JsonConvert.SerializeObject(sp));
                List<RtnSpace> rss = JsonConvert.DeserializeObject<List<RtnSpace>>(rtn);
                foreach (var item in rss)
                {
                    if (string.IsNullOrEmpty(item.pntId)) continue;
                    ComerId += item.pntId + ',';
                }
                return ComerId;
            }
            catch (Exception er)
            {
                throw;
            }
        }

        /// <summary>
        /// 更新作业成员是否监管区域内状态
        /// </summary>
        /// <returns></returns>
        public object SaveSafeworkUserStateIifo([FromBody] JObject json)
        {
            try
            {
                string jsonstr = json.Value<string>("json");
                SafeWorkUserEntity ls = JsonConvert.DeserializeObject<SafeWorkUserEntity>(jsonstr);
                safeworkcontrolbll.SaveSafeworkUserStateIofo(ls.WorkId, ls.Userid, int.Parse(ls.State));
                return new { Code = 1, Count = 0, Info = "更新成功！" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }



        #endregion

    }
}
