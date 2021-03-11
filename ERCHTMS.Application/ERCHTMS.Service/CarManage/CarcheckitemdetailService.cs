using System;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.IService.CarManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using ERCHTMS.Code;

namespace ERCHTMS.Service.CarManage
{
    /// <summary>
    /// 描 述：危化品车辆检查项目表
    /// </summary>
    public class CarcheckitemdetailService : RepositoryFactory<CarcheckitemdetailEntity>, CarcheckitemdetailIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<CarcheckitemdetailEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable(it=>it.PID==queryJson).OrderBy(it=>it.Sort).ToList();
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public CarcheckitemdetailEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion

        #region 提交数据

        /// <summary>
        /// 修改详情列表并提交状态
        /// </summary>
        /// <param name="detaillist"></param>
        public void Update(List<CarcheckitemdetailEntity> detaillist)
        {
            var res = DbFactory.Base().BeginTrans();
            try
            {
                Repository<HazardouscarEntity> inlogdb = new Repository<HazardouscarEntity>(DbFactory.Base());
                HazardouscarEntity haza= inlogdb.FindEntity(detaillist[0].PID);
                Operator user = OperatorProvider.Provider.Current();
                haza.HazardousProcess = 1;
                haza.ProcessingName = user.UserName;
                haza.ProcessingId = user.UserId;
                haza.ProcessingSign = user.SignImg;
                haza.Modify(detaillist[0].PID);
                for (int i = 0; i < detaillist.Count; i++)
                {
                    detaillist[i].Modify(detaillist[i].ID);
                }

                res.Update<HazardouscarEntity>(haza);
                res.Update<CarcheckitemdetailEntity>(detaillist);
                res.Commit();
            }
            catch (Exception e)
            {
                res.Rollback();
                throw;
            }
        }

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
        public void SaveForm(string keyValue, CarcheckitemdetailEntity entity)
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
        #endregion
    }
}
