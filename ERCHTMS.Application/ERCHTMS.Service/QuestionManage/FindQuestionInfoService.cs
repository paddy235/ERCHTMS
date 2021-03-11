using ERCHTMS.Entity.QuestionManage;
using ERCHTMS.IService.QuestionManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Code;
using System;

namespace ERCHTMS.Service.QuestionManage
{
    /// <summary>
    /// 描 述：发现问题基本信息表
    /// </summary>
    public class FindQuestionInfoService : RepositoryFactory<FindQuestionInfoEntity>, FindQuestionInfoIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<FindQuestionInfoEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public FindQuestionInfoEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion


        #region  发现问题基础信息查询
        /// <summary>
        /// 发现问题基础信息查询    
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetFindQuestionInfoPageList(Pagination pagination, string queryJson) 
        {
            DatabaseType dataType = DbHelper.DbType;

            pagination.p_fields = @" createuserid ,createusername ,organizeid,organizename,deptid,deptname,questioncontent,questionpic,flowstate,appsign,actionperson,flowdescribe,createdate";

            pagination.p_kid = "id";

            pagination.conditionJson = " 1=1";

            var queryParam = queryJson.ToJObject();

            //当前用户
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();


            pagination.p_tablename = @" ( select a.* ,(case when a.flowstate ='开始' then '问题登记' when a.flowstate ='评估' then '问题评估' else '已处理' end ) flowdescribe,b.actionperson,b.participantname from bis_findquestioninfo a  left join v_findquestionworkflow  b on a.id =b.id ) a";

            //数据范围
            pagination.conditionJson += string.Format(@" and (createuserid ='{0}'  or   (actionperson  like  '%{1}%' and flowstate='评估')  or  flowstate='结束') ", user.UserId, user.Account + ",");

            //查询条件 流程状态
            if (!queryParam["flowstate"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and flowstate = '{0}' ", queryParam["flowstate"].ToString());
            }
            //所属部门
            if (!queryParam["deptid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and deptid = '{0}'", queryParam["deptid"].ToString());
            }
            //问题内容 
            if (!queryParam["questioncontent"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and questioncontent like '%{0}%'", queryParam["questioncontent"].ToString());
            }

            //问题时间开始时间
            if (!queryParam["stdate"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and createdate >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", queryParam["stdate"].ToString());
            }
            //问题时间结束时间
            if (!queryParam["etdate"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and createdate < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(queryParam["etdate"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
            }
            //待评估的
            if (!queryParam["action"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and  actionperson  like  '%{0}%' and flowstate='评估'",  user.Account + ",");
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
        public void SaveForm(string keyValue, FindQuestionInfoEntity entity)
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