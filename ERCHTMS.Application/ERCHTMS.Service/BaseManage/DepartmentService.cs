using ERCHTMS.Entity.BaseManage;
using ERCHTMS.IService.BaseManage;
using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util.Extension;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Data;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Service.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Dynamic;
using Newtonsoft.Json;
using System.Net;
using ERCHTMS.Service.SystemManage;
using System.Web;
using System.Configuration;

namespace ERCHTMS.Service.BaseManage
{
    /// <summary>
    /// 描 述：部门管理
    /// </summary>
    public class DepartmentService : RepositoryFactory<DepartmentEntity>, IDepartmentService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 验证部门名称是否重复
        /// </summary>
        /// <param name="departmentName">部门名称</param>
        /// <returns></returns>
        public bool ExistDeptJugement(string departmentName, string orgId = "")
        {
            IEnumerable<DepartmentEntity> count = this.BaseRepository().FindList(string.Format("select departmentid  from BASE_DEPARTMENT t where t.fullname='{0}' and organizeid='{1}'", departmentName, orgId));
            if (count.Count() > 0)
                return false;
            else return true;
        }

        #region    获取上一个部门
        /// <summary>
        /// 获取上一个部门
        /// </summary>
        /// <param name="parentid"></param>
        /// <param name="nature"></param>
        /// <returns></returns>
        public DepartmentEntity GetParentDeptBySpecialArgs(string parentid, string nature)
        {
            DepartmentEntity pentity = new DepartmentEntity();

            try
            {
                var deptEntity = GetEntity(parentid);

                if (null != deptEntity)
                {
                    if (!string.IsNullOrEmpty(nature))
                    {
                        //如果当前部门满足对应部门性质，则取当前部门
                        if (deptEntity.Nature == nature)
                        {
                            pentity = deptEntity;
                        }
                        else
                        {
                            pentity = GetParentDeptBySpecialArgs(deptEntity.ParentId, nature);
                        }
                    }
                    else
                    {
                        pentity = deptEntity;
                    }
                }
            }
            catch (System.Exception)
            {
                throw;
            }
            return pentity;
        }
        #endregion

        /// <summary>
        /// 部门列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DepartmentEntity> GetList()
        {
            return this.BaseRepository().IQueryable().OrderByDescending(t => t.CreateDate).ToList();
        }
        /// <summary>
        /// 非黑名单的承包商列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DepartmentEntity> GetNotBlackList()
        {
            string sql = @"select b.* from epg_outsourcingproject o
                                 inner join base_department b on o.outprojectid=b.departmentid
                                 where o.blackliststate='0'";
            return this.BaseRepository().FindList(sql);
        }
        /// <summary>
        /// 部门实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public DepartmentEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }


        public DepartmentEntity GetEntityByCode(string keyValue)
        {
            return this.BaseRepository().IQueryable().Where(p => p.EnCode == keyValue).FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<DepartmentEntity> GetDepts(string parentid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT * FROM base_department u CONNECT BY PRIOR u.departmentid=u.parentid  START WITH u.departmentid='{0}'", parentid);
            return new RepositoryFactory().BaseRepository().FindList<DepartmentEntity>(strSql.ToString());
        }

        /// <summary>
        /// 获取机构厂级部门
        /// <param name="orgid">当前机构</param>
        /// <param name="isorg">是否厂级 0-否 1-是</param>
        /// </summary>
        public IEnumerable<DepartmentEntity> GetDepts(string orgid, int isorg)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT * FROM base_department t where t.organizeid = '{0}' and t.isorg = '{1}'", orgid, isorg);
            return new RepositoryFactory().BaseRepository().FindList<DepartmentEntity>(strSql.ToString());
        }

