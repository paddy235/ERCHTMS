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
    /// �� ������������ɸѡ
    /// </summary>
    public class NoSuitableDetailService : RepositoryFactory<NoSuitableDetailEntity>, NoSuitableDetailIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<NoSuitableDetailEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public NoSuitableDetailEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }


        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageListJson(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;
            DataTable dtresult = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            foreach (DataRow item in dtresult.Rows)
            {
                var dt = this.BaseRepository().FindTable(string.Format(@"select nvl(sum(score),0) from  bis_assessmentchapters  where  id in(select chapterid from bis_nosuitabledetail  where assessmentplanid='{0}' and createuserid='{1}' )", item[0], user.UserId));
                if (dt.Rows.Count > 0)
                {
                    item["score"] = dt.Rows[0][0].ToString();
                }
                //�ܵ�״̬
                var dt1 = this.BaseRepository().FindTable(string.Format(@"select a,b from (select count(1) a ,assessmentplanid,reserve,sum(count(1)) over() b from bis_assessmentsum where 
                              assessmentplanid='{0}' group by assessmentplanid,reserve)t where reserve='δɸѡ'", item[0]));
                if (dt1.Rows.Count > 0)
                {
                    if (dt1.Rows[0][0].ToString() == dt1.Rows[0][1].ToString())
                        item["Status"] = "δɸѡ";
                    else
                        item["Status"] = "������";
                }
                else
                {
                    item["Status"] = "���ύ";
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
                var dt3 = this.BaseRepository().FindTable(string.Format(@"select count(id) from bis_assessmentsum  where assessmentplanid='{0}' and dutyid='{1}' and reserve='��ɸѡ'", item[0], user.UserId));
                if (dt2.Rows.Count > 0 && dt3.Rows.Count > 0)
                {
                    if (islock == "����")//û������
                    {
                        if (duty == user.UserId)//�Ǹ�����
                        {
                            item["able"] = true;
                            item["isUpdate"] = "ɸѡ";

                        }
                        else//�Ǹ�����
                        {
                            int self = Convert.ToInt32(dt3.Rows[0][0].ToString());//��ɸѡ����
                            int sum = Convert.ToInt32(dt2.Rows[0][0].ToString());//����
                            if (sum != 0)
                            {
                                item["able"] = true;
                                if (self == sum)//�����������͸�������ɸѡ�����
                                    item["isUpdate"] = "�޶�";
                                else
                                    item["isUpdate"] = "ɸѡ";
                            }
                        }
                    }
                    else//�����˸������ƻ�
                    {
                        item["isUpdate"] = "ɸѡ";
                    }
                    item["progress"] = dt3.Rows[0][0].ToString() + "/" + dt2.Rows[0][0].ToString();
                }
            }
            return dtresult;
        }

        /// <summary>
        /// ���ݼƻ�id��ȡ����
        /// </summary>
        /// <param name="planid"></param>
        /// <returns></returns>
        public DataTable GetDetailInfo(string planid)
        {
            DataTable dtresult = new DataTable();
            dtresult.Columns.Add("chaptersid");
            dtresult.Columns.Add("chaptersname");//��ڵ�����
            dtresult.Columns.Add("nosuitnum");//������������
            dtresult.Columns.Add("isclick");//�Ƿ��ܵ��(1.��,2:��)
            int duty = 0;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            duty = this.BaseRepository().FindObject(string.Format(@" select count(id) from bis_assessmentplan where id='{0}' and teamleader='{1}'", planid, user.UserId)).ToInt();
            //��ȡ��ڵ�
            var dt = this.BaseRepository().FindTable("select ID,MajorNumber,ChaptersName from bis_assessmentchapters where ChaptersParentID='-1' order by cast(replace(majornumber,'.','') as number)");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dtresult.NewRow();
                var num = this.BaseRepository().FindObject(string.Format(@"select count(id) from bis_nosuitabledetail where assessmentplanid='{0}' and chapterid in(select id from bis_assessmentchapters where ChaptersParentID='{1}')", planid, dt.Rows[i]["ID"].ToString())).ToInt();
                if (duty > 0)//�������鳤
                {
                    row["isclick"] = 1;
                }
                else//���������鳤
                {
                    //�������Ƿ��ǵ�ǰ��¼������
                    var dutynum = this.BaseRepository().FindObject(string.Format(@"select count(id) from bis_assessmentsum where chapterid='{0}' and assessmentplanid='{1}' and dutyid='{2}'", dt.Rows[i]["ID"].ToString(), planid, user.UserId)).ToInt();
                    row["isclick"] = dutynum > 0 ? "1" : "2";
                }
                row["chaptersname"] = dt.Rows[i]["MajorNumber"] + dt.Rows[i]["ChaptersName"].ToString();//���½�����
                row["nosuitnum"] = num;
                row["chaptersid"] = dt.Rows[i]["ID"].ToString();
                dtresult.Rows.Add(row);
            }
            return dtresult;
        }


        /// <summary>
        ///���������С���б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetAllDetailPage(Pagination pagination)
        {
            DatabaseType dataType = DbHelper.DbType;
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }


        /// <summary>
        /// ���ݼƻ�id��С��ڵ�id��ȡ������������
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="chapterid"></param>
        /// <returns></returns>
        public NoSuitableDetailEntity GetNoSuitByPlanOrChapID(string planid, string chapterid)
        {
            var expression = LinqExtensions.True<NoSuitableDetailEntity>();
            if (!string.IsNullOrEmpty(planid))
            {
                expression = expression.And(t => t.AssessmentPlanID == planid);
            }
            if (!string.IsNullOrEmpty(chapterid))
            {
                expression = expression.And(t => t.ChapterID==chapterid);
            }
            return this.BaseRepository().IQueryable(expression).ToList().FirstOrDefault();
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
        public void SaveForm(string keyValue, NoSuitableDetailEntity entity)
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
