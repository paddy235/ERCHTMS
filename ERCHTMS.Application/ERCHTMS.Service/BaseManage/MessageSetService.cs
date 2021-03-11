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
using System.Data;

namespace ERCHTMS.Service.BaseManage
{
    /// <summary>
    /// 描 述：消息设置
    /// </summary>
    public class MessageSetService : RepositoryFactory<MessageSetEntity>, IMessageSetService
    {
        #region 获取数据

        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MessageSetEntity> GetList(string deptCode)
        {
            var expression = LinqExtensions.True<MessageSetEntity>();
            //expression = expression.And(t => t.DeptCode == deptCode);
            return this.BaseRepository().IQueryable(expression).ToList();
        }
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public  DataTable GetPageList(Pagination pagination, string queryJson)
        {
            var expression = LinqExtensions.True<DataSetEntity>();
            var queryParam = queryJson.ToJObject();
            //查询条件
            if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            {
                string condition = queryParam["condition"].ToString();
                string keyword = queryParam["keyword"].ToString();
                switch (condition)
                {
                    case "Code":            //编号
                        pagination.conditionJson+=string.Format(" and Code like '%{0}%'",keyword.Trim());
                        break;
                    case "Name":          //名称
                        pagination.conditionJson += string.Format(" and name like '%{0}%'", keyword.Trim());
                        break;
                    case "DeptName":            //单位
                        pagination.conditionJson += string.Format(" and DeptName like '%{0}%'", keyword.Trim());
                        break;
                    default:
                        break;
                }
            }
            DatabaseType dataType = DbHelper.DbType;
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public MessageSetEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 根据编号获取消息对象
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public MessageSetEntity GetEntityByCode(string code)
        {
            return this.BaseRepository().IQueryable(t => t.Code == code && t.Status == 1).FirstOrDefault();
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
        public void SaveForm(string keyValue, MessageSetEntity ds)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                ds.Modify(keyValue);
                this.BaseRepository().Update(ds);
            }
            else
            {
                ds.Create();
                this.BaseRepository().Insert(ds);
            }
             
        }
        #endregion
    }
}
