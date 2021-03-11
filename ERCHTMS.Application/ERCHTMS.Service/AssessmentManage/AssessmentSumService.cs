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
    /// 描 述：自评总结
    /// </summary>
    public class AssessmentSumService : RepositoryFactory<AssessmentSumEntity>, AssessmentSumIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<AssessmentSumEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public AssessmentSumEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }


        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }


        /// <summary>
        /// 根据计划id和大项节点id获取
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="chapterid"></param>
        /// <returns></returns>
        public AssessmentSumEntity GetSumByPlanOrChapID(string planid, string chapterid)
        {
            var expression = LinqExtensions.True<AssessmentSumEntity>();
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

        /// <summary>
        /// 获取综述等相关数据
        /// </summary>
        /// <param name="planid"></param>
        /// <returns></returns>
        public DataTable GetSumDataInfo(string planid)
        {
            DataTable dtresult = new DataTable();
            dtresult.Columns.Add("bscore");//标准分
            dtresult.Columns.Add("kscore");//扣分
            dtresult.Columns.Add("nosuitscore");//不适宜项分
            dtresult.Columns.Add("sum");//总分
            dtresult.Columns.Add("zscore");//最终得分
            dtresult.Columns.Add("grade");//评定等级
            dtresult.Columns.Add("leadersum");//综述
            var strleader = this.BaseRepository().FindObject("select leadersum from bis_assessmentplan where id='" + planid + "'").ToString();
            int bScore = this.BaseRepository().FindObject("select nvl(sum(score),0) from bis_assessmentchapters where ChaptersParentID='-1'").ToInt();//标准分
            int kScore = this.BaseRepository().FindObject(string.Format("select nvl(sum(kScore),0) from bis_kscoredetail where AssessmentPlanID='{0}'", planid)).ToInt();//总扣分
            int noSuitScore = this.BaseRepository().FindObject(string.Format("select nvl(sum(score),0) from bis_assessmentchapters where id in(select chapterid from bis_nosuitabledetail  where assessmentplanid='{0}')", planid)).ToInt();//不适宜项分数
            //总得分=标准得分-总扣分-不适宜项得分
            int sumScore = bScore - kScore - noSuitScore;
            //最终得分=总得分/（标准总分-不适宜项分）
            double zScore = Math.Round(((double)sumScore / (double)(bScore - noSuitScore)) * 100, 2);
            DataRow row = dtresult.NewRow();
            row["bscore"] = bScore;
            row["kscore"] = kScore;
            row["nosuitscore"] = noSuitScore;
            row["sum"] = sumScore;
            row["zscore"] = zScore.ToString() + "%";
            string str = "";
            //（等级）一级：最终得分大于等于90分；二级：最终得分大于等于80分；三级：最终得分大于等于70分。
            if (zScore >= 90)
                str = "一级";
            else if (zScore >= 80)
                str = "二级";
            else if (zScore >= 70)
                str = "三级";
            else
                str = "四级";
            row["grade"] = str;
            row["leadersum"] = strleader;
            dtresult.Rows.Add(row);
            return dtresult;
        }

        /// <summary>
        /// 根据计划id和大项节点id获取数据
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="chapterid"></param>
        /// <returns></returns>
        public DataTable GetSummarizeInfo(string planid, string chapterid)
        {
            DataTable dtresult = new DataTable();
            dtresult.Columns.Add("selfsum");//自评总结
            dtresult.Columns.Add("chaptersname");//大节点名称
            dtresult.Columns.Add("describe");//得分情况
            DataRow row = dtresult.NewRow();
            var expression = LinqExtensions.True<AssessmentSumEntity>();
            if (!string.IsNullOrEmpty(planid))
                expression = expression.And(t => t.AssessmentPlanID == planid);
            if (!string.IsNullOrEmpty(chapterid))
                expression = expression.And(t => t.ChapterID == chapterid);
            var resultentity = this.BaseRepository().IQueryable(expression).ToList().FirstOrDefault();
            if (resultentity != null)
                row["selfsum"] = resultentity.SelfSum;
            var dt = this.BaseRepository().FindTable(string.Format("select count(id),nvl(sum(score),0) from bis_assessmentchapters where id in(select chapterid from bis_nosuitabledetail  where assessmentplanid='{0}' and chapterid in(select id  from bis_assessmentchapters where ChaptersParentID='{1}'))", planid, chapterid));
            var dt1 = this.BaseRepository().FindTable(string.Format("select nvl(sum(kscore),0) from bis_kscoredetail  where assessmentplanid='{0}' and chapterid in(select id  from bis_assessmentchapters where ChaptersParentID='{1}')", planid, chapterid));
            var dt2 = this.BaseRepository().FindTable(string.Format("select Score,MajorNumber,ChaptersName  from bis_assessmentchapters where id='{0}'", chapterid));
            string sumScore = dt2.Rows[0]["Score"].ToString();
            row["chaptersname"] = dt2.Rows[0]["MajorNumber"].ToString() + dt2.Rows[0]["ChaptersName"].ToString();//显示大章节名称（例如：5.1目标）
            int number = 0, nosuitScore = 0, KSumScore = 0;
            if (dt.Rows.Count > 0)
            {
                number = Convert.ToInt32(dt.Rows[0][0].ToString());
                nosuitScore = Convert.ToInt32(dt.Rows[0][1].ToString());
            }
            if (dt1.Rows.Count > 0)
                KSumScore = Convert.ToInt32(dt1.Rows[0][0].ToString());
            //实际得分=该要素的所有分值总和-不适宜的分值总和-扣分分值总和
            string reallyScore = (Convert.ToInt32(sumScore) - nosuitScore - KSumScore).ToString();
            row["describe"] = "本项标准分值:" + sumScore + ",实际得分分值:" + reallyScore + ",不适宜项" + number + "项";
            dtresult.Rows.Add(row);
            return dtresult;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetSumUpPageJson(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;
            DataTable dtresult = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            string duty = "";
            string islock = "";
            foreach (DataRow item in dtresult.Rows)
            {
                var dt = this.BaseRepository().FindTable(string.Format("select count(id) from bis_assessmentsum where assessmentplanid='{0}'", item["id"].ToString()));
                var dt1 = this.BaseRepository().FindTable(string.Format("select count(id) from bis_assessmentsum where assessmentplanid='{0}' and selfsum is not null", item["id"].ToString()));
                item["progress"] = Convert.ToInt32(dt1.Rows[0][0].ToString()) + "/" + Convert.ToInt32(dt.Rows[0][0].ToString());//自评进度
                if (string.IsNullOrEmpty(item["leadersum"].ToString()))//自评综述是否为空
                {
                    item["status"] = "进行中";
                }
                else
                {
                    item["status"] = "已提交";
                }
                var dtSuty = this.BaseRepository().FindTable(string.Format(@"select id,islock,teamleader from bis_assessmentplan where id='{0}'", item["id"].ToString()));
                if (dtSuty.Rows.Count > 0)
                {
                    duty = dtSuty.Rows[0][2].ToString();
                    islock = dtSuty.Rows[0][1].ToString();
                }
                if (islock == "锁定")//没有锁定
                {
                    if (duty == user.UserId)//是负责人
                    {
                        if (dt1.Rows[0][0].ToString() == dt.Rows[0][0].ToString())
                        {//完成了所有自评总结
                            item["isupdate"] = "1";
                            if (!string.IsNullOrEmpty(item["leadersum"].ToString()))
                                item["report"] = "1";
                        }
                        else
                            item["isupdate"] = "0";

                    }
                    else//是负责人
                        item["isupdate"] = "0";
                }
                else//锁定了该自评计划
                    item["isupdate"] = "0";
            }
            return dtresult;
        }


        /// <summary>
        /// 根据计划id统计数据
        /// </summary>
        /// <param name="planid"></param>
        /// <returns></returns>
        public string GetSumDataCount(string planid)
        {
            var dt = this.BaseRepository().FindTable("select ID,MajorNumber,ChaptersName from bis_assessmentchapters where ChaptersParentID='-1' order by cast(replace(majornumber,'.','') as number)");
            var chapterN = dt.Select().Select(x => x.Field<string>("MajorNumber") + x.Field<string>("ChaptersName"));
            DataTable dtSum = new DataTable();
            dtSum = GetEveryBigNoSuitScore(planid);
            var sum = dtSum.Select().Select(x => Math.Round(double.Parse(x.Field<string>("zScore")), 2));
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { Fields = chapterN, data = sum });
        }



        /// <summary>
        /// 每个项的标准得分，不适宜项分，扣分，最终得分
        /// </summary>
        /// <returns></returns>
        public DataTable GetEveryBigNoSuitScore(string planid)
        {
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            DataTable dt4 = new DataTable();
            dt2.Columns.Add("id", typeof(string));//大章节id
            dt2.Columns.Add("MajorNumber", typeof(string));//大章节要素号
            dt2.Columns.Add("ChaptersName", typeof(string));//大章节项目名称
            dt2.Columns.Add("nosuidCount", typeof(string));//不适宜项数量
            dt2.Columns.Add("gradeCount", typeof(string));//评分项数量
            dt2.Columns.Add("kCount", typeof(string));//扣分项数量
            dt2.Columns.Add("score", typeof(string));//标准分
            dt2.Columns.Add("zScore", typeof(string));//最终得分
            dt2.Columns.Add("yScore", typeof(string));//实得分=标准得分-总扣分-不适宜项得分
            dt2.Columns.Add("scoreRate", typeof(string));//得分率=实得分/标准分
            dt2.Columns.Add("nosuidScore", typeof(string));//不适宜项分
            dt = this.BaseRepository().FindTable("select ID,score,MajorNumber,ChaptersName from bis_assessmentchapters where ChaptersParentID='-1' order by cast(replace(majornumber,'.','') as number)");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow newrow = dt2.NewRow();
                var strs = this.BaseRepository().FindObject(string.Format(" select count(id) from bis_assessmentchapters where ChaptersParentID='{0}' and content is not null", dt.Rows[i][0].ToString())).ToInt();
                newrow["gradeCount"] = strs;
                newrow["id"] = dt.Rows[i][0].ToString();
                newrow["score"] = dt.Rows[i][1].ToString();
                newrow["MajorNumber"] = dt.Rows[i][2].ToString();
                newrow["ChaptersName"] = dt.Rows[i][3].ToString();
                dt3 = this.BaseRepository().FindTable(string.Format("select nvl(sum(score),0),count(id) from bis_assessmentchapters where id in(select chapterid from bis_nosuitabledetail  where assessmentplanid='{0}' and chapterid in(select id  from bis_assessmentchapters where ChaptersParentID='{1}')) ", planid, dt.Rows[i][0].ToString()));
                newrow["nosuidCount"] = dt3.Rows[0][1].ToString();
                newrow["nosuidScore"] = dt3.Rows[0][0].ToString();
                dt4 = this.BaseRepository().FindTable(string.Format("select nvl(sum(kscore),0),count(id) from bis_kscoredetail  where assessmentplanid='{0}' and chapterid in(select id  from bis_assessmentchapters where ChaptersParentID='{1}')", planid, dt.Rows[i][0].ToString()));
                newrow["kCount"] = dt4.Rows[0][1].ToString();
                //  //总得分=标准得分-总扣分-不适宜项得分
                //最终得分=总得分/（标准总分-不适宜项分）；
                int sumScore = Convert.ToInt32(dt.Rows[i][1].ToString()) - Convert.ToInt32(dt4.Rows[0][0].ToString()) - Convert.ToInt32(dt3.Rows[0][0].ToString());
                //对被除数或除数为0做如下处理
                if (sumScore != 0)
                {
                    newrow["yScore"] = sumScore;
                    string bsum = dt.Rows[i][1].ToString();
                    if (bsum != "0")
                    {
                        newrow["scoreRate"] = Math.Round(((double)sumScore / Convert.ToDouble(bsum)) * 100, 2);
                        double m = (double)sumScore / (double)(Convert.ToInt32(bsum) - Convert.ToInt32(dt3.Rows[0][0].ToString()));
                        newrow["zScore"] = Math.Round(m * 100, 2).ToString();
                    }
                    else
                    {
                        newrow["scoreRate"] = 0;
                        newrow["zScore"] = 0;
                    }
                }
                else
                {
                    newrow["yScore"] = 0;
                    newrow["scoreRate"] = 0;
                    newrow["zScore"] = 0;
                }
                dt2.Rows.Add(newrow);
            }
            return dt2;
        }


        public string GetEveryBigPerson(string planid, string type)
        {
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            string dutyname = "";
            dt = this.BaseRepository().FindTable("select ID,score from bis_assessmentchapters where ChaptersParentID='-1' order by cast(replace(majornumber,'.','') as number)");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (type == "1")//所有大项
                {
                    dt2 = this.BaseRepository().FindTable(string.Format("select DutyName from bis_assessmentsum where chapterid='{0}' and assessmentplanid='{1}'", dt.Rows[i][0], planid));
                    dutyname += dt2.Rows[0][0].ToString() + ",";
                }
                if (type == "2")//第7项和第10项
                {
                    if (i == 6 || i == 9)
                    {
                        dt2 = this.BaseRepository().FindTable(string.Format("select DutyName from bis_assessmentsum where chapterid='{0}' and assessmentplanid='{1}'", dt.Rows[i][0], planid));
                        dutyname += dt2.Rows[0][0].ToString() + ",";
                    }
                }
                if (type == "3")//第六项
                {
                    if (i == 5)
                    {
                        dt2 = this.BaseRepository().FindTable(string.Format("select DutyName from bis_assessmentsum where chapterid='{0}' and assessmentplanid='{1}'", dt.Rows[i][0], planid));
                        dutyname += dt2.Rows[0][0].ToString() + ",";
                    }
                }

            }
            if (dutyname.Contains(","))
            {
                dutyname = dutyname.TrimEnd(',');
            }
            return dutyname;
        }



        /// <summary>
        /// 该计划每个项扣分和（除第六项,七项和第十项之外）
        /// </summary>
        /// <returns></returns>
        public int GetEverySumScore(string planid)
        {
            int num = this.BaseRepository().FindObject(string.Format(@"select nvl(sum(kscore),0) as sumscore from bis_kscoredetail where chapterid not in(select id from bis_assessmentchapters where ChaptersParentID in (select id from bis_assessmentchapters where ChaptersParentID='-1' and majornumber='5.6' or majornumber='5.7' or majornumber='5.10') and content is not null)
and assessmentplanid='{0}'", planid)).ToInt();
            return num;
        }


        /// <summary>
        /// 该计划每个项扣分值和扣分原因（除第六项,七项和第十项之外）
        /// </summary>
        /// <returns></returns>
        public string GetEveryResonAndScore(string planid)
        {
            DataTable dt = new DataTable();
            dt = this.BaseRepository().FindTable(string.Format(@"select kScoreReason,kscore  from bis_kscoredetail where chapterid not in(select id from bis_assessmentchapters where ChaptersParentID in (select id from bis_assessmentchapters where ChaptersParentID='-1' and majornumber='5.6' or majornumber='5.7' or majornumber='5.10') and content is not null)
and assessmentplanid='{0}'", planid));
            string s = "主要扣分点:";
            string m = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                m += dt.Rows[i][0].ToString() + "; 扣" + dt.Rows[i][1].ToString() + "分; ";
            }
            if (string.IsNullOrEmpty(m))
                return "";
            else
                return s + m;
        }

        /// <summary>
        /// 该计划第七项和第十项扣分和
        /// </summary>
        /// <returns></returns>
        public int GetEverySumScore2(string planid)
        {
            int num = this.BaseRepository().FindObject(string.Format(@"select nvl(sum(kscore),0) as sumscore from bis_kscoredetail where chapterid  in(select id from bis_assessmentchapters where ChaptersParentID in (select id from bis_assessmentchapters where ChaptersParentID='-1' and  majornumber='5.7' or majornumber='5.10') and content is not null)
and assessmentplanid='{0}'", planid)).ToInt();
            return num;
        }


        /// <summary>
        /// 该计划第七项和第十项扣分值和扣分原因
        /// </summary>
        /// <returns></returns>
        public string GetEveryResonAndScore2(string planid)
        {
            DataTable dt = new DataTable();
            dt = this.BaseRepository().FindTable(string.Format(@"select kScoreReason,kscore  from bis_kscoredetail where chapterid  in(select id from bis_assessmentchapters where ChaptersParentID in (select id from bis_assessmentchapters where ChaptersParentID='-1' and  majornumber='5.7' or majornumber='5.10') and content is not null)
and assessmentplanid='{0}'", planid));
            string s = "主要扣分点:";
            string m = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                m += dt.Rows[i][0].ToString() + "; 扣" + dt.Rows[i][1].ToString() + "分; ";
            }
            if (string.IsNullOrEmpty(m))
                return "";
            else
                return s + m;
        }


        /// <summary>
        /// 该计划以下元素扣分和
        /// </summary>
        /// <returns></returns>
        public int GetEverySumScore3(string planid, string strMarjor)
        {
            int num = this.BaseRepository().FindObject(string.Format(@"select nvl(sum(kscore),0) as sumscore from bis_kscoredetail where chapterid  in(select id from bis_assessmentchapters where majornumber in({1}))
and assessmentplanid='{0}'", planid, strMarjor)).ToInt();
            return num;
        }


        /// <summary>
        /// 该计划以下元素扣分值和扣分原因
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="strMarjor"></param>
        /// <returns></returns>
        public string GetEveryResonAndScore3(string planid, string strMarjor)
        {
            DataTable dt = new DataTable();
            dt = this.BaseRepository().FindTable(string.Format(@"select kScoreReason,kscore  from bis_kscoredetail where chapterid  in(select id from bis_assessmentchapters where majornumber in({1}))
and assessmentplanid='{0}'", planid, strMarjor));
            string s = "主要扣分点:";
            string m = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                m += dt.Rows[i][0].ToString() + "; 扣" + dt.Rows[i][1].ToString() + "分; ";
            }
            if (string.IsNullOrEmpty(m))
                return "";
            else
                return s + m;
        }

        /// <summary>
        /// 附件一的统计
        /// </summary>
        /// <param name="planid"></param>
        /// <returns></returns>
        public DataTable GetAffixOne(string planid)
        {
            DataTable dt = new DataTable();
            dt = this.BaseRepository().FindTable(string.Format(@"select majorNumber,ChaptersName,kScoreReason,score,kscore,score-kscore as rScore,Measure from bis_kscoredetail a left join bis_assessmentchapters b
                 on a.chapterid=b.id  where assessmentplanid='{0}'", planid));
            return dt;
        }


        /// <summary>
        /// 获取根节点的章节的所有标准分
        /// </summary>
        /// <param name="code"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public int GetBigChapterScore()
        {
            int num = this.BaseRepository().FindObject("select nvl(sum(score),0) from bis_assessmentchapters where ChaptersParentID='-1'").ToInt();
            return num;
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
        public void SaveForm(string keyValue, AssessmentSumEntity entity)
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
        /// 根据自评计划id删除数据
        /// </summary>
        /// <param name="planId">自评计划id</param>
        public int Remove(string planId)
        {
            return this.BaseRepository().ExecuteBySql(string.Format("delete from bis_assessmentsum where AssessmentPlanID='{0}'", planId));
        }
        #endregion
    }
}
