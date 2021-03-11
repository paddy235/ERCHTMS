using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.HighRiskWork.ViewModel
{
    public class PushMessageData
    {
        public string UserDept { get; set; }
        public string UserRole { get; set; }
        public string SendCode { get; set; }
        public string EntityId { get; set; }
        public int Success { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public string IsSpecial { get; set; }//0:否 1:是
        public string SpecialtyType { get; set; }//专业分类

        public string UserAccount { get; set; } //下一步审核人账号
    }
}
