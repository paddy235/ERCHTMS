using ERCHTMS.Entity.MatterManage;
using ERCHTMS.IService.MatterManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Data;
using ERCHTMS.Service.SystemManage;

namespace ERCHTMS.Service.MatterManage
{
    /// <summary>
    /// 描 述：计量管理
    /// </summary>
    public class CalculateService : RepositoryFactory<CalculateEntity>, CalculateIService
    {
        private object numberLock = new object();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<CalculateEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public CalculateEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 获取最新称重计量信息
        /// </summary>
        /// <param name="Numbers">车牌号</param>
        /// <returns></returns>
        public CalculateEntity GetNewEntity(string Platenumber)
        {
            CalculateEntity entity = new CalculateEntity();
            if (!string.IsNullOrEmpty(Platenumber))
            {
                entity = this.BaseRepository().IQueryable().ToList().Where(a => a.Platenumber == Platenumber && a.IsOut != 1).OrderByDescending(a => a.CreateDate).FirstOrDefault();
            }
            else
            {
                entity = this.BaseRepository().IQueryable().ToList().Where(a => a.IsDelete == 1).OrderByDescending(a => a.CreateDate).FirstOrDefault();
            }
            return entity;
        }

        /// <summary>
        /// 列表分页  
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();

            if (!queryParam["keyword"].IsEmpty())
            {//车牌号
                string PlateNumber = queryParam["keyword"].ToString().Trim();
                pagination.conditionJson += string.Format(" and PlateNumber like '%{0}%'", PlateNumber);
            }
            if (!queryParam["Takegoodsname"].IsEmpty() && queryParam["Takegoodsname"].ToString().Trim() != "全部")
            {//提货方
                string Transporttype = queryParam["Takegoodsname"].ToString().Trim();
                pagination.conditionJson += string.Format(" and Takegoodsname='{0}'", Transporttype);
            }
            if (!queryParam["Transporttype"].IsEmpty() && queryParam["Transporttype"].ToString().Trim() != "全部")
            {//运输类型
                string Transporttype = queryParam["Transporttype"].ToString().Trim();
                pagination.conditionJson += string.Format(" and Transporttype like '%{0}%'", Transporttype);
            }
            if (!queryParam["Goodsname"].IsEmpty() && queryParam["Goodsname"].ToString().Trim() != "全部")
            {//货物名称
                string Goodsname = queryParam["Goodsname"].ToString().Trim();
                pagination.conditionJson += string.Format(" and Goodsname like '%{0}%'", Goodsname);
            }
            if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
            {//打印时间起
                string stime = queryParam["StartTime"].ToString().Trim();
                pagination.conditionJson += string.Format(" and roughtime >  to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') ", Convert.ToDateTime(stime));
                //打印时间至
                string etime = queryParam["EndTime"].ToString().Trim();
                pagination.conditionJson += string.Format(" and roughtime <= to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') ", Convert.ToDateTime(etime));
            }
            if (!queryParam["UserName"].IsEmpty())
            {//司磅员
                string username = queryParam["UserName"].ToString().Trim();
                pagination.conditionJson += string.Format(" and ( Roughusername like '%{0}%' or Tareusername like '%{0}%' )", username);
            }
            if (!queryParam["CarType"].IsEmpty())
            {//车辆类型
                string CarType = queryParam["CarType"].ToString().Trim();
                pagination.conditionJson += string.Format(" and datatype='{0}'", CarType);
            }

            DataTable dt = this.BaseRepository().FindTableByProcPager(pagination, DbHelper.DbType);
            return dt;
        }