        /// <summary>
        /// 获取承包商信息
        /// </summary>
        /// <param name="orgCode"></param>
        /// <returns></returns>
        public DataTable GetContractDepts(string orgCode)
        {
            object obj = base.BaseRepository().FindObject(string.Format("select encode from base_department where description='{0}' and encode like '{1}%'", "外包工程承包商", orgCode));
            if (obj != null)
            {
                return base.BaseRepository().FindTable(string.Format("select encode,fullname,'0' depttype from base_department where encode like '{0}%' and (nature='承包商' or nature='分包商' or description='外包工程承包商') order by encode", obj.ToString()));
            }
            else
            {

                DataTable dt = BaseRepository().FindTable("select '' encode,'' fullname,'0' depttype from dual");
                dt.Rows.Clear();
                return dt;
            }

        }
        /// <summary>
        /// 获取当前用户所属的省级
        /// </summary>
        /// <param name="user">当前用户</param>
        /// <returns></returns>
        public DepartmentEntity GetUserCompany(ERCHTMS.Code.Operator user)
        {
            object obj = BaseRepository().FindObject(string.Format("select d.departmentid  from base_department d where instr('{0}',d.deptcode)=1 and d.description is null and d.nature='{1}'", user.NewDeptCode, "省级"));
            return obj == null ? null : BaseRepository().FindEntity(obj.ToString());
        }
        /// <summary>
        /// 获取指定部门的所属机构信息
        /// </summary>
        /// <param name="deptId">部门Id</param>
        /// <returns></returns>
        public DepartmentEntity GetDeptOrgInfo(string deptId)
        {
            var rep = new Repository<DepartmentEntity>(DbFactory.Base());
            DepartmentEntity de = rep.FindEntity(deptId);
            if (de != null)
            {
                if ((de.Nature == "集团" || de.Nature == "省级" || de.Nature == "厂级") && string.IsNullOrEmpty(de.Description))
                {
                    return de;
                }
                else
                {
                    DepartmentEntity dept = new DepartmentEntity();
                    if (de.Nature == "部门")
                    {
                        dept = rep.FindEntity(de.ParentId);
                    }
                    if (de.Nature == "承包商" || de.Nature == "分包商" || de.Nature == "班组")
                    {
                        dept = rep.IQueryable(t => t.Nature == "厂级" && string.IsNullOrEmpty(t.Description) && de.DeptCode.StartsWith(t.DeptCode)).FirstOrDefault();
                    }
                    return dept;
                }
            }
            else
            {
                return null;
            }

        }
        /// <summary>
        /// 获取省级下属管辖的所有电厂信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable GetAllFactory(ERCHTMS.Code.Operator user)
        {
            DataTable dt = BaseRepository().FindTable(string.Format("select encode,fullname,'0',deptcode,departmentid,Manager,DepartDuty from BASE_DEPARTMENT d where d.deptcode like '{0}%' and d.nature='{1}' and d.description is null order by d.deptcode ", user.NewDeptCode, "厂级"));
            return dt;
        }
        /// <summary>
        /// 根据部门名称和机构Id获取部门编码
        /// </summary>
        /// <param name="deptName">部门名称</param>
        /// <param name="orgId">机构Id</param>
        /// <returns></returns>
        public string GetDeptCode(string deptName, string orgId)
        {
            if (string.IsNullOrEmpty(deptName))
            {
                return "";
            }
            object obj = base.BaseRepository().FindObject(string.Format("select encode from BASE_DEPARTMENT t where fullname='{0}' and organizeid='{1}'", deptName, orgId));
            return obj == null ? "" : obj.ToString();
        }
        /// <summary>
        /// 根据岗位名称和机构Id获取岗位Id
        /// </summary>
        /// <param name="postName">部门名称</param>
        /// <param name="orgId">机构Id</param>
        /// <param name="deptCode">部门编码</param>
        /// <returns></returns>
        public string GetPostId(string postName, string orgId, string deptCode)
        {
            if (string.IsNullOrEmpty(postName))
            {
                return "";
            }
            object obj = base.BaseRepository().FindObject(string.Format("select roleid from BASE_ROLE t where fullname='{0}' and organizeid='{1}' and t.category=2 and nature=(select nature from base_department  where encode='{2}')", postName, orgId, deptCode));
            return obj == null ? "" : obj.ToString();
        }
        /// <summary>
        /// 根据区域名称和机构Id获取区域Id
        /// </summary>
        /// <param name="areaName">区域名称</param>
        /// <param name="orgId">机构Id</param>
        /// <returns></returns>
        public string GetAreaId(string areaName, string orgId)
        {
            if (string.IsNullOrEmpty(areaName))
            {
                return "";
            }
            object obj = base.BaseRepository().FindObject(string.Format("select t.districtid from BIS_DISTRICT t where t.districtname='{0}' and t.organizeid='{1}'", areaName, orgId));
            return obj == null ? "" : obj.ToString();
        }
        #endregion

