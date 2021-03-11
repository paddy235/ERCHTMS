using ERCHTMS.Entity.PersonManage;
using ERCHTMS.IService.PersonManage;
using ERCHTMS.Service.PersonManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Busines.PersonManage
{
    /// <summary>
    /// 描 述：人员证书
    /// </summary>
    public class WorkRecordBLL
    {
        private WorkRecordIService service = new WorkRecordService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>返回列表</returns>
        public IEnumerable<WorkRecordEntity> GetList(string userId)
        {
            return service.GetList(userId);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public WorkRecordEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
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
        public void SaveForm(string keyValue, WorkRecordEntity entity)
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
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void NewSaveForm(string keyValue, WorkRecordEntity entity)
        {
            try
            {
                service.NewSaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 人员离场时写工作记录
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="deptId">部门Id</param>
        /// <returns></returns>
        public int WriteWorkRecord(UserInfoEntity user, ERCHTMS.Code.Operator currUser)
        {
            return service.WriteWorkRecord(user, currUser);
        }

        /// <summary>
        /// 人员操作换部门时写记录
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="deptId">部门Id</param>
        /// <returns></returns>
        public int WriteChangeRecord(UserInfoEntity user, ERCHTMS.Code.Operator currUser)
        {
            return service.WriteChangeRecord(user, currUser);
        }

        /// <summary>
        /// 修改离场原因则修改最近的工作记录结束时间
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public int EditRecord(string userid, string time)
        {
            return service.EidtRecord(userid, time);
        }

        #endregion

      
    }
}