        /// <summary>
        /// 列表分页  
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetNewPageList(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();

            if (!queryParam["keyword"].IsEmpty())
            {//车牌号
                string PlateNumber = queryParam["keyword"].ToString().Trim();
                pagination.conditionJson += string.Format(" and PlateNumber like '%{0}%'", PlateNumber);
            }
            if (!queryParam["Takegoodsname"].IsEmpty() && queryParam["Takegoodsname"].ToString().Trim() != "全部")
            {//提货方
                string Transporttype = queryParam["Takegoodsname"].ToString().Trim();
                pagination.conditionJson += string.Format(" and Takegoodsname='{0}'", Transporttype);
            }
            if (!queryParam["Transporttype"].IsEmpty() && queryParam["Transporttype"].ToString().Trim() != "全部")
            {//运输类型
                string Transporttype = queryParam["Transporttype"].ToString().Trim();
                pagination.conditionJson += string.Format(" and Transporttype like '%{0}%'", Transporttype);
            }
            if (!queryParam["Goodsname"].IsEmpty() && queryParam["Goodsname"].ToString().Trim() != "全部")
            {//货物名称
                string Goodsname = queryParam["Goodsname"].ToString().Trim();
                pagination.conditionJson += string.Format(" and Goodsname like '%{0}%'", Goodsname);
            }
            if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
            {//毛重时间起
                string StartTime = queryParam["StartTime"].ToString().Trim();
                pagination.conditionJson += string.Format(" and (ROUGHTIME is NULL OR ROUGHTIME>=to_date('{0}','yyyy-MM-dd hh24:mi:ss')) ", StartTime);
                //毛重时间至
                string EndTime = queryParam["EndTime"].ToString().Trim();
                DateTime dst = Convert.ToDateTime(EndTime).AddDays(1);
                pagination.conditionJson += string.Format(" and (ROUGHTIME is NULL OR ROUGHTIME<=to_date('{0}','yyyy-MM-dd hh24:mi:ss')) ", dst);
            }
            else
            {//当天记录
                pagination.conditionJson += string.Format(" and (ROUGHTIME is NULL OR to_char(ROUGHTIME,'yyyy-MM-dd')='{0}') ", DateTime.Now.ToString("yyyy-MM-dd"));
            }
            if (!queryParam["UserName"].IsEmpty())
            {//司磅员
                string username = queryParam["UserName"].ToString().Trim();
                pagination.conditionJson += string.Format(" and ( Roughusername like '%{0}%' or Tareusername like '%{0}%' )", username);
            }
            //DataTable dt = this.BaseRepository().FindTableByProcPager(pagination, DbHelper.DbType);
            string sql = string.Format("select  {0} from {1} where {2}", pagination.p_fields, pagination.p_tablename, pagination.conditionJson += "  order by ROUGHTIME desc");
            DataTable dt = this.BaseRepository().FindTable(sql);
            return dt;
        }

        /// <summary>
        /// 获取计量统计列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetCountPageList(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();

            DataTable dt = this.BaseRepository().FindTableByProcPager(pagination, DbHelper.DbType);

            //  this.BaseRepository().ExecuteByProc("p_test");


