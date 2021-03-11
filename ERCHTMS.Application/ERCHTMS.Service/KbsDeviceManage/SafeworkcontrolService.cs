using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.IService.KbsDeviceManage;

namespace ERCHTMS.Service.KbsDeviceManage
{
    /// <summary>
    /// 描 述：作业现场安全管控
    /// </summary>
    public class SafeworkcontrolService : RepositoryFactory<SafeworkcontrolEntity>, SafeworkcontrolIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SafeworkcontrolEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }

        /// <summary>
        /// 获取根据状态获取现场作业信息
        /// </summary>
        /// <param name="State">1开始 2结束</param>
        /// <returns></returns>
        public IEnumerable<SafeworkcontrolEntity> GetStartList(int State)
        {
            return this.BaseRepository().IQueryable().Where(a => a.State == State).ToList();
        }

        /// <summary>
        /// 获取到所有当前开始的作业
        /// </summary>
        /// <returns></returns>
        public List<SafeworkcontrolEntity> GetNowWork()
        {
            return this.BaseRepository().IQueryable(it =>
                ((it.Actualstarttime <= DateTime.Now && it.ActualEndTime >= DateTime.Now) || (it.Actualstarttime <= DateTime.Now && it.ActualEndTime == null)) && it.State == 1).ToList();
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SafeworkcontrolEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 分页数据列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            var curuser = OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;


            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }


        /// <summary>
        /// 获取今日高风险作业
        /// </summary>
        /// <returns></returns>    
        public List<SafeworkcontrolEntity> GetDangerWorkToday(string level = "一级风险")
        {
            string sql = string.Format(@"SELECT ID,TASKNAME,ACTUALSTARTTIME, NVL(ACTUALENDTIME, PLANENDTIME) PLANENDTIME , DANGERLEVEL,TASKMANAGENAME, DEPTNAME,ISSUENAME, TASKREGIONNAME,PERMITNAME,GUARDIANNAME,EXECUTIVENAMES,SUPERVISIONNAMES, '0' as Remark FROM bis_safeworkcontrol
                            WHERE SYSDATE<ACTUALSTARTTIME AND TO_CHAR(SYSDATE,'yyyy-MM-dd')=TO_CHAR(ACTUALSTARTTIME,'yyyy-MM-dd') AND INSTR('{0}',DANGERLEVEL)>0
                            UNION
                            SELECT ID,TASKNAME,ACTUALSTARTTIME, NVL(ACTUALENDTIME, PLANENDTIME) PLANENDTIME , DANGERLEVEL,TASKMANAGENAME, DEPTNAME,ISSUENAME, TASKREGIONNAME,PERMITNAME,GUARDIANNAME,EXECUTIVENAMES,SUPERVISIONNAMES, '1' as Remark FROM bis_safeworkcontrol
                            WHERE SYSDATE>ACTUALSTARTTIME AND ACTUALENDTIME IS NULL  AND INSTR('{0}',DANGERLEVEL)>0
                            UNION
                            SELECT ID,TASKNAME,ACTUALSTARTTIME, NVL(ACTUALENDTIME, PLANENDTIME) PLANENDTIME , DANGERLEVEL,TASKMANAGENAME, DEPTNAME,ISSUENAME, TASKREGIONNAME,PERMITNAME,GUARDIANNAME,EXECUTIVENAMES,SUPERVISIONNAMES, '2' as Remark FROM bis_safeworkcontrol
                            WHERE  ACTUALENDTIME IS NOT NULL AND TO_CHAR(SYSDATE,'yyyy-MM-dd')=TO_CHAR(ACTUALENDTIME,'yyyy-MM-dd')  AND INSTR('{0}',DANGERLEVEL)>0", level);
            List<SafeworkcontrolEntity> result = this.BaseRepository().FindList(sql).OrderBy(x => x.Actualstarttime).ToList();
            foreach (var item in result)
            {
                if (item.Remark == "1")
                    item.MonitorUsers = GetSafeWorkUser(item.ID);
            }
            return result;
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
        public void SaveForm(string keyValue, SafeworkcontrolEntity entity)
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

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void AppSaveForm(string keyValue, SafeworkcontrolEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                this.BaseRepository().Update(entity);
            }
            else
            {
                this.BaseRepository().Insert(entity);
                SaveSafeworkUserInfoForm("", entity);
            }
        }

        #endregion


        #region 业务子表预警信息
        /// <summary>
        /// 查询作业的相关监护人员
        /// </summary>
        /// <param name="workId"></param>
        /// <returns></returns>

        public List<SafeworkUserEntity> GetSafeWorkUser(string workId)
        {
            Repository<SafeworkUserEntity> dbFactory = new Repository<SafeworkUserEntity>(DbFactory.Base());
            string sql = string.Format(@"SELECT ID,USERID, USERNAME,TYPE,STATE FROM bis_safeworkUser where WORKID='{0}'", workId);
            return dbFactory.FindList(sql).ToList();
        }



        /// <summary>
        /// 监管人信息
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="uname"></param>
        /// <param name="workid"></param>
        /// <returns></returns>
        public List<SafeworkUserEntity> GetSafeuserinfolist(string uid, string uname, string workid, int type)
        {
            List<SafeworkUserEntity> list = new List<SafeworkUserEntity>();
            if (!string.IsNullOrEmpty(uid))
            {
                var ids = uid.Split(',');
                var names = uname.Split(',');
                for (int i = 0; i < ids.Length; i++)
                {
                    SafeworkUserEntity uentity = new SafeworkUserEntity();
                    uentity.Create();
                    uentity.userid = ids[i];
                    uentity.username = names[i];
                    uentity.workid = workid;
                    uentity.type = type;
                    uentity.state = 0;
                    list.Add(uentity);
                }
            }
            return list;
        }

        /// <summary>
        /// 保存监护人（新增）
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void SaveSafeworkUserInfoForm(string keyValue, SafeworkcontrolEntity entity)
        {
            //开始事物
            var res = DbFactory.Base().BeginTrans();
            try
            {
                List<SafeworkUserEntity> list = new List<SafeworkUserEntity>();
                if (!string.IsNullOrEmpty(entity.Taskmanagename))
                {
                    SafeworkUserEntity uentity = new SafeworkUserEntity();
                    uentity.Create();
                    uentity.userid = entity.Taskmanageid;
                    uentity.username = entity.Taskmanagename;
                    uentity.workid = entity.ID;
                    uentity.type = 3;
                    uentity.state = 0;
                    list.Add(uentity);
                }
                list.AddRange(GetSafeuserinfolist(entity.Guardianid, entity.Guardianname, entity.ID, 0));
                list.AddRange(GetSafeuserinfolist(entity.ExecutiveIds, entity.ExecutiveNames, entity.ID, 1));
                list.AddRange(GetSafeuserinfolist(entity.SupervisionIds, entity.SupervisionNames, entity.ID, 2));
                res.Insert<SafeworkUserEntity>(list);
                res.Commit();
            }
            catch (Exception ex)
            {
                res.Rollback();
            }
        }

        /// <summary>
        /// 更新作业成员是否监管区域内状态
        /// </summary>
        public void SaveSafeworkUserStateIofo(string workid, string userid, int state)
        {
            //开始事物
            var res = DbFactory.Base().BeginTrans();
            try
            {
                if (!string.IsNullOrEmpty(workid))
                {
                    Repository<SafeworkUserEntity> caruserdb = new Repository<SafeworkUserEntity>(DbFactory.Base());
                    SafeworkUserEntity entity = caruserdb.IQueryable(it => it.workid == workid && it.userid == userid).FirstOrDefault();
                    if (entity != null)
                    {
                        entity.state = state;
                        entity.MODIFYDATE = DateTime.Now;
                        res.Update(entity);
                        res.Commit();
                    }
                }
            }
            catch (Exception)
            {
                res.Rollback();
            }
        }



        /// <summary>
        /// 获取预警信息实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public WarningInfoEntity GetWarningInfoEntity(string keyValue)
        {
            Repository<WarningInfoEntity> inlogdb = new Repository<WarningInfoEntity>(DbFactory.Base());
            var entity = inlogdb.FindEntity(keyValue);
            if (entity != null)
            {
                var workitem = this.GetEntity(entity.BaseId);
                if (workitem != null)
                {
                    entity.deptCode = workitem.Workno;//仅用于页面临时显示
                }
                else
                {
                    entity.deptCode = "";
                }
            }
            return entity;
        }
        /// <summary>
        /// 获取预警列表信心
        /// </summary>
        /// <param name="type">0人员预警 1现场工作</param>
        /// <returns></returns>
        public List<WarningInfoEntity> GetWarningInfoList(int type)
        {
            Repository<WarningInfoEntity> inlogdb = new Repository<WarningInfoEntity>(DbFactory.Base());
            string sql = string.Format(@"select id,TaskName,WarningContent,LiableName,LiableId,WarningTime,BaseId,type,deptName,deptName,State,NoticeType from bis_EarlyWarning d where type={0} ", type);
            return inlogdb.FindList(sql).ToList();
        }
        /// <summary>
        /// 获取所有预警信息列表
        /// </summary>
        /// <returns></returns>
        public List<WarningInfoEntity> GetWarningAllList()
        {
            Repository<WarningInfoEntity> inlogdb = new Repository<WarningInfoEntity>(DbFactory.Base());
            string sql = string.Format(@"select id,TaskName,WarningContent,LiableName,LiableId,WarningTime,BaseId,type,deptName,deptName,State,NoticeType,CREATEDATE,MODIFYDATE from bis_EarlyWarning d ");
            return inlogdb.FindList(sql).ToList();
        }
        /// <summary>
        /// 获取人员安全管控各个时段人数
        /// </summary>
        /// <returns></returns>
        public List<KbsEntity> GetDayTimeIntervalUserNum()
        {
            Repository<KbsEntity> inlogdb = new Repository<KbsEntity>(DbFactory.Base());
            string sql = string.Format(@"select onlinehour as Num2,count(1) as num from bis_persononline d where d.onlinedate='{0}' group by d.onlinehour ", DateTime.Now.ToString("yyyy-MM-dd"));
            return inlogdb.FindList(sql).ToList();
        }


        /// <summary>
        /// 保存预警（新增、修改）
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void SaveWarningInfoForm(string keyValue, WarningInfoEntity entity)
        {
            //开始事物
            var res = DbFactory.Base().BeginTrans();
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    res.Update<WarningInfoEntity>(entity);
                    res.Commit();
                }
                else
                {
                    res.Insert<WarningInfoEntity>(entity);
                    res.Commit();
                }
            }
            catch (Exception)
            {
                res.Rollback();
            }
        }

        /// <summary>
        /// 保存预警（新增、修改）
        /// </summary>
        /// <param name="type">0新增 1修改</param>
        /// <param name="list"></param>
        public void SaveWarningInfoForm(int type, IList<WarningInfoEntity> list)
        {
            //开始事物
            var res = DbFactory.Base().BeginTrans();
            try
            {
                if (type == 0)
                {
                    res.Insert<WarningInfoEntity>(list);
                    res.Commit();
                }
                else if (type == 1)
                {
                    res.Update<WarningInfoEntity>(list);
                    res.Commit();
                }
            }
            catch (Exception)
            {
                res.Rollback();
            }
        }

        /// <summary>
        /// 删除预警信息
        /// </summary>
        /// <param name="keyValue"></param>
        public void DelWarningInForm(string keyValue)
        {
            //开始事物
            var res = DbFactory.Base().BeginTrans();
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    Repository<WarningInfoEntity> inlogdb = new Repository<WarningInfoEntity>(DbFactory.Base());
                    WarningInfoEntity entity = inlogdb.FindEntity(keyValue);
                    if (entity != null)
                    {
                        res.Delete<WarningInfoEntity>(entity);
                        res.Commit();
                    }
                }
            }
            catch (Exception)
            {
                res.Rollback();
            }
        }
        /// <summary>
        /// 批量删除预警信息（通过主表记录Id）
        /// </summary>
        /// <param name="BaseId"></param>
        public void DelBatchWarningInForm(string BaseId)
        {
            //开始事物
            var res = DbFactory.Base().BeginTrans();
            try
            {
                if (!string.IsNullOrEmpty(BaseId))
                {
                    Repository<WarningInfoEntity> caruserdb = new Repository<WarningInfoEntity>(DbFactory.Base());
                    List<WarningInfoEntity> list = caruserdb.IQueryable(it => it.BaseId == BaseId).ToList();
                    if (list != null)
                    {
                        res.Delete<WarningInfoEntity>(list);
                        res.Commit();
                    }
                }
            }
            catch (Exception)
            {
                res.Rollback();
            }
        }



        #endregion





        /// <summary>
        /// 获取已结束的预警信息
        /// </summary>
        public List<WarningInfoEntity> GetBatchWarningInfoList(string WorkId)
        {
            //开始事物
            var res = DbFactory.Base().BeginTrans();
            List<WarningInfoEntity> list = new List<WarningInfoEntity>();
            try
            {
                if (!string.IsNullOrEmpty(WorkId))
                {
                    Repository<WarningInfoEntity> caruserdb = new Repository<WarningInfoEntity>(DbFactory.Base());
                    list = caruserdb.IQueryable(it => it.BaseId == WorkId).ToList();
                }
            }
            catch (Exception)
            {
                res.Rollback();
            }
            return list;
        }

        public List<RiskAssessEntity> GetDistrictLevel()
        {
            var db = DbFactory.Base();

            var query = from q1 in db.IQueryable<DistrictEntity>()
                        join q2 in db.IQueryable<SafeworkcontrolEntity>() on q1.DistrictID equals q2.Taskregionid
                        where q2.State == 1
                        group q2.DangerLevel by q1.DistrictCode
                        into g
                        select new { g.Key, Level = g.Min(x => (x == "一级风险" ? 1 : x == "二级风险" ? 2 : x == "三级风险" ? 3 : 4)) };
            return query.ToList().Select(x => new RiskAssessEntity { DistrictId = x.Key, GradeVal = x.Level }).ToList();
        }
    }
}
