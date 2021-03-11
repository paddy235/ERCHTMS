using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.IService.KbsDeviceManage;
using ERCHTMS.Service.KbsDeviceManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using System.Linq;
using BSFramework.Util;
using BSFramework.Util.Extension;

namespace ERCHTMS.Busines.KbsDeviceManage
{
    /// <summary>
    /// 描 述：标签管理
    /// </summary>
    public class LablemanageBLL
    {
        private LablemanageIService service = new LablemanageService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public List<LablemanageEntity> GetPageList(Pagination pagination, string queryJson)
        {
            var list = service.GetPageList(pagination, queryJson);
            //是否在线
            //if (!queryParam["selectStatus"].IsEmpty())
            //{
            //    string selectStatus = queryParam["selectStatus"].ToString();

            //    list = list.Where(it => it.State == selectStatus).ToList();
            //}
            //if (!queryParam["selectType"].IsEmpty())
            //{
            //    string selectType = queryParam["selectType"].ToString();
            //    list = list.Where(it => it.LableTypeId == selectType).ToList();
            //}
            //if (!queryParam["deptCode"].IsEmpty())
            //{
            //    string deptCode = queryParam["deptCode"].ToString();
            //    list = list.Where(it => it.DeptCode.Contains(deptCode)).ToList();
            //}
            //if (!queryParam["Search"].IsEmpty())
            //{
            //    string Search = queryParam["Search"].ToString();
            //    list = list.Where(it => it.LableId.Contains(Search) || it.DeptName.Contains(Search) || it.LableTypeName.Contains(Search) || it.Name.Contains(Search)).ToList();
            //}

            return list;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public List<LablemanageEntity> GetPageList(string LableID, string Search)
        {
            var list = service.GetPageList(null, "");
            if (!string.IsNullOrEmpty(LableID))
            {
                list = list.Where(it => it.LableId == LableID).ToList();
            }
            if (!string.IsNullOrEmpty(Search))
            {
                list = list.Where(it => it.DeptName == Search || it.Name == Search).ToList();
            }
            return list;
        }


        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public List<LablemanageEntity> GetList(string LableTypeId, string DeptCode, string BindTime, int type)
        {
            var list = service.GetPageList(null, "");
            if (type != -1)
            {
                list = list.Where(it => it.Type == type).ToList();
            }
            //是否在线
            if (!string.IsNullOrEmpty(LableTypeId))
            {
                list = list.Where(it => it.LableTypeId == LableTypeId).ToList();
            }
            if (!string.IsNullOrEmpty(DeptCode))
            {
                list = list.Where(it => it.DeptCode.Contains(DeptCode)).ToList();
            }
            if (!string.IsNullOrEmpty(BindTime))
            {
                list = list.Where(it => it.BindTime.ToString().Contains(BindTime)).ToList();
            }

            return list;
        }

        /// <summary>
        /// 获取所有标签列表
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<LablemanageEntity> GetList(string queryJson)
        {
            return service.GetList("");
        }



        /// <summary>
        /// 获取标签总数
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return service.GetCount();
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public LablemanageEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 获取标签统计图
        /// </summary>
        /// <returns></returns>
        public string GetLableChart()
        {
            return service.GetLableChart();
        }

        /// <summary>
        /// 获取标签统计信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetLableStatistics()
        {
            return service.GetLableStatistics();
        }

        /// <summary>
        /// 获取用户绑定标签
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public LablemanageEntity GetUserLable(string userid)
        {
            return service.GetUserLable(userid);
        }

        /// <summary>
        /// 获取车辆是否绑定标签
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public LablemanageEntity GetCarLable(string CarNo)
        {
            return service.GetCarLable(CarNo);
        }

        /// <summary>
        /// 获取标签是否重复绑定
        /// </summary>
        /// <param name="LableId"></param>
        /// <returns></returns>
        public bool GetIsBind(string LableId)
        {
            return service.GetIsBind(LableId);
        }

        /// <summary>
        /// 根据lableId获取是否有绑定信息
        /// </summary>
        /// <param name="LableId"></param>
        /// <returns></returns>
        public LablemanageEntity GetLable(string LableId)
        {
            return service.GetLable(LableId);
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
        /// 解绑标签
        /// </summary>
        /// <param name="keyValue"></param>
        public void Untie(string keyValue)
        {
            try
            {
                service.Untie(keyValue);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, LablemanageEntity entity)
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
        #endregion
    }
}
