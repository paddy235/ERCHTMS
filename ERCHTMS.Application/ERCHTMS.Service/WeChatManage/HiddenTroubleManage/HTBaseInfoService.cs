using ERCHTMS.Entity.BaseManage;
using ERCHTMS.IService.BaseManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.IService.HiddenTroubleManage;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.Data;
using System;
using ERCHTMS.Service.BaseManage;
using BSFramework.Application.Entity;

namespace ERCHTMS.Service.HiddenTroubleManage
{
    /// <summary>
    /// �� ��������������Ϣ��
    /// </summary>
    public class HTBaseInfoService : RepositoryFactory<HTBaseInfoEntity>, HTBaseInfoIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<HTBaseInfoEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }

        public IList<HTBaseInfoEntity> GetListByCode(string hidcode)
        {
            return this.BaseRepository().IQueryable().ToList().Where(p => p.HIDCODE == hidcode).ToList();
        }

        /// <summary>
        /// ������鼯��
        /// </summary>
        /// <param name="checkId"></param>
        /// <param name="checkman"></param>
        /// <returns></returns>
        public DataTable GetList(string checkId, string checkman, string districtcode, string workstream)
        {
            string sql = @"select t.id,t.hidcode,e.changeperson   from bis_htbaseinfo t 
                                                inner join v_htchangeinfo e on t.hidcode=e.hidcode 
                                                where 1=1";
            if (!string.IsNullOrEmpty(checkId))
            {
                sql += string.Format(" and t.safetycheckobjectid = '{0}'", checkId);
            }
            if (!string.IsNullOrEmpty(checkman))
            {
                sql += string.Format(" and t.checkman = '{0}'", checkman);
            }
            if (!string.IsNullOrEmpty(districtcode))
            {
                sql += string.Format(" and t.hidpoint = '{0}'", districtcode);
            }
            if (!string.IsNullOrEmpty(workstream))
            {
                sql += string.Format(" and t.workstream = '{0}'", workstream);
            }
            return this.BaseRepository().FindTable(sql);
        }

        #region ͨ����ǰ�û���ȡ��Ӧ��������������
        /// <summary>
        /// ͨ����ǰ�û���ȡ��Ӧ��������������
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable GetDescribeListByUserId(string userId, string hiddescribe)
        {

            string sql = string.Format(@"select a.*  from (select a.createuserdeptcode,a.createuserorgcode,a.createuserid, a.CREATEDATE,
                                       a.checktypename,a.hidtypename,a.hidrankname,a.checkdepartname,a.isgetafter,              
                                      a.id,a.hidproject,a.checktype, a.hidtype,a.hidrank,a.hidplace,a.hidpoint,a.workstream ,a.addtype ,
                                    c.postponedept ,c.postponedeptname ,a.hiddescribe,c.changemeasure  ��row_number() over( order by a.createdate desc) as rn  from v_htbaseinfo a
                                                                            left join v_workflow b on a.id = b.id 
                                                                            left join v_htchangeinfo c on a.hidcode = c.hidcode
                                                                            left join v_htacceptinfo d on a.hidcode = d.hidcode
                                                                            left join v_principal e on c.changedutydepartcode = e.departmentcode
                                                                             where 1=1  and createuserid ='{0}'  order by a.createdate desc
                                                                             ) a  where rn <=10  ", userId);

            if (!hiddescribe.IsEmpty())
            {
                sql += string.Format(@" and hiddescribe like '%{0}%'", hiddescribe);
            }

            return this.BaseRepository().FindTable(sql);
        }
        #endregion

        public DataTable GetGeneralQuery(string sql, Pagination pagination)
        {
            var dt = this.BaseRepository().FindTable(sql, pagination);
            return dt;
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public HTBaseInfoEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        #region Υ���б�
        /// <summary>
        /// Υ���б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetRulerPageList(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();

            string sql = string.Format("select t.ID,t.CHECKDATE,t.HIDDESCRIBE,e.CHANGEMEASURE from BIS_HTBASEINFO t left join v_htchangeinfo e on t.HIDCODE=e.HIDCODE where 1=1 ");
            pagination.p_fields = "ID,FINDDATE,HIDDESCRIBE,CHANGEMEASURE";
            pagination.p_kid = "ID";
            //��ѯ����
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyord = queryParam["keyword"].ToString();
                sql += string.Format(" and t.ISBREAKRULE =1 and t.BREAKRULEUSERIDS  like '%{0}%'", keyord);
            }
            else
            {
                sql += " and 1=2";
            }
            var dt = this.BaseRepository().FindTable(sql, pagination);
            return dt;
        }
        #endregion

        #endregion

        #region  ����������Ϣ
        /// <summary>
        /// ����������Ϣ
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetHiddenBaseInfoPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;

            pagination.p_fields = @" account,createuserdeptcode,createuserorgcode,createuserid,checktypename,hidtypename,hidrankname,checkdepartname,
                                    id,createdate,hidcode,checktype,isgetafter,exposurestate,isbreakrule,hidtype,acceptdepartcode,acceptperson,changeperson,
                                    hidrank,hidpoint,workstream,addtype,participant,applicationstatus,postponedept,postponedeptname,hiddescribe,changemeasure,
                                    changedeadine,changedutydepartcode,safetycheckobjectid,principal";



            pagination.p_kid = "id";

            pagination.conditionJson = " 1=1";

            var queryParam = queryJson.ToJObject();

            string userId = queryParam["userId"].ToString();  //��ǰ��ID

            UserEntity user = new UserService().GetEntity(userId); //��ǰ�û�

            if (!queryParam["qWorkstream"].IsEmpty())
            {
                pagination.p_tablename = @"v_basehiddeninfo";
            }
            else
            {
                pagination.p_tablename = @"v_hiddenbasedata";
            }
            //��ѯ����
            if (!queryParam["action"].IsEmpty())
            {
                string action = queryParam["action"].ToString();

                switch (action)
                {
                    //�����Ǽ�
                    case "Register":
                        pagination.conditionJson += string.Format(@" and  createuserid ='{0}' ", user.UserId);
                        break;
                    //������׼
                    case "Approval":
                        pagination.conditionJson += @" and workstream  = '������׼'";
                        break;
                    //��������
                    case "Change":
                        pagination.conditionJson += @" and workstream  = '��������'";
                        break;
                    //��������
                    case "Accept":
                        pagination.conditionJson += @" and workstream  = '��������'";
                        break;
                    //����Ч������
                    case "Estimate":
                        pagination.conditionJson += @" and workstream  = '����Ч������'";
                        break;
                    //���Ľ���
                    case "BaseEnd":
                        pagination.conditionJson += @" and workstream  = '���Ľ���'";
                        break;
                    //�������ڽ׶ε�����
                    case "Postpone":
                        pagination.conditionJson += @" and applicationstatus  in ('1','2')";
                        break;

                }
            }
            //����״̬
            if (!queryParam["ChangeStatus"].IsEmpty())
            {
                switch (queryParam["ChangeStatus"].ToString())
                {
                    case "δ����":
                        pagination.conditionJson += @" and workstream = '��������' ";
                        break;
                    case "����δ����":
                        pagination.conditionJson += string.Format(@" and workstream = '��������'  and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') > changedeadine + 1", DateTime.Now);
                        break;
                    case "��������":
                        pagination.conditionJson += @" and  hidcode in (select distinct hidcode from bis_htextension where handlesign ='1')";
                        break;
                    case "��������δ����":
                        pagination.conditionJson += @"and workstream = '��������' and ((hidrankname = 'һ������' and changedeadine - 3 <= 
                        sysdate  and sysdate <= changedeadine + 1 )  or (hidrankname like '%�ش�%' and changedeadine - 5 <= sysdate and  sysdate <= changedeadine + 1 ) )";
                        break;
                    case "���˵Ǽ�":
                        pagination.conditionJson += string.Format(@" and  createuserid ='{0}'", "");
                        break;
                    case "������":
                        pagination.conditionJson += @" and   workstream in ('��������','����Ч������','���Ľ���')"; //changeresult ='1' and
                        break;
                    case "���ƶ���":
                        pagination.conditionJson += @" and  isgetafter ='1'";
                        break;
                }
            }
            //���ݷ�Χ
            if (!queryParam["DataScope"].IsEmpty())
            {
                switch (queryParam["DataScope"].ToString())
                {
                    case "���˵Ǽ�":
                        pagination.conditionJson += string.Format(@" and  createuserid ='{0}'", user.UserId);
                        break;
                    case "���˺�׼":
                        pagination.conditionJson += string.Format(@" and participant  like  '%{0}%'", user.Account);
                        break;
                    case "�����ź�׼":
                        string departmentCode = user.DepartmentCode;
                        //��˾���û�
                        if (new UserInfoService().HaveRoleListByKey(user.UserId, Config.GetValue("HidOrganize")).Rows.Count > 0)
                        {
                            departmentCode = user.OrganizeCode;
                        }
                        pagination.conditionJson += string.Format(@" and  hidcode in  (select distinct hidcode from v_approvaldata where departmentcode ='{0}' and name ='������׼')", departmentCode);
                        break;
                    case "��������":
                        pagination.conditionJson += string.Format(@" and changeperson  =  '{0}'", user.UserId);
                        break;
                    case "����������":
                        pagination.conditionJson += string.Format(@" and changedutydepartcode  =  '{0}'", user.DepartmentCode);
                        break;
                    case "��������":
                        pagination.conditionJson += string.Format(@" and acceptperson  =  '{0}' and workstream = '��������'", user.UserId);
                        break;
                    case "����������":
                        pagination.conditionJson += string.Format(@" and acceptdepartcode  =  '{0}' and workstream = '��������' ", user.DepartmentCode);
                        break;
                    case "������������":
                        pagination.conditionJson += string.Format(@" and acceptperson  =  '{0}' and  workstream = '����Ч������' ", user.UserId);
                        break;
                    case "��������������":
                        pagination.conditionJson += string.Format(@" and acceptdepartcode  =  '{0}' and workstream = '����Ч������' ", user.DepartmentCode);
                        break;
                    case "������(��)��":
                        pagination.conditionJson += string.Format(@" and  ((applicationstatus ='2' and postponedept  like  '%,{0},%')", user.DepartmentCode);
                        string tSql = string.Format(@"  select * from v_principal a  where    (useraccount || ',')  like '{0},%' and departmentcode ='{1}'", user.Account, user.DepartmentCode);
                        //���Ÿ�����
                        var tempData = this.BaseRepository().FindTable(tSql); //��ѯ�Ƿ���ڸ�����
                        if (tempData.Rows.Count > 0)
                        {
                            pagination.conditionJson += string.Format(" or  (applicationstatus ='1' and changedutydepartcode ='{0}'))", user.DepartmentCode);
                        }
                        else
                        {
                            pagination.conditionJson = pagination.conditionJson.Replace("((", "(");
                        }
                        break;
                    case "��������(��)��":
                        pagination.conditionJson += string.Format(@"  and  ((applicationstatus ='2' and postponedept  like  '%,{0},%')
                                                         or  (applicationstatus ='1' and changedutydepartcode  = '{0}'))", user.DepartmentCode);
                        break;
                }
            }
            //ȷ�����ݷ�Χ
            if (!queryParam["qWorkstream"].IsEmpty())
            {
                string isPlanLevel = "0";

                if (!queryParam["isPlanLevel"].IsEmpty())
                {
                    isPlanLevel = queryParam["isPlanLevel"].ToString();
                }
                string deptcode = "";

                //��ǰ�û��ǹ�˾���������û�
                if (isPlanLevel == "1")
                {
                    deptcode = user.OrganizeCode;
                }
                else
                {
                    deptcode = user.DepartmentCode;
                }


                string tempSql = string.Format(@"select  distinct to_char(changedutydepartcode) as encode  from v_htchangeinfo where changedutydepartcode like '{0}%'
                                                 union 
                                                 select distinct to_char(a.encode) as encode from base_department a 
                                                 left join base_department b on a.senddeptid = b.departmentid 
                                                 where  a.senddeptid is not null  and b.encode like '{0}%'", deptcode);

                var tempDataTable = this.BaseRepository().FindTable(tempSql);

                string strDeptCode = "";

                foreach (DataRow row in tempDataTable.Rows)
                {
                    strDeptCode += "'" + row["encode"].ToString() + "',";
                }

                if (!strDeptCode.IsEmpty())
                {
                    strDeptCode = strDeptCode.Substring(0, strDeptCode.Length - 1);
                }
                pagination.conditionJson += string.Format(@"   and  changedutydepartcode  in ({0})  and  to_char(createdate,'yyyy') ='{1}'", strDeptCode, DateTime.Now.Year.ToString());

            }
            //����״̬
            if (!queryParam["WorkStream"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and workstream = '{0}'", queryParam["WorkStream"].ToString());
            }
            //�������
            if (!queryParam["SaftyCheckType"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and checktype = '{0}'", queryParam["SaftyCheckType"].ToString());
            }
            //��������
            if (!queryParam["HidType"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and hidtype = '{0}'", queryParam["HidType"].ToString());
            }
            //�Ƿ�Υ��
            if (!queryParam["IsBreakRule"].IsEmpty())
            {
                string isbreakrule = queryParam["IsBreakRule"].ToString();
                //�Ƿ�Υ��
                if (isbreakrule == "1")
                {
                    pagination.conditionJson += @" and isbreakrule = '1'";
                }
                else
                {
                    pagination.conditionJson += @"  and  (isbreakrule = '0' or  isbreakrule is null)";
                }
            }
            //�Ƿ��ع�
            if (!queryParam["IsExposureState"].IsEmpty())
            {
                string exposurestate = queryParam["IsExposureState"].ToString();
                //�Ƿ��ع�
                if (exposurestate == "1")
                {
                    pagination.conditionJson += @"  and exposurestate = '1'";
                }
                else
                {
                    pagination.conditionJson += @"  and  (exposurestate = '0'  or  exposurestate is null)";
                }
            }
            //���� Or ����
            if (!queryParam["isOrg"].IsEmpty())
            {
                string isOrg = queryParam["isOrg"].ToString();
                string code = string.Empty;
                if (!queryParam["code"].IsEmpty())
                {
                    code = queryParam["code"].ToString();
                }
                if (isOrg == "Organize" && !code.IsEmpty())
                {
                    //���ű���
                    pagination.conditionJson += string.Format(@" and createuserorgcode  like '{0}%'", queryParam["code"].ToString());
                }
                if (isOrg == "Department" && !code.IsEmpty())
                {
                    //���ű���
                    pagination.conditionJson += string.Format(@" and createuserdeptcode  like '{0}%'", queryParam["code"].ToString());
                }
            }
            //��������
            if (!queryParam["HidPoint"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and  hidpoint='{0}' ", queryParam["HidPoint"].ToString());
            }
            //�����ĿID
            if (!queryParam["checkId"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and safetycheckobjectid ='{0}'", queryParam["checkId"].ToString());
            }
            if (!queryParam["checkType"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and safetycheckobjectid in (select id from bis_saftycheckdatadetailed  where recid='{0}')", queryParam["checkType"].ToString());
            }
            //��������
            if (!queryParam["HidRank"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and hidrank = '{0}'", queryParam["HidRank"].ToString());
            }
            //��������
            if (!queryParam["HidDescribe"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and hiddescribe like '%{0}%'", queryParam["HidDescribe"].ToString());
            }
            //����������ʼʱ��
            if (!queryParam["StartTime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and createdate >=to_date('{0}','yyyy-mm-dd hh24:mi:ss')", queryParam["StartTime"].ToString());
            }
            //������������ʱ��
            if (!queryParam["EndTime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and createdate <= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(queryParam["EndTime"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
            }
            //��ǰ����
            if (!user.OrganizeCode.IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and  createuserorgcode = '{0}' ", user.OrganizeCode);
            }

            var dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);

            return dt;
        }
        #endregion

        #region  ��ѯ�����������
        /// <summary>
        /// ��ѯ�����������
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public DataTable GetHiddenByKeyValue(string keyValue)
        {
            string sql = string.Format(@"select a.id as hiddenid,
                                        a.hidcode as problemid,
                                        a.isbreakrule as breakrulebehavior,
                                        a.hidrank , 
                                        a.hidrankname as rankname,
                                        a.hidtype as categoryid,
                                        a.hidtypename as category,
                                        a.hidpoint as  hidpointid ,
                                        a.hidpointname as hidpoint,
                                        a.hiddescribe,
                                        c.changeperson as dutypersonid,
                                        c.changepersonname as dutyperson,
                                        c.changedutydepartcode as dutydeptcode,
                                        c.changedutydepartcode as dutydeptid,
                                        c.changedutydepartname as dutydept,
                                        c.changedutytel as  dutytel,
                                        c.changedeadine as deadinetime,
                                        c.changefinishdate as reformfinishdate,
                                        c.changemeasure as reformmeasure,
                                        c.changeresume as reformdescribe,
                                        c.applicationstatus,
                                        c.postponedays,
                                        c.postponedept,
                                        c.postponedeptname,
                                        c.backreason,
                                        h.postponeresult,
                                        h.postponeopinion,
                                        h.handleuserid,
                                        h.handleusername,
                                        h.handledeptcode,
                                        h.handledeptname,
                                        h.handledate,
                                        a.addtype as reformtype,
                                        d.acceptperson as checkpersonid,
                                        d.acceptpersonname as checkperson ,
                                        d.acceptdepartcode  ,
                                        d.acceptdepartname,
                                        d.acceptdate as checktime,
                                        d.acceptidea  as checkopinion,
                                        a.checkman ,
                                        a.checkmanname , 
                                        a.checkdepartid as checkdept,
                                        a.checkdepartname as  checkdeptname,
                                        d.acceptstatus as checkresult,
                                        a.workstream,
                                        a.exposurestate as isexpose,
                                        c.planmanagecapital  ,
                                        c.realitymanagecapital,
                                        a.checktype as checktypeid,
                                        a.checktypename as checktype,
                                        a.hidplace as dangerlocation,
                                        a.reportdigest as reportsummary,
                                        a.hidreason as causereason,
                                        a.hiddangerlevel as damagelevel,
                                        a.preventmeasure,
                                        a.hidchageplan as reformplan,
                                        a.exigenceresume as replan,
                                        a.isgetafter as tosupervise,
                                        a.breakruleusernames as breakruleperson, 
                                        a.breakruleuserids as breakrulepersonid,
                                        a.traintemplateid as trainframeworkid,
                                        a.traintemplatename as trainframework,
                                        a.hidphoto,
                                        c.hidchangephoto,
                                        d.acceptphoto,
                                        a.checkdate,
                                        f.approvalperson,
                                        f.approvalpersonname,
                                        f.approvaldepartcode ,
                                        f.approvaldepartname,
                                        f.approvaldate,
                                        f.approvalresult,
                                        f.approvalreason,
                                        g.estimateperson ,
                                        g.estimatepersonname,
                                        g.estimatedepartcode,
                                        g.estimatedepartname,
                                        g.estimatedate,
                                        g.estimatedepart,
                                        g.estimaterank,
                                        g.estimateresult,
                                        g.estimatephoto 
                                        from v_htbaseinfo a
                                        left join v_workflow b on a.id = b.id 
                                        left join v_htchangeinfo c on a.hidcode = c.hidcode
                                        left join v_htacceptinfo d on a.hidcode = d.hidcode
                                        left join v_principal e on c.changedutydepartcode = e.departmentcode  
                                        left join v_htapprovalinfo f on a.hidcode = f.hidcode 
                                        left join v_htestimateinfo g on  a.hidcode = g.hidcode 
                                        left join v_htextensioninfo h on a.hidcode = h.hidcode
                                        where 1=1 and a.id ='{0}'", keyValue);

            return this.BaseRepository().FindTable(sql);
        }
        #endregion

        #region �����ع�
        /// <summary>
        /// �����ع�
        /// </summary>
        /// <returns></returns>
        public DataTable GetHiddenByIsExpose()
        {
            string sql = @" select a.id as hiddenid,
                            a.hidcode as problemid,
                            a.isbreakrule as breakrulebehavior,
                            a.hidrank , 
                            a.hidrankname as rankname,
                            a.hidtype as categoryid,
                            a.hidtypename as category,
                            a.hidpoint as  hidpointid ,
                            a.hidpointname as hidpoint,
                            a.hiddescribe,
                            c.changeperson as dutypersonid,
                            c.changepersonname as dutyperson,
                            c.changedutydepartcode as dutydeptcode,
                            c.changedutydepartcode as dutydeptid,
                            c.changedutydepartname as dutydept,
                            c.changedutytel as  dutytel,
                            c.changedeadine as deadinetime,
                            c.changefinishdate as reformfinishdate,
                            c.changemeasure as reformmeasure,
                            c.changeresume as reformdescribe,
                            a.addtype as reformtype,
                            d.acceptperson as checkpersonid,
                            d.acceptpersonname as checkperson ,
                            d.acceptdepartcode  ,
                            d.acceptdepartname,
                            d.acceptdate as checktime,
                            a.checkman ,
                            a.checkmanname , 
                            a.checkdepartid as checkdept,
                            a.checkdepartname  as  checkdeptname,
                            d.acceptstatus as checkresult,
                            a.workstream,
                            a.exposurestate as isexpose,
                            c.planmanagecapital  ,
                            c.realitymanagecapital,
                            a.checktype as checktypeid,
                            a.checktypename as checktype,
                            a.hidplace as dangerlocation,
                            a.reportdigest as reportsummary,
                            a.hidreason as causereason,
                            a.hiddangerlevel as damagelevel,
                            a.preventmeasure,
                            a.hidchageplan as reformplan,
                            a.exigenceresume as replan,
                            a.isgetafter as tosupervise,
                            a.breakruleusernames as breakruleperson, 
                            a.breakruleuserids as breakrulepersonid,
                            a.traintemplateid as trainframeworkid,
                            a.traintemplatename as trainframework,
                            a.hidphoto,
                            c.hidchangephoto,
                            d.acceptphoto,
                            a.checkdate,
                            f.approvalperson,
                            f.approvalpersonname,
                            f.approvaldepartcode ,
                            f.approvaldepartname,
                            f.approvaldate,
                            f.approvalresult,
                            f.approvalreason,
                            g.estimateperson ,
                            g.estimatepersonname,
                            g.estimatedepartcode,
                            g.estimatedepartname,
                            g.estimatedate,
                            g.estimatedepart,
                            g.estimaterank,
                            g.estimateresult,
                            g.estimatephoto 
                            from v_htbaseinfo a
                            left join v_workflow b on a.id = b.id 
                            left join v_htchangeinfo c on a.hidcode = c.hidcode
                            left join v_htacceptinfo d on a.hidcode = d.hidcode
                            left join v_principal e on c.changedutydepartcode = e.departmentcode  
                            left join v_htapprovalinfo f on a.hidcode = f.hidcode 
                            left join v_htestimateinfo g on  a.hidcode = g.hidcode 
                            where 1=1 and a.exposurestate ='1'";

            return this.BaseRepository().FindTable(sql);
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
        public void SaveForm(string keyValue, HTBaseInfoEntity entity)
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

        #region ��ѯ����ͳ��
        /// <summary>
        /// ����ͳ��
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="year"></param>
        /// <param name="area"></param>
        /// <param name="hidrank"></param>
        /// <param name="userId"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public DataTable QueryStatisticsByAction(StatisticsEntity sentity)
        {
            string sql = string.Empty;

            string str = " 1=1";

            int mark = 0;  //��Ǳ���  0 �������������� ��1 ��ʶ1���������� 2 ��ʶ����������������

            //����Χ
            if (!sentity.sArea.IsEmpty())
            {
                str += string.Format("  and hidpoint like '{0}%'", sentity.sArea.ToString());
            }
            //��������
            if (!sentity.sHidRank.IsEmpty())
            {
                string[] tempRank = sentity.sHidRank.Split(',');

                mark = tempRank.Length;  //��Ǳ���

                string args = "";

                foreach (string s in tempRank)
                {
                    args += "'" + s.Trim() + "',";
                }
                if (!args.IsEmpty())
                {
                    args = args.Substring(0, args.Length - 1);
                }

                str += string.Format("  and  rankname in ({0})", args);
            }
            //�������
            if (!sentity.isCheck.IsEmpty())
            {
                str += @"  and  safetycheckobjectid is not null";

                if (!sentity.sCheckType.IsEmpty())
                {
                    str += string.Format(@"  and  checktype = (select a.itemdetailid from BASE_DATAITEMDETAIL a
                                            left join base_dataitem b on a.itemid = b.itemid  where b.itemcode ='SaftyCheckType' and a.itemvalue ='{0}') ", sentity.sCheckType);
                }
            }

            switch (sentity.sAction)
            {

                //�����ȼ������б�
                case "1":
                    #region ������˾�û�
                    if (sentity.isCompany)
                    {
                        sql = string.Format(@"
                                            select a.hidpoint,a.hidpointname,nvl(a.OrdinaryHid,0) OrdinaryHid,nvl(a.ImportanHid,0) ImportanHid,(nvl(a.OrdinaryHid,0) + nvl(a.ImportanHid,0)) as total  from (
                                                select * from (
                                                select a.districtcode as hidpoint,a.districtname as hidpointname,a.organizeid ,nvl(b.y,0) as pnum ,b.rankname from BIS_DISTRICT a
                                                    left join (
                                                    select  count(1) as y ,substr(hidpoint,0,6) as hidpoint,rankname   from v_basehiddeninfo 
                                                    where to_char(createdate,'yyyy') ='{0}' and  changedutydepartcode  like '{1}%'  group by hidpoint,rankname 
                                                    ) b on a.districtcode = b.hidpoint 
                                                    where a.parentid = '0' and a.organizeid ='{2}'
                                                ) pivot (sum(pnum) for rankname in ('һ������' as OrdinaryHid,'�ش�����' as ImportanHid))) a ", sentity.sYear, sentity.sDeptCode, sentity.sOrganize);
                    }
                    else
                    {
                        sql = string.Format(@"select sys_guid() as hidpoint,0 as hidpointname, nvl(a.OrdinaryHid,0) OrdinaryHid,nvl(a.ImportanHid,0) ImportanHid,(nvl(a.OrdinaryHid,0) + nvl(a.ImportanHid,0)) as total  from (
                                        select * from (
                                                           select  count(1) as pnum ,rankname   from v_basehiddeninfo 
                                                           where to_char(createdate,'yyyy') ='{0}' and  changedutydepartcode  like '{1}%'  group by rankname 
                                        ) pivot (sum(pnum) for rankname in ('һ������' as OrdinaryHid,'�ش�����' as ImportanHid))) a ", sentity.sYear, sentity.sDeptCode);
                    }
                    #endregion
                    break;
                //�����ȼ�����ͳ��ͼ
                case "2":
                    #region �����ȼ�����ͳ��ͼ
                    sql = string.Format(@"select  count(1) as y ,rankname  as name from v_basehiddeninfo where changedutydepartcode  like '{0}%' 
                                          and to_char(createdate,'yyyy') ='{1}'  group by  rankname  ", sentity.sDeptCode, sentity.sYear);
                    #endregion
                    break;
                //��������ֲ����ͼ
                case "3":
                    #region ��������ֲ����ͼ
                    if (sentity.isCompany)
                    {
                        sql = string.Format(@"select a.districtcode as  hidpoint,a.districtname as name,a.organizeid ,sum(nvl(b.y,0)) as y from BIS_DISTRICT a
                                                left join (
                                                select  count(1) as y ,substr(hidpoint,0,6) as hidpoint  from v_basehiddeninfo 
                                                where to_char(createdate,'yyyy') ='{0}' and  changedutydepartcode  like '{1}%'  group by hidpoint
                                                ) b on a.districtcode = b.hidpoint 
                                                where a.parentid = '0' and a.organizeid ='{2}' group by 
                                                a.districtcode ,a.districtname ,a.organizeid", sentity.sYear, sentity.sDeptCode, sentity.sOrganize);
                    }
                    #endregion
                    break;
                /****���������仯����ͼ****/
                case "4":
                    #region ���������仯����ͼ
                    if (mark == 0)
                    {
                        sql = string.Format(@"select a.month,nvl(b.allhid,0) allhid  from (select lpad(level,2,0) as month from dual connect by level <13) a
                                              left join (
                                                   select * from (
                                                           select  count(1) as pnum , to_char(createdate,'MM') as tMonth ,'��������' as rankname from v_basehiddeninfo where changedutydepartcode  like '{0}%' 
                                                           and to_char(createdate,'yyyy') ='{1}' and {2}    group by to_char(createdate,'MM')
                                                       ) pivot (sum(pnum) for rankname in ('��������' as AllHid))
                                              ) b on a.month =b.tMonth order by a.month", sentity.sDeptCode, sentity.sYear, str);
                    }
                    else if (mark == 1)
                    {
                        string tempStr = sentity.sHidRank == "һ������" ? " 'һ������' as OrdinaryHid " : " '�ش�����' as ImportanHid";
                        string tempField = sentity.sHidRank == "һ������" ? "OrdinaryHid" : "ImportanHid";
                        sql = string.Format(@"select a.month, nvl(b.{4},0) {4}  from (select lpad(level,2,0) as month from dual connect by level <13) a
                                              left join (
                                                     select * from (    
                                                       select  count(1) as pnum , to_char(createdate,'MM') as tMonth ,rankname from v_basehiddeninfo where changedutydepartcode  like '{0}%' 
                                                       and to_char(createdate,'yyyy') ='{1}'  and  {2} group by to_char(createdate,'MM') ,rankname 
                                                   ) pivot (sum(pnum) for rankname in ({3})) 
                                               ) b on a.month =b.tMonth order by a.month",
                                      sentity.sDeptCode, sentity.sYear, str, tempStr, tempField);
                    }
                    else
                    {
                        sql = string.Format(@"select a.month,  nvl(b.OrdinaryHid,0) OrdinaryHid,  nvl(b.ImportanHid,0) ImportanHid from (select lpad(level,2,0) as month from dual connect by level <13) a
                                              left join ( 
                                                       select * from (
                                                        select  count(1) as pnum , to_char(createdate,'MM') as tMonth ,rankname from v_basehiddeninfo where changedutydepartcode  like '{0}%' 
                                                            and to_char(createdate,'yyyy') ='{1}'  and  {2}  group by to_char(createdate,'MM') ,rankname 
                                                            ) pivot (sum(pnum) for rankname in ('һ������' as OrdinaryHid,'�ش�����' as ImportanHid))
                                              ) b on a.month =b.tMonth order by a.month",
                                      sentity.sDeptCode, sentity.sYear, str);
                    }
                    #endregion
                    break;
                /****���ȼ��������ͳ��****/
                case "5":
                    #region ���ȼ��������ͳ��
                    bool deptMark = false;
                    DepartmentEntity dentity = new DepartmentService().GetEntityByCode(sentity.sDeptCode);
                    if (null == dentity)
                    {
                        OrganizeEntity orgentity = new OrganizeService().GetEntityByCode(sentity.sDeptCode);
                        //��ǰѡ����ǻ���
                        if (null != orgentity)
                        {
                            deptMark = true;
                        }
                    }

                    //����
                    if (deptMark)
                    {
                        sql = string.Format(@"select a.changedutydepartcode,nvl(a.OrdinaryHid,0) OrdinaryHid,nvl(a.ImportanHid,0) ImportanHid,(nvl(a.OrdinaryHid,0) + nvl(a.ImportanHid,0)) as total,b.fullname  from (
                                                select * from ( select  count(1) as pnum , substr(changedutydepartcode,0,6) as changedutydepartcode,rankname  from v_basehiddeninfo where changedutydepartcode  like '{0}%' 
                                                and to_char(createdate,'yyyy') ='{1}' and  length(changedutydepartcode)>3  and {2}  group by  substr(changedutydepartcode,0,6),rankname 
                                            ) pivot (sum(pnum) for rankname in ('һ������' as OrdinaryHid,'�ش�����' as ImportanHid))) a 
                                            left join base_department b on a.changedutydepartcode = b.encode order by b.sortcode ",

                          sentity.sDeptCode, sentity.sYear, str);
                    }
                    else //�ǳ���
                    {
                        sql = string.Format(@"select a.changedutydepartcode,nvl(a.OrdinaryHid,0) OrdinaryHid,nvl(a.ImportanHid,0) ImportanHid,(nvl(a.OrdinaryHid,0) + nvl(a.ImportanHid,0)) as total,b.fullname  from (
                                                select * from ( select  count(1) as pnum , changedutydepartcode,rankname  from v_basehiddeninfo where changedutydepartcode  like '{0}%' 
                                                and to_char(createdate,'yyyy') ='{1}' and  length(changedutydepartcode)>3  and {2}  group by  changedutydepartcode,rankname  
                                            ) pivot (sum(pnum) for rankname in ('һ������' as OrdinaryHid,'�ش�����' as ImportanHid))) a 
                                            left join base_department b on a.changedutydepartcode = b.encode order by b.sortcode ",
                                           sentity.sDeptCode, sentity.sYear, str);
                    }
                    #endregion
                    break;
                /****�����������ͳ��ͼ****/
                case "6":
                    #region �����������ͳ��ͼ
                    sql = string.Format(@"select a.month,nvl(b.yValue,0) yValue,nvl(c.wValue,0) wValue from (select lpad(level,2,0) as month from dual connect by level <13) a
                                        left join 
                                        (
                                           select count(1) as yValue , to_char(createdate,'MM') as yMonth from v_basehiddeninfo  where workstream in ('��������','����Ч������','���Ľ���') and 
                                           changedutydepartcode  like '{0}%' and to_char(createdate,'yyyy') ='{1}' and {2}   group by  to_char(createdate,'MM')
                                        ) b on a.month = b.yMonth
                                        left join 
                                        (
                                          select count(1) as wValue, to_char(createdate,'MM') as wMonth from v_basehiddeninfo  where   workstream = '��������' and 
                                           changedutydepartcode  like '{0}%' and to_char(createdate,'yyyy') ='{1}' and {2}  group by  to_char(createdate,'MM')
                                        ) c on a.month = c.wMonth  order by a.month", sentity.sDeptCode, sentity.sYear, str);
                    #endregion
                    break;
                /****���������������ͼ****/
                case "7":
                    #region ���������������ͼ
                    sql = string.Format(@"select a.month , nvl(b.aValue,0) aValue , nvl(c.yValue,0) yValue, nvl(d.aiValue,0) aiValue , nvl(e.iValue,0) iValue,
                                           nvl(f.aoValue,0) aoValue , nvl(g.oValue,0) oValue,
                                           round((case when nvl(b.aValue,0) = 0 then 0 else  nvl(c.yValue,0) / nvl(b.aValue,0) * 100 end ),2) aChangeVal,
                                           round((case when nvl(d.aiValue,0) = 0 then 0 else  nvl(e.iValue,0) / nvl(d.aiValue,0) * 100 end ),2) iChangeVal,
                                           round((case when nvl(f.aoValue,0) = 0 then 0 else  nvl(g.oValue,0) / nvl(f.aoValue,0) * 100 end ),2) oChangeVal
                                           from (select lpad(level,2,0) as month from dual connect by level <13) a
                                            left join 
                                            (
                                              select count(1) as aValue, to_char(createdate,'MM') as aMonth  from v_basehiddeninfo  where  workstream in ( '��������','��������','����Ч������','���Ľ���')   and 
                                              changedutydepartcode  like '{0}%' and to_char(createdate,'yyyy') ='{1}' and {2} group by  to_char(createdate,'MM')
                                            ) b on a.month = b.aMonth 
                                            left join 
                                            (
                                              select count(1) as yValue, to_char(createdate,'MM') as yMonth from v_basehiddeninfo  where  workstream in ( '��������','����Ч������','���Ľ���')   and 
                                              changedutydepartcode  like '{0}%' and to_char(createdate,'yyyy') ='{1}' and {2} group by  to_char(createdate,'MM')
                                            ) c on a.month = c.yMonth 
                                            left join 
                                            (
                                              select count(1) as aiValue, to_char(createdate,'MM') as aiMonth  from v_basehiddeninfo  where  workstream in ( '��������','��������','����Ч������','���Ľ���') and rankname in ('�ش�����')  and 
                                              changedutydepartcode  like '{0}%' and to_char(createdate,'yyyy') ='{1}' and {2} group by  to_char(createdate,'MM')
                                            ) d on a.month = d.aiMonth
                                            left join 
                                            (
                                              select count(1) as iValue, to_char(createdate,'MM') as iMonth from v_basehiddeninfo  where  workstream in ( '��������','����Ч������','���Ľ���') and rankname in ('�ش�����')  and 
                                              changedutydepartcode  like '{0}%' and to_char(createdate,'yyyy') ='{1}' and {2} group by  to_char(createdate,'MM')
                                            ) e on a.month = e.iMonth 
                                            left join 
                                            (
                                              select count(1) as aoValue, to_char(createdate,'MM') as aoMonth  from v_basehiddeninfo  where  workstream in ( '��������','��������','����Ч������','���Ľ���') and rankname in ('һ������')  and 
                                              changedutydepartcode  like '{0}%' and to_char(createdate,'yyyy') ='{1}' and {2} group by  to_char(createdate,'MM')
                                            ) f on a.month = f.aoMonth 
                                            left join 
                                            (
                                              select count(1) as oValue, to_char(createdate,'MM') as oMonth from v_basehiddeninfo  where  workstream in ( '��������','����Ч������','���Ľ���') and rankname in ('һ������')  and 
                                              changedutydepartcode  like '{0}%' and to_char(createdate,'yyyy') ='{1}' and {2} group by  to_char(createdate,'MM')
                                            ) g on a.month = g.oMonth 
                                             order by a.month ", sentity.sDeptCode, sentity.sYear, str);
                    #endregion
                    break;
                /****������������Ա�ͼ****/
                case "8":
                    #region ������������Ա�ͼ
                    bool dMark = false;
                    DepartmentEntity dtentity = new DepartmentService().GetEntityByCode(sentity.sDeptCode);

                    if (null == dtentity)
                    {
                        OrganizeEntity orgentity = new OrganizeService().GetEntityByCode(sentity.sDeptCode);
                        //��ǰѡ����ǻ���
                        if (null != orgentity)
                        {
                            dMark = true;
                        }
                    }

                    //����  ���в���
                    if (dMark)
                    {

                        sql = string.Format(@"select a.changedutydepartcode,nvl(a.nonChange,0) nonChange,nvl(a.thenChange,0) thenChange ,(nvl(a.nonChange,0) + nvl(a.thenChange,0)) as total,b.fullname  from (
                                                  select * from (
                                                            select  count(1) as pValue, substr(changedutydepartcode,0,6) as changedutydepartcode,'������' changestatus  from v_basehiddeninfo  where  workstream in ('��������','����Ч������','���Ľ���')  and 
                                                            changedutydepartcode  like '{0}%' and  length(changedutydepartcode)>3  and to_char(createdate,'yyyy') ='{1}' and {2}  group by  substr(changedutydepartcode,0,6)
                                                            union 
                                                            select  count(1) as pValue, substr(changedutydepartcode,0,6) as changedutydepartcode,'δ����' changestatus  from v_basehiddeninfo  where  workstream in ('��������')  and 
                                                            changedutydepartcode  like '{0}%' and  length(changedutydepartcode)>3  and to_char(createdate,'yyyy') ='{1}' and {2}  group by  substr(changedutydepartcode,0,6)
                                                        ) pivot (sum(pValue) for changestatus in ('δ����' as nonChange,'������' as thenChange))
                                                    ) a 
                                                    left join base_department b on a.changedutydepartcode = b.encode order by b.sortcode ",
                                       sentity.sDeptCode, sentity.sYear, str);

                    }
                    else //�ǳ���
                    {
                        sql = string.Format(@"select a.changedutydepartcode,nvl(a.nonChange,0) nonChange,nvl(a.thenChange,0) thenChange ,(nvl(a.nonChange,0) + nvl(a.thenChange,0)) as total,b.fullname  from (
                                                  select * from (
                                                        select  count(1) as pValue, changedutydepartcode ,'������' changestatus  from v_basehiddeninfo  where  workstream in ( '��������','����Ч������','���Ľ���')  and 
                                                        changedutydepartcode  like '{0}%' and  length(changedutydepartcode)>3  and to_char(createdate,'yyyy') ='{1}' and  {2}   group by  changedutydepartcode
                                                        union 
                                                        select  count(1) as pValue, changedutydepartcode ,'δ����' changestatus  from v_basehiddeninfo  where  workstream in ( '��������')  and 
                                                        changedutydepartcode  like '{0}%' and  length(changedutydepartcode)>3  and to_char(createdate,'yyyy') ='{1}' and  {2}  group by  changedutydepartcode
                                                    ) pivot (sum(pValue) for changestatus in ('δ����' as nonChange,'������' as thenChange))
                                                  ) a 
                                                  left join base_department b on a.changedutydepartcode = b.encode order by b.sortcode ",
                                        sentity.sDeptCode, sentity.sYear, str);
                    }
                    #endregion
                    break;
                /********��������б�**********/
                case "9":
                    #region ��������б�
                    sql = string.Format(@"select a.month,  nvl(b.OrdinaryHid,0) OrdinaryHid,  nvl(b.ImportanHid,0) ImportanHid ,(nvl(b.OrdinaryHid,0) + nvl(b.ImportanHid,0))  total from (select lpad(level,2,0) as month from dual connect by level <13) a
                                            left join ( 
                                                     select * from (
                                                      select  count(1) as pnum , to_char(createdate,'MM') as tMonth ,rankname from v_basehiddeninfo where changedutydepartcode  like '{0}%' 
                                                          and to_char(createdate,'yyyy') ='{1}' and  length(changedutydepartcode)>3   and  {2}  group by to_char(createdate,'MM') ,rankname 
                                                          ) pivot (sum(pnum) for rankname in ('һ������' as OrdinaryHid,'�ش�����' as ImportanHid))
                                            ) b on a.month =b.tMonth  order by a.month", sentity.sDeptCode, sentity.sYear, str);
                    #endregion
                    break;

            }

            if (!sql.IsEmpty())
            {
                var dt = this.BaseRepository().FindTable(sql);

                return dt;
            }
            else
            {
                return new DataTable();
            }
        }
        #endregion

        #region ˫�ع����б�
        /// <summary>
        /// ˫�ع���
        /// </summary>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataTable QueryHidWorkList(string deptcode, string organizecode)
        {

            string sql = string.Format(@" select '0' as itemdetailid, 'ȫ������' as itemvalue ,  sum(total) as total  ,sum(yzgsl) as yzgsl ,sum(wzgsl) as wzgsl ,sum(yqwzgsl) as yqwzgsl,
                                         (case when  sum(total) =0 then 0 else  round(sum(yzgsl) *100 / sum(total),2)  end) as yhzgl  from (
                                          select a.itemdetailid, a.itemvalue, nvl(b.pnum,0) as total , nvl(c.pnum,0) as yzgsl, nvl(d.pnum,0) as wzgsl , nvl(e.pnum,0) as yqwzgsl  from 
                                          (  select a.itemdetailid ,itemvalue from base_dataitemdetail a
                                          left join base_dataitem b on a.itemid = b.itemid
                                          where b.itemcode ='HidRank') a
                                          left join (
                                             select  count(1) as pnum ,hidrank from v_basechangeinfo where createuserorgcode ='{2}' and  ( changedutydepartcode  like '{0}%' or changedutydepartcode in 
                                             (select a.encode from base_department a left join base_department b on a.senddeptid = b.departmentid 
                                             where  a.senddeptid is not null  and b.encode like '{0}%'  ) ) and to_char(createdate,'yyyy') ='{1}'
                                             group by hidrank
                                          ) b on a.itemdetailid =b.hidrank
                                          left join (
                                             select  count(1) as pnum , hidrank from v_basechangeinfo where createuserorgcode ='{2}' and ( changedutydepartcode  like '{0}%' or changedutydepartcode in 
                                             (select a.encode from base_department a left join base_department b on a.senddeptid = b.departmentid 
                                             where   a.senddeptid is not null  and b.encode like '{0}%'  ) ) and to_char(createdate,'yyyy') ='{1}'
                                             and  workstream in ('��������','����Ч������','���Ľ���')  group by hidrank
                                          ) c on a.itemdetailid =c.hidrank
                                          left join (
                                             select  count(1) as pnum ,hidrank from v_basechangeinfo where createuserorgcode ='{2}' and ( changedutydepartcode  like '{0}%' or changedutydepartcode in 
                                             (select a.encode from base_department a left join base_department b on a.senddeptid = b.departmentid 
                                             where  a.senddeptid is not null  and b.encode like '{0}%'  ) ) and to_char(createdate,'yyyy') ='{1}'
                                             and  workstream = '��������'  group by hidrank
                                          ) d on a.itemdetailid =d.hidrank
                                          left join (
                                              select  count(1) as pnum , hidrank from v_basechangeinfo where createuserorgcode ='{2}' and ( changedutydepartcode  like '{0}%' or changedutydepartcode in 
                                             (select a.encode from base_department a left join base_department b on a.senddeptid = b.departmentid 
                                             where  a.senddeptid is not null  and b.encode like '{0}%'  ) ) and to_char(createdate,'yyyy') ='{1}'
                                              and  workstream = '��������'  and  sysdate > changedeadine + 1  group by hidrank
                                          ) e on a.itemdetailid =e.hidrank
                                      ) a 
                                      union 
                                       select a.itemdetailid, a.itemvalue, nvl(b.pnum,0) as total , nvl(c.pnum,0) as yzgsl, nvl(d.pnum,0) as wzgsl , nvl(e.pnum,0) as yqwzgsl,
                                      (case when  nvl(b.pnum,0) =0 then 0 else  round(nvl(c.pnum,0)*100  / nvl(b.pnum,0),2)  end) as yhzgl   from (  select a.itemdetailid ,itemvalue from base_dataitemdetail a
                                      left join base_dataitem b on a.itemid = b.itemid
                                      where b.itemcode ='HidRank') a
                                      left join (
                                         select  count(1) as pnum ,hidrank from v_basechangeinfo where createuserorgcode ='{2}' and ( changedutydepartcode  like '{0}%' or changedutydepartcode in 
                                             (select a.encode from base_department a left join base_department b on a.senddeptid = b.departmentid 
                                             where  a.senddeptid is not null  and b.encode like '{0}%'  ) ) and to_char(createdate,'yyyy') ='{1}'
                                         group by hidrank
                                      ) b on a.itemdetailid =b.hidrank
                                      left join (
                                         select  count(1) as pnum , hidrank from v_basechangeinfo where createuserorgcode ='{2}' and ( changedutydepartcode  like '{0}%' or changedutydepartcode in 
                                             (select a.encode from base_department a left join base_department b on a.senddeptid = b.departmentid 
                                             where  a.senddeptid is not null  and b.encode like '{0}%'  ) ) and to_char(createdate,'yyyy') ='{1}'
                                         and  workstream in ('��������','����Ч������','���Ľ���')  group by hidrank
                                      ) c on a.itemdetailid =c.hidrank
                                      left join (
                                         select  count(1) as pnum ,hidrank from v_basechangeinfo where createuserorgcode ='{2}' and ( changedutydepartcode  like '{0}%' or changedutydepartcode in 
                                             (select a.encode from base_department a left join base_department b on a.senddeptid = b.departmentid 
                                             where  a.senddeptid is not null  and b.encode like '{0}%'  ) ) and to_char(createdate,'yyyy') ='{1}'
                                         and  workstream = '��������'  group by hidrank
                                      ) d on a.itemdetailid =d.hidrank
                                      left join (
                                          select  count(1) as pnum , hidrank from v_basechangeinfo where createuserorgcode ='{2}' and ( changedutydepartcode  like '{0}%' or changedutydepartcode in 
                                             (select a.encode from base_department a left join base_department b on a.senddeptid = b.departmentid 
                                             where  a.senddeptid is not null  and b.encode like '{0}%'  ) ) and to_char(createdate,'yyyy') ='{1}'
                                          and  workstream = '��������'  and  sysdate > changedeadine + 1  group by hidrank
                                      ) e on a.itemdetailid =e.hidrank", deptcode, DateTime.Now.Year.ToString(), organizecode);

            var dt = this.BaseRepository().FindTable(sql);

            return dt;
        }
        #endregion

        #region �����¼
        /// <summary>
        /// �����¼
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable QueryHidBacklogRecord(string value, string userId)
        {
            UserEntity curUser = new UserService().GetEntity(userId); //��ǰ�û�

            string sql = "";

            string str = ""; //��ѯ����

            //���˴���ҵ��
            if (value == "0")
            {
                string roleCode = Config.GetValue("HidPrincipalSetting");

                IList<UserEntity> ulist = new UserService().GetUserListByRole(curUser.DepartmentCode, roleCode).ToList();

                //���صļ�¼��,����0����ʶ��ǰ�û�ӵ�а�ȫ����Ա��ݣ���֮����
                int returnVal = ulist.Where(p => p.UserId == curUser.UserId).Count();

                //��ǰ���Ǹ�����
                if (returnVal > 0)
                {
                    str = string.Format(@"  ((applicationstatus ='2' and postponedept  like  '%,{0},%')
                                               or  (applicationstatus ='1' and changedutydepartcode ='{0}'))", curUser.DepartmentCode);
                }
                else
                {
                    str = string.Format(@"  (applicationstatus ='2' and postponedept  like  '%,{0},%')", curUser.DepartmentCode);
                }

                //���Լ������ҵ��

                sql = string.Format(@" select a.* from (
                                 select count(1) as pnum , 1 as serialnumber from v_htbaseinfo a
                                                    left join v_workflow b on a.id = b.id 
                                                    where 1=1 and a.createuserorgcode like '{0}%' and b.participant  like  '%{1}%'  and a.workstream ='������׼' 
                                   union
                                    select count(1) as pnum,2 as serialnumber from v_htbaseinfo a
                                                     left join v_htchangeinfo b on a.hidcode = b.hidcode
                                                     where 1=1 and a.createuserorgcode like '{0}%' and b.changeperson  = '{2}' and a.workstream ='��������' 
                                   union 
                                   select count(1) as pnum,3 as serialnumber from v_htbaseinfo a
                                               left join v_htchangeinfo b on a.hidcode = b.hidcode
                                               where 1=1 and a.createuserorgcode like '{0}%'  and  {3}
                                   union
                                      select count(1) as pnum,4 as serialnumber from v_htbaseinfo a
                                                     left join v_htacceptinfo b on a.hidcode = b.hidcode
                                                     where 1=1 and a.createuserorgcode like '{0}%' and acceptperson  = '{2}' and workstream = '��������' 
                                    union
                                            select count(1) as pnum,5 as serialnumber from v_htbaseinfo a
                                                     left join v_htacceptinfo b on a.hidcode = b.hidcode
                                                     where 1=1 and a.createuserorgcode like '{0}%' and acceptperson  =  '{2}' and workstream = '����Ч������' 
                                                     ) a  order by serialnumber", curUser.OrganizeCode, curUser.Account, curUser.UserId, str);
            }
            //���˴���,����δ����,���ϴ�������
            else if (value == "2")
            {
                sql = string.Format(@" select a.* from ( select count(1) as pnum , 1 as serialnumber from v_htbaseinfo a
                                          where 1=1 and a.createuserorgcode like '{0}%'   and  createuserid ='{1}'
                                        union
                                         select count(1) as pnum , 2 as serialnumber from v_htbaseinfo a
                                        left join v_htchangeinfo b on a.hidcode = b.hidcode
                                          where 1=1 and a.createuserorgcode like '{0}%' and  workstream = '��������'  and  sysdate > changedeadine + 1  
                                          and  changeperson ='{1}') a order by serialnumber", curUser.OrganizeCode, curUser.UserId);
            }
            else   //ȫ������ҵ��
            {

                sql = string.Format(@" select a.* from (
                                          select count(1) as pnum , 1 as serialnumber from v_htbaseinfo a
                                                        left join v_workflow b on a.id = b.id 
                                                        where 1=1 and a.createuserorgcode like '{0}%' and  a.hidcode in  (
                                                        select distinct hidcode from v_approvaldata a
                                                         where departmentcode = '{1}' and name ='������׼')  and a.workstream ='������׼'
                                                         union
                                          select count(1) as pnum , 2 as serialnumber from v_htbaseinfo a
                                                         left join v_htchangeinfo b on a.hidcode = b.hidcode
                                                         where 1=1 and a.createuserorgcode like '{0}%' and changedutydepartcode  =  '{1}' and a.workstream ='��������'
                                                         union
                                          select count(1) as pnum , 3 as serialnumber from v_htbaseinfo a
                                                         left join v_htchangeinfo b on a.hidcode = b.hidcode
                                                         where 1=1 and a.createuserorgcode like '{0}%'  and ((applicationstatus ='2' and postponedept  like  '%,{1},%')
                                                         or  (applicationstatus ='1' and changedutydepartcode  = '{1}'))
                                                         union
                                          select count(1) as pnum , 4 as serialnumber from v_htbaseinfo a
                                                         left join v_htacceptinfo b on a.hidcode = b.hidcode
                                                         where 1=1 and a.createuserorgcode like '{0}%' and acceptdepartcode  =  '{1}' and workstream = '��������' 
                                                         union
                                           select count(1) as pnum , 5 as serialnumber from v_htbaseinfo a
                                                         left join v_htacceptinfo b on a.hidcode = b.hidcode
                                                         where 1=1 and a.createuserorgcode like '{0}%' and acceptdepartcode  =  '{1}' and workstream = '����Ч������' 
                                      ) a  order by serialnumber ", curUser.OrganizeCode, curUser.DepartmentCode);
            }

            var dt = this.BaseRepository().FindTable(sql);

            return dt;
        }
        #endregion

        #region  ��ȡǰ�����ع������
        /// <summary>
        /// ��ȡǰ�����ع������
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public DataTable QueryExposureHid(string organizeid, string num)
        {

            string sql = string.Format(@" select a.* from (
                                        select distinct a.id ,a.hidcode ,a.hiddangername,a.hiddescribe,a.createdate,a.workstream,a.addtype,
                                         f.filepath ,a.createuserorgcode from v_htbaseinfo a
                                        left join v_imageview f on a.hidphoto = f.folderid  where a.exposurestate ='1' and a.createuserorgcode ='{1}') a where rownum <= {0} order by createdate  ", int.Parse(num), organizeid);

            var dt = this.BaseRepository().FindTable(sql);

            return dt;
        }
        #endregion

        #region �ֻ��˷�ҳ
        /// <summary>
        /// �ֻ��˷�ҳ
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="basetype"></param>
        /// <returns></returns>
        public DataTable GetBaseInfoForApp(Pagination pagination)
        {
            DatabaseType datatype = DbHelper.DbType;

            return this.BaseRepository().FindTableByProcPager(pagination, datatype);
        }
        #endregion

        #region ��ȡ�ֻ�������ͳ������
        /// <summary>
        /// ��ȡ�ֻ�������ͳ������
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable GetAppHidStatistics(string code, int mode)
        {
            string sql = "";

            if (mode == 0)  //������������
            {
                sql = string.Format(@"  select allhid ��ordinaryhid,importanhid    from (
                          select  count(1) as pnum ,rankname from v_basehiddeninfo
                           where changedutydepartcode  like '{0}%' 
                              and to_char(createdate,'yyyy') ='{1}'    group by rankname 
                             union
                             select  count(1) as pnum ,'ȫ������' as rankname from v_basehiddeninfo
                           where changedutydepartcode  like '{0}%' 
                              and to_char(createdate,'yyyy') ='{1}'
                           ) pivot (sum(pnum) for rankname in ('һ������' as OrdinaryHid,'�ش�����' as ImportanHid,'ȫ������' as AllHid))", code, DateTime.Now.Year.ToString());
            }
            else if (mode == 1) //�����������
            {
                sql = string.Format(@"select  b.yzgnum,c.wzgnum, (case when nvl(a.allnum,0) =0 then 0 else round(b.yzgnum * 100 / a.allnum,2) end)  as allzgl��
                                               e.zdyzgnum,f.zdwzgnum, (case when nvl(d.zdallnum,0) =0 then 0 else round(e.zdyzgnum * 100 / d.zdallnum,2) end)  as  zdzgl��
                                               h.ybyzgnum,i.ybwzgnum, (case when nvl(g.yballnum,0) =0 then 0 else round(h.ybyzgnum * 100 / g.yballnum,2) end)  as  ybzgl
                                        from (                                        
                                        select count(1) as allnum ,to_char(createdate,'yyyy') as createdate from v_basehiddeninfo  where  workstream 
                                        in ( '��������','��������','����Ч������','���Ľ���')  and 
                                         changedutydepartcode  like '{0}%' and to_char(createdate,'yyyy') ='{1}' group by to_char(createdate,'yyyy')
                                         )  a 
                                         left join (
                                       select count(1) as yzgnum,to_char(createdate,'yyyy') as createdate  from v_basehiddeninfo  where  
                                       workstream in ('��������','����Ч������','���Ľ���')   and 
                                       changedutydepartcode  like '{0}%' and to_char(createdate,'yyyy') ='{1}'  group by to_char(createdate,'yyyy') 
                                       )  b on a.createdate = b.createdate
                                        left join (
                                          select count(1) as wzgnum,to_char(createdate,'yyyy') as createdate  from v_basehiddeninfo  where  
                                       workstream in ('��������')   and 
                                       changedutydepartcode  like '{0}%' and to_char(createdate,'yyyy') ='{1}'  group by to_char(createdate,'yyyy')
                                       �� c on a.createdate = c.createdate     
                                       left join (
                                        select count(1) as  zdallnum ,to_char(createdate,'yyyy') as createdate from v_basehiddeninfo  where  workstream 
                                        in ( '��������','��������','����Ч������','���Ľ���') and rankname in ('�ش�����')  and 
                                         changedutydepartcode  like '{0}%' and to_char(createdate,'yyyy') ='{1}' group by to_char(createdate,'yyyy')
                                       )  d on a.createdate = d.createdate  
                                       left join (
                                       select count(1) as  zdyzgnum,to_char(createdate,'yyyy') as createdate  from v_basehiddeninfo  where  
                                       workstream in ('��������','����Ч������','���Ľ���') and rankname in ('�ش�����')   and 
                                       changedutydepartcode  like '{0}%' and to_char(createdate,'yyyy') ='{1}'  group by to_char(createdate,'yyyy')
                                       )  e on a.createdate = e.createdate 
                                       left join (
                                          select count(1) as zdwzgnum,to_char(createdate,'yyyy') as createdate  from v_basehiddeninfo  where  
                                       workstream in ('��������') and rankname in ('�ش�����')   and 
                                       changedutydepartcode  like '{0}%' and to_char(createdate,'yyyy') ='{1}'  group by to_char(createdate,'yyyy')
                                       ) f on a.createdate = f.createdate 
                                       left join (                                    
                                        select count(1) as yballnum ,to_char(createdate,'yyyy') as createdate from v_basehiddeninfo  where  workstream 
                                        in ( '��������','��������','����Ч������','���Ľ���') and rankname in ('һ������')  and 
                                         changedutydepartcode  like '{0}%' and to_char(createdate,'yyyy') ='{1}' group by to_char(createdate,'yyyy')
                                       ) g on a.createdate = g.createdate
                                       left join ( 
                                       select count(1) as ybyzgnum,to_char(createdate,'yyyy') as createdate  from v_basehiddeninfo  where  
                                       workstream in ('��������','����Ч������','���Ľ���') and rankname in ('һ������')   and 
                                       changedutydepartcode  like '{0}%' and to_char(createdate,'yyyy') ='{1}'  group by to_char(createdate,'yyyy')
                                       ) h on a.createdate = h.createdate 
                                       left join (
                                          select count(1) as ybwzgnum,to_char(createdate,'yyyy') as createdate  from v_basehiddeninfo  where  
                                       workstream in ('��������') and rankname in ('һ������')   and 
                                       changedutydepartcode  like '{0}%' and to_char(createdate,'yyyy') ='{1}'  group by to_char(createdate,'yyyy') 
                                       ) i on a.createdate = i.createdate  ", code, DateTime.Now.Year.ToString());

            }
            else if (mode == 3)   //��ȫ����µ�����
            {
                sql = string.Format(@" select a.itemname, nvl(b.allnum,0) as pnun from (
                                          select a.itemdetailid,a.itemname,a.itemvalue from  base_dataitemdetail  a
                                          left join  base_dataitem b on a.itemid = b.itemid 
                                          where b.itemcode = 'SaftyCheckType'
                                          ) a
                                          left join (
                                            select count(1) as allnum ,checktype from v_basehiddeninfo  where  workstream 
                                            in ( '��������','��������','����Ч������','���Ľ���')   and safetycheckobjectid is not null and
                                             changedutydepartcode  like '{0}%' and to_char(createdate,'yyyy') ='{1}'  group by checktype
                                           ) b on a.itemdetailid = b.checktype  order by a.itemvalue", code, DateTime.Now.Year.ToString());
            }

            var dt = this.BaseRepository().FindTable(sql);

            return dt;
        }
        #endregion
    }
}
