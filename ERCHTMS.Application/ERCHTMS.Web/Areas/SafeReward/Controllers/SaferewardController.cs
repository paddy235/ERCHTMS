using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ERCHTMS.Entity.SafeReward;
using ERCHTMS.Busines.SafeReward;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BSFramework.Util.Offices;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Code;
using ERCHTMS.Entity.OutsourcingProject;
using Newtonsoft.Json;
using System.Web;
using Aspose.Words;
using ERCHTMS.Busines.BaseManage;
using BSFramework.Util.Extension;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Web.Areas.SafeReward.Controllers
{
    /// <summary>
    /// �� ������ȫ����
    /// </summary>
    public class SaferewardController : MvcControllerBase
    {
        private SaferewardBLL saferewardbll = new SaferewardBLL();
        private SaferewarddetailBLL saferewarddetailbll = new SaferewarddetailBLL();
        private DepartmentBLL departmentbll = new DepartmentBLL();
        private FileInfoBLL fileinfobll = new FileInfoBLL();

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
        /// ���ͱ�׼
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RewardStandard()
        {
            return View();
        }

        /// <summary>
        /// ����ͳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RewardStatistics()
        {
            return View();
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Flow()
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
        /// ������ϸ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RewardDetail()
        {
            return View();
        }
        #endregion

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
           // Operator user = OperatorProvider.Provider.Current();
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "Id";
            pagination.p_fields = @"CreateUserId,CreateDate,CreateUserName,ModifyUserId,ModifyDate,ModifyUserName,CreateUserDeptCode,CreateUserOrgCode,FlowState,ApplyUserId,ApproverPeopleIds,
                                case when ApplyState= 0 then '������'
                                   when ApplyState= 1 then 'רҵ���' 
                                      when ApplyState= 2 then '�������' 
                                      when ApplyState= 3 then 'EHS�����' 
                                        when ApplyState= 4 then '�ֹ��쵼���' 
                                          when ApplyState= 5 then '�ܾ������'
                                            when ApplyState= 6 then '�����' end as  ApplyState,SafeRewardCode,RewardUserName,ApplyDeptName,ApplyTime,ApplyRewardRmb,RewardRemark,belongdept,applyusername";
            pagination.p_tablename = "Bis_SafeReward";
            pagination.sidx = "CreateDate";//�����ֶ�
            pagination.sord = "desc";//����ʽ
            pagination.conditionJson = "1=1";
            var data = saferewardbll.GetPageList(pagination, queryJson);
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return Content(JsonData.ToJson());
        }


        /// <summary>
        /// ��ȡ������׼
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetStandardJson()
        {
            var data = saferewardbll.GetStandardJson();
            return ToJsonResult(data);
        }

        
        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = saferewardbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        ///��ȡͳ������
        /// </summary>
        /// <param name="year">���</param>
        /// <returns></returns>
        [HttpGet]
        public string GetRewardStatisticsCount(string year = "")
        {
            return saferewardbll.GetRewardStatisticsCount(year);
        }


        /// <summary>
        ///��ȡ��������ͳ������
        /// </summary>
        /// <param name="year">���</param>
        /// <returns></returns>
        [HttpGet]
        public string GetRewardStatisticsTime(string year = "")
        {
            return saferewardbll.GetRewardStatisticsTime(year);
        }


        /// <summary>
        ///��ȡͳ������(�б�)
        /// </summary>
        /// <param name="year">���</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetRewardStatisticsList(string year = "")
        {
            try
            {
                var curUser = new OperatorProvider().Current(); //��ǰ�û�  
                var treeList = new List<TreeGridEntity>();
                var dt = saferewardbll.GetRewardStatisticsList(year);

                foreach (DataRow row in dt.Rows)
                {
                    DataTable newDt = new DataTable();
                    newDt.Columns.Add("changedutydepartcode");
                    newDt.Columns.Add("fullname");
                    newDt.Columns.Add("departmentid");
                    newDt.Columns.Add("parent");
                    newDt.Columns.Add("total");
                    newDt.Columns.Add("haschild", typeof(bool));
                    newDt.Columns.Add("january");
                    newDt.Columns.Add("february");
                    newDt.Columns.Add("march");
                    newDt.Columns.Add("april");
                    newDt.Columns.Add("may");
                    newDt.Columns.Add("june");
                    newDt.Columns.Add("july");
                    newDt.Columns.Add("august");
                    newDt.Columns.Add("september");
                    newDt.Columns.Add("october");
                    newDt.Columns.Add("november");
                    newDt.Columns.Add("december");

                    DataRow newDr = newDt.NewRow();
                    newDr["changedutydepartcode"] = row["encode"].ToString();
                    newDr["fullname"] = row["fullname"].ToString();
                    newDr["departmentid"] = row["departmentid"].ToString();
                    if (row["parentid"].ToString() != "0")
                    {
                        newDr["parent"] = row["parentid"].ToString();
                    }
                    newDr["total"] = row["total"].ToString();
                    newDr["january"] = row["january"].ToString();
                    newDr["february"] = row["february"].ToString();
                    newDr["march"] = row["march"].ToString();
                    newDr["april"] = row["april"].ToString();
                    newDr["may"] = row["may"].ToString();
                    newDr["june"] = row["june"].ToString();
                    newDr["july"] = row["july"].ToString();
                    newDr["august"] = row["august"].ToString();
                    newDr["september"] = row["september"].ToString();
                    newDr["october"] = row["october"].ToString();
                    newDr["november"] = row["november"].ToString();
                    newDr["december"] = row["december"].ToString();
                    TreeGridEntity tree = new TreeGridEntity();
                    bool hasChildren = dt.Select(string.Format(" parentid ='{0}'", row["departmentid"].ToString())).Count() == 0 ? false : true;
                    //string[] january = dt.Select(string.Format(" parentid ='{0}'", row["departmentid"].ToString())).Select(d => d.Field<string>("january")).ToArray();
                    //foreach (var item in january)
                    //{
                    //    newDr["january"] = (Convert.ToInt32(newDr["january"].ToString()) + Convert.ToInt32(item.ToString())).ToString();
                    //}
                    newDr["haschild"] = hasChildren;
                    newDt.Rows.Add(newDr);
                    //tentity.haschild = hasChildren;
                    tree.id = row["departmentid"].ToString();
                    tree.parentId = row["parentid"].ToString();
                    string itemJson = newDt.ToJson().Replace("[", "").Replace("]", "");
                    //string itemJson = tentity.ToJson();
                    tree.entityJson = itemJson;
                    tree.expanded = false;
                    tree.hasChildren = hasChildren;
                    treeList.Add(tree);
                }

                //�������
                return Content(treeList.TreeJson(curUser.ParentId));
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
        }

        /// <summary>
        ///��ȡ������������(�б�)
        /// </summary>
        /// <param name="year">���</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetRewardStatisticsTimeList(string year = "")
        {
            try
            {
                var curUser = new OperatorProvider().Current(); //��ǰ�û�  
                var treeList = new List<TreeGridEntity>();
                var dt = saferewardbll.GetRewardStatisticsTimeList(year);

                foreach (DataRow row in dt.Rows)
                {
                    DataTable newDt = new DataTable();
                    newDt.Columns.Add("changedutydepartcode");
                    newDt.Columns.Add("fullname");
                    newDt.Columns.Add("departmentid");
                    newDt.Columns.Add("parent");
                    newDt.Columns.Add("total");
                    newDt.Columns.Add("haschild", typeof(bool));
                    newDt.Columns.Add("january");
                    newDt.Columns.Add("february");
                    newDt.Columns.Add("march");
                    newDt.Columns.Add("april");
                    newDt.Columns.Add("may");
                    newDt.Columns.Add("june");
                    newDt.Columns.Add("july");
                    newDt.Columns.Add("august");
                    newDt.Columns.Add("september");
                    newDt.Columns.Add("october");
                    newDt.Columns.Add("november");
                    newDt.Columns.Add("december");

                    DataRow newDr = newDt.NewRow();
                    newDr["changedutydepartcode"] = row["encode"].ToString();
                    newDr["fullname"] = row["fullname"].ToString();
                    newDr["departmentid"] = row["departmentid"].ToString();
                    if (row["parentid"].ToString() != "0")
                    {
                        newDr["parent"] = row["parentid"].ToString();
                    }
                    newDr["total"] = row["total"].ToString();
                    newDr["january"] = row["january"].ToString();
                    newDr["february"] = row["february"].ToString();
                    newDr["march"] = row["march"].ToString();
                    newDr["april"] = row["april"].ToString();
                    newDr["may"] = row["may"].ToString();
                    newDr["june"] = row["june"].ToString();
                    newDr["july"] = row["july"].ToString();
                    newDr["august"] = row["august"].ToString();
                    newDr["september"] = row["september"].ToString(); 
                    newDr["october"] = row["october"].ToString();
                    newDr["november"] = row["november"].ToString();
                    newDr["december"] = row["december"].ToString();
                    TreeGridEntity tree = new TreeGridEntity();
                    bool hasChildren = dt.Select(string.Format(" parentid ='{0}'", row["departmentid"].ToString())).Count() == 0 ? false : true;
                    //string[] january = dt.Select(string.Format(" parentid ='{0}'", row["departmentid"].ToString())).Select(d => d.Field<string>("january")).ToArray();
                    //foreach (var item in january)
                    //{
                    //    newDr["january"] = (Convert.ToInt32(newDr["january"].ToString()) + Convert.ToInt32(item.ToString())).ToString();
                    //}
                    newDr["haschild"] = hasChildren;
                    newDt.Rows.Add(newDr);
                    //tentity.haschild = hasChildren;
                    tree.id = row["departmentid"].ToString();
                    tree.parentId = row["parentid"].ToString();
                    string itemJson = newDt.ToJson().Replace("[", "").Replace("]", "");
                    //string itemJson = tentity.ToJson();
                    tree.entityJson = itemJson;
                    tree.expanded = false;
                    tree.hasChildren = hasChildren;
                    treeList.Add(tree);
                }

                //�������
                return Content(treeList.TreeJson(curUser.ParentId));
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
        }



        /// <summary>
        /// ������ȫ����excel
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "������ȫ����excel")]
        public ActionResult ExportSafeRewardExcel(string condition, string queryJson)
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "Id";
            pagination.p_fields = @"case when ApplyState= 0 then '������'
            when ApplyState= 1 then 'רҵ���' 
            when ApplyState= 2 then '�������' 
            when ApplyState= 3 then 'EHS�����' 
            when ApplyState= 4 then '�ֹ��쵼����' 
            when ApplyState= 5 then '�ܾ�������'
            when ApplyState= 6 then '�����' end as  applyState,saferewardcode,applyusername,belongdept,to_char(applytime,'yyyy-MM-dd HH24:mi:ss') applytime,applyrewardrmb,rewardremark ";
            pagination.p_tablename = "bis_safereward";
            pagination.conditionJson = "1=1";
            pagination.sidx = "CreateDate";
            pagination.sord = "desc";
            var data = saferewardbll.GetPageList(pagination, queryJson);


            //���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "��ȫ����";
            excelconfig.TitleFont = "΢���ź�";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "��ȫ����" + ".xls";
            excelconfig.IsAllSizeColumn = true;
            //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            excelconfig.ColumnEntity = new List<ColumnEntity>()
            {
                new ColumnEntity() {Column = "applyState", ExcelColumn = "����״̬", Alignment = "center"},
                new ColumnEntity() {Column = "saferewardcode", ExcelColumn = "�������", Alignment = "center"},
                new ColumnEntity() {Column = "applyusername", ExcelColumn = "������", Alignment = "center"},
                new ColumnEntity() {Column = "belongdept", ExcelColumn = "����רҵ(����)", Alignment = "center"},
                new ColumnEntity() {Column = "applytime", ExcelColumn = "����ʱ��", Alignment = "center"},             
                new ColumnEntity() {Column = "rewardremark", ExcelColumn = "�¼�����", Alignment = "center"}
            };

            //���õ�������
            ExcelHelper.ExportByAspose(data, excelconfig.FileName, excelconfig.ColumnEntity);
            return Success("�����ɹ���");
        }


        /// <summary>
        /// ������ȫ�������ͳ��excel
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "������ȫ�������ͳ��excel")]
        public ActionResult ExportStatisticsExcel(string year)
        {
            string jsonList = saferewardbll.GetRewardStatisticsExcel(year);

            dynamic dyObj = JsonConvert.DeserializeObject(jsonList);
            ;
            DataTable tb = JsonToDataTable(dyObj.rows.ToString());

                //���õ�����ʽ
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "��ȫ����";
                excelconfig.TitleFont = "΢���ź�";
                excelconfig.TitlePoint = 25;
                excelconfig.FileName = "��ȫ�������ͳ��";
                excelconfig.IsAllSizeColumn = true;
                //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
                excelconfig.ColumnEntity = new List<ColumnEntity>()
                {
                    new ColumnEntity() {Column = "DeptName", ExcelColumn = "������λ", Alignment = "center"},
                    new ColumnEntity() {Column = "num1", ExcelColumn = "1��", Alignment = "center"},
                    new ColumnEntity() {Column = "num2", ExcelColumn = "2��", Alignment = "center"},
                    new ColumnEntity() {Column = "num3", ExcelColumn = "3��", Alignment = "center"},
                    new ColumnEntity() {Column = "num4", ExcelColumn = "4��", Alignment = "center"},
                    new ColumnEntity() {Column = "num5", ExcelColumn = "5��", Alignment = "center"},
                    new ColumnEntity() {Column = "num6", ExcelColumn = "6��", Alignment = "center"},
                    new ColumnEntity() {Column = "num7", ExcelColumn = "7��", Alignment = "center"},
                    new ColumnEntity() {Column = "num8", ExcelColumn = "8��", Alignment = "center"},
                    new ColumnEntity() {Column = "num9", ExcelColumn = "9��", Alignment = "center"},
                    new ColumnEntity() {Column = "num10", ExcelColumn = "10��", Alignment = "center"},
                    new ColumnEntity() {Column = "num11", ExcelColumn = "11��", Alignment = "center"},
                    new ColumnEntity() {Column = "num12", ExcelColumn = "12��", Alignment = "center"},
                    new ColumnEntity() {Column = "Total", ExcelColumn = "�ܼ�(Ԫ)", Alignment = "center"},
                };

                //���õ�������
                ExcelHelper.ExportByAspose(tb, excelconfig.FileName, excelconfig.ColumnEntity);
                return Success("�����ɹ���");
        }


        /// <summary>
        /// ������ȫ����ͳ��excel
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "������ȫ��������ͳ��excel")]
        public ActionResult ExportStatisticsTimeExcel(string year)
        {
            string jsonList = saferewardbll.GetRewardStatisticsTimeExcel(year);

            dynamic dyObj = JsonConvert.DeserializeObject(jsonList);
            ;
            DataTable tb = JsonToDataTable(dyObj.rows.ToString());

            //���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "��ȫ����";
            excelconfig.TitleFont = "΢���ź�";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "��ȫ��������ͳ��";
            excelconfig.IsAllSizeColumn = true;
            //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            excelconfig.ColumnEntity = new List<ColumnEntity>()
                {
                    new ColumnEntity() {Column = "DeptName", ExcelColumn = "������λ", Alignment = "center"},
                    new ColumnEntity() {Column = "num1", ExcelColumn = "1��", Alignment = "center"},
                    new ColumnEntity() {Column = "num2", ExcelColumn = "2��", Alignment = "center"},
                    new ColumnEntity() {Column = "num3", ExcelColumn = "3��", Alignment = "center"},
                    new ColumnEntity() {Column = "num4", ExcelColumn = "4��", Alignment = "center"},
                    new ColumnEntity() {Column = "num5", ExcelColumn = "5��", Alignment = "center"},
                    new ColumnEntity() {Column = "num6", ExcelColumn = "6��", Alignment = "center"},
                    new ColumnEntity() {Column = "num7", ExcelColumn = "7��", Alignment = "center"},
                    new ColumnEntity() {Column = "num8", ExcelColumn = "8��", Alignment = "center"},
                    new ColumnEntity() {Column = "num9", ExcelColumn = "9��", Alignment = "center"},
                    new ColumnEntity() {Column = "num10", ExcelColumn = "10��", Alignment = "center"},
                    new ColumnEntity() {Column = "num11", ExcelColumn = "11��", Alignment = "center"},
                    new ColumnEntity() {Column = "num12", ExcelColumn = "12��", Alignment = "center"},
                    new ColumnEntity() {Column = "Total", ExcelColumn = "�ܼ�(��)", Alignment = "center"},
                };

            //���õ�������
            ExcelHelper.ExportByAspose(tb, excelconfig.FileName, excelconfig.ColumnEntity);
            return Success("�����ɹ���");
        }


        #region ��ȡ��ȫ��������ͼ����
        [HttpPost]
        [AjaxOnly]
        public ActionResult GetWorkActionList(string keyValue)
        {
            var josnData = saferewardbll.GetFlow(keyValue);
            return Content(josnData.ToJson());
        }

        /// <summary>
        /// ������ȫ������ϸ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult ExportSafeRewardInfo(string keyValue)
        {

            HttpResponse resp = System.Web.HttpContext.Current.Response;
            //�������

            string fileName = "��ȫ����������_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
            string strDocPath = Request.PhysicalApplicationPath + @"Resource\ExcelTemplate\��ȫ��������ģ��.docx";
            Aspose.Words.Document doc = new Aspose.Words.Document(strDocPath);
            DocumentBuilder builder = new DocumentBuilder(doc);
            DataTable dt = new DataTable();
            dt.Columns.Add("RewardCode"); //���
            dt.Columns.Add("ApplyUserName"); //������
            dt.Columns.Add("ApplyTime"); //����ʱ��
            dt.Columns.Add("BelongDept"); //����������
            dt.Columns.Add("ReardNum"); //���뽱�����
            dt.Columns.Add("RewardName"); //������������
            dt.Columns.Add("RewardRemark"); //�¼�����
            dt.Columns.Add("approve1");//��һ��������
            dt.Columns.Add("approvename1");//��һ�������
            dt.Columns.Add("approvetime1");//��һ�����ʱ��
            dt.Columns.Add("approve2");//�ڶ���������
            dt.Columns.Add("approvename2");//�ڶ��������
            dt.Columns.Add("approvetime2");//�ڶ������ʱ��
            dt.Columns.Add("approve3");//������������
            dt.Columns.Add("approvename3");//�����������
            dt.Columns.Add("approvetime3");//���������ʱ��
            dt.Columns.Add("approve4");//���Ĳ�������
            dt.Columns.Add("approvename4");//���Ĳ������
            dt.Columns.Add("approvetime4");//���Ĳ����ʱ��
            dt.Columns.Add("approve5");//���岽������
            dt.Columns.Add("approvename5");//���岽�����
            dt.Columns.Add("approvetime5");//���岽���ʱ��
            DataRow row = dt.NewRow();


            //��ȫ������Ϣ
            SaferewardEntity saferewardentity = saferewardbll.GetEntity(keyValue);
            row["RewardCode"] = saferewardentity.SafeRewardCode;
            row["ApplyUserName"] = saferewardentity.ApplyUserName;
            row["ApplyTime"] = saferewardentity.ApplyTime.IsEmpty() ? "" : Convert.ToDateTime(saferewardentity.ApplyTime).ToString("yyyy-MM-dd");
            row["RewardRemark"] = saferewardentity.RewardRemark;

            row["approve1"] = saferewardentity.SpecialtyOpinion;
            row["approvetime1"] = saferewardentity.CreateDate.IsEmpty() ? "" : Convert.ToDateTime(saferewardentity.CreateDate).ToString("yyyy-MM-dd");
            UserEntity createuser = new UserBLL().GetEntity(saferewardentity.CreateUserId);
            if (createuser.SignImg.IsEmpty())
            {
                row["approvename1"] = Server.MapPath("~/content/Images/no_1.png");
            }
            else
            {
                var filepath = Server.MapPath("~/") + createuser.SignImg.ToString().Replace("../../", "").ToString();
                if (System.IO.File.Exists(filepath))
                {
                    row["approvename1"] = filepath;
                }
                else
                {
                    row["approvename1"] = Server.MapPath("~/content/Images/no_1.png");
                }
            }
            builder.MoveToMergeField("approvename1");
            builder.InsertImage(row["approvename1"].ToString(), 80, 35);
            var flist = fileinfobll.GetImageListByRecid(keyValue);
            builder.MoveToMergeField("RewardImage");
            foreach (FileInfoEntity fmode in flist)
            {
                string path = "";
                if (string.IsNullOrWhiteSpace(fmode.FilePath))
                {
                    path = Server.MapPath("~/content/Images/no_1.png");
                }
                else
                {
                    var filepath = Server.MapPath("~/") + fmode.FilePath.Replace("~/", "").ToString();
                    if (System.IO.File.Exists(filepath))
                    {
                        path = filepath;
                    }
                    else
                    {
                        path = Server.MapPath("~/content/Images/no_1.png");
                    }
                }
                builder.MoveToMergeField("RewardImage");
                builder.InsertImage(path, 200, 160);
            }

            //��ȡ�����˶���
            SaferewarddetailEntity SaferewarddetailEntity = saferewarddetailbll.GetListByRewardId(keyValue).OrderBy(t => t.CreateDate).FirstOrDefault();
            row["BelongDept"] = departmentbll.GetEntity(SaferewarddetailEntity.BelongDept) == null ? "" : departmentbll.GetEntity(SaferewarddetailEntity.BelongDept).FullName;
            row["ReardNum"] = SaferewarddetailEntity.RewardNum;
            row["RewardName"] = SaferewarddetailEntity.RewardName;
            DataTable dtAptitude = saferewardbll.GetAptitudeInfo(keyValue);
            int count = dtAptitude.Rows.Count;
            for (int i = dtAptitude.Rows.Count - 1; i > 0; i--)
            {
                if (i==(dtAptitude.Rows.Count-2))
                {
                    row["approve5"] = dtAptitude.Rows[i]["auditremark"];
                    row["approvetime5"] = dtAptitude.Rows[i]["auditdate"].IsEmpty() ? "" : Convert.ToDateTime(dtAptitude.Rows[i]["auditdate"]).ToString("yyyy-MM-dd");
                    if (dtAptitude.Rows[i]["auditsignimg"].IsEmpty())
                    {
                        row["approvename5"] = Server.MapPath("~/content/Images/no_1.png");
                    }
                    else
                    {
                        var filepath = Server.MapPath("~/") + dtAptitude.Rows[i]["auditsignimg"].ToString().Replace("../../", "").ToString();
                        if (System.IO.File.Exists(filepath))
                        {
                            row["approvename5"] = filepath;
                        }
                        else
                        {
                            row["approvename5"] = Server.MapPath("~/content/Images/no_1.png");
                        }
                    }
                    builder.MoveToMergeField("approvename5");
                    builder.InsertImage(row["approvename5"].ToString(), 80, 35);
                }
                else if (i==(dtAptitude.Rows.Count-3))
                {
                    row["approve4"] = dtAptitude.Rows[i]["auditremark"];
                    row["approvetime4"] = dtAptitude.Rows[i]["auditdate"].IsEmpty() ? "" : Convert.ToDateTime(dtAptitude.Rows[i]["auditdate"]).ToString("yyyy-MM-dd");
                    if (dtAptitude.Rows[i]["auditsignimg"].IsEmpty())
                    {
                        row["approvename4"] = Server.MapPath("~/content/Images/no_1.png");
                    }
                    else
                    {
                        var filepath = Server.MapPath("~/") + dtAptitude.Rows[i]["auditsignimg"].ToString().Replace("../../", "").ToString();
                        if (System.IO.File.Exists(filepath))
                        {
                            row["approvename4"] = filepath;
                        }
                        else
                        {
                            row["approvename4"] = Server.MapPath("~/content/Images/no_1.png");
                        }
                    }
                    builder.MoveToMergeField("approvename4");
                    builder.InsertImage(row["approvename4"].ToString(), 80, 35);
                }
                else if (i==(dtAptitude.Rows.Count-4))
                {
                    row["approve3"] = dtAptitude.Rows[i]["auditremark"];
                    row["approvetime3"] = dtAptitude.Rows[i]["auditdate"].IsEmpty() ? "" : Convert.ToDateTime(dtAptitude.Rows[i]["auditdate"]).ToString("yyyy-MM-dd");
                    if (dtAptitude.Rows[i]["auditsignimg"].IsEmpty())
                    {
                        row["approvename3"] = Server.MapPath("~/content/Images/no_1.png");
                    }
                    else
                    {
                        var filepath = Server.MapPath("~/") + dtAptitude.Rows[i]["auditsignimg"].ToString().Replace("../../", "").ToString();
                        if (System.IO.File.Exists(filepath))
                        {
                            row["approvename3"] = filepath;
                        }
                        else
                        {
                            row["approvename3"] = Server.MapPath("~/content/Images/no_1.png");
                        }
                    }
                    builder.MoveToMergeField("approvename3");
                    builder.InsertImage(row["approvename3"].ToString(), 80, 35);
                }
                else if (i==(dtAptitude.Rows.Count-5))
                {
                    row["approve2"] = dtAptitude.Rows[i]["auditremark"];
                    row["approvetime2"] = dtAptitude.Rows[i]["auditdate"].IsEmpty() ? "" : Convert.ToDateTime(dtAptitude.Rows[i]["auditdate"]).ToString("yyyy-MM-dd");
                    if (dtAptitude.Rows[i]["auditsignimg"].IsEmpty())
                    {
                        row["approvename2"] = Server.MapPath("~/content/Images/no_1.png");
                    }
                    else
                    {
                        var filepath = Server.MapPath("~/") + dtAptitude.Rows[i]["auditsignimg"].ToString().Replace("../../", "").ToString();
                        if (System.IO.File.Exists(filepath))
                        {
                            row["approvename2"] = filepath;
                        }
                        else
                        {
                            row["approvename2"] = Server.MapPath("~/content/Images/no_1.png");
                        }
                    }
                    builder.MoveToMergeField("approvename2");
                    builder.InsertImage(row["approvename2"].ToString(), 80, 35);
                }
            }
            dt.Rows.Add(row);
            doc.MailMerge.Execute(dt);
            doc.MailMerge.DeleteFields();

            doc.Save(resp, Server.UrlEncode(fileName), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
            return Success("�����ɹ�!");
        }

        #endregion


        #region ��ȡ�ֹ��쵼

        /// <summary>
        /// ��ȡ�ֹ��쵼
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetLeaderList()
        {
            var data = saferewardbll.GetLeaderList();
            return Content(data.ToJson());
        }

        #endregion

        /// <summary>
        /// ����ҵ��id��ȡ��Ӧ����˼�¼�б� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetSpecialAuditList(string keyValue)
        {
            var data = new AptitudeinvestigateauditBLL().GetAuditList(keyValue).Where(p=>p.REMARK!="0").ToList();
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡ���
        /// </summary>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetRewardCode()
        {
            var data = saferewardbll.GetRewardCode();
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ����id(���Ų㼶)
        /// </summary>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetDeptPId(string applyDeptId)
        {
            if (!string.IsNullOrEmpty(applyDeptId))
            {
                var data = saferewardbll.GetDeptPId(applyDeptId);
                return ToJsonResult(data);
            }

            return ToJsonResult("");
        }
        

        /// <summary>
        /// ���ش�����������
        /// </summary>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetRewardNum()
        {
            var data = saferewardbll.GetRewardNum();
            return ToJsonResult(data);
        }
        #endregion

        #region �ύ����
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
            saferewardbll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, SaferewardEntity entity, [System.Web.Http.FromBody]string dataJson)
        {

            var year = DateTime.Now.ToString("yyyy");
            var month = DateTime.Now.ToString("MM");
            var day = DateTime.Now.ToString("dd");
            var rewardCode = "Q/CRPHZHB 2208.06.01-JL01-" + year + month + day + saferewardbll.GetRewardCode();
            entity.SafeRewardCode = !string.IsNullOrEmpty(entity.SafeRewardCode) ? entity.SafeRewardCode : rewardCode;
            int? rewardmoney = 0;
            if (dataJson.Length > 0)
            {
                if (saferewarddetailbll.Remove(keyValue) > 0)
                {
                    List<SaferewarddetailEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SaferewarddetailEntity>>(dataJson);
                    foreach (SaferewarddetailEntity data in list)
                    {
                        data.RewardId = keyValue;
                        saferewarddetailbll.SaveForm("", data);
                        rewardmoney += data.RewardNum;
                    }
                }
            }
            entity.RewardMoney = rewardmoney;
            saferewardbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }

        /// <summary>
        /// �ύ�����������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">��������</param>
        /// <param name="rewEntity">������Ϣ</param>
        /// <param name="leaderShipId">�ֹ��쵼</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult CommitApply(string keyValue, AptitudeinvestigateauditEntity entity, SaferewardEntity rewEntity, string leaderShipId, [System.Web.Http.FromBody]string dataJson)
        {
            if (rewEntity != null && (string.IsNullOrEmpty(rewEntity.ApplyState) || rewEntity.ApplyState == "0"))
            {
                var year = DateTime.Now.ToString("yyyy");
                var month = DateTime.Now.ToString("MM");
                var day = DateTime.Now.ToString("dd");
                var rewardCode = "Q/CRPHZHB 2208.06.01-JL01-" + year + month + day + saferewardbll.GetRewardCode();
                rewEntity.SafeRewardCode = !string.IsNullOrEmpty(rewEntity.SafeRewardCode) ? rewEntity.SafeRewardCode : rewardCode;
                int? rewardmoney = 0;
                if (dataJson.Length > 0)
                {
                    if (saferewarddetailbll.Remove(keyValue)>0)
                    {
                        List<SaferewarddetailEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SaferewarddetailEntity>>(dataJson);
                        foreach (SaferewarddetailEntity data in list)
                        {
                            data.RewardId = keyValue;
                            saferewarddetailbll.SaveForm("", data);
                            rewardmoney += data.RewardNum;
                        }
                    }
                }
                rewEntity.RewardMoney = rewardmoney;
                saferewardbll.SaveForm(keyValue, rewEntity);
            }

            if (!string.IsNullOrEmpty(keyValue) && entity != null)
            {
                saferewardbll.CommitApply(keyValue, entity, leaderShipId);
            }

            return Success("�����ɹ���");
        }
        #endregion


        #region Json �ַ��� ת��Ϊ DataTable���ݼ���
        /// <summary>
        /// Json �ַ��� ת��Ϊ DataTable���ݼ��� ��ʽ[{"xxx":"yyy","x1":"yy2"},{"x2":"y2","x3":"y4"}]
        /// </summary>  
        /// <param name="json"></param>
        /// <returns></returns>
        public DataTable JsonToDataTable(string json)
        {
            DataTable dataTable = new DataTable();  //ʵ����
            DataTable result;
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //ȡ�������ֵ
                ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);
                if (arrayList.Count > 0)
                {
                    foreach (Dictionary<string, object> dictionary in arrayList)
                    {
                        if (dictionary.Keys.Count<string>() == 0)
                        {
                            result = dataTable;
                            return result;
                        }
                        if (dataTable.Columns.Count == 0)
                        {
                            foreach (string current in dictionary.Keys)
                            {
                                dataTable.Columns.Add(current, dictionary[current].GetType());
                            }
                        }
                        DataRow dataRow = dataTable.NewRow();
                        foreach (string current in dictionary.Keys)
                        {
                            dataRow[current] = dictionary[current];
                        }

                        dataTable.Rows.Add(dataRow); //ѭ������е�DataTable��
                    }
                }
            }
            catch
            {
            }
            result = dataTable;
            return result;
        }
        #endregion
    }
}
