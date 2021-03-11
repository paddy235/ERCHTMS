using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Collections;
using System.Collections.Generic;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Busines.SystemManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Entity.HiddenTroubleManage;



namespace ERCHTMS.Web.Areas.SystemManage.Controllers
{
    /// <summary>
    /// 描 述：流程实例
    /// </summary>
    public class ProcessController : MvcControllerBase
    {
        private WfTBProcessBLL processbll = new WfTBProcessBLL();
        private WfTBActivityBLL activitybll = new WfTBActivityBLL();
        private WfTBConditionBLL conditionbll = new WfTBConditionBLL();

        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }


        /// <summary>
        /// 流程活动页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ActivityForm()
        {
            return View();
        }


        /// <summary>
        /// 流程Line页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LineForm()
        { 
            return View();
        }

        /// <summary>
        /// 节点Form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NodeForm()
        {
            return View();
        }
        #endregion

        #region 获取流程实例列表数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = processbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        #endregion

        #region 获取流程实例实体对象
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = processbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #endregion

        #region 删除流程实例
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            processbll.RemoveForm(keyValue);

            var activitylist = activitybll.GetList(keyValue);

            foreach (WfTBActivityEntity entity in activitylist)
            {
                activitybll.RemoveForm(entity.Id);
            }
            var conditionlist = conditionbll.GetList(keyValue);
            foreach (WfTBConditionEntity entity in conditionlist)
            {
                conditionbll.RemoveForm(entity.Id);
            }
            return Success("删除成功。");
        }
        #endregion

        #region 保存流程实例
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, WfTBProcessEntity entity)
        {
            processbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion

        #region 获取流程活动节点列表数据
        /// <summary>
        /// 获取对应流程实例下的流程活动节点
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult GetActionListJson(string keyValue)
        {
            var data = activitybll.GetList(keyValue);

            return Content(data.ToJson());
        }


        public ActionResult GetActionDataJson(string keyValue)
        {
            var data = activitybll.GetList(keyValue);

            var resultdata = data.Select(x => new { id = x.Id, name = x.Name });

            return Content(resultdata.ToJson());
        }
        #endregion

        #region 获取流程节点对象
        /// <summary>
        /// 获取流程节点对象
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult GetActivityEntityJson(string keyValue)
        {
            var data = activitybll.GetEntity(keyValue);
            return Content(data.ToJson());
        }
        #endregion

        #region 获取流程转向配置
        /// <summary>
        /// 获取流程转向配置
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult GetConditionEntity(string keyValue)
        {
            var data = conditionbll.GetEntity(keyValue);
            return Content(data.ToJson());
        } 
        #endregion

        #region  保存流程活动转向
        /// <summary>
        /// 保存流程节点
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveConditionForm(string keyValue, WfTBConditionEntity entity)
        {
            conditionbll.SaveForm(keyValue, entity);
            return Success("操作成功!");
        }
        #endregion

        #region  保存流程活动节点
        /// <summary>
        /// 保存流程节点
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveActivityForm(string keyValue, WfTBActivityEntity entity)
        {
            activitybll.SaveForm(keyValue, entity);
            return Success("操作成功!");
        }
        #endregion

        #region 删除流程活动节点
        /// <summary>
        /// 删除流程活动节点
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveActivityForm(string keyValue)
        {
            activitybll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        #endregion

        #region 获取工作流数据集合
        /// <summary>
        /// 获取工作流数据集合
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult LoadWorkFlowDesign(string keyValue)
        {

            //节点列表
            var nodeData = activitybll.GetList(keyValue);

            //线性列表
            var lineData = conditionbll.GetList(keyValue);

            List<nodes> nodeList = new List<nodes>();
            List<lines> lineList = new List<lines>();

            #region 创建nodes对象
            foreach (WfTBActivityEntity entity in nodeData)
            {
                nodes nodes = new nodes();

                nodes.alt = true;
                nodes.css = "";
                nodes.id = entity.Id; //主键
                nodes.img = "";
                nodes.name = entity.Name;
                nodes.type = entity.FormName;
                nodes.height = entity.FormHeight.Value;
                nodes.left = entity.GraphLeft.Value;
                nodes.top = entity.Graphtop.Value;
                nodes.width = entity.FormWidth.Value;
                nodeList.Add(nodes);
            }
            #endregion

            #region 创建lines对象
            foreach (WfTBConditionEntity entity in lineData)
            {
                lines lines = new lines();
                lines.alt = true;
                lines.id = entity.Id;
                lines.from = entity.ActivityId;
                lines.to = entity.ToActivityId;
                lines.name = "";
                lines.type = "sl";
                lineList.Add(lines);
            }
            #endregion


            Flow flow = new Flow();
            flow.activeID = keyValue;
            flow.title = "隐患流程图";
            flow.initNum = 22;
            flow.nodes = nodeList;
            flow.lines = lineList;


            var jsonData = new { flow = flow };

            return Content(jsonData.ToJson());
        }
        #endregion

        #region 保存工作流数据
        /// <summary>
        /// 保存工作流数据
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult SaveWorkFlowData(string queryJson)
        {

            var queryParam = queryJson.ToJObject();

            var flow = Newtonsoft.Json.JsonConvert.DeserializeObject<Flow>(queryParam["Flow"].ToString());  //flow

            var process = Newtonsoft.Json.JsonConvert.DeserializeObject<WfTBProcessEntity>(queryParam["Process"].ToString());  //ProcessEntity

            processbll.SaveForm(process.Id, process);  //创建或保存流程实例

            string keyValue = process.Id; //流程实例Id

            var sourceNodes = activitybll.GetList(keyValue); //流程节点对象

            var sourceLines = conditionbll.GetList(keyValue); //流程导向对象

            List<string> nodeids = new List<string>();

            List<string> lineids = new List<string>();

            IList<nodes> nodeList = flow.nodes;

            IList<lines> lineList = flow.lines;

            foreach (nodes node in nodeList)
            {
                WfTBActivityEntity entity = new WfTBActivityEntity();
                string nodeid = node.id;
                if (nodeid.Contains("FlowPanel_node"))
                {
                    nodeid = "";
                }
                if (!string.IsNullOrEmpty(nodeid)) 
                {
                    nodeids.Add(nodeid);
                }
                entity.Id = nodeid;
                entity.ProcessId = keyValue;  //流程Id
                entity.FormName = node.type; //
                entity.Name = node.name;
                entity.FormWidth = node.width;
                entity.FormHeight = node.height;
                entity.GraphLeft = node.left;
                entity.Graphtop = node.top;
                string kind = string.Empty;
                switch (node.type)
                {
                    case "startround":
                        kind = "开始节点";
                        break;
                    case "stepnode":
                        kind = "标准节点";
                        break;
                    case "endround":
                        kind = "结束节点";
                        break;
                }
                entity.Kind = kind;
                activitybll.SaveForm(entity.Id, entity);  //保存节点

                if (node.id.Contains("FlowPanel_node"))
                {
                    for (int i = 0; i < lineList.Count(); i++)
                    {
                        if (lineList[i].from == node.id)
                        {
                            lineList[i].from = entity.Id;
                        }
                        if (lineList[i].to == node.id)
                        {
                            lineList[i].to = entity.Id;
                        }
                    }
                }
            }


            foreach (lines line in lineList)
            {
                WfTBConditionEntity entity = new WfTBConditionEntity();
                string lineid = line.id;
                if (lineid.Contains("FlowPanel_line"))
                {
                    lineid = "";
                }
                if (!string.IsNullOrEmpty(lineid))
                {
                    lineids.Add(lineid);
                }
                entity.Id = lineid;
                entity.ActivityId = line.from;
                entity.ToActivityId = line.to;
                entity.ProcessId = keyValue;
                conditionbll.SaveForm(entity.Id, entity); ;
            }

            //删除多余的节点
            var dyNodes = sourceNodes.Where(p => !nodeids.Contains(p.Id));
            foreach (WfTBActivityEntity delEntity in dyNodes) 
            {
                activitybll.RemoveForm(delEntity.Id);
            }

            //删除多余的流程转向
            var dyLines = sourceLines.Where(p => !lineids.Contains(p.Id));
            foreach (WfTBConditionEntity delEntity in dyLines) 
            {
                conditionbll.RemoveForm(delEntity.Id);
            }
            return Success("保存成功!");
        }
        #endregion

        #region 判断当前编码是否存在
        /// <summary>
        /// 判断当前编码是否存在
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult ValidateHavaCode(string code)
        {
            var data = processbll.GetList("").Where(p => p.Code == code);

            return Content(data.Count().ToString());
        }
        #endregion
    }
}
