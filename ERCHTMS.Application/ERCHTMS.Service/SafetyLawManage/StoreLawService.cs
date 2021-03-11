using ERCHTMS.Entity.SafetyLawManage;
using ERCHTMS.IService.SafetyLawManage;
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
using ERCHTMS.Service.BaseManage;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Service.SafetyLawManage
{
    /// <summary>
    /// 描 述：收藏法律法规
    /// </summary>
    public class StoreLawService : RepositoryFactory<StoreLawEntity>, StoreLawIService
    {
        DepartmentService DepartmentService = new DepartmentService();
        #region 获取数据
        /// <summary>
        /// 获取安全生产法律法规我的收藏列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageDataTable(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //查询条件
            if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            {
                string condition = queryParam["condition"].ToString();
                string keyord = queryParam["keyword"].ToString();
                switch (condition)
                {
                    case "filename"://文件名称
                        pagination.conditionJson += string.Format(" and b.FileName  like '%{0}%'", keyord);
                        break;
                    case "issuedept"://颁发部门
                        pagination.conditionJson += string.Format(" and b.IssueDept  like '%{0}%'", keyord);
                        break;
                    default:
                        break;
                }
            }
            //类型节点
            if (!queryParam["code"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and b.LawTypeCode like '%{0}%'", queryParam["code"].ToString());
            }
            #region 书面工作程序SWP
            //查询条件
            if (!queryParam["filename"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and FileName  like '%{0}%'", queryParam["filename"].ToString());
            }
            //所属单位
            if (!queryParam["belongcode"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and BelongTypeCode like '{0}%'", queryParam["belongcode"].ToString());
            }
            #endregion
            //选中的数据
            if (!queryParam["idsData"].IsEmpty())
            {
                var ids = queryParam["idsData"].ToString();
                string idsarr = "";
                if (ids.Contains(','))
                {
                    string[] array = ids.TrimEnd(',').Split(',');
                    foreach (var item in array)
                    {
                        idsarr += "'" + item + "',";
                    }
                    if (idsarr.Contains(","))
                        idsarr = idsarr.TrimEnd(',');
                }
                pagination.conditionJson += string.Format(" and b.id in({0})", idsarr);
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }


        /// <summary>
        /// 获取安全管理制度我的收藏列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageJsonInstitution(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var queryParam = queryJson.ToJObject();
            //查询条件
            if (!queryParam["filename"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and FileName like '%{0}%'", queryParam["filename"].ToString());
            }

            if (user.RoleName.Contains("省级用户"))
            {
                if (!queryParam["orgcode"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and t.createuserorgcode ='{0}'", queryParam["orgcode"].ToString());
                }
            }
            else
            {
                //0本级,1上级
                if (!queryParam["state"].IsEmpty())
                {
                    if (queryParam["state"].ToString() == "0")
                    {
                        pagination.conditionJson += string.Format(" and t.createuserorgcode ='{0}'", user.OrganizeCode);
                    }
                    else
                    {
                        var provdata = DepartmentService.GetList().Where(t => user.NewDeptCode.StartsWith(t.DeptCode) && t.Nature == "省级" && string.IsNullOrWhiteSpace(t.Description));
                        if (provdata.Count() > 0)
                        {
                            DepartmentEntity provEntity = provdata.FirstOrDefault();
                            pagination.conditionJson += string.Format(" and t.createuserorgcode ='{0}'", provEntity.EnCode);
                        }
                    }
                }
            }
            //时间范围
            if (!queryParam["sTime"].IsEmpty() || !queryParam["eTime"].IsEmpty())
            {
                string startTime = queryParam["sTime"].ToString();
                string endTime = queryParam["eTime"].ToString();
                if (queryParam["sTime"].IsEmpty())
                {
                    startTime = "1899-01-01";
                }
                if (queryParam["eTime"].IsEmpty())
                {
                    endTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
                pagination.conditionJson += string.Format(" and ReleaseDate between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", startTime, endTime);
            }
            //选择的类型
            if (!queryParam["treeCode"].IsEmpty())
            {
                if (!queryParam["flag"].IsEmpty())
                {
                    if (queryParam["flag"].ToString() == "0")
                    {
                        pagination.conditionJson += string.Format(" and  LawTypeCode='{0}'", queryParam["treeCode"].ToString());
                    }
                    else
                    {
                        pagination.conditionJson += string.Format(@" and LawTypeCode like '{0}%'", queryParam["treeCode"].ToString());
                    }
                }
                else
                {
                    pagination.conditionJson += string.Format(" and  LawTypeCode='{0}'", queryParam["treeCode"].ToString());
                }
            }
            //选中的数据
            if (!queryParam["idsData"].IsEmpty())
            {
                var ids = queryParam["idsData"].ToString();
                string idsarr = "";
                if (ids.Contains(','))
                {
                    string[] array = ids.TrimEnd(',').Split(',');
                    foreach (var item in array)
                    {
                        idsarr += "'" + item + "',";
                    }
                    if (idsarr.Contains(","))
                        idsarr = idsarr.TrimEnd(',');
                }
                pagination.conditionJson += string.Format(" and storeid in({0})", idsarr);
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);

        }


        /// <summary>
        /// 获取安全操作规程我的收藏列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageJsonStandards(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var queryParam = queryJson.ToJObject();
            //查询条件
            if (!queryParam["filename"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and FileName like '%{0}%'", queryParam["filename"].ToString());
            }

            if (user.RoleName.Contains("省级用户"))
            {
                if (!queryParam["orgcode"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and t.createuserorgcode ='{0}'", queryParam["orgcode"].ToString());
                }
            }
            else
            {
                //0本级,1上级
                if (!queryParam["state"].IsEmpty())
                {
                    if (queryParam["state"].ToString() == "0")
                    {
                        pagination.conditionJson += string.Format(" and t.createuserorgcode ='{0}'", user.OrganizeCode);
                    }
                    else
                    {
                        var provdata = DepartmentService.GetList().Where(t => user.NewDeptCode.StartsWith(t.DeptCode) && t.Nature == "省级" && string.IsNullOrWhiteSpace(t.Description));
                        if (provdata.Count() > 0)
                        {
                            DepartmentEntity provEntity = provdata.FirstOrDefault();
                            pagination.conditionJson += string.Format(" and t.createuserorgcode ='{0}'", provEntity.EnCode);
                        }
                    }
                }
            }
            //时间范围
            if (!queryParam["sTime"].IsEmpty() || !queryParam["eTime"].IsEmpty())
            {
                string startTime = queryParam["sTime"].ToString();
                string endTime = queryParam["eTime"].ToString();
                if (queryParam["sTime"].IsEmpty())
                {
                    startTime = "1899-01-01";
                }
                if (queryParam["eTime"].IsEmpty())
                {
                    endTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
                pagination.conditionJson += string.Format(" and ReleaseDate between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", startTime, endTime);
            }
            //选择的类型
            if (!queryParam["treeCode"].IsEmpty())
            {
                if (!queryParam["flag"].IsEmpty())
                {
                    if (queryParam["flag"].ToString() == "0")
                    {
                        pagination.conditionJson += string.Format(" and  LawTypeCode='{0}'", queryParam["treeCode"].ToString());
                    }
                    else
                    {
                        pagination.conditionJson += string.Format(@" and LawTypeCode like '{0}%'", queryParam["treeCode"].ToString());
                    }
                }
                else
                {
                    pagination.conditionJson += string.Format(" and  LawTypeCode='{0}'", queryParam["treeCode"].ToString());
                }
            }
            //选中的数据
            if (!queryParam["idsData"].IsEmpty())
            {
                var ids = queryParam["idsData"].ToString();
                string idsarr = "";
                if (ids.Contains(','))
                {
                    string[] array = ids.TrimEnd(',').Split(',');
                    foreach (var item in array)
                    {
                        idsarr += "'" + item + "',";
                    }
                    if (idsarr.Contains(","))
                        idsarr = idsarr.TrimEnd(',');
                }
                pagination.conditionJson += string.Format(" and storeid in({0})", idsarr);
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);

        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<StoreLawEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public StoreLawEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 根据法律id确定是否已收藏
        /// </summary>
        /// <returns></returns>
        public int GetStoreBylawId(string lawid)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            int num = 0;
            StoreLawEntity entity = this.BaseRepository().IQueryable().ToList().Where(t => t.LawId == lawid && t.UserId==user.UserId).FirstOrDefault();
            if (entity != null)
                num = 1;
            return num;
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
        public void SaveForm(string keyValue, StoreLawEntity entity)
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
