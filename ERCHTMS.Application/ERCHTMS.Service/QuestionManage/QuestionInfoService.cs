using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Entity.QuestionManage;
using ERCHTMS.IService.QuestionManage;
using ERCHTMS.Code;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.Service.BaseManage;
using System.Text;

namespace ERCHTMS.Service.QuestionManage
{
    /// <summary>
    /// 描 述：问题基本信息表
    /// </summary>
    public class QuestionInfoService : RepositoryFactory<QuestionInfoEntity>, QuestionInfoIService
    {
        private IDepartmentService idepartmentservice = new DepartmentService();
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<QuestionInfoEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public QuestionInfoEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {

            string sql = string.Empty;
            /**********删除所有问题相关的信息************/
            //删除问题整改信息
            sql = string.Format(@" delete bis_questionreform where questionid ='{0}' ", keyValue);
            this.BaseRepository().ExecuteBySql(sql);
            //删除问题验证信息
            sql = string.Format(@" delete bis_questionverify where questionid ='{0}' ", keyValue);
            this.BaseRepository().ExecuteBySql(sql);

            //删除问题基本信息
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, QuestionInfoEntity entity)
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




        #region 问题集合

        /// <summary>
        /// 问题集合
        /// </summary>
        /// <param name="relevanceId"></param>
        /// <returns></returns>
        public DataTable GeQuestiontByRelevanceId(string relevanceId, string flowstate = "问题登记")
        {
            string sql = @"select t.createuserid, t.id,t.reformpeople,t.belongdeptid,t.reformpeoplename from v_questioninfo t where 1=1";
            if (!string.IsNullOrEmpty(relevanceId))
            {
                sql += string.Format(" and t.relevanceId = '{0}'", relevanceId);
            }
            if (!string.IsNullOrEmpty(flowstate))
            {
                sql += string.Format(" and t.flowstate = '{0}'", flowstate);
            }
            return this.BaseRepository().FindTable(sql);
        }
        #endregion


        #region 问题检查集合

        /// <summary>
        /// 问题检查集合
        /// </summary>
        /// <param name="checkId"></param>
        /// <param name="checkman"></param>
        /// <returns></returns>
        public DataTable GeQuestiontOfCheckList(string checkId, string checkman, string flowstate) 
        { 
            string sql = @"select t.id,t.reformpeople,t.belongdeptid,t.reformpeoplename,t.createuserid from v_questioninfo t where 1=1";
            if (!string.IsNullOrEmpty(checkId))
            {
                sql += string.Format(" and t.checkid = '{0}'", checkId);
            }
            if (!string.IsNullOrEmpty(checkman))
            {
                sql += string.Format(" and t.createuserid = '{0}'", checkman);
            }
            if (!string.IsNullOrEmpty(flowstate))
            {
                sql += string.Format(" and t.flowstate = '{0}'", flowstate);
            }
            return this.BaseRepository().FindTable(sql);
        }
        #endregion

