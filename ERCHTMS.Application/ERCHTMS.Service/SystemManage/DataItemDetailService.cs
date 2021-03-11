using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ERCHTMS.IService.SystemManage;
using BSFramework.Data.Repository;
using BSFramework.Util.Extension;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ERCHTMS.Code;

namespace ERCHTMS.Service.SystemManage
{
    /// <summary>
    /// 描 述：数据字典明细
    /// </summary>
    public class DataItemDetailService : RepositoryFactory<DataItemDetailEntity>, IDataItemDetailService
    {
        #region 获取数据
        /// <summary>
        /// 明细列表
        /// </summary>
        /// <param name="itemId">分类Id</param>
        /// <returns></returns>
        public IEnumerable<DataItemDetailEntity> GetList(string itemId)
        {
            return this.BaseRepository().IQueryable(t => t.ItemId == itemId).OrderBy(t => t.SortCode).ToList();
        }

        /// <summary>
        /// 明细列表
        /// </summary>
        /// <param name="itemCode">分类Code</param>
        /// <returns></returns>
        public DataTable GetListByCode(string itemCode)
        {
            return this.BaseRepository().FindTable(string.Format("select t.ItemName,t.ItemValue from BASE_DATAITEMDETAIL t where t.enabledmark = 1 and t.deletemark = 0 and  t.itemid=(select itemid from base_dataitem a where a.itemcode='{0}') order by sortcode", itemCode));
            //return this.BaseRepository().IQueryable(t => t.ItemId == itemId).OrderBy(t => t.SortCode).ToList();
        }

        /// <summary>
        /// 明细列表
        /// </summary>
        /// <param name="itemCode">分类Code</param>
        /// <returns></returns>
        public List<DataItemDetailEntity> ListByItemCode(string itemCode)
        {
            //return this.BaseRepository().FindTable(string.Format("select t.ItemName,t.ItemValue from BASE_DATAITEMDETAIL t where t.enabledmark = 1 and t.deletemark = 0 and  t.itemid=(select itemid from base_dataitem a where a.itemcode='{0}') order by sortcode", itemCode));
            //return this.BaseRepository().IQueryable(t => t.ItemId == itemId).OrderBy(t => t.SortCode).ToList();


            var db = DbFactory.Base();
            var linq = from q1 in db.IQueryable<DataItemDetailEntity>()
                       join q2 in db.IQueryable<DataItemEntity>() on q1.ItemId equals q2.ItemId
                       where q2.ItemCode == itemCode
                       select q1;

            return linq.ToList();
        }

