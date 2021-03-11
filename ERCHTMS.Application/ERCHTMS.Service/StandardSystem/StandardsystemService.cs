using ERCHTMS.Entity.StandardSystem;
using ERCHTMS.IService.StandardSystem;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Data;
using System.Data;
using ERCHTMS.Code;

namespace ERCHTMS.Service.StandardSystem
{
    /// <summary>
    /// �� ������׼��ϵ
    /// </summary>
    public class StandardsystemService : RepositoryFactory<StandardsystemEntity>, StandardsystemIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                if (queryJson.Length > 0)
                {
                    var queryParam = queryJson.ToJObject();
                    if (!queryParam["standardtype"].IsEmpty())
                    {
                        pagination.conditionJson += " and a.standardtype='" + queryParam["standardtype"].ToString() + "'";
                    }
                    if (!queryParam["enCode"].IsEmpty())
                    {
                        pagination.conditionJson += " and b.id in (select id from hrs_stcategory where encode like '" + queryParam["enCode"].ToString() + "%')";
                    }
                    if (!queryParam["filename"].IsEmpty())
                    {
                        pagination.conditionJson += " and filename like '%" + queryParam["filename"].ToString() + "%'";
                    }
                    if (!queryParam["station"].IsEmpty())
                    {
                        string[] PostidList = queryParam["station"].ToString().Replace("��", ",").Split(',');
                        string forsql = " and (";
                        foreach (var item in PostidList)
                        {
                            forsql += "stationname like '%" + item.ToString() + "%' or";
                        }
                        if (forsql.Length > 6)
                        {
                            forsql = forsql.Substring(0, forsql.Length - 2);
                        }
                        forsql += ")";
                        pagination.conditionJson += forsql;
                    }
                    if (!queryParam["keyword"].IsEmpty())
                    {
                        pagination.conditionJson += " and (stationname like '%" + queryParam["keyword"].ToString() + "%' or filename like '%" + queryParam["keyword"].ToString() + "%')";
                    }
                    //���ºϹ�������ר������
                    if (!queryParam["standardtypestr"].IsEmpty())
                    {
                        pagination.page = 1;
                        pagination.rows = 1000000000;
                        pagination.conditionJson += " and a.standardtype in("+ queryParam["standardtypestr"].ToString() + ")";
                    }
                    if (!queryParam["timeliness"].IsEmpty())
                    {
                        pagination.conditionJson += " and a.timeliness ='" + queryParam["timeliness"].ToString() + "'";
                    }
                    if (!queryParam["carrydate"].IsEmpty())
                    {
                        pagination.conditionJson += " and to_char(carrydate,'yyyy')=" + queryParam["carrydate"].ToString();
                    }
                }
                return this.BaseRepository().FindTableByProcPager(pagination, DbHelper.DbType);
            }
            catch (System.Exception ex)
            {

                throw;
            }
            
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<StandardsystemEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public StandardsystemEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }



        //public DataTable GetStandardCount()
        //{
        //    Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
        //    string forsql = "";
        //    if (!user.PostName.IsEmpty())
        //    {
        //        string[] PostidList = user.PostName.Replace("��", ",").Split(',');
        //        forsql = " and (";
        //        foreach (var item in PostidList)
        //        {
        //            forsql += "stationname like '%" + item.ToString() + "%' or";
        //        }
        //        if (forsql.Length > 6)
        //        {
        //            forsql = forsql.Substring(0, forsql.Length - 2);
        //        }
        //        forsql += ")";
        //    }
        //    string sql = @"select  (select count(1)  from hrs_standardsystem where createuserorgcode = '" + user.OrganizeCode + @"' and standardtype=1 " + forsql + @" and add_months( createdate,1) > sysdate) as type1,
        //        (select count(1)  from hrs_standardsystem where createuserorgcode = '" + user.OrganizeCode + @"' and standardtype = 2 " + forsql + @" and add_months( createdate,1) > sysdate) as type2,
        //        (select count(1)  from hrs_standardsystem where createuserorgcode = '" + user.OrganizeCode + @"' and standardtype = 3 " + forsql + @" and add_months( createdate,1) > sysdate) as type3,
        //        (select count(1)  from hrs_standardsystem where createuserorgcode = '" + user.OrganizeCode + @"' and standardtype = 4 " + forsql + @" and add_months( createdate,1) > sysdate) as type4,
        //        (select count(1)  from hrs_standardsystem where createuserorgcode = '" + user.OrganizeCode + @"' and standardtype = 5 " + forsql + @" and add_months( createdate,1) > sysdate) as type5,
        //        (select count(1)  from hrs_standardsystem where createuserorgcode = '" + user.OrganizeCode + @"' and standardtype = 6 " + forsql + @" and add_months( createdate,1) > sysdate) as type6 from dual";
        //    return this.BaseRepository().FindTable(sql);
        //}

        /// <summary>
        /// ������ҳ��ȡ��׼����
        /// </summary>
        /// <returns></returns>
        public DataTable GetStandardCount()
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string forsql = "";
            DataTable dt = new DataTable();
            dt.Columns.Add("num");
            dt.Columns.Add("standardtype");
            dt.Columns.Add("standardtypename");
            if (!user.PostId.IsEmpty())
            {
                string[] PostidList = user.PostId.Replace("��", ",").Split(',');
                forsql = " and (";
                foreach (var item in PostidList)
                {
                    forsql += "stationid like '%" + item.ToString() + "%' or";
                }
                if (forsql.Length > 6)
                {
                    forsql = forsql.Substring(0, forsql.Length - 2);
                }
                forsql += ")";
            }
            string[] strlist = { "������׼��ϵ", "�����׼��ϵ", "��λ��׼��ϵ", "�ϼ���׼���ļ�", "ָ����׼", "���ɷ���", "��׼��ϵ�߻��빹��", "��׼��ϵ������Ľ�", "��׼����ѵ" };
            for (int i = 1; i < 10; i++)
            {
                if (i!=7 && i!=8)
                {
                    DataRow dtrow = dt.NewRow();
                    string sql = @" select count(1) as num from hrs_standardsystem a left join hrs_standardreadrecord b on a.id = b.recid and 
                            b.createuserid = '" + user.UserId + @"' where a.createuserorgcode = '" + user.OrganizeCode + @"' and standardtype ='" + i + "' " + forsql + @" and b.recid is null";
                    DataTable dttemp = this.BaseRepository().FindTable(sql);
                    if (dttemp.Rows.Count>0)
                    {
                        dtrow["num"] = dttemp.Rows[0]["num"].ToString();
                        dtrow["standardtype"] = i;
                        dtrow["standardtypename"] = strlist[i-1].ToString();
                        dt.Rows.Add(dtrow);
                    }
                }
            }

            return dt;
            
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
        /// ɾ����׼����ͬ��ɾ����Ӧ�������Ѿ��е�����
        /// </summary>
        /// <param name="ids"></param>
        public void RemoveCategoryForms(string ids)
        {
            this.BaseRepository().ExecuteBySql("delete hrs_standardsystem where categorycode in (" + ids + ")");
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, StandardsystemEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                StandardsystemEntity se = this.BaseRepository().FindEntity(keyValue);
                if (se == null)
                {
                    entity.ID = keyValue;
                    entity.Create();
                    this.BaseRepository().Insert(entity);


                }
                else
                {
                    entity.Modify(keyValue);
                    if (entity.CONSULTNUM == 0)
                    {
                        entity.CONSULTNUM = se.CONSULTNUM;
                    }
                    entity.CREATEUSERDEPTNAME = null;
                    entity.CATEGORYNAME = null;
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
