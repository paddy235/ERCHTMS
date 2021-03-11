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
    /// �� ��������ʲ�Ž�����
    /// </summary>
    public class JTSafetyCheckBLL
    {
        private JTSafetyCheckIService service = new JTSafetyCheckService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡȫ���б�����
        /// </summary>
        /// <returns>���ط�ҳ�б�</returns>
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
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SafetyCheckEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
         public DataTable GetItemsList(string checkId,string status="")
        {
            return service.GetItemsList(checkId,status);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
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
        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
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
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
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
                string content = string.Format("����һ���������Ϊ��{0}����Ҫ���ģ�����{1}֮ǰ����{2}��Ҫ��������ġ�",dr["ItemName"],dr["PlanDate"],dr["Measures"]);
                MessageEntity msg = new MessageEntity
                {
                    Title = "����һ�����������Ҫ����",
                    Category = "����",
                    Content =content,
                    UserId = account,
                    UserName = userName,
                    Status = "",
                    Url = string.Format("/SaftyCheck/JTSafetyCheck/Form?keyValue={0}&action=show", chkId),
                    SendUser ="System",
                    SendUserName = "ϵͳ����Ա",
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
