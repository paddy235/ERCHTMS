using ERCHTMS.Entity.CarManage;
using ERCHTMS.IService.CarManage;
using ERCHTMS.Service.CarManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.CarManage
{
    /// <summary>
    /// 描 述：危害因素车辆表
    /// </summary>
    public class HazardouscarBLL
    {
        private HazardouscarIService service = new HazardouscarService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<HazardouscarEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }

        /// <summary>
        /// 分页查询列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public HazardouscarEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 获取此危害因素是否配置了检查表
        /// </summary>
        /// <param name="HazardousId"></param>
        /// <returns></returns>
        public bool GetHazardous(string HazardousId)
        {
            return service.GetHazardous(HazardousId);
        }

        /// <summary>
        /// 根据车牌号获取车辆信息
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public HazardouscarEntity GetCar(string CarNo)
        {
            return service.GetCar(CarNo);
        }

        /// <summary>
        /// 获取当日危化品车辆数量
        /// </summary>
        /// <returns></returns>
        public List<HazardouscarEntity> GetHazardousList(string day)
        {
            return service.GetHazardousList(day);
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
        public void SaveForm(string keyValue, HazardouscarEntity entity)
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
        /// 保存表单随行人员及人脸图片
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <param name="userjson"></param>
        public void SaveFaceUserForm(string keyValue, HazardouscarEntity entity, List<CarUserFileImgEntity> userjson)
        {
            try
            {
                service.SaveFaceUserForm(keyValue, entity,userjson);
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
        public void Update(string keyValue, HazardouscarEntity entity)
        {
            try
            {
                service.Update(keyValue, entity);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// 改变GPS绑定信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <param name="pgpslist"></param>
        public void ChangeGps(string keyValue, HazardouscarEntity entity, List<PersongpsEntity> pgpslist)
        {
            try
            {
                service.ChangeGps(keyValue, entity, pgpslist);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// 改变危化品车辆数据状态位交接完成
        /// </summary>
        /// <param name="id"></param>
        public void ChangeProcess(string id)
        {
            try
            {
                service.ChangeProcess(id);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion
    }
}
