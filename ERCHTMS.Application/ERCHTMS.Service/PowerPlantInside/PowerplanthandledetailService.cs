using ERCHTMS.Entity.PowerPlantInside;
using ERCHTMS.IService.PowerPlantInside;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using System.Data;
using ERCHTMS.Service.BaseManage;
using System;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using ERCHTMS.Code;

namespace ERCHTMS.Service.PowerPlantInside
{
    /// <summary>
    /// 描 述：事故事件处理信息
    /// </summary>
    public class PowerplanthandledetailService : RepositoryFactory<PowerplanthandledetailEntity>, PowerplanthandledetailIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<PowerplanthandledetailEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public PowerplanthandledetailEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                if (queryJson.Length > 0)
                {
                    var queryParam = queryJson.ToJObject();
                }
                return this.BaseRepository().FindTableByProcPager(pagination, DbHelper.DbType);
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// 根据事故事件处理记录ID获取事故事件处理信息列表
        /// </summary>
        /// <param name="keyValue">事故事件处理记录I</param>
        /// <returns></returns>
        public IList<PowerplanthandledetailEntity> GetHandleDetailList(string keyValue)
        {
            string sql = string.Format("select * from bis_powerplanthandledetail where powerplanthandleid='{0}'", keyValue);
            return this.BaseRepository().FindList(sql).ToList();
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
        public void SaveForm(string keyValue, PowerplanthandledetailEntity entity)
        {
            var res = GetEntity(keyValue);
            //给整改责任部门赋值
            if (!string.IsNullOrWhiteSpace(entity.RectificationDutyPersonId))
            {
                entity.RectificationDutyDeptId = "";
                entity.RectificationDutyDept = "";
                string[] dutyuserlist = entity.RectificationDutyPersonId.Split(',');
                for (int i = 0; i < dutyuserlist.Length; i++)
                {
                    var user = new UserInfoService().GetUserInfoEntity(dutyuserlist[i].ToString());
                    if (user != null)
                    {
                        entity.RectificationDutyDept += entity.RectificationDutyDeptId.Contains(user.DepartmentId) ? "" : user.DeptName + ",";
                        entity.RectificationDutyDeptId += entity.RectificationDutyDeptId.Contains(user.DepartmentId) ? "" : user.DepartmentId + ",";
                    }
                }
                if (!string.IsNullOrWhiteSpace(entity.RectificationDutyDeptId))
                {
                    entity.RectificationDutyDept = entity.RectificationDutyDept.Substring(0, entity.RectificationDutyDept.Length - 1);
                    entity.RectificationDutyDeptId = entity.RectificationDutyDeptId.Substring(0, entity.RectificationDutyDeptId.Length - 1);
                }
            }
            if (res == null)
            {
                entity.Create();
                entity.ApplyState = 0;
                entity.ApplyCode = DateTime.Now.ToString("yyyyMMddHHmmss");
                this.BaseRepository().Insert(entity);
            }
            else
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
        }
        #endregion
    }
}
