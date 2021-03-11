using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.IService.KbsDeviceManage;
using ERCHTMS.Service.KbsDeviceManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Linq;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Entity.CarManage;
using Newtonsoft.Json;

namespace ERCHTMS.Busines.KbsDeviceManage
{
    /// <summary>
    /// 描 述：康巴什门禁管理
    /// </summary>
    public class KbsdeviceBLL
    {
        private KbsdeviceIService service = new KbsdeviceService();

        #region 获取数据
        /// <summary>
        /// 获取全部列表数据
        /// </summary>
        /// <returns>返回分页列表</returns>
        public List<KbsdeviceEntity> GetPageList(string queryJson)
        {
            var list = service.GetPageList();
            if (!string.IsNullOrEmpty(queryJson))
            {
                var queryParam = queryJson.ToJObject();
                //是否在线
                if (!queryParam["selectStatus"].IsEmpty())
                {
                    string selectStatus = queryParam["selectStatus"].ToString();
                    list = list.Where(it => it.State == selectStatus).ToList();
                }

                if (!queryParam["AreaCode"].IsEmpty())
                {
                    string AreaCode = queryParam["AreaCode"].ToString();
                    list = list.Where(it => it.AreaCode.Contains(AreaCode)).ToList();
                }

                if (!queryParam["Search"].IsEmpty())
                {
                    string Search = queryParam["Search"].ToString();
                    list = list.Where(it => it.DeviceId.Contains(Search) || it.DeviceName.Contains(Search) || it.DeviceModel.Contains(Search) || it.AreaName.Contains(Search) || it.FloorNo.Contains(Search) || it.OperUserName.Contains(Search)).ToList();
                }
            }

            return list;
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<KbsdeviceEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public KbsdeviceEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 根据状态获取摄像头数量
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public int GetDeviceNum(string status)
        {
            return service.GetDeviceNum(status);
        }

        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
                RabbitMQHelper rh = RabbitMQHelper.CreateInstance();
                SendData sd = new SendData();
                sd.DataName = "RemoveDevice";
                sd.EntityString = keyValue;
                rh.SendMessage(JsonConvert.SerializeObject(sd));
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, KbsdeviceEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
                RabbitMQHelper rh = RabbitMQHelper.CreateInstance();
                SendData sd = new SendData();
                if (keyValue == "")
                {
                    sd.DataName = "AddDevice";
                }
                else
                {
                    sd.DataName = "UpdateDevice";
                }
                sd.EntityString = JsonConvert.SerializeObject(entity);
                rh.SendMessage(JsonConvert.SerializeObject(sd));
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 接口修改状态用方法
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateState(KbsdeviceEntity entity)
        {
            try
            {
                service.UpdateState(entity);
            }
            catch (Exception e)
            {
                throw;
                throw;
            }
        }

        #endregion
    }
}
