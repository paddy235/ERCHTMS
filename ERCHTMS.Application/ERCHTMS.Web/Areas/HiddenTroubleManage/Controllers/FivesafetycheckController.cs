using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using System;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.BaseManage;
using System.Web;
using Aspose.Words;
using System.Data;

namespace ERCHTMS.Web.Areas.HiddenTroubleManage.Controllers
{
    /// <summary>
    /// �� �����嶨��ȫ���
    /// </summary>
    public class FivesafetycheckController : MvcControllerBase
    {
        private FivesafetycheckBLL fivesafetycheckbll = new FivesafetycheckBLL();
        private AptitudeinvestigateauditBLL aptitudeinvestigateauditbll = new AptitudeinvestigateauditBLL();
        private PeopleReviewBLL peoplereviewbll = new PeopleReviewBLL();
        private ManyPowerCheckBLL powerCheckbll = new ManyPowerCheckBLL();
        private FivesafetycheckauditBLL fivesafetycheckauditbll = new FivesafetycheckauditBLL();

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
        /// �������ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CheckForm()
        {
            return View();
        }

        /// <summary>
        /// ����ͼ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FlowForm()
        {
            return View();
        }

        [HttpGet]
        public ActionResult List()
        {
            return View();
        }

        /// <summary>
        ///  ����
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }
        #endregion

