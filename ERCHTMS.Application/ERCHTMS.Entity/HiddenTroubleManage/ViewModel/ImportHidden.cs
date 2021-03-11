using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.HiddenTroubleManage.ViewModel
{
    public class ImportHidden
    {
        public int serialnumber { get; set; } //序号
        public string hidcode { get; set; } //隐患编码
        public string checkobj { get; set; }
        public string checkcontent { get; set; }
        public string relevanceid { get; set; }
        public string hiddenname { get; set; }
        public string rankid { get; set; }
        public string areacode { get; set; }
        public string areaname { get; set; }
        public string majorclassify { get; set; }
        public string hidtype { get; set; }
        public string devicename { get; set; }
        public string hiddescribe { get; set; }
        public string changepersonid { get; set; }
        public string changepersonname { get; set; }
        public string changetelephone { get; set; }
        public string changedeptcode { get; set; }
        public string changedeptid { get; set; }
        public string changedeptname { get; set; }
        public DateTime? changedeadline { get; set; }
        public string changemeasure { get; set; }
        public string acceptpersonid { get; set; }
        public string acceptpersonname { get; set; }
        public string acceptdeptcode { get; set; }
        public string acceptdeptname { get; set; }
        public DateTime? acceptdate { get; set; }
    }

    public class ImportLllegal
    {
        public int serialnumber { get; set; } //序号
        public string lllegalid { get; set; } //违章id
        public string lllegalnumber { get; set; } //违章编码
        public string checkobj { get; set; }  //检查对象
        public string checkcontent { get; set; } //检查内容
        public string reseverone { get; set; } //检查id
        public string resevertwo { get; set; } //检查内容id 
        public string reseverid { get; set; } //检查对象id
        public string lllegaltype { get; set; } //违章类型
        public string lllegallevel { get; set; } //违章级别
        public string majorclassify { get; set; } //专业分类id
        public string majorclassifyvalue { get; set; }  //专业分类值
        public string lllegalperson { get; set; }
        public string lllegalpersonid { get; set; }
        public string lllegalteam { get; set; } //违章单位
        public string lllegalteamid { get; set; } //违章单位
        public string lllegalteamcode { get; set; }
        public DateTime? lllegaltime { get; set; } //违章时间
        public string lllegaladdress { get; set; } //违章地点 
        public string lllegaldescribe { get; set; } //违章描述
        public string reformpeopleid { get; set; }
        public string reformpeople { get; set; }
        public string reformtelephone { get; set; }
        public string reformdeptcode { get; set; }
        public string reformdeptname { get; set; }
        public DateTime? reformdeadline { get; set; }
        public string reformrequire { get; set; }
        public string acceptpeopleid { get; set; }
        public string acceptpeople { get; set; }
        public string acceptdeptcode { get; set; }
        public string acceptdeptname { get; set; }
        public string lllegaldepart { get; set; } //违章责任单位
        public string lllegaldepartcode { get; set; } //违章责任单位编码
        public string wzdwpunish { get; set; } //违章单位考核金额
        public string zrdwpunish { get; set; } //责任单位考核金额
        public DateTime? accepttime { get; set; }
    }
    public class ImportLllegalExamin
    {
        public int serialnumber { get; set; } //序号
        public string lllegalid { get; set; } //违章id
        public string lllegalnumber { get; set; } //违章编码
        public string assessobject { get; set; }  //考核对象
        public string personinchargename { get; set; } //考核人员/单位
        public string personinchargeid { get; set; } //考核人员/单位
        public string economicspunish { get; set; } //经济处罚(元)
        public string lllegalpoint { get; set; } //违章扣分
        public string education { get; set; }//教育培训(学时)
        public string awaitjob { get; set; } //待岗(月)
        public string performancepoint { get; set; } //EHS绩效考核(分)
        public string awardusername { get; set; }   //奖励人员
        public string awarduserid { get; set; }   //奖励人员
        public string awarddeptid { get; set; }   //奖励部门id
        public string awarddeptname { get; set; }   //奖励部门名称
        public string awardpoint { get; set; } //奖励积分(分)
        public string awardmoney { get; set; } //奖励金额(元)
    }
    public class ImportQuestion  
    {
        public int serialnumber { get; set; } //序号
        public string questionnumber { get; set; } //问题编码
        public string checkobj { get; set; } //检查对象
        public string checkcontent { get; set; } //检查内容
        public string checkid { get; set; } //检查id
        public string relevanceid { get; set; } //检查对象id
        public string correlationid { get; set; } //检查内容id
        public string questionaddress { get; set; } //问题地点 
        public string questiondescribe { get; set; } //违章描述
        public string checkpersonname { get; set; } //检查人员
        public string checkpersonid { get; set; } //检查人员
        public string checkdeptname { get; set; } //检查单位
        public string checkdeptid { get; set; } //检查单位
        public string checkimpcontent { get; set; } //检查重点内容
        public string checkdate { get; set; } //检查日期
        public string checktype { get; set; } //检查类型
        public string checkname { get; set; } //检查名称
        public string dutydeptname { get; set; } //联责单位
        public string dutydeptid { get; set; } //联责单位
        public string dutydeptcode { get; set; } //联责单位
        public string reformpeople { get; set; }
        public string reformpeoplename { get; set; }  
        public string reformtelephone { get; set; }
        public string reformdeptid { get; set; }
        public string reformdeptcode { get; set; }
        public string reformdeptname { get; set; }
        public DateTime? reformplandate { get; set; }
        public string reformmeasure { get; set; }
        public string verifypeople { get; set; } 
        public string verifypeoplename { get; set; } 
        public string verifydeptcode { get; set; }
        public string verifydeptid { get; set; }
        public string verifydeptname { get; set; }
        public DateTime? verifydate { get; set; }
    }


    public class ImportQuantifyIndex
    {
        public string DeptId { get; set; }
        public string DeptName { get; set; }
        public string DutyId { get; set; }
        public string DutyName { get; set; }
        public string IndexValue { get; set; }
        public string YearValue { get; set; }
    }
}
