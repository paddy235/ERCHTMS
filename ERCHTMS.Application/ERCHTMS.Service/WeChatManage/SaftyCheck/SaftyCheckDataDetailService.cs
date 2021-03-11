using ERCHTMS.Entity.SaftyCheck;
using ERCHTMS.IService.SaftyCheck;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.Data;
using System.Data.Common;
using System.Text;
using ERCHTMS.Code;
using System;

namespace ERCHTMS.Service.SaftyCheck
{
    /// <summary>
    /// �� ������ȫ��������
    /// </summary>
    public class SaftyCheckDataDetailService : RepositoryFactory<SaftyCheckDataDetailEntity>, SaftyCheckDataDetailIService
    {

        #region ��ȡ����
        /// <summary>
        /// ���ĵǼ�״̬
        /// </summary>
        public void RegisterPer(string userAccount, string id)
        {
            IEnumerable<SaftyCheckDataDetailEntity> list = this.BaseRepository().FindList("select *from BIS_SAFTYCHECKDATADETAILED where recid='" + id + "' and instr(CHECKMANID,'"+ userAccount + "')>0");
            foreach (SaftyCheckDataDetailEntity item in list)
            {
                item.CheckState = 2;
                this.BaseRepository().Update(item);
            }
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SaftyCheckDataDetailEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȫ����ʱ��������
        /// </summary>
        public DataTable GetDetails(string ids)
        {
            //���ؼ����
            DataTable dt = this.BaseRepository().FindTable("select * from BIS_SAFTYCHECKDATADETAILED where recid in ('" + ids + "')  order by BelongDistrictCode");
            return dt;
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SaftyCheckDataDetailEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// ��ȫ�����б�(ϵͳ����)
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetPageOfSysCreate(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            DatabaseType dataType = DbHelper.DbType;
            if (!queryParam["chargedeptcode"].IsEmpty())
            {
                string chargedeptcode = queryParam["chargedeptcode"].ToString();
                StringBuilder sb = new StringBuilder();
                var d = OperatorProvider.Provider.Current();
                if (d.RoleName != null)
                {
                    if (!d.RoleName.Contains("����"))
                        sb.Append(" and instr(t.deptcode,'" + chargedeptcode + "')>0");
                    else
                        sb.Append(" and instr(t.deptcode,'" + chargedeptcode.Substring(0, 3) + "')>0");
                }
                pagination.conditionJson += sb.ToString();
            }
            pagination.conditionJson += string.Format(" and  status>0  order by AREACODE");
            DataTable dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            return dt;
        }


        /// <summary>
        /// ��ȫ�����б�(ϵͳ����)
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetListOfSysCreate(string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            string sql = "select status,ID,DISTRICTNAME,DANGERSOURCE,DISTRICTID,AreaCode from BIS_RISKASSESS t where 1=1";
            DatabaseType dataType = DbHelper.DbType;
            if (!queryParam["chargedeptcode"].IsEmpty())
            {
                string chargedeptcode = queryParam["chargedeptcode"].ToString();
                StringBuilder sb = new StringBuilder();
                var d = OperatorProvider.Provider.Current();
                if (d.RoleName != null)
                {
                    if (!d.RoleName.Contains("����"))
                        sb.Append(string.Format(" and  t.deptcode like '{0}%'", chargedeptcode));
                    else
                        sb.Append(string.Format(" and  t.deptcode like '{0}%'", chargedeptcode.Substring(0, 3)));
                }
                sql += sb.ToString();
            }
            sql += string.Format(" and  status=1  and deleteMark='0' order by AREACODE");
            DataTable dt = this.BaseRepository().FindTable(sql);
            return dt;
        }
        /// <summary>
        /// ��ȡ �������
        /// </summary>
        /// <param name="baseID">���յ�ID</param>
        public DataTable GetPageContent(string baseID)
        {
            DataTable dt = this.BaseRepository().FindTable("select content from BIS_MEASURES where riskid='" + baseID + "'");
            return dt;
        }
        /// <summary>
        /// ��ȫ�����б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public IEnumerable<SaftyCheckDataDetailEntity> GetPageList(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();

            //ѡ�������
            if (!queryParam["type"].IsEmpty())
            {
                string type = queryParam["type"].ToString();
                if (!type.Contains("all"))
                {
                    pagination.conditionJson += string.Format(" and t.recid in (select id from BIS_SAFTYCHECKDATA where CHECKDATATYPE in ('{0}') and t.CHECKDATATYPE is not null)", type);
                }
            }
            //�����ؼ���
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyword = queryParam["keyword"].ToString();
                pagination.conditionJson += string.Format(" and t.CheckContent like '%{0}%'", keyword);
            }
            //��������
            if (!queryParam["code"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.belongdeptid  in (select departmentid from base_department where encode like '{0}%')", queryParam["code"].ToString());
            }
            //ѡȡ�Ѿ�������
            //if (!queryParam["newstate"].IsEmpty())
            //{
            //    string state = queryParam["newstate"].ToString();
            //    pagination.conditionJson += string.Format(" and t.CHECKSTATE='{0}'", state);
            //}
            //�����Լ��ļ��
            if (!queryParam["userAccount"].IsEmpty())
            {
                string userAccount = queryParam["userAccount"].ToString();
                if (OperatorProvider.Provider.Current().IsSystem == false)
                    pagination.conditionJson += string.Format(" and instr(t.CHECKMANID,'{0}')>0", userAccount);
            }
            //��������
            if (!queryParam["recid"].IsEmpty())
            {
                string recid = queryParam["recid"].ToString();
                pagination.conditionJson += string.Format(" and t.recid='{0}'", recid);
            }
            else pagination.conditionJson += string.Format(" and t.recid='{0}'", "");
            pagination.conditionJson += string.Format("  order by BelongDistrictCode");
            IEnumerable<SaftyCheckDataDetailEntity> list = this.BaseRepository().FindListByProcPager(pagination, dataType).Select(r =>
            {
                //�õ�������ݺͼ����Ա
                if (!queryParam["userAccount"].IsEmpty())
                {
                    if (OperatorProvider.Provider.Current().IsSystem == false)
                    {
                        DataTable dtContent = this.BaseRepository().FindTable("select SaftyContent,CheckManName,CheckManAccount from BIS_SAFTYCONTENT o where instr(checkmanaccount,'" + queryParam["userAccount"].ToString() + "')>0 and recid='" + r.RecID + "' and DETAILID='" + r.ID + "' and DistrictId='" + r.BelongDistrictID + "'");
                        string SaftyContent = "";
                        string CheckManName = "";
                        string CheckManAccount = "";
                        foreach (DataRow item in dtContent.Rows)
                        {
                            SaftyContent += item[0].ToString() + "|";
                            CheckManName += item[1].ToString() + "|";
                            CheckManAccount += item[2].ToString() + "|";
                        }
                        r.CheckContent = SaftyContent.Substring(0, SaftyContent.Length - 1);
                        r.CheckMan = CheckManName.Substring(0, CheckManName.Length - 1);
                        r.CheckManID = CheckManAccount.Substring(0, CheckManAccount.Length - 1);
                    }
                }

                if (!queryParam["allcount"].IsEmpty())
                {
                    string i = queryParam["allcount"].ToString();
                    if (i == "1")
                    {
                        //�õ����������е���������
                        DataTable dt = this.BaseRepository().FindTable("select id from bis_htbaseinfo o where HIDPOINT='" + r.BelongDistrictCode + "' and  o.safetycheckobjectid in(select recid from bis_saftycheckdatadetailed where recid='" + queryParam["recid"].ToString() + "' and BelongDistrictID='" + r.BelongDistrictID + "')");
                        r.Count = dt.Rows.Count;
                    }
                }
                else
                {
                    //����������������
                    DataTable dt = this.BaseRepository().FindTable("select id from bis_htbaseinfo o where HIDPOINT='" + r.BelongDistrictCode + "' and  o.safetycheckobjectid in(select recid from bis_saftycheckdatadetailed where recid='" + queryParam["recid"].ToString() + "' and BelongDistrictID='" + r.BelongDistrictID + "')");
                    r.Count = dt.Rows.Count;
                }
                return r;
            });

            return list;

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
        /// <param name="list">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, List<SaftyCheckDataDetailEntity> list)
        {

            foreach (SaftyCheckDataDetailEntity entity in list)
            {
                if (!string.IsNullOrEmpty(entity.ID))
                {
                    entity.Modify(entity.ID);
                    this.BaseRepository().Update(entity);
                }
                else
                {
                    entity.Create();
                    entity.RecID = keyValue;
                    this.BaseRepository().Insert(entity);
                }

            }
        }
        /// <summary>
        /// ���浽���������ǼǼ���¼����ʱ���޸ģ�
        /// </summary>
        public void SaveResultForm(List<SaftyCheckDataDetailEntity> list)
        {
            foreach (SaftyCheckDataDetailEntity entity in list)
            {
                entity.Modify(entity.ID);
                this.BaseRepository().Update(entity);
            }
        }
        /// <summary>
        /// ר�����ƶ��ƻ���������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="list">ʵ�����</param>
        /// <returns></returns>
        public void SaveFormToContent(string keyValue, List<SaftyCheckDataDetailEntity> list)
        {
            foreach (SaftyCheckDataDetailEntity entity in list)
            {
                entity.Create();
                entity.RecID = keyValue;
                this.BaseRepository().Insert(entity);
            }
        }
        public int Remove(string recid)
        {
            return this.BaseRepository().ExecuteBySql("delete from bis_saftycheckdatadetailed where recid='" + recid + "'");
        }
        #endregion

        #region ��ȡ����(�ֻ���)
        public IEnumerable<SaftyCheckDataDetailEntity> GetSaftyDataDetail(string safeCheckIdItem)
        {
            string sqlWhere = "select *from BIS_SAFTYCHECKDATADETAILED where 1=1";
            if (!string.IsNullOrEmpty(sqlWhere))
            {
                sqlWhere += " and recid='" + safeCheckIdItem + "'";
            }
            IEnumerable<SaftyCheckDataDetailEntity> list = this.BaseRepository().FindList(sqlWhere).Select(x =>
            {
                //�õ����������е���������
                DataTable dt = this.BaseRepository().FindTable(string.Format("select id from bis_htbaseinfo o where HIDPOINT='" + x.BelongDistrictCode + "' and  o.safetycheckobjectid='{0}'", safeCheckIdItem));
                // in(select recid from bis_saftycheckdatadetailed where  BelongDistrictID='" + x.BelongDistrictID + "')
                x.Count = dt.Rows.Count;
                return x;
            });
            return list;
        }

        public void insertIntoDetails(string checkExcelId, string recid)
        {
            string sqlWhere = string.Format("select * from Bis_Saftycheckdatadetailed where 1=1 and recid='{0}'", checkExcelId);
            IEnumerable<SaftyCheckDataDetailEntity> list = this.BaseRepository().FindList(sqlWhere);
            foreach (SaftyCheckDataDetailEntity item in list)
            {
                item.ID = Guid.NewGuid().ToString();
                item.RecID = recid;
                item.CheckDataId = checkExcelId;
                this.BaseRepository().Insert(item);
            }
        }
        #endregion
    }
}
