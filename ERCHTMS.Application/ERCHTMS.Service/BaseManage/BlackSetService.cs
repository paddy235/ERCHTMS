using BSFramework.Data.Repository;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using BSFramework.Util.Extension;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using BSFramework.Data;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.Service.AuthorizeManage;
using ERCHTMS.IService.AuthorizeManage;

namespace ERCHTMS.Service.BaseManage
{
    /// <summary>
    /// 描 述：黑名单条件设置
    /// </summary>
    public class BlackSetService : RepositoryFactory<BlackSetEntity>, IBlackSetService
    {
        #region 获取数据
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BlackSetEntity> GetList(string deptCode)
        {
            var expression = LinqExtensions.True<BlackSetEntity>();
            expression = expression.And(t => t.DeptCode == deptCode);
            return this.BaseRepository().IQueryable(expression).OrderBy(t => t.SortCode).ToList();
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public BlackSetEntity GetAgeRange(string deptCode)
        {
            var expression = LinqExtensions.True<BlackSetEntity>();
            expression = expression.And(t => t.DeptCode == deptCode && t.ItemCode == "01" && t.Status == 1);
            return this.BaseRepository().IQueryable(expression).FirstOrDefault();
        }

        #endregion

        #region 验证数据

        #endregion

        #region 提交数据
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="list">实体</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, List<BlackSetEntity> list)
        {
            string deptCode= list[0].DeptCode;
            List<BlackSetEntity> listBlacks = this.BaseRepository().IQueryable(t => t.DeptCode == deptCode).ToList();
            if (listBlacks.Count>0)
            {
                this.BaseRepository().Delete(listBlacks);
            }
            this.BaseRepository().Insert(list);
        }
        #endregion
    }
}
