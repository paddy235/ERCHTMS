using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.HiddenTroubleManage
{
    /// <summary>
    /// 流程控制对象
    /// </summary>
    public class WfControlObj
    {
        public string instanceid { get; set; } //流程实例id
        public string businessid { get; set; } //业务主键
        public string argument1 { get; set; } //业务参数1
        public string argument2 { get; set; } //业务参数2
        public string argument3 { get; set; } //业务参数3 
        public string argument4 { get; set; } //业务参数4 
        public string argument5 { get; set; } //业务参数5 
        public string argument6 { get; set; } //业务参数6 
        public string organizeid { get; set; } //机构id (当前用户为省级用户时，传入对应电厂的organizedid)
        public string businessaccount { get; set; } //业务用户
        public string startflow { get; set; }  //起始流程状态 
        public string endflow { get; set; } //目标流程状态
        public string rankname { get; set; }  //级别(隐患)
        public string rankid { get; set; }  //级别(隐患)
        public string submittype { get; set; }  //提交类型  提交 上报  退回
        public string mark { get; set; } //流程标记  隐患排查 
        public Operator user { get; set; } //用户
        public UserInfoEntity spuser { get; set; } //用户 
        public string depttype { get; set; } //部门类型
        public string cursettingid { get; set; } //流程配置项id  
        public bool isvaliauth { get; set; }  //是否仅仅只是验证权限
        public string istoend { get; set; } //是否完结流程

    }


    /// <summary>
    /// 输出结果
    /// </summary>
    public class WfControlResult
    {
        public string wfflag { get; set; }
        public bool ishave { get; set; }  //是否有当前流程的操作权限(用于评估人上报控制) 
        public WfCode code { get; set; } //状态码
        public string message { get; set; }
        public bool isend { get; set; } //是否整改结束
        public bool ischangestatus { get; set; } //是否更改状态
        public string actionperson { get; set; }
        public string username { get; set; }
        public string deptname { get; set; }
        public string deptcode { get; set; }
        public string deptid { get; set; }
        public string settingid { get; set; }
        public bool isspecialchange { get; set; }

    }

    /// <summary>
    /// 账户筛选对象
    /// </summary>
    public class AccountQueryObj
    {
        public string account { get; set; }
        public string username { get; set; }
        public string deptname { get; set; }
        public string deptcode { get; set; }
        public string deptid { get; set; }
        public int addcount { get; set; }

    }


    public enum WfCode
    {
        /// <summary>
        /// 处理成功
        /// </summary>
        [Description("处理成功")]
        Sucess = 000000,

        /// <summary>
        /// 未配置流程实例
        /// </summary>
        [Description("未启用流程配置实例")]
        NoEnable = 000001,
        /// <summary>
        /// 未配置流程实例
        /// </summary>
        [Description("未配置流程实例")]
        NoInstance = 000002,
        /// <summary>
        /// 未配置流程项
        /// </summary>
        [Description("未配置流程项")]
        NoSetting = 000003,

        /// <summary>
        /// 非自动处理
        /// </summary>
        [Description("非自动处理")]
        NoAutoHandle = 000004,

        /// <summary>
        /// 目标流程参与者脚本未定义
        /// </summary>
        [Description("目标流程参与者脚本未定义")]
        NoScriptSQL = 000005,

        /// <summary>
        /// 目标流程参与者脚本未定义
        /// </summary>
        [Description("目标流程参与者未定义")]
        NoAccount = 000006,
        

        /// <summary>
        /// 程序错误，请联系管理员
        /// </summary>
        [Description("程序出错，请联系管理员")]
        Error = -000000,

        /// <summary>
        /// 获取流程实例出错，请联系管理员
        /// </summary>
        [Description("获取流程实例出错，请联系管理员")]
        InstanceError = -000001, 

        /// <summary>
        /// 过滤流程配置出错，请联系管理员
        /// </summary>
        [Description("过滤流程配置出错，请联系管理员")]
        FilterError = -000002 ,  

        /// <summary>
        /// 目标流程配置出错，请联系管理员
        /// </summary>
        [Description("获取目标流程配置出错，请联系管理员")]
        TargetError = -000003 , 

        /// <summary>
        /// 起始流程脚本配置出错，请联系管理员
        /// </summary>
        [Description("起始流程脚本配置出错，请联系管理员")]
        StartSqlError = -000004,  

        
        /// <summary>
        /// 起始流程条件配置出错，请联系管理员
        /// </summary>
        [Description("起始流程条件配置出错，请联系管理员")]
        StartSettingError = -000005 ,

        /// <summary>
        /// 目标流程脚本配置出错，请联系管理员
        /// </summary>
        [Description("目标流程脚本配置出错，请联系管理员")]
        EndSqlError = -000006,


        /// <summary>
        /// 目标流程条件配置出错，请联系管理员
        /// </summary>
        [Description("目标流程条件配置出错，请联系管理员")]
        EndSettingError = -000007   
    }
}
