using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.IService.RiskDatabase;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Util.Extension;
using System.Data.Common;
using BSFramework.Data;
using System.Data;
using System.Linq;
using System.Text;
using ERCHTMS.Code;

namespace ERCHTMS.Service.RiskDatabase
{
    /// <summary>
    /// 描 述：风险管控措施表
    /// </summary>
    public class MeasuresService : RepositoryFactory<MeasuresEntity>, MeasuresIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<MeasuresEntity> GetList(string queryJson)
        {
            return this.BaseRepository().FindList(string.Format("select * from bis_measures where 1=1 " + queryJson)).ToList();
        }
        /// <summary>
        ///根据区域获取列表
        /// </summary>
        /// <param name="areaId">区域Id</param>
        /// <param name="riskId">风险库记录ID</param>
        /// <returns></returns>
        public IEnumerable<MeasuresEntity> GetList(string areaId, string riskId)
        {
            var expression = LinqExtensions.True<MeasuresEntity>();
            if (!string.IsNullOrEmpty(areaId))
            {
                expression = expression.And(t => t.AreaId == areaId);
            }
            if (!string.IsNullOrEmpty(riskId))
            {
                expression = expression.And(t => t.RiskId == riskId);
            }
            return this.BaseRepository().IQueryable(expression).OrderBy(t => t.CreateDate).ToList();
        }
        /// <summary>
        /// 获取管控措施
        /// </summary>
        /// <param name="riskId">风险记录Id</param>
        /// <param name="typeName">管控措施类别</param>
        /// <returns></returns>
        public DataTable GetDTList(string riskId, string typeName)
        {
            return this.BaseRepository().FindTable(string.Format("select id,content,riskid,1 as status from bis_measures where riskid='{0}' and typename='{1}'", riskId, typeName));
        }
        public string GetMeasures(string riskId,string typeName="")
        {
            int j = 1;
            StringBuilder sb = new StringBuilder();
            string sql = string.Format("select content from bis_measures where riskid='{0}'", riskId);
            if (!string.IsNullOrWhiteSpace(typeName))
            {
                sql += " and typename='"+typeName+"'";
            }
            DataTable dt = this.BaseRepository().FindTable(sql);
            if (dt.Rows.Count == 1)
            {
                sb.Append(dt.Rows[0][0].ToString());
            }
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (!string.IsNullOrWhiteSpace(typeName))
                    {
                        sb.AppendFormat("{0}.{1};", j, dr[0].ToString().Trim(';'));
                    }
                    else
                    {
                        sb.AppendFormat("{0}.{1};\r\n", j, dr[0].ToString());
                    }
                    j++;
                }
            }
            dt.Dispose();
            return sb.ToString();
        }

        public DataTable GetMeasuresDetail(string worktask, string areaid)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DataTable rDt = new DataTable();
            rDt.Columns.Add("riskdesc", typeof(string));
            rDt.Columns.Add("content", typeof(string));
            DataTable dt = this.BaseRepository().FindTable(string.Format(@"select distinct(riskdesc),Max(id) id ,Max(dangersource) dangersource
                                                                                    from bis_riskassess 
                                                                                    where worktask='{0}' 
                                                                                    and createuserorgcode='{1}'  
                                                                                    and districtid='{2}'
                                                                                      and status=1 
                                                                                    and deletemark=0 
                                                                                    group by riskdesc", worktask, user.OrganizeCode, areaid));
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row = rDt.NewRow();
                    row["riskdesc"] = dt.Rows[i]["dangersource"].ToString() + dt.Rows[i]["riskdesc"].ToString();
                    DataTable dt1 = this.BaseRepository().FindTable(string.Format("select content from bis_measures where riskid='{0}'", dt.Rows[i]["id"].ToString()));
                    StringBuilder sb = new StringBuilder();
                    int j = 1;
                    if (dt1.Rows.Count == 1)
                    {
                        sb.Append(dt1.Rows[0][0].ToString());
                    }
                    else
                    {
                        foreach (DataRow dr in dt1.Rows)
                        {
                            sb.AppendFormat("{0}.{1};\r\n", j, dr[0].ToString());
                            j++;
                        }
                    }
                    row["content"] = sb.ToString();
                    rDt.Rows.Add(row);
                }
            }
            return rDt;

        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public MeasuresEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 根据区域id获取相关联的区域
        /// </summary>
        /// <param name="Areaid"></param>
        /// <returns></returns>
        public DataTable GetLinkAreaById(string Areaid)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DataTable dt = this.BaseRepository().FindTable(string.Format(@"select b.districtid, b.districtname
                                                                          from BIS_DISTRICT b
                                                                         where b.linktocompanyid like '%{0}%' and b.organizeid='{1}'
                                                                            ", Areaid, user.OrganizeId));
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
        /// 删除数据
        /// </summary>
        /// <param name="riskId">风险库记录Id</param>
        public int Remove(string riskId)
        {
            //DbParameter[] parameter = 
            //{
            //      DbParameters.CreateDbParameter("@RiskId", riskId)
            //};
            return this.BaseRepository().ExecuteBySql(string.Format("delete from BIS_MEASURES where riskid='{0}'", riskId));
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="list">MeasuresEntity集合对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, List<MeasuresEntity> list)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                foreach (MeasuresEntity entity in list)
                {
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }
            }
            else
            {
                foreach (MeasuresEntity entity in list)
                {
                    entity.Create();
                    this.BaseRepository().Insert(entity);
                }
            }
        }
        public void Save(string keyValue, MeasuresEntity entity)
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