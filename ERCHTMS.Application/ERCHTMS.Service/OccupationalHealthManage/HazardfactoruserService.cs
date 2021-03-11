using System;
using ERCHTMS.Entity.OccupationalHealthManage;
using ERCHTMS.IService.OccupationalHealthManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Code;

namespace ERCHTMS.Service.OccupationalHealthManage
{
    /// <summary>
    /// �� ����Σ��������Ա��
    /// </summary>
    public class HazardfactoruserService : RepositoryFactory<HazardfactoruserEntity>, HazardfactoruserIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<HazardfactoruserEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable(it => it.Hid == queryJson).ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public HazardfactoruserEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// ��ѯְҵ���Ӵ��û���
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            var curuser = OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();


            if (!queryParam["Name"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and realname='{0}'", queryParam["Name"].ToString().Trim());
            }
            if (!queryParam["DeptCode"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and DeptCode like  '{0}%'", queryParam["DeptCode"].ToString().Trim());
            }
            //����
            if (!queryParam["PostId"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and  PostId = '{0}'", queryParam["PostId"].ToString().Trim());
            }
            //���֤��
            if (!queryParam["us"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and  us like '%{0}%'", queryParam["us"].ToString().Trim());
            }

            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// ��ѯְҵ���Ӵ��û���
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetPageList(string sqlwhere, string queryJson)
        {
            var curuser = OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;

            string sql = string.Format(
                @" select u.USERID,REALNAME,MOBILE,OrganizeName,DEPTNAME,DUTYNAME,POSTNAME,usertype,GENDER,OrganizeCode,identifyid,realname as username,DEPARTURETIME,u.deptcode,nature,u.DEPARTMENTCODE,organizeid,u.IsTransfer,u.DepartureReason,us from v_userinfo u left join (select userid,username,LISTAGG(riskvalue,',') WITHIN GROUP (ORDER BY riskvalue) AS us from (  select userid,username,riskvalue from BIS_HAZARDFACTORUSER hauser left join BIS_HAZARDFACTORs ha on ha.hid=hauser.hid where 1=1 group by userid,username,riskvalue) f  group by userid,username) t on u.account=t.userid where ");

            var queryParam = queryJson.ToJObject();


            if (!queryParam["Name"].IsEmpty())
            {
                sqlwhere += string.Format(" and realname='{0}'", queryParam["Name"].ToString().Trim());
            }
            if (!queryParam["DeptCode"].IsEmpty())
            {
                sqlwhere += string.Format(" and DeptCode like  '{0}%'", queryParam["DeptCode"].ToString().Trim());
            }
            //����
            if (!queryParam["PostId"].IsEmpty())
            {
                sqlwhere += string.Format(" and  PostId = '{0}'", queryParam["PostId"].ToString().Trim());
            }
            //���֤��
            if (!queryParam["us"].IsEmpty())
            {
                sqlwhere += string.Format(" and  us like '%{0}%'", queryParam["us"].ToString().Trim());
            }

            return this.BaseRepository().FindTable(sql + sqlwhere);
        }

        /// <summary>
        /// ��ȡ�û��ĽӴ�Σ������
        /// </summary>
        /// <returns></returns>
        public string GetUserHazardfactor(string useraccount)
        {
            string sql = string.Format(
                @"select LISTAGG(riskvalue,',') WITHIN GROUP (ORDER BY riskvalue) AS us from (              
            select userid,username,riskvalue from BIS_HAZARDFACTORUSER hauser
            left join BIS_HAZARDFACTORs ha on ha.hid=hauser.hid
             where 1=1
             group by userid,username,riskvalue) f where userid='{0}'  group by userid,username", useraccount);
            object ret = BaseRepository().FindObject(sql);
            if (ret == null)
            {
                return "";
            }
            else
            {
                return ret.ToString();
            }
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
        public void SaveForm(string keyValue, HazardfactoruserEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
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
