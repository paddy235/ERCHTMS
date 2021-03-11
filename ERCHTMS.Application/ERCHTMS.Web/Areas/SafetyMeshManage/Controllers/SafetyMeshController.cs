using ERCHTMS.Entity.SafetyMeshManage;
using ERCHTMS.Busines.SafetyMeshManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System;
using System.Linq;
using System.Data;
using ERCHTMS.Code;
using System.Collections.Generic;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Busines.BaseManage;
using BSFramework.Util.Extension;
using BSFramework.Util.Offices;
using System.Text;
using System.Web;
using Aspose.Cells;
using System.Drawing;

namespace ERCHTMS.Web.Areas.SafetyMeshManage.Controllers
{
    /// <summary>
    /// �� ������ȫ�����쵼����
    /// </summary>
    public class SafetyMeshController : MvcControllerBase
    {
        private SafetyMeshBLL SafetyMeshbll = new SafetyMeshBLL();

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
        [HttpGet]
        public ActionResult Import()
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
        public ActionResult GetListJson()
        {
            var data = SafetyMeshbll.GetList();
            return ToJsonResult(data);
        }
        [HttpGet]
        public ActionResult GetTableJson(string queryJson)
        {
            var data = SafetyMeshbll.GetTable(queryJson);
            //DepartmentBLL deptBll = new DepartmentBLL();
            //foreach(DataRow dr in data.Rows)
            //{
            //    int count = deptBll.GetDataTable(string.Format("select count(1) from BIS_NETORGCHANGERECORD t where userid='{0}' and changeid in(select id from bis_netorgchange where isover=0)",dr["userid"].ToString())).Rows[0][0].ToInt();
            //    dr["state"] = count>0?"1":"0";
            //    count = deptBll.GetDataTable(string.Format("select count(1) from BIS_NETORGCHANGERECORD t where userid='{0}' and changeid in(select id from bis_netorgchange where isover=0)", dr["userid1"].ToString())).Rows[0][0].ToInt();
            //    dr["state1"] = count > 0 ? "1" : "0";
            //}
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
            var data = SafetyMeshbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="MeshRank">����</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetWorkJob(string MeshRank)
        {
            var user = OperatorProvider.Provider.Current();//������˾
            var disItem = SafetyMeshbll.GetListForCon(x => x.MeshRank == MeshRank && x.CreateUserOrgCode == user.OrganizeCode).OrderByDescending(x => x.CreateDate).FirstOrDefault();
            return ToJsonResult(disItem);
        }
        /// <summary>
        /// ���Ʋ����ظ�
        /// </summary>
        /// <param name="MeshName">��������</param>
        /// <param name="SuperiorId">�ϼ����</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExistName(string MeshName, string SuperiorId)
        {
            var user = OperatorProvider.Provider.Current();//������˾
            var list = SafetyMeshbll.GetListForCon(x => x.MeshName == MeshName && x.SuperiorId == SuperiorId && x.CreateUserOrgCode == user.OrganizeCode).ToList();
            bool IsOk = list.Count() == 0 ? true : false;
            return Content(IsOk.ToString());
        }
        #region ��ȡ�ϼ��������ṹ
        /// <summary>
        /// ��ȡ�ϼ��������ṹ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSuperiorDataJson(string orgID = "0", string keyword = "")
        {
            var user = OperatorProvider.Provider.Current();//������˾    

            var list = SafetyMeshbll.GetListForCon(x => x.CreateUserOrgCode == user.OrganizeCode).ToList();
            List<SafetyMeshEntity> meshdata = new List<SafetyMeshEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                list = list.Where(t => t.MeshName.Contains(keyword.Trim())).ToList();
            }
            meshdata = list.OrderBy(a => a.SortCode).ToList();
            if (orgID != "0")
            {
                meshdata = meshdata.Where(a => a.CreateUserOrgCode == orgID).ToList();
            }
            //List<SafetyMeshEntity> list1 = new List<SafetyMeshEntity>();
            //foreach (SafetyMeshEntity entity in meshdata)
            //{
            //    string code = string.Empty;
            //    if (entity.DistrictCode.Length > 5)
            //    {
            //        code = entity.DistrictCode.Substring(0, 6);
            //    }
            //    else
            //    {
            //        code = entity.DistrictCode.Substring(0, 3);
            //    }
            //    if (districtdata.Where(t => t.DistrictCode == code).Count() == 0)
            //    {
            //        DistrictEntity de = list.Where(t => t.DistrictCode == code).FirstOrDefault();
            //        if (de != null)
            //        {
            //            if (!list1.Contains(de))
            //            {
            //                list1.Add(de);
            //            }
            //        }
            //    }
            //}
            //meshdata = meshdata.Concat(list).ToList();
            meshdata = meshdata.OrderBy(t => t.SortCode).ToList();
            List<TreeEntity> treeList = new List<TreeEntity>();

            //����
            //var data = SafetyMeshbll.GetList(null);


            //var treeList = new List<TreeEntity>();
            foreach (SafetyMeshEntity item in meshdata)
            {
                TreeEntity tree = new TreeEntity();
                bool hasChildren = meshdata.Where(p => p.SuperiorId == item.Id).ToList().Count() == 0 ? false : true;
                tree.id = item.Id;
                tree.text = item.MeshName;
                tree.value = item.Id;
                tree.complete = true;
                tree.hasChildren = string.IsNullOrEmpty(keyword) ? hasChildren : false;
                tree.parentId = string.IsNullOrEmpty(keyword) ? (item.SuperiorId == null ? "0" : item.SuperiorId) : "0";
                tree.Attribute = "Code";
                tree.AttributeValue = item.MeshRank;
                tree.isexpand = false;
                tree.showcheck = true;
                treeList.Add(tree);
            }
            if (treeList.Count > 0)
            {
                treeList[0].isexpand = true;
            }
            return Content(treeList.TreeToJson());

        }
        #endregion
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
            SafetyMeshbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, SafetyMeshEntity entity)
        {
            try
            {
                SafetyMeshbll.SaveForm(keyValue, entity);
                return Success("�����ɹ���");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        [HandlerMonitor(0, "������ȫ����")]
        public ActionResult Export(string queryJson)
        {
            try
            {
                var exportTable = SafetyMeshbll.GetTableList(queryJson);

                ////���õ�������
                //ExcelHelper.ExcelDownload(data, excelconfig);
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                string fName = "��ȫ����_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                wb.Open(Server.MapPath("~/Resource/ExcelTemplate/tmp.xls"));
                var num = wb.Worksheets[0].Cells.Columns.Count;

                Aspose.Cells.Worksheet sheet = wb.Worksheets[0] as Aspose.Cells.Worksheet;
                Aspose.Cells.Cell cell = sheet.Cells[0, 0];
                cell.PutValue("��ȫ����"); //����
                cell.Style.Pattern = BackgroundType.Solid;
                cell.Style.Font.Size = 16;
                cell.Style.Font.Color = Color.Black;
                List<string> colList = new List<string>() { "��������", "�ϼ���������", "����������", "��ϵ�绰", "��������", "���缶��", "����ְ��" };
                List<string> colList1 = new List<string>() { "meshname", "superiorname", "dutyuser", "dutytel", "district", "meshrank", "workjob" };
                for (int i = 0; i < colList.Count; i++)
                {
                    //�����
                    Aspose.Cells.Cell serialcell = sheet.Cells[1, 0];
                    serialcell.PutValue(" ");

                    for (int j = 0; j < colList.Count; j++)
                    {
                        Aspose.Cells.Cell curcell = sheet.Cells[1, j];
                        //sheet.Cells.SetColumnWidth(j, 40);
                        curcell.Style.Pattern = BackgroundType.Solid;
                        curcell.Style.Font.Size = 12;
                        curcell.Style.Font.Color = Color.Black;
                        curcell.PutValue(colList[j].ToString()); //��ͷ
                    }
                    Aspose.Cells.Cells cells = sheet.Cells;
                    cells.Merge(0, 0, 1, colList.Count);
                }
                for (int i = 0; i < exportTable.Rows.Count; i++)
                {
                    //�������
                    for (int j = 0; j < colList1.Count; j++)
                    {
                        Aspose.Cells.Cell curcell = sheet.Cells[i + 2, j];
                        curcell.PutValue(exportTable.Rows[i][colList1[j]].ToString());
                    }

                }
                HttpResponse resp = System.Web.HttpContext.Current.Response;
                //wb.Save(Server.MapPath("~/Resource/Temp/" + fName));
                wb.Save(Server.UrlEncode(fName), Aspose.Cells.FileFormatType.Excel2003, Aspose.Cells.SaveType.OpenInBrowser, resp);

                return Success("�����ɹ���");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        [HttpPost]
        public ActionResult ExpInfo(string json)
        {
            try
            {
                Operator curUser = OperatorProvider.Provider.Current();
                string title = "��ȫ����";
                //�����ļ���
                string fileName = curUser.OrganizeName + title + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
                title = curUser.OrganizeName + "<br />" + title;
                json = HttpUtility.UrlDecode(json);
                //���ص���ģ��
                Aspose.Words.Document doc = new Aspose.Words.Document(Server.MapPath("~/Resource/DocumentFile/html.docx"));
                Aspose.Words.DocumentBuilder db = new Aspose.Words.DocumentBuilder(doc);
                //���뵽��ǩ
                db.MoveToBookmark("title");
                db.InsertHtml(title);
                db.MoveToBookmark("HTML");
                db.InsertHtml(json.Replace(@"\", "").Replace(@"&nbsp;", "").TrimStart('"').TrimEnd('"'));
                doc.Save(Server.MapPath("~/Resource/Temp/" + fileName));
                string path = Server.MapPath("~/Resource/Temp/" + fileName);
                return Success("�����ɹ�", fileName);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        /// <summary>
        /// ���밲ȫ����
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(5, "���밲ȫ����")]
        public string ImportData()
        {
            var user = OperatorProvider.Provider.Current();//������˾          
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
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                wb.Open(Server.MapPath("~/Resource/temp/" + fileName));
                Aspose.Cells.Cells cells = wb.Worksheets[0].Cells;
                DataTable dt = cells.ExportDataTable(0, 0, cells.MaxDataRow + 1, cells.MaxColumn + 1, true);

                string orgid = OperatorProvider.Provider.Current().OrganizeId;
                int order = 2;
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    SafetyMeshEntity item = new SafetyMeshEntity();
                    order++;
                    #region ��������
                    string MeshName = dt.Rows[i][0].ToString();
                    if (!string.IsNullOrEmpty(MeshName))
                    {
                        item.MeshName = MeshName;
                    }
                    else
                    {
                        falseMessage += string.Format(@"��{0}�е���ʧ��,�������Ʋ���Ϊ�գ�</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region ��������
                    string District = dt.Rows[i][1].ToString();
                    bool flag = false;
                    if (!string.IsNullOrEmpty(District))
                    {
                        string DistrictIdStr = "";
                        string DistrictStr = "";
                        string[] districtArr = District.Split(',');

                        foreach (string district in districtArr)
                        {
                            var disItem = new DistrictBLL().GetListForCon(x => x.DistrictName == district.Trim() && x.OrganizeId == orgid).FirstOrDefault();
                            if (disItem != null)
                            {
                                DistrictIdStr += disItem.DistrictID + ",";
                                DistrictStr += district.Trim() + ",";
                            }
                            else
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,��������[" + district + "]�����ڣ�</br>", order);
                                error++;
                                flag = true;
                                continue;
                            }
                        }
                        item.DistrictId = DistrictIdStr.TrimEnd(',');
                        item.District = DistrictStr.TrimEnd(',');
                    }
                    if (flag)
                    {
                        continue;
                    }
                    #endregion

                    #region �ϼ���������
                    string Superior = dt.Rows[i][2].ToString();
                    if (!string.IsNullOrEmpty(Superior))
                    {
                        var disItem = SafetyMeshbll.GetListForCon(x => x.MeshName == Superior.Trim() && x.CreateUserOrgCode == user.OrganizeCode).FirstOrDefault();
                        if (disItem != null)
                        {
                            item.SuperiorId = disItem.Id;
                            item.SuperiorName = disItem.MeshName;
                            if (disItem.MeshRank == "1")
                            {
                                item.MeshRank = "2";
                            }
                            if (disItem.MeshRank == "2")
                            {
                                item.MeshRank = "3";
                            }
                            if (disItem.MeshRank == "3")
                            {
                                item.MeshRank = "4";
                            }
                            if (disItem.MeshRank == "4")
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,�ϼ���������[" + Superior + "]Ϊ΢������</br>", order);
                                error++;
                                continue;
                            }
                            var list = SafetyMeshbll.GetListForCon(x => x.MeshName == item.MeshName && x.SuperiorId == item.SuperiorId && x.CreateUserOrgCode == user.OrganizeCode).ToList();
                            bool IsOk = list.Count() == 0 ? true : false;
                            if (!IsOk)
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,��������[" + item.MeshName + "]��[" + item.SuperiorName + "]�¼�����������</br>", order);
                                error++;
                                continue;
                            }
                        }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,�ϼ���������[" + Superior + "]�����ڣ�</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        item.MeshRank = "1";
                        var list = SafetyMeshbll.GetListForCon(x => x.MeshName == item.MeshName && x.MeshRank == "1" && x.CreateUserOrgCode == user.OrganizeCode).ToList();
                        bool IsOk = list.Count() == 0 ? true : false;
                        if (!IsOk)
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,��������[" + item.MeshName + "]����������</br>", order);
                            error++;
                            continue;
                        }
                    }
                    #endregion

                    #region ������
                    string DutyUser = dt.Rows[i][3].ToString();
                    if (!string.IsNullOrEmpty(DutyUser))
                    {
                        string DutyUserIdStr = "";
                        string DutyUserStr = "";
                        string[] userArr = DutyUser.Split(',');
                        foreach (string useritem in userArr)
                        {
                            if (!string.IsNullOrEmpty(useritem.Trim()))
                            {
                                string[] Arr = useritem.Split('/');
                                if (Arr.Count() == 1)
                                {
                                    var userList = new UserBLL().GetListForCon(x => x.RealName == useritem.Trim() && x.OrganizeId == orgid).ToList();
                                    if (userList.Count == 1)
                                    {
                                        var userEntity = userList.FirstOrDefault();
                                        DutyUserIdStr += userEntity.UserId + ",";
                                        DutyUserStr += useritem.Trim() + ",";
                                    }
                                    else if (userList.Count > 1)
                                    {
                                        falseMessage += string.Format(@"��{0}�е���ʧ��,������[" + useritem + "]�������밴��ʽ[����/�˺�]������д��</br>", order);
                                        error++;
                                        flag = true;
                                        break;
                                    }
                                    else
                                    {
                                        falseMessage += string.Format(@"��{0}�е���ʧ��,������[" + useritem + "]�����ڣ�</br>", order);
                                        error++;
                                        flag = true;
                                        break;
                                    }
                                }
                                else if (Arr.Count() == 2)
                                {
                                    string name = Arr[0].Trim();
                                    string account = Arr[1].Trim();
                                    var userEntity2 = new UserBLL().GetListForCon(x => x.RealName == name && x.Account == account && x.OrganizeId == orgid).FirstOrDefault();
                                    if (userEntity2 != null)
                                    {
                                        DutyUserIdStr += userEntity2.UserId + ",";
                                        DutyUserStr += userEntity2.RealName + ",";
                                    }
                                    else
                                    {
                                        falseMessage += string.Format(@"��{0}�е���ʧ��,������[" + Arr[0].Trim() + "]�ʺ�[" + Arr[1].Trim() + "]�����ڣ�</br>", order);
                                        error++;
                                        flag = true;
                                        break;
                                    }
                                }
                                else
                                {
                                    falseMessage += string.Format(@"��{0}�е���ʧ��,������[" + useritem + "]�����ʽ����</br>", order);
                                    error++;
                                    flag = true;
                                    break;
                                }
                            }
                        }
                        item.DutyUserId = DutyUserIdStr.TrimEnd(',');
                        item.DutyUser = DutyUserStr.TrimEnd(',');
                    }
                    else
                    {
                        falseMessage += string.Format(@"��{0}�е���ʧ��,�����˲���Ϊ�գ�</br>", order);
                        error++;
                        flag = true;
                        break;
                    }
                    if (flag)
                    {
                        continue;
                    }
                    #endregion

                    #region ��ϵ�绰
                    string DutyTel = dt.Rows[i][4].ToString();
                    if (!string.IsNullOrEmpty(DutyTel))
                    {
                        item.DutyTel = DutyTel;
                    }
                    #endregion

                    #region ����
                    string SortCode = dt.Rows[i][5].ToString();
                    int tempSortCode;
                    if (!string.IsNullOrEmpty(SortCode))
                    {
                        if (int.TryParse(SortCode, out tempSortCode))
                            item.SortCode = tempSortCode;
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,�����������Ϊ���֣�</br>", order);
                            error++;
                            continue;
                        }
                    }
                    #endregion

                    #region ����ְ��
                    string WorkJob = dt.Rows[i][6].ToString();
                    if (!string.IsNullOrEmpty(WorkJob))
                    {
                        item.WorkJob = WorkJob;
                    }
                    #endregion
                    try
                    {
                        SafetyMeshbll.SaveForm("", item);
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
        #endregion
    }
}
