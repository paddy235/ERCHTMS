using Aspose.Words.Saving;
using BSFramework.Util;
using BSFramework.Util.Offices;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.HazardsourceManage;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.RiskDatabase;
using ERCHTMS.Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.HazardsourceManage.Controllers
{
    public class DjjdController : MvcControllerBase
    {
        private HazardsourceBLL hazardsourcebll = new HazardsourceBLL();
        //
        // GET: /HazardsourceManage/Djjd/


        [HandlerAuthorize(PermissionMode.Ignore)]
        public void DownloadFileForKeyValue(string keyValue)
        {
            FileInfoBLL fileInfoBLL = new FileInfoBLL();

            if (string.IsNullOrEmpty(keyValue)) { return; }
            DataTable data = null;

            data = fileInfoBLL.GetFiles(keyValue);
            if (data != null && data.Rows.Count > 0)
            {
                string name = string.IsNullOrEmpty(data.Rows[0]["FileName"].ToString()) ? Server.UrlDecode(data.Rows[0]["FileName"].ToString()) : Server.UrlDecode(data.Rows[0]["FileName"].ToString());//返回客户端文件名称
                string filepath = this.Server.MapPath(data.Rows[0]["FilePath"].ToString());
                if (FileDownHelper.FileExists(filepath))
                {
                    FileDownHelper.DownLoadold(filepath, name);
                }

            }
        }

        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult GetFileNameByKeyValue(string keyValue)
        {
            FileInfoBLL fileInfoBLL = new FileInfoBLL();

            if (string.IsNullOrEmpty(keyValue)) { return Content(""); }
            DataTable data = null;

            data = fileInfoBLL.GetFiles(keyValue);
            if (data == null || data.Rows.Count == 0)
                return Content("");
            string name = string.IsNullOrEmpty(data.Rows[0]["FileName"].ToString()) ? Server.UrlDecode(data.Rows[0]["FileName"].ToString()) : Server.UrlDecode(data.Rows[0]["FileName"].ToString());//返回客户端文件名称

            return Content(name);
        }


        public ActionResult SelectCommon()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Form()
        {
            return View();
        }


        [HandlerMonitor(0, "即时单下载")]
        public void Down(string keyValue)
        {
            HazardsourceBLL hazardsourcebll = new HazardsourceBLL();
            //查询数据
            var entity = hazardsourcebll.GetEntity(keyValue);
            //数据合并
            string[] text = new string[] { };
            if (entity.Way == "DEC")
                text = new string[] { "DistrictName", "DangerSource", "AccidentName", "DeptName", "JdglzrrFullName", 
                "Way", 
                "ItemDecQ",
                "ItemDecQ1", "ItemDecB", "ItemDecB1", "ItemDecR", "IsDanger", "Grade", "MeaSure"};
            else
                text = new string[] { "DistrictName", "DangerSource", "AccidentName", "DeptName", "JdglzrrFullName", 
                "Way", 
                "ItemA",
                "ItemB", "ItemC", "ItemR", "IsDanger", "Grade", "MeaSure"}; ;
            MeasuresBLL measuresbll = new MeasuresBLL();
            var data = measuresbll.GetList(" and riskId='" + keyValue + "'");

            var MeaSureShow = "";
            int i = 1;
            foreach (var item in data)
            {
                MeaSureShow += i + "、" + item.Content + "\r\n";
                i++;
            }


            //if (!string.IsNullOrEmpty(entity.MeaSure))
            //{
            //    var arr = entity.MeaSure.Split(';');

            //    int i = 1;
            //    foreach (var item in arr)
            //    {
            //        MeaSureShow += i + "、" + item + "\r\n";
            //        i++;
            //    }
            //}
            string[] value = new string[] { };
            var tempPath = Server.MapPath("~/Resource/Temp/辨识分级记录.doc");
            var outputPath = Server.MapPath("~/Resource/ExcelTemplate/辨识分级记录.doc");
            if (entity.Way == "DEC")
            {
                value = new string[] { 
                 entity.DistrictName,
                 entity.DangerSource,
                 entity.AccidentName,
                 entity.DeptName,
                 entity.JdglzrrFullName,
                 entity.Way == "DEC" ? "危险化学品重大危险源辨识" : "LEC法风险辨识",
                 entity.ItemDecQ,
                 entity.ItemDecQ1,
                 entity.ItemDecB.ToString(),
                 entity.ItemDecB1,
                 entity.ItemDecR.ToString(),
                 entity.IsDanger == 1 ? "是" : "否",
                 entity.GradeVal > 0 ? entity.Grade : "未定级",
                 MeaSureShow
            };
            }
            else
            {
                tempPath = Server.MapPath("~/Resource/Temp/辨识分级记录2.doc");
                outputPath = Server.MapPath("~/Resource/ExcelTemplate/辨识分级记录2.doc");
                value = new string[] { 
                 entity.DistrictName,
                 entity.DangerSource,
                 entity.AccidentName,
                 entity.DeptName,
                 entity.JdglzrrFullName,
                 entity.Way == "DEC" ? "危险化学品重大危险源辨识" : "法风险辨识",
                 entity.ItemA.ToString(),
                 entity.ItemB.ToString(),
                 entity.ItemC.ToString(),
                 entity.ItemR.ToString(),
                 entity.IsDanger == 1 ? "是" : "否",
                 entity.GradeVal > 0 ? entity.Grade : "未定级",
                 MeaSureShow
            };
            }




            Aspose.Words.Document doc = new Aspose.Words.Document(tempPath);
            //数据合并
            doc.MailMerge.Execute(text, value);
            doc.Save(outputPath);
            //调用导出方法
            var docStream = new MemoryStream();
            doc.Save(docStream, SaveOptions.CreateSaveOptions(Aspose.Words.SaveFormat.Doc));
            Response.ContentType = "application/msword";
            Response.AddHeader("content-disposition", "attachment;filename=辨识分级记录.doc");
            Response.BinaryWrite(docStream.ToArray());
            Response.End();
        }


        #region 数据导出
        /// <summary>
        /// 导出用户列表
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "导出数据")]
        public ActionResult Export(string condition, string queryJson)
        {
            Pagination pagination = new Pagination();
            queryJson = queryJson ?? "";
            pagination.p_kid = "ID";
            pagination.p_fields = @"districtname, dangersource,accidentname,deptname,jdglzrrfullname,
            case WHEN  gradeval=0 then '未定级' WHEN  gradeval>0 then Grade end as gradeStr,
            case WHEN  ishx=1 then '是' else '否' end as ishxStr,
            case WHEN  isba=1 then '是' else '否' end as isbaStr,
            case WHEN  isdjjd=1 then '是' else '否' end as isdjjdStr";
            pagination.p_tablename = "v_hsd_dangerqd_djjd t";
            pagination.conditionJson = "1=1";
            pagination.page = 1;
            pagination.sidx = "createdate";
            pagination.rows = 100000000;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                if (user.RoleName.Contains("省级"))
                {
                    pagination.conditionJson += " and (CreateUserId='" + user.UserId + "' or DeptCode in(select  encode from BASE_DEPARTMENT start with encode='" + user.NewDeptCode + "' connect by  prior  departmentid = parentid))";
                }
                else
                {
                    pagination.conditionJson += " and (CreateUserId='" + user.UserId + "' or DeptCode in(select  encode from BASE_DEPARTMENT start with encode='" + user.DeptCode + "' connect by  prior  departmentid = parentid))";
                }
                //pagination.conditionJson += " and (CreateUserId='" + user.UserId + "' or DeptCode like '" + user.DeptCode + "%')";
            }



            var watch = CommonHelper.TimerStart();
            var data = hazardsourcebll.GetPageList(pagination, queryJson);

            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "重大危险源监控登记建档";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "重大危险源监控登记建档.xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();

            listColumnEntity.Add(new ColumnEntity() { Column = "districtname", ExcelColumn = "所属区域" });
            listColumnEntity.Add(new ColumnEntity() { Column = "dangersource".ToLower(), ExcelColumn = "危险源名称/场所" });
            listColumnEntity.Add(new ColumnEntity() { Column = "accidentname".ToLower(), ExcelColumn = "可能导致的事故类型" });
            listColumnEntity.Add(new ColumnEntity() { Column = "deptname".ToLower(), ExcelColumn = "责任部门" });
            listColumnEntity.Add(new ColumnEntity() { Column = "jdglzrrfullname".ToLower(), ExcelColumn = "监督管理责任人" });
            listColumnEntity.Add(new ColumnEntity() { Column = "gradeStr".ToLower(), ExcelColumn = "是否为重大危险源" });
            listColumnEntity.Add(new ColumnEntity() { Column = "ishxStr".ToLower(), ExcelColumn = "是否核销" });
            listColumnEntity.Add(new ColumnEntity() { Column = "isbaStr".ToLower(), ExcelColumn = "是否备案" });
            listColumnEntity.Add(new ColumnEntity() { Column = "isdjjdStr".ToLower(), ExcelColumn = "是否登记建档" });
            excelconfig.ColumnEntity = listColumnEntity;

            //调用导出方法
            ExcelHelper.ExcelDownload(data, excelconfig);
            return Success("导出成功。");
        }
        #endregion


    }
}
