using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using BSFramework.Util.Extension;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Linq;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.IService.AuthorizeManage;
using ERCHTMS.Service.AuthorizeManage;
using System;
using ERCHTMS.Code;
using ERCHTMS.Entity.EmergencyPlatform;
using ERCHTMS.IService.EmergencyPlatform;
using System.Linq.Expressions;
using ERCHTMS.Service.CommonPermission;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Service.BaseManage;
using ERCHTMS.Service.HighRiskWork;
using ERCHTMS.Service.SystemManage;

namespace ERCHTMS.Service.EmergencyPlatform
{
    /// <summary>
    /// �� ����Ӧ��������¼
    /// </summary>
    public class DrillplanrecordService : RepositoryFactory<DrillplanrecordEntity>, IDrillplanrecordService
    {
        #region ��ȡ����


        public DataTable GetAssessRecordList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            if (queryJson.Length > 0)
            {
                var queryParam = queryJson.ToJObject();
                if (!queryParam["drillrecordid"].IsEmpty())
                {
                    string drillrecordid = queryParam["drillrecordid"].ToString();
                    pagination.conditionJson += string.Format(" and drillrecordid = '{0}'", drillrecordid);
                }
            }

            Repository<DrillrecordAssessEntity> AssessReg = new Repository<DrillrecordAssessEntity>(DbFactory.Base());
            return AssessReg.FindTableByProcPager(pagination, dataType);
        }

