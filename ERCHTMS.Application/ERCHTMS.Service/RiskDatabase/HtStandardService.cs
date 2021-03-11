using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.IService.RiskDatabase;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
namespace ERCHTMS.Service.RiskDatabase
{
    /// <summary>
    /// 描 述：隐患标准库
    /// </summary>
    public class HtStandardService : RepositoryFactory<HtStandardEntity>, HtStandardIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<HtStandardEntity> GetList(string queryJson)
        {
            //return this.BaseRepository().IQueryable().ToList();
            string sql = "select * from BIS_HTSTANDARD where 1=1 ";
            if (!queryJson.IsEmpty())
                sql += queryJson;

            return this.BaseRepository().FindList(sql).ToList();
        }
        /// <summary>
        /// 判断节点下有无子节点数据
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public bool IsHasChild(string parentId)
        {
            return this.BaseRepository().FindObject(string.Format("select count(1) from BIS_HTSTANDARD where parentid='{0}'",parentId)).ToInt() > 0 ? true : false;
        }
        /// <summary>
        /// 列表分页
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //上级节点ID
            if (!queryParam["parentId"].IsEmpty())
            {
                string parentId = queryParam["parentId"].ToString();
                pagination.conditionJson += string.Format(" and parentId = '{0}'", parentId);
            }
          