        #region 问题实体所有元素对象
        /// <summary>
        /// 问题实体所有元素对象
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public DataTable GetQuestionModel(string keyValue) 
        {
            DataTable dt = new DataTable();
            string sql = string.Format(@"select * from  v_questioninfo where id ='{0}' ", keyValue);
            dt = this.BaseRepository().FindTable(sql);
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

        #region  问题基础信息查询
        /// <summary>
        /// 违章基础信息查询    
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetQuestionBaseInfo(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;

            if (pagination.p_fields.IsEmpty())
            {
                pagination.p_fields = @" belongdeptname,belongdeptid,createuserid,createuserdeptcode,createuserorgcode,createdate,createusername,questionnumber,questionaddress,questiondescribe,
                                         questionpic,checkpersonname,checkpersonid,checkdeptid,checkdeptname,checktype,checktypename,checkname,checkid,to_char(checkdate,'yyyy-MM-dd') checkdate,checkcontent,relevanceid,flowstate,
                                         qflag,appsign,username,deptcode,deptname,reformdeptid,reformdeptcode,reformdeptname,dutydeptid,dutydeptcode,dutydeptname,reformpeople,reformpeoplename,reformtel,
                                         to_char(reformplandate,'yyyy-MM-dd') reformplandate,reformdescribe,reformmeasure,reformstatus,reformstatusname,reformreason,reformsign,to_char(reformfinishdate,'yyyy-MM-dd') reformfinishdate,
                                         reformpic,verifyopinion,verifyresult, verifyresultname,verifypeople,verifypeoplename,verifysign,verifydeptid,verifydeptcode,
                                         verifydeptname,to_char(verifydate,'yyyy-MM-dd') verifydate,questionfilepath,reformfilepath,actionperson,participantname,closedloop";
            }
            else 
            {
                pagination.p_fields = pagination.p_fields.Replace("checkdate", "to_char(checkdate,'yyyy-MM-dd') checkdate").
                        Replace("reformplandate", "to_char(reformplandate,'yyyy-MM-dd') reformplandate").
                        Replace("reformfinishdate", "to_char(reformfinishdate,'yyyy-MM-dd') reformfinishdate").
                        Replace("verifydate", "to_char(verifydate,'yyyy-MM-dd') verifydate");
            }
            pagination.p_kid = "id";

            pagination.conditionJson = " 1=1";

            var queryParam = queryJson.ToJObject();

            //当前用户
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            pagination.p_tablename = @"v_questioninfo";

            //台账标记
            if (!queryParam["standingmark"].IsEmpty())
            {
                pagination.conditionJson += @" and flowstate  != '问题登记'";
            }
            //组织机构
            if (!string.IsNullOrEmpty(user.OrganizeCode))
            {
                //省级单位
                if (user.RoleName.Contains("省级用户"))
                {
                    pagination.conditionJson += string.Format(@" and  newdeptcode  like '{0}%' ", user.NewDeptCode);
                }
                else   //厂级
                {
                    pagination.conditionJson += string.Format(@" and  belongdeptid = '{0}' ", user.OrganizeId);
                }
            }

            string queryDeptOrgCode = "createuserorgcode";  //1为按照创建单位  0 为按照整改单位 
            string queryDeptCode = "deptcode";
            string choosetag = string.Empty;
            string action = string.Empty;
            int querybtntype = 1;

            if (!queryParam["querybtntype"].IsEmpty())
            {
                querybtntype = int.Parse(queryParam["querybtntype"].ToString());

                //按创建单位来
                if (querybtntype > 0)
                {
                    queryDeptOrgCode = "createuserorgcode";
                    queryDeptCode = "deptcode";

                    choosetag = queryParam["choosetag"].ToString();
                }
                else //按整改单位来
                {
                    queryDeptOrgCode = "createuserorgcode";
                    queryDeptCode = "reformdeptcode";
                }
            }

            //查询条件
            if (!queryParam["action"].IsEmpty())
            {
                action = queryParam["action"].ToString();

                switch (action)
                {
                    //问题登记
                    case "Register":
                        pagination.conditionJson += string.Format(@" and  createuserid ='{0}' ", user.UserId);
                        break;
                    //问题整改
                    case "Reform":
                        pagination.conditionJson += @" and flowstate  = '问题整改'";
                        break;
                    //问题验证
                    case "Verify":
                        pagination.conditionJson += @" and  flowstate  = '问题验证'";
                        break;
                    //流程结束
                    case "BaseEnd":
                        pagination.conditionJson += @" and flowstate  = '流程结束'";
                        break;
                }
            }
            //违章状态
            if (!queryParam["reformstatus"].IsEmpty())
            {
                switch (queryParam["reformstatus"].ToString())
                {
                    case "未整改":
                        pagination.conditionJson += @" and flowstate = '问题整改' ";
                        break;
                    case "逾期未整改":
                        pagination.conditionJson += string.Format(@" and flowstate = '问题整改'  and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') > reformplandate + 1", DateTime.Now);
                        break;
                    case "即将到期未整改":
                        pagination.conditionJson += @"and flowstate = '问题整改' and (reformplandate - 3 <= sysdate  and sysdate <= reformplandate + 1)";
                        break;
                    case "本人登记":
                        pagination.conditionJson += string.Format(@" and  createuserid ='{0}'", user.UserId);
                        break;
                    case "已整改":
                        pagination.conditionJson += @" and  flowstate in ('问题验证','流程结束')";
                        break;
                    case "未闭环":
                        pagination.conditionJson += @" and  flowstate in ('问题整改','问题验证')";
                        break;
                    case "已闭环":
                        pagination.conditionJson += @" and  flowstate  = '流程结束'";
                        break;
                }
            }
            //数据范围
            if (!queryParam["datascope"].IsEmpty())
            {
                switch (queryParam["datascope"].ToString())
                {
                    case "本人登记":
                        pagination.conditionJson += string.Format(@" and  createuserid ='{0}'", user.UserId);
                        break;
                    case "待本人整改":
                        pagination.conditionJson += string.Format(@" and  actionperson  like  '%{0}%' and flowstate = '问题整改'", user.Account + ",");
                        break;
                    case "本部门整改":
                        pagination.conditionJson += string.Format(@" and reformdeptid ='{0}' and flowstate = '问题整改'", user.DeptId);
                        break;
                    case "待本人验证":
                        pagination.conditionJson += string.Format(@" and  actionperson  like  '%{0}%' and flowstate='问题验证'", user.Account + ",");
                        break;
                    case "本部门验证":
                        pagination.conditionJson += string.Format(@" and instr(verifydeptid,'{0}')>=0  and flowstate='问题验证'", user.DeptId);
                        break;
                    case "本人已验证":
                        pagination.conditionJson += string.Format(@" and ','||verifypeople||','  like  '%{0}%' and  flowstate='流程结束'", "," + user.Account + ",");
                        break;
                }
            }
            //数据范围
            if (!queryParam["findtype"].IsEmpty())
            {
                switch (queryParam["findtype"].ToString())
                {
                    case "本周":
                        pagination.conditionJson += string.Format(@" and  to_char(checkdate,'iW') = to_char(to_date('{0}','yyyy-mm-dd hh24:mi:ss'),'iW')", DateTime.Now.ToString("yyyy-MM-dd"));
                        break;
                    case "本月":
                        pagination.conditionJson += string.Format(@" and  to_char(checkdate,'yyyy-MM') = to_char(to_date('{0}','yyyy-mm-dd hh24:mi:ss'),'yyyy-MM') ", DateTime.Now.ToString("yyyy-MM-dd"));
                        break;
                }
            }
            //整改部门编码
            if (!queryParam["qdeptcode"].IsEmpty())
            {
                //来自于统计
                pagination.conditionJson += string.Format(@" and reformdeptcode like '{0}%'", queryParam["qdeptcode"].ToString());
            }
            //台账类型
            if (!queryParam["standingtype"].IsEmpty())
            {
                string standingtype = queryParam["standingtype"].ToString();

                pagination.conditionJson += @" and flowstate != '问题登记'";

                if (standingtype.Contains("公司级"))
                {
                    pagination.conditionJson += @" and  (rolename  like  '%公司级%' or   rolename  like  '%厂级部门用户%') ";
                }
                else
                {
                    pagination.conditionJson += string.Format(@" and rolename  like  '%{0}%'  and  rolename not like '%厂级%' ", standingtype);
                }
            }

            //流程状态
            if (!queryParam["flowstate"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and flowstate = '{0}'", queryParam["flowstate"].ToString());
            }

            //问题描述 
            if (!queryParam["questiondescribe"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and questiondescribe like '%{0}%'", queryParam["questiondescribe"].ToString());
            }
            //所属单位 
            if (!queryParam["belongdeptid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and belongdeptid = '{0}'", queryParam["belongdeptid"].ToString());
            }
            //问题时间开始时间
            if (!queryParam["starttime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and createdate >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", queryParam["starttime"].ToString());
            }
            //违章时间结束时间
            if (!queryParam["endtime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and createdate < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(queryParam["endtime"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
            }
            //创建年度
            if (!queryParam["qyear"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and to_char(createdate,'yyyy') = '{0}'", queryParam["qyear"].ToString());
            }
            //创建年月
            if (!queryParam["qyearmonth"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and to_char(createdate,'yyyy-MM') = '{0}'", queryParam["qyearmonth"].ToString());
            }

            //关联id 1
            if (!queryParam["relevanceid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and relevanceid = '{0}'", queryParam["relevanceid"].ToString());
            }
            //关联id 2
            if (!queryParam["correlationid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and correlationid = '{0}'", queryParam["correlationid"].ToString());
            }
            //安全检查id关联
            if (!queryParam["checkid"].IsEmpty())
            {
                string ckId = queryParam["checkid"].ToString();
                string ids = GetCheckIds(ckId);
                if (!queryParam["pfrom"].IsEmpty())
                {
                    string pfrom = queryParam["pfrom"].ToString();
                    if (pfrom == "0")
                    {
                        pagination.conditionJson += string.Format(@" and (checkid in('{1}') or checkid='{0}')", ckId,ids.Replace(",","','"));
                    }
                    if (pfrom == "1")
                    {
                        string dutydept = BaseRepository().FindObject(string.Format("select dutydept from bis_saftycheckdatarecord where id='{0}'", ckId)).ToString();
                        pagination.conditionJson += string.Format(@" and (checkid in('{1}') or checkid='{0}')", ckId, ckId, ids.Replace(",", "','"));
                    }
                }
                else
                {
                    pagination.conditionJson += string.Format(@" and checkid = '{0}'", ckId);
                }

               
            }
            if (!queryParam["isOrg"].IsEmpty())
            {
                string isOrg = queryParam["isOrg"].ToString();
                if (!queryParam["code"].IsEmpty())
                {
                    string code = queryParam["code"].ToString();
                    //本单位
                    if (choosetag == "0")
                    {
                        //非省级用户,只能看自己电厂的
                        pagination.conditionJson += string.Format(@" and {0} = '{1}' and  belongdeptid = '{2}'", queryDeptCode, code, user.OrganizeId);
                    }
                    else
                    {
                        //非省级用户,只能看自己电厂的
                        pagination.conditionJson += string.Format(@" and {0} like '{1}%' and  belongdeptid = '{2}'", queryDeptCode, code, user.OrganizeId);
                    }
                }
            }
          
            var dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);

            return dt;
        }
        #endregion

        #region 通过问题编号，来判断是否存在重复现象
        /// <summary>
        /// 通过问题编号，来判断是否存在重复现象
        /// </summary>
        /// <param name="QuestionNumber"></param>
        /// <returns></returns>
        public IList<QuestionInfoEntity> GetListByNumber(string QuestionNumber)
        {
            return this.BaseRepository().IQueryable().ToList().Where(p => p.QUESTIONNUMBER == QuestionNumber).ToList();
        }
        #endregion

        #region 获取新编码
        /// <summary>
        /// 获取新编码
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="maxfields"></param>
        /// <param name="seriallen">4</param>
        /// <returns></returns>
        public string GenerateCode(string tablename, string maxfields, int seriallen)
        {
            string code = "";
            string newCode = "";
            try
            {
                string startCode = "AQ" + DateTime.Now.Year + "第" + DateTime.Now.Month + "期";
                string sql = string.Format("select max(cast(substr({0},length({0})-({2}-1),{2}) as number)) from {1} t where {0} like '{3}%'", maxfields, tablename, seriallen, startCode);
                object obj = this.BaseRepository().FindObject(sql);
                string num = obj == null || obj == DBNull.Value ? "1" : (int.Parse(obj.ToString()) + 1).ToString();
                string str = "";
                //最大值小于流水号长度
                if (num.Length < seriallen)
                {
                    for (int j = 0; j < seriallen - num.Length; j++)
                    {
                        str += "0";
                    }
                }
                code = str + num;
                newCode = startCode + code;
            }
            catch (Exception)
            {
                throw;
            }
            return newCode;
        }
        #endregion
    }
}