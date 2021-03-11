using ERCHTMS.Entity.SystemManage;
using ERCHTMS.IService.SystemManage;
using ERCHTMS.Service.SystemManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.SystemManage
{
    /// <summary>
    /// 描 述：外包电厂提交资料说明表
    /// </summary>
    public class OutcommitfileBLL
    {
        private OutcommitfileIService service = new OutcommitfileService();

        #region 获取数据


        public DataTable GetPageList(Pagination pagination, string queryJson) {
            return service.GetPageList(pagination, queryJson);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<OutcommitfileEntity> GetList()
        {
            return service.GetList();
        }
        /// <summary>
        /// 根据机构Code查询本机构是否已经添加
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public bool GetIsExist(string orgCode){
            return service.GetIsExist(orgCode);
        }

        /// <summary>
        /// 根据机构Code查询本机构添加的信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public OutcommitfileEntity GetEntityByOrgCode(string orgCode)
        {
            return service.GetEntityByOrgCode(orgCode);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public OutcommitfileEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, OutcommitfileEntity entity)
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
