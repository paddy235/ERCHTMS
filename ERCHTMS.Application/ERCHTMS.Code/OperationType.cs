﻿using System.ComponentModel;

namespace ERCHTMS.Code
{
    /// <summary>
    /// 操作类型
    /// </summary>
    public enum OperationType
    {
        /// <summary>
        /// 其他
        /// </summary>
        [Description("其他")]
        Other = 0,
        /// <summary>
        /// 登陆
        /// </summary>
        [Description("登录")]
        Login = 1,
        /// <summary>
        /// 登陆
        /// </summary>
        [Description("退出")]
        Exit = 2,
        /// <summary>
        /// 访问
        /// </summary>
        [Description("访问")]
        Visit = 3,
        /// <summary>
        /// 离开
        /// </summary>
        [Description("离开")]
        Leave = 4,
        /// <summary>
        /// 保存
        /// </summary>
        [Description("保存")]
        Create = 5,
        /// <summary>
        /// 删除
        /// </summary>
        [Description("删除")]
        Delete = 6,
        /// <summary>
        /// 修改
        /// </summary>
        [Description("修改")]
        Update = 7,
        /// <summary>
        /// 提交
        /// </summary>
        [Description("提交")]
        Submit = 8,
        /// <summary>
        /// 异常
        /// </summary>
        [Description("异常")]
        Exception = 9,
        /// <summary>
        /// 异常
        /// </summary>
        [Description("移动登录")]
        AppLogin = 10,
        /// <summary>
        /// LADP数据同步日志
        /// </summary>
        [Description("LADP数据同步日志")]
        LdapSync = 11
    }
}
