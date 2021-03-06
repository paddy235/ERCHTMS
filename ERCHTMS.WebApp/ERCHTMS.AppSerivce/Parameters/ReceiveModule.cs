namespace BSFramework.Application.AppSerivce
{
    /// <summary>
    /// 描 述:接收数据实体
    /// </summary>
    public class ReceiveModule<T>where T : class
    {
        /// <summary>
        /// 标记
        /// </summary>
        public string token { set; get; }
        /// <summary>
        /// 传送数据
        /// </summary>
        public T data { set; get; }
        /// <summary>
        /// 用户id（可选）
        /// </summary>
        public string userid { set; get; }
        /// <summary>
        /// 平台信息
        /// </summary>
        public string platform { set; get; }
    }

    public class ReceiveModule
    {
        /// <summary>
        /// 标记
        /// </summary>
        public string token { set; get; }
        /// <summary>
        /// 用户id（可选）
        /// </summary>
        public string userid { set; get; }
        /// <summary>
        /// 平台信息
        /// </summary>
        public string platform { set; get; }
    }
}