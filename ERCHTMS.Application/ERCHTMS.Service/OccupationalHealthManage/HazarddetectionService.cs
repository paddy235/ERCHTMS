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

namespace ERCHTMS.Service.OccupationalHealthManage
{
    /// <summary>
    /// 描 述：职业病危害因素监测
    /// </summary>
    public class HazarddetectionService : RepositoryFactory<HazarddetectionEntity>, HazarddetectionIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<HazarddetectionEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public HazarddetectionEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
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
            if (!queryParam["riskid"].IsEmpty())//危害因素
            {
                string riskid = queryParam["riskid"].ToString();
                pagination.conditionJson += string.Format(" and RISKID  ='{0}'", riskid.Trim());
                pagination.sidx = "NLSSORT(SUBSTR(AREAVALUE,0,1),'NLS_SORT=SCHINESE_PINYIN_M') asc,ENDTIME";//筛选危害因素后 先按照区域首字母排序 在按照时间排序
            }
            if (!queryParam["areaid"].IsEmpty())//区域
            {
                string areaid = queryParam["areaid"].ToString();
                pagination.conditionJson += string.Format(" and AREAID  ='{0}'", areaid.Trim());
                pagination.sidx = "NLSSORT(SUBSTR(RISKVALUE,0,1),'NLS_SORT=SCHINESE_PINYIN_M') asc,ENDTIME";//筛选危害因素后 先按照区域首字母排序 在按照时间排序
            }
            if (!queryParam["starttime"].IsEmpty() && !queryParam["endtime"].IsEmpty())//时间范围
            {
                string starttime = queryParam["starttime"].ToString();
                string endtime = queryParam["endtime"].ToString();
                pagination.conditionJson += string.Format(" and STARTTIME  <= TO_DATE('{0}','yyyy-mm-dd hh24:mi:ss') and ENDTIME  >= TO_DATE('{1}','yyyy-mm-dd hh24:mi:ss')", endtime.Trim(), starttime.Trim());
            }
            else if (!queryParam["starttime"].IsEmpty()) //只有开始时间
            {
                string starttime = queryParam["starttime"].ToString();
                pagination.conditionJson += string.Format(" and STARTTIME  >= TO_DATE('{0}','yyyy-mm-dd hh24:mi:ss') ", starttime.Trim());
            }
            else if (!queryParam["endtime"].IsEmpty())//只有结束时间
            {
                string endtime = queryParam["endtime"].ToString();
                pagination.conditionJson += string.Format("  and ENDTIME  <= TO_DATE('{0}','yyyy-mm-dd hh24:mi:ss')", endtime.Trim());
            }
            if (!queryParam["isexcessive"].IsEmpty())//是否超标
            {
                string isexcessive = queryParam["isexcessive"].ToString();
                pagination.conditionJson += string.Format(" and ISEXCESSIVE  ={0}", isexcessive.Trim());
            }
            if (!queryParam["detectionuserid"].IsEmpty())//检测人id
            {
                string detectionuserid = queryParam["detectionuserid"].ToString();
                pagination.conditionJson += string.Format(" and DETECTIONUSERID  ='{0}'", detectionuserid.Trim());
            }
            if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            {
                string con = queryParam["condition"].ToString();
                if (con == "month")
                {
                    string month = Convert.ToDateTime(queryParam["keyword"]).ToString("yyyy-MM");
                    pagination.conditionJson += string.Format(" and TO_CHAR(ENDTIME,'yyyy-mm')  = '{0}' and ISEXCESSIVE=1", month.Trim());
                }

            }



            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }


        /// <summary>
        /// 获取全部列表数据
        /// </summary>
        /// <param name="riskid">危害因素</param>
        /// <param name="areaid">区域</param>
        /// <param name="starttime">时间范围</param>
        /// <param name="endtime">时间范围</param>
        /// <param name="isexcessive">是否超标</param>
        /// <param name="detectionuserid">检测人id</param>
        /// <returns></returns>
        public DataTable GetDataTable(string queryJson, string where)
        {
            string order = " order by ENDTIME desc";
            DatabaseType dataType = DbHelper.DbType;
            string sql = "SELECT HID,AREAVALUE,RISKVALUE,LOCATION,STARTTIME,to_char(ENDTIME,'yyyy-mm-dd hh24:mi:ss') as ENDTIME,STANDARD,DETECTIONUSERNAME,ISEXCESSIVE FROM BIS_HAZARDDETECTION WHERE 1=1";

            var queryParam = queryJson.ToJObject();
            string riskid = "";
            string areaid = "";
            string starttime = "";
            string endtime = "";
            string isexcessive = "";
            string detectionuserid = "";
            //查询条件
            if (!queryParam["riskid"].IsEmpty())//危害因素
            {
                riskid = queryParam["riskid"].ToString();

            }
            if (!queryParam["areaid"].IsEmpty())//区域
            {
                areaid = queryParam["areaid"].ToString();

            }
            if (!queryParam["starttime"].IsEmpty())//时间范围
            {
                starttime = queryParam["starttime"].ToString();
            }
            if (!queryParam["endtime"].IsEmpty()) 
            {
                endtime = queryParam["endtime"].ToString();
            }
           
            if (!queryParam["isexcessive"].IsEmpty())//是否超标
            {
                isexcessive = queryParam["isexcessive"].ToString();

            }
            if (!queryParam["detectionuserid"].IsEmpty())//检测人id
            {
                detectionuserid = queryParam["detectionuserid"].ToString();
            }

            //查询条件
            if (!riskid.IsEmpty())//危害因素
            {
                sql += string.Format(" and RISKID  ='{0}'", riskid.Trim());
                order = " order by NLSSORT(SUBSTR(AREAVALUE,0,1),'NLS_SORT=SCHINESE_PINYIN_M') asc,ENDTIME desc";
            }
            if (!areaid.IsEmpty())//区域
            {
                sql += string.Format(" and AREAID  ='{0}'", areaid.Trim());
                order = " order by NLSSORT(SUBSTR(RISKVALUE,0,1),'NLS_SORT=SCHINESE_PINYIN_M') asc,ENDTIME desc";
            }
            if (!starttime.IsEmpty() && !endtime.IsEmpty())//时间范围
            {
                sql += string.Format(" and STARTTIME  <= TO_DATE('{0}','yyyy-mm-dd hh24:mi:ss')  and ENDTIME  >= TO_DATE('{1}','yyyy-mm-dd hh24:mi:ss')", endtime.Trim(), starttime.Trim());
            }
            else if (!starttime.IsEmpty()) 
            {
                sql += string.Format(" and STARTTIME  >= TO_DATE('{0}','yyyy-mm-dd hh24:mi:ss') ", starttime.Trim());
            }
            else if (!endtime.IsEmpty()) 
            {
                sql += string.Format("  and ENDTIME  <= TO_DATE('{0}','yyyy-mm-dd hh24:mi:ss')", endtime.Trim());
            }
            if (!isexcessive.IsEmpty())//是否超标
            {
                sql += string.Format(" and ISEXCESSIVE  ={0}", isexcessive.Trim());
            }
            if (!detectionuserid.IsEmpty())//检测人id
            {
                sql += string.Format(" and DETECTIONUSERID  ='{0}'", detectionuserid.Trim());
            }

            sql += where;

            sql += order;



            return this.BaseRepository().FindTable(sql);
        }

        /// <summary>
        /// 获取测量指标及标准
        /// </summary>
        /// <param name="RiskId">职业病id</param>
        /// <returns></returns>
        public string GetStandard(string RiskId, string where)
        {
            string Sql = "SELECT STANDARD FROM BIS_HAZARDDETECTION WHERE";

            Sql += string.Format(" RISKID = '{0}'", RiskId);

            Sql += where;

            Sql += string.Format(" order by CREATEDATE desc");

            object obj = this.BaseRepository().FindObject(Sql);
            if (obj != null)
            {
                return obj.ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 获取危害因素监测统计数据
        /// </summary>
        /// <param name="year">哪一年数据</param>
        /// <param name="risk">职业病种类</param>
        /// <param name="type">true查询全部 false查询超标数据</param>
        /// <returns></returns>
        public DataTable GetStatisticsHazardTable(int year, string risk, bool type, string where)
        {
            string Sql = @"select to_char(HAZ.ENDTIME,'mm') as time,count(1) from BIS_HAZARDDETECTION HAZ where 
                            1=1";

            if (year != null)
            {
                Sql += string.Format(" AND to_char(HAZ.ENDTIME,'yyyy')='{0}'", year);
            }
            if (!type)//是否查询全部
            {
                Sql += string.Format("  AND HAZ.ISEXCESSIVE=1");
            }
            if (risk != null && risk != "")
            {
                Sql += string.Format("  AND HAZ.RISKID={0}", risk);
            }
            Sql += where;
            Sql += string.Format("  group by to_char(HAZ.ENDTIME,'mm') order by to_char(HAZ.ENDTIME,'mm') asc");
            return this.BaseRepository().FindTable(Sql);
        }


        #endregion

        #region 提交数据
        /// <summary>
        /// 根据id数组批量删除
        /// </summary>
        /// <param name="Ids"></param>
        public void Remove(string Ids)
        {
            string[] id = Ids.Split(',');
            this.BaseRepository().Delete(it => id.Contains(it.HId));
        }

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
        public void SaveForm(string keyValue, HazarddetectionEntity entity)
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

        #region 手机端
        /// <summary>
        /// 新增危险因素监测数据
        /// </summary>
        /// <param name="assess">实体</param>
        /// <param name="user">当前用户</param>
        /// <returns></returns>
        public int SaveHazard(HazarddetectionEntity hazard, ERCHTMS.Code.Operator user)
        {
            string sql = "";
            if (string.IsNullOrEmpty(hazard.HId))
            {
                sql = string.Format(@"insert into BIS_HAZARDDETECTION(
                HID,AREAID,AREAVALUE,RISKID,RISKVALUE,LOCATION,STARTTIME,ENDTIME,STANDARD,DETECTIONUSERID,DETECTIONUSERNAME,
                ISEXCESSIVE,CREATEUSERID,CREATEDATE,CREATEUSERDEPTCODE,CREATEUSERORGCODE) values(
                '{0}','{1}','{2}','{3}','{4}','{5}',to_timestamp('{6}','yyyy-mm-dd hh24:mi:ss'),to_timestamp('{7}','yyyy-mm-dd hh24:mi:ss'),'{8}','{9}','{10}',{11},'{12}',to_timestamp('{13}','yyyy-mm-dd hh24:mi:ss'),'{14}','{15}')", Guid.NewGuid().ToString(), hazard.AreaId, hazard.AreaValue, hazard.RiskId, hazard.RiskValue, hazard.Location, hazard.StartTime, hazard.EndTime, hazard.Standard, hazard.DetectionUserId, hazard.DetectionUserName, hazard.IsExcessive, user.UserId, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), user.DeptCode, user.OrganizeCode);
            }
            else
            {
                sql = string.Format(@"update BIS_HAZARDDETECTION set 
                AREAID='{1}',AREAVALUE='{2}',RISKID='{3}',RISKVALUE='{4}',LOCATION='{5}',
                STARTTIME=to_timestamp('{6}','yyyy-mm-dd hh24:mi:ss'),to_timestamp('{7}','yyyy-mm-dd hh24:mi:ss'),STANDARD='{8}',DETECTIONUSERID='{9}',DETECTIONUSERNAME='{10}',
                ISEXCESSIVE='{11}',MODIFYUSERID={12},MODIFYDATE=to_timestamp('{13}','yyyy-mm-dd hh24:mi:ss') where HID='{0}'", hazard.HId, hazard.AreaId, hazard.AreaValue, hazard.RiskId, hazard.RiskValue, hazard.Location, hazard.StartTime, hazard.EndTime, hazard.Standard, hazard.DetectionUserId, hazard.DetectionUserName, hazard.IsExcessive, user.UserId, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            }

            return this.BaseRepository().ExecuteBySql(sql);
        }


        /// <summary>
        /// 获取全部列表数据
        /// </summary>
        /// <param name="riskid">危害因素</param>
        /// <param name="areaid">区域</param>
        /// <param name="starttime">时间范围</param>
        /// <param name="endtime">时间范围</param>
        /// <param name="isexcessive">是否超标</param>
        /// <param name="detectionuserid">检测人id</param>
        /// <returns></returns>
        public DataTable GetDataTable(string riskid, string areaid, string starttime, string endtime, string isexcessive, string detectionuserid, string where)
        {
            string order = " order by ENDTIME desc";
            DatabaseType dataType = DbHelper.DbType;
            string sql = "SELECT HID,AREAVALUE,RISKVALUE,LOCATION,to_char(STARTTIME,'yyyy-mm-dd') as STARTTIME,to_char(ENDTIME,'yyyy-mm-dd') as ENDTIME,STANDARD,DETECTIONUSERNAME,ISEXCESSIVE FROM BIS_HAZARDDETECTION WHERE 1=1";

            //查询条件
            if (!riskid.IsEmpty())//危害因素
            {
                sql += string.Format(" and RISKID  ='{0}'", riskid.Trim());
                order = " order by NLSSORT(SUBSTR(AREAVALUE,0,1),'NLS_SORT=SCHINESE_PINYIN_M') asc,ENDTIME desc";
            }
            if (!areaid.IsEmpty())//区域
            {
                sql += string.Format(" and AREAID  ='{0}'", areaid.Trim());
                order = " order by NLSSORT(SUBSTR(RISKVALUE,0,1),'NLS_SORT=SCHINESE_PINYIN_M') asc,ENDTIME desc";
            }
            if (!starttime.IsEmpty() && !endtime.IsEmpty())//时间范围
            {
                sql += string.Format(" and STARTTIME  <= TO_DATE('{0}','yyyy-mm-dd hh24:mi:ss')  and ENDTIME  >= TO_DATE('{1}','yyyy-mm-dd hh24:mi:ss')", endtime.Trim(), starttime.Trim());
            }
            else if (!starttime.IsEmpty())
            {
                sql += string.Format(" and STARTTIME  >= TO_DATE('{0}','yyyy-mm-dd hh24:mi:ss') ", starttime.Trim());
            }
            else if (!endtime.IsEmpty())
            {
                sql += string.Format("  and ENDTIME  <= TO_DATE('{0}','yyyy-mm-dd hh24:mi:ss')", endtime.Trim());
            }

            if (!isexcessive.IsEmpty())//是否超标
            {
                sql += string.Format(" and ISEXCESSIVE  ={0}", isexcessive.Trim());
            }
            if (!detectionuserid.IsEmpty())//检测人id
            {
                sql += string.Format(" and DETECTIONUSERID  ='{0}'", detectionuserid.Trim());
            }

            sql += where;

            sql += order;

            return this.BaseRepository().FindTable(sql);
        }
        #endregion
    }
}
