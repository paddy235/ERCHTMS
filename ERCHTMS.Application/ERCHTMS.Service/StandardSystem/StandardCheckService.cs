using ERCHTMS.Entity.StandardSystem;
using ERCHTMS.IService.StandardSystem;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util.Extension;
using BSFramework.Util;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Service.BaseManage;
using ERCHTMS.Service.AuthorizeManage;
using ERCHTMS.Code;

namespace ERCHTMS.Service.StandardSystem
{
    /// <summary>
    /// �� ������׼�ޱ�����
    /// </summary>
    public class StandardCheckService : RepositoryFactory<StandardCheckEntity>, StandardCheckIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<StandardCheckEntity> GetList(string queryJson)
        {
            var sql = string.Format("select * from HRS_StandardCheck where 1=1 {0}", queryJson);
            return this.BaseRepository().FindList(sql);
        }
        /// <summary>
        /// ��ȡ��ҳ�б�
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var queryParam = queryJson.ToJObject();
            if (pagination.p_fields.IsEmpty())
            {
                pagination.p_fields = @"createdate,createuserid,createuserdeptcode,createuserorgcode,modifydate,modifyuserid,checkresult,checkreason,checkdate,checkdeptid,checkdeptname,checkuserid,checkusername,checkbacktype,checktype";
            }
            pagination.p_kid = "id";
            pagination.p_tablename = @"HRS_StandardCheck";
            pagination.conditionJson = string.Format(" createuserorgcode='{0}'",user.OrganizeCode);  
            //������
            if (!queryParam["recid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and recid = '{0}'", queryParam["recid"].ToString());
            }      
            var dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);

            return dt;
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public StandardCheckEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// �Ƿ��ǩ���
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="checkUserId"></param>
        /// <returns></returns>
        public bool FinishSign(string keyValue, string checkUserId)
        {
            bool r = false;

            if (!string.IsNullOrWhiteSpace(keyValue) && !string.IsNullOrWhiteSpace(checkUserId))
            {
                var chkCount = 0;
                var chkUserList = checkUserId.Split(new char[] { ',' });
                var list = this.BaseRepository().IQueryable().Where(x => x.RecID == keyValue).OrderByDescending(x => x.CREATEDATE).ToList();
                foreach (var e in list)
                {
                    if (e.CheckType != "���Ż�ǩ")
                    {
                        break;
                    }
                    if (checkUserId.Contains(e.CheckUserId))
                    {
                        chkCount++;
                    }
                }
                r = chkUserList.Count() == chkCount;
            }

            return r;
        }
        /// <summary>
        /// ��ί������Ƿ����
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="checkUserId"></param>
        /// <returns></returns>
        public bool FinishCommittee(string keyValue, string checkUserId)
        {
            bool r = false;

            if (!string.IsNullOrWhiteSpace(keyValue) && !string.IsNullOrWhiteSpace(checkUserId))
            {
                var chkCount = 0;
                var chkUserList = checkUserId.Split(new char[] { ',' });
                var list = this.BaseRepository().IQueryable().Where(x => x.RecID == keyValue).OrderByDescending(x => x.CREATEDATE).ToList();
                foreach (var e in list)
                {
                    if (e.CheckType != "��ί�����")
                    {
                        break;
                    }
                    if (checkUserId.Contains(e.CheckUserId))
                    {
                        chkCount++;
                    }
                }
                r = chkUserList.Count() == chkCount;
            }

            return r;
        }
        /// <summary>
        /// �Ƿ�ȫ��������
        /// </summary>
        /// <returns></returns>
        public bool FinishComplete(string checkUserId, string checkUserName,string checkType)
        {
            bool r = false;

            if (!string.IsNullOrWhiteSpace(checkUserId)&&!string.IsNullOrWhiteSpace(checkUserName))
            {
                var chkCount = 0;
                var uList = new UserService().GetListForCon(x => checkUserId.Contains(x.UserId)).Select(x => x.RealName).ToList();
                foreach(var u in uList)
                {
                    if (checkUserName.Contains(string.Format("{0}({1})", u, checkType)))
                        chkCount++;
                }
                r = chkCount == uList.Count;
            }

            return r;
        }
        /// <summary>
        /// ��������༭��ȡ�������˼�¼
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="checkType"></param>
        /// <returns></returns>
        public StandardCheckEntity GetLastEntityByRecId(string keyValue,string checkType)
        {
            StandardCheckEntity result = null;

            result =  this.BaseRepository().IQueryable().Where(x => x.RecID == keyValue && x.CheckType == checkType).OrderByDescending(x => x.CREATEDATE).FirstOrDefault(); ;
           
            return result;
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(x => x.RecID == keyValue);
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, StandardCheckEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                var old = GetEntity(keyValue);
                if (old == null)
                {
                    entity.Create();
                    entity.ID = keyValue;
                    this.BaseRepository().Insert(entity);
                }
                else
                {
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }                
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
