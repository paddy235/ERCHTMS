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
    /// �� ��������ʵ��
    /// </summary>
    public class ProcessController : MvcControllerBase
    {
        private WfTBProcessBLL processbll = new WfTBProcessBLL();
        private WfTBActivityBLL activitybll = new WfTBActivityBLL();
        private WfTBConditionBLL conditionbll = new WfTBConditionBLL();

        #region ��ͼ����
        /// <summary>
        /// �б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }


        /// <summary>
        /// ���̻ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ActivityForm()
        {
            return View();
        }


        /// <summary>
        /// ����Lineҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LineForm()
        { 
            return View();
        }

        /// <summary>
        /// �ڵ�Form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NodeForm()
        {
            return View();
        }
        #endregion

        #region ��ȡ����ʵ���б�����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = processbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        #endregion

        #region ��ȡ����ʵ��ʵ�����
        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = processbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #endregion

        #region ɾ������ʵ��
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
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
            return Success("ɾ���ɹ���");
        }
        #endregion

        #region ��������ʵ��
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, WfTBProcessEntity entity)
        {
            processbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion

        #region ��ȡ���̻�ڵ��б�����
        /// <summary>
        /// ��ȡ��Ӧ����ʵ���µ����̻�ڵ�
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

        #region ��ȡ���̽ڵ����
        /// <summary>
        /// ��ȡ���̽ڵ����
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult GetActivityEntityJson(string keyValue)
        {
            var data = activitybll.GetEntity(keyValue);
            return Content(data.ToJson());
        }
        #endregion

        #region ��ȡ����ת������
        /// <summary>
        /// ��ȡ����ת������
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult GetConditionEntity(string keyValue)
        {
            var data = conditionbll.GetEntity(keyValue);
            return Content(data.ToJson());
        } 
        #endregion

        #region  �������̻ת��
        /// <summary>
        /// �������̽ڵ�
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
            return Success("�����ɹ�!");
        }
        #endregion

        #region  �������̻�ڵ�
        /// <summary>
        /// �������̽ڵ�
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
            return Success("�����ɹ�!");
        }
        #endregion

        #region ɾ�����̻�ڵ�
        /// <summary>
        /// ɾ�����̻�ڵ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveActivityForm(string keyValue)
        {
            activitybll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }
        #endregion

        #region ��ȡ���������ݼ���
        /// <summary>
        /// ��ȡ���������ݼ���
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult LoadWorkFlowDesign(string keyValue)
        {

            //�ڵ��б�
            var nodeData = activitybll.GetList(keyValue);

            //�����б�
            var lineData = conditionbll.GetList(keyValue);

            List<nodes> nodeList = new List<nodes>();
            List<lines> lineList = new List<lines>();

            #region ����nodes����
            foreach (WfTBActivityEntity entity in nodeData)
            {
                nodes nodes = new nodes();

                nodes.alt = true;
                nodes.css = "";
                nodes.id = entity.Id; //����
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

            #region ����lines����
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
            flow.title = "��������ͼ";
            flow.initNum = 22;
            flow.nodes = nodeList;
            flow.lines = lineList;


            var jsonData = new { flow = flow };

            return Content(jsonData.ToJson());
        }
        #endregion

        #region ���湤��������
        /// <summary>
        /// ���湤��������
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

            processbll.SaveForm(process.Id, process);  //�����򱣴�����ʵ��

            string keyValue = process.Id; //����ʵ��Id

            var sourceNodes = activitybll.GetList(keyValue); //���̽ڵ����

            var sourceLines = conditionbll.GetList(keyValue); //���̵������

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
                entity.ProcessId = keyValue;  //����Id
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
                        kind = "��ʼ�ڵ�";
                        break;
                    case "stepnode":
                        kind = "��׼�ڵ�";
                        break;
                    case "endround":
                        kind = "�����ڵ�";
                        break;
                }
                entity.Kind = kind;
                activitybll.SaveForm(entity.Id, entity);  //����ڵ�

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

            //ɾ������Ľڵ�
            var dyNodes = sourceNodes.Where(p => !nodeids.Contains(p.Id));
            foreach (WfTBActivityEntity delEntity in dyNodes) 
            {
                activitybll.RemoveForm(delEntity.Id);
            }

            //ɾ�����������ת��
            var dyLines = sourceLines.Where(p => !lineids.Contains(p.Id));
            foreach (WfTBConditionEntity delEntity in dyLines) 
            {
                conditionbll.RemoveForm(delEntity.Id);
            }
            return Success("����ɹ�!");
        }
        #endregion

        #region �жϵ�ǰ�����Ƿ����
        /// <summary>
        /// �жϵ�ǰ�����Ƿ����
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
