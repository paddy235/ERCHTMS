using BSFramework.Util.WebControl;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Entity.PersonManage;
using ERCHTMS.IService.PersonManage;
using ERCHTMS.Service.PersonManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Busines.PersonManage
{
    /// <summary>
    /// 临时分组管理
    /// </summary>
    public class TemporaryGroupsBLL
    {
        private TemporaryGroupsIService service = new TemporaryGroupsService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<TemporaryGroupsEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 列表分页
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
        public TemporaryGroupsEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 获取临时人员实体(或用户表基础信息)
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public TemporaryUserEntity GetUserEntity(string keyValue)
        {
            return service.GetUserEntity(keyValue);
        }

        /// <summary>
        /// 获取临时人员实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public TemporaryUserEntity GetEmptyUserEntity(string keyValue)
        {
            return service.GetEmptyUserEntity(keyValue);
        }

        /// <summary>
        /// 获取满足黑名单条件的人员
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable GetBlacklistUsers(ERCHTMS.Code.Operator user)
        {
            return service.GetBlacklistUsers(user);
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
        public void SaveForm(string keyValue, List<TemporaryGroupsEntity> entity, List<TemporaryGroupsEntity> Updatelist)
        {
            try
            {
                service.SaveForm(keyValue, entity,Updatelist);
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
        public List<AddJurisdictionEntity> SaveUForm(string keyValue, TemporaryUserEntity entity)
        {
            try
            {
               return service.SaveUForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 批量保存临时人员
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveTemporaryList(string keyValue, List<TemporaryUserEntity> entity)
        {
            try
            {
                service.SaveTemporaryList(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 批量导入临时人员
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveTempImport(string type, List<TemporaryUserEntity> entity)
        {
            try
            {
                service.SaveTempImport(type, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 删除临时人员
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void DeleteTemporaryList(string type, TemporaryUserEntity entity)
        {
            try
            {
                service.DeleteTemporaryList(type, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 删除设备中对应出入权限配置
        /// </summary>
        public AddJurisdictionEntity DeleteUserlimits(List<TemporaryUserEntity> list, string baseUrl, string Key, string Signature)
        {
            try
            {
              return  service.DelEquipmentRecord(list, baseUrl, Key, Signature);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// 批量权限设置
        /// </summary>
        /// <param name="list"></param>
        public List<AddJurisdictionEntity> SaveCycle(List<TemporaryUserEntity> list)
        {
            try
            {
              return  service.SaveCycle(list);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 根据出入权限配置快捷下载(IC卡号、人脸)
        /// </summary>
        /// <param name="resourceInfos"></param>
        /// <param name="baseUrl"></param>
        /// <param name="Key"></param>
        /// <param name="Signature"></param>
        public void downloadUserlimits(List<resourceInfos1> resourceInfos, string baseUrl, string Key, string Signature)
        {
            try
            {
                service.downloadUserlimits(resourceInfos, baseUrl, Key, Signature);
            }
            catch (Exception)
            {
                throw;
            }
        }



        /// <summary>
        /// 加入禁入名单 
        /// </summary>
        /// <param name="list"></param>
        public List<AddJurisdictionEntity> SaveForbidden(List<TemporaryUserEntity> list)
        {
            try
            {
                return service.SaveForbidden(list);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// 删除设备上人脸权限数据
        /// </summary>
        /// <param name="list"></param>
        public List<AddJurisdictionEntity> DeleteRightFromDevice(List<TemporaryUserEntity> list)
        {
            try
            {
                return service.DeleteRightFromDevice(list);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 移除禁入名单
        /// </summary>
        /// <param name="list"></param>
        public List<AddJurisdictionEntity> RemoveForbidden(string list)
        {
            try
            {
                return service.RemoveForbidden(list);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 用户权限管理
        /// </summary>
        /// <param name="UserEntity"></param>
        /// <param name="userids"></param>
        /// <param name="type"></param>
        public void SaveUserJurisdiction(TemporaryUserEntity UserEntity, string userids, string type)
        {
            try
            {
                service.SaveUserJurisdiction(UserEntity, userids, type);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 单条记录授权或重新授权
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="keyValue"></param>
        /// <param name="Power">是否受过权 true 更改权限 false 添加权限</param>
        public List<AddJurisdictionEntity> SaveUserFace(TemporaryUserEntity entity, string keyValue, bool Power)
        {
            try
            {
                return service.SaveUserFace(entity, keyValue, Power);
            }
            catch (Exception)
            {
                throw;
            }

        }


        #endregion



        #region 新增

        /// <summary>
        /// 获取所有临时人员列表
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public List<TemporaryUserEntity> GetUserList()
        {
            return service.GetUserList();
        }

        /// <summary>
        /// 批量添加人员并录入人脸
        /// </summary>
        public bool SaveFace(List<TemporaryUserEntity> insertList, List<TemporaryUserEntity> updateList, string baseUrl,
            string Key, string Signature)
        {
            return service.SaveFace(insertList, updateList, baseUrl, Key, Signature);
        }

        /// <summary>
        /// 获取临时人员实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public TemporaryUserEntity HikGetUserEntity(string keyValue)
        {
            return service.HikGetUserEntity(keyValue);
        }



        #endregion
    }
}
