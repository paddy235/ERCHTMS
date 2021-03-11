  using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.IService.RiskDatabase;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Util.Extension;
using System.Data;
using System.Text;
using ERCHTMS.Code;
using System;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
namespace ERCHTMS.Service.RiskDatabase
{
    /// <summary>
    /// 描 述：风险点信息
    /// </summary>
    public class DangerSourceService : RepositoryFactory<DangerSourceEntity>, DangerSourceIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns>返回列表</returns>
        public IEnumerable<DangerSourceEntity> GetList()
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        ///根据区域获取列表
        /// </summary>
        /// <param name="parentId">节点Id</param>
        /// <param name="keyword">关键字查询</param>
        /// <returns></returns>
        public IEnumerable<DangerSourceEntity> GetList(string parentId, string keyword)
        {
            var expression = LinqExtensions.True<DangerSourceEntity>();
            if (!string.IsNullOrEmpty(parentId))
            {
                expression = expression.And(t => t.ParentId == parentId);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.Name.Contains(keyword));
                expression = expression.Or(t => t.Name.Contains(keyword));
            }
            return this.BaseRepository().IQueryable(expression).OrderBy(t => t.CreateDate).ToList();
        }
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询条件</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            
            //风险级别
            if (!queryParam["level"].IsEmpty())
            {
                string level = queryParam["level"].ToString();
                pagination.conditionJson += string.Format(" and gradeval={0}", level);
            }
           
            //区域Code
            string areaCode = "";
            if (!queryParam["areaCode"].IsEmpty())
            {
                areaCode = queryParam["areaCode"].ToString();
                pagination.conditionJson += string.Format(" and areaCode like '{0}%'", areaCode);
            }
            //区域ID
            if (!queryParam["areaId"].IsEmpty())
            {
                string areaId = queryParam["areaId"].ToString();
                pagination.conditionJson += string.Format(" and areaId = '{0}'", areaId);
            }
            //风险等级
            if (!queryParam["grade"].IsEmpty())
            {
                string grade = queryParam["grade"].ToString();
                pagination.conditionJson += string.Format(" and grade = '{0}'", grade);
            }
            //事故类型
            if (!queryParam["accType"].IsEmpty())
            {
                string accType = queryParam["accType"].ToString();
                pagination.conditionJson += string.Format(" and AccidentName like '%{0}%'", accType);
            }
            //部门Code
            if (!queryParam["deptCode"].IsEmpty())
            {
                string deptCode = queryParam["deptCode"].ToString();
                pagination.conditionJson += string.Format(" and deptCode like '{0}%'", deptCode);
            }
            if (!queryParam["deptCode1"].IsEmpty())
            {
                string deptCode = queryParam["deptCode1"].ToString();
                pagination.conditionJson += string.Format(" and deptCode='{0}'", deptCode);
            }
            //查询关键字
            if (!queryParam["keyWord"].IsEmpty())
            {
                string keyWord = queryParam["keyWord"].ToString().Trim();
                pagination.conditionJson += string.Format(" and (Description like '%{0}%' or riskdesc like '%{0}%' or result like '%{0}%' or WorkTask like '%{0}%') ", keyWord.Trim());
            }
            if (!queryParam["keyValue"].IsEmpty())
            {
                string keyValue = queryParam["keyValue"].ToString();
                pagination.conditionJson += string.Format(" and ( deptname like '%{0}%' or postname like '%{0}%') ", keyValue.Trim());
            }
            if (!queryParam["AreaIds"].IsEmpty())
            {
                string AreaIds = queryParam["AreaIds"].ToString();
                if (AreaIds.Length > 0)
                {
                    pagination.conditionJson += string.Format(" and districtid in('{0}')", AreaIds.Replace(",", "','"));
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public DangerSourceEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 根据名称查询记录是否存在
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public string GetIdByName(string name)
        {
            DataTable dt=this.BaseRepository().FindTable(string.Format("select id from BIS_DANGERSOURCE where name='{0}'",name.TrimEnd()));
            if (dt.Rows.Count>0)
            {
                return dt.Rows[0][0].ToString();
            }
            return "";
        }
        /// <summary>
        /// 获取下拉选项html字符串
        /// <param name="parentId">父节点Id</param>
        /// </summary>
        public string GetOptionsStringForArea(string parentId,string orgCode="")
        {
            string sql = string.Format("select NAME,ID from BIS_DANGERSOURCE where ParentId='{0}'", parentId);
            if (!string.IsNullOrEmpty(orgCode))
            {
                sql = string.Format("select distinct areaNAME,areaID from BIS_RISKASSESS where deptcode='{0}' and areaNAME is not null", orgCode);
            }
            DataTable dt = this.BaseRepository().FindTable(sql);
            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in dt.Rows)
            {
                sb.AppendFormat("<option value='{0}'>{1}</option>", dr[1].ToString(), dr[0].ToString());
            }
            return sb.ToString();
        }
        /// <summary>
        /// 获取内置部门信息
        /// </summary>
        public string GetOptionsStringForInitDept()
        {
            DataTable dt = this.BaseRepository().FindTable(string.Format("select distinct deptname from BIS_RISKDATABASE"));
            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in dt.Rows)
            {
                sb.AppendFormat("<option value='{0}'>{0}</option>", dr[0].ToString());
            }
            dt.Dispose();
            return sb.ToString();
        }
        /// <summary>
        /// 获取内置岗位信息
        /// </summary>
        public string GetOptionsStringForInitPost(string deptName="")
        {
            DataTable dt = null;
            if (string.IsNullOrEmpty(deptName))
            {
                dt = this.BaseRepository().FindTable("select distinct postname from BIS_RISKDATABASE ");
            }
            else
            {
                dt = this.BaseRepository().FindTable(string.Format("select distinct postname from BIS_RISKDATABASE where deptname in ('{0}')", deptName.Replace(",", "','")));
            }
            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in dt.Rows)
            {
                sb.AppendFormat("<option value='{0}'>{0}</option>", dr[0].ToString());
            }
            dt.Dispose();
            return sb.ToString();
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
        public void SaveForm(string keyValue, DangerSourceEntity entity)
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
        /// 修改内置风险点和区域间的关系
        /// </summary>
        /// <param name="districtId">区域ID</param>
        /// <param name="areaId">内置区域ID,多个用英文逗号分割</param>
        /// <param name="areaCode">区域Code</param>
        /// <param name="areaName">区域名称</param>
        /// <param name="deptCode">管控部门Code</param>
        /// <param name="user">当前操作用户</param>
        /// <returns></returns>  
        public int Update(string districtId, string areaId, string areaCode, string areaName, string deptCode,Operator user)
        {
            try
            {
                string id = Guid.NewGuid().ToString();
                //先删除关联数据
                string sql = string.Format(@"delete from bis_riskassess where  deptcode='{0}' and status=1 and districtid='{1}'", deptCode,  districtId);
                this.BaseRepository().ExecuteBySql(sql);

                sql = string.Format(@"insert into bis_riskassess(id,areaid,areaname,areacode,dangersource,way,itema,itemb,itemc,itemr,grade,deptcode,deptname,postid,postname,createuserid,createdate,createusername,createuserdeptcode,createuserorgcode,status,gradeval,RESULT,ResultType,accidentname,AccidentType,harmtype,risktype,districtid,districtname,DeleteMark,EquipmentName,Parts,WorkTask,Process,RiskDesc,HTMEASURES,majorname,description,harmproperty,faulttype,levelname)
select  id || '{0}',areaid,areaname,'{1}',t.dangersource,t.way,t.itema,t.itemb,t.itemc,t.itemr,t.grade,deptcode,deptname,postid,postname,'{2}',to_date('{3}','yyyy-mm-dd hh24:mi:ss'),'{4}','{5}','{6}',1,t.gradeval,t.result,ResultType,t.accidentname,AccidentType,t.harmtype,t.risktype,'{7}','{8}',0,EquipmentName,Parts,WorkTask,Process,RiskDesc,HTMEASURES,majorname,description,harmproperty,faulttype,levelname from bis_riskassess t
where deptcode='{9}' and areaid in('{10}') and status=0 order by id", id, areaCode, user.UserId, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), user.UserName, user.DeptCode, user.OrganizeCode, districtId, areaName, deptCode, areaId.Replace(",", "','"));
                //主表插入成功后在插入措施表
                if (this.BaseRepository().ExecuteBySql(sql) > 0)
                {
                    string newId = Guid.NewGuid().ToString();
                    sql = string.Format(" insert into BIS_MEASURES(id,content,riskid,typename) select '{2}'|| rownum,content,riskid || '{3}',typename from BIS_MEASURES where riskid in(select id from bis_riskassess where deptcode='{0}' and areaid in('{1}') and status=0) order by riskid", deptCode, areaId.Replace(",", "','"), newId, id);
                    this.BaseRepository().ExecuteBySql(sql);
                }
                return 1;
            }
            catch
            {
                return 0;
            }
           
        }
        /// <summary>
        /// 保存部门与内置部门的风险配置清单信息
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="deptName">部门名称</param>
        /// <param name="newDeptName">关联的内置部门名称，多个用英文逗号分隔</param>
        /// <param name="postName">关联的岗位名称，多个用英文逗号分隔</param>
        /// <param name="newPostName">岗位名称</param>
        /// <param name="postId">岗位Id</param>
        /// <param name="user">当前用户对象</param>
        /// <returns></returns>
        public int SaveConfig(string deptCode, string deptName, string newDeptName, string postName, string newPostName, string postId, Operator user)
        {
            try
            {
                
                string id = Guid.NewGuid().ToString();
                //先删除相关数据
                string sql = string.Format(@"delete from bis_riskassess where deptname='{0}' and postname='{1}' and deptcode='{2}' and postid='{3}' and status=0", deptName, postName, deptCode, postId);
                //判断是否配置关联岗位
                if (string.IsNullOrEmpty(postId))
                {
                    sql = string.Format(@"delete from bis_riskassess where deptname='{0}' and deptcode='{1}'  and status=0", deptName, deptCode);
                }
                this.BaseRepository().ExecuteBySql(sql);
                sql = string.Format(@"insert into bis_riskassess(id,areaid,areaname,dangersource,way,itema,itemb,itemc,itemr,grade,deptcode,deptname,postid,postname,createuserid,createdate,createusername,createuserdeptcode,createuserorgcode,status,gradeval,RESULT,accidentname,AccidentType,resulttype,risktype,DeleteMark,EquipmentName,Parts,WorkTask,Process,RiskDesc,HTMEASURES,majorname,description,harmtype,harmproperty)
select id || '{11}',t.areaid,t.areaname,t.dangersource,t.way,t.itema,t.itemb,t.itemc,t.itemr,t.grade,'{0}','{1}','{2}','{3}','{4}',to_date('{5}','yyyy-mm-dd hh24:mi:ss'),'{6}','{7}','{8}',0,t.gradeval,t.result,t.accidentname,AccidentType,t.resulttype,t.risktype,0,EquipmentName,Parts,WorkTask,Process,RiskDesc,HTMEASURES,majorname,description,harmtype,harmproperty,faulttype,levelname from BIS_RISKDATABASE t
where deptname in('{9}') and postname in('{10}') order by id", deptCode, deptName, postId, postName, user.UserId, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), user.UserName, user.DeptCode, user.OrganizeCode, newDeptName.Replace(",", "','"), newPostName.Replace(",", "','"), id);
                //如果未配置关联岗位则值根据关联部门插入数据
                if (string.IsNullOrEmpty(postId))
                {
                    string sql1 = string.Format("select id from BIS_RISKDATABASE where deptname in('{0}')", newDeptName.Replace(",", "','"));

                    sql = string.Format(@"insert into bis_riskassess(id,areaid,areaname,dangersource,way,itema,itemb,itemc,itemr,grade,deptcode,deptname,postid,postname,createuserid,createdate,createusername,createuserdeptcode,createuserorgcode,status,gradeval,RESULT,resulttype,accidentname,AccidentType,risktype,DeleteMark,EquipmentName,Parts,WorkTask,Process,RiskDesc,HTMEASURES,majorname,description,harmtype,harmproperty,faulttype,levelname)
select id || '{10}',t.areaid,t.areaname,t.dangersource,t.way,t.itema,t.itemb,t.itemc,t.itemr,t.grade,'{0}','{1}','{2}','{3}','{4}',to_date('{5}','yyyy-mm-dd hh24:mi:ss'),'{6}','{7}','{8}',0,t.gradeval,t.result,resulttype,t.accidentname,AccidentType,t.risktype,0,EquipmentName,Parts,WorkTask,Process,RiskDesc,HTMEASURES,majorname,description,harmtype,harmproperty,faulttype,levelname from BIS_RISKDATABASE t
where deptname in('{9}') order by id", deptCode, deptName, postId, postName, user.UserId, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), user.UserName, user.DeptCode, user.OrganizeCode, newDeptName.Replace(",", "','"), id);
                }
                //根据部门和岗位配置插入风险清单
                if (this.BaseRepository().ExecuteBySql(sql) > 0)
                {
                    //插入从表风险关联措施记录
                    string newId = Guid.NewGuid().ToString();
                    sql = string.Format(" insert into BIS_MEASURES(id,content,riskid,typename) select '{0}' || rownum,content,riskid || '{3}',typename from BIS_MEASURES where riskid in(select id from BIS_RISKDATABASE where deptname in('{1}') and postname in('{2}'))", newId, newDeptName.Replace(",", "','"), newPostName.Replace(",", "','"),id);
                    if (string.IsNullOrEmpty(postId))
                    {
                        sql = string.Format("insert into BIS_MEASURES(id,content,riskid,typename) select '{0}' || rownum,content,riskid || '{2}',typename from BIS_MEASURES where riskid in(select id from BIS_RISKDATABASE where deptname in('{1}')) order by riskid", newId, newDeptName.Replace(",", "','"),id);
                    }
                    this.BaseRepository().ExecuteBySql(sql);
                }
                return 1;
            }
            catch
            {
                return 0;
            }
           
        }
        /// <summary>
        /// 根据区域编码获取名称全路径,格式如1>1.1>1.1.1
        /// </summary>
        /// <param name="code">区域编码</param>
        /// <returns></returns>
        public string GetPathName(string code,string orgId)
        {
            string dCode = code.Substring(0,6);
            DataTable dt = this.BaseRepository().FindTable(string.Format("select t.districtname from BIS_DISTRICT t where organizeid='{1}' and t.districtcode like '" + dCode + "%' and instr('{0}',t.districtcode)>0 and length(t.districtcode)<=" + code.Length + " order by t.districtcode,t.sortcode asc", code,orgId));
            StringBuilder sb = new StringBuilder();
            foreach(DataRow dr in dt.Rows)
            {
                sb.AppendFormat("{0}>",dr[0].ToString());   
            }
            return sb.ToString().TrimEnd('>');
        }
        #endregion
    }
}