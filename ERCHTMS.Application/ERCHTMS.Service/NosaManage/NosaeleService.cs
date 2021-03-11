using ERCHTMS.Entity.NosaManage;
using ERCHTMS.IService.NosaManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using ERCHTMS.Code;
using BSFramework.Util;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.NosaManage
{
    /// <summary>
    /// �� ����Ԫ�ر�
    /// </summary>
    public class NosaeleService : RepositoryFactory<NosaeleEntity>, NosaeleIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<NosaeleEntity> GetList(string queryJson)
        {
            var sql = string.Format("select * from hrs_nosaele where 1=1  and state!=1 {0}", queryJson);
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
                pagination.p_fields = @"createdate,createuserid,createuserdeptcode,createuserorgcode,modifydate,modifyuserid,no,name,dutyuserid,dutyusername,dutydepartid,dutydepartname";
            }
            pagination.p_kid = "id";
            pagination.p_tablename = @"hrs_nosaele";
            pagination.conditionJson = string.Format(" createuserorgcode='{0}' and state!=1", user.OrganizeCode);
            //���
            if (!queryParam["no"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and no like '%{0}%'", queryParam["no"].ToString());
            }
            //����
            if (!queryParam["name"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and name like '%{0}%'", queryParam["name"].ToString());
            }
            if (!queryParam["DutyPerson"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and dutyusername like '%{0}%'", queryParam["DutyPerson"].ToString());
            }
            if (!queryParam["DutyDept"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and dutydepartname like '%{0}%'", queryParam["DutyDept"].ToString());
            }
            //������id
            if (!queryParam["dutyuserid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and dutyuserid = '{0}'", queryParam["dutyuserid"].ToString());
            }
            var dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);

            return dt;
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public NosaeleEntity GetEntity(string keyValue)
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
            var resp = this.BaseRepository();
            var sql = string.Format("update hrs_nosaele set state=1 where id in(select id from hrs_nosaele start with id='{0}' connect by  prior id = parentid)", keyValue);
            resp.ExecuteBySql(sql);
            //resp.BeginTrans();
            //try
            //{
            //    //ɾ�������ɹ����
            //    string sql = string.Format("delete from hrs_nosaworkitem where workid in(select id from hrs_nosaworks where eleid in(select id from hrs_nosaele start with id='{0}' connect by  prior id = parentid))", keyValue);
            //    resp.ExecuteBySql(sql);                
            //    //ɾ�������ɹ���Ŀ
            //    sql = string.Format("delete from hrs_nosaworkresult where workid in(select id from hrs_nosaworks where eleid in(select id from hrs_nosaele start with id='{0}' connect by  prior id = parentid))", keyValue);
            //    resp.ExecuteBySql(sql);
            //    //ɾ�������嵥
            //    sql = string.Format("delete from hrs_nosaworks where eleid in(select id from hrs_nosaele start with id='{0}' connect by  prior id = parentid)", keyValue);
            //    resp.ExecuteBySql(sql);
            //    //ɾ��Ԫ��
            //    sql = string.Format("delete from hrs_nosaele where id in(select id from hrs_nosaele start with id='{0}' connect by  prior id = parentid)", keyValue);
            //    resp.ExecuteBySql(sql);
            //    resp.Commit();
            //}
            //catch
            //{
            //    resp.Rollback();
            //}
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, NosaeleEntity entity)
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
