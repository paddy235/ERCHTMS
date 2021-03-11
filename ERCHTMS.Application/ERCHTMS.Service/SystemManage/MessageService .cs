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
using ERCHTMS.IService.SystemManage;
using ERCHTMS.Entity.SystemManage;
using System;
using ERCHTMS.Code;
using ERCHTMS.Service.BaseManage;

namespace ERCHTMS.Service.SystemManage
{
    /// <summary>
    /// 描 述：短消息
    /// </summary>
    public class MessageService : RepositoryFactory<MessageEntity>, IMessageService
    {
        #region 获取数据

        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MessageEntity> GetList(string sqlWhere)
        {
            //var expression = LinqExtensions.True<MessageEntity>();
            ////expression = expression.And(t => t.DeptCode == deptCode);
            //return this.BaseRepository().IQueryable(expression).ToList();

            var sql = "select * from base_message where 1=1 "+ sqlWhere;
            return this.BaseRepository().FindList(sql);
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
                    case "Name":          
                        pagination.conditionJson+=string.Format(" and username like '%{0}%'",keyword.Trim());
                        break;
                    case "Title":          
                        pagination.conditionJson += string.Format(" and title like '%{0}%'", keyword.Trim());
                        break;
                    default:
                        break;
                }
            }
            if (!queryParam["SendUser"].IsEmpty()) {
                pagination.conditionJson += string.Format(" and senduser = '{0}'", queryParam["SendUser"].ToString());
            }
            if (!queryParam["UserId"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and userid like'%{0}%'", queryParam["UserId"].ToString());
            }
           
            //根据登陆用户查询用户关注的消息设置
            Operator currUser = OperatorProvider.Provider.Current();
            string sql = string.Format(@"select setcategory from base_messageuserset t where t.createuserid='{0}' and  t.createuserorgcode='{1}'", currUser.UserId, currUser.OrganizeCode);
            DataTable dt = this.BaseRepository().FindTable(sql);
            var strWhere = string.Empty;
            if (dt.Rows.Count > 0) {
                if (!string.IsNullOrWhiteSpace(dt.Rows[0][0].ToString())) {
                    var arr = dt.Rows[0][0].ToString().Split(',');
                    strWhere += "and (";
                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (string.IsNullOrWhiteSpace(arr[i])) continue;
                        else
                        {
                            strWhere += string.Format(@" t.category  like '{0}%' or", arr[i]);
                        }
                    }
                    //strWhere = strWhere.Substring(0, strWhere.Length - 2);
                    strWhere += string.Format(@" t.category  like '{0}%'", "其它");
                    strWhere += ")"; 
                }
            }
            pagination.conditionJson += strWhere;
            DatabaseType dataType = DbHelper.DbType;

            //if (!queryParam["status"].IsEmpty())
            //{
            //    if (queryParam["status"].ToString() == "0") {
            //        pagination.sidx = "t.status";
            //        pagination.sord = "desc";
            //    }
               
            //}
            DataTable result= this.BaseRepository().FindTableByProcPager(pagination, dataType);
            //for (int i = 0; i < result.Rows.Count; i++)
            //{
            //   DataTable dtt= this.BaseRepository().FindTable(string.Format("select status from base_messagedetail d where d.useraccount='{0}' and d.messageid='{1}'", currUser.Account, result.Rows[i]["id"].ToString()));
            //   if (dtt.Rows.Count > 0)
            //   {
            //       result.Rows[i]["status"] = dtt.Rows[0][0];
            //   }
            //   else {
            //       result.Rows[i]["status"] = 0;
            //   }
            //   if (result.Rows[i]["senduser"].ToString() == currUser.Account)
            //   {
            //       result.Rows[i]["status"] = 1;
            //   }
            //}
            //if (pagination.sidx == "status")
            //{
            //    result.DefaultView.Sort = pagination.sidx + " " + pagination.sord;
            //    return result.DefaultView.ToTable();
            //}
            //else {
                return result;
            //}
           
            //if (!queryParam["status"].IsEmpty())
            //{
            //    var r = result.Select("status=0");
            //    if (r.Length == 0) return new DataTable();
            //    else return r.CopyToDataTable();
            //}
            //else {
               
