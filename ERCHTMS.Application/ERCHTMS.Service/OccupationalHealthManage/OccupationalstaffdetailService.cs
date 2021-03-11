using ERCHTMS.Entity.OccupationalHealthManage;
using ERCHTMS.IService.OccupationalHealthManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System;
using ERCHTMS.Code;


namespace ERCHTMS.Service.OccupationalHealthManage
{
    /// <summary>
    /// 描 述：职业病人详情表
    /// </summary>
    public class OccupationalstaffdetailService : RepositoryFactory<OccupationalstaffdetailEntity>, OccupationalstaffdetailIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<OccupationalstaffdetailEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }

        /// <summary>
        /// 根据关联id批量修改体检时间
        /// </summary>
        /// <param name="time">体检时间</param>
        /// <param name="parenid">关联id</param>
        /// <returns></returns>
        public int UpdateTime(DateTime time, string parenid)
        {
            //return this.BaseRepository().ExecuteBySql(DbParameters.FormatSql("update BIS_OCCUPATIONALSTAFFDETAIL set INSPECTIONTIME=:INSPECTIONTIME where OCCID=:OCCID"), new DbParameter[] { DbParameters.CreateDbParameter(":INSPECTIONTIME", time), DbParameters.CreateDbParameter(":OCCID", parenid) });
            return this.BaseRepository().ExecuteBySql(string.Format("UPDATE BIS_OCCUPATIONALSTAFFDETAIL SET INSPECTIONTIME=to_timestamp('{0}','yyyy-mm-dd hh24:mi:ss') where OCCID='{1}'", time, parenid));
        }

        /// <summary>
        /// 根据条件查询所有数据
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetTable(string queryJson, string where)
        {
            string Sql = "SELECT row_number() over(ORDER BY INSPECTIONTIME DESC) as IDNUM,OCCDETAILID,USERNAME,USERNAMEPINYIN,TO_CHAR(INSPECTIONTIME,'yyyy-mm-dd hh24:mi:ss')as INSPECTIONTIME,ISSICK,SICKTYPENAME as SICKTYPE,CASE WHEN ISSICK =1 THEN '是' ELSE '否' END AS ISENDEMIC,CASE WHEN ISSICK =2 THEN '是' ELSE '否' END AS ISUNUSUAL,UNUSUALNOTE  FROM BIS_OCCUPATIONALSTAFFDETAIL WHERE 1=1";
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //查询条件
            if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            {
                string condition = queryParam["condition"].ToString();
                string keyord = queryParam["keyword"].ToString();

                //父ID
                Sql += string.Format(" and OCCID  like '%{0}%'", condition.Trim());
                if (keyord.ToInt() == 1) //查询职业病
                {
                    //Type
                    Sql += string.Format(" and ISSICK like {0}", keyord.Trim());
                }
                else if (keyord.ToInt() == 2)  //查询异常人员
                {
                    Sql += string.Format(" and ISSICK like {0}", keyord.Trim());
                }
                else if (keyord.ToInt() == 4)  //查询不是健康人员
                {
                    Sql += string.Format(" and ISSICK != 4");
                }
                //if (keyord.ToInt() < 2)//等于2时查询全部
                //{
                //    //Type
                //    Sql += string.Format(" and ISSICK like {0}", keyord.Trim());
                //}

            }
            Sql += where;

            Sql += " ORDER BY INSPECTIONTIME DESC";
            return this.BaseRepository().FindTable(Sql);
        }

        /// <summary>
        /// 获取用户id下的所有体检记录
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetUserTable(string userid)
        {
            string Sql = "SELECT USERNAME,INSPECTIONTIME,case when ISSICK=0 then '否' else '是' end issick,ITEMNAME,UNUSUALNOTE,MECHANISMNAME FROM V_USERSTAFF WHERE 1=1";

            if (userid.Trim() != "")
            {
                Sql += string.Format(" and USERID  = '{0}'", userid.Trim());
            }
            //Sql += where;
            return this.BaseRepository().FindTable(Sql);
        }

        /// <summary>
        /// 获取用户id下的所有体检记录 新
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable NewGetUserTable(string userid)
        {
            string sql =
                @"SELECT st.USERNAME,st.INSPECTIONTIME,ITEMNAME,st.mechanismname,staff.occid,isannex,filenum,detail.UNUSUALNOTE,CASE WHEN DETAIL.ISSICK =1 THEN '是' ELSE '否' END AS ISENDEMIC,CASE WHEN DETAIL.ISSICK =2 THEN '是' ELSE '否' END AS ISUNUSUAL FROM V_USERSTAFF  st
            left join bis_occupationalstaffdetail  detail on st.occdetailid=detail.occdetailid
            left join V_OCCUPATIOALSTAFF staff on detail.occid=staff.occid WHERE 1=1 ";
            if (userid.Trim() != "")
            {
                sql += string.Format(" and st.USERID  = '{0}'", userid.Trim());
            }
            //Sql += where;
            return this.BaseRepository().FindTable(sql);
        }

        /// <summary>
        /// 获取用户的接触危害因素
        /// </summary>
        /// <returns></returns>
        public DataTable GetUserHazardfactor(string useraccount)
        {
            string sql = string.Format(
                @"select LISTAGG(riskvalue,',') WITHIN GROUP (ORDER BY riskvalue) AS us from (              
            select userid,username,riskvalue from BIS_HAZARDFACTORUSER hauser
            left join BIS_HAZARDFACTORs ha on ha.hid=hauser.hid
             where 1=1
             group by userid,username,riskvalue) f where userid='{0}'  group by userid,username", useraccount);
            DataTable ret = BaseRepository().FindTable(sql);
            return ret;

        }

        /// <summary>
        /// 获取职业病统计表
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetStatisticsUserTable(string year, string where)
        {
            string Sql = @"select DDE.ITEMNAME,count(DDE.ITEMNAME) as itemsum,OCC.SICKTYPE from BIS_OCCUPATIONALSTAFFDETAIL OCC
                            LEFT JOIN BASE_DATAITEMDETAIL DDE on OCC.Sicktype=DDE.ITEMVALUE
                            LEFT JOIN Base_Dataitem DI ON DI.ITEMID=DDE.Itemid
                            LEFT JOIN BIS_OCCUPATIOALSTAFF OC ON OCC.OCCID=OC.OCCID
                            where DI.ITEMCODE='SICKTYPE' and OC.MECHANISMNAME is not null";

            if (year != "")
            {
                Sql += string.Format(" and to_char(OC.INSPECTIONTIME,'yyyy')='{0}'", year.Trim());
            }
            Sql += " and OCC." + where;
            Sql += string.Format("  group by DDE.ITEMNAME,OCC.SICKTYPE");



            return this.BaseRepository().FindTable(Sql);
        }

        /// <summary>
        /// 获取新的职业病统计表
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable NewGetStatisticsUserTable(string year, string where)
        {
            //先获取所有一级职业病信息
            string Sql = @"select DDE.ITEMNAME,count(DDE.ITEMNAME) as count,DDE.ITEMVALUE from BASE_DATAITEMDETAIL DDE
                            LEFT JOIN Base_Dataitem DI ON DI.ITEMID=DDE.Itemid
                            LEFT JOIN (select SUBSTR(DDE.ITEMVALUE,0,3) ITEMVALUE from BASE_DATAITEMDETAIL DDE
                            LEFT JOIN Base_Dataitem DI ON DI.ITEMID=DDE.Itemid
                            LEFT JOIN BIS_OCCUPATIONALSTAFFDETAIL OCC ON instr(OCC.Sicktype,DDE.ITEMVALUE)>0
                            LEFT JOIN BIS_OCCUPATIOALSTAFF OC ON OCC.OCCID=OC.OCCID
                            where DI.ITEMCODE='SICKTYPE' and length(DDE.ITEMVALUE)>3
                            and OC.MECHANISMNAME is not null
                            ";

            if (year != "")
            {
                Sql += string.Format(" and to_char(OC.INSPECTIONTIME,'yyyy')='{0}'", year.Trim());
            }
            Sql += " and OCC." + where;
            Sql += string.Format(" group by SUBSTR(DDE.ITEMVALUE,0,3),OCC.Userid");//这一句决定了是统计得病人数还是统计得病数量 注释就是统计得病数量
            Sql += string.Format(@" ) DT ON DT.Itemvalue=DDE.Itemvalue
                            where DI.ITEMCODE='SICKTYPE' and length(DDE.ITEMVALUE)=3 and DT.ITEMVALUE is not null");

            Sql += string.Format("  group by DDE.ITEMNAME,DDE.Sortcode,DDE.ITEMVALUE  order by DDE.Sortcode");



            return this.BaseRepository().FindTable(Sql);
        }

        /// <summary>
        /// 获取部门职业病统计表
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetStatisticsDeptTable(string year, string where)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string Sql = @"select Dep.Fullname, Count(Dep.Fullname) as DepSum,Dep.Encode from Base_Department Dep
                            LEFT JOIN Base_User U ON DEP.Encode =U.Departmentcode
                            LEFT JOIN BIS_OCCUPATIONALSTAFFDETAIL OSD ON OSD.Userid=U.Userid
                            LEFT JOIN BIS_OCCUPATIOALSTAFF OC ON OSD.OCCID=OC.OCCID
                            where DEP.OrganizeId='" + user.OrganizeId + "' and OC.MECHANISMNAME is not null and OSD.Issick=1";

            if (year != "")
            {
                Sql += string.Format(" and to_char(OC.INSPECTIONTIME,'yyyy')='{0}'", year.Trim()); 
            }
            Sql += " and OC." + where;
            Sql += string.Format("   group by Dep.Encode,Dep.Fullname");



            return this.BaseRepository().FindTable(Sql);
        }

        /// <summary>
        /// 获取年度职业病统计表
        /// </summary>
        /// <param name="yearType">近几年数据</param>
        /// <param name="Dept">部门EnCode</param>
        /// <returns></returns>
        public DataTable GetStatisticsYearTable(int yearType, string Dept, string where)
        {
            int year = DateTime.Now.Year;
            string Sql = @"select to_char(OC.INSPECTIONTIME,'yyyy') as year ,COUNT(OC.INSPECTIONTIME) from BIS_OCCUPATIONALSTAFFDETAIL OSD
                            LEFT JOIN BIS_OCCUPATIOALSTAFF OC ON OSD.OCCID=OC.OCCID
                            LEFT JOIN Base_User U ON U.Userid=OSD.Userid
                            LEFT JOIN Base_Department Dep  ON DEP.Encode =SUBSTR(U.Departmentcode,0,6)
                            where OC.MECHANISMNAME is not null  and OSD.Issick=1";
            if (yearType != 0)//如果是0则查全部
            {
                Sql += string.Format("  AND OC.INSPECTIONTIME  >= TO_DATE('{0}','yyyy') AND OC.INSPECTIONTIME<=TO_DATE('{1}','yyyy-mm-dd hh24:mi:ss')", (year - yearType), year + "-12-31 23:59:59");
            }
            if (Dept != null && Dept != "")
            {
                Sql += string.Format(" AND DEP.Encode like'{0}%'", Dept);
            }
            Sql += " AND OC." + where;
            Sql += string.Format("  group by to_char(OC.INSPECTIONTIME,'yyyy') order by to_char(OC.INSPECTIONTIME,'yyyy') asc");
            return this.BaseRepository().FindTable(Sql);
        }

        /// <summary>
        /// 获取职业病人员清单(全部的)
        /// </summary>
        /// <param name="queryJson">查询条件</param>
        /// <returns></returns>
        public DataTable GetStaffList(string queryJson, string where)
        {
            string Sql = "SELECT OCCDETAILID,USERID,USERNAME,GENDER,DEPTNAME,DUTYNAME,ISSICK,SICKTYPE,SICKTYPENAME FROM V_STAFFDETAILLIST WHERE ISSICK=1";
            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();
            //查询条件
            if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            {
                string condition = queryParam["condition"].ToString();
                string keyord = queryParam["keyword"].ToString();
                switch (condition)
                {
                    case "UserName":          //姓名
                        Sql += string.Format(" and USERNAME  like '%{0}%'", keyord);
                        break;
                    default:
                        break;
                }
            }

            //查询相当于上一年度增加的职业病人 根据下拉年度的上一年度来筛选
            if (!queryParam["type"].IsEmpty())
            {
                string type = queryParam["type"].ToString();
                if (type == "0")
                {   //只查询今年新增数据
                    Sql += string.Format(" and Userid not in(select userid from BIS_OCCUPATIONALSTAFFDETAIL D LEFT JOIN BIS_OCCUPATIOALSTAFF OCC ON D.OCCID=OCC.OCCID where OCC.MECHANISMNAME is not null and to_char(D.INSPECTIONTIME,'yyyy')='{0}' and ISSICK=1)", DateTime.Now.Year - 1);
                }
                else
                {
                    if (!queryParam["time"].IsEmpty())
                    {
                        string time = queryParam["time"].ToString();
                        Sql += string.Format(" and to_char(INSPECTIONTIME,'yyyy') = '{0}'", time);
                    }
                }

            }

            Sql += where;

            Sql += string.Format(" order by INSPECTIONTIME desc ,Usernamepinyin asc ");
            return this.BaseRepository().FindTable(Sql);
        }


        /// <summary>
        /// 获取职业病人员清单人员数量
        /// </summary>
        /// <param name="queryJson">查询条件</param>
        /// <returns></returns>
        public int GetStaffListSum(string queryJson, string where)
        {
            string Sql = "SELECT Count(OCCDETAILID)as count FROM V_STAFFDETAILLIST WHERE ISSICK=1";
            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();
            //查询条件
            if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            {
                string condition = queryParam["condition"].ToString();
                string keyord = queryParam["keyword"].ToString();
                switch (condition)
                {
                    case "UserName":          //姓名
                        Sql += string.Format(" and USERNAME  like '%{0}%'", keyord);
                        break;
                    case "SickType":        //职业病
                        Sql += string.Format(" and SICKTYPE  Like '%{0}%'", keyord);
                        break;
                    case "DeptCode":        //部门编码
                        Sql += string.Format(" and ENCODE  like '{0}%'", keyord);
                        break;
                    default:
                        break;
                }
            }

            //查询相当于上一年度增加的职业病人 根据下拉年度的上一年度来筛选
            if (!queryParam["type"].IsEmpty())
            {
                string type = queryParam["type"].ToString();
                if (type == "0")
                {   //只查询今年新增数据
                    Sql += string.Format(" and Userid not in(select userid from BIS_OCCUPATIONALSTAFFDETAIL D LEFT JOIN BIS_OCCUPATIOALSTAFF OCC ON D.OCCID=OCC.OCCID where OCC.MECHANISMNAME is not null and to_char(D.INSPECTIONTIME,'yyyy')='{0}' and ISSICK=1) and to_char(INSPECTIONTIME,'yyyy')='{1}'", DateTime.Now.Year - 1, DateTime.Now.Year);
                }
                else
                {
                    if (!queryParam["time"].IsEmpty())
                    {
                        string time = queryParam["time"].ToString();
                        Sql += string.Format(" and to_char(INSPECTIONTIME,'yyyy') = '{0}'", time);
                    }
                }

            }
            Sql += where;

            Sql += string.Format(" order by INSPECTIONTIME desc ,Usernamepinyin asc ");

            object obj = this.BaseRepository().FindObject(Sql);

            return obj.ToInt(); ;
        }

        /// <summary>
        /// 获取职业病人员清单
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetStaffListPage(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();
            //查询条件
            if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            {
                string condition = queryParam["condition"].ToString();
                string keyord = queryParam["keyword"].ToString();
                switch (condition)
                {
                    case "Account":            //账户
                        pagination.conditionJson += string.Format(" and ACCOUNT  like '%{0}%'", keyord);
                        break;
                    case "UserName":          //姓名
                        pagination.conditionJson += string.Format(" and USERNAME  like '%{0}%'", keyord);
                        break;
                    case "Mobile":          //手机
                        pagination.conditionJson += string.Format(" and MOBILE like '%{0}%'", keyord);
                        break;
                    case "SickType":        //职业病
                        pagination.conditionJson += string.Format(" and SICKTYPE  Like '%{0}%'", keyord);
                        break;
                    case "DeptCode":        //部门编码
                        pagination.conditionJson += string.Format(" and ENCODE  like '{0}%'", keyord);
                        break;
                    default:
                        break;
                }
            }

            //查询相当于上一年度增加的职业病人 根据下拉年度的上一年度来筛选
            if (!queryParam["type"].IsEmpty())
            {
                string type = queryParam["type"].ToString();
                if (type == "0")
                {
                    //只查询今年新增数据
                    pagination.conditionJson += string.Format(" and Userid not in(select userid from BIS_OCCUPATIONALSTAFFDETAIL D LEFT JOIN BIS_OCCUPATIOALSTAFF OCC ON D.OCCID=OCC.OCCID where OCC.MECHANISMNAME is not null and to_char(D.INSPECTIONTIME,'yyyy')='{0}' and ISSICK=1) and to_char(INSPECTIONTIME,'yyyy')='{1}'", DateTime.Now.Year - 1, DateTime.Now.Year);
                }
                else
                {
                    if (!queryParam["time"].IsEmpty())
                    {
                        string time = queryParam["time"].ToString();
                        pagination.conditionJson += string.Format(" and to_char(INSPECTIONTIME,'yyyy') = '{0}'", time);
                    }
                }

            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }


        /// <summary>
        /// 根据条件删除健康用户 重新添加
        /// </summary>
        /// <param name="SickType">是否有职业病</param>
        /// <param name="parenid">关联id</param>
        /// <returns></returns>
        public int Delete(string parenid, int SickType)
        {
            return this.BaseRepository().ExecuteBySql(string.Format("DELETE BIS_OCCUPATIONALSTAFFDETAIL where OCCID='{0}' and ISSICK={1}", parenid, SickType));
        }


        /// <summary>
        /// 根据父id和是否生病查询表中信息
        /// </summary>
        /// <param name="Pid">父id</param>
        /// <param name="SickType">是否生病</param>
        /// <returns></returns>
        public IEnumerable<OccupationalstaffdetailEntity> GetList(string Pid, int Issick)
        {
            //等于1跟2查询有职业病跟异常的人员
            if (Issick ==1 || Issick==2)
            {
                return this.BaseRepository().IQueryable(it => it.OccId == Pid && it.Issick == Issick).ToList();
            }
            else if (Issick ==4)
            {
                return this.BaseRepository().IQueryable(it => it.OccId == Pid && (it.Issick == 1 || it.Issick == 2));
            }
            else
            {
                return this.BaseRepository().IQueryable(it => it.OccId == Pid).ToList();
            }
        }


        /// <summary>
        /// 存储过程分页
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageListByProc(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //查询条件
            if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            {
                string condition = queryParam["condition"].ToString();
                string keyord = queryParam["keyword"].ToString();

                //父ID
                pagination.conditionJson += string.Format(" and OCCID  like '%{0}%'", condition.Trim());
                if (keyord.ToInt() == 1) //查询职业病
                {
                    //Type
                    pagination.conditionJson += string.Format(" and ISSICK like {0}", keyord.Trim());
                }
                else if (keyord.ToInt() == 2)  //查询异常人员
                {
                    pagination.conditionJson += string.Format(" and ISSICK like {0}", keyord.Trim());
                }
                else if (keyord.ToInt() ==4)
                {
                    pagination.conditionJson += string.Format(" and ISSICK != 4");
                }


            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public OccupationalstaffdetailEntity GetEntity(string keyValue)
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
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, OccupationalstaffdetailEntity entity)
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
    }
}
