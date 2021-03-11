using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.WebControl;
using ERCHTMS.Code;
using ERCHTMS.Entity.NosaManage;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ERCHTMS.IService.NosaManage;
using ERCHTMS.Service.SystemManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ERCHTMS.Service.NosaManage
{
    /// <summary>
    /// �� ������������
    /// </summary>
    public class NosaworksService : RepositoryFactory<NosaworksEntity>, NosaworksIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<NosaworksEntity> GetList(string queryJson)
        {
            var sql = string.Format("select * from hrs_nosaworks where 1=1 {0}", queryJson);
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
                pagination.p_fields = @"createdate,createuserid,createuserdeptcode,createuserorgcode,modifydate,modifyuserid,name,according,ratenum,enddate,dutyuserid,dutyusername,dutydepartid,dutydepartname,submituserid,submitusername,eleno,elename,eledutyuserid,eledutyusername,eledutydepartname,issubmited,pct,dutyuserhtml,dutydeparthtml";
                pagination.p_fields += ",(select wm_concat(to_char(name)) from HRS_NOSAWorkResult r where r.workid=HRS_NOSAWorks.Id) workresult";
                //pagination.p_fields += ",(select wm_concat(to_char(dutyusername)) from HRS_NOSAWorkitem r where r.workid=HRS_NOSAWorks.Id and r.state='ͨ��') checkedusername";
                pagination.p_fields += ",(select count(1) from HRS_NOSAWorkitem r where r.workid=HRS_NOSAWorks.Id and r.state='�����') checkcount";
                pagination.p_fields += ",(select case when (state='���ϴ�' or state='��ͨ��') then 1 when state='�����' then 2 else 3 end from hrs_nosaworkitem i where i.workid=hrs_nosaworks.id and i.dutyuserid='" + user.UserId + "' and rownum<=1) as state";
                pagination.p_fields += ",(select state || '|' || to_char(uploaddate,'YYYY-MM-DD') from hrs_nosaworkitem i where i.workid=hrs_nosaworks.id and i.dutyuserid='" + user.UserId + "' and rownum<=1) as itemcol";
            }                                                                                                                        
            pagination.p_kid = "id";                                                                                                   
            pagination.p_tablename = @"hrs_nosaworks";
            pagination.conditionJson = "1=1";

            if (!queryParam["datascope"].IsEmpty() && queryParam["datascope"].ToString()=="1")
            {//���ϴ�ʱ�����ݷ�Χ
                pagination.conditionJson += string.Format(" and createuserorgcode='{0}' and issubmited='��' and instr(dutyuserid,'{1}')>0", user.OrganizeCode, user.UserId);
                if (!queryParam["waitforupload"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and (submituserid is null or instr(submituserid,'{0}')<=0)", user.UserId);
                }
            }
            else
            {//NOSA�����嵥�б�Ĭ�����ݷ�Χ                
                DataItemModel ehsDepart = new DataItemDetailService().GetDataItemListByItemCode("'EHSDepartment'").Where(p => p.ItemName == user.OrganizeId).ToList().FirstOrDefault();
                if (user.RoleName.Contains("��˾���û�") || (ehsDepart != null && ehsDepart.ItemValue == user.DeptCode))
                {//��˾���û���EHS�����û��鿴ȫ������
                    pagination.conditionJson += string.Format(" and ((createuserorgcode='{0}' and issubmited='��') or createuserid='{1}')", user.OrganizeCode, user.UserId);
                }
                else if (user.RoleName.Contains("������") || user.RoleName.Contains("��ȫ����Ա"))
                {//�����ż��Ӳ�������
                    //pagination.conditionJson += string.Format(@" and createuserdeptcode like '{0}%' and (issubmited='��' or createuserid='{1}')", user.DeptCode, user.UserId);
                    pagination.conditionJson += string.Format(@" and (((createuserdeptcode like '{0}%' or instr(dutyuserid,'{1}')>0) and issubmited='��') or createuserid='{1}')", user.DeptCode, user.UserId);
                }
                else
                {//�Լ���������˵�����
                    //pagination.conditionJson += string.Format(@" and createuserid='{0}'", user.UserId);
                    pagination.conditionJson += string.Format(@" and (createuserid='{0}' or (issubmited='��' and instr(dutyuserid,'{0}')>0))", user.UserId);
                }
            }
            //�ļ�����                                                                                                            
            if (!queryParam["name"].IsEmpty())                                                                                
            {                                                                                                                   
                pagination.conditionJson += string.Format(@" and name like '%{0}%'", queryParam["name"].ToString());      
            }                                                                                                                   
            //����id                                                                                                                     
            if (!queryParam["eleid"].IsEmpty())                                                                                          
            {                                                                                                                          
                pagination.conditionJson += string.Format(@" and eleid = '{0}'", queryParam["eleid"].ToString());                
            }
            //��ʼʱ��
            if (!queryParam["starttime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and enddate >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", queryParam["starttime"].ToString());
            }
            //����ʱ��
            if (!queryParam["endtime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and enddate < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(queryParam["endtime"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
            }
            //���ݷ�Χ                                                                                                                     
            if (!queryParam["datascope"].IsEmpty())
            {
                var datascope = queryParam["datascope"].ToString();                
                if (datascope == "2")
                {//�Ҵ���������
                    pagination.conditionJson += string.Format(" and createuserid='{0}'", user.UserId);
                }
                else if (datascope == "3")
                {//��Ӧ��˵�����
                    pagination.conditionJson += string.Format(" and issubmited='��' and eledutyuserid='{0}' and exists(select 1 from hrs_nosaworkitem i where i.workid=hrs_nosaworks.id and i.state='�����')", user.UserId);
                }
            }
            var dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);                                       
                                                                                                                                        
            return dt;                                                                                                                  
        }
        /// <summary>
        /// ��ҳ����
        /// </summary>
        /// <param name="indexType">�������ͣ�1����Ӧ�ϴ���3����Ӧ��ˣ�</param>
        /// <returns></returns>
        public int CountIndex(string indexType)
        {
            int num = 0;

            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (indexType == "1")
            {                
                string sql = string.Format("select count(1) from hrs_nosaworks where createuserorgcode='{0}' and issubmited='��' and instr(dutyuserid,'{1}')>0 and (submituserid is null or instr(submituserid,'{1}')<=0)", user.OrganizeCode, user.UserId);
                object obj = this.BaseRepository().FindObject(sql);
                int.TryParse(obj.ToString(), out num);
            }
            else if (indexType == "3")
            {
                string sql = string.Format("select count(1) from hrs_nosaworks where createuserorgcode='{0}' and issubmited='��' and eledutyuserid='{1}' and exists(select 1 from hrs_nosaworkitem i where i.workid=hrs_nosaworks.id and i.state='�����')", user.OrganizeCode, user.UserId);
                object obj = this.BaseRepository().FindObject(sql);
                int.TryParse(obj.ToString(), out num);
            }

            return num;
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public NosaworksEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, NosaworksEntity entity)
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
