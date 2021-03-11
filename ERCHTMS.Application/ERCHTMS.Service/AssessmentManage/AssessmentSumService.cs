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
    /// �� ���������ܽ�
    /// </summary>
    public class AssessmentSumService : RepositoryFactory<AssessmentSumEntity>, AssessmentSumIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<AssessmentSumEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public AssessmentSumEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }


        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }


        /// <summary>
        /// ���ݼƻ�id�ʹ���ڵ�id��ȡ
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
        /// ��ȡ�������������
        /// </summary>
        /// <param name="planid"></param>
        /// <returns></returns>
        public DataTable GetSumDataInfo(string planid)
        {
            DataTable dtresult = new DataTable();
            dtresult.Columns.Add("bscore");//��׼��
            dtresult.Columns.Add("kscore");//�۷�
            dtresult.Columns.Add("nosuitscore");//���������
            dtresult.Columns.Add("sum");//�ܷ�
            dtresult.Columns.Add("zscore");//���յ÷�
            dtresult.Columns.Add("grade");//�����ȼ�
            dtresult.Columns.Add("leadersum");//����
            var strleader = this.BaseRepository().FindObject("select leadersum from bis_assessmentplan where id='" + planid + "'").ToString();
            int bScore = this.BaseRepository().FindObject("select nvl(sum(score),0) from bis_assessmentchapters where ChaptersParentID='-1'").ToInt();//��׼��
            int kScore = this.BaseRepository().FindObject(string.Format("select nvl(sum(kScore),0) from bis_kscoredetail where AssessmentPlanID='{0}'", planid)).ToInt();//�ܿ۷�
            int noSuitScore = this.BaseRepository().FindObject(string.Format("select nvl(sum(score),0) from bis_assessmentchapters where id in(select chapterid from bis_nosuitabledetail  where assessmentplanid='{0}')", planid)).ToInt();//�����������
            //�ܵ÷�=��׼�÷�-�ܿ۷�-��������÷�
            int sumScore = bScore - kScore - noSuitScore;
            //���յ÷�=�ܵ÷�/����׼�ܷ�-��������֣�
            double zScore = Math.Round(((double)sumScore / (double)(bScore - noSuitScore)) * 100, 2);
            DataRow row = dtresult.NewRow();
            row["bscore"] = bScore;
            row["kscore"] = kScore;
            row["nosuitscore"] = noSuitScore;
            row["sum"] = sumScore;
            row["zscore"] = zScore.ToString() + "%";
            string str = "";
            //���ȼ���һ�������յ÷ִ��ڵ���90�֣����������յ÷ִ��ڵ���80�֣����������յ÷ִ��ڵ���70�֡�
            if (zScore >= 90)
                str = "һ��";
            else if (zScore >= 80)
                str = "����";
            else if (zScore >= 70)
                str = "����";
            else
                str = "�ļ�";
            row["grade"] = str;
            row["leadersum"] = strleader;
            dtresult.Rows.Add(row);
            return dtresult;
        }

        /// <summary>
        /// ���ݼƻ�id�ʹ���ڵ�id��ȡ����
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="chapterid"></param>
        /// <returns></returns>
        public DataTable GetSummarizeInfo(string planid, string chapterid)
        {
            DataTable dtresult = new DataTable();
            dtresult.Columns.Add("selfsum");//�����ܽ�
            dtresult.Columns.Add("chaptersname");//��ڵ�����
            dtresult.Columns.Add("describe");//�÷����
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
            row["chaptersname"] = dt2.Rows[0]["MajorNumber"].ToString() + dt2.Rows[0]["ChaptersName"].ToString();//��ʾ���½����ƣ����磺5.1Ŀ�꣩
            int number = 0, nosuitScore = 0, KSumScore = 0;
            if (dt.Rows.Count > 0)
            {
                number = Convert.ToInt32(dt.Rows[0][0].ToString());
                nosuitScore = Convert.ToInt32(dt.Rows[0][1].ToString());
            }
            if (dt1.Rows.Count > 0)
                KSumScore = Convert.ToInt32(dt1.Rows[0][0].ToString());
            //ʵ�ʵ÷�=��Ҫ�ص����з�ֵ�ܺ�-�����˵ķ�ֵ�ܺ�-�۷ַ�ֵ�ܺ�
            string reallyScore = (Convert.ToInt32(sumScore) - nosuitScore - KSumScore).ToString();
            row["describe"] = "�����׼��ֵ:" + sumScore + ",ʵ�ʵ÷ַ�ֵ:" + reallyScore + ",��������" + number + "��";
            dtresult.Rows.Add(row);
            return dtresult;
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
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
                item["progress"] = Convert.ToInt32(dt1.Rows[0][0].ToString()) + "/" + Convert.ToInt32(dt.Rows[0][0].ToString());//��������
                if (string.IsNullOrEmpty(item["leadersum"].ToString()))//���������Ƿ�Ϊ��
                {
                    item["status"] = "������";
                }
                else
                {
                    item["status"] = "���ύ";
                }
                var dtSuty = this.BaseRepository().FindTable(string.Format(@"select id,islock,teamleader from bis_assessmentplan where id='{0}'", item["id"].ToString()));
                if (dtSuty.Rows.Count > 0)
                {
                    duty = dtSuty.Rows[0][2].ToString();
                    islock = dtSuty.Rows[0][1].ToString();
                }
                if (islock == "����")//û������
                {
                    if (duty == user.UserId)//�Ǹ�����
                    {
                        if (dt1.Rows[0][0].ToString() == dt.Rows[0][0].ToString())
                        {//��������������ܽ�
                            item["isupdate"] = "1";
                            if (!string.IsNullOrEmpty(item["leadersum"].ToString()))
                                item["report"] = "1";
                        }
                        else
                            item["isupdate"] = "0";

                    }
                    else//�Ǹ�����
                        item["isupdate"] = "0";
                }
                else//�����˸������ƻ�
                    item["isupdate"] = "0";
            }
            return dtresult;
        }


        /// <summary>
        /// ���ݼƻ�idͳ������
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
        /// ÿ����ı�׼�÷֣���������֣��۷֣����յ÷�
        /// </summary>
        /// <returns></returns>
        public DataTable GetEveryBigNoSuitScore(string planid)
        {
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            DataTable dt4 = new DataTable();
            dt2.Columns.Add("id", typeof(string));//���½�id
            dt2.Columns.Add("MajorNumber", typeof(string));//���½�Ҫ�غ�
            dt2.Columns.Add("ChaptersName", typeof(string));//���½���Ŀ����
            dt2.Columns.Add("nosuidCount", typeof(string));//������������
            dt2.Columns.Add("gradeCount", typeof(string));//����������
            dt2.Columns.Add("kCount", typeof(string));//�۷�������
            dt2.Columns.Add("score", typeof(string));//��׼��
            dt2.Columns.Add("zScore", typeof(string));//���յ÷�
            dt2.Columns.Add("yScore", typeof(string));//ʵ�÷�=��׼�÷�-�ܿ۷�-��������÷�
            dt2.Columns.Add("scoreRate", typeof(string));//�÷���=ʵ�÷�/��׼��
            dt2.Columns.Add("nosuidScore", typeof(string));//���������
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
                //  //�ܵ÷�=��׼�÷�-�ܿ۷�-��������÷�
                //���յ÷�=�ܵ÷�/����׼�ܷ�-��������֣���
                int sumScore = Convert.ToInt32(dt.Rows[i][1].ToString()) - Convert.ToInt32(dt4.Rows[0][0].ToString()) - Convert.ToInt32(dt3.Rows[0][0].ToString());
                //�Ա����������Ϊ0�����´���
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
                if (type == "1")//���д���
                {
                    dt2 = this.BaseRepository().FindTable(string.Format("select DutyName from bis_assessmentsum where chapterid='{0}' and assessmentplanid='{1}'", dt.Rows[i][0], planid));
                    dutyname += dt2.Rows[0][0].ToString() + ",";
                }
                if (type == "2")//��7��͵�10��
                {
                    if (i == 6 || i == 9)
                    {
                        dt2 = this.BaseRepository().FindTable(string.Format("select DutyName from bis_assessmentsum where chapterid='{0}' and assessmentplanid='{1}'", dt.Rows[i][0], planid));
                        dutyname += dt2.Rows[0][0].ToString() + ",";
                    }
                }
                if (type == "3")//������
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
        /// �üƻ�ÿ����۷ֺͣ���������,����͵�ʮ��֮�⣩
        /// </summary>
        /// <returns></returns>
        public int GetEverySumScore(string planid)
        {
            int num = this.BaseRepository().FindObject(string.Format(@"select nvl(sum(kscore),0) as sumscore from bis_kscoredetail where chapterid not in(select id from bis_assessmentchapters where ChaptersParentID in (select id from bis_assessmentchapters where ChaptersParentID='-1' and majornumber='5.6' or majornumber='5.7' or majornumber='5.10') and content is not null)
and assessmentplanid='{0}'", planid)).ToInt();
            return num;
        }


        /// <summary>
        /// �üƻ�ÿ����۷�ֵ�Ϳ۷�ԭ�򣨳�������,����͵�ʮ��֮�⣩
        /// </summary>
        /// <returns></returns>
        public string GetEveryResonAndScore(string planid)
        {
            DataTable dt = new DataTable();
            dt = this.BaseRepository().FindTable(string.Format(@"select kScoreReason,kscore  from bis_kscoredetail where chapterid not in(select id from bis_assessmentchapters where ChaptersParentID in (select id from bis_assessmentchapters where ChaptersParentID='-1' and majornumber='5.6' or majornumber='5.7' or majornumber='5.10') and content is not null)
and assessmentplanid='{0}'", planid));
            string s = "��Ҫ�۷ֵ�:";
            string m = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                m += dt.Rows[i][0].ToString() + "; ��" + dt.Rows[i][1].ToString() + "��; ";
            }
            if (string.IsNullOrEmpty(m))
                return "";
            else
                return s + m;
        }

        /// <summary>
        /// �üƻ�������͵�ʮ��۷ֺ�
        /// </summary>
        /// <returns></returns>
        public int GetEverySumScore2(string planid)
        {
            int num = this.BaseRepository().FindObject(string.Format(@"select nvl(sum(kscore),0) as sumscore from bis_kscoredetail where chapterid  in(select id from bis_assessmentchapters where ChaptersParentID in (select id from bis_assessmentchapters where ChaptersParentID='-1' and  majornumber='5.7' or majornumber='5.10') and content is not null)
and assessmentplanid='{0}'", planid)).ToInt();
            return num;
        }


        /// <summary>
        /// �üƻ�������͵�ʮ��۷�ֵ�Ϳ۷�ԭ��
        /// </summary>
        /// <returns></returns>
        public string GetEveryResonAndScore2(string planid)
        {
            DataTable dt = new DataTable();
            dt = this.BaseRepository().FindTable(string.Format(@"select kScoreReason,kscore  from bis_kscoredetail where chapterid  in(select id from bis_assessmentchapters where ChaptersParentID in (select id from bis_assessmentchapters where ChaptersParentID='-1' and  majornumber='5.7' or majornumber='5.10') and content is not null)
and assessmentplanid='{0}'", planid));
            string s = "��Ҫ�۷ֵ�:";
            string m = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                m += dt.Rows[i][0].ToString() + "; ��" + dt.Rows[i][1].ToString() + "��; ";
            }
            if (string.IsNullOrEmpty(m))
                return "";
            else
                return s + m;
        }


        /// <summary>
        /// �üƻ�����Ԫ�ؿ۷ֺ�
        /// </summary>
        /// <returns></returns>
        public int GetEverySumScore3(string planid, string strMarjor)
        {
            int num = this.BaseRepository().FindObject(string.Format(@"select nvl(sum(kscore),0) as sumscore from bis_kscoredetail where chapterid  in(select id from bis_assessmentchapters where majornumber in({1}))
and assessmentplanid='{0}'", planid, strMarjor)).ToInt();
            return num;
        }


        /// <summary>
        /// �üƻ�����Ԫ�ؿ۷�ֵ�Ϳ۷�ԭ��
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="strMarjor"></param>
        /// <returns></returns>
        public string GetEveryResonAndScore3(string planid, string strMarjor)
        {
            DataTable dt = new DataTable();
            dt = this.BaseRepository().FindTable(string.Format(@"select kScoreReason,kscore  from bis_kscoredetail where chapterid  in(select id from bis_assessmentchapters where majornumber in({1}))
and assessmentplanid='{0}'", planid, strMarjor));
            string s = "��Ҫ�۷ֵ�:";
            string m = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                m += dt.Rows[i][0].ToString() + "; ��" + dt.Rows[i][1].ToString() + "��; ";
            }
            if (string.IsNullOrEmpty(m))
                return "";
            else
                return s + m;
        }

        /// <summary>
        /// ����һ��ͳ��
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
        /// ��ȡ���ڵ���½ڵ����б�׼��
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

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
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
        /// ���������ƻ�idɾ������
        /// </summary>
        /// <param name="planId">�����ƻ�id</param>
        public int Remove(string planId)
        {
            return this.BaseRepository().ExecuteBySql(string.Format("delete from bis_assessmentsum where AssessmentPlanID='{0}'", planId));
        }
        #endregion
    }
}