        public DataTable GetListByItemCode(string itemCode)
        {
            string[] arr = itemCode.TrimEnd('$').Split('$');
            if (itemCode.Length > 1)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string str in arr)
                {
                    sb.AppendFormat(" itemcode like '%{0}%' or ", str);
                }
                string code = sb.ToString();
                code = code.Substring(0, code.Length - 3);
                return this.BaseRepository().FindTable(string.Format("select t.ItemName,t.ItemValue from BASE_DATAITEMDETAIL t where enabledmark = 1 and ({0}) order by sortcode", code));
            }
            else
            {
                return this.BaseRepository().FindTable(string.Format("select t.ItemName,t.ItemValue from BASE_DATAITEMDETAIL t where itemcode='{0}' order by sortcode", itemCode));
            }
        }
        /// <summary>
        /// 通过编码获取数据实体
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        public DataItemDetailEntity GetListByItemCodeEntity(string itemCode)
        {

            Operator curUser = OperatorProvider.Provider.Current();
            return this.BaseRepository().IQueryable(t => t.ItemCode == itemCode).OrderBy(t => t.SortCode).FirstOrDefault();
        }

        public DataItemDetailEntity GetEntityByItemName(string itemName)
        {
            Operator curUser = OperatorProvider.Provider.Current();
            return this.BaseRepository().IQueryable(t => t.ItemName == itemName).OrderBy(t => t.SortCode).FirstOrDefault();
        }
        /// <summary>
        /// 明细实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public DataItemDetailEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 获取数据字典列表（给绑定下拉框提供的）
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DataItemModel> GetDataItemList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  i.ItemId ,
                                    i.ItemCode AS EnCode ,
                                    d.ItemDetailId ,
                                    d.ParentId ,
                                    d.ItemCode ,
                                    d.ItemName ,
                                    d.ItemValue ,
                                    d.QuickQuery ,
                                    d.SimpleSpelling ,
                                    d.IsDefault ,
                                    d.SortCode ,
                                    d.EnabledMark
                            FROM    Base_DataItemDetail d
                                    LEFT JOIN Base_DataItem i ON i.ItemId = d.ItemId
                            WHERE   1 = 1
                                    AND d.EnabledMark = 1
                                    AND d.DeleteMark = 0
                            ORDER BY d.SortCode ASC");
            return new RepositoryFactory().BaseRepository().FindList<DataItemModel>(strSql.ToString());
        }
        #endregion



        /// <summary>
        /// 通过编码获取对象
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        public IEnumerable<DataItemModel> GetAllDataItemListByItemCode(string itemCode)
        {
            StringBuilder strSql = new StringBuilder();
            if (itemCode.Contains("'"))
            {
                strSql.AppendFormat(@"select  i.itemid,i.itemcode  encode, d.itemdetailid,d.parentid,d.itemcode ,d.itemname ,
                                 d.itemvalue ,d.quickquery ,d.simplespelling ,d.isdefault ,d.sortcode ,d.enabledmark ,d.description
                                 from  base_dataitemdetail d  left join base_dataitem i on i.itemid = d.itemid
                                 where i.itemcode in ({0})
                                order by d.sortcode asc", itemCode);
            }
            else
            {
                strSql.AppendFormat(@"select  i.itemid,i.itemcode  encode, d.itemdetailid,d.parentid,d.itemcode ,d.itemname ,
                                 d.itemvalue ,d.quickquery ,d.simplespelling ,d.isdefault ,d.sortcode ,d.enabledmark ,d.description
                                 from  base_dataitemdetail d  left join base_dataitem i on i.itemid = d.itemid
                                 where  i.itemcode in ('{0}')
                                order by d.sortcode asc", itemCode);
            }

            return new RepositoryFactory().BaseRepository().FindList<DataItemModel>(strSql.ToString());
        }

        /// <summary>
        /// 通过编码获取对象
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        public IEnumerable<DataItemModel> GetDataItemListByItemCode(string itemCode)
        {
            StringBuilder strSql = new StringBuilder();
            if (itemCode.Contains("'"))
            {
                strSql.AppendFormat(@"select  i.itemid,i.itemcode  encode, d.itemdetailid,d.parentid,d.itemcode ,d.itemname ,
                                 d.itemvalue ,d.quickquery ,d.simplespelling ,d.isdefault ,d.sortcode ,d.enabledmark ,d.description
                                 from  base_dataitemdetail d  left join base_dataitem i on i.itemid = d.itemid
                                 where  d.enabledmark = 1 and d.deletemark = 0  and i.itemcode in ({0})
                                order by d.sortcode asc", itemCode);
            }
            else
            {
                strSql.AppendFormat(@"select  i.itemid,i.itemcode  encode, d.itemdetailid,d.parentid,d.itemcode ,d.itemname ,
                                 d.itemvalue ,d.quickquery ,d.simplespelling ,d.isdefault ,d.sortcode ,d.enabledmark ,d.description
                                 from  base_dataitemdetail d  left join base_dataitem i on i.itemid = d.itemid
                                 where  d.enabledmark = 1 and d.deletemark = 0  and i.itemcode in ('{0}')
                                order by d.sortcode asc", itemCode);
            }

            return new RepositoryFactory().BaseRepository().FindList<DataItemModel>(strSql.ToString());
        }
        public IEnumerable<DataItemModel> GetDataItem(string itemCode, string itemDetailCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"select  i.itemid,i.itemcode  encode, d.itemdetailid,d.parentid,d.itemcode ,d.itemname ,
                                 d.itemvalue ,d.quickquery ,d.simplespelling ,d.isdefault ,d.sortcode ,d.enabledmark ,d.description
                                 from  base_dataitemdetail d  left join base_dataitem i on i.itemid = d.itemid
                                 where  d.enabledmark = 1 and d.deletemark = 0  and i.itemcode ='{0}' and d.itemvalue='{1}'
                                order by d.sortcode asc", itemCode, itemDetailCode);
            return new RepositoryFactory().BaseRepository().FindList<DataItemModel>(strSql.ToString());
        }

        public IEnumerable<DataItemModel> GetDataItemByDetailCode(string itemCode, string itemDetailCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"select  i.itemid,i.itemcode  encode, d.itemdetailid,d.parentid,d.itemcode ,d.itemname ,
                                 d.itemvalue ,d.quickquery ,d.simplespelling ,d.isdefault ,d.sortcode ,d.enabledmark ,d.description
                                 from  base_dataitemdetail d  left join base_dataitem i on i.itemid = d.itemid
                                 where  d.enabledmark = 1 and d.deletemark = 0  and i.itemcode ='{0}' and d.itemcode='{1}'
                                order by d.sortcode asc", itemCode, itemDetailCode);
            return new RepositoryFactory().BaseRepository().FindList<DataItemModel>(strSql.ToString());
        }

        public IEnumerable<DataItemModel> GetDataItemByDetailValue(string itemCode, string itemDetailValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"select  i.itemid,i.itemcode  encode, d.itemdetailid,d.parentid,d.itemcode ,d.itemname ,
                                 d.itemvalue ,d.quickquery ,d.simplespelling ,d.isdefault ,d.sortcode ,d.enabledmark ,d.description
                                 from  base_dataitemdetail d  left join base_dataitem i on i.itemid = d.itemid
                                 where  d.enabledmark = 1 and d.deletemark = 0  and i.itemcode ='{0}' and d.itemvalue='{1}'
                                order by d.sortcode asc", itemCode, itemDetailValue);
            return new RepositoryFactory().BaseRepository().FindList<DataItemModel>(strSql.ToString());
        }
        /// <summary>
        /// 通过编码类别名称获取编码
        /// </summary>
        /// <param name="itemName">编码类别名称</param>
        /// <returns></returns>
        public DataTable GetDataItemListByItemName(string itemName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"select a.itemname,a.itemvalue,description from  base_dataitemdetail a where itemid=(select itemid from base_dataitem where itemname='{0}' ) order by a.sortcode asc ", itemName);
            return new RepositoryFactory().BaseRepository().FindTable(strSql.ToString());
        }
        /// <summary>
        /// 根据编码名称获取编码值
        /// </summary>
        /// <param name="itemName">编码名称</param>
        /// <returns></returns>
        public string GetItemValue(string itemName)
        {
            string sql = string.Format("select t.itemvalue from base_dataitemdetail t where itemname='{0}'", itemName);
            object obj = new RepositoryFactory().BaseRepository().FindObject(sql);
            return obj == null || obj == System.DBNull.Value ? "" : obj.ToString();
        }

        /// <summary>
        /// 根据编码名称获取有效的编码值
        /// </summary>
        /// <param name="itemName">编码名称</param>
        /// <returns></returns>
        public string GetEnableItemValue(string itemName)
        {
            string sql = string.Format("select t.itemvalue from base_dataitemdetail t where itemname='{0}' and t.EnabledMark=1", itemName);
            object obj = new RepositoryFactory().BaseRepository().FindObject(sql);
            return obj == null || obj == System.DBNull.Value ? "" : obj.ToString();
        }

        /// <summary>
        /// 根据编码值获取编码名称
        /// </summary>
        /// <param name="itemCode"></param>
        /// <param name="itemValue"></param>
        /// <returns></returns>
        public string GetItemName(string itemCode, string itemValue)
        {
            string sql = string.Format("select itemname from  base_dataitemdetail  where itemid =(select  itemid from base_dataitem where itemcode='{0}') and itemvalue='{1}'", itemCode, itemValue);
            object obj = new RepositoryFactory().BaseRepository().FindObject(sql);
            return obj == null || obj == System.DBNull.Value ? "" : obj.ToString();
        }

        /// <summary>
        /// 根据编码名称获取编码code
        /// </summary>
        /// <param name="itemName">编码名称</param>
        /// <returns></returns>
        public string GetItemCode(string itemName)
        {
            string sql = string.Format("select t.itemcode from base_dataitemdetail t where itemname='{0}'", itemName);
            object obj = new RepositoryFactory().BaseRepository().FindObject(sql);
            return obj == null || obj == System.DBNull.Value ? "" : obj.ToString();
        }

        /// <summary>
        /// 根据编码名称获取编码值
        /// </summary>
        /// <param name="itemName">编码名称</param>
        /// <returns></returns>
        public string GetItemValue(string itemName, string itemcode)
        {
            string sql = string.Format("select itemvalue from  base_dataitemdetail  where itemid =(select  itemid from base_dataitem where itemcode='{0}') and itemname='{1}'", itemcode, itemName);
            object obj = new RepositoryFactory().BaseRepository().FindObject(sql);
            return obj == null || obj == System.DBNull.Value ? "" : obj.ToString();
        }
        #region 验证数据
        /// <summary>
        /// 项目值不能重复
        /// </summary>
        /// <param name="itemValue">项目值</param>
        /// <param name="keyValue">主键</param>
        /// <param name="itemId">分类Id</param>
        /// <returns></returns>
        public bool ExistItemValue(string itemValue, string keyValue, string itemId)
        {
            var expression = LinqExtensions.True<DataItemDetailEntity>();
            expression = expression.And(t => t.ItemValue == itemValue).And(t => t.ItemId == itemId);
            if (!string.IsNullOrEmpty(keyValue))
            {
                expression = expression.And(t => t.ItemDetailId != keyValue);
            }
            return this.BaseRepository().IQueryable(expression).Count() == 0 ? true : false;
        }
        /// <summary>
        /// 项目名不能重复
        /// </summary>
        /// <param name="itemName">项目名</param>
        /// <param name="keyValue">主键</param>
        /// <param name="itemId">分类Id</param>
        /// <returns></returns>
        public bool ExistItemName(string itemName, string keyValue, string itemId)
        {
            var expression = LinqExtensions.True<DataItemDetailEntity>();
            expression = expression.And(t => t.ItemName == itemName).And(t => t.ItemId == itemId);
            if (!string.IsNullOrEmpty(keyValue))
            {
                expression = expression.And(t => t.ItemDetailId != keyValue);
            }
            return this.BaseRepository().IQueryable(expression).Count() == 0 ? true : false;
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除明细
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 根据编码类别Id删除明细
        /// </summary>
        /// <param name="keyValue">编码类别Id</param>
        public void RemoveByItemId(string itemId)
        {
            this.BaseRepository().Delete(itemId, "ItemId");
        }
        /// <summary>
        /// 保存明细表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="dataItemDetailEntity">明细实体</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, DataItemDetailEntity dataItemDetailEntity)
        {

            if (!string.IsNullOrEmpty(keyValue))
            {
                dataItemDetailEntity.ItemDetailId = keyValue;
                DataItemDetailEntity entity = BaseRepository().FindEntity(keyValue);
                if (entity == null)
                {
                    dataItemDetailEntity.Create();
                    this.BaseRepository().Insert(dataItemDetailEntity);
                }
                else
                {
                    dataItemDetailEntity.Modify(keyValue);
                    this.BaseRepository().Update(dataItemDetailEntity);
                }

            }
            else
            {
                dataItemDetailEntity.Create();
                this.BaseRepository().Insert(dataItemDetailEntity);
            }
        }

        public void Save(List<DataItemDetailEntity> listItems)
        {
            this.BaseRepository().Delete(listItems);
            this.BaseRepository().Insert(listItems);
        }

        public IEnumerable<DataItemDetailEntity> GetListItems(string category)
        {
            var db = DbFactory.Base();
            var query = from q1 in db.IQueryable<DataItemEntity>()
                        join q2 in db.IQueryable<DataItemDetailEntity>() on q1.ItemId equals q2.ItemId
                        where q1.ItemName == category && q2.EnabledMark == 1
                        orderby q2.SortCode
                        select q2;
            return query.ToList();
        }
        #endregion
    }
}
