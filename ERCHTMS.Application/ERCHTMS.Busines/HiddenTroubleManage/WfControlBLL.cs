using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.IService.HiddenTroubleManage;
using ERCHTMS.Service.HiddenTroubleManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Busines.HiddenTroubleManage
{
    public class WfControlBLL
    {
        private WfControlIServices service = new WfControlServices();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        public WfControlResult GetWfControlObject(WfControlObj obj)
        {
            try
            {
                return service.GetWfControlObject(obj);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public WfControlResult GetWfControl(WfControlObj obj)
        {

            string organizeid = string.Empty;

            if (null != obj.user) 
            {
                organizeid = obj.user.OrganizeId;
            }
            if (null != obj.spuser)
            {
                organizeid = obj.spuser.OrganizeId;
            }
            if (string.IsNullOrEmpty(obj.organizeid)) 
            {
                obj.organizeid = organizeid;
            }
            //是否启用
            var wfenablerank = dataitemdetailbll.GetDataItemListByItemCode("'WorkFlowEnableRank'").Where(p=>p.ItemValue == obj.organizeid).ToList();

            //启用具体级别划分
            if (wfenablerank.Count() > 0)
            {
                //隐患专用级别id 
                if (!string.IsNullOrEmpty(obj.rankid))
                {
                    //隐患级别
                    var detailItem = dataitemdetailbll.GetDataItemListByItemCode("'HidRank'").ToList().Where(p => p.ItemDetailId == obj.rankid).FirstOrDefault();
                    obj.rankname = detailItem.ItemName;
                }
            }
            else 
            {
                if (!string.IsNullOrEmpty(obj.rankname)) 
                {
                    //如果是一般隐患，则推送到整改阶段  
                    if (obj.rankname.Contains("一般"))
                    {
                        obj.rankname = "一般隐患";
                    }
                    else
                    {
                        obj.rankname = "重大隐患";
                    }
                }
                //隐患专用级别id 
                if (!string.IsNullOrEmpty(obj.rankid))
                {
                    string rankname = string.Empty;
                    //隐患级别
                    var detailItem = dataitemdetailbll.GetDataItemListByItemCode("'HidRank'").ToList().Where(p => p.ItemDetailId == obj.rankid).FirstOrDefault();
                    //如果是一般隐患，则推送到整改阶段  
                    if (detailItem.ItemName.Contains("一般"))
                    {
                        rankname = "一般隐患";
                    }
                    else
                    {
                        rankname = "重大隐患";
                    }
                    obj.rankname = rankname;
                }
            }
            if (string.IsNullOrEmpty(obj.rankname)) { obj.rankname = "无"; } 
         
   
            WfControlResult result = GetWfControlObject(obj);
            if (result.code == WfCode.Sucess)
            {
                result.message = "处理成功";
            }
           else if (result.code == WfCode.NoEnable)
            {
                result.message = "未启用流程实例";
            }
            else if (result.code == WfCode.NoInstance)
            {
                result.message = "未配置流程实例";
            }
            else if (result.code == WfCode.NoSetting)
            {
                result.message = "未配置流程流转项";
            }
            else if (result.code == WfCode.NoAutoHandle)
            {
                result.message = "非自动处理的流程";
            }
            else if (result.code == WfCode.NoScriptSQL)
            {
                result.message = "目标流程参与者脚本未定义";
            }
            else if (result.code == WfCode.NoAccount)
            {
                result.message = "目标流程参与者未定义";
            }
            else if (result.code == WfCode.Error)
            {
                result.message = "程序出错，请联系管理员";
            }
            else if (result.code == WfCode.InstanceError)
            {
                result.message = "获取流程实例出错，请联系管理员";
            }
            else if (result.code == WfCode.FilterError)
            {
                result.message = "过滤流程配置出错，请联系管理员";
            }
            else if (result.code == WfCode.TargetError)
            {
                result.message = "获取目标流程配置出错，请联系管理员";
            }
            else if (result.code == WfCode.StartSqlError)
            {
                result.message = "起始流程脚本设置出错，请联系管理员";
            }
            else if (result.code == WfCode.StartSettingError)
            {
                result.message = "起始流程条件设置出错，请联系管理员";
            }
            else if (result.code == WfCode.EndSqlError)
            {
                result.message = "目标流程脚本设置出错，请联系管理员";
            }
            else if (result.code == WfCode.EndSettingError)
            {
                result.message = "目标流程条件设置出错，请联系管理员";
            }

            return result;
        }

    }
}
