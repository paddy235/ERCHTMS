using BSFramework.Util;
using BSFramework.Util.Offices;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.EquipmentManage;
using ERCHTMS.Busines.RiskDatabase;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.EquipmentManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ThoughtWorks.QRCode.Codec;
using BSFramework.Data.Dapper;
using System.Configuration;
using BSFramework.Data;
using Newtonsoft.Json;

namespace ERCHTMS.Web.Areas.EquipmentManage.Controllers
{
    /// <summary>
    /// 描 述：普通设备基本信息表
    /// </summary>
    public class EquipmentController : MvcControllerBase
    {
        private EquipmentBLL equipmentbll = new EquipmentBLL();
        private ERCHTMS.Busines.PublicInfoManage.FileInfoBLL fileInfoBLL = new ERCHTMS.Busines.PublicInfoManage.FileInfoBLL();

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
        /// 导入页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }
        /// <summary>
        /// 选择设备
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult Select()
        {
            return View();
        }
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult BuilderImage()
        {
            return View();
        }

        /// <summary>
        /// 离场表单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Leave()
        {
            return View();
        }

        /// <summary>
        /// 离场列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LeaveList()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "ID";
            pagination.p_fields = @"EquipmentName,EquipmentNo,Specifications,district,districtid,districtcode,case when t.state=1 then '未启用'
when t.state=2 then '在用' when t.state=3 then '停用' when t.state=4 then '报废' when t.state=5 then '离厂' end as state,ControlDeptCode,CreateUserId,affiliation,(select count(1) from v_basehiddeninfo where workstream != '整改结束' and  deviceid=t.ID) hidnum,remark";
            pagination.p_tablename = "BIS_EQUIPMENT t";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "  1=1";
            }
            else {
                //pagination.conditionJson = string.Format(" CREATEUSERORGCODE ='{0}'", user.OrganizeCode);
                pagination.conditionJson = string.Format("  CREATEUSERORGCODE in(select  encode from BASE_DEPARTMENT start with encode='{0}' connect by  prior  departmentid = parentid)", user.OrganizeCode);
            }
            
            //if (user.IsSystem)
            //{
            //    pagination.conditionJson = "1=1";
            //}
            //else
            //{
            //    string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
            //    pagination.conditionJson = where;
            //}
           
