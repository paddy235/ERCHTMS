using ERCHTMS.Entity.BaseManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.Service.BaseManage;
using System;
using System.Linq;
using System.Collections.Generic;
using BSFramework.Cache.Factory;
using System.Data;
using BSFramework.Util.WebControl;
using ERCHTMS.Entity.OutsourcingProject;
using System.Text;
using System.Data.Common;

namespace ERCHTMS.Busines.BaseManage
{
    /// <summary>
    /// 描 述：部门管理
    /// </summary>
    public class DepartmentBLL
    {
        private IDepartmentService service = new DepartmentService();
        /// <summary>
        /// 缓存key
        /// </summary>
        public string cacheKey = BSFramework.Util.Config.GetValue("SoftName") + "_DepartmentCache";

        #region 获取数据
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// 验证部门名称是否重复
        /// </summary>
        /// <param name="departmentName">部门名称</param>
        /// <returns></returns>
        public bool ExistDeptJugement(string departmentName, string orgId = "")
        {
            return service.ExistDeptJugement(departmentName, orgId);
        }

        #region 获取上一个节点
        /// <summary>
        /// 获取上一个节点
        /// </summary>
        /// <param name="parentid"></param>
        /// <param name="nature"></param>
        /// <returns></returns>
        public DepartmentEntity GetParentDeptBySpecialArgs(string parentid, string nature)
        {
            return service.GetParentDeptBySpecialArgs(parentid, nature);
        }
        #endregion

        /// <summary>
        /// 部门列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DepartmentEntity> GetList()
        {
            return service.GetList();
        }
        /// <summary>
        /// 根据部门Id获取下属部门列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DepartmentEntity> GetList(string parentId)
        {
            return service.GetList().ToList().Where(t => t.ParentId == parentId || t.OrganizeId == parentId);
        }
        /// <summary>
        /// 部门实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public DepartmentEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        public DepartmentEntity GetEntityByCode(string keyValue)
        {
            return service.GetEntityByCode(keyValue);
        }
        /// <summary>
        /// 判断是否存在上级
        /// </summary>
        /// <param name="departID"></param>
        /// <returns></returns>
        public bool IsExistSuperior(string departID)
        {
            return service.IsExistSuperior(departID);
        }

        /// <summary>
        /// 非黑名单的承包商列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DepartmentEntity> GetNotBlackList()
        {
            return service.GetNotBlackList();

        }
        /// <summary>
        /// 获取指定部门的所属机构信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public DepartmentEntity GetDeptOrgInfo(string deptId)
        {
            return service.GetDeptOrgInfo(deptId);
        }
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<DepartmentEntity> GetDepts(string parentid)
        {
            return service.GetDepts(parentid);
        }

