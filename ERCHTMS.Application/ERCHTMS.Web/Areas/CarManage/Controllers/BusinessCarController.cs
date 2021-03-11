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
    public class BusinessCarController : MvcControllerBase
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
            pagination.p_fields = "info.carno,info.dirver,info.phone,info.numberlimit,info.model,info.insperctiondate,info.nextinsperctiondate,NVL(zvi.num,0) as znum,NVL(vi.num,0) as num";
            pagination.p_tablename = @"bis_carinfo info
     left join	(SELECT COUNT(vi.CARDNO) as num, vi.CARDNO FROM  BIS_CARVIOLATION  vi   group by CARDNO)
						zvi on info.CARNO=zvi.CARDNO
					 left join	(SELECT COUNT(vi.CARDNO) as num, vi.CARDNO FROM  BIS_CARVIOLATION  vi   where ISPROCESS=0 group by CARDNO)
						vi on info.CARNO=vi.CARDNO
            ";
            pagination.conditionJson = " 1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            //if (user.IsSystem)
            //{
            pagination.conditionJson = "1=1";
            //}
            //else
            //{




            //    string where = new ERCHTMS.Busines.AuthorizeManage.AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
            //    pagination.conditionJson += " and " + where;




            //}

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
            carinfobll.SaveForm(keyValue, entity,pitem,url);
            return Success("操作成功。");
        }

        /// <summary>
        /// 导入清单
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
                DataItemDetailBLL dataItemDetailBLL=new DataItemDetailBLL();
                List<DataItemModel> data = dataItemDetailBLL.GetDataItemListByItemCode("'CarNo'").ToList();


                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    //车牌号
                    string CarNo = dt.Rows[i][0].ToString();
                    //车辆品牌系列
                    string Model = dt.Rows[i][1].ToString();
                    //最近年检日期
                    string Time = dt.Rows[i][2].ToString();
                    //起始时间
                    string StartTime = dt.Rows[i][3].ToString();
                    ////结束时间
                    string EndTime = dt.Rows[i][4].ToString();
                   
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
                    if (Time.Trim() == "")
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "最近年检日期为空,未能导入.";
                        error++;
                        continue;
                    }
                    if (StartTime.Trim() == "")
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "起始时间为空,未能导入.";
                        error++;
                        continue;
                    }
                    if (EndTime.Trim() == "")
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "结束时间为空,未能导入.";
                        error++;
                        continue;
                    }
                    DateTime dtime;
                    try
                    {
                        DateTime.TryParse(Time, out dtime);

                        if (dtime.ToString("yyyy-MM-dd") == "0001-01-01")
                        {
                            falseMessage += "</br>" + "第" + (i + 2) + "最近年检日期格式不对,请输入2019-01-01格式,未能导入.";
                            error++;
                            continue;
                        }
                    }
                    catch (Exception e)
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "最近年检日期格式不对,请输入yyyy-MM-dd格式,未能导入.";
                        error++;
                        continue;
                    }

                    DateTime Stime;


                    DateTime.TryParse(StartTime, out Stime);

                    if (Stime.ToString("yyyy-MM-dd") == "0001-01-01")
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "起始时间格式不对,请输入yyyy-MM-dd格式,未能导入.";
                        error++;
                        continue;
                    }



                    DateTime Etime;


                    DateTime.TryParse(StartTime, out Etime);

                    if (Etime.ToString("yyyy-MM-dd") == "0001-01-01")
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "结束时间格式不对,请输入yyyy-MM-dd格式,未能导入.";
                        error++;
                        continue;
                    }


                    if (carinfobll.GetCarNoIsRepeat(CarNo, ""))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "车牌号数据已存在,请勿重复录入.";
                        error++;
                        continue;
                    }
                    Etime = Convert.ToDateTime(Etime.ToString("yyyy-MM-dd 23:59:59"));
                    Stime = Convert.ToDateTime(Stime.ToString("yyyy-MM-dd 00:00:00"));

                    CarinfoEntity hf = new CarinfoEntity();
                    hf.CarNo = CarNo.Trim().ToUpper();//英文转换为大写
                    hf.GpsId = "";
                    hf.GpsName = "";
                    hf.Dirver = "";
                    hf.InsperctionDate = dtime;
                    hf.Model = Model;
                    hf.Endtime = Etime;
                    hf.Starttime = Stime;
                    hf.Phone = "";
                    hf.NextInsperctionDate = dtime.AddDays(-1).AddYears(1);
                    hf.Type = 2;


                    try
                    {
                        DataItemDetailBLL pdata = new DataItemDetailBLL();
                        var pitem = pdata.GetItemValue("Hikappkey");//海康服务器密钥
                        var url = pdata.GetItemValue("HikBaseUrl");//海康服务器地址
                        carinfobll.SaveForm("", hf, pitem,url);
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