            //标准编码
            if (!queryParam["enCode"].IsEmpty())
            {
                string enCode = queryParam["enCode"].ToString();
                pagination.conditionJson += string.Format(" and STCODE like '{0}%'", enCode);
            }
            //查询关键字
            if (!queryParam["keyWord"].IsEmpty())
            {
                string keyWord = queryParam["keyWord"].ToString();
                pagination.conditionJson += string.Format(" and (content like '%{0}%' or require like '%{0}%')", keyWord.Trim());
            }
            //如果是来自安全检查中的选择
            if (!queryParam["stIds"].IsEmpty())
            {
                string ids = queryParam["stIds"].ToString().Replace("[", "").Replace("]", "").Replace("\r\n", "").Replace("\"", "'");
                pagination.conditionJson += string.Format(" and stid in({0})", System.Text.RegularExpressions.Regex.Replace(ids, @"\s", ""));
                //if (!queryParam["noIds"].IsEmpty())
                //{
                //    string noIds = queryParam["noIds"].ToString().Replace("[", "").Replace("]", "").Replace("\r\n", "").Replace("\"", "'");
                //    pagination.conditionJson += string.Format(" and stid not in({0})", System.Text.RegularExpressions.Regex.Replace(ids, @"\s", ""));
                //}
                return this.BaseRepository().FindTable(string.Concat("select ", pagination.p_kid, ",", pagination.p_fields, " from ", pagination.p_tablename, " where ", pagination.conditionJson, "order by b.encode asc,name"));
            }
            else
            {
                return this.BaseRepository().FindTableByProcPager(pagination,dataType);
            }
           
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public HtStandardEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 获取检查标准被引用的次数
        /// </summary>
        /// <param name="id">标准项Id</param>
        /// <returns></returns>
        public int GetNumber(string id)
        {
            return BaseRepository().FindObject(string.Format("select count(1) from BIS_SAFTYCHECKDATADETAILED where checkdataid='{0}'",id)).ToInt();
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            var entity = BaseRepository().FindEntity(keyValue);
            if (entity!=null)
            {
                string code = entity.EnCode;
                if (this.BaseRepository().Delete(keyValue) > 0)
                {
                    this.BaseRepository().ExecuteBySql(string.Format("delete from BIS_HTSTANDARD where encode like '{0}%'", code));
                    this.BaseRepository().ExecuteBySql(string.Format("delete from BIS_HTSTANDARDITEM where stcode like '{0}%'", code));
                }
            }
           
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, HtStandardEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                HtStandardEntity hs=BaseRepository().FindEntity(keyValue);
                if (hs==null)
                {
                  
                    var node = this.BaseRepository().FindEntity(entity.Parentid);
                    string enCode = node == null ? "" : node.EnCode;
                    int count = BaseRepository().FindObject(string.Format("select count(1) from BIS_HTSTANDARD where parentid='{0}'", entity.Parentid)).ToInt();
                    count++;
                    if (count.ToString().Length < 2)
                    {
                        enCode += "00" + count;
                    }
                    else if (count.ToString().Length >= 2 && count.ToString().Length < 3)
                    {
                        enCode += "0" + count;
                    }
                    else
                    {
                        enCode += count.ToString();
                    }
                    entity.Id = keyValue;
                    entity.EnCode = enCode;
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
                var node = this.BaseRepository().FindEntity(entity.Parentid);
                string enCode = node==null?"":node.EnCode;
                int count = BaseRepository().FindObject(string.Format("select count(1) from BIS_HTSTANDARD where parentid='{0}'", entity.Parentid)).ToInt();
                count++;
                if (count.ToString().Length < 2)
                {
                    enCode += "00" + count;
                }
                else if (count.ToString().Length >= 2 && count.ToString().Length < 3)
                {
                    enCode += "0" + count;
                }
                else
                {
                    enCode += count.ToString();
                }
                entity.EnCode = enCode;
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }
        /// <summary>
        /// 导入隐患标准库
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <param name="three"></param>
        /// <param name="four"></param>
        /// <param name="five"></param>
        /// <param name="content"></param>
        /// <param name="require"></param>
        /// <param name="norm"></param>
        /// <returns></returns>
        public string Save(string one,string two,string three,string four,string five,string content,string require,string norm)  
        {
            DataTable dt = BaseRepository().FindTable(string.Format("select id,encode from BIS_HTSTANDARD where name='{0}' and parentid='0' and createuserorgcode='{1}'", one,ERCHTMS.Code.OperatorProvider.Provider.Current().OrganizeCode));
            string enCode ="";
            string parentId = "";
            HtStandardEntity entity = new HtStandardEntity();
            if(dt.Rows.Count>0)
            {
                enCode = dt.Rows[0][1].ToString();
                parentId=dt.Rows[0][0].ToString();
                entity = BaseRepository().FindEntity(parentId);
                if(!string.IsNullOrEmpty(two))
                {
                    object obj = BaseRepository().FindObject(string.Format("select id from BIS_HTSTANDARD where name='{0}' and parentid='{1}'", two, parentId));
                    if (obj == null)
                    {
                        int count = BaseRepository().FindObject(string.Format("select count(1) from BIS_HTSTANDARD where parentid='{0}'", parentId)).ToInt();
                        count++;
                        if (count.ToString().Length < 2)
                        {
                            enCode += "00" + count;
                        }
                        else if (count.ToString().Length >= 2 && count.ToString().Length < 3)
                        {
                            enCode += "0" + count;
                        }
                        else
                        {
                            enCode += count.ToString();
                        }
                        entity = new HtStandardEntity
                        {
                            Id = System.Guid.NewGuid().ToString(),
                            EnCode = enCode,
                            Name = two,
                            Parentid = parentId,
                            lev = 2
                        };
                        entity.Create();
                        this.BaseRepository().Insert(entity);
                        parentId = entity.Id;
                    }
                    else
                    {
                        parentId = obj.ToString();
                        entity = this.BaseRepository().FindEntity(parentId);
                        if(!string.IsNullOrEmpty(three))
                        {
                            obj=BaseRepository().FindObject(string.Format("select id from BIS_HTSTANDARD where name='{0}' and parentid='{1}'", three, parentId));
                            if (obj==null)
                            {
                                int count = BaseRepository().FindObject(string.Format("select count(1) from BIS_HTSTANDARD where parentid='{0}'", parentId)).ToInt();
                                count++;
                                enCode = entity.EnCode;
                                if (count.ToString().Length < 2)
                                {
                                    enCode += "00" + count;
                                }
                                else if (count.ToString().Length >= 2 && count.ToString().Length < 3)
                                {
                                    enCode += "0" + count;
                                }
                                else
                                {
                                    enCode += count.ToString();
                                }
                                entity = new HtStandardEntity
                                {
                                    Id = System.Guid.NewGuid().ToString(),
                                    EnCode = enCode,
                                    Name = three,
                                    Parentid = parentId,
                                    lev = 3
                                };
                                entity.Create();
                                this.BaseRepository().Insert(entity);
                                parentId = entity.Id;
                            }
                            else
                            {
                                parentId = obj.ToString();
                                entity = this.BaseRepository().FindEntity(parentId);
                            }
                            if (!string.IsNullOrEmpty(four))
                            {
                                obj = BaseRepository().FindObject(string.Format("select id from BIS_HTSTANDARD where name='{0}' and parentid='{1}'", four, parentId));
                                if (obj == null)
                                {
                                    enCode = entity.EnCode;
                                    int count = BaseRepository().FindObject(string.Format("select count(1) from BIS_HTSTANDARD where parentid='{0}'", parentId)).ToInt();
                                    count++;
                                    if (count.ToString().Length < 2)
                                    {
                                        enCode += "00" + count;
                                    }
                                    else if (count.ToString().Length >= 2 && count.ToString().Length < 3)
                                    {
                                        enCode += "0" + count;
                                    }
                                    else
                                    {
                                        enCode += count.ToString();
                                    }
                                    entity = new HtStandardEntity
                                    {
                                        Id = System.Guid.NewGuid().ToString(),
                                        EnCode = enCode,
                                        Name = four,
                                        Parentid = parentId,
                                        lev = 4
                                    };
                                    entity.Create();
                                    this.BaseRepository().Insert(entity);
                                    parentId = entity.Id;
                                }
                                else
                                {
                                    parentId = obj.ToString();
                                    entity = this.BaseRepository().FindEntity(parentId);
                                }
                                if (!string.IsNullOrEmpty(five))
                                {
                                    obj = BaseRepository().FindObject(string.Format("select id from BIS_HTSTANDARD where name='{0}' and parentid='{1}'", five, parentId));
                                    if (obj == null)
                                    {
                                        enCode = entity.EnCode;
                                        int count = BaseRepository().FindObject(string.Format("select count(1) from BIS_HTSTANDARD where parentid='{0}'", parentId)).ToInt();
                                        count++;
                                        if (count.ToString().Length < 2)
                                        {
                                            enCode += "00" + count;
                                        }
                                        else if (count.ToString().Length >= 2 && count.ToString().Length < 3)
                                        {
                                            enCode += "0" + count;
                                        }
                                        else
                                        {
                                            enCode += count.ToString();
                                        }
                                        entity = new HtStandardEntity
                                        {
                                            Id = System.Guid.NewGuid().ToString(),
                                            EnCode = enCode,
                                            Name = five,
                                            Parentid = parentId,
                                            lev = 5
                                        };
                                        entity.Create();
                                        this.BaseRepository().Insert(entity);
                                    }
                                    else
                                    {
                                        parentId = obj.ToString();
                                        entity = this.BaseRepository().FindEntity(parentId);
                                    }
                                    

                                }

                            }
                        }
                    }
                }
            }
            else
            {
              
                int count = BaseRepository().FindObject(string.Format("select count(1) from BIS_HTSTANDARD where parentid='0'")).ToInt();
                if (count.ToString().Length<2)
                {
                    enCode = "00"+count;
                }
                else if (count.ToString().Length >= 2 && count.ToString().Length<3)
                {
                    enCode = "0" + count;
                }
                else
                {
                    enCode =  count.ToString();
                }
                entity = new HtStandardEntity { 
                  Id=System.Guid.NewGuid().ToString(),
                  EnCode=enCode,
                  Name=one,
                  Parentid="0",
                  lev=1
                };
                entity.Create();
                parentId = entity.Id;
                if(this.BaseRepository().Insert(entity)>0)
                {
                    if(!string.IsNullOrEmpty(two))
                    {
                        entity = new HtStandardEntity
                        {
                            Id = System.Guid.NewGuid().ToString(),
                            EnCode = entity.EnCode + "001",
                            Name = two,
                            Parentid = entity.Id,
                            lev = 2
                        };
                        entity.Create();
                        parentId = entity.Id;
                        if (this.BaseRepository().Insert(entity) > 0)
                        {
                            if (!string.IsNullOrEmpty(three))
                            {
                                entity = new HtStandardEntity
                                {
                                    Id = System.Guid.NewGuid().ToString(),
                                    EnCode = entity.EnCode + "001",
                                    Name = three,
                                    Parentid = entity.Id,
                                    lev = 3
                                };
                                entity.Create();
                                parentId = entity.Id;
                                if (this.BaseRepository().Insert(entity) > 0)
                                {
                                    if (!string.IsNullOrEmpty(four))
                                    {
                                        entity = new HtStandardEntity
                                        {
                                            Id = System.Guid.NewGuid().ToString(),
                                            EnCode = entity.EnCode + "001",
                                            Name = four,
                                            Parentid = entity.Id,
                                            lev = 4
                                        };
                                        entity.Create();
                                        parentId = entity.Id;
                                        if (this.BaseRepository().Insert(entity) > 0)
                                        {
                                            if (!string.IsNullOrEmpty(five))
                                            {
                                                entity = new HtStandardEntity
                                                {
                                                    Id = System.Guid.NewGuid().ToString(),
                                                    EnCode = entity.EnCode + "001",
                                                    Name = five,
                                                    Parentid = entity.Id,
                                                    lev = 5
                                                };
                                                entity.Create();
                                                parentId = entity.Id;
                                                this.BaseRepository().Insert(entity);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    
                }
            }
            HtStandardItemEntity item = new HtStandardItemEntity
            {
               StId = entity.Id,
               StCode=entity.EnCode,
               Content=content.Trim(),
               Require=require.Trim(),
               Norm = norm.Trim().Replace("？", "")
            };
            item.Create();
            new RepositoryFactory<HtStandardItemEntity>().BaseRepository().Insert(item);
            return parentId;
        }
        #endregion
    }
}
