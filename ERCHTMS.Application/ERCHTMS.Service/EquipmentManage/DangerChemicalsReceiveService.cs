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
    /// �� ����Σ�ջ�ѧƷ����
    /// </summary>
    public class DangerChemicalsReceiveService : RepositoryFactory<DangerChemicalsReceiveEntity>, DangerChemicalsReceiveIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<DangerChemicalsReceiveEntity> GetList(string queryJson)
        {
            var sql = string.Format("select * from xld_DangerChemicalsReceive where 1=1 {0}", queryJson);
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
            if (!queryParam["deptCode"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and t.createuserdeptcode like '{0}%'", queryParam["deptCode"].ToString());
            }
            //Σ��Ʒ���� 
            if (!queryParam["Name"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and ((t1.Name like '%{0}%') or (t.ReceiveUser like '%{0}%'))", queryParam["Name"].ToString());
            }
            //Σ��ƷID
            if (!queryParam["MainId"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and t.MainId='{0}'", queryParam["MainId"].ToString());
            }
            if (!queryParam["mode"].IsEmpty())
            {
                if (queryParam["mode"].ToString() == "whply")
                {
                    string ids = GetLyCheckList(user);
                    if (ids.Length > 0)
                    {
                        ids = ids.TrimEnd(',');
                        pagination.conditionJson += string.Format(@" and t.id in({0})", ids);
                    }
                    else
                    {
                        pagination.conditionJson += string.Format(@" and t.id=''");
                    }
                }

            }
            var dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);

            return dt;
        }
        public string GetLyCheckList(ERCHTMS.Code.Operator user)
        {
            string ids = "";
            DataTable dt = this.BaseRepository().FindTable(string.Format(@"select * from XLD_DANGEROUSCHEMICALRECEIVE t"));
            int count = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (!string.IsNullOrWhiteSpace(dr["flowdept"].ToString()))
                {
                    if (dr["flowdept"].ToString() == user.DeptId)
                    {
                        if (!string.IsNullOrWhiteSpace(dr["flowrolename"].ToString()))
                        {

                            var roleArr = user.RoleName.Split(','); //��ǰ��Ա��ɫ
                            var roleName = dr["flowrolename"].ToString(); //�����ɫ
                            for (var i = 0; i < roleArr.Length; i++)
                            {
                                //������˲���ͬ��ǰ�˲���idһ�£��е�ǰ�˽�ɫ��������˽�ɫ��
                                if (roleName.IndexOf(roleArr[i]) >= 0)
                                {
                                    ids += "'" + dr["id"].ToString() + "',";
                                    break;
                                }
                            }
                        }
                    }
                }
                DataTable whpList = this.BaseRepository().FindTable(string.Format(@"select * from XLD_DANGEROUSCHEMICAL t where id='{0}'", dr["mainid"].ToString()));
                if (whpList != null && whpList.Rows.Count > 0)
                {
                    if (whpList.Rows[0]["grantpersonid"].ToString() == user.UserId)
                    {
                        if (dr["grantstate"].ToString() == "2")
                        {
                            ids += "'" + dr["id"].ToString() + "',";
                            continue;
                        }
                    }
                }
            }
            return ids;
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
        public DangerChemicalsReceiveEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// ��ȡ����˲��Ź����ƻ�
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int GetDangerChemicalsReceiveBMNum(ERCHTMS.Code.Operator user) {
            int count = 0;
            try
            {
                count = BaseRepository().FindObject(string.Format(@"select count(0)
from hrs_DangerChemicalsReceive
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
        public int GetDangerChemicalsReceiveGRNum(ERCHTMS.Code.Operator user)
        {
            int count = 0;
            try
            {
                count = BaseRepository().FindObject(string.Format(@"select count(0)
from hrs_DangerChemicalsReceive
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
            this.BaseRepository().Delete(x => x.ID == keyValue);
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, DangerChemicalsReceiveEntity entity)
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
