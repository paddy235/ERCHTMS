using ERCHTMS.Entity.PersonManage;
using ERCHTMS.IService.PersonManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.Data;
using ERCHTMS.IService.Desktop;
using System;
using System.Text;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.Service.BaseManage;
using ERCHTMS.Code;
using System.Collections;
using ERCHTMS.Service.SystemManage;
using ERCHTMS.IService.HiddenTroubleManage;
using ERCHTMS.Service.HiddenTroubleManage;
using ERCHTMS.IService.SystemManage;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.IService.SaftyCheck;
using ERCHTMS.Service.SaftyCheck;
using System.Data.Common;
using BSFramework.Data;
using ERCHTMS.Service.PersonManage;
using ERCHTMS.IService.LllegalManage;
using ERCHTMS.Service.LllegalManage;
using ERCHTMS.Service.HighRiskWork;
using ERCHTMS.Entity.Home;
using ERCHTMS.Service.DangerousJob;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Entity.Common;
using System.Net;

namespace ERCHTMS.Service.Desktop
{

    /// <summary>
    /// 描 述：首页服务对象
    /// </summary>
    public class DesktopService : RepositoryFactory<ScoreSetEntity>, DesktopIService
    {
        private IDepartmentService departmentIService = new DepartmentService();

        private IClassificationService iclassificationservice = new ClassificationService();
        private IClassificationIndexService classificationindexservices = new ClassificationIndexService();
        private HTBaseInfoIService htbaseinfoiservice = new HTBaseInfoService();
        private SaftyCheckDataRecordIService saftycheckdatarecordiservice = new SaftyCheckDataRecordService();
        private IDataItemDetailService idataitemdetailservice = new DataItemDetailService();
        private CertificateIService certificateiservice = new CertificateService();
        private LllegalRegisterIService lllegalregisteriservice = new LllegalRegisterService();
        private ManyPowerCheckService manypowercheckservice = new ManyPowerCheckService();
        private HighRiskCommonApplyService highriskcommonapplyservice = new HighRiskCommonApplyService();
        private DepartmentService departmentservice = new DepartmentService();
        private DangerousJobFlowDetailService DetailService = new DangerousJobFlowDetailService();
        private JobSafetyCardApplyService JobSafetyCardApplyService = new JobSafetyCardApplyService();
        private JobApprovalFormService JobApprovalFormService = new JobApprovalFormService();


        #region  通用版本的领导驾驶舱(电厂层级)

        #region 核心屏

