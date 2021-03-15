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
using ERCHTMS.Code;
using ERCHTMS.IService.SystemManage;
using ERCHTMS.Service.SystemManage;
using ERCHTMS.Entity.SystemManage;
using System.Text;
using System.Collections;

namespace ERCHTMS.Service.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：隐患基本信息表
    /// </summary>
    public class HTBaseInfoService : RepositoryFactory<HTBaseInfoEntity>, HTBaseInfoIService
    {

        private IDepartmentService Idepartmentservice = new DepartmentService();
        private IDataItemDetailService idataitemdetailservice = new DataItemDetailService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<HTBaseInfoEntity> GetList(string queryJson)
        {
            return this.BaseRepository().FindList(" select * from BIS_HTBASEINFO where 1=1 " + queryJson).ToList();
        }
        #endregion

        #region 通过隐患编码获取隐患集合
        /// <summary>
        /// 通过隐患编码获取隐患集合
        /// </summary>
        /// <param name="hidcode"></param>
        /// <returns></returns>
        public IList<HTBaseInfoEntity> GetListByCode(string hidcode)
        {
            return this.BaseRepository().IQueryable().ToList().Where(p => p.HIDCODE == hidcode).ToList();
        }
        #endregion

        #region 隐患检查集合

        /// <summary>
        /// 隐患检查集合
        /// </summary>
        /// <param name="checkId"></param>
        /// <param name="checkman"></param>
        /// <returns></returns>
        public DataTable GetList(string checkId, string checkman, string districtcode, string workstream)
        {
            string sql = @"select t.id,t.hidcode,e.changeperson,t.hiddepart,t.addtype ,t.hidrank,t.isselfchange,t.upsubmit,t.majorclassify,t.hidtype,t.hidbmid ,e.isappoint,t.createuserid  from bis_htbaseinfo t 
                                                inner join v_htchangeinfo e on t.hidcode=e.hidcode 
                                                where 1=1";
            if (!string.IsNullOrEmpty(checkId))
            {
                sql += string.Format(" and t.safetycheckobjectid = '{0}'", checkId);
            }
            if (!string.IsNullOrEmpty(checkman))
            {
                sql += string.Format(" and t.createuserid='{0}' ", checkman);
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
        #endregion

        #region 通过当前用户获取对应隐患的隐患描述(取前十个)
        /// <summary>
        /// 通过当前用户获取对应隐患的隐患描述
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable GetDescribeListByUserId(string userId, string hiddescribe)
        {

            string sql = string.Empty;

            string strwhere = " 1=1 ";

            if (!hiddescribe.IsEmpty())
            {
                strwhere += string.Format(@" and hiddescribe like '%{0}%'", hiddescribe);
            }

            if (DbHelper.DbType == DatabaseType.MySql)
            {
                sql = string.Format(@"select a.createuserdeptcode,a.createuserorgcode,a.createuserid, a.createdate,
                                       a.checktypename,a.hidtypename,a.hidrankname,a.checkdepartname,a.isgetafter,              
                                      a.id,a.hidproject,a.checktype, a.hidtype,a.hidrank,a.hidplace,a.hidpoint,a.workstream ,a.addtype ,
                                    c.postponedept ,c.postponedeptname ,a.hiddescribe,c.changemeasure   from v_htbaseinfo a
                                                                            left join v_workflow b on a.id = b.id 
                                                                            left join v_htchangeinfo c on a.hidcode = c.hidcode
                                                                            left join v_htacceptinfo d on a.hidcode = d.hidcode
                                                                             where {0} and createuserid ='{1}' and a.hiddescribe is not null and c.changemeasure  is not null   order by a.createdate desc  limit 0,10 ", strwhere, userId);
            }
            else
            {
                sql = string.Format(@"select a.*  from (select a.createuserdeptcode,a.createuserorgcode,a.createuserid, a.createdate,
                                       a.checktypename,a.hidtypename,a.hidrankname,a.checkdepartname,a.isgetafter,              
                                      a.id,a.hidproject,a.checktype, a.hidtype,a.hidrank,a.hidplace,a.hidpoint,a.workstream ,a.addtype ,
                                    c.postponedept ,c.postponedeptname ,a.hiddescribe,c.changemeasure  ,row_number() over( order by a.createdate desc) as rn  from v_htbaseinfo a
                                                                            left join v_workflow b on a.id = b.id 
                                                                            left join v_htchangeinfo c on a.hidcode = c.hidcode
                                                                            left join v_htacceptinfo d on a.hidcode = d.hidcode
                                                                             where {0}  and createuserid ='{1}' and a.hiddescribe is not null and c.changemeasure  is not null  order by a.createdate desc
                                                                             ) a  where rn <=10  order by createdate desc ", strwhere, userId);
            }



            return this.BaseRepository().FindTable(sql);
        }
        #endregion

        #region 获取通用查询分页
        /// <summary>
        /// 获取通用查询分页
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public DataTable GetGeneralQuery(string sql, Pagination pagination)
        {
            var dt = this.BaseRepository().FindTable(sql, pagination);
            return dt;
        }

        /// <summary>
        /// 获取通用查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public DataTable GetGeneralQueryBySql(string sql)
        {
            var dt = this.BaseRepository().FindTable(sql);
            return dt;
        }
        #endregion

        #region 获取实体
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public HTBaseInfoEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion

        #region 违章列表
        /// <summary>
        /// 违章列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetRulerPageList(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();

            string sql = string.Format("select t.ID,t.CHECKDATE,t.HIDDESCRIBE,e.CHANGEMEASURE from BIS_HTBASEINFO t left join v_htchangeinfo e on t.HIDCODE=e.HIDCODE where 1=1 ");
            pagination.p_fields = "ID,FINDDATE,HIDDESCRIBE,CHANGEMEASURE";
            pagination.p_kid = "ID";
            //查询条件
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

        public string GetCheckIds(string id)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dt = BaseRepository().FindTable(string.Format("select id from BIS_SAFTYCHECKDATARECORD where rid='{0}'", id));
            foreach (DataRow dr in dt.Rows)
            {
                sb.AppendFormat("{0},", dr[0].ToString());
                sb.AppendFormat("{0},", GetCheckIds(dr[0].ToString()));
            }
            return sb.ToString().Trim(',').Replace(",,", ",");
        }

        #region  隐患基础信息
        /// <summary>
        /// 隐患基础信息    
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetHiddenBaseInfoPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            //当前用户
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            if (pagination.p_fields.IsEmpty())
            {
                pagination.p_fields = @" account,createuserdeptcode,createuserorgcode,createuserid,checktypename,to_char(checkdate,'yyyy-MM-dd') checkdate,hidtypename,hiddentypename,hidrankname,hiddepart,hiddepartname,checkdepartname,
                                         createdate,hidcode,checktype,isgetafter,exposurestate,isbreakrule,hidtype,acceptdepartcode,acceptperson,acceptpersonname,acceptdepartname,acceptdate,changeperson,hidproject,hidprojectname,
                                         hidrank,hidpoint,workstream,addtype,participant,applicationstatus,postponeperson,postponepersonname,postponedept,postponedeptname,hiddescribe,changemeasure,changeresume,
                                         to_char(changedeadine,'yyyy-MM-dd') changedeadine,to_char(changefinishdate,'yyyy-MM-dd') changefinishdate,realitymanagecapital,planmanagecapital,changedutydepartcode,safetycheckobjectid,checkmanname,hidpointname,hidplace,changedutydepartname,changepersonname,
                                         deviceid,devicecode,devicename,relevanceid,relevancetype,monitorpersonid,monitorpersonname,majorclassify,majorclassifyname,hidname,hidstatus,hidconsequence,
                                         actionperson,recheckperson,recheckpersonname,recheckdepartcode,recheckdepartname,changedeptcode,hidbasefilepath,reformfilepath,changeresult,hidphoto,hidchangephoto,
                                         safetycheckname,chargeperson,chargepersonname,chargedeptid,chargedeptname,isappoint,participantname,createdeptname,createusername,hidbmid,hidbmname,changeplanid,curapprovedate,curacceptdate,beforeapprovedate,beforeacceptdate,afterapprovedate,afteracceptdate";
            }
            else
            {
                pagination.p_fields = pagination.p_fields.Replace("checkdate", "to_char(checkdate,'yyyy-MM-dd') checkdate").
                        Replace("changedeadine", "to_char(changedeadine,'yyyy-MM-dd') changedeadine").
                        Replace("changefinishdate", "to_char(changefinishdate,'yyyy-MM-dd') changefinishdate").Replace("acceptdate", "to_char(acceptdate,'yyyy-MM-dd') acceptdate") + ",curapprovedate,curacceptdate,beforeapprovedate,beforeacceptdate,afterapprovedate,afteracceptdate";
            }



            pagination.p_kid = "id";

            pagination.conditionJson = " 1=1";

            var queryParam = queryJson.ToJObject();


            if (!queryParam["qWorkstream"].IsEmpty())
            {
                pagination.p_tablename = @"v_basehiddeninfo";
            }
            else
            {
                pagination.p_tablename = @"v_hiddenbasedata";
            }

            //组织机构
            if (!string.IsNullOrEmpty(user.OrganizeCode))
            {
                //省级单位
                if (user.RoleName.Contains("省级用户") || user.RoleName.Contains("集团用户"))
                {
                    pagination.conditionJson += string.Format(@" and  deptcode  like '{0}%' ", user.OrganizeCode);
                }
                else   //厂级
                {
                    pagination.conditionJson += string.Format(@" and  hiddepart = '{0}' ", user.OrganizeId);
                }
            }

            //查询条件
            #region 查询条件
            if (!queryParam["action"].IsEmpty())
            {
                string action = queryParam["action"].ToString();

                switch (action)
                {
                    //隐患登记
                    case "Register":
                        pagination.conditionJson += string.Format(@" and  createuserid ='{0}' ", user.UserId);
                        break;
                    //隐患完善
                    case "Perfection":
                        pagination.conditionJson += @" and workstream  = '隐患完善'";
                        break;
                    //制定整改计划
                    case "ChangePlan":
                        pagination.conditionJson += @" and workstream  = '制定整改计划'";
                        break;
                    //隐患评估
                    case "Approval":
                        pagination.conditionJson += @" and workstream  = '隐患评估'";
                        break;
                    //隐患整改
                    case "Change":
                        pagination.conditionJson += @" and workstream  = '隐患整改'";
                        break;
                    //隐患验收
                    case "Accept":
                        pagination.conditionJson += @" and workstream  = '隐患验收'";
                        break;
                    //复查验证
                    case "ReCheck":
                        pagination.conditionJson += @" and workstream  = '复查验证'";
                        break;
                    //整改效果评估
                    case "Estimate":
                        pagination.conditionJson += @" and workstream  = '整改效果评估'";
                        break;
                    //整改结束
                    case "BaseEnd":
                        pagination.conditionJson += @" and workstream  = '整改结束'";
                        break;
                    //整改延期阶段的数据
                    case "Postpone":
                        pagination.conditionJson += @" and applicationstatus  = '1'";
                        break;
                }
            }
            #endregion
            //隐患状态
            #region 隐患状态
            if (!queryParam["ChangeStatus"].IsEmpty())
            {
                switch (queryParam["ChangeStatus"].ToString())
                {
                    case "制定整改计划":
                        pagination.conditionJson += @" and workstream = '制定整改计划' ";
                        break;
                    case "未整改":
                        pagination.conditionJson += @" and workstream = '隐患整改' ";
                        break;
                    case "逾期未评估":
                        pagination.conditionJson += string.Format(@" and workstream = '隐患评估'  and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') > afterapprovedate", DateTime.Now);
                        break;
                    case "即将到期未评估":
                        pagination.conditionJson += string.Format(@" and workstream = '隐患评估'  and   to_date('{0}','yyyy-mm-dd hh24:mi:ss') >= beforeapprovedate  and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') <=  afterapprovedate  ", DateTime.Now);
                        break;
                    case "逾期未整改":
                        pagination.conditionJson += string.Format(@" and workstream = '隐患整改'  and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') > changedeadine + 1", DateTime.Now);
                        break;
                    case "延期整改":
                        pagination.conditionJson += @" and  hidcode in (select distinct hidcode from bis_htextension where handlesign ='1')";
                        break;
                    case "即将到期未整改":
                        pagination.conditionJson += @"and workstream = '隐患整改' and ((rankname = '一般隐患' and changedeadine - 3 <= 
                                                         sysdate  and sysdate <= changedeadine + 1 )  or (rankname = '重大隐患' and changedeadine - 5 <= sysdate and  sysdate <= changedeadine + 1 ) )";

                        break;
                    case "逾期未验收":
                        pagination.conditionJson += string.Format(@" and workstream = '隐患验收'  and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') > afteracceptdate", DateTime.Now);
                        break;
                    case "即将到期未验收":
                        pagination.conditionJson += string.Format(@" and workstream = '隐患验收'  and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') >= beforeacceptdate   and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') <= afteracceptdate  ", DateTime.Now);
                        break;
                    case "本人登记":
                        pagination.conditionJson += string.Format(@" and  createuserid ='{0}'", user.UserId);
                        break;
                    case "已整改":
                        pagination.conditionJson += @" and   workstream in ('隐患验收','复查验证','整改效果评估','整改结束')"; //
                        break;
                    case "挂牌督办":
                        pagination.conditionJson += @" and  isgetafter ='1'";
                        break;
                    case "未整改结束":
                        pagination.conditionJson += @" and  workstream !='整改结束'";
                        break;
                    case "未闭环":
                        pagination.conditionJson += @" and  workstream !='整改结束' and  workstream !='隐患评估' ";
                        break;
                    case "已闭环":
                        pagination.conditionJson += @" and  workstream ='整改结束'";
                        break;
                }
            }
            #endregion
            //数据范围
            #region 数据范围
            if (!queryParam["DataScope"].IsEmpty())
            {
                switch (queryParam["DataScope"].ToString())
                {
                    case "本人登记":
                        pagination.conditionJson += string.Format(@" and  createuserid ='{0}'", user.UserId);
                        break;
                    case "本人完善":
                        pagination.conditionJson += string.Format(@" and actionperson   like  '%,{0},%'  and workstream ='隐患完善'", user.Account);
                        break;
                    case "本人评估":
                        pagination.conditionJson += string.Format(@" and actionperson   like  '%,{0},%'  and workstream ='隐患评估'", user.Account);
                        break;
                    case "本部门评估":
                        pagination.conditionJson += string.Format(@" and  hidcode in  (select distinct hidcode from v_approvaldata where departmentcode ='{0}' and name ='隐患评估')", user.DeptCode);
                        break;
                    case "本人制定整改计划":
                        pagination.conditionJson += string.Format(@" and actionperson   like  '%,{0},%'  and workstream ='制定整改计划'", user.Account);
                        break;
                    case "本人整改":
                        pagination.conditionJson += string.Format(@" and changeperson  =  '{0}'", user.UserId);
                        break;
                    case "本部门整改":
                        pagination.conditionJson += string.Format(@" and changedutydepartcode  =  '{0}'", user.DeptCode);
                        break;
                    case "本人验收":
                        pagination.conditionJson += string.Format(@" and actionperson   like  '%,{0},%' and workstream = '隐患验收'", user.Account);
                        break;
                    case "本部门验收":
                        pagination.conditionJson += string.Format(@" and  hidcode in  (select distinct hidcode from v_approvaldata where departmentcode ='{0}' and name ='隐患验收')  ", user.DeptCode);
                        break;
                    case "本人效果评估":
                        pagination.conditionJson += string.Format(@" and actionperson   like  '%,{0},%' and  workstream = '整改效果评估' ", user.Account);
                        break;
                    case "本部门效果评估":  //存在问题
                        pagination.conditionJson += string.Format(@" and  hidcode in  (select distinct hidcode from v_approvaldata where departmentcode ='{0}' and name ='整改效果评估') ", user.DeptCode);
                        break;
                    case "本人审(核)批": //整改延期申请审批
                        pagination.conditionJson += string.Format(@" and  (applicationstatus ='1' and postponeperson  like  '%,{0},%')", user.Account);
                        break;
                    case "本部门审(核)批"://部门整改延期申请审批
                        pagination.conditionJson += string.Format(@"  and  (applicationstatus ='1' and postponedept  like  '%,{0},%')", user.DeptCode);
                        break;
                    case "本人复查":
                        pagination.conditionJson += string.Format(@" and actionperson  like  '%,{0},%' and  workstream = '复查验证' ", user.Account);
                        break;
                    case "本部门复查":
                        pagination.conditionJson += string.Format(@" and  hidcode in  (select distinct hidcode from v_approvaldata where departmentcode ='{0}' and name ='复查验证') ", user.DeptCode);
                        break;
                }
            }
            #endregion
            //确定数据范围
            #region 确定数据范围
            if (!queryParam["qWorkstream"].IsEmpty())
            {
                string deptcode = string.Empty;
                //省级首页统计
                #region 当前用户是公司级及厂级用户
                //当前用户是公司级及厂级用户
                if (user.RoleName.Contains("省级用户") || user.RoleName.Contains("厂级") || user.RoleName.Contains("公司级"))
                {
                    DepartmentEntity deptEntity = Idepartmentservice.GetEntity(user.OrganizeId);
                    deptcode = deptEntity.DeptCode;
                    pagination.conditionJson += string.Format(@"   and  (changedeptcode  like '{0}%' or changedutydepartcode like '{0}%') ", deptcode);
                }
                //else
                //{
                //    DepartmentEntity deptEntity = Idepartmentservice.GetEntity(user.DeptId);
                //    deptcode = deptEntity.DeptCode;  //当前用户部门的所属新编码
                //}

                #endregion
            }
            #endregion
            //登记单位
            if (!queryParam["hidregistertype"].IsEmpty())
            {
                if (queryParam["hidregistertype"].ToString() == "省公司登记")
                {
                    pagination.conditionJson += @" and addtype = '2'";
                }
                else if (queryParam["hidregistertype"].ToString() == "下属各单位登记")
                {
                    pagination.conditionJson += @" and addtype != '2'";
                }
            }

            //台账标记
            if (!queryParam["standingmark"].IsEmpty())
            {
                pagination.conditionJson += @" and workstream != '隐患登记'";
            }
            //台账类型
            if (!queryParam["HidStandingType"].IsEmpty())
            {
                string standingtype = queryParam["HidStandingType"].ToString();

                pagination.conditionJson += @" and workstream != '隐患评估'";

                if (standingtype.Contains("公司级"))
                {
                    pagination.conditionJson += @" and  (rolename  like  '%公司级%' or   rolename  like  '%厂级部门用户%'  or  rankname  = '重大隐患') ";
                }
                else
                {
                    pagination.conditionJson += string.Format(@" and rolename  like  '%{0}%' and rankname  = '一般隐患' and  rolename not like '%厂级%' ", standingtype);
                }
            }
            //流程状态
            if (!queryParam["WorkStream"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and workstream = '{0}'", queryParam["WorkStream"].ToString());
            }
            //创建时间年度
            if (!queryParam["qyear"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and to_char(createdate,'yyyy') = '{0}'", queryParam["qyear"].ToString());
            }
            //检查类型
            if (!queryParam["SaftyCheckType"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and checktype = '{0}'", queryParam["SaftyCheckType"].ToString());
            }
            //专业类型
            if (!queryParam["majorClassify"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and majorclassify = '{0}'", queryParam["majorClassify"].ToString());
            }
            //隐患类型
            if (!queryParam["HidType"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and hidtype = '{0}'", queryParam["HidType"].ToString());
            }
            //所属单位
            if (!queryParam["HidDepart"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and hiddepart = '{0}'", queryParam["HidDepart"].ToString());
            }
            //确定为隐患的数据
            if (!queryParam["pType"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and workstream in ('隐患整改','隐患验收','复查验证','整改效果评估','整改结束')");
            }
            //确定为重大隐患的数据 
            if (!queryParam["HtLevel"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and rankname = '{0}' and workstream in ('隐患整改','隐患验收','复查验证','整改效果评估','整改结束')", "重大隐患");
            }
            //隐患级别
            if (!queryParam["qrankname"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and rankname  = '{0}'", queryParam["qrankname"].ToString());
            }
            //获取安全检查下登记的隐患
            if (!queryParam["pMode"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and safetycheckobjectid is not null");
            }
            if (!queryParam["mode"].IsEmpty())
            {
                if (queryParam["mode"].ToString() == "pType")
                {
                    if (!queryParam["bmmark"].IsEmpty())
                    {
                        pagination.conditionJson += string.Format(@" and changedutydepartcode like '{0}%'", user.DeptCode);  //按整改单位来统计
                    }
                }
                #region  全部隐患
                if (queryParam["mode"].ToString() == "qbyh")
                {
                    pagination.conditionJson += string.Format(@" and to_char(createdate,'yyyy') = '{0}'", DateTime.Now.Year.ToString());

                    if (!queryParam["bmmark"].IsEmpty())
                    {
                        pagination.conditionJson += string.Format(@" and changedutydepartcode like '{0}%'", user.DeptCode);  //按整改单位来统计
                    }
                }
                #endregion
                #region  一般隐患
                else if (queryParam["mode"].ToString() == "ybqbyh")
                {
                    pagination.conditionJson += string.Format(@" and rankname = '一般隐患' ");
                }
                #endregion
                #region  重大隐患
                else if (queryParam["mode"].ToString() == "bigHt")
                {
                    pagination.conditionJson += string.Format(@" and rankname = '重大隐患'");
                }
                #endregion
                #region  省级首页
                else if (queryParam["mode"].ToString() == "home")
                {
                    if (user.RoleName.Contains("省级用户"))
                    {
                        pagination.conditionJson += string.Format(@" and  createuserorgcode  = '{0}' ", user.OrganizeCode);

                        pagination.conditionJson += string.Format(@" and to_char(createdate,'yyyy') = '{0}'", DateTime.Now.Year.ToString());
                    }
                }
                #endregion
                #region 设备统计
                if (queryParam["mode"].ToString() == "sbtjyh")
                {
                    string cwhere = string.Empty;

                    if (!queryParam["stDate"].IsEmpty())
                    {
                        cwhere += string.Format(@" and equ.checkdate >= to_date('{0}','yyyy-MM-dd') ", queryParam["stDate"].ToString());
                    }
                    if (!queryParam["etDate"].IsEmpty())
                    {
                        cwhere += string.Format(@" and equ.checkdate < to_date('{0}','yyyy-MM-dd') ", Convert.ToDateTime(queryParam["etDate"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
                    }
                    if (!queryParam["aff"].IsEmpty())
                    {
                        cwhere += string.Format(@" and equ.affiliation ='{0}' ", queryParam["aff"].ToString());
                    }
                    if (!queryParam["equtype"].IsEmpty())
                    {
                        cwhere += string.Format(@" and equ.equipmenttype ='{0}' ", queryParam["equtype"].ToString());
                    }

                    string csql = string.Format(@"select equ.id from bis_specialequipment equ 
                                                 where equ.createuserorgcode ='{0}'  {1}", user.OrganizeCode, cwhere);

                    pagination.conditionJson += string.Format(@" and deviceid in ({0})", csql);
                }
                #endregion
                #region 一般设备隐患
                else if (queryParam["mode"].ToString() == "sbyh")
                {
                    pagination.conditionJson += string.Format(@" and workstream !='整改结束' ");
                }
                #endregion
                #region 隐患发现排行榜
                else if (queryParam["mode"].ToString() == "yhfxphb")
                {
                    string code = queryParam["qdeptcode"].ToString();  //old部门编码

                    pagination.conditionJson += string.Format(@" and checkdepartid like '{0}%'", code);

                    pagination.conditionJson += string.Format(@" and to_char(createdate,'yyyy') = '{0}'", DateTime.Now.Year.ToString());
                }
                #endregion
                #region 隐患整改排行榜
                else if (queryParam["mode"].ToString() == "yhzgphb")
                {
                    string code = queryParam["qdeptcode"].ToString();  //old部门编码

                    var newdeptcode = Idepartmentservice.GetEntityByCode(code); //获取对应的部门信息

                    pagination.conditionJson += string.Format(@" and changedeptcode like '{0}%'", newdeptcode.DeptCode);

                    pagination.conditionJson += string.Format(@" and to_char(createdate,'yyyy') = '{0}'", DateTime.Now.Year.ToString());
                }
                #endregion
                #region 隐患趋势图
                else if (queryParam["mode"].ToString() == "tendencyht")
                {
                    if (!queryParam["qyearmonth"].IsEmpty())
                    {
                        string qyearmonth = queryParam["qyearmonth"].ToString();
                        pagination.conditionJson += string.Format(@" and to_char(createdate,'yyyy-MM') = '{0}'", qyearmonth.Replace('.', '-'));
                    }
                }
                #endregion
                #region 省级设备隐患统计
                else if (queryParam["mode"].ToString() == "sjsbyhtj")
                {
                    //单位
                    if (!queryParam["qdeptcode"].IsEmpty())
                    {
                        string code = queryParam["qdeptcode"].ToString();  //old部门编码

                        var newdeptcode = Idepartmentservice.GetEntityByCode(code); //获取对应的部门信息

                        pagination.conditionJson += string.Format(@" and changedeptcode like '{0}%'", newdeptcode.DeptCode);
                    }
                    //筛选检查年度
                    if (!queryParam["qyear"].IsEmpty())
                    {
                        pagination.conditionJson += string.Format(@" and to_char(checkdate,'yyyy') = '{0}'", queryParam["qyear"].ToString());
                    }

                    pagination.conditionJson += @" and deviceid in (select distinct  id  from bis_specialequipment)";  //特种设备
                }
                #endregion
                #region 隐患统计信息(按隐患整改来统计)
                else if (queryParam["mode"].ToString() == "yhtjinfo")
                {
                    //单位
                    if (!queryParam["qdeptcode"].IsEmpty())
                    {
                        string code = queryParam["qdeptcode"].ToString();  //old部门编码

                        var newdeptcode = Idepartmentservice.GetEntityByCode(code); //获取对应的部门信息

                        if (!queryParam["bmmark"].IsEmpty())
                        {
                            pagination.conditionJson += string.Format(@" and changedeptcode = '{0}'", newdeptcode.DeptCode);  //按登记单位来统计
                        }
                        else
                        {
                            pagination.conditionJson += string.Format(@" and changedeptcode like '{0}%'", newdeptcode.DeptCode);  //按登记单位来统计
                        }
                    }
                    //筛选年度
                    if (!queryParam["qyear"].IsEmpty())
                    {
                        pagination.conditionJson += string.Format(@" and to_char(createdate,'yyyy') = '{0}'", queryParam["qyear"].ToString());
                    }
                    //筛选年月
                    if (!queryParam["qyearmonth"].IsEmpty())
                    {
                        pagination.conditionJson += string.Format(@" and to_char(createdate,'yyyy-MM') = '{0}'", queryParam["qyearmonth"].ToString());
                    }
                    //整改状态
                    if (!queryParam["qchangestatus"].IsEmpty())
                    {
                        if (queryParam["qchangestatus"].ToString() == "已整改")
                        {
                            pagination.conditionJson += @" and   workstream in ('隐患验收','复查验证','整改效果评估','整改结束')";
                        }
                        if (queryParam["qchangestatus"].ToString() == "未整改")
                        {
                            pagination.conditionJson += @" and workstream = '隐患整改' ";
                        }
                    }
                    //检查类型 qchecktype
                    if (!queryParam["qchecktype"].IsEmpty())
                    {
                        pagination.conditionJson += string.Format(@" and checktypename = '{0}'", queryParam["qchecktype"].ToString());
                    }
                }
                #endregion
                #region  隐患统计信息(按隐患登记单位来统计)
                else if (queryParam["mode"].ToString() == "yhdjinfo")
                {
                    //单位
                    if (!queryParam["qdeptcode"].IsEmpty())
                    {
                        string code = queryParam["qdeptcode"].ToString();  //old部门编码

                        var newdeptcode = Idepartmentservice.GetEntityByCode(code); //获取对应的部门信息

                        if (!queryParam["bmmark"].IsEmpty())
                        {
                            pagination.conditionJson += string.Format(@" and deptcode = '{0}'", newdeptcode.DeptCode);  //按登记单位来统计
                        }
                        else
                        {
                            pagination.conditionJson += string.Format(@" and deptcode like '{0}%'", newdeptcode.DeptCode);  //按登记单位来统计
                        }
                    }
                    //筛选年度
                    if (!queryParam["qyear"].IsEmpty())
                    {
                        pagination.conditionJson += string.Format(@" and to_char(createdate,'yyyy') = '{0}'", queryParam["qyear"].ToString());
                    }
                    //筛选年月
                    if (!queryParam["qyearmonth"].IsEmpty())
                    {
                        pagination.conditionJson += string.Format(@" and to_char(createdate,'yyyy-MM') = '{0}'", queryParam["qyearmonth"].ToString());
                    }
                    //整改状态
                    if (!queryParam["qchangestatus"].IsEmpty())
                    {
                        if (queryParam["qchangestatus"].ToString() == "已整改")
                        {
                            pagination.conditionJson += @" and   workstream in ('隐患验收','复查验证','整改效果评估','整改结束')";
                        }
                        if (queryParam["qchangestatus"].ToString() == "未整改")
                        {
                            pagination.conditionJson += @" and workstream = '隐患整改' ";
                        }
                    }
                    //检查类型 qchecktype
                    if (!queryParam["qchecktype"].IsEmpty())
                    {
                        pagination.conditionJson += string.Format(@" and checktypename = '{0}'", queryParam["qchecktype"].ToString());
                    }
                }
                #endregion
                #region 未闭环隐患统计
                else if (queryParam["mode"].ToString() == "wbhtjcx")
                {
                    //单位
                    if (!queryParam["qdeptcode"].IsEmpty())
                    {
                        string code = queryParam["qdeptcode"].ToString();  //old部门编码

                        var newdeptcode = Idepartmentservice.GetEntityByCode(code); //获取对应的部门信息

                        if (newdeptcode.Nature == "厂级")
                        {
                            pagination.conditionJson += string.Format(@" and changedeptcode = '{0}'", newdeptcode.DeptCode);
                        }
                        else
                        {
                            pagination.conditionJson += string.Format(@" and changedeptcode like '{0}%'", newdeptcode.DeptCode);
                        }
                    }
                }
                #endregion
                #region 外包工程下的隐患统计信息
                else if (queryParam["mode"].ToString() == "wbgcyh")
                {
                    pagination.conditionJson += string.Format(@" and hidproject = '{0}'", queryParam["engineerid"].ToString());
                }
                #endregion
            }
            //隐患区域
            if (!queryParam["areaCode"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and hidpoint like '{0}%'", queryParam["areaCode"].ToString());
            }
            //是否曝光
            if (!queryParam["IsExposureState"].IsEmpty())
            {
                string exposurestate = queryParam["IsExposureState"].ToString();
                //是否曝光
                if (exposurestate == "1")
                {
                    pagination.conditionJson += @"  and exposurestate = '是'";
                }
                else
                {
                    pagination.conditionJson += @"  and  (exposurestate = '否'  or  exposurestate is null)";
                }
            }
            //机构 Or 部门  //按照整改部门来查询统计
            if (!queryParam["isOrg"].IsEmpty())
            {
                string choosetag = string.Empty;
                //1为按照创建单位  0 为按照整改单位 
                string queryDeptCode = "deptcode";
                int querybtntype = 1;

                if (!queryParam["querybtntype"].IsEmpty())
                {
                    choosetag = queryParam["choosetag"].ToString();

                    querybtntype = int.Parse(queryParam["querybtntype"].ToString());

                    //按创建单位来
                    if (querybtntype > 0)
                    {
                        queryDeptCode = "deptcode";
                    }
                    else //按整改单位来
                    {
                        queryDeptCode = "changedeptcode";
                    }
                }

                string isOrg = queryParam["isOrg"].ToString();
                string code = string.Empty;
                if (!queryParam["code"].IsEmpty())
                {
                    code = queryParam["code"].ToString();  //old部门编码

                    var newdeptcode = Idepartmentservice.GetEntityByCode(code); //获取对应的部门信息
                    //按创建单位
                    if (querybtntype == 1)
                    {
                        //按本单位
                        if (choosetag == "0")
                        {
                            pagination.conditionJson += string.Format(@" and {0}  = '{1}'", queryDeptCode, newdeptcode.DeptCode);
                        }
                        else
                        {
                            pagination.conditionJson += string.Format(@" and {0}  like '{1}%'", queryDeptCode, newdeptcode.DeptCode);
                        }
                    }
                    else  //按整改单位
                    {
                        if (newdeptcode.Nature == "省级")
                        {
                            pagination.conditionJson += string.Format(@" and hiddepart in (select departmentid from base_department where deptcode  like '{0}%'  and nature ='厂级')", newdeptcode.DeptCode);  //取所有
                        }
                        else if (newdeptcode.Nature == "厂级" || newdeptcode.Nature == "公司级")
                        {
                            pagination.conditionJson += string.Format(@" and hiddepart  = '{0}'", newdeptcode.DepartmentId); //按所属单位
                        }
                        else
                        {
                            pagination.conditionJson += string.Format(@" and {0}  like '{1}%'", queryDeptCode, newdeptcode.DeptCode);
                        }
                    }
                }
            }
            //隐患区域
            if (!queryParam["HidPoint"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and  hidpoint='{0}' ", queryParam["HidPoint"].ToString());
            }
            //检查项目ID
            if (!queryParam["checkId"].IsEmpty())
            {
                string ckId = queryParam["checkId"].ToString();
                string ids = GetCheckIds(ckId);
                if (!queryParam["pfrom"].IsEmpty())
                {
                    string pfrom = queryParam["pfrom"].ToString();
                    if (pfrom == "0")
                    {
                        pagination.conditionJson += string.Format(@" and (safetycheckobjectid in('{1}') or safetycheckobjectid='{0}')", ckId, ids.Replace(",", "','"));
                    }
                    if (pfrom == "1")
                    {

                        pagination.conditionJson += string.Format(@" and (safetycheckobjectid in('{1}') or safetycheckobjectid='{0}')", ckId, ids.Replace(",", "','"));
                    }
                }
                else
                {
                    pagination.conditionJson += string.Format(@" and safetycheckobjectid ='{0}'", ckId);
                }
            }
            //检查对象
            if (!queryParam["checkObjId"].IsEmpty())
            {
                if (!queryParam["checkId"].IsEmpty())
                {
                    if (!queryParam["checkObjId"].ToString().Equals(queryParam["checkId"].ToString()))
                    {
                        pagination.conditionJson += string.Format(@" and relevanceid in(select id from bis_saftycheckdatadetailed where checkobjectid='{0}')", queryParam["checkObjId"].ToString());
                    }
                }
            }
            //设备id
            if (!queryParam["DeviceId"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and DeviceId like '%{0}%'", queryParam["DeviceId"].ToString());
            }
            if (!queryParam["checkType"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and safetycheckobjectid in (select id from bis_saftycheckdatadetailed  where recid='{0}')", queryParam["checkType"].ToString());
            }
            //隐患级别
            if (!queryParam["HidRank"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and hidrank = '{0}'", queryParam["HidRank"].ToString());
            }
            //隐患描述
            if (!queryParam["HidDescribe"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and hiddescribe like '%{0}%'", queryParam["HidDescribe"].ToString());
            }
            //隐患创建开始时间
            if (!queryParam["StartTime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and createdate >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", queryParam["StartTime"].ToString());
            }
            //隐患创建结束时间
            if (!queryParam["EndTime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and createdate <= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(queryParam["EndTime"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
            }
            //隐患整改截止开始时间
            if (!queryParam["cdstDate"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and changedeadine >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", queryParam["cdstDate"].ToString());
            }
            //隐患整改截止结束时间
            if (!queryParam["cdetDate"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and changedeadine <= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(queryParam["cdetDate"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
            }
            //关联程序ID
            if (!queryParam["RelevanceId"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and  relevanceid like '%{0}%' ", queryParam["RelevanceId"].ToString());
            }
            //关联程序Type
            if (!queryParam["RelevanceType"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and  relevancetype = '{0}' ", queryParam["RelevanceType"].ToString());
            }

            var dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);

            return dt;
        }
        #endregion

        #region 获取关联对象下的隐患信息
        /// <summary>
        /// 获取关联对象下的隐患信息
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetHiddenByRelevanceId(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;

            if (pagination.p_fields.IsEmpty())
            {
                pagination.p_fields = @" addtype,hidcode,hidtypename,hidrankname,checktypename,checkdepartname,hiddescribe,relevanceid ";
            }

            pagination.p_kid = "id";

            pagination.conditionJson = " 1=1";

            pagination.p_tablename = @"v_htbaseinfo";

            var queryParam = queryJson.ToJObject();

            //查询条件
            if (!queryParam["RelevanceId"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and  relevanceid like '%{0}%' ", queryParam["RelevanceId"].ToString());
            }

            var dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);

            return dt;
        }

        #endregion}

        #region  查询隐患所有相关
        /// <summary>
        /// 查询隐患所有相关
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public DataTable GetHiddenByKeyValue(string keyValue)
        {
            string sql = string.Format(@"select a.id as hiddenid,
                              a.hidcode as problemid,
                              a.createuserid,
                              a.createuserdeptcode,
                              a.isbreakrule as breakrulebehavior,
                              a.hidbmid , 
                              a.hidbmname , 
                              a.hidrank , 
                              a.hidrankname as rankname,
                              a.hidtype as categoryid,
                              a.hidtypename as category,
                              a.hidpoint as  hidpointid ,
                              a.hidpointname as hidpoint,
                              a.hiddescribe,
                              a.hidplace,
                              a.reportdigest,
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
                              c.changeresult as reformresult,
                              c.applicationstatus,
                              c.postponedays,
                              c.postponedept,
                              c.postponedeptname,
                              c.postponeperson,
                              c.postponepersonname,
                              c.backreason,
                              c.attachment,
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
                              d.isupaccept,
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
                              a.deviceid,
                              a.devicecode,
                              a.devicename,
                              a.monitorpersonid,
                              a.monitorpersonname,
                              a.relevanceid,
                              a.relevancetype,
                              a.majorclassify,
                              a.majorclassifyname, 
                              a.hidname,
                              a.hidstatus,
                              a.hidconsequence,
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
                              f.approvalfile,
                              g.estimateperson ,
                              g.estimatepersonname,
                              g.estimatedepartcode,
                              g.estimatedepartname,
                              g.estimatedate,
                              g.estimatedepart,
                              g.estimaterank,
                              g.estimateresult,
                              g.estimatephoto,
                              a.hidproject,
                              a.hidprojectname ,
                              a.hiddepart,
                              a.hiddepartname,
                              i.recheckperson,
                              i.recheckpersonname,
                              i.recheckdepartcode,
                              i.recheckdepartname,
                              i.recheckdate,
                              i.recheckstatus,
                              i.recheckidea,
                              b.actionperson,
                              a.safetycheckobjectid,
                              a.upsubmit,
                              a.isselfchange,
                              a.isformulate,
                              a.safetycheckname,c.chargeperson,c.chargepersonname,c.chargedeptid,c.chargedeptname,c.isappoint
                              from v_htbaseinfo a
                              left join v_workflow b on a.id = b.id 
                              left join v_htchangeinfo c on a.hidcode = c.hidcode
                              left join v_htacceptinfo d on a.hidcode = d.hidcode
                              left join v_htapprovalinfo f on a.hidcode = f.hidcode 
                              left join v_htestimateinfo g on  a.hidcode = g.hidcode 
                              left join v_htextensioninfo h on a.hidcode = h.hidcode
                              left join v_htrecheck  i on a.hidcode = i.hidcode
                              where 1=1 and a.id ='{0}'", keyValue);

            return this.BaseRepository().FindTable(sql);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, HTBaseInfoEntity entity)
        {
            entity.ID = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                HTBaseInfoEntity ht = this.BaseRepository().FindEntity(keyValue);
                if (ht == null)
                {

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
        #endregion

        #region 查询隐患统计

        #region 隐患统计(厂级)
        /// <summary>
        /// 隐患统计
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

            string tempStr = string.Empty;

            Operator cuUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            if (cuUser.RoleName.Contains("厂级") || cuUser.RoleName.Contains("公司级"))
            {
                sentity.isCompany = true;
            }
            else
            {
                sentity.isCompany = false;
            }

            int mark = 0;  //标记变量  0 无隐患级别条件 ，1 标识1哥隐患级别， 2 标识两个隐患级别条件

            //区域范围
            if (!sentity.sArea.IsEmpty())
            {
                str += string.Format("  and hidpoint like '{0}%'", sentity.sArea.ToString());
            }
            //隐患级别
            if (!sentity.sHidRank.IsEmpty())
            {
                string[] tempRank = sentity.sHidRank.Split(',');

                mark = tempRank.Length;  //标记变量

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
            //检查类型
            if (!sentity.isCheck.IsEmpty())
            {
                str += @"  and  safetycheckobjectid in (select id from bis_saftycheckdatarecord)";

                if (!sentity.sCheckType.IsEmpty())
                {
                    var chkType = sentity.sCheckType;
                    if (chkType != "0")
                    {
                        str += string.Format(@"  and  checktype = (select a.itemdetailid from BASE_DATAITEMDETAIL a
                                            left join base_dataitem b on a.itemid = b.itemid  where b.itemcode ='SaftyCheckType' and a.itemvalue ='{0}') ", sentity.sCheckType);
                    }
                    else
                    {//上级单位的安全检查
                        str += string.Format(@" and safetycheckobjectid in (select id from bis_saftycheckdatarecord where checkeddepartid in (select  departmentid from BASE_DEPARTMENT  where (nature='厂级' or nature='电厂') start with encode='{0}' connect by  prior  departmentid = parentid) )", sentity.sDeptCode);
                    }
                }
            }
            //省公司或电厂的检查
            if (!sentity.sCType.IsEmpty())
            {
                if (sentity.sCType == "省公司")
                {
                    str += string.Format(@" and safetycheckobjectid in (select id from bis_saftycheckdatarecord where checkeddepartid in (select  departmentid from BASE_DEPARTMENT  where (nature='厂级' or nature='电厂') start with encode='{0}' connect by  prior  departmentid = parentid) )", sentity.sDeptCode);
                }
                else if (sentity.sCType == "电厂")
                {
                    str += string.Format(@" and safetycheckobjectid in (select id from bis_saftycheckdatarecord where checkeddepartid is null and createuserorgcode in (select  encode from BASE_DEPARTMENT  where (nature='厂级' or nature='电厂') start with encode='{0}' connect by  prior  departmentid = parentid) )", sentity.sDeptCode);
                }
            }
            //起始日期
            if (!sentity.startDate.IsEmpty())
            {
                str += string.Format(" and createdate >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')  ", sentity.startDate);
            }
            //截止日期
            if (!sentity.endDate.IsEmpty())
            {
                str += string.Format(" and createdate <= to_date('{0}','yyyy-mm-dd hh24:mi:ss')  ", sentity.endDate);
            }
            //年度
            if (!sentity.sYear.IsEmpty())
            {
                str += string.Format(" and to_char(createdate,'yyyy') ='{0}' ", sentity.sYear);
            }
            //统计类型
            if (!sentity.statType.IsEmpty())
            {
                if (sentity.statType == "已闭环")
                {
                    str += string.Format(" and  workstream ='整改结束' ");
                }
                if (sentity.statType == "未闭环")
                {
                    str += string.Format(" and  workstream !='整改结束' ");
                }
            }
            switch (sentity.sAction)
            {

                //隐患等级数量列表
                case "1":
                    #region 厂级或公司用户
                    if (sentity.isCompany)
                    {

                        sql = string.Format(@"select a.districtcode as hidpoint,a.districtname as hidpointname,nvl(b.OrdinaryHid,0) as OrdinaryHid,nvl(b.ImportanHid,0) as ImportanHid,nvl(b.total,0) as total ,
                                            a.organizeid from ( select districtcode ,districtname ,sortcode，organizeid  from bis_district  where organizeid ='{1}' and parentid ='0' ) a
                                            left join (
                                                        select a.hidpoint,a.hidpointname,nvl(a.OrdinaryHid,0) OrdinaryHid,nvl(a.ImportanHid,0) ImportanHid,(nvl(a.OrdinaryHid,0) + nvl(a.ImportanHid,0)) as total  from (
                                                        select * from (
                                                        select a.districtcode as hidpoint,a.districtname as hidpointname,a.organizeid ,nvl(b.y,0) as pnum ,b.rankname from bis_district a
                                                            left join (
                                                            select  count(1) as y , hidpoint,rankname   from v_basehiddeninfo 
                                                            where    createuserdeptcode  like '{0}%' and  {2} group by hidpoint,rankname 
                                                            ) b on a.districtcode =  substr(b.hidpoint,0, length(a.districtcode))
                                                            where a.parentid = '0' and a.organizeid ='{1}'
                                                        ) pivot (sum(pnum) for rankname in ('一般隐患' as OrdinaryHid,'重大隐患' as ImportanHid))) a    
                                             )  b on a.districtcode =  b.hidpoint  order by a.sortcode ", sentity.sDeptCode, sentity.sOrganize, str);
                    }
                    else
                    {
                        sql = string.Format(@"select '' hidpoint,'' hidpointname, nvl(a.OrdinaryHid,0) OrdinaryHid,nvl(a.ImportanHid,0) ImportanHid,(nvl(a.OrdinaryHid,0) + nvl(a.ImportanHid,0)) as total  from (
                                        select * from (
                                                        select  count(1) as pnum ,rankname   from v_basehiddeninfo 
                                                        where   createuserdeptcode  like '{0}%'  and  {1} group by rankname 
                                        ) pivot (sum(pnum) for rankname in ('一般隐患' as OrdinaryHid,'重大隐患' as ImportanHid))) a  order by hidpoint", sentity.sDeptCode, str);
                    }
                    #endregion
                    break;
                //隐患等级数量统计图
                case "2":
                    #region 隐患等级数量统计图
                    //统计类型
                    if (!sentity.statType.IsEmpty())
                    {
                        if (sentity.statType == "已闭环")
                        {
                            tempStr += string.Format(" and  workstream ='整改结束' ");
                        }
                        if (sentity.statType == "未闭环")
                        {
                            tempStr += string.Format(" and  workstream !='整改结束' ");
                        }
                    }
                    sql = string.Format(@"select  count(1) as y ,rankname  as name from v_basehiddeninfo where createuserdeptcode  like '{0}%' 
                                          and to_char(createdate,'yyyy') ='{1}' {2} group by  rankname  ", sentity.sDeptCode, sentity.sYear, tempStr);
                    #endregion
                    break;
                //隐患区域分布情况图
                case "3":
                    #region 隐患区域分布情况图
                    if (sentity.isCompany)
                    {
                        tempStr = string.Empty;
                        //统计类型
                        if (!sentity.statType.IsEmpty())
                        {
                            if (sentity.statType == "已闭环")
                            {
                                tempStr += string.Format(" and  workstream ='整改结束' ");
                            }
                            if (sentity.statType == "未闭环")
                            {
                                tempStr += string.Format(" and  workstream !='整改结束' ");
                            }
                        }

                        sql = string.Format(@"select a.districtcode as hidpoint,a.districtname as name,nvl(b.y,0) as y ,a.organizeid from ( select districtcode ,districtname ,sortcode，organizeid  
                                                 from bis_district  where organizeid ='{2}' and parentid ='0' ) a
                                                 left join (
                                                            select a.districtcode as  hidpoint,a.districtname as name,a.organizeid ,sum(nvl(b.y,0)) as y from BIS_DISTRICT a
                                                            left join (
                                                            select  count(1) as y , hidpoint  from v_basehiddeninfo 
                                                            where to_char(createdate,'yyyy') ='{0}' and  createuserdeptcode  like '{1}%' {3}  group by hidpoint
                                                            ) b on a.districtcode =  substr(b.hidpoint,0, length(a.districtcode))
                                                            where a.parentid = '0' and a.organizeid ='{2}' group by 
                                                            a.districtcode ,a.districtname ,a.organizeid
                                                 )  b on a.districtcode =  b.hidpoint  order by a.sortcode", sentity.sYear, sentity.sDeptCode, sentity.sOrganize, tempStr);
                    }
                    #endregion
                    break;
                /****隐患数量变化趋势图****/
                case "4":
                    #region 隐患数量变化趋势图
                    if (mark == 0)
                    {
                        sql = string.Format(@"select a.month,nvl(b.allhid,0) allhid  from (select lpad(level,2,0) as month from dual connect by level <13) a
                                              left join (
                                                   select * from (
                                                           select  count(1) as pnum , to_char(createdate,'MM') as tMonth ,'所有隐患' as rankname from v_basehiddeninfo where createuserdeptcode  like '{0}%' 
                                                           and to_char(createdate,'yyyy') ='{1}' and {2}    group by to_char(createdate,'MM')
                                                       ) pivot (sum(pnum) for rankname in ('所有隐患' as AllHid))
                                              ) b on a.month =b.tMonth order by a.month", sentity.sDeptCode, sentity.sYear, str);
                    }
                    else if (mark == 1)
                    {
                        tempStr = sentity.sHidRank == "一般隐患" ? " '一般隐患' as OrdinaryHid " : " '重大隐患' as ImportanHid";
                        string tempField = sentity.sHidRank == "一般隐患" ? "OrdinaryHid" : "ImportanHid";

                        sql = string.Format(@"select a.month, nvl(b.{4},0) {4}  from (select lpad(level,2,0) as month from dual connect by level <13) a
                                              left join (
                                                     select * from (    
                                                       select  count(1) as pnum , to_char(createdate,'MM') as tMonth ,rankname from v_basehiddeninfo where createuserdeptcode  like '{0}%' 
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
                                                        select  count(1) as pnum , to_char(createdate,'MM') as tMonth ,rankname from v_basehiddeninfo where createuserdeptcode  like '{0}%' 
                                                            and to_char(createdate,'yyyy') ='{1}'  and  {2}  group by to_char(createdate,'MM') ,rankname 
                                                            ) pivot (sum(pnum) for rankname in ('一般隐患' as OrdinaryHid,'重大隐患' as ImportanHid))
                                              ) b on a.month =b.tMonth order by a.month",
                                                  sentity.sDeptCode, sentity.sYear, str);
                    }
                    #endregion
                    break;
                /****各等级隐患情况统计****/
                case "5":
                    #region 各等级隐患情况统计
                    if (sentity.sMark == 0)
                    {
                        //厂级
                        if (sentity.isCompany)
                        {
                            sql = string.Format(@"select a.encode  as createuserdeptcode,a.fullname, nvl(b.OrdinaryHid,0) as  OrdinaryHid, nvl(b.ImportanHid,0) as ImportanHid,nvl(b.total,0) as total ,a.sortcode  from
                                                    (select encode ,fullname,sortcode,departmentid,parentid from base_department  where (nature='部门' or nature ='厂级')  and  encode like '{0}%' ) a
                                                     left join (
                                                           select b.encode createuserdeptcode,sum(nvl(a.OrdinaryHid,0)) OrdinaryHid,sum(nvl(a.ImportanHid,0)) ImportanHid,sum(nvl(a.OrdinaryHid,0) + nvl(a.ImportanHid,0)) as total,b.fullname  from 
                                                            (
                                                                    select * from (
                                                                        select  count(1) as pnum ,  b.encode createuserdeptcode ,rankname  from v_basehiddeninfo a
                                                                        left join 
                                                                        (
                                                                           select encode ,fullname,sortcode from base_department  where nature='部门' and  encode like '{0}%'
       
                                                                        ) b  on  substr(a.createuserdeptcode,0,length(b.encode)) = b.encode 
                                                                        where a.createuserdeptcode  like '{0}%'   and   {1} group by  b.encode,rankname
                                                                        union
                                                                        select  count(1) as pnum ,  b.encode createuserdeptcode ,rankname  from v_basehiddeninfo a
                                                                        left join 
                                                                        (
                                                                           select encode ,fullname,sortcode from base_department  where nature='厂级' and  encode = '{0}'
                                                                        ) b  on  a.createuserdeptcode = b.encode 
                                                                        where a.createuserdeptcode  = '{0}'  and   {1} group by  b.encode,rankname 
                                                                    ) pivot (sum(pnum) for rankname in ('一般隐患' as OrdinaryHid,'重大隐患' as ImportanHid))
                                                            ) a 
                                                            left join base_department b on a.createuserdeptcode = b.encode group by b.encode,b.fullname
                                                    ) b on a.encode = b.createuserdeptcode  order by sortcode   ", sentity.sDeptCode, str);
                        }
                        else //非厂级
                        {
                            sql = string.Format(@"select a.createuserdeptcode,nvl(a.OrdinaryHid,0) OrdinaryHid,nvl(a.ImportanHid,0) ImportanHid,(nvl(a.OrdinaryHid,0) + nvl(a.ImportanHid,0)) as total,b.fullname,b.sortcode  from (
                                                select * from ( select  count(1) as pnum , createuserdeptcode,rankname  from v_basehiddeninfo where createuserdeptcode  like '{0}%' 
                                                and  {1}  group by  createuserdeptcode,rankname  
                                            ) pivot (sum(pnum) for rankname in ('一般隐患' as OrdinaryHid,'重大隐患' as ImportanHid))) a 
                                            left join base_department b on a.createuserdeptcode = b.encode order by b.encode ", sentity.sDeptCode, str);
                        }
                    }
                    else if (sentity.sMark == 1)
                    {
                        DepartmentEntity dentity = Idepartmentservice.GetEntityByCode(sentity.sDeptCode);

                        if (dentity.Nature == "厂级")
                        {
                            sql = string.Format(@"select b.createuserdeptcode,nvl(b.OrdinaryHid,0) OrdinaryHid,nvl(b.ImportanHid,0) ImportanHid,(nvl(b.OrdinaryHid,0) + nvl(b.ImportanHid,0)) as total,a.fullname,a.sortcode  from (
                                                   select encode , fullname,sortcode  from  base_department b where encode = '{0}' 
                                                 ) a 
                                                left join
                                                (
                                                  select * from (
                                                   select  count(1) as pnum , createuserdeptcode,rankname  from v_basehiddeninfo where createuserdeptcode  = '{0}' 
                                                   and  {1}   group by  createuserdeptcode,rankname  
                                                  ) pivot (sum(pnum) for rankname in ('一般隐患' as OrdinaryHid,'重大隐患' as ImportanHid))
                                                ) b  on a.encode = b.createuserdeptcode
                                                 order by a.sortcode  ", sentity.sDeptCode, str);
                        }
                        else
                        {
                            sql = string.Format(@"select b.createuserdeptcode,nvl(b.OrdinaryHid,0) OrdinaryHid,nvl(b.ImportanHid,0) ImportanHid,(nvl(b.OrdinaryHid,0) + nvl(b.ImportanHid,0)) as total,a.fullname,a.sortcode  from (
                                                   select encode , fullname,sortcode  from  base_department b where encode like '{0}%' 
                                                 ) a 
                                                left join
                                                (
                                                  select * from (
                                                   select  count(1) as pnum , createuserdeptcode,rankname  from v_basehiddeninfo where createuserdeptcode  like '{0}%' 
                                                    and  {1}   group by  createuserdeptcode,rankname  
                                                  ) pivot (sum(pnum) for rankname in ('一般隐患' as OrdinaryHid,'重大隐患' as ImportanHid))
                                                ) b  on a.encode = b.createuserdeptcode
                                                 order by a.encode  ", sentity.sDeptCode, str);
                        }
                    }
                    else if (sentity.sMark == 2)
                    {
                        //厂级
                        if (sentity.isCompany)
                        {
                            sql = string.Format(@"select a.encode  as createuserdeptcode,a.fullname, nvl(b.OrdinaryHid,0) as  OrdinaryHid, nvl(b.ImportanHid,0) as ImportanHid,nvl(b.total,0) as total ,a.sortcode,a.departmentid,'0'  parentid from
                                                    (select encode ,fullname,sortcode,departmentid,parentid from base_department  where (nature='部门' or nature ='厂级')  and  encode like '{0}%' ) a
                                                     left join (
                                                           select b.encode createuserdeptcode,sum(nvl(a.OrdinaryHid,0)) OrdinaryHid,sum(nvl(a.ImportanHid,0)) ImportanHid,sum(nvl(a.OrdinaryHid,0) + nvl(a.ImportanHid,0)) as total,b.fullname  from 
                                                            (
                                                                    select * from (
                                                                        select  count(1) as pnum ,  b.encode createuserdeptcode ,rankname  from v_basehiddeninfo a
                                                                        left join 
                                                                        (
                                                                           select encode ,fullname,sortcode from base_department  where nature='部门' and  encode like '{0}%'
                                                                        ) b  on  substr(a.createuserdeptcode,0,length(b.encode)) = b.encode 
                                                                        where a.createuserdeptcode  like '{0}%'   and   {1} group by  b.encode,rankname
                                                                        union
                                                                        select  count(1) as pnum ,  b.encode createuserdeptcode ,rankname  from v_basehiddeninfo a
                                                                        left join 
                                                                        (
                                                                           select encode ,fullname,sortcode from base_department  where nature='厂级' and  encode = '{0}'
                                                                        ) b  on  a.createuserdeptcode = b.encode 
                                                                        where a.createuserdeptcode  = '{0}'  and   {1} group by  b.encode,rankname 
                                                                    ) pivot (sum(pnum) for rankname in ('一般隐患' as OrdinaryHid,'重大隐患' as ImportanHid))
                                                            ) a 
                                                            left join base_department b on a.createuserdeptcode = b.encode group by b.encode,b.fullname
                                                    ) b on a.encode = b.createuserdeptcode  
                                                    union
                                                    select a.encode  as createuserdeptcode,a.fullname, nvl(b.OrdinaryHid,0) as  OrdinaryHid, nvl(b.ImportanHid,0) as ImportanHid,nvl(b.total,0) as total ,a.sortcode,a.departmentid,a.parentid from
                                                    (select encode ,fullname,sortcode,departmentid,parentid from base_department  where nature!='部门' and nature !='厂级'  and  encode like '{0}%' ) a
                                                     left join (
                                                       select b.encode createuserdeptcode,sum(nvl(a.OrdinaryHid,0)) OrdinaryHid,sum(nvl(a.ImportanHid,0)) ImportanHid,sum(nvl(a.OrdinaryHid,0) + nvl(a.ImportanHid,0)) as total,b.fullname  from 
                                                        (
                                                                select * from (
                                                                    select  count(1) as pnum ,  b.encode createuserdeptcode ,rankname  from v_basehiddeninfo a
                                                                    left join 
                                                                    (
                                                                       select encode ,fullname,sortcode from base_department  where nature!='部门' and nature !='厂级'  and  encode like '{0}%'
                                                                    ) b  on  a.createuserdeptcode = b.encode 
                                                                    where a.createuserdeptcode  like '{0}%'  and    {1} group by  b.encode,rankname 
                                                                ) pivot (sum(pnum) for rankname in ('一般隐患' as OrdinaryHid,'重大隐患' as ImportanHid))
                                                        ) a 
                                                        left join base_department b on a.createuserdeptcode = b.encode group by b.encode,b.fullname
                                                    ) b on a.encode = b.createuserdeptcode  
                                                    order by sortcode   ", sentity.sDeptCode, str);
                        }
                        else //非厂级
                        {
                            sql = string.Format(@"select a.createuserdeptcode,nvl(a.OrdinaryHid,0) OrdinaryHid,nvl(a.ImportanHid,0) ImportanHid,(nvl(a.OrdinaryHid,0) + nvl(a.ImportanHid,0)) as total,b.fullname,b.sortcode ,b.departmentid,'0' parentid  from (
                                                select * from ( select  count(1) as pnum , createuserdeptcode,rankname  from v_basehiddeninfo where createuserdeptcode  like '{0}%' 
                                                and  {1}  group by  createuserdeptcode,rankname  
                                            ) pivot (sum(pnum) for rankname in ('一般隐患' as OrdinaryHid,'重大隐患' as ImportanHid))) a 
                                            left join base_department b on a.createuserdeptcode = b.encode  
                                            order by b.encode ", sentity.sDeptCode, str);
                        }
                    }
                    #endregion
                    break;
                /****隐患整改情况统计图****/
                case "6":
                    #region 隐患整改情况统计图
                    sql = string.Format(@"select a.month,nvl(b.yValue,0) yValue,nvl(c.wValue,0) wValue from (select lpad(level,2,0) as month from dual connect by level <13) a
                                        left join 
                                        (
                                           select count(1) as yValue , to_char(createdate,'MM') as yMonth from v_basehiddeninfo  where workstream in ('隐患验收','复查验证','整改效果评估','整改结束') and 
                                           changedutydepartcode  like '{0}%' and to_char(createdate,'yyyy') ='{1}' and {2}   group by  to_char(createdate,'MM')
                                        ) b on a.month = b.yMonth
                                        left join 
                                        (
                                          select count(1) as wValue, to_char(createdate,'MM') as wMonth from v_basehiddeninfo  where   workstream = '隐患整改' and 
                                           changedutydepartcode  like '{0}%' and to_char(createdate,'yyyy') ='{1}' and {2}  group by  to_char(createdate,'MM')
                                        ) c on a.month = c.wMonth  order by a.month", sentity.sDeptCode, sentity.sYear, str);
                    #endregion
                    break;
                /****隐患整改情况趋势图****/
                case "7":
                    #region 隐患整改情况趋势图

                    sql = string.Format(@"select a.month , nvl(b.aValue,0) aValue , nvl(c.yValue,0) yValue, nvl(d.aiValue,0) aiValue , nvl(e.iValue,0) iValue,
                                           nvl(f.aoValue,0) aoValue , nvl(g.oValue,0) oValue,
                                           round((case when nvl(b.aValue,0) = 0 then 0 else  nvl(c.yValue,0) / nvl(b.aValue,0) * 100 end ),2) aChangeVal,
                                           round((case when nvl(d.aiValue,0) = 0 then 0 else  nvl(e.iValue,0) / nvl(d.aiValue,0) * 100 end ),2) iChangeVal,
                                           round((case when nvl(f.aoValue,0) = 0 then 0 else  nvl(g.oValue,0) / nvl(f.aoValue,0) * 100 end ),2) oChangeVal
                                           from (select lpad(level,2,0) as month from dual connect by level <13) a
                                            left join 
                                            (
                                              select count(1) as aValue, to_char(createdate,'MM') as aMonth  from v_basehiddeninfo  where  workstream in ( '隐患整改','隐患验收','复查验证','整改效果评估','整改结束')   and 
                                              changedutydepartcode  like '{0}%' and to_char(createdate,'yyyy') ='{1}' and {2} group by  to_char(createdate,'MM')
                                            ) b on a.month = b.aMonth 
                                            left join 
                                            (
                                              select count(1) as yValue, to_char(createdate,'MM') as yMonth from v_basehiddeninfo  where  workstream in ( '隐患验收','复查验证','整改效果评估','整改结束')   and 
                                              changedutydepartcode  like '{0}%' and to_char(createdate,'yyyy') ='{1}' and {2} group by  to_char(createdate,'MM')
                                            ) c on a.month = c.yMonth 
                                            left join 
                                            (
                                              select count(1) as aiValue, to_char(createdate,'MM') as aiMonth  from v_basehiddeninfo  where  workstream in ( '隐患整改','隐患验收','复查验证','整改效果评估','整改结束') and rankname in ('重大隐患')  and 
                                              changedutydepartcode  like '{0}%' and to_char(createdate,'yyyy') ='{1}' and {2} group by  to_char(createdate,'MM')
                                            ) d on a.month = d.aiMonth
                                            left join 
                                            (
                                              select count(1) as iValue, to_char(createdate,'MM') as iMonth from v_basehiddeninfo  where  workstream in ( '隐患验收','复查验证','整改效果评估','整改结束') and rankname in ('重大隐患')  and 
                                              changedutydepartcode  like '{0}%' and to_char(createdate,'yyyy') ='{1}' and {2} group by  to_char(createdate,'MM')
                                            ) e on a.month = e.iMonth 
                                            left join 
                                            (
                                              select count(1) as aoValue, to_char(createdate,'MM') as aoMonth  from v_basehiddeninfo  where  workstream in ( '隐患整改','隐患验收','复查验证','整改效果评估','整改结束') and rankname in ('一般隐患')  and 
                                              changedutydepartcode  like '{0}%' and to_char(createdate,'yyyy') ='{1}' and {2} group by  to_char(createdate,'MM')
                                            ) f on a.month = f.aoMonth 
                                            left join 
                                            (
                                              select count(1) as oValue, to_char(createdate,'MM') as oMonth from v_basehiddeninfo  where  workstream in ( '隐患验收','复查验证','整改效果评估','整改结束') and rankname in ('一般隐患')  and 
                                              changedutydepartcode  like '{0}%' and to_char(createdate,'yyyy') ='{1}' and {2} group by  to_char(createdate,'MM')
                                            ) g on a.month = g.oMonth 
                                             order by a.month ", sentity.sDeptCode, sentity.sYear, str);
                    #endregion
                    break;
                /****隐患整改情况对比图****/
                case "8":
                    #region 隐患整改情况对比图
                    if (sentity.sMark == 0)
                    {
                        //厂级  所有部门
                        if (sentity.isCompany)
                        {
                            sql = string.Format(@"select a.encode as changedutydepartcode,a.fullname, nvl(b.nonChange,0) as  nonChange, nvl(b.thenChange,0) as thenChange,nvl(b.total,0) as total ,a.sortcode  
                                             from (select encode ,fullname,sortcode from base_department  where (nature='部门' or nature ='厂级')  and  encode like '{0}%' ) a
                                            left join (   
                                                    select b.encode changedutydepartcode,sum(nvl(a.nonChange,0)) nonChange,sum(nvl(a.thenChange,0)) thenChange ,sum(nvl(a.nonChange,0) + nvl(a.thenChange,0)) as total,b.fullname  from (
                                                         select * from (
                                                            select  count(1) as pValue, changedutydepartcode,'已整改' changestatus  from v_basehiddeninfo  where  workstream in ('隐患验收','复查验证','整改效果评估','整改结束')  and 
                                                            changedutydepartcode  like '{0}%'   and  {1}  group by changedutydepartcode
                                                            union 
                                                            select  count(1) as pValue, changedutydepartcode,'未整改' changestatus  from v_basehiddeninfo  where  workstream in ('隐患整改')  and 
                                                            changedutydepartcode  like '{0}%'    and  {1}   group by  changedutydepartcode
                                                        ) pivot (sum(pValue) for changestatus in ('未整改' as nonChange,'已整改' as thenChange))
                                                    ) a 
                                                    left join (select encode ,fullname,sortcode from base_department  where nature='部门'  and  encode like '{0}%') b on 
                                                    substr(a.changedutydepartcode,0,length(b.encode)) = b.encode  group by  b.encode,b.fullname
                                                    union
                                                    select b.encode changedutydepartcode,sum(nvl(a.nonChange,0)) nonChange,sum(nvl(a.thenChange,0)) thenChange ,sum(nvl(a.nonChange,0) + nvl(a.thenChange,0)) as total,b.fullname  from (
                                                         select * from (
                                                            select  count(1) as pValue, changedutydepartcode,'已整改' changestatus  from v_basehiddeninfo  where  workstream in ('隐患验收','复查验证','整改效果评估','整改结束')  and 
                                                            changedutydepartcode  = '{0}'   and  {1}  group by changedutydepartcode
                                                            union 
                                                            select  count(1) as pValue, changedutydepartcode,'未整改' changestatus  from v_basehiddeninfo  where  workstream in ('隐患整改')  and 
                                                            changedutydepartcode  = '{0}'   and   {1}   group by  changedutydepartcode
                                                        ) pivot (sum(pValue) for changestatus in ('未整改' as nonChange,'已整改' as thenChange))
                                                    ) a 
                                                    left join (select encode ,fullname,sortcode from base_department  where nature='厂级'  and  encode = '{0}') b on 
                                                    a.changedutydepartcode = b.encode  group by  b.encode,b.fullname
                                              ) b on a.encode = b.changedutydepartcode  order by a.sortcode  ", sentity.sDeptCode, str);
                        }
                        else //非厂级
                        {
                            sql = string.Format(@"select a.changedutydepartcode,nvl(a.nonChange,0) nonChange,nvl(a.thenChange,0) thenChange ,(nvl(a.nonChange,0) + nvl(a.thenChange,0)) as total,b.fullname , b.sortcode from (
                                                  select * from (
                                                        select  count(1) as pValue, changedutydepartcode ,'已整改' changestatus  from v_basehiddeninfo  where  workstream in ( '隐患验收','复查验证','整改效果评估','整改结束')  and 
                                                        changedutydepartcode  like '{0}%' and    {1}   group by  changedutydepartcode
                                                        union 
                                                        select  count(1) as pValue, changedutydepartcode ,'未整改' changestatus  from v_basehiddeninfo  where  workstream in ( '隐患整改')  and 
                                                        changedutydepartcode  like '{0}%' and    {1}  group by  changedutydepartcode
                                                    ) pivot (sum(pValue) for changestatus in ('未整改' as nonChange,'已整改' as thenChange))
                                                  ) a 
                                                  left join base_department b on a.changedutydepartcode = b.encode order by b.encode ", sentity.sDeptCode, str);
                        }
                    }
                    else if (sentity.sMark == 1)
                    {
                        DepartmentEntity dentity = Idepartmentservice.GetEntityByCode(sentity.sDeptCode);

                        if (dentity.Nature == "厂级")
                        {
                            sql = string.Format(@"select a.changedutydepartcode,nvl(a.nonChange,0) nonChange,nvl(a.thenChange,0) thenChange ,(nvl(a.nonChange,0) + nvl(a.thenChange,0)) as total,b.fullname , b.sortcode from (
                                                  select * from (
                                                        select  count(1) as pValue, changedutydepartcode ,'已整改' changestatus  from v_basehiddeninfo  where  workstream in ( '隐患验收','复查验证','整改效果评估','整改结束')  and 
                                                        changedutydepartcode  = '{0}' and    {1}   group by  changedutydepartcode
                                                        union 
                                                        select  count(1) as pValue, changedutydepartcode ,'未整改' changestatus  from v_basehiddeninfo  where  workstream in ( '隐患整改')  and 
                                                        changedutydepartcode  = '{0}' and    {1}  group by  changedutydepartcode
                                                    ) pivot (sum(pValue) for changestatus in ('未整改' as nonChange,'已整改' as thenChange))
                                                  ) a 
                                                  left join base_department b on a.changedutydepartcode = b.encode order by b.sortcode  ", sentity.sDeptCode, str);
                        }
                        else
                        {
                            sql = string.Format(@"select a.changedutydepartcode,nvl(a.nonChange,0) nonChange,nvl(a.thenChange,0) thenChange ,(nvl(a.nonChange,0) + nvl(a.thenChange,0)) as total,b.fullname , b.sortcode from (
                                                  select * from (
                                                        select  count(1) as pValue, changedutydepartcode ,'已整改' changestatus  from v_basehiddeninfo  where  workstream in ( '隐患验收','复查验证','整改效果评估','整改结束')  and 
                                                        changedutydepartcode  like '{0}%' and    {1}   group by  changedutydepartcode
                                                        union 
                                                        select  count(1) as pValue, changedutydepartcode ,'未整改' changestatus  from v_basehiddeninfo  where  workstream in ( '隐患整改')  and 
                                                        changedutydepartcode  like '{0}%' and    {1}  group by  changedutydepartcode
                                                    ) pivot (sum(pValue) for changestatus in ('未整改' as nonChange,'已整改' as thenChange))
                                                  ) a 
                                                  left join base_department b on a.changedutydepartcode = b.encode order by b.encode ", sentity.sDeptCode, str);
                        }
                    }
                    else if (sentity.sMark == 2)
                    {
                        //厂级
                        if (sentity.isCompany)
                        {
                            sql = string.Format(@"select a.encode as changedutydepartcode,a.fullname, nvl(b.nonChange,0) as  nonChange, nvl(b.thenChange,0) as thenChange,nvl(b.total,0) as total ,a.sortcode ,a.departmentid,'0' parentid  
                                             from (select encode ,fullname,sortcode,departmentid,parentid from base_department  where (nature='部门' or nature ='厂级')  and  encode like '{0}%' ) a
                                            left join (   
                                                    select b.encode changedutydepartcode,sum(nvl(a.nonChange,0)) nonChange,sum(nvl(a.thenChange,0)) thenChange ,sum(nvl(a.nonChange,0) + nvl(a.thenChange,0)) as total,b.fullname  from (
                                                         select * from (
                                                            select  count(1) as pValue, changedutydepartcode,'已整改' changestatus  from v_basehiddeninfo  where  workstream in ('隐患验收','复查验证','整改效果评估','整改结束')  and 
                                                            changedutydepartcode  like '{0}%'   and  {1}  group by changedutydepartcode
                                                            union 
                                                            select  count(1) as pValue, changedutydepartcode,'未整改' changestatus  from v_basehiddeninfo  where  workstream in ('隐患整改')  and 
                                                            changedutydepartcode  like '{0}%'    and  {1}   group by  changedutydepartcode
                                                        ) pivot (sum(pValue) for changestatus in ('未整改' as nonChange,'已整改' as thenChange))
                                                    ) a 
                                                    left join (select encode ,fullname,sortcode from base_department  where nature='部门'  and  encode like '{0}%') b on 
                                                    substr(a.changedutydepartcode,0,length(b.encode)) = b.encode  group by  b.encode,b.fullname
                                                    union
                                                    select b.encode changedutydepartcode,sum(nvl(a.nonChange,0)) nonChange,sum(nvl(a.thenChange,0)) thenChange ,sum(nvl(a.nonChange,0) + nvl(a.thenChange,0)) as total,b.fullname  from (
                                                         select * from (
                                                            select  count(1) as pValue, changedutydepartcode,'已整改' changestatus  from v_basehiddeninfo  where  workstream in ('隐患验收','复查验证','整改效果评估','整改结束')  and 
                                                            changedutydepartcode  = '{0}'   and  {1}  group by changedutydepartcode
                                                            union 
                                                            select  count(1) as pValue, changedutydepartcode,'未整改' changestatus  from v_basehiddeninfo  where  workstream in ('隐患整改')  and 
                                                            changedutydepartcode  = '{0}'   and   {1}   group by  changedutydepartcode
                                                        ) pivot (sum(pValue) for changestatus in ('未整改' as nonChange,'已整改' as thenChange))
                                                    ) a 
                                                    left join (select encode ,fullname,sortcode from base_department  where nature='厂级'  and  encode = '{0}') b on 
                                                    a.changedutydepartcode = b.encode  group by  b.encode,b.fullname
                                              ) b on a.encode = b.changedutydepartcode 
                                              union
                                              select a.encode as changedutydepartcode,a.fullname, nvl(b.nonChange,0) as  nonChange, nvl(b.thenChange,0) as thenChange,nvl(b.total,0) as total ,a.sortcode ,a.departmentid,a.parentid 
                                             from (select encode ,fullname,sortcode,departmentid,parentid from base_department  where nature!='部门' and nature !='厂级'  and  encode like '{0}%' ) a
                                            left join (   
                                                    select b.encode changedutydepartcode,sum(nvl(a.nonChange,0)) nonChange,sum(nvl(a.thenChange,0)) thenChange ,sum(nvl(a.nonChange,0) + nvl(a.thenChange,0)) as total,b.fullname  from (
                                                         select * from (
                                                            select  count(1) as pValue, changedutydepartcode,'已整改' changestatus  from v_basehiddeninfo  where  workstream in ('隐患验收','复查验证','整改效果评估','整改结束')  and 
                                                            changedutydepartcode  like '{0}%'   and    {1} group by changedutydepartcode
                                                            union 
                                                            select  count(1) as pValue, changedutydepartcode,'未整改' changestatus  from v_basehiddeninfo  where  workstream in ('隐患整改')  and 
                                                            changedutydepartcode  like '{0}%'   and    {1}  group by  changedutydepartcode
                                                        ) pivot (sum(pValue) for changestatus in ('未整改' as nonChange,'已整改' as thenChange))
                                                    ) a 
                                                    left join (select encode ,fullname,sortcode from base_department  where nature!='部门' and nature !='厂级'  and  encode like  '{0}%') b on 
                                                    a.changedutydepartcode = b.encode  group by  b.encode,b.fullname
                                              ) b on a.encode = b.changedutydepartcode
                                                order by sortcode ", sentity.sDeptCode, str);
                        }
                        else //非厂级
                        {
                            sql = string.Format(@"select a.changedutydepartcode,nvl(a.nonChange,0) nonChange,nvl(a.thenChange,0) thenChange ,(nvl(a.nonChange,0) + nvl(a.thenChange,0)) as total,b.fullname , b.sortcode ,b.departmentid,'0' parentid from (
                                                  select * from (
                                                        select  count(1) as pValue, changedutydepartcode ,'已整改' changestatus  from v_basehiddeninfo  where  workstream in ( '隐患验收','复查验证','整改效果评估','整改结束')  and 
                                                        changedutydepartcode  like '{0}%' and    {1}   group by  changedutydepartcode
                                                        union 
                                                        select  count(1) as pValue, changedutydepartcode ,'未整改' changestatus  from v_basehiddeninfo  where  workstream in ( '隐患整改')  and 
                                                        changedutydepartcode  like '{0}%' and    {1}  group by  changedutydepartcode
                                                    ) pivot (sum(pValue) for changestatus in ('未整改' as nonChange,'已整改' as thenChange))
                                                  ) a 
                                                  left join base_department b on a.changedutydepartcode = b.encode order by b.encode ", sentity.sDeptCode, str);
                        }
                    }
                    #endregion
                    break;
                /********检查隐患列表******/
                case "9":
                    #region 检查隐患列表
                    sql = string.Format(@"select a.month,  nvl(b.OrdinaryHid,0) OrdinaryHid,  nvl(b.ImportanHid,0) ImportanHid ,(nvl(b.OrdinaryHid,0) + nvl(b.ImportanHid,0))  total from (select lpad(level,2,0) as month from dual connect by level <13) a
                                            left join ( 
                                                     select * from (
                                                      select  count(1) as pnum , to_char(createdate,'MM') as tMonth ,rankname from v_basehiddeninfo where changedutydepartcode  like '{0}%' 
                                                          and to_char(createdate,'yyyy') ='{1}' and  {2}  group by to_char(createdate,'MM') ,rankname 
                                                          ) pivot (sum(pnum) for rankname in ('一般隐患' as OrdinaryHid,'重大隐患' as ImportanHid))
                                            ) b on a.month =b.tMonth  order by a.month", sentity.sDeptCode, sentity.sYear, str);
                    #endregion
                    break;
                /****隐患整改情况区域对比图****/
                case "10":
                    #region 隐患整改情况区域对比图
                    sql = string.Format(@"  select a.districtcode  hidpoint,a.districtname  hidpointname,nvl(b.nonChange,0) as nonChange,nvl(b.thenChange,0) as thenChange,nvl(b.total,0) as total,
                                              a.organizeid from (
                                                  select districtcode ,districtname ,sortcode ,organizeid  from bis_district  where organizeid ='{0}' and parentid ='0'
                                                ) a
                                              left join (                                            
                                                           select a.districtcode hidpoint, sum(nvl(b.nonChange,0)) nonChange, sum(nvl(b.thenChange,0)) thenChange , sum(nvl(b.nonChange,0) + nvl(b.thenChange,0)) as total from
                                                          （
                                                             select districtcode ,districtname ,sortcode ,organizeid  from bis_district  where organizeid ='{0}' and parentid ='0'
                                                           ） a left join 
                                                           (
                                                              select * from
                                                               (
                                                                    select  count(1) as pValue,  hidpoint,'已整改' changestatus  from v_basehiddeninfo  where  workstream in ('隐患验收','复查验证','整改效果评估','整改结束')  and 
                                                                    changedutydepartcode  like '{1}%' and    {2} group by  hidpoint
                                                                    union 
                                                                    select  count(1) as pValue,  hidpoint,'未整改' changestatus  from v_basehiddeninfo  where  workstream in ('隐患整改')  and 
                                                                    changedutydepartcode  like '{1}%' and    {2}  group by  hidpoint
                                                               ) pivot (sum(pValue) for changestatus in ('未整改' as nonChange,'已整改' as thenChange))
                                                           ) b    on  a.districtcode =  substr(b.hidpoint,0, length(a.districtcode))  group by a.districtcode
                                             )  b on  a.districtcode =  b.hidpoint order by a.sortcode ", sentity.sOrganize, sentity.sDeptCode, str);
                    #endregion
                    break;
                /********省公司检查隐患列表******/
                case "11":
                    #region 省公司检查隐患列表
                    sql = string.Format(@"select a.month,  nvl(b.OrdinaryHid,0) OrdinaryHid,  nvl(b.ImportanHid,0) ImportanHid ,(nvl(b.OrdinaryHid,0) + nvl(b.ImportanHid,0))  total from (select lpad(level,2,0) as month from dual connect by level <13) a
                                            left join ( 
                                                     select * from (
                                                      select  count(1) as pnum , to_char(createdate,'MM') as tMonth ,rankname from v_basehiddeninfo where changedutydepartcode in (select  encode from BASE_DEPARTMENT  start with encode='{0}' connect by  prior  departmentid = parentid) 
                                                          and to_char(createdate,'yyyy') ='{1}' and  {2}  group by to_char(createdate,'MM') ,rankname 
                                                          ) pivot (sum(pnum) for rankname in ('一般隐患' as OrdinaryHid,'重大隐患' as ImportanHid))
                                            ) b on a.month =b.tMonth  order by a.month", sentity.sDeptCode, sentity.sYear, str);
                    #endregion
                    break;
                /****省公司各等级隐患情况统计****/
                case "12":
                    #region 省公司各等级隐患情况统计
                    Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                    sql = string.Format(@"select a.changedutydepartcode,nvl(a.OrdinaryHid,0) OrdinaryHid,nvl(a.ImportanHid,0) ImportanHid,(nvl(a.OrdinaryHid,0) + nvl(a.ImportanHid,0)) as total,a.changedutydepartcode as fullname,a.sortCode  from (
                            select * from (select  count(1) as pnum , '电厂' as changedutydepartcode,rankname,1 as sortCode  from v_basehiddeninfo 
                            where changedutydepartcode in (select  encode from BASE_DEPARTMENT  start with encode='{0}' connect by  prior  departmentid = parentid)  and to_char(createdate,'yyyy') ='{1}' and {2}
                            and safetycheckobjectid in (select id from  bis_saftycheckdatarecord where ((datatype=2 and checkeddepartid in(select departmentid from BASE_DEPARTMENT  where (nature='厂级' or nature='电厂') start with encode='{0}' connect by  prior  departmentid = parentid)) or (datatype=0 and createuserorgcode in (select  encode from BASE_DEPARTMENT  where (nature='厂级' or nature='电厂') start with encode='{0}' connect by  prior  departmentid = parentid))) )
                                group by  changedutydepartcode,rankname union all
                                                        select  0 as pnum , '电厂' as changedutydepartcode,'一般隐患' as RANKNAME,1 as sortCode  from dual  union all
                                                        select  count(1) as pnum , '省公司' as changedutydepartcode,rankname,0 as sortCode  from v_basehiddeninfo 
                            where changedutydepartcode  in (select  encode from BASE_DEPARTMENT  start with encode='{3}' connect by  prior  departmentid = parentid)  and to_char(createdate,'yyyy') ='{1}' and {2}
                            and safetycheckobjectid in (select id from bis_saftycheckdatarecord where (datatype=0 and checkeddepartid in (select  departmentid from BASE_DEPARTMENT  where (nature='厂级' or nature='电厂') start with encode='{3}' connect by  prior  departmentid = parentid)) )
                            group by  changedutydepartcode,rankname union all
                                                        select  0 as pnum , '省公司' as changedutydepartcode,'一般隐患' as RANKNAME,0 as sortCode  from dual
                                                    ) pivot (sum(pnum) for rankname in ('一般隐患' as OrdinaryHid,'重大隐患' as ImportanHid))) a 
                        order by a.sortCode ", sentity.sDeptCode, sentity.sYear, str, user.OrganizeCode);
                    #endregion
                    break;
                /****省公司隐患数量变化趋势图****/
                case "13":
                    #region 省公司隐患数量变化趋势图
                    if (mark == 0)
                    {
                        sql = string.Format(@"select a.month,nvl(b.allhid,0) allhid  from (select lpad(level,2,0) as month from dual connect by level <13) a
                                              left join (
                                                   select * from (
                                                           select  count(1) as pnum , to_char(createdate,'MM') as tMonth ,'所有隐患' as rankname from v_basehiddeninfo where changedutydepartcode  in (select  encode from BASE_DEPARTMENT  start with encode='{0}' connect by  prior  departmentid = parentid) and to_char(createdate,'yyyy') ='{1}' and {2}    group by to_char(createdate,'MM')
                                                       ) pivot (sum(pnum) for rankname in ('所有隐患' as AllHid))
                                              ) b on a.month =b.tMonth order by a.month", sentity.sDeptCode, sentity.sYear, str);
                    }
                    else if (mark == 1)
                    {
                        tempStr = sentity.sHidRank == "一般隐患" ? " '一般隐患' as OrdinaryHid " : " '重大隐患' as ImportanHid";
                        string tempField = sentity.sHidRank == "一般隐患" ? "OrdinaryHid" : "ImportanHid";

                        sql = string.Format(@"select a.month, nvl(b.{4},0) {4}  from (select lpad(level,2,0) as month from dual connect by level <13) a
                                              left join (
                                                     select * from (    
                                                       select  count(1) as pnum , to_char(createdate,'MM') as tMonth ,rankname from v_basehiddeninfo where changedutydepartcode  in (select  encode from BASE_DEPARTMENT  start with encode='{0}' connect by  prior  departmentid = parentid) and to_char(createdate,'yyyy') ='{1}'  and  {2} group by to_char(createdate,'MM') ,rankname 
                                                   ) pivot (sum(pnum) for rankname in ({3})) 
                                               ) b on a.month =b.tMonth order by a.month",
                                      sentity.sDeptCode, sentity.sYear, str, tempStr, tempField);
                    }
                    else
                    {
                        sql = string.Format(@"select a.month,  nvl(b.OrdinaryHid,0) OrdinaryHid,  nvl(b.ImportanHid,0) ImportanHid from (select lpad(level,2,0) as month from dual connect by level <13) a
                                              left join ( 
                                                       select * from (
                                                        select  count(1) as pnum , to_char(createdate,'MM') as tMonth ,rankname from v_basehiddeninfo where changedutydepartcode  in (select  encode from BASE_DEPARTMENT  start with encode='{0}' connect by  prior  departmentid = parentid) and to_char(createdate,'yyyy') ='{1}'  and  {2}  group by to_char(createdate,'MM') ,rankname 
                                                            ) pivot (sum(pnum) for rankname in ('一般隐患' as OrdinaryHid,'重大隐患' as ImportanHid))
                                              ) b on a.month =b.tMonth order by a.month",
                                                  sentity.sDeptCode, sentity.sYear, str);
                    }
                    #endregion
                    break;
                /*班组隐患变化趋势统计图(按创建单位)*/
                case "14":
                    sql = string.Format(@"select a.month, nvl(b.OrdinaryHid,0) OrdinaryHid ,nvl(b.ImportanHid,0) ImportanHid from (select lpad(level,2,0) as month from dual connect by level <13) a
                                              left join (
                                                     select * from (    
                                                       select  count(1) as pnum , to_char(createdate,'MM') as tMonth ,rankname from v_basehiddeninfo where createuserdeptcode  like '{0}%' 
                                                       and to_char(createdate,'yyyy') ='{1}' group by to_char(createdate,'MM') ,rankname 
                                                   ) pivot (sum(pnum) for rankname in ( '一般隐患'as OrdinaryHid,'重大隐患' as ImportanHid )) 
                                               ) b on a.month =b.tMonth order by a.month",
                                  sentity.sDeptCode, sentity.sYear);
                    break;
                /*班组隐患数量统计图(按创建单位)*/
                case "15":
                    sql = string.Format(@"select nvl(a.OrdinaryHid,0) OrdinaryHid,nvl(a.ImportanHid,0) ImportanHid,(nvl(a.OrdinaryHid,0) + nvl(a.ImportanHid,0)) as total  from (
                                                select * from ( select  count(1) as pnum , rankname  from v_basehiddeninfo where createuserdeptcode  like '{0}%' 
                                                and to_char(createdate,'yyyy') ='{1}'   group by  rankname  
                                            ) pivot (sum(pnum) for rankname in ('一般隐患' as OrdinaryHid,'重大隐患' as ImportanHid))) a ", sentity.sDeptCode, sentity.sYear);
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

        #region 隐患统计(省级)
        /// <summary>
        /// 省级统计数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DataTable QueryProvStatisticsByAction(ProvStatisticsEntity entity)
        {
            string sql = string.Empty;

            string str = " 1=1";

            string tempstr = " 1=1";

            string areastr = " 1=1 and  a.parentid = '0' ";

            Operator cuUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            int mark = 0;  //标记变量  0 无隐患级别条件 ，1 标识1哥隐患级别， 2 标识两个隐患级别条件

            string encode = string.Empty;

            string newdeptcode = string.Empty; //部门编码

            DepartmentEntity deptEntity = new DepartmentEntity();

            //年度
            if (!entity.sYear.IsEmpty())
            {
                str += string.Format(" and to_char(createdate,'yyyy') = '{0}' ", entity.sYear);
            }
            //部门为空
            if (entity.sDepartId.IsEmpty())
            {
                entity.sDepartId = cuUser.OrganizeId;//如果部门为传入，则传入当前用户的部门机构id
            }

            deptEntity = Idepartmentservice.GetEntity(entity.sDepartId);

            encode = deptEntity.EnCode;  //老部门编码

            newdeptcode = deptEntity.DeptCode; //新部门编码

            //厂级临时条件
            if (deptEntity.Nature == "厂级")
            {
                str += string.Format("  and  hiddepart ='{0}'", entity.sDepartId);

                tempstr += string.Format(" and nature='部门'  and  deptcode like '{0}%'", newdeptcode);

                areastr += string.Format(" and  a.organizeid ='{0}'", entity.sDepartId);
            }
            else if (deptEntity.Nature == "省级")
            {
                str += string.Format("  and  changedeptcode  like '{0}%'", newdeptcode);

                tempstr += string.Format(" and nature='厂级'  and  deptcode like '{0}%' and description is null", newdeptcode);
            }
            //整改部门
            if (!entity.sDepartCode.IsEmpty())
            {
                var tedept = Idepartmentservice.GetEntityByCode(entity.sDepartCode);
                str += string.Format(" and changedeptcode like '{0}%'", tedept.DeptCode);
            }
            //创建时间
            if (!entity.sStartDate.IsEmpty())
            {
                str += string.Format(" and createdate >= to_date('{0}','yyyy-mm-dd hh24:mi:ss') ", entity.sStartDate);
            }
            //截止时间
            if (!entity.sEndDate.IsEmpty())
            {
                str += string.Format(" and createdate < to_date('{0}','yyyy-mm-dd hh24:mi:ss') ", Convert.ToDateTime(entity.sEndDate).AddDays(1).ToString("yyyy-MM-dd"));
            }
            //区域范围
            if (!entity.sArea.IsEmpty())
            {
                str += string.Format("  and hidpoint like '{0}%'", entity.sArea.ToString());
            }
            //隐患级别
            if (!entity.sHidRank.IsEmpty())
            {
                string[] tempRank = entity.sHidRank.Split(',');

                mark = tempRank.Length;  //标记变量

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
            //统计类型
            if (!entity.statType.IsEmpty())
            {
                if (entity.statType == "已闭环")
                {
                    str += string.Format(" and  workstream ='整改结束' ");
                }
                if (entity.statType == "未闭环")
                {
                    str += string.Format(" and  workstream !='整改结束' ");
                }
            }

            switch (entity.sAction)
            {
                //隐患等级数量列表
                case "1":
                    #region 省级用户查看电厂数据
                    sql = string.Format(@"select a.districtcode as hidpoint,a.districtname as hidpointname,nvl(b.OrdinaryHid,0) as OrdinaryHid,nvl(b.ImportanHid,0) as ImportanHid,nvl(b.total,0) as total ,
                                            a.organizeid from ( select districtcode ,districtname ,sortcode，organizeid  from bis_district a  where {1} ) a
                                            left join (
                                                        select a.hidpoint,a.hidpointname,nvl(a.OrdinaryHid,0) OrdinaryHid,nvl(a.ImportanHid,0) ImportanHid,(nvl(a.OrdinaryHid,0) + nvl(a.ImportanHid,0)) as total  from (
                                                        select * from (
                                                        select a.districtcode as hidpoint,a.districtname as hidpointname,a.organizeid ,nvl(b.y,0) as pnum ,b.rankname from bis_district a
                                                            left join (
                                                            select  count(1) as y , hidpoint,rankname   from v_basehiddeninfo 
                                                            where {0} group by hidpoint,rankname 
                                                            ) b on a.districtcode =  substr(b.hidpoint,0, length(a.districtcode))
                                                            where {1}
                                                        ) pivot (sum(pnum) for rankname in ('一般隐患' as OrdinaryHid,'重大隐患' as ImportanHid))) a    
                                             )  b on a.districtcode =  b.hidpoint  order by a.sortcode ", str, areastr);
                    #endregion
                    break;
                //隐患等级数量统计图
                case "2":
                    #region 隐患等级数量统计图
                    sql = string.Format(@"select  count(1) as y ,rankname  as name from v_basehiddeninfo where {0} 
                                          group by  rankname  ", str);
                    #endregion
                    break;
                //隐患区域分布情况图
                case "3":
                    #region 隐患区域分布情况图
                    sql = string.Format(@"select a.districtcode as hidpoint,a.districtname as name,nvl(b.y,0) as y ,a.organizeid from ( select districtcode ,districtname ,sortcode，organizeid  
                                                 from bis_district a where {0}) a
                                                 left join (
                                                            select a.districtcode as  hidpoint,a.districtname as name,a.organizeid ,sum(nvl(b.y,0)) as y from bis_district a
                                                            left join (
                                                                select  count(1) as y , hidpoint  from v_basehiddeninfo 
                                                                where  {1} group by hidpoint
                                                            ) b on a.districtcode =  substr(b.hidpoint,0, length(a.districtcode))
                                                            where  {0} group by 
                                                            a.districtcode ,a.districtname ,a.organizeid
                                                 )  b on a.districtcode =  b.hidpoint  order by a.sortcode", areastr, str);

                    #endregion
                    break;
                /****隐患数量变化趋势图****/
                case "4":
                    #region 隐患数量变化趋势图
                    if (mark == 0)
                    {
                        sql = string.Format(@"select a.month,nvl(b.allhid,0) allhid  from (select lpad(level,2,0) as month from dual connect by level <13) a
                                              left join (
                                                   select * from (
                                                           select  count(1) as pnum , to_char(createdate,'MM') as tMonth ,'所有隐患' as rankname from v_basehiddeninfo where {0} group by to_char(createdate,'MM')
                                                       ) pivot (sum(pnum) for rankname in ('所有隐患' as AllHid))
                                              ) b on a.month =b.tMonth order by a.month", str);
                    }
                    else if (mark == 1)
                    {
                        string tempStr = entity.sHidRank == "一般隐患" ? " '一般隐患' as OrdinaryHid " : " '重大隐患' as ImportanHid";
                        string tempField = entity.sHidRank == "一般隐患" ? "OrdinaryHid" : "ImportanHid";

                        sql = string.Format(@"select a.month, nvl(b.{2},0) {2}  from (select lpad(level,2,0) as month from dual connect by level <13) a
                                              left join (
                                                     select * from (    
                                                       select  count(1) as pnum , to_char(createdate,'MM') as tMonth ,rankname from v_basehiddeninfo where  {0} group by to_char(createdate,'MM') ,rankname 
                                                   ) pivot (sum(pnum) for rankname in ({1})) 
                                               ) b on a.month =b.tMonth order by a.month", str, tempStr, tempField);
                    }
                    else
                    {
                        sql = string.Format(@"select a.month,  nvl(b.OrdinaryHid,0) OrdinaryHid,  nvl(b.ImportanHid,0) ImportanHid from (select lpad(level,2,0) as month from dual connect by level <13) a
                                              left join ( 
                                                       select * from (
                                                        select  count(1) as pnum , to_char(createdate,'MM') as tMonth ,rankname from v_basehiddeninfo where  {0}  group by to_char(createdate,'MM') ,rankname 
                                                            ) pivot (sum(pnum) for rankname in ('一般隐患' as OrdinaryHid,'重大隐患' as ImportanHid))
                                              ) b on a.month =b.tMonth order by a.month", str);
                    }
                    #endregion
                    break;
                /******各等级隐患情况统计******/
                case "5":
                    #region 各等级隐患情况统计
                    sql = string.Format(@"select a.departmentid  hiddepart ,a.encode,a.fullname, nvl(b.OrdinaryHid,0) as  OrdinaryHid, nvl(b.ImportanHid,0) as ImportanHid,nvl(b.total,0) as total ,a.sortcode from
                                           (select departmentid ,fullname,sortcode ,encode from base_department  where {0}) a
                                             left join (
                                                   select b.departmentid hiddepart,sum(nvl(a.OrdinaryHid,0)) OrdinaryHid,sum(nvl(a.ImportanHid,0)) ImportanHid,sum(nvl(a.OrdinaryHid,0) + nvl(a.ImportanHid,0)) as total,b.fullname  from 
                                                    (
                                                            select * from (
                                                            select  count(1) as pnum ,   b.encode changedutydepartcode ,rankname  from v_basehiddeninfo a
                                                            left join 
                                                            (
                                                               select departmentid ,fullname, encode, sortcode from base_department  where {0}
                                                            ) b  on  substr(a.changedutydepartcode,0,length(b.encode)) = b.encode 
                                                            where  {1} group by  b.encode,rankname  
                                                            ) pivot (sum(pnum) for rankname in ('一般隐患' as OrdinaryHid,'重大隐患' as ImportanHid))
                                                    ) a 
                                                    left join base_department b on a.changedutydepartcode = b.encode group by b.departmentid,b.fullname
                                            ) b on a.departmentid = b.hiddepart  order by a.sortcode ", tempstr, str);
                    #endregion
                    break;
                /****隐患整改情况统计图****/
                case "6":
                    #region 隐患整改情况统计图
                    sql = string.Format(@"select a.month,nvl(b.yValue,0) yValue,nvl(c.wValue,0) wValue from (select lpad(level,2,0) as month from dual connect by level <13) a
                                        left join 
                                        (
                                           select count(1) as yValue , to_char(createdate,'MM') as yMonth from v_basehiddeninfo  where workstream in ('隐患验收','复查验证','整改效果评估','整改结束') and 
                                              {0}   group by  to_char(createdate,'MM')
                                        ) b on a.month = b.yMonth
                                        left join 
                                        (
                                          select count(1) as wValue, to_char(createdate,'MM') as wMonth from v_basehiddeninfo  where   workstream = '隐患整改' and 
                                              {0}  group by  to_char(createdate,'MM')
                                        ) c on a.month = c.wMonth  order by a.month", str);
                    #endregion
                    break;
                /****隐患整改情况趋势图****/
                case "7":
                    #region 隐患整改情况趋势图
                    sql = string.Format(@"select a.month , nvl(b.aValue,0) aValue , nvl(c.yValue,0) yValue, nvl(d.aiValue,0) aiValue , nvl(e.iValue,0) iValue,
                                           nvl(f.aoValue,0) aoValue , nvl(g.oValue,0) oValue,
                                           round((case when nvl(b.aValue,0) = 0 then 0 else  nvl(c.yValue,0) / nvl(b.aValue,0) * 100 end ),2) aChangeVal,
                                           round((case when nvl(d.aiValue,0) = 0 then 0 else  nvl(e.iValue,0) / nvl(d.aiValue,0) * 100 end ),2) iChangeVal,
                                           round((case when nvl(f.aoValue,0) = 0 then 0 else  nvl(g.oValue,0) / nvl(f.aoValue,0) * 100 end ),2) oChangeVal
                                           from (select lpad(level,2,0) as month from dual connect by level <13) a
                                            left join 
                                            (
                                              select count(1) as aValue, to_char(createdate,'MM') as aMonth  from v_basehiddeninfo  where  workstream in ( '隐患整改','隐患验收','复查验证','整改效果评估','整改结束')   and 
                                              {0} group by  to_char(createdate,'MM')
                                            ) b on a.month = b.aMonth 
                                            left join 
                                            (
                                              select count(1) as yValue, to_char(createdate,'MM') as yMonth from v_basehiddeninfo  where  workstream in ( '隐患验收','复查验证','整改效果评估','整改结束')   and 
                                               {0} group by  to_char(createdate,'MM')
                                            ) c on a.month = c.yMonth 
                                            left join 
                                            (
                                              select count(1) as aiValue, to_char(createdate,'MM') as aiMonth  from v_basehiddeninfo  where  workstream in ( '隐患整改','隐患验收','复查验证','整改效果评估','整改结束') and rankname = '重大隐患'  and 
                                               {0} group by  to_char(createdate,'MM')
                                            ) d on a.month = d.aiMonth
                                            left join 
                                            (
                                              select count(1) as iValue, to_char(createdate,'MM') as iMonth from v_basehiddeninfo  where  workstream in ( '隐患验收','复查验证','整改效果评估','整改结束') and rankname = '重大隐患'  and 
                                               {0} group by  to_char(createdate,'MM')
                                            ) e on a.month = e.iMonth 
                                            left join 
                                            (
                                              select count(1) as aoValue, to_char(createdate,'MM') as aoMonth  from v_basehiddeninfo  where  workstream in ( '隐患整改','隐患验收','复查验证','整改效果评估','整改结束') and rankname = '一般隐患'  and 
                                               {0} group by  to_char(createdate,'MM')
                                            ) f on a.month = f.aoMonth 
                                            left join 
                                            (
                                              select count(1) as oValue, to_char(createdate,'MM') as oMonth from v_basehiddeninfo  where  workstream in ( '隐患验收','复查验证','整改效果评估','整改结束') and rankname = '一般隐患'  and 
                                               {0} group by  to_char(createdate,'MM')
                                            ) g on a.month = g.oMonth 
                                             order by a.month ", str);
                    #endregion
                    break;
                /****隐患整改情况对比图****/
                case "8":
                    #region 隐患整改情况对比图
                    sql = string.Format(@"select a.encode as changedutydepartcode,a.fullname, nvl(b.nonChange,0) as  nonChange, nvl(b.thenChange,0) as thenChange,nvl(b.total,0) as total ,a.sortcode  
                                             from (select encode ,fullname,sortcode from base_department  where {0}) a
                                            left join (   
                                                  select b.encode changedutydepartcode,sum(nvl(a.nonChange,0)) nonChange,sum(nvl(a.thenChange,0)) thenChange ,sum(nvl(a.nonChange,0) + nvl(a.thenChange,0)) as total,b.fullname  from (
                                                         select * from (
                                                            select  count(1) as pValue, changedutydepartcode,'已整改' changestatus  from v_basehiddeninfo  where  workstream in ('隐患验收','复查验证','整改效果评估','整改结束')  and 
                                                            {1}  group by changedutydepartcode
                                                            union 
                                                            select  count(1) as pValue, changedutydepartcode,'未整改' changestatus  from v_basehiddeninfo  where  workstream in ('隐患整改')  and 
                                                            {1}  group by  changedutydepartcode
                                                        ) pivot (sum(pValue) for changestatus in ('未整改' as nonChange,'已整改' as thenChange))
                                                    ) a 
                                                    left join (select encode ,fullname,sortcode from base_department  where {0}) b on 
                                                    substr(a.changedutydepartcode,0,length(b.encode)) = b.encode  group by  b.encode,b.fullname
                                              ) b on a.encode = b.changedutydepartcode  order by a.sortcode  ", tempstr, str);
                    #endregion
                    break;
                /****隐患整改情况区域对比图****/
                case "10":
                    #region 隐患整改情况区域对比图
                    sql = string.Format(@"  select a.districtcode  hidpoint,a.districtname  hidpointname,nvl(b.nonChange,0) as nonChange,nvl(b.thenChange,0) as thenChange,nvl(b.total,0) as total,
                                              a.organizeid from (
                                                  select districtcode ,districtname ,sortcode ,organizeid  from bis_district  a  where {1}
                                                ) a
                                              left join (                                            
                                                           select a.districtcode hidpoint, sum(nvl(b.nonChange,0)) nonChange, sum(nvl(b.thenChange,0)) thenChange , sum(nvl(b.nonChange,0) + nvl(b.thenChange,0)) as total from
                                                          （
                                                             select districtcode ,districtname ,sortcode ,organizeid  from bis_district a  where {1}
                                                           ） a left join 
                                                           (
                                                              select * from
                                                               (
                                                                    select  count(1) as pValue,  hidpoint,'已整改' changestatus  from v_basehiddeninfo  where  workstream in ('隐患验收','复查验证','整改效果评估','整改结束')  and 
                                                                    {0}  group by  hidpoint
                                                                    union 
                                                                    select  count(1) as pValue,  hidpoint,'未整改' changestatus  from v_basehiddeninfo  where  workstream in ('隐患整改')  and 
                                                                    {0}  group by  hidpoint
                                                               ) pivot (sum(pValue) for changestatus in ('未整改' as nonChange,'已整改' as thenChange))
                                                           ) b    on  a.districtcode =  substr(b.hidpoint,0, length(a.districtcode))  group by a.districtcode
                                             )  b on  a.districtcode =  b.hidpoint order by a.sortcode ", str, areastr);
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

        #endregion

        #region 双控工作列表
        /// <summary>
        /// 双控工作
        /// </summary>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataTable QueryHidWorkList(Operator curUser)
        {
            string deptcode = string.Empty;

            if (curUser.RoleName.Contains("超级管理员"))
            {
                deptcode = curUser.DeptCode;
            }
            else if (curUser.RoleName.Contains("省级用户") || curUser.RoleName.Contains("厂级") || curUser.RoleName.Contains("公司级"))
            {
                deptcode = Idepartmentservice.GetEntity(curUser.OrganizeId).DeptCode;  //部门用户
            }
            else
            {
                deptcode = Idepartmentservice.GetEntity(curUser.DeptId).DeptCode; //部门用户
            }

            string sql = string.Empty;

            sql = string.Format(@" select '0' as itemdetailid, '全部隐患' as itemvalue ,  sum(total) as total  ,sum(yzgsl) as yzgsl ,sum(wzgsl) as wzgsl ,sum(yqwzgsl) as yqwzgsl,
                                         (case when  sum(total) =0 then 0 else  round(sum(yzgsl) *100 / sum(total),2)  end) as yhzgl  from (
                                          select a.itemdetailid, a.itemvalue, nvl(b.pnum,0) as total , nvl(c.pnum,0) as yzgsl, nvl(d.pnum,0) as wzgsl , nvl(e.pnum,0) as yqwzgsl  from 
                                          (  select a.itemdetailid ,itemvalue from base_dataitemdetail a
                                          left join base_dataitem b on a.itemid = b.itemid
                                          where b.itemcode ='HidRank') a
                                          left join (
                                             select  count(1) as pnum ,hidrank from v_basechangeinfo where   ( deptcode  like '{0}%' or changedutydepartcode in 
                                             (select a.encode from base_department a left join base_department b on a.senddeptid = b.departmentid 
                                             where  a.senddeptid is not null  and a.deptcode like '{0}%'  ) ) and to_char(createdate,'yyyy') ='{1}'
                                             group by hidrank
                                          ) b on a.itemdetailid =b.hidrank
                                          left join (
                                             select  count(1) as pnum , hidrank from v_basechangeinfo where  ( deptcode  like '{0}%' or changedutydepartcode in 
                                             (select a.encode from base_department a left join base_department b on a.senddeptid = b.departmentid 
                                             where   a.senddeptid is not null  and a.deptcode like '{0}%'  ) ) and to_char(createdate,'yyyy') ='{1}'
                                             and  workstream in ('隐患验收','复查验证','整改效果评估','整改结束')  group by hidrank
                                          ) c on a.itemdetailid =c.hidrank
                                          left join (
                                             select  count(1) as pnum ,hidrank from v_basechangeinfo where  ( deptcode  like '{0}%' or changedutydepartcode in 
                                             (select a.encode from base_department a left join base_department b on a.senddeptid = b.departmentid 
                                             where  a.senddeptid is not null  and a.deptcode like '{0}%'  ) ) and to_char(createdate,'yyyy') ='{1}'
                                             and  workstream = '隐患整改'  group by hidrank
                                          ) d on a.itemdetailid =d.hidrank
                                          left join (
                                              select  count(1) as pnum , hidrank from v_basechangeinfo where  ( deptcode  like '{0}%' or changedutydepartcode in 
                                             (select a.encode from base_department a left join base_department b on a.senddeptid = b.departmentid 
                                             where  a.senddeptid is not null  and a.deptcode like '{0}%'  ) ) and to_char(createdate,'yyyy') ='{1}'
                                              and  workstream = '隐患整改'  and  sysdate > changedeadine + 1  group by hidrank
                                          ) e on a.itemdetailid =e.hidrank
                                      ) a 
                                      union 
                                       select a.itemdetailid, a.itemvalue, nvl(b.pnum,0) as total , nvl(c.pnum,0) as yzgsl, nvl(d.pnum,0) as wzgsl , nvl(e.pnum,0) as yqwzgsl,
                                      (case when  nvl(b.pnum,0) =0 then 0 else  round(nvl(c.pnum,0)*100  / nvl(b.pnum,0),2)  end) as yhzgl   from (  select a.itemdetailid ,itemvalue from base_dataitemdetail a
                                      left join base_dataitem b on a.itemid = b.itemid
                                      where b.itemcode ='HidRank') a
                                      left join (
                                         select  count(1) as pnum ,hidrank from v_basechangeinfo where  ( deptcode  like '{0}%' or changedutydepartcode in 
                                             (select a.encode from base_department a left join base_department b on a.senddeptid = b.departmentid 
                                             where  a.senddeptid is not null  and a.deptcode like '{0}%'  ) ) and to_char(createdate,'yyyy') ='{1}'
                                         group by hidrank
                                      ) b on a.itemdetailid =b.hidrank
                                      left join (
                                         select  count(1) as pnum , hidrank from v_basechangeinfo where  ( deptcode  like '{0}%' or changedutydepartcode in 
                                             (select a.encode from base_department a left join base_department b on a.senddeptid = b.departmentid 
                                             where  a.senddeptid is not null  and a.deptcode like '{0}%'  ) ) and to_char(createdate,'yyyy') ='{1}'
                                         and  workstream in ('隐患验收','复查验证','整改效果评估','整改结束')  group by hidrank
                                      ) c on a.itemdetailid =c.hidrank
                                      left join (
                                         select  count(1) as pnum ,hidrank from v_basechangeinfo where  ( deptcode  like '{0}%' or changedutydepartcode in 
                                             (select a.encode from base_department a left join base_department b on a.senddeptid = b.departmentid 
                                             where  a.senddeptid is not null  and a.deptcode like '{0}%'  ) ) and to_char(createdate,'yyyy') ='{1}'
                                         and  workstream = '隐患整改'  group by hidrank
                                      ) d on a.itemdetailid =d.hidrank
                                      left join (
                                          select  count(1) as pnum , hidrank from v_basechangeinfo where  ( deptcode  like '{0}%' or changedutydepartcode in 
                                             (select a.encode from base_department a left join base_department b on a.senddeptid = b.departmentid 
                                             where  a.senddeptid is not null  and a.deptcode like '{0}%'  ) ) and to_char(createdate,'yyyy') ='{1}'
                                          and  workstream = '隐患整改'  and  sysdate > changedeadine + 1  group by hidrank
                                      ) e on a.itemdetailid =e.hidrank", deptcode, DateTime.Now.Year.ToString());
               

            var dt = this.BaseRepository().FindTable(sql);

            return dt;
        }
        #endregion

        #region 待办记录
        /// <summary>
        /// 待办记录
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable QueryHidBacklogRecord(string value, string userId)
        {
            UserEntity curUser = new UserService().GetEntity(userId); //当前用户

            string sql = "";

            string str = ""; //查询条件

            //个人待办业务
            if (value == "0")
            {
                //待自己处理的业务
                string actionperson = "," + curUser.Account + ",";

                sql = string.Format(@" select a.* from (
                                 select count(1) as pnum , 1 as serialnumber from v_hiddenbasedata a
                                                    where a.actionperson  like  '%{0}%'  and a.workstream ='隐患评估' 
                                   union
                                    select count(1) as pnum,2 as serialnumber from v_hiddenbasedata a
                                                     left join v_htchangeinfo b on a.hidcode = b.hidcode
                                                     where b.changeperson  = '{1}' and a.workstream ='隐患整改' 
                                   union 
                                   select count(1) as pnum,3 as serialnumber from v_basehiddeninfo a
                                               where applicationstatus ='1' and postponeperson like '%{0}%' 
                                   union
                                      select count(1) as pnum,4 as serialnumber from v_hiddenbasedata a
                                                     where a.actionperson  like '%{0}%' and workstream = '隐患验收' 
                                    union
                                            select count(1) as pnum,5 as serialnumber from v_hiddenbasedata a
                                                     where  a.actionperson  like '%{0}%' and workstream = '整改效果评估' 
       union
                                            select count(1) as pnum,6 as serialnumber from v_hiddenbasedata a
                                                     where   a.actionperson  like '%{0}%' and workstream = '隐患完善' 
       union
                                            select count(1) as pnum,7 as serialnumber from v_hiddenbasedata a
                                                     where  a.actionperson  like '%{0}%' and workstream = '复查验证'
       union
                                            select count(1) as pnum,8 as serialnumber from v_hiddenbasedata a
                                                     where  a.actionperson  like '%{0}%' and workstream = '制定整改计划'  
                                                     ) a  order by serialnumber", actionperson, curUser.UserId, str);
            }
            //个人待办,逾期未整改,我上传的隐患
            else if (value == "2")
            {
                if (curUser.RoleName.Contains("公司级用户") || curUser.RoleName.Contains("厂级部门用户"))
                {
                    sql = string.Format(@" select a.* from ( select count(1) as pnum , 1 as serialnumber from v_htbaseinfo a
                                          where 1=1 and a.createuserorgcode like '{0}%' 
                                        union
                                         select count(1) as pnum , 2 as serialnumber from v_htbaseinfo a
                                        left join v_htchangeinfo b on a.hidcode = b.hidcode
                                          where 1=1 and a.createuserorgcode like '{0}%' and  workstream = '隐患整改'  and  sysdate > changedeadine + 1  ) a order by serialnumber", curUser.OrganizeCode);
                }
                else
                {
                    sql = string.Format(@" select a.* from ( select count(1) as pnum , 1 as serialnumber from v_htbaseinfo a
                                          where 1=1 and a.createuserorgcode like '{0}%' 
                                        union
                                         select count(1) as pnum , 2 as serialnumber from v_htbaseinfo a
                                        left join v_htchangeinfo b on a.hidcode = b.hidcode
                                          where 1=1 and a.createuserdeptcode like '{0}%' and  workstream = '隐患整改'  and  sysdate > changedeadine + 1) a order by serialnumber", curUser.DepartmentCode);
                }
            }
            else if (value == "10")
            {
                //if (curUser.RoleName.Contains("公司级用户") || curUser.RoleName.Contains("厂级部门用户"))
                //{
                //    sql = string.Format(@" select a.* from ( select count(1) as pnum , 1 as serialnumber from v_htbaseinfo a
                //                          where  createuserid ='{1}'
                //                        union
                //                         select count(1) as pnum , 2 as serialnumber from v_htbaseinfo a
                //                        left join v_htchangeinfo b on a.hidcode = b.hidcode
                //                          where  CHANGEDUTYDEPARTCODE like '{0}%' and  workstream = '隐患整改'  and  sysdate > changedeadine + 1  
                //                         ) a order by serialnumber", curUser.OrganizeCode, curUser.UserId);
                //}
                //else
                //{
                //    sql = string.Format(@" select a.* from ( select count(1) as pnum , 1 as serialnumber from v_htbaseinfo a
                //                          where  createuserid ='{1}'
                //                        union
                //                         select count(1) as pnum , 2 as serialnumber from v_htbaseinfo a
                //                        left join v_htchangeinfo b on a.hidcode = b.hidcode
                //                          where  CHANGEDUTYDEPARTCODE like '{0}%' and  workstream = '隐患整改'  and  sysdate > changedeadine + 1  
                //                         ) a order by serialnumber", curUser.DepartmentCode, curUser.UserId);
                //}
                sql = string.Format(@" select a.* from ( select count(1) as pnum , 1 as serialnumber from v_htbaseinfo a
                                          where  createuserid ='{0}'
                                        union
                                         select count(1) as pnum , 2 as serialnumber from v_htbaseinfo a
                                        left join v_htchangeinfo b on a.hidcode = b.hidcode
                                          where  a.hiddepart = '{1}' and  workstream = '隐患整改'  and   to_date('{2}','yyyy-mm-dd hh24:mi:ss')  > changedeadine + 1  
                                         ) a order by serialnumber", curUser.UserId, curUser.OrganizeId, DateTime.Now);

            }
            else   //全部待办业务
            {
                sql = string.Format(@" select a.* from (
                                          select count(1) as pnum , 1 as serialnumber from v_htbaseinfo a
                                                        left join v_workflow b on a.id = b.id 
                                                        where 1=1 and a.createuserorgcode like '{0}%' and  a.hidcode in  (
                                                        select distinct hidcode from v_approvaldata a
                                                         where departmentcode = '{1}' and name ='隐患评估')  and a.workstream ='隐患评估'
                                                         union
                                          select count(1) as pnum , 2 as serialnumber from v_htbaseinfo a
                                                         left join v_htchangeinfo b on a.hidcode = b.hidcode
                                                         where 1=1 and a.createuserorgcode like '{0}%' and changedutydepartcode  =  '{1}' and a.workstream ='隐患整改'
                                                         union
                                          select count(1) as pnum , 3 as serialnumber from v_htbaseinfo a
                                                         left join v_htchangeinfo b on a.hidcode = b.hidcode
                                                         where 1=1 and a.createuserorgcode like '{0}%'  and ((a.applicationstatus ='2' and a.postponedept  like  '%,{1},%')
                                                         or  (a.applicationstatus ='1' and changedutydepartcode  = '{1}'))
                                                         union
                                          select count(1) as pnum , 4 as serialnumber from v_htbaseinfo a
                                                         left join v_htacceptinfo b on a.hidcode = b.hidcode
                                                         where 1=1 and a.createuserorgcode like '{0}%' and acceptdepartcode = '{1}' and workstream = '隐患验收' 
                                                         union
                                           select count(1) as pnum , 5 as serialnumber from v_htbaseinfo a
                                                         left join v_htacceptinfo b on a.hidcode = b.hidcode
                                                         where 1=1 and a.createuserorgcode like '{0}%' and acceptdepartcode = '{1}' and workstream = '整改效果评估' 
                  union
                                           select count(1) as pnum , 5 as serialnumber from v_htbaseinfo a
                                                          left join v_htchangeinfo b on a.hidcode = b.hidcode
                                                         where 1=1 and a.createuserorgcode like '{0}%' and changedutydepartcode = '{1}' and workstream = '制定整改计划' 
                                      ) a  order by serialnumber ", curUser.OrganizeCode, curUser.DepartmentCode);
            }

            var dt = this.BaseRepository().FindTable(sql);

            return dt;
        }
        #endregion

        #region  获取前几个曝光的数据(取当前年的)
        /// <summary>
        /// 获取前几个曝光的数据(取当前年的)
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public DataTable QueryExposureHid(string num)
        {
            DataItemDetailService dataitemdetailservice = new DataItemDetailService();
            string sql = string.Empty;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.RoleName.Contains("省级"))
            {
                sql = string.Format(@" select a.* from (
                                        select distinct a.id , a.id  hiddenid,a.hiddepart ,a.hidcode problemid,a.hiddescribe,a.createdate,a.workstream,a.addtype,
                                    a.hidbasefilepath , (case when a.hidbasefilepath is not 
                                                null then ('{3}'||substr(a.hidbasefilepath,2)) else '' end) filepath， a.createuserorgcode from v_basehiddeninfo  a where a.exposurestate ='是' and 
                                        a.deptcode like '{1}%' and to_char(a.createdate,'yyyy') ='{2}' ) a where rownum <= {0} order by createdate  ", int.Parse(num), user.NewDeptCode, DateTime.Now.Year.ToString(), dataitemdetailservice.GetItemValue("imgUrl"));
            }
            else
            {
                sql = string.Format(@" select a.* from (
                                        select distinct a.id , a.id  hiddenid,a.hiddepart ,a.hidcode problemid,a.hiddescribe,a.createdate,a.workstream,a.addtype,
                                    a.hidbasefilepath  , (case when a.hidbasefilepath is not 
                                                null then ('{3}'||substr(a.hidbasefilepath,2)) else '' end)  filepath，a.createuserorgcode from v_basehiddeninfo  a where a.exposurestate ='是'  and 
                                       a.hiddepart ='{1}' and to_char(a.createdate,'yyyy') ='{2}' ) a where rownum <= {0} order by createdate  ", int.Parse(num), user.OrganizeId, DateTime.Now.Year.ToString(), dataitemdetailservice.GetItemValue("imgUrl"));
            }
            var dt = this.BaseRepository().FindTable(sql);
            return dt;
        }
        #endregion

        #region 手机端分页
        /// <summary>
        /// 手机端分页
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

        #region 获取手机端隐患统计内容(用于手机移动端统计/班组(五型班组)终端)
        /// <summary>
        /// 获取手机端隐患统计内容
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable GetAppHidStatistics(string code, int mode, int category)
        {
            string sql = "";
            var from = DateTime.MinValue;
            var to = DateTime.MinValue;

            switch (category)
            {
                case 0:
                    from = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    to = from.AddMonths(1);
                    break;
                case 1:
                    from = new DateTime(DateTime.Today.Year, ((int)Math.Ceiling(((float)DateTime.Today.Month / 3)) - 1) * 3 + 1, 1);
                    to = from.AddMonths(3);
                    break;
                case 2:
                    from = new DateTime(DateTime.Today.Year, 1, 1);
                    to = from.AddYears(1);
                    break;
                default:
                    break;
            }


            string newcode = string.Empty;

            //传值为空，则传入机构新代码
            if (string.IsNullOrEmpty(code))
            {
                Operator user = OperatorProvider.Provider.Current();
                newcode = user.NewDeptCode;
            }
            else  //目标部门新代码
            {
                newcode = Idepartmentservice.GetEntityByCode(code).DeptCode; //取新编码
            }

            #region 整改隐患数量
            if (mode == 0)
            {
                sql = string.Format(@"  select nvl(allhid,0) as allhid ，nvl(ordinaryhid,0) as ordinaryhid,nvl(importanhid ,0) as importanhid   from (
                                 select  count(1) as pnum ,rankname from v_basehiddeninfo where changedeptcode  like '{0}%' and to_char(createdate,'yyyy') ='{1}'    group by rankname 
                                 union
                                 select  count(1) as pnum ,'全部隐患' as rankname from v_basehiddeninfo where changedeptcode  like '{0}%' and to_char(createdate,'yyyy') ='{1}'
                           ) pivot (sum(pnum) for rankname in ('一般隐患' as OrdinaryHid,'重大隐患' as ImportanHid,'全部隐患' as AllHid))", newcode, DateTime.Now.Year.ToString());
            }
            #endregion
            #region 隐患整改情况
            else if (mode == 1)
            {
                sql = string.Format(@"select  nvl(b.yzgnum,0) as  yzgnum , nvl(c.wzgnum,0) as  wzgnum, (case when nvl(a.allnum,0) =0 then 0 else round(b.yzgnum * 100 / a.allnum,2) end)  as allzgl,
                                              nvl(e.zdyzgnum,0) as zdyzgnum , nvl(f.zdwzgnum,0) as zdwzgnum, (case when nvl(d.zdallnum,0) =0 then 0 else round(e.zdyzgnum * 100 / d.zdallnum,2) end)  as  zdzgl,
                                               nvl(h.ybyzgnum,0) as ybyzgnum , nvl(i.ybwzgnum,0) as ybwzgnum, (case when nvl(g.yballnum,0) =0 then 0 else round(h.ybyzgnum * 100 / g.yballnum,2) end)  as  ybzgl
                                        from (                                        
                                        select count(1) as allnum ,to_char(createdate,'yyyy') as createdate from v_basehiddeninfo  where  workstream 
                                        in ( '隐患整改','隐患验收','复查验证','整改效果评估','整改结束')  and 
                                         changedeptcode  like '{0}%' and to_char(createdate,'yyyy') ='{1}' group by to_char(createdate,'yyyy')
                                         )  a 
                                         left join (
                                       select count(1) as yzgnum,to_char(createdate,'yyyy') as createdate  from v_basehiddeninfo  where  
                                       workstream in ('隐患验收','复查验证','整改效果评估','整改结束')   and 
                                       changedeptcode  like '{0}%' and to_char(createdate,'yyyy') ='{1}'  group by to_char(createdate,'yyyy') 
                                        )  b on a.createdate = b.createdate
                                        left join (
                                          select count(1) as wzgnum,to_char(createdate,'yyyy') as createdate  from v_basehiddeninfo  where  
                                       workstream in ('隐患整改')   and 
                                       changedeptcode  like '{0}%' and to_char(createdate,'yyyy') ='{1}'  group by to_char(createdate,'yyyy')
                                       ) c on a.createdate = c.createdate     
                                       left join (
                                        select count(1) as  zdallnum ,to_char(createdate,'yyyy') as createdate from v_basehiddeninfo  where  workstream 
                                        in ( '隐患整改','隐患验收','复查验证','整改效果评估','整改结束') and rankname in ('重大隐患')  and 
                                         changedeptcode  like '{0}%' and to_char(createdate,'yyyy') ='{1}' group by to_char(createdate,'yyyy')
                                       )  d on a.createdate = d.createdate  
                                       left join (
                                       select count(1) as  zdyzgnum,to_char(createdate,'yyyy') as createdate  from v_basehiddeninfo  where  
                                       workstream in ('隐患验收','复查验证','整改效果评估','整改结束') and rankname in ('重大隐患')   and 
                                       changedeptcode  like '{0}%' and to_char(createdate,'yyyy') ='{1}'  group by to_char(createdate,'yyyy')
                                       )  e on a.createdate = e.createdate 
                                       left join (
                                          select count(1) as zdwzgnum,to_char(createdate,'yyyy') as createdate  from v_basehiddeninfo  where  
                                       workstream in ('隐患整改') and rankname in ('重大隐患')   and 
                                       changedeptcode  like '{0}%' and to_char(createdate,'yyyy') ='{1}'  group by to_char(createdate,'yyyy')
                                       ) f on a.createdate = f.createdate 
                                       left join (                                    
                                        select count(1) as yballnum ,to_char(createdate,'yyyy') as createdate from v_basehiddeninfo  where  workstream 
                                        in ( '隐患整改','隐患验收','复查验证','整改效果评估','整改结束') and rankname in ('一般隐患')  and 
                                         changedeptcode  like '{0}%' and to_char(createdate,'yyyy') ='{1}' group by to_char(createdate,'yyyy')
                                       ) g on a.createdate = g.createdate
                                       left join ( 
                                       select count(1) as ybyzgnum,to_char(createdate,'yyyy') as createdate  from v_basehiddeninfo  where  
                                       workstream in ('隐患验收','复查验证','整改效果评估','整改结束') and rankname in ('一般隐患')   and 
                                       changedeptcode  like '{0}%' and to_char(createdate,'yyyy') ='{1}'  group by to_char(createdate,'yyyy')
                                       ) h on a.createdate = h.createdate 
                                       left join (
                                          select count(1) as ybwzgnum,to_char(createdate,'yyyy') as createdate  from v_basehiddeninfo  where  
                                       workstream in ('隐患整改') and rankname in ('一般隐患')   and 
                                       changedeptcode  like '{0}%' and to_char(createdate,'yyyy') ='{1}'  group by to_char(createdate,'yyyy') 
                                       ) i on a.createdate = i.createdate  ", newcode, DateTime.Now.Year.ToString());
            }
            #endregion
            #region 安全检查下的隐患
            else if (mode == 3)
            {
                sql = string.Format(@" select a.itemname, nvl(b.allnum,0) as pnun from (
                                          select a.itemdetailid,a.itemname,a.itemvalue from  base_dataitemdetail  a
                                          left join  base_dataitem b on a.itemid = b.itemid 
                                          where b.itemcode = 'SaftyCheckType'
                                          ) a
                                          left join (
                                            select count(1) as allnum ,checktype from v_basehiddeninfo  where  workstream 
                                            in ( '隐患整改','隐患验收','复查验证','整改效果评估','整改结束')   and safetycheckobjectid is not null and
                                             changedeptcode  like '{0}%' and to_char(createdate,'yyyy') ='{1}'  group by checktype
                                           ) b on a.itemdetailid = b.checktype  order by a.itemvalue", newcode, DateTime.Now.Year.ToString());

            }
            #endregion
            #region 登记/整改的隐患数量/违章次数(按登记单位)/应急演练次数(按登记单位来)
            else if (mode == 4)
            {
                sql = string.Format(@"select  count(1)  pnum from v_basehiddeninfo where deptcode  like '{0}%' and to_char(createdate,'yyyy') ='{1}'
                                      union  all
                                      select  count(1)  pnum from v_basehiddeninfo where changedeptcode  like '{0}%' and to_char(createdate,'yyyy') = '{1}'
                                      union  all 
                                      select  count(1) pnum from v_lllegalbaseinfo where createuserdeptcode  like '{2}%' and to_char(createdate,'yyyy') ='{1}'
                                      union  all 
                                      select count(1) pnum from mae_drillplanrecord where createuserdeptcode  like '{2}%' and createdate >= to_date('{3}','yyyy/mm/dd') and createdate < to_date('{4}','yyyy/mm/dd')
                                      ", newcode, DateTime.Now.Year.ToString(), code, from.ToString("yyyy/MM/dd"), to.ToString("yyyy/MM/dd"));
            }
            #endregion
            #region 未整改隐患、未整改违章数量数量及其他指标内容
            else if (mode == 5)
            {
                sql = string.Format(@"select  count(1)  pnum from v_basehiddeninfo where workstream = '隐患整改' and changedutydepartcode  like '{0}%'   
                                      union  all 
                                      select  count(1)  pnum from v_lllegalbaseinfo where flowstate = '违章整改' and  reformdeptcode  like '{0}%'
                                      union  all 
                                      select  count(1)  pnum from mae_drillplanrecord where createuserdeptcode  like '{0}%' and to_char(createdate,'yyyy-MM') ='{1}'  
                                      union  all 
                                      select  count(1)  pnum from v_basehiddeninfo where createuserdeptcode  like '{0}%' and to_char(createdate,'yyyy-MM') ='{1}'
                                      union  all 
                                      select  count(1)  pnum from v_lllegalbaseinfo where createuserdeptcode  like '{0}%' and to_char(createdate,'yyyy-MM') ='{1}'
                                      union  all 
                                      select  count(1) from bis_saftycheckdatarecord t where ((t.createuserdeptcode like '{0}%' and datatype=0) or (belongdept like  '{0}%' and datatype=2)) and  to_char(createdate,'yyyy-MM') ='{1}' 
                                      union all 
                                      select  count(1)  pnum from v_basehiddeninfo where rankname ='一般隐患' and  createuserdeptcode  like '{0}%' and to_char(createdate,'yyyy-MM') ='{1}'
                                      union all 
                                      select  count(1)  pnum from v_basehiddeninfo where rankname ='重大隐患' and  createuserdeptcode  like '{0}%' and to_char(createdate,'yyyy-MM') ='{1}'
                                      union all 
                                      select (case when b.pnum = 0 then 0 else  round(a.pnum/b.pnum *100,2) end) pnum  from (select  count(1)  pnum  from v_basehiddeninfo where createuserdeptcode  like '{0}%' and  workstream!='隐患整改' and to_char(createdate,'yyyy-MM') ='{1}' ) a, (select  count(1)  pnum from v_basehiddeninfo where createuserdeptcode  like '{0}%' and to_char(createdate,'yyyy-MM') ='{1}') b 
                                      union all 
                                      select (case when b.pnum = 0 then 0 else  round(a.pnum/b.pnum *100,2) end) pnum  from (select  count(1)  pnum  from v_basehiddeninfo where createuserdeptcode  like '{0}%' and rankname ='一般隐患' and  workstream!='隐患整改' and to_char(createdate,'yyyy-MM') ='{1}' ) a, (select  count(1)  pnum from v_basehiddeninfo where createuserdeptcode  like '{0}%' and rankname ='一般隐患' and to_char(createdate,'yyyy-MM') ='{1}') b 
                                      union all 
                                      select (case when b.pnum = 0 then 0 else  round(a.pnum/b.pnum *100,2) end) pnum  from (select  count(1)  pnum  from v_basehiddeninfo where createuserdeptcode  like '{0}%' and rankname ='重大隐患' and workstream!='隐患整改' and to_char(createdate,'yyyy-MM') ='{1}' ) a, (select  count(1)  pnum from v_basehiddeninfo where createuserdeptcode  like '{0}%' and  rankname ='重大隐患' and to_char(createdate,'yyyy-MM') ='{1}') b 
                                      union all
                                      select (case when b.pnum = 0 then 0 else  round(a.pnum/b.pnum *100,2) end) pnum  from (select  count(1)  pnum  from v_lllegalbaseinfo where createuserdeptcode  like '{0}%' and flowstate!='违章整改' and to_char(createdate,'yyyy-MM') ='{1}' ) a, (select  count(1)  pnum from v_lllegalbaseinfo where createuserdeptcode  like '{0}%' and to_char(createdate,'yyyy-MM') ='{1}') b 
                                      union all
                                      select nvl(sum(num),0) pnum from (select count(distinct(a.taskuserid)) as num from bis_staffinfo a  where a.pteamcode like '{0}%' and a.tasklevel='2' and a.supervisestate='1' and to_char(createdate,'yyyy-mm')='{1}' group by  to_char(createdate,'yyyy-mm-dd'))
                                      union all 
                                      select (case when b.pnum = 0 then 0 else  round(a.pnum/b.pnum *100,2) end) pnum  from (select  count(1)  pnum  from v_basehiddeninfo where changedutydepartcode  like '{0}%' and  workstream!='隐患整改' and to_char(createdate,'yyyy-MM') ='{1}' ) a, (select  count(1)  pnum from v_basehiddeninfo where changedutydepartcode  like '{0}%' and to_char(createdate,'yyyy-MM') ='{1}') b 
                                      union all
                                      select (case when b.pnum = 0 then 0 else  round(a.pnum/b.pnum *100,2) end) pnum  from (select  count(1)  pnum  from v_lllegalbaseinfo where reformdeptcode  like '{0}%' and flowstate!='违章整改' and to_char(createdate,'yyyy-MM') ='{1}' ) a, (select  count(1)  pnum from v_lllegalbaseinfo where reformdeptcode  like '{0}%' and to_char(createdate,'yyyy-MM') ='{1}') b
                                      union all 
                                      select  count(1)  pnum from v_basehiddeninfo where workstream = '隐患整改' and changedutydepartcode  like '{0}%' and to_char(createdate,'yyyy-MM') ='{1}'
                                      union all 
                                      select  count(1)  pnum from v_basehiddeninfo where workstream != '隐患整改' and  changedutydepartcode  like '{0}%' and to_char(createdate,'yyyy-MM') ='{1}' 
                                      union  all 
                                      select  count(1)  pnum from v_lllegalbaseinfo where flowstate = '违章整改' and  reformdeptcode  like '{0}%' and to_char(createdate,'yyyy-MM') ='{1}' 
                                      union  all 
                                      select  count(1)  pnum from v_lllegalbaseinfo where  flowstate != '违章整改' and reformdeptcode  like '{0}%' and to_char(createdate,'yyyy-MM') ='{1}'", code, DateTime.Now.ToString("yyyy-MM"));
            }
            #endregion
            //隐患未整改
            else if (mode == 6 || mode == 7)
            {
                sql = string.Format(@" select account,modifydate,createuserdeptcode,createuserorgcode,createuserid,checktypename,hidtypename,hidrankname,checkdepartname,
                                    createdate,problemid,checkdate,checktype,checknumber,isgetafter,relevanceid,relevancetype,
                                    isbreakrule ,hidtype, hidrank,hidplace,hidpoint,workstream,addtype,participant,applicationstatus,postponedept,
                                    postponedeptname,postponeperson,postponepersonname,hiddescribe,changemeasure,filepath,changedutydepartcode,hiddepartname,
                                    recheckdepartname,recheckpersonname,deptcode,checkdepart,actionpersonname,actionstatus,to_char(changedeadine,'yyyy-MM-dd') changedeadine from  (
                                                  select a.DeviceId,a.account,a.modifydate,a.createuserdeptcode,a.createuserorgcode,a.createuserid,
                                                  a.checktypename,a.hidtypename,a.hidrankname,a.checkdepartname,a.isgetafter,a.id as hiddenid ,to_char(a.createdate,'yyyy-MM-dd') createdate,
                                                  a.hidcode as problemid ,to_char(a.checkdate,'yyyy-MM-dd') checkdate,a.checkdepart,a.checkdepartid,a.checktype,a.checknumber,a.relevanceid,a.relevancetype,  a.isbreakrule,a.hidrank,a.hidplace,a.hidpoint,a.workstream ,a.addtype,b.participant,c.applicationstatus ,b.actionperson,b.actionpersonname,a.hidtype,c.changedutydepartcode,
                                                  c.changeperson,a.exposurestate,c.postponedept,c.postponedeptname ,c.postponeperson,c.postponepersonname, a.hiddescribe,
                                                  c.changemeasure,c.changedeadine,a.safetycheckobjectid,d.acceptdepartcode,d.acceptperson, f.filepath,
                                                  m.recheckperson,m.recheckpersonname,m.recheckdepartcode,m.recheckdepartname,a.hiddepart,a.hiddepartname,a.deptcode,
                                                 ( case when  a.workstream ='隐患登记' then '隐患登记' when a.workstream ='隐患评估' then  '评估中' when  a.workstream ='隐患完善' then '完善中' when
                                                     a.workstream ='隐患整改' then '整改中' when  a.workstream ='制定整改计划' then '制定整改计划中'  when  a.workstream ='隐患验收' then '验收中' when  a.workstream ='复查验证' then '复查中' when  a.workstream ='整改效果评估' then '效果评估中' when
                                                      a.workstream ='整改结束' then '整改结束' end ) actionstatus,a.safetycheckname,c.chargeperson,c.chargepersonname,c.chargedeptid,c.chargedeptname,c.isappoint ,a.rolename,b.participantname,a.hidphoto
                                                  from v_htbaseinfo a
                                                  left join (
                                                   select a.id,a.participant,a.actionperson,a.participantname,a.participantname actionpersonname from v_workflow a
                                                  ) b on a.id = b.id 
                                                  left join v_htchangeinfo c on a.hidcode = c.hidcode
                                                  left join v_htacceptinfo d on a.hidcode = d.hidcode
                                                  left join v_htrecheck m  on a.hidcode = m.hidcode 
                                                  left join  (
                                                    select a.id,wm_concat((case when b.filepath is not null then ('{0}'||substr(b.filepath,2)) else '' end)) filepath from bis_htbaseinfo a
                                                    left join base_fileinfo b on a.hidphoto = b.recid  group by a. id
                                                  ) f on  a.id = f.id 
                                         ) a  where workstream = '隐患整改' and changedutydepartcode  like '{1}%' ", idataitemdetailservice.GetItemValue("imgUrl"), code);
                if (mode == 7) //逾期部分
                {
                    sql += string.Format(@"   and  to_date('{0}','yyyy-mm-dd hh24:mi:ss')> (changedeadine+1) ", DateTime.Now);
                }
            }
            //违章未整改
            else if (mode == 8 || mode == 9)
            {
                sql = string.Format(@" select belongdepartid,belongdepart,createuserdeptcode,createuserorgcode,createuserid, createdate,lllegalnumber,lllegaltype, lllegaltypename ,lllegaltime,lllegallevel,
                                    lllegallevelname, lllegalperson,lllegalpersonid,lllegalteam,lllegalteamcode,lllegaldepart,lllegaldepartcode,lllegaldescribe,lllegaladdress ,lllegalpic,
                                    reformrequire,flowstate,createusername ,addtype,isexposure,reformpeople,reformpeopleid,reformtel,reformdeptcode,reformdeptname, to_char(a.reformdeadline,'yyyy-MM-dd') reformdeadline,reformfinishdate,reformstatus,reformmeasure,isgrpaccept,acceptpeopleid,acceptpeople,acceptdeptname,acceptdeptcode,acceptresult,acceptmind,accepttime,reseverid,resevertype,reseverone,resevertwo,reseverthree ,participant,filepath,actionpersonname,actionstatus   from  ( 
                                            select a.belongdepart,a.belongdepartid,a.createuserdeptcode,a.createuserorgcode,a.modifydate, a.createuserid, a.id,a.createdate, a.lllegalnumber,
                                            a.lllegaltype,a.lllegaltypename ,a.lllegaltime,a.lllegallevel,a.lllegallevelname, a.lllegalperson,a.lllegalpersonid,a.lllegalteam,a.lllegalteamcode,
                                            a.lllegaldepart,a.lllegaldepartcode,a.lllegaldescribe,a.lllegaladdress ,a.lllegalpic,a.reformrequire,a.flowstate,a.createusername ,a.addtype,a.isexposure,
                                            a.reformpeople,a.reformpeopleid,a.reformtel,a.reformdeptcode,a.reformdeptname,a.reformdeadline,a.reformfinishdate,a.reformstatus,a.reformmeasure,a.reformchargeperson,
                                            a.reformchargepersonname,a.reformchargedeptid,a.reformchargedeptname,a.isappoint,a.applicationstatus,a.postponedays,a.postponedept,a.postponedeptname,a.postponeperson,
                                            a.postponepersonname,a.isgrpaccept,a.acceptpeopleid,a.acceptpeople,a.acceptdeptname,a.acceptdeptcode,a.acceptresult,a.acceptmind,a.accepttime ,a.reseverid,a.resevertype,
                                            a.reseverone,a.resevertwo,a.reseverthree ,a.participant ,f.filepath,c.actionpersonname,
                                            ( case when  a.flowstate ='违章核准' then  '核准中' when  a.flowstate ='违章审核' then  '审核中' when  a.flowstate ='违章完善' then '完善中'   when  a.flowstate ='制定整改计划' then '制定整改计划中'  when
                                            a.flowstate ='违章整改' then '整改中' when  a.flowstate ='违章验收' then '验收中'  when  a.flowstate ='验收确认' then '验收确认中' when a.flowstate ='流程结束' then '流程结束' else a.flowstate end ) actionstatus ,a.rolename  
                                                from v_lllegalallbaseinfo a
                                                left join v_imageview b on a.lllegalpic = b.recid  
                                                left join (  select a.id,a.participant, (select listagg(b.realname,',') within group(order by b.account) from base_user b
                                                 where instr(','|| substr(a.participant,2,length(a.participant)-1) ||',',','||b.account||',')>0) actionpersonname from v_lllegalworkflow a  ) c on a.id = c.id
                                            left join  (
                                                    select a.id,wm_concat((case when b.filepath is not null then ('{0}'||substr(b.filepath,2)) else '' end)) filepath from bis_lllegalregister a
                                                    left join base_fileinfo b on a.lllegalpic = b.recid  group by a. id
                                                  ) f on  a.id = f.id   
                                        ) a  where flowstate = '违章整改'  and reformdeptcode  like '{1}%'   ", idataitemdetailservice.GetItemValue("imgUrl"), code);
                if (mode == 9)//逾期部分
                {
                    sql += string.Format(@"    and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') > (reformdeadline + 1) ", DateTime.Now);
                }
            }
            //西塞山违章未整改
            else if (mode == 10 || mode == 11)
            {
                string strwhere = string.Empty;

                //厂级、公司层级
                strwhere = @" and  (rolename  like  '%公司级%' or   rolename  like  '%厂级部门用户%'  or  id  in  (select distinct  objectid from v_xsslllegalstandingbook where  encode in (select distinct a.encode  from base_department a  left join base_user b on a.departmentid =b.departmentid where b.rolename like '%厂级%' or  b.rolename like '%公司级%'))) ";

                sql = string.Format(@" select belongdepartid,belongdepart,createuserdeptcode,createuserorgcode,createuserid, createdate,lllegalnumber,lllegaltype, lllegaltypename ,lllegaltime,lllegallevel,
                                    lllegallevelname, lllegalperson,lllegalpersonid,lllegalteam,lllegalteamcode,lllegaldepart,lllegaldepartcode,lllegaldescribe,lllegaladdress ,lllegalpic,
                                    reformrequire,flowstate,createusername ,addtype,isexposure,reformpeople,reformpeopleid,reformtel,reformdeptcode,reformdeptname, to_char(a.reformdeadline,'yyyy-MM-dd') reformdeadline,reformfinishdate,reformstatus,reformmeasure,isgrpaccept,acceptpeopleid,acceptpeople,acceptdeptname,acceptdeptcode,acceptresult,acceptmind,accepttime,reseverid,resevertype,reseverone,resevertwo,reseverthree ,participant,filepath,actionpersonname,actionstatus   from  ( 
                                            select a.belongdepart,a.belongdepartid,a.createuserdeptcode,a.createuserorgcode,a.modifydate, a.createuserid, a.id,a.createdate, a.lllegalnumber,
                                            a.lllegaltype,a.lllegaltypename ,a.lllegaltime,a.lllegallevel,a.lllegallevelname, a.lllegalperson,a.lllegalpersonid,a.lllegalteam,a.lllegalteamcode,
                                            a.lllegaldepart,a.lllegaldepartcode,a.lllegaldescribe,a.lllegaladdress ,a.lllegalpic,a.reformrequire,a.flowstate,a.createusername ,a.addtype,a.isexposure,
                                            a.reformpeople,a.reformpeopleid,a.reformtel,a.reformdeptcode,a.reformdeptname,a.reformdeadline,a.reformfinishdate,a.reformstatus,a.reformmeasure,a.reformchargeperson,
                                            a.reformchargepersonname,a.reformchargedeptid,a.reformchargedeptname,a.isappoint,a.applicationstatus,a.postponedays,a.postponedept,a.postponedeptname,a.postponeperson,
                                            a.postponepersonname,a.isgrpaccept,a.acceptpeopleid,a.acceptpeople,a.acceptdeptname,a.acceptdeptcode,a.acceptresult,a.acceptmind,a.accepttime ,a.reseverid,a.resevertype,
                                            a.reseverone,a.resevertwo,a.reseverthree ,a.participant ,f.filepath,c.actionpersonname,
                                            ( case when a.flowstate ='违章核准' then  '核准中' when  a.flowstate ='违章审核' then  '审核中'  when  a.flowstate ='违章完善' then '完善中'   when  a.flowstate ='制定整改计划' then '制定整改计划中'  when
                                            a.flowstate ='违章整改' then '整改中' when  a.flowstate ='违章验收' then '验收中'  when  a.flowstate ='验收确认' then '验收确认中' when a.flowstate ='流程结束' then '流程结束' else a.flowstate end ) actionstatus ,a.rolename  
                                                from v_lllegalallbaseinfo a
                                                left join v_imageview b on a.lllegalpic = b.recid  
                                                left join (  select a.id,a.participant, (select listagg(b.realname,',') within group(order by b.account) from base_user b
                                                 where instr(','|| substr(a.participant,2,length(a.participant)-1) ||',',','||b.account||',')>0) actionpersonname from v_lllegalworkflow a  ) c on a.id = c.id
                                            left join  (
                                                    select a.id,wm_concat((case when b.filepath is not null then ('{0}'||substr(b.filepath,2)) else '' end)) filepath from bis_lllegalregister a
                                                    left join base_fileinfo b on a.lllegalpic = b.recid  group by a. id
                                                  ) f on  a.id = f.id   
                                        ) a  where flowstate = '违章整改'  and reformdeptcode  like '{1}%'  {2} ", idataitemdetailservice.GetItemValue("imgUrl"), code, strwhere);
                if (mode == 11)//逾期部分
                {
                    sql += string.Format(@"    and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') > (reformdeadline + 1) ", DateTime.Now);
                }
            }
            var dt = this.BaseRepository().FindTable(sql);

            return dt;
        }
        #endregion

        #region 获取指定年月的隐患指标记录
        /// <summary>
        /// 获取指定年月的隐患指标记录
        /// </summary>
        /// <param name="user"></param>
        /// <param name="startDate"></param>
        /// <returns></returns>
        public decimal GetHiddenWarning(Operator user, string startDate)
        {
            string endDate = string.Empty;
            if (startDate.IsEmpty())
            {
                startDate = DateTime.Now.Year.ToString() + "-01" + "-01";
                endDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            }
            else
            {
                endDate = Convert.ToDateTime(startDate).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd"); //截至时间
            }


            DataTable dt = GetHiddenInfoOfWarning(user, startDate, endDate);

            decimal hiddenscore = 0;

            foreach (DataRow row in dt.Rows)
            {
                string score = !string.IsNullOrEmpty(row["score"].ToString()) ? row["score"].ToString() : "0";
                hiddenscore += Convert.ToDecimal(score);
            }
            return hiddenscore;
        }
        #endregion

        #region 获取隐患排查指标相关数据
        /// <summary>
        /// 获取隐患排查指标相关数据
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable GetHiddenInfoOfWarning(Operator user, string startDate, string endDate)
        {
            IClassificationIndexService classificationindexservices = new ClassificationIndexService();

            var list = classificationindexservices.GetListByOrganizeId(user.OrganizeId).Where(p => p.ClassificationCode == "01").ToList();
            if (list.Count() == 0)
            {
                list = classificationindexservices.GetListByOrganizeId("0").Where(p => p.ClassificationCode == "01").ToList();
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("indexcode"); //ID
            dt.Columns.Add("indexname"); //指标项
            dt.Columns.Add("indexscore"); //指标总分
            dt.Columns.Add("indexstandard");//评分标准
            dt.Columns.Add("deductpoint");//扣分
            dt.Columns.Add("score"); //得分
            int indexValue = 1;

            DataTable tempData = new DataTable();

            try
            {
                foreach (ClassificationIndexEntity entity in list)
                {
                    DataRow row = dt.NewRow();
                    row["indexcode"] = entity.IndexCode;
                    row["indexname"] = entity.IndexName;
                    row["indexscore"] = entity.IndexScore;
                    row["indexstandard"] = entity.IndexStandard;
                    string[] argValue = entity.IndexArgsValue.Split('|');

                    decimal downScore = 0; //扣分

                    decimal lastScore = 0;  //最终得分

                    switch (entity.IndexCode)
                    {
                        case "01":  //逾期未整改
                            indexValue = 1;
                            tempData = GetHiddenInfoOfWarning(indexValue, user, startDate, endDate); //逾期未整改的记录数
                            int yqwzg = tempData.Rows.Count; //条数
                            argValue[0] = !string.IsNullOrEmpty(argValue[0]) ? argValue[0].ToString() : "0";
                            downScore = yqwzg * Convert.ToDecimal(argValue[0].ToString());
                            if (downScore > Convert.ToDecimal(entity.IndexScore))
                            {
                                downScore = Convert.ToDecimal(entity.IndexScore);
                            }
                            lastScore = Convert.ToDecimal(entity.IndexScore) - downScore;
                            if (lastScore < 0) { lastScore = 0; }
                            row["deductpoint"] = downScore;
                            row["score"] = lastScore;
                            break;
                        case "02": //未一次性整改隐患
                            indexValue = 2;
                            tempData = GetHiddenInfoOfWarning(indexValue, user, startDate, endDate); //逾期未整改的记录数
                            int wycxzg = tempData.Rows.Count; //条数
                            argValue[0] = !string.IsNullOrEmpty(argValue[0]) ? argValue[0].ToString() : "0";
                            downScore = wycxzg * Convert.ToDecimal(argValue[0].ToString());
                            if (downScore > Convert.ToDecimal(entity.IndexScore))
                            {
                                downScore = Convert.ToDecimal(entity.IndexScore);
                            }
                            lastScore = Convert.ToDecimal(entity.IndexScore) - downScore;
                            if (lastScore < 0) { lastScore = 0; }
                            row["deductpoint"] = downScore;
                            row["score"] = lastScore;
                            break;
                        case "03": //隐患整改率
                            indexValue = 3;
                            tempData = GetHiddenInfoOfWarning(indexValue, user, startDate, endDate); //逾期未整改的记录数
                            if (tempData.Rows.Count == 1)
                            {
                                decimal yhzgl = Convert.ToDecimal(tempData.Rows[0]["yhzgl"]); //隐患整改率
                                argValue[0] = !string.IsNullOrEmpty(argValue[0]) ? argValue[0].ToString() : "0";
                                if (yhzgl < Convert.ToDecimal(argValue[0].ToString()))
                                {
                                    decimal middleValue = Math.Floor(Convert.ToDecimal(argValue[0].ToString()) - yhzgl);  //差值
                                    downScore = Convert.ToDecimal(argValue[1].ToString()) * middleValue;
                                    //扣除的分数大于总分，赋值总分
                                    if (downScore > Convert.ToDecimal(entity.IndexScore))
                                    {
                                        downScore = Convert.ToDecimal(entity.IndexScore);
                                    }
                                    lastScore = Convert.ToDecimal(entity.IndexScore) - downScore;
                                    if (lastScore < 0) { lastScore = 0; }
                                    row["deductpoint"] = downScore;
                                    row["score"] = lastScore;
                                }
                                else
                                {
                                    row["deductpoint"] = 0;
                                    row["score"] = Convert.ToDecimal(entity.IndexScore);
                                }
                            }
                            else
                            {
                                row["deductpoint"] = 0;
                                row["score"] = Convert.ToDecimal(entity.IndexScore);
                            }
                            break;
                    }
                    dt.Rows.Add(row);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return dt;
        }
        #endregion

        #region 每一个月的考核
        /// <summary>
        /// 每一个月的考核
        /// </summary>
        /// <param name="user"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public object GetHiddenInfoOfEveryMonthWarning(Operator user, string startDate, string endDate)
        {
            IClassificationIndexService classificationindexservices = new ClassificationIndexService();
            IClassificationService classificationservice = new ClassificationService();
            decimal allScore = 0;//应得总分
            decimal finalScore = 0;//实际总得分
            decimal weight = 0; //权重
            string orgId = user.OrganizeId;//用户所属机构
            //获取权重
            var classification = classificationservice.GetList(orgId).Where(p => p.ClassificationCode == "01").ToList().FirstOrDefault();
            if (null != classification)
            {
                weight = Convert.ToDecimal(classification.WeightCoeffcient);
            }
            //其他
            var list = classificationindexservices.GetListByOrganizeId(user.OrganizeId).Where(p => p.ClassificationCode == "01").ToList();
            if (list.Count() == 0)
            {
                list = classificationindexservices.GetListByOrganizeId("0").Where(p => p.ClassificationCode == "01").ToList();
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("indexcode"); //ID
            dt.Columns.Add("indexname"); //指标项
            dt.Columns.Add("indexscore"); //指标总分
            dt.Columns.Add("indexstandard");//评分标准
            dt.Columns.Add("deductpoint");//扣分
            dt.Columns.Add("score"); //得分
            int indexValue = 1;

            DataTable tempData = new DataTable();

            try
            {
                foreach (ClassificationIndexEntity entity in list)
                {
                    DataRow row = dt.NewRow();
                    row["indexcode"] = entity.IndexCode;
                    row["indexname"] = entity.IndexName;
                    row["indexscore"] = entity.IndexScore;
                    row["indexstandard"] = entity.IndexStandard;
                    string[] argValue = entity.IndexArgsValue.Split('|');

                    decimal downScore = 0; //扣分

                    decimal lastScore = 0;  //最终得分

                    #region 计算
                    switch (entity.IndexCode)
                    {
                        case "01":  //逾期未整改
                            indexValue = 1;
                            tempData = GetHiddenInfoOfWarning(indexValue, user, startDate, endDate); //逾期未整改的记录数
                            int yqwzg = tempData.Rows.Count; //条数
                            argValue[0] = !string.IsNullOrEmpty(argValue[0]) ? argValue[0].ToString() : "0";
                            downScore = yqwzg * Convert.ToDecimal(argValue[0].ToString());
                            if (downScore > Convert.ToDecimal(entity.IndexScore))
                            {
                                downScore = Convert.ToDecimal(entity.IndexScore);
                            }
                            lastScore = Convert.ToDecimal(entity.IndexScore) - downScore;
                            if (lastScore < 0) { lastScore = 0; }
                            row["deductpoint"] = downScore;
                            row["score"] = lastScore;
                            break;
                        case "02": //未一次性整改隐患
                            indexValue = 2;
                            tempData = GetHiddenInfoOfWarning(indexValue, user, startDate, endDate); //逾期未整改的记录数
                            int wycxzg = tempData.Rows.Count; //条数
                            argValue[0] = !string.IsNullOrEmpty(argValue[0]) ? argValue[0].ToString() : "0";
                            downScore = wycxzg * Convert.ToDecimal(argValue[0].ToString());
                            if (downScore > Convert.ToDecimal(entity.IndexScore))
                            {
                                downScore = Convert.ToDecimal(entity.IndexScore);
                            }
                            lastScore = Convert.ToDecimal(entity.IndexScore) - downScore;
                            if (lastScore < 0) { lastScore = 0; }
                            row["deductpoint"] = downScore;
                            row["score"] = lastScore;
                            break;
                        case "03": //隐患整改率
                            indexValue = 3;
                            tempData = GetHiddenInfoOfWarning(indexValue, user, startDate, endDate); //逾期未整改的记录数
                            if (tempData.Rows.Count == 1)
                            {
                                decimal yhzgl = Convert.ToDecimal(tempData.Rows[0]["yhzgl"]); //隐患整改率
                                argValue[0] = !string.IsNullOrEmpty(argValue[0]) ? argValue[0].ToString() : "0";
                                if (yhzgl < Convert.ToDecimal(argValue[0].ToString()))
                                {
                                    decimal middleValue = Math.Floor(Convert.ToDecimal(argValue[0].ToString()) - yhzgl);  //差值
                                    downScore = Convert.ToDecimal(argValue[1].ToString()) * middleValue;
                                    //扣除的分数大于总分，赋值总分
                                    if (downScore > Convert.ToDecimal(entity.IndexScore))
                                    {
                                        downScore = Convert.ToDecimal(entity.IndexScore);
                                    }
                                    lastScore = Convert.ToDecimal(entity.IndexScore) - downScore;
                                    if (lastScore < 0) { lastScore = 0; }
                                    row["deductpoint"] = downScore;
                                    row["score"] = lastScore;
                                }
                                else
                                {
                                    row["deductpoint"] = 0;
                                    row["score"] = Convert.ToDecimal(entity.IndexScore);
                                }
                            }
                            else
                            {
                                row["deductpoint"] = 0;
                                row["score"] = Convert.ToDecimal(entity.IndexScore);
                            }
                            break;
                    }
                    #endregion
                    allScore += Convert.ToDecimal(entity.IndexScore);  //总分
                    finalScore += Convert.ToDecimal(row["score"].ToString()); //实际总得分
                    dt.Rows.Add(row);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return new { data = dt, allScore = allScore, finalScore = finalScore, weight = weight };
        }
        #endregion

        #region 获取隐患排查指标相关数据
        /// <summary>
        /// 获取隐患排查指标相关数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable GetHiddenInfoOfWarning(int action, Operator user, string startDate, string endDate)
        {

            DatabaseType dataType = DbHelper.DbType;

            string sql = string.Empty;

            string deptcode = user.OrganizeCode;

            endDate = Convert.ToDateTime(endDate).AddDays(1).ToString("yyyy-MM-dd");
            try
            {
                if (action < 3)
                {

                    sql = @"select  account,createuserdeptcode,createuserorgcode,createuserid,checktypename,hidtypename,hidrankname,checkdepartname,
                                    id,createdate,hidcode,checktype,isgetafter,exposurestate,isbreakrule,hidtype,acceptdepartcode,acceptperson,changeperson,
                                    hidrank,hidpoint,workstream,addtype,participant,applicationstatus,postponedept,postponedeptname,hiddescribe,changemeasure,
                                    changedeadine,changedutydepartcode,safetycheckobjectid from v_basehiddeninfo";

                    string strWhere = " where 1=1 ";

                    string strDeptCode = string.Empty;
                    //省级首页统计
                    #region 当前用户是公司级及厂级用户
                    //当前用户是公司级及厂级用户
                    if (user.RoleName.Contains("省级用户") || user.RoleName.Contains("厂级") || user.RoleName.Contains("公司级"))
                    {
                        strDeptCode = user.NewDeptCode; //新机构
                    }
                    else
                    {
                        DepartmentEntity deptEntity = Idepartmentservice.GetEntity(user.DeptId);
                        strDeptCode = deptEntity.DeptCode;  //当前用户部门的所属新编码
                    }
                    #endregion

                    strWhere += string.Format(@"   and  changedeptcode  like   '{0}%'  and to_date('{1}','yyyy-mm-dd hh24:mi:ss') <= createdate  and createdate <=  to_date('{2}','yyyy-mm-dd hh24:mi:ss')  ", strDeptCode, startDate, endDate);

                    switch (action)
                    {
                        //逾期未整改数量
                        case 1:
                            strWhere += string.Format(@" and workstream = '隐患整改'  and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') > changedeadine + 1", DateTime.Now.ToString("yyyy-MM-dd"));
                            break;
                        //未一次性整改隐患
                        case 2:
                            strWhere += string.Format(@"  and  hidcode in (select  distinct a.hidcode from v_muchhtchangeinfo  a
                                                                                left join base_department b on a.changedutydepartcode = b.encode 
                                                                                where b.deptcode like '{0}%')", strDeptCode);  //获取多次整改，即未一次性整改完成
                            break;
                    }

                    sql += strWhere;
                }
                else if (action == 3)  //隐患整改率
                {
                    sql = string.Format(@" select nvl(a.pnum,0) as total , nvl(b.pnum,0) as yzg,nvl(c.pnum,0) as wzg,(case when nvl(a.pnum,0) =0 then 0 else round(nvl(b.pnum,0) * 100 / a.pnum,2) end)  as yhzgl from  (
                                                select  count(1) as pnum ,createuserorgcode from v_basechangeinfo where createuserorgcode ='{0}' and ( changedutydepartcode  like '{1}%' or changedutydepartcode in 
                                                (select a.encode from base_department a left join base_department b on a.senddeptid = b.departmentid 
                                                where  a.senddeptid is not null  and b.encode like '{1}%'  ) ) and to_date('{2}','yyyy-mm-dd hh24:mi:ss') <= createdate  and createdate <=  to_date('{3}','yyyy-mm-dd hh24:mi:ss')
                                                group by createuserorgcode
                                           ) a
                                        left join (
                                                select  count(1) as pnum ,createuserorgcode from v_basechangeinfo where createuserorgcode ='{0}' and ( changedutydepartcode  like '{1}%' or changedutydepartcode in 
                                                (select a.encode from base_department a left join base_department b on a.senddeptid = b.departmentid 
                                                where  a.senddeptid is not null  and b.encode like '{1}%'  ) ) and to_date('{2}','yyyy-mm-dd hh24:mi:ss') <= createdate  and createdate <=  to_date('{3}','yyyy-mm-dd hh24:mi:ss')
                                                and  workstream in ('隐患验收','复查验证','整改效果评估','整改结束') group by createuserorgcode
                                        ) b on a.createuserorgcode =b.createuserorgcode
                                        left join (                                
                                            select  count(1) as pnum ,createuserorgcode from v_basechangeinfo where createuserorgcode ='{0}' and ( changedutydepartcode  like '{1}%' or changedutydepartcode in 
                                            (select a.encode from base_department a left join base_department b on a.senddeptid = b.departmentid 
                                            where  a.senddeptid is not null  and b.encode like '{1}%'  ) ) and to_date('{2}','yyyy-mm-dd hh24:mi:ss') <= createdate  and createdate <=  to_date('{3}','yyyy-mm-dd hh24:mi:ss')
                                            and  workstream = '隐患整改' group by createuserorgcode
                                        )  c on a.createuserorgcode =c.createuserorgcode", user.OrganizeCode, deptcode, startDate, endDate);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return this.BaseRepository().FindTable(sql);
        }
        #endregion

        #region 获取隐患排查指标相关数据
        /// <summary>
        /// 获取隐患排查指标相关数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable GetSafetyValueOfWarning(int action, string orgCode, string startDate, string endDate)
        {

            DatabaseType dataType = DbHelper.DbType;

            string sql = string.Empty;

            string deptcode = orgCode;

            endDate = Convert.ToDateTime(endDate).AddDays(1).ToString("yyyy-MM-dd");
            try
            {

                sql = @"select  account,createuserdeptcode,createuserorgcode,createuserid,checktypename,hidtypename,hidrankname,checkdepartname,
                            id,createdate,hidcode,checktype,isgetafter,exposurestate,isbreakrule,hidtype,acceptdepartcode,acceptperson,changeperson,
                            hidrank,hidpoint,workstream,addtype,participant,applicationstatus,postponedept,postponedeptname,hiddescribe,changemeasure,
                            changedeadine,changedutydepartcode,safetycheckobjectid from v_basehiddeninfo";

                string strWhere = " where 1=1 ";

                string strDeptCode = string.Empty;

                if (!string.IsNullOrEmpty(startDate))
                {
                    strWhere += string.Format(@"   and   to_date('{0}','yyyy-mm-dd hh24:mi:ss') <= createdate   ", startDate);
                }

                if (!string.IsNullOrEmpty(endDate))
                {
                    strWhere += string.Format(@"   and  createdate <=  to_date('{0}','yyyy-mm-dd hh24:mi:ss')    ", endDate);
                }
                if (!string.IsNullOrEmpty(deptcode))
                {
                    strWhere += string.Format(@"   and  changedutydepartcode  like   '{0}%'  ", deptcode);
                }

                switch (action)
                {
                    //逾期未整改重大隐患数量
                    case 1:
                        strWhere += string.Format(@" and workstream = '隐患整改' and rankname = '重大隐患'  and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') > changedeadine + 1", DateTime.Now.ToString("yyyy-MM-dd"));
                        break;
                    //逾期未整改一般隐患数
                    case 2:
                        strWhere += string.Format(@" and workstream = '隐患整改' and rankname = '一般隐患'  and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') > changedeadine + 1", DateTime.Now.ToString("yyyy-MM-dd"));
                        break;
                }

                sql += strWhere;
                return this.BaseRepository().FindTable(sql);

            }
            catch (Exception)
            {
                throw;
            }


        }
        #endregion

        #region 重要指标(省级)
        /// <summary>
        /// 重要指标(省级)
        /// </summary>
        /// <param name="action"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable GetImportantIndexForProvincial(int action, Operator user)
        {
            string sql = string.Empty;


            switch (action)
            {
                //重大隐患数量(往年未整改的重大隐患+本年度所有的重大隐患) 按电厂来列
                case 1:
                    sql = string.Format(@" select count(b.id) num,a.encode deptcode, a.departmentname deptname  from ( select departmentid ,fullname departmentname,deptcode,encode  from base_department where nature ='厂级' and deptcode like '{0}%') a
                                             left join (
                                              select id,hiddepart from v_basehiddeninfo  where rankname ='重大隐患' and to_char(createdate,'yyyy') ='{1}'
                                              union 
                                              select id,hiddepart from v_basehiddeninfo  where rankname ='重大隐患' and to_char(createdate,'yyyy') !='{1}' and workstream ='隐患整改' 
                                            ) b on a.departmentid = b.hiddepart  group by  a.departmentname,a.encode ,a.deptcode order by deptcode ", user.NewDeptCode, DateTime.Now.Year);
                    break;
                //逾期未整改隐患(截止到当前时间) 按电厂来列
                case 2:
                    sql = string.Format(@" select count(b.id) num,a.encode deptcode, a.departmentname  deptname from ( select departmentid ,fullname departmentname,deptcode ,encode from base_department where nature ='厂级' and deptcode like '{0}%') a
                                         left join (
                                          select id,hiddepart from v_basehiddeninfo  where workstream = '隐患整改'  and  to_date('{1}','yyyy-mm-dd hh24:mi:ss') > changedeadine + 1
                                        ) b on a.departmentid = b.hiddepart  group by  a.departmentname,a.encode  ,a.deptcode order by deptcode", user.NewDeptCode, DateTime.Now);
                    break;
                //省公司发现的隐患(本年度) 按电厂来列
                case 3:
                    sql = string.Format(@" select count(b.id) num,a.encode deptcode, a.departmentname deptname from ( select departmentid ,fullname departmentname,deptcode,encode  from base_department where nature ='厂级' and deptcode like '{0}%') a
                                             left join (
                                              select id,hiddepart from v_basehiddeninfo  where  to_char(createdate,'yyyy') ='{1}'  and addtype=2
                                            ) b on a.departmentid = b.hiddepart  group by  a.departmentname ,a.encode ,a.deptcode order by deptcode", user.NewDeptCode, DateTime.Now.Year);
                    break;
                //违章数量(本年度) 按电厂来列
                case 4:
                    sql = string.Format(@"  select count(b.id) num,a.encode deptcode, a.departmentname  deptname from ( select departmentid ,fullname departmentname,encode, deptcode  from base_department where nature ='厂级' and deptcode like '{0}%') a
                                             left join (
                                              select id,reformdeptcode  from v_lllegalbaseinfo   where  to_char(createdate,'yyyy') ='{1}'  
                                            ) b on a.encode = substr(b.reformdeptcode,1,length(a.encode))  group by  a.departmentname,a.encode  ,a.deptcode order by deptcode", user.NewDeptCode, DateTime.Now.Year);
                    break;
            }
            return this.BaseRepository().FindTable(sql);
        }
        #endregion

        #region 电力安全隐患排查治理情况月报表
        /// <summary>
        /// 电力安全隐患排查治理情况月报表 
        /// </summary>
        /// <param name="deptcode"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public DataTable GetHiddenSituationOfMonth(string deptcode, string curdate, Operator curUser)
        {
            string sql = string.Empty;  //所有统计

            string strwhere = "   1=1"; //查询条件

            string hidtype = "HidType";
            //行业版本
            if (curUser.Industry != "电力" && !string.IsNullOrEmpty(curUser.Industry))
            {
                hidtype = "GIHiddenType";
            }

            //部门
            if (!string.IsNullOrEmpty(deptcode))
            {
                strwhere += string.Format(@"  and createuserdeptcode  like '{0}%'", deptcode);
            }
            //年度
            if (!string.IsNullOrEmpty(curdate))
            {
                strwhere += string.Format(@"  and to_char(createdate,'yyyy') = '{0}'", curdate);
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("typename"); //隐患类别
            dt.Columns.Add("col1"); //隐患类别
            dt.Columns.Add("col2"); //隐患类别
            dt.Columns.Add("col3"); //隐患类别
            dt.Columns.Add("yjhid");  //一级重大隐患排查数量
            dt.Columns.Add("zgyjhid"); //已整改的一级重大隐患
            dt.Columns.Add("yjhidzgl"); //一级重大隐患整改率 
            dt.Columns.Add("ejhid");  //二级重大隐患排查数量
            dt.Columns.Add("zgejhid"); //已整改的二级重大隐患
            dt.Columns.Add("ejhidzgl"); //二级重大隐患整改率
            dt.Columns.Add("ybhid");   //一般隐患排查数量
            dt.Columns.Add("zgybhid");  //已整改的一般隐患
            dt.Columns.Add("ybhidzgl"); //一般隐患整改率
            dt.Columns.Add("money");  //落实的隐患治理资金总和

            //获取总的隐患统计

            sql = string.Format(@" select a.typename ,sum(nvl(a.ybhid,0)) ybhid,sum(nvl(a.yjhid,0)) yjhid,sum(nvl(a.ejhid,0)) ejhid,sum(nvl(b.zgybhid,0)) zgybhid,sum(nvl(b.zgyjhid,0)) zgyjhid ,sum(nvl(b.zgejhid,0)) zgejhid,sum(nvl(c.money,0)) money from (
                                    select * from (
                                        select  (case when a.itemcode !='0' then '其他事故隐患' else  a.itemname end)  typename ,b.numbers numbers ,b.hidrankname rankname  ,a.sortcode from (
                                            select b.itemdetailid,b.itemcode, b.itemname ,b.sortcode  from base_dataitem a 
                                            left join base_dataitemdetail b on a.itemid = b.itemid where a.itemcode ='{1}' order by b.itemcode
                                        ) a
                                        left join (
                                          select count(1) numbers, (case when  instr(hidrankname,'一般隐患')>0 then '一般隐患' when hidrankname ='I级重大隐患' then 'Ⅰ级重大隐患' 
                                          when hidrankname ='II级重大隐患' then 'Ⅱ级重大隐患'  else hidrankname end ) hidrankname ,hidtype from v_basehiddeninfo where  {0} group by hidrankname,hidtype
                                        ) b on a.itemdetailid =b.hidtype
                                    )  pivot (sum(numbers) for rankname in ('一般隐患' as ybHid,'Ⅰ级重大隐患' as yjHid,'Ⅱ级重大隐患' as ejHid))   order by sortcode
                                    )  a 
                                    left join (
                                    select * from (
                                        select  (case when a.itemcode !='0' then '其他事故隐患' else  a.itemname end)  typename ,b.numbers numbers ,b.hidrankname rankname ,a.sortcode  from (
                                          select  b.itemdetailid,b.itemcode, b.itemname,b.sortcode from base_dataitem a 
                                          left join base_dataitemdetail b on a.itemid = b.itemid where a.itemcode ='{1}' order by b.itemcode
                                        ) a
                                        left join (
                                        select count(1) numbers, (case when  instr(hidrankname,'一般隐患')>0 then '一般隐患' when hidrankname ='I级重大隐患' then 'Ⅰ级重大隐患' 
                                          when hidrankname ='II级重大隐患' then 'Ⅱ级重大隐患'  else hidrankname end ) hidrankname ,hidtype from v_basehiddeninfo where  {0}  and  workstream != '隐患整改'    group by hidrankname,hidtype
                                        ) b on a.itemdetailid =b.hidtype
                                    )  pivot (sum(numbers) for rankname in ('一般隐患' as zgybHid,'Ⅰ级重大隐患' as zgyjHid,'Ⅱ级重大隐患' as zgejHid)) order by sortcode
                                    ) b on a.typename =b.typename
                                    left join (
                                      select  (case when a.itemcode !='0' then '其他事故隐患' else  a.itemname end)  typename,nvl(b.money,0)  money   from (
                                          select b.itemdetailid,b.itemcode,b.itemname,b.sortcode from base_dataitem a 
                                          left join base_dataitemdetail b on a.itemid = b.itemid where a.itemcode ='{1}' order by b.itemcode
                                        ) a
                                        left join (
                                         select sum(realitymanagecapital) money,hidtype  from v_basehiddeninfo where  {0} and  workstream != '隐患整改'   group by hidtype
                                        ) b on a.itemdetailid =b.hidtype  order by sortcode
                                    )  c on a.typename =c.typename  group by  a.typename ", strwhere, hidtype);

            DataTable tdt = this.BaseRepository().FindTable(sql);

            int num = 1;
            foreach (DataRow trow in tdt.Rows)
            {
                DataRow row = dt.NewRow();
                row["typename"] = num.ToString() + "、" + trow["typename"].ToString();
                row["col1"] = "";
                row["col2"] = "";
                row["col3"] = "";

                //一级重大隐患
                row["yjhid"] = trow["yjhid"].ToString();
                row["zgyjhid"] = trow["zgyjhid"].ToString();
                string yjzgl = "0";
                if (!string.IsNullOrEmpty(trow["yjhid"].ToString()) && Convert.ToDecimal(trow["yjhid"].ToString()) > 0)
                {
                    yjzgl = Math.Round(Convert.ToDecimal(!string.IsNullOrEmpty(trow["zgyjhid"].ToString()) ? trow["zgyjhid"].ToString() : "0") / Convert.ToDecimal(trow["yjhid"].ToString()) * 100, 2).ToString();
                }
                row["yjhidzgl"] = yjzgl;
                //二级重大隐患
                row["ejhid"] = trow["ejhid"].ToString();
                row["zgejhid"] = trow["zgejhid"].ToString();
                string ejzgl = "0";
                if (!string.IsNullOrEmpty(trow["ejhid"].ToString()) && Convert.ToDecimal(trow["ejhid"].ToString()) > 0)
                {
                    ejzgl = Math.Round(Convert.ToDecimal(!string.IsNullOrEmpty(trow["zgejhid"].ToString()) ? trow["zgejhid"].ToString() : "0") / Convert.ToDecimal(trow["ejhid"].ToString()) * 100, 2).ToString();
                }
                row["ejhidzgl"] = ejzgl;
                row["money"] = Math.Round(Convert.ToDecimal(trow["money"].ToString()) / 10000, 4).ToString();
                dt.Rows.Add(row);

                //一般隐患
                row["ybhid"] = trow["ybhid"].ToString();
                row["zgybhid"] = trow["zgybhid"].ToString();
                string ybzgl = "0";
                if (!string.IsNullOrEmpty(trow["ybhid"].ToString()) && Convert.ToDecimal(trow["ybhid"].ToString()) > 0)
                {

                    ybzgl = Math.Round(Convert.ToDecimal(!string.IsNullOrEmpty(trow["zgybhid"].ToString()) ? trow["zgybhid"].ToString() : "0") / Convert.ToDecimal(trow["ybhid"].ToString()) * 100, 2).ToString();
                }
                row["ybhidzgl"] = ybzgl;

                num++;
            }
            return dt;
        }
        #endregion

        #region 获取首页隐患统计
        /// <summary>
        /// 获取首页隐患统计
        /// </summary>
        /// <param name="orginezeId"></param>
        /// <param name="curYear"></param>
        /// <param name="topNum"></param>
        /// <returns></returns>
        public DataTable GetHomePageHiddenByHidType(Operator curUser, int curYear, int topNum, int qType)
        {

            string sql = string.Empty;

            //按排查单位进行统计(部门级)
            if (qType == 0)
            {
                sql = string.Format(@"select a.numbers,a.encode,a.fullname  from (
                                        select count(b.id) numbers,a.encode,a.fullname,a.sortcode from 
                                        (
                                           select encode,fullname,sortcode  from base_department  where organizeid ='{0}' and nature ='部门' order by sortcode
                                        ) a
                                        left join
                                        (
                                           select checkdepartid ,id from v_basehiddeninfo where  to_char(createdate,'yyyy')={1}  
                                       ) b on  substr(b.checkdepartid,0,length(a.encode)) = a.encode  
                                        group by a.encode,a.fullname,a.sortcode order by  numbers  asc,sortcode asc
                                      )  a where rownum < {2}", curUser.OrganizeId, curYear, topNum);
            }
            //按整改单位进行统计(部门级)
            if (qType == 1)
            {
                sql = string.Format(@"select a.numbers,a.encode,a.fullname  from (
                                        select count(b.id) numbers,a.encode,a.fullname,a.sortcode from 
                                        (
                                           select encode,fullname,sortcode  from base_department  where organizeid ='{0}' and nature ='部门' order by sortcode
                                        ) a
                                        left join
                                        (
                                            select changedutydepartcode ,id from v_basehiddeninfo where  to_char(createdate,'yyyy')={1} 
                                        ) b on substr(b.changedutydepartcode,0,length(a.encode)) = a.encode 
                                        group by a.encode,a.fullname,a.sortcode order by  numbers  asc,sortcode asc
                                      )  a where rownum < {2}", curUser.OrganizeId, curYear, topNum);
            }
            DataTable dt = this.BaseRepository().FindTable(sql);
            return dt;
        }
        #endregion

        #region 根据部门编码获取首页统计数据
        /// <summary>
        /// 根据部门编码获取
        /// </summary>
        /// <param name="orginezeId"></param>
        /// <param name="encode"></param>
        /// <param name="curYear"></param>
        /// <param name="qType"></param>
        /// <returns></returns>
        public DataTable GetHomePageHiddenByDepart(string orginezeId, string encode, string curYear, int qType)
        {
            string sql = string.Empty;

            //按排查单位进行统计(部门级)
            if (qType == 0)
            {
                sql = string.Format(@" select a.numbers,a.encode,a.fullname  from (
                                            select count(b.id) numbers,a.encode,a.fullname,a.sortcode from (
                                            select encode,fullname,sortcode ,nature  from base_department  where organizeid ='{0}' and encode like '{1}%' order by sortcode
                                        ) a
                                        left join ( select checkdepartid,id from v_basehiddeninfo  where  to_char(createdate,'yyyy')={2} and checkdepartid  
                                       like '{1}%' and organizeid ='{0}' ) b on b.checkdepartid =  a.encode   
                                        group by a.encode,a.fullname,a.sortcode order by  numbers  desc,sortcode asc ) a", orginezeId, encode, curYear);
            }
            //按整改单位进行统计(部门级)
            if (qType == 1)
            {
                sql = string.Format(@" select a.numbers,a.encode,a.fullname  from (
                                            select count(b.id) numbers,a.encode,a.fullname,a.sortcode from (
                                            select encode,fullname,sortcode ,nature  from base_department  where organizeid ='{0}' and encode like '{1}%' order by sortcode
                                        ) a
                                        left join ( select changedutydepartcode,id from v_basehiddeninfo  where  to_char(createdate,'yyyy')={2} and changedutydepartcode  
                                       like '{1}%' and organizeid ='{0}' ) b on b.changedutydepartcode =  a.encode   
                                        group by a.encode,a.fullname,a.sortcode order by  numbers  desc,sortcode asc) a", orginezeId, encode, curYear);
            }

            DataTable dt = this.BaseRepository().FindTable(sql);

            return dt;
        }
        #endregion

        #region 获取安全预警
        /// <summary>
        /// 获取安全预警
        /// </summary>
        /// <returns></returns>
        public DataTable GetHidSafetyWarning(int type, string orgcode)
        {

            DataTable dt = new DataTable();
            string sql = string.Empty;
            string sql1 = string.Empty;

            //隐患整改率低于80%
            if (type == 1)
            {
                dt.Columns.Add("total");  //总数
                dt.Columns.Add("deptname"); //电厂名称
                dt.Columns.Add("deptcode"); //电厂编码
                dt.Columns.Add("organizeid"); //机构id

                sql1 = string.Format(@"select encode ,fullname ,departmentid,organizeid,deptcode from BASE_DEPARTMENT t where deptcode like '{0}%'  and nature ='厂级' ", orgcode);

                DataTable deptDt = this.BaseRepository().FindTable(sql1);

                foreach (DataRow row in deptDt.Rows)
                {
                    string orgid = row["departmentid"].ToString();  //电厂id

                    sql = string.Format(@"select  nvl(b.yzgnum,0) as  yzgnum , nvl(c.wzgnum,0) as  wzgnum, nvl((case when nvl(a.allnum,0) =0 then 0 else round(b.yzgnum * 100 / a.allnum,2) end),0)  as allzgl
                                        from (                                        
                                        select count(1) as allnum ,to_char(createdate,'yyyy') as createdate from v_basehiddeninfo  where  workstream 
                                        in ( '隐患整改','隐患验收','复查验证','整改效果评估','整改结束')  and 
                                         hiddepart = '{0}' and to_char(createdate,'yyyy') ='{1}' group by to_char(createdate,'yyyy')
                                         )  a 
                                         left join (
                                       select count(1) as yzgnum,to_char(createdate,'yyyy') as createdate  from v_basehiddeninfo  where  
                                       workstream in ('隐患验收','复查验证','整改效果评估','整改结束')   and 
                                       hiddepart = '{0}' and to_char(createdate,'yyyy') ='{1}'  group by to_char(createdate,'yyyy') 
                                        )  b on a.createdate = b.createdate
                                        left join (
                                          select count(1) as wzgnum,to_char(createdate,'yyyy') as createdate  from v_basehiddeninfo  where  
                                         workstream in ('隐患整改')   and 
                                        hiddepart = '{0}' and to_char(createdate,'yyyy') ='{1}'  group by to_char(createdate,'yyyy')
                                       ) c on a.createdate = c.createdate  ", orgid, DateTime.Now.Year.ToString());

                    DataTable tDt = this.BaseRepository().FindTable(sql);

                    if (tDt.Rows.Count == 1)
                    {
                        decimal total = Convert.ToDecimal(tDt.Rows[0]["allzgl"].ToString());
                        //当前年整改率小于80%的数据
                        if (total < 80)
                        {
                            DataRow prow = dt.NewRow();
                            prow["total"] = tDt.Rows[0]["allzgl"].ToString() + "%";
                            prow["deptname"] = row["fullname"].ToString();
                            prow["deptcode"] = row["encode"].ToString();
                            prow["organizeid"] = row["organizeid"].ToString();
                            dt.Rows.Add(prow);
                        }
                    }
                }
            }
            //存在重大隐患
            if (type == 2)
            {
                sql = string.Format(@"select count(b.id) total,a.fullname  deptname ,a.encode deptcode ,a.organizeid from ( 
                    select encode ,fullname ,departmentid,organizeid,deptcode from BASE_DEPARTMENT t where deptcode like '{0}%'  and nature ='厂级' 
                    )  a 
                    left join   
                    (
                      select hiddepart ,id from v_basehiddeninfo where rankname ='重大隐患' and  to_char(createdate,'yyyy') ='{1}' 
                    ) b on a.departmentid = b.hiddepart  group by a.fullname ,a.encode,a.organizeid  order by deptcode", orgcode, DateTime.Now.Year.ToString());

                dt = this.BaseRepository().FindTable(sql);
            }

            return dt;
        }
        #endregion

        #region 按月统计当天登记的违章并且当天未验收的违章数量和当天登记的违章的总数量
        /// <summary>
        /// 按月统计当天登记的违章并且当天未验收的违章数量和当天登记的违章的总数量
        /// </summary>
        /// <param name="currDate">时间</param>
        ///  <param name="deptCode">部门Code</param>
        /// <returns></returns>
        public DataTable GetLllegalRegisterNumByMonth(string currDate, string deptCode)
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            DataTable dt = new DataTable();

            string twhere = " and 1=1 ";

            //如果是负责人/班组长，则查看部门新增的
            if (curUser.RoleName.Contains("负责人"))
            {
                twhere += string.Format("  and r.createuserdeptcode = '{0}'", curUser.DeptCode);
            }
            else
            {
                twhere += string.Format("  and  r.createuserid = '{0}'", curUser.UserId);
            }
            string sql = string.Format(@"select to_char(dates) dates, to_char(sum(alls)) alls ,to_char(sum(dcls)) dcls ,to_char(sum(regsl)) regsl from (
                                        select 
                                       cast(to_char(r.createdate, 'dd')as number) dates,
                                        count(id) alls,
                                        sum(case when flowstate in (select itemname from v_yesqrwzstatus where itemname !='流程结束') then 1 else 0 end) dcls,
                                        0  regsl
                                        from v_lllegalallbaseinfo r
                                        where to_char(r.createdate, 'MM') = to_char(to_date('{0}','yyyy-MM-dd'), 'MM') and 
                                        to_char(r.createdate, 'yyyy') = to_char(to_date('{0}','yyyy-MM-dd'), 'yyyy') and 
                                        r.lllegalteamcode like '{1}%' group by to_char(r.createdate, 'dd')
                                        union
                                        select 
                                        cast(to_char(r.createdate, 'dd') as number) dates,
                                        0 alls,
                                        0 dcls,
                                        count(id) regsl
                                        from v_lllegalallbaseinfo r
                                        where to_char(r.createdate, 'MM') = to_char(to_date('{0}','yyyy-MM-dd'), 'MM') and 
                                        to_char(r.createdate, 'yyyy') = to_char(to_date('{0}','yyyy-MM-dd'), 'yyyy')  
                                        {2} group by to_char(r.createdate, 'dd')
                                        ) a group by dates   order by dates desc", currDate, deptCode, twhere);

            dt = this.BaseRepository().FindTable(sql);

            return dt;
        }
        #endregion

        #region 导出安全检查对应的隐患内容
        /// <summary>
        /// 导出安全检查对应的隐患内容
        /// </summary>
        /// <param name="keyvalue"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public DataTable GetHiddenOfSafetyCheck(string keyValue, int mode)
        {
            string sql = string.Empty;

            if (mode == 0)
            {
                sql = string.Format(@"select a.id,a.hiddescribe,a.changemeasure measure,a.changedutydepartname dutydept,a.changepersonname dutyman,  to_char(a.changedeadine,'yyyy-MM-dd') endtime,
                       (case when a.workstream ='整改结束' then  to_char(b.acceptdate,'yyyy-MM-dd') else '' end) complatetime,(case when a.workstream ='整改结束' then to_char(b.acceptpersonname)  else '' end) checkman,a.changeresume,a.hidchangephoto,c.filepath ,d.reformfilepath from v_hiddenbasedata a 
                        left join v_htacceptinfo b on a.hidcode = b.hidcode 
                        left join  (
                          select a.id,wm_concat((case when b.filepath is not null then  b.filepath else '' end)) filepath from bis_htbaseinfo a
                          left join base_fileinfo b on a.hidphoto = b.recid  group by a.id
                        ) c on  a.id = c.id 
                        left join  (
                          select a.hidcode,wm_concat((case when b.filepath is not null then  b.filepath else '' end)) reformfilepath from v_htchangeinfo a
                          left join base_fileinfo b on a.hidchangephoto = b.recid  group by a.hidcode
                        ) d on  a.hidcode = d.hidcode where a.safetycheckobjectid='{0}'", keyValue);
            }
            else
            {
                sql = string.Format(@"select a.id,a.hiddescribe,a.changemeasure measure,a.changedutydepartname dutydept,a.changepersonname dutyman,  to_char(a.changedeadine,'yyyy-MM-dd') endtime,
                       (case when a.workstream ='整改结束' then  to_char(b.acceptdate,'yyyy-MM-dd') else '' end) complatetime,(case when a.workstream ='整改结束' then to_char(b.acceptpersonname)  else '' end) checkman,a.changeresume,a.hidchangephoto,c.filepath ,d.reformfilepath from v_hiddenbasedata a 
                        left join v_htacceptinfo b on a.hidcode = b.hidcode 
                        left join  (
                          select a.id,wm_concat((case when b.filepath is not null then  b.filepath else '' end)) filepath from bis_htbaseinfo a
                          left join base_fileinfo b on a.hidphoto = b.recid  group by a.id
                        ) c on  a.id = c.id 
                        left join  (
                          select a.hidcode,wm_concat((case when b.filepath is not null then  b.filepath else '' end)) reformfilepath from v_htchangeinfo a
                          left join base_fileinfo b on a.hidchangephoto = b.recid  group by a.hidcode
                        ) d on  a.hidcode = d.hidcode where a.id ='{0}'", keyValue);
            }
            return this.BaseRepository().FindTable(sql);
        }
        #endregion

        #region 获取江凌-未整改隐患
        /// <summary>
        /// 获取江凌-未整改隐患
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable GetNoChangeHidList(string code)
        {
            string sql = string.Format(@" select a.id,a.hidcode,f.districtname , a.workstream,a.addtype, a.hiddescribe,c.itemname rankname,a.safetycheckname,a.checkdate,d.changepersonname,d.changeperson,d.changedeadine,d.changedutydepartname, e.filepath  from bis_htbaseinfo a
                                            left join base_dataitemdetail  b on a.checktype = b.itemdetailid
                                            left join base_dataitemdetail  c on a.hidrank = c.itemdetailid
                                            left join v_htchangeinfo d on a.hidcode = d.hidcode
                                            left join  (
                                                select a.id,wm_concat((case when b.filepath is not null then b.filepath else '' end)) filepath from bis_htbaseinfo a
                                                left join base_fileinfo b on a.hidphoto = b.recid  group by a. id
                                            ) e on  a.id = e.id 
                    left join bis_district f on a.hidpoint = f.districtcode 
                    where a.workstream ='隐患整改'  and d.changedutydepartcode like '{0}%'  order by d.changedeadine", code);

            var dt = this.BaseRepository().FindTable(sql);

            return dt;
        }

        /// <summary>
        /// 获取区域下未整改的隐患
        /// </summary>
        /// <param name="areaCodes">区域编码</param>
        /// <returns></returns>
        public IList GetCountByArea(List<string> areaCodes)
        {
            var query = BaseRepository().IQueryable(x => areaCodes.Contains(x.HIDPOINT)).GroupBy(x => new { x.HIDPOINT, x.HIDNAME }).Select(x => new
            {
                DistrictName = x.Key.HIDNAME,
                DistrictID = x.Key.HIDPOINT,
                Count = x.Count()
            });
            var data = query.ToList();
            return data;
        }
        #endregion
    }
}
