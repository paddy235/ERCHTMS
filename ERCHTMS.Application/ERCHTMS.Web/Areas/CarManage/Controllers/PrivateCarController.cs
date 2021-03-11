using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Busines.CarManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using BSFramework.Util.Offices;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.SystemManage.ViewModel;

namespace ERCHTMS.Web.Areas.CarManage.Controllers
{
    public class PrivateCarController : MvcControllerBase
    {
        private CarinfoBLL carinfobll = new CarinfoBLL();

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
        public ActionResult ImgView()
        {
            return View();
        }
        /// <summary>
        /// 私家车审批
        /// </summary>
        /// <returns></returns>
        public ActionResult ExamineForm()
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
        #endregion

        #region 获取数据

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            string orgcode = OperatorProvider.Provider.Current().OrganizeCode;
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "ID";
            pagination.p_fields = "info.state,info.carno,info.createdate,info.dirver,info.phone,info.deptname,info.driverlicenseurl,info.drivinglicenseurl,info.model,info.nextinsperctiondate,NVL(zvi.num,0) as znum,NVL(vi.num,0) as num,info.currentgname,info.STARTTIME,info.ENDTIME";
            pagination.p_tablename = @"bis_carinfo info
      left join	(SELECT COUNT(vi.CARDNO) as num, vi.CARDNO FROM  BIS_CARVIOLATION  vi   group by CARDNO)
						zvi on info.CARNO=zvi.CARDNO
					 left join	(SELECT COUNT(vi.CARDNO) as num, vi.CARDNO FROM  BIS_CARVIOLATION  vi   where ISPROCESS=0 group by CARDNO)
						vi on info.CARNO=vi.CARDNO
            ";
            pagination.conditionJson = " 1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.conditionJson = "1=1";