        public List<DrillrecordAssessEntity> GetAssessList(string drillrecordid) 
        {
            Repository<DrillrecordAssessEntity> AssessReg = new Repository<DrillrecordAssessEntity>(DbFactory.Base());
            return AssessReg.IQueryable().Where(p => p.DrillRecordId == drillrecordid).ToList();
        }
        /// <summary>
        /// ��ȡ��ʷ��¼
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetHistoryPageListJson(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            Repository<DrillplanrecordHistoryEntity> historyReg = new Repository<DrillplanrecordHistoryEntity>(DbFactory.Base());
            return historyReg.FindTableByProcPager(pagination, dataType);
        }
        #region Ӧ������Ԥ������ͳ��
        /// <summary>
        /// Ӧ������Ԥ������ͳ��
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public DataTable GetDrillPlanRecordTypeSta(string condition, int mode)
        {
            string sql = string.Empty;

            //��ȡӦ������Ԥ��ͳ��
            if (mode == 0)
            {
                sql = string.Format(@"select nvl(num,0) num , a.itemname,a.itemvalue from (
                                      select a.* from base_dataitemdetail a , base_dataitem  b where b.itemcode ='MAE_PlanType' and a.itemid = b.itemid ) a
                                      left join   (select  count(id) num, drilltype from  mae_drillplanrecord  a where 1=1 {0} group by  drilltype)   b on a.itemvalue = b.drilltype ", condition);
            }
            else if (mode == 1)  //��ȡӦ������Ԥ��ͳ��(�ֳ����÷���ͳ��)
            {
                sql = string.Format(@"select nvl(num,0) num , a.itemname ,a.itemvalue from (
                                        select a.* from base_dataitemdetail a , base_dataitem  b where b.itemcode ='HandlePlanType' and a.itemid = b.itemid ) a
                                        left join   (
                                          select  b.plantypehandlecode, count(a.id) num  from  mae_drillplanrecord  a 
                                          left join mae_reserverplan b on a.drillplanid = b.id where a.isconnectplan ='��'  {0} group by b.plantypehandlecode 
                                        )   b on a.itemvalue = b.plantypehandlecode ", condition);
            }
            else if (mode == 2) //�μ������˴�ͳ��
            {
                sql = string.Format(@"select nvl(num,0) num , a.itemname,a.itemvalue from (
                                    select a.* from base_dataitemdetail a , base_dataitem  b where b.itemcode ='MAE_DirllMode' and a.itemid = b.itemid ) a
                                    left join   (select drillmode  ,sum(drillpeoplenumber) num from mae_drillplanrecord  a  where 1=1 {0}  group by  drillmode)   b on a.itemvalue = b.drillmode ", condition);
            }
            DataTable dt = this.BaseRepository().FindTable(sql);

            return dt;
        }
        public DataTable DrillplanStat(string drillmode, bool isCompany, string deptCode, string starttime, string endtime)
        {
            string strWhere = string.Empty;
            string strWhere1 = string.Empty;
            if (!string.IsNullOrWhiteSpace(drillmode))
            {
                strWhere1 += string.Format(" and t.drillmodename='{0}'", drillmode);
            }


            if (!string.IsNullOrWhiteSpace(starttime))
            {
                strWhere1 += string.Format("and to_char(t.drilltime,'yyyy-MM-dd') >='{0}'", starttime);
            }
            if (!string.IsNullOrWhiteSpace(endtime))
            {
                strWhere1 += string.Format("and to_char(t.drilltime,'yyyy-MM-dd') <='{0}'", endtime);
            }
            string sql = string.Empty;
            if (isCompany)
            {
                strWhere += "and m.nature in ('����','����')";
                sql = string.Format(@"select * from(select nvl(sum(p.total),0) recordnum,m.fullname,m.encode,max(m.sortcode) sortcode,
                                p.drillmodename,max(m.departmentid) departmentid,max(m.parentid) parentid from base_department m 
                                left join(select count(1) total,t.drillmodename,d.fullname,d.encode from mae_drillplanrecord t
                                left join base_department d on d.encode = t.orgdeptcode 
                                where d.nature='����' and t.iscommit=1 {0}
                                group by t.drillmodename,d.fullname,d.encode
                                union 
                                select count(1) total,t.drillmodename,d.fullname,d.encode from mae_drillplanrecord t
                                left join base_department d on d.encode = substr(t.orgdeptcode,0,length(d.encode))
                                where d.nature='����' and t.iscommit=1 {0}
                                group by t.drillmodename,d.fullname,d.encode) p on p.encode=m.encode
                                where 1=1 {1}
                                group by m.fullname,m.encode,p.drillmodename) order by sortcode", strWhere1, strWhere);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(deptCode))
                {
                    strWhere += string.Format("and m.encode like'{0}%'", deptCode);
                }
                sql = string.Format(@"select * from (select nvl(sum(p.total),0) recordnum,m.fullname,m.encode,max(m.sortcode) sortcode from base_department m 
                                left join(
                                select count(1) total,t.orgdept,t.orgdeptcode from mae_drillplanrecord t
                                left join base_department d on d.encode = substr(t.orgdeptcode,0,length(d.encode))
                                where d.nature='����' and t.iscommit=1 {0}
                                group by t.orgdept,t.orgdeptcode) p on p.orgdeptcode=m.encode
                                where 1=1 {1}
                                group by m.fullname,m.encode) order by sortcode", strWhere1, strWhere);
            }

            return this.BaseRepository().FindTable(sql);
        }
        public DataTable DrillplanStatList(string drillmode, bool isCompany, string deptCode, string starttime, string endtime)
        {
            string strWhere = string.Empty;
            string strWhere1 = string.Empty;

            if (!string.IsNullOrWhiteSpace(deptCode))
            {
                strWhere += string.Format("and m.encode like'{0}%'", deptCode);
            }
            if (!string.IsNullOrWhiteSpace(starttime))
            {
                strWhere1 += string.Format("and to_char(t.drilltime,'yyyy-MM-dd') >='{0}'", starttime);
            }
            if (!string.IsNullOrWhiteSpace(endtime))
            {
                strWhere1 += string.Format("and to_char(t.drilltime,'yyyy-MM-dd') <='{0}'", endtime);
            }
            string strWhere2 = string.Empty;
            string selWhere = string.Empty;
            string strWhere3 = string.Empty;
            string sumWhere = string.Empty;
            var datamode = new DataItemDetailService().GetDataItemListByItemCode("'MAE_DirllMode'").ToList();
            if (datamode.Count > 0)
            {
                for (int i = 0; i < datamode.Count; i++)
                {
                    strWhere2 += "'" + datamode[i].ItemName + "' mode" + (i + 1) + ",";
                    selWhere += "nvl(sum(p.mode" + (i + 1) + "),0) recordnum" + (i + 1) + ",";
                    sumWhere += "nvl(p.mode" + (i + 1) + ",0)+";
                    strWhere3 += "mode" + (i + 1) + ",";
                }
                if (strWhere2.Length > 0) {
                    strWhere2 = strWhere2.Substring(0, strWhere2.Length - 1);
                    selWhere = selWhere.Substring(0, selWhere.Length - 1);
                    sumWhere = sumWhere.Substring(0, sumWhere.Length - 1);
                    strWhere3 = strWhere3.Substring(0, strWhere3.Length - 1);
                }

            }
            else {
                strWhere2 = "'��������' mode1,'ʵս����' mode2";
                selWhere = "nvl(sum(p.mode1),0) recordnum1,nvl(sum(p.mode2),0) recordnum2";
                sumWhere = "(nvl(p.mode1,0)+nvl(p.mode2,0))";
                strWhere3 = "mode1,mode2";
            }
            string sql = string.Empty;
            if (isCompany)
            {
                sql = string.Format(@"select * from (select m.fullname,{0},nvl(sum({1}),0) total,max(m.sortcode) sortcode,
                                m.encode,max(m.departmentid) departmentid,max(m.parentid) parentid from base_department m 
                                left join(select {5},fullname,encode from(select count(1) total,d.fullname,d.encode,t.drillmodename from mae_drillplanrecord t
                                left join base_department d on d.encode = t.orgdeptcode 
                                where d.nature='����' and t.iscommit=1 {2}
                                group by d.fullname,d.encode,t.drillmodename
                                union 
                                select count(1) total,d.fullname,d.encode,t.drillmodename from mae_drillplanrecord t
                                left join base_department d on d.encode = substr(t.orgdeptcode,0,length(d.encode))
                                where d.nature='����' and t.iscommit=1    {2}
                                group by d.fullname,d.encode,t.drillmodename) pivot(max(total)for drillmodename in({4}))) p on p.encode=m.encode
                                where 1=1 and m.nature in ('����','����')  {3}
                                group by m.fullname,m.encode
                                union
                                select m.fullname,{0},nvl(sum({1}),0) total,max(m.sortcode) sortcode,m.encode,
                                max(m.departmentid) departmentid,max(m.parentid) parentid from base_department m 
                                left join(select {5},orgdept,orgdeptcode from
                                (select count(1) total,t.orgdept,t.orgdeptcode,t.drillmodename from mae_drillplanrecord t
                                left join base_department d on d.encode = t.orgdeptcode
                                where t.iscommit=1  {2}
                                group by t.orgdept,t.orgdeptcode,t.drillmodename)pivot(max(total)for drillmodename in({4}))) p on p.orgdeptcode=m.encode
                                where 1=1 and m.nature not in('����','��','����','ʡ��','����') {3}
                                group by m.fullname,m.encode)  order by sortcode ", selWhere, sumWhere, strWhere1, strWhere, strWhere2,strWhere3);
//                sql = string.Format(@"select * from (select m.fullname,nvl(sum(p.total),0) recordnum,nvl(sum(p.total),0) total,max(m.sortcode) sortcode,
//                                m.encode,max(m.departmentid) departmentid,max(m.parentid) parentid from base_department m 
//                                left join(select count(1) total,d.fullname,d.encode from mae_drillplanrecord t
//                                left join base_department d on d.encode = t.orgdeptcode 
//                                where d.nature='����' and t.iscommit=1 
//                                group by d.fullname,d.encode
//                                union 
//                                select count(1) total,d.fullname,d.encode from mae_drillplanrecord t
//                                left join base_department d on d.encode = substr(t.orgdeptcode,0,length(d.encode))
//                                where d.nature='����' and t.iscommit=1   {0}
//                                group by d.fullname,d.encode) p on p.encode=m.encode
//                                where 1=1 and m.nature in ('����','����') {1}
//                                group by m.fullname,m.encode
//                                union
//                                select m.fullname,nvl(sum(p.total),0) recordnum,nvl(sum(p.total),0) total,max(m.sortcode) sortcode,m.encode,
//                                max(m.departmentid) departmentid,max(m.parentid) parentid from base_department m 
//                                left join(
//                                select count(1) total,t.orgdept,t.orgdeptcode from mae_drillplanrecord t
//                                left join base_department d on d.encode = t.orgdeptcode
//                                where t.iscommit=1  {0}
//                                group by t.orgdept,t.orgdeptcode) p on p.orgdeptcode=m.encode
//                                where 1=1 and m.nature not in('����','��','����','ʡ��','����') {1}
//                                group by m.fullname,m.encode) order by sortcode", strWhere1, strWhere);
            }
            else
            {
                sql = string.Format(@"select * from (select m.fullname,{3},nvl(sum({4}),0) total,max(m.sortcode) sortcode,
                                m.encode,max(m.departmentid) departmentid,max(m.parentid) parentid from base_department m 
                                left join(select {5},orgdept,orgdeptcode from(
                                select count(1) total,t.orgdept,t.orgdeptcode,t.drillmodename from mae_drillplanrecord t
                                left join base_department d on d.encode = substr(t.orgdeptcode,0,length(d.encode))
                                where d.nature='����' and t.iscommit=1  {0}
                                group by t.orgdept,t.orgdeptcode,t.drillmodename)pivot(max(total)for drillmodename in({2}))) p on p.orgdeptcode=m.encode
                                where 1=1  {1}
                                group by m.fullname,m.encode) order by sortcode", strWhere1, strWhere, strWhere2, selWhere, sumWhere,strWhere3);
//                sql = string.Format(@"select * from (select m.fullname,nvl(sum(p.total),0) recordnum,nvl(sum(p.total),0) total,max(m.sortcode) sortcode,
//                                m.encode,max(m.departmentid) departmentid,max(m.parentid) parentid from base_department m 
//                                left join(
//                                select count(1) total,t.orgdept,t.orgdeptcode from mae_drillplanrecord t
//                                left join base_department d on d.encode = substr(t.orgdeptcode,0,length(d.encode))
//                                where d.nature='����' and t.iscommit=1 {0}
//                                group by t.orgdept,t.orgdeptcode) p on p.orgdeptcode=m.encode
//                                where 1=1 {1}
//                                group by m.fullname,m.encode) order by sortcode", strWhere1, strWhere);
            }

            return this.BaseRepository().FindTable(sql);
        }
        public DataTable DrillplanStatDetail(string drillmode, bool isCompany, string deptCode, string starttime, string endtime)
        {
            string strWhere = string.Empty;
            string strWhere1 = string.Empty;
            if (!string.IsNullOrWhiteSpace(drillmode))
            {
                strWhere1 += string.Format(" and t.drillmodename='{0}'", drillmode);
            }
            if (isCompany)
            {
                strWhere += "and m.nature in ('����','����')";
            }
            if (!string.IsNullOrWhiteSpace(deptCode))
            {
                strWhere += string.Format("and m.encode like'{0}%'", deptCode);
            }
            if (!string.IsNullOrWhiteSpace(starttime))
            {
                strWhere1 += string.Format("and to_char(t.drilltime,'yyyy-MM-dd') >='{0}'", starttime);
            }
            if (!string.IsNullOrWhiteSpace(endtime))
            {
                strWhere1 += string.Format("and to_char(t.drilltime,'yyyy-MM-dd') <='{0}'", endtime);
            }
            string sql = string.Empty;
            sql = string.Format(@"select * from (select nvl(sum(p.total),0) recordnum,m.fullname,m.encode,max(m.sortcode) sortcode from base_department m 
                                left join(
                                select count(1) total,t.orgdept,t.orgdeptcode from mae_drillplanrecord t
                                left join base_department d on d.encode = substr(t.orgdeptcode,0,length(d.encode))
                                where d.nature='����' and t.iscommit=1 {0}
                                group by t.orgdept,t.orgdeptcode) p on p.orgdeptcode=m.encode
                                where 1=1 {1}
                                group by m.fullname,m.encode) order by sortcode", strWhere1, strWhere);
            return this.BaseRepository().FindTable(sql);

        }
        #endregion

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="condition">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<DrillplanrecordEntity> GetListForCon(Expression<Func<DrillplanrecordEntity, bool>> condition)
        {
            return this.BaseRepository().IQueryable(condition).ToList();
        }

