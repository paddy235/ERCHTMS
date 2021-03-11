using ERCHTMS.Entity.FireManage;
using ERCHTMS.Busines.FireManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ERCHTMS.Busines.SystemManage;
using System.Linq;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using System.Web;
using System;
using System.Data;
using ERCHTMS.Cache;
using ERCHTMS.Busines.BaseManage;
using ThoughtWorks.QRCode.Codec;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Text.RegularExpressions;

namespace ERCHTMS.Web.Areas.FireManage.Controllers
{
    /// <summary>
    /// 描 述：重点防火部位
    /// </summary>
    public class KeyPartController : MvcControllerBase
    {
        private KeyPartBLL keypartbll = new KeyPartBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private readonly DepartmentBLL departBLL = new DepartmentBLL();
        private readonly UserBLL userBLL = new UserBLL();
        private readonly OrganizeBLL orgBLL = new OrganizeBLL();

        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.ehsDepartCode = "";
            //当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            DataItemModel ehsDepart = dataitemdetailbll.GetDataItemListByItemCode("'EHSDepartment'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();
            if (ehsDepart != null)
                ViewBag.ehsDepartCode = ehsDepart.ItemValue;
            return View();
        }

        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GenericIndex()
        {
            ViewBag.ehsDepartCode = "";
            //当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            DataItemModel ehsDepart = dataitemdetailbll.GetDataItemListByItemCode("'EHSDepartment'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();
            if (ehsDepart != null)
                ViewBag.ehsDepartCode = ehsDepart.ItemValue;
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
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GenericForm()
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
        #endregion

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
        /// 导入页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GenericImport()
        {
            return View();
        }
        

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
            queryJson = queryJson ?? "";
            pagination.p_kid = "Id";
            pagination.p_fields = "PartName,District,DutyUser,Dutydept,Rank,NextPatrolDate,EmployState,createuserid,createuserdeptcode,createuserorgcode,DutyUserId";
            pagination.p_tablename = "HRS_KEYPART";
            pagination.conditionJson = "1=1";
            var watch = CommonHelper.TimerStart();
            var data = keypartbll.GetPageList(pagination, queryJson);
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
            var data = keypartbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = keypartbll.GetEntity(keyValue);
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
            keypartbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, KeyPartEntity entity)
        {
            keypartbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion

        #region 数据导出
        /// <summary>
        /// 批量生成二维码并导出到word
        /// </summary>
        /// <param name="Ids"></param>
        /// <param name="partNames"></param>
        /// <param name="dutyDepts"></param>
        /// <param name="dutyUsers"></param>
        /// <param name="equiptype"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult BuilderImg(string Ids, string partNames, string dutyDepts, string dutyUsers, string equiptype)
        {
            if (!System.IO.Directory.Exists(Server.MapPath("~/Resource/qrcode")))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Resource/qrcode"));
            }

            Aspose.Words.Document doc = new Aspose.Words.Document(Server.MapPath("~/Resource/ExcelTemplate/重点防火部位二维码打印.doc"));
            Aspose.Words.DocumentBuilder db = new Aspose.Words.DocumentBuilder(doc);
            DataTable dt = new DataTable("U");
            dt.Columns.Add("BigEWM2");
            dt.Columns.Add("PartName");
            dt.Columns.Add("DutyDept");
            dt.Columns.Add("DutyUser");
            int i = 0;
            string fileName = "";
            string[] partNameArr = partNames.Split(',');
            string[] dutyDeptArr = dutyDepts.Split(',');
            string[] dutyUserArr = dutyUsers.Split(',');
            foreach (string code in Ids.Split(','))
            {
                DataRow dr = dt.NewRow();
                dr[1] = partNameArr[i];
                dr[2] = dutyDeptArr[i];
                dr[3] = dutyUserArr[i];

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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="filePath"></param>
        /// <param name="equiptype"></param>
        public void BuilderImg10(string keyValue, string filePath, string equiptype)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder
            {
                QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE,
                QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M,
                QRCodeVersion = 21,
                QRCodeScale = 3,
                QRCodeForegroundColor = System.Drawing.Color.Black
            };
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
        /// <summary>
        /// 导出日常巡查
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "导出日常巡查清单")]
        public ActionResult Export(string queryJson)
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "ID";
            pagination.p_fields = @"PartName,District,Dutydept,DutyUser,to_char(NextPatrolDate,'yyyy-MM-dd') as NextPatrolDate,
 case when EmployState = '0' then '在用' when EmployState = '1' then '停用' when EmployState = '2' then '其他' else '' end as EmployState,
 case when Rank = '1' then '一级动火区域' when Rank = '2' then '二级动火区域' else '' end as Rank";
            pagination.p_tablename = "HRS_KEYPART";
            pagination.conditionJson = "1=1";
            var watch = CommonHelper.TimerStart();
            var data = keypartbll.GetPageList(pagination, queryJson);

            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "重点防火部位";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "重点防火部位.xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();

