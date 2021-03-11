using ERCHTMS.Entity.BaseManage;
using ERCHTMS.IService.BaseManage;
using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using System.Linq;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.IService.HiddenTroubleManage;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Service.BaseManage;
using BSFramework.Application.Entity;
using ERCHTMS.Code;
using ERCHTMS.IService.SystemManage;
using ERCHTMS.Service.SystemManage;
using ERCHTMS.Entity.SystemManage;
using System.Data.Common;

namespace ERCHTMS.Service.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：流程配置实例表
    /// </summary>
    public class WfInstanceService : RepositoryFactory<WfInstanceEntity>, WfInstanceIService
    {

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<WfInstanceEntity> GetList(string queryJson)
        {
            var list = this.BaseRepository().IQueryable().ToList();
            if (!string.IsNullOrEmpty(queryJson))
            {
                list = list.Where(p => p.INSTANCETYPE == "基础流程").ToList();
            }
            else
            {
                list = list.Where(p => p.INSTANCETYPE != "基础流程").ToList();
            }
            return list;
        }





        #region 获取特定的流程配置实例
        /// <summary>
        /// 获取特定的流程配置实例
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="rankname"></param>
        /// <param name="mark"></param>
        /// <returns></returns>
        public DataTable GetProcessData()
        {
            string sql = "select id,name,code from sys_wftbprocess  where 1=1 ";

            return this.BaseRepository().FindTable(sql);
        }
        #endregion


        #region 获取流程对象
        /// <summary>
        /// 获取流程对象
        /// </summary>
        /// <param name="instanceid"></param>
        /// <returns></returns>
        public DataTable GetActivityData(string instanceid)
        {
            string sql = string.Format(@"select a.name , a.id from sys_wftbactivity a 
                          left join sys_wftbprocess b  on a.processid = b.id 
                           left join bis_wfinstance c on b.id = c.processid where c.id = '{0}'", instanceid);
            return this.BaseRepository().FindTable(sql);
        }
        #endregion

        #region 获取特定的流程配置实例
        /// <summary>
        /// 获取特定的流程配置实例
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="rankname"></param>
        /// <param name="mark"></param>
        /// <returns></returns>
        public List<WfInstanceEntity> GetListByArgs(string orgid, string rankname, string mark)
        {

            var list = this.BaseRepository().IQueryable().ToList();
            string sql = @"select   id,                
                                  autoid,           
                                  createuserid,   
                                  createuserdeptcode,
                                  createuserorgcode,
                                  createdate,    
                                  createusername,    
                                  modifydate,        
                                  modifyuserid,      
                                  modifyusername,    
                                  instancename,      
                                  organizeid,        
                                  organizename,      
                                  rankname,          
                                  objectname,        
                                  isenable,          
                                  mark,              
                                  remarks           
                                 from bis_wfinstance  where 1=1 ";
            string strwhere = string.Empty;
            var parameter = new List<DbParameter>();
            if (!string.IsNullOrEmpty(orgid))
            {
                strwhere += @" and  organizeid = @organizeid";
                parameter.Add(DbParameters.CreateDbParameter("@organizeid", orgid));
            }
            if (!string.IsNullOrEmpty(rankname))
            {
                strwhere += @" and  rankname = @rankname";
                parameter.Add(DbParameters.CreateDbParameter("@rankname", rankname));
            }
            if (!string.IsNullOrEmpty(mark))
            {
                strwhere += @" and  mark = @mark";
                parameter.Add(DbParameters.CreateDbParameter("@mark", mark));
            }
            sql += strwhere;
            DbParameter[] arrayparam = parameter.ToArray();
            return this.BaseRepository().FindList(sql, arrayparam).ToList();
        }
        #endregion

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public WfInstanceEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion

        #region  流程配置实例信息
        /// <summary>
        /// 流程配置实例信息
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetWfInstanceInfoPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            if (pagination.p_fields.IsEmpty())
            {
                pagination.p_fields = @"serialnumber,instancetype,instancename,organizeid,rankname,isenable,mark,remarks,organizename,createdate,processname";
            }
            pagination.p_kid = "id";
            pagination.conditionJson = " 1=1";
            var queryParam = queryJson.ToJObject();
            //当前用户
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.p_tablename = @" (
                                         select a.id, a.serialnumber,a.instancetype, a.instancename,a.organizeid,a.rankname,a.createdate,
                                         a.isenable,a.mark,a.remarks ,a.organizename,b.name processname from bis_wfinstance a  
                                          left join sys_wftbprocess b on a.processid = b.id
                                        ) ";
            //机构id
            if (!queryParam["organizeid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and  organizeid ='{0}' ", queryParam["organizeid"].ToString());
            }
            //级别ID
            if (!queryParam["rankname"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and  rankname like '%{0}%' ", queryParam["rankname"].ToString());
            }
            //流程标记
            if (!queryParam["mark"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and  mark like '%{0}%' ", queryParam["mark"].ToString());
            }
            //流程类型
            if (!queryParam["instancetype"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and  instancetype = '{0}' ", queryParam["instancetype"].ToString());
            }
            else
            {
                pagination.conditionJson += @" and  instancetype !='基础流程' ";
            }
            var dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);

            return dt;
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
        public void SaveForm(string keyValue, WfInstanceEntity entity)
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

        #region 批量更新流程实例
        /// <summary>
        /// 批量更新流程实例Id
        /// </summary>
        /// <param name="typename"></param>
        public void BatchUpdateInstance(string typename)
        {
            string sql0 = string.Format(@"select * from bis_wfinstance where instancetype ='{0}'", typename);

            DataTable dt0 = this.BaseRepository().FindTable(sql0);

            foreach (DataRow row0 in dt0.Rows)
            {
                string oldId = row0["id"].ToString(); //历史id

                string newId = Guid.NewGuid().ToString(); //新id

                string sql1 = string.Format(@"select * from bis_wfsetting where instanceid ='{0}'", oldId);

                DataTable dt1 = this.BaseRepository().FindTable(sql1);

                foreach (DataRow row1 in dt1.Rows)
                {
                    string oldsettingId = row1["id"].ToString();

                    string newsettingId = Guid.NewGuid().ToString(); //新id

                    string sql2 = string.Format(@"select * from  bis_wfcondition where settingid ='{0}'", oldsettingId);

                    DataTable dt2 = this.BaseRepository().FindTable(sql2);

                    foreach (DataRow row2 in dt2.Rows)
                    {
                        string oldconditionId = row2["id"].ToString();

                        string newconditionId = Guid.NewGuid().ToString(); //新id

                        string sql3 = string.Format(@"select * from  bis_wfconditionaddtion where wfconditionid ='{0}'", oldconditionId);

                        DataTable dt3 = this.BaseRepository().FindTable(sql3);

                        foreach (DataRow row3 in dt3.Rows)
                        {
                            string oldconditionaddtionId = row3["id"].ToString();

                            string newconditionaddtionId = Guid.NewGuid().ToString(); //新id  

                            string upsql3 = string.Format(@"update bis_wfconditionaddtion set id ='{0}',wfconditionid='{1}' where id ='{2}'", newconditionaddtionId, newconditionId, oldconditionaddtionId);

                            this.BaseRepository().ExecuteBySql(upsql3); //执行更新
                        }
                        string upsql2 = string.Format(@"update bis_wfcondition set id ='{0}',settingid='{1}'  where id ='{2}'", newconditionId, newsettingId, oldconditionId);

                        this.BaseRepository().ExecuteBySql(upsql2); //执行更新
                    }
                    string upsql1 = string.Format(@"update bis_wfsetting set id ='{0}',instanceid='{1}' where id ='{2}'", newsettingId, newId, oldsettingId);

                    this.BaseRepository().ExecuteBySql(upsql1); //执行更新
                }

                string upsql0 = string.Format(@"update bis_wfinstance set id ='{0}' where id ='{1}'", newId, oldId);

                this.BaseRepository().ExecuteBySql(upsql0); //执行更新

                //条件更新
                string sql4 = string.Format(@"select * from  bis_wfconditionofrole where instanceid ='{0}'", oldId);

                DataTable dt4 = this.BaseRepository().FindTable(sql4);

                foreach (DataRow row4 in dt4.Rows)
                {
                    string oldroleid = row4["id"].ToString();

                    string newroleid = Guid.NewGuid().ToString(); //新id   

                    string upsql4 = string.Format(@"update bis_wfconditionofrole set  id ='{0}',instanceid='{1}'  where id ='{2}'", newroleid, newId, oldroleid);

                    this.BaseRepository().ExecuteBySql(upsql4); //执行更新
                }
            }
        }
        #endregion

        #endregion
    }
}