        /// <summary>
        /// ��ȡ����Ӧ�������ƻ�̨��
        /// </summary>
        /// <param name="strsql"></param>
        /// <returns></returns>
        public DataTable GetBZList(String strsql)
        {
            return this.BaseRepository().FindTable(strsql);
        }

        /// <summary>
        /// �û��б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;

            if (queryJson.Length > 0)
            {
                var queryParam = queryJson.ToJObject();
                //��ѯ����
                if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
                {
                    string condition = queryParam["condition"].ToString();
                    string keyord = queryParam["keyword"].ToString();
                    switch (condition)
                    {
                        case "DrillTypeName":            //�˻�
                            pagination.conditionJson += string.Format(" and DrillTypeName  like '%{0}%'", keyord);
                            break;
                        case "Name":          //����
                            pagination.conditionJson += string.Format(" and Name  like '%{0}%'", keyord);
                            break;
                        case "DrillModeName":          //�ֻ�
                            pagination.conditionJson += string.Format(" and DrillModeName like '%{0}%'", keyord);
                            break;
                        default:
                            break;
                    }
                }
                if (!queryParam["DrillType"].IsEmpty())
                {
                    string DrillType = queryParam["DrillType"].ToString();
                    pagination.conditionJson += string.Format(" and DrillType = '{0}'", DrillType);
                }
                if (!queryParam["DrillMode"].IsEmpty())
                {
                    string DrillMode = queryParam["DrillMode"].ToString();
                    pagination.conditionJson += string.Format(" and DrillMode = '{0}'", DrillMode);
                }
                if (!queryParam["name"].IsEmpty())
                {
                    string name = queryParam["name"].ToString();
                    pagination.conditionJson += string.Format(" and Name  like '%{0}%'", name);
                }

                if (!queryParam["happentimestart"].IsEmpty())
                {
                    string happentimestart = queryParam["happentimestart"].ToString();
                    pagination.conditionJson += string.Format(" and drilltime >= (select  to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') from dual)", happentimestart);
                }
                if (!queryParam["happentimeend"].IsEmpty())
                {
                    string happentimeend = queryParam["happentimeend"].ToString();
                    pagination.conditionJson += string.Format(" and drilltime <= (select  to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') from dual)", happentimeend);
                }
                #region Ȩ���ж�
                if (!queryParam["code"].IsEmpty() && !queryParam["isOrg"].IsEmpty())
                {
                    var isOrg = queryParam["isOrg"].ToString();
                    var deptCode = queryParam["code"].ToString();
                    if (isOrg == "Organize")
                    {
                        pagination.conditionJson += string.Format(" and CREATEUSERORGCODE  like '{0}%'", deptCode);
                    }

                    else
                    {
                        pagination.conditionJson += string.Format(" and orgdeptcode like '{0}%'", deptCode);
                    }
                }
                if (!queryParam["IndexMode"].IsEmpty())
                {
                    var IndexMode = queryParam["IndexMode"].ToString();
                    string strWhere = string.Empty;
                    switch (IndexMode)
                    {
                        case "1":
                            string[] arrRole = user.RoleName.Split(',');
                            strWhere += string.Format(" and evaluatedeptid ='{0}' and isstartconfig=1 and iscommit=1 and isoverevaluate=0 and createuserorgcode='{1}' ", user.DeptId, user.OrganizeCode);
                            strWhere += " and (";
                            foreach (string str in arrRole)
                            {
                                strWhere += string.Format(" evaluaterole  like '%{0}%' or", str);
                            }
                            strWhere = strWhere.Substring(0, strWhere.Length - 2);
                            strWhere += " )";
                            break;
                        case "2":
                            strWhere += string.Format(@" and isassessrecord ='1' and iscommit=1 and assessperson like'%{0}%'", user.UserId);
                           
                            break;
                        case "3":
                            //��ǰ��δ�ύ����Ҫ���ƵĲ�������  �����˼��ƻ�ִ���˶���������
                            strWhere += string.Format(@" and iscommit =0  and (createuserid='{0}' or  executepersonid ='{0}')", user.UserId);

                            break;
                        default:
                            break;
                    }
                    pagination.conditionJson += strWhere;
                }

                if (!queryParam["deptcode"].IsEmpty())
                {
                    string deptcode = queryParam["deptcode"].ToString();
                    var dept = new DepartmentService().GetEntityByCode(deptcode);
                    if (dept != null) {
                        if (dept.Nature == "����")
                        {
                            pagination.conditionJson += string.Format(" and orgdeptcode = '{0}'", deptcode);
                        }
                        else {
                            pagination.conditionJson += string.Format(" and orgdeptcode like '{0}%'", deptcode);
                        }
                    }
                }
                if (!queryParam["drillmodename"].IsEmpty())
                {
                    string drillmodename = queryParam["drillmodename"].ToString();
                    pagination.conditionJson += string.Format(" and drillmodename = '{0}'", drillmodename);
                }
                #endregion
            }

            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// ��ȡ��������ͼ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public Flow GetEvaluateFlow(string keyValue)
        {
            List<nodes> nlist = new List<nodes>();
            List<lines> llist = new List<lines>();
            var isendflow = false;//���̽������
            string flowid = string.Empty;
            Flow flow = new Flow();
            flow.title = "";
            flow.initNum = 22;
            Operator currUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var EvaluateEntity = this.BaseRepository().FindEntity(keyValue);
            flowid = EvaluateEntity.NodeId;
            if (EvaluateEntity.IsOverEvaluate == 1)
            {
                isendflow = true;
            }
            string moduleName = string.Empty;
            if (EvaluateEntity != null)
            {
                switch (EvaluateEntity.DrillLevel)
                {

                    case "����":
                        moduleName = "����������¼����";
                        break;
                    case "���ż�":
                        moduleName = "���ż�������¼����";
                        break;
                    case "���鼶":
                        moduleName = "���鼶������¼����";
                        break;
                    default:
                        break;

                }
            }
            string flowSql = string.Format(@"select t.flowname,t.id,t.serialnum,t.checkrolename,
                                            t.checkroleid,t.checkdeptid,t.checkdeptcode,
                                            d.evaluateperson,d.evaluatetime,d.evaluatedept,
                                            d.evaluateopinion,t.checkdeptname
                                        from  bis_manypowercheck t 
                                             left join mae_drillrecordevaluate d on d.nodeid=t.id and d.drillrecordid='{2}'
                                        where t.createuserorgcode='{0}' and t.modulename='{1}' 
                                              order by t.serialnum asc", currUser.OrganizeCode, moduleName, keyValue);
            DataTable dt = this.BaseRepository().FindTable(flowSql);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    nodes nodes = new nodes();
                    nodes.alt = true;
                    nodes.isclick = false;
                    nodes.css = "";
                    nodes.id = dr["id"].ToString(); //����
                    nodes.img = "";
                    nodes.name = dr["flowname"].ToString();
                    nodes.type = "stepnode";
                    //λ��
                    int m = i % 4;
                    int n = i / 4;
                    if (m == 0)
                    {
                        nodes.left = 120;
                    }
                    else
                    {
                        nodes.left = 120 + ((150 + 60) * m);
                    }
                    if (n == 0)
                    {
                        nodes.top = 54;
                    }
                    else
                    {
                        nodes.top = (n * 100) + 54;
                    }
                    setInfo sinfo = new setInfo();
                    sinfo.NodeName = nodes.name;

                    //��˼�¼
                    if (dr["evaluateperson"] != null && !string.IsNullOrEmpty(dr["evaluateperson"].ToString()))
                    {
                        sinfo.Taged = 1;
                        List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                        DateTime auditdate;
                        DateTime.TryParse(dr["evaluatetime"].ToString(), out auditdate);
                        nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                        nodedesignatedata.creatdept = dr["evaluatedept"].ToString();
                        nodedesignatedata.createuser = dr["evaluateperson"].ToString();
                        nodedesignatedata.status = dr["evaluateopinion"].ToString();
                        if (i == 0)
                        {
                            nodedesignatedata.prevnode = "��";
                        }
                        else
                        {
                            nodedesignatedata.prevnode = dt.Rows[i - 1]["flowname"].ToString();
                        }

                        nodelist.Add(nodedesignatedata);
                        sinfo.NodeDesignateData = nodelist;
                        nodes.setInfo = sinfo;
                    }
                    else
                    {

                        if (dr["id"].ToString() == flowid)
                        {
                            sinfo.Taged = 0;
                        }
                        List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                        nodedesignatedata.createdate = "��";
                        var checkDeptId = dr["checkdeptid"].ToString();
                        if (checkDeptId == "-1")//������̱�� -1 Ϊִ�е�λ=������̵ķ�����λ
                        {
                            var deptentity = new DepartmentService().GetEntityByCode(EvaluateEntity.CREATEUSERDEPTCODE);
                            if (deptentity != null)
                            {
                                if (deptentity.Nature == "����")
                                {
                                    var parentDept = new DepartmentService().GetParentDeptBySpecialArgs(deptentity.ParentId, "����");
                                    if (parentDept != null)
                                    {
                                        checkDeptId = parentDept.DepartmentId;
                                        nodedesignatedata.creatdept = parentDept.FullName;
                                    }
                                }
                            }
                            else
                            {
                                nodedesignatedata.creatdept = "��";
                            }
                        }
                        else if (checkDeptId == "-3")
                        {
                            var deptentity = new DepartmentService().GetEntityByCode(EvaluateEntity.CREATEUSERDEPTCODE);
                            if (deptentity != null)
                            {
                                checkDeptId = deptentity.DepartmentId;
                                nodedesignatedata.creatdept = deptentity.FullName;
                            }
                            else
                            {
                                nodedesignatedata.creatdept = "��";
                            }
                        }
                        else
                        {
                            nodedesignatedata.creatdept = dr["checkdeptname"].ToString();
                        }
                        string userNames = new ScaffoldService().GetUserName(checkDeptId, dr["checkrolename"].ToString(), "0").Split('|')[0];
                        nodedesignatedata.createuser = !string.IsNullOrEmpty(userNames) ? userNames : "��";

                        nodedesignatedata.status = "��";
                        if (i == 0)
                        {
                            nodedesignatedata.prevnode = "��";
                        }
                        else
                        {
                            nodedesignatedata.prevnode = dt.Rows[i - 1]["flowname"].ToString();
                        }

                        nodelist.Add(nodedesignatedata);
                        sinfo.NodeDesignateData = nodelist;
                        nodes.setInfo = sinfo;
                    }
                    nlist.Add(nodes);
                }
                //���̽����ڵ�
                nodes nodes_end = new nodes();
                nodes_end.alt = true;
                nodes_end.isclick = false;
                nodes_end.css = "";
                nodes_end.id = Guid.NewGuid().ToString();
                nodes_end.img = "";
                nodes_end.name = "���̽���";
                nodes_end.type = "endround";
                nodes_end.width = 150;
                nodes_end.height = 60;
                //ȡ���һ���̵�λ�ã������λ
                nodes_end.left = nlist[nlist.Count - 1].left;
                nodes_end.top = nlist[nlist.Count - 1].top + 100;
                nlist.Add(nodes_end);

                //���״̬Ϊ���ͨ����ͨ�������̽������б�ʶ 
                if (isendflow)
                {
                    setInfo sinfo = new setInfo();
                    sinfo.NodeName = nodes_end.name;
                    sinfo.Taged = 1;
                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                    NodeDesignateData nodedesignatedata = new NodeDesignateData();

                    //ȡ���̽���ʱ�Ľڵ���Ϣ
                    if (!string.IsNullOrWhiteSpace(flowid))
                    {
                        DataRow[] end_rows = dt.Select("id = '" + flowid + "'");
                        DataRow end_row = end_rows[0];
                        DateTime auditdate;
                        DateTime.TryParse(end_row["evaluatetime"].ToString(), out auditdate);
                        nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                        nodedesignatedata.creatdept = end_row["evaluatedept"].ToString();
                        nodedesignatedata.createuser = end_row["evaluateperson"].ToString();
                        nodedesignatedata.status = end_row["evaluateopinion"].ToString();
                        nodedesignatedata.prevnode = end_row["flowname"].ToString();
                    }
                    else
                    {
                        //��ʷ������Flowid 
                        nodedesignatedata.createdate = "";
                        nodedesignatedata.creatdept = "";
                        nodedesignatedata.createuser = "";
                        nodedesignatedata.status = "";
                        nodedesignatedata.prevnode = "";
                    }
                    nodelist.Add(nodedesignatedata);
                    sinfo.NodeDesignateData = nodelist;
                    nodes_end.setInfo = sinfo;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    lines lines = new lines();
                    lines.alt = true;
                    lines.id = Guid.NewGuid().ToString();
                    lines.from = dt.Rows[i]["id"].ToString();
                    if (i < dt.Rows.Count - 1)
                    {
                        lines.to = dt.Rows[i + 1]["id"].ToString();
                    }
                    lines.name = "";
                    lines.type = "sl";
                    llist.Add(lines);
                }

                lines lines_end = new lines();
                lines_end.alt = true;
                lines_end.id = Guid.NewGuid().ToString();
                lines_end.from = dt.Rows[dt.Rows.Count - 1]["id"].ToString();
                lines_end.to = nodes_end.id;
                llist.Add(lines_end);

                flow.nodes = nlist;
                flow.lines = llist;
            }
            return flow;
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<DrillplanrecordEntity> GetList(string queryJson)
        {
            return this.BaseRepository().FindList(" select * from v_mae_Drillplanrecord where 1=1 " + queryJson).ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public DrillplanrecordEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// ��ȡ��ʷ��¼ʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        public DrillplanrecordHistoryEntity GetHistoryEntity(string keyValue)
        {
            Repository<DrillplanrecordHistoryEntity> historyReg = new Repository<DrillplanrecordHistoryEntity>(DbFactory.Base());
            return historyReg.FindEntity(keyValue);
        }
        /// <summary>
        /// ���ݵ�ǰ��½�˻�ȡ��ҳ����������
        /// </summary>
        /// <returns></returns>
        public int GetDrillPlanRecordEvaluateNum(Operator currUser)
        {
            string role = currUser.RoleName;
            string deptId = string.Empty;
            string deptName = string.Empty;

            //��˾���û�ȡ��������
            if (role.Contains("��˾���û�"))
            {
                deptId = currUser.OrganizeId;  //����ID
                deptName = currUser.OrganizeName;//��������
            }
            else
            {
                deptId = currUser.DeptId; //����ID
                deptName = currUser.DeptName; //����ID
            }
            string sql = string.Empty;
            string[] arrRole = role.Split(',');

            string strWhere = string.Empty;

            sql = string.Format("select count(id) evaluatenum from mae_drillplanrecord d ");
            strWhere += string.Format(" where d.evaluatedeptid ='{0}' and d.isstartconfig=1 and d.iscommit=1 and d.isoverevaluate=0 and d.createuserorgcode='{1}' ", currUser.DeptId, currUser.OrganizeCode);
            strWhere += " and (";
            foreach (string str in arrRole)
            {
                strWhere += string.Format(" d.evaluaterole  like '%{0}%' or", str);
            }
            strWhere = strWhere.Substring(0, strWhere.Length - 2);
            strWhere += " )";
            sql = sql + strWhere;
            var dt = this.BaseRepository().FindTable(sql);

            if (dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0]["evaluatenum"].ToString());
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// ���ݵ�ǰ��½�˻�ȡ��ҳ����������
        /// </summary>
        /// <returns></returns>
        public int GetDrillPlanRecordAssessNum(Operator currUser)
        {
            string role = currUser.RoleName;
            string deptId = string.Empty;
            string deptName = string.Empty;

            //��˾���û�ȡ��������
            if (role.Contains("��˾���û�"))
            {
                deptId = currUser.OrganizeId;  //����ID
                deptName = currUser.OrganizeName;//��������
            }
            else
            {
                deptId = currUser.DeptId; //����ID
                deptName = currUser.DeptName; //����ID
            }
            string strWhere = string.Empty;
            string sql = string.Empty;
            sql = string.Format("select count(id) evaluatenum from mae_drillplanrecord d ");
            strWhere += string.Format(" where d.isassessrecord ='1' and d.iscommit=1 and d.assessperson like'%{0}%' and d.createuserorgcode='{1}' ", currUser.UserId, currUser.OrganizeCode);
            sql = sql + strWhere;
            var dt = this.BaseRepository().FindTable(sql);

            if (dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0]["evaluatenum"].ToString());
            }
            else
            {
                return 0;
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
        public void SaveForm(string keyValue, DrillplanrecordEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                DrillplanrecordEntity se = this.BaseRepository().FindEntity(keyValue);
                if (se == null)
                {
                    entity.ID = keyValue;
                    entity.Create();
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
        /// <summary>
        /// ������ʷ��¼
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void SaveHistoryForm(string keyValue, DrillplanrecordHistoryEntity entity)
        {
            var res = DbFactory.Base().BeginTrans();
            try
            {
                res.Insert<DrillplanrecordHistoryEntity>(entity);
                Repository<DrillrecordAssessEntity> audit = new Repository<DrillrecordAssessEntity>(DbFactory.Base());
                List<DrillrecordAssessEntity> list = audit.FindList(string.Format(@"select * from  MAE_DRILLRECORDASSESS t where t.DrillRecordId='{0}'", entity.HistoryId)).ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].DrillRecordId = entity.ID;
                }
                res.Update<DrillrecordAssessEntity>(list);
                res.Commit();
            }
            catch (Exception)
            {

                res.Rollback();
            }
        }
        #endregion

    }
}
