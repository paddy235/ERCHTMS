using ERCHTMS.Entity.ComprehensiveManage;
using ERCHTMS.IService.ComprehensiveManage;
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
using ERCHTMS.Entity.SystemManage.ViewModel;
using ERCHTMS.Service.SystemManage;

namespace ERCHTMS.Service.ComprehensiveManage
{
    /// <summary>
    /// �� ������Ϣ���ͱ�
    /// </summary>
    public class InfoSubmitService : RepositoryFactory<InfoSubmitEntity>, InfoSubmitIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<InfoSubmitEntity> GetList(string queryJson)
        {
            var sql = string.Format("select * from hrs_infosubmit where 1=1 {0}", queryJson);
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
                pagination.p_fields = @"createdate,createuserid,createuserdeptcode,createuserorgcode,createusername,modifydate,modifyuserid,infoname,require,starttime,endtime,submituserid,submiteduserid,submituseraccount,submitusername,submitdepartid,submitdepartname,pct,remnum,remusername,remdepartname,issubmit,infotype";
            }
            pagination.p_kid = "id";
            pagination.p_tablename = @"hrs_infosubmit";
            pagination.conditionJson = "1=1";

            //��˾���û���EHS�����û��鿴ȫ������
            DataItemModel ehsDepart = new DataItemDetailService().GetDataItemListByItemCode("'EHSDepartment'").Where(p => p.ItemName == user.OrganizeId).ToList().FirstOrDefault();
            if (user.RoleName.Contains("��˾���û�") || (ehsDepart != null && ehsDepart.ItemValue == user.DeptCode))
            {
                pagination.conditionJson += string.Format(" and createuserorgcode='{0}' and (issubmit='��' or createuserid='{1}')", user.OrganizeCode, user.UserId);
            }
            else if (user.RoleName.Contains("������") || user.RoleName.Contains("��ȫ����Ա"))
            {//�����ż��Ӳ�������
                //pagination.conditionJson += string.Format(@" and issubmit='��' and instr(submitdepartid,'{0}')>0", user.DeptId);//����������
                pagination.conditionJson += string.Format(@" and issubmit='��' and id in(select distinct(id) from hrs_infosubmit join base_department d on instr(submitdepartid,d.departmentid)>0 where d.encode like '{0}%')", user.DeptCode);
            }
            else
            {//������Ա�鿴�Լ���ص�����
                pagination.conditionJson += string.Format(@" and issubmit='��' and instr(submituserid,'{0}')>0", user.UserId);
            }
            //��ʼʱ��
            if (!queryParam["starttime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and starttime >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", queryParam["starttime"].ToString());
            }
            //����ʱ��
            if (!queryParam["endtime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and starttime < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(queryParam["endtime"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
            }
            //����
            if (!queryParam["infotype"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and infotype = '{0}'", queryParam["infotype"].ToString());
            }
            //��������
            if (!queryParam["infoname"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and infoname like '%{0}%'", queryParam["infoname"].ToString());
            }
            //��ҳ��ת
            if (!queryParam["indextype"].IsEmpty() && queryParam["indextype"].ToString()=="1")
            {
                pagination.conditionJson += string.Format(@" and instr(submituserid,'{0}')>0 and (submiteduserid is null or instr(submiteduserid,'{0}')<=0)", user.UserId);
            }
            var dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);

            return dt;
        }
        /// <summary>
        /// ��ҳ����
        /// </summary>
        /// <param name="indexType">�������ͣ�1�����Ϣ��2����������Ҫ��</param>
        /// <returns></returns>
        public int CountIndex(string indexType)
        {
            int num = 0;

            if (indexType == "1")
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                string sql = string.Format("select count(1) from hrs_infosubmit where instr(submituserid,'{0}')>0 and (submiteduserid is null or instr(submiteduserid,'{0}')<=0)", user.UserId);
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
        public InfoSubmitEntity GetEntity(string keyValue)
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
            this.BaseRepository().Delete(x => x.ID == keyValue);
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, InfoSubmitEntity entity)
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
