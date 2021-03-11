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
using ERCHTMS.Code;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace ERCHTMS.Service.SaftyCheck
{
    /// <summary>
    /// 描 述：安全检查表
    /// </summary>
    public class SaftyCheckDataService : RepositoryFactory<SaftyCheckDataEntity>, SaftyCheckDataIService
    {
        #region 获取数据
        /// <summary>
        /// 通过folderId 获取对应的文件
        /// </summary>
        /// <param name="folderId"></param>
        /// <returns></returns>
        public DataTable GetListByObject(string folderId)
        {
            var strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT  FileId ,
                                    FolderId ,
                                    FileName ,
                                    FileSize ,
                                    FileType ,
                                    CreateUserId ,
                                    ModifyDate,
                                    IsShare,FilePath
                            FROM    Base_FileInfo
                            WHERE   DeleteMark = 0
                                   
                                    AND folderId = '{0}'  ORDER BY ModifyDate ASC", folderId);
            return this.BaseRepository().FindTable(strSql.ToString());
        }
        public DataTable GetCheckStat(ERCHTMS.Code.Operator user, int category)
        {
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

            //权限判断
            //var user = OperatorProvider.Provider.Current();
            string deptCode = user.DeptCode;
            string sql = " where datatype in(0,2) and CreateDate >= to_date('" + from.ToString("yyyy/MM/dd") + "', 'yyyy/mm/dd') and CreateDate < to_date('" + to.ToString("yyyy/MM/dd") + "', 'yyyy/mm/dd')";
            if (DbHelper.DbType == DatabaseType.MySql)
            {
                sql = " where 1=1 and date_format(CreateDate,'%Y')='" + DateTime.Now.Year + "'";
            }
            var arg = "";

            if (user.RoleName.Contains("公司级") || user.RoleName.Contains("厂级"))
            {

                sql += string.Format("  and (belongdept like '{0}%' and ',' || checkdeptcode like '%,{0}%' or  ',' || CHECKDEPTID like '%,{0}%')", user.OrganizeCode);
            }
            else if (user.RoleName.Contains("省级"))
            {
                sql += string.Format("  and createuserorgcode in (select encode from base_department  where deptcode like '{0}%' and nature='厂级')", user.NewDeptCode);
            }
            else
            {
                arg = user.DeptCode;
                sql += string.Format(" and (belongdept like '{0}%' or  ',' || checkdeptid || ',' like '%,{0}%' or ',' || checkdeptcode || ',' like '%,{0}%') ", user.DeptCode);
                //sql += string.Format(" and (( createuserdeptcode like '{0}%') or (((',' || checkdeptid || ',') like '%,{0}%') or (datatype=0 and id in(select recid from BIS_SAFETYCHECKDEPT where deptcode in(select encode from base_department d where d.encode like '{0}%' )))))", user.DeptCode);

                //sql += string.Format("  and ((belongdept in(select encode from base_department  where encode like '{0}%'  or senddeptid='{1}') and datatype=0) or (datatype=2 and belongdept in(select encode from base_department  where encode like '{0}%')))", arg, user.DeptId);
            }
            //
            string sqlWhere = string.Format(@"
select nvl(b.num, 0) num
  from ( select itemvalue,''
                  from BASE_DATAITEMDETAIL
                 where itemid = (select itemid
                                   from base_dataitem
                                  where itemcode = 'SaftyCheckType')
                 order by itemvalue) a
   left join(select to_char(CHECKDATATYPE) as itemvalue, count(1) as num
 
                from (select * from BIS_SAFTYCHECKDATARECORD {0}) t", sql);
            sqlWhere += @" where 1 = 1
                group by CHECKDATATYPE) b
     on a.itemvalue = b.itemvalue ";

            if (DbHelper.DbType == DatabaseType.MySql)
            {
                sqlWhere = string.Format(@"
select  case when isnull(b.num) then 0 else b.num end as num
  from ( select itemvalue,''
                  from BASE_DATAITEMDETAIL
                 where itemid = (select itemid
                                   from base_dataitem
                                  where itemcode = 'SaftyCheckType')
                 order by itemvalue) a
   left join(select CHECKDATATYPE as itemvalue, count(1) as num
 
                from (select * from BIS_SAFTYCHECKDATARECORD {0}) t", sql);
                sqlWhere += @" where 1 = 1
                group by CHECKDATATYPE) b
     on a.itemvalue = b.itemvalue ";
            }

            string sqlAdd = string.Format(@"select count(*) as num  from BIS_SAFTYCHECKDATARECORD {0}", sql);

            DataTable dt = this.BaseRepository().FindTable(sqlAdd);

            DataTable dt2 = this.BaseRepository().FindTable(sqlWhere);
            foreach (DataRow item in dt2.Rows)
            {
                DataRow dr = dt.NewRow();
                dr[0] = item[0];
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// 首页待办事项
        /// </summary>
        /// <param name="user"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public int[] GetCheckCount(ERCHTMS.Code.Operator user, int mode)
        {
            int[] countSum = new int[5];
            int count1 = 0, count2 = 0, count3 = 0, count4 = 0, countRc = 0;
            if (mode == 0)
            {
                string sql = "";
                if (user.RoleName.Contains("省级") || user.RoleName.Contains("集团"))
                {
                    sql = string.Format(@"select checkDataType,count(1) from  bis_saftycheckdatarecord t where  id in(select a.recid from bis_saftycheckdatadetailed a where  instr((',' || a.checkmanid || ','),',{0},')>0 and  a.id not in(select b.detailid from BIS_SAFTYCONTENT b)
) group by checkDataType", user.Account);
                }
                else
                {
                    sql = string.Format(@"select t.checkdatatype,count(1) from bis_saftycheckdatarecord t where 
 id in(select distinct  a.recid from bis_saftycheckdatadetailed a left join bis_saftycontent b on a.id=b.detailid where checkDataType<>1 and b.id is null and (',' || a.CheckManid || ',') like '%,{0},%'
) group by checkdatatype", user.Account);
                    //                    sql = string.Format(@"select checkDataType,count(1) from   bis_saftycheckdatarecord t left join  (select recid from BIS_SAFTYCHECKDATADETAILED where instr(CheckManid,'{0}')>0 group by  recid) t1
                    //on t.id=t1.recid  where recid is not null and ((instr(solveperson,'{0}'))=0  or solveperson is null)  and belongdept like  '{1}%' group by checkDataType", user.Account, user.OrganizeCode);

                }

                DataTable dt = this.BaseRepository().FindTable(sql);
                count1 = dt.Select("checkDataType=2").Length == 0 ? 0 : int.Parse(dt.Select("checkDataType=2")[0][1].ToString());
                count2 = dt.Select("checkDataType=3").Length == 0 ? 0 : int.Parse(dt.Select("checkDataType=3")[0][1].ToString());
                count3 = dt.Select("checkDataType=4").Length == 0 ? 0 : int.Parse(dt.Select("checkDataType=4")[0][1].ToString());
                count4 = dt.Select("checkDataType=5").Length == 0 ? 0 : int.Parse(dt.Select("checkDataType=5")[0][1].ToString());
                string sqlRc = string.Format(@"select count(1) from bis_saftycheckdatarecord where  checkDataType=1 and (datatype=0 or datatype=2) and checkmanid like '%{0}%' and ((instr(solveperson,'{0}'))=0  or solveperson is null)   and belongdept like '{1}%' ", user.Account, user.OrganizeCode, user.UserId);
                countRc = this.BaseRepository().FindObject(sqlRc).ToInt();

            }
            else if (mode == 1)
            {
                if (user.RoleName.Contains("公司级") || user.RoleName.Contains("厂级"))
                {
                    string sql = string.Format("select checkDataType,count(1) from bis_saftycheckdatarecord where  solveperson is null   and belongdept like '{0}%' group by checkDataType", user.OrganizeCode);
                    DataTable dt = this.BaseRepository().FindTable(sql);
                    countRc = dt.Select("checkDataType='1'").Length == 0 ? 0 : int.Parse(dt.Select("checkDataType='1'")[0][1].ToString());
                    count1 = dt.Select("checkDataType='2'").Length == 0 ? 0 : int.Parse(dt.Select("checkDataType='2'")[0][1].ToString());
                    count2 = dt.Select("checkDataType='3'").Length == 0 ? 0 : int.Parse(dt.Select("checkDataType='3'")[0][1].ToString());
                    count3 = dt.Select("checkDataType='4'").Length == 0 ? 0 : int.Parse(dt.Select("checkDataType='4'")[0][1].ToString());
                    count4 = dt.Select("checkDataType='5'").Length == 0 ? 0 : int.Parse(dt.Select("checkDataType='5'")[0][1].ToString());
                }
                else
                {
                    string sql = string.Format(@"select checkDataType,count(1) from bis_saftycheckdatarecord t right join (select * from (select id,(','||checkmanid||',') as solveperson  from  bis_saftycheckdatarecord) a 
left join (
select (','||account||',') as account from base_user where  departmentcode in(select encode from base_department  where encode= '{0}')
) b on a.solveperson  like '%'||b.account||'%' where (account is not null or solveperson = ',,'))d
 on t.id=d.id  where t.id in(select id from bis_saftycheckdatarecord t left join  (select recid from (select id,(','||CheckManid||',') as CheckManAccount,recid  from BIS_SAFTYCHECKDATADETAILED) a 
left join (select (','||account||',') as account from base_user where  departmentcode in(select encode from base_department  where encode= '{0}')
) b on a.CheckManAccount  like '%'||b.account||'%' where account is not null  group by recid)t1
on t.id=t1.recid where t1.recid is not null and t.solveperson is null) group by checkDataType", user.DeptCode);
                    DataTable dt = this.BaseRepository().FindTable(sql);
                    count1 = dt.Select("checkDataType='2'").Length == 0 ? 0 : int.Parse(dt.Select("checkDataType='2'")[0][1].ToString());
                    count2 = dt.Select("checkDataType='3'").Length == 0 ? 0 : int.Parse(dt.Select("checkDataType='3'")[0][1].ToString());
                    count3 = dt.Select("checkDataType='4'").Length == 0 ? 0 : int.Parse(dt.Select("checkDataType='4'")[0][1].ToString());
                    count4 = dt.Select("checkDataType='5'").Length == 0 ? 0 : int.Parse(dt.Select("checkDataType='5'")[0][1].ToString());
                    string sqlRc = string.Format("select count(1),checkDataType from bis_saftycheckdatarecord where (solveperson is null) and checkDataType=1  and belongdept ='{0}' group by checkDataType", user.DeptCode, user.Account);
                    countRc = this.BaseRepository().FindObject(sqlRc).ToInt();
                }
            }
            countSum[0] = countRc;
            countSum[1] = count1;
            countSum[2] = count2;
            countSum[3] = count3;
            countSum[4] = count4;
            return countSum;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SaftyCheckDataEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SaftyCheckDataEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 安全检查名称字典列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetCheckNamePageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //检查名称
            if (!queryParam["checkName"].IsEmpty())
            {
                string checkName = queryParam["checkName"].ToString();
                pagination.conditionJson += string.Format(" and checkname like '%{0}%'", checkName.Trim());
            }
            //状态
            if (!queryParam["status"].IsEmpty())
            {
                string status = queryParam["status"].ToString();
                pagination.conditionJson += string.Format(" and t.status='{0}'", status);
            }
            DataTable dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            return dt;
        }
        /// <summary>
        /// 安全检查表列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<SaftyCheckDataEntity> GetPageList(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();

            //选择的类型
            if (!queryParam["type"].IsEmpty())
            {
                string type = queryParam["type"].ToString();
                pagination.conditionJson += string.Format(" and t.CHECKDATATYPE in ('{0}') and t.CHECKDATATYPE is not null", type);
            }
            //搜索关键字
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyword = queryParam["keyword"].ToString();
                pagination.conditionJson += string.Format(" and t.CheckDataName like '%{0}%'", keyword);
            }
            //所属部门,这里传过来的是选取的部门的code,保存的时候存所属部门ID,查询的时候穿点击树的code
            if (!queryParam["belongdeptcode"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.belongdeptid  in (select departmentid from base_department where encode like '{0}%' union select ORGANIZEID from BASE_ORGANIZE where encode like '{0}%')", queryParam["belongdeptcode"].ToString());
            }
            IEnumerable<SaftyCheckDataEntity> list = this.BaseRepository().FindListByProcPager(pagination, dataType);
            return list;

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
            this.BaseRepository().ExecuteBySql("delete from bis_saftycheckdatadetailed where recid='" + keyValue + "'");
        }
        /// <summary>
        /// 删除检查名称
        /// </summary>
        /// <param name="keyValue"></param>
        public void RemoveCheckName(string keyValue)
        {
            DbFactory.Base().Delete<CheckNameSetEntity>(keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public int SaveForm(string keyValue, SaftyCheckDataEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.ID = keyValue;
                var sc = this.BaseRepository().FindEntity(keyValue);
                if (sc == null)
                {
                    entity.Create();
                    return this.BaseRepository().Insert(entity);
                }
                else
                {
                    entity.Modify(keyValue);
                    return this.BaseRepository().Update(entity);
                }

            }
            else
            {
                entity.Create();
                return this.BaseRepository().Insert(entity);
            }
        }
        /// <summary>
        /// 保存检查名称
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public int SaveCheckName(Operator user, List<CheckNameSetEntity> list)
        {
            var db = DbFactory.Base().BeginTrans();
            try
            {
                db.Delete<CheckNameSetEntity>(t => t.OrgCode.Equals(user.OrganizeCode));
                db.Insert<CheckNameSetEntity>(list);
                db.Commit();
                return 1;
            }
            catch(Exception ex)
            {
                db.Rollback();
                return 0;
            }
        }
        #endregion

        #region 手机端
        public IEnumerable<SaftyCheckDataEntity> selectCheckExcel(Operator user)
        {
            //string sqlWhere = "select *from BIS_SAFTYCHECKDATA where 1=1";
            //--------------*根据user进行权限判断*----------------

            //---------**********----------------
            string sqlWhere = @"select * from BIS_SAFTYCHECKDATA t left join (select checkdataid,count(1) as usetime from bis_saftycheckdatarecord a left join (select recid,checkdataid from bis_saftycheckdatadetailed  group by recid,checkdataid)
 b on a.id=b.recid group by checkdataid) b
on t.id=b.checkdataid where checkdatatype='1'";
            string arg = user.OrganizeCode;
            sqlWhere += string.Format("  and belongdeptcode like '{0}%'", arg);
            //if (user.RoleName.Contains("厂级") || user.RoleName.Contains("公司级"))
            //{
            //    arg = user.OrganizeCode;
            //    sqlWhere += string.Format("  and belongdeptcode like '{0}%'", arg);
            //}
            //else
            //{
            //    arg = user.DeptCode;
            //    sqlWhere += string.Format("  and  belongdeptcode in(select encode from base_department  where encode like '{0}%'  or senddeptid='{1}')", arg, user.DeptId);
            //}
            return this.BaseRepository().FindList(sqlWhere);
        }
        public List<DistinctArray> getDistinctGroup(string recid)
        {
            string sqlWhere = "select  t.belongdistrict,t.belongdistrictid,t.BelongDistrictCode from BIS_SAFTYCHECKDATADETAILED t  where RECID='" + recid + "'  group by t.belongdistrict,t.belongdistrictid,t.BelongDistrictCode ";
            DataTable dt = this.BaseRepository().FindTable(sqlWhere);
            List<DistinctArray> listarr = new List<DistinctArray>();
            foreach (DataRow item in dt.Rows)
            {
                DistinctArray arr = new DistinctArray();
                arr.areaname = item[0].ToString();
                arr.areanameid = item[1].ToString();
                arr.areanamecode = item[2].ToString();
                string sqlD = string.Format(@"select disreictchargeperson,b.userid  as disreictchargepersonid,chargedept,chargedeptcode,b.mobile as linktel from bis_district a
left join base_user b on  a.disreictchargepersonid=b.account  where districtid='{0}'", item[1].ToString());
                DataTable dtD = this.BaseRepository().FindTable(sqlD);
                if (dtD.Rows.Count > 0)
                {
                    arr.disreictchargeperson = dtD.Rows[0][0].ToString();
                    arr.disreictchargepersonid = dtD.Rows[0][1].ToString();
                    arr.chargedept = dtD.Rows[0][2].ToString();
                    arr.chargedeptcode = dtD.Rows[0][3].ToString();
                    arr.linktel = dtD.Rows[0][4].ToString();
                }
                string sqlWhere2 = "select  t.riskName as riskname,id as risknameid from BIS_SAFTYCHECKDATADETAILED t  where RECID='" + recid + "' and belongdistrictid='" + item[1].ToString() + "'";

                arr.riskdescarray = this.BaseRepository().FindTable(sqlWhere2);
                listarr.Add(arr);
            }
            return listarr;
        }


        public List<DistinctArray> getDistinctGroupDj(string recid, string checkdatatype, Operator user)
        {
            var sqlWhere = "";
            if (checkdatatype == "1")
            {
                sqlWhere = "select  t.belongdistrict,t.belongdistrictid,t.BelongDistrictCode from BIS_SAFTYCHECKDATADETAILED t  where RECID='" + recid + "'  group by t.belongdistrict,t.belongdistrictid,t.BelongDistrictCode ";
            }
            else
            {
                sqlWhere = string.Format(@"select  b.belongdistrict,b.belongdistrictid,b.BelongDistrictCode from BIS_SAFTYCONTENT a left join BIS_SAFTYCHECKDATADETAILED b 
                  on a.DETAILID=b.id  where instr(checkmanaccount,'{0}')>0 and a.recid='{1}' 
                 and DETAILID in (select id from BIS_SAFTYCHECKDATADETAILED where recid='{1}') 
                 group by b.belongdistrict,b.belongdistrictid,b.BelongDistrictCode", user.Account, recid);
            }
            DataTable dt = this.BaseRepository().FindTable(sqlWhere);
            List<DistinctArray> listarr = new List<DistinctArray>();
            foreach (DataRow item in dt.Rows)
            {
                DistinctArray arr = new DistinctArray();
                arr.areaname = item[0].ToString();
                arr.areanameid = item[1].ToString();
                arr.areanamecode = item[2].ToString();
                string sqlD = string.Format(@"select disreictchargeperson,b.userid  as disreictchargepersonid,chargedept,chargedeptcode,b.mobile as linktel from bis_district a
left join base_user b on  a.disreictchargepersonid=b.account  where districtid='{0}'", item[1].ToString());
                DataTable dtD = this.BaseRepository().FindTable(sqlD);
                if (dtD.Rows.Count > 0)
                {
                    arr.disreictchargeperson = dtD.Rows[0][0].ToString();
                    arr.disreictchargepersonid = dtD.Rows[0][1].ToString();
                    arr.chargedept = dtD.Rows[0][2].ToString();
                    arr.chargedeptcode = dtD.Rows[0][3].ToString();
                    arr.linktel = dtD.Rows[0][4].ToString();
                }
                if (checkdatatype == "1")
                {
                    string sqlWhere2 = "select  t.riskName as riskname,id as risknameid from BIS_SAFTYCHECKDATADETAILED t  where RECID='" + recid + "' and belongdistrictid='" + item[1].ToString() + "'";
                    arr.riskdescarray = this.BaseRepository().FindTable(sqlWhere2);
                }
                else
                {
                    string sqlWhere2 = string.Format(@"select  b.riskname as riskname,b.id as risknameid,belongdistrictid from BIS_SAFTYCONTENT a left join BIS_SAFTYCHECKDATADETAILED b 
on a.DETAILID=b.id where instr(checkmanaccount,'{0}')>0 and a.recid='{1}' 
and DETAILID in (select id from BIS_SAFTYCHECKDATADETAILED where recid='{1}') 
and belongdistrictid='{2}'  group by  b.riskname,b.id,belongdistrictid", user.Account, recid, item[1].ToString());
                    arr.riskdescarray = this.BaseRepository().FindTable(sqlWhere2);
                }
                listarr.Add(arr);
            }
            return listarr;
        }

        public DataTable selectCheckContent(string risknameid, string userAccount, string type)
        {
            string sqlWhere;
            if (type == "1")//如果是日常安全检查
            {
                sqlWhere = string.Format("select checkcontent,ID as checkcontentid,checkobject,checkobjectid,checkobjecttype from Bis_Saftycheckdatadetailed where id='{0}'", risknameid);


            }
            else //其他安全检查则要判断当前用户是否是检查人员
            {

                sqlWhere = string.Format(@"select o.SaftyContent checkContent,r.id checkContentId,o.CheckObject,o.CheckObjectId,o.CheckObjectType,issure,remark,riskname htdesc from BIS_SAFTYCONTENT o
                                             left join bis_saftycheckdatadetailed r  on o.recid=r.RecID and DETAILID=r.ID 
                                              where r.id='{0}' and  instr(o.checkmanaccount,'{1}')>0", risknameid, userAccount);
            }

            DataTable dt = this.BaseRepository().FindTable(sqlWhere);
            return dt;
        }

        public DataTable getCheckPlanList(Operator user, string ctype)
        {
            string sqlWhere = "select CHECKDATARECORDNAME as checkplanname,id as checkplannameid from BIS_SAFTYCHECKDATARECORD where 1=1  and CheckDataType='" + ctype + "' order by CreateDate desc";
            //--------------*根据user进行权限判断*----------------

            //---------**********----------------
            return this.BaseRepository().FindTable(sqlWhere);
        }



        #region  安全检查统计
        /// <summary>
        /// 安全检查统计
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public object GetCheckStatistics(ERCHTMS.Code.Operator user, string deptCode = "")
        {
            string roleNames = user.RoleName;
            var arg = "";
            string sql = string.Format(@"select checkdatatype ,nvl(sum(num),0),count(1) from bis_saftycheckdatarecord a 
left join (
select safetycheckobjectid,count(1) num from bis_htbaseinfo where safetycheckobjectid is not null group by safetycheckobjectid) b
on a.id=b.safetycheckobjectid  where datatype in(0,2) and to_char(CreateDate,'yyyy')='{0}'", DateTime.Now.Year);
            if (DbHelper.DbType == DatabaseType.MySql)
            {
                sql = string.Format(@"select checkdatatype ,case when isnull(sum(num)) then 0 else sum(num) end,count(1) from bis_saftycheckdatarecord a 
left join (
select safetycheckobjectid,count(1) num from bis_htbaseinfo where safetycheckobjectid is not null group by safetycheckobjectid)b
on a.id=b.safetycheckobjectid  where 1=1 and date_format(CreateDate,'%Y')='{0}'", DateTime.Now.Year);
            }

            if (!string.IsNullOrEmpty(deptCode))
            {
                sql += string.Format("  and belongdept like '{0}%'", deptCode);
            }
            else
            {
                if (user.RoleName.Contains("省级"))
                {
                    sql += string.Format("  and ((belongdept in(select encode from BASE_DEPARTMENT where deptcode like '{0}%')) and (status=0 or status=2))", user.NewDeptCode);
                }
                else if (user.RoleName.Contains("厂级") || user.RoleName.Contains("公司") || user.RoleName.Contains("省级"))
                {
                    arg = user.OrganizeCode;
                    sql += string.Format("  and (belongdept like '{0}%' and (status=0 or status=2))", arg);
                }
                else
                {
                    arg = user.DeptCode;
                    sql += string.Format("  and ((belongdept in(select encode from base_department  where encode like '{0}%')) and (status=0 or status=2))", arg, user.DeptId);
                }
            }
            sql += " group by checkdatatype";
            DataTable dt = this.BaseRepository().FindTable(sql);
            int count1 = 0; int count2 = 0; int count3 = 0; int count4 = 0; int count5 = 0;
            int countyh1 = 0; int countyh2 = 0; int countyh3 = 0; int countyh4 = 0; int countyh5 = 0;
            if (dt.Select("checkdatatype='1'").Length != 0)
            {
                countyh1 = int.Parse(dt.Select("checkdatatype='1'")[0][1].ToString());
                count1 = int.Parse(dt.Select("checkdatatype='1'")[0][2].ToString());
            }
            if (dt.Select("checkdatatype='2'").Length != 0)
            {
                countyh2 = int.Parse(dt.Select("checkdatatype='2'")[0][1].ToString());
                count2 = int.Parse(dt.Select("checkdatatype='2'")[0][2].ToString());
            }
            if (dt.Select("checkdatatype='3'").Length != 0)
            {
                countyh3 = int.Parse(dt.Select("checkdatatype='3'")[0][1].ToString());
                count3 = int.Parse(dt.Select("checkdatatype='3'")[0][2].ToString());
            }
            if (dt.Select("checkdatatype='4'").Length != 0)
            {
                countyh4 = int.Parse(dt.Select("checkdatatype='4'")[0][1].ToString());
                count4 = int.Parse(dt.Select("checkdatatype='4'")[0][2].ToString());
            }
            if (dt.Select("checkdatatype='5'").Length != 0)
            {
                countyh5 = int.Parse(dt.Select("checkdatatype='5'")[0][1].ToString());
                count5 = int.Parse(dt.Select("checkdatatype='5'")[0][2].ToString());
            }
            int sum = count1 + count2 + count3 + count4 + count5;
            int sumyh = countyh1 + countyh2 + countyh3 + countyh4 + countyh5;

            List<object> list = new List<object>();
            decimal percent = sum == 0 ? 0 : decimal.Parse(count1.ToString()) / decimal.Parse(sum.ToString());
            decimal percentyh = sumyh == 0 ? 0 : decimal.Parse(countyh1.ToString()) / decimal.Parse(sumyh.ToString());
            list.Add(new { checktype = "日常检查", checknum = count1, checkrate = Math.Round(percent, 4), problemnum = countyh1, problemrate = Math.Round(percentyh, 4) });

            percent = sum == 0 ? 0 : decimal.Parse(count2.ToString()) / decimal.Parse(sum.ToString());
            percentyh = sumyh == 0 ? 0 : decimal.Parse(countyh2.ToString()) / decimal.Parse(sumyh.ToString());
            list.Add(new { checktype = "专项检查", checknum = count2, checkrate = Math.Round(percent, 4), problemnum = countyh2, problemrate = Math.Round(percentyh, 4) });

            percent = sum == 0 ? 0 : decimal.Parse(count3.ToString()) / decimal.Parse(sum.ToString());
            percentyh = sumyh == 0 ? 0 : decimal.Parse(countyh3.ToString()) / decimal.Parse(sumyh.ToString());
            list.Add(new { checktype = "节假日检查", checknum = count3, checkrate = Math.Round(percent, 4), problemnum = countyh3, problemrate = Math.Round(percentyh, 4) });

            percent = sum == 0 ? 0 : decimal.Parse(count4.ToString()) / decimal.Parse(sum.ToString());
            percentyh = sumyh == 0 ? 0 : decimal.Parse(countyh4.ToString()) / decimal.Parse(sumyh.ToString());
            list.Add(new { checktype = "季节性检查", checknum = count4, checkrate = Math.Round(percent, 4), problemnum = countyh4, problemrate = Math.Round(percentyh, 4) });

            percent = sum == 0 ? 0 : decimal.Parse(count5.ToString()) / decimal.Parse(sum.ToString());
            percentyh = sumyh == 0 ? 0 : decimal.Parse(countyh5.ToString()) / decimal.Parse(sumyh.ToString());
            list.Add(new { checktype = "综合性检查", checknum = count5, checkrate = Math.Round(percent, 4), problemnum = countyh5, problemrate = Math.Round(percentyh, 4) });
            return new { checktotalnum = sum, checkproblemnum = sumyh, checklist = list };
        }
        #endregion

        /// <summary>
        /// 获取检查对象
        /// </summary>
        /// <param name="recId"></param>
        /// <returns></returns>
        public DataTable GetCheckObjects(string recId, int mode = 0)
        {
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (mode == 0)
            {
                return BaseRepository().FindTable(string.Format("select distinct t.checkobjectid,t.checkobject,checkobjecttype from BIS_SAFTYCHECKDATADETAILED t where t.recid='{0}' order by checkobject asc ", recId));
            }
            else
            {
                return BaseRepository().FindTable(string.Format("select distinct t.checkobjectid,t.checkobject,checkobjecttype from BIS_SAFTYCHECKDATADETAILED t where t.recid='{0}' and instr((','||CheckManId||','),',{1},')>0 order by checkobject asc ", recId, user.Account));
            }

        }
        /// <summary>
        /// 获取检查内容
        /// </summary>
        /// <param name="checkObjId"></param>
        /// <returns></returns>
        public DataTable GetCheckItems(string checkObjId, string recId, int mode = 0)
        {
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DataTable dt = new DataTable();
            if (mode == 0)
            {
                dt = BaseRepository().FindTable(string.Format("select t.id checkid,t.checkcontent,t.issure,case when c.issure is null then 0 when c.issure=0 then 1 else 2 end isreg，'' htdesc,0 htcount,0 wzcount,0 wtcount,'' equipmentsno,'' equipmentname,t.checkobjectid,t.checkobjecttype,t.checkobject,'' areacode,'' areaname,case when c.issure is null then 0 else 1 end status,c.remark from BIS_SAFTYCHECKDATADETAILED t left join bis_saftycontent c on t.id=c.detailid where t.checkobjectid='{0}' and t.recid='{1}' order by autoid,checkobject asc ", checkObjId, recId));
            }
            else
            {
                dt = BaseRepository().FindTable(string.Format("select t.id checkid,t.checkcontent,t.issure,case when c.issure is null then 0 when c.issure=0 then 1 else 2 end isreg ，'' htdesc,0 htcount,0 wzcount,0 wtcount,'' equipmentsno,'' equipmentname,t.checkobjectid,t.checkobjecttype,t.checkobject,'' areacode,'' areaname,case when c.issure is null then 0 else 1 end status,c.remark from BIS_SAFTYCHECKDATADETAILED t  left join bis_saftycontent c on t.id=c.detailid where t.checkobjectid='{0}' and t.recid='{1}' and instr((','||CheckManId||','),',{2},')>0  order by autoid,checkobject asc ", checkObjId, recId, user.Account));
            }
            string sql = string.Format("select itemvalue from  base_dataitemdetail  where itemid =(select  itemid from base_dataitem where itemcode='{0}') and itemname='{1}'", "CacheSafetyCheck", user.OrganizeCode);
            object obj = new RepositoryFactory().BaseRepository().FindObject(sql);
            //string isCache= obj == null || obj == System.DBNull.Value ? "" : obj.ToString();
            //isCache = string.IsNullOrEmpty(isCache) ? "0" : isCache;
            //if (isCache=="1")
            //{
            foreach (DataRow dr in dt.Rows)
            {
                string checkid = dr["checkid"].ToString();
                DataTable dtContent = GetCheckContentInfo(checkid);
                if (dtContent.Rows.Count > 0)
                {
                    DataRow dr1 = dtContent.Rows[0];
                    List<int> list = GetHtAndWzCount(checkid, 1);
                    string type = dr1["checkobjecttype"].ToString();
                    type = string.IsNullOrEmpty(type) ? "3" : type;
                    string objectId = dr1["checkobjectid"].ToString();
                    string sno = "";
                    string equname = "";
                    string areacode = "";
                    string areaname = "";
                    if (type == "0")
                    {
                        DataTable dtEqu = GetEquimentInfo(objectId);
                        if (dtEqu.Rows.Count > 0)
                        {
                            sno = dtEqu.Rows[0][2].ToString();
                            equname = dtEqu.Rows[0][1].ToString();
                            areacode = dtEqu.Rows[0][3].ToString();
                            areaname = dtEqu.Rows[0][4].ToString();

                        }
                        dtEqu.Dispose();
                    }
                    dr["htdesc"] = dr1["htdesc"].ToString();
                    dr["equipmentsno"] = sno;
                    dr["status"] = dr1["status"].ToString();
                    dr["equipmentname"] = equname;
                    dr["areacode"] = areacode;
                    dr["areaname"] = areaname;
                    dr["htcount"] = list[0];
                    dr["wzcount"] = list[1];
                    dr["wtcount"] = list[2];
                }
            }
            // }
            return dt;

        }

        /// <summary>
        /// 获取检查表检查内容
        /// </summary>
        /// <param name="checkId"></param>
        /// <returns></returns>
        public List<object> GetCheckContents(string checkId, int mode = 0)
        {
            List<dynamic> list = new List<dynamic>();
            DataTable dtObjects = GetCheckObjects(checkId);
            foreach (DataRow dr in dtObjects.Rows)
            {
                var entity = list.Where(x => x.checkobject == dr[1].ToString()).FirstOrDefault();
                if (entity == null)
                {
                    list.Add(new
                    {
                        objectid = dr[0].ToString(),
                        checkobject = dr[1].ToString(),
                        checkobjecttype = dr[2].ToString(),
                        checkItems = GetCheckItems(dr[0].ToString(), checkId, mode)
                    });
                }
                else
                {
                    DataTable dt = GetCheckItems(dr[0].ToString(), checkId, mode);
                    var dtItems = ((DataTable)entity.checkItems);
                    foreach (DataRow dr1 in dt.Rows)
                    {
                        var newRow = dtItems.NewRow();
                        newRow["checkid"] = dr1["checkid"].ToString();
                        newRow["checkcontent"] = dr1["checkcontent"].ToString();
                        newRow["isreg"] = dr1["isreg"].ToString();
                        dtItems.Rows.Add(newRow);
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// 获取隐患和违章数量（顺序依次为隐患，违章）
        /// </summary>
        /// <param name="checkId">检查记录Id</param>
        /// <param name="mode">查询方式（0：获取关联检查记录的所有隐患和违章数量，1：获取检查项目登记的隐患和违章数量）</param>
        /// <returns></returns>
        public List<int> GetHtAndWzCount(string recId, int mode)
        {
            List<int> list = new List<int>();
            int count = 0;
            if (mode == 0)
            {
                //隐患数量
                count = BaseRepository().FindObject(string.Format("select count(1) from bis_htbaseinfo where safetycheckobjectid='{0}'", recId)).ToInt();
                list.Add(count);
                //违章数量
                count = BaseRepository().FindObject(string.Format("select count(1) from BIS_LLLEGALREGISTER where reseverone='{0}'", recId)).ToInt();
                list.Add(count);
                //问题数量
                count = BaseRepository().FindObject(string.Format("select count(1) from BIS_QUESTIONINFO where CHECKID='{0}'", recId)).ToInt();
                list.Add(count);
            }
            else
            {
                //隐患数量
                count = BaseRepository().FindObject(string.Format("select count(1) from bis_htbaseinfo where relevanceid='{0}'", recId)).ToInt();
                list.Add(count);
                //违章数量
                count = BaseRepository().FindObject(string.Format("select count(1) from BIS_LLLEGALREGISTER where resevertwo='{0}'", recId)).ToInt();
                list.Add(count);
                //问题数量
                count = BaseRepository().FindObject(string.Format("select count(1) from BIS_QUESTIONINFO where CORRELATIONID='{0}'", recId)).ToInt();
                list.Add(count);
            }
            return list;
        }
        /// <summary>
        /// 获取检查中登记的隐患列表
        /// </summary>
        /// <param name="recId"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public DataTable GetHtList(string recId, int mode)
        {
            DataTable dt = new DataTable();
            if (mode == 0)
            {
                dt = BaseRepository().FindTable(string.Format("select id hid,HIDCODE sno,HIDNAME name,HIDDESCRIBE describe,WORKSTREAM,rankname lev from V_BASEHIDDENINFO where safetycheckobjectid='{0}'", recId));
            }
            if (mode == 0)
            {
                dt = BaseRepository().FindTable(string.Format("select id hid,HIDCODE sno,HIDNAME name,HIDDESCRIBE describe,WORKSTREAM,rankname lev from V_BASEHIDDENINFO where relevanceid='{0}'", recId));
            }
            return dt;
        }
        /// <summary>
        /// 获取检查中登记的违章列表
        /// </summary>
        /// <param name="recId"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public DataTable GetWzList(string recId, int mode)
        {
            DataTable dt = new DataTable();
            if (mode == 0)
            {
                dt = BaseRepository().FindTable(string.Format("select id hid,LLLEGALNUMBER sno,LLLEGALDESCRIBE describe,flowstate wrokstream,LLLEGALLEVEL lev from BIS_LLLEGALREGISTER  where safetycheckobjectid='{0}'", recId));
            }
            if (mode == 0)
            {
                dt = BaseRepository().FindTable(string.Format("select id hid,LLLEGALNUMBER sno,LLLEGALDESCRIBE describe,flowstate wrokstream,LLLEGALLEVEL lev from BIS_LLLEGALREGISTER  where relevanceid='{0}'", recId));
            }
            return dt;
        }
        /// <summary>
        /// 获取检查内容详情
        /// </summary>
        /// <param name="itemid"></param>
        /// <returns></returns>
        public DataTable GetCheckContentInfo(string itemid)
        {
            return BaseRepository().FindTable(string.Format("select t.id checkid,saftycontent,checkcontent,a.issure,a.remark,t.riskname htdesc,case when a.issure is null then 0 else 1 end status,t.checkobjectid,t.checkobjecttype from BIS_SAFTYCHECKDATADETAILED t left join bis_saftycontent a on t.id=a.detailid where t.id='{0}'", itemid));
        }
        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetEquimentInfo(string id)
        {
            return BaseRepository().FindTable(string.Format(@"select id, equipmentname,equipmentno,districtcode,district  from (select id,a.equipmentname,a.equipmentno,districtcode,district from bis_equipment a union all select id,b.equipmentname,b.equipmentno,districtcode,district from bis_specialequipment b) t
where id='{0}'", id));
        }
        #endregion
        /// <summary>
        /// 执行周期性计划，根据规则自动创建检查计划
        /// </summary>
        /// <returns></returns>
        public string AutoCreateCheckPlan()
        {
            StringBuilder content = new StringBuilder("执行周期性安全检查计划信息：\r\n");
            Regex reg = new Regex(@"^\d{4}-\d{2}-\d{2}日常检查$");
            StringBuilder sb = new StringBuilder();
            DataTable dt = BaseRepository().FindTable(string.Format(@"select checkdatarecordname,t.id,t.checkdatatype,t.createuserid,t.createusername,t.createuserdeptcode,t.createuserorgcode,t.checklevel,t.belongdeptid,t.belongdept,t.checkmanageman,t.checkmanagemanid,t.checkdeptcode,t.checkdeptid,t.checkdept,t.checkusers,t.checkuserids,t.checkeddepartid,t.checkeddepart,t.remark,t.autotype,t.days,t.months,t.seltype,t.thweeks,t.isskip,rounds,weeks,CheckBeginTime from BIS_SAFTYCHECKDATARECORD t where t.isauto=1 and isover=0 and checkbegintime<to_date('{0}','yyyy-mm-dd hh24:mi:ss')", DateTime.Now.ToString("yyyy-MM-dd 00:00:00")));
            StringBuilder sbSql = new StringBuilder();
            foreach (DataRow dr in dt.Rows)
            {
                bool flag = true;
                bool isSkip = false;
                int count = 0;
                string name = dr["checkdatarecordname"].ToString();
                string rounds = "0";
                if (dr["checkdatatype"].ToString() == "1")
                {
                    name = !reg.IsMatch(name) ? name : string.Format("{0}日常检查", DateTime.Now.ToString("yyyy-MM-dd"));
                }
                else
                {
                    rounds = dr["rounds"] == null ? "0" : dr["rounds"].ToString();
                }
                string startDate = DateTime.Now.ToString("yyyy-MM-dd");
                string newId = Guid.NewGuid().ToString();
                string endDate = "";
                string months = "," + dr["months"].ToString() + ",";
                string weeks = "," + dr["weeks"].ToString() + ",";

                //每天
                if (dr["autotype"].ToString() == "0")
                {
                    endDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                    if (dr["isskip"].ToString() == "1") //跳过双休
                    {
                        isSkip = "Saturday,Sunday".Contains(DateTime.Now.DayOfWeek.ToString());
                    }
                }
                if (dr["autotype"].ToString() == "1")//每周
                {

                    flag = weeks.Contains("," + DateTime.Now.DayOfWeek.GetHashCode().ToString() + ",");//星期几
                    if (dr["isskip"].ToString() == "1")//跳过双休
                    {
                        isSkip = "Saturday,Sunday".Contains(DateTime.Now.DayOfWeek.ToString());
                    }
                    if (dr["checkdatatype"].ToString() == "1")
                    {
                        endDate = startDate;
                    }
                    else
                    {
                        endDate = DateTime.Now.AddDays(int.Parse(rounds)).ToString("yyyy-MM-dd");
                    }
                }
                if (dr["autotype"].ToString() == "2")//每月
                {


                    string selType = dr["seltype"].ToString();
                    string days = "," + dr["days"].ToString() + ",";
                    weeks = "," + dr["thweeks"].ToString() + ",";
                    if (months.Contains("," + DateTime.Now.Month.ToString() + ","))
                    {
                        if (dr["checkdatatype"].ToString() == "1")
                        {
                            endDate = startDate;
                        }
                        else
                        {
                            endDate = DateTime.Now.AddDays(int.Parse(rounds)).ToString("yyyy-MM-dd");
                        }
                        if (selType == "0")
                        {
                            flag = days.Contains("," + DateTime.Now.Day.ToString() + ",");//按日期

                        }
                        else
                        {
                            count = DateTime.Now.Day / 7;
                            flag = weeks.Contains("," + count + ",");//按日期
                        }
                        if (dr["isskip"].ToString() == "1")//跳过双休
                        {
                            isSkip = "Saturday,Sunday".Contains(DateTime.Now.DayOfWeek.ToString());
                        }
                    }
                    else
                    {
                        flag = false;
                    }
                }
                if (flag == true && isSkip == false)
                {
                    sb.AppendFormat("{0},", newId);
                    sbSql.AppendFormat(string.Format("insert into BIS_SAFTYCHECKDATARECORD(CREATEDATE,id,checkdatatype,createuserid,createusername,createuserdeptcode,createuserorgcode,checklevel,belongdeptid,belongdept,checkmanageman,checkmanagemanid,checkdeptcode,checkdeptid,checkdept,checkusers,checkuserids,checkeddepartid,checkeddepart,remark,isauto,checkdatarecordname,CheckBeginTime,CheckEndTime,CheckLevelID,CheckMan,CheckManID) select to_date('{5}','yyyy-mm-dd hh24:mi:ss'),'{0}',t.checkdatatype,t.createuserid,t.createusername,t.createuserdeptcode,t.createuserorgcode,t.checklevel,t.belongdeptid,t.belongdept,t.checkmanageman,t.checkmanagemanid ,t.checkdeptcode,t.checkdeptid,t.checkdept,t.checkusers,t.checkuserids,t.checkeddepartid,t.checkeddepart,t.remark,0,'{2}',to_date('{3}','yyyy-mm-dd hh24:mi:ss'),to_date('{4}','yyyy-mm-dd hh24:mi:ss'),CheckLevelID,CheckMan,CheckManID from BIS_SAFTYCHECKDATARECORD t where t.isauto=1  and id='{1}';\r\n", newId, dr["id"].ToString(), name, startDate, endDate, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    sbSql.Append(string.Format("insert into bis_saftycheckdatadetailed(id,riskname,checkcontent,belongdeptid,belongdept,recid,checkman,checkmanid,checkdataid,checkobject,checkobjectid,checkobjecttype,checkstate) select '{2}' || rownum,t.riskname,t.checkcontent,t.belongdeptid,t.belongdept,'{1}',t.checkman,t.checkmanid,t.checkdataid,t.checkobject,t.checkobjectid,t.checkobjecttype,t.checkstate from bis_saftycheckdatadetailed t where recid='{0}';\r\n", dr["id"].ToString(), newId, Guid.NewGuid().ToString()));
                    //                    count = BaseRepository().ExecuteBySql(string.Format(@"insert into BIS_SAFTYCHECKDATARECORD(CREATEDATE,id,checkdatatype,createuserid,createusername,createuserdeptcode,createuserorgcode,checklevel,belongdeptid,belongdept,checkmanageman,checkmanagemanid,checkdeptcode,checkdeptid,checkdept,checkusers,checkuserids,checkeddepartid,checkeddepart,remark,isauto,checkdatarecordname,CheckBeginTime,CheckEndTime,CheckLevelID,CheckMan,CheckManID) select to_date('{5}','yyyy-mm-dd hh24:mi:ss'),'{0}',t.checkdatatype,t.createuserid,t.createusername,t.createuserdeptcode,t.createuserorgcode,t.checklevel,t.belongdeptid,t.belongdept,t.checkmanageman,t.checkmanagemanid ,t.checkdeptcode,t.checkdeptid,t.checkdept,t.checkusers,t.checkuserids,t.checkeddepartid,t.checkeddepart,t.remark,0,'{2}',to_date('{3}','yyyy-mm-dd hh24:mi:ss'),to_date('{4}','yyyy-mm-dd hh24:mi:ss'),CheckLevelID,CheckMan,CheckManID from BIS_SAFTYCHECKDATARECORD t where t.isauto=1  and id='{1}'", newId, dr["id"].ToString(), name, startDate, endDate, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    //                    if (count > 0)
                    //                    {
                    //                        BaseRepository().ExecuteBySql(string.Format(@"insert into bis_saftycheckdatadetailed(id,riskname,checkcontent,belongdeptid,belongdept,recid,checkman,checkmanid,checkdataid,checkobject,checkobjectid,checkobjecttype,checkstate) 
                    //select '{2}' || rownum,t.riskname,t.checkcontent,t.belongdeptid,t.belongdept,'{1}',t.checkman,t.checkmanid,t.checkdataid,t.checkobject,t.checkobjectid,t.checkobjecttype,t.checkstate from bis_saftycheckdatadetailed t where recid='{0}'", dr["id"].ToString(), newId, Guid.NewGuid().ToString()));
                    //                        sb.AppendFormat("{0},", newId);


                    //}
                    content.AppendFormat("原计划信息：{0}，关联计划ID:" + sb.ToString().Trim(',') + "\r\n", Newtonsoft.Json.JsonConvert.SerializeObject(dr.ItemArray));
                    if (dr["checkdatatype"].ToString() != "1")
                    {
                        object userId = BaseRepository().FindObject(string.Format("select userid from base_user where account='{0}'", dr["checkmanagemanid"].ToString()));
                        if (userId != null)
                        {
                            string title = "您好,按相关规定，您今天需带队开展检查，请知晓";
                            sbSql.AppendFormat("insert into bis_safenote(id,createuserid,createdate,createusername,time,value,type,recid) values('{0}','{1}',{2},'{3}',{4},'{5}','{6}','{7}');\r\n", Guid.NewGuid().ToString(), userId, "sysdate", dr["checkmanageman"].ToString(), "sysdate", title, "5", newId);
                        }
                    }
                }

            }
            if (sbSql.Length > 0)
            {
                sbSql.Append("commit;\r\n end;");
                if (BaseRepository().ExecuteBySql("begin \r\n" + sbSql.ToString()) > 0)
                {

                }
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + DateTime.Now.ToString("yyyyMMdd") + ".log"), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ：" + content.ToString() + "\r\n");
            }
            return sbSql.ToString().Length == 0 ? "没有计划可执行" : content.ToString();

        }
        /// <summary>
        /// 设置是否中止周期性计划任务
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int SetStatus(string id, int status)
        {
            return BaseRepository().ExecuteBySql(string.Format("update BIS_SAFTYCHECKDATARECORD set isover={0} where id='{1}'", status, id));
        }
    }
}
