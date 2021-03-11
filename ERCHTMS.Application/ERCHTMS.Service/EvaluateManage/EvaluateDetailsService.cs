using ERCHTMS.Entity.EvaluateManage;
using ERCHTMS.IService.EvaluateManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using ERCHTMS.Code;
using BSFramework.Data;
using BSFramework.Util;
using System;
using BSFramework.Util.Extension;
using System.Data;
using System.Linq.Expressions;
using ERCHTMS.Entity.StandardSystem;
using System.Text;

namespace ERCHTMS.Service.EvaluateManage
{
    /// <summary>
    /// �� �����Ϲ���������ϸ
    /// </summary>
    public class EvaluateDetailsService : RepositoryFactory<EvaluateDetailsEntity>, EvaluateDetailsIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;
            if (queryJson != null && queryJson != "")
            {
                var queryParam = queryJson.ToJObject();
                //���������ۼƻ�
                if (!queryParam["MainId"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and MainId='{0}'", queryParam["MainId"].ToString());
                }
                //���������ۼƻ�
                if (!queryParam["EvaluatePlanId"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and EvaluatePlanId='{0}'", queryParam["EvaluatePlanId"].ToString());
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<EvaluateDetailsEntity> GetList(string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;
            if (queryJson != null && queryJson != "")
            {
               
                var queryParam = queryJson.ToJObject();
                var expression = LinqExtensions.True<EvaluateDetailsEntity>();
                if (!queryParam["IsConform"].IsEmpty())
                {
                    int IsConform = Convert.ToInt32(queryParam["IsConform"]);
                    expression = expression.And(t => t.IsConform == IsConform);
                }
                if (!queryParam["MainId"].IsEmpty())
                {
                    string MainId = queryParam["MainId"].ToString();
                    expression = expression.And(t => t.MainId == MainId);
                }
                if (!queryParam["EvaluatePlanId"].IsEmpty())
                {
                    string EvaluatePlanId = queryParam["EvaluatePlanId"].ToString();
                    expression = expression.And(t => t.EvaluatePlanId == EvaluatePlanId);
                }

                 return this.BaseRepository().IQueryable(expression).OrderBy(x=>x.CreateDate).ToList();

            }
            else
            {
                return this.BaseRepository().IQueryable().OrderBy(x => x.CreateDate).ToList();
            }
        }

        public DataTable GetStCategory()
        {
            //��ȡ��Ҫ���۵Ĳ���
            DataTable dt = BaseRepository().FindTable("select * from HRS_STCATEGORY t where parentid=(select id from HRS_STCATEGORY where parentid='0' and name like'%���ɷ���%' and rownum<2)");
            return dt;
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public EvaluateDetailsEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
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
        public void SaveForm(string keyValue, EvaluateDetailsEntity entity)
        {
            entity.Id = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                EvaluateDetailsEntity ee = this.BaseRepository().FindEntity(keyValue);
                if (ee == null)
                {
                    entity.Create();
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

        /// <summary>
        /// �����ļ����ƻ�ȡ�������ƣ�ȡ����ࣩ
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string getStCategory(string str)
        {
            try
            {
                Operator curUser = OperatorProvider.Provider.Current();
                DataTable dt = BaseRepository().FindTable(@"select c.name from hrs_stcategory a 
left join hrs_standardsystem b on b.categorycode =a.id 
left join hrs_stcategory c on c.id =a.parentid 
where b.filename='" + str + "'");
                return dt.Rows[0][0].ToString();
            }
            catch
            {
                return "";
            }
        }
    }
}
