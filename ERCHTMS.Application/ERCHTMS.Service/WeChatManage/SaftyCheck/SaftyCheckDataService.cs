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

namespace ERCHTMS.Service.SaftyCheck
{
    /// <summary>
    /// 描 述：安全检查表
    /// </summary>
    public class SaftyCheckDataService : RepositoryFactory<SaftyCheckDataEntity>, SaftyCheckDataIService
    {
        #region 获取数据
        public DataTable GetCheckStat()
        {
            //权限判断
            var user = OperatorProvider.Provider.Current();
            string deptCode = OperatorProvider.Provider.Current().DeptCode;
            string sql = " where 1=1 and to_char(CreateDate,'yyyy')='" + DateTime.Now.Year + "'";
            var arg = "";
            if (!user.RoleName.Contains("厂级") && !string.IsNullOrEmpty(user.DeptId))
            {
                arg = user.DeptCode;
                sql += string.Format("  and belongdept in(select encode from base_department  where encode like '{0}%'  or senddeptid='{1}')", arg, user.DeptId);
            }
            else
            {
                arg = user.DeptCode.Substring(0, 3);
                sql += string.Format("  and belongdept like '{0}%'", arg);
            }

            //
            string sqlWhere = string.Format(@"select count(*) as num  from BIS_SAFTYCHECKDATARECORD {0}
union all
select nvl(b.num, 0) num
  from(select t.itemvalue, count(1) as num
          from (select*from BASE_DATAITEMDETAIL 
         where itemid = (select itemid from base_dataitem where itemcode='SaftyCheckType') order by itemvalue) t
          group by itemvalue) a
   left join(select to_char(CHECKDATATYPE) as itemvalue, count(1) as num
 
                from (select * from BIS_SAFTYCHECKDATARECORD {0}) t", sql);
            sqlWhere += @" where 1 = 1
                group by CHECKDATATYPE) b
     on a.itemvalue = b.itemvalue ";
            DataTable dt = this.BaseRepository().FindTable(sqlWhere);
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
                string sql = string.Format(@"select checkDataType,count(1) from   bis_saftycheckdatarecord t left join  (select recid from bis_saftycontent where CheckManAccount='{0}' group by  recid)t1
on t.id=t1.recid  where recid is not null and ((instr(solveperson,'{0}'))=0 or solveperson is null)  and belongdept like  '{1}%' group by checkDataType", user.Account,user.OrganizeCode);
                DataTable dt = this.BaseRepository().FindTable(sql);
                count1 = dt.Select("checkDataType='2'").Length == 0 ? 0 : int.Parse(dt.Select("checkDataType='2'")[0][1].ToString());
                count2 = dt.Select("checkDataType='3'").Length == 0 ? 0 : int.Parse(dt.Select("checkDataType='3'")[0][1].ToString());
                count3 = dt.Select("checkDataType='4'").Length == 0 ? 0 : int.Parse(dt.Select("checkDataType='4'")[0][1].ToString());
                count4 = dt.Select("checkDataType='5'").Length == 0 ? 0 : int.Parse(dt.Select("checkDataType='5'")[0][1].ToString());
                string sqlRc = string.Format(@"select count(1) from bis_saftycheckdatarecord where checkDataType='1' and  instr(checkmanid,'{0}')>0  and ((instr(solveperson,'{0}'))=0 or solveperson is null)  and belongdept like  '{1}%' ", user.Account,user.OrganizeCode);
                countRc = this.BaseRepository().FindObject(sqlRc).ToInt();

            }
            else if (mode == 1)
            {
                if (user.RoleName.Contains("公司"))
                {
                    string sql = string.Format("select checkDataType,count(1) from bis_saftycheckdatarecord where solveperson is null and belongdept like '{0}%' group by checkDataType",user.DeptCode);
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
) b on a.solveperson  like '%'||b.account||'%' where (account is not null or solveperson=',,'))d
 on t.id=d.id  where t.id in(select id from bis_saftycheckdatarecord t left join  (select recid from (select id,(','||CheckManAccount||',') as CheckManAccount,recid  from  BIS_SAFTYCONTENT) a 
left join (select (','||account||',') as account from base_user where  departmentcode in(select encode from base_department  where encode= '{0}')
) b on a.CheckManAccount  like '%'||b.account||'%' where account is not null  group by recid)t1
on t.id=t1.recid where t1.recid is not null) group by checkDataType", user.DeptCode);
                    DataTable dt = this.BaseRepository().FindTable(sql);
                    count1 = dt.Select("checkDataType='2'").Length == 0 ? 0 : int.Parse(dt.Select("checkDataType='2'")[0][1].ToString());
                    count2 = dt.Select("checkDataType='3'").Length == 0 ? 0 : int.Parse(dt.Select("checkDataType='3'")[0][1].ToString());
                    count3 = dt.Select("checkDataType='4'").Length == 0 ? 0 : int.Parse(dt.Select("checkDataType='4'")[0][1].ToString());
                    count4 = dt.Select("checkDataType='5'").Length == 0 ? 0 : int.Parse(dt.Select("checkDataType='5'")[0][1].ToString());
                    string sqlRc = string.Format(@"select count(1) from bis_saftycheckdatarecord t right join (select * from (select id,(','||checkmanid||',') as solveperson  from  bis_saftycheckdatarecord where  checkDataType='1') a 
left join (
select (','||account||',') as account from base_user where  departmentcode in(select encode from base_department  where encode= '{0}')
) b on a.solveperson  like '%'||b.account||'%' where (account is not null or solveperson=',,')  )d
 on t.id=d.id  where t.id in(
 select t.id from  bis_saftycheckdatarecord t left join(select id from (select id,(','||checkmanid||',') as checkmanid  from  bis_saftycheckdatarecord) a 
left join (
select (','||account||',') as account from base_user where  departmentcode in(select encode from base_department  where encode= '{0}' )
) b on a.checkmanid  like '%'||b.account||'%' where account is not null group by id)t1
on t.ID=t1.id where t1.id is not null
) ", user.DeptCode);
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
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public int SaveForm(string keyValue, SaftyCheckDataEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                return this.BaseRepository().Update(entity);
            }
            else
            {
                entity.Create();
                return this.BaseRepository().Insert(entity);
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
            string arg = "";
            if (user.RoleName.Contains("厂级") || user.RoleName.Contains("公司"))
            {
                arg = user.DeptCode.Substring(0, 3);
                sqlWhere += string.Format("  and belongdeptcode like '{0}%'", arg);
            }
            else
            {
                arg = user.DeptCode;
                sqlWhere += string.Format("  and  belongdeptcode in(select encode from base_department  where encode like '{0}%'  or senddeptid='{1}')", arg, user.DeptId);
            }
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
and belongdistrictid='{2}'", user.Account, recid, item[1].ToString());
                    arr.riskdescarray = this.BaseRepository().FindTable(sqlWhere2);
                }
                listarr.Add(arr);
            }
            return listarr;
        }

        public DataTable selectCheckContent(string risknameid)
        {
            string sqlWhere = string.Format("select checkcontent,ID as checkcontentid from Bis_Saftycheckdatadetailed where id='{0}'", risknameid);
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
        public object GetCheckStatistics(ERCHTMS.Code.Operator user)
        {
            string roleNames = user.RoleName;
            var arg = "";
            string sql = string.Format(@"select checkdatatype ,nvl(sum(num),0),count(1) from bis_saftycheckdatarecord a 
left join (
select safetycheckobjectid,count(1) num from bis_htbaseinfo where safetycheckobjectid is not null group by safetycheckobjectid)b
on a.id=b.safetycheckobjectid  where 1=1 and to_char(CreateDate,'yyyy')='{0}'", DateTime.Now.Year);
            if (user.RoleName.Contains("厂级") || user.RoleName.Contains("公司"))
            {
                arg = user.DeptCode.Substring(0, 3);
                sql += string.Format("  and belongdept like '{0}%'", arg);
            }
            else
            {
                arg = user.DeptCode;
                sql += string.Format("  and belongdept in(select encode from base_department  where encode like '{0}%')", arg, user.DeptId);
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
        #endregion

    }
}
