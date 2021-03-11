using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.Busines.KbsDeviceManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using BSFramework.Util.Offices;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Entity.SystemManage.ViewModel;

namespace ERCHTMS.Web.Areas.KbsDeviceManage.Controllers
{
    /// <summary>
    /// 描 述：康巴什门禁管理
    /// </summary>
    public class KbsdeviceController : MvcControllerBase
    {
        private KbsdeviceBLL kbsdevicebll = new KbsdeviceBLL();
        DataItemDetailBLL itemBll = new DataItemDetailBLL();
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
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult See()
        {
            return View();
        }

        /// <summary>
        /// 统计页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Static()
        {
            return View();
        }
        /// <summary>
        /// 基础管理首页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult BaseHome()
        {
            return View();
        }
        public ActionResult GetPos()
        {
            return View();
        }

        /// <summary>
        /// 三维全屏显示
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowThreePage()
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
            string orgcode = OperatorProvider.Provider.Current().OrganizeCode;
            var watch = CommonHelper.TimerStart();
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            var data = kbsdevicebll.GetPageList(queryJson);

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
        /// WEB获取三维地图路径
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string WebGetThreeUrl()
        {
            return itemBll.GetItemValue("WebKbsThreeDURL");
        }
        /// <summary>
        /// APP获取三维地图路径
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetThreeUrl()
        {
            return itemBll.GetItemValue("KbsThreeDURL");
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = kbsdevicebll.GetList(queryJson);
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
            var data = kbsdevicebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 标签统计图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetDeviceChart()
        {
            List<object[]> list = new List<object[]>();
            var data = kbsdevicebll.GetPageList("");
            DistrictBLL districtbll = new DistrictBLL();
            List<DistrictEntity> AreaList = districtbll.GetListByOrgIdAndParentId("", "0");
            List<KbsEntity> klist = new List<KbsEntity>();
            int Znum = 0;
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

        /// <summary>
        /// 标签统计表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetDeviceStatistics()
        {
            var data = kbsdevicebll.GetPageList("");
            DistrictBLL districtbll = new DistrictBLL();
            List<DistrictEntity> AreaList = districtbll.GetListByOrgIdAndParentId("", "0");
            List<KbssEntity> klist = new List<KbssEntity>();
            int Znum = 0;
            foreach (var item in AreaList)
            {
                KbssEntity kbs = new KbssEntity();
                kbs.Name = item.DistrictName;
                kbs.Num = data.Where(it => it.AreaCode.Contains(item.DistrictCode)).Count();
                kbs.DistrictCode = item.DistrictCode;
                Znum += kbs.Num;
                kbs.OnNum = data.Where(it => it.AreaCode.Contains(item.DistrictCode) && it.State == "在线").Count();
                kbs.OffNum = data.Where(it => it.AreaCode.Contains(item.DistrictCode) && it.State == "离线").Count();
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
                klist[j].OnProportion = Proportion.ToString("0.00") + "%";
                klist[j].OfflineProportion = offProportion.ToString("0.00") + "%";
            }

            return Content(klist.ToJson());
        }

        /// <summary>
        /// 获取标签总数
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetCount()
        {
            return kbsdevicebll.GetPageList("").Count.ToString();
        }

        /// <summary>
        /// 根据状态获取基站数量
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetDeviceNum(string status)
        {
            return kbsdevicebll.GetDeviceNum(status).ToString();
        }
        #endregion

        #region 提交数据
        //<summary>
        //导入门禁
        //</summary>
        //<returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportDevice()
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


                DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();
                List<DataItemModel> data = dataItemDetailBLL.GetDataItemListByItemCode("'KbsOutType'").ToList();



                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    string DeviceID = dt.Rows[i][0].ToString();
                    string ControllerId = dt.Rows[i][1].ToString();
                    string OutType = dt.Rows[i][2].ToString();
                    string DeviceName = dt.Rows[i][3].ToString();
                    string DeviceModel = dt.Rows[i][4].ToString();
                    //区域
                    string AreaName = dt.Rows[i][5].ToString();
                    string AreaValue = "";
                    string AreaCode = "";
                    //楼层编号
                    string FloorNo = dt.Rows[i][6].ToString();
                    string DevicePoint = dt.Rows[i][7].ToString();
                    string DeviceIp = dt.Rows[i][8].ToString();

                    if (string.IsNullOrEmpty(DeviceID))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "门禁ID为空,未能导入.";
                        error++;
                        continue;
                    }
                    if (string.IsNullOrEmpty(ControllerId))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "控制器ID为空,未能导入.";
                        error++;
                        continue;
                    }
                    if (string.IsNullOrEmpty(OutType))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "进出类型为空,未能导入.";
                        error++;
                        continue;
                    }

                    if (string.IsNullOrEmpty(DeviceName))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "门禁名称为空,未能导入.";
                        error++;
                        continue;
                    }

                    if (string.IsNullOrEmpty(DeviceModel))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "门禁型号为空,未能导入.";
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

                    if (string.IsNullOrEmpty(DevicePoint))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "门禁坐标为空,未能导入.";
                        error++;
                        continue;
                    }

                    if (string.IsNullOrEmpty(DeviceIp))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "IP地址为空,未能导入.";
                        error++;
                        continue;
                    }
                    var IP = @"(^(\d+)\.(\d+)\.(\d+)\.(\d+)$)";//@"/^(\d+)\.(\d+)\.(\d+)\.(\d+)$/g";
                    var point = @"(^\d{1,9}(.\d{1,2});\d{1,9}(.\d{1,2})$)";

                    ////验证是否是IP
                    if (!Regex.IsMatch(DeviceIp, IP))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行IP地址格式填写错误,未能导入.";
                        error++;
                        continue;
                    }

                    ////验证是否是坐标
                    if (!Regex.IsMatch(DevicePoint, point))
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

                    var ot = data.Where(it => it.ItemName == OutType).FirstOrDefault();
                    if (ot == null)
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行进出类型填写错误,未找到对应的类型,未能导入.";
                        error++;
                        continue;
                    }

                    AreaValue = area.DistrictID;
                    AreaCode = area.DistrictCode;

                    KbsdeviceEntity kbs = new KbsdeviceEntity();
                    kbs.AreaCode = AreaCode;
                    kbs.AreaName = AreaName;
                    kbs.DeviceId = DeviceID;
                    kbs.DeviceName = DeviceName;
                    kbs.DeviceModel = DeviceModel;
                    kbs.OutType = Convert.ToInt32(ot.ItemValue);
                    kbs.FloorNo = FloorNo;
                    kbs.OperUserName = OperatorProvider.Provider.Current().UserName;
                    kbs.AreaId = AreaValue;
                    kbs.DeviceIP = DeviceIp;
                    kbs.DevicePoint = DevicePoint;
                    kbs.ControllerId = ControllerId;
                    kbs.State = "在线";

                    try
                    {
                        kbsdevicebll.SaveForm("", kbs);
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
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            kbsdevicebll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, KbsdeviceEntity entity)
        {
            if (keyValue == "")
            {
                entity.State = "在线";
            }
            kbsdevicebll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }

        #endregion
    }
}
