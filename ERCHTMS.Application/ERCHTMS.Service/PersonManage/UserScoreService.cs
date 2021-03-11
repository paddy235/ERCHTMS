using ERCHTMS.Entity.PersonManage;
using ERCHTMS.IService.PersonManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
namespace ERCHTMS.Service.PersonManage
{
    /// <summary>
    /// �� ������Ա����
    /// </summary>
    public class UserScoreService : RepositoryFactory<UserScoreEntity>, UserScoreIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<UserScoreEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return this.BaseRepository().FindList(pagination);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public  DataTable GetList(string userId)
        {
            return this.BaseRepository().FindTable(string.Format("select s.itemname,t.createdate as time,itemtype,t.score from  BIS_USERSCORE t left join base_user u on t.userid=u.userid left join bis_scoreset s on t.itemid=s.id where t.userid='{0}'",userId));
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public UserScoreEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// ��ȡ��Ա����׻��ֺ��ۼƻ���
        /// </summary>
        /// <param name="userId">�û�Id</param>
        /// <returns></returns>
        public string GetScoreInfo(string userId)
        {
            int score = 100;//select t.itemvalue from BASE_DATAITEMDETAIL t where t.itemdetailid=(select * from base_user u where u.userid='{0}')
            DataTable dt = BaseRepository().FindTable(string.Format("select t.itemvalue from BASE_DATAITEMDETAIL t where t.itemdetailid=(select organizeid from base_user u where u.userid='{0}')", userId));
            if(dt.Rows.Count>0)
            {
                score=dt.Rows[0][0].ToInt();
            }
            else
            {
                dt = BaseRepository().FindTable(string.Format("select t.itemvalue from BASE_DATAITEMDETAIL t where t.itemdetailid='{0}'", "csjf"));
                if (dt.Rows.Count > 0)
                {
                    score = dt.Rows[0][0].ToInt();
                }
            }
            string sql = string.Format("(select nvl(sum(score),0) from BIS_USERSCORE t where year='{1}' and userid='{0}') union all  (select nvl(sum(score),0) from BIS_USERSCORE t where userid='{0}')",userId,System.DateTime.Now.Year);
            dt = BaseRepository().FindTable(sql);
            int currScore=score + int.Parse(dt.Rows[0][0].ToString());
            score += int.Parse(dt.Rows[1][0].ToString());
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { currScore = currScore, sumScore = score });
        }
        /// <summary>
        /// ��ȡ��Ա���ֿ�����ϸ
        /// </summary>
        /// <param name="keyValue">��¼Id</param>
        /// <returns></returns>
        public object GetInfo(string keyValue)
        {
            DataTable dt = this.BaseRepository().FindTable(string.Format("select t.score,t.userid,s.itemname,s.itemtype,u.realname,u.usertype,u.identifyid,t.itemid  from BIS_USERSCORE t left join bis_scoreset s on t.itemid=s.id left join base_user u on t.userid=u.userid where t.id='{0}'",keyValue));

            return new { UserId = dt.Rows[0]["userid"].ToString(), UserName = dt.Rows[0]["realname"].ToString(), Score = dt.Rows[0]["score"].ToString(), ItemId = dt.Rows[0]["itemid"].ToString(), ItemName = dt.Rows[0]["itemname"].ToString(), ItemType = dt.Rows[0]["itemtype"].ToString(), UserType = dt.Rows[0]["usertype"].ToString(), IdCard = dt.Rows[0]["identifyid"].ToString() };
        }
        /// <summary>
        /// ��ȡ�û�ָ����ݵĻ���
        /// </summary>
        /// <param name="userId">�û�Id</param>
        /// <param name="year">���</param>
        /// <returns></returns>
        public decimal GetUserScore(string userId,string year)
        {
            object obj = BaseRepository().FindObject(string.Format("select sum(score) from bis_userscore where userid='{0}' and year='{1}'",userId,year));
            return obj == null || obj == System.DBNull.Value ? 0 : obj.ToDecimal();

        }
        /// <summary>
        /// �洢���̷�ҳ��ѯ
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetPageJsonList(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;
            if (!string.IsNullOrEmpty(queryJson))
            {
                 var queryParam = queryJson.ToJObject();
                 if (!queryParam["userId"].IsEmpty())
                 {
                     string userId = queryParam["userId"].ToString().Trim();
                     pagination.conditionJson += string.Format(" and u.userId='{0}'", userId);
                 }
            //����ʱ��
            if (!queryParam["startDate"].IsEmpty() && !queryParam["endDate"].IsEmpty())
            {
                 string startDate = queryParam["startDate"].ToString().Trim();
                 string endDate = queryParam["endDate"].ToString().Trim();
                 pagination.conditionJson += string.Format(" and t.createdate between to_date('{0}','yyyy-mm-dd hh24:mi:ss') and to_date('{1} 23:59:59','yyyy-mm-dd hh24:mi:ss')", startDate,endDate);
            }
            //��������
            if (!queryParam["itemType"].IsEmpty())
            {
                string itemType = queryParam["itemType"].ToString().Trim();
                pagination.conditionJson += string.Format(" and itemType='{0}'", itemType);
            }
            //��ѯ���
            if (!queryParam["year"].IsEmpty())
            {
                string year = queryParam["year"].ToString().Trim();
                pagination.conditionJson += string.Format(" and year='{0}'", year);
            }
            //���ռ���
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyword = queryParam["keyword"].ToString().Trim();
                pagination.conditionJson += string.Format(" and (realname like '%{0}%' or deptname like '%{0}%' or itemname  like '%{0}%')", keyword);
            }
            }
           
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
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
        public void SaveForm(string keyValue, UserScoreEntity entity)
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
        /// ����������ֿ��˼�¼
        /// </summary>
        /// <param name="list"></param>
        public void Save(List<UserScoreEntity> list)
        {
            this.BaseRepository().Insert(list);
        }
        #endregion
    }
}
