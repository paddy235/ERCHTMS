using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.HiddenTroubleManage
{
    public class Flow
    {
        public int initNum { get; set; }
        public List<lines> lines { get; set; }
        public List<nodes> nodes { get; set; }
        public string title { get; set; }
        public string activeID { get; set; } //当前活动的流程主键
    }

    public class lines
    {
        public bool alt { get; set; }
        public string from { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string to { get; set; }
        public string type { get; set; }

    }

    public class nodes
    {
        public bool alt { get; set; }  //startround  开始  stepnode 节点   endround  结束
        public string css { get; set; }
        public int height { get; set; }
        public string id { get; set; }
        public string img { get; set; }
        public int left { get; set; }
        public string name { get; set; }
        public int top { get; set; }
        public string type { get; set; }
        public int width { get; set; }
        public bool isclick { get; set; }
        public setInfo setInfo { get; set; }
        public string url { get; set; }

    }
    //item.setInfo.Taged
    public class setInfo
    {
        public string NodeName { get; set; }  //节点名称
        public string NodeCode { get; set; } //节点id
        public int? Taged { get; set; }   //  1  为通过

        public List<NodeDesignateData> NodeDesignateData { get; set; }
    }

    public class NodeDesignateData
    {
        public string createdate { get; set; }
        public string creatdept { get; set; }
        public string createuser { get; set; }
        public string status { get; set; }
        public string prevnode { get; set; }
        public string nextuser { get; set; }
    }

    /// <summary>
    /// 流程返回对象
    /// </summary>
    public class PushData
    {
        public string ProcessName { get; set; }
        public string NextActivityName { get; set; }
        public int IsSucess { get; set; }

    }

    public class ResultData
    {
        public string key { get; set; }
        public string name { get; set; }
        public string value { get; set; }

    }

}