        #region  预警指标、安全指标
        /// <summary>
        /// 预警指标、安全指标
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<DesktopPageIndex> GetPowerPlantWarningIndex(Operator user)
        {
            List<string> lstItems = GetDeptDataSet(user, "SSJK").AsEnumerable().Select(t => t.Field<string>("itemcode")).ToList();
            List<DesktopPageIndex> list = new List<DesktopPageIndex>();

            string sql = string.Empty;

            #region 预警指标

            #region 预警指标  CJLDJSC-001
            if (lstItems.Contains("YJZB001"))
            {
                //获取逾期未整改隐患
                sql = string.Format("select count(1) from v_basehiddeninfo where workstream = '隐患整改'  and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') > changedeadine + 1  and hiddepart='{1}' ", DateTime.Now, user.OrganizeId);

                DesktopPageIndex YJZB001 = new DesktopPageIndex();
                YJZB001.code = "YJZB001";
                YJZB001.name = "逾期未整改隐患";
                YJZB001.modulecode = "CJLDJSC-001";
                YJZB001.modulename = "预警指标";
                YJZB001.count = BaseRepository().FindObject(sql).ToInt().ToString();
                YJZB001.isdecimal = false;
                list.Add(YJZB001);
            }
            if (lstItems.Contains("YJZB005"))
            {
                sql = string.Format(@"select count(1) from v_basehiddeninfo where  workstream = '隐患整改' and ((rankname = '一般隐患' and changedeadine - 3 <= sysdate  and sysdate <= changedeadine + 1)  or(rankname = '重大隐患' and changedeadine - 5 <= sysdate and  sysdate <= changedeadine + 1) ) and hiddepart='{0}'  ", user.OrganizeId);

                DesktopPageIndex YJZB005 = new DesktopPageIndex();
                YJZB005.code = "YJZB005";
                YJZB005.name = "即将到期未整改隐患";
                YJZB005.modulecode = "CJLDJSC-001";
                YJZB005.modulename = "预警指标";
                YJZB005.count = BaseRepository().FindObject(sql).ToInt().ToString();
                YJZB005.isdecimal = false;
                list.Add(YJZB005);
            }

            if (lstItems.Contains("YJZB007"))
            {
                //获取个人分数
                decimal personScore = 12; //默认初始
                var dataitem = idataitemdetailservice.GetEntityByItemName("LllegalPointInitValue"); // 个人默认违章积分
                if (null != dataitem)
                {
                    if (!string.IsNullOrEmpty(dataitem.ItemValue))
                    {
                        personScore = Convert.ToDecimal(dataitem.ItemValue);//获取个人默认违章积分 
                    }
                }

                //违章积分低于8分的人员
                sql = string.Format(@"select count(1) from (
                       select a.userid,a.realname,a.account,({0} - sum(a.lllegalpoint))  pnum from v_lllegalassesforperson  a
                        left join (
                         select count(1) pnum ,recoveruserid,createdate  from v_lllegalpointrecoverdetail group by recoveruserid,createdate 
                      ) b on a.userid = b.recoveruserid  
                      where 1=1 and a.organizeid='{1}' and  to_char(a.lllegaltime,'yyyy')='{2}' and (nvl(b.pnum,0)=0  or  (nvl(b.pnum,0)>0  and  a.createdate > b.createdate)) group by a.userid,a.realname,a.account) a  where pnum<=8 ", personScore, user.OrganizeId, DateTime.Now.Year.ToString());

                DesktopPageIndex YJZB007 = new DesktopPageIndex();
                YJZB007.code = "YJZB007";
                YJZB007.name = "违章积分低于8分的人员";
                YJZB007.modulecode = "CJLDJSC-001";
                YJZB007.modulename = "预警指标";
                YJZB007.count = BaseRepository().FindObject(sql).ToInt().ToString();
                YJZB007.isdecimal = false;
                list.Add(YJZB007);
            }
            if (lstItems.Contains("YJZB002"))
            {
                //获取逾期未整改违章
                sql = string.Format("select count(1) from v_lllegalbaseinfo where  flowstate = '违章整改'  and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') > reformdeadline + 1  and reformdeptcode like '{1}%' ", DateTime.Now, user.OrganizeCode);

                DesktopPageIndex YJZB002 = new DesktopPageIndex();
                YJZB002.code = "YJZB002";
                YJZB002.name = "逾期未整改违章";
                YJZB002.modulecode = "CJLDJSC-001";
                YJZB002.modulename = "预警指标";
                YJZB002.count = BaseRepository().FindObject(sql).ToInt().ToString();
                YJZB002.isdecimal = false;
                list.Add(YJZB002);
            }

            if (lstItems.Contains("YJZB003"))
            {
                sql = string.Format(@"select count(1) from v_lllegalbaseinfo where  flowstate = '违章整改' and (reformdeadline - 3 <= sysdate  and sysdate <= reformdeadline + 1) and  reformdeptcode like '{0}%'  ", user.OrganizeCode);

                DesktopPageIndex YJZB006 = new DesktopPageIndex();
                YJZB006.code = "YJZB006";
                YJZB006.name = "即将到期未整改违章";
                YJZB006.modulecode = "CJLDJSC-001";
                YJZB006.modulename = "预警指标";
                YJZB006.count = BaseRepository().FindObject(sql).ToInt().ToString();
                YJZB006.isdecimal = false;
                list.Add(YJZB006);

                //一般隐患整改率低于100%的部门
                sql = string.Format(@"select count(1) from (
                  select nvl((case when total >0 then round( yzgtotal /total*100,2) else null end),0) zgl ，a.departmentname,a.encode  from (
                        select  sum(total) total,  departmentname,encode  from (
                            select  nvl(b.total,0) total,a.fullname departmentname ,a.encode from 
                            ( 
                             select encode ,fullname ,departmentid,organizeid,deptcode from base_department t where organizeid = '{0}'  and nature ='部门' 
                            )  a 
                            left join   
                            (
                              select changedutydepartcode ,count(1) total from v_basehiddeninfo where rankname ='一般隐患' and  to_char(createdate,'yyyy') ='{1}'  group by changedutydepartcode
                            ) b on a.encode = substr(b.changedutydepartcode,1,length(a.encode))  
                            union
                            select  nvl(b.total,0) total ,a.fullname departmentname,a.encode from 
                            ( 
                             select encode ,fullname ,departmentid,organizeid,deptcode from base_department t where organizeid = '{0}'   and nature ='厂级' 
                            )  a 
                            left join   
                            (
                              select changedutydepartcode ,count(1) total from v_basehiddeninfo where rankname ='一般隐患' and  to_char(createdate,'yyyy') ='{1}'  group by changedutydepartcode
                            ) b on a.encode = b.changedutydepartcode  
                        ) a group by  departmentname,encode
                    )  a 
                    left join 
                    (
                      select  sum(total) yzgtotal,  departmentname,encode  from (
                            select  nvl(b.total,0) total,a.fullname departmentname ,a.encode from 
                            ( 
                             select encode ,fullname ,departmentid,organizeid,deptcode from base_department t where organizeid = '{0}'  and nature ='部门' 
                            )  a 
                            left join   
                            (
                              select changedutydepartcode ,count(1) total from v_basehiddeninfo where rankname ='一般隐患' and workstream !='隐患整改' and  to_char(createdate,'yyyy') ='{1}'  group by changedutydepartcode
                            ) b on a.encode = substr(b.changedutydepartcode,1,length(a.encode))  
                            union
                            select  nvl(b.total,0) total ,a.fullname departmentname,a.encode from 
                            ( 
                             select encode ,fullname ,departmentid,organizeid,deptcode from base_department t where organizeid = '{0}'   and nature ='厂级' 
                            )  a 
                            left join   
                            (
                              select changedutydepartcode ,count(1) total from v_basehiddeninfo where rankname ='一般隐患' and workstream !='隐患整改' and  to_char(createdate,'yyyy') ='{1}'  group by changedutydepartcode
                            ) b on a.encode = b.changedutydepartcode  
                        ) a group by  departmentname,encode
                       ) b  on a.encode = b.encode  where a.total >0
                    )  a
                    where a.zgl<100 ", user.OrganizeId, DateTime.Now.Year);

                DesktopPageIndex YJZB003 = new DesktopPageIndex();
                YJZB003.code = "YJZB003";
                YJZB003.name = "一般隐患整改率低于100%的部门";
                YJZB003.modulecode = "CJLDJSC-001";
                YJZB003.modulename = "预警指标";
                YJZB003.count = BaseRepository().FindObject(sql).ToInt().ToString();
                YJZB003.isdecimal = false;
                list.Add(YJZB003);
            }
            if (lstItems.Contains("YJZB004"))
            {
                //重大隐患整改率低于80%的部门
                sql = string.Format(@"select count(1) from (
                    select nvl((case when total >0 then round( yzgtotal /total*100,2) else null end),0) zgl ，a.departmentname,a.encode  from (
                        select  sum(total) total,  departmentname,encode  from (
                            select  nvl(b.total,0) total,a.fullname departmentname ,a.encode from 
                            ( 
                             select encode ,fullname ,departmentid,organizeid,deptcode from base_department t where organizeid = '{0}'  and nature ='部门' 
                            )  a 
                            left join   
                            (
                              select changedutydepartcode ,count(1) total from v_basehiddeninfo where rankname ='重大隐患' and  to_char(createdate,'yyyy') ='{1}'  group by changedutydepartcode
                            ) b on a.encode = substr(b.changedutydepartcode,1,length(a.encode))  
                            union
                            select  nvl(b.total,0) total ,a.fullname departmentname,a.encode from 
                            ( 
                             select encode ,fullname ,departmentid,organizeid,deptcode from base_department t where organizeid = '{0}'   and nature ='厂级' 
                            )  a 
                            left join   
                            (
                              select changedutydepartcode ,count(1) total from v_basehiddeninfo where rankname ='重大隐患' and  to_char(createdate,'yyyy') ='{1}'  group by changedutydepartcode
                            ) b on a.encode = b.changedutydepartcode  
                        ) a group by  departmentname,encode
                    )  a 
                    left join 
                    (
                      select  sum(total) yzgtotal,  departmentname,encode  from (
                            select  nvl(b.total,0) total,a.fullname departmentname ,a.encode from 
                            ( 
                             select encode ,fullname ,departmentid,organizeid,deptcode from base_department t where organizeid = '{0}'  and nature ='部门' 
                            )  a 
                            left join   
                            (
                              select changedutydepartcode ,count(1) total from v_basehiddeninfo where rankname ='重大隐患' and workstream !='隐患整改' and  to_char(createdate,'yyyy') ='{1}'  group by changedutydepartcode
                            ) b on a.encode = substr(b.changedutydepartcode,1,length(a.encode))  
                            union
                            select  nvl(b.total,0) total ,a.fullname departmentname,a.encode from 
                            ( 
                             select encode ,fullname ,departmentid,organizeid,deptcode from base_department t where organizeid = '{0}'   and nature ='厂级' 
                            )  a 
                            left join   
                            (
                              select changedutydepartcode ,count(1) total from v_basehiddeninfo where rankname ='重大隐患' and workstream !='隐患整改' and  to_char(createdate,'yyyy') ='{1}'  group by changedutydepartcode
                            ) b on a.encode = b.changedutydepartcode  
                        ) a group by  departmentname,encode
                    ) b  on a.encode = b.encode   where a.total >0
                    )  a
                    where a.zgl<80 ", user.OrganizeId, DateTime.Now.Year);

                DesktopPageIndex YJZB004 = new DesktopPageIndex();
                YJZB004.code = "YJZB004";
                YJZB004.name = "重大隐患整改率低于80%的部门";
                YJZB004.modulecode = "CJLDJSC-001";
                YJZB004.modulename = "预警指标";
                YJZB004.count = BaseRepository().FindObject(sql).ToInt().ToString();
                YJZB004.isdecimal = false;
                list.Add(YJZB004);
            }

            #endregion

            #region 安全指标预警指标
            if (lstItems.Contains("YQZDGZ"))
            {
                sql = string.Format(@"select count(1) from BIS_SafetyWorkSupervise t
left join base_department i on t.dutydeptcode = i.encode
 where flowstate='1' and to_date('{0}','yyyy-mm-dd hh24:mi:ss')>finishdate+1 and  i.deptcode  like '{1}%'", DateTime.Now, user.OrganizeCode);
                DesktopPageIndex YQZDGZ = new DesktopPageIndex();
                YQZDGZ.code = "YQZDGZ";
                YQZDGZ.name = "逾期安全重点工作";
                YQZDGZ.modulecode = "CJLDJSC-001";
                YQZDGZ.modulename = "预警指标";
                YQZDGZ.count = BaseRepository().FindObject(sql).ToInt().ToString();
                YQZDGZ.isdecimal = false;
                list.Add(YQZDGZ);
            }

            if (lstItems.Contains("JJDQZDGZ"))
            {
                sql = string.Format(@"select count(1) from BIS_SafetyWorkSupervise t
left join base_department i on t.dutydeptcode = i.encode
 where flowstate='1' and (finishdate - 2 <= sysdate  and sysdate <= finishdate + 1)
    and  i.deptcode  like '{0}%'", user.OrganizeCode);
                DesktopPageIndex JJDQZDGZ = new DesktopPageIndex();
                JJDQZDGZ.code = "JJDQZDGZ";
                JJDQZDGZ.name = "即将到期安全重点工作";
                JJDQZDGZ.modulecode = "CJLDJSC-001";
                JJDQZDGZ.modulename = "预警指标";
                JJDQZDGZ.count = BaseRepository().FindObject(sql).ToInt().ToString();
                JJDQZDGZ.isdecimal = false;
                list.Add(JJDQZDGZ);
            }

            #endregion

            #endregion

            #region 安全指标

            #region 安全风险管理 CJLDJSC-002

            if (lstItems.Contains("AQZB001"))
            {
                //重大风险数量  
                sql = string.Format(@"select count(1) from bis_riskassess t where t.status=1 and t.deletemark=0 and t.enabledmark=0 and t.deptcode like '{0}%' and  t.grade='重大风险'", user.OrganizeCode);

                DesktopPageIndex AQZB001 = new DesktopPageIndex();
                AQZB001.code = "AQZB001";
                AQZB001.name = "重大风险数量";
                AQZB001.modulecode = "CJLDJSC-002";
                AQZB001.modulename = "安全指标-安全风险管理";
                AQZB001.count = BaseRepository().FindObject(sql).ToInt().ToString();
                AQZB001.isdecimal = false;
                list.Add(AQZB001);
            }
            if (lstItems.Contains("AQZB002"))
            {
                //较大风险数量
                sql = string.Format(@"select count(1) from bis_riskassess t where t.status=1 and t.deletemark=0 and t.enabledmark=0 and t.deptcode like '{0}%' and  t.grade='较大风险'", user.OrganizeCode);

                DesktopPageIndex AQZB002 = new DesktopPageIndex();
                AQZB002.code = "AQZB002";
                AQZB002.name = "较大风险数量";
                AQZB002.modulecode = "CJLDJSC-002";
                AQZB002.modulename = "安全指标-安全风险管理";
                AQZB002.count = BaseRepository().FindObject(sql).ToInt().ToString();
                AQZB002.isdecimal = false;
                list.Add(AQZB002);
            }
            if (lstItems.Contains("AQZB003"))
            {
                //高风险作业(待定)
                sql = string.Format(@"select  sum(nvl(t1.nums,0)) nums from(
                                           select itemvalue,itemname from   base_dataitemdetail where  itemid =(select itemid from base_dataitem where itemcode='StatisticsType')
                                        ) t left join (
                                         select count(id) nums,worktype from (
                                              select to_char(a.id) as id,to_char(a.worktype) as worktype,b.itemname as risktypename,to_char(a.risktype) as risktype,to_char(workdeptname) as workdeptname,
                                              to_char(workareaname) as workareaname,to_char(a.workdeptcode) as workdeptcode  from  bis_highriskcommonapply a 
                                              left join base_dataitemdetail b on a.risktype=b.itemvalue and itemid =(select itemid from base_dataitem where itemcode='CommonRiskType')
                                              where applystate='5' and  realityworkstarttime is not null and realityworkendtime is null

                                              union all
                                              select to_char(a.id) as id,'-1' as worktype,'' as risktypename,''as risktype,to_char(a.setupcompanyname) as workdeptname,to_char(a.workarea) as workareaname,
                                              to_char(a.setupcompanycode) as workdeptcode  from bis_scaffold a left join 
                                              (select id,scaffoldid,auditusername,auditstate,auditdate,row_number() over(partition by scaffoldid order by auditdate desc) as num from bis_scaffoldauditrecord) b 
                                              on a.id = b.scaffoldid and b.num = 1 where a.id in (select id from bis_scaffold where scaffoldtype = 0 and auditstate = 3 
                                              and id not in(select id from bis_scaffold where id in(select setupinfoid from bis_scaffold where scaffoldtype = 1 and auditstate = 3))) 
                                              and  a.actsetupstartdate is not null and a.actsetupenddate is null

                                              union all
                                              select to_char(a.id) as id,'-2' as worktype,'' as risktypename,''as risktype,to_char(a.setupcompanyname) as workdeptname,to_char(a.workarea) as workareaname,to_char(a.setupcompanycode) as workdeptcode  from bis_scaffold a left join bis_scaffold c 
                                              on nvl(a.setupinfoid,'-') = c.id left join
                                              (select id,scaffoldid,auditusername,auditstate,auditdate,row_number() over(partition by scaffoldid order by auditdate desc) as num from bis_scaffoldauditrecord) d 
                                              on c.id = d.scaffoldid and d.num = 1 where a.auditstate = 3 and a.scaffoldtype = 2 and a.realitydismentlestartdate is not null and a.realitydismentleenddate is null
                                              union all
                                              select to_char(a.id) as id,'-3' as worktype,'' as risktypename,''as risktype,to_char(workunit) as workdeptname,to_char(workarea) as workareaname,to_char(a.workunitcode) as workdeptcode from bis_Safetychange a where iscommit=1 and isapplyover=1 
                                              and a.id not in(select id from bis_safetychange where ((isaccpcommit=0 and isaccepover=0  and  RealityChangeTime is null) or (isaccpcommit=1 and isaccepover=1)))
                                           )  a where 1=1  and  a.workdeptcode like '{0}%' group by worktype
                                         ) t1 on t.itemvalue =t1.worktype  ", user.OrganizeCode);
                DesktopPageIndex AQZB003 = new DesktopPageIndex();
                AQZB003.code = "AQZB003";
                AQZB003.name = "高风险作业数量";
                AQZB003.modulecode = "CJLDJSC-002";
                AQZB003.modulename = "安全指标-安全风险管理";
                AQZB003.count = BaseRepository().FindObject(sql).ToInt().ToString();
                AQZB003.isdecimal = false;
                list.Add(AQZB003);
            }

            if (lstItems.Contains("AQZB029"))
            {
                //危险作业审批单数据
                sql = string.Format(@"select count(1) as nums from BIS_JOBAPPROVALFORM where JOBSTATE=2 and CREATEUSERORGCODE ='{0}' ", user.OrganizeCode);
                DesktopPageIndex AQZB029 = new DesktopPageIndex();
                AQZB029.code = "AQZB029";
                AQZB029.name = "高危作业总数";
                AQZB029.modulecode = "CJLDJSC-002";
                AQZB029.modulename = "安全指标-安全风险管理";
                AQZB029.count = BaseRepository().FindObject(sql).ToInt().ToString();
                AQZB029.isdecimal = false;
                list.Add(AQZB029);
            }

            #endregion

            #region 隐患排查治理 CJLDJSC-003
            if (lstItems.Contains("AQZB004"))
            {
                //安全检查次数
                sql = string.Format(@" select count(1) from bis_saftycheckdatarecord t where  
                            datatype in(0,2) and to_char(t.checkbegintime,'yyyy')='{1}'  and (belongdept like '{0}%' or ',' || CheckDeptCode like '%,{0}%' or ',' || CHECKDEPTID like '%,{0}%')
                             and to_char(t.createdate,'yyyy')='{1}' and  createuserorgcode ='{0}'", user.OrganizeCode, DateTime.Now.Year);

                DesktopPageIndex AQZB004 = new DesktopPageIndex();
                AQZB004.code = "AQZB004";
                AQZB004.name = "安全检查次数";
                AQZB004.modulecode = "CJLDJSC-003";
                AQZB004.modulename = "安全指标-隐患排查治理";
                AQZB004.count = BaseRepository().FindObject(sql).ToInt().ToString();
                AQZB004.isdecimal = false;
                list.Add(AQZB004);
            }
            if (lstItems.Contains("AQZB005"))
            {
                //一般隐患数量
                sql = string.Format(@" select count(1) from v_basehiddeninfo t where t.hiddepart='{0}' and  changedutydepartcode in (select encode from base_department  where organizeid='{0}')  and t.rankname ='一般隐患' and to_char(createdate,'yyyy')='{1}'", user.OrganizeId, DateTime.Now.Year);

                DesktopPageIndex AQZB005 = new DesktopPageIndex();
                AQZB005.code = "AQZB005";
                AQZB005.name = "一般隐患数量";
                AQZB005.modulecode = "CJLDJSC-003";
                AQZB005.modulename = "安全指标-隐患排查治理";
                AQZB005.count = BaseRepository().FindObject(sql).ToInt().ToString();
                AQZB005.isdecimal = false;
                list.Add(AQZB005);
            }

            if (lstItems.Contains("AQZB006"))
            {
                //重大隐患数量
                sql = string.Format(@" select count(1) from v_basehiddeninfo t where t.hiddepart='{0}' and  changedutydepartcode in (select encode from base_department  where organizeid='{0}')  and t.rankname ='重大隐患' and to_char(createdate,'yyyy')='{1}'", user.OrganizeId, DateTime.Now.Year);

                DesktopPageIndex AQZB006 = new DesktopPageIndex();
                AQZB006.code = "AQZB006";
                AQZB006.name = "重大隐患数量";
                AQZB006.modulecode = "CJLDJSC-003";
                AQZB006.modulename = "安全指标-隐患排查治理";
                AQZB006.count = BaseRepository().FindObject(sql).ToInt().ToString();
                AQZB006.isdecimal = false;
                list.Add(AQZB006);
            }
            if (lstItems.Contains("AQZB007"))
            {
                //未整改隐患数量
                sql = string.Format(@" select count(1) from v_basehiddeninfo t where t.hiddepart='{0}' and  t.workstream ='隐患整改' ", user.OrganizeId);

                DesktopPageIndex AQZB007 = new DesktopPageIndex();
                AQZB007.code = "AQZB007";
                AQZB007.name = "未整改隐患数量";
                AQZB007.modulecode = "CJLDJSC-003";
                AQZB007.modulename = "安全指标-隐患排查治理";
                AQZB007.count = BaseRepository().FindObject(sql).ToInt().ToString();
                AQZB007.isdecimal = false;
                list.Add(AQZB007);
            }

            if (lstItems.Contains("AQZB008"))
            {
                //一般隐患整改率
                sql = string.Format(@" select  (case when sum(a.numbers) =0 then 0 else round(sum(b.numbers)/sum(a.numbers) *100,2) end ) total from  (
                                           select count(1) numbers ,hiddepart from v_basehiddeninfo t where t.hiddepart='{0}' and  changedutydepartcode in (select encode from base_department  where organizeid='{0}')  and rankname ='一般隐患' and to_char(changedeadine,'yyyy') ='{1}'   group by hiddepart
                                        )  a 
                                        left join ( 
                                        select count(1) numbers ,hiddepart from v_basehiddeninfo t where t.hiddepart='{0}' and  changedutydepartcode in (select encode from base_department  where organizeid='{0}')  and t.workstream !='隐患整改' and to_char(changedeadine,'yyyy') ='{1}'  and rankname ='一般隐患' group by hiddepart
                                        ) b on a.hiddepart = b.hiddepart ", user.OrganizeId, DateTime.Now.Year);

                DesktopPageIndex AQZB008 = new DesktopPageIndex();
                AQZB008.code = "AQZB008";
                AQZB008.name = "一般隐患整改率";
                AQZB008.modulecode = "CJLDJSC-003";
                AQZB008.modulename = "安全指标-隐患排查治理";
                AQZB008.count = BaseRepository().FindObject(sql).ToInt().ToString();
                AQZB008.isdecimal = true;
                list.Add(AQZB008);

            }
            if (lstItems.Contains("AQZB009"))
            {

                //重大隐患整改率
                sql = string.Format(@" select  (case when sum(a.numbers) =0 then 0 else round(sum(b.numbers)/sum(a.numbers) *100,2) end ) total from  (
                                           select count(1) numbers ,hiddepart from v_basehiddeninfo t where t.hiddepart='{0}' and  changedutydepartcode in (select encode from base_department  where organizeid='{0}')  and rankname ='重大隐患'  and to_char(changedeadine,'yyyy') ='{1}'  group by hiddepart
                                        )  a 
                                        left join ( 
                                        select count(1) numbers ,hiddepart from v_basehiddeninfo t where t.hiddepart='{0}' and  changedutydepartcode in (select encode from base_department  where organizeid='{0}')  and t.workstream !='隐患整改'  and to_char(changedeadine,'yyyy') ='{1}' and rankname ='重大隐患' group by hiddepart
                                        ) b on a.hiddepart = b.hiddepart ", user.OrganizeId, DateTime.Now.Year);

                DesktopPageIndex AQZB009 = new DesktopPageIndex();
                AQZB009.code = "AQZB009";
                AQZB009.name = "重大隐患整改率";
                AQZB009.modulecode = "CJLDJSC-003";
                AQZB009.modulename = "安全指标-隐患排查治理";
                AQZB009.count = BaseRepository().FindObject(sql).ToDecimal().ToString();
                AQZB009.isdecimal = true;
                list.Add(AQZB009);
            }
            #endregion

            #region 反违章管理 CJLDJSC-004
            if (lstItems.Contains("AQZB010"))
            {
                //违章总数
                sql = string.Format(@" select count(1) from v_lllegalbaseinfo where  reformdeptcode like '{0}%' and to_char(createdate,'yyyy') ='{1}' ", user.OrganizeCode, DateTime.Now.Year);

                DesktopPageIndex AQZB010 = new DesktopPageIndex();
                AQZB010.code = "AQZB010";
                AQZB010.name = "违章总数";
                AQZB010.modulecode = "CJLDJSC-004";
                AQZB010.modulename = "安全指标-反违章管理";
                AQZB010.count = BaseRepository().FindObject(sql).ToInt().ToString();
                AQZB010.isdecimal = false;
                list.Add(AQZB010);
            }
            if (lstItems.Contains("AQZB011"))
            {
                //未整改违章总数
                sql = string.Format(@" select count(1) from v_lllegalbaseinfo where  reformdeptcode like '{0}%' and  flowstate = '违章整改' ", user.OrganizeCode);

                DesktopPageIndex AQZB011 = new DesktopPageIndex();
                AQZB011.code = "AQZB011";
                AQZB011.name = "未整改违章总数";
                AQZB011.modulecode = "CJLDJSC-004";
                AQZB011.modulename = "安全指标-反违章管理";
                AQZB011.count = BaseRepository().FindObject(sql).ToInt().ToString();
                AQZB011.isdecimal = false;
                list.Add(AQZB011);
            }
            if (lstItems.Contains("AQZB012"))
            {
                //违章整改率
                sql = string.Format(@" select (case when  sum(a.numbers) =0 then 0 else  round(sum(b.numbers)/sum(a.numbers)*100,2) end ) total from (
                                           select count(1) numbers ,reformdeptcode from v_lllegalbaseinfo t where t.reformdeptcode like '{0}%'  and to_char(reformdeadline,'yyyy') ='{1}'   group by reformdeptcode
                                        )  a 
                                        left join
                                        ( 
                                          select count(1) numbers ,reformdeptcode from v_lllegalbaseinfo t where t.reformdeptcode like '{0}%'  and to_char(reformdeadline,'yyyy') ='{1}'  and t.flowstate !='违章整改'  group by reformdeptcode
                                        ) b on a.reformdeptcode = b.reformdeptcode  ", user.OrganizeCode, DateTime.Now.Year);

                DesktopPageIndex AQZB012 = new DesktopPageIndex();
                AQZB012.code = "AQZB012";
                AQZB012.name = "违章整改率";
                AQZB012.modulecode = "CJLDJSC-004";
                AQZB012.modulename = "安全指标-反违章管理";
                AQZB012.count = BaseRepository().FindObject(sql).ToDecimal().ToString();
                AQZB012.isdecimal = true;
                list.Add(AQZB012);
            }
            #endregion

            #region 外包管理 CJLDJSC-005
            if (lstItems.Contains("AQZB013"))
            {
                //外包工程总数
                sql = string.Format(@" select count(1) from epg_outsouringengineer a where a.createuserorgcode='{0}' and a.isdeptadd=1 ", user.OrganizeCode);

                DesktopPageIndex AQZB013 = new DesktopPageIndex();
                AQZB013.code = "AQZB013";
                AQZB013.name = "外包工程总数";
                AQZB013.modulecode = "CJLDJSC-005";
                AQZB013.modulename = "安全指标-外包管理";
                AQZB013.count = BaseRepository().FindObject(sql).ToInt().ToString();
                AQZB013.isdecimal = false;
                list.Add(AQZB013);
            }

            if (lstItems.Contains("AQZB014"))
            {
                //在厂外包单位数
                sql = string.Format(@" select count(1) from epg_outsourcingproject a left join base_department b on a.outprojectid=b.departmentid where a.outorin=0 and a.blackliststate='0' and b.organizeid='{0}' ", user.OrganizeId);

                DesktopPageIndex AQZB014 = new DesktopPageIndex();
                AQZB014.code = "AQZB014";
                AQZB014.name = "在厂外包单位数";
                AQZB014.modulecode = "CJLDJSC-005";
                AQZB014.modulename = "安全指标-外包管理";
                AQZB014.count = BaseRepository().FindObject(sql).ToInt().ToString();
                AQZB014.isdecimal = false;
                list.Add(AQZB014);
            }
            if (lstItems.Contains("AQZB015"))
            {
                //外包人员违章数
                //select count(1) from v_lllegalbaseinfo t left join base_user u on t.LLLEGALPERSONID=userid where u.isepiboly='1' and organizecode='{0}' and to_char(lllegaltime,'yyyy')='{1}'
                sql = string.Format(@" select count(1)  from v_lllegalbaseinfo t 
                                      left join base_user u on t.lllegalpersonid =u.userid
                                      left join base_department c on t.belongdepartid = c.departmentid where u.isepiboly='1'  and c.deptcode like '{0}%'", user.NewDeptCode);

                DesktopPageIndex AQZB015 = new DesktopPageIndex();
                AQZB015.code = "AQZB015";
                AQZB015.name = "外包人员违章数";
                AQZB015.modulecode = "CJLDJSC-005";
                AQZB015.modulename = "安全指标-外包管理";
                AQZB015.count = BaseRepository().FindObject(sql).ToInt().ToString();
                AQZB015.isdecimal = false;
                list.Add(AQZB015);
            }
            if (lstItems.Contains("AQZB016"))
            {
                //本月新进外包人员
                sql = string.Format(@"select count(1) from base_user u where u.ispresence='1' and isepiboly='1' and u.organizecode='{0}' and to_char(entertime,'yyyy-mm')='{1}'", user.OrganizeCode, DateTime.Now.ToString("yyyy-MM"));

                DesktopPageIndex AQZB016 = new DesktopPageIndex();
                AQZB016.code = "AQZB016";
                AQZB016.name = "本月新进外包人员";
                AQZB016.modulecode = "CJLDJSC-005";
                AQZB016.modulename = "安全指标-外包管理";
                AQZB016.count = BaseRepository().FindObject(sql).ToInt().ToString();
                AQZB016.isdecimal = false;
                list.Add(AQZB016);
            }
            if (lstItems.Contains("AQZB017"))
            {
                //在建外包工程数
                sql = string.Format(@"select count(1) from epg_outsouringengineer a where a.createuserorgcode='{0}' and a.engineerstate='002' and a.isdeptadd=1 ", user.OrganizeCode);

                DesktopPageIndex AQZB017 = new DesktopPageIndex();
                AQZB017.code = "AQZB017";
                AQZB017.name = "在建外包工程数";
                AQZB017.modulecode = "CJLDJSC-005";
                AQZB017.modulename = "安全指标-外包管理";
                AQZB017.count = BaseRepository().FindObject(sql).ToInt().ToString();
                AQZB017.isdecimal = false;
                list.Add(AQZB017);
            }
            if (lstItems.Contains("AQZB018"))
            {
                //外包工程在厂人数
                sql = string.Format(@"select count(1) from base_user u where u.ispresence='1' and isepiboly='1' and u.organizecode='{0}'", user.OrganizeCode);

                DesktopPageIndex AQZB018 = new DesktopPageIndex();
                AQZB018.code = "AQZB018";
                AQZB018.name = "外包工程在厂人数";
                AQZB018.modulecode = "CJLDJSC-005";
                AQZB018.modulename = "安全指标-外包管理";
                AQZB018.count = BaseRepository().FindObject(sql).ToInt().ToString();
                AQZB018.isdecimal = false;
                list.Add(AQZB018);
            }

            #endregion

            #region 危险源管理 CJLDJSC-006
            if (lstItems.Contains("AQZB019"))
            {
                //重大危险源数量
                sql = string.Format(@"select count(1) from hsd_hazardsource d where ( d.deptcode like '{0}%'  or  d.createuserid='{1}' ) and d.IsDanger>0", user.OrganizeCode, user.UserId);

                DesktopPageIndex AQZB019 = new DesktopPageIndex();
                AQZB019.code = "AQZB019";
                AQZB019.name = "重大危险源数量";
                AQZB019.modulecode = "CJLDJSC-006";
                AQZB019.modulename = "安全指标-危险源管理";
                AQZB019.count = BaseRepository().FindObject(sql).ToInt().ToString();
                AQZB019.isdecimal = false;
                list.Add(AQZB019);
            }


            #endregion

            #region 特种设备及人员管理 CJLDJSC-007

            if (lstItems.Contains("AQZB020"))
            {
                //全厂特种设备数量
                sql = string.Format(@"select count(1) from bis_specialequipment t where t.controldeptcode like '{0}%'", user.OrganizeCode);

                DesktopPageIndex AQZB020 = new DesktopPageIndex();
                AQZB020.code = "AQZB020";
                AQZB020.name = "全厂特种设备数量";
                AQZB020.modulecode = "CJLDJSC-007";
                AQZB020.modulename = "安全指标-特种设备及人员管理";
                AQZB020.count = BaseRepository().FindObject(sql).ToInt().ToString();
                AQZB020.isdecimal = false;
                list.Add(AQZB020);
            }
            if (lstItems.Contains("AQZB021"))
            {
                //特种作业人员
                sql = string.Format(@"select count(1) from base_user u where  u.organizeid='{0}' and u.ispresence=1 and u.isspecial='是'", user.OrganizeId);

                DesktopPageIndex AQZB021 = new DesktopPageIndex();
                AQZB021.code = "AQZB021";
                AQZB021.name = "特种作业人员";
                AQZB021.modulecode = "CJLDJSC-007";
                AQZB021.modulename = "安全指标-特种设备及人员管理";
                AQZB021.count = BaseRepository().FindObject(sql).ToInt().ToString();
                AQZB021.isdecimal = false;
                list.Add(AQZB021);
            }

            if (lstItems.Contains("AQZB022"))
            {
                //特种设备作业人员
                sql = string.Format(@"select count(1) from base_user u where  u.organizeid='{0}' and u.ispresence=1 and u.isspecialequ='是'", user.OrganizeId);

                DesktopPageIndex AQZB022 = new DesktopPageIndex();
                AQZB022.code = "AQZB022";
                AQZB022.name = "特种设备作业人员";
                AQZB022.modulecode = "CJLDJSC-007";
                AQZB022.modulename = "安全指标-特种设备及人员管理";
                AQZB022.count = BaseRepository().FindObject(sql).ToInt().ToString();
                AQZB022.isdecimal = false;
                list.Add(AQZB022);
            }

            #endregion

            #region 教育培训 CJLDJSC-008
            #endregion

            #region 事故事件 CJLDJSC-009
            if (lstItems.Contains("AQZB026"))
            {
                //未遂事件起数
                sql = string.Format(@"select count(1) from  v_aem_wssjbg_deal_order v where v.createuserorgcode='{0}' and issubmit_deal =1 and to_char(v.createdate,'yyyy')='{1}'", user.OrganizeCode, DateTime.Now.Year);

                DesktopPageIndex AQZB026 = new DesktopPageIndex();
                AQZB026.code = "AQZB026";
                AQZB026.name = "未遂事件起数";
                AQZB026.modulecode = "CJLDJSC-009";
                AQZB026.modulename = "安全指标-事故事件";
                AQZB026.count = BaseRepository().FindObject(sql).ToInt().ToString();
                AQZB026.isdecimal = false;
                list.Add(AQZB026);
            }



            if (lstItems.Contains("AQZB027"))
            {
                sql = string.Format(@"select count(1) from aem_bulletin_deal a where issubmit_deal>0 and a.createuserorgcode='{0}' and to_char(createdate,'yyyy')='{1}'", user.OrganizeCode, DateTime.Now.Year);

                DesktopPageIndex AQZB027 = new DesktopPageIndex();
                AQZB027.code = "AQZB027";
                AQZB027.name = "事故事件起数";
                AQZB027.modulecode = "CJLDJSC-009";
                AQZB027.modulename = "安全指标-事故事件";
                AQZB027.count = BaseRepository().FindObject(sql).ToInt().ToString();
                AQZB027.isdecimal = false;
                list.Add(AQZB027);
            }
            if (lstItems.Contains("AQZB028"))
            {
                //事故事件重伤人数
                sql = string.Format(@"select sum(zsnum_deal) from aem_bulletin_deal a where a.issubmit_deal>0 and  a.createuserorgcode='{0}'  and to_char(createdate,'yyyy')='{1}'", user.OrganizeCode, DateTime.Now.Year);

                DesktopPageIndex AQZB028 = new DesktopPageIndex();
                AQZB028.code = "AQZB028";
                AQZB028.name = "事故事件重伤人数";
                AQZB028.modulecode = "CJLDJSC-009";
                AQZB028.modulename = "安全指标-事故事件";
                AQZB028.count = BaseRepository().FindObject(sql).ToInt().ToString();
                AQZB028.isdecimal = false;
                list.Add(AQZB028);
            }

            #endregion

            #endregion

            return list;
        }
        #endregion

        #region 获取整改率低于多少的电厂数据
        /// <summary>
        /// 获取整改率低于多少的电厂数据
        /// </summary>
        /// <param name="user"></param>
        /// <param name="rankname"></param>
        /// <returns></returns>
        public DataTable GetRectificationRateUnderHowMany(Operator user, string rankname, decimal num)
        {
            string sql = string.Empty;

            sql = string.Format(@"select a.* from (
                select nvl((case when total >0 then round( yzgtotal /total*100,2) else null end),0) zgl ，a.departmentname,a.encode  from (
                          select  sum(total) total,  departmentname,encode  from (
                              select  nvl(b.total,0) total,a.fullname departmentname ,a.encode from 
                              ( 
                               select encode ,fullname ,departmentid,organizeid,deptcode from base_department t where organizeid = '{0}'  and nature ='部门' 
                              )  a 
                              left join   
                              (
                                select changedutydepartcode ,count(1) total from v_basehiddeninfo where rankname ='{1}' and  to_char(createdate,'yyyy') ='{2}'  group by changedutydepartcode
                              ) b on a.encode = substr(b.changedutydepartcode,1,length(a.encode))  
                              union
                              select  nvl(b.total,0) total ,a.fullname departmentname,a.encode from 
                              ( 
                               select encode ,fullname ,departmentid,organizeid,deptcode from base_department t where organizeid = '{0}'   and nature ='厂级' 
                              )  a 
                              left join   
                              (
                                select changedutydepartcode ,count(1) total from v_basehiddeninfo where rankname ='{1}' and  to_char(createdate,'yyyy') ='{2}'  group by changedutydepartcode
                              ) b on a.encode = b.changedutydepartcode  
                          ) a group by  departmentname,encode
                      )  a 
                      left join 
                      (
                        select  sum(total) yzgtotal,  departmentname,encode  from (
                              select  nvl(b.total,0) total,a.fullname departmentname ,a.encode from 
                              ( 
                               select encode ,fullname ,departmentid,organizeid,deptcode from base_department t where organizeid = '{0}'  and nature ='部门' 
                              )  a 
                              left join   
                              (
                                select changedutydepartcode ,count(1) total from v_basehiddeninfo where rankname ='{1}' and workstream !='隐患整改' and  to_char(createdate,'yyyy') ='{2}'  group by changedutydepartcode
                              ) b on a.encode = substr(b.changedutydepartcode,1,length(a.encode))  
                              union
                              select  nvl(b.total,0) total ,a.fullname departmentname,a.encode from 
                              ( 
                               select encode ,fullname ,departmentid,organizeid,deptcode from base_department t where organizeid = '{0}'   and nature ='厂级' 
                              )  a 
                              left join   
                              (
                                select changedutydepartcode ,count(1) total from v_basehiddeninfo where rankname ='{1}' and workstream !='隐患整改' and  to_char(createdate,'yyyy') ='{2}'  group by changedutydepartcode
                              ) b on a.encode = b.changedutydepartcode  
                          ) a group by  departmentname,encode
                      ) b  on a.encode = b.encode   where a.total >0
                    )  a where a.zgl<{3}  order by zgl desc,encode asc", user.OrganizeId, rankname, DateTime.Now.Year, num);

            return BaseRepository().FindTable(sql);
        }
        #endregion

        #endregion

        #region 左侧屏

        #region  未闭环隐患统计
        /// <summary>
        /// 未闭环隐患统计
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public DataTable GetNoCloseLoopHidStatistics(Operator user, int mode)
        {
            string sql = string.Empty;

            string code = string.Empty;

            if (user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户"))
            {
                code = user.OrganizeCode;
            }
            else
            {
                code = user.DeptCode;
            }
            //未闭环隐患
            switch (mode)
            {
                //按责任单位统计
                case 1:
                    #region 公司级用户、厂级部门用户
                    if (user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户"))
                    {
                        sql = string.Format(@"with ybTable as (
                         select  count(b.id) ybwbh, a.encode,a.fullname,a.organizeid from (
                                select departmentid,encode ,fullname ,deptcode ,organizeid from base_department  where nature = '部门' and organizeid = '{0}' 
                              ) a
                              left join 
                              (
                                  select b.changedutydepartcode ,a.id from  bis_htbaseinfo a 
                                  left join v_htchangeinfo b on a.hidcode = b.hidcode
                                  left join base_dataitemdetail c on a.hidrank = c.itemdetailid   where  substr(c.itemname,length(c.itemname)-3)='一般隐患'  and  a.workstream !='整改结束' 
                                  and  a.workstream !='隐患登记' and a.workstream !='隐患评估' and a.workstream !='隐患完善' and a.workstream !='制定整改计划'   and  changedutydepartcode  like  '{1}%'
                              ) b on a.encode = substr(b.changedutydepartcode,1,length(a.encode))  
                              group by a.encode,a.fullname,a.organizeid
                              union
                              select  count(b.id) ybwbh, a.encode,a.fullname,a.organizeid from (
                                select departmentid,encode ,fullname ,deptcode ,organizeid from base_department  where nature = '厂级' and organizeid = '{0}'  
                              ) a
                              left join 
                              (
                                  select b.changedutydepartcode ,a.id from  bis_htbaseinfo a 
                                  left join v_htchangeinfo b on a.hidcode = b.hidcode
                                  left join base_dataitemdetail c on a.hidrank = c.itemdetailid   where  substr(c.itemname,length(c.itemname)-3)='一般隐患'  and  a.workstream !='整改结束' 
                                  and  a.workstream !='隐患登记' and a.workstream !='隐患评估' and a.workstream !='隐患完善' and a.workstream !='制定整改计划'   and  changedutydepartcode  like  '{1}%'
                              ) b on a.encode = b.changedutydepartcode
                              group by a.encode,a.fullname,a.organizeid
                ),
                zdTable as (
                    select  count(b.id) zdwbh, a.encode,a.fullname,a.organizeid from (
                        select departmentid,encode ,fullname ,deptcode ,organizeid from base_department  where nature ='部门' and organizeid = '{0}' 
                      ) a
                      left join   
                      (
                              select b.changedutydepartcode ,a.id from  bis_htbaseinfo a 
                              left join v_htchangeinfo b on a.hidcode = b.hidcode
                              left join base_dataitemdetail c on a.hidrank = c.itemdetailid   where  substr(c.itemname,length(c.itemname)-3) ='重大隐患'  and  a.workstream !='整改结束' 
                              and  a.workstream !='隐患登记' and a.workstream !='隐患评估' and a.workstream !='隐患完善' and a.workstream !='制定整改计划'   and  changedutydepartcode  like  '{1}%'
                      ) b on a.encode = substr(b.changedutydepartcode,1,length(a.encode)) 
                      group by a.encode,a.fullname,a.organizeid
                       union
                      select  count(b.id) ybwbh, a.encode,a.fullname,a.organizeid from (
                        select departmentid,encode ,fullname ,deptcode ,organizeid from base_department  where nature = '厂级' and organizeid = '{0}'  
                      ) a
                      left join 
                      (
                            select b.changedutydepartcode ,a.id from  bis_htbaseinfo a 
                            left join v_htchangeinfo b on a.hidcode = b.hidcode
                            left join base_dataitemdetail c on a.hidrank = c.itemdetailid   where  substr(c.itemname,length(c.itemname)-3) ='重大隐患'  and  a.workstream !='整改结束' 
                            and  a.workstream !='隐患登记' and a.workstream !='隐患评估' and a.workstream !='隐患完善' and a.workstream !='制定整改计划'   and  changedutydepartcode  like  '{1}%'
                      ) b on a.encode = b.changedutydepartcode
                      group by a.encode,a.fullname,a.organizeid
)
select  (to_char(a.ybwbh) || '('|| to_char(nvl((case when c.ybzs =0 then 0 else round(a.ybwbh / c.ybzs * 100,2) end),0))|| '%)') s1  , 
                                  (to_char(b.zdwbh) || '(' || to_char(nvl((case when d.zdzs =0 then 0 else round(b.zdwbh / d.zdzs * 100,2) end),0)) || '%)') s2, a.encode code ,a.fullname name  from ybTable  a
                              left join zdTable  b on a.encode = b.encode 
                             left join
                            (
                                  select count (a.id) ybzs ,a.hiddepart from bis_htbaseinfo a 
                                  left join v_htchangeinfo b on a.hidcode = b.hidcode
                                  left join base_dataitemdetail c on a.hidrank = c.itemdetailid   where substr(c.itemname,length(c.itemname)-3) ='一般隐患'  and  a.workstream !='整改结束' 
                                  and  a.workstream !='隐患登记' and a.workstream !='隐患评估' and a.workstream !='隐患完善' and a.workstream !='制定整改计划'   and  b.changedutydepartcode  like  '{1}%'  group by a.hiddepart
                            ) c on a.organizeid = c.hiddepart
                             left join
                             (
                                  select count (a.id) zdzs ,a.hiddepart from bis_htbaseinfo a 
                                  left join v_htchangeinfo b on a.hidcode = b.hidcode
                                  left join base_dataitemdetail c on a.hidrank = c.itemdetailid   where  substr(c.itemname,length(c.itemname)-3)='重大隐患'  and  a.workstream !='整改结束' 
                                  and  a.workstream !='隐患登记' and a.workstream !='隐患评估' and a.workstream !='隐患完善' and a.workstream !='制定整改计划'   and  b.changedutydepartcode  like  '{1}%' group by a.hiddepart
                             ) d on a.organizeid = d.hiddepart  order by  b.zdwbh desc ,a.ybwbh desc , a.fullname asc  ", user.OrganizeId, code);
                    }
                    #endregion
                    #region 普通用户
                    else
                    {
                        sql = string.Format(@"with ybTable as (
                                select  count(b.id) ybwbh, a.encode,a.fullname,a.organizeid from (
                                    select departmentid,encode ,fullname ,deptcode ,organizeid from base_department  where  encode like '{0}%' 
                                ) a
                                left join 
                                (
                                    select b.changedutydepartcode ,a.id from bis_htbaseinfo a 
                                    left join v_htchangeinfo b on a.hidcode = b.hidcode
                                    left join base_dataitemdetail c on a.hidrank = c.itemdetailid   where  substr(c.itemname,length(c.itemname)-3)='一般隐患'  and  a.workstream !='整改结束' 
                                    and  a.workstream !='隐患登记' and a.workstream !='隐患评估' and a.workstream !='隐患完善' and a.workstream !='制定整改计划'  and  b.changedutydepartcode  like '{0}%' 
                                ) b on a.encode =b.changedutydepartcode
                                group by a.encode,a.fullname,a.organizeid 
                        ),
                        zdTable as (
                                select  count(b.id) zdwbh, a.encode,a.fullname,a.organizeid from (
                                    select departmentid,encode ,fullname ,deptcode ,organizeid from base_department  where  encode like '{0}%' 
                                ) a
                                left join   
                                (
                                        select b.changedutydepartcode ,a.id from  bis_htbaseinfo a 
                                        left join v_htchangeinfo b on a.hidcode = b.hidcode
                                        left join base_dataitemdetail c on a.hidrank = c.itemdetailid   where  substr(c.itemname,length(c.itemname)-3)='重大隐患'  and  a.workstream !='整改结束' 
                                        and  a.workstream !='隐患登记' and a.workstream !='隐患评估' and a.workstream !='隐患完善' and a.workstream !='制定整改计划'   and  changedutydepartcode  like  '{1}%'
                                ) b on a.encode =b.changedutydepartcode
                                group by a.encode,a.fullname,a.organizeid
                        )
                   select  (to_char(a.ybwbh) || '('|| to_char(nvl((case when c.ybzs =0 then 0 else round(a.ybwbh / c.ybzs * 100,2) end),0))|| '%)') s1  , 
                                    (to_char(b.zdwbh) || '(' || to_char(nvl((case when d.zdzs =0 then 0 else round(b.zdwbh / d.zdzs * 100,2) end),0)) || '%)') s2, a.encode code ,a.fullname name  from ybTable  a
                                left join zdTable b on a.encode = b.encode 
                             left join
                             (
                                          select count (a.id) ybzs ,a.hiddepart from bis_htbaseinfo a 
                                        left join v_htchangeinfo b on a.hidcode = b.hidcode
                                        left join base_dataitemdetail c on a.hidrank = c.itemdetailid   where  substr(c.itemname,length(c.itemname)-3)='一般隐患'  and  a.workstream !='整改结束' 
                                        and  a.workstream !='隐患登记' and a.workstream !='隐患评估' and a.workstream !='隐患完善' and a.workstream !='制定整改计划'  and  b.changedutydepartcode  like  '{0}%'   group by a.hiddepart
                             ) c on a.organizeid = c.hiddepart
                             left join
                             (
                               select count (a.id) zdzs ,a.hiddepart from bis_htbaseinfo a 
                                          left join v_htchangeinfo b on a.hidcode = b.hidcode
                                          left join base_dataitemdetail c on a.hidrank = c.itemdetailid   where  substr(c.itemname,length(c.itemname)-3)='重大隐患'  and  a.workstream !='整改结束' 
                                          and  a.workstream !='隐患登记' and a.workstream !='隐患评估' and a.workstream !='隐患完善' and a.workstream !='制定整改计划'    and  b.changedutydepartcode  like  '{0}%'  group by a.hiddepart
                             ) d on a.organizeid = d.hiddepart  order by  b.zdwbh desc ,a.ybwbh desc , a.fullname asc  ", code);
                    }
                    #endregion
                    break;
                //按区域统计
                case 2:
                    sql = string.Format(@"with ybTable as (
                        select  count(b.id) ybwbh, a.districtcode,a.districtname,a.organizeid from (
                                                  select  districtid, districtname, districtcode ,sortcode ,organizeid from  bis_district  where  organizeid = '{0}'  and parentid ='0' 
                                                ) a
                                                left join 
                                                (
                                                    select a.hidpoint ,a.id from  bis_htbaseinfo a 
                                                    left join v_htchangeinfo b on a.hidcode = b.hidcode
                                                    left join base_dataitemdetail c on a.hidrank = c.itemdetailid   where  substr(c.itemname,length(c.itemname)-3)='一般隐患'  and  a.workstream !='整改结束' 
                                                    and  a.workstream !='隐患登记' and a.workstream !='隐患评估' and a.workstream !='隐患完善' and a.workstream !='制定整改计划' and  b.changedutydepartcode  like  '{1}%'  
                                                 ) b on  a.districtcode = substr(b.hidpoint,1,length(a.districtcode))
                                                 group by a.districtcode,a.districtname,a.organizeid 
                        ),
                        zdTable as (
                         select  count(b.id) zdwbh, a.districtcode,a.districtname,a.organizeid from (
                                                    select  districtid, districtname, districtcode ,sortcode,organizeid from  bis_district  where  organizeid = '{0}'  and parentid ='0'
                                                ) a
                                                left join   
                                                (
                                                    select a.hidpoint ,a.id from bis_htbaseinfo a 
                                                    left join v_htchangeinfo b on a.hidcode = b.hidcode
                                                    left join base_dataitemdetail c on a.hidrank = c.itemdetailid   where  substr(c.itemname,length(c.itemname)-3)='重大隐患'  and  a.workstream !='整改结束' 
                                                    and  a.workstream !='隐患登记' and a.workstream !='隐患评估' and a.workstream !='隐患完善' and a.workstream !='制定整改计划'  and  b.changedutydepartcode  like  '{1}%'    
                                                ) b on  a.districtcode = substr(b.hidpoint,1,length(a.districtcode))
                                                group by a.districtcode,a.districtname,a.organizeid 
                        )
                        select  (to_char(a.ybwbh) || '('|| to_char(nvl((case when c.ybzs =0 then 0 else round(a.ybwbh / c.ybzs * 100,2) end),0))|| '%)') s1  , 
                                                                        (to_char(b.zdwbh) || '(' || to_char(nvl((case when d.zdzs =0 then 0 else round(b.zdwbh/d.zdzs * 100,2) end),0)) || '%)') s2, a.districtcode code ,a.districtname name  from ybTable  a
                                                                 left join zdTable b on a.districtcode = b.districtcode 
                                                                 left join
                                                                 (
                                                                   select count (a.id) ybzs ,a.hiddepart from bis_htbaseinfo a 
                                                                            left join v_htchangeinfo b on a.hidcode = b.hidcode
                                                                            left join base_dataitemdetail c on a.hidrank = c.itemdetailid   where  substr(c.itemname,length(c.itemname)-3) ='一般隐患'  and  a.workstream !='整改结束' 
                                                                            and  a.workstream !='隐患登记' and a.workstream !='隐患评估' and a.workstream !='隐患完善' and a.workstream !='制定整改计划' and  b.changedutydepartcode  like  '{1}%'  group by hiddepart
                                                                 ) c on a.organizeid = c.hiddepart
                                                                 left join
                                                                 (
                                                                   select count (a.id) zdzs ,a.hiddepart from bis_htbaseinfo a 
                                                                            left join v_htchangeinfo b on a.hidcode = b.hidcode
                                                                            left join base_dataitemdetail c on a.hidrank = c.itemdetailid   where  substr(c.itemname,length(c.itemname)-3) ='重大隐患'  and  a.workstream !='整改结束' 
                                                                            and  a.workstream !='隐患登记' and a.workstream !='隐患评估' and a.workstream !='隐患完善' and a.workstream !='制定整改计划'  and  b.changedutydepartcode  like  '{1}%'  group by hiddepart
                                                                 ) d on a.organizeid = d.hiddepart  order by  b.zdwbh desc ,a.ybwbh desc , a.districtname asc ", user.OrganizeId, code);
                    break;
                //按专业统计
                case 3:
                    sql = string.Format(@"with ybTable as (
    select  count(b.id) ybwbh, a.itemdetailid ,a.itemname ,a.sortcode,b.hiddepart from (
                                                      select a.itemdetailid ,a.itemname,a.sortcode from base_dataitemdetail a
                                                      left join base_dataitem b on a.itemid = b.itemid  where b.itemname = '隐患专业分类'   
                                                ) a
                                                left join 
                                                (
                                                    select a.majorclassify ,a.id,a.hiddepart from bis_htbaseinfo a 
                                                    left join v_htchangeinfo b on a.hidcode = b.hidcode
                                                    left join base_dataitemdetail c on a.hidrank = c.itemdetailid   where  substr(c.itemname,length(c.itemname)-3) ='一般隐患'  and  a.workstream !='整改结束' 
                                                    and  a.workstream !='隐患登记' and a.workstream !='隐患评估' and a.workstream !='隐患完善' and a.workstream !='制定整改计划' and  b.changedutydepartcode  like  '{0}%'
                                                 ) b on a.itemdetailid = b.majorclassify
                                                 group by  a.itemdetailid ,a.itemname ,a.sortcode,b.hiddepart
),
zdTable as (
     select  count(b.id) zdwbh, a.itemdetailid ,a.itemname ,a.sortcode from (
                                                          select a.itemdetailid ,a.itemname,a.sortcode from base_dataitemdetail a
                                                          left join base_dataitem b on a.itemid = b.itemid  where b.itemname = '隐患专业分类'   
                                                    ) a
                                                    left join 
                                                    (
                                                        select a.majorclassify ,a.id,a.hiddepart from bis_htbaseinfo a 
                                                        left join v_htchangeinfo b on a.hidcode = b.hidcode
                                                        left join base_dataitemdetail c on a.hidrank = c.itemdetailid   where  substr(c.itemname,length(c.itemname)-3) ='重大隐患'  and  a.workstream !='整改结束' 
                                                        and  a.workstream !='隐患登记' and a.workstream !='隐患评估' and a.workstream !='隐患完善' and a.workstream !='制定整改计划'  and  b.changedutydepartcode  like  '{0}%'
                                                     ) b on a.itemdetailid = b.majorclassify
                                                     group by  a.itemdetailid ,a.itemname ,a.sortcode
)
select  (to_char(a.ybwbh) || '('|| to_char( nvl((case when c.ybzs =0 then 0 else round(a.ybwbh / c.ybzs * 100,2) end),0))|| '%)') s1  , 
                                                (to_char(b.zdwbh) || '(' || to_char(nvl((case when d.zdzs =0 then 0 else round(b.zdwbh/d.zdzs * 100,2) end),0)) || '%)') s2, a.sortcode code ,a.itemname name ,a.itemdetailid id from ybTable a
                                            left join zdTable b on a.itemdetailid = b.itemdetailid 
                                             left join
                                             (
                                               select count (a.id) ybzs ,a.hiddepart from bis_htbaseinfo a 
                                                    left join v_htchangeinfo b on a.hidcode = b.hidcode
                                                    left join base_dataitemdetail c on a.hidrank = c.itemdetailid   where  substr(c.itemname,length(c.itemname)-3)='一般隐患'  and  a.workstream !='整改结束' 
                                                    and  a.workstream !='隐患登记' and a.workstream !='隐患评估' and a.workstream !='隐患完善' and a.workstream !='制定整改计划' and  b.changedutydepartcode  like  '{0}%'  group by hiddepart
                                             ) c on a.hiddepart = c.hiddepart
                                             left join
                                             (
                                               select count (a.id) zdzs ,a.hiddepart from bis_htbaseinfo a 
                                                        left join v_htchangeinfo b on a.hidcode = b.hidcode
                                                        left join base_dataitemdetail c on a.hidrank = c.itemdetailid   where  substr(c.itemname,length(c.itemname)-3) ='重大隐患'  and  a.workstream !='整改结束' 
                                                        and  a.workstream !='隐患登记' and a.workstream !='隐患评估' and a.workstream !='隐患完善' and a.workstream !='制定整改计划'  and  b.changedutydepartcode  like  '{0}%'  group by hiddepart
                                             ) d on a.hiddepart = d.hiddepart  order by  b.zdwbh desc ,a.ybwbh desc , a.itemname asc", code);
                    break;
            }
            return BaseRepository().FindTable(sql);
        }
        #endregion

        #region 隐患整改率
        /// <summary>
        /// 隐患整改率
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable GetHiddenChangeForLeaderCockpit(Operator user)
        {

            string sql = string.Empty;

            if (user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户"))
            {
                sql = string.Format(@"with  ybTable as (      
                                                      select * from (
                                                                select  count(1) as pValue, changedutydepartcode,'已整改' changestatus  from  bis_htbaseinfo a 
                                                                left join v_htchangeinfo b on a.hidcode = b.hidcode
                                                                left join base_dataitemdetail c on a.hidrank = c.itemdetailid  
                                                                where   a.workstream != '隐患整改'  and  substr(c.itemname,length(c.itemname)-3) ='一般隐患' and a.hiddepart = '{0}' 
                                                                and  a.workstream !='隐患登记' and a.workstream !='隐患评估' and a.workstream !='隐患完善' and a.workstream !='制定整改计划' 
                                                                and b.changedutydepartcode is not null  group by changedutydepartcode
                                                                union 
                                                                select  count(1) as pValue, changedutydepartcode,'未整改' changestatus  from  bis_htbaseinfo a 
                                                                left join v_htchangeinfo b on a.hidcode = b.hidcode
                                                                left join base_dataitemdetail c on a.hidrank = c.itemdetailid  
                                                                where   a.workstream = '隐患整改'  and  substr(c.itemname,length(c.itemname)-3)='一般隐患' and a.hiddepart = '{0}' 
                                                                and b.changedutydepartcode is not null  group by changedutydepartcode
                                                            ) pivot (sum(pValue) for changestatus in ('未整改' as nonChange,'已整改' as thenChange))
                                    ),
                                    zdTable as (
                                                 select * from (
                                                                select  count(1) as pValue, changedutydepartcode,'已整改' changestatus  from  bis_htbaseinfo a 
                                                                left join v_htchangeinfo b on a.hidcode = b.hidcode
                                                                left join base_dataitemdetail c on a.hidrank = c.itemdetailid  
                                                                where   a.workstream != '隐患整改'  and  substr(c.itemname,length(c.itemname)-3) ='重大隐患' and a.hiddepart = '{0}' 
                                                                and  a.workstream !='隐患登记' and a.workstream !='隐患评估' and a.workstream !='隐患完善' and a.workstream !='制定整改计划' 
                                                                and b.changedutydepartcode is not null  group by changedutydepartcode
                                                                union 
                                                                select  count(1) as pValue, changedutydepartcode,'未整改' changestatus  from  bis_htbaseinfo a 
                                                                left join v_htchangeinfo b on a.hidcode = b.hidcode
                                                                left join base_dataitemdetail c on a.hidrank = c.itemdetailid  
                                                                where   a.workstream = '隐患整改'  and  substr(c.itemname,length(c.itemname)-3) ='重大隐患' and a.hiddepart = '{0}' 
                                                                and b.changedutydepartcode is not null  group by changedutydepartcode
                                                            ) pivot (sum(pValue) for changestatus in ('未整改' as nonChange,'已整改' as thenChange))
                                    )
                                    select a.encode as changedutydepartcode,a.fullname,a.sortcode, nvl(b.total,0) ybcount,nvl(b.thenChange,0) yzgybcount,nvl(c.thenChange,0) yzgzdcount,
                                                     (case when nvl(b.total,0) =0 then 0 else round( nvl(b.thenChange,0) / b.total *100,2) end) ybzgl ,
                                                     case when nvl(b.total,0)+ nvl(c.total,0) = 0 then 0 else round(nvl((nvl(b.thenChange,0)+ nvl(c.thenChange,0))/(nvl(b.total,0)+nvl(c.total,0))*100 ,0),2) end zgl,
                                                     (case when nvl(c.total,0) =0 then 0 else  round(nvl( c.thenChange,0) /c.total *100,2) end) zdzgl,nvl(c.total,0) zdcount
                                                    from (select encode ,fullname,sortcode from base_department  where nature in （'部门','厂级'）  and  organizeid = '{0}' ) a
                                                    left join (   
                                                       select b.encode changedutydepartcode,sum(nvl(a.thenChange,0)) thenChange ,sum(nvl(a.nonChange,0) + nvl(a.thenChange,0)) as total,b.fullname  from ybTable a 
                                                      left join (select encode ,fullname,sortcode from base_department  where nature='部门'  and  organizeid = '{0}') b on 
                                                      substr(a.changedutydepartcode,0,length(b.encode)) = b.encode  group by  b.encode,b.fullname
                                                      union
                                                      select b.encode changedutydepartcode,sum(nvl(a.thenChange,0)) thenChange ,sum(nvl(a.nonChange,0) + nvl(a.thenChange,0)) as total,b.fullname  from ybTable a 
                                                      left join (select encode ,fullname,sortcode from base_department  where nature='厂级'  and  organizeid = '{0}') b on 
                                                      a.changedutydepartcode = b.encode  group by  b.encode,b.fullname
                                                    ) b on a.encode = b.changedutydepartcode
                                                    left join 
                                                    (
                                                       select b.encode changedutydepartcode,sum(nvl(a.thenChange,0)) thenChange ,sum(nvl(a.nonChange,0) + nvl(a.thenChange,0)) as total,b.fullname  from zdTable a 
                                                      left join (select encode ,fullname,sortcode from base_department  where nature='部门'  and  organizeid = '{0}') b on 
                                                      substr(a.changedutydepartcode,0,length(b.encode)) = b.encode  group by  b.encode,b.fullname
                                                      union
                                                       select b.encode changedutydepartcode,sum(nvl(a.thenChange,0)) thenChange ,sum(nvl(a.nonChange,0) + nvl(a.thenChange,0)) as total,b.fullname  from zdTable a 
                                                      left join (select encode ,fullname,sortcode from base_department  where nature='厂级'  and  organizeid = '{0}') b on 
                                                      a.changedutydepartcode = b.encode   group by  b.encode,b.fullname
                                                    )  c on a.encode = c.changedutydepartcode order by a.encode, a.sortcode ", user.OrganizeId);
            }
            else
            {
                sql = string.Format(@"with  ybTable as (     
                  select * from (
                            select  count(1) as pValue, changedutydepartcode,'已整改' changestatus  from  bis_htbaseinfo a 
                            left join v_htchangeinfo b on a.hidcode = b.hidcode
                            left join base_dataitemdetail c on a.hidrank = c.itemdetailid  
                            where   a.workstream != '隐患整改'  and  substr(c.itemname,length(c.itemname)-3) ='一般隐患' and b.changedutydepartcode like '{0}%'
                            and  a.workstream !='隐患登记' and a.workstream !='隐患评估' and a.workstream !='隐患完善' and a.workstream !='制定整改计划' 
                            and b.changedutydepartcode is not null  group by changedutydepartcode
                            union 
                            select  count(1) as pValue, changedutydepartcode,'未整改' changestatus  from  bis_htbaseinfo a 
                            left join v_htchangeinfo b on a.hidcode = b.hidcode
                            left join base_dataitemdetail c on a.hidrank = c.itemdetailid  
                            where   a.workstream = '隐患整改'  and  substr(c.itemname,length(c.itemname)-3) ='一般隐患' and b.changedutydepartcode like '{0}%'
                            and b.changedutydepartcode is not null  group by changedutydepartcode
                        ) pivot (sum(pValue) for changestatus in ('未整改' as nonChange,'已整改' as thenChange))
                   ),
                    zdTable as (
                                 select * from (
                                                select  count(1) as pValue, changedutydepartcode,'已整改' changestatus  from  bis_htbaseinfo a 
                                                left join v_htchangeinfo b on a.hidcode = b.hidcode
                                                left join base_dataitemdetail c on a.hidrank = c.itemdetailid  
                                                where   a.workstream != '隐患整改'  and  substr(c.itemname,length(c.itemname)-3) ='重大隐患' and b.changedutydepartcode like '{0}%' 
                                                and  a.workstream !='隐患登记' and a.workstream !='隐患评估' and a.workstream !='隐患完善' and a.workstream !='制定整改计划' 
                                                and b.changedutydepartcode is not null  group by changedutydepartcode
                                                union 
                                                select  count(1) as pValue, changedutydepartcode,'未整改' changestatus  from  bis_htbaseinfo a 
                                                left join v_htchangeinfo b on a.hidcode = b.hidcode
                                            left join base_dataitemdetail c on a.hidrank = c.itemdetailid  
                                            where   a.workstream = '隐患整改'  and substr(c.itemname,length(c.itemname)-3) ='重大隐患' and b.changedutydepartcode like '{0}%'
                                            and b.changedutydepartcode is not null  group by changedutydepartcode
                                        ) pivot (sum(pValue) for changestatus in ('未整改' as nonChange,'已整改' as thenChange))
                )
           select  a.encode as changedutydepartcode,a.fullname,a.sortcode, nvl(b.total,0) ybcount,nvl(b.thenChange,0) yzgybcount,nvl(c.thenChange,0) yzgzdcount,
                                           (case when nvl(b.total,0) =0 then 0 else round( nvl(b.thenChange,0) / b.total *100,2) end) ybzgl ,
                                           case when nvl(b.total,0)+nvl(c.total,0)=0 then 0 else round(nvl((nvl(b.thenChange,0)+ nvl(c.thenChange,0))/(nvl(b.total,0)+nvl(c.total,0))*100 ,0),2) end zgl,
                                           (case when nvl(c.total,0) =0 then 0 else  round(nvl( c.thenChange,0) /c.total *100,2) end) zdzgl,nvl(c.total,0) zdcount
                                        from (select encode ,fullname,sortcode from base_department  where  encode like '{0}%' ) a
                                        left join (   
                                           select b.encode changedutydepartcode,sum(nvl(a.thenChange,0)) thenChange ,sum(nvl(a.nonChange,0) + nvl(a.thenChange,0)) as total,b.fullname  from ybTable a 
                                          left join (select encode ,fullname,sortcode from base_department  where encode like '{0}%') b on 
                                          a.changedutydepartcode = b.encode  group by  b.encode,b.fullname
                                        ) b on a.encode = b.changedutydepartcode
                                        left join 
                                        (
                                           select b.encode changedutydepartcode,sum(nvl(a.thenChange,0)) thenChange ,sum(nvl(a.nonChange,0) + nvl(a.thenChange,0)) as total,b.fullname  from zdTable a 
                                          left join (select encode ,fullname,sortcode from base_department  where encode like '{0}%') b on 
                                         a.changedutydepartcode = b.encode  group by  b.encode,b.fullname
                                        )  c on a.encode = c.changedutydepartcode order by a.encode, a.sortcode  ", user.DeptCode);
            }

            return BaseRepository().FindTable(sql);
        }
        #endregion

        #region 今日作业风险/高风险作业统计
        /// <summary>
        /// 今日作业风险/高风险作业统计
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable GetHighRiskWorkingForLeaderCockpit(Operator user, int mode)
        {
            string strWhere = "";

            if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级"))
            {
                strWhere += string.Format(" and workdeptcode like '{0}%' ", user.OrganizeCode);
            }
            else
            {
                strWhere += string.Format(" and ((workdeptcode in(select encode from base_department where encode like '{0}%'))  or (engineeringid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", user.DeptCode, user.DeptId);
            }
            string sql = string.Empty;
            string isJdz = new DataItemDetailService().GetItemValue("景德镇版本");
            string strSql = string.Empty;
            if (!string.IsNullOrEmpty(isJdz))
            {
                //景德镇 高处作业风险等级处理
                strSql = "(CASE WHEN WORKTYPE='12' THEN (select itemid from base_dataitem where itemcode='CommonWorkType') ELSE (select itemid from base_dataitem where itemcode='CommonRiskType') END)";
            }
            else
            {
                strSql = "(select itemid from base_dataitem where itemcode='CommonRiskType')";
            }
            if (mode == 0)
            {
                sql = string.Format(@"select t.*,t1.itemname from(
                                        select to_char(a.id) as id,to_char(a.worktype) as worktype,b.itemname as risktypename,to_char(a.risktype) as risktype,to_char(workdeptname) as workdeptname,
                                        to_char(workareaname) as workareaname,to_char(a.workdeptcode) as workdeptcode,to_char(a.engineeringid) as engineeringid   from  bis_highriskcommonapply a 
                                        left join base_dataitemdetail b on a.risktype=b.itemvalue and itemid ={1} 
                                        where applystate='5' and  realityworkstarttime is not null and realityworkendtime is null

                                        union all
                                        select to_char(a.id) as id,'-1' as worktype,'' as risktypename,''as risktype,to_char(a.setupcompanyname) as workdeptname,to_char(a.workarea) as workareaname,
                                        to_char(a.setupcompanycode) as workdeptcode,to_char(a.outprojectid) as engineeringid   from bis_scaffold a left join 
                                        (select id,scaffoldid,auditusername,auditstate,auditdate,row_number() over(partition by scaffoldid order by auditdate desc) as num from bis_scaffoldauditrecord) b 
                                        on a.id = b.scaffoldid and b.num = 1 where a.id in (select id from bis_scaffold where scaffoldtype = 0 and auditstate = 3 
                                        and id not in(select id from bis_scaffold where id in(select setupinfoid from bis_scaffold where scaffoldtype = 1 and auditstate = 3))) 
                                        and  a.actsetupstartdate is not null and a.actsetupenddate is null

                                        union all
                                        select to_char(a.id) as id,'-2' as worktype,'' as risktypename,''as risktype,to_char(a.setupcompanyname) as workdeptname,to_char(a.workarea) as workareaname,to_char(a.setupcompanycode) as workdeptcode,to_char(a.outprojectid) as engineeringid  from bis_scaffold a left join bis_scaffold c 
                                        on nvl(a.setupinfoid,'-') = c.id left join
                                        (select id,scaffoldid,auditusername,auditstate,auditdate,row_number() over(partition by scaffoldid order by auditdate desc) as num from bis_scaffoldauditrecord) d 
                                        on c.id = d.scaffoldid and d.num = 1 where a.auditstate = 3 and a.scaffoldtype = 2 and a.realitydismentlestartdate is not null and a.realitydismentleenddate is null
                                        union all
                                        select to_char(a.id) as id,'-3' as worktype,'' as risktypename,''as risktype,to_char(workunit) as workdeptname,to_char(workarea) as workareaname,to_char(a.workunitcode) as workdeptcode,to_char(a.projectid) as engineeringid from bis_Safetychange a where iscommit=1 and isapplyover=1 
                                        and a.id not in (select id from bis_safetychange where ((isaccpcommit=0 and isaccepover=0  and  RealityChangeTime is null) or (isaccpcommit=1 and isaccepover=1)))
                            ) t left join base_dataitemdetail t1 on t.worktype=t1.itemvalue where t1.itemid =(select itemid from base_dataitem where itemcode='StatisticsType') and t1.enabledmark=1 {0}", strWhere, strSql);

                sql += " order by risktype";
            }
            else if (mode == 1)
            {
                sql = string.Format(@"select nvl(t1.nums,0) nums,t.itemname from(
                                           select itemvalue,itemname from   base_dataitemdetail where  itemid =(select itemid from base_dataitem where itemcode='StatisticsType') and enabledmark=1
                                        ) t left join (
                                         select count(id) nums,worktype from (
                                              select to_char(a.id) as id,to_char(a.worktype) as worktype,b.itemname as risktypename,to_char(a.risktype) as risktype,to_char(workdeptname) as workdeptname,
                                              to_char(workareaname) as workareaname,to_char(a.workdeptcode) as workdeptcode,to_char(a.engineeringid) as engineeringid  from  bis_highriskcommonapply a 
                                              left join base_dataitemdetail b on a.risktype=b.itemvalue and itemid ={1}
                                              where applystate='5' and  realityworkstarttime is not null and realityworkendtime is null

                                              union all
                                              select to_char(a.id) as id,'-1' as worktype,'' as risktypename,''as risktype,to_char(a.setupcompanyname) as workdeptname,to_char(a.workarea) as workareaname,
                                              to_char(a.setupcompanycode) as workdeptcode,to_char(a.outprojectid) as engineeringid  from bis_scaffold a left join 
                                              (select id,scaffoldid,auditusername,auditstate,auditdate,row_number() over(partition by scaffoldid order by auditdate desc) as num from bis_scaffoldauditrecord) b 
                                              on a.id = b.scaffoldid and b.num = 1 where a.id in (select id from bis_scaffold where scaffoldtype = 0 and auditstate = 3 
                                              and id not in(select id from bis_scaffold where id in(select setupinfoid from bis_scaffold where scaffoldtype = 1 and auditstate = 3))) 
                                              and  a.actsetupstartdate is not null and a.actsetupenddate is null

                                              union all
                                              select to_char(a.id) as id,'-2' as worktype,'' as risktypename,''as risktype,to_char(a.setupcompanyname) as workdeptname,to_char(a.workarea) as workareaname,to_char(a.setupcompanycode) as workdeptcode,to_char(a.outprojectid) as engineeringid  from bis_scaffold a left join bis_scaffold c 
                                              on nvl(a.setupinfoid,'-') = c.id left join
                                              (select id,scaffoldid,auditusername,auditstate,auditdate,row_number() over(partition by scaffoldid order by auditdate desc) as num from bis_scaffoldauditrecord) d 
                                              on c.id = d.scaffoldid and d.num = 1 where a.auditstate = 3 and a.scaffoldtype = 2 and a.realitydismentlestartdate is not null and a.realitydismentleenddate is null
                                              union all
                                              select to_char(a.id) as id,'-3' as worktype,'' as risktypename,''as risktype,to_char(workunit) as workdeptname,to_char(workarea) as workareaname,to_char(a.workunitcode) as workdeptcode,to_char(a.projectid) as engineeringid from bis_Safetychange a where iscommit=1 and isapplyover=1 
                                              and a.id not in(select id from bis_safetychange where ((isaccpcommit=0 and isaccepover=0  and  RealityChangeTime is null) or (isaccpcommit=1 and isaccepover=1)))
                                           )  a where 1=1 {0} group by worktype
                                         ) t1 on t.itemvalue =t1.worktype  ", strWhere, strSql);
            }
            else if (mode == 2)
            {
                sql = string.Format(@"select count(1) num from (select to_char(a.workdeptcode) as workdeptcode,to_char(a.engineeringid) as engineeringid  from  bis_highriskcommonapply a 
                                    left join base_dataitemdetail b on a.risktype=b.itemvalue and itemid ={1}
                                    where applystate='5' and  realityworkstarttime is not null and realityworkendtime is null) where 1=1 {0}
                                    union all
                                    select count(1) num from (select to_char(a.setupcompanycode) as workdeptcode,to_char(a.outprojectid) as engineeringid   from bis_scaffold a left join 
                                    (select id,scaffoldid,auditusername,auditstate,auditdate,row_number() over(partition by scaffoldid order by auditdate desc) as num from bis_scaffoldauditrecord) b 
                                    on a.id = b.scaffoldid and b.num = 1 where a.id in (select id from bis_scaffold where scaffoldtype = 0 and auditstate = 3 
                                    and id not in(select id from bis_scaffold where id in(select setupinfoid from bis_scaffold where scaffoldtype = 1 and auditstate = 3))) 
                                    and  a.actsetupstartdate is not null and a.actsetupenddate is null)  where 1=1 {0}
                                     union all
                                     select count(1) num from (select to_char(a.setupcompanycode) as workdeptcode,to_char(a.outprojectid) as engineeringid   from bis_scaffold a left join bis_scaffold c  on nvl(a.setupinfoid,'-') = c.id left join
                                    (select id,scaffoldid,auditusername,auditstate,auditdate,row_number() over(partition by scaffoldid order by auditdate desc) as num from bis_scaffoldauditrecord) d 
                                    on c.id = d.scaffoldid and d.num = 1 where a.auditstate = 3 and a.scaffoldtype = 2 and a.realitydismentlestartdate is not null and a.realitydismentleenddate is null)  where 1=1 {0}
                                     union all
                                     select count(1) num from (select to_char(a.workunitcode) as workdeptcode,to_char(a.projectid) as engineeringid from bis_Safetychange a where iscommit=1 and isapplyover=1 
                                    and a.id not in(select id from bis_safetychange where ((isaccpcommit=0 and isaccepover=0  and  RealityChangeTime is null) or (isaccpcommit=1 and isaccepover=1)))) where 1=1 {0}", strWhere, strSql);
            }
            else if (mode == 3) //广西华昇首页 查询今日风险作业
            {
                var Formdata = JobApprovalFormService.GetList("").Where(t => t.RealityJobStartTime != null && t.RealityJobEndTime == null).Select(x => x.JobSafetyCardId).ToList();
                string str = string.Empty;
                List<string> ilist = new List<string>();
                foreach (var item in Formdata)
                {
                    if (item != null)
                    {
                        var lists = item.Split(',');
                        foreach (var item1 in lists)
                        {
                            ilist.Add(item1);
                        }
                    }
                }
                str = string.Join("','", ilist);
                string where = "";
                if (!string.IsNullOrWhiteSpace(str))
                {
                    where += string.Format(" and a.id not in('{0}')", str);
                }
                sql = string.Format(@"select * from( select to_char(a.id) as id,to_char(a.jobarea) as workareaname,to_char(a.JobDeptName) as workdeptname, to_char(a.jobtypename) as itemname,'4' as RiskTypeValue, '' as risktypename
                                  from BIS_JOBSAFETYCARDAPPLY a
                                 where a.jobstate = 10 and a.createuserorgcode='{0}' {1}
                                union all
                                select  to_char(a.id) as id,to_char(a.jobarea) as workareaname,to_char(a.JobDeptName) as workdeptname,to_char(a.JOBTYPENAME) as itemname,to_char(a.joblevel) as RiskTypeValue,
                                d.itemname as risktypename from BIS_JOBAPPROVALFORM a
                           left join (select c.itemname,c.itemvalue from base_dataitemdetail c where c.itemid in (select itemid from base_dataitem where itemcode='DangerousJobCheck')) d on a.joblevel=d.itemvalue
                        where a.realityjobstarttime is not null and a.realityjobendtime is null and a.createuserorgcode='{0}') order by RiskTypeValue", user.OrganizeCode, where);
            }
            else if (mode == 4) //广西华昇 高风险作业统计
            {
                sql = @"select count(id) nums, d.itemname itemname from BIS_JOBSAFETYCARDAPPLY t
                            right join (select itemname,itemcode from base_dataitem where parentid = ( select itemid from base_dataitem  where itemcode = 'DangerousJobConfig') and itemcode not in ('DangerousJobList','DangerousJobCheck') ) d 
                            on t.jobtype=d.itemcode 
                            group by d.itemname, jobtype";
            }
            return this.BaseRepository().FindTable(sql);
        }
        #endregion

        #endregion

        #region 右侧屏

        #region 各部门未闭环违章统计
        /// <summary>
        ///各部门未闭环违章统计
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>

        public DataTable GetNoCloseLoopLllegalStatistics(Operator user)
        {
            string sql = string.Empty;

            if (user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户"))
            {
                sql = string.Format(@"with ybTable as (
                                    select b.reformdeptcode ,a.id,a.flowstate ,c.itemname lllegallevelname from bis_lllegalregister a 
                                    left join v_lllegalreforminfo b on a.id = b.lllegalid 
                                    left join base_dataitemdetail  c on a.lllegallevel = c.itemdetailid 
                                    where a.flowstate in (select itemname from v_yesqrwzstatus) and c.itemname ='一般违章'
                                ),
                                jyzTable as (
                                 select b.reformdeptcode ,a.id ,a.flowstate,c.itemname lllegallevelname from bis_lllegalregister a 
                                                                              left join v_lllegalreforminfo b on a.id = b.lllegalid 
                                                                              left join base_dataitemdetail  c on a.lllegallevel = c.itemdetailid 
                                                                              where a.flowstate in (select itemname from v_yesqrwzstatus) and c.itemname ='较严重违章' 
                                ),
                                yzTable as (
                                 select b.reformdeptcode ,a.id,a.flowstate ,c.itemname lllegallevelname from bis_lllegalregister a 
                                                                              left join v_lllegalreforminfo b on a.id = b.lllegalid 
                                                                              left join base_dataitemdetail  c on a.lllegallevel = c.itemdetailid 
                                                                              where a.flowstate in (select itemname from v_yesqrwzstatus) and c.itemname ='严重违章' 
                                ),
                                ybhzTb as (
                                 select  count(b.id) ybwbh , a.encode ,a.fullname,a.description  from (
                                                                              select departmentid,encode ,fullname ,deptcode ,organizeid ,description from base_department  where nature ='部门' and organizeid =  '{0}' 
                                                                          ) a
                                                                          left join 
                                                                          (
                                                                              select * from ybTable where  flowstate !='流程结束'
                                                                          ) b on a.encode = substr(b.reformdeptcode,1,length(a.encode))   group by a.encode,a.fullname,a.description
                                                                          union
                                                                              select  count(b.id) ybwbh , a.encode ,a.fullname,a.description from (
                                                                              select departmentid,encode ,fullname ,deptcode ,organizeid,description from base_department  where nature ='厂级' and organizeid =  '{0}' 
                                                                          ) a
                                                                          left join 
                                                                          (
                                                                              select * from ybTable where  flowstate !='流程结束'
                                                                          ) b on a.encode = b.reformdeptcode   group by a.encode,a.fullname,a.description
                                ),
                                jyzhzTb as (
                                      select  count(b.id) jyzwbh , a.encode ,a.fullname,a.description from (
                                                               select departmentid,encode ,fullname ,deptcode ,organizeid,description from base_department  where nature ='部门' and organizeid =  '{0}' 
                                                          ) a
                                                          left join 
                                                          (
                                                              select * from jyzTable where  flowstate !='流程结束'
                                                          ) b on a.encode = substr(b.reformdeptcode,1,length(a.encode))   group by a.encode,a.fullname,a.description
                                                          union
                                                              select  count(b.id) jyzwbh , a.encode ,a.fullname,a.description from (
                                                              select departmentid,encode ,fullname ,deptcode ,organizeid,description from base_department  where nature ='厂级' and organizeid =  '{0}' 
                                                          ) a
                                                          left join 
                                                          (
                                                            select * from jyzTable where  flowstate !='流程结束'
                                                          ) b on a.encode = b.reformdeptcode   group by a.encode,a.fullname,a.description
                                ),
                                yzhzTb as (
                                    select  count(b.id) yzwbh , a.encode ,a.fullname,a.description from (
                                                                          select departmentid,encode ,fullname ,deptcode ,organizeid,description from base_department  where nature ='部门' and organizeid =   '{0}'  
                                                                      ) a
                                                                      left join 
                                                                      (
                                                                           select * from yzTable where  flowstate !='流程结束'
                                                                      ) b on a.encode = substr(b.reformdeptcode,1,length(a.encode))   group by a.encode,a.fullname,a.description
                                                                      union
                                                                          select  count(b.id) yzwbh , a.encode ,a.fullname,a.description from (
                                                                          select departmentid,encode ,fullname ,deptcode ,organizeid,description from base_department  where nature ='厂级' and organizeid =   '{0}' 
                                                                      ) a
                                                                      left join 
                                                                      (
                                                                           select * from yzTable where  flowstate !='流程结束'
                                                                      ) b on a.encode = b.reformdeptcode   group by a.encode,a.fullname,a.description
                                )
                                select   (to_char(a.ybwbh) || '('|| to_char(nvl(case when d.ybzs =0 then 0 else round(a.ybwbh / d.ybzs*100,2) end,0))|| '%)') s1  , 
                                                                    (to_char(b.jyzwbh) || '(' || to_char(nvl(case when e.jyzzs =0 then 0 else round(b.jyzwbh/e.jyzzs*100,2) end,0)) || '%)') s2 , 
                                                                    (to_char(c.yzwbh) || '(' || to_char(nvl(case when f.yzzs =0 then 0 else round(c.yzwbh/f.yzzs*100,2) end,0)) || '%)') s3 ,  
                                                                  a.encode code ,a.fullname name  from ybhzTb  a 
                                                                  left join jyzhzTb b on a.encode = b.encode 
                                                                  left join yzhzTb c on a.encode = c.encode 
                                                                  left join 
                                                                  ( 
                                                                      select a.encode, count(b.id) ybzs from base_department a
                                                                      left join ybTable b on a.encode = substr(b.reformdeptcode,1,length(a.encode))  
                                                                      where  b.lllegallevelname ='一般违章'  and  a.nature ='厂级' and a.organizeid ='{0}'  group by  a.encode
                                                                  ) d on  substr(a.encode,1,length(d.encode)) =  d.encode
                                                                      left join   
                                                                  (
                                                                      select a.encode, count(b.id) jyzzs from base_department a
                                                                      left join jyzTable b on a.encode = substr(b.reformdeptcode,1,length(a.encode))  
                                                                      where  b.lllegallevelname ='较严重违章'  and  a.nature ='厂级' and a.organizeid ='{0}'   group by  a.encode  
                                                                  ) e on  substr(a.encode,1,length(e.encode)) =  e.encode 
                                                                      left join   
                                                                  (
                                                                      select a.encode, count(b.id) yzzs from base_department a
                                                                      left join yzTable b on a.encode = substr(b.reformdeptcode,1,length(a.encode))  
                                                                      where  b.lllegallevelname ='严重违章'  and  a.nature ='厂级' and a.organizeid =  '{0}'   group by  a.encode  
                                                                  ) f on  substr(a.encode,1,length(f.encode)) =  f.encode  order by  c.yzwbh desc ,b.jyzwbh desc ,a.ybwbh  desc,  a.description asc  , a.fullname asc  ", user.OrganizeId);
            }
            else
            {
                sql = string.Format(@" with ybTable as (
                                    select b.reformdeptcode ,a.id,a.flowstate ,c.itemname lllegallevelname from bis_lllegalregister a 
                                    left join v_lllegalreforminfo b on a.id = b.lllegalid 
                                    left join base_dataitemdetail  c on a.lllegallevel = c.itemdetailid 
                                    where a.flowstate in (select itemname from v_yesqrwzstatus) and c.itemname ='一般违章'
                                ),
                                jyzTable as (
                                    select b.reformdeptcode ,a.id ,a.flowstate,c.itemname lllegallevelname from bis_lllegalregister a 
                                                                                left join v_lllegalreforminfo b on a.id = b.lllegalid 
                                                                                left join base_dataitemdetail  c on a.lllegallevel = c.itemdetailid 
                                                                                where a.flowstate in (select itemname from v_yesqrwzstatus) and c.itemname ='较严重违章' 
                                ),
                                yzTable as (
                                    select b.reformdeptcode ,a.id,a.flowstate ,c.itemname lllegallevelname from bis_lllegalregister a 
                                                                                left join v_lllegalreforminfo b on a.id = b.lllegalid 
                                                                                left join base_dataitemdetail  c on a.lllegallevel = c.itemdetailid 
                                                                                where a.flowstate in (select itemname from v_yesqrwzstatus) and c.itemname ='严重违章' 
                                ),
                                ybhzTb as (
                                            select  count(b.id) ybwbh , a.encode ,a.fullname,a.description  from (
                                                                                select departmentid,encode ,fullname ,deptcode ,organizeid ,description from base_department  where encode like '{0}%' 
                                                                            ) a
                                                                            left join 
                                                                            (
                                                                                select * from ybTable where   flowstate !='流程结束'
                                                                            ) b on a.encode = b.reformdeptcode   group by a.encode,a.fullname,a.description
                                ),
                                jyzhzTb as (
                                        select  count(b.id) jyzwbh , a.encode ,a.fullname,a.description from (
                                                                                select departmentid,encode ,fullname ,deptcode ,organizeid,description from base_department   where encode like '{0}%' 
                                                                            ) a
                                                                            left join 
                                                                            (
                                                                                select * from jyzTable where  flowstate !='流程结束'
                                                                            ) b on a.encode = b.reformdeptcode   group by a.encode,a.fullname,a.description
                                ),
                                yzhzTb as (
                                        select  count(b.id) yzwbh , a.encode ,a.fullname,a.description from (
                                                                            select departmentid,encode ,fullname ,deptcode ,organizeid,description from base_department where encode like '{0}%' 
                                                                        ) a
                                                                        left join 
                                                                        (
                                                                            select * from yzTable where   flowstate !='流程结束'
                                                                        ) b on a.encode = b.reformdeptcode   group by a.encode,a.fullname,a.description
                                )
                                select   (to_char(a.ybwbh) || '('|| to_char(nvl(case when d.ybzs =0 then 0 else round(a.ybwbh / d.ybzs*100,2) end,0))|| '%)') s1  , 
                                    (to_char(b.jyzwbh) || '(' || to_char(nvl(case when e.jyzzs =0 then 0 else round(b.jyzwbh/e.jyzzs*100,2) end,0)) || '%)') s2 , 
                                    (to_char(c.yzwbh) || '(' || to_char(nvl(case when f.yzzs =0 then 0 else round(c.yzwbh/f.yzzs*100,2) end,0)) || '%)') s3 ,  
                                  a.encode code ,a.fullname name  from ybhzTb  a 
                                  left join jyzhzTb  b on a.encode = b.encode 
                                  left join yzhzTb c on a.encode = c.encode 
                                  left join 
                                  ( 
                                      select a.encode, count(b.id) ybzs from base_department a
                                      left join ybTable b on a.encode = substr(b.reformdeptcode,1,length(a.encode))  
                                      where  b.lllegallevelname ='一般违章'   and a.encode like '{0}%'   group by  a.encode
                                  ) d on  a.encode =  d.encode
                                      left join   
                                  (
                                      select a.encode, count(b.id) jyzzs from base_department a
                                      left join jyzTable b on a.encode = substr(b.reformdeptcode,1,length(a.encode))  
                                      where  b.lllegallevelname ='较严重违章'   and a.encode like '{0}%'   group by  a.encode  
                                  ) e on  substr(a.encode,1,length(e.encode)) =  e.encode 
                                      left join   
                                  (
                                      select a.encode, count(b.id) yzzs from base_department a
                                      left join yzTable b on a.encode = substr(b.reformdeptcode,1,length(a.encode))  
                                      where  b.lllegallevelname ='严重违章'  and a.encode like '{0}%'   group by  a.encode  
                                  ) f on  substr(a.encode,1,length(f.encode)) =  f.encode  order by  c.yzwbh desc ,b.jyzwbh desc ,a.ybwbh  desc,  a.description asc,a.fullname asc   ", user.DeptCode);
            }

            return BaseRepository().FindTable(sql);

        }
        #endregion

        #region 各部门违章整改率统计
        /// <summary>
        /// 各部门违章整改率统计
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable GetLllegalChangeForLeaderCockpit(Operator user)
        {
            string sql = string.Empty;

            if (user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户"))
            {
                sql = string.Format(@"with ybTable as (
                                           select * from (
                                                      select  count(1) as pValue, b.reformdeptcode,'已整改' changestatus  from  bis_lllegalregister a 
                                                      left join v_lllegalreforminfo b on a.id = b.lllegalid 
                                                      left join base_dataitemdetail  c on a.lllegallevel = c.itemdetailid 
                                                      where  a.flowstate = '流程结束'  and c.itemname ='一般违章' and b.reformdeptcode like '{1}%'   group by b.reformdeptcode
                                                      union 
                                                      select  count(1) as pValue, b.reformdeptcode,'未整改' changestatus  from   bis_lllegalregister a 
                                                      left join v_lllegalreforminfo b on a.id = b.lllegalid 
                                                      left join base_dataitemdetail  c on a.lllegallevel = c.itemdetailid 
                                                      where a.flowstate in (select itemname from v_yesqrwzstatus) and  flowstate != '流程结束'  and  c.itemname ='一般违章'  and 
                                                      b.reformdeptcode like '{1}%'   group by  b.reformdeptcode
                                                  ) pivot (sum(pValue) for changestatus in ('未整改' as nonChange,'已整改' as thenChange))
                                ),
                                jyzTable as (
                                        select * from (
                                                      select  count(1) as pValue, b.reformdeptcode,'已整改' changestatus  from  bis_lllegalregister a 
                                                      left join v_lllegalreforminfo b on a.id = b.lllegalid 
                                                      left join base_dataitemdetail  c on a.lllegallevel = c.itemdetailid 
                                                      where  a.flowstate = '流程结束'  and c.itemname ='较严重违章' and b.reformdeptcode like '{1}%'   group by b.reformdeptcode
                                                      union 
                                                      select  count(1) as pValue, b.reformdeptcode,'未整改' changestatus  from   bis_lllegalregister a 
                                                      left join v_lllegalreforminfo b on a.id = b.lllegalid 
                                                      left join base_dataitemdetail  c on a.lllegallevel = c.itemdetailid 
                                                      where a.flowstate in (select itemname from v_yesqrwzstatus) and  flowstate != '流程结束'  and  c.itemname ='较严重违章'  and 
                                                      b.reformdeptcode like '{1}%'   group by  b.reformdeptcode
                                                  ) pivot (sum(pValue) for changestatus in ('未整改' as nonChange,'已整改' as thenChange))
                                ),
                                yzTable as (
                                       select * from (
                                                      select  count(1) as pValue, b.reformdeptcode,'已整改' changestatus  from  bis_lllegalregister a 
                                                      left join v_lllegalreforminfo b on a.id = b.lllegalid 
                                                      left join base_dataitemdetail  c on a.lllegallevel = c.itemdetailid 
                                                      where  a.flowstate = '流程结束'  and c.itemname ='严重违章' and b.reformdeptcode like '{1}%'   group by b.reformdeptcode
                                                      union 
                                                      select  count(1) as pValue, b.reformdeptcode,'未整改' changestatus  from   bis_lllegalregister a 
                                                      left join v_lllegalreforminfo b on a.id = b.lllegalid 
                                                      left join base_dataitemdetail  c on a.lllegallevel = c.itemdetailid 
                                                      where a.flowstate in (select itemname from v_yesqrwzstatus) and  flowstate != '流程结束'  and  c.itemname ='严重违章'  and 
                                                      b.reformdeptcode like '{1}%'   group by  b.reformdeptcode
                                                  ) pivot (sum(pValue) for changestatus in ('未整改' as nonChange,'已整改' as thenChange))
                                )
                            select a.encode as reformdeptcode,a.fullname,a.sortcode, 
                                (case when nvl(b.total,0) =0 then 0 else  round(nvl(b.thenChange,0)/nvl(b.total,0) *100,2) end) ybzgl,(nvl(b.total,0) - nvl(b.thenChange,0))  ybcount,  (nvl(c.total,0) - nvl(c.thenChange,0))  jyzcount, (nvl(d.total,0) - nvl(d.thenChange,0))   zdcount,
                                (case when nvl(c.total,0) =0 then 0 else  round(nvl(c.thenChange,0)/nvl(c.total,0) *100,2) end) jyzzgl,
                                 case when nvl(b.total,0) + nvl(c.total,0) + nvl(d.total,0) =0 then 0 else round(nvl((nvl(b.thenChange,0) + nvl(c.thenChange,0) + nvl(d.thenChange,0))/(nvl(b.total,0) + nvl(c.total,0) + nvl(d.total,0)),0)*100,2) end zgl,
                                (case when nvl(d.total,0) =0 then 0 else  round(nvl(d.thenChange,0)/nvl(d.total,0) *100,2) end) yzzgl   
                                        from (select encode ,fullname,sortcode from base_department  where nature in （'部门','厂级')  and  organizeid = '{0}' ) a
                                        left join (   
                                            select b.encode reformdeptcode,sum(nvl(a.thenChange,0)) thenChange ,sum(nvl(a.nonChange,0) + nvl(a.thenChange,0)) as total,b.fullname  from ybTable a 
                                            left join (select encode ,fullname,sortcode from base_department  where nature='部门'  and  organizeid = '{0}') b on 
                                            substr(a.reformdeptcode,0,length(b.encode)) = b.encode  group by  b.encode,b.fullname
                                            union
                                            select b.encode reformdeptcode,sum(nvl(a.thenChange,0)) thenChange ,sum(nvl(a.nonChange,0) + nvl(a.thenChange,0)) as total,b.fullname  from ybTable  a 
                                            left join (select encode ,fullname,sortcode from base_department  where nature='厂级'  and  organizeid = '{0}') b on 
                                            a.reformdeptcode = b.encode  group by  b.encode,b.fullname
                                        ) b on a.encode = b.reformdeptcode
                                        left join 
                                        (
                                          select b.encode reformdeptcode,sum(nvl(a.thenChange,0)) thenChange ,sum(nvl(a.nonChange,0) + nvl(a.thenChange,0)) as total,b.fullname  from jyzTable a 
                                          left join (select encode ,fullname,sortcode from base_department  where nature='部门'  and  organizeid = '{0}') b on 
                                          substr(a.reformdeptcode,0,length(b.encode)) = b.encode  group by  b.encode,b.fullname
                                          union
                                          select b.encode reformdeptcode,sum(nvl(a.thenChange,0)) thenChange ,sum(nvl(a.nonChange,0) + nvl(a.thenChange,0)) as total,b.fullname  from jyzTable a 
                                          left join (select encode ,fullname,sortcode from base_department  where nature='厂级'  and  organizeid = '{0}') b on 
                                          a.reformdeptcode = b.encode  group by  b.encode,b.fullname
                                        )  c on a.encode = c.reformdeptcode 
                                        left join 
                                        (
                                           select b.encode reformdeptcode,sum(nvl(a.thenChange,0)) thenChange ,sum(nvl(a.nonChange,0) + nvl(a.thenChange,0)) as total,b.fullname  from yzTable a 
                                          left join (select encode ,fullname,sortcode from base_department  where nature='部门'  and  organizeid = '{0}') b on 
                                          substr(a.reformdeptcode,0,length(b.encode)) = b.encode  group by  b.encode,b.fullname
                                          union
                                             select b.encode reformdeptcode,sum(nvl(a.thenChange,0)) thenChange ,sum(nvl(a.nonChange,0) + nvl(a.thenChange,0)) as total,b.fullname  from yzTable a 
                                          left join (select encode ,fullname,sortcode from base_department  where nature='厂级'  and  organizeid = '{0}') b on 
                                          a.reformdeptcode = b.encode  group by  b.encode,b.fullname
                                        )  d on a.encode = d.reformdeptcode  order by a.encode, a.sortcode ", user.OrganizeId, user.OrganizeCode);
            }
            else
            {
                sql = string.Format(@"with ybTable as (
                                       select * from (
                                                  select  count(1) as pValue, b.reformdeptcode,'已整改' changestatus  from  bis_lllegalregister a 
                                                  left join v_lllegalreforminfo b on a.id = b.lllegalid 
                                                  left join base_dataitemdetail  c on a.lllegallevel = c.itemdetailid 
                                                  where  a.flowstate = '流程结束'  and c.itemname ='一般违章' and b.reformdeptcode like '{0}%'   group by b.reformdeptcode
                                                  union 
                                                  select  count(1) as pValue, b.reformdeptcode,'未整改' changestatus  from   bis_lllegalregister a 
                                                  left join v_lllegalreforminfo b on a.id = b.lllegalid 
                                                  left join base_dataitemdetail  c on a.lllegallevel = c.itemdetailid 
                                                  where a.flowstate in (select itemname from v_yesqrwzstatus) and  flowstate != '流程结束'  and  c.itemname ='一般违章'  and 
                                                  b.reformdeptcode like '{0}%'   group by  b.reformdeptcode
                                              ) pivot (sum(pValue) for changestatus in ('未整改' as nonChange,'已整改' as thenChange))
                            ),
                            jyzTable as (
                                    select * from (
                                                  select  count(1) as pValue, b.reformdeptcode,'已整改' changestatus  from  bis_lllegalregister a 
                                                  left join v_lllegalreforminfo b on a.id = b.lllegalid 
                                                  left join base_dataitemdetail  c on a.lllegallevel = c.itemdetailid 
                                                  where  a.flowstate = '流程结束'  and c.itemname ='较严重违章' and b.reformdeptcode like '{0}%'   group by b.reformdeptcode
                                                  union 
                                                  select  count(1) as pValue, b.reformdeptcode,'未整改' changestatus  from   bis_lllegalregister a 
                                                  left join v_lllegalreforminfo b on a.id = b.lllegalid 
                                                  left join base_dataitemdetail  c on a.lllegallevel = c.itemdetailid 
                                                  where a.flowstate in (select itemname from v_yesqrwzstatus) and  flowstate != '流程结束'  and  c.itemname ='较严重违章'  and 
                                                  b.reformdeptcode like '{0}%'   group by  b.reformdeptcode
                                              ) pivot (sum(pValue) for changestatus in ('未整改' as nonChange,'已整改' as thenChange))
                            ),
                            yzTable as (
                                   select * from (
                                                  select  count(1) as pValue, b.reformdeptcode,'已整改' changestatus  from  bis_lllegalregister a 
                                                  left join v_lllegalreforminfo b on a.id = b.lllegalid 
                                                  left join base_dataitemdetail  c on a.lllegallevel = c.itemdetailid 
                                                  where  a.flowstate = '流程结束'  and c.itemname ='严重违章' and b.reformdeptcode like '{0}%'   group by b.reformdeptcode
                                                  union 
                                                  select  count(1) as pValue, b.reformdeptcode,'未整改' changestatus  from   bis_lllegalregister a 
                                                  left join v_lllegalreforminfo b on a.id = b.lllegalid 
                                                  left join base_dataitemdetail  c on a.lllegallevel = c.itemdetailid 
                                                  where a.flowstate in (select itemname from v_yesqrwzstatus) and  flowstate != '流程结束'  and  c.itemname ='严重违章'  and 
                                                  b.reformdeptcode like '{0}%'   group by  b.reformdeptcode
                                              ) pivot (sum(pValue) for changestatus in ('未整改' as nonChange,'已整改' as thenChange))
                            )

                            select a.encode as reformdeptcode,a.fullname,a.sortcode, 
                                (case when nvl(b.total,0) =0 then 0 else  round(nvl(b.thenChange,0)/nvl(b.total,0) *100,2) end) ybzgl,(nvl(b.total,0) - nvl(b.thenChange,0))  ybcount,  (nvl(c.total,0) - nvl(c.thenChange,0))  jyzcount, (nvl(d.total,0) - nvl(d.thenChange,0))   zdcount,
                                (case when nvl(c.total,0) =0 then 0 else  round(nvl(c.thenChange,0)/nvl(c.total,0) *100,2) end) jyzzgl,
                                 case when nvl(b.total,0) + nvl(c.total,0) + nvl(d.total,0) =0 then 0 else round(nvl((nvl(b.thenChange,0) + nvl(c.thenChange,0) + nvl(d.thenChange,0))/(nvl(b.total,0) + nvl(c.total,0) + nvl(d.total,0)),0)*100,2) end zgl,
                                (case when nvl(d.total,0) =0 then 0 else  round(nvl(d.thenChange,0)/nvl(d.total,0) *100,2) end) yzzgl  
                                    from (select encode ,fullname,sortcode from base_department  where encode like '{0}%'  ) a
                                    left join (   
                                       select b.encode reformdeptcode,sum(nvl(a.thenChange,0)) thenChange ,sum(nvl(a.nonChange,0) + nvl(a.thenChange,0)) as total,b.fullname  from ybTable a 
                                      left join (select encode ,fullname,sortcode from base_department  where encode like '{0}%') b on 
                                      a.reformdeptcode = b.encode  group by  b.encode,b.fullname
                                    ) b on a.encode = b.reformdeptcode
                                    left join 
                                    (
                                       select b.encode reformdeptcode,sum(nvl(a.thenChange,0)) thenChange ,sum(nvl(a.nonChange,0) + nvl(a.thenChange,0)) as total,b.fullname  from jyzTable a 
                                      left join (select encode ,fullname,sortcode from base_department  where encode like '{0}%') b on 
                                      a.reformdeptcode = b.encode  group by  b.encode,b.fullname
                                    )  c on a.encode = c.reformdeptcode 
                                    left join 
                                    (
                                       select b.encode reformdeptcode,sum(nvl(a.thenChange,0)) thenChange ,sum(nvl(a.nonChange,0) + nvl(a.thenChange,0)) as total,b.fullname  from yzTable a 
                                      left join (select encode ,fullname,sortcode from base_department  where encode like '{0}%') b on 
                                      a.reformdeptcode = b.encode  group by  b.encode,b.fullname
                                    )  d on a.encode = d.reformdeptcode  order by a.encode, a.sortcode  ", user.DeptCode);
            }

            return BaseRepository().FindTable(sql);
        }
        #endregion

        #endregion

        #endregion

        #region 其他部分

        /// <summary>
        /// 获取外包工程数量信息（在建工程数量，外包人员在厂人数，外包工程总数，在场外包单位数，本月新进外包人员，外包单位违章次数）
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetWBProjectNum(ERCHTMS.Code.Operator user)
        {
            List<int> list = new List<int>();
            int count = 0;
            if (user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户"))
            {
                //在建外包工程数
                count = BaseRepository().FindObject(string.Format("select count(1) from EPG_OUTSOURINGENGINEER p where p.createuserorgcode='{0}' and ENGINEERSTATE='002' and isdeptadd=1", user.OrganizeCode)).ToInt();
                list.Add(count);
                //外包人员在厂人数
                count = BaseRepository().FindObject(string.Format("select count(1) from base_user u where u.ispresence='1' and isepiboly='1' and u.organizecode='{0}'", user.OrganizeCode)).ToInt();
                list.Add(count);


            }
            else if (user.RoleName.Contains("集团") || user.RoleName.Contains("省级"))
            {
                //在建外包工程数
                count = BaseRepository().FindObject(string.Format("select count(1) from EPG_OUTSOURINGENGINEER p where p.createuserorgcode in (select encode from BASE_DEPARTMENT d where d.deptcode like '{0}%' and d.nature='{1}') and ENGINEERSTATE='002' and isdeptadd=1", user.NewDeptCode, "厂级")).ToInt();
                list.Add(count);
                //外包人员在厂人数
                count = BaseRepository().FindObject(string.Format("select count(1) from base_user u where u.ispresence='1' and isepiboly='1' and u.organizecode in (select encode from BASE_DEPARTMENT d where d.deptcode like '{0}%' and d.nature='{1}')", user.NewDeptCode, "厂级")).ToInt();
                list.Add(count);
            }
            else
            {
                //在建外包工程数
                count = BaseRepository().FindObject(string.Format("select count(1) from EPG_OUTSOURINGENGINEER p where p.createuserdeptcode like '{0}%' and ENGINEERSTATE='002' and isdeptadd=1", user.DeptCode)).ToInt();
                list.Add(count);
                //外包人员在厂人数
                count = BaseRepository().FindObject(string.Format("select count(1) from base_user u where u.ispresence='1' and isepiboly='1'  and u.departmentcode like '{0}%' ", user.DeptCode)).ToInt();
                list.Add(count);
            }
            //外包工程总数
            count = BaseRepository().FindObject(string.Format("select count(1) from EPG_OUTSOURINGENGINEER p where p.createuserorgcode='{0}' and isdeptadd=1", user.OrganizeCode)).ToInt();
            list.Add(count);
            //在场外包单位数
            count = BaseRepository().FindObject(string.Format("select count(1) from EPG_OUTSOURCINGPROJECT p left join base_department d on p.OUTPROJECTID=d.departmentid where d.organizeid='{0}' and p.outorin=0", user.OrganizeId)).ToInt();
            list.Add(count);
            //本月新进外包人员
            count = BaseRepository().FindObject(string.Format("select count(1) from base_user u where u.ispresence='1' and isepiboly='1' and u.organizecode='{0}' and to_char(ENTERTIME,'yyyy-mm')='{1}'", user.OrganizeCode, DateTime.Now.ToString("yyyy-MM"))).ToInt();
            list.Add(count);
            //外包单位违章次数
            count = BaseRepository().FindObject(string.Format("select count(1) from v_lllegalbaseinfo t left join base_user u on t.LLLEGALPERSONID=userid where u.isepiboly='1' and organizecode='{0}' and to_char(lllegaltime,'yyyy')='{1}'", user.OrganizeCode, DateTime.Now.Year)).ToInt();
            list.Add(count);
            return list;
        }
        /// <summary>
        /// 获取危险源数量（依次为总数量，重大危险源数量）
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetDangerSourceNum(ERCHTMS.Code.Operator user)
        {
            List<int> list = new List<int>();
            int count = 0;
            if (user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户"))
            {
                //危险源总数
                count = BaseRepository().FindObject(string.Format("select count(1) from HSD_HAZARDSOURCE d where d.deptcode like '{0}%'", user.OrganizeCode)).ToInt();
                list.Add(count);
                //重大危险源总数
                count = BaseRepository().FindObject(string.Format("select count(1) from HSD_HAZARDSOURCE d where d.deptcode like '{0}%' and d.IsDanger>0", user.OrganizeCode)).ToInt();
                list.Add(count);
            }
            else if (user.RoleName.Contains("集团") || user.RoleName.Contains("省级"))
            {
                //危险源总数
                count = BaseRepository().FindObject(string.Format("select count(1) from HSD_HAZARDSOURCE d where d.createuserorgcode in (select encode from BASE_DEPARTMENT d where d.deptcode like '{0}%' and d.nature='{1}')", user.NewDeptCode, "厂级")).ToInt();
                list.Add(count);
                //重大危险源总数
                count = BaseRepository().FindObject(string.Format("select count(1) from HSD_HAZARDSOURCE d where d.createuserorgcode in (select encode from BASE_DEPARTMENT d where d.deptcode like '{0}%' and d.nature='{1}') and d.IsDanger>0", user.NewDeptCode, "厂级")).ToInt();
                list.Add(count);
            }
            else
            {
                //危险源总数
                count = BaseRepository().FindObject(string.Format("select count(1) from HSD_HAZARDSOURCE d where d.deptcode like '{0}%'", user.DeptCode)).ToInt();
                list.Add(count);
                //重大危险源总数
                count = BaseRepository().FindObject(string.Format("select count(1) from HSD_HAZARDSOURCE d where d.deptcode like '{0}%' and d.IsDanger>0", user.DeptCode)).ToInt();
                list.Add(count);
            }

            return list;
        }
        /// <summary>
        /// 获取事故数量信息（依次为事故起数，死亡人数，重伤人员）
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetAccidentNum(ERCHTMS.Code.Operator user)
        {
            List<int> list = new List<int>();
            int count = 0;
            if (user.RoleName.Contains("集团") || user.RoleName.Contains("省级"))
            {
                //事故起数
                count = BaseRepository().FindObject(string.Format("select count(1) from AEM_BULLETIN_DEAL a where issubmit_deal>0 and a.createuserorgcode in (select encode from BASE_DEPARTMENT d where d.deptcode like '{0}%' and d.nature='{2}') and IsSubmit_Deal>0 and to_char(createdate,'yyyy')='{1}'", user.NewDeptCode, DateTime.Now.Year, "厂级")).ToInt();
                list.Add(count);
                //事故死亡人数
                count = BaseRepository().FindObject(string.Format("select sum(SWNUM_DEAL) from AEM_BULLETIN_DEAL a where a.issubmit_deal>0 and  a.createuserorgcode  in (select encode from BASE_DEPARTMENT d where d.deptcode like '{0}%' and d.nature='{2}') and IsSubmit_Deal>0 and to_char(createdate,'yyyy')='{1}'", user.NewDeptCode, DateTime.Now.Year, "厂级")).ToInt();
                list.Add(count);
                //事故重伤人数
                count = BaseRepository().FindObject(string.Format("select sum(ZSNUM_DEAL) from AEM_BULLETIN_DEAL a where a.issubmit_deal>0 and  a.createuserorgcode  in (select encode from BASE_DEPARTMENT d where d.deptcode like '{0}%' and d.nature='{2}') and IsSubmit_Deal>0 and to_char(createdate,'yyyy')='{1}'", user.NewDeptCode, DateTime.Now.Year, "厂级")).ToInt();
                list.Add(count);
            }
            else
            {
                //事故起数
                count = BaseRepository().FindObject(string.Format("select count(1) from AEM_BULLETIN_DEAL a where issubmit_deal>0 and a.createuserorgcode='{0}' and IsSubmit_Deal>0 and to_char(createdate,'yyyy')='{1}'", user.OrganizeCode, DateTime.Now.Year)).ToInt();
                list.Add(count);
                //事故死亡人数
                count = BaseRepository().FindObject(string.Format("select sum(SWNUM_DEAL) from AEM_BULLETIN_DEAL a where a.issubmit_deal>0 and  a.createuserorgcode='{0}' and IsSubmit_Deal>0 and to_char(createdate,'yyyy')='{1}'", user.OrganizeCode, DateTime.Now.Year)).ToInt();
                list.Add(count);
                //事故重伤人数
                count = BaseRepository().FindObject(string.Format("select sum(ZSNUM_DEAL) from AEM_BULLETIN_DEAL a where a.issubmit_deal>0 and  a.createuserorgcode='{0}' and IsSubmit_Deal>0 and to_char(createdate,'yyyy')='{1}'", user.OrganizeCode, DateTime.Now.Year)).ToInt();
                list.Add(count);
            }
            return list;

        }
        /// <summary>
        /// 待分配的检查任务
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int GetAllotCheckCount(ERCHTMS.Code.Operator user)
        {
            int count = 0;
            if (user.RoleName.Contains("厂级"))
            {
                count = BaseRepository().FindObject(string.Format(@"select count(1) from BIS_SAFTYCHECKDATARECORD t where (belongdeptid='{0}' or belongdeptid='{2}') and status=0 and datatype=1 and (',' ||receiveuserids || ',') like '%,{1},%' and rid is not null", user.OrganizeId, user.Account, user.DeptId)).ToInt();
            }
            else
            {
                count = BaseRepository().FindObject(string.Format(@"select count(1) from BIS_SAFTYCHECKDATARECORD t where belongdeptid='{0}' and status=0 and datatype=1 and (',' ||receiveuserids || ',') like '%,{1},%' and rid is not null", user.DeptId, user.Account)).ToInt();
            }
            return count;
        }
        /// <summary>
        /// 隐患统计图表
        /// </summary>
        /// <param name="user">机构编码</param>
        /// <returns></returns>
        public string GetHTChart(ERCHTMS.Code.Operator user)
        {
            string orgCode = user.OrganizeCode;
            string[] xValues = { "隐患数量", "一般隐患数", "重大隐患数", "已整改隐患数", "逾期未整改隐患数" };
            List<int> yValues = new List<int>();
            int count = 0;
            if (user.RoleName.Contains("集团") || user.RoleName.Contains("省级"))
            {
                //隐患总数
                count = BaseRepository().FindObject(string.Format("select count(1) from V_BASEHIDDENINFO t where t.createuserorgcode in (select encode from BASE_DEPARTMENT d where d.deptcode like '{0}%' and d.nature='{2}') and changedutydepartcode is not null and to_char(createdate,'yyyy')='{1}'", user.NewDeptCode, DateTime.Now.Year, "厂级")).ToInt();
                yValues.Add(count);
                //一般隐患数
                count = BaseRepository().FindObject(string.Format("select count(1) from V_BASEHIDDENINFO t where t.createuserorgcode in (select encode from BASE_DEPARTMENT d where d.deptcode like '{0}%' and d.nature='{2}') and changedutydepartcode is not null and t.rankname='一般隐患' and to_char(createdate,'yyyy')='{1}'", user.NewDeptCode, DateTime.Now.Year, "厂级")).ToInt();
                yValues.Add(count);
                //重大隐患数
                count = BaseRepository().FindObject(string.Format("select count(1) from V_BASEHIDDENINFO t where t.createuserorgcode in (select encode from BASE_DEPARTMENT d where d.deptcode like '{0}%' and d.nature='{2}') and changedutydepartcode is not null and t.rankname='重大隐患' and to_char(createdate,'yyyy')='{1}'", user.NewDeptCode, DateTime.Now.Year, "厂级")).ToInt();
                yValues.Add(count);
                //已整改隐患数
                count = BaseRepository().FindObject(string.Format("select count(1) from V_BASEHIDDENINFO t where t.createuserorgcode in (select encode from BASE_DEPARTMENT d where d.deptcode like '{0}%' and d.nature='{2}') and changedutydepartcode is not null and t.workstream<>'隐患整改' and to_char(createdate,'yyyy')='{1}'", user.NewDeptCode, DateTime.Now.Year, "厂级")).ToInt();
                yValues.Add(count);
                //逾期未整改隐患数
                count = BaseRepository().FindObject(string.Format("select count(1) from V_BASEHIDDENINFO t where t.createuserorgcode in (select encode from BASE_DEPARTMENT d where d.deptcode like '{0}%' and d.nature='{2}') and changedutydepartcode is not null and (t.changedeadine + 1) <to_date('{2}','yyyy-mm-dd hh24:mi:ss') and t.workstream='隐患整改' and to_char(createdate,'yyyy')='{1}'", user.NewDeptCode, DateTime.Now.Year, DateTime.Now.ToString("yyyy-MM-dd 23:59:59"), "厂级")).ToInt();
                yValues.Add(count);
            }
            else
            {
                //隐患总数
                count = BaseRepository().FindObject(string.Format("select count(1) from V_BASEHIDDENINFO t where t.changedutydepartcode like '{0}%' and changedutydepartcode is not null and to_char(createdate,'yyyy')='{1}'", orgCode, DateTime.Now.Year)).ToInt();
                yValues.Add(count);
                //一般隐患数
                count = BaseRepository().FindObject(string.Format("select count(1) from V_BASEHIDDENINFO t where t.changedutydepartcode like '{0}%'  and changedutydepartcode is not null and t.rankname='一般隐患' and to_char(createdate,'yyyy')='{1}'", orgCode, DateTime.Now.Year)).ToInt();
                yValues.Add(count);
                //重大隐患数
                count = BaseRepository().FindObject(string.Format("select count(1) from V_BASEHIDDENINFO t where t.changedutydepartcode like '{0}%'  and changedutydepartcode is not null and t.rankname='重大隐患' and to_char(createdate,'yyyy')='{1}'", orgCode, DateTime.Now.Year)).ToInt();
                yValues.Add(count);
                //已整改隐患数
                count = BaseRepository().FindObject(string.Format("select count(1) from V_BASEHIDDENINFO t where t.changedutydepartcode like '{0}%'  and changedutydepartcode is not null and t.workstream<>'隐患整改' and to_char(createdate,'yyyy')='{1}'", orgCode, DateTime.Now.Year)).ToInt();
                yValues.Add(count);
                //逾期未整改隐患数
                count = BaseRepository().FindObject(string.Format("select count(1) from V_BASEHIDDENINFO t where t.changedutydepartcode like '{0}%'  and changedutydepartcode is not null and (t.changedeadine + 1) <to_date('{2}','yyyy-mm-dd hh24:mi:ss') and t.workstream='隐患整改' and to_char(createdate,'yyyy')='{1}'", orgCode, DateTime.Now.Year, DateTime.Now.ToString("yyyy-MM-dd 23:59:59"))).ToInt();
                yValues.Add(count);
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(new { xValues = xValues, yValues = yValues });
        }
        /// <summary>
        /// 按工程类型统计外包工程
        /// </summary>
        /// <param name="user">机构编码</param>
        /// <returns></returns>
        public DataTable GetProjectChart(ERCHTMS.Code.Operator user)
        {
            string orgCode = user.OrganizeCode;
            string sql = "";
            if (user.RoleName.Contains("集团") || user.RoleName.Contains("省级"))
            {
                sql = string.Format(@"select itemname as name,nvl(num,0) as num from (select a.itemname,a.itemvalue from  base_dataitemdetail a where itemid=(select itemid from base_dataitem where itemname='工程类型')) e
left join (
select engineertype,count(1) as num from epg_outsouringengineer t where t.createuserorgcode in (select encode from BASE_DEPARTMENT d where d.deptcode like '{0}%' and d.nature='{2}') group by t.engineertype) b on e.itemvalue=b.engineertype ", user.NewDeptCode, "厂级");
            }
            else
            {
                sql = string.Format(@"select itemname as name,nvl(num,0) as num from (select a.itemname,a.itemvalue from  base_dataitemdetail a where itemid=(select itemid from base_dataitem where itemname='工程类型')) e
left join (
select engineertype,count(1) as num from epg_outsouringengineer t where t.createuserorgcode='{0}' group by t.engineertype) b on e.itemvalue=b.engineertype ", orgCode);
            }
            return BaseRepository().FindTable(sql);
        }
        /// <summary>
        /// 按工程风险等级统计外包工程
        /// </summary>
        /// <param name="user">机构编码</param>
        /// <returns></returns>
        public DataTable GetProjectChartByLevel(ERCHTMS.Code.Operator user)
        {
            string orgCode = user.OrganizeCode;
            string sql = "";
            if (user.RoleName.Contains("集团") || user.RoleName.Contains("省级"))
            {
                sql = string.Format(@"select itemname as name,nvl(num,0) as num from (select a.itemname,a.itemvalue from  base_dataitemdetail a where itemid=(select itemid from base_dataitem where itemname='工程风险等级')) e
left join (
select engineerlevel,count(1) as num from epg_outsouringengineer t where t.createuserorgcode in (select encode from BASE_DEPARTMENT d where d.deptcode like '{0}%' and d.nature='{2}') group by t.engineerlevel) b on e.itemvalue=b.engineerlevel ", user.NewDeptCode, "厂级");
            }
            else
            {
                sql = string.Format(@"select itemname as name,nvl(num,0) as num from (select a.itemname,a.itemvalue from  base_dataitemdetail a where itemid=(select itemid from base_dataitem where itemname='工程风险等级')) e
left join (
select engineerlevel,count(1) as num from epg_outsouringengineer t where t.createuserorgcode='{0}' group by t.engineerlevel) b on e.itemvalue=b.engineerlevel ", orgCode);
            }
            return BaseRepository().FindTable(sql);
        }
        /// <summary>
        /// 外包人员数量变化趋势图
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GetProjectPersonChart(ERCHTMS.Code.Operator user)
        {
            string orgCode = user.OrganizeCode;
            List<string> xValues = new List<string>();
            List<int> yValues = new List<int>();
            List<int> yValuesJC = new List<int>();
            List<int> yValuesLC = new List<int>();
            string time = System.DateTime.Now.AddMonths(-12).ToString("yyyy-MM-01");
            for (int j = 1; j <= 12; j++)
            {
                int count = 0;
                string startDate = System.DateTime.Parse(time).AddMonths(j).ToString("yyyy-MM-dd 00:00:01");
                string endDate = DateTime.Parse(startDate).AddMonths(1).ToString("yyyy-MM-dd 00:00:01");
                //进场人数
                string sql = string.Format("select count(1) from base_user u where u.organizecode='{0}' and u.isepiboly=1 and u.entertime between to_date('{1}','yyyy-mm-dd hh24:mi:ss') and  to_date('{2}','yyyy-mm-dd hh24:mi:ss')", orgCode, startDate, endDate);
                count = BaseRepository().FindObject(sql).ToInt();
                yValuesJC.Add(count);
                //离场人数
                sql = string.Format("select count(1) from base_user u where u.organizecode='{0}' and u.isepiboly=1 and u.ispresence=0 and u.departuretime between to_date('{1}','yyyy-mm-dd hh24:mi:ss') and to_date('{2}','yyyy-mm-dd hh24:mi:ss')", orgCode, startDate, endDate);
                count = BaseRepository().FindObject(sql).ToInt();
                yValuesLC.Add(count);
                //在场人数
                sql = string.Format("select count(1) from base_user u where u.organizecode='{0}' and u.isepiboly=1 and (entertime<to_date('{1}','yyyy-mm-dd hh24:mi:ss') and  ((u.ispresence=0 and departuretime>to_date('{1}','yyyy-mm-dd hh24:mi:ss')) or u.ispresence=1))", orgCode, endDate);
                count = BaseRepository().FindObject(sql).ToInt();
                yValues.Add(count);
                xValues.Add(DateTime.Parse(startDate).ToString("yyyy.MM"));
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { xValues = xValues, y = yValues, yJC = yValuesJC, yLC = yValuesLC });
        }

        /// <summary>
        /// 获取日常安全检查统计数量
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GetSafetyCheckOfEveryDay(ERCHTMS.Code.Operator user)
        {
            string stDate = DateTime.Now.Year.ToString() + "-01-01";
            string enDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            string sql = "";
            if (user.RoleName.Contains("集团") || user.RoleName.Contains("省级"))
            {
                sql = string.Format(@"select count(1)  numbers  from bis_saftycheckdatarecord  where createuserorgcode  in (select encode from BASE_DEPARTMENT d where d.deptcode like '{0}%' and d.nature='{3}')  and  
             checkbegintime >= to_date('{1}','yyyy-mm-dd hh24:mi:ss') and  checkendtime <= to_date('{2}','yyyy-mm-dd hh24:mi:ss')  and checkdatatype ='1' and (datatype=0 or datatype is null)", user.NewDeptCode, stDate, enDate, "厂级");
            }
            else
            {
                sql = string.Format(@"select count(1)  numbers  from bis_saftycheckdatarecord  where createuserorgcode like '{0}%'  and  
             checkbegintime >= to_date('{1}','yyyy-mm-dd hh24:mi:ss') and  checkendtime <= to_date('{2}','yyyy-mm-dd hh24:mi:ss')  and checkdatatype ='1' ", user.OrganizeCode, stDate, enDate);
            }
            DataTable dt = BaseRepository().FindTable(sql);
            return dt.Rows[0][0].ToString();
        }
        /// <summary>
        /// 按隐患分类统计隐患
        /// </summary>
        /// <param name="user">机构编码</param>
        /// <returns></returns>
        public DataTable GetHTTypeChart(ERCHTMS.Code.Operator user)
        {
            string where = "";
            if (user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户") || user.RoleName.Contains("公司管理员"))
            {
                where = string.Format(" where hiddepart = '{0}'", user.OrganizeId);
            }
            else if (user.RoleName.Contains("集团") || user.RoleName.Contains("省级"))
            {
                where = string.Format(" where hiddepart in (select departmentid from base_department d where d.deptcode like '{0}%' and d.nature='厂级')", user.NewDeptCode);
            }
            else
            {
                where = string.Format(" where changedutydepartcode like '{0}%'", user.DeptCode);
            }
            Operator curUser = OperatorProvider.Provider.Current();
            string type = "";
            if (curUser.Industry != "电力" && !string.IsNullOrEmpty(curUser.Industry))
            {
                type = "GIHiddenType";
            }
            else
            {
                type = "HidType";
            }

            string sql = string.Format(@"select itemname as name,nvl(num,0) as value from (
                                            select a.itemname,a.itemvalue from  base_dataitemdetail a where itemid=(select itemid from base_dataitem where itemcode='{2}')
                                        ) e
                                        left join (
                                            select hidtypename,count(1) as num from v_basehiddeninfo t {0} and to_char(createdate,'yyyy')='{1}' group by t.hidtypename
                                        ) b on e.itemvalue=b.hidtypename", where, DateTime.Now.Year, type);
            return BaseRepository().FindTable(sql);
        }

        /// <summary>
        /// 隐患数量变化趋势图 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GetHTChangeChart(ERCHTMS.Code.Operator user)
        {
            List<string> xValues = new List<string>();
            List<int> yValues = new List<int>();
            List<int> yValuesYB = new List<int>();
            List<int> yValuesZD = new List<int>();
            string time = System.DateTime.Now.AddMonths(-12).ToString("yyyy-MM-01");

            string where = "";
            if (user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户") || user.RoleName.Contains("公司管理员"))
            {
                where = string.Format(" and hiddepart = '{0}'", user.OrganizeId);
            }
            else if (user.RoleName.Contains("集团") || user.RoleName.Contains("省级"))
            {
                where = string.Format(" and hiddepart in (select departmentid from base_department d where d.deptcode like '{0}%' and d.nature='厂级')", user.NewDeptCode);
            }
            else
            {
                where = string.Format(" and changedutydepartcode like '{0}%'", user.DeptCode);
            }
            for (int j = 1; j <= 12; j++)
            {
                int count = 0;
                string startDate = System.DateTime.Parse(time).AddMonths(j).ToString("yyyy-MM-dd 00:00:01");
                string endDate = DateTime.Parse(startDate).AddMonths(1).ToString("yyyy-MM-dd 00:00:01");
                //一般隐患
                string sql = string.Format("select count(1) from v_basehiddeninfo h where h.rankname='{0}'  {3} and h.createdate between to_date('{1}','yyyy-mm-dd hh24:mi:ss') and  to_date('{2}','yyyy-mm-dd hh24:mi:ss')", "一般隐患", startDate, endDate, where);
                count = BaseRepository().FindObject(sql).ToInt();
                yValuesYB.Add(count);
                //重大隐患
                sql = string.Format("select count(1) from v_basehiddeninfo h where h.rankname='{0}'  {3} and h.createdate between to_date('{1}','yyyy-mm-dd hh24:mi:ss') and to_date('{2}','yyyy-mm-dd hh24:mi:ss')", "重大隐患", startDate, endDate, where);
                int count1 = BaseRepository().FindObject(sql).ToInt();
                yValuesZD.Add(count1);
                //全部隐患
                yValues.Add(count + count1);
                xValues.Add(DateTime.Parse(startDate).ToString("yyyy.MM"));
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { xValues = xValues, y = yValues, yYB = yValuesYB, yZD = yValuesZD });
        }


        #region 获取本年度趋势图(隐患总数、安全检查总数)
        /// <summary>
        /// 获取本年度趋势图(隐患总数、安全检查总数)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GetTendencyChart(ERCHTMS.Code.Operator user)
        {
            string values = string.Empty;
            List<string> xValues = new List<string>();
            List<int> yValuesYH = new List<int>();  //隐患
            List<int> yValuesJC = new List<int>();  //检查
            string time = System.DateTime.Now.AddMonths(-12).ToString("yyyy-MM-01");
            string yhwhere = string.Empty; //隐患条件
            string jcwhere = string.Empty; //安全检查条件
            if (user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户") || user.RoleName.Contains("公司管理员"))
            {
                yhwhere = string.Format(" changedutydepartcode like '{0}%'", user.OrganizeCode);
                jcwhere = string.Format(" DataType in(0,2) and belongdept like '{0}%' and (',' || checkdeptcode like '%,{0}%' or ',' || checkdeptid like '%,{0}%'  or checkeddepartid like '%{1}%' )", user.OrganizeCode, user.OrganizeId);
            }
            else
            {
                yhwhere = string.Format(" changedutydepartcode like '{0}%'", user.DeptCode);
                jcwhere = string.Format(" DataType in(0,2) and (',' || checkdeptcode like '%,{0}%' or ',' || checkdeptid like '%,{0}%') ", user.DeptCode);
            }
            for (int j = 1; j <= 12; j++)
            {
                int yhcount = 0; //隐患总数
                int jccount = 0; //检查总数
                string startDate = System.DateTime.Parse(time).AddMonths(j).ToString("yyyy-MM-dd 00:00:00");
                string endDate = DateTime.Parse(startDate).AddMonths(1).ToString("yyyy-MM-dd 00:00:00");
                //隐患总数
                string yhsql = string.Format("select count(1) from v_basehiddeninfo h where 1=1 and {2} and h.createdate between to_date('{0}','yyyy-mm-dd hh24:mi:ss') and  to_date('{1}','yyyy-mm-dd hh24:mi:ss')", startDate, endDate, yhwhere);
                yhcount = BaseRepository().FindObject(yhsql).ToInt();
                yValuesYH.Add(yhcount);
                //检查总数
                string jcsql = string.Format(@"select count(1) from bis_saftycheckdatarecord  where 1=1 and {2}  and  
                                            checkbegintime >= to_date('{0}','yyyy-mm-dd hh24:mi:ss') and  checkendtime < to_date('{1}','yyyy-mm-dd hh24:mi:ss') ", startDate, endDate, jcwhere);
                jccount = BaseRepository().FindObject(jcsql).ToInt();
                yValuesJC.Add(jccount);
                xValues.Add(DateTime.Parse(startDate).ToString("yyyy.MM"));
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { xValues = xValues, yYH = yValuesYH, yJC = yValuesJC });
        }
        #endregion

        /// <summary>
        /// 获取通知公告
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable GetNotices(ERCHTMS.Code.Operator user)
        {
            string strWhere = "";
            if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级"))
            {
                strWhere += " and  t.createuserorgcode='" + user.OrganizeCode + "'";
            }
            else
            {
                strWhere += string.Format(@" and ((t.createuserdeptcode in
       (select encode
            from base_department
           where encode like '{0}%'
          union
          select b.encode
            from epg_outsouringengineer a
            left join base_department b
              on a.outprojectid = b.departmentid
           where a.engineerletdeptid =
                 '{1}')) or
       (d.userid = '{2}'))
   and t.id not in
       (select id
          from bis_announcement
         where issend = '1'
           and createuserid != '{2}')", user.DeptCode, user.DeptId, user.UserId);
            }
            //            string sql = string.Format(@"select * from (select id,b.title,to_char(b.releasetime,'yyyy-mm-dd') time,b.issend 
            //from bis_announcement b where id not in(select id from bis_announcement where issend='1' and  createuserid!='{0}') {1} order by createdate desc) where rownum<6", user.UserId, strWhere);

            string sql = string.Format(@"select * from(select t.id,
       t.title,
       to_char(t.releasetime,'yyyy-mm-dd') time,
       t.issend,t.isimportant,t.publisherdept,t.notictype
  from bis_announcement t
  left join (select count(id) undonenum, u.auuounid
               from bis_announdetail u
              where u.status = 0
              group by u.auuounid) p
    on p.auuounid = t.id
  left join bis_announdetail d
    on d.auuounid = t.id
   and d.userid = '{0}'where issend = '0' {1} order by t.createdate desc) where rownum<6 ", user.UserId, strWhere);
            return BaseRepository().FindTable(sql);
        }


        /// <summary>
        /// 获取一号岗大屏标题
        /// </summary>        
        /// <returns></returns>
        public DataTable GetScreenTitle()
        {
            string sql = @"select t.id,
                            t.title,
                            to_char(t.releasetime, 'yyyy-mm-dd') time,
                            t.issend,t.isimportant,t.publisherdept,t.notictype,t.createuserdeptcode
                            from bis_announcement t WHERE t.NOTICTYPE = '其他公告' AND ISSEND = '0' AND t.createuserdeptcode ='00001001001001001002'  ORDER BY CREATEDATE DESC";
            return BaseRepository().FindTable(sql);
        }


        /// <summary>
        /// 获取安全会议
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable GetMeets(ERCHTMS.Code.Operator user)
        {
            string sql = string.Format("select *from (select id,b.conferencename CONTENT,to_char(b.conferencetime,'yyyy-mm-dd') time from BIS_CONFERENCE b where ISSEND=0 and (instr(userid,'{0}')>0 or createuserid='{0}') order by conferencetime desc) where rownum<6", user.UserId);
            return BaseRepository().FindTable(sql);
        }
        /// <summary>
        /// 获取安全动态
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable GetTrends(ERCHTMS.Code.Operator user)
        {
            string sql = string.Format("select *from (select id,e.title,to_char(releasetime,'yyyy-mm-dd') time,f.filepath from BIS_SECURITYDYNAMICS e left join base_fileinfo f on e.id=f.recid where ISSEND=0 and ISOVER=1 and CREATEUSERORGCODE='{0}' order by releasetime desc ) where rownum<6", user.OrganizeCode);
            return BaseRepository().FindTable(sql);
        }
        /// <summary>
        /// 获取红黑榜
        /// </summary>
        /// <param name="user"></param>
        ///  <param name="mode">0:红榜，1:黑榜</param>
        /// <returns></returns>
        public DataTable GetRedBlack(ERCHTMS.Code.Operator user, int mode)
        {
            string sql = string.Format("select *from (select id,e.title,to_char(releasetime,'yyyy-mm-dd') time,f.filepath from bis_securityredlist e left join v_imageview f on e.id=f.recid where issend=0 and state={0} and publisherdeptcode like '{1}%' order by releasetime desc) where rownum<6", mode, user.OrganizeCode);
            return BaseRepository().FindTable(sql);
        }
        /// <summary>
        /// 外包工程概况统计
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GetProjectStat(ERCHTMS.Code.Operator user)
        {
            string sql = "";
            if (user.RoleName.Contains("厂级部门用户") || user.RoleName.Contains("公司级用户") || user.RoleName.Contains("公司领导") || user.RoleName.Contains("公司管理员"))
            {
                sql = string.Format("select d.departmentid,d.encode,d.fullname  from base_department d where parentid=(select departmentid from base_department where description='外包工程承包商' and organizeid='{0}')", user.OrganizeId);
            }
            else
            {
                sql = string.Format("select d.departmentid,d.encode,d.fullname from base_department d where d.departmentid in (select e.outprojectid from epg_outsouringengineer e where e.engineerletdeptid='{0}' and isdeptadd=1)", user.DeptId);
            }
            List<string[]> list = new List<string[]>();
            DataTable dtDepts = BaseRepository().FindTable(sql);
            foreach (DataRow dr in dtDepts.Rows)
            {
                string[] arr = new string[6];
                arr[0] = dr[2].ToString();
                sql = string.Format("select count(1) from EPG_OUTSOURINGENGINEER t where t.outprojectid='{0}' and t.engineerstate='{1}' and t.isdeptadd=1", dr[0].ToString(), "002");
                string count = BaseRepository().FindObject(sql).ToString();
                arr[1] = count;
                sql = string.Format("select count(1) from base_user u where u.departmentid='{0}' and u.ispresence=1", dr[0].ToString());//在场人员
                count = BaseRepository().FindObject(sql).ToString();
                arr[2] = count;
                sql = string.Format("select count(1) from base_user u where u.departmentid='{0}' and u.ispresence=1 and u.isspecial='是'", dr[0].ToString());//特种作业人员
                count = BaseRepository().FindObject(sql).ToString();
                arr[3] = count;
                sql = string.Format("select count(1) from base_user u where u.departmentid='{0}' and u.ispresence=1 and u.isspecialequ='是'", dr[0].ToString());//特种设备作业人员
                count = BaseRepository().FindObject(sql).ToString();
                arr[4] = count;
                sql = string.Format("select count(1) from BIS_SPECIALEQUIPMENT t where t.epibolydeptid='{0}' ", dr[0].ToString());
                count = BaseRepository().FindObject(sql).ToString();
                arr[5] = count;
                list.Add(arr);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(list);
        }
        /// <summary>
        /// 获取人员违章信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="mode">数据来源（0：个人违章，1：个人登记）</param>
        /// <returns></returns>
        public DataTable GetWZInfo(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;

            pagination.p_kid = "id";

            pagination.conditionJson = " 1=1";

            var queryParam = queryJson.ToJObject();

            int mode = !queryParam["mode"].IsEmpty() ? int.Parse(queryParam["mode"].ToString()) : 0;

            string userId = !queryParam["userId"].IsEmpty() ? queryParam["userId"].ToString() : string.Empty;
            pagination.p_fields = @"actiontype,addtype,flowstate,lllegalnumber,createdate,lllegaltime,lllegaltype, lllegaltypename ,lllegallevel,lllegallevelname,
                                        lllegaladdress,lllegaldescribe,lllegalteam,reformmeasure,reformrequire,lllegalpersonid,createuserid,khnum,jlnum";

            pagination.p_tablename = string.Format(@" (
                                   with basewz as (
                                        select a.id,a.addtype,a.flowstate,a.lllegalnumber,a.createdate,a.lllegaltime,a.lllegaltype,c.itemname lllegaltypename ,a.lllegallevel,d.itemname lllegallevelname,
                                        a.lllegaladdress,a.lllegaldescribe,a.lllegalteam,b.reformmeasure,a.reformrequire,a.lllegalpersonid,a.createuserid,nvl(e.khnum,0) khnum ,nvl(f.jlnum,0) jlnum  from bis_lllegalregister a
                                        left join v_lllegalreforminfo b on a.id =b.lllegalid 
                                        left join base_dataitemdetail  c on a.lllegaltype = c.itemdetailid
                                        left join base_dataitemdetail  d on a.lllegallevel =d.itemdetailid  
                                        left join (select count(1) khnum,lllegalid from bis_lllegalpunish group by lllegalid ) e on a.id =e.lllegalid
                                        left join (select count(1) jlnum,lllegalid from bis_lllegalawarddetail group by lllegalid ) f on a.id =f.lllegalid 
                                        where a.flowstate in (select itemname from v_yesqrwzstatus) 
                                      )
                                  select (case when lllegalpersonid ='{0}' then '本人违章' when  createuserid='{0}' then '本人登记' else '' end) actiontype,a.id,a.addtype,a.flowstate,a.lllegalnumber,a.createdate,a.lllegaltime,a.lllegaltype,a.lllegaltypename,a.lllegallevel,a.lllegallevelname,a.lllegaladdress,a.lllegaldescribe,a.lllegalteam,a.reformmeasure,a.reformrequire,a.lllegalpersonid,a.createuserid,a.khnum,a.jlnum from basewz a  where lllegalpersonid='{0}' or createuserid ='{0}' order by lllegaltime desc ) a", userId);

            var dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);

            return dt;
        }

        public DataTable GetWZInfo(string userId, int mode=0)
        {
            string sql = string.Format(@"select id,addtype,flowstate,LLLEGALNUMBER,CREATEDATE,to_char(lllegaltime,'yyyy-mm-dd') lllegaltime,LLLEGALTYPE,LLLEGALLEVEL,LLLEGALADDRESS,lllegaldescribe,LLLEGALTEAM,REFORMMEASURE,REFORMREQUIRE  from ", userId);
            if (mode == 1)
            {
                sql += string.Format(@" V_LLLEGALALLBASEINFO a  where createuserid='{0}' order by lllegaltime desc", userId);
            }
            if (mode == 0)
            {
                sql += string.Format(@" v_lllegalbaseinfo a  where lllegalpersonid='{0}' order by lllegaltime desc", userId);
            }
            if (mode == 2)
            {
                sql = string.Format(@"select max(addtype) as addtype,max(flowstate) as flowstate,max(a.id) as id,LLLEGALNUMBER,to_char(lllegaltime,'yyyy-mm-dd') lllegaltime,LLLEGALTYPE,LLLEGALLEVEL,LLLEGALADDRESS,max(f.filepath) filepath,lllegaldescribe,LLLEGALTEAM  from v_lllegalbaseinfo a left join base_fileinfo f on a.LLLEGALPIC=f.recid where lllegalpersonid='{0}'  group by  LLLEGALNUMBER,lllegaltime,LLLEGALTYPE,LLLEGALLEVEL,LLLEGALADDRESS,lllegaldescribe,LLLEGALTEAM order by lllegaltime desc", userId);
            }
            return BaseRepository().FindTable(sql);
        }

        public DataTable GetWZInfoByUserId(string userId, int mode = 0)
        {
            DataTable dt = BaseRepository().FindTable(string.Format("select itemvalue from base_dataitemdetail t where t.itemname='imgUrl'"));
            string path = dt.Rows.Count == 0 ? "" : dt.Rows[0][0].ToString();
            string sql = string.Empty;
            sql = string.Format(@" with basewz as (
                              select a.id,a.addtype,a.flowstate,a.lllegalnumber,to_char(a.createdate,'yyyy-MM-dd') createdate ,to_char(a.lllegaltime,'yyyy-MM-dd') lllegaltime,a.lllegaltype,c.itemname lllegaltypename ,a.lllegallevel,d.itemname lllegallevelname,
                              a.lllegaladdress,a.lllegaldescribe,a.lllegalteam,b.reformmeasure,a.reformrequire,a.lllegalpersonid,a.createuserid,nvl(e.EconomicsPunish,0) EconomicsPunish,
                              nvl(e.LllegalPoint,0) LllegalPoint,nvl(e.education,0) education,nvl(e.awaitjob,0) awaitjob,nvl(e.performancepoint,0) performancepoint,nvl(f.points,0) points,nvl(f.money,0) money,a.lllegalpic  from bis_lllegalregister a
                              left join v_lllegalreforminfo b on a.id =b.lllegalid 
                              left join base_dataitemdetail  c on a.lllegaltype = c.itemdetailid
                              left join base_dataitemdetail  d on a.lllegallevel =d.itemdetailid 
                              left join (select sum(EconomicsPunish) EconomicsPunish,sum(LllegalPoint) LllegalPoint,sum(education) education,sum(awaitjob) awaitjob,sum(performancepoint) performancepoint,lllegalid from bis_lllegalpunish  where personinchargeid='{0}' group by lllegalid ) e on a.id =e.lllegalid
                              left join (select sum(points) points,sum(money) money,lllegalid from bis_lllegalawarddetail where userid='{0}'  group by lllegalid ) f on a.id =f.lllegalid 
                              where a.flowstate in (select itemname from v_yesqrwzstatus) 
                            )
                        select (case when a.lllegalpersonid ='{0}' then '本人违章' when  a.createuserid='{0}' then '本人登记' else '' end) actiontype,
                        (case when a.lllegalpersonid ='{0}' then a.lllegaltime when  a.createuserid='{0}' then a.createdate else '' end) lllegaltime,a.EconomicsPunish,a.LllegalPoint,a.education,a.awaitjob,a.performancepoint,a.money,a.points ,'无' content,
                        (case when b.filepath is null then '' else ('{1}' || substr(filepath,2)) end) picture,a.id,a.lllegaldescribe from basewz a  left join v_imageview b on a.lllegalpic = b.recid
                        where a.lllegalpersonid='{0}' or a.createuserid ='{0}' order by lllegaltime desc", userId, path);

            var resultdt = BaseRepository().FindTable(sql);
            foreach (DataRow row in resultdt.Rows)
            {
                string content = string.Empty;
                if (Convert.ToDecimal(row["money"].ToString()) > 0)
                {
                    content += string.Format(@" 奖励金额{0}元,", row["money"].ToString());
                }
                if (Convert.ToDecimal(row["points"].ToString()) > 0)
                {
                    content += string.Format(@" 奖励积分{0}分,", row["points"].ToString());
                }
                if (Convert.ToDecimal(row["economicspunish"].ToString()) > 0)
                {
                    content += string.Format(@" 经济处罚{0}元,", row["economicspunish"].ToString());
                }
                if (Convert.ToDecimal(row["lllegalpoint"].ToString()) > 0)
                {
                    content += string.Format(@" 违章扣{0}分,", row["lllegalpoint"].ToString());
                }
                if (Convert.ToDecimal(row["education"].ToString()) > 0)
                {
                    content += string.Format(@" 教育培训{0}学时,", row["education"].ToString());
                }
                if (Convert.ToDecimal(row["awaitjob"].ToString()) > 0)
                {
                    content += string.Format(@" 待岗{0}月,", row["awaitjob"].ToString());
                }
                if (Convert.ToDecimal(row["performancepoint"].ToString()) > 0)
                {
                    content += string.Format(@" EHS绩效考核{0}分,", row["performancepoint"].ToString());
                }
                if (!string.IsNullOrEmpty(content))
                {
                    content = content.Substring(0, content.Length - 1) + ".";
                    row["content"] = content;
                }
       
            }
            resultdt.Columns.Remove("money");
            resultdt.Columns.Remove("points");
            resultdt.Columns.Remove("economicspunish");
            resultdt.Columns.Remove("lllegalpoint");
            resultdt.Columns.Remove("education");
            resultdt.Columns.Remove("awaitjob");
            resultdt.Columns.Remove("performancepoint");

            return resultdt;
        }
        /// <summary>
        /// 获取未签到的会议数量
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetMeetNum(string userId)
        {
            string sql = string.Format("select count(1) from Bis_Conferenceuser t left join bis_conference e on t.conferenceid=e.id where e.issend='0' and t.userid='{0}' and t.issign='1' and (t.reviewstate='0' or t.reviewstate='3' or t.reviewstate='1')", userId);
            return BaseRepository().FindObject(sql).ToInt();
        }
        /// <summary>
        /// 获取施工中危大工程数
        /// <param name="user"></param>
        /// </summary>
        /// <returns></returns>
        public int GetProjectNum(ERCHTMS.Code.Operator user)
        {
            string sql = "";
            if (user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户"))
            {
                sql = string.Format("select count(1) from BIS_PERILENGINEERING t where t.evolvecase='正在施工' and createuserorgcode='{0}'", user.OrganizeCode);
            }
            else
            {
                sql = string.Format("select count(1) from BIS_PERILENGINEERING t where t.evolvecase='正在施工' and belongdeptid='{0}'", user.DeptId);
            }
            return BaseRepository().FindObject(sql).ToInt();
        }
        /// <summary>
        /// 获取隐患数量(依次为总数量，重大隐患数量，一般隐患数量,整改延期的隐患数,逾期未整改隐患数,已整改隐患，未整改隐患)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetHtNum(ERCHTMS.Code.Operator user)
        {
            List<int> list = new List<int>();
            string sql = "";
            string sql1 = "";
            if (user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户"))
            {
                sql = string.Format("select count(1) from V_BASEHIDDENINFO t where t.CHANGEDUTYDEPARTCODE like '{0}%'", user.OrganizeCode);
                sql1 = string.Format(" select  count(1) from v_basehiddeninfo  where id in  (select distinct hidid from BIS_HTEXTENSION t where t.handledeptcode like '{0}%')", user.OrganizeCode);
            }
            else if (user.RoleName.Contains("集团") || user.RoleName.Contains("省级"))
            {
                sql = string.Format("select count(1) from V_BASEHIDDENINFO t where t.createuserorgcode in (select encode from BASE_DEPARTMENT d where d.deptcode like '{0}%' and d.nature='{1}')", user.NewDeptCode, "厂级");
                sql1 = string.Format("select  count(1) from v_basehiddeninfo  where id in  (select distinct hidid from BIS_HTEXTENSION t where t.createuserorgcode in (select encode from BASE_DEPARTMENT d where d.deptcode like '{0}%' and d.nature='{1}'))", user.NewDeptCode, "厂级");
            }
            else
            {
                sql = string.Format("select count(1) from V_BASEHIDDENINFO t where t.CHANGEDUTYDEPARTCODE like '{0}%'", user.DeptCode);
                sql1 = string.Format(" select  count(1) from v_basehiddeninfo  where id in  (select distinct hidid from BIS_HTEXTENSION t where t.handledeptcode like '{0}%')", user.DeptCode);
            }
            int count = BaseRepository().FindObject(sql).ToInt();//隐患总数
            list.Add(count);

            //重大隐患数
            count = BaseRepository().FindObject(string.Format(sql + " and t.rankname='{0}'", "重大隐患")).ToInt();
            list.Add(count);

            //一般隐患数
            count = BaseRepository().FindObject(string.Format(sql + " and t.rankname='{0}'", "一般隐患")).ToInt();
            list.Add(count);

            //整改延期的隐患数量
            count = BaseRepository().FindObject(sql1).ToInt();
            list.Add(count);

            //逾期未整改隐患数
            count = BaseRepository().FindObject(string.Format(sql + " and (t.changedeadine+1) <to_date('{0}','yyyy-mm-dd hh24:mi:ss') and t.workstream='隐患整改'", DateTime.Now.ToString("yyyy-MM-dd 23:59:59"))).ToInt();
            list.Add(count);

            //已整改隐患数
            count = BaseRepository().FindObject(string.Format("select count(1) from V_BASEHIDDENINFO t where t.createuserdeptcode like '{0}%' and t.workstream<>'隐患整改'", user.DeptCode)).ToInt();
            list.Add(count);
            //未整改隐患数
            count = BaseRepository().FindObject(string.Format("select count(1) from V_BASEHIDDENINFO t where t.createuserdeptcode like '{0}%' and t.workstream='隐患整改'", user.DeptCode)).ToInt();
            list.Add(count);

            return list;
        }
        /// <summary>
        /// 获取重大风险数量(依次为总数量，重大风险数量，较大风险数量，一般风险数量，低风险数量)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetRiskNum(ERCHTMS.Code.Operator user)
        {
            List<int> list = new List<int>();
            string sql = "";
            if (user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户"))
            {
                sql = string.Format("select count(1) from BIS_RISKASSESS t where status=1 and deletemark=0 and enabledmark=0  and t.deptcode like '{0}%' ", user.OrganizeCode);
            }
            else if (user.RoleName.Contains("集团") || user.RoleName.Contains("省级"))
            {
                sql = string.Format("select count(1) from BIS_RISKASSESS t where status=1 and deletemark=0 and enabledmark=0  and t.createuserorgcode in (select encode from BASE_DEPARTMENT d where d.deptcode like '{0}%' and d.nature='{1}')", user.NewDeptCode, "厂级");
            }
            else
            {
                sql = string.Format("select count(1) from BIS_RISKASSESS t where status=1 and deletemark=0 and enabledmark=0  and t.deptcode like '{0}%'", user.DeptCode);
            }
            int count = BaseRepository().FindObject(sql).ToInt();
            list.Add(count);
            count = BaseRepository().FindObject(sql + " and grade='重大风险'").ToInt();
            list.Add(count);
            count = BaseRepository().FindObject(sql + " and grade='较大风险'").ToInt();
            list.Add(count);
            count = BaseRepository().FindObject(sql + " and grade='一般风险'").ToInt();
            list.Add(count);
            count = BaseRepository().FindObject(sql + " and grade='低风险'").ToInt();
            list.Add(count);
            return list;
        }
        /// <summary>
        /// 获取高风险作业(依次为高风险通用待确认作业数量，高风险通用待审核(批)作业数量,旁站监督任务待分配的数量,高风险作业总数）
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetWorkNum(ERCHTMS.Code.Operator user)
        {
            List<int> list = new List<int>();
            string isJDZ = new DataItemDetailService().GetItemValue("景德镇版本");
            string isHDGZ = new DataItemDetailService().GetItemValue("贵州毕节版本");
            string isAllDataRange = new DataItemDetailService().GetEnableItemValue("HighRiskWorkDataRange");
            string sqlstr = "select count(1) from bis_highriskcommonapply where 1=1";
            string sqlstr1 = "select a.workdepttype,a.workdeptid,a.engineeringid,a.createuserdeptcode,a.flowid,a.id,a.nextstepapproveuseraccount,a.specialtytype,a.approvedeptid,'' as approveacount,e.outtransferuseraccount,e.intransferuseraccount from bis_highriskcommonapply a left join (select recid,flowid,outtransferuseraccount,intransferuseraccount,row_number()  over(partition by recid,flowid order by createdate desc) as num from BIS_TRANSFERRECORD where disable=0) e on a.id=e.recid and a.flowid=e.flowid and e.num=1 where 1=1";
            string sqlstrWhere = "";
            if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级") || user.RoleName.Contains("值班管理员") || !string.IsNullOrEmpty(isHDGZ) || !string.IsNullOrWhiteSpace(isAllDataRange) || !string.IsNullOrEmpty(isJDZ))
            {
                sqlstrWhere += string.Format(" and createuserorgcode='{0}'", user.OrganizeCode);
            }
            else if (user.RoleName.Contains("集团") || user.RoleName.Contains("省级"))
            {
                sqlstrWhere += string.Format(" and createuserorgcode in (select encode from BASE_DEPARTMENT d where d.deptcode like '{0}%' and d.nature='{1}')", user.NewDeptCode, "厂级");
            }
            else
            {
                sqlstrWhere += string.Format("  and ((workdeptcode in(select encode from base_department where encode like '{0}%'))  or (engineeringid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", user.DeptCode, user.DeptId);
            }

            string strWhere = string.Empty;
            string strCondition = " and applystate='1'";
            DataTable data = BaseRepository().FindTable(sqlstr1 + sqlstrWhere + strCondition);
            for (int i = 0; i < data.Rows.Count; i++)
            {
                string executedept = string.Empty;
                highriskcommonapplyservice.GetExecutedept(data.Rows[i]["workdepttype"].ToString(), data.Rows[i]["workdeptid"].ToString(), data.Rows[i]["engineeringid"].ToString(), out executedept);
                string createdetpid = departmentservice.GetEntityByCode(data.Rows[i]["createuserdeptcode"].ToString()).IsEmpty() ? "" : departmentservice.GetEntityByCode(data.Rows[i]["createuserdeptcode"].ToString()).DepartmentId;
                string outsouringengineerdept = string.Empty;
                highriskcommonapplyservice.GetOutsouringengineerDept(data.Rows[i]["workdeptid"].ToString(), out outsouringengineerdept);
                string str = manypowercheckservice.GetApproveUserAccount(data.Rows[i]["flowid"].ToString(), data.Rows[i]["id"].ToString(), data.Rows[i]["nextstepapproveuseraccount"].ToString(), data.Rows[i]["specialtytype"].ToString(), executedept, outsouringengineerdept, createdetpid, "", data.Rows[i]["approvedeptid"].ToString());
                data.Rows[i]["approveacount"] = str;
            }
            string[] applyids = data.Select("(outtransferuseraccount is null or outtransferuseraccount not like '%" + user.Account + ",%') and (approveacount like '%" + user.Account + ",%' or intransferuseraccount like '%" + user.Account + ",%')").AsEnumerable().Select(d => d.Field<string>("id")).ToArray();

            strCondition += string.Format(" and id in ('{0}')", string.Join("','", applyids));

            //strWhere += string.Format(" and approveaccount like '%{0}%' {1} ", user.Account + ",", strCondition);

            int count = BaseRepository().FindObject(sqlstr + sqlstrWhere + strCondition).ToInt();
            list.Add(count);//1.高风险通用待确认作业数量

            strWhere = string.Empty;
            strCondition = string.Empty;
            strCondition = " and applystate='3'";
            data = BaseRepository().FindTable(sqlstr1 + sqlstrWhere + strCondition);
            for (int i = 0; i < data.Rows.Count; i++)
            {
                string executedept = string.Empty;
                highriskcommonapplyservice.GetExecutedept(data.Rows[i]["workdepttype"].ToString(), data.Rows[i]["workdeptid"].ToString(), data.Rows[i]["engineeringid"].ToString(), out executedept);
                string createdetpid = departmentservice.GetEntityByCode(data.Rows[i]["createuserdeptcode"].ToString()).IsEmpty() ? "" : departmentservice.GetEntityByCode(data.Rows[i]["createuserdeptcode"].ToString()).DepartmentId;
                string outsouringengineerdept = string.Empty;
                highriskcommonapplyservice.GetOutsouringengineerDept(data.Rows[i]["workdeptid"].ToString(), out outsouringengineerdept);
                string str = manypowercheckservice.GetApproveUserAccount(data.Rows[i]["flowid"].ToString(), data.Rows[i]["id"].ToString(), data.Rows[i]["nextstepapproveuseraccount"].ToString(), data.Rows[i]["specialtytype"].ToString(), executedept, outsouringengineerdept, createdetpid, "", data.Rows[i]["approvedeptid"].ToString());
                data.Rows[i]["approveacount"] = str;
            }
            applyids = data.Select("(outtransferuseraccount is null or outtransferuseraccount not like '%" + user.Account + ",%') and (approveacount like '%" + user.Account + ",%' or intransferuseraccount like '%" + user.Account + ",%')").AsEnumerable().Select(d => d.Field<string>("id")).ToArray();
            strCondition += string.Format(" and id in ('{0}')", string.Join("','", applyids));
            //strWhere += string.Format(" and approveaccount like '%{0}%' {1} ", user.Account + ",", strCondition);
            count = BaseRepository().FindObject(sqlstr + sqlstrWhere + strCondition).ToInt();
            list.Add(count);//2.高风险通用待审核(批)作业数量

            string sql2 = string.Format("select count(1) from bis_taskshare a where  a.id  not in(select id from bis_taskshare where issubmit='0' and  createuserid!='{0}' and flowdept is null)", user.UserId);
            if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级"))
            {
                sql2 += " and a.createuserorgcode='" + user.OrganizeCode + "'";
            }
            else
            {
                sql2 += string.Format(@" and ((a.supervisedeptcode like  '{0}%' ) or (a.id in(select distinct(taskshareid) from  (select taskshareid from bis_teamsinfo where teamcode like '{0}%' union select taskshareid from bis_staffinfo where pteamcode like '{0}%'))))", user.DeptCode);
            }
            string roleWhere = "";
            string[] roles = user.RoleName.Split(',');
            foreach (var r in roles)
            {
                roleWhere += string.Format("or a.flowrolename like '%{0}%'", r);
            }
            roleWhere = roleWhere.Substring(2);
            //当前有审核权限的部门及角色，才可查看
            sql2 += string.Format("  and a.flowdept like '%{0}%' and ({1})", user.DeptId, roleWhere);
            //sql2 += string.Format("  and ((a.flowdept like '%{0}%' and ({1}))", user.DeptId, roleWhere);
            //sql2 += string.Format("  or (issubmit='0' and  a.createuserid='{0}'))", user.UserId);
            count = BaseRepository().FindObject(sql2).ToInt();
            list.Add(count);//3.待分配的数量


            string sql3 = sqlstr + sqlstrWhere + " and applystate='5'";
            count = BaseRepository().FindObject(sql3).ToInt();//4.高风险作业总数
            list.Add(count);

            string sql4 = sqlstr + sqlstrWhere + string.Format(" and applystate='5' and WorkEndTime>to_date('{0}','yyyy-mm-dd hh24:mi:ss')", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            count = BaseRepository().FindObject(sql4).ToInt();//5.正在进行的高风险作业
            list.Add(count);

            string sql5 = string.Format("select count(1) from bis_staffinfo  where tasklevel='2'  and taskuserid='{0}' and dataissubmit='0'", user.UserId);
            count = BaseRepository().FindObject(sql5).ToInt();//6.待监督的任务
            list.Add(count);

            string sql6 = string.Format("select count(1) from bis_staffinfo a  left join bis_taskurge b on a.id=b.staffid where tasklevel='1' and a.dataissubmit='1' ");

            #region 我的监管
            string roleCondition = "";
            strWhere = string.Empty;
            if (user.RoleName.Contains("厂级"))
            {
                roleCondition = idataitemdetailservice.GetItemValue(user.OrganizeId, "urgerole");//安全主管部门监管角色
                strWhere = string.Format("  and a.id not in(select  distinct(a.staffid) from bis_taskurge a where a.dataissubmit='1') and a.createuserorgcode='{0}'", user.OrganizeCode);
            }
            else if (user.RoleName.Contains("部门级用户"))
            {
                roleCondition = idataitemdetailservice.GetItemValue(user.OrganizeId, "sidepersonurgerole");//旁站监督所在部门
                strWhere = string.Format("  and a.id not in(select  distinct(a.staffid) from bis_taskurge a where a.dataissubmit='1') and a.createuserorgcode='{0}' and pteamid in(select to_char(departmentid) from base_department t where t.organizeid='{1}' and t.encode like '{2}%' union  select  to_char(outprojectid) from epg_outsouringengineer a where a.engineerletdeptid = '{3}')", user.OrganizeCode, user.OrganizeId, user.DeptCode, user.DeptId);
                if (user.RoleName.Contains("专工") && !string.IsNullOrEmpty(user.SpecialtyType))
                {
                    string strSpecialtyType = "'" + user.SpecialtyType.Replace(",", "','") + "'";
                    strWhere += " and SpecialtyType in (" + strSpecialtyType + ")";
                }
            }
            else if (user.RoleName.Contains("承包商用户"))
            {
                roleCondition = idataitemdetailservice.GetItemValue(user.OrganizeId, "contracturgerole");//承包商级
                strWhere = string.Format("  and a.id not in(select  distinct(a.staffid) from bis_taskurge a where a.dataissubmit='1') and a.createuserorgcode='{0}' and pteamid in(select departmentid from base_department t where t.organizeid='{1}' and t.deptcode like '{2}%')", user.OrganizeCode, user.OrganizeId, user.DeptCode);
            }
            if (!string.IsNullOrEmpty(roleCondition))
            {
                var isuse = roleCondition.Split('|');
                string[] arrrolename = isuse[0].Split(',');
                if (isuse.Length >= 2 && isuse[1] == "1")
                {
                    var num = 0;
                    for (int i = 0; i < arrrolename.Length; i++)
                    {
                        if (user.RoleName.Contains(arrrolename[i]))
                            num++;
                        if (num != 0)
                            break;
                    }
                    if (num != 0)
                        sql6 += strWhere;
                    else
                        sql6 += " and 1=2";
                }
                else
                {
                    sql6 += " and 1=2";
                }
            }
            else
            {
                sql6 += " and 1=2";
            }
            #endregion
            count = BaseRepository().FindObject(sql6).ToInt();//7.待监管的任务
            list.Add(count);

            string sql7 = string.Format("select count(1) from v_underwaywork where 1=1 ");
            if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级"))
            {
                sqlstrWhere += string.Format(" and createuserorgcode='{0}'", user.OrganizeCode);
            }
            else if (user.RoleName.Contains("集团") || user.RoleName.Contains("省级"))
            {
                sqlstrWhere += string.Format(" and createuserorgcode in (select encode from BASE_DEPARTMENT d where d.deptcode like '{0}%' and d.nature='{1}')", user.NewDeptCode, "厂级");
            }
            else
            {
                sqlstrWhere += string.Format("  and ((workdeptcode in(select encode from base_department where encode like '{0}%'))  or (engineeringid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", user.DeptCode, user.DeptId);
            }
            count = BaseRepository().FindObject(sql7 + sqlstrWhere).ToInt();
            list.Add(count);//8.今日高风险作业数量

            return list;
        }


        /// <summary>
        /// 获取高危作业安全许可证审批待办（依次为高处作业、起重吊装作业、动土作业、断路作业、动火作业、盲板抽堵作业、受限空间作业、设备检修清理作业、待措施确认、待停电、待备案、待验收、待送电）
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetJobSafetyCardNum(ERCHTMS.Code.Operator user)
        {
            List<int> list = new List<int>();
            string sqlstr = @"select t.id,t1.id as flowdetailid,t.jobtype,'' as isrole from BIS_JobSafetyCardApply t left join BIS_DangerousJobFlowDetail t1
                                        on t.id=t1.businessid and t1.status=0 where t.jobstate=1";
            string where = "";
            DataTable dt = BaseRepository().FindTable(sqlstr);
            foreach (DataRow dr in dt.Rows)
            {
                string BusinessId = dr["id"].ToString();
                //获取当前用户是否有权限操作该条数据
                string approveName = "";
                string approveId = "";
                string approveAccount = "";
                dr["isrole"] = DetailService.GetCurrentStepRole(BusinessId, user.Account, dr["flowdetailid"] == null ? "" : dr["flowdetailid"].ToString(), out approveName, out approveId, out approveAccount);
            }
            where += string.Format(" and t.id in ('{0}')", string.Join("','", dt.Select("isrole ='0'").AsEnumerable().Select(d => d.Field<string>("id")).ToArray()));

            DataTable data = BaseRepository().FindTable(sqlstr + where);
            list.Add(data.Select("jobtype='HeightWorking'").Count());//高处作业
            list.Add(data.Select("jobtype='Lifting'").Count());//起重吊装作业
            list.Add(data.Select("jobtype='Digging'").Count());//动土作业
            list.Add(data.Select("jobtype='OpenCircuit'").Count());//断路作业
            list.Add(data.Select("jobtype='WhenHot'").Count());//动火作业
            list.Add(data.Select("jobtype='BlindPlateWall'").Count());//盲板抽堵作业
            list.Add(data.Select("jobtype='LimitedSpace'").Count());//受限空间作业
            list.Add(data.Select("jobtype='EquOverhaulClean'").Count());//设备检修清理作业

            sqlstr = @"select t.id from BIS_JobSafetyCardApply t where t.jobstate=3 and t.measurepersonid like '%" + user.UserId + "%'";
            list.Add(BaseRepository().FindTable(sqlstr).Rows.Count);//待措施确认

            sqlstr = @"select t.id from BIS_JobSafetyCardApply t where t.jobstate=4 and t.powercutpersonid like '%" + user.UserId + "%'";
            list.Add(BaseRepository().FindTable(sqlstr).Rows.Count);//待停电

            sqlstr = @"select t.id from BIS_JobSafetyCardApply t where t.jobstate=5 and t.recordspersonid like '%" + user.UserId + "%'";
            list.Add(BaseRepository().FindTable(sqlstr).Rows.Count);//待备案

            sqlstr = @"select t.id from BIS_JobSafetyCardApply t where t.jobstate=6 and t.checkpersonid like '%" + user.UserId + "%'";
            list.Add(BaseRepository().FindTable(sqlstr).Rows.Count);//待验收

            sqlstr = @"select t.id from BIS_JobSafetyCardApply t where t.jobstate=7 and t.powergivepersonid like '%" + user.UserId + "%'";
            list.Add(BaseRepository().FindTable(sqlstr).Rows.Count);//待送电

            return list;
        }
        /// <summary>
        /// 获取脚手架统计(待验收，待审核）
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetScaffoldNum(ERCHTMS.Code.Operator user)
        {
            List<int> list = new List<int>();

            string sql = "select count(1) from bis_scaffold where 1=1";
            string sqlstr1 = "select a.*,'' as approveuseraccount,e.outtransferuseraccount,e.intransferuseraccount from bis_scaffold a left join (select recid,flowid,outtransferuseraccount,intransferuseraccount,row_number()  over(partition by recid,flowid order by createdate desc) as num from BIS_TRANSFERRECORD where disable=0) e on a.id=e.recid and a.flowid=e.flowid and e.num=1 where 1=1";
            string sqlWhere = string.Empty;
            string isAllDataRange = new DataItemDetailService().GetEnableItemValue("HighRiskWorkDataRange");
            if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级") || !string.IsNullOrEmpty(isAllDataRange))
            {
                sql += string.Format(" and createuserorgcode='{0}'", user.OrganizeCode);
            }
            else if (user.RoleName.Contains("集团") || user.RoleName.Contains("省级"))
            {
                sql += string.Format(" and createuserorgcode in (select encode from BASE_DEPARTMENT d where d.deptcode like '{0}%' and d.nature='{1}')", user.NewDeptCode, "厂级");
            }
            else
            {
                sql += string.Format("  and ((setupcompanyid in(select departmentid from base_department where encode like '{0}%'))  or (outprojectid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", user.DeptCode, user.DeptId);
            }

            sqlWhere += " and AuditState = 4 ";


            DataTable data = BaseRepository().FindTable(sqlstr1 + sqlWhere);
            for (int i = 0; i < data.Rows.Count; i++)
            {
                string executedept = string.Empty;
                highriskcommonapplyservice.GetExecutedept(data.Rows[i]["setupcompanytype"].ToString(), data.Rows[i]["setupcompanyid"].ToString(), data.Rows[i]["outprojectid"].ToString(), out executedept);
                string createdetpid = departmentservice.GetEntityByCode(data.Rows[i]["createuserdeptcode"].ToString()).IsEmpty() ? "" : departmentservice.GetEntityByCode(data.Rows[i]["createuserdeptcode"].ToString()).DepartmentId;
                string outsouringengineerdept = string.Empty;
                highriskcommonapplyservice.GetOutsouringengineerDept(data.Rows[i]["setupcompanyid"].ToString(), out outsouringengineerdept);
                string str = manypowercheckservice.GetApproveUserAccount(data.Rows[i]["flowid"].ToString(), data.Rows[i]["id"].ToString(), "", data.Rows[i]["specialtytype"].ToString(), executedept, outsouringengineerdept, createdetpid, "", "");
                data.Rows[i]["approveuseraccount"] = str;
            }
            string[] applyids = data.Select("(outtransferuseraccount is null or outtransferuseraccount not like '%" + user.Account + ",%') and (approveuseraccount like '%" + user.Account + ",%' or intransferuseraccount like '%" + user.Account + ",%')").AsEnumerable().Select(d => d.Field<string>("id")).ToArray();

            sqlWhere += string.Format(" and id in ('{0}')", string.Join("','", applyids));

            int count = BaseRepository().FindObject(sql + sqlWhere).ToInt();
            list.Add(count);//待验收

            sqlWhere = string.Empty;
            sqlWhere += " and AuditState in (1,6)";
            data = BaseRepository().FindTable(sqlstr1 + sqlWhere);
            for (int i = 0; i < data.Rows.Count; i++)
            {
                string executedept = string.Empty;
                highriskcommonapplyservice.GetExecutedept(data.Rows[i]["setupcompanytype"].ToString(), data.Rows[i]["setupcompanyid"].ToString(), data.Rows[i]["outprojectid"].ToString(), out executedept);
                string createdetpid = departmentservice.GetEntityByCode(data.Rows[i]["createuserdeptcode"].ToString()).IsEmpty() ? "" : departmentservice.GetEntityByCode(data.Rows[i]["createuserdeptcode"].ToString()).DepartmentId;
                string outsouringengineerdept = string.Empty;
                highriskcommonapplyservice.GetOutsouringengineerDept(data.Rows[i]["setupcompanyid"].ToString(), out outsouringengineerdept);
                string str = manypowercheckservice.GetApproveUserAccount(data.Rows[i]["flowid"].ToString(), data.Rows[i]["id"].ToString(), "", data.Rows[i]["specialtytype"].ToString(), executedept, outsouringengineerdept, createdetpid, "", "");
                data.Rows[i]["approveuseraccount"] = str;
            }
            applyids = data.Select("(outtransferuseraccount is null or outtransferuseraccount not like '%" + user.Account + ",%') and (approveuseraccount like '%" + user.Account + ",%' or intransferuseraccount like '%" + user.Account + ",%')").AsEnumerable().Select(d => d.Field<string>("id")).ToArray();
            sqlWhere += string.Format(" and id in ('{0}')", string.Join("','", applyids));
            count = BaseRepository().FindObject(sql + sqlWhere).ToInt();
            list.Add(count);//待审核

            return list;
        }

        /// <summary>
        /// 获取消防水待审核(批)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetFireWaterNum(ERCHTMS.Code.Operator user)
        {
            List<int> list = new List<int>();

            string sql = "select count(1) from bis_firewater a where 1=1";
            string sqlstr1 = "select a.*,'' as approveuseraccount,e.outtransferuseraccount,e.intransferuseraccount from bis_firewater a left join (select recid,flowid,outtransferuseraccount,intransferuseraccount,row_number()  over(partition by recid,flowid order by createdate desc) as num from BIS_TRANSFERRECORD where disable=0) e on a.id=e.recid and a.flowid=e.flowid and e.num=1 where 1=1";
            string sqlWhere = string.Empty;
            var IsHrdl = new DataItemDetailService().GetItemValue("IsOpenPassword");
            string isAllDataRange = idataitemdetailservice.GetEnableItemValue("HighRiskWorkDataRange"); //特殊标记，高风险作业模块是否看全厂数据
            if (IsHrdl == "true" || !string.IsNullOrWhiteSpace(isAllDataRange))
            {
                sql += " and  createuserorgcode='" + user.OrganizeCode + "'";
            }
            else
            {
                //配置的部门拥有特殊查看权限
                string specialDeptId = new DataItemDetailService().GetItemValue(user.OrganizeId, "FireDept");
                if (!string.IsNullOrEmpty(specialDeptId) && specialDeptId.Contains(user.DeptId))
                {
                    sql += " and  createuserorgcode='" + user.OrganizeCode + "'";
                }
                else
                {
                    if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级"))
                    {
                        sql += string.Format(" and createuserorgcode='{0}'", user.OrganizeCode);
                    }
                    else if (user.RoleName.Contains("集团") || user.RoleName.Contains("省级"))
                    {
                        sql += string.Format(" and createuserorgcode in (select encode from BASE_DEPARTMENT d where d.deptcode like '{0}%' and d.nature='{1}')", user.NewDeptCode, "厂级");
                    }
                    else
                    {
                        sql += string.Format("  and ((workdeptcode in(select encode from base_department where encode like '{0}%'))  or (engineeringid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", user.DeptCode, user.DeptId);
                    }
                }
            }

            sqlWhere += " and a.applystate ='1'";
            DataTable data = BaseRepository().FindTable(sqlstr1 + sqlWhere);
            for (int i = 0; i < data.Rows.Count; i++)
            {
                string executedept = string.Empty;
                highriskcommonapplyservice.GetExecutedept(data.Rows[i]["workdepttype"].ToString(), data.Rows[i]["workdeptid"].ToString(), data.Rows[i]["engineeringid"].ToString(), out executedept);
                string createdetpid = departmentservice.GetEntityByCode(data.Rows[i]["createuserdeptcode"].ToString()).IsEmpty() ? "" : departmentservice.GetEntityByCode(data.Rows[i]["createuserdeptcode"].ToString()).DepartmentId;
                string outsouringengineerdept = string.Empty;
                highriskcommonapplyservice.GetOutsouringengineerDept(data.Rows[i]["workdeptid"].ToString(), out outsouringengineerdept);
                string str = manypowercheckservice.GetApproveUserAccount(data.Rows[i]["flowid"].ToString(), data.Rows[i]["id"].ToString(), "", data.Rows[i]["specialtytype"].ToString(), executedept, outsouringengineerdept, createdetpid, "", "");
                data.Rows[i]["approveuseraccount"] = str;
            }
            string[] applyids = data.Select("(outtransferuseraccount is null or outtransferuseraccount not like '%" + user.Account + ",%') and (approveuseraccount like '%" + user.Account + ",%' or intransferuseraccount like '%" + user.Account + ",%')").AsEnumerable().Select(d => d.Field<string>("id")).ToArray();

            sqlWhere += string.Format(" and a.id in ('{0}')", string.Join("','", applyids));
            int count = BaseRepository().FindObject(sql + sqlWhere).ToInt();
            list.Add(count);//待审核
            return list;
        }
        /// <summary>
        /// 获取检查发现的隐患
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int GetCheckHtNum(ERCHTMS.Code.Operator user)
        {
            int count = 0;
            if (user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户"))
            {
                count = BaseRepository().FindObject(string.Format("select count(1) from bis_htbaseinfo h where h.hiddepart = '{0}' and h.safetycheckobjectid is not null  and  workstream != '隐患登记' and workstream != '隐患评估' and workstream != '隐患完善' and to_char(createdate,'yyyy')='{1}'", user.OrganizeId, DateTime.Now.Year)).ToInt();
            }
            else if (user.RoleName.Contains("集团") || user.RoleName.Contains("省级"))
            {
                var dept = departmentIService.GetEntity(user.OrganizeId); //当前省级的机构对象

                count = BaseRepository().FindObject(string.Format("select count(1) from bis_htbaseinfo h where h.hiddepart in ( select departmentid from base_department where  deptcode like '{0}%'  and nature='厂级') and h.safetycheckobjectid is not null  and  workstream != '隐患登记' and workstream != '隐患评估'  and workstream != '隐患完善' and to_char(createdate,'yyyy')='{1}'", dept.DeptCode, DateTime.Now.Year)).ToInt();
            }
            else
            {
                count = BaseRepository().FindObject(string.Format("select count(1) from bis_htbaseinfo h where h.createuserdeptcode like '{0}%' and h.safetycheckobjectid is not null  and  workstream != '隐患登记' and workstream != '隐患评估'  and workstream != '隐患完善' and to_char(createdate,'yyyy')='{1}'", user.DeptCode, DateTime.Now.Year)).ToInt();
            }

            return count;
        }
        /// <summary>
        /// 高风险作业分类
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GetWorkTypeChart(ERCHTMS.Code.Operator user)
        {
            List<string> xValues = new List<string>();
            List<int> yValues = new List<int>();


            string sql = "";
            if (user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户"))
            {
                sql = string.Format(@"select itemname,nvl(num,0) as num from (select a.itemname,a.itemvalue from  base_dataitemdetail a where itemid=(select itemid from base_dataitem where itemcode='CommonType')) e
                    left join (
                    select WORKTYPE,count(1) as num from bis_highriskcommonapply t where applystate='5' and createuserorgcode='{0}' group by t.WORKTYPE) b on e.itemvalue=b.WORKTYPE", user.OrganizeCode);
            }
            else
            {
                sql = string.Format(@"select itemname,nvl(num,0) as num from (select a.itemname,a.itemvalue from  base_dataitemdetail a where itemid=(select itemid from base_dataitem where itemcode='CommonType')) e
                    left join (
                    select WORKTYPE,count(1) as num from bis_highriskcommonapply t where applystate='5'  and WorkDeptCode in(select departmentid from base_department  where encode like '{0}%' union select b.departmentid from epg_outsouringengineer  a left join base_department b on a.outprojectid=b.departmentid  where  a.engineerletdeptid='{1}') group by t.WORKTYPE) b on e.itemvalue=b.WORKTYPE", user.DeptCode, user.DeptId);
            }
            var dt = BaseRepository().FindTable(sql);
            foreach (DataRow item in dt.Rows)
            {
                yValues.Add(item["num"].ToInt());
                xValues.Add(item["itemname"].ToString());
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { xValues = xValues, yValues = yValues });
        }

        /// <summary>
        /// 获取特种设备数量
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int GetEquimentNum(ERCHTMS.Code.Operator user)
        {
            string sql = string.Format("select count(1) from BIS_SPECIALEQUIPMENT t where t.controldeptcode like '{0}%'", user.OrganizeCode);
            //if (user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户"))
            //{
            //    sql = string.Format("select count(1) from BIS_SPECIALEQUIPMENT t where t.controldeptcode like '{0}%'", user.OrganizeCode);
            //}
            //else
            //{
            //    sql = string.Format("select count(1) from BIS_SPECIALEQUIPMENT t where t.controldeptcode like '{0}%'", user.DeptCode);
            //}
            return BaseRepository().FindObject(sql).ToInt();
        }
        /// <summary>
        /// 获取违章数量信息（依次为：违章总数量、待核准、待整改、待验收、逾期未整改数量、逾期整改数量，未整改的违章数量，待完善的违章,待制定整改计划，待验收确认）
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetlllegalNum(ERCHTMS.Code.Operator user)
        {
            List<int> list = new List<int>();
            string sql = "";
            string deptcode = string.Empty;
            if (user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户"))
            {
                deptcode = user.OrganizeCode;
                sql = string.Format(@"select count(1) from v_lllegalbaseinfo  where lllegalteamcode like '{0}%'", user.OrganizeCode);
            }
            else
            {
                deptcode = user.DeptCode;
                sql = string.Format(@"select count(1) from v_lllegalbaseinfo  where lllegalteamcode like '{0}%'", user.DeptCode);
            }
            int count = BaseRepository().FindObject(sql).ToInt();
            list.Add(count);//违章总数量

            count = BaseRepository().FindObject(string.Format(" select count(1) from v_lllegalallbaseinfo where (','|| substr(participant,2) ||',')  like  '%,{0},%' and flowstate  in ('违章核准','违章审核')", user.Account)).ToInt();
            list.Add(count);//待当前用户核准的数量
            count = BaseRepository().FindObject(string.Format("select count(1) from v_lllegalallbaseinfo where  reformpeopleid like '%{0}%' and flowstate  = '违章整改'  ", user.UserId)).ToInt();
            list.Add(count);//待当前用户整改的数量
            count = BaseRepository().FindObject(string.Format("select count(1) from v_lllegalallbaseinfo where (','|| substr(participant,2) ||',')  like  '%,{0},%' and flowstate  = '违章验收'", user.Account)).ToInt();
            list.Add(count);//待当前用户验收的数量
            count = BaseRepository().FindObject(string.Format("select count(1) from v_lllegalallbaseinfo where flowstate  = '违章整改'  and to_date('{0}','yyyy-mm-dd hh24:mi:ss') > (reformdeadline +1)  and lllegalteamcode like '{1}%'", DateTime.Now.ToString("yyyy-MM-dd 23:59:59"), deptcode)).ToInt();
            list.Add(count);//逾期未整改数量

            count = BaseRepository().FindObject(string.Format(sql + " and flowstate  = '违章整改'  ")).ToInt();
            list.Add(count);//未整改的数量

            count = BaseRepository().FindObject(string.Format(" select count(1) from v_lllegalallbaseinfo where (','|| substr(participant,2) ||',')  like  '%,{0},%' and flowstate  = '违章完善'", user.Account)).ToInt();
            list.Add(count);//待完善的违章

            count = BaseRepository().FindObject(string.Format(" select count(1) from v_lllegalallbaseinfo where  (','|| reformchargeperson ||',') like  '%,{0},%' and flowstate  = '制定整改计划'", user.Account)).ToInt();
            list.Add(count);//待制定整改计划的违章

            count = BaseRepository().FindObject(string.Format(" select count(1) from v_lllegalallbaseinfo where (','|| substr(participant,2) ||',')  like  '%,{0},%' and flowstate  = '验收确认'", user.Account)).ToInt();
            list.Add(count);//待验收确认的违章

            count = BaseRepository().FindObject(string.Format(" select count(1) from v_lllegalallbaseinfo where  (applicationstatus ='1' and postponeperson  like  '%,{0},%')", user.Account)).ToInt();
            list.Add(count);//待整改延期申请审批

            //可门专属  违章整改确认
            count = BaseRepository().FindObject(string.Format(" select count(1) from v_lllegalallbaseinfo where  reformpeopleid not like '%{0}%' and reformdeptcode ='{1}' and flowstate  = '违章整改'", user.UserId, user.DeptCode)).ToInt();
            list.Add(count);//违章整改确认

            //违章档案扣分待完善
            count = 0;
            if (user.RoleName.Contains("负责人") || user.RoleName.Contains("副管用户") || user.RoleName.Contains("安全管理员"))
            {
                count = BaseRepository().FindObject(string.Format(" select count(1) from v_lllegalrecordinfo  where deptid='{0}' and to_number(appsign) > 0 and userid is null", user.DeptId)).ToInt();
            }
            list.Add(count);//违章档案扣分待完善
            return list;
        }
        /// <summary>
        /// 获取应急数量
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetDrillRecordNum(ERCHTMS.Code.Operator user)
        {
            List<int> list = new List<int>();
            string sql = "";
            string deptcode = string.Empty;
            if (user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户"))
            {
                deptcode = user.OrganizeCode;
            }
            else
            {
                deptcode = user.DeptCode;
            }
            //创建人及计划执行人都可以完善
            sql = string.Format(@"select count(1) from (select t.*,b.executepersonid,b.executepersonname  from mae_drillplanrecord t 
                                                        left join mae_drillplan  b  on t.drillplanid =b.id ) a  where createuserdeptcode like '{0}%'  ", deptcode);
            //待办应急演练记录
            sql += string.Format(@" and (createuserid ='{0}' or executepersonid ='{0}') and iscommit ='0'  ", user.UserId);

            int count = BaseRepository().FindObject(sql).ToInt();

            list.Add(count);//待办应急演练记录数量

            return list;
        }

        #region 获取问题数量信息
        /// <summary>
        /// 获取问题数量信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetQuestionNum(ERCHTMS.Code.Operator user)
        {
            List<int> list = new List<int>();
            int count = 0;

            count = BaseRepository().FindObject(string.Format(" select count(1) from v_questioninfo where  (reformpeople||',') like '%{0}%' and flowstate = '问题整改'", user.Account + ",")).ToInt();
            list.Add(count);//待整改的问题

            count = BaseRepository().FindObject(string.Format(" select count(1) from v_questioninfo where  actionperson like '%{0}%' and flowstate = '问题验证'", "," + user.Account + ",")).ToInt();
            list.Add(count);//待验证的问题

            count = BaseRepository().FindObject(string.Format(@"  select count(1) from  ( select a.* ,(case when a.flowstate ='开始' then '问题登记' when a.flowstate ='评估' then '问题评估' else '已处理' end ) flowdescribe,b.actionperson,b.participantname
from bis_findquestioninfo a  left join v_findquestionworkflow  b on a.id =b.id ) a  where actionperson like '%{0}%' and flowstate = '评估'", "," + user.Account + ",")).ToInt();
            list.Add(count);//待评估的发现问题
            return list;
        }
        #endregion

        /// <summary>
        /// 获取违章整改率
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public decimal GetlllegalRatio(ERCHTMS.Code.Operator user)
        {
            string sql = "";
            if (user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户"))
            {
                sql = string.Format("select count(1) from V_LLLEGALBASEINFO t where t.LLLEGALTEAMCODE like '{0}%'", user.OrganizeCode);
            }
            else
            {
                sql = string.Format("select count(1) from V_LLLEGALBASEINFO t where t.LLLEGALTEAMCODE like '{0}%'", user.DeptCode);
            }
            decimal sum = BaseRepository().FindObject(sql).ToDecimal();
            decimal count = BaseRepository().FindObject(sql + " and flowstate in('流程结束','违章验收')").ToDecimal();
            return sum == 0 ? 0 : Math.Round(count / sum, 4) * 100;
        }
        /// <summary>
        /// 按风险等级绘图
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GetRiskCounChart(ERCHTMS.Code.Operator user)
        {
            List<object> list = new List<object>();

            string sql = "";
            if (user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户"))
            {
                sql = string.Format("select grade,count(1) from BIS_RISKASSESS where status=1 and deptcode like '{0}%' and  deletemark=0 and enabledmark=0 and districtid is not null", user.OrganizeCode);
            }
            else if (user.RoleName.Contains("集团") || user.RoleName.Contains("省级"))
            {
                sql = string.Format(" select grade,count(1) from BIS_RISKASSESS where status=1 and deletemark=0 and enabledmark=0 and districtid is not null and createuserorgcode in (select encode from BASE_DEPARTMENT d where d.deptcode like '{0}%' and d.nature='{1}')", user.NewDeptCode, "厂级");
            }
            else
            {
                sql = string.Format("select grade,count(1) from BIS_RISKASSESS where status=1 and deptcode like '{0}%' and  deletemark=0 and enabledmark=0 and districtid is not null", user.DeptCode);
            }
            sql += " group by grade";
            DataTable dt = this.BaseRepository().FindTable(sql);

            int count = dt.Select("grade='重大风险'").Length == 0 ? 0 : int.Parse(dt.Select("grade='重大风险'")[0][1].ToString());
            list.Add(new { name = "重大风险", value = count });
            count = dt.Select("grade='较大风险'").Length == 0 ? 0 : int.Parse(dt.Select("grade='较大风险'")[0][1].ToString());
            list.Add(new { name = "较大风险", value = count });
            count = dt.Select("grade='一般风险'").Length == 0 ? 0 : int.Parse(dt.Select("grade='一般风险'")[0][1].ToString());
            list.Add(new { name = "一般风险", value = count });
            count = dt.Select("grade='低风险'").Length == 0 ? 0 : int.Parse(dt.Select("grade='低风险'")[0][1].ToString());
            list.Add(new { name = "低风险", value = count });
            dt.Dispose();
            return Newtonsoft.Json.JsonConvert.SerializeObject(list);
        }
        /// <summary>
        /// 获取安全事例
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable GetWorks(ERCHTMS.Code.Operator user)
        {
            //安全会议（个人参与的会议）
            string sql = string.Format("select to_char(b.conferencetime,'yyyy-mm-dd') time from BIS_CONFERENCE b where ISSEND=0 and (instr(userid,'{0}')>0 or createuserid='{0}')", user.UserId);
            DataTable dt = BaseRepository().FindTable(sql);
            //隐患排查(个人登记的，排查的，整改及验收的)
            sql = string.Format("select to_char(t.checkdate,'yyyy-mm-dd') time from v_basehiddeninfo t where t.createuserid='{0}' or t.monitorpersonid='{0}' or t.acceptperson='{0}' or t.changeperson='{0}'", user.UserId);
            DataTable dtHt = BaseRepository().FindTable(sql);
            dt.Merge(dtHt);

            //个人登记的违章或本人违章的
            sql = string.Format("select to_char(t.LLLEGALTIME,'yyyy-mm-dd') time from bis_lllegalregister t where (t.createuserid='{0}' or t.lllegalpersonid='{0}') and flowstate !='违章登记' and flowstate !='违章举报'", user.UserId);
            dtHt = BaseRepository().FindTable(sql);
            dt.Merge(dtHt);
            return dt;
        }
        /// <summary>
        /// 根据日期获取个人安全事例记录
        /// </summary>
        /// <param name="user"></param>
        /// <param name="time">时间</param>
        /// <returns></returns>
        public DataTable GetWorkInfoByTime(ERCHTMS.Code.Operator user, string time)
        {
            //安全会议（个人参与的会议）
            string sql = string.Format("select id,conferencename title,1 type  from BIS_CONFERENCE b where ISSEND=0 and (instr(userid,'{0}')>0 or createuserid='{0}') and to_char(b.conferencetime,'yyyy-mm-dd')='{1}'", user.UserId, time);
            DataTable dt = BaseRepository().FindTable(sql);
            //隐患排查(个人登记的，排查的，整改及验收的)
            sql = string.Format("select id,hidname title,3 type from v_basehiddeninfo t where t.createuserid='{0}' and (to_char(t.checkdate,'yyyy-mm-dd')='{1}' or to_char(t.createdate,'yyyy-mm-dd')='{1}')", user.UserId, time);
            DataTable dtHt = BaseRepository().FindTable(sql);
            dt.Merge(dtHt);

            //个人登记的违章或本人违章的
            sql = string.Format("select id,lllegaldescribe title,4 type from BIS_LLLEGALREGISTER t where (t.createuserid='{0}' or t.lllegalpersonid='{0}')  and to_char(t.LLLEGALTIME,'yyyy-mm-dd')='{1}'", user.UserId, time);
            dtHt = BaseRepository().FindTable(sql);
            dt.Merge(dtHt);
            return dt;
        }
        /// <summary>
        /// 获取各区域的最大风险等级信息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="areaCode"></param>
        /// <returns></returns>
        public DataTable GetAreaStatus(ERCHTMS.Code.Operator user, string areaCodes, int mode = 1)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("code");
            dt.Columns.Add("status");
            dt.Columns.Add("htnum");
            dt.Columns.Add("fxnum");
            dt.Columns.Add("areacode");
            dt.Columns.Add("wxnum");
            dt.Columns.Add("content");
            StringBuilder sb = new StringBuilder();
            foreach (string code in areaCodes.Split(','))
            {
                int val = 0;
                string htNum = "";
                string fxNum = "";
                string areaCode = "";
                object obj = BaseRepository().FindObject(string.Format("select d.districtcode from bis_district d where d.description='{0}' and d.districtcode like '{1}%'", code, user.OrganizeCode));
                if (obj != null)
                {
                    areaCode = obj.ToString();
                    string sql = "";
                    if (mode == 10)
                    {
                        sql = " and risktype in('管理','设备','环境')";
                    }
                    obj = BaseRepository().FindObject(string.Format("select min(gradeval) from BIS_RISKASSESS t where status=1 and deletemark=0 and t.areacode like '{0}%' {1}", areaCode, sql));
                    if (obj != null)
                    {
                        val = obj.ToInt();
                    }
                    else
                    {
                        val = 0;
                    }
                    if (mode == 1 || mode == 10)
                    {
                        //隐患数量
                        DataTable dtHt = BaseRepository().FindTable(string.Format("select rankname,count(1) num from v_basehiddeninfo t where t.workstream!='整改结束' and t.hidpoint like '{0}%' group by rankname", areaCode));
                        if (dtHt.Rows.Count > 0)
                        {
                            var rows = dtHt.Select("rankname='一般隐患'");
                            if (rows.Length > 0)
                            {
                                htNum = rows[0][1].ToString();
                            }
                            else
                            {
                                htNum = "0";
                            }
                            rows = dtHt.Select("rankname='重大隐患'");
                            if (rows.Length > 0)
                            {
                                htNum += "," + rows[0][1].ToString();
                            }
                            else
                            {
                                htNum += ",0";
                            }
                        }
                        sb.Clear();
                        //风险数量
                        DataTable dtRisk = BaseRepository().FindTable(string.Format(@"select nvl(num,0) from (select 1 gradeval from dual union all select 2 gradeval from dual union all select 3 gradeval from dual union all select 4 gradeval from dual) a
left join (select gradeval,count(1) num from BIS_RISKASSESS t where status=1 and deletemark=0 and t.areacode like '{0}%' {1} group by grade,gradeval) b
on a.gradeval=b.gradeval order by a.gradeval asc", areaCode, sql));
                        foreach (DataRow dr in dtRisk.Rows)
                        {
                            sb.AppendFormat("{0},", dr[0].ToString());
                        }
                        //重大危险源数量
                        int count = BaseRepository().FindObject(string.Format("select count(1) from HSD_HAZARDSOURCE t where IsDanger=1 and gradeval>0 and deptcode like '{0}%' and t.districtid in(select districtid from bis_district d where d.districtcode like '{1}%')", user.OrganizeCode, areaCode)).ToInt();
                        fxNum = sb.ToString().TrimEnd(',');
                        DataRow row = dt.NewRow();
                        row[0] = code;
                        row[1] = val;
                        row[2] = htNum;
                        row[3] = fxNum;
                        row[4] = areaCode;
                        row[5] = count;
                        dt.Rows.Add(row);
                    }

                    if (mode == 3)
                    {
                        DataTable dtSb = BaseRepository().FindTable(string.Format("select t.equipmentname,count(1) num from HRS_FIREFIGHTING t where districtcode='{1}' and t.dutydeptcode like '{0}%' group by equipmentname", user.OrganizeCode, areaCode));
                        if (dtSb.Rows.Count > 0)
                        {
                            DataRow row = dt.NewRow();
                            row[0] = code;
                            row[4] = areaCode;
                            row[2] = 1;
                            if (dtSb.Rows.Count > 0)
                            {
                                row["content"] = Newtonsoft.Json.JsonConvert.SerializeObject(dtSb);
                            }
                            dt.Rows.Add(row);
                        }

                    }
                }
                else
                {
                    DataRow row = dt.NewRow();
                    row[0] = code;
                    row[1] = val;
                    row[2] = "";
                    row[3] = "";
                    row[4] = "";
                    row[5] = 0;
                    dt.Rows.Add(row);
                }
            }
            return dt;
        }

        /// <summary>
        /// 获取各区域的最大风险等级信息(康巴什版本)
        /// </summary>
        /// <param name="user"></param>
        /// <param name="areaCode"></param>
        /// <returns></returns>
        public List<AreaRiskLevel> GetKbsAreaStatus()
        {
            string sql = "select areacode,min(gradeval) gradeval from BIS_RISKASSESS where status=1 and deletemark=0  and areacode is not null group by areacode";
            Repository<AreaRiskLevel> inlogdb = new Repository<AreaRiskLevel>(DbFactory.Base());
            List<AreaRiskLevel> dlist = inlogdb.FindList(sql).ToList();
            return dlist;
        }

        /// <summary>
        /// 获取各区域的最大风险等级信息(可门管控中心【风险四色图】版本)
        /// </summary>
        /// <returns></returns>
        public List<AreaRiskLevel> GetKMAreaStatus()
        {
            string sql = "select areacode,min(gradeval) gradeval from BIS_RISKASSESS where status=1 and deletemark=0 and (risktype='设备' or risktype='管理' or risktype='区域') and areacode is not null group by areacode";
            Repository<AreaRiskLevel> inlogdb = new Repository<AreaRiskLevel>(DbFactory.Base());
            List<AreaRiskLevel> dlist = inlogdb.FindList(sql).ToList();
            return dlist;
        }

        public int GetSafetyChangeNum(ERCHTMS.Code.Operator user)
        {
            string isHDGZ = new DataItemDetailService().GetItemValue("贵州毕节版本");
            string isAllDataRange = new DataItemDetailService().GetEnableItemValue("HighRiskWorkDataRange");
            string sqlstr = "select count(1) from bis_safetychange where 1=1";
            string sqlstr1 = "select a.*,'' as approveacount,e.outtransferuseraccount,e.intransferuseraccount from bis_safetychange a left join (select recid,flowid,outtransferuseraccount,intransferuseraccount,row_number()  over(partition by recid,flowid order by createdate desc) as num from BIS_TRANSFERRECORD where disable=0) e on a.id=e.recid and a.nodeid=e.flowid and e.num=1 where 1=1";
            string sqlstrWhere = "";
            if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级") || !string.IsNullOrWhiteSpace(isAllDataRange))
            {
                sqlstrWhere += string.Format(" and createuserorgcode='{0}'", user.OrganizeCode);
            }
            else
            {
                sqlstrWhere += string.Format(" and ((workunitcode in(select encode from base_department where encode like '{0}%'))  or (projectid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", user.DeptCode, user.DeptId);
            }

            string strCondition = " and ((isapplyover =0 and  iscommit =1) or (isapplyover =1 and  iscommit =1 and  isaccpcommit =1 and isaccepover=0))";
            DataTable data = BaseRepository().FindTable(sqlstr1 + sqlstrWhere + strCondition);
            for (int i = 0; i < data.Rows.Count; i++)
            {
                string executedept = string.Empty;
                highriskcommonapplyservice.GetExecutedept(data.Rows[i]["workunittype"].ToString(), data.Rows[i]["workunitid"].ToString(), data.Rows[i]["projectid"].ToString(), out executedept);
                string createdetpid = departmentservice.GetEntityByCode(data.Rows[i]["createuserdeptcode"].ToString()).IsEmpty() ? "" : departmentservice.GetEntityByCode(data.Rows[i]["createuserdeptcode"].ToString()).DepartmentId;
                string outsouringengineerdept = string.Empty;
                highriskcommonapplyservice.GetOutsouringengineerDept(data.Rows[i]["workunitid"].ToString(), out outsouringengineerdept);
                string str = manypowercheckservice.GetApproveUserAccount(data.Rows[i]["nodeid"].ToString(), data.Rows[i]["id"].ToString(), "", data.Rows[i]["specialtytype"].ToString(), executedept, outsouringengineerdept, createdetpid, "", "");
                data.Rows[i]["approveacount"] = str;
            }
            string[] applyids = data.Select("(outtransferuseraccount is null or outtransferuseraccount not like '%" + user.Account + ",%') and (approveacount like '%" + user.Account + ",%' or intransferuseraccount like '%" + user.Account + ",%')").AsEnumerable().Select(d => d.Field<string>("id")).ToArray();

            strCondition += string.Format(" and id in ('{0}')", string.Join("','", applyids));

            //strWhere += string.Format(" and approveaccount like '%{0}%' {1} ", user.Account + ",", strCondition);

            int SafetyNum = BaseRepository().FindObject(sqlstr + sqlstrWhere + strCondition).ToInt();
            return SafetyNum;
        }
        /// <summary>
        /// 获取安全检查数，依次为安全检查次数，待执行的安全检查数（省公司级）
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetSafetyCheckForGroup(ERCHTMS.Code.Operator user)
        {
            List<int> list = new List<int>();
            DataTable dt = BaseRepository().FindTable(string.Format(@"
             select count(1) from BIS_SAFTYCHECKDATARECORD t where t.createuserorgcode='{0}' and datatype in(0,2)
              union all
            select count(1) from bis_saftycheckdatarecord t where 
 id in(select distinct  a.recid from bis_saftycheckdatadetailed a left join bis_saftycontent b on a.id=b.detailid where checkDataType<>1 and b.id is null and (',' || a.CheckManid || ',') like '%,{1},%'
) ", user.OrganizeCode, user.Account));
            list.Add(dt.Rows[0][0].ToInt());//安全检查次数
            list.Add(dt.Rows[1][0].ToInt());//待执行的安全检查数
            dt.Dispose();

            int count = BaseRepository().FindObject(string.Format("select count(1) from bis_saftycheckdatarecord where createuserorgcode='{0}' and datatype=1 and rid is null", user.OrganizeCode)).ToInt();
            list.Add(count); //获取下发给下属电厂的检查任务数
            return list;
        }

        /// <summary>
        /// 获取省公司下发的安全检查任务
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetSafetyCheckTask(ERCHTMS.Code.Operator user)
        {
            List<int> list = new List<int>();
            int count = BaseRepository().FindObject(string.Format("select count(1) from bis_saftycheckdatarecord where checkeddepartid like '%{0}%' and (dutyuserid='{1}' or (',' ||receiveuserids || ',') like '%,{1},%') and length(checkdeptcode)<3", user.OrganizeId, user.Account)).ToInt();
            list.Add(count);
            return list;
        }
        /// <summary>
        /// 获取隐患信息，依次为重大隐患数，整改延期隐患数，逾期未整改隐患数，发现的隐患数量，未治理完成重大隐患数（省公司级）
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetHtForGroup(ERCHTMS.Code.Operator user)
        {
            List<int> list = new List<int>();
            string sql = "";
            string sql1 = "";
            sql = string.Format("select count(1) from v_basehiddeninfo t where  t.deptcode like '{0}%'  ", user.OrganizeCode);
            sql1 = string.Format("select distinct hidid from bis_htextension t where t.createuserorgcode in (select encode from BASE_DEPARTMENT d where d.deptcode like '{0}%' and d.nature='{1}' and d.description is null)", user.NewDeptCode, "厂级");
            int count = 0;
            //list.Add(count);

            //重大隐患数
            count = BaseRepository().FindObject(string.Format(sql + " and ((t.rankname='{0}' and  to_char(createdate,'yyyy')!='{1}' and workstream ='隐患整改') or (t.rankname='{0}' and  to_char(createdate,'yyyy') ='{1}'))", "重大隐患", DateTime.Now.Year)).ToInt();
            list.Add(count);

            //整改延期的隐患数量
            DataTable dt = BaseRepository().FindTable(sql1);
            list.Add(dt.Rows.Count);

            //逾期未整改隐患数
            count = BaseRepository().FindObject(string.Format(sql + " and (t.changedeadine+1) < to_date('{0}','yyyy-mm-dd hh24:mi:ss') and t.workstream='隐患整改'", DateTime.Now.ToString("yyyy-MM-dd 23:59:59"))).ToInt();
            list.Add(count);


            //省公司排查的隐患数量(发现的)
            string tsql = string.Format(@" {0} and createuserorgcode='{1}'", sql, user.OrganizeCode);
            count = BaseRepository().FindObject(tsql).ToInt();
            list.Add(count);

            //未治理完成的重大隐患数量
            count = BaseRepository().FindObject(string.Format("select count(1) from V_BASEHIDDENINFO t where t.deptcode like '{0}%' and t.rankname='重大隐患' and t.workstream<>'整改结束' and to_char(CHECKDATE,'yyyy')='{1}'", user.NewDeptCode, DateTime.Now.Year)).ToInt();
            list.Add(count);

            //待验收违章(省公司层级)
            count = BaseRepository().FindObject(string.Format(@"select count(a.id)  from v_lllegalallbaseinfo  a where  acceptpeopleid  =  '{0}' and flowstate = '违章验收'", user.UserId)).ToInt();
            list.Add(count);

            //违章次数
            count = BaseRepository().FindObject(string.Format(@" select count(b.id) from ( select departmentid ,fullname departmentname,encode, deptcode  from base_department where nature ='厂级' and deptcode like '{0}%') a
                                             left join (
                                              select id,reformdeptcode  from v_lllegalbaseinfo   where  to_char(createdate,'yyyy') ='{1}'  
                                            ) b on a.encode = substr(b.reformdeptcode,1,length(a.encode))   ", user.NewDeptCode, DateTime.Now.Year)).ToInt();
            list.Add(count);

            return list;
        }
        /// <summary>
        /// 安全预警项目（省公司级），依次为存在重大隐患的电厂数，隐患整改率小于80%的电厂数,隐患整改率
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<decimal> GetWarnItems(ERCHTMS.Code.Operator user)
        {
            int total = 0;
            List<decimal> list = new List<decimal>();
            //存在重大隐患
            //string sql = string.Format("select createuserorgcode from v_basehiddeninfo t where t.rankname='重大隐患' and t.createuserorgcode in(select encode from base_department d where d.deptcode like '{0}%' and d.nature='{1}' and  to_char(createdate,'yyyy') ='{2}') group by createuserorgcode", user.NewDeptCode, "厂级",DateTime.Now.Year.ToString());
            string sql = string.Format(@"select count(b.id) total,a.fullname  deptname ,a.encode deptcode ,a.organizeid from ( 
                    select encode ,fullname ,departmentid,organizeid,deptcode from BASE_DEPARTMENT t where deptcode like '{0}%'  and nature ='厂级' 
                    )  a 
                    left join   
                    (
                      select hiddepart ,id from v_basehiddeninfo where rankname ='重大隐患' and  to_char(createdate,'yyyy') ='{1}' 
                    ) b on a.departmentid = b.hiddepart  group by a.fullname ,a.encode,a.organizeid  order by deptcode", user.OrganizeCode, DateTime.Now.Year.ToString());
            DataTable dt = BaseRepository().FindTable(sql);
            foreach (DataRow row in dt.Rows)
            {
                total += int.Parse(row["total"].ToString());
            }
            list.Add(total);

            //隐患整改率低于80%的电厂
            dt = BaseRepository().FindTable(string.Format("select encode from base_department d where d.deptcode like '{0}%' and d.nature='{1}' and d.description is null", user.NewDeptCode, "厂级"));
            int num = 0;
            int sum = 0;
            foreach (DataRow dr in dt.Rows)
            {
                sum = BaseRepository().FindObject(string.Format("select count(1) from v_basehiddeninfo t where t.createuserorgcode='{0}'  and changedutydepartcode is not null  and to_char(createdate,'yyyy')='{1}'", dr[0].ToString(), DateTime.Now.Year)).ToInt();
                if (sum > 0)
                {
                    int count = BaseRepository().FindObject(string.Format("select count(1) from v_basehiddeninfo t where t.createuserorgcode='{0}'  and changedutydepartcode is not null and t.workstream<>'隐患整改' and to_char(createdate,'yyyy')='{1}'", dr[0].ToString(), DateTime.Now.Year)).ToInt();
                    if (double.Parse(count.ToString()) / double.Parse(sum.ToString()) < 0.8)
                    {
                        num++;
                    }
                }
            }
            list.Add(num);
            //隐患整改率
            sum = BaseRepository().FindObject(string.Format("select count(1) from v_basehiddeninfo t where deptcode like '{0}%'  and changedutydepartcode is not null  and to_char(createdate,'yyyy')='{1}'", user.NewDeptCode, DateTime.Now.Year)).ToInt();
            num = BaseRepository().FindObject(string.Format("select count(1) from v_basehiddeninfo t where deptcode like '{0}%'  and changedutydepartcode is not null and  t.workstream<>'隐患整改' and to_char(createdate,'yyyy')='{1}'", user.NewDeptCode, DateTime.Now.Year)).ToInt();
            decimal precent = 0;
            if (sum > 0)
            {
                precent = decimal.Parse(num.ToString()) / decimal.Parse(sum.ToString());
                precent = Math.Round(precent, 2) * 100;
            }
            list.Add(precent);

            //存在重大隐患的电厂
            int nums = BaseRepository().FindObject(string.Format(@"select count(deptcode)  nums from (
                                                          select count(b.id) total,a.encode deptcode from ( 
                                                             select encode ,fullname ,departmentid,organizeid,deptcode from BASE_DEPARTMENT t where deptcode like '{0}%'  and nature ='厂级' 
                                                          )  a 
                                                          left join   
                                                          (
                                                            select hiddepart ,id from v_basehiddeninfo where rankname ='重大隐患' and   to_char(createdate,'yyyy') ='{1}' 
                                                          ) b on a.departmentid = b.hiddepart  group by a.encode
                                                        ) a  where  total >0", user.NewDeptCode, DateTime.Now.Year)).ToInt();
            list.Add(nums);

            return list;
        }


        /// <summary>
        /// 一级风险超过3个的电厂
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<decimal> GetRiskAnalyzeItems(ERCHTMS.Code.Operator user)
        {
            int total = 0;

            List<decimal> list = new List<decimal>();

            string sql = string.Format(@"select count(a.encode)  from (
                                        select count(a.id) nums,b.encode from bis_riskassess  a
                                        left join (select encode ,d.fullname from base_department d where d.deptcode like '{0}%' and d.nature='厂级') b  on a.createuserorgcode = b.encode
                                        where a.status=1 and a.deletemark=0 and a.enabledmark=0 and a.grade ='一级'  
                                        group by b.encode )  a  where nums >3", user.NewDeptCode);

            total = BaseRepository().FindObject(sql).ToInt();

            list.Add(total);

            return list;
        }

        /// <summary>
        /// 获取电厂隐患整改率
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public List<decimal> GetHtZgl(string orgId)
        {
            List<decimal> list = new List<decimal>();
            //隐患整改率
            string sql = string.Format(@"select count(1) from v_basehiddeninfo t where t.HIDDEPART='{0}'  and changedutydepartcode is not null and t.workstream<>'隐患整改' and to_char(createdate,'yyyy')='{1}'
union all select count(1) from v_basehiddeninfo t where t.HIDDEPART='{0}'  and changedutydepartcode is not null and to_char(createdate,'yyyy')='{1}'", orgId, DateTime.Now.ToString("yyyy"));
            DataTable dt1 = BaseRepository().FindTable(sql);
            decimal precent = 0;
            if (dt1.Rows[1][0].ToString() != "0")
            {
                precent = decimal.Parse(dt1.Rows[0][0].ToString()) / decimal.Parse(dt1.Rows[1][0].ToString());
                precent = Math.Round(precent * 100, 2);
            }
            list.Add(decimal.Parse(dt1.Rows[1][0].ToString()));
            list.Add(precent);
            return list;
        }

        #region 获取隐患或者风险的统计项目
        /// <summary>
        /// 获取隐患或者风险的统计项目
        /// </summary>
        /// <param name="user"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public DataTable GetHtOrRiskItems(ERCHTMS.Code.Operator user, int mode)
        {
            string sql = string.Empty;

            //存在重大隐患的电厂
            if (mode == 2)
            {
                sql = string.Format(@"  select * from ( select count(b.id) nums,a.fullname departmentname ,a.departmentid，a.deptcode from ( 
                                           select encode ,fullname ,departmentid,organizeid,deptcode from BASE_DEPARTMENT t where deptcode like '{0}%'  and nature ='厂级' 
                                          )  a 
                                          left join   
                                          (
                                          select hiddepart ,id from v_basehiddeninfo where rankname ='重大隐患'  and  to_char(createdate,'yyyy') ='{1}' 
                                          ) b on a.departmentid = b.hiddepart  group by a.fullname ,a.departmentid，a.deptcode  ) a  where  a.nums >0   order by deptcode", user.NewDeptCode, DateTime.Now.Year);
            }
            else //一级风险超过3个的电厂
            {
                sql = string.Format(@"select * from (
                                             select count(a.id) nums,b.departmentname,b.departmentid,b.deptcode from bis_riskassess  a
                                              left join 
                                              (
                                                select d.encode ,d.fullname departmentname ,d.departmentid,d.deptcode  from base_department d where d.deptcode like '{0}%' and d.nature='厂级'
                                              ) b  on a.createuserorgcode = b.encode
                                              where a.status=1 and a.deletemark=0 and a.enabledmark=0 and a.grade ='一级'  
                                              group by b.departmentname,b.departmentid,b.deptcode) a where a.nums>3 order by deptcode", user.NewDeptCode);
            }

            DataTable dt = BaseRepository().FindTable(sql);

            return dt;
        }
        #endregion

        /// <summary>
        /// 电厂安全检查和隐患信息统计，依次为 安全检查次数，隐患总数，重大隐患数，未闭环隐患数，逾期未整改隐患数，隐患整改率，未治理完成重大隐患数
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public List<decimal> GetHt2CheckOfFactory(string orgId, string time, string orgCode = "")
        {
            List<decimal> list = new List<decimal>();
            string sql = "";
            sql = string.Format("select count(1) from BIS_SAFTYCHECKDATARECORD t where datatype in(0,2) and belongdept like '{2}%' and to_char(checkendtime,'yyyy-mm')='{1}' and (checkeddepartid like '%{0}%' or ',' || checkdeptcode like '%,{0}%' or createuserdeptcode  in (select encode from base_department start with encode='{2}' connect by  prior departmentid = parentid))", orgId, DateTime.Parse(time).ToString("yyyy-MM"), orgCode);
            int count = BaseRepository().FindObject(sql).ToInt();//安全检查次数
            list.Add(count);

            sql = string.Format("select count(1) from V_BASEHIDDENINFO t where t.HIDDEPART='{0}' and to_char(createdate,'yyyy-mm')='{1}'", orgId, DateTime.Parse(time).ToString("yyyy-MM"));
            count = BaseRepository().FindObject(sql).ToInt();//隐患总数
            list.Add(count);

            //重大隐患数
            count = BaseRepository().FindObject(string.Format("select count(1) from V_BASEHIDDENINFO t where t.HIDDEPART='{0}' and t.rankname='{1}' and to_char(createdate,'yyyy-mm')='{2}'", orgId, "重大隐患", DateTime.Parse(time).ToString("yyyy-MM"))).ToInt();
            list.Add(count);

            //未闭环隐患数
            count = BaseRepository().FindObject(string.Format("select count(1) from V_BASEHIDDENINFO t where t.HIDDEPART='{0}' and t.workstream<>'整改结束' and to_char(createdate,'yyyy-mm')='{1}'", orgId, DateTime.Parse(time).ToString("yyyy-MM"))).ToInt();
            list.Add(count);

            //逾期未整改隐患数
            count = BaseRepository().FindObject(string.Format("select count(1) from V_BASEHIDDENINFO t where t.HIDDEPART='{0}' and (t.changedeadine+1) <to_date('{2}','yyyy-mm-dd hh24:mi:ss') and t.workstream='隐患整改' and  to_char(t.createdate,'yyyy-mm') ='{1}' ", orgId, DateTime.Parse(time).ToString("yyyy-MM"), DateTime.Now.ToString("yyyy-MM-dd 23:59:59"))).ToInt();
            list.Add(count);

            //隐患整改率
            sql = string.Format(@"select count(1) from v_basehiddeninfo t where t.HIDDEPART='{0}'  and changedutydepartcode is not null and t.workstream<>'隐患整改' and to_char(createdate,'yyyy-mm')='{1}'
union all select count(1) from v_basehiddeninfo t where t.HIDDEPART='{0}'  and changedutydepartcode is not null and to_char(createdate,'yyyy-mm')='{1}'", orgId, DateTime.Parse(time).ToString("yyyy-MM"));
            DataTable dt1 = BaseRepository().FindTable(sql);
            decimal precent = 0;
            if (dt1.Rows[1][0].ToString() != "0")
            {
                precent = decimal.Parse(dt1.Rows[0][0].ToString()) / decimal.Parse(dt1.Rows[1][0].ToString());
                precent = Math.Round(precent * 100, 2);
            }
            list.Add(precent);

            ERCHTMS.Code.Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            //未治理完成的重大隐患数量
            count = BaseRepository().FindObject(string.Format("select count(1) from V_BASEHIDDENINFO t where t.deptcode like '{0}%' and t.rankname='重大隐患' and t.workstream<>'整改结束' and to_char(createdate,'yyyy-mm')='{1}'", user.NewDeptCode, DateTime.Parse(time).ToString("yyyy-MM"))).ToInt();
            list.Add(count);

            return list;
        }
        /// <summary>
        /// 电厂隐患排名
        /// </summary>
        /// <param name="user">用户对象</param>
        /// <param name="mode">排名方式，0：按隐患数量排名，1：按隐患整改率排名，2：按未闭环的数量排名</param>
        /// <returns></returns>
        public DataView GetRatioDataOfFactory(ERCHTMS.Code.Operator user, int mode = 0)
        {
            string deptCode = user.NewDeptCode;
            DataTable dt = null;
            string where = "nature ='厂级'";
            string sql = "";
            if (!user.RoleName.Contains("省级"))
            {
                where = string.Format("(nature ='{0}' or nature='{1}' or nature='{2}') and description is null", "部门", "承包商", "分包商");
                sql = string.Format(@"select count(b.id) total,a.fullname  deptname,a.encode,departmentid deptid from ( 
                    select encode,fullname,departmentid,organizeid,deptcode from BASE_DEPARTMENT t where deptcode like '{1}%'  and {2} 
                    )  a 
                    left join   
                    (
                      select changedutydepartcode,id from v_basehiddeninfo where  to_char(createdate,'yyyy') ='{0}' 
                    ) b on a.encode = b.changedutydepartcode  group by a.fullname,a.encode,departmentid order by total desc", DateTime.Now.Year, deptCode, where);
            }
            else
            {
                sql = string.Format(@"select count(b.id) total,a.fullname  deptname,a.encode,departmentid deptid from ( 
                    select encode,fullname,departmentid,organizeid,deptcode from BASE_DEPARTMENT t where deptcode like '{1}%'  and {2} 
                    )  a 
                    left join   
                    (
                      select hiddepart ,id from v_basehiddeninfo where  to_char(createdate,'yyyy') ='{0}' 
                    ) b on a.departmentid = b.hiddepart  group by a.fullname,a.encode,departmentid order by total desc", DateTime.Now.Year, deptCode, where);
            }

            dt = BaseRepository().FindTable(sql);
            if (mode == 1)
            {
                dt = BaseRepository().FindTable("select 1 total,fullname deptname,encode,departmentid deptid from BASE_DEPARTMENT where departmentid='001'");
                DataTable dtDepts = BaseRepository().FindTable(string.Format("select encode,fullname,departmentid from BASE_DEPARTMENT d where d.deptcode like '{0}%' and {1} order by sortcode asc", user.NewDeptCode, where));
                foreach (DataRow dr in dtDepts.Rows)
                {
                    sql = string.Format(@"select count(1) from v_basehiddeninfo t where t.HIDDEPART='{0}'  and changedutydepartcode is not null and t.workstream<>'隐患整改' and to_char(createdate,'yyyy')='{1}'
union all select count(1) from v_basehiddeninfo t where t.HIDDEPART='{0}'  and changedutydepartcode is not null and to_char(createdate,'yyyy')='{1}'", dr[2].ToString(), DateTime.Now.Year);
                    if (!user.RoleName.Contains("省级"))
                    {
                        sql = string.Format(@"select count(1) from v_basehiddeninfo t where changedutydepartcode='{0}'  and changedutydepartcode is not null and t.workstream<>'隐患整改' and to_char(createdate,'yyyy')='{1}'
union all select count(1) from v_basehiddeninfo t where changedutydepartcode='{0}' and to_char(createdate,'yyyy')='{1}'", dr[0].ToString(), DateTime.Now.Year);
                    }
                    DataTable dt1 = BaseRepository().FindTable(sql);
                    decimal precent = 0;
                    if (dt1.Rows[1][0].ToString() != "0")
                    {
                        precent = decimal.Parse(dt1.Rows[0][0].ToString()) / decimal.Parse(dt1.Rows[1][0].ToString());
                        precent = Math.Round(precent * 100, 2);
                    }
                    DataRow newRow = dt.NewRow();
                    newRow[0] = precent;
                    newRow[1] = dr[1].ToString();
                    newRow[2] = dr[0].ToString();
                    newRow[3] = dr[2].ToString();
                    dt.Rows.Add(newRow);
                }

            }
            if (mode == 2)
            {
                if (!user.RoleName.Contains("省级"))
                {
                    sql = string.Format(@"select count(b.id) total,a.fullname  deptname,a.encode,departmentid deptid from ( 
                    select encode,fullname,departmentid,organizeid from BASE_DEPARTMENT t where deptcode like '{1}%'  and {2} 
                    )  a 
                    left join   
                    (
                      select changedutydepartcode,id from v_basehiddeninfo where  to_char(createdate,'yyyy') ='{0}' and  workstream<>'整改结束'
                    ) b on a.encode = b.changedutydepartcode  group by a.fullname ,a.encode,departmentid order by total desc", DateTime.Now.Year, deptCode, where);
                }
                else
                {
                    sql = string.Format(@"select count(b.id) total,a.fullname  deptname,a.encode,departmentid deptid from ( 
                    select encode,fullname,departmentid,organizeid from BASE_DEPARTMENT t where deptcode like '{1}%'  and {2} 
                    )  a 
                    left join   
                    (
                      select hiddepart,id from v_basehiddeninfo where  to_char(createdate,'yyyy') ='{0}' and  workstream<>'整改结束'
                    ) b on a.departmentid = b.hiddepart  group by a.fullname ,a.encode,departmentid order by total desc", DateTime.Now.Year, deptCode, where);
                }

                dt = BaseRepository().FindTable(sql);
            }
            dt.DefaultView.Sort = "total desc";
            return dt.DefaultView;
        }
        /// <summary>
        /// 获取当前用户所属机构的数据指标项目
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable GetDeptDataSet(ERCHTMS.Code.Operator user, string itemType)
        {
            string[] arr = user.RoleName.Split(',');
            StringBuilder sb = new StringBuilder();
            foreach (string name in arr)
            {
                sb.AppendFormat("rolename like '%{0}%' or ", name);
            }
            string roleNames = sb.ToString();
            roleNames = roleNames.Substring(0, roleNames.Length - 3);
            string sql = string.Format("select t.itemcode,t.itemname,t.icon,t.address,t.callback,itemstyle,itemtype from base_dataset t where  t.isopen='是' and itemkind like '%{1}%' and  itemrole like '%一般用户%' and (t.isdefault='是' or t.deptcode like '%,{0},%') and (rolename is null or (rolename is not null and ({2}))) order by sortcode asc", user.OrganizeCode, itemType, roleNames);
            if (user.RoleName.Contains("领导") || user.RoleName.Contains("厂级"))
            {
                sql = string.Format("select t.itemcode,t.itemname,t.icon,t.address,t.callback,itemstyle,itemtype from base_dataset t where  t.isopen='是' and itemkind like '%{1}%' and itemrole like '%公司领导%' and (t.isdefault='是' or t.deptcode like '%,{0},%') and (rolename is null or (rolename is not null and ({2}))) order by sortcode asc", user.OrganizeCode, itemType, roleNames);
            }
            DataTable dt = BaseRepository().FindTable(sql);
            return dt;
        }


        /// <summary>
        /// 危险作业审批单待办
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int GetJobApprovalFormNum(ERCHTMS.Code.Operator user)
        {
            List<int> list = new List<int>();
            string sqlstr = @"select t.id,t1.id as flowdetailid,t.jobtype,'' as isrole from BIS_JobApprovalForm t left join BIS_DangerousJobFlowDetail t1
                                        on t.id=t1.businessid and t1.status=0 where t.jobstate=1";
            string where = "";
            DataTable dt = BaseRepository().FindTable(sqlstr);
            foreach (DataRow dr in dt.Rows)
            {
                string BusinessId = dr["id"].ToString();
                //获取当前用户是否有权限操作该条数据
                string approveName = "";
                string approveId = "";
                string approveAccount = "";
                dr["isrole"] = DetailService.GetCurrentStepRole(BusinessId, user.Account, dr["flowdetailid"] == null ? "" : dr["flowdetailid"].ToString(), out approveName, out approveId, out approveAccount);
            }
            where += string.Format(" and t.id in ('{0}')", string.Join("','", dt.Select("isrole ='0'").AsEnumerable().Select(d => d.Field<string>("id")).ToArray()));

            DataTable data = BaseRepository().FindTable(sqlstr + where);
            //list.Add(data.Select("jobtype='HeightWorking'").Count());//高处作业


            return data.Rows.Count;
        }
        #endregion

        #region 厂级安全评估分值计算

        #region 获取指定年月的隐患指标记录
        /// <summary>
        /// 获取指定年月的隐患指标记录
        /// </summary>
        /// <param name="user"></param>
        /// <param name="startDate"></param>
        /// <returns></returns>
        public decimal GetSafetyAssessedValue(SafetyAssessedArguments entity)
        {
            var list = GetSafetyAssessedData(entity);
            decimal score = 0;
            foreach (SafetyAssessedModel model in list)
            {
                score += model.classificationscore;
            }
            return score;
        }
        #endregion

        #region 获取安全评估模型分值数据
        /// <summary>
        /// 获取隐患排查指标相关数据
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<SafetyAssessedModel> GetSafetyAssessedData(SafetyAssessedArguments argument)
        {
            List<SafetyAssessedModel> list = new List<SafetyAssessedModel>();
            try
            {
                //获取本机构下的模块功能
                var classification = iclassificationservice.GetList(argument.orgId).Where(p => p.CisEnable == "1").ToList();
                //如果对应机构不存在模块功能
                if (classification.Count() == 0)
                {
                    classification = iclassificationservice.GetList("0").ToList();
                }
                foreach (ClassificationEntity entity in classification)
                {
                    SafetyAssessedModel smodel = new SafetyAssessedModel();
                    smodel.classificationcode = entity.ClassificationCode;  //模块编码
                    smodel.classificationindex = entity.ClassificationIndex; //模块名称
                    argument.classificationcode = entity.ClassificationCode;
                    //基础指标数据
                    var cislist = classificationindexservices.GetListByOrganizeId(argument.orgId).Where(p => p.ClassificationCode == entity.ClassificationCode && p.IsEnable == "1").ToList();
                    if (cislist.Count() == 0)
                    {
                        cislist = classificationindexservices.GetListByOrganizeId("0").Where(p => p.ClassificationCode == entity.ClassificationCode).ToList();
                    }
                    var data = GetSafetyAssessedChildData(argument, cislist);
                    smodel.data = data;
                    decimal score = 0;
                    foreach (SafetyAssessedChildModel cmode in data)
                    {
                        score += cmode.score;
                    }
                    smodel.classificationscore = Math.Round(score * (!string.IsNullOrEmpty(entity.WeightCoeffcient) ? Math.Round(Convert.ToDecimal(entity.WeightCoeffcient) / 100, 2) : 0), 2);//模块获得的分数
                    list.Add(smodel);
                }
                return list;
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion

        #region 获取模块下对应的指标
        /// <summary>
        /// 获取模块下对应的指标
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<SafetyAssessedChildModel> GetSafetyAssessedChildData(SafetyAssessedArguments argument, List<ClassificationIndexEntity> list)
        {
            List<SafetyAssessedChildModel> rlist = new List<SafetyAssessedChildModel>();
            foreach (ClassificationIndexEntity entity in list)
            {
                SafetyAssessedChildModel cmodel = new SafetyAssessedChildModel();
                cmodel.indexcode = entity.IndexCode;
                cmodel.indexname = entity.IndexName;
                cmodel.indexscore = entity.IndexScore;
                cmodel.indexstandard = entity.IndexStandard;
                string[] argValue = entity.IndexArgsValue.Split('|');
                decimal downScore = 0; //扣分
                int record = 0; //返回的记录数
                int indexValue = 0;
                string sql = string.Empty;
                //各模块
                switch (argument.classificationcode)
                {
                    #region 隐患排查
                    case "01":
                        #region  逾期未整改重大隐患数量，逾期未整改一般隐患数
                        if (entity.IndexCode == "01" || entity.IndexCode == "02")
                        {
                            indexValue = entity.IndexCode == "01" ? 1 : 2;  //action为1：逾期未整改重大隐患数量 ,action为2:逾期未整改一般隐患数
                            var result = htbaseinfoiservice.GetSafetyValueOfWarning(indexValue, argument.orgCode, "", argument.endDate);
                            record = result.Rows.Count; //返回的记录数
                            argValue[0] = !string.IsNullOrEmpty(argValue[0]) ? argValue[0].ToString() : "0";
                            downScore = record * Convert.ToDecimal(argValue[0].ToString());
                            if (downScore > Convert.ToDecimal(entity.IndexScore))
                            {
                                downScore = Convert.ToDecimal(entity.IndexScore);
                            }
                            downScore = downScore > Convert.ToDecimal(entity.IndexScore) ? Convert.ToDecimal(entity.IndexScore) : downScore;
                            cmodel.deductpoint = downScore; //扣分项
                            cmodel.score = Convert.ToDecimal(entity.IndexScore) - downScore; //得分项
                        }
                        #endregion
                        #region 公司级每月安全检查次数、部门级每月安全检查次数
                        else if (entity.IndexCode == "03" || entity.IndexCode == "04")
                        {
                            #region 公司级检查
                            //实际次数
                            int selCount = 0;
                            //条件
                            sql = string.Format(@"select count(id) from bis_saftycheckdatarecord where 1=1 and createuserorgcode='{0}'", argument.orgCode);
                            //次数
                            int everyTime = !string.IsNullOrEmpty(argValue[1]) ? int.Parse(argValue[1].ToString()) : 0;
                            //每次扣分扣分
                            decimal everyPoint = !string.IsNullOrEmpty(argValue[2]) ? Convert.ToDecimal(argValue[2].ToString()) : 0;
                            if (entity.IndexCode == "03") //公司级
                            {
                                sql += " and checklevel =1 ";
                            }
                            if (entity.IndexCode == "04") //部门级
                            {
                                sql += " and checklevel =2";
                            }
                            int cMonth = DateTime.Now.Month;
                            for (int i = 1; i <= cMonth; i++)
                            {
                                string csql = string.Empty;
                                string startdate = string.Empty;
                                string enddate = string.Empty;
                                if (i == 12)
                                {
                                    startdate = DateTime.Now.Year + "-" + i + "-1";
                                    enddate = DateTime.Now.Year + "-" + i + "-31";
                                    csql = string.Format(@" and createdate>=to_date('{0}','yyyy-mm-dd hh24:mi:ss') and createdate<to_date('{1}','yyyy-mm-dd hh24:mi:ss')", startdate, enddate);
                                }
                                else
                                {
                                    startdate = DateTime.Now.Year + "-" + i + "-1";
                                    enddate = DateTime.Now.Year + "-" + (i + 1) + "-1";
                                    csql += string.Format(@" and createdate>=to_date('{0}','yyyy-mm-dd hh24:mi:ss') and createdate<to_date('{1}','yyyy-mm-dd hh24:mi:ss')", startdate, enddate);
                                }
                                string tsql = sql + csql;
                                selCount = this.BaseRepository().FindObject(string.Format(tsql)).ToInt();
                                if (selCount < everyTime)
                                {
                                    downScore += everyPoint * (everyTime - selCount);
                                }
                            }
                            downScore = downScore > Convert.ToDecimal(entity.IndexScore) ? Convert.ToDecimal(entity.IndexScore) : downScore;
                            cmodel.deductpoint = downScore; //扣分项
                            cmodel.score = Convert.ToDecimal(entity.IndexScore) - downScore; //得分项
                            #endregion
                        }
                        #endregion
                        break;
                    #endregion
                    #region 安全风险管控
                    case "02":
                        #region 安全风险定期辨识评估次数
                        if (entity.IndexCode == "01")
                        {
                            string equaldate = string.Empty;
                            string curdate = DateTime.Now.ToString("yyyy-MM-dd");
                            //获取制定安全风险定期辨识的部门
                            string regcode = argument.orgCode;
                            //获取上一年度的风险辨识评估
                            sql = @"select id, to_char(modifydate,'yyyy-MM-dd') modifydate  from bis_riskplan a where a.status=1 and a.createuserdeptcode  like  '{0}%' and  to_char(modifydate,'yyyy') ='{1}' order by modifydate desc";
                            var prevDt = this.BaseRepository().FindTable(string.Format(sql, regcode, DateTime.Now.AddYears(-1).Year.ToString()));
                            //获取当前年度的风险辨识评估
                            var curDt = this.BaseRepository().FindTable(string.Format(sql, regcode, DateTime.Now.Year.ToString()));
                            //上一年度存在辨识数据
                            if (prevDt.Rows.Count > 0)
                            {
                                equaldate = prevDt.Rows[0]["modifydate"].ToString();   //获取对应的比对日期(上一年辨识完成的日期)
                            }
                            //当前年度存在满足条件的辨识的数据
                            if (curDt.Rows.Count >= Convert.ToDecimal(argValue[1].ToString()))
                            {
                                cmodel.deductpoint = 0;
                                cmodel.score = Convert.ToDecimal(entity.IndexScore); //得分项
                            }
                            else //当前年度不存在辨识的数据,扣完所有分数
                            {
                                cmodel.deductpoint = Convert.ToDecimal(entity.IndexScore);
                                cmodel.score = 0; //得分项
                                if (!string.IsNullOrEmpty(equaldate))    //如果对比日期不为空,则表示存在往年辨识记录
                                {
                                    //当前时间超过 前一年度辨识完成日期对应的今年应该辨识的日期时，进行天数积分运算;
                                    int differenceValue = (Convert.ToDateTime(curdate) - Convert.ToDateTime(equaldate).AddYears(1)).Days;
                                    if (differenceValue > 0)
                                    {
                                        downScore = Math.Round(differenceValue / Convert.ToDecimal(argValue[3].ToString()), 2) * Convert.ToDecimal(argValue[4].ToString());
                                        cmodel.deductpoint = downScore > Convert.ToDecimal(entity.IndexScore) ? Convert.ToDecimal(entity.IndexScore) : downScore;
                                        cmodel.score = Convert.ToDecimal(entity.IndexScore) - cmodel.deductpoint;
                                    }
                                    else  //还未到应该辨识的日期，则给满分
                                    {
                                        cmodel.deductpoint = 0;
                                        cmodel.score = Convert.ToDecimal(entity.IndexScore);
                                    }
                                }
                            }
                        }
                        #endregion
                        #region 正在进行的一级高风险作业数量,正在进行的二级高风险作业数量
                        if (entity.IndexCode == "02" || entity.IndexCode == "03")
                        {
                            int risktype = entity.IndexCode == "02" ? 0 : 1; //一级风险 0  or 二级风险 1
                            sql = string.Format(@"select count(a.id) from  bis_highriskcommonapply a 
                                                left join base_dataitemdetail b on a.risktype=b.itemvalue and itemid =(select itemid from base_dataitem where itemcode='CommonRiskType')
                                                where applystate='5' and  realityworkstarttime is not null and realityworkendtime is null  and  a.createdate < to_date('{0}' ,'yyyy-mm-dd hh24:mi:ss') 
                                                and a.createuserorgcode like '{1}%'  and  a.risktype ='{2}'", argument.endDate, argument.orgCode, risktype);
                            record = this.BaseRepository().FindObject(sql).ToInt();
                            if (record > 0)
                            {
                                downScore = Math.Round(record / Convert.ToDecimal(argValue[0].ToString()), 2) * Convert.ToDecimal(argValue[1].ToString());
                                cmodel.deductpoint = downScore > Convert.ToDecimal(entity.IndexScore) ? Convert.ToDecimal(entity.IndexScore) : downScore;
                                cmodel.score = Convert.ToDecimal(entity.IndexScore) - cmodel.deductpoint;
                            }
                            else
                            {
                                cmodel.deductpoint = 0;
                                cmodel.score = Convert.ToDecimal(entity.IndexScore);
                            }
                        }
                        #endregion
                        break;
                    #endregion
                    #region 事故事件管理
                    case "03":
                        #region 未遂事件起数
                        if (entity.IndexCode == "01")
                        {
                            //未遂事件起数
                            sql = @"select count(1) from  v_aem_wssjbg_deal_order v where v.createuserorgcode='{0}' and issubmit_deal =1 and to_char(v.createdate,'yyyy')='{1}'";
                            record = this.BaseRepository().FindObject(string.Format(sql, argument.orgCode, DateTime.Now.Year)).ToInt();
                            if (record > 0)
                            {
                                downScore = record * Convert.ToDecimal(argValue[0].ToString());
                                cmodel.deductpoint = downScore > Convert.ToDecimal(entity.IndexScore) ? Convert.ToDecimal(entity.IndexScore) : downScore;
                                cmodel.score = Convert.ToDecimal(entity.IndexScore) - cmodel.deductpoint;
                            }
                            else
                            {
                                cmodel.deductpoint = 0;
                                cmodel.score = Convert.ToDecimal(entity.IndexScore);
                            }
                        }
                        #endregion
                        #region 事故事件起数
                        else
                        {
                            //事故事件起数
                            sql = string.Format(@"select sum(num) num , sglevelname_deal from (
                                                    select count(1) num ,(case when  sglevelname_deal !='一般事故' then '其他事故' else '一般事故' end) sglevelname_deal   
                                                    from aem_bulletin_deal a where issubmit_deal>0  and a.createuserorgcode='{0}' and to_char(createdate,'yyyy')='{1}' 
                                       group by sglevelname_deal )   group by  sglevelname_deal ", argument.orgCode, DateTime.Now.Year);
                            var result = this.BaseRepository().FindTable(sql);
                            //如果存在记录
                            if (result.Rows.Count > 0)
                            {
                                downScore = 0;
                                foreach (DataRow row in result.Rows)
                                {
                                    record = int.Parse(row["num"].ToString());
                                    if (row["sglevelname_deal"].ToString() == "一般事故")
                                    {
                                        downScore += record * Convert.ToDecimal(argValue[0].ToString()); //一般事故
                                    }
                                    else
                                    {
                                        downScore += record * Convert.ToDecimal(argValue[1].ToString()); //较大及以上事故
                                    }
                                }
                                cmodel.deductpoint = downScore > Convert.ToDecimal(entity.IndexScore) ? Convert.ToDecimal(entity.IndexScore) : downScore;
                                cmodel.score = Convert.ToDecimal(entity.IndexScore) - cmodel.deductpoint;
                            }
                            else
                            {
                                cmodel.deductpoint = 0;
                                cmodel.score = Convert.ToDecimal(entity.IndexScore);
                            }
                        }
                        #endregion
                        break;
                    #endregion
                    #region 危险源管理
                    case "04":
                        #region 重大危险源登记建档数
                        if (entity.IndexCode == "01")
                        {
                            //总的数量
                            sql = string.Format(@"select count(1) from hsd_hazardsource a where a.createuserorgcode='{0}'  and a.isdanger>0", argument.orgCode);
                            record = this.BaseRepository().FindObject(sql).ToInt();
                            //已经建档的数量
                            string tsql = string.Format(@"select count(1) from v_hsd_dangerqd_djjd a where a.createuserorgcode='{0}' ", argument.orgCode);
                            int trecord = this.BaseRepository().FindObject(tsql).ToInt();
                            if (record > 0)
                            {
                                downScore = Math.Round((record - trecord) / Convert.ToDecimal(argValue[0].ToString()), 2) * Convert.ToDecimal(argValue[1].ToString());
                                cmodel.deductpoint = downScore > Convert.ToDecimal(entity.IndexScore) ? Convert.ToDecimal(entity.IndexScore) : downScore;
                                cmodel.score = Convert.ToDecimal(entity.IndexScore) - cmodel.deductpoint;
                            }
                            else
                            {
                                cmodel.deductpoint = 0;
                                cmodel.score = Convert.ToDecimal(entity.IndexScore);
                            }
                        }
                        #endregion
                        #region 重大危险源监控率
                        else
                        {
                            sql = string.Format(@"select count(1) from hsd_hazardsource a where a.deptcode like '{0}%' and a.isdanger>0", argument.orgCode); //重大危险源总数,截止目前
                            record = this.BaseRepository().FindObject(sql).ToInt();

                            string tsql = string.Format(@"select count(a.id) from hsd_jkjc a left join hsd_hazardsource b on a.hdid = b.id   
                                                        where b.isdanger >0 and b.deptcode  like '{0}%'", argument.orgCode); //重大危险源监控,截止目前
                            int trecord = this.BaseRepository().FindObject(tsql).ToInt();
                            decimal cvalue = (Convert.ToDecimal(record.ToString()) > 0 ? Math.Round((Convert.ToDecimal(trecord.ToString()) / Convert.ToDecimal(record.ToString())) * 100, 2) : 100);
                            downScore = Math.Round(Convert.ToDecimal(argValue[2].ToString()) * (Convert.ToDecimal(argValue[0].ToString()) - cvalue) / Convert.ToDecimal(argValue[1].ToString()), 2);
                            cmodel.deductpoint = downScore > Convert.ToDecimal(entity.IndexScore) ? Convert.ToDecimal(entity.IndexScore) : downScore;
                            cmodel.score = Convert.ToDecimal(entity.IndexScore) - cmodel.deductpoint;
                        }
                        #endregion
                        break;
                    #endregion
                    #region 应急管理
                    case "05":
                        #region 应急预案按期演练率
                        if (entity.IndexCode == "01")
                        {
                            sql = string.Format(@"select count(1) from mae_drillplan a  where to_char(a.createdate,'yyyy') ='{0}' and a.createuserorgcode ='{1}'", DateTime.Now.Year.ToString(), argument.orgCode);
                            record = this.BaseRepository().FindObject(sql).ToInt();
                            string tsql = string.Format(@"select count(1) from (
                                                    select count(b.id) nums, b.drillplanid from mae_drillplan a 
                                                    left join mae_drillplanrecord b  on a.id = b.drillplanid
                                                    where to_char(a.createdate,'yyyy') = '{0}' and a.createuserorgcode = '{1}'  group by  b.drillplanid
                                                )  a where a.nums >0", DateTime.Now.Year, argument.orgCode);
                            int trecord = this.BaseRepository().FindObject(tsql).ToInt();

                            decimal cvalue = record > 0 ? Convert.ToDecimal(argValue[0].ToString()) - Math.Round((Convert.ToDecimal(trecord.ToString()) / Convert.ToDecimal(record.ToString())) * 100, 2) * 100 : 0;
                            downScore = cvalue > 0 ? Math.Round(cvalue / Convert.ToDecimal(argValue[1].ToString()), 2) * Convert.ToDecimal(argValue[2].ToString()) : 0;
                            cmodel.deductpoint = downScore > Convert.ToDecimal(entity.IndexScore) ? Convert.ToDecimal(entity.IndexScore) : downScore;
                            cmodel.score = Convert.ToDecimal(entity.IndexScore) - cmodel.deductpoint;
                        }
                        #endregion
                        #region 综合(专项)应急预案演练次数
                        else if (entity.IndexCode == "02")
                        {
                            string equaldate = string.Empty;
                            string curdate = DateTime.Now.ToString("yyyy-MM-dd");
                            //获取上一年度的综合(专项)应急预案演练
                            sql = @"select  id,  drilltime  from mae_drillplanrecord a  where to_char(a.drilltime,'yyyy') = '{0}' and a.createuserorgcode = '{1}' and  (a.drilltypename ='综合应急预案' or a.drilltypename ='专项应急预案')";
                            var prevDt = this.BaseRepository().FindTable(string.Format(sql, DateTime.Now.AddYears(-1).Year.ToString(), argument.orgCode));
                            //获取当前年度的综合(专项)应急预案演练
                            var curDt = this.BaseRepository().FindTable(string.Format(sql, DateTime.Now.Year.ToString(), argument.orgCode));
                            //上一年度存在综合(专项)应急预案演练
                            if (prevDt.Rows.Count > 0)
                            {
                                equaldate = prevDt.Rows[0]["drilltime"].ToString();   //获取对应的比对日期(上一年演练的日期)
                            }
                            //当前年度存在综合(专项)应急预案演练的数据
                            if (curDt.Rows.Count >= Convert.ToDecimal(argValue[0].ToString()))
                            {
                                cmodel.deductpoint = 0;
                                cmodel.score = Convert.ToDecimal(entity.IndexScore); //得分项
                            }
                            else //当前年度不存在数据,扣完所有分数
                            {
                                downScore = (Convert.ToDecimal(argValue[0].ToString()) - curDt.Rows.Count) / Convert.ToDecimal(argValue[1].ToString()) * Convert.ToDecimal(argValue[2].ToString());
                                cmodel.deductpoint = downScore > Convert.ToDecimal(entity.IndexScore) ? Convert.ToDecimal(entity.IndexScore) : downScore;
                                cmodel.score = Convert.ToDecimal(entity.IndexScore) - cmodel.deductpoint; //得分项
                                if (!string.IsNullOrEmpty(equaldate))    //如果对比日期不为空,则表示存在往年记录
                                {
                                    //当前时间超过 前一年度完成日期对应的今年应该存在数据的日期;
                                    if (Convert.ToDateTime(curdate) <= Convert.ToDateTime(equaldate).AddYears(1))
                                    {
                                        cmodel.deductpoint = 0;
                                        cmodel.score = Convert.ToDecimal(entity.IndexScore); //得分项
                                    }
                                }
                            }
                        }
                        #endregion
                        #region 现场处置方案演练次数
                        else
                        {
                            string fhalfdate = string.Empty; //上半年日期
                            string shalfdate = string.Empty;  //下半年日期
                            string curdate = DateTime.Now.ToString("yyyy-MM-dd");
                            //获取上一年度的现场处置方案
                            sql = @"select  id,  drilltime  from mae_drillplanrecord a  where  to_char(a.drilltime,'yyyy') ='{0}'
                                   and a.createuserorgcode = '{1}' and   a.drilltypename ='现场处置方案'";

                            var prevDt = this.BaseRepository().FindTable(string.Format(sql, DateTime.Now.AddYears(-1).Year, argument.orgCode));
                            //获取当前年度的现场处置方案
                            var curDt = this.BaseRepository().FindTable(string.Format(sql, DateTime.Now.Year, argument.orgCode));
                            //上一年度存在现场处置方案
                            if (prevDt.Rows.Count > 0)
                            {
                                //上半年
                                string fstr = string.Format(" drilltime <= '{0}'", DateTime.Now.AddYears(-1).ToString("yyyy-06-30"));
                                DataRow[] fhalfrows = prevDt.Select(fstr, " drilltime desc");
                                if (fhalfrows.Count() >= int.Parse(argValue[0].ToString()))
                                {
                                    fhalfdate = fhalfrows[0]["drilltime"].ToString();   //获取对应的比对日期(上一年度上半年演练的日期)
                                }
                                //下半年
                                fstr = string.Format(" drilltime > '{0}'  and  drilltime <='{1}'", DateTime.Now.AddYears(-1).ToString("yyyy-06-30"), DateTime.Now.AddYears(-1).ToString("yyyy-12-31"));
                                DataRow[] shalfrows = prevDt.Select(fstr, " drilltime desc");
                                if (shalfrows.Count() >= int.Parse(argValue[0].ToString()))
                                {
                                    shalfdate = shalfrows[0]["drilltime"].ToString();   //获取对应的比对日期(上一年度上半年演练的日期)
                                }
                            }

                            //当前年度存在现场处置方案的数据
                            if (curDt.Rows.Count >= Convert.ToDecimal(argValue[0].ToString()))
                            {
                                //上半年
                                string fstr = string.Format(" drilltime <= '{0}'", DateTime.Now.ToString("yyyy-06-30"));
                                DataRow[] fhalfrows = curDt.Select(fstr, " drilltime desc");
                                if (fhalfrows.Count() < int.Parse(argValue[0].ToString()))
                                {
                                    downScore += (int.Parse(argValue[0].ToString()) - fhalfrows.Count()) / Convert.ToDecimal(argValue[2].ToString()) * Convert.ToDecimal(argValue[3].ToString());
                                }
                                //下半年
                                fstr = string.Format(" drilltime > '{0}'  and  drilltime <= '{1}'", DateTime.Now.ToString("yyyy-06-30"), DateTime.Now.ToString("yyyy-12-31"));
                                DataRow[] shalfrows = curDt.Select(fstr, " drilltime desc");
                                if (shalfrows.Count() < int.Parse(argValue[0].ToString()))
                                {
                                    downScore += (int.Parse(argValue[0].ToString()) - shalfrows.Count()) / Convert.ToDecimal(argValue[2].ToString()) * Convert.ToDecimal(argValue[3].ToString());   //获取对应的比对日期(上一年度上半年演练的日期)
                                }
                                cmodel.deductpoint = downScore > Convert.ToDecimal(entity.IndexScore) ? Convert.ToDecimal(entity.IndexScore) : downScore;
                                cmodel.score = Convert.ToDecimal(entity.IndexScore) - cmodel.deductpoint; //得分项
                            }
                            else //当前年度不存在数据,扣完所有分数
                            {
                                downScore = (Convert.ToDecimal(argValue[1].ToString()) - curDt.Rows.Count) / Convert.ToDecimal(argValue[2].ToString()) * Convert.ToDecimal(argValue[3].ToString());
                                cmodel.deductpoint = downScore > Convert.ToDecimal(entity.IndexScore) ? Convert.ToDecimal(entity.IndexScore) : downScore;
                                cmodel.score = Convert.ToDecimal(entity.IndexScore) - cmodel.deductpoint; //得分项
                                decimal getScore = 0;
                                if (!string.IsNullOrEmpty(fhalfdate))    //如果对比日期不为空,则表示存在往年记录
                                {
                                    //当前时间未超过 前一年度完成日期对应的今年应该存在数据的日期;
                                    if (Convert.ToDateTime(curdate) <= Convert.ToDateTime(fhalfdate).AddYears(1))
                                    {
                                        getScore += Convert.ToDecimal(argValue[3].ToString());  //得分
                                    }
                                }
                                if (!string.IsNullOrEmpty(shalfdate))    //如果对比日期不为空,则表示存在往年记录
                                {
                                    //表示存在往年上半年数据
                                    if (getScore > 0)
                                    {
                                        //当前时间未超过 前一年度完成日期对应的今年应该存在数据的日期;
                                        if (Convert.ToDateTime(curdate) <= Convert.ToDateTime(shalfdate).AddYears(1) && Convert.ToDateTime(curdate) > Convert.ToDateTime(fhalfdate).AddYears(1))
                                        {
                                            getScore += Convert.ToDecimal(argValue[3].ToString()); //得分
                                        }
                                    }
                                    else
                                    {
                                        string tdate = DateTime.Now.ToString("yyyy-06-30");
                                        //当前时间未超过 前一年度完成日期对应的今年应该存在数据的日期;
                                        if (Convert.ToDateTime(curdate) <= Convert.ToDateTime(shalfdate).AddYears(1) && Convert.ToDateTime(curdate) > Convert.ToDateTime(tdate))
                                        {
                                            getScore += Convert.ToDecimal(argValue[3].ToString()); //得分
                                        }
                                    }
                                }

                                if (getScore > 0)
                                {
                                    cmodel.deductpoint = Convert.ToDecimal(entity.IndexScore) - getScore;
                                    cmodel.score = getScore; //得分项
                                }
                            }
                        }
                        #endregion
                        break;
                    #endregion
                    #region 职业健康
                    case "06":
                        #region 职业危害因素检测
                        if (entity.IndexCode == "01")
                        {
                            string equaldate = string.Empty;
                            string curdate = DateTime.Now.ToString("yyyy-MM-dd");
                            //上一年度的职业危害因素检测
                            sql = @"select id,inspectiontime from v_inspection  where  to_char(inspectiontime,'yyyy') = '{0}' and createuserorgcode ='{1}' ";
                            var prevDt = this.BaseRepository().FindTable(string.Format(sql, DateTime.Now.AddYears(-1).Year, argument.orgCode));
                            //获取当前年度的职业危害因素检测
                            var curDt = this.BaseRepository().FindTable(string.Format(sql, DateTime.Now.Year, argument.orgCode));
                            //上一年度存在职业危害因素检测
                            if (prevDt.Rows.Count > 0)
                            {
                                equaldate = prevDt.Rows[0]["inspectiontime"].ToString();   //获取对应的比对日期(上一年演练的日期)
                            }

                            //当前年度存在职业危害因素检测的数据
                            if (curDt.Rows.Count >= Convert.ToDecimal(argValue[0].ToString()))
                            {
                                cmodel.deductpoint = 0;
                                cmodel.score = Convert.ToDecimal(entity.IndexScore); //得分项
                            }
                            else //当前年度满足要求,扣分
                            {
                                downScore = (Convert.ToDecimal(argValue[0].ToString()) - curDt.Rows.Count) / Convert.ToDecimal(argValue[1].ToString()) * Convert.ToDecimal(argValue[2].ToString());
                                cmodel.deductpoint = downScore > Convert.ToDecimal(entity.IndexScore) ? Convert.ToDecimal(entity.IndexScore) : downScore;
                                cmodel.score = Convert.ToDecimal(entity.IndexScore) - cmodel.deductpoint; //得分项
                                if (!string.IsNullOrEmpty(equaldate))    //如果对比日期不为空,则表示存在往年记录
                                {
                                    //当前时间超过 前一年度完成日期对应的今年应该存在数据的日期;
                                    if (Convert.ToDateTime(curdate) <= Convert.ToDateTime(equaldate).AddYears(1))
                                    {
                                        cmodel.deductpoint = 0;
                                        cmodel.score = Convert.ToDecimal(entity.IndexScore); //得分项
                                    }
                                }
                            }
                        }
                        #endregion
                        #region 职业病危害因素监测合格率
                        else
                        {
                            sql = string.Format(@"select count(1) from bis_hazarddetection a  where  a.createuserorgcode ='{0}'", argument.orgCode);
                            record = this.BaseRepository().FindObject(sql).ToInt();

                            string tsql = string.Format(@"select count(1) from bis_hazarddetection a  where a.createuserorgcode ='{0}' and isexcessive =0 ", argument.orgCode);
                            int trecord = this.BaseRepository().FindObject(tsql).ToInt();

                            decimal cvalue = record > 0 ? Convert.ToDecimal(argValue[0].ToString()) - Math.Round((Convert.ToDecimal(trecord.ToString()) / Convert.ToDecimal(record.ToString())) * 100, 2) : 0;

                            downScore = cvalue > 0 ? Math.Round(cvalue / Convert.ToDecimal(argValue[1].ToString()), 2) * Convert.ToDecimal(argValue[2].ToString()) : 0;

                            cmodel.deductpoint = downScore > Convert.ToDecimal(entity.IndexScore) ? Convert.ToDecimal(entity.IndexScore) : downScore;

                            cmodel.score = Convert.ToDecimal(entity.IndexScore) - cmodel.deductpoint;
                        }
                        #endregion
                        break;
                    #endregion
                    #region 人员管理
                    case "07":
                        #region 特种作业人员持证率
                        if (entity.IndexCode == "01")
                        {
                            sql = string.Format(@"select count(a.userid) from v_userinfo a 
                                            left join (
                                              select t.userid,certname,certnum,senddate,sendorgan,years,enddate,filepath from bis_certificate t left join v_userinfo u on t.userid=u.userid where  isspecial='是' and certname='特种作业操作证'
                                            ) b on a.userid=b.userid where  1=1  and isspecial='是'  and organizecode='{0}' and a.ispresence ='是'", argument.orgCode);
                            record = this.BaseRepository().FindObject(sql).ToInt();
                            string orgstr = string.Format(@"  departmentcode like '{0}%'", argument.orgCode);
                            decimal precent = certificateiservice.GetCertPercent(record, orgstr, "特种作业操作证"); //获取特种设备持证率
                            decimal cvalue = Convert.ToDecimal(argValue[0].ToString()) - precent; //获取标准持证率及实际持证率的差值
                            if (cvalue > 0)
                            {
                                downScore = Math.Round(cvalue / Convert.ToDecimal(argValue[1].ToString()), 2) * Convert.ToDecimal(argValue[2].ToString());
                                cmodel.deductpoint = downScore > Convert.ToDecimal(entity.IndexScore) ? Convert.ToDecimal(entity.IndexScore) : downScore;
                                cmodel.score = Convert.ToDecimal(entity.IndexScore) - cmodel.deductpoint;
                            }
                            else
                            {
                                cmodel.deductpoint = 0;
                                cmodel.score = Convert.ToDecimal(entity.IndexScore);
                            }
                        }
                        #endregion
                        #region 安全管理人员持证率
                        else if (entity.IndexCode == "02")
                        {
                            sql = string.Format(@"select  count(a.userid) from v_userinfo a 
                                            left join (
                                              select t.userid,certname,certnum,senddate,sendorgan,years,enddate,filepath from bis_certificate t left join v_userinfo u on t.userid=u.userid where  isspecial='否' and u.usertype ='安全管理人员' and certname='安全管理人员证'
                                            ) b on a.userid=b.userid where  1=1  and isspecial='否' and usertype ='安全管理人员' and organizecode='{0}' and a.ispresence ='是'", argument.orgCode);
                            record = this.BaseRepository().FindObject(sql).ToInt();
                            string orgstr = string.Format(@"  departmentcode like '{0}%'", argument.orgCode);
                            decimal precent = certificateiservice.GetCertPercent(record, orgstr, "安全管理人员证"); //获取特种设备持证率
                            decimal cvalue = record > 0 ? (Convert.ToDecimal(argValue[0].ToString()) - precent) : 0; //获取标准持证率及实际持证率的差值
                            if (cvalue > 0)
                            {
                                downScore = Math.Round(cvalue / Convert.ToDecimal(argValue[1].ToString()), 2) * Convert.ToDecimal(argValue[2].ToString());
                                cmodel.deductpoint = downScore > Convert.ToDecimal(entity.IndexScore) ? Convert.ToDecimal(entity.IndexScore) : downScore;
                                cmodel.score = Convert.ToDecimal(entity.IndexScore) - cmodel.deductpoint;
                            }
                            else
                            {
                                cmodel.deductpoint = 0;
                                cmodel.score = Convert.ToDecimal(entity.IndexScore);
                            }
                        }
                        #endregion
                        #region 人员积分
                        else
                        {
                            sql = string.Format(@" select count(1) from (select a.userid ,a.realname, 100 + sum(nvl(b.score,0)) score from base_user a 
                                    left join bis_userscore b on a.userid = b.userid where a.organizecode = '{0}'  and to_char(b.createdate,'yyyy') =  '{1}'  
                                    group by  a.userid ,a.realname ) a where a.score < '{2}' ", argument.orgCode, DateTime.Now.Year.ToString(), Convert.ToDecimal(argValue[0].ToString()));
                            record = this.BaseRepository().FindObject(sql).ToInt();
                            downScore = record * Convert.ToDecimal(argValue[1].ToString());
                            cmodel.deductpoint = downScore > Convert.ToDecimal(entity.IndexScore) ? Convert.ToDecimal(entity.IndexScore) : downScore;
                            cmodel.score = Convert.ToDecimal(entity.IndexScore) - cmodel.deductpoint;
                        }
                        #endregion
                        break;
                    #endregion
                    #region 特种设备管理
                    case "08":
                        #region 特种设备检测率
                        if (entity.IndexCode == "01")
                        {
                            sql = string.Format(@"select count(1) from bis_specialequipment a  where a.createuserorgcode ='{0}' and a.state=2 ", argument.orgCode);
                            //总数
                            record = this.BaseRepository().FindObject(sql).ToInt();
                            string sjsql = string.Format(@"select  count(a.id) from bis_specialequipment a where a.createuserorgcode ='{0}' and a.state=2 and  nextcheckdate >=  to_date('{1}','yyyy-mm-dd hh24:mi:ss') ", argument.orgCode, DateTime.Now.ToString());
                            //实际数
                            int sjrecord = this.BaseRepository().FindObject(sjsql).ToInt();

                            decimal precent = record > 0 ? Math.Round(sjrecord / Convert.ToDecimal(record) * 100, 2) : 100;
                            downScore = Math.Round((Convert.ToDecimal(argValue[0].ToString()) - precent) / Convert.ToDecimal(argValue[1].ToString()) * Convert.ToDecimal(argValue[2].ToString()), 2);
                            cmodel.deductpoint = downScore > Convert.ToDecimal(entity.IndexScore) ? Convert.ToDecimal(entity.IndexScore) : downScore;
                            cmodel.score = Convert.ToDecimal(entity.IndexScore) - cmodel.deductpoint;
                        }
                        #endregion
                        #region 特种设备维保率
                        else
                        {
                            sql = string.Format(@"select count(1) from bis_specialequipment a  where  state=2 and a.createuserorgcode ='{0}'", argument.orgCode);
                            //在用设备总数
                            record = this.BaseRepository().FindObject(sql).ToInt();
                            string sjsql = string.Format(@"select nvl(sum(b.numbers),0) from bis_specialequipment a 
                                    left join  (select count(1) numbers,equipmentid from bis_maintainingrecord where to_char(maintainingdate,'yyyy')='{0}' group by equipmentid ) b on a.id = b.equipmentid 
                                    where a.state=2 and  a.createuserorgcode ='{1}'", DateTime.Now.Year.ToString(), argument.orgCode);
                            //实际数
                            int sjrecord = this.BaseRepository().FindObject(sjsql).ToInt();

                            decimal precent = record > 0 ? Math.Round(sjrecord / Convert.ToDecimal(record) * 100, 2) : 100;
                            downScore = Math.Round((Convert.ToDecimal(argValue[0].ToString()) - precent) / Convert.ToDecimal(argValue[1].ToString()) * Convert.ToDecimal(argValue[2].ToString()), 2);
                            downScore = downScore < 0 ? 0 : downScore;
                            cmodel.deductpoint = downScore > Convert.ToDecimal(entity.IndexScore) ? Convert.ToDecimal(entity.IndexScore) : downScore;
                            cmodel.score = Convert.ToDecimal(entity.IndexScore) - cmodel.deductpoint;
                        }
                        #endregion
                        break;
                    #endregion
                    #region 安全教育培训
                    case "09":
                        #region 安全培训率
                        if (entity.IndexCode == "01")
                        {
                            cmodel.deductpoint = 0;
                            cmodel.score = Convert.ToDecimal(entity.IndexScore);
                        }
                        #endregion
                        #region 培训合格率
                        else
                        {
                            cmodel.deductpoint = 0;
                            cmodel.score = Convert.ToDecimal(entity.IndexScore);
                        }
                        #endregion
                        break;
                    #endregion
                    #region 反违章管理
                    case "10":
                        string lllegallevelname = string.Empty;
                        #region 逾期未整改严重违章数量
                        if (entity.IndexCode == "01")
                        {
                            lllegallevelname = "严重违章";
                        }
                        #endregion
                        #region 逾期未整改较严重违章数
                        else if (entity.IndexCode == "02")
                        {
                            lllegallevelname = "较严重违章";
                        }
                        #endregion
                        #region 逾期未整改的一般违章数量
                        else
                        {
                            lllegallevelname = "一般违章";
                        }
                        #endregion

                        sql = string.Format(@"select count(1) from v_lllegalbaseinfo where  reformdeptcode like '{0}%' and  flowstate = '违章整改' 
                        and lllegallevelname ='{1}' and  to_date('{2}','yyyy-mm-dd hh24:mi:ss') > reformdeadline + 1", argument.orgCode, lllegallevelname, DateTime.Now.ToString()); //重大危险源总数,截止目前
                        record = this.BaseRepository().FindObject(sql).ToInt();

                        if (record > 0)
                        {
                            downScore = record * Convert.ToDecimal(argValue[0].ToString());
                            cmodel.deductpoint = downScore > Convert.ToDecimal(entity.IndexScore) ? Convert.ToDecimal(entity.IndexScore) : downScore;
                            cmodel.score = Convert.ToDecimal(entity.IndexScore) - cmodel.deductpoint;
                        }
                        else
                        {
                            cmodel.deductpoint = 0;
                            cmodel.score = Convert.ToDecimal(entity.IndexScore);
                        }
                        break;
                        #endregion
                }
                cmodel.deductpoint = Math.Round(cmodel.deductpoint, 2);
                cmodel.score = Math.Round(cmodel.score, 2);
                rlist.Add(cmodel); //添加对象到集合中
            }
            return rlist;
        }
        #endregion



        #region 获取曝光信息
        /// <summary>
        /// 获取曝光信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetExposureInfo(ERCHTMS.Code.Operator user)
        {

            string sql = string.Format(@"select a.* from (
                                         select a.id bid, to_char(a.createdate,'yyyy-MM-dd') viewdate,to_char(a.hiddescribe) title, b.filepath,0 dznum, 0 readnum ,2 ranktype ,to_char(a.workstream) workstream ,to_char(a.id)  hiddenid,  to_char(a.hidcode)  problemid, to_char(a.hidrankname)  rankname   from v_basehiddeninfo a
                                        left join  (
                                          select a.id,wm_concat((case when b.filepath is not null then ('{0}'||substr(b.filepath,2)) else '' end)) filepath   from bis_htbaseinfo a
                                          left join base_fileinfo b on a.hidphoto = b.recid  group by a.id
                                        ) b on  a.id = b.id  where a.exposurestate ='是' and a.hiddepart ='{1}' order by a.createdate desc 
                                        ) a where rownum =1  
                                        union
                                        select b.*  from (
                                        select  a.id bid,to_char(a.createdate,'yyyy-MM-dd') viewdate,to_char(a.lllegaldescribe) title, b.filepath,0 dznum, 0 readnum ,3 ranktype ,'' workstream, '' hiddenid, '' problemid, ''  rankname   from  v_lllegalbaseinfo a
                                         left join  (
                                          select a.id,wm_concat((case when b.filepath is not null then ('{0}'||substr(b.filepath,2)) else '' end)) filepath from bis_lllegalregister a
                                          left join base_fileinfo b on a.lllegalpic = b.recid  group by a. id
                                        ) b on  a.id = b.id  where  a.isexposure ='1' and a.belongdepartid  ='{1}' order by a.createdate desc  
                                         ) b  where rownum =1     
                                        union
                                        select c.* from (
                                        select   a.id bid, to_char(a.releasetime,'yyyy-MM-dd') viewdate,to_char(a.title) title, b.filepath,(select count(e.id) from BIS_ReadUserManage e where e.moduleid=a.id and e.isdz='3') as dznum,
                                        (select nvl(sum(e.isyd),0)  from BIS_ReadUserManage e where e.moduleid=a.id ) as readnum ,0 ranktype  ,'' workstream, '' hiddenid, '' problemid, ''  rankname   from BIS_SecurityRedList a 
                                         left join  (
                                          select a.id,wm_concat((case when b.filepath is not null then ('{0}'||substr(b.filepath,2)) else '' end)) filepath from BIS_SecurityRedList a
                                          left join base_fileinfo b on a.id = b.recid  group by a. id
                                        ) b on  a.id = b.id 
                                         where  a.IsSend='0' and a.STATE = '0' and a.createuserorgcode ='{2}'  order by a.RELEASETIME desc ) c   where rownum =1     
                                         union
                                         select d.* from (
                                        select  a.id bid,to_char(a.releasetime,'yyyy-MM-dd') viewdate,to_char(a.title) title, b.filepath ,0 dznum,
                                        (select nvl(sum(e.isyd),0)  from BIS_ReadUserManage e where e.moduleid=a.id ) as readnum,1 ranktype  ,'' workstream, '' hiddenid, '' problemid, ''  rankname    from BIS_SecurityRedList a 
                                            left join  (
                                            select a.id,wm_concat((case when b.filepath is not null then ('{0}'||substr(b.filepath,2)) else '' end)) filepath from BIS_SecurityRedList a
                                            left join base_fileinfo b on a.id = b.recid  group by a. id
                                        ) b on  a.id = b.id   where  a.IsSend='0' and a.STATE = '1' and a.createuserorgcode ='{2}'  order by a.RELEASETIME desc ) d  where rownum =1 ", idataitemdetailservice.GetItemValue("imgUrl"), user.OrganizeId, user.OrganizeCode);

            return this.BaseRepository().FindTable(sql);
        }
        #endregion
        #endregion

        #region 华昇大屏实时工作
        /// <summary>
        /// 华昇大屏实时工作
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<RealTimeWorkModel> GetRealTimeWork(Operator user)
        {
            List<RealTimeWorkModel> data = new List<RealTimeWorkModel>();
            List<RealTimeWorkModel> temp = new List<RealTimeWorkModel>();
            string sql = "";
            //外包工程停工
            sql = string.Format("select a.id,(b.engineername || '停工') WorkDescribe ,STOPTIME as Time,c.fullname deptname,c.departmentid DeptId,c.encode deptcode,'外包工程停工' as ModuleType from EPG_STOPRETURNWORK a left join epg_outsouringengineer b  on a.outengineerid=b.id left join base_department c on a.outprojectid=c.departmentid where a.iscommit=1 and to_char(a.stoptime,'yyyy-mm-dd')='{0}'", DateTime.Now.ToString("yyyy-MM-dd"));
            temp = DbFactory.Base().FindList<RealTimeWorkModel>(sql).ToList();
            data = data.AsEnumerable().Union(temp).ToList();
            //外包工程复工
            sql = string.Format("select a.id,(b.engineername || '复工') WorkDescribe ,APPLYRETURNTIME as Time,c.fullname deptname,c.departmentid DeptId,c.encode deptcode,'外包工程复工' as ModuleType from EPG_RETURNTOWORK a left join epg_outsouringengineer b  on a.outengineerid=b.id left join base_department c on a.outprojectid=c.departmentid where a.iscommit=1 and to_char(a.APPLYRETURNTIME,'yyyy-mm-dd')='{0}'", DateTime.Now.ToString("yyyy-MM-dd"));
            temp = DbFactory.Base().FindList<RealTimeWorkModel>(sql).ToList();
            data = data.AsEnumerable().Union(temp).ToList();
            //外包工程开工申请
            sql = string.Format("select a.id,(b.engineername || '计划开工 ' || b.ENGINEERLETPEOPLE || ':' || b.ENGINEERLETPEOPLEPHONE) WorkDescribe ,APPLYRETURNTIME as Time,c.fullname deptname,c.departmentid DeptId,c.encode deptcode,'外包工程开工' as ModuleType from epg_startapplyfor a left join epg_outsouringengineer b  on a.outengineerid=b.id left join base_department c on a.outprojectid=c.departmentid where a.iscommit=1 and a.isover=1 and  to_char(a.APPLYRETURNTIME,'yyyy-mm-dd')='{0}'", DateTime.Now.ToString("yyyy-MM-dd"));
            temp = DbFactory.Base().FindList<RealTimeWorkModel>(sql).ToList();
            data = data.AsEnumerable().Union(temp).ToList();
            //安全风险管控
            sql = string.Format("select a.id, (a.areaname || '、' || a.dangersource || '、' || a.grade) WorkDescribe,a.createdate as time,b.fullname deptname,b.departmentid DeptId,b.encode deptcode,'安全风险管控' as ModuleType  from bis_riskassess a left join base_department b on a.createuserdeptcode=b.encode where a.gradeval in(1,2) and to_char(a.createdate,'yyyy-mm-dd')='{0}'", DateTime.Now.ToString("yyyy-MM-dd"));
            temp = DbFactory.Base().FindList<RealTimeWorkModel>(sql).ToList();
            data = data.AsEnumerable().Union(temp).ToList();
            ////高风险作业开始作业
            //sql = string.Format("select a.id, ('开始作业:' || a.WorkContent) WorkDescribe,a.realityworkstarttime as time,b.fullname deptname,b.departmentid DeptId,b.encode deptcode,'高风险作业开始作业' as ModuleType from bis_highriskcommonapply a left join base_department b on a.workdeptid=b.departmentid where a.applystate=5 and to_char(a.realityworkstarttime,'yyyy-mm-dd')='{0}'", DateTime.Now.ToString("yyyy-MM-dd"));
            //temp = DbFactory.Base().FindList<RealTimeWorkModel>(sql).ToList();
            //data = data.AsEnumerable().Union(temp).ToList();
            ////高风险作业结束作业
            //sql = string.Format("select a.id, ('结束作业:' || a.WorkContent) WorkDescribe,a.realityworkendtime as time,b.fullname deptname,b.departmentid DeptId,b.encode deptcode,'高风险作业结束作业' as ModuleType from bis_highriskcommonapply a left join base_department b on a.workdeptid=b.departmentid where a.applystate=5 and to_char(a.realityworkendtime,'yyyy-mm-dd')='{0}'", DateTime.Now.ToString("yyyy-MM-dd"));
            //temp = DbFactory.Base().FindList<RealTimeWorkModel>(sql).ToList();
            //data = data.AsEnumerable().Union(temp).ToList();
            //高危作业开始作业
            sql = string.Format("select a.id, ('开始作业:' || a.jobcontent) WorkDescribe,a.REALITYJOBSTARTTIME as time,a.jobdeptname deptname,a.jobdeptid deptid,a.jobdeptcode deptcode,'高危作业开始作业' as ModuleType from BIS_JOBSAFETYCARDAPPLY a where  to_char(a.REALITYJOBSTARTTIME,'yyyy-mm-dd')='{0}'", DateTime.Now.ToString("yyyy-MM-dd"));
            temp = DbFactory.Base().FindList<RealTimeWorkModel>(sql).ToList();
            data = data.AsEnumerable().Union(temp).ToList();
            //高危作业结束作业
            sql = string.Format("select a.id, ('结束作业:' || a.jobcontent) WorkDescribe,a.REALITYJOBENDTIME as time,a.jobdeptname deptname,a.jobdeptid deptid,a.jobdeptcode deptcode,'高危作业结束作业' as ModuleType from BIS_JOBSAFETYCARDAPPLY a where  to_char(a.REALITYJOBENDTIME,'yyyy-mm-dd')='{0}'", DateTime.Now.ToString("yyyy-MM-dd"));
            temp = DbFactory.Base().FindList<RealTimeWorkModel>(sql).ToList();
            data = data.AsEnumerable().Union(temp).ToList();
            //应急管理
            sql = string.Format("select a.id, case when a.isconnectplan='是' then a.DRILLPLANNAME else  a.DRILLPLANNAME || '演练' end  as   WorkDescribe,a.DRILLTIME as time,a.DEPARTNAME deptname,a.DEPARTID DeptId,'应急演练' as ModuleType from mae_drillplanrecord a where a.iscommit=1 and to_char(a.DRILLTIME,'yyyy-mm-dd')='{0}'", DateTime.Now.ToString("yyyy-MM-dd"));
            temp = DbFactory.Base().FindList<RealTimeWorkModel>(sql).ToList();
            data = data.AsEnumerable().Union(temp).ToList();
            //隐患登记(以登记时间为基准)
            sql = string.Format(@"select  a.createuserid id,(b.realname||'登记隐患'||to_char(nvl(a.pnum,0))||'条') WorkDescribe,a.createdate time ,c.fullname deptname ,c.departmentid DeptId,c.encode deptcode ,'违章/隐患信息' as ModuleType  from 
                                (select count(1) pnum,createuserid,createdate from  bis_htbaseinfo a where to_char(createdate,'yyyy-MM-dd')='{0}' group by createuserid,createdate) a
                                left join base_user b on a.createuserid = b.userid 
                                left join base_department c on b.departmentid = c.departmentid ", DateTime.Now.ToString("yyyy-MM-dd"));
            temp = DbFactory.Base().FindList<RealTimeWorkModel>(sql).ToList();
            data = data.AsEnumerable().Union(temp).ToList();
            //隐患整改(以验收时间为基准)
            sql = string.Format(@"select  a.changeperson id,(b.realname||'整改隐患'||to_char(nvl(a.pnum,0))||'条') WorkDescribe,a.acceptdate time ,c.fullname deptname ,c.departmentid DeptId,c.encode deptcode ,'违章/隐患信息' as ModuleType  from 
                                (select count(a.id) pnum,c.changeperson,b.acceptdate from  bis_htbaseinfo a 
                                left join v_htacceptinfo b on a.hidcode = b.hidcode 
                                left join v_htchangeinfo c on a.hidcode = c.hidcode
                                where b.acceptstatus =1 and a.workstream ='整改结束' and  to_char(b.acceptdate,'yyyy-MM-dd')='{0}' group by c.changeperson,b.acceptdate) a
                                left join base_user b on a.changeperson = b.userid 
                                left join base_department c on b.departmentid = c.departmentid  ", DateTime.Now.ToString("yyyy-MM-dd"));
            temp = DbFactory.Base().FindList<RealTimeWorkModel>(sql).ToList();
            data = data.AsEnumerable().Union(temp).ToList();

            //违章登记(以登记时间为基准)
            sql = string.Format(@"select  a.createuserid id,(b.realname||'登记违章'||to_char(nvl(a.pnum,0))||'条') WorkDescribe,a.createdate time ,c.fullname deptname ,c.departmentid DeptId,c.encode deptcode ,'违章/隐患信息' as ModuleType  from 
                                (select count(1) pnum,createuserid,createdate from  bis_lllegalregister a   where  to_char(createdate,'yyyy-MM-dd')='{0}' group by createuserid,createdate) a
                                left join base_user b on a.createuserid = b.userid 
                                left join base_department c on b.departmentid = c.departmentid ", DateTime.Now.ToString("yyyy-MM-dd"));
            temp = DbFactory.Base().FindList<RealTimeWorkModel>(sql).ToList();
            data = data.AsEnumerable().Union(temp).ToList();
            //违章整改(以验收时间为基准)
            sql = string.Format(@"select  a.reformpeopleid id,(b.realname||'整改违章'||to_char(nvl(a.pnum,0))||'条') WorkDescribe,a.accepttime time ,c.fullname deptname ,c.departmentid DeptId,c.encode deptcode ,'违章/隐患信息' as ModuleType  from 
                                (
                                  select count(a.id) pnum,c.reformpeopleid ,b.accepttime from  bis_lllegalregister a 
                                  left join v_lllegalacceptinfo b on a.id = b.lllegalid 
                                  left join v_lllegalreforminfo c on a.id = c.lllegalid
                                  where b.acceptresult =1 and a.flowstate ='流程结束' and  to_char(b.accepttime,'yyyy-MM-dd')='{0}' group by c.reformpeopleid,b.accepttime
                                ) a
                                left join base_user b on instr(a.reformpeopleid,b.userid)>0 
                                left join base_department c on b.departmentid = c.departmentid ", DateTime.Now.ToString("yyyy-MM-dd"));
            temp = DbFactory.Base().FindList<RealTimeWorkModel>(sql).ToList();
            data = data.AsEnumerable().Union(temp).ToList();
            return data;
        }
        #endregion

        #region 华昇大屏预警中心
        /// <summary>
        /// 华昇大屏预警中心
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<RealTimeWorkModel> GetWarningCenterWork(Operator user)
        {
            List<RealTimeWorkModel> data = new List<RealTimeWorkModel>();
            List<RealTimeWorkModel> temp = new List<RealTimeWorkModel>();
            IDatabase db = DbFactory.Base();
            string sql = "";
            //重大风险
            sql = @"select a.id, ('重大危险源' || a.dangersource || '未监控') WorkDescribe,a.createdate as time,'重大危险源' as ModuleType,('所属区域' || a.areaname) as Remark,0 as Status  from bis_riskassess a  where a.gradeval =1";
            temp = db.FindList<RealTimeWorkModel>(sql).ToList();
            data = data.AsEnumerable().Union(temp).ToList();
            //设备设施
            sql = string.Format(@"select '设备定检' ModuleType,'08:30' time,ControlDeptCode,(District || '区域' ||  EquipmentName || '超期未检') WorkDescribe,('已超过检验日期：' || to_char(NextCheckDate,'yyyy-mm-dd')) remark from BIS_SPECIALEQUIPMENT t where NextCheckDate<sysdate
union all 
select '设备定检' ModuleType,'08:30' time,ControlDeptCode,(District || '区域' ||  EquipmentName || '即将检验到期') WorkDescribe,('下次检验日期：' || to_char(NextCheckDate,'yyyy-mm-dd')) remark from BIS_SPECIALEQUIPMENT t where NextCheckDate>sysdate and NextCheckDate<=to_date('{0}','yyyy-mm-dd')", DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd"));
            temp = DbFactory.Base().FindList<RealTimeWorkModel>(sql).ToList();
            data = data.Concat(temp).ToList();

            //隐患到期未整改
            sql = string.Format(@"select to_char(a.changedutydepartcode) id,(to_char(a.changedutydepartname)|| to_char(nvl(a.pnum,0)) ||'条隐患即将逾期未整改') WorkDescribe,a.changedeadine time,'隐患到期' ModuleType,to_char(a.changedeadine,'yyyy-MM-dd')||'前整改' Remark from ( 
                      select b.changedutydepartcode ,b.changedutydepartname ,count(a.id) pnum,b.changedeadine from bis_htbaseinfo a
                      left join v_htchangeinfo b on a.hidcode = b.hidcode where a.workstream ='隐患整改' and b.changedeadine + 1 > to_date('{0}','yyyy-mm-dd hh24:mi:ss') group by b.changedutydepartcode ,b.changedutydepartname,b.changedeadine
                ) a", DateTime.Now.ToString("yyyy-MM-dd"));
            temp = DbFactory.Base().FindList<RealTimeWorkModel>(sql).ToList();
            data = data.Concat(temp).ToList();

            //隐患逾期未整改
            sql = string.Format(@"select to_char(a.changedutydepartcode) id,(to_char(a.changedutydepartname)|| to_char(nvl(a.pnum,0)) ||'条隐患逾期未整改') WorkDescribe,a.changedeadine time,'隐患逾期' ModuleType,to_char(a.changedeadine,'yyyy-MM-dd')||'前整改' Remark from ( 
                      select b.changedutydepartcode ,b.changedutydepartname ,count(a.id) pnum,b.changedeadine from bis_htbaseinfo a
                      left join v_htchangeinfo b on a.hidcode = b.hidcode where a.workstream ='隐患整改' and b.changedeadine + 1 <= to_date('{0}','yyyy-mm-dd hh24:mi:ss') group by b.changedutydepartcode ,b.changedutydepartname,b.changedeadine
                ) a", DateTime.Now.ToString("yyyy-MM-dd"));
            temp = DbFactory.Base().FindList<RealTimeWorkModel>(sql).ToList();
            data = data.Concat(temp).ToList();


            //违章到期未整改
            sql = string.Format(@"select to_char(a.reformdeptcode) id,(to_char(a.reformdeptname)|| to_char(nvl(a.pnum,0)) ||'条违章即将逾期未整改') WorkDescribe,a.reformdeadline time,'违章到期' ModuleType,to_char(a.reformdeadline,'yyyy-MM-dd')||'前整改' Remark from ( 
                    select b.reformdeptcode ,b.reformdeptname ,count(a.id) pnum,b.reformdeadline from bis_lllegalregister a
                    left join v_lllegalreforminfo b on a.id = b.lllegalid where a.flowstate ='违章整改' and b.reformdeadline + 1 > to_date('{0}','yyyy-mm-dd hh24:mi:ss') group by b.reformdeptcode ,b.reformdeptname,b.reformdeadline
                ) a", DateTime.Now.ToString("yyyy-MM-dd"));
            temp = DbFactory.Base().FindList<RealTimeWorkModel>(sql).ToList();
            data = data.Concat(temp).ToList();

            //违章逾期未整改
            sql = string.Format(@"select to_char(a.reformdeptcode) id,(to_char(a.reformdeptname)|| to_char(nvl(a.pnum,0)) ||'条违章逾期未整改') WorkDescribe,a.reformdeadline time,'违章逾期' ModuleType,to_char(a.reformdeadline,'yyyy-MM-dd')||'前整改' Remark from ( 
                    select b.reformdeptcode ,b.reformdeptname ,count(a.id) pnum,b.reformdeadline from bis_lllegalregister a
                    left join v_lllegalreforminfo b on a.id = b.lllegalid where a.flowstate ='违章整改' and b.reformdeadline + 1 <= to_date('{0}','yyyy-mm-dd hh24:mi:ss') group by b.reformdeptcode ,b.reformdeptname,b.reformdeadline
                ) a", DateTime.Now.ToString("yyyy-MM-dd"));
            temp = DbFactory.Base().FindList<RealTimeWorkModel>(sql).ToList();
            data = data.Concat(temp).ToList();

            return data;

        }
        #endregion

        /// <summary>
        /// 安措计划代办数量
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        public int GetSafeMeasureNum(Operator user)
        {
            int count = 0;
            string sql = @" select FlowRoleName from BIS_SAFEMEASURE where iscommit='1' and isover='0' and FLOWDEPT=:FlowDept";
            DataTable dt = DbFactory.Base().FindTable(sql, new DbParameter[] { DbParameters.CreateDbParameter(":FlowDept", user.DeptId) });
            foreach (DataRow dr in dt.Rows)
            {
                if (!string.IsNullOrWhiteSpace(dr["flowrolename"].ToString()))
                {

                    var roleArr = user.RoleName.Split(','); //当前人员角色
                    var roleName = dr["flowrolename"].ToString(); //审核橘色
                    for (var i = 0; i < roleArr.Length; i++)
                    {
                        //满足审核部门同当前人部门id一致，切当前人角色存在与审核角色中
                        if (roleName.IndexOf(roleArr[i]) >= 0)
                        {
                            count++;
                            break;
                        }
                    }
                }
            }
            return count;
        }

        #region 国电汉川对接待办推送
        /// <summary>
        /// 国电汉川待办对接(接收)
        /// </summary>
        /// <param name="entity"></param>
        public void GdhcDbsxSyncJS(GdhcDbsxEntity entity)
        {
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            wc.Headers.Add("Content-Type", "application/json");
            wc.Encoding = System.Text.Encoding.UTF8;
            Object obj = new
            {
                syscode = entity.syscode,
                flowid = entity.flowid,
                requestname = entity.requestname,
                workflowname = entity.workflowname,
                nodename = entity.nodename,
                pcurl = entity.pcurl,
                appurl = entity.appurl,
                creator = entity.creator,
                createdatetime = entity.createdatetime,
                receiver = entity.receiver,
                receivedatetime = entity.receivedatetime,
                requestlevel = entity.requestlevel
            };
            if (!string.IsNullOrWhiteSpace(entity.LogUrl))
            {
                if (!System.IO.Directory.Exists(entity.LogUrl))
                {
                    System.IO.Directory.CreateDirectory(entity.LogUrl);
                }
            }

            try
            {
                System.IO.File.AppendAllText(entity.LogUrl + "/" + DateTime.Now.ToString("yyyyMMdd") + ".txt", "开始推送接收待办：参数*********" + Newtonsoft.Json.JsonConvert.SerializeObject(obj) + "******\r\n");
                string result = wc.UploadString(new Uri(entity.GdhcUrl + "/rest/ofs/ReceiveTodoRequestByJson"), Newtonsoft.Json.JsonConvert.SerializeObject(obj));
                System.IO.File.AppendAllText(entity.LogUrl + "/" + DateTime.Now.ToString("yyyyMMdd") + ".txt", "推送接收待办：参数*********" + Newtonsoft.Json.JsonConvert.SerializeObject(obj) + "******\r\n" + "返回结果：*******" + result + "********\r\n");
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(entity.LogUrl + "/" + DateTime.Now.ToString("yyyyMMdd") + ".txt", "推送接收待办失败：参数*********" + Newtonsoft.Json.JsonConvert.SerializeObject(obj) + "********异常说明*******" + ex.ToString() + "********\r\n");
            }
        }

        /// <summary>
        /// 国电汉川待办对接（已办）
        /// </summary>
        /// <param name="entity"></param>
        public void GdhcDbsxSyncYB(GdhcDbsxEntity entity)
        {
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            wc.Headers.Add("Content-Type", "application/json");
            wc.Encoding = System.Text.Encoding.UTF8;
            Object obj = new
            {
                syscode = entity.syscode,
                flowid = entity.flowid,
                requestname = entity.requestname,
                workflowname = entity.workflowname,
                nodename = entity.nodename,
                receiver = entity.receiver,
                requestlevel = entity.requestlevel
            };
            if (!string.IsNullOrWhiteSpace(entity.LogUrl))
            {
                if (!System.IO.Directory.Exists(entity.LogUrl))
                {
                    System.IO.Directory.CreateDirectory(entity.LogUrl);
                }
            }
            try
            {
                string result = wc.UploadString(new Uri(entity.GdhcUrl + "/rest/ofs/ProcessDoneRequestByJson"), Newtonsoft.Json.JsonConvert.SerializeObject(obj));
                System.IO.File.AppendAllText(entity.LogUrl + "/" + DateTime.Now.ToString("yyyyMMdd") + ".txt", "推送已办待办成功：参数*********" + Newtonsoft.Json.JsonConvert.SerializeObject(obj) + "******\r\n" + "返回结果：*******" + result + "********\r\n");
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(entity.LogUrl + "/" + DateTime.Now.ToString("yyyyMMdd") + ".txt", "推送已办待办失败：参数*********" + Newtonsoft.Json.JsonConvert.SerializeObject(obj) + "********异常说明*******" + ex.ToString() + "********\r\n");
            }
        }
        /// <summary>
        /// 国电汉川待办对接（办结）
        /// </summary>
        /// <param name="entity"></param>
        public void GdhcDbsxSyncBJ(GdhcDbsxEntity entity)
        {
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            wc.Headers.Add("Content-Type", "application/json");
            wc.Encoding = System.Text.Encoding.UTF8;
            Object obj = new
            {
                syscode = entity.syscode,
                flowid = entity.flowid,
                requestname = entity.requestname,
                workflowname = entity.workflowname,
                nodename = entity.nodename,
                receiver = entity.receiver,
                requestlevel = entity.requestlevel
            };
            if (!string.IsNullOrWhiteSpace(entity.LogUrl))
            {
                if (!System.IO.Directory.Exists(entity.LogUrl))
                {
                    System.IO.Directory.CreateDirectory(entity.LogUrl);
                }
            }
            try
            {
                string result = wc.UploadString(new Uri(entity.GdhcUrl + "/rest/ofs/ProcessOverRequestByJson"), Newtonsoft.Json.JsonConvert.SerializeObject(obj));
                System.IO.File.AppendAllText(entity.LogUrl + "/" + DateTime.Now.ToString("yyyyMMdd") + ".txt", "推送办结待办成功：参数*********" + Newtonsoft.Json.JsonConvert.SerializeObject(obj) + "******\r\n" + "返回结果：*******" + result + "********\r\n");
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(entity.LogUrl + "/" + DateTime.Now.ToString("yyyyMMdd") + ".txt", "推送办结待办失败：参数*********" + Newtonsoft.Json.JsonConvert.SerializeObject(obj) + "********异常说明*******" + ex.ToString() + "********\r\n");
            }
        }
        #endregion

    }
}
