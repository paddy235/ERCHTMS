using System;
using System.Collections.Generic;

namespace ERCHTMS.Entity.CarManage
{
    /// <summary>
    ///  安全帽检测事件报文
    /// </summary>
    public class HikSmartCamera
    {
        /// <summary>
        /// 
        /// </summary>
        public HikSmartCamera()
        { }
        /// <summary>
        /// 
        /// </summary>
        public string dataType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? recvTime { get; set; }
        /// <summary>
        /// 
        /// </summary>

        public DateTime? sendTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? dateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>

        public string ipAddress { get; set; }
        /// <summary>
        /// 
        /// </summary>

        public int? portNo { get; set; }
        /// <summary>
        /// 
        /// </summary>

        public int? channelID { get; set; }
        /// <summary>
        /// 
        /// </summary>

        public string eventType { get; set; }
        /// <summary>
        /// 
        /// </summary>

        public string eventDescription { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<SafetyHelmeDetection> safetyHelmeDetection { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class SafetyHelmeDetection
    {
        /// <summary>
        /// 
        /// </summary>
        public SafetyHelmeDetection()
        { }
        /// <summary>
        /// 
        /// </summary>

        public TargetAttruits targetAttrs { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string dataSource { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string imageUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Receipt rect { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class TargetAttruits
    {
        /// <summary>
        /// 
        /// </summary>
        public TargetAttruits()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public string imageServerCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string deviceIndexCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string cameraIndexCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string cameraAddress { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string longitude { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string latitude { get; set; }

    }


    /// <summary>
    /// 
    /// </summary>
    public class Receipt
    {
        /// <summary>
        /// 
        /// </summary>
        public Receipt()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public int height { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int width { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int x { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int y { get; set; }

    }


}
