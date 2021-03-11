using ERCHTMS.Entity.MessageManage;
using ERCHTMS.IService.MessageManage;
using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util.Extension;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace ERCHTMS.Service.MessageManage
{
    /// <summary>
    /// 描 述：即时通信用户管理
    /// </summary>
    public class IMUserService : RepositoryFactory, IMsgUserService
    {
        /// <summary>
        /// 获取联系人列表（即时通信）
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IMUserModel> GetList(string OrganizeId)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"SELECT  u.UserId ,
                                    u.RealName ,
                                    o.FullName AS OrganizeId ,
                                    d.FullName AS DepartmentId ,
                                    u.HeadIcon  
                            FROM    Base_User u
                                    LEFT JOIN Base_Organize o ON o.OrganizeId = u.OrganizeId
                                    LEFT JOIN Base_Department d ON d.DepartmentId = u.DepartmentId
                            WHERE   1=1");
            var parameter = new List<DbParameter>();
            //公司主键
            if (!OrganizeId.IsEmpty())
            {
                strSql.Append(" AND u.OrganizeId = @OrganizeId");
                parameter.Add(DbParameters.CreateDbParameter("@OrganizeId", OrganizeId));
            }
            //strSql.Append(" AND u.UserId <> 'System'");
            strSql.Append(" order by d.FullName");
            return this.BaseRepository().FindList<IMUserModel>(strSql.ToString(), parameter.ToArray());
        }
    }
}
