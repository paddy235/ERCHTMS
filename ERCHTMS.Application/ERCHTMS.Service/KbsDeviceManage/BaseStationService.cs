using BSFramework.Data.Repository;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.IService.KbsDeviceManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Service.KbsDeviceManage
{
    /// <summary>
    /// 描述：基站管理
    /// </summary>
    public class BaseStationService : RepositoryFactory<BaseStationEntity>, BaseStationIService
    {
        #region 获取数据
        /// <summary>
        /// 获取全部列表数据
        /// </summary>
        /// <returns>返回分页列表</returns>
        public List<BaseStationEntity> GetPageList()
        {
            return this.BaseRepository().IQueryable().OrderByDescending(it=>it.CreateDate).ToList();

        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<BaseStationEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public BaseStationEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 根据状态获取基站数量
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public int GetBaseStationNum(string status)
        {
            return this.BaseRepository().IQueryable(it => it.State == status).Count();
        }

        /// <summary>
        /// 获取基站统计图
        /// </summary>
        /// <returns></returns>
        public string GetLableChart()
        {
            List<object[]> list = new List<object[]>();
            string sql = string.Format("select areaid,areaname,count(1) from BIS_BASESTATION d group by d.areaid,d.areaname");
            DataTable dt = this.BaseRepository().FindTable(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                object[] arr = { dt.Rows[i][1].ToString(), Convert.ToInt32(dt.Rows[i][2]) };
                list.Add(arr);

            }
            dt.Dispose();
            return Newtonsoft.Json.JsonConvert.SerializeObject(list);
        }


        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, BaseStationEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }
        /// <summary>
        /// 接口修改状态用方法
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateState(BaseStationEntity entity)
        {
            this.BaseRepository().Update(entity);
        }
        #endregion
    }
}
