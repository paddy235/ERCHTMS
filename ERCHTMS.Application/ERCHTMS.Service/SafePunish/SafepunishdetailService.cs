using ERCHTMS.Entity.SafePunish;
using ERCHTMS.IService.SafePunish;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Service.BaseManage;

namespace ERCHTMS.Service.SafePunish
{
    /// <summary>
    /// 描 述：安全考核详细
    /// </summary>
    public class SafepunishdetailService : RepositoryFactory<SafepunishdetailEntity>, SafepunishdetailIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SafepunishdetailEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SafepunishdetailEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, SafepunishdetailEntity entity)
        {

            if (entity.PunishType == "人员")
            {
                UserEntity user = new UserService().GetEntity(entity.PunishNameId);
                if (user !=null)
                {
                    entity.BelongDept = user.DepartmentId;
                }
                
            }
            else if (entity.PunishType == "部门")
            {
                entity.BelongDept = entity.PunishNameId;
            }

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
        /// 获取奖励详细列表
        /// </summary>
        /// <param name="punishId">安全考核id</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public IEnumerable<SafepunishdetailEntity> GetListByPunishId(string punishId,string type)
        {
            return this.BaseRepository().IQueryable().ToList().Where(t => t.PunishId == punishId && t.Type == type);
        }

        /// <summary>
        /// 根据安全考核ID删除数据
        /// </summary>
        /// <param name="punishId">安全考核ID</param>
        /// <param name="type">类型</param>
        public int Remove(string punishId,string type)
        {
            return this.BaseRepository().ExecuteBySql(string.Format("delete from BIS_SAFEPUNISHDETAIL where punishId='{0}' and type='{1}'", punishId, type));
        }
        #endregion
    }
}
