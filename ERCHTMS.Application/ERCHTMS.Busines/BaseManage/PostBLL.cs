using ERCHTMS.Entity.BaseManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.Service.BaseManage;
using BSFramework.Util.WebControl;
using System;
using System.Linq;
using System.Collections.Generic;
using BSFramework.Cache.Factory;
using System.Data;


namespace ERCHTMS.Busines.BaseManage
{
    /// <summary>
    /// 描 述：岗位管理
    /// </summary>
    public class PostBLL
    {
        private IPostService service = new PostService();
        /// <summary>
        /// 缓存key
        /// </summary>
        public string cacheKey = BSFramework.Util.Config.GetValue("SoftName") + "_PostCache";

        #region 获取数据
        /// <summary>
        /// 验证岗位名称是否重复
        /// </summary>
        /// <param name="postName">部门名称</param>
        /// <returns></returns>
        public bool ExistPostJugement(string postName)
        {
            return service.ExistPostJugement(postName);
        }
        /// <summary>
        /// 岗位列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RoleEntity> GetList()
        {
            return service.GetList();
        }
        /// <summary>
        /// 岗位列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<RoleEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        public DataTable GetList(Pagination pagination, string queryJson)
        {
            return service.GetList(pagination, queryJson);
        }
        /// <summary>
        /// 岗位列表(ALL)
        /// </summary>
        /// <returns></returns>
        public List<RoleEntity> GetAllList()
        {
            return service.GetAllList().ToList();
        }
        /// <summary>
        /// 岗位实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public RoleEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 手机端获取岗位
        /// </summary>
        /// <param name="organizeid"></param>
        /// <returns></returns>
        public DataTable GetPostForAPP()
        {
            return service.GetPostForAPP();
        }
        #endregion

        #region 验证数据
        /// <summary>
        /// 岗位编号不能重复
        /// </summary>
        /// <param name="enCode">编号</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool ExistEnCode(string enCode, string keyValue)
        {
            return service.ExistEnCode(enCode, keyValue);
        }
        /// <summary>
        /// 岗位名称不能重复
        /// </summary>
        /// <param name="fullName">名称</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool ExistFullName(string fullName, string keyValue)
        {
            return service.ExistFullName(fullName, keyValue);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除岗位
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
                CacheFactory.Cache().RemoveCache(cacheKey);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 保存岗位表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="postEntity">岗位实体</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, RoleEntity postEntity)
        {
            try
            {
                service.SaveForm(keyValue, postEntity);
                CacheFactory.Cache().RemoveCache(cacheKey);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
