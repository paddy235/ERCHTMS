using ERCHTMS.Entity.FireManage;
using ERCHTMS.IService.FireManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System;
using System.Text;
using ERCHTMS.Code;
using System.Collections;

namespace ERCHTMS.Service.FireManage
{
    /// <summary>
    /// 描 述：消防设施
    /// </summary>
    public class FirefightingService : RepositoryFactory<FirefightingEntity>, FirefightingIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;
            if (queryJson != null && queryJson != "")
            {
                var queryParam = queryJson.ToJObject();
                
                //查询条件 名称
                if (!queryParam["EquipmentNameNo"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and EquipmentNameNo='{0}'", queryParam["EquipmentNameNo"].ToString());
                }
                //查询条件 类型
                if (!queryParam["ExtinguisherTypeNo"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and ExtinguisherTypeNo='{0}'", queryParam["ExtinguisherTypeNo"].ToString());
                }
                //部门
                if (!queryParam["DutyDeptCode"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and DutyDeptCode like '{0}%'", queryParam["DutyDeptCode"].ToString());
                }
                //检查 开始时间
                if (!queryParam["ExamineStartDate"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(@" and ExamineDate >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", queryParam["ExamineStartDate"].ToString());
                }
                //检查 结束时间
                if (!queryParam["ExamineEndDate"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(@" and ExamineDate < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(queryParam["ExamineEndDate"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
                }
                //维保 开始时间
                if (!queryParam["DetectionStartDate"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(@" and DetectionDate >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", queryParam["DetectionStartDate"].ToString());
                }
                //维保 结束时间
                if (!queryParam["DetectionEndDate"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(@" and DetectionDate < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(queryParam["DetectionEndDate"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<FirefightingEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public FirefightingEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 获取统计数据
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable StatisticsData(string queryJson) {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"select max(EquipmentNameNo) as EquipmentNameNo,sum(amount) as EquipmentNum from HRS_FIREFIGHTING");
            DatabaseType dataType = DbHelper.DbType;
            if (queryJson != null && queryJson != "")
            {
                var queryParam = queryJson.ToJObject();

                //查询条件 名称
                if (!queryParam["EquipmentNameNo"].IsEmpty())
                {
                    strSql.AppendFormat(" where 1=1");
                    strSql.AppendFormat(" and EquipmentNameNo='{0}'", queryParam["EquipmentNameNo"].ToString());
                    //查询条件 类型
                    if (!queryParam["ExtinguisherTypeNo"].IsEmpty())
                    {
                        strSql.AppendFormat(" and ExtinguisherTypeNo='{0}'", queryParam["ExtinguisherTypeNo"].ToString());
                    }
                    //部门
                    if (!queryParam["DutyDeptCode"].IsEmpty())
                    {
                        strSql.AppendFormat(" and DutyDeptCode like '{0}%'", queryParam["DutyDeptCode"].ToString());
                    }
                    //检查 开始时间
                    if (!queryParam["ExamineStartDate"].IsEmpty())
                    {
                        strSql.AppendFormat(" and ExamineDate >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", queryParam["ExamineStartDate"].ToString());
                    }
                    //检查 结束时间
                    if (!queryParam["ExamineEndDate"].IsEmpty())
                    {
                        strSql.AppendFormat(" and ExamineDate < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(queryParam["ExamineEndDate"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
                    }
                    //维保 开始时间
                    if (!queryParam["DetectionStartDate"].IsEmpty())
                    {
                        strSql.AppendFormat(" and DetectionDate >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", queryParam["DetectionStartDate"].ToString());
                    }
                    //维保 结束时间
                    if (!queryParam["DetectionEndDate"].IsEmpty())
                    {
                        strSql.AppendFormat(" and DetectionDate < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(queryParam["DetectionEndDate"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
                    }
                }
                else {
                    string whereStr = "where 1=1";
                    //部门
                    if (!queryParam["DutyDeptCode"].IsEmpty())
                    {
                        whereStr += string.Format(" and DutyDeptCode like '{0}%'", queryParam["DutyDeptCode"].ToString());
                    }

                    //检查 开始时间
                    if (!queryParam["ExamineStartDate"].IsEmpty())
                    {
                        whereStr += string.Format(" and ExamineDate >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", queryParam["ExamineStartDate"].ToString());
                    }
                    //检查 结束时间
                    if (!queryParam["ExamineEndDate"].IsEmpty())
                    {
                        whereStr += string.Format(" and ExamineDate < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(queryParam["ExamineEndDate"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
                    }
                    //维保 开始时间
                    if (!queryParam["DetectionStartDate"].IsEmpty())
                    {
                        whereStr += string.Format(" and DetectionDate >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", queryParam["DetectionStartDate"].ToString());
                    }
                    //维保 结束时间
                    if (!queryParam["DetectionEndDate"].IsEmpty())
                    {
                        whereStr += string.Format(" and DetectionDate < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(queryParam["DetectionEndDate"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
                    }
                    strSql.AppendFormat(@" {0} group by EquipmentNameNo
                                           UNION ALL
                                          select null as EquipmentNameNo,sum(amount) as EquipmentNum 
                                           from HRS_FIREFIGHTING {0}", whereStr);
                    
                }
            }
            return new RepositoryFactory().BaseRepository().FindTable(strSql.ToString());
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
            this.BaseRepository().Delete(it => id.Contains(it.Id));
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
        public void SaveForm(string keyValue, FirefightingEntity entity)
        {
            entity.Id = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                FirefightingEntity fe = this.BaseRepository().FindEntity(keyValue);
                if (fe == null)
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

        /// <summary>
        /// 同一类型，编号不能重复
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Code"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public bool ExistCode(string Type, string Code, string keyValue)
        {
            var expression = LinqExtensions.True<FirefightingEntity>();
            expression = expression.And(t => t.EquipmentNameNo == Type);
            expression = expression.And(t => t.EquipmentCode == Code);
            if (!string.IsNullOrEmpty(keyValue))
            {
                expression = expression.And(t => t.Id != keyValue);
            }
            return this.BaseRepository().IQueryable(expression).Count() == 0 ? true : false;
        }

        /// <summary>
        /// 根据区域获取区域底下的消防设施
        /// </summary>
        /// <param name="areaCodes">区域编码</param>
        /// <returns></returns>
        public IList GetCountByArea(List<string> areaCodes)
        {
            var query = BaseRepository().IQueryable(x => areaCodes.Contains(x.DistrictCode)).GroupBy(x => new { x.DistrictCode, x.District, x.EquipmentName }).Select(x => new
            {
                DistrictName = x.Key.District,
                DistrictID = x.Key.DistrictCode,
                x.Key.EquipmentName,
                Count = x.Count()
            });
            var data = query.ToList();
            return data;
        }
    }
}
