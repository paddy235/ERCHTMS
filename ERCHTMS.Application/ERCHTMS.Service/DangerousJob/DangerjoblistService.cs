using ERCHTMS.Entity.DangerousJob;
using ERCHTMS.IService.DangerousJob;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Code;
using ERCHTMS.Service.BaseManage;
using ERCHTMS.Service.SystemManage;

namespace ERCHTMS.Service.DangerousJob
{
    /// <summary>
    /// 描 述：危险作业清单
    /// </summary>
    public class DangerjoblistService : RepositoryFactory<DangerjoblistEntity>, DangerjoblistIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<DangerjoblistEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string role = user.RoleName;
            var queryParam = queryJson.ToJObject();
            #region 查表
            pagination.p_kid = "t.id";
            pagination.p_fields = @"t.createuserid,t.createdate,t.createusername,t.modifyuserid,t.modifydate,t.modifyusername,t.createuserdeptcode,t.createuserorgcode,t.dangerjobname,t.numberofpeoplename,
                       t.deptids, t.deptnames,t.jobfrequency,t.dangerfactors,t.accidentcategories,t.safetymeasures,t.joblevelname,t.principalnames";
            pagination.p_tablename = @"BIS_DANGERJOBLIST t ";
            if (pagination.sidx == null)
            {
                pagination.sidx = "t.createdate";
            }
            if (pagination.sord == null)
            {
                pagination.sord = "desc";
            }
            #endregion

    
            //关键字
            if (!queryParam["keyword"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and (t.dangerjobname like '%{0}%' or t.deptnames like '%{0}%' or t.dangerfactors like '%{0}%' or t.accidentcategories like '%{0}%') ", queryParam["keyword"].ToString());
            }

            DepartmentService departmentDll = new DepartmentService();
            DataItemDetailService dataItemDetail = new DataItemDetailService();
            string codes = user.OrganizeCode;
            var spdepart = dataItemDetail.GetDataItemListByItemCode("spdepart").Select(p => p.ItemValue).FirstOrDefault();

            //树节点code
            var deptCode = !queryParam["code"].IsEmpty() ? queryParam["code"].ToString() : user.DeptCode;
            if (deptCode == user.OrganizeCode || ((!string.IsNullOrEmpty(spdepart) && spdepart.Contains(deptCode)) && queryParam["code"].IsEmpty()))
            {
                if (!string.IsNullOrEmpty(spdepart) && spdepart.Contains(deptCode))
                {
                    List<string> deptCodes = spdepart.Split(',').ToList();
                    deptCodes.Add(user.OrganizeCode);
                    codes = string.Join("','", deptCodes);
                }
                pagination.conditionJson += string.Format(" and (t.createuserdeptcode in ('{0}') ) ", codes);
            }
            else
            {
                pagination.conditionJson += string.Format(" and t.createuserdeptcode='{0}' ", deptCode);
            }         

            //危险作业级别
            if (!queryParam["joblevel"].IsEmpty())
            {
                var joblevel = queryParam["joblevel"].ToString();
                pagination.conditionJson += string.Format(" and t.joblevel='{0}' ", joblevel);
            }
            //作业人数
            if (!queryParam["numberofpeople"].IsEmpty())
            {
                var numberofpeople = queryParam["numberofpeople"].ToString();
                pagination.conditionJson += string.Format(" and t.numberofpeople='{0}' ", numberofpeople);
            } 
            DataTable data = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            foreach (DataRow dr in data.Rows)
            {
                string deptNames = "";
                var deptids = dr["deptids"].ToString().Split(',');
                for (int i = 0; i < deptids.Length; i++)
                {
                    var dept = departmentDll.GetEntity(deptids[i]);
                    if (dept != null)
                    {
                        if (dept.Nature == "专业" || dept.Nature == "班组")
                        {
                            DataTable dt = departmentDll.GetDataTable(string.Format(
                                "select fullname from BASE_DEPARTMENT where encode=(select encode from BASE_DEPARTMENT t where instr('{0}',encode)=1 and nature='{1}' and organizeid='{2}') or encode='{0}' order by deptcode",
                                dept.EnCode, "部门", dept.OrganizeId));
                            if (dt.Rows.Count > 0)
                            {
                                string name = "";
                                foreach (DataRow dr1 in dt.Rows)
                                {
                                    name += dr1["fullname"].ToString() + "/";
                                }

                                deptNames += name.TrimEnd('/')+",";
                            }
                        }
                        else
                        {
                            deptNames += dept.FullName + ",";
                        }
                    }               
                }
                if (!string.IsNullOrEmpty(deptNames) && deptNames.Length>1)
                    dr["deptNames"] = deptNames.TrimEnd(',');
            }

            return data;
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public DangerjoblistEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
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
        public void SaveForm(string keyValue, DangerjoblistEntity entity)
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
