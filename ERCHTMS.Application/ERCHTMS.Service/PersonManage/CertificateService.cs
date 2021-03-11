using ERCHTMS.Entity.PersonManage;
using ERCHTMS.IService.PersonManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Data;

namespace ERCHTMS.Service.PersonManage
{
    /// <summary>
    /// �� ������Ա֤��
    /// </summary>
    public class CertificateService : RepositoryFactory<CertificateEntity>, CertificateIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="userId">�û�Id</param>
        /// <returns>�����б�</returns>
        public IEnumerable<CertificateEntity> GetList(string userId, Pagination pag = null)
        {
            string sql = "select * from BIS_CERTIFICATE where userid='" + userId + "'";

            if (pag != null)
            {
                if (!string.IsNullOrWhiteSpace(pag.sidx))
                {
                    sql += " order by " + pag.sidx + " " + pag.sord;
                }
            }
            IEnumerable<CertificateEntity> list = this.BaseRepository().FindList(sql);
            return list;
        }
        /// <summary>
        /// ��ȡ��Ա֤��
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable GetCertList(string userId)
        {
            return this.BaseRepository().FindTable(string.Format("select * from BIS_CERTAUDIT t where certid='{0}'", userId));
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public CertificateEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// ������Ա��֤��
        /// </summary>
        /// <param name="sum">��Ա������</param>
        /// <param name="deptCode">��ǰ�û��������ű���</param>
        /// <param name="certType">֤�����</param>
        /// <returns></returns>
        public decimal GetCertPercent(int sum, string deptCode, string certType)
        {
            string colName = certType == "������ҵ����֤" ? "isspecial='��'" : "isspecialequ='��'";
            string sql = string.Format("select distinct userid  from BIS_CERTIFICATE t where (certname='{1}' or certtype='{1}') and  userid in (select userid from base_user u where ispresence='1' and {2} and  {0}) ", deptCode, certType, colName);
            decimal count = this.BaseRepository().FindTable(sql).Rows.Count;
            sum = int.Parse(BaseRepository().FindObject(string.Format("select count(1) from base_user u where ispresence='1' and {1} and  {0}", deptCode, colName)).ToString());
            return sum == 0 ? 0 : Math.Round((count * 100) / decimal.Parse(sum.ToString()), 2);

        }
        /// <summary>
        /// ���ݵ�ǰ�û���Ȩ�޷�Χ��ȡ������ļ������ں��ѹ��ڵ�֤����Ϣ
        /// </summary>
        /// <param name="where">����Ȩ�޷�Χ</param>
        /// <returns></returns>
        public Dictionary<string, string> GetOverdueCertList(string where)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string sql = string.Format("select userid,max(status) as status from (select userid,case when enddate<to_date(to_char(sysdate,'yyyy-mm-dd'),'yyyy-mm-dd') then 1  when add_months(sysdate,3)>enddate and enddate>=to_date(to_char(sysdate,'yyyy-mm-dd'),'yyyy-mm-dd') then 2  else 0  end status from BIS_CERTIFICATE t where userid in(select userid from base_user u where {0})) a where status=1 or status=2 group by userid", where);
            DataTable dt = this.BaseRepository().FindTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                dic.Add(dr[0].ToString(), dr[1].ToString());
            }
            return dic;
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue)
        {
            CertificateEntity entity = BaseRepository().FindEntity(keyValue);
            if (entity != null)
            {
                int count = this.BaseRepository().Delete(entity);
                if (count > 0)
                {
                    //count = int.Parse(BaseRepository().FindObject(string.Format("select count(1) from BIS_CERTIFICATE where userid='{0}' and (certname='{1}' or certtype='{1}') ", entity.UserId, "������ҵ����֤")).ToString());
                    //if (count ==0)
                    //{
                    //    BaseRepository().ExecuteBySql(string.Format("update base_user set isspecial='��' where userid='{0}'", entity.UserId));
                    //}
                    //count = int.Parse(BaseRepository().FindObject(string.Format("select count(1) from BIS_CERTIFICATE where userid='{0}' and (certname='{1}' or certtype='{1}') ", entity.UserId, "�����豸��ҵ��Ա֤")).ToString());
                    //if (count==0)
                    //{
                    //    BaseRepository().ExecuteBySql(string.Format("update base_user set isspecialequ='��' where userid='{0}'", entity.UserId));
                    //}
                    //count = int.Parse(BaseRepository().FindObject(string.Format("select count(1) from BIS_CERTIFICATE where userid='{0}' and (certname='{1}' or certtype='{1}') ", entity.UserId, "������Ա֤��")).ToString());
                    //if (count == 0)
                    //{
                    //    BaseRepository().ExecuteBySql(string.Format("update base_user set ISFOURPERSON='��' where userid='{0}'", entity.UserId));
                    //}
                }
            }

        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public bool SaveForm(string keyValue, CertificateEntity entity)
        {
            int count = 0;
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Id = keyValue;
                CertificateEntity cert = BaseRepository().FindEntity(keyValue);
                if (cert == null)
                {
                    entity.Create();
                    count = this.BaseRepository().Insert(entity);
                }
                else
                {
                    entity.Modify(keyValue);
                    count = this.BaseRepository().Update(entity);

                }

            }
            else
            {
                entity.Create();
                count = this.BaseRepository().Insert(entity);
            }
            if (count > 0)
            {
                count = int.Parse(BaseRepository().FindObject(string.Format("select count(1) from BIS_CERTIFICATE where userid='{0}' and (certname='{1}' or certtype='{1}') ", entity.UserId, "������ҵ����֤")).ToString());
                if (count > 0)
                {
                    BaseRepository().ExecuteBySql(string.Format("update base_user set isspecial='��' where userid='{0}'", entity.UserId));
                }
                count = int.Parse(BaseRepository().FindObject(string.Format("select count(1) from BIS_CERTIFICATE where userid='{0}' and (certname='{1}' or certtype='{1}') ", entity.UserId, "�����豸��ҵ��Ա֤")).ToString());
                if (count > 0)
                {
                    BaseRepository().ExecuteBySql(string.Format("update base_user set isspecialequ='��' where userid='{0}'", entity.UserId));
                }
                //count = int.Parse(BaseRepository().FindObject(string.Format("select count(1) from BIS_CERTIFICATE where userid='{0}' and (certname='{1}' or certtype='{1}') ", entity.UserId, "������Ա֤��")).ToString());
                //if (count > 0)
                //{
                //    BaseRepository().ExecuteBySql(string.Format("update base_user set ISFOURPERSON='��' where userid='{0}'", entity.UserId));
                //}
                return true;
            }
            else
            {
                return false;
            }

        }
        /// <summary>
        /// ��ȡ������Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public CertAuditEntity GetAuditEntity(string keyValue)
        {
            return DbFactory.Base().FindEntity<CertAuditEntity>(keyValue);
        }
        /// <summary>
        /// ��ȡ֤�������¼
        /// </summary>
        /// <param name="certId"></param>
        /// <returns></returns>
        public DataTable GetAuditList(string certId)
        {
            return BaseRepository().FindTable(string.Format("select *from bis_certaudit where certid='{0}'", certId));
        }
        /// <summary>
        /// �������޸�֤�鸴���¼
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool SaveCertAudit(CertAuditEntity entity)
        {
            try
            {
                var db = DbFactory.Base();
                if (string.IsNullOrWhiteSpace(entity.Id))
                {
                    entity.Create();
                    db.Insert<CertAuditEntity>(entity);
                }
                else
                {
                    CertAuditEntity ca = db.FindEntity<CertAuditEntity>(entity.Id);
                    if (ca == null)
                    {
                        entity.Create();
                        DbFactory.Base().Insert<CertAuditEntity>(entity);
                    }
                    else
                    {
                        DbFactory.Base().Update<CertAuditEntity>(entity);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// ɾ��֤�鸴���¼
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public bool RemoveCertAudit(string keyValue)
        {
            try
            {
                var db = DbFactory.Base();
                CertAuditEntity ca = db.FindEntity<CertAuditEntity>(keyValue);
                if (ca != null)
                {
                    db.Delete<CertAuditEntity>(keyValue);
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
