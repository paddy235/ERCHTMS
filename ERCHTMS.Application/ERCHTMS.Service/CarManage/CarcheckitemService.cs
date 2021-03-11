using System;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.IService.CarManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Code;
using ERCHTMS.Entity.HseManage;
using ERCHTMS.Entity.SystemManage.ViewModel;

namespace ERCHTMS.Service.CarManage
{
    /// <summary>
    /// 描 述：配置危化品检查项目表
    /// </summary>
    public class CarcheckitemService : RepositoryFactory<CarcheckitemEntity>, CarcheckitemIService
    {
        #region 获取数据

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            var curuser = OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<CarcheckitemEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public CarcheckitemEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 获取去重的危害因素列表
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public IEnumerable<DataItemModel> GetHazardousList(string KeyValue)
        {
            string where = "";
            if (KeyValue != "")
            {
                where = string.Format(
                    " where cid!='{0}'",
                    KeyValue);
            }

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"select  i.itemid,i.itemcode  encode, d.itemdetailid,d.parentid,d.itemcode ,d.itemname ,
                                             d.itemvalue ,d.quickquery ,d.simplespelling ,d.isdefault ,d.sortcode ,d.enabledmark ,d.description
                                             from  base_dataitemdetail d  left join base_dataitem i on i.itemid = d.itemid
                                             where  d.enabledmark = 1 and d.deletemark = 0  and i.itemcode in ({0}) and d.itemdetailid not in(select hazardousid from BIS_CARCHECKITEMHAZARDOUS {1} )
                                            order by d.sortcode asc", "'HazardousCar'", where);


            return new RepositoryFactory().BaseRepository().FindList<DataItemModel>(strSql.ToString());
        }

        /// <summary>
        /// 获取通行门岗
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public IEnumerable<DataItemModel> GetCurrentList(string KeyValue)
        {
            string where = "";
            if (KeyValue != "")
            {
                where = string.Format(
                    " where cid!='{0}'",
                    KeyValue);
            }

            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select  i.itemid,i.itemcode  encode, d.itemdetailid,d.parentid,d.itemcode ,d.itemname ,
                                 d.itemvalue ,d.quickquery ,d.simplespelling ,d.isdefault ,d.sortcode ,d.enabledmark ,d.description
                                 from  base_dataitemdetail d  left join base_dataitem i on i.itemid = d.itemid
                                 where  d.enabledmark = 1 and d.deletemark = 0  and i.itemcode in ('CurrentGatekeeper') 
                                order by d.sortcode asc");


            return new RepositoryFactory().BaseRepository().FindList<DataItemModel>(strSql.ToString());
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
        public void SaveForm(string keyValue, string CheckItemName, List<CarcheckitemhazardousEntity> HazardousArray, List<CarcheckitemmodelEntity> ItemArray)
        {
            //开始事物
            var res = DbFactory.Base().BeginTrans();
            try
            {
                CarcheckitemEntity item;
                if (!string.IsNullOrEmpty(keyValue))
                {
                    Repository<CarcheckitemEntity> inlogdb = new Repository<CarcheckitemEntity>(DbFactory.Base());
                    item = inlogdb.FindEntity(keyValue);
                    item.Modify(keyValue);
                    item.CheckItemName = CheckItemName;
                    res.Update<CarcheckitemEntity>(item);
                    res.Delete<CarcheckitemhazardousEntity>(it => it.CID == keyValue);
                    res.Delete<CarcheckitemmodelEntity>(it => it.CID == keyValue);
                }
                else
                {
                    item = new CarcheckitemEntity();
                    item.Create();
                    item.CheckItemName = CheckItemName;
                    item.CreateUserName = OperatorProvider.Provider.Current().UserName;
                    res.Insert<CarcheckitemEntity>(item);
                }

                for (int i = 0; i < HazardousArray.Count; i++)
                {
                    HazardousArray[i].Create();
                    HazardousArray[i].CID = item.ID;

                }

                for (int i = 0; i < ItemArray.Count; i++)
                {
                    ItemArray[i].Create();
                    ItemArray[i].CID = item.ID;

                }

                res.Insert<CarcheckitemmodelEntity>(ItemArray);
                res.Insert<CarcheckitemhazardousEntity>(HazardousArray);

                res.Commit();
            }
            catch (Exception ex)
            {
                res.Rollback();
                throw;
            }

        }
        #endregion
    }
}