        #region 验证数据
        /// <summary>
        /// 部门编号不能重复
        /// </summary>
        /// <param name="enCode">编号</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool ExistEnCode(string enCode, string keyValue)
        {
            var expression = LinqExtensions.True<DepartmentEntity>();
            expression = expression.And(t => t.EnCode == enCode);
            if (!string.IsNullOrEmpty(keyValue))
            {
                expression = expression.And(t => t.DepartmentId != keyValue);
            }
            return this.BaseRepository().IQueryable(expression).Count() == 0 ? true : false;
        }
        /// <summary>
        /// 部门名称不能重复
        /// </summary>
        /// <param name="fullName">名称</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool ExistFullName(string fullName, string keyValue)
        {
            var expression = LinqExtensions.True<DepartmentEntity>();
            expression = expression.And(t => t.FullName == fullName);
            if (!string.IsNullOrEmpty(keyValue))
            {
                expression = expression.And(t => t.DepartmentId != keyValue);
            }
            return this.BaseRepository().IQueryable(expression).Count() == 0 ? true : false;
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="ue">用户实体</param>
        public bool RemoveForm(string keyValue, List<UserEntity> ue)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            try
            {
                DepartmentEntity dept = this.BaseRepository().FindEntity(keyValue);
                if (dept != null)
                {
                    db.Delete<DepartmentEntity>(keyValue);
                    if (ue != null)
                    {
                        if (ue.Count > 0)
                        {
                            db.Delete(ue);
                        }

                    }
                    ExecuteSql(string.Format("delete from BIS_TOOLSDEPT where deptcode like '{0}%'", dept.EnCode));
                }
                db.Commit();
                return true;
            }
            catch (Exception)
            {
                db.Rollback();
                return false;
            }
        }
        /// <summary>
        /// 保存部门表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="departmentEntity">机构实体</param>
        /// <returns></returns>
        public bool SaveForm(string keyValue, DepartmentEntity departmentEntity)
        {

            string parentId = departmentEntity.ParentId;
            DepartmentEntity dept = new DepartmentEntity();
            int count = 0;
            string wbDeptId = "";
            DataTable dtWB = GetDataTable(string.Format("select d.departmentid from base_department d where d.organizeid='{0}' and  d.description='外包工程承包商'", departmentEntity.OrganizeId));
            if (dtWB.Rows.Count > 0)
            {
                wbDeptId = dtWB.Rows[0][0].ToString();
            }
            if (!string.IsNullOrEmpty(keyValue))
            {
                DepartmentEntity curEntity = this.BaseRepository().FindEntity(keyValue);
                if (curEntity == null)
                {
                    //departmentEntity.EnCode = GetDepartmentCode(departmentEntity);
                    departmentEntity.DeptCode = GetDeptCode(departmentEntity);
                    departmentEntity.EnCode = GetDeptCode(departmentEntity, "encode");
                    departmentEntity.Create();
                    if (departmentEntity.Nature == "分包商")
                    {
                        //将此分包商的发包单位改为此上级承包商的发包单位
                        curEntity = this.BaseRepository().FindEntity(departmentEntity.ParentId);
                        departmentEntity.SendDeptID = curEntity.SendDeptID;
                        departmentEntity.SendDeptName = curEntity.SendDeptName;
                    }
                    if (departmentEntity.Nature == "承包商" && departmentEntity.ParentId == wbDeptId)
                    {
                        //保存外包单位基础信息
                        OutsourcingprojectEntity OutProEntity = new OutsourcingprojectEntity();
                        OutProEntity.OUTPROJECTID = departmentEntity.DepartmentId;
                        OutProEntity.OUTSOURCINGNAME = departmentEntity.FullName;
                        OutProEntity.BLACKLISTSTATE = "0";//默认状态
                        new OutsourcingprojectService().SaveForm(OutProEntity.ID, OutProEntity);
                    }
                    if (string.IsNullOrWhiteSpace(departmentEntity.ParentId))
                    {
                        departmentEntity.ParentId = "0";
                    }
                    if (departmentEntity.Nature == "集团" || departmentEntity.Nature == "省级" || departmentEntity.Nature == "厂级")
                    {
                        departmentEntity.OrganizeId = departmentEntity.DepartmentId;
                    }
                    count = this.BaseRepository().Insert(departmentEntity);
                    if (count > 0)
                    {
                        departmentEntity = this.BaseRepository().FindEntity(departmentEntity.DepartmentId);
                        dept = departmentEntity;
                        if ((departmentEntity.Nature == "集团" || departmentEntity.Nature == "省级") && string.IsNullOrEmpty(departmentEntity.Description))
                        {
                            string desc = "各电厂";
                            if (departmentEntity.Nature == "集团")
                            {
                                desc = "区域子公司";
                            }
                            dept = new DepartmentEntity
                            {
                                DepartmentId = System.Guid.NewGuid().ToString(),
                                DeptCode = departmentEntity.DeptCode + "001",
                                EnCode = departmentEntity.EnCode + "001",
                                Nature = "省级",
                                Description = desc,
                                SortCode = 100,
                                FullName = desc,
                                ParentId = departmentEntity.DepartmentId,
                                OrganizeId = departmentEntity.DepartmentId,
                                CreateDate = DateTime.Now
                            };
                            this.BaseRepository().Insert(dept);
                        }
                        if (departmentEntity.Nature == "厂级")
                        {

                            dept = new DepartmentEntity
                            {
                                DepartmentId = System.Guid.NewGuid().ToString(),
                                DeptCode = departmentEntity.DeptCode + "001",
                                EnCode = departmentEntity.EnCode + "001",
                                Nature = "部门",
                                SortCode = 100,
                                Description = "外包工程承包商",
                                FullName = "外包工程承包商",
                                ParentId = departmentEntity.DepartmentId,
                                OrganizeId = departmentEntity.DepartmentId,
                                CreateDate = DateTime.Now

                            };
                            this.BaseRepository().Insert(dept);
                        }
                    }
                }
                else
                {
                    if (curEntity.ParentId != departmentEntity.ParentId || string.IsNullOrEmpty(curEntity.EnCode))
                    {

                        departmentEntity.DeptCode = GetDeptCode(departmentEntity);
                    }
                    departmentEntity.Modify(keyValue);
                    if (departmentEntity.Nature == "集团" || departmentEntity.Nature == "省级" || departmentEntity.Nature == "厂级")
                    {
                        departmentEntity.OrganizeId = departmentEntity.DepartmentId;
                    }
                    count = this.BaseRepository().Update(departmentEntity);
                    if (count > 0)
                    {
                        dept = departmentEntity;
                        if (curEntity.ParentId != parentId)
                        {
                            dept = BaseRepository().FindEntity(departmentEntity.ParentId);
                            if (dept != null)
                            {
                                BaseRepository().ExecuteBySql(string.Format("update BASE_DEPARTMENT set deptcode='{0}' || encode where encode like '{1}%'", dept.DeptCode, departmentEntity.EnCode));
                            }
                        }
                    }
                    if (curEntity.Nature == "承包商")
                    {
                        var expression = LinqExtensions.True<DepartmentEntity>();
                        expression = expression.And(t => t.ParentId == departmentEntity.DepartmentId);
                        var allc = this.BaseRepository().IQueryable(expression).ToList();
                        foreach (var item in allc)
                        {
                            //将此承包商下级的分包商的发包单位改为承包商的发包单位
                            item.SendDeptID = departmentEntity.SendDeptID;
                            item.SendDeptName = departmentEntity.SendDeptName;
                            this.BaseRepository().Update(item);
                        }
                        if (departmentEntity.ParentId == wbDeptId)
                        {
                            OutsourcingprojectEntity OutProEntity = new OutsourcingprojectService().GetInfo(departmentEntity.DepartmentId);
                            if (OutProEntity == null)
                            {
                                //保存外包单位基础信息
                                OutProEntity = new OutsourcingprojectEntity();
                                OutProEntity.OUTPROJECTID = departmentEntity.DepartmentId;
                                OutProEntity.OUTSOURCINGNAME = departmentEntity.FullName;
                                OutProEntity.BLACKLISTSTATE = "0";//默认状态
                                new OutsourcingprojectService().SaveForm(OutProEntity.ID, OutProEntity);
                            }
                            else
                            {
                                OutProEntity.OUTSOURCINGNAME = departmentEntity.FullName;
                                new OutsourcingprojectService().SaveForm(OutProEntity.ID, OutProEntity);
                            }
                        }

                    }
                }

            }
            else
            {
                departmentEntity.DeptCode = GetDeptCode(departmentEntity);
                departmentEntity.EnCode = GetDeptCode(departmentEntity, "encode");
                departmentEntity.Create();
                if (departmentEntity.Nature == "分包商")
                {
                    //将此分包商的发包单位改为此上级承包商的发包单位
                    DepartmentEntity curEntity = this.BaseRepository().FindEntity(departmentEntity.ParentId);
                    departmentEntity.SendDeptID = curEntity.SendDeptID;
                    departmentEntity.SendDeptName = curEntity.SendDeptName;
                }
                if (departmentEntity.Nature == "承包商" && departmentEntity.ParentId == wbDeptId)
                {
                    //保存外包单位基础信息
                    OutsourcingprojectEntity OutProEntity = new OutsourcingprojectEntity();
                    OutProEntity.OUTPROJECTID = departmentEntity.DepartmentId;
                    OutProEntity.OUTSOURCINGNAME = departmentEntity.FullName;
                    OutProEntity.BLACKLISTSTATE = "0";//默认状态
                    new OutsourcingprojectService().SaveForm(OutProEntity.ID, OutProEntity);
                }
                if (string.IsNullOrWhiteSpace(departmentEntity.ParentId))
                {
                    departmentEntity.ParentId = "0";
                }
                if (departmentEntity.Nature == "集团" || departmentEntity.Nature == "省级" || departmentEntity.Nature == "厂级")
                {
                    departmentEntity.OrganizeId = departmentEntity.DepartmentId;
                }
                count = this.BaseRepository().Insert(departmentEntity);
                if (count > 0)
                {
                    dept = departmentEntity;
                    if (departmentEntity.Nature == "集团" || departmentEntity.Nature == "省级")
                    {
                        string desc = "各电厂";
                        if (departmentEntity.Nature == "集团")
                        {
                            desc = "区域子公司";
                        }
                        dept = new DepartmentEntity
                        {
                            DepartmentId = System.Guid.NewGuid().ToString(),
                            DeptCode = departmentEntity.DeptCode + "001",
                            EnCode = departmentEntity.EnCode + "001",
                            Nature = "省级",
                            Description = desc,
                            FullName = desc,
                            SortCode = 100,
                            ParentId = departmentEntity.DepartmentId,
                            OrganizeId = departmentEntity.DepartmentId,
                            CreateDate = DateTime.Now

                        };
                        this.BaseRepository().Insert(dept);
                    }
                    if (departmentEntity.Nature == "厂级")
                    {

                        dept = new DepartmentEntity
                        {
                            DepartmentId = System.Guid.NewGuid().ToString(),
                            DeptCode = departmentEntity.DeptCode + "001",
                            EnCode = departmentEntity.EnCode + "001",
                            Nature = "部门",
                            SortCode = 100,
                            Description = "外包工程承包商",
                            FullName = "外包工程承包商",
                            ParentId = departmentEntity.DepartmentId,
                            OrganizeId = departmentEntity.DepartmentId,
                            CreateDate = DateTime.Now

                        };
                        this.BaseRepository().Insert(dept);
                    }
                }
            }
            if (count > 0)
            {
                if (!string.IsNullOrWhiteSpace(departmentEntity.ToolsKey))
                {
                    string[] arr = departmentEntity.ToolsKey.Split('|');
                    string key = "";
                    if (arr.Length > 2)
                    {
                        key = arr[2];
                    }
                    BaseRepository().ExecuteBySql(string.Format("begin \r\n delete from BIS_TOOLSDEPT where deptcode='{1}';\r\n insert into BIS_TOOLSDEPT(deptid,deptcode,deptname,unitid,unitcode,unitname,keys) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}');\r\n commit;\r\n end;", dept.DepartmentId, dept.EnCode, dept.FullName, arr[0], arr[1], dept.Fax, key));
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
        /// <summary>
        /// 按规则生成编码（如cout=3,len=3,则编码则为003）
        /// </summary>
        /// <param name="count">值</param>
        /// <param name="len">长度</param>
        /// <returns></returns>
        public string GetSno(int count, int len)
        {

            int length = len - count.ToString().Length;
            length = length > 0 ? length : 0;
            string code = "";
            for (int j = 0; j < length; j++)
            {
                code += "0";
            }
            string val = "";
            if (count.ToString().Contains("E"))
            {
                val = Decimal.Parse(count.ToString(), System.Globalization.NumberStyles.Float).ToString();
            }
            else
            {
                val = count.ToString();
            }
            return code + val;
        }
        /// <summary>
        /// 根据编码获取部门编码，如果当前编码存在则重新生成编码
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        private string GetNewCode(string deptCode, string name = "encode")
        {
            int count = BaseRepository().FindObject(string.Format("select count(1) from base_department where {1}='{0}'", deptCode, name)).ToInt();
            if (count > 0)
            {
                if (deptCode.Length > 3)
                {
                    count = int.Parse(deptCode.Substring(deptCode.Length - 3));
                }
                else
                {
                    count = int.Parse(deptCode);
                }
                count++;
                string code = GetSno(count, 3);
                deptCode = deptCode.ToString().Substring(0, deptCode.ToString().Length - 3) + code;
                return GetNewCode(deptCode);
            }
            else
            {
                return deptCode;
            }
        }
        /// <summary>
        /// 生成单位编码
        /// </summary>
        /// <param name="dept"></param>
        /// <returns></returns>
        public string GetDeptCode(DepartmentEntity dept, string name = "deptcode")
        {
            string deptCode = "";
            int count = 0;
            //如果是一级节点单位
            if (string.IsNullOrWhiteSpace(dept.ParentId))
            {
                dept.ParentId = "0";
                object obj = BaseRepository().FindObject(string.Format("select max({1}) from base_department d where d.parentid='{0}' ", dept.ParentId, name));
                if (obj == null || obj == DBNull.Value)
                {
                    count = 0;
                }
                else
                {
                    if (obj.ToString().Length > 3)
                    {
                        count = int.Parse(obj.ToString().Substring(obj.ToString().Length - 3));
                    }
                    else
                    {
                        count = int.Parse(obj.ToString());
                    }
                }
                count++;
                deptCode = GetSno(count, 3);
            }
            else
            {
                DepartmentEntity parentDept = BaseRepository().FindEntity(dept.ParentId);
                string code = name == "deptcode" ? parentDept.DeptCode : parentDept.EnCode;
                object obj = BaseRepository().FindObject(string.Format("select max({1}) from base_department where parentid='{0}'", dept.ParentId, name));
                if (obj == null || obj == DBNull.Value)
                {
                    deptCode = code + "001";
                }
                else
                {
                    code = obj.ToString();
                    if (code.Length > 3)
                    {
                        count = int.Parse(code.Substring(code.Length - 3));
                    }
                    else
                    {
                        count = int.Parse(code);
                    }
                    count++;
                    code = GetSno(count, 3);
                    deptCode = obj.ToString().Substring(0, obj.ToString().Length - 3) + code;
                }
            }
            return GetNewCode(deptCode, name);
        }
        #region  根据当前机构获取对应的机构代码  机构代码 2-6-8-10  位
        /// <summary>
        /// 根据当前机构获取对应的机构代码  机构代码 2-6-8-10  位
        /// </summary>
        /// <param name="departmentEntity"></param>
        /// <returns></returns>
        public string GetDepartmentCode(DepartmentEntity departmentEntity)
        {
            string maxCode = string.Empty;

            OrganizeEntity oEntity = null;

            DepartmentEntity dEntity = null;

            string deptcode = string.Empty;

            if (departmentEntity.ParentId == "0")//选择是机构
            {
                oEntity = new OrganizeService().BaseRepository().FindEntity(departmentEntity.OrganizeId);  //获取父对象(机构)

                deptcode = oEntity.EnCode;
            }
            else //选择的是部门
            {
                dEntity = this.BaseRepository().FindEntity(departmentEntity.ParentId);//获取部门父对象

                deptcode = dEntity.EnCode;
            }

            //查询是否存在平级部门
            IEnumerable<DepartmentEntity> maxObj = this.BaseRepository().FindList(string.Format("select t.*  from BASE_DEPARTMENT t where t.encode  like '{0}%' and t.encode !='{0}' and t.parentid='" + departmentEntity.ParentId + "' and  encode is not null order by encode desc", deptcode));

            //确定是否存在上级部门,非部门根节点
            if (departmentEntity.ParentId != "0")
            {
                //存在，则取编码最大的那一个
                if (maxObj.Count() > 0)
                {
                    maxCode = maxObj.FirstOrDefault().EnCode;  //获取最大的Code 
                    if (!string.IsNullOrEmpty(maxCode))
                    {
                        maxCode = new OrganizeService().QueryOrganizeCodeByCondition(maxCode);
                    }
                }
                else
                {
                    DepartmentEntity parentEntity = this.BaseRepository().FindEntity(departmentEntity.ParentId);  //获取父对象

                    maxCode = parentEntity.EnCode + "001";  //固定值,非可变
                }
            }
            else  //部门根节点的操作
            {
                //do somethings 
                if (maxObj.Count() > 0)
                {
                    maxCode = maxObj.FirstOrDefault().EnCode;  //获取最大的Code 

                    if (!string.IsNullOrEmpty(maxCode))
                    {
                        maxCode = new OrganizeService().QueryOrganizeCodeByCondition(maxCode);
                    }
                    else
                    {
                        maxCode = deptcode + "001";
                    }
                }
                else
                {
                    maxCode = deptcode + "001";  //固定值,非可变,加机构编码，组合新编码
                }
            }
            //先判断当前编码是否存在于数据库表中
            return maxCode;
        }

        /// <summary>
        /// 批量导入部门的时候部门code
        /// </summary>
        /// <param name="parentName">上级部门名称</param>
        /// <param name="orgId">当前用户所属机构id</param>
        /// <returns></returns>
        public List<string> GetImportDeptCode(string parentName, string orgId)
        {
            List<string> list = new List<string>();
            //DataTable orgData = this.BaseRepository().FindTable("select departmentid from base_organize where fullname='" + parentName + "' and organizeid='" + orgId + "'");
            //string deptCode = string.Empty;
            //string maxCode = string.Empty;
            //if (orgData.Rows.Count > 0)
            //{
            //    //相当于现在机构导入
            //    list.Add(orgId);//parentid
            //}
            //else
            //{
            DataTable deptData = this.BaseRepository().FindTable("select departmentid from base_department where fullname='" + parentName + "' and organizeid='" + orgId + "'");
            if (deptData.Rows.Count > 0)
            {
                //相当于选择部门导入
                list.Add(deptData.Rows[0]["departmentid"].ToString());//parentid
            }
            else
            {
                //既不是选择的机构也不是选择的部门，那么导入失败
                list.Add("-1");
            }

            // }
            return list;
        }

        #endregion

        #region 根据当前部门判断是否存在上级部门
        /// <summary>
        /// 根据当前部门判断是否存在上级部门
        /// </summary>
        /// <param name="departID"></param>
        /// <returns></returns>
        public bool IsExistSuperior(string departID)
        {
            bool isSuccess = false;

            string sql = string.Format(@"select * from base_department where departmentid  = 
                          (select parentid from base_department where departmentid ='{0}')", departID);

            var list = this.BaseRepository().FindList(sql);

            if (list.Count() > 0)
            {
                isSuccess = true;
            }

            return isSuccess;
        }
        #endregion
        /// <summary>
        /// 同步部门信息到培训平台
        /// </summary>
        /// <param name="dept"></param>
        /// <returns></returns>
        public string SyncDept(DepartmentEntity dept, string keyValue)
        {
            string fileName = "Train_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            try
            {
                string result = "";
                DepartmentEntity parentDept = BaseRepository().FindEntity(dept.ParentId);
                if (parentDept != null)
                {
                    DataTable dt = BaseRepository().FindTable(string.Format("select px_deptid,px_deptcode from xss_dept where deptid='{0}'", dept.ParentId));
                    if (dt.Rows.Count > 0)
                    {
                        List<object> list = new List<object>();
                        list.Add(new
                        {
                            Id = dept.DepartmentId,
                            deptName = dept.FullName,
                            parentCode = dt.Rows[0][1].ToString(),
                            parentID = dt.Rows[0][0].ToString()
                        });
                        WebClient wc = new WebClient();
                        wc.Credentials = CredentialCache.DefaultCredentials;
                        //wc.Headers.Add("Content-Type", "text/json; charset=utf-8");
                        wc.Encoding = Encoding.GetEncoding("GB2312");
                        System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                        byte[] bytes = null;
                        //发送请求到web api并获取返回值，默认为post方式
                        string url = new DataItemDetailService().GetItemValue("TrainServiceUrl");
                        string json = Newtonsoft.Json.JsonConvert.SerializeObject(new { business = "SavedeptInfro", DpetInfo = list });
                        nc.Add("json", json);
                        bytes = wc.UploadValues(new Uri(url), "POST", nc);
                        result = Encoding.UTF8.GetString(bytes);
                        System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ">执行动作：同步部门到培训平台,返回信息：" + result + "\r\n");
                        dynamic dy = Newtonsoft.Json.JsonConvert.DeserializeObject<ExpandoObject>(result);

                        if (string.IsNullOrEmpty(keyValue) && dy.meta.success)
                        {
                            dy = dy.retDept[0];
                            if (BaseRepository().ExecuteBySql(string.Format("insert into xss_dept(deptid,deptcode,deptname,px_parentcode,px_parentid,px_deptid,px_deptcode,parentid) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", dept.DepartmentId, dept.EnCode, dept.FullName, dy.parentCode, dy.parentID, dy.Id, dy.deptCode, dept.ParentId)) > 0)
                            {
                                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " >新增部门关联信息到双控平台成功，部门信息：" + json + "\r\n");
                            }
                            else
                            {
                                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " >新增部门关联信息到双控平台失败,部门信息：" + json + " \r\n");
                            }

                        }
                        else
                        {
                            System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ">新增部门关联信息到双控平台失败,部门信息：" + result + " \r\n");
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ">执行结果：同步部门信息失败,异常信息：" + ex.Message + "\r\n");
                return ex.Message;
            }

        }
        /// <summary>
        /// 初始化培训平台部门和用户信息并保存到双控平台
        /// </summary>
        /// <param name="deptId">部门Id</param>
        /// <param name="deptCode">部门编码</param>
        /// <param name="jsonDepts">来自培训平台的部门信息</param>
        /// <param name="jsonUsers">来自培训平台的用户信息</param>
        /// <returns></returns>
        public string SyncDeptForTrain(string deptId, string deptCode, string jsonDepts, string jsonUsers, string deptKey = "")
        {
            string fileName = "Train_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            try
            {

                WebClient wc = new WebClient();
                wc.Credentials = CredentialCache.DefaultCredentials;
                //wc.Headers.Add("Content-Type", "text/json; charset=utf-8");
                wc.Encoding = Encoding.GetEncoding("GB2312");
                System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                byte[] bytes = null;
                //发送请求到web api并获取返回值，默认为post方式
                string url = new DataItemDetailService().GetItemValue("TrainServiceUrl");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(jsonDepts);
                List<object> listDepts = dy.Deptdata;
                List<object> list = new List<object>();
                DataTable dt = null;
                if (!string.IsNullOrEmpty(deptKey))
                {
                    string[] arr = deptKey.Split('|');
                    if (arr.Length > 1)
                    {
                        dt = BaseRepository().FindTable(string.Format("select departmentid,fullname,'" + arr[1] + "' as encode,'" + arr[0] + "' as parentid from  BASE_DEPARTMENT where parentid='{0}' and description is null", deptId));
                    }
                }
                string sql = string.Format("begin\r\n delete from xss_dept where deptcode like '{0}%';\r\n insert into xss_dept(deptid,deptcode,deptname,parentid) select departmentid,encode,a.fullname,parentid from base_department a where nature!='承包商' and  a.description is null and encode like '{0}%';\r\ncommit;\r\n end; ", deptCode);
                BaseRepository().ExecuteBySql(sql);

                StringBuilder sb = new StringBuilder("begin\r\n");
                foreach (object obj in listDepts)
                {
                    dy = obj;
                    sb.AppendFormat("update xss_dept set px_deptid='{0}',px_deptcode='{1}',px_parentid='{2}',px_parentcode='{3}' where deptname='{4}';\r\n", dy.PARTYID, dy.DEPARTCODE, dy.PARENTID, dy.PARENTCODE, dy.DEPARTNAME);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow[] rows = dt.Select("fullname='" + dy.DEPARTNAME + "'");
                        if (rows.Length > 0)
                        {
                            dt.Rows.Remove(rows[0]);
                        }
                    }
                }
                sb.Append("end\r\ncommit;\r\n");
                if (listDepts.Count > 0)
                {
                    BaseRepository().ExecuteBySql(sb.ToString());
                }
                if (dt.Rows.Count > 0)
                {
                    nc = new System.Collections.Specialized.NameValueCollection();
                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(new { business = "SavedeptInfro", DpetInfo = dt });
                    json = json.Replace("departmentid", "Id").Replace("fullname", "deptName").Replace("encode", "parentCode").Replace("parentid", "parentID");
                    nc.Add("json", json);
                    bytes = wc.UploadValues(new Uri(url), "POST", nc);
                    string result = Encoding.UTF8.GetString(bytes);
                    System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ">同步部门信息到培训平台,同步信息：" + json + ",返回信息：" + result + "\r\n");
                    dy = JsonConvert.DeserializeObject<ExpandoObject>(result);
                    if (result.Contains("\"Code\":"))
                    {
                        if (dy.Code == -1)
                        {
                            return "调用培训平台方法方法SavedeptInfro发生异常，错误信息：" + result;
                        }
                    }
                    listDepts = dy.retDept;
                    sb = new StringBuilder("begin\r\n");
                    foreach (object obj in listDepts)
                    {
                        dy = obj;
                        sb.AppendFormat("update xss_dept set px_deptcode='{0}',px_parentid='{1}',px_parentcode='{2}' where deptname='{3}';\r\n", dy.deptCode, dy.parentID, dy.parentCode, dy.deptName);

                    }
                    if (listDepts.Count > 0)
                    {
                        sb.Append("end\r\ncommit;\r\n");
                        BaseRepository().ExecuteBySql(sb.ToString());
                    }
                    System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ">同步部门信息到培训平台,同步信息：" + json + ",返回信息：" + result + "\r\n");
                }
                sql = string.Format("begin\r\n delete from xss_user where deptcode like '{0}%';\r\n insert into xss_user(userid,username,useraccount,IdCard,deptid,deptcode,pwd,status) select userid,realname,account,b.identifyid,b.departmentid,b.departmentcode,b.password,b.enabledmark from base_user b left join base_department d on b.DEPARTMENTID=d.DEPARTMENTID where nature!='承包商' and  d.description is null and d.encode like '{0}%';\r\n commit;\r\n end;", deptCode);
                BaseRepository().ExecuteBySql(sql);

                var dept = GetEntity(deptId);
                dt = BaseRepository().FindTable(string.Format("select userid,username,useraccount,'123456' pwd,idcard,b.px_deptid,'0' role,'一般人员' userkind from XSS_USER a left join xss_dept b on a.deptid=b.deptid where a.px_deptid is null and b.px_deptid is not null and a.deptcode like '{0}%'", dept.EnCode));
                dy = JsonConvert.DeserializeObject<ExpandoObject>(jsonUsers);
                list = dy.Userdata;
                if (list.Count > 0)
                {
                    sb.Clear();
                    sb.Append("begin\r\n");
                    foreach (object obj in list)
                    {
                        dy = obj;
                        sb.AppendFormat("update xss_user set px_uid='{0}',px_account='{1}',px_deptid='{2}' where idcard='{3}';\r\n", dy.PARTYID, dy.USERACCOUNT, dy.DEPARTID, dy.IDCARD);
                        DataRow[] rows = dt.Select("useraccount='" + dy.USERACCOUNT + "'");
                        if (dt.Rows.Count > 0)
                        {
                            if (rows.Length > 0)
                            {
                                dt.Rows.Remove(rows[0]);
                            }
                        }
                    }
                    sb.Append("end\r\ncommit;\r\n");
                    if (list.Count > 0)
                    {
                        BaseRepository().ExecuteBySql(sb.ToString());
                    }

                }

                if (dt.Rows.Count > 0)
                {
                    nc = new System.Collections.Specialized.NameValueCollection();
                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(new { business = "saverUser", UserInfo = dt });
                    json = json.Replace("userid", "Id").Replace("username", "userName").Replace("useraccount", "userAccount").Replace("pwd", "pwd").Replace("idcard", "IdCard").Replace("px_deptid", "departid").Replace("role", "role").Replace("userkind", "userkind");
                    nc.Add("json", json);
                    bytes = wc.UploadValues(new Uri(url), "POST", nc);
                    string result = Encoding.UTF8.GetString(bytes);
                    System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ">同步用户信息到培训平台,同步信息：" + json + ",返回信息：" + result + "\r\n");
                    dy = JsonConvert.DeserializeObject<ExpandoObject>(result);
                    if (result.Contains("\"Code\":"))
                    {
                        if (dy.Code == -1)
                        {
                            return "调用培训平台方法方法saverUser发生异常，错误信息：" + result;
                        }
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// 根据sql获取DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string sql)
        {
            return BaseRepository().FindTable(sql);
        }

        public DataTable GetDataTableByParams(string sql, DbParameter[] parameters)
        {
            return BaseRepository().FindTable(sql, parameters);
        }
        /// <summary>
        /// 获取DataTable
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="connStr">连接串</param>
        /// <param name="DbType">1:Oracle,2:SqlServer,3:MySql</param>
        /// <returns></returns>
        public DataTable GetDataTable(string sql, string connStr, int DbType = 1)
        {
            DatabaseType dataType = DatabaseType.Oracle;
            if (DbType == 2)
            {
                dataType = DatabaseType.SqlServer;
            }
            if (DbType == 3)
            {
                dataType = DatabaseType.MySql;
            }
            return DbFactory.DapperBase(connStr, dataType).FindTable(sql);
        }
        /// <summary>
        /// 执行sql操作
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="connStr">连接串key</param>
        /// <param name="DbType">1:Oracle,2:SqlServer,3:MySql</param>
        /// <returns></returns>
        public int ExecuteSql(string sql, string connStr, int DbType = 1)
        {
            DatabaseType dataType = DatabaseType.Oracle;
            if (DbType == 2)
            {
                dataType = DatabaseType.SqlServer;
            }
            if (DbType == 3)
            {
                dataType = DatabaseType.MySql;
            }
            return DbFactory.DapperBase(connStr, dataType).ExecuteBySql(sql);
        }
        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteSql(string sql)
        {
            return BaseRepository().ExecuteBySql(sql);
        }

        public List<DepartmentEntity> GetSubDepartments(string id, string category)
        {
            var list = default(List<string>);

            if (!string.IsNullOrEmpty(category))
                list = category.Split(',').ToList();

            var db = new RepositoryFactory().BaseRepository();

            var current = from q in db.IQueryable<DepartmentEntity>()
                          where q.DepartmentId == id
                          select q;

            var subquery = from q1 in current
                           join q2 in db.IQueryable<DepartmentEntity>() on q1.DepartmentId equals q2.ParentId
                           select q2;

            while (subquery.Count() > 0)
            {
                current = current.Concat(subquery);
                subquery = from q1 in subquery
                           join q2 in db.IQueryable<DepartmentEntity>() on q1.DepartmentId equals q2.ParentId
                           select q2;
            }

            if (list != null && list.Count > 0) current = current.Where(x => list.Contains(x.Nature));

            return current.OrderBy(x => x.EnCode).ToList();
        }

        public DepartmentEntity GetCompany(string deptId)
        {
            var db = new RepositoryFactory().BaseRepository();

            var query = from q in db.IQueryable<DepartmentEntity>()
                        where q.DepartmentId == deptId
                        select q;

            var dept = query.FirstOrDefault();
            while (dept != null && dept.Nature != "厂级" && dept.Nature != "省级" && dept.Nature != "集团")
            {
                query = from q1 in db.IQueryable<DepartmentEntity>()
                        join q2 in query on q1.DepartmentId equals q2.ParentId
                        select q1;
                dept = query.FirstOrDefault();
            }

            if (dept == null) dept = (from q in db.IQueryable<DepartmentEntity>()
                                      where q.ParentId == "0"
                                      select q).FirstOrDefault();

            return dept;
        }

        /// <summary>
        /// 同步外包人员到科技MIS系统(国电荥阳)
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public int SyncUsers(List<AptitudeinvestigatepeopleEntity> list)
        {
            try
            {
                ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();

                fileMap.ExeConfigFilename = System.Web.HttpContext.Current.Server.MapPath(@"~/XmlConfig/remote_database.config");

                Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

                AppSettingsSection appsection = (AppSettingsSection)config.GetSection("appSettings");

                string connStr = appsection.Settings["RemoteBaseDb"].Value;
                int count = 0;
                if (list.Count > 0)
                {
                    string deptId = list[0].OUTPROJECTID;
                    DataTable dtDept = GetDataTable(string.Format("select d.encode,d.fullname from base_department d where d.departmentid='{0}'", deptId));
                    if (dtDept.Rows.Count > 0)
                    {

                        string deptCode = dtDept.Rows[0][0].ToString();
                        string deptName = dtDept.Rows[0][1].ToString();

                        dtDept = GetDataTable(string.Format("select code from SAFETBRYDASORT where name='{0}'", deptName.Trim()), connStr);
                        string sql = "";

                        if (dtDept.Rows.Count > 0)
                        {
                            deptCode = dtDept.Rows[0][0].ToString();
                            count = 1;
                        }
                        else
                        {
                            sql = string.Format("insert into SAFETBRYDASORT(id,OPERDATE,OPERUSER,CREATEUSER,CREATEDATE,CREATEUSERID,CREATEUSERDEPTID,CREATEUSERDEPTCODE,CODE,Name,PARENTID,Rydasortid) values('{0}',sysdate,'{1}','{2}',sysdate,'{3}','{4}','{5}','{6}','{7}','{8}','{9}')", Guid.NewGuid().ToString(), "admin", "admin", "-2", "-1", "00", deptCode, deptName, "1f004bb1-087e-42ea-afa7-023421481389", deptId);
                            count = ExecuteSql(sql, connStr);
                        }
                        if (count > 0)
                        {
                            StringBuilder sb = new StringBuilder("begin\r\n");
                            foreach (AptitudeinvestigatepeopleEntity person in list)
                            {
                                string stype = person.SpecialtyType;
                                if (!string.IsNullOrWhiteSpace(person.SpecialtyType) && person.SpecialtyType != "null")
                                {
                                    DataTable dtItems = GetDataTable(string.Format("select itemname from base_dataitemdetail a where a.itemid=(select itemid from base_dataitem where itemcode='SpecialtyType') and a.itemvalue in('{0}')", person.SpecialtyType.Replace(",", "','")));
                                    if (dtItems.Rows.Count > 0)
                                    {
                                        string[] arr = dtItems.AsEnumerable().Select(t => t.Field<string>("itemname")).ToArray();
                                        stype = string.Join(",", arr);
                                    }
                                }
                                else
                                {
                                    stype = "";
                                }
                                sql = string.Format("select id from SAFETBRYDAINFO where DeptCode='{0}' and IdCardCode='{1}'", deptCode, person.IDENTIFYID);
                                DataTable dtUser = GetDataTable(sql, connStr);
                                if (dtUser.Rows.Count > 0)
                                {
                                    sb.AppendFormat("update SAFETBRYDAINFO set Name='{0}',Sex='{1}',IdCardCode='{2}',DeptName='{3}',DeptCode='{4}',WorkType='{5}',EduExper='{6}',SpecialtyName='{7}',PhoneNum='{8}',BirthDate=to_date('{9}','yyyy-mm-dd hh24:mi:ss'),CultureDegree='{10}',UserType='{11}',HealthCase='{12}' where id='{13}';\r\n", person.REALNAME, person.GENDER, person.IDENTIFYID, deptName, deptCode, person.WORKOFTYPE, person.DEGREESID, stype, person.MOBILE, person.BIRTHDAY == null ? "" : person.BIRTHDAY.Value.ToString("yyyy-MM-dd HH:mm:ss"), person.DEGREESID, person.USERTYPE, person.STATEOFHEALTH, dtUser.Rows[0][0].ToString());
                                }
                                else
                                {
                                    sb.AppendFormat("insert into SAFETBRYDAINFO(id,OPERDATE,OPERUSER,CREATEUSER,CREATEDATE,CREATEUSERID,CREATEUSERDEPTID,CREATEUSERDEPTCODE,Name,Sex,IdCardCode,DeptName,DeptCode,WorkType,EduExper,SpecialtyName,PhoneNum,BirthDate,CultureDegree,UserType,HealthCase) values('{0}',sysdate,'{1}','{2}',sysdate,'{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}',to_date('{15}','yyyy-mm-dd hh24:mi:ss'),'{16}','{17}','{18}');\r\n", Guid.NewGuid().ToString(), "admin", "admin", "-2", "-1", "00", person.REALNAME, person.GENDER, person.IDENTIFYID, deptName, deptCode, person.WORKOFTYPE, person.DEGREESID, stype, person.MOBILE, person.BIRTHDAY == null ? "" : person.BIRTHDAY.Value.ToString("yyyy-MM-dd HH:mm:ss"), person.DEGREESID, person.USERTYPE, person.STATEOFHEALTH);
                                }
                            }
                            sb.Append("commit;\r\n end;");
                            count = ExecuteSql(sb.ToString(), connStr);
                        }
                    }
                }
                return count;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }
    }
}
