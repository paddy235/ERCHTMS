using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Data;
using BSFramework.Data;
using ERCHTMS.Service.SystemManage;
using ERCHTMS.Entity.SystemManage.ViewModel;

namespace ERCHTMS.Service.OutsourcingProject
{
    /// <summary>
    /// 描 述：施工器具
    /// </summary>
    public class ProjecttoolsService : RepositoryFactory<ProjecttoolsEntity>, IProjecttoolsService
    {
        private DataItemDetailService dataitemdetailservice = new DataItemDetailService();
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<ProjecttoolsEntity> GetList(string queryJson)
        {
            //return this.BaseRepository().IQueryable().ToList();
            return this.BaseRepository().FindList(" select * from EPG_PROJECTTOOLS where 1=1 " + queryJson).ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ProjecttoolsEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
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
        public void SaveForm(string keyValue, ProjecttoolsEntity entity)
        {
            entity.PROJECTTOOLSID = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                ProjecttoolsEntity se = this.BaseRepository().FindEntity(keyValue);
                if (se == null)
                {
                    //设备类型编码配置未配置对应专业则不需要验收
                    DataItemModel model = dataitemdetailservice.GetDataItemByDetailValue("ToolEquipmentType", entity.TOOLTYPE).FirstOrDefault();
                    entity.Status = string.IsNullOrWhiteSpace(model.Description) ? "0" : "";
                    entity.Create();
                    this.BaseRepository().Insert(entity);
                }
                else
                {
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }
            }
            else
            {
                //设备类型编码配置未配置对应专业则不需要验收
                DataItemModel model = dataitemdetailservice.GetDataItemByDetailValue("ToolEquipmentType", entity.TOOLTYPE).FirstOrDefault();
                entity.Status = string.IsNullOrWhiteSpace(model.Description) ? "" : "0";
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="list"></param>
        public int UpdateMultData(List<ProjecttoolsEntity> list)
        {
            IDatabase db = DbFactory.Base().BeginTrans();
            try
            {
                db.Update<ProjecttoolsEntity>(list);
                db.Commit();
                return 1;
            }
            catch (Exception ex)
            {

                db.Rollback();
                return 0;
            }
            
        }
        #endregion
    }
}