            var watch = CommonHelper.TimerStart();
            var data = equipmentbll.GetPageList(pagination, queryJson);
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
        public ActionResult GetListJson(string queryJson)
        {
            var data = equipmentbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取隐患排查标准
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetHidStdJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "ID";
            pagination.p_fields = @"require stdname,CONTENT hiddesc,require hidmeasure";
            pagination.p_tablename = "BIS_HTSTANDARDITEM";
            pagination.conditionJson = "1=1";

            var watch = CommonHelper.TimerStart();
            var data = new HtStandardItemBLL().GetList(pagination, queryJson);
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
            var data = equipmentbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取设备编号
        /// </summary>
        /// <param name="EquipmentNo">设备类别</param>
        /// <param name="orgcode">机构编码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetEquipmentNo(string EquipmentNo, string orgcode)
        {
            return ToJsonResult(equipmentbll.GetEquipmentNo(EquipmentNo, orgcode));
        }
        /// <summary>
        /// 获取附件集合
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult GetFiles(string keyValue)
        {
            var data = fileInfoBLL.GetFiles(keyValue);
            return ToJsonResult(data);
        }

        /// <summary> 
        /// 通过设备id获取设备列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetEquipmentTable(string ids)
        {
            DataTable dt = equipmentbll.GetEquipmentTable(ids.Split(','));
            return Content(dt.ToJson());
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
            equipmentbll.RemoveForm(keyValue);
            DeleteFiles(keyValue);
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
        public ActionResult SaveForm(string keyValue, EquipmentEntity entity)
        {
            entity.EquipmentName = entity.EquipmentName.Trim();
            entity.Specifications = entity.Specifications.Trim();
            entity.RelWord = string.IsNullOrWhiteSpace(entity.RelWord) ? "" : entity.RelWord.Trim();
            entity.UseAddress = string.IsNullOrWhiteSpace(entity.UseAddress) ? "": entity.UseAddress.Trim();
            entity.FactoryNo = string.IsNullOrWhiteSpace(entity.FactoryNo) ? "" : entity.FactoryNo.Trim();
            equipmentbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }


        /// <summary>
        /// 普通设备离场
        /// </summary>
        /// <param name="leaveTime">离场时间</param>
        /// <param name="equipmentId">设备Id,多个用逗号分隔</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "普通设备离场")]
        public ActionResult Leave(string leaveTime, [System.Web.Http.FromBody]string equipmentId, [System.Web.Http.FromBody]string DepartureReason)
        {
            try
            {
                if (equipmentbll.SetLeave(equipmentId, leaveTime, DepartureReason) > 0)
                {
                    return Success("操作成功。");
                }
                else
                {
                    return Error("操作不成功。");
                }
                
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        #endregion

        #region 批量生成二维码
        /// <summary>
        /// 批量生成二维码并导出到word
        /// </summary>
        /// <param name="userId">用户Id,多个用逗号分隔</param>
        /// <param name="userName">用户姓名,多个用逗号分隔</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult BuilderImg(string equipIds, string equipNames, string equipNos,string equiptype)
        {            
            if (!System.IO.Directory.Exists(Server.MapPath("~/Resource/qrcode")))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Resource/qrcode"));
            }

            Aspose.Words.Document doc = new Aspose.Words.Document(Server.MapPath("~/Resource/ExcelTemplate/设备二维码打印.doc"));
            Aspose.Words.DocumentBuilder db = new Aspose.Words.DocumentBuilder(doc);
            DataTable dt = new DataTable("U");
            dt.Columns.Add("BigEWM2");
            dt.Columns.Add("PersonName");
            dt.Columns.Add("PersonNo");
            int i = 0;
            string fileName = "";
            string[] equipNameArr = equipNames.Split(',');
            string[] equipNoArr = equipNos.Split(',');
            foreach (string code in equipIds.Split(','))
            {
                DataRow dr = dt.NewRow();
                dr[1] = equipNameArr[i];
                dr[2] = equipNoArr[i];

                fileName = code + ".jpg";
                string filePath = Server.MapPath("~/Resource/qrcode/" + fileName);
                if (!System.IO.File.Exists(filePath))
                {
                    BuilderImg10(code, filePath, equiptype);
                }
                dr[0] = Server.MapPath("~/Resource/qrcode/" + fileName);                
                dt.Rows.Add(dr);
                i++;
            }

            doc.MailMerge.Execute(dt);
            fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
            doc.Save(Server.MapPath("~/Resource/temp/" + fileName));
            return Success("生成成功", new { fileName = fileName });
        }
        public void BuilderImg10(string keyValue, string filePath,string equiptype)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            qrCodeEncoder.QRCodeVersion = 21;
            qrCodeEncoder.QRCodeScale = 3;
            qrCodeEncoder.QRCodeForegroundColor = System.Drawing.Color.Black;         
            float size = 301, margin = 1f;
            System.Drawing.Image image = qrCodeEncoder.Encode(keyValue + "|" + equiptype, Encoding.UTF8);
            int resWidth = (int)(size + 2 * margin);
            int resHeight = (int)(size + 2 * margin);
            // 核心就是这里新建一个bitmap对象然后将image在这里渲染
            Bitmap newBit = new Bitmap(resWidth, resHeight, PixelFormat.Format32bppRgb);
            Graphics gg = Graphics.FromImage(newBit);

            // 设置背景白色
            for (int x = 0; x < resWidth; x++)
            {
                for (int y = 0; y < resHeight; y++)
                {
                    newBit.SetPixel(x, y, System.Drawing.Color.White);
                }
            }

            // 设置黑色边框
            for (int i = 0; i < resWidth; i++)
            {
                newBit.SetPixel(i, 0, System.Drawing.Color.Black);
                newBit.SetPixel(i, resHeight - 1, System.Drawing.Color.Black);

            }

            for (int j = 0; j < resHeight; j++)
            {
                newBit.SetPixel(0, j, System.Drawing.Color.Black);
                newBit.SetPixel(resWidth - 1, j, System.Drawing.Color.Black);

            }
            RectangleF desRect = new RectangleF() { X = margin, Y = margin, Width = size, Height = size };
            RectangleF srcRect = new RectangleF() { X = 0, Y = 0, Width = image.Width, Height = image.Height };
            gg.DrawImage(image, desRect, srcRect, GraphicsUnit.Pixel);    
            newBit.Save(filePath, ImageFormat.Jpeg);
            newBit.Dispose();
            image.Dispose();
        }
        #endregion

        #region 数据导出
        /// <summary>
        /// 导出用户列表
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "导出普通设备数据")]
        public ActionResult Export(string queryJson)
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "rownum idx";
            pagination.p_fields = @"EquipmentName,EquipmentNo,Specifications,district,case when t.affiliation=1 then '本单位自有' when t.affiliation=2 then '外包单位所有' end as affiliation,controldept,case when t.state=1 then '未启用' when t.state=2 then '在用' when t.state=3 then '停用' when t.state=4 then '报废' when t.state=5 then '离厂' end as useSta";
            pagination.p_tablename = "BIS_EQUIPMENT t";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                //string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
                //pagination.conditionJson = where;
                pagination.conditionJson = string.Format(" CREATEUSERORGCODE ='{0}'", user.OrganizeCode);
            }

            var watch = CommonHelper.TimerStart();
            var data = equipmentbll.GetPageList(pagination, queryJson);

            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "普通设备管理";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 16;
            excelconfig.FileName = "普通设备管理.xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity = listColumnEntity;
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "idx", ExcelColumn = "序号", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "equipmentname", ExcelColumn = "设备名称", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "equipmentno", ExcelColumn = "设备编号", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "specifications", ExcelColumn = "规格型号", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "district", ExcelColumn = "所在区域", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "affiliation", ExcelColumn = "所属关系", Alignment = "center" });            
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "controldept", ExcelColumn = "管控部门", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "usesta", ExcelColumn = "使用状况", Alignment = "center" });

            //调用导出方法
            ExcelHelper.ExcelDownload(data, excelconfig);

            return Success("导出成功。");
        }
        #endregion

        #region 导入普通设备
        /// <summary>
        /// 导入普通设备
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportEquipment()
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
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                string filePath = Server.MapPath("~/Resource/temp/" + fileName);
                file.SaveAs(filePath);
                DataTable dt = ExcelHelper.ExcelImport(filePath);
                var districtList = new DistrictBLL().GetList().Where(x => x.OrganizeId == ERCHTMS.Code.OperatorProvider.Provider.Current().OrganizeId).ToList();
                var deptList = new DepartmentBLL().GetList().Where(x => x.OrganizeId == ERCHTMS.Code.OperatorProvider.Provider.Current().OrganizeId).ToList();
                var eno = int.Parse(equipmentbll.GetEquipmentNo("P1-", ERCHTMS.Code.OperatorProvider.Provider.Current().OrganizeCode)) + 1;                
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    object[] vals = dt.Rows[i].ItemArray;
                    if (IsEndRow(vals) == true)
                        break;
                    var msg = "";
                    if (Validate(i, vals,deptList,districtList, out msg) == true)
                    {
                        var entity = GenEntity(vals, deptList, districtList, "P1-" + eno.ToString().PadLeft(4, '0'));
                        equipmentbll.SaveForm("", entity);
                        eno++;
                        sussceed++;
                    }
                    else
                    {
                        falseMessage += "第" + (i + 1) + "行" + msg + "</br>";
                        error++;
                    }
                }
                count = dt.Rows.Count;
                message = "共有" + count + "条记录,成功导入" + sussceed + "条，失败" + error + "条";
                message += "</br>" + falseMessage;
                //删除临时文件
                System.IO.File.Delete(filePath);
            }
            return message;
        }
        private bool IsEndRow(object[] vals)
        {
            bool r = false;

            r = Array.TrueForAll(vals, x => (x == null || x == DBNull.Value || x.ToString() == ""));

            return r;
        }
        private bool Validate(int index, object[] vals, List<DepartmentEntity> deptList, List<DistrictEntity> districtList, out string msg)
        {
            var r = true;
            var i = index + 1;
            msg = "";
            if (vals.Length < 6)
            {
                msg += "，格式不正确";
                r = false;
            }
            var obj = vals[0];
            if (obj == null || obj == DBNull.Value || obj.ToString().Trim() == "")
            {
                msg += "，设备名称不能为空";
                r = false;
            }
            obj = vals[1];
            if (obj == null || obj == DBNull.Value || obj.ToString().Trim() == "")
            {
                msg += "，规格型号不能为空";
                r = false;
            }           
            obj = vals[2];
            if (obj == null || obj == DBNull.Value || obj.ToString().Trim() == "")
            {
                msg += "，所属区域不能为空";
                r = false;
            }
            else
            {
                int ncount = districtList.Count(x => x.DistrictName == obj.ToString().Trim());
                if (ncount <= 0)
                {
                    msg += "，所属区域不正确";
                    r = false;
                }
            }
            obj = vals[3];
            if (obj == null || obj == DBNull.Value || obj.ToString().Trim() == "")
            {
                msg += "，管控部门不能为空";
                r = false;
            }
            else
            {
                int ncount = deptList.Count(x=>x.FullName==obj.ToString().Trim());
                if (ncount <= 0)
                {
                    msg += "，管控部门不正确";
                    r = false;
                }
            }
            //obj = vals[4];
            //if (obj == null || obj == DBNull.Value || obj.ToString().Trim() == "")
            //{                
            //    msg += "，关联词不能为空";
            //    r = false;
            //}
            obj = vals[5];
            if (obj == null || obj == DBNull.Value || obj.ToString() == "")
            {
                msg += "，使用状况不能为空";
                r = false;
            }
            else
            {
                var v = obj.ToString();
                if (v != "未启用" && v != "在用" && v != "停用" && v != "报废" && v!="离厂")
                {
                    msg += "，使用状况值不正确";
                    r = false;
                }
            }

            return r;
        }
        private EquipmentEntity GenEntity(object[] vals, List<DepartmentEntity> deptList, List<DistrictEntity> districtList,string eno)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            EquipmentEntity entity = new EquipmentEntity();
            entity.EquipmentName = vals[0].ToString().Trim();
            entity.Specifications = vals[1].ToString().Trim();            
            var obj = vals[2];
            if (obj != null && obj != DBNull.Value)
            {
                var district = districtList.FirstOrDefault(x => x.DistrictName == obj.ToString().Trim());
                if (district != null)
                {
                    entity.District = district.DistrictName;
                    entity.DistrictId = district.DistrictID;
                    entity.DistrictCode = district.DistrictCode;
                }
            }
            obj = vals[3];
            if (obj != null && obj != DBNull.Value)
            {
                var dept = deptList.FirstOrDefault(x => x.FullName == obj.ToString().Trim());
                if (dept != null)
                {
                    entity.ControlDept = dept.FullName;
                    entity.ControlDeptCode = dept.EnCode;
                    entity.ControlDeptID = dept.DepartmentId;
                }
            }
            entity.RelWord = vals[4].ToString().Trim();
            entity.State = "1";
            var state = vals[5].ToString();
            if (state == "在用")
                entity.State = "2";
            else if (state == "停用")
                entity.State = "3";
            else if (state == "报废")
                entity.State = "4";
            else if (state == "离厂")
                entity.State = "5";

            entity.Affiliation = "1";
            entity.EquipmentNo = eno;

            return entity;
        }
        #endregion


        /// <summary>
        /// 调用远程的设备信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpGet]
        public object GetRemoteEquipmentList(string parentId)
        {
            try
            {
                ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
                fileMap.ExeConfigFilename = System.Web.HttpContext.Current.Server.MapPath(@"~/XmlConfig/remote_database.config");
                Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                AppSettingsSection appsection = (AppSettingsSection)config.GetSection("appSettings");
                string connectstring = appsection.Settings["RemoteBaseDb"].Value;
                IDatabase dbBase = new OracleDatabase(connectstring);
                string strWhere = " 1=1";
               
                if (!string.IsNullOrEmpty(parentId))
                {
                    strWhere += string.Format(" and parentid ='{0}' ");
                }
                else 
                {
                    strWhere += string.Format(" and id ='-1' "); //根
                }
                string sql = string.Format("select facname,faccode,parentid, id from FACTBFACILITY t where {0} order by faccode",strWhere);

                DataTable dt = dbBase.FindTable(sql);

                dbBase.Close();
           
                return new { code = 0, info = "获取数据成功", count = dt.Rows.Count, data = dt };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }
        }


        /// <summary>
        /// 调用远程的设备信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpGet]
        public object GetRemoteEquipmentJson(int checkmode = 0, string facname = "",string nodeid="" ,int level =0)   
        {
            try
            {

                var treeList = new List<TreeEntity>();

                ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();

                fileMap.ExeConfigFilename = System.Web.HttpContext.Current.Server.MapPath(@"~/XmlConfig/remote_database.config");

                Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

                AppSettingsSection appsection = (AppSettingsSection)config.GetSection("appSettings");

                string connectstring = appsection.Settings["RemoteBaseDb"].Value;

                IDatabase dbBase = new OracleDatabase(connectstring);

                string strWhere = " 1=1";

                if (!string.IsNullOrEmpty(nodeid))
                {
                    level += 1;
                    strWhere += string.Format(@" and  parentid = '{0}' ", nodeid);
                }
                else 
                {
                    strWhere += @" and  parentid = '-1' ";
                }
                if (!string.IsNullOrEmpty(facname)) 
                {
                    strWhere += string.Format(@" and facname like '%{0}%'", facname);
                }

                string sql = string.Format("select createdate, facname,faccode,parentid, id from factbfacility t where {0} order by faccode", strWhere);

                DataTable dt = dbBase.FindTable(sql);

                dt.Columns.Add("level", typeof(Int32));
                dt.Columns.Add("isLeaf", typeof(bool));
                dt.Columns.Add("expanded", typeof(bool));

               
                DataTable clonedt = dt.Clone();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        item["level"] = level;
                        DataRow[] itemDt = dt.Select(string.Format(" parentid ='{0}'", item["id"].ToString()));
                        item["isLeaf"] = itemDt.Length > 0;
                        item["expanded"] = false;
                        DataRow crow = clonedt.NewRow();
                        crow["facname"] = item["facname"];
                        crow["faccode"] = item["faccode"];
                        crow["createdate"] = item["createdate"];
                        crow["level"] = item["level"];
                        crow["parentid"] = item["parentid"];
                        crow["isLeaf"] = item["isLeaf"];
                        crow["id"] = item["id"];
                        clonedt.Rows.Add(crow);
                    }
                }
                var jsonData = new
                {
                    rows = clonedt,
                    total = clonedt.Rows.Count,
                    page =1,
                    records = clonedt.Rows.Count,

                };
                return ToJsonResult(jsonData);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }


       /// <summary>
        /// 调用远程的设备信息
       /// </summary>
       /// <param name="lv"></param>
       /// <param name="id"></param>
       /// <param name="pId"></param>
       /// <returns></returns>
        [HttpGet]
        public object GetRemoteEquipmentTreeJson(string lv,string id ="-1",string pId ="-1") 
        {
            try
            {

                var treeList = new List<TreeEntity>();

                ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();

                fileMap.ExeConfigFilename = System.Web.HttpContext.Current.Server.MapPath(@"~/XmlConfig/remote_database.config");

                Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

                AppSettingsSection appsection = (AppSettingsSection)config.GetSection("appSettings");

                string connectstring = appsection.Settings["RemoteBaseDb"].Value;

                IDatabase dbBase = new OracleDatabase(connectstring);

                string strWhere = " 1=1";

                if (!string.IsNullOrEmpty(id))
                {
                    strWhere += string.Format(@" and  parentid = '{0}' ", id);
                }
                string sql = string.Format("select  facname t ,  facname name ,parentid  pId, id,faccode code from factbfacility t where {0} order by faccode", strWhere);

                DataTable dt = dbBase.FindTable(sql);
                dt.Columns.Add("click", typeof(bool));
                dt.Columns.Add("isParent", typeof(bool));
                DataTable clonedt = dt.Clone();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        var childDt = dbBase.FindTable(string.Format("select  facname t ,  facname name ,parentid  pId, id from factbfacility t where parentid ='{0}' ", item["id"].ToString()));
                        item["isParent"] = childDt.Rows.Count > 0;
                        item["click"] = true;
                        DataRow crow = clonedt.NewRow();
                        crow["code"] = item["code"];
                        crow["name"] = item["name"];
                        crow["pId"] = item["pId"];
                        crow["id"] = item["id"];
                        crow["t"] = item["t"];
                        crow["click"] = item["click"];
                        crow["isParent"] = item["isParent"];
                        clonedt.Rows.Add(crow);
                    }
                }
                return ToJsonResult(clonedt); 
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
    }
}
