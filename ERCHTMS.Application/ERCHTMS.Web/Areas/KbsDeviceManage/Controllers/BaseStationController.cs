using BSFramework.Util;
using BSFramework.Util.Offices;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.KbsDeviceManage;
using ERCHTMS.Busines.MatterManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Entity.KbsDeviceManage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.KbsDeviceManage.Controllers
{
    /// <summary>
    /// 基站管理
    /// </summary>
    public class BaseStationController : MvcControllerBase
    {
        private BaseStationBLL basebll = new BaseStationBLL();

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
        /// 统计页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Static()
        {
            return View();
        }
        /// <summary>
        /// 基站导入
        /// </summary>
        /// <returns></returns>
        public ActionResult Import()
        {
            return View();
        }
        public ActionResult GetPos()
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
            var user = OperatorProvider.Provider.Current();
            string orgcode = user.OrganizeId;
            var watch = CommonHelper.TimerStart();
            var data = basebll.GetPageList(queryJson);

            int total = data.Count / pagination.rows;
            if (data.Count % pagination.rows != 0)
            {
                total += 1;
            }

            var jsonData = new
            {
                rows = data,
                total = total,
                page = 1,
                records = data.Count,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return ToJsonResult(jsonData);
        }

        /// <summary>
        /// 根据状态获取基站数量
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetBaseStationNum(string status)
        {
            return basebll.GetBaseStationNum(status).ToString();
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = basebll.GetList(queryJson);
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
            var data = basebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取基站总数量
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetCount()
        {
            return basebll.GetList("").ToList().Count.ToString();
        }

        /// <summary>
        /// 统计表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetLableStatistics()
        {
            var data = basebll.GetPageList("");
            DistrictBLL districtbll = new DistrictBLL();
            List<DistrictEntity> AreaList = districtbll.GetListByOrgIdAndParentId("", "0");
            List<KbsEntity> klist = new List<KbsEntity>();
            int Znum = 0;
            foreach (var item in AreaList)
            {
                KbsEntity kbs = new KbsEntity();
                kbs.Name = item.DistrictName;
                kbs.Num = data.Where(a => a.AreaCode.Contains(item.DistrictCode)).Count();
                Znum += kbs.Num;
                kbs.Num2 = kbs.Num;
                klist.Add(kbs);
            }
            for (int j = 0; j < klist.Count; j++)
            {
                double Proportion = 0;
                if (Znum != 0)
                {
                    Proportion = (double)klist[j].Num / Znum;
                    Proportion = Proportion * 100;
                }
                klist[j].Proportion = Proportion.ToString("0") + "%";
            }

            return Content(klist.ToJson());
        }

        /// <summary>
        /// 首页基站表格统计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetIndexBaseStation()
        {
            var data = basebll.GetPageList("");
            DistrictBLL districtbll = new DistrictBLL();
            List<DistrictEntity> AreaList = districtbll.GetListByOrgIdAndParentId("", "0");
            List<KbssEntity> klist = new List<KbssEntity>();
            int Znum = 0;
            foreach (var item in AreaList)
            {
                KbssEntity kbs = new KbssEntity();
                kbs.Name = item.DistrictName;
                kbs.Num = data.Where(a => a.AreaCode.Contains(item.DistrictCode)).Count();
                kbs.DistrictCode = item.DistrictCode;
                Znum += kbs.Num;
                kbs.OnNum = data.Where(a => a.AreaCode.Contains(item.DistrictCode) && a.State == "在线").Count();
                kbs.OffNum = data.Where(a => a.AreaCode.Contains(item.DistrictCode) && a.State == "离线").Count();
                klist.Add(kbs);
            }

            for (int j = 0; j < klist.Count; j++)
            {
                double Proportion = 0;
                double offProportion = 0;
                if (Znum != 0)
                {
                    Proportion = (double)klist[j].OnNum / Znum;
                    offProportion = (double)klist[j].OffNum / Znum;
                    Proportion = Proportion * 100;
                    offProportion = offProportion * 100;
                }
                klist[j].Count = Znum;
                klist[j].OnProportion = Proportion.ToString("0") + "%";
                klist[j].OfflineProportion = offProportion.ToString("0") + "%";
            }
            return Content(klist.ToJson());

        }


        /// <summary>
        /// 基站统计图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetLableChart()
        {
            List<object[]> list = new List<object[]>();
            var data = basebll.GetPageList("");
            DistrictBLL districtbll = new DistrictBLL();
            List<DistrictEntity> AreaList = districtbll.GetListByOrgIdAndParentId("", "0");
            List<KbsEntity> klist = new List<KbsEntity>();
            foreach (var item in AreaList)
            {
                int num = 0;
                num = data.Where(it => it.AreaCode.Contains(item.DistrictCode)).Count();
                object[] arr = { item.DistrictName, num };
                if (num > 0)
                {
                    list.Add(arr);
                }
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(list);
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
            basebll.RemoveForm(keyValue);
            //将基站信息同步到后台计算服务中
            RabbitMQHelper rh = RabbitMQHelper.CreateInstance();
            SendData sd = new SendData();
            sd.DataName = "DelBaseStationEntity";
            sd.EntityString = keyValue;
            rh.SendMessage(JsonConvert.SerializeObject(sd));
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
        public ActionResult SaveForm(string keyValue, BaseStationEntity entity)
        {
            basebll.SaveForm(keyValue, entity);
            //将基站信息同步到后台计算服务中
            RabbitMQHelper rh = RabbitMQHelper.CreateInstance();
            SendData sd = new SendData();
            if (string.IsNullOrEmpty(keyValue)) { sd.DataName = "AddBaseStationEntity"; }
            else { sd.DataName = "UppBaseStationEntity"; }
            sd.EntityString = JsonConvert.SerializeObject(entity);
            rh.SendMessage(JsonConvert.SerializeObject(sd));
            return Success("操作成功。");
        }

        //<summary>
        //导入基站信息
        //</summary>
        //<returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportStation()
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

                DistrictBLL districtbll = new DistrictBLL();
                List<DistrictEntity> AreaList = districtbll.GetListByOrgIdAndParentId("", "");

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    string CameraID = dt.Rows[i][0].ToString();
                    string CameraName = dt.Rows[i][1].ToString();
                    string CameraType = dt.Rows[i][2].ToString();
                    //区域
                    string AreaName = dt.Rows[i][3].ToString();
                    string AreaValue = "";
                    string AreaCode = "";
                    //楼层编号
                    string FloorNo = dt.Rows[i][4].ToString();
                    string CameraPoint = dt.Rows[i][5].ToString();
                    string CameraIp = dt.Rows[i][6].ToString();

                    if (string.IsNullOrEmpty(CameraID))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "基站ID为空,未能导入.";
                        error++;
                        continue;
                    }

                    if (string.IsNullOrEmpty(CameraName))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "基站名称为空,未能导入.";
                        error++;
                        continue;
                    }

                    if (string.IsNullOrEmpty(CameraType))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "基站型号为空,未能导入.";
                        error++;
                        continue;
                    }

                    if (string.IsNullOrEmpty(AreaName))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "区域名称为空,未能导入.";
                        error++;
                        continue;
                    }

                    if (string.IsNullOrEmpty(FloorNo))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "楼层编号为空,未能导入.";
                        error++;
                        continue;
                    }

                    if (string.IsNullOrEmpty(CameraPoint))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "基站坐标为空,未能导入.";
                        error++;
                        continue;
                    }

                    if (string.IsNullOrEmpty(CameraIp))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "IP地址为空,未能导入.";
                        error++;
                        continue;
                    }
                    var IP = @"(^(\d+)\.(\d+)\.(\d+)\.(\d+)$)";//@"/^(\d+)\.(\d+)\.(\d+)\.(\d+)$/g";
                    var point = @"(^\d{1,9}(.\d{1,2});\d{1,9}(.\d{1,2})$)";

                    ////验证是否是IP
                    if (!Regex.IsMatch(CameraIp, IP))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行IP地址格式填写错误,未能导入.";
                        error++;
                        continue;
                    }

                    ////验证是否是坐标
                    if (!Regex.IsMatch(CameraPoint, point))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行坐标格式填写错误,格式应为xx.xx;xx.xx,未能导入.";
                        error++;
                        continue;
                    }

                    var area = AreaList.Where(it => it.DistrictName == AreaName).FirstOrDefault();
                    if (area == null)
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行区域名称填写错误,未找到对应的区域,未能导入.";
                        error++;
                        continue;
                    }

                    AreaValue = area.DistrictID;
                    AreaCode = area.DistrictCode;

                    BaseStationEntity kbs = new BaseStationEntity();
                    kbs.AreaCode = AreaCode;
                    kbs.AreaId = AreaValue;
                    kbs.AreaName = AreaName;
                    kbs.StationID = CameraID;
                    kbs.StationName = CameraName;
                    kbs.StationType = CameraType;
                    kbs.FloorCode = FloorNo;
                    kbs.OperUserName = OperatorProvider.Provider.Current().UserName;
                    kbs.AreaId = AreaValue;
                    kbs.StationIP = CameraIp;
                    kbs.StationCode = CameraPoint;
                    try
                    {
                        basebll.SaveForm("", kbs);
                        //将基站信息同步到后台计算服务中
                        RabbitMQHelper rh = RabbitMQHelper.CreateInstance();
                        SendData sd = new SendData();
                        sd.DataName = "AddBaseStationEntity";
                        sd.EntityString = JsonConvert.SerializeObject(kbs);
                        rh.SendMessage(JsonConvert.SerializeObject(sd));
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