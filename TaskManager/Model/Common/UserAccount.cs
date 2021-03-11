using System;

namespace Model.Common
{
    [Serializable]
    public class UserAccount
    {
        /// <summary>
        /// 用户GUID
        /// </summary>
        public string UserGUID { get; set; }

        /// <summary>
        /// 用户代码
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
}
