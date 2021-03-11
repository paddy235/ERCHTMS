using ERCHTMS.Entity.WorkPlan;
using ERCHTMS.IService.WorkPlan;
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
using System;

namespace ERCHTMS.Service.WorkPlan
{
    /// <summary>
    /// �� ����EHS�ƻ������
    /// </summary>
    public class PlanApplyService : RepositoryFactory<PlanApplyEntity>, PlanApplyIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<PlanApplyEntity> GetList(string queryJson)
        {
            var sql = string.Format("select * from hrs_planapply where 1=1 {0}", queryJson);
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
                pagination.p_fields = @"createdate,createuserid,createuserdeptcode,createuserorgcode,modifydate,modifyuserid,userid,username,departid,departname,applytype,applydate,flowstate,checkuseraccount,case when flowstate='�ϱ��ƻ�' then 1 when flowstate='����' then 3 else 2 end num,(select count(1) from hrs_planapply a where a.baseid=hrs_planapply.id) as changed";
            }
            pagination.p_kid = "id";
            pagination.p_tablename = @"hrs_planapply";
            pagination.conditionJson = string.Format(" createuserorgcode='{0}' and (createuserid='{1}' or instr(checkuseraccount,'{2}')>0 or flowstate='����')", user.OrganizeCode, user.UserId, user.Account);
            //��ʼʱ��
            if (!queryParam["starttime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and applydate >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", queryParam["starttime"].ToString());
            }
            //����ʱ��
            if (!queryParam["endtime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and applydate < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(queryParam["endtime"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
            }
            //���ñ�� 
            if (!queryParam["baseid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and baseid = '{0}'", queryParam["baseid"].ToString());
            }
            //����id 
            if (!queryParam["departid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and departid = '{0}'", queryParam["departid"].ToString());
            }
            //����code 
            if (!queryParam["departcode"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and exists(select 1 from base_department d where d.encode like '%{0}%' and d.departmentid =departid)", queryParam["departcode"].ToString());
            }
            //�������� 
            if (!queryParam["applytype"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and applytype = '{0}'", queryParam["applytype"].ToString());
            }
            //��Ч����
            if (!queryParam["isavailable"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and baseid is null");
            }
            //����״̬
            if (!queryParam["flowstate"].IsEmpty())
            {
                var flowstate = queryParam["flowstate"].ToString();
                if (flowstate == "1")
                {
                    pagination.conditionJson += string.Format(@" and flowstate ='{0}'", "�ϱ��ƻ�");
                }
                else if (flowstate == "2")
                {
                    pagination.conditionJson += string.Format(@" and flowstate !='{0}' and flowstate !='{1}'", "�ϱ��ƻ�", "����");
                }
                else if(flowstate=="3")
                {
                    pagination.conditionJson += string.Format(@" and flowstate ='{0}'", "����");
                }                
            }
            //���ݷ�Χ
            if (!queryParam["datascope"].IsEmpty())
            {
                var datascope = queryParam["datascope"].ToString();
                var ismeWhere = "";
                if (datascope == "1")
                {//�����������
                    ismeWhere = string.Format(@" and createuserid ='{0}'", user.UserId);
                }
                else if (datascope == "2")
                {//�Ҵ��������      
                    ismeWhere = string.Format(@" and instr(checkuseraccount,'{0}')>0", user.Account);
                }
                pagination.conditionJson += ismeWhere;
            }
            var dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);

            return dt;
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
        public PlanApplyEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// ��ȡ����˲��Ź����ƻ�
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int GetPlanApplyBMNum(ERCHTMS.Code.Operator user) {
            int count = 0;
            try
            {
                count = BaseRepository().FindObject(string.Format(@"select count(0)
from hrs_planapply
where createuserorgcode='{0}' 
and (instr(checkuseraccount,'{1}')>0 or flowstate='����') and applytype = '���Ź����ƻ�' and baseid is null
and checkuseraccount like '%{1}%'", user.OrganizeCode, user.Account)).ToInt();
            }
            catch {
                return 0;
            }
            return count;
        }
        /// <summary>
        /// ��ȡ����˸��˹����ƻ�
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int GetPlanApplyGRNum(ERCHTMS.Code.Operator user)
        {
            int count = 0;
            try
            {
                count = BaseRepository().FindObject(string.Format(@"select count(0)
from hrs_planapply
where createuserorgcode='{0}' 
and (instr(checkuseraccount,'{1}')>0 or flowstate='����') and applytype = '���˹����ƻ�' and baseid is null
and checkuseraccount like '%{1}%'", user.OrganizeCode,user.Account)).ToInt();
            }
            catch
            {
                return 0;
            }
            return count;
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(x => x.ID == keyValue || x.BaseId == keyValue);
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, PlanApplyEntity entity)
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
