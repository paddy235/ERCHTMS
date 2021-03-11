using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.IService.RiskDatabase;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Entity.SafetyLawManage;
using ERCHTMS.IService.SafetyLawManage;
namespace ERCHTMS.Service.RiskDatabase
{
    /// <summary>
    /// 描 述：隐患标准库
    /// </summary>
    public class SafetyLawClassService : RepositoryFactory<SafetyLawClassEntity>, SafetyLawClassIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SafetyLawClassEntity> GetList(string queryJson)
        {
            //return this.BaseRepository().IQueryable().ToList();
            string sql = "select * from BIS_SAFETYLAWCLASS where 1=1 ";
            if (!queryJson.IsEmpty())
                sql += queryJson;

            return this.BaseRepository().FindList(sql).ToList();
        }
        /// <summary>
        /// 判断节点下有无子节点数据
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public bool IsHasChild(string parentId)
        {
            return this.BaseRepository().FindObject(string.Format("select count(1) from BIS_SAFETYLAWCLASS where parentid='{0}'", parentId)).ToInt() > 0 ? true : false;
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SafetyLawClassEntity GetEntity(string keyValue)
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
            var entity = BaseRepository().FindEntity(keyValue);
            if (entity != null)
            {
                string code = entity.EnCode;
                if (this.BaseRepository().Delete(keyValue) > 0)
                {
                    this.BaseRepository().ExecuteBySql(string.Format("delete from BIS_SAFETYLAWCLASS where encode like '{0}%'", code));
                    this.BaseRepository().ExecuteBySql(string.Format("delete from bis_safetylaw where lawtypecode like '{0}%'", code));
                }
            }

        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, SafetyLawClassEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                var node = this.BaseRepository().FindEntity(entity.Parentid);
                string enCode = node == null ? "" : node.EnCode;
                int count = BaseRepository().FindObject(string.Format("select count(1) from BIS_SAFETYLAWCLASS where parentid='{0}'", entity.Parentid)).ToInt();
                int level = BaseRepository().FindObject(string.Format("select lev from BIS_SAFETYLAWCLASS where id='{0}'", entity.Parentid)).ToInt();
                count++;
                level++;
                if (count.ToString().Length < 2)
                {
                    enCode += "00" + count;
                }
                else if (count.ToString().Length >= 2 && count.ToString().Length < 3)
                {
                    enCode += "0" + count;
                }
                else
                {
                    enCode += count.ToString();
                }
                entity.EnCode = enCode;
                entity.lev = level;
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }
        #endregion
    }
}
