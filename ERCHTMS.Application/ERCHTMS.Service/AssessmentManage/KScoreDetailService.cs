using ERCHTMS.Entity.AssessmentManage;
using ERCHTMS.IService.AssessmentManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.Data;
using System;
using ERCHTMS.Code;

namespace ERCHTMS.Service.AssessmentManage
{
    /// <summary>
    /// 描 述：自评扣分明细
    /// </summary>
    public class KScoreDetailService : RepositoryFactory<KScoreDetailEntity>, KScoreDetailIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<KScoreDetailEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public KScoreDetailEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

          /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageListJson(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;
            DataTable dtresult = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            foreach (DataRow item in dtresult.Rows)
            {   //当前用户对于扣分项的总分
                var dt = this.BaseRepository().FindTable(string.Format(@"select nvl(sum(kscore),0) from   bis_kscoredetail where assessmentplanid='{0}' and createuserid='{1}'", item[0], user.UserId));
               if (dt.Rows.Count > 0)
               {
                   item["score"] = dt.Rows[0][0].ToString();
               }
                //总的状态
               var dt1 = this.BaseRepository().FindTable(string.Format(@"select a,b from (select count(1) a ,assessmentplanid,GradeStatus,sum(count(1)) over() b from bis_assessmentsum where 
                              assessmentplanid='{0}' group by assessmentplanid,GradeStatus)t where GradeStatus='未评分'", item[0]));
                if (dt1.Rows.Count > 0)
                {
                    if (dt1.Rows[0][0].ToString() == dt1.Rows[0][1].ToString())
                        item["Status"] = "未评分";
                    else
                        item["Status"] = "进行中";
                }
                else//所有的大项都已经被评分了
                {
                    item["Status"] = "已提交";
                }
                DataTable dt4 = new DataTable();
                string duty = "";
                string islock = "";
                dt4 = this.BaseRepository().FindTable(string.Format(@"select id,islock,teamleader from bis_assessmentplan where id='{0}'", item[0]));
                if (dt4.Rows.Count > 0)
                {
                    duty = dt4.Rows[0][2].ToString();
                    islock = dt4.Rows[0][1].ToString();
                }
                var dt2 = this.BaseRepository().FindTable(string.Format(@"select count(id) from bis_assessmentsum  where assessmentplanid='{0}' and dutyid='{1}'", item[0], user.UserId));
                var dt3 = this.BaseRepository().FindTable(string.Format(@"select count(id) from bis_assessmentsum  where assessmentplanid='{0}' and dutyid='{1}' and GradeStatus='已评分'", item[0], user.UserId));
                if (dt2.Rows.Count > 0 && dt3.Rows.Count > 0)
                {
                    if (islock == "锁定")//没有锁定
                    {
                        if (duty == user.UserId)
                        {
                            item["able"] = true;
                            item["isUpdate"] = "评分";
                        }
                        else//不是负责人
                        {
                            int self = Convert.ToInt32(dt3.Rows[0][0].ToString());//已评分项数
                            int sum = Convert.ToInt32(dt2.Rows[0][0].ToString());//总数
                            if (sum != 0)
                            {
                                item["able"] = true;
                                if (self == sum)//个人项总数和个人项评分选项相等
                                    item["isUpdate"] = "修订";
                                else
                                    item["isUpdate"] = "评分";
                            }
                        }
                    }
                    else//锁定了该自评计划
                    {
                        item["isUpdate"] = "评分";
                    }
                    item["progress"] = dt3.Rows[0][0].ToString() + "/" + dt2.Rows[0][0].ToString();
                }
            }
            return dtresult;
        }


        /// <summary>
        /// 根据计划id获取数据
        /// </summary>
        /// <param name="planid"></param>
        /// <returns></returns>
        public DataTable GetDetailInfo(string planid)
        {
            DataTable dtresult = new DataTable();
            dtresult.Columns.Add("chaptersid");
            dtresult.Columns.Add("chaptersname");//大节点名称
            dtresult.Columns.Add("kscorenum");//扣分项数
            dtresult.Columns.Add("isclick");//是否能点击(1.是,2:否)
            dtresult.Columns.Add("issum");//总结状态
            dtresult.Columns.Add("gradestatus");//评分状态
            int duty = 0;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            duty = this.BaseRepository().FindObject(string.Format(@" select count(id) from bis_assessmentplan where id='{0}' and teamleader='{1}'", planid, user.UserId)).ToInt();
            //获取大节点
            var dt = this.BaseRepository().FindTable("select ID,MajorNumber,ChaptersName from bis_assessmentchapters where ChaptersParentID='-1' order by cast(replace(majornumber,'.','') as number)");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dtresult.NewRow();
                var num = this.BaseRepository().FindObject(string.Format(@"select count(id) from bis_kscoredetail where assessmentplanid='{0}' and chapterid in(select id from bis_assessmentchapters where ChaptersParentID='{1}')", planid, dt.Rows[i]["ID"].ToString())).ToInt();

                var dtIsSum = this.BaseRepository().FindTable(string.Format(" select selfsum,GradeStatus from bis_assessmentsum where chapterid='{0}' and assessmentplanid='{1}'", dt.Rows[i]["ID"].ToString(), planid));
                string isSum = "未总结";
                string GradeStatus = "未评分";
                if (dtIsSum.Rows.Count > 0)
                {
                    isSum = string.IsNullOrEmpty(dtIsSum.Rows[0][0].ToString()) ? "未总结" : "已总结";
                    GradeStatus = dtIsSum.Rows[0][1].ToString();//评分状态
                }
                if (duty > 0)//是自评组长
                {
                    row["isclick"] = 1;
                }
                else//不是自评组长
                {
                    //看此项是否是当前登录人评审
                    var dutynum = this.BaseRepository().FindObject(string.Format(@"select count(id) from bis_assessmentsum where chapterid='{0}' and assessmentplanid='{1}' and dutyid='{2}'", dt.Rows[i]["ID"].ToString(), planid, user.UserId)).ToInt();
                    row["isclick"] = dutynum > 0 ? "1" : "2";
                }
                row["chaptersname"] = dt.Rows[i]["MajorNumber"] + dt.Rows[i]["ChaptersName"].ToString();//大章节名称
                row["kscorenum"] = num;
                row["chaptersid"] = dt.Rows[i]["ID"].ToString();
                row["issum"] = isSum;
                row["gradestatus"] = GradeStatus;
                dtresult.Rows.Add(row);
            }
            return dtresult;
        }

        /// <summary>
        ///大项的所有小项列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetAllDetailPage(Pagination pagination)
        {
            DatabaseType dataType = DbHelper.DbType;
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }


        /// <summary>
        /// 根据计划id和小项节点id获取扣分项
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="chapterid"></param>
        /// <returns></returns>
        public KScoreDetailEntity GetKScoreByPlanOrChapID(string planid, string chapterid)
        {
            var expression = LinqExtensions.True<KScoreDetailEntity>();
            if (!string.IsNullOrEmpty(planid))
            {
                expression = expression.And(t => t.AssessmentPlanID == planid);
            }
            if (!string.IsNullOrEmpty(chapterid))
            {
                expression = expression.And(t => t.ChapterID == chapterid);
            }
            return this.BaseRepository().IQueryable(expression).ToList().FirstOrDefault();
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
        public void SaveForm(string keyValue, KScoreDetailEntity entity)
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
