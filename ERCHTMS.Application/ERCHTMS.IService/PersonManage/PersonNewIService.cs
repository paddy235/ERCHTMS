using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Entity.PersonManage;
using System.Data;
using BSFramework.Util.WebControl;

namespace ERCHTMS.IService.PersonManage
{
    /// <summary>
    /// 1.4版及以上版本权限下发
    /// </summary>
    public interface PersonNewIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<TemporaryGroupsEntity> GetList(string queryJson);
        /// <summary>
        /// 列表分页
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        TemporaryGroupsEntity GetEntity(string keyValue);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        TemporaryUserEntity GetUserEntity(string keyValue);
        TemporaryUserEntity GetEmptyUserEntity(string keyValue);

        /// <summary>
        /// 获取满足黑名单条件的人员
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        DataTable GetBlacklistUsers(ERCHTMS.Code.Operator user);
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveForm(string keyValue, List<TemporaryGroupsEntity> entity, List<TemporaryGroupsEntity> Updatelist);

        List<AddJurisdictionEntity> SaveUForm(string keyValue, TemporaryUserEntity entity);
        void SaveTemporaryList(string type, List<TemporaryUserEntity> entity);
        void SaveTempImport(string type, List<TemporaryUserEntity> entity);//批量导入临时人员
        void DeleteTemporaryList(string type, TemporaryUserEntity entity);
        AddJurisdictionEntity DelEquipmentRecord(List<TemporaryUserEntity> list, string baseUrl, string Key, string Signature);

        void SaveUserJurisdiction(TemporaryUserEntity UserEntity, string userids, string type);

        List<AddJurisdictionEntity> SaveCycle(List<TemporaryUserEntity> entity);//考勤周期设置 

        void downloadUserlimits(List<resourceInfos1> resourceInfos, string baseUrl, string Key, string Signature);

        List<AddJurisdictionEntity> SaveForbidden(List<TemporaryUserEntity> entity);
        List<AddJurisdictionEntity> RemoveForbidden(string entity);

        List<AddJurisdictionEntity> SaveUserFace(TemporaryUserEntity entity, string keyValue, bool b);

        #endregion

        #region 新增
        /// <summary>
        /// 获取所有临时人员
        /// </summary>
        /// <returns></returns>
        List<TemporaryUserEntity> GetUserList();

        /// <summary>
        /// 批量添加人员并录入人脸
        /// </summary>
        bool SaveFace(List<TemporaryUserEntity> insertList, List<TemporaryUserEntity> updateList, string baseUrl,
            string Key, string Signature);

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        TemporaryUserEntity HikGetUserEntity(string keyValue);
        #endregion
    }
}
