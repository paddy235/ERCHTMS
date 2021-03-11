using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using ERCHTMS.Entity.LaborProtectionManage;
using ERCHTMS.Busines.LaborProtectionManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ERCHTMS.Busines.HseToolManage;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Entity.HseToolMange;
using System;
using BSFramework.Util.Extension;
using ERCHTMS.Busines.BaseManage;
using System.Data;
using System.Web;
using BSFramework.Util.Offices;
using System.Collections;

namespace ERCHTMS.Web.Areas.HseToolManage.Controllers
{/// <summary>
 /// 安全观察卡
 /// </summary>
    public class HseObserveController : MvcControllerBase
    {
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private UserBLL user = new UserBLL();

        private HseObserveBLL hseobservebll = new HseObserveBLL();
        private HseObserveNormBLL hseobservenormbll = new HseObserveNormBLL();

        #region 安全观察卡
        #region 视图
        // GET: HseToolManage/HseObserve
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Index2()
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>

        public ActionResult Form()
        {
            return View();
        }
        #endregion
        #region 获取数据
        /// <summary>
        /// 查看本人观察的记录
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "Id";
            pagination.p_fields = @"CREATEUSERID,CREATEUSERDEPTCODE,CREATEUSERORGCODE,DEPARTMENTCODE AS DEPTCODE,CREATEDATE,CREATEUSERNAME,MODIFYDATE,MODIFYUSERID,MODIFYUSERNAME,OBSERVEUSER,OBSERVEUSERID,DEPARTMENT,DEPARTMENTID,TASK,AREA,OBSERVEDATE,CONTENT,OBSERVETYPE,DESCRIBE,ISMODIFY,MEASURES,OBSERVEACTION,OBSERVELEVEL,OBSERVESTATE,OBSERVERESULT,CREATEUSERDEPT";
            pagination.p_tablename = @"HSE_SECURITYOBSERVE  ";
            pagination.sidx = "createdate";
            pagination.sord = "desc";
            pagination.conditionJson = "1=1";
            Operator currUser = OperatorProvider.Provider.Current();
            if (!currUser.IsSystem)
            {
                //根据当前用户对模块的权限获取记录
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "createuserdeptcode", "createuserorgcode");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }
            }
            var queryParam = queryJson.ToJObject();


            if (!string.IsNullOrEmpty(queryJson))
            {

                if (queryJson.Contains("selectType"))
                {
                    pagination.conditionJson += string.Format("and CREATEUSERID='{0}' ", currUser.UserId);
                }
                else
                {
                    pagination.conditionJson += string.Format(" and OBSERVESTATE !='{0}'", "未提交");

                }

                if (queryJson.Contains("deptcode"))
                {
                    if (!queryParam["deptcode"].IsEmpty())
                    {
                        pagination.conditionJson += string.Format(" and DEPARTMENTCODE like'{0}%'", queryParam["deptcode"].ToString());
                    }
                }

                if (!queryParam["starttime"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and to_char(CREATEDATE,'yyyy-mm-dd')>=to_char(to_date('{0}','yyyy-mm-dd'),'yyyy-mm-dd') ", queryParam["starttime"].ToString());
                }
                if (!queryParam["endtime"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and to_char(CREATEDATE,'yyyy-mm-dd')<=to_char(to_date('{0}','yyyy-mm-dd'),'yyyy-mm-dd') ", queryParam["endtime"].ToString());

                }
                if (!queryParam["txt_keyword"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and TASK like'%{0}%'", queryParam["txt_keyword"].ToString());
                }
                if (!queryParam["txt_state"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and OBSERVESTATE ='{0}'", queryParam["txt_state"].ToString());

                }
                if (!queryParam["txt_type"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and OBSERVETYPE like'%{0}%'", queryParam["txt_type"].ToString());

                }
            }
            else
            {
                pagination.conditionJson += string.Format("and CREATEUSERID='{0}' ", currUser.UserId);
            }
            var data = hseobservebll.GetPageList(pagination, null);



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
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            try
            {
                var data = hseobservebll.GetEntity(keyValue);
                return ToJsonResult(data);
            }
            catch (Exception)
            {

                return ToJsonResult("");
            }

        }

        #endregion

        #region 数据操作

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="keyValue"></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            try
            {
                hseobservebll.RemoveForm(keyValue);

                return Success("操作成功");
            }
            catch (Exception ex)
            {

                return Error(ex.Message);

            }

        }
        /// <summary>
        ///关闭观察卡
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveSuccessRecord(string keyValue, string content)
        {
            try
            {
                var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                HseObserveEntity entity = hseobservebll.GetEntity(keyValue);
                entity.ObserveResult = content;
                entity.ObserveState = "已关闭";
                entity.MODIFYDATE = DateTime.Now;
                entity.MODIFYUSERID = user.UserId;
                entity.MODIFYUSERNAME = user.UserName;
                hseobservebll.SaveForm(entity.Id, entity);
                return Success("操作成功");

            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }


        }
        /// <summary>
        /// 保存提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, HseObserveEntity entity)
        {
            Operator user = OperatorProvider.Provider.Current();
            try
            {

                if (entity.Id != keyValue)
                {
                    if (!string.IsNullOrEmpty(entity.Departmentid))
                    {
                        var dept = departmentBLL.GetEntity(entity.Departmentid);
                        entity.deptcode = dept.EnCode;
                    }

                    entity.CREATEDATE = DateTime.Now;
                    entity.CREATEUSERDEPT = user.DeptName;
                    entity.CREATEUSERID = user.UserId;
                    entity.CREATEUSERNAME = user.UserName;
                    entity.CREATEUSERDEPTCODE = user.DeptCode;
                    entity.CREATEUSERORGCODE = user.OrganizeCode;
                    hseobservebll.SaveForm("", entity);
                }
                else
                {
                    if (!string.IsNullOrEmpty(entity.Departmentid))
                    {
                        var dept = departmentBLL.GetEntity(entity.Departmentid);
                        entity.deptcode = dept.EnCode;
                    }
                    entity.MODIFYDATE = DateTime.Now;
                    entity.MODIFYUSERID = user.UserId;
                    entity.MODIFYUSERNAME = user.UserName;
                    hseobservebll.SaveForm(entity.Id, entity);
                }
                return Success("操作成功");
            }
            catch (System.Exception ex)
            {

                return Error(ex.Message);

            }
        }
        #endregion
        #endregion

        #region 安全观察内容标准
        #region 视图
        /// <summary>
        /// 内容标准首页
        /// </summary>
        /// <returns></returns>
        public ActionResult ContentIndex()
        {
            return View();
        }
        /// <summary>
        /// 内容标准
        /// </summary>
        /// <returns></returns>
        public ActionResult ContentForm()
        {
            return View();
        }
        /// <summary>
        /// 导入
        /// </summary>
        /// <returns></returns>
        public ActionResult Import()
        {
            return View();
        }
        #endregion
        #region 获取数据
        /// <summary>
        /// 查看本人观察的记录
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListNormJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "Id";
            pagination.p_fields = @"CREATEUSERID,CREATEUSERDEPTCODE,CREATEUSERORGCODE,CREATEDATE,CREATEUSERNAME,MODIFYDATE,MODIFYUSERID,MODIFYUSERNAME,NAME,OCONTENT";
            pagination.p_tablename = @"HSE_SECURITYOBSERVENORM  ";
            pagination.sidx = "createdate";
            pagination.sord = "desc";
            pagination.conditionJson = "1=1";
            Operator currUser = OperatorProvider.Provider.Current();
            if (!currUser.IsSystem)
            {
                //根据当前用户对模块的权限获取记录
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "createuserdeptcode", "createuserorgcode");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }
            }
            var queryParam = queryJson.ToJObject();

            //if (queryJson.Contains("deptcode"))
            //{
            //    if (!queryParam["deptcode"].IsEmpty())
            //    {
            //        pagination.conditionJson += string.Format(" and DEPARTMENTCODE like'{0}%'", queryParam["deptcode"].ToString());
            //    }
            //}

            //if (!queryParam["starttime"].IsEmpty())
            //{
            //    pagination.conditionJson += string.Format(" and to_char(CREATEDATE,'yyyy-mm-dd')>=to_char(to_date('{0}','yyyy-mm-dd'),'yyyy-mm-dd') ", queryParam["starttime"].ToString());
            //}
            //if (!queryParam["endtime"].IsEmpty())
            //{
            //    pagination.conditionJson += string.Format(" and to_char(CREATEDATE,'yyyy-mm-dd')<=to_char(to_date('{0}','yyyy-mm-dd'),'yyyy-mm-dd') ", queryParam["endtime"].ToString());

            //}
            var data = hseobservenormbll.GetPageList(pagination, null);
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
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetListJson()
        {
            try
            {
                var data = hseobservenormbll.GetList();
                return ToJsonResult(data);
            }
            catch (Exception)
            {

                return ToJsonResult("");
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetFormNormJson(string keyValue)
        {
            try
            {
                var data = hseobservenormbll.GetEntity(keyValue);
                return ToJsonResult(data);
            }
            catch (Exception)
            {

                return ToJsonResult("");
            }

        }

        #endregion

        #region 数据操作

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="keyValue"></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveFormNorm(string keyValue)
        {
            try
            {
                hseobservenormbll.RemoveForm(keyValue);

                return Success("操作成功");
            }
            catch (Exception ex)
            {

                return Error(ex.Message);

            }

        }

        /// <summary>
        /// 保存提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveFormNorm(string keyValue, HseObserveNormEntity entity)
        {
            Operator user = OperatorProvider.Provider.Current();
            try
            {

                if (entity.Id != keyValue)
                {


                    entity.CREATEDATE = DateTime.Now;
                    entity.CREATEUSERDEPT = user.DeptName;
                    entity.CREATEUSERID = user.UserId;
                    entity.CREATEUSERNAME = user.UserName;
                    entity.CREATEUSERDEPTCODE = user.DeptCode;
                    entity.CREATEUSERORGCODE = user.OrganizeCode;
                    //entity.MODIFYDATE = DateTime.Now;
                    entity.MODIFYUSERID = user.UserId;
                    entity.MODIFYUSERNAME = user.UserName;
                    hseobservenormbll.SaveForm("", entity);
                }
                else
                {

                    // entity.MODIFYDATE = DateTime.Now;
                    entity.MODIFYUSERID = user.UserId;
                    entity.MODIFYUSERNAME = user.UserName;
                    hseobservenormbll.SaveForm(entity.Id, entity);
                }
                return Success("操作成功");
            }
            catch (System.Exception ex)
            {

                return Error(ex.Message);

            }
        }
        #endregion



        #region 导入自评标准
        /// <summary>
        /// 导入自评标准
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportCase()
        {
            Operator user = OperatorProvider.Provider.Current();

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
                DataTable dt = ExcelHelper.ExcelImport(Server.MapPath("~/Resource/temp/" + fileName));
                Hashtable hast = new Hashtable();
                List<HseObserveNormEntity> listEntity = new List<HseObserveNormEntity>();
                HseObserveNormEntity Hse = new HseObserveNormEntity();
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i][0].ToString()))
                    {
                        Hse = new HseObserveNormEntity();
                        Hse.Id = Guid.NewGuid().ToString();
                        Hse.CREATEDATE = DateTime.Now;
                        Hse.CREATEUSERDEPT = user.DeptName;
                        Hse.CREATEUSERID = user.UserId;
                        Hse.CREATEUSERNAME = user.UserName;
                        Hse.CREATEUSERDEPTCODE = user.DeptCode;
                        Hse.CREATEUSERORGCODE = user.OrganizeCode;
                        Hse.MODIFYDATE = DateTime.Now;
                        Hse.MODIFYUSERID = user.UserId;
                        Hse.MODIFYUSERNAME = user.UserName;
                        Hse.Name = dt.Rows[i][0].ToString();
                        Hse.Ocontent = dt.Rows[i][1].ToString();
                        listEntity.Add(Hse);
                    }

                }
                for (int j = 0; j < listEntity.Count; j++)
                {
                    try
                    {
                        hseobservenormbll.SaveForm("", listEntity[j]);
                    }
                    catch (Exception ex)
                    {
                        error++;
                    }

                }
                count = listEntity.Count;
                message = "共有" + count + "条记录,成功导入" + (count - error) + "条，失败" + error + "条";
                message += "</br>" + falseMessage;
            }
            return message;
        }
        #endregion
        #endregion
    }
}