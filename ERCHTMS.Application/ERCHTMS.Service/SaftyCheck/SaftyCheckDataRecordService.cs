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
using System;
using ERCHTMS.Code;
using System.Text;
using ERCHTMS.Service.SystemManage;
using System.Data.Common;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.Service.BaseManage;

namespace ERCHTMS.Service.SaftyCheck
{
    /// <summary>
    /// 描 述：安全检查记录
    /// </summary>
    public class SaftyCheckDataRecordService : RepositoryFactory<SaftyCheckDataRecordEntity>, SaftyCheckDataRecordIService
    {
        private IUserInfoService userservice = new UserInfoService();

        #region 获取数据
        /// <summary>
        /// 在隐患登记中选择检查记录进行关联
        /// </summary>
        /// <param name="recId">安全检查记录Id</param>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GetRecordFromHT(string recId, Operator user)
        {
            int count = BaseRepository().FindObject(string.Format("select count(1) from BIS_SAFTYCHECKDATADETAILED where recid='{0}'", recId)).ToInt();
            if (count > 0)
            {
                string newId = "";
                object obj = BaseRepository().FindObject(string.Format("select id from BIS_SAFTYCHECKDATADETAILED where recid='{0}' and (checkcontent='{1}' or checkcontent='符合安全管理要求') and checkobject='{1}'", recId, "其他"));
                if (obj != null)
                {
                    DataTable dtItem = BaseRepository().FindTable(string.Format("select a.id,checkmanaccount,detailid from BIS_SAFTYCONTENT a left join BIS_SAFTYCHECKDATADETAILED b on a.detailid=b.id where a.recid='{0}' and (checkcontent='{1}' or checkcontent='符合安全管理要求') and a.checkobject='{1}'", recId, "其他"));
                    if (dtItem.Rows.Count == 0)
                    {
                        newId = obj.ToString();
                        BaseRepository().ExecuteBySql(string.Format(@"insert into BIS_SAFTYCONTENT(id,createuserid,createdate,createuserdeptcode,createuserorgcode,recid,checkmanname,checkmanaccount,detailid,checkobject,checkobjectid,checkobjecttype,issure,createusername) 
values('{0}','{1}',to_date('{2}','yyyy-mm-dd hh24:mi:ss'),'{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','3','0','{11}')", Guid.NewGuid().ToString(), user.UserId, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), user.DeptCode, user.OrganizeCode, recId, user.UserName, user.Account, newId, "其他", newId, user.UserName));
                    }
                    else
                    {
                        string accounts = dtItem.Rows[0]["checkmanaccount"].ToString();
                        if (!("," + accounts + ",").Contains("," + user.Account + ","))
                        {
                            string sql = string.Format("update BIS_SAFTYCONTENT set checkmanname=checkmanname || ',{0}',checkmanaccount= checkmanaccount || ',{1}' where id='{2}'", user.UserName, user.Account, dtItem.Rows[0][0].ToString());
                            BaseRepository().ExecuteBySql(sql);

                        }
                        newId = dtItem.Rows[0]["detailid"].ToString();
                    }
                }
                else
                {
                    newId = Guid.NewGuid().ToString();
                    if (BaseRepository().ExecuteBySql(string.Format(@"insert into bis_saftycheckdatadetailed(id,checkcontent,belongdeptid,belongdept,recid,checkman,checkmanid,checkobject,checkobjectid,checkobjecttype,issure) 
values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','0')", newId, "符合安全管理要求", user.DeptId, user.DeptCode, recId, user.UserName, user.Account, "其他", newId, 3)) > 0)
                    {
                        BaseRepository().ExecuteBySql(string.Format(@"insert into BIS_SAFTYCONTENT(id,createuserid,createdate,createuserdeptcode,createuserorgcode,recid,checkmanname,checkmanaccount,detailid,checkobject,checkobjectid,checkobjecttype,issure,createusername) 
values('{0}','{1}',to_date('{2}','yyyy-mm-dd hh24:mi:ss'),'{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','3','0','{11}')", Guid.NewGuid().ToString(), user.UserId, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), user.DeptCode, user.OrganizeCode, recId, user.UserName, user.Account, newId, "其他", newId, user.UserName));
                    }
                }
                return newId;
            }
            else
            {
                return recId;
            }

        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<SaftyCheckDataRecordEntity> GetPageList(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;

            var user = OperatorProvider.Provider.Current();

            var queryParam = queryJson.ToJObject();

            //时间选择
            if (!queryParam["st"].IsEmpty())
            {
                string st = queryParam["st"].ToString();
                string et = queryParam["et"].ToString();
                pagination.conditionJson += string.Format(" and t.CHECKBEGINTIME between to_date('{0}','yyyy-mm-dd') and to_date('{1}','yyyy-mm-dd')", st, et);
            }
            if (!queryParam["stm"].IsEmpty())
            {
                string st = queryParam["stm"].ToString();
                string et = queryParam["etm"].ToString();
                pagination.conditionJson += string.Format(" and t.CHECKBEGINTIME>=to_date('{0}','yyyy-mm-dd') and CHECKENDTIME<=to_date('{1}','yyyy-mm-dd')", st, et);
            }
            if (!queryParam["curym"].IsEmpty())
            {
                string stm = queryParam["curym"].ToString().Replace('.', '-') + "-01";
                string etm = Convert.ToDateTime(stm).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                pagination.conditionJson += string.Format(" and t.CHECKBEGINTIME>=to_date('{0}','yyyy-mm-dd') and CHECKENDTIME<=to_date('{1}','yyyy-mm-dd')", stm, etm);
            }
            //检查名称
            if (!queryParam["keyword"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.CheckDataRecordName like '%{0}%'", queryParam["keyword"].ToString());
            }
            //安全检查类型
            if (!queryParam["ctype"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.checkdatatype='{0}'", queryParam["ctype"].ToString());
            }
            //所属部门
            if (!queryParam["code"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.belongdeptid  in (select departmentid from base_department start with encode='{0}' connect by  prior departmentid = parentid union select ORGANIZEID from BASE_ORGANIZE where encode like '{0}%')", queryParam["code"].ToString());
            }
            //检查单位
            if (!queryParam["qdeptcode"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and (t.belongdept like '{0}%' or ',' || t.checkdeptid like '%,{0}%' or ',' || t.checkdeptcode like '%,{0}%')", queryParam["qdeptcode"].ToString());
            }
            //检查单位
            if (!queryParam["qyearmonth"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and  to_char(t.checkbegintime,'yyyy-MM')='{0}'", queryParam["qyearmonth"].ToString());
            }
            //检查单位
            if (!queryParam["qyear"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and  to_char(t.checkbegintime,'yyyy')='{0}' ", queryParam["qyear"].ToString());
            }
            //mode
            if (!queryParam["dataType"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.datatype='{0}'", queryParam["dataType"].ToString());
            }
            //安全指标内容
            if (!queryParam["querytype"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and to_char(t.createdate,'yyyy')='{0}' and  createuserorgcode ='{1}'", DateTime.Now.Year.ToString(), user.OrganizeCode);
            }
            if (!queryParam["startDate"].IsEmpty() && !queryParam["endDate"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and createdate between to_date('{0}','yyyy-mm-dd') and to_date('{1}','yyyy-mm-dd hh24:mi:ss')", queryParam["startDate"], queryParam["endDate"]);
            }
            if (!queryParam["pfrom"].IsEmpty())
            {
                if (queryParam["pfrom"].ToString() == "1")
                {
                    pagination.conditionJson += string.Format(" and  createuserorgcode <>'{0}'", user.OrganizeCode);
                }
            }
            IEnumerable<SaftyCheckDataRecordEntity> list = this.BaseRepository().FindListByProcPager(pagination, dataType).Select(r =>
            {
                //隐患数量
                int count = this.BaseRepository().FindObject("select count(1) from bis_htbaseinfo o where  o.safetycheckobjectid='" + r.ID + "'").ToInt();
                r.Count = count;
                //违章数量
                count = this.BaseRepository().FindObject("select count(1) from BIS_LLLEGALREGISTER o where  o.reseverone='" + r.ID + "'").ToInt();
                r.WzCount = count;
                //问题数量
                count = this.BaseRepository().FindObject("select count(1) from BIS_QUESTIONINFO o where  o.CHECKID='" + r.ID + "'").ToInt();
                r.WtCount = count;

                decimal count1 = this.BaseRepository().FindObject("select count(1) from bis_htbaseinfo o where workstream='整改结束' and  o.safetycheckobjectid='" + r.ID + "'").ToInt();
                if (r.Count>0)
                {
                    count1 = Math.Round(decimal.Parse((count1 / r.Count).ToString()), 2);
                    r.Count1 = count1*100;
                }
                else
                {
                    r.Count1 = 0;

                }
                count1 = this.BaseRepository().FindObject("select count(1) from BIS_LLLEGALREGISTER o where flowstate='流程结束' and  o.reseverone='" + r.ID + "'").ToInt();
                if (r.WzCount > 0)
                {
                    count1 = Math.Round(decimal.Parse((count1 / r.WzCount).ToString()), 2);
                    r.WzCount1 = count1*100;
                }
                else
                {
                    r.WzCount1 = 0;
                }
                count1 = this.BaseRepository().FindObject("select count(1) from BIS_QUESTIONINFO o where flowstate='流程结束' and  o.CHECKID='" + r.ID + "'").ToInt();
                if (r.WtCount > 0)
                {
                    count1 = Math.Round(decimal.Parse((count1 / r.WtCount).ToString()), 2);
                    r.WtCount1 = count1*100;
                }
                else
                {
                    r.WtCount1 = 0;
                }
                return r;
            });
            return list;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageListJsonByTz(Pagination pagination)
        {
            DatabaseType dataType = DbHelper.DbType;
            //IEnumerable<SaftyCheckDataRecordEntity> list = this.BaseRepository().FindListByProcPager(pagination, dataType).Select(r =>
            //{
            //    DataTable dtc = this.BaseRepository().FindTable(string.Format(@"select id from bis_htbaseinfo o where o.safetycheckobjectid='{0}' and o.deviceid='{1}'", r.ID, keyValue));
            //    r.Count = dtc.Rows.Count;
            //    return r;
            //});

            //return list;
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        ///获取统计表格数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">年份</param>
        /// <param name="belongdistrictcode">区域</param>
        /// <param name="ctype">检查类型</param>
        /// <returns></returns>
        public string GetSaftyList(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "")
        {
            string whereSQL = " datatype in(0,2)";
            string roleName = OperatorProvider.Provider.Current().RoleName;
            var user = OperatorProvider.Provider.Current();

            //单位
            if (!String.IsNullOrEmpty(deptCode))
            {
                whereSQL += " and  (',' || CheckDeptCode like '%," + deptCode + "%' or ',' || CHECKDEPTID like '%," + deptCode + "%')";
            }
            else
            {
                if (roleName.Contains("厂级部门用户") || roleName.Contains("公司级用户"))
                {
                    whereSQL += string.Format(" and ((( ',' || CheckDeptCode like '%,{0}%' or ',' || CHECKDEPTID like '%,{0}%')) or (datatype=2 and checkeddepartid like '%{1}%'))", user.OrganizeCode, user.OrganizeId);
                }
                else
                {
                    whereSQL += string.Format(" and (belongdept like '{0}%' or  ',' || checkdeptid || ',' like '%,{0}%' or ',' || checkdeptcode || ',' like '%,{0}%') ", user.DeptCode);
                    //whereSQL += string.Format(" and ((createuserdeptcode like '{0}%') or (((',' || checkdeptid || ',') like '%,{0}%') or (datatype=0 and id in(select recid from BIS_SAFETYCHECKDEPT where deptcode in(select encode from base_department d where d.encode like '{0}%' )))))", user.DeptCode);
                    //whereSQL += string.Format(" and checkeddepartid is null and (CheckDeptCode like '{0}%' or CHECKDEPTID like '{0}%')", user.DeptCode);
                }
            }
            //年限
            whereSQL += " and to_char(t.checkbegintime,'yyyy')='" + year + "'";
            //区域类型
            if (!String.IsNullOrEmpty(belongdistrictcode))
            {
                whereSQL += " and  t.belongdeptid  in (select departmentid from base_department where encode like '" + belongdistrictcode + "%')";
            }
            List<SaftyCheckCountEntity> list = new List<SaftyCheckCountEntity>();
            for (int i = 1; i <= 12; i++)
            {
                string whereSQL2 = " and to_char(t.checkbegintime,'mm')=" + i.ToString();
                string forsql = @"select nvl(b.num,0) num,a.itemvalue from (
select t.itemvalue,count(1) as num from BASE_DATAITEMDETAIL t where t.itemid =(select itemid from base_dataitem where itemcode='SaftyCheckType') group by itemvalue
) a
left join(
select to_char(CHECKDATATYPE) as itemvalue, count(1) as num from BIS_SAFTYCHECKDATARECORD t where " + whereSQL + whereSQL2 + " group by CHECKDATATYPE)  b on a.itemvalue = b.itemvalue order by  a.itemvalue";
                DataTable cou = this.BaseRepository().FindTable(forsql);
                int rc = 0, zx = 0, jj = 0, jjr = 0, zh = 0;
                for (int j = 0; j < cou.Rows.Count; j++)
                {
                    var checktype = cou.Rows[j][1].ToString();
                    var checknum = Convert.ToInt32(cou.Rows[j][0].ToString());
                    switch (checktype)
                    {
                        case "1":
                            rc = checknum;
                            break;
                        case "2":
                            zx = checknum;
                            break;
                        case "3":
                            jjr = checknum;
                            break;
                        case "4":
                            jj = checknum;
                            break;
                        case "5":
                            zh = checknum;
                            break;
                        default:
                            break;
                    }
                }
                string wheresj = " to_char(checkbegintime,'yyyy')='" + year + "' and to_char(t.checkbegintime,'mm')=" + i.ToString() + " and checkeddepartid like '%" + user.OrganizeId + "%' and (issynview='1' or (issynview='0' and checkbegintime<=to_date('" + DateTime.Now.ToString("yyyy-MM-dd") + "','yyyy-MM-dd')))";
                var cousj = this.BaseRepository().FindObject("select count(1) from BIS_SAFTYCHECKDATARECORD t where " + wheresj).ToInt();
                list.Add(new SaftyCheckCountEntity
                {
                    month = i.ToString() + "月",
                    rc = rc,
                    zx = zx,
                    jj = jj,
                    jjr = jjr,
                    zh = zh,
                    sj = cousj,
                    sum = Convert.ToInt32(cou.Rows[0][0].ToString()) + Convert.ToInt32(cou.Rows[1][0].ToString()) + Convert.ToInt32(cou.Rows[2][0].ToString()) + Convert.ToInt32(cou.Rows[3][0].ToString()) + Convert.ToInt32(cou.Rows[4][0].ToString()) + cousj
                });
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                total = 1,
                page = 1,
                records = list.Count,
                rows = list
            });
        }
        /// <summary>
        /// 省公司安全检查统计
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="year"></param>
        /// <param name="belongdistrictcode"></param>
        /// <param name="ctype"></param>
        /// <returns></returns>
        public string GetGrpSaftyList(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "")
        {
            var user = OperatorProvider.Provider.Current();
            string whereSQL = "datatype in(0,2)";
            if (ctype != "省公司")
            {
                whereSQL += " and t.createuserorgcode<>'" + user.OrganizeCode + "'";
            }
            whereSQL += string.Format(" and ((createuserorgcode in (select encode from BASE_DEPARTMENT  where (nature='厂级' or nature='电厂') start with encode='{0}' connect by  prior  departmentid = parentid))", deptCode);
            whereSQL += string.Format(" or (checkeddepartid in (select departmentid from BASE_DEPARTMENT  where (nature='厂级' or nature='电厂') start with encode='{0}' connect by  prior  departmentid = parentid)))", deptCode);

            //whereSQL += string.Format(" and belongdept in (select encode from BASE_DEPARTMENT  where (nature='厂级' or nature='电厂') start with encode='{0}' connect by  prior  departmentid = parentid)", deptCode);
            //年限
            whereSQL += " and to_char(t.checkbegintime,'yyyy')='" + year + "'";
            //区域类型
            if (!String.IsNullOrEmpty(belongdistrictcode))
            {
                whereSQL += " and  t.belongdeptid  in (select departmentid from base_department where encode like '" + belongdistrictcode + "%')";
            }
            List<SaftyCheckCountEntity> list = new List<SaftyCheckCountEntity>();
            for (int i = 1; i <= 12; i++)
            {
                string whereSQL2 = " and to_char(t.checkbegintime,'mm')=" + i.ToString();
                string forsql = @"select nvl(b.num,0) num,a.itemvalue from (
select t.itemvalue,count(1) as num from BASE_DATAITEMDETAIL t where t.itemid =(select itemid from base_dataitem where itemcode='SaftyCheckType') group by itemvalue
) a
left join(
select to_char(CHECKDATATYPE) as itemvalue, count(1) as num from BIS_SAFTYCHECKDATARECORD t where " + whereSQL + whereSQL2 + " group by CHECKDATATYPE)  b on a.itemvalue = b.itemvalue order by  a.itemvalue";
                DataTable cou = this.BaseRepository().FindTable(forsql);
                int rc = 0, zx = 0, jj = 0, jjr = 0, zh = 0, sj = 0;
                if (ctype != "省公司")
                {
                    for (int j = 0; j < cou.Rows.Count; j++)
                    {
                        var checktype = cou.Rows[j][1].ToString();
                        var checknum = Convert.ToInt32(cou.Rows[j][0].ToString());
                        switch (checktype)
                        {
                            case "1":
                                rc = checknum;
                                break;
                            case "2":
                                zx = checknum;
                                break;
                            case "3":
                                jjr = checknum;
                                break;
                            case "4":
                                jj = checknum;
                                break;
                            case "5":
                                zh = checknum;
                                break;
                            default:
                                break;
                        }
                    }
                }
                if (ctype != "电厂")
                {
                    string wheresj = " datatype=0 and to_char(checkbegintime,'yyyy')='" + year + "' and to_char(t.checkbegintime,'mm')=" + i.ToString() + " and checkeddepartid in(select departmentid from base_department where (nature='电厂' or nature='厂级') start with encode='" + deptCode + "' connect by  prior  departmentid = parentid)";
                    sj = this.BaseRepository().FindObject("select count(1) from BIS_SAFTYCHECKDATARECORD t where " + wheresj).ToInt();
                }
                list.Add(new SaftyCheckCountEntity
                {
                    month = i.ToString() + "月",
                    rc = rc,
                    zx = zx,
                    jj = jj,
                    jjr = jjr,
                    zh = zh,
                    sj = sj,
                    sum = rc + zx + jjr + jj + zh + sj
                });
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                total = 1,
                page = 1,
                records = list.Count,
                rows = list
            });
        }
        public List<SaftyCheckCountEntity> GetSaftyList(string deptCode, string year, string ctype)
        {
            string whereSQL = " datatype in(0,2)";
            string roleName = OperatorProvider.Provider.Current().RoleName;
            var user = OperatorProvider.Provider.Current();
            //单位
            if (!String.IsNullOrEmpty(deptCode))
            {
                whereSQL += "  and (',' || CheckDeptCode like '%," + deptCode + "%' or ',' ||  CHECKDEPTID like '%," + deptCode + "%' or belongdept like '{0}%')";
            }
            else
            {
                if (roleName.Contains("厂级部门用户") || roleName.Contains("公司级用户"))
                {
                    whereSQL += string.Format(" and (datatype in(0,2) and (',' || CheckDeptCode like '%,{0}%' or ',' || CHECKDEPTID like '{0}%') or (datatype=2 and checkeddepartid like '%{1}%'))", user.OrganizeCode, user.OrganizeId);
                }
                else
                {
                    whereSQL += string.Format(" and (datatype in(0,2) and (',' || CheckDeptCode like '%,{0}%' or ',' || CHECKDEPTID like '%,{0}%')", user.DeptCode);
                }
            }
            //年限
            whereSQL += " and to_char(t.checkbegintime,'yyyy')='" + year + "'";

            List<SaftyCheckCountEntity> list = new List<SaftyCheckCountEntity>();
            for (int i = 1; i <= 12; i++)
            {
                string whereSQL2 = " and to_char(t.checkbegintime,'mm')=" + i.ToString();
                string forsql = @"select nvl(b.num,0) num,a.itemvalue from (
select t.itemvalue,count(1) as num from BASE_DATAITEMDETAIL t where t.itemid =(select itemid from base_dataitem where itemcode='SaftyCheckType') group by itemvalue
) a
left join(
select to_char(CHECKDATATYPE) as itemvalue, count(1) as num from BIS_SAFTYCHECKDATARECORD t where " + whereSQL + whereSQL2 + " group by CHECKDATATYPE)  b on a.itemvalue = b.itemvalue order by  a.itemvalue";
                DataTable cou = this.BaseRepository().FindTable(forsql);
                int rc = 0, zx = 0, jj = 0, jjr = 0, zh = 0;
                for (int j = 0; j < cou.Rows.Count; j++)
                {
                    var checktype = cou.Rows[j][1].ToString();
                    var checknum = Convert.ToInt32(cou.Rows[j][0].ToString());
                    switch (checktype)
                    {
                        case "1":
                            rc = checknum;
                            break;
                        case "2":
                            zx = checknum;
                            break;
                        case "3":
                            jjr = checknum;
                            break;
                        case "4":
                            jj = checknum;
                            break;
                        case "5":
                            zh = checknum;
                            break;
                        default:
                            break;
                    }
                }
                list.Add(new SaftyCheckCountEntity
                {
                    month = i.ToString() + "月",
                    rc = rc,
                    zx = zx,
                    jj = jj,
                    jjr = jjr,
                    zh = zh,
                    sum = Convert.ToInt32(cou.Rows[0][0].ToString()) + Convert.ToInt32(cou.Rows[1][0].ToString()) + Convert.ToInt32(cou.Rows[2][0].ToString()) + Convert.ToInt32(cou.Rows[3][0].ToString()) + Convert.ToInt32(cou.Rows[4][0].ToString())
                });
            }
            return list;
        }
        /// <summary>
        ///获取统计表格数据(对比)
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">年份</param>
        /// <param name="belongdistrictcode">区域</param>
        /// <param name="ctype">检查类型</param>
        /// <returns></returns>
        public string GetSaftyListDB(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "")
        {
            string sql = "";
            //判断用户觉得是不是厂级或者公司级用户,查看所有数据
            string roleName = OperatorProvider.Provider.Current().RoleName;
            var user = OperatorProvider.Provider.Current();
            string owndeptcode = user.DeptCode;
            if (String.IsNullOrEmpty(deptCode))
            {
                if (roleName.Contains("厂级部门用户") || roleName.Contains("公司级用户"))
                {
                    sql = string.Format("select  encode,fullname from BASE_DEPARTMENT  where encode like '{0}___' ", user.OrganizeCode);
                }
                else if (roleName.Contains("省级用户") || roleName.Contains("集团用户"))
                {
                    sql = string.Format("select  encode,fullname from BASE_DEPARTMENT  where nature='厂级' and deptcode like '{0}%'", user.NewDeptCode);
                }
                else
                {
                    sql = string.Format("select  encode,fullname from BASE_DEPARTMENT  where (encode='{0}' or  encode like '{0}___' )", user.DeptCode);
                }
            }
            else
            {
                sql = string.Format("select  encode,fullname from BASE_DEPARTMENT  where (encode='{0}' or  encode like '{0}___' )", deptCode);
            }
            //检查表类型
            ctype = "日常安全检查,专项安全检查,季节性安全检查,节假日前后安全检查,综合安全检查";

            sql += " order by SortCode asc";
            DataTable dtDepts = this.BaseRepository().FindTable(sql);
            string whereSQL = " 1=1";
            //单位

            //年限
            whereSQL += " and  to_char(t.checkbegintime,'yyyy')='" + year + "'";
            //区域类型
            if (!String.IsNullOrEmpty(belongdistrictcode))
            {
                whereSQL += " and  t.belongdeptid  in (select departmentid from base_department where encode like '" + belongdistrictcode + "%')";
            }
            List<SaftyCheckCountEntity> list = new List<SaftyCheckCountEntity>();
            foreach (DataRow item in dtDepts.Rows)
            {
                var where = string.Format(" and datatype in(0,2) and (',' || checkdeptcode like '%,{0}%' or  ',' || CHECKDEPTID like '%,{0}%' or belongdept like '{0}%')", item[0].ToString());
                //if ((deptCode == item[0].ToString() && deptCode.Length == 6 && deptCode == owndeptcode) || (owndeptcode == item[0].ToString() && owndeptcode.Length == 6 && deptCode == ""))
                //{
                //    where = string.Format(" and datatype in(0,2) and (',' || checkdeptcode like '%,{0}' or  ',' || CHECKDEPTID like '%,{0}')", item[0].ToString());
                //}
                //else
                //{
                //    where = string.Format(" and datatype in(0,2) and (',' ||  CheckDeptCode like '%,{0}%' or ','|| CHECKDEPTID like '%,{0}%')", item[0].ToString());
                //}
                string forsql = @"select nvl(b.num,0) num,a.itemvalue from (
select t.itemvalue,count(1) as num from BASE_DATAITEMDETAIL t where t.itemid =(select itemid from base_dataitem where itemcode='SaftyCheckType')  group by itemvalue
) a
left join(
select to_char(CHECKDATATYPE) as itemvalue, count(1) as num from BIS_SAFTYCHECKDATARECORD t where " + whereSQL + where + " group by CHECKDATATYPE)  b on a.itemvalue = b.itemvalue order by  a.itemvalue";
                DataTable cou = this.BaseRepository().FindTable(forsql);
                int rc = 0, zx = 0, jj = 0, jjr = 0, zh = 0;
                for (int i = 0; i < cou.Rows.Count; i++)
                {
                    var checktype = cou.Rows[i][1].ToString();
                    var checknum = Convert.ToInt32(cou.Rows[i][0].ToString());
                    switch (checktype)
                    {
                        case "1":
                            rc = checknum;
                            break;
                        case "2":
                            zx = checknum;
                            break;
                        case "3":
                            jjr = checknum;
                            break;
                        case "4":
                            jj = checknum;
                            break;
                        case "5":
                            zh = checknum;
                            break;
                        default:
                            break;
                    }
                }
                list.Add(new SaftyCheckCountEntity
                {
                    month = item[1].ToString(),
                    deptcode = item[0].ToString(),
                    rc = rc,
                    zx = zx,
                    jjr = jjr,
                    jj = jj,
                    zh = zh,
                    sum = Convert.ToInt32(cou.Rows[0][0].ToString()) + Convert.ToInt32(cou.Rows[1][0].ToString()) + Convert.ToInt32(cou.Rows[2][0].ToString()) + Convert.ToInt32(cou.Rows[3][0].ToString()) + Convert.ToInt32(cou.Rows[4][0].ToString())
                });
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                total = 1,
                page = 1,
                records = list.Count,
                rows = list
            });
        }
        /// <summary>
        /// 省公司安全检查统计
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="year"></param>
        /// <param name="belongdistrictcode"></param>
        /// <param name="ctype"></param>
        /// <returns></returns>
        public string GetGrpSaftyListDB(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "")
        {
            string sql = "";
            //判断用户觉得是不是厂级或者公司级用户,查看所有数据       
            var user = OperatorProvider.Provider.Current();
            //string owndeptcode = user.OrganizeCode;
            sql = string.Format("select  encode,fullname,departmentid from BASE_DEPARTMENT  where (nature='厂级' or nature='电厂') start with encode='{0}' connect by  prior  departmentid = parentid order by SortCode asc", deptCode);
            DataTable dtDepts = this.BaseRepository().FindTable(sql);
            string whereSQL = " datatype in(0,2)";
            whereSQL += " and to_char(t.checkbegintime,'yyyy')='" + year + "'";
            //区域类型
            if (!String.IsNullOrEmpty(belongdistrictcode))
            {
                whereSQL += " and  t.belongdeptid  in (select departmentid from base_department where encode like '" + belongdistrictcode + "%')";
            }
            List<SaftyCheckCountEntity> list = new List<SaftyCheckCountEntity>();
            foreach (DataRow item in dtDepts.Rows)
            {
                string where = string.Format(" and ((createuserorgcode  in (select encode from base_department where (nature='厂级' or nature='电厂') start with encode='{0}' connect by  prior departmentid = parentid)))", item[0].ToString());
                //var where = " and ((',' || checkdeptcode like '%," + item[0].ToString() + "%' and datatype in(0,2)))";
                string forsql = @"select nvl(b.num,0) num,a.itemvalue from (
select t.itemvalue,count(1) as num from BASE_DATAITEMDETAIL t where t.itemid =(select itemid from base_dataitem where itemcode='SaftyCheckType')  group by itemvalue
) a
left join(
select to_char(CHECKDATATYPE) as itemvalue, count(1) as num from BIS_SAFTYCHECKDATARECORD t where " + whereSQL + where + " group by CHECKDATATYPE)  b on a.itemvalue = b.itemvalue order by  a.itemvalue";
                DataTable cou = this.BaseRepository().FindTable(forsql);
                int rc = 0, zx = 0, jj = 0, jjr = 0, zh = 0, sj = 0;
                if (ctype != "省公司")
                {
                    for (int i = 0; i < cou.Rows.Count; i++)
                    {
                        var checktype = cou.Rows[i][1].ToString();
                        var checknum = Convert.ToInt32(cou.Rows[i][0].ToString());
                        switch (checktype)
                        {
                            case "1":
                                rc = checknum;
                                break;
                            case "2":
                                zx = checknum;
                                break;
                            case "3":
                                jjr = checknum;
                                break;
                            case "4":
                                jj = checknum;
                                break;
                            case "5":
                                zh = checknum;
                                break;
                            default:
                                break;
                        }
                    }
                }
                if (ctype != "电厂")
                {
                    string whereSQL2 = " to_char(checkbegintime,'yyyy')='" + year + "' and (checkeddepartid like '%" + item[2].ToString() + "%' and datatype=0)";
                    sj = this.BaseRepository().FindObject("select count(1) from BIS_SAFTYCHECKDATARECORD t where " + whereSQL2).ToInt();
                }
                list.Add(new SaftyCheckCountEntity
                {
                    month = item[1].ToString(),
                    deptcode = item[0].ToString(),
                    deptid = item[2].ToString(),
                    rc = rc,
                    zx = zx,
                    jjr = jjr,
                    jj = jj,
                    zh = zh,
                    sj = sj,
                    sum = rc + zx + jjr + jj + zh + sj
                });
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                total = 1,
                page = 1,
                records = list.Count,
                rows = list
            });
        }
        /// <summary>
        /// 条形图统计数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">年份</param>
        /// <param name="belongdistrictcode">区域code</param>
        /// <param name="ctype">检查类型</param>
        /// <returns></returns>
        public string getRatherCheckCount(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "日常安全检查,专项安全检查,季节性安全检查,节假日安全检查,综合性安全检查")
        {
            if (string.IsNullOrEmpty(ctype) || ctype == "请选择")
            {
                ctype = "日常安全检查,专项安全检查,季节性安全检查,节假日前后安全检查,综合安全检查,上级单位安全检查";
            }
            List<string> yValues = new List<string>();
            for (int i = 1; i <= 12; i++)
            {
                yValues.Add(i.ToString() + "月");
            }
            List<object> dic = new List<object>();
            string[] type = ctype.Split(',');
            string whereSQL = "datatype in(0,2) ";

            string roleName = OperatorProvider.Provider.Current().RoleName;
            var user = OperatorProvider.Provider.Current();
            string owndeptcode = user.DeptCode;
            //单位
            if (!String.IsNullOrEmpty(deptCode))
            {
                whereSQL += " and (',' || CheckDeptCode  like '%," + deptCode + "%' or ',' || CHECKDEPTID like '%," + deptCode + "%')";
            }
            else
            {
                if (roleName.Contains("厂级部门用户") || roleName.Contains("公司级用户"))
                {
                    whereSQL += string.Format(" and (',' || CheckDeptCode like '%,{0}%' or ','|| CHECKDEPTID like '%,{0}%' or checkeddepartid like '%{1}%')", user.OrganizeCode, user.OrganizeId);
                }
                else
                {
                    whereSQL += string.Format(" and (belongdept like '{0}%' or  ',' || checkdeptid || ',' like '%,{0}%' or ',' || checkdeptcode || ',' like '%,{0}%') ", user.DeptCode);
                    //whereSQL += string.Format(" and ((datatype=0 and createuserdeptcode like '{0}%') or ((status=2 and (',' || checkdeptid || ',') like '%,{0}%') or (datatype=0 and id in(select recid from BIS_SAFETYCHECKDEPT where deptcode in(select encode from base_department d where d.encode like '{0}%' )))))", owndeptcode);
                    //whereSQL += string.Format(" and checkeddepartid is null and (CheckDeptCode like '{0}%' or CHECKDEPTID like '{0}%')", owndeptcode);
                }
            }
            //年限
            whereSQL += " and to_char(checkbegintime,'yyyy')='" + year + "'";
            //区域类型
            if (!String.IsNullOrEmpty(belongdistrictcode))
            {
                whereSQL += " and  belongdeptid  in (select departmentid from base_department where encode like '" + belongdistrictcode + "%')";
            }


            foreach (string itemType in type)
            {
                var item = itemType.TrimStart();//传过来的字符中有空格
                string ct = "1";
                if (item == "专项安全检查") ct = "2";
                if (item == "节假日前后安全检查") ct = "3";
                if (item == "季节性安全检查") ct = "4";
                if (item == "综合安全检查") ct = "5";
                List<int> list = new List<int>();
                for (int i = 1; i <= 12; i++)
                {
                    int cou = 0;
                    if (itemType == "上级单位安全检查")
                    {
                        string whereSQL2 = " to_char(checkbegintime,'yyyy')='" + year + "' and to_char(t.checkbegintime,'mm')=" + i.ToString() + " and checkeddepartid like '%" + user.OrganizeId + "%' and (issynview='1' or (issynview='0' and checkbegintime<=to_date('" + DateTime.Now.ToString("yyyy-MM-dd") + "','yyyy-MM-dd')))";
                        cou = this.BaseRepository().FindObject("select count(1) from BIS_SAFTYCHECKDATARECORD t where " + whereSQL2).ToInt();
                    }
                    else
                    {
                        string whereSQL2 = " and to_char(t.checkbegintime,'mm')=" + i.ToString() + " and CHECKDATATYPE='" + ct + "' and belongdept like '" + user.OrganizeCode + "%'";
                        cou = this.BaseRepository().FindObject("select count(1) from BIS_SAFTYCHECKDATARECORD t where " + whereSQL + whereSQL2).ToInt();
                    }
                    list.Add(cou);
                }
                dic.Add(new { name = item, data = list });
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(new { x = dic, y = yValues });
        }
        public string getGrpRatherCheckCount(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "")
        {
            List<string> yValues = new List<string>();
            for (int i = 1; i <= 12; i++)
            {
                yValues.Add(i.ToString() + "月");
            }
            List<object> dic = new List<object>();
            string[] type = new string[] { "日常安全检查", "专项安全检查", "季节性安全检查", "节假日前后安全检查", "综合安全检查", "省公司安全检查" };
            string whereSQL = " datatype in(0,2)";
            if (ctype != "省公司")
            {
                whereSQL += " and t.createuserorgcode<>'" + deptCode + "'";
            }
            var user = OperatorProvider.Provider.Current();
            whereSQL += string.Format(" and (( createuserorgcode in (select encode from BASE_DEPARTMENT  where (nature='厂级' or nature='电厂') start with encode='{0}' connect by  prior  departmentid = parentid))", deptCode);
            whereSQL += string.Format(" or (checkeddepartid in (select departmentid from BASE_DEPARTMENT  where (nature='厂级' or nature='电厂') start with encode='{0}' connect by  prior  departmentid = parentid)))", deptCode);
            //年限
            whereSQL += "  and to_char(checkbegintime,'yyyy')='" + year + "'";
            //区域类型
            if (!String.IsNullOrEmpty(belongdistrictcode))
            {
                whereSQL += " and  belongdeptid  in (select departmentid from base_department where encode like '" + belongdistrictcode + "%')";
            }
            foreach (string itemType in type)
            {
                var item = itemType.TrimStart();//传过来的字符中有空格
                string ct = "1";
                if (item == "专项安全检查") ct = "2";
                if (item == "节假日前后安全检查") ct = "3";
                if (item == "季节性安全检查") ct = "4";
                if (item == "综合安全检查") ct = "5";
                List<int> list = new List<int>();
                for (int i = 1; i <= 12; i++)
                {
                    int cou = 0;
                    if (itemType == "省公司安全检查")
                    {
                        if (ctype != "电厂")
                        {
                            string whereSQL2 = " datatype in(0,2) and to_char(checkbegintime,'yyyy')='" + year + "' and to_char(t.checkbegintime,'mm')=" + i.ToString() + " and checkeddepartid in(select departmentid from base_department where (nature='电厂' or nature='厂级') start with encode='" + deptCode + "' connect by  prior  departmentid = parentid)";
                            cou = this.BaseRepository().FindObject("select count(1) from BIS_SAFTYCHECKDATARECORD t where " + whereSQL2).ToInt();
                        }
                    }
                    else
                    {
                        if (ctype != "省公司")
                        {
                            string whereSQL2 = " and to_char(t.checkbegintime,'mm')=" + i.ToString() + " and CHECKDATATYPE='" + ct + "' ";
                            //whereSQL2 += string.Format(" or (datatype=2 and checkeddepartid in (select departmentid from BASE_DEPARTMENT  where (nature='厂级' or nature='电厂') start with encode='{0}' connect by  prior  departmentid = parentid))", deptCode);
                            cou = this.BaseRepository().FindObject("select count(1) from BIS_SAFTYCHECKDATARECORD t where " + whereSQL + whereSQL2).ToInt();
                        }
                    }
                    list.Add(cou);
                }
                dic.Add(new { name = item, data = list });
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(new { x = dic, y = yValues });
        }
        /// <summary>
        /// 统计对下属各电厂下发的任务次数统计
        /// </summary>
        /// <param name="user"></param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns></returns>
        public string getCheckTaskCount(Operator user, string startDate = "", string endDate = "")
        {

            List<object> dic = new List<object>();
            string[] type = new string[] { "专项安全检查", "季节性安全检查", "节假日前后安全检查", "综合安全检查" };

            DataTable dtDepts = new DepartmentService().GetAllFactory(user);
            List<string> yValues = new List<string>();
            foreach (DataRow dr in dtDepts.Rows)
            {
                yValues.Add(dr["fullname"].ToString());
            }
            foreach (string itemType in type)
            {
                var item = itemType.TrimStart();//传过来的字符中有空格
                string ct = "2";
                if (item == "专项安全检查") ct = "2";
                if (item == "节假日前后安全检查") ct = "3";
                if (item == "季节性安全检查") ct = "4";
                if (item == "综合安全检查") ct = "5";
                List<int> list = new List<int>();
                foreach (DataRow dr in dtDepts.Rows)
                {
                    int cou = 0;
                    string whereSQL2 = string.Format("datatype=2 and CheckedDepartID='{0}' and checkdatatype={1}", dr["departmentid"].ToString(), ct);
                    if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
                    {
                        whereSQL2 += string.Format(" and createdate between to_date('{0}','yyyy-mm-dd') and to_date('{1}','yyyy-mm-dd hh24:mi:ss')", startDate, endDate);
                    }
                    cou = this.BaseRepository().FindObject("select count(1) from BIS_SAFTYCHECKDATARECORD t where " + whereSQL2).ToInt();
                    list.Add(cou);
                }
                dic.Add(new { name = item, data = list });

            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(new { x = dic, y = yValues });
        }
        public DataTable getCheckTaskData(Operator user, string startDate = "", string endDate = "")
        {

            List<object> dic = new List<object>();
            string[] type = new string[] { "专项安全检查", "季节性安全检查", "节假日前后安全检查", "综合安全检查" };

            DataTable dtDepts = new DepartmentService().GetAllFactory(user);
            List<string> yValues = new List<string>();
            DataTable dtData = new DataTable();
            dtData.Columns.Add("deptname");
            dtData.Columns.Add("zx");
            dtData.Columns.Add("jj");
            dtData.Columns.Add("jjr");
            dtData.Columns.Add("zh");
            dtData.Columns.Add("deptcode");
            foreach (DataRow dr in dtDepts.Rows)
            {
                DataRow dr1 = dtData.NewRow();
                dr1[0] = dr["fullname"].ToString();
                dr1["deptcode"] = dr["encode"].ToString();
                for (int j = 2; j < 6; j++)
                {
                    int cou = 0;
                    string whereSQL2 = string.Format("datatype=2 and CheckedDepartID='{0}' and checkdatatype={1}", dr["departmentid"].ToString(), j);
                    if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
                    {
                        whereSQL2 += string.Format(" and createdate between to_date('{startDate}','yyyy-mm-dd') and to_date('{startDate}','yyyy-mm-dd hh24:mi:ss')", startDate, endDate);
                    }
                    cou = this.BaseRepository().FindObject("select count(1) from BIS_SAFTYCHECKDATARECORD t where " + whereSQL2).ToInt();
                    dr1[j - 1] = cou;
                }
                dtData.Rows.Add(dr1);
            }
            return dtData;
        }
        /// <summary>
        /// 获取对比数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">年份</param>
        /// <param name="belongdistrictcode">区域</param>
        /// <param name="ctype">检查类型</param>

        /// <returns></returns>
        public string GetAreaSaftyState(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "")
        {

            string sql = "";
            //判断用户觉得是不是厂级或者公司级用户,查看所有数据
            string roleName = OperatorProvider.Provider.Current().RoleName;
            var user = OperatorProvider.Provider.Current();
            string owndeptcode = user.DeptCode;
            if (String.IsNullOrEmpty(deptCode))
            {
                if (roleName.Contains("省级用户") || roleName.Contains("集团用户"))
                {
                    sql = string.Format("select  encode,fullname from BASE_DEPARTMENT  where nature='厂级' and deptcode like '{0}%'", user.NewDeptCode);
                }
                else if (roleName.Contains("厂级部门用户") || roleName.Contains("公司级用户"))
                {
                    sql = string.Format("select  encode,fullname from BASE_DEPARTMENT  where encode like '{0}___' ", user.OrganizeCode);
                }
                else
                {
                    sql = string.Format("select  encode,fullname from BASE_DEPARTMENT  where (encode='{0}' or  encode like '{0}___' )", owndeptcode);
                }
            }
            else
            {
                sql = string.Format("select  encode,fullname from BASE_DEPARTMENT  where (encode='{0}' or  encode like '{0}___' )", deptCode);
            }
            if (string.IsNullOrEmpty(ctype) || ctype == "请选择")
            {
                ctype = "日常安全检查,专项安全检查,季节性安全检查,节假日前后安全检查,综合安全检查";
            }
            sql += " order by SortCode asc";
            DataTable dtDepts = this.BaseRepository().FindTable(sql);

            List<string> yValues = new List<string>();

            List<object> dic = new List<object>();
            string[] type = ctype.Split(',');
            string whereSQL = " 1=1";

            //单位

            //年限
            whereSQL += " and to_char(checkbegintime,'yyyy')='" + year + "'";
            //区域类型
            if (!String.IsNullOrEmpty(belongdistrictcode))
            {
                whereSQL += " and  belongdeptid  in (select departmentid from base_department where encode like '" + belongdistrictcode + "%')";
            }

            bool isRead = false;
            foreach (string itemType in type)
            {
                var item = itemType.TrimStart();//传值出现空格
                string ct = "1";
                if (item == "专项安全检查") ct = "2";
                if (item == "节假日前后安全检查") ct = "3";
                if (item == "季节性安全检查") ct = "4";
                if (item == "综合安全检查") ct = "5";
                List<int> list = new List<int>();
                foreach (DataRow dept in dtDepts.Rows)
                {
                    if (!isRead)
                    {
                        yValues.Add(dept[1].ToString());
                    }
                    var where = "";
                    if ((deptCode == dept[0].ToString() && deptCode.Length == 6 && deptCode == owndeptcode) || (owndeptcode == dept[0].ToString() && owndeptcode.Length == 6 && deptCode == ""))
                    {
                        where = string.Format("  and CHECKDATATYPE='" + ct + "' and datatype in(0,2)  and (',' || CHECKDEPTID like '%,{0}%')", dept[0].ToString());
                    }
                    else
                    {
                        where = " and CHECKDATATYPE='" + ct + "' and datatype in(0,2) and (',' || CheckDeptCode like '%," + dept[0].ToString() + "%' or  ',' || CHECKDEPTID like '%," + dept[0].ToString() + "%')";
                    }
                    //if (ct == "1")
                    //{
                    //    where = "  and CHECKDATATYPE='" + ct + "'  and instr(CREATEUSERDEPTCODE,'" + dept[0].ToString() + "')>0";
                    //}
                    //else
                    //{
                    //    where = "  and CHECKDATATYPE='" + ct + "'  and instr(CHECKDEPTID,'" + dept[0].ToString() + "')>0";
                    //}
                    int cou = this.BaseRepository().FindObject("select count(1) from BIS_SAFTYCHECKDATARECORD t where " + whereSQL + where).ToInt();
                    list.Add(cou);
                }

                isRead = true;
                dic.Add(new { name = item, data = list });
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(new { x = dic, y = yValues });
        }
        /// <summary>
        /// 省公司统计
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="year"></param>
        /// <param name="belongdistrictcode"></param>
        /// <param name="ctype"></param>
        /// <returns></returns>
        public string GetGrpAreaSaftyState(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "")
        {

            string sql = "";
            var user = OperatorProvider.Provider.Current();
            //string owndeptcode = user.OrganizeCode;
            sql = string.Format("select  encode,fullname,departmentid from BASE_DEPARTMENT  where (nature='厂级' or nature='电厂') start with encode='{0}' connect by  prior  departmentid = parentid order by SortCode asc", deptCode);
            DataTable dtDepts = this.BaseRepository().FindTable(sql);
            List<string> yValues = new List<string>();
            List<object> dic = new List<object>();
            string[] type = "日常安全检查,专项安全检查,季节性安全检查,节假日前后安全检查,综合安全检查,省公司安全检查".Split(',');
            string whereSQL = " 1=1";

            //年限
            whereSQL += string.Format(" and to_char(checkbegintime,'yyyy')='" + year + "'");
            //区域类型
            if (!String.IsNullOrEmpty(belongdistrictcode))
            {
                whereSQL += " and  belongdeptid  in (select departmentid from base_department where encode like '" + belongdistrictcode + "%')";
            }

            bool isRead = false;
            foreach (string itemType in type)
            {
                var item = itemType.TrimStart();//传值出现空格
                string ct = "1";
                if (item == "专项安全检查") ct = "2";
                if (item == "节假日前后安全检查") ct = "3";
                if (item == "季节性安全检查") ct = "4";
                if (item == "综合安全检查") ct = "5";
                List<int> list = new List<int>();
                foreach (DataRow dept in dtDepts.Rows)
                {
                    if (!isRead)
                    {
                        yValues.Add(dept[1].ToString());
                    }
                    if (itemType == "省公司安全检查")
                    {
                        if (ctype != "电厂")
                        {
                            var wheresgs = string.Format(" to_char(checkbegintime,'yyyy')='{0}' and (checkeddepartid='{1}' and datatype=0)", year, dept[2].ToString());
                            int cou = this.BaseRepository().FindObject("select count(1) from BIS_SAFTYCHECKDATARECORD t where " + wheresgs).ToInt();
                            list.Add(cou);
                        }
                        else
                            list.Add(0);
                    }
                    else
                    {
                        if (ctype != "省公司")
                        {
                            var where = " and datatype in(0,2)  and CHECKDATATYPE='" + ct + "'  and (createuserorgcode='" + dept[0].ToString() + "' or (checkeddepartid in(select departmentid from base_department where (nature='电厂' or nature='厂级') start with encode='" + dept[0].ToString() + "' connect by  prior  departmentid = parentid) and datatype=2))";
                            int cou = this.BaseRepository().FindObject("select count(1) from BIS_SAFTYCHECKDATARECORD t where " + whereSQL + where).ToInt();
                            list.Add(cou);
                        }
                        else
                            list.Add(0);
                    }
                }
                isRead = true;
                dic.Add(new { name = item, data = list });
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(new { x = dic, y = yValues });
        }
        /// <summary>
        /// 曲线图统计数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">年份</param>
        /// <param name="belongdistrictcode">区域code</param>
        /// <param name="ctype">检查类型</param>
        /// <returns></returns>
        public string getMonthCheckCount(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "日常安全检查,专项安全检查,季节性安全检查,节假日安全检查,综合性安全检查,上级单位安全检查")
        {
            if (string.IsNullOrEmpty(ctype) || ctype == "请选择")
            {
                ctype = "日常安全检查,专项安全检查,季节性安全检查,节假日前后安全检查,综合安全检查,上级单位安全检查";
            }
            List<object> dic = new List<object>();
            List<int> list = new List<int>();
            List<string> yValues = new List<string>();
            string[] type = ctype.Split(',');
            string whereSQL = " datatype in(0,2)";
            string roleName = OperatorProvider.Provider.Current().RoleName;
            var user = OperatorProvider.Provider.Current();
            string owndeptcode = user.DeptCode;
            //单位
            if (!String.IsNullOrEmpty(deptCode))
            {
                whereSQL += " and  (',' || CheckDeptCode like '%," + deptCode + "%' or ',' || CHECKDEPTID like '%," + deptCode + "%')";
            }
            else
            {
                if (roleName.Contains("厂级部门用户") || roleName.Contains("公司级用户"))
                {
                    whereSQL += string.Format(" and ((datatype in(0,2) and ( ',' || CheckDeptCode like '%,{0}%' or ',' || CHECKDEPTID like '%,{0}%')) or (datatype=2 and checkeddepartid like '%{1}%'))", user.OrganizeCode, user.OrganizeId);
                }
                else
                {
                    whereSQL += string.Format(" and (',' || CheckDeptCode like '%,{0}%' or belongdept like '{0}%' or  ',' || CHECKDEPTID like '%,{0}%')", owndeptcode);
                }
            }
            //年限
            whereSQL += string.Format(" and  to_char(checkbegintime,'yyyy')='" + year + "'");
            //区域类型
            if (!String.IsNullOrEmpty(belongdistrictcode))
            {
                whereSQL += " and  belongdeptid  in (select departmentid from base_department where encode like '" + belongdistrictcode + "%')";
            }
            string cr = " and CHECKDATATYPE in ('";
            if (ctype.Contains("日常安全检查")) cr += "1','";
            if (ctype.Contains("专项安全检查")) cr += "2','";
            if (ctype.Contains("季节性安全检查")) cr += "3','";
            if (ctype.Contains("节假日前后安全检查")) cr += "4','";
            if (ctype.Contains("综合安全检查")) cr += "5','";
            cr += "')";
            whereSQL += cr;
            for (int i = 1; i <= 12; i++)
            {
                string whereSQL2 = " and to_char(t.checkbegintime,'mm')=" + i.ToString();
                int cou = this.BaseRepository().FindObject("select count(1) from BIS_SAFTYCHECKDATARECORD t where " + whereSQL + whereSQL2).ToInt();

                string whereSJ = " to_char(checkbegintime,'yyyy')='" + year + "' and to_char(t.checkbegintime,'mm')=" + i.ToString() + " and checkeddepartid like '%" + user.OrganizeId + "%' and (issynview='1' or (issynview='0' and checkbegintime<=to_date('" + DateTime.Now.ToString("yyyy-MM-dd") + "','yyyy-MM-dd')))";
                int couSJ = this.BaseRepository().FindObject("select count(1) from BIS_SAFTYCHECKDATARECORD t where " + whereSJ).ToInt();
                cou += couSJ;
                list.Add(cou);
                yValues.Add(i.ToString() + "月");
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(new { x = list, y = yValues });
        }
        public string getGrpMonthCheckCount(string deptCode, string year = "", string belongdistrictcode = "", string ctype = "")
        {
            List<object> dic = new List<object>();
            List<int> list = new List<int>();
            List<string> yValues = new List<string>();
            string whereSQL = "datatype in(0,2)";
            if (ctype != "省公司")
            {
                whereSQL += " and t.createuserorgcode<>'" + deptCode + "'";
            }
            var user = OperatorProvider.Provider.Current();

            whereSQL += string.Format(" and ((createuserorgcode in (select encode from BASE_DEPARTMENT  where (nature='厂级' or nature='电厂') start with encode='{0}' connect by  prior  departmentid = parentid)", deptCode);
            //年限
            whereSQL += "  ) or (',' || checkdeptcode like '%," + deptCode + "%')) and to_char(checkbegintime,'yyyy')='" + year + "'";

            //whereSQL += string.Format(" and ((checkeddepartid is null and createuserorgcode in (select encode from BASE_DEPARTMENT  where (nature='厂级' or nature='电厂') start with encode='{0}' connect by  prior  departmentid = parentid)) or datatype=2)", deptCode);          
            ////年限
            //whereSQL += " and to_char(checkbegintime,'yyyy')='" + year + "'";
            //区域类型
            if (!String.IsNullOrEmpty(belongdistrictcode))
            {
                whereSQL += " and  belongdeptid  in (select departmentid from base_department where encode like '" + belongdistrictcode + "%')";
            }
            string cr = " and  CHECKDATATYPE in ('1','2','3','4','5')";
            whereSQL += cr;
            for (int i = 1; i <= 12; i++)
            {
                int cou = 0;
                if (ctype != "省公司")
                {
                    string whereSQL2 = " and to_char(t.checkbegintime,'mm')=" + i.ToString();
                    cou = this.BaseRepository().FindObject("select count(1) from BIS_SAFTYCHECKDATARECORD t where " + whereSQL + whereSQL2).ToInt();
                }

                int couSJ = 0;
                if (ctype != "电厂")
                {
                    string whereSJ = "  datatype in(0,2) and to_char(checkbegintime,'yyyy')='" + year + "' and to_char(t.checkbegintime,'mm')=" + i.ToString() + " and (datatype=0 and checkeddepartid in(select departmentid from base_department where (nature='电厂' or nature='厂级') start with encode='" + deptCode + "' connect by  prior  departmentid = parentid))";
                    couSJ = this.BaseRepository().FindObject("select count(1) from BIS_SAFTYCHECKDATARECORD t where " + whereSJ).ToInt();
                }
                cou += couSJ;

                list.Add(cou);
                yValues.Add(i.ToString() + "月");
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(new { x = list, y = yValues });
        }
        /// <summary>
        /// 专项检查和其他导出
        /// </summary>
        public DataTable ExportData(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();
            string checkType = "";
            //时间选择
            if (!queryParam["st"].IsEmpty())
            {
                string st = queryParam["st"].ToString();
                string et = queryParam["et"].ToString();
                pagination.conditionJson += string.Format(" and t.CHECKBEGINTIME between to_date('{0}','yyyy-mm-dd') and to_date('{1}','yyyy-mm-dd')", st, et);
            }
            if (!queryParam["stm"].IsEmpty())
            {
                string st = queryParam["stm"].ToString();
                string et = queryParam["etm"].ToString();
                pagination.conditionJson += string.Format(" and t.CHECKBEGINTIME>=to_date('{0}','yyyy-mm-dd') and CHECKENDTIME<=to_date('{1}','yyyy-mm-dd')", st, et);
            }
            //检查名称
            if (!queryParam["keyword"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.CheckDataRecordName like '%{0}%'", queryParam["keyword"].ToString());
            }
            //安全检查类型
            if (!queryParam["ctype"].IsEmpty())
            {
                checkType = queryParam["ctype"].ToString();
                pagination.conditionJson += string.Format(" and t.CHECKDATATYPE='{0}'", queryParam["ctype"].ToString());
            }
            //被检查单位
            if (!queryParam["chkDept"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.CheckedDepartID = '{0}'", queryParam["chkDept"].ToString());
            }
            //省公司创建的检查
            if (!queryParam["isGrpChecked"].IsEmpty())
            {
                pagination.conditionJson += " and t.CheckedDepartID is not null";
            }
            //所属部门
            if (!queryParam["code"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.belongdeptid  in (select departmentid from base_department start with encode='{0}' connect by  prior departmentid = parentid union select organizeid from base_organize where encode like '{0}%') ", queryParam["code"].ToString());
            }
            DataTable dtAll = this.BaseRepository().FindTableByProcPager(pagination, dataType);

            foreach (DataRow item in dtAll.Rows)
            {
                double sum = BaseRepository().FindObject(string.Format("select count(1) from bis_saftycheckdatadetailed t where recid='{0}'", item["id"].ToString())).ToDouble();
                if (sum == 0)
                {
                    item["SolveCount"] = "未开始检查";
                }
                else
                {
                    string tableName = checkType == "1" ? "BIS_SAFTYCHECKDATADETAILED" : "BIS_SAFTYCONTENT";
                    double num = BaseRepository().FindObject(string.Format("select count(1) from {1} t where recid='{0}' and issure is not null", item["id"].ToString(), tableName)).ToDouble();
                    double result = Math.Round(num / sum * 100, 2);
                    result = result >= 100 ? 100 : result;
                    item["SolveCount"] = num == 0 ? "未开始检查" : result.ToString();
                }
                if(item["CheckLevel"].ToString()=="0")
                {
                    DataTable dtName = this.BaseRepository().FindTable("select itemname from base_dataitemdetail where itemid=(select itemid from base_dataitem where itemcode='SuperiorCheckLevel') and itemvalue='" + item["SJCheckLevel"] + "'");
                    if (dtName != null && dtName.Rows.Count > 0)
                        item["CheckLevel"] = dtName.Rows[0]["itemname"];
                }
                else
                {
                    DataTable dtName = this.BaseRepository().FindTable("select itemname from base_dataitemdetail where itemid=(select itemid from base_dataitem where itemcode='SaftyCheckLevel') and itemvalue='" + item["CheckLevel"] + "'");
                    if (dtName != null && dtName.Rows.Count > 0)
                        item["CheckLevel"] = dtName.Rows[0]["itemname"];
                }
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                int count = this.BaseRepository().FindObject("select count(1) from bis_htbaseinfo o where  o.safetycheckobjectid='" + item["id"].ToString() + "'").ToInt();
                string html = "隐患数量："+count.ToString();
                string jindu = "隐患处理进度:";
                decimal count1 = this.BaseRepository().FindObject("select count(1) from bis_htbaseinfo o where workstream='整改结束' and  o.safetycheckobjectid='" + item["id"].ToString() + "'").ToInt();
                if (count > 0)
                {
                    count1 = Math.Round(decimal.Parse((count1 / count).ToString()), 2);
                    jindu = string.Format("隐患处理进度：{0}%",count1 * 100);
                }
                else
                {
                    jindu = "隐患处理进度：-";
                }

                int num1= BaseRepository().FindObject(string.Format("select count(1) from  base_appsettingassociation a where a.deptid='{0}' and a.moduleid in(select id from BASE_MENUCONFIG t where t.modulecode='PECCANCY_ADD')", user.OrganizeId)).ToInt();
                if (num1>0)
                {
                    int wzcount = this.BaseRepository().FindObject("select count(1) from BIS_LLLEGALREGISTER o where  o.reseverone='" + item["id"].ToString() + "'").ToInt();
                    html += ",违章数量：" + wzcount.ToString();

                    count1 = this.BaseRepository().FindObject("select count(1) from BIS_LLLEGALREGISTER o where flowstate='流程结束' and  o.reseverone='" + item["id"].ToString() + "'").ToInt();
                    if (wzcount > 0)
                    {
                        count1 = Math.Round(decimal.Parse((count1 / wzcount).ToString()), 2);
                        jindu += string.Format(",违章处理进度：{0}%", count1 * 100);
                    }
                    else
                    {
                        jindu += ",违章处理进度：-";
                    }

                }
                num1 = BaseRepository().FindObject(string.Format("select count(1) from  base_appsettingassociation a where a.deptid='{0}' and a.moduleid in(select id from BASE_MENUCONFIG t where t.modulecode='QUESTION_ADD')", user.OrganizeId)).ToInt();
                if (num1 > 0)
                {
                    int wtcount = this.BaseRepository().FindObject("select count(1) from BIS_QUESTIONINFO o where  o.CHECKID='" + item["id"].ToString() + "'").ToInt();
                    html += ",问题数量：" + wtcount.ToString();
                    count1 = this.BaseRepository().FindObject("select count(1) from BIS_QUESTIONINFO o where flowstate='流程结束' and  o.CHECKID='" + item["id"].ToString() + "'").ToInt();
                    if (wtcount > 0)
                    {
                        count1 = Math.Round(decimal.Parse((count1 / wtcount).ToString()), 2);
                        jindu += string.Format(",违章处理进度：{0}%", count1 * 100);
                    }
                    else
                    {
                        jindu += ",问题处理进度：-";
                    }
                }
                item["count"] = html;
                item["count1"] = jindu;
            }
            return dtAll;
        }

        /// <summary>
        /// 专项和其他列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>

        public IEnumerable<SaftyCheckDataRecordEntity> GetPageListForType(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();

            //时间选择
            if (!queryParam["stm"].IsEmpty())
            {
                string st = queryParam["stm"].ToString();
                pagination.conditionJson += string.Format(" and t.CHECKBEGINTIME >=to_date('{0}','yyyy-mm-dd') ", st);
            }
            if (!queryParam["etm"].IsEmpty())
            {
                string et = queryParam["etm"].ToString();
                pagination.conditionJson += string.Format(" and t.CHECKENDTIME<=to_date('{0}','yyyy-mm-dd')", et);
            }
            //检查名称
            if (!queryParam["keyword"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.CheckDataRecordName like '%{0}%'", queryParam["keyword"].ToString());
            }
            //被检查单位
            if (!queryParam["chkDept"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.CheckedDepartID like '%{0}%'", queryParam["chkDept"].ToString());
            }
            if (!queryParam["chkDeptName"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.CheckedDepart like '%{0}%'", queryParam["chkDeptName"].ToString());
            }
            //省公司创建的检查
            if (!queryParam["isGrpChecked"].IsEmpty())
            {
                pagination.conditionJson += " and (t.CheckedDepartID is not null and datatype=0)";
            }
            //所属部门
            if (!queryParam["code"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.belongdeptid  in (select departmentid from base_department start with encode='{0}' connect by  prior departmentid = parentid union select organizeid from base_organize where encode like '{0}%') ", queryParam["code"].ToString());
            }

            //安全检查类型
            if (!queryParam["ctype"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.CHECKDATATYPE='{0}'", queryParam["ctype"].ToString());
            }
            if (!queryParam["dataType"].IsEmpty())
            {
                if (queryParam["dataType"].ToString() == "10")
                {
                    pagination.conditionJson += string.Format(" and (datatype=1 or datatype=2)", queryParam["dataType"].ToString());
                }
                else
                {
                    pagination.conditionJson += string.Format(" and datatype={0}", queryParam["dataType"].ToString());
                }

            }
            IEnumerable<SaftyCheckDataRecordEntity> list = this.BaseRepository().FindListByProcPager(pagination, dataType);

            foreach (SaftyCheckDataRecordEntity r in list)
            {
                //总共的检查人员数量
                //double havdel = dt2.Rows.Count;
                //double havdel = string.IsNullOrEmpty(r.CheckUserIds) ? 0 : r.CheckUserIds.Split(',').Length;
                //if (string.IsNullOrEmpty(r.SolvePerson))
                //{
                //    r.SolveCount = 0;
                //}
                //else
                //{
                double sum = BaseRepository().FindObject(string.Format("select count(1) from bis_saftycheckdatadetailed t where recid='{0}'", r.ID)).ToDouble();
                if (sum == 0)
                {
                    r.SolveCount = 0;
                }
                else
                {
                    double count = BaseRepository().FindObject(string.Format("select count(1) from BIS_SAFTYCONTENT t where recid='{0}'", r.ID)).ToDouble();
                    count = count > sum ? sum : count;
                    r.SolveCount = Math.Round(count / sum * 100, 2);
                }
                //}
                //计算所含隐患个数
                int num = this.BaseRepository().FindObject("select count(1) from bis_htbaseinfo o where o.safetycheckobjectid='" + r.ID + "'").ToInt();
                r.Count = num;
                //获取违章数量
                num = this.BaseRepository().FindObject("select count(1) from BIS_LLLEGALREGISTER where reseverone='" + r.ID + "'").ToInt();
                r.WzCount = num;
                //问题数量
                num = this.BaseRepository().FindObject("select count(1) from BIS_QUESTIONINFO where CHECKID='" + r.ID + "'").ToInt();
                r.WtCount = num;

                decimal count1 = this.BaseRepository().FindObject("select count(1) from bis_htbaseinfo o where workstream='整改结束' and  o.safetycheckobjectid='" + r.ID + "'").ToInt();
                if (r.Count > 0)
                {
                    count1 = Math.Round(decimal.Parse((count1 / r.Count).ToString()), 2);
                    r.Count1 = count1 * 100;
                }
                else
                {
                    r.Count1 = 0;
                }
                count1 = this.BaseRepository().FindObject("select count(1) from BIS_LLLEGALREGISTER o where flowstate='流程结束' and  o.reseverone='" + r.ID + "'").ToInt();
                if (r.WzCount > 0)
                {
                    count1 = Math.Round(decimal.Parse((count1 / r.WzCount).ToString()), 2);
                    r.WzCount1 = count1 * 100;
                }
                else
                {
                    r.WzCount1 = 0;
                }
                count1 = this.BaseRepository().FindObject("select count(1) from BIS_QUESTIONINFO o where flowstate='流程结束' and  o.CHECKID='" + r.ID + "'").ToInt();
                if (r.WtCount > 0)
                {
                    count1 = Math.Round(decimal.Parse((count1 / r.WtCount).ToString()), 2);
                    r.WtCount1 = count1 * 100;
                }
                else
                {
                    r.WtCount1 = 0;
                }
            }
            return list;
        }
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
        /// <summary>
        /// 安全检查任务
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetCheckTaskList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();

            //时间选择
            if (!queryParam["st"].IsEmpty())
            {
                string st = queryParam["st"].ToString();
                string et = queryParam["et"].ToString();
                pagination.conditionJson += string.Format(" and t.CHECKBEGINTIME between to_date('{0}','yyyy-mm-dd') and to_date('{1}','yyyy-mm-dd')", st, et);
            }
            if (!queryParam["stm"].IsEmpty())
            {
                string st = queryParam["stm"].ToString();
                string et = queryParam["etm"].ToString();
                pagination.conditionJson += string.Format(" and t.CHECKBEGINTIME>=to_date('{0}','yyyy-mm-dd') and CHECKENDTIME<=to_date('{1}','yyyy-mm-dd')", st, et);
            }
            //检查名称
            if (!queryParam["keyword"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.CheckDataRecordName like '%{0}%'", queryParam["keyword"].ToString());
            }
            //被检查单位
            if (!queryParam["chkDept"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.CheckedDepartID like '%{0}%'", queryParam["chkDept"].ToString());
            }
            if (!queryParam["chkDeptName"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.CheckedDepart like '%{0}%'", queryParam["chkDeptName"].ToString());
            }
            //省公司创建的检查
            if (!queryParam["isGrpChecked"].IsEmpty())
            {
                pagination.conditionJson += " and (t.CheckedDepartID is not null and datatype=0)";
            }
            //所属部门
            if (!queryParam["code"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.belongdept='{0}' ", queryParam["code"].ToString());
            }
            //安全检查类型
            if (!queryParam["ctype"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.CHECKDATATYPE='{0}'", queryParam["ctype"].ToString());
            }
            if (!queryParam["rId"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.rid='{0}'", queryParam["rId"].ToString());
            }
            if (!queryParam["recId"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.dutyuserid='{0}'", queryParam["recId"].ToString());
            }
            if (!queryParam["status"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.status={0}", queryParam["status"].ToString());
            }
            if (!queryParam["indexData"].IsEmpty())
            {
                if (queryParam["indexData"].ToString() == "0")
                {
                    pagination.conditionJson += string.Format(" and status=0 and datatype=1", queryParam["indexData"].ToString());
                }
            }
            if (!queryParam["dataType"].IsEmpty())
            {
                if (queryParam["dataType"].ToString() == "10")
                {
                    pagination.conditionJson += string.Format(" and (datatype=1 or datatype=2)", queryParam["dataType"].ToString());
                }
                else
                {
                    pagination.conditionJson += string.Format(" and datatype={0}", queryParam["dataType"].ToString());
                }

            }
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            int isWz = BaseRepository().FindObject(string.Format("select count(1) from  base_appsettingassociation a where a.deptid='{0}' and a.moduleid in(select id from BASE_MENUCONFIG t where t.modulecode='PECCANCY_ADD')", user.OrganizeId)).ToInt();
            int isWt = BaseRepository().FindObject(string.Format("select count(1) from  base_appsettingassociation a where a.deptid='{0}' and a.moduleid in(select id from BASE_MENUCONFIG t where t.modulecode='QUESTION_ADD')", user.OrganizeId)).ToInt();
            DataTable dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            foreach (DataRow r in dt.Rows)
            {
                if (!queryParam["pall"].IsEmpty())
                {
                    string mode = queryParam["pall"].ToString();
                    string ids = GetCheckIds(r["ID"].ToString());
                    //if (mode == "0")
                    //{

                        //计算所含隐患个数
                        int num = this.BaseRepository().FindObject(string.Format("select count(1) from bis_htbaseinfo o where o.safetycheckobjectid in('{1}') or safetycheckobjectid='{0}'", r["ID"].ToString(), ids.Replace(",", "','"))).ToInt();
                        r["Count"] = num;

                        decimal count1 = this.BaseRepository().FindObject(string.Format("select count(1) from bis_htbaseinfo o where workstream='整改结束' and (o.safetycheckobjectid in('{1}') or safetycheckobjectid='{0}')", r["ID"].ToString(), ids.Replace(",", "','"))).ToInt();
                        if (num > 0)
                        {
                            count1 = Math.Round(decimal.Parse((count1 / num).ToString()), 2);
                            r["Count1"] = count1 * 100;
                        }
                        else
                        {
                            r["Count1"] = 0;
                        }

                        num = this.BaseRepository().FindObject(string.Format("select count(1) from BIS_LLLEGALREGISTER where reseverone in('{1}') or reseverone='{0}'", r["ID"].ToString(), ids.Replace(",", "','"))).ToInt();
                        r["WzCount"] = num;
                        count1 = this.BaseRepository().FindObject(string.Format("select count(1) from BIS_LLLEGALREGISTER o where flowstate='流程结束' and  (reseverone in('{1}') or reseverone='{0}')", r["ID"].ToString(), ids.Replace(",", "','"))).ToInt();
                        if (num > 0)
                        {
                            count1 = Math.Round(decimal.Parse((count1 / num).ToString()), 2);
                            r["WzCount1"] = count1 * 100;
                        }
                        else
                        {
                            r["WzCount1"] = 0;
                        }
                        num = this.BaseRepository().FindObject(string.Format("select count(1) from BIS_QUESTIONINFO where CHECKID in('{1}') or CHECKID='{0}'", r["ID"].ToString(), ids.Replace(",", "','"))).ToInt();
                        r["WtCount"] = num;
                        count1 = this.BaseRepository().FindObject(string.Format("select count(1) from BIS_QUESTIONINFO o where flowstate='流程结束' and (CHECKID in('{1}') or CHECKID='{0}')", r["ID"].ToString(), ids.Replace(",", "','"))).ToInt();
                        if (num > 0)
                        {
                            count1 = Math.Round(decimal.Parse((count1 / num).ToString()), 2);
                            r["WtCount1"] = count1 * 100;
                        }
                        else
                        {
                            r["WtCount1"] = 0;
                        }
                    //}
                    //if (mode == "1")
                    //{
                    //    //计算所含隐患个数
                    //    int num = this.BaseRepository().FindObject(string.Format("select count(1) from bis_htbaseinfo o where o.safetycheckobjectid in('{0}') or safetycheckobjectid='{1}'", ids.Replace(",", "','"), r["ID"].ToString())).ToInt();
                    //    r["Count"] = num;
                    //    //违章数量
                    //    num = this.BaseRepository().FindObject(string.Format("select count(1) from BIS_LLLEGALREGISTER where reseverone in('{0}') or reseverone='{1}'", ids.Replace(",", "','"), r["ID"].ToString())).ToInt();
                    //    r["WzCount"] = num;

                    //    //问题数量
                    //    num = this.BaseRepository().FindObject(string.Format("select count(1) from BIS_QUESTIONINFO where CHECKID in('{0}') or CHECKID='{1}'", ids.Replace(",", "','"), r["ID"].ToString())).ToInt();
                    //    r["WtCount"] = num;
                    //}
                }
                else
                {
                    //计算所含隐患个数
                    int num = this.BaseRepository().FindObject("select count(1) from bis_htbaseinfo o where o.safetycheckobjectid='" + r["ID"].ToString() + "'").ToInt();
                    r["Count"] = num;
                    decimal count1 = this.BaseRepository().FindObject("select count(1) from bis_htbaseinfo o where workstream='整改结束' and  o.safetycheckobjectid='" + r["ID"].ToString() + "'").ToInt();
                    if (num > 0)
                    {
                        count1 = Math.Round(decimal.Parse((count1 / num).ToString()), 2);
                        r["Count1"] = count1 * 100;
                    }
                    else
                    {
                        r["Count1"] = 0;
                    }
                    //违章数量
                    num = this.BaseRepository().FindObject("select count(1) from BIS_LLLEGALREGISTER where reseverone='" + r["ID"].ToString() + "'").ToInt();
                    r["WzCount"] = num;
                    count1 = this.BaseRepository().FindObject("select count(1) from BIS_LLLEGALREGISTER o where flowstate='流程结束' and  o.reseverone='" + r["ID"].ToString() + "'").ToInt();
                    if (num > 0)
                    {
                        count1 = Math.Round(decimal.Parse((count1 / num).ToString()), 2);
                        r["WzCount1"] = count1 * 100;
                    }
                    else
                    {
                        r["WzCount1"] = 0;
                    }
                    //问题数量
                    num = this.BaseRepository().FindObject("select count(1) from BIS_QUESTIONINFO where CHECKID='" + r["ID"].ToString() + "'").ToInt();
                    r["WtCount"] = num;
                    count1 = this.BaseRepository().FindObject("select count(1) from BIS_QUESTIONINFO o where flowstate='流程结束' and  o.CHECKID='" + r["ID"].ToString() + "'").ToInt();
                    if (num > 0)
                    {
                        count1 = Math.Round(decimal.Parse((count1 / num).ToString()), 2);
                        r["WtCount1"] = count1 * 100;
                    }
                    else
                    {
                        r["WtCount1"] = 0;
                    }

                }
                DataTable dtRes = BaseRepository().FindTable(string.Format(@"select count(1) from bis_saftycheckdatadetailed t where t.recid='{0}' union all select count(1) from bis_saftycontent where recid='{0}'", r["ID"].ToString()));
                if (dtRes.Rows[0][0].ToString() != "0")
                {
                    decimal result = decimal.Parse(dtRes.Rows[1][0].ToString()) / decimal.Parse(dtRes.Rows[0][0].ToString());
                    r["SolveCount"] = Math.Round(result * 100, 2);
                }
                StringBuilder sb =new StringBuilder( string.Format("隐患数量:{0}", r["count"]));
                if(dt.Columns.Contains("title1"))
                {
                     if(isWz>0)
                     {
                         sb.AppendFormat(",违章数量:{0}",r["wzcount"]);
                     }
                     if (isWt > 0)
                     {
                         sb.AppendFormat(",问题数量:{0}", r["wtcount"]);
                     }
                     r["title1"] = sb.ToString();
                }
                sb = new StringBuilder(string.Format("隐患处理进度:{0}", r["count"].ToInt()==0?"-": r["count1"].ToString()+"%"));
                if (dt.Columns.Contains("title2"))
                {
                    if (isWz > 0)
                    {
                        sb.AppendFormat(",违章处理进度:{0}", r["wzcount"].ToInt() == 0 ? "-" : r["wzcount1"].ToString() + "%");
                    }
                    if (isWt > 0)
                    {
                        sb.AppendFormat(",问题处理进度:{0}%", r["wtcount"].ToInt() == 0 ? "-" : r["wtcount1"].ToString() + "%");
                    }
                    r["title2"] = sb.ToString();
                }

            }
            return dt;

        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SaftyCheckDataRecordEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SaftyCheckDataRecordEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 更改登记人
        /// </summary>
        public void RegisterPer(string userAccount, string id)
        {
            SaftyCheckDataRecordEntity entity = this.BaseRepository().FindEntity(id);
            if (entity != null)
            {
                string users = string.IsNullOrEmpty(entity.SolvePerson) ? "" : "|" + entity.SolvePerson;
                if (!users.Contains("|" + userAccount + "|"))
                {
                    users = users.Replace("|" + userAccount + "|", "") + userAccount + "|";
                    this.BaseRepository().ExecuteBySql("update bis_saftycheckdatarecord set SOLVEPERSON='" + users.TrimStart('|') + "',SolvePersonName=SolvePersonName||'" + OperatorProvider.Provider.Current().UserName + ",' where id='" + id + "'");
                }
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
            string sql = string.Format("begin\r\n delete from bis_saftycheckdatadetailed where recid='{0}';\r\n delete from bis_safenote where recid='{0}';\r\ncommit;\r\n end;", keyValue);
            this.BaseRepository().ExecuteBySql(sql);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public int SaveForm(string keyValue, SaftyCheckDataRecordEntity entity, ref string recid)
        {
            if (!string.IsNullOrEmpty(entity.CheckDeptCode))
            {
                entity.CheckDeptCode = entity.CheckDeptCode.Trim(',');
            }
            if (!string.IsNullOrEmpty(keyValue))
            {

                entity.ID = keyValue;
                var sd = BaseRepository().FindEntity(keyValue);
                if (sd != null)
                {
                    entity.Modify(keyValue);
                    return this.BaseRepository().Update(entity);

                }
                else
                {
                    entity.Create();
                    recid = entity.ID;
                    return this.BaseRepository().Insert(entity);

                }
            }
            else
            {
                entity.Create();
                recid = entity.ID;
                return this.BaseRepository().Insert(entity);
            }
        }

        /// <summary>
        /// 修改已检察人员
        /// </summary>
        public void UpdateCheckMan(string userAccount)
        {
            this.BaseRepository().ExecuteBySql("update BIS_SAFTYCHECKDATARECORD set SOLVEPERSON=SOLVEPERSON+'" + userAccount + "|',SolvePersonName=SolvePersonName||'" + OperatorProvider.Provider.Current().UserName + ",'");
        }
        #endregion

        #region 获取数据手机端
        public IEnumerable<SaftyCheckDataRecordEntity> GetSaftyDataList(string safeCheckTypeId, string searchString, Operator user, string deptCode = "")
        {
            List<SaftyCheckDataRecordEntity> listAll = new List<SaftyCheckDataRecordEntity>();
            string sql = "";
            if (searchString == "1")
            {
                sql = string.Format(@"select * from  bis_saftycheckdatarecord t where  id in(select a.recid from bis_saftycheckdatadetailed a where  instr((',' || a.checkmanid || ','),',{0},')>0 and  a.id not in(select b.detailid from BIS_SAFTYCONTENT b)
) ", user.Account);
                var listOther = this.BaseRepository().FindList(sql);//其他安全检查
                listAll.AddRange(listOther);
            }
            else
            {
                sql = string.Format(@"select * from bis_saftycheckdatarecord t  where checkdatatype='1' and datatype=0 and belongdept like '{0}%'", user.OrganizeCode);
                var list = this.BaseRepository().FindList(sql);//日常检查
                listAll.AddRange(list);
                sql = string.Format(@"select * from bis_saftycheckdatarecord t  where checkdatatype<>1 and datatype in(0,2) and (belongdept like '{0}%'
            or ',' || checkdeptcode like '%,{0}%' or createuserid='{1}' or checkeddepartid like '%{2}%')", user.OrganizeCode, user.UserId, user.OrganizeId);
                var listOther = this.BaseRepository().FindList(sql);//其他安全检查
                listAll.AddRange(listOther);
            }
            if (!string.IsNullOrEmpty(safeCheckTypeId))
            {
                if (safeCheckTypeId != "8x")
                {
                    listAll = listAll.FindAll(x => x.CheckDataType == int.Parse(safeCheckTypeId));
                }
                else
                {
                    listAll = listAll.FindAll(x => (x.CheckedDepartID == user.OrganizeId && (x.IsSynView == "1" || (x.IsSynView == "0" && x.CheckBeginTime.Value.Date <= DateTime.Now.Date))));
                }
            }
            return listAll.OrderByDescending(x => x.CreateDate);

        }
        public IEnumerable<SaftyCheckDataRecordEntity> GetSaftyDataList(string safeCheckTypeId, string searchString, Operator user, string deptCode, int page, int size, out int total)
        {
            Pagination pagination = new Pagination();
            pagination.sidx = "CreateDate desc,id";
            pagination.sord = "desc";
            pagination.page = page;
            pagination.rows = size;
            pagination.p_kid = "id";
            pagination.p_fields = "CheckBeginTime,checkendtime,CheckDataRecordName,CheckDataType,CheckedDepart,CheckDept,CheckedDepartID,CheckDeptID";
            pagination.p_tablename = "bis_saftycheckdatarecord t";
            string sql = " datatype in(0,2) ";
            if (!string.IsNullOrEmpty(safeCheckTypeId))
            {
                if (safeCheckTypeId != "8x")//上级单位安全检查
                {
                    sql += string.Format(" and CheckDataType={0}", safeCheckTypeId);
                }
                else
                {
                    sql += string.Format(" and CheckedDepartID='{0}' and (IsSynView='1' or (IsSynView='0' and CheckBeginTime<sysdate) )", user.OrganizeId);
                }
            }

            if (searchString == "1") //本人待办的
            {
                sql += string.Format(@" and id in(select a.recid from bis_saftycheckdatadetailed a where  instr((',' || a.checkmanid || ','),',{0},')>0 and  a.id not in(select b.detailid from BIS_SAFTYCONTENT b)
) ", user.Account);
            }
            else
            {
                sql += string.Format("  and (belongdept like '{0}%' or ',' || checkdeptcode like '%,{0}%' or createuserid='{1}' or checkeddepartid like '%{2}%')", user.OrganizeCode, user.UserId, user.OrganizeId);
            }

            pagination.conditionJson = sql;
            IEnumerable<SaftyCheckDataRecordEntity> listAll = BaseRepository().FindListByProcPager(pagination, DatabaseType.Oracle);
            total = pagination.records;
            return listAll;

        }
        public DataTable GetSaftyCheckDataList(string safeCheckTypeId, long status, Operator user, string deptCode, long page, long size, out int total, string startTime, string endTime)
        {
            Pagination pagination = new Pagination();
            pagination.sidx = "CreateDate desc,id";
            pagination.sord = "desc";
            pagination.page = int.Parse(page.ToString());
            pagination.rows = int.Parse(size.ToString());
            pagination.p_kid = "id ckid";
            pagination.p_fields = @"to_char(checkbegintime,'yyyy-mm-dd') starttime,to_char(checkendtime,'yyyy-mm-dd') endtime,CheckDataRecordName checkname,case when checkdatatype=1 then '日常安全检查' when checkdatatype=2 then '专项安全检查' when checkdatatype=3 then '节假日前后安全检查' when checkdatatype=4 then '季节性安全检查' when checkdatatype=5 then '综合安全检查' when checkdatatype=6 then '其他安全检查' else '上级单位安全检查' end checktype,checkdatatype checktypevalue,
case when CHECKLEVEL=1 then '公司级' when CHECKLEVEL=2 then '部门级' when CHECKLEVEL=3 then '专业级' when CHECKLEVEL=4 then '班组级' else '承包商自查' end checklevel,0 htcount,0 wzcount,0 wtcount,a.status,CreateDate";
            pagination.p_tablename = "bis_saftycheckdatarecord t left join V_SAFETYCHECKLIST a on t.id=a.checkid";
            string sql = " datatype in(0,2) ";
            if (!string.IsNullOrWhiteSpace(deptCode))
            {
                sql += string.Format(" and (',' || CheckDeptID || ',') like '%,{0},%'", deptCode);
            }
            if (!string.IsNullOrEmpty(safeCheckTypeId))
            {
                if (safeCheckTypeId != "8x")//上级单位安全检查
                {
                    sql += string.Format(" and CheckDataType={0}", safeCheckTypeId);
                }
                else
                {
                    sql += string.Format(" and CheckedDepartID='{0}' and (IsSynView='1' or (IsSynView='0' and CheckBeginTime<sysdate) )", user.OrganizeId);
                }
            }
            if (!string.IsNullOrWhiteSpace(startTime))
            {
                sql += string.Format(" and checkbegintime>=to_date('{0}','yyyy-mm-dd') ", startTime);
            }
            if (!string.IsNullOrWhiteSpace(endTime))
            {
                sql += string.Format(" and checkbegintime<=to_date('{0}','yyyy-mm-dd')", endTime);
            }
            DataTable dt = new DataTable();
            if (status == 1) //我的
            {
                string tabName = "bis_saftycheckdatarecord t left join V_RCSAFETYCHECKLIST a on t.id=a.checkid";
                string sql1 = string.Format("select * from (select {0} from {1} where {2} union all select {0} from {4} where {3}) t order by CreateDate desc", pagination.p_kid + "," + pagination.p_fields, pagination.p_tablename, sql + string.Format(@" and a.status in(0,1) and id in(select a.recid from bis_saftycheckdatadetailed a where  instr((',' || a.checkmanid || ','),',{0},')>0 and  a.id not in(select b.detailid from BIS_SAFTYCONTENT b))", user.Account), sql + string.Format(" and checkdatatype=1 and datatype in(0,2) and instr(('|' || t.SolvePerson),'|{0}|')=0 and a.status in(0,1) and (t.createuserid='{1}' or instr((',' || t.checkmanid || ','),',{0},') >0)", user.Account, user.UserId), tabName);
                dt = BaseRepository().FindTable(sql1);
                total = dt.Rows.Count;
            }
            else
            {
                sql += string.Format("  and (belongdept like '{0}%' or ',' || checkdeptcode like '%,{0}%' or createuserid='{1}' or checkeddepartid like '%{2}%')", user.OrganizeCode, user.UserId, user.OrganizeId);
                pagination.conditionJson = sql;
                dt = BaseRepository().FindTableByProcPager(pagination, DatabaseType.Oracle);
                total = pagination.records;
            }
            return dt;

        }
        public SaftyCheckDataRecordEntity getSaftyCheckDataRecordEntity(string safeCheckIdItem)
        {
            SaftyCheckDataRecordEntity entity = this.BaseRepository().FindEntity(safeCheckIdItem);
            return entity;
        }

        public DataTable getCheckRecordDetail(string safeCheckIdItem, string riskPointId)
        {
            string sql = string.Format(@"select a.account,a.modifydate,a.createuserdeptcode,a.createuserorgcode,a.createuserid,  a.checktypename,a.hidtypename,a.hidrankname,a.checkdepartname,a.isgetafter,
                                        a.hiddepartname,a.id as hiddenid ,a.createdate,a.hidcode as problemid ,a.hiddangername,a.hidproject,a.checkdate,a.checkdepart,a.checktype,a.checknumber,
                                        a.isbreakrule,a.breakruleuserids,a.breakruleusernames,a.traintemplateid ,
                                        a.traintemplatename,a.hidtype,a.hidrank,a.hidplace,a.hidpoint,a.workstream ,a.addtype ,b.participant,c.applicationstatus ,
                                        c.postponedept ,c.postponedeptname ,a.hiddescribe,c.changemeasure,
                                       (case when f.filepath is not null then ('{0}'||substr(f.filepath,2)) else '' end) as filepath   from v_htbaseinfo a
                                        left join v_workflow b on a.id = b.id 
                                        left join v_htchangeinfo c on a.hidcode = c.hidcode
                                        left join v_htacceptinfo d on a.hidcode = d.hidcode
                                        left join v_imageview f on a.hidphoto = f.recid  
                                        where 1=1 ", new DataItemDetailService().GetItemValue("imgUrl"));
            if (!string.IsNullOrEmpty(safeCheckIdItem))
            {
                sql += string.Format(" and a.safetycheckobjectid = '{0}'", safeCheckIdItem);
            }
            if (!string.IsNullOrEmpty(riskPointId))
            {
                sql += string.Format(" and a.hidpoint = '{0}'", riskPointId);
            }

            DataTable dt = this.BaseRepository().FindTable(sql);
            return dt;
        }

        public int addDailySafeCheck(SaftyCheckDataRecordEntity se, Operator user)
        {
            se.insertInto(se, user);
            se.BelongDept = user.DeptCode;
            se.BelongDeptID = user.DeptId;
            return this.BaseRepository().Insert(se);
        }

        public DataTable selectCheckPerson(Operator user)
        {
            //权限判断

            //
            //
            var strSql = new StringBuilder();
            strSql.Append(@"SELECT u.userid as nameId,u.account as account,
                                    u.realname as name ,
                                    case when u.departmentid is null then o.organizeid else u.departmentid end  parentId,
                                    u.departmentid,
                                    d.fullname AS deptname ,
                                    d.ENCODE as deptcode
                            FROM    Base_User u
                                    LEFT JOIN Base_Organize o ON o.OrganizeId = u.OrganizeId
                                    LEFT JOIN Base_Department d ON d.DepartmentId = u.DepartmentId
                            WHERE   1=1");
            strSql.Append(" AND u.UserId <> 'System' AND u.EnabledMark = 1 AND u.DeleteMark=0 ");
            return this.BaseRepository().FindTable(strSql.ToString());
        }


        #region 首页待办列表
        public List<SaftyCheckDataRecordEntity> GetSaftDataIndexList(ERCHTMS.Code.Operator user)
        {
            List<SaftyCheckDataRecordEntity> listAll = new List<SaftyCheckDataRecordEntity>();
            string sql = string.Format(@"select * from bis_saftycheckdatarecord t where 
 id in(select distinct  a.recid from bis_saftycheckdatadetailed a left join bis_saftycontent b on a.id=b.detailid where checkDataType<>1 and b.id is null and (',' || a.CheckManid || ',') like '%,{0},%'
) ", user.Account);
            var list = this.BaseRepository().FindList(sql).ToList();
            listAll.AddRange(list);
            string sqlRc = string.Format(@"select * from bis_saftycheckdatarecord where checkDataType='1' and  CheckManid like '%{0}%'  and ((instr(solveperson,'{0}'))=0 or solveperson is null)  and belongdept like  '{1}%' ", user.Account, user.OrganizeCode, user.UserId);
            var list1 = this.BaseRepository().FindList(sqlRc).ToList();
            listAll.AddRange(list1);
            return listAll;
        }
        #endregion
        #endregion

        /**/
        /// <summary>
        /// 得到一年中的某周的起始日和截止日
        /// 年 nYear
        /// 周数 nNumWeek
        /// 周始 out dtWeekStart
        /// 周终 out dtWeekeEnd
        /// </summary>
        /// <param name="nYear"></param>
        /// <param name="nNumWeek"></param>
        /// <param name="dtWeekStart"></param>
        /// <param name="dtWeekeEnd"></param>
        public static void GetWeek(int nYear, int nNumWeek, out DateTime dtWeekStart, out DateTime dtWeekeEnd)
        {
            DateTime dt = new DateTime(nYear, 1, 1);
            dt = dt + new TimeSpan((nNumWeek - 1) * 7, 0, 0, 0);
            dtWeekStart = dt.AddDays(-(int)dt.DayOfWeek + (int)DayOfWeek.Monday);
            dtWeekeEnd = dt.AddDays((int)DayOfWeek.Saturday - (int)dt.DayOfWeek + 1);
        }

        /**/
        /// <summary>
        /// 求当前日期是一年的中第几周
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int WeekOfYear(DateTime curDay)
        {
            int firstdayofweek = Convert.ToInt32(Convert.ToDateTime(curDay.Year.ToString() + "- " + "1-1 ").DayOfWeek);
            int days = curDay.DayOfYear;
            int daysOutOneWeek = days - (7 - firstdayofweek);
            if (daysOutOneWeek <= 0)
            {
                return 1;
            }
            else
            {
                int weeks = daysOutOneWeek / 7;
                if (daysOutOneWeek % 7 != 0)
                    weeks++;
                return weeks + 1;
            }
        }

        /// <summary>
        /// 首页安全检查提醒
        /// </summary>
        /// <param name="user">当前登录人user</param>
        /// <returns></returns>
        public DataTable GetSafeCheckWarning(ERCHTMS.Code.Operator user, string mode = "0")
        {
            string sqlEx = string.Format("select count(1) from BIS_CLASSIFICATIONINDEX where affiliatedorganizeid='{0}'", user.OrganizeId);
            int countEx = this.BaseRepository().FindObject(sqlEx).ToInt();

            var sql = string.Format("select indexname,indexscore,IndexStandard,indexargsvalue,0 kscore,0 score,indexcode from  bis_classificationindex  where classificationcode='01'");
            if (countEx == 0)
            {
                sql += " and affiliatedorganizeid='0' order by indexcode";
            }
            else
            {
                sql += " and affiliatedorganizeid='" + user.OrganizeId + "' order by indexcode";
            }
            DataTable dt = this.BaseRepository().FindTable(sql);

            //扣分总数
            double selPoint = 0;
            //实际次数
            int selCount = 0;

            //条件
            string sqlWhere = "select count(id) from bis_saftycheckdatarecord where 1=1 and CREATEUSERORGCODE='" + user.OrganizeCode + "'";
            foreach (DataRow dr in dt.Rows)
            {
                var indexCode = dr["indexcode"].ToString();
                double allScorePoint = Convert.ToDouble(dr["indexscore"].ToString());
                //未执行安全检查计划次数
                if (indexCode == "00")
                {
                    sql = string.Format(@"select count(1) as num from Bis_Saftycheckdatarecord where CheckDataType!='1' and  to_char(CheckEndTime,'yyyy')='{0}' and solvePerson is null and to_char(checkEndTime,'yyyy-MM-dd')<'{1}' and  CreateUserOrgCode='{2}'", DateTime.Now.Year, DateTime.Now.ToString("yyyy-MM-dd"), user.OrganizeCode);
                    if (DbHelper.DbType == DatabaseType.MySql)
                    {
                        sql = string.Format(@"select count(1) as num from Bis_Saftycheckdatarecord where CheckDataType!='1' and  date_format(CheckEndTime,'%Y')='{0}' and solvePerson is null and  checkEndTime<'{1}' and  CreateUserOrgCode='{2}'", DateTime.Now.Year, DateTime.Now.ToString("yyyy-MM-dd"), user.OrganizeCode);
                    }
                    var count = this.BaseRepository().FindObject(sql).ToInt();
                    double allkScore = 0;
                    if (count > 0)
                    {
                        allkScore = count * double.Parse(dr["indexargsvalue"].ToString());//设定分值
                    }
                    //dr["indexscore"]总分,allkScore总扣分
                    allkScore = allkScore >= allScorePoint ? allScorePoint : allkScore;
                    dr["kscore"] = allkScore;
                    dr["score"] = allScorePoint - allkScore;
                }
                //公司级检查
                else if (indexCode == "03")
                {
                    #region 公司级检查
                    double gsselPoint = 0;
                    //次数
                    int everyTime = Convert.ToInt32(dr["indexargsvalue"].ToString().Split('|')[1]);
                    //每次扣分
                    double everyPoint = 0;
                    if (dr["indexargsvalue"].ToString().Split('|').Length > 2)
                    {
                        everyPoint = Convert.ToInt32(dr["indexargsvalue"].ToString().Split('|')[2]);
                    }
                    //总分

                    //周/月/季度/半年/年
                    string everyDate = dr["indexargsvalue"].ToString().Split('|')[0];
                    sqlWhere += " and createuserid in (select USERID from base_user where (instr(rolename,'厂级部门用户')>0 or instr(rolename,'公司级用户')>0))";
                    sqlWhere += " and CHECKLEVEL=1";
                    switch (everyDate)
                    {
                        case "周":

                            int nowWeek = WeekOfYear(DateTime.Now);
                            DateTime firstDay = DateTime.Now;
                            DateTime endDay = DateTime.Now;
                            for (int i = 1; i <= nowWeek; i++)
                            {
                                string sqlWeek = sqlWhere;
                                GetWeek(DateTime.Now.Year, i, out firstDay, out endDay);
                                if (DbHelper.DbType == DatabaseType.MySql)
                                {
                                    sqlWeek += " and createdate between '" + firstDay + "' and  '" + endDay + "'";
                                }
                                else
                                {
                                    sqlWeek += " and createdate between to_date('" + firstDay + "','yyyy-mm-dd hh24:mi:ss') and to_date('" + endDay + "','yyyy-mm-dd hh24:mi:ss'）";
                                }
                                selCount += this.BaseRepository().FindObject(sqlWeek).ToInt(); ;
                                if (selCount < everyTime)
                                    gsselPoint += everyPoint * (everyTime - selCount);
                            }
                            break;
                        case "月":
                            int cMonth = DateTime.Now.Month;
                            for (int i = 1; i <= cMonth; i++)
                            {
                                string sqlMonth = sqlWhere;
                                if (i == 12)
                                {
                                    if (DbHelper.DbType == DatabaseType.MySql)
                                    {
                                        sqlMonth += " and createdate>='" + DateTime.Now.Year + "-" + i + "-1' and createdate<'" + DateTime.Now.Year + "-" + i + "-31'";
                                    }
                                    else
                                    {
                                        sqlMonth += " and createdate>=to_date('" + DateTime.Now.Year + "-" + i + "-1','yyyy-mm-dd hh24:mi:ss') and createdate<to_date('" + DateTime.Now.Year + "-" + i + "-31','yyyy-mm-dd hh24:mi:ss')";
                                    }

                                }
                                else
                                {
                                    if (DbHelper.DbType == DatabaseType.MySql)
                                    {
                                        sqlMonth += " and createdate>='" + DateTime.Now.Year + "-" + i + "-1' and createdate<'" + DateTime.Now.Year + "-" + (i + 1) + "-1'";
                                    }
                                    else
                                    {
                                        sqlMonth += " and createdate>=to_date('" + DateTime.Now.Year + "-" + i + "-1','yyyy-mm-dd hh24:mi:ss') and createdate<to_date('" + DateTime.Now.Year + "-" + (i + 1) + "-1','yyyy-mm-dd hh24:mi:ss')";
                                    }
                                }

                                selCount = this.BaseRepository().FindObject(sqlMonth).ToInt(); ;
                                if (selCount < everyTime)
                                    gsselPoint += everyPoint * (everyTime - selCount);
                            }
                            break;
                        case "季度":
                            int e = 1;
                            if (DateTime.Now.Month < 4) { e = 1; }
                            else
                                if (DateTime.Now.Month < 7) { e = 2; }
                                else
                                    if (DateTime.Now.Month < 10) { e = 3; }
                                    else
                                        if (DateTime.Now.Month <= 12) { e = 4; }


                            for (int i = 0; i < e; i++)
                            {
                                string sqlSeaon = sqlWhere;
                                string a = "";
                                if ((i + 1) * 3 + 1 > 12) a = DateTime.Now.Year.ToString() + "-12-31";
                                else a = DateTime.Now.Year + "-" + ((i + 1) * 3 + 1).ToString() + "-1";
                                if (DbHelper.DbType == DatabaseType.MySql)
                                {
                                    sqlSeaon += " and createdate>='" + DateTime.Now.Year + "-" + (i * 3 == 0 ? 1 : i * 3 + 1) + "-1' and createdate<'" + a + "'";
                                }
                                else
                                {
                                    sqlSeaon += " and createdate>=to_date('" + DateTime.Now.Year + "-" + (i * 3 == 0 ? 1 : i * 3 + 1) + "-1','yyyy-mm-dd hh24:mi:ss') and createdate<to_date('" + a + "','yyyy-mm-dd hh24:mi:ss')";
                                }

                                selCount = this.BaseRepository().FindObject(sqlSeaon).ToInt(); ;
                                if (selCount < everyTime)
                                    gsselPoint += everyPoint * (everyTime - selCount);
                            }
                            break;
                        case "半年":
                            int chalfYear = DateTime.Now.Month;
                            if (chalfYear <= 6)
                            {
                                string sqlhalfYear = sqlWhere;
                                if (DbHelper.DbType == DatabaseType.MySql)
                                {
                                    sqlhalfYear += " and createdate>='" + DateTime.Now.Year + "-1-1' and createdate<'" + DateTime.Now.Year + "-7-1'";
                                }
                                else
                                {
                                    sqlhalfYear += " and createdate>=to_date('" + DateTime.Now.Year + "-1-1','yyyy-mm-dd hh24:mi:ss') and createdate<to_date('" + DateTime.Now.Year + "-7-1','yyyy-mm-dd hh24:mi:ss')";
                                }

                                selCount = this.BaseRepository().FindObject(sqlhalfYear).ToInt(); ;
                                if (selCount < everyTime)
                                    gsselPoint += everyPoint * (everyTime - selCount);
                            }
                            else
                            {
                                string sqlhalfYear = sqlWhere, sqlhalfYear2 = sqlWhere;
                                if (DbHelper.DbType == DatabaseType.MySql)
                                {
                                    sqlhalfYear += " and createdate>='" + DateTime.Now.Year + "-1-1' and createdate<'" + DateTime.Now.Year + "-7-1'";
                                }
                                else
                                {
                                    sqlhalfYear += " and createdate>=to_date('" + DateTime.Now.Year + "-1-1','yyyy-mm-dd hh24:mi:ss') and createdate<to_date('" + DateTime.Now.Year + "-7-1','yyyy-mm-dd hh24:mi:ss')";
                                }

                                selCount = this.BaseRepository().FindObject(sqlhalfYear).ToInt(); ;
                                if (selCount < everyTime)
                                {
                                    gsselPoint += everyPoint * (everyTime - selCount);
                                }
                                if (DbHelper.DbType == DatabaseType.MySql)
                                {
                                    sqlhalfYear2 += " and createdate>='" + DateTime.Now.Year + "-7-1' and createdate<'" + DateTime.Now.Year + "-12-31'";
                                }
                                else
                                {
                                    sqlhalfYear2 += " and createdate>=to_date('" + DateTime.Now.Year + "-7-1','yyyy-mm-dd hh24:mi:ss') and createdate<to_date('" + DateTime.Now.Year + "-12-31','yyyy-mm-dd hh24:mi:ss')";
                                }

                                selCount = this.BaseRepository().FindObject(sqlhalfYear2).ToInt(); ;
                                if (selCount < everyTime)
                                    gsselPoint += everyPoint * (everyTime - selCount);
                            }
                            break;
                        case "年":
                            if (DbHelper.DbType == DatabaseType.MySql)
                            {
                                sqlWhere += "and createdate between '" + DateTime.Now.Year + "-1-1' and '" + DateTime.Now.Year + "-12-31'";
                            }
                            else
                            {
                                sqlWhere += "and createdate between to_date('" + DateTime.Now.Year + "-1-1','yyyy-mm-dd hh24:mi:ss') and to_date('" + DateTime.Now.Year + "-12-31','yyyy-mm-dd hh24:mi:ss')";
                            }

                            selCount = this.BaseRepository().FindObject(sqlWhere).ToInt(); ;
                            if (selCount < everyTime)
                                gsselPoint += everyPoint * (everyTime - selCount);
                            break;
                        default: break;
                    }
                    gsselPoint = Double.Parse((gsselPoint > allScorePoint ? allScorePoint : gsselPoint).ToString("f1"));
                    dr["kscore"] = gsselPoint.ToString();
                    dr["score"] = (Convert.ToDouble(dr["indexscore"]) - gsselPoint).ToString();
                    #endregion
                }
                //部门检查次数
                else if (indexCode == "04")
                {
                    double bmselPoint = 0;
                    //第几周
                    int nowWeek = WeekOfYear(DateTime.Now);

                    int timet = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(DateTime.Now.Day / 7) + 1));
                    DateTime firstDay = DateTime.Now;
                    DateTime endDay = DateTime.Now;
                    int allDeptCount = this.BaseRepository().FindTable("select * from BASE_DEPARTMENT t where t.organizeid='" + user.OrganizeId + "' and isorg!='1' and (DESCRIPTION!='外包工程承包商' or DESCRIPTION is null) and nature='部门'").Rows.Count;
                    string sqlFor = "and  CREATEUSERORGCODE='" + user.OrganizeCode + "' and   CHECKLEVEL=2 and createuserid in (select USERID from base_user where instr(rolename,'商级')=0  and length(DEPARTMENTCODE)>3)";
                    for (int i = 0; i < timet; i++)
                    {
                        GetWeek(DateTime.Now.Year, nowWeek - i, out firstDay, out endDay);
                        string sqlWeek = sqlWhere;
                        string sqlDept = " ";
                        if (DbHelper.DbType == DatabaseType.MySql)
                        {
                            sqlWeek = "  and createdate between '" + firstDay + "' and '" + endDay + "'";
                            sqlDept = "select substr(CREATEUSERDEPTCODE,1,6) from (select *from bis_saftycheckdatarecord where 1=1 " + sqlWeek + sqlFor + ")  group by substr(CREATEUSERDEPTCODE,1,6)  ";

                        }
                        else
                        {
                            sqlWeek = "  and createdate between to_date('" + firstDay + "','yyyy-mm-dd hh24:mi:ss') and to_date('" + endDay + "','yyyy-mm-dd hh24:mi:ss'）";
                            sqlDept = "select substr(CREATEUSERDEPTCODE,0,6) from (select *from bis_saftycheckdatarecord where 1=1 " + sqlWeek + sqlFor + ")  group by substr(CREATEUSERDEPTCODE,0,6)  ";
                        }
                        selCount = this.BaseRepository().FindTable(sqlDept).Rows.Count;
                        if (selCount < allDeptCount)
                            bmselPoint += 2;
                    }
                    bmselPoint = Double.Parse((bmselPoint > allScorePoint ? allScorePoint : bmselPoint).ToString("f1"));
                    dr["kscore"] = bmselPoint.ToString();
                    dr["score"] = (Convert.ToInt32(dr["indexscore"]) - bmselPoint).ToString();
                }
                //班组检查次数
                else if (indexCode == "05")
                {
                    string[] arr = dr["indexargsvalue"].ToString().Split('|');
                    int everyNum = Convert.ToInt32(arr[0]);
                    double everyScore = Convert.ToDouble(arr[1]);
                    int allDayOfM = DateTime.Now.Day;
                    double allkScore = 0;
                    DateTime fristDay = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM") + "-01");
                    for (int i = 0; i < allDayOfM; i++)
                    {
                        sql = string.Format(@"select count(encode) as cou,nvl(sum(nvl(sum(daynum),0)),0) as allnum from base_department a 
left join (select createuserdeptcode, 1 daynum from (select createuserdeptcode,count(1) as num  from  Bis_Saftycheckdatarecord  
where to_char(checkBeginTime,'yyyy-MM-dd')='{0}' and checkLevel='4' group by createuserdeptcode) where num>={1} ) b on  a.encode=b.createuserdeptcode
where nature='班组' and organizeId='{2}' group by encode", fristDay.ToString("yyyy-MM-dd"), everyNum, user.OrganizeId);
                        if (DbHelper.DbType == DatabaseType.MySql)
                        {
                            sql = string.Format(@"select count(encode) as cou,case when isnull(sum(daynum)) then 0 else sum(daynum) end as allnum from base_department a 
left join (select createuserdeptcode, 1 daynum from (select createuserdeptcode,count(1) as num  from  Bis_Saftycheckdatarecord  
where date_format(checkBeginTime,'%Y-%m-%d')='{0}' and checkLevel='4' group by createuserdeptcode) where num>={1} ) b on  a.encode=b.createuserdeptcode
where nature='班组' and organizeId='{2}' group by encode", fristDay.ToString("yyyy-MM-dd"), everyNum, user.OrganizeId);
                        }
                        DataTable dtDayNum = this.BaseRepository().FindTable(sql);
                        if (dtDayNum.Rows.Count > 0)
                        {
                            allkScore += int.Parse(dtDayNum.Rows[0][0].ToString()) > int.Parse(dtDayNum.Rows[0][1].ToString()) ? everyScore : 0;
                        }
                        if (allkScore >= allScorePoint)
                            break;
                        fristDay = fristDay.AddDays(1);
                    }
                    allkScore = allkScore >= allScorePoint ? allScorePoint : allkScore;
                    dr["kscore"] = allkScore;
                    dr["score"] = allScorePoint - allkScore;

                }
            }
            return dt;
        }

        /// <summary>
        /// 获取首页安全检查考核结果数据
        /// </summary>
        /// <param name="user">当前登录人user</param>
        /// <param name="time">统计时间</param>
        /// <returns></returns>
        public object GetSafeCheckWarningByTime(ERCHTMS.Code.Operator user, string time, int mode = 0)
        {
            string sqlEx = string.Format("select count(1) from BIS_CLASSIFICATIONINDEX where affiliatedorganizeid='{0}'", user.OrganizeId);
            int countEx = this.BaseRepository().FindObject(sqlEx).ToInt();

            var sql = string.Format("select indexname,indexscore,IndexStandard,indexargsvalue,0 kscore,0 score,indexcode from  bis_classificationindex  where classificationcode='02'");
            if (countEx == 0)
            {
                sql += " and affiliatedorganizeid='0' order by indexcode";
            }
            else
            {
                sql += " and affiliatedorganizeid='" + user.OrganizeId + "' order by indexcode";
            }
            DataTable dt = this.BaseRepository().FindTable(sql);

            //扣分总数

            //实际次数
            int selCount = 0;

            double totalScore = 0;//应得总分
            decimal finalScore = 0;//实际总得分
            decimal weight = 0;
            object obj = this.BaseRepository().FindObject("select t.weightcoeffcient from BIS_CLASSIFICATION t where t.affiliatedorganizeid=@orgId", new DbParameter[] { DbParameters.CreateDbParameter("@orgId", user.OrganizeId) });
            if (obj != null && obj != DBNull.Value)
            {
                weight = obj.ToDecimal();
            }

            DateTime checkTime = DateTime.Parse(time).AddMonths(1).AddDays(-1);
            //条件
            string sqlWhere = "select count(id) from bis_saftycheckdatarecord where 1=1 and CREATEUSERORGCODE='" + user.OrganizeCode + "'";
            foreach (DataRow dr in dt.Rows)
            {
                string startDate = mode == 0 ? checkTime.ToString("yyyy-MM-01 00:00:01") : checkTime.ToString("yyyy-01-01 00:00:01");
                var indexCode = dr["indexcode"].ToString();
                double allScorePoint = Convert.ToDouble(dr["indexscore"].ToString());
                //未执行安全检查计划次数
                if (indexCode == "01")
                {
                    sql = string.Format(@"select count(1) as num from Bis_Saftycheckdatarecord where CheckDataType!='1' and  to_char(CheckEndTime,'yyyy-mm-dd hh24:mi:ss')>'{0}' and solvePerson is null and to_char(checkEndTime,'yyyy-MM-dd')<'{1}' and  CreateUserOrgCode='{2}'", startDate, checkTime.ToString("yyyy-MM-dd 23:59:59"), user.OrganizeCode);
                    if (DbHelper.DbType == DatabaseType.MySql)
                    {
                        sql = string.Format(@"select count(1) as num from Bis_Saftycheckdatarecord where CheckDataType!='1' and  CheckEndTime>'{0}' and solvePerson is null and checkEndTime<'{1}' and  CreateUserOrgCode='{2}'", startDate, checkTime.ToString("yyyy-MM-dd 23:59:59"), user.OrganizeCode);
                    }
                    var count = this.BaseRepository().FindObject(sql).ToInt();
                    var allkScore = count * double.Parse(dr["indexargsvalue"].ToString());//设定分值
                    //dr["indexscore"]总分,allkScore总扣分
                    allkScore = allkScore >= allScorePoint ? allScorePoint : allkScore;
                    dr["kscore"] = allkScore;
                    dr["score"] = allScorePoint - allkScore;
                }
                //公司级检查
                else if (indexCode == "02")
                {
                    #region 公司级检查
                    double gsselPoint = 0;
                    //次数
                    int everyTime = Convert.ToInt32(dr["indexargsvalue"].ToString().Split('|')[1]);
                    //每次扣分扣分
                    double everyPoint = Convert.ToInt32(dr["indexargsvalue"].ToString().Split('|')[2]);
                    //总分

                    //周/月/季度/半年/年
                    string everyDate = dr["indexargsvalue"].ToString().Split('|')[0];

                    if (DbHelper.DbType == DatabaseType.MySql)
                    {
                        sqlWhere += " and createuserid in (select USERID from base_user where (instr(rolename,'厂级部门用户')>1 or instr(rolename,'公司级用户')>1))";
                    }
                    if (DbHelper.DbType == DatabaseType.Oracle)
                    {
                        sqlWhere += " and createuserid in (select USERID from base_user where (instr(rolename,'厂级部门用户')>0 or instr(rolename,'公司级用户')>0))";
                    }
                    sqlWhere += " and CHECKLEVEL=1";
                    switch (everyDate)
                    {
                        case "周":
                            int nowWeek = mode == 0 ? WeekOfYear(DateTime.Parse(time)) : WeekOfYear(checkTime);
                            DateTime firstDay = DateTime.Parse(time);
                            DateTime endDay = checkTime;
                            int weeks = mode == 0 ? 5 : nowWeek;
                            for (int i = 1; i <= weeks; i++)
                            {
                                string sqlWeek = sqlWhere;
                                int count = mode == 0 ? i + nowWeek - 1 : i;
                                GetWeek(checkTime.Year, count, out firstDay, out endDay);
                                if (DbHelper.DbType == DatabaseType.MySql)
                                {
                                    sqlWeek += " and createdate between '" + firstDay + "' and '" + endDay + "'";
                                }
                                if (DbHelper.DbType == DatabaseType.Oracle)
                                {
                                    sqlWeek += " and createdate between to_date('" + firstDay + "','yyyy-mm-dd hh24:mi:ss') and to_date('" + endDay + "','yyyy-mm-dd hh24:mi:ss'）";
                                }

                                selCount = this.BaseRepository().FindObject(sqlWeek).ToInt();
                                if (mode == 1)
                                {
                                    selCount += selCount;
                                }
                                if (selCount < everyTime)
                                    gsselPoint += everyPoint * (everyTime - selCount);
                            }
                            break;
                        case "月":
                            int cMonth = checkTime.Month;
                            for (int i = 1; i <= cMonth; i++)
                            {
                                string sqlMonth = sqlWhere;
                                if (i == 12)
                                {
                                    if (DbHelper.DbType == DatabaseType.MySql)
                                    {
                                        sqlMonth += " and createdate>='" + checkTime.Year + "-" + i + "-1' and createdate<'" + checkTime.Year + "-" + i + "-31'";
                                    }
                                    if (DbHelper.DbType == DatabaseType.Oracle)
                                    {
                                        sqlMonth += " and createdate>=to_date('" + checkTime.Year + "-" + i + "-1','yyyy-mm-dd hh24:mi:ss') and createdate<to_date('" + checkTime.Year + "-" + i + "-31','yyyy-mm-dd hh24:mi:ss')";
                                    }


                                }
                                else
                                {
                                    if (DbHelper.DbType == DatabaseType.MySql)
                                    {
                                        sqlMonth += " and createdate>='" + checkTime.Year + "-" + i + "-1' and createdate<'" + checkTime.Year + "-" + (i + 1) + "-1'";
                                    }
                                    if (DbHelper.DbType == DatabaseType.Oracle)
                                    {
                                        sqlMonth += " and createdate>=to_date('" + checkTime.Year + "-" + i + "-1','yyyy-mm-dd hh24:mi:ss') and createdate<to_date('" + checkTime.Year + "-" + (i + 1) + "-1','yyyy-mm-dd hh24:mi:ss')";
                                    }
                                }

                                selCount = this.BaseRepository().FindObject(sqlMonth).ToInt(); ;
                                if (selCount < everyTime)
                                    gsselPoint += everyPoint * (everyTime - selCount);
                            }
                            break;
                        case "季度":
                            int e = 1;
                            if (checkTime.Month < 4) { e = 1; }
                            else
                                if (checkTime.Month < 7) { e = 2; }
                                else
                                    if (checkTime.Month < 10) { e = 3; }
                                    else
                                        if (checkTime.Month <= 12) { e = 4; }


                            for (int i = 0; i < e; i++)
                            {
                                string sqlSeaon = sqlWhere;
                                string a = "";
                                if ((i + 1) * 3 + 1 > 12) a = checkTime.Year.ToString() + "-12-31";
                                else a = checkTime.Year + "-" + ((i + 1) * 3 + 1).ToString() + "-1";
                                if (DbHelper.DbType == DatabaseType.MySql)
                                {
                                    sqlSeaon += " and createdate>='" + checkTime.Year + "-" + (i * 3 == 0 ? 1 : i * 3 + 1) + "-1' and createdate<'" + a + "'";
                                }
                                if (DbHelper.DbType == DatabaseType.Oracle)
                                {
                                    sqlSeaon += " and createdate>=to_date('" + checkTime.Year + "-" + (i * 3 == 0 ? 1 : i * 3 + 1) + "-1','yyyy-mm-dd hh24:mi:ss') and createdate<to_date('" + a + "','yyyy-mm-dd hh24:mi:ss')";
                                }


                                selCount = this.BaseRepository().FindObject(sqlSeaon).ToInt(); ;
                                if (selCount < everyTime)
                                    gsselPoint += everyPoint * (everyTime - selCount);
                            }
                            break;
                        case "半年":
                            int chalfYear = checkTime.Month;
                            if (chalfYear <= 6)
                            {
                                string sqlhalfYear = sqlWhere;

                                if (DbHelper.DbType == DatabaseType.MySql)
                                {
                                    sqlhalfYear += " and createdate>='" + checkTime.Year + "-1-1' and createdate<'" + checkTime.Year + "-7-1'";
                                }
                                if (DbHelper.DbType == DatabaseType.Oracle)
                                {
                                    sqlhalfYear += " and createdate>=to_date('" + checkTime.Year + "-1-1','yyyy-mm-dd hh24:mi:ss') and createdate<to_date('" + checkTime.Year + "-7-1','yyyy-mm-dd hh24:mi:ss')";
                                }


                                selCount = this.BaseRepository().FindObject(sqlhalfYear).ToInt(); ;
                                if (selCount < everyTime)
                                    gsselPoint += everyPoint * (everyTime - selCount);
                            }
                            else
                            {
                                string sqlhalfYear = sqlWhere, sqlhalfYear2 = sqlWhere;
                                if (DbHelper.DbType == DatabaseType.MySql)
                                {
                                    sqlhalfYear += " and createdate>='" + checkTime.Year + "-1-1' and createdate<'" + checkTime.Year + "-7-1'";
                                }
                                if (DbHelper.DbType == DatabaseType.Oracle)
                                {
                                    sqlhalfYear += " and createdate>=to_date('" + checkTime.Year + "-1-1','yyyy-mm-dd hh24:mi:ss') and createdate<to_date('" + checkTime.Year + "-7-1','yyyy-mm-dd hh24:mi:ss')";
                                }


                                selCount = this.BaseRepository().FindObject(sqlhalfYear).ToInt(); ;
                                if (selCount < everyTime)
                                    gsselPoint += everyPoint * (everyTime - selCount);
                                if (DbHelper.DbType == DatabaseType.MySql)
                                {
                                    sqlhalfYear2 += " and createdate>='" + checkTime.Year + "-7-1' and createdate<'" + checkTime.Year + "-12-31'";
                                }
                                if (DbHelper.DbType == DatabaseType.Oracle)
                                {
                                    sqlhalfYear2 += " and createdate>=to_date('" + checkTime.Year + "-7-1','yyyy-mm-dd hh24:mi:ss') and createdate<to_date('" + checkTime.Year + "-12-31','yyyy-mm-dd hh24:mi:ss')";
                                }


                                selCount = this.BaseRepository().FindObject(sqlhalfYear2).ToInt(); ;
                                if (selCount < everyTime)
                                    gsselPoint += everyPoint * (everyTime - selCount);
                            }
                            break;
                        case "年":
                            if (DbHelper.DbType == DatabaseType.MySql)
                            {
                                sqlWhere += "and createdate between '" + checkTime.Year + "-1-1' and '" + checkTime.Year + "-12-31'";
                            }
                            if (DbHelper.DbType == DatabaseType.Oracle)
                            {
                                sqlWhere += "and createdate between to_date('" + checkTime.Year + "-1-1','yyyy-mm-dd hh24:mi:ss') and to_date('" + checkTime.Year + "-12-31','yyyy-mm-dd hh24:mi:ss')";
                            }


                            selCount = this.BaseRepository().FindObject(sqlWhere).ToInt(); ;
                            if (selCount < everyTime)
                                gsselPoint += everyPoint * (everyTime - selCount);
                            break;
                        default: break;
                    }
                    gsselPoint = Double.Parse((gsselPoint > allScorePoint ? allScorePoint : gsselPoint).ToString("f1"));
                    dr["kscore"] = gsselPoint.ToString();
                    dr["score"] = (Convert.ToDouble(dr["indexscore"]) - gsselPoint).ToString();
                    #endregion
                }
                //部门检查次数
                else if (indexCode == "03")
                {

                    double bmselPoint = 0;
                    DateTime dtimew = Convert.ToDateTime(DateTime.Parse(time));
                    int nowWeekBM = WeekOfYear(dtimew);//当前月的一号
                    DateTime firstDay = dtimew;
                    DateTime endDay = dtimew;
                    int allDeptCount = this.BaseRepository().FindTable("select * from BASE_DEPARTMENT t where t.organizeid='" + user.OrganizeId + "' and isorg!='1' and (DESCRIPTION!='外包工程承包商' or DESCRIPTION is null) and  nature='部门'").Rows.Count;
                    string sqlFor = "and  CREATEUSERORGCODE='" + user.OrganizeCode + "' and   CHECKLEVEL=2 and createuserid in (select USERID from base_user where instr(rolename,'商级')=0  and length(DEPARTMENTCODE)>3)";
                    for (int i = 0; i < 5; i++)
                    {
                        string sqlWeek = sqlWhere;
                        GetWeek(dtimew.Year, i + nowWeekBM, out firstDay, out endDay);
                        string sqlDept = "select  substr(CREATEUSERDEPTCODE,0,6) from (select *from bis_saftycheckdatarecord where 1=1 " + sqlWeek + sqlFor + ")  group by  substr(CREATEUSERDEPTCODE,0,6) ";
                        if (DbHelper.DbType == DatabaseType.MySql)
                        {
                            sqlWeek = " and createdate between '" + firstDay + "' and '" + endDay + "'";
                            sqlDept = "select  substr(CREATEUSERDEPTCODE,1,6) from (select *from bis_saftycheckdatarecord where 1=1 " + sqlWeek + sqlFor + ") a  group by  substr(CREATEUSERDEPTCODE,1,6) ";
                        }
                        if (DbHelper.DbType == DatabaseType.Oracle)
                        {
                            sqlWeek = " and createdate between to_date('" + firstDay + "','yyyy-mm-dd hh24:mi:ss') and to_date('" + endDay + "','yyyy-mm-dd hh24:mi:ss')";
                            sqlDept = "select  substr(CREATEUSERDEPTCODE,0,6) from (select *from bis_saftycheckdatarecord where 1=1 " + sqlWeek + sqlFor + ")  group by  substr(CREATEUSERDEPTCODE,0,6) ";
                        }


                        selCount = this.BaseRepository().FindTable(sqlDept).Rows.Count;
                        if (selCount < allDeptCount)
                            bmselPoint += 2;
                    }

                    bmselPoint = Double.Parse((bmselPoint > allScorePoint ? allScorePoint : bmselPoint).ToString("f1"));
                    //selPoint += Double.Parse((allScorePoint - bmkf).ToString("f1"));

                    dr["kscore"] = bmselPoint;
                    dr["score"] = (Convert.ToInt32(dr["indexscore"]) - bmselPoint).ToString();
                }
                //班组检查次数
                else if (indexCode == "04")
                {
                    string[] arr = dr["indexargsvalue"].ToString().Split('|');
                    int everyNum = Convert.ToInt32(arr[0]);
                    double everyScore = Convert.ToDouble(arr[1]);
                    int allDayOfM = checkTime.Day;
                    double soldPoint = 0;
                    DateTime fristDay = Convert.ToDateTime(DateTime.Parse(time));
                    for (int i = 0; i < allDayOfM; i++)
                    {
                        sql = string.Format(@"select count(encode) as cou,nvl(sum(nvl(sum(daynum),0)),0) as allnum  from base_department a 
left join (select createuserdeptcode, 1 daynum from (select createuserdeptcode,count(1) as num  from  Bis_Saftycheckdatarecord  
where to_char(checkBeginTime,'yyyy-MM-dd')='{0}' and checkLevel='4' group by createuserdeptcode) where num>={1} ) b on  a.encode=b.createuserdeptcode
where nature='班组' and organizeId='{2}' group by encode", fristDay.ToString("yyyy-MM-dd"), everyNum, user.OrganizeId);
                        if (DbHelper.DbType == DatabaseType.MySql)
                        {
                            sql = string.Format(@"select count(encode) as cou,case when isnull(sum(daynum)) then 0 else sum(daynum) end as allnum  from base_department a 
left join (select createuserdeptcode, 1 daynum from (select createuserdeptcode,count(1) as num  from  Bis_Saftycheckdatarecord  
where date_format(checkBeginTime,'%Y-%m-%d')='{0}' and checkLevel='4' group by createuserdeptcode) r where num>={1} ) b on  a.encode=b.createuserdeptcode
where nature='班组' and organizeId='{2}' group by encode", fristDay.ToString("yyyy-MM-dd"), everyNum, user.OrganizeId);
                        }
                        DataTable dtDayNum = this.BaseRepository().FindTable(sql);
                        if (dtDayNum.Rows.Count > 0)
                        {
                            soldPoint += int.Parse(dtDayNum.Rows[0][0].ToString()) > int.Parse(dtDayNum.Rows[0][1].ToString()) ? everyScore : 0;
                        }
                        if (soldPoint >= allScorePoint)
                            break;
                        fristDay = fristDay.AddDays(1);
                    }
                    soldPoint = soldPoint >= allScorePoint ? allScorePoint : soldPoint;
                    dr["kscore"] = soldPoint;
                    dr["score"] = allScorePoint - soldPoint;

                }
                totalScore += allScorePoint;
                finalScore += decimal.Parse(dr["score"].ToString());
            }
            return new { data = dt, allScore = totalScore, finalScore = finalScore, weight = weight };
        }
        /// <summary>
        /// 首页指标得分(大图)
        /// </summary>
        /// <param name="date">首页传过来的日期</param>
        /// <returns></returns>
        public decimal GetSafeCheckWarningM(Operator user, string date, int mode = 0)
        {

            string sqlEx = string.Format("select count(1) from BIS_CLASSIFICATIONINDEX where affiliatedorganizeid='{0}'", user.OrganizeId);
            int countEx = this.BaseRepository().FindObject(sqlEx).ToInt();
            string forMonthDate = date;

            var sql = string.Format("select indexname,indexscore,IndexStandard,indexargsvalue,0 kscore,0 score,indexcode from  bis_classificationindex  where classificationcode='01' ");
            if (countEx == 0)
            {
                sql += " and affiliatedorganizeid='0'  order by indexcode";
            }
            else
            {
                sql += " and affiliatedorganizeid='" + user.OrganizeId + "' order by indexcode";
            }
            DataTable dt = this.BaseRepository().FindTable(sql);
            //实际次数
            int selCount = 0;
            //扣分分值
            double soldPoint = 0;
            //得分总数
            double selPoint = 0;//这里要根据月份累计加起来
            string sendMonth = DateTime.Now.ToString("yyyy-MM-dd");
            //条件
            string sqlWhere = "select count(id) from bis_saftycheckdatarecord where 1=1 and CREATEUSERORGCODE='" + user.OrganizeCode + "'";
            if (!string.IsNullOrEmpty(date)) sendMonth = date;
            string endTime = DateTime.Parse(date).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
            foreach (DataRow dr in dt.Rows)
            {
                var indexCode = dr["indexcode"].ToString();
                double allScorePoint = Convert.ToDouble(dr["indexscore"].ToString());
                switch (indexCode)
                {
                    //未执行安全检查计划次数
                    case "00":
                        sendMonth = mode == 1 ? DateTime.Parse(sendMonth).ToString("yyyy-01-01 00:00:01") : sendMonth;
                        sql = string.Format(@"select count(1) as num from Bis_Saftycheckdatarecord where CheckDataType!='1'  and solvePerson is null 
and to_char(checkEndTime,'yyyy-MM-dd')>='{0}' and to_char(checkEndTime,'yyyy-MM-dd')<='{1}' and  CreateUserOrgCode='{2}'", sendMonth, endTime, user.OrganizeCode);
                        if (DbHelper.DbType == DatabaseType.MySql)
                        {
                            sql = string.Format(@"select count(1) as num from Bis_Saftycheckdatarecord where CheckDataType!='1'  and solvePerson is null 
and checkEndTime>='{0}' and checkEndTime<='{1}' and  CreateUserOrgCode='{2}'", sendMonth, endTime, user.OrganizeCode);
                        }
                        selCount = this.BaseRepository().FindObject(sql).ToInt();
                        soldPoint = 0;
                        if (selCount > 0)
                        {
                            soldPoint = selCount * double.Parse(dr["indexargsvalue"].ToString());//设定分值
                        }
                        soldPoint = soldPoint >= allScorePoint ? allScorePoint : soldPoint;
                        selPoint += (allScorePoint - soldPoint);
                        break;
                    //公司级检查
                    case "03":

                        #region 公司级检查
                        double gskf = 0;
                        //次数
                        int everyTime = Convert.ToInt32(dr["indexargsvalue"].ToString().Split('|')[1]);
                        //每次扣分扣分
                        double everyPoint = Convert.ToInt32(dr["indexargsvalue"].ToString().Split('|')[2]);
                        //总分

                        //周/月/季度/半年/年
                        string everyDate = dr["indexargsvalue"].ToString().Split('|')[0];
                        sqlWhere += " and createuserid in (select USERID from base_user where (instr(rolename,'厂级部门用户')>0 or instr(rolename,'公司级用户')>0))";
                        sqlWhere += " and CHECKLEVEL=1";
                        switch (everyDate)
                        {
                            case "周":
                                //传过来的日期的第一天是第几周
                                DateTime nowWeek = mode == 0 ? Convert.ToDateTime(sendMonth) : DateTime.Parse(endTime);
                                int nowWeekw = WeekOfYear(nowWeek);
                                DateTime firstDayw = nowWeek;
                                DateTime endDayw = nowWeek;
                                int weeks = mode == 0 ? 5 : nowWeekw;
                                for (int i = 1; i <= weeks; i++)
                                {
                                    string sqlWeeks = sqlWhere;
                                    int count = mode == 0 ? i + nowWeekw - 1 : i;
                                    GetWeek(nowWeek.Year, count, out firstDayw, out endDayw);
                                    // GetWeek(nowWeek.Year, i + nowWeekw, out firstDayw, out endDayw);
                                    if (DbHelper.DbType == DatabaseType.Oracle)
                                    {
                                        sqlWeeks += " and createdate between to_date('" + firstDayw.ToString("yyyy-MM-dd HH:mm:ss") + "','yyyy-mm-dd hh24:mi:ss') and to_date('" + endDayw.ToString("yyyy-MM-dd HH:mm:ss") + "','yyyy-mm-dd hh24:mi:ss'）";
                                    }
                                    if (DbHelper.DbType == DatabaseType.MySql)
                                    {
                                        sqlWeeks += " and createdate between '" + firstDayw.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + endDayw.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                                    }
                                    selCount = this.BaseRepository().FindObject(sqlWeeks).ToInt();
                                    if (selCount < everyTime)
                                        gskf += everyPoint * (everyTime - selCount);
                                }
                                break;
                            case "月":
                                DateTime dtime = Convert.ToDateTime(sendMonth);
                                int cMonth = dtime.Month;
                                //for (int i = 1; i <= cMonth; i++)
                                //{
                                string sqlMonth = sqlWhere;
                                if (cMonth == 12)
                                {
                                    if (DbHelper.DbType == DatabaseType.Oracle)
                                    {
                                        sqlMonth += " and createdate>=to_date('" + dtime + "','yyyy-mm-dd hh24:mi:ss') and createdate<to_date('" + dtime.Year + "-" + cMonth + "-31','yyyy-mm-dd hh24:mi:ss')";
                                    }
                                    if (DbHelper.DbType == DatabaseType.MySql)
                                    {
                                        sqlMonth += " and createdate>='" + dtime + "' and createdate<'" + dtime.Year + "-" + cMonth + "-31'";
                                    }
                                }
                                else
                                {
                                    if (DbHelper.DbType == DatabaseType.Oracle)
                                    {
                                        sqlMonth += " and createdate>=to_date('" + dtime + "','yyyy-mm-dd hh24:mi:ss') and createdate<to_date('" + dtime.AddMonths(1) + "','yyyy-mm-dd hh24:mi:ss')";
                                    }
                                    if (DbHelper.DbType == DatabaseType.MySql)
                                    {
                                        sqlMonth += " and createdate>='" + dtime + "' and createdate<'" + dtime.AddMonths(1) + "'";
                                    }
                                    selCount = this.BaseRepository().FindObject(sqlMonth).ToInt(); ;
                                    if (selCount < everyTime)
                                        gskf += everyPoint * (everyTime - selCount);
                                }
                                break;
                            case "季度":
                                DateTime dtimeS = Convert.ToDateTime(sendMonth);

                                int i1 = 0; int j = 0;
                                if (dtimeS.Month < 4) { i1 = 1; j = 3; }
                                else
                                    if (dtimeS.Month < 7) { i1 = 4; j = 6; }
                                    else
                                        if (dtimeS.Month < 10) { i1 = 7; j = 9; }
                                        else
                                            if (dtimeS.Month <= 12) { i1 = 10; j = 12; }
                                string sqlSeaon = sqlWhere;
                                string a = "";
                                if (j == 12) a = dtimeS.Year.ToString() + "-12-31";
                                else a = dtimeS.Year + "-" + (j + 1).ToString() + "-1";
                                if (DbHelper.DbType == DatabaseType.Oracle)
                                {
                                    sqlSeaon += " and createdate>=to_date('" + dtimeS.Year + "-" + i1 + "-1','yyyy-mm-dd hh24:mi:ss') and createdate<to_date('" + a + "','yyyy-mm-dd hh24:mi:ss')";
                                }

                                if (DbHelper.DbType == DatabaseType.MySql)
                                {
                                    sqlSeaon += " and createdate>='" + dtimeS.Year + "-" + i1 + "-1' and createdate<'" + a + "'";
                                }
                                selCount = this.BaseRepository().FindObject(sqlSeaon).ToInt(); ;
                                if (selCount < everyTime)
                                    gskf += everyPoint * (everyTime - selCount);
                                break;
                            case "半年":
                                DateTime dtimeH = Convert.ToDateTime(sendMonth);
                                int chalfYear = dtimeH.Month;
                                if (chalfYear <= 6)
                                {
                                    string sqlhalfYear = sqlWhere;
                                    if (DbHelper.DbType == DatabaseType.Oracle)
                                    {
                                        sqlhalfYear += " and createdate>=to_date('" + dtimeH.Year + "-1-1','yyyy-mm-dd hh24:mi:ss') and createdate<to_date('" + dtimeH.Year + "-7-1','yyyy-mm-dd hh24:mi:ss')";
                                    }

                                    if (DbHelper.DbType == DatabaseType.MySql)
                                    {
                                        sqlhalfYear += " and createdate>='" + dtimeH.Year + "-1-1' and createdate<'" + dtimeH.Year + "-7-1'";
                                    }
                                    selCount = this.BaseRepository().FindObject(sqlhalfYear).ToInt(); ;
                                    if (selCount < everyTime)
                                        gskf += everyPoint * (everyTime - selCount);
                                }
                                else
                                {
                                    string sqlhalfYear = sqlWhere, sqlhalfYear2 = sqlWhere;
                                    //sqlhalfYear += " and createdate>=to_date('" + DateTime.Now.Year + "-1-1','yyyy-mm-dd hh24:mi:ss') and createdate<to_date('" + DateTime.Now.Year + "-7-1','yyyy-mm-dd hh24:mi:ss')";
                                    //selCount = this.BaseRepository().FindObject(sqlhalfYear).ToInt(); ;
                                    //if (selCount < everyTime)
                                    //    gskf += everyPoint * (everyTime - selCount);

                                    if (DbHelper.DbType == DatabaseType.Oracle)
                                    {
                                        sqlhalfYear2 += " and createdate>=to_date('" + dtimeH.Year + "-7-1','yyyy-mm-dd hh24:mi:ss') and createdate<to_date('" + dtimeH.Year + "-12-31','yyyy-mm-dd hh24:mi:ss')";
                                    }
                                    if (DbHelper.DbType == DatabaseType.MySql)
                                    {
                                        sqlhalfYear2 += " and createdate>='" + dtimeH.Year + "-7-1' and createdate<'" + dtimeH.Year + "-12-31'";
                                    }
                                    selCount = this.BaseRepository().FindObject(sqlhalfYear2).ToInt(); ;
                                    if (selCount < everyTime)
                                        gskf += everyPoint * (everyTime - selCount);
                                }
                                break;
                            case "年":
                                DateTime allYear = Convert.ToDateTime(sendMonth);
                                if (DbHelper.DbType == DatabaseType.Oracle)
                                {
                                    sqlWhere += "and createdate between to_date('" + allYear.Year + "-1-1','yyyy-mm-dd hh24:mi:ss') and to_date('" + allYear.Year + "-12-31','yyyy-mm-dd hh24:mi:ss')";
                                }

                                if (DbHelper.DbType == DatabaseType.MySql)
                                {
                                    sqlWhere += "and createdate between '" + allYear.Year + "-1-1' and '" + allYear.Year + "-12-31'";
                                }
                                selCount = this.BaseRepository().FindObject(sqlWhere).ToInt(); ;
                                if (selCount < everyTime)
                                    gskf += everyPoint * (everyTime - selCount);
                                break;
                            default: break;
                        }
                        gskf = Double.Parse((gskf > allScorePoint ? allScorePoint : gskf).ToString("f1"));
                        dr["kscore"] = gskf.ToString();
                        dr["score"] = (Convert.ToDouble(dr["indexscore"]) - gskf).ToString();
                        selPoint += Convert.ToDouble(dr["indexscore"]) - gskf;
                        #endregion
                        break;
                    //部门检查次数
                    case "04":

                        //次数
                        double bmkf = 0;

                        DateTime dtimew = Convert.ToDateTime(forMonthDate);
                        int nowWeekBM = WeekOfYear(dtimew);//当前月的一号
                        DateTime firstDay = dtimew;
                        DateTime endDay = dtimew;
                        int allDeptCount = this.BaseRepository().FindTable("select * from BASE_DEPARTMENT t where t.organizeid='" + user.OrganizeId + "' and isorg!='1' and (DESCRIPTION!='外包工程承包商' or DESCRIPTION is null) and  nature='部门'").Rows.Count;
                        string sqlFor = "and  CREATEUSERORGCODE='" + user.OrganizeCode + "' and   CHECKLEVEL=2 and createuserid in (select USERID from base_user where instr(rolename,'商级')=0  and length(DEPARTMENTCODE)>3)";
                        for (int i = 0; i < 5; i++)
                        {
                            string sqlWeek = sqlWhere;
                            GetWeek(dtimew.Year, i + nowWeekBM, out firstDay, out endDay);
                            sqlWeek = " and createdate between to_date('" + firstDay + "','yyyy-mm-dd hh24:mi:ss') and to_date('" + endDay + "','yyyy-mm-dd hh24:mi:ss')";
                            if (DbHelper.DbType == DatabaseType.MySql)
                            {
                                sqlWeek = " and createdate between '" + firstDay + "' and '" + endDay + "'";
                            }
                            string sqlDept = "select  substr(CREATEUSERDEPTCODE,0,6) from (select *from bis_saftycheckdatarecord where 1=1 " + sqlWeek + sqlFor + ") a group by  substr(CREATEUSERDEPTCODE,0,6) ";
                            if (DbHelper.DbType == DatabaseType.MySql)
                            {
                                sqlDept = "select  substr(CREATEUSERDEPTCODE,1,6) from (select *from bis_saftycheckdatarecord where 1=1 " + sqlWeek + sqlFor + ") a group by  substr(CREATEUSERDEPTCODE,1,6) ";
                            }
                            selCount = this.BaseRepository().FindTable(sqlDept).Rows.Count;
                            if (selCount < allDeptCount)
                                bmkf += 2;
                        }
                        bmkf = Double.Parse((bmkf > allScorePoint ? allScorePoint : bmkf).ToString("f1"));
                        selPoint += Double.Parse((allScorePoint - bmkf).ToString("f1"));
                        break;
                    //班组检查次数
                    case "05":
                        string[] arr = dr["indexargsvalue"].ToString().Split('|');
                        int everyNum = Convert.ToInt32(arr[0]);
                        double everyScore = Convert.ToDouble(arr[1]);
                        int allDayOfM = DateTime.Parse(endTime).Day;
                        soldPoint = 0;
                        DateTime fristDay = Convert.ToDateTime(date);
                        for (int i = 0; i < allDayOfM; i++)
                        {
                            sql = string.Format(@"select count(encode) as cou,nvl(sum(nvl(sum(daynum),0)),0) as allnum  from base_department a 
left join (select createuserdeptcode, 1 daynum from (select createuserdeptcode,count(1) as num  from  Bis_Saftycheckdatarecord  
where to_char(checkBeginTime,'yyyy-MM-dd')='{0}' and checkLevel='4' group by createuserdeptcode) where num>={1} ) b on  a.encode=b.createuserdeptcode
where nature='班组' and organizeId='{2}' group by encode", fristDay.ToString("yyyy-MM-dd"), everyNum, user.OrganizeId);
                            if (DbHelper.DbType == DatabaseType.MySql)
                            {
                                sql = string.Format(@"select count(encode) as cou,case when isnull(sum(daynum)) then 0 else sum(daynum) end as allnum  from base_department a 
left join (select createuserdeptcode, 1 daynum from (select createuserdeptcode,count(1) as num  from  Bis_Saftycheckdatarecord  
where date_format(checkBeginTime,'%Y-%m-%d')='{0}' and checkLevel='4' group by createuserdeptcode) r where num>={1} ) b on  a.encode=b.createuserdeptcode
where nature='班组' and organizeId='{2}' group by encode", fristDay.ToString("yyyy-MM-dd"), everyNum, user.OrganizeId);
                            }
                            DataTable dtDayNum = this.BaseRepository().FindTable(sql);
                            if (dtDayNum.Rows.Count > 0)
                            {
                                soldPoint += int.Parse(dtDayNum.Rows[0][0].ToString()) > int.Parse(dtDayNum.Rows[0][1].ToString()) ? everyScore : 0;
                            }
                            if (soldPoint >= allScorePoint)
                                break;
                            fristDay = fristDay.AddDays(1);
                        }
                        soldPoint = soldPoint >= allScorePoint ? allScorePoint : soldPoint;
                        selPoint += (allScorePoint - soldPoint);
                        break;
                    default: break;
                }
            }
            return Convert.ToDecimal(selPoint);
        }
        /// <summary>
        /// 首页指标得分(小图)
        /// </summary>
        /// <returns></returns>
        public string GetSafeCheckWarningS()
        {
            List<decimal> list = new List<decimal>();
            List<string> xValues = new List<string>();
            Operator user = OperatorProvider.Provider.Current();
            for (int j = -6; j < 0; j++)
            {
                string startDate = DateTime.Now.AddMonths(j).ToString("yyyy-MM-01");
                xValues.Add(DateTime.Parse(startDate).ToString("yyyy.MM"));
                list.Add(GetSafeCheckWarningM(user, startDate));
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { x = xValues, y = list });
        }
        /// <summary>
        /// 根据部门和类型获取部门的检查表内容
        /// </summary>
        /// <param name="DeptCode">部门code集合</param>
        /// <returns></returns>
        public DataTable AddDeptCheckTable(string DeptCode, string Type)
        {

            string[] deptCodeArr = DeptCode.Split(',');
            string sql = string.Empty;
            for (int i = 0; i < deptCodeArr.Length; i++)
            {
                sql += string.Format(@"select *from (select s.ID  from
                                            (SELECT  ID ,ROWNUM num
                                            FROM
	                                            BIS_SAFTYCHECKDATA T
                                            where  t.belongdeptcode = '{0}' and  t.CHECKDATATYPE='{1}' ORDER by t.CREATEDATE desc) s
                                            )where rownum<2 ", deptCodeArr[i], Type);
                if (i < deptCodeArr.Length - 1)
                {
                    sql += " UNION  ";
                }
            }
            string sqlstr = string.Format(@"select * from BIS_SAFTYCHECKDATADETAILED where recid in ({0})  order by autoid,checkobject asc", sql);
            DataTable dt = this.BaseRepository().FindTable(sqlstr);

            return dt;
        }

        /// <summary>
        /// 根据部门CODE获取部门人员集合
        /// </summary>
        /// <param name="Encode">部门Code</param>
        /// <returns>返回对象Json</returns>
        public DataTable GetPeopleByEncode(string Encode)
        {
            string sql = string.Format(@"select  WM_CONCAT(Account) CheckAccount , WM_CONCAT(REALNAME) CheckManName from BASE_USER u where u.DEPARTMENTCODE='{0}'", Encode);
            return this.BaseRepository().FindTable(sql);
        }
        /// <summary>
        /// 安全预警图
        /// </summary>
        /// <param name="user">当前登录人user</param>
        /// <returns></returns>
        public decimal GetSafeCheckSumCount(Operator user)
        {
            var dt = GetSafeCheckWarning(user);
            decimal sumcount = 0;
            foreach (DataRow item in dt.Rows)
            {
                sumcount += Convert.ToDecimal(item["score"].ToString());
            }
            return sumcount;
        }
        /// <summary>
        /// 修正安全检查记录的检查人员
        /// </summary>
        public void UpdateCheckUsers()
        {
            DataTable dtData = BaseRepository().FindTable(string.Format("select id from bis_saftycheckdatarecord where checkdatatype<>1"));
            StringBuilder sb = new StringBuilder("begin \r\n ");
            foreach (DataRow dr in dtData.Rows)
            {
                DataTable dtAccounts = BaseRepository().FindTable(string.Format(@"SELECT DISTINCT
	                                            (
		                                            SUBSTR (
			                                            T .ca,
			                                            (INSTR(T .ca, ',', 1, c.lv) + 1),
			                                            (
				                                            INSTR (T .ca, ',', 1, c.lv + 1) - (INSTR(T .ca, ',', 1, c.lv) + 1)
			                                            )
		                                            )
	                                            ) AS checkmanaccount
                                            FROM
	                                            (
		                                            SELECT
			                                            RECID,
			                                            ',' || checkmanid || ',' AS ca,
			                                            LENGTH (checkmanid || ',') - NVL (
				                                            LENGTH (
					                                            REPLACE (checkmanid, ',')
				                                            ),
				                                            0
			                                            ) AS cnt
		                                            FROM
			                                            BIS_SAFTYCHECKDATADETAILED
	                                            ) T,
	                                            (
		                                            SELECT
			                                            LEVEL lv
		                                            FROM
			                                            dual CONNECT BY LEVEL <= 100
	                                            ) c
                                            WHERE
	                                            c.lv <= T .cnt
                                            AND T .RECID = '{0}'", dr[0].ToString()));
                StringBuilder sbAccounts = new StringBuilder();
                foreach (DataRow dr1 in dtAccounts.Rows)
                {
                    sbAccounts.AppendFormat("{0},", dr1[0].ToString());
                }
                sb.AppendFormat("update bis_saftycheckdatarecord set checkuserids='{0}' where id='{1}';\r\n", sbAccounts.ToString().TrimEnd(','), dr[0].ToString());
            }
            sb.Append("commit;\r\n end;");
            BaseRepository().ExecuteBySql(sb.ToString());
        }
    }
}