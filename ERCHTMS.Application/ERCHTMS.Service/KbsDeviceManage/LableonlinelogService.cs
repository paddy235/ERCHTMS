using System;
using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.IService.KbsDeviceManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;

namespace ERCHTMS.Service.KbsDeviceManage
{
    /// <summary>
    /// 描 述：标签上下线日志
    /// </summary>
    public class LableonlinelogService : RepositoryFactory<LableonlinelogEntity>, LableonlinelogIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<LableonlinelogEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public LableonlinelogEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 根据标签ID获取在线标签
        /// </summary>
        /// <param name="LableId"></param>
        /// <returns></returns>
        public LableonlinelogEntity GetOnlineEntity(string LableId)
        {
            return this.BaseRepository().IQueryable(it => it.IsOut == 0 && it.LableId == LableId).FirstOrDefault();
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
        public void SaveForm(string keyValue, LableonlinelogEntity entity)
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
        /// 储存上下线数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool SaveStatus(LableonlinelogEntity entity)
        {
            bool flag = true;
            //开始事物
            var res = DbFactory.Base().BeginTrans();
            try
            {
                if (entity.IsOut == 0)
                {
                    res.Insert<LableonlinelogEntity>(entity);
                }
                else
                {

                    List<PersononlineEntity> plist = new List<PersononlineEntity>();
                    int hour = (Convert.ToDateTime(entity.OutTime) - Convert.ToDateTime(entity.CreateDate)).Hours;
                    for (int i = 0; i <= hour; i++)
                    {
                        PersononlineEntity person = new PersononlineEntity();
                        person.ID = Guid.NewGuid().ToString();
                        person.TimeNum = Convert.ToDateTime(entity.CreateDate).AddHours(i);
                        person.UserId = entity.UserId;
                        person.UserName = entity.UserName;
                        person.DeptId = entity.DeptId;
                        person.DeptName = entity.DeptName;
                        person.DeptCode = entity.DeptCode;
                        person.OnlineHour = person.TimeNum.Value.Hour.ToString();
                        person.OnlineDate = person.TimeNum.Value.ToString("yyyy-MM-dd");
                        person.LogId = entity.ID;
                        person.LogType = 0;
                        person.CreateDate = DateTime.Now;
                        person.CreateUserId = "system";
                        person.CreateUserDeptCode = "00";
                        person.CreateUserOrgCode = "00";
                        plist.Add(person);
                    }

                    res.Insert<PersononlineEntity>(plist);
                    res.Update<LableonlinelogEntity>(entity);
                    

                }
                res.Commit();
            }
            catch (Exception e)
            {
                res.Rollback();
                flag = false;
            }

            return flag;
        }

        #endregion
    }
}
