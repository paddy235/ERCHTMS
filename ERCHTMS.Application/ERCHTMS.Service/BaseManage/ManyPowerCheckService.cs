using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using BSFramework.Util.Extension;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Linq;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.IService.AuthorizeManage;
using ERCHTMS.Service.AuthorizeManage;
using System;
using ERCHTMS.Code;
using System.Linq.Expressions;

namespace ERCHTMS.Service.BaseManage
{
    /// 描 述：用户管理
    /// </summary>
    public class ManyPowerCheckService : RepositoryFactory<ManyPowerCheckEntity>, IManyPowerCheckService
    {
        private UserService userservice = new UserService();
        private DepartmentService deptservice = new DepartmentService();
        #region 获取数据
        /// <summary>
        /// 根据机构编码和模块名称查询权限配置
        /// </summary>
        /// <param name="orgCode">机构编码</param>
        /// <param name="moduleName">模块名称</param>
        /// <returns></returns>
        public List<ManyPowerCheckEntity> GetList(string orgCode, string moduleName)
        {
            var list = new List<ManyPowerCheckEntity>();

            if (!string.IsNullOrWhiteSpace(orgCode) && !string.IsNullOrWhiteSpace(moduleName))
            {
                list = this.BaseRepository().IQueryable(x => x.CREATEUSERORGCODE == orgCode && x.MODULENAME == moduleName).OrderBy(x => x.AUTOID).OrderBy(x => x.CREATEDATE).ToList();
                //list = this.BaseRepository().IQueryable(x => x.CREATEUSERORGCODE == orgCode && x.MODULENAME == moduleName).OrderBy(x => x.AUTOID).OrderBy(x => x.SERIALNUM).ToList();
            }

            return list;
        }
        /// <summary>
        /// 根据机构编码和模块编码查询权限配置
        /// </summary>
        /// <param name="orgCode">机构编码</param>
        /// <param name="moduleNo">模块编码</param>
        /// <returns></returns>
        public List<ManyPowerCheckEntity> GetListByModuleNo(string orgCode, string moduleNo)
        {
            var list = new List<ManyPowerCheckEntity>();

            if (!string.IsNullOrWhiteSpace(orgCode) && !string.IsNullOrWhiteSpace(moduleNo))
            {
                list = this.BaseRepository().IQueryable(x => x.CREATEUSERORGCODE == orgCode && x.MODULENO == moduleNo).OrderBy(x => x.AUTOID).OrderBy(x => x.CREATEDATE).ToList();
                //list = this.BaseRepository().IQueryable(x => x.CREATEUSERORGCODE == orgCode && x.MODULENAME == moduleName).OrderBy(x => x.AUTOID).OrderBy(x => x.SERIALNUM).ToList();
            }

            return list;
        }
        public List<ManyPowerCheckEntity> GetListBySerialNum(string orgCode, string moduleName)
        {
            var list = new List<ManyPowerCheckEntity>();

            if (!string.IsNullOrWhiteSpace(orgCode) && !string.IsNullOrWhiteSpace(moduleName))
            {
                //list = this.BaseRepository().IQueryable(x => x.CREATEUSERORGCODE == orgCode && x.MODULENAME == moduleName).OrderBy(x => x.AUTOID).OrderBy(x => x.CREATEDATE).ToList();
                list = this.BaseRepository().IQueryable(x => x.CREATEUSERORGCODE == orgCode && x.MODULENAME == moduleName).OrderBy(x => x.AUTOID).OrderBy(x => x.SERIALNUM).ToList();
            }

            return list;
        }


        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="orgCode"></param>
        /// <param name="moduleName"></param>
        /// <param name="flowType"></param>
        /// <returns></returns>
        public List<ManyPowerCheckEntity> GetList(string orgCode, string moduleName,int serialnum) 
        {
            var list = new List<ManyPowerCheckEntity>();

            list = this.BaseRepository().IQueryable(x => x.CREATEUSERORGCODE == orgCode && x.MODULENAME == moduleName && x.SERIALNUM == serialnum).OrderBy(x => x.AUTOID).OrderBy(x => x.CREATEDATE).ToList();


            return list;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetManyPowerCheckEntityPage(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);

            return dt;
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ManyPowerCheckEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 获取通用的查询内容
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetGeneralQuery(string sql, DbParameter[] param)
        {
            var dt = this.BaseRepository().FindTable(sql, param);
            return dt;
        }

        /// <summary>
        /// 获取下一步审核流程节点
        /// </summary>
        /// <param name="currUser"></param>
        /// <param name="moduleName"></param>
        /// <param name="curFlowId"></param>
        /// <returns></returns>
        public ManyPowerCheckEntity CheckAuditForNext(Operator currUser, string moduleName, string curFlowId)
        {
            ManyPowerCheckEntity nextCheck = null;//下一步审核
            List<ManyPowerCheckEntity> powerList = GetListBySerialNum(currUser.OrganizeCode, moduleName).ToList();
            if (powerList.Count > 0)
            {
                //(当前流程节点不为控)寻找下一个流程节点
                if (!string.IsNullOrEmpty(curFlowId))
                {
                    int curIndex = powerList.FindIndex(p => p.ID == curFlowId); //当前的索引

                    int nextIndex = curIndex + 1; //下一个索引记录

                    if (nextIndex < powerList.Count())
                    {
                        nextCheck = powerList.ElementAt(nextIndex);
                    }
                    if (nextIndex + 1 < powerList.Count())
                    {
                        nextCheck.NextStepFlowEntity = powerList.ElementAt(nextIndex + 1);
                    }
                }
                else  //当前流程节点为空，取索引为0的对象 ,则记为初始登记提交流程
                {
                    nextCheck = powerList.ElementAt(0);  //取当前集合下的第一个节点
                    if (powerList.Count() > 1)
                    {
                        nextCheck.NextStepFlowEntity = powerList.ElementAt(1);
                    }
                }
                //退回则取默认为空。
                return nextCheck;
            }
            else
            {
                //审核配置返回空
                return nextCheck;
            }
        }

        /// <summary>
        /// 获取下一步审核流程节点
        /// </summary>
        /// <param name="currUser"></param>
        /// <param name="moduleName"></param>
        /// <param name="executedept">执行部门</param>
        /// <param name="createdept">创建部门</param>
        /// <param name="outengineerid">外包单位</param>
        /// <param name="businessid">业务id</param>
        /// <returns></returns>
        public ManyPowerCheckEntity CheckAuditForNext(Operator currUser, string moduleName,string executedept,string createdept,string outengineerid,string businessid)
        {
            try
            {
                ManyPowerCheckEntity nextCheck = null;//下一步审核
                List<ManyPowerCheckEntity> powerList = GetListBySerialNum(currUser.OrganizeCode, moduleName);

                if (powerList.Count > 0)
                {
                    //先查出执行部门编码
                    for (int i = 0; i < powerList.Count; i++)
                    {
                        if (powerList[i].ApplyType=="0")
                        {
                            if (powerList[i].CHECKDEPTCODE == "-3" || powerList[i].CHECKDEPTID == "-3")
                            {
                                switch (powerList[i].ChooseDeptRange) //判断部门范围
                                {
                                    case "0":
                                        powerList[i].CHECKDEPTID = createdept;
                                        break;
                                    case "1":
                                        var dept = deptservice.GetEntity(createdept);
                                        while (dept.Nature != "部门" && dept.Nature != "厂级")
                                        {
                                            dept = deptservice.GetEntity(dept.ParentId);
                                        }
                                        powerList[i].CHECKDEPTID = dept.DepartmentId;
                                        break;
                                    case "2":
                                        var dept1 = deptservice.GetEntity(createdept);
                                        while (dept1.Nature != "部门" && dept1.Nature != "厂级")
                                        {
                                            dept1 = deptservice.GetEntity(dept1.ParentId);
                                        }
                                        powerList[i].CHECKDEPTID = (dept1.DepartmentId + "," + createdept).Trim(',');
                                        break;
                                    default:
                                        powerList[i].CHECKDEPTID = createdept;
                                        break;
                                }
                            }
                        }
                    }
                    List<ManyPowerCheckEntity> checkPower = new List<ManyPowerCheckEntity>();

                    //登录人是否有审核权限--有审核权限直接审核通过
                    for (int i = 0; i < powerList.Count; i++)
                    {
                        if (powerList[i].ApplyType == "0")
                        {
                            if (powerList[i].CHECKDEPTID == currUser.DeptId)
                            {
                                var rolelist = currUser.RoleName.Split(',');
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
                        else if (powerList[i].ApplyType == "1")
                        {
                            var parameter = new List<DbParameter>();
                            //取脚本，获取账户的范围信息
                            if (powerList[i].ScriptCurcontent.Contains("@outengineerid"))
                            {
                                parameter.Add(DbParameters.CreateDbParameter("@outengineerid", !string.IsNullOrEmpty(outengineerid) ? outengineerid : ""));
                            }
                            //取脚本，获取账户的范围信息
                            if (powerList[i].ScriptCurcontent.Contains("@id"))
                            {
                                parameter.Add(DbParameters.CreateDbParameter("@id", !string.IsNullOrEmpty(businessid) ? businessid : ""));
                            }
                            DbParameter[] arrayparam = parameter.ToArray();
                            var userIds = DbFactory.Base().FindList<UserEntity>(powerList[i].ScriptCurcontent, arrayparam).Cast<UserEntity>().Aggregate("", (current, user) => current + (user.UserId + ",")).Trim(',');
                            if (userIds.Contains(currUser.UserId))
                            {
                                checkPower.Add(powerList[i]);
                                //break;
                            }
                        }
                        else if (powerList[i].ApplyType == "2")
                        {
                            if (powerList[i].CheckUserId.Contains(currUser.UserId))
                            {
                                checkPower.Add(powerList[i]);
                                //break;
                            }
                            
                        }
                    }

                    while (nextCheck == null || (string.IsNullOrEmpty(nextCheck.CHECKDEPTID) && nextCheck.ApplyType == "0"))
                    {
                        if (checkPower.Count > 0)
                        {
                            ManyPowerCheckEntity check = checkPower.Last();//当前

                            for (int i = 0; i < powerList.Count; i++)
                            {
                                if (check.ID == powerList[i].ID)
                                {
                                    if ((i + 1) >= powerList.Count)
                                    {
                                        return null;
                                    }
                                    else
                                    {
                                        nextCheck = powerList[i + 1];
                                    }
                                }
                            }
                        }
                        else
                        {
                            nextCheck = powerList.First();
                        }
                        checkPower.Add(nextCheck);
                    }

                }
                else
                {
                    nextCheck = null;
                }

                if (nextCheck != null)
                {
                    //当前审核序号下的对应集合
                    var serialList = powerList.Where(p => p.SERIALNUM == nextCheck.SERIALNUM);
                    //集合记录大于1，则表示存在并行审核（审查）的情况
                    if (serialList.Count() > 1)
                    {
                        ManyPowerCheckEntity slastEntity = new ManyPowerCheckEntity();
                        slastEntity = serialList.LastOrDefault();
                        nextCheck = slastEntity;
                    }
                }
                return nextCheck;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取审核人账号
        /// </summary>
        /// <param name="currflowid">当前流程节点</param>
        /// <param name="businessid">业务逻辑节点ID</param>
        /// <param name="NextStepApproveUserAccount">指定下一步审核人账号</param>
        /// <param name="specialtytype">专业类别</param>
        /// <param name="executedept">执行部门</param>
        /// <param name="outsourcingdept">外包单位</param>
        /// <param name="createdept">创建部门</param>
        /// <param name="professionaldept">专业部门</param>
        /// <param name="ondutydept">值班部门</param>
        /// <param name="supervisordept">监理单位</param>
        /// <param name="outengineerid">外包工程id</param>
        /// <returns></returns>
        public string GetApproveUserAccount(string currflowid, string businessid, string NextStepApproveUserAccount = "", string specialtytype = "", string executedept = "", string outsourcingdept = "", string createdept = "", string professionaldept = "", string ondutydept = "",string supervisordept="",string outengineerid="")
        {
            try
            {
                string useraccount = string.Empty;
                Operator currUser = OperatorProvider.Provider.Current();
                ManyPowerCheckEntity currFlow = GetEntity(currflowid);
                if (currFlow != null)
                {
                    IList<ManyPowerCheckEntity> FlowList = GetListBySerialNum(currUser.OrganizeCode, currFlow.MODULENAME).ToList().Where(t => t.SERIALNUM == currFlow.SERIALNUM).ToList();
                    //给对应特殊部门赋值
                    foreach (var item in FlowList)
                    {
                        switch (item.CHECKDEPTID)
                        {
                            case "-1":
                                switch (item.ChooseDeptRange) //判断部门范围
                                {
                                    case "0":
                                        item.CHECKDEPTID = executedept;
                                        break;
                                    case "1":
                                        var dept = deptservice.GetEntity(executedept);
                                        while (dept.Nature != "部门")
                                        {
                                            dept = deptservice.GetEntity(dept.ParentId);
                                        }
                                        item.CHECKDEPTID = dept.DepartmentId;
                                        break;
                                    case "2":
                                        var dept1 = deptservice.GetEntity(executedept);
                                        while (dept1.Nature != "部门")
                                        {
                                            dept1 = deptservice.GetEntity(dept1.ParentId);
                                        }
                                        item.CHECKDEPTID = (dept1.DepartmentId + "," + executedept).Trim(',');
                                        break;
                                    default:
                                        item.CHECKDEPTID = executedept;
                                        break;
                                }
                                break;
                            case "-2":
                                item.CHECKDEPTID = outsourcingdept;
                                break;
                            case "-3":
                                switch (item.ChooseDeptRange) //判断部门范围
                                {
                                    case "0":
                                        item.CHECKDEPTID = createdept;
                                        break;
                                    case "1":
                                        var dept = deptservice.GetEntity(createdept);
                                        while (dept.Nature != "部门" && dept.Nature != "厂级")
                                        {
                                            dept = deptservice.GetEntity(dept.ParentId);
                                        }
                                        item.CHECKDEPTID = dept.DepartmentId;
                                        break;
                                    case "2":
                                        var dept1 = deptservice.GetEntity(createdept);
                                        while (dept1.Nature != "部门" && dept1.Nature != "厂级")
                                        {
                                            dept1 = deptservice.GetEntity(dept1.ParentId);
                                        }
                                        item.CHECKDEPTID = (dept1.DepartmentId + "," + createdept).Trim(',');
                                        break;
                                    default:
                                        item.CHECKDEPTID = executedept;
                                        break;
                                }
                                break;
                            case "-4":
                                item.CHECKDEPTID = professionaldept;
                                break;
                            case "-5":
                                item.CHECKDEPTID = ondutydept;
                                break;
                            case "-6":
                                item.CHECKDEPTID = supervisordept;
                                break;
                            default:
                                break;
                        }
                    }

                    //查询审核账号
                    foreach (var item in FlowList)
                    {
                        switch (item.ApplyType)
                        {
                            case "0":
                                string type = item.REMARK != "1" ? "0" : "1";
                                string useraccountandnames = userservice.GetUserAccount(item.CHECKDEPTID, item.CHECKROLENAME, type, specialtytype);
                                useraccount += string.IsNullOrEmpty(useraccountandnames) ? "" : useraccountandnames.Split('|')[1] + ",";
                                break;
                            case "1":
                                string curstartsql = item.ScriptCurcontent;
                                #region 脚本获取起始对象条件
                                if (!string.IsNullOrEmpty(curstartsql))
                                {
                                    var parameter = new List<DbParameter>();
                                    //取脚本，获取账户的范围信息
                                    if (curstartsql.Contains("@id"))
                                    {
                                        parameter.Add(DbParameters.CreateDbParameter("@id", !string.IsNullOrEmpty(businessid) ? businessid : ""));
                                    }
                                    if (curstartsql.Contains("@ID"))
                                    {
                                        parameter.Add(DbParameters.CreateDbParameter("@ID", !string.IsNullOrEmpty(businessid) ? businessid : ""));
                                    }
                                    //取脚本，获取账户的范围信息
                                    if (curstartsql.Contains("@outengineerid"))
                                    {
                                        parameter.Add(DbParameters.CreateDbParameter("@outengineerid", !string.IsNullOrEmpty(outengineerid) ? outengineerid : ""));
                                    }
                                    DbParameter[] arrayparam = parameter.ToArray();
                                    DataTable joinDt = GetGeneralQuery(curstartsql, arrayparam);
                                    //起始用户账户匹配
                                    if (joinDt.Rows.Count > 0)
                                    {
                                        foreach (DataRow jrow in joinDt.Rows)
                                        {
                                            string rowaccount = jrow["account"].ToString();
                                            if (!string.IsNullOrEmpty(rowaccount))
                                            {
                                                useraccount += rowaccount + ",";
                                            }
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "2":
                                useraccount += string.IsNullOrEmpty(item.CheckUserAccount) ? "" : item.CheckUserAccount + ",";
                                break;
                            case "3":
                                useraccount += string.IsNullOrEmpty(NextStepApproveUserAccount) ? "" : NextStepApproveUserAccount + ",";
                                break;
                            default:
                                break;
                        }
                    }
                }
                return useraccount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        /// <summary>
        /// 获取审核人账号
        /// </summary>
        /// <param name="currflowid">当前流程节点</param>
        /// <param name="businessid">业务逻辑节点ID</param>
        /// <param name="NextStepApproveUserAccount">指定下一步审核人账号</param>
        /// <param name="specialtytype">专业类别</param>
        /// <param name="executedept">执行部门</param>
        /// <param name="outsourcingdept">外包单位</param>
        /// <param name="createdept">创建部门</param>
        /// <param name="professionaldept">专业部门</param>
        /// <param name="ondutydept">值班部门</param>
        /// /// <param name="supervisordept">监理单位</param>
        /// <param name="outengineerid">工程ID</param>
        /// <returns></returns>
        public string GetApproveUserId(string currflowid, string businessid, string NextStepApproveUserAccount = "", string specialtytype = "", string executedept = "", string outsourcingdept = "", string createdept = "", string professionaldept = "", string ondutydept = "",string supervisordept="",string outengineerid="")
        {
            try
            {
                string userid = string.Empty;
                Operator currUser = OperatorProvider.Provider.Current();
                ManyPowerCheckEntity currFlow = GetEntity(currflowid);
                if (currFlow != null)
                {
                    IList<ManyPowerCheckEntity> FlowList = GetListBySerialNum(currUser.OrganizeCode, currFlow.MODULENAME).ToList().Where(t => t.SERIALNUM == currFlow.SERIALNUM).ToList();
                    //给对应特殊部门赋值
                    foreach (var item in FlowList)
                    {
                        switch (item.CHECKDEPTID)
                        {
                            case "-1":
                                switch (item.ChooseDeptRange) //判断部门范围
                                {
                                    case "0": 
                                        item.CHECKDEPTID = executedept;
                                        break;
                                    case "1":
                                        var dept = deptservice.GetEntity(executedept);
                                        while (dept.Nature != "部门")
                                        {
                                            dept = deptservice.GetEntity(dept.ParentId);
                                        }
                                        item.CHECKDEPTID = dept.DepartmentId;
                                        break;
                                    case "2":
                                        var dept1 = deptservice.GetEntity(executedept);
                                        while (dept1.Nature != "部门")
                                        {
                                            dept1 = deptservice.GetEntity(dept1.ParentId);
                                        }
                                        item.CHECKDEPTID = (dept1.DepartmentId + "," + executedept).Trim(',');
                                        break;
                                    default:
                                        item.CHECKDEPTID = executedept;
                                        break;
                                }
                                break;
                            case "-2":
                                item.CHECKDEPTID = outsourcingdept;
                                break;
                            case "-3":
                                switch (item.ChooseDeptRange) //判断部门范围
                                {
                                    case "0":
                                        item.CHECKDEPTID = createdept;
                                        break;
                                    case "1":
                                        var dept = deptservice.GetEntity(createdept);
                                        while (dept.Nature != "部门" && dept.Nature!="厂级")
                                        {
                                            dept = deptservice.GetEntity(dept.ParentId);
                                        }
                                        item.CHECKDEPTID = dept.DepartmentId;
                                        break;
                                    case "2":
                                        var dept1 = deptservice.GetEntity(createdept);
                                        while (dept1.Nature != "部门" && dept1.Nature!="厂级")
                                        {
                                            dept1 = deptservice.GetEntity(dept1.ParentId);
                                        }
                                        item.CHECKDEPTID = (dept1.DepartmentId + "," + createdept).Trim(',');
                                        break;
                                    default:
                                        item.CHECKDEPTID = executedept;
                                        break;
                                }
                                break;
                            case "-4":
                                item.CHECKDEPTID = professionaldept;
                                break;
                            case "-5":
                                item.CHECKDEPTID = ondutydept;
                                break;
                            case "-6":
                                item.CHECKDEPTID = supervisordept;
                                break;
                            default:
                                break;
                        }
                    }

                    //查询审核账号
                    foreach (var item in FlowList)
                    {
                        switch (item.ApplyType)
                        {
                            case "0":
                                string type = item.REMARK != "1" ? "0" : "1";
                                specialtytype = item.ChooseMajor == "1" ? item.SpecialtyType : specialtytype;
                                string users = userservice.GetUserAccount(item.CHECKDEPTID, item.CHECKROLENAME, type, specialtytype);
                                userid += string.IsNullOrEmpty(users) ? "" : users.Split('|')[2] + ",";
                                break;
                            case "1":
                                string curstartsql = item.ScriptCurcontent;
                                #region 脚本获取起始对象条件
                                if (!string.IsNullOrEmpty(curstartsql))
                                {
                                    var parameter = new List<DbParameter>();
                                    //取脚本，获取账户的范围信息
                                    if (curstartsql.Contains("@outengineerid"))
                                    {
                                        parameter.Add(DbParameters.CreateDbParameter("@outengineerid", !string.IsNullOrEmpty(outengineerid) ? outengineerid : ""));
                                    }
                                    if (curstartsql.Contains("@id"))
                                    {
                                        parameter.Add(DbParameters.CreateDbParameter("@id", !string.IsNullOrEmpty(businessid) ? businessid : ""));
                                    }
                                    if (curstartsql.Contains("@ID"))
                                    {
                                        parameter.Add(DbParameters.CreateDbParameter("@ID", !string.IsNullOrEmpty(businessid) ? businessid : ""));
                                    }
                                    if (curstartsql.Contains("@engineerletdeptid"))
                                    {
                                        parameter.Add(DbParameters.CreateDbParameter("@engineerletdeptid", !string.IsNullOrEmpty(executedept) ? executedept : ""));
                                    }
                                    DbParameter[] arrayparam = parameter.ToArray();
                                    DataTable joinDt = GetGeneralQuery(curstartsql, arrayparam);
                                    //起始用户账户匹配
                                    if (joinDt.Rows.Count > 0)
                                    {
                                        foreach (DataRow jrow in joinDt.Rows)
                                        {
                                            string rowuserid = jrow["userid"].ToString();
                                            if (!string.IsNullOrEmpty(rowuserid))
                                            {
                                                userid += rowuserid + ",";
                                            }
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "2":
                                userid += string.IsNullOrEmpty(item.CheckUserId) ? "" : item.CheckUserId + ",";
                                break;
                            case "3":
                                userid += string.IsNullOrEmpty(NextStepApproveUserAccount) ? "" : NextStepApproveUserAccount + ",";
                                break;
                            default:
                                break;
                        }
                    }
                }
                return userid;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, ManyPowerCheckEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }
        #endregion
    }
}
