using ERCHTMS.Entity.EquipmentManage;
using ERCHTMS.IService.EquipmentManage;
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

namespace ERCHTMS.Service.EquipmentManage
{
    /// <summary>
    /// �� ����Σ�ջ�ѧƷ���
    /// </summary>
    public class DangerChemicalsService : RepositoryFactory<DangerChemicalsEntity>, DangerChemicalsIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<DangerChemicalsEntity> GetList(string queryJson)
        {
            var sql = string.Format("select * from XLD_DANGEROUSCHEMICAL where 1=1 {0}", queryJson);
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
           

            //����code 
            if (!queryParam["DutyDeptCode"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and DutyDeptCode like '{0}%'", queryParam["DutyDeptCode"].ToString());
            }
            //Σ��Ʒ���� 
            if (!queryParam["RiskType"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and RiskType = '{0}'", queryParam["RiskType"].ToString());
            }
            //��ŵص����� 
            if (!queryParam["IsScene"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and IsScene = '{0}'", queryParam["IsScene"].ToString());
            }
            //Σ��Ʒ���� 
            if (!queryParam["Name"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and Name like '%{0}%'", queryParam["Name"].ToString());
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
        public DangerChemicalsEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// ��ȡ����˲��Ź����ƻ�
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int GetDangerChemicalsBMNum(ERCHTMS.Code.Operator user) {
            int count = 0;
            try
            {
                count = BaseRepository().FindObject(string.Format(@"select count(0)
from hrs_DangerChemicals
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
        public int GetDangerChemicalsGRNum(ERCHTMS.Code.Operator user)
        {
            int count = 0;
            try
            {
                count = BaseRepository().FindObject(string.Format(@"select count(0)
from hrs_DangerChemicals
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
            var old = GetEntity(keyValue);
            if (old != null)
            {
                old.IsDelete = 1;  //ɾ��
                this.BaseRepository().Update(old);
            }
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, DangerChemicalsEntity entity)
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
