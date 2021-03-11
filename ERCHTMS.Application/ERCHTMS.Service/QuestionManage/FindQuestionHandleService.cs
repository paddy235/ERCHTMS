using ERCHTMS.Entity.QuestionManage;
using ERCHTMS.IService.QuestionManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace ERCHTMS.Service.QuestionManage
{
    /// <summary>
    /// 描 述：发现问题处理记录表
    /// </summary>
    public class FindQuestionHandleService : RepositoryFactory<FindQuestionHandleEntity>, FindQuestionHandleIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<FindQuestionHandleEntity> GetList(string queryJson) 
        {
            return this.BaseRepository().IQueryable().ToList();
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public FindQuestionHandleEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion

        #region 获取问题处理内容
        /// <summary>
        /// 获取问题处理内容
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public DataTable GetQuestionHandleTable(string keyValue)
        {
            string sql = string.Format(@" select  *  from ( select a.*,to_char(b.workstream) appstate ,to_char(b.addtype) addtype,to_char(b.id) appid   from bis_findquestionhandle a left  join  bis_htbaseinfo b  on  a.relevanceid = b.id  where  a.questionid ='{0}' and  a.relevancetype='yh' 
                                        union
                                        select a.*,b.flowstate  appstate ,b.addtype,to_char(b.id) appid  from bis_findquestionhandle a left  join  bis_lllegalregister b on  a.relevanceid = b.id  where a.questionid ='{0}' and  a.relevancetype='wz' 
                                        union
                                        select a.*,b.flowstate appstate  ,'0' addtype ,to_char(b.id) appid  from bis_findquestionhandle a left  join  bis_questioninfo b on  a.relevanceid = b.id  where a.questionid ='{0}' and  a.relevancetype='wt' 
                                        union
                                        select a.*,'' appstate  ,'' addtype ,'' appid   from bis_findquestionhandle a  where a.questionid ='{0}' and a.relevanceid is null )  a order by a.createdate desc ", keyValue);

            return this.BaseRepository().FindTable(sql);
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
        public void SaveForm(string keyValue, FindQuestionHandleEntity entity)
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