using ERCHTMS.Entity.OccupationalHealthManage;
using ERCHTMS.Busines.OccupationalHealthManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Data;
using ERCHTMS.Code;
using System.Web;
using System;
using BSFramework.Util.Offices;
using ERCHTMS.Busines.SystemManage;
using System.Collections.Generic;
using ERCHTMS.Entity.SystemManage;
using System.Text.RegularExpressions;
using System.Linq;
using System.Linq.Expressions;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;
using Newtonsoft.Json;

namespace ERCHTMS.Web.Areas.OccupationalHealthManage.Controllers
{
    /// <summary>
    /// �� ����Σ�������嵥
    /// </summary>
    public class HazardfactorsController : MvcControllerBase
    {
        private HazardfactorsBLL hazardfactorsbll = new HazardfactorsBLL();

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
        /// ����ҳ��
        /// </summary>
        /// <returns></returns>
        public ActionResult Import()
        {
            return View();
        }

        /// <summary>
        /// ѡ����ҳ��
        /// </summary>
        /// <returns></returns>
        public ActionResult Select()
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
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "HID";
            pagination.p_fields = "AREAID,AREAVALUE,RISKID,RISKVALUE,CONTACTNUMBER";//ע���˴�Ҫ�滻����Ҫ��ѯ����
            pagination.p_tablename = "BIS_HAZARDFACTORS";
            pagination.conditionJson = "1=1";
            pagination.sidx = "CREATEDATE";

            ERCHTMS.Code.Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                string where = new ERCHTMS.Busines.AuthorizeManage.AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
                pagination.conditionJson += " and " + where;
            }

