using BSFramework.Data;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.IService.HiddenTroubleManage;
using ERCHTMS.IService.OutsourcingProject;
using ERCHTMS.IService.SystemManage;
using ERCHTMS.Service.BaseManage;
using ERCHTMS.Service.OutsourcingProject;
using ERCHTMS.Service.SystemManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Service.HiddenTroubleManage
{
    /// <summary>
    /// 流程配置控制基础服务
    /// </summary>
    public class WfControlServices : WfControlIServices
    {
        private WfSettingIService wfsettingiservice = new WfSettingService();
        private WfConditionIService wfconditioniservice = new WfConditionService();
        private WfConditionAddtionService wfconditionaddtionservice = new WfConditionAddtionService();
        private WfInstanceIService wfinstanceiservice = new WfInstanceService();
        private IUserInfoService iuserservice = new UserInfoService();
        private IDepartmentService idepartmentservice = new DepartmentService();
        private HTWorkFlowIService htworkflowiservice = new HTWorkFlowService();
        private OutsouringengineerIService outsouringengineeriservice = new OutsouringengineerService();
        private IDataItemDetailService idataItemDetailService = new DataItemDetailService();

        #region 获取流程控制对象
        /// <summary>
        /// 获取流程控制对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public WfControlResult GetWfControlObject(WfControlObj obj)
        {
            //返回的结果对象
            WfControlResult result = new WfControlResult();
            WfInstanceEntity entity = new WfInstanceEntity();
            Operator curUser = new Operator();
            UserInfoEntity spUser = new UserInfoEntity(); 
            string curOrganizeId = string.Empty;
            string curRoleName = string.Empty;
            string deptType = string.Empty; //部门类型

            string curSettingId = string.Empty; //当前的流程配置项

            #region 用户相关信息提取
            if (null != obj.user)
            {
                curUser = obj.user; //当前用户对象
                curRoleName = curUser.RoleName; //当前用户角色
                curOrganizeId = curUser.OrganizeId; //机构id
                if (curRoleName.Contains("承包商"))
                {
                    deptType = "承包商";
                }
                if (curRoleName.Contains("班组级"))
                {
                    deptType = "班组";
                }
                if (curRoleName.Contains("专业级"))
                {
                    deptType = "专业";
                }
                if (curRoleName.Contains("部门级"))
                {
                    deptType = "部门";
                }
                if (curRoleName.Contains("公司级"))
                {
                    deptType = "厂级";
                }
                if (curRoleName.Contains("省级"))
                {
                    deptType = "省级";
                    curOrganizeId = obj.organizeid; //当前选中的电厂id
                }
                obj.depttype = deptType;
            }
            #endregion

            #region 用户相关信息提取
            if (null != obj.spuser)
            {
                spUser = obj.spuser; //当前用户对象
                curRoleName = spUser.RoleName; //当前用户角色
                curOrganizeId = spUser.OrganizeId; //机构id
                if (curRoleName.Contains("承包商"))
                {
                    deptType = "承包商";
                }
                if (curRoleName.Contains("班组级"))
                {
                    deptType = "班组";
                }
                if (curRoleName.Contains("专业级"))
                {
                    deptType = "专业";
                }
                if (curRoleName.Contains("部门级"))
                {
                    deptType = "部门";
                }
                if (curRoleName.Contains("公司级"))
                {
                    deptType = "厂级";
                }
                if (curRoleName.Contains("省级"))
                {
                    deptType = "省级";
                    curOrganizeId = obj.organizeid; //当前选中的电厂id
                }
                if (curRoleName.Contains("集团"))
                {
                    deptType = "集团";
                    curOrganizeId = obj.organizeid; //当前选中的电厂id
                }
                obj.depttype = deptType;
            }
            #endregion

            //获取流程实例对象  机构id  级别(一般隐患  or  重大隐患)  流程标记(隐患排查，后期扩展违章等其他工作流)
            try
            {
                string instanceOrgId = !string.IsNullOrEmpty(obj.organizeid) ? obj.organizeid : curOrganizeId;
                #region 获取流程实例对象
                var list = wfinstanceiservice.GetListByArgs(instanceOrgId, obj.rankname, obj.mark);
                if (list.Count() > 0)
                {
                    //获取启用的流程
                    var tempList = list.Where(p => p.ISENABLE == "是").ToList();  //获取流程配置实例
                    if (tempList.Count() > 0)
                    {
                        entity = tempList.FirstOrDefault();
                        obj.instanceid = entity.ID;
                    }
                    else
                    {
                        entity = null;
                        result.code = WfCode.NoEnable; //未启用流程配置实例 
                    }
                }
                else
                {
                    entity = null;
                    result.code = WfCode.NoInstance; //未配置流程实例
                }
                #endregion
            }
            catch (Exception ex)
            {
                result.code = WfCode.InstanceError; //获取流程实例出错
                return result;
            }

            //流程配置实例
            if (null != entity)
            {
                //是否弃用流程实例
                #region 是否弃用流程实例
                if (entity.ISENABLE == "是")
                {
                    try
                    {
                        //起始流程对象分析
                        DataTable dt = wfsettingiservice.GetWfSettingForInstance(obj, "", ""); //获取起始流程下的流程配置及条件内容

                        //获取当前符合条件的流程流转配置项
                        var tempResult = GetCurSettingObj(obj, dt); //过滤所有流程配置项及条件项

                        if (tempResult.code == WfCode.Sucess)
                        {
                            curSettingId = tempResult.settingid;
                        }
                        else
                        {
                            return tempResult;
                        }
                    }
                    catch (Exception ex)
                    {
                        result.code = WfCode.InstanceError; //获取流程实例出错
                        return result;
                    }


                    try
                    {
                        #region 获取最终符合条件的配置项
                        if (!string.IsNullOrEmpty(curSettingId))
                        {
                            obj.cursettingid = curSettingId; //当前配置流程id

                            var settingmodel = wfsettingiservice.GetEntity(curSettingId);
                            if (null != settingmodel)
                            {
                                result.isend = settingmodel.ISENDPOINT == "是"; //是否结束流程
                            }
                            //仅仅验证当前是否具有此流程的操作权限(如隐患登记阶段，评估是否具有上报功能)
                            if (obj.isvaliauth)
                            {
                                result.ishave = true; //存在对应的配置项，则表示当前用户存在对应的功能操作权限
                            }
                            else //流程推送取目标流程对象
                            {
                                result.isspecialchange = true;
                                result.ischangestatus = settingmodel.ISUPDATEFLOW == "是"; //是否更改流程状态
                                //结束流程不用定义
                                #region 结束流程不用定义，进行中则取目标对象
                                if (result.isend)
                                {
                                    result.code = WfCode.Sucess;
                                    if (null != curUser && !string.IsNullOrEmpty(curUser.UserId))
                                    {
                                        result.actionperson = curUser.Account;
                                        result.deptname = curUser.DeptName;
                                        result.deptcode = curUser.DeptCode;
                                        result.deptid = curUser.DeptId;
                                        result.username = curUser.UserName;
                                    }
                                    if (null != spUser && !string.IsNullOrEmpty(spUser.UserId))
                                    {
                                        result.actionperson = spUser.Account;
                                        result.deptname = spUser.DeptName;
                                        result.deptcode = spUser.DeptCode;
                                        result.deptid = spUser.DepartmentId;
                                        result.username = spUser.RealName;
                                    }
                                    //是否更改流程状态
                                    if (settingmodel.ISUPDATEFLOW == "是")
                                    {
                                        result.wfflag = settingmodel.WFFLAG; //配置标记
                                    }
                                }
                                else //获取目标流程
                                {
                                    result = GetTargetFlowObj(obj, result);
                                }
                                //通用型，基于流程配置化管理
                                if (result.ischangestatus)
                                {
                                    //特殊处理，是否需要改变流程状态
                                    result.ischangestatus = result.isspecialchange;
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            result.code = WfCode.NoSetting;  //未配置流程项目
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        result.code = WfCode.TargetError; //获取流程实例出错
                        return result;
                    }
                }
                #endregion
                else
                {
                    result.code = WfCode.NoEnable; //未启用流程配置实例
                }
            }
            return result;
        }
        #endregion

        #region 根据起始流程对象获取流程配置项
        /// <summary>
        /// 获取配置的起始对象
        /// </summary>
        /// <param name="obj">请求对象</param>
        /// <param name="dt">配置项内容</param>
        /// <returns></returns>
        public WfControlResult GetCurSettingObj(WfControlObj obj, DataTable dt)
        {

            WfControlResult result = new WfControlResult();

            List<string> handleSettingIds = new List<string>();

            try
            {
                string curSettingId = string.Empty; //当前待处理的配置项

                string curRoleName = string.Empty; //当前用户角色
                if (null != obj.user)
                {
                    curRoleName = obj.user.RoleName;
                }
                if (null != obj.spuser)
                {
                    curRoleName = obj.spuser.RoleName;
                }

                //存取流程配置项，用于过滤当前的对应配置项目.
                List<string> ids = new List<string>();  //满足的配置项

                List<string> noids = new List<string>(); //不满足的配置项

                #region 遍历获取符合条件的配置项目
                foreach (DataRow row in dt.Rows)
                {

                    string settingid = row["id"].ToString(); //流程导向配置id 
               
                    //条件id
                    string conditionid = row["conditionid"].ToString();
                    string isexcutecursql = row["isexcutecursql"].ToString(); //是否执行起始参与者脚本
                    string isexcutesql  = row["isexcutesql"].ToString(); //目标对象
                    string settingtype = row["settingtype"].ToString();  //提交形式
                    string isexecsql = row["isexecsql"].ToString(); //流程配置条件里是否执行脚本
                    //当不存在条件时，去配置项查找当前起始参与者脚本，进一步匹配，如果不存在，则视为无任何流程配置满足当前人的任意提交请求
                    if (isexcutecursql == "是")
                    {
                        string curstartsql = row["scriptcurcontent"].ToString();
                        try
                        {
                            #region 脚本获取起始对象条件
                            if (!string.IsNullOrEmpty(curstartsql))
                            {
                                List<IndexOfModel> mlist = new List<IndexOfModel>();
                                mlist = SubstringCountList(curstartsql, "@id", mlist);
                                mlist = SubstringCountList(curstartsql, "@argument1", mlist);
                                mlist = SubstringCountList(curstartsql, "@argument2", mlist);
                                mlist = SubstringCountList(curstartsql, "@argument3", mlist);
                                mlist = SubstringCountList(curstartsql, "@argument4", mlist);
                                mlist = SubstringCountList(curstartsql, "@argument5", mlist);
                                mlist = SubstringCountList(curstartsql, "@argument6", mlist);
                                mlist = mlist.OrderBy(p => p.indexValue).ToList();
                                var parameter = new List<DbParameter>();
                                for (int i = 0; i < mlist.Count(); i++)
                                {
                                    //取脚本，获取账户的范围信息
                                    if (mlist[i].indexName == "@id")
                                    {
                                        parameter.Add(DbParameters.CreateDbParameter("@id", !string.IsNullOrEmpty(obj.businessid) ? obj.businessid : ""));
                                    }
                                    //自定义参数
                                    else if (mlist[i].indexName == "@argument1")
                                    {
                                        parameter.Add(DbParameters.CreateDbParameter("@argument1", !string.IsNullOrEmpty(obj.argument1) ? obj.argument1 : ""));
                                    }
                                    //自定义参数
                                    else if (mlist[i].indexName == "@argument2")
                                    {
                                        parameter.Add(DbParameters.CreateDbParameter("@argument2", !string.IsNullOrEmpty(obj.argument2) ? obj.argument2 : ""));
                                    }
                                    //自定义参数
                                    else if (mlist[i].indexName == "@argument3")
                                    {
                                        parameter.Add(DbParameters.CreateDbParameter("@argument3", !string.IsNullOrEmpty(obj.argument3) ? obj.argument3 : ""));
                                    }
                                    //自定义参数
                                    else if (mlist[i].indexName == "@argument4")
                                    {
                                        parameter.Add(DbParameters.CreateDbParameter("@argument4", !string.IsNullOrEmpty(obj.argument4) ? obj.argument4 : ""));
                                    }
                                    //自定义参数
                                    else if (mlist[i].indexName == "@argument5")
                                    {
                                        parameter.Add(DbParameters.CreateDbParameter("@argument5", !string.IsNullOrEmpty(obj.argument5) ? obj.argument5 : ""));
                                    }
                                    //自定义参数
                                    else if (mlist[i].indexName == "@argument6")
                                    {
                                        parameter.Add(DbParameters.CreateDbParameter("@argument6", !string.IsNullOrEmpty(obj.argument6) ? obj.argument6 : ""));
                                    }
                                }
                                DbParameter[] arrayparam = parameter.ToArray();
                                DataTable joinDt = wfsettingiservice.GetGeneralQuery(curstartsql, arrayparam);
                                //起始用户账户匹配
                                if (joinDt.Rows.Count > 0)
                                {
                                    string joinaccount = string.Empty;
                                    foreach (DataRow jrow in joinDt.Rows)
                                    {
                                        string rowaccount = jrow["account"].ToString();
                                        if (!string.IsNullOrEmpty(rowaccount))
                                        {
                                            joinaccount += rowaccount + ",";
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(joinaccount))
                                    {
                                        joinaccount = "," + joinaccount;

                                        string sourceAccount = string.Empty;

                                        if (null != obj.user)
                                        {
                                            sourceAccount = "," + obj.user.Account + ",";
                                        }
                                        if (null != obj.spuser)
                                        {
                                            sourceAccount = "," + obj.spuser.Account + ",";
                                        }

                                        //单个匹配账户
                                        if (joinaccount.Contains(sourceAccount))
                                        {
                                            ids.Add(settingid);
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            result.code = WfCode.StartSqlError; //起始流程脚本对象出错
                            return result;
                        }
                    }
                    else
                    {
                        if (isexecsql == "是" && settingtype == "起始流程")
                        {
                            string conditionsql = row["sqlcontent"].ToString();
                            try
                            {
                                #region 脚本获取起始对象条件
                                if (!string.IsNullOrEmpty(conditionsql))
                                {
                                    List<IndexOfModel> mlist = new List<IndexOfModel>();
                                    mlist = SubstringCountList(conditionsql, "@id", mlist);
                                    mlist = SubstringCountList(conditionsql, "@argument1", mlist);
                                    mlist = SubstringCountList(conditionsql, "@argument2", mlist);
                                    mlist = SubstringCountList(conditionsql, "@argument3", mlist);
                                    mlist = SubstringCountList(conditionsql, "@argument4", mlist);
                                    mlist = SubstringCountList(conditionsql, "@argument5", mlist);
                                    mlist = SubstringCountList(conditionsql, "@argument6", mlist);
                                    mlist = mlist.OrderBy(p => p.indexValue).ToList();
                                    var parameter = new List<DbParameter>();
                                    for (int i = 0; i < mlist.Count(); i++) 
                                    {
                                        //取脚本，获取账户的范围信息
                                        if (mlist[i].indexName == "@id")
                                        {
                                            parameter.Add(DbParameters.CreateDbParameter("@id", !string.IsNullOrEmpty(obj.businessid) ? obj.businessid : ""));
                                        }
                                        //自定义参数
                                        else if (mlist[i].indexName == "@argument1")
                                        {
                                            parameter.Add(DbParameters.CreateDbParameter("@argument1", !string.IsNullOrEmpty(obj.argument1) ? obj.argument1 : ""));
                                        }
                                        //自定义参数
                                        else if (mlist[i].indexName == "@argument2")
                                        {
                                            parameter.Add(DbParameters.CreateDbParameter("@argument2", !string.IsNullOrEmpty(obj.argument2) ? obj.argument2 : ""));
                                        }
                                        //自定义参数
                                        else if (mlist[i].indexName == "@argument3")
                                        {
                                            parameter.Add(DbParameters.CreateDbParameter("@argument3", !string.IsNullOrEmpty(obj.argument3) ? obj.argument3 : ""));
                                        }
                                        //自定义参数
                                        else if (mlist[i].indexName == "@argument4")
                                        {
                                            parameter.Add(DbParameters.CreateDbParameter("@argument4", !string.IsNullOrEmpty(obj.argument4) ? obj.argument4 : ""));
                                        }
                                        //自定义参数
                                        else if (mlist[i].indexName == "@argument5")
                                        {
                                            parameter.Add(DbParameters.CreateDbParameter("@argument5", !string.IsNullOrEmpty(obj.argument5) ? obj.argument5 : ""));
                                        }
                                        //自定义参数
                                        else if (mlist[i].indexName == "@argument6")
                                        {
                                            parameter.Add(DbParameters.CreateDbParameter("@argument6", !string.IsNullOrEmpty(obj.argument6) ? obj.argument6 : ""));
                                        }
                                    }
                                    DbParameter[] arrayparam = parameter.ToArray();
                                    DataTable joinDt = wfsettingiservice.GetGeneralQuery(conditionsql, arrayparam);
                                    //起始用户账户匹配
                                    if (joinDt.Rows.Count > 0)
                                    {
                                        string joinaccount = string.Empty;
                                        foreach (DataRow jrow in joinDt.Rows)
                                        {
                                            string rowaccount = jrow["account"].ToString();
                                            if (!string.IsNullOrEmpty(rowaccount))
                                            {
                                                joinaccount += rowaccount + ",";
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(joinaccount))
                                        {
                                            joinaccount = "," + joinaccount;
                                            string sourceAccount = string.Empty;

                                            if (null != obj.user)
                                            {
                                                sourceAccount = "," + obj.user.Account + ",";
                                            }
                                            if (null != obj.spuser)
                                            {
                                                sourceAccount = "," + obj.spuser.Account + ",";
                                            }
                                            //单个匹配账户
                                            if (joinaccount.Contains(sourceAccount))
                                            {
                                                ids.Add(settingid);
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                result.code = WfCode.StartSqlError; //起始流程脚本对象出错
                                return result;
                            }
                        }
                        else
                        {
                            #region 判定当前配置是否存在条件内容
                            try
                            {
                                //判定当前配置是否存在条件内容(起始流程部分，暂时未提供指定部门的情况)
                                if (!string.IsNullOrEmpty(conditionid) && !string.IsNullOrEmpty(settingtype) && settingtype == "起始流程")
                                {
                                    //是否包含当前角色
                                    string rolerule = row["rolerule"].ToString();
                                    string sdepttype = row["depttype"].ToString(); //部门性质
                                    string sroletype = row["roletype"].ToString();  //角色性质
                                    string choosetype = row["choosetype"].ToString(); //针对当前人员选择的部门
                                    string wfflag = string.Empty;
                                    //如果是确定部门选择形式为非其他部门，则依据部门形式来选择
                                    if (!string.IsNullOrEmpty(choosetype) && choosetype != "其他")
                                    {
                                        //如果已经处理过的对象
                                        if (handleSettingIds.Contains(settingid))
                                        {
                                            continue;
                                        }
                                        var startDt = wfsettingiservice.GetWfSettingForInstance(obj, "起始流程", settingid);
                                        if (!handleSettingIds.Contains(settingid))
                                        {
                                            handleSettingIds.Add(settingid);
                                            List<AccountQueryObj> aqlist = HandleFlowCondition(obj, startDt, result, out wfflag).ToList();

                                            string sourceAccount = string.Empty;

                                            if (null != obj.user)
                                            {
                                                sourceAccount = obj.user.Account;
                                            }
                                            if (null != obj.spuser)
                                            {
                                                sourceAccount = obj.spuser.Account;
                                            }
                                            aqlist = aqlist.Where(p => p.account == sourceAccount).ToList();
                                            if (aqlist.Count() > 0)
                                            {
                                                ids.Add(settingid);
                                            }
                                            else
                                            {
                                                noids.Add(settingid);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // 满足 部门属性  角色属性  获取相应的配置项
                                        #region 满足 部门属性  角色属性  获取相应的配置项
                                        if (!string.IsNullOrEmpty(sdepttype) && !string.IsNullOrEmpty(sroletype))
                                        {
                                            #region  其他项，则排除
                                            if (sdepttype == "其他")
                                            {
                                                if (curRoleName.Contains(sroletype) && rolerule == "包含")
                                                {
                                                    if (!ids.Contains(settingid))
                                                    {
                                                        ids.Add(settingid);
                                                    }
                                                }
                                                else if (!curRoleName.Contains(sroletype) && rolerule == "不包含")
                                                {
                                                    if (!ids.Contains(settingid))
                                                    {
                                                        ids.Add(settingid);
                                                    }
                                                }
                                                else //不符合条件
                                                {
                                                    if (!noids.Contains(settingid))
                                                    {
                                                        noids.Add(settingid); //不符合要求的配置项
                                                    }
                                                }
                                            }
                                            #endregion
                                            #region 符合条件
                                            else
                                            {
                                                //符合条件 
                                                if (sdepttype == obj.depttype && curRoleName.Contains(sroletype) && rolerule == "包含")
                                                {
                                                    if (!ids.Contains(settingid))
                                                    {
                                                        ids.Add(settingid);
                                                    }
                                                }
                                                //符合条件
                                                else if (sdepttype == obj.depttype && !curRoleName.Contains(sroletype) && rolerule == "不包含")
                                                {
                                                    if (!ids.Contains(settingid))
                                                    {
                                                        ids.Add(settingid);
                                                    }
                                                }
                                                else //不符合条件
                                                {
                                                    if (!noids.Contains(settingid))
                                                    {
                                                        noids.Add(settingid); //不符合要求的配置项
                                                    }
                                                }
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                        #endregion
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                result.code = WfCode.StartSettingError; //起始流程条件配置出错
                                return result;
                            }
                            #endregion
                        }
                    }
                    
                }
                #endregion

                #region 归纳，计算得出最终的符合条件的流程配置项目
                //归纳，计算得出最终的符合条件的流程配置项目
                if (ids.Count() > 0)
                {
                    if (noids.Count() > 0)
                    {
                        foreach (string noid in noids)
                        {
                            //如果不符合条件的配置存在于符合配置项当中，则从符合配置中移除
                            if (ids.Contains(noid))
                            {
                                ids.Remove(noid);
                            }
                        }
                    }
                }

                //符合条件的配置项为1时
                if (ids.Count() > 0)
                {
                    if (ids.Count() > 1)
                    {
                        //排序取最大值的对象
                        var tempsetting = wfsettingiservice.GetList(obj.instanceid).Where(p => ids.Contains(p.ID)).OrderByDescending(p => p.STARTLEVEL).FirstOrDefault();
                        if (null != tempsetting)
                        {
                            curSettingId = tempsetting.ID;
                        }
                    }
                    else
                    {
                        curSettingId = ids[0].ToString(); //有条件配置项目
                    }
                    result.code = WfCode.Sucess; //获取成功
                    result.settingid = curSettingId;
                }
                #endregion
            }
            catch (Exception ex)
            {
                result.code = WfCode.Error; //程序出错
                return result;
            }

            return result;
        }
        #endregion

        #region 获取目标流程参与者及其他相关对象
        /// <summary>
        /// 获取目标流程参与者及其他相关对象
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="result"></param>
        /// <param name="curSettingId"></param>
        /// <returns></returns>
        public WfControlResult GetTargetFlowObj(WfControlObj obj, WfControlResult result)
        {
            WfSettingEntity entity = wfsettingiservice.GetEntity(obj.cursettingid); //获取对应配置项 

            string lastaccount = string.Empty;

            string lastdeptname = string.Empty; //部门名称

            string lastdeptcode = string.Empty;//部门编码

            string lastusername = string.Empty;  //用户名称

            try
            {
                //自动处理
                #region 自动处理
                if (entity.ISAUTOHANDLE == "是")
                {
                    //是否更改状态
                    if (entity.ISUPDATEFLOW == "是")
                    {
                        result.wfflag = entity.WFFLAG; //流程流转Flag标记
                    }
                    //是否执行脚本获取对应的目标流程参与人
                    #region 目标流程执行脚本获取参与者
                    if (entity.ISEXCUTESQL == "是")
                    {
                        #region sql语句
                        if (!string.IsNullOrEmpty(entity.SCRIPTCONTENT))
                        {
                            try
                            {
                                string curstartsql = entity.SCRIPTCONTENT;
                                List<IndexOfModel> mlist = new List<IndexOfModel>();
                                mlist = SubstringCountList(curstartsql, "@id", mlist);
                                mlist = SubstringCountList(curstartsql, "@argument1", mlist);
                                mlist = SubstringCountList(curstartsql, "@argument2", mlist);
                                mlist = SubstringCountList(curstartsql, "@argument3", mlist);
                                mlist = SubstringCountList(curstartsql, "@argument4", mlist);
                                mlist = SubstringCountList(curstartsql, "@argument5", mlist);
                                mlist = SubstringCountList(curstartsql, "@argument6", mlist);
                                mlist = mlist.OrderBy(p => p.indexValue).ToList();
                                var parameter = new List<DbParameter>();
                                for (int i = 0; i < mlist.Count(); i++)
                                {
                                    //取脚本，获取账户的范围信息
                                    if (mlist[i].indexName == "@id")
                                    {
                                        parameter.Add(DbParameters.CreateDbParameter("@id", !string.IsNullOrEmpty(obj.businessid) ? obj.businessid : ""));
                                    }
                                    //自定义参数
                                    else if (mlist[i].indexName == "@argument1")
                                    {
                                        parameter.Add(DbParameters.CreateDbParameter("@argument1", !string.IsNullOrEmpty(obj.argument1) ? obj.argument1 : ""));
                                    }
                                    //自定义参数
                                    else if (mlist[i].indexName == "@argument2")
                                    {
                                        parameter.Add(DbParameters.CreateDbParameter("@argument2", !string.IsNullOrEmpty(obj.argument2) ? obj.argument2 : ""));
                                    }
                                    //自定义参数
                                    else if (mlist[i].indexName == "@argument3")
                                    {
                                        parameter.Add(DbParameters.CreateDbParameter("@argument3", !string.IsNullOrEmpty(obj.argument3) ? obj.argument3 : ""));
                                    }
                                    //自定义参数
                                    else if (mlist[i].indexName == "@argument4")
                                    {
                                        parameter.Add(DbParameters.CreateDbParameter("@argument4", !string.IsNullOrEmpty(obj.argument4) ? obj.argument4 : ""));
                                    }
                                    //自定义参数
                                    else if (mlist[i].indexName == "@argument5")
                                    {
                                        parameter.Add(DbParameters.CreateDbParameter("@argument5", !string.IsNullOrEmpty(obj.argument5) ? obj.argument5 : ""));
                                    }
                                    //自定义参数
                                    else if (mlist[i].indexName == "@argument6")
                                    {
                                        parameter.Add(DbParameters.CreateDbParameter("@argument6", !string.IsNullOrEmpty(obj.argument6) ? obj.argument6 : ""));
                                    }
                                }
                                DbParameter[] arrayparam = parameter.ToArray();
                                DataTable joinDt = wfsettingiservice.GetGeneralQuery(entity.SCRIPTCONTENT, arrayparam);
                                if (joinDt.Rows.Count > 0)
                                {
                                    #region MyRegion
                                    foreach (DataRow row in joinDt.Rows)
                                    {
                                        string taccount = row["account"].ToString();

                                        if (!string.IsNullOrEmpty(taccount))
                                        {
                                            if (string.IsNullOrEmpty(lastaccount))
                                            {
                                                lastaccount = ",";
                                            }
                                            if (!lastaccount.Contains("," + taccount + ","))
                                            {
                                                lastaccount += taccount + ",";
                                            }
                                        }
                                        //用户名称
                                        if (joinDt.Columns.Contains("username"))
                                        {
                                            string tusername = row["username"].ToString();
                                            if (!string.IsNullOrEmpty(tusername))
                                            {
                                                if (string.IsNullOrEmpty(lastusername))
                                                {
                                                    lastusername = ",";
                                                }
                                                if (!lastusername.Contains("," + tusername + ","))
                                                {
                                                    lastusername += tusername + ",";
                                                }
                                            }
                                        }
                                        //部门名称
                                        if (joinDt.Columns.Contains("deptname"))
                                        {
                                            string tdeptname = row["deptname"].ToString();

                                            if (!string.IsNullOrEmpty(tdeptname))
                                            {
                                                if (string.IsNullOrEmpty(lastdeptname))
                                                {
                                                    lastdeptname = ",";
                                                }
                                                if (!lastdeptname.Contains("," + tdeptname + ","))
                                                {
                                                    lastdeptname += tdeptname + ",";
                                                }
                                            }
                                        }
                                        //部门编码
                                        if (joinDt.Columns.Contains("deptcode"))
                                        {
                                            string tdeptcode = row["deptcode"].ToString();
                                            if (!string.IsNullOrEmpty(tdeptcode))
                                            {
                                                if (string.IsNullOrEmpty(lastdeptcode))
                                                {
                                                    lastdeptcode = ",";
                                                }
                                                if (!lastdeptcode.Contains("," + tdeptcode + ","))
                                                {
                                                    lastdeptcode += tdeptcode + ",";
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                }
                            }
                            catch (Exception ex)
                            {
                                result.code = WfCode.EndSqlError; //目标流程脚本出错
                                return result;
                            } 
                        }
                        else
                        {
                            result.code = WfCode.NoScriptSQL; //目标流程参与者脚本未定义
                        }  
                        #endregion
                    }
                    #endregion
                    #region 通过目标流程条件获取
                    else  //获取对应目标流程的条件配置 
                    {
                        #region 获取对应目标流程的条件配置
                        try
                        {
                            #region 指定具体的部门形式
                            string wfflag = string.Empty;
                            obj.depttype = string.Empty; //将原有起始流程使用的部门性质赋值为空
                            //目标条件处理环节
                            DataTable dt = wfsettingiservice.GetWfSettingForInstance(obj, "目标流程", entity.ID); //获取起始流程下的流程配置及条件内容
                            if (dt.Rows.Count > 0)
                            {
                                List<AccountQueryObj> list = HandleFlowCondition(obj, dt, result, out wfflag); //目标流程参与者
                                if (!string.IsNullOrEmpty(wfflag)) { result.wfflag = wfflag; }
                                #region 目标流程参与者结果

                                foreach (AccountQueryObj aqobj in list)
                                {
                                    //账号
                                    string taccount = aqobj.account;
                                    if (!string.IsNullOrEmpty(taccount))
                                    {
                                        if (string.IsNullOrEmpty(lastaccount))
                                        {
                                            lastaccount = ",";
                                        }
                                        if (!lastaccount.Contains("," + taccount + ","))
                                        {
                                            lastaccount += taccount + ",";
                                        }
                                    }
                                    //部门
                                    string tdeptname = aqobj.deptname;
                                    if (!string.IsNullOrEmpty(tdeptname))
                                    {
                                        if (string.IsNullOrEmpty(lastdeptname))
                                        {
                                            lastdeptname = ",";
                                        }
                                        if (!lastdeptname.Contains("," + tdeptname + ","))
                                        {
                                            lastdeptname += tdeptname + ",";
                                        }
                                    }
                                    //部门编码
                                    string tdeptcode = aqobj.deptcode;
                                    if (!lastdeptcode.Contains(tdeptcode))
                                    {
                                        if (string.IsNullOrEmpty(lastdeptcode))
                                        {
                                            lastdeptcode = ",";
                                        }
                                        if (!lastdeptcode.Contains("," + tdeptcode + ","))
                                        {
                                            lastdeptcode += tdeptcode + ",";
                                        }
                                    }
                                    //用户
                                    string tusername = aqobj.username;
                                    if (!lastusername.Contains(tusername))
                                    {
                                        if (string.IsNullOrEmpty(lastusername))
                                        {
                                            lastusername = ",";
                                        }
                                        if (!lastusername.Contains("," + tusername + ","))
                                        {
                                            lastusername += tusername + ",";
                                        }
                                    }
                                }
                                #endregion
                            }
                            #endregion
                        }
                        catch (Exception)
                        {
                            result.code = WfCode.EndSettingError; //目标流程设置出错
                            return result;
                        }
                        #endregion
                    }
                    #endregion
                    #region 归纳结果

                    if (!string.IsNullOrEmpty(lastaccount))
                    {
                        //账户
                        if (lastaccount.Length > 2)
                        {
                            lastaccount = lastaccount.Substring(1, lastaccount.Length - 2);//  返回格式：',xxxx,xxx,yxyxy,'
                            result.actionperson = lastaccount;
                        }
                        //部门编码
                        if (!string.IsNullOrEmpty(lastdeptcode))
                        {
                            result.deptcode = lastdeptcode; //  返回格式：',xxxx,xxx,yxyxy,'
                        }
                        //部门名称
                        if (!string.IsNullOrEmpty(lastdeptname))
                        {
                            result.deptname = lastdeptname.Substring(1, lastdeptname.Length - 2); //  返回格式：'xxxx,xxx,yxyxy'
                        }
                        //用户名称
                        if (!string.IsNullOrEmpty(lastusername))
                        {
                            result.username = lastusername.Substring(1, lastusername.Length - 2);//  返回格式：'xxxx,xxxyy'
                        }
                        result.code = WfCode.Sucess;//成功
                    }
                    else
                    {
                        result.code = WfCode.NoAccount; //未定义目标流程参与者
                    }
                    #endregion
                }
                else
                {
                    result.code = WfCode.NoAutoHandle; //非自动处理
                }
                #endregion
            }
            catch (Exception ex)
            {
                result.code = WfCode.Error; //程序出错
                return result;
            }
            return result;
        }
        #endregion

        #region 处理流程条件获取对应的参与者账户
        /// <summary>
        /// 处理流程条件获取对应的参与者账户
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="dt"></param>
        /// <param name="wfflag"></param>
        /// <returns></returns>
        public List<AccountQueryObj> HandleFlowCondition(WfControlObj obj, DataTable dt, WfControlResult result, out string wfflag)
        {
            string curdeptid = string.Empty;
            string curparentid = string.Empty;
            string curprojectid = string.Empty;
            if (null != obj.user)
            {
                curdeptid = obj.user.DeptId;
                curparentid = obj.user.ParentId;
                curprojectid = obj.user.ProjectID;
            }
            if (null != obj.spuser)
            {
                curdeptid = obj.spuser.DepartmentId;
                curparentid = obj.spuser.ParentId;
                curprojectid = obj.spuser.ProjectId;
            }

            IList<UserInfoEntity> ulist = new List<UserInfoEntity>(); //包含的用户

            IList<UserInfoEntity> wlist = new List<UserInfoEntity>(); //不包含的用户

            List<AccountQueryObj> list = new List<AccountQueryObj>();  //包含相应角色的用户 

            IList<UserInfoEntity> youlist = new List<UserInfoEntity>(); //包含的用户

            IList<UserInfoEntity> meilist = new List<UserInfoEntity>(); //不包含的用户

            wfflag = "";
            //遍历条件对象
            foreach (DataRow row in dt.Rows)
            {
                //条件id
                string conditionid = row["conditionid"].ToString();
                //判定当前配置是否存在条件内容
                #region 判定当前配置是否存在条件内容
                if (!string.IsNullOrEmpty(conditionid))
                {
                    //是否包含当前角色
                    string orgid = row["organizeid"].ToString();  //所属单位
                    bool rolerule = row["rolerule"].ToString() == "包含" ? true : false;  //角色判定规则
                    string sdepttype = row["depttype"].ToString(); //部门性质
                    string sroletype = row["roletype"].ToString();  //角色性质
                    string srolecode = row["rolecode"].ToString();  //角色性质
                    string choosetype = row["choosetype"].ToString(); //针对当前人员选择的部门
                    string isexecsql = row["isexecsql"].ToString();  //是否执行条件脚本
                    string sqlcontent = row["sqlcontent"].ToString(); //sql执行脚本

                    //执行脚本
                    if (isexecsql == "是")
                    {
                        #region 脚本获取起始对象条件
                        if (!string.IsNullOrEmpty(sqlcontent))
                        {
                            List<IndexOfModel> mlist = new List<IndexOfModel>();
                            mlist = SubstringCountList(sqlcontent, "@id", mlist);
                            mlist = SubstringCountList(sqlcontent, "@argument1", mlist);
                            mlist = SubstringCountList(sqlcontent, "@argument2", mlist);
                            mlist = SubstringCountList(sqlcontent, "@argument3", mlist);
                            mlist = SubstringCountList(sqlcontent, "@argument4", mlist);
                            mlist = SubstringCountList(sqlcontent, "@argument5", mlist);
                            mlist = SubstringCountList(sqlcontent, "@argument6", mlist);
                            mlist = mlist.OrderBy(p => p.indexValue).ToList();
                            var parameter = new List<DbParameter>();
                            for (int i = 0; i < mlist.Count(); i++)
                            {
                                //取脚本，获取账户的范围信息
                                if (mlist[i].indexName == "@id")
                                {
                                    parameter.Add(DbParameters.CreateDbParameter("@id", !string.IsNullOrEmpty(obj.businessid) ? obj.businessid : ""));
                                }
                                //自定义参数
                                else if (mlist[i].indexName == "@argument1")
                                {
                                    parameter.Add(DbParameters.CreateDbParameter("@argument1", !string.IsNullOrEmpty(obj.argument1) ? obj.argument1 : ""));
                                }
                                //自定义参数
                                else if (mlist[i].indexName == "@argument2")
                                {
                                    parameter.Add(DbParameters.CreateDbParameter("@argument2", !string.IsNullOrEmpty(obj.argument2) ? obj.argument2 : ""));
                                }
                                //自定义参数
                                else if (mlist[i].indexName == "@argument3")
                                {
                                    parameter.Add(DbParameters.CreateDbParameter("@argument3", !string.IsNullOrEmpty(obj.argument3) ? obj.argument3 : ""));
                                }
                                //自定义参数
                                else if (mlist[i].indexName == "@argument4")
                                {
                                    parameter.Add(DbParameters.CreateDbParameter("@argument4", !string.IsNullOrEmpty(obj.argument4) ? obj.argument4 : ""));
                                }
                                //自定义参数
                                else if (mlist[i].indexName == "@argument5")
                                {
                                    parameter.Add(DbParameters.CreateDbParameter("@argument5", !string.IsNullOrEmpty(obj.argument5) ? obj.argument5 : ""));
                                }
                                //自定义参数
                                else if (mlist[i].indexName == "@argument6")
                                {
                                    parameter.Add(DbParameters.CreateDbParameter("@argument6", !string.IsNullOrEmpty(obj.argument6) ? obj.argument6 : ""));
                                }
                            }
                            DbParameter[] arrayparam = parameter.ToArray();
                            DataTable joinDt = wfsettingiservice.GetGeneralQuery(sqlcontent, arrayparam);
                            //起始用户账户匹配
                            if (joinDt.Rows.Count > 0)
                            {
                                string joinaccount = string.Empty;
                                foreach (DataRow jrow in joinDt.Rows)
                                {
                                    string rowaccount = jrow["account"].ToString();
                                    if (!string.IsNullOrEmpty(rowaccount))
                                    {
                                        joinaccount += rowaccount + ",";
                                    }
                                }
                                ulist = iuserservice.GetWFUserListByDeptRoleOrg(orgid, "", "", "", joinaccount);
                            }
                        }
                        #endregion
                    }
                    else 
                    {
                        //如果是确定部门选择形式为非其他部门，则依据部门形式来选择
                        if (!string.IsNullOrEmpty(choosetype) && choosetype != "其他")
                        {
                            #region 选择部门的形式(参考对象为起始流程的参与人)
                            if (!string.IsNullOrEmpty(choosetype))
                            {
                                #region 选择部门的形式
                                switch (choosetype)
                                {
                                    case "本部门":
                                        //本部门对应角色的人员
                                        if (rolerule)
                                        {
                                            ulist = iuserservice.GetWFUserListByDeptRoleOrg(orgid, curdeptid, string.Empty, srolecode, string.Empty);
                                        }
                                        else
                                        {
                                            wlist = iuserservice.GetWFUserListByDeptRoleOrg(orgid, curdeptid, string.Empty, srolecode, string.Empty);
                                        }
                                        break;
                                    case "本机构":
                                        //本机构对应角色的人员
                                        if (rolerule)
                                        {
                                            ulist = iuserservice.GetWFUserListByDeptRoleOrg(orgid, string.Empty, string.Empty, srolecode, string.Empty);
                                        }
                                        else
                                        {
                                            wlist = iuserservice.GetWFUserListByDeptRoleOrg(orgid, string.Empty, string.Empty, srolecode, string.Empty);
                                        }
                                        break;
                                    case "上级部门":
                                        //获取上级部门的对应角色用户
                                        #region 获取上级部门的对应角色用户
                                        var superior = idepartmentservice.GetParentDeptBySpecialArgs(curparentid, sdepttype);
                                        if (null != superior)
                                        {
                                            if (!string.IsNullOrEmpty(superior.DepartmentId))
                                            {
                                                if (rolerule)
                                                {
                                                    ulist = iuserservice.GetWFUserListByDeptRoleOrg(orgid, superior.DepartmentId, string.Empty, srolecode, string.Empty);
                                                }
                                                else
                                                {
                                                    wlist = iuserservice.GetWFUserListByDeptRoleOrg(orgid, superior.DepartmentId, string.Empty, srolecode, string.Empty);
                                                }
                                            }
                                        }
                                        #endregion
                                        break;
                                    case "上级部门(含专业过滤)":
                                        //获取上级部门的对应角色用户
                                        #region 获取上级部门的对应角色用户
                                        string smajorValue = string.Empty;
                                        //携带专业参数
                                        if (!string.IsNullOrEmpty(obj.argument1))
                                        {
                                            smajorValue = idataItemDetailService.GetEntity(obj.argument1).ItemValue;
                                        }
                                        var msuperior = idepartmentservice.GetParentDeptBySpecialArgs(curparentid, sdepttype);
                                        if (null != msuperior)
                                        {
                                            if (!string.IsNullOrEmpty(msuperior.DepartmentId))
                                            {
                                                if (rolerule)
                                                {
                                                    ulist = iuserservice.GetWFUserListByDeptRoleOrg(orgid, msuperior.DepartmentId, string.Empty, srolecode, string.Empty, string.Empty, smajorValue);
                                                }
                                                else
                                                {
                                                    wlist = iuserservice.GetWFUserListByDeptRoleOrg(orgid, msuperior.DepartmentId, string.Empty, srolecode, string.Empty, string.Empty, smajorValue);
                                                }
                                            }
                                        }
                                        #endregion
                                        break;
                                    case "指定部门":
                                        //获取指定部门账户
                                        #region 获取指定部门账户
                                        var pointdept = wfconditionaddtionservice.GetList(conditionid); //获取对应的指定部门条件
                                        IList<UserInfoEntity> haveList = new List<UserInfoEntity>();
                                        IList<UserInfoEntity> noList = new List<UserInfoEntity>();
                                        foreach (WfConditionAddtionEntity addEntity in pointdept)
                                        {
                                            string addrole = string.Empty;
                                            string adduseraccount = string.Empty;
                                            string deptid = addEntity.DEPTID; //指定的部门
                                            string ishrole = addEntity.ISHROLE; //是否指定角色
                                            string ishuser = addEntity.ISHUSER; //是否指定用户
                                            if (ishrole == "是")
                                            {
                                                addrole = addEntity.ROLECODE; //角色的id;
                                            }
                                            if (ishuser == "是")
                                            {
                                                adduseraccount = addEntity.USERACCOUNT; //指定人员的账号 
                                            }
                                            if (rolerule)
                                            {
                                                haveList = iuserservice.GetWFUserListByDeptRoleOrg(addEntity.ORGANIZEID, deptid, string.Empty, addrole, adduseraccount);
                                            }
                                            else
                                            {
                                                noList = iuserservice.GetWFUserListByDeptRoleOrg(addEntity.ORGANIZEID, deptid, string.Empty, addrole, adduseraccount);
                                            }

                                            //包含项
                                            foreach (UserInfoEntity entity in haveList)
                                            {
                                                if (ulist.Where(p => p.UserId == entity.UserId).Count() == 0)
                                                {
                                                    ulist.Add(entity);
                                                }
                                            }
                                            //不包含项
                                            foreach (UserInfoEntity entity in noList)
                                            {
                                                if (wlist.Where(p => p.UserId == entity.UserId).Count() == 0)
                                                {
                                                    wlist.Add(entity);
                                                }
                                            }
                                        }
                                        #endregion
                                        break;
                                    case "发包部门":
                                        #region 发包部门
                                        if (!string.IsNullOrEmpty(curprojectid))
                                        {
                                            string engineerletdeptid = outsouringengineeriservice.GetEntity(curprojectid).ENGINEERLETDEPTID; //发包部门 
                                            if (!string.IsNullOrEmpty(engineerletdeptid))
                                            {
                                                ulist = iuserservice.GetWFUserListByDeptRoleOrg(orgid, engineerletdeptid, string.Empty, srolecode, string.Empty);
                                            }
                                        }
                                        else
                                        {
                                            List<string> eperson = new List<string>();
                                            //获取当前人下的所有外包工程
                                            DataTable engineerdt = outsouringengineeriservice.GetEngineerByCurrDept();
                                            foreach (DataRow erow in engineerdt.Rows)
                                            {
                                                string engineerletdeptid = erow["engineerletdeptid"].ToString();
                                                if (!string.IsNullOrEmpty(engineerletdeptid))
                                                {
                                                    var tempuserlist = iuserservice.GetWFUserListByDeptRoleOrg(orgid, engineerletdeptid, string.Empty, srolecode, string.Empty);//获取对应外包工程下的责任人
                                                    foreach (UserInfoEntity uiEntity in tempuserlist)
                                                    {
                                                        if (ulist.Where(p => p.UserId == uiEntity.UserId).Count() == 0)
                                                        {
                                                            ulist.Add(uiEntity);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                        break;
                                    case "发包部门(含专业过滤)":
                                        string majorValue = string.Empty;
                                        //携带专业参数
                                        if (!string.IsNullOrEmpty(obj.argument1))
                                        {
                                            majorValue = idataItemDetailService.GetEntity(obj.argument1).ItemValue;
                                        }
                                        #region 发包部门(含专业参数)
                                        if (!string.IsNullOrEmpty(curprojectid))
                                        {
                                            string engineerletdeptid = outsouringengineeriservice.GetEntity(curprojectid).ENGINEERLETDEPTID; //发包部门 
                                            if (!string.IsNullOrEmpty(engineerletdeptid))
                                            {
                                                ulist = iuserservice.GetWFUserListByDeptRoleOrg(orgid, engineerletdeptid, string.Empty, srolecode, string.Empty, string.Empty, majorValue);
                                            }
                                        }
                                        else
                                        {
                                            List<string> eperson = new List<string>();
                                            //获取当前人下的所有外包工程
                                            DataTable engineerdt = outsouringengineeriservice.GetEngineerByCurrDept();
                                            foreach (DataRow erow in engineerdt.Rows)
                                            {
                                                string engineerletdeptid = erow["engineerletdeptid"].ToString();
                                                if (!string.IsNullOrEmpty(engineerletdeptid))
                                                {
                                                    var tempuserlist = iuserservice.GetWFUserListByDeptRoleOrg(orgid, engineerletdeptid, string.Empty, srolecode, string.Empty, string.Empty, majorValue);//获取对应外包工程下的责任人
                                                    foreach (UserInfoEntity uiEntity in tempuserlist)
                                                    {
                                                        if (ulist.Where(p => p.UserId == uiEntity.UserId).Count() == 0)
                                                        {
                                                            ulist.Add(uiEntity);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                        break;
                                    case "上一流程生产者":
                                        #region 上一流程生产者
                                        if (!string.IsNullOrEmpty(obj.businessid))
                                        {
                                            DataTable prevDt = htworkflowiservice.GetBackFlowObjectByKey(obj.businessid);
                                            if (prevDt.Rows.Count > 0)
                                            {
                                                string prevAccount = prevDt.Rows[0]["participant"].ToString();  //上一流程生产者
                                                wfflag = prevDt.Rows[0]["wfflag"].ToString();
                                                string createuser = prevDt.Rows[0]["createuser"].ToString();
                                                bool isupdate = prevDt.Rows[0]["isupdate"].ToString() == "1"; //是否改变了流程状态  
                                                //推进的流程节点中操作人和创建人不一致时，流程状态未改，且更换了操作人
                                                result.ischangestatus = isupdate;
                                                result.isspecialchange = isupdate;
                                                if (!string.IsNullOrEmpty(prevAccount))
                                                {
                                                    var prevUserInfo = iuserservice.GetUserInfoByAccount(prevAccount);
                                                    ulist.Add(prevUserInfo);
                                                }
                                            }
                                        }
                                        #endregion
                                        break;
                                    case "脚本":
                                        #region 脚本获取起始对象条件
                                        if (!string.IsNullOrEmpty(sqlcontent))
                                        {
                                            var parameter = new List<DbParameter>();
                                            //取脚本，获取账户的范围信息
                                            if (sqlcontent.Contains("@id"))
                                            {
                                                parameter.Add(DbParameters.CreateDbParameter("@id", !string.IsNullOrEmpty(obj.businessid) ? obj.businessid : ""));
                                            }
                                            //自定义参数
                                            if (sqlcontent.Contains("@argument1"))
                                            {
                                                parameter.Add(DbParameters.CreateDbParameter("@argument1", !string.IsNullOrEmpty(obj.argument1) ? obj.argument1 : ""));
                                            }
                                            //自定义参数
                                            if (sqlcontent.Contains("@argument2"))
                                            {
                                                parameter.Add(DbParameters.CreateDbParameter("@argument2", !string.IsNullOrEmpty(obj.argument2) ? obj.argument2 : ""));
                                            }
                                            //自定义参数
                                            if (sqlcontent.Contains("@argument3"))
                                            {
                                                parameter.Add(DbParameters.CreateDbParameter("@argument3", !string.IsNullOrEmpty(obj.argument3) ? obj.argument3 : ""));
                                            }
                                            //自定义参数
                                            if (sqlcontent.Contains("@argument4"))
                                            {
                                                parameter.Add(DbParameters.CreateDbParameter("@argument4", !string.IsNullOrEmpty(obj.argument4) ? obj.argument4 : ""));
                                            }
                                            //自定义参数
                                            if (sqlcontent.Contains("@argument5"))
                                            {
                                                parameter.Add(DbParameters.CreateDbParameter("@argument5", !string.IsNullOrEmpty(obj.argument5) ? obj.argument5 : ""));
                                            }
                                            //自定义参数
                                            if (sqlcontent.Contains("@argument6"))
                                            {
                                                parameter.Add(DbParameters.CreateDbParameter("@argument6", !string.IsNullOrEmpty(obj.argument6) ? obj.argument6 : ""));
                                            }
                                            DbParameter[] arrayparam = parameter.ToArray();
                                            DataTable joinDt = wfsettingiservice.GetGeneralQuery(sqlcontent, arrayparam);
                                            //起始用户账户匹配
                                            if (joinDt.Rows.Count > 0)
                                            {
                                                string joinaccount = string.Empty;
                                                foreach (DataRow jrow in joinDt.Rows)
                                                {
                                                    string rowaccount = jrow["account"].ToString();
                                                    if (!string.IsNullOrEmpty(rowaccount))
                                                    {
                                                        joinaccount += rowaccount + ",";
                                                    }
                                                }
                                                if (rolerule)
                                                {
                                                    ulist = iuserservice.GetWFUserListByDeptRoleOrg("", "", "", "", joinaccount);
                                                }
                                                else
                                                {
                                                    wlist = iuserservice.GetWFUserListByDeptRoleOrg("", "", "", "", joinaccount);
                                                }
                                            }
                                        }
                                        #endregion
                                        break;
                                    default:
                                        break;
                                }
                                #endregion
                            }
                            #endregion
                        }
                        else
                        {     //按照所属单位+部门性质+角色性质进行判定
                            if (rolerule)
                            {
                                ulist = iuserservice.GetWFUserListByDeptRoleOrg(orgid, string.Empty, sdepttype, srolecode, string.Empty);
                            }
                            else
                            {
                                wlist = iuserservice.GetWFUserListByDeptRoleOrg(orgid, string.Empty, sdepttype, srolecode, string.Empty);
                            }
                        }
                    }
   
                }
                #endregion

                #region 汇总

                foreach (UserInfoEntity uentity in ulist)
                {
                    if (youlist.Where(p => p.UserId == uentity.UserId).Count() == 0)
                    {
                        youlist.Add(uentity);
                    }
                }


                foreach (UserInfoEntity mentity in wlist)
                {
                    if (meilist.Where(p => p.UserId == mentity.UserId).Count() == 0)
                    {
                        meilist.Add(mentity);
                    }
                }
                #endregion
            }

            if (meilist.Count() > 0)
            {
                foreach (UserInfoEntity entity in meilist)
                {
                    if (youlist.Where(p => p.UserId == entity.UserId).Count() > 0)
                    {
                        var curentity = youlist.Where(p => p.UserId == entity.UserId).FirstOrDefault();
                        youlist.Remove(curentity);
                    }
                }
            }
            foreach (UserInfoEntity entity in youlist)
            {
                if (list.Where(p => p.account == entity.Account).Count() == 0)
                {
                    AccountQueryObj aqobj = new AccountQueryObj();
                    aqobj.deptcode = entity.DepartmentCode;
                    aqobj.deptid = entity.DepartmentId;
                    aqobj.deptname = entity.DeptName;
                    aqobj.account = entity.Account;
                    aqobj.username = entity.RealName;
                    list.Add(aqobj);
                }
            }

            return list;
        }
        #endregion


        //public int SubstringCount(string sourcestr, string findstr) 
        //{
        //    int counter = 0;
        //    int startposition = 0;
        //    while (startposition < sourcestr.Length)
        //    {
        //        int search = sourcestr.IndexOf(findstr, startposition);
        //        if (search != -1)
        //        {
        //            counter++;
        //            startposition = search + findstr.Length;
        //        }
        //        else 
        //        {
        //            break;
        //        }
        //    }
        //    return counter;
        //}

        public List<IndexOfModel> SubstringCountList(string sourcestr, string findstr, List<IndexOfModel> list) 
        {
            int counter = 0;
            int startposition = 0;
            while (startposition < sourcestr.Length)
            {
                int search = sourcestr.IndexOf(findstr, startposition);
                if (search != -1)
                {
                    list.Add(new IndexOfModel { indexName = findstr, indexValue=search }); 
                    startposition = search + findstr.Length;
                }
                else
                {
                    break;
                }
            }
            return list;
        }
    }

    public class IndexOfModel 
    {
        public string indexName { get; set; }

        public int indexValue { get; set; }
    }    
}
