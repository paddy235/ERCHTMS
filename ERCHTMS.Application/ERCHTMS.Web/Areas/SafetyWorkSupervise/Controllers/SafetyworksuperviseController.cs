using ERCHTMS.Entity.SafetyWorkSupervise;
using ERCHTMS.Busines.SafetyWorkSupervise;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Collections.Generic;
using ERCHTMS.Code;
using System;
using System.Web;
using System.Data;
using BSFramework.Util.Offices;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using System.Linq;

namespace ERCHTMS.Web.Areas.SafetyWorkSupervise.Controllers
{
    /// <summary>
    /// 描 述：安全重点工作督办
    /// </summary>
    public class SafetyworksuperviseController : MvcControllerBase
    {
        SuperviseconfirmationBLL superviseconfirmationbll = new SuperviseconfirmationBLL();
        private SafetyworksuperviseBLL safetyworksupervisebll = new SafetyworksuperviseBLL();
        SafetyworkfeedbackBLL safetyworkfeedbackbll = new SafetyworkfeedbackBLL();
        private DepartmentBLL departmentBLL = new DepartmentBLL();

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
        /// 重点工作页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SuperviseDetail()
        {
            return View();
        }
        /// <summary>
        /// 反馈、督办确认工作页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FeedbackDetail()
        {
            return View();
        }
        /// <summary>
        /// 流程图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Flow()
        {
            return View();
        }
        /// <summary>
        /// 导入
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }
        /// <summary>
        /// 批量确认
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Confirm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = safetyworksupervisebll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.conditionJson = " 1=1 ";