            var data = hazardfactorsbll.GetPageListByProc(pagination, queryJson);
            var jsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return ToJsonResult(jsonData);

        }

        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = hazardfactorsbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetList()
        {

            string wheresql = "";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                wheresql = "";
            }
            else
            {
                string where = new ERCHTMS.Busines.AuthorizeManage.AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
                wheresql += " and " + where;
            }

            var data = hazardfactorsbll.GetList("", wheresql);
            return ToJsonResult(data);
        }


        /// <summary>
        /// ���ݲ�ѯ������ȡְҵ������
        /// </summary>
        /// <param name="Code">ְҵ���ֵ�Code</param>
        /// <param name="deptIds">ҳ���������ְҵ��ids</param>
        /// <param name="keyword">�ؼ���</param>
        /// <param name="checkMode">��ѡ���ѡ��0:��ѡ��1:��ѡ</param>
        /// <returns>��������Json</returns>
        [HttpGet]
        public ActionResult GetOccpuationalTreeJson(string Code, string keyword, int checkMode = 0, int mode = 0, string deptIds = "0")
        {

            DataItemBLL di = new DataItemBLL();
            //�Ȼ�ȡ���ֵ���
            DataItemEntity DataItems = di.GetEntityByCode(Code);

            DataItemDetailBLL did = new DataItemDetailBLL();
            //�����ֵ����ȡֵ
            List<DataItemDetailEntity> didList = did.GetList(DataItems.ItemId).OrderBy(t => t.ItemValue).ToList();



            //didList = didList.Where(it => it.ParentId == "0").ToList();//������Ҫ�����ڿ���ʱ���� ������һ��ɸѡ

            var treeList = new List<TreeEntity>();

            //��ȡ���и��ڵ㼯��
            List<DataItemDetailEntity> parentList = didList.Where(it => it.ItemValue.Length == 2).ToList();

            //��ȡ�����ӽڵ�ڵ㼯��
            List<DataItemDetailEntity> SunList = didList.Where(it => it.ItemValue.Length > 2).ToList();

            if (!string.IsNullOrEmpty(keyword))
            {
                SunList = SunList.Where(t => t.ItemName.Contains(keyword)).ToList();
            }

            //�Ȱ󶨸�����
            foreach (DataItemDetailEntity oe in parentList)
            {
                treeList.Add(new TreeEntity
                {
                    id = oe.ItemValue,
                    text = oe.ItemName,
                    value = oe.ItemValue,
                    parentId = "0",
                    isexpand = true,
                    complete = true,
                    showcheck = false,
                    hasChildren = true,
                    Attribute = "Sort",
                    AttributeValue = "Parent",
                    AttributeA = "manager",
                    AttributeValueA = "" + "," + "" + ",1"
                });
            }

            //�ٰ��Ӽ���
            foreach (DataItemDetailEntity item in SunList)
            {
                int chkState = 0;
                string[] arrids = deptIds.Split(',');
                if (arrids.Contains(item.ItemValue))
                {
                    chkState = 1;
                }
                TreeEntity tree = new TreeEntity();
                bool hasChildren = false;
                tree.id = item.ItemValue;
                tree.text = item.ItemName;
                tree.value = item.ItemValue;
                tree.isexpand = true;
                tree.complete = true;
                tree.checkstate = chkState;
                tree.showcheck = checkMode == 0 ? false : true;
                tree.hasChildren = hasChildren;
                tree.parentId = item.ItemValue.Substring(0, 2);
                tree.Attribute = "Sort";
                tree.AttributeValue = "Sun";
                tree.AttributeA = "manager";
                tree.AttributeValueA = "" + "," + "" + "," + "2";
                treeList.Add(tree);
            }
            return Content(treeList.TreeToJson());
        }

        /// <summary>
        /// ���Σ��������������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetRiskCmbList()
        {

            string wheresql = "";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                wheresql = "";
            }
            else
            {
                string where = new ERCHTMS.Busines.AuthorizeManage.AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
                wheresql += " and " + where;
            }

            //�Ȼ�ȡ��������
            DataTable hlist = hazardfactorsbll.GetList("", wheresql);
            List<ComboxEntity> Rlist = new List<ComboxEntity>();
            List<string> conStr = new List<string>();//�����ж��ظ��ļ���
            foreach (DataRow item in hlist.Rows)
            {
                string rv = item["RISKVALUE"].ToString();
                string rid = item["RISKID"].ToString();


                ComboxEntity risk = new ComboxEntity();
                risk.itemName = rv;
                risk.itemValue = rid;
                if (!conStr.Contains(rv))
                {
                    Rlist.Add(risk);//���û���ظ������
                    conStr.Add(rv);
                }
            }

            var data = Rlist;

            return ToJsonResult(data);
        }

        /// <summary>
        /// �Ƿ񳬱�����������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetIsExcessiveCmbList()
        {
            List<ComboxEntity> Rlist = new List<ComboxEntity>();
            ComboxEntity yes = new ComboxEntity();
            yes.itemName = "��";
            yes.itemValue = "1";

            ComboxEntity no = new ComboxEntity();
            no.itemName = "��";
            no.itemValue = "0";

            Rlist.Add(yes);
            Rlist.Add(no);

            return ToJsonResult(Rlist);

        }

        /// <summary>
        /// ������ѡ����id���Σ��������������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetArRiskCmbList(string areaid)
        {

            string wheresql = "";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                wheresql = "";
            }
            else
            {
                string where = new ERCHTMS.Busines.AuthorizeManage.AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
                wheresql += " and " + where;
            }

            //�Ȼ�ȡ��������
            DataTable hlist = hazardfactorsbll.GetList(areaid, wheresql);
            List<ComboxEntity> Rlist = new List<ComboxEntity>();
            List<string> conStr = new List<string>();//�����ж��ظ��ļ���
            foreach (DataRow item in hlist.Rows)
            {

                string rv = item["RISKVALUE"].ToString();
                string rid = item["RISKID"].ToString();


                ComboxEntity risk = new ComboxEntity();
                risk.itemName = rv;
                risk.itemValue = rid;
                if (!conStr.Contains(rv))
                {
                    Rlist.Add(risk);//���û���ظ������
                    conStr.Add(rv);
                }



            }
            var data = Rlist;

            return ToJsonResult(data);
        }

        /// <summary>
        /// ���������������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAreaCmbList()
        {
            string wheresql = "";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                wheresql = "";
            }
            else
            {
                string where = new ERCHTMS.Busines.AuthorizeManage.AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
                wheresql += " and " + where;
            }

            //�Ȼ�ȡ��������
            DataTable hlist = hazardfactorsbll.GetList("", wheresql);
            List<ComboxEntity> Rlist = new List<ComboxEntity>();
            List<string> conStr = new List<string>();//�����ж��ظ��ļ���
            foreach (DataRow item in hlist.Rows)
            {

                string av = item["AreaValue"].ToString();
                string ai = item["AreaId"].ToString();
                ComboxEntity risk = new ComboxEntity();
                risk.itemName = av;
                risk.itemValue = ai;
                if (!conStr.Contains(av))
                {
                    Rlist.Add(risk);//���û���ظ������
                    conStr.Add(av);
                }

            }
            var data = Rlist;
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
            hazardfactorsbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, HazardfactorsEntity entity, string UserName, string UserId)
        {
            try
            {
                string[] RiskValueList = entity.RiskValue.Split('|');
                string[] RiskIdList = entity.Riskid.Split('|');
                List<HazardfactorsEntity> list = new List<HazardfactorsEntity>();
                for (int i = 0; i < RiskValueList.Length; i++)
                {
                    HazardfactorsEntity temp = new HazardfactorsEntity();
                    temp = JsonConvert.DeserializeObject<HazardfactorsEntity>(JsonConvert.SerializeObject(entity));
                    temp.Riskid = RiskIdList[i];
                    temp.RiskValue = RiskValueList[i];
                    list.Add(temp);
                }
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                foreach (var item in list)
                {
                    if (hazardfactorsbll.ExistDeptJugement(item.AreaValue, user.OrganizeCode, item.RiskValue))
                    {
                        hazardfactorsbll.SaveForm(keyValue, item, UserName, UserId);
                    }
                }
                return Success("1");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
            

        }
        /// <summary>
        /// �����嵥
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportDept()
        {

            int error = 0;
            string message = "��ѡ���ʽ��ȷ���ļ��ٵ���!";
            string falseMessage = "";
            int count = HttpContext.Request.Files.Count;
            if (count > 0)
            {
                HttpPostedFileBase file = HttpContext.Request.Files[0];
                if (string.IsNullOrEmpty(file.FileName))
                {
                    return message;
                }
                if (!(file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xls") || file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xlsx")))
                {
                    return message;
                }
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName));
                DataTable dt = ExcelHelper.ExcelImport(Server.MapPath("~/Resource/temp/" + fileName));
                int order = 1;

                //�Ȼ�ȡ�ֵ�
                DataItemBLL di = new DataItemBLL();
                DistrictBLL districtbll = new DistrictBLL();
                IEnumerable<DataItemDetailEntity> ReskList = di.GetList("Risk");
                IEnumerable<DistrictEntity> AreaList = districtbll.GetOrgList(OperatorProvider.Provider.Current().OrganizeId);

                Expression<Func<UserEntity, bool>> condition;
                if (OperatorProvider.Provider.Current().IsSystem)
                {
                    condition = it => it.Account != "System" && it.IsPresence == "1";//����ǹ���Ա
                }
                else
                {
                    string orgid = OperatorProvider.Provider.Current().OrganizeId;
                    condition = it => it.Account != "System" && it.IsPresence == "1" && it.OrganizeId == orgid;//���ǹ���Ա��鵽���б������µ��û�
                }

                //�Ȼ�ȡ�����û��¿���ѡ����û�
                List<UserEntity> userlist = new UserBLL().GetListForCon(condition).ToList();

                for (int i = 1; i < dt.Rows.Count; i++)
                {

                    //����
                    string AreaName = dt.Rows[i][0].ToString();
                    string AreaValue = "";

                    //Σ��Դ
                    string RiskName = dt.Rows[i][1].ToString();//Σ��Դ����
                    Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                    //��֤ͬһ��˾��Χ���Ƿ����ظ�����
                    if (!hazardfactorsbll.ExistDeptJugement(AreaName, user.OrganizeCode, RiskName))
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "�����ݴ����ظ�,δ�ܵ���.";
                        error++;
                        continue;
                    }

                    //��֤�����Ƿ���ƥ����
                    if (!GetAreaIsTrue(AreaList, AreaName.Trim(), out AreaValue))
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "��ֵ����ֵ���ڿ�ѡ��Χ��,δ�ܵ���.";
                        error++;
                        continue;
                    }





                    string RiskValue = "";
                    //if (Risks.Length == 0)
                    //{
                    //    falseMessage += "</br>" + "��" + (i + 2) + "��ְֵҵ��Σ����������ֵΪ��,δ�ܵ���.";
                    //    error++;
                    //    continue;
                    //}
                    if (!GetIsTrue(ReskList, RiskName, out RiskValue))
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "��ְֵҵ��Σ����������ֵ���ڿ�ѡ��Χ��,δ�ܵ���.";
                        error++;
                        continue;
                    }

                    ////Σ��Դ
                    //string Num = dt.Rows[i][2].ToString();//�Ӵ�������

                    //string pattern = "^[0-9]+$";
                    //int Contactnumber = 0;


                    ////��֤�Ƿ�������
                    //if (Regex.IsMatch(Num, pattern))
                    //{
                    //    Contactnumber = Convert.ToInt32(Num);
                    //}
                    //else if (Num.Trim() == "")
                    //{

                    //}
                    //else
                    //{
                    //    falseMessage += "</br>" + "��" + (i + 2) + "��ֵ�Ӵ�����ֻ����д���ֻ��߲���,ֵ���ʹ���,δ�ܵ���.";
                    //    error++;
                    //    continue;
                    //}

                    string users = dt.Rows[i][2].ToString();
                    if (users.Trim() == "")
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "��ֵ�Ӵ���Ա����Ϊ��,δ�ܵ���.";
                        error++;
                        continue;
                    }

                    users = users.Replace('��', ',');//�����д�д���滻��Сд,

                    string[] uses = users.Split(',');
                    string userids = "";
                    string errorname = "";
                    for (int j = 0; j < uses.Length; j++)
                    {
                        UserEntity ue = userlist.Where(it => it.RealName == uses[j]).FirstOrDefault();
                        if (ue == null)//����û�������
                        {
                            if (errorname == "")
                            {
                                errorname = uses[j];
                            }
                            else
                            {
                                errorname += "," + uses[j];
                            }
                            continue;
                        }

                        if (j == 0)
                        {
                            userids = ue.UserId;
                        }
                        else
                        {
                            userids += "," + ue.UserId;
                        }
                    }

                    if (errorname.Trim() != "")
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "��ֵ�Ӵ���Ա:" + errorname + ",ֵ���ڿ�ѡ��Χ��,δ�ܵ���.";
                        error++;
                        continue;
                    }

                    HazardfactorsEntity hf = new HazardfactorsEntity();
                    hf.AreaId = AreaValue;
                    hf.AreaValue = AreaName;
                    hf.Riskid = RiskValue;
                    hf.RiskValue = RiskName;
                    if (uses.Length > 0)
                    {
                        hf.ContactNumber = uses.Length;
                    }


                    try
                    {
                        hazardfactorsbll.SaveForm("", hf, users, userids);
                    }
                    catch
                    {
                        error++;
                    }

                }
                count = dt.Rows.Count - 1;
                message = "����" + count + "����¼,�ɹ�����" + (count - error) + "����ʧ��" + error + "��";
                message += "</br>" + falseMessage;
            }

            return message;
        }

        /// <summary>
        /// �����ֵ�Σ��Դ //��ʱ�������ʹ�ã��������Ҫ���µ������ݿ���ʹ��
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportDataItem()
        {

            int error = 0;
            string message = "��ѡ���ʽ��ȷ���ļ��ٵ���!";
            string falseMessage = "";
            int count = HttpContext.Request.Files.Count;
            if (count > 0)
            {
                HttpPostedFileBase file = HttpContext.Request.Files[0];
                if (string.IsNullOrEmpty(file.FileName))
                {
                    return message;
                }
                if (!(file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xls") || file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xlsx")))
                {
                    return message;
                }
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName));
                DataTable dt = ExcelHelper.ExcelImport(Server.MapPath("~/Resource/temp/" + fileName));
                int order = 1;

                //�Ȼ�ȡ�ֵ�
                DataItemBLL di = new DataItemBLL();
                DataItemEntity Resk = di.GetEntityByCode("Risk");
                DataItemDetailBLL detabll = new DataItemDetailBLL();
                string[] names = { "�۳�", "��ѧ����", "��������", "����������", "��������", "��������" };
                string[] namevalues = { "01", "02", "03", "04", "05", "06" };

                string value = "";
                int index = 1;
                string fname = file.FileName.Substring(0, file.FileName.IndexOf('.'));
                for (int i = 0; i < names.Length; i++)
                {
                    if (fname == names[i])
                    {
                        value = namevalues[i];
                    }
                }

                if (value != "")
                {
                    DataItemDetailEntity dide = new DataItemDetailEntity();
                    dide.ItemId = Resk.ItemId;
                    dide.ItemValue = value;
                    dide.ItemName = fname;
                    dide.SortCode = index;
                    dide.ParentId = "0";
                    try
                    {
                        detabll.SaveForm("", dide);
                    }
                    catch (Exception)
                    {

                        error++;
                    }

                    foreach (DataRow dr in dt.Rows)
                    {
                        DataItemDetailEntity did = new DataItemDetailEntity();
                        did.ItemId = Resk.ItemId;
                        if (index < 10)
                        {
                            did.ItemValue = value + "00" + index;
                        }
                        else if (index < 100)
                        {
                            did.ItemValue = value + "0" + index;
                        }
                        else
                        {
                            did.ItemValue = value + index;
                        }
                        did.ItemName = dr[0].ToString();
                        did.SortCode = index;
                        did.ParentId = "0";
                        index++;
                        try
                        {
                            detabll.SaveForm("", did);
                        }
                        catch (Exception)
                        {

                            error++;
                        }
                    }
                }



                count = dt.Rows.Count - 1;
                message = "����" + count + "����¼,�ɹ�����" + (count - error) + "����ʧ��" + error + "��";
                message += "</br>" + falseMessage;


            }
            return message;
        }

        #endregion
        /// <summary>
        /// �ж�ֵ�Ƿ�����ڼ�����
        /// </summary>
        /// <param name="ReskList"></param>
        /// <param name="value"></param>
        /// <param name="RiskValue"></param>
        /// <returns></returns>
        public bool GetIsTrue(IEnumerable<DataItemDetailEntity> ReskList, string value, out string RiskValue)
        {
            RiskValue = "";
            bool listFlag = false;
            foreach (DataItemDetailEntity item in ReskList)
            {
                if (value.Trim() == item.ItemName)
                {

                    RiskValue = item.ItemValue;
                    listFlag = true;
                }
            }
            return listFlag;
        }

        /// <summary>
        /// �ж�ֵ�Ƿ�����ڼ�����
        /// </summary>
        /// <param name="ReskList"></param>
        /// <param name="value"></param>
        /// <param name="RiskValue"></param>
        /// <returns></returns>
        public bool GetAreaIsTrue(IEnumerable<DistrictEntity> ReskList, string value, out string RiskValue)
        {
            RiskValue = "";
            bool listFlag = false;
            foreach (DistrictEntity item in ReskList)
            {
                if (value.Trim() == item.DistrictName)
                {
                    RiskValue = item.DistrictID;
                    listFlag = true;
                }
            }
            return listFlag;
        }

        ///// <summary>
        ///// �ж��������Ƿ��в���
        ///// </summary>
        ///// <param name="Risks"></param>
        ///// <param name="ReskList"></param>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //public bool GetCountTrue(string Risks, IEnumerable<DataItemDetailEntity> ReskList, out string Riskvalue)
        //{
        //    string value = "";
        //    Riskvalue = "";
        //    foreach (string r in Risks)
        //    {
        //        if (GetIsTrue(ReskList, r, out value))
        //        {
        //            if (Riskvalue == "")
        //            {
        //                Riskvalue = value;
        //            }
        //            else
        //            {
        //                Riskvalue += "," + value;
        //            }
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        /// <summary>
        /// ������Excel
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult Excel(string queryJson)
        {
            string wheresql = "";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                wheresql = "";
            }
            else
            {
                string where = new ERCHTMS.Busines.AuthorizeManage.AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
                wheresql += " and ha." + where;
            }

            DataTable dt = hazardfactorsbll.GetTable(queryJson, wheresql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i][0] = i + 1;
            }
            string FileUrl = @"\Resource\ExcelTemplate\ְҵ��Σ�������嵥_����ģ��.xlsx";
            AsposeExcelHelper.ExecuteResult(dt, FileUrl, "ְҵ��Σ�������嵥", "ְҵ��Σ�������б�");

            return Success("�����ɹ���");
        }

        /// <summary>
        /// ����ȥ�ظ�ֵ
        /// </summary>
        /// <param name="Group"></param>
        /// <returns></returns>
        public string[] Deduplication(string[] Group)
        {
            List<string> str = new List<string>();
            foreach (string item in Group)
            {
                if (!str.Contains(item))
                {
                    str.Add(item);
                }
            }
            return str.ToArray();
        }
    }
}
