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
    public class StandardApplyService : RepositoryFactory<StandardApplyEntity>, StandardApplyIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<StandardApplyEntity> GetList(string queryJson)
        {
            var sql = string.Format("select * from HRS_STANDARDAPPLY where 1=1 {0}", queryJson);
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
                pagination.p_fields = @"createdate,createuserid,createuserdeptcode,createuserorgcode,modifydate,modifyuserid,filename,editperson,editdeptid,editdeptname,editdate,remark,checkdeptid,checkdeptname,checkuserid,checkusername,flowstate,case when flowstate='����������' then 1 when flowstate='����' then 3 else 2 end num";
            }
            pagination.p_kid = "id";
            pagination.p_tablename = @"hrs_standardapply";
            pagination.conditionJson = string.Format(" createuserorgcode='{0}'",user.OrganizeCode);  
            //Υ������ 
            if (!queryParam["filename"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and filename like '%{0}%'", queryParam["filename"].ToString());
            }
            //����״̬
            var flowstate = "";
            if (!queryParam["flowstate"].IsEmpty())
            {
                flowstate = queryParam["flowstate"].ToString();
                pagination.conditionJson += string.Format(@" and flowstate ='{0}'", flowstate);
            }
            //���ݷ�Χ
            if (!queryParam["datascope"].IsEmpty())
            {
                var datascope = queryParam["datascope"].ToString();
                var ismeWhere = "";
                if (datascope=="1")
                {//�����������
                    ismeWhere = string.Format(@" and createuserid ='{0}'", user.UserId);
                }
                else if(datascope == "2")
                {//�Ҵ��������      
                    ismeWhere = string.Format(@" and instr(checkuserid,'{0}')>0", user.UserId);
                }
                pagination.conditionJson += ismeWhere;
            }
            //��ҳ������������
            if (!queryParam["indextype"].IsEmpty())
            {
                var indextype = queryParam["indextype"].ToString();                
                if (indextype == "1")
                {//��������
                    pagination.conditionJson += string.Format(@" and createuserid ='{0}' and flowstate='����������' and exists(select 1 from hrs_standardcheck where recid=hrs_standardapply.id)", user.UserId);
                }
                else if (indextype == "2")
                {//����ˣ�����      
                    pagination.conditionJson += " and (";
                    pagination.conditionJson += string.Format(@" (instr(checkuserid,'{0}')>0 and flowstate in('1�����','2�����','����'))", user.UserId);
                    pagination.conditionJson += string.Format(@" or (flowstate ='���Ż�ǩ' and instr(checkuserid,'{0}')>0 and instr(checkusername,'{1}(��ǩ)')<1)", user.UserId, user.UserName);
                    pagination.conditionJson += string.Format(@" or (flowstate ='��ί�����' and instr(checkuserid,'{0}')>0 and instr(checkusername,'{1}(����)')<1)", user.UserId, user.UserName);
                    pagination.conditionJson += ")";
                }
                else if (indextype == "3")
                {//������      
                    pagination.conditionJson += string.Format(@" and instr(checkuserid,'{0}')>0 and flowstate in('��˷����ǩ','�����ί��')", user.UserId);
                }                
            }
            var dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);

            return dt;
        }
        /// <summary>
        /// ͳ����ҳ������������
        /// </summary>
        /// <param name="indextype">1���������룬2������ˣ�������3��������</param>
        /// <returns></returns>
        public int CountIndex(string indextype)
        {
            int r = 0;

            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string sql = string.Format("select count(1) from HRS_STANDARDAPPLY where createuserorgcode='{0}'", user.OrganizeCode);
            switch (indextype)
            {
                case "1":
                    sql += string.Format(@" and createuserid ='{0}' and flowstate='����������' and exists(select 1 from hrs_standardcheck where recid=hrs_standardapply.id)", user.UserId); 
                    break;
                case "2":
                    sql += " and (";
                    sql += string.Format(@" (instr(checkuserid,'{0}')>0 and flowstate in('1�����','2�����','����'))", user.UserId);
                    sql += string.Format(@" or (flowstate ='���Ż�ǩ' and instr(checkuserid,'{0}')>0 and instr(checkusername,'{1}(��ǩ)')<1)", user.UserId,user.UserName);
                    sql += string.Format(@" or (flowstate ='��ί�����' and instr(checkuserid,'{0}')>0 and instr(checkusername,'{1}(����)')<1)",user.UserId, user.UserName);
                    sql += ")";
                    break;
                case "3":
                    sql += string.Format(@" and instr(checkuserid,'{0}')>0 and flowstate in('��˷����ǩ','�����ί��')", user.UserId);
                    break;
            }
            object obj = this.BaseRepository().FindObject(sql);
            int.TryParse(obj.ToString(), out r);

            return r;
        }
        /// <summary>
        /// ��ȡ�������ڵ�
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public DataTable GetWorkDetailList(string objectId)
        {
            DatabaseType dataType = DbHelper.DbType;
            string sql = string.Format("select * from sys_wftbactivity where processid='{0}' order by autoid asc", objectId);
            var dt = this.BaseRepository().FindTable(sql);

            return dt;
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public StandardApplyEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, StandardApplyEntity entity)
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
