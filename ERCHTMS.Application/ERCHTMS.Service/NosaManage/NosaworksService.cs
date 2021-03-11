using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.WebControl;
using ERCHTMS.Code;
using ERCHTMS.Entity.NosaManage;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ERCHTMS.IService.NosaManage;
using ERCHTMS.Service.SystemManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ERCHTMS.Service.NosaManage
{
    /// <summary>
    /// 描 述：工作任务
    /// </summary>
    public class NosaworksService : RepositoryFactory<NosaworksEntity>, NosaworksIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<NosaworksEntity> GetList(string queryJson)
        {
            var sql = string.Format("select * from hrs_nosaworks where 1=1 {0}", queryJson);
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
                pagination.p_fields = @"createdate,createuserid,createuserdeptcode,createuserorgcode,modifydate,modifyuserid,name,according,ratenum,enddate,dutyuserid,dutyusername,dutydepartid,dutydepartname,submituserid,submitusername,eleno,elename,eledutyuserid,eledutyusername,eledutydepartname,issubmited,pct,dutyuserhtml,dutydeparthtml";
                pagination.p_fields += ",(select wm_concat(to_char(name)) from HRS_NOSAWorkResult r where r.workid=HRS_NOSAWorks.Id) workresult";
                //pagination.p_fields += ",(select wm_concat(to_char(dutyusername)) from HRS_NOSAWorkitem r where r.workid=HRS_NOSAWorks.Id and r.state='通过') checkedusername";
                pagination.p_fields += ",(select count(1) from HRS_NOSAWorkitem r where r.workid=HRS_NOSAWorks.Id and r.state='待审核') checkcount";
                pagination.p_fields += ",(select case when (state='待上传' or state='不通过') then 1 when state='待审核' then 2 else 3 end from hrs_nosaworkitem i where i.workid=hrs_nosaworks.id and i.dutyuserid='" + user.UserId + "' and rownum<=1) as state";
                pagination.p_fields += ",(select state || '|' || to_char(uploaddate,'YYYY-MM-DD') from hrs_nosaworkitem i where i.workid=hrs_nosaworks.id and i.dutyuserid='" + user.UserId + "' and rownum<=1) as itemcol";
            }                                                                                                                        
            pagination.p_kid = "id";                                                                                                   
            pagination.p_tablename = @"hrs_nosaworks";
            pagination.conditionJson = "1=1";

            if (!queryParam["datascope"].IsEmpty() && queryParam["datascope"].ToString()=="1")
            {//我上传时的数据范围
                pagination.conditionJson += string.Format(" and createuserorgcode='{0}' and issubmited='是' and instr(dutyuserid,'{1}')>0", user.OrganizeCode, user.UserId);
                if (!queryParam["waitforupload"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and (submituserid is null or instr(submituserid,'{0}')<=0)", user.UserId);
                }
            }
            else
            {//NOSA工作清单列表默认数据范围                
                DataItemModel ehsDepart = new DataItemDetailService().GetDataItemListByItemCode("'EHSDepartment'").Where(p => p.ItemName == user.OrganizeId).ToList().FirstOrDefault();
                if (user.RoleName.Contains("公司级用户") || (ehsDepart != null && ehsDepart.ItemValue == user.DeptCode))
                {//公司级用户、EHS部门用户查看全厂数据
                    pagination.conditionJson += string.Format(" and ((createuserorgcode='{0}' and issubmited='是') or createuserid='{1}')", user.OrganizeCode, user.UserId);
                }
                else if (user.RoleName.Contains("负责人") || user.RoleName.Contains("安全管理员"))
                {//本部门及子部门数据
                    //pagination.conditionJson += string.Format(@" and createuserdeptcode like '{0}%' and (issubmited='是' or createuserid='{1}')", user.DeptCode, user.UserId);
                    pagination.conditionJson += string.Format(@" and (((createuserdeptcode like '{0}%' or instr(dutyuserid,'{1}')>0) and issubmited='是') or createuserid='{1}')", user.DeptCode, user.UserId);
                }
                else
                {//自己创建或审核的数据
                    //pagination.conditionJson += string.Format(@" and createuserid='{0}'", user.UserId);
                    pagination.conditionJson += string.Format(@" and (createuserid='{0}' or (issubmited='是' and instr(dutyuserid,'{0}')>0))", user.UserId);
                }
            }
            //文件名称                                                                                                            
            if (!queryParam["name"].IsEmpty())                                                                                
            {                                                                                                                   
                pagination.conditionJson += string.Format(@" and name like '%{0}%'", queryParam["name"].ToString());      
            }                                                                                                                   
            //引用id                                                                                                                     
            if (!queryParam["eleid"].IsEmpty())                                                                                          
            {                                                                                                                          
                pagination.conditionJson += string.Format(@" and eleid = '{0}'", queryParam["eleid"].ToString());                
            }
            //开始时间
            if (!queryParam["starttime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and enddate >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", queryParam["starttime"].ToString());
            }
            //结束时间
            if (!queryParam["endtime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and enddate < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(queryParam["endtime"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
            }
            //数据范围                                                                                                                     
            if (!queryParam["datascope"].IsEmpty())
            {
                var datascope = queryParam["datascope"].ToString();                
                if (datascope == "2")
                {//我创建的数据
                    pagination.conditionJson += string.Format(" and createuserid='{0}'", user.UserId);
                }
                else if (datascope == "3")
                {//我应审核的数据
                    pagination.conditionJson += string.Format(" and issubmited='是' and eledutyuserid='{0}' and exists(select 1 from hrs_nosaworkitem i where i.workid=hrs_nosaworks.id and i.state='待审核')", user.UserId);
                }
            }
            var dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);                                       
                                                                                                                                        
            return dt;                                                                                                                  
        }
        /// <summary>
        /// 首页提醒
        /// </summary>
        /// <param name="indexType">提醒类型（1：我应上传，3：我应审核）</param>
        /// <returns></returns>
        public int CountIndex(string indexType)
        {
            int num = 0;

            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (indexType == "1")
            {                
                string sql = string.Format("select count(1) from hrs_nosaworks where createuserorgcode='{0}' and issubmited='是' and instr(dutyuserid,'{1}')>0 and (submituserid is null or instr(submituserid,'{1}')<=0)", user.OrganizeCode, user.UserId);
                object obj = this.BaseRepository().FindObject(sql);
                int.TryParse(obj.ToString(), out num);
            }
            else if (indexType == "3")
            {
                string sql = string.Format("select count(1) from hrs_nosaworks where createuserorgcode='{0}' and issubmited='是' and eledutyuserid='{1}' and exists(select 1 from hrs_nosaworkitem i where i.workid=hrs_nosaworks.id and i.state='待审核')", user.OrganizeCode, user.UserId);
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
        public NosaworksEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, NosaworksEntity entity)
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
