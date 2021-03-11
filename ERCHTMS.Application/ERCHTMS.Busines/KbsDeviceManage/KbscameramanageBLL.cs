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
    /// 描 述：康巴什摄像头管理
    /// </summary>
    public class KbscameramanageBLL
    {
        private KbscameramanageIService service = new KbscameramanageService();

        #region 获取数据
        /// <summary>
        /// 获取全部列表数据
        /// </summary>
        /// <returns>返回分页列表</returns>
        public List<KbscameramanageEntity> GetPageList(string queryJson)
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
                    list = list.Where(it =>
                        it.CameraId.Contains(Search) || it.CameraName.Contains(Search) ||
                        it.CameraType.Contains(Search) || it.AreaName.Contains(Search) || it.FloorNo.Contains(Search) ||
                        it.OperuserName.Contains(Search)).ToList();
                }
            }

            return list;
        }
        
        /// <summary>
        /// 设想编码，唯一性检查。无重复返回true
        /// </summary>
        /// <param name="cameraId"></param>
        /// <returns></returns>
        public bool UniqueCheck(string cameraId)
        {
            return service.UniqueCheck(cameraId);
        }

        public void SaveEntity(string keyValue, KbscameramanageEntity entity)
        {
            service.SaveForm(keyValue, entity);
        }

        public void RemoveEntity(string keyValue)
        {
            service.RemoveForm(keyValue);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<KbscameramanageEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public KbscameramanageEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 根据状态获取摄像头数量
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public int GetCameraNum(string status)
        {
            return service.GetCameraNum(status);
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
                sd.DataName = "RemoveCamera";
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
        public void SaveForm(string keyValue, KbscameramanageEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
                RabbitMQHelper rh = RabbitMQHelper.CreateInstance();
                SendData sd = new SendData();
                if (keyValue == "")
                {
                    sd.DataName = "AddCamera";
                }
                else
                {
                    sd.DataName = "UpdateCamera";
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
        public void UpdateState(KbscameramanageEntity entity)
        {
            try
            {
                service.UpdateState(entity);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion
    }
}