            var data = carinfobll.GetPageList(pagination, queryJson);
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch),

            };
            return Content(JsonData.ToJson());
        }

        /// <summary>
        /// 获取明年年检时间
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public string GetTimeString(string time)
        {
            if (time.Trim() != "")
            {
                return Convert.ToDateTime(time).AddDays(-1).AddYears(1).ToString("yyyy-MM-dd");
            }
            else
            {
                return "";
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
            var data = carinfobll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #endregion

        #region 提交数据

        /// <summary>
        /// 把客户清单转成sql导入
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportData()
        {

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
                DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();
                List<DataItemModel> data = dataItemDetailBLL.GetDataItemListByItemCode("'CarNo'").ToList();

                UserBLL userbll = new UserBLL();
                DepartmentBLL deptbll = new DepartmentBLL();
                var userlist = userbll.GetList();
                var deptList = deptbll.GetList();
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    //车牌号
                    string CarNo = dt.Rows[i][2].ToString().Trim();
                    //内部编号
                    string Model = dt.Rows[i][1].ToString();
                    //驾驶人
                    string Dirver = dt.Rows[i][3].ToString();
                    //驾驶人电话
                    string Phone = dt.Rows[i][4].ToString();
                    if (CarNo.Trim() == "")
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "车牌号为空,未能导入.";
                        error++;
                        continue;
                    }

                    CarNo = CarNo.Trim().ToUpper();//英文转换为大写


                    string s = CarNo.Substring(0, 1);
                    bool flag = false;
                    foreach (var d in data)
                    {
                        if (d.ItemName == s)
                        {
                            flag = true;
                            break;
                        }
                    }

                    if (flag == false)
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "车牌号输入格式错误,第一位请输入正确的省缩写,未能导入.";
                        error++;
                        continue;
                    }

                    //判断车牌号位数是否合法
                    if (!Regex.IsMatch(CarNo.Trim(), "(^[京津沪渝冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤青藏川宁琼使领A-Z]{1}[A-Z]{1}[A-Z0-9]{4}[A-Z0-9挂学警港澳]{1}$)"))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "车牌号填写错误,未能导入.";
                        error++;
                        continue;
                    }

                    if (Model.Trim() == "")
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "内部编号为空,未能导入.";
                        error++;
                        continue;
                    }
                    if (Dirver.Trim() == "")
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "驾驶人为空,未能导入.";
                        error++;
                        continue;
                    }
                    if (Phone.Trim() == "")
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "驾驶人电话为空,未能导入.";
                        error++;
                        continue;
                    }

                    var user = userlist.Where(it => it.Account == Phone).FirstOrDefault();
                    if (user != null)
                    {

                        DateTime Stime = Convert.ToDateTime("2020-09-22 00:00:00");
                        DateTime Etime = Convert.ToDateTime("2024-09-22 23:59:59");

                        CarinfoEntity hf = new CarinfoEntity();
                        hf.CarNo = CarNo.Trim();
                        hf.GpsId = "";
                        hf.GpsName = "";
                        //hf.Dirver = Dirver;
                        hf.State = "1";
                        hf.Remark = "";
                        hf.CreateUserId = user.UserId;
                        hf.Model = Model;
                        hf.Dirver = user.RealName;
                        hf.DirverId = user.UserId;
                        hf.Phone = user.Mobile;
                        var dept = deptList.Where(it => it.DepartmentId == user.DepartmentId).FirstOrDefault();
                        string deptname = "";
                        if (dept != null)
                        {
                            deptname = dept.FullName;
                        }
                        hf.Deptname = deptname;
                        hf.IsAuthorized = 0;
                        hf.Endtime = Etime;
                        hf.Starttime = Stime;
                        hf.Type = 1;


                        try
                        {
                            string sql = string.Format(@"insert into bis_carinfo (ID, CREATEUSERID, CREATEDATE, MODIFYUSERID, MODIFYDATE, CREATEUSERDEPTCODE, CREATEUSERORGCODE, CARNO, DIRVER, PHONE, INSPERCTIONDATE, NEXTINSPERCTIONDATE, TYPE, NUMBERLIMIT, DRIVERLICENSEURL, DRIVINGLICENSEURL, MODEL, GPSID, GPSNAME, ISAUTHORIZED, AUTHUSERID, AUTHUSERNAME, STARTTIME, ENDTIME, STATE, CURRENTGNAME, CURRENTGID, REMARK, ISENABLE, DIRVERID, DEPTNAME)
values ('{0}', '{1}', '{2}', null, null, '{3}', '{4}', '{5}', '{6}', '{7}', null, null, {8}, null, null, null, '{9}', '', '', '{10}', '', '', '{11}', '{12}', '{13}', '1号岗', '2849d7a3cfbe4b64b35c9553f3861f10', '', 0, '{14}', '{15}');", Guid.NewGuid().ToString(), hf.CreateUserId, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), user.DepartmentCode, user.OrganizeCode, hf.CarNo, hf.Dirver, hf.Phone, hf.Type, hf.Model, hf.IsAuthorized, hf.Starttime, hf.Endtime, hf.State, hf.DirverId, hf.Deptname);
                            string LogName = "PrivateCar.txt";
                            string Url = "D:\\sql";
                            if (!System.IO.Directory.Exists(Url))
                            {
                                System.IO.Directory.CreateDirectory(Url);
                            }
                            System.IO.File.AppendAllText(Url + "\\" + LogName, sql + "\r\n");

                        }
                        catch
                        {
                            error++;
                        }
                    }
                    else
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行为找到对应人员，未能导入.";
                        error++;
                        continue;
                    }



                }
                count = dt.Rows.Count;
                message = "共有" + count + "条记录,成功导入" + (count - error) + "条，失败" + error + "条";
                message += "</br>" + falseMessage;
            }

            return message;
        }

        /// <summary>
        /// 上传驾驶证/行驶证
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadFile(string type)
        {
            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            //没有文件上传，直接返回
            if (files[0].ContentLength == 0 || string.IsNullOrEmpty(files[0].FileName))
            {
                return HttpNotFound();
            }
            string virtualPath = "";
            string UserId = OperatorProvider.Provider.Current().UserId;

            DataItemDetailBLL dd = new DataItemDetailBLL();
            string path = dd.GetItemValue("imgPath") + "\\Resource\\" + type;

            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            var file = files[0];
            string ext = System.IO.Path.GetExtension(file.FileName);
            string fileName = Guid.NewGuid().ToString() + ".png";
            file.SaveAs(path + "\\" + fileName);
            //if (GetPicThumbnail(path + "\\" + fileName, path + "\\s" + fileName, 40, 100, 20))
            //{
            //    virtualPath = "/Resource/" + type + "/s" + fileName;
            //}
            //else
            //{
            virtualPath = "/Resource/" + type + "/" + fileName;
            //}
            return Success("上传成功。", virtualPath);



        }

        /// 无损压缩图片  
        /// <param name="sFile">原图片</param>  
        /// <param name="dFile">压缩后保存位置</param>  
        /// <param name="dHeight">高度</param>  
        /// <param name="dWidth"></param>  
        /// <param name="flag">压缩质量(数字越小压缩率越高) 1-100</param>  
        /// <returns></returns>  

        public static bool GetPicThumbnail(string sFile, string dFile, int dHeight, int dWidth, int flag)
        {
            System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);
            ImageFormat tFormat = iSource.RawFormat;
            int sW = 0, sH = 0;

            //按比例缩放
            Size tem_size = new Size(iSource.Width, iSource.Height);

            if (tem_size.Width > dHeight || tem_size.Width > dWidth)
            {
                if ((tem_size.Width * dHeight) > (tem_size.Width * dWidth))
                {
                    sW = dWidth;
                    sH = (dWidth * tem_size.Height) / tem_size.Width;
                }
                else
                {
                    sH = dHeight;
                    sW = (tem_size.Width * dHeight) / tem_size.Height;
                }
            }
            else
            {
                sW = tem_size.Width;
                sH = tem_size.Height;
            }

            Bitmap ob = new Bitmap(dWidth, dHeight);
            Graphics g = Graphics.FromImage(ob);

            g.Clear(Color.WhiteSmoke);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);

            g.Dispose();
            //以下代码为保存图片时，设置压缩质量  
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100  
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    ob.Save(dFile, jpegICIinfo, ep);//dFile是压缩后的新路径  
                }
                else
                {
                    ob.Save(dFile, tFormat);
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                iSource.Dispose();
                ob.Dispose();
            }
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
            carinfobll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, CarinfoEntity entity)
        {
            DataItemDetailBLL pdata = new DataItemDetailBLL();
            var pitem = pdata.GetItemValue("Hikappkey");//海康服务器密钥
            var url = pdata.GetItemValue("HikBaseUrl");//海康服务器地址
            carinfobll.SaveForm(keyValue, entity, pitem, url);
            return Success("操作成功。");
        }

        /// <summary>
        /// 私家车辆审核
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public ActionResult CartoExamine(string keyValue, CarinfoEntity entity)
        {
            var item = carinfobll.GetEntity(keyValue);
            if (item != null)
            {
                item.State = entity.State;
                item.Remark = entity.Remark;
                if (item.State == "1")
                {
                    item.Starttime = entity.Starttime;
                    item.Endtime = entity.Endtime;
                    item.Currentgid = entity.Currentgid;
                    item.Currentgname = entity.Currentgname;
                }
                carinfobll.CartoExamine(keyValue, item);
            }
            return Success("操作成功。");
        }



        #endregion
    }
}
