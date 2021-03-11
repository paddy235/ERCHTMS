using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.IService.KbsDeviceManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Service.KbsDeviceManage;

namespace ERCHTMS.Busines.KbsDeviceManage
{
    /// <summary>
    ///描述：基站管理
    /// </summary>
    public class BaseStationBLL
    {

        private BaseStationIService service = new BaseStationService();

        #region 获取数据
        /// <summary>
        /// 获取全部列表数据
        /// </summary>
        /// <returns>返回分页列表</returns>
        public List<BaseStationEntity> GetPageList(string queryJson)
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
                {//区域
                    string AreaCode = queryParam["AreaCode"].ToString();
                    list = list.Where(it => it.AreaCode.Contains(AreaCode)).ToList();
                }
                if (!queryParam["Search"].IsEmpty())
                {//全列
                    string Search = queryParam["Search"].ToString();
                    list = list.Where(it => it.StationID.Contains(Search) || it.StationName.Contains(Search) || it.StationType.Contains(Search) || it.AreaName.Contains(Search) || it.FloorCode.Contains(Search) || it.OperUserName.Contains(Search)).ToList();
                }
            }

            return list;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<BaseStationEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public BaseStationEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 获取基站统计图
        /// </summary>
        /// <returns></returns>
        public string GetLableChart()
        {
            return service.GetLableChart();
        }


        /// <summary>
        /// 根据状态获取基站数量
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public int GetBaseStationNum(string status)
        {
            return service.GetBaseStationNum(status);
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
        public void SaveForm(string keyValue, BaseStationEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
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
        public void UpdateState(BaseStationEntity entity)
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
