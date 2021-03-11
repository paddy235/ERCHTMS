using ERCHTMS.Entity.ComprehensiveManage;
using ERCHTMS.IService.ComprehensiveManage;
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
using ERCHTMS.Entity.SystemManage.ViewModel;
using ERCHTMS.Service.SystemManage;

namespace ERCHTMS.Service.ComprehensiveManage
{
    /// <summary>
    /// 描 述：信息报送表
    /// </summary>
    public class InfoSubmitService : RepositoryFactory<InfoSubmitEntity>, InfoSubmitIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<InfoSubmitEntity> GetList(string queryJson)
        {
            var sql = string.Format("select * from hrs_infosubmit where 1=1 {0}", queryJson);
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
            if (pagination.p_fields.IsEmpty())
            {
                pagination.p_fields = @"createdate,createuserid,createuserdeptcode,createuserorgcode,createusername,modifydate,modifyuserid,infoname,require,starttime,endtime,submituserid,submiteduserid,submituseraccount,submitusername,submitdepartid,submitdepartname,pct,remnum,remusername,remdepartname,issubmit,infotype";
            }
            pagination.p_kid = "id";
            pagination.p_tablename = @"hrs_infosubmit";
            pagination.conditionJson = "1=1";

            //公司级用户、EHS部门用户查看全厂数据
            DataItemModel ehsDepart = new DataItemDetailService().GetDataItemListByItemCode("'EHSDepartment'").Where(p => p.ItemName == user.OrganizeId).ToList().FirstOrDefault();
            if (user.RoleName.Contains("公司级用户") || (ehsDepart != null && ehsDepart.ItemValue == user.DeptCode))
            {
                pagination.conditionJson += string.Format(" and createuserorgcode='{0}' and (issubmit='是' or createuserid='{1}')", user.OrganizeCode, user.UserId);
            }
            else if (user.RoleName.Contains("负责人") || user.RoleName.Contains("安全管理员"))
            {//本部门及子部门数据
                //pagination.conditionJson += string.Format(@" and issubmit='是' and instr(submitdepartid,'{0}')>0", user.DeptId);//本部门数据
                pagination.conditionJson += string.Format(@" and issubmit='是' and id in(select distinct(id) from hrs_infosubmit join base_department d on instr(submitdepartid,d.departmentid)>0 where d.encode like '{0}%')", user.DeptCode);
            }
            else
            {//其他人员查看自己相关的数据
                pagination.conditionJson += string.Format(@" and issubmit='是' and instr(submituserid,'{0}')>0", user.UserId);
            }
            //开始时间
            if (!queryParam["starttime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and starttime >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", queryParam["starttime"].ToString());
            }
            //结束时间
            if (!queryParam["endtime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and starttime < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(queryParam["endtime"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
            }
            //类型
            if (!queryParam["infotype"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and infotype = '{0}'", queryParam["infotype"].ToString());
            }
            //报送名称
            if (!queryParam["infoname"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and infoname like '%{0}%'", queryParam["infoname"].ToString());
            }
            //首页跳转
            if (!queryParam["indextype"].IsEmpty() && queryParam["indextype"].ToString()=="1")
            {
                pagination.conditionJson += string.Format(@" and instr(submituserid,'{0}')>0 and (submiteduserid is null or instr(submiteduserid,'{0}')<=0)", user.UserId);
            }
            var dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);

            return dt;
        }
        /// <summary>
        /// 首页提醒
        /// </summary>
        /// <param name="indexType">提醒类型（1：填报信息，2：新增报送要求）</param>
        /// <returns></returns>
        public int CountIndex(string indexType)
        {
            int num = 0;

            if (indexType == "1")
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                string sql = string.Format("select count(1) from hrs_infosubmit where instr(submituserid,'{0}')>0 and (submiteduserid is null or instr(submiteduserid,'{0}')<=0)", user.UserId);
                object obj = this.BaseRepository().FindObject(sql);
                int.TryParse(obj.ToString(), out num);
            }

            return num;
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public InfoSubmitEntity GetEntity(string keyValue)
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
            this.BaseRepository().Delete(x => x.ID == keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, InfoSubmitEntity entity)
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
