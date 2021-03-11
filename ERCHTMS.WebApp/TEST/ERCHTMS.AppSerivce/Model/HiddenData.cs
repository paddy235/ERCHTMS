using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERCHTMS.AppSerivce.Model
{
    public class HiddenData
    {

        public string safetydetailid { get; set; } //安全检查
        public string problemid { get; set; }// 隐患编号
        public string hiddenid { get; set; } // 隐患主键
        public string deptname { get; set; } // 单位名称
        public string isselfchange { get; set; } // 是否本部门整改
        public string isformulate { get; set; } //是否已经制定了整改计划
        public string breakrulebehavior { get; set; } // 违章行为
        public string deptid { get; set; } // 单位id
        public string hidrank { get; set; } // 隐患级别id
        public string rankname { get; set; } // 隐患级别名称
        //public string hidname { get; set; } // 隐患名称
        public string hidtype { get; set; } // 隐患类型分类
        public string hidtypeid { get; set; } // 隐患类型分类id
        public string category { get; set; } // 隐患类别
        public string categoryid { get; set; } // 隐患类别id
        public string hidpoint { get; set; } // 隐患区域
        public string hidpointid { get; set; } // 隐患区域id
        public string hiddescribe { get; set; } // 隐患描述
        public string dutydept { get; set; }  // 整改责任部门
        public string dutydeptid { get; set; }  // 整改责任部门id
        public string dutydeptcode { get; set; }  // 整改责任部门code
        public string dutyperson { get; set; }  // 整改责任人
        public string dutypersonid { get; set; }  //整改责任人ID
        public string dutytel { get; set; }  // 整改责任人电话
        public string deadinetime { get; set; }  // 整改截至日期
        public string checkperson { get; set; }  // 验收人
        public string checkpersonid { get; set; }  //验收人id
        public string acceptdepartname { get; set; }  // 验收单位
        public string acceptdepartcode { get; set; }  // 验收单位(code)
        public string checktime { get; set; }  // 验收日期
        public string checkopinion { get; set; }  // 验收意见

        public string isupaccept { get; set; } //是否省级验收
        public string uploaddate { get; set; }  // 上传时间
        public string reformfinishdate { get; set; }  // 整改完成日期
        public string reformmeasure { get; set; }  // 整改措施
        public string reformdescribe { get; set; }  // 整改情况描述
        public string reformtype { get; set; }  //整改类型(0:普通 1:立即 2:限期)
        public string checkman { get; set; }  // 检查人员(排查人id)id
        public string checkmanname { get; set; }  // 检查人员(排查人)
        public string checkdeptname { get; set; }  // 排查单位
        public string checkdept { get; set; }  // 排查单位id(code)
        public string checkresult { get; set; }  // 验收情况(验收结果)
        public string problemstate { get; set; }  // 隐患状态
        public string engineerid { get; set; } //所属工程
        public string engineername { get; set; }  //所属工程名称
        public string hidplace { get; set; } //隐患地点
        public string reportdigest { get; set; } //隐患报告摘要 

        public string deviceid { get; set; }  // 设备id
        public string devicecode { get; set; }  // 设备编号
        public string devicename { get; set; }  // 设备名称
        public string monitorpersonid { get; set; }  // 厂级监控人员id
        public string monitorpersonname { get; set; }  // 厂级监控人员名称
        public string relevanceid { get; set; }  // 关联其他应用id
        public string relevancetype { get; set; }  // 关联其他应用标记
        public string majorclassify { get; set; }  // 专业分类
        public string majorclassifyname { get; set; }  // 专业分类名称
        public string hidname { get; set; }  // 隐患名称
        public string hidstatus { get; set; }  // 隐患现状
        public string hidconsequence { get; set; } //可能导致的后果


        public IList<Photo> problempics { get; set; } // 隐患图片
        public IList<Photo> reformpics { get; set; } // 整改图片
        public IList<Photo> attachment { get; set; } // 整改附件
        public IList<Photo> checkpics { get; set; } // 验收图片
        public IList<Photo> evaluatepics { get; set; } // 整改评估图片 
        public IList<string> deleteurl { get; set; }
        public string isexpose { get; set; } // 是否曝光
        public string planmanagecapital { get; set; }// 计划治理资金
        public string realitymanagecapital { get; set; } // 实际治理资金
        public string checkdate { get; set; } // 排查日期
        //public string backreason { get; set; } // 回退原因
        public string reformresult { get; set; }// 整改是否退回 1:通过 0:不通过
        public bool isenableback { get; set; } //是否禁用退回
        public string reformbackreason { get; set; } // 整改回退原因
        public string workstream { get; set; } // 隐患流程
        public string evaluateperson { get; set; } // 评估人
        public string evaluatepersonid { get; set; } // 评估人
        public string evaluateresult { get; set; } // 评估结果 1:通过 0:不通过
        public string estimatedate { get; set; } // 评估日期
        public string isyq { get; set; } // 是否延期过 y:延期过 n:没有延期过
        public string yqid { get; set; }

        /********复查验证*****/
        public string rechecksperson { get; set; } //复查验证人
        public string recheckspersonid { get; set; } //复查验证人  
        public string rechecksdepartcode { get; set; } //复查部门编码
        public string rechecksdepartname { get; set; } //复查部门名称
        public string rechecksdate { get; set; } //复查日期
        public string rechecksstatus { get; set; } //复查结果
        public string rechecksidea { get; set; } //复查意见
        // ------------双预控新增字段-----------
        public string checktype { get; set; } // 检查类型(id)
        public string checktypeid { get; set; } // 检查类型(名称)
        public string dangerlocation { get; set; } // 隐患地点
        public string reportsummary { get; set; } // 隐患报告摘要
        public string causereason { get; set; } // 隐患产生原因
        public string damagelevel { get; set; } //隐患危害程度
        public string preventmeasure { get; set; } // 防控措施
        public string reformplan { get; set; } // 隐患整改计划
        public string replan { get; set; } // 应急预案简述
        public string isgetafter { get; set; } // 是否挂牌督办 
        public string approvalperson { get; set; } // 评估人
        public string approvalpersonid { get; set; } // 评估人id
        public string approvaldate { get; set; } // 评估时间
        public string approvalresult { get; set; } // 评估结果
        public string approvalreason { get; set; } // 评估原因
        public string approvaldepartname { get; set; } //评估部门
        //public string approveresult { get; set; } // 核查结果 1:通过 0:不通过
        //public string approvedate { get; set; } // 核查日期
        public IList<Photo> approvalpics { get; set; } // 核查附件 
        public string isupsubmit { get; set; } // 是否上报
        public bool ishaveupsubmit { get; set; } //是否拥有上报权限
        public string applicationstatus { get; set; }  //延期状态
        public string actionperson { get; set; }  //可操作对象
        public int currole { get; set; }  //角色标识    
        // ------------延期申请-----------
        public PostPoneData postdata { get; set; }

        public bool isusechangeck { get; set; } //是否可操作是否本部门整改

        //-------------------------------
        //public string breakruleperson { get; set; }// 违章人员
        //public string breakrulepersonid { get; set; } // 违章人员id
        //public string trainframework { get; set; } // 培训模板
        //public string trainframeworkid { get; set; } // 培训模板id

    }

}