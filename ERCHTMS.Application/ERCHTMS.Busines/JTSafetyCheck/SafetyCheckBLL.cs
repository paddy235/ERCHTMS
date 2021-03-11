using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Linq;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Entity.CarManage;
using Newtonsoft.Json;
using ERCHTMS.IService.JTSafetyCheck;
using ERCHTMS.Service.JTSafetyCheck;
using ERCHTMS.Entity.JTSafetyCheck;
using System.Data;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.SystemManage;

namespace ERCHTMS.Busines.JTSafetyCheck
{
    /// <summary>
    /// 描 述：康巴什门禁管理
    /// </summary>
    public class JTSafetyCheckBLL
    {
        private JTSafetyCheckIService service = new JTSafetyCheckService();

        #region 获取数据
        /// <summary>
        /// 获取全部列表数据
        /// </summary>
        /// <returns>返回分页列表</returns>
        public List<SafetyCheckEntity> GetPageList(string queryJson)
        {
            var list = service.GetPageList();
            if (!string.IsNullOrEmpty(queryJson))
            {
                var queryParam = queryJson.ToJObject();
                
            }
            return list;
        }
         public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);

        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SafetyCheckEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
         public DataTable GetItemsList(string checkId,string status="")
        {
            return service.GetItemsList(checkId,status);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SafetyCheckEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        public CheckItemsEntity GetItemEntity(string keyValue)
        {
            return service.GetItemEntity(keyValue);
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
        public void RemoveItemForm(string keyValue)
        {
            service.RemoveItemForm(keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public bool SaveForm(string keyValue, SafetyCheckEntity entity)
        {
            try
            {
                 return service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool SaveItemForm(string keyValue, CheckItemsEntity entity)
        {
            try
            {
                service.SaveItemForm(keyValue, entity);
                return true;
            }
            catch(Exception ex)
            {
                return false;

            }
        }
        public bool Save(string keyValue,SafetyCheckEntity entity, List<CheckItemsEntity> items)
        {
            return Save(keyValue, entity, items);
        }
        public bool SaveItems(List<CheckItemsEntity> items)
        {
            return service.SaveItems(items);
        }

        public void SendMessage(string chkId)
        {
            List<string> lstIds = new List<string>();
            MessageBLL msgBll = new MessageBLL();
            DepartmentBLL deptBll=new DepartmentBLL();
            DataTable dt = deptBll.GetDataTable(string.Format("select id, account,realname,ItemName,PlanDate,Measures from jt_checkitems a right join base_user u on a.dutyuserid=u.userid where a.checkid='{0}' and a.issend=0 and RealityDate is null", chkId));
            foreach (DataRow dr in dt.Rows)
            {
                string account = dr["account"].ToString();
                string userName = dr["realname"].ToString();
                string content = string.Format("您有一条检查问题为“{0}”需要整改，请在{1}之前按“{2}”要求进行整改。",dr["ItemName"],dr["PlanDate"],dr["Measures"]);
                MessageEntity msg = new MessageEntity
                {
                    Title = "您有一条检查问题需要整改",
                    Category = "其它",
                    Content =content,
                    UserId = account,
                    UserName = userName,
                    Status = "",
                    Url = string.Format("/SaftyCheck/JTSafetyCheck/Form?keyValue={0}&action=show", chkId),
                    SendUser ="System",
                    SendUserName = "系统管理员",
                    RecId = chkId
                };
                bool result=msgBll.SaveForm("", msg);
                if(result)
                {
                    lstIds.Add(dr["id"].ToString());
                }
            }
            if(lstIds.Count>0)
            {
                deptBll.ExecuteSql(string.Format("update jt_checkitems set issend=1 where id in('{0}')",string.Join("','",lstIds)));
            }
        }
        #endregion
    }
}
