using ERCHTMS.Entity.PersonManage;
using ERCHTMS.IService.PersonManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ERCHTMS.Service.PersonManage
{
    /// <summary>
    /// 描 述：用户门禁数据类
    /// </summary>
    public class AcesscontrolinfoService : RepositoryFactory<AcesscontrolinfoEntity>, AcesscontrolinfoIService
    {
        #region 获取数据

        /// <summary>
        /// 获取用户关联指纹人脸数据
        /// </summary>
        /// <param name="userids"></param>
        /// <returns></returns>
        public DataTable GetUserList(string userids)
        {
            string[] users=userids.Split(',');
            string userid = "";
            for (int i = 0; i < users.Length; i++)
            {
                if (i == 0)
                {
                    userid = "'" + users[i] + "'";
                }
                else
                {
                    userid += ",'" + users[i] + "'";
                }
            }
            string sql = string.Format(@"select u.userid,realname,mobile,isfinger,isface,tid from base_user u
            left join BIS_ACESSCONTROLINFO ace on u.userid=ace.userid
            where u.userid in ({0})", userid);
            return BaseRepository().FindTable(sql);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<AcesscontrolinfoEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public AcesscontrolinfoEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, AcesscontrolinfoEntity entity)
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