                var data = safetyworksupervisebll.GetPageList(pagination, queryJson);
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = safetyworksupervisebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取实体/链表查 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="fid">历史反馈记录id</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetEntityByT(string keyValue,string fid)
        {
            var data = safetyworksupervisebll.GetEntityByT(keyValue, fid);
            List<SuperviseEntity> list = new List<SuperviseEntity>();
            if (data != null&&data.Rows.Count>0) {
                list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SuperviseEntity>>(data.ToJson());
            }
            if (list.Count > 0) {
                return ToJsonResult(list[0]);
            }
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取流程图对象
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult GetFlow(string keyValue)
        {
            var josnData = safetyworksupervisebll.GetFlow(keyValue);
            return Content(josnData.ToJson());

        }
        /// <summary>
        /// 获取首页统计图
        /// </summary>
        /// <param name="keyValue">1表示上个月</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSuperviseStat(string keyValue)
        {
            try
            {
                var data = safetyworksupervisebll.GetSuperviseStat(keyValue);
                return ToJsonResult(data);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

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
            safetyworksupervisebll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, SafetyworksuperviseEntity entity)
        {
            safetyworksupervisebll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        /// <summary>
        /// 批量下发
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult MultiSend(string keyValue)
        {
            try
            {
                string sql = string.Format("update BIS_SAFETYWORKSUPERVISE set FlowState='1' where id in('{0}')", keyValue.Trim(',').Replace(",", "','"));
                int count = new DepartmentBLL().ExecuteSql(sql);
                return Success("操作成功。");
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
           
        }
        /// <summary>
        /// 批量确认
        /// </summary>
        /// <param name="SuperviseIds">督办任务ID(多个用逗号分隔)</param>
        /// <param name="result">审核结果（0：同意，1：不同意）</param>
        /// <param name="signImg">签名照片路径</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult MultiConfirm(string SuperviseIds, string result,string signImg)
        {
            try
            {
                bool res=safetyworksupervisebll.MutilConfirm(SuperviseIds, result, signImg);
                return res?Success("操作成功"): Error("操作失败");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

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
        public ActionResult SaveFormY(SafetyworksuperviseEntity entity, [System.Web.Http.FromBody]string dataJson)
        {
            if (dataJson.Length > 0)
            {
                List<SafetyworksuperviseEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SafetyworksuperviseEntity>>(dataJson);
                int s = 0;
                DateTime dd = DateTime.Now;
                foreach (SafetyworksuperviseEntity data in list)
                {
                    s = s + 1;
                    data.CreateDate = dd.AddSeconds(s);
                    safetyworksupervisebll.SaveForm("", data);
                }
            }
            return Success("操作成功。");
        }
        #endregion
        /// <summary>
        /// 导入督办数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportData()
        {
            try
            {
                int error = 0;
                int sussceed = 0;
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
                    var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                    string filePath = Server.MapPath("~/Resource/temp/" + fileName);
                    file.SaveAs(filePath);
                    DataTable dt = ExcelHelper.ExcelImport(filePath);
                    DepartmentBLL deptBll = new DepartmentBLL();
                    UserBLL userbll = new UserBLL();
                    string supervisedate = string.Empty;
                    string worktask = string.Empty;string flowstate = "0";//默认下发
                    string dutydeptname = string.Empty, dutyperson = string.Empty, superviseperson = string.Empty;
                    string supervisedeptname = string.Empty, finishdate = string.Empty, remark = string.Empty;
                    string dutydeptid = string.Empty, dutydeptcode = string.Empty, dutypersonid = string.Empty;
                    string supervisepersonid = string.Empty, supervisedeptid = string.Empty, supervisedeptcode = string.Empty;
                    int num = 0;
                    List<SafetyworksuperviseEntity> list = new List<SafetyworksuperviseEntity>();
                    for (int i = 0; i < dt.Rows.Count; i++) {
                        object[] vals = dt.Rows[i].ItemArray;
                        if (IsEndRow(vals) == true)
                            break;
                        if (i == 0)
                        {
                            supervisedate = vals[1].ToString();//督办时间
                            if (string.IsNullOrEmpty(supervisedate))
                            {
                                falseMessage += "第" + (i + 1) + "行:督办时间不能为空！</br>";
                                error++;
                                continue;
                            }
                        }
                        else if (i == 1)
                        {
                            continue;//第二行是列名,跳过
                        }
                        else {
                            num++;
                            if (!string.IsNullOrEmpty(vals[0].ToString()))
                            {
                                worktask = vals[0].ToString();//重点工作内容
                            }
                            //else {
                            //    falseMessage += "第" + (i + 1) + "行:重点工作内容不能为空！</br>";
                            //    error++;
                            //    continue;
                            //}
                            dutydeptname = vals[1].ToString();//责任单位名称
                            if (string.IsNullOrEmpty(dutydeptname))
                            {
                                falseMessage += "第" + (i + 2) + "行:责任单位名称不能为空！</br>";
                                error++;
                                continue;
                            }

                            var p1 = string.Empty;
                            var p2 = string.Empty;
                            bool isSkip = false;
                            string dname = string.Empty;
                            //验证所填部门是否存在
                            var array = dutydeptname.Split('/');
                            for (int j = 0; j < array.Length; j++)
                            {
                                if (j == 0)
                                {
                                    var entity = departmentBLL.GetList().Where(x => x.OrganizeId == user.OrganizeId && x.Nature == "厂级" && x.FullName == array[j].ToString()).FirstOrDefault();
                                    if (entity == null)
                                    {
                                        entity = departmentBLL.GetList().Where(x => x.OrganizeId == user.OrganizeId && x.Nature == "部门" && x.FullName == array[j].ToString()).FirstOrDefault();
                                        if (entity == null)
                                        {
                                            entity = departmentBLL.GetList().Where(x => x.OrganizeId == user.OrganizeId && x.Nature == "承包商" && x.FullName == array[j].ToString()).FirstOrDefault();
                                            if (entity == null)
                                            {
                                                falseMessage += "第" + (i + 2) + "行:责任部门与系统的部门信息不匹配！</br>";
                                                error++;
                                                isSkip = true;
                                                break;
                                            }
                                            else
                                            {
                                                dutydeptid = entity.DepartmentId;
                                                dutydeptcode = entity.EnCode;
                                                dutydeptname = entity.FullName;
                                                p1 = entity.DepartmentId;
                                            }
                                        }
                                        else
                                        {
                                            dutydeptid = entity.DepartmentId;
                                            dutydeptcode = entity.EnCode;
                                            dutydeptname = entity.FullName;
                                            p1 = entity.DepartmentId;
                                        }
                                    }
                                    else
                                    {
                                        dutydeptid = entity.DepartmentId;
                                        dutydeptcode = entity.EnCode;
                                        dutydeptname = entity.FullName;
                                        p1 = entity.DepartmentId;
                                    }
                                }
                                else if (j == 1)
                                {
                                    var entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == user.OrganizeId && (x.Nature == "专业" || x.Nature == "承包商" || x.Nature == "分包商") && x.FullName == array[j].ToString() && x.ParentId == p1).FirstOrDefault();
                                    if (entity1 == null)
                                    {
                                        entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == user.OrganizeId && x.Nature == "班组" && x.FullName == array[j].ToString() && x.ParentId == p1).FirstOrDefault();
                                        if (entity1 == null)
                                        {
                                            falseMessage += "第" + (i + 2) + "行:责任部门与系统的部门信息不匹配！</br>";
                                            error++;
                                            isSkip = true;
                                            break;
                                        }
                                        else
                                        {
                                            dutydeptid = entity1.DepartmentId;
                                            dutydeptcode = entity1.EnCode;
                                            dutydeptname = entity1.FullName;
                                            p2 = entity1.DepartmentId;
                                        }
                                    }
                                    else
                                    {
                                        dutydeptid = entity1.DepartmentId;
                                        dutydeptcode = entity1.EnCode;
                                        dutydeptname = entity1.FullName;
                                        p2 = entity1.DepartmentId;
                                        if (entity1.Nature == "承包商")
                                        {
                                            if (!string.IsNullOrEmpty(p1))
                                            {
                                                var d = departmentBLL.GetEntity(p1);
                                                if (d != null)
                                                {
                                                    dname = d.FullName;
                                                }
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    var entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == user.OrganizeId && (x.Nature == "班组" || x.Nature == "承包商" || x.Nature == "分包商") && x.FullName == array[j].ToString() && x.ParentId == p2).FirstOrDefault();
                                    if (entity1 == null)
                                    {
                                        falseMessage += "第" + (i + 2) + "行:责任部门与系统的部门信息不匹配！</br>";
                                        error++;
                                        isSkip = true;
                                        break;
                                    }
                                    else
                                    {
                                        dutydeptid = entity1.DepartmentId;
                                        dutydeptcode = entity1.EnCode;
                                        dutydeptname = entity1.FullName;
                                        if (entity1.Nature == "承包商")
                                        {
                                            if (!string.IsNullOrEmpty(p2)) {
                                                var d = departmentBLL.GetEntity(p2);
                                                if (d != null)
                                                {
                                                    dname = d.FullName;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (isSkip)
                            {
                                continue;
                            }

                            //dutydeptcode = deptBll.GetDeptCode(dutydeptname, user.OrganizeId);//获取责任单位code
                            //if (string.IsNullOrEmpty(dutydeptcode))
                            //{
                            //    falseMessage += "第" + (i + 1) + "行:责任部门与系统的部门信息不匹配！</br>";
                            //    error++;
                            //    continue;
                            //}
                            //else {
                            //    dutydeptid = deptBll.GetEntityByCode(dutydeptcode).DepartmentId;//获取责任单位id
                            //}
                            dutyperson = vals[2].ToString();//责任人
                            if (string.IsNullOrEmpty(dutyperson))
                            {
                                falseMessage += "第" + (i + 2) + "行:责任人不能为空！</br>";
                                error++;
                                continue;
                            }
                            UserInfoEntity userEntity = userbll.GetUserInfoByName(dutydeptname, dutyperson);
                            if (!string.IsNullOrEmpty(dname)) {
                                userEntity = userbll.GetUserInfoByName(dname, dutyperson);
                            }
                            if (userEntity == null)
                            {
                                falseMessage += "第" + (i + 2) + "行:责任人和责任部门信息不匹配！</br>";
                                error++;
                                continue;
                            }
                            else {
                                dutypersonid = userEntity.UserId;
                            }
                            supervisedeptname = vals[4].ToString();//督办部门
                            if (string.IsNullOrEmpty(supervisedeptname))
                            {
                                falseMessage += "第" + (i + 2) + "行:督办部门不能为空！</br>";
                                error++;
                                continue;
                            }

                            p1 = string.Empty;
                            p2 = string.Empty;
                            isSkip = false;
                            dname = string.Empty;
                            //验证所填部门是否存在
                            array = supervisedeptname.Split('/');
                            for (int j = 0; j < array.Length; j++)
                            {
                                if (j == 0)
                                {
                                    var entity = departmentBLL.GetList().Where(x => x.OrganizeId == user.OrganizeId && x.Nature == "厂级" && x.FullName == array[j].ToString()).FirstOrDefault();
                                    if (entity == null)
                                    {
                                        entity = departmentBLL.GetList().Where(x => x.OrganizeId == user.OrganizeId && x.Nature == "部门" && x.FullName == array[j].ToString()).FirstOrDefault();
                                        if (entity == null)
                                        {
                                            entity = departmentBLL.GetList().Where(x => x.OrganizeId == user.OrganizeId && x.Nature == "承包商" && x.FullName == array[j].ToString()).FirstOrDefault();
                                            if (entity == null)
                                            {
                                                falseMessage += "第" + (i + 2) + "行:督办部门与系统的部门信息不匹配！</br>";
                                                error++;
                                                isSkip = true;
                                                break;
                                            }
                                            else
                                            {
                                                supervisedeptid = entity.DepartmentId;
                                                supervisedeptcode = entity.EnCode;
                                                supervisedeptname = entity.FullName;
                                                p1 = entity.DepartmentId;
                                            }
                                        }
                                        else
                                        {
                                            supervisedeptid = entity.DepartmentId;
                                            supervisedeptcode = entity.EnCode;
                                            supervisedeptname = entity.FullName;
                                            p1 = entity.DepartmentId;
                                        }
                                    }
                                    else
                                    {
                                        supervisedeptid = entity.DepartmentId;
                                        supervisedeptcode = entity.EnCode;
                                        supervisedeptname = entity.FullName;
                                        p1 = entity.DepartmentId;
                                    }
                                }
                                else if (j == 1)
                                {
                                    var entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == user.OrganizeId && (x.Nature == "专业" || x.Nature == "承包商" || x.Nature == "分包商") && x.FullName == array[j].ToString() && x.ParentId == p1).FirstOrDefault();
                                    if (entity1 == null)
                                    {
                                        entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == user.OrganizeId && x.Nature == "班组" && x.FullName == array[j].ToString() && x.ParentId == p1).FirstOrDefault();
                                        if (entity1 == null)
                                        {
                                            falseMessage += "第" + (i + 2) + "行:督办部门与系统的部门信息不匹配！</br>";
                                            error++;
                                            isSkip = true;
                                            break;
                                        }
                                        else
                                        {
                                            supervisedeptid = entity1.DepartmentId;
                                            supervisedeptcode = entity1.EnCode;
                                            supervisedeptname = entity1.FullName;
                                            p2 = entity1.DepartmentId;
                                        }
                                    }
                                    else
                                    {
                                        supervisedeptid = entity1.DepartmentId;
                                        supervisedeptcode = entity1.EnCode;
                                        supervisedeptname = entity1.FullName;
                                        p2 = entity1.DepartmentId;
                                        if (entity1.Nature == "承包商")
                                        {
                                            if (!string.IsNullOrEmpty(p1))
                                            {
                                                var d = departmentBLL.GetEntity(p1);
                                                if (d != null)
                                                {
                                                    dname = d.FullName;
                                                }
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    var entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == user.OrganizeId && (x.Nature == "班组" || x.Nature == "承包商" || x.Nature == "分包商") && x.FullName == array[j].ToString() && x.ParentId == p2).FirstOrDefault();
                                    if (entity1 == null)
                                    {
                                        falseMessage += "第" + (i + 2) + "行:督办部门与系统的部门信息不匹配！</br>";
                                        error++;
                                        isSkip = true;
                                        break;
                                    }
                                    else
                                    {
                                        supervisedeptid = entity1.DepartmentId;
                                        supervisedeptcode = entity1.EnCode;
                                        supervisedeptname = entity1.FullName;
                                        if (entity1.Nature == "承包商")
                                        {
                                            if (!string.IsNullOrEmpty(p2))
                                            {
                                                var d = departmentBLL.GetEntity(p2);
                                                if (d != null)
                                                {
                                                    dname = d.FullName;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (isSkip)
                            {
                                continue;
                            }


                            //supervisedeptcode = deptBll.GetDeptCode(supervisedeptname, user.OrganizeId);//获取督办部门code
                            //if (string.IsNullOrEmpty(supervisedeptcode))
                            //{
                            //    falseMessage += "第" + (i + 1) + "行:督办部门与系统的部门信息不匹配！</br>";
                            //    error++;
                            //    continue;
                            //}
                            //else
                            //{
                            //    supervisedeptid = deptBll.GetEntityByCode(supervisedeptcode).DepartmentId;//获取责任单位id
                            //}
                            superviseperson = vals[3].ToString();//督办人
                            if (string.IsNullOrEmpty(superviseperson))
                            {
                                falseMessage += "第" + (i + 2) + "行:督办人不能为空！</br>";
                                error++;
                                continue;
                            }
                            userEntity = userbll.GetUserInfoByName(supervisedeptname, superviseperson);
                            if (!string.IsNullOrEmpty(dname)) {
                                userEntity = userbll.GetUserInfoByName(dname, superviseperson);
                            }
                            if (userEntity == null)
                            {
                                falseMessage += "第" + (i + 2) + "行:督办人和督办部门信息不匹配！</br>";
                                error++;
                                continue;
                            }
                            else
                            {
                                supervisepersonid = userEntity.UserId;
                            }
                            finishdate = vals[5].ToString();//完成时间
                            if (string.IsNullOrEmpty(finishdate))
                            {
                                falseMessage += "第" + (i + 2) + "行:完成时间不能为空！</br>";
                                error++;
                                continue;
                            }
                            remark = vals[6].ToString();//备注
                            SafetyworksuperviseEntity entityWork = new SafetyworksuperviseEntity()
                            {
                                Id = Guid.NewGuid().ToString(),
                                SuperviseDate = string.IsNullOrEmpty(supervisedate) ? DateTime.Now : Convert.ToDateTime(supervisedate + "-01"),
                                WorkTask = worktask,
                                DutyDeptName = dutydeptname,
                                DutyDeptId = dutydeptid,
                                DutyDeptCode = dutydeptcode,
                                DutyPerson = dutyperson,
                                DutyPersonId = dutypersonid,
                                SupervisePerson = superviseperson,
                                SupervisePersonId = supervisepersonid,
                                SuperviseDeptName = supervisedeptname,
                                SuperviseDeptId = supervisedeptid,
                                SuperviseDeptCode = supervisedeptcode,
                                FinishDate = Convert.ToDateTime(finishdate),
                                FlowState = flowstate,
                                Remark = remark,

                            };
                            list.Add(entityWork);
                        }
                        
                    }
                    //count = dt.Rows.Count;
                    //message = "共有" + num + "条记录,成功导入" + sussceed + "条，失败" + error + "条";
                    message = "共有" + num + "条记录,失败" + error + "条,请修改后重新导入";
                    if (error > 0)
                    {
                        message += "，错误信息如下：</br>" + falseMessage;
                    }
                    else {
                        
                        try
                        {
                            int s = 0;
                            DateTime dd = DateTime.Now;
                            foreach (SafetyworksuperviseEntity item in list)
                            {
                                s = s + 1;
                                item.CreateDate = dd.AddSeconds(s);
                                safetyworksupervisebll.SaveForm("", item);
                                sussceed++;
                            }
                            message = "共有" + num + "条记录,成功导入" + sussceed + "条";
                        }
                        catch (Exception ex)
                        {
                            message = "导入失败：" + ex.Message;
                        }
                        
                        
                    }

                    //删除临时文件
                    System.IO.File.Delete(filePath);
                }
                return message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        private bool IsEndRow(object[] vals)
        {
            bool r = false;

            r = Array.TrueForAll(vals, x => (x == null || x == DBNull.Value || x.ToString() == ""));

            return r;
        }
        #region 数据导出
        /// <summary>
        /// 导出重点工作内容
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HandlerMonitor(0, "导出重点工作内容")]
        public string Export(string queryJson)
        {
            try
            {

                var queryParam = queryJson.ToJObject();
                string sTime = queryParam["sTime"].ToString();//开始时间
                string eTime = queryParam["eTime"].ToString();//结束时间
                DateTime s = Convert.ToDateTime(sTime);
                DateTime e = Convert.ToDateTime(eTime);
                int year = e.Year - s.Year;
                int Month = year * 12 + (e.Month - s.Month) + 1;
                string DutyDeptCode = queryParam["DutyDeptCode"].ToString();//责任部门(单位)
                string DutyDeptName = queryParam["DutyDeptName"].ToString();//责任部门(单位)名称
                var userInfo = OperatorProvider.Provider.Current();  //获取当前用户
                string fileName = "安全重点工作督办单_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
                string strDocPath = Server.MapPath("~/Resource/ExcelTemplate/安全重点工作督办单导出模板.docx");
                string name = "安全重点工作督办单" + DateTime.Now.ToString("yyyyMMddhhmmss");
                HttpResponse resp = System.Web.HttpContext.Current.Response;

                if (!string.IsNullOrEmpty(DutyDeptCode))
                {
                    string pic = Server.MapPath("~/content/Images/no_1.png");//默认图片
                    string tablename = @" BIS_SafetyWorkSupervise t 
                        left join (select c.* from  BIS_SafetyWorkFeedback c where c.flag='0') t1 
on t.id=t1.superviseid left join (select c1.* from BIS_SuperviseConfirmation c1 where c1.flag='0') t2 
on t1.id=t2.feedbackid";
                    string sqlWhere = string.Format(@"");
                    string[] str = DutyDeptCode.Split(',');
                    string[] strName = DutyDeptName.Split(',');
                    for (int i = 0; i < Month; i++)
                    {
                        string timeStr = s.AddMonths(i).ToString("yyyy-MM");
                        int c = 0;
                        foreach (string item in str)
                        {
                            DataSet ds = new DataSet();
                            DataTable dtPro = new DataTable("project");
                            dtPro.Columns.Add("Year");
                            dtPro.Columns.Add("Mon");

                            DataTable dt = new DataTable("list");
                            dt.Columns.Add("xh");//序号
                            dt.Columns.Add("worktask");//重点工作任务
                            dt.Columns.Add("dutydeptname");//责任部门（单位）
                            dt.Columns.Add("finishdate");//完成时间
                            dt.Columns.Add("remark");//备注
                            dt.Columns.Add("finishinfo");//完成情况
                            dt.Columns.Add("Sign1");//部门（单位）确认签字
                            dt.Columns.Add("Sign2");//党组织成员签字
                            DataRow row = dtPro.NewRow();
                            if (string.IsNullOrEmpty(item))
                            {
                                continue;
                            }
                            fileName = string.Format(@"{0}年{1}月安全重点工作督办单（{2}）.docx", s.AddMonths(i).Year, s.AddMonths(i).Month, strName[c]);
                            sqlWhere = string.Format(@" t.flowstate!=0 and to_char(t.supervisedate,'yyyy-MM')='{0}' and t.dutydeptcode = '{1}'", timeStr, item);
                            Pagination pagination = new Pagination
                            {
                                page = 1,
                                rows = 100000000,
                                p_kid = "t.Id",
                                p_fields = "t.WorkTask,t.DutyDeptName,to_char(t.FinishDate,'yyyy-MM-dd') as FinishDate,t.Remark,t1.FinishInfo,t1.SignUrl,t2.SignUrl as SignUrlT",
                                p_tablename = tablename,
                                conditionJson = sqlWhere,
                                sidx = "t.CreateDate",
                                sord = "desc"
                            };
                            row["Year"] = s.AddMonths(i).Year;
                            row["Mon"] = s.AddMonths(i).Month;
                            dtPro.Rows.Add(row);
                            DataTable data = safetyworksupervisebll.GetPageList(pagination, queryJson);
                            int j = 1;
                            foreach (DataRow dr in data.Rows)
                            {
                                DataRow dtrow = dt.NewRow();
                                dtrow["xh"] = j;
                                dtrow["worktask"] = dr["worktask"].ToString();
                                dtrow["dutydeptname"] = dr["dutydeptname"].ToString();
                                dtrow["finishdate"] = dr["finishdate"].ToString();
                                dtrow["remark"] = dr["remark"].ToString();
                                dtrow["finishinfo"] = dr["finishinfo"].ToString();
                                var filepath1 = "";
                                filepath1 = string.IsNullOrEmpty(dr["SignUrl"].ToString()) ? "" : (Server.MapPath("~/") + dr["SignUrl"].ToString().Replace("../../", "").ToString()).Replace(@"\/", "\\").ToString();
                                if (!string.IsNullOrEmpty(filepath1) && System.IO.File.Exists(filepath1))
                                    dtrow["Sign1"] = filepath1;
                                else
                                    dtrow["Sign1"] = pic;

                                var filepath2 = "";
                                filepath2 = string.IsNullOrEmpty(dr["SignUrlT"].ToString()) ? "" : (Server.MapPath("~/") + dr["SignUrlT"].ToString().Replace("../../", "").ToString()).Replace(@"\/", "\\").ToString();
                                if (!string.IsNullOrEmpty(filepath2) && System.IO.File.Exists(filepath2))
                                    dtrow["Sign2"] = filepath2;
                                else
                                    dtrow["Sign2"] = pic;
                                dt.Rows.Add(dtrow);
                                j++;
                            }
                            ds.Tables.Add(dt);
                            ds.Tables.Add(dtPro);
                            Aspose.Words.Document doc = new Aspose.Words.Document(strDocPath);
                            doc.MailMerge.Execute(dtPro);
                            doc.MailMerge.ExecuteWithRegions(dt);
                            doc.MailMerge.DeleteFields();
                            string path = Server.MapPath("~/") + "Resource/Temp/" + name + "/" + fileName;
                            doc.Save(path, Aspose.Words.SaveFormat.Docx);
                            c++;
                        }
                    }
                    //压缩
                    FastZip fz = new FastZip();
                    fz.CreateEmptyDirectories = true;
                    string FilePath = Server.MapPath("~/") + "Resource/Temp/" + name + ".zip";
                    fz.CreateZip(FilePath, Server.MapPath("~/") + "Resource/Temp/" + name, true, "");
                    fz = null;
                    Directory.Delete(Server.MapPath("~/") + "Resource/Temp/" + name, true);
                    return name + ".zip";
                    //return RedirectToAction("DownloadFile", "../Utility",new { filePath= Server.UrlEncode("~/Resource/Temp/" + name + ".zip") });
                }
                else {
                    return "1";
                }
            }
            catch (Exception ex)
            {
                return "-1";
            }

        }
        #endregion
    }
}
