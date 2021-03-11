using System;
using ERCHTMS.Entity.LaborProtectionManage;
using ERCHTMS.IService.LaborProtectionManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Code;

namespace ERCHTMS.Service.LaborProtectionManage
{
    /// <summary>
    /// 描 述：劳动防护用品
    /// </summary>
    public class LaborprotectionService : RepositoryFactory<LaborprotectionEntity>, LaborprotectionIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<LaborprotectionEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public LaborprotectionEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 存储过程分页
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageListByProc(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //查询条件
            if (!queryParam["name"].IsEmpty())
            {
                string name = queryParam["name"].ToString();

                pagination.conditionJson += string.Format(" and NAME  like '%{0}%'", name);

                
            }
            if (!queryParam["type"].IsEmpty())
            {
                string unit = queryParam["type"].ToString();

                pagination.conditionJson += string.Format(" and type  like '%{0}%'", unit);
            }

            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// 获取当前机构所有物资
        /// </summary>
        /// <returns></returns>
        public List<LaborprotectionEntity> GetLaborList()
        {
            string sql = string.Format("select * from BIS_LABORPROTECTION where CREATEUSERORGCODE='{0}' order by no desc",
                OperatorProvider.Provider.Current().OrganizeCode);
            List<LaborprotectionEntity> LaborList = BaseRepository().FindList(sql).ToList();
            return LaborList;
        }

        /// <summary>
        /// 获取到当前
        /// </summary>
        /// <returns></returns>
        public string GetNo()
        {
            string time = DateTime.Now.ToString("yyyyMM");
            string sql = string.Format("select No from (select NO from BIS_LABORPROTECTION where CREATEUSERORGCODE='{0}' and no like '{1}%' order by no desc) where  rownum<=1",
                OperatorProvider.Provider.Current().OrganizeCode, time);
            object no = BaseRepository().FindObject(sql);
            if (no == null)
            {
                return time + "001";
            }
            else
            {
                int newno = Convert.ToInt32(no) + 1;
                return newno.ToString();
            }
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
        public void SaveForm(string keyValue, LaborprotectionEntity entity)
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
