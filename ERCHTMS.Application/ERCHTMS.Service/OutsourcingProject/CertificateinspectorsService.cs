using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Util;
using BSFramework.Data;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.OutsourcingProject
{
    /// <summary>
    /// 描 述：资质审查人员证件表
    /// </summary>
    public class CertificateinspectorsService : RepositoryFactory<CertificateinspectorsEntity>, CertificateinspectorsIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<CertificateinspectorsEntity> GetList(string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            //查询条件
            if (!queryParam["UserId"].IsEmpty())
            {
                return this.BaseRepository().IQueryable().ToList().Where(x => x.USERID == queryParam["UserId"].ToString());
            }
            return null;
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public CertificateinspectorsEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, CertificateinspectorsEntity entity)
        {
            int count = 0;
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.ID = keyValue;
                CertificateinspectorsEntity cert = BaseRepository().FindEntity(keyValue);
                if (cert==null)
                {
                    entity.Create();  
                    count = this.BaseRepository().Insert(entity);
                }
                else
                {
                    entity.Modify(keyValue);
                    count = this.BaseRepository().Update(entity);
                }
              
            }
            else
            {
                entity.Create();
                count = this.BaseRepository().Insert(entity);
            }
            if (count > 0)
            {
                count = int.Parse(BaseRepository().FindObject(string.Format("select count(1) from EPG_CERTIFICATEINSPECTORS where userid='{0}' and (CREDENTIALSNAME='{1}' or certtype='{1}') ", entity.USERID, "特种作业操作证")).ToString());
                if (count > 0)
                {
                    BaseRepository().ExecuteBySql(string.Format("update EPG_APTITUDEINVESTIGATEPEOPLE set isspecial='是' where id='{0}'", entity.USERID));
                }
                count = int.Parse(BaseRepository().FindObject(string.Format("select count(1) from EPG_CERTIFICATEINSPECTORS where userid='{0}' and (CREDENTIALSNAME='{1}' or certtype='{1}') ", entity.USERID, "特种设备作业人员证")).ToString());
                if (count > 0)
                {
                    BaseRepository().ExecuteBySql(string.Format("update EPG_APTITUDEINVESTIGATEPEOPLE set isspecialequ='是' where id='{0}'", entity.USERID));
                }
            }
        }
        #endregion
    }
}