        /// <summary>
        /// 获取机构厂级部门
        /// <param name="orgid">当前机构</param>
        /// <param name="orgid">是否厂级 0-否 1-是</param>
        /// </summary>
        public IEnumerable<DepartmentEntity> GetDepts(string orgid, int isorg)
        {
            return service.GetDepts(orgid,isorg);
        }
        /// <summary>
        /// 获取承包商信息
        /// </summary>
        /// <param name="orgCode"></param>
        /// <returns></returns>
        public DataTable GetContractDepts(string orgCode)
        {
            return service.GetContractDepts(orgCode);
        }
        /// <summary>
        /// 根据部门名称和机构Id获取部门编码
        /// </summary>
        /// <param name="deptName">部门名称</param>
        /// <param name="orgId">机构Id</param>
        /// <returns></returns>
        public string GetDeptCode(string deptName, string orgId)
        {
            return service.GetDeptCode(deptName, orgId);
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
            return service.GetPostId(postName, orgId, deptCode);
        }
        /// <summary>
        /// 获取当前用户所属的省级
        /// </summary>
        /// <param name="user">当前用户</param>
        /// <returns></returns>
        public DepartmentEntity GetUserCompany(ERCHTMS.Code.Operator user)
        {
            return service.GetUserCompany(user);
        }
        /// <summary>
        /// 根据区域名称和机构Id获取区域Id
        /// </summary>
        /// <param name="areaName">区域名称</param>
        /// <param name="orgId">机构Id</param>
        /// <returns></returns>
        public string GetAreaId(string areaName, string orgId)
        {
            return service.GetAreaId(areaName, orgId);
        }
        /// <summary>
        /// 获取省级下属管辖的所有电厂
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable GetAllFactory(ERCHTMS.Code.Operator user)
        {
            return service.GetAllFactory(user);
        }
        public DataTable GetDataTable(string sql)
        {
            return service.GetDataTable(sql);

        }
        public DataTable GetDataTableByParams(string sql, DbParameter[] parameters)
        {
            try
            {
                return service.GetDataTableByParams(sql, parameters);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 获取DataTable
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="connStr">连接串key</param>
        /// <param name="DbType">1:Oracle,2:SqlServer,3:MySql</param>
        /// <returns></returns>
        public DataTable GetDataTable(string sql, string connStr, int DbType)
        {
            return service.GetDataTable(sql, connStr, DbType);
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
            return service.ExecuteSql(sql, connStr, DbType);
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
            return service.ExistEnCode(enCode, keyValue);
        }
        /// <summary>
        /// 部门名称不能重复
        /// </summary>
        /// <param name="fullName">名称</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool ExistFullName(string fullName, string keyValue)
        {
            return service.ExistFullName(fullName, keyValue);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="keyValue">主键</param>
        public bool RemoveForm(string keyValue, List<UserEntity> ue)
        {
            try
            {
                return service.RemoveForm(keyValue, ue);
                
            }
            catch (Exception)
            {
                return false;

            }
        }
        /// <summary>
        /// 保存部门表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="departmentEntity">部门实体</param>
        /// <returns></returns>
        public bool SaveForm(string keyValue, DepartmentEntity departmentEntity)
        {
            try
            {
                return service.SaveForm(keyValue, departmentEntity);
                
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 批量导入部门的时候部门code
        /// </summary>
        /// <param name="parentName">上级部门名称</param>
        /// <param name="orgId">当前用户所属机构id</param>
        /// <returns></returns>
        public List<string> GetImportDeptCode(string parentName, string orgId)
        {
            try
            {
                return service.GetImportDeptCode(parentName, orgId);
            }
            catch (Exception)
            {
                throw;
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
            return service.SyncDeptForTrain(deptId, deptCode, jsonDepts, jsonUsers, deptKey);
        }
        /// <summary>
        /// 同步部门信息到培训平台
        /// </summary>
        /// <param name="dept"></param>
        /// <returns></returns>
        public string SyncDept(DepartmentEntity dept, string keyValue)
        {
            return service.SyncDept(dept, keyValue);

        }
        public int ExecuteSql(string sql)
        {
            return service.ExecuteSql(sql);
        }
        #endregion

        public List<DepartmentEntity> GetSubDepartments(string id, string category)
        {
            return service.GetSubDepartments(id, category);
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
                
                int count = 0;
                if (list.Count>0)
                {
                    string deptId = list[0].OUTPROJECTID;
                    DataTable dtDept = GetDataTable(string.Format("select d.encode,d.fullname from base_department d where d.departmentid='{0}'", deptId));
                    if (dtDept.Rows.Count > 0)
                    {

                        string deptCode = dtDept.Rows[0][0].ToString();
                        string deptName = dtDept.Rows[0][1].ToString();

                        dtDept = GetDataTable(string.Format("select code from SAFETBRYDASORT where name='{0}'", deptName.Trim()));
                        string sql = "";

                        if (dtDept.Rows.Count > 0)
                        {
                            deptCode = dtDept.Rows[0][0].ToString();
                            count = 1;
                        }
                        else
                        {
                            sql = string.Format("insert into SAFETBRYDASORT(id,OPERDATE,OPERUSER,CREATEUSER,CREATEDATE,CREATEUSERID,CREATEUSERDEPTID,CREATEUSERDEPTCODE,CODE,Name,PARENTID,Rydasortid) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')", Guid.NewGuid().ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "admin", "admin", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "-2", "-1", "00", deptCode, deptName, "1f004bb1-087e-42ea-afa7-023421481389", deptId);
                            count = ExecuteSql(sql, "BaseDb1");
                        }
                        if (count > 0)
                        {
                            StringBuilder sb = new StringBuilder("begin\r\n");
                            foreach(AptitudeinvestigatepeopleEntity person in list)
                            {
                                sb.AppendFormat("insert into SAFETBRYDAINFO(id,OPERDATE,OPERUSER,CREATEUSER,CREATEDATE,CREATEUSERID,CREATEUSERDEPTID,CREATEUSERDEPTCODE,Name,Sex,IdCardCode,DeptName,DeptCode,WorkType,EduExper,SpecialtyName,PhoneNum,BirthDate,CultureDegree,UserType,HealthCase) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}');\r\n", Guid.NewGuid().ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "admin", "admin", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "-2", "-1", "00", person.REALNAME, person.GENDER, person.IDENTIFYID, deptName, deptCode, person.WORKOFTYPE, person.DEGREESID, person.SpecialtyType, person.MOBILE, person.BIRTHDAY, person.DEGREESID, person.STATEOFHEALTH, person.USERTYPE);
                            }
                            sb.Append("commit;\r\n end;");
                            count = ExecuteSql(sb.ToString(), "BaseDb1");
                        }
                   }
                }
                return count;
            }
            catch(Exception ex)
            {
                return 0;
            }
            
        }

        public DepartmentEntity GetCompany(string deptId)
        {
            return service.GetCompany(deptId);
        }


    }
}