            listColumnEntity.Add(new ColumnEntity() { Column = "partname", ExcelColumn = "重点防火部位名称", Alignment = "center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "district", ExcelColumn = "区域", Alignment = "center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "dutydept", ExcelColumn = "责任部门", Alignment = "center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "dutyuser", ExcelColumn = "责任人", Alignment = "center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "nextpatroldate", ExcelColumn = "下次巡查日期", Alignment = "center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "employstate", ExcelColumn = "使用状态", Alignment = "center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "rank", ExcelColumn = "动火级别", Alignment = "center" });

            excelconfig.ColumnEntity = listColumnEntity;
            //调用导出方法
            //ExcelHelper.ExcelDownload(data, excelconfig);
            //调用导出方法
            ExcelHelper.ExportByAspose(data, excelconfig.FileName, listColumnEntity);
            return Success("导出成功。");
        }
        #endregion

        #region 数据导入
        /// <summary>
        /// 导入重点防火部位
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        //[AjaxOnly]
        //[HandlerAuthorize(PermissionMode.Ignore)]
        // [ValidateAntiForgeryToken]
        public string ImportFirefighting()
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
                DataTable dt = ExcelHelper.ExcelImport(Server.MapPath("~/Resource/temp/" + fileName));
                int order = 1;
                string orgid = OperatorProvider.Provider.Current().OrganizeId;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    KeyPartEntity item = new KeyPartEntity();
                    order = i + 1;
                    #region 重点防火部位名称
                    string partname = dt.Rows[i][0].ToString();
                    if (!string.IsNullOrEmpty(partname))
                    {
                        item.PartName = partname;
                        var data = new DataItemCache().ToItemValue("PartName", partname);
                        if (data != null && !string.IsNullOrEmpty(data))
                            item.PartNo = data;
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,重点防火部位名称不存在！</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"第{0}行导入失败,重点防火部位名称不能为空！</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region 所在位置
                    string district = dt.Rows[i][1].ToString();
                    if (!string.IsNullOrEmpty(district))
                    {
                        var disItem = new DistrictBLL().GetListForCon(x => x.DistrictName == district && x.OrganizeId == orgid).FirstOrDefault();
                        if (disItem != null)
                        {
                            item.DistrictId = disItem.DistrictID;
                            item.DistrictCode = disItem.DistrictCode;
                            item.District = district;
                        }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,所在位置不存在！</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"第{0}行导入失败,所在位置不能为空！</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region 责任部门
                    string dutydept = dt.Rows[i][2].ToString();
                    if (!string.IsNullOrEmpty(dutydept))
                    {
                        var data = departBLL.GetList().FirstOrDefault(e => e.FullName == dutydept && e.OrganizeId == orgid);
                        if (data != null)
                        {
                            item.DutyDept = dutydept;
                            item.DutyDeptCode = data.EnCode;
                        }

                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,责任部门不存在！</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"第{0}行导入失败,责任部门不能为空！</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region 责任人
                    string dutyUser = dt.Rows[i][3].ToString();
                    if (!string.IsNullOrEmpty(dutyUser))
                    {
                        var userEntity = userBLL.GetListForCon(x => x.RealName == dutyUser && x.OrganizeId == orgid).FirstOrDefault();
                        if (userEntity != null)
                        {
                            item.DutyUserId = userEntity.UserId;
                            item.DutyUser = dutyUser;
                        }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,责任人不存在！</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"第{0}行导入失败,责任人不能为空！</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region 责任人电话
                    string dutyTel = dt.Rows[i][4].ToString();
                    if (!string.IsNullOrEmpty(dutyTel))
                    {
                        item.DutyTel = dutyTel;
                    }
                    #endregion

                    #region 建筑结构
                    string structure = dt.Rows[i][5].ToString();
                    if (!string.IsNullOrEmpty(structure))
                    {
                        item.Structure = structure;
                        var data = new DataItemCache().ToItemValue("Structure", structure);
                        if (data != null && !string.IsNullOrEmpty(data))
                            item.Structure = data;
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,建筑结构不存在！</br>", order);
                            error++;
                            continue;
                        }
                    }
                    //else
                    //{
                    //    falseMessage += string.Format(@"第{0}行导入失败,建筑结构不能为空！</br>", order);
                    //    error++;
                    //    continue;
                    //}
                    #endregion

                    #region 建筑面积（m2）
                    string acreage = dt.Rows[i][6].ToString();
                    int tempAcreage;
                    if (!string.IsNullOrEmpty(acreage))
                    {
                        if (int.TryParse(acreage, out tempAcreage))
                            item.Acreage = tempAcreage;
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,建筑面积（m2）必须为数字！</br>", order);
                            error++;
                            continue;
                        }
                    }
                    //else
                    //{
                    //    falseMessage += string.Format(@"第{0}行导入失败,建筑面积（m2）不能为空！</br>", order);
                    //    error++;
                    //    continue;
                    //}
                    #endregion

                    #region 主要存储物品
                    string storegoods = dt.Rows[i][7].ToString();
                    if (!string.IsNullOrEmpty(storegoods))
                    {
                        item.StoreGoods = storegoods;
                    }
                    #endregion

                    #region 主要灭火器装备
                    string outfireequip = dt.Rows[i][8].ToString();
                    if (!string.IsNullOrEmpty(outfireequip))
                    {
                        item.OutfireEquip = outfireequip;
                    }
                    else
                    {
                        falseMessage += string.Format(@"第{0}行导入失败,主要灭火器装备不能为空！</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region 重点防火部位人数
                    string peoplenum = dt.Rows[i][9].ToString();
                    int tempPeopleNum;
                    if (!string.IsNullOrEmpty(peoplenum))
                    {
                        if (int.TryParse(peoplenum, out tempPeopleNum))
                            item.PeopleNum = tempPeopleNum;
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,重点防火部位人数必须为数字！</br>", order);
                            error++;
                            continue;
                        }
                    }
                    #endregion

                    #region 动火级别
                    string rank = dt.Rows[i][10].ToString();
                    if (!string.IsNullOrEmpty(rank))
                    {
                        if (rank == "一级动火区域") item.Rank = 1;
                        else if (rank == "二级动火区域") item.Rank = 2;
                        else {
                            falseMessage += string.Format(@"第{0}行导入失败,动火级别不存在！</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"第{0}行导入失败,动火级别不能为空！</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region 最近巡查日期（年月日）
                    string latelypatroldate = dt.Rows[i][11].ToString();
                    DateTime tempLatelyPatrolDate;
                    if (!string.IsNullOrEmpty(latelypatroldate))
                        if (DateTime.TryParse(latelypatroldate, out tempLatelyPatrolDate))
                            item.LatelyPatrolDate = tempLatelyPatrolDate;
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,最近巡查日期不对！</br>", order);
                            error++;
                            continue;
                        }
                    #endregion

                    #region 巡查周期（天）
                    string patrolperiod = dt.Rows[i][12].ToString();
                    int tempPatrolPeriod;
                    if (!string.IsNullOrEmpty(patrolperiod))
                    {
                        if (int.TryParse(patrolperiod, out tempPatrolPeriod))
                            item.PatrolPeriod = tempPatrolPeriod;
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,巡查周期（天）必须为数字！</br>", order);
                            error++;
                            continue;
                        }
                    }
                    #endregion

                    #region 下次巡查日期
                    string nextpatroldate = dt.Rows[i][13].ToString();
                    DateTime tempNextPatrolDate;
                    if (!string.IsNullOrEmpty(nextpatroldate))
                        if (DateTime.TryParse(nextpatroldate, out tempNextPatrolDate))
                            item.NextPatrolDate = tempNextPatrolDate;
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,下次巡查日期不对！</br>", order);
                            error++;
                            continue;
                        }
                    else
                    {
                        falseMessage += string.Format(@"第{0}行导入失败,下次巡查日期不能为空！</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region 使用状态
                    string employstate = dt.Rows[i][14].ToString();
                    if (!string.IsNullOrEmpty(employstate))
                    {
                        if (employstate == "在用") item.EmployState = 0;
                        else if (employstate == "停用") item.EmployState = 1;
                        else if (employstate == "其他") item.EmployState = 2;
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,使用状态不存在！</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"第{0}行导入失败,使用状态不能为空！</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region 火灾危险性分析
                    string analyze = dt.Rows[i][15].ToString();
                    if (!string.IsNullOrEmpty(analyze))
                    {
                        item.Analyze = analyze;
                    }
                    else
                    {
                        falseMessage += string.Format(@"第{0}行导入失败,火灾危险性分析不能为空！</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region 防火措施
                    string measure = dt.Rows[i][16].ToString();
                    if (!string.IsNullOrEmpty(measure))
                    {
                        item.Measure = measure;
                    }
                    else
                    {
                        falseMessage += string.Format(@"第{0}行导入失败,防火措施不能为空！</br>", order);
                        error++;
                        continue;
                    }
                    #endregion
                    //管理要求
                    string require = dt.Rows[i][17].ToString();
                    if (!string.IsNullOrEmpty(require))
                    {
                        item.Require = require;
                    }
                    //备注
                    string remark = dt.Rows[i][18].ToString();
                    if (!string.IsNullOrEmpty(remark))
                    {
                        item.Remark = remark;
                    }

                    try
                    {
                        keypartbll.SaveForm("", item);
                    }
                    catch
                    {
                        error++;
                    }

                }
                count = dt.Rows.Count;
                message = "共有" + count + "条记录,成功导入" + (count - error) + "条，失败" + error + "条";
                message += "</br>" + falseMessage;
            }

            return message;
        }


        /// <summary>
        /// 导入重点防火部位
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        //[AjaxOnly]
        //[HandlerAuthorize(PermissionMode.Ignore)]
        // [ValidateAntiForgeryToken]
        public string ImportGenericFirefighting()
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
                DataTable dt = ExcelHelper.ExcelImport(Server.MapPath("~/Resource/temp/" + fileName));
                int order = 1;
                string orgid = OperatorProvider.Provider.Current().OrganizeId;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    KeyPartEntity item = new KeyPartEntity();
                    order = i + 1;
                    #region 重点防火部位名称
                    string partname = dt.Rows[i][0].ToString();
                    if (!string.IsNullOrEmpty(partname))
                    {
                        item.PartName = partname;
                        item.PartNo = partname;
                        //var data = new DataItemCache().ToItemValue("PartName", partname);
                        //if (data != null && !string.IsNullOrEmpty(data))
                        //    item.PartNo = data;
                        //else
                        //{
                        //    falseMessage += string.Format(@"第{0}行导入失败,重点防火部位名称不存在！</br>", order);
                        //    error++;
                        //    continue;
                        //}
                    }
                    else
                    {
                        falseMessage += string.Format(@"第{0}行导入失败,重点防火部位名称不能为空！</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region 所在位置
                    string district = dt.Rows[i][1].ToString();
                    if (!string.IsNullOrEmpty(district))
                    {
                        var disItem = new DistrictBLL().GetListForCon(x => x.DistrictName == district && x.OrganizeId == orgid).FirstOrDefault();
                        if (disItem != null)
                        {
                            item.DistrictId = disItem.DistrictID;
                            item.DistrictCode = disItem.DistrictCode;
                            item.District = district;
                        }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,所在位置不存在！</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"第{0}行导入失败,所在位置不能为空！</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region 责任部门
                    string dutydept = dt.Rows[i][2].ToString();
                    if (!string.IsNullOrEmpty(dutydept))
                    {
                        var data = departBLL.GetList().FirstOrDefault(e => e.FullName == dutydept && e.OrganizeId == orgid);
                        if (data != null)
                        {
                            item.DutyDept = dutydept;
                            item.DutyDeptCode = data.EnCode;
                        }

                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,责任部门不存在！</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"第{0}行导入失败,责任部门不能为空！</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region 责任人
                    string dutyUser = dt.Rows[i][3].ToString();
                    if (!string.IsNullOrEmpty(dutyUser))
                    {
                        var userEntity = userBLL.GetListForCon(x => x.RealName == dutyUser && x.OrganizeId == orgid).FirstOrDefault();
                        if (userEntity != null)
                        {
                            item.DutyUserId = userEntity.UserId;
                            item.DutyUser = dutyUser;
                        }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,责任人不存在！</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"第{0}行导入失败,责任人不能为空！</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region 责任人电话
                    string dutyTel = dt.Rows[i][4].ToString();
                    if (!string.IsNullOrEmpty(dutyTel))
                    {
                        item.DutyTel = dutyTel;
                    }
                    #endregion

                    #region 建筑结构
                    string structure = dt.Rows[i][5].ToString();
                    if (!string.IsNullOrEmpty(structure))
                    {
                        item.Structure = structure;
                        var data = new DataItemCache().ToItemValue("Structure", structure);
                        if (data != null && !string.IsNullOrEmpty(data))
                            item.Structure = data;
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,建筑结构不存在！</br>", order);
                            error++;
                            continue;
                        }
                    }
                    //else
                    //{
                    //    falseMessage += string.Format(@"第{0}行导入失败,建筑结构不能为空！</br>", order);
                    //    error++;
                    //    continue;
                    //}
                    #endregion

                    #region 建筑面积（m2）
                    string acreage = dt.Rows[i][6].ToString();
                    int tempAcreage;
                    if (!string.IsNullOrEmpty(acreage))
                    {
                        if (int.TryParse(acreage, out tempAcreage))
                            item.Acreage = tempAcreage;
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,建筑面积（m2）必须为数字！</br>", order);
                            error++;
                            continue;
                        }
                    }
                    //else
                    //{
                    //    falseMessage += string.Format(@"第{0}行导入失败,建筑面积（m2）不能为空！</br>", order);
                    //    error++;
                    //    continue;
                    //}
                    #endregion

                    #region 主要存储物品
                    string storegoods = dt.Rows[i][7].ToString();
                    if (!string.IsNullOrEmpty(storegoods))
                    {
                        item.StoreGoods = storegoods;
                    }
                    #endregion

                    #region 主要灭火器装备
                    string outfireequip = dt.Rows[i][8].ToString();
                    if (!string.IsNullOrEmpty(outfireequip))
                    {
                        item.OutfireEquip = outfireequip;
                    }
                    else
                    {
                        falseMessage += string.Format(@"第{0}行导入失败,主要灭火器装备不能为空！</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region 重点防火部位人数
                    string peoplenum = dt.Rows[i][9].ToString();
                    int tempPeopleNum;
                    if (!string.IsNullOrEmpty(peoplenum))
                    {
                        if (int.TryParse(peoplenum, out tempPeopleNum))
                            item.PeopleNum = tempPeopleNum;
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,重点防火部位人数必须为数字！</br>", order);
                            error++;
                            continue;
                        }
                    }
                    #endregion

                    #region 动火级别
                    string rank = dt.Rows[i][10].ToString();
                    if (!string.IsNullOrEmpty(rank))
                    {
                        if (rank == "一级动火区域") item.Rank = 1;
                        else if (rank == "二级动火区域") item.Rank = 2;
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,动火级别不存在！</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"第{0}行导入失败,动火级别不能为空！</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region 最近巡查日期（年月日）
                    string latelypatroldate = dt.Rows[i][11].ToString();
                    DateTime tempLatelyPatrolDate;
                    if (!string.IsNullOrEmpty(latelypatroldate))
                        if (DateTime.TryParse(latelypatroldate, out tempLatelyPatrolDate))
                            item.LatelyPatrolDate = tempLatelyPatrolDate;
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,最近巡查日期不对！</br>", order);
                            error++;
                            continue;
                        }
                    #endregion

                    #region 巡查周期（天）
                    string patrolperiod = dt.Rows[i][12].ToString();
                    int tempPatrolPeriod;
                    if (!string.IsNullOrEmpty(patrolperiod))
                    {
                        if (int.TryParse(patrolperiod, out tempPatrolPeriod))
                            item.PatrolPeriod = tempPatrolPeriod;
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,巡查周期（天）必须为数字！</br>", order);
                            error++;
                            continue;
                        }
                    }
                    #endregion

                    #region 下次巡查日期
                    string nextpatroldate = dt.Rows[i][13].ToString();
                    DateTime tempNextPatrolDate;
                    if (!string.IsNullOrEmpty(nextpatroldate))
                        if (DateTime.TryParse(nextpatroldate, out tempNextPatrolDate))
                            item.NextPatrolDate = tempNextPatrolDate;
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,下次巡查日期不对！</br>", order);
                            error++;
                            continue;
                        }
                    else
                    {
                        falseMessage += string.Format(@"第{0}行导入失败,下次巡查日期不能为空！</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region 使用状态
                    string employstate = dt.Rows[i][14].ToString();
                    if (!string.IsNullOrEmpty(employstate))
                    {
                        if (employstate == "在用") item.EmployState = 0;
                        else if (employstate == "停用") item.EmployState = 1;
                        else if (employstate == "其他") item.EmployState = 2;
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,使用状态不存在！</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"第{0}行导入失败,使用状态不能为空！</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region 火灾危险性分析
                    string analyze = dt.Rows[i][15].ToString();
                    if (!string.IsNullOrEmpty(analyze))
                    {
                        item.Analyze = analyze;
                    }
                    else
                    {
                        falseMessage += string.Format(@"第{0}行导入失败,火灾危险性分析不能为空！</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region 防火措施
                    string measure = dt.Rows[i][16].ToString();
                    if (!string.IsNullOrEmpty(measure))
                    {
                        item.Measure = measure;
                    }
                    else
                    {
                        falseMessage += string.Format(@"第{0}行导入失败,防火措施不能为空！</br>", order);
                        error++;
                        continue;
                    }
                    #endregion
                    //管理要求
                    string require = dt.Rows[i][17].ToString();
                    if (!string.IsNullOrEmpty(require))
                    {
                        item.Require = require;
                    }
                    //备注
                    string remark = dt.Rows[i][18].ToString();
                    if (!string.IsNullOrEmpty(remark))
                    {
                        item.Remark = remark;
                    }

                    try
                    {
                        keypartbll.SaveForm("", item);
                    }
                    catch
                    {
                        error++;
                    }

                }
                count = dt.Rows.Count;
                message = "共有" + count + "条记录,成功导入" + (count - error) + "条，失败" + error + "条";
                message += "</br>" + falseMessage;
            }

            return message;
        }
        #endregion
    }
}