            return dt;
        }

        /// <summary>
        /// 获取地磅员列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageUserList(Pagination pagination, string queryJson, string res)
        {
            var queryParam = queryJson.ToJObject();

            if (!queryParam["keyword"].IsEmpty())
            {//姓名
                string realname = queryParam["keyword"].ToString().Trim();
                pagination.conditionJson += string.Format(" and realname like '%{0}%'", realname);
            }
            if (!queryParam["status"].IsEmpty() && queryParam["status"].ToString().Trim() != "全部")
            {//1已授权0未授权
                string status = queryParam["status"].ToString().Trim();
                if (status == "已授权")
                {
                    pagination.conditionJson += string.Format(" and t.status='1'");
                }
                else
                {
                    pagination.conditionJson += string.Format(" and (t.status='0' or t.status is null) ");
                }
            }
            if (!string.IsNullOrEmpty(res))
            {//地磅员角色
                pagination.conditionJson += string.Format(" and d.rolename like ('%{0}%')", res);
            }

            DataTable dt = this.BaseRepository().FindTableByProcPager(pagination, DbHelper.DbType);
            return dt;
        }

        /// <summary>
        /// 获取用户授权记录
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public UserEmpowerRecordEntity GetUserRecord(string keyValue)
        {
            Repository<UserEmpowerRecordEntity> equ = new Repository<UserEmpowerRecordEntity>(DbFactory.Base());
            string sql = string.Format("select ACCOUNT,RealName,UserId,StartTime,EndTime,CREATEUSERNAME,Status from wl_userempowerRecord where userid='{0}' order by createdate desc", keyValue);
            UserEmpowerRecordEntity List = equ.FindList(sql).FirstOrDefault();
            return List;
        }


        /// <summary>
        /// 查询未出场票
        /// </summary>
        /// <returns></returns>
        public CalculateEntity GetEntranceTicket(string carNo)
        {
            CalculateEntity entity = null;
            OperticketmanagerEntity opEntity;

            //车辆上磅称重
            if (!string.IsNullOrEmpty(carNo))
            {
                //Repository<CalculateEntity> repository = new Repository<CalculateEntity>(DbFactory.Base());
                //查询已过皮重未过毛重
                string sql = string.Format(@"SELECT a.id,a.numbers,a.takegoodsname,a.GOODSNAME,a.platenumber,a.tare,a.taretime,a.BASEID,DataType FROM wl_calculate a  where a.isdelete='1' and a.rough is null and a.platenumber='{0}'", carNo);
                entity = this.BaseRepository().FindList(sql).FirstOrDefault();
                if (entity == null)
                {
                    //查询已开票未称重数据(开票室)
                    sql = string.Format("SELECT * FROM WL_OPERTICKETMANAGER a WHERE a.isdelete='1' and  NOT EXISTS(SELECT ID FROM WL_CALCULATE b WHERE a.ID=b.BASEID) AND PLATENUMBER='{0}'", carNo);
                    Repository<OperticketmanagerEntity> repository = new Repository<OperticketmanagerEntity>(DbFactory.Base());
                    opEntity = repository.FindList(sql).OrderByDescending(x => x.CreateDate).FirstOrDefault();

                    //未找到开票信息
                    if (opEntity == null)
                    {
                        //装船开票数据
                        Repository<OperticketmanagerEntity> repositoryLoading = new Repository<OperticketmanagerEntity>(DbFactory.Base());
                        sql = string.Format("SELECT * FROM WL_OPERTICKETMANAGER WHERE ShipLoading=1 and  PLATENUMBER='{0}' and CREATEDATE>TO_DATE('{1}','yyyy-MM-dd HH24:mi:ss')", carNo, DateTime.Now.AddHours(-2).ToString("yyyy-MM-dd HH:mm:ss"));
                        opEntity = repositoryLoading.FindList(sql).OrderByDescending(x => x.CreateDate).FirstOrDefault();
                        if (opEntity != null)
                        {
                            entity = AutoGenerateTicket(opEntity);
                        }
                        else
                        {
                            //查询已开票未称重数据(地磅室)
                            sql = string.Format("SELECT * FROM WL_CALCULATEDETAILED a WHERE NOT EXISTS(SELECT NUMBERS FROM WL_CALCULATE b WHERE a.NUMBERS=b.NUMBERS) AND PLATENUMBER='{0}'", carNo);
                            Repository<CalculateDetailedEntity> repositoryPound = new Repository<CalculateDetailedEntity>(DbFactory.Base());
                            CalculateDetailedEntity detailEntity = repositoryPound.FindList(sql).OrderByDescending(x => x.CreateDate).FirstOrDefault();
                            if (detailEntity != null)
                            {
                                entity = new CalculateEntity()
                                {
                                    BaseId = detailEntity.ID,
                                    DataType = "0",
                                    Numbers = detailEntity.Numbers,
                                    Takegoodsname = detailEntity.Takegoodsname,
                                    Goodsname = detailEntity.Goodsname,
                                    Platenumber = detailEntity.Platenumber,
                                };
                            }
                            else
                            {
                                //卸船开票
                                Repository<CalculateDetailedEntity> repositoryPoundUnloading = new Repository<CalculateDetailedEntity>(DbFactory.Base());
                                sql = string.Format("SELECT * FROM WL_CALCULATEDETAILED WHERE ShipUnloading=1 and  PLATENUMBER='{0}' and CREATEDATE>TO_DATE('{1}','yyyy-MM-dd HH24:mi:ss')", carNo, DateTime.Now.AddHours(-2).ToString("yyyy-MM-dd HH:mm:ss"));
                                detailEntity = repositoryPoundUnloading.FindList(sql).OrderByDescending(x => x.CreateDate).FirstOrDefault();
                                if (detailEntity != null)
                                {
                                    entity = AutoGenerateTicket(detailEntity);
                                }
                            }
                        }
                    }
                    else //已存在开票数据
                    {
                        entity = new CalculateEntity()
                        {
                            BaseId = opEntity.ID,
                            DataType = "4",
                            Numbers = opEntity.Numbers,
                            Takegoodsname = opEntity.Takegoodsname,
                            Goodsname = opEntity.Producttype,
                            Platenumber = opEntity.Platenumber,
                        };
                    }
                }
            }
            return entity;
        }

        /// <summary>
        /// 自动生成装船开票单
        /// </summary>
        /// <param name="originalEntity"></param>
        private CalculateEntity AutoGenerateTicket(OperticketmanagerEntity originalEntity)
        {
            OperticketmanagerEntity newEntity = new OperticketmanagerEntity()
            {                 
                CreateUserId = originalEntity.CreateUserId,
                CreateUserName = originalEntity.CreateUserName,
                Createuserdeptid = originalEntity.Createuserdeptid,
                CreateUserDeptCode = originalEntity.CreateUserDeptCode,
                CreateUserOrgCode = originalEntity.CreateUserOrgCode,
                CreateDate = DateTime.Now,
                Getdata = DateTime.Now,
                Producttype = originalEntity.Producttype,
                ProducttypeId = originalEntity.ProducttypeId,
                Platenumber = originalEntity.Platenumber,
                DriverName=originalEntity.DriverName,
                DriverTel=originalEntity.DriverTel,
                Dress = originalEntity.Dress,
                Takegoodsname = originalEntity.Takegoodsname,
                Takegoodsid = originalEntity.Takegoodsid,
                Supplyname = originalEntity.Supplyname,
                Supplyid = originalEntity.Supplyid,
                Opername = originalEntity.Opername,
                Operaccount = originalEntity.Operaccount,
                Transporttype = originalEntity.Transporttype,
                Isdelete = 1,
                Remark = originalEntity.Remark,
                PassRemark = originalEntity.PassRemark,
                IsFirst = "是",
                Status = originalEntity.Status,
                ExamineStatus = 0,
                Weight = 0,
                ISwharf = originalEntity.ISwharf,
                TravelStatus = originalEntity.TravelStatus,
                TemplateSort = originalEntity.TemplateSort,
                ShipLoading = originalEntity.ShipLoading
            };
             new OperticketmanagerService().SaveForm(string.Empty,newEntity);
            

            CalculateEntity returnEntity = new CalculateEntity()
            {
                BaseId = newEntity.ID,
                Numbers = newEntity.Numbers,
                DataType = "4",
                Takegoodsname = newEntity.Takegoodsname,
                Goodsname = newEntity.Producttype,
                Platenumber = newEntity.Platenumber,
            };
            return returnEntity;
        }

        /// <summary>
        /// 自动生成装船开票单
        /// </summary>
        /// <param name="originalEntity"></param>
        private CalculateEntity AutoGenerateTicket(CalculateDetailedEntity originalEntity)
        {
            CalculateDetailedEntity newEntity = new CalculateDetailedEntity()
            {
                CreateUserId = originalEntity.CreateUserId,
                CreateUserName = originalEntity.CreateUserName,
                CreateUserDeptCode = originalEntity.CreateUserDeptCode,
                CreateUserOrgCode = originalEntity.CreateUserOrgCode,
                Createuserdeptid = originalEntity.Createuserdeptid,
                Platenumber = originalEntity.Platenumber,
                Takegoodsname = originalEntity.Takegoodsname,
                Goodsname = originalEntity.Goodsname,
                ShipUnloading = originalEntity.ShipUnloading,
                DataType = originalEntity.DataType,
                IsDelete = 1,
                Remark = originalEntity.Remark
            };
            SaveWeightBridgeDetail(string.Empty, newEntity);

            CalculateEntity returnEntity = new CalculateEntity()
            {
                BaseId = newEntity.ID,
                Numbers = newEntity.Numbers,
                DataType = newEntity.DataType,
                Takegoodsname = newEntity.Takegoodsname,
                Goodsname = newEntity.Goodsname,
                Platenumber = newEntity.Platenumber,
            };
            return returnEntity;
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
        public void SaveForm(string keyValue, CalculateEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                entity.Netwneight = entity.Rough - entity.Tare;
                this.BaseRepository().Update(entity);
            }
            else
            {
                var existsEntity = this.BaseRepository().FindEntity(x => x.Numbers == entity.Numbers);
                if (existsEntity == null)
                {
                    entity.Create();
                    entity.Netwneight = entity.Rough - entity.Tare;
                    this.BaseRepository().Insert(entity);
                }
            }
        }

        /// <summary>
        /// 手机接口保存
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void SaveAppForm(string keyValue, CalculateEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                //entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                //entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="entity"></param>
        public void SaveWeightBridgeDetail(string key, CalculateDetailedEntity entity)
        {
            Repository<CalculateDetailedEntity> Repository = new Repository<CalculateDetailedEntity>(DbFactory.Base());
            if (!string.IsNullOrEmpty(key))
            {
                entity.Modify(key);
                Repository.Update(entity);
            }
            else
            {
                entity.Create();
                string sql = string.Format("SELECT COUNT(NUMBERS) FROM WL_CALCULATEDETAILED WHERE  TO_CHAR(CREATEDATE,'yyyy-MM-dd')='{0}'", DateTime.Now.ToString("yyyy-MM-dd"));
                lock (numberLock)
                {
                    DataTable count = this.BaseRepository().FindTable(sql);
                    string numbers = string.Format("{0}{1}", DateTime.Now.ToString("yyMMdd"), (Convert.ToInt32(count.Rows[0][0]) + 1).ToString().PadLeft(3, '0'));
                    entity.Numbers = numbers;
                    Repository.Insert(entity);
                }
            }
        }

        /// <summary>
        /// 获取记录管理详情记录实体
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public CalculateDetailedEntity GetAppDetailedEntity(string KeyValue)
        {
            Repository<CalculateDetailedEntity> equ = new Repository<CalculateDetailedEntity>(DbFactory.Base());
            return equ.FindEntity(KeyValue);
        }


        /// <summary>
        /// 保存用户授权信息
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveUserForm(string keyValue, UserEmpowerRecordEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Create();
                Repository<UserEmpowerRecordEntity> equ = new Repository<UserEmpowerRecordEntity>(DbFactory.Base());
                equ.Insert(entity);
            }
        }

        #endregion
    }
}
