using ERCHTMS.Entity.BaseManage;
using ERCHTMS.IService.BaseManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System;
using System.Data;

namespace ERCHTMS.Service.BaseManage
{
    /// <summary>
    /// 描 述：区域设置
    /// </summary>
    public class DistrictService : RepositoryFactory<DistrictEntity>, IDistrictService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="orgID">当前用户所属组织机构ID</param>
        /// <returns>返回列表</returns>
        public IEnumerable<DistrictEntity> GetList(string orgID = "")
        {
            if (orgID == "")
                return this.BaseRepository().IQueryable().Where(t => t.DistrictCode.Length > 0).ToList();
            else return this.BaseRepository().IQueryable().ToList().Where(a => a.ParentID == orgID && a.DistrictCode.Length > 0);
        }
        /// <summary>
        /// 获取名称和ID
        /// </summary>
        /// <param name="ids">id集合</param>
        /// <returns>返回列表</returns>
        public DataTable GetNameAndID(string ids)
        {
            string whereSQL = "'";
            string[] arr = ids.Split(',');

            if (arr.Length > 0)
            {
                foreach (string item in arr)
                {
                    whereSQL += item + "','";
                }
                whereSQL = whereSQL + "'";
            }
            else whereSQL = whereSQL + "'";
            DataTable dt = this.BaseRepository().FindTable("select DISTRICTNAME,PARENTID,DISTRICTID,CHARGEDEPT,CHARGEDEPTCODE from bis_district where DISTRICTID in(" + whereSQL + ")");
            return dt;
        }
        /// <summary>
        /// 获取部门管控区域名称集合
        /// </summary>
        /// <param name="deptId">部门Id</param>
        /// <returns>返回列表Json</returns>
        public DataTable GetDeptNames(string deptId)
        {
            DataTable dt = this.BaseRepository().FindTable("select DISTRICTNAME from bis_district where CHARGEDEPTID='" + deptId + "'");
            return dt;
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public DistrictEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 区域列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<DistrictEntity> GetPageList(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();

            //查询条件
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyord = queryParam["keyword"].ToString();
                pagination.conditionJson += string.Format(" and t.DISTRICTNAME  like '%{0}%'", keyord);
            }
            if (!queryParam["code"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and (t.DISTRICTID='{0}'or t.PARENTID='{0}')", queryParam["code"].ToString());
            }

            IEnumerable<DistrictEntity> list = this.BaseRepository().FindListByProcPager(pagination, dataType);

            return list;

        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            var entity = GetEntity(keyValue);
            this.BaseRepository().ExecuteBySql(string.Format("Delete bis_district where districtCode like '{0}%'",entity.DistrictCode));
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, DistrictEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                entity.Create();
                entity.DistrictCode = GetDepartmentCode(entity);
                this.BaseRepository().Insert(entity);
            }
        }
        /// <summary>
        /// 根据当前机构获取对应的机构代码  机构代码 2-6-8-10  位
        /// </summary>
        /// <param name="districtEntity"></param>
        /// <returns></returns>
        public string GetDepartmentCode(DistrictEntity districtEntity)
        {
            string maxCode = string.Empty;

            OrganizeEntity oEntity = null;

            DistrictEntity dEntity = null;

            string deptcode = string.Empty;

            if (districtEntity.ParentID == "0")//选择是机构,没有选择上级区域
            {
                oEntity = new OrganizeService().BaseRepository().FindEntity(districtEntity.OrganizeId);  //获取父对象(机构)

                deptcode = oEntity.EnCode;
            }
            else //选择的是部门
            {
                dEntity = this.BaseRepository().FindEntity(districtEntity.ParentID);//获取部门父对象

                deptcode = dEntity.DistrictCode;
            }
            var maxObj = this.BaseRepository().FindList(string.Format("select max(districtcode) as districtcode  from bis_district t where  parentid='{0}' and organizeid='{1}'", districtEntity.ParentID, districtEntity.OrganizeId)).FirstOrDefault();
            if (!string.IsNullOrEmpty(maxObj.DistrictCode))
            {
                maxCode = new OrganizeService().QueryOrganizeCodeByCondition(maxObj.DistrictCode);
            }
            else
            {
                DistrictEntity parentEntity = this.BaseRepository().FindEntity(districtEntity.ParentID);  //获取父对象
                if (parentEntity != null && districtEntity.ParentID != "0")
                    maxCode = parentEntity.DistrictCode + "001";  //固定值,非可变
                else
                    maxCode = deptcode + "001";
            }

            ////确定是否存在上级部门,非部门根节点
            //if (districtEntity.ParentID != "0")
            //{
            //    //存在，则取编码最大的那一个
            //    if (maxObj.Count() > 0)
            //    {
            //        maxCode = maxObj.FirstOrDefault().DistrictCode;  //获取最大的Code 
            //        if (!string.IsNullOrEmpty(maxCode))
            //        {
            //            maxCode = new OrganizeService().QueryOrganizeCodeByCondition(maxCode);
            //        }
            //    }
            //    else
            //    {
            //        DistrictEntity parentEntity = this.BaseRepository().FindEntity(districtEntity.ParentID);  //获取父对象

            //        maxCode = parentEntity.DistrictCode + "001";  //固定值,非可变
            //    }
            //}
            //else  //部门根节点的操作
            //{
            //    //do somethings 
            //    if (maxObj.Count() > 0)
            //    {
            //        maxCode = maxObj.FirstOrDefault().DistrictCode;  //获取最大的Code 

            //        if (!string.IsNullOrEmpty(maxCode))
            //        {
            //            maxCode = new OrganizeService().QueryOrganizeCodeByCondition(maxCode);
            //        }
            //        else
            //        {
            //            maxCode = deptcode + "001";
            //        }
            //    }
            //    else
            //    {
            //        maxCode = deptcode + "001";  //固定值,非可变,加机构编码，组合新编码
            //    }
            //}
            //先判断当前编码是否存在于数据库表中
            return maxCode;
        }
        #endregion
    }
}
