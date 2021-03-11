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
    /// 描 述：五定安全检查
    /// </summary>
    public class FivesafetycheckController : MvcControllerBase
    {
        private FivesafetycheckBLL fivesafetycheckbll = new FivesafetycheckBLL();
        private AptitudeinvestigateauditBLL aptitudeinvestigateauditbll = new AptitudeinvestigateauditBLL();
        private PeopleReviewBLL peoplereviewbll = new PeopleReviewBLL();
        private ManyPowerCheckBLL powerCheckbll = new ManyPowerCheckBLL();
        private FivesafetycheckauditBLL fivesafetycheckauditbll = new FivesafetycheckauditBLL();

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
        /// 新增检查页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CheckForm()
        {
            return View();
        }

        /// <summary>
        /// 流程图
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
        ///  导入
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取首页内容
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
        /// 查询审核流程图
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="urltype">查询类型：0：安全考核</param>
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
            //报告对象
            string fileName = Server.MapPath("~/Resource/ExcelTemplate/五定安全检查导出模板.docx");
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
                        row1["actionresult"] = assmentData.Rows[i]["actionresult"].ToString() == "0" ? "已完成" : "未完成";
                    }
                    
                    row1["actualdate"] = assmentData.Rows[i]["actualdate"];
                    row1["beizhu"] = assmentData.Rows[i]["beizhu"];
                    dt1.Rows.Add(row1);
                }
            }
            doc.MailMerge.ExecuteWithRegions(dt1);
            doc.MailMerge.DeleteFields();
            doc.Save(resp, Server.UrlEncode("检查问题清单_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".doc"), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
        }


        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = fivesafetycheckbll.GetList(queryJson);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取整改情况列表分页
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
        /// 获取审核信息
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
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
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
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = fivesafetycheckbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #endregion

        #region 提交数据
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
            fivesafetycheckbll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, FivesafetycheckEntity entity)
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            string state = string.Empty;

            string moduleName = "五定安全检查";

            string flowid = string.Empty;

            ManyPowerCheckEntity mpcEntity = peoplereviewbll.CheckAuditPower(curUser, out state, moduleName, "", false);
            entity.CHECKEDDEPARTIDALL = entity.CHECKEDDEPARTID;


            if (mpcEntity == null)
            {
                // 没有扣成就直接进入整改中
                entity.ISSAVED = 1;
                entity.ISOVER = 1;
            }
            else
            {
                //直接进入申请中（没有保存）
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

            return Success("操作成功。");
        }

        /// <summary>
        /// 检查审批考核
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <param name="aentity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult ApporveForm(string keyValue, AptitudeinvestigateauditEntity aentity)
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            FivesafetycheckEntity entity = fivesafetycheckbll.GetEntity(keyValue);

            string state = string.Empty;

            string moduleName = "五定安全检查";

            ManyPowerCheckEntity mpcEntity = peoplereviewbll.CheckAuditPower(curUser, out state, moduleName, "", false, entity.FLOWID);

            #region //审核信息表
            AptitudeinvestigateauditEntity aidEntity = new AptitudeinvestigateauditEntity();
            aidEntity.AUDITRESULT = aentity.AUDITRESULT; //通过
            aidEntity.AUDITTIME = Convert.ToDateTime(aentity.AUDITTIME.Value.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss")); //审核时间
            aidEntity.AUDITPEOPLE = aentity.AUDITPEOPLE;  //审核人员姓名
            aidEntity.AUDITPEOPLEID = aentity.AUDITPEOPLEID;//审核人员id
            aidEntity.APTITUDEID = keyValue;  //关联的业务ID 
            aidEntity.AUDITDEPTID = aentity.AUDITDEPTID;//审核部门id
            aidEntity.AUDITDEPT = aentity.AUDITDEPT; //审核部门
            aidEntity.AUDITOPINION = aentity.AUDITOPINION; //审核意见
            aidEntity.FlowId = entity.FLOWID; //流程步骤
            if (null != mpcEntity)
            {
                aidEntity.REMARK = (mpcEntity.AUTOID.Value - 1).ToString(); //备注 存流程的顺序号
            }
            else
            {
                aidEntity.REMARK = "7";
            }

            
            aptitudeinvestigateauditbll.SaveForm(aidEntity.ID, aidEntity);
            #endregion

            #region  保存
            //审核通过
            if (aentity.AUDITRESULT == "0")
            {
                //判断是否走的会签，走会签就要判断是不是所有的部门都有签到,未完成的会签只添加审核记录
                var checkstep = powerCheckbll.GetEntity(entity.FLOWID);

                int hqresult = 0; // 0：会签完成，走后续流程 1：会签未完成
                if (checkstep.SERIALNUM == 1) // 脚本会签
                {
                    var deptauditlist = fivesafetycheckbll.GetStepDept(checkstep, keyValue); //获取未会签的部门
                    foreach (UserEntity userinfp in deptauditlist)
                    {
                        if (userinfp.DepartmentId != curUser.DeptId)
                        {
                            hqresult = 1;
                        }
                        else //更新当前人会签记录
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

                if (hqresult == 0) // 会签完成，走后续流程
                {
                    //0表示流程未完成，1表示流程结束 会签走完了才执行后面的代码
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
            else //审核不通过 
            {
                entity.ISSAVED = 0;
                entity.ISOVER = 0;
                entity.CHECKDEPTALL = "";
                entity.CHECKUSERALL = "";
                entity.FLOWID = "";

                // 审批不通过，将历史审批记录打标（为流程图）
                //获取当前业务对象的所有审核记录
                var shlist = aptitudeinvestigateauditbll.GetAuditList(keyValue);
                //批量更新审核记录关联ID
                foreach (AptitudeinvestigateauditEntity mode in shlist)
                {
                    mode.REMARK = "-1"; //历史审批记录改成-1
                    aptitudeinvestigateauditbll.SaveForm(mode.ID, mode);
                }

            }
            fivesafetycheckbll.SaveForm(keyValue, entity);

            #endregion


            return Success("操作成功!");
        }


         /// <summary>
         /// 导入检查问题详情
         /// </summary>
         /// <param name="keyvalue"></param>
         /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportQuestion(string keyvalue)
        {
            if (OperatorProvider.Provider.Current().IsSystem)
            {
                return "超级管理员无此操作权限";
            }
            string orgId = OperatorProvider.Provider.Current().OrganizeId;//所属公司
            int error = 0;
            string message = "请选择格式正确的文件再导入!";
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
                    //序号
                    string sortnum = dt.Rows[i][0].ToString();
                    //发现问题
                    string findquestion = dt.Rows[i][1].ToString();
                    //整改措施
                    string actioncontent = dt.Rows[i][2].ToString();
                    //责任部门
                    string dutydept = dt.Rows[i][3].ToString();
                    //责任人
                    string dutyusername = dt.Rows[i][4].ToString();
                    //要求完成时间
                    string finishdate = dt.Rows[i][5].ToString();
                    //验收人
                    string acceptuser = dt.Rows[i][6].ToString();

                    //整改完成情况
                    string actionresult = dt.Rows[i][7].ToString();
                    //实际完成时间
                    string actualdate = dt.Rows[i][8].ToString();
                    //备注
                    string beizhu = dt.Rows[i][9].ToString();

                    //---****值存在空验证*****--
                    if (string.IsNullOrEmpty(findquestion) || string.IsNullOrEmpty(actioncontent) || string.IsNullOrEmpty(dutydept) || string.IsNullOrEmpty(dutyusername) || string.IsNullOrEmpty(finishdate) || string.IsNullOrEmpty(acceptuser))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行值存在空,未能导入.";
                        error++;
                        continue;
                    }

                    if (actionresult == "已完成"&& string.IsNullOrEmpty(actualdate))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行值,整改完成情况为已完成时必须填实际完成时间.";
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
                            // 如果查询不到部门，智能提醒检查 刘畅
                            string stdept = fivesafetycheckbll.GetDeptByName(dutydept);
                            if (stdept != "" && stdept != null)
                            {
                                falseMessage += "</br>" + "第" + (i + 2) + "行责任部门未在系统中查到,智能识别判断您输入的可能是<"+ stdept + ">.";
                            }
                            else
                            {
                                falseMessage += "</br>" + "第" + (i + 2) + "行责任部门未在系统中查到,未能导入.";
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
                                    falseMessage += "</br>" + "第" + (i + 2) + "行责任人未在系统中查到,未能导入.";
                                    error++;
                                    continue;
                                }
                                
                            }
                            else
                            {
                                falseMessage += "</br>" + "第" + (i + 2) + "行责任人未在系统中查到,未能导入.";
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
                                falseMessage += "</br>" + "第" + (i + 2) + "行责任人未在系统中查到,未能导入.";
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
                                    falseMessage += "</br>" + "第" + (i + 2) + "行验收人未在系统中查到,未能导入.";
                                    error++;
                                    continue;
                                }

                            }
                            else
                            {
                                falseMessage += "</br>" + "第" + (i + 2) + "行验收人未在系统中查到,未能导入.";
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
                                falseMessage += "</br>" + "第" + (i + 2) + "行验收人未在系统中查到,未能导入.";
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
                        falseMessage += "</br>" + "第" + (i + 2) + "行要求完成时间有误,未能导入.";
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
                        falseMessage += "</br>" + "第" + (i + 2) + "行实际完成时间有误,未能导入.";
                        error++;
                        continue;
                    }

                    //完成情况
                    if (!string.IsNullOrEmpty(actionresult))
                    {
                        if (actionresult == "已完成")
                        {
                            pe.ACTIONRESULT = "0";
                            pe.CHECKPASS = "0";
                            pe.ACCEPTREUSLT = "0";
                        }
                        else if (actionresult == "未完成")
                        {
                            pe.ACTIONRESULT = "1";
                            pe.ACTUALDATE = null;
                        }
                        else
                        {
                            falseMessage += "</br>" + "第" + (i + 2) + "行整改完成情况有误,未能导入.";
                            error++;
                            continue;
                        }
                    }


                    

                    //备注
                    if (beizhu.Length > 2000)
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行备注文本过长,未能导入.";
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
                        falseMessage += "</br>" + "第" + (i + 2) + "行保存失败,未能导入.";
                        error++;
                        continue;
                    }
                }
                count = dt.Rows.Count - 1;
                message = "共有" + count + "条记录,成功导入" + (count - error) + "条，失败" + error + "条";
                message += "</br>" + falseMessage;
            }
            return message;
        }

        #endregion
    }
}
