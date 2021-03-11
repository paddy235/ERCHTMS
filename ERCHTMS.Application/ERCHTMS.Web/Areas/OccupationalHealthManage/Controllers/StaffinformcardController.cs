using ERCHTMS.Entity.OccupationalHealthManage;
using ERCHTMS.Busines.OccupationalHealthManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Entity.PublicInfoManage;
using System.Data;
using ERCHTMS.Code;

namespace ERCHTMS.Web.Areas.OccupationalHealthManage.Controllers
{
    /// <summary>
    /// 描 述：职业病危害告知卡
    /// </summary>
    public class StaffinformcardController : MvcControllerBase
    {
        private StaffinformcardBLL staffinformcardbll = new StaffinformcardBLL();

        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.IsSystem = ERCHTMS.Code.OperatorProvider.Provider.Current().IsSystem;
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
        /// 告知卡库
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CardLibraryForm()
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
        public ActionResult GetListJson(Pagination pagination, string queryJson, string type)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "ID";
            pagination.p_fields = "INFORMCARDNAME,INFORMCARDVALUE,INFORMACARDPOSITION,SETTINGTIME,FileId,FileName,filepath";//注：此处要替换成需要查询的列
            pagination.p_tablename = "V_STAFFINFORMCARD";
            pagination.conditionJson = "CARDTYPE=" + type;
            pagination.sidx = "SETTINGTIME";

            if (type == "0")//告知卡库不判断权限
            {
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
            }

            var data = staffinformcardbll.GetPageListByProc(pagination, queryJson);
            var jsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return ToJsonResult(jsonData);

            //var data = occupationalstaffdetailbll.GetList(queryJson);
            //return ToJsonResult(data);
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = staffinformcardbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #endregion

        #region 提交数据

        /// <summary>
        /// 复制告知卡到本单位
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="Fileid">文件主键</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult CopyForm(string keyValue, string Fileid)
        {
            StaffinformcardEntity data = staffinformcardbll.GetEntity(keyValue);//先根据id获取实体
            data.CardType = 0;
            data.Id = System.Guid.NewGuid().ToString();
            staffinformcardbll.SaveForm("", data);//进行新增
            FileInfoBLL file = new FileInfoBLL();
            FileInfoEntity fi = file.GetEntity(Fileid);//获取文件信息
            string oldfilename = fi.FilePath.Substring(fi.FilePath.LastIndexOf('/') + 1);
            string url = fi.FilePath.Substring(1, fi.FilePath.LastIndexOf('/') + 1);//不要~
            string[] filenames = oldfilename.Split('.');
            if (filenames.Length > 1)
            {
                string hz = filenames[filenames.Length - 1];//后缀名
                string newfilename = System.Guid.NewGuid().ToString() + "." + hz;
                string newUrl = url + newfilename;
                string oldPath = fi.FilePath.Substring(1);
                CopyFile(oldPath, newUrl);//复制到新地址
                fi.FileId = "";
                fi.RecId = data.Id;
                fi.FilePath = "~" + newUrl;//加入记录时加上~
                file.SaveForm("", fi);//生成新的文件记录
                return Success("true");
            }
            return Success("false");

        }

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="dir1">要复制的文件的路径已经全名(包括后缀)</param>
        /// <param name="dir2">目标位置,并指定新的文件名</param>
        public void CopyFile(string dir1, string dir2)
        {
            dir1 = dir1.Replace("/", "\\");
            dir2 = dir2.Replace("/", "\\");
            if (System.IO.File.Exists(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir1))
            {
                System.IO.File.Copy(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir1, System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir2, true);
            }
        }

        /// <summary>
        /// 导出到Excel
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult Excel(string queryJson)
        {
            string whereSql = "";
            ERCHTMS.Code.Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                whereSql = "";
            }
            else
            {
                string where = new ERCHTMS.Busines.AuthorizeManage.AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
                whereSql += " and " + where;
            }
            whereSql += " and CARDTYPE=0";
            DataTable dt = staffinformcardbll.GetTable(queryJson, whereSql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i][0] = (i + 1).ToString();
            }
            string FileUrl = @"\Resource\ExcelTemplate\职业病危害告知卡_导出模板.xlsx";



            AsposeExcelHelper.ExecuteResult(dt, FileUrl, "职业病危害告知卡", "职业病危害告知卡");

            return Success("导出成功。");
        }


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
            staffinformcardbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, StaffinformcardEntity entity)
        {
            staffinformcardbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}
