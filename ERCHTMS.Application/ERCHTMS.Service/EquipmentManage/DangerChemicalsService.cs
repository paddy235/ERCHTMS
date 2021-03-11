using ERCHTMS.Entity.EquipmentManage;
using ERCHTMS.IService.EquipmentManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util.Extension;
using BSFramework.Util;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Service.BaseManage;
using ERCHTMS.Service.AuthorizeManage;
using ERCHTMS.Code;
using System;

namespace ERCHTMS.Service.EquipmentManage
{
    /// <summary>
    /// 描 述：危险化学品库存
    /// </summary>
    public class DangerChemicalsService : RepositoryFactory<DangerChemicalsEntity>, DangerChemicalsIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<DangerChemicalsEntity> GetList(string queryJson)
        {
            var sql = string.Format("select * from XLD_DANGEROUSCHEMICAL where 1=1 {0}", queryJson);
            return this.BaseRepository().FindList(sql);
        }
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var queryParam = queryJson.ToJObject();
           

            //部门code 
            if (!queryParam["DutyDeptCode"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and DutyDeptCode like '{0}%'", queryParam["DutyDeptCode"].ToString());
            }
            //危险品类型 
            if (!queryParam["RiskType"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and RiskType = '{0}'", queryParam["RiskType"].ToString());
            }
            //存放地点类型 
            if (!queryParam["IsScene"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and IsScene = '{0}'", queryParam["IsScene"].ToString());
            }
            //危化品名称 
            if (!queryParam["Name"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and Name like '%{0}%'", queryParam["Name"].ToString());
            }

            var dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);

            return dt;
        }        
        /// <summary>
        /// 获取工作流节点
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public DataTable GetWorkDetailList(string objectId)
        {
            DatabaseType dataType = DbHelper.DbType;
            string sql = string.Format("select * from sys_wftbactivity where processid='{0}' order by autoid asc", objectId);
            var dt = this.BaseRepository().FindTable(sql);

            return dt;
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public DangerChemicalsEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 获取待审核部门工作计划
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int GetDangerChemicalsBMNum(ERCHTMS.Code.Operator user) {
            int count = 0;
            try
            {
                count = BaseRepository().FindObject(string.Format(@"select count(0)
from hrs_DangerChemicals
where createuserorgcode='{0}' 
and (instr(checkuseraccount,'{1}')>0 or flowstate='结束') and applytype = '部门工作计划' and baseid is null
and checkuseraccount like '%{1}%'", user.OrganizeCode, user.Account)).ToInt();
            }
            catch {
                return 0;
            }
            return count;
        }
        /// <summary>
        /// 获取待审核个人工作计划
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int GetDangerChemicalsGRNum(ERCHTMS.Code.Operator user)
        {
            int count = 0;
            try
            {
                count = BaseRepository().FindObject(string.Format(@"select count(0)
from hrs_DangerChemicals
where createuserorgcode='{0}' 
and (instr(checkuseraccount,'{1}')>0 or flowstate='结束') and applytype = '个人工作计划' and baseid is null
and checkuseraccount like '%{1}%'", user.OrganizeCode,user.Account)).ToInt();
            }
            catch
            {
                return 0;
            }
            return count;
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            var old = GetEntity(keyValue);
            if (old != null)
            {
                old.IsDelete = 1;  //删除
                this.BaseRepository().Update(old);
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, DangerChemicalsEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                var old = GetEntity(keyValue);
                if (old == null)
                {
                    entity.Create();
                    entity.ID = keyValue;
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
    }
}
