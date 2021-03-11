using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.IService.RiskDatabase;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.OutsourcingProject
{
    /// <summary>
    /// 描 述：安全考核标标准分类
    /// </summary>
    public class SafestandardService : RepositoryFactory<SafestandardEntity>, SafestandardIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SafestandardEntity> GetList(string queryJson)
        {
            string sql = "select * from EPG_SAFESTANDARD where 1=1 ";
            if (!queryJson.IsEmpty())
                sql += queryJson;

            return this.BaseRepository().FindList(sql).ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SafestandardEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 判断节点下有无子节点数据
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public bool IsHasChild(string parentId)
        {
            return this.BaseRepository().FindObject(string.Format("select count(1) from EPG_SAFESTANDARD where parentid='{0}'", parentId)).ToInt() > 0 ? true : false;
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
            if (entity != null)
            {
                string code = entity.ENCODE;
                if (this.BaseRepository().Delete(keyValue) > 0)
                {
                    this.BaseRepository().ExecuteBySql(string.Format("delete from EPG_SAFESTANDARD where encode like '{0}%'", code));
                    this.BaseRepository().ExecuteBySql(string.Format("delete from EPG_SAFESTANDARDITEM   where stcode like '{0}%'", code));
                }
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, SafestandardEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                SafestandardEntity hs = BaseRepository().FindEntity(keyValue);
                if (hs == null)
                {

                    var node = this.BaseRepository().FindEntity(entity.PARENTID);
                    string enCode = node == null ? "" : node.ENCODE;
                    int count = BaseRepository().FindObject(string.Format("select count(1) from EPG_SAFESTANDARD where parentid='{0}'", entity.PARENTID)).ToInt();
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
                    entity.ID = keyValue;
                    entity.ENCODE = enCode;
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
                var node = this.BaseRepository().FindEntity(entity.PARENTID);
                string enCode = node == null ? "" : node.ENCODE;
                int count = BaseRepository().FindObject(string.Format("select count(1) from EPG_SAFESTANDARD where parentid='{0}'", entity.PARENTID)).ToInt();
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
                entity.ENCODE = enCode;
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
        public string Save(string one, string two, string three, string four, string five, string content, string require, string norm)
        {
            DataTable dt = BaseRepository().FindTable(string.Format("select id,encode from EPG_SAFESTANDARD where name='{0}' and parentid='0' and createuserorgcode='{1}'", one, ERCHTMS.Code.OperatorProvider.Provider.Current().OrganizeCode));
            string enCode = "";
            string parentId = "";
            SafestandardEntity entity = new SafestandardEntity();
            if (dt.Rows.Count > 0)
            {
                enCode = dt.Rows[0][1].ToString();
                parentId = dt.Rows[0][0].ToString();
                entity = BaseRepository().FindEntity(parentId);
                if (!string.IsNullOrEmpty(two))
                {
                    object obj = BaseRepository().FindObject(string.Format("select id from EPG_SAFESTANDARD where name='{0}' and parentid='{1}'", two, parentId));
                    if (obj == null)
                    {
                        int count = BaseRepository().FindObject(string.Format("select count(1) from EPG_SAFESTANDARD where parentid='{0}'", parentId)).ToInt();
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
                        entity = new SafestandardEntity
                        {
                            ID = System.Guid.NewGuid().ToString(),
                            ENCODE = enCode,
                            NAME = two,
                            PARENTID = parentId,
                            LEV = 2
                        };
                        entity.Create();
                        this.BaseRepository().Insert(entity);
                        parentId = entity.ID;
                    }
                    else
                    {
                        parentId = obj.ToString();
                        entity = this.BaseRepository().FindEntity(parentId);
                        if (!string.IsNullOrEmpty(three))
                        {
                            obj = BaseRepository().FindObject(string.Format("select id from EPG_SAFESTANDARD where name='{0}' and parentid='{1}'", three, parentId));
                            if (obj == null)
                            {
                                int count = BaseRepository().FindObject(string.Format("select count(1) from EPG_SAFESTANDARD where parentid='{0}'", parentId)).ToInt();
                                count++;
                                enCode = entity.ENCODE;
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
                                entity = new SafestandardEntity
                                {
                                    ID = System.Guid.NewGuid().ToString(),
                                    ENCODE = enCode,
                                    NAME = three,
                                    PARENTID = parentId,
                                    LEV = 3
                                };
                                entity.Create();
                                this.BaseRepository().Insert(entity);
                                parentId = entity.ID;
                            }
                            else
                            {
                                parentId = obj.ToString();
                                entity = this.BaseRepository().FindEntity(parentId);
                            }
                            if (!string.IsNullOrEmpty(four))
                            {
                                obj = BaseRepository().FindObject(string.Format("select id from EPG_SAFESTANDARD where name='{0}' and parentid='{1}'", four, parentId));
                                if (obj == null)
                                {
                                    enCode = entity.ENCODE;
                                    int count = BaseRepository().FindObject(string.Format("select count(1) from EPG_SAFESTANDARD where parentid='{0}'", parentId)).ToInt();
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
                                    entity = new SafestandardEntity
                                    {
                                        ID = System.Guid.NewGuid().ToString(),
                                        ENCODE = enCode,
                                        NAME = four,
                                        PARENTID = parentId,
                                        LEV = 4
                                    };
                                    entity.Create();
                                    this.BaseRepository().Insert(entity);
                                    parentId = entity.ID;
                                }
                                else
                                {
                                    parentId = obj.ToString();
                                    entity = this.BaseRepository().FindEntity(parentId);
                                }
                                if (!string.IsNullOrEmpty(five))
                                {
                                    obj = BaseRepository().FindObject(string.Format("select id from EPG_SAFESTANDARD where name='{0}' and parentid='{1}'", five, parentId));
                                    if (obj == null)
                                    {
                                        enCode = entity.ENCODE;
                                        int count = BaseRepository().FindObject(string.Format("select count(1) from EPG_SAFESTANDARD where parentid='{0}'", parentId)).ToInt();
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
                                        entity = new SafestandardEntity
                                        {
                                            ID = System.Guid.NewGuid().ToString(),
                                            ENCODE = enCode,
                                            NAME = five,
                                            PARENTID = parentId,
                                            LEV = 5
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

                int count = BaseRepository().FindObject(string.Format("select count(1) from EPG_SAFESTANDARD where parentid='0'")).ToInt();
                if (count.ToString().Length < 2)
                {
                    enCode = "00" + count;
                }
                else if (count.ToString().Length >= 2 && count.ToString().Length < 3)
                {
                    enCode = "0" + count;
                }
                else
                {
                    enCode = count.ToString();
                }
                entity = new SafestandardEntity
                {
                    ID = System.Guid.NewGuid().ToString(),
                    ENCODE = enCode,
                    NAME = one,
                    PARENTID = "0",
                    LEV = 1
                };
                entity.Create();
                parentId = entity.ID;
                if (this.BaseRepository().Insert(entity) > 0)
                {
                    if (!string.IsNullOrEmpty(two))
                    {
                        entity = new SafestandardEntity
                        {
                            ID = System.Guid.NewGuid().ToString(),
                            ENCODE = entity.ENCODE + "001",
                            NAME = two,
                            PARENTID = entity.ID,
                            LEV = 2
                        };
                        entity.Create();
                        parentId = entity.ID;
                        if (this.BaseRepository().Insert(entity) > 0)
                        {
                            if (!string.IsNullOrEmpty(three))
                            {
                                entity = new SafestandardEntity
                                {
                                    ID = System.Guid.NewGuid().ToString(),
                                    ENCODE = entity.ENCODE + "001",
                                    NAME = three,
                                    PARENTID = entity.ID,
                                    LEV = 3
                                };
                                entity.Create();
                                parentId = entity.ID;
                                if (this.BaseRepository().Insert(entity) > 0)
                                {
                                    if (!string.IsNullOrEmpty(four))
                                    {
                                        entity = new SafestandardEntity
                                        {
                                            ID = System.Guid.NewGuid().ToString(),
                                            ENCODE = entity.ENCODE + "001",
                                            NAME = four,
                                            PARENTID = entity.ID,
                                            LEV = 4
                                        };
                                        entity.Create();
                                        parentId = entity.ID;
                                        if (this.BaseRepository().Insert(entity) > 0)
                                        {
                                            if (!string.IsNullOrEmpty(five))
                                            {
                                                entity = new SafestandardEntity
                                                {
                                                    ID = System.Guid.NewGuid().ToString(),
                                                    ENCODE = entity.ENCODE + "001",
                                                    NAME = five,
                                                    PARENTID = entity.ID,
                                                    LEV = 5
                                                };
                                                entity.Create();
                                                parentId = entity.ID;
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
            SafestandarditemEntity item = new SafestandarditemEntity
            {
                STID = entity.ID,
                STCODE = entity.ENCODE,
                CONTENT = content.Trim(),
                REQUIRE = require.Trim(),
                NORM = norm.Trim().Replace("？", "")
            };
            item.Create();
            new RepositoryFactory<SafestandarditemEntity>().BaseRepository().Insert(item);
            return parentId;
        }
        #endregion
    }
}
