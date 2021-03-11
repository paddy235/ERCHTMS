using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.LllegalManage.ViewModel
{
    /// <summary>
    /// 违章档案对象
    /// </summary>
    public class LllegalRecord
    {
        public string year { get; set; }
        public string residuePoint { get; set; } //剩余个人积分
        public string lllegalcount { get; set; }  //个人违章总次数
        public string deductmarks { get; set; } //个人违章扣分
        public string penalty { get; set; } //个人罚款总额
        public string relatedcount { get; set; } //连带违章次数
        public string relatedpoint { get; set; } //连带违章扣分
        public string relatedpenalty { get; set; } //连带罚款
        public List<LllegalModel> data { get; set; }  //违章数据

    }

    public class LllegalModel
    {
        public string lllegalid { get; set; } //违章id
        public string lllegaltime { get; set; } //违章时间
        public string lllegaladdress { get; set; } //违章地点
        public string lllegaldescribe { get; set; } //违章描述
        public string lllegalpoint { get; set; } //违章扣分
        public string economicspunish { get; set; } //罚款 
        public string lllegalduty { get; set; } //违章责任
    }
}