            //}
          
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public MessageEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
             /// <summary>
        /// 根据用户Id获取未读消息数量
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public int GetMessCountByUserId(ERCHTMS.Code.Operator currUser)
        {
            //根据登陆用户查询用户关注的消息设置
//           var user = new UserInfoService().GetUserInfoByAccount(currUser.Account);
//            string sql = string.Format(@"select setcategory from base_messageuserset t where t.createuserid='{0}' and  t.createuserorgcode='{1}'", currUser.UserId, currUser.OrganizeCode);
//            DataTable dt = this.BaseRepository().FindTable(sql);
//            var strWhere = string.Empty;
//            if (dt.Rows.Count > 0)
//            {
//                if (!string.IsNullOrWhiteSpace(dt.Rows[0][0].ToString()))
//                {
//                    var arr = dt.Rows[0][0].ToString().Split(',');
//                    strWhere += "and (";
//                    for (int i = 0; i < arr.Length; i++)
//                    {
//                        if (string.IsNullOrWhiteSpace(arr[i])) continue;
//                        else
//                        {
//                            strWhere += string.Format(@" t.category  like '{0}%' or", arr[i]);
//                        }
//                    }
//                    //strWhere = strWhere.Substring(0, strWhere.Length - 2);
//                    strWhere += string.Format(@" t.category  like '{0}%'", "其它");
//                    strWhere += ")";
//                }
//            }
//            string Sql = string.Format(@"select count(d.id) from base_messagedetail d
//left join base_message  t on t.id=d.messageid
//where d.useraccount='{0}' and d.status=0 {1}", currUser.Account, strWhere);
            string Sql = string.Format(@"select count(d.id) from base_messagedetail d
left join base_message  t on t.id=d.messageid
where d.useraccount='{0}' and d.status=0 and (t.category like '其它%' or t.category in({1}))", currUser.Account, string.Format(@"select setcategory from base_messageuserset t where t.createuserid='{0}' and  t.createuserorgcode='{1}'", currUser.UserId, currUser.OrganizeCode));
            var dt1=this.BaseRepository().FindTable(Sql);
            if (dt1.Rows.Count > 0)
            {
                return Convert.ToInt32(dt1.Rows[0][0].ToString());
            }
            else {
                return 0;
            }
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
        public bool SaveForm(string keyValue, MessageEntity ds)
        {
            try
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
                var num = this.BaseRepository().ExecuteBySql(string.Format("select count(id) from base_messagedetail where messageid='{0}'", ds.Id));
                if (num > 0)
                {
                    this.BaseRepository().ExecuteBySql(string.Format("delete from base_messagedetail where messageid='{0}'", ds.Id));
                }
                if (!string.IsNullOrWhiteSpace(ds.UserId))
                {
                    var arr = ds.UserId.Split(new char[] {',' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < arr.Length; i++)
                    {
                        var userEntity = new UserInfoService().GetUserInfoByAccount(arr[i]);
                        if (userEntity != null)
                        {
                            
                            MessageDetail md = new MessageDetail();
                            md.UserAccount = userEntity.Account;
                            md.UserId = userEntity.UserId;
                            md.UserName = userEntity.RealName;
                            md.DeptCode = userEntity.DepartmentCode;
                            md.DeptId = userEntity.DepartmentId;
                            md.DeptName = userEntity.DeptName;
                            if (userEntity.Account == ds.SendUser)
                            {
                                md.Status = 1;
                                md.LookTime = DateTime.Now;
                            }
                            else {
                                md.Status = 0;
                            }
                            md.MessageId = ds.Id;
                            new MessageDetailService().SaveForm(md.Id, md);
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        /// <summary>
        /// 根据用户账号标记短消息已读
        /// </summary>
        /// <param name="userAccount"></param>
        public void FlagReadMessage(string userAccount)
        {
            string sql = string.Format("update base_messagedetail t set t.status=1 where t.useraccount='{0}' ", userAccount);
            this.BaseRepository().ExecuteBySql(sql);
        }

        public DataTable FindTable(string sql)
        {
            return this.BaseRepository().FindTable(sql);
        }
    }
}