        #region ��ȡ����
        /// <summary>
        /// ��ȡ��ҳ����
        /// </summary>
        /// <param name="itemcode"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeskTotalByCheckType(string itemcode)
        {
            try
            {
                var data = fivesafetycheckbll.DeskTotalByCheckType(itemcode);
                return ToJsonResult(data);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }


        /// <summary>
        /// ��ѯ�������ͼ
        /// </summary>
        /// <param name="keyValue">����</param>
        /// <param name="urltype">��ѯ���ͣ�0����ȫ����</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAuditFlowData(string keyValue, string urltype)
        {
            try
            {
                var data = fivesafetycheckbll.GetAuditFlowData(keyValue, urltype);
                return ToJsonResult(data);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyvalue"></param>
        public void ExportAuditTotal(string keyvalue)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            HttpResponse resp = System.Web.HttpContext.Current.Response;
            //�������
            string fileName = Server.MapPath("~/Resource/ExcelTemplate/�嶨��ȫ��鵼��ģ��.docx");
            Aspose.Words.Document doc = new Aspose.Words.Document(fileName);
            DocumentBuilder builder = new DocumentBuilder(doc);
            DataTable dt = new DataTable();
            var info = fivesafetycheckbll.GetEntity(keyvalue);
            dt.Columns.Add("question");
            DataRow row = dt.NewRow();

            if (info == null)
            {
                row["question"] = "";
            }
            else
            {
                row["question"] = info.CHECKNAME;
            }
            


            dt.Rows.Add(row);
            doc.MailMerge.Execute(dt);

            DataTable dt1 = new DataTable("A");
            dt1.Columns.Add("sortnum");
            dt1.Columns.Add("findquestion");
            dt1.Columns.Add("actioncontent");
            dt1.Columns.Add("dutydeptid");
            dt1.Columns.Add("dutyusername");
            dt1.Columns.Add("finishdate");
            dt1.Columns.Add("acceptuser");
            dt1.Columns.Add("actionresult");

            dt1.Columns.Add("actualdate");
            dt1.Columns.Add("beizhu");
            DataTable assmentData = fivesafetycheckbll.ExportAuditTotal(keyvalue);
            if (assmentData.Rows.Count > 0)
            {
                for (int i = 0; i < assmentData.Rows.Count; i++)
                {
                    DataRow row1 = dt1.NewRow();
                    row1["sortnum"] = i + 1;
                    row1["findquestion"] = assmentData.Rows[i]["findquestion"];
                    row1["actioncontent"] = assmentData.Rows[i]["actioncontent"];

                    row1["dutydeptid"] = assmentData.Rows[i]["dutydept"];
                    row1["dutyusername"] = assmentData.Rows[i]["dutyusername"];
                    row1["finishdate"] = assmentData.Rows[i]["finishdate"];
                    row1["acceptuser"] = assmentData.Rows[i]["acceptuser"];
                    if (assmentData.Rows[i]["actionresult"] != null)
                    {
                        row1["actionresult"] = assmentData.Rows[i]["actionresult"].ToString() == "0" ? "�����" : "δ���";
                    }
                    
                    row1["actualdate"] = assmentData.Rows[i]["actualdate"];
                    row1["beizhu"] = assmentData.Rows[i]["beizhu"];
                    dt1.Rows.Add(row1);
                }
            }
            doc.MailMerge.ExecuteWithRegions(dt1);
            doc.MailMerge.DeleteFields();
            doc.Save(resp, Server.UrlEncode("��������嵥_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".doc"), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
        }


        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = fivesafetycheckbll.GetList(queryJson);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ��������б��ҳ
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAuditListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var data = fivesafetycheckbll.GetAuditListJson(pagination, queryJson);
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
        /// ��ȡ�����Ϣ
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetApplyListJson(Pagination pagination, string queryJson)
        {


            var watch = CommonHelper.TimerStart();
            var queryParam = queryJson.ToJObject();
            var data = aptitudeinvestigateauditbll.GetAuditList(queryParam["ID"].ToString());
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
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var data = fivesafetycheckbll.GetPageListJson(pagination, queryJson);
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
            var data = fivesafetycheckbll.GetEntity(keyValue);
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
            fivesafetycheckbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, FivesafetycheckEntity entity)
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            string state = string.Empty;

            string moduleName = "�嶨��ȫ���";

            string flowid = string.Empty;

            ManyPowerCheckEntity mpcEntity = peoplereviewbll.CheckAuditPower(curUser, out state, moduleName, "", false);
            entity.CHECKEDDEPARTIDALL = entity.CHECKEDDEPARTID;


            if (mpcEntity == null)
            {
                // û�п۳ɾ�ֱ�ӽ���������
                entity.ISSAVED = 1;
                entity.ISOVER = 1;
            }
            else
            {
                //ֱ�ӽ��������У�û�б��棩
                entity.ISSAVED = 1;
                entity.ISOVER = 0;
                entity.FLOWDEPT = mpcEntity.CHECKDEPTID;
                entity.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                entity.FLOWROLE = mpcEntity.CHECKROLEID;
                entity.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                entity.FLOWNAME = mpcEntity.FLOWNAME;
                entity.FLOWID = mpcEntity.ID;
            }



            fivesafetycheckbll.SaveForm(keyValue, entity);

            return Success("�����ɹ���");
        }

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <param name="aentity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult ApporveForm(string keyValue, AptitudeinvestigateauditEntity aentity)
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            FivesafetycheckEntity entity = fivesafetycheckbll.GetEntity(keyValue);

            string state = string.Empty;

            string moduleName = "�嶨��ȫ���";

            ManyPowerCheckEntity mpcEntity = peoplereviewbll.CheckAuditPower(curUser, out state, moduleName, "", false, entity.FLOWID);

            #region //�����Ϣ��
            AptitudeinvestigateauditEntity aidEntity = new AptitudeinvestigateauditEntity();
            aidEntity.AUDITRESULT = aentity.AUDITRESULT; //ͨ��
            aidEntity.AUDITTIME = Convert.ToDateTime(aentity.AUDITTIME.Value.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss")); //���ʱ��
            aidEntity.AUDITPEOPLE = aentity.AUDITPEOPLE;  //�����Ա����
            aidEntity.AUDITPEOPLEID = aentity.AUDITPEOPLEID;//�����Աid
            aidEntity.APTITUDEID = keyValue;  //������ҵ��ID 
            aidEntity.AUDITDEPTID = aentity.AUDITDEPTID;//��˲���id
            aidEntity.AUDITDEPT = aentity.AUDITDEPT; //��˲���
            aidEntity.AUDITOPINION = aentity.AUDITOPINION; //������
            aidEntity.FlowId = entity.FLOWID; //���̲���
            if (null != mpcEntity)
            {
                aidEntity.REMARK = (mpcEntity.AUTOID.Value - 1).ToString(); //��ע �����̵�˳���
            }
            else
            {
                aidEntity.REMARK = "7";
            }

            
            aptitudeinvestigateauditbll.SaveForm(aidEntity.ID, aidEntity);
            #endregion

            #region  ����
            //���ͨ��
            if (aentity.AUDITRESULT == "0")
            {
                //�ж��Ƿ��ߵĻ�ǩ���߻�ǩ��Ҫ�ж��ǲ������еĲ��Ŷ���ǩ��,δ��ɵĻ�ǩֻ�����˼�¼
                var checkstep = powerCheckbll.GetEntity(entity.FLOWID);

                int hqresult = 0; // 0����ǩ��ɣ��ߺ������� 1����ǩδ���
                if (checkstep.SERIALNUM == 1) // �ű���ǩ
                {
                    var deptauditlist = fivesafetycheckbll.GetStepDept(checkstep, keyValue); //��ȡδ��ǩ�Ĳ���
                    foreach (UserEntity userinfp in deptauditlist)
                    {
                        if (userinfp.DepartmentId != curUser.DeptId)
                        {
                            hqresult = 1;
                        }
                        else //���µ�ǰ�˻�ǩ��¼
                        {
                            if (entity.CHECKDEPTALL == "" || entity.CHECKDEPTALL == null)
                            {
                                entity.CHECKDEPTALL = curUser.DeptId;
                                entity.CHECKUSERALL = curUser.UserId;
                            }
                            else
                            {
                                entity.CHECKDEPTALL += "," + curUser.DeptId;
                                entity.CHECKUSERALL += "," + curUser.UserId;
                            }
                            
                        }
                    }

                    
                }

                if (hqresult == 0) // ��ǩ��ɣ��ߺ�������
                {
                    //0��ʾ����δ��ɣ�1��ʾ���̽��� ��ǩ�����˲�ִ�к���Ĵ���
                    if (null != mpcEntity)
                    {
                        entity.ISOVER = 0;
                        entity.FLOWID = mpcEntity.ID;
                        entity.FLOWDEPT = mpcEntity.CHECKDEPTID;
                        entity.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                        entity.FLOWROLE = mpcEntity.CHECKROLEID;
                        entity.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                        entity.FLOWNAME = mpcEntity.FLOWNAME;
                    }
                    else
                    {
                        entity.ISOVER = 1;
                        entity.FLOWID = "";

                        entity.FLOWDEPT = "";
                        entity.FLOWDEPTNAME = "";
                        entity.FLOWROLE = "";
                        entity.FLOWROLENAME = "";
                        entity.FLOWNAME = "";
                    }
                }

                
                
            }
            else //��˲�ͨ�� 
            {
                entity.ISSAVED = 0;
                entity.ISOVER = 0;
                entity.CHECKDEPTALL = "";
                entity.CHECKUSERALL = "";
                entity.FLOWID = "";

                // ������ͨ��������ʷ������¼��꣨Ϊ����ͼ��
                //��ȡ��ǰҵ������������˼�¼
                var shlist = aptitudeinvestigateauditbll.GetAuditList(keyValue);
                //����������˼�¼����ID
                foreach (AptitudeinvestigateauditEntity mode in shlist)
                {
                    mode.REMARK = "-1"; //��ʷ������¼�ĳ�-1
                    aptitudeinvestigateauditbll.SaveForm(mode.ID, mode);
                }

            }
            fivesafetycheckbll.SaveForm(keyValue, entity);

            #endregion


            return Success("�����ɹ�!");
        }


         /// <summary>
         /// ��������������
         /// </summary>
         /// <param name="keyvalue"></param>
         /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportQuestion(string keyvalue)
        {
            if (OperatorProvider.Provider.Current().IsSystem)
            {
                return "��������Ա�޴˲���Ȩ��";
            }
            string orgId = OperatorProvider.Provider.Current().OrganizeId;//������˾
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
                DataTable dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn + 1, false);
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    //���
                    string sortnum = dt.Rows[i][0].ToString();
                    //��������
                    string findquestion = dt.Rows[i][1].ToString();
                    //���Ĵ�ʩ
                    string actioncontent = dt.Rows[i][2].ToString();
                    //���β���
                    string dutydept = dt.Rows[i][3].ToString();
                    //������
                    string dutyusername = dt.Rows[i][4].ToString();
                    //Ҫ�����ʱ��
                    string finishdate = dt.Rows[i][5].ToString();
                    //������
                    string acceptuser = dt.Rows[i][6].ToString();

                    //����������
                    string actionresult = dt.Rows[i][7].ToString();
                    //ʵ�����ʱ��
                    string actualdate = dt.Rows[i][8].ToString();
                    //��ע
                    string beizhu = dt.Rows[i][9].ToString();

                    //---****ֵ���ڿ���֤*****--
                    if (string.IsNullOrEmpty(findquestion) || string.IsNullOrEmpty(actioncontent) || string.IsNullOrEmpty(dutydept) || string.IsNullOrEmpty(dutyusername) || string.IsNullOrEmpty(finishdate) || string.IsNullOrEmpty(acceptuser))
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "��ֵ���ڿ�,δ�ܵ���.";
                        error++;
                        continue;
                    }

                    if (actionresult == "�����"&& string.IsNullOrEmpty(actualdate))
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "��ֵ,����������Ϊ�����ʱ������ʵ�����ʱ��.";
                        error++;
                        continue;
                    }
                    FivesafetycheckauditEntity pe = new FivesafetycheckauditEntity();
                    pe.ID = Guid.NewGuid().ToString();
                    pe.FINDQUESTION = findquestion;
                    pe.ACTIONCONTENT = actioncontent;
                    pe.CHECKPASS = "1";
                    pe.CHECKID = keyvalue;


                    pe.DUTYDEPT = dutydept;
                    if (!string.IsNullOrEmpty(dutydept))
                    {
                        string[] ar = dutydept.Split('/');
                        //int resultdept = 0;
                        DataTable deptentity = null;
                        string deptid = "";
                        foreach (string arstr in ar)
                        {
                            if (deptid == "")
                            {
                                deptentity = fivesafetycheckbll.GetInfoBySql("select departmentid,encode from base_department   where fullname = '" + arstr + "' ");
                            }
                            else
                            {
                                deptentity = fivesafetycheckbll.GetInfoBySql("select departmentid,encode from base_department   where  PARENTID = '"+ deptid + "' and  fullname = '" + arstr + "' ");
                            }
                            
                            if (deptentity.Rows.Count == 0)
                            {
                                //resultdept = 1;
                                break;
                            }
                            else
                            {
                                deptid = deptentity.Rows[0]["departmentid"].ToString();
                            }
                            

                        }
                        //var deptentity = fivesafetycheckbll.GetInfoBySql("select departmentid,encode from base_department   where fullname = '" + dutydept + "' ");


                        if (deptentity.Rows.Count > 0)
                        {
                            pe.DUTYDEPTCODE = deptentity.Rows[0]["encode"].ToString();
                            pe.DUTYDEPTID = deptentity.Rows[0]["departmentid"].ToString();
                        }
                        else
                        {
                            // �����ѯ�������ţ��������Ѽ�� ����
                            string stdept = fivesafetycheckbll.GetDeptByName(dutydept);
                            if (stdept != "" && stdept != null)
                            {
                                falseMessage += "</br>" + "��" + (i + 2) + "�����β���δ��ϵͳ�в鵽,����ʶ���ж�������Ŀ�����<"+ stdept + ">.";
                            }
                            else
                            {
                                falseMessage += "</br>" + "��" + (i + 2) + "�����β���δ��ϵͳ�в鵽,δ�ܵ���.";
                            }
                            
                            error++;
                            continue;
                        }
                        
                    }

                    pe.DUTYUSERNAME = dutyusername;
                    if (!string.IsNullOrEmpty(dutyusername))
                    {
                        if (dutyusername.IndexOf('/') > -1)
                        {
                            var deptentity = fivesafetycheckbll.GetInfoBySql("select userid,mobile from base_user where realname = '" + dutyusername.Split('/')[0] + "' ");
                            if (deptentity.Rows.Count == 1)
                            {
                                pe.DUTYUSERID = deptentity.Rows[0]["userid"].ToString();
                            }
                            else if (deptentity.Rows.Count > 1)
                            {
                                deptentity = fivesafetycheckbll.GetInfoBySql("select userid,mobile from base_user where realname = '" + dutyusername.Split('/')[0] + "' and mobile='"+ dutyusername.Split('/')[1] + "' ");
                                if (deptentity.Rows[0]["mobile"].ToString() == dutyusername.Split('/')[1])
                                {
                                    pe.DUTYUSERID = deptentity.Rows[0]["userid"].ToString();
                                }
                                else
                                {
                                    falseMessage += "</br>" + "��" + (i + 2) + "��������δ��ϵͳ�в鵽,δ�ܵ���.";
                                    error++;
                                    continue;
                                }
                                
                            }
                            else
                            {
                                falseMessage += "</br>" + "��" + (i + 2) + "��������δ��ϵͳ�в鵽,δ�ܵ���.";
                                error++;
                                continue;
                            }
                        }
                        else
                        {
                            var deptentity = fivesafetycheckbll.GetInfoBySql("select userid from base_user where realname = '" + dutyusername + "' ");
                            if (deptentity.Rows.Count > 0)
                            {
                                pe.DUTYUSERID = deptentity.Rows[0]["userid"].ToString();
                            }
                            else
                            {
                                falseMessage += "</br>" + "��" + (i + 2) + "��������δ��ϵͳ�в鵽,δ�ܵ���.";
                                error++;
                                continue;
                            }
                        }
                        

                    }


                    pe.ACCEPTUSER = acceptuser;
                    if (!string.IsNullOrEmpty(acceptuser))
                    {
                        if (acceptuser.IndexOf('/') > -1)
                        {
                            var deptentity = fivesafetycheckbll.GetInfoBySql("select userid,mobile from base_user where realname = '" + acceptuser.Split('/')[0] + "' ");
                            if (deptentity.Rows.Count == 1)
                            {
                                pe.ACCEPTUSERID = deptentity.Rows[0]["userid"].ToString();
                            }
                            else if (deptentity.Rows.Count > 1)
                            {
                                deptentity = fivesafetycheckbll.GetInfoBySql("select userid,mobile from base_user where realname = '" + acceptuser.Split('/')[0] + "' and  mobile='" + acceptuser.Split('/')[1] + "' ");
                                if (deptentity.Rows[0]["mobile"].ToString() == acceptuser.Split('/')[1])
                                {
                                    pe.ACCEPTUSERID = deptentity.Rows[0]["userid"].ToString();
                                }
                                else
                                {
                                    falseMessage += "</br>" + "��" + (i + 2) + "��������δ��ϵͳ�в鵽,δ�ܵ���.";
                                    error++;
                                    continue;
                                }

                            }
                            else
                            {
                                falseMessage += "</br>" + "��" + (i + 2) + "��������δ��ϵͳ�в鵽,δ�ܵ���.";
                                error++;
                                continue;
                            }
                        }
                        else
                        {
                            var deptentity = fivesafetycheckbll.GetInfoBySql("select userid from base_user where realname = '" + acceptuser + "' ");
                            if (deptentity.Rows.Count > 0)
                            {
                                pe.ACCEPTUSERID = deptentity.Rows[0]["userid"].ToString();
                            }
                            else
                            {
                                falseMessage += "</br>" + "��" + (i + 2) + "��������δ��ϵͳ�в鵽,δ�ܵ���.";
                                error++;
                                continue;
                            }
                        }

                        

                    }
                    try
                    {
                        if (!string.IsNullOrEmpty(finishdate))
                        {
                            pe.FINISHDATE = DateTime.Parse(DateTime.Parse(finishdate).ToString("yyyy-MM-dd"));
                        }

                    }
                    catch
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "��Ҫ�����ʱ������,δ�ܵ���.";
                        error++;
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(actualdate))
                        {
                            pe.ACTUALDATE = DateTime.Parse(DateTime.Parse(finishdate).ToString("yyyy-MM-dd"));
                        }

                    }
                    catch
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "��ʵ�����ʱ������,δ�ܵ���.";
                        error++;
                        continue;
                    }

                    //������
                    if (!string.IsNullOrEmpty(actionresult))
                    {
                        if (actionresult == "�����")
                        {
                            pe.ACTIONRESULT = "0";
                            pe.CHECKPASS = "0";
                            pe.ACCEPTREUSLT = "0";
                        }
                        else if (actionresult == "δ���")
                        {
                            pe.ACTIONRESULT = "1";
                            pe.ACTUALDATE = null;
                        }
                        else
                        {
                            falseMessage += "</br>" + "��" + (i + 2) + "����������������,δ�ܵ���.";
                            error++;
                            continue;
                        }
                    }


                    

                    //��ע
                    if (beizhu.Length > 2000)
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "�б�ע�ı�����,δ�ܵ���.";
                        error++;
                        continue;
                    }
                    else
                    {
                        pe.BEIZHU = beizhu;
                    }

                    try
                    {
                        fivesafetycheckauditbll.SaveForm(pe.ID, pe);
                    }
                    catch
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "�б���ʧ��,δ�ܵ���.";
                        error++;
                        continue;
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